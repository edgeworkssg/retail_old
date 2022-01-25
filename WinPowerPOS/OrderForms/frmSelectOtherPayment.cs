using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using POSDevices;   
using WinPowerPOS.Reports;
using PowerPOS.Container;
using System.Configuration;
using Features = PowerPOS.Feature;
namespace WinPowerPOS.OrderForms
{
    public partial class frmSelectOtherPayment : Form
    {
        public POSController pos;
        
        public bool isSuccessful;
        public decimal amount;
        string status;
        public bool isPartialPayment;
        public string PaymentTypeSelected;
        public string membershipNo;
        public string[] packageList;
        public DataTable CurrentAmounts;
        public bool PrintReceipt = true;
        public BackgroundWorker syncSalesThread;
        public BackgroundWorker ParentSyncPointsThread;
        

        public frmSelectOtherPayment()
        {
            InitializeComponent();
            isPartialPayment = false;
//            LoadPaymentTypeLabels();
        }
        private const int TOTAL_BUTTONS = 6;
        private void LoadPaymentTypeLabels()
        {
            for (int i = 0; i < CurrentAmounts.Rows.Count; i++)
            {
                    Button b = new Button();
                    b.Width = 200;
                    b.Height = 100;
                    b.BackColor = System.Drawing.Color.Orange;
                    b.Font = new Font("Verdana", 9, FontStyle.Bold);
                    Item it = new Item(CurrentAmounts.Rows[i][1].ToString());
                    decimal amt = pos.CalculateTotalPaid_ByPointsByName(CurrentAmounts.Rows[i][1].ToString(), out status);
                    b.Text = it.ItemName + ", Available : $" + (Convert.ToDecimal(CurrentAmounts.Rows[i][2].ToString()) - amt).ToString("N2");
                    b.Tag = CurrentAmounts.Rows[i][1].ToString();

                    b.Click += delegate
                    {
                        btnMakePayment_Click(b, new EventArgs());
                    };
                    flowLayoutPanel1.Controls.Add(b);
            }
        }
        
