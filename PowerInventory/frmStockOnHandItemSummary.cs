using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using PowerPOS.Container;
using PowerInventory.InventoryForms;
using PowerInventory.ItemForms;
using System.Configuration;
using SubSonic;
using LanguageManager = PowerInventory.Properties.InventoryLanguage;
using System.IO;

namespace PowerInventory.InventoryForms
{
    public partial class frmStockOnHandItemSummary: Form
    {                                
        ItemController itemLogic;
        public string searchQueryString;
        string existingConnectionString;
        private string searchQuery;
        private bool isRemoveZeroQty;
        private int inventoryLocationID;
        private bool IsItemPictureShown = false;

        public frmStockOnHandItemSummary()
        {
            Program.LoadCultureCode();
            existingConnectionString = DataService.GetInstance("POWERPOS").DefaultConnectionString;

            InitializeComponent();
            itemLogic = new ItemController();
            if (PointOfSaleInfo.IntegrateWithInventory)
            {
                InventoryController.AssignStockOutToConfirmedOrderUsingTransaction(true);
                InventoryController.AssignStockOutToPreOrderSalesUsingTransaction();
            }

            if (!AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.ShowPreOrderQtyInStockOnHand), false))
            {
                dgvPurchase.Columns["PreOrderQty"].Visible = false;
                dgvPurchase.Columns["PreOrderBalance"].Visible = false;
            }

            //hide cost price by default
            dgvPurchase.Columns[3].Visible = false;
            lblTotal.Visible = false;

