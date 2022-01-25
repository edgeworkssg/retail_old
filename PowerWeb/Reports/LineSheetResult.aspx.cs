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


public partial class LineSheetResult : PageBase
{
    
    private const string SORT_DIRECTION = "SORT_DIRECTION";
    private const string ORDER_BY = "ORDER_BY";

    private const int Col_ItemNo = 6;
    private const int Col_Qty = 8;
    private const int Col_OrderDetID = 1;
    private const int Col_Status = 12;
    private const int Col_Outstanding = 9;

    private DataTable dtLineSheet ,dtResult;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            ViewState["sortBy"] = "";
            ViewState[SORT_DIRECTION] = "";
            
            BindGrid();
        }
        
    }

    private void ConvertToResult(DataTable dt)
    {
        dt.DefaultView.Sort = "CategoryName ASC";
        dtResult = new DataTable();
        dtResult.Columns.Add("ItemNo");
        dtResult.Columns.Add("ItemName");
        dtResult.Columns.Add("CategoryName");
        dtResult.Columns.Add("RetailPrice", typeof(Decimal));
        dtResult.Columns.Add("FactoryPrice", typeof(Decimal));
        dtResult.Columns.Add("Attributes1");
        dtResult.Columns.Add("Attributes2");
        dtResult.Columns.Add("Attributes3");
        dtResult.Columns.Add("Attributes4");

        string category = "";
        foreach (DataRow dr in dt.Rows)
        {
            if (category == "")
            {
                DataRow drCat = dtResult.NewRow();
                drCat["ItemNo"] = "";
                drCat["ItemName"] = dr["CategoryName"];
                drCat["Attributes4"] = "P";
                dtResult.Rows.Add(drCat);
                category = dr["CategoryName"].ToString();
            }
            else
            {
                if (category != dr["CategoryName"].ToString())
                {
                    DataRow drCat = dtResult.NewRow();
                    drCat["ItemNo"] = "";
                    drCat["ItemName"] = dr["CategoryName"];
                    
                    drCat["Attributes4"] = "P";
                    dtResult.Rows.Add(drCat);
                    category = dr["CategoryName"].ToString() ;
                }
            }

            DataRow drProd = dtResult.NewRow();
            drProd["ItemNo"] = dr["ItemNo"];
            drProd["ItemName"] = dr["ItemName"];
            drProd["RetailPrice"] = dr["RetailPrice"];
            drProd["FactoryPrice"] = dr["CostPrice"];
            drProd["Attributes1"] = dr["Attributes1"].ToString() == "&nbsp;" ? "" : dr["Attributes1"];
            drProd["Attributes2"] = dr["Attributes2"].ToString() == "&nbsp;" ? "" : dr["Attributes2"];
            drProd["Attributes3"] = dr["Attributes3"].ToString() == "&nbsp;" ? "" : dr["Attributes3"];
            drProd["Attributes4"] = dr["Attributes4"].ToString() == "&nbsp;" ? "" : dr["Attributes4"];
            dtResult.Rows.Add(drProd);


        }
        
        //return dtResult;
    }

    private void BindGrid()
    {
        if (Session["LineData"] != null && Session["LineData"] is DataTable)
        {
            dtLineSheet = (DataTable)Session["LineData"];
            ConvertToResult(dtLineSheet);
        }

        if (ViewState["sortBy"] == null)
        {
            ViewState["sortBy"] = "";
        }

        try
        {
            
            //dt = DeliveryController.FetchDeliveryListNew(txtRefNo.Text, pos.PointOfSaleID, startDate, endDate, txtSearch.Text, ProjectName, CorporateName, Status);


            if (dtResult != null && dtResult.Rows.Count > 0)
            {
                gvReport.DataSource = dtResult;
            }
            else
            {
                gvReport.DataSource = null;
            }
            gvReport.DataBind();
        }
        catch (Exception X)
        {
            Logger.writeLog(X);
            //lblResult.Text = "<span style=\"font-weight:bold; color:#22bb22\">Customer saved.</span>";
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
            decimal amount;
            if (decimal.TryParse(e.Row.Cells[7].Text, out amount))
            {
                e.Row.Cells[7].Text = String.Format("{0:N2}", amount);
            }
            if (decimal.TryParse(e.Row.Cells[6].Text, out amount))
            {
                e.Row.Cells[6].Text = String.Format("{0:N2}", amount);
            }
        }
    }
    protected void ddPOS_Init(object sender, EventArgs e)
    {
        /*
            cmbPOS.DataSource = (new PointOfSaleCollection()).Load();
            cmbPOS.ValueMember = PointOfSale.Columns.PointOfSaleID;
            cmbPOS.DisplayMember = PointOfSale.Columns.PointOfSaleName;
        */

        
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        ViewState["sortBy"] = null;
        
        BindGrid();
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {  
            

            BindGrid();
    }



    protected void lnkExport_Click(object sender, EventArgs e)
    {
        BindGrid();
        DataTable dt = (DataTable)gvReport.DataSource;
        CommonWebUILib.ExportCSV(dt, this.Page.Title.Trim(' '),this.Page.Title,gvReport);
    }
    protected void lnkSelectAll_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow row in gvReport.Rows)
        {
            decimal outstanding = 0;
            Decimal.TryParse(row.Cells[Col_Outstanding].Text, out outstanding);
            if (outstanding > 0)
            {
                ((CheckBox)row.FindControl("CheckBox1")).Checked = true;
            }
        }
    }
    protected void lnkSelectNone_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow row in gvReport.Rows)
            ((CheckBox)row.FindControl("CheckBox1")).Checked = false;
    }

    protected void btnCloseHelp_Click(object sender, EventArgs e)
    {
        BindGrid();
        DataTable dt = (DataTable)gvReport.DataSource;
        CommonWebUILib.ExportCSVAllColumns(dt, this.Page.Title.Trim(' '), this.Page.Title);
    }

    protected void txtStartDate_TextChanged(object sender, EventArgs e)
    {

    }

    protected void btnSend_Click(object sender, EventArgs e)
    {
        if (dtLineSheet != null && dtLineSheet.Rows.Count > 0)
        {

        }
        

    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        
    }

    protected void btnImport_Click(object sender, EventArgs e)
    {
        //Response.Redirect("SalesImporter.aspx");
    }   
}
