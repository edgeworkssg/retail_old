using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PowerPOS.Container;
using System.Collections;
using PowerPOS;
using SubSonic;
using LanguageManager = PowerInventory.Properties.InventoryLanguage;

namespace PowerInventory
{
    public partial class frmAddItem : Form
    {
        public string searchReq;
        public string criteria;
        public bool IsPurchaseOrder = false;
        ItemController itemLogic;
        public decimal PreferedDiscount;
        public ArrayList itemNumbers;
        public ArrayList itemNames;
        public ArrayList descriptions;
        public ArrayList userflag1s;
        public ArrayList attributes1;
        public string SupplierName;

        #region "Form Initialization and loading"
        public frmAddItem()
        {
            Program.LoadCultureCode();
            InitializeComponent();
            if (string.IsNullOrEmpty(criteria)) criteria = "contains"; // contains | starts with | ends with | exact match
        }
        private void frmAddItem_Load(object sender, EventArgs e)
        {
            try
            {
                itemLogic = new ItemController();

                //find item using the given text            
                //ViewItemCollection coll = itemLogic.SearchItem(searchReq, true, false);


                //item exist?
                //dgvItemList.AutoGenerateColumns = false;
                //this.dgvItemList.DataSource = coll;
                //this.dgvItemList.Refresh();
                bool suppliercompulsory = IsPurchaseOrder ?  AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.SupplierIsCompulsoryPO), false) : AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.SupplierIsCompulsory), false);
                if (SupplierName == null || SupplierName == "" || !suppliercompulsory)
                {
                    //find item using the given text            
                    ViewItemCollection coll = itemLogic.SearchItem(searchReq, true, false, false, criteria);
                    //item exist?
                    dgvItemList.AutoGenerateColumns = false;
                    this.dgvItemList.DataSource = coll;
                    this.dgvItemList.Refresh();
                }
                else
                {
                    string searchText;
                    if (criteria.ToLower() == "contains")
                        searchText = "%" + searchReq + "%";
                    else if (criteria.ToLower() == "starts with")
                        searchText = searchReq + "%";
                    else if (criteria.ToLower() == "ends with")
                        searchText = "%" + searchReq;
                    else if (criteria.ToLower() == "exact match")
                        searchText = searchReq;
                    else
                        searchText = "%" + searchReq + "%";

                    ViewItemSupplierCollection coll = new ViewItemSupplierCollection();
                    //coll.Where(ViewItemSupplier.Columns.ItemName, SubSonic.Comparison.Like, searchText);
                    //coll.Where(ViewItemSupplier.Columns.SupplierName, SupplierName);
                    string sql = "SELECT * FROM ViewItemSupplier WHERE (ItemNo LIKE N'{0}' OR ItemName LIKE N'{0}') AND SupplierName = N'{1}'";
                    sql = string.Format(sql, searchText.Replace("'", "''"), SupplierName.Replace("'", "''"));
                    coll.Load(DataService.GetDataSet(new QueryCommand(sql, "PowerPOS")).Tables[0]);

                    //item exist?
                    dgvItemList.AutoGenerateColumns = false;
                    this.dgvItemList.DataSource = coll;
                    this.dgvItemList.Refresh();
                }

            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show(LanguageManager.An_unknown_error_has_been_encountered__Please_contact_your_system_administrator_, LanguageManager.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        #endregion

        #region "Add Item Logic"
        private void btnAddItem_Click(object sender, EventArgs e)
        {
            pnlLoading.Visible = true;
            pnlLoading.BringToFront();

            try
            {
                itemNumbers = new ArrayList();
                itemNames = new ArrayList();
                descriptions = new ArrayList();
                userflag1s = new ArrayList();
                attributes1 = new ArrayList();  

                //Add item from the selected checkboxes into the gridview
                for (int i = 0; i < dgvItemList.Rows.Count; i++)
                {
                    //if (dgvItemList.Rows[i].Cells[0]
                    if (dgvItemList.Rows[i].Cells[0].Value is bool &&
                        (bool)dgvItemList.Rows[i].Cells[0].Value == true)
                    {
                        itemNumbers.Add(dgvItemList.Rows[i].Cells[1].Value);
                        itemNames.Add(dgvItemList.Rows[i].Cells[2].Value);
                        descriptions.Add(dgvItemList.Rows[i].Cells[3].Value);
                        userflag1s.Add(dgvItemList.Rows[i].Cells["Userflag1"].Value);

                        if (dgvItemList.Rows[i].Cells["Userflag1"].Value.ToString().ToLower() == "true")
                        {
                            attributes1.Add(dgvItemList.Rows[i].Cells["colAttributes1"].Value.ToString());
                        }
                        else 
                        {
                            //for checking needed show form matrix
                            attributes1.Add("NoAttributes1");
                        }
                    }
                }
                pnlLoading.Visible = false;
                pnlLoading.SendToBack();

                this.Close();
            }
            catch (Exception ex)
            {
                pnlLoading.Visible = false;
                pnlLoading.SendToBack();
                Logger.writeLog(ex);
                MessageBox.Show(LanguageManager.An_unknown_error_has_been_encountered__Please_contact_your_system_administrator_, LanguageManager.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region "Close Form"
        private void btnCancelAdd_Click(object sender, EventArgs e)
        {
            try
            {
                itemNumbers = null;
                this.Close();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show(LanguageManager.An_unknown_error_has_been_encountered__Please_contact_your_system_administrator_, LanguageManager.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        private void dgvItemList_CellClick
            (object sender, DataGridViewCellEventArgs e)
        {
            /*
            if (e.RowIndex >= 0 && e.ColumnIndex == 1)
            {
                //Get the item no            
                itemNumbers = new ArrayList();
                itemNumbers.Add(dgvItemList.Rows[e.RowIndex].Cells[1].Value);
                this.Close();
            }*/
        }

        private void llSelectAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            for (int i = 0; i < dgvItemList.Rows.Count; i++)
            {
                dgvItemList.Rows[i].Cells[0].Value = true;
            }
        }

        private void llSelectNone_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            for (int i = 0; i < dgvItemList.Rows.Count; i++)
            {
                dgvItemList.Rows[i].Cells[0].Value = false;
            }
        }

        private void llInvert_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            for (int i = 0; i < dgvItemList.Rows.Count; i++)
            {
                if (dgvItemList.Rows[i].Cells[0].Value is bool)
                {
                    dgvItemList.Rows[i].Cells[0].Value = !((bool)dgvItemList.Rows[i].Cells[0].Value);
                }
                else
                {
                    dgvItemList.Rows[i].Cells[0].Value = true;
                }
            }
        }

        private void cbShowDetails_CheckedChanged(object sender, EventArgs e)
        {
            bool isShown = cbShowDetails.Checked;

            colCategoryName.Visible = isShown;
            colDepartment.Visible = isShown;
            colAttributes1.Visible = isShown;
            colAttributes2.Visible = isShown;
            colAttributes3.Visible = isShown;
            colAttributes4.Visible = isShown;
            colAttributes5.Visible = isShown;
            colAttributes6.Visible = isShown;
            colAttributes7.Visible = isShown;
            colAttributes8.Visible = isShown;
        }
    }
}
