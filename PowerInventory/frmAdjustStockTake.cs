using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using PowerPOS.Container;
using System.Configuration;
using SubSonic;
using System.Collections;
using LanguageManager = PowerInventory.Properties.InventoryLanguage;
using PowerPOS.InventoryRealTimeController;
using System.IO;

namespace PowerInventory
{
    public partial class frmAdjustStockTake : Form
    {
        string existingConnectionString;
        Boolean showCostPrice;
        private List<string> syncStockTakeHistory = new List<string>();
        private int maxHistory = 1000;
        private bool IsItemPictureShown = false;
        public frmAdjustStockTake()
        {
            existingConnectionString = DataService.GetInstance("POWERPOS").DefaultConnectionString;
            showCostPrice = !(AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.HideInventoryCost), false)); 
            InitializeComponent();
            dgvStock.AutoGenerateColumns = false;

            IsItemPictureShown = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.AddItemPicture), false);
        }

        private void frmAdjustStockTake_Load(object sender, EventArgs e)
        {
            try
            {
                existingConnectionString = DataService.GetInstance("POWERPOS").DefaultConnectionString;

                if (!PointOfSaleInfo.IntegrateWithInventory)
                {
                    pnlProgress.Visible = true;
                    bgDownload.RunWorkerAsync();
                    this.Enabled = false;
                }
                else
                {
                    InventoryLocationCollection inv = StockTakeController.GetAllLocationWithOutstandingStockTake();
                    if (inv.Count == 0)
                    {
                        MessageBox.Show(LanguageManager.There_is_no_Stock_Take_data_to_adjust__All_Stock_Take_has_been_adjusted);

                    }
                    InventoryLocation tmp = new InventoryLocation();
                    tmp.InventoryLocationID = 0;
                    tmp.InventoryLocationName = "--SELECT--";
                    inv.Insert(0, tmp);
                    cmbLocation.DataSource = inv;
                    cmbLocation.Refresh();
                }

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
                MessageBox.Show(LanguageManager.Error_encounter_ +  ex.Message);
            }
        }

        private void BindGrid()
        {
            try
            {
                if (cmbLocation.SelectedIndex == 0)
                {                 
                    cmbLocation.Focus();
                    dgvStock.DataSource = null;
                    dgvStock.Refresh();
                    txtQtyDisc.Text = "";
                    txtValueDisc.Text = "";

                    return;
                }
                else
                {
                    DataTable dt;
                    StockTakeController st = new StockTakeController();
                    dt = st.FetchByLocationWithFilter(((InventoryLocation)cmbLocation.SelectedValue).InventoryLocationID,showCostPrice, txtFilter.Text );
                    dgvStock.DataSource = dt;
                    dgvStock.Refresh();

                    //Get The Sum
                    if (dt.Rows.Count > 0)
                    {
                        if (showCostPrice) txtValueDisc.Text = (decimal.Parse(dt.Compute("SUM(TotalCost)", "").ToString())).ToString("N2");
                        else txtValueDisc.Visible = false;
                        txtQtyDisc.Text = (decimal.Parse(dt.Compute("SUM(Defi)", "").ToString())).ToString("0.####");
                    }
                    else
                    {
                        if (showCostPrice)  txtValueDisc.Text = "0.00";
                        else txtValueDisc.Visible = false;
                        txtQtyDisc.Text = "0";
                    }


                }
                return;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show(LanguageManager.Error_encounter_ + ex.Message);
            }
        }

        private void cmbLocation_SelectedIndexChanged(object sender, EventArgs e)
        {            
            BindGrid();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbLocation.SelectedIndex == 0)
                {
                    MessageBox.Show(LanguageManager.Please_select_the_location_);
                    cmbLocation.Focus();
                    return;
                }

                if (IsLineItemEmpty())
                {
                    MessageBox.Show(LanguageManager.Please_tick_the_item_you_wish_to_process);
                    return;
                }

                //show panel please wait...
                pnlProgress.Visible = true;
                this.Enabled = false;
                bgSearch.RunWorkerAsync(((InventoryLocation)cmbLocation.SelectedValue).InventoryLocationID);
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show(LanguageManager.Error_encounter_ + ex.Message);
            }
        }
        private void btnExport_Click(object sender, EventArgs e)
        {

            if (dgvStock != null && dgvStock.Rows.Count > 0)
            {
                CommonUILib.displayTransparent();fsdExportToExcel.ShowDialog();CommonUILib.hideTransparent();
            }
            else
            {
                MessageBox.Show(LanguageManager.There_is_no_item_to_be_printed_);
            }            
        }
        private void fsdExportToExcel_FileOk(object sender, CancelEventArgs e)
        {
            ExportController.ExportToExcel(dgvStock, fsdExportToExcel.FileName);
            MessageBox.Show(LanguageManager.File_saved);
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnDeleteChecked_Click(object sender, EventArgs e)        
        {
            try
            {
                DialogResult dr = MessageBox.Show(LanguageManager.Are_you_sure_you_want_to_delete_this_item_, "", MessageBoxButtons.YesNo);
                if (dr == DialogResult.No)
                {
                    return;
                }

                //
                for (int i = 0; i < dgvStock.Rows.Count; i++)
                {
                    if (dgvStock.Rows[i].Cells[0].Value != null &&
                        dgvStock.Rows[i].Cells[0].Value.ToString().ToLower() == "true")
                    {
                        int tmpStockTakeID;
                        if (int.TryParse(dgvStock.Rows[i].Cells["StockTakeID"].Value.ToString(), out tmpStockTakeID))
                        {
                            if (!PointOfSaleInfo.IntegrateWithInventory)
                            {
                                SyncClientController.Load_WS_URL();
                                PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                                ws.Timeout = 100000;
                                ws.Url = SyncClientController.WS_URL;
                                ws.DeleteStockTake(tmpStockTakeID);
                                //StockTake.Delete(tmpStockTakeID);
                            }
                            else
                            {
                                //StockTake.Delete(tmpStockTakeID);
                                StockTake.DeleteLogically(tmpStockTakeID);
                            }
                        }
                    }
                }
                if (!PointOfSaleInfo.IntegrateWithInventory)
                {
                    bgDownload.RunWorkerAsync();
                }
                else
                {
                    BindGrid();
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show(LanguageManager.Error_encounter_ + ex.Message);
            }
        }
        private void bgSearch_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                bool isSuccess = false;
                if (!PointOfSaleInfo.IntegrateWithInventory)
                {
                    List<string> markedList = new List<string>();
                    for (int q = 0; q < dgvStock.Rows.Count; q++)
                    {
                        if ((bool)dgvStock.Rows[q].Cells[0].Value == true)
                        {
                            markedList.Add(dgvStock.Rows[q].Cells["StockTakeID"].Value.ToString());
                        }
                    } 
                    SyncClientController.Load_WS_URL();
                    PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                    //ws.Timeout = 100000;
                    ws.Timeout = 300000;
                    ws.Url = SyncClientController.WS_URL;

                    string markedL = Newtonsoft.Json.JsonConvert.SerializeObject(markedList);
                    string status = "";

                    if (ws.AdjustStockTakeDiscrepancyNew(UserInfo.username, markedL, (int)e.Argument, out status))
                    {
                        e.Result = true;                        
                    }
                    else
                    {
                        if (status.ToLower().Contains("timeout") || status.ToLower().Contains("time out"))
                        {
                            bool isContinue = true;

                            while (isContinue)
                            {
                                int adjusted = 0;
                                if (ws.GetTotalOfAdjustedItem(markedL, out adjusted))
                                {
                                    if (adjusted != markedList.Count)
                                    {
                                        if (ws.CorrectStockTakeDiscrepancy(UserInfo.username, (int)e.Argument, out status))
                                        {
                                            isContinue = false;
                                            e.Result = true;
                                            return;
                                        }
                                        else
                                        {
                                            if (status.ToLower().Contains("timeout") || status.ToLower().Contains("time out"))
                                            {
                                                isContinue = true;
                                            }
                                            else
                                            {
                                                isContinue = false;
                                                e.Result = false;
                                                return;
                                            }
                                        }
                                    }

                                }
                                else
                                {
                                    isContinue = false;
                                    e.Result = false;
                                    return;
                                }
                            }
                        }
                        else
                        {

                            e.Result = false;
                            return;
                        }
                    }
                }
                else
                {
                    StockTakeController ct = new StockTakeController();
                    e.Result = ct.CorrectStockTakeDiscrepancy
                                (UserInfo.username, (int)e.Argument);
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);                                
            }
        }

        private void bgSearch_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (e.Result is bool && (bool)e.Result)
                {
                    MessageBox.Show(LanguageManager.Stock_Take_has_been_adjusted_);
                    this.Close();
                    return;
                }
                else
                {
                    MessageBox.Show(LanguageManager.ERROR_adjusting_stock_take__Please_try_again__If_the_problem_persist__please_contact_your_system_administrator_);
                    pnlProgress.Visible = false;
                    this.Enabled = true;
                    return;
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                this.Enabled = true;
                MessageBox.Show(LanguageManager.Error_encounter_ + ex.Message);
            }
        }

        private void bgSearch_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pgb1.PerformStep();
        }

        private void dgvStock_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == 1)
                {
                    frmItemTraceTool f = new frmItemTraceTool();
                    f.txtItemNo.Text = dgvStock.Rows[e.RowIndex].Cells[1].Value.ToString();
                    f.ShowDialog();
                    f.Dispose();
                }
        }
        private bool IsLineItemEmpty()
        {
            try
            {
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
                MessageBox.Show(LanguageManager.Error_encounter_ + ex.Message);
                return false;
            }
        }
        private void dgvStock_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && e.ColumnIndex == 0)
                {
                    bool value = false;
                    int stockTakeID;
                    if (int.TryParse(dgvStock.Rows[e.RowIndex].Cells["StockTakeID"].Value.ToString(), out stockTakeID))
                    {
                        StockTakeController st = new StockTakeController();
                        if (dgvStock.Rows[e.RowIndex].Cells[0].Value != null
                            && dgvStock.Rows[e.RowIndex].Cells[0].Value is bool)
                        {
                            value = !(bool)dgvStock.Rows[e.RowIndex].Cells[0].Value;
                        }
                        else
                        {
                            value = true;
                        }
                        if (!st.updateMarked(stockTakeID, value))
                        {
                            MessageBox.Show(LanguageManager.Update_failed__Please_try_again);
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
        private void llSelectAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                for (int i = 0; i < dgvStock.Rows.Count; i++)
                {
                    int stockTakeID;
                    if (int.TryParse(dgvStock.Rows[i].Cells["StockTakeID"].Value.ToString(), out stockTakeID))
                    {
                        dgvStock.Rows[i].Cells[0].Value = true;
                        StockTakeController st = new StockTakeController();
                        st.updateMarked(stockTakeID, true);
                    }
                }
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
                for (int i = 0; i < dgvStock.Rows.Count; i++)
                {
                    int stockTakeID;
                    if (int.TryParse(dgvStock.Rows[i].Cells["StockTakeID"].Value.ToString(), out stockTakeID))
                    {
                        dgvStock.Rows[i].Cells[0].Value = false;
                        StockTakeController st = new StockTakeController();
                        st.updateMarked(stockTakeID, false);
                    }
                }
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
                for (int i = 0; i < dgvStock.Rows.Count; i++)
                {
                    int stockTakeID;
                    if (int.TryParse(dgvStock.Rows[i].Cells["StockTakeID"].Value.ToString(), out stockTakeID))
                    {

                        StockTakeController st = new StockTakeController();

                        if (dgvStock.Rows[i].Cells[0].Value != null
                            && dgvStock.Rows[i].Cells[0].Value.ToString().ToLower() == "true")
                        {
                            dgvStock.Rows[i].Cells[0].Value = false;
                            st.updateMarked(stockTakeID, false);
                        }
                        else
                        {
                            dgvStock.Rows[i].Cells[0].Value = true;
                            st.updateMarked(stockTakeID, true);
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


        private void dgvStock_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            /*
            if (e.RowIndex >= 0 && e.ColumnIndex == 0)
            {
                bool value = false;
                int stockTakeID;
                if (int.TryParse(dgvStock.Rows[e.RowIndex].Cells["StockTakeID"].Value.ToString(), out stockTakeID))
                {
                    StockTakeController st = new StockTakeController();
                    if (dgvStock.Rows[e.RowIndex].Cells[0].Value != null
                        && dgvStock.Rows[e.RowIndex].Cells[0].Value is bool)
                    {
                        value = !(bool)dgvStock.Rows[e.RowIndex].Cells[0].Value;
                    }
                    else
                    {
                        value = true;
                    }
                    if (!st.updateMarked(stockTakeID, value))
                    {
                        MessageBox.Show("Update failed. Please try again");
                    }
                }
            }*/
        }

        private void frmAdjustStockTake_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }

        void sync_OnProgressUpdates(object sender, string message)
        {
            //this.lblStatus.Text = message;
            //addLog("product", message);
            this.Invoke((MethodInvoker)delegate
            {
                lblStatus.Text = message; // runs on UI thread
            });
        }

        private void bgDownload_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //download Stock Take
                SyncClientController.Load_WS_URL();
                SyncInventoryRealTimeController sync = new SyncInventoryRealTimeController();
                sync.OnProgressUpdates += new PowerPOS.InventoryRealTimeController.UpdateProgress(sync_OnProgressUpdates);
                bool result = sync.GetRealTimeStockTake();
                e.Result = result;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                //MessageBox.Show("Error: " + ex.Message);  

                // if FK conflict then throw error
                if (ex.GetType().ToString() == "System.Data.SqlClient.SqlException")
                {
                    if (((System.Data.SqlClient.SqlException)ex).Number == 547)
                    {
                        MessageBox.Show("You need to download latest Items from Server, please go to Setup Menu and Sync.");
                    }
                }
            }
        }
        private void bgDownload_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                pnlProgress.Visible = false;
                this.Enabled = true;
                if (!(bool)e.Result)
                {
                    MessageBox.Show(LanguageManager.Error_loading_inventory_from_the_web__Please_download_all_data_from_server_first_);
                    this.Close();
                }
                else
                {
                    if (cmbLocation.Items.Count == 0)
                    {
                        InventoryLocationCollection inv = StockTakeController.GetAllLocationWithOutstandingStockTake();
                        if (inv.Count == 0)
                        {
                            MessageBox.Show(LanguageManager.There_is_no_Stock_Take_data_to_adjust__All_Stock_Take_has_been_adjusted);

                        }
                        InventoryLocation tmp = new InventoryLocation();
                        tmp.InventoryLocationID = 0;
                        tmp.InventoryLocationName = "--SELECT--";
                        inv.Insert(0, tmp);
                        cmbLocation.DataSource = inv;
                        cmbLocation.Refresh();
                    }
                    else
                    {
                        BindGrid();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show(LanguageManager.Error_encounter_ + ex.Message);              
            }
        }

        private void txtFilter_Validated(object sender, EventArgs e)
        {
            BindGrid();
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {

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
                        string itemNo = row.Cells["dataGridViewTextBoxColumn1"].Value.ToString();
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
                                string itemNo = dgvStock.Rows[e.RowIndex].Cells["dataGridViewTextBoxColumn1"].Value.ToString();
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
