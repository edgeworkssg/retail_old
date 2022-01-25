using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using PowerPOS.Container;
using PowerPOS;
using SubSonic;
using SubSonic.Utilities;

public partial class SalesReportDetailed : System.Web.UI.Page
{
    private const int AMOUNT = 14;
    private const int QTY = 15;
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

        string SQLSyntax =
            "SELECT OrderDet.OrderDetDate, OrderHdr.OrderRefNo, Item.CategoryName, Item.ItemName"+
                ", ISNULL(Attributes1,'') Attributes1, ISNULL(Attributes2,'') Attributes2" +
                ", ISNULL(Attributes3,'') Attributes3, ISNULL(Attributes4,'') Attributes4" +
                ", ISNULL(Attributes5,'') Attributes5, ISNULL(Attributes6,'') Attributes6" +
                ", ISNULL(Attributes7,'') Attributes7, ISNULL(Attributes8,'') Attributes8" +
                ", OrderDet.Amount, " + 
                "isnull(Nullif(OrderDet.userfld1,''),SalesCommissionRecord.salespersonid) as SalesPersonID, OrderDet.Quantity, " +
                "PointOfSale.PointOfSaleName, OrderHdr.NettAmount, OrderHdr.IsVoided, " +
                "OrderDet.IsVoided AS IsLineVoided, PointOfSale.OutletName, PointOfSale.DepartmentID, PointOfSale.PointOfSaleID, " +
                "OrderDet.InventoryHdrRefNo, orderdet.orderdetid " +
                ", PaymentType " +
            "FROM OrderDet INNER JOIN " +
                "OrderHdr ON OrderDet.OrderHdrID = OrderHdr.OrderHdrID INNER JOIN " +
                "( "+
                    "SELECT ReceiptHdrID, CASE WHEN COUNT(*) > 1 THEN MIN(PaymentType) + ' (COMBINED)' ELSE MAX(PaymentType) END AS PaymentType "+
                    "FROM ReceiptDet GROUP BY ReceiptHdrID "+
                ") PYMT ON OrderHdr.OrderHdrID = PYMT.ReceiptHdrID INNER JOIN "+
                "PointOfSale ON OrderHdr.PointOfSaleID = PointOfSale.PointOfSaleID INNER JOIN " +
                "Item ON OrderDet.ItemNo = Item.ItemNo INNER JOIN " +
                "SalesCommissionRecord ON OrderHdr.OrderHdrID = SalesCommissionRecord.OrderHdrID ";

        #region *) Order & Filter
        System.Collections.Generic.List<string> Filters = new System.Collections.Generic.List<string>();
        if (cbUseStartDate.Checked && cbUseEndDate.Checked)
        {
            Filters.Add("OrderDet.OrderDetDate BETWEEN @StartDate AND @EndDate");
        }
        else if (cbUseStartDate.Checked)
        {
            Filters.Add("OrderDet.OrderDetDate >= @StartDate");
        }
        else if (cbUseEndDate.Checked)
        {
            Filters.Add("OrderDet.OrderDetDate <= @EndDate");
        }

        if (txtItemName.Text.Trim() != "")
        {
            Filters.Add("Item.ItemName LIKE '%' + @ItemName + '%'");
        }
        if (ddCategory.SelectedValue != "")
        {
            Filters.Add("Item.CategoryName LIKE @CategoryName");
        }

        if (selectedOutlet != "")
        {
            Filters.Add("PointOfSale.OutletName LIKE @OutletName");
        }
        if (ddDept.SelectedValue.ToString() != "0")
        {
            Filters.Add("PointOfSale.DepartmentID LIKE @DepartmentID");
        }

        if (SelectedPOS != "")
        {
            Filters.Add("PointOfSale.PointOfSaleName LIKE @PointOfSaleName");
        }

        if (txtCashier.Text.Trim() != "")
        {
            Filters.Add("isnull(Nullif(OrderDet.userfld1,''),SalesCommissionRecord.salespersonid) LIKE @SalesPersonID");
        }
        //SubSonic.TableSchema.TableColumn t = ViewTransactionDetail.Schema.GetColumn(SortColumn);

        //if (t != null)
        //{
        //    if (SortDir.Trim() == "ASC")
        //    {
        //        myViewTransactionDetail.OrderByAsc(SortColumn);
        //    }
        //    else if (SortDir.Trim() == "DESC")
        //    {
        //        myViewTransactionDetail.OrderByDesc(SortColumn);
        //    }
        //    else
        //    {
        //        myViewTransactionDetail.OrderByDesc("OrderDetDate");
        //    }
        //}
        //else
        //{
        //    myViewTransactionDetail.OrderByDesc("OrderDetDate");
        //}
        #endregion