            IsItemPictureShown = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.AddItemPicture), false);
        }    

        #region *) Event Handler
        
        private void OrderTaking_Load(object sender, EventArgs e)
        {
            try
            {
                existingConnectionString = DataService.GetInstance("POWERPOS").DefaultConnectionString;
                /*if (!PointOfSaleInfo.IntegrateWithInventory)
                {

                }
                else
                {
                    if (StockTakeController.IsThereUnAdjustedStockTake())
                    {   
                        MessageBox.Show("WARNING! There is an unadjusted stock take!");  
                    }   
                }*/

                dgvPurchase.AutoGenerateColumns = false;
                //dgvPurchase.Columns[3].Visible = false;

                AttributesLabelCollection labels = new AttributesLabelCollection();
                DataTable dt = labels.Load().ToDataTable();
                foreach (DataRow row in dt.Rows)
                {
                    string ColumnName = row["Label"].ToString();
                    DataGridViewColumn dColumn = new DataGridViewColumn();
                    dColumn.HeaderText = ColumnName;
                    dColumn.DataPropertyName = "Attributes" + row["AttributesNo"].ToString();
                    dColumn.CellTemplate = new DataGridViewTextBoxCell();
                    dColumn.Width = 70;
                    dColumn.Frozen = false;
                    dgvPurchase.Columns.Insert(dgvPurchase.Columns.Count - 2, dColumn);
                }

                InventoryLocationCollection inv = new InventoryLocationCollection();
                inv.Where(PowerPOS.InventoryLocation.Columns.Deleted, false);
                inv.Load();
                InventoryLocation tmpInv = new InventoryLocation();
                tmpInv.InventoryLocationID = 0;
                tmpInv.InventoryLocationName = "ALL";
                inv.Insert(0, tmpInv);
                cmbLocation.DataSource = inv;
                cmbLocation.Refresh();
                cmbLocation.SelectedIndex = 0;

                CategoryCollection cat = new CategoryCollection();
                cat.Where(PowerPOS.Category.Columns.Deleted, false);
                cat.Load();
                Category tmpCat = new Category();
                tmpCat.CategoryName = "ALL";
                cat.Insert(0, tmpCat);
                cmbCategory.DisplayMember = "CategoryName";
                cmbCategory.ValueMember = "CategoryName";
                cmbCategory.DataSource = cat;
                cmbCategory.Refresh();
                cmbCategory.SelectedIndex = 0;

                DataGridViewImageColumn pictureColumn = new DataGridViewImageColumn();
                pictureColumn.Name = "Photo";
                pictureColumn.Width = 60;
                pictureColumn.Visible = true;
                dgvPurchase.Columns.Insert(2, pictureColumn);

                if (!IsItemPictureShown)
                {
                    pictureColumn.Visible = false;
                }

                this.WindowState = FormWindowState.Maximized;
                txtItemName.Select();

                if (searchQueryString != null && searchQueryString != "")
                {
                    txtItemName.Text = searchQueryString;
                    PopulateData();
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show(LanguageManager.An_unknown_error_has_been_encountered__Please_contact_your_system_administrator_ + ex.Message);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {

            DataTable dt = (DataTable)dgvPurchase.DataSource;
            if (dt != null && dt.Rows.Count > 0)
            {
                CommonUILib.displayTransparent();
                fsdExportToExcel.ShowDialog();
                CommonUILib.hideTransparent();
            }
            else
            {
                MessageBox.Show(LanguageManager.There_is_no_data_to_export);
            }
        }

        private void fsdExportToExcel_FileOk(object sender, CancelEventArgs e)
        {
            ExportController.ExportToExcelWithItemImageInventory(dgvPurchase, fsdExportToExcel.FileName);
            MessageBox.Show(LanguageManager.File_saved);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            PopulateData();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvPurchase_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex >= 0)
            {
                frmEditItem f = new frmEditItem();
                f.ItemRefNo = dgvPurchase.Rows[e.RowIndex].Cells[ItemNo.Name].Value.ToString();
                //change to show CostOFGoods instead of Factory Price, set this to false to set back to Factory Price
                f.IsUseItemCost = true;
                f.ItemCost = Convert.ToDecimal(dgvPurchase.Rows[e.RowIndex].Cells[FactoryPrice.Name].Value.ToString() ?? "0");
                f.IsReadOnly = true;
                CommonUILib.displayTransparent(); f.ShowDialog(); CommonUILib.hideTransparent();
            }

            if (e.ColumnIndex == dgvPurchase.Columns[ItemName.Name].Index && e.RowIndex >= 0)
            {
                int inventoryLocationID = ((InventoryLocation)(cmbLocation.SelectedValue)).InventoryLocationID;
                string itemno = dgvPurchase.Rows[e.RowIndex].Cells[ItemNo.Name].Value.ToString();

                Item i = new Item(itemno);
                if (i.Userflag1 == true)
                {
                    frmStockOnHandMatrix f = new frmStockOnHandMatrix(i.Attributes1, inventoryLocationID);

                    CommonUILib.displayTransparent(); f.ShowDialog(); CommonUILib.hideTransparent();
                }
            }
        }

        private void txtItemName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch_Click(this, new EventArgs());
            }
        }

        private bool shown = false;
        private void dgvPurchase_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (IsItemPictureShown)
                {
                    if (e.RowIndex >= 0)
                    {
                        if (e.ColumnIndex == dgvPurchase.Columns["Photo"].Index)
                        {
                            //if (dgvPurchase.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag == null) return;

                            if (!shown)
                            {
                                string itemNo = dgvPurchase.Rows[e.RowIndex].Cells[ItemNo.Name].Value.ToString();
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
                                        Location = new Point(dgvPurchase.Location.X, dgvPurchase.Location.Y),
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

        private void dgvPurchase_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            ShowItemPicture();
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


        #endregion
                                       
        #region *) Method

        private void PopulateData()
        {
            try
            {
                decimal TotalCost = 0;
                int TotalItem = 0;
                inventoryLocationID = ((InventoryLocation)(cmbLocation.SelectedValue)).InventoryLocationID;
                searchQuery = txtItemName.Text;
                isRemoveZeroQty = cbRemoveQty.Checked;
                if (PointOfSaleInfo.IntegrateWithInventory)
                {
                    inventoryLocationID = ((InventoryLocation)(cmbLocation.SelectedValue)).InventoryLocationID;
                    searchQuery = txtItemName.Text;
                    DataTable dt = ItemSummaryController.FetchStockBalanceWithZeroQty(searchQuery, inventoryLocationID, isRemoveZeroQty, cmbCategory.Text);
                    dgvPurchase.DataSource = dt;
                    dgvPurchase.Refresh();

                    //TotalCost = (decimal)dt.Compute("SUM(COG)", "");
                    if (Decimal.TryParse(dt.Compute("SUM(COG)", "").ToString(), out TotalCost))
                    {
                        if (TotalCost != 0)
                        {
                            lblTotal.Text = "Total Cost: " + TotalCost.ToString("N2");
                        }
                        else
                        {
                            lblTotal.Text = "Total Cost: -";
                        }
                    }
                    else
                    {
                        lblTotal.Text = "Total Cost: -";
                    }

                    //TotalItem = (Int32)dt.Compute("COUNT(COG)", "");
                    if (Int32.TryParse(dt.Compute("COUNT(COG)", "").ToString(), out TotalItem))
                    {
                        if (TotalItem != 0)
                        {
                            lblItemCount.Text = "Total Item Count: " + TotalItem.ToString();
                        }
                        else
                        {
                            lblItemCount.Text = "Total Item Count: -";
                        }
                    }
                    else
                    {
                        lblItemCount.Text = "Total Item Count: -";
                    }
                }
                else
                {
                    inventoryLocationID = ((InventoryLocation)(cmbLocation.SelectedValue)).InventoryLocationID;
                    searchQuery = txtItemName.Text;
                    isRemoveZeroQty = cbRemoveQty.Checked;
                    SyncClientController.Load_WS_URL();
                    PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                    ws.Timeout = 100000;
                    ws.Url = SyncClientController.WS_URL;
                    byte[] data = ws.FetchStockBalanceReportWithZeroQtyByCategory(searchQuery, inventoryLocationID, isRemoveZeroQty, cmbCategory.Text);
                    DataSet myDataSet = SyncClientController.DecompressDataSetFromByteArray(data);
                    if (myDataSet.Tables.Count > 0)
                    {
                        dgvPurchase.DataSource = myDataSet.Tables[0];
                        dgvPurchase.Refresh();

                        //TotalCost = (decimal)myDataSet.Tables[0].Compute("SUM(COG)", "");

                        if (Decimal.TryParse(myDataSet.Tables[0].Compute("SUM(COG)", "").ToString(), out TotalCost))
                        {
                            if (TotalCost != 0)
                            {
                                lblTotal.Text = "Total Cost: " + TotalCost.ToString("N2");
                            }
                            else
                            {
                                lblTotal.Text = "Total Cost: -";
                            }
                        }
                        else {
                            lblTotal.Text = "Total Cost: -";
                        }

                        if (Int32.TryParse(myDataSet.Tables[0].Compute("COUNT(COG)", "").ToString(), out TotalItem))
                        {
                            if (TotalItem != 0)
                            {
                                lblItemCount.Text = "Total Item Count: " + TotalItem.ToString();
                            }
                            else
                            {
                                lblItemCount.Text = "Total Item Count: -";
                            }
                        }
                        else
                        {
                            lblItemCount.Text = "Total Item Count: -";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show(LanguageManager.An_unknown_error_has_been_encountered__Please_contact_your_system_administrator_ + ex.Message);
            }
        }

        private bool GetValueFromRow(out int qty, DataGridViewCellEventArgs e)
        {
            qty = -1;
            
            if (!int.TryParse(dgvPurchase.Rows[e.RowIndex].Cells[SystemBalQty.Name].Value.ToString(), out qty) || qty < 0)
            {
                MessageBox.Show(LanguageManager.You_need_to_enter_a_non_negative_number_for_quantity);                
                dgvPurchase.CancelEdit();
                return false;
            }                        
            return true;
        }

        private void ShowItemPicture()
        {
            try
            {
                if (IsItemPictureShown)
                {
                    //string[] extensions = { "jpg", "png", "bmp", "jpeg" };
                    foreach (DataGridViewRow row in dgvPurchase.Rows)
                    {
                        string itemNo = row.Cells[ItemNo.Name].Value.ToString();
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
       
        #endregion               

        
    }
}