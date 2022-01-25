using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using SubSonic;

namespace WinUtility
{
    public partial class WinCollectionTest : Form
    {
        public WinCollectionTest()
        {
            InitializeComponent();
        }

        private void WinCollectionTest_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            POSController pos = new POSController("07100900100001");
            DataTable dt = pos.FetchCostOfGoods();
            dataGridView1.DataSource = dt;
            dataGridView1.Refresh();
            /*
            CounterCloseLogCollection col = new CounterCloseLogCollection();
            col.Load();
            for (int i=0;i < col.Count;i++)
                POSDevices.POSDeviceController.PrintCloseCounterReport(col[i]);                       */

        }
    }
}