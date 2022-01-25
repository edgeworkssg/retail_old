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
    public partial class frmStockOutReturn : frmInventoryParent
    {
        private bool useCustomRefNo;
        private bool generateNoInServer;

        public frmStockOutReturn()
        {
            InitializeComponent();
            LoadInventoryController();

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
                    btnSave.Text = LanguageManager.ISSUE;
                    this.Text = LanguageManager.STOCK_RETURN;
                    AddAdditionalInformation();
                    LoadInventoryController();
                    invCtrl.SetGSTRule(cmbGST.SelectedValue.ToString().GetIntValue());

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
        private ComboBox cmbStockOutReason;
        private ComboBox cmbSupplier;
        private ComboBox cmbGST;
        private Label txtGSTAmount;
        private bool isLoaded;
        private TextBox txtCreditNoteNo;

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
                lb.Text = LanguageManager.Stock_Out_Reason;
                cmbStockOutReason = CreateInventoryComboBox();
                cmbStockOutReason.DropDownStyle = ComboBoxStyle.DropDownList;
                cmbStockOutReason.Name = "cmbStockOutReason";

                //populate the combo box...
                InventoryStockOutReasonCollection colis = new InventoryStockOutReasonCollection();
                colis.Where(InventoryStockOutReason.Columns.Deleted, false);
                colis.Where(InventoryStockOutReason.Columns.ReasonID, Comparison.GreaterThan, 2);
                colis.Load();
                InventoryStockOutReason ts = new InventoryStockOutReason();
                ts.ReasonName = LanguageManager.Select_Reason;
                ts.ReasonID = -1;
                colis.Insert(0, ts);
                cmbStockOutReason.DataSource = colis;
                tblInventory.Controls.Add(lb, 0, 6);
                tblInventory.Controls.Add(cmbStockOutReason, 1, 6);

                lb = CreateInventoryLabel();
                lb.Text = LanguageManager.Credit_Note_No;
                txtCreditNoteNo = CreateInventoryTextBox();
                txtCreditNoteNo.Name = "txtCreditNoteNo";
                txtCreditNoteNo.Text = "";
                tblInventory.Controls.Add(lb, 3, 6);
                tblInventory.Controls.Add(txtCreditNoteNo, 4, 6);


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



                tblInventory.Controls.Add(lb, 0, 8);
                tblInventory.Controls.Add(cmbGST, 1, 8);


                lb = CreateInventoryLabel();
                lb.Text = gstText + " Amount";
                txtGSTAmount = CreateInventoryLabel();
                txtGSTAmount.Name = "txtGSTAmount";
                txtGSTAmount.Text = "";
                tblInventory.Controls.Add(lb, 0, 9);
                tblInventory.Controls.Add(txtGSTAmount, 1, 9);


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
            BindGrid();
            txtGSTAmount.Text = invCtrl.InvHdr.Tax.GetValueOrDefault(0).ToString("N");
        }

        public override void btnScanItemNo_Click(object sender, EventArgs e)
        {
            try
            {
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

                                        if (!invCtrl.AddItemIntoInventoryStockReturn(det[z].ItemNo.ToString(), multiplier * (int)det[z].UnitQty, ChangeCostPriceStkAdjOut && (this is frmAdjustStock || this is frmStockOut), out status))
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
                            if (correctItemNoList.Count == 1)
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

                                    if (!invCtrl.AddItemIntoInventoryStockReturn(itemnolist[i].ToString(), (int)qtylist[i], ChangeCostPriceStkAdjOut && (this is frmAdjustStock || this is frmStockOut), out status))
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

                                                    if (!invCtrl.AddItemIntoInventoryStockReturn(igm.ItemNo, (int)fAddItemGroup.Qty * igm.UnitQty, ChangeCostPriceStkAdjOut && (this is frmAdjustStock || this is frmStockOut), out status))
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

                                                            if (!invCtrl.AddItemIntoInventoryStockReturn(igm.ItemNo, (decimal)fQty.ht[correctItemNoList[i].ToString()] * igm.UnitQty, ChangeCostPriceStkAdjOut && (this is frmAdjustStock || this is frmStockOut), out status))
                                                            {
                                                                MessageBox.Show("Error: " + status);
                                                            }

                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {

                                                if (!invCtrl.AddItemIntoInventoryStockReturn(correctItemNoList[i].ToString(), (decimal)fQty.ht[correctItemNoList[i].ToString()],
                                                                                  ChangeCostPriceStkAdjOut && (this is frmAdjustStock || this is frmStockOut), out status))
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

                                                        if (!invCtrl.AddItemIntoInventoryStockReturn(igm.ItemNo, defaultQty * igm.UnitQty, ChangeCostPriceStkAdjOut && (this is frmAdjustStock || this is frmStockOut), out status))
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
                                            if (!invCtrl.AddItemIntoInventoryStockReturn(correctItemNoList[i].ToString(), defaultQty, ChangeCostPriceStkAdjOut && (this is frmAdjustStock || this is frmStockOut), out status))
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
                if (cmbStockOutReason.SelectedIndex == 0)
                {
                    MessageBox.Show(LanguageManager.Please_specify_the_stock_out_reason_);
                    tbControl.SelectedIndex = 0;
                    cmbStockOutReason.Select();
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

                string status = "";

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
                    ("", supplier, txtRemark.Text,
                    0.0M, 0, 0.0M);

                invCtrl.SetVendorInvoiceNo(txtCreditNoteNo.Text);

                bool isSuccess = false;
                ShowPanelPleaseWait();

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

                    if (ws.StockReturnCompressed
                        (data,
                        UserInfo.username,
                        ((InventoryStockOutReason)(cmbStockOutReason.SelectedValue)).ReasonID,
                        ((InventoryLocation)(cmbLocation.SelectedValue)).InventoryLocationID,
                        false, true, out newInventoryHdrRefNo, out status))
                    {

                        isSuccess = true;
                        pnlLoading.Visible = false;
                        invCtrl.SetInventoryHdrRefNo(newInventoryHdrRefNo);
                        invCtrl.SetIsNew(false);
                        MessageBox.Show(LanguageManager.Stock_Return_successful);

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
                    if (invCtrl.StockReturn(UserInfo.username,
                        ((InventoryStockOutReason)(cmbStockOutReason.SelectedValue)).ReasonID,
                        ((InventoryLocation)(cmbLocation.SelectedValue)).InventoryLocationID,
                        false, true, out status))
                    {
                        //InventoryController.AssignStockOutToConfirmedOrderUsingTransactionScope();
                        pnlLoading.Visible = false;
                        newInventoryHdrRefNo = invCtrl.GetInvHdrRefNo();
                        newCustomRefNo = invCtrl.GetCustomRefNo();
                        isSuccess = true;
                        MessageBox.Show(LanguageManager.Stock_Return_successful);
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
                    AccessLogController.AddLog(AccessSource.POS, UserInfo.username, "-", string.Format("Stock Return : {0}", invCtrl.GetInvHdrRefNo()), "");

                    if (printAfterConfirm)
                    {
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

        private void ClearAdditionalInformation()
        {
            try
            {
                if (cmbSupplier != null && cmbSupplier.Items.Count > 0) cmbSupplier.SelectedIndex = 0;
                invCtrl.SetGSTRule(cmbGST.SelectedValue.ToString().GetIntValue());

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
                string SupplierName = string.Empty;

                if ((int)cmbSupplier.SelectedValue != -1)
                {
                    SupplierName = ((ComboBox)this.Controls.Find("cmbSupplier", true)[0]).Text;
                }
                string StockActivity = this.Text;
                string CreditNoteNo = ((TextBox)this.Controls.Find("txtCreditNoteNo", true)[0]).Text;

                frmStockOutReturn.PrintStockReturnSheet
                    (invCtrl, SupplierName, "STOCK RETURN", CreditNoteNo, showOnHandQty, showCostPrice);
                pnlLoading.Visible = false;
                this.Refresh();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show(LanguageManager.Error_encounter_ + ex.Message);
            }

        }

        public static void PrintStockReturnSheet
            (InventoryController invController,
            string SupplierName, string StockActivity, string CreditNoteNo, bool displayStockOnHand, bool displayCostPrice
            )
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
                printOutParameter.UserField1Label = "Supplier";
                printOutParameter.UserField1Value = SupplierName;
                if (printOutParameter.UserField1Value == "") printOutParameter.UserField1Value = "-";

                printOutParameter.UserField2Label = "Credit Note No";
                printOutParameter.UserField2Value = CreditNoteNo;
                if (printOutParameter.UserField2Value == "") printOutParameter.UserField2Value = "-";

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

        protected override void dgvStock_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            base.dgvStock_CellClick(sender, e);
            txtGSTAmount.Text = invCtrl.InvHdr.Tax.GetValueOrDefault(0).ToString("N");
        }
    }
}
