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

public partial class TransactionReport : PageBase
{
    private const int AMOUNT =6;
    private const int GST = 7;
    private const int AMOUNT_BEFORE_GST = 8;
    private const int DISCOUNT = 9;
    private const int GROSSAMOUNT = 10;
    private const int ISVOIDED = 19;
    private const int POINTOFSALE = 17;
    private const int OUTLET = 18;
    private const string SORT_DIRECTION = "SORT_DIRECTION";
    private const string ORDER_BY = "ORDER_BY";
    public bool ShowGrossAmount = false;

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
            //ddlOutlet.Items.AddRange(PointOfSaleController.FetchOutletNames(false, Session["UserName"] + ""));
            //ddlOutlet.DataBind();
            //ddPOS.Items.AddRange(PointOfSaleController.FetchPointOfSaleNames(false, Session["UserName"] + ""));
            //ddPOS.DataBind();

            for(int i=0; i<60; i++)
            {
                ddStartMinute.Items.Add(new ListItem(i.ToString("00")));
                ddStartSecond.Items.Add(new ListItem(i.ToString("00")));
                ddEndMinute.Items.Add(new ListItem(i.ToString("00")));
                ddEndSecond.Items.Add(new ListItem(i.ToString("00")));
            }

            if (Request.QueryString["POS"] != null)
            {
                string POS = Request.QueryString["POS"];
                string Outlet = Request.QueryString["Outlet"];
                DateTime start = DateTime.Parse(Request.QueryString["Start"]);
                DateTime end = DateTime.Parse(Request.QueryString["End"]);

                OutletDropdownList.SetDdlOutletSelectedValueFirstTime(Outlet);
                OutletDropdownList.SetDdlPOSSelectedValueFirstTime(POS);

                txtStartDate.Text = start.ToString("dd MMM yyyy");
                ddStartHour.SelectedValue = start.Hour.ToString("00");
                ddStartMinute.SelectedValue = start.Minute.ToString("00");
                ddStartSecond.SelectedValue = start.Second.ToString("00");

                txtEndDate.Text = end.ToString("dd MMM yyyy");
                ddEndHour.SelectedValue = end.Hour.ToString("00");
                ddEndMinute.SelectedValue = end.Minute.ToString("00");
                ddEndSecond.SelectedValue = end.Second.ToString("00");

                ViewState["sortBy"] = "";
                ViewState[SORT_DIRECTION] = "";
                BindGrid(POS.GetIntValue(), Outlet);
            }
            else
            {
                txtStartDate.Text = DateTime.Today.ToString("dd MMM yyyy");
                txtEndDate.Text = DateTime.Today.ToString("dd MMM yyyy");
                ddEndHour.SelectedValue = "23";
                ddEndMinute.SelectedValue = "59";
                ddEndSecond.SelectedValue = "59";

                ViewState["sortBy"] = "";
                ViewState[SORT_DIRECTION] = "";
                BindGrid();
            }
            
        }
        ShowGrossAmount = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Report.ShowGrossAmount), false);
    }

    private void SetFormSetting()
    {
        try
        {
            bool enableMallManagement = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.MallIntegration.UseMallManagement),false);
            string posText = LanguageManager.GetTranslation(LabelController.PointOfSaleText);
            string outletText = LanguageManager.GetTranslation(LabelController.OutletText);
            gvReport.Columns[POINTOFSALE].HeaderText = posText;
            gvReport.Columns[OUTLET].HeaderText = outletText;
            OutletDropdownList.SetLabelOutlet(outletText);
            OutletDropdownList.SetLabelPOS(posText);
            gvReport.Columns[5].Visible = enableMallManagement;
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
            if (ViewState["sortBy"] == null)
                ViewState["sortBy"] = "";
            String tmpStartDate, tmpEndDate;
            tmpStartDate = txtStartDate.Text + " " + ddStartHour.SelectedValue + ":" + ddStartMinute.SelectedItem + ":" + ddStartSecond.SelectedItem;
            tmpEndDate = txtEndDate.Text + " " + ddEndHour.SelectedValue + ":" + ddEndMinute.SelectedItem + ":" + ddEndSecond.SelectedItem;

            DateTime startDate = DateTime.Parse(tmpStartDate);
            DateTime endDate = DateTime.Parse(tmpEndDate);
            DataTable dt = ReportController.FetchTransactoionReport(
                startDate, endDate, cbUseStartDate.Checked, cbUseEndDate.Checked,
                txtOrderNo.Text, txtCashier.Text, (OutletDropdownList.GetDdlPOSSelectedValue+"").GetIntValue(), 
                OutletDropdownList.GetDdlOutletSelectedValue, txtPaymentMode.Text, txtRemark.Text, 
                chkShowVoidedTransaction.Checked);

            dt = UtilityController.RoundToTwoDecimalDigits(dt);

            gvReport.DataSource = dt;
            gvReport.DataBind();

            gvReport.HeaderRow.Cells[GROSSAMOUNT].Visible = ShowGrossAmount;
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
        }
    }

    private void BindGrid(int PointOfSaleID, string OutletName)
    {
        try
        {
            if (ViewState["sortBy"] == null)
                ViewState["sortBy"] = "";
            String tmpStartDate, tmpEndDate;
            tmpStartDate = txtStartDate.Text + " " + ddStartHour.SelectedValue + ":" + ddStartMinute.SelectedItem + ":" + ddStartSecond.SelectedItem;
            tmpEndDate = txtEndDate.Text + " " + ddEndHour.SelectedValue + ":" + ddEndMinute.SelectedItem + ":" + ddEndSecond.SelectedItem;

            DateTime startDate = DateTime.Parse(tmpStartDate);
            DateTime endDate = DateTime.Parse(tmpEndDate);
            DataTable dt = ReportController.FetchTransactoionReport(
                startDate, endDate, cbUseStartDate.Checked, cbUseEndDate.Checked,
                txtOrderNo.Text, txtCashier.Text, PointOfSaleID,
                OutletName, txtPaymentMode.Text, txtRemark.Text,
                chkShowVoidedTransaction.Checked);

            dt = UtilityController.RoundToTwoDecimalDigits(dt);

            gvReport.DataSource = dt;
            gvReport.DataBind();
            gvReport.HeaderRow.Cells[GROSSAMOUNT].Visible = ShowGrossAmount;
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
        }
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

            if (e.Row.Cells[GST].Text == "&nbsp;")
                e.Row.Cells[GST].Text = "0";
            e.Row.Cells[GST].Text = String.Format("{0:N2}", Decimal.Parse(e.Row.Cells[GST].Text));
            
            if (e.Row.Cells[AMOUNT_BEFORE_GST].Text == "&nbsp;")
                e.Row.Cells[AMOUNT_BEFORE_GST].Text = "0";
            e.Row.Cells[AMOUNT_BEFORE_GST].Text = String.Format("{0:N2}", Decimal.Parse(e.Row.Cells[AMOUNT_BEFORE_GST].Text));
            
            e.Row.Cells[DISCOUNT].Text = String.Format("{0:N2}", Decimal.Parse(e.Row.Cells[DISCOUNT].Text));


            if (e.Row.Cells[GROSSAMOUNT].Text == "&nbsp;")
                e.Row.Cells[GROSSAMOUNT].Text = "0";
            e.Row.Cells[GROSSAMOUNT].Text = String.Format("{0:N2}", Decimal.Parse(e.Row.Cells[GROSSAMOUNT].Text));

            if (e.Row.Cells[ISVOIDED].Text.ToLower() == "true")
            {
                e.Row.Cells[ISVOIDED].Text = "Yes";
                e.Row.ForeColor = System.Drawing.Color.White;
                e.Row.BackColor = System.Drawing.Color.DarkRed;
            }
            else
            {
                e.Row.Cells[ISVOIDED].Text = "No";
            }

            e.Row.Cells[GROSSAMOUNT].Visible = ShowGrossAmount;
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
