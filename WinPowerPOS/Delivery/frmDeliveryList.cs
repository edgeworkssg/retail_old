using PowerPOS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinPowerPOS.Delivery
{
    public partial class frmDeliveryList : Form
    {
        public BackgroundWorker SyncSendDeliveryOrderThread;

        public frmDeliveryList()
        {
            InitializeComponent();
        }

        private void frmDeliveryList_Load(object sender, EventArgs e)
        {
            Tabel.AutoGenerateColumns = false;

            
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Tabel.DataSource = DeliveryController.FetchDeliveryReport(tRefNo.Text, 0, dtpStartDate.Value, dtpEndDate.Value, tSearch.Text);
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (Tabel.SelectedCells.Count > 0 )
            {
                int rowindex= Tabel.SelectedCells[0].RowIndex;
                if (Tabel.Rows[rowindex].Cells["dgvcRefNo"].Value != null && Tabel.Rows[rowindex].Cells["dgvcRefNo"].ToString() != "")
                {
                    DeliveryOrder hdr = new DeliveryOrder(Tabel.Rows[rowindex].Cells["dgvcRefNo"].Value.ToString());
                    POSController pos = new POSController();
                    if (hdr != null && hdr.OrderNumber != "")
                    {
                        DeliveryOrderDetailCollection finalDet = new DeliveryOrderDetailCollection();
                        finalDet.Where(DeliveryOrderDetail.Columns.Dohdrid, hdr.OrderNumber);
                        finalDet.Load();
                        POSDevices.Receipt rcpt = new POSDevices.Receipt();
                        rcpt.PrintDeliveryOrder(hdr, finalDet, pos);
                    }
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (Tabel.SelectedCells.Count > 0)
            {
                int rowindex = Tabel.SelectedCells[0].RowIndex;
                if (Tabel.Rows[rowindex].Cells["dgvcRefNo"].Value != null && Tabel.Rows[rowindex].Cells["dgvcRefNo"].ToString() != "")
                {
                    DeliveryOrder hdr = new DeliveryOrder(Tabel.Rows[rowindex].Cells["dgvcRefNo"].Value.ToString());
                    POSController pos = new POSController();
                    if (hdr != null && hdr.OrderNumber != "")
                    {
                        frmDeliverySetup fDelivery = new frmDeliverySetup();
                        fDelivery.SyncSendDeliveryOrderThread = SyncSendDeliveryOrderThread;
                        fDelivery.delOrderHdr = hdr;
                        fDelivery.ShowDialog();
                        fDelivery.Dispose();
                        Tabel.DataSource = DeliveryController.FetchDeliveryReport(tRefNo.Text, 0, dtpStartDate.Value, dtpEndDate.Value, tSearch.Text);
                    }
                }
            }
        }
    }
}
