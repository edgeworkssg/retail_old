using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PowerPOS;
using SubSonic;
using System.IO;
using SpreadsheetLight;

public partial class AggregatedSalesReport : PageBase
{
    private const string SORT_DIRECTION = "SORT_DIRECTION";
    private const string ORDER_BY = "ORDER_BY";

    private const int HIST_COL_LogDate = 1;
    private const int HIST_COL_StartDate = 2;
    private const int HIST_COL_EndDate = 3;
    private const int HIST_COL_PointOfSale = 4;
    private const int HIST_COL_TransactionType = 5;
    private const int HIST_COL_Filename = 6;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            List<ListItem> listYear = new List<ListItem>();
            int minyear = UtilityController.GetMinOrderDateYear();
            for (int i = DateTime.Today.Year; i >= minyear; i--)
            {
                listYear.Add(new ListItem(i.ToString(), i.ToString()));
            }
            ddlYear.Items.AddRange(listYear.ToArray());
            ddlYear.SelectedIndex = 0;
            ddlMonth.SelectedValue = DateTime.Today.Month.ToString();
            txtStartDate.Text = DateTime.Today.AddDays(-7).ToString("dd MMM yyyy");
            txtEndDate.Text = DateTime.Today.AddDays(-1).ToString("dd MMM yyyy");
            //OutletDropdownList.setIsUseAllBreakdownOutlet(false);
            //OutletDropdownList.setIsUseAllBreakdownPOS(false);
            LoadDdlPOS();
            ViewState["sortBy"] = "";
            //ViewState[SORT_DIRECTION] = "";

            BindHistory();
            //BindGrid();
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        ViewState["sortBy"] = null;

        if (rdbMonth.Checked)
        {
            txtStartDate.Text = (new DateTime(int.Parse(ddlYear.Text), int.Parse(ddlMonth.SelectedValue), 1)).ToString("dd MMM yyyy");
            txtEndDate.Text = new DateTime(int.Parse(ddlYear.Text), int.Parse(ddlMonth.SelectedValue), DateTime.DaysInMonth(int.Parse(ddlYear.Text), int.Parse(ddlMonth.SelectedValue))).ToString("dd MMM yyyy");
        }

        if (ddlPOS.SelectedValue != "" && ddlTransactionType.SelectedValue != "")
        {
            SaveSearchHistory();
            BindHistory();
        }
        BindGrid();
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {

        //OutletDropdownList.ResetDdlOutlet();
        //OutletDropdownList.ResetDdlPOS();
        txtStartDate.Text = DateTime.Today.AddDays(-7).ToString("dd MMM yyyy");
        txtEndDate.Text = DateTime.Today.AddDays(-1).ToString("dd MMM yyyy");
        txtItemName.Text = "";
        ddlPOS.SelectedIndex = 0;
        ddlTransactionType.SelectedIndex = 0;

        //gvReport.PageIndex = 0;
    }

    private void LoadDdlPOS()
    {
        ddlPOS.Items.Clear();
        ddlPOS.Items.AddRange(PointOfSaleController.FetchPointOfSaleNames(false, Session["UserName"] + "", "ALL"));
        ddlPOS.Items.Insert(0, new ListItem("-- Please select --", ""));
    }

