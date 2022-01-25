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
using SubSonic;
using SubSonic.Utilities;
using PowerPOS;
using System.Linq;

public partial class PrivilegeScaffold : PageBase
{
    private bool isAdd = false;
    private const string SORT_DIRECTION = "SORT_DIRECTION";
    private const string ORDER_BY = "ORDER_BY";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["id"] != null)
        {
            string id = Utility.GetParameter("id");
            if (!String.IsNullOrEmpty(id) && id != "0")
            {
                if (!Page.IsPostBack)
                {
                    LoadEditor(id);
                }
            }
            else
            {
                //it's an add, show the editor
                isAdd = true;
                ToggleEditor(true);
                if (!Page.IsPostBack)
                    LoadDrops();
                btnDelete.Visible = false;
            }
        }
        else
        {
            ToggleEditor(false);
            if (!Page.IsPostBack)
            {
                BindGrid();
            }
        }
    }

    /// <summary>
    /// Loads the editor with data
    /// </summary>
    /// <param name="id"></param>
    void LoadEditor(string id)
    {
        ToggleEditor(true);
        LoadDrops();
        if (!string.IsNullOrEmpty(id) && id != "0")
        {
            hdfID.Value = id.ToString();
            txtID.Text = id.ToString();

            //pull the record
            UserPrivilege item = new UserPrivilege(id);
            txtPrivilegesName.Text = item.PrivilegeName;
            txtFormName.Text = item.FormName;
            txtPrivilegeCategory.Text = item.PrivilegeCategory;
            cbIsRetail.Checked = item.IsRetail;
            cbIsWholesale.Checked = item.IsWholesale;
            cbIsBeauty.Checked = item.IsBeauty;
            cbIsSuperAdmin.Checked = item.IsSuperAdmin;
            ctrlCreatedBy.Text = item.CreatedBy;
            ctrlCreatedOn.Text = item.CreatedOn.ToString();
            ctrlModifiedBy.Text = item.ModifiedBy;
            ctrlModifiedOn.Text = item.ModifiedOn.ToString();
            //set the delete confirmation
            btnDelete.Attributes.Add("onclick", "return CheckDelete();");
        }
    }

    /// <summary>
    /// Loads the DropDownLists
    /// </summary>
    void LoadDrops()
    {
        //load the listboxes
    }


    /// <summary>
    /// Shows/Hides the Grid and Editor panels
    /// </summary>
    /// <param name="showIt"></param>
    void ToggleEditor(bool showIt)
    {
        pnlEdit.Visible = showIt;
        pnlGrid.Visible = !showIt;
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        LoadEditor("0");
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        UserPrivilege.Delete(Utility.GetParameter("id"));
        AccessLogController.AddLog(AccessSource.WEB, Session["UserName"] + "", "-", string.Format("DELETE UserPrivilege : {0}", Utility.GetParameter("id")), "");
        //redirect
        Response.Redirect(Request.CurrentExecutionFilePath);
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string id = hdfID.Value;
        //bool haveError = false;
        try
        {
            BindAndSave(id);
            lblResult.Text = "<span style=\"font-weight:bold; color:#22bb22\">" + LanguageManager.GetTranslation("User Privilege saved.") + "</span>";
        }
        catch (Exception x)
        {
            //haveError = true;
            lblResult.Text = "<span style=\"font-weight:bold; color:#990000\">" + LanguageManager.GetTranslation("User Privilege not saved:") + "</span> " + x.Message;
        }
    }

    /// <summary>
    /// Binds and saves the data
    /// </summary>
    /// <param name="id"></param>
    void BindAndSave(string id)
    {
        if (!String.IsNullOrEmpty(id) && id != "0")
        {
            Query qr = UserPrivilege.CreateQuery();
            qr.QueryType = QueryType.Update;
            qr.AddWhere(UserPrivilege.Columns.UserPrivilegeID, hdfID.Value);
            qr.AddUpdateSetting(UserPrivilege.Columns.PrivilegeName, txtPrivilegesName.Text);
            qr.AddUpdateSetting(UserPrivilege.Columns.FormName, txtFormName.Text);
            qr.AddUpdateSetting(UserPrivilege.UserColumns.PrivilegeCategory, txtPrivilegeCategory.Text);
            qr.AddUpdateSetting(UserPrivilege.UserColumns.IsRetail, cbIsRetail.Checked);
            qr.AddUpdateSetting(UserPrivilege.UserColumns.IsBeauty, cbIsBeauty.Checked);
            qr.AddUpdateSetting(UserPrivilege.UserColumns.IsWholesale, cbIsWholesale.Checked);
            qr.AddUpdateSetting(UserPrivilege.UserColumns.IsSuperAdmin, cbIsSuperAdmin.Checked);
            qr.AddUpdateSetting(UserPrivilege.Columns.ModifiedBy, Session["Username"].ToString());
            qr.AddUpdateSetting(UserPrivilege.Columns.ModifiedOn, DateTime.Now);
            qr.AddUpdateSetting(UserPrivilege.Columns.Deleted, ctrlDeleted.Checked);
            qr.Execute();
            AccessLogController.AddLog(AccessSource.WEB, Session["UserName"] + "", "-", string.Format("UPDATE UserPrivilege : {0}", Utility.GetParameter("id")), "");
        }
        else
        {

            UserPrivilege ct = new UserPrivilege();
            ct.IsNew = true;
            ct.PrivilegeName = txtPrivilegesName.Text;
            ct.FormName = txtFormName.Text;
            ct.PrivilegeCategory = txtPrivilegeCategory.Text;
            ct.IsWholesale = cbIsWholesale.Checked;
            ct.IsRetail = cbIsRetail.Checked;
            ct.IsBeauty = cbIsBeauty.Checked;
            ct.IsSuperAdmin = cbIsSuperAdmin.Checked;
            ct.ModifiedBy = Session["Username"].ToString();
            ct.ModifiedOn = DateTime.Now;
            ct.CreatedBy = Session["Username"].ToString();
            ct.CreatedOn = DateTime.Now;
            ct.Deleted = false;
            ct.IsNew = true;
            ct.Save(Session["username"].ToString());
            AccessLogController.AddLog(AccessSource.WEB, Session["UserName"] + "", "-", string.Format("ADD UserPrivilege : {0}", txtID.Text), "");
        }
    }

    /// <summary>
    /// Binds the GridView
    /// </summary>
    private void BindGrid()
    {
        string search = txtSearch.Text;

        string sql = "SELECT UserPrivilegeID, PrivilegeName, FormName, ISNULL(Userfld1,'') as PrivilegeCategory, ISNULL(Userflag1,0) as IsSuperAdmin, " +
                    "ISNULL(Userflag2,0) as IsRetail, ISNULL(Userflag3,0) as IsWholesale, ISNULL(Userflag4,0) as IsBeauty " + 
                    "FROM UserPrivilege where ISNULL(PrivilegeName,'') + ISNULL(FormName,'') + ISNULL(userfld1,'') like '%"+ search +"%'";

        if (ViewState[ORDER_BY] == null)
            ViewState[ORDER_BY] = "PrivilegeName";

        if (ViewState[SORT_DIRECTION] == null)
            ViewState[SORT_DIRECTION] = "ASC";

        DataTable dt = DataService.GetDataSet(new QueryCommand(sql)).Tables[0];
        DataView dv = new DataView(dt);
        dv.Sort = ViewState[ORDER_BY].ToString() + " " + ViewState[SORT_DIRECTION].ToString();

        GridView1.DataSource = dv;
        GridView1.DataBind();
    }

    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        BindGrid();
    }


    protected void GridView1_DataBound(Object sender, EventArgs e)
    {
        GridViewRow gvrPager = GridView1.BottomPagerRow;
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
            for (int i = 0; i < GridView1.PageCount; i++)
            {
                int intPageNumber = i + 1;
                ListItem lstItem = new ListItem(intPageNumber.ToString());
                if (i == GridView1.PageIndex)
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
            DataSet ds = GridView1.DataSource as DataSet;
            if (ds != null)
            {
                itemCount = ds.Tables[0].Rows.Count;
            }

            string pageCount = "<b>" + GridView1.PageCount.ToString() + "</b> (" + itemCount.ToString() + " Items)";
            lblPageCount.Text = pageCount;
        }

        Button btnPrev = (Button)gvrPager.Cells[0].FindControl("btnPrev");
        Button btnNext = (Button)gvrPager.Cells[0].FindControl("btnNext");
        Button btnFirst = (Button)gvrPager.Cells[0].FindControl("btnFirst");
        Button btnLast = (Button)gvrPager.Cells[0].FindControl("btnLast");
        //now figure out what page we're on
        if (GridView1.PageIndex == 0)
        {
            btnPrev.Enabled = false;
            btnFirst.Enabled = false;
        }

        else if (GridView1.PageIndex + 1 == GridView1.PageCount)
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
        GridViewRow gvrPager = GridView1.BottomPagerRow;
        DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages");
        GridView1.PageIndex = ddlPages.SelectedIndex;
        // a method to populate your grid
        BindGrid();
    }

    protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
    {
        string columnName = e.SortExpression;
        //rebind the grid
        if (ViewState[SORT_DIRECTION] == null || ((string)ViewState[SORT_DIRECTION]) == SqlFragment.ASC)
        {
            ViewState[SORT_DIRECTION] = SqlFragment.DESC;
        }
        else
        {
            ViewState[SORT_DIRECTION] = SqlFragment.ASC;
        }

        ViewState[ORDER_BY] = columnName;
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


    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindGrid();
    }

    protected void btnClearFilter_Click(object sender, EventArgs e)
    {
        txtSearch.Text = "";
    }
}