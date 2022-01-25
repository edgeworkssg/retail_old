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
using SubSonic;

namespace WinVoucherBatchGenerator
{
    public partial class frmVoucherList : Form
    {
        public frmVoucherList()
        {
            InitializeComponent();
        }

        private void frmVoucherList_Load(object sender, EventArgs e)
        {
            dgvVoucherList.AutoGenerateColumns = false;                
            //CashierLogin cr = new CashierLogin();
            //cr.ShowDialog();                    
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {                        
            VoucherCollection v = new VoucherCollection();
            if (txtVoucherNoFrom.Text != "")
            {
                v.Where(Voucher.Columns.VoucherNo, Comparison.GreaterOrEquals, txtVoucherNoFrom.Text);
            }
            if (txtVoucherNoTo.Text != "")
            {    
                v.Where(Voucher.Columns.VoucherNo, Comparison.LessOrEquals, txtVoucherNoTo.Text);
            }
            v.Where(Voucher.Columns.Deleted, false);
            v.Load();

            dgvVoucherList.DataSource = v;
            dgvVoucherList.Refresh();            
        }

        private void btnIssue_Click(object sender, EventArgs e)
        {
            VoucherCollection v = new VoucherCollection();            
            v.Where(Voucher.Columns.DateIssued, Comparison.GreaterOrEquals, dtpFrom.Value);
            v.Where(Voucher.Columns.DateIssued, Comparison.LessOrEquals, dtpTo.Value);            
            v.Where(Voucher.Columns.Deleted, false);
            v.Load();

            dgvVoucherList.DataSource = v;
            dgvVoucherList.Refresh();            
        }

        private void btnSold_Click(object sender, EventArgs e)
        {
            VoucherCollection v = new VoucherCollection();
            if (txtVoucherNoFrom.Text != "")
            {
                v.Where(Voucher.Columns.VoucherNo, Comparison.GreaterOrEquals, txtVoucherNoFrom.Text);
            }
            if (txtVoucherNoTo.Text != "")
            {
                v.Where(Voucher.Columns.VoucherNo, Comparison.LessOrEquals, txtVoucherNoTo.Text);
            }
            v.Where(Voucher.Columns.Deleted, false);
            v.Load();

            dgvVoucherList.DataSource = v;
            dgvVoucherList.Refresh();            
        }

        private void btnRedeemed_Click(object sender, EventArgs e)
        {
            VoucherCollection v = new VoucherCollection();
            v.Where(Voucher.Columns.DateRedeemed, Comparison.GreaterOrEquals, dtpFrom.Value);
            v.Where(Voucher.Columns.DateRedeemed, Comparison.LessOrEquals, dtpTo.Value);
            v.Where(Voucher.Columns.Deleted, false);
            v.Load();

            dgvVoucherList.DataSource = v;
            dgvVoucherList.Refresh();            
        }

        private void btnExpiry_Click(object sender, EventArgs e)
        {
            VoucherCollection v = new VoucherCollection();
            v.Where(Voucher.Columns.ExpiryDate, Comparison.GreaterOrEquals, dtpFrom.Value);
            v.Where(Voucher.Columns.ExpiryDate, Comparison.LessOrEquals, dtpTo.Value);
            v.Where(Voucher.Columns.Deleted, false);
            v.Load();

            dgvVoucherList.DataSource = v;
            dgvVoucherList.Refresh();            
        }

        private void btnAmount_Click(object sender, EventArgs e)
        {
            decimal Amount;

            if (decimal.TryParse(txtMisc.Text, out Amount))
            {
                VoucherCollection v = new VoucherCollection();
                v.Where(Voucher.Columns.Amount, Comparison.GreaterOrEquals, Amount);
                v.Where(Voucher.Columns.Deleted, false);
                v.Load();

                dgvVoucherList.DataSource = v;
                dgvVoucherList.Refresh();
            }
            else
            {
                MessageBox.Show("Please specify a number on the search box");
                txtMisc.Select();
            }
        }

        private void btnRemark_Click(object sender, EventArgs e)
        {
            VoucherCollection v = new VoucherCollection();
            v.Where(Voucher.Columns.Remark, Comparison.Like, "%" + Remark + "%");
            v.Where(Voucher.Columns.Deleted, false);
            v.Load();

            dgvVoucherList.DataSource = v;
            dgvVoucherList.Refresh();
        }

        private void btnGenerateNew_Click(object sender, EventArgs e)
        {
            frmGenerateVoucher f = new frmGenerateVoucher();
            f.ShowDialog();
            f.Dispose();
        }
    }
}
