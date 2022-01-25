using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PowerPOS;

namespace PowerInventory
{
    public partial class frmChoosePackageSize : Form
    {
        public string itemNo;
        public string supplierName;
        public string packageSize;
        public decimal costPrice;
        public decimal packingSizeUOm;

        public frmChoosePackageSize()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void frmChoosePackageSize_Load(object sender, EventArgs e)
        {
            try
            {
                Supplier s = new Supplier(Supplier.Columns.SupplierName, supplierName);
                if (s == null || s.SupplierID == 0)
                {
                    this.DialogResult = DialogResult.Cancel;
                }
                ItemSupplierMapCollection ism = new ItemSupplierMapCollection();
                ism.Where(ItemSupplierMap.Columns.ItemNo, itemNo);
                ism.Where(ItemSupplierMap.Columns.SupplierID, s.SupplierID);
                ism.Load();

                if (ism.Count < 1)
                {
                    MessageBox.Show("There are no Item Supplier Map Definition for this item");
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                    return;
                }

                DataTable dt = new DataTable();
                dt.Columns.Add("PackingSize");
                dt.Columns.Add("Qty");
                dt.Columns.Add("UOM");
                dt.Columns.Add("Cost", typeof(decimal));
                IDataReader reader = null;
                reader = Item.FetchByParameter("ItemNo", itemNo);
                string UOM = "";
                if (reader.Read())
                {
                    UOM = reader["userfld1"].ToString();
                }

                if (ism[0].PackingSize1 != null && ism[0].PackingSize1 != "")
                {
                    DataRow dr = dt.NewRow();
                    dr["PackingSize"] = ism[0].PackingSize1;
                    dr["UOM"] = ism[0].PackingSizeUOM1;
                    dr["Qty"] = UOM;
                    dr["Cost"] = ism[0].CostPrice1;
                    dt.Rows.Add(dr);
                }
                if (ism[0].PackingSize2 != null && ism[0].PackingSize2 != "")
                {
                    DataRow dr = dt.NewRow();
                    dr["PackingSize"] = ism[0].PackingSize2;
                    dr["UOM"] = ism[0].PackingSizeUOM2;
                    dr["Qty"] = UOM;
                    dr["Cost"] = ism[0].CostPrice2;
                    dt.Rows.Add(dr);
                }
                if (ism[0].PackingSize3 != null && ism[0].PackingSize3 != "")
                {
                    DataRow dr = dt.NewRow();
                    dr["PackingSize"] = ism[0].PackingSize3;
                    dr["UOM"] = ism[0].PackingSizeUOM3;
                    dr["Cost"] = ism[0].CostPrice3;
                    dr["Qty"] = UOM;
                    dt.Rows.Add(dr);
                }
                if (ism[0].PackingSize4 != null && ism[0].PackingSize4 != "")
                {
                    DataRow dr = dt.NewRow();
                    dr["PackingSize"] = ism[0].PackingSize4;
                    dr["UOM"] = ism[0].PackingSizeUOM4;
                    dr["Cost"] = ism[0].CostPrice4;
                    dr["Qty"] = UOM;
                    dt.Rows.Add(dr);
                }
                if (ism[0].PackingSize5 != null && ism[0].PackingSize5 != "")
                {
                    DataRow dr = dt.NewRow();
                    dr["PackingSize"] = ism[0].PackingSize5;
                    dr["UOM"] = ism[0].PackingSizeUOM5;
                    dr["Cost"] = ism[0].CostPrice5;
                    dr["Qty"] = UOM;
                    dt.Rows.Add(dr);
                }
                if (ism[0].PackingSize6 != null && ism[0].PackingSize6 != "")
                {
                    DataRow dr = dt.NewRow();
                    dr["PackingSize"] = ism[0].PackingSize6;
                    dr["UOM"] = ism[0].PackingSizeUOM6;
                    dr["Cost"] = ism[0].CostPrice6;
                    dr["Qty"] = UOM;
                    dt.Rows.Add(dr);
                }
                if (ism[0].PackingSize7 != null && ism[0].PackingSize7 != "")
                {
                    DataRow dr = dt.NewRow();
                    dr["PackingSize"] = ism[0].PackingSize7;
                    dr["UOM"] = ism[0].PackingSizeUOM7;
                    dr["Cost"] = ism[0].CostPrice7; dr["Qty"] = UOM;
                    dt.Rows.Add(dr);
                }
                if (ism[0].PackingSize8 != null && ism[0].PackingSize8 != "")
                {
                    DataRow dr = dt.NewRow();
                    dr["PackingSize"] = ism[0].PackingSize8;
                    dr["UOM"] = ism[0].PackingSizeUOM8;
                    dr["Cost"] = ism[0].CostPrice8;
                    dr["Qty"] = UOM;
                    dt.Rows.Add(dr);
                }
                if (ism[0].PackingSize9 != null && ism[0].PackingSize9 != "")
                {
                    DataRow dr = dt.NewRow();
                    dr["PackingSize"] = ism[0].PackingSize9;
                    dr["UOM"] = ism[0].PackingSizeUOM9;
                    dr["Cost"] = ism[0].CostPrice9;
                    dr["Qty"] = UOM;
                    dt.Rows.Add(dr);
                }
                if (ism[0].PackingSize10 != null && ism[0].PackingSize10 != "")
                {
                    DataRow dr = dt.NewRow();
                    dr["PackingSize"] = ism[0].PackingSize10;
                    dr["UOM"] = ism[0].PackingSizeUOM10;
                    dr["Cost"] = ism[0].CostPrice10;
                    dr["Qty"] = UOM;
                    dt.Rows.Add(dr);
                }

                dataGridView1.AutoGenerateColumns = false;
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Occured : " + ex.Message);
                this.Close();
                Logger.writeLog(ex);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex > -1)
                {
                    packageSize = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                    //costPrice = 0; 
                    if (!decimal.TryParse(dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString(), out packingSizeUOm))
                        packingSizeUOm = 1;
                    if (!decimal.TryParse (dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString(), out costPrice))
                        costPrice =0;
                    this.DialogResult = DialogResult.OK;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Occured : " + ex.Message);
                Logger.writeLog(ex);
            }
        }

        private void btnNoPacking_Click(object sender, EventArgs e)
        {
            packageSize = "No Packing";
            this.DialogResult = DialogResult.OK;
        }
    }
}
