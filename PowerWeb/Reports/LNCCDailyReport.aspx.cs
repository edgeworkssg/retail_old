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
    public partial class LNCCDailyReport : System.Web.UI.Page
    {
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
                ddlOutlet.Items.AddRange(PointOfSaleController.FetchOutletNames(true));
                ddlOutlet.DataBind();
                ddCategory.Items.AddRange(ItemController.FetchCategoryNamesInListItem());
                ddCategory.DataBind();
                txtStartDate.Text = DateTime.Today.AddDays(-1).ToString("dd MMM yyyy");
                txtEndDate.Text = DateTime.Today.AddDays(-1).ToString("dd MMM yyyy");

                BindGrid();
            }
        }

        protected void BindGrid()
        {
            DateTime startDate = DateTime.Parse(txtStartDate.Text);
            DateTime endDate = DateTime.Parse(txtEndDate.Text);

            DataTable dt = ReportController.GetLNCCReport(startDate, endDate, ddItemDepartment.SelectedValue.ToString(),
                ddCategory.SelectedValue.ToString(), ddlOutlet.SelectedItem.Text, txtSearch.Text);

            gvReport.DataSource = dt;
            gvReport.DataBind();
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
            //ViewState["sortBy"] = e.SortExpression;
            ////rebind the grid
            //if (ViewState[SORT_DIRECTION] == null || ((string)ViewState[SORT_DIRECTION]) == SqlFragment.ASC)
            //{
            //    ViewState[SORT_DIRECTION] = SqlFragment.DESC;
            //}

            //else
            //{
            //    ViewState[SORT_DIRECTION] = SqlFragment.ASC;
            //}

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
            //string sortDir = " ASC";
            //if (ViewState["sortBy"] != null)
            //{
            //    string sortedBy = ViewState["sortBy"].ToString();
            //    if (sortedBy == sortBy)
            //    {
            //        //the direction should be desc
            //        sortDir = " DESC";
            //        //reset the sorter to null
            //        ViewState["sortBy"] = null;
            //    }

            //    else
            //    {
            //        //this is the first sort for this row
            //        //put it to the ViewState
            //        ViewState["sortBy"] = sortBy;
            //    }

            //}

            //else
            //{
            //    //it's null, so this is the first sort
            //    ViewState["sortBy"] = sortBy;
            //}

            return "";
        }
        protected void gvReport_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {

            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            ViewState["sortBy"] = null;

            if (rdbMonth.Checked)
            {
                txtStartDate.Text = (new DateTime(int.Parse(ddlYear.SelectedValue.ToString()), int.Parse(ddlMonth.SelectedValue), 1)).ToString("dd MMM yyyy");
                txtEndDate.Text = new DateTime(int.Parse(ddlYear.SelectedValue.ToString()), int.Parse(ddlMonth.SelectedValue), DateTime.DaysInMonth(int.Parse(ddlYear.SelectedValue.ToString()), int.Parse(ddlMonth.SelectedValue))).ToString("dd MMM yyyy");
            }

            BindGrid();
        }
        protected void btnClear_Click(object sender, EventArgs e)
        {

            ddlOutlet.SelectedIndex = 0;
            ddItemDepartment.SelectedIndex = 0;
            ddCategory.SelectedIndex = 0;
            txtStartDate.Text = DateTime.Today.ToString("dd MMM yyyy");
            txtEndDate.Text = DateTime.Today.ToString("dd MMM yyyy");
            txtSearch.Text = "";

            gvReport.PageIndex = 0;
        }
        protected void lnkExport_Click(object sender, EventArgs e)
        {
            BindGrid();
            DataTable dt = (DataTable)gvReport.DataSource;
            string text = CommonWebUILib.ExportText(dt, gvReport);

            Response.Clear();
            Response.ClearHeaders();

            Response.AddHeader("Content-Length", text.Length.ToString());
            Response.ContentType = "text/plain";
            Response.AppendHeader("content-disposition", "attachment;filename=\"" + DateTime.Now.ToString("ddMMyyHHmm") + ".txt\"");

            Response.Write(text);
            Response.End();
        }
        protected void ddDept_Init(object sender, EventArgs e)
        {

        }
        protected void ddPOS_Init(object sender, EventArgs e)
        {

        }
    }
}
