using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using PowerPOS;
using SubSonic;

namespace PowerWeb.Reports
{
    public partial class NewCommissionReport : System.Web.UI.Page
    {
        private const string SORT_DIRECTION = "SORT_DIRECTION";
        private const string ORDER_BY = "ORDER_BY";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                txtStartDate.Text = DateTime.Today.ToString("dd MMM yyyy");
                txtEndDate.Text = DateTime.Today.ToString("dd MMM yyyy");
                ViewState["sortBy"] = "";
                ViewState[SORT_DIRECTION] = "";
                BindGrid();
            }
        }

        private void BindGrid()
        {
            if (ViewState["sortBy"] == null)
            {
                ViewState["sortBy"] = "";
            }

            string commissionBasedOn = AppSetting.GetSetting(AppSetting.SettingsName.Commission.CommissionBasedOn);

            DateTime startDate = DateTime.Parse(txtStartDate.Text);
            if (!cbUseStartDate.Checked)
                startDate = startDate.AddYears(-100);

            DateTime endDate = DateTime.Parse(txtEndDate.Text);
            if (!cbUseEndDate.Checked)
                endDate = endDate.AddYears(100);

            string sql = @"EXEC [dbo].[REPORT_NewCommissionReport]
		                    @FilterStartDate = @StartDate,
		                    @FilterEndDate = @EndDate,
		                    @FilterUserName = @UserName";


            QueryCommand cmd = new QueryCommand(sql);
            cmd.AddParameter("@StartDate", startDate);
            cmd.AddParameter("@EndDate", endDate);
            cmd.AddParameter("@UserName", ddlUser.SelectedValue);

            DataTable dt = new DataTable();
            dt.Load(DataService.GetReader(cmd));

            gvReport.DataSource = dt;
            gvReport.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            ViewState["sortBy"] = null;
            BindGrid();
        }

        protected void cldStartDate_SelectionChanged(object sender, EventArgs e)
        {
            cbUseStartDate.Checked = true;
        }
        protected void cldEndDate_SelectionChanged(object sender, EventArgs e)
        {
            cbUseEndDate.Checked = true;
        }
        protected void btnClear_Click(object sender, EventArgs e)
        {
            cbUseStartDate.Checked = false;
            cbUseEndDate.Checked = false;

            txtStartDate.Text = DateTime.Today.ToString("dd MMM yyyy");
            txtEndDate.Text = DateTime.Today.ToString("dd MMM yyyy");

            gvReport.PageIndex = 0;
        }
        protected void lnkExport_Click(object sender, EventArgs e)
        {
            BindGrid();
            DataTable dt = (DataTable)gvReport.DataSource;
            CommonWebUILib.ExportCSV(dt, this.Page.Title.Trim(' '), this.Page.Title, gvReport);
        }

        protected void gvReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvReport.PageIndex = e.NewPageIndex;
            BindGrid();
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

        protected void gvReport_DataBound(object sender, EventArgs e)
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

        protected void gvReport_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                DataTable dt = (DataTable)gvReport.DataSource;
                decimal totalCommission = (from o in dt.AsEnumerable()
                                           select o.Field<decimal>("TotalCommission")).Sum<decimal>(o => o);
                e.Row.Cells[4].Text = totalCommission.ToString("N2");
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

        protected void ddlUser_Init(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            var data = new UserMstController().FetchAll().Where(o => o.Deleted.GetValueOrDefault(false) == false).OrderBy(o => o.DisplayName).ToList();
            data.Insert(0, new UserMst { UserName = "ALL", DisplayName = "ALL" });
            ddl.DataSource = data;
            ddl.DataBind();
        }
    }
}
