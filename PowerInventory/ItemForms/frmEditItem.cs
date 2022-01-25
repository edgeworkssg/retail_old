using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using PowerPOS.Container;
using System.IO;
using PowerPOS;
namespace PowerInventory.ItemForms
{
    public partial class frmEditItem : Form
    {
        public string ItemRefNo,imageFilePath;
        public decimal ItemCost = 0;
        public bool IsUseItemCost = false;
        Item myItem;
        bool IsAdd;
        bool showCostPrice;


        public frmEditItem()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAddCat_Click(object sender, EventArgs e)
        {
            try
            {
                frmAddItemCategory addCat = new frmAddItemCategory();
                CommonUILib.displayTransparent();addCat.ShowDialog();CommonUILib.hideTransparent();
                addCat.Dispose();

                cmbCategoryName.DataSource = ItemController.FetchCategoryNames();
                cmbCategoryName.Refresh();
            }
            catch (Exception ex)
            {   Logger.writeLog(ex);
                MessageBox.Show("An unknown error has been encountered. Please contact your system administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public bool IsReadOnly;
        private void DisableControls()
        {
            txtRefNo.ReadOnly = true; 
            txtItemName.ReadOnly = true;
            txtBarcode.ReadOnly = true; 
            txtPrice.ReadOnly = true; 
            cbInventory.Enabled = false; 
            cbIsNonDiscountable.Enabled = false;
            cmbCategoryName.Enabled = false;
            txtItemDesc.ReadOnly = true;
            txtRemark.ReadOnly = true;            
            txtAuthor.ReadOnly = true;
            txtLanguage.ReadOnly = true;
            txtPublisher.ReadOnly = true;
            txtTopic.ReadOnly = true;
            txtType.ReadOnly = true;
            txtCostPrice.ReadOnly = true;
            txtCurrency.ReadOnly = true;
            btnAddEdit.Enabled = false;
            btnAlternateBarcode.Enabled = false;
        }

        private void frmEditItem_Load(object sender, EventArgs e)
        {
            try
            {
                showCostPrice = !(AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.HideInventoryCost), false));
                if (!showCostPrice)
                {
                    lblCostPrice.Visible = false;
                    txtCostPrice.Visible = false;
                }
                //if (!PrivilegesController.HasPrivilege("Goods Receive", UserInfo.privileges))
                //{
                //    txtCostPrice.Visible = false;
                //}
                if (!PrivilegesController.HasPrivilege("Product Additional", UserInfo.privileges))
                {
                    pnlAdditional.Visible = false;
                }
                //Load Controls
                lblAttributes1.Text = ProductAttributeInfo.Attributes1;
                lblAttributes2.Text = ProductAttributeInfo.Attributes2;
                lblAttributes3.Text = ProductAttributeInfo.Attributes3;
                lblAttributes4.Text = ProductAttributeInfo.Attributes4;
                lblAttributes5.Text = ProductAttributeInfo.Attributes5;

               
                //DisableControls();
                //panel1.Enabled = false;
                //btnAddEdit.Visible = false;

                ItemDepartmentCollection itemDept = new ItemDepartmentCollection();
                itemDept.Where(ItemDepartment.Columns.Deleted,false);
                itemDept.Load();
                itemDept.Insert(0, new ItemDepartment());
                
                cmbDepartment.DataSource = itemDept;
                cmbDepartment.Refresh();
                cmbDepartment.SelectedIndex = 1;

                cmbCategoryName.DataSource = ItemController.FetchCategoryNames();
                cmbCategoryName.Refresh();
                cmbCategoryName.SelectedIndex = 1;
                
                //edit or add
                if (ItemRefNo == null | ItemRefNo == "")
                {
                    //Add
                    myItem = new Item();
                    IsAdd = true;
                    //myItem.ItemNo = ItemController.getNewItemRefNo();
                    txtRefNo.Text = ItemController.getNewItemRefNo();
                    //txtRefNo.Text = myItem.ItemNo;
                }
                else
                {
                    IsAdd = false;
                    //Edit
                    myItem = new Item(ItemRefNo);
                    if (myItem == null)
                    {
                        MessageBox.Show("The Item you specified is invalid.");
                        this.Close();
                    }
                    if (File.Exists(Application.StartupPath + "\\Products\\" + myItem.ItemNo + ".jpg"))
                    {
                        pbImage.Image = Image.FromFile(Application.StartupPath + "\\Products\\" + myItem.ItemNo + ".jpg");
                        pbImage.Refresh();
                    }
                    txtRefNo.ReadOnly = true;
                    txtRefNo.Text = myItem.ItemNo;
                    txtItemName.Text = myItem.ItemName;
                    txtBarcode.Text = myItem.Barcode;                    
                    txtPrice.Text = myItem.RetailPrice.ToString("N");
                    if (IsUseItemCost)
                    {
                        txtCostPrice.Text = ItemCost.ToString("N");
                    }
                    else
                    {
                        txtCostPrice.Text = myItem.FactoryPrice.ToString("N");
                    }
                    cbInventory.Checked = myItem.IsInInventory;
                    //txtCurrency.Text = myItem.FactoryPriceCurrency;
                    cbIsNonDiscountable.Checked = myItem.IsNonDiscountable;
                    for (int i = 0; i < cmbDepartment.Items.Count; i++)
                    {
                        if (((ItemDepartment)cmbDepartment.Items[i]).ItemDepartmentID == myItem.Category.ItemDepartmentId)
                        {
                            cmbDepartment.SelectedIndex = i;
                        }                                                
                    }
                    for (int i = 0; i < cmbCategoryName.Items.Count; i++)
                    {
                        if (cmbCategoryName.Items[i].ToString() == myItem.CategoryName)
                        {
                            cmbCategoryName.SelectedIndex = i;
                        }
                    }
                    
                    txtItemDesc.Text = myItem.ItemDesc;
                    txtRemark.Text = myItem.Remark;
                    btnAddEdit.Text = "Edit Item";
                    txtAuthor.Text = myItem.Attributes1;
                    txtLanguage.Text = myItem.Attributes5;
                    txtPublisher.Text = myItem.Attributes2;
                    txtTopic.Text = myItem.Attributes4;
                    txtType.Text = myItem.Attributes3;                    
                }
                txtRefNo.Focus();
                txtRefNo.Select();
                if (IsReadOnly == true)
                {
                    DisableControls();
                }
                if (myItem.ItemImage != null)
                {
                    picPhoto.Image = Image.FromStream(new MemoryStream(myItem.ItemImage));
                }
                else
                {
                    picPhoto.Image = null;
                }
                picPhoto.Refresh();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("An unknown error has been encountered. Please contact your system administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddEdit_Click(object sender, EventArgs e)
        {
            try
            {
                decimal price = 0, minprice;

                if (cmbDepartment.SelectedIndex <= 0)
                {
                    MessageBox.Show("Please select item department");
                    cmbDepartment.Select();
                    return;
                }
                if (cmbCategoryName.SelectedIndex  <= 0)
                {
                    MessageBox.Show("Please select item category");
                    cmbCategoryName.Select();
                    return;
                }
                if (!decimal.TryParse(txtPrice.Text, out price) || price < 0)
                {
                    MessageBox.Show("Price must be a non-negative number", "", MessageBoxButtons.OK,MessageBoxIcon.Error);
                    return;
                }
                if (txtCostPrice.Text != "")
                {
                    if (!decimal.TryParse(txtCostPrice.Text, out minprice) || minprice < 0)
                    {
                        MessageBox.Show("Cosr Price must be a non-negative number.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    minprice = 0;
                }
                myItem.ItemNo = txtRefNo.Text;
                myItem.ItemName = txtItemName.Text;
                myItem.Barcode = txtBarcode.Text;                
                myItem.CategoryName = cmbCategoryName.SelectedItem.ToString();
                myItem.FactoryPrice = minprice;
                myItem.RetailPrice = price;
                myItem.IsInInventory = cbInventory.Checked;
                myItem.Remark = txtRemark.Text;
                myItem.ItemDesc = txtItemDesc.Text;
                myItem.Attributes4 = txtTopic.Text;
                myItem.Attributes1 = txtAuthor.Text;
                myItem.Attributes2 = txtPublisher.Text;
                myItem.Attributes3 = txtType.Text;
                myItem.Attributes5 = txtLanguage.Text;
                //myItem.FactoryPriceCurrency = txtCurrency.Text;
                myItem.IsNonDiscountable = cbIsNonDiscountable.Checked;
                myItem.Deleted = false;
                myItem.Save();

                if (IsAdd)
                {
                    MessageBox.Show("Item Added");
                    txtRefNo.Text = "";
                    txtRefNo.Text = "";
                    txtItemName.Text = "";
                    txtBarcode.Text = "";
                    txtPrice.Text = "";
                    txtCostPrice.Text = "";
                    txtCurrency.Text = "";
                    cbInventory.Checked = true;
                    cbIsNonDiscountable.Checked = false;
                    cmbCategoryName.SelectedIndex = 0;
                    txtItemDesc.Text = "";
                    txtRemark.Text = "";                    
                    txtAuthor.Text = "";
                    txtLanguage.Text = "";
                    txtPublisher.Text = "";
                    txtTopic.Text = "";
                    txtType.Text = "";
                    txtRefNo.Focus();
                    txtRefNo.Select();
                    pbImage.Image = null;
                    pbImage.Refresh();
                    myItem = new Item();
                }
                else
                {
                    MessageBox.Show("Save successful");
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("PK_"))
                {
                    MessageBox.Show("The item no [" + txtRefNo.Text + "] has already been used.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtRefNo.Select();
                }
                else
                {
                    MessageBox.Show("An unknown error has been encountered. Please contact your system administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                Logger.writeLog(ex);
                
            }
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void cmbDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            //repopulate
            if (cmbDepartment.SelectedIndex == 0)
            {
                cmbCategoryName.DataSource = null;
                cmbCategoryName.Refresh();
            }
            else
            {
                cmbCategoryName.DataSource = ItemController.FetchCategoryNames(((ItemDepartment)cmbDepartment.SelectedValue).ItemDepartmentID);
                cmbCategoryName.Refresh();
            }
        }

        private void btnAlternateBarcode_Click(object sender, EventArgs e)
        {
            if (!IsAdd)
            {
                frmAlternateBarcode f = new frmAlternateBarcode();
                f.itemNo = txtRefNo.Text;
                CommonUILib.displayTransparent();f.ShowDialog();CommonUILib.hideTransparent();
            }
        }

        private void btnSetImage_Click(object sender, EventArgs e)
        {
            CommonUILib.displayTransparent();saveFileDialog1.ShowDialog();CommonUILib.hideTransparent();
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            //Check extention...
            
            txtSaveImage.Text = saveFileDialog1.FileName;
            Image img = Image.FromFile(saveFileDialog1.FileName);
            pbImage.Image = img;
            img.Save(Application.StartupPath + "\\Products\\" + txtRefNo.Text + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
            pbImage.Refresh();
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void btnViewActivity_Click(object sender, EventArgs e)
        {
            frmItemTraceTool f = new frmItemTraceTool();
            f.txtItemNo.Text = txtRefNo.Text;
            f.ShowDialog();
            f.Dispose();
        }        
    }
}