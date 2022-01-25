using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
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
using System.Linq;

public partial class ProductSalesReport : PageBase
{
    private const int QTY = 5;
    private const int AMOUNT_WITH_GST = 6;
    private const int GSTAMOUNT = 7;
    private const int AMOUNT = 8;
    private const int COGS = 9;
    private const int PL = 10;
    private const int PLPercent = 11;

    private const string SORT_DIRECTION = "SORT_DIRECTION";
    private const string SORT_BY = "sortBy";
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
            ddCategory.Items.AddRange(ItemController.FetchCategoryNamesInListItem());
            ddCategory.DataBind();

            for (int i = 0; i < 60; i++)
            {
                ddStartMinute.Items.Add(new ListItem(i.ToString("00")));
                ddStartSecond.Items.Add(new ListItem(i.ToString("00")));
                ddEndMinute.Items.Add(new ListItem(i.ToString("00")));
                ddEndSecond.Items.Add(new ListItem(i.ToString("00")));
            }

            txtStartDate.Text = DateTime.Today.AddDays(0).ToString("dd MMM yyyy");
            txtEndDate.Text = DateTime.Today.AddDays(0).ToString("dd MMM yyyy");
            ddEndHour.SelectedValue = "23";
            ddEndMinute.SelectedValue = "59";
            ddEndSecond.SelectedValue = "59";

            ViewState[SORT_BY] = "";
            ViewState[SORT_DIRECTION] = "";
            LoadDataSource();

            #region *) Display: Set Attributes Columns
            Dictionary<int, string> Attributes = AttributesLabelController.GetAttributesLabel();
            int StartOfAttributesColumn = 7;
            for (int Counter = 1; Counter <= AttributesLabelController.MaxAttributes; Counter++)
            {
                gvReport.Columns[StartOfAttributesColumn + Counter - 1].Visible = false;
                if (Attributes.ContainsKey(Counter))
                {
                    gvReport.Columns[StartOfAttributesColumn + Counter - 1].Visible = true;
                    gvReport.Columns[StartOfAttributesColumn + Counter - 1].HeaderText = Attributes[Counter];

                }
            }
            #endregion

