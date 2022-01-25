using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Linq;
using PowerPOS;
using PowerPOS.Container;
using SubSonic;
using SubSonic.Utilities;

namespace PowerWeb.Reports
{
    public partial class OutletDailyCategorySales : PageBase
    {
        private const string SORT_DIRECTION = "SORT_DIRECTION";
        private const string ORDER_BY = "ORDER_BY";

        private List<String> categories = new List<string>();
        private List<decimal> totals = new List<decimal>();

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

                CategoryCollection cc = new CategoryCollection();
                ArrayList list = new ArrayList();
                cc.Load();
                for (int i = 0; i < cc.Count; i++)
                {
                    list.Add(cc[i].CategoryName);
                }
                mccCategory.ClearAll();
                mccCategory.AddItems(list);
                list.Clear();
            }
        }

        private void SetFormSetting()
        {
            try
            {
                string outletText = LanguageManager.GetTranslation(LabelController.OutletText);
                string posText = LanguageManager.GetTranslation(LabelController.PointOfSaleText);
                Page.Title = outletText + " "+LanguageManager.GetTranslation("Category Daily Sales");
                OutletDropdownList.SetLabelOutlet(outletText);
                OutletDropdownList.SetLabelPOS(posText);
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void lnkExport_Click(object sender, EventArgs e)
        {
            BindGrid();
            DataView dv = (DataView)gvReport.DataSource;
            DataTable dt = dv.Table;

            {
                int rc = dt.Rows.Count;

                {
                    var r = dt.NewRow();
                    r[1] = "Total";
                    for (int i = 2; i < dt.Columns.Count; i++)
                        r[i] = totals[i - 2];

                    dt.Rows.Add(r);
                }

                {
                    var r = dt.NewRow();
                    r[1] = "Average";
                    for (int i = 2; i < dt.Columns.Count; i++)
                        r[i] = totals[i - 2] / rc;

                    dt.Rows.Add(r);
                }
            }

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
                header[i] = dt.Columns[i].ColumnName.Replace("'", "`");
                dt.Columns[i].ColumnName = "col" + i.ToString();
            }

            RKLib.ExportData.Export objExport = new
                RKLib.ExportData.Export("Web");

            objExport.ExportDetails(dt, column, header,
                 RKLib.ExportData.Export.ExportFormat.CSV,
                 this.Page.Title.Replace(" ", "") + DateTime.Now.ToString("ddMMMyyyy")
                 + ".CSV");
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            //mccCategory.unselectAllItems();
            //mccCategory.Text = "";
            OutletDropdownList.ResetDdlOutlet();
            OutletDropdownList.ResetDdlPOS();
            ddlMonth.SelectedIndex = 0;
            ddlYear.SelectedIndex = 0;
            gvReport.PageIndex = 0;
            BindGrid();
        }

        private void BindGrid()
        {
            DataService.ExecuteQuery(new QueryCommand("SET ANSI_NULLS ON"));

            string sort = ViewState["Sort"].ToString();
            DateTime startDate = DateTime.Parse(ddlYear.SelectedValue + "-" + ddlMonth.SelectedValue + "-" + "1");
            DateTime endDate = startDate.AddMonths(1).AddSeconds(-1);

            int pointOfSaleID = (OutletDropdownList.GetDdlPOSSelectedValue + "").GetIntValue();
            string outlet = OutletDropdownList.GetDdlOutletSelectedItemText + "";

            string column = "od.Amount";
            string select = @"[C_0RD3R_D4T3,.,] as [OrderDate], [W33KD4Y,.,] as [Weekday], ";
            string wh = "";

            if (pointOfSaleID != 0)
                wh = " AND oh.PointOfSaleID = " + pointOfSaleID;

            if (outlet != "ALL")
                wh += " AND ot.OutletName = '" + outlet + "'";

            if (chkShowBeforeGST.Checked)
                column = "od.Amount - od.GSTAmount";


            categories.Clear();


            string[] cc = null;

            if (mccCategory.Text != "")
                cc = mccCategory.Text.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            else
            {
                QueryCommand cmd = new QueryCommand(@"
                    SELECT CategoryName FROM Category Where Deleted = 0
                ");

                var ds = DataService.GetDataSet(cmd);
                cc = new string[ds.Tables[0].Rows.Count];
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    cc[i] = string.Format("{0}", ds.Tables[0].Rows[i][0].ToString());
                }
            }

            if (cc.Length <= 0)
            {
                gvReport.DataSource = null;
                gvReport.EmptyDataText = "No Record";
                gvReport.DataBind();
                return;
            }

            {
                for (int i = 0; i < cc.Length; i++)
                {
                    categories.Add(string.Format("[{0}]", cc[i].Trim().Replace("]", @"]]")));
                    totals.Add(0);
                }
            }

            // generate select from categories
            {
                for (int i = 0; i < cc.Length; i++)
                {
                    select += " isnull(" + categories[i] + ", 0) as " + categories[i];
                    if (i < categories.Count - 1)
                        select += ", ";
                }
            }

            {
                string str = string.Join(", ", categories.ToArray());

                QueryCommand cmd = new QueryCommand(@"
                    SELECT 
	                    " + select + @"
                    FROM (
                        SELECT CONVERT(VARCHAR, dt.[Date], 106) as [C_0RD3R_D4T3,.,], dt.[WeekDay] as [W33KD4Y,.,], '' as [C4T3G0RY,.,], 0 as SumAmt
                        FROM FnGetEveryDateBetween(@StartDate, @EndDate) dt
                        UNION
	                    SELECT 
		                    CONVERT(VARCHAR, oh.OrderDate, 106) as [C_0RD3R_D4T3,.,],
                            DATENAME(dw,oh.OrderDate) as [W33KD4Y,.,], 
                            it.CategoryName as [C4T3G0RY,.,],
		                    SUM(" + column + @") as SumAmt 
	                    FROM OrderHdr oh
	                    JOIN OrderDet od
		                    ON od.OrderHdrID = oh.OrderHdrID
	                    JOIN Item it
		                    ON it.ItemNo = od.ItemNo
                        JOIN PointOfSale ot
                            ON ot.PointOfSaleID = oh.PointOfSaleID
	                    WHERE
		                    CAST(oh.OrderDate AS DATE) >= CAST(@StartDate AS DATE)
                            AND CAST(oh.OrderDate AS DATE) <= CAST(@EndDate AS DATE)
                            AND oh.IsVoided = 0 " + wh + @"
	                    GROUP BY CONVERT(VARCHAR, oh.OrderDate, 106), DATENAME(dw,oh.OrderDate), it.CategoryName
                    ) d
                    PIVOT
                    (
	                    SUM(SumAmt)
	                    FOR [C4T3G0RY,.,] in (" + str + @")
                    ) as p
                    ORDER BY [C_0RD3R_D4T3,.,] ASC
                ");

                cmd.AddParameter("@StartDate", startDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                cmd.AddParameter("@EndDate", endDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));


                var ds = DataService.GetDataSet(cmd);

                DataView dv = new DataView();
                dv.Table = ds.Tables[0];

                if (dv.Table.Columns.Count > 0)
                {
                    dv.Table.Columns[0].ColumnName = LanguageManager.GetTranslation(dv.Table.Columns[0].ColumnName);
                    dv.Table.Columns[1].ColumnName = LanguageManager.GetTranslation(dv.Table.Columns[1].ColumnName);
                }

                for (int i = 0; i < dv.Table.Rows.Count; i++)
                {
                    dv.Table.Rows[i][1] = LanguageManager.GetTranslation(dv.Table.Rows[i][1]+"");
                }

                if (!string.IsNullOrEmpty(sort))
                    dv.Sort = sort + " " + ViewState[SORT_DIRECTION];

                gvReport.DataSource = dv;
                gvReport.EmptyDataText = LanguageManager.GetTranslation("No Record");
                gvReport.DataBind();
            }
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

        protected void gvReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvReport.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void gvReport_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRow dr = ((System.Data.DataRowView)e.Row.DataItem).Row;

                for (int i = 0; i < categories.Count; i++)
                {
                    decimal val = 0;
                    if (decimal.TryParse(dr[i + 2].ToString(), out val))
                        totals[i] += val;
                }

                for (int colIndex = 2; colIndex < dr.ItemArray.Length; colIndex++)
                {
                    if (!string.IsNullOrEmpty(dr[colIndex] + ""))
                        e.Row.Cells[colIndex].Text = (dr[colIndex] + "").GetDecimalValue().ToString("N2");
                    else
                    {
                        if (isUsingMallIntegration)
                            e.Row.Cells[colIndex].Text = "-";
                        else
                            e.Row.Cells[colIndex].Text = "0.00";
                    }
                }
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[1].Text = LanguageManager.GetTranslation("Total");
                for (int i = 0; i < categories.Count; i++)
                    e.Row.Cells[i + 2].Text = (totals[i] + "").GetDecimalValue().ToString("N2");

                var r = new GridViewRow(gvReport.Rows.Count + 1, -1, e.Row.RowType, e.Row.RowState);
                for (int i = 0; i < categories.Count + 2; i++)
                    r.Cells.Add(new TableCell());

                r.Cells[1].Text = LanguageManager.GetTranslation("Average");
                for (int i = 0; i < categories.Count; i++)
                    r.Cells[i + 2].Text = ((totals[i] / gvReport.Rows.Count) + "").GetDecimalValue().ToString("N2");

                ((Table)gvReport.Controls[0]).Rows.Add(r);
            }

        }

        protected void ddlPages_SelectedIndexChanged(Object sender, EventArgs e)
        {
            GridViewRow gvrPager = gvReport.BottomPagerRow;
            DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages");
            gvReport.PageIndex = ddlPages.SelectedIndex;
            BindGrid();
        }

        bool isUsingMallIntegration = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.MallIntegration.UseMallManagement), false);
    }
}
