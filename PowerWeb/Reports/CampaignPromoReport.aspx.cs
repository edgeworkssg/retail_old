using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PowerPOS;
using SubSonic;
using System.Data;

namespace PowerWeb.Reports
{
    public partial class CampaignPromoReport : PageBase
    {

        private const string SORT_DIRECTION = "SORT_DIRECTION";
        private const string ORDER_BY = "ORDER_BY";

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
                //lblYear.Text = DateTime.Today.Year.ToString();
                ddlOutlet.Items.AddRange(PointOfSaleController.FetchOutletNames(false));
                ddlOutlet.DataBind();
                txtStartDate.Text = DateTime.Today.ToString("dd MMM yyyy");
                txtEndDate.Text = DateTime.Today.ToString("dd MMM yyyy");
                ViewState[ORDER_BY] = "";
                ViewState[SORT_DIRECTION] = "asc";
                BindGrid();
            }
        }

        private void BindGrid()
        {
            string orderby = ViewState[ORDER_BY] == null ? "" : ViewState[ORDER_BY].ToString();
            string sortdir = ViewState[SORT_DIRECTION] == null ? "" : ViewState[SORT_DIRECTION].ToString();

            DateTime startDate = DateTime.Parse(txtStartDate.Text);
            DateTime endDate = DateTime.Parse(txtEndDate.Text);
            DataTable dt =
                ReportController.FetchCampaignReport(
                startDate, endDate.AddSeconds(86399),
                txtSearch.Text, ddlOutlet.SelectedValue.ToString(), orderby, sortdir);
            gvReport.DataSource = dt;
            gvReport.DataBind(); 
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
            ViewState[ORDER_BY] = e.SortExpression;
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
            if (ViewState[ORDER_BY] != null)
            {
                string sortedBy = ViewState[ORDER_BY].ToString();
                if (sortedBy == sortBy)
                {
                    //the direction should be desc
                    sortDir = " DESC";
                    //reset the sorter to null
                    ViewState[ORDER_BY] = null;
                }
                else
                {
                    //this is the first sort for this row
                    //put it to the ViewState
                    ViewState[ORDER_BY] = sortBy;
                }
            }
            else
            {
                //it's null, so this is the first sort
                ViewState[ORDER_BY] = sortBy;
            }

            return sortDir;
        }

        protected void gvReport_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            ViewState[SORT_DIRECTION] = null;
            if (rdbYear.Checked)
            {
                txtStartDate.Text = (new DateTime(int.Parse(ddlYear.Text), 1, 1)).ToString("dd MMM yyyy");
                txtEndDate.Text = new DateTime(int.Parse(ddlYear.Text), 12, DateTime.DaysInMonth(int.Parse(ddlYear.Text), 12)).ToString("dd MMM yyyy");
            }
            BindGrid();
        }

        protected void gvReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvReport.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ddlOutlet.SelectedIndex = 0;
            txtStartDate.Text = DateTime.Today.ToString("dd MMM yyyy");
            txtEndDate.Text = DateTime.Today.ToString("dd MMM yyyy");
            //txtCategoryName.Text = "";        
            //txtPointOfSale.Text = "";
            gvReport.PageIndex = 0;
        }
        protected void lnkExport_Click(object sender, EventArgs e)
        {
            BindGrid();
            DataTable dt = (DataTable)gvReport.DataSource;
            CommonWebUILib.ExportCSV(dt, this.Page.Title.Trim(' '), this.Page.Title, gvReport);
        }
    }
}
