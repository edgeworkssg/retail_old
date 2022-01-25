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
using SubSonic;
using SubSonic.Utilities;
using PowerPOS;
using CrystalDecisions.CrystalReports.Engine;

namespace PowerPOS
{
    public partial class PrintDeliveryOrder : PageBase
    {
        private const string SORT_DIRECTION = "SORT_DIRECTION";
        private const string ORDER_BY = "ORDER_BY";
        ReportDocument rpt;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                txtSearchDateFrom.Text = DateTime.Today.ToString("dd MMM yyyy");
                txtSearchDateTo.Text = DateTime.Today.ToString("dd MMM yyyy");

                MultiView1.ActiveViewIndex = 0;
            }

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
                    ToggleEditor(true);
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

            Bind_CRViewer();
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            if (rpt != null)
            {
                rpt.Close();
                rpt.Dispose();
                GC.Collect();
            }
        }

        /// <summary>
        /// Loads the editor with data
        /// </summary>
        /// <param name="id"></param>
        void LoadEditor(string id)
        {
            ToggleEditor(true);
            if (!String.IsNullOrEmpty(id) && id != "0")
            {
                lblID.Text = id.ToString();

                //pull the record
                DeliveryController doCtrl = new DeliveryController(id);
                DeliveryOrder item = doCtrl.myDeliveryOrderHdr;
                //bind the page 

                // Get OrderHdr
                OrderHdr orderHdr = new OrderHdr(item.SalesOrderRefNo);

                ctrlInvoiceNo.Text = orderHdr.Userfld5;
                ctrlOutlet.Text = orderHdr.PointOfSale.OutletName;
                ctrlInvoiceDate.Text = orderHdr.OrderDate.ToString("dd MMM yyyy HH:mm:ss");
                ctrlPurchaseOrderRefNo.Text = item.PurchaseOrderRefNo;
                ctrlRecipientName.Text = item.RecipientName;
                ctrlMobileNo.Text = item.MobileNo;
                ctrlHomeNo.Text = item.HomeNo;
                ctrlPostalCode.Text = item.PostalCode;
                ctrlDeliveryAddress.Text = item.DeliveryAddress;
                ctrlUnitNo.Text = item.UnitNo;
                if (item.DeliveryDate.HasValue)
                {
                    ctrlDeliveryDate.Text = item.DeliveryDate.Value.ToString("dd MMM yyyy");
                }
                if (item.TimeSlotFrom.HasValue && item.TimeSlotTo.HasValue)
                {
                    ctrlDeliveryTime.Text = item.TimeSlotFrom.Value.ToString("htt") + " - " + item.TimeSlotTo.Value.ToString("htt");
                }
                ctrlRemark.Text = item.Remark;
                ctrlBalancePayment.Text =  Installment.GetOutstandingBalance(item.SalesOrderRefNo, DateTime.Now).ToString("0.00");

                // Bind the Details
                GridView2.DataSource = doCtrl.FetchDeliveryItems();
                GridView2.DataBind();
            }
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

        /// <summary>
        /// Binds the GridView
        /// </summary>
        private void BindGrid(string orderBy)
        {
            //string sql = "SELECT do.*, oh.OrderRefNo, oh.userfld5 AS CustomInvoiceNo, oh.OrderDate, pos.PointOfSaleName, pos.OutletName " +
            //             "FROM DeliveryOrder do " +
            //             "     INNER JOIN OrderHdr oh ON do.SalesOrderRefNo = oh.OrderHdrID " +
            //             "     INNER JOIN PointOfSale pos ON oh.PointOfSaleID = pos.PointOfSaleID " +
            //             "     INNER JOIN Membership mbr ON do.MembershipNo = mbr.MembershipNo " +
            //             "WHERE ISNULL(do.Deleted, 0) = 0 " +
            //             "  AND do.DeliveryDate BETWEEN @DateFrom AND @DateTo ";

            string sql = @"SELECT do.*, oh.OrderRefNo, oh.userfld5 AS CustomInvoiceNo, oh.OrderDate, pos.PointOfSaleName, pos.OutletName, 
                                  ISNULL(do.DeliveryAddress,'') AS DeliveryAddressFull, 
                                  CONVERT(varchar(20), do.DeliveryDate, 103) + ' ' + ISNULL(REPLACE(RIGHT(do.TimeSlotFrom, 7), ':00', '') + ' - ' + REPLACE(RIGHT(do.TimeSlotTo, 7), ':00', ''), '') AS DeliveryDateTime, 
                                  dbo.GetInstallmentOutstandingBalance(oh.OrderHdrID, GETDATE()) AS BalancePayment
                           FROM DeliveryOrder do 
                                INNER JOIN OrderHdr oh ON do.SalesOrderRefNo = oh.OrderHdrID 
                                INNER JOIN PointOfSale pos ON oh.PointOfSaleID = pos.PointOfSaleID 
                                INNER JOIN Membership mbr ON do.MembershipNo = mbr.MembershipNo 
                                LEFT JOIN PostalCodeDB pcd ON pcd.ZIP = do.PostalCode
                           WHERE ISNULL(do.Deleted, 0) = 0 AND ISNULL(oh.IsVoided, 0) = 0 
                             AND do.DeliveryDate BETWEEN @DateFrom AND @DateTo 
                           ";
            if (!string.IsNullOrEmpty(orderBy))
            {
                sql += " ORDER BY " + orderBy + ViewState[SORT_DIRECTION];
            }
            QueryCommand cmd = new QueryCommand(sql, "PowerPOS");

            DateTime dateFrom;
            DateTime dateTo;

            try
            { dateFrom = System.Data.SqlTypes.SqlDateTime.Parse(txtSearchDateFrom.Text).Value; }
            catch
            { dateFrom = System.Data.SqlTypes.SqlDateTime.MinValue.Value; }

            try
            { dateTo = System.Data.SqlTypes.SqlDateTime.Parse(txtSearchDateTo.Text).Value; }
            catch
            { dateTo = System.Data.SqlTypes.SqlDateTime.MaxValue.Value; }

            if (dateTo.TimeOfDay.ToString() == "00:00:00") dateTo = dateTo.Add(TimeSpan.Parse("23:59:59"));
            cmd.AddParameter("@DateFrom", dateFrom, DbType.DateTime);
            cmd.AddParameter("@DateTo", dateTo, DbType.DateTime);

            DataTable dt = DataService.GetDataSet(cmd).Tables[0];
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

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;

                HiddenField hidOrderNumber = (HiddenField)e.Row.FindControl("hidOrderNumber");
                hidOrderNumber.Value = drv["OrderNumber"].ToString();

                e.Row.Cells[1].Text = string.Format("<a href='PrintDeliveryOrder.aspx?id={0}'>{1}</a>", drv["OrderNumber"].ToString(), drv["PurchaseOrderRefNo"].ToString());
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGrid(string.Empty);
        }

        protected void btnPrintSelected_Click(object sender, EventArgs e)
        {
            List<string> orderNumbers = new List<string>();            
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                GridViewRow gvr = GridView1.Rows[i];
                if (gvr.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkTicked = (CheckBox)gvr.FindControl("chkTicked");
                    HiddenField hidOrderNumber = (HiddenField)gvr.FindControl("hidOrderNumber");
                    if (chkTicked.Checked)
                    {
                        orderNumbers.Add(hidOrderNumber.Value);
                    }
                }
            }

            if (orderNumbers.Count > 0)
            {
                ViewState["OrderNumbers"] = orderNumbers;
                Bind_CRViewer();
                MultiView1.ActiveViewIndex = 1;
            }
        }

        protected void btnPrintAll_Click(object sender, EventArgs e)
        {
            List<string> orderNumbers = new List<string>();
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                GridViewRow gvr = GridView1.Rows[i];
                if (gvr.RowType == DataControlRowType.DataRow)
                {
                    HiddenField hidOrderNumber = (HiddenField)gvr.FindControl("hidOrderNumber");
                    orderNumbers.Add(hidOrderNumber.Value);
                }
            }

            if (orderNumbers.Count > 0)
            {
                ViewState["OrderNumbers"] = orderNumbers;
                Bind_CRViewer();
                MultiView1.ActiveViewIndex = 1;
            }
        }

        private void Bind_CRViewer()
        {
            if (ViewState["OrderNumbers"] == null)
                return;

            List<string> orderNumbers = (List<string>)ViewState["OrderNumbers"];

            DataTable dtDelivery = null;
            foreach (string orderNumber in orderNumbers)
            {
                DataTable dt = DeliveryController.FetchDeliveryOrderToPrint(orderNumber);
                if (dtDelivery == null)
                {
                    dtDelivery = dt;
                }
                else
                {
                    dtDelivery.Merge(dt);
                }
            }

            string ReportName = Server.MapPath("~\\bin\\Reports\\Mayer_DeliveryOrderRpt.rpt");
            rpt = new ReportDocument();
            rpt.Load(ReportName);
            rpt.SetDataSource(dtDelivery);
            CrystalReportViewer1.ReportSource = rpt;
            CrystalReportViewer1.RefreshReport();
        }

        protected void btnDone_Click(object sender, EventArgs e)
        {
            ViewState["OrderNumbers"] = null;
            MultiView1.ActiveViewIndex = 0;
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                lblResult.Text = GridView1.Rows.Count.ToString();
                BindGrid(String.Empty);
                DataTable dt = (DataTable)GridView1.DataSource;
                CommonWebUILib.ExportCSV(dt, this.Page.Title.Trim(' '), this.Page.Title, GridView1);
            }
            catch (Exception ex)
            {
                lblResult.Text = ex.Message;
            }
        }
    }
}