    private void BindGrid()
    {
        litMessage.Text = "";
        gvReport.DataSource = null;
        gvReport.DataBind();

        if (ViewState["sortBy"] == null || ViewState["sortBy"].ToString() == "")
            ViewState["sortBy"] = "CustomerCode";
        if (ViewState[SORT_DIRECTION] == null || ViewState[SORT_DIRECTION].ToString() == "")
            ViewState[SORT_DIRECTION] = "ASC";

        if (ddlPOS.SelectedValue == "")
        {
            litMessage.Text = "Point Of Sale is empty";
            return;
        }
        if (ddlTransactionType.SelectedValue == "")
        {
            litMessage.Text = "Transaction Type is empty";
            return;
        }
        DateTime startDate = DateTime.Parse(txtStartDate.Text);
        DateTime endDate = DateTime.Parse(txtEndDate.Text);

        #region *) Query command
        string sql = @"
                        SELECT CategoryName, ItemNo, ItemName, ABS(SUM(Quantity)) AS Quantity, UnitPrice, 
                            ROUND(SUM(TotalAmtInclGST),2) AS TotalAmtInclGST, ROUND(SUM(GSTAmount),2) AS GSTAmount, 
                            CASE WHEN SUM(GrossSales) = 0 THEN 0 ELSE ROUND((SUM(GrossSales-TotalAmtInclGST) / SUM(GrossSales) * 100),2) END AS DiscountPercent, 
                            ROUND(SUM(NettAmtWoGST),2) AS NettAmtWoGST, ROUND(SUM(DiscAmtWoGST),2) AS DiscAmtWoGST, ROUND(SUM(TotalAmtWoGST),2) AS TotalAmtWoGST, 
                            PaymentType, PointOfSaleName, OutletName, Remark, Attributes1, CustomerCode 
                        FROM (
                            SELECT it.CategoryName, it.ItemNo, it.ItemName, od.Quantity, it.RetailPrice AS UnitPrice, 
                                od.Amount AS TotalAmtInclGST, ROUND((od.Amount / 1.07 * 0.07), 4) AS GSTAmount, od.GrossSales, 
                                (od.Amount - ROUND((od.Amount / 1.07 * 0.07), 4)) AS NettAmtWoGST,
                                (od.GrossSales - od.Amount) / (1 + oh.GST/100) AS DiscAmtWoGST,
                                (od.Amount - ROUND((od.Amount / 1.07 * 0.07), 4)) + ((od.GrossSales - od.Amount) / (1 + oh.GST/100)) AS TotalAmtWoGST, --> NettAmtWoGST + DiscAmtWoGST
                                ISNULL(pay.PaymentType, 'CASH') AS PaymentType, pos.PointOfSaleName, pos.OutletName, it.Remark, it.Attributes1, sap.CustomerCode 
                            FROM OrderHdr oh
                                INNER JOIN OrderDet od ON od.OrderHdrID = oh.OrderHdrID
                                INNER JOIN Item it ON it.ItemNo = od.ItemNo
                                INNER JOIN PointOfSale pos ON pos.PointOfSaleID = oh.PointOfSaleID
                                LEFT JOIN (
                                        SELECT DISTINCT oh.OrderHdrID, 
                                            CASE ISNULL(MAX(rd.PaymentType), 'CASH') 
                                                WHEN 'CASH' THEN 'CASH'
		                                        WHEN 'VOUCHER' THEN 'CASH'
                                                WHEN 'PayPal' THEN 'E-PAYMENT'
                                                ELSE 'CARD'
                                            END AS PaymentType
                                        FROM OrderHdr oh
                                            LEFT JOIN ReceiptDet rd ON rd.ReceiptHdrID = oh.OrderHdrID
	                                        GROUP BY OH.OrderhdrID
	                                        HAVING COUNT(*) = 1
                                        UNION ALL 
                                        SELECT DISTINCT oh.OrderHdrID, 
                                            CASE ISNULL(MIN(rd.PaymentType), 'CASH') 
                                                WHEN 'CASH' THEN 'CASH'
		                                        WHEN 'VOUCHER' THEN 'CASH'
                                                WHEN 'PayPal' THEN 'E-PAYMENT'
                                                ELSE 'CARD'
                                            END AS PaymentType
                                        FROM OrderHdr oh
                                            LEFT JOIN ReceiptDet rd ON rd.ReceiptHdrID = oh.OrderHdrID
                                        	GROUP BY OH.OrderhdrID
                                        HAVING COUNT(*) > 1
                                    ) pay ON pay.OrderHdrID = oh.OrderHdrID 
                                LEFT JOIN SAPCustomerCode sap ON sap.SalesType = @TransactionType AND sap.PaymentType = pay.PaymentType AND sap.PointOfSaleID = pos.PointOfSaleID
                            WHERE oh.OrderDate BETWEEN @StartDate AND @EndDate
                                AND oh.IsVoided = 0 AND od.IsVoided = 0 
                                AND (@PointOfSaleID = 0 OR oh.PointOfSaleID = @PointOfSaleID)
                                AND 1 = CASE @TransactionType
                                            WHEN 'Normal sales' THEN CASE WHEN (od.Quantity > 0 AND od.Amount > 0) THEN 1 ELSE 0 END
                                            WHEN 'Refund with Product Return' THEN CASE WHEN (od.Quantity < 0 AND od.Amount < 0 AND ISNULL(od.InventoryHdrRefNo,'') <> 'NOTRETURNED') THEN 1 ELSE 0 END
                                            WHEN 'Refund without Product Return' THEN CASE WHEN (od.Quantity < 0 AND od.Amount < 0 AND ISNULL(od.InventoryHdrRefNo,'') = 'NOTRETURNED') THEN 1 ELSE 0 END
                                            WHEN 'Sample' THEN CASE WHEN (od.Quantity > 0 AND od.Amount = 0) THEN 1 ELSE 0 END
                                        END
                                AND it.CategoryName <> 'SYSTEM' AND ISNULL(it.Attributes1, '') <> ''
                        ) Res
                        GROUP BY CategoryName, ItemNo, ItemName, UnitPrice, PaymentType, PointOfSaleName, OutletName, Remark, Attributes1, CustomerCode 
                        ORDER BY {0} {1}
                     ";
        sql = string.Format(sql, ViewState["sortBy"].ToString(), ViewState[SORT_DIRECTION].ToString());
        QueryCommand cmd = new QueryCommand(sql, "PowerPOS");
        cmd.Parameters.Add("@StartDate", startDate, DbType.DateTime);
        cmd.Parameters.Add("@EndDate", endDate.AddSeconds(86399), DbType.DateTime);
        cmd.Parameters.Add("@PointOfSaleID", ddlPOS.SelectedValue, DbType.Int32);
        cmd.Parameters.Add("@TransactionType", ddlTransactionType.SelectedValue, DbType.String);
        #endregion

        DataTable dt = DataService.GetDataSet(cmd).Tables[0];
        gvReport.DataSource = dt;
        gvReport.DataBind();
    }

