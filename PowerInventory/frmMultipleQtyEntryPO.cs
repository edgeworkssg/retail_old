using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using PowerPOS;

namespace PowerInventory
{
    public partial class frmMultipleQtyEntryPO : Form
    {
        public ArrayList itemNos;
        public ArrayList itemNames;
        public ArrayList descriptions;
        private int supplierID = 0;
        private bool showPackingSize = false;
        public frmMultipleQtyEntryPO(int supplierID)
        {
            InitializeComponent();
            this.supplierID = supplierID;
            showPackingSize = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowPackingSize), false);
        }

        public bool IsSuccessful;
        public DataTable SelectedItems = new DataTable();
        private void frmMultipleQtyEntry_Load(object sender, EventArgs e)
        {
            try
            {
                IsSuccessful = false;
                if (itemNos == null || itemNames == null || descriptions == null || itemNos.Count == 0)
                    this.Close();

                #region *) Instantiate SelectedItems

                SelectedItems = new DataTable();
                SelectedItems.Columns.Add("ItemNo", typeof(string));
                SelectedItems.Columns.Add("Qty", typeof(decimal));
                SelectedItems.Columns.Add("PackingSizeName", typeof(string));
                SelectedItems.Columns.Add("PackingSizeUOM", typeof(decimal));
                SelectedItems.Columns.Add("PackingSizeCostPrice", typeof(decimal));
                #endregion

                for (int i = 0; i < itemNos.Count; i++)
                {
                    Label tmp;
                    tmp = new Label();
                    tmp.AutoSize = true;
                    if (i % 2 == 0)
                    {
                        tmp.BackColor = Color.Gainsboro;
                    }
                    string status = "";
                    string MainItemNo = ItemSupplierMapController.FetchMainItemNonInvProduct(itemNos[i].ToString(), out status);

                    tmp.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                            | System.Windows.Forms.AnchorStyles.Left)
                            | System.Windows.Forms.AnchorStyles.Right)));
                    tmp.Text = MainItemNo;
                    tblQuantity.Controls.Add(tmp, 0, i);
                    tmp = new Label();
                    if (i % 2 == 0)
                    {
                        tmp.BackColor = Color.Gainsboro;
                    }
                    tmp.AutoSize = true;
                    tmp.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                            | System.Windows.Forms.AnchorStyles.Left)
                            | System.Windows.Forms.AnchorStyles.Right)));
                    
                    if (MainItemNo != itemNos[i].ToString())
                    { 
                        Item itm = new Item(MainItemNo);
                        tmp.Text = itm.ItemName;
                    }
                    else
                    {
                        if (itemNames[i] != null) tmp.Text = itemNames[i].ToString();
                    }


                    tblQuantity.Controls.Add(tmp, 1, i);
                    tmp = new Label();
                    tmp.AutoSize = true;
                    if (i % 2 == 0)
                    {
                        tmp.BackColor = Color.Gainsboro;
                    }
                    tmp.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                            | System.Windows.Forms.AnchorStyles.Left)
                            | System.Windows.Forms.AnchorStyles.Right)));
                    if (descriptions[i] != null) tmp.Text = descriptions[i].ToString();
                    tblQuantity.Controls.Add(tmp, 2, i);

                    TextBox txt = new TextBox();

                    txt.Name = itemNos[i].ToString(); //assuming no duplicate allowed
                    txt.Text = "0";
                    txt.Tag = i;
                    txt.KeyDown += txt_KeyDown;
                    txt.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                            | System.Windows.Forms.AnchorStyles.Left)
                            | System.Windows.Forms.AnchorStyles.Right)));

                    tblQuantity.Controls.Add(txt, 3, i);
                    var packingSize = new List<PackingSize>();
                   
                    packingSize = PurchaseOrderController.FetchPackingSizeByItemNoAndSupplierForDropdownNew(MainItemNo, supplierID);
                    ComboBox tmpUOM = new ComboBox();
                    tmpUOM.DropDownStyle = ComboBoxStyle.DropDownList;
                    tmpUOM.Name = "cmbPackingSize" + i;
                    tmpUOM.AutoSize = true;

                    if (i % 2 == 0)
                        tmpUOM.BackColor = Color.Gainsboro;
                    tmpUOM.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                            | System.Windows.Forms.AnchorStyles.Left)
                            | System.Windows.Forms.AnchorStyles.Right)));

                    tmpUOM.DisplayMember = "PackingSizeName";
                    tmpUOM.ValueMember = "PackingSizeName";
                    tmpUOM.DataSource = packingSize;
                    
                    
                    tmpUOM.Refresh();
                    tmpUOM.Visible = packingSize.Count > 0;
                    tmpUOM.Enabled = packingSize.Count > 1;
                    tmpUOM.SelectedIndexChanged += new EventHandler(tmpUOM_SelectedIndexChanged);

                    Label lblUOM = new Label();
                    lblUOM.Name = "lblPackingSize" + i;
                    //if(packingSize.Count>=1)
                    //    lblUOM.Text = string.Format("x {0} {1}", packingSize[0].PackingSizeUOM.ToString("N0"), packingSize[0].BaseUOM);
                    lblUOM.Text = "";
                    lblUOM.Margin = new Padding(5, 5, 0, 0);
                    lblUOM.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                            | System.Windows.Forms.AnchorStyles.Left)
                            | System.Windows.Forms.AnchorStyles.Right)));
                    if (tmpUOM.Enabled)
                    {
                        tblQuantity.Controls.Add(tmpUOM, 4, i);
                        tblQuantity.Controls.Add(lblUOM, 5, i);
                    }

                    //check if NIP then set selected uom
                    if (MainItemNo != itemNos[i].ToString())
                    {
                        Item NIP = new Item(itemNos[i].ToString());

                        if (!string.IsNullOrEmpty(NIP.UOM))
                        {
                            tmpUOM.SelectedValue = NIP.UOM;
                        }
                    }

                    if (packingSize.Count >= 1)
                        tmpUOM_SelectedIndexChanged(tmpUOM, new EventArgs());
                }
                tblQuantity.RowCount = itemNos.Count;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void tmpUOM_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            if (cb != null)
            {
                int index = cb.Name.Replace("cmbPackingSize", "").GetIntValue();
                Label tmp = (Label)tblQuantity.GetControlFromPosition(5, index);
                PackingSize selectedPacking = (PackingSize)cb.SelectedItem;
                if (selectedPacking != null)
                {
                    if (selectedPacking.PackingSizeName == selectedPacking.BaseUOM)
                    {
                        tmp.Text = "";
                    }
                    else
                    {
                        tmp.Text = string.Format("x {0} {1}", selectedPacking.PackingSizeUOM.ToString("N0"), selectedPacking.BaseUOM);
                    }
                }
            }
        }

        private void txt_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Down)
                {
                    if ((int)((TextBox)sender).Tag < tblQuantity.RowCount - 1) SendKeys.Send("{TAB}");
                }
                else if (e.KeyCode == Keys.Up)
                {
                    if ((int)((TextBox)sender).Tag > 0) SendKeys.Send("+{TAB}");
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < itemNos.Count; i++)
                {
                    Label tmp = (Label)tblQuantity.GetControlFromPosition(0, i);
                    TextBox txt = (TextBox)tblQuantity.GetControlFromPosition(3, i);
                    ComboBox cb = (ComboBox)tblQuantity.GetControlFromPosition(4, i);
                    var newRow = SelectedItems.NewRow();

                    decimal qty = 0;
                    if (decimal.TryParse(txt.Text, out qty))
                    {
                        string itemNo = tmp.Text;
                        newRow["ItemNo"] = itemNo;
                        newRow["Qty"] = qty;

                        PackingSize selectedPacking = null;
                        if(cb!=null)
                            selectedPacking = (PackingSize)cb.SelectedItem;

                        if(selectedPacking == null)
                        {
                            newRow["PackingSizeName"] = "";
                            newRow["PackingSizeUOM"] = 1;
                            newRow["PackingSizeCostPrice"] = new Item(itemNo).FactoryPrice;
                        }
                        else
                        {
                            newRow["PackingSizeName"] = selectedPacking.PackingSizeName;
                            newRow["PackingSizeUOM"] = selectedPacking.PackingSizeUOM;
                            newRow["PackingSizeCostPrice"] = selectedPacking.PackingSizeCostPrice;
                        }
                        SelectedItems.Rows.Add(newRow);
                    }
                    else
                    {
                        MessageBox.Show("Invalid quantity for row " + i.ToString());
                        SelectedItems.Rows.Clear();
                        txt.Select();
                        return;
                    }
                }
                IsSuccessful = true;
                this.Close();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            IsSuccessful = false;
            this.Close();
        }
    }
}
