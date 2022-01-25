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

public partial class TransactionDetailWithCostReport : PageBase
{
    
    private const int RETAIL_PRICE = 13;
    private const int COST_PRICE = 14;
    private const int QTY = 15;
    private const int AMOUNT = 16;
    private const int GST = 17;
    private const int AMOUNT_BEFORE_GST = 18;
    private const int DISCOUNT = 19;
    private const int DISC_PERCENTAGE = 20;
    private const int AVERAGECOST = 21;
    private const int ATTRIBUTES1 = 5;
    private const int ATTRIBUTES2 = 6;
    private const int ATTRIBUTES3 = 7;
    private const int ATTRIBUTES4 = 8;
    private const int ATTRIBUTES5 = 9;

    private const string SORT_DIRECTION = "SORT_DIRECTION";
    private const string ORDER_BY = "ORDER_BY";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            ddCategory.Items.AddRange(ItemController.FetchCategoryNamesInListItem());
            ddCategory.DataBind();            
            ddlOutlet.Items.AddRange(PointOfSaleController.FetchOutletNames(false));
            ddlOutlet.DataBind();
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
        string selectedOutlet = ddlOutlet.SelectedItem.Text;
        if (selectedOutlet == "ALL") selectedOutlet = "";
        string SelectedPOS = ddPOS.SelectedItem.Text;
        if (SelectedPOS == "ALL")
        {
            SelectedPOS = "%";
        }
        String tmpStartDate, tmpEndDate;
        tmpStartDate = txtStartDate.Text + " " + ddStartHour.SelectedValue + ":" + ddStartMinute.SelectedItem;
        tmpEndDate = txtEndDate.Text + " " + ddEndHour.SelectedValue + ":" + ddEndMinute.SelectedItem;

        DateTime startDate = DateTime.Parse(tmpStartDate);
        DateTime endDate = DateTime.Parse(tmpEndDate);

        DataTable dt = ReportController.FetchTransactionDetailReportForWeb(
            cbUseStartDate.Checked, cbUseStartDate.Checked,
            startDate, endDate, txtItemName.Text,ddCategory.SelectedValue,             
            0,SelectedPOS, selectedOutlet, ddDept.SelectedValue.ToString(),
            ViewState["sortBy"].ToString(), ViewState[SORT_DIRECTION].ToString());

        
        gvReport.DataSource = dt;
        gvReport.DataBind();

        //for (int i = ATTRIBUTES1; i <= ATTRIBUTES5; i++)
        //{
        //    string strd = gvReport.Columns[i].HeaderText;
        //    int id = 0;

        //    switch (strd)
        //    {
        //        case "Attributes1": id = 1;
        //            break;
        //        case "Attributes2": id = 2;
        //            break;
        //        case "Attributes3": id = 3;
        //            break;
        //        case "Attributes4": id = 4;
        //            break;
        //        case "Attributes5": id = 5;
        //            break;
        //    }

