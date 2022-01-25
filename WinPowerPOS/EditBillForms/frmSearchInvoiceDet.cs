using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SubSonic;
using PowerPOS;
using PowerPOS.Container;


namespace WinPowerPOS.EditBillForms
{
    public partial class frmSearchInvoiceDet : Form
    {
        public frmSearchInvoiceDet()
        {
            InitializeComponent();
        }

        private void frmSearchInvoiceDet_Load(object sender, EventArgs e)
        {
            CommonUILib.FormatDateFilter(ref dtpStartDate, ref dtpEndDate);
            dgvRcpt.AutoGenerateColumns = false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        private void BindGrid()
        {
            string Search = "";

            Search = txtSearch.Text;


            DataTable dt = ReportController.TransactionDetailReportWithRemark(dtpStartDate.Value,
                dtpEndDate.Value, 0, "", "", "", "", Search);

            dt = CommonUILib.DataTableConvertBoolColumnToYesOrNo(dt);
            dgvRcpt.DataSource = dt;
            dgvRcpt.Refresh();
        }

        private void dgvRcpt_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (dgvRcpt.Rows[e.RowIndex].Cells["Amount"].Value.ToString() == "")
                dgvRcpt.Rows[e.RowIndex].Cells["Amount"].Value = decimal.Parse(dgvRcpt.Rows[e.RowIndex].Cells["Amount"].Value.ToString()).ToString("N2");

            if (dgvRcpt.Rows[e.RowIndex].Cells["UnitPrice"].Value.ToString() == "")
                dgvRcpt.Rows[e.RowIndex].Cells["UnitPrice"].Value = decimal.Parse(dgvRcpt.Rows[e.RowIndex].Cells["UnitPrice"].Value.ToString()).ToString("N2");
            
            if (dgvRcpt.Rows[e.RowIndex].Cells["Disc"].Value.ToString() == "")
                dgvRcpt.Rows[e.RowIndex].Cells["Disc"].Value = decimal.Parse(dgvRcpt.Rows[e.RowIndex].Cells["Disc"].Value.ToString()).ToString("N2");

            if (dgvRcpt.Rows[e.RowIndex].Cells["IsVoided"].Value.ToString().ToUpper() == "YES")
                //|| bool.Parse(dgvRcpt.Rows[e.RowIndex].Cells["IsLineVoided"].Value.ToString()) == true)
            {
                for (int j = 0; j < dgvRcpt.ColumnCount; ++j)
                {
                    if (dgvRcpt.Rows[e.RowIndex].Cells[j].Visible)
                    {
                        dgvRcpt.Rows[e.RowIndex].Cells[j].Style.ForeColor = System.Drawing.Color.White;
                        dgvRcpt.Rows[e.RowIndex].Cells[j].Style.BackColor = System.Drawing.Color.DarkRed;

                    }
                }
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)dgvRcpt.DataSource;
            if (dt != null && dt.Rows.Count > 0)
            {
                fsdExportToExcel.ShowDialog();                
            }
            else
            {
                MessageBox.Show("There is no data to export");
            }            
        }

        private void fsdExportToExcel_FileOk(object sender, CancelEventArgs e)
        {
            ExportController.ExportToExcel(dgvRcpt, fsdExportToExcel.FileName);
            MessageBox.Show("File saved");
        }
    }
}