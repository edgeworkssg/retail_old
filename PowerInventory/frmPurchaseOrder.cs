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
using System.IO;
using PowerInventory.OrderForms;
using System.Configuration;
using LanguageManager = PowerInventory.Properties.InventoryLanguage;
using Newtonsoft;

namespace PowerInventory
{
    public partial class frmPurchaseOrder : Form
    {
        private string existingConnectionString;
        protected bool showCostPrice;
        protected bool showOnHandQty;
        private bool showPacking, showCurrency;
        private bool showUOM;
        private bool showGST;
        private string filename;
        protected string saveRemark;
        private const string SavedFolder = "SAVED";
        protected const bool printAfterConfirm = true;
        protected bool isTherePendingStockTake;
        private bool useCustomRefNo;

        public void setFileName(string tFileName, string tSavedRemark)
        {
            filename = tFileName;
            saveRemark = tSavedRemark;
        }
        private PurchaseOdrController invCtrl;
        private PurchaseOrderHdrController poCtrl;

        private bool isCostPriceEditable = false;
        private bool isCostPerPackingSizeEditable = false;
        private bool isSellPriceEditable = false;

        public frmPurchaseOrder()
        {
            
            try
            {
                existingConnectionString = DataService.GetInstance("POWERPOS").DefaultConnectionString;
                showCostPrice = !(AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.HideInventoryCost), false));
                showOnHandQty = false;
                isCostPriceEditable = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.IsCostPriceEditable), false);
                isCostPerPackingSizeEditable = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.IsCostPerPackingSizeEditable), false);
                isSellPriceEditable = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.IsSellingPriceEditable), false);

                InitializeComponent();
                dgvStock.AutoGenerateColumns = false;
                lblTotalCostPrice.Visible = true;
                lblTotalCostPriceAmount.Visible = true;
                showUOM = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowUOM), false);
                showGST = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowGST), false);
                showPacking = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowPackingSize), false);
                showCurrency = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowCurrency), false);
                //show total Cost Price (for Robert MiniMart only so far)
                /*if (this is frmStockIn)
                {
                    if (showCostPrice)
                    {
                    }
                    else
                    {
                        lblTotalCostPrice.Visible = false ;
                        lblTotalCostPriceAmount.Visible = false;
                    }
                }*/

                if (!PointOfSaleInfo.IntegrateWithInventory)
                    GetCustomPONoSettingFromServer();

                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.AllowChangeInventoryDate), false))
                    dtpInventoryDate.Enabled = true;
                useCustomRefNo = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.UseCustomNo), false);
                btnLoadAddress.Visible = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowLoadAddressBtn), false);
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("Error encounter." + ex.Message);
            }
        }

        protected void ShowPanelPleaseWait()
        {
            try
            {
                pnlLoading.Parent = this;
                pnlLoading.Location = new Point(
                    this.ClientSize.Width / 2 - pnlLoading.Size.Width / 2,
                    this.ClientSize.Height / 2 - pnlLoading.Size.Height / 2);
                pnlLoading.Anchor = AnchorStyles.None;
                pnlLoading.BringToFront();
                pnlLoading.Visible = true;
                this.Refresh();
            }
            catch (Exception ex)
            {
                pnlLoading.Visible = false;
                Logger.writeLog(ex);
                MessageBox.Show("Error encounter." + ex.Message);
            }
        }
        //protected int defaultLoc;
        private void InitiateInventoryController()
        {
            try
            {
                InventoryLocationCollection inv = new InventoryLocationCollection();
                inv.Where(InventoryLocation.Columns.Deleted, false);
                //DB ACCESS
                inv.Load();
                if (invCtrl == null)
                {
                    invCtrl = new PurchaseOdrController();
                }

                if (inv.Count > 1)
                {
                    //cmbLocation.Items.Insert(0, "--Please Select--");
                    InventoryLocation tmp = new InventoryLocation();
                    tmp.InventoryLocationID = 0;
                    tmp.InventoryLocationName = "--Please Select--";
                    inv.Insert(0, tmp);
                }
                cmbLocation.DataSource = inv;
                cmbLocation.SelectedIndex = 0;
                cmbLocation.Refresh();
                
                invCtrl.SetInventoryLocation(((InventoryLocation)(cmbLocation.SelectedValue)).InventoryLocationID);

                if (useCustomRefNo)
                    txtRefNo.Text = PurchaseOdrController.CreateNewCustomRefNo();
                else
                    txtRefNo.Text = PurchaseOdrController.getNewPurchaseOrderRefNo(PointOfSaleInfo.InventoryLocationID);

                BindGrid();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("Error encounter: " + ex.Message);
            }
        }

        protected void LoadInventoryController()
        {
            try
            {
                if (filename != null && filename != "")
                {
                    //DB ACCESS
                    SavedItemController s = new SavedItemController();
                    object obj = s.LoadObject(filename, Application.StartupPath + "\\" + SavedFolder + "\\");
                    if (obj != null)
                    {
                        invCtrl = (PurchaseOdrController)obj;
                        lblGST.Text = invCtrl.GetRemark();
                        dtpInventoryDate.Value = invCtrl.GetPurchaseOrderDate();

                        //assign according to form type
                        //Save all possible additional information - 
                        /*if (this is frmStockIn)
                        {
                            //use find control...
                            ((TextBox)this.Controls.Find("txtPurchaseOrder", true)[0]).Text = invCtrl.getPurchaseOrderNo();
                            ((TextBox)this.Controls.Find("txtSupplier", true)[0]).Text = invCtrl.getSupplier();
                            ((TextBox)this.Controls.Find("txtFreight", true)[0]).Text = invCtrl.GetFreightCharges().ToString();
                            ((TextBox)this.Controls.Find("txtDiscount", true)[0]).Text = invCtrl.getDiscount().ToString();
                            ((TextBox)this.Controls.Find("txtExchangeRate", true)[0]).Text = invCtrl.getExchangeRate().ToString();
                        }
                        else if (this is frmStockOut)
                        {
                            int stockOutReasonID = invCtrl.getStockOutReasonID();
                            ComboBox cmb = ((ComboBox)this.Controls.Find("cmbStockOutReason", true)[0]);
                            for (int i = 0; i < cmb.Items.Count; i++)
                            {
                                if (((InventoryStockOutReason)(cmb.Items[i])).ReasonID == stockOutReasonID)
                                {
                                    cmb.SelectedIndex = i;
                                    break;
                                }
                            }
                        }
                        else if (this is frmStockTransfer)
                        {
                            int transferDestinationID = invCtrl.getTransferDestination();
                            ComboBox cmb = ((ComboBox)this.Controls.Find("cmbDestination", true)[0]);
                            for (int i = 0; i < cmb.Items.Count; i++)
                            {
                                if (((InventoryLocation)(cmb.Items[i])).InventoryLocationID == transferDestinationID)
                                {
                                    cmb.SelectedIndex = i;
                                    break;
                                }
                            }

                        }
                        else if (this is frmAdjustStock)
                        {
                            int adjustmentdirection = invCtrl.getStockAdjustmentDirection();
                            ComboBox cmb = ((ComboBox)this.Controls.Find("cmbAdjustDirection", true)[0]);
                            cmb.SelectedIndex = adjustmentdirection;
                            cmb.Refresh();
                        }
                        else if (this is frmStockTake)
                        {
                            string takenBy = "", verifiedBy = "";

                            invCtrl.GetInventoryStockTakeTakenBy(out takenBy, out verifiedBy);
                            TextBox txtTakenBy = ((TextBox)this.Controls.Find("txtTakenBy", true)[0]);
                            TextBox txtVerifiedBy = ((TextBox)this.Controls.Find("txtVerifiedBy", true)[0]);
                            if (takenBy != null)
                                txtTakenBy.Text = takenBy;
                            if (verifiedBy != null)
                                txtVerifiedBy.Text = verifiedBy.ToString();
                        }*/
                    }
                }
                InitiateInventoryController();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("Error encounter." + ex.Message);
            }
        }

        private void btnScanItemNo_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbSupplier.Text == "--Select Supplier--")
                {
                    MessageBox.Show("Please Select Supplier");
                    return;
                }

                string correctItemNo;
                ArrayList correctItemNoList = null;
                ArrayList correctItemNames = null;
                ArrayList correctDescriptions = null;
                ArrayList correctUserflag1s = null;
                ArrayList correctAttributes1 = null;

                bool isshowmatrixform = false;
                bool ismatrixattributes1 = false;

                if (ItemController.IsInventoryItem(txtItemNoBarcode.Text.Trim()))
                {
                    //by ITEM NUMBER
                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.SupplierIsCompulsory), false))
                    {
                        if (ItemController.IsSupplierItem(txtItemNoBarcode.Text.Trim(), cmbSupplier.Text))
                        {
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
                            MessageBox.Show("Item is not linked with this supplier", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                    else
                    {
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
                            //MessageBox.Show("Not a valid item number of barcode");
                            //POP UP
                            ShowPanelPleaseWait();
                            frmAddItem myAddItem = new frmAddItem();
                            myAddItem.SupplierName = cmbSupplier.Text;
                            myAddItem.searchReq = txtItemNoBarcode.Text.Replace(' ', '%');
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
                string status;
                int defaultQty = Int32.Parse(string.IsNullOrEmpty(txtDefaultQty.Text) ? "0" : txtDefaultQty.Text);

                //cek result if
                if (!ismatrixattributes1)
                {
                    //cek result if
                    if (correctItemNoList.Count == 1)
                    {
                        //ismatrix item
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
                            if (!invCtrl.AddItemIntoInventorySupplier(itemnolist[i].ToString(), (int)qtylist[i]
                                , cmbSupplier.Text, 0, out status))
                            {
                                MessageBox.Show("Error: " + status);
                            }
                        }
                        BindGrid();

                    }
                }
                else
                {
                    if (!cbDefaultQty.Checked)
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
                                if (!invCtrl.AddItemIntoInventorySupplier(correctItemNoList[i].ToString(), (int)fQty.ht[correctItemNoList[i].ToString()], cmbSupplier.Text, 0, out status))
                                {
                                    MessageBox.Show("Error: " + status);
                                }
                            }
                            BindGrid();
                            txtItemNoBarcode.Text = "";
                            txtItemNoBarcode.Focus();
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
                            for (int i = correctItemNoList.Count - 1; i >= 0; i--)
                            {
                                //AddItem to Inventory
                                if (!invCtrl.AddItemIntoInventorySupplier(correctItemNoList[i].ToString(), defaultQty, cmbSupplier.Text, 0, out status))
                                {
                                    MessageBox.Show("Error: " + status);
                                }
                            }
                            BindGrid();
                            txtItemNoBarcode.Text = "";
                            txtItemNoBarcode.Focus();
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

        protected void BindGrid()
        {
            try
            {
                string status = "";

                if (invCtrl == null) return;

                //set order header                


                //populate order items    
                DataTable dt = invCtrl.FetchUnSavedInventoryItems(showOnHandQty, showCostPrice, showPacking, showUOM, showCurrency, out status);   
                dgvStock.DataSource = dt;
                if (status != "")
                {
                    MessageBox.Show("Error: " + status);
                    return;
                }

                //if (this is frmStockIn)
                //{
                if (showCostPrice)
                {
                    decimal totalCostPrice = 0;
                    //populate total cost price column 
                    for (int i = 0; i < dgvStock.Rows.Count; i++)
                    {
                        //if (dgvStock.Rows[i].Cells["PackingSize"].Value != null && dgvStock.Rows[i].Cells["PackingSize"].Value.ToString() != "")
                        //    dgvStock.Rows[i].Cells["TotalCostPrice"].Value = (decimal)((decimal)dgvStock.Rows[i].Cells["CostPerPackingSize"].Value / decimal.Parse(dt.Rows[i]["PackingSizeUOM"].ToString()) * (int)dgvStock.Rows[i].Cells["Quantity"].Value);
                        //else
                            dgvStock.Rows[i].Cells["TotalCostPrice"].Value = (decimal)((decimal)dgvStock.Rows[i].Cells["FactoryPrice"].Value * (int)dgvStock.Rows[i].Cells["Quantity"].Value);
                        totalCostPrice += (decimal)dgvStock.Rows[i].Cells["TotalCostPrice"].Value;
                    }
                    lblTotalCostPriceAmount.Text = totalCostPrice.ToString("N2");

                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowOrderInfo), false))
                    {
                        lblOrderAmountValue.Text = totalCostPrice.ToString("N2");
                        decimal deliveryCharge = 0;
                        if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowDeliveryCharge), false))
                        {
                            decimal minPurchase;
                            if (decimal.TryParse(txtMinPurchase.Text, out minPurchase))
                            {
                                if (minPurchase > totalCostPrice)
                                    decimal.TryParse(txtDeliveryCharge.Text, out deliveryCharge);
                            }
                            lblDeliveryChargeValue.Text = deliveryCharge.ToString("N2");

                        }
                        else
                        {
                            lblDeliveryCharge1.Visible = false;
                            lblDeliveryChargeValue.Visible = false;
                        }

                        decimal Subtotal = totalCostPrice + deliveryCharge;

                        decimal gstAmount = 0;
                        decimal grandTotal = 0;
                        if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowGST), false))
                        {
                            if (cmbGST.SelectedValue.ToString() == "1")
                            {
                                //Exclusive
                                gstAmount = Subtotal * (decimal)0.07;
                                grandTotal = Subtotal + gstAmount;

                            }
                            else if (cmbGST.SelectedValue.ToString() == "2")
                            {
                                //inclusive
                                gstAmount = Subtotal * (decimal)0.07 / (decimal)1.07;
                                grandTotal = Subtotal;
                                Subtotal = Subtotal - gstAmount;

                            }
                            else
                            {
                                gstAmount = 0;
                                grandTotal = Subtotal + gstAmount;
                            }

                        }
                        else
                        {
                            gstAmount = 0;
                            lblGSTValue.Text = "0";
                        }

                        lblSubtotalValue.Text = Subtotal.ToString("N2");
                        lblGSTValue.Text = gstAmount.ToString("N2");
                        lblGrandTotalValue.Text = grandTotal.ToString("N2");
                    }
                    else
                    {
                        lblOrderAmount1.Visible = false;
                        lblOrderAmountValue.Visible = false;
                        lblDeliveryCharge1.Visible = false;
                        lblDeliveryChargeValue.Visible = false;
                        lblSubtotal1.Visible = false;
                        lblSubtotalValue.Visible = false;
                        lblGST1.Visible = false;
                        lblGSTValue.Visible = false;
                        lblGrandTotal1.Visible = false;
                        lblGrandTotalValue.Visible = false;
                    }
                    
                }
                //}

                dgvStock.Refresh();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("An unknown error has been encountered. Please contact your system administrator.");
                txtItemNoBarcode.Focus();
            }
        }

        private void txtItemNoBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnScanItemNo_Click(this, new EventArgs());
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex.Message);
                MessageBox.Show("Error encounter: " + ex.Message);
            }
        }

        private void frmInventoryParent_Load(object sender, EventArgs e)
        {
            try
            {
                existingConnectionString = DataService.GetInstance("POWERPOS").DefaultConnectionString;

                if (!isTherePendingStockTake)
                {
                    //showCostPrice = true;
                    showOnHandQty = true;
                    HideStockBalanceAndFactoryPrice();
                    btnSave.Text = "ORDER";
                    this.Text = "PURCHASE ORDER";
                    AddAdditionalInformation();
                    LoadInventoryController();

                }
            }

            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        /*private ComboBox cmbSupplier;
        private TextBox txtFreightCharges;
        private DateTimePicker txtDeliveryDate;
        private ComboBox cmbGST;
        private TextBox txtDiscount;
        private TextBox txtExchangeRate;
        private ComboBox cmbCurrencies;
        private TextBox txtDeliveryAddress;
        private TextBox txtPaymentTerm;*/

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
                //lb = CreateInventoryLabel();
                //lb.Text = "Supplier";
                //txtSupplier = CreateInventoryTextBox();
                //txtSupplier.Name = "txtSupplier";
                //tblInventory.Controls.Add(lb, 0, 7);
                //tblInventory.Controls.Add(txtSupplier, 1, 7);

                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowSupplier), false))
                {
                    /*lb = CreateInventoryLabel();
                    lb.Text = "Supplier";
                    cmbSupplier = CreateInventoryComboBox();
                    cmbSupplier.DropDownStyle = ComboBoxStyle.DropDownList;
                    cmbSupplier.Name = "cmbSupplier";*/

                    DataSet dsSupplier = ws.GetDataTable("Supplier", true);
                    DataTable dtSupplier = new DataTable();
                    if (dsSupplier.Tables.Count > 0)
                    {
                        dtSupplier = dsSupplier.Tables[0];
                    }
                    DataView dv = dtSupplier.DefaultView;
                    dv.Sort = "SupplierName ASC";
                    DataTable dtSortedSupplier = dv.ToTable();
                    DataRow drSupplier = dtSortedSupplier.NewRow();
                    drSupplier["SupplierID"] = -1;
                    drSupplier["SupplierName"] = "--Select Supplier--";
                    dtSortedSupplier.Rows.InsertAt(drSupplier, 0);
                    dtSortedSupplier.AcceptChanges();
                    cmbSupplier.DataSource = dtSortedSupplier;
                    cmbSupplier.DisplayMember = "SupplierName";
                    cmbSupplier.ValueMember = "SupplierID";
                    //tblInventory.Controls.Add(lb, 0, 6);
                    //tblInventory.Controls.Add(cmbSupplier, 1, 6);8
                }
                else
                {
                    lblSupplier.Visible = false;
                    cmbSupplier.Visible = false;
                }

                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowDeliveryDateTime), false))
                {
                    /*lb = CreateInventoryLabel();
                    lb.Text = "Delivery Date Time";
                    txtDeliveryDate = CreateInventoryDateTimePicker();
                    txtDeliveryDate.Name = "txtDeliveryDate";*/
                    //txtDeliveryDate.Text = "0";
                    txtDeliveryDate.Value = DateTime.Now;
                    txtDeliveryDate.Format = DateTimePickerFormat.Custom;
                    txtDeliveryDate.CustomFormat = "dd-MM-yyyy";

                    //tblInventory.Controls.Add(lb, 0, 7);
                    //tblInventory.Controls.Add(txtDeliveryDate, 1, 7);
                }
                else
                {
                    lblDeliveryDate.Visible = false;
                    txtDeliveryDate.Visible = false;
                    lblReceivingTime.Visible = false;
                    txtReceivingTime.Visible = false;

                }

                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowGST), false))
                {
                    
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
                    dr["GSTType"] = "Non GST";
                    dtGST.Rows.Add(dr);

                    cmbGST.DataSource = dtGST;
                    cmbGST.DisplayMember = "GSTType";
                    cmbGST.ValueMember = "ID";
                    cmbGST.SelectedValue = 1;
                    //tblInventory.Controls.Add(lb, 0, 8);
                    //tblInventory.Controls.Add(cmbGST, 1, 8);
                }
                else
                {
                    lblGST.Visible = false;
                    cmbGST.Visible = false;
                }

                if (!AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowDeliveryAddress), false))
                {
                    lblDeliveryAddress.Visible = false;
                    txtDeliveryAddress.Visible = false;

                    /*lb = CreateInventoryLabel();
                    //lb.Text = "Delivery Address";
                    txtDeliveryAddress = CreateInventoryTextBox();
                    txtDeliveryAddress.Name = "txtDeliveryAddress";
                    txtDeliveryAddress.Text = "0";
                    tblInventory.Controls.Add(lb, 3, 6);
                    tblInventory.Controls.Add(txtDeliveryAddress, 4, 6);*/
                }

                if (!AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowMinPurchase), false))
                {
                    lblMinPurchase.Visible = false;
                    txtMinPurchase.Visible = false;
                }
                else
                {
                    txtMinPurchase.Text = "0";
                }
                
                if (!AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowDeliveryCharge), false))
                {
                    lblDeliveryCharge.Visible = false;
                    txtDeliveryCharge.Visible = false;
                }
                else
                {
                    txtDeliveryCharge.Text = "0";
                }

                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowCurrency), false))
                {
                    CurrencyCollection col = new CurrencyCollection();
                    col.Where(PowerPOS.Currency.Columns.Deleted, false);
                    col.Load();
                    Currency t = new Currency();
                    t.CurrencyCode = "--Select Currency--";
                    t.CurrencyId = -1;
                    col.Insert(0, t);
                    cmbCurrencies.DataSource = col;
                    cmbCurrencies.DisplayMember = "CurrencyCode";
                    cmbCurrencies.ValueMember = "CurrencyId";
                }
                else
                {
                    cmbCurrencies.Visible = false;
                    lblCurrencies.Visible = false;
                }
                

                

                if (!AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowPaymentType), false))
                {
                    lblPaymentTerm.Visible = false;
                    txtPaymentTerm.Visible = false;
                    /*lb = CreateInventoryLabel();
                    lb.Text = "Payment Term";
                    txtPaymentTerm = CreateInventoryTextBox();
                    txtPaymentTerm.Name = "txtPaymentTerm";
                    txtPaymentTerm.Text = "0";
                    tblInventory.Controls.Add(lb, 3, 9);
                    tblInventory.Controls.Add(txtPaymentTerm, 4, 9);
                    tblInventory.SetRowSpan(txtPaymentTerm, 3);*/
                }
                /*lb = CreateInventoryLabel();
                lb.Text = "Discount";
                txtDiscount = CreateInventoryTextBox();
                txtDiscount.Name = "txtDiscount";
                txtDiscount.Text = "0";
                tblInventory.Controls.Add(lb, 3, 6);
                tblInventory.Controls.Add(txtDiscount, 4, 6);*/

                /*lb = CreateInventoryLabel();
                lb.Text = "Exchange Rate";
                txtExchangeRate = CreateInventoryTextBox();
                txtExchangeRate.Name = "txtExchangeRate";
                txtExchangeRate.Text = "1";
                tblInventory.Controls.Add(lb, 3, 8);
                tblInventory.Controls.Add(txtExchangeRate, 4, 8);
                
                lb = CreateInventoryLabel();
                lb.Text = "Currencies";
                cmbCurrencies = CreateInventoryComboBox();
                cmbCurrencies.DropDownStyle = ComboBoxStyle.DropDownList;
                cmbCurrencies.Name = "cmbCurrencies";
                cmbCurrencies.SelectedIndexChanged += new EventHandler(this.cmbCurrencies_SelectedIndexChanged);

                //populate the combo box...
                CurrencyCollection col = new CurrencyCollection();
                col.Where(Currency.Columns.Deleted, false);
                col.Load();
                Currency t = new Currency();
                t.CurrencyCode = "--Select Currency--";
                t.CurrencyId = -1;
                col.Insert(0, t);
                cmbCurrencies.DataSource = col;
                tblInventory.Controls.Add(lb, 3, 9);
                tblInventory.Controls.Add(cmbCurrencies, 4, 9);*/

                tblInventory.Refresh();
                showCostPrice = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowFactoryPrice), false);
                //make total cost price column visible
                if (showCostPrice)
                {
                    dgvStock.Columns["TotalCostPrice"].Visible = true;
                    dgvStock.Columns["RetailPrice"].Visible = true;
                }
                else
                {
                    dgvStock.Columns["TotalCostPrice"].Visible = false;
                    dgvStock.Columns["RetailPrice"].Visible = false;
                }

                //bool showPackingSize = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowPackingSize), false);
                if (showPacking)
                {
                    dgvStock.Columns["PackingSize"].Visible = true;
                    dgvStock.Columns["ChoosePackingSize"].Visible = true;
                }
                else
                {
                    dgvStock.Columns["PackingSize"].Visible = false;
                    dgvStock.Columns["ChoosePackingSize"].Visible = false;
                }

                bool showUOM = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowUOM), false);
                if (showUOM)
                {
                    dgvStock.Columns["UOM"].Visible = true;
                }
                else
                {
                    dgvStock.Columns["UOM"].Visible = false;
                }

                /*if (showCurrency)
                {
                    dgvStock.Columns["Currency"].Visible = true;
                }
                else
                {*/
                    dgvStock.Columns["Currency"].Visible = false;
                //}

                bool showRetailPrice = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowRetailPrice), false);
                if (showRetailPrice)
                {
                    dgvStock.Columns["RetailPrice"].Visible = true;
                    dgvStock.Columns["RetailPrice"].HeaderText = "Selling Price";
                }
                else
                {
                    dgvStock.Columns["RetailPrice"].Visible = false;
                    dgvStock.Columns["RetailPrice"].HeaderText = "Retail Price";
                }

            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("Error encounter." + ex.Message);
            }
        }

        private void cmbCurrencies_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*try
            {
                if (cmbCurrencies.SelectedIndex != 0)
                {
                    DialogResult dr = MessageBox.Show
                        ("WARNING. You will lose all your modified cost priced. Are you sure you want to change your Currency?", "", MessageBoxButtons.YesNo);
                    if (dr == DialogResult.Yes)
                    {
                        invCtrl.UpdateCurrency(((Currency)cmbCurrencies.SelectedValue).CurrencyId);
                        BindGrid();
                    }
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
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("Error encounter." + ex.Message);
            }*/
        }

        protected void HideStockBalanceAndFactoryPrice()
        {
            try
            {
                if (!showOnHandQty)
                {
                    dgvStock.Columns["OnHand"].Visible = false;
                }
                if (!showCostPrice)
                {
                    dgvStock.Columns["CostPerPackingSize"].Visible = false;
                    dgvStock.Columns["FactoryPrice"].Visible = false;
                    dgvStock.Columns["TotalCostPrice"].Visible = false;
                }
                dgvStock.Refresh();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                this.Close();
            }
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            //pop up a new form to ask for new item....

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (IsLineItemEmpty())
            {
                MessageBox.Show("Please tick the item you wish to process.");
                tbControl.SelectedIndex = 1;
                return;
            }
            if (cmbLocation.Items.Count > 1 && cmbLocation.SelectedIndex == 0)
            {
                MessageBox.Show("Please select the location.");
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
            //validate quantity 
            //bool check = true;
            foreach (PurchaseOrderDet poDet in invCtrl.GetPODetail())
            {
                if (poDet.PackingSizeName != null && poDet.PackingSizeName != "" && poDet.PackingSizeUOM != 0)
                {
                    if (!invCtrl.ValidateQuantity(poDet.PurchaseOrderDetRefNo, poDet.PackingSizeUOM))
                    {

                        MessageBox.Show("Item " + poDet.Item.ItemNo + "-" + poDet.Item.ItemName + " order qty is not multiply of the packing size quantity. Please rectify before proceed.");
                        return;
                    }
                    
                }
            }
            
            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowGST), false))
            {
                if (cmbGST.Items.Count > 1 && cmbGST.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select the GST Type.");
                    tbControl.SelectedIndex = 0;
                    cmbGST.Focus();
                    return;
                }
            }

            string status;
            //decimal freightCharges, discount;
            //double exchangeRate;

            //if (txtExchangeRate.Text == "") txtExchangeRate.Text = "0";
            //if (txtFreightCharges.Text == "") txtFreightCharges.Text = "0";
            //if (txtDiscount.Text == "") txtDiscount.Text = "0";

            /*if (!decimal.TryParse(txtFreightCharges.Text, out freightCharges))
            {
                MessageBox.Show("Invalid Freight Charges");
                tbControl.SelectedIndex = 0;
                txtFreightCharges.Focus();
                return;
            }*/
            /*if (!decimal.TryParse(txtDiscount.Text, out discount))
            {
                MessageBox.Show("Invalid Discount");
                tbControl.SelectedIndex = 0;
                txtDiscount.Focus();
                return;
            }*/

            /*if (!double.TryParse(txtExchangeRate.Text, out exchangeRate))
            {
                MessageBox.Show("Invalid Exchange Rate");
                tbControl.SelectedIndex = 0;
                txtExchangeRate.Focus();
                return;
            }*/
            decimal minPurchase = 0;
            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowMinPurchase), false))
            {
                if (!decimal.TryParse(txtMinPurchase.Text, out minPurchase))
                {
                    MessageBox.Show("Invalid Min Purchase");
                    tbControl.SelectedIndex = 0;
                    txtMinPurchase.Focus();
                    return;
                }
            }

            decimal delCharge = 0;
            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowDeliveryCharge), false))
            {
                if (!decimal.TryParse(txtDeliveryCharge.Text, out delCharge))
                {
                    MessageBox.Show("Invalid Delivery Charge");
                    tbControl.SelectedIndex = 0;
                    txtDeliveryCharge.Focus();
                    return;
                }
            }
            if (!CommonUILib.ShowAreYouSure()) return;

            PurchaseOrderDetCollection tmpDetCol;
            RemoveUnticked(out status, out tmpDetCol);


            invCtrl.SetPurchaseOrderHeaderInfo
                    (cmbSupplier.SelectedValue.ToString() == "-1" ? string.Empty :
                    cmbSupplier.SelectedValue.ToString(), lblGST.Text,
                    0, 1, 0);

            DateTime delDate = DateTime.Now;
            string receiveTime = "";
            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowDeliveryDateTime), false))
            {
                delDate = txtDeliveryDate.Value;
                receiveTime = txtReceivingTime.Text;
            }

            string delAddress = "";
            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowDeliveryAddress), false))
            {
                delAddress = txtDeliveryAddress.Text;
            }

            string paymentTerm = "";
            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowPaymentType), false))
            {
                paymentTerm = txtPaymentTerm.Text;
            }

            int gstType = 0;
            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowGST), false))
            {
                gstType = int.Parse(cmbGST.SelectedValue.ToString());
            }
            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowCurrency), false))
            {
                int tmpCur = 0;
                if (int.TryParse(cmbCurrencies.SelectedValue.ToString(), out tmpCur))
                    invCtrl.SetCurrency(tmpCur);
            }
            invCtrl.SetAdditionalHeaderInfo(delDate, delAddress, paymentTerm, receiveTime, gstType.ToString(), minPurchase, delCharge);
            
            bool isSuccess = false;
            string newPurchaseOrderHdrRefNo = "";
            //ShowPanelPleaseWait();
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

                if (ws.PurchaseOrderCompressed
                    (data,
                    UserInfo.username,
                    ((InventoryLocation)(cmbLocation.SelectedValue)).InventoryLocationID,
                    false, out newPurchaseOrderHdrRefNo, out status))
                {
                    if (useCustomRefNo)
                    {
                        if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.GenerateCustomNoInServer), false))
                        {
                            // Get the latest number from server (note: update has already performed in server)
                            GetCustomPONoSettingFromServer();
                        }
                        else
                        {
                            PurchaseOdrController.CustomRefNoUpdate();
                        }
                    }

                    //download inventoryhdr and inventorydet                        
                    if (SyncClientController.GetCurrentPurchaseOrder())
                    {
                        isSuccess = true;
                        pnlLoading.Visible = false;
                        MessageBox.Show("Purchase Order successful");
                    }
                    else
                    {
                        Logger.writeLog("Unable to download data from server: " + status);
                        isSuccess = false;
                        this.Close();
                        return;
                    }
                }
                else
                {
                    MessageBox.Show(status);
                }

            }
            else
            {
                if (invCtrl.CreateOrder(UserInfo.username,
                        ((InventoryLocation)(cmbLocation.SelectedValue)).InventoryLocationID,
                        out status))
                {
                    //InventoryController.AssignStockOutToConfirmedOrderUsingTransactionScope();
                    pnlLoading.Visible = false;
                    newPurchaseOrderHdrRefNo = invCtrl.GetPurchaseOrderHdrRefNo();
                    isSuccess = true;
                    MessageBox.Show("Purchase Order successful");
                }
                else
                {
                    pnlLoading.Visible = false;
                    isSuccess = false;
                    MessageBox.Show("Error!" + status);
                }

            }
            //print
            if (isSuccess)
            {
                AccessLogController.AddLog(AccessSource.POS, UserInfo.username, "-", "Stock Transfer : " + invCtrl.InvHdr.PurchaseOrderHdrRefNo, "");
                
                if (printAfterConfirm)
                {
                    invCtrl = new PurchaseOdrController(newPurchaseOrderHdrRefNo); //reload...
                    btnPrint_Click(this, new EventArgs());
                }

                //clear control.....                
                invCtrl = new PurchaseOdrController();
                if (tmpDetCol.Count > 0)
                {
                    //invCtrl.SetInventoryLocation(((InventoryLocation)(cmbLocation.SelectedValue)).InventoryLocationID);
                    //invCtrl.AddDet(tmpDetCol);
                    //SaveToDisk(true);
                }
                else
                {
                    ClearControls();
                    ClearAdditionalInformation();
                    //cmbLocation.SelectedIndex = defaultLoc;
                    invCtrl.SetInventoryLocation(((InventoryLocation)(cmbLocation.SelectedValue)).InventoryLocationID);
                }
                BindGrid();
                tbControl.SelectedIndex = 0;
            }

        }

        private void ClearAdditionalInformation()
        {
            try
            {
                /*if (txtDeliveryAddress.Visible != false)*/ txtDeliveryAddress.Text = "";
                /*if (txtPaymentTerm.Visible != false)*/ txtPaymentTerm.Text = "";
                
                /*if (cmbSupplier.Visible != false)*/ cmbSupplier.SelectedValue = -1;
                /*if (txtDeliveryDate.Visible != false)*/ txtDeliveryDate.Value = DateTime.Now;
                /*if (txtMinPurchase.Visible != false)*/ txtMinPurchase.Text = "0";
                /*if (txtDeliveryCharge.Visible != false)*/ txtDeliveryCharge.Text = "0";

                LoadInventoryController();
                //if (txtFreightCharges != null) txtFreightCharges.Text = "0";
                //if (txtDiscount != null) txtDiscount.Text = "0";
                //if (txtExchangeRate != null) txtExchangeRate.Text = "1";
                //if (txtPurchaseOrder != null) txtPurchaseOrder.Text = "";
                //if (cmbSupplier != null) cmbSupplier.SelectedValue = 0;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("Error encounter." + ex.Message);
            }
        }

        protected TextBox CreateInventoryTextBox()
        {
            try
            {
                TextBox tb = new TextBox();
                tb.Anchor =
                    ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                            | System.Windows.Forms.AnchorStyles.Left)
                            | System.Windows.Forms.AnchorStyles.Right)));
                tb.Margin = new System.Windows.Forms.Padding(0);
                return tb;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("Error: " + ex.Message);
                return null;
            }
        }

        

        protected DateTimePicker CreateInventoryDateTimePicker()
        {
            try
            {
                DateTimePicker tb = new DateTimePicker();
                tb.Anchor =
                    ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                            | System.Windows.Forms.AnchorStyles.Left)
                            | System.Windows.Forms.AnchorStyles.Right)));
                tb.Margin = new System.Windows.Forms.Padding(0);
                return tb;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("Error: " + ex.Message);
                return null;
            }
        }

        protected Label CreateInventoryLabel()
        {
            try
            {
                Label tb = new Label();
                tb.Anchor =
                    ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                            | System.Windows.Forms.AnchorStyles.Left)
                            | System.Windows.Forms.AnchorStyles.Right)));
                tb.Margin = new System.Windows.Forms.Padding(0);
                return tb;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("Error: " + ex.Message);
                return null;
            }
        }

        protected ComboBox CreateInventoryComboBox()
        {
            try
            {
                ComboBox tb = new ComboBox();
                tb.Anchor =
                    ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top
                            | System.Windows.Forms.AnchorStyles.Bottom)
                            | System.Windows.Forms.AnchorStyles.Left)
                            | System.Windows.Forms.AnchorStyles.Right)));
                tb.Margin = new System.Windows.Forms.Padding(0);
                return tb;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("Error: " + ex.Message);
                return null;
            }
        }
        protected void ClearControls()
        {
            try
            {
                txtItemNoBarcode.Text = "";
                txtRefNo.Text = "";
                lblGST.Text = "";
                //invCtrl.SetInventoryLocation(defaultLoc);
                //if filename is specified, mark it as deleted....
                if (filename != null && filename != "")
                {
                    Query qr = SavedFile.CreateQuery();
                    qr.QueryType = QueryType.Update;
                    qr.AddWhere(SavedFile.Columns.SaveName, filename);
                    qr.AddUpdateSetting(SavedFile.Columns.Deleted, true);
                    qr.Execute();
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        protected void RemoveUnticked(out string status, out PurchaseOrderDetCollection tmpDetCol)
        {
            try
            {
                status = "";
                ArrayList ar = new ArrayList();
                //loop through and delete....
                for (int i = 0; i < dgvStock.Rows.Count; i++)
                {
                    if (dgvStock.Rows[i].Cells[0].Value == null ||
                        dgvStock.Rows[i].Cells[0].Value.ToString().ToLower() == "false")
                    {
                        ar.Add(dgvStock.Rows[i].Cells["PurchaseOrderDetRefNo"].Value);

                    }
                }

                tmpDetCol = new PurchaseOrderDetCollection();
                for (int i = 0; i < ar.Count; i++)
                {
                    tmpDetCol.Add(invCtrl.GetPurchaseOrderDet(ar[i].ToString()));
                    invCtrl.DeleteFromPurchaseOrderDetail(ar[i].ToString(), out status);
                }

                for (int i = 0; i < tmpDetCol.Count; i++)
                {
                    tmpDetCol[i].Deleted = false;
                }
            }
            catch (Exception ex)
            {
                status = ex.Message;
                tmpDetCol = null;
                Logger.writeLog(ex);
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        protected bool IsLineItemEmpty()
        {
            try
            {
                if (tbControl.SelectedIndex == 0 && invCtrl.GetNumberOfLineItem() == 0)
                {
                    tbControl.SelectedIndex = 1;
                    tbControl.Refresh();
                    return true;
                }
                //check if any is checked...
                for (int i = 0; i < dgvStock.Rows.Count; i++)
                {
                    if (dgvStock.Rows[i].Cells[0].Value != null
                        && dgvStock.Rows[i].Cells[0].Value.ToString().ToLower() == "true")
                    {
                        return false; //found at least 1 item check....
                    }
                }
                return true;// meaning none is checked....
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("Error: " + ex.Message);
                return false;
            }
        }
        protected void btnDeleteChecked_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dr = MessageBox.Show("Are you sure you want to delete?", "Warning", MessageBoxButtons.YesNo);
                if (dr == DialogResult.No)
                {
                    return;
                }

                ArrayList ar = new ArrayList();

                //loop through and delete....
                for (int i = 0; i < dgvStock.Rows.Count; i++)
                {
                    if (dgvStock.Rows[i].Cells[0].Value != null &&
                        dgvStock.Rows[i].Cells[0].Value.ToString().ToLower() == "true")
                    {
                        ar.Add(dgvStock.Rows[i].Cells["PurchaseOrderDetRefNo"].Value);

                    }
                }
                string status;
                for (int i = 0; i < ar.Count; i++)
                {
                    invCtrl.DeleteFromPurchaseOrderDetail(ar[i].ToString(), out status);
                }
                BindGrid();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void tbBody_Enter(object sender, EventArgs e)
        {
            txtItemNoBarcode.Select();
        }

        private void dgvStock_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string status;

                //Check Quantity column
                if (e.RowIndex >= 0)
                {
                    if (e.ColumnIndex == 0)
                    {
                        //Mark delete/undelete
                        //load value...        
                        bool value;
                        if (dgvStock.Rows[e.RowIndex].Cells[0].Value != null
                            && dgvStock.Rows[e.RowIndex].Cells[0].Value is bool)
                        {
                            value = !(bool)dgvStock.Rows[e.RowIndex].Cells[0].Value;
                        }
                        else
                        {
                            value = true;
                        }

                        invCtrl.MarkAsDeletedFromPurchaseOrderDetail
                            (dgvStock.Rows[e.RowIndex].Cells["PurchaseOrderDetRefNo"].Value.ToString(),
                            value,
                            out status);
                        dgvStock.Refresh();

                    }
                    if (dgvStock.Columns[e.ColumnIndex].Name == Quantity.Name) //previously index 4
                    {
                        frmKeypad f = new frmKeypad();
                        f.IsInteger = true;
                        f.initialValue = dgvStock.Rows[e.RowIndex].Cells["Quantity"].Value.ToString();
                        CommonUILib.displayTransparent(); f.ShowDialog(); CommonUILib.hideTransparent();
                        int tmp;
                        if (f.value != "" && Int32.TryParse(f.value, out tmp))
                        {
                            if (invCtrl.ChangeItemQty(dgvStock.Rows[e.RowIndex].Cells["PurchaseOrderDetRefNo"].Value.ToString(), tmp, out status))
                            {
                                if (!invCtrl.ValidateQuantity(dgvStock.Rows[e.RowIndex].Cells["PurchaseOrderDetRefNo"].Value.ToString() ))
                                {
                                    MessageBox.Show("The Order Quantity is not multiply of packing size quantity. Please change accordingly.");
                                }

                                f.Dispose();
                                int firstRowIndex = dgvStock.FirstDisplayedCell.RowIndex;
                                BindGrid();
                                dgvStock.FirstDisplayedCell = dgvStock.Rows[firstRowIndex].Cells[0];
                            }
                            else
                            {
                                f.Dispose();
                                MessageBox.Show("Error. " + status);
                            }
                        }
                    }
                    //click cost price column
                    else if (dgvStock.Columns[e.ColumnIndex].Name == FactoryPrice.Name) //previously index 5
                    {
                        if (showCostPrice && isCostPriceEditable)
                        {

                            frmKeypad f = new frmKeypad();
                            f.IsInteger = false;
                            decimal val = decimal.Parse(dgvStock.Rows[e.RowIndex].Cells["FactoryPrice"].Value.ToString());
                            f.initialValue = val.ToString("N2");
                            CommonUILib.displayTransparent(); f.ShowDialog(); CommonUILib.hideTransparent();

                            decimal tmp;

                            if (f.value != "" && Decimal.TryParse(f.value, out tmp))
                            {
                                if (invCtrl.ChangeFactoryPrice(dgvStock.Rows[e.RowIndex].Cells["PurchaseOrderDetRefNo"].Value.ToString(), tmp, int.Parse(cmbGST.SelectedValue.ToString()), out status))
                                {
                                    f.Dispose();
                                    int firstRowIndex = dgvStock.FirstDisplayedCell.RowIndex;
                                    BindGrid();
                                    dgvStock.FirstDisplayedCell = dgvStock.Rows[firstRowIndex].Cells[0];
                                }
                                else
                                {
                                    f.Dispose();
                                    MessageBox.Show("Error. " + status);
                                }
                            }
                        }
                    }
                    //click retail price column
                    /*else if (dgvStock.Columns[e.ColumnIndex].Name == RetailPrice.Name) //previously index 5
                    {
                        if (isSellPriceEditable)
                        {

                            frmKeypad f = new frmKeypad();
                            f.IsInteger = false;
                            f.initialValue = dgvStock.Rows[e.RowIndex].Cells["RetailPrice"].Value.ToString();
                            CommonUILib.displayTransparent(); f.ShowDialog(); CommonUILib.hideTransparent();

                            decimal tmp;

                            if (f.value != "" && Decimal.TryParse(f.value, out tmp))
                            {
                                if (invCtrl.ChangeRetailPrice(dgvStock.Rows[e.RowIndex].Cells["PurchaseOrderDetRefNo"].Value.ToString(), tmp, out status))
                                {
                                    f.Dispose();
                                    int firstRowIndex = dgvStock.FirstDisplayedCell.RowIndex;
                                    BindGrid();
                                    dgvStock.FirstDisplayedCell = dgvStock.Rows[firstRowIndex].Cells[0];
                                }
                                else
                                {
                                    f.Dispose();
                                    MessageBox.Show("Error. " + status);
                                }
                            }
                        }
                    }*/
                    //click remark column
                    else if (dgvStock.Columns[e.ColumnIndex].Name == Remark.Name) //previously index 7
                    {
                        frmRemark f = new frmRemark();
                        f.txtRemark.Text = dgvStock.Rows[e.RowIndex].Cells["Remark"].Value.ToString();
                        CommonUILib.displayTransparent(); f.ShowDialog(); CommonUILib.hideTransparent();

                        if (f.IsSuccessful)
                        {
                            if (invCtrl.ChangeRemark(dgvStock.Rows[e.RowIndex].Cells["PurchaseOrderDetRefNo"].Value.ToString(), f.txtRemark.Text, out status))
                            {
                                f.Dispose();
                                int firstRowIndex = dgvStock.FirstDisplayedCell.RowIndex;
                                BindGrid();
                                dgvStock.FirstDisplayedCell = dgvStock.Rows[firstRowIndex].Cells[0];
                            }
                            else
                            {
                                f.Dispose();
                                MessageBox.Show("Error. " + status);
                            }
                        }
                    }
                    else if (dgvStock.Columns[e.ColumnIndex].Name == ChoosePackingSize.Name) //previously index 7
                    {
                        //Load the data
                        frmChoosePackageSize frmc = new frmChoosePackageSize();
                        frmc.itemNo = dgvStock.Rows[e.RowIndex].Cells["ItemNo"].Value.ToString();
                        frmc.supplierName = cmbSupplier.Text;
                        if (frmc.ShowDialog() == DialogResult.OK)
                        {
                            //
                            if (frmc.packageSize != "No Packing")
                            {
                                invCtrl.ChangePackageSize(dgvStock.Rows[e.RowIndex].Cells["PurchaseOrderDetRefNo"].Value.ToString(), frmc.packageSize, out status);
                                invCtrl.ChangeFactoryPrice(dgvStock.Rows[e.RowIndex].Cells["PurchaseOrderDetRefNo"].Value.ToString(), frmc.costPrice / frmc.packingSizeUOm, 0, out status);
                                invCtrl.ChangePackingSizeUOM(dgvStock.Rows[e.RowIndex].Cells["PurchaseOrderDetRefNo"].Value.ToString(), frmc.packingSizeUOm, out status);
                                invCtrl.ChangeCostPerPackingSize(dgvStock.Rows[e.RowIndex].Cells["PurchaseOrderDetRefNo"].Value.ToString(), frmc.costPrice, out status);
                                if (!invCtrl.ValidateQuantity(dgvStock.Rows[e.RowIndex].Cells["PurchaseOrderDetRefNo"].Value.ToString(), frmc.packingSizeUOm))
                                {
                                    MessageBox.Show("The Order Quantity is not multiply of packing size quantity. Please change accordingly.");
                                }
                            }
                            else
                            {
                                invCtrl.ChangePackageSize(dgvStock.Rows[e.RowIndex].Cells["PurchaseOrderDetRefNo"].Value.ToString(), "", out status);
                                invCtrl.ResetFactoryPrice(dgvStock.Rows[e.RowIndex].Cells["PurchaseOrderDetRefNo"].Value.ToString(), out status);
                                invCtrl.ChangePackingSizeUOM(dgvStock.Rows[e.RowIndex].Cells["PurchaseOrderDetRefNo"].Value.ToString(), 1, out status);
                            }
                            
                        }
                        frmc.Dispose();
                        BindGrid();
                    }
                    else if (dgvStock.Columns[e.ColumnIndex].Name == CostPerPackingSize.Name) //previously index 5
                    {
                        if (showCostPrice && isCostPerPackingSizeEditable)
                        {

                            frmKeypad f = new frmKeypad();
                            f.IsInteger = false;
                            f.initialValue = dgvStock.Rows[e.RowIndex].Cells["CostPerPackingSize"].Value.ToString();
                            CommonUILib.displayTransparent(); f.ShowDialog(); CommonUILib.hideTransparent();

                            decimal tmp;

                            if (f.value != "" && Decimal.TryParse(f.value, out tmp))
                            {
                                if (invCtrl.ChangeCostPerPackingSize(dgvStock.Rows[e.RowIndex].Cells["PurchaseOrderDetRefNo"].Value.ToString(), tmp, out status))
                                {
                                    f.Dispose();
                                    int firstRowIndex = dgvStock.FirstDisplayedCell.RowIndex;
                                    BindGrid();
                                    dgvStock.FirstDisplayedCell = dgvStock.Rows[firstRowIndex].Cells[0];
                                }
                                else
                                {
                                    f.Dispose();
                                    MessageBox.Show("Error. " + status);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void dtpInventoryDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                invCtrl.SetPurchaseOrderDate(dtpInventoryDate.Value);
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dr = MessageBox.Show("Are you sure you want to cancel?", "", MessageBoxButtons.YesNo);
                if (dr == DialogResult.No)
                {
                    return;
                }
                this.Close();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void cmbLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                invCtrl.SetInventoryLocation(((InventoryLocation)cmbLocation.SelectedValue).InventoryLocationID);
                InventoryLocation il = new InventoryLocation(((InventoryLocation)cmbLocation.SelectedValue).InventoryLocationID);
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowDeliveryAddress), false))
                {
                    string tmpAddress = (String.IsNullOrEmpty(il.Userfld2) ? "" : il.Userfld2 + "~") +
                        (String.IsNullOrEmpty(il.Userfld3) ? "" : il.Userfld3 + "~") + (String.IsNullOrEmpty(il.Userfld4) ? "" : il.Userfld4 + "~") 
                        + (String.IsNullOrEmpty(il.Userfld5) ? "" : il.Userfld5 + "~") + (String.IsNullOrEmpty(il.Userfld6) ? "" : il.Userfld6 + " ") 
                        + (String.IsNullOrEmpty(il.Userfld7) ? "" : il.Userfld7);
                    txtDeliveryAddress.Text = tmpAddress.Replace("~", Environment.NewLine);
                }
                BindGrid();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnSaveToDisk_Click(object sender, EventArgs e)
        {
            try
            {
                string status;

                //prompt remark
                frmRemark f = new frmRemark();
                f.comment = "Enter Remark for the saved file";
                f.defaultValue = this.Text;
                CommonUILib.displayTransparent(); f.ShowDialog(); CommonUILib.hideTransparent();
                bool IsSuccessful = f.IsSuccessful;
                string remark = f.txtRemark.Text;
                f.Dispose();

                //Pop out Textbox to save file name
                this.saveRemark = remark;
                status = SaveToDisk(false);

                MessageBox.Show(status);
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        protected string SaveToDisk(bool autosave)
        {
            try
            {

                string status;
                //save header info
                invCtrl.SetPurchaseOrderDate(dtpInventoryDate.Value);
                invCtrl.SetRemark(lblGST.Text);

                //Save all possible additional information - 
                //use find control...
                string purchaseOrderNo = ((TextBox)this.Controls.Find("txtRefNo", true)[0]).Text;
                string Supplier = "";//((TextBox)this.Controls.Find("Supplier", true)[0]).Text;
                decimal FreightCharges;
                decimal Discount;
                double ExchangeRate;

                invCtrl.SetPurchaseOrder(purchaseOrderNo);
                invCtrl.SetSupplier(Supplier);
                //if (txtExchangeRate.Text == "") txtExchangeRate.Text = "0";
                //if (txtFreightCharges.Text == "") txtFreightCharges.Text = "0";
                //if (txtDiscount.Text == "") txtDiscount.Text = "0";



                SavedItemController s = new SavedItemController();
                if (!Directory.Exists(Application.StartupPath + "\\" + SavedFolder + "\\"))
                {
                    Directory.CreateDirectory(Application.StartupPath + "\\" + SavedFolder + "\\");
                }
                if (invCtrl.getUniqueID() == Guid.Empty)
                {
                    invCtrl.createNewGUID();
                }
                if (s.SaveObject
                    (invCtrl, invCtrl.getUniqueID().ToString(), this.Text,
                    Application.StartupPath + "\\" + SavedFolder + "\\", saveRemark, autosave, out status))
                {
                    status = "Save successful";
                    //MessageBox.Show();
                }
                else
                {
                    status = "Error encountered during saving. Please contact your system administrator. Error: " + status;
                }
                return status;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("Error: " + ex.Message);
                return ex.Message;
            }
        }

        protected virtual void dgvStock_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            try
            {
                {
                    if ((int)dgvStock.Rows[e.RowIndex].Cells["Quantity"].Value < 0 ||
                        ((this is frmStockOut || this is frmStockTransfer) &&
                        (int)dgvStock.Rows[e.RowIndex].Cells["Quantity"].Value >
                        (int)dgvStock.Rows[e.RowIndex].Cells["OnHand"].Value))
                    {
                        dgvStock.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.DarkRed;
                        dgvStock.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.White;
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                if (invCtrl.GetNumberOfLineItem() == 0)
                {
                    MessageBox.Show("There is no item to be printed.");
                    return;
                }
                ShowPanelPleaseWait();
                string PurchaseOrderNo;
                string SupplierName = string.Empty;
                string FreightCharges;
                string Discount;
                string ExchangeRate;
                //PurchaseOrderNo = ((TextBox)this.Controls.Find("txtPurchaseOrder", true)[0]).Text;
                if ((int)cmbSupplier.SelectedValue != -1)
                {
                    SupplierName = ((ComboBox)this.Controls.Find("cmbSupplier", true)[0]).Text;
                }
                FreightCharges = "0";
                Discount = "0";
                ExchangeRate = "1";
                string StockActivity = this.Text;
                frmPurchaseOrder.PrintPurchaseOrderSheet
                    (invCtrl, SupplierName, FreightCharges, Discount,
                    ExchangeRate, showOnHandQty, showCostPrice);
                pnlLoading.Visible = false;
                this.Refresh();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("Error encounter." + ex.Message);
            }
        }

        private void llSelectAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                string status;
                for (int i = 0; i < dgvStock.Rows.Count; i++)
                {
                    dgvStock.Rows[i].Cells[0].Value = true;
                    //invCtrl.MarkAsDeletedFromPurchDetail
                    //        (dgvStock.Rows[i].Cells["InventoryDetRefNo"].Value.ToString(),
                    //        true, out status);
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void llSelectNone_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                string status;
                for (int i = 0; i < dgvStock.Rows.Count; i++)
                {
                    dgvStock.Rows[i].Cells[0].Value = false;
                    /*invCtrl.MarkAsDeletedFromInventoryDetail
                            (dgvStock.Rows[i].Cells["InventoryDetRefNo"].Value.ToString(),
                            false, out status);*/
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void llInvert_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                string status;
                for (int i = 0; i < dgvStock.Rows.Count; i++)
                {
                    if (dgvStock.Rows[i].Cells[0].Value != null
                        && dgvStock.Rows[i].Cells[0].Value.ToString().ToLower() == "true")
                    {
                        dgvStock.Rows[i].Cells[0].Value = false;
                        /*invCtrl.MarkAsDeletedFromInventoryDetail
                            (dgvStock.Rows[i].Cells["InventoryDetRefNo"].Value.ToString(),
                            false, out status);*/
                    }
                    else
                    {
                        dgvStock.Rows[i].Cells[0].Value = true;
                        /*invCtrl.MarkAsDeletedFromInventoryDetail
                            (dgvStock.Rows[i].Cells["InventoryDetRefNo"].Value.ToString(),
                            true, out status);*/
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            CommonUILib.displayTransparent(); saveFileDialogExport.ShowDialog(); CommonUILib.hideTransparent();
        }

        private void saveFileDialogExport_FileOk(object sender, CancelEventArgs e)
        {
            try
            {
                ExportController.ExportToExcel(dgvStock, saveFileDialogExport.FileName);
                MessageBox.Show("Save successful.");
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("This will import data collector text file into the system. The format of the data collector file is <barcode>,<quantity>,<scanned date time>");
            //CommonUILib.displayTransparent();
            //openFileDialog1.FileName = "";
            //openFileDialog1.ShowDialog();
            //CommonUILib.hideTransparent();
            try
            {
                string correctItemNo;
                ArrayList correctItemNoList = null;
                ArrayList correctItemNames = null;
                ArrayList correctDescriptions = null;
                correctItemNoList = new ArrayList();
                correctItemNames = new ArrayList();
                correctDescriptions = new ArrayList();
                ItemCollection ic = new ItemCollection();
                ic.Load();

                foreach (Item i in ic)
                {
                    if (ItemController.IsLowQuantityItem(i.ItemNo, ((InventoryLocation)(cmbLocation.SelectedValue)).InventoryLocationID, 0))
                    {

                        correctItemNoList.Add(i.ItemNo);
                        correctItemNames.Add(i.ItemName);
                        correctDescriptions.Add(i.ItemDesc);
                    }
                }

                if (correctItemNoList.Count > 0)
                {
                    frmMultipleQtyEntry fQty = new frmMultipleQtyEntry();
                    fQty.itemNos = correctItemNoList;
                    fQty.itemNames = correctItemNames;
                    fQty.descriptions = correctDescriptions;
                    CommonUILib.displayTransparent(); fQty.ShowDialog(); CommonUILib.hideTransparent();
                    pnlLoading.Visible = false;
                    /*pop up keypad 
                    frmKeypad f = new frmKeypad();
                    f.IsInteger = true;
                    CommonUILib.displayTransparent();f.ShowDialog();CommonUILib.hideTransparent();
                    int qty;*/
                    string status;

                    if (fQty.IsSuccessful)
                    {
                        for (int i = correctItemNoList.Count - 1; i >= 0; i--)
                        {
                            //AddItem to Inventory
                            if (!invCtrl.AddItemIntoInventory(correctItemNoList[i].ToString(), (int)fQty.ht[correctItemNoList[i].ToString()], cmbSupplier.SelectedValue.ToString(), 0, out status))
                            {
                                MessageBox.Show("Error: " + status);
                            }
                        }
                        BindGrid();
                        txtItemNoBarcode.Text = "";
                        txtItemNoBarcode.Focus();
                    }
                }
                else
                {
                    MessageBox.Show("No Low Quantity Item");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            try
            {
                DataTable message = null;
                DataTable ErrorDb;
                //DataSet ds = null;

                FileInfo inf = new FileInfo(openFileDialog1.FileName);
                if (inf.Extension.ToLower() == ".csv")
                {
                    if (!ExcelController.ImportExcelCSV(openFileDialog1.FileName, out message, true))
                        throw new Exception("");
                }
                else if (inf.Extension.ToLower() == ".xls")
                {
                    if (!ExcelController.ImportExcelXLS(openFileDialog1.FileName, out message, true))
                        throw new Exception("");
                }
                else if (inf.Extension.ToLower() == ".txt")
                {
                    if (!ExcelController.ImportExcelCSV(openFileDialog1.FileName, out message, false))
                        throw new Exception("");

                    for (int Counter = 0; Counter < message.Columns.Count; Counter++)
                    {
                        string Value = AppSetting.GetSetting("DataCollector_Column" + Counter.ToString("N0"));
                        if (Value != null && Value != "")
                        {
                            message.Columns[Counter].ColumnName = Value;
                        }
                        else if (Counter == 0)
                        { message.Columns[Counter].ColumnName = "Barcode"; }
                        else if (Counter == 1)
                        { message.Columns[Counter].ColumnName = "Qty"; }
                    }
                }

                if (message == null || message.Rows.Count < 1)
                    throw new Exception("No data inside");

                if (invCtrl.ImportFromDataTable(message, out ErrorDb))
                {
                    MessageBox.Show("Load successful.");
                }
                else
                {
                    //MessageBox.Show(message);
                    frmImportErrorMessage f = new frmImportErrorMessage();
                    f.source = ErrorDb;
                    CommonUILib.displayTransparent(); f.ShowDialog(); CommonUILib.hideTransparent();
                }
                BindGrid();

                #region -= Old Code - Without Column Header =-
                ////Read barcode and quantity....
                //DataTable message;
                //if (invCtrl.ImportFromCSVTextFile(openFileDialog1.FileName, out message))
                //{
                //    MessageBox.Show("Load successful.");
                //}
                //else
                //{
                //    //MessageBox.Show(message);
                //    frmImportErrorMessage f = new frmImportErrorMessage();
                //    f.source = message;
                //    CommonUILib.displayTransparent();f.ShowDialog();CommonUILib.hideTransparent();
                //}
                //BindGrid();
                #endregion
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("Error. Load FAILED!." + ex.Message);
            }
        }

        private void dgvStock_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    if (e.ColumnIndex == 0)
                    {
                        //Mark delete/undelete
                        //load value...
                        bool value;
                        string status;
                        if (dgvStock.Rows[e.RowIndex].Cells[0].Value != null
                            && dgvStock.Rows[e.RowIndex].Cells[0].Value is bool)
                        {
                            value = !(bool)dgvStock.Rows[e.RowIndex].Cells[0].Value;
                        }
                        else
                        {
                            value = true;
                        }

                        /*invCtrl.MarkAsDeletedFromInventoryDetail
                            (dgvStock.Rows[e.RowIndex].Cells["InventoryDetRefNo"].Value.ToString(),
                            value,
                            out status);*/
                        dgvStock.Refresh();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        protected void RestoreConnectionString()
        {
            //DataService.GetInstance("PowerPOS").DefaultConnectionString = existingConnectionString;
        }
        private void frmInventoryParent_FormClosed(object sender, FormClosedEventArgs e)
        {
            //RestoreConnectionString();
        }

        private void bgSearch_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //download inventoryhdr and inventorydet
                SyncClientController.Load_WS_URL();
                bool result = SyncClientController.GetCurrentInventory();
                if (result)
                    result = SyncClientController.UpdateOrderDetFromDownloadedInventoryHdr();
                result = result && SyncClientController.GenerateInventoryHdrForAdjustedSales();
                e.Result = result;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);

                // if FK conflict then throw error
                if (ex.GetType().ToString() == "System.Data.SqlClient.SqlException")
                {
                    if (((System.Data.SqlClient.SqlException)ex).Number == 547)
                    {
                        MessageBox.Show("You need to download latest Items from Server, please go to Setup Menu and Sync.");
                    }
                }

                e.Result = false;
            }
        }

        private void bgSearch_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                pnlProgress.Visible = false;
                isTherePendingStockTake = false;
                if (StockTakeController.IsThereUnAdjustedStockTake() &&
                    this.Text.ToUpper() != "STOCK TAKE")
                {
                    isTherePendingStockTake = true;
                    MessageBox.Show("There is an unadjusted stock take. No inventory movement is allowed! Please adjust stock take first");
                    this.Close();

                }
                else
                {
                    this.Enabled = true;
                    if (!(bool)e.Result)
                    {
                        MessageBox.Show("Error loading inventory from the web. Please check your internet connection.");
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void bgSearch_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pgb1.PerformStep();
        }

        private void btnDownloadTemplate_Click(object sender, EventArgs e)
        {
            frmExportTemplate fExportMenu = new frmExportTemplate();
            fExportMenu.ShowDialog();
            fExportMenu.Dispose();
        }

        private void btnSetting_Click(object sender, EventArgs e)
        {
            frmInventorySettings Stgs = new frmInventorySettings();
            Stgs.ShowDialog();
            Stgs.Dispose();
        }

        public static void PrintPurchaseOrderSheet
            (PurchaseOdrController invController,
            string SupplierName, string FreightCharges, string Discount,
             string ExchangeRate, bool displayStockOnHand, bool displayCostPrice)
        {
            try
            {
                frmViewPurchaseOrderSheet f = new frmViewPurchaseOrderSheet();
                f.invCtrl = invController;
                f.showCostPrice = displayCostPrice;
                f.showOnHand = displayStockOnHand;
                //f.StockActivity = StockActivity;
                //use find control...
                PrintOutParameters1 printOutParameter = new PrintOutParameters1();
                //printOutParameter.UserField1Label = "Purchase Order No";
                //printOutParameter.UserField1Value = PurchaseOrderNo;
                //if (printOutParameter.UserField1Value == "") printOutParameter.UserField1Value = "-";
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

                try
                {
                    Supplier supp = new Supplier(Supplier.Columns.SupplierName, SupplierName);

                    printOutParameter.UserField2Label = "Supplier";
                    printOutParameter.UserField2Value = supp.SupplierName;

                    printOutParameter.UserField7Label = "Supplier Address";
                    printOutParameter.UserField7Value = string.IsNullOrEmpty(supp.CustomerAddress) ? "-" : supp.CustomerAddress;

                    printOutParameter.UserField8Label = "Telp Number";
                    printOutParameter.UserField8Value = string.IsNullOrEmpty(supp.ContactNo1) ? "-" : supp.ContactNo1;

                    printOutParameter.UserField9Label = "Fax Number";
                    printOutParameter.UserField9Value = string.IsNullOrEmpty(supp.ContactNo3) ? "-" : supp.ContactNo3;

                    printOutParameter.UserField10Label = "Delivery Time";
                    printOutParameter.UserField10Value = string.IsNullOrEmpty(invController.getDeliveryTimeFormatted()) ? "-" : invController.getDeliveryTimeFormatted();

                    printOutParameter.UserField11Label = "Delivery Address";
                    printOutParameter.UserField11Value = string.IsNullOrEmpty(invController.getDeliveryAddress()) ? "-" : invController.getDeliveryAddress();

                    printOutParameter.UserField12Label = "Payment Term";
                    printOutParameter.UserField12Value = string.IsNullOrEmpty(invController.getPaymentTerm()) ? "-" : invController.getPaymentTerm();

                    printOutParameter.UserField13Label = "Receiving Time";
                    printOutParameter.UserField13Value = string.IsNullOrEmpty(invController.getReceivingTime()) ? "-" : invController.getReceivingTime();

                    printOutParameter.UserField14Label = "User Name";
                    printOutParameter.UserField14Value = string.IsNullOrEmpty(UserInfo.username) ? "-" : UserInfo.username;

                    printOutParameter.UserField15Label = "PO Role";
                    printOutParameter.UserField15Value = string.IsNullOrEmpty(invController.InvHdr.Userfld8) ? "-" : invController.InvHdr.Userfld8;

                    printOutParameter.UserField16Label = "PO Company";
                    printOutParameter.UserField16Value = string.IsNullOrEmpty(invController.InvHdr.Userfld9) ? "-" : invController.InvHdr.Userfld9;

                    printOutParameter.UserField17Label = "Status";
                    printOutParameter.UserField17Value = string.IsNullOrEmpty(invController.InvHdr.Userfld7) ? "Submitted" : invController.InvHdr.Userfld7;

                    printOutParameter.UserField18Label = "GST";
                    printOutParameter.UserField18Value = string.IsNullOrEmpty(invController.InvHdr.Userfld10) ? "" : invController.InvHdr.Userfld10;

                    printOutParameter.UserField19Label = "Min Purchase";
                    printOutParameter.UserField19Value = invController.InvHdr.Userfloat1.HasValue ? invController.InvHdr.Userfloat1.ToString() : "0";

                    printOutParameter.UserField20Label = "Delivery Charge";
                    printOutParameter.UserField20Value = invController.InvHdr.Userfloat2.HasValue ? invController.InvHdr.Userfloat2.ToString() : "0";

                    printOutParameter.SupplierAddress2 = string.IsNullOrEmpty(supp.ShipToAddress) ? "-" : supp.ShipToAddress;
                    printOutParameter.SupplierAddress3 = string.IsNullOrEmpty(supp.BillToAddress) ? "-" : supp.BillToAddress;
                    printOutParameter.ContactPerson1 = string.IsNullOrEmpty(supp.ContactPerson1) ? "-" : supp.ContactPerson1;
                    printOutParameter.ContactPerson2 = string.IsNullOrEmpty(supp.ContactPerson2) ? "-" : supp.ContactPerson2;
                    printOutParameter.ContactPerson3 = string.IsNullOrEmpty(supp.ContactPerson3) ? "-" : supp.ContactPerson3;
                    printOutParameter.SupplierCode = string.IsNullOrEmpty(supp.SupplierCode) ? "-" : supp.SupplierCode;
                    printOutParameter.SupplierEmail = string.IsNullOrEmpty(supp.ContactNo2) ? "-" : supp.ContactNo2;
                }
                catch (Exception ex)
                {
                    Logger.writeLog(ex);
                }

                f.printOutParameters = printOutParameter;
                CommonUILib.displayTransparent(); f.ShowDialog(); CommonUILib.hideTransparent();
                f.Dispose();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("Error encounter." + ex.Message);
            }
        }

        private void btnImport1_Click(object sender, EventArgs e)
        {
            CommonUILib.displayTransparent();
            openFileDialog1.FileName = "";
            openFileDialog1.ShowDialog();
            CommonUILib.hideTransparent();
        }

        private void tblInventory_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dgvStock_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void cmbSupplier_SelectedValueChanged(object sender, EventArgs e)
        {
            int tmp = 0;
            if (int.TryParse(cmbSupplier.SelectedValue.ToString(), out tmp))
            {
                /*SyncClientController.Load_WS_URL();
                PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                ws.Url = SyncClientController.WS_URL;

                string sTemp = new Supplier(tmp);*/

                Supplier s = new Supplier(tmp);
                if (s != null && s.SupplierID > -1)
                {
                    if (s.Userint1.HasValue)
                        cmbGST.SelectedValue = s.Userint1;
                    if (s.Userfld1 != null)
                        txtPaymentTerm.Text = s.Userfld1;
                    if (s.Userfld2 != null && AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowCurrency), false))
                    {
                        PowerPOS.Currency cr = new PowerPOS.Currency("CurrencyCode", s.Userfld2);
                        if (cr!= null && cr.CurrencyCode != "")
                            cmbCurrencies.SelectedValue = cr.CurrencyId;
                    }
                    if (s.Userfloat1.HasValue && AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowMinPurchase), false))
                        txtMinPurchase.Text = s.Userfloat1.ToString();
                    if (s.Userfloat2.HasValue && AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowDeliveryCharge), false))
                        txtDeliveryCharge.Text = s.Userfloat2.ToString();
                }
            }
            BindGrid();
        }

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void lblOrderAmount_Click(object sender, EventArgs e)
        {

        }

        private void txtDeliveryCharge_Validated(object sender, EventArgs e)
        {
            BindGrid();
        }

        private void txtMinPurchase_Validated(object sender, EventArgs e)
        {
            BindGrid();
        }

        private void cmbGST_SelectedValueChanged(object sender, EventArgs e)
        {
            BindGrid();
        }

        private void btnMembership_Click(object sender, EventArgs e)
        {
            frmAddMember f = new frmAddMember();
            f.searchReq = "";
            f.ShowDialog();
            string tmpMembershipNo = f.membershipNo;
            if (tmpMembershipNo != "")
            {
                Membership mbr = new Membership(tmpMembershipNo);
                if (mbr != null && mbr.MembershipNo == tmpMembershipNo)
                {
                    string address = "";
                    address += mbr.NameToAppear + Environment.NewLine;
                    address += mbr.StreetName + Environment.NewLine +
                               mbr.StreetName2 + Environment.NewLine +
                               mbr.Country + " " + mbr.ZipCode + Environment.NewLine;
                    address += (string.IsNullOrEmpty(mbr.Mobile) ? "" : "Mobile: " + mbr.Mobile) + Environment.NewLine;
                    address += (string.IsNullOrEmpty(mbr.Home) ? "" : "Phone: " + mbr.Home);
                    address = address.Replace(Environment.NewLine + Environment.NewLine, Environment.NewLine);

                    txtDeliveryAddress.Text = address;
                }
            }
            f.Dispose();
        }

        private void GetCustomPONoSettingFromServer()
        {
            try
            {
                SyncClientController.Load_WS_URL();
                PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                ws.Timeout = 100000;
                ws.Url = SyncClientController.WS_URL;
                DataTable dt = ws.GetCustomPONoSetting();
                if (dt != null)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        AppSetting.SetSetting(dr["key"].ToString(), dr["value"].ToString());
                    }
                    // Now the Custom No will always be generated in server
                    AppSetting.SetSetting(AppSetting.SettingsName.PurchaseOrder.GenerateCustomNoInServer, AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.UseCustomNo));
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("Failed to get Ref No from server. Error: " + ex.Message);
            }
        }
    }
}