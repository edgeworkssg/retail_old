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

public partial class ProductSalesReportWithProfit : PageBase
{
    private const int QTY = 9;
    private const int AMOUNT_WITH_GST = 10;
    private const int GSTAMOUNT = 11;
    private const int AMOUNT = 13;
    private const int COGS = 14;
    private const int PL = 14;
    private const int PLPercent = 15;
    private const int ATTRIBUTES1 = 4;
    private const int ATTRIBUTES2 = 5;
    private const int ATTRIBUTES3 = 6;
    private const int ATTRIBUTES4 = 7;
    private const int ATTRIBUTES5 = 8;

    private const string SORT_DIRECTION = "SORT_DIRECTION";
    private const string ORDER_BY = "ORDER_BY";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            lblYear.Text = DateTime.Today.Year.ToString();
            ddlMonth.SelectedValue = DateTime.Today.Month.ToString();
            ddlOutlet.Items.AddRange(PointOfSaleController.FetchOutletNames(true));
            ddlOutlet.DataBind();
            ddCategory.Items.AddRange(ItemController.FetchCategoryNamesInListItem());
            ddCategory.DataBind();
            txtStartDate.Text = DateTime.Today.AddDays(-1).ToString("dd MMM yyyy");
            txtEndDate.Text = DateTime.Today.AddDays(-1).ToString("dd MMM yyyy");
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
        DateTime startDate = DateTime.Parse(txtStartDate.Text);
        DateTime endDate = DateTime.Parse(txtEndDate.Text);

        //InventoryController

        DataTable dt =
            ReportController.FetchProductSalesReport(
            startDate, endDate.AddSeconds(86399),
            txtItemName.Text, ddPOS.SelectedItem.Text, ddlOutlet.SelectedValue.ToString(),
             ddCategory.SelectedValue.ToString(), ddDept.SelectedValue.ToString(), false,
            ViewState["sortBy"].ToString(), ViewState[SORT_DIRECTION].ToString());
        gvReport.DataSource = dt;
        gvReport.DataBind();

        for (int i = ATTRIBUTES1; i <= ATTRIBUTES5; i++)
        {
            string strd = gvReport.Columns[i].HeaderText;
            int id = 0;

            switch (strd)
            {
                case "Attributes1": id = 1;
                    break;
                case "Attributes2": id = 2;
                    break;
                case "Attributes3": id = 3;
                    break;
                case "Attributes4": id = 4;
                    break;
                case "Attributes5": id = 5;
                    break;
            }

            if (id != 0)
            {
                String MyQuery = "select Label from attributeslabel where AttributesNo=" + id.ToString() ;
                QueryCommand Qcmd = new QueryCommand(MyQuery);
                string str = Convert.ToString(DataService.ExecuteScalar(Qcmd).ToString());
                gvReport.Columns[i].HeaderText = str;
            }
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
            //e.Row.Cells[AMOUNT_WITH_GST].Text = String.Format("{0:N2}", Decimal.Parse(e.Row.Cells[AMOUNT_WITH_GST].Text));
            //e.Row.Cells[GSTAMOUNT].Text = String.Format("{0:N2}", Decimal.Parse(e.Row.Cells[GSTAMOUNT].Text));
            //e.Row.Cells[AMOUNT].Text = String.Format("{0:N2}", Decimal.Parse(e.Row.Cells[AMOUNT].Text));
            //e.Row.Cells[COGS].Text = String.Format("{0:N2}", Decimal.Parse(e.Row.Cells[COGS].Text));

            //decimal tmp = Decimal.Parse(e.Row.Cells[PLPercent].Text);
            //if (tmp >= -100 && tmp <= 100)
            //{
            //    e.Row.Cells[PLPercent].Text = tmp.ToString("N2") + "%";
            //}
            //else
            //{
            //    e.Row.Cells[PLPercent].Text = "ERR";
            //}
            //e.Row.Cells[PL].Text = String.Format("{0:N2}", Decimal.Parse(e.Row.Cells[PL].Text));

        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            DataTable dt = (DataTable)gvReport.DataSource;
            if (dt != null)
            {
                decimal Amount, PLAmt, AmountWithoutGST, GSTAmount;
                e.Row.Cells[QTY].Text = dt.Compute("SUM(TOTALQuantity)", "").ToString();
                Amount = decimal.Parse(dt.Compute("SUM(TOTALAMOUNT)", "").ToString());
                e.Row.Cells[AMOUNT_WITH_GST].Text = Amount.ToString("N2");
                AmountWithoutGST = decimal.Parse(dt.Compute("SUM(TOTALAMOUNTWITHOUTGST)", "").ToString());
                //e.Row.Cells[AMOUNT].Text = AmountWithoutGST.ToString("N2");
                GSTAmount = decimal.Parse(dt.Compute("SUM(GSTAMOUNT)", "").ToString());
                e.Row.Cells[GSTAMOUNT].Text = GSTAmount.ToString("N2");
                e.Row.Cells[COGS].Text = decimal.Parse(dt.Compute("SUM(TotalCOSTOFGOODSSOLD)", "").ToString()).ToString("N2");

                PLAmt = decimal.Parse(dt.Compute("SUM(PROFITLOSS)", "").ToString());
                e.Row.Cells[PL].Text = PLAmt.ToString("N2");
                e.Row.Cells[PLPercent].Text = (PLAmt / (Amount + (decimal)0.0001) * 100).ToString("N2") + "%";

            }
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        ViewState["sortBy"] = null;

        if (rdbMonth.Checked)
        {
            txtStartDate.Text = (new DateTime(int.Parse(lblYear.Text), int.Parse(ddlMonth.SelectedValue), 1)).ToString("dd MMM yyyy");
            txtEndDate.Text = new DateTime(int.Parse(lblYear.Text), int.Parse(ddlMonth.SelectedValue), DateTime.DaysInMonth(int.Parse(lblYear.Text), int.Parse(ddlMonth.SelectedValue))).ToString("dd MMM yyyy");
        }

        BindGrid();
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {

        ddlOutlet.SelectedIndex = 0;
        txtStartDate.Text = DateTime.Today.ToString("dd MMM yyyy");
        txtEndDate.Text = DateTime.Today.ToString("dd MMM yyyy");
        txtItemName.Text = "";

        gvReport.PageIndex = 0;
    }
    protected void lnkExport_Click(object sender, EventArgs e)
    {
        BindGrid();
        DataTable dt = (DataTable)gvReport.DataSource;
        CommonWebUILib.ExportCSV(dt, this.Page.Title.Trim(' '), this.Page.Title, gvReport);
    }
    protected void ddDept_Init(object sender, EventArgs e)
    {

    }
    protected void ddPOS_Init(object sender, EventArgs e)
    {

    }
}
