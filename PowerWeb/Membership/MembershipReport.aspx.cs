using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using PowerPOS.Container;
using PowerPOS;
using SubSonic;
using SubSonic.Utilities;

public partial class MembershipReport : PageBase
{
    private const int AMOUNT = 4;
    private const string SORT_DIRECTION = "SORT_DIRECTION";
    private const string ORDER_BY = "ORDER_BY";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {                        
            txtStartExpiryDate.Text = DateTime.Today.ToString("dd MMM yyyy");
            txtEndExpiryDate.Text = DateTime.Today.ToString("dd MMM yyyy");

            txtStartSubsDate.Text = DateTime.Today.ToString("dd MMM yyyy");
            txtEndSubsDate.Text = DateTime.Today.ToString("dd MMM yyyy");
            
            txtStartBirthDate.Text = DateTime.Today.ToString("dd MMM yyyy");
            txtEndBirthDate.Text = DateTime.Today.ToString("dd MMM yyyy");

            UserMstCollection umc = new UserMstCollection();
            umc.Where(UserMst.Columns.Deleted, false);
            umc.Where(UserMst.Columns.IsASalesPerson, true);
            umc.Load();
            umc.Insert(0, new UserMst());
            ddlStylist.DataSource = umc;
            ddlStylist.DataBind();

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
        if (cbUseBirthDayMonth.Checked && ddlMonth.SelectedValue == "0")
        {            
            return;
        }
        DateTime startExpiryDate = DateTime.Parse(txtStartExpiryDate.Text);
        DateTime endExpiryDate = DateTime.Parse(txtEndExpiryDate.Text);

        DateTime startSubsDate = DateTime.Parse(txtStartSubsDate.Text);
        DateTime endSubsDate = DateTime.Parse(txtEndSubsDate.Text);

        DateTime startBirthDate = DateTime.Parse(txtStartBirthDate.Text);
        DateTime endBirthDate = DateTime.Parse(txtEndBirthDate.Text);

        string stylst = "";
        if (ddlStylist.SelectedIndex > 0)
        {
            stylst = new UserMst(UserMst.Columns.DisplayName, ddlStylist.SelectedValue.ToString()).UserName;
        }
        DataTable dt = ReportController.FetchMembershipReport(
            
            cbUseStartExpiryDate.Checked, cbUseStartExpiryDate.Checked,
            startExpiryDate, endExpiryDate.AddSeconds(86399),

            cbUseStartSubsDate.Checked, cbUseEndSubsDate.Checked,
            startSubsDate, endSubsDate.AddSeconds(86399),
            
            cbUseStartBirthDate.Checked, cbUseStartBirthDate.Checked,
            startBirthDate, endBirthDate.AddSeconds(86399), 
            cbUseBirthDayMonth.Checked,int.Parse(ddlMonth.SelectedValue.ToString()),
            txtFromMembershipNo.Text, txtToMembershipNo.Text, 
            int.Parse(ddGroupName.SelectedValue),txtNRIC.Text, ddlGender.SelectedValue,
            txtNameToAppear.Text, txtStreetName.Text,            
            txtMobileNo.Text, txtHomeNo.Text, stylst, txtEmail.Text, 
            ViewState["sortBy"].ToString(), ViewState[SORT_DIRECTION].ToString());
        dt = CommonUILib.DataTableConvertBoolColumnToYesOrNo(dt);
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
        ViewState["sortBy"]  = e.SortExpression;
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

    protected void gvReport_RowDataBound(object sender, GridViewRowEventArgs e)
    {
       
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            /*
            DateTime bday, expirydate;
            
                
                if (DateTime.TryParse(e.Row.Cells[9].Text, out bday))
                {
                    e.Row.Cells[9].Text = bday.ToString("dd MMM yyyy");
                }
                if (DateTime.TryParse(e.Row.Cells[26].Text, out expirydate)) {
                                    e.Row.Cells[26].Text = expirydate.ToString("dd MMM yyyy");
                }
            */
        }           
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        ViewState["sortBy"] = null;
        BindGrid();
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {        
        gvReport.PageIndex = 0;
    }
    protected void lnkExport_Click(object sender, EventArgs e)
    {
        BindGrid();
        DataTable dt = (DataTable)gvReport.DataSource;
        CommonWebUILib.ExportCSV(dt, this.Page.Title.Trim(' '), this.Page.Title, gvReport);
    }
}