    private void SaveSearchHistory()
    {
        SaveSearchHistory("");
    }

    private void SaveSearchHistory(string histFile)
    {
        try
        {
            DataSet ds = new DataSet("SearchHistory");
            string path = AppDomain.CurrentDomain.BaseDirectory;
            string fileName = path + "AggregatedSalesReport.xml";

            DataTable dt = ds.Tables.Add();
            dt.Columns.Add("LogDate", Type.GetType("System.DateTime"));
            dt.Columns.Add("StartDate", Type.GetType("System.String"));
            dt.Columns.Add("EndDate", Type.GetType("System.String"));
            dt.Columns.Add("PointOfSale", Type.GetType("System.String"));
            dt.Columns.Add("TransactionType", Type.GetType("System.String"));
            dt.Columns.Add("Filename", Type.GetType("System.String"));
            
            if (File.Exists(fileName))
            {
                ds.ReadXml(fileName);
            }

            //DataTable dt = ds.Tables[0];
            //string histFile = "";
            string filter = "StartDate='{0}' AND EndDate='{1}' AND PointOfSale='{2}' AND TransactionType='{3}'";
            filter = string.Format(filter, txtStartDate.Text, txtEndDate.Text, ddlPOS.SelectedItem.Text, ddlTransactionType.SelectedValue);
            DataRow[] dr = dt.Select(filter);
            if (dr.Length > 0)
            {
                if (string.IsNullOrEmpty(histFile)) histFile = dr[0]["Filename"].ToString();
                dr[0].Delete();
            }

            DataRow newdr = dt.NewRow();
            newdr.ItemArray = new object[] { DateTime.Now, txtStartDate.Text, txtEndDate.Text, ddlPOS.SelectedItem.Text, ddlTransactionType.SelectedValue, histFile };
            dt.Rows.InsertAt(newdr, 0); // Always insert at top

            int maxHistory = AppSetting.CastInt(AppSetting.GetSetting(AppSetting.SettingsName.Reports.AggregatedSalesReportMaxHistory), 100);
            while (dt.Rows.Count > maxHistory)
            {
                dt.Rows.RemoveAt(maxHistory);
            }

            ds.WriteXml(fileName);
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
        }
    }

