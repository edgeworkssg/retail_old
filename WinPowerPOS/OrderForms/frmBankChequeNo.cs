using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WinPowerPOS.OrderForms
{
    public partial class frmBankChequeNo : Form
    {
        public string BankName;
        public string ChequeNo;
        public bool IsSuccessful;
        public frmBankChequeNo()
        {
            InitializeComponent();
        }

        private void frmBankChequeNo_Load(object sender, EventArgs e)
        {
            BankName = "";
            ChequeNo = "";
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (txtBankName.Text == "" | txtChequeNo.Text == "")
            {
                MessageBox.Show("Please enter bank name and cheque number.");
                return;
            }
            IsSuccessful = true;
            //set
            BankName = txtBankName.Text;
            ChequeNo = txtChequeNo.Text;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            IsSuccessful = false;
            this.Close();
        }

    }
}