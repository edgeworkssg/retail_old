using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using PowerPOS;
using PowerPOS.Container;
using SubSonic;

namespace PowerWeb.Inventory
{
    public partial class EditPO : PageBase
    {
        PurchaseOdrController invCtrl;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

                #region Load Supplier
                var suppliers = new List<Supplier>();
                suppliers.Add(new Supplier { SupplierID = 0, SupplierName = "ALL" });
                suppliers.AddRange(new SupplierController().FetchAll()
                                                   .OrderBy(o => o.SupplierName)
                                                   .ToList());
                cbSupplier.DataValueField = "SupplierID";
                cbSupplier.DataTextField = "SupplierName";
                cbSupplier.DataSource = suppliers;
                cbSupplier.DataBind();
                #endregion
                DataTable dtGST = new DataTable();
                dtGST.Columns.Add("ID");
                dtGST.Columns.Add("GSTType");
                DataRow dr = dtGST.NewRow();
                dr["ID"] = "1";
                dr["GSTType"] = "Exclusive";
                dtGST.Rows.Add(dr);

                dr = dtGST.NewRow();
                dr["ID"] = "2";
                dr["GSTType"] = "Inclusive";
                dtGST.Rows.Add(dr);

                dr = dtGST.NewRow();
                dr["ID"] = "0";
                dr["GSTType"] = "Non GST";
                dtGST.Rows.Add(dr);

                cbGST.DataSource = dtGST;
                cbGST.DataTextField = "GSTType";
                cbGST.DataValueField = "ID";
                cbGST.DataBind();
                cbGST.SelectedValue = "1";

                int rangeOfDays = txtRangeOfDays.Text.GetIntValue();

                if (rangeOfDays == 0)
                    rangeOfDays = 1;

                DateTime startDate1, startDate2, startDate3, endDate1, endDate2, endDate3;
                startDate1 = DateTime.Today.AddDays(rangeOfDays * -1);
                startDate2 = DateTime.Today.AddDays(rangeOfDays * -2);
                startDate3 = DateTime.Today.AddDays(rangeOfDays * -3);
                endDate1 = startDate1.AddDays(rangeOfDays).AddSeconds(-1);
                endDate2 = startDate2.AddDays(rangeOfDays).AddSeconds(-1);
                endDate3 = startDate3.AddDays(rangeOfDays).AddSeconds(-1);

                gvItem.Columns[SalesPeriod1].HeaderText = startDate1.ToString("d MMM yyyy").Replace(DateTime.Today.Year.ToString(), "").Trim() + " - " + endDate1.ToString("d MMM yyyy").Replace(DateTime.Today.Year.ToString(), "").Trim();;
                gvItem.Columns[SalesPeriod2].HeaderText = startDate2.ToString("d MMM yyyy").Replace(DateTime.Today.Year.ToString(), "").Trim() + " - " + endDate2.ToString("d MMM yyyy").Replace(DateTime.Today.Year.ToString(), "").Trim();
                gvItem.Columns[SalesPeriod3].HeaderText = startDate3.ToString("d MMM yyyy").Replace(DateTime.Today.Year.ToString(), "").Trim() + " - " + endDate3.ToString("d MMM yyyy").Replace(DateTime.Today.Year.ToString(), "").Trim();

