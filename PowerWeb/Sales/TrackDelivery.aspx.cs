using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SubSonic;
using PowerPOS;
using PowerPOS.Container;

namespace PowerWeb.Sales
{
    public partial class TrackDelivery : System.Web.UI.Page
    {
        private const string SORT_DIRECTION = "SORT_DIRECTION";
        private const string ORDER_BY = "ORDER_BY";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
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

            string orderDetID = Request.QueryString["RefNo"];

            OrderDet od = new OrderDet(orderDetID);
            if (od != null && od.OrderDetID == orderDetID)
            {
                litItem.Text = od.ItemNo + " - " + od.Item.ItemName;
                litInvNo.Text = od.OrderHdr.Userfld5;

                string sql = "select ROW_NUMBER() over(order by d.Orderdetid asc) as DeliveryNo, " +
                                "convert(varchar(50), h.deliverydate, 106) as DeliveryDate, d.Quantity as DeliveryQty, " +
                                "Case isnull(IsDelivered, 0) when 1 then 'Delivered' else 'Not Delivered' end as DeliveryStatus, d.OrderDetID,  d.DOHdrID " +
                                "from deliveryorderdetails d inner join deliveryorder h on d.DOHDRID = h.OrderNumber " +
                                "where ISNULL(d.Deleted,0) = 0 AND ISNULL(h.Deleted,0) = 0 AND d.OrderDetID = '" + orderDetID + "'";

                DataSet ds = DataService.GetDataSet(new QueryCommand(sql, "PowerPOS"));
                gvReport.DataSource = ds.Tables[0];
                gvReport.DataBind();
            }
        }

        protected void gvReport_DataBound(object sender, EventArgs e)
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

        protected void gvReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvReport.PageIndex = e.NewPageIndex;
            BindGrid();
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

        protected void gvReport_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Delivered")
                {
                    string doHdrID = e.CommandArgument.ToString();
                    DeliveryOrder dohdr = new DeliveryOrder(doHdrID);

                    if (dohdr.IsDelivered == null || (bool)dohdr.IsDelivered == false)
                    {
                        Query qr = new Query("DeliveryOrder");
                        qr.QueryType = QueryType.Update;
                        qr.AddWhere(DeliveryOrder.Columns.OrderNumber, doHdrID);
                        qr.AddUpdateSetting(DeliveryOrder.Columns.IsDelivered, true);
                        qr.AddUpdateSetting(DeliveryOrder.Columns.ModifiedOn, DateTime.Now);
                        qr.AddUpdateSetting(DeliveryOrder.Columns.ModifiedBy, Session["UserName"] ?? "");

                        DataService.ExecuteQuery(qr.BuildUpdateCommand());
                        BindGrid();
                        litSuccessMsg.Text = "Successfully marked as Delivered";
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                BindGrid();
                litErrorMsg.Text = "Error occured: " + ex.Message;
            }
        }
    }
}
