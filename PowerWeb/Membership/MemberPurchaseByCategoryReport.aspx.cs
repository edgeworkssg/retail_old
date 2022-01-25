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
using System.Collections.Generic;


public partial class MemberPurchaseByCategoryReport : PageBase
{    
    private const string SORT_DIRECTION = "SORT_DIRECTION";
    private const string ORDER_BY = "ORDER_BY";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            ViewState["Sort"] = "";
            BindGrid(ViewState["Sort"].ToString());
            ddlMonth.SelectedIndex = DateTime.Today.Month - 1;
            ddlMonth.DataBind();
            txtStartDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            txtEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        }
    }

    private void BindGrid(string sort)
    {
        DateTime StartDate, EndDate;
        if (cbFilterByDate.Checked)
        {
            if (rdbMonth.Checked)
            {
                StartDate = new DateTime(DateTime.Today.Year, int.Parse(ddlMonth.SelectedItem.Value), 1);
                EndDate = new DateTime(DateTime.Today.Year, int.Parse(ddlMonth.SelectedItem.Value), 
                    DateTime.DaysInMonth(DateTime.Today.Year, int.Parse(ddlMonth.SelectedItem.Value)));
            }
            else
            {
                if (!DateTime.TryParse(txtStartDate.Text, out StartDate) ||
                    !DateTime.TryParse(txtEndDate.Text, out EndDate))
                {
                    StartDate = new DateTime(2007, 01, 01);
                    EndDate = DateTime.Now;
                }
            }
        }
        else
        {
            StartDate = new DateTime(2007, 01, 01);
            EndDate = DateTime.Now;
        }

        if (rdbMonth.Checked)
        {
            int selectedMonth = int.Parse(ddlMonth.SelectedValue.ToString());
            StartDate = new DateTime(DateTime.Now.Year, selectedMonth, 1);
            EndDate = new DateTime(DateTime.Now.Year, selectedMonth, DateTime.DaysInMonth(DateTime.Now.Year, selectedMonth));
        }

        DataTable dt =
            ReportController.FetchCustomerPurchaseGroupByCategory(StartDate , EndDate);

        var col = new List<int>();
        for (int i = 1; i < dt.Columns.Count; i++)
            col.Add(i);
        dt = LanguageManager.GetTranslation(dt, col.ToArray());


        DataView dv = new DataView();        
        dv.Table = dt;
        if (sort == "")
            sort = LanguageManager.GetTranslation("Total");
        dv.Sort = sort+ " DESC";
        
        gvReport.DataSource = dv;
        gvReport.DataBind(); 
        
    }

    protected void gvReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvReport.PageIndex = e.NewPageIndex;
        BindGrid(ViewState["Sort"].ToString());
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
        DataView dv = (DataView)gvReport.DataSource;
        DataTable dt = dv.Table;
        
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
    
    protected void gvReport_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            try
            {

                int NumOfCat = new Query("Category").GetCount("CategoryName");
                decimal amt;
                for (int i = 3; i < NumOfCat + 4; i++)
                {
                    if (decimal.TryParse(e.Row.Cells[i].Text, out amt))
                        e.Row.Cells[i].Text = String.Format("{0:N2}", amt);
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            int NumOfCat = new Query("Category").GetCount("CategoryName");
            DataTable dt = ((DataView)gvReport.DataSource).Table;
            for (int i = 3; i < NumOfCat + 4; i++)
            {
                //if (decimal.TryParse(e.Row.Cells[i].Text, out amt))

                //e.Row.Cells[i].Text = String.Format("{0:N2}", (decimal)dt.Compute("SUM([" + dt.Columns[i-1].ColumnName + "])", ""));
            }
        }
        else if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[1].Text = LanguageManager.GetTranslation("MembershipNo");
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (ViewState["Sort"] == null)
            ViewState["Sort"] = "Total";
        BindGrid(ViewState["Sort"].ToString());
    }
}
