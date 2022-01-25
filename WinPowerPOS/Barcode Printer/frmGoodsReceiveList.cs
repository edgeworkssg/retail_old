using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PowerPOS.Container;
using PowerPOS;

namespace WinPowerPOS.BarcodePrinter
{
    public partial class frmGoodsReceiveList : Form
    {
        public frmGoodsReceiveList()
        {
            InitializeComponent();
        }
        public string RefNo;
        private void frmGoodsReceiveList_Load(object sender, EventArgs e)
        {
            CommonUILib.FormatDateFilter(ref dtpStartDate, ref dtpEndDate);

            InventoryLocationCollection inv = new InventoryLocationCollection();
            inv.Where(PowerPOS.InventoryLocation.Columns.Deleted, false);
            inv.Load();
            InventoryLocation tmpInv = new InventoryLocation();
            tmpInv.InventoryLocationName = "ALL";
            inv.Insert(0, tmpInv);
            cmbLocation.DataSource = inv;
            cmbLocation.Refresh();
            cmbLocation.SelectedIndex = 0;
            this.WindowState = FormWindowState.Maximized;
            populateItem();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;

            if (e.ColumnIndex == dataGridView1.Columns[CButton.Name].Index)
            {
                //Tabel[dgQuantity.Name, e.RowIndex].Value = int.Parse(Tabel[dgQuantity.Name, e.RowIndex].Value.ToString()) + 1;
                RefNo = dataGridView1.Rows[e.RowIndex].Cells["CRefNo"].Value.ToString();
                DialogResult = DialogResult.Yes;
            }
        }

        private bool UseStartDate, UseEndDate;
        private DateTime StartDate, EndDate;
        private string movementType = "Stock In";
        private string inventoryLocationID;

        private void bgSearch_DoWork(object sender, DoWorkEventArgs e)
        {

            try
            {
                DataTable dt = new DataTable();
                if (PointOfSaleInfo.IntegrateWithInventory)
                {
                    dt =
                    ReportController.FetchInventoryActivityHeader
                    (UseStartDate, UseEndDate,
                     StartDate, EndDate, "%%",
                     inventoryLocationID, movementType, "", "", "",
                     PrivilegesController.HasPrivilege("Goods Receive", UserInfo.privileges));
                }
                else
                {
                    SyncClientController.Load_WS_URL();
                    PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                    ws.Timeout = 100000;
                    ws.Url = SyncClientController.WS_URL;
                    byte[] data = ws.FetchInventoryActivityHeader(UseStartDate, UseEndDate,
                     StartDate, EndDate, "%%",
                     inventoryLocationID, movementType, "", "", "",
                     PrivilegesController.HasPrivilege("Goods Receive", UserInfo.privileges));
                    DataSet myDataSet = SyncClientController.DecompressDataSetFromByteArray(data);
                    if (myDataSet.Tables.Count > 0)
                    {
                        dt = myDataSet.Tables[0];
                    }
                }

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
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataView view = new System.Data.DataView(dt);
                    DataTable selected = view.ToTable(true, "InventoryHdrRefNo", "InventoryDate", "UserName", "InventoryLocationName", "Remark");

                    dataGridView1.DataSource = selected;
                    dataGridView1.Refresh();
                }
                pnlProgress.Visible = false;
                this.Enabled = true;
                pnlProgress.Visible = false;
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

        private void populateItem()
        {
            UseStartDate = dtpStartDate.Checked;
            UseEndDate = dtpEndDate.Checked;
            StartDate = dtpStartDate.Value;
            EndDate = dtpEndDate.Value;
            inventoryLocationID = ((InventoryLocation)(cmbLocation.SelectedValue)).InventoryLocationID.ToString();
            bgSearch.RunWorkerAsync();
            this.Enabled = false;
            pnlProgress.Visible = true;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            populateItem();
        }

        private void cmbLocation_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }
}
