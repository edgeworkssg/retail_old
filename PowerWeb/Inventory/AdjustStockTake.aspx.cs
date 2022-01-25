using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PowerPOS;
using System.Data;
using SubSonic;
using System.Collections;
using System.Text;

namespace PowerWeb.Inventory
{
    public partial class AdjustStockTake : System.Web.UI.Page
    {
        protected Int32 Col_SystemBalQty = 4;
        protected Int32 Col_StockTakeQty = 5;
        protected Int32 Col_defi = 6;
        protected Int32 Col_BatchNo = 8;
        protected Int32 Col_ParValue = 9;
        protected Int32 Col_StockTakeID = 12; 
        private const string SORT_DIRECTION = "SORT_DIRECTION";
        private const string ORDER_BY = "ORDER_BY";

        private List<string> StocktakeIDs
        {
            get
            {
                if (this.ViewState["StockTakeIDs"] == null)
                {
                    this.ViewState["StockTakeIDs"] = new List<string>();
                }
                return this.ViewState["StockTakeIDs"] as List<string>;
            }
        }

        private DataTable StockSource
        {
            get
            {
                if (this.ViewState["StockSource"] == null)
                {
                    DataTable dtStock = new DataTable();
                    dtStock.Columns.Add("StockTakeDate", typeof(DateTime));
                    dtStock.Columns.Add("ItemNo", typeof(string));
                    dtStock.Columns.Add("StockTakeID", typeof(int));
                    dtStock.Columns.Add("InventoryLocationID", typeof(int));
                    dtStock.Columns.Add("StockTakeQty", typeof(decimal));
                    dtStock.Columns.Add("TakenBy", typeof(string));
                    dtStock.Columns.Add("VerifiedBy", typeof(string));
                    dtStock.Columns.Add("AuthorizedBy", typeof(string));
                    dtStock.Columns.Add("IsAdjusted", typeof(bool));
                    dtStock.Columns.Add("Remark", typeof(string));
                    dtStock.Columns.Add("CostOfGoods", typeof(decimal));
                    dtStock.Columns.Add("ItemNam", typeof(string));
                    dtStock.Columns.Add("CategoryName", typeof(string));
                    dtStock.Columns.Add("IsInventory", typeof(bool));
                    dtStock.Columns.Add("InventoryLocationName", typeof(string));
                    dtStock.Columns.Add("AdjustmentHdrRefNo", typeof(string));
                    dtStock.Columns.Add("BalQtyAtEntry", typeof(decimal));
                    dtStock.Columns.Add("AdjustmentQty", typeof(string));
                    dtStock.Columns.Add("Marked", typeof(bool));
                    dtStock.Columns.Add("BatchNo", typeof(string));
                    dtStock.Columns.Add("ParValue", typeof(string));
                    this.ViewState["StockSource"] = dtStock ;
                }
                return this.ViewState["StockSource"] as DataTable;
            }

            set 
            {
                this.ViewState["StockSource"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string username = "";
                UserMst usr = new UserMst();
                if (Session["UserName"] != null)
                {
                    username = Session["UserName"].ToString();
                    usr = new UserMst(username);
                }

                InventoryLocationCollection inv = StockTakeController.GetAllLocationWithOutstandingStockTake();
                if (!String.IsNullOrEmpty(usr.AssignedOutlet) && usr.AssignedOutlet != "ALL")
                {
                    InventoryLocationCollection invLocListToDelete = new InventoryLocationCollection();
                    foreach (InventoryLocation invLoc in inv)
                    {
                        bool isFound = false;
                        foreach (string tmp in usr.AssignedOutletList)
                        {
                            if (tmp == invLoc.InventoryLocationName)
                                isFound = true;
                        }
                        if (!isFound)
                            invLocListToDelete.Add(invLoc);
                    }

                    foreach (InventoryLocation invLocToDelete in invLocListToDelete)
                    {
                        inv.Remove(invLocToDelete);
                    }
                }
                if (inv.Count == 0)
                {
                    ShowMessage("There is no Stock Take data to adjust. All Stock Take has been adjusted");

                }
                InventoryLocation tmp1 = new InventoryLocation();
                tmp1.InventoryLocationID = 0;
                tmp1.InventoryLocationName = "--SELECT--";
                inv.Insert(0, tmp1);
                ddlInventoryLocation.DataSource = inv;
                ddlInventoryLocation.DataValueField = InventoryLocation.Columns.InventoryLocationID;
                ddlInventoryLocation.DataTextField = InventoryLocation.Columns.InventoryLocationName;
                ddlInventoryLocation.DataBind();

                setDate();

            }
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            BindGrid();
            SelectAll();
        }

        protected void ddlInventoryLocation_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            this.StocktakeIDs.Clear();
            BindGrid();
            SelectAll();
        }

