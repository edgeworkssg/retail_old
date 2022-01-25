using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using PowerPOS.Container;
using System.Linq;
using PowerPOS;
using SubSonic;
using SubSonic.Utilities;

namespace PowerWeb.Reports
{
    public partial class DailySalesReport : PageBase
    {
        private const int TOTAL_SALES = 12;
        private const int TOTAL_CUSTOMER = 8;
        private const int TOTAL_TRANS = 5;
        private const int TOTAL_CASHNETS = 4;
        private const int TOTAL_POINTS = 6;
        private const int TOTAL_INSTALLMENT = 7;

        private const string SORT_DIRECTION = "SORT_DIRECTION";
        private const string ORDER_BY = "ORDER_BY";

        private bool showPointInstallmentBreakdown = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            SetFormSetting();
            if (!Page.IsPostBack)
            {
                ddlYear.Items.Clear();
                //for (int i = DateTime.Now.Year - 5; i < DateTime.Now.Year + 5; i++)
                //    ddlYear.Items.Add(i.ToString());
                int minyear = UtilityController.GetMinOrderDateYear();
                for (int i = DateTime.Today.Year; i >= minyear; i--)
                {
                    ddlYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
                }

                ddlMonth.SelectedValue = DateTime.Today.Month.ToString();
                ddlYear.SelectedValue = DateTime.Today.Year.ToString();
                txtStartDate.Text = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).ToString("dd MMM yyyy");
                txtEndDate.Text = DateTime.Today.AddDays(-1).ToString("dd MMM yyyy");
                ViewState["sortBy"] = "";
                ViewState[SORT_DIRECTION] = "";
                string queryStr = Request.QueryString["ExportData"] + "";
                if (queryStr.ToUpper().Equals("YES"))
                    ExportData();
                else
                    BindGrid();
            }
        }

        private void ExportData()
        {
            string userName = (Request.QueryString["UserName"] + "");
            Session["UserName"] = userName;
            DateTime startDate = (Request.QueryString["StartDate"] + "").GetDateTimeValue("dd MMM yyyy");
            DateTime endDate = (Request.QueryString["EndDate"] + "").GetDateTimeValue("dd MMM yyyy");
            string pointOfSale = Request.QueryString["PointOfSale"] + "";
            string outlet = Request.QueryString["Outlet"] + "";
            bool showBeforGST = (Request.QueryString["ShowAmountBeforeGST"] + "") == "1";
            chkShowAmountBeforeGST.Checked = showBeforGST;
            txtStartDate.Text = startDate.ToString("dd MMM yyyy");
            txtEndDate.Text = endDate.ToString("dd MMM yyyy");

            OutletDropdownList.SetDdlOutletSelectedValue(outlet);
            OutletDropdownList.SetDdlPOSSelectedValue(pointOfSale);
            lnkExport_Click(lnkExport, new EventArgs());
        }

        private void SetFormSetting()
        {
            try
            {
                bool enableMallManagement = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.MallIntegration.UseMallManagement), false);
                string posText = LanguageManager.GetTranslation(LabelController.PointOfSaleText);
                string outletText = LanguageManager.GetTranslation(LabelController.OutletText);
                gvReport.Columns[3].HeaderText = posText;
                gvReport.Columns[13].HeaderText = outletText;
                OutletDropdownList.SetLabelPOS(posText);
                OutletDropdownList.SetLabelOutlet(outletText);
                string attribute1 = AppSetting.GetSetting(AppSetting.SettingsName.MallIntegration.RetailerLevelAttribute1) + "";
                string attribute2 = AppSetting.GetSetting(AppSetting.SettingsName.MallIntegration.RetailerLevelAttribute2) + "";

                gvReport.Columns[10].HeaderText = attribute1;
                gvReport.Columns[11].HeaderText = attribute2;
                gvReport.Columns[10].Visible = !string.IsNullOrEmpty(attribute1) && enableMallManagement;
                gvReport.Columns[11].Visible = !string.IsNullOrEmpty(attribute2) && enableMallManagement;
                gvReport.Columns[1].Visible = enableMallManagement;
                gvReport.Columns[2].Visible = enableMallManagement;
                gvReport.Columns[9].Visible = enableMallManagement;

                showPointInstallmentBreakdown = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Reports.ShowPointInstallmentBreakdownInDailySales), false);
                gvReport.Columns[TOTAL_CASHNETS].Visible = showPointInstallmentBreakdown;
                gvReport.Columns[TOTAL_POINTS].Visible = showPointInstallmentBreakdown;
                gvReport.Columns[TOTAL_INSTALLMENT].Visible = showPointInstallmentBreakdown;
                gvReport.Columns[TOTAL_SALES].Visible = !showPointInstallmentBreakdown;
                if (showPointInstallmentBreakdown)
                {
                    gvReport.Columns[TOTAL_CASHNETS].HeaderText = "Total Sales";
                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Reports.SeparateInstallmentPaymentInDailySales), false))
                    {
                        gvReport.Columns[TOTAL_INSTALLMENT].HeaderText = "Total Installment Payment";
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        private void BindGrid()
        {
            try
            {
                DataService.ExecuteQuery(new QueryCommand("SET ANSI_NULLS ON"));

                if(string.IsNullOrEmpty(ViewState["sortBy"]+""))
                    ViewState["sortBy"] = "SalesDate";

                DateTime startDate = (txtStartDate.Text).GetDateTimeValue("dd MMM yyyy");
                DateTime endDate = (txtEndDate.Text).GetDateTimeValue("dd MMM yyyy");

                DataTable dt =
                    ReportController.FetchDailySalesReport(startDate, endDate, 0,
                    OutletDropdownList.GetDdlOutletSelectedItemText, (OutletDropdownList.GetDdlPOSSelectedValue + "").GetIntValue(),
                    chkShowAmountBeforeGST.Checked,
                    ViewState["sortBy"]+"", ViewState[SORT_DIRECTION]+"");
                dt.Columns.Add("TotalSalesSTR", typeof(string));
                dt.Columns.Add("TotalTransSTR", typeof(string));

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (string.IsNullOrEmpty((dt.Rows[i]["TotalTrans"] + "")))
                    {
                        dt.Rows[i]["TotalSalesSTR"] = "-";
                        dt.Rows[i]["TotalTransSTR"] = "-";
                    }
                    else
                    {
                        dt.Rows[i]["TotalSalesSTR"] = (dt.Rows[i]["TotalSales"] + "").GetDecimalValue().ToString("N2");
                        dt.Rows[i]["TotalTransSTR"] = (dt.Rows[i]["TotalTrans"] + "").GetIntValue(); 
                    }
                }

                DataView dv = new DataView();
                dt.TableName = "Report";
                dv.Table = dt;
                dv.Sort = ViewState["sortBy"].ToString() + " " + ViewState[SORT_DIRECTION].ToString();
                gvReport.DataSource = dv;
                gvReport.DataBind();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        protected void gvReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvReport.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void gvReport_DataBound(Object sender, EventArgs e)
        {
            GridViewRow gvrPager = gvReport.BottomPagerRow;
            if (gvrPager == null)
            {
                return;
            }

            // get your controls from the gridview
            DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages");
            Label lblPageCount = (Label)gvrPager.Cells[0].FindControl("lblPageCount");
            if (ddlPages != null)
            {
                // populate pager
                for (int i = 0; i < gvReport.PageCount; i++)
                {
                    int intPageNumber = i + 1;
                    ListItem lstItem = new ListItem(intPageNumber.ToString());
                    if (i == gvReport.PageIndex)
                    {
                        lstItem.Selected = true;
                    }

                    ddlPages.Items.Add(lstItem);
                }

            }

            int itemCount = 0;
            // populate page count
            if (lblPageCount != null)
            {
                //pull the datasource
                DataSet ds = gvReport.DataSource as DataSet;
                if (ds != null)
                {
                    itemCount = ds.Tables[0].Rows.Count;
                }

                string pageCount = "<b>" + gvReport.PageCount.ToString() + "</b>";
                lblPageCount.Text = pageCount;
            }

            Button btnPrev = (Button)gvrPager.Cells[0].FindControl("btnPrev");
            Button btnNext = (Button)gvrPager.Cells[0].FindControl("btnNext");
            Button btnFirst = (Button)gvrPager.Cells[0].FindControl("btnFirst");
            Button btnLast = (Button)gvrPager.Cells[0].FindControl("btnLast");
            //now figure out what page we're on
            if (gvReport.PageIndex == 0)
            {
                btnPrev.Enabled = false;
                btnFirst.Enabled = false;
            }

            else if (gvReport.PageIndex + 1 == gvReport.PageCount)
            {
                btnLast.Enabled = false;
                btnNext.Enabled = false;
            }

            else
            {
                btnLast.Enabled = true;
                btnNext.Enabled = true;
                btnPrev.Enabled = true;
                btnFirst.Enabled = true;
            }

        }
        protected void gvReport_Sorting(object sender, GridViewSortEventArgs e)
        {
            ViewState["sortBy"] = e.SortExpression;
            //rebind the grid
            if (ViewState[SORT_DIRECTION] == null || ((string)ViewState[SORT_DIRECTION]) == SqlFragment.ASC)
            {
                ViewState[SORT_DIRECTION] = SqlFragment.DESC;
            }

            else
            {
                ViewState[SORT_DIRECTION] = SqlFragment.ASC;
            }

            BindGrid();
        }
        protected void ddlPages_SelectedIndexChanged(Object sender, EventArgs e)
        {
            GridViewRow gvrPager = gvReport.BottomPagerRow;
            DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages");
            gvReport.PageIndex = ddlPages.SelectedIndex;
            // a method to populate your grid
            BindGrid();
        }
        string GetSortDirection(string sortBy)
        {
            string sortDir = " ASC";
            if (ViewState["sortBy"] != null)
            {
                string sortedBy = ViewState["sortBy"].ToString();
                if (sortedBy == sortBy)
                {
                    //the direction should be desc
                    sortDir = " DESC";
                    //reset the sorter to null
                    ViewState["sortBy"] = null;
                }
                else
                {
                    //this is the first sort for this row
                    //put it to the ViewState
                    ViewState["sortBy"] = sortBy;
                }
            }
            else
            {
                //it's null, so this is the first sort
                ViewState["sortBy"] = sortBy;
            }
            return sortDir;
        }

        bool isUsingMallIntegration = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.MallIntegration.UseMallManagement), false);
        protected void gvReport_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //string val = (e.Row.Cells[TOTAL_TRANS].Text + "").Replace("&nbsp;","");
                //if (!string.IsNullOrEmpty(val))
                //    e.Row.Cells[TOTAL_SALES].Text =  val.GetDecimalValue().ToString("N2");
                //else
                //{
                //    if (isUsingMallIntegration)
                //    {
                //        e.Row.Cells[TOTAL_TRANS].Text = "-";
                //        e.Row.Cells[TOTAL_SALES].Text = "-";
                //    }
                //    else
                //    {
                //        e.Row.Cells[TOTAL_TRANS].Text = "0";
                //        e.Row.Cells[TOTAL_SALES].Text = "$0.00";
                //    }
                //}

                Label TotalSales = (Label)e.Row.FindControl("TotalSales");
                if (showPointInstallmentBreakdown && TotalSales != null)
                {
                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Reports.SeparateInstallmentPaymentInDailySales), false))
                    {
                        QueryCommand qc = new QueryCommand(@"
                            SELECT
	                            SUM(CASE WHEN OD.ItemNo <> 'INST_PAYMENT' THEN OD.Amount ELSE 0 END) as TotalSales,
	                            0 as TotalPoints,
	                            SUM(CASE WHEN OD.ItemNo = 'INST_PAYMENT' THEN OD.Amount ELSE 0 END) as TotalInstallment
                            FROM OrderHdr OH
                            INNER JOIN OrderDet OD ON OH.OrderHdrID = OD.OrderHdrID 
                            INNER JOIN PointOfSale POS ON POS.PointOfSaleID = OH.PointOfSaleID
                            INNER JOIN Outlet OU ON OU.OutletName = POS.OutletName
                            WHERE
	                            OH.OrderDate BETWEEN @StartDate AND @EndDate
                                AND (POS.PointOfSaleName = @PointOfSaleName OR @PointOfSaleName = 'ALL' OR @PointOfSaleName = 'ALL - BreakDown')
                                AND (OU.OutletName = @OutletName OR @OutletName = 'ALL' OR @OutletName = 'ALL - BreakDown')
                                AND ISNULL(OH.IsVoided,0) = 0
                        ", "PowerPOS");
                        DateTime dt = DateTime.Now;
                        if (DateTime.TryParse(e.Row.Cells[0].Text, out dt))
                        {
                            qc.AddParameter("@StartDate", dt.ToString("yyyy-MM-dd") + " 00:00:00");
                            qc.AddParameter("@EndDate", dt.ToString("yyyy-MM-dd") + " 23:59:59");
                            qc.AddParameter("@PointOfSaleName", Server.HtmlDecode(e.Row.Cells[3].Text));
                            qc.AddParameter("@OutletName", Server.HtmlDecode(e.Row.Cells[13].Text));

                            decimal totalSales = 0;
                            decimal totalPoints = 0;
                            decimal totalInstallment = 0;

                            IDataReader rdr = DataService.GetReader(qc);
                            if (rdr.Read())
                            {
                                decimal.TryParse(rdr["TotalSales"].ToString(), out totalSales);
                                decimal.TryParse(rdr["TotalPoints"].ToString(), out totalPoints);
                                decimal.TryParse(rdr["TotalInstallment"].ToString(), out totalInstallment);

                                Label lblTotalSales = (Label)e.Row.FindControl("TotalSales");
                                if (lblTotalSales != null)
                                    lblTotalSales.Text = totalSales.ToString("N2");

                                Label lblTotalPoints = (Label)e.Row.FindControl("TotalPointsRedeem");
                                if (lblTotalPoints != null)
                                    lblTotalPoints.Text = totalPoints.ToString("N2");

                                Label lblTotalInstallment = (Label)e.Row.FindControl("TotalInstallment");
                                if (lblTotalInstallment != null)
                                    lblTotalInstallment.Text = totalInstallment.ToString("N2");
                            }
                        }
                    }
                    else
                    {
                        QueryCommand qc = new QueryCommand(@"
                            SELECT
	                            SUM(CASE WHEN RD.PaymentType <> 'POINTS' AND RD.PaymentType <> 'INSTALLMENT' THEN RD.Amount ELSE 0 END) as TotalSales,
	                            SUM(CASE WHEN RD.PaymentType = 'POINTS' THEN RD.Amount ELSE 0 END) as TotalPoints,
	                            SUM(CASE WHEN RD.PaymentType = 'INSTALLMENT' THEN RD.Amount ELSE 0 END) as TotalInstallment
                            FROM OrderHdr OH
                            INNER JOIN ReceiptHdr RH ON RH.OrderHdrID = OH.OrderHdrID
                            INNER JOIN ReceiptDet RD ON RD.ReceiptHdrID = RH.ReceiptHdrID
                            INNER JOIN PointOfSale POS ON POS.PointOfSaleID = OH.PointOfSaleID
                            INNER JOIN Outlet OU ON OU.OutletName = POS.OutletName
                            WHERE
	                            OH.OrderDate BETWEEN @StartDate AND @EndDate
                                AND (POS.PointOfSaleName = @PointOfSaleName OR @PointOfSaleName = 'ALL' OR @PointOfSaleName = 'ALL - BreakDown')
                                AND (OU.OutletName = @OutletName OR @OutletName = 'ALL' OR @OutletName = 'ALL - BreakDown')
                                AND ISNULL(OH.IsVoided,0) = 0
                        ", "PowerPOS");
                        DateTime dt = DateTime.Now;
                        if (DateTime.TryParse(e.Row.Cells[0].Text, out dt))
                        {
                            qc.AddParameter("@StartDate", dt.ToString("yyyy-MM-dd") + " 00:00:00");
                            qc.AddParameter("@EndDate", dt.ToString("yyyy-MM-dd") + " 23:59:59");
                            qc.AddParameter("@PointOfSaleName", Server.HtmlDecode(e.Row.Cells[3].Text));
                            qc.AddParameter("@OutletName", Server.HtmlDecode(e.Row.Cells[13].Text));

                            decimal totalSales = 0;
                            decimal totalPoints = 0;
                            decimal totalInstallment = 0;

                            IDataReader rdr = DataService.GetReader(qc);
                            if (rdr.Read())
                            {
                                decimal.TryParse(rdr["TotalSales"].ToString(), out totalSales);
                                decimal.TryParse(rdr["TotalPoints"].ToString(), out totalPoints);
                                decimal.TryParse(rdr["TotalInstallment"].ToString(), out totalInstallment);

                                Label lblTotalSales = (Label)e.Row.FindControl("TotalSales");
                                if (lblTotalSales != null)
                                    lblTotalSales.Text = totalSales.ToString("N2");

                                Label lblTotalPoints = (Label)e.Row.FindControl("TotalPointsRedeem");
                                if (lblTotalPoints != null)
                                    lblTotalPoints.Text = totalPoints.ToString("N2");

                                Label lblTotalInstallment = (Label)e.Row.FindControl("TotalInstallment");
                                if (lblTotalInstallment != null)
                                    lblTotalInstallment.Text = totalInstallment.ToString("N2");
                            }
                        }
                    }

                }
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                DataTable dt = ((DataView)gvReport.DataSource).Table;
                if (dt != null && dt.Rows.Count > 0)
                {
                    e.Row.Cells[TOTAL_SALES].Text = (dt.Compute("SUM(TotalSales)", "").ToString()).GetDecimalValue().ToString("N2"); // totalGrossSales.ToString("N2");
                    e.Row.Cells[TOTAL_TRANS].Text = (dt.Compute("SUM(TotalTrans)", "").ToString()).GetIntValue().ToString(); // totalGrossSales.ToString("N2");
                    e.Row.Cells[TOTAL_CUSTOMER].Text = (dt.Compute("SUM(TotalCustomer)", "").ToString()).GetIntValue().ToString(); // totalGrossSales.ToString("N2");}

                    if (showPointInstallmentBreakdown)
                    {
                        if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Reports.SeparateInstallmentPaymentInDailySales), false))
                        {
                            QueryCommand qc = new QueryCommand(@"
                    SELECT
                        SUM(CASE WHEN OD.ItemNo <> 'INST_PAYMENT' THEN OD.Amount ELSE 0 END) as TotalSales,
	                            0 as TotalPoints,
	                            SUM(CASE WHEN OD.ItemNo = 'INST_PAYMENT' THEN OD.Amount ELSE 0 END) as TotalInstallment
                    FROM OrderHdr OH
                    INNER JOIN OrderDet OD ON OD.OrderHdrID = OH.OrderHdrID
                    INNER JOIN PointOfSale POS ON POS.PointOfSaleID = OH.PointOfSaleID
                    INNER JOIN Outlet OU ON OU.OutletName = POS.OutletName
                    WHERE
                        OH.OrderDate BETWEEN @StartDate AND @EndDate
                        AND (OH.PointOfSaleID = @POSID OR @POSID = 0 OR @POSID = -1)
                        AND (OU.OutletName = @OutletName OR @OutletName = 'ALL' OR @OutletName = 'ALL - BreakDown')
                        AND ISNULL(OH.IsVoided,0) = 0
                    ", "PowerPOS");

                            DateTime startDate = (txtStartDate.Text).GetDateTimeValue("dd MMM yyyy");
                            DateTime endDate = (txtEndDate.Text).GetDateTimeValue("dd MMM yyyy");


                            qc.AddParameter("@StartDate", startDate.ToString("yyyy-MM-dd") + " 00:00:00");
                            qc.AddParameter("@EndDate", endDate.ToString("yyyy-MM-dd") + " 23:59:59");
                            qc.AddParameter("@POSID", (OutletDropdownList.GetDdlPOSSelectedValue + "").GetIntValue());
                            qc.AddParameter("@OutletName", OutletDropdownList.GetDdlOutletSelectedItemText);

                            decimal totalSales = 0;
                            decimal totalPoints = 0;
                            decimal totalInstallment = 0;

                            IDataReader rdr = DataService.GetReader(qc);
                            if (rdr.Read())
                            {
                                decimal.TryParse(rdr["TotalSales"].ToString(), out totalSales);
                                decimal.TryParse(rdr["TotalPoints"].ToString(), out totalPoints);
                                decimal.TryParse(rdr["TotalInstallment"].ToString(), out totalInstallment);

                                Label lblTotalSales = (Label)e.Row.FindControl("TotalSales");
                                if (lblTotalSales != null)
                                    lblTotalSales.Text = totalSales.ToString("N2");

                                Label lblTotalPoints = (Label)e.Row.FindControl("TotalPointsRedeem");
                                if (lblTotalPoints != null)
                                    lblTotalPoints.Text = totalPoints.ToString("N2");

                                Label lblTotalInstallment = (Label)e.Row.FindControl("TotalInstallment");
                                if (lblTotalInstallment != null)
                                    lblTotalInstallment.Text = totalInstallment.ToString("N2");
                            }
                        }
                        else
                        {
                            QueryCommand qc = new QueryCommand(@"
                    SELECT
                        SUM(CASE WHEN RD.PaymentType <> 'POINTS' AND RD.PaymentType <> 'INSTALLMENT' THEN RD.Amount ELSE 0 END) as TotalSales,
                        SUM(CASE WHEN RD.PaymentType = 'POINTS' THEN RD.Amount ELSE 0 END) as TotalPoints,
                        SUM(CASE WHEN RD.PaymentType = 'INSTALLMENT' THEN RD.Amount ELSE 0 END) as TotalInstallment
                    FROM OrderHdr OH
                    INNER JOIN ReceiptHdr RH ON RH.OrderHdrID = OH.OrderHdrID
                    INNER JOIN ReceiptDet RD ON RD.ReceiptHdrID = RH.ReceiptHdrID
                    INNER JOIN PointOfSale POS ON POS.PointOfSaleID = OH.PointOfSaleID
                    INNER JOIN Outlet OU ON OU.OutletName = POS.OutletName
                    WHERE
                        OH.OrderDate BETWEEN @StartDate AND @EndDate
                        AND (OH.PointOfSaleID = @POSID OR @POSID = 0 OR @POSID = -1)
                        AND (OU.OutletName = @OutletName OR @OutletName = 'ALL' OR @OutletName = 'ALL - BreakDown')
                        AND ISNULL(OH.IsVoided,0) = 0
                    ", "PowerPOS");

                            DateTime startDate = (txtStartDate.Text).GetDateTimeValue("dd MMM yyyy");
                            DateTime endDate = (txtEndDate.Text).GetDateTimeValue("dd MMM yyyy");


                            qc.AddParameter("@StartDate", startDate.ToString("yyyy-MM-dd") + " 00:00:00");
                            qc.AddParameter("@EndDate", endDate.ToString("yyyy-MM-dd") + " 23:59:59");
                            qc.AddParameter("@POSID", (OutletDropdownList.GetDdlPOSSelectedValue + "").GetIntValue());
                            qc.AddParameter("@OutletName", OutletDropdownList.GetDdlOutletSelectedItemText);

                            decimal totalSales = 0;
                            decimal totalPoints = 0;
                            decimal totalInstallment = 0;

                            IDataReader rdr = DataService.GetReader(qc);
                            if (rdr.Read())
                            {
                                decimal.TryParse(rdr["TotalSales"].ToString(), out totalSales);
                                decimal.TryParse(rdr["TotalPoints"].ToString(), out totalPoints);
                                decimal.TryParse(rdr["TotalInstallment"].ToString(), out totalInstallment);

                                Label lblTotalSales = (Label)e.Row.FindControl("TotalSales");
                                if (lblTotalSales != null)
                                    lblTotalSales.Text = totalSales.ToString("N2");

                                Label lblTotalPoints = (Label)e.Row.FindControl("TotalPointsRedeem");
                                if (lblTotalPoints != null)
                                    lblTotalPoints.Text = totalPoints.ToString("N2");

                                Label lblTotalInstallment = (Label)e.Row.FindControl("TotalInstallment");
                                if (lblTotalInstallment != null)
                                    lblTotalInstallment.Text = totalInstallment.ToString("N2");
                            }
                        }
                    }
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (rdbMonth.Checked)
            {
                int selectedMonth = int.Parse(ddlMonth.SelectedValue);
                DateTime startDate = new DateTime((ddlYear.SelectedValue+"").GetIntValue(), selectedMonth, 1);
                txtStartDate.Text =
                    startDate.ToString("dd MMM yyyy");

                txtEndDate.Text = startDate.AddMonths(1).AddDays(-1).ToString("dd MMM yyyy");
            }
            BindGrid();
        }
        protected void btnClear_Click(object sender, EventArgs e)
        {

            OutletDropdownList.ResetDdlOutlet();
            OutletDropdownList.ResetDdlPOS();
            txtStartDate.Text = DateTime.Today.ToString("dd MMM yyyy");
            txtEndDate.Text = DateTime.Today.ToString("dd MMM yyyy");
            gvReport.PageIndex = 0;
        }
        protected void lnkExport_Click(object sender, EventArgs e)
        {
            BindGrid();
            DataTable dt = ((DataView)gvReport.DataSource).Table;
            CommonWebUILib.ExportCSV(this.Page.Title.Trim(' '), "Daily Sales Report", gvReport, Response);
        }


    }
}