                string refNo = Request.QueryString["RefNo"] + "";
                if (!string.IsNullOrEmpty(refNo))
                {
                    invCtrl = new PurchaseOdrController();
                    invCtrl.LoadConfirmedPurchaseOrderController(refNo);
                    txtPurchaseOrderHdrRefNo.Text = string.IsNullOrEmpty(invCtrl.GetCustomRefNo()) ? invCtrl.GetPurchaseOrderHdrRefNo() : invCtrl.GetCustomRefNo();
                    txtPurchaseOrderDate.Text = invCtrl.GetPurchaseOrderDate().ToString("yyyy-MM-dd HH:mm:ss");
                    txtPurchaseOrderHdrRefNo.Enabled = false;
                    txtPurchaseOrderDate.Enabled = false;
                    Supplier supp = new Supplier(Supplier.Columns.SupplierID, invCtrl.InvHdr.Supplier);
                    cbSupplier.SelectedValue = invCtrl.getSupplier();
                    cbGST.SelectedValue = invCtrl.getGSTType();
                    BindGrid();

                }
            }
        }

        private void BindGrid()
        {
            if (invCtrl == null)
            {
                string refNo = Request.QueryString["RefNo"] + "";
                invCtrl = new PurchaseOdrController();
                invCtrl.LoadConfirmedPurchaseOrderController(refNo);
            }
            string status = "";

            int rangeOfDays = txtRangeOfDays.Text.GetIntValue();

            if (rangeOfDays == 0)
                rangeOfDays = 1;

            txtRangeOfDays.Text = rangeOfDays.ToString();

            gvItem.DataSource = invCtrl.FetchUnSavedPurchaseOrderItemsWithDetailDeletedSupplierPortal(UserInfo.username, rangeOfDays.ToString(), out status);
            gvItem.DataBind();

            DateTime startDate1, startDate2, startDate3, endDate1, endDate2, endDate3;
            startDate1 = DateTime.Today.AddDays(rangeOfDays * -1);
            startDate2 = DateTime.Today.AddDays(rangeOfDays * -2);
            startDate3 = DateTime.Today.AddDays(rangeOfDays * -3);
            endDate1 = startDate1.AddDays(rangeOfDays).AddSeconds(-1);
            endDate2 = startDate2.AddDays(rangeOfDays).AddSeconds(-1);
            endDate3 = startDate3.AddDays(rangeOfDays).AddSeconds(-1);

            gvItem.Columns[SalesPeriod1].HeaderText = startDate1.ToString("d MMM yyyy").Replace(DateTime.Today.Year.ToString(), "").Trim() + " - " + endDate1.ToString("d MMM yyyy").Replace(DateTime.Today.Year.ToString(), "").Trim();;
            gvItem.Columns[SalesPeriod2].HeaderText = startDate2.ToString("d MMM yyyy").Replace(DateTime.Today.Year.ToString(), "").Trim() + " - " + endDate2.ToString("d MMM yyyy").Replace(DateTime.Today.Year.ToString(), "").Trim();
            gvItem.Columns[SalesPeriod3].HeaderText = startDate3.ToString("d MMM yyyy").Replace(DateTime.Today.Year.ToString(), "").Trim() + " - " + endDate3.ToString("d MMM yyyy").Replace(DateTime.Today.Year.ToString(), "").Trim();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string refNo = Request.QueryString["RefNo"] + "";
                if (!string.IsNullOrEmpty(refNo))
                {
                    QueryCommandCollection col = new QueryCommandCollection();
                    foreach (GridViewRow rw in gvItem.Rows)
                    {
                        if (rw.Visible)
                        {
                            string detrefno = ((Label)rw.FindControl("lblPurchaseOrderDetRefNo")).Text;
                            PurchaseOrderDet pd = new PurchaseOrderDet(detrefno);
                            if (pd.IsLoaded && pd.PurchaseOrderDetRefNo != "")
                            {
                                TextBox ss = rw.FindControl("txtQuantity") as TextBox;
                                TextBox ss1 = rw.FindControl("txtCostOfGoods") as TextBox;

                                decimal temp = 0;
                                decimal tempdec = 0;
                                bool isChanged = false;
                                if (decimal.TryParse(ss.Text, out temp))
                                {
                                    pd.PackingQuantity = temp;
                                    pd.Quantity = temp * pd.Userfloat4.GetValueOrDefault(1);
                                    pd.RemainingQty = temp * pd.Userfloat4.GetValueOrDefault(1);
                                    isChanged = true;
                                }
                                if (decimal.TryParse(ss1.Text, out tempdec))
                                {
                                    pd.PackingSizeCost = tempdec;
                                    pd.CostOfGoods = pd.PackingSizeCost / pd.Userfloat4.GetValueOrDefault(1);
                                    pd.FactoryPrice = pd.CostOfGoods;
                                    isChanged = true;
                                }

                                if (isChanged)
                                {
                                    col.Add(pd.GetUpdateCommand(UserInfo.username));
                                    PurchaseOrderHdr ph = new PurchaseOrderHdr(pd.PurchaseOrderHdrRefNo);
                                    if (ph != null && ph.PurchaseOrderHdrRefNo == pd.PurchaseOrderHdrRefNo) ;
                                    col.Add(ph.GetUpdateCommand(UserInfo.username));
                                }
                            }
                        }
                    }
                    DataService.ExecuteTransaction(col);
                }
                BindGrid();
            
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex.Message);
                lblError.Text = string.Format("Error save PO: {0}", ex.Message);
            }
        }

        protected int Amount = 10;
        protected int SalesPeriod1 = 14;
        protected int SalesPeriod2 = 15;
        protected int SalesPeriod3 = 16;

        protected void cbShowPreviousSales_OnCheckedChanged(object sender, EventArgs e)
        {

            if (cbShowPreviousSales.Checked)
            {
                gvItem.Columns[SalesPeriod1].Visible = true;
                gvItem.Columns[SalesPeriod2].Visible = true;
                gvItem.Columns[SalesPeriod3].Visible = true;
            }
            else {
                gvItem.Columns[SalesPeriod1].Visible = false;
                gvItem.Columns[SalesPeriod2].Visible = false;
                gvItem.Columns[SalesPeriod3].Visible = false;
            }
        }

        protected void txtRangeOfDays_OnTextChanged(object sender, EventArgs e)
        {
            int rangeOfDays = txtRangeOfDays.Text.GetIntValue();

            if (rangeOfDays == 0)
                rangeOfDays = 1;

            txtRangeOfDays.Text = rangeOfDays.ToString();

            DateTime startDate1, startDate2, startDate3, endDate1, endDate2, endDate3;
            startDate1 = DateTime.Today.AddDays(rangeOfDays * -1);
            startDate2 = DateTime.Today.AddDays(rangeOfDays * -2);
            startDate3 = DateTime.Today.AddDays(rangeOfDays * -3);
            endDate1 = startDate1.AddDays(rangeOfDays).AddSeconds(-1);
            endDate2 = startDate2.AddDays(rangeOfDays).AddSeconds(-1);
            endDate3 = startDate3.AddDays(rangeOfDays).AddSeconds(-1);

            gvItem.Columns[SalesPeriod1].HeaderText = startDate1.ToString("d MMM yyyy").Replace(DateTime.Today.Year.ToString(), "").Trim() + " - " + endDate1.ToString("d MMM yyyy").Replace(DateTime.Today.Year.ToString(), "").Trim();
            gvItem.Columns[SalesPeriod2].HeaderText = startDate2.ToString("d MMM yyyy").Replace(DateTime.Today.Year.ToString(), "").Trim() + " - " + endDate2.ToString("d MMM yyyy").Replace(DateTime.Today.Year.ToString(), "").Trim();
            gvItem.Columns[SalesPeriod3].HeaderText = startDate3.ToString("d MMM yyyy").Replace(DateTime.Today.Year.ToString(), "").Trim() + " - " + endDate3.ToString("d MMM yyyy").Replace(DateTime.Today.Year.ToString(), "").Trim();

            string status = "";
            string refNo = Request.QueryString["RefNo"] + "";
            if (!string.IsNullOrEmpty(refNo))
            {
                invCtrl = new PurchaseOdrController();
                invCtrl.LoadConfirmedPurchaseOrderController(refNo);

                foreach (GridViewRow dRow in gvItem.Rows)
                {
                    string itemno = dRow.Cells[0].Text;

                    Label sp1 = dRow.FindControl("lblSalesPeriod1") as Label;
                    Label sp2 = dRow.FindControl("lblSalesPeriod2") as Label;
                    Label sp3 = dRow.FindControl("lblSalesPeriod3") as Label;
                    

                    decimal salesPeriod1 = InventoryController.InventoryFetchItemSales(itemno, invCtrl.getSupplier().GetIntValue(), "1",
                        rangeOfDays.ToString(), invCtrl.GetInventoryLocationID(), UserInfo.username, out status);
                    decimal salesPeriod2 = InventoryController.InventoryFetchItemSales(itemno, invCtrl.getSupplier().GetIntValue(), "2",
                        rangeOfDays.ToString(), invCtrl.GetInventoryLocationID(), UserInfo.username, out status);
                    decimal salesPeriod3 = InventoryController.InventoryFetchItemSales(itemno, invCtrl.getSupplier().GetIntValue(), "3",
                        rangeOfDays.ToString(), invCtrl.GetInventoryLocationID(), UserInfo.username, out status);

                    sp1.Text = String.Format("{0:0.####}", salesPeriod1);
                    sp2.Text = String.Format("{0:0.####}", salesPeriod2);
                    sp3.Text = String.Format("{0:0.####}", salesPeriod3);
                }
            }
        }

        protected void gvItem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Footer) 
                {
                    decimal amount = 0;
                    foreach (GridViewRow dRow in gvItem.Rows)
                    {
                        if (dRow.Visible)
                        {
                            TextBox ss = dRow.FindControl("txtQuantity") as TextBox;
                            TextBox ss1 = dRow.FindControl("txtCostOfGoods") as TextBox;

                            decimal amt = ss.Text.GetDecimalValue() * ss1.Text.GetDecimalValue();

                            Label ss3 = dRow.FindControl("lblAmount") as Label;
                            ss3.Text = String.Format("{0:0.####}", amt);
                            amount += amt;
                        }
                    }

                    e.Row.Cells[Amount].Text = String.Format("{0:0.####}", amount);
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        protected void RecalculateAmount(object sender, EventArgs e)
        {
            CalculateTotal();
        }

        protected void CalculateTotal()
        {
            try
            {

                decimal amount = 0;
                foreach (GridViewRow dRow in gvItem.Rows)
                {
                    if (dRow.Visible)
                    {
                        TextBox ss = dRow.FindControl("txtQuantity") as TextBox;
                        TextBox ss1 = dRow.FindControl("txtCostOfGoods") as TextBox;

                        decimal amt = ss.Text.GetDecimalValue() * ss1.Text.GetDecimalValue();

                        Label ss3 = dRow.FindControl("lblAmount") as Label;
                        ss3.Text = String.Format("{0:0.####}", amt);
                        amount += amt;
                    }
                }
                gvItem.FooterRow.Cells[Amount].Text = String.Format("{0:0.####}", amount);
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        protected void gvItem_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string arg = e.CommandArgument.ToString();
            if (e.CommandName == "DeleteDetail")
                DeleteDetailPO(arg);            
        }

        private void DeleteDetailPO(string detailRefNo)
        {
            try
            {
                PurchaseOrderDet det = new PurchaseOrderDet(detailRefNo);

                if (!det.IsNew)
                {
                    QueryCommandCollection col = new QueryCommandCollection();

                    Query q = new Query("PurchaseOrderDet");
                    q.AddUpdateSetting(PurchaseOrderDet.UserColumns.IsDetailDeleted, true);
                    q.AddUpdateSetting(PurchaseOrderDet.Columns.ModifiedBy, Session["username"] + "");
                    q.AddUpdateSetting(PurchaseOrderDet.Columns.ModifiedOn, DateTime.Now);
                    q.AddWhere(PurchaseOrderDet.Columns.PurchaseOrderDetRefNo, detailRefNo);

                    col.Add(q.BuildUpdateCommand());

                    DataService.ExecuteTransaction(col);

                    foreach (GridViewRow rw in gvItem.Rows)
                    {
                        string detrefno = ((Label)rw.FindControl("lblPurchaseOrderDetRefNo")).Text;
                        if (detrefno == detailRefNo)
                            rw.Visible = false;
                    }

                    CalculateTotal();
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(string.Format("Error delete detail: {0}", ex.Message));
                lblError.Text = string.Format("Error delete detail: {0}", ex.Message);
            }
        }
    }
}
