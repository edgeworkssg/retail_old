using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PowerPOS;
using System.Data;
using SubSonic;

namespace PowerWeb.Reports
{
    public partial class AverageBillReport : PageBase
    {
        private const string SORT_DIRECTION = "SORT_DIRECTION";
        private const string ORDER_BY = "ORDER_BY";

        private void Load_ddlYear()
        {
            int minyear = UtilityController.GetMinOrderDateYear();
            for (int i = DateTime.Today.Year; i >= minyear; i--)
            {
                ddlYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            OutletDropdownList.setIsUseAllBreakdownOutlet(false);
            OutletDropdownList.setIsUseAllBreakdownPOS(false);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SetFormSetting();
            if (!Page.IsPostBack)
            {

                ViewState["Sort"] = "";
                ViewState[SORT_DIRECTION] = "ASC";
                Load_ddlYear();
                ddlMonth.SelectedValue = DateTime.Today.Month.ToString();
                txtStartDate.Text = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).ToString("dd MMM yyyy");
                txtEndDate.Text = DateTime.Today.AddDays(-1).ToString("dd MMM yyyy");
                BindGrid();
            }
        }

        private void SetFormSetting()
        {
            try
            {
                string outletText = LanguageManager.GetTranslation(LabelController.OutletText);
                string posText = LanguageManager.GetTranslation(LabelController.PointOfSaleText);
                OutletDropdownList.SetLabelOutlet(outletText);
                OutletDropdownList.SetLabelPOS(posText);
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

                string sort = ViewState["Sort"].ToString();
                DateTime startDate = (txtStartDate.Text).GetDateTimeValue("dd MMM yyyy");
                DateTime endDate = (txtEndDate.Text).GetDateTimeValue("dd MMM yyyy");
                int pos = (OutletDropdownList.GetDdlPOSSelectedValue + "").GetIntValue();
                string outlet = OutletDropdownList.GetDdlOutletSelectedItemText + "";
                DataTable dt = FetchData(outlet, pos, startDate, endDate);
                
                DataView dv = new DataView();
                dv.Table = dt;
                if (!string.IsNullOrEmpty(sort))
                    dv.Sort = sort + " " + ViewState[SORT_DIRECTION];

                gvReport.DataSource = dv;
                gvReport.DataBind();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        private DataTable FetchData(string outlet, int pos, DateTime startDate, DateTime endDate)
        {
            DataTable dt = new DataTable();

            try
            {
                Logger.writeLog(">>>> Start Executing FetchDailySalesByOutlet " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                string sql = @"DECLARE @StartDate DATETIME;
                            DECLARE @EndDate DATETIME;
                            DECLARE @OutletName NVARCHAR(200);
                            DECLARE @PointOfSaleID INT;

                            SET @StartDate = '{0}';
                            SET @EndDate = '{1}';

                            SET @OutletName = N'{2}';
                            SET @PointOfSaleID = {3};

                            CREATE TABLE #TabDailySales
                            (
                                 [Date] DATE
                                ,[Category] NVARCHAR(50)
                                ,[PointOfSaleName] NVARCHAR(500)
                                ,[CountAvg] INTEGER
                            )

                            CREATE TABLE #ReceiptCategory
                            (
	                            [Category] NVARCHAR(50)
                            )

                            INSERT INTO #ReceiptCategory(Category) values ('$0 - $19')
                            INSERT INTO #ReceiptCategory(Category) values ('$20 - $39' )
                            INSERT INTO #ReceiptCategory(Category) values ('$40 - $59')
                            INSERT INTO #ReceiptCategory(Category) values ('$60 - $79')
                            INSERT INTO #ReceiptCategory(Category) values ('$80 - $99')
                            INSERT INTO #ReceiptCategory(Category) values ('> $100' )

                            INSERT INTO #TabDailySales
                            SELECT TC.[Date],
	                               TC.Category,
	                               TC.PointOfSaleName,
	                               ISNULL(TS.CountAvg,0) as CountAvg
                            FROM
                            (
	                            SELECT * 
	                            FROM #ReceiptCategory
	                            CROSS JOIN FnGetEveryDateBetween(@StartDate, @EndDate) dt 
	                            CROSS JOIN PointOfSale pos 
	                            WHERE	ISNULL(POS.Deleted,0) = 0 
			                            AND (POS.PointOfSaleID = @PointOfSaleID OR @PointOfSaleID = 0)
			                            AND (POS.OutletName = @OutletName OR @OutletName = 'ALL')
                            )TC LEFT JOIN 
                            (
	                            SELECT 
		                            T.OrderDate,
		                            T.PointOfSaleID,
		                            CASE  
			                            WHEN 0 <= T.NettAmount  AND t.NettAmount <= 19 THEN '$0 - $19' 
			                            WHEN 20 <= T.NettAmount  AND t.NettAmount <= 39 THEN '$20 - $39' 
			                            WHEN 40 <= T.NettAmount  AND t.NettAmount <= 59 THEN '$40 - $59' 
			                            WHEN 60 <= T.NettAmount  AND t.NettAmount <= 79 THEN '$60 - $79' 
			                            WHEN 80 <= T.NettAmount  AND t.NettAmount <= 99 THEN '$80 - $99' 
			                            WHEN T.NettAmount >= 100 THEN '> $100' 
			                            END AS AverageReceipt,
		                            Count(T.OrderHdrID) as CountAvg
	                            FROM
	                            (
		                            select CAST(OH.OrderDate AS DATE) OrderDate
						                            ,OH.OrderHdrID
						                            ,POS.PointOfSaleID
						                            ,POS.PointOfSaleName
						                            ,SUM(OD.Amount) AS NettAmount  
		                            from OrderHdr OH
		                            LEFT JOIN PointOfSale POS ON POS.PointOfSaleID = OH.PointOfSaleID
		                            LEFT JOIN OrderDet OD ON OD.OrderHdrID = OH.OrderHdrID
		                            WHERE	OH.IsVoided = 0
			                            AND CAST(OH.OrderDate AS DATE) >= CAST(@StartDate AS DATE)
			                            AND CAST(OH.OrderDate AS DATE) <= CAST(@EndDate AS DATE)
			                            AND (POS.PointOfSaleID = @PointOfSaleID OR @PointOfSaleID = 0)
			                            AND (POS.OutletName = @OutletName OR @OutletName = 'ALL')
		                            GROUP BY CAST(OH.OrderDate AS DATE)
			                            ,OH.OrderHdrID
			                            ,POS.PointOfSaleID
			                            ,POS.PointOfSaleName
	                            ) T
	                            GROUP BY T.[OrderDate], T.[PointOfSaleID],
		                            CASE  
			                            WHEN 0 <= T.NettAmount  AND t.NettAmount <= 19 THEN '$0 - $19' 
			                            WHEN 20 <= T.NettAmount  AND t.NettAmount <= 39 THEN '$20 - $39' 
			                            WHEN 40 <= T.NettAmount  AND t.NettAmount <= 59 THEN '$40 - $59' 
			                            WHEN 60 <= T.NettAmount  AND t.NettAmount <= 79 THEN '$60 - $79' 
			                            WHEN 80 <= T.NettAmount  AND t.NettAmount <= 99 THEN '$80 - $99' 
			                            WHEN T.NettAmount >= 100 THEN '> $100' 
			                            END
                            )TS ON TC.Category = TS.AverageReceipt and TC.PointOfSaleID = TS.PointOfSaleID and TC.[Date] = TS.OrderDate
                            ORDER BY TC.[Date], TC.Category

                            --SELECT * FROM #TabDailySales

                            DECLARE @cols AS NVARCHAR(MAX);
                            DECLARE @query AS NVARCHAR(MAX);

                            SET @cols = STUFF((SELECT DISTINCT ',' + QUOTENAME(FR.PointOfSaleName) 
                                        FROM PointOfSale FR 
                                        WHERE ISNULL(FR.Deleted,0) = 0
					                            AND (FR.PointOfSaleID = @PointOfSaleID OR @PointOfSaleID = 0)
					                            AND (FR.OutletName = @OutletName OR @OutletName = 'ALL')	            
                                        FOR XML PATH(''), TYPE
                                        ).value('.', 'NVARCHAR(MAX)') 
                                    ,1,1,'')

                            SET @query ='SELECT [Date],[Category] as [Average Bill],' + @cols + ' FROM 
                                         (
                                            SELECT	 [Date]
                                                    ,[Category]
                                                    ,[PointOfSaleName]
						                            ,[CountAvg]
                                            FROM	#TabDailySales
                                         ) X
                                         PIVOT 
                                         (
                                             SUM(CountAvg)
                                             FOR PointOfSaleName IN (' + @cols + ')
                                         ) P ORDER BY [Date], [Category]'

                            EXECUTE(@query)

                            DROP TABLE #TabDailySales;
                            DROP TABLE #ReceiptCategory;
                            ";
                sql = string.Format(sql, startDate.ToString("yyyy-MM-dd")
                                       , endDate.ToString("yyyy-MM-dd")
                                       , outlet
                                       , pos);
                QueryCommand qc = new QueryCommand(sql);
                qc.CommandTimeout = 60000;
                dt.Load(DataService.GetReader(qc));
                dt.TableName = "AverageBillReport";
                Logger.writeLog(">>>> Finish Executing AverageBillReport " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
               
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }

            return dt;
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
                DataView ds = (DataView)gvReport.DataSource;
                if (ds != null)
                {
                    itemCount = ds.Table.Rows.Count;
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

        protected void lnkExport_Click(object sender, EventArgs e)
        {
            BindGrid();
            DataView dv = (DataView)gvReport.DataSource;
            DataTable dt = dv.Table;

            //CommonWebUILib.ExportCSV(dt, this.Page.Title.Trim(' '));
            // Export the details of specified columns to Excel
            int[] column;
            column = new int[dt.Columns.Count];
            for (int i = 0; i < column.Length; i++)
            {
                column[i] = i;
            }
            string[] header;
            header = new string[dt.Columns.Count];
            //Work around for bug in the export to excel library
            for (int i = 0; i < header.Length; i++)
            {
                header[i] = dt.Columns[i].ColumnName;
                dt.Columns[i].ColumnName = "col" + i.ToString();
            }

            RKLib.ExportData.Export objExport = new
                RKLib.ExportData.Export("Web");

            objExport.ExportDetails(dt, column, header,
                 RKLib.ExportData.Export.ExportFormat.CSV,
                 this.Page.Title.Trim(' ') + DateTime.Now.ToString("ddMMMyyyy")
                 + ".CSV");
        }

        protected void gvReport_Sorting(object sender, GridViewSortEventArgs e)
        {
            ViewState["Sort"] = e.SortExpression;
            if (ViewState[SORT_DIRECTION] == null || ((string)ViewState[SORT_DIRECTION]) == SqlFragment.ASC)
            {
                ViewState[SORT_DIRECTION] = SqlFragment.DESC;
            }

            else
            {
                ViewState[SORT_DIRECTION] = SqlFragment.ASC;
            }

            //rebind the grid
            BindGrid();
        }

        protected void ddlPages_SelectedIndexChanged(Object sender, EventArgs e)
        {
            GridViewRow gvrPager = gvReport.BottomPagerRow;
            DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages");
            gvReport.PageIndex = ddlPages.SelectedIndex;
            BindGrid();
        }

        protected void gvReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvReport.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (rdbMonth.Checked)
            {
                int selectedMonth = int.Parse(ddlMonth.SelectedValue);
                DateTime startDate = new DateTime((ddlYear.SelectedValue + "").GetIntValue(), selectedMonth, 1);
                txtStartDate.Text =
                    startDate.ToString("dd MMM yyyy");

                txtEndDate.Text = startDate.AddMonths(1).ToString("dd MMM yyyy");
            }
            BindGrid();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            OutletDropdownList.ResetDdlOutlet();
            OutletDropdownList.ResetDdlPOS();
            ddlMonth.SelectedIndex = 0;
            ddlYear.SelectedIndex = 0;
            gvReport.PageIndex = 0;
            BindGrid();
        }
        
        protected void gvReport_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Format date
                DataRow row = ((DataRowView)e.Row.DataItem).Row;
                try
                {
                    e.Row.Cells[0].Text = ((DateTime)row["Date"]).ToString("dd MMM yyyy");
                }
                catch (Exception ex)
                {

                }
            }
        }
    }
}
