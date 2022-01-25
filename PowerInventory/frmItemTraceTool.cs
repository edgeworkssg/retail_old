using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SubSonic;
using PowerPOS;
using PowerPOS.Container;
namespace PowerInventory
{
    public partial class frmItemTraceTool : Form
    {
        public frmItemTraceTool()
        {
            InitializeComponent();
            dtpEndDate.Value = DateTime.Now;
            dtpStartDate.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = InventoryController.FetchItemTrace(true, true, dtpStartDate.Value, dtpEndDate.Value, txtItemNo.Text);
                if (!PointOfSaleInfo.IntegrateWithInventory)
                {
                    SyncClientController.Load_WS_URL();
                    PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                    ws.Timeout = 100000;
                    ws.Url = SyncClientController.WS_URL;
                    byte[] data = ws.FetchItemTraceReport(dtpStartDate.Value, dtpEndDate.Value, txtItemNo.Text);
                    DataSet myDataSet = SyncClientController.DecompressDataSetFromByteArray(data);
                    if (myDataSet.Tables.Count > 0)
                        dt = myDataSet.Tables[0];
                }
                DataView dtView = new DataView();
                dtView.Table = dt;
                dtView.Sort = "Date";
                dgvResult.DataSource = dtView;
            }catch (Exception ex)
            {
                Logger.writeLog(ex.Message);
            }

        }
    }
}
