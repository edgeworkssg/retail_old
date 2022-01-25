using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PowerPOS;

namespace PowerInventory
{
    public partial class frmSelectPackingSize : Form
    {
        public bool IsSuccess = false;
        public string SelectedPackingSizeName = "";
        public decimal SelectedPackingSizeUOM = 0;
        public decimal SelectedPackingSizeCost = 0;

        public frmSelectPackingSize(string itemNo, int supplierID, string packingSize, List<PackingSize> data)
        {
            InitializeComponent();
            try
            {
                dgvPacking.AutoGenerateColumns = false;
                dgvPacking.DataSource = data;
                dgvPacking.Refresh();
                dgvPacking.ClearSelection();
                for (int i = 0; i < dgvPacking.Rows.Count; i++)
                {
                    if ((dgvPacking.Rows[i].Cells[PackingSizeName.Name].Value + "") == packingSize)
                    {
                        dgvPacking.CurrentCell = dgvPacking.Rows[i].Cells[4];
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            IsSuccess = false;
            this.Close();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (dgvPacking.SelectedRows.Count > 0)
            {
                SelectedPackingSizeName = dgvPacking.SelectedRows[0].Cells[PackingSizeName.Name].Value + "";
                SelectedPackingSizeUOM = (dgvPacking.SelectedRows[0].Cells[PackingSizeUOM.Name].Value + "").GetDecimalValue();
                SelectedPackingSizeCost = (dgvPacking.SelectedRows[0].Cells[PackingSizeCostPrice.Name].Value + "").GetDecimalValue();
                IsSuccess = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Please select packing size");
            }
        }
    }
}
