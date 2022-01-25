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
using PowerPOS;
using SubSonic;
using SubSonic.Utilities;
using System.Linq;
using System.Collections.Generic;

namespace PowerWeb.Reports
{
    public partial class OutletTransactionHourlyReport : PageBase
    {
        private const string SORT_DIRECTION = "SORT_DIRECTION";
        private const string ORDER_BY = "ORDER_BY";

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
                txtStartDate.Text = DateTime.Today.ToString("dd MMM yyyy");
                txtEndDate.Text = DateTime.Today.AddDays(1).ToString("dd MMM yyyy");
                ViewState["Sort"] = "";
                ViewState[SORT_DIRECTION] = "ASC";
                BindGrid();
            }
        }

        private void SetFormSetting()
        {
            try
            {
                string posText = LanguageManager.GetTranslation(LabelController.PointOfSaleText);
                string outletText = LanguageManager.GetTranslation(LabelController.OutletText);
                OutletDropdownList.SetLabelOutlet(outletText);
                OutletDropdownList.SetLabelPOS(posText);
                this.Page.Title = LanguageManager.GetTranslation(LabelController.OutletText) + " " + LanguageManager.GetTranslation("Transaction Hourly Report");
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
                string sort = ViewState["Sort"].ToString();
                String tmpStartDate, tmpEndDate;
                tmpStartDate = txtStartDate.Text + " " + ddStartHour.SelectedValue + ":" + ddStartMinute.SelectedItem;
                tmpEndDate = txtEndDate.Text + " " + ddEndHour.SelectedValue + ":" + ddEndMinute.SelectedItem;

                DateTime startDate = DateTime.Parse(tmpStartDate);
                DateTime endDate = DateTime.Parse(tmpEndDate);

                DataTable dt = ReportController.FetchOutletTransactionHourlyReport(startDate, endDate, (OutletDropdownList.GetDdlPOSSelectedValue + "").GetIntValue(), (OutletDropdownList.GetDdlOutletSelectedItemText + ""), chkShowBeforeGST.Checked);
                DataView dv = new DataView();
                dv.Table = LanguageManager.GetTranslation(dt, 0, 1, dt.Columns.Count - 1);
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
            txtStartDate.Text = DateTime.Today.ToString("dd MMM yyyy");
            txtEndDate.Text = DateTime.Today.ToString("dd MMM yyyy");
            BindGrid();
        }

        List<decimal> totalAmount = new List<decimal>();
        public void CalculateSum()
        {
            totalAmount = new List<decimal>();

            DataView dv = (DataView)gvReport.DataSource;
            DataTable dt = dv.ToTable();
            for (int j = 2; j < dt.Columns.Count; j++)
            {
                decimal total = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    total += dt.Rows[i][j].ToString().GetDecimalValue();
                }
                totalAmount.Add(total);
            }
        }

        protected void gvReport_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Format date
                DataRow row = ((DataRowView)e.Row.DataItem).Row;
                e.Row.Cells[0].Text = ((DateTime)row["Order Date"]).ToString("dd MMM yyyy");

                // Add formatting
                for (int colIndex = 2; colIndex < row.ItemArray.Length; colIndex++)
                {
                    e.Row.Cells[colIndex].Text = (row[colIndex] + "").GetDecimalValue().ToString("N2");
                }
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                CalculateSum();
                for (int i = 0; i < totalAmount.Count; i++)
                {
                    e.Row.Cells[i + 2].Text = totalAmount[i].ToString("N2");
                }
            }
        }

    }
}
