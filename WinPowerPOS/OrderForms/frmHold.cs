using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PowerPOS;

namespace WinPowerPOS.OrderForms
{
    public partial class frmHold : Form
    {
        private HoldController MasterControl;
        private int cnt = 0;

        public POSController SelectedPOS = null;

        public frmHold()
        {
            InitializeComponent();
        }

        private void frmHold_Load(object sender, EventArgs e)
        {
            cnt = 0;

            MasterControl = new HoldController();
            Tabel.AutoGenerateColumns = false;
            Tabel.DataSource = MasterControl.ToDataTable();
            
            Tabel.Sort(Tabel.Columns[dgvcAppTime.Name], ListSortDirection.Ascending);

            if (Tabel.Rows.Count > 0)
                Tabel.ClearSelection();
        }

        private void Tabel_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            Tabel.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Silver;
        }

        private void Tabel_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            Tabel.Rows[e.RowIndex].DefaultCellStyle.BackColor = Tabel.DefaultCellStyle.BackColor;
        }

        private void Tabel_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= Tabel.Rows.Count) return;

            if (cnt > 0) return;
            cnt++;

            string SelectedIndex = Tabel[dgvcHoldNo.Name, e.RowIndex].Value.ToString();

            SelectedPOS = MasterControl.LoadHold(SelectedIndex);

            #region *) Update the POS object to match current structure
            OrderHdr newOrderHdr = new OrderHdr();
            OrderHdrCollection tmpOrderHdr = new OrderHdrCollection();
            tmpOrderHdr.Add(SelectedPOS.myOrderHdr);
            newOrderHdr.Load(tmpOrderHdr.ToDataTable());
            SelectedPOS.myOrderHdr = newOrderHdr;

            OrderDetCollection newOrderDet = new OrderDetCollection();
            newOrderDet.Load(SelectedPOS.myOrderDet.ToDataTable());
            SelectedPOS.myOrderDet = newOrderDet;

            ReceiptHdr newReceiptHdr = new ReceiptHdr();
            ReceiptHdrCollection tmpReceiptHdr = new ReceiptHdrCollection();
            tmpReceiptHdr.Add(SelectedPOS.recHdr);
            newReceiptHdr.Load(tmpReceiptHdr.ToDataTable());
            SelectedPOS.recHdr = newReceiptHdr;

            ReceiptDetCollection newReceiptDet = new ReceiptDetCollection();
            newReceiptDet.Load(SelectedPOS.recDet.ToDataTable());
            SelectedPOS.recDet = newReceiptDet;
            #endregion

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
