using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using POSDevices;
using System.Linq;
using SubSonic;
//using WinPowerPOS.InstallmentForms;
using System.Configuration;
namespace WinPowerPOS.OrderForms
{

    public partial class frmQuickPayment : Form
    {

        public POSController pos;
        PriceDisplay myDisplay;
        public bool IsEdit;
        public bool hasInstallment;
        public bool IsSuccessful;
        public decimal change;
        private decimal OrderAmount;
        private string ChequeNo, BankName;
        int poleDisplayWidth = 0;

        public bool CustomerIsAMember
        {
            get
            { return btnPoint.Enabled; }
            set
            {
                btnPoint.Enabled = false;
                if (AppSetting.GetSettingFromDBAndConfigFile("RealTimePointSystem") != null)
                    btnPoint.Enabled = (AppSetting.GetSettingFromDBAndConfigFile("RealTimePointSystem").
                        ToString() == "yes") && value;
            }

        }

        public frmQuickPayment()
        {
            InitializeComponent();
            LoadPaymentTypeLabels();
            myDisplay = new PriceDisplay();

        }

        private bool SetDefaultAmount()
        {
            bool isSuccess = false;

            if (txtAmt.Text == "")
            {
                string status = "";
                decimal totalPaid = pos.CalculateTotalPaid(out status);
                decimal totalOrderAmt = 0;
                if (cbRounding.Checked)
                    totalOrderAmt = pos.RoundTotalReceiptAmount();
                else
                    totalOrderAmt = pos.CalculateTotalAmount(out status);

                decimal val = (totalOrderAmt - totalPaid);
                if (val > 0)
                    txtAmt.Text = val.ToString("N2");
            }

            return isSuccess;
        }

