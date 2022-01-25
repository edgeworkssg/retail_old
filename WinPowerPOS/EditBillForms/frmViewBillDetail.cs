using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PowerPOS.Container;
using PowerPOS;
using POSDevices;
using WinPowerPOS.OrderForms;
using SubSonic;
//using WinPowerPOS.InstallmentForms;
using WinPowerPOS.MembershipForms;
using System.IO;
using System.Reflection;
using System.Configuration;
using LanguageManager = WinPowerPOS.Properties.Language;

namespace WinPowerPOS.EditBillForms
{
    /// <remarks>
    /// Update Track:
    /// *) Remarks Feature:
    ///    -) [205] Remarks Button only add new lines in remarks field (Cannot delete) [24 Sep 201]
    ///    -) [205] Remarks Button change data directly to Database - loaded pos variable become unconsistent [24 Sep 2010]
    ///    -) [205] Cannot change Remarks after CounterClosing [27 Sep 2010]
    /// 
    /// *) [205] [OnProgress] User can assign/reassign (CHANGE) membership to OrderHdr as long as Member is exists within LOCAL database
    /// 
    /// Unhandled Issue:
    /// *) [205] If user buy points (ex: 500Points), and user void in this form, Points become unrelevant
    /// *) [205] If user buy points (ex: 500Points), and user change member in this form, Points become unrelevant
    /// </remarks>
    public partial class frmViewBillDetail : Form
    {
        //Controllers        
        POSController pos;
        /*OrderHdr oHdr;
        OrderDetCollection oDet;*/
        public string OrderHdrID;
        private bool EnableSecondDiscount = false;
        private string lineInfoCaption;
        public BackgroundWorker SyncSalesThread;

        bool isFromOtherOutlet = false;


