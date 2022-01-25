using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;
using PowerPOS;
using System.Data.Sql;

namespace WinUtility
{
    public partial class frmCostPriceUpdater : Form
    {
        List<String> inventoryDetIdCollection = new List<String>();
        public frmCostPriceUpdater()
        {
            InitializeComponent();
        }

        private void frmCostPriceUpdater_Load(object sender, EventArgs e)
        {
            this.cboDatabaseNames.Items.Add("---SELECT DATABASE---");
            this.LoadDatabaseList(ConfigurationManager.ConnectionStrings["PowerPOS"].ConnectionString);
            this.ClearValues();
        }


        private void LoadDatabaseList(String ConnStr)
        {
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                conn.Open();
                DataTable databases = conn.GetSchema("Databases");
                conn.Close();

                List<String> db = new List<String>();

                foreach (DataRow database in databases.Rows)
                {
                    if (database.Field<short>("dbid") > 4)
                    {
                        String databaseName = database.Field<String>("database_name");
                        db.Add(databaseName);
                    }
                }
                db.Sort();
                foreach (String str in db)
                {
                    this.cboDatabaseNames.Items.Add(str);
                }
            }
        }

        private String LoadCostPrice(String Database, String ItemNo)
        {
            Decimal CostOfGoods = 0;
            try
            {
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["PowerPOS"].ConnectionString);
                conn.Open();
                conn.ChangeDatabase(Database);
                String SQL = "SELECT CostOfGoods, InventoryDetRefNo FROM InventoryDet WHERE ItemNo = '" + ItemNo + "'";
                using (SqlDataAdapter da = new SqlDataAdapter(SQL, conn))
                {
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    if (Decimal.TryParse(ds.Tables[0].Rows[0]["CostOfGoods"].ToString(), out CostOfGoods))
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            this.inventoryDetIdCollection.Add(row["InventoryDetRefNo"].ToString());
                        }
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return String.Format("{0:C}", CostOfGoods);
        }

        private Boolean UpdateCostPrice(String Database, String ItemNo, String OldValue, String NewValue)
        {
            try
            {
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["PowerPOS"].ConnectionString);
                conn.Open();
                conn.ChangeDatabase(Database);
                String SQL = "Update InventoryDet SET CostOfGoods = '" + NewValue + "', FactoryPrice = '" + NewValue + "'  WHERE ItemNo = '" + ItemNo + "'";
                using (SqlCommand cmd = new SqlCommand(SQL, conn))
                {
                    cmd.ExecuteNonQuery();
                    foreach (String invDetID in this.inventoryDetIdCollection)
                    {
                        String SQLLog = "INSERT INTO PowerLog (LogDate, LogMsg) VALUES ('" + DateTime.Now.ToString() + "', 'Updating Cost Price for InventoryDetRefNo " + invDetID + " with ItemNo " + ItemNo + " from " + OldValue + " to " + NewValue + "')";
                        cmd.CommandText = SQLLog;
                        cmd.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        private void txtItemNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!String.IsNullOrEmpty(this.txtItemNo.Text) && this.cboDatabaseNames.SelectedIndex > 0)
                {
                    this.txtCostPrice.Text = this.LoadCostPrice(this.cboDatabaseNames.Text.Trim(), this.txtItemNo.Text.Trim());
                }
            }
        }

        private void txtItemNo_Leave(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(this.txtItemNo.Text) && this.cboDatabaseNames.SelectedIndex > 0)
            {
                this.txtCostPrice.Text = this.LoadCostPrice(this.cboDatabaseNames.Text.Trim(), this.txtItemNo.Text.Trim());
            }
        }

        private void btnCommit_Click(object sender, EventArgs e)
        {
            Decimal newValue =0;
            if (decimal.TryParse(this.txtChangeTo.Text, out newValue))
            {
                if (this.UpdateCostPrice(this.cboDatabaseNames.Text.Trim(), this.txtItemNo.Text.Trim(), this.txtCostPrice.Text, newValue.ToString()))
                {
                    MessageBox.Show("Cost Price updated successfully.");
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.ClearValues();
            
        }

        private void ClearValues()
        {
            this.cboDatabaseNames.SelectedIndex = 0;
            this.txtItemNo.Text = String.Empty;
            this.txtCostPrice.Text = String.Empty;
            this.txtChangeTo.Text = String.Empty;
            this.inventoryDetIdCollection.Clear();
        }
    }
}
