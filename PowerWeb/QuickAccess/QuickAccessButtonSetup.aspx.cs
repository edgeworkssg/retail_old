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

public partial class QuickAccessButtonSetup: PageBase
{
    string catID, groupID;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserName"] == null || Session["Role"] == null || (string)Session["UserName"] == "" || (string)Session["Role"] == "")
        {
            Response.Redirect("../login.aspx");
        }

        if (Request.QueryString["CatId"] != null)
        {
            catID = Utility.GetParameter("CatId");
            QuickAccessCategory q = new QuickAccessCategory(new Guid(catID));
            lblCatName.Text = q.QuickAccessCatName;
            CategoryID = q.QuickAccessCategoryId;            
        }
        if (Request.QueryString["GroupID"] != null)
        {
            groupID = Utility.GetParameter("GroupID");
        }
        BindTable();
    }
    private Guid CategoryID;
    private void BindTable()
    {
        ViewQuickAccessButtonCollection v = new ViewQuickAccessButtonCollection();
        v.Where(ViewQuickAccessButton.Columns.QuickAccessCategoryID, new Guid(catID));
        v.Where(ViewQuickAccessButton.Columns.QuickAccessGroupID, new Guid(groupID));
        v.Load().ToDataTable();
               

            DataTable dt = new DataTable("Blank");
            dt.Columns.Add("c0");
            dt.Columns.Add("c1");
            dt.Columns.Add("c2");
            dt.Columns.Add("c3");
            dt.Columns.Add("c4");
            dt.Columns.Add("c5");
            dt.Columns.Add("c6");
            for (int i = 0; i < 7; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            GridView1.DataSource = dt;
            GridView1.DataBind();
            //run to the row and column of the table and populate it.....
            for (int i = 0; i < v.Count; i++)
            {
                if (v[i].Row >= 0 && v[i].Col >= 0 &&
                    v[i].Row < GridView1.Rows.Count &&
                    v[i].Col < GridView1.Columns.Count)
                {
                    Control c = GridView1.Rows[v[i].Row].Cells[v[i].Col].Controls[1];
                    HyperLink tmp = ((HyperLink)c);
                    tmp.Text = v[i].ItemName;
                    if (v[i].ForeColor != null && v[i].ForeColor != "")
                    {
                        tmp.ForeColor =
                            System.Drawing.Color.FromName(v[i].ForeColor);
                    }
                    if (v[i].BackColor != null && v[i].BackColor != "")
                    {
                        GridView1.Rows[v[i].Row].Cells[v[i].Col].BackColor =
                            System.Drawing.Color.FromName(v[i].BackColor);
                    }
                    tmp.NavigateUrl = "javascript:poptastic('QuickAccessButtonForm.aspx"
                        + "?catid=" +
                    CategoryID.ToString() + "&row=" + v[i].Row +
                    "&col=" + v[i].Col + "&update=1" + "');";
                }
            }
            //GridView1.DataBind();
        //}
    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //Bound every columns with the appropriate query string....
            for (int i = 0; i < GridView1.Columns.Count; i++)
            {
                Control c = e.Row.Cells[i].Controls[1];
                HyperLink tmp = ((HyperLink)c);
                tmp.NavigateUrl = "javascript:poptastic('QuickAccessButtonForm.aspx"                    
                    + "?catid=" + 
                    CategoryID.ToString() + "&row=" + e.Row.RowIndex +
                    "&col=" + i.ToString() + "');";
            }
        }
    }
}
