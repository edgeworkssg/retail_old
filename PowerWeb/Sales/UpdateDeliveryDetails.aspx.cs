
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

namespace PowerPOS
{
    public partial class UpdateDeliveryDetails : PageBase
    {
        private bool isAdd = false;
        private const string SORT_DIRECTION = "SORT_DIRECTION";
        private const string ORDER_BY = "ORDER_BY";
        private DataTable DeliveryTimes;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                txtSearchInvDateFrom.Text = DateTime.Today.ToString("dd MMM yyyy");
                txtSearchInvDateTo.Text = DateTime.Today.ToString("dd MMM yyyy");
                txtSearchDODateFrom.Text = DateTime.Today.ToString("dd MMM yyyy");
                txtSearchDODateTo.Text = DateTime.Today.ToString("dd MMM yyyy");
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
                    //it's an add, show the editor
                    isAdd = true;
                    ToggleEditor(true);
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
                    string timeFrom = item.TimeSlotFrom.Value.ToString("HH:mm");
                    string timeTo = item.TimeSlotTo.Value.ToString("HH:mm");

                    DataRow[] dr = DeliveryTimes.Select(string.Format("Value='{0}-{1}'", timeFrom, timeTo));
                    if (dr.Length > 0)
                    {
                        ctrlDeliveryTime.SelectedValue = dr[0]["Value"].ToString();
                    }
                    else
                    {
                        // If not registered in DeliveryTimes datatable, then add new list to the combobox
                        string value = timeFrom + "-" + timeTo;
                        string text = item.TimeSlotFrom.Value.ToString("h:mmtt").ToLower().Replace(":00", "") +
                                      " - " +
                                      item.TimeSlotTo.Value.ToString("h:mmtt").ToLower().Replace(":00", "");
                        ctrlDeliveryTime.Items.Add(new ListItem(text, value));
                        ctrlDeliveryTime.SelectedValue = value;
                    }

                    //if (item.TimeSlotFrom.Value.Hour == 10 && item.TimeSlotTo.Value.Hour == 13)
                    //{
                    //    ctrlDeliveryTime.SelectedIndex = 1;
                    //}
                    //else if (item.TimeSlotFrom.Value.Hour == 12 && item.TimeSlotTo.Value.Hour == 15)
                    //{
                    //    ctrlDeliveryTime.SelectedIndex = 2;
                    //}
                    //else if (item.TimeSlotFrom.Value.Hour == 14 && item.TimeSlotTo.Value.Hour == 17)
                    //{
                    //    ctrlDeliveryTime.SelectedIndex = 3;
                    //}
                    //else
                    //{
                    //    ctrlDeliveryTime.SelectedIndex = 0;
                    //}
                }
                else
                {
                    ctrlDeliveryTime.SelectedIndex = 0;
                }
                ctrlRemark.Text = item.Remark;

                bool allowChanges = true;
                if (AppSetting.GetSetting("DO_AllowChangesForPastDeliveryDate") == null)
                {
                    AppSetting.SetSetting("DO_AllowChangesForPastDeliveryDate", "True");
                }
                allowChanges = AppSetting.CastBool(AppSetting.GetSetting("DO_AllowChangesForPastDeliveryDate"), true);

                // If Delivery Date has passed, don't allow changes to be made
                if (item.DeliveryDate < DateTime.Today && !allowChanges)
                {
                    ctrlRecipientName.Enabled = false;
                    ctrlMobileNo.Enabled = false;
                    ctrlHomeNo.Enabled = false;
                    ctrlPostalCode.Enabled = false;
                    btnPostalCode.Enabled = false;
                    ctrlDeliveryAddress.Enabled = false;
                    ctrlUnitNo.Enabled = false;
                    ctrlDeliveryDate.Enabled = false;
                    ctrlDeliveryTime.Enabled = false;
                    ImageButton1.Enabled = false;
                    ctrlRemark.Enabled = false;

                    btnSave.Visible = false;
                    btnDelete.Visible = false;
                }

                // Bind the Details
                GridView2.DataSource = doCtrl.FetchDeliveryItems();
                GridView2.DataBind();

