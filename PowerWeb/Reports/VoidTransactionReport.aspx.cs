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

public partial class VoidTransactionReport : PageBase
{
    private const int AMOUNT = 5;
    private const string SORT_DIRECTION = "SORT_DIRECTION";
    private const string ORDER_BY = "ORDER_BY";


    protected void Page_Init(object sender, EventArgs e)
    {
        OutletDropdownList.setIsUseAllBreakdownOutlet(false);
        OutletDropdownList.setIsUseAllBreakdownPOS(false);
    }

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

                ddlEndTimeHour.SelectedValue = "23";
                ddlEndTimeMinute.SelectedValue = "59";
                ddlEndTimeSecond.SelectedValue = "59";
            }

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
        string selectedOutlet = OutletDropdownList.GetDdlOutletSelectedItemText;
        if (selectedOutlet == "ALL") selectedOutlet = "";
        string SelectedPOS = OutletDropdownList.GetDdlPOSSelectedItemText;
        if (SelectedPOS == "ALL")
        {
            SelectedPOS = "%";
        }
        DateTime startDate = DateTime.Parse(txtStartDate.Text + " " + ddlStartTimeHour.SelectedValue + ":" + ddlStartTimeMinute.SelectedValue + ":" + ddlStartTimeSecond.SelectedValue);
        DateTime endDate = DateTime.Parse(txtEndDate.Text + " " + ddlEndTimeHour.SelectedValue + ":" + ddlEndTimeMinute.SelectedValue + ":" + ddlEndTimeSecond.SelectedValue);
        string paymentType = "";
        DataTable dt = ReportController.FetchTransactionReport(            
            startDate, endDate,
            txtOrderNo.Text, txtCashier.Text, paymentType, 
            selectedOutlet, txtRemark.Text, "1","");
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
            //CommonWebUILib.SetVideoBrowserClientScript(e, 2, int.Parse(((DataTable)gvReport.DataSource).Rows[e.Row.RowIndex+gvReport.PageIndex * gvReport.PageSize]["PointOfSaleID"].ToString()));
            e.Row.Cells[AMOUNT].Text = String.Format("{0:N2}", Decimal.Parse(e.Row.Cells[AMOUNT].Text));
            if (e.Row.Cells[12].Text.ToLower() == "true")
            {
                e.Row.Cells[12].Text = "Yes";
                e.Row.ForeColor = System.Drawing.Color.White;
                e.Row.BackColor = System.Drawing.Color.DarkRed;
            }
            else
            {
                e.Row.Cells[12].Text = "No";
            }
        }           
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
        txtCashier.Text = "";
        txtOrderNo.Text = "";
        //txtPointOfSale.Text = "";
        gvReport.PageIndex = 0;
    }
    protected void lnkExport_Click(object sender, EventArgs e)
    {
        BindGrid();
        DataTable dt = (DataTable)gvReport.DataSource;
        CommonWebUILib.ExportCSV(dt, this.Page.Title.Trim(' '), this.Page.Title,gvReport);
    }        
    protected void ddPOS_Init(object sender, EventArgs e)
    {
        /*
        if (Session["DeptID"] != null && Session["DeptID"].ToString() != "0")
        {
            ddPOS.WhereField = "DepartmentID";
            ddPOS.WhereValue = Session["DeptID"].ToString();
        }*/
    }
}
