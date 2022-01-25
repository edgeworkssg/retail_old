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
using PowerPOS;
using PowerPOS.Container;
using SubSonic;
using SubSonic.Utilities;

namespace PowerWeb.Reports
{
    public partial class OutletDailySales : PageBase
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
                BindGrid();
            }
        }

        private void SetFormSetting()
        {
            try
            {
                string outletText = LanguageManager.GetTranslation(LabelController.OutletText);
                string posText = LanguageManager.GetTranslation(LabelController.PointOfSaleText);
                Page.Title = outletText +" "+ LanguageManager.GetTranslation("Daily Sales");
                //lblPOS.Text = posText + " Name";
                //lblOutlet.Text = outletText + " Name";
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
                //if (ddlOutlet.SelectedValue == null || ddlPOS.SelectedValue == null)
                //    return;

                DataService.ExecuteQuery(new QueryCommand("SET ANSI_NULLS ON"));

                string sort = ViewState["Sort"].ToString();
                DateTime startDate = DateTime.Parse(ddlYear.SelectedValue + "-" + ddlMonth.SelectedValue + "-" + "1");
                DateTime endDate = startDate.AddMonths(1).AddSeconds(-1);
                int pos = (OutletDropdownList.GetDdlPOSSelectedValue + "").GetIntValue();
                string outlet = OutletDropdownList.GetDdlOutletSelectedItemText + "";
                DataTable dt = FetchData(outlet, pos, startDate, endDate, chkShowBeforeGST.Checked);
                dt.TableName = "FetchDailySalesByOutlet";
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

        private DataTable FetchData(string outlet, int pos, DateTime startDate, DateTime endDate, bool isShowBeforeGST)
        {
            DataTable dt = new DataTable();

            try
            {
                Logger.writeLog(">>>> Start Executing FetchDailySalesByOutlet " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                string sql = @"DECLARE @StartDate DATETIME;
DECLARE @EndDate DATETIME;
DECLARE @ShowAmountBeforeGSt BIT;
DECLARE @OutletName NVARCHAR(200);
DECLARE @PointOfSaleID INT;

CREATE TABLE #TabDailySales
(
     [Date] DATE
    ,[WeekDay] NVARCHAR(20)
    ,[PointOfSaleName] NVARCHAR(500)
    ,[NettAmount] MONEY
)

SET @StartDate = '{0}';
SET @EndDate = '{1}';
SET @ShowAmountBeforeGSt = {2};

SET @OutletName = N'{3}';
SET @PointOfSaleID = {4};

INSERT INTO #TabDailySales
SELECT	 TheDate.[Date]
        ,LEFT(TheDate.[WeekDay], 3) AS [WeekDay]
        ,TheDate.PointOfSaleName
        ,TheSales.NettAmount AS NettAmount 
FROM	( 
            SELECT * 
            FROM	FnGetEveryDateBetween(@StartDate, @EndDate) dt 
                    CROSS JOIN PointOfSale pos 
            WHERE	ISNULL(POS.Deleted,0) = 0 
					AND (POS.PointOfSaleID = @PointOfSaleID OR @PointOfSaleID = 0)
					AND (POS.OutletName = @OutletName OR @OutletName = 'ALL')					
        ) TheDate LEFT JOIN ( 
            SELECT   CAST(OH.OrderDate AS DATE) OrderDate
                    ,POS.PointOfSaleID
                    ,POS.PointOfSaleName
                    ,SUM(CASE WHEN @ShowAmountBeforeGSt = 1 THEN (OD.Amount-OD.GSTAmount) ELSE OD.Amount END) AS NettAmount 							
            FROM	OrderHdr OH
                    LEFT JOIN PointOfSale POS ON POS.PointOfSaleID = OH.PointOfSaleID
                    LEFT JOIN OrderDet OD ON OD.OrderHdrID = OH.OrderHdrID
                    LEFT JOIN Item I ON I.ItemNo = OD.ItemNo
            WHERE	OH.IsVoided = 0
                    AND CAST(OH.OrderDate AS DATE) >= CAST(@StartDate AS DATE)
                    AND CAST(OH.OrderDate AS DATE) <= CAST(@EndDate AS DATE)
					AND (POS.PointOfSaleID = @PointOfSaleID OR @PointOfSaleID = 0)
					AND (POS.OutletName = @OutletName OR @OutletName = 'ALL')	                    
            GROUP BY CAST(OH.OrderDate AS DATE)
                    ,POS.PointOfSaleID
                    ,POS.PointOfSaleName
        ) TheSales ON TheDate.[Date] = TheSales.OrderDate AND TheDate.PointOfSaleID = TheSales.PointOfSaleID 
ORDER BY TheDate.[Date], TheDate.PointOfSaleName

INSERT INTO #TabDailySales
SELECT  Date, WeekDay, 'Total' PointOfSaleName, SUM(ISNULL(NettAmount,0)) NettAmount
FROM	#TabDailySales
GROUP BY Date, WeekDay

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

SET @query ='SELECT [Date],[WeekDay],' + @cols + ',[Total] FROM 
             (
                SELECT	 [Date]
                        ,[WeekDay]
                        ,[PointOfSaleName]
                        ,[NettAmount]
                FROM	#TabDailySales
             ) X
             PIVOT 
             (
                 SUM(NettAmount)
                 FOR PointOfSaleName IN (' + @cols + ',[Total])
             ) P ORDER BY [Date]'

EXECUTE(@query)

DROP TABLE #TabDailySales;";
                sql = string.Format(sql, startDate.ToString("yyyy-MM-dd")
                                       , endDate.ToString("yyyy-MM-dd")
                                       , isShowBeforeGST ? 1 : 0
                                       , outlet
                                       , pos);
                QueryCommand qc = new QueryCommand(sql);
                qc.CommandTimeout = 60000;
                dt.Load(DataService.GetReader(qc));
                dt.TableName = "FetchDailySalesByOutlet";
                Logger.writeLog(">>>> Finish Executing FetchDailySalesByOutlet " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        dt.Columns[i].ReadOnly = false;
                        if (dt.Columns[i].DataType == typeof(string))
                            dt.Columns[i].MaxLength = int.MaxValue;
                    }
                    // Prepare 2 new rows for Total and Average
                    DataRow dRowTotal = dt.NewRow();
                    DataRow dRowAverage = dt.NewRow();
                    foreach (DataRow row in dt.Rows)
                    {
                        for (int colIndex = 2; colIndex < dt.Columns.Count - 1; colIndex++)
                        {
                            if (!string.IsNullOrEmpty(row[colIndex] + ""))
                                row[colIndex] = Math.Round((row[colIndex] + "").GetDecimalValue(), 2); 
                        }
                    }

                    // Calculate value for TOTAL and AVERAGE row
                    dRowTotal["WeekDay"] = "TOTAL";
                    dRowAverage["WeekDay"] = "AVERAGE";
                    for (int colIndex = 2; colIndex < dt.Columns.Count; colIndex++)
                    {
                        decimal sum = 0;
                        for (int rowIndex = 0; rowIndex < dt.Rows.Count; rowIndex++)
                        {
                            if (dt.Rows[rowIndex][colIndex].GetType() == typeof(decimal))
                                sum += ((decimal)dt.Rows[rowIndex][colIndex]);
                        }
                        string colName = dt.Columns[colIndex].ColumnName;
                        dRowTotal[colName] = sum;
                        dRowAverage[colName] = sum / Convert.ToDecimal(dt.Rows.Count);
                    }

                    dt.Rows.Add(dRowTotal);
                    dt.Rows.Add(dRowAverage);
                    
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dt.Rows[i][1] = LanguageManager.GetTranslation(dt.Rows[i][1] + "");
                    }

                    if (dt.Columns.Count > 0)
                    {
                        dt.Columns[0].ColumnName = LanguageManager.GetTranslation(dt.Columns[0].ColumnName);
                        dt.Columns[1].ColumnName = LanguageManager.GetTranslation(dt.Columns[1].ColumnName);
                        dt.Columns[dt.Columns.Count - 1].ColumnName = LanguageManager.GetTranslation(dt.Columns[dt.Columns.Count - 1].ColumnName);
                    }
                }
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
        bool isUsingMallIntegration = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.MallIntegration.UseMallManagement), false);
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
                // Add formatting
                for (int colIndex = 2; colIndex < row.ItemArray.Length; colIndex++)
                {
                    if (!string.IsNullOrEmpty(row[colIndex] + ""))
                        e.Row.Cells[colIndex].Text = (row[colIndex] + "").GetDecimalValue().ToString("N2");
                    else
                    {
                        if (isUsingMallIntegration)
                            e.Row.Cells[colIndex].Text = "-";
                        else
                            e.Row.Cells[colIndex].Text = "0.00";
                    }
                }
            }
        }
        #region Obsolete
        //protected void ddlOutlet_Init(object sender, EventArgs e)
        //{
        //    //try
        //    //{
        //    //    ddlOutlet.DataSource = OutletController.FetchByUserNameForReport(false, true, (Session["UserName"] + ""));
        //    //    ddlOutlet.DataBind();
        //    //    ddlOutlet.SelectedIndex = 0;
        //    //    ddlOutlet_SelectedIndexChanged(ddlOutlet, new EventArgs());
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    Logger.writeLog(ex);
        //    //}
        //}

        //protected void ddlOutlet_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    //try
        //    //{
        //    //    ddlPOS.DataSource = PointOfSaleController.FetchByUserNameForReport(false, true, (Session["UserName"] + ""), ddlOutlet.SelectedValue);
        //    //    ddlPOS.DataBind();
        //    //    BindGrid();
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    Logger.writeLog(ex);
        //    //}
        //}
        #endregion
    }
}