    private void BindHistory()
    {
        try
        {
            DataSet ds = new DataSet("SearchHistory");
            string path = AppDomain.CurrentDomain.BaseDirectory;
            string fileName = path + "AggregatedSalesReport.xml";

            DataTable dt = ds.Tables.Add();
            dt.Columns.Add("LogDate", Type.GetType("System.DateTime"));
            dt.Columns.Add("StartDate", Type.GetType("System.String"));
            dt.Columns.Add("EndDate", Type.GetType("System.String"));
            dt.Columns.Add("PointOfSale", Type.GetType("System.String"));
            dt.Columns.Add("TransactionType", Type.GetType("System.String"));
            dt.Columns.Add("Filename", Type.GetType("System.String"));

            if (File.Exists(fileName))
            {
                ds.ReadXml(fileName);
            }

            gvHistory.DataSource = ds.Tables[0];
            gvHistory.DataBind();
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
        }
    }

    protected void ddlPages_SelectedIndexChanged(Object sender, EventArgs e)
    {
        GridViewRow gvrPager = gvReport.BottomPagerRow;
        DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages");
        gvReport.PageIndex = ddlPages.SelectedIndex;
        // a method to populate your grid
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

    protected void gvReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvReport.PageIndex = e.NewPageIndex;
        BindGrid();
    }

    protected void gvReport_RowDataBound(object sender, GridViewRowEventArgs e)
    {
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        e.Row.Cells[AMOUNT_WITH_GST].Text = String.Format("{0:N2}", Decimal.Parse(e.Row.Cells[AMOUNT_WITH_GST].Text));
    //        //e.Row.Cells[GSTAMOUNT].Text = String.Format("{0:N2}", Decimal.Parse(e.Row.Cells[GSTAMOUNT].Text));
    //        //e.Row.Cells[AMOUNT].Text = String.Format("{0:N2}", Decimal.Parse(e.Row.Cells[AMOUNT].Text));            
    //        //e.Row.Cells[COGS].Text = String.Format("{0:N2}", Decimal.Parse(e.Row.Cells[COGS].Text));
    //        /*
    //        decimal tmp = Decimal.Parse(e.Row.Cells[PLPercent].Text);
    //        if (tmp >= -100 && tmp <= 100)
    //        {
    //            e.Row.Cells[PLPercent].Text = tmp.ToString("N2") + "%";
    //        }
    //        else
    //        {
    //            e.Row.Cells[PLPercent].Text = "ERR";
    //        }
    //        e.Row.Cells[PL].Text = String.Format("{0:N2}", Decimal.Parse(e.Row.Cells[PL].Text));            
    //         */
    //    }
    //    else if (e.Row.RowType == DataControlRowType.Footer)
    //    {
    //        DataTable dt = (DataTable)gvReport.DataSource;
    //        if (dt != null)
    //        {
    //            decimal Amount, PLAmt, AmountWithoutGST, GSTAmount;
    //            e.Row.Cells[QTY].Text = dt.Compute("SUM(TOTALQuantity)", "").ToString();
    //            Amount = decimal.Parse(dt.Compute("SUM(TOTALAMOUNT)", "").ToString());
    //            e.Row.Cells[AMOUNT_WITH_GST].Text =  Amount.ToString("N2");
    //            AmountWithoutGST = decimal.Parse(dt.Compute("SUM(TOTALAMOUNTWITHOUTGST)", "").ToString());
    //            //e.Row.Cells[AMOUNT].Text =  AmountWithoutGST.ToString("N2");
    //            GSTAmount = decimal.Parse(dt.Compute("SUM(GSTAMOUNT)", "").ToString());
    //            e.Row.Cells[GSTAMOUNT].Text =  GSTAmount.ToString("N2");
    //            e.Row.Cells[COGS].Text =  decimal.Parse(dt.Compute("SUM(TotalCOSTOFGOODSSOLD)", "").ToString()).ToString("N2");
    //            /*
    //            PLAmt = decimal.Parse(dt.Compute("SUM(PROFITLOSS)", "").ToString());
    //            e.Row.Cells[PL].Text =  PLAmt.ToString("N2") + "%";
    //            e.Row.Cells[PLPercent].Text = (PLAmt / Amount * 100).ToString("N2") + "%";
    //             * */
    //        }
    //    }
    }