        private void BindGrid()
        {
            string sortDir = "ASC";
            string orderBy = "StockTakeID";

            if (ViewState[ORDER_BY] != null)
            {
                orderBy = ViewState[ORDER_BY].ToString();
            }

            if (ViewState[SORT_DIRECTION] != null)
            {
                sortDir = (string)ViewState[SORT_DIRECTION];
            }

            try
            {
                bool showCostPrice = false;

                if (ddlInventoryLocation.SelectedIndex == 0)
                {
                    gvStock.DataSource = null;
                    this.StockSource = null;
                    //gvStock.DataBind();
                }
                else
                {
                    DataTable dt;
                    StockTakeController st = new StockTakeController();


                    dt = st.FetchByLocationWithFilterBatchNoParValue(ddlInventoryLocation.SelectedValue.GetIntValue(), showCostPrice, txtSearch.Text);

                    DataView dv = new DataView(dt);
                    dv.Sort = orderBy + " " + sortDir;
                    gvStock.DataSource = dv;
                    this.StockSource = dt;
                }

                if (!AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.ShowBatchNoStockTake), false))
                {
                    gvStock.Columns[Col_BatchNo].Visible = false;
                }

                if (!AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.ShowParValueStockTake), false))
                {
                    gvStock.Columns[Col_ParValue].Visible = false;
                }

                gvStock.DataBind();
                setDate();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                lblResult.Text = "<span style=\"font-weight:bold; color:#22bb22\">There is no Stock Take data to adjust. All Stock Take has been adjusted</span>";

                ShowMessage("Error" + ex.Message);
            }
        }

        protected void ShowMessage(string message)
        {
            lblResult.Text = "<span style=\"font-weight:bold; color:#22bb22\">" + message + "</span>";
        }

        protected void SelectAll() 
        {
            DataTable dv =this.StockSource;
            foreach (DataRow row in dv.Rows)
            {
                var stockID = row["StockTakeID"].ToString();

                if (!this.StocktakeIDs.Contains(stockID))
                {
                    this.StocktakeIDs.Add(stockID);
                }
            }

            foreach (GridViewRow row in gvStock.Rows)
            {
                CheckBox field = (CheckBox)row.FindControl("CheckBox1");
                field.Checked = true;
            }
        }

        protected void gvStock_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton editBtn = (LinkButton)e.Row.FindControl("editBtn");
                string data = "";
                data += DataBinder.Eval(e.Row.DataItem, "StockTakeID") + "|";
                data += DataBinder.Eval(e.Row.DataItem, "ItemNo") + "|";
                data += DataBinder.Eval(e.Row.DataItem, "ItemName") + "|";
                data += DataBinder.Eval(e.Row.DataItem, "StockTakeQty").ToString().GetDecimalValue().ToString("0.##");

                editBtn.Attributes.Add("data", data);

                var stocktakeid = DataBinder.Eval(e.Row.DataItem, "StockTakeID");
                CheckBox cb1 = (CheckBox)e.Row.FindControl("CheckBox1");
                if (this.StocktakeIDs.Contains(stocktakeid.ToString()))
                {
                    cb1.Checked = true;
                }
                else 
                {
                    cb1.Checked = false;
                }

            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                DataTable dt = ((DataView)gvStock.DataSource).Table;
                if (dt != null && dt.Rows.Count > 0)
                {
                    e.Row.Cells[Col_SystemBalQty].Text = (dt.Compute("SUM(SystemBalQty)", "").ToString()).GetDecimalValue().ToString("0.##"); // totalGrossSales.ToString("N2");
                    e.Row.Cells[Col_StockTakeQty].Text = (dt.Compute("SUM(StockTakeQty)", "").ToString()).GetDecimalValue().ToString("0.##"); // totalGrossSales.ToString("N2");
                    e.Row.Cells[Col_defi].Text = (dt.Compute("SUM(defi)", "").ToString()).GetDecimalValue().ToString("0.##"); // totalGrossSales.ToString("N2");
                }
            }

        }

        protected void BtnSelectAll_Click(object sender, EventArgs e)
        {
            SelectAll();    
        }

        protected void BtnClearSelection_Click(object sender, EventArgs e)
        {
            this.StocktakeIDs.Clear();
            foreach (GridViewRow row in gvStock.Rows)
            {
                CheckBox field = (CheckBox)row.FindControl("CheckBox1");
                field.Checked = false;
            }
        }

        protected void setDate() 
        {
            txtStockDate.Text = DateTime.Now.ToString("dd MMM yyyy");
            DateTime dt = DateTime.Parse("00:00 AM");
            MKB.TimePicker.TimeSelector.AmPmSpec am_pm;
            if (dt.ToString("tt") == "AM")
            {
                am_pm = MKB.TimePicker.TimeSelector.AmPmSpec.AM;
            }
            else
            {
                am_pm = MKB.TimePicker.TimeSelector.AmPmSpec.PM;
            }
            txtTimeStock.SetTime(dt.Hour, dt.Minute, am_pm);
        }

        protected void BtnDeleteSelection_Click(object sender, EventArgs e)
        {
            QueryCommandCollection commands = new QueryCommandCollection();
            try
            {
                foreach (string stocktakeid in this.StocktakeIDs)
                {
                    //CheckBox field = (CheckBox)row.FindControl("CheckBox1");
                    //if (field.Checked)
                    //{
                        //Label StockTakeID = (Label)row.FindControl("lblStockTakeID");
                        StockTake stock = new StockTake(stocktakeid);
                        if (!stock.IsNew)
                        {
                            StockTake.Delete(stock.StockTakeID);
                        }
                    //}
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                ShowMessage("Error delete :" + ex.Message);
            }
            BindGrid();
        }

        protected bool isLineEmpty()
        {
            bool isEmpty = true;
            foreach (GridViewRow row in gvStock.Rows)
            {
                CheckBox field = (CheckBox)row.FindControl("CheckBox1");
                if (field.Checked)
                {
                    isEmpty = false;
                }
            }

            return isEmpty;
        }

        protected void BtnSetStockDateTime_Click(object sender, EventArgs e)
        {
            try
            {
                string dateStock = string.Format("{0} {1}", StockDateDate.Value.ToString(), StockDateTime.Value.ToString());
                DateTime datestock = dateStock.GetDateTimeValue("dd MMM yyyy hh:mm:ss tt");
                if (isLineEmpty())
                {
                    ShowMessage("Please at least select one!");
                }
                else
                {
                    if (ddlInventoryLocation.SelectedIndex != 0)
                    {

                        ArrayList markedList = new ArrayList();
                        foreach (GridViewRow row in gvStock.Rows)
                        {
                            CheckBox field = (CheckBox)row.FindControl("CheckBox1");
                            if (field.Checked)
                            {
                                Label StockTakeID = (Label)row.FindControl("lblStockTakeID");
                                markedList.Add(StockTakeID.Text);
                            }
                        }

                        Query qr;
                        for (int i = 0; i < markedList.Count; i++)
                        {
                            qr = StockTake.CreateQuery();
                            qr.QueryType = QueryType.Update;
                            qr.AddWhere(StockTake.Columns.StockTakeID, Comparison.In, markedList);
                            qr.AddUpdateSetting(StockTake.Columns.StockTakeDate, datestock);
                            qr.Execute();
                        }

                        ShowMessage("Stock Take Date is updated");

                        BindGrid();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                ShowMessage("Error Update Stock Take:" + ex.Message);
            }
        }

        protected void BtnExport_Click(object sender, EventArgs e)
        {

            try
            {
                string filename = "StockTake" + DateTime.Now.ToString("ddMMyyyy") + ".txt";
                var exportdocno = ExportStockTakeDocNo.Value;
                string sql = @"
                    select ISNULL(i.attributes1,'') as [Plant], ISNULL(i.attributes2,'') [Storage Location], i.ItemNo as [Material Code], ISNULL(st.userfld1,'') as [Batch No], 
	                    1 as [Stock Type], cast(st.StockTakeQty as decimal(18,2)) as Quantity, ISNULL(i.userfld1,'') as [UOM],
                        CASE WHEN st.StockTakeQty = 0 THEN 'X' else ' ' END as [Zero Count]
                    from stocktake st 
                    inner join Item i on st.ItemNo = i.ItemNo
                    where ISNULL(st.Userfld3,'') = '{0}'";

                sql = string.Format(sql, exportdocno);
                DataTable dt = DataService.GetDataSet(new QueryCommand(sql)).Tables[0];

                if (dt != null)
                {
                    StringBuilder st = new StringBuilder();

                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        st.Append(dt.Columns[i].ColumnName).Append("\t");
                    }
                    st.AppendLine();

                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        for (int k = 0; k < dt.Columns.Count; k++)
                        {
                            st.Append(dt.Rows[j][k].ToString()).Append("\t");
                        }
                        st.AppendLine();
                    }


                    Response.Clear();
                    Response.ClearHeaders();

                    Response.AddHeader("Content-Length", st.Length.ToString());
                    Response.ContentType = "text/plain";
                    Response.AppendHeader("content-disposition", "attachment;filename=" + filename + "_Result.txt");

                    Response.Write(st);
                    Response.End();
                }
                else {
                    throw new  Exception("No data to be downloaded");
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                ShowMessage("Error Update Stock Take:" + ex.Message);
            }

        }

        protected void BtnExportWardTopUp_Click(object sender, EventArgs e)
        {

            try
            {
                string filename = "WardTopUp" + DateTime.Now.ToString("ddMMyyyy") + ".txt";
                var exportdocno = ExportWardStockTakeDocNo.Value;

                //Get userfld for DepartmentOU 
                string fieldDepartmentOu = "' ' as [Department OU] ";
                AppSettingCollection qr1 = new AppSettingCollection();
                qr1.Where(AppSetting.Columns.AppSettingValue, "Department OU");
                qr1.Load();

                if (qr1.Count() > 0)
                { 
                    string userfld1 = qr1[0].AppSettingKey;
                    fieldDepartmentOu = "ISNULL(iq." + userfld1.Substring(6, userfld1.Length - 6) + ",'') as [Department OU] ";
                }

                string fieldNursingOu = "' ' as [Nursing OU] ";
                AppSettingCollection qr2 = new AppSettingCollection();
                qr2.Where(AppSetting.Columns.AppSettingValue, "Nursing OU");
                qr2.Load();

                if (qr2.Count() > 0)
                {
                    string userfld1 = qr2[0].AppSettingKey;
                    fieldNursingOu = "ISNULL(iq." + userfld1.Substring(6, userfld1.Length - 6) + ",'') as [Nursing OU] ";
                }

                string fieldParValue =  "iq.Userfld1";
                AppSettingCollection qr3 = new AppSettingCollection();
                qr3.Where(AppSetting.Columns.AppSettingValue, "Par Value");
                qr3.Load();

                if (qr2.Count() > 0)
                {
                    string userfld1 = qr3[0].AppSettingKey;
                    fieldParValue = "ISNULL(iq." + userfld1.Substring(6, userfld1.Length - 6) + ",'') ";
                }

                string sql = @"
                    select st.ItemNo as [Material Code], ISNULL(i.Attributes1,'') [Plant], ISNULL(i.Attributes2,'') as [SLOC], 
	                    {0}, {1}, cast(st.StockTakeQty as decimal(18,2)) as Qty, 
                        CASE WHEN CHARINDEX(' ', {3}) > 0 
	                    then RIGHT(ISNULL({3},''), LEN(iq.userfld1) - CHARINDEX(' ', {3}))  else '' end as [UOM]
                   from StockTake st
                    inner join Item i on st.ItemNo = i.ItemNo 
                    left outer join ItemQuantityTrigger iq on i.ItemNo = iq.ItemNo and Iq.InventoryLocationId = st.InventoryLocationID 
                    where ISNULL(st.Userfld3,'') = '{2}' and ISNULL(iq.Deleted,0) = 0 ";

                sql = string.Format(sql, fieldDepartmentOu, fieldNursingOu, exportdocno, fieldParValue);
                DataTable dt = DataService.GetDataSet(new QueryCommand(sql)).Tables[0];

                if (dt != null)
                {
                    StringBuilder st = new StringBuilder();

                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        st.Append(dt.Columns[i].ColumnName).Append("\t");
                    }
                    st.AppendLine();

                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        for (int k = 0; k < dt.Columns.Count; k++)
                        {
                            st.Append(dt.Rows[j][k].ToString()).Append("\t");
                        }
                        st.AppendLine();
                    }


                    Response.Clear();
                    Response.ClearHeaders();

                    Response.AddHeader("Content-Length", st.Length.ToString());
                    Response.ContentType = "text/plain";
                    Response.AppendHeader("content-disposition", "attachment;filename=" + filename + "_Result.txt");

                    Response.Write(st);
                    Response.End();
                }
                else
                {
                    throw new Exception("No data to be downloaded");
                }

            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                ShowMessage("Error Update Stock Take:" + ex.Message);
            }

        }

        protected void BtnUpdateStockTake_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(UpdateStockTakeID.Value))
                {
                    StockTake st = new StockTake(UpdateStockTakeID.Value);
                    st.StockTakeQty = UpdateQty.Value.GetDecimalValue();
                    st.AdjustmentQty = UpdateQty.Value.GetDecimalValue() - st.BalQtyAtEntry;
                    st.Save(Session["username"].ToString());

                    BindGrid();
                    UpdateStockTakeID.Value = "0";
                    UpdateQty.Value = "0";
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                ShowMessage("Error Update Stock Take:" + ex.Message);
            }

        }

        protected void CheckBox1_OnCheckedChanged(Object sender, EventArgs args)
        {
            CheckBox checkbox = (CheckBox)sender;
            GridViewRow row = (GridViewRow)checkbox.NamingContainer;
            Label lblStockTakeID = (Label)row.FindControl("lblStockTakeID");
            string stocktakeid = lblStockTakeID.Text;

            if (checkbox.Checked)
            {
                if (!this.StocktakeIDs.Contains(stocktakeid))
                    this.StocktakeIDs.Add(stocktakeid);
            }
            else
            {

                if (this.StocktakeIDs.Contains(stocktakeid))
                    this.StocktakeIDs.Remove(stocktakeid);
                
            }
        }
        
        protected void BtnAdjust_Click(object sender, EventArgs e)
        {
            try
            {
                string dateStock = string.Format("{0} {1}", StockDateDate.Value.ToString(), StockDateTime.Value.ToString());
                DateTime datestock = dateStock.GetDateTimeValue("dd MMM yyyy hh:mm:ss tt");

                if (isLineEmpty())
                {
                    ShowMessage("Please at least select one!");
                }
                else
                {
                    if (ddlInventoryLocation.SelectedIndex != 0)
                    {
                        string stockdoc = StockDocumentNo.Value;

                        string checkSQL = @"select distinct userfld3 as[StockDocumentNo] from stocktake where userfld3 = '{0}'";

                        checkSQL = string.Format(checkSQL, stockdoc);
                        DataTable sq = DataService.GetDataSet(new QueryCommand(checkSQL)).Tables[0];

                        if (sq.Rows.Count > 0)
                        {
                            ShowMessage("Stock Take Document No " + stockdoc + " already used");
                        }
                        else
                        {

                            //ArrayList markedList = new ArrayList();
                            //foreach (GridViewRow row in gvStock.Rows)
                            //{
                            //    CheckBox field = (CheckBox)row.FindControl("CheckBox1");
                            //    if (field.Checked)
                            //    {
                            //        Label StockTakeID = (Label)row.FindControl("lblStockTakeID");
                            //        markedList.Add(StockTakeID.Text);
                            //    }
                            //}

                            Query qr;
                            for (int i = 0; i < this.StocktakeIDs.Count; i++)
                            {
                                StockTake st = new StockTake(StocktakeIDs[i]);
                                string status = "";
                                decimal BalQtyAtEntry = InventoryController.GetStockBalanceQtyByItemByDate(st.ItemNo, st.InventoryLocationID, datestock, out status);


                                qr = StockTake.CreateQuery();
                                qr.QueryType = QueryType.Update;
                                qr.AddWhere(StockTake.Columns.StockTakeID, Comparison.Like, StocktakeIDs[i]);
                                qr.AddUpdateSetting(StockTake.Columns.StockTakeDate, datestock);
                                qr.AddUpdateSetting(StockTake.Columns.BalQtyAtEntry, BalQtyAtEntry);
                                qr.AddUpdateSetting(StockTake.Columns.AdjustmentQty, st.StockTakeQty - BalQtyAtEntry);
                                qr.AddUpdateSetting(StockTake.Columns.Marked, true);
                                qr.Execute();
                            }
                            
                            StockTakeController ct = new StockTakeController();
                            if (ct.CorrectStockTakeDiscrepancyWithStockDocument(Session["username"].ToString(), ddlInventoryLocation.SelectedValue.GetIntValue(), StockDocumentNo.Value))
                            {
                                Query qr1;
                                for (int i = 0; i < this.StocktakeIDs.Count; i++)
                                {
                                    qr1 = StockTake.CreateQuery();
                                    qr1.QueryType = QueryType.Update;
                                    qr1.AddWhere(StockTake.Columns.StockTakeID, Comparison.In, StocktakeIDs);
                                    //qr1.AddUpdateSetting(StockTake.Columns.StockTakeDate, datestock);
                                    qr1.AddUpdateSetting(StockTake.Columns.Userfld3, stockdoc);
                                    //qr1.AddUpdateSetting(StockTake.Columns.Marked, true);
                                    qr1.Execute();
                                }
                                ShowMessage("Stock Take adjusted");
                            }
                            else
                            {
                                ShowMessage("Stock Take Adjustment Failed");
                            }

                            

                            BindGrid();
                            SelectAll();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                ShowMessage("Error Update Stock Take:" + ex.Message);
            }

        }

        protected void BtnAddMissedOutItems_Click(object sender, EventArgs e)
        {
            string status = "";
            try
            {
                if (ddlInventoryLocation.SelectedIndex != 0)
                {
                    DataTable dt = StockTakeController.FetchMissedOutItemWithFilter(ddlInventoryLocation.SelectedValue.GetIntValue(), "");

                    if (dt.Rows.Count > 0)
                    {
                        InventoryController invctrl = new InventoryController();
                        invctrl.SetInventoryDate(DateTime.Now);
                        invctrl.SetInventoryLocation(ddlInventoryLocation.SelectedValue.GetIntValue());

                        for(int i = 0; i < dt.Rows.Count; i++)
                        {
                            string itemno = dt.Rows[i]["ItemNo"].ToString().Trim();
                            invctrl.AddItemIntoInventoryStockTake(itemno, 0, 0, out status);
                        }

                        if (!invctrl.CreateStockTakeEntries(Session["username"].ToString(), Session["username"].ToString(), Session["username"].ToString(), out status))
                        {
                            throw new Exception(status);
                        }

                        BindGrid();
                        ShowMessage("Missed out items succesfully added.");
                    }
                }
                else
                    ShowMessage("Please Select Inventory Location!");
                
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                ShowMessage("Error Add Missed Out Items: " + ex.Message);
            }

        }
        

        protected void ddlPages_SelectedIndexChanged(Object sender, EventArgs e)
        {
            GridViewRow gvrPager = gvStock.BottomPagerRow;
            DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages");
            gvStock.PageIndex = ddlPages.SelectedIndex;
            // a method to populate your grid
            BindGrid();
        }

        protected void gvStock_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvStock.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void gvStock_Sorting(object sender, GridViewSortEventArgs e)
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

        protected void gvStock_DataBound(object sender, EventArgs e)
        {

            GridViewRow gvrPager = gvStock.BottomPagerRow;
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
                for (int i = 0; i < gvStock.PageCount; i++)
                {
                    int intPageNumber = i + 1;
                    ListItem lstItem = new ListItem(intPageNumber.ToString());
                    if (i == gvStock.PageIndex)
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
                DataSet ds = gvStock.DataSource as DataSet;
                if (ds != null)
                {
                    itemCount = ds.Tables[0].Rows.Count;
                }

                string pageCount = "<b>" + gvStock.PageCount.ToString() + "</b>";
                lblPageCount.Text = pageCount;
            }

            Button btnPrev = (Button)gvrPager.Cells[0].FindControl("btnPrev");
            Button btnNext = (Button)gvrPager.Cells[0].FindControl("btnNext");
            Button btnFirst = (Button)gvrPager.Cells[0].FindControl("btnFirst");
            Button btnLast = (Button)gvrPager.Cells[0].FindControl("btnLast");

            //now figure out what page we're on
            if (gvStock.PageIndex == 0)
            {
                btnPrev.Enabled = false;
                btnFirst.Enabled = false;
            }
            else if (gvStock.PageIndex + 1 == gvStock.PageCount)
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
    }
}
