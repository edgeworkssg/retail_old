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

public partial class ViewReport : PageBase
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            BindGrid();
        }
    }

    private void BindGrid()
    {
        if (Session["Report"] != null &&
            Session["Report"] is DataTable)
        {
            //write remark

            gvReport.DataSource = (DataTable)Session["Report"];
            gvReport.DataBind();            
        }
        else
        {
            
        }
        if (Session["ReportRemark"] != null &&
            Session["ReportRemark"] is string)
        {
            //write remark
            lblRemark.Text = Session["ReportRemark"].ToString();            
        }
        else
        {

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
        /*
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
        
        BindGrid();*/
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
        DataTable dt = (DataTable)Session["Report"];
        if (e.Row.RowType == DataControlRowType.Header)
        {
            
            for (int k = 0; k < dt.Columns.Count; k++)
            {
                e.Row.Cells[k+1].Text = dt.Columns[k].Caption;                
            }
        }
        else if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int k = 0; k < dt.Columns.Count; k++)
            {
                if (dt.Columns[k].ExtendedProperties.ContainsKey("money")
                        && dt.Columns[k].ExtendedProperties["money"] is bool
                        && (bool)dt.Columns[k].ExtendedProperties["money"] == true
                        )
                {
                    e.Row.Cells[k+1].Text = String.Format("{0:N2}", dt.Rows[e.Row.RowIndex][k]);
                }
            }
        }
    }

    
    protected void lnkExport_Click(object sender, EventArgs e)
    {
        //        
        if (Session["Report"] != null &&
           Session["Report"] is DataTable)
        {

            DataTable dt = (DataTable)Session["Report"];

            
            GridView tmpG = new GridView();
            for (int p = 0; p < dt.Columns.Count; p++)
            {
                BoundField bf = new BoundField();
                bf.HeaderText = dt.Columns[p].Caption;
                bf.DataField = dt.Columns[p].ColumnName;
                tmpG.Columns.Add(bf);
            }
            
            CommonWebUILib.ExportCSVWithInvinsibleFields(dt, this.Page.Title.Trim(' '), this.Page.Title,tmpG);
        }     
    }

    protected void gvReport_RowCreated(object sender, GridViewRowEventArgs e)
    {        
    }
}
