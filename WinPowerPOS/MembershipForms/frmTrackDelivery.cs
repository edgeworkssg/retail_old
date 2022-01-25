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
using Newtonsoft.Json;

namespace WinPowerPOS.MembershipForms
{
    public partial class frmTrackDelivery : Form
    {
        public string orderdetid {get;set;}
        public frmTrackDelivery()
        {
            InitializeComponent();
        }

        private void frmTrackDelivery_Load(object sender, EventArgs e)
        {
            SyncClientController.Load_WS_URL();
            PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
            ws.Timeout = 100000;
            ws.Url = SyncClientController.WS_URL;

            string preOrderStr = ws.DeliveryGetDeliveryTrack(orderdetid);

            DataTable preorderData = JsonConvert.DeserializeObject<DataTable>(preOrderStr);

            dgvTrackDelivery.DataSource = preorderData;
            dgvTrackDelivery.Refresh();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvTrackDelivery_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex == 0)
                {
                    var DOHdrID = dgvTrackDelivery.Rows[e.RowIndex].Cells["DOHdrID"].Value.ToString();

                    DeliveryOrder dohdr = new DeliveryOrder(DOHdrID);

                    if ((dohdr.IsDelivered != null && (bool)dohdr.IsDelivered) || dgvTrackDelivery.Rows[e.RowIndex].Cells["Delivered"].Value.ToString() == "")
                    {
                        //MessageBox.Show("The goods already delivered. You can not deliver it again.");
                        return; // Do nothing
                    }
                    else
                    {
                        if (MessageBox.Show("Are you sure to mark this as delivered ??", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            string status = "";

                            SyncClientController.Load_WS_URL();
                            PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                            ws.Timeout = 100000;
                            ws.Url = SyncClientController.WS_URL;

                            if (!ws.DeliverySetDelivered(DOHdrID, UserInfo.username, out status))
                            {
                                MessageBox.Show(status);
                                return;
                            }
                            //else
                            //{
                            //    string preOrderStr = ws.FetchDeliveryOrderToPrintByOrderDetID(orderdetid);

                            //    if (!string.IsNullOrEmpty(preOrderStr))
                            //    {
                            //        DataTable preorderData = JsonConvert.DeserializeObject<DataTable>(preOrderStr);

                            //        POSDevices.Receipt rcpt = new POSDevices.Receipt();
                            //        rcpt.PrintPreOrderDelivery(preorderData);
                            //    }


                            //    this.Close();
                            //}
                        }
                    }
                }
                else if (e.ColumnIndex == 1)
                {
                    var DOHdrID = dgvTrackDelivery.Rows[e.RowIndex].Cells["DOHdrID"].Value.ToString();

                    SyncClientController.Load_WS_URL();
                    PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                    ws.Timeout = 100000;
                    ws.Url = SyncClientController.WS_URL;

                    string preOrderStr = ws.FetchDeliveryOrderToPrintByOrderDetID(orderdetid);

                    if (!string.IsNullOrEmpty(preOrderStr))
                    {
                        DataTable preorderData = JsonConvert.DeserializeObject<DataTable>(preOrderStr);

                        POSDevices.Receipt rcpt = new POSDevices.Receipt();
                        rcpt.PrintPreOrderDelivery(preorderData);
                    }


                    this.Close();
                }
            }
        }

        private void dgvTrackDelivery_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach(DataGridViewRow row in dgvTrackDelivery.Rows)
            {
                string deliveryStatus = row.Cells["DeliveryStatus"].Value.ToString();
                if (deliveryStatus == "Delivered")
                {
                    DataGridViewButtonCell btnCell = (DataGridViewButtonCell)row.Cells["Delivered"];
                    btnCell.UseColumnTextForButtonValue = false;
                    btnCell.FlatStyle = FlatStyle.Flat;
                    btnCell.Value = "";
                }
            }
        }
    }
}