                //set the delete confirmation
                btnDelete.Attributes.Add("onclick", "return CheckCancelDelivery();");
            }
        }
        /// <summary>
        /// Loads the DropDownLists
        /// </summary>
        void LoadDrops()
        {
            //load the listboxes

            //Query qryctrlMembershipNo = Membership.CreateQuery();
            //qryctrlMembershipNo.OrderBy = OrderBy.Asc("MembershipGroupId");
            //Utility.LoadDropDown(ctrlMembershipNo, qryctrlMembershipNo.ExecuteReader(), true);
            //ctrlMembershipNo.Items.Insert(0, new ListItem("(Not Specified)", String.Empty));

            #region *) Load Delivery Time DropDownList
            string path = Server.MapPath("~/DeliveryTime.xml");
            if (System.IO.File.Exists(path))
            {
                DataSet ds = new DataSet();
                ds.ReadXml(path);
                DeliveryTimes = ds.Tables[0];
            }
            else
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Value", Type.GetType("System.String"));
                dt.Columns.Add("Text", Type.GetType("System.String"));
                dt.Rows.Add("10:00-13:00", "10am - 1pm");
                dt.Rows.Add("13:00-15:00", "1pm - 3pm");
                dt.Rows.Add("15:00-17:00", "3pm - 5pm");
                DeliveryTimes = dt;
            }

            ctrlDeliveryTime.DataSource = DeliveryTimes;
            ctrlDeliveryTime.DataTextField = "Text";
            ctrlDeliveryTime.DataValueField = "Value";
            ctrlDeliveryTime.DataBind();
            ctrlDeliveryTime.Items.Insert(0, "");
            #endregion
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
            DeliveryOrder.Delete(Utility.GetParameter("id"));
            //redirect
            Response.Redirect(Request.CurrentExecutionFilePath);
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string id = Utility.GetParameter("id");
            //bool haveError = false;
            try
            {
                BindAndSave(id);
                lblResult.Text = "<span style=\"font-weight:bold; color:#22bb22\">" + LanguageManager.GetTranslation("Delivery Order saved.") + "</span>";
            }
            catch (Exception x)
            {
                //haveError = true;
                lblResult.Text = "<span style=\"font-weight:bold; color:#990000\">" + LanguageManager.GetTranslation("Delivery Order not saved:") + "</span> " + x.Message;
            }
            //if(!haveError)
            //  Response.Redirect(Request.CurrentExecutionFilePath);
        }
        /// <summary>
        /// Binds and saves the data
        /// </summary>
        /// <param name="id"></param>
        void BindAndSave(string id)
        {

            DeliveryOrder item;
            if (!String.IsNullOrEmpty(id) && id != "0")
            {
                //it's an edit
                item = new DeliveryOrder(id);
            }
            else
            {
                //add
                item = new DeliveryOrder();
            }

            item.RecipientName = ctrlRecipientName.Text.Trim();
            item.MobileNo = ctrlMobileNo.Text.Trim();
            item.HomeNo = ctrlHomeNo.Text.Trim();
            item.PostalCode = ctrlPostalCode.Text.Trim();
            item.DeliveryAddress = ctrlDeliveryAddress.Text.Trim();
            item.UnitNo = ctrlUnitNo.Text.Trim();
            
            DateTime deliveryDate;
            if (DateTime.TryParse(ctrlDeliveryDate.Text.Trim(), out deliveryDate))
            {
                deliveryDate = deliveryDate.Date;   // Make sure we only get the date, not including the time
                item.DeliveryDate = deliveryDate;
            }
            else
            {
                item.DeliveryDate = null;
                deliveryDate = System.Data.SqlTypes.SqlDateTime.MinValue.Value.Date;
                //ctrlDeliveryTime.SelectedIndex = 0;  // Make the time slot null too
            }

            string deliveryTime = ctrlDeliveryTime.SelectedValue;
            if (string.IsNullOrEmpty(deliveryTime))
            {
                item.TimeSlotFrom = null;
                item.TimeSlotTo = null;
            }
            else
            {
                string timeFrom = deliveryTime.Split('-')[0].Trim();
                string timeTo = deliveryTime.Split('-')[1].Trim();
                item.TimeSlotFrom = deliveryDate.Add(TimeSpan.Parse(timeFrom));
                item.TimeSlotTo = deliveryDate.Add(TimeSpan.Parse(timeTo));
            }

            //if (ctrlDeliveryTime.SelectedValue == "10am - 1pm")
            //{
            //    item.TimeSlotFrom = deliveryDate.AddHours(10);
            //    item.TimeSlotTo = deliveryDate.AddHours(13);
            //}
            //else if (ctrlDeliveryTime.SelectedValue == "12pm - 3pm")
            //{
            //    item.TimeSlotFrom = deliveryDate.AddHours(12);
            //    item.TimeSlotTo = deliveryDate.AddHours(15);
            //}
            //else if (ctrlDeliveryTime.SelectedValue == "2pm - 5pm")
            //{
            //    item.TimeSlotFrom = deliveryDate.AddHours(14);
            //    item.TimeSlotTo = deliveryDate.AddHours(17);
            //}
            //else
            //{
            //    item.TimeSlotFrom = null;
            //    item.TimeSlotTo = null;
            //}

            item.Remark = ctrlRemark.Text.Trim();
            item.IsServerUpdate = true;

            //bind it
            item.Save(Session["username"].ToString());
            AccessLogController.AddLog(AccessSource.WEB, (Session["UserName"] + ""), "-", "UPDATE Delivery Order : " + item.OrderNumber, "");
        }
        /// <summary>
        /// Binds the GridView
        /// </summary>
        private void BindGrid(string orderBy)
        {
            string sql = @"SELECT do.*, oh.OrderRefNo, oh.userfld5 AS CustomInvoiceNo, oh.OrderDate, pos.PointOfSaleName, pos.OutletName 
                           FROM DeliveryOrder do 
                                INNER JOIN OrderHdr oh ON do.SalesOrderRefNo = oh.OrderHdrID 
                                INNER JOIN PointOfSale pos ON oh.PointOfSaleID = pos.PointOfSaleID 
                                INNER JOIN Membership mbr ON do.MembershipNo = mbr.MembershipNo 
                           WHERE ISNULL(do.Deleted, 0) = 0 AND ISNULL(oh.IsVoided, 0) = 0 
                             AND ISNULL(mbr.FirstName,'') + ISNULL(mbr.LastName,'') + ISNULL(mbr.ChristianName,'') + ISNULL(mbr.ChineseName,'') + ISNULL(mbr.NameToAppear,'') LIKE @CustName 
                             --AND mbr.MembershipNo LIKE @CustNo 
                             AND (ISNULL(mbr.Mobile,'') + ' ' + ISNULL(mbr.Home,'') + ' ' + ISNULL(mbr.Office,'') + ' ' + ISNULL(mbr.Fax,'') + ' ' + ISNULL(do.MobileNo,'') + ' ' + ISNULL(do.HomeNo,'')) LIKE @ContactNo 
                             AND (oh.OrderHdrID LIKE @InvNo OR oh.OrderRefNo LIKE @InvNo OR oh.userfld5 LIKE @InvNo) 
                             AND pos.OutletName LIKE @Outlet 
                             AND oh.OrderDate BETWEEN @DateFrom AND @DateTo 
                             AND (do.DeliveryDate IS NULL OR do.DeliveryDate BETWEEN @DODateFrom AND @DODateTo) 
                             AND (CASE 
                                      WHEN @DeliveryDetails = 'ALL' THEN 1 
                                      WHEN @DeliveryDetails = 'ASSIGNED' AND (ISNULL(do.DeliveryAddress,'') <> '' AND ISNULL(CONVERT(varchar(50), do.DeliveryDate),'') <> '') THEN 1 
                                      WHEN @DeliveryDetails = 'UNASSIGNED' AND (ISNULL(do.DeliveryAddress,'') = '' OR ISNULL(CONVERT(varchar(50), do.DeliveryDate),'') = '') THEN 1 
                                 END) = 1 
                          ";
            if (!string.IsNullOrEmpty(orderBy))
            {
                sql += " ORDER BY " + orderBy + ViewState[SORT_DIRECTION];
            }
            QueryCommand cmd = new QueryCommand(sql, "PowerPOS");

            cmd.AddParameter("@CustName", "%" + txtSearchCustName.Text + "%", DbType.String);
            //cmd.AddParameter("@CustNo", "%" + txtSearchCustNo.Text + "%", DbType.String);
            cmd.AddParameter("@ContactNo", "%" + txtSearchContactNo.Text + "%", DbType.String);
            cmd.AddParameter("@InvNo", "%" + txtSearchInvoiceNo.Text + "%", DbType.String);
            cmd.AddParameter("@Outlet", "%" + ddlSearchOutlet.SelectedValue + "%", DbType.String);
            cmd.AddParameter("@DeliveryDetails", ddlSearchDeliveryDetails.SelectedValue, DbType.String);

            DateTime dateFrom;
            DateTime dateTo;
            try 
            { dateFrom = System.Data.SqlTypes.SqlDateTime.Parse(txtSearchInvDateFrom.Text).Value; }
            catch
            { dateFrom = System.Data.SqlTypes.SqlDateTime.MinValue.Value; }

            try
            { dateTo = System.Data.SqlTypes.SqlDateTime.Parse(txtSearchInvDateTo.Text).Value; }
            catch
            { dateTo = System.Data.SqlTypes.SqlDateTime.MaxValue.Value; }

            if (dateTo.TimeOfDay.ToString() == "00:00:00") dateTo = dateTo.Add(TimeSpan.Parse("23:59:59"));
            cmd.AddParameter("@DateFrom", dateFrom, DbType.DateTime);
            cmd.AddParameter("@DateTo", dateTo, DbType.DateTime);

            DateTime dateFrom_DO;
            DateTime dateTo_DO;
            try
            { dateFrom_DO = System.Data.SqlTypes.SqlDateTime.Parse(txtSearchDODateFrom.Text).Value; }
            catch
            { dateFrom_DO = System.Data.SqlTypes.SqlDateTime.MinValue.Value; }

            try
            { dateTo_DO = System.Data.SqlTypes.SqlDateTime.Parse(txtSearchDODateTo.Text).Value; }
            catch
            { dateTo_DO = System.Data.SqlTypes.SqlDateTime.MaxValue.Value; }

            if (dateTo_DO.TimeOfDay.ToString() == "00:00:00") dateTo_DO = dateTo_DO.Add(TimeSpan.Parse("23:59:59"));
            cmd.AddParameter("@DODateFrom", dateFrom_DO, DbType.DateTime);
            cmd.AddParameter("@DODateTo", dateTo_DO, DbType.DateTime);

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

                string city = new PostalCodeDB(drv["PostalCode"].ToString()).City;
                if (string.IsNullOrEmpty(city)) city = "";

                Literal litDeliveryAddress = (Literal)e.Row.FindControl("litDeliveryAddress");
                litDeliveryAddress.Text = drv["DeliveryAddress"].ToString().Replace(Environment.NewLine, "<br/>");

                string deliveryDate = "";
                DateTime tmpDeliveryDate;
                if (DateTime.TryParse(drv["DeliveryDate"].ToString(), out tmpDeliveryDate))
                {
                    deliveryDate = tmpDeliveryDate.ToString("dd/MM/yyyy");
                }
                else
                {
                    deliveryDate = "-";
                }

                if (DateTime.TryParse(drv["TimeSlotFrom"].ToString(), out tmpDeliveryDate))
                {
                    deliveryDate = deliveryDate + "<br/>" + tmpDeliveryDate.ToString("h tt");
                }
                if (DateTime.TryParse(drv["TimeSlotTo"].ToString(), out tmpDeliveryDate))
                {
                    deliveryDate = deliveryDate + " - " + tmpDeliveryDate.ToString("h tt");
                }

                Literal litDeliveryDate = (Literal)e.Row.FindControl("litDeliveryDate");
                litDeliveryDate.Text = deliveryDate;
            }
        }

        protected void btnPostalCode_Click(object sender, EventArgs e)
        {
            PostalCodeDB postalCode = new PostalCodeDB(ctrlPostalCode.Text);
            if (string.IsNullOrEmpty(postalCode.Zip))
            {
                ctrlDeliveryAddress.Text = LanguageManager.GetTranslation("ADDRESS NOT FOUND");
            }
            else
            {
                //ctrlDeliveryAddress.Text = postalCode.Area1;
                ctrlDeliveryAddress.Text = "";
                if (!string.IsNullOrEmpty(postalCode.Area1))
                {
                    if (ctrlUnitNo.Text == "")
                        ctrlDeliveryAddress.Text += postalCode.Area1 + Environment.NewLine;
                    else
                        ctrlDeliveryAddress.Text += postalCode.Area1 + " " + ctrlUnitNo.Text + Environment.NewLine;
                }
                if (!string.IsNullOrEmpty(postalCode.Area2))
                {
                    if (ctrlDeliveryAddress.Text == "")
                        ctrlDeliveryAddress.Text += postalCode.Area2 + " " + ctrlUnitNo.Text + Environment.NewLine;
                    else
                        ctrlDeliveryAddress.Text += postalCode.Area2 + Environment.NewLine;
                }
                if (!string.IsNullOrEmpty(postalCode.City))
                    ctrlDeliveryAddress.Text += postalCode.City + " ";
                if (!string.IsNullOrEmpty(ctrlDeliveryAddress.Text))
                    ctrlDeliveryAddress.Text += postalCode.Zip;
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGrid(string.Empty);
        }
    }
}
