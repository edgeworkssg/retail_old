using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using SubSonic;
using PowerPOS.Container;
using System.Collections;
using System.Configuration;
using System.IO.Compression;
using System.IO;
using LanguageManager = PowerInventory.Properties.InventoryLanguage;
using System.Net;
using PowerPOSLib.Controller;

namespace PowerInventory
{
    public partial class frmStockIn : frmInventoryParent
    {
        private bool useCustomRefNo;
        private bool generateNoInServer;

        public frmStockIn()
        {
            InitializeComponent();

            if (!PointOfSaleInfo.IntegrateWithInventory)
                GetCustomGRNoSettingFromServer();

            useCustomRefNo = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.UseCustomNo), false);
            generateNoInServer = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.GenerateCustomNoInServer), false);
        }

        private void frmStockIn_Load(object sender, EventArgs e)
        {
            try
            {
                if (!isTherePendingStockTake)
                {
                    //showCostPrice = true;
                    showOnHandQty = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.ShowBalanceQuantityOnTransaction), false);
                    HideStockBalanceAndFactoryPrice();
                    btnSave.Text = LanguageManager.RECEIVE;
                    this.Text = LanguageManager.GOODS_RECEIVE;
                    AddAdditionalInformation();
                    LoadInventoryController();
                    invCtrl.SetGSTRule(cmbGST.SelectedValue.ToString().GetIntValue());
                    if (useCustomRefNo)
                    {
                        if (generateNoInServer)
                        {
                            txtRefNo.Text = InventoryController.CreateNewCustomRefNo();
                        }
                        else
                        {
                            invCtrl.SetCustomRefNo(InventoryController.CreateNewCustomRefNo());
                            txtRefNo.Text = invCtrl.GetCustomRefNo();
                        }
                    }

                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.OnlyAllowInCurrentInventoryLocation), false))
                    {
                        //InventoryLocation tmp = new InventoryLocation(PointOfSaleInfo.InventoryLocationID);
                        for (int i = 0; i < cmbLocation.Items.Count; i++)
                        {
                            if (((InventoryLocation)(cmbLocation.Items[i])).InventoryLocationID == PointOfSaleInfo.InventoryLocationID)
                            {
                                cmbLocation.SelectedIndex = i;
                                invCtrl.SetInventoryLocation(((InventoryLocation)(cmbLocation.SelectedValue)).InventoryLocationID);
                                cmbLocation.Enabled = false;
                            }
                        }

                    }
                    btnSave.Click += new EventHandler(this.btnSave_Click);
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show(LanguageManager.Error_encounter_ + ex.Message);
            }

        }
        private TextBox txtPurchaseOrder;
        private TextBox txtVendorInvoiceNo;
        private ComboBox cmbSupplier;
        private TextBox txtFreightCharges;
        private TextBox txtDiscount;
        private TextBox txtExchangeRate;
        private ComboBox cmbCurrencies;
        private ComboBox cmbGST;
        private Label txtGSTAmount;
        private bool isLoaded;

        private TextBox txtCustomField1 = new TextBox();
        private TextBox txtCustomField2 = new TextBox();
        private TextBox txtCustomField3 = new TextBox();
        private TextBox txtCustomField4 = new TextBox();
        private TextBox txtCustomField5 = new TextBox();
        private TextBox txtAdditionalCost1 = new TextBox();
        private TextBox txtAdditionalCost2 = new TextBox();
        private TextBox txtAdditionalCost3 = new TextBox();
        private TextBox txtAdditionalCost4 = new TextBox();
        private TextBox txtAdditionalCost5 = new TextBox();

        private void txtPurchaseOrder_Leave(object sender, EventArgs e)
        {

            if (txtPurchaseOrder.Text.Length > 0 && !isLoaded)
            {
                PurchaseOdrController inv = new PurchaseOdrController();
                string status = "";
                bool result = inv.LoadPurchaseOrder(txtPurchaseOrder.Text, out status);
                if (result)
                {
                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.AllowLoadUnapprovedPOInGR), false) || inv.isApproved())
                    {

                        LoadInventoryController();
                        if (useCustomRefNo)
                        {
                            if (generateNoInServer)
                            {
                                txtRefNo.Text = InventoryController.CreateNewCustomRefNo();
                            }
                            else
                            {
                                invCtrl.SetCustomRefNo(InventoryController.CreateNewCustomRefNo());
                                txtRefNo.Text = invCtrl.GetCustomRefNo();
                            }
                        }

                        cmbLocation.Text = inv.GetInventoryLocation();
                        cmbSupplier.Text = inv.getSupplierName();
                        txtFreightCharges.Text = inv.GetFreightCharges().ToString("N");
                        txtExchangeRate.Text = inv.getExchangeRate().ToString("N");
                        //txtDiscount.Text = inv.getDiscount().ToString("N2");

                        invCtrl.SetSupplier(inv.getSupplier());
                        invCtrl.SetFreightCharges(inv.GetFreightCharges());
                        invCtrl.SetExchangeRate(inv.getExchangeRate());
                        //invCtrl.SetDiscount(inv.getDiscount());
                        invCtrl.SetPurchaseOrder(txtPurchaseOrder.Text);

                        if (inv.hasGSTRule())
                            cmbGST.SelectedValue = inv.getGSTType();
                        else
                            cmbGST.SelectedValue = "0";
                        invCtrl.SetGSTRule(int.Parse(cmbGST.SelectedValue.ToString()));

                        if (cmbCurrencies.Items.Count > 0)
                        {
                            int CurrencyID = inv.getCurrencyID();
                            cmbCurrencies.SelectedIndex = 0;
                            for (int i = 0; i < cmbCurrencies.Items.Count; i++)
                            {
                                if (((Currency)cmbCurrencies.Items[i]).CurrencyId == CurrencyID)
                                {
                                    cmbCurrencies.SelectedIndex = i;
                                }
                            }
                        }

                        int tmp = 0;
                        if (int.TryParse(cmbSupplier.SelectedValue.ToString(), out tmp))
                        {
                            Supplier s = new Supplier(tmp);
                            if (s != null && s.SupplierID > -1)
                            {
                                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.GoodsReceiveGSTRuleFromSupplier), false))
                                {
                                    invCtrl.SetGSTRule(s.Userint1 ?? 0);
                                    cmbGST.SelectedValue = invCtrl.GetGSTRule().ToString();
                                }
                                if (s.Userfld2 != null && AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowCurrency), false))
                                {
                                    PowerPOS.Currency cr = new PowerPOS.Currency("CurrencyCode", s.Userfld2);
                                    if (cr != null && cr.CurrencyCode != "")
                                        cmbCurrencies.SelectedIndex = cmbCurrencies.FindString(cr.CurrencyCode);
                                }
                            }
                        }
                        
                        if (status != "")
                        {
                            MessageBox.Show(status);
                        }
                        DataTable dt = inv.InvDetToDataTable();
                        status = "";


                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (dt.Rows[i]["Userflag1"].ToString().ToLower() == "true" || dt.Rows[i]["Userflag1"].ToString().ToLower() == "yes" || dt.Rows[i]["Userflag1"].ToString().ToLower() == "1")
                                continue;

                            decimal packingCost = 0;
                            decimal.TryParse(dt.Rows[i]["Userfloat3"].ToString(), out packingCost);
                            decimal packingQuantity = 0;
                            decimal.TryParse(dt.Rows[i]["Userfloat5"].ToString(), out packingQuantity);
                            decimal totalCost = packingQuantity * packingCost;
                            if (!invCtrl.AddItemIntoInventoryWithRetailPriceForPO(dt.Rows[i]["ItemNo"].ToString(), Convert.ToDecimal(dt.Rows[i]["Quantity"].ToString()), Convert.ToDecimal(dt.Rows[i]["FactoryPrice"].ToString()), (dt.Rows[i]["userfloat2"] + "").GetDecimalValue(), dt.Rows[i]["PurchaseOrderDetRefNo"].ToString(), totalCost, out status))
                            {
                                MessageBox.Show("Error: " + status);
                            }
                        }
                        isLoaded = true;

                        BindGrid();
                        txtGSTAmount.Text = invCtrl.InvHdr.Tax.GetValueOrDefault(0).ToString("N");
                    }
                    else
                    {
                        MessageBox.Show("Purchase Order is Unapproved. Please approve this purchase order before proceed");
                    }
                }
                /*else
                {
                    MessageBox.Show("Purchase Order No " + txtPurchaseOrder.Text + " is not valid.");
                    txtPurchaseOrder.Text = "";
                    txtPurchaseOrder.Focus();
                }*/


            }

        }

        private void AddAdditionalInformation()
        {
            try
            {
                // Init WebService
                SyncClientController.Load_WS_URL();
                PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                ws.Url = SyncClientController.WS_URL;

                //Add Additional Information//
                Label lb;

                lb = CreateInventoryLabel();
                lb.Text = LanguageManager.Purchase_Order;
                txtPurchaseOrder = CreateInventoryTextBox();
                txtPurchaseOrder.Name = "txtPurchaseOrder";
                tblInventory.Controls.Add(lb, 0, 6);
                tblInventory.Controls.Add(txtPurchaseOrder, 1, 6);
                txtPurchaseOrder.Leave += new EventHandler(this.txtPurchaseOrder_Leave);

                //lb = CreateInventoryLabel();
                //lb.Text = "Supplier";
                //txtSupplier = CreateInventoryTextBox();
                //txtSupplier.Name = "txtSupplier";
                //tblInventory.Controls.Add(lb, 0, 7);
                //tblInventory.Controls.Add(txtSupplier, 1, 7);

                lb = CreateInventoryLabel();
                lb.Text = LanguageManager.Supplier;
                cmbSupplier = CreateInventoryComboBox();
                cmbSupplier.DropDownStyle = ComboBoxStyle.DropDownList;
                cmbSupplier.Name = "cmbSupplier";
                cmbSupplier.SelectedIndexChanged += new EventHandler(this.cmbSupplier_SelectedIndexChanged);
                try
                {
                    DataSet dsSupplier = ws.GetDataTable("Supplier", true);
                    var dtSupplier = new DataTable();
                    if (dsSupplier.Tables.Count > 0)
                    {
                        dtSupplier = dsSupplier.Tables[0];
                    }
                    dtSupplier.DeleteRow("Deleted", 1);
                    DataView dv = dtSupplier.DefaultView;
                    dv.Sort = "SupplierName ASC";
                    dtSupplier = dv.ToTable();

                    DataRow drSupplier = dtSupplier.NewRow();
                    drSupplier["SupplierID"] = -1;
                    drSupplier["SupplierName"] = "--Select Supplier--";
                    dtSupplier.Rows.InsertAt(drSupplier, 0);
                    dtSupplier.AcceptChanges();
                    cmbSupplier.DataSource = dtSupplier;
                    cmbSupplier.DisplayMember = "SupplierName";
                    cmbSupplier.ValueMember = "SupplierID";
                }
                catch (Exception ex)
                {
                    Logger.writeLog(ex);
                }

                tblInventory.Controls.Add(lb, 0, 7);
                tblInventory.Controls.Add(cmbSupplier, 1, 7);

                lb = CreateInventoryLabel();
                lb.Text = LanguageManager.Freight_Charges;
                txtFreightCharges = CreateInventoryTextBox();
                txtFreightCharges.Name = "txtFreight";
                txtFreightCharges.Text = "0";
                tblInventory.Controls.Add(lb, 3, 6);
                tblInventory.Controls.Add(txtFreightCharges, 4, 6);
                txtFreightCharges.Leave += new EventHandler(Cost_Changed);


                lb = CreateInventoryLabel();
                lb.Text = LanguageManager.Discount;
                txtDiscount = CreateInventoryTextBox();
                txtDiscount.Name = "txtDiscount";
                txtDiscount.Text = "0";
                tblInventory.Controls.Add(lb, 3, 7);
                tblInventory.Controls.Add(txtDiscount, 4, 7);
                txtDiscount.Leave += new EventHandler(Cost_Changed);

                lb = CreateInventoryLabel();
                lb.Text = LanguageManager.Exchange_Rate;
                txtExchangeRate = CreateInventoryTextBox();
                txtExchangeRate.Name = "txtExchangeRate";
                txtExchangeRate.Text = "1";
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.ShowCurrency), false))
                {
                    tblInventory.Controls.Add(lb, 3, 8);
                    tblInventory.Controls.Add(txtExchangeRate, 4, 8);
                }
                txtExchangeRate.Leave += new EventHandler(Cost_Changed);

                lb = CreateInventoryLabel();
                lb.Text = LanguageManager.Currencies;
                cmbCurrencies = CreateInventoryComboBox();
                cmbCurrencies.DropDownStyle = ComboBoxStyle.DropDownList;
                cmbCurrencies.Name = "cmbCurrencies";
                cmbCurrencies.SelectedIndexChanged += new EventHandler(this.cmbCurrencies_SelectedIndexChanged);

                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.ShowCurrency), false))
                {
                    //populate the combo box...
                    CurrencyCollection col = new CurrencyCollection();
                    col.Where(Currency.Columns.Deleted, false);
                    col.Load();
                    Currency t = new Currency();
                    t.CurrencyCode = LanguageManager.Select_Currency;
                    t.CurrencyId = -1;
                    col.Insert(0, t);
                    cmbCurrencies.DataSource = col;
                    tblInventory.Controls.Add(lb, 3, 9);
                    tblInventory.Controls.Add(cmbCurrencies, 4, 9);

                    tblInventory.Refresh();
                }


                lb = CreateInventoryLabel();
                lb.Text = LanguageManager.Vendor_Invoice_No;
                txtVendorInvoiceNo = CreateInventoryTextBox();
                txtVendorInvoiceNo.Name = "txtVendorInvoiceNo";
                txtVendorInvoiceNo.Text = "";
                tblInventory.Controls.Add(lb, 0, 8);
                tblInventory.Controls.Add(txtVendorInvoiceNo, 1, 8);

                string gstText = "GST";
                if (!string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.Interface.ReplaceGSTTextWith)))
                    gstText = AppSetting.GetSetting(AppSetting.SettingsName.Interface.ReplaceGSTTextWith);

                lb = CreateInventoryLabel();
                lb.Text = gstText;
                cmbGST = CreateInventoryComboBox();
                cmbGST.DropDownStyle = ComboBoxStyle.DropDownList;
                cmbGST.Name = "cmbGST";
                cmbGST.SelectedIndexChanged += new EventHandler(this.cmbGST_SelectedIndexChanged);


                DataTable dtGST = new DataTable();
                dtGST.Columns.Add("ID", typeof(int));
                dtGST.Columns.Add("GSTType");
                DataRow dr = dtGST.NewRow();
                dr["ID"] = 1;
                dr["GSTType"] = "Exclusive";
                dtGST.Rows.Add(dr);

                dr = dtGST.NewRow();
                dr["ID"] = 2;
                dr["GSTType"] = "Inclusive";
                dtGST.Rows.Add(dr);

                dr = dtGST.NewRow();
                dr["ID"] = 0;
                dr["GSTType"] = "Non " + gstText;
                dtGST.Rows.Add(dr);

                cmbGST.DataSource = dtGST;
                cmbGST.DisplayMember = "GSTType";
                cmbGST.ValueMember = "ID";
                //cmbGST.Text = "Non GST";
                cmbGST.Refresh();

                

                tblInventory.Controls.Add(lb, 0, 9);
                tblInventory.Controls.Add(cmbGST, 1, 9);


                lb = CreateInventoryLabel();
                lb.Text = gstText + " Amount";
                txtGSTAmount = CreateInventoryLabel();
                txtGSTAmount.Name = "txtGSTAmount";
                txtGSTAmount.Text = "";
                tblInventory.Controls.Add(lb, 0, 10);
                tblInventory.Controls.Add(txtGSTAmount, 1, 10);


                #region *) Add custom fields based on AppSetting
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.ShowCustomField1), false))
                {
                    lb = CreateInventoryLabel();
                    lb.Text = AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.CustomField1Label);
                    txtCustomField1 = CreateInventoryTextBox();
                    txtCustomField1.Name = "txtCustomField1";
                    txtCustomField1.Tag = lb.Text;
                    tblInventory.Controls.Add(lb, 0, 11);
                    tblInventory.Controls.Add(txtCustomField1, 1, 11);
                }

                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.ShowCustomField2), false))
                {
                    lb = CreateInventoryLabel();
                    lb.Text = AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.CustomField2Label);
                    txtCustomField2 = CreateInventoryTextBox();
                    txtCustomField2.Name = "txtCustomField2";
                    txtCustomField2.Tag = lb.Text;
                    tblInventory.Controls.Add(lb, 3, 11);
                    tblInventory.Controls.Add(txtCustomField2, 4, 11);
                }

                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.ShowCustomField3), false))
                {
                    lb = CreateInventoryLabel();
                    lb.Text = AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.CustomField3Label);
                    txtCustomField3 = CreateInventoryTextBox();
                    txtCustomField3.Name = "txtCustomField3";
                    txtCustomField3.Tag = lb.Text;
                    tblInventory.Controls.Add(lb, 0, 12);
                    tblInventory.Controls.Add(txtCustomField3, 1, 12);
                }

                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.ShowCustomField4), false))
                {
                    lb = CreateInventoryLabel();
                    lb.Text = AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.CustomField4Label);
                    txtCustomField4 = CreateInventoryTextBox();
                    txtCustomField4.Name = "txtCustomField4";
                    txtCustomField4.Tag = lb.Text;
                    tblInventory.Controls.Add(lb, 3, 12);
                    tblInventory.Controls.Add(txtCustomField4, 4, 12);
                }

                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.ShowCustomField5), false))
                {
                    lb = CreateInventoryLabel();
                    lb.Text = AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.CustomField5Label);
                    txtCustomField5 = CreateInventoryTextBox();
                    txtCustomField5.Name = "txtCustomField5";
                    txtCustomField5.Tag = lb.Text;
                    tblInventory.Controls.Add(lb, 0, 13);
                    tblInventory.Controls.Add(txtCustomField5, 1, 13);
                }

                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.ShowAdditionalCost1), false))
                {
                    lb = CreateInventoryLabel();
                    lb.Text = AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.AdditionalCost1Label);
                    txtAdditionalCost1 = CreateInventoryTextBox();
                    txtAdditionalCost1.Name = "txtAdditionalCost1";
                    txtAdditionalCost1.Text = "0";
                    txtAdditionalCost1.Tag = lb.Text;
                    tblInventory.Controls.Add(lb, 3, 13);
                    tblInventory.Controls.Add(txtAdditionalCost1, 4, 13);
                    txtAdditionalCost1.Leave += new EventHandler(Cost_Changed);
                }

                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.ShowAdditionalCost2), false))
                {
                    lb = CreateInventoryLabel();
                    lb.Text = AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.AdditionalCost2Label);
                    txtAdditionalCost2 = CreateInventoryTextBox();
                    txtAdditionalCost2.Name = "txtAdditionalCost2";
                    txtAdditionalCost2.Text = "0";
                    txtAdditionalCost2.Tag = lb.Text;
                    tblInventory.Controls.Add(lb, 0, 14);
                    tblInventory.Controls.Add(txtAdditionalCost2, 1, 14);
                    txtAdditionalCost2.Leave += new EventHandler(Cost_Changed);
                }

                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.ShowAdditionalCost3), false))
                {
                    lb = CreateInventoryLabel();
                    lb.Text = AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.AdditionalCost3Label);
                    txtAdditionalCost3 = CreateInventoryTextBox();
                    txtAdditionalCost3.Name = "txtAdditionalCost3";
                    txtAdditionalCost3.Text = "0";
                    txtAdditionalCost3.Tag = lb.Text;
                    tblInventory.Controls.Add(lb, 3, 14);
                    tblInventory.Controls.Add(txtAdditionalCost3, 4, 14);
                    txtAdditionalCost3.Leave += new EventHandler(Cost_Changed);
                }

                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.ShowAdditionalCost4), false))
                {
                    lb = CreateInventoryLabel();
                    lb.Text = AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.AdditionalCost4Label);
                    txtAdditionalCost4 = CreateInventoryTextBox();
                    txtAdditionalCost4.Name = "txtAdditionalCost4";
                    txtAdditionalCost4.Text = "0";
                    txtAdditionalCost4.Tag = lb.Text;
                    tblInventory.Controls.Add(lb, 0, 15);
                    tblInventory.Controls.Add(txtAdditionalCost4, 1, 15);
                    txtAdditionalCost4.Leave += new EventHandler(Cost_Changed);
                }

                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.ShowAdditionalCost5), false))
                {
                    lb = CreateInventoryLabel();
                    lb.Text = AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.AdditionalCost5Label);
                    txtAdditionalCost5 = CreateInventoryTextBox();
                    txtAdditionalCost5.Name = "txtAdditionalCost5";
                    txtAdditionalCost5.Text = "0";
                    txtAdditionalCost5.Tag = lb.Text;
                    tblInventory.Controls.Add(lb, 3, 15);
                    tblInventory.Controls.Add(txtAdditionalCost5, 4, 15);
                    txtAdditionalCost5.Leave += new EventHandler(Cost_Changed);
                }
                #endregion

                //make total cost price column visible
                dgvStock.Columns["TotalCostPrice"].Visible = showCostPrice;
                dgvStock.Columns["RetailPrice"].Visible = showRetailPrice;

            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show(LanguageManager.Error_encounter_ + ex.Message);
            }
        }

        private void cmbCurrencies_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbCurrencies.SelectedIndex != 0)
                {
                    /*DialogResult dr = MessageBox.Show
                        (LanguageManager.WARNING__You_will_lose_all_your_modified_cost_priced__Are_you_sure_you_want_to_change_your_Currency_, "", MessageBoxButtons.YesNo);
                    if (dr == DialogResult.Yes)
                    {*/
                    //check if got currency conversion
                    if (((Currency)cmbCurrencies.SelectedValue).CurrencyCode != "SGD")
                    {
                        ExchangeRateController ctrl = new ExchangeRateController();
                        ctrl.Load("ExchangeRate");
                        decimal rate;
                        Hashtable ht = ctrl.GetHashTable();
                        bool isFound = false;
                        if (ht != null)
                        {
                            object obj = ht[((Currency)cmbCurrencies.SelectedValue).CurrencyCode];
                            if (obj != null)
                            {
                                isFound = true;
                                decimal.TryParse(obj.ToString(), out rate);
                                txtExchangeRate.Text = rate.ToString("N");
                                invCtrl.UpdateCurrency(((Currency)cmbCurrencies.SelectedValue).CurrencyId);
                                BindGrid();
                                txtGSTAmount.Text = invCtrl.InvHdr.Tax.GetValueOrDefault(0).ToString("N");
                            }

                        }

                        if (!isFound)
                        {
                            MessageBox.Show("The currency conversion from " + ((Currency)cmbCurrencies.SelectedValue).CurrencyCode + " to SGD is not setup yet. " +
                                "\n Pls setup the currency conversion in the POS Setup -> Setup Currency");
                            return;
                        }
                    }
                    else
                    {

                        invCtrl.UpdateCurrency(((Currency)cmbCurrencies.SelectedValue).CurrencyId);
                        BindGrid();
                        txtGSTAmount.Text = invCtrl.InvHdr.Tax.GetValueOrDefault(0).ToString("N");
                    }
                    /*}
                    else
                    {

                        //roll back!
                        //get the previous currency id
                        //roll back to the position by looping... stupid way
                        int CurrencyID = invCtrl.getCurrencyID();
                        cmbCurrencies.SelectedIndex = 0;
                        for (int i = 0; i < cmbCurrencies.Items.Count; i++)
                        {
                            if (((Currency)cmbCurrencies.Items[i]).CurrencyId == CurrencyID)
                            {
                                cmbCurrencies.SelectedIndex = i;
                                return;
                            }
                        }
                    }*/
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show(LanguageManager.Error_encounter_ + ex.Message);
            }
        }

        private void cmbSupplier_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbSupplier.SelectedIndex != 0)
                {
                    invCtrl.SetSupplier(cmbSupplier.SelectedValue.ToString());

                    int tmp = 0;
                    if (int.TryParse(cmbSupplier.SelectedValue.ToString(), out tmp))
                    {
                        Supplier s = new Supplier(tmp);
                        if (s != null && s.SupplierID > -1)
                        {
                            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.GoodsReceiveGSTRuleFromSupplier), false))
                            {
                                invCtrl.SetGSTRule(s.Userint1 ?? 0);
                                cmbGST.SelectedValue = invCtrl.GetGSTRule().ToString();
                            }
                            if (s.Userfld2 != null && AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowCurrency), false))
                            {
                                PowerPOS.Currency cr = new PowerPOS.Currency("CurrencyCode", s.Userfld2);
                                if (cr != null && cr.CurrencyCode != "")
                                    cmbCurrencies.SelectedIndex = cmbCurrencies.FindString(cr.CurrencyCode);
                            }
                        }
                    }
                    BindGrid();
                    txtGSTAmount.Text = invCtrl.InvHdr.Tax.GetValueOrDefault(0).ToString("N");
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show(LanguageManager.Error_encounter_ + ex.Message);
            }
        }

        private void cmbGST_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                invCtrl.SetGSTRule(cmbGST.SelectedValue.ToString().GetIntValue());
                BindGrid();
                txtGSTAmount.Text = invCtrl.InvHdr.Tax.GetValueOrDefault(0).ToString("N");

            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show(LanguageManager.Error_encounter_ + ex.Message);
            }
        }

        private void Cost_Changed(object sender, EventArgs e)
        {
            SetAdditionalCost();
            BindGrid();
            txtGSTAmount.Text = invCtrl.InvHdr.Tax.GetValueOrDefault(0).ToString("N");
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsLineItemEmpty())
                {
                    MessageBox.Show(LanguageManager.Please_tick_the_item_you_wish_to_process);
                    tbControl.SelectedIndex = 1;
                    return;
                }
                if (cmbLocation.Items.Count > 1 && cmbLocation.SelectedIndex == 0)
                {
                    MessageBox.Show(LanguageManager.Please_select_the_location_);
                    tbControl.SelectedIndex = 0;
                    cmbLocation.Focus();
                    return;
                }
                if (cmbCurrencies.Items.Count > 1 && cmbCurrencies.SelectedIndex == 0)
                {
                    MessageBox.Show("Please select the Currency.");
                    tbControl.SelectedIndex = 0;
                    cmbCurrencies.Focus();
                    return;
                }

                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.VendorInvoiceNoCompulsory), false) && string.IsNullOrEmpty(txtVendorInvoiceNo.Text))
                {
                    MessageBox.Show("Please fill Vendor Invoice No.");
                    tbControl.SelectedIndex = 0;
                    txtVendorInvoiceNo.Focus();
                    return;
                }
                
                string PurchaseOrderHdrRefNo = "";
                string status = "";
                if (txtPurchaseOrder.Text.Length > 0)
                {
                    PurchaseOdrController inv = new PurchaseOdrController();
                    bool result = inv.LoadPurchaseOrder(txtPurchaseOrder.Text, out status);
                    if (result)
                    {
                        PurchaseOrderHdrRefNo = inv.GetPurchaseOrderHdrRefNo();
                        if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.AllowLoadUnapprovedPOInGR), false) || inv.isApproved())
                        {
                        }
                        else
                        {
                            MessageBox.Show("Purchase Order is Unapproved. Please approve this purchase order before proceed");
                            return;
                        }
                    }
                    else
                    {
                        PurchaseOrderHdrRefNo = txtPurchaseOrder.Text;
                    }
                }

                //string status;
                decimal freightCharges, discount;
                double exchangeRate;

                if (txtExchangeRate.Text == "") txtExchangeRate.Text = "0";
                if (txtFreightCharges.Text == "") txtFreightCharges.Text = "0";
                if (txtDiscount.Text == "") txtDiscount.Text = "0";

                if (!decimal.TryParse(txtFreightCharges.Text, out freightCharges))
                {
                    MessageBox.Show(LanguageManager.Invalid_Freight_Charges);
                    tbControl.SelectedIndex = 0;
                    txtFreightCharges.Focus();
                    return;
                }
                if (!decimal.TryParse(txtDiscount.Text, out discount))
                {
                    MessageBox.Show(LanguageManager.Invalid_Discount_);
                    tbControl.SelectedIndex = 0;
                    txtDiscount.Focus();
                    return;
                }

                if (!double.TryParse(txtExchangeRate.Text, out exchangeRate))
                {
                    MessageBox.Show(LanguageManager.Invalid_Exchange_Rate);
                    tbControl.SelectedIndex = 0;
                    txtExchangeRate.Focus();
                    return;
                }

                var listInvDet = new List<string>();
                for (int i = 0; i < dgvStock.Rows.Count; i++)
                {
                    if ((dgvStock.Rows[i].Cells[0].Value + "").IsEqual("true"))
                        listInvDet.Add(dgvStock.Rows[i].Cells["InventoryDetRefNo"].Value + "");
                }

                string msgSerialNo = "";
                if (!invCtrl.IsSerialNoValid(listInvDet, out msgSerialNo))
                {
                    MessageBox.Show(msgSerialNo);
                    return;
                }

                if (!CommonUILib.ShowAreYouSure()) return;

                if (!PointOfSaleInfo.IntegrateWithInventory)
                {
                    ShowPanelPleaseWait();
                    SyncClientController.Load_WS_URL();
                    PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                    ws.Timeout = 100000;
                    ws.Url = SyncClientController.WS_URL;
                    if (ws.IsThereUnAdjustedStockTake(((InventoryLocation)cmbLocation.SelectedValue).InventoryLocationID) && Text.ToUpper() != "FRMSTOCKTAKE")
                    {
                        isTherePendingStockTake = true;
                        MessageBox.Show(
                            "There is an unadjusted stock take for this location. No inventory movement is allowed! Please adjust stock take first");
                        pnlLoading.Visible = false;
                        return;
                    }
                    pnlLoading.Visible = false;
                }
                else
                {
                    if (StockTakeController.IsThereUnAdjustedStockTake(((InventoryLocation)cmbLocation.SelectedValue).InventoryLocationID) && Text.ToUpper() != "FRMSTOCKTAKE")
                    {
                        isTherePendingStockTake = true;
                        MessageBox.Show(
                            "There is an unadjusted stock take for this location. No inventory movement is allowed! Please adjust stock take first");
                        return;
                    }
                }

                //Remove unticked to temporary storage////
                InventoryDetCollection tmpDetCol;
                RemoveUnticked(out status, out tmpDetCol);

                string supplier = "";
                if (cmbSupplier.SelectedValue != null && cmbSupplier.SelectedValue.ToString() != "-1")
                    supplier = cmbSupplier.SelectedValue.ToString();

                invCtrl.SetInventoryHeaderInfo
                    (PurchaseOrderHdrRefNo, supplier, txtRemark.Text,
                    freightCharges, exchangeRate, discount);

                invCtrl.SetVendorInvoiceNo(txtVendorInvoiceNo.Text);

                bool isSuccess = false;
                ShowPanelPleaseWait();

                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.UpdateFactoryPriceOnStockIn), false))
                    UpdateFactoryPrice();

                string newInventoryHdrRefNo = "";
                string newCustomRefNo = "";
                if (!PointOfSaleInfo.IntegrateWithInventory)
                {
                    SyncClientController.Load_WS_URL();
                    PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                    ws.Timeout = 100000;
                    ws.Url = SyncClientController.WS_URL;
                    DataSet myDataSet = new DataSet();
                    myDataSet.Tables.Add(invCtrl.InvHdrToDataTable());
                    myDataSet.Tables.Add(invCtrl.InvDetToDataTable());
                    byte[] data = SyncClientController.CompressDataSetToByteArray(myDataSet);

                    if (ws.StockInCompressed
                        (data,
                        UserInfo.username,
                        ((InventoryLocation)(cmbLocation.SelectedValue)).InventoryLocationID,
                        false, true, out newInventoryHdrRefNo, out newCustomRefNo, out status))
                    {
                        if (useCustomRefNo)
                        {
                            if (generateNoInServer)
                            {
                                // Get the latest number from server (note: update has already performed in server)
                                GetCustomGRNoSettingFromServer();
                            }
                            else
                            {
                                InventoryController.CustomRefNoUpdate();
                            }
                        }

                        //download inventoryhdr and inventorydet                        
                        //if (SyncClientController.GetCurrentInventoryRealTime())
                        //{
                        isSuccess = true;
                        pnlLoading.Visible = false;
                        invCtrl.SetInventoryHdrRefNo(newInventoryHdrRefNo);
                        invCtrl.SetCustomRefNo(newCustomRefNo);
                        invCtrl.SetIsNew(false);
                        MessageBox.Show(LanguageManager.Goods_receive_successful);
                        //}
                        //else
                        //{
                        //    Logger.writeLog("Unable to download data from server: " + status);
                        //    isSuccess = false;
                        //    this.Close();
                        //    return;
                        //}
                    }
                    else
                    {
                        isSuccess = false;
                        pnlLoading.Visible = false;
                        MessageBox.Show(LanguageManager.Some_Error_Encountered_Contact_Admin + status);
                        return;
                    }
                }
                else
                {
                    if (invCtrl.StockIn(UserInfo.username,
                        ((InventoryLocation)(cmbLocation.SelectedValue)).InventoryLocationID,
                        false, true, out status))
                    {
                        //InventoryController.AssignStockOutToConfirmedOrderUsingTransactionScope();
                        pnlLoading.Visible = false;
                        newInventoryHdrRefNo = invCtrl.GetInvHdrRefNo();
                        newCustomRefNo = invCtrl.GetCustomRefNo();
                        isSuccess = true;
                        MessageBox.Show(LanguageManager.Goods_receive_successful);
                    }
                    else
                    {
                        pnlLoading.Visible = false;
                        isSuccess = false;
                        MessageBox.Show(LanguageManager.Error_encounter_ + status);
                    }
                }
                if (isSuccess)
                {
                    AccessLogController.AddLog(AccessSource.POS, UserInfo.username, "-", string.Format("Good Receive : {0}", invCtrl.GetInvHdrRefNo()), "");

                    if (printAfterConfirm)
                    {
                        //remark by Adi 20150910. remark will be used 
                        //invCtrl = new InventoryController(PowerPOS.Container.InventorySettings.CostingMethod, newInventoryHdrRefNo); //reload...
                        btnPrint_Click(this, new EventArgs());
                    }

                    //clear control.....                
                    invCtrl = new InventoryController(PowerPOS.Container.InventorySettings.CostingMethod);
                    if (tmpDetCol.Count > 0)
                    {
                        invCtrl.SetInventoryLocation(((InventoryLocation)(cmbLocation.SelectedValue)).InventoryLocationID);
                        invCtrl.AddInvDet(tmpDetCol);
                        SaveToDisk(true);
                    }
                    else
                    {
                        ClearControls();
                        ClearAdditionalInformation();
                        SetAdditionalCost();

                        //cmbLocation.SelectedIndex = defaultLoc;
                        invCtrl.SetInventoryLocation(((InventoryLocation)(cmbLocation.SelectedValue)).InventoryLocationID);
                    }
                    BindGrid();
                    txtGSTAmount.Text = invCtrl.InvHdr.Tax.GetValueOrDefault(0).ToString("N");
                    tbControl.SelectedIndex = 0;

                    pnlLoading.Visible = false;
                }
            }
            catch (Exception ex)
            {
                pnlLoading.Visible = false;
                MessageBox.Show(LanguageManager.Error_encountered_while_trying_to_perform_stock_in__Error + ex.Message);
                SaveToDisk(true);
                Logger.writeLog(ex);
            }
        }

        private void UpdateFactoryPrice()
        {
            var itemList = invCtrl.InvDetToDataTable();
            foreach (DataRow item in itemList.Rows)
            {
                try
                {
                    var costOfGoods = item[InventoryDet.UserColumns.InitialFactoryPrice];

                    if ((Convert.ToDecimal(costOfGoods) == 0) && AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.DontUpdateFactoryPriceIfZero), false))
                    {
                        continue;
                    }

                    string supplier = invCtrl.getSupplier() ?? "";
                    string userName = UserInfo.username;
                    string itemNo = (string)item["ItemNo"];

                    SyncClientController.Load_WS_URL();
                    string wsURL = SyncClientController.WS_URL;
                    bool isLocalhost = wsURL.StartsWith("http://localhost");
                    if (!isLocalhost)
                    {
                        using (WebClient wc = new WebClient())
                        {
                            System.Collections.Specialized.NameValueCollection reqparm = new System.Collections.Specialized.NameValueCollection();
                            reqparm.Add("ItemNo", itemNo);
                            reqparm.Add("Username", userName);
                            reqparm.Add("FactoryPrice", costOfGoods.ToString());
                            reqparm.Add("Supplier", supplier);
                            SyncClientController.Load_WS_URL();
                            string url = SyncClientController.WS_URL.Replace("synchronization/synchronization.asmx", "") + "API/Product/Item/UpdateFactoryPrice.ashx";
                            byte[] responsebytes = wc.UploadValues(url, "POST", reqparm);
                            string responsebody = Encoding.UTF8.GetString(responsebytes);

                            //Item currentItem = new Item(itemNo);
                            //currentItem.FactoryPrice = Convert.ToDecimal(costOfGoods);
                            //currentItem.Save(userName);
                        }
                    }
                    else
                    {
                        //Item currentItem = new Item(itemNo);
                        //currentItem.FactoryPrice = Convert.ToDecimal(costOfGoods);
                        //currentItem.Save(userName);

                        QueryCommandCollection cmdColl = new QueryCommandCollection();
                        string strSql = @"update Item 
                                             set FactoryPrice = " + costOfGoods.ToString() + @"
                                               , ModifiedOn = getdate()
                                               , ModifiedBy = '" + userName + @"'
                                           where ItemNo = '" + itemNo + @"'
                                        ";
                        cmdColl.Add(new QueryCommand(strSql, "PowerPOS"));

                        if (!string.IsNullOrEmpty(supplier))
                        {
                            strSql = @"
                                        update ItemSupplierMap
                                        set CostPrice = " + costOfGoods.ToString() + @"
                                          , ModifiedOn = getdate()
                                          , ModifiedBy = '" + userName + @"'
                                        where ItemNo = '" + itemNo + @"'
                                          and SupplierID = " + supplier + @"
                                      ";
                            cmdColl.Add(new QueryCommand(strSql, "PowerPOS"));
                        }

                        DataService.ExecuteTransaction(cmdColl);
                    }
                }
                catch (Exception ex)
                {
                    Logger.writeLog(ex);
                }
            }
        }

        public override void btnScanItemNo_Click(object sender, EventArgs e)
        {
            try
            {
                #region *) OBSOLETE : no need to check pending stock take when adding item.
                //if (!PointOfSaleInfo.IntegrateWithInventory)
                //{
                //    ShowPanelPleaseWait();
                //    SyncClientController.Load_WS_URL();
                //    PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                //    ws.Timeout = 100000;
                //    ws.Url = SyncClientController.WS_URL;
                //    if (ws.IsThereUnAdjustedStockTake(((InventoryLocation)cmbLocation.SelectedValue).InventoryLocationID) && Text.ToUpper() != "FRMSTOCKTAKE")
                //    {
                //        isTherePendingStockTake = true;
                //        MessageBox.Show(
                //            "There is an unadjusted stock take for this location. No inventory movement is allowed! Please adjust stock take first");
                //        pnlLoading.Visible = false;
                //        return;

                //    }
                //    pnlLoading.Visible = false;
                //}
                //else
                //{
                //    if (StockTakeController.IsThereUnAdjustedStockTake(((InventoryLocation)cmbLocation.SelectedValue).InventoryLocationID) && Text.ToUpper() != "FRMSTOCKTAKE")
                //    {
                //        isTherePendingStockTake = true;
                //        MessageBox.Show(
                //            "There is an unadjusted stock take for this location. No inventory movement is allowed! Please adjust stock take first");
                //        return;
                //    }
                //}
                #endregion

                string correctItemNo;
                ArrayList correctItemNoList = null;
                ArrayList correctItemNames = null;
                ArrayList correctDescriptions = null;
                ArrayList correctUserflag1s = null;
                ArrayList correctAttributes1 = null;

                bool isshowmatrixform = false;
                bool ismatrixattributes1 = false;
                bool isItemGroup = false;
                string status;
                int defaultQty = Int32.Parse(string.IsNullOrEmpty(txtDefaultQty.Text) ? "0" : txtDefaultQty.Text);

                if (cbDefaultQty.Checked && defaultQty == 0)
                {
                    MessageBox.Show(LanguageManager.Warning + " Please fill default quantitiy.");
                }
                else
                {
                    if (ItemController.IsPromoBarcode(txtItemNoBarcode.Text.Trim()))
                    {
                        int multiplier = defaultQty;
                        int PromoCampaignHdrId = 0;

                        PromoCampaignHdrCollection hdr = new PromoCampaignHdrCollection();
                        hdr.Where(PromoCampaignHdr.Columns.Barcode, Comparison.Equals, txtItemNoBarcode.Text.Trim());
                        hdr.Where(PromoCampaignHdr.Columns.Deleted, Comparison.Equals, false);
                        hdr.Load();

                        if (hdr.Count > 0)
                            PromoCampaignHdrId = hdr[0].PromoCampaignHdrID;

                        if (!cbDefaultQty.Checked)
                        {
                            ShowPanelPleaseWait();
                            frmAddPromoItem myaddPromo = new frmAddPromoItem();
                            myaddPromo.Barcode = txtItemNoBarcode.Text.Trim();
                            CommonUILib.displayTransparent();
                            myaddPromo.ShowDialog();
                            CommonUILib.hideTransparent();

                            multiplier = myaddPromo.Qty;
                        }

                        pnlLoading.Visible = false;
                        if (multiplier > 0)
                        {
                            PromoCampaignDetCollection det = new PromoCampaignDetCollection();
                            det.Where(PromoCampaignDet.Columns.PromoCampaignHdrID, Comparison.Equals, PromoCampaignHdrId);
                            det.Where(PromoCampaignDet.Columns.Deleted, Comparison.Equals, false);
                            det.Load();

                            if (det.Count > 0)
                            {
                                for (int z = 0; z < det.Count; z++)
                                {
                                    if ((det[z].ItemNo != null && det[z].ItemNo.ToString() != "") && det[z].UnitQty > 0)
                                    {
                                        if (!invCtrl.AddItemIntoInventoryStockIn(det[z].ItemNo.ToString(), multiplier * (int)det[z].UnitQty, out status))
                                        {
                                            MessageBox.Show("Error: " + status);
                                        }
                                    }
                                }
                                BindGrid();
                                txtGSTAmount.Text = invCtrl.InvHdr.Tax.GetValueOrDefault(0).ToString("N");
                                txtItemNoBarcode.Text = "";
                                txtItemNoBarcode.Focus();
                            }

                        }

                    }
                    else
                    {
                        if (ItemController.IsInventoryItem(txtItemNoBarcode.Text.Trim()))
                        {
                            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.SupplierIsCompulsory), false))
                            {
                                if (ItemController.IsSupplierItem(txtItemNoBarcode.Text.Trim(), cmbSupplier.Text))
                                {
                                    //by ITEM NUMBER
                                    correctItemNoList = new ArrayList();
                                    correctItemNames = new ArrayList();
                                    correctDescriptions = new ArrayList();
                                    correctUserflag1s = new ArrayList();
                                    correctAttributes1 = new ArrayList();

                                    Item it = new Item(txtItemNoBarcode.Text);
                                    correctItemNoList.Add(txtItemNoBarcode.Text.Trim());
                                    correctItemNames.Add(it.ItemName);
                                    correctDescriptions.Add(it.ItemDesc);
                                    correctUserflag1s.Add(it.Userflag1 ?? false);
                                    correctAttributes1.Add(it.Attributes1);
                                    //correctUnitQty.Add(1);
                                }
                                else {
                                    //by ITEM NUMBER
                                    correctItemNoList = new ArrayList();
                                    correctItemNames = new ArrayList();
                                    correctDescriptions = new ArrayList();
                                    correctUserflag1s = new ArrayList();
                                    correctAttributes1 = new ArrayList();
                                }
                            }
                            else
                            {
                                //by ITEM NUMBER
                                correctItemNoList = new ArrayList();
                                correctItemNames = new ArrayList();
                                correctDescriptions = new ArrayList();
                                correctUserflag1s = new ArrayList();
                                correctAttributes1 = new ArrayList();

                                Item it = new Item(txtItemNoBarcode.Text);
                                correctItemNoList.Add(txtItemNoBarcode.Text.Trim());
                                correctItemNames.Add(it.ItemName);
                                correctDescriptions.Add(it.ItemDesc);
                                correctUserflag1s.Add(it.Userflag1 ?? false);
                                correctAttributes1.Add(it.Attributes1);
                                // correctUnitQty.Add(1);
                            }

                        }
                        else
                        {
                            if (ItemController.IsInventoryItemBarcode(txtItemNoBarcode.Text.Trim(), out correctItemNo))
                            {
                                correctItemNoList = new ArrayList();
                                correctItemNames = new ArrayList();
                                correctDescriptions = new ArrayList();
                                correctUserflag1s = new ArrayList();
                                correctAttributes1 = new ArrayList();

                                Item it = new Item(correctItemNo);
                                correctItemNoList.Add(correctItemNo);
                                correctItemNames.Add(it.ItemName);
                                correctDescriptions.Add(it.ItemDesc);
                                correctUserflag1s.Add(it.Userflag1 ?? false);
                                correctAttributes1.Add(it.Attributes1);
                            }
                            else
                            {
                                if (ItemController.IsMatrixAttributes1(txtItemNoBarcode.Text.Trim()))
                                {
                                    correctAttributes1 = new ArrayList();

                                    correctAttributes1.Add(txtItemNoBarcode.Text.Trim());

                                    ismatrixattributes1 = true;
                                    isshowmatrixform = true;
                                }
                                else
                                {
                                    if (ItemController.IsItemGroup(txtItemNoBarcode.Text.Trim()))
                                    {
                                        ItemGroup it = new ItemGroup(ItemGroup.Columns.Barcode, txtItemNoBarcode.Text.Trim());
                                        correctItemNoList = new ArrayList();
                                        correctItemNames = new ArrayList();
                                        correctDescriptions = new ArrayList();
                                        correctUserflag1s = new ArrayList();
                                        correctItemNoList.Add(txtItemNoBarcode.Text.Trim());
                                        correctItemNames.Add(it.ItemGroupName);
                                        correctDescriptions.Add(it.ItemGroupName);
                                        correctUserflag1s.Add(false);
                                        isItemGroup = true;
                                    }
                                    else
                                    {
                                        ShowPanelPleaseWait();
                                        frmAddItem myAddItem = new frmAddItem();
                                        myAddItem.SupplierName = cmbSupplier.Text;
                                        myAddItem.searchReq = txtItemNoBarcode.Text.Replace(' ', '%');
                                        myAddItem.criteria = cmbCriteria.Text;
                                        CommonUILib.displayTransparent();
                                        myAddItem.ShowDialog();
                                        CommonUILib.hideTransparent();

                                        if (myAddItem != null && myAddItem.itemNumbers != null)
                                        {
                                            //correctItemNo = myAddItem.itemNumbers[0].ToString();
                                            correctItemNoList = myAddItem.itemNumbers;
                                            correctItemNames = myAddItem.itemNames;
                                            correctDescriptions = myAddItem.descriptions;
                                            correctUserflag1s = myAddItem.userflag1s;
                                            correctAttributes1 = myAddItem.attributes1;
                                            myAddItem.Dispose();
                                        }
                                        else
                                        {
                                            myAddItem.Dispose();
                                            txtItemNoBarcode.Text = "";
                                            txtItemNoBarcode.Focus();
                                            pnlLoading.Visible = false;
                                            return;
                                        }
                                    }
                                }
                            }
                        }

                        //cek result if
                        if (!ismatrixattributes1)
                        {
                            if (correctItemNoList != null && correctItemNoList.Count == 1)
                            {
                                //ismatrix item add directly to list
                                if ((correctUserflag1s.Count == 1) && (correctUserflag1s[0].ToString().ToLower() == "true"))
                                {
                                    if (!cbDefaultQty.Checked)
                                    {
                                        isshowmatrixform = true;
                                    }
                                }
                            }
                            else
                            {
                                List<string> att1distinct = correctAttributes1.Cast<string>().Distinct().ToList();

                                if ((att1distinct.Count() == 1) && (!att1distinct[0].ToString().Equals("NoAttributes1")))
                                {
                                    isshowmatrixform = true;
                                }
                            }
                        }



                        if (isshowmatrixform)
                        {
                            frmMatrixQtyEntry fMatrix = new frmMatrixQtyEntry(correctAttributes1[0].ToString());
                            CommonUILib.displayTransparent();
                            fMatrix.ShowDialog();
                            CommonUILib.hideTransparent();
                            pnlLoading.Visible = false;

                            if (fMatrix.IsSuccessful)
                            {
                                var itemnolist = fMatrix.ItemNoList;
                                var qtylist = fMatrix.ItemQty;

                                for (int i = itemnolist.Count - 1; i >= 0; i--)
                                {
                                    //AddItem to Inventory
                                    if (!invCtrl.AddItemIntoInventoryStockIn(itemnolist[i].ToString(), (int)qtylist[i], out status))
                                    {
                                        MessageBox.Show("Error: " + status);
                                    }
                                }
                                BindGrid();
                                txtGSTAmount.Text = invCtrl.InvHdr.Tax.GetValueOrDefault(0).ToString("N");
                            }
                        }
                        else
                        {
                            if (!cbDefaultQty.Checked)
                            {
                                if (isItemGroup)
                                {
                                    frmAddItemGroup fAddItemGroup = new frmAddItemGroup();
                                    fAddItemGroup.Barcode = correctItemNoList[0].ToString();
                                    CommonUILib.displayTransparent();
                                    fAddItemGroup.ShowDialog();
                                    CommonUILib.hideTransparent();
                                    pnlLoading.Visible = false;
                                    if (fAddItemGroup.Qty > 0)
                                    {
                                        ItemGroup ig = new ItemGroup(ItemGroup.Columns.Barcode, fAddItemGroup.Barcode);
                                        if (ig != null && ig.ItemGroupId != null)
                                        {
                                            ItemGroupMapCollection itm = new ItemGroupMapCollection();
                                            itm.Where(ItemGroup.Columns.ItemGroupId, ig.ItemGroupId);
                                            itm.Load();
                                            if (itm.Count > 0)
                                            {
                                                foreach (ItemGroupMap igm in itm)
                                                {
                                                    if (!invCtrl.AddItemIntoInventoryStockIn(igm.ItemNo, (int)fAddItemGroup.Qty * igm.UnitQty, out status))
                                                    {
                                                        MessageBox.Show("Error: " + status);
                                                    }
                                                }

                                            }
                                            BindGrid();
                                            txtGSTAmount.Text = invCtrl.InvHdr.Tax.GetValueOrDefault(0).ToString("N");
                                            txtItemNoBarcode.Text = "";
                                            txtItemNoBarcode.Focus();
                                        }
                                    }
                                }
                                else
                                {
                                    //pop up multiple quantity
                                    frmMultipleQtyEntry fQty = new frmMultipleQtyEntry();
                                    fQty.itemNos = correctItemNoList;
                                    fQty.itemNames = correctItemNames;
                                    fQty.descriptions = correctDescriptions;
                                    CommonUILib.displayTransparent();
                                    fQty.ShowDialog();
                                    CommonUILib.hideTransparent();
                                    pnlLoading.Visible = false;
                                    /*pop up keypad 
                                    frmKeypad f = new frmKeypad();
                                    f.IsInteger = true;
                                    CommonUILib.displayTransparent();f.ShowDialog();CommonUILib.hideTransparent();
                                    int qty;*/

                                    if (fQty.IsSuccessful)
                                    {
                                        for (int i = correctItemNoList.Count - 1; i >= 0; i--)
                                        {
                                            //AddItem to Inventory
                                            if (isItemGroup)
                                            {
                                                ItemGroup ig = new ItemGroup(ItemGroup.Columns.Barcode, correctItemNoList[i].ToString());
                                                if (ig != null && ig.ItemGroupId != null)
                                                {
                                                    ItemGroupMapCollection itm = new ItemGroupMapCollection();
                                                    itm.Where(ItemGroup.Columns.ItemGroupId, ig.ItemGroupId);
                                                    itm.Load();
                                                    if (itm.Count > 0)
                                                    {
                                                        foreach (ItemGroupMap igm in itm)
                                                        {
                                                            if (!invCtrl.AddItemIntoInventoryStockIn(igm.ItemNo, (decimal)fQty.ht[correctItemNoList[i].ToString()] * igm.UnitQty, out status))
                                                            {
                                                                MessageBox.Show("Error: " + status);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (
                                                    !invCtrl.AddItemIntoInventoryStockIn(correctItemNoList[i].ToString(), (decimal)fQty.ht[correctItemNoList[i].ToString()],
                                                                                  out status))
                                                {
                                                    MessageBox.Show("Error: " + status);
                                                }
                                            }
                                        }
                                        BindGrid();
                                        txtGSTAmount.Text = invCtrl.InvHdr.Tax.GetValueOrDefault(0).ToString("N");
                                        txtItemNoBarcode.Text = "";
                                        txtItemNoBarcode.Focus();
                                    }
                                }
                            }
                            else
                            {
                                pnlLoading.Visible = false;

                                if (defaultQty == 0)
                                {
                                    MessageBox.Show(LanguageManager.Warning + " Please fill default quantitiy.");
                                }
                                else
                                {
                                    if (isItemGroup)
                                    {
                                        for (int i = 0; i < correctItemNoList.Count; i++)
                                        {
                                            ItemGroup ig = new ItemGroup(ItemGroup.Columns.Barcode, correctItemNoList[i].ToString());
                                            if (ig != null && ig.ItemGroupId != null)
                                            {
                                                ItemGroupMapCollection itm = new ItemGroupMapCollection();
                                                itm.Where(ItemGroup.Columns.ItemGroupId, ig.ItemGroupId);
                                                itm.Load();
                                                if (itm.Count > 0)
                                                {
                                                    foreach (ItemGroupMap igm in itm)
                                                    {
                                                        if (!invCtrl.AddItemIntoInventoryStockIn(igm.ItemNo, defaultQty * igm.UnitQty, out status))
                                                        {
                                                            MessageBox.Show("Error: " + status);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        for (int i = correctItemNoList.Count - 1; i >= 0; i--)
                                        {
                                            //AddItem to Inventory
                                            if (!invCtrl.AddItemIntoInventoryStockIn(correctItemNoList[i].ToString(), defaultQty, out status))
                                            {
                                                MessageBox.Show("Error: " + status);
                                            }
                                        }
                                    }
                                    BindGrid();
                                    txtGSTAmount.Text = invCtrl.InvHdr.Tax.GetValueOrDefault(0).ToString("N");
                                    txtItemNoBarcode.Text = "";
                                    txtItemNoBarcode.Focus();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show(LanguageManager.Error_encounter_ + ex.Message);
            }
        }


        private void ClearAdditionalInformation()
        {
            try
            {
                if (txtFreightCharges != null) txtFreightCharges.Text = "0";
                if (txtDiscount != null) txtDiscount.Text = "0";
                if (txtExchangeRate != null) txtExchangeRate.Text = "1";
                if (txtPurchaseOrder != null) txtPurchaseOrder.Text = "";
                if (cmbSupplier != null && cmbSupplier.Items.Count > 0) cmbSupplier.SelectedIndex = 0;
                if (txtVendorInvoiceNo != null) txtVendorInvoiceNo.Text = "";
                invCtrl.SetGSTRule(cmbGST.SelectedValue.ToString().GetIntValue());

                txtCustomField1.Text = "";
                txtCustomField2.Text = "";
                txtCustomField3.Text = "";
                txtCustomField4.Text = "";
                txtCustomField5.Text = "";
                txtAdditionalCost1.Text = "0";
                txtAdditionalCost2.Text = "0";
                txtAdditionalCost3.Text = "0";
                txtAdditionalCost4.Text = "0";
                txtAdditionalCost5.Text = "0";

                LoadInventoryController();
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.OnlyAllowInCurrentInventoryLocation), false))
                {
                    //InventoryLocation tmp = new InventoryLocation(PointOfSaleInfo.InventoryLocationID);
                    for (int i = 0; i < cmbLocation.Items.Count; i++)
                    {
                        if (((InventoryLocation)(cmbLocation.Items[i])).InventoryLocationID == PointOfSaleInfo.InventoryLocationID)
                        {
                            cmbLocation.SelectedIndex = i;
                            invCtrl.SetInventoryLocation(((InventoryLocation)(cmbLocation.SelectedValue)).InventoryLocationID);
                            cmbLocation.Enabled = false;
                        }
                    }

                }

                txtRefNo.Text = InventoryController.getNewInventoryRefNo(PointOfSaleInfo.InventoryLocationID);
                if (useCustomRefNo)
                {
                    if (generateNoInServer)
                    {
                        txtRefNo.Text = InventoryController.CreateNewCustomRefNo();
                    }
                    else
                    {
                        invCtrl.SetCustomRefNo(InventoryController.CreateNewCustomRefNo());
                        txtRefNo.Text = invCtrl.GetCustomRefNo();
                    }
                }

                isLoaded = false;
                BindGrid();
                txtGSTAmount.Text = invCtrl.InvHdr.Tax.GetValueOrDefault(0).ToString("N");
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show(LanguageManager.Error_encounter_ + ex.Message);
            }
        }

        public override void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                if (invCtrl.GetNumberOfLineItem() == 0)
                {
                    MessageBox.Show(LanguageManager.There_is_no_item_to_be_printed_);
                    return;
                }
                ShowPanelPleaseWait();
                string PurchaseOrderNo;
                string SupplierName = string.Empty;
                string FreightCharges;
                string Discount;
                string ExchangeRate;
                string VendorInvoiceNo;

                PurchaseOrderNo = ((TextBox)this.Controls.Find("txtPurchaseOrder", true)[0]).Text;
                if ((int)cmbSupplier.SelectedValue != -1)
                {
                    SupplierName = ((ComboBox)this.Controls.Find("cmbSupplier", true)[0]).Text;
                }
                FreightCharges = ((TextBox)this.Controls.Find("txtFreight", true)[0]).Text.GetDecimalValue().ToString("N2");
                Discount = ((TextBox)this.Controls.Find("txtDiscount", true)[0]).Text.GetDecimalValue().ToString("N2");
                if (this.Controls.Find("txtExchangeRate", true).Length > 0)
                    ExchangeRate = ((TextBox)this.Controls.Find("txtExchangeRate", true)[0]).Text;
                else
                    ExchangeRate = "1";
                string StockActivity = this.Text;

                VendorInvoiceNo = ((TextBox)this.Controls.Find("txtVendorInvoiceNo", true)[0]).Text;

                #region *) Assign additional fields' Label and Value
                string CustomField1Label, CustomField2Label, CustomField3Label, CustomField4Label, CustomField5Label,
                    CustomField1Value, CustomField2Value, CustomField3Value, CustomField4Value, CustomField5Value,
                    AdditionalCost1Label, AdditionalCost2Label, AdditionalCost3Label, AdditionalCost4Label, AdditionalCost5Label,
                    AdditionalCost1Value, AdditionalCost2Value, AdditionalCost3Value, AdditionalCost4Value, AdditionalCost5Value;

                CustomField1Label = "" + txtCustomField1.Tag;
                CustomField2Label = "" + txtCustomField2.Tag;
                CustomField3Label = "" + txtCustomField3.Tag;
                CustomField4Label = "" + txtCustomField4.Tag;
                CustomField5Label = "" + txtCustomField5.Tag;
                CustomField1Value = txtCustomField1.Text;
                CustomField2Value = txtCustomField2.Text;
                CustomField3Value = txtCustomField3.Text;
                CustomField4Value = txtCustomField4.Text;
                CustomField5Value = txtCustomField5.Text;

                AdditionalCost1Label = "" + txtAdditionalCost1.Tag;
                AdditionalCost2Label = "" + txtAdditionalCost2.Tag;
                AdditionalCost3Label = "" + txtAdditionalCost3.Tag;
                AdditionalCost4Label = "" + txtAdditionalCost4.Tag;
                AdditionalCost5Label = "" + txtAdditionalCost5.Tag;
                AdditionalCost1Value = txtAdditionalCost1.Text;
                AdditionalCost2Value = txtAdditionalCost2.Text;
                AdditionalCost3Value = txtAdditionalCost3.Text;
                AdditionalCost4Value = txtAdditionalCost4.Text;
                AdditionalCost5Value = txtAdditionalCost5.Text;
                #endregion

                frmStockIn.PrintStockInSheet
                    (invCtrl, PurchaseOrderNo, SupplierName, FreightCharges, Discount,
                    ExchangeRate, StockActivity, showOnHandQty, showCostPrice,
                    CustomField1Label, CustomField2Label, CustomField3Label, CustomField4Label, CustomField5Label,
                    CustomField1Value, CustomField2Value, CustomField3Value, CustomField4Value, CustomField5Value,
                    AdditionalCost1Label, AdditionalCost2Label, AdditionalCost3Label, AdditionalCost4Label, AdditionalCost5Label,
                    AdditionalCost1Value, AdditionalCost2Value, AdditionalCost3Value, AdditionalCost4Value, AdditionalCost5Value,
                    VendorInvoiceNo);
                pnlLoading.Visible = false;
                this.Refresh();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show(LanguageManager.Error_encounter_ + ex.Message);
            }

        }

        public static void PrintStockInSheet
            (InventoryController invController, string PurchaseOrderNo,
            string SupplierName, string FreightCharges, string Discount,
             string ExchangeRate, string StockActivity, bool displayStockOnHand, bool displayCostPrice,
            string CustomField1Label, string CustomField2Label, string CustomField3Label, string CustomField4Label, string CustomField5Label,
            string CustomField1Value, string CustomField2Value, string CustomField3Value, string CustomField4Value, string CustomField5Value,
            string AdditionalCost1Label, string AdditionalCost2Label, string AdditionalCost3Label, string AdditionalCost4Label, string AdditionalCost5Label,
            string AdditionalCost1Value, string AdditionalCost2Value, string AdditionalCost3Value, string AdditionalCost4Value, string AdditionalCost5Value,
            string VendorInvoiceNo)
        {
            try
            {
                frmViewInventorySheet f = new frmViewInventorySheet();
                f.invCtrl = invController;
                f.showCostPrice = displayCostPrice;
                f.showOnHand = displayStockOnHand;
                f.StockActivity = StockActivity;
                //use find control...
                PrintOutParameters printOutParameter = new PrintOutParameters();
                printOutParameter.UserField1Label = "Purchase Order No";
                printOutParameter.UserField1Value = PurchaseOrderNo;
                if (printOutParameter.UserField1Value == "") printOutParameter.UserField1Value = "-";
                printOutParameter.UserField2Label = "Supplier";
                printOutParameter.UserField2Value = SupplierName;
                if (printOutParameter.UserField2Value == "") printOutParameter.UserField2Value = "-";
                printOutParameter.UserField3Label = "Freight Charges";
                printOutParameter.UserField3Value = FreightCharges;
                if (printOutParameter.UserField3Value == "") printOutParameter.UserField3Value = "-";
                printOutParameter.UserField4Label = "Discount";
                printOutParameter.UserField4Value = Discount;
                if (printOutParameter.UserField4Value == "") printOutParameter.UserField4Value = "-";
                printOutParameter.UserField5Label = "Exchange Rate";
                printOutParameter.UserField5Value = ExchangeRate;
                if (printOutParameter.UserField5Value == "") printOutParameter.UserField5Value = "-";
                printOutParameter.UserField6Label = "Vendor Invoice No";
                printOutParameter.UserField6Value = VendorInvoiceNo;
                if (printOutParameter.UserField6Value == "") printOutParameter.UserField6Value = "-";


                #region *) Additional fields
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.ShowCustomField1), false))
                {
                    printOutParameter.CustomField1Label = CustomField1Label;
                    printOutParameter.CustomField1Value = CustomField1Value;
                }
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.ShowCustomField2), false))
                {
                    printOutParameter.CustomField2Label = CustomField2Label;
                    printOutParameter.CustomField2Value = CustomField2Value;
                }
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.ShowCustomField3), false))
                {
                    printOutParameter.CustomField3Label = CustomField3Label;
                    printOutParameter.CustomField3Value = CustomField3Value;
                }
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.ShowCustomField4), false))
                {
                    printOutParameter.CustomField4Label = CustomField4Label;
                    printOutParameter.CustomField4Value = CustomField4Value;
                }
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.ShowCustomField5), false))
                {
                    printOutParameter.CustomField5Label = CustomField5Label;
                    printOutParameter.CustomField5Value = CustomField5Value;
                }

                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.ShowAdditionalCost1), false))
                {
                    printOutParameter.AdditionalCost1Label = AdditionalCost1Label;
                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.AdditionalCost1_IsPercentage), false))
                        printOutParameter.AdditionalCost1Value = AdditionalCost1Value + "%";
                    else
                        printOutParameter.AdditionalCost1Value = "$" + AdditionalCost1Value;
                }

                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.ShowAdditionalCost2), false))
                {
                    printOutParameter.AdditionalCost2Label = AdditionalCost2Label;
                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.AdditionalCost2_IsPercentage), false))
                        printOutParameter.AdditionalCost2Value = AdditionalCost2Value + "%";
                    else
                        printOutParameter.AdditionalCost2Value = AdditionalCost2Value;
                }

                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.ShowAdditionalCost3), false))
                {
                    printOutParameter.AdditionalCost3Label = AdditionalCost3Label;
                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.AdditionalCost3_IsPercentage), false))
                        printOutParameter.AdditionalCost3Value = AdditionalCost3Value + "%";
                    else
                        printOutParameter.AdditionalCost3Value = AdditionalCost3Value;
                }

                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.ShowAdditionalCost4), false))
                {
                    printOutParameter.AdditionalCost4Label = AdditionalCost4Label;
                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.AdditionalCost4_IsPercentage), false))
                        printOutParameter.AdditionalCost4Value = AdditionalCost4Value + "%";
                    else
                        printOutParameter.AdditionalCost4Value = AdditionalCost4Value;
                }

                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.ShowAdditionalCost5), false))
                {
                    printOutParameter.AdditionalCost5Label = AdditionalCost5Label;
                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.AdditionalCost5_IsPercentage), false))
                        printOutParameter.AdditionalCost5Value = AdditionalCost5Value + "%";
                    else
                        printOutParameter.AdditionalCost5Value = AdditionalCost5Value;
                }
                #endregion

                f.printOutParameters = printOutParameter;
                CommonUILib.displayTransparent(); f.ShowDialog(); CommonUILib.hideTransparent();
                f.Dispose();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show(LanguageManager.Error_encounter_ + ex.Message);
            }
        }



        private void GetCustomGRNoSettingFromServer()
        {
            try
            {
                SyncClientController.Load_WS_URL();
                PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                ws.Timeout = 100000;
                ws.Url = SyncClientController.WS_URL;
                DataTable dt = ws.GetCustomGRNoSetting();
                if (dt != null)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        AppSetting.SetSetting(dr["key"].ToString(), dr["value"].ToString());
                    }
                    // Now the Custom No will always be generated in server
                    AppSetting.SetSetting(AppSetting.SettingsName.GoodsReceive.GenerateCustomNoInServer, AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.UseCustomNo));
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("Failed to get Ref No from server. Error: " + ex.Message);
            }
        }

        private void SetAdditionalCost()
        {
            InventoryHdr InvHdr = invCtrl.InvHdr;
            InventoryDetCollection InvDet = invCtrl.InvDet;

            decimal freightCharges, discount;
            double exchangeRate;

            if (txtExchangeRate.Text == "") txtExchangeRate.Text = "0";
            if (txtFreightCharges.Text == "") txtFreightCharges.Text = "0";
            if (txtDiscount.Text == "") txtDiscount.Text = "0";

            if (!decimal.TryParse(txtFreightCharges.Text, out freightCharges))
            {
                MessageBox.Show(LanguageManager.Invalid_Freight_Charges);
                tbControl.SelectedIndex = 0;
                txtFreightCharges.Focus();
                return;
            }
            if (!decimal.TryParse(txtDiscount.Text, out discount))
            {
                MessageBox.Show(LanguageManager.Invalid_Discount_);
                tbControl.SelectedIndex = 0;
                txtDiscount.Focus();
                return;
            }

            if (!double.TryParse(txtExchangeRate.Text, out exchangeRate))
            {
                MessageBox.Show(LanguageManager.Invalid_Exchange_Rate);
                tbControl.SelectedIndex = 0;
                txtExchangeRate.Focus();
                return;
            }

            decimal additionalCost1, additionalCost2, additionalCost3, additionalCost4, additionalCost5;

            if (txtAdditionalCost1.Text == "") txtAdditionalCost1.Text = "0";
            if (txtAdditionalCost2.Text == "") txtAdditionalCost2.Text = "0";
            if (txtAdditionalCost3.Text == "") txtAdditionalCost3.Text = "0";
            if (txtAdditionalCost4.Text == "") txtAdditionalCost4.Text = "0";
            if (txtAdditionalCost5.Text == "") txtAdditionalCost5.Text = "0";

            if (!decimal.TryParse(txtAdditionalCost1.Text, out additionalCost1))
            {
                MessageBox.Show(LanguageManager.Invalid_value_for + " " + txtAdditionalCost1.Tag);
                tbControl.SelectedIndex = 0;
                txtAdditionalCost1.Focus();
                return;
            }
            if (!decimal.TryParse(txtAdditionalCost2.Text, out additionalCost2))
            {
                MessageBox.Show(LanguageManager.Invalid_value_for + " " + txtAdditionalCost2.Tag);
                tbControl.SelectedIndex = 0;
                txtAdditionalCost2.Focus();
                return;
            }
            if (!decimal.TryParse(txtAdditionalCost3.Text, out additionalCost3))
            {
                MessageBox.Show(LanguageManager.Invalid_value_for + " " + txtAdditionalCost3.Tag);
                tbControl.SelectedIndex = 0;
                txtAdditionalCost3.Focus();
                return;
            }
            if (!decimal.TryParse(txtAdditionalCost4.Text, out additionalCost4))
            {
                MessageBox.Show(LanguageManager.Invalid_value_for + " " + txtAdditionalCost4.Tag);
                tbControl.SelectedIndex = 0;
                txtAdditionalCost4.Focus();
                return;
            }
            if (!decimal.TryParse(txtAdditionalCost5.Text, out additionalCost5))
            {
                MessageBox.Show(LanguageManager.Invalid_value_for + " " + txtAdditionalCost5.Tag);
                tbControl.SelectedIndex = 0;
                txtAdditionalCost5.Focus();
                return;
            }

            InvHdr.FreightCharge = freightCharges;
            InvHdr.ExchangeRate = exchangeRate;
            InvHdr.Discount = discount;

            invCtrl.SetInventoryHeaderAdditionalInfo(txtCustomField1.Text, txtCustomField2.Text, txtCustomField3.Text,
                    txtCustomField4.Text, txtCustomField5.Text, additionalCost1, additionalCost2, additionalCost3,
                    additionalCost4, additionalCost5);
        }

        protected override void dgvStock_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            base.dgvStock_CellClick(sender, e);
            txtGSTAmount.Text = invCtrl.InvHdr.Tax.GetValueOrDefault(0).ToString("N");
        }
    }
}