        //    if (id != 0)
        //    {
        //        String MyQuery = "select Label from attributeslabel where AttributesNo=" + id.ToString();
        //        QueryCommand Qcmd = new QueryCommand(MyQuery);
        //        string str = Convert.ToString(DataService.ExecuteScalar(Qcmd).ToString());
        //        gvReport.Columns[i].HeaderText = str;
        //    }
        //}
    }

    private DataTable RemoveAllVoided(DataTable dt)
    {
        throw new NotImplementedException();
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

    private decimal TotalAmount;
    private int TotalQty;
    private decimal TotalDiscount;
    private decimal TotalGST;
    private decimal TotalAmountBeforeGST;
    protected void gvReport_RowDataBound(object sender, GridViewRowEventArgs e)
    {
       
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //CommonWebUILib.SetVideoBrowserClientScript(e, 2, int.Parse(((DataTable)gvReport.DataSource).Rows[e.Row.RowIndex+gvReport.PageIndex * gvReport.PageSize]["PointOfSaleID"].ToString()));
            e.Row.Cells[AMOUNT].Text = String.Format("{0:N2}", Decimal.Parse(e.Row.Cells[AMOUNT].Text));
            e.Row.Cells[RETAIL_PRICE].Text = String.Format("{0:N2}", Decimal.Parse(e.Row.Cells[RETAIL_PRICE].Text));
            e.Row.Cells[COST_PRICE].Text = String.Format("{0:N2}", Decimal.Parse(e.Row.Cells[COST_PRICE].Text));
            e.Row.Cells[AVERAGECOST].Text = String.Format("{0:N2}", Decimal.Parse(e.Row.Cells[AVERAGECOST].Text));
            e.Row.Cells[GST].Text = String.Format("{0:N2}", Decimal.Parse(e.Row.Cells[GST].Text));
            e.Row.Cells[AMOUNT_BEFORE_GST].Text = String.Format("{0:N2}", Decimal.Parse(e.Row.Cells[AMOUNT_BEFORE_GST].Text));
            e.Row.Cells[DISCOUNT].Text = String.Format("{0:N2}", Decimal.Parse(e.Row.Cells[DISCOUNT].Text));
            e.Row.Cells[DISC_PERCENTAGE].Text = String.Format("{0:N}", Decimal.Parse(e.Row.Cells[DISC_PERCENTAGE].Text)); 
        
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            calculateSum();
            e.Row.Cells[AMOUNT].Text =  TotalAmount.ToString("N2");
            e.Row.Cells[QTY].Text = TotalQty.ToString();
            e.Row.Cells[DISCOUNT].Text =  TotalDiscount.ToString("N2");
            e.Row.Cells[GST].Text =  TotalGST.ToString("N2");
            e.Row.Cells[AMOUNT_BEFORE_GST].Text =  TotalAmountBeforeGST.ToString("N2");
        }
    }
    public void calculateSum()
    {
        TotalAmount = 0;
        TotalQty = 0;
        TotalDiscount = 0;
        TotalAmountBeforeGST = 0;
        TotalGST = 0;

        DataTable dt = (DataTable)gvReport.DataSource;
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            TotalAmount += decimal.Parse(dt.Rows[i]["Amount"].ToString());
            TotalQty += int.Parse(dt.Rows[i]["Quantity"].ToString());
            TotalDiscount += decimal.Parse(dt.Rows[i]["Disc"].ToString());
            TotalGST += decimal.Parse(dt.Rows[i]["GSTAmountPerItem"].ToString());
            TotalAmountBeforeGST += decimal.Parse(dt.Rows[i]["AmountBefGST"].ToString());
            
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
        ddlOutlet.SelectedIndex = 0;
        txtStartDate.Text = DateTime.Today.ToString("dd MMM yyyy");
        txtEndDate.Text = DateTime.Today.ToString("dd MMM yyyy");
        txtCashier.Text = "";
        txtItemName.Text = "";
        //txtPointOfSale.Text = "";
        gvReport.PageIndex = 0;
    }
    protected void lnkExport_Click(object sender, EventArgs e)
    {
        BindGrid();
        DataTable dt = (DataTable)gvReport.DataSource;
        CommonWebUILib.ExportCSV(dt, this.Page.Title.Replace(' ', '_'), this.Page.Title, gvReport);           
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
        if (Session["DeptID"] != null && Session["DeptID"].ToString() != "0" & ddDept.SelectedValue == "0")
        {
            ddPOS.WhereField = "DepartmentID";
            ddPOS.WhereValue = Session["DeptID"].ToString();

        }
    }
    
    protected void ddDept_SelectedIndexChanged(object sender, EventArgs e)
    {        
            ddPOS.Items.Clear();
            IDataReader rdr;
            
            Query qry = new Query("PointOfSale");
            qry.QueryType = QueryType.Select;
        
            if (ddDept.SelectedValue != "0")
            {
                qry.AddWhere(PointOfSale.Columns.DepartmentID, ddDept.SelectedValue.ToString());
            }
            if (ddlOutlet.SelectedValue != "")
            {                
                qry.AddWhere(PointOfSale.Columns.OutletName, 
                    Comparison.Like, ddlOutlet.SelectedValue.ToString());
            }
            rdr = qry.ExecuteReader();

            ListItemCollection ls = new ListItemCollection();
            SubSonic.Utilities.Utility.LoadListItems(ls, rdr, "PointOfSaleName", "PointOfSaleID", "", true);            
            ls.Insert(0, new ListItem("ALL",""));                        
            SubSonic.Utilities.Utility.LoadDropDown(ddPOS, ls, "Text", "Value", "");        
    }

}
