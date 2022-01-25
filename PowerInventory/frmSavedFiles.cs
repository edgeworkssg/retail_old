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
using System.Collections;
using SubSonic;
using LanguageManager = PowerInventory.Properties.InventoryLanguage;


namespace PowerInventory
{
    public partial class frmSavedFiles : Form
    {
        public frmSavedFiles()
        {
            InitializeComponent();
            dgvStock.AutoGenerateColumns = false;
        }

        private void frmSavedFiles_Load(object sender, EventArgs e)
        {
            BindGrid();
        }
        private void BindGrid()
        {
            string[] movementTypes = {"Goods Receive","Stock Issue","Stock Transfer","Stock Adjustment","Stock Take","Stock Return"};

            SavedFileCollection col = new SavedFileCollection();
            col.Where(SavedFile.Columns.Deleted, false);
            ArrayList ar = new ArrayList();
            for (int i = 0; i < movementTypes.Length; i++)
            {
                if (PrivilegesController.HasPrivilege(movementTypes[i], UserInfo.privileges))
                {
                    if (movementTypes[i] == "Stock Adjustment")
                    {
                        ar.Add("Adjustment");                        
                    }
                    else if (movementTypes[i] == "Stock Transfer")
                    {
                        ar.Add("Transfer");                        
                    }
                    else
                    {
                        ar.Add(movementTypes[i]);
                    }
                }
            }

            
            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.SaveFileInTheServer), false))
            {
                string list = "";
                for (int i = 0; i < ar.Count; i++)
                {
                    list = list + ar[i].ToString() + ",";
                }
                list = list.Substring(0, list.Length - 1);
                DataSet ds = SyncClientController.GetSavedInventoryFile(list);
                if (ds.Tables.Count > 0)
                {
                    dgvStock.DataSource = ds.Tables[0];
                    dgvStock.Refresh();
                }
            }
            else
            {
                col.Where(SavedFile.Columns.MovementType, SubSonic.Comparison.In, ar);
                col.OrderByDesc(SavedFile.Columns.SavedDate);
                col.Load();
                dgvStock.DataSource = col.ToDataTable();
                dgvStock.Refresh();
            }
            
            
        }

        private void dgvStock_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex == 0)
            {
                
                //select what type and open the right form accordingly....
                if (dgvStock["MovementType", e.RowIndex].Value.ToString() == "GOODS RECEIVE")
                {
                    frmStockIn f = new frmStockIn();

                    f.setFileName(dgvStock["Filename", e.RowIndex].Value.ToString(), dgvStock["SavedRemark", e.RowIndex].Value.ToString());
                    f.WindowState = FormWindowState.Maximized;
                    f.Show();
                    f.BringToFront();

                    this.Close();
                    
                }
                else if (dgvStock["MovementType", e.RowIndex].Value.ToString() == "STOCK ISSUE")
                {
                    
                    frmStockOut f = new frmStockOut();
                    f.setFileName(dgvStock["Filename", e.RowIndex].Value.ToString(), dgvStock["SavedRemark", e.RowIndex].Value.ToString());
                    f.WindowState = FormWindowState.Maximized;
                    f.Show();
                    f.BringToFront();
                    this.Close();
                }
                else if (dgvStock["MovementType", e.RowIndex].Value.ToString() == "TRANSFER")
                {
                    
                    frmStockTransfer f = new frmStockTransfer();
                    f.setFileName(dgvStock["Filename", e.RowIndex].Value.ToString(), dgvStock["SavedRemark", e.RowIndex].Value.ToString());
                    f.WindowState = FormWindowState.Maximized;
                    f.Show();
                    f.BringToFront();
                    this.Close();
                    
                }
                else if (dgvStock["MovementType", e.RowIndex].Value.ToString() == "ADJUSTMENT")
                {
                    frmAdjustStock f = new frmAdjustStock();
                    f.setFileName(dgvStock["Filename", e.RowIndex].Value.ToString(), dgvStock["SavedRemark", e.RowIndex].Value.ToString());
                    f.WindowState = FormWindowState.Maximized;
                    f.Show();
                    f.BringToFront();
                    this.Close();
                }
                else if (dgvStock["MovementType", e.RowIndex].Value.ToString() == "STOCK TAKE")
                {
                    
                    frmStockTake f = new frmStockTake();
                    f.setFileName(dgvStock["Filename", e.RowIndex].Value.ToString(), dgvStock["SavedRemark", e.RowIndex].Value.ToString());
                    f.WindowState = FormWindowState.Maximized;
                    f.Show();
                    f.BringToFront();
                    this.Close();
                }
                else if (dgvStock["MovementType", e.RowIndex].Value.ToString() == "STOCK RETURN")
                {

                    frmStockOutReturn f = new frmStockOutReturn();
                    f.setFileName(dgvStock["Filename", e.RowIndex].Value.ToString(), dgvStock["SavedRemark", e.RowIndex].Value.ToString());
                    f.WindowState = FormWindowState.Maximized;
                    f.Show();
                    f.BringToFront();
                    this.Close();
                }
            }
            if (e.RowIndex > -1 && e.ColumnIndex == 1)
            {
                string saveName = dgvStock["Filename", e.RowIndex].Value.ToString();
                DialogResult dr = MessageBox.Show("All your saved files will be deleted! Are you sure you want to delete file? ", "", MessageBoxButtons.YesNo);
                if (dr == DialogResult.Yes)
                {
                    try
                    {
                        string status = "";

                        if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.SaveFileInTheServer), false))
                        {
                            SyncClientController.Load_WS_URL();
                            PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                            ws.Timeout = 1000000;
                            ws.Url = SyncClientController.WS_URL;
    
                            if(!ws.DeleteSavedFileByName(saveName, out status))
                                throw new Exception(status);
                            else
                            {
                                MessageBox.Show(LanguageManager.Saved_Files_Updated);
                                BindGrid();
                            }
                        }
                        else
                        {
                            if (!SavedFileController.DeleteSavedFileByName(saveName, out status))
                                throw new Exception(status);
                            else
                            {
                                MessageBox.Show(LanguageManager.Saved_Files_Updated);
                                BindGrid();
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.writeLog(ex);
                        MessageBox.Show(LanguageManager.An_unknown_error_has_been_encountered__Please_contact_your_system_administrator_ + ex.Message);
                    }

                }

            }
        }

        private void btnDeleteAll_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("All your saved files will be deleted! Are you sure you want to delete all? ", "",MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                try
                {
                    string status = "";

                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.SaveFileInTheServer), false))
                    {
                        pnlProgress.Visible = true;
                        this.Enabled = false;
                        bgWorker.RunWorkerAsync();
                    }
                    else{
                        if (!SavedFileController.DeleteSavedFileAll(out status))
                            throw new Exception(status);
                        else
                        {
                            MessageBox.Show(LanguageManager.Saved_Files_Updated);
                            BindGrid();
                        }

                    }
                }
                catch (Exception ex)
                {
                    Logger.writeLog(ex);
                    MessageBox.Show(LanguageManager.An_unknown_error_has_been_encountered__Please_contact_your_system_administrator_ + ex.Message);
                }
               
            }
        }

        private bool isSuccessSendSave = false;

        private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            string status = "";
            try 
            {
                SyncClientController.Load_WS_URL();
                PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                ws.Timeout = 1000000;
                ws.Url = SyncClientController.WS_URL;

                if (!ws.DeleteSavedFileAll(out status))
                    throw new Exception(status);
                else
                {
                    isSuccessSendSave = true;
                }
                
            }
            catch (Exception ex)
            {
                isSuccessSendSave = false;
                Logger.writeLog(ex);
                MessageBox.Show(LanguageManager.An_unknown_error_has_been_encountered__Please_contact_your_system_administrator_ + ex.Message);
            }
        }

        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            pnlProgress.Visible = false;
            this.Enabled = true;

            if (isSuccessSendSave)
            {
                MessageBox.Show(LanguageManager.Saved_Files_Updated);
                BindGrid();
            }
            else
                MessageBox.Show(LanguageManager.An_unknown_error_has_been_encountered__Please_contact_your_system_administrator_);
        }
    }
}
