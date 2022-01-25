using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using System.Collections;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Web.Script.Services;
using System.Web.Script.Serialization;

namespace MallIntegrationPicker
{
    public partial class FrmPickerMain : Form
    {
        public FrmPickerMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog od = new OpenFileDialog();
            var data = new ArrayList();
            if (od.ShowDialog() == DialogResult.OK)
            {
                DataTable message = null;
                DataTable ErrorDb;

                if (!ExcelController.ImportExcelCSVWithDelimiter(';',od.FileName, out message, false))
                    throw new Exception("");

                if (message.Columns.Count != 8)
                    MessageBox.Show("The format is wrong. number of columns is not match with the format specification.");
                dgvDAta.DataSource = message;
                //POSController pos = new POSController();
                foreach (DataRow dr in message.Rows)
                {
                    var tmpRow = new
                    {
                        
                        MallCode = dr[0].ToString(),
                        TenantCode = dr[1].ToString(),
                        Date = dr[2].ToString(),
                        Hour = dr[3].ToString(),
                        TransactionCount = dr[4].ToString(),
                        TotalSalesAfterTax = dr[5].ToString(),
                        TotalSalesBeforeTax = dr[6].ToString(),
                        TotalTax = dr[7].ToString()

                    };
                    data.Add(tmpRow);
                }

                MallIntegrationService.MallIntegrationService msService = new MallIntegrationPicker.MallIntegrationService.MallIntegrationService();
                MallIntegrationService.UserCredentials user = new MallIntegrationService.UserCredentials();

                user.userName = Properties.Settings.Default.UserName;
                user.password = Properties.Settings.Default.Password;
                msService.UserCredentialsValue = user;
                string temp = msService.SendData(new JavaScriptSerializer().Serialize(data));
                MessageBox.Show(temp);
            }

        }

        frmSetting fSetting;
        private void settingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fSetting == null || fSetting.IsDisposed)
            {
                fSetting = new frmSetting();
                fSetting.ShowDialog();
            }
            else
            {
                fSetting.WindowState = FormWindowState.Normal;
                fSetting.Activate();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
