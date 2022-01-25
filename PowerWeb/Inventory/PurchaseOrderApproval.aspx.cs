using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using PowerPOS;
using System.Globalization;
using PowerPOS.Container;
using SubSonic;

namespace PowerWeb.Inventory
{
    public partial class PurchaseOrderApproval : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
                LoadDrops();
        }

        #region *) Method

        private void LoadDrops()
        {
            var users = new List<UserMst>();
            users.Add(new UserMst{ UserName="ALL"});
            users.AddRange(new UserMstController().FetchAll()
                                               .Where(o => o.Deleted.GetValueOrDefault(false) == false)
                                               .OrderBy(o => o.UserName)
                                               .ToList());
            ddlUser.DataValueField = "UserName";
            ddlUser.DataTextField = "UserName";
            ddlUser.DataSource = users;
            ddlUser.DataBind();

            var locations = new List<InventoryLocation>();
            locations.Add(new InventoryLocation{ InventoryLocationID = 0, InventoryLocationName="ALL"});
            locations.AddRange(new InventoryLocationController().FetchAll()
                                               .Where(o => o.Deleted.GetValueOrDefault(false) == false)
                                               .OrderBy(o => o.InventoryLocationName)
                                               .ToList());
            ddlLocation.DataValueField = "InventoryLocationID";
            ddlLocation.DataTextField = "InventoryLocationName";
            ddlLocation.DataSource = locations;
            ddlLocation.DataBind();

            var suppliers = new List<Supplier>();
            suppliers.Add(new Supplier { SupplierID = 0, SupplierName = "ALL" });
            suppliers.AddRange(new SupplierController().FetchAll()
                                               .OrderBy(o => o.SupplierName)
                                               .ToList());
            ddlSupplier.DataValueField = "SupplierID";
            ddlSupplier.DataTextField = "SupplierName";
            ddlSupplier.DataSource = suppliers;
            ddlSupplier.DataBind();

            txtStartDate.Text = DateTime.Now.ToString("dd MMM yyyy");
            txtEndDate.Text = DateTime.Now.ToString("dd MMM yyyy");

            txtRemarks.Text = "";
            ddlStatus.SelectedIndex = 0;
        }

        private void BindData()
        {
            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now;

            DateTime.TryParseExact(txtStartDate.Text, "dd MMM yyyy", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out startDate);
            DateTime.TryParseExact(txtEndDate.Text, "dd MMM yyyy", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out endDate);


            PurchaseOrderHdrController ph = new PurchaseOrderHdrController();
            gvPurchaseOrder.DataSource = ph.FetchData(startDate, endDate, ddlUser.SelectedValue,
                ddlLocation.SelectedValue.GetIntValue(), ddlSupplier.SelectedValue.GetIntValue(), txtRemarks.Text,
                ddlStatus.SelectedValue, "", "", txtPONumber.Text);
            gvPurchaseOrder.DataBind();
        }

        private void ViewPO(string refNo)
        {
 
        }

        private void PrintPO(string refNo)
        {
 
        }

        private void ApprovePO(string refNo)
        {
            UserInfo.username = Session["UserName"] + "";
            string status = "";
            //PurchaseOrderHdrController ph = new PurchaseOrderHdrController();
            //if (ph.ApprovePO(refNo, out status))
            //{
            //    AccessLogController.AddLog(AccessSource.WEB, Session["UserName"] + "", "-", string.Format("Approve PO : {0}", refNo), "");
            //    BindData();
            //    Show("Purchase order successfully approved!");
            //}
            //else
            //{
            //    Show(status);
            //}

            PurchaseOdrController poCtrl = new PurchaseOdrController(refNo);
            bool updateCostPrice = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.AutoUpdateCostPriceOnSupplierPOApproval), false);
            QueryCommandCollection approveCmd;
            if (poCtrl.GetApprovePOAndUpdateCostPriceCommand(updateCostPrice, out status, out approveCmd))
            {
                if (approveCmd != null && approveCmd.Count > 0)
                {
                    DataService.ExecuteTransaction(approveCmd);
                    AccessLogController.AddLog(AccessSource.WEB, Session["UserName"] + "", "-", string.Format("Approve PO : {0}", refNo), "");
                    BindData();
                    Show("Purchase order successfully approved!");
                }
            }
            else
            {
                Show(status);
            } 
        }

        private void RejectPO(string refNo)
        {
            InventoryHdrCollection col = new InventoryHdrCollection();
            col.Where(InventoryHdr.Columns.PurchaseOrderNo, refNo);
            col.Load();
            if (col.Count > 0)
            {
                Show("Purchase Order is already used in the goods receive");
                return;
            }

            UserInfo.username = Session["UserName"] + "";
            PurchaseOrderHdrController ph = new PurchaseOrderHdrController();
            string status = "";
            if (ph.RejectPO(refNo, out status))
            {
                AccessLogController.AddLog(AccessSource.WEB, Session["UserName"] + "", "-", string.Format("Reject PO : {0}", refNo), "");
                BindData();
                Show("Purchase order successfully rejected!");
            }
            else
            {
                Show(status);
            }
        }

        private void CancelPO(string refNo)
        {
            try
            {
                PurchaseOrderHdr po = new PurchaseOrderHdr(refNo);
                po.Userfld7 = "Canceled";
                //po.Deleted = true;
                po.Save(Session["UserName"] + "");
                BindData();
                Show("Purchase order successfully canceled!");
            }
            catch (Exception ex)
            {
                Show("Error : "+ex.Message);
                Logger.writeLog(ex);
            }
        }

        private void ClosePO(string refNo)
        {
            try
            {
                PurchaseOrderHdr po = new PurchaseOrderHdr(refNo);
                po.Userfld7 = "Posted";
                //po.Deleted = true;
                po.Save(Session["UserName"] + "");
                BindData();
                Show("Purchase order successfully closed!");
            }
            catch (Exception ex)
            {
                Show("Error : " + ex.Message);
                Logger.writeLog(ex);
            }
        }

        public void Show(string message)
        {
            message = LanguageManager.GetTranslation(message);
            string cleanMessage = message.Replace("'", "\'");
            Page page = HttpContext.Current.CurrentHandler as Page;
            string script = string.Format("alert('{0}');", cleanMessage);
            if (page != null && !page.ClientScript.IsClientScriptBlockRegistered("alert"))
            {
                page.ClientScript.RegisterClientScriptBlock(page.GetType(), "alert", script, true /* addScriptTags */);
            }
        } 

        #endregion

        #region *) Event Handler

        protected void gvPurchaseOrder_Sorting(object sender, GridViewSortEventArgs e)
        {

        }

        protected void gvPurchaseOrder_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    bool isApproved = e.Row.Cells[7].Text == "Approved" || e.Row.Cells[7].Text == "Rejected";
                    bool isRejected = e.Row.Cells[7].Text == "Rejected";
                    bool isPosted = e.Row.Cells[7].Text == "Posted" || e.Row.Cells[7].Text == "Partially Received";
                    bool isPartiallyReceived = e.Row.Cells[7].Text == "Partially Received";
                    bool isSubmitted = e.Row.Cells[7].Text == "Submitted";
                    bool isCanceled = e.Row.Cells[7].Text == "Canceled";
                    Button btnApprove = ((Button)(e.Row.FindControl("btnApprove")));
                    Button btnCancel = ((Button)(e.Row.FindControl("btnCancel")));
                    Button btnClose = ((Button)(e.Row.FindControl("btnClose")));
                    HtmlGenericControl divEdit = ((HtmlGenericControl)(e.Row.FindControl("divEdit")));
                    HtmlGenericControl divMail = ((HtmlGenericControl)(e.Row.FindControl("divMail")));
                    
                    Button btnReject = ((Button)(e.Row.FindControl("btnReject")));
                    //Button btnApprove = ((Button)(e.Row.Cells[8].Controls[1]));
                    //Button btnPrint = ((Button)(e.Row.Cells[8].Controls[3]));
                    btnApprove.Visible = !isApproved && !isPosted && !isCanceled;
                    divEdit.Visible = !isApproved && !isPosted && !isCanceled;
                    btnReject.Visible = isSubmitted;
                    divMail.Visible = isApproved && !isRejected && !isPosted && !isCanceled;
                    btnCancel.Visible = !isPosted && !isCanceled && !isRejected;
                    btnClose.Visible = isPosted && isPartiallyReceived;
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        protected void gvPurchaseOrder_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string arg = e.CommandArgument.ToString();
            if (e.CommandName == "View")
                ViewPO(arg);
            else if (e.CommandName == "Print")
                PrintPO(arg);
            else if (e.CommandName == "Approve")
                ApprovePO(arg);
            else if (e.CommandName == "Reject")
                RejectPO(arg);
            else if (e.CommandName == "CancelPO")
                CancelPO(arg);
            else if (e.CommandName == "Close")
                ClosePO(arg);
        }

        protected void ddlPages_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow gvrPager = gvPurchaseOrder.BottomPagerRow;
            DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages");
            gvPurchaseOrder.PageIndex = ddlPages.SelectedIndex;
            // a method to populate your grid
            BindData();
        }

        protected void gvPurchaseOrder_DataBound(object sender, EventArgs e)
        {
            GridViewRow gvrPager = gvPurchaseOrder.BottomPagerRow;
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
                for (int i = 0; i < gvPurchaseOrder.PageCount; i++)
                {
                    int intPageNumber = i + 1;
                    ListItem lstItem = new ListItem(intPageNumber.ToString());
                    if (i == gvPurchaseOrder.PageIndex)
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
                DataSet ds = gvPurchaseOrder.DataSource as DataSet;
                if (ds != null)
                {
                    itemCount = ds.Tables[0].Rows.Count;
                }

                string pageCount = "<b>" + gvPurchaseOrder.PageCount.ToString() + "</b>";
                lblPageCount.Text = pageCount;
            }

            Button btnPrev = (Button)gvrPager.Cells[0].FindControl("btnPrev");
            Button btnNext = (Button)gvrPager.Cells[0].FindControl("btnNext");
            Button btnFirst = (Button)gvrPager.Cells[0].FindControl("btnFirst");
            Button btnLast = (Button)gvrPager.Cells[0].FindControl("btnLast");

            //now figure out what page we're on
            if (gvPurchaseOrder.PageIndex == 0)
            {
                btnPrev.Enabled = false;
                btnFirst.Enabled = false;
            }
            else if (gvPurchaseOrder.PageIndex + 1 == gvPurchaseOrder.PageCount)
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

        protected void gvPurchaseOrder_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvPurchaseOrder.PageIndex = e.NewPageIndex;
            BindData();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindData();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            gvPurchaseOrder.DataSource = null;
            gvPurchaseOrder.DataBind();
            LoadDrops();

        }

        protected void lnkExport_Click(object sender, EventArgs e)
        {
            BindData();
            DataTable dt = (DataTable)gvPurchaseOrder.DataSource;
            CommonWebUILib.ExportCSV(dt, this.Page.Title.Trim(' '), this.Page.Title, gvPurchaseOrder);
        }

        protected void gvPurchaseOrder_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[10].Visible = false;
                e.Row.Cells[11].Visible = false;
                e.Row.Cells[12].Visible = false;
                e.Row.Cells[13].Visible = false;
                e.Row.Cells[14].Visible = false;
                e.Row.Cells[15].ColumnSpan = 6;
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                DataTable dt = (DataTable)gvPurchaseOrder.DataSource;
                if (dt != null)
                {
                    decimal GrantTotalAmount = decimal.Parse(dt.Compute("SUM(TotalAmount)", "").ToString());
                    e.Row.Cells[9].Text = GrantTotalAmount.ToString("N2");
                }
            }
        }
        #endregion
    }
}