        private void LoadPaymentTypeLabels()
        {
            try
            {
                DataSet ds = new DataSet();
                ds.ReadXml(AppDomain.CurrentDomain.BaseDirectory + "\\PaymentTypes.xml");
                var paymentType = ds.Tables[0];
                for (int i = 0; i < paymentType.Rows.Count; i++)
                {
                    string theType = ((string)paymentType.Rows[i]["Name"]) + "";
                    if (theType.ToUpper().Equals("CASH") ||
                        theType.ToUpper().Equals("INSTALLMENT") ||
                        theType.ToUpper().Equals("POINTS") ||
                        theType.ToUpper().Equals("POINT") ||
                        theType.ToUpper().Equals("CHEQUE"))
                    {
                        continue;
                    }

                    Button btn = new Button();
                    btn.Name = "btn" + theType.ToUpper();
                    btn.Tag = theType.ToUpper();
                    btn.Text = theType.ToUpper();
                    btn.BackColor = Color.MidnightBlue;
                    btn.ForeColor = Color.WhiteSmoke;
                    btn.Size = new Size(128, 44);
                    btn.Font = new Font(new FontFamily("Verdana"), 9f, FontStyle.Bold, GraphicsUnit.Point);
                    btn.Click += new EventHandler(btnMakePayment_Click);
                    flpButtonPay.Controls.Add(btn);

                    Panel pnl = new Panel();
                    pnl.Name = "pnl" + theType.ToUpper() + i;
                    pnl.Tag = theType.ToUpper();
                    pnl.Size = new Size(95, 44);
                    //pnl.BackColor = Color.Violet;

                    Label lbl = new Label();
                    lbl.Name = "lbl" + theType.ToUpper() + i;
                    lbl.Tag = theType.ToUpper();
                    //lbl.Size = new Size(34, 20);
                    lbl.Location = new Point(3, 11);
                    lbl.Text = "0";
                    lbl.Font = new Font(new FontFamily("Microsoft Sans Serif"), 12f, FontStyle.Bold, GraphicsUnit.Point);
                    lbl.Visible = false;
                    pnl.Controls.Add(lbl);

                    flpLabelPay.Controls.Add(pnl);

                    Button btnDel = new Button();
                    btnDel.Name = "btnDel" + theType.ToUpper();
                    btnDel.Tag = theType.ToUpper();
                    btnDel.Size = new Size(66, 44);
                    btnDel.BackColor = Color.Maroon;
                    btnDel.ForeColor = Color.White;
                    btnDel.Font = new Font(new FontFamily("Verdana"), 9f, FontStyle.Bold, GraphicsUnit.Point);
                    btnDel.Click += new EventHandler(btnDel_Click);
                    btnDel.Text = "X";
                    flpDelPay.Controls.Add(btnDel);
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        protected void btnDel_Click(object sender, EventArgs e)
        {
            txtAmt.Text = "0";
            AddPayment(((Button)sender).Tag.ToString());
        }

        private void frmPartialPayment_Load(object sender, EventArgs e)
        {
            string refno = pos.GetUnsavedRefNo();
            hasInstallment = pos.hasPaymentType(POSController.PAY_INSTALLMENT);
            lblRefNo.Text = refno.Substring(refno.Length - 3, 3);
            lblNumOfItems.Text = pos.GetNoOfItem().ToString();
            lblTotalQty.Text = pos.GetSumOfItemQuantity().ToString();
            string status;
            change = 0;
            if (!decimal.TryParse(lblAmount.Text, out OrderAmount))
            {
                MessageBox.Show("Error. Invalid Order amount", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                IsSuccessful = false;
                this.Close();
            }
            dgvPayment.AutoGenerateColumns = false;
            BindGrid();
            myDisplay.ShowTotal((double)pos.CalculateTotalAmount(out status));
            if (!pos.MembershipApplied() || (pos.CurrentMember != null && pos.CurrentMember.MembershipNo.ToLower() == "walk-in"))
            {
                btnInstallment.Enabled = false;
            }
            if (AppSetting.GetSetting(AppSetting.SettingsName.Payment.InstallmentText) != null && AppSetting.GetSetting(AppSetting.SettingsName.Payment.InstallmentText) != "")
            {
                btnInstallment.Text = AppSetting.GetSetting(AppSetting.SettingsName.Payment.InstallmentText).ToUpper();
            }
        }

        private void BindGrid()
        {
            string status = "";
            DataTable paymentData = pos.FetchUnsavedReceipt();
            dgvPayment.DataSource = paymentData;
            dgvPayment.Refresh();

            for (int i = 0; i < flpLabelPay.Controls.Count; i++)
            {
                Panel pnl = (Panel)flpLabelPay.Controls[i];
                Label lbl = (Label)pnl.Controls[0];
                if (lbl != null)
                {
                    decimal payAmt = (from o in pos.recDet
                                      where o.PaymentType.ToUpper() == lbl.Tag.ToString().ToUpper()
                                      select (decimal?)o.Amount)
                                      .FirstOrDefault()
                                      .GetValueOrDefault(0);
                    lbl.Visible = payAmt != 0;
                    lbl.Text = payAmt.ToString("N2");
                }
            }


            decimal totalPaid = pos.CalculateTotalPaid(out status);
            decimal totalOrderAmt;
            if (cbRounding.Checked)
            {
                totalOrderAmt = pos.RoundTotalReceiptAmount();
            }
            else
            {
                totalOrderAmt = pos.CalculateTotalAmount(out status);
            }
            lblAmount.Text = totalOrderAmt.ToString("N2");

            if (status != "")
            {
                MessageBox.Show("Error while calculating total amount: " + status, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            lblTotalPaid.Text = totalPaid.ToString("N2");
            lblShortFall.Text = (totalOrderAmt - totalPaid).ToString("N2");
            if (!int.TryParse(AppSetting.GetSetting(AppSetting.SettingsName.PoleDisplay.PoleDisplayLinesLength), out poleDisplayWidth))
            {
                poleDisplayWidth = 20;
            }
            myDisplay.ClearScreen();
            myDisplay.ShowItemPrice("Balance: ", (double)(totalOrderAmt - totalPaid), (double)totalOrderAmt, poleDisplayWidth);
        }

        private void AddPayment(string paymentType)
        {
            #region *) Set Default Amount
            SetDefaultAmount();
            #endregion

            #region *) Remove Old Value

            decimal newVal = 0;
            decimal oldVal = 0;
            if (decimal.TryParse(txtAmt.Text, out newVal))
            {
                oldVal = (from o in pos.recDet
                          where o.PaymentType == paymentType
                          select (decimal?)o.Amount).FirstOrDefault().GetValueOrDefault(0);
                RemovePayment(paymentType);
                if (newVal != 0)
                {
                    txtAmt.Text = (oldVal + newVal).ToString();
                }
                else
                {
                    txtAmt.Text = "";
                    BindGrid();
                    return;
                }
            }
            else
            {
                return;
            }

            #endregion

            try
            {
                decimal paymentAmt = 0, TotalPaid = 0;
                string status, refno = "";

                #region *) Check Installment in chinesse
                if (paymentType == "欠款余额")
                {
                    paymentType = "Installment";
                }
                #endregion

                //Validate txtAmount
                if (!decimal.TryParse(lblTotalPaid.Text, out TotalPaid) || !decimal.TryParse(txtAmt.Text, out paymentAmt) || paymentAmt <= 0)
                {
                    frmKeypadFC k = new frmKeypadFC();
                    k.IsInteger = false;
                    k.textMessage = "O/S:" + lblShortFall.Text;
                    if (paymentType == POSController.PAY_POINTS)
                    {
                        decimal InitValue = pos.getAllRedeemableTotalAmount;
                        InitValue -= pos.CalculateTotalPaid_ByPoints(out status);
                        k.initialValue = InitValue.ToString();
                    }
                    k.ShowDialog();
                    if (!decimal.TryParse(k.value, out paymentAmt))
                    {
                        BindGrid();
                        return;
                    }
                }

                #region *) Warning: Notice user if there is extra charge
                decimal ExtraChargeTotalAmount = pos.CheckExtraChargeAmount(paymentType, paymentAmt);

                if (ExtraChargeTotalAmount != 0)
                {
                    //DialogResult DR = MessageBox.Show(
                    //    "There will be extra charge applicable of " + ExtraChargeTotalAmount.ToString("N2") + ". Do you still want to continue?"
                    //    , "Extra Charge Applicable", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                    //if (DR == DialogResult.Cancel) return;
                    frmExtraCharge frmExt = new frmExtraCharge();
                    frmExt.totalAmount = paymentAmt;
                    frmExt.extraCharge = ExtraChargeTotalAmount;
                    frmExt.totalAmountAfterCharge = paymentAmt + ExtraChargeTotalAmount;
                    frmExt.ShowDialog();

                    if (!frmExt.isConfirmed) return;
                }
                #endregion

                #region -= Point Validation =-
                if (paymentType == POSController.PAY_POINTS)
                {
                    decimal TotalPointableSales = pos.getAllRedeemableTotalAmount;
                    decimal PointsPaid = pos.CalculateTotalPaid_ByPoints(out status);
                    decimal PointOnServer;

                    if (!PowerPOS.Feature.Package.getAvailablePoints(pos.CurrentMember.MembershipNo, pos.GetOrderDate(), out PointOnServer, out status))
                    {
                        Logger.writeLog(status);
                        MessageBox.Show("Some error occurred: " + status, "Payment Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (PointOnServer - (PointsPaid + paymentAmt) < 0)
                    {
                        MessageBox.Show("Point is not enough", "Payment Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (TotalPointableSales <= 0)
                    {
                        MessageBox.Show("There is no item can be paid by points.\nPlease select another payment type",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (PointsPaid + paymentAmt > TotalPointableSales)
                    {
                        MessageBox.Show("Customer pay too much\n" +
                            "Point-Redeemable : " + (TotalPointableSales - PointsPaid).ToString("N2") + "\n" +
                            "Payment by point : " + paymentAmt, "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        paymentAmt = TotalPointableSales - PointsPaid;
                    }

                    if ((pos.CalculateTotalPaid_ByPoints(out status) + paymentAmt) > PointOnServer)
                    {
                        if (MessageBox.Show("Customer only have " + (PointOnServer - PointsPaid).ToString("N2") + " points\nDo you want to use it?"
                            , "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                        { return; }

                        paymentAmt = (PointOnServer - PointsPaid);
                    }

                    #region *) Signature
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
                }
                #endregion

                if (paymentType == POSController.PAY_CHEQUE)
                {
                    if (!pos.AddChequeReceiptLinePayment(paymentAmt, ChequeNo, BankName, out status))
                    {
                        MessageBox.Show("Error: " + status, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else if (paymentType == POSController.PAY_POINTS)
                {
                    SortedList<string, decimal> AddedPoints = new SortedList<string, decimal>();
                    foreach (OrderDet oneOrderDet in pos.myOrderDet)
                    {
                        Item myOneItem = oneOrderDet.Item;
                        if (myOneItem.PointRedeemMode == Item.PointMode.Dollar)
                        {
                            if (AddedPoints.ContainsKey(myOneItem.ItemNo))
                                AddedPoints[myOneItem.ItemNo] += oneOrderDet.Quantity.GetValueOrDefault(0) * (myOneItem.PointRedeemAmount == 0 ? myOneItem.RetailPrice : myOneItem.PointRedeemAmount);
                            else
                                AddedPoints.Add(myOneItem.ItemNo, oneOrderDet.Quantity.GetValueOrDefault(0) * (myOneItem.PointRedeemAmount == 0 ? myOneItem.RetailPrice : myOneItem.PointRedeemAmount));
                        }
                    }

                    if (!PowerPOS.Feature.Package.BreakAmountIntoReceipts(pos.GetMemberInfo().MembershipNo, pos.GetOrderDate(), AddedPoints, paymentAmt, ref pos, out status))
                        throw new Exception("Error encountered - " + status);
                }
                else
                {
                    if (!pos.AddReceiptLinePayment(paymentAmt, paymentType, refno, 0, "", 0, out change, out status))
                    {
                        MessageBox.Show("Error: " + status, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }


                lblChange.Text = change.ToString("N2");
                BindGrid();
                if (change > 0)
                {
                    myDisplay.ClearScreen();
                    myDisplay.SendCommandToDisplay("Change: $" + lblChange.Text);
                }
            }
            catch (Exception X)
            {
                Logger.writeLog(X);
                MessageBox.Show(X.Message);
                if (oldVal != 0)
                {
                    txtAmt.Text = oldVal.ToString();
                    AddPayment(paymentType);
                }
                BindGrid();
            }
        }

        private void RemovePayment(string paymentType)
        {
            bool isSuccess = pos.RemoveReceiptByPaymentType(paymentType);
        }

        #region -= Add Payment Selection =-
        private void btnCash_Click(object sender, EventArgs e)
        {
            AddPayment(POSController.PAY_CASH);
            txtAmt.Text = "";
        }

        private void btnMakePayment_Click(object sender, EventArgs e)
        {
            AddPayment(((Button)sender).Tag.ToString());
            txtAmt.Text = "";
        }

        private void btnCheque_Click(object sender, EventArgs e)
        {
            frmBankChequeNo frm = new frmBankChequeNo();
            frm.ShowDialog();

            if (frm.IsSuccessful)
            {
                ChequeNo = frm.ChequeNo;
                BankName = frm.BankName;
                AddPayment(POSController.PAY_CHEQUE);
                frm.Dispose();
            }
        }

        private void btnPoint_Click(object sender, EventArgs e)
        {
            if (IsEdit)
            {
                MessageBox.Show("Cannot edit payment by points", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (pos.getAllRedeemableTotalAmount <= 0)
            {
                MessageBox.Show("There is no item can be paid by points.\nPlease select another payment type",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            AddPayment(POSController.PAY_POINTS);
        }
        #endregion

        private bool IgnoreValidation = false;

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //set 
            IgnoreValidation = true;
            IsSuccessful = false;
            this.Close();
        }
        private decimal maxFinalDisc = 0;

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            // Start the counter time
            frmOrderTaking.counterStartTime = DateTime.Now;

            //check amount completed....
            if (Decimal.Parse(lblShortFall.Text) > 0)
            {
                MessageBox.Show("There is still some amount that has not been completed. Please clear all amount", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            if (!IsEdit)
            {
                #region *) Open Cash Drawer
                if (pos.hasPaymentType(POSController.PAY_CASH) && pos.GetTotalSales() != 0)
                {
                    CashDrawer drw = new CashDrawer();
                    drw.OpenDrawer();
                }
                #endregion

                string status;
                bool IsQtyInsufficient = false;
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.BlockTransactionIfBalQtyNotSuf), false))
                {
                    if (!pos.IsQtySufficientToDoStockOutLocal(out status))
                    {
                        DialogResult dr = MessageBox.Show
                                ("Error: " + status + ". You can not continue the transaction.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {

                    if (!AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.RemoveZeroInventoryValidation), false))
                    {
                        if (!pos.IsQtySufficientToDoStockOut(out status))
                        {

                            DialogResult dr = MessageBox.Show
                                ("Error: " + status + ". Are you sure you want to continue?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                            if (dr == DialogResult.No)
                            {
                                pos.DeleteAllReceiptLinePayment();
                                return;
                            }
                            IsQtyInsufficient = true;
                        }
                    }
                }
                

                bool IsPointAllocationSuccess = false;
                //pos.SetPaymentRemark(txtRemark.Text);
                if (!pos.ConfirmOrder(cbRounding.Checked, out IsPointAllocationSuccess, out status))
                {
                    MessageBox.Show("Error encountered when confirming order: " + status, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Logger.writeLog(status);
                    return;
                }

                if (!IsPointAllocationSuccess)
                { MessageBox.Show("Point is not updated!" + Environment.NewLine + status, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning); }

                if (!IsQtyInsufficient)
                    if (!pos.ExecuteStockOut(out status))
                    {
                        MessageBox.Show
                            ("Error while performing Stock Out: " + status, "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }


                //set 
                IsSuccessful = true;


                //close dialogue
                this.Close();
            }
            else
            {
                //clear all payments in existing                 
                //update to new payment types.....
                pos.SavePaymentTypes();

                if (hasInstallment &&
                    !pos.hasPaymentType(POSController.PAY_INSTALLMENT))
                {
                    Installment.Delete(Installment.Columns.OrderHdrId, pos.GetSavedRefNo().Substring(2));
                }
                //prompt installment
                /*
                if (pos.hasPaymentType(POSController.PAY_INSTALLMENT))
                {
                    frmCreateInstallment f = new frmCreateInstallment();
                    f.pos = pos;
                    f.ShowDialog();
                }*/
                IsSuccessful = true;
                this.Close();
            }
        }

        private void dgvPayment_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                string PaymentType = pos.getReceiptPaymentType(dgvPayment[3, e.RowIndex].Value.ToString());
                if (string.IsNullOrEmpty(PaymentType))
                {
                    MessageBox.Show("Error in deleting payment. Please try again.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (PaymentType == POSController.PAY_POINTS || PaymentType == POSController.PAY_PACKAGE)
                {
                    if (IsEdit)
                    {
                        MessageBox.Show("Cannot delete payment by points", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                //delete from receipt line...                
                if (!pos.DeleteReceiptLinePayment(dgvPayment[3, e.RowIndex].Value.ToString()))
                {
                    MessageBox.Show("Error in deleting payment. Please try again.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                BindGrid();
            }
        }

        private void txtAmt_Validating(object sender, CancelEventArgs e)
        {
            //decimal result;
            //e.Cancel = !decimal.TryParse(txtAmt.Text, out result);
            //if (e.Cancel)
            //{
            //    errorProvider1.SetError(txtAmt, "Please enter valid amount");
            //}
            //if (!IgnoreValidation && !CommonUILib.ValidateTextBoxAsUnsignedDecimal(txtAmt, out result))
            //{
            //    errorProvider1.SetError(txtAmt, "Please enter valid amount");
            //    e.Cancel = true;
            //}
        }

        private void txtAmt_Click(object sender, EventArgs e)
        {
            ////prompt keypad
            //frmKeypad f = new frmKeypad();
            //f.IsInteger = false;
            //f.initialValue = ((TextBox)sender).Text;
            //f.textMessage = "O/S:" + lblShortFall.Text;
            //f.ShowDialog();

            //((TextBox)sender).Text = f.value.ToString();

        }

        private void cbRounding_CheckedChanged(object sender, EventArgs e)
        {
            BindGrid();
        }

        private void btnInstallment_Click(object sender, EventArgs e)
        {
            AddPayment("INSTALLMENT");
            txtAmt.Text = "";
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            txtAmt.Select();
            SendKeys.Send(((Button)sender).Text);
        }

        private void btnPlus_Click(object sender, EventArgs e)
        {
            decimal val;
            if (txtAmt.Text == "")
                return;
            val = decimal.Parse(txtAmt.Text);
            val = val + 1;
            txtAmt.Text = val.ToString();
            txtAmt.Select();
        }

        private void btnMin_Click(object sender, EventArgs e)
        {
            if (txtAmt.Text == "")
                return;
            decimal val;
            val = decimal.Parse(txtAmt.Text);
            if (val >= 1)
            {
                val = val - 1;
            }
            txtAmt.Text = val.ToString();
            txtAmt.Select();
        }

        private void btnCLEAR_Click(object sender, EventArgs e)
        {
            txtAmt.Text = "";
        }

        private void btnBackSpace_Click(object sender, EventArgs e)
        {
            //txtEntry.Text = txtEntry.Text.Remove(txtEntry.Text.Length - 1);
            txtAmt.Select();
            SendKeys.Send("{BACKSPACE}");
        }

        private void btnDot_Click(object sender, EventArgs e)
        {
            if (!txtAmt.Text.Contains("."))
            {
                txtAmt.Focus();
                SendKeys.Send(((Button)sender).Text);
            }
        }

        private void txtAmt_TextChanged(object sender, EventArgs e)
        {
            decimal val = 0;
            var txt = (TextBox)sender;
            if (txt.Text != "")
            {
                if (!decimal.TryParse(txt.Text, out val))
                    txt.Text = "";
            }
        }

        private void btnDirectAmount_Click(object sender, EventArgs e)
        {
            if (((Button)sender).Tag != null)
            {
                txtAmt.Text = ((Button)sender).Tag.ToString();
                AddPayment(POSController.PAY_CASH);
                txtAmt.Text = "";
            }
        }
    }
}