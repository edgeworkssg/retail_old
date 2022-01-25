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
    public partial class frmLoading : Form
    {
        public frmLoading(string instruction)
        {
            InitializeComponent(instruction);
        }

        private void frmLoading_Load(object sender, EventArgs e)
        {
            pnlProgress.Visible = true;
        }
    }
}
