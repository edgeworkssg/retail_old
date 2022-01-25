using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using POSDevices;

namespace WinUtility
{
    public partial class Form3 : Form
    {

        public Form3()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FlyTechCashDrawer c = new FlyTechCashDrawer();
            char tmp = c.checkStatus();
            MessageBox.Show(tmp.ToString());

            bool res = c.isCashDrawerOpen();
            if (res)
                MessageBox.Show("Is Open");
            else
                MessageBox.Show("Is Closed");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FlyTechCashDrawer c = new FlyTechCashDrawer();
            c.OpenDrawer("1");
            //MessageBox.Show(tmp.ToString());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FlyTechCashDrawer c = new FlyTechCashDrawer();
            c.CloseDrawer("1");
        }
    }
}
