using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using PowerPOS;
using SubSonic;
using System.Collections;
using PowerPOS.Container;
using POSDevices;

namespace PowerWeb.Reports
{
    public partial class PreOrderReport : PageBase
    {
        private const string SORT_DIRECTION = "SORT_DIRECTION";
        private const string ORDER_BY = "ORDER_BY";
        private const string OnlyReadyToDeliver = "OnlyReadyToDeliver";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ddlOutlet.Items.AddRange(PointOfSaleController.FetchOutletNames(false, Session["UserName"] + ""));
                ddlOutlet.DataBind();
                List<ListItem> listYear = new List<ListItem>();
                int minyear = UtilityController.GetMinOrderDateYear();
                for (int i = DateTime.Today.Year; i >= minyear; i--)
                {
                    listYear.Add(new ListItem(i.ToString(), i.ToString()));
                }
                ddlYear.Items.AddRange(listYear.ToArray());
                ddlYear.SelectedIndex = 0;
                ddlMonth.SelectedValue = DateTime.Today.Month.ToString();
                txtStartDate.Text = DateTime.Today.ToString("dd MMM yyyy");
                txtEndDate.Text = DateTime.Today.ToString("dd MMM yyyy");
                ViewState[ORDER_BY] = "";
                ViewState[SORT_DIRECTION] = "";
                ViewState[OnlyReadyToDeliver] = false;

