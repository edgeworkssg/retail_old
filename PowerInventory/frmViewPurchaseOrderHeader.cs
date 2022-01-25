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
using SubSonic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace PowerInventory.InventoryForms
{
    public partial class frmViewPurchaseOrderHeader: Form
    {                                
        ItemController itemLogic;
        public string searchQueryString;
        private OpenFileDialog openFileAttachment;
        private FileAttachmentCollection attachColl = new FileAttachmentCollection();
        private string tempAttachmentFolder;
        private string purchaseOrderHdrRefNo;

        #region "Form event handler"
        string existingConnectionString;
        public frmViewPurchaseOrderHeader()
        {
            existingConnectionString = DataService.GetInstance("POWERPOS").DefaultConnectionString;
            InitializeComponent();            
            itemLogic = new ItemController();                        
            AssignPrivileges();            
        }        
        private void OrderTaking_Activated(object sender, EventArgs e)
        {                        

        }
        private void OrderTaking_Load(object sender, EventArgs e)
        {
            try
            {
                if (!PointOfSaleInfo.IntegrateWithInventory)
                {
                    this.Enabled = false;
                    pnlProgress.Visible = true;
                    bgDownload.RunWorkerAsync();
                }
                CommonUILib.FormatDateFilter(ref dtpStartDate, ref dtpEndDate);

                SyncClientController.Load_WS_URL();
                PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                ws.Url = SyncClientController.WS_URL;


                //txtBarcode.Focus();                        
                dgvPurchase.AutoGenerateColumns = false;
                InventoryLocationCollection inv = new InventoryLocationCollection();
                inv.Where(PowerPOS.InventoryLocation.Columns.Deleted, false);
                inv.Load();
                //cmbMovementType.SelectedIndex = 0;
                InventoryLocation tmpInv = new InventoryLocation();
                tmpInv.InventoryLocationName = "ALL";
                inv.Insert(0, tmpInv);
                cmbLocation.DataSource = inv;
                cmbLocation.Refresh();
                cmbLocation.SelectedIndex = 0;

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
                drSupplier["SupplierName"] = "ALL";
                dtSortedSupplier.Rows.InsertAt(drSupplier, 0);
                dtSortedSupplier.AcceptChanges();
                cmbSupplier.DataSource = dtSortedSupplier;
                cmbSupplier.DisplayMember = "SupplierName";
                cmbSupplier.ValueMember = "SupplierID";

                this.WindowState = FormWindowState.Maximized;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("Error: " + ex.Message);                
            }
        }
        #endregion

        #region "Privileges Related"
        private void AssignPrivileges()
        {
            /*
            if (!PrivilegesController.HasPrivilege(PrivilegesController.INVENTORY_TRANSACTION, UserInfo.privileges))
            {                
                txtBarcode.Enabled = false;
            }*/
        }
        #endregion

        #region "button handler"
        private void btnLogOut_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure you want to quit?", "", MessageBoxButtons.OKCancel);
            if (dr == DialogResult.OK)
            {                
                this.Close();
            }
        }
        #endregion

        #region "Quick Access Buttons & Programmable Keyboard"
        private void frmOrderTaking_KeyDown(object sender, KeyEventArgs e)
        {
            mapKeyPress(e);
        }
        
        private void mapKeyPress(KeyEventArgs e)
        {
            
        }                        

        #endregion
                                       
        #region "DataGridView - Editing of qty in the cell"

        private void dgvPurchase_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {            
        }

        private void populateOrderItemGridView()
        {
            try
            {
                //show panel please wait...
                pnlProgress.Visible = true;
                dgvPurchase.DataSource = null;
                dgvPurchase.Refresh();
                inventoryLocationID = ((InventoryLocation)(cmbLocation.SelectedValue)).InventoryLocationID.ToString();
                //movementType = cmbMovementType.SelectedItem.ToString();
                userName = txtUserName.Text;                 
                UseStartDate = dtpStartDate.Checked;
                UseEndDate = dtpEndDate.Checked;
                StartDate = dtpStartDate.Value;
                EndDate = dtpEndDate.Value;
                remark = txtRemark.Text;
                supplierName = cmbSupplier.Text == "ALL" ? "" : cmbSupplier.Text;
                bgSearch.RunWorkerAsync();
                this.Enabled = false;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("An unknown error has been encountered. Please contact your system administrator.");
                
            }
        }
        private bool GetValueFromRow(out int qty, DataGridViewCellEventArgs e)
        {
            try
            {
                qty = -1;

                if (!int.TryParse(dgvPurchase.Rows[e.RowIndex].Cells["Quantity"].Value.ToString(), out qty) || qty < 0)
                {
                    MessageBox.Show("You need to enter a non negative number for quantity");
                    dgvPurchase.CancelEdit();
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("Error: " + ex.Message);
                qty = 0;
                return false;
            }
        }
       
        #endregion                                          

        private void btnExit_Click(object sender, EventArgs e)
        {         
            this.Close();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = (DataTable)dgvPurchase.DataSource;
                if (dt != null && dt.Rows.Count > 0)
                {
                    CommonUILib.displayTransparent(); fsdExportToExcel.ShowDialog(); CommonUILib.hideTransparent();
                }
                else
                {
                    MessageBox.Show("There is no data to export");
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("Error: " + ex.Message);                
            }
        }

        private void fsdExportToExcel_FileOk(object sender, CancelEventArgs e)
        {
            try
            {
                ExportController.ExportToExcel(dgvPurchase, fsdExportToExcel.FileName);
                MessageBox.Show("File saved");
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("Error: " + ex.Message);                
            }
        }
        

        private void btnSearch_Click(object sender, EventArgs e)
        {
            populateOrderItemGridView();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private bool UseStartDate, UseEndDate;
        private DateTime StartDate, EndDate;
        private string userName;
        private string movementType;
        private string searchQuery;
        private string remark;
        private string inventoryLocationID;
        private string supplierName;
        private void bgSearch_DoWork(object sender, DoWorkEventArgs e)
        {

            try
            {
                DataTable dt = 
                    ReportController.FetchPurchaseOrderHeader
                    (UseStartDate, UseEndDate,
                     StartDate, EndDate, "%" + userName + "%",
                     inventoryLocationID, movementType, remark, "", "",
                     PrivilegesController.HasPrivilege("Purchase Order", UserInfo.privileges), supplierName);

                e.Result = dt;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                e.Result = null;
            }
        }

        private void bgSearch_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                DataTable dt = (DataTable)e.Result;
                dgvPurchase.DataSource = dt;
                dgvPurchase.Refresh();
                pnlProgress.Visible = false;
                this.Enabled = true;
                if (dt == null || dt.Rows.Count == 0)
                {
                    MessageBox.Show("There is no data for the given search criteria.");
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

        private void dgvPurchase_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0) return;

                purchaseOrderHdrRefNo = dgvPurchase.Rows[e.RowIndex].Cells["RefNo"].Value.ToString();

                if (e.ColumnIndex == 0)
                {
                    //Show Inventory Activity Sheet?
                    PurchaseOdrController inv = new PurchaseOdrController();
                    inv.LoadConfirmedPurchaseOrderController(purchaseOrderHdrRefNo);
                    //inv.LoadConfirmedInventoryController(dgvPurchase.Rows[e.RowIndex].Cells["RefNo"].Value.ToString());

                    Supplier supp = new Supplier(inv.getSupplier());
                    
                    frmPurchaseOrder.PrintPurchaseOrderSheet
                        (inv, supp.SupplierName, inv.GetFreightCharges().ToString("N2"),
                        inv.getDiscount().ToString("N2"), inv.getExchangeRate().ToString("N2"), 
                        true, PrivilegesController.HasPrivilege("PURCHASE ORDER", UserInfo.privileges));
                }
                else if (dgvPurchase.Columns[e.ColumnIndex] == colEdit)
                {
                    PurchaseOdrController inv = new PurchaseOdrController();
                    
                    if (!inv.LoadConfirmedPurchaseOrderController(purchaseOrderHdrRefNo))
                    {
                        MessageBox.Show("PO with number " + purchaseOrderHdrRefNo + " doesn't exist ");
                        return;
                    }

                    if (inv.InvHdr.Status != "Rejected" && inv.InvHdr.Status != "Submitted" && inv.InvHdr.Status != "")
                    {
                        MessageBox.Show("Only can Edit PO with status Rejected or Submitted");
                        return;
                    }

                    frmPurchaseOrderNew fPO = new frmPurchaseOrderNew(purchaseOrderHdrRefNo);
                    fPO.ShowDialog();
                    fPO.Dispose();

                    if (!PointOfSaleInfo.IntegrateWithInventory)
                    {
                        this.Enabled = false;
                        pnlProgress.Visible = true;
                        bgDownload.RunWorkerAsync();
                    }

                    populateOrderItemGridView();
                   
                }   
                else if (dgvPurchase.Columns[e.ColumnIndex] == colAttachment)
                {
                    BindAttachment();
                    ShowPanelAttachment();
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("Error: " + ex.Message);                
            }
        }

        private void BindAttachment()
        {
            try
            {
                attachColl = new FileAttachmentCollection();
                attachColl.Where(FileAttachment.Columns.ModuleName, "PurchaseOrder");
                attachColl.Where(FileAttachment.Columns.RefID, purchaseOrderHdrRefNo);
                attachColl.Where(FileAttachment.Columns.Deleted, false);
                attachColl.Load();
                dgvcRemove.DefaultCellStyle.NullValue = "Remove";
                dgvAttachment.AutoGenerateColumns = false;
                dgvAttachment.DataSource = attachColl;
                //foreach (DataGridViewRow row in dgvAttachment.Rows)
                //{
                //    row.Cells["dgvcRemove"].Value = "Remove";
                //}
                tempAttachmentFolder = Guid.NewGuid().ToString();
            }
            catch (Exception ex)
            {
                pnlAttachment.Visible = false;
                Logger.writeLog(ex);
                MessageBox.Show("Error encounter." + ex.Message);
            }
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

        private void frmViewActivityHeader_FormClosed(object sender, FormClosedEventArgs e)
        {
            //DataService.GetInstance("PowerPOS").DefaultConnectionString = existingConnectionString;
        }
        private void bgDownload_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //download PurchaseOrderHdr and PurchaseOrderDet
                SyncClientController.Load_WS_URL();
                bool result = SyncClientController.GetCurrentPurchaseOrder();
                e.Result = result;

                PowerPOS.SyncFileAttachmentController.SyncFileAttachmentRealTimeController sfa = new PowerPOS.SyncFileAttachmentController.SyncFileAttachmentRealTimeController();
                sfa.Load_WS_URL();
                sfa.SyncFileAttachment("PurchaseOrder");
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
        private void bgDownload_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                pnlProgress.Visible = false;
                if (!(bool)e.Result)
                {
                    MessageBox.Show("Error loading inventory from the web. Please download all data from server first.");
                    this.Close();
                }
                this.Enabled = true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("Error: " + ex.Message);                
            }

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void btnCloseAttachment_Click(object sender, EventArgs e)
        {
            pnlAttachment.Visible = false;
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

                attachColl.Add(new FileAttachment()
                {
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

        private void dgvAttachment_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    var attId = dgvAttachment.Rows[e.RowIndex].Cells["dgvcAttachmentID"].Value.ToString();

                    if (dgvAttachment.Columns[e.ColumnIndex].Name == "dgvcRemove")
                    {
                        FileAttachment fa = attachColl.First(a => a.AttachmentID.ToString() == attId);
                        fa.Deleted = !fa.Deleted;

                        for (int j = 0; j < dgvAttachment.ColumnCount; ++j)
                        {
                            if (dgvAttachment.Columns[j].Visible == true && fa.Deleted == true)
                            {
                                if (dgvAttachment.Columns[j].Name == "dgvcRemove") dgvAttachment.Rows[e.RowIndex].Cells[j].Value = "Undo Remove";
                                dgvAttachment.Rows[e.RowIndex].Cells[j].Style.ForeColor = System.Drawing.Color.White;
                                dgvAttachment.Rows[e.RowIndex].Cells[j].Style.BackColor = System.Drawing.Color.DarkRed;
                            }
                            else
                            {
                                if (dgvAttachment.Columns[j].Name == "dgvcRemove") dgvAttachment.Rows[e.RowIndex].Cells[j].Value = "Remove";
                                dgvAttachment.Rows[e.RowIndex].Cells[j].Style.ForeColor = System.Drawing.Color.Black;
                                dgvAttachment.Rows[e.RowIndex].Cells[j].Style.BackColor = System.Drawing.Color.White;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("Error occured: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSaveAttachment_Click(object sender, EventArgs e)
        {
            FileAttachmentCollection toDelete = new FileAttachmentCollection();
            FileAttachmentCollection toAdd = new FileAttachmentCollection();
            string status;

            foreach (FileAttachment fa in attachColl)
            {
                if (!fa.IsNew && fa.Deleted.GetValueOrDefault(false))
                    toDelete.Add(fa);
                if (fa.IsNew && !fa.Deleted.GetValueOrDefault(false))
                    toAdd.Add(fa);
            }

            this.Enabled = false;
            pnlProgress.Visible = true;
            pnlProgress.BringToFront();

            SyncClientController.Load_WS_URL();
            PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
            ws.Timeout = 1000000;
            ws.Url = SyncClientController.WS_URL;

            #region *) Delete Attachment
            if (toDelete.Count > 0)
            {
                DataTable dt = toDelete.ToDataTable().DefaultView.ToTable(true, "AttachmentID");
                if (!ws.DeleteAttachment(dt, UserInfo.username, out status))
                {
                    MessageBox.Show(string.Format("Failed to delete attachment data in server. Error message: {0}", status), "Error");
                    return;
                }
            }
            #endregion

            #region *) Upload Attachment
            if (toAdd.Count > 0)
            {
                #region *) Upload the files first
                FileAttachmentCollection tmpAttColl = new FileAttachmentCollection();

                toAdd.CopyTo(tmpAttColl);
                foreach (FileAttachment fa in tmpAttColl)
                {
                    string fullPath = Path.Combine(fa.FileLocation, fa.FileName);
                    FileInfo fInfo = new FileInfo(fullPath);
                    long numBytes = fInfo.Length;
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
                    if (!ws.UploadAttachment(attachment, tempAttachmentFolder, fa.FileName, false, "PurchaseOrder", out serverPath, out status))
                    {
                        MessageBox.Show(string.Format("Failed to upload {0} to server. Error message: {1}", fa.FileName, status), "Error");
                        return;
                    }

                    fa.FileName = Path.GetFileName(serverPath);
                    fa.FileLocation = Path.GetDirectoryName(serverPath);
                }
                #endregion

                if (!ws.SaveAttachment(tmpAttColl.ToDataTable(), "PurchaseOrder", purchaseOrderHdrRefNo, UserInfo.username, out status))
                {
                    MessageBox.Show(string.Format("Failed to save new attachment data to server. Error message: {0}", status), "Error");
                    return;
                }
            }
            #endregion

            if (!PointOfSaleInfo.IntegrateWithInventory)
            {
                bgDownload.RunWorkerAsync();
            }
            else
            {
                pnlProgress.Visible = false;
                this.Enabled = true;
            }

            MessageBox.Show("Attachment saved successfully", "Information");
            pnlAttachment.Visible = false;
        }
    }
}