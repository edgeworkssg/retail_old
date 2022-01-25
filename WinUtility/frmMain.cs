using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinUtility
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            SqlConnectionStringBuilder Conn = new SqlConnectionStringBuilder(System.Configuration.ConfigurationManager.ConnectionStrings["PowerPOS"].ConnectionString);
            tTableName.Text = Conn.InitialCatalog;
        }

        private void tTableName_Validated(object sender, EventArgs e)
        {
            GlobalVar.TableName = tTableName.Text;
        }

        private void btnRestoreDB_Click(object sender, EventArgs e)
        {
            using (frmRestoreDB instance = new frmRestoreDB())
            {
                instance.ShowDialog();
            }
        }

        private void btnRetailTouchScreen_Click(object sender, EventArgs e)
        {
            using (frmCreateTouchMenuFromItem instance = new frmCreateTouchMenuFromItem())
            {
                instance.ShowDialog();
            }
        }

        private void btnClearData_Click(object sender, EventArgs e)
        {
            using (frmClearTrainingData instance = new frmClearTrainingData())
            {
                instance.ShowDialog();
            }
        }
    }
}
