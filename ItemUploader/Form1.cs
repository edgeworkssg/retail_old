using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlServerCe;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace ItemUploader
{
    public partial class frmItemLoader : Form
    {
        public frmItemLoader()
        {
            InitializeComponent();
        }

        SqlCeConnection conn;
        bool Connected;
        DataTable dt;
        int totalError;
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            //openFileDialog1.Filter = "Print Template|*.rpt";
            //openFileDialog1.Tag = Print_ReceiptFileLocation;
            fileDlg.ShowDialog();
        }

        private void fileDlg_FileOk(object sender, CancelEventArgs e)
        {
            txtFileName.Text = fileDlg.FileName;
        }

        private void btnValidateConnection_Click(object sender, EventArgs e)
        {
            try
            {
                conn = new SqlCeConnection("Data Source=" + txtFileName.Text + ";" + "SSCE:Database Password='" + txtPassword.Text + "';");
                SqlCeCommand command = new SqlCeCommand("SELECT * FROM Item", conn);
                SqlCeDataAdapter dataAdapter = new SqlCeDataAdapter(command);
                DataSet ds = new DataSet();
                dataAdapter.Fill(ds);
                Connected = true;
                MessageBox.Show("Test Connection Successful");
            }
            catch (Exception ex) { MessageBox.Show("Connection Error. " + ex.Message); }
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void btnLoadItems_Click(object sender, EventArgs e)
        {
            if (!Connected)
            {
                MessageBox.Show("Please check your connection.");
                tabControl1.SelectedIndex = 2;
            }
            if (txtServer.Text == "")
            {
                MessageBox.Show("Please enter Server Name!");
                return;
            }
            if (txtDatabase.Text == "")
            {
                MessageBox.Show("Please enter Database Name!");
                return;
            }
            if (txtTableName.Text == "")
            {
                MessageBox.Show("Please enter Table Name!");
                return;
            }
            try
            {
                //Load Source into DataTable
                if (chkClearItem.Checked)
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();
                    SqlCeCommand command = new SqlCeCommand("Delete from Item", conn);
                    command.ExecuteNonQuery();
                }
                panel1.Visible = true;

                string connStr = "Data Source=" + txtServer.Text + ";Initial Catalog=" + txtDatabase.Text + ";uid=" + txtUserName.Text + ";pwd=" + txtSqlPassword.Text;
                SqlConnection conn1 = new SqlConnection(connStr);
                conn1.Open();

                SqlCommand cmd = new SqlCommand("Select * from " + txtTableName.Text + " Where IsIninventory= 1 ", conn1);

                dt = new DataTable();
                dt.Load(cmd.ExecuteReader());

                progressBar1.Minimum = 1;
                progressBar1.Maximum = dt.Rows.Count;

                totalError = 0;
                backgroundWorker1.RunWorkerAsync();

            }
            catch (Exception ex) { MessageBox.Show("Error."+ex.Message); }
        }

        private void frmItemLoader_Load(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 2;
            backgroundWorker1.WorkerReportsProgress = true;
            Connected = false;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            
            
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                // Report progress to 'UI' thread
                backgroundWorker1.ReportProgress(i+1);
                // Simulate long task
                
                try
                {
                    string barcode  = dt.Rows[i]["Barcode"] == null ? "" : dt.Rows[i]["Barcode"].ToString();
                    string ItemNo  = dt.Rows[i]["ItemNo"] == null ? "" : dt.Rows[i]["ItemNo"].ToString();
                    string ItemName  = dt.Rows[i]["ItemName"] == null ? "" : RemoveSpecialCharacters(dt.Rows[i]["ItemName"].ToString());
                    string CategoryName = dt.Rows[i]["CategoryName"] == null ? "" : RemoveSpecialCharacters(dt.Rows[i]["CategoryName"].ToString());
                    string Attributes1 = dt.Rows[i]["Attributes1"] == null ? "" : RemoveSpecialCharacters(dt.Rows[i]["Attributes1"].ToString());
                    string Attributes2 = dt.Rows[i]["Attributes2"] == null ? "" : RemoveSpecialCharacters(dt.Rows[i]["Attributes2"].ToString());
                    string Attributes3 = dt.Rows[i]["Attributes3"] == null ? "" : RemoveSpecialCharacters(dt.Rows[i]["Attributes3"].ToString());
                    string Attributes4 = dt.Rows[i]["Attributes4"] == null ? "" : RemoveSpecialCharacters(dt.Rows[i]["Attributes4"].ToString());
                    string Attributes5 = dt.Rows[i]["Attributes5"] == null ? "" : RemoveSpecialCharacters(dt.Rows[i]["Attributes5"].ToString());
                    string Attributes6 = dt.Rows[i]["Attributes6"] == null ? "" : RemoveSpecialCharacters(dt.Rows[i]["Attributes6"].ToString());
                    string Attributes7 = dt.Rows[i]["Attributes7"] == null ? "" : RemoveSpecialCharacters(dt.Rows[i]["Attributes7"].ToString());
                    string Attributes8 = dt.Rows[i]["Attributes8"] == null ? "" : RemoveSpecialCharacters(dt.Rows[i]["Attributes8"].ToString());

                    string sqlString = "Insert into item (barcode, ItemNo, ItemName, RetailPrice, CategoryName, Attributes1, Attributes2, Attributes3, Attributes4, Attributes5, Attributes6, Attributes7, Attributes8) values " +
                        "('" + barcode + "', '" + ItemNo + "','" + ItemName + "'," + dt.Rows[i]["RetailPrice"].ToString() + ",'" + CategoryName + "','" + Attributes1 + "','" +
                        Attributes2 + "','" + Attributes3 + "','" + Attributes4 + "','" + Attributes5 + "','" + Attributes6 + "','" + Attributes7 + "','" + Attributes8 + "')";
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();
                    SqlCeCommand command = new SqlCeCommand(sqlString, conn);
                    command.ExecuteNonQuery();
                }
                catch (Exception ex) { MessageBox.Show(ex.Message);  totalError++; }
            }
        }

        
        public string RemoveSpecialCharacters(string str)
        {
            return Regex.Replace(str, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            lblProgress.Text = "Executing " + e.ProgressPercentage.ToString() + " of " + dt.Rows.Count.ToString() + "\n" + " Total Error : " + totalError.ToString();
            
        }
    }
}
