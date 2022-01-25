using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PowerPOS;
using SubSonic;
using System.Data;
using SubSonic.Utilities;
using System.Drawing;

namespace PowerWeb.Promo
{
    public partial class PromoGroupPriceDiscount : System.Web.UI.Page
    {
        private const bool AUTO_GENERATEID = false;
        private const string SORT_DIRECTION = "SORT_DIRECTION";
        private const string ORDER_BY = "ORDER_BY";
        private const int colItemNo = 2;
        private const string CampaignType = "AnyXOffAllItems";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                // NOTE: the following uses an overload of RegisterClientScriptBlock() 
                // that will surround our string with the needed script tags 
                ClientScript.RegisterClientScriptBlock(GetType(), "IsPostBack", "var isPostBack = true;", true);
            }

            if (Request.QueryString["id"] != null)
            {
                string id = Utility.GetParameter("id");
                #region *) Display: Show Error Message (If Any)
                if (Request.QueryString["msg"] != null)
                {
                    string msg = Utility.GetParameter("msg"); ;
                    lblResult.Text = "<span style=\"font-weight:bold; color:#22bb22\">" + msg + "</span>";
                }
                #endregion

                if (!String.IsNullOrEmpty(id) && id != "0")
                {
                    if (!IsPostBack)
                    {
                        ToggleEditor(true);
                        BindOutlet();
                        LoadEditor(id);
                    }
                }
                else
                {
                    if (!IsPostBack)
                    {
                        ToggleEditor(true);
                        BindOutlet();
                        SelectAllOutlet();
                        BindPromoDetails();

                        //rbAll.Checked = true;
                        //rbPrice.Checked = true;
                        btnDelete.Visible = false;
                    }
                }

            }
            else
            {
                if (!IsPostBack)
                {
                    ToggleEditor(false);
                    LoadOutlet();
                    BindGrid();

                    if (Session["DeleteMessage"] != null && Session["DeleteMessage"].ToString() != "")
                    {
                        lblMsg.Text = Session["DeleteMessage"].ToString();
                        Session["DeleteMessage"] = null;
                    }
                    else
                    {
                        lblMsg.Text = "";
                    }
                }
            }

        }

        #region "helper"

        private void LoadEditor(string id)
        {
            /*header*/
            PromoCampaignHdr header = new PromoCampaignHdr(id);

            hdPromoCampaignHdrID.Value = header.PromoCampaignHdrID.ToString();
            txtPromoCode.Text = header.PromoCode;
            txtBarcode.Text = header.Barcode;
            txtPromoName.Text = header.PromoCampaignName;
            txtPriority.Text = header.Priority.ToString();
            txtCtrlStartDate.Text = header.DateFrom.ToString("dd MMM yyyy hh:mm tt");
            txtCtrlEndDate.Text = header.DateTo.ToString("dd MMM yyyy hh:mm tt");

            if (header.IsRestricHour == true)
            {
                cbIsRestrictHourOnly.Checked = true;
                txtRestrictHourStart.Enabled = true;
                txtRestrictHourStart.Text = header.RestrictHourStart.Value.ToString("hh:mm tt");

                txtRestrictHourEnd.Enabled = true;
                txtRestrictHourEnd.Text = header.RestrictHourEnd.Value.ToString("hh:mm tt");
            }

            if (header.ForNonMembersAlso == true)
            {
                rbAll.Checked = true;
            }
            else
            {
                rbMemberOnly.Checked = true;
            }

            /*days*/
            foreach (ListItem i in cbDaysApplicable.Items)
            {
                i.Selected = false;
            }

            try
            {
                var selGroup = header.ListMembershipGroup;
                for (int i = 0; i < ddlMemberGroup.Items.Count; i++)
                    ddlMemberGroup.Items[i].Selected = selGroup.Where(o => o.MembershipGroupId == ddlMemberGroup.Items[i].Value.GetIntValue()).FirstOrDefault() != null;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }

            PromoDaysMapCollection dayscol = new PromoDaysMapCollection();
            dayscol.Where(PromoDaysMap.Columns.PromoCampaignHdrID, Comparison.Equals, id);
            dayscol.Where(PromoDaysMap.Columns.Deleted, Comparison.Equals, false);
            dayscol.Load();

            if (dayscol.Count() > 0)
            {
                foreach (PromoDaysMap d in dayscol)
                {
                    switch (d.DaysPromo)
                    {
                        case "Monday": cbDaysApplicable.Items[0].Selected = true; break;
                        case "Tuesday": cbDaysApplicable.Items[1].Selected = true; break;
                        case "Wednesday": cbDaysApplicable.Items[2].Selected = true; break;
                        case "Thursday": cbDaysApplicable.Items[3].Selected = true; break;
                        case "Friday": cbDaysApplicable.Items[4].Selected = true; break;
                        case "Saturday": cbDaysApplicable.Items[5].Selected = true; break;
                        case "Sunday": cbDaysApplicable.Items[6].Selected = true; break;
                    }
                }
            }

            /*details*/
            PromoCampaignDetCollection col = new PromoCampaignDetCollection();
            col.Where(PromoCampaignDet.Columns.PromoCampaignHdrID, Comparison.Equals, id);
            col.Where(PromoCampaignDet.Columns.Deleted, Comparison.Equals, false);
            col.Load();

            DataTable dt = new DataTable();
            dt = new DataTable("PromoCampaignDet");
            dt.Columns.Add("DetailID");
            dt.Columns.Add("PromoCampaignDetID");
            dt.Columns.Add("ItemNo");
            dt.Columns.Add("ItemGroupId");
            dt.Columns.Add("ItemName");
            dt.Columns.Add("UnitQty");
            dt.Columns.Add("AnyQty");
            dt.Columns.Add("CategoryName");
            dt.Columns.Add("UnitPrice");
            dt.Columns.Add("RetailPrice");
            dt.Columns.Add("PromoPrice");
            dt.Columns.Add("DiscPercent");
            dt.Columns.Add("DiscAmount");
            dt.Columns.Add("Deleted");


            int DetailId = 1;
            foreach (PromoCampaignDet d in col)
            {
                decimal UnitPrice = 0;
                string ItemName = "";
                string ItemNo = "";

                if (d.ItemGroupID != null && d.ItemGroupID != 0)
                {
                    ItemGroup ig = new ItemGroup(d.ItemGroupID);
                    if (ig != null)
                    {
                        ItemName = ig.ItemGroupName;
                    }
                }
                else
                {
                    if (d.ItemNo != null && d.ItemNo != "")
                    {
                        Item item = new Item(d.ItemNo);
                        ItemNo = d.ItemNo;
                        if (item != null)
                        {
                            ItemName = item.ItemName;
                            UnitPrice = item.RetailPrice;
                        }
                    }
                    else
                    {
                        ItemNo = "*";
                        string query = "Select ISNULL(MAX(RetailPrice),0) As RetailPrice " +
                                        "from Item where CategoryName = '" + d.CategoryName + "'";
                        QueryCommand cmd = new QueryCommand(query, "PowerPOS");
                        DataTable dq = new DataTable();
                        dq.Load(DataService.GetReader(cmd));

                        if (dq != null && dq.Rows.Count > 0)
                        {
                            ItemName = "*";
                            UnitPrice = Decimal.Parse(dq.Rows[0][0].ToString());
                        }

                    }
                }

                string minQuantity = d.MinQuantity == -1 ? "" : d.MinQuantity.ToString();
                dt.Rows.Add(DetailId, d.PromoCampaignDetID, ItemNo, d.ItemGroupID.ToString(), ItemName, d.UnitQty, d.AnyQty, d.CategoryName, UnitPrice, d.UnitQty * UnitPrice, d.PromoPrice, d.DiscPercent, d.DiscAmount, d.Deleted);
                DetailId++;
            }
            ViewState["Details"] = dt;
            BindPromoDetails();
            //CountTotalPrice();

            PromoOutletCollection ol = new PromoOutletCollection();
            ol.Where(PromoOutlet.Columns.PromoCampaignHdrID, Comparison.Equals, header.PromoCampaignHdrID);
            ol.Where(p => p.Deleted != true);
            ol.Load();

            foreach (GridViewRow row in gvOutlet.Rows)
            {
                foreach (PromoOutlet o in ol)
                {
                    if (row.Cells[1].Text.Trim() == o.OutletName.Trim())
                    {
                        CheckBox field = (CheckBox)row.FindControl("CheckBox1");
                        field.Checked = true;
                    }
                }
            }
        }

        private void LoadOutlet()
        {
            //OutletCollection outletlist = new OutletCollection();
            //outletlist.Where("Deleted", Comparison.NotEquals, true);
            //outletlist.Load();
           
            OutletCollection outletList = OutletController.FetchByUserNameForReport(false, false, Session["UserName"] + "");
            var allOutlet = OutletController.FetchAll(false, false);
            bool isAssignedToAll = outletList.Count >= allOutlet.Count;

            ddlOutlet.Items.Clear();
            if(isAssignedToAll)
                ddlOutlet.Items.Add(new ListItem() { Text = "ALL", Value = "-1" });

            if (outletList != null && outletList.Count() > 0)
            {
                foreach (Outlet it in outletList)
                {
                    ddlOutlet.Items.Add(new ListItem() { Text = it.OutletName, Value = it.OutletName });
                }
            }
        }

        private void BindGrid()
        {
            string sortDir = "ASC";
            string orderBy = "PromoCampaignHdrID";

            if (ViewState[ORDER_BY] != null)
            {
                orderBy = ViewState[ORDER_BY].ToString();
            }

            if (ViewState[SORT_DIRECTION] != null)
            {
                sortDir = (string)ViewState[SORT_DIRECTION];
            }

            
            string search = txtSearch.Text ?? "%";
            DataTable dt = PromotionAdminController.SearchActivePromoWithOutlet(search, txtPromoDate.Text, ddlOutlet.SelectedValue, orderBy, sortDir);
            dt = CommonUILib.DataTableConvertBoolColumnToYesOrNo(dt);

            GridView1.DataSource = dt;
            GridView1.DataBind();
        }

        private void ToggleEditor(bool showIt)
        {
            pnlEdit.Visible = showIt;
            pnlIndex.Visible = !showIt;
            if (showIt)
            {
                var groupList = new MembershipGroupController().FetchAll().Where(o => o.Deleted.GetValueOrDefault(false)==false).OrderBy(o => o.GroupName).ToList();
                ddlMemberGroup.Items.Clear();
                foreach (var grp in groupList)
                    ddlMemberGroup.Items.Add(new ListItem { Value = grp.MembershipGroupId.ToString(), Text = grp.GroupName }); 
            }
        }

        private void BindOutlet()
        {
            DataTable dt = new DataTable();
            dt = new DataTable("PromoOutlet");
            dt.Columns.Add("OutletName");
            dt.Columns.Add("PointOfSaleName");

            //OutletCollection outletlist = new OutletCollection();
            //outletlist.Where("Deleted", Comparison.NotEquals, true);
            //outletlist.OrderByAsc(Outlet.Columns.OutletName);
            //outletlist.Load();


            OutletCollection outletlist = OutletController.FetchByUserNameForReport(false, false, Session["UserName"] + "");

            foreach (Outlet ot in outletlist)
            {
                PointOfSaleCollection po = new PointOfSaleCollection();
                po.Where(p => p.Deleted != true);
                po.Where(PointOfSale.Columns.OutletName, Comparison.Equals, ot.OutletName);
                po.OrderByAsc(PointOfSale.Columns.PointOfSaleName);
                po.Load();

                string pointofsalename = "";

                foreach (PointOfSale ps in po)
                {
                    pointofsalename += ps.PointOfSaleName + "<br />";
                }

                dt.Rows.Add(ot.OutletName, pointofsalename);
            }

            ViewState["OutletDetails"] = dt;
            gvOutlet.DataSource = dt;
            gvOutlet.DataBind();
        }

        private void SelectAllOutlet()
        {
            foreach (GridViewRow row in gvOutlet.Rows)
            {
                CheckBox field = (CheckBox)row.FindControl("CheckBox1");
                field.Checked = true;
            }
        }

        private void BindPromoDetails()
        {
            var dt = (DataTable)ViewState["Details"];
            if (dt == null)
            {
                dt = new DataTable();
                dt.Columns.Add("DetailID");
                dt.Columns.Add("PromoCampaignDetID");
                dt.Columns.Add("ItemNo");
                dt.Columns.Add("ItemGroupId");
                dt.Columns.Add("ItemName");
                dt.Columns.Add("UnitQty");
                dt.Columns.Add("AnyQty");
                dt.Columns.Add("CategoryName");
                dt.Columns.Add("UnitPrice");
                dt.Columns.Add("RetailPrice");
                dt.Columns.Add("PromoPrice");
                dt.Columns.Add("DiscPercent");
                dt.Columns.Add("DiscAmount");
                dt.Columns.Add("Deleted");
            }

            gvDetails.DataSource = dt;
            gvDetails.DataBind();
        }

        //private void CountTotalPrice() { 
        //    DataTable dt = (DataTable)ViewState["Details"];

        //    int anyQty = Int32.Parse(string.IsNullOrEmpty(txtAnyQty.Text) ? "0" : txtAnyQty.Text);
        //    decimal price = Decimal.Parse(string.IsNullOrEmpty(txtPrice.Text) ? "0" : txtPrice.Text.Replace(",", ""));
        //    decimal discount = decimal.Parse(string.IsNullOrEmpty(txtDiscount.Text) ? "0" : txtDiscount.Text.Replace(",", ""));
        //    decimal totalprice = 0;
        //    decimal discountPrice = 0;

        //    if (anyQty > 0)
        //    {
        //        decimal total = 0;
        //        int count = 0;
        //        if (dt != null && dt.Rows.Count > 0)
        //        {
        //            foreach (DataRow dr in dt.Rows)
        //            {
        //                if (dr["Deleted"].ToString().ToLower() != "true")
        //                {
        //                    int unitQty = Int32.Parse(dr["UnitQty"].ToString() ?? "0");
        //                    decimal retailPrice = Decimal.Parse(dr["RetailPrice"].ToString() ?? "0");

        //                    total += unitQty * retailPrice;
        //                    count = unitQty;
        //                }
        //            }

        //            totalprice = anyQty * (total / count);
        //        }
        //    }
        //    else 
        //    {
        //        if (dt != null && dt.Rows.Count > 0)
        //        {
        //            foreach (DataRow dr in dt.Rows)
        //            {
        //                if (dr["Deleted"].ToString().ToLower() != "true")
        //                {
        //                    int unitQty = Int32.Parse(dr["UnitQty"].ToString() ?? "0");
        //                    decimal retailPrice = Decimal.Parse(dr["RetailPrice"].ToString() ?? "0");

        //                    totalprice += unitQty * retailPrice;
        //                }
        //            }
        //        }
        //    }

        //    lblTotalPrice.Text = "$ " + totalprice.ToString("N2");
        //    if (rbPrice.Checked)
        //    {
        //        discountPrice = price;
        //    }
        //    else
        //    {
        //        if (discount > 0)
        //        {
        //            discountPrice = discount * totalprice / 100;
        //        }
        //    }

        //    lblDiscountedPrice.Text = discountPrice.ToString("N2");
        //    lblDiscountedGiven.Text = (totalprice - discountPrice).ToString("N2");
        //}

        #endregion

        #region "event handler"

        protected void BtnSelectAll_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in GridView1.Rows)
            {
                CheckBox field = (CheckBox)row.FindControl("CheckBox1");
                field.Checked = true;
            }
        }

        protected void BtnClearSelection_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in GridView1.Rows)
            {
                CheckBox field = (CheckBox)row.FindControl("CheckBox1");
                field.Checked = false;
            }
        }

        protected void BtnDeleteSelection_Click(object sender, EventArgs e)
        {
            int count = 0;
            QueryCommandCollection commands = new QueryCommandCollection();
            foreach (GridViewRow row in GridView1.Rows)
            {
                CheckBox field = (CheckBox)row.FindControl("CheckBox1");
                if (field.Checked)
                {
                    Label hdrid = (Label)row.FindControl("lblPromoCampaignHdrIDGV");
                    PromoCampaignHdr myItem = new PromoCampaignHdr(Int32.Parse(hdrid.Text));
                    if (myItem.PromoCampaignHdrID.ToString() != "")
                    {
                        myItem.Deleted = true;
                        commands.Add(myItem.GetUpdateCommand("SYSTEM"));

                        #region *) Audit Log
                        if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.AuditLog.SetupProductPromotion), false))
                        {
                            AuditLogController.AddLog("DELETE", "PromoCampaignHdr", "PromoCampaignHdrID", myItem.PromoCampaignHdrID.ToString(), "Deleted = false", "Deleted = true", Session["username"].ToString());
                        }
                        #endregion

                        count++;
                    }

                }

            }
            SubSonic.DataService.ExecuteTransaction(commands);
            Session["DeleteMessage"] = String.Format("<span style='color:red; font-weight:bold;'>Deleted {0} Record(s)..</span>", count);
            Response.Redirect("SetupProductPromotion.aspx");
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                try
                {
                    if (e.Row.Cells[10].Text.ToLower() == "yes")
                    {
                        e.Row.Cells[10].Text = "All";
                    }
                    else
                    {
                        e.Row.Cells[10].Text = "Member Only";
                    }

                }
                catch (Exception ex)
                {
                    //Unable to convert
                    Logger.writeLog(ex);
                }
            }
        }

        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            ViewState[ORDER_BY] = e.SortExpression;
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

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void GridView1_DataBound(object sender, EventArgs e)
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

                string pageCount = "<b>" + GridView1.PageCount.ToString() + "</b>";
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

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void btnClearFilter_Click(object sender, EventArgs e)
        {
            txtPromoDate.Text = "";
            txtSearch.Text = "";
            ddlOutlet.SelectedValue = "-1";
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                System.Transactions.TransactionOptions to = new System.Transactions.TransactionOptions();
                to.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
                using (System.Transactions.TransactionScope transScope =
                new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Required, to))
                {
                    /*validate form*/

                    int promocampaignhdrid = 0;
                    if (!String.IsNullOrEmpty(hdPromoCampaignHdrID.Value))
                    {
                        promocampaignhdrid = Int32.Parse(hdPromoCampaignHdrID.Value);

                    }

                    /*validate outlet*/
                    int countcheckedoutlet = 0;
                    List<string> outletList = new List<string>();
                    foreach (GridViewRow row in gvOutlet.Rows)
                    {
                        CheckBox field = (CheckBox)row.FindControl("CheckBox1");
                        if (field.Checked == true)
                        {
                            countcheckedoutlet++;
                            outletList.Add(row.Cells[1].Text);
                        }
                    }

                    if (countcheckedoutlet == 0)
                    {
                        throw new Exception("At least 1 outlet must be selected");
                    }

                    /*duplicate barcode*/
                    if (!string.IsNullOrEmpty(txtBarcode.Text))
                    {
                        if (PromotionAdminController.CheckDuplicateBarcodePromo(txtBarcode.Text.Trim(), promocampaignhdrid, outletList))
                        {
                            throw new Exception("Barcode is duplicated");
                        }
                    }


                    /*duplicate promo code*/
                    if (!string.IsNullOrEmpty(txtPromoCode.Text))
                    {
                        if (PromotionAdminController.CheckDuplicatePromoCode(txtPromoCode.Text.Trim(), promocampaignhdrid))
                        {
                            throw new Exception("Promo Code is duplicated");
                        }
                    }

                    /*validate date*/
                    DateTime start = DateTime.Parse(txtCtrlStartDate.Text);
                    DateTime end = DateTime.Parse(txtCtrlEndDate.Text);

                    if (start > end)
                    {
                        throw new Exception("End Date can not be earlier than Start Date");
                    }

                    if (end <= DateTime.Today)
                    {
                        throw new Exception("End Date can not be earlier than today");
                    }

                    /*validate restrict hour*/
                    if (cbIsRestrictHourOnly.Checked)
                    {
                        DateTime starthour = DateTime.Parse(txtRestrictHourStart.Text);
                        DateTime endhour = DateTime.Parse(txtRestrictHourEnd.Text);

                        if (endhour <= starthour)
                        {
                            throw new Exception("End Hour can not be earlier than Start Hour");
                        }
                    }

                    /*validate days applicable*/
                    int selecteddays = 0;
                    foreach (ListItem i in cbDaysApplicable.Items)
                    {
                        if (i.Selected)
                        {
                            selecteddays++;
                        }
                    }

                    if (selecteddays == 0)
                    {
                        throw new Exception("At least 1 applicable day must be selected");
                    }

                    /*validate details*/
                    DataTable dt = (DataTable)ViewState["Details"];
                    if (dt == null || dt.Rows.Count == 0)
                    {
                        throw new Exception("At least 1 item must be added");
                    }
                    else
                    {
                        int rowdeleted = 0;
                        foreach (DataRow dr in dt.Rows)
                        {
                            if (dr["Deleted"].ToString().ToLower() == "true")
                                rowdeleted++;
                        }

                        if (rowdeleted == dt.Rows.Count)
                        {
                            throw new Exception("At least 1 item must be added");
                        }
                    }

                    /*Save Header*/
                    PromoCampaignHdr header;
                    PromoCampaignHdr originalHeader = new PromoCampaignHdr();

                    if (!String.IsNullOrEmpty(hdPromoCampaignHdrID.Value))
                    {
                        promocampaignhdrid = Int32.Parse(hdPromoCampaignHdrID.Value);
                        header = new PromoCampaignHdr(promocampaignhdrid);
                        header.CopyTo(originalHeader);
                        originalHeader.IsNew = false;
                    }
                    else
                    {
                        header = new PromoCampaignHdr();
                    }

                    /*check duplicate promocode*/
                    header.PromoCode = txtPromoCode.Text;
                    header.Barcode = txtBarcode.Text;
                    header.PromoCampaignName = txtPromoName.Text;
                    header.CampaignType = CampaignType;
                    header.DateFrom = start;
                    header.DateTo = end;
                    header.Priority = Int32.Parse(txtPriority.Text ?? "0");
                    header.Enabled = true;

                    header.ForNonMembersAlso = false;
                    if (rbAll.Checked)
                    {
                        header.ForNonMembersAlso = true;
                    }

                    if (cbIsRestrictHourOnly.Checked)
                    {
                        header.IsRestricHour = true;
                        header.RestrictHourStart = DateTime.Parse(txtRestrictHourStart.Text);
                        header.RestrictHourEnd = DateTime.Parse(txtRestrictHourEnd.Text);
                    }
                    else
                    {
                        header.IsRestricHour = false;
                        header.RestrictHourStart = null;
                        header.RestrictHourEnd = null;
                    }

                    var selMemberGroup = new List<string>();
                    for (int i = 0; i < ddlMemberGroup.Items.Count; i++)
                    {
                        if (ddlMemberGroup.Items[i].Selected)
                            selMemberGroup.Add(ddlMemberGroup.Items[i].Value);
                    }

                    if (selMemberGroup.Count == 0 && !header.ForNonMembersAlso.GetValueOrDefault(false))
                        throw new Exception("Please select at least 1 membership group");

                    if (selMemberGroup.Count == ddlMemberGroup.Items.Count)
                    {
                        selMemberGroup.Clear();
                        selMemberGroup.Add("0");
                    }

                    header.MembershipGroupID = string.Join(",", selMemberGroup.ToArray());

                    header.Deleted = false;
                    header.Save(Session["username"].ToString());
                    if (!header.Userint1.HasValue)
                        header.Userint1 = 1;
                    else
                        header.Userint1 += 1;
                    hdPromoCampaignHdrID.Value = header.PromoCampaignHdrID.ToString();

                    #region *) Audit Log for PromoCampaignHdr
                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.AuditLog.SetupProductPromotion), false))
                    {
                        string operation = originalHeader.IsNew ? "INSERT" : "UPDATE";
                        if (header.DateFrom != originalHeader.DateFrom)
                            AuditLogController.AddLog(operation, "PromoCampaignHdr", "PromoCampaignHdrID", header.PromoCampaignHdrID.ToString(), "DateFrom = " + originalHeader.DateFrom.ToString("yyyy-MM-dd HH:mm:ss"), "DateFrom = " + header.DateFrom.ToString("yyyy-MM-dd HH:mm:ss"), Session["username"].ToString());
                        if (header.DateTo != originalHeader.DateTo)
                            AuditLogController.AddLog(operation, "PromoCampaignHdr", "PromoCampaignHdrID", header.PromoCampaignHdrID.ToString(), "DateTo = " + originalHeader.DateTo.ToString("yyyy-MM-dd HH:mm:ss"), "DateTo = " + header.DateTo.ToString("yyyy-MM-dd HH:mm:ss"), Session["username"].ToString());
                    }
                    #endregion

                    /*Save Promo Days*/
                    string sql2 = "Update PromoDaysMap set Deleted = 'true',modifiedon = GETDATE() where PromoCampaignHdrID = " + promocampaignhdrid;
                    DataService.ExecuteQuery(new QueryCommand(sql2));

                    foreach (ListItem i in cbDaysApplicable.Items)
                    {
                        PromoDaysMapCollection dDay = new PromoDaysMapCollection();
                        dDay.Where(PromoDaysMap.Columns.DaysNumber, Comparison.Equals, i.Value);
                        dDay.Where(PromoDaysMap.Columns.PromoCampaignHdrID, Comparison.Equals, header.PromoCampaignHdrID.ToString());
                        dDay.Load();

                        string dayPromo = "";
                        int daysNumber = 0;
                        switch (i.Value)
                        {
                            case "1": dayPromo = "Monday"; daysNumber = 1; break;
                            case "2": dayPromo = "Tuesday"; daysNumber = 2; break;
                            case "3": dayPromo = "Wednesday"; daysNumber = 3; break;
                            case "4": dayPromo = "Thursday"; daysNumber = 4; break;
                            case "5": dayPromo = "Friday"; daysNumber = 5; break;
                            case "6": dayPromo = "Saturday"; daysNumber = 6; break;
                            case "7": dayPromo = "Sunday"; daysNumber = 7; break;
                        }

                        if (i.Selected)
                        {
                            PromoDaysMap monday;
                            if (dDay.Count > 0)
                            {
                                monday = dDay[0];
                            }
                            else
                            {
                                monday = new PromoDaysMap();
                            }

                            monday.PromoCampaignHdrID = header.PromoCampaignHdrID;
                            monday.DaysPromo = dayPromo;
                            monday.DaysNumber = daysNumber;
                            monday.Deleted = false;
                            monday.Save(Session["username"].ToString());

                        }

                    }

                    /*Save Promo Details*/
                    string sql = "Update PromoCampaignDet set Deleted = 'true', modifiedon=GETDATE() where PromoCampaignHdrID = " + promocampaignhdrid;
                    DataService.ExecuteQuery(new QueryCommand(sql));

                    foreach (DataRow dr in dt.Rows)
                    {
                        PromoCampaignDet detail;
                        PromoCampaignDet originalDetail = new PromoCampaignDet();

                        if (dr["Deleted"].ToString().ToLower() == "false")
                        {
                            if (dr["PromoCampaignDetID"].ToString() != "" && dr["PromoCampaignDetID"].ToString() != "0")
                            {
                                detail = new PromoCampaignDet(Int32.Parse(dr["PromoCampaignDetID"].ToString()));
                                detail.CopyTo(originalDetail);
                                originalDetail.IsNew = false;
                            }
                            else
                            {
                                detail = new PromoCampaignDet();
                                detail.PromoCampaignHdrID = Int32.Parse(hdPromoCampaignHdrID.Value ?? "0");
                            }

                            if (!string.IsNullOrEmpty(dr["ItemGroupId"].ToString()) && dr["ItemGroupId"].ToString() != "0")
                            {
                                detail.ItemGroupID = Int32.Parse(dr["ItemGroupId"].ToString());
                            }
                            else
                            {
                                detail.ItemGroupID = null;
                            }

                            if (!string.IsNullOrEmpty(dr["CategoryName"].ToString()))
                            {
                                detail.CategoryName = dr["CategoryName"].ToString();
                            }
                            else
                            {
                                detail.CategoryName = null;
                            }

                            if (dr["ItemNo"].ToString() != "" && dr["ItemNo"].ToString() != "*")
                            {
                                detail.ItemNo = dr["ItemNo"].ToString();
                            }
                            else
                            {
                                detail.ItemNo = null;
                            }

                            detail.UnitQty = Int32.Parse(dr["UnitQty"].ToString() ?? "0");
                            detail.MinQuantity = -1;
                            detail.AnyQty = Int32.Parse(string.IsNullOrEmpty(dr["AnyQty"].ToString()) ? "0" : dr["AnyQty"].ToString());

                            detail.PromoPrice = Decimal.Parse(dr["PromoPrice"].ToString() ?? "0");
                            detail.DiscPercent = Decimal.Parse(dr["DiscPercent"].ToString() ?? "0");
                            detail.DiscAmount = Decimal.Parse(dr["DiscAmount"].ToString() ?? "0");

                            detail.Deleted = false;
                            detail.Save(Session["username"].ToString());

                            #region *) Audit Log for PromoCampaignDet
                            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.AuditLog.SetupProductPromotion), false))
                            {
                                string operation = originalDetail.IsNew ? "INSERT" : "UPDATE";
                                if (detail.PromoPrice != originalDetail.PromoPrice)
                                    AuditLogController.AddLog(operation, "PromoCampaignDet", "PromoCampaignDetID", detail.PromoCampaignDetID.ToString(), "PromoPrice = " + originalDetail.PromoPrice.GetValueOrDefault(0).ToString("N2"), "PromoPrice = " + detail.PromoPrice.GetValueOrDefault(0).ToString("N2"), Session["username"].ToString());
                            }
                            #endregion
                        }
                        else
                        {
                            #region *) Audit Log for PromoCampaignDet
                            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.AuditLog.SetupProductPromotion), false))
                            {
                                if (dr["PromoCampaignDetID"].ToString() != "" && dr["PromoCampaignDetID"].ToString() != "0")
                                {
                                    AuditLogController.AddLog("DELETE", "PromoCampaignDet", "PromoCampaignDetID", dr["PromoCampaignDetID"].ToString(), "Deleted = false", "Deleted = true", Session["username"].ToString());
                                }
                            }
                            #endregion
                        }
                    }

                    /*Outlet*/
                    /*validate outlet*/
                    string sql3 = "Update PromoOutlet set Deleted = 'true', modifiedon=GETDATE() where PromoCampaignHdrID = " + promocampaignhdrid;
                    DataService.ExecuteQuery(new QueryCommand(sql3));
                    foreach (GridViewRow row in gvOutlet.Rows)
                    {
                        PromoOutlet ou;
                        CheckBox field = (CheckBox)row.FindControl("CheckBox1");
                        PromoOutletCollection col = new PromoOutletCollection();
                        col.Where(PromoOutlet.Columns.PromoCampaignHdrID, hdPromoCampaignHdrID.Value);
                        col.Where(PromoOutlet.Columns.OutletName, row.Cells[1].Text.ToString());
                        col.Load();
                        if (field.Checked == true)
                        {
                            if (col.Count > 0)
                            {
                                ou = col[0];
                            }
                            else 
                            {
                                ou = new PromoOutlet();
                            }

                            ou.PromoCampaignHdrID = Int32.Parse(hdPromoCampaignHdrID.Value ?? "0"); ;
                            ou.OutletName = row.Cells[1].Text.ToString();
                            ou.Deleted = false;
                            ou.Save(Session["username"].ToString());
                        }
                    }

                    transScope.Complete();
                    Response.Redirect("SetupProductPromotion.aspx?id=" + hdPromoCampaignHdrID.Value + "&msg=Product saved");
                }
            }
            catch (Exception x)
            {
                Logger.writeLog(x);
                if (x.Message.Contains("Violation of PRIMARY KEY constraint"))
                {
                    lblResult.Text = "<span style=\"font-weight:bold; color:#990000\">Promo not saved: Promo Code: " + txtPromoCode.Text + " has already been used. Choose another name</span> ";
                }
                else
                {
                    //haveError = true;
                    lblResult.Text = "<span style=\"font-weight:bold; color:#990000\">Product not saved:</span> " + x.Message;
                }
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            PromoCampaignHdrController ctr = new PromoCampaignHdrController();
            ctr.Delete(Utility.GetParameter("id"));

            #region *) Audit Log
            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.AuditLog.SetupProductPromotion), false))
            {
                AuditLogController.AddLog("DELETE", "PromoCampaignHdr", "PromoCampaignHdrID", Utility.GetParameter("id"), "Deleted = false", "Deleted = true", Session["username"].ToString());
            }
            #endregion

            //redirect
            Response.Redirect(Request.CurrentExecutionFilePath);
        }

        protected void btnAddDetails_Click(object sender, EventArgs e)
        {
            var dt = (DataTable)ViewState["Details"];
            if (dt == null)
            {
                dt = new DataTable("PromoCampaignDet");
                dt.Columns.Add("DetailID");
                dt.Columns.Add("PromoCampaignDetID");
                dt.Columns.Add("ItemNo");
                dt.Columns.Add("ItemGroupId");
                dt.Columns.Add("ItemName");
                dt.Columns.Add("UnitQty");
                dt.Columns.Add("AnyQty");
                dt.Columns.Add("CategoryName");
                dt.Columns.Add("UnitPrice");
                dt.Columns.Add("RetailPrice");
                dt.Columns.Add("PromoPrice");
                dt.Columns.Add("DiscPercent");
                dt.Columns.Add("DiscAmount");
                dt.Columns.Add("Deleted");
            }

            int IsEdit = tmpIsEdit.Value.ToString() == "" ? 0 : Int32.Parse(tmpIsEdit.Value.ToString());
            int detailid = tmpDetailId.Value.ToString() == "0" ? gvDetails.Rows.Count + 1 : Int32.Parse(tmpDetailId.Value.ToString());
            int promoCampaignDetID = Int32.Parse(tmpPromoCampaignDetID.Value.ToString() ?? "0");
            string itemNo = tmpItemNo.Value;
            string ItemGroupId = tmpItemGroup.Value.ToString();
            int unitQty = Int32.Parse(string.IsNullOrEmpty(tmpQty.Value.ToString()) ? "0" : tmpQty.Value.ToString().Replace(",", ""));
            string anyQty = string.IsNullOrEmpty(tmpAnyQty.Value.ToString()) ? "0" : tmpAnyQty.Value.ToString().Replace(",", "");
            string categoryName = tmpCategoryName.Value;
            string itemName = "";
            decimal unitPrice = decimal.Parse(string.IsNullOrEmpty(tmpUnitPrice.Value.ToString()) ? "0" : tmpUnitPrice.Value.ToString().Replace(",", ""));
            decimal retailPrice = decimal.Parse(string.IsNullOrEmpty(tmpRetailPrice.Value.ToString()) ? "0" : tmpRetailPrice.Value.ToString().Replace(",", ""));
            decimal promoPrice = decimal.Parse(string.IsNullOrEmpty(tmpPromoPrice.Value.ToString()) ? "0" : tmpPromoPrice.Value.ToString().Replace(",", ""));
            decimal discPercent = decimal.Parse(string.IsNullOrEmpty(tmpDiscPercent.Value.ToString()) ? "0" : tmpDiscPercent.Value.ToString().Replace(",", ""));
            decimal discAmount = decimal.Parse(string.IsNullOrEmpty(tmpDiscAmount.Value.ToString()) ? "0" : tmpDiscAmount.Value.ToString().Replace(",", ""));

            if (ItemGroupId != "")
            {
                ItemGroup ig = new ItemGroup(ItemGroupId);
                if (ig != null)
                {
                    itemName = ig.ItemGroupName;
                }

            }
            else
            {
                if (itemNo != "*")
                {
                    Item item = new Item(itemNo);
                    if (item != null)
                    {
                        itemName = item.ItemName;
                        categoryName = item.CategoryName;
                    }
                }
                else
                {
                    itemName = "*";
                }
            }

            DataRow[] result = dt.Select("ItemNo = '" + itemNo + "' AND CategoryName = '" + categoryName.Replace("'", "''") + "' AND ItemGroupId='" + ItemGroupId.ToString() + "' and Deleted='False'");

            //if (promoCampaignDetID <= 0 || detailid <= 0)
            if (IsEdit == 0)
            {
                if (result.Count() > 0)
                {
                    lblResultDetail.Text = "<span style=\"font-weight:bold; color:#990000\">You must not add same detail twice.</span>";
                }
                else
                {
                    dt.Rows.Add(detailid, promoCampaignDetID, itemNo, ItemGroupId, itemName, unitQty, anyQty, categoryName, unitPrice, retailPrice, promoPrice, discPercent, discAmount, "false");
                    lblResultDetail.Text = "";
                }
            }
            else
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //string a = dt.Rows[i]["PromoCampaignDetID"].ToString();
                    //string b = promoCampaignDetID.ToString();
                    string a = dt.Rows[i]["DetailID"].ToString();
                    string b = detailid.ToString();
                    if (a == b)
                    {
                        dt.Rows[i]["ItemNo"] = itemNo;
                        dt.Rows[i]["ItemGroupID"] = ItemGroupId;
                        dt.Rows[i]["ItemName"] = itemName;
                        dt.Rows[i]["UnitQty"] = unitQty;
                        dt.Rows[i]["AnyQty"] = anyQty;
                        dt.Rows[i]["CategoryName"] = categoryName;
                        dt.Rows[i]["UnitPrice"] = unitPrice;
                        dt.Rows[i]["RetailPrice"] = retailPrice;
                        dt.Rows[i]["PromoPrice"] = promoPrice;
                        dt.Rows[i]["DiscPercent"] = discPercent;
                        dt.Rows[i]["DiscAmount"] = discAmount;

                        break;
                    }
                }
            }

            ViewState["Details"] = dt;
            BindPromoDetails();
            //CountTotalPrice();
        }

        protected void gvDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            foreach (GridViewRow row in gvDetails.Rows)
            {
                Label field = (Label)row.FindControl("lblDeletedGVDetails");

                if (field.Text.ToString().ToLower() == "true")
                {
                    row.Cells[0].Controls[1].Visible = false;
                    row.Cells[1].Controls[0].Visible = false;
                    //row.Cells[1].Controls[0].Visible = false;
                    row.BackColor = Color.LightPink;
                }
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton editBtn = (LinkButton)e.Row.FindControl("editBtn");
                string data = "";
                data += DataBinder.Eval(e.Row.DataItem, "PromoCampaignDetID") + "|";
                data += DataBinder.Eval(e.Row.DataItem, "ItemGroupID") + "|";
                data += DataBinder.Eval(e.Row.DataItem, "ItemNo") + "|";
                data += DataBinder.Eval(e.Row.DataItem, "CategoryName") + "|";
                data += DataBinder.Eval(e.Row.DataItem, "UnitQty") + "|";
                data += DataBinder.Eval(e.Row.DataItem, "AnyQty") + "|";
                data += DataBinder.Eval(e.Row.DataItem, "DiscPercent") + "|";
                data += DataBinder.Eval(e.Row.DataItem, "DiscAmount") + "|";
                data += DataBinder.Eval(e.Row.DataItem, "promoprice") + "|";
                data += Utility.GetParameter("id") + "|";
                data += DataBinder.Eval(e.Row.DataItem, "DetailId");

                editBtn.Attributes.Add("data", data);
            }
        }

        protected void gvDetails_OnRowDeleting(Object sender, GridViewDeleteEventArgs e)
        {
            DataTable dt = (DataTable)ViewState["Details"];
            int rowIndex = e.RowIndex + gvDetails.PageIndex * gvDetails.PageSize;
            dt.Rows[rowIndex]["Deleted"] = "true";

            ViewState["Details"] = dt;
            BindPromoDetails();
        }

        protected void gvDetails_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {

            gvDetails.EditIndex = -1;
            BindPromoDetails();
        }

        protected void gvDetails_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            gvDetails.PageIndex = e.NewPageIndex;
            BindPromoDetails();
        }

        protected void gvDetails_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvDetails.EditIndex = e.NewEditIndex;
            int index = e.NewEditIndex;
            DataTable dt = (DataTable)ViewState["Details"];
            tmpDetailId.Value = dt.Rows[index]["DetailId"].ToString();
            tmpPromoCampaignDetID.Value = dt.Rows[index]["PromoCampaignDetID"].ToString();
            tmpCategoryName.Value = dt.Rows[index]["CategoryName"].ToString();
            tmpItemNo.Value = dt.Rows[index]["ItemNo"].ToString();
            tmpItemGroup.Value = dt.Rows[index]["ItemGroupId"].ToString();
            tmpQty.Value = dt.Rows[index]["UnitQty"].ToString();
            tmpAnyQty.Value = dt.Rows[index]["AnyQty"].ToString();
            tmpPromoPrice.Value = dt.Rows[index]["PromoPrice"].ToString();
            tmpDiscPercent.Value = dt.Rows[index]["DiscPercent"].ToString();
            tmpDiscAmount.Value = dt.Rows[index]["DiscAmount"].ToString();
            tmpRetailPrice.Value = dt.Rows[index]["RetailPrice"].ToString();
            tmpUnitPrice.Value = dt.Rows[index]["UnitPrice"].ToString();


            BindPromoDetails();

            ClientScript.RegisterClientScriptBlock(GetType(), "EditDetail", "EditDetail();", true);
        }

        protected void gvDetails_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            gvDetails.EditIndex = -1;
            GridViewRow row = (GridViewRow)gvDetails.Rows[e.RowIndex];
            DataTable dt = (DataTable)ViewState["Details"];

            TextBox qty = (TextBox)row.Cells[1].FindControl("txtQtyGV");
            dt.Rows[e.RowIndex]["UnitQty"] = Int32.Parse(qty.Text ?? "0");

            TextBox anyqty = (TextBox)row.Cells[1].FindControl("txtAnyQtyGV");
            dt.Rows[e.RowIndex]["AnyQty"] = Int32.Parse(anyqty.Text ?? "0");

            TextBox promoprice = (TextBox)row.Cells[1].FindControl("txtPromoPriceGV");
            TextBox discpercent = (TextBox)row.Cells[1].FindControl("txtDiscPercentGV");
            TextBox discamount = (TextBox)row.Cells[1].FindControl("txtDiscAmountGV");

            var retailprice = Convert.ToDecimal(dt.Rows[e.RowIndex]["RetailPrice"]);
            var newpromoprice = Decimal.Parse(promoprice.Text ?? "0");
            var newdiscpercent = Decimal.Parse(discpercent.Text ?? "0");
            var newdiscamount = Decimal.Parse(discamount.Text ?? "0");

            if (newdiscpercent > 0)
            {
                dt.Rows[e.RowIndex]["PromoPrice"] = retailprice - (newdiscpercent / 100 * retailprice);
                dt.Rows[e.RowIndex]["DiscPercent"] = newdiscpercent;
                dt.Rows[e.RowIndex]["DiscAmount"] = 0;
            }
            else if (newdiscamount > 0)
            {
                dt.Rows[e.RowIndex]["PromoPrice"] = retailprice - newdiscamount;
                dt.Rows[e.RowIndex]["DiscPercent"] = 0;
                dt.Rows[e.RowIndex]["DiscAmount"] = newdiscamount;
            }
            else
            {
                dt.Rows[e.RowIndex]["PromoPrice"] = newpromoprice;
                dt.Rows[e.RowIndex]["DiscPercent"] = 0;
                dt.Rows[e.RowIndex]["DiscAmount"] = 0;
            }

            ViewState["Details"] = dt;
            BindPromoDetails();
        }

        protected void cbIsRestrictHourOnly_OnCheckedChanged(object sender, EventArgs e)
        {
            if (cbIsRestrictHourOnly.Checked)
            {
                txtRestrictHourStart.Enabled = true;
                txtRestrictHourEnd.Enabled = true;
            }
            else
            {
                txtRestrictHourStart.Enabled = false;
                txtRestrictHourEnd.Enabled = false;
            }
        }

        protected void onItemEdited(object sender, EventArgs e)
        {
            var lnk = sender as LinkButton;
            var s = lnk.CommandArgument;
            
        }

        protected void btnCopyPromotion_Click(object sender, EventArgs e)
        {
            string OutletFrom = tmpOutletFrom.Value;
            string OutletTo = tmpOutletTo.Value;
            try
            {
                string query = string.Format("Select h.* From PromoCampaignHdr h inner join PromoOutlet o on h.PromoCampaignHdrID = o.PromoCampaignHdrId where h.DateTo >= getdate() and h.CampaignType ='AnyXOffAllItems' and ISNULL(h.Deleted,0) = 0 and ISNULL(o.deleted,0) = 0 and o.OutletName = '{0}'", OutletFrom);
                DataSet ds = DataService.GetDataSet(new QueryCommand(query));

                PromoCampaignHdrCollection col = new PromoCampaignHdrCollection();
                col.Load(ds.Tables[0]);

                if (col.Count() > 0)
                {
                    PromoOutletController cont = new PromoOutletController();

                    for (int i = 0; i < col.Count; i++)
                    {
                        Query qrs = new Query("PromoOutlet");
                        qrs.QueryType = QueryType.Select;
                        qrs.AddWhere(PromoOutlet.Columns.PromoCampaignHdrID, Comparison.Equals, col[i].PromoCampaignHdrID);
                        qrs.AddWhere(PromoOutlet.Columns.OutletName, Comparison.Equals, OutletTo);
                        qrs.AddWhere(PromoOutlet.Columns.Deleted, Comparison.Equals, false);
                        PromoOutletCollection promoCollect = cont.FetchByQuery(qrs);

                        if (promoCollect.Count == 0) 
                        {
                            PromoOutlet outlet = new PromoOutlet();
                            outlet.PromoCampaignHdrID = col[i].PromoCampaignHdrID;
                            outlet.OutletName = OutletTo;
                            outlet.Deleted = false;
                            outlet.Save(Session["username"].ToString());
                        }
                    }

                    lblMsg.Text = "Copy Promotion From Outlet " + OutletFrom +" to Outlet " + OutletTo + " is succeeded.";
                }
                else {
                    lblMsg.Text = "No active promotion that copied to Outlet : " + OutletTo;
                }


            }
            catch (Exception ex) {
                lblMsg.Text = "Error Copy Promotion: " + ex.Message;
                Logger.writeLog("Error Copy Promotion: " + ex.Message);
            }

            BindGrid();
        }

        #endregion
    }
}