    protected void lnkExport_Click(object sender, EventArgs e)
    {
        BindGrid();
        DataTable dt = (DataTable)gvReport.DataSource;
        //CommonWebUILib.ExportCSV(dt, this.Page.Title.Trim(' '), this.Page.Title, gvReport);
        ExportData(this.Page.Title.Trim(' '), dt);
    }

    private void ExportData(string fileName, DataTable dt)
    {
        if (dt == null) return;

        fileName = fileName.Replace(" ", "_");
        SLDocument sl = new SLDocument();
        //var style = sl.CreateStyle();
        //style.FormatCode = "###";
        int iStartRowIndex = 1;
        int iStartColumnIndex = 1;
        //sl.SetColumnStyle(5, style);
        sl.ImportDataTable(iStartRowIndex, iStartColumnIndex, dt, true);

        #region *) Update column header text
        sl.SetCellValue(1, 1, "Category");
        sl.SetCellValue(1, 2, "Item No");
        sl.SetCellValue(1, 3, "Item Name");
        sl.SetCellValue(1, 4, "Qty");
        sl.SetCellValue(1, 5, "UnitPrice");
        sl.SetCellValue(1, 6, "Total Amount (inclusive GST)");
        sl.SetCellValue(1, 7, "GST");
        sl.SetCellValue(1, 8, "Discount (%)");
        sl.SetCellValue(1, 9, "Nett Amount (w/o GST)");
        sl.SetCellValue(1, 10, "Discount Amount (w/o GST)");
        sl.SetCellValue(1, 11, "Total Amount (w/o GST)");
        sl.SetCellValue(1, 12, "Payment Type");
        sl.SetCellValue(1, 13, "Point Of Sale Name");
        sl.SetCellValue(1, 14, "Outlet Name");
        sl.SetCellValue(1, 15, "Remark");
        sl.SetCellValue(1, 16, "SAP material code");
        sl.SetCellValue(1, 17, "SAP Customer number");
        #endregion

        int iEndRowIndex = iStartRowIndex + dt.Rows.Count + 1 - 1;
        int iEndColumnIndex = iStartColumnIndex + dt.Columns.Count - 1;
        SLTable table = sl.CreateTable(iStartRowIndex, iStartColumnIndex, iEndRowIndex, iEndColumnIndex);
        table.SetTableStyle(SLTableStyleTypeValues.Medium2);
        table.HasTotalRow = false;
        sl.InsertTable(table);

        Response.Clear();
        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName + "_Result.xlsx");
        sl.SaveAs(Response.OutputStream);
        Response.End();
    }

    protected void gvHistory_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToUpper() == "USE")
        {
            int rowIndex = e.CommandArgument.ToString().GetIntValue();
            txtStartDate.Text = gvHistory.Rows[rowIndex].Cells[HIST_COL_StartDate].Text;
            txtEndDate.Text = gvHistory.Rows[rowIndex].Cells[HIST_COL_EndDate].Text;

            ListItem liPOS;
            liPOS = ddlPOS.Items.FindByText(gvHistory.Rows[rowIndex].Cells[HIST_COL_PointOfSale].Text);
            if (liPOS != null)
            {
                ddlPOS.ClearSelection();
                liPOS.Selected = true;
            }

            ListItem liTrans;
            liTrans = ddlTransactionType.Items.FindByValue(gvHistory.Rows[rowIndex].Cells[HIST_COL_TransactionType].Text);
            if (liTrans != null)
            {
                ddlTransactionType.ClearSelection();
                liTrans.Selected = true;
            }

            BindGrid();
        }
    }

    protected void lnkZXV3_Click(object sender, EventArgs e)
    {
        try
        {
            string exportDirectory = AppDomain.CurrentDomain.BaseDirectory + "Exports\\";

            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(exportDirectory);
            if (!dir.Exists) dir.Create();

            #region *) Get data to export
            litMessage.Text = "";

            if (ddlPOS.SelectedValue == "")
            {
                litMessage.Text = "Point Of Sale is empty";
                return;
            }
            if (ddlTransactionType.SelectedValue == "")
            {
                litMessage.Text = "Transaction Type is empty";
                return;
            }
            DateTime startDate = DateTime.Parse(txtStartDate.Text);
            DateTime endDate = DateTime.Parse(txtEndDate.Text);

            string orderType = "";
            if (ddlTransactionType.SelectedValue == "Normal sales")
                orderType = "ZORS";
            else if (ddlTransactionType.SelectedValue == "Refund with Product Return")
                orderType = "ZRES";
            else if (ddlTransactionType.SelectedValue == "Refund without Product Return")
                orderType = "ZCRS";
            else if (ddlTransactionType.SelectedValue == "Sample")
                orderType = "ZSML";

            string posCode = "ALL";
            if (ddlPOS.SelectedItem.Text != "ALL")
            {
                PointOfSale p = new PointOfSale(ddlPOS.SelectedValue);
                if (string.IsNullOrEmpty(p.PointOfSaleDescription))
                    posCode = "";
                else
                    posCode = p.PointOfSaleDescription.Left(2).ToUpper();
            }

            string fileName = string.Format("SG{0}{1}{2}{3}.txt",
                                            posCode,
                                            orderType,
                                            startDate.ToString("ddMM"),
                                            endDate.ToString("ddMM"));
            SaveSearchHistory(fileName);
            BindHistory();

            string sql = @"
                        SELECT CustomerCode, OrderType, PaymentType, PointOfSaleName, 
                            ItemNo, Attributes1, ABS(SUM(Quantity)) AS Quantity
                        FROM (
                            SELECT sap.CustomerCode, 
                                OrderType = CASE @TransactionType
                                                WHEN 'Normal sales' THEN 'ZORS'
                                                WHEN 'Refund with Product Return' THEN 'ZRES'
                                                WHEN 'Refund without Product Return' THEN 'ZCRS'
                                                WHEN 'Sample' THEN 'ZSML'
                                            END,
                                ISNULL(pay.PaymentType, 'CASH') AS PaymentType,
                                pos.PointOfSaleName, 
                                it.ItemNo, od.Quantity, it.Attributes1 
                            FROM OrderHdr oh
                                INNER JOIN OrderDet od ON od.OrderHdrID = oh.OrderHdrID 
                                INNER JOIN Item it ON it.ItemNo = od.ItemNo 
                                INNER JOIN PointOfSale pos ON pos.PointOfSaleID = oh.PointOfSaleID 
                                LEFT JOIN (
                                        SELECT DISTINCT oh.OrderHdrID,  
                                            CASE ISNULL(MAX(rd.PaymentType), 'CASH')  
                                                WHEN 'CASH' THEN 'CASH' 
		                                        WHEN 'VOUCHER' THEN 'CASH' 
                                                WHEN 'PayPal' THEN 'E-PAYMENT' 
                                                ELSE 'CARD' 
                                            END AS PaymentType 
                                        FROM OrderHdr oh 
                                            LEFT JOIN ReceiptDet rd ON rd.ReceiptHdrID = oh.OrderHdrID 
	                                        GROUP BY OH.OrderhdrID 
	                                        HAVING COUNT(*) = 1 
                                        UNION ALL 
                                        SELECT DISTINCT oh.OrderHdrID, 
                                            CASE ISNULL(MIN(rd.PaymentType), 'CASH') 
                                                WHEN 'CASH' THEN 'CASH' 
		                                        WHEN 'VOUCHER' THEN 'CASH' 
                                                WHEN 'PayPal' THEN 'E-PAYMENT' 
                                                ELSE 'CARD'
                                            END AS PaymentType
                                        FROM OrderHdr oh
                                            LEFT JOIN ReceiptDet rd ON rd.ReceiptHdrID = oh.OrderHdrID
                                        	GROUP BY OH.OrderhdrID
                                        HAVING COUNT(*) > 1
                                    ) pay ON pay.OrderHdrID = oh.OrderHdrID 
                                LEFT JOIN SAPCustomerCode sap ON sap.SalesType = @TransactionType AND sap.PaymentType = pay.PaymentType AND sap.PointOfSaleID = pos.PointOfSaleID
                            WHERE oh.OrderDate BETWEEN @StartDate AND @EndDate
                                AND oh.IsVoided = 0 AND od.IsVoided = 0 
                                AND (@PointOfSaleID = 0 OR oh.PointOfSaleID = @PointOfSaleID)
                                AND 1 = CASE @TransactionType
                                            WHEN 'Normal sales' THEN CASE WHEN (od.Quantity > 0 AND od.Amount > 0) THEN 1 ELSE 0 END
                                            WHEN 'Refund with Product Return' THEN CASE WHEN (od.Quantity < 0 AND od.Amount < 0 AND ISNULL(od.InventoryHdrRefNo,'') <> 'NOTRETURNED') THEN 1 ELSE 0 END
                                            WHEN 'Refund without Product Return' THEN CASE WHEN (od.Quantity < 0 AND od.Amount < 0 AND ISNULL(od.InventoryHdrRefNo,'') = 'NOTRETURNED') THEN 1 ELSE 0 END
                                            WHEN 'Sample' THEN CASE WHEN (od.Quantity > 0 AND od.Amount = 0) THEN 1 ELSE 0 END
                                        END
                                AND it.CategoryName <> 'SYSTEM' AND ISNULL(it.Attributes1, '') <> ''
                        ) Res
                        GROUP BY CustomerCode, OrderType, PaymentType, PointOfSaleName, ItemNo, Attributes1
                        ORDER BY CustomerCode, OrderType, PaymentType, PointOfSaleName, ItemNo, Attributes1
                     ";
            sql = string.Format(sql, ViewState["sortBy"].ToString(), ViewState[SORT_DIRECTION].ToString());
            QueryCommand cmd = new QueryCommand(sql, "PowerPOS");
            cmd.Parameters.Add("@StartDate", startDate, DbType.DateTime);
            cmd.Parameters.Add("@EndDate", endDate.AddSeconds(86399), DbType.DateTime);
            cmd.Parameters.Add("@PointOfSaleID", ddlPOS.SelectedValue, DbType.Int32);
            cmd.Parameters.Add("@TransactionType", ddlTransactionType.SelectedValue, DbType.String);
            DataTable dt = DataService.GetDataSet(cmd).Tables[0];
            #endregion

            DataTable hdr = dt.DefaultView.ToTable(true, "CustomerCode", "OrderType", "PaymentType", "PointOfSaleName");
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(exportDirectory + fileName, false))
            {
                int i = 0;
                foreach (DataRow drHdr in hdr.Rows)
                {
                    i++;
                    decimal qty;

                    #region *) Header
                    string str = "HDR";   //1-3 File ID
                    str += i.ToString().PadLeft(6, '0');   //4-9 Order No
                    str += DateTime.Now.ToString("yyyyMMdd");   // 10-17 Order Date
                    str += ("SG-" + DateTime.Now.ToString("yyMMMdd") + "-" + i.ToString().PadLeft(3, '0')).PadRight(15);   // 18-32 PO NO
                    str += drHdr["CustomerCode"].ToString().Left(13).PadRight(13);   // 33-45 Customer Code
                    str += drHdr["CustomerCode"].ToString().Left(13).PadRight(13);   // 46-58 Ship to Code
                    str += DateTime.Now.ToString("yyyyMMdd");   // 59-66 Delivery Date
                    str += "".PadRight(13);   // 67-79 Order Amount
                    str += "".PadRight(13);   // 80-92 Net Amount
                    str += "".PadRight(11);   // 93-103 VAT Amount
                    str += "".PadRight(13);   // 104-116 Total Amount
                    str += "4481".PadRight(13);   // 117-129 Supplier Code
                    str += "".PadRight(10);   // 130-139 PRICAT No
                    str += "2210".PadRight(4);   // 140-143 Sales Org
                    str += drHdr["OrderType"].ToString().Left(4).PadRight(4);   // 144-147 Order Type
                    str += (drHdr["OrderType"].ToString() == "ZRES" ? "501" : "".PadRight(3));   // 148-150 Order Reason
                    str += "02".PadRight(2);   // 151-152 Delivery Block

                    // Write the Header
                    file.WriteLine(str);
                    #endregion

                    #region *) Detail
                    int j = 0;
                    string filter = string.Format("OrderType='{0}' AND PaymentType='{1}' AND PointOfSaleName='{2}'",
                                                    drHdr["OrderType"].ToString(),
                                                    drHdr["PaymentType"].ToString(),
                                                    drHdr["PointOfSaleName"].ToString());
                    foreach (DataRow drDet in dt.Select(filter))
                    {
                        j++;
                        str = "DTL";   //1-3 File ID
                        str += i.ToString().PadLeft(6, '0');   //4-9 Order No
                        str += j.ToString().PadLeft(3, '0');   //10-12 Line No
                        str += drDet["Attributes1"].ToString().Left(15).PadRight(15);   // 13-27 Item Code
                        if (decimal.TryParse(drDet["Quantity"].ToString(), out qty))
                            str += qty.ToString("0.000").Replace(".", "").Left(9).PadLeft(9, '0');   // 28-36 Order Qty
                        else
                            str += "".PadRight(9);   // 28-36 Order Qty
                        str += "".PadRight(11);   // 37-47 List Price
                        str += "".PadRight(11);   // 48-58 Override Price
                        str += "".PadRight(2);   // 59-60 Discount Rate
                        str += "".PadRight(15);   // 61-75 Line Item Amount
                        str += "PC".PadRight(3);   // 76-78 Order Unit
                        str += "".PadRight(1);   // 79 Material Description-Language
                        str += "";   // 80-... Material Description-Text

                        // Write the Details
                        file.WriteLine(str);
                    }
                    #endregion

                    #region *) TLR
                    str = "TLR";   //1-3 File ID
                    str += j.ToString().PadLeft(5, '0');   //4-8 Total number of lines
                    // Write the Trailer
                    file.WriteLine(str);
                    #endregion
                }
            }

            //Send via FTP
            string result;
            string uploadDir = AppSetting.CastString(AppSetting.GetSetting(AppSetting.SettingsName.Reports.ZXV3UploadDirectory));
            if (!PowerWeb.BLL.Helper.FTP.StartTransfer("upload", exportDirectory, uploadDir, out result))
                litMessage.Text = result;
            else
            {
                litMessage.Text = "ZXV3 file generated successfully.";
                litMessage.ForeColor = System.Drawing.Color.Green;
            }
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
            litMessage.Text = "Error occured: " + ex.Message;
        }
    }
    
}
