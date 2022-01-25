using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PowerInventory.Support
{
    public partial class frmStockReport : Form
    {
        public frmStockReport()
        {
            InitializeComponent();
        }

        frmStockIn Inst = new frmStockIn();
        private void frmStockReport_Load(object sender, EventArgs e)
        {
            Inst = new frmStockIn();
            Inst.FormBorderStyle = FormBorderStyle.None;
            tabPage1.Controls.Add(Inst);
        }
    }
}
