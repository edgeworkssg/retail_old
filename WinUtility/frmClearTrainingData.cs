using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace WinUtility
{
    public partial class frmClearTrainingData : Form
    {
        public frmClearTrainingData()
        {
            InitializeComponent();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                tTableName.ReadOnly = false;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                tTableName.ReadOnly = true;

                SqlConnectionStringBuilder Conn = (SqlConnectionStringBuilder)tConnStr.Tag;

                if (string.IsNullOrEmpty(GlobalVar.TableName))
                    tTableName.Text = Conn.InitialCatalog;
                else
                    tTableName.Text = GlobalVar.TableName;
            }
        }

        private void btnAction_Click(object sender, EventArgs e)
        {
        }

        private void frmClearTrainingData_Load(object sender, EventArgs e)
        {
            SqlConnectionStringBuilder Conn = new SqlConnectionStringBuilder(System.Configuration.ConfigurationManager.ConnectionStrings["PowerPOS"].ConnectionString);

            if (string.IsNullOrEmpty(GlobalVar.TableName))
                tTableName.Text = Conn.InitialCatalog;
            else
                tTableName.Text = GlobalVar.TableName;

            tConnStr.Tag = Conn;
            tConnStr.Text = Conn.ConnectionString;
        }
    }
}
