using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using PowerPOS;
using SubSonic;
using SubSonic.Utilities;

public partial class StockOut : System.Web.UI.Page
{
    private const string SORT_DIRECTION = "SORT_DIRECTION";
    private const string ORDER_BY = "ORDER_BY";

    private InventoryController invCtrl;
    protected void Page_Load(object sender, EventArgs e)
    {
        #region *) Fetch: Get Local InvController from Web Browser's Cache
        invCtrl = null;
        if (Session["ToBeDeliveredList"] != null && Session["ToBeDeliveredList"] is InventoryController)
            invCtrl = (InventoryController)Session["ToBeDeliveredList"];

        if (invCtrl == null)
        {
            string CostingMethod = AppSetting.GetSetting(AppSetting.SettingsName.Inventory.CostingMethod);
            if (CostingMethod == null) CostingMethod = "";
            CostingMethod = CostingMethod.ToLower();

            if (CostingMethod == "fifo")
                invCtrl = new InventoryController(PowerPOS.Container.CostingMethods.FIFO);
            else if (CostingMethod == "fixed avg")
                invCtrl = new InventoryController(PowerPOS.Container.CostingMethods.FixedAvg);
            else
                invCtrl = new InventoryController(PowerPOS.Container.CostingMethods.FIFO);            
        }
        #endregion

        if (!IsPostBack)
        {
            ViewState["PreviousPage"] = Request.UrlReferrer; //Saves the Previous page url in ViewState

            #region *) Fetch: Load Stock Out Reasons
            InventoryStockOutReasonCollection Reasons = new InventoryStockOutReasonCollection();
            Reasons.Where(InventoryStockOutReason.Columns.ReasonID, Comparison.GreaterThan, 2);
            Reasons.Where(InventoryStockOutReason.Columns.Deleted, false);
            Reasons.Load();
            InventoryStockOutReason t = new InventoryStockOutReason();
            t.ReasonName = "--Select Reason--";
            t.ReasonID = -1;
            Reasons.Insert(0, t);

            Utility.LoadDropDown(cmbReason, Reasons, InventoryStockOutReason.Columns.ReasonName, InventoryStockOutReason.Columns.ReasonID, "--SELECT--");
            #endregion


            txtRefNo.Text = invCtrl.GetInvHdrRefNo() != "" ? invCtrl.GetInvHdrRefNo() : "-- Auto Generate --";

            txtInventoryDate.Text = DateTime.Now.ToString("dd MMM yyyy HH:mm");
            if (invCtrl.GetInventoryLocationID() != 0)
            {
                cmbLocation.ClearSelection();
                cmbLocation.Items.FindByValue(invCtrl.GetInventoryLocationID().ToString()).Selected = true;
            }
            if (invCtrl.getStockOutReasonID() != 0)
            {
                cmbReason.ClearSelection();
                cmbReason.Items.FindByValue(invCtrl.getStockOutReasonID().ToString()).Selected = true;
                cmbReason.Enabled = false;
            }
            if (invCtrl.GetRemark() != "")
                txtRemarks.Text = invCtrl.GetRemark();
        }

        BindGrid();
    }

    private void BindGrid()
    {
        string status = "";

        try
        {
            InventoryController invCtrl = null;
            #region *) Fetch: Get Local InvController from Web Browser's Cache
            if (Session["ToBeDeliveredList"] != null && Session["ToBeDeliveredList"] is InventoryController)
                invCtrl = (InventoryController)Session["ToBeDeliveredList"];

            if (invCtrl == null)
            {
                string CostingMethod = AppSetting.GetSetting(AppSetting.SettingsName.Inventory.CostingMethod);
                if (CostingMethod == null) CostingMethod = "";
                CostingMethod = CostingMethod.ToLower();

                if (CostingMethod == "fifo")
                    invCtrl = new InventoryController(PowerPOS.Container.CostingMethods.FIFO);
                else if (CostingMethod == "fixed avg")
                    invCtrl = new InventoryController(PowerPOS.Container.CostingMethods.FixedAvg);
                else
                    invCtrl = new InventoryController(PowerPOS.Container.CostingMethods.FIFO);
            }
            #endregion

            #region *) Fetch: Populate Registered Items
            gvReport.DataSource = invCtrl.FetchUnSavedInventoryItems(true, true, out status);
            gvReport.DataBind();
            #endregion
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
        }
    }
    /// <summary>
    /// Binds and saves the data
    /// </summary>
    /// <param name="id"></param>
    void BindAndSave()
    {
            string status="";

            if (!invCtrl.StockOut(Session["UserName"].ToString(), int.Parse(cmbReason.SelectedValue),
                int.Parse(cmbLocation.SelectedValue), false, true, out status))
            {
                //lblResult.Text = "<span style=\"font-weight:bold; color:#22bb22\">Customer saved.</span>";
                Logger.writeLog(status);
                throw new Exception(status);
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

            string pageCount = "<b>" + gvReport.PageCount.ToString() + "</b> (" + itemCount.ToString() + " Items)";
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
            decimal tmp = 0;
            Decimal.TryParse(e.Row.Cells[4].Text, out tmp);
            e.Row.Cells[4].Text = String.Format("{0:N2}", tmp);
        }
    }

    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        try
        {
            if (cmbLocation.SelectedValue == "0")
                throw new Exception("Please select location");
            if (cmbLocation.SelectedValue == "0")
                throw new Exception("Please select location");

            BindAndSave();

            Session.Remove("ToBeDeliveredList");

            if (ViewState["PreviousPage"] != null)	//Check if the ViewState 
            //contains Previous page URL
            {
                Response.Redirect(ViewState["PreviousPage"].ToString());//Redirect to 
                //Previous page by retrieving the PreviousPage Url from ViewState.
            }
        }

        catch (Exception x)
        {
            lblResult.Text = "<span style=\"font-weight:bold; color:#990000\">Customer not saved:</span> " + x.Message;
        }
    }


}
