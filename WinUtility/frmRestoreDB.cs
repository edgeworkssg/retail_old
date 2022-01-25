using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System.Data.SqlClient;

namespace WinUtility
{
    public partial class frmRestoreDB : Form
    {
        public frmRestoreDB()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            tURL.Text = openFileDialog1.FileName;
        }

        private void frmRestoreDB_Load(object sender, EventArgs e)
        {
            SqlConnectionStringBuilder Conn = new SqlConnectionStringBuilder(System.Configuration.ConfigurationManager.ConnectionStrings["PowerPOS"].ConnectionString);
            
            if (string.IsNullOrEmpty(GlobalVar.TableName))
                tTableName.Text = Conn.InitialCatalog;
            else
                tTableName.Text = GlobalVar.TableName;

            Conn.InitialCatalog = "master";

            tConnStr.Tag = Conn;
            tConnStr.Text = Conn.ConnectionString;
        }

        private void btnAction_Click(object sender, EventArgs e)
        {
            SqlConnectionStringBuilder Conn = (SqlConnectionStringBuilder)tConnStr.Tag;
            string DBName = tTableName.Text;
            Conn.InitialCatalog = DBName;

            if (!SqlCustomTools.IsDatabaseExist(Conn.DataSource, DBName))
            {
                SqlCustomTools.Create_NewDatabase(Conn.ConnectionString);
            }

            SqlCustomTools.Restore_FromBakFile(Conn.ConnectionString, tURL.Text);
        }
    }
}