        private void btnMakePayment_Click(object sender, EventArgs e)
        {
            if (isPartialPayment)
            {
                isSuccessful = true;
                PaymentTypeSelected = ((Button)sender).Tag.ToString();
                this.Close();
            }
            else
            {
                if (handlePayment(((Button)sender).Tag.ToString()))
                {
                    //GenericReport.NewPrint.A5Controller Printer = new GenericReport.NewPrint.A5Controller();
                    //Printer.PrintInvoice(pos);

                    //ask for signature here
                    #region Signature
                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Signature.IsAvailable), false))
                    {
                        //pop out whether want signature or not
                        frmCustomMessageBox myfrm = new frmCustomMessageBox
                            ("Add Signature", "Do you want to add signature to this transaction?");
                        DialogResult DR = myfrm.ShowDialog();

                        if (myfrm.choice == "yes")
                        {
                            myfrm.Dispose();

                            bool isUsingStandard = false;
                            string signatureDevice = AppSetting.GetSetting(AppSetting.SettingsName.Signature.SignatureDevice);
                            if (string.IsNullOrEmpty(signatureDevice))
                                signatureDevice = AppSetting.SettingsName.Signature.STANDARD;
                            isUsingStandard = signatureDevice.ToUpper().Equals(AppSetting.SettingsName.Signature.STANDARD.ToUpper());

                            if (signatureDevice.ToUpper().Equals(AppSetting.SettingsName.Signature.WACOM.ToUpper()))
                            {
                                wgssSTU.UsbDevices usbDevices = new wgssSTU.UsbDevices();

                                if (usbDevices.Count == 0)
                                {
                                    isUsingStandard = true;
                                    MessageBox.Show("There is no STU device attached, using standard signature form");
                                }
                                else
                                {
                                    try
                                    {
                                        wgssSTU.IUsbDevice usbDevice = usbDevices[0]; // select a device

                                        frmSignatureWacom demo = new frmSignatureWacom(usbDevice, pos.GetUnsavedRefNo());
                                        demo.ShowDialog();
                                        List<wgssSTU.IPenData> penData = demo.getPenData();
                                        if (penData != null)
                                        {
                                            // process penData here!

                                            wgssSTU.IInformation information = demo.getInformation();
                                            wgssSTU.ICapability capability = demo.getCapability();
                                        }
                                        demo.Dispose();
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show("An error occurred, please contact admin");
                                        Logger.writeLog(ex);
                                    }
                                }
                            }

                            if (isUsingStandard)
                            {
                                //asking for Signature
                                frmSignature f = new frmSignature(pos.GetUnsavedRefNo());
                                f.ShowDialog();
                                if (f.IsSuccessful)
                                {
                                    f.Dispose();
                                }
                            }
                        }
                        else if (myfrm.choice == "no" || DR == DialogResult.Cancel)
                            myfrm.Dispose();
                    }
                    #endregion

                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSales), false))
                        if (!syncSalesThread.IsBusy)
                            syncSalesThread.RunWorkerAsync();

                    tryDownloadPoints();

                    //print receipt
                    if (PrintReceipt)
                    {
                        POSDeviceController.PrintAHAVATransactionReceipt(pos, 0, false,
                            (ReceiptSizes)Enum.ToObject(typeof(ReceiptSizes),
                            PrintSettingInfo.receiptSetting.PaperSize.Value),
                            PrintSettingInfo.receiptSetting.NumOfCopies.Value);
                    }
                    isSuccessful = true;
                    this.Close();
                }
            }
        }
        
        /// <remarks>
        /// Do not handle Multiple Payment
        /// </remarks>
        private bool handlePayment(string paymentType)
        {
            try
            {
                decimal change;
                string status = "";

                #region
                if (paymentType == "欠款余额")
                {
                    paymentType = "Installment";
                }
                #endregion

                #region *) Validation: Installment payment must be attached to a member
                if (paymentType.ToLower() == "installment"
                    && (!pos.MembershipApplied() || (pos.CurrentMember != null && pos.CurrentMember.MembershipNo.ToLower() == "walk-in")))
                    throw new Exception("(warning)Please assign a member to use installments");
                #endregion

                #region *) Initialization: Clear/Delete all PaymentList in ReceiptDetails
                pos.DeleteAllReceiptLinePayment();
                #endregion

                #region *) Warning: Notice user if there is extra charge
                decimal ExtraChargeTotalAmount = pos.CheckExtraChargeAmount(paymentType, amount);

                if (ExtraChargeTotalAmount != 0)
                {
                    //DialogResult DR = MessageBox.Show(
                    //    "There will be extra charge applicable of " + ExtraChargeTotalAmount.ToString("N2") + ". Do you want to continue?"
                    //    , "Extra Charge Applicable", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                    //if (DR == DialogResult.Cancel) return false;
                    frmExtraCharge frmExt = new frmExtraCharge();
                    frmExt.totalAmount = amount;
                    frmExt.extraCharge = ExtraChargeTotalAmount;
                    frmExt.totalAmountAfterCharge = amount + ExtraChargeTotalAmount;
                    frmExt.ShowDialog();

                    if (!frmExt.isConfirmed) return false;
                }
                #endregion

                #region *) Core: Add PaymentList in ReceiptDetails
                SortedList<string, decimal> AddedPoints = new SortedList<string, decimal>();
                #region *) Disabled: Enabled & review the following if you want to use the "newly bought" points
                //foreach (OrderDet oneOrderDet in pos.myOrderDet)
                //{
                //    Item myOneItem = oneOrderDet.Item;
                //    if (myOneItem.PointRedeemMode == Item.PointMode.Dollar)
                //    {
                //        if (AddedPoints.ContainsKey(myOneItem.ItemNo))
                //            AddedPoints[myOneItem.ItemNo] += oneOrderDet.Quantity * (myOneItem.PointRedeemAmount == 0 ? myOneItem.RetailPrice : myOneItem.PointRedeemAmount);
                //        else
                //            AddedPoints.Add(myOneItem.ItemNo, oneOrderDet.Quantity * (myOneItem.PointRedeemAmount == 0 ? myOneItem.RetailPrice : myOneItem.PointRedeemAmount));
                //    }
                //}
                #endregion

                if (!Features.Package.PayReceiptsByPoints(pos.GetMemberInfo().MembershipNo, pos.GetOrderDate(), paymentType, AddedPoints, amount, ref pos, out status))
                    throw new Exception(status);
                
                #endregion



                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

                bool IsPointAllocationSuccess = true;
                bool isSuccessful;

                #region *) Core: Confirm Order
                isSuccessful = pos.ConfirmOrder
                    (false, out IsPointAllocationSuccess, out status);
                #endregion

                if (isSuccessful)
                {
                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Magento.UseMagentoFeatures), false))
                    {
                        if (!pos.SyncToMagento(out status))
                        {
                            if (status != "")
                                MessageBox.Show(status);
                        }
                    }

                    this.Cursor = System.Windows.Forms.Cursors.Default;
                    //if (!IsPointAllocationSuccess)
                    //{ MessageBox.Show("Point is not updated!\n" + status.Replace("(error)", "").Replace("(warning)", ""), "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning); }

                    //if (!IsQtyInsufficient)
                    if (!pos.ExecuteStockOut(out status))
                    {
                        //MessageBox.Show
                        //    ("Error while performing Stock Out: " + status,
                        //    "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    //Logger.writeLog("after stock out");
                    return true;
                }
                else
                {
                    pos.DeleteAllReceiptLinePayment();
                    this.Cursor = System.Windows.Forms.Cursors.Default;
                    MessageBox.Show("Error encountered: " + status, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                return false;
            }
            catch (Exception X)
            {
                CommonUILib.HandleException(X);
                return false;
            }
        }

        private void tryDownloadPoints()
        {
            if (PrintSettingInfo.receiptSetting.PrintReceipt)
            {
                bool overwriteSetting = !AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Print.HidePrintPackageBalanceOnReceipt), true);
                if ((Features.Package.isAvailable && AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Points.WaitToDownloadPointsBeforePrintReceipt), false)) || overwriteSetting)
                {
                    //Download 
                    //this.Enabled = false;
                    string isRealTime = ConfigurationManager.AppSettings["RealTimePointSystem"];
                    if (isRealTime == "yes" || isRealTime == "true")
                    {
                        if (pos.GetMemberInfo().MembershipNo != "WALK-IN")
                        {
                            frmDownloadPoints fDownloadPoints = new frmDownloadPoints();
                            fDownloadPoints.membershipNo = pos.GetMemberInfo().MembershipNo;
                            fDownloadPoints.orderHdrID = pos.myOrderHdr.OrderHdrID;
                            fDownloadPoints.ParentSyncPointsThread = ParentSyncPointsThread;
                            fDownloadPoints.ShowDialog();
                            if (!fDownloadPoints.IsSuccessful)
                                MessageBox.Show("Latest Point Data is not downloaded yet. Showing the latest point data in the receipt.");
                        }
                    }

                }
            }
        }

        private void frmSelectPayment_Load(object sender, EventArgs e)
        {
            isSuccessful = false;
            this.lblRefNo.Text = pos.GetUnsavedRefNo().Substring(13);
            lblNumOfItems.Text = pos.GetSumOfItemQuantity().ToString();
            LoadPaymentTypeLabels();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            isSuccessful = false;
            this.Close();
        }

        private void btnOtherPayment_Click(object sender, EventArgs e)
        {
          
        }
    }
}
