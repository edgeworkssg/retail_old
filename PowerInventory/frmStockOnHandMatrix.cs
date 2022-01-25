using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using LanguageManager = PowerInventory.Properties.InventoryLanguage;
using SubSonic;

namespace PowerInventory
{
    public partial class frmStockOnHandMatrix : Form
    {
        public string Attributes1;
        public int InventoryLocationID;

        public frmStockOnHandMatrix(string attributes1, int inventoryLocationID)
        {
            this.Attributes1 = attributes1;
            this.InventoryLocationID = inventoryLocationID;
            InitializeComponent();
        }

        private void btnCancelAdd_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmStockOnHandMatrix_Load(object sender, EventArgs e)
        {
            try
            {
                ItemController itemLogic = new ItemController();
                //find item using the given text      

                Item itemmatrix = ItemController.GetDataItemMatrixWithAttributes1(Attributes1);

                lblItemName.Text = itemmatrix.Attributes2;
                lblItemNo.Text = itemmatrix.Attributes1;
                lblDescription.Text = itemmatrix.ItemDesc;

                //DataSet coll = SPs.GetViewMatrixStockOnHand(InventoryLocationName,Attributes1).GetDataSet();
                //DataTable table = coll.Tables[0];

                DataTable dt = ReportController.FetchStockReport(Attributes1, InventoryLocationID, false, "", "ItemNo", "ASC");

                Query qr = Item.Query();
                qr.SelectList = "Attributes4";
                qr.WHERE("Attributes1", Comparison.Equals, Attributes1);
                qr.WHERE("Deleted", Comparison.Equals, false);
                DataSet set1 = qr.DISTINCT().ExecuteDataSet();

                Query qr2 = Item.Query();
                qr.SelectList = "Attributes3";
                qr.WHERE("Attributes1", Comparison.Equals, Attributes1);
                qr.WHERE("Deleted", Comparison.Equals, false);
                DataSet set2 = qr.DISTINCT().ExecuteDataSet();

                DataTable ds = new DataTable();
                ds.Columns.Add("Item");

                for (int i = 0; i < set2.Tables[0].Rows.Count; i++)
                {
                    var att3 = set2.Tables[0].Rows[i][0].ToString();
                    if(!string.IsNullOrEmpty(att3))
                        ds.Columns.Add(att3);
                }

                foreach (DataRow row2 in set1.Tables[0].Rows)
                {
                    var att4 = row2[0].ToString();
                    DataRow row = ds.NewRow();

                    row[0] = att4;
                    for (int i = 0; i < set2.Tables[0].Rows.Count; i++)
                    {
                        var att3 = set2.Tables[0].Rows[i][0].ToString();

                        int stock = GetStockHand(dt, att3, att4);
                        row[i + 1] = stock;
                    }
                    ds.Rows.Add(row);
                }

                dgvMatrix.AutoGenerateColumns = true;
                this.dgvMatrix.DataSource = ds;

                this.dgvMatrix.Columns[0].ReadOnly = true;
                this.dgvMatrix.Columns[0].HeaderText = "";

                for (int i = 0; i < dgvMatrix.Rows.Count; i++)
                {
                    for (int j = 1; j < dgvMatrix.Columns.Count; j++)
                    {
                        string att4 = dgvMatrix.Rows[i].Cells[0].Value.ToString();
                        var att3 = dgvMatrix.Columns[j].HeaderText;

                        Query qrs = Item.Query();
                        qrs.WHERE("Attributes1", Comparison.Equals, Attributes1);
                        qrs.WHERE("Attributes3", Comparison.Equals, att3);
                        qrs.WHERE("Attributes4", Comparison.Equals, att4);


                        DataGridViewCellStyle style = new DataGridViewCellStyle();
                        style.BackColor = Color.Gray;
                        ItemCollection col = itemLogic.FetchByQuery(qrs);
                        if (col != null && col.Count > 0)
                        {
                            if (col[0].Deleted == true)
                            {
                                dgvMatrix.Rows[i].Cells[j].Value = "";
                                dgvMatrix.Rows[i].Cells[j].Style = style;
                                dgvMatrix.Rows[i].Cells[j].ReadOnly = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show(LanguageManager.An_unknown_error_has_been_encountered__Please_contact_your_system_administrator_, LanguageManager.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int GetStockHand(DataTable dt, string att3, string att4)
        {
            int objreturn = 0;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if ((dt.Rows[i]["Attributes1"].ToString() == Attributes1) && (dt.Rows[i]["Attributes3"].ToString() == att3) && (dt.Rows[i]["Attributes4"].ToString() == att4))
                {
                    objreturn += Int32.Parse(dt.Rows[i]["OnHand"].ToString() ?? "0");
                }
            }

            return objreturn;
        }
    }
}
