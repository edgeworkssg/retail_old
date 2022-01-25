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

public partial class MembershipTransactionReport : PageBase
{
    private const int QTY_COL = 8;
    private const int AMOUNT = 9;
    private const int REMARK_COL = 16;
    private const int LINEINFO_COL = 17;
    private const int BALANCEPAYMENT_COL = 18;
    private const int QTYONHAND_COL = 19;
    
    private const string SORT_DIRECTION = "SORT_DIRECTION";
    private const string ORDER_BY = "ORDER_BY";

    private string lineInfoCaption;

    protected void Page_Init(object sender, EventArgs e)
    {
        OutletDropdownList.setIsUseAllBreakdownOutlet(false);
        OutletDropdownList.setIsUseAllBreakdownPOS(false);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            ddCategory.Items.AddRange(ItemController.FetchCategoryNamesInListItem());
            ddCategory.DataBind();
            //ddlOutlet.DataSource = PointOfSaleController.FetchOutletNames(false);
            //ddlOutlet.DataBind();
            txtStartDate.Text = DateTime.Today.ToString("dd MMM yyyy");
            txtEndDate.Text = DateTime.Today.AddDays(1).ToString("dd MMM yyyy");
            ViewState["sortBy"] = "";
            ViewState[SORT_DIRECTION] = "";

            lineInfoCaption = AppSetting.CastString(AppSetting.GetSetting(AppSetting.SettingsName.LineInfo.ReplaceTextWith));
            if (!string.IsNullOrEmpty(lineInfoCaption))
            {
                litLineInfo.Text = lineInfoCaption;
                gvReport.Columns[LINEINFO_COL].HeaderText = lineInfoCaption;
            }

            gvReport.Columns[REMARK_COL].Visible = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Membership.ShowRemarkInTransactionReport), false);
            gvReport.Columns[LINEINFO_COL].Visible = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Membership.ShowLineInfoInTransactionReport), false);
            gvReport.Columns[BALANCEPAYMENT_COL].Visible = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Membership.ShowBalancePaymentInTransactionReport), false);
            gvReport.Columns[QTY_COL].Visible = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Membership.ShowQtyInTransactionReport), false);
            gvReport.Columns[QTYONHAND_COL].Visible = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Membership.ShowQtyOnHandInTransactionReport), false);
            litRemark.Visible = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Membership.ShowRemarkInTransactionReport), false);
            txtRemark.Visible = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Membership.ShowRemarkInTransactionReport), false);
            litLineInfo.Visible = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Membership.ShowLineInfoInTransactionReport), false);
            txtLineInfo.Visible = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Membership.ShowLineInfoInTransactionReport), false);

            BindGrid();
        }
    }

    private void BindGrid()
    {
        if (ViewState["sortBy"] == null)
        {
            ViewState["sortBy"] = "";
        }

        string selectedOutlet = OutletDropdownList.GetDdlOutletSelectedItemText;
        if (selectedOutlet == "ALL") selectedOutlet = "";
        string SelectedPOS = OutletDropdownList.GetDdlPOSSelectedItemText;
        if (SelectedPOS == "ALL")
        {
            SelectedPOS = "%";
        }
        DateTime startDate = DateTime.Parse(txtStartDate.Text);
        DateTime endDate = DateTime.Parse(txtEndDate.Text);

        startDate = startDate.AddHours(ddStartHour.SelectedValue.GetDoubleValue());
        startDate = startDate.AddMinutes(ddStartMinute.SelectedValue.GetDoubleValue());
        endDate = endDate.AddHours(ddEndHour.SelectedValue.GetDoubleValue());
        endDate = endDate.AddMinutes(ddEndMinute.SelectedValue.GetDoubleValue());

        DataTable dt = ReportController.FetchMembershipTransactionReport(
            cbUseStartDate.Checked, cbUseStartDate.Checked,
            startDate, endDate.AddSeconds(86399),txtMembershipNo.Text,int.Parse(ddGroupName.SelectedValue.ToString()),
            txtNRIC.Text,txtFirstName.Text,txtLastName.Text,txtNameToAppear.Text,txtOrderNo.Text,            
            txtItemName.Text,ddCategory.SelectedValue.ToString(), 0, SelectedPOS, selectedOutlet,ddDept.SelectedValue.ToString(),
            "%" + txtRemark.Text + "%", "%" + txtLineInfo.Text + "%", txtStaff.Text, 
            ViewState["sortBy"].ToString(), ViewState[SORT_DIRECTION].ToString());        
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
            CommonWebUILib.SetVideoBrowserClientScript(e, 2, int.Parse(((DataTable)gvReport.DataSource).Rows[e.Row.RowIndex+gvReport.PageIndex * gvReport.PageSize]["PointOfSaleID"].ToString()));
            e.Row.Cells[AMOUNT].Text = String.Format("{0:N2}", Decimal.Parse(e.Row.Cells[AMOUNT].Text));
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            DataTable dt = (DataTable)gvReport.DataSource;            
            e.Row.Cells[AMOUNT].Text = String.Format("{0:N2}", calculateSum());
        }   
    }
    public decimal calculateSum()
    {
        
        decimal total = 0;
        DataTable dt = (DataTable)gvReport.DataSource;
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            total += decimal.Parse(dt.Rows[i]["LineAmount"].ToString());
        }
        return total;

    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        ViewState["sortBy"] = null;
        BindGrid();
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        cbUseStartDate.Checked = false;
        cbUseEndDate.Checked = false;
        OutletDropdownList.ResetDdlOutlet();
        OutletDropdownList.ResetDdlPOS();
        txtStartDate.Text = DateTime.Today.ToString("dd MMM yyyy");
        txtEndDate.Text = DateTime.Today.ToString("dd MMM yyyy");
        txtItemName.Text = "";
        txtFirstName.Text = "";
        txtLastName.Text = "";
        txtNameToAppear.Text = "";
        txtMembershipNo.Text = "";
        txtNRIC.Text = "";        
        txtOrderNo.Text = "";
        //txtPointOfSale.Text = "";
        txtStaff.Text = "";
        gvReport.PageIndex = 0;
    }
    protected void lnkExport_Click(object sender, EventArgs e)
    {
        BindGrid();
        DataTable dt = (DataTable)gvReport.DataSource;
        CommonWebUILib.ExportCSVWithInvinsibleFields(dt, this.Page.Title.Trim(' '), this.Page.Title,gvReport);
    }    
    protected void ddDept_Init(object sender, EventArgs e)
    {
        if (Session["DeptID"] != null && Session["DeptID"].ToString() != "0")
        {
            ddDept.WhereField = "DepartmentID";
            ddDept.WhereValue = Session["DeptID"].ToString();
        }
    }
    protected void ddPOS_Init(object sender, EventArgs e)
    {
        //if (Session["DeptID"] != null && Session["DeptID"].ToString() != "0")
        //{
        //    ddPOS.WhereField = "DepartmentID";
        //    ddPOS.WhereValue = Session["DeptID"].ToString();
        //}
    }
}