            BindGrid();
        }
    }

    private void LoadDataSource()
    {
//        string strSql = @"
//            select * 
//              from AttributesLabel 
//             order by Label asc
//        ";
//        QueryCommand cmd = new QueryCommand(strSql);
//        DataTable dsAttributes = DataService.GetDataSet(cmd).Tables[0];
//        chkAttributes.Items.Clear();
//        List<KeyValuePair<int, string>> chkAttributesRawData = new List<KeyValuePair<int, string>>();
//        foreach (DataRow row in dsAttributes.Rows)
//        {
//            chkAttributesRawData.Add(new KeyValuePair<int, string>(int.Parse(row["AttributesNo"].ToString()), row["Label"].ToString()));
//        }
//        chkAttributes.DataSource = chkAttributesRawData;
    }

    private void BindGrid()
    {
        try
        {
            if (ViewState[SORT_DIRECTION] == null)
                ViewState[SORT_DIRECTION] = "ASC";

            if (ViewState[SORT_BY] == null)
                ViewState[SORT_BY] = "ItemName";

            String tmpStartDate, tmpEndDate;
            if (cbUseStartDate.Checked)
                tmpStartDate = txtStartDate.Text + " " + ddStartHour.SelectedValue + ":" + ddStartMinute.SelectedItem + ":" + ddStartSecond.SelectedItem;
            else
                tmpStartDate = "2000-01-01 00:00:00";

            if (cbUseEndDate.Checked)
                tmpEndDate = txtEndDate.Text + " " + ddEndHour.SelectedValue + ":" + ddEndMinute.SelectedItem + ":" + ddEndSecond.SelectedItem;
            else
                tmpEndDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            DateTime startDate = DateTime.Parse(tmpStartDate);
            DateTime endDate = DateTime.Parse(tmpEndDate);

            //DataTable dt = 
            //    ReportController.FetchProductSalesReport(
            //    startDate, endDate.AddSeconds(86399),
            //    txtItemName.Text, OutletDropdownList.GetDdlPOSSelectedItemText, OutletDropdownList.GetDdlOutletSelectedValue.ToString(), 
            //     ddCategory.SelectedValue.ToString(), ddDept.SelectedValue.ToString(), (ddlSupplier.SelectedValue+"").GetIntValue(), false,
            //    ViewState["sortBy"].ToString(), ViewState[SORT_DIRECTION].ToString());        
            ReportController.RegenerateDWData(new DWType[] { DWType.ALL }, startDate, endDate, ddlOutlet.SelectedValue);

            string sql = @"
            EXEC [dbo].[REPORT_ProductSalesReport]
	            @CategoryName = @CategoryName_,
	            @StartDate = @StartDate_,
	            @EndDate = @EndDate_,
	            @OutletName = @OutletName_,
	            @Search = @Search_,
	            @SupplierID = @SupplierID_,
	            @ItemDepartmentID = @ItemDepartmentID_";
            QueryCommand cmd = new QueryCommand(sql);
            cmd.AddParameter("@CategoryName_", ddCategory.SelectedValue + "");
            cmd.AddParameter("@StartDate_", startDate.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.AddParameter("@EndDate_", endDate.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.AddParameter("@OutletName_", ddlOutlet.SelectedValue + "");
            cmd.AddParameter("@Search_", txtItemName.Text);
            cmd.AddParameter("@SupplierID_", (ddlSupplier.SelectedValue + "").GetIntValue());
            cmd.AddParameter("@ItemDepartmentID_", ddDept.SelectedValue + "");

            DataTable dt = new DataTable();
            dt.Load(DataService.GetReader(cmd));

            DataView dv = dt.DefaultView;
            dv.Sort = ViewState[SORT_BY] + " " + ViewState[SORT_DIRECTION];
            DataTable sortedDT = dv.ToTable();

            gvReport.DataSource = sortedDT;
            gvReport.DataBind();
        }
        catch (Exception exx)
        {
            Logger.writeLog(exx);
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
        ViewState[SORT_BY]  = e.SortExpression;
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
        if (ViewState[SORT_BY] != null)
        {
            string sortedBy = ViewState[SORT_BY].ToString();
            if (sortedBy == sortBy)
            {
                //the direction should be desc
                sortDir = " DESC";
                //reset the sorter to null
                ViewState[SORT_BY] = null;
            }

            else
            {
                //this is the first sort for this row
                //put it to the ViewState
                ViewState[SORT_BY] = sortBy;
            }

        }

        else
        {
            //it's null, so this is the first sort
            ViewState[SORT_BY] = sortBy;
        }

        return sortDir;
    }
    protected void gvReport_RowDataBound(object sender, GridViewRowEventArgs e)
    {        
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //e.Row.Cells[AMOUNT_WITH_GST].Text = String.Format("{0:N2}", Decimal.Parse(e.Row.Cells[AMOUNT_WITH_GST].Text));
            //e.Row.Cells[GSTAMOUNT].Text = String.Format("{0:N2}", Decimal.Parse(e.Row.Cells[GSTAMOUNT].Text));
            //e.Row.Cells[AMOUNT].Text = String.Format("{0:N2}", Decimal.Parse(e.Row.Cells[AMOUNT].Text));            
            //e.Row.Cells[COGS].Text = String.Format("{0:N2}", Decimal.Parse(e.Row.Cells[COGS].Text));
            /*
            decimal tmp = Decimal.Parse(e.Row.Cells[PLPercent].Text);
            if (tmp >= -100 && tmp <= 100)
            {
                e.Row.Cells[PLPercent].Text = tmp.ToString("N2") + "%";
            }
            else
            {
                e.Row.Cells[PLPercent].Text = "ERR";
            }
            e.Row.Cells[PL].Text = String.Format("{0:N2}", Decimal.Parse(e.Row.Cells[PL].Text));            
             */ 
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            DataTable dt = (DataTable) gvReport.DataSource;
            if (dt != null)
            {
                decimal Amount, PLAmt, AmountWithoutGST, GSTAmount;
                e.Row.Cells[QTY].Text = dt.Compute("SUM(TOTALQuantity)", "").ToString();

                Amount = decimal.Parse(dt.Compute("SUM(TOTALAMOUNT)", "").ToString());
                e.Row.Cells[AMOUNT_WITH_GST].Text = Amount.ToString("N2");
                //AmountWithoutGST = decimal.Parse(dt.Compute("SUM(TOTALAMOUNTWITHOUTGST)", "").ToString());
                //e.Row.Cells[AMOUNT].Text = AmountWithoutGST.ToString("N2");
                //GSTAmount = decimal.Parse(dt.Compute("SUM(GSTAMOUNT)", "").ToString());
                //e.Row.Cells[GSTAMOUNT].Text = GSTAmount.ToString("N2");
                //e.Row.Cells[COGS].Text = decimal.Parse(dt.Compute("SUM(TotalCOSTOFGOODSSOLD)", "").ToString()).ToString("N2");
                /*
                PLAmt = decimal.Parse(dt.Compute("SUM(PROFITLOSS)", "").ToString());
                e.Row.Cells[PL].Text = PLAmt.ToString("N2") + "%";
                e.Row.Cells[PLPercent].Text = (PLAmt / Amount * 100).ToString("N2") + "%";
                 * */                    
            }
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        ViewState["sortBy"] = null;
        
        if (rdbMonth.Checked)
        {            
            txtStartDate.Text = (new DateTime(int.Parse(ddlYear.Text), int.Parse(ddlMonth.SelectedValue), 1)).ToString("dd MMM yyyy");
            ddStartHour.SelectedValue = "00";
            ddStartMinute.SelectedValue = "00";
            ddStartSecond.SelectedValue = "00";

            txtEndDate.Text = new DateTime(int.Parse(ddlYear.Text), int.Parse(ddlMonth.SelectedValue), DateTime.DaysInMonth(int.Parse(ddlYear.Text), int.Parse(ddlMonth.SelectedValue))).ToString("dd MMM yyyy");
            ddEndHour.SelectedValue = "23";
            ddEndMinute.SelectedValue = "59";
            ddEndSecond.SelectedValue = "59";
        }

        BindGrid();
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        txtStartDate.Text = DateTime.Today.ToString("dd MMM yyyy");
        txtEndDate.Text = DateTime.Today.ToString("dd MMM yyyy");
        txtItemName.Text = "";

        gvReport.PageIndex = 0;
    }

    protected void lnkExport_Click(object sender, EventArgs e)
    {
        BindGrid();
        DataTable dt = (DataTable)gvReport.DataSource;
        CommonWebUILib.ExportCSV(dt, this.Page.Title.Trim(' '),this.Page.Title, gvReport);
    }

    protected void ddlSupplier_Init(object sender, EventArgs e)
    {
        try
        {
            var data = new SupplierController().FetchAll()
                                               .Where(o => o.Deleted.GetValueOrDefault(false) == false)
                                               .OrderBy(o => o.SupplierName)
                                               .ToList();
            data.Insert(0, new Supplier { SupplierID = 0, SupplierName = "ALL" });
            ddlSupplier.DataSource = data;
            ddlSupplier.DataBind();
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
        }
    }

    protected void ddlOutlet_Init(object sender, EventArgs e)
    {
        ddlOutlet.Items.Clear();
        ddlOutlet.Items.AddRange(PointOfSaleController.FetchOutletNames(true, Session["UserName"] + ""));
    }
}
