using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinPowerPOS.KioskForms
{
    public partial class frmSimulateScanWeight : Form
    {
        private IScanWeight main;

        public frmSimulateScanWeight(IScanWeight main)
        {
            InitializeComponent();

            this.main = main;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            decimal d = new decimal(0);

            decimal.TryParse(textBox1.Text, out d);

            main.SetWeightResult(true, d);
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            main.CancelScanWeight();
            this.Close();
        }
    }
}
