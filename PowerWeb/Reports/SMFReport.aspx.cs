using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using PowerPOS;

namespace PowerWeb.Reports
{
    public partial class SMFReport : PageBase
    {
        private const string SORT_DIRECTION = "SORT_DIRECTION";
        private const string ORDER_BY = "ORDER_BY";

        int ReceiptNo_COL = 0;
        int TotalSMF_COL = 0;
        int PatientPayable_COL = 0;
        int PAMedifund_COL = 0;
        int PWF_COL = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                for (var i = 0; i < 24; i++)
                {
                    ddlStartTimeHour.Items.Add(new ListItem(i.ToString().PadLeft(2, '0')));
                    ddlEndTimeHour.Items.Add(new ListItem(i.ToString().PadLeft(2, '0')));
                }

                for (var i = 0; i < 60; i++)
                {
                    ddlStartTimeMinute.Items.Add(new ListItem(i.ToString().PadLeft(2, '0')));
                    ddlStartTimeSecond.Items.Add(new ListItem(i.ToString().PadLeft(2, '0')));
                    ddlEndTimeMinute.Items.Add(new ListItem(i.ToString().PadLeft(2, '0')));
                    ddlEndTimeSecond.Items.Add(new ListItem(i.ToString().PadLeft(2, '0')));
                }

                txtStartDate.Text = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).ToString("dd MMM yyyy");
                txtEndDate.Text = DateTime.Today.AddDays(-1).ToString("dd MMM yyyy");
                ViewState["Sort"] = "";
                BindGrid(ViewState["Sort"].ToString());
            }
        }

        private void BindGrid(string sort)
        {
            /*           
            DataTable dt = 
                ReportController.FetchInventorySummaryReport(ddDept.SelectedValue.ToString());
            */
            //DataTable dt = ReportController.FetchStockReportBreakdownByLocation
            //    (txtSearch.Text, false, "", "", "");
            DateTime startDate = DateTime.Parse(txtStartDate.Text + " " + ddlStartTimeHour.SelectedValue + ":" + ddlStartTimeMinute.SelectedValue + ":" + ddlStartTimeSecond.SelectedValue);
            DateTime endDate = (DateTime.Parse(txtEndDate.Text + " " + ddlEndTimeHour.SelectedValue + ":" + ddlEndTimeMinute.SelectedValue + ":" + ddlEndTimeSecond.SelectedValue));
            DataTable dt = ReportController.GetSMFReport(txtSearch.Text, IsUseStartDate.Checked, IsUseEndDate.Checked, startDate, endDate);
            //DataView dv = new DataView();
            //dv.Table = dt;
            
            //if (sort == "")
            //    sort = "ReceiptNo";

            //dv.Sort = sort + " ASC";

            #region *) Get column position
            ReceiptNo_COL = dt.Columns["ReceiptNo"].Ordinal;
            TotalSMF_COL = dt.Columns["TotalSMF"].Ordinal;
            PatientPayable_COL = dt.Columns["PatientPayable"].Ordinal;
            PAMedifund_COL = dt.Columns["PAMedifund"].Ordinal;
            PWF_COL = dt.Columns["PWF"].Ordinal;
            #endregion

            gvReport.DataSource = dt;
            gvReport.DataBind();
        }

        protected void gvReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvReport.PageIndex = e.NewPageIndex;
            BindGrid(ViewState["Sort"].ToString());
        }

        protected void gvReport_DataBound(Object sender, EventArgs e)
        {

            //pull the datasource
            DataView ds = gvReport.DataSource as DataView;

            #region *) Get total and put in footer row
            if (gvReport.FooterRow != null)
            {
                DataTable dt = (DataTable)gvReport.DataSource;
                gvReport.FooterRow.Cells[ReceiptNo_COL].Text = "TOTAL";
                //gvReport.FooterRow.Cells[TotalSMF_COL].Text = dt.Compute("SUM(TotalSMF)", "").ToString();
                //gvReport.FooterRow.Cells[PatientPayable_COL].Text = dt.Compute("SUM(PatientPayable)", "").ToString();
                //gvReport.FooterRow.Cells[PAMedifund_COL].Text = dt.Compute("SUM(PAMedifund)", "").ToString();
                //gvReport.FooterRow.Cells[PWF_COL].Text = dt.Compute("SUM(PWF)", "").ToString();

                foreach (DataColumn col in dt.Columns)
                {
                    if (col.DataType == Type.GetType("System.Decimal"))
                    {
                        gvReport.FooterRow.Cells[col.Ordinal].Text = dt.Compute("SUM([" + col.ColumnName + "])", "").ToString();
                    }
                }

            }
            #endregion

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

        protected void ddlPages_SelectedIndexChanged(Object sender, EventArgs e)
        {
            GridViewRow gvrPager = gvReport.BottomPagerRow;
            DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages");
            gvReport.PageIndex = ddlPages.SelectedIndex;
            // a method to populate your grid
            BindGrid(ViewState["Sort"].ToString());
        }

        protected void lnkExport_Click(object sender, EventArgs e)
        {
            BindGrid(ViewState["Sort"].ToString());
            DataTable dt = (DataTable)gvReport.DataSource;

            #region *) Get Total from Gridview footer
            DataRow dr = dt.NewRow();
            dr["ReceiptNo"] = gvReport.FooterRow.Cells[ReceiptNo_COL].Text;
            dr["TotalSMF"] = gvReport.FooterRow.Cells[TotalSMF_COL].Text;
            dr["PatientPayable"] = gvReport.FooterRow.Cells[PatientPayable_COL].Text;
            dr["PAMedifund"] = gvReport.FooterRow.Cells[PAMedifund_COL].Text;
            dr["PWF"] = gvReport.FooterRow.Cells[PWF_COL].Text;
            dt.Rows.Add(dr);
            #endregion

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
            BindGrid(ViewState["Sort"].ToString());
        }

        protected void ddDept_Init(object sender, EventArgs e)
        {
            if (Session["DeptID"] != null && Session["DeptID"].ToString() != "0")
            {

            }
        }

        protected void ddDept_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGrid(ViewState["Sort"].ToString());
        }

        protected void gvReport_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        }
    }
}
