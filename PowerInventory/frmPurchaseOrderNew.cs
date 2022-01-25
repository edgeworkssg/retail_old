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
    public partial class frmPurchaseOrderNew : Form
    {
        private string existingConnectionString;
        protected bool showCostPrice;
        protected bool showOnHandQty;
        private bool showPacking; 
        private bool showCurrency;
        private bool showUOM;
        private bool showGST;
        private bool showRetailPrice;
        private string filename;
        protected string saveRemark;
        private const string SavedFolder = "SAVED";
        protected const bool printAfterConfirm = true;
        protected bool isTherePendingStockTake;
        private bool useCustomRefNo;
        private OpenFileDialog openFileAttachment;
        private FileAttachmentCollection attachColl = new FileAttachmentCollection();
        private string tempAttachmentFolder;
        private BackgroundWorker bgSave;

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

        private string gstText = "GST";
        private string purchaseOrderHdrRefNo = "";
        private bool isEditPO = false;
        private bool IsItemPictureShown = false;

        public frmPurchaseOrderNew()
        {
            
            try
            {
                bgSave = new BackgroundWorker();
                bgSave.DoWork += new DoWorkEventHandler(bgSave_DoWork);
                bgSave.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgSave_RunWorkerCompleted);

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
                //showGST = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowGST), false);
                showPacking = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowPackingSize), false);
                showCurrency = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowCurrency), false);
                //showCostPrice = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowFactoryPrice), false);
                showUOM = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowUOM), false);
                showRetailPrice = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowRetailPrice), false);
                
                string status = "";

                if (!PointOfSaleInfo.IntegrateWithInventory)
                {
                    GetCustomPONoSettingFromServer(out status);                       
                }

                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.AllowChangeInventoryDate), false))
                    dtpInventoryDate.Enabled = true;
                useCustomRefNo = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.UseCustomNo), false);
                btnLoadAddress.Visible = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowLoadAddressBtn), false);

                tempAttachmentFolder = Guid.NewGuid().ToString();
                btnAttachment.Visible = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.EnableAttachment), false);

                if (!string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.Interface.ReplaceGSTTextWith)))
                    gstText = AppSetting.GetSetting(AppSetting.SettingsName.Interface.ReplaceGSTTextWith);

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
                MessageBox.Show("Error encounter." + ex.Message);
            }
        }

        public frmPurchaseOrderNew(string purchaseOrderHdrRefNo)
        {

            try
            {
                
                bgSave = new BackgroundWorker();
                bgSave.DoWork += new DoWorkEventHandler(bgSave_DoWork);
                bgSave.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgSave_RunWorkerCompleted);
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
                //showGST = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowGST), false);
                showPacking = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowPackingSize), false);
                showCurrency = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowCurrency), false);
                //showCostPrice = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowFactoryPrice), false);
                showUOM = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowUOM), false);
                showRetailPrice = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowRetailPrice), false);

                string status = "";

                if (!PointOfSaleInfo.IntegrateWithInventory)
                {
                    GetCustomPONoSettingFromServer(out status);                        
                }

                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.AllowChangeInventoryDate), false))
                    dtpInventoryDate.Enabled = true;
                useCustomRefNo = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.UseCustomNo), false);
                btnLoadAddress.Visible = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowLoadAddressBtn), false);

                tempAttachmentFolder = Guid.NewGuid().ToString();
                btnAttachment.Visible = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.EnableAttachment), false);

                if (!string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.Interface.ReplaceGSTTextWith)))
                    gstText = AppSetting.GetSetting(AppSetting.SettingsName.Interface.ReplaceGSTTextWith);

                cmbCriteria.Items.Add("Contains");
                cmbCriteria.Items.Add("Starts With");
                cmbCriteria.Items.Add("Ends With");
                cmbCriteria.Items.Add("Exact Match");
                cmbCriteria.SelectedIndex = 0;

                this.purchaseOrderHdrRefNo = purchaseOrderHdrRefNo;
                this.isEditPO = true;

                
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("Error encounter." + ex.Message);
            }
        }

        void bgSave_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                SyncClientController.Load_WS_URL();
                PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                ws.Timeout = 1000000;
                ws.Url = SyncClientController.WS_URL;

                #region *) Upload the files first
                FileAttachmentCollection tmpAttColl = new FileAttachmentCollection();
                if (attachColl.Count > 0)
                {
                    attachColl.CopyTo(tmpAttColl);
                    foreach (FileAttachment fa in tmpAttColl)
                    {
                        string fullPath = Path.Combine(fa.FileLocation, fa.FileName);
                        FileInfo fInfo = new FileInfo(fullPath);
                        long numBytes = fInfo.Length;
                        //double dLen = Convert.ToDouble(fInfo.Length / 1024); // Get the file size in KB
                        byte[] attachment;

                        using (FileStream fStream = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
                        {
                            using (BinaryReader br = new BinaryReader(fStream))
                            {
                                // convert the file to a byte array
                                attachment = br.ReadBytes((int)numBytes);
                                br.Close();
                            }
                            fStream.Close();
                            fStream.Dispose();
                        }

                        string serverPath;
                        if (!ws.UploadAttachment(attachment, tempAttachmentFolder, fa.FileName, false, "PurchaseOrder", out serverPath, out bgSave_status))
                        {
                            MessageBox.Show(string.Format("Failed to upload {0} to server. Error message: {1}", fa.FileName, bgSave_status), "Error");
                            return;
                        }

                        fa.FileName = Path.GetFileName(serverPath);
                        fa.FileLocation = Path.GetDirectoryName(serverPath);
                    }
                }
                #endregion
                string status = "";
                DataSet myDataSet = new DataSet();
                myDataSet.Tables.Add(invCtrl.InvHdrToDataTable());
                myDataSet.Tables.Add(invCtrl.InvDetToDataTable());
                myDataSet.Tables.Add(tmpAttColl.ToDataTable());
                byte[] data = SyncClientController.CompressDataSetToByteArray(myDataSet);

                if (ws.PurchaseOrderCompressedWithCustomRefNoEdit
                    (data,
                    UserInfo.username,
                    (int)e.Argument,
                    false,isEditPO, out newPurchaseOrderHdrRefNo, out newCustomRefNo, out bgSave_status))
                {
                    isSavePOSuccess = true;
                    invCtrl.InvHdr.PurchaseOrderHdrRefNo = newPurchaseOrderHdrRefNo;
                    invCtrl.InvHdr.CustomRefNo = newCustomRefNo;

                    if (useCustomRefNo)
                    {
                        if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.GenerateCustomNoInServer), false))
                        {
                            // Get the latest number from server (note: update has already performed in server)
                            if (!GetCustomPONoSettingFromServer(out status))
                                throw new Exception(status);
                        }
                        else
                        {
                            PurchaseOdrController.CustomRefNoUpdate();
                        }
                    }

                    ////download inventoryhdr and inventorydet                        
                    //if (SyncClientController.GetCurrentPurchaseOrder())
                    //{
                    //    isSavePOSuccess = true;
                    //}
                    //else
                    //{
                    //    Logger.writeLog("Unable to download data from server: " + bgSave_status);
                    //    isSavePOSuccess = true; // Still set to true because data has already been saved in server
                    //}
                }
                else
                {
                    isSavePOSuccess = false;
                }
            }
            catch (Exception ex)
            {
                bgSave_status = ex.Message;
                Logger.writeLog(ex);
            }
        }

        void bgSave_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                pnlLoading.Visible = false;
                this.Enabled = true;

                if (isSavePOSuccess)
                {
                    MessageBox.Show("Purchase Order successful");

                    AccessLogController.AddLog(AccessSource.POS, UserInfo.username, "-", "Stock Transfer : " + invCtrl.InvHdr.PurchaseOrderHdrRefNo, "");

                    if (printAfterConfirm)
                    {
                        //invCtrl = new PurchaseOdrController(newPurchaseOrderHdrRefNo); //reload...
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
                else
                {
                    MessageBox.Show("Save Purchase Order failed. " + bgSave_status);
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("Error encounter." + ex.Message);
            }
        }

        #region *) Method

        private double GST;

        private bool LoadGST()
        {
            //Load GST from GST Table
            Query qry = new Query("GST");
            Where whr = new Where();
            whr.ColumnName = PowerPOS.Gst.Columns.CommenceDate;
            whr.Comparison = Comparison.LessOrEquals;
            whr.ParameterName = "@CommenceDate";
            whr.ParameterValue = DateTime.Now.ToString("yyyy-MM-dd");
            whr.TableName = "GST";
            //pull out from GST table
            object obj = qry.GetMax(PowerPOS.Gst.Columns.GSTRate, whr);
            GST = (double.Parse(obj.ToString()));
            return true;
        }

        protected void BindGrid()
        {
            try
            {
                string status = "";

                if (invCtrl == null) return;
                DataTable dt = invCtrl.FetchUnSavedPurchaseOrderItemsWithDetailDeleted(out status);
                dgvStock.DataSource = dt;
                if (status != "")
                {
                    MessageBox.Show("Error: " + status);
                    return;
                }

                
                decimal totalCostPrice = 0;
                //populate total cost price column 
                for (int i = 0; i < dgvStock.Rows.Count; i++)
                {
                    totalCostPrice += (decimal)dgvStock.Rows[i].Cells[CostOfGoods.Name].Value;
                }
                lblTotalCostPriceAmount.Text = totalCostPrice.ToString("N");

                
                lblOrderAmountValue.Text = totalCostPrice.ToString("N");
                decimal deliveryCharge = 0;
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowDeliveryCharge), false))
                {
                    decimal minPurchase;
                    if (decimal.TryParse(txtMinPurchase.Text, out minPurchase))
                    {
                        if (minPurchase > totalCostPrice)
                            decimal.TryParse(txtDeliveryCharge.Text, out deliveryCharge);
                    }
                    lblDeliveryChargeValue.Text = deliveryCharge.ToString("N");

                }
                else
                {
                    lblDeliveryCharge1.Visible = false;
                    lblDeliveryChargeValue.Visible = false;
                }

                decimal Subtotal = totalCostPrice + deliveryCharge;

                decimal gstAmount = 0;
                decimal grandTotal = 0;
                LoadGST();
                decimal gstPercentage = (decimal)GST / 100;
                if (cmbGST.SelectedValue != null && cmbGST.SelectedValue.ToString() == "1")
                {
                    //Exclusive
                    gstAmount = Subtotal * gstPercentage;
                    grandTotal = Subtotal + gstAmount;
                }
                else if (cmbGST.SelectedValue != null && cmbGST.SelectedValue.ToString() == "2")
                {
                    //inclusive
                    gstAmount = Subtotal * gstPercentage / (1 + gstPercentage);
                    grandTotal = Subtotal;
                    Subtotal = Subtotal - gstAmount;
                }
                else
                {
                    gstAmount = 0;
                    grandTotal = Subtotal + gstAmount;
                }

                lblSubtotalValue.Text = Subtotal.ToString("N");
                lblGSTValue.Text = gstAmount.ToString("N");
                lblGrandTotalValue.Text = grandTotal.ToString("N");
                
            

                dgvStock.Refresh();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("An unknown error has been encountered. Please contact your system administrator.");
                txtItemNoBarcode.Focus();
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

                DataSet dsSupplier = ws.GetDataTable("Supplier", true);
                DataTable dtSupplier = new DataTable();
                if (dsSupplier.Tables.Count > 0)
                {
                    dtSupplier = dsSupplier.Tables[0];
                }
                dtSupplier.DeleteRow("Deleted", 1);
                DataView dv = dtSupplier.DefaultView;
                dv.Sort = "SupplierName ASC";
                DataTable dtSortedSupplier = dv.ToTable();
                DataRow drSupplier = dtSortedSupplier.NewRow();
                drSupplier["SupplierID"] = -1;
                drSupplier["SupplierName"] = "--Select Supplier--";
                dtSortedSupplier.Rows.InsertAt(drSupplier, 0);
                dtSortedSupplier.AcceptChanges();
                cmbSupplier.DataSource = dtSortedSupplier;
                cmbSupplier.ValueMember = "SupplierID";
                cmbSupplier.DisplayMember = "SupplierName";
            

                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowDeliveryDateTime), false))
                {
                    txtDeliveryDate.Value = DateTime.Now;
                    txtDeliveryDate.Format = DateTimePickerFormat.Custom;
                    txtDeliveryDate.CustomFormat = "dd-MM-yyyy";
                }
                else
                {
                    lblDeliveryDate.Visible = false;
                    txtDeliveryDate.Visible = false;
                    lblReceivingTime.Visible = false;
                    txtReceivingTime.Visible = false;

                }

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
                cmbGST.SelectedValue = 1;
                

                if (!AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowDeliveryAddress), false))
                {
                    lblDeliveryAddress.Visible = false;
                    txtDeliveryAddress.Visible = false;
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
                }
                tblInventory.Refresh();

                //dgvStock.Columns[CostOfGoods.Name].Visible = showCostPrice;
                //dgvStock.Columns[FactoryPrice.Name].Visible = showCostPrice;
                //dgvStock.Columns[PackingSizeCost.Name].Visible = showCostPrice;
                dgvStock.Columns[PackingQuantity.Name].Visible = showPacking;
                dgvStock.Columns[PackingSizeName.Name].Visible = showPacking;
                dgvStock.Columns[PackingSizeCost.Name].Visible = showPacking;
                dgvStock.Columns[UOM.Name].Visible = showUOM;
                dgvStock.Columns[RetailPrice.Name].Visible = showRetailPrice;

                dgvStock.Columns[RetailPrice.Name].HeaderText = showRetailPrice ? "Selling Price" : "Retail Price";
                dgvStock.Columns[CostOfGoods.Name].HeaderText = "Total Cost Price";
                dgvStock.Columns[FactoryPrice.Name].HeaderText = "Unit Cost In Base UOM";
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("Error encounter." + ex.Message);
            }
        }

        protected void HideStockBalanceAndFactoryPrice()
        {
            try
            {
                if (!showOnHandQty)
                {
                    dgvStock.Columns[OnHand.Name].Visible = false;
                }
                if (!showCostPrice)
                {
                    dgvStock.Columns[PackingSizeCost.Name].Visible = false;
                    dgvStock.Columns[FactoryPrice.Name].Visible = false;
                    dgvStock.Columns[CostOfGoods.Name].Visible = false;
                }
                dgvStock.Refresh();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                this.Close();
            }
        }

        private void ClearAdditionalInformation()
        {
            try
            {
                txtDeliveryAddress.Text = "";
                txtPaymentTerm.Text = "";

                cmbSupplier.SelectedValue = -1;
                txtDeliveryDate.Value = DateTime.Now;
                txtMinPurchase.Text = "0";
                txtDeliveryCharge.Text = "0";

                LoadInventoryController();
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
                attachColl = new FileAttachmentCollection();
                tempAttachmentFolder = Guid.NewGuid().ToString();
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
                    if (dgvStock.Rows[i].Cells[SN.Name].Value == null ||
                        dgvStock.Rows[i].Cells[SN.Name].Value.ToString().ToLower() == "false")
                    {
                        ar.Add(dgvStock.Rows[i].Cells[PurchaseOrderDetRefNo.Name].Value);

                    }
                }

                tmpDetCol = new PurchaseOrderDetCollection();
                for (int i = 0; i < ar.Count; i++)
                {
                    if (!invCtrl.GetPurchaseOrderDet(ar[i].ToString()).IsDetailDeleted)
                    {
                        tmpDetCol.Add(invCtrl.GetPurchaseOrderDet(ar[i].ToString()));
                        invCtrl.DeleteFromPurchaseOrderDetail(ar[i].ToString(), out status);
                    }
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
                    if (dgvStock.Rows[i].Cells[SN.Name].Value != null
                        && dgvStock.Rows[i].Cells[SN.Name].Value.ToString().ToLower() == "true")
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
                    printOutParameter.UserField10Value = string.IsNullOrEmpty(invController.getDeliveryTime()) ? "-" : invController.getDeliveryTime();

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

                    string gstLabel = "GST";
                    if (!string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.Interface.ReplaceGSTTextWith)))
                        gstLabel = AppSetting.GetSetting(AppSetting.SettingsName.Interface.ReplaceGSTTextWith);

                    printOutParameter.UserField18Label = gstLabel;
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


        private bool GetCustomPONoSettingFromServer(out string status)
        {
            status = "";
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
                else
                    throw new Exception("Get Custom PO no is Empty");
  

                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = "Failed to get Ref No from server. Error: " + ex.Message;

                return false;
            }
        }

        private void BindAttachment()
        {
            dgvAttachment.AutoGenerateColumns = false;
            dgvAttachment.DataSource = attachColl;
        }

        protected void ShowPanelAttachment()
        {
            try
            {
                pnlAttachment.Parent = this;
                pnlAttachment.Height = 250;
                pnlAttachment.Location = new Point(
                    this.ClientSize.Width / 2 - pnlAttachment.Size.Width / 2,
                    this.ClientSize.Height / 2 - pnlAttachment.Size.Height / 2);
                pnlAttachment.Anchor = AnchorStyles.None;
                pnlAttachment.BringToFront();
                pnlAttachment.Visible = true;
                this.Refresh();
            }
            catch (Exception ex)
            {
                pnlAttachment.Visible = false;
                Logger.writeLog(ex);
                MessageBox.Show("Error encounter." + ex.Message);
            }
        }

        #endregion

        #region *) Event Handler

        private void btnScanItemNo_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbSupplier.Text == "--Select Supplier--")
                {
                    MessageBox.Show("Please Select Supplier");
                    return;
                }

                if (cmbLocation.Text == "--Please Select--")
                {
                    MessageBox.Show("Please Select Location");
                    return;
                }

                string status;
                string correctItemNo;
                ArrayList correctItemNoList = null;
                ArrayList correctItemNames = null;
                ArrayList correctDescriptions = null;
                ArrayList correctUserflag1s = null;
                ArrayList correctAttributes1 = null;

                bool isshowmatrixform = false;
                bool ismatrixattributes1 = false;

                bool supplierIsCompulsory = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.SupplierIsCompulsoryPO), false);
                bool searchItemBySupplier = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.SearchItemBySupplier), false);

                if (ItemController.IsInventoryItem(txtItemNoBarcode.Text.Trim()))
                {
                    //by ITEM NUMBER
                    if (supplierIsCompulsory)
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
                    if (txtItemNoBarcode.Text.Trim() != "" && ItemController.IsInventoryItemBarcode(txtItemNoBarcode.Text.Trim(), out correctItemNo))
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

                            if (supplierIsCompulsory && searchItemBySupplier)
                            {
                                frmAddItemWithFilter myAddItem = new frmAddItemWithFilter();
                                myAddItem.supplierID = cmbSupplier.SelectedValue.ToString().GetIntValue();
                                myAddItem.supplierName = cmbSupplier.Text;
                                myAddItem.searchReq = txtItemNoBarcode.Text.Replace(' ', '%');
                                myAddItem.criteria = cmbCriteria.Text;
                                myAddItem.invLocation = cmbLocation.SelectedValue is InventoryLocation ? ((InventoryLocation)cmbLocation.SelectedValue).InventoryLocationID : 0;
                                CommonUILib.displayTransparent();
                                myAddItem.ShowDialog();
                                CommonUILib.hideTransparent();

                                if (myAddItem != null && myAddItem.itemNumbers != null)
                                {
                                    ArrayList uom = new ArrayList();
                                    correctItemNoList = myAddItem.itemNumbers;
                                    myAddItem.Dispose();
                                    pnlLoading.Visible = false;

                                    for (int i = correctItemNoList.Count - 1; i >= 0; i--)
                                    {
                                        string itemNo = correctItemNoList[i] + "";
                                        Item it = new Item(itemNo);

                                        decimal qty = (myAddItem.quantities[i] + "").GetDecimalValue();
                                        string packingSizeName = myAddItem.uoms[i] + "";
                                        decimal packingSizeUOM = (myAddItem.convRate[i] + "").GetDecimalValue();;
                                        decimal packingSizeCostPrice = (myAddItem.costPrices[i] + "").GetDecimalValue();

                                        int gstType = 0;
                                        if (!int.TryParse(cmbGST.SelectedValue.ToString(), out gstType))
                                            gstType = 0;
                                        
                                        //AddItem to Inventory
                                        if (!invCtrl.AddItemToPurchaseOrderByPackingSize(itemNo, qty, packingSizeName, packingSizeUOM, packingSizeCostPrice, gstType, out status))
                                        {
                                            MessageBox.Show("Error: " + status);
                                        }
                                        
                                    }

                                    BindGrid();
                                    txtItemNoBarcode.Text = "";
                                    txtItemNoBarcode.Focus();
                                    return;
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
                            else
                            {
                                frmAddItem myAddItem = new frmAddItem();
                                myAddItem.SupplierName = cmbSupplier.Text;
                                myAddItem.searchReq = txtItemNoBarcode.Text.Replace(' ', '%');
                                myAddItem.criteria = cmbCriteria.Text;
                                myAddItem.IsPurchaseOrder = true;
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
                    //cek result if
                    if (correctItemNoList.Count == 1)
                    {
                        //ismatrix item
                        if ((correctUserflag1s.Count == 1) && (correctUserflag1s[0].ToString().ToLower() == "true"))
                        {
                            isshowmatrixform = true;
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
                    //pop up multiple quantity
                    frmMultipleQtyEntryPO fQty = new frmMultipleQtyEntryPO((cmbSupplier.SelectedValue + "").GetIntValue());
                    fQty.itemNos = correctItemNoList;
                    fQty.itemNames = correctItemNames;
                    fQty.descriptions = correctDescriptions;
                    CommonUILib.displayTransparent();
                    fQty.ShowDialog();
                    CommonUILib.hideTransparent();
                    pnlLoading.Visible = false;
                    if (fQty.IsSuccessful)
                    {
                        DataTable dtItem = fQty.SelectedItems;
                        for (int i = dtItem.Rows.Count - 1; i >= 0; i--)
                        {
                            string itemNo = dtItem.Rows[i]["ItemNo"] + "";
                            decimal qty = (dtItem.Rows[i]["Qty"] + "").GetDecimalValue();
                            string packingSizeName = dtItem.Rows[i]["PackingSizeName"] + "";
                            decimal packingSizeUOM = (dtItem.Rows[i]["PackingSizeUOM"] + "").GetDecimalValue();
                            decimal packingSizeCostPrice = (dtItem.Rows[i]["PackingSizeCostPrice"] + "").GetDecimalValue();
                            int gstType = 0;
                            if (!int.TryParse(cmbGST.SelectedValue.ToString(), out gstType))
                                gstType = 0;
                            
                            if (!invCtrl.AddItemToPurchaseOrderByPackingSize(itemNo, qty, packingSizeName, packingSizeUOM, packingSizeCostPrice, gstType, out status))
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
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show(LanguageManager.Error_encounter_ + ex.Message);
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
            string status = "";

            try
            {
                existingConnectionString = DataService.GetInstance("POWERPOS").DefaultConnectionString;
                tbControl.SelectedIndex = 1;
                tbControl.SelectedIndex = 0;

                if (!isTherePendingStockTake)
                {
                    //showCostPrice = true;
                    showOnHandQty = true;
                    HideStockBalanceAndFactoryPrice();
                    btnSave.Text = "ORDER";
                    this.Text = "PURCHASE ORDER";
                    label11.Text = gstText;
                    lblGST1.Text = gstText;
                    AddAdditionalInformation();
                    LoadInventoryController();

                    DataGridViewImageColumn pictureColumn = new DataGridViewImageColumn();
                    pictureColumn.Name = "Photo";
                    pictureColumn.Width = 60;
                    pictureColumn.Visible = true;
                    dgvStock.Columns.Insert(2, pictureColumn);

                    if (!IsItemPictureShown)
                    {
                        pictureColumn.Visible = false;
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

                    if (isEditPO && !string.IsNullOrEmpty(purchaseOrderHdrRefNo))
                    {
                        invCtrl.LoadConfirmedPurchaseOrderController(purchaseOrderHdrRefNo);

                        // Remove rows where IsDetailDeleted = true 
                        foreach (var poDet in invCtrl.InvDet.Clone())
                        {
                            if (poDet.IsDetailDeleted)
                                invCtrl.DeleteFromPurchaseOrderDetail(poDet.PurchaseOrderDetRefNo, out status);
                        }

                        // Fix the CostOfGoods for displaying in DataGrid
                        foreach (var poDet in invCtrl.InvDet)
                        {
                            poDet.CostOfGoods = poDet.Quantity.GetValueOrDefault(0) * poDet.FactoryPrice;
                        }

                        txtRefNo.Text = invCtrl.InvHdr.PurchaseOrderHdrRefNo;
                        for (int a = 0; a < cmbLocation.Items.Count; a++)
                        {
                            if (((InventoryLocation)cmbLocation.Items[a]).InventoryLocationID == invCtrl.InvHdr.InventoryLocationID)
                                cmbLocation.SelectedItem = cmbLocation.Items[a];
                        }
                        dtpInventoryDate.Value = DateTime.Now;
                        lblGST.Text = invCtrl.InvHdr.Remark.Replace("\r\n", Environment.NewLine);
                        invCtrl.InvHdr.Status = null;

                        cmbSupplier.SelectedIndex = cmbSupplier.FindStringExact(invCtrl.getSupplierName());

                        if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowDeliveryDateTime), false))
                        {
                            txtDeliveryDate.Value = invCtrl.getDeliveryTimeFormatted().GetDateTimeValue("dd MMM yyyy");
                            
                        }

                        cmbGST.SelectedValue = invCtrl.getGSTType();


                        if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowDeliveryAddress), false))
                        {
                            txtDeliveryAddress.Text = invCtrl.getDeliveryAddress();
                        }

                        if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowMinPurchase), false))
                        {
                            txtMinPurchase.Text = invCtrl.InvHdr.MinPurchase.ToString("N2");
                        }

                        if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowDeliveryCharge), false))
                        {
                            txtDeliveryCharge.Text = invCtrl.InvHdr.DeliveryCharge.GetValueOrDefault(0).ToString("N2");
                        }
                        

                        if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowCurrency), false))
                        {
                            cmbCurrencies.SelectedValue = invCtrl.getCurrencyID();
                        }
                        
                        if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowPaymentType), false))
                        {
                            txtPaymentTerm.Text = invCtrl.getPaymentTerm();
                        }

                        BindGrid();
                    }

                }
            }

            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        PurchaseOrderDetCollection tmpDetCol;
        bool isSavePOSuccess = false;
        string newPurchaseOrderHdrRefNo = "";
        string newCustomRefNo = "";
        string bgSave_status = "";
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
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

                foreach (PurchaseOrderDet poDet in invCtrl.GetPODetail())
                {
                    if (poDet.PackingSizeName != null && poDet.PackingSizeName != "" && poDet.PackingSizeUOM != 0)
                    {
                        if (!invCtrl.ValidateQuantity(poDet.PurchaseOrderDetRefNo, (decimal)poDet.PackingSizeUOM))
                        {
                            MessageBox.Show("Item " + poDet.Item.ItemNo + "-" + poDet.Item.ItemName + " order qty is not multiply of the packing size quantity. Please rectify before proceed.");
                            return;
                        }

                    }
                }

                foreach (PurchaseOrderDet poDet in invCtrl.GetPODetail())
                {

                    decimal minorderqty = 0;

                    if (ItemSupplierMapController.IsUnderMinQtyOrder(poDet.ItemNo, cmbSupplier.Text, poDet.Quantity.GetValueOrDefault(0), ((InventoryLocation)(cmbLocation.SelectedValue)).InventoryLocationID, out minorderqty))
                    {
                        DialogResult dr = MessageBox.Show(string.Format("Minimum Order quantity of {0} is {1} and you only order {2}. Do you want to continue ?", poDet.ItemNo, minorderqty.ToString(), poDet.Quantity.GetValueOrDefault(0).ToString()), "Warning", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.No)
                            return;
                    }
                }

                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowGST), false))
                {
                    if (cmbGST.Items.Count > 1 && cmbGST.SelectedIndex == -1)
                    {
                        MessageBox.Show("Please select the " + gstText + " Type.");
                        tbControl.SelectedIndex = 0;
                        cmbGST.Focus();
                        return;
                    }
                }

                string status;
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
                if (!int.TryParse(cmbGST.SelectedValue.ToString(), out gstType))
                    gstType = 0;

                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowCurrency), false))
                {
                    int tmpCur = 0;
                    if (int.TryParse(cmbCurrencies.SelectedValue.ToString(), out tmpCur))
                        invCtrl.SetCurrency(tmpCur);
                }
                invCtrl.SetAdditionalHeaderInfo(delDate, delAddress, paymentTerm, receiveTime, gstType.ToString(), minPurchase, delCharge);

                invCtrl.UpdateGSTForDetail(gstType);

                ShowPanelPleaseWait();
                if (!PointOfSaleInfo.IntegrateWithInventory)
                {
                    this.Enabled = false;
                    bgSave.RunWorkerAsync(((InventoryLocation)(cmbLocation.SelectedValue)).InventoryLocationID);
                    return;
                }
                else
                {
                    if (invCtrl.CreateEditOrder(UserInfo.username,
                            ((InventoryLocation)(cmbLocation.SelectedValue)).InventoryLocationID, isEditPO,
                            out status))
                    {
                        //InventoryController.AssignStockOutToConfirmedOrderUsingTransactionScope();
                        newPurchaseOrderHdrRefNo = invCtrl.GetPurchaseOrderHdrRefNo();

                        #region *) Upload attachment
                        SyncClientController.Load_WS_URL();
                        PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                        ws.Timeout = 1000000;
                        ws.Url = SyncClientController.WS_URL;
                        FileAttachmentCollection tmpAttColl = new FileAttachmentCollection();
                        if (attachColl.Count > 0)
                        {
                            attachColl.CopyTo(tmpAttColl);
                            foreach (FileAttachment fa in tmpAttColl)
                            {
                                string fullPath = Path.Combine(fa.FileLocation, fa.FileName);
                                FileInfo fInfo = new FileInfo(fullPath);
                                long numBytes = fInfo.Length;
                                //double dLen = Convert.ToDouble(fInfo.Length / 1024); // Get the file size in KB
                                byte[] attachment;

                                using (FileStream fStream = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
                                {
                                    using (BinaryReader br = new BinaryReader(fStream))
                                    {
                                        // convert the file to a byte array
                                        attachment = br.ReadBytes((int)numBytes);
                                        br.Close();
                                    }
                                    fStream.Close();
                                    fStream.Dispose();
                                }

                                string serverPath;
                                if (!ws.UploadAttachment(attachment, newPurchaseOrderHdrRefNo, fa.FileName, true, "PurchaseOrder", out serverPath, out status))
                                {
                                    MessageBox.Show(string.Format("Failed to upload {0} to server. Error message: {1}", fa.FileName, status), "Error");
                                    return;
                                }

                                fa.FileName = Path.GetFileName(serverPath);
                                fa.FileLocation = Path.GetDirectoryName(serverPath).Replace('\\', '/');
                                fa.RefID = newPurchaseOrderHdrRefNo;
                                fa.IsNew = true;
                                fa.Save(UserInfo.username);
                            }
                        }
                        #endregion

                        pnlLoading.Visible = false;
                        isSavePOSuccess = true;
                        MessageBox.Show("Purchase Order successful");
                    }
                    else
                    {
                        pnlLoading.Visible = false;
                        isSavePOSuccess = false;
                        MessageBox.Show("Error!" + status);
                    }

                }
                //print
                if (isSavePOSuccess)
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
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("Error: " + ex.Message);
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
                    if (dgvStock.Rows[i].Cells[SN.Name].Value != null &&
                        dgvStock.Rows[i].Cells[SN.Name].Value.ToString().ToLower() == "true")
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
            if (e.ColumnIndex < 0 || e.RowIndex < 0)
                return;

            string status;
            try
            {
                string selectedColName = dgvStock.Columns[e.ColumnIndex].Name;
                string poDetRefNo = dgvStock.Rows[e.RowIndex].Cells[PurchaseOrderDetRefNo.Name].Value + "";
                if (string.IsNullOrEmpty(poDetRefNo))
                    return;
                if (selectedColName == SN.Name)
                {
                    bool value = false;
                    if (dgvStock.Rows[e.RowIndex].Cells[SN.Name].Value != null
                        && dgvStock.Rows[e.RowIndex].Cells[SN.Name].Value is bool)
                        value = !(bool)dgvStock.Rows[e.RowIndex].Cells[SN.Name].Value;
                    else
                        value = true;
                    dgvStock.Rows[e.RowIndex].Cells[SN.Name].Value = value;
                }
                else if (selectedColName == PackingQuantity.Name)
                {
                    int oldVal = (dgvStock.Rows[e.RowIndex].Cells[PackingQuantity.Name].Value + "").GetDecimalValue().GetIntValue();
                    frmKeypad frm = new frmKeypad();
                    frm.IsInteger = true;
                    frm.initialValue = oldVal.ToString();
                    CommonUILib.displayTransparent();
                    frm.ShowDialog();
                    CommonUILib.hideTransparent();
                    if (string.IsNullOrEmpty(frm.value))
                        return;
                    int newVal = frm.value.GetIntValue();
                    if (invCtrl.ChangePOPackingQty(poDetRefNo, newVal, out status))
                    {
                        int firstRowIndex = dgvStock.FirstDisplayedCell.RowIndex;
                        BindGrid();
                        dgvStock.FirstDisplayedCell = dgvStock.Rows[firstRowIndex].Cells[SN.Name];
                    }
                    else
                    {
                        MessageBox.Show("Error. " + status);
                    }
                }
                else if (selectedColName == PackingSizeName.Name)
                {
                    string itemNo = dgvStock.Rows[e.RowIndex].Cells[ItemNo.Name].Value + "";
                    int supplierID = (cmbSupplier.SelectedValue + "").GetIntValue();
                    string packingSize = dgvStock.Rows[e.RowIndex].Cells[PackingSizeName.Name].Value + "";
                    var data = PurchaseOrderController.FetchPackingSizeByItemNoAndSupplierNew(itemNo, supplierID);
                    if (data.Count == 0)
                        return;
                    frmSelectPackingSize frm = new frmSelectPackingSize(itemNo, supplierID, packingSize, data);
                    CommonUILib.displayTransparent();
                    frm.ShowDialog();
                    CommonUILib.hideTransparent();
                    if (!frm.IsSuccess)
                        return;
                    if (invCtrl.ChangePOPackingSize(poDetRefNo, frm.SelectedPackingSizeName, frm.SelectedPackingSizeUOM, frm.SelectedPackingSizeCost, out status))
                    {
                        int firstRowIndex = dgvStock.FirstDisplayedCell.RowIndex;
                        BindGrid();
                        dgvStock.FirstDisplayedCell = dgvStock.Rows[firstRowIndex].Cells[SN.Name];
                    }
                    else
                    {
                        MessageBox.Show("Error. " + status);
                    }
                }
                else if (selectedColName == PackingSizeCost.Name)
                {
                    if (!isCostPerPackingSizeEditable)
                        return;
                    decimal oldVal = (dgvStock.Rows[e.RowIndex].Cells[PackingSizeCost.Name].Value + "").GetDecimalValue();
                    frmKeypad frm = new frmKeypad();
                    frm.IsInteger = false;
                    frm.initialValue = oldVal.ToString();
                    CommonUILib.displayTransparent();
                    frm.ShowDialog();
                    CommonUILib.hideTransparent();
                    if (string.IsNullOrEmpty(frm.value))
                        return;
                    decimal newVal = frm.value.GetDecimalValue();
                    if (invCtrl.ChangePOPackingCost(poDetRefNo, newVal, out status))
                    {
                        int firstRowIndex = dgvStock.FirstDisplayedCell.RowIndex;
                        BindGrid();
                        dgvStock.FirstDisplayedCell = dgvStock.Rows[firstRowIndex].Cells[SN.Name];
                    }
                    else
                    {
                        MessageBox.Show("Error. " + status);
                    }
                }
                else if (selectedColName == Quantity.Name)
                {
                    if (showPacking)
                        return;
                    int oldVal = (dgvStock.Rows[e.RowIndex].Cells[Quantity.Name].Value + "").GetDecimalValue().GetIntValue();
                    frmKeypad frm = new frmKeypad();
                    frm.IsInteger = true;
                    frm.initialValue = oldVal.ToString();
                    CommonUILib.displayTransparent();
                    frm.ShowDialog();
                    CommonUILib.hideTransparent();
                    if (string.IsNullOrEmpty(frm.value))
                        return;
                    int newVal = frm.value.GetIntValue();
                    if (invCtrl.ChangePOQty(poDetRefNo, newVal, out status))
                    {
                        int firstRowIndex = dgvStock.FirstDisplayedCell.RowIndex;
                        BindGrid();
                        dgvStock.FirstDisplayedCell = dgvStock.Rows[firstRowIndex].Cells[SN.Name];
                    }
                    else
                    {
                        MessageBox.Show("Error. " + status);
                    }
                }
                else if (selectedColName == FactoryPrice.Name)
                {
                    if (!isCostPriceEditable)
                        return;
                    if (showPacking)
                        return;
                    decimal oldVal = (dgvStock.Rows[e.RowIndex].Cells[FactoryPrice.Name].Value + "").GetDecimalValue();
                    frmKeypad frm = new frmKeypad();
                    frm.IsInteger = false;
                    frm.initialValue = oldVal.ToString();
                    CommonUILib.displayTransparent();
                    frm.ShowDialog();
                    CommonUILib.hideTransparent();
                    if (string.IsNullOrEmpty(frm.value))
                        return;
                    decimal newVal = frm.value.GetDecimalValue();
                    if (invCtrl.ChangePOFactoryPrice(poDetRefNo, newVal, out status))
                    {
                        int firstRowIndex = dgvStock.FirstDisplayedCell.RowIndex;
                        BindGrid();
                        dgvStock.FirstDisplayedCell = dgvStock.Rows[firstRowIndex].Cells[SN.Name];
                    }
                    else
                    {
                        MessageBox.Show("Error. " + status);
                    }
                }
                else if (selectedColName == RetailPrice.Name)
                {
                    if (!isSellPriceEditable)
                        return;
                    decimal oldVal = (dgvStock.Rows[e.RowIndex].Cells[RetailPrice.Name].Value + "").GetDecimalValue();
                    frmKeypad frm = new frmKeypad();
                    frm.IsInteger = false;
                    frm.initialValue = oldVal.ToString();
                    CommonUILib.displayTransparent();
                    frm.ShowDialog();
                    CommonUILib.hideTransparent();
                    if (string.IsNullOrEmpty(frm.value))
                        return;
                    decimal newVal = frm.value.GetDecimalValue();
                    if (invCtrl.ChangeRetailPrice(poDetRefNo, newVal, out status))
                    {
                        int firstRowIndex = dgvStock.FirstDisplayedCell.RowIndex;
                        BindGrid();
                        dgvStock.FirstDisplayedCell = dgvStock.Rows[firstRowIndex].Cells[SN.Name];
                    }
                    else
                    {
                        MessageBox.Show("Error. " + status);
                    }
                }
                else if (selectedColName == CostOfGoods.Name)
                {
                    decimal oldVal = (dgvStock.Rows[e.RowIndex].Cells[CostOfGoods.Name].Value + "").GetDecimalValue();
                    frmKeypad frm = new frmKeypad();
                    frm.IsInteger = false;
                    frm.initialValue = oldVal.ToString();
                    CommonUILib.displayTransparent();
                    frm.ShowDialog();
                    CommonUILib.hideTransparent();
                    if (string.IsNullOrEmpty(frm.value))
                        return;
                    decimal newVal = frm.value.GetDecimalValue();
                    if (invCtrl.ChangePOTotalCost(poDetRefNo, newVal, out status))
                    {
                        int firstRowIndex = dgvStock.FirstDisplayedCell.RowIndex;
                        BindGrid();
                        dgvStock.FirstDisplayedCell = dgvStock.Rows[firstRowIndex].Cells[SN.Name];
                    }
                    else
                    {
                        MessageBox.Show("Error. " + status);
                    }
                }

                //click remark column
                /*else if (dgvStock.Columns[e.ColumnIndex].Name == TotalCostPrice.Name) //previously index 7
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
                    
                }*/

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
                    dgvStock.Rows[i].Cells[SN.Name].Value = true;
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
                    dgvStock.Rows[i].Cells[SN.Name].Value = false;
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
                    if (dgvStock.Rows[i].Cells[SN.Name].Value != null
                        && dgvStock.Rows[i].Cells[SN.Name].Value.ToString().ToLower() == "true")
                    {
                        dgvStock.Rows[i].Cells[SN.Name].Value = false;
                    }
                    else
                    {
                        dgvStock.Rows[i].Cells[SN.Name].Value = true;
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
                DataTable exp = new DataTable();
                exp.Columns.Add("ItemNo");
                exp.Columns.Add("ItemName");
                exp.Columns.Add("POUOM");
                exp.Columns.Add("POQty");
                exp.Columns.Add("POUnitCost");
                exp.Columns.Add("OnHandQty");
                exp.Columns.Add("SuggestedQty");
                exp.Columns.Add("BaseUOM");
                exp.Columns.Add("QtyInBaseUOM");
                exp.Columns.Add("UnitCostInBaseUOM");
                exp.Columns.Add("SellingPrice");
                exp.Columns.Add("TotalCostPrice");

                DataTable stock = (DataTable)dgvStock.DataSource;
                if (stock != null && stock.Rows.Count > 0)
                {
                    for (int i = 0; i < stock.Rows.Count; i++)
                    {
                        DataRow row = exp.NewRow();
                        row["ItemNo"] = stock.Rows[i]["ItemNo"];
                        row["ItemName"] = stock.Rows[i]["ItemName"];
                        row["POUOM"] = stock.Rows[i]["PackingSizeName"];
                        row["POQty"] = stock.Rows[i]["PackingQuantity"];
                        row["POUnitCost"] = stock.Rows[i]["PackingSizeCost"];
                        row["OnHandQty"] = stock.Rows[i]["OnHand"];
                        row["SuggestedQty"] = stock.Rows[i]["SuggestedQty"];
                        row["BaseUOM"] = stock.Rows[i]["UOM"];
                        row["QtyInBaseUOM"] = stock.Rows[i]["Quantity"];
                        row["UnitCostInBaseUOM"] = stock.Rows[i]["FactoryPrice"];
                        row["SellingPrice"] = stock.Rows[i]["RetailPrice"];
                        row["TotalCostPrice"] = stock.Rows[i]["CostOfGoods"];

                        exp.Rows.Add(row);
                    }
                }
                else
                {
                    DataRow row = exp.NewRow();
                    row["ItemNo"] = "";
                    row["ItemName"] = "";
                    row["POUOM"] = "";
                    row["POQty"] = "0";
                    row["POUnitCost"] = "0";
                    row["OnHandQty"] = "0";
                    row["SuggestedQty"] = "0";
                    row["BaseUOM"] = "";
                    row["QtyInBaseUOM"] = "0";
                    row["UnitCostInBaseUOM"] = "0";
                    row["SellingPrice"] = "0";
                    row["TotalCostPrice"] = "0";


                    exp.Rows.Add(row);
                }
                ExportController.ExportToExcel(exp, saveFileDialogExport.FileName);
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

                if (cmbSupplier.SelectedValue.ToString() == "-1")
                {
                    MessageBox.Show("Please select supplier.");
                    return;
                }

                foreach (Item i in ic)
                {

                    if (ItemController.IsLowQuantityItem(i.ItemNo, ((InventoryLocation)(cmbLocation.SelectedValue)).InventoryLocationID,0))

                    {

                        correctItemNoList.Add(i.ItemNo);
                        correctItemNames.Add(i.ItemName);
                        correctDescriptions.Add(i.ItemDesc);
                    }
                }

                if (correctItemNoList.Count > 0)
                {
                    #region old code using old frmMultiplequantity
                    //frmMultipleQtyEntry fQty = new frmMultipleQtyEntry();
                    //fQty.itemNos = correctItemNoList;
                    //fQty.itemNames = correctItemNames;
                    //fQty.descriptions = correctDescriptions;
                    //CommonUILib.displayTransparent(); fQty.ShowDialog(); CommonUILib.hideTransparent();
                    //pnlLoading.Visible = false;
                    //string status;

                    //if (fQty.IsSuccessful)
                    //{
                    //    if(cmbSupplier != null && cmbSupplier.SelectedValue.ToString() != "-1")
                    //        invCtrl.SetSupplier(cmbSupplier.SelectedValue.ToString());

                    //    for (int i = correctItemNoList.Count - 1; i >= 0; i--)
                    //    {
                    //        //AddItem to Inventory
                    //        if (!invCtrl.AddItemIntoInventory(correctItemNoList[i].ToString(), (int)fQty.ht[correctItemNoList[i].ToString()], cmbSupplier.SelectedValue.ToString(), 0, out status))
                    //        {
                    //            MessageBox.Show("Error: " + status);
                    //        }
                    //    }
                    //    BindGrid();
                    //    txtItemNoBarcode.Text = "";
                    //    txtItemNoBarcode.Focus();
                    //}
                    #endregion

                    string status = "";

                    frmMultipleQtyEntryPO fQty = new frmMultipleQtyEntryPO((cmbSupplier.SelectedValue + "").GetIntValue());
                    fQty.itemNos = correctItemNoList;
                    fQty.itemNames = correctItemNames;
                    fQty.descriptions = correctDescriptions;
                    CommonUILib.displayTransparent();
                    fQty.ShowDialog();
                    CommonUILib.hideTransparent();
                    pnlLoading.Visible = false;
                    if (fQty.IsSuccessful)
                    {
                        DataTable dtItem = fQty.SelectedItems;
                        for (int i = dtItem.Rows.Count - 1; i >= 0; i--)
                        {
                            string itemNo = dtItem.Rows[i]["ItemNo"] + "";
                            decimal qty = (dtItem.Rows[i]["Qty"] + "").GetDecimalValue();
                            string packingSizeName = dtItem.Rows[i]["PackingSizeName"] + "";
                            decimal packingSizeUOM = (dtItem.Rows[i]["PackingSizeUOM"] + "").GetDecimalValue();
                            decimal packingSizeCostPrice = (dtItem.Rows[i]["PackingSizeCostPrice"] + "").GetDecimalValue();
                            int gstType = 0;
                            if (!int.TryParse(cmbGST.SelectedValue.ToString(), out gstType))
                                gstType = 0;
                            if (!invCtrl.AddItemToPurchaseOrderByPackingSize(itemNo, qty, packingSizeName, packingSizeUOM, packingSizeCostPrice, gstType, out status))
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

                if (cmbSupplier.SelectedValue.ToString() == "-1")
                {
                    throw new Exception("Please select Supplier");
                }

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

                int gstType = 0;
                if (!int.TryParse(cmbGST.SelectedValue.ToString(), out gstType))
                    gstType = 0;

                int SupplierID = 0;
                if (!int.TryParse(cmbSupplier.SelectedValue.ToString(), out SupplierID))
                {
                    throw new Exception("Supplier Not Valid");
                }

                if (invCtrl.ImportFromDataTableWithPacking(message, gstType, SupplierID, out ErrorDb))
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

        private void btnImport1_Click(object sender, EventArgs e)
        {
            CommonUILib.displayTransparent();
            openFileDialog1.FileName = "";
            openFileDialog1.ShowDialog();
            CommonUILib.hideTransparent();
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

        private void btnAttachment_Click(object sender, EventArgs e)
        {
            BindAttachment();
            ShowPanelAttachment();
        }

        void openFileAttachment_FileOk(object sender, CancelEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(openFileAttachment.FileName)) return;
                string attachedFile = openFileAttachment.FileName;
                FileInfo file = new FileInfo(attachedFile);

                #region *) Validation
                long maxSize = AppSetting.CastInt(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.AttachmentMaxFileSize), 0); // Still in KB
                if (file.Length > maxSize * 1024) // maxSize converted to bytes
                {
                    MessageBox.Show(string.Format("File size exceeds maximum allowed of {0} KB", maxSize.ToString("#,#")), "Error");
                    return;
                }
                #endregion

                #region *) Convert filesize to more readable text
                string fileSize;
                if (file.Length > 1024 * 1024) // MB
                    fileSize = Math.Round((decimal)file.Length / 1024 / 1024, 2).ToString("N2") + " MB";
                else if (file.Length > 1024) // KB
                    fileSize = Math.Round((decimal)file.Length / 1024, 2).ToString("N2") + " KB";
                else
                    fileSize = file.Length.ToString() + " bytes";
                #endregion

                attachColl.Add(new FileAttachment() { 
                    AttachmentID = Guid.NewGuid(), 
                    FileName = Path.GetFileName(attachedFile),
                    FileSize = file.Length,
                    FileLocation = Path.GetDirectoryName(attachedFile),
                    PointOfSaleID = PointOfSaleInfo.PointOfSaleID,
                    ModuleName = "PurchaseOrder",
                    Deleted = false,
                    SizeInText = fileSize
                });
                //string dir = Application.StartupPath + "\\" + AttachmentFolder + "\\" + PointOfSaleInfo.PointOfSaleID + "\\PurchaseOrder\\";
                //if (!Directory.Exists(dir))
                //{
                //    Directory.CreateDirectory(dir);
                //}
                //FileInfo file = new FileInfo(attachedFile);
                //file.CopyTo(Path.Combine(dir, Path.GetFileName(attachedFile)));
                //FileAttachment fa = new FileAttachment();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("Error occured: Attach FAILED!. " + ex.Message);
            }
        }

        private void btnAddAttachment_Click(object sender, EventArgs e)
        {
            CommonUILib.displayTransparent();
            openFileAttachment = new OpenFileDialog();
            openFileAttachment.FileName = "";
            openFileAttachment.FileOk += new CancelEventHandler(openFileAttachment_FileOk);
            openFileAttachment.ShowDialog();
            CommonUILib.hideTransparent();
        }

        private void btnCloseAttachment_Click(object sender, EventArgs e)
        {
            pnlAttachment.Visible = false;
        }

        #endregion

        private void dgvAttachment_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    var attId = dgvAttachment.Rows[e.RowIndex].Cells["dgvcAttachmentID"].Value.ToString();

                    if (dgvAttachment.Columns[e.ColumnIndex].Name == "dgvcDelete")
                    {
                        attachColl.Remove(attachColl.First(a => a.AttachmentID.ToString() == attId));
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("Error occured: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                        string[] extensions = new string[] { "jpg","gif", "png", "bmp", "jpeg" };

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