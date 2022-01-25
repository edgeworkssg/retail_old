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
using System.Linq;
using PowerPOS;
using SubSonic;
using SubSonic.Utilities;


public partial class InventorySummaryReport : PageBase
{    
    private const string SORT_DIRECTION = "SORT_DIRECTION";
    private const string ORDER_BY = "ORDER_BY";
    
    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (!Page.IsPostBack)
        {
            ViewState["Sort"] = "";
            BindGrid(ViewState["Sort"].ToString());
        }
    }

    

    private void BindGrid(string sort)
    {
        try
        {
            string sql = @"
            EXEC [dbo].[REPORT_InventorySummaryReport]
		    @ItemDepartmentID = @ItemDepartmentID_,
		    @Category = @Category_,
		    @SupplierID = @SupplierID_,
		    @Search = @Search_";

            QueryCommand cmd = new QueryCommand(sql);
            cmd.AddParameter("@ItemDepartmentID_", ddlDept.SelectedValue);
            cmd.AddParameter("@Category_", ddlCategory.SelectedValue);
            cmd.AddParameter("@SupplierID_", (ddlSupplier.SelectedValue + "").GetIntValue());
            cmd.AddParameter("@Search_", txtSearch.Text);
            DataTable dt = new DataTable();
            dt.Load(DataService.GetReader(cmd));

            gvReport.DataSource = LanguageManager.GetTranslation(dt, 0, 1, 2);
            gvReport.DataBind();
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
        }
    }

    protected void gvReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvReport.PageIndex = e.NewPageIndex;
        BindGrid(ViewState["Sort"].ToString());
    }

    protected void gvReport_DataBound(Object sender, EventArgs e)
    {

        //pull the datasource
        DataView ds = gvReport.DataSource as DataView;

        
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
            if (ds != null)
            {
                itemCount = ds.Table.Rows.Count;
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

    protected void ddlPages_SelectedIndexChanged(Object sender, EventArgs e)
    {
        GridViewRow gvrPager = gvReport.BottomPagerRow;
        DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages");
        gvReport.PageIndex = ddlPages.SelectedIndex;
        // a method to populate your grid
        BindGrid(ViewState["Sort"].ToString());
    }


    protected void lnkExport_Click(object sender, EventArgs e)
    {
        BindGrid(ViewState["Sort"].ToString());
        DataTable dt = (DataTable)gvReport.DataSource;
        
        //CommonWebUILib.ExportCSV(dt, this.Page.Title.Trim(' '));
        // Export the details of specified columns to Excel
        int[] column;
        column = new int[dt.Columns.Count];
        for (int i=0; i < column.Length;i++) 
        {
            column[i] = i;
        }
        string[] header;
        header = new string[dt.Columns.Count];
        //Work around for bug in the export to excel library
        for (int i = 0; i < header.Length; i++) 
        {
            header[i] = dt.Columns[i].ColumnName;
            dt.Columns[i].ColumnName= "col" + i.ToString();
        }

        RKLib.ExportData.Export objExport = new
            RKLib.ExportData.Export("Web");
        
        objExport.ExportDetails(dt,column,header,
             RKLib.ExportData.Export.ExportFormat.CSV, 
             this.Page.Title.Trim(' ') + DateTime.Now.ToString("ddMMMyyyy")
             + ".CSV");
    }
    protected void gvReport_Sorting(object sender, GridViewSortEventArgs e)
    {
        ViewState["Sort"] = e.SortExpression;        
        BindGrid(ViewState["Sort"].ToString());
    }
    protected void ddDept_Init(object sender, EventArgs e)
    {
        if (Session["DeptID"] != null && Session["DeptID"].ToString() != "0")
        {
            
        }
    }
    protected void ddDept_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindGrid(ViewState["Sort"].ToString());
    }

    protected void gvReport_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            
            for (int i = 6; i < e.Row.Cells.Count; i++)
            {
                e.Row.Cells[i].Text = string.Format("{0:0.####}", e.Row.Cells[i].Text.GetDecimalValue());
            }
        }

        if (e.Row.RowType == DataControlRowType.Footer)
        {
            DataTable source = (DataTable)gvReport.DataSource;
            for (int i = 6; i < source.Columns.Count; i++)
            {
                decimal TotalQty = 0;
                for (int j = 0; j < source.Rows.Count; j++)
                {
                    TotalQty += (source.Rows[j][source.Columns[i].ColumnName.ToString()].ToString()).GetDecimalValue();
                }

               //e.Row.Cells[i].Text = source.Compute("SUM(" + source.Columns[i].ColumnName + ")","").ToString();
                e.Row.Cells[i].Text = string.Format("{0:0.##}", TotalQty);
            }        
        }
    }

    protected void ddlDept_Init(object sender, EventArgs e)
    {
        try
        {
            var data = new ItemDepartmentController().FetchAll()
                                                     .Where(o => o.Deleted.GetValueOrDefault(false) == false)
                                                     .OrderBy(o => o.DepartmentName)
                                                     .ToList();
            data.Insert(0, new ItemDepartment { ItemDepartmentID = "ALL", DepartmentName = "ALL" });
            var ddl = (DropDownList)sender;
            ddl.DataSource = data;
            ddl.DataBind();
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
        }
    }

    protected void ddlCategory_Init(object sender, EventArgs e)
    {
        try
        {
            var data = new CategoryController().FetchAll()
                                                     .Where(o => o.Deleted.GetValueOrDefault(false) == false)
                                                     .OrderBy(o => o.CategoryName)
                                                     .ToList();
            data.Insert(0, new Category { CategoryName = "ALL" });
            var ddl = (DropDownList)sender;
            ddl.DataSource = data;
            ddl.DataBind();
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
        }
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
            var ddl = (DropDownList)sender;
            ddl.DataSource = data;
            ddl.DataBind();
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
        }
    }
}
