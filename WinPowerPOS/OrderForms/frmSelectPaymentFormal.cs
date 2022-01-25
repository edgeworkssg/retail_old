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
    public partial class frmSelectPaymentFormal : Form
    {
        public POSController pos;
        public bool isSuccessful;
        public decimal amount;
        string status;
        public string mode;
        public bool PrintReceipt = true;
        public bool choosePrintResult = false;
        public BackgroundWorker syncSalesThread;
        public BackgroundWorker ParentSyncPointsThread;
        public bool IsEdit = false;

        public frmSelectPaymentFormal()
        {
            InitializeComponent();
            LoadPaymentTypeLabels();
        }
        private const int TOTAL_BUTTONS = 10;
        private void LoadPaymentTypeLabels()
        {
            DataTable FormalPaymentTypeDt = new DataTable();
            DataSet ds = new DataSet();
            ds.ReadXml(AppDomain.CurrentDomain.BaseDirectory + "\\FormalPaymentTypes.xml");
            FormalPaymentTypeDt = ds.Tables[0];
            for (int Counter = 1; Counter <= TOTAL_BUTTONS; Counter++)
            {
                Control[] tmp = this.Controls.Find("btnPay" + Counter.ToString(), true);
                if (tmp.Length > 0)
                {
                    Button btnPay = (Button)tmp[0];
                    string paymentName = "";
                    DataRow[] dr = FormalPaymentTypeDt.Select("ID = '" + Counter.ToString() + "'");
                    if (dr.Length > 0)
                    {
                        paymentName = dr[0]["Name"].ToString();
                    }
                    //paymentName = PaymentTypesController.FetchPaymentByID(btnPay.Tag.ToString());
                    if (paymentName == "" || paymentName == "-")
                    {
                        btnPay.Visible = false;
                        btnPay.Enabled = false;
                    }
                    else
                    {
                        String[] tmpPayment = paymentName.Split('~');
                        if (tmpPayment.GetLength(0) > 1)
                        {
                            btnPay.Tag = tmpPayment[0];
                            btnPay.Text = tmpPayment[1];
                        }
                        else
                        {
                            btnPay.Tag = tmpPayment[0];
                            btnPay.Text = tmpPayment[0];
                        }
                    }
                }
            }
            //
            //btnPoints.Enabled = Features.Package.isAvailable && Features.Package.isRealTime;
            //btnPoints.Enabled = Features.Package.isAvailable; 
        }
        //PAY CASH
        public string change;
        private void btnCashPayment_Click(object sender, EventArgs e)
        {
            //CASH....
            try
            {
                /*
                CashDrawer drw = new CashDrawer();
                drw.OpenDrawer();
                */
                //Get payment mode...                
                frmPaymentTaking pyment = new frmPaymentTaking();
                pyment.pos = pos;

                pyment.lblAmount.Text = pos.RoundTotalReceiptAmount().ToString("N");

                pyment.ShowDialog();

                isSuccessful = pyment.isSuccessful;
                if (isSuccessful)
                {

                    frmChoosePrint fchoosePrint = new frmChoosePrint();
                    fchoosePrint.ShowDialog();
                    if (fchoosePrint.IsSuccessful)
                    {
                        choosePrintResult = true;
                        //ask for signature here
                        #region Signature
                        if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Signature.IsAvailableForAllPayment), false))
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
                            /*
                        else if (DR == DialogResult.Cancel || myfrm.choice == "cancel")
                        {
                            myfrm.Dispose();
                            return;
                        }
                             */
                        }
                        #endregion

                        if (PrintReceipt)
                        {
                            if (mode == "formal")
                            {
                                POSDeviceController.PrintFormalAHAVATransactionReceipt(pos, pyment.change, false,
                                    (ReceiptSizes)Enum.ToObject(typeof(ReceiptSizes),
                                PrintSettingInfo.receiptSetting.PaperSize.Value),
                                PrintSettingInfo.receiptSetting.NumOfCopies.Value, true);
                            }
                            else
                            {
                                POSDeviceController.PrintAHAVATransactionReceipt(pos, pyment.change, false,
                                    (ReceiptSizes)Enum.ToObject(typeof(ReceiptSizes),
                                PrintSettingInfo.receiptSetting.PaperSize.Value),
                                PrintSettingInfo.receiptSetting.NumOfCopies.Value);
                            }
                        }
                    }
                    change = pyment.lblChange.Text;
                    fchoosePrint.Dispose();
                    pyment.Dispose();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("An unknown error has been encountered. Please contact your system administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tryDownloadPoints()
        {
            if (Features.Package.isAvailable && AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Points.WaitToDownloadPointsBeforePrintReceipt), false))
            {
                //Download 
                //ShowPanelPleaseWait();
                this.Enabled = false;
                string isRealTime = ConfigurationManager.AppSettings["RealTimePointSystem"];
                if (isRealTime == "yes" || isRealTime == "true")
                {
                    if (pos.GetMemberInfo().MembershipNo != "WALK-IN")
                    {
                        PowerPOS.SyncPointsController.SyncPointsRealTimeController snc = new PowerPOS.SyncPointsController.SyncPointsRealTimeController();
                        if (!snc.SyncPoints(pos.GetMemberInfo().MembershipNo))
                        {
                            MessageBox.Show("Download Points Error.");
                        }
                    }
                }
                this.Enabled = true;
            }
        }

        //Multiple Payment Mode
        private void btnOther_Click(object sender, EventArgs e)
        {

            frmPartialPayment frm = new frmPartialPayment();
            frm.pos = pos;
            frm.IsEdit = false;
            frm.lblAmount.Text = amount.ToString();
            //frm.CustomerIsAMember = !string.IsNullOrEmpty(pos.CurrentMember.MembershipNo);
            frm.CustomerIsAMember = (pos.MembershipApplied() && pos.CurrentMember.MembershipNo.ToLower() != "walk-in");
            frm.ParentSyncPointsThread = ParentSyncPointsThread;
            frm.ShowDialog();

            if (frm.IsSuccessful)
            {
                 frmChoosePrint fchoosePrint = new frmChoosePrint();
                 fchoosePrint.ShowDialog();
                 if (fchoosePrint.IsSuccessful)
                 {

                     choosePrintResult = true;
                     //ask for signature here
                     #region Signature
                     if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Signature.IsAvailableForAllPayment), false))
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
                         /*
                     else if (DR == DialogResult.Cancel || myfrm.choice == "cancel")
                     {
                         myfrm.Dispose();
                         return;
                     }
                          */
                     }
                     #endregion
                     change = "$" + frm.lblChange.Text;

                     if (PrintReceipt)
                     {
                         if (mode == "formal")
                         {
                             POSDeviceController.PrintFormalAHAVATransactionReceipt(pos, frm.change, false,
                                 (ReceiptSizes)Enum.ToObject(typeof(ReceiptSizes),
                                 PrintSettingInfo.receiptSetting.PaperSize.Value),
                                 PrintSettingInfo.receiptSetting.NumOfCopies.Value,true);
                         }
                         else
                         {
                             POSDeviceController.PrintAHAVATransactionReceipt(pos, frm.change, false,
                                 (ReceiptSizes)Enum.ToObject(typeof(ReceiptSizes),
                                 PrintSettingInfo.receiptSetting.PaperSize.Value),
                                 PrintSettingInfo.receiptSetting.NumOfCopies.Value);
                         }
                     }
                 }


                isSuccessful = true;
                fchoosePrint.Dispose();
                frm.Dispose();
                this.Close();
            }
            else //clear payment lines if the form cancelled
                pos.DeleteAllReceiptLinePayment();

        }

        private void btnMakePayment_Click(object sender, EventArgs e)
        {
            // Start the counter time
            frmOrderTaking.counterStartTime = DateTime.Now;

            if (handlePayment(((Button)sender).Tag.ToString(),((Button)sender).Text ))
            {
                //GenericReport.NewPrint.A5Controller Printer = new GenericReport.NewPrint.A5Controller();
                //Printer.PrintInvoice(pos);

                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSales), false))
                    if (!syncSalesThread.IsBusy)
                        syncSalesThread.RunWorkerAsync();

                if (!IsEdit)
                {
                    tryDownloadPoints();

                    frmChoosePrint fchoosePrint = new frmChoosePrint();
                    fchoosePrint.ShowDialog();
                    if (fchoosePrint.IsSuccessful)
                    {
                        choosePrintResult = true;
                        //ask for signature here
                        #region Signature
                        if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Signature.IsAvailableForAllPayment), false))
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
                            /*
                        else if (DR == DialogResult.Cancel || myfrm.choice == "cancel")
                        {
                            myfrm.Dispose();
                            return;
                        }
                             */
                        }
                        #endregion

                        if (PrintReceipt)
                        {
                            //print receipt
                            if (mode == "formal")
                            {
                                POSDeviceController.PrintFormalAHAVATransactionReceipt(pos, 0, false,
                                    (ReceiptSizes)Enum.ToObject(typeof(ReceiptSizes),
                                    PrintSettingInfo.receiptSetting.PaperSize.Value),
                                    PrintSettingInfo.receiptSetting.NumOfCopies.Value, true);
                            }
                            else
                            {
                                POSDeviceController.PrintAHAVATransactionReceipt(pos, 0, false,
                                    (ReceiptSizes)Enum.ToObject(typeof(ReceiptSizes),
                                    PrintSettingInfo.receiptSetting.PaperSize.Value),
                                    PrintSettingInfo.receiptSetting.NumOfCopies.Value);
                            }
                        }

                    }
                    /*else
                    {
                        POSDeviceController.PrintFormalAHAVATransactionReceipt(pos, 0, false,
                                (ReceiptSizes)Enum.ToObject(typeof(ReceiptSizes),
                                PrintSettingInfo.receiptSetting.PaperSize.Value),
                                PrintSettingInfo.receiptSetting.NumOfCopies.Value,true);
                    }*/
                    
                    fchoosePrint.Dispose();
                }

                isSuccessful = true;
                this.Close();
            }
        }
        /*
        private void btnVisa_Click(object sender, EventArgs e)
        {
            if (handlePayment(POSController.PAY_VISA))
            {
                POSDeviceController.PrintAHAVATransactionReceipt(pos, 0, false,
                    (ReceiptSizes)Enum.ToObject(typeof(ReceiptSizes),
                    PrintSettingInfo.receiptSetting.PaperSize.Value),
                    PrintSettingInfo.receiptSetting.NumOfCopies.Value);

                isSuccessful = true;
                this.Close();
            }
        }
        private void btnMaster_Click(object sender, EventArgs e)
        {
            if (handlePayment(POSController.PAY_MASTER))
            {
                POSDeviceController.PrintAHAVATransactionReceipt(pos, 0, false,
                    (ReceiptSizes)Enum.ToObject(typeof(ReceiptSizes),
                    PrintSettingInfo.receiptSetting.PaperSize.Value),
                    PrintSettingInfo.receiptSetting.NumOfCopies.Value);

                isSuccessful = true;
                this.Close();
            }
        }
        */
        /// <remarks>
        /// Do not handle Multiple Payment
        /// </remarks>
        public bool handlePayment(string paymentType, string remarks)
        {
            try
            {
                decimal change;
                string status = "";
                bool isRoundingAllPayment = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Order.IsRoundingPreferenceAccordingToConfig), false);

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
                if (paymentType == POSController.PAY_POINTS)
                {
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

                    if (!Features.Package.BreakAmountIntoReceipts(pos.GetMemberInfo().MembershipNo, pos.GetOrderDate(), AddedPoints, amount, ref pos, out status))
                        throw new Exception(status);
                }
                else
                {
                    if (isRoundingAllPayment)
                        amount = pos.RoundTotalReceiptAmount();

                    if (!pos.AddReceiptLinePaymentWithRemark(amount, paymentType, remarks, out change, out status))
                        throw new Exception(status);
                }
                #endregion


                bool isSuccessful;
                if (!IsEdit)
                {

                    bool IsQtyInsufficient = false;

                    #region *) Validation: Check if Qty is sufficient to do stockout [Prompt if Error - Can be terminated by user]
                    /*if (!AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.RemoveZeroInventoryValidation), false))
                {
                    if (!pos.IsQtySufficientToDoStockOut(out status))
                    {

                        DialogResult dr = MessageBox.Show
                            ("Error: " + status + ". Are you sure you want to continue?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                        //DialogResult dr = DialogResult.Yes;
                        if (dr == DialogResult.No)
                        {
                            pos.DeleteAllReceiptLinePayment();
                            return false;
                        }
                        IsQtyInsufficient = true;
                    }
                }*/
                    #endregion

                    this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

                    bool IsPointAllocationSuccess = true;
                    #region *) Core: Confirm Order
                    bool isOutletSales = false;
                    if (pos.GetMemberInfo() != null && pos.GetMemberInfo().MembershipNo != "" && pos.GetMemberInfo().MembershipNo != "WALK-IN")
                    {
                        //validation if outlet order
                        PointOfSaleCollection posColl = new PointOfSaleCollection();
                        posColl.Where(PointOfSale.UserColumns.LinkedMembershipNo, pos.GetMemberInfo().MembershipNo);
                        posColl.Load();
                        if (posColl.Count > 0)
                            isOutletSales = true;

                    }


                    isSuccessful = pos.ConfirmOrder
                        (isRoundingAllPayment, out IsPointAllocationSuccess, out status);
                    #endregion

                    if (isSuccessful)
                    {

                        this.Cursor = System.Windows.Forms.Cursors.Default;
                        /*if (!IsPointAllocationSuccess)
                        { MessageBox.Show("Point is not updated!\n" + status.Replace("(error)", "").Replace("(warning)", ""), "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
                        */
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
                }
                else
                {
                    this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

                    //clear all payments in existing                 
                    //update to new payment types.....
                    isSuccessful = pos.SavePaymentTypes();

                    if (isSuccessful)
                    {
                        this.Cursor = System.Windows.Forms.Cursors.Default;
                        return true;
                    }
                    else
                    {
                        this.Cursor = System.Windows.Forms.Cursors.Default;
                        pos.DeleteAllReceiptLinePayment();
                        MessageBox.Show("Error encountered: " + status, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                return false;
            }
            catch (Exception X)
            {
                CommonUILib.HandleException(X);
                return false;
            }
        }


        private void frmSelectPayment_Load(object sender, EventArgs e)
        {
            isSuccessful = false;
            this.lblRefNo.Text = pos.GetUnsavedRefNo().Substring(13);
            lblNumOfItems.Text = pos.GetSumOfItemQuantity().ToString();

            //btnPoints.Enabled = (pos != null) && (pos.MembershipApplied () /*string.IsNullOrEmpty(pos.CurrentMember.MembershipNo)*/) && Features.Package.isAvailable && Features.Package.isRealTime;
            //btnPoints.Enabled = (pos != null) && (pos.MembershipApplied() /*string.IsNullOrEmpty(pos.CurrentMember.MembershipNo)*/) && Features.Package.isAvailable;
            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            isSuccessful = false;
            this.Close();
        }

        /// <summary>
        /// Handle payment by points (Only)
        /// </summary>
        /// <remarks>
        /// *) If Not all line item can be paid by points, Exit
        /// *) If Points not sufficient, Exit
        /// </remarks>
        private void btnPoints_Click(object sender, EventArgs e)
        {
            try
            {
                if (!pos.MembershipApplied())
                    throw new Exception("(warning)Please assign a member to use point");

                if (pos.hasItemThatCannotBeRedeemedByPoints)
                    throw new Exception("(warning)Some item cannot be paid by points");

                //ask for sign after breaking amount to receipt
                #region Signature
                if (!AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Signature.IsAvailableForAllPayment), false))
                {
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
                        /*
                    else if (DR == DialogResult.Cancel || myfrm.choice == "cancel")
                    {
                        myfrm.Dispose();
                        return;
                    }
                         */
                    }
                }
                #endregion


                /*if (handlePayment(POSController.PAY_POINTS))
                {
                    Logger.writeLog("start printing");
                    frmChoosePrint fchoosePrint = new frmChoosePrint();
                    fchoosePrint.ShowDialog();
                    if (fchoosePrint.IsSuccessful)
                    {
                        POSDeviceController.PrintAHAVATransactionReceipt(pos, 0, false,
                            (ReceiptSizes)Enum.ToObject(typeof(ReceiptSizes),
                            PrintSettingInfo.receiptSetting.PaperSize.Value),
                            PrintSettingInfo.receiptSetting.NumOfCopies.Value);
                        Logger.writeLog("end printing");
                    }
                    isSuccessful = true;
                    fchoosePrint.Dispose();
                    this.Close();
                }*/
            }
            catch (Exception X)
            {
                if (X.Message.StartsWith("(warning)"))
                {
                    MessageBox.Show(X.Message.Replace("(warning)", ""), "Warning",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (X.Message.StartsWith("(error)"))
                {
                    MessageBox.Show(X.Message.Replace("(error)", ""), "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    Logger.writeLog(X);
                    MessageBox.Show(
                        "Some error occurred. Please contact your administrator.", "Error"
                        , MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
