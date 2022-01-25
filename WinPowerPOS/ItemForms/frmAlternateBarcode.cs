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

namespace WinPowerPOS.ItemForms
{
    public partial class frmAlternateBarcode : Form
    {
        public string itemNo;
        public frmAlternateBarcode()
        {
            InitializeComponent();
            dgvItems.AutoGenerateColumns = false;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //
            AlternateBarcode abr = new AlternateBarcode();
            abr.ItemNo = itemNo;
            abr.Barcode = txtAltBarcode.Text;
            abr.IsNew = true;
            abr.Deleted = false;
            abr.Save(UserInfo.username);

            MessageBox.Show("Alternate Barcode Saved");
            BindGrid();
        }
        private void BindGrid()
        {
            AlternateBarcodeCollection abr = new AlternateBarcodeCollection();
            abr.Where(AlternateBarcode.Columns.ItemNo, itemNo);
            abr.Where(AlternateBarcode.Columns.Deleted, false);
            abr.OrderByDesc(AlternateBarcode.Columns.CreatedOn);
            abr.Load();

            dgvItems.DataSource = abr.ToDataTable();
            dgvItems.Refresh();
        }

        private void dgvItems_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == 0)
            {
                AlternateBarcode.Delete(int.Parse(dgvItems.Rows[e.RowIndex].Cells["BarcodeIdCol"].Value.ToString()));
                BindGrid();
            }
        }

        private void frmAlternateBarcode_Load(object sender, EventArgs e)
        {
            Item it = new Item(itemNo);
            lblItemNoName.Text = it.ItemNo + " " + it.ItemName;
            BindGrid();
        }

        private void txtAltBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAdd_Click(this, new EventArgs());
            }
        }
    }
}
