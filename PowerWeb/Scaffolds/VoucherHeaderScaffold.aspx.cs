using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SubSonic;
using SubSonic.Utilities;
using PowerPOS;

public partial class VoucherHeaderScaffold : PageBase
{
    private bool isAdd = false;
    private const string SORT_DIRECTION = "SORT_DIRECTION";
    private const string ORDER_BY = "ORDER_BY";
    private const int COL_Outlet = 13;

    protected void Page_Load(object sender, EventArgs e)
    {
        SetFormSetting();
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
                BindGrid(String.Empty);
            }
        }
    }

    private void SetFormSetting()
    {
        try
        {
            string outletText = LanguageManager.GetTranslation(LabelController.OutletText);
            GridView1.Columns[COL_Outlet].HeaderText = outletText;
            lblOutlet.Text = outletText;
            ddlMultiOutlet.Texts.SelectBoxCaption = "Select " + outletText;
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
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
        if (!String.IsNullOrEmpty(id) && id != "0")
        {
            lblVoucherHeaderID.Text = id.ToString();
            //pull the record
            VoucherHeader item = new VoucherHeader(id);

            txtVoucherHeaderName.Text = item.VoucherHeaderName;
            txtAmount.Text = item.Amount.ToString("N2");
            txtValidFrom.Text = item.ValidFrom.ToString("dd MMM yyyy");
            txtValidTo.Text = item.ValidTo.ToString("dd MMM yyyy");
            txtVoucherPrefix.Text = item.VoucherPrefix;
            txtVoucherSuffix.Text = item.VoucherSuffix;
            txtStartNumber.Text = item.StartNumber.ToString();
            txtEndNumber.Text = item.EndNumber.ToString();
            txtNumOfDigit.Text = item.NumOfDigit.ToString();

            try
            {
                string[] selOutlet = item.AssignedOutletList;
                for (int i = 0; i < ddlMultiOutlet.Items.Count; i++)
                    ddlMultiOutlet.Items[i].Selected = selOutlet.Contains(ddlMultiOutlet.Items[i].Value);
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }

            ctrlCreatedBy.Text = item.CreatedBy;
            if (item.CreatedOn.HasValue)
                ctrlCreatedOn.Text = item.CreatedOn.Value.ToString();
            ctrlModifiedBy.Text = item.ModifiedBy;
            if (item.ModifiedOn.HasValue)
                ctrlModifiedOn.Text = item.ModifiedOn.Value.ToString();
            if (item.Deleted.HasValue)
                ctrlDeleted.Checked = item.Deleted.Value;

            //set the delete confirmation
            btnDelete.Attributes.Add("onclick", "return CheckDelete();");
        }
        else
        {
            txtValidFrom.Text = DateTime.Today.ToString("dd MM yyyy");
            txtValidTo.Text = DateTime.Today.AddYears(100).ToString("dd MM yyyy");
        }
    }

    /// <summary>
    /// Loads the DropDownLists
    /// </summary>
    void LoadDrops()
    {
        //load the listboxes
        var qr = new Query("Outlet");
        qr.AddWhere(Outlet.Columns.Deleted, Comparison.Equals, 0);
        var ouList = new OutletController()
                    .FetchByQuery(qr)
                    .OrderByDescending(o => o.OutletName)
                    .ToList();

        ddlMultiOutlet.Items.Clear();
        foreach (var ou in ouList)
            ddlMultiOutlet.Items.Add(new ListItem { Value = ou.OutletName, Text = ou.OutletName });
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
        try
        {
            string status;
            int voucherHeaderID = Utility.GetParameter("id").GetIntValue();
            if (CancelAllVouchers(voucherHeaderID, out status))
            {
                VoucherHeader.Delete(Utility.GetParameter("id"));
                AccessLogController.AddLog(AccessSource.WEB, Session["UserName"] + "", "-", string.Format("DELETE VoucherHeader : {0}", Utility.GetParameter("id")), "");
                //redirect
                Response.Redirect(Request.CurrentExecutionFilePath);
            }
            else
            {
                lblResult.Text = "<span style=\"font-weight:bold; color:#990000\">" + LanguageManager.GetTranslation("Voucher Header not deleted:") + "</span> " + status;
            }
        }
        catch (Exception x)
        {
            lblResult.Text = "<span style=\"font-weight:bold; color:#990000\">" + LanguageManager.GetTranslation("Voucher Header not deleted:") + "</span> " + x.Message;
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string id = Utility.GetParameter("id");
        try
        {
            string status;
            BindAndSave(id);
            if (!String.IsNullOrEmpty(id) && id != "0") //if edit
                VoucherController.ChangeBatchVoucherOutlet(id.GetIntValue(), Session["UserName"] + "", out status);
            lblResult.Text = "<span style=\"font-weight:bold; color:#22bb22\">" + LanguageManager.GetTranslation("Voucher Header saved.") + "</span>";
        }
        catch (Exception x)
        {
            //haveError = true;
            lblResult.Text = "<span style=\"font-weight:bold; color:#990000\">" + LanguageManager.GetTranslation("Voucher Header not saved:") + "</span> " + x.Message;
        }
    }

    /// <summary>
    /// Binds and saves the data
    /// </summary>
    /// <param name="id"></param>
    void BindAndSave(string id)
    {
        #region *) Validation
        DateTime validFrom, validTo;
        if (!DateTime.TryParse(txtValidFrom.Text, out validFrom))
        {
            throw new Exception("'Valid From' value is invalid");
        }
        if (!DateTime.TryParse(txtValidTo.Text, out validTo))
        {
            throw new Exception("'Valid To' value is invalid");
        }
        #endregion

        VoucherHeader item;
        if (!String.IsNullOrEmpty(id) && id != "0")
        {
            //it's an edit
            item = new VoucherHeader(id);
            AccessLogController.AddLog(AccessSource.WEB, Session["UserName"] + "", "-", string.Format("UPDATE VoucherHeader : {0}", Utility.GetParameter("id")), "");
        }
        else
        {
            //add
            item = new VoucherHeader();
            item.Deleted = false;
        }

        item.VoucherHeaderName = txtVoucherHeaderName.Text;
        item.Amount = txtAmount.Text.GetDecimalValue();
        item.ValidFrom = validFrom;
        item.ValidTo = validTo;
        item.VoucherPrefix = txtVoucherPrefix.Text;
        item.VoucherSuffix = txtVoucherSuffix.Text;
        item.StartNumber = txtStartNumber.Text.Trim().GetIntValue();
        item.EndNumber = txtEndNumber.Text.Trim().GetIntValue();
        item.NumOfDigit = txtNumOfDigit.Text.Trim().GetIntValue();

        var selOutlet = new List<string>();
        for (int i = 0; i < ddlMultiOutlet.Items.Count; i++)
        {
            if (ddlMultiOutlet.Items[i].Selected)
                selOutlet.Add(ddlMultiOutlet.Items[i].Value);
        }
        if (selOutlet.Count == ddlMultiOutlet.Items.Count)
        {
            selOutlet.Clear();
            selOutlet.Add("ALL");
        }
        item.AssignedOutletList = selOutlet.ToArray();

        item.Save(Session["UserName"] + "");
    }

    /// <summary>
    /// Binds the GridView
    /// </summary>
    private void BindGrid(string orderBy)
    {
        DataTable dt;
        if (orderBy == string.Empty)
            orderBy = "VoucherHeaderID";

        string sql = @"
                        SELECT *,
                            VoucherPrefix + REPLACE(STR(StartNumber, NumOfDigit), ' ', '0') + VoucherSuffix AS VoucherNoFrom,
                            VoucherPrefix + REPLACE(STR(EndNumber, NumOfDigit), ' ', '0') + VoucherSuffix AS VoucherNoTo
                        FROM VoucherHeader
                        ORDER BY {0} {1}
                     ";
        if ((string)ViewState[SORT_DIRECTION] == SqlFragment.ASC)
            sql = string.Format(sql, orderBy, "ASC");
        else
            sql = string.Format(sql, orderBy, "DESC");

        dt = DataService.GetDataSet(new QueryCommand(sql, "PowerPOS")).Tables[0];

        GridView1.DataSource = dt;
        GridView1.DataBind();
    }

    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        BindGrid(String.Empty);
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
        BindGrid(String.Empty);
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

        BindGrid(columnName);
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

    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string status = "";
        try
        {
            if (e.CommandName.ToUpper() == "ISSUEALL")
            {
                int voucherHeaderID = int.Parse(e.CommandArgument.ToString());
                VoucherHeader vh = new VoucherHeader(voucherHeaderID);
                if (vh != null && vh.VoucherHeaderID == voucherHeaderID)
                {
                    if (VoucherController.CreateBatchVoucherNo(vh.StartNumber, vh.EndNumber, vh.VoucherPrefix, vh.VoucherSuffix, vh.Amount, vh.NumOfDigit, vh.ValidFrom, vh.ValidTo, vh.VoucherHeaderID, vh.Outlet, out status))
                    {
                        string sql = "UPDATE VoucherHeader SET IssuedQuantity = (SELECT COUNT(*) FROM Vouchers WHERE VoucherHeaderID = VoucherHeader.VoucherHeaderID), ModifiedOn = GETDATE(), ModifiedBy = '{1}' WHERE VoucherHeaderID = {0}";
                        sql = string.Format(sql, vh.VoucherHeaderID, Session["UserName"] + "");
                        DataService.ExecuteQuery(new QueryCommand(sql, "PowerPOS"));
                        lblResult.Text = "<span style=\"font-weight:bold; color:#22bb22\">Vouchers issued successfully.</span>";
                        BindGrid(ViewState["sortBy"] + "");
                    }
                    else
                    {
                        lblResult.Text = "<span style=\"font-weight:bold; color:#990000\">" + LanguageManager.GetTranslation("Error:") + "</span> " + status;
                    }
                }
                else
                {
                    lblResult.Text = "<span style=\"font-weight:bold; color:#990000\">" + LanguageManager.GetTranslation("Error:") + "</span> Voucher Header ID not found.";
                }
            }
            else if (e.CommandName.ToUpper() == "CANCELALL")
            {
                int voucherHeaderID = int.Parse(e.CommandArgument.ToString());
                if (CancelAllVouchers(voucherHeaderID, out status))
                {
                    lblResult.Text = "<span style=\"font-weight:bold; color:#22bb22\">Vouchers canceled successfully.</span>";
                    BindGrid(ViewState["sortBy"] + "");
                }
                else
                {
                    lblResult.Text = "<span style=\"font-weight:bold; color:#990000\">" + LanguageManager.GetTranslation("Error:") + "</span> " + status;
                }
            }
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
            status = ex.Message;
            lblResult.Text = "<span style=\"font-weight:bold; color:#990000\">" + LanguageManager.GetTranslation("Error:") + "</span> " + status;
        }
    }

    private bool CancelAllVouchers(int voucherHeaderID, out string status)
    {
        if (VoucherController.CancelBatchVoucherNo(voucherHeaderID, Session["UserName"] + "", out status))
        {
            string sql = "UPDATE VoucherHeader SET CanceledQuantity = (SELECT COUNT(*) FROM Vouchers WHERE VoucherHeaderID = VoucherHeader.VoucherHeaderID AND Deleted = 1), ModifiedOn = GETDATE(), ModifiedBy = '{1}' WHERE VoucherHeaderID = {0}";
            sql = string.Format(sql, voucherHeaderID, Session["UserName"] + "");
            DataService.ExecuteQuery(new QueryCommand(sql, "PowerPOS"));
            return true;
        }
        else
        {
            return false;
        }
    }
}
