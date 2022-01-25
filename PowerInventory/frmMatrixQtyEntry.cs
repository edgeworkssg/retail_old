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
using LanguageManager = PowerInventory.Properties.InventoryLanguage;
using SubSonic;

namespace PowerInventory
{
    public partial class frmMatrixQtyEntry : Form
    {
        public string Attributes1;
        public ArrayList ItemNoList;
        public ArrayList ItemQty;
        public ItemController itemLogic;
        public bool IsSuccessful;

        public frmMatrixQtyEntry(string attributes1)
        {
            this.Attributes1 = attributes1;

            InitializeComponent();
        }

        private void frmMatrixItem_Load(object sender, EventArgs e)
        {
            IsSuccessful = false;
            try
            {
                itemLogic = new ItemController();
                //find item using the given text      

                Item itemmatrix = ItemController.GetDataItemMatrixWithAttributes1(Attributes1);

                lblItemName.Text = itemmatrix.Attributes2;
                lblItemNo.Text = itemmatrix.Attributes1;
                lblDescription.Text = itemmatrix.ItemDesc;

                Query qr = Item.Query();
                qr.SelectList = "Attributes4";
                qr.WHERE(Item.Columns.Attributes1, Comparison.Equals, Attributes1);
                qr.WHERE(Item.Columns.Deleted, Comparison.Equals, false);
                qr.WHERE(Item.Columns.Userflag1, Comparison.Equals, true);
                DataSet set1 = qr.DISTINCT().ExecuteDataSet();

                Query qr2 = Item.Query();
                qr.SelectList = "Attributes3";
                qr.WHERE(Item.Columns.Attributes1, Comparison.Equals, Attributes1);
                qr.WHERE(Item.Columns.Deleted, Comparison.Equals, false);
                qr.WHERE(Item.Columns.Userflag1, Comparison.Equals, true);
                DataSet set2 = qr.DISTINCT().ExecuteDataSet();

                DataTable ds = new DataTable();
                ds.Columns.Add("Item");

                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.SwitchColumnRowsMatrixForm), false))
                {
                    for (int i = 0; i < set1.Tables[0].Rows.Count; i++)
                    {
                        var att4 = set1.Tables[0].Rows[i][0].ToString();
                        if (!string.IsNullOrEmpty(att4))
                            ds.Columns.Add(att4);
                    }

                    foreach (DataRow row2 in set2.Tables[0].Rows)
                    {
                        var att3 = row2[0].ToString();
                        DataRow row = ds.NewRow();

                        row[0] = att3;
                        for (int i = 0; i < set1.Tables[0].Rows.Count; i++)
                        {
                            var att4 = set1.Tables[0].Rows[i][0].ToString();
                            row[i + 1] = 0;
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
                            string att3 = dgvMatrix.Rows[i].Cells[0].Value.ToString();
                            var att4 = dgvMatrix.Columns[j].HeaderText;

                            Query qrs = Item.Query();
                            qrs.WHERE("Attributes1", Comparison.Equals, Attributes1);
                            qrs.WHERE("Attributes3", Comparison.Equals, att3);
                            qrs.WHERE("Attributes4", Comparison.Equals, att4);


                            DataGridViewCellStyle style = new DataGridViewCellStyle();
                            style.BackColor = Color.Gray;
                            ItemCollection col = itemLogic.FetchByQuery(qrs);
                            if (col != null && col.Count > 0)
                            {
                                bool isSetDeleted = true;
                                for (int l = 0; l < col.Count; l++)
                                {
                                    if (col[l].Deleted == false)
                                    {
                                        isSetDeleted = false;
                                        break;
                                    }
                                }

                                if (isSetDeleted)
                                {
                                    dgvMatrix.Rows[i].Cells[j].Value = "";
                                    dgvMatrix.Rows[i].Cells[j].Style = style;
                                    dgvMatrix.Rows[i].Cells[j].ReadOnly = true;
                                }
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < set2.Tables[0].Rows.Count; i++)
                    {
                        var att3 = set2.Tables[0].Rows[i][0].ToString();
                        if (!string.IsNullOrEmpty(att3))
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
                            row[i + 1] = 0;
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
                                bool isSetDeleted = true;
                                for (int l = 0; l < col.Count; l++)
                                {
                                    if (col[l].Deleted == false)
                                    {
                                        isSetDeleted = false;
                                        break;
                                    }
                                }

                                if (isSetDeleted)
                                {
                                    dgvMatrix.Rows[i].Cells[j].Value = "";
                                    dgvMatrix.Rows[i].Cells[j].Style = style;
                                    dgvMatrix.Rows[i].Cells[j].ReadOnly = true;
                                }
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

        private void btnCancelAdd_Click(object sender, EventArgs e)
        {
            try
            {
                IsSuccessful = false;
                this.Close();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show(LanguageManager.An_unknown_error_has_been_encountered__Please_contact_your_system_administrator_, LanguageManager.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddItems_Click(object sender, EventArgs e)
        {
            try
            {
                IsSuccessful = false;
                ItemNoList = new ArrayList();
                ItemQty = new ArrayList();

                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.SwitchColumnRowsMatrixForm), false))
                {
                    for (int i = 0; i < dgvMatrix.Rows.Count; i++)
                    {
                        for (int j = 1; j < dgvMatrix.Columns.Count; j++)
                        {
                            string att3 = dgvMatrix.Rows[i].Cells[0].Value.ToString();

                            var att4 = dgvMatrix.Columns[j].HeaderText;

                            string strqty = dgvMatrix.Rows[i].Cells[j].Value.ToString();
                            if (strqty != null && strqty != "")
                            {
                                var qty = 0;

                                if (int.TryParse(strqty, out qty))
                                {
                                    if (qty > 0)
                                    {
                                        Query qrs = new Query("Item");
                                        qrs.QueryType = QueryType.Select;
                                        qrs.SelectList = "ItemNo";
                                        qrs.AddWhere(Item.Columns.Attributes1, Comparison.Equals, Attributes1);
                                        qrs.AddWhere(Item.Columns.Attributes3, Comparison.Equals, att3);
                                        qrs.AddWhere(Item.Columns.Attributes4, Comparison.Equals, att4);
                                        qrs.AddWhere(Item.Columns.Deleted, Comparison.Equals, false);
                                        DataSet ds = qrs.ExecuteDataSet();
                                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                        {
                                            ItemNoList.Add(ds.Tables[0].Rows[0]["ItemNo"].ToString());
                                            ItemQty.Add(qty);
                                        }
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Invalid quantity for matrix input ");
                                    IsSuccessful = false;
                                    return;
                                }
                            }

                            IsSuccessful = true;

                        }
                    }
                }
                else
                {
                    for (int i = 0; i < dgvMatrix.Rows.Count; i++)
                    {
                        for (int j = 1; j < dgvMatrix.Columns.Count; j++)
                        {
                            string att4 = dgvMatrix.Rows[i].Cells[0].Value.ToString();

                            var att3 = dgvMatrix.Columns[j].HeaderText;

                            string strqty = dgvMatrix.Rows[i].Cells[j].Value.ToString();
                            if (strqty != null && strqty != "")
                            {
                                var qty = 0;

                                if (int.TryParse(strqty, out qty))
                                {
                                    if (qty > 0)
                                    {
                                        Query qrs = new Query("Item");
                                        qrs.QueryType = QueryType.Select;
                                        qrs.SelectList = "ItemNo";
                                        qrs.AddWhere(Item.Columns.Attributes1, Comparison.Equals, Attributes1);
                                        qrs.AddWhere(Item.Columns.Attributes3, Comparison.Equals, att3);
                                        qrs.AddWhere(Item.Columns.Attributes4, Comparison.Equals, att4);
                                        qrs.AddWhere(Item.Columns.Deleted, Comparison.Equals, false);
                                        DataSet ds = qrs.ExecuteDataSet();
                                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                        {
                                            ItemNoList.Add(ds.Tables[0].Rows[0]["ItemNo"].ToString());
                                            ItemQty.Add(qty);
                                        }
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Invalid quantity for matrix input ");
                                    IsSuccessful = false;
                                    return;
                                }
                            }

                            IsSuccessful = true;

                        }
                    }
                }

                this.Close();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show(LanguageManager.An_unknown_error_has_been_encountered__Please_contact_your_system_administrator_, LanguageManager.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