        SQLSyntax = SQLSyntax + " WHERE " + string.Join(" AND ", Filters.ToArray());
        if (ViewState["sortBy"].ToString().Trim() == "")
            SQLSyntax = SQLSyntax + " ORDER BY OrderDetDate ASC ";
        else
            SQLSyntax = SQLSyntax + " ORDER BY " + ViewState["sortBy"].ToString().Trim() + " " + ViewState[SORT_DIRECTION].ToString().Trim();

        QueryCommand Cmd = new QueryCommand(SQLSyntax);

        #region *) Add Filter Parameter
        if (cbUseStartDate.Checked && cbUseEndDate.Checked)
        {
            Cmd.AddParameter("@StartDate", startDate, DbType.DateTime);
            Cmd.AddParameter("@EndDate", endDate, DbType.DateTime);
        }
        else if (cbUseStartDate.Checked)
        {
            Cmd.AddParameter("@StartDate", startDate, DbType.DateTime);
        }
        else if (cbUseEndDate.Checked)
        {
            Cmd.AddParameter("@EndDate", endDate, DbType.DateTime);
        }

        if (txtItemName.Text.Trim() != "")
        {
            Cmd.AddParameter("@ItemName", txtItemName.Text.Trim(), DbType.String);
        }
        if (ddCategory.SelectedValue != "")
        {
            Cmd.AddParameter("@CategoryName", ddCategory.SelectedValue, DbType.String);
        }

        if (selectedOutlet != "")
        {
            Cmd.AddParameter("@OutletName", selectedOutlet, DbType.String);
        }
        if (ddDept.SelectedValue.ToString() != "0")
        {
            Cmd.AddParameter("@DepartmentID", ddDept.SelectedValue.ToString(), DbType.String);
        }

        if (SelectedPOS != "")
        {
            Cmd.AddParameter("@PointOfSaleName", SelectedPOS, DbType.String);
        }

        if (txtCashier.Text.Trim() != "")
        {
            Cmd.AddParameter("@SalesPersonID", txtCashier.Text.Trim(), DbType.String);
        }
        #endregion

        AttributesLabelCollection Labels = new AttributesLabelCollection();
        int AttributesColumn = 5; /* Attributes column started from index 4 - 11 */
        
        for (int Counter = 6; Counter <= 13; Counter++)
            gvReport.Columns[Counter].Visible = false;

        Labels.Load();

        for (int Counter = 0; Counter < Labels.Count; Counter++)
        {
            gvReport.Columns[AttributesColumn + Labels[Counter].AttributesNo].Visible = true;
            gvReport.Columns[AttributesColumn + Labels[Counter].AttributesNo].HeaderText = Labels[Counter].Label;
        }

        DataTable dt = new DataTable();
        Cmd.CommandTimeout = 3000; //extend command time out
        dt.Load(DataService.GetReader(Cmd));

        gvReport.DataSource = dt;
        gvReport.DataBind();

        //DataTable dt = ReportController.FetchTransactionDetailReport(
        //    cbUseStartDate.Checked, cbUseStartDate.Checked,
        //    startDate, endDate, txtItemName.Text, ddCategory.SelectedValue,
        //    0, SelectedPOS, selectedOutlet, ddDept.SelectedValue.ToString(),
        //    ViewState["sortBy"].ToString(), ViewState[SORT_DIRECTION].ToString());
        //gvReport.DataSource = dt;
        //gvReport.DataBind();
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
    private decimal TotalAmount; private int TotalQty;
    protected void gvReport_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CommonWebUILib.SetVideoBrowserClientScript(e, 2, int.Parse(((DataTable)gvReport.DataSource).Rows[e.Row.RowIndex + gvReport.PageIndex * gvReport.PageSize]["PointOfSaleID"].ToString()));
            e.Row.Cells[AMOUNT].Text = String.Format("{0:N2}", Decimal.Parse(e.Row.Cells[AMOUNT].Text));
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            calculateSum();
            e.Row.Cells[AMOUNT].Text =  TotalAmount.ToString("N2");
            e.Row.Cells[QTY].Text = TotalQty.ToString();
        }
    }
    public void calculateSum()
    {
        TotalAmount = 0;
        TotalQty = 0;

        DataTable dt = (DataTable)gvReport.DataSource;
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            TotalAmount += decimal.Parse(dt.Rows[i]["Amount"].ToString());
            TotalQty += int.Parse(dt.Rows[i]["Quantity"].ToString());


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
        ls.Insert(0, new ListItem("ALL", ""));
        SubSonic.Utilities.Utility.LoadDropDown(ddPOS, ls, "Text", "Value", "");
    }
}
