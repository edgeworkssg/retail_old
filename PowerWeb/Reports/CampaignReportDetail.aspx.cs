using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SubSonic;
using PowerPOS;

namespace PowerWeb.Reports
{
    public partial class CampaignReportDetail : PageBase
    {
        private const string SORT_DIRECTION = "SORT_DIRECTION";
        private const string ORDER_BY = "ORDER_BY";

        private DateTime StartDate = new DateTime();
        private DateTime EndDate = new DateTime();
        private Int32 PromoCampaignHdrID = 0;
        private string strOutlet = "";
        private string Search = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string promoCode = Request.QueryString["PromoCode"];
                string strStartDate = Request.QueryString["StartDate"];
                string strEndDate = Request.QueryString["EndDate"];
                string outlet = Request.QueryString["Outlet"];
                string strSearch = Request.QueryString["Search"];
                
                strOutlet = string.IsNullOrEmpty(outlet) ? "ALL" : outlet;
                StartDate = DateTime.Parse(strStartDate);
                EndDate = DateTime.Parse(strEndDate);
                PromoCampaignHdrID = Int32.Parse(promoCode);
                PromoCampaignHdr head = new PromoCampaignHdr(promoCode);

                txtPromoID.Value = head.PromoCampaignHdrID.ToString();
                txtPromoCode.Text = head.PromoCode;
                txtPromoName.Text = head.PromoCampaignName;
                txtStartDate.Text = strStartDate;
                txtEndDate.Text = strEndDate;
                txtOutlet.Text = strOutlet;
                txtSearch.Value = "%";

                BindGrid();
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

        private void BindGrid()
        {
            string orderby = ViewState[ORDER_BY] == null ? "" : ViewState[ORDER_BY].ToString();
            string sortdir = ViewState[SORT_DIRECTION] == null ? "" : ViewState[SORT_DIRECTION].ToString();

            strOutlet = string.IsNullOrEmpty(txtOutlet.Text) ? "ALL" : txtOutlet.Text;
            StartDate = DateTime.Parse(txtStartDate.Text);
            EndDate = DateTime.Parse(txtEndDate.Text);
            PromoCampaignHdrID = Int32.Parse(txtPromoID.Value);
            PromoCampaignHdr head = new PromoCampaignHdr(PromoCampaignHdrID);
            Search = txtSearch.Value;

           DataTable dt = ReportController.FetchCampaignReportDetail(PromoCampaignHdrID,
                StartDate, EndDate.AddSeconds(86399),
                Search, strOutlet, orderby, sortdir);
            gvReport.DataSource = dt;
            gvReport.DataBind(); 
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

        protected void gvReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvReport.PageIndex = e.NewPageIndex;
            BindGrid();
        }
    }
}