        public frmViewBillDetail()
        {
            InitializeComponent();
            dgvLineItem.AutoGenerateColumns = false;
            EnableSecondDiscount = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Discount.EnableSecondDiscount), false);
            lineInfoCaption = AppSetting.CastString(AppSetting.GetSetting(AppSetting.SettingsName.LineInfo.ReplaceTextWith));
            isFromOtherOutlet = false;
        }
        private void frmViewBillDetail_Load(object sender, EventArgs e)
        {
            try
            {
                pos = new POSController(OrderHdrID);
                lblRefNo.Text = pos.GetCustomizedRefNo();

                if (lblRefNo.Text == "")
                {
                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Membership.DownloadAllRecentPurchase), false))
                    {
                        string status1 = "";
                        DataSet ds = SyncClientController.GetOrder(OrderHdrID, out status1);
                        if (ds != null && ds.Tables != null && ds.Tables.Count == 4)
                        {
                            pos = new POSController();
                            pos.myOrderHdr.Load(ds.Tables[0]);
                            pos.myOrderDet.Load(ds.Tables[1]);
                            pos.recHdr.Load(ds.Tables[2]);
                            pos.recDet.Load(ds.Tables[3]);
                            if (pos.myOrderHdr.MembershipNo != null && pos.myOrderHdr.MembershipNo != "")
                            {
                                pos.CurrentMember = new Membership(Membership.Columns.MembershipNo, pos.myOrderHdr.MembershipNo);
                            }
                            else
                            {
                                pos.CurrentMember = new Membership();
                            }
                            isFromOtherOutlet = true;

                        }
                        else
                        {
                            MessageBox.Show("Receipt Number " + OrderHdrID + " does not exist in this computer", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Receipt Number " + OrderHdrID + " does not exist in this computer", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                lblRefNo.Text = pos.GetCustomizedRefNo();


                if (pos.IsVoided())
                {
                    groupBox4.BackColor = Color.Pink;
                }
                else
                {
                    groupBox4.BackColor = Color.Transparent;
                }
                bool isEnableVoidButton = (pos.GetOrderDate() < POSController.FetchLatestCloseCounterTime(pos.GetPointOfSaleId()));
                //check if user has privilege  ALLOW DO PAST VOID
                bool isEnableEditPaymentButton = isEnableVoidButton;
                if (PrivilegesController.HasPrivilege(PrivilegesController.ALLOW_DO_PAST_VOID, UserController.FetchGroupPrivilegesWithUsername(UserInfo.role, UserInfo.username)))
                {
                    if (!pos.IsVoided())
                        isEnableVoidButton = false;
                }
                if (PrivilegesController.HasPrivilege(PrivilegesController.CHANGE_PAST_PAYMENT_TYPE, UserController.FetchGroupPrivilegesWithUsername(UserInfo.role, UserInfo.username)))
                {
                    if (!pos.IsVoided())
                        isEnableEditPaymentButton = false;
                }
                btnVoid.Enabled = !isEnableVoidButton;
                btnEdit.Enabled = !isEnableEditPaymentButton;
                btnWarranty.Enabled = !isEnableVoidButton;
                lblOrderDate.Text = pos.GetOrderDate().ToString("dd MMM yyyy HH:mm");
                lblSalesPerson.Text = pos.GetSalesPerson();
                lblCashier.Text = pos.GetCashierID();
                lblRemark.Text = pos.GetHeaderRemark(); if (string.IsNullOrEmpty(lblRemark.Text)) lblRemark.Text = "-";
                lblLineInfo.Text = pos.GetLineInfoOfFirstLine(); if (string.IsNullOrEmpty(lblLineInfo.Text)) lblLineInfo.Text = "-";
                string status;
                DataTable dtLine = pos.FetchUnSavedOrderItems(out status);
                DataView dv = dtLine.DefaultView;
                dv.Sort = "OrderDetSequence asc";
                dgvLineItem.DataSource = dv.ToTable(); // GetOrderDetail();
                dgvLineItem.Refresh();

                dgvLineItem.Columns["DiscountDetail"].Visible = EnableSecondDiscount;
                dgvLineItem.Columns["Disc"].Visible = !EnableSecondDiscount;

                //pos.LoadMembership();
                if (pos.MembershipApplied())
                {
                    Membership member = pos.GetMemberInfo();
                    lblMembershipNo.Text = member.MembershipNo;
                    lblName.Text = member.NameToAppear;
                    lblMembershipGroup.Text = member.MembershipGroup.GroupName;
                    lblMembershipDiscount.Text = member.MembershipGroup.Discount.ToString("N2");
                }
                /*else
                {
                    //Load the membership No
                    
                    Membership member = pos.GetOtherMemberInfo();
                    if (member != null)
                    {
                        lblMembershipNo.Text = member.MembershipNo;
                        lblName.Text = member.NameToAppear;
                        if (member.MembershipGroup != null)
                        {
                            lblMembershipGroup.Text = member.MembershipGroup.GroupName;
                            lblMembershipDiscount.Text = member.MembershipGroup.Discount.ToString("N2");
                        }
                    }
                }*/

                btnResend.Visible = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.MallIntegration.useNUHMallIntegration), false);
                btnReprintValidation.Visible = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.SICC.CheckValidationPrinting), false);
                dgvPaymentType.AutoGenerateColumns = false;
                dgvPaymentType.DataSource = pos.FetchUnsavedReceipt(true);
                dgvPaymentType.Refresh();

                /*          
                pos = new POSController();
                //itemLogic = new ItemController();

                pnlViewDetails.Visible = true;
                pnlViewDetails.BringToFront();*/

                #region load signature image
                //hide signature panel if the settings is not ticked
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Signature.IsAvailable), false)
                    || AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Signature.IsAvailableForAllPayment), false))
                {
                    //receipt ref num is the image name
                    string signatureFile = Application.StartupPath + "\\Signature\\sign" + pos.GetUnsavedRefNo() + ".jpg";

                    if (System.IO.File.Exists(signatureFile))
                    {
                        //Bitmap MyBitmap = new Bitmap(signatureFile);
                        imgSignature.Load(signatureFile);
                        imgSignature.Visible = true;
                    }
                    else //hide if there is no image
                        imgSignature.Visible = false;
                }
                #endregion

                gbLineInfo.Visible = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.LineInfo.ShowInInvoice), false);
                if (!string.IsNullOrEmpty(lineInfoCaption)) gbLineInfo.Text = lineInfoCaption;

                bool EnableSecondSalesPerson = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Order.EnableSecondSalesPerson), false);
                if (!EnableSecondSalesPerson)
                {
                    dgvLineItem.Columns["SalesPerson2"].Visible = false;
                }

                groupBox1.Text = string.Format("{0}", AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.IsReplaceMembershipText), false) && !string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.MembershipTextReplacement)) ? AppSetting.GetSetting(AppSetting.SettingsName.MembershipTextReplacement).ToString().ToUpper() : "MEMBERSHIP");
                btnChangeMember.Text = string.Format("CHANGE {0}", AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.IsReplaceMembershipText), false) && !string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.MembershipTextReplacement)) ? AppSetting.GetSetting(AppSetting.SettingsName.MembershipTextReplacement).ToString().ToUpper() : "MEMBER");

                if (pos.GetCashReceived() > 0)
                {
                    gbCash.Visible = true;
                    lblCashReceived.Text = pos.GetCashReceived().ToString("N2");
                    lblChange.Text = pos.GetChange().ToString("N2");
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("An unknown error has been encountered. Please contact your system administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public DataTable GetOrderDetail()
        {

            try
            {
                //create and return a datatable.....
                DataTable dTable = new DataTable();
                DataRow dr;

                dTable.Columns.Add("ItemNo");
                dTable.Columns.Add("ItemName");
                dTable.Columns.Add("itemdesc");
                dTable.Columns.Add("CategoryName");
                dTable.Columns.Add("Quantity");
                dTable.Columns.Add("Disc(%)");
                dTable.Columns.Add("Price");
                dTable.Columns.Add("Amount");
                dTable.Columns.Add("IsVoided");
                dTable.Columns.Add("IsSpecial");
                dTable.Columns.Add("ID");
                dTable.Columns.Add("IsFreeOfCharge");
                dTable.Columns.Add("IsPromo");

                Item myItem;
                decimal qty, unitprice;

                OrderDetCollection oDet = pos.FetchUnsavedOrderDet();
                for (int i = oDet.Count - 1; i >= 0; i--)
                {
                    dr = dTable.NewRow();
                    myItem = new Item(oDet[i].ItemNo);
                    dr["ItemNo"] = myItem.ItemNo;
                    dr["ItemName"] = myItem.ItemName;
                    dr["CategoryName"] = myItem.CategoryName;
                    dr["ItemDesc"] = myItem.ItemDesc;
                    qty = oDet[i].Quantity.GetValueOrDefault(0);
                    unitprice = oDet[i].UnitPrice;
                    dr["Quantity"] = qty.ToString("N2");
                    dr["Price"] = unitprice.ToString("N");
                    dr["IsVoided"] = CommonUILib.GetYesOrNoFromBool(oDet[i].IsVoided);
                    dr["IsSpecial"] = CommonUILib.GetYesOrNoFromBool(oDet[i].IsSpecial);
                    dr["ID"] = oDet[i].OrderDetID;

                    dr["IsPromo"] = CommonUILib.GetYesOrNoFromBool(oDet[i].IsPromo);
                    if (oDet[i].IsPromo)
                    {
                        dr["Amount"] = ((decimal)oDet[i].PromoAmount).ToString("N");
                        dr["Disc(%)"] = ((decimal)oDet[i].PromoDiscount).ToString("N2");
                        dr["IsFreeOfCharge"] = CommonUILib.GetYesOrNoFromBool(oDet[i].IsPromoFreeOfCharge);
                    }
                    else
                    {
                        dr["Amount"] = oDet[i].Amount.ToString("N");
                        dr["IsFreeOfCharge"] = CommonUILib.GetYesOrNoFromBool(oDet[i].IsFreeOfCharge);
                        dr["Disc(%)"] = oDet[i].Discount.ToString("N2");
                    }

                    dTable.Rows.Add(dr);
                }
                return dTable;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
        }
        /*
        #region "DataGridView"
        private void dgvPurchase_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            try
            {
                for (int i = 0; i < dgvPurchase.ColumnCount; i++)
                {
                    if (dgvPurchase.Columns[i].Visible && bool.Parse(dgvPurchase.Rows[e.RowIndex].Cells["IsVoided"].Value.ToString()) == true)
                    {
                        dgvPurchase.Rows[e.RowIndex].Cells[i].Style.ForeColor = System.Drawing.Color.White;
                        dgvPurchase.Rows[e.RowIndex].Cells[i].Style.BackColor = System.Drawing.Color.DarkRed;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("An unknown error has been encountered. Please contact your system administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void populateOrderItemGridView()
        {
            try
            {
                //Where to get the info? POS Controllers?

            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("An unknown error has been encountered. Please contact your system administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        #endregion              
        */
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvLineItem_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            try
            {
                for (int i = 0; i < dgvLineItem.ColumnCount; i++)
                {
                    if (dgvLineItem.Columns[i].Visible &&
                        String.Compare(dgvLineItem.Rows[e.RowIndex].Cells["IsVoided"].Value.ToString(), "yes", true) == 0)
                    {
                        dgvLineItem.Rows[e.RowIndex].Cells[i].Style.ForeColor = System.Drawing.Color.White;
                        dgvLineItem.Rows[e.RowIndex].Cells[i].Style.BackColor = System.Drawing.Color.DarkRed;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("An unknown error has been encountered. Please contact your system administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnReprint_Click(object sender, EventArgs e)
        {
            frmKeypad f = new frmKeypad();
            f.textMessage = "NUMBER OF COPIES";
            f.initialValue = "1";
            f.IsInteger = true;
            f.ShowDialog();

            if (f.DialogResult == DialogResult.Cancel) return;

            int numOfCopies = 1;
            if (!int.TryParse(f.value, out numOfCopies))
            {
                return;
            }
            else
            {
                if (numOfCopies > 10)
                {
                    MessageBox.Show("Please do not print more than 10 copies");
                }
                else
                {
                    //tryDownloadPoints("");
                    POSDeviceController.PrintAHAVATransactionReceipt(pos, 0, true,
                        (ReceiptSizes)Enum.ToObject(typeof(ReceiptSizes),
                            PrintSettingInfo.receiptSetting.PaperSize.Value), numOfCopies);
                }
            }
        }

        private void btnVoid_Click(object sender, EventArgs e)
        {
            try
            {
                if (isFromOtherOutlet)
                {

                    MessageBox.Show("Cannot void transaction from another outlet.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;

                }

                if (pos.IsVoided())
                {
                    MessageBox.Show("This order is already voided.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (pos.HasReturnedItems())
                {
                    MessageBox.Show("Cannot void Refund Transaction.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                bool isRefunded = false;
                if (!AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Refund.RefundReceiptFromSameOutlet), false))
                {
                    foreach (OrderDet od in pos.myOrderDet)
                    {
                        decimal qtyRefunded = 0;
                        if (POSController.isRefunded(od.OrderDetID, out qtyRefunded) || (od.Quantity < 0 && od.Item.Category.CategoryName != "SYSTEM") || qtyRefunded > 0)
                        {
                            isRefunded = true;
                        }
                    }
                    if (isRefunded)
                    {
                        MessageBox.Show("Transaction already refunded, cannot void this transaction.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    string status = "";
                    DataSet ds = SyncClientController.GetOrderForRefund(pos.GetCustomizedRefNo(), PointOfSaleInfo.PointOfSaleID, true, out status);
                    if (ds == null || ds.Tables.Count == 0)
                    {
                        MessageBox.Show("Transaction already refunded, cannot void this transaction.");
                        return;
                    }
                    else
                    {
                        if (status != "")
                        {
                            MessageBox.Show("Transaction already refunded, cannot void this transaction.");
                            return;
                        }
                    }

                }

                #region *) Validation: Check whether there's insufficient Points for refund
                if (pos.HasOrderedPointPackageItems())
                {
                    string status;
                    string msg = "{0} cannot be refunded because remaining points is insufficient (Refund: {1}, Remaining: {2}).";
                    DataTable CurrentAmount = new DataTable();
                    if (PowerPOS.Feature.Package.GetCurrentAmount(pos.GetMemberInfo().MembershipNo, pos.myOrderHdr.OrderDate, out CurrentAmount, out status))
                    {
                        // Include point changes from PointTempLog into the CurrentAmounts.
                        // (if PointTempLog got data that means the point has not been synced to posdb yet)
                        DataTable dtPTL;
                        PowerPOS.Feature.Package.AllocateNewlyEarnedPointsToCurrentAmounts(CurrentAmount, pos.myOrderHdr.MembershipNo, out dtPTL, out status);
                        CurrentAmount = dtPTL.Copy();

                        if (CurrentAmount.Rows.Count > 0)
                        {
                            List<string> insufficient = new List<string>();

                            foreach (OrderDet od in pos.GetOrderItemPointPackage())
                            {
                                if (od.Userflag5.GetValueOrDefault(false) == true) continue; //use package not validated

                                decimal pointToRefund = Math.Abs(od.Item.PointGetAmount == 0 ? od.Amount : od.Quantity.GetValueOrDefault(0) * od.Item.PointGetAmount);

                                if (od.Item.IsOpenPricePackage)
                                {
                                    pointToRefund = od.PointGetAmount;
                                }

                                DataRow[] rows = CurrentAmount.Select("RefNo='" + od.Item.ItemName + "'");
                                if (rows.Length > 0)
                                {
                                    decimal pointAvailable = 0;
                                    decimal.TryParse(rows[0]["Points"].ToString(), out pointAvailable);
                                    if (pointToRefund > pointAvailable)
                                        insufficient.Add(string.Format(msg, od.Item.ItemName, pointToRefund.ToString("N"), pointAvailable.ToString("N")));
                                }
                                else
                                {
                                    insufficient.Add(string.Format(msg, od.Item.ItemName, pointToRefund.ToString("N"), "0.00"));
                                }
                            }

                            if (insufficient.Count > 0)
                            {
                                MessageBox.Show(insufficient.ToArray().AsSingleLineString(Environment.NewLine));
                                return;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Cannot refund point/package item because currently there's no points/package available for this member.");
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show(status);
                        return;
                    }
                }
                #endregion

                DialogResult drResult =
                    MessageBox.Show("Are you sure? This action cant be undone.", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);


                if (drResult == DialogResult.No)
                {
                    return;
                }

                bool IsAuthorized;
                string SupID = "-";
                #region *) Authorization: Check Supervisor ID
                bool Prompt = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PromptPassword.OnVoid), true);

                if (!PrivilegesController.HasPrivilege(PrivilegesController.VOID_BILL, UserController.FetchGroupPrivilegesWithUsername(UserInfo.role, UserInfo.username)))
                {
                    string useMagenticStripReader = AppSetting.GetSetting(AppSetting.SettingsName.UseMagneticStripReader.ForAuthorizing);
                    if (useMagenticStripReader != null && useMagenticStripReader.ToLower() == "yes")
                    {

                        frmReadMSR f = new frmReadMSR();
                        f.privilegeName = PrivilegesController.VOID_BILL;
                        f.loginType = LoginType.Authorizing;
                        f.ShowDialog();
                        IsAuthorized = f.IsAuthorized;
                        f.Dispose();
                    }
                    else
                    {
                        //Ask for verification....
                        //Prompt Supervisor Password
                        LoginForms.frmSupervisorLogin sl = new LoginForms.frmSupervisorLogin();
                        sl.privilegeName = PrivilegesController.VOID_BILL;
                        sl.ShowDialog();
                        SupID = sl.mySupervisor;
                        IsAuthorized = sl.IsAuthorized;
                    }
                }
                else
                { IsAuthorized = true; }
                #endregion



                if (IsAuthorized)
                {
                    string Remarks = "";
                    //prompt remark as for void reason!

                    string selectableRemark = AppSetting.GetSetting(AppSetting.SettingsName.Invoice.SelectableVoidReason);

                    if (string.IsNullOrEmpty(selectableRemark))
                    {
                        frmVoidRemark fRemark = new frmVoidRemark();
                        fRemark.pos = pos;
                        CommonUILib.displayTransparent();
                        fRemark.ShowDialog();
                        CommonUILib.hideTransparent();
                        if (fRemark.IsSuccess)
                            Remarks = fRemark.pos.myOrderHdr.Remark;
                        else
                        {
                            fRemark.Dispose();
                            return;
                        }
                        fRemark.Dispose();
                    }
                    else
                    {

                        frmSelectVoidReason fSelecReason = new frmSelectVoidReason();
                        fSelecReason.pos = pos;
                        CommonUILib.displayTransparent();
                        fSelecReason.ShowDialog();
                        CommonUILib.hideTransparent();
                        if (fSelecReason.IsSuccess)
                            Remarks = fSelecReason.Reason;
                        else
                        {
                            fSelecReason.Dispose();
                            return;
                        }
                        fSelecReason.Dispose();
                    }


                    string status = "";
                    MallIntegrationController mc = new MallIntegrationController(pos.myOrderHdr.OrderHdrID);
                    if (POSController.VoidReceipt(pos.GetSavedRefNo(), UserInfo.username, SupID, Remarks))
                    {
                        AccessLogController.AddAccessLog(AccessSource.POS, UserInfo.username, "-", "VOID Receipt : " + pos.GetSavedRefNo(), "");
                        pos.myOrderHdr.IsVoided = true;

                        bool isOutletSales = false;
                        if (pos.GetMemberInfo() != null && pos.GetMemberInfo().MembershipNo != "" && pos.GetMemberInfo().MembershipNo != "WALK-IN")
                        {
                            //validation if outlet order
                            PointOfSaleCollection posColl1 = new PointOfSaleCollection();
                            posColl1.Where(PointOfSale.UserColumns.LinkedMembershipNo, pos.GetMemberInfo().MembershipNo);
                            posColl1.Load();
                            if (posColl1.Count > 0)
                                isOutletSales = true;

                        }

                        #region
                        if (pos.GetOrderDate() < POSController.FetchLatestCloseCounterTime(pos.GetPointOfSaleId()))
                        {
                            AccessLogController.AddAccessLog(AccessSource.POS, UserInfo.username, "-", "Late VOID", pos.GetSavedRefNo() + " - " + pos.myOrderHdr.NettAmount);
                            AccessLogController.AddLogHelper(PointOfSaleInfo.PointOfSaleID, AccessSource.POS, UserInfo.username, "-", "Late VOID", pos.GetSavedRefNo() + " - " + pos.myOrderHdr.NettAmount);
                            // change COunterCloseAount
                            CounterCloseLogCollection CCLColl = new CounterCloseLogCollection();
                            CCLColl.Where(CounterCloseLog.StartTimeColumn.ColumnName, SubSonic.Comparison.LessOrEquals, pos.myOrderHdr.OrderDate);
                            CCLColl.Where(CounterCloseLog.EndTimeColumn.ColumnName, SubSonic.Comparison.GreaterOrEquals, pos.myOrderHdr.OrderDate);
                            CCLColl.Load();
                            if (CCLColl.Count > 0)
                            {
                                Query qrCCLUpdate = CounterCloseLog.CreateQuery();
                                qrCCLUpdate.QueryType = QueryType.Update;
                                qrCCLUpdate.AddWhere(CounterCloseLog.Columns.CounterCloseID, CCLColl[0].CounterCloseID);


                                var PaymentTypeUsed = "CASH";
                                var newAmount = 0;
                                // change payment type amount
                                foreach (var recDet in pos.recDet)
                                {


                                    //CashRecorded,NetsRecorded,VisaRecorded, AmexRecorded, ChinaNets, VoucherRecorded, TotalRecorded, NetsCashCard, NetsFlashPay, NetsATMCard, 
                                    if (recDet.PaymentType.ToUpper() == "CASH")
                                    {
                                        PaymentTypeUsed = "CASH";
                                        //CCLColl[0].CashRecorded=  CCLColl[0].CashRecorded-recDet.Amount;
                                        //CCLColl[0].TotalSystemRecorded = CCLColl[0].TotalSystemRecorded - recDet.Amount;
                                        //CCLColl[0].Variance = CCLColl[0].Variance - recDet.Amount;
                                        qrCCLUpdate.AddUpdateSetting(CounterCloseLog.Columns.CashRecorded, CCLColl[0].CashRecorded - recDet.Amount);
                                        qrCCLUpdate.AddUpdateSetting(CounterCloseLog.Columns.TotalSystemRecorded, CCLColl[0].TotalSystemRecorded - recDet.Amount);
                                        qrCCLUpdate.AddUpdateSetting(CounterCloseLog.Columns.Variance, CCLColl[0].Variance - recDet.Amount);
                                        //break;
                                    }
                                    if (recDet.PaymentType.ToUpper() == "NETS")
                                    {
                                        PaymentTypeUsed = "NETS";
                                        qrCCLUpdate.AddUpdateSetting(CounterCloseLog.Columns.NetsRecorded, CCLColl[0].NetsRecorded - recDet.Amount);
                                        qrCCLUpdate.AddUpdateSetting(CounterCloseLog.Columns.TotalSystemRecorded, CCLColl[0].TotalSystemRecorded - recDet.Amount);
                                        qrCCLUpdate.AddUpdateSetting(CounterCloseLog.Columns.Variance, CCLColl[0].Variance - recDet.Amount);
                                        //break;
                                    }
                                    if (recDet.PaymentType.ToUpper() == "VISA")
                                    {
                                        PaymentTypeUsed = "VISA";
                                        qrCCLUpdate.AddUpdateSetting(CounterCloseLog.Columns.VisaRecorded, CCLColl[0].NetsRecorded - recDet.Amount);
                                        qrCCLUpdate.AddUpdateSetting(CounterCloseLog.Columns.TotalSystemRecorded, CCLColl[0].TotalSystemRecorded - recDet.Amount);
                                        qrCCLUpdate.AddUpdateSetting(CounterCloseLog.Columns.Variance, CCLColl[0].Variance - recDet.Amount);
                                        //break;
                                    }
                                    if (recDet.PaymentType.ToUpper() == "AMEX")
                                    {
                                        PaymentTypeUsed = "AMEX";
                                        qrCCLUpdate.AddUpdateSetting(CounterCloseLog.Columns.AmexRecorded, CCLColl[0].AmexRecorded - recDet.Amount);
                                        qrCCLUpdate.AddUpdateSetting(CounterCloseLog.Columns.TotalSystemRecorded, CCLColl[0].TotalSystemRecorded - recDet.Amount);
                                        qrCCLUpdate.AddUpdateSetting(CounterCloseLog.Columns.Variance, CCLColl[0].Variance - recDet.Amount);
                                        //break;
                                    }
                                    if (recDet.PaymentType.ToUpper() == "CHINANETS")
                                    {
                                        PaymentTypeUsed = "CHINANETS";
                                        qrCCLUpdate.AddUpdateSetting(CounterCloseLog.Columns.ChinaNetsRecorded, CCLColl[0].ChinaNetsRecorded - recDet.Amount);
                                        qrCCLUpdate.AddUpdateSetting(CounterCloseLog.Columns.TotalSystemRecorded, CCLColl[0].TotalSystemRecorded - recDet.Amount);
                                        qrCCLUpdate.AddUpdateSetting(CounterCloseLog.Columns.Variance, CCLColl[0].Variance - recDet.Amount);
                                        //break;
                                    }
                                    if (recDet.PaymentType.ToUpper() == "VOUCHER")
                                    {
                                        PaymentTypeUsed = "VOUCHER";
                                        qrCCLUpdate.AddUpdateSetting(CounterCloseLog.Columns.VoucherRecorded, CCLColl[0].VoucherRecorded - recDet.Amount);
                                        qrCCLUpdate.AddUpdateSetting(CounterCloseLog.Columns.TotalSystemRecorded, CCLColl[0].TotalSystemRecorded - recDet.Amount);
                                        qrCCLUpdate.AddUpdateSetting(CounterCloseLog.Columns.Variance, CCLColl[0].Variance - recDet.Amount);
                                        //break;
                                    }
                                    if (recDet.PaymentType.ToUpper() == "NETSCASHCARD")
                                    {
                                        PaymentTypeUsed = "NETSCASHCARD";
                                        qrCCLUpdate.AddUpdateSetting(CounterCloseLog.Columns.NetsCashCardRecorded, CCLColl[0].NetsCashCardRecorded - recDet.Amount);
                                        qrCCLUpdate.AddUpdateSetting(CounterCloseLog.Columns.TotalSystemRecorded, CCLColl[0].TotalSystemRecorded - recDet.Amount);
                                        qrCCLUpdate.AddUpdateSetting(CounterCloseLog.Columns.Variance, CCLColl[0].Variance - recDet.Amount);
                                        //break;
                                    }
                                    if (recDet.PaymentType.ToUpper() == "NETSFLASHPAY")
                                    {
                                        PaymentTypeUsed = "NETSFLASHPAY";
                                        qrCCLUpdate.AddUpdateSetting(CounterCloseLog.Columns.NetsFlashPayRecorded, CCLColl[0].NetsFlashPayRecorded - recDet.Amount);
                                        qrCCLUpdate.AddUpdateSetting(CounterCloseLog.Columns.TotalSystemRecorded, CCLColl[0].TotalSystemRecorded - recDet.Amount);
                                        qrCCLUpdate.AddUpdateSetting(CounterCloseLog.Columns.Variance, CCLColl[0].Variance - recDet.Amount);
                                        //break;
                                    }
                                    if (recDet.PaymentType.ToUpper() == "NETSATMCARD")
                                    {
                                        PaymentTypeUsed = "NETSATMCARD";
                                        qrCCLUpdate.AddUpdateSetting(CounterCloseLog.Columns.NetsATMCardRecorded, CCLColl[0].NetsATMCardRecorded - recDet.Amount);
                                        qrCCLUpdate.AddUpdateSetting(CounterCloseLog.Columns.TotalSystemRecorded, CCLColl[0].TotalSystemRecorded - recDet.Amount);
                                        qrCCLUpdate.AddUpdateSetting(CounterCloseLog.Columns.Variance, CCLColl[0].Variance - recDet.Amount);
                                        //break;
                                    }
                                    if (recDet.PaymentType == PaymentTypesController.FetchPaymentByID("5"))
                                    {
                                        PaymentTypeUsed = "Pay5";
                                        qrCCLUpdate.AddUpdateSetting(CounterCloseLog.UserColumns.Pay5Recorded, CCLColl[0].Pay5Recorded - recDet.Amount);
                                        qrCCLUpdate.AddUpdateSetting(CounterCloseLog.Columns.TotalSystemRecorded, CCLColl[0].TotalSystemRecorded - recDet.Amount);
                                        qrCCLUpdate.AddUpdateSetting(CounterCloseLog.Columns.Variance, CCLColl[0].Variance - recDet.Amount);
                                    }
                                    if (recDet.PaymentType == PaymentTypesController.FetchPaymentByID("6"))
                                    {
                                        PaymentTypeUsed = "Pay6";
                                        qrCCLUpdate.AddUpdateSetting(CounterCloseLog.UserColumns.Pay6Recorded, CCLColl[0].Pay6Recorded - recDet.Amount);
                                        qrCCLUpdate.AddUpdateSetting(CounterCloseLog.Columns.TotalSystemRecorded, CCLColl[0].TotalSystemRecorded - recDet.Amount);
                                        qrCCLUpdate.AddUpdateSetting(CounterCloseLog.Columns.Variance, CCLColl[0].Variance - recDet.Amount);
                                    }
                                    if (recDet.PaymentType == PaymentTypesController.FetchPaymentByID("7"))
                                    {
                                        PaymentTypeUsed = "Pay7";
                                        qrCCLUpdate.AddUpdateSetting(CounterCloseLog.Columns.Pay7Recorded, CCLColl[0].Pay7Recorded - recDet.Amount);
                                        qrCCLUpdate.AddUpdateSetting(CounterCloseLog.Columns.TotalSystemRecorded, CCLColl[0].TotalSystemRecorded - recDet.Amount);
                                        qrCCLUpdate.AddUpdateSetting(CounterCloseLog.Columns.Variance, CCLColl[0].Variance - recDet.Amount);
                                    }
                                    if (recDet.PaymentType == PaymentTypesController.FetchPaymentByID("8"))
                                    {
                                        PaymentTypeUsed = "Pay8";
                                        qrCCLUpdate.AddUpdateSetting(CounterCloseLog.Columns.Pay8Recorded, CCLColl[0].Pay8Recorded - recDet.Amount);
                                        qrCCLUpdate.AddUpdateSetting(CounterCloseLog.Columns.TotalSystemRecorded, CCLColl[0].TotalSystemRecorded - recDet.Amount);
                                        qrCCLUpdate.AddUpdateSetting(CounterCloseLog.Columns.Variance, CCLColl[0].Variance - recDet.Amount);
                                    }
                                    if (recDet.PaymentType == PaymentTypesController.FetchPaymentByID("9"))
                                    {
                                        PaymentTypeUsed = "Pay9";
                                        qrCCLUpdate.AddUpdateSetting(CounterCloseLog.Columns.Pay9Recorded, CCLColl[0].Pay9Recorded - recDet.Amount);
                                        qrCCLUpdate.AddUpdateSetting(CounterCloseLog.Columns.TotalSystemRecorded, CCLColl[0].TotalSystemRecorded - recDet.Amount);
                                        qrCCLUpdate.AddUpdateSetting(CounterCloseLog.Columns.Variance, CCLColl[0].Variance - recDet.Amount);
                                    }
                                    if (recDet.PaymentType == PaymentTypesController.FetchPaymentByID("10"))
                                    {
                                        PaymentTypeUsed = "Pay10";
                                        qrCCLUpdate.AddUpdateSetting(CounterCloseLog.Columns.Pay10Recorded, CCLColl[0].Pay10Recorded - recDet.Amount);
                                        qrCCLUpdate.AddUpdateSetting(CounterCloseLog.Columns.TotalSystemRecorded, CCLColl[0].TotalSystemRecorded - recDet.Amount);
                                        qrCCLUpdate.AddUpdateSetting(CounterCloseLog.Columns.Variance, CCLColl[0].Variance - recDet.Amount);
                                    }
                                }

                                qrCCLUpdate.Execute();

                            }
                        }
                        #endregion


                        #region *) Try To Sync to server if is enabled
                        if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSales), false))
                        {
                            if (isOutletSales)
                            {
                                if (!SyncClientController.SendRealTimeSalesByOrderHdrID(pos, pos.myOrderHdr.OrderHdrID, out status))
                                {
                                    MessageBox.Show(status);
                                }
                            }
                            else
                            {
                                if (!SyncSalesThread.IsBusy)
                                    SyncSalesThread.RunWorkerAsync();
                            }
                        }
                        #endregion

                        //tryDownloadPoints(pos.myOrderHdr.OrderHdrID);

                        POSDeviceController.PrintAHAVATransactionReceipt(pos, 0, true,
                        (ReceiptSizes)Enum.ToObject(typeof(ReceiptSizes),
                        PrintSettingInfo.receiptSetting.PaperSize.Value), 1);
                        MessageBox.Show(LanguageManager.Order_voided_successfully_, "", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        #region *) External Integration
                        if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.MallIntegration.useNUHMallIntegration), false))
                        {
                            //run mall integration

                            if (!mc.GenerateVoid(out status))
                            {
                                MessageBox.Show("Generate Integration File was not successful. Please recreate the file.");

                            }
                        }
                        #endregion
                        #region *) Magento Integration
                        /*if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Magento.UseMagentoFeatures), false))
                        {
                            //run cancel 
                            string status = "";
                            if (!pos.Magento_CancelOrder(out status))
                            {
                                MessageBox.Show("Failed to Update Order." + status);
                            }
                        }*/
                        #endregion
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Unable to void order.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show(ex.Message);
                return;
            }

        }

        private void tryDownloadPoints(string orderhdrid)
        {

            bool overwriteSetting = !AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Print.HidePrintPackageBalanceOnReceipt), true);
            if ((PowerPOS.Feature.Package.isAvailable && AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Points.WaitToDownloadPointsBeforePrintReceipt), false)) || overwriteSetting)
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
                        fDownloadPoints.orderHdrID = orderhdrid;
                        fDownloadPoints.ShowDialog();
                        if (!fDownloadPoints.IsSuccessful)
                            MessageBox.Show("Latest Point Data is not downloaded yet. Showing the latest point data in the receipt.");

                    }
                }

            }
        }

        private void frmViewBillDetail_Activated(object sender, EventArgs e)
        {

        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            //
            if (pos.IsVoided())
            {
                MessageBox.Show("This order is already voided.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // CHECK WHETHER NORMAL OR FORMAL INVOICE
            string status = "";
            if (pos.IsFormalInvoice())
            {
                frmSelectPaymentFormal f = new frmSelectPaymentFormal();
                f.pos = pos;
                f.amount = pos.CalculateTotalAmount(out status);
                f.syncSalesThread = SyncSalesThread;
                f.IsEdit = true;
                f.ShowDialog();

                if (f.isSuccessful)
                {
                    #region *) Try To Sync to server if is enabled
                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSales), false))
                        if (!SyncSalesThread.IsBusy)
                            SyncSalesThread.RunWorkerAsync();
                    #endregion
                    AccessLogController.AddLog(AccessSource.POS, UserInfo.username, "-", "UPDATE Payment of Receipt : " + pos.GetSavedRefNo(), "");
                    frmViewBillDetail_Load(this, new EventArgs());
                    MessageBox.Show("Save successful");
                }

            }
            else
            {
                frmPartialPayment f = new frmPartialPayment();
                f.pos = pos;
                f.IsEdit = true;
                f.ShowDialog();

                if (f.IsSuccessful)
                {
                    #region *) Try To Sync to server if is enabled
                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSales), false))
                        if (!SyncSalesThread.IsBusy)
                            SyncSalesThread.RunWorkerAsync();
                    #endregion
                    AccessLogController.AddLog(AccessSource.POS, UserInfo.username, "-", "UPDATE Payment of Receipt : " + pos.GetSavedRefNo(), "");
                    frmViewBillDetail_Load(this, new EventArgs());
                    MessageBox.Show("Save successful");
                }
            }
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            //Get the previous Order ID before this
            string tmp = GetPrevOrderNo(OrderHdrID);
            if (tmp != "")
            {
                OrderHdrID = tmp;
                frmViewBillDetail_Load(this, new EventArgs());
            }
        }

        private string GetPrevOrderNo(string OrderHdrID)
        {
            if (OrderHdrID != "")
            {
                OrderHdr oh = new OrderHdr(OrderHdrID);

                //get yesterday one
                Query q = OrderHdr.CreateQuery();
                object ob = q.WHERE(OrderHdr.Columns.CreatedOn, Comparison.LessThan, oh.CreatedOn).GetMax(OrderHdr.Columns.OrderHdrID);
                if (ob != null)
                {
                    return ob.ToString();
                }

                //string sqlString = "Select OrderHdrID from OrderHdr where modifiedon < '" +
                //    oh.ModifiedOn.ToString("yyyy-MM-dd HH:mm:ss.fff") + "' order by modifiedon DESC ";
                /*string sqlString = "Select OrderHdrID from OrderHdr where OrderDate < '" +
                   oh.OrderDate.ToString("yyyy-MM-dd HH:mm:ss.fff") + "' order by OrderDate DESC ";
                DataTable dt = DataService.GetDataSet(new QueryCommand(sqlString)).Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    return dt.Rows[0]["OrderHdrID"].ToString();
                }*/
            }
            return "";
        }

        private string GetNextOrderNo(string OrderHdrID)
        {
            if (OrderHdrID != "")
            {
                OrderHdr oh = new OrderHdr(OrderHdrID);
                //get yesterday one
                /*Query q = OrderHdr.CreateQuery();
                object ob = q.WHERE(OrderHdr.Columns.ModifiedOn,
                    Comparison.GreaterThan, oh.ModifiedOn).GetMin(OrderHdr.Columns.OrderHdrID);*/
                string sqlString = "Select OrderHdrID from OrderHdr where createdon > '" +
                    oh.CreatedOn.ToString("yyyy-MM-dd HH:mm:ss.fff") + "' order by createdon asc ";
                //string sqlString = "Select OrderHdrID from OrderHdr where OrderDate > '" +
                //    oh.OrderDate.ToString("yyyy-MM-dd HH:mm:ss.fff") + "' order by OrderDate asc ";
                DataTable dt = DataService.GetDataSet(new QueryCommand(sqlString)).Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    return dt.Rows[0]["OrderHdrID"].ToString();
                }
            }
            return "";
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            string tmp = GetNextOrderNo(OrderHdrID);
            if (tmp != "")
            {
                OrderHdrID = tmp;
                frmViewBillDetail_Load(this, new EventArgs());
            }
        }

        private void btnWarranty_Click(object sender, EventArgs e)
        {
            WarrantyForms.frmCreateWarranty f = new WinPowerPOS.WarrantyForms.frmCreateWarranty();
            f.membershipNo = pos.GetMemberInfo().MembershipNo;
            f.orderHdrID = pos.GetSavedRefNo().Substring(2);
            f.ShowDialog();
            f.Dispose();
        }
        /*
        private void btnInstallment_Click(object sender, EventArgs e)
        {
            frmCreateInstallment f = new frmCreateInstallment();
            f.pos = pos;
            f.ShowDialog();
        }*/

        private void lblMembershipNo_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(lblMembershipNo.Text)) return;
            Membership member = new Membership(Membership.Columns.MembershipNo, lblMembershipNo.Text);
            if (!member.IsNew)
            {
                using (frmNewMembershipEdit f = new frmNewMembershipEdit(member))
                {
                    f.IsReadOnly = false;
                    f.ShowDialog();
                    f.Dispose();
                }
            }
        }

        private void btnRemark_Click(object sender, EventArgs e)
        {
            using (frmRemark frm = new frmRemark())
            {
                try
                {
                    OrderHdr myOrderHdr = new OrderHdr(OrderHdr.Columns.OrderRefNo, lblRefNo.Text);
                    if (myOrderHdr.IsLoaded)
                    {
                        if (string.IsNullOrEmpty(myOrderHdr.OrderHdrID))
                        {
                            myOrderHdr = new OrderHdr(OrderHdr.Columns.Userfld5, lblRefNo.Text);
                        }

                        DateTime orderDate = myOrderHdr.OrderDate;
                        DateTime closingDate = POSController.FetchLatestCloseCounterTime(PointOfSaleInfo.PointOfSaleID);

                        bool allowAddRemarkAfterClosing = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Invoice.AllowAddRemarkAfterClosing), false);
                        if (orderDate < closingDate && !allowAddRemarkAfterClosing)
                            return;

                        if (myOrderHdr.IsVoided)
                            return;

                        //frm.txtRemark.Text = pos.GetHeaderRemark();
                        frm.ShowDialog();
                        if (frm.IsSuccessful)
                        {
                            string originalRemark = lblRemark.Text;
                            frm.txtRemark.Text = frm.txtRemark.Text.Trim(new char[] { ' ', '\n', '\r' });
                            if (!string.IsNullOrEmpty(frm.txtRemark.Text))
                            {
                                myOrderHdr.IsNew = false;
                                if (lblRemark.Text.Trim() == "-") lblRemark.Text = "";
                                lblRemark.Text = lblRemark.Text + " " + frm.txtRemark.Text;
                                myOrderHdr.Remark = lblRemark.Text;

                                #region *) Update remark to server
                                bool success = false;
                                bool useRealTimeSales = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSales), false);
                                if (useRealTimeSales || PointOfSaleInfo.IntegrateWithInventory)
                                {
                                    success = true; // No need to sync to server
                                }
                                else
                                {
                                    success = SyncClientController.UpdateOrderHdrRemarkToServer(myOrderHdr.OrderHdrID, lblRemark.Text);
                                }
                                #endregion

                                if (success)
                                {
                                    SubSonic.DataService.ExecuteQuery(myOrderHdr.GetUpdateCommand(UserInfo.username));
                                    frmViewBillDetail_Load(this, new EventArgs());
                                }
                                else
                                {
                                    MessageBox.Show("Failed to update data in server. Remark is not saved.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    lblRemark.Text = originalRemark;
                                    return;
                                }
                            }
                        }
                    }
                    frm.Dispose();
                }
                catch (Exception ex)
                {
                    Logger.writeLog(ex);
                    MessageBox.Show("An unknown error has been encountered. Please contact your system administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnChangeMember_Click(object sender, EventArgs e)
        {
            try
            {
                OrderHdr myOrderHdr = new OrderHdr(OrderHdr.Columns.OrderRefNo, lblRefNo.Text);
                #region *) Validation: OrderHdr is exists in database
                if (!myOrderHdr.IsLoaded) return;
                #endregion

                #region *) Validation: OrderHdr has not been settled
                if (myOrderHdr.OrderDate < POSController.FetchLatestCloseCounterTime(PointOfSaleInfo.PointOfSaleID))
                {
                    MessageBox.Show("Cannot make any change. This order has been settled.");
                    return;
                }
                #endregion

                #region *) Validation: OrderHdr is not Voided
                if (myOrderHdr.IsVoided)
                {
                    MessageBox.Show("Cannot make any change. This order has been settled.");
                    return;
                }
                #endregion

                #region *) Do Confirmation
                DialogResult drResult =
                    MessageBox.Show(
                        "Membership Discount remain unchanged\n" +
                        "Points are not assigned to new member\n" +
                        "You have to allocate manually.\n" +
                        "Are you sure to change member?", "Warning!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (drResult == DialogResult.No) return;
                #endregion

                bool IsAuthorized;
                //check setting change member without supervisor login
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Order.ChangeMemberWithoutSupervisorLogin), false))
                {
                    IsAuthorized = true;
                }
                else
                {
                    #region *) Verification: Prompt Supervisor Login
                    LoginForms.frmSupervisorLogin sl = new LoginForms.frmSupervisorLogin();
                    sl.privilegeName = PrivilegesController.VOID_BILL;
                    sl.ShowDialog();
                    IsAuthorized = sl.IsAuthorized;
                    #endregion
                }




                #region *) Validation: Authorization success
                if (!IsAuthorized) return;
                #endregion

                string NewMembershipNo;
                #region *) Input: Get new MembershipNo
                /// Cancel button will return Empty string
                NewMembershipNo = Microsoft.VisualBasic.Interaction.InputBox("Please scan new Membership No.", "Membership Change", "", 0, 0);
                #endregion

                #region *) Validation: New MembershipNo is Not Empty
                if (string.IsNullOrEmpty(NewMembershipNo))
                    return;
                #endregion

                bool isRegistered = false;
                #region *) Validation: MembershipNo is registered in DataBase [Member / NRIC]
                bool hasExpired;
                DateTime ExpiryDate;
                if (MembershipController.IsExistingMember(NewMembershipNo, out hasExpired, out ExpiryDate))
                {
                    isRegistered = true;
                }
                else
                {
                    if (hasExpired)
                    {
                        MessageBox.Show("This member has already expired on " + ExpiryDate.ToString("dd MMM yyyy") + ".", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else
                    {
                        //Check NRIC
                        Membership member;
                        if (MembershipController.IsMembersNRIC(NewMembershipNo, out member))
                        {

                            if (member.ExpiryDate.HasValue && member.ExpiryDate.Value >= DateTime.Now)
                            {
                                //havent expired
                                isRegistered = true;
                                NewMembershipNo = member.MembershipNo;
                            }
                            else if (member.ExpiryDate.HasValue)
                            {
                                MessageBox.Show("This member has already expired on " + member.ExpiryDate.Value.ToString("dd MMM yyyy") + ".", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }
                }

                if (!isRegistered)
                {
                    MessageBox.Show("Membership is not found");
                    return;
                }
                #endregion

                #region *) Core: Update OrderHdr's MembershipNo
                myOrderHdr.MembershipNo = NewMembershipNo;
                SubSonic.DataService.ExecuteQuery(myOrderHdr.GetUpdateCommand(UserInfo.username));

                frmViewBillDetail_Load(this, new EventArgs());
                #endregion

                #region *) Try To Sync to server if is enabled
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSales), false))
                    if (!SyncSalesThread.IsBusy)
                        SyncSalesThread.RunWorkerAsync();
                #endregion
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("An unknown error has been encountered. Please contact your system administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnShowRemark_Click(object sender, EventArgs e)
        {
            MessageBox.Show(lblRemark.Text, "Remarks Information", MessageBoxButtons.OK);
        }

        private void btnPrintSticker_Click(object sender, EventArgs e)
        {
            POSDeviceController.PrintAgapeBabySticker(pos, 1);
        }

        private void btnEmailReceipt_Click(object sender, EventArgs e)
        {
            var member = pos.CurrentMember;
            frmMailInput frm = new frmMailInput();
            if (member != null)
            {
                if (!string.IsNullOrEmpty(member.Email))
                    frm.EmailTo = member.Email;
                else
                    frm.EmailTo = AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_DefaultMailTo);

                string useForReceiptNo = AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_ReceiptNoInEmailReceipt);
                string receiptNo = "";
                if (string.IsNullOrEmpty(useForReceiptNo) || useForReceiptNo.ToLower() == "orderhdrid")
                    receiptNo = pos.myOrderHdr.OrderHdrID;
                else if (useForReceiptNo.ToLower() == "custom invoice no")
                    receiptNo = pos.myOrderHdr.Userfld5;
                else if (useForReceiptNo.ToLower() == "line info")
                    receiptNo = pos.GetLineInfoOfFirstLine();
                else
                    receiptNo = pos.myOrderHdr.OrderHdrID;

                frm.EmailSubject = string.Format("Receipt {0} for purchase at {1}", receiptNo, CompanyInfo.CompanyName);
                frm.EmailBody = "Please find the receipt attachment";
            }

            #region *) Send BCC if necessary
            bool sendBcc = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_BccToOwner), false);
            string ownerEmail = AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_OwnerEmailAddress);
            if (sendBcc && !string.IsNullOrEmpty(ownerEmail))
            {
                frm.EmailBcc = ownerEmail;
            }
            #endregion

            frm.ButtonSendClicked += new frmMailInput.ButtonSendClickHandler(frm_ButtonSendClicked);
            frm.Show();
        }

        protected void frm_ButtonSendClicked(object sender, SendMailArgs e)
        {
            if (POSDeviceController.SendMailReceipt(pos, e.MailTo, e.MailSubject, e.MailContent, e.MailBcc))
                MessageBox.Show("Send Email Success");
            else
                MessageBox.Show("Send Email Failed");
        }

        private void btnResend_Click(object sender, EventArgs e)
        {
            MallIntegrationController mc = new MallIntegrationController(pos.myOrderHdr.OrderHdrID);
            #region *) External Integration
            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.MallIntegration.useNUHMallIntegration), false))
            {
                //run mall integration
                string status = "";

                if (!mc.GenerateFile(out status))
                {
                    MessageBox.Show("Generate Integration File was not successful. Please recreate the file.");

                }
                else
                {
                    MessageBox.Show("Resend File Successful.");
                }
            }
            #endregion
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmKeypad f = new frmKeypad();
            f.textMessage = "NUMBER OF COPIES";
            f.initialValue = "1";
            f.IsInteger = true;
            f.ShowDialog();

            if (f.DialogResult == DialogResult.Cancel) return;

            int numOfCopies = 1;
            if (!int.TryParse(f.value, out numOfCopies))
            {
                return;
            }
            else
            {
                if (numOfCopies > 10)
                {
                    MessageBox.Show("Please do not print more than 10 copies");
                }
                else
                {
                    //tryDownloadPoints("");
                    POSDeviceController.PrintFormalAHAVATransactionReceipt(pos, 0, true,
                        (ReceiptSizes)Enum.ToObject(typeof(ReceiptSizes),
                            PrintSettingInfo.receiptSetting.PaperSize.Value), numOfCopies, true);
                }
            }
        }

        private void btnExportToInventoryForma_Click(object sender, EventArgs e)
        {
            try
            {
                CommonUILib.displayTransparent();
                if (saveFileDialogExport.ShowDialog() == DialogResult.OK)
                {
                    ExportController.ExportToExcel(pos.getInventoryItemsForExport(), saveFileDialogExport.FileName);
                    MessageBox.Show("Save Successful");
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog("Error Exporting Sales." + ex.Message);
                MessageBox.Show("Error Exporting Sales.");
            }
            finally
            {
                CommonUILib.hideTransparent();
            }
        }

        private void dgvLineItem_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0)
            {
                if (e.ColumnIndex == 10 && e.RowIndex >= 0)
                {

                    if (pos.GetOrderDate() <
                            POSController.FetchLatestCloseCounterTime
                            (PointOfSaleInfo.PointOfSaleID))
                        return;

                    if (pos.IsVoided())
                        return;
                    string status = "";
                    string LineID = dgvLineItem.Rows[e.RowIndex].Cells["ID"].Value.ToString();
                    //pop out line remark and sales person

                    OrderDet det = pos.GetLine(LineID, out status);

                    frmLineCommission f = new frmLineCommission(det.ItemNo);
                    f.Remark = det.Remark;
                    f.UserName = det.Userfld1;
                    f.UserName2 = det.SalesPerson2;
                    f.OldReceiptNo = det.ReturnedReceiptNo;

                    if (det.Quantity < 0)
                    {
                        f.IsSalesReturn = true;
                    }

                    f.ShowDialog();
                    if (f.IsSuccessful)
                    {
                        //assign to Line ID
                        det.Userfld1 = f.UserName;
                        //pos.SetLineRemark(LineID, f.Remark, out status);
                        det.SalesPerson2 = f.UserName2;
                        det.Userfld4 = f.LineInfoRemark.Trim();

                        SubSonic.DataService.ExecuteQuery(det.GetUpdateCommand(UserInfo.username));
                        SubSonic.DataService.ExecuteQuery(pos.myOrderHdr.GetUpdateCommand(UserInfo.username));
                        frmViewBillDetail_Load(this, new EventArgs());
                        //pos.AssignSalesReturnReceiptNo(LineID, f.OldReceiptNo, out status);
                        //ItemReturn = f.OldReceiptNo;

                        #region *) Try To Sync to server if is enabled
                        if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSales), false))
                            if (!SyncSalesThread.IsBusy)
                                SyncSalesThread.RunWorkerAsync();
                        #endregion
                    }
                    f.Dispose();
                }
            }
        }

        #region For Printing Validation
        public void CheckValidationAndPrint()
        {
            //only if item with isCommission true then we will print validation
            bool isComm = false;
            OrderDetCollection det = pos.FetchUnsavedOrderDet();
            for (int i = 0; i < det.Count; i++)
            {
                isComm = Convert.ToBoolean(det[i].Item.IsCommission);

                //found
                if (isComm == true)
                    break;
            }

            //keep asking to print validation to continue, else cannot continue
            bool valid = false;
            if (isComm == true)
            {
                //while (!valid)
                //{
                //ask if user wants to print validation
                //DialogResult dr = MessageBox.Show("Do you want to print validation?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                //if (dr == DialogResult.Yes) //print validation
                //{
                //print validation
                POSDeviceController.PrintAHAVAValidationReceipt(pos, 0, false,
                    ReceiptSizes.A4, //A4 Size validation
                    PrintSettingInfo.receiptSetting.NumOfCopies.Value);

                valid = true;
                //}
                //else
                //{
                //    valid = false;
                //MessageBox.Show("Please print validation to proceed!");
                //}
                //}
            }
        }
        #endregion

        private void btnReprintValidation_Click(object sender, EventArgs e)
        {
            CheckValidationAndPrint();
        }


    }
}