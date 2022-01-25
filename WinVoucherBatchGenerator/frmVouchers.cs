using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PowerPOS;

namespace WinVoucherBatchGenerator
{
    public partial class frmVouchers : Form
    {
        public frmVouchers()
        {
            InitializeComponent();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            int StartNumber, EndNumber, numOfDigit;
            decimal amount;
            string status;

            if (!int.TryParse(txtStartNumber.Text, out StartNumber))
            {
                MessageBox.Show("Please specify correct start number");
                txtStartNumber.Select();
                return;
            }
            if (!int.TryParse(txtEndNumber.Text, out EndNumber))
            {
                MessageBox.Show("Please specify correct end number");
                txtEndNumber.Select();
                return;
            }
            if (!int.TryParse(txtNumberOfDigit.Text, out numOfDigit))
            {
                MessageBox.Show("Please specify correct number of digit.");
                txtNumberOfDigit.Select();
                return;
            }

            if (!decimal.TryParse(txtAmount.Text, out amount))
            {
                MessageBox.Show("Please specify correct amount");
                txtAmount.Select();
                return;
            }

            if (VoucherController.CreateBatchVoucherNo(StartNumber, EndNumber, txtPrefix.Text,
                txtSuffix.Text, amount, numOfDigit, dtpIssueDate.Value, dtpExpiryDate.Value, out status))
            {
                MessageBox.Show("Successful");
            }
            else
            {
                MessageBox.Show("Error: " + status);
            }
        }
    }
}
