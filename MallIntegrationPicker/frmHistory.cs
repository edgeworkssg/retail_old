using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MallIntegrationPicker
{
    public partial class frmHistory : Form
    {
        delegate bool writeHistoryCallback(string msg);

        public frmHistory(string msg)
        {
            InitializeComponent();
            txtLog.Text = msg;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        public bool writeHistory(string msg)
        {
            if (!chkPause.Checked)
            {
                if (txtLog.InvokeRequired)
                {
                    writeHistoryCallback d = new writeHistoryCallback(writeHistory);
                    Invoke(d, msg);
                }
                else
                {
                    txtLog.Text = msg;
                }
            }
            return true;
        }
    }
}
