using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinPowerPOS.OrderForms
{
    public partial class frmChangeGST : Form
    {

        public int selectedGST;
        public bool isSuccesful;

        public frmChangeGST()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            isSuccesful = false;
            this.Close();
        }

        private void frmChangeGST_Load(object sender, EventArgs e)
        {
        }

        private void btnExcGST_Click(object sender, EventArgs e)
        {
            selectedGST = 1;
            this.Close();
        }

        private void btnIncGST_Click(object sender, EventArgs e)
        {
            selectedGST = 2;
            this.Close();
        }

        private void btnNoGST_Click(object sender, EventArgs e)
        {
            selectedGST = 3;
            this.Close();
        }

        private void btnAbsorbGST_Click(object sender, EventArgs e)
        {
            selectedGST = 4;
            this.Close();
        }
    }
}
