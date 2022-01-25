using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SubSonic;
using PowerPOS;
using PowerPOS.Container;
using System.Configuration;
using POSDevices;
using Features = PowerPOS.Feature;
using System.Collections;
using WinPowerPOS.OrderForms;
using System.Net;
using System.Collections.Specialized;
using PowerPOSLib.Container;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WinPowerPOS.MembershipForms
{
    public partial class frmPurchaseSummary : Form
    {
        private string membershipno;
        private string nametoappear;
        private bool allowSplitDelivery;
        
        public decimal PreferedDiscount = 0;
        public bool ApplyPromo = false;
        public bool IsUsePackage = false;
        public BackgroundWorker syncSalesThread;
        public BackgroundWorker SyncSendDeliveryOrderThread;
        public BackgroundWorker ParentSyncPointsThread;

        public POSController MainPOS;

        DateTime timeSyncPaymentBalanceStarted;
        DateTime timeSyncPointStarted;
        DateTime timeSyncTransactionStarted;

        public frmPurchaseSummary(string MembershipNo, string NameToAppear)
        {
            membershipno = MembershipNo;
            nametoappear = NameToAppear;
            allowSplitDelivery = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Order.AllowSplitDelivery), false);
            IsUsePackage = false;
            InitializeComponent();
        }

        private void syncMembershipPaymentBalance()
        {
            string serverUrl = SyncClientController.WS_URL.ToLower().Replace("synchronization/synchronization.asmx", "");
            string url = serverUrl + "API/Sales/UpdateBalancePayment/GetUpdatedInfo.ashx";
            using (WebClient webClient = new WebClient())
            {
                NameValueCollection parameters = new NameValueCollection();
                parameters.Add("MembershipNo", this.membershipno);
                byte[] responsebytes = webClient.UploadValues(url, "POST", parameters);
                string jsonDataRaw = Encoding.UTF8.GetString(responsebytes);
                var jsonData = JsonConvert.DeserializeObject<UpdateBalancePaymentModel>(jsonDataRaw);

                if (jsonData.OrderHdrList != null)
                {
                    var orderHdrList = JsonConvert.DeserializeObject<List<OrderHdr>>(jsonData.OrderHdrList.ToString());

                    foreach (OrderHdr data in orderHdrList)
                    {
                        string query = "select count(*) from OrderHdr a where a.OrderHdrID = '" + data.OrderHdrID + "'";
                        QueryCommand cmd = new QueryCommand(query);
                        int orderHdrCount = (int)DataService.ExecuteScalar(cmd);

                        if (orderHdrCount == 0)
                        {
                            data.Userfld10 = "ORDER_PAYMENT";
                            data.Save();
                        }
                    }
                }

                if (jsonData.OrderDetList != null)
                {
                    var orderDetList = JsonConvert.DeserializeObject<List<OrderDet>>(jsonData.OrderDetList.ToString());

                    foreach (OrderDet data in orderDetList)
                    {
                        string query = "select count(*) from OrderDet a where a.OrderDetID = '" + data.OrderDetID + "'";
                        QueryCommand cmd = new QueryCommand(query);
                        int orderDetCount = (int)DataService.ExecuteScalar(cmd);

                        if (orderDetCount == 0)
                        {
                            data.Userfld10 = "ORDER_PAYMENT";
                            data.Save();
                        }
                    }
                }

                if (jsonData.ReceiptHdrList != null)
                {
                    var receiptHdrList = JsonConvert.DeserializeObject<List<ReceiptHdr>>(jsonData.ReceiptHdrList.ToString());

                    foreach (ReceiptHdr data in receiptHdrList)
                    {
                        string query = "select count(*) from ReceiptHdr a where a.ReceiptHdrID = '" + data.ReceiptHdrID + "'";
                        QueryCommand cmd = new QueryCommand(query);
                        int receiptHdrCount = (int)DataService.ExecuteScalar(cmd);

                        if (receiptHdrCount == 0)
                        {
                            data.Userfld10 = "ORDER_PAYMENT";
                            data.Save();
                        }
                    }
                }

                if (jsonData.ReceiptDetList != null)
                {
                    var receiptDetList = JsonConvert.DeserializeObject<List<ReceiptDet>>(jsonData.ReceiptDetList.ToString());

                    foreach (ReceiptDet data in receiptDetList)
                    {
                        string query = "select count(*) from ReceiptDet a where a.ReceiptDetID = '" + data.ReceiptDetID + "'";
                        QueryCommand cmd = new QueryCommand(query);
                        int receiptDetCount = (int)DataService.ExecuteScalar(cmd);

                        if (receiptDetCount == 0)
                        {
                            data.Userfld10 = "ORDER_PAYMENT";
                            data.Save();
                        }
                    }
                }
            }
        }

        protected void ShowPanelPleaseWait()
        {
            try
            {
                pnlLoading.Parent = this;
                pnlLoading.Location = new Point(
                    ClientSize.Width / 2 - pnlLoading.Size.Width / 2,
                    ClientSize.Height / 2 - pnlLoading.Size.Height / 2);
                pnlLoading.Anchor = AnchorStyles.None;
                pnlLoading.BringToFront();
                pnlLoading.Visible = true;
                //Refresh();
            }
            catch (Exception ex)
            {
                pnlLoading.Visible = false;
                Logger.writeLog(ex);
                MessageBox.Show("Error." + ex.Message);
            }
        }

        private void frmPurchaseSummary_Load(object sender, EventArgs e)
        {
            string Status = "";
            try
            {
                #region *) Validation: Return if there is no MembershipNo
                if (membershipno == "" || membershipno == "-")
                {
                    this.Close();
                    return;
                }
                #endregion

                #region *) GUI Modification: Take all panel from TabControl and show directly in form without TabControl
                for (int Counter = 0; Counter < SandBox.TabPages.Count; Counter++)
                {
                    for (int inCnt = 0; inCnt < SandBox.TabPages[Counter].Controls.Count; inCnt++)
                        if (SandBox.TabPages[Counter].Controls[inCnt] is Panel)
                            pContent.Controls.Add(SandBox.TabPages[Counter].Controls[inCnt]);
                }
                #endregion

                #region *) Display Data: Show Membership Informations
                lblCardNo.Text = membershipno;
                lblName.Text = nametoappear;
                #endregion

                #region *) GUI Modification: Clear RemainingPackage Buttons
                foreach (Control oneCtl in pButton.Controls)
                {
                    if (oneCtl.Name == "showSummary") continue;
                    pButton.Controls.Remove(oneCtl);
                }
                #endregion

                

                Membership activeMember = new Membership(membershipno);
                //Membership activeMember = new Membership(membershipno);
                this.lblMobile.Text = String.IsNullOrEmpty(activeMember.Mobile) ? "NA" : activeMember.Mobile;
                this.lblAddress.Text = String.IsNullOrEmpty(activeMember.Block + activeMember.StreetName + activeMember.City + activeMember.Country + activeMember.ZipCode) ? "NA" : String.Format("{0} {1} {2} {3} {4}", activeMember.Block, activeMember.StreetName, activeMember.City, activeMember.Country, activeMember.ZipCode);
                this.lblNRIC.Text = String.IsNullOrEmpty(activeMember.Nric) ? "NA" : activeMember.Nric;
                //Activate button view info when member is active --- John Harries
                if (!string.IsNullOrEmpty(activeMember.MembershipNo))
                {
                    if (activeMember.CreatedOn != null && activeMember.ModifiedOn != null)
                    {
                        this.btnViewInfo.Visible = true;
                    }
                }

                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Membership.ShowPreOrderSummaryButton), false))
                {
                    btnPreOrderSummary.Visible = true;
                }

                if (AppSetting.GetSetting(AppSetting.SettingsName.Payment.InstallmentText) != null && AppSetting.GetSetting(AppSetting.SettingsName.Payment.InstallmentText) != "")
                {
                    btnInstallment.Text = AppSetting.GetSetting(AppSetting.SettingsName.Payment.InstallmentText);
                }

                showSummary_Click(sender,e);



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
            finally
            {
                this.Enabled = true;
                pnlLoading.Visible = false;
                
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Tabel_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (!(sender is DataGridView)) return;
            if (e.RowIndex < 0) return;

            ((DataGridView)sender).Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Silver;
        }

        private void Tabel_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (!(sender is DataGridView)) return;
            if (e.RowIndex < 0) return;

            ((DataGridView)sender).Rows[e.RowIndex].DefaultCellStyle.BackColor = ((DataGridView)sender).DefaultCellStyle.BackColor;
        }

        private void showSummary_Click(object sender, EventArgs e)
        {
            try
            {
                pnlInstallment.SendToBack();
                pnlInstallment.Visible = false;
                pSummary.Show();
                pSummary.BringToFront();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("Error occured: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                pnlLoading.SendToBack();
                pnlLoading.Visible = false;
            }
        }

        private void showPoint_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime StartValidPeriod;
                DateTime EndValidPeriod;
                decimal remainingPoint;

                string Status = "";

                string PackageName = "";
                string DisplayPackageName = "";
                string Item_PointMode = "";
                string refNo = "";
                if (((Button)sender).Tag.ToString().Contains("|OPP|"))
                {
                    //PackageName = ((Button)sender).Tag.ToString().Split(new string[] { "|OPP|" }, StringSplitOptions.RemoveEmptyEntries)[0];
                    DisplayPackageName = ((Button)sender).Text;
                    PackageName = ((Button)sender).Tag.ToString().Substring(1);
                    Item_PointMode = Item.PointMode.Times;
                    refNo = PackageName;
                }
                else
                {
                    PackageName = ((Button)sender).Text;
                    DisplayPackageName = PackageName;
                    Item_PointMode = ((Button)sender).Tag.ToString();
                    string sql = @"SELECT TOP 1 ItemNo FROM Item WHERE ItemName = N'{0}' AND ISNULL(Deleted,0) = 0";
                    sql = string.Format(sql, PackageName);
                    DataTable dtItem = new DataTable();
                    dtItem.Load(DataService.GetReader(new QueryCommand(sql)));
                    if (dtItem.Rows.Count > 0)
                        refNo = dtItem.Rows[0]["ItemNo"] + "";
                }

                tablePoint.AutoGenerateColumns = false;
                tablePointCompressed.AutoGenerateColumns = false;

                DataTable dt = new DataTable();
                if (!Features.Package.GetHistory_Last50Local(membershipno, PackageName, out dt, out StartValidPeriod, out EndValidPeriod, out remainingPoint, out Status))
                {
                    pHistoryPoint.Hide();
                    throw new Exception(Status);
                }

                decimal breakdownPrice =0;
                if (!Features.Package.GetCurrentBreakdownPrice(membershipno, DateTime.Now, refNo, out breakdownPrice, out Status))
                {
                    pHistoryPoint.Hide();
                    throw new Exception(Status); 
                }

                tablePoint.DataSource = dt;

                DataTable CompressedDT = dt.Copy();
                for (int Counter = CompressedDT.Rows.Count - 1; Counter >= 0; Counter--)
                    if (((decimal)CompressedDT.Rows[Counter]["Amount"]) <= 0)
                        CompressedDT.Rows.RemoveAt(Counter);

                CompressedDT.Columns.Add("ExpiryDate", Type.GetType("System.DateTime"));
                CompressedDT.Columns.Add("Used", Type.GetType("System.Decimal"));
                CompressedDT.Columns.Add("Remaining", Type.GetType("System.Decimal"));

                int ExpiredAfter=0;
                if (!AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Points.HaveExpiryDate), false))
                {
                    string StrExpiredAfter;
                    StrExpiredAfter = AppSetting.GetSetting(AppSetting.SettingsName.Points.ExpiredAfter);
                    if (StrExpiredAfter != null)
                        int.TryParse(StrExpiredAfter, out ExpiredAfter);
                }
                if (ExpiredAfter == 0) ExpiredAfter = 1200; /* 100 Years */

                decimal PointLeft = remainingPoint;
                //for (int Counter = 0; Counter < CompressedDT.Rows.Count; Counter++)
                for (int Counter = CompressedDT.Rows.Count - 1; Counter >= 0; Counter--)
                {
                    if (PointLeft <= 0)
                        //CompressedDT.Rows.RemoveAt(Counter--);
                        CompressedDT.Rows.RemoveAt(Counter);
                    else if (PointLeft >= ((decimal)CompressedDT.Rows[Counter]["Amount"]))
                    {
                        CompressedDT.Rows[Counter]["Used"] = 0;
                        CompressedDT.Rows[Counter]["Remaining"] = CompressedDT.Rows[Counter]["Amount"];
                        PointLeft -= ((decimal)CompressedDT.Rows[Counter]["Amount"]);
                        CompressedDT.Rows[Counter]["ExpiryDate"] = ((DateTime)CompressedDT.Rows[Counter]["AllocationDate"]).AddMonths(ExpiredAfter);
                    }
                    else
                    {
                        CompressedDT.Rows[Counter]["Used"] = ((decimal)CompressedDT.Rows[Counter]["Amount"]) - PointLeft;
                        CompressedDT.Rows[Counter]["Remaining"] = PointLeft;
                        PointLeft = 0;
                        CompressedDT.Rows[Counter]["ExpiryDate"] = ((DateTime)CompressedDT.Rows[Counter]["AllocationDate"]).AddMonths(ExpiredAfter);
                    }
                }

                tablePointCompressed.DataSource = CompressedDT;

                txtItemName.Text = DisplayPackageName;
                txtItemName2.Text = DisplayPackageName;

                tStartPoint.Text = StartValidPeriod.ToString("dd MMMM yyyy");
                tEndPoint.Text = EndValidPeriod.ToString("dd MMMM yyyy");
                lblDeductionStartValidPeriod.Text = StartValidPeriod.ToString("dd MMMM yyyy");
                lblDeductionEndValidPeriod.Text = EndValidPeriod.ToString("dd MMMM yyyy");
                if (Item_PointMode == Item.PointMode.Dollar)
                {
                    tRemainingPoint.Text = remainingPoint.ToString("N2");
                    lblDeductionRemainingPoints.Text = remainingPoint.ToString("N2");
                    //txtPointDeducted.Text = remainingPoint.ToString("N2");
                    dgvcAmount.DefaultCellStyle.Format = "N2";
                    dgvcDeductAmount.DefaultCellStyle.Format = "N2";
                    dgvcDeductRemaining.DefaultCellStyle.Format = "N2";
                    dgvcDeductUsed.DefaultCellStyle.Format = "N2";
                    dgvcAmount.HeaderText = "Points";
                    btnUsePoint.Visible = false;
                    lblPackageValue.Visible = false;
                    lblPackageBreakdown.Visible = false;
                    lblPackageValueText.Visible = false;
                    lblPackageBreakdownText.Visible = false;

                    if (remainingPoint > 0)
                        pButtonUse.Show();
                }
                else if (Item_PointMode == Item.PointMode.Times)
                {
                    tRemainingPoint.Text = remainingPoint.ToString("N0") + " Times";
                    lblDeductionRemainingPoints.Text = remainingPoint.ToString("N0") + " Times";
                    //txtPointDeducted.Text = remainingPoint.ToString("N0");
                    dgvcAmount.DefaultCellStyle.Format = "N0";
                    dgvcDeductAmount.DefaultCellStyle.Format = "N0";
                    dgvcDeductRemaining.DefaultCellStyle.Format = "N0";
                    dgvcDeductUsed.DefaultCellStyle.Format = "N0";
                    dgvcAmount.HeaderText = "Times";
                    btnUsePoint.Visible = true;
                    lblPackageValue.Visible = true;
                    lblPackageBreakdown.Visible = true;
                    lblPackageValueText.Visible = true;
                    lblPackageBreakdownText.Visible = true;

                    lblPackageValue.Text = (remainingPoint * breakdownPrice).ToString("N2");
                    lblPackageBreakdown.Text = breakdownPrice.ToString("N2");
                    if (remainingPoint > 0)
                        pButtonUse.Show();
                }
                else
                {
                    tRemainingPoint.Text = remainingPoint.ToString("N2");
                    dgvcAmount.DefaultCellStyle.Format = "N2";
                    dgvcAmount.HeaderText = "Amounts";
                    pButtonUse.Hide();
                }

                btnUsePoint.Tag = sender;
                pnlInstallment.Visible = false;
                pnlInstallment.SendToBack();
                pHistoryPoint.Show();
                pHistoryPoint.BringToFront();
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

        private void btnUsePoint_Click(object sender, EventArgs e)
        {
            string status = "";
            try
            {
                string selItemName;    /// Selected Item

                #region *) Get selected Course
                if (!(btnUsePoint.Tag is Button))
                    throw new Exception("(Error)Cannot get Item Name.\nPlease try again.");

                Button qButton = (Button)btnUsePoint.Tag;

                if (qButton.Tag != null && qButton.Tag.ToString().Contains("|OPP|"))
                {
                    selItemName = qButton.Tag.ToString().Substring(1);
                }
                else
                {

                    if (qButton.Tag == null | qButton.Tag.ToString() != Item.PointMode.Times)
                        throw new Exception("(Error)Cannot use Item.\nThis is not a \"Times\" package.\nPlease contact your administrator.");

                    selItemName = qButton.Text;
                }
                #endregion

                POSController pos = new POSController();

                #region *) Check & Assign Membership
                bool hasExpired = false;
                DateTime ExpiryDate = new DateTime(2010, 1, 1);

                if (MembershipController.IsExistingMember(membershipno, out hasExpired, out ExpiryDate))
                {
                    #region *) Assign Member to POS
                    if (pos.AssignMembership(membershipno, out status))
                    {
                        pos.ApplyMembershipDiscount();
                    }
                    #endregion
                }
                else if (hasExpired)
                {
                    //prompt window to allow bypass?
                    DialogResult dr = MessageBox.Show("This member has already expired on " + ExpiryDate + ".\nDo you want to allow it to continue using the card", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        #region Assign Expired Member To POS
                        if (pos.AssignExpiredMember(membershipno, out status))
                        {
                            pos.ApplyMembershipDiscount();
                        }
                        #endregion
                    }
                    else
                    {
                        MessageBox.Show("Process canceled by user", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
                else
                {
                    throw new Exception("Member does not exist.\nPlease try again.");
                }
                #endregion

                #region *) Check & Assign Item
                Item theItem;
                string PackageRefNo = "";
                if (selItemName.Contains("|OPP|") && selItemName.IndexOf("|OPP|") > 0)
                {
                    theItem = new Item(selItemName.Substring(0, selItemName.IndexOf("|OPP|")));
                    PackageRefNo = selItemName;
                }
                else
                {
                    theItem = new Item(Item.Columns.ItemName, selItemName);
                    PackageRefNo = theItem.ItemNo;
                }
                int NumberOfUsage = 1;
                decimal BreakdownPrice = 0;

                //item exist?
                if (theItem.IsLoaded && !theItem.IsNew)
                {
                    if (!PowerPOS.Feature.Package.GetCurrentBreakdownPrice(pos.CurrentMember.MembershipNo, pos.GetOrderDate(), PackageRefNo, out BreakdownPrice, out status))
                        throw new Exception(status);

                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Points.NotAlwaysUse1Point), false))
                    {
                        frmKeypad keypad = new frmKeypad();
                        keypad.initialValue = "1";
                        keypad.IsInteger = true;
                        if (keypad.ShowDialog() == DialogResult.Cancel) return;
                        int.TryParse(keypad.value, out NumberOfUsage);

                        if (NumberOfUsage < 1) return;
                    }
                    if (!MainPOS.AddPackageToOrder(theItem, NumberOfUsage, PreferedDiscount, ApplyPromo, true, PackageRefNo, BreakdownPrice, out status))
                        throw new Exception("(Error)" + status);

                    if (status != "")
                        throw new Exception("(Warning)" + status);

                    this.Close();
                    return;
                }
                #endregion

                AccessLogController.AddLog(AccessSource.POS, UserInfo.username, "-", string.Format("Use Point for member:{0} {1}", membershipno, qButton.Tag), "");

                Logger.writeLog("start printing");
                POSDeviceController.PrintAHAVATransactionReceipt(pos, 0, false,
                    (ReceiptSizes)Enum.ToObject(typeof(ReceiptSizes),
                    PrintSettingInfo.receiptSetting.PaperSize.Value),
                    PrintSettingInfo.receiptSetting.NumOfCopies.Value);
                Logger.writeLog("end printing");

                this.Close();

                #region *) After Effect
                /*Hashtable eligibility = pos.generateEligibleClasses();
                if (eligibility.Count > 0)
                {
                    ClassAttendance.frmClassAttendance f = new ClassAttendance.frmClassAttendance();
                    f.eligibility = eligibility;
                    f.checkEligibility = true;
                    f.membershipNo = pos.GetMemberInfo().MembershipNo;
                    f.ShowDialog();
                    f.Dispose();
                }*/
                /*
                bool hasWarranty = pos.hasWarrantyItems();
                bool hasDelivery = pos.hasDeliveryItem();
                if (hasWarranty || hasDelivery) //change this to has warranty!
                {
                    //prompt warranty....
                    MDIPowerPOS mdi = new MDIPowerPOS();
                    mdi.pos = pos;
                    mdi.orderHdrId = pos.GetUnsavedRefNo().Substring(2);
                    mdi.membershipNo = pos.GetMemberInfo().MembershipNo;
                    mdi.hasWarranty = hasWarranty;
                    mdi.hasDelivery = hasDelivery;
                    mdi.ShowDialog();
                    mdi.Dispose();
                }
                */
                #endregion

                showPoint_Click(btnUsePoint.Tag, new EventArgs());

                #region *) Navigation: Open Attendance Form
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Attendance.IsAvailable), false)
                    && AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Attendance.AutoAttendAfterSales), false))
                {
                    FormController.ShowForm(FormController.FormNames.AttendanceModule, new Attendance.frmAttendance());
                }
                #endregion

                IsUsePackage = true;
            }
            catch (Exception X)
            {
                if (X.Message.StartsWith("(warning)"))
                {
                    MessageBox.Show(X.Message.Replace("(warning)", ""), "Warning",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (X.Message.StartsWith("(Error)"))
                {
                    MessageBox.Show(X.Message.Replace("(error)", ""), "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    Logger.writeLog(X);
                    MessageBox.Show(
                        "Some error occurred. Please contact your administrator. " + X.Message, "Error"
                        , MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void SummaryTable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;

            if (SummaryTable.Columns[e.ColumnIndex].Name == dgvcView.Name)
            {
                EditBillForms.frmViewBillDetail myfrm = new EditBillForms.frmViewBillDetail();
                myfrm.OrderHdrID = SummaryTable.Rows[e.RowIndex].Cells[dgvcReferenceNo.Name].Value.ToString();
                myfrm.ShowDialog();
                myfrm.Dispose();
                showSummary.PerformClick();

                #region *) Reselect old rows
                SummaryTable.ClearSelection();
                if (e.RowIndex < SummaryTable.Rows.Count)
                {
                    for (int Counter = SummaryTable.Columns.Count - 1; Counter >= 0; Counter--)
                    {
                        if (!SummaryTable.Columns[Counter].Visible) continue;
                        SummaryTable[Counter, e.RowIndex].Selected = true;
                    }
                }
                #endregion
            }
        }

        private void btnInstallment_Click(object sender, EventArgs e)
        {
            //pnlInstallment.Visible = true;
            //pnlInstallment.BringToFront();
            //BindInstallment();

            pnlInstallmentSummary.Visible = true;
            pnlInstallmentSummary.BringToFront();
        }

        private bool isInsSumShowAll = false;
        private void BindInstallmentSummary()
        {
            try
            {
                if (!bgSyncInstallmentSumm.IsBusy)
                {
                    chkInsSumShowAll.Enabled = false;
                    dgvInstSummary.DataSource = null;
                    lblInsSumBalValue.Text = "0.00";

                    isInsSumShowAll = chkInsSumShowAll.Checked;
                    bgSyncInstallmentSumm.RunWorkerAsync();
                }
            }
            catch (Exception X)
            {
                CommonUILib.HandleException(X);
            }
        }

        private bool isInsDetShowAll = false;
        private string _instDetOrderHdrID = "";
        private void BindInstallmentDetail()
        {
            try
            {
                if (!bgSyncInstallmentDet.IsBusy)
                {
                    chkInsDetShowAll.Enabled = false;
                    dgvDebitCredit.DataSource = null;
                    lblCreditTotal.Text = "0.00";
                    lblDebit.Text = "0.00";
                    lblPlusMinus.Text = "0.00";

                    isInsDetShowAll = chkInsDetShowAll.Checked;
                    bgSyncInstallmentDet.RunWorkerAsync();
                }
            }
            catch (Exception X)
            {
                CommonUILib.HandleException(X);
            }
        }


        #region *) OBSOLETE
        //private void BindInstallment()
        //{
        //    try
        //    {
        //        //DataTable source = ReportController.FetchCustomerInstallmentHistoryReport(new DateTime(2007, 2, 1), DateTime.Now, membershipno);
        //        DataTable source = Installment.GetMemberHistory(membershipno, new DateTime(1990, 1, 1), new DateTime(2100, 1, 1));

        //        dgvDebitCredit.AutoGenerateColumns = false;
        //        dgvDebitCredit.DataSource = source;
        //        //dgvcInstReceiptNo.Visible = false;

        //        dgvDebitCredit.Refresh();
        //        object obj;
        //        decimal debitTotal = 0.00M;
        //        obj = source.Compute("SUM(debit)", "");
        //        if (obj is decimal) debitTotal = (decimal)obj;
        //        decimal creditTotal = 0.00M;
        //        obj = source.Compute("SUM(credit)", "");
        //        if (obj is decimal) creditTotal = (decimal)obj;
        //        decimal plusminus = debitTotal - creditTotal;

        //        lblCreditTotal.Text = creditTotal.ToString("N");
        //        lblDebit.Text = debitTotal.ToString("N");
        //        lblPlusMinus.Text = plusminus.ToString("N");
        //        if (plusminus < 0)
        //        {
        //            lblPlusMinus.ForeColor = Color.Pink;
        //        }
        //        else
        //        {
        //            lblPlusMinus.ForeColor = Color.White;
        //        }
        //    }
        //    catch (Exception X)
        //    {
        //        CommonUILib.HandleException(X);
        //    }
        //    //Installment DS = new Installment();

        //}
        #endregion


        private void dgvDebitCredit_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;


            if (dgvDebitCredit.Columns[e.ColumnIndex].Name == dgvcPay.Name)
            {
                try
                {
                    #region *) Validation: Cannot pay the InstallmentPayment - Not Logic :)
                    if (_instDetOrderHdrID != dgvDebitCredit[dgvcInstReceiptNo.Name, e.RowIndex].Value.ToString())
                        return; /// It is a payment, no need to pay the payment.
                    #endregion

                    PayInstallment(_instDetOrderHdrID);                    
                }
                catch (Exception X)
                {
                    CommonUILib.HandleException(X);
                }
            }
            else if (dgvDebitCredit.Columns[e.ColumnIndex].Name == dgvcInstReceiptNo.Name)
            {
                EditBillForms.frmViewBillDetail myfrm = new EditBillForms.frmViewBillDetail();

                myfrm.OrderHdrID =
                    dgvDebitCredit.Rows[e.RowIndex].Cells[dgvcInstReceiptNo.Name].Value.ToString();
                myfrm.ShowDialog();
                myfrm.Dispose();

                //BindInstallment();
                _instDetOrderHdrID = myfrm.OrderHdrID;
                BindInstallmentDetail();
            }
            else if (dgvDebitCredit.Columns[e.ColumnIndex].Name == dgvcInstReceiptTo.Name)
            {
                string OrderHdrID = dgvDebitCredit.Rows[e.RowIndex].Cells[dgvcInstReceiptTo.Name].Value.ToString();

                if (OrderHdrID == "")
                {
                    MessageBox.Show("We cannot locate the original receipt no. This feature is still in progress");
                }
                else
                {
                    EditBillForms.frmViewBillDetail myfrm = new EditBillForms.frmViewBillDetail();
                    myfrm.OrderHdrID = OrderHdrID;
                    myfrm.ShowDialog();
                    myfrm.Dispose();
                }

                //BindInstallment();
                _instDetOrderHdrID = OrderHdrID;
                BindInstallmentDetail();
            }
        }

        private void CustNumUpdate()
        {
            #region customNo Update
            int runningNo = 0;

            string selectmaxno = "select AppSettingValue from AppSetting where AppSettingKey='CurrentReceiptNo'";
            string currentReceiptNo = DataService.ExecuteScalar(new QueryCommand(selectmaxno)).ToString();

            int.TryParse(currentReceiptNo, out runningNo);

            string updatemaxnum1 = "update appsetting set AppSettingValue='" + ++runningNo + "' where AppSettingKey='CurrentReceiptNo'";
            DataService.ExecuteQuery(new QueryCommand(updatemaxnum1));

            //default max receiptno is 4
            string sql2 = "select case AppSettingValue when '' then '4' else AppSettingValue end from AppSetting where AppSettingKey='MaxReceiptNo'";
            QueryCommand Qcmd2 = new QueryCommand(sql2);
            int maxReceiptNo = 4;
            int.TryParse(DataService.ExecuteScalar(Qcmd2).ToString(), out maxReceiptNo);

            //check if it has reached the max no for that digit (99,999,9999.. etc)
            bool maximumReached = false;
            if (maxReceiptNo != 0)
            {
                maximumReached = true;
                for (int i = 0; i < maxReceiptNo; i++)
                {
                    if (currentReceiptNo[i] != '9')
                    {
                        maximumReached = false;
                        break;
                    }
                }

            }

            //if it has reached, update the maxreceiptno
            if (maximumReached)
            {
                string sql3 = "update appsetting set AppSettingValue = " + ++maxReceiptNo + " where AppSettingKey='MaxReceiptNo'";
                QueryCommand Qcmd3 = new QueryCommand(sql3);
                DataService.ExecuteQuery(Qcmd3);

                //string sql4 = "update appsetting set AppSettingValue = 0 where AppSettingKey='CurrentReceiptNo'";
                //QueryCommand Qcmd4 = new QueryCommand(sql4);
                //DataService.ExecuteQuery(Qcmd4);

                //currentReceiptNo = "0";
            }
            #endregion
        }

        // public bool makeInstallmentPayment;
        private void btnMakePayment_Click(object sender, EventArgs e)
        {
            string status = "";
            try
            {
                string selItemName;    /// Selected Item


                POSController pos = new POSController();

                #region *) Check & Assign Membership
                bool hasExpired = false;
                DateTime ExpiryDate = new DateTime(2010, 1, 1);

                if (MembershipController.IsExistingMember(membershipno, out hasExpired, out ExpiryDate))
                {
                    #region *) Assign Member to POS
                    if (pos.AssignMembership(membershipno, out status))
                    {
                        pos.ApplyMembershipDiscount();
                    }
                    #endregion
                }
                else if (hasExpired)
                {
                    //prompt window to allow bypass?
                    DialogResult dr = MessageBox.Show("This member has already expired on " + ExpiryDate + ".\nDo you want to allow it to continue using the card", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        #region Assign Expired Member To POS
                        if (pos.AssignExpiredMember(membershipno, out status))
                        {
                            pos.ApplyMembershipDiscount();
                        }
                        #endregion
                    }
                    else
                    {
                        MessageBox.Show("Process canceled by user", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
                else
                {
                    throw new Exception("Member does not exist.\nPlease try again.");
                }
                #endregion

                #region *) Check & Assign Item
                Item theItem = new Item(Item.Columns.ItemNo, "INST_PAYMENT");

                decimal BreakdownPrice = 0;

                //item exist?
                if (theItem.IsLoaded && !theItem.IsNew)
                {
                    frmKeypad keypad = new frmKeypad();
                    keypad.initialValue = "0";
                    keypad.ShowDialog();
                    decimal amount = 0;
                    decimal.TryParse(keypad.value, out amount);
                    if (amount > 0)
                    {
                        theItem.RetailPrice = amount;
                        if (!(pos.AddItemToOrder(theItem, 1, 0, false, out status)))
                        {

                            throw new Exception("Error creating payment item." + status);
                        }
                        else
                        {
                            //pos.myOrderDet[0].UnitPrice = amount;
                            //pos.getl(pos.get
                            //pos.myOrderDet[0].UnitPrice = amount;
                            pos.CalculateTotalAmount(out status);
                        }
                    }
                    else
                    {
                        pos = null;
                        return;
                    }
                }
                else
                {
                    throw new Exception("(error)Cannot find item.\nPlease try again.");
                }
                #endregion

                //pos.ApplyMembershipDiscount();


                #region *) Initialize: Choose Sales Person
                if (PointOfSaleInfo.promptSalesPerson)
                {
                    OrderForms.frmSalesPerson f = new OrderForms.frmSalesPerson();
                    f.ShowDialog();
                    if (!f.IsSuccessful)
                    {
                        f.Dispose();
                        return;
                    }
                    f.Dispose();
                }
                else
                {
                    SalesPersonInfo.SalesPersonID = UserInfo.username;
                    SalesPersonInfo.SalesPersonName = UserInfo.displayName;
                    SalesPersonInfo.SalesGroupID = UserInfo.SalesPersonGroupID;
                }
                #endregion

                #region Handle Payment
                //decimal amount = BreakdownPrice;

                #region *) Core: Add PaymentList in ReceiptDetails
                frmSelectPayment fPayment = new frmSelectPayment();
                fPayment.pos = pos;
                fPayment.amount = pos.CalculateTotalAmount(out status);
                fPayment.ParentSyncPointsThread = ParentSyncPointsThread;
                fPayment.ShowDialog();
                bool isSuccess = fPayment.isSuccessful;
                fPayment.Dispose();
                #endregion

                //bool IsQtyInsufficient = false;
                /*
                #region *) Validation: Check if Qty is sufficient to do stockout [Prompt if Error - Can be terminated by user]
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
                #endregion

                bool IsPointAllocationSuccess;
                if (!pos.ConfirmOrder(false, out IsPointAllocationSuccess, out status))
                {
                    pos.DeleteAllReceiptLinePayment();
                    throw new Exception("(error)" + status);
                }

                //if (!IsQtyInsufficient)
                if (!pos.ExecuteStockOut(out status))
                {
                    MessageBox.Show
                        ("Error while performing Stock Out: " + status,
                        "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                Logger.writeLog("start printing");
                POSDeviceController.PrintAHAVATransactionReceipt(pos, 0, false,
                    (ReceiptSizes)Enum.ToObject(typeof(ReceiptSizes),
                    PrintSettingInfo.receiptSetting.PaperSize.Value),
                    PrintSettingInfo.receiptSetting.NumOfCopies.Value);
                Logger.writeLog("end printing");
                */
                #endregion

                if (isSuccess)
                    btnInstallment_Click(this, new EventArgs());
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
                        "Some error occurred. Please contact your administrator. " + X.Message, "Error"
                        , MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void tablePoint_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;

            if (tablePoint.Columns[e.ColumnIndex].Name == dgvcPointView.Name)
            {
                EditBillForms.frmViewBillDetail myfrm = new EditBillForms.frmViewBillDetail();
                myfrm.OrderHdrID = tablePoint.Rows[e.RowIndex].Cells[dgvcPointRefNo.Name].Value.ToString();
                myfrm.ShowDialog();
                myfrm.Dispose();
            }

        }

        private void btnDeductPoint_Click(object sender, EventArgs e)
        {
            pnlDeduction.Show();
            pnlDeduction.BringToFront();
            txtPointDeducted.Focus();
        }

        private void btnExecuteDeduction_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtPointDeducted.Text == "")
                {
                    MessageBox.Show("Please Enter Correct Amount to be deducted");
                    return;
                }

                decimal RemainingPoint = decimal.Parse(tRemainingPoint.Text.Replace(" Times", "").Replace("$", "").Replace(",", ""));
                decimal DeductedPoint = 0;
                if (!decimal.TryParse(txtPointDeducted.Text.Replace("$", "").Replace(",", ""), out DeductedPoint))
                {
                    MessageBox.Show("Please Enter Correct Amount to be deducted");
                    return;
                }

                if (DeductedPoint == 0)
                {
                    MessageBox.Show("Cannot Enter 0 as Amount to be deducted");
                    return;
                }

                if (DeductedPoint > RemainingPoint)
                    throw new Exception("(warning)Remaining point is not enough");

                string SelectedItemNo = "";
                #region *) Get selected Course
                string SelectedItemName = "";
                if (!(btnUsePoint.Tag is Button))
                    throw new Exception("(Error)Cannot get Item Name.\nPlease try again.");

                Button qButton = (Button)btnUsePoint.Tag;

                if (qButton.Tag == null)
                    throw new Exception("(Error)Cannot use Item.\nThis is not a \"Times\" package.\nPlease contact your administrator.");

                SelectedItemName = qButton.Text;

                Item theItem = new Item(Item.Columns.ItemName, SelectedItemName);

                SelectedItemNo = theItem.ItemNo;
                #endregion

                bool IsAuthorized;
                string supName;
                string SupID = "-";
                #region *) Authorization: Check Supervisor ID
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PromptPassword.OnDeductPoints), true))
                {
                    string useMagenticStripReader = AppSetting.GetSetting(AppSetting.SettingsName.UseMagneticStripReader.ForAuthorizing);
                    if (useMagenticStripReader != null && useMagenticStripReader.ToLower() == "yes")
                    {

                        frmReadMSR f = new frmReadMSR();
                        f.privilegeName = PrivilegesController.DEDUCT_POINT;
                        f.loginType = LoginType.Authorizing;
                        f.ShowDialog();
                        IsAuthorized = f.IsAuthorized;
                        supName = f.buffer;
                        f.Dispose();
                    }
                    else
                    {
                        //Ask for verification....
                        //Prompt Supervisor Password
                        LoginForms.frmSupervisorLogin sl = new LoginForms.frmSupervisorLogin();
                        sl.privilegeName = PrivilegesController.DEDUCT_POINT;
                        sl.NeedSomeoneElseToVerify = false;
                        sl.ShowDialog();
                        SupID = sl.mySupervisor;
                        supName = sl.mySupervisor;
                        IsAuthorized = sl.IsAuthorized;
                    }
                }
                else
                {
                    IsAuthorized = true;
                    supName = "admin";
                }
                #endregion

                if (!IsAuthorized) return;

                #region #) Core: Send data to server
                DataTable dt = new DataTable("PointPackage");
                dt.Columns.Add("RefNo", Type.GetType("System.String"));
                dt.Columns.Add("Amount", Type.GetType("System.Decimal"));
                dt.Columns.Add("PointType", Type.GetType("System.String"));

                dt.Rows.Add(new object[] { theItem.ItemNo, 0 - DeductedPoint, qButton.Tag.ToString() });

                decimal InitialPoint, DiffPoint;
                string status = "";
                if (!PowerPOS.Feature.Package.UpdatePackage(dt, "ADJUSTMENT", DateTime.Now, 0, membershipno, supName + " [ADJUST]", UserInfo.username, out InitialPoint, out DiffPoint, out status))
                {
                    Logger.writeLog(status);
                    MessageBox.Show("Package Cannot Be Deducted. " + status);
                }
                else
                {
                    AccessLogController.AddLog(AccessSource.POS, UserInfo.username, "-", string.Format("Point deduction for member:{0} {1} {2}", membershipno, qButton.Tag, DeductedPoint), "");
                    IsUsePackage = true;
                    MessageBox.Show("Package Deducted Successfully" );
                    this.Close();
                }
                #endregion
            }
            catch (Exception X)
            {
                CommonUILib.HandleException(X);
            }
        }

        private void btnCancelDeduct_Click(object sender, EventArgs e)
        {
            pHistoryPoint.BringToFront();
        }

        private void btnInstallmentTutorial_Click(object sender, EventArgs e)
        {
            pnlInstallmentTutorial.Show();
            pnlInstallmentTutorial.BringToFront();
        }

        private void pnlInstallmentTutorial_MouseClick(object sender, MouseEventArgs e)
        {
            pnlInstallment.BringToFront();
        }

        /// <summary>
        /// View member info detail. (created by John Harries)
        /// </summary>
        private void btnViewInfo_Click(object sender, EventArgs e)
        {
            frmMembershipViewInfo frm = new frmMembershipViewInfo(membershipno);
            frm.ShowDialog(this);
        }

        private void btnPreOrderSummary_Click(object sender, EventArgs e)
        {
            pnlOrderSummary.Visible = true;
            pnlOrderSummary.BringToFront();
            BindPreOrder();
        }

        private void BindPreOrder()
        {
            //show preorder data
            //var preorderData = MembershipController.GetPreOrderSummary(membershipno);

            SyncClientController.Load_WS_URL();
            PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
            ws.Timeout = 100000;
            ws.Url = SyncClientController.WS_URL;

            string preOrderStr = ws.FetchPreOrderReportFromWeb(DateTime.Now.AddYears(-100), DateTime.Now.AddYears(100),
           "", membershipno, "", "", "", false, PointOfSaleInfo.InventoryLocationID, "", "", "");

            if (!string.IsNullOrEmpty(preOrderStr) && preOrderStr != "[]")
            {

                DataTable preorderData = JsonConvert.DeserializeObject<DataTable>(preOrderStr);

                preorderData = CommonUILib.DataTableConvertBoolColumnToYesOrNo(preorderData);

                preorderData.Columns.Add("BalanceQty", typeof(decimal), "QtyOnHand - TotalPreOrderQty");

                dgvPreOrderSummary.AutoGenerateColumns = false;
                dgvPreOrderSummary.DataSource = preorderData;

                if (!allowSplitDelivery)
                {
                    dgvPreOrderSummary.Columns["PreOrderOutstandingQty"].Visible = false;
                    dgvPreOrderSummary.Columns["PreOrderStatus"].Visible = false;
                }
            }
            dgvPreOrderSummary.Refresh();
        }

        private void dgvPreOrderSummary_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string status = "";

            SyncClientController.Load_WS_URL();
            PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
            ws.Timeout = 100000;
            ws.Url = SyncClientController.WS_URL;

            try
            {
                if (e.RowIndex >= 0)
                {
                    var orderdetid = dgvPreOrderSummary.Rows[e.RowIndex].Cells["OrderDetID"].Value.ToString();

                    //delivery button
                    if (dgvPreOrderSummary.Columns[e.ColumnIndex].Name == "PreOrderDelivery")
                    {
                        if (dgvPreOrderSummary.Rows[e.RowIndex].Cells["PreOrderDelivery"].Value.ToString() == "")
                        {
                            // this means the button is "disabled", so do nothing
                            return;
                        }

                        var orderhdrid = dgvPreOrderSummary.Rows[e.RowIndex].Cells["OrderHdrID"].Value.ToString();
                        var itemno = dgvPreOrderSummary.Rows[e.RowIndex].Cells["PreOrderItemNo"].Value.ToString();
                        var itemname = dgvPreOrderSummary.Rows[e.RowIndex].Cells["PreOrderItemName"].Value.ToString();
                        var pointOfSaleID = dgvPreOrderSummary.Rows[e.RowIndex].Cells["PointOfSaleID"].Value.ToString().GetIntValue();

                        //var stockonhand = ItemController.GetStockOnHand(itemno, PointOfSaleInfo.InventoryLocationID);
                        var stockonhand = Convert.ToInt32(dgvPreOrderSummary.Rows[e.RowIndex].Cells["QtyOnHand"].Value);
                        var outstandingqty = Convert.ToInt32(dgvPreOrderSummary.Rows[e.RowIndex].Cells["PreOrderOutstandingQty"].Value);
                        var balancepayment = Convert.ToDouble(dgvPreOrderSummary.Rows[e.RowIndex].Cells["PreOrderBalancePayment"].Value);

                        if (stockonhand <= 0)
                        {
                            MessageBox.Show("Stock on Hand for item " + itemname + " is " + stockonhand.ToString() + ". You can not do delivery");
                        }
                        else
                        {
                            if (outstandingqty > 0)
                            {
                                int NumberOfUsage = 0;

                                if (allowSplitDelivery)
                                {
                                    frmKeypad keypad = new frmKeypad();
                                    keypad.initialValue = "1";
                                    keypad.IsInteger = true;
                                    if (keypad.ShowDialog() == DialogResult.Cancel) return;
                                    int.TryParse(keypad.value, out NumberOfUsage);
                                    if (NumberOfUsage < 1) return;
                                }
                                else
                                {
                                    NumberOfUsage = outstandingqty;
                                }

                                //check if deliver more than needed
                                if (NumberOfUsage > outstandingqty)
                                {
                                    //if (MessageBox.Show("Are you sure to deliver more than outstanding qty ?", "Warning", MessageBoxButtons.YesNo) == DialogResult.No)
                                    //{
                                    //    return;
                                    //}

                                    MessageBox.Show("Cannot deliver more than outstanding quantity", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    return;
                                }

                                if (balancepayment > 0)
                                {
                                    MessageBox.Show("Balance Payment: " + balancepayment.ToString());
                                }

                                //create delivery order
                                POSController pos = new POSController();

                                #region *) Check & Assign Membership
                                bool hasExpired = false;
                                DateTime ExpiryDate = new DateTime(2010, 1, 1);

                                if (MembershipController.IsExistingMember(membershipno, out hasExpired, out ExpiryDate))
                                {
                                    #region *) Assign Member to POS
                                    if (pos.AssignMembership(membershipno, out status))
                                    {
                                        pos.ApplyMembershipDiscount();
                                    }
                                    #endregion
                                }
                                else if (hasExpired)
                                {
                                    //prompt window to allow bypass?
                                    DialogResult dr = MessageBox.Show("This member has already expired on " + ExpiryDate + ".\nDo you want to allow it to continue using the card", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                    if (dr == DialogResult.Yes)
                                    {
                                        #region Assign Expired Member To POS
                                        if (pos.AssignExpiredMember(membershipno, out status))
                                        {
                                            pos.ApplyMembershipDiscount();
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        MessageBox.Show("Process canceled by user", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        return;
                                    }
                                }
                                else
                                {
                                    throw new Exception("Member does not exist.\nPlease try again.");
                                }
                                #endregion

                                Bitmap signature = null;

                                #region Signature
                                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Signature.IsDeliveryPreOrder), false))
                                {
                                    //pop out whether want signature or not
                                    frmCustomMessageBox myfrm = new frmCustomMessageBox
                                        ("Add Signature", "Do you want to add signature to this transaction?");
                                    DialogResult DR = myfrm.ShowDialog();

                                    if (myfrm.choice == "yes")
                                    {
                                        myfrm.Dispose();

                                        //asking for Signature
                                        frmSignatureDelivery frm = new frmSignatureDelivery();
                                        frm.ShowDialog();
                                        if (frm.IsSuccessful)
                                        {
                                            signature = frm.bmp;
                                            frm.Dispose();
                                        }
                                    }
                                    else if (myfrm.choice == "no" || DR == DialogResult.Cancel)
                                    {
                                        myfrm.Dispose();
                                        return;
                                    }
                                }
                                #endregion
                                //frmDeliveryPersonel per = new frmDeliveryPersonel();
                                //per.ShowDialog();
                                //int deliverypersonid = per.DeliveryPersonID;
                                //per.Dispose();

                                //if (per.IsSuccesful)
                                //{

                                #region obsolete
                                //#region *) Create Delivery Order

                                //int deliverypersonid = -1;
                                //if (!pos.CreateDeliveryPreOrderSingleOrderDet(orderdetid,orderhdrid, membershipno, pointOfSaleID, itemno, NumberOfUsage, deliverypersonid, signature, false))
                                //{
                                //    MessageBox.Show("Error when creating delivery order");
                                //    return;
                                //}
                                //#endregion

                                //#region *) do stock out
                                ////if (!pos.DoStockOutPreOrderSingle(orderdetid, NumberOfUsage, out status))
                                ////{
                                ////    MessageBox.Show("Error when do stock out");
                                ////    return;
                                ////}

                                //if (PointOfSaleInfo.IntegrateWithInventory)
                                //{
                                //    InventoryController.AssignStockOutToPreOrderSalesUsingTransaction();
                                //}
                                //else
                                //{
                                //    try
                                //    {
                                //        #region *) Try To Sync to server if is enabled
                                //        if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSyncDeliveryOrder), false))
                                //            if (!SyncSendDeliveryOrderThread.IsBusy)
                                //                SyncSendDeliveryOrderThread.RunWorkerAsync();
                                //        #endregion
                                //        //SyncClientController.SendDeliveryOrderToServer(DateTime.Today, DateTime.Now, true);
                                //        //SyncClientController.GetDeliveryOrderFromServer(DateTime.Today, DateTime.Now);
                                //        InventorySync.GetCurrentInventory();
                                //        SyncClientController.UpdateOrderDetFromDownloadedInventoryHdr();
                                //    }
                                //    catch (Exception ex)
                                //    {
                                //        Logger.writeLog(ex);
                                //        MessageBox.Show("Error occured when syncing data to server.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                //    }
                                //}
                                //#endregion

                                #endregion
                                int deliverypersonid = -1;

                                byte[] sig = null;

                                if (signature != null)
                                {
                                    ImageConverter converter = new ImageConverter();
                                    sig = (byte[])converter.ConvertTo(signature, typeof(byte[]));
                                }

                                if (!ws.CreateDeliveryPreOrderSingleOrderDetForWeb(orderdetid, NumberOfUsage, deliverypersonid, PointOfSaleInfo.PointOfSaleID, sig, out status))
                                {
                                    MessageBox.Show(status);
                                    return;
                                }

                                BindPreOrder();

                                MessageBox.Show("Delivery Order created succesfully");


                                //}
                            }
                            else
                            {
                                MessageBox.Show("Outstanding quantity for item " + itemname + " is " + outstandingqty.ToString() + ". You can not do delivery");
                            }
                        }
                    }
                    else if (dgvPreOrderSummary.Columns[e.ColumnIndex].Name == "PreOrderNotify") // Notify button
                    {
                        if (dgvPreOrderSummary.Rows[e.RowIndex].Cells["PreOrderNotify"].Value.ToString() == "")
                        {
                            // this means the button is "disabled", so do nothing
                            return;
                        }

                        if (MessageBox.Show("Are you sure to send notification email?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.No)
                        {
                            return;
                        }

                        var member = new Membership(membershipno);
                        var orderhdrid = dgvPreOrderSummary.Rows[e.RowIndex].Cells["OrderHdrID"].Value.ToString();

                        if (ws.SendNotifiyMailDelivery(orderhdrid, orderdetid, out status))
                            MessageBox.Show("Send Email Success");
                        else
                            MessageBox.Show(status);

                        BindPreOrder();
                    }
                    else if (dgvPreOrderSummary.Columns[e.ColumnIndex].Name == "PreOrderStatus") // Partial delivery button
                    {
                        frmTrackDelivery ftd = new frmTrackDelivery();
                        ftd.orderdetid = orderdetid;
                        ftd.ShowDialog();
                        ftd.Dispose();
                    }
                    else if (dgvPreOrderSummary.Columns[e.ColumnIndex].Name == "Delivered") // Delivered button
                    {
                        if (dgvPreOrderSummary.Rows[e.RowIndex].Cells["Delivered"].Value.ToString() == "")
                        {
                            // this means the button is "disabled", so do nothing
                            return;
                        }

                        if (MessageBox.Show("Are you sure to mark this as delivered?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.No)
                        {
                            return;
                        }

                        if (!ws.SetDeliveryAsDeliveredStatus(orderdetid, UserInfo.username, out status))
                        {
                            MessageBox.Show(status);
                            return;
                        }
                        else
                        {
                            MessageBox.Show("Pre Order Status has been changed");

                            string preOrderStr = ws.FetchDeliveryOrderToPrintByOrderDetID(orderdetid);

                            if (!string.IsNullOrEmpty(preOrderStr))
                            {
                                DataTable preorderData = JsonConvert.DeserializeObject<DataTable>(preOrderStr);

                                POSDevices.Receipt rcpt = new POSDevices.Receipt();
                                rcpt.PrintPreOrderDelivery(preorderData);
                            }
                        }
                           

                        BindPreOrder();
                    }
                    else if (dgvPreOrderSummary.Columns[e.ColumnIndex].Name == "CancelPreOrder") // Cancel Pre Order button
                    {
                        if (dgvPreOrderSummary.Rows[e.RowIndex].Cells["CancelPreOrder"].Value.ToString() == "")
                        {
                            // this means the button is "disabled", so do nothing
                            return;
                        }

                        bool isReturn = false;
                        frmSelectCancelPreOrder slc = new frmSelectCancelPreOrder();
                        slc.ShowDialog();

                        isReturn = slc.IsReturnDeposit;
                        slc.Dispose();

                        if (!isReturn)
                        {

                            if (MessageBox.Show("Are you sure you want to cancel this pre order item?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.No)
                            {
                                return;
                            }

                            var orderhdrid = dgvPreOrderSummary.Rows[e.RowIndex].Cells["OrderHdrID"].Value.ToString();

                            if (!ws.CancelPreOrder(orderhdrid, orderdetid, UserInfo.username, out status))
                            {
                                MessageBox.Show(status);
                                return;
                            }
                            else
                            {
                                MessageBox.Show("Pre Order has been canceled");
                            }

                            BindPreOrder();
                        }
                        else {
                            var orderhdrid = dgvPreOrderSummary.Rows[e.RowIndex].Cells["OrderHdrID"].Value.ToString();
                            var pointOfSaleID = dgvPreOrderSummary.Rows[e.RowIndex].Cells["PointOfSaleID"].Value.ToString().GetIntValue();

                            PointOfSale p = new PointOfSale(pointOfSaleID);                            
                            bool AllowRefundForSameOutlet = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Refund.RefundReceiptFromSameOutlet), false);
                            byte[] tmp = ws.GetDataOrderDetForRefund(orderhdrid, orderdetid, PointOfSaleInfo.PointOfSaleID, AllowRefundForSameOutlet, out status);

                            if (status != "" && status.StartsWith("Warning."))
                            {
                                MessageBox.Show(status.Replace("Warning.", ""));
                            }

                            DataSet ds = new DataSet();
                            if (tmp != null)
                            {
                                ds = SyncClientController.DecompressDataSetFromByteArray(tmp);

                                MainPOS.DeleteAllReceiptLinePayment();
                                MainPOS.myOrderHdr.PointOfSaleID = PointOfSaleInfo.PointOfSaleID;
                                MainPOS.myOrderHdr.IsPointAllocated = false; // Make sure IsPointAllocated is false


                                MainPOS.myOrderDet.Load(ds.Tables[1]);
                                this.Close();
                            }

                            
                        }
                    }
                    else if (dgvPreOrderSummary.Columns[e.ColumnIndex].Name == "Print")
                    {
                        if (dgvPreOrderSummary.Rows[e.RowIndex].Cells["Print"].Value.ToString() == "")
                        {
                            // this means the button is "disabled", so do nothing
                            return;
                        }

                        string preOrderStr = ws.FetchDeliveryOrderToPrintByOrderDetID(orderdetid);

                        if (!string.IsNullOrEmpty(preOrderStr))
                        {
                            DataTable preorderData = JsonConvert.DeserializeObject<DataTable>(preOrderStr);

                            POSDevices.Receipt rcpt = new POSDevices.Receipt();
                            rcpt.PrintPreOrderDelivery(preorderData);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("Error occured: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvPreOrderSummary_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in dgvPreOrderSummary.Rows)
            {
                string deliveryStatus = row.Cells["DeliveryStatus"].Value.ToString();
                if (deliveryStatus != "Pending")
                {
                    // format display for Delivered button
                    DataGridViewButtonCell btnCell = (DataGridViewButtonCell)row.Cells["Delivered"];
                    btnCell.UseColumnTextForButtonValue = false;
                    btnCell.FlatStyle = FlatStyle.Flat;
                    btnCell.Value = "";
                }

                int onHandQty;
                int.TryParse(row.Cells["QtyOnHand"].Value.ToString(), out onHandQty);
                if (onHandQty <= 0 || deliveryStatus == "Delivered")
                {
                    // format display for Notify and Delivery button
                    DataGridViewButtonCell btnCell = (DataGridViewButtonCell)row.Cells["PreOrderNotify"];
                    btnCell.UseColumnTextForButtonValue = false;
                    btnCell.FlatStyle = FlatStyle.Flat;
                    btnCell.Value = "";
                    
                    btnCell = (DataGridViewButtonCell)row.Cells["PreOrderDelivery"];
                    btnCell.UseColumnTextForButtonValue = false;
                    btnCell.FlatStyle = FlatStyle.Flat;
                    btnCell.Value = "";
                }

                if (deliveryStatus != "Not Delivered")
                {
                    // format display for Cancel Pre Order button
                    DataGridViewButtonCell btnCell = (DataGridViewButtonCell)row.Cells["CancelPreOrder"];
                    btnCell.UseColumnTextForButtonValue = false;
                    btnCell.FlatStyle = FlatStyle.Flat;
                    btnCell.Value = "";
                }

                if (deliveryStatus != "Delivered")
                {
                    // format display for Print button
                    DataGridViewButtonCell btnCell = (DataGridViewButtonCell)row.Cells["Print"];
                    btnCell.UseColumnTextForButtonValue = false;
                    btnCell.FlatStyle = FlatStyle.Flat;
                    btnCell.Value = "";
                }
            }
        }

        private void bgSyncPoints_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                // ParentSyncPointsThread is a backgroundworker running in frmOrderTaking to sync points when member is scanned.
                if (ParentSyncPointsThread != null)
                {
                    bool waiting = ParentSyncPointsThread.IsBusy;
                    while (waiting)
                    {
                        System.Threading.Thread.Sleep(500);
                        waiting = ParentSyncPointsThread.IsBusy;
                    }
                    e.Result = true;
                }
                else
                {
                    //timeSyncPaymentBalanceStarted = DateTime.Now;
                    //syncMembershipPaymentBalance();
                    //TimeSpan ts = DateTime.Now - timeSyncPaymentBalanceStarted;
                    //Logger.writeLog(string.Format("Sync Membership Payment Balance completed in {0} seconds.", ts.Seconds.ToString()));

                    timeSyncPointStarted = DateTime.Now;
                    Logger.writeLog(string.Format("Sync Points started on {0}", timeSyncPointStarted.ToString("yyyy-MM-dd HH:mm:ss.fff")));
                    if (Features.Package.isAvailable)
                    {
                        //Download 

                        string isRealTime = ConfigurationManager.AppSettings["RealTimePointSystem"];
                        if (isRealTime == "yes" || isRealTime == "true")
                        {
                            PowerPOS.SyncPointsController.SyncPointsRealTimeController snc = new PowerPOS.SyncPointsController.SyncPointsRealTimeController();
                            if (!snc.SyncPoints(membershipno))
                            {
                                e.Result = false;
                            }
                            else
                            {
                                e.Result = true;
                            }
                            e.Result = true;
                            TimeSpan ts = DateTime.Now - timeSyncPointStarted;
                            Logger.writeLog(string.Format("Download Points completed in {0} seconds.", ts.Seconds.ToString()));
                        }
                        else
                        {
                            e.Result = true;
                        }
                    }
                    else
                    {
                        e.Result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                e.Result = false;
                
                //throw new InvalidOperationException("Error Downloading Points." + ex.Message);
            }
        }

        private void bgSyncPoints_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                timeSyncPointStarted = DateTime.Now;

                if (this.IsDisposed) return;

                bool res = false;
                if (!bool.TryParse(e.Result.ToString(), out res))
                    res = false;
                if (!res)
                    MessageBox.Show("Download Points Data Error. Please check your connection");

                #region *) Make sure the PointTempLog table is cleaned
                PointTempLogController.Clean();
                #endregion

                //Load Points
                if (Features.Package.isAvailable)
                {
                    //Download 
                    string Status = "";
                    string[] Btns;

                    #region *) Load Existing-Package list
                    if ((Btns = Features.Package.getPackageListLocal(membershipno, out Status)) == null)
                    { throw new Exception(Status); }
                    #endregion

                    #region *) Draw Package List's buttons
                    if (Btns != null && Btns.Length > 0)
                    {
                        foreach (string oneBtn in Btns)
                        {
                            Button newBtn = new Button();
                            newBtn.Name = "Btn" + DateTime.Now.ToString("ddMMyyhhmmssfff");

                            string itemNo = "";
                            if (oneBtn.Contains("|OPP|") && oneBtn.IndexOf("|OPP|") > 0)
                            {
                                //Item myItem = new Item(oneBtn.Substring(1, oneBtn.IndexOf("|OPP|") - 1));
                                itemNo = oneBtn.Substring(1);// myItem.ItemName;
                            }
                            else if (oneBtn.StartsWith(Item.PointMode.Times))
                            {
                                itemNo = oneBtn.Substring(Item.PointMode.Times.Length);
                            }
                            else if (oneBtn.StartsWith(Item.PointMode.Dollar))
                            {
                                itemNo = oneBtn.Substring(Item.PointMode.Dollar.Length);
                            }
                            else
                            {
                                itemNo = oneBtn;
                            }

                            decimal remainingPts = Features.Package.GetCurrentBalance(membershipno, DateTime.Now, itemNo, out Status);
                            if (remainingPts <= 0)
                                newBtn.BackColor = Color.Gray;
                            else
                                newBtn.BackgroundImage = showPoint.BackgroundImage;
                            newBtn.Dock = DockStyle.Top;
                            newBtn.Font = showPoint.Font;
                            newBtn.Size = showPoint.Size;
                            newBtn.UseVisualStyleBackColor = true;
                            newBtn.Show();
                            pButton.Controls.Add(newBtn);
                            newBtn.Click += new EventHandler(showPoint_Click);
                            newBtn.BringToFront();

                            if (oneBtn.Contains("|OPP|") && oneBtn.IndexOf("|OPP|") > 0)
                            {
                                newBtn.Tag = oneBtn;
                                Item myItem = new Item(oneBtn.Substring(1, oneBtn.IndexOf("|OPP|") - 1));
                                if (!myItem.IsLoaded) throw new Exception("Package item not found in DB");
                                newBtn.Text = myItem.ItemName + " " + oneBtn.Substring(oneBtn.IndexOf("|OPP|") + 31);
                            }
                            else if (oneBtn.StartsWith(Item.PointMode.Times))
                            {
                                newBtn.Tag = Item.PointMode.Times;
                                newBtn.Text = oneBtn.Substring(Item.PointMode.Times.Length);
                            }
                            else if (oneBtn.StartsWith(Item.PointMode.Dollar))
                            {
                                newBtn.Tag = Item.PointMode.Dollar;
                                newBtn.Text = oneBtn.Substring(Item.PointMode.Dollar.Length);
                            }
                            else
                            {
                                newBtn.Text = oneBtn;
                                newBtn.Tag = Item.PointMode.None;
                            }
                        }
                    }
                    #endregion
                }
                pnlLoading.SendToBack();
                pnlLoading.Visible = false;
                this.Enabled = true;
                pButton.Enabled = true; pHeader.Enabled = true;
                flowLayoutPanel1.Enabled = true;

                TimeSpan ts = DateTime.Now - timeSyncPointStarted;
                Logger.writeLog(string.Format("Points Button Loaded in {0} seconds.", ts.Seconds.ToString()));
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        private void frmPurchaseSummary_Shown(object sender, EventArgs e)
        {
            if (Features.Package.isAvailable && membershipno != "WALK-IN")
            {
                this.Enabled = false;
                pButton.Enabled = false;
                pHeader.Enabled = false;
                flowLayoutPanel1.Enabled = false;
                ShowPanelPleaseWait();
                SummaryTable.Visible = false;
                showSummary.Text += " (Loading...)";
                bgSyncTransaction.RunWorkerAsync();
                bgSyncPoints.RunWorkerAsync();
                BindInstallmentSummary();
            }
        }

        private void pHeader_Paint(object sender, PaintEventArgs e)
        {

        }

        private static DataTable LastTransactionData;
        private void bgSyncTransaction_DoWork(object sender, DoWorkEventArgs e)
        {
            //pnlLoading.Visible = true;
            //pnlLoading.BringToFront();

            timeSyncTransactionStarted = DateTime.Now;

            try
            {
                LastTransactionData = null;
                Membership activeMember = new Membership(membershipno);

                //Activate button view info when member is active --- John Harries
                //if (!string.IsNullOrEmpty(activeMember.MembershipNo))
                //{
                //    this.btnViewInfo.Visible = true;
                //}
                int rowtotal = 0;
                if (!int.TryParse(AppSetting.GetSetting(AppSetting.SettingsName.Membership.MembershipSummaryRowNumber), out rowtotal))
                {
                    rowtotal = 5000;
                }


                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Membership.DownloadAllRecentPurchase), false))
                {
                    PointOfSale posInfo = new PointOfSale(PointOfSaleInfo.PointOfSaleID);

                    PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                    SyncClientController.Load_WS_URL();
                    ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                    ws.Url = SyncClientController.WS_URL;
                    byte[] data = ws.GetPastTransactionWithOutlet(activeMember.MembershipNo, rowtotal, posInfo.OutletName);
                    DataSet ds = SyncClientController.DecompressDataSetFromByteArray(data);
                    LastTransactionData = ds.Tables[0];
                }
                else
                {
                    LastTransactionData = activeMember.GetPastTransaction(rowtotal);
                }
            }
            catch (Exception X)
            {
                if (X.Message.StartsWith("(warning)"))
                {
                    MessageBox.Show(X.Message.Replace("(warning)", ""), "Warning",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    Logger.writeLog(X);
                    if (X.ToString().Contains("System.Net.WebException"))
                    {
                        MessageBox.Show(
                        "Failed download summary data from server. Please check your connection", "Error"
                        , MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show(
                        "Some error occurred. Please contact your administrator.", "Error"
                        , MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
            }
        }

        private void bgSyncTransaction_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (this.IsDisposed) return;

                showSummary.Text = showSummary.Text.Replace(" (Loading...)", "");
                //pnlLoading.Visible = false;
                SummaryTable.AutoGenerateColumns = false;
                SummaryTable.Visible = true;
                if (LastTransactionData != null)
                {
                    SummaryTable.DataSource = LastTransactionData;
                    SummaryTable.Sort(SummaryTable.Columns[dgvcTransTime.Name], ListSortDirection.Descending);
                }

                Decimal Total = decimal.Zero;
                foreach (DataGridViewRow row in SummaryTable.Rows)
                {
                    Decimal temp = decimal.Zero;
                    decimal.TryParse(row.Cells["dgvcSumAmount"].Value.ToString(), out temp);
                    Total = Total += temp;
                }
                lblTotal.Text =  Total.ToString("N");

                TimeSpan ts = DateTime.Now - timeSyncTransactionStarted;
                Logger.writeLog(string.Format("Sync Transactions completed in {0} seconds.", ts.Seconds.ToString()));
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        private static DataTable dtInstallmentSummary;
        private void bgSyncInstallmentSumm_DoWork(object sender, DoWorkEventArgs e)
        {
            string status = "";

            try
            {
                dtInstallmentSummary = null;

                if (!PointOfSaleInfo.IntegrateWithInventory)
                {
                    SyncClientController.Load_WS_URL();
                    PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                    ws.Url = SyncClientController.WS_URL;

                    DataSet ds = ws.GetInstallmentListByCustomer(membershipno, !isInsSumShowAll, out status);
                    if (ds != null && ds.Tables.Count > 0)
                        dtInstallmentSummary = ds.Tables[0];
                }
                else
                {
                    DataSet ds = InstallmentController.GetInstallmentListByCustomer(membershipno, !chkInsSumShowAll.Checked, out status);
                    if (ds != null && ds.Tables.Count > 0)
                        dtInstallmentSummary = ds.Tables[0];
                }
            }
            catch (Exception X)
            {
                if (X.Message.StartsWith("(warning)"))
                {
                    MessageBox.Show(X.Message.Replace("(warning)", ""), "Warning",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    Logger.writeLog(X);
                    if (X.ToString().Contains("System.Net.WebException"))
                    {
                        MessageBox.Show(
                        "Failed download installment summary data from server. Please check your connection", "Error"
                        , MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show(
                        "Some error occurred. Please contact your administrator.", "Error"
                        , MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void bgSyncInstallmentSumm_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            try
            {
                chkInsSumShowAll.Enabled = true;

                dgvInstSummary.AutoGenerateColumns = false;
                dgvInstSummary.DataSource = dtInstallmentSummary;
                dgvInstSummary.Refresh();

                object obj;
                decimal balanceTotal = 0.00M;
                obj = dtInstallmentSummary.Compute("SUM(CurrentBalance)", "");
                if (obj is decimal) balanceTotal = (decimal)obj;

                lblInsSumBalValue.Text = balanceTotal.ToString("N");
                if (balanceTotal <= 0)
                {
                    lblInsSumBalText.ForeColor = Color.LightGreen;
                    lblInsSumBalValue.ForeColor = Color.LightGreen;
                }
                else
                {
                    lblInsSumBalText.ForeColor = Color.Salmon;
                    lblInsSumBalValue.ForeColor = Color.Salmon;
                }
            }
            catch (Exception X)
            {
                CommonUILib.HandleException(X);
            }
        }

        private void dgvInstSummary_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

                _instDetOrderHdrID = dgvInstSummary.Rows[e.RowIndex].Cells[dgvcInsSumOrderHdrId.Name].Value.ToString();

                if (dgvInstSummary.Columns[e.ColumnIndex].Name == dgvcInsSumView.Name) // View button
                {
                    chkInsDetShowAll.Checked = false;
                    BindInstallmentDetail();
                    pnlInstallment.Visible = true;
                    pnlInstallment.BringToFront();
                }
                else if (dgvInstSummary.Columns[e.ColumnIndex].Name == dgvcInsSumPay.Name) // PAY button
                {
                    PayInstallment(_instDetOrderHdrID);
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("Error occured: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static DataTable dtInstallmentDetail;
        private void bgSyncInstallmentDet_DoWork(object sender, DoWorkEventArgs e)
        {
            string status = "";

            try
            {
                dtInstallmentDetail = null;

                if (!PointOfSaleInfo.IntegrateWithInventory)
                {
                    SyncClientController.Load_WS_URL();
                    PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                    ws.Url = SyncClientController.WS_URL;

                    DataSet ds = ws.GetInstallmentDetailByOrderHdrID(membershipno, _instDetOrderHdrID, isInsDetShowAll, out status);
                    if (ds != null && ds.Tables.Count > 0)
                        dtInstallmentDetail = ds.Tables[0];
                }
                else
                {
                    DataSet ds = InstallmentController.GetInstallmentDetailByOrderHdrID(membershipno, _instDetOrderHdrID, isInsDetShowAll, out status);
                    if (ds != null && ds.Tables.Count > 0)
                        dtInstallmentDetail = ds.Tables[0];
                }
            }
            catch (Exception X)
            {
                if (X.Message.StartsWith("(warning)"))
                {
                    MessageBox.Show(X.Message.Replace("(warning)", ""), "Warning",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    Logger.writeLog(X);
                    if (X.ToString().Contains("System.Net.WebException"))
                    {
                        MessageBox.Show(
                        "Failed download installment detail data from server. Please check your connection", "Error"
                        , MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show(
                        "Some error occurred. Please contact your administrator.", "Error"
                        , MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void bgSyncInstallmentDet_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                chkInsDetShowAll.Enabled = true;

                dgvDebitCredit.AutoGenerateColumns = false;
                dgvDebitCredit.DataSource = dtInstallmentDetail;
                dgvDebitCredit.Refresh();

                object obj;
                decimal debitTotal = 0.00M;
                obj = dtInstallmentDetail.Compute("SUM(debit)", "");
                if (obj is decimal) debitTotal = (decimal)obj;
                decimal creditTotal = 0.00M;
                obj = dtInstallmentDetail.Compute("SUM(credit)", "");
                if (obj is decimal) creditTotal = (decimal)obj;
                decimal plusminus = debitTotal - creditTotal;

                lblCreditTotal.Text = creditTotal.ToString("N");
                lblDebit.Text = debitTotal.ToString("N");
                lblPlusMinus.Text = plusminus.ToString("N");
                if (plusminus < 0)
                {
                    lblPlusMinus.ForeColor = Color.Pink;
                }
                else
                {
                    lblPlusMinus.ForeColor = Color.White;
                }
            }
            catch (Exception X)
            {
                CommonUILib.HandleException(X);
            }
        }

        private void chkInsSumShowAll_CheckedChanged(object sender, EventArgs e)
        {
            BindInstallmentSummary();
        }

        private void chkInsDetShowAll_CheckedChanged(object sender, EventArgs e)
        {
            BindInstallmentDetail();
        }

        private void PayInstallment(string orderHdrID)
        {
            try
            {
                POSController pos = new POSController();
                string status;

                DataTable source = (DataTable)dgvInstSummary.DataSource;
                if (source == null) return;

                // Get the row of this OrderHdrID
                DataRow[] rows = source.Select(string.Format("OrderHdrId = '{0}'", orderHdrID));
                if (rows.Length <= 0) 
                    throw new Exception(string.Format("Transaction {0} cannot be found.", orderHdrID));

                decimal OutstandingBalance;
                #region *) Initialize: Get Outstanding Balance
                OutstandingBalance = (rows[0]["CurrentBalance"] + "").GetDecimalValue();  // Installment.GetOutstandingBalance(orderHdrID, new DateTime(2100, 1, 1));
                #endregion

                #region *) Validation: Only able to pay receipt that is still outstanding
                if (OutstandingBalance <= 0)
                    throw new Exception("(warning)Receipt [" + orderHdrID + "] has no outstanding balance");
                #endregion

                #region *) Initialize: Check & Assign Membership
                bool hasExpired = false;
                DateTime ExpiryDate = new DateTime(2010, 1, 1);

                if (MembershipController.IsExistingMember(membershipno, out hasExpired, out ExpiryDate))
                {
                    #region *) Assign Member to POS
                    if (pos.AssignMembership(membershipno, out status))
                    {
                        pos.ApplyMembershipDiscount();
                    }
                    #endregion
                }
                else if (hasExpired)
                {
                    //prompt window to allow bypass?
                    DialogResult dr = MessageBox.Show("This member has already expired on " + ExpiryDate + ".\nDo you want to allow it to continue using the card", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        #region Assign Expired Member To POS
                        if (pos.AssignExpiredMember(membershipno, out status))
                        {
                            pos.ApplyMembershipDiscount();
                        }
                        #endregion
                    }
                    else
                    {
                        MessageBox.Show("Process canceled by user", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
                else
                {
                    throw new Exception("Member does not exist.\nPlease try again.");
                }
                #endregion

                #region *) Initialize: Check & Assign Item
                bool IsCreditNote = false;

                frmSelectInstallmentPay fselect = new frmSelectInstallmentPay();
                fselect.ShowDialog();

                if (fselect.IsSuccesful)
                {
                    IsCreditNote = fselect.IsCreditNote;
                }

                Item theItem = IsCreditNote ? new Item(Item.Columns.ItemNo, "CREDIT_NOTE") : new Item(Item.Columns.ItemNo, "INST_PAYMENT");


                //item exist?
                if (theItem.IsNew || !theItem.IsLoaded)
                    throw new Exception("(error)Cannot find item.\nPlease try again.");

                decimal MaxInstPayment = OutstandingBalance;

                decimal amount = 0;
                do
                {
                    frmKeypad keypad = new frmKeypad();
                    keypad.initialValue = "0";
                    keypad.ShowDialog();
                    keypad.textMessage = "Please Insert Credit Note Remark";
                    decimal.TryParse(keypad.value, out amount);

                    if (amount == 0)
                    { pos = null; return; }

                    if (amount > MaxInstPayment)
                        MessageBox.Show("Outstanding amount for this receipt is only " + OutstandingBalance.ToString("N")
                            , "Payment is more than outstanding amount", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                } while (amount > MaxInstPayment);

                theItem.RetailPrice = IsCreditNote ? 0 - amount : amount;
                if (!(pos.AddItemToOrder(theItem, 1, 0, false, out status)))
                    throw new Exception("(warning)Error creating payment item." + status);

                pos.myOrderDet[0].InstRefNo = orderHdrID;
                if (IsCreditNote)
                {
                    frmKeyboard fKeypad = new frmKeyboard();
                    fKeypad.textMessage = "Please Insert Remark";
                    fKeypad.ShowDialog();

                    if (fKeypad.value != "")
                    {
                        pos.myOrderDet[0].Remark = fKeypad.value;
                    }
                }
                pos.OutstandingBalanceOrder = OutstandingBalance - amount;
                pos.OutstandingBalanceOverall = Installment.GetOutstandingBalancePerMember(membershipno) - amount;

                OrderHdr refOH = new OrderHdr(orderHdrID);
                string refOrderNo = string.IsNullOrEmpty(refOH.Userfld5) ? refOH.OrderHdrID : refOH.Userfld5;
                pos.SetHeaderRemark("Reference: " + refOrderNo);

                //pos.myOrderDet[0].UnitPrice = amount;
                //pos.getl(pos.get
                //pos.myOrderDet[0].UnitPrice = amount;
                pos.CalculateTotalAmount(out status);
                #endregion

                #region *) Initialize: Choose Sales Person
                SalesPersonInfo.SalesPersonID = UserInfo.username;
                SalesPersonInfo.SalesPersonName = UserInfo.displayName;
                SalesPersonInfo.SalesGroupID = UserInfo.SalesPersonGroupID;
                #endregion

                #region Handle Payment
                //decimal amount = BreakdownPrice;

                #region *) Core: Add PaymentList in ReceiptDetails
                // if credit note add default installment, amount minus
                bool isSuccess = false;
                if (!IsCreditNote)
                {
                    frmSelectPayment fPayment = new frmSelectPayment();
                    fPayment.syncSalesThread = syncSalesThread;
                    fPayment.pos = pos;
                    fPayment.amount = pos.CalculateTotalAmount(out status);
                    fPayment.ParentSyncPointsThread = ParentSyncPointsThread;
                    fPayment.IsCreditNote = IsCreditNote;
                    fPayment.ShowDialog();
                    isSuccess = fPayment.isSuccessful;
                    fPayment.Dispose();
                }
                else
                {
                    #region handlepayment installment
                    isSuccess = HandlePaymentCreditNote(-amount, pos, out status);
                    #endregion
                }
                #endregion

                //bool IsQtyInsufficient = false;
                /*
                #region *) Validation: Check if Qty is sufficient to do stockout [Prompt if Error - Can be terminated by user]
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
                #endregion

                bool IsPointAllocationSuccess;
                if (!pos.ConfirmOrder(false, out IsPointAllocationSuccess, out status))
                {
                    pos.DeleteAllReceiptLinePayment();
                    throw new Exception("(error)" + status);
                }

                //if (!IsQtyInsufficient)
                if (!pos.ExecuteStockOut(out status))
                {
                    MessageBox.Show
                        ("Error while performing Stock Out: " + status,
                        "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                Logger.writeLog("start printing");
                POSDeviceController.PrintAHAVATransactionReceipt(pos, 0, false,
                    (ReceiptSizes)Enum.ToObject(typeof(ReceiptSizes),
                    PrintSettingInfo.receiptSetting.PaperSize.Value),
                    PrintSettingInfo.receiptSetting.NumOfCopies.Value);
                Logger.writeLog("end printing");
                */
                #endregion

                if (isSuccess)
                {
                    bool UseCustomInvoiceNo = AppSetting.CastBool(AppSetting.GetSetting("UseCustomInvoiceNo"), false);
                    if (UseCustomInvoiceNo)
                    {
                        CustNumUpdate();//Graham
                    }

                    #region *) Update DepositAmount in OrderDet
                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Order.AutoAssignDepositUponInstPayment), false))
                    {
                        SyncClientController.Load_WS_URL();
                        PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                        ws.Timeout = 100000;
                        ws.Url = SyncClientController.WS_URL;

                        //if (!ws.AutoAssignDepositWhenpayInstallment(orderHdrID, amount, out status))
                        //{
                        //    MessageBox.Show(status);
                        //}
                    }
                    #endregion

                    //btnInstallment_Click(this, new EventArgs());
                    IsUsePackage = true;
                    MessageBox.Show("Payment is Successful");
                    this.Close();
                }
            }
            catch (Exception X)
            {
                CommonUILib.HandleException(X);
            }
        }

        private bool HandlePaymentCreditNote(decimal amount, POSController pos, out string statusmessage)
        {
            statusmessage = "";
            bool PrintReceipt = true;
            try
            {
                string status = "";
                string paymentType = POSController.PAY_INSTALLMENT;
                decimal change;

                #region *) Initialization: Clear/Delete all PaymentList in ReceiptDetails
                pos.DeleteAllReceiptLinePayment();
                #endregion

                #region *) Warning: Notice user if there is extra charge
                decimal ExtraChargeTotalAmount = pos.CheckExtraChargeAmount(paymentType, amount);
                if (ExtraChargeTotalAmount != 0)
                {
                    DialogResult DR = MessageBox.Show(
                        "There will be extra charge applicable of " + ExtraChargeTotalAmount.ToString("N2") + ". Do you want to continue?"
                        , "Extra Charge Applicable", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                    if (DR == DialogResult.Cancel) return false;
                }
                #endregion

                if (!pos.AddReceiptLinePayment(amount, paymentType, "", 0, "", 0, out change, out status))
                    throw new Exception(status);

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
                    }
                    #endregion

                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSales), false))
                       if (!syncSalesThread.IsBusy)
                            syncSalesThread.RunWorkerAsync();                    
                    

                    tryDownloadPoints(pos);
                    //print receipt
                    if (PrintReceipt)
                    {
                        POSDeviceController.PrintAHAVATransactionReceipt(pos, 0, false,
                            (ReceiptSizes)Enum.ToObject(typeof(ReceiptSizes),
                            PrintSettingInfo.receiptSetting.PaperSize.Value),
                            PrintSettingInfo.receiptSetting.NumOfCopies.Value);
                    }

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
            catch (Exception ex)
            {
                statusmessage = ex.Message;
                return false;

            }
        }

        private void tryDownloadPoints(POSController pos)
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

        private void PayChkdInstallment(decimal totalOuntstanding, List<string> listOrderHdrId)
        {
            try
            {
                POSController pos = new POSController();
                string status;

                DataTable source = (DataTable)dgvInstSummary.DataSource;
                if (source == null) return;


                var allOrderHdrId = "'";
                foreach (var ordrHdr in listOrderHdrId)
                {
                    allOrderHdrId += ordrHdr + "','";
                }
                allOrderHdrId = allOrderHdrId.Substring(0, allOrderHdrId.Length - 2);
                // Get the row of this OrderHdrID
                DataRow[] rows = source.Select(string.Format("OrderHdrId in ({0})", allOrderHdrId));
                if (rows.Length <= 0)
                    throw new Exception(string.Format("Transaction {0} cannot be found.", allOrderHdrId));

                #region *) Initialize: Check & Assign Membership
                bool hasExpired = false;
                DateTime ExpiryDate = new DateTime(2010, 1, 1);

                if (MembershipController.IsExistingMember(membershipno, out hasExpired, out ExpiryDate))
                {
                    #region *) Assign Member to POS
                    if (pos.AssignMembership(membershipno, out status))
                    {
                        pos.ApplyMembershipDiscount();
                    }
                    #endregion
                }
                else if (hasExpired)
                {
                    //prompt window to allow bypass?
                    DialogResult dr = MessageBox.Show("This member has already expired on " + ExpiryDate + ".\nDo you want to allow it to continue using the card", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        #region Assign Expired Member To POS
                        if (pos.AssignExpiredMember(membershipno, out status))
                        {
                            pos.ApplyMembershipDiscount();
                        }
                        #endregion
                    }
                    else
                    {
                        MessageBox.Show("Process canceled by user", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
                else
                {
                    throw new Exception("Member does not exist.\nPlease try again.");
                }
                #endregion

                #region *) Initialize: Check & Assign Item
                bool IsCreditNote = false;

                frmSelectInstallmentPay fselect = new frmSelectInstallmentPay();
                fselect.ShowDialog();

                if (fselect.IsSuccesful)
                {
                    IsCreditNote = fselect.IsCreditNote;
                }

                Item theItem = IsCreditNote ? new Item(Item.Columns.ItemNo, "CREDIT_NOTE") : new Item(Item.Columns.ItemNo, "INST_PAYMENT");


                //item exist?
                if (theItem.IsNew || !theItem.IsLoaded)
                    throw new Exception("(error)Cannot find item.\nPlease try again.");

                decimal MaxInstPayment = totalOuntstanding;

                decimal amount = 0;
                do
                {
                    frmKeypad keypad = new frmKeypad();
                    keypad.textMessage = "Total : " + totalOuntstanding.ToString("N2");
                    keypad.initialValue = "0";
                    keypad.ShowDialog();
                    decimal.TryParse(keypad.value, out amount);

                    if (amount == 0)
                    { pos = null; return; }

                    if (amount != MaxInstPayment)
                        MessageBox.Show("Outstanding amount for this receipt is  " + totalOuntstanding.ToString("N")
                            , "Payment have to be same amount ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                } while (amount != MaxInstPayment);


                theItem.RetailPrice = amount;
                if (!(pos.AddItemToOrder(theItem, 1, 0, false, out status)))
                    throw new Exception("(warning)Error creating payment item." + status);

                pos.myOrderDet[0].InstRefNo = allOrderHdrId.Replace("'", "");
                if (IsCreditNote)
                {
                    frmKeyboard fKeypad = new frmKeyboard();
                    fKeypad.textMessage = "Please Insert Remark";
                    fKeypad.ShowDialog();

                    if (fKeypad.value != "")
                    {
                        pos.myOrderDet[0].Remark = fKeypad.value;
                    }
                }
                pos.OutstandingBalanceOrder = totalOuntstanding - amount;
                pos.OutstandingBalanceOverall = Installment.GetOutstandingBalancePerMember(membershipno) - amount;

                string refOrderNo = "";

                foreach (var ordrHdr in listOrderHdrId)
                {
                    foreach (var row in rows)
                    {
                        if (ordrHdr == row["OrderHdrId"].ToString())
                        {
                            var refNo = row["CustomReceiptNo"].ToString();
                            refOrderNo += refNo + ",";
                            break;
                        }
                    }    
                }
                refOrderNo = refOrderNo.Substring(0, refOrderNo.Length - 1);

                pos.SetHeaderRemark("Reference: " + refOrderNo);

                //pos.myOrderDet[0].UnitPrice = amount;
                //pos.getl(pos.get
                //pos.myOrderDet[0].UnitPrice = amount;
                pos.CalculateTotalAmount(out status);
                #endregion

                #region *) Initialize: Choose Sales Person
                SalesPersonInfo.SalesPersonID = UserInfo.username;
                SalesPersonInfo.SalesPersonName = UserInfo.displayName;
                SalesPersonInfo.SalesGroupID = UserInfo.SalesPersonGroupID;
                #endregion

                #region Handle Payment
                //decimal amount = BreakdownPrice;

                              #region *) Core: Add PaymentList in ReceiptDetails
                // if credit note add default installment, amount minus
                bool isSuccess = false;
                if (!IsCreditNote)
                {
                    frmSelectPayment fPayment = new frmSelectPayment();
                    fPayment.syncSalesThread = syncSalesThread;
                    fPayment.pos = pos;
                    fPayment.amount = pos.CalculateTotalAmount(out status);
                    fPayment.ParentSyncPointsThread = ParentSyncPointsThread;
                    fPayment.IsCreditNote = IsCreditNote;
                    fPayment.ShowDialog();
                    isSuccess = fPayment.isSuccessful;
                    fPayment.Dispose();
                }
                else
                {
                    #region handlepayment installment
                    isSuccess = HandlePaymentCreditNote(-amount, pos, out status);
                    #endregion
                }
                #endregion


                #endregion
                if (isSuccess)
                {
                    bool UseCustomInvoiceNo = AppSetting.CastBool(AppSetting.GetSetting("UseCustomInvoiceNo"), false);
                    if (UseCustomInvoiceNo)
                    {
                        CustNumUpdate();//Graham
                    }

                    #region *) Update DepositAmount in OrderDet
                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Order.AutoAssignDepositUponInstPayment), false))
                    {
                        foreach (var ordrHdr in listOrderHdrId)
                        {
                            OrderHdr refOH = new OrderHdr(ordrHdr);
                            QueryCommandCollection cmdColl = new QueryCommandCollection();
                            OrderDetCollection refOD = refOH.OrderDetRecords();
                            decimal remainingAmt = amount;
                            foreach (OrderDet od in refOD)
                            {
                                if (od.Amount == null) od.Amount = 0;
                                if (od.DepositAmount == null) od.DepositAmount = 0;

                                if (od.Amount > 0 && od.Amount > od.DepositAmount)
                                {
                                    decimal discrepancy = od.Amount - od.DepositAmount;
                                    if (discrepancy <= remainingAmt)
                                    {
                                        od.DepositAmount += discrepancy;
                                        remainingAmt -= discrepancy;
                                    }
                                    else
                                    {
                                        od.DepositAmount += remainingAmt;
                                        remainingAmt = 0;
                                    }
                                    if (remainingAmt <= 0) break;
                                }
                            }

                            #region *) Send Deposit Amount to server
                            DataSet ds = new DataSet();
                            DataTable dt = new DataTable("DepositAmount");
                            dt.Columns.Add("OrderDetID", Type.GetType("System.String"));
                            dt.Columns.Add("DepositAmount", Type.GetType("System.Decimal"));
                            foreach (OrderDet od in refOD)
                            {
                                if (od.IsDirty)
                                    dt.Rows.Add(od.OrderDetID, od.DepositAmount);
                            }
                            ds.Tables.Add(dt);
                            SyncClientController.UpdateDepositAmountToServer(ds);
                            #endregion

                            cmdColl.AddRange(refOD.GetSaveCommands(UserInfo.username));
                            DataService.ExecuteTransaction(cmdColl);
                        }
                    }
                    #endregion

                    //btnInstallment_Click(this, new EventArgs());
                    IsUsePackage = true;
                    MessageBox.Show("Payment is Successful");
                    this.Close();
                }


            }
            catch (Exception X)
            {
                CommonUILib.HandleException(X);
            }

        }

        private void chkAllTransaction_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAllTransaction.Checked)
            {
                foreach (DataGridViewRow row in dgvInstSummary.Rows)
                {
                    DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells["CheckPay"];
                    chk.Value = chk.TrueValue;
                }
            }
            else
            {
                foreach (DataGridViewRow row in dgvInstSummary.Rows)
                {
                    DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells["CheckPay"];
                    chk.Value = chk.FalseValue;
                }
            }
        }

        private void btnPayAllChkdTransaction_Click(object sender, EventArgs e)
        {
            decimal totalAmount = 0;
            List<string> chkdInstallment = new List<string>();
            foreach (DataGridViewRow row in dgvInstSummary.Rows)
            {
                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells["CheckPay"];
                decimal amount = decimal.Parse(row.Cells["dgvcInsSumOutstanding"].Value.ToString());
                string orderhdrid = row.Cells["dgvcInsSumOrderHdrId"].Value.ToString();
                if (chk.Value == chk.TrueValue)
                {
                    totalAmount += amount;
                    chkdInstallment.Add(orderhdrid);
                }
            }

            if (totalAmount == 0)
            {
                MessageBox.Show("Need To Select At Least 1 Outstanding Installment");
            }
            else
            {
                MessageBox.Show("Total Checked Outstanding Installment : " + totalAmount.ToString("N2"));
                PayChkdInstallment(totalAmount, chkdInstallment);

            }
        }

    }
}
