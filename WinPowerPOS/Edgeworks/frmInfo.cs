using SubSonic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinPowerPOS.Edgeworks
{
    public partial class frmInfo : Form
    {
        public frmInfo()
        {
            InitializeComponent();
        }

        private void frmInfo_Load(object sender, EventArgs e)
        {
            DataTable DT = new DataTable();
            DT.Columns.Add("Key");
            DT.Columns.Add("Value");

            DT.Rows.Add("Connection String", DataService.Provider.DefaultConnectionString);
        }
    }
}
