using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Linq;
using PowerInventory.OrderForms;
using PowerPOS;
using PowerPOS.Container;
using SubSonic;
using LanguageManager = PowerInventory.Properties.InventoryLanguage;
using System.Collections.Generic;

namespace PowerInventory
{
    public partial class frmInventoryParent : Form
    {
        private string existingConnectionString;
        protected bool showCostPrice;
        protected bool showRetailPrice;
        protected bool canUpdateRetailPrice;
        protected bool showOnHandQty;
        private string filename;
        protected string saveRemark;
        private const string SavedFolder = "SAVED";
        protected const bool printAfterConfirm = true;
        protected bool isTherePendingStockTake;
        protected bool ChangeCostPriceStkAdjOut;
        private bool IsItemPictureShown = false;
		protected bool ENABLE_PRODUCT_SERIAL_NO = false;

        public void setFileName(string tFileName, string tSavedRemark)
        {
            filename = tFileName;
            saveRemark = tSavedRemark;
        }

        protected InventoryController invCtrl;

        public frmInventoryParent()
        {
            Program.LoadCultureCode();
            try
            {
                existingConnectionString = DataService.GetInstance("POWERPOS").DefaultConnectionString;
                if (this is frmStockTransfer)
                    showCostPrice = !(AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.HideCostInStockTransfer), false));
                else
                    showCostPrice = !(AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.HideInventoryCost), false));

                showRetailPrice = !(AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.HideRetailPrice), false));
                showOnHandQty = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.ShowBalanceQuantityOnTransaction), false);
                ENABLE_PRODUCT_SERIAL_NO = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.EnableProductSerialNo), false);

                ChangeCostPriceStkAdjOut = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.ChangePriceStockAdjIssue), false);

                if (this is frmStockIn)
                    canUpdateRetailPrice = (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.AllowToUpdateRetailPriceInGoodsReceive), false));
                else
                    canUpdateRetailPrice = false;

                InitializeComponent();
                dgvStock.AutoGenerateColumns = false;

                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.AllowChangeInventoryDate), false))
                    dtpInventoryDate.Enabled = true;

                //show total Cost Price (for Robert MiniMart only so far)
                if (this is frmStockIn || this is frmStockOutReturn)
                {
                    if (showCostPrice)
                    {
                        lblTotalCostPrice.Visible = true;
                        lblTotalCostPriceAmount.Visible = true;
                    }
                    else
                    {
                        lblTotalCostPrice.Visible = false;
                        lblTotalCostPriceAmount.Visible = false;
                    }
                }

                cmbCriteria.Items.Add("Contains");
                cmbCriteria.Items.Add("Starts With");
                cmbCriteria.Items.Add("Ends With");
                cmbCriteria.Items.Add("Exact Match");
                cmbCriteria.SelectedIndex = 0;

                IsItemPictureShown = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.AddItemPicture), false);
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show(LanguageManager.Error_encounter_ + ex.Message);
            }
        }

        protected void ShowPanelPleaseWait()
        {
            try
            {
                pnlLoading.Parent = this;
                pnlLoading.Location = new Point(
                    ClientSize.Width / 2 - pnlLoading.Size.Width / 2,
                    ClientSize.Height / 2 - pnlLoading.Size.Height / 2);
                pnlLoading.Anchor = AnchorStyles.None;
                pnlLoading.BringToFront();
                pnlLoading.Visible = true;
                Refresh();
            }
            catch (Exception ex)
            {
                pnlLoading.Visible = false;
                Logger.writeLog(ex);
                MessageBox.Show(LanguageManager.Error_encounter_ + ex.Message);
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
                    invCtrl = new InventoryController(InventorySettings.CostingMethod);
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
                txtRefNo.Text = InventoryController.getNewInventoryRefNo(PointOfSaleInfo.InventoryLocationID);

                _currentPage = 0;
                BindGrid();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show(LanguageManager.Error_encounter_ + ex.Message);
            }
        }

        protected void LoadInventoryController()
        {
            try
            {
                int selIndex = 0;
                if (filename != null && filename != "")
                {
                    //DB ACCESS
                    object obj = null;
                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.SaveFileInTheServer), false))
                    {
                        obj = SyncClientController.GetLoadedInventoryFile(filename);
                    }
                    else
                    {
                        SavedItemController s = new SavedItemController();
                        obj = s.LoadObject(filename, Application.StartupPath + "\\" + SavedFolder + "\\");
                    }
                    if (obj != null)
                    {
                        invCtrl = (InventoryController)obj;
                        txtRemark.Text = invCtrl.GetRemark();
                        //Change the inventorydate to Datetime Now
                        invCtrl.SetInventoryDate(DateTime.Now);
                        dtpInventoryDate.Value = invCtrl.GetInventoryDate();
                        selIndex = invCtrl.InvHdr.InventoryLocationID.GetValueOrDefault(0);
                        //assign according to form type
                        //Save all possible additional information - 
                        if (this is frmStockIn)
                        {
                            //use find control...
                            if (Controls.Find("txtPurchaseOrder", true).Length > 0)
                                (Controls.Find("txtPurchaseOrder", true)[0]).Text = invCtrl.getPurchaseOrderNo();
                            if (Controls.Find("cmbSupplier", true).Length > 0 && invCtrl.getSupplier() != null)
                                ((ComboBox)Controls.Find("cmbSupplier", true)[0]).SelectedValue = invCtrl.getSupplier();
                            if (Controls.Find("txtFreight", true).Length > 0)
                                (Controls.Find("txtFreight", true)[0]).Text = invCtrl.GetFreightCharges().ToString();
                            if (Controls.Find("txtDiscount", true).Length > 0)
                                (Controls.Find("txtDiscount", true)[0]).Text = invCtrl.getDiscount().ToString();
                            if (Controls.Find("txtExchangeRate", true).Length > 0)
                                (Controls.Find("txtExchangeRate", true)[0]).Text = invCtrl.getExchangeRate().ToString();
                            if (Controls.Find("txtVendorInvoiceNo", true).Length > 0)
                                (Controls.Find("txtVendorInvoiceNo", true)[0]).Text = String.IsNullOrEmpty(invCtrl.getVendorInvoiceNo()) ? "" : invCtrl.getVendorInvoiceNo().ToString();
                            if (Controls.Find("cmbGST", true).Length > 0)
                                ((ComboBox)(Controls.Find("cmbGST", true)[0])).SelectedValue = invCtrl.GetGSTRule().ToString();

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
                        }
                        else if (this is frmStockOut)
                        {
                            int stockOutReasonID = invCtrl.getStockOutReasonID();
                            ComboBox cmb = ((ComboBox)Controls.Find("cmbStockOutReason", true)[0]);
                            for (int i = 0; i < cmb.Items.Count; i++)
                            {
                                if (((InventoryStockOutReason)(cmb.Items[i])).ReasonID == stockOutReasonID)
                                {
                                    cmb.SelectedIndex = i;
                                    break;
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
                        }
                        else if (this is frmStockTransfer)
                        {
                            int transferDestinationID = invCtrl.getTransferDestination();
                            ComboBox cmb = ((ComboBox)Controls.Find("cmbDestination", true)[0]);
                            for (int i = 0; i < cmb.Items.Count; i++)
                            {
                                if (((InventoryLocation)(cmb.Items[i])).InventoryLocationID == transferDestinationID)
                                {
                                    cmb.SelectedIndex = i;
                                    break;
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
                        }
                        else if (this is frmAdjustStock)
                        {
                            int adjustmentdirection = invCtrl.getStockAdjustmentDirection();
                            ComboBox cmb = ((ComboBox)Controls.Find("cmbAdjustDirection", true)[0]);
                            cmb.SelectedIndex = adjustmentdirection;
                            cmb.Refresh();
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
                        }
                        else if (this is frmStockTake)
                        {
                            string takenBy = "", verifiedBy = "";

                            invCtrl.GetInventoryStockTakeTakenBy(out takenBy, out verifiedBy);
                            TextBox txtTakenBy = ((TextBox)Controls.Find("txtTakenBy", true)[0]);
                            TextBox txtVerifiedBy = ((TextBox)Controls.Find("txtVerifiedBy", true)[0]);
                            if (takenBy != null)
                                txtTakenBy.Text = takenBy;
                            if (verifiedBy != null)
                                txtVerifiedBy.Text = verifiedBy;
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
                        }
                        else if (this is frmStockOutReturn)
                        {
                            //use find control...
                            if (Controls.Find("cmbSupplier", true).Length > 0 && invCtrl.getSupplier() != null)
                                ((ComboBox)Controls.Find("cmbSupplier", true)[0]).SelectedValue = invCtrl.getSupplier();

                            if (Controls.Find("txtCreditNoteNo", true).Length > 0)
                                (Controls.Find("txtCreditNoteNo", true)[0]).Text = String.IsNullOrEmpty(invCtrl.getVendorInvoiceNo()) ? "" : invCtrl.getVendorInvoiceNo().ToString();


                            int stockOutReasonID = invCtrl.getStockOutReasonID();
                            ComboBox cmb = ((ComboBox)Controls.Find("cmbStockOutReason", true)[0]);
                            for (int i = 0; i < cmb.Items.Count; i++)
                            {
                                if (((InventoryStockOutReason)(cmb.Items[i])).ReasonID == stockOutReasonID)
                                {
                                    cmb.SelectedIndex = i;
                                    break;
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
                        }
                    }
                }
                InitiateInventoryController();
                for (int i = 0; i < cmbLocation.Items.Count; i++)
                {
                    if (((InventoryLocation)(cmbLocation.Items[i])).InventoryLocationID == selIndex)
                        cmbLocation.SelectedIndex = i;
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show(LanguageManager.Error_encounter_ + ex.Message);
            }
        }

        public virtual void btnScanItemNo_Click(object sender, EventArgs e)
        {
            try
            {
                #region *) OBSOLETE : no need to check pending stock take when adding item.
                //if (!(this is frmStockTake))
                //{
                //    if (!PointOfSaleInfo.IntegrateWithInventory)
                //    {
                //        ShowPanelPleaseWait();
                //        SyncClientController.Load_WS_URL();
                //        PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                //        ws.Timeout = 100000;
                //        ws.Url = SyncClientController.WS_URL;
                //        if (ws.IsThereUnAdjustedStockTake(((InventoryLocation)cmbLocation.SelectedValue).InventoryLocationID) && Text.ToUpper() != "FRMSTOCKTAKE")
                //        {
                //            isTherePendingStockTake = true;
                //            MessageBox.Show(
                //                "There is an unadjusted stock take for this location. No inventory movement is allowed! Please adjust stock take first");
                //            pnlLoading.Visible = false;
                //            return;

                //        }
                //        pnlLoading.Visible = false;
                //    }
                //    else
                //    {
                //        if (StockTakeController.IsThereUnAdjustedStockTake(((InventoryLocation)cmbLocation.SelectedValue).InventoryLocationID) && Text.ToUpper() != "FRMSTOCKTAKE")
                //        {
                //            isTherePendingStockTake = true;
                //            MessageBox.Show(
                //                "There is an unadjusted stock take for this location. No inventory movement is allowed! Please adjust stock take first");
                //            return;
                //        }
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
                                        if (this is frmStockIn)
                                        {
                                            if (!invCtrl.AddItemIntoInventoryStockIn(det[z].ItemNo.ToString(), multiplier * (int)det[z].UnitQty, out status))
                                            {
                                                MessageBox.Show("Error: " + status);
                                            }
                                        }
                                        else if (this is frmStockOutReturn)
                                        {
                                            if (!invCtrl.AddItemIntoInventoryStockReturn(det[z].ItemNo.ToString(), multiplier * (int)det[z].UnitQty, ChangeCostPriceStkAdjOut && (this is frmAdjustStock || this is frmStockOut), out status))
                                            {
                                                MessageBox.Show("Error: " + status);
                                            }
                                        }
                                        else
                                        {
                                            //if (this is frmStockTransfer || this is frmAdjustStock || this is frmStockTake || this is frmStockTake)
                                            //{
                                            //    //decimal qtyTemp = 0;
                                            //    if (!invCtrl.AddItemIntoInventoryForSales(det[z].ItemNo.ToString(), multiplier * (int)det[z].UnitQty
                                            //        , ItemSummaryController.GetAvgCostPrice(det[z].ItemNo.ToString(), invCtrl.GetInventoryLocationID())
                                            //        , out status))
                                            //    {
                                            //        MessageBox.Show("Error Adding Item: " + status);
                                            //    }
                                            //}
                                            //else
                                            {
                                                if (!invCtrl.AddItemIntoInventoryUsingAltCost(det[z].ItemNo.ToString(), multiplier * (int)det[z].UnitQty, ChangeCostPriceStkAdjOut && (this is frmAdjustStock || this is frmStockOut), out status))
                                                {
                                                    MessageBox.Show("Error: " + status);
                                                }
                                            }
                                        }
                                    }
                                }
                                BindGrid();
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
                                    //AddItem to Inventory
                                    //if (this is frmStockTransfer || this is frmAdjustStock || this is frmStockTake)
                                    //{
                                    //    decimal qtyTemp =0;
                                    //    if (!decimal.TryParse(qtylist[i].ToString(),out qtyTemp))
                                    //        MessageBox.Show("Error Adding Item. Please check your quantity");
                                    //    if (!invCtrl.AddItemIntoInventoryForSales(itemnolist[i].ToString(), qtyTemp
                                    //        , ItemSummaryController.GetAvgCostPrice(itemnolist[i].ToString(), invCtrl.GetInventoryLocationID())
                                    //        , out status))
                                    //    {
                                    //        MessageBox.Show("Error Adding Item: " + status);
                                    //    }
                                    //}
                                    //else
                                    if (this is frmStockOutReturn)
                                    {
                                        if (!invCtrl.AddItemIntoInventoryStockReturn(itemnolist[i].ToString(), (int)qtylist[i], ChangeCostPriceStkAdjOut && (this is frmAdjustStock || this is frmStockOut), out status))
                                        {
                                            MessageBox.Show("Error: " + status);
                                        }
                                    }
                                    else
                                    {
                                        if (!invCtrl.AddItemIntoInventoryUsingAltCost(itemnolist[i].ToString(), (int)qtylist[i], ChangeCostPriceStkAdjOut && (this is frmAdjustStock || this is frmStockOut), out status))
                                        {
                                            MessageBox.Show("Error: " + status);
                                        }
                                    }
                                }
                                BindGrid();
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
                                                    if (this is frmStockIn)
                                                    {
                                                        if (!invCtrl.AddItemIntoInventoryStockIn(igm.ItemNo, (int)fAddItemGroup.Qty * igm.UnitQty, out status))
                                                        {
                                                            MessageBox.Show("Error: " + status);
                                                        }
                                                    }
                                                    else if (this is frmStockOutReturn)
                                                    {
                                                        if (!invCtrl.AddItemIntoInventoryStockReturn(igm.ItemNo, (int)fAddItemGroup.Qty * igm.UnitQty, ChangeCostPriceStkAdjOut && (this is frmAdjustStock || this is frmStockOut), out status))
                                                        {
                                                            MessageBox.Show("Error: " + status);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        //if (this is frmStockTransfer || this is frmAdjustStock || this is frmStockTake)
                                                        //{
                                                        //    decimal qtyTemp = 0;
                                                        //    /*if (!decimal.TryParse(fQty.ht[correctItemNoList[i].ToString()].ToString(), out qtyTemp))
                                                        //        MessageBox.Show("Error Adding Item. Please check your quantity");*/
                                                        //    if (!invCtrl.AddItemIntoInventoryForSales(igm.ItemNo, (int)fAddItemGroup.Qty * igm.UnitQty
                                                        //        , ItemSummaryController.GetAvgCostPrice(igm.ItemNo, invCtrl.GetInventoryLocationID())
                                                        //        , out status))
                                                        //    {
                                                        //        MessageBox.Show("Error Adding Item: " + status);
                                                        //    }
                                                        //}
                                                        //else
                                                        if (!invCtrl.AddItemIntoInventoryUsingAltCost(igm.ItemNo, (int)fAddItemGroup.Qty * igm.UnitQty, ChangeCostPriceStkAdjOut && (this is frmAdjustStock || this is frmStockOut), out status))
                                                        {
                                                            MessageBox.Show("Error: " + status);
                                                        }
                                                    }
                                                }

                                            }
                                            BindGrid();
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
                                                            if (this is frmStockIn)
                                                            {
                                                                if (!invCtrl.AddItemIntoInventoryStockIn(igm.ItemNo, (decimal)fQty.ht[correctItemNoList[i].ToString()] * igm.UnitQty, out status))
                                                                {
                                                                    MessageBox.Show("Error: " + status);
                                                                }
                                                            }
                                                            else if (this is frmStockOutReturn)
                                                            {
                                                                if (!invCtrl.AddItemIntoInventoryStockReturn(igm.ItemNo, (decimal)fQty.ht[correctItemNoList[i].ToString()] * igm.UnitQty, ChangeCostPriceStkAdjOut && (this is frmAdjustStock || this is frmStockOut), out status))
                                                                {
                                                                    MessageBox.Show("Error: " + status);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                //if (this is frmStockTransfer || this is frmAdjustStock || this is frmStockTake)
                                                                //{
                                                                //    decimal qtyTemp = 0;
                                                                //    if (!decimal.TryParse(fQty.ht[correctItemNoList[i].ToString()].ToString(), out qtyTemp))
                                                                //        MessageBox.Show("Error Adding Item. Please check your quantity");
                                                                //    if (!invCtrl.AddItemIntoInventoryForSales(igm.ItemNo.ToString(), qtyTemp * igm.UnitQty
                                                                //        , ItemSummaryController.GetAvgCostPrice(igm.ItemNo, invCtrl.GetInventoryLocationID())
                                                                //        , out status))
                                                                //    {
                                                                //        MessageBox.Show("Error Adding Item: " + status);
                                                                //    }
                                                                //}
                                                                //else
                                                                if (!invCtrl.AddItemIntoInventoryUsingAltCost(igm.ItemNo, (decimal)fQty.ht[correctItemNoList[i].ToString()] * igm.UnitQty, ChangeCostPriceStkAdjOut && (this is frmAdjustStock || this is frmStockOut), out status))
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
                                                if (this is frmStockIn)
                                                {
                                                    if (!invCtrl.AddItemIntoInventoryStockIn(correctItemNoList[i].ToString(), (decimal)fQty.ht[correctItemNoList[i].ToString()],
                                                                                  out status))
                                                    {
                                                        MessageBox.Show("Error: " + status);
                                                    }
                                                }
                                                else if (this is frmStockOutReturn)
                                                {
                                                    if (!invCtrl.AddItemIntoInventoryStockReturn(correctItemNoList[i].ToString(), (decimal)fQty.ht[correctItemNoList[i].ToString()],
                                                                                      ChangeCostPriceStkAdjOut && (this is frmAdjustStock || this is frmStockOut), out status))
                                                    {
                                                        MessageBox.Show("Error: " + status);
                                                    }
                                                }
                                                else
                                                {
                                                    //if (this is frmStockTransfer || this is frmAdjustStock || this is frmStockTake)
                                                    //{
                                                    //    decimal qtyTemp = 0;
                                                    //    if (!decimal.TryParse(fQty.ht[correctItemNoList[i].ToString()].ToString(), out qtyTemp))
                                                    //        MessageBox.Show("Error Adding Item. Please check your quantity");
                                                    //    if (!invCtrl.AddItemIntoInventoryForSales(correctItemNoList[i].ToString(), qtyTemp
                                                    //        , ItemSummaryController.GetAvgCostPrice(correctItemNoList[i].ToString(), invCtrl.GetInventoryLocationID())
                                                    //        , out status))
                                                    //    {
                                                    //        MessageBox.Show("Error Adding Item: " + status);
                                                    //    }
                                                    //}
                                                    //else
                                                    if (!invCtrl.AddItemIntoInventoryUsingAltCost(correctItemNoList[i].ToString(), (decimal)fQty.ht[correctItemNoList[i].ToString()],
                                                                                      ChangeCostPriceStkAdjOut && (this is frmAdjustStock || this is frmStockOut), out status))
                                                    {
                                                        MessageBox.Show("Error: " + status);
                                                    }
                                                }
                                            }
                                        }
                                        BindGrid();
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
                                                        if (this is frmStockIn)
                                                        {
                                                            if (!invCtrl.AddItemIntoInventoryStockIn(igm.ItemNo, defaultQty * igm.UnitQty, out status))
                                                            {
                                                                MessageBox.Show("Error: " + status);
                                                            }
                                                        }
                                                        else if (this is frmStockOutReturn)
                                                        {
                                                            if (!invCtrl.AddItemIntoInventoryStockReturn(igm.ItemNo, defaultQty * igm.UnitQty, ChangeCostPriceStkAdjOut && (this is frmAdjustStock || this is frmStockOut), out status))
                                                            {
                                                                MessageBox.Show("Error: " + status);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            //if (this is frmStockTransfer || this is frmAdjustStock || this is frmStockTake)
                                                            //{
                                                            //    decimal qtyTemp = 0;
                                                            //    /*if (!decimal.TryParse(fQty.ht[correctItemNoList[i].ToString()].ToString(), out qtyTemp))
                                                            //        MessageBox.Show("Error Adding Item. Please check your quantity");*/
                                                            //    if (!invCtrl.AddItemIntoInventoryForSales(igm.ItemNo, defaultQty * igm.UnitQty
                                                            //        , ItemSummaryController.GetAvgCostPrice(igm.ItemNo, invCtrl.GetInventoryLocationID())
                                                            //        , out status))
                                                            //    {
                                                            //        MessageBox.Show("Error Adding Item: " + status);
                                                            //    }
                                                            //}
                                                            //else
                                                            if (!invCtrl.AddItemIntoInventoryUsingAltCost(igm.ItemNo, defaultQty * igm.UnitQty, ChangeCostPriceStkAdjOut && (this is frmAdjustStock || this is frmStockOut), out status))
                                                            {
                                                                MessageBox.Show("Error: " + status);
                                                            }
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
                                            if (this is frmStockIn)
                                            {
                                                if (!invCtrl.AddItemIntoInventoryStockIn(correctItemNoList[i].ToString(), defaultQty, out status))
                                                {
                                                    MessageBox.Show("Error: " + status);
                                                }
                                            }
                                            else if (this is frmStockOutReturn)
                                            {
                                                if (!invCtrl.AddItemIntoInventoryStockReturn(correctItemNoList[i].ToString(), defaultQty, ChangeCostPriceStkAdjOut && (this is frmAdjustStock || this is frmStockOut), out status))
                                                {
                                                    MessageBox.Show("Error: " + status);
                                                }
                                            }
                                            else
                                            {
                                                //if (this is frmStockTransfer || this is frmAdjustStock || this is frmStockTake)
                                                //{
                                                //    decimal qtyTemp = 0;
                                                //    /*if (!decimal.TryParse(fQty.ht[correctItemNoList[i].ToString()].ToString(), out qtyTemp))
                                                //        MessageBox.Show("Error Adding Item. Please check your quantity");*/
                                                //    if (!invCtrl.AddItemIntoInventoryForSales(correctItemNoList[i].ToString(), defaultQty 
                                                //        , ItemSummaryController.GetAvgCostPrice(correctItemNoList[i].ToString(), invCtrl.GetInventoryLocationID())
                                                //        , out status))
                                                //    {
                                                //        MessageBox.Show("Error Adding Item: " + status);
                                                //    }
                                                //}
                                                //else
                                                if (!invCtrl.AddItemIntoInventoryUsingAltCost(correctItemNoList[i].ToString(), defaultQty, ChangeCostPriceStkAdjOut && (this is frmAdjustStock || this is frmStockOut), out status))
                                                {
                                                    MessageBox.Show("Error: " + status);
                                                }
                                            }
                                        }
                                    }
                                    BindGrid();
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

        protected int _currentPage = 0;
        protected const int _pageSize = 100;
        protected int _totalPages = 1;

        protected void BindGrid()
        {
            try
            {
                string status;

                if (invCtrl == null) return;

                _totalPages = (int)Math.Ceiling(invCtrl.GetNumberOfLineItem() / (float)_pageSize);

                if (_currentPage < 0)
                    _currentPage = 0;

                if (_currentPage >= _totalPages)
                    _currentPage = 0;

                btnPage.Text = string.Format("Page {0} of {1}", _currentPage + 1, _totalPages);
                btnLastPage.Enabled = btnNextPage.Enabled = _currentPage < (_totalPages - 1);
                btnFirstPage.Enabled = btnPreviousPage.Enabled = _currentPage > 0;
                ShowPanelPleaseWait();

                if (this is frmStockIn)
                {
                    invCtrl.CalculateAdditionalCost();
                    if (!invCtrl.SetGST(invCtrl.InvHdr.GSTRule,true, out status))
                    {

                        MessageBox.Show(string.Format("Error: {0}", status));
                        return;

                    }                    
                }

                if (this is frmStockOutReturn)
                {
                    if (!invCtrl.SetGST(invCtrl.InvHdr.GSTRule,false, out status))
                    {

                        MessageBox.Show(string.Format("Error: {0}", status));
                        return;
                    }
                }

                //populate order items
                DataTable dt = invCtrl.FetchUnSavedInventoryItems(showOnHandQty, showCostPrice, out status, _currentPage, _pageSize, ChangeCostPriceStkAdjOut && (this is frmAdjustStock || this is frmStockOut));
                dgvStock.DataSource = dt;
                lblTotalItem.Text = invCtrl.GetNumberOfLineItem().ToString("N0");
                lblTotalQuantity.Text = invCtrl.GetTotalQuantity().ToString("0.####");
                if (dt.Rows.Count > 0)
                    dgvStock.FirstDisplayedScrollingRowIndex = dgvStock.RowCount - 1;
                if (status != "")
                {
                    MessageBox.Show(string.Format("Error: {0}", status));
                    return;
                }

                if (this is frmStockTake)
                    dgvStock.Columns["CategoryName"].Visible = true;

                if ((this is frmStockOutReturn && showCostPrice) || (this is frmStockIn && showCostPrice))
                {
                    //dt.Columns.Add("TotalCostPrice", Type.GetType("System.Decimal"));

                    decimal totalCostPrice = 0;
                    //populate total cost price column 
                    for (int i = 0; i < dgvStock.Rows.Count; i++)
                    {
                        //dt.Rows[i]["TotalCostPrice"] = (decimal)((decimal)dgvStock.Rows[i].Cells["CalcFactoryPrice"].Value * (int)dgvStock.Rows[i].Cells["Quantity"].Value);

                        totalCostPrice += (decimal)dt.Rows[i]["TotalCostPrice"];
                        //dgvStock.Rows[i].Cells["TotalCostPrice"].Value = (decimal)((decimal)dgvStock.Rows[i].Cells["CalcFactoryPrice"].Value * (int)dgvStock.Rows[i].Cells["Quantity"].Value);
                        //totalCostPrice += (decimal)dgvStock.Rows[i].Cells["TotalCostPrice"].Value;
                    }
                    lblTotalCostPriceAmount.Text = totalCostPrice.ToString("N");
                }
                //lblTotalCostPriceAmount.Text = string.Format("${0}", invCtrl.GetTotalCostOfGoods().ToString("N2"));

                dgvStock.Refresh();
                pnlLoading.Visible = false;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("An unknown error has been encountered. Please contact your system administrator.");
                txtItemNoBarcode.Focus();
                pnlLoading.Visible = false;
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
                MessageBox.Show(LanguageManager.Error_encounter_ + ex.Message);
            }
        }

        private void frmInventoryParent_Load(object sender, EventArgs e)
        {
            try
            {
                existingConnectionString = DataService.GetInstance("POWERPOS").DefaultConnectionString;

                if (!PointOfSaleInfo.IntegrateWithInventory)
                {
                    pnlProgress.Parent = this;
                    pnlProgress.BringToFront();
                    pnlProgress.Visible = true;
                    Enabled = false;
                    bgSearch.RunWorkerAsync();
                }
                else
                {
                    isTherePendingStockTake = false;
                    /*if (StockTakeController.IsThereUnAdjustedStockTake() && Text.ToUpper() != "FRMSTOCKTAKE")
                    {
                        isTherePendingStockTake = true;
                        MessageBox.Show(
                            "There is an unadjusted stock take. No inventory movement is allowed! Please adjust stock take first");
                        Close();
                    }*/
                }
				SerialNo.Visible = ENABLE_PRODUCT_SERIAL_NO;
                DataGridViewImageColumn pictureColumn = new DataGridViewImageColumn();
                pictureColumn.Name = "Photo";
                pictureColumn.Width = 60;
                pictureColumn.Visible = true;
                dgvStock.Columns.Insert(2, pictureColumn);

                if (!IsItemPictureShown)
                {
                    pictureColumn.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show(LanguageManager.Error_encounter_ + ex.Message);
            }
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
                    dgvStock.Columns["FactoryPrice"].Visible = false;
                    dgvStock.Columns["CalcFactoryPrice"].Visible = false;
                    dgvStock.Columns["TotalCostPrice"].Visible = false;
                }

                if (ChangeCostPriceStkAdjOut && (this is frmAdjustStock || this is frmStockOut))
                {
                    dgvStock.Columns["AlternateCostPrice"].Visible = true;
                    dgvStock.Columns["FactoryPrice"].Visible = false;
                    dgvStock.Columns["CalcFactoryPrice"].Visible = true;
                    dgvStock.Columns["TotalCostPrice"].Visible = true;
                }
                else
                {
                    dgvStock.Columns["AlternateCostPrice"].Visible = false;
                }
                if (!AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.ShowUOM), false))
                {
                    dgvStock.Columns["UOM"].Visible = false;
                }
                if (!AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.ShowCurrency), false))
                {
                    dgvStock.Columns["Currency"].Visible = false;
                }

                dgvStock.Columns["Discount"].Visible = (this is frmStockIn);

                dgvStock.Refresh();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                Close();
            }
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            //pop up a new form to ask for new item....
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
        }

        protected TextBox CreateInventoryTextBox()
        {
            try
            {
                TextBox tb = new TextBox();
                tb.Anchor =
                    (((((AnchorStyles.Top | AnchorStyles.Bottom)
                        | AnchorStyles.Left)
                       | AnchorStyles.Right)));
                tb.Margin = new Padding(0);
                return tb;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show(LanguageManager.Error_encounter_ + ex.Message);
                return null;
            }
        }

        protected Label CreateInventoryLabel()
        {
            try
            {
                Label tb = new Label();
                tb.Anchor =
                    (((((AnchorStyles.Top | AnchorStyles.Bottom)
                        | AnchorStyles.Left)
                       | AnchorStyles.Right)));
                tb.Margin = new Padding(0);
                return tb;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show(LanguageManager.Error_encounter_ + ex.Message);
                return null;
            }
        }

        protected ComboBox CreateInventoryComboBox()
        {
            try
            {
                ComboBox tb = new ComboBox();
                tb.Anchor =
                    (((((AnchorStyles.Top
                         | AnchorStyles.Bottom)
                        | AnchorStyles.Left)
                       | AnchorStyles.Right)));
                tb.Margin = new Padding(0);
                return tb;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show(LanguageManager.Error_encounter_ + ex.Message);
                return null;
            }
        }

        protected void ClearControls()
        {
            try
            {
                txtItemNoBarcode.Text = "";
                txtRefNo.Text = "";
                txtRemark.Text = "";
                dtpInventoryDate.Value = DateTime.Now;
                //invCtrl.SetInventoryLocation(defaultLoc);
                //if filename is specified, mark it as deleted....
                if (filename != null && filename != "")
                {
                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.SaveFileInTheServer), false))
                    {
                        SyncClientController.removeSavedFile(filename);
                        filename = "";
                    }
                    else
                    {
                        Query qr = SavedFile.CreateQuery();
                        qr.QueryType = QueryType.Update;
                        qr.AddWhere(SavedFile.Columns.SaveName, filename);
                        qr.AddUpdateSetting(SavedFile.Columns.Deleted, true);
                        qr.Execute();
                        filename = "";
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show(LanguageManager.Error_encounter_ + ex.Message);
            }
        }

        protected void RemoveUnticked(out string status, out InventoryDetCollection tmpDetCol)
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
                        ar.Add(dgvStock.Rows[i].Cells["InventoryDetRefNo"].Value);
                    }
                }

                tmpDetCol = new InventoryDetCollection();
                for (int i = 0; i < ar.Count; i++)
                {
                    tmpDetCol.Add(invCtrl.GetInvDetClone(ar[i].ToString()));
                    invCtrl.DeleteFromInventoryDetail(ar[i].ToString(), out status);
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
                MessageBox.Show(LanguageManager.Error_encounter_ + ex.Message);
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
                return true; // meaning none is checked....
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show(LanguageManager.Error_encounter_ + ex.Message);
                return false;
            }
        }

        protected void btnDeleteChecked_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dr = MessageBox.Show(LanguageManager.Are_you_sure_you_want_to_delete_, "Warning",
                                                  MessageBoxButtons.YesNo);
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
                        ar.Add(dgvStock.Rows[i].Cells["InventoryDetRefNo"].Value);
                    }
                }
                string status;
                for (int i = 0; i < ar.Count; i++)
                {
                    invCtrl.DeleteFromInventoryDetail(ar[i].ToString(), out status);
                }
                BindGrid();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show(LanguageManager.Error_encounter_ + ex.Message);
            }
        }

        private void tbBody_Enter(object sender, EventArgs e)
        {
            txtItemNoBarcode.Select();
        }

        protected virtual void dgvStock_CellClick(object sender, DataGridViewCellEventArgs e)
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
                        if (dgvStock.Rows[e.RowIndex].Cells[0].Value is bool)
                        {
                            value = !(bool)dgvStock.Rows[e.RowIndex].Cells[0].Value;
                        }
                        else
                        {
                            value = true;
                        }

                        invCtrl.MarkAsDeletedFromInventoryDetail
                            (dgvStock.Rows[e.RowIndex].Cells["InventoryDetRefNo"].Value.ToString(),
                             value,
                             out status);
                        dgvStock.Refresh();
                    }
                    if (dgvStock.Columns[e.ColumnIndex].Name == Quantity.Name) //previously index 4
                    {
                        frmKeypad f = new frmKeypad();
                        f.IsInteger = false;
                        f.initialValue = dgvStock.Rows[e.RowIndex].Cells["Quantity"].Value.ToString();
                        CommonUILib.displayTransparent();
                        f.ShowDialog();
                        CommonUILib.hideTransparent();
                        decimal tmp;
                        if (f.value != "" && Decimal.TryParse(f.value, out tmp))
                        {
                            if (this is frmStockIn)
                            {
                                if (invCtrl.ChangeItemQtyStockIn(dgvStock.Rows[e.RowIndex].Cells["InventoryDetRefNo"].Value.ToString(), tmp, out status))
                                {
                                    f.Dispose();
                                    int firstRowIndex = dgvStock.FirstDisplayedCell.RowIndex;
                                    BindGrid();
                                    dgvStock.FirstDisplayedCell = dgvStock.Rows[firstRowIndex].Cells[0];
                                }
                                else
                                {
                                    f.Dispose();
                                    MessageBox.Show(LanguageManager.Error_encounter_ + status);
                                }
                            }
                            else
                            {
                                if (invCtrl.ChangeItemQty(dgvStock.Rows[e.RowIndex].Cells["InventoryDetRefNo"].Value.ToString(), tmp, out status))
                                {
                                    f.Dispose();
                                    int firstRowIndex = dgvStock.FirstDisplayedCell.RowIndex;
                                    BindGrid();
                                    dgvStock.FirstDisplayedCell = dgvStock.Rows[firstRowIndex].Cells[0];
                                }
                                else
                                {
                                    f.Dispose();
                                    MessageBox.Show(LanguageManager.Error_encounter_ + status);
                                }
                            }
                        }
                    }
                    //click cost price column
                    else if (dgvStock.Columns[e.ColumnIndex].Name == FactoryPrice.Name) //previously index 5
                    {
                        if (showCostPrice && !(this is frmAdjustStock || this is frmStockTransfer))
                        {
                            frmKeypad f = new frmKeypad();
                            f.IsInteger = false;
                            f.initialValue = dgvStock.Rows[e.RowIndex].Cells["FactoryPrice"].Value.ToString();
                            CommonUILib.displayTransparent();
                            f.ShowDialog();
                            CommonUILib.hideTransparent();

                            decimal tmp;

                            if (f.value != "" && Decimal.TryParse(f.value, out tmp))
                            {
                                if (invCtrl.ChangeFactoryPrice(dgvStock.Rows[e.RowIndex].Cells["InventoryDetRefNo"].Value.ToString(), tmp,
                                                               out status))
                                {
                                    f.Dispose();
                                    int firstRowIndex = dgvStock.FirstDisplayedCell.RowIndex;
                                    BindGrid();
                                    dgvStock.FirstDisplayedCell = dgvStock.Rows[firstRowIndex].Cells[0];
                                }
                                else
                                {
                                    f.Dispose();
                                    MessageBox.Show(LanguageManager.Error_encounter_ + status);
                                }
                            }
                        }
                    }

                    else if (dgvStock.Columns[e.ColumnIndex].Name == AlternateCostPrice.Name)
                    {
                        if (ChangeCostPriceStkAdjOut && (this is frmAdjustStock || this is frmStockOut))
                        {
                            frmKeypad f = new frmKeypad();
                            f.IsInteger = false;
                            f.initialValue = dgvStock.Rows[e.RowIndex].Cells["AlternateCostPrice"].Value.ToString();
                            CommonUILib.displayTransparent();
                            f.ShowDialog();
                            CommonUILib.hideTransparent();

                            decimal tmp;

                            if (f.value != "" && Decimal.TryParse(f.value, out tmp))
                            {
                                if (invCtrl.ChangeAlternateCostPrice(dgvStock.Rows[e.RowIndex].Cells["InventoryDetRefNo"].Value.ToString(), tmp,
                                                               out status))
                                {
                                    f.Dispose();
                                    int firstRowIndex = dgvStock.FirstDisplayedCell.RowIndex;
                                    BindGrid();
                                    dgvStock.FirstDisplayedCell = dgvStock.Rows[firstRowIndex].Cells[0];
                                }
                                else
                                {
                                    f.Dispose();
                                    MessageBox.Show(LanguageManager.Error_encounter_ + status);
                                }
                            }
                        }
                    }
                    //click remark column
                    else if (dgvStock.Columns[e.ColumnIndex].Name == Remark.Name) //previously index 7
                    {
                        frmRemark f = new frmRemark();
                        f.txtRemark.Text = dgvStock.Rows[e.RowIndex].Cells["Remark"].Value.ToString();
                        CommonUILib.displayTransparent();
                        f.ShowDialog();
                        CommonUILib.hideTransparent();

                        if (f.IsSuccessful)
                        {
                            if (invCtrl.ChangeRemark(dgvStock.Rows[e.RowIndex].Cells["InventoryDetRefNo"].Value.ToString(), f.txtRemark.Text,
                                                     out status))
                            {
                                f.Dispose();
                                int firstRowIndex = dgvStock.FirstDisplayedCell.RowIndex;
                                BindGrid();
                                dgvStock.FirstDisplayedCell = dgvStock.Rows[firstRowIndex].Cells[0];
                            }
                            else
                            {
                                f.Dispose();
                                MessageBox.Show(LanguageManager.Error_encounter_ + status);
                            }
                        }
                    }
                    //click remark column
                    else if (dgvStock.Columns[e.ColumnIndex].Name == TotalCostPrice.Name) //previously index 7
                    {
                        if (this is frmStockIn)
                        {
                            frmKeypad f = new frmKeypad();
                            f.IsInteger = false;
                            f.initialValue = dgvStock.Rows[e.RowIndex].Cells["TotalCostPrice"].Value.ToString();
                            CommonUILib.displayTransparent();
                            f.ShowDialog();
                            CommonUILib.hideTransparent();

                            decimal tmp;

                            if (f.value != "" && Decimal.TryParse(f.value, out tmp))
                            {
                                if (invCtrl.ChangeTotalCostPrice(dgvStock.Rows[e.RowIndex].Cells["InventoryDetRefNo"].Value.ToString(), tmp,
                                                               out status))
                                {
                                    f.Dispose();
                                    int firstRowIndex = dgvStock.FirstDisplayedCell.RowIndex;
                                    BindGrid();
                                    dgvStock.FirstDisplayedCell = dgvStock.Rows[firstRowIndex].Cells[0];
                                }
                                else
                                {
                                    f.Dispose();
                                    MessageBox.Show(LanguageManager.Error_encounter_ + status);
                                }
                            }
                        }
                    }
					else if (dgvStock.Columns[e.ColumnIndex].Name == SerialNo.Name)
                    {
                        string detRefNo = dgvStock.Rows[e.RowIndex].Cells["InventoryDetRefNo"].Value.ToString();
                        var invDet = invCtrl.InvDet.Where(o => o.InventoryDetRefNo.IsEqual(detRefNo)).FirstOrDefault();
                        if (invDet == null)
                            return;

                        int inventoryLocationID = invCtrl.InvHdr.InventoryLocationID.GetValueOrDefault(0);
                        if (inventoryLocationID == 0)
                        {
                            MessageBox.Show("Please select Inventory Location");
                            return;
                        }


                        var validationMode = ValidationMode.CHECK_IS_EXIST;
                        if (this is frmStockIn)
                            validationMode = ValidationMode.CHECK_IS_NOT_EXIST;
                        bool validateSerialNo = true;
                        if (this is frmStockTake)
                            validateSerialNo = false;

                        frmSerialNo frmSerial = new frmSerialNo(invDet.SerialNo, invDet.Item, (int)invDet.Quantity.GetValueOrDefault(0), validateSerialNo, validationMode, inventoryLocationID);
                        if (frmSerial.ShowDialog() == DialogResult.OK)
                        {
                            string sts = "";
                            if((int)invDet.Quantity.GetValueOrDefault(0) != frmSerial.Qty)
                                invCtrl.ChangeItemQty(detRefNo, frmSerial.Qty, out sts);
                            invDet.SerialNo = frmSerial.SerialNo;
                            BindGrid();
                        }
                    }
                    //click retail price column
                    else if (dgvStock.Columns[e.ColumnIndex].Name == RetailPrice.Name)
                    {
                        if (canUpdateRetailPrice)
                        {
                            frmKeypad f = new frmKeypad();
                            f.IsInteger = false;
                            f.initialValue = dgvStock.Rows[e.RowIndex].Cells["RetailPrice"].Value.ToString();
                            CommonUILib.displayTransparent();
                            f.ShowDialog();
                            CommonUILib.hideTransparent();

                            decimal tmp;

                            if (f.value != "" && Decimal.TryParse(f.value, out tmp))
                            {
                                if (invCtrl.ChangeRetailPrice(dgvStock.Rows[e.RowIndex].Cells["InventoryDetRefNo"].Value.ToString(), tmp,
                                                               out status))
                                {
                                    f.Dispose();
                                    int firstRowIndex = dgvStock.FirstDisplayedCell.RowIndex;
                                    BindGrid();
                                    dgvStock.FirstDisplayedCell = dgvStock.Rows[firstRowIndex].Cells[0];
                                }
                                else
                                {
                                    f.Dispose();
                                    MessageBox.Show(LanguageManager.Error_encounter_ + status);
                                }
                            }
                        }
                    }
					
                    if (dgvStock.Columns[e.ColumnIndex].Name == Discount.Name)
                    {
                        frmKeypad f = new frmKeypad();
                        f.IsInteger = false;
                        f.initialValue = dgvStock.Rows[e.RowIndex].Cells["Discount"].Value.ToString();
                        CommonUILib.displayTransparent();
                        f.ShowDialog();
                        CommonUILib.hideTransparent();
                        decimal tmp;
                        if (f.value != "" && Decimal.TryParse(f.value, out tmp))
                        {
                            if (invCtrl.ChangeLineDiscount(dgvStock.Rows[e.RowIndex].Cells["InventoryDetRefNo"].Value.ToString(), tmp, out status))
                            {
                                f.Dispose();
                                int firstRowIndex = dgvStock.FirstDisplayedCell.RowIndex;
                                BindGrid();
                                dgvStock.FirstDisplayedCell = dgvStock.Rows[firstRowIndex].Cells[0];
                            }
                            else
                            {
                                f.Dispose();
                                MessageBox.Show(LanguageManager.Error_encounter_ + status);
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

        private void dtpInventoryDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                invCtrl.SetInventoryDate(dtpInventoryDate.Value);
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show(LanguageManager.Error_encounter_ + ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dr = MessageBox.Show(LanguageManager.Are_you_sure_you_want_to_cancel_, "", MessageBoxButtons.YesNo);
                if (dr == DialogResult.No)
                {
                    return;
                }
                Close();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show(LanguageManager.Error_encounter_ + ex.Message);
            }
        }

        private void cmbLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                /*if (this is frmStockIn || this is frmStockOut)
                {
                    if (!PointOfSaleInfo.IntegrateWithInventory)
                    {
                        SyncClientController.Load_WS_URL();
                        PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                        ws.Timeout = 100000;
                        ws.Url = SyncClientController.WS_URL;
                        if (ws.IsThereUnAdjustedStockTake(((InventoryLocation)cmbLocation.SelectedValue).InventoryLocationID) && Text.ToUpper() != "FRMSTOCKTAKE")
                        {
                            isTherePendingStockTake = true;
                            MessageBox.Show(
                                "There is an unadjusted stock take for this location. No inventory movement is allowed! Please adjust stock take first");
                            return;
                        }
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
                }*/
                invCtrl.SetInventoryLocation(((InventoryLocation)cmbLocation.SelectedValue).InventoryLocationID);
                BindGrid();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show(LanguageManager.Error_encounter_ + ex.Message);
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
                f.defaultValue = Text;
                CommonUILib.displayTransparent();
                f.ShowDialog();
                CommonUILib.hideTransparent();
                bool IsSuccessful = f.IsSuccessful;
                string remark = f.txtRemark.Text;
                f.Dispose();

                if (IsSuccessful)
                {
                    //Pop out Textbox to save file name
                    saveRemark = remark;
                    status = SaveToDisk(false);
                    MessageBox.Show(status);
                }



            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show(LanguageManager.Error_encounter_ + ex.Message);
            }
        }

        protected string SaveToDisk(bool autosave)
        {
            try
            {
                string status;
                //save header info
                invCtrl.SetInventoryDate(dtpInventoryDate.Value);
                invCtrl.SetRemark(txtRemark.Text);

                //Save all possible additional information - 
                if (this is frmStockIn)
                {
                    //use find control...
                    string purchaseOrderNo = (Controls.Find("txtPurchaseOrder", true)[0]).Text;
                    Control[] cmbSup = Controls.Find("cmbSupplier", true);
                    string Supplier = "";
                    if (cmbSup.GetLength(0) > 0)
                        Supplier = ((ComboBox)cmbSup[0]).SelectedValue.ToString();
                    //(Controls.Find("cmbSupplier", true)[0]).Text;
                    decimal FreightCharges;
                    decimal Discount;
                    double ExchangeRate;

                    invCtrl.SetPurchaseOrder(purchaseOrderNo);
                    invCtrl.SetSupplier(Supplier);
                    if (decimal.TryParse((Controls.Find("txtFreight", true)[0]).Text, out FreightCharges))
                    {
                        invCtrl.SetFreightCharges(FreightCharges);
                    }
                    if (decimal.TryParse((Controls.Find("txtDiscount", true)[0]).Text, out Discount))
                    {
                        invCtrl.SetDiscount(Discount);
                    }
                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.ShowCurrency), false))
                    {
                        if (double.TryParse((Controls.Find("txtExchangeRate", true)[0]).Text, out ExchangeRate))
                        {
                            invCtrl.SetExchangeRate(ExchangeRate);
                        }
                    }
                }
                else if (this is frmStockOut)
                {
                    //
                    int stockOutReasonID =
                        ((InventoryStockOutReason)(((ComboBox)Controls.Find("cmbStockOutReason", true)[0]).SelectedValue)).ReasonID;
                    invCtrl.setInventoryStockOutReasonID(stockOutReasonID);
                }
                else if (this is frmStockTransfer)
                {
                    int destinationID =
                        ((InventoryLocation)(((ComboBox)Controls.Find("cmbDestination", true)[0]).SelectedValue)).InventoryLocationID;
                    invCtrl.setTmpSavedData(destinationID);
                }
                else if (this is frmAdjustStock)
                {
                    invCtrl.setTmpSavedData(((ComboBox)Controls.Find("cmbAdjustDirection", true)[0]).SelectedIndex);
                }
                else if (this is frmStockOutReturn)
                {
                    int stockOutReasonID =
                        ((InventoryStockOutReason)(((ComboBox)Controls.Find("cmbStockOutReason", true)[0]).SelectedValue)).ReasonID;
                    invCtrl.setInventoryStockOutReasonID(stockOutReasonID);
                }
                //string status;
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.SaveFileInTheServer), false))
                {
                    if (invCtrl.getUniqueID() == Guid.Empty)
                    {
                        invCtrl.createNewGUID();
                    }
                    status = LanguageManager.Save_successful;
                    if (SyncClientController.SaveInventoryFile(invCtrl, invCtrl.getUniqueID().ToString(), Tag.ToString().Equals("") ? Text : Tag.ToString(),
                         saveRemark, autosave, out status))
                    {
                        status = LanguageManager.Save_successful;
                        //MessageBox.Show();
                    }
                    else
                    {
                        status = LanguageManager.Error_encounter_ + status;
                    }
                    return status;
                }
                else
                {

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
                        (invCtrl, invCtrl.getUniqueID().ToString(), Tag.ToString().Equals("") ? Text : Tag.ToString(),
                         Application.StartupPath + "\\" + SavedFolder + "\\", saveRemark, autosave, out status))
                    {
                        status = LanguageManager.Save_successful;
                        //MessageBox.Show();
                    }
                    else
                    {
                        status = LanguageManager.Error_encounter_ + status;
                    }
                    return status;
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show(LanguageManager.Error_encounter_ + ex.Message);
                return ex.Message;
            }
        }

        protected virtual void dgvStock_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            try
            {
                {
                    if ((decimal)dgvStock.Rows[e.RowIndex].Cells["Quantity"].Value < 0 ||
                        ((this is frmStockOut || this is frmStockTransfer) &&
                         (decimal)dgvStock.Rows[e.RowIndex].Cells["Quantity"].Value >
                         (decimal)dgvStock.Rows[e.RowIndex].Cells["OnHand"].Value))
                    {
                        dgvStock.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.DarkRed;
                        dgvStock.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.White;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show(LanguageManager.Error_encounter_ + ex.Message);
            }
        }

        public virtual void btnPrint_Click(object sender, EventArgs e)
        {
            SaveToDisk(true);
        }

        private void llSelectAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                invCtrl.MarkAllAsDeletedFromInventoryDetail();
                BindGrid();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show(LanguageManager.Error_encounter_ + ex.Message);
            }
        }

        private void llSelectNone_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                invCtrl.UnmarkkAllAsDeletedFromInventoryDetail();
                BindGrid();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show(LanguageManager.Error_encounter_ + ex.Message);
            }
        }

        private void llInvert_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                invCtrl.InvertAllAsDeletedFromInventoryDetail();
                BindGrid();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show(LanguageManager.Error_encounter_ + ex.Message);
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                CommonUILib.displayTransparent();
                if (saveFileDialogExport.ShowDialog() == DialogResult.OK)
                {
                    ExportController.ExportToExcel(invCtrl.GetInventoryItemsForExport(), saveFileDialogExport.FileName);
                    MessageBox.Show(LanguageManager.Save_successful);
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show(LanguageManager.Error_encounter_ + ex.Message);
            }
            finally
            {
                CommonUILib.hideTransparent();
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            CommonUILib.displayTransparent();

            try
            {
                openFileDialog1.FileName = "";

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    DataTable message = null;
                    DataTable ErrorDb;

                    var inf = new FileInfo(openFileDialog1.FileName);
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
                            if (!string.IsNullOrEmpty(Value))
                            {
                                message.Columns[Counter].ColumnName = Value;
                            }
                            else if (Counter == 0)
                            {
                                message.Columns[Counter].ColumnName = "Barcode";
                            }
                            else if (Counter == 1)
                            {
                                message.Columns[Counter].ColumnName = "Qty";
                            }
                        }
                    }

                    if (message == null || message.Rows.Count < 1)
                        throw new Exception("No data inside");

                    if (this is frmStockIn)
                    {
                        if (invCtrl.ImportFromDataTableForStockIn(message, out ErrorDb))
                        {
                            MessageBox.Show(LanguageManager.Load_successful);
                        }
                        else
                        {
                            var f = new frmImportErrorMessage { source = ErrorDb };
                            f.ShowDialog();
                        }
                    }
                    else
                    {
                        if (invCtrl.ImportFromDataTable(message, out ErrorDb))
                        {
                            MessageBox.Show(LanguageManager.Load_successful);
                        }
                        else
                        {
                            var f = new frmImportErrorMessage { source = ErrorDb };
                            f.ShowDialog();
                        }
                    }
                    BindGrid();
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show(LanguageManager.Load_FAILED_ + ex.Message);
            }
            finally
            {
                CommonUILib.hideTransparent();
            }
        }

        private void dgvStock_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            return;

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

                        invCtrl.MarkAsDeletedFromInventoryDetail
                            (dgvStock.Rows[e.RowIndex].Cells["InventoryDetRefNo"].Value.ToString(),
                             value,
                             out status);
                        dgvStock.Refresh();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show(LanguageManager.Error_encounter_ + ex.Message);
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
                bool result = true;
                //if (this is frmStockTake)
                //    result = SyncClientController.GetCurrentInventory();
                //else
                //    result = SyncClientController.GetCurrentInventoryRealTime();
                //if (result)
                //	result = SyncClientController.UpdateOrderDetFromDownloadedInventoryHdr();
                //result = result && SyncClientController.GenerateInventoryHdrForAdjustedSales();
                e.Result = result;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);

                // if FK conflict then throw error
                if (ex.GetType().ToString() == "System.Data.SqlClient.SqlException")
                {
                    if (((SqlException)ex).Number == 547)
                    {
                        MessageBox.Show(LanguageManager.Error_encounter_ +
                                        "You need to download latest Items from Server, please go to Setup Menu and Sync.");
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
                /*if (StockTakeController.IsThereUnAdjustedStockTake() &&
                    Text.ToUpper() != LanguageManager.STOCK_TAKE)
                {
                    isTherePendingStockTake = true;
                    MessageBox.Show(
                        "There is an unadjusted stock take. No inventory movement is allowed! Please adjust stock take first");
                    //MessageBox.Show(LanguageManager.There_is_no_Stock_Take_data_to_adjust__All_Stock_Take_has_been_adjusted);
                    Close();
                }
                else*/
                //{
                Enabled = true;
                if (!(bool)e.Result)
                {
                    MessageBox.Show(LanguageManager.Error_loading_inventory_from_the_web__Please_check_your_internet_connection_);
                    Close();
                }
                //}
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show(LanguageManager.Error_encounter_ + ex.Message);
            }
        }

        private void bgSearch_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pgb1.PerformStep();
        }

        private void btnDownloadTemplate_Click(object sender, EventArgs e)
        {
            frmExportTemplate fExportMenu = new frmExportTemplate();
            fExportMenu.STOCKTYPE = this is frmStockIn ? "STOCKIN" : "";
            fExportMenu.ShowDialog();
            fExportMenu.Dispose();
        }

        private void btnSetting_Click(object sender, EventArgs e)
        {
            frmInventorySettings Stgs = new frmInventorySettings();
            Stgs.ShowDialog();
            Stgs.Dispose();
        }

        #region PagerControls

        private void btnPreviousPage_Click(object sender, EventArgs e)
        {
            _currentPage--;
            BindGrid();
        }

        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            _currentPage = 0;
            BindGrid();
        }

        private void btnNextPage_Click(object sender, EventArgs e)
        {
            _currentPage++;
            BindGrid();
        }

        private void btnLastPage_Click(object sender, EventArgs e)
        {
            _currentPage = _totalPages - 1;
            BindGrid();
        }

        #endregion

        private void txtDefaultQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void dgvStock_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            ShowItemPicture();
        }

        private bool shown = false;
        private void dgvStock_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (IsItemPictureShown)
                {
                    if (e.RowIndex >= 0)
                    {
                        if (e.ColumnIndex == dgvStock.Columns["Photo"].Index)
                        {
                            //if (dgvPurchase.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag == null) return;

                            if (!shown)
                            {
                                string itemNo = dgvStock.Rows[e.RowIndex].Cells["ItemNo"].Value.ToString();
                                var myItem = new Item(Item.Columns.ItemNo, itemNo);
                                Image img = null;

                                string[] extensions = new string[] { "jpg", "gif", "png", "bmp", "jpeg" };

                                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.UseImageItemFromLocalFolder), false))
                                {
                                    string ImagePhotosFolder = AppSetting.GetSetting(AppSetting.SettingsName.Item.ItemImageFolderLocal);
                                    if (!string.IsNullOrEmpty(ImagePhotosFolder) && Directory.Exists(ImagePhotosFolder))
                                    {

                                        foreach (string ext in extensions)
                                        {
                                            string ImagePath = ImagePhotosFolder + itemNo.Trim() + "." + ext;
                                            if (System.IO.File.Exists(ImagePath))
                                            {
                                                img = Image.FromFile(ImagePath);
                                            }

                                        }
                                    }
                                }
                                else
                                {
                                    if (myItem.ItemImage != null)
                                    {
                                        img = Image.FromStream(new MemoryStream(myItem.ItemImage));
                                    }
                                }

                                if (img != null)
                                {
                                    PictureBox pictureBox = new PictureBox()
                                    {
                                        Image = img,
                                        Dock = DockStyle.Fill,
                                        SizeMode = PictureBoxSizeMode.Zoom,
                                        BorderStyle = BorderStyle.None
                                    };

                                    Form newForm = new Form()
                                    {
                                        FormBorderStyle = FormBorderStyle.None,
                                        TopMost = true,
                                        ControlBox = false,
                                        //Text=" ",
                                        MaximizeBox = false,
                                        MinimizeBox = false,
                                        ShowInTaskbar = false,
                                        StartPosition = FormStartPosition.CenterParent,
                                        Location = new Point(dgvStock.Location.X, dgvStock.Location.Y),
                                        //WindowState = FormWindowState.Maximized
                                    };

                                    newForm.Controls.Add(pictureBox);
                                    //newForm.BackColor = Color.White;
                                    newForm.TransparencyKey = SystemColors.Control;
                                    newForm.Left = 100;
                                    newForm.Top = 100;
                                    newForm.Width = this.Width - 200;
                                    newForm.Height = this.Height - 200;
                                    newForm.KeyDown += new KeyEventHandler(newForm_KeyDown);
                                    newForm.Deactivate += new EventHandler(newForm_Deactivate);
                                    newForm.Show();
                                    shown = true;
                                }
                                //newForm.Dispose();
                            }
                        }
                    }
                }
            }
            catch
            { }
        }

        private void ShowItemPicture()
        {
            try
            {
                if (IsItemPictureShown)
                {
                    //string[] extensions = { "jpg", "png", "bmp", "jpeg" };
                    foreach (DataGridViewRow row in dgvStock.Rows)
                    {
                        string itemNo = row.Cells["ItemNo"].Value.ToString();
                        var myItem = new Item(Item.Columns.ItemNo, itemNo);
                        string[] extensions = new string[] { "jpg", "gif", "png", "bmp", "jpeg" };

                        if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.UseImageLocal), false))
                        {
                            string ImagePhotosFolder = AppSetting.GetSetting(AppSetting.SettingsName.Inventory.ImageLocalPath);
                            if (!string.IsNullOrEmpty(ImagePhotosFolder) && Directory.Exists(ImagePhotosFolder))
                            {

                                foreach (string ext in extensions)
                                {
                                    string ImagePath = ImagePhotosFolder + itemNo.Trim() + "." + ext;
                                    if (System.IO.File.Exists(ImagePath))
                                    {
                                        Image img = ItemController.ResizeImage(Image.FromFile(ImagePath), new Size(40, 40));
                                        row.Cells["Photo"].Value = img;
                                        row.Cells["Photo"].Tag = ImagePath;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (myItem.ItemImage != null)
                            {
                                row.Cells["Photo"].Value = ItemController.ResizeImage(Image.FromStream(new MemoryStream(myItem.ItemImage)), new Size(40, 40));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        private void newForm_Deactivate(object sender, EventArgs e)
        {
            ((Form)sender).Close();
            shown = false;
        }
        private void newForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                ((Form)sender).Close();
        }
    }
}