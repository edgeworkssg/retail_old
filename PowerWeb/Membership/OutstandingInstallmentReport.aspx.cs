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

public partial class OutstandingInstallmentReport : PageBase
{
    private const string SORT_DIRECTION = "SORT_DIRECTION";
    private const string ORDER_BY = "ORDER_BY";

    private const int CREDIT = 6;
    private const int DEBIT = 7;
    private const int BALANCE = 8;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {                        
            UserMstCollection umc = new UserMstCollection();
            umc.Where(UserMst.Columns.Deleted, false);
            umc.Where(UserMst.Columns.IsASalesPerson, true);
            umc.Load();
            umc.Insert(0, new UserMst());

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

        /*
        DataTable dt = ReportController.FetchMembershipReport(
            cbUseStartExpiryDate.Checked, cbUseStartExpiryDate.Checked,
            startExpiryDate, endExpiryDate.AddSeconds(86399),
            cbUseStartBirthDate.Checked, cbUseStartBirthDate.Checked,
            startBirthDate, endBirthDate.AddSeconds(86399), 
            cbUseBirthDayMonth.Checked,int.Parse(ddlMonth.SelectedValue.ToString()),
            txtFromMembershipNo.Text, txtToMembershipNo.Text, 
            int.Parse(ddGroupName.SelectedValue),txtNRIC.Text, ddlGender.SelectedValue,
            txtNameToAppear.Text, txtStreetName.Text,            
            txtMobileNo.Text, txtHomeNo.Text, stylst, txtEmail.Text, 
            ViewState["sortBy"].ToString(), ViewState[SORT_DIRECTION].ToString());
         */

        DataTable dt = ReportController.FetchInstallmentReport(txtSearch.Text, cbIsShowOutstandingOnly.Checked);

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
            //decimal Credit, Debit, Balance;

            //bool result = false;

            //result = Decimal.TryParse(e.Row.Cells[CREDIT - 1].Text, out Credit);
            //if (!result)
            //    Credit = 0;
            //e.Row.Cells[CREDIT - 1].Text = String.Format("{0:N2}", Credit);

            //result = Decimal.TryParse(e.Row.Cells[DEBIT - 1].Text, out Debit);
            //if (!result)
            //    Debit = 0;
            //e.Row.Cells[DEBIT - 1].Text = String.Format("{0:N2}", Debit);

            //result = Decimal.TryParse(e.Row.Cells[BALANCE - 1].Text, out Balance);
            //if (!result)
            //    Balance = 0;
            //e.Row.Cells[BALANCE - 1].Text = String.Format("{0:N2}", Balance);
        }           
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        ViewState["sortBy"] = null;
        BindGrid();
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        txtSearch.Text = "";
        cbIsShowOutstandingOnly.Checked = false;
        gvReport.PageIndex = 0;
    }
    protected void lnkExport_Click(object sender, EventArgs e)
    {
        BindGrid();
        DataTable dt = (DataTable)gvReport.DataSource;
        CommonWebUILib.ExportCSV(dt, this.Page.Title.Trim(' '), this.Page.Title, gvReport);
    }

    protected void gvReport_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Select")
        {
            //gvReport.SelectedRow.
            //DataRow row = (DataRow)sender;\
            
           // Response.Redirect();
        }
    }

    protected void gvReport_SelectedIndexChanged(object sender, EventArgs e)
    {
        string membershipNo = gvReport.SelectedRow.Cells[1].Text;
        string script = "<script>window.open('MemberTransactionsReport.aspx?id=" + membershipNo + "',null,'left=0," + " top=0,height=600, width=900, status=no, resizable= no, scrollbars= no," + "toolbar= no,location= no, menubar= no');</script>";
        if (!ClientScript.IsStartupScriptRegistered("popUp"))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "popUp", script);
        }
        //LinkButton button = (LinkButton)gvReport.SelectedRow.FindControl("View");
        //button.Attributes.Add("OnClientClick", "window.open('WebForm2.aspx','','height=600,width=600');return false");
       // Response.Redirect("javascript:void();");
        //gvReport.DataBind();
        BindGrid();
    }

    protected void cbIsShowOutstandingOnly_OnCheckedChanged(Object sender, EventArgs e)
    {
        BindGrid();
    }

}