                BindGrid();
            }
        }

        private void BindGrid()
        {
            litSuccessMsg.Text = "";
            litErrorMsg.Text = "";

            if (ViewState[ORDER_BY] == null)
                ViewState[ORDER_BY] = "";

            if (ViewState[SORT_DIRECTION] == null)
                ViewState[SORT_DIRECTION] = "";

            DateTime startDate = DateTime.Parse(txtStartDate.Text);
            DateTime endDate = DateTime.Parse(txtEndDate.Text);

            DataTable dt = ReportController.FetchPreOrderReport(startDate, endDate.AddSeconds(86399), 
                txtItem.Text, "", txtCustName.Text, rblOutstandingBal.SelectedValue, rblNotification.SelectedValue,
                (bool)ViewState[OnlyReadyToDeliver], ViewState[ORDER_BY].ToString(), ViewState[SORT_DIRECTION].ToString(),ddlOutlet.SelectedItem.Text, rblStatus.SelectedValue);
            dt = CommonUILib.DataTableConvertBoolColumnToYesOrNo(dt);
            dt.Columns.Add("BalanceQty", Type.GetType("System.Int32"), "QtyOnHand - TotalPreOrderQty");
            gvReport.DataSource = dt;
            gvReport.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            ViewState[ORDER_BY] = null;
            ViewState[SORT_DIRECTION] = null;
            ViewState[OnlyReadyToDeliver] = false;

            if (rdbMonth.Checked)
            {
                txtStartDate.Text = (new DateTime(int.Parse(ddlYear.Text), int.Parse(ddlMonth.SelectedValue), 1)).ToString("dd MMM yyyy");
                txtEndDate.Text = new DateTime(int.Parse(ddlYear.Text), int.Parse(ddlMonth.SelectedValue), DateTime.DaysInMonth(int.Parse(ddlYear.Text), int.Parse(ddlMonth.SelectedValue))).ToString("dd MMM yyyy");
            }

            BindGrid();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtStartDate.Text = DateTime.Today.AddDays(-1).ToString("dd MMM yyyy");
            txtEndDate.Text = DateTime.Today.AddDays(-1).ToString("dd MMM yyyy");
            ddlMonth.SelectedValue = DateTime.Today.Month.ToString();
            ddlYear.SelectedIndex = 0;
            rdbRange.Checked = true;
            rdbMonth.Checked = false;
            txtItem.Text = "";
            txtCustName.Text = "";
            rblOutstandingBal.SelectedIndex = 0;
            rblNotification.SelectedIndex = 0;

            BindGrid();
            gvReport.PageIndex = 0;
        }

        protected void lnkExport_Click(object sender, EventArgs e)
        {
            BindGrid();
            DataTable dt = (DataTable)gvReport.DataSource;
            CommonWebUILib.ExportCSV(dt, this.Page.Title.Trim(' '), this.Page.Title, gvReport);
        }

        protected void lnkReadyToDeliver_Click(object sender, EventArgs e)
        {
            ViewState[ORDER_BY] = null;
            ViewState[SORT_DIRECTION] = null;
            ViewState[OnlyReadyToDeliver] = true;

            if (rdbMonth.Checked)
            {
                txtStartDate.Text = (new DateTime(int.Parse(ddlYear.Text), int.Parse(ddlMonth.SelectedValue), 1)).ToString("dd MMM yyyy");
                txtEndDate.Text = new DateTime(int.Parse(ddlYear.Text), int.Parse(ddlMonth.SelectedValue), DateTime.DaysInMonth(int.Parse(ddlYear.Text), int.Parse(ddlMonth.SelectedValue))).ToString("dd MMM yyyy");
            }

            BindGrid();
        }

        protected void lnkDeliver_Click(object sender, EventArgs e)
        {
            List<string> errorList = new List<string>();
            List<string> successList = new List<string>();
            foreach (GridViewRow gridRow in gvReport.Rows)
            {
                if (((CheckBox)gridRow.FindControl("chkSelect")).Checked)
                {
                    bool proceed = true;
                    string orderHdrID = ((Label)gridRow.FindControl("lblOrderHdrID")).Text;
                    string orderDetID = ((Label)gridRow.FindControl("lblOrderDetID")).Text;
                    //string orderHdrID = gridRow.Cells[13].Text;
                    int deliverQty;
                    int.TryParse(((TextBox)gridRow.FindControl("txtDeliveryQty")).Text, out deliverQty);

                    int onHandQty = int.Parse(((Label)gridRow.FindControl("lblQtyOnHand")).Text);
                    int outstandingQty = int.Parse(((Label)gridRow.FindControl("lblOutstandingQty")).Text);
                    string itemName = ((Label)gridRow.FindControl("lblItemName")).Text;
                    string invoiceNo = ((Label)gridRow.FindControl("lblInvoiceNo")).Text;
                    string deliveryStatus = ((Label)gridRow.FindControl("lblDeliveryStatus")).Text;

                    if (deliverQty <= 0)
                    {
                        errorList.Add(string.Format("Delivery quantity for item {0} (Preorder# {1}) cannot be zero or negative.", itemName, invoiceNo));
                        proceed = false;
                    }

                    if (proceed && onHandQty <= 0)
                    {
                        errorList.Add(string.Format("On Hand Quantity for item {0} (Preorder# {1}) is {2}. You can not do delivery", itemName, invoiceNo, onHandQty.ToString()));
                        proceed = false;
                    }

                    if (proceed && deliveryStatus == "Delivered")
                    {
                        errorList.Add(string.Format("Delivery Status for item {0} (Preorder# {1}) is {2}. You can not do delivery", itemName, invoiceNo, deliveryStatus));
                        proceed = false;
                    }

                    if (proceed && outstandingQty <= 0)
                    {
                        errorList.Add(string.Format("Outstanding quantity for item {0} (Preorder# {1}) is {2}. You can not do delivery", itemName, invoiceNo, outstandingQty.ToString()));
                        proceed = false;
                    }

                    if (proceed && deliverQty > outstandingQty)
                    {
                        errorList.Add(string.Format("Delivery quantity for item {0} (Preorder# {1}) is greater than outstanding quantity.", itemName, invoiceNo));
                        proceed = false;
                    }

                    if (proceed)
                    {
                        #region *) Create Delivery Order
                        POSController pos = new POSController();

                        int deliverypersonid = -1;
                        if (!pos.CreateDeliveryPreOrderSingleOrderDet(orderDetID, deliverQty, deliverypersonid, true))
                        {
                            errorList.Add(string.Format("Error when creating Delivery Order for item {0} (Preorder# {1})", itemName, invoiceNo));
                        }
                        else
                        {
                            successList.Add(string.Format("Delivery Order for item {0} (Preorder# {1}) is created successfully", itemName, invoiceNo));

                            #region *) do stock out
                            //string status;
                            //PointOfSaleInfo.IntegrateWithInventory = true;
                            //if (!pos.DoStockOutPreOrderSingle(orderDetID, deliverQty, out status))
                            //{
                            //    errorList.Add(string.Format("Error when doing stock out for item {0} (Preorder# {1})", itemName, invoiceNo));
                            //}

                            if (!InventoryController.AssignStockOutToPreOrderSalesUsingTransaction())
                            {
                                errorList.Add(string.Format("Error when doing stock out for item {0} (Preorder# {1})", itemName, invoiceNo));
                            }
                            #endregion
                        }
                        #endregion
                    }
                }
            }

            BindGrid();
            litSuccessMsg.Text = string.Join("<br/>", successList.ToArray());
            litErrorMsg.Text = string.Join("<br/>", errorList.ToArray());
        }

        protected void lnkNotify_Click(object sender, EventArgs e)
        {
            List<string> errorList = new List<string>();
            List<string> successList = new List<string>();
            foreach (GridViewRow gridRow in gvReport.Rows)
            {
                if (((CheckBox)gridRow.FindControl("chkSelect")).Checked)
                {
                    bool proceed = true;
                    string itemName = ((Label)gridRow.FindControl("lblItemName")).Text;
                    string invoiceNo = ((Label)gridRow.FindControl("lblInvoiceNo")).Text;
                    string orderHdrID = ((Label)gridRow.FindControl("lblOrderHdrID")).Text;
                    string orderDetID = ((Label)gridRow.FindControl("lblOrderDetID")).Text;
                    int onHandQty = int.Parse(((Label)gridRow.FindControl("lblQtyOnHand")).Text);
                    int outstandingQty = int.Parse(((Label)gridRow.FindControl("lblOutstandingQty")).Text);
                    string deliveryStatus = ((Label)gridRow.FindControl("lblDeliveryStatus")).Text;

                    if (onHandQty <= 0)
                    {
                        errorList.Add(string.Format("On Hand Quantity for item {0} (Preorder# {1}) is {2}. You can not do delivery", itemName, invoiceNo, onHandQty.ToString()));
                        proceed = false;
                    }

                    if (proceed && deliveryStatus == "Delivered")
                    {
                        errorList.Add(string.Format("Delivery Status for item {0} (Preorder# {1}) is {2}. You can not do delivery", itemName, invoiceNo, deliveryStatus));
                        proceed = false;
                    }

                    if (proceed && outstandingQty <= 0)
                    {
                        errorList.Add(string.Format("Outstanding quantity for item {0} (Preorder# {1}) is {2}. You can not do delivery", itemName, invoiceNo, outstandingQty.ToString()));
                        proceed = false;
                    }

                    if (proceed)
                    {
                        POSController pos = new POSController();
                        OrderHdr oh = new OrderHdr(orderHdrID);
                        OrderDetCollection odcol = new OrderDetCollection();
                        OrderDet od = new OrderDet(orderDetID);
                        odcol.Add(od);

                        var member = new Membership(oh.MembershipNo);
                        pos.myOrderHdr = oh;
                        pos.myOrderDet = odcol;
                        PointOfSaleController.GetPointOfSaleInfo();

                        string EmailTo = "";
                        string EmailSubject = "";
                        string EmailBody = "";
                        string EmailBcc = "";
                        if (member != null)
                        {
                            if (!string.IsNullOrEmpty(member.Email))
                                EmailTo = member.Email;
                            else
                                EmailTo = AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_DefaultMailTo);

                            string useForReceiptNo = AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_ReceiptNoInEmailReceipt);
                            string receiptNo = "";
                            if (string.IsNullOrEmpty(useForReceiptNo) || useForReceiptNo.ToLower() == "orderhdrid")
                                receiptNo = oh.OrderHdrID;
                            else if (useForReceiptNo.ToLower() == "custom invoice no")
                                receiptNo = oh.Userfld5;
                            else if (useForReceiptNo.ToLower() == "line info")
                                receiptNo = od.LineInfo;
                            else
                                receiptNo = oh.OrderHdrID;

                            EmailSubject = string.Format("Receipt {0} for purchase at {1}", receiptNo, CompanyInfo.CompanyName);
                            EmailBody = "Please find the receipt attachment";
                        }

                        #region *) Send BCC if necessary
                        bool sendBcc = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_BccToOwner), false);
                        string ownerEmail = AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_OwnerEmailAddress);
                        if (sendBcc && !string.IsNullOrEmpty(ownerEmail))
                        {
                            EmailBcc = ownerEmail;
                        }
                        #endregion

                        if (POSDeviceController.SendMailNotifyDelivery(pos, EmailTo, EmailSubject, EmailBody, EmailBcc))
                            successList.Add(string.Format("Notification Email has been successfully sent for item {0} (Preorder# {1}).", itemName, invoiceNo));
                        else
                            errorList.Add(string.Format("Failed to send Notification Email for item {0} (Preorder# {1}).", itemName, invoiceNo));
                    }
                }
            }

            BindGrid();
            litSuccessMsg.Text = string.Join("<br/>", successList.ToArray());
            litErrorMsg.Text = string.Join("<br/>", errorList.ToArray());
        }

        /// <summary>
        /// To set the DeliveryOrder.IsDelivered to true
        /// </summary>
        protected void lnkDelivered_Click(object sender, EventArgs e)
        {
            List<string> errorList = new List<string>();
            List<string> successList = new List<string>();
            foreach (GridViewRow gridRow in gvReport.Rows)
            {
                if (((CheckBox)gridRow.FindControl("chkSelect")).Checked)
                {
                    bool proceed = true;
                    string orderDetID = ((Label)gridRow.FindControl("lblOrderDetID")).Text;
                    string itemName = ((Label)gridRow.FindControl("lblItemName")).Text;
                    string invoiceNo = ((Label)gridRow.FindControl("lblInvoiceNo")).Text;
                    string deliveryStatus = ((Label)gridRow.FindControl("lblDeliveryStatus")).Text;

                    if (deliveryStatus != "Pending")
                    {
                        errorList.Add(string.Format("Delivery status for item {0} (Preorder# {1}) is {2}. You can not set the status to Delivered", itemName, invoiceNo, deliveryStatus));
                        proceed = false;
                    }

                    if (proceed)
                    {
                        #region *) SQL String
                        string sql = @"
                                        UPDATE do
                                        SET do.IsDelivered = 1, do.ModifiedBy = @ModifiedBy, do.ModifiedOn = GETDATE()
                                        FROM DeliveryOrderDetails dod
                                            INNER JOIN DeliveryOrder do ON do.OrderNumber = dod.DOHDRID
                                        WHERE dod.OrderDetID = @OrderDetID
                                            AND ISNULL(dod.Deleted, 0) = 0 AND ISNULL(do.Deleted, 0) = 0
                                            AND ISNULL(do.IsDelivered, 0) = 0
                                     ";
                        #endregion
                        QueryCommand cmd = new QueryCommand(sql, "PowerPOS");
                        cmd.Parameters.Add("@ModifiedBy", UserInfo.username, DbType.String);
                        cmd.Parameters.Add("@OrderDetID", orderDetID, DbType.String);
                        DataService.ExecuteQuery(cmd);
                    }
                }
            }

            BindGrid();
            litSuccessMsg.Text = string.Join("<br/>", successList.ToArray());
            litErrorMsg.Text = string.Join("<br/>", errorList.ToArray());
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
            ViewState[ORDER_BY] = e.SortExpression;
            //rebind the grid
            if (ViewState[SORT_DIRECTION] == null || ((string)ViewState[SORT_DIRECTION]) == "ASC")
            {
                ViewState[SORT_DIRECTION] = "DESC";
            }
            else
            {
                ViewState[SORT_DIRECTION] = "ASC";
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

        protected void gvReport_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    e.Row.Cells[AMOUNT_WITH_GST].Text = String.Format("{0:N2}", Decimal.Parse(e.Row.Cells[AMOUNT_WITH_GST].Text));
            //    //e.Row.Cells[GSTAMOUNT].Text = String.Format("{0:N2}", Decimal.Parse(e.Row.Cells[GSTAMOUNT].Text));
            //    //e.Row.Cells[AMOUNT].Text = String.Format("{0:N2}", Decimal.Parse(e.Row.Cells[AMOUNT].Text));            
            //    //e.Row.Cells[COGS].Text = String.Format("{0:N2}", Decimal.Parse(e.Row.Cells[COGS].Text));
            //    /*
            //    decimal tmp = Decimal.Parse(e.Row.Cells[PLPercent].Text);
            //    if (tmp >= -100 && tmp <= 100)
            //    {
            //        e.Row.Cells[PLPercent].Text = tmp.ToString("N2") + "%";
            //    }
            //    else
            //    {
            //        e.Row.Cells[PLPercent].Text = "ERR";
            //    }
            //    e.Row.Cells[PL].Text = String.Format("{0:N2}", Decimal.Parse(e.Row.Cells[PL].Text));            
            //     */
            //}
            //else if (e.Row.RowType == DataControlRowType.Footer)
            //{
            //    DataTable dt = (DataTable)gvReport.DataSource;
            //    if (dt != null)
            //    {
            //        decimal Amount, PLAmt, AmountWithoutGST, GSTAmount;
            //        e.Row.Cells[QTY].Text = dt.Compute("SUM(TOTALQuantity)", "").ToString();
            //        Amount = decimal.Parse(dt.Compute("SUM(TOTALAMOUNT)", "").ToString());
            //        e.Row.Cells[AMOUNT_WITH_GST].Text =  Amount.ToString("N2");
            //        AmountWithoutGST = decimal.Parse(dt.Compute("SUM(TOTALAMOUNTWITHOUTGST)", "").ToString());
            //        //e.Row.Cells[AMOUNT].Text =  AmountWithoutGST.ToString("N2");
            //        GSTAmount = decimal.Parse(dt.Compute("SUM(GSTAMOUNT)", "").ToString());
            //        e.Row.Cells[GSTAMOUNT].Text =  GSTAmount.ToString("N2");
            //        e.Row.Cells[COGS].Text =  decimal.Parse(dt.Compute("SUM(TotalCOSTOFGOODSSOLD)", "").ToString()).ToString("N2");
            //        /*
            //        PLAmt = decimal.Parse(dt.Compute("SUM(PROFITLOSS)", "").ToString());
            //        e.Row.Cells[PL].Text =  PLAmt.ToString("N2") + "%";
            //        e.Row.Cells[PLPercent].Text = (PLAmt / Amount * 100).ToString("N2") + "%";
            //         * */
            //    }
            //}
        }
    }
}
