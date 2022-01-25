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
using SubSonic;
using PowerEdge.ItemForms;

namespace PowerEdge
{
    public partial class frmProductDataEntryFormStyle : Form
    {
        private ListBox lbCategoryNameAutoComplete;
        private ListBox lbItemNameAutoComplete;

        private const int X_CAT_POS = 215;
        private const int Y_CAT_POS = 320;

        private const int X_ITEM_POS = 215;
        private const int Y_ITEM_POS = 440;

        public frmProductDataEntryFormStyle()
        {
            try
            {
                InitializeComponent();

                
                lbCategoryNameAutoComplete = new ListBox();
                lbCategoryNameAutoComplete.Visible = false;
                lbCategoryNameAutoComplete.KeyDown += new KeyEventHandler(listBox1CategoryName_KeyDown);
                lbCategoryNameAutoComplete.Location = new Point(X_CAT_POS, Y_CAT_POS);


                lbItemNameAutoComplete = new ListBox();
                lbItemNameAutoComplete.Visible = false;
                lbItemNameAutoComplete.KeyDown += new KeyEventHandler(listBox1ItemName_KeyDown);
                lbItemNameAutoComplete.Location = new Point(X_ITEM_POS, Y_ITEM_POS);

                this.Controls.Add(lbCategoryNameAutoComplete);
                this.Controls.Add(lbItemNameAutoComplete);
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        private void txtGeneric_Enter(object sender, EventArgs e)
        {
            try
            {
                ((TextBox)sender).BackColor = Color.Yellow;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        private void txtGeneric_Leave(object sender, EventArgs e)
        {
            try
            {
                ((TextBox)sender).BackColor = Color.White;
            }catch(Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        private void txtGeneric_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Right || e.KeyCode == Keys.Down || e.KeyCode == Keys.Enter)
                {
                    SendKeys.Send("{Tab}");
                }
                else if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Up)
                {
                    SendKeys.Send("+{Tab}");
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        private void tableLayoutPanel1_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
            try
            {
                if (e.Row % 2 == 0)
                {
                    e.Graphics.FillRectangle(Brushes.LightSteelBlue, e.CellBounds);
                }
                else
                {
                    e.Graphics.FillRectangle(Brushes.LightSkyBlue, e.CellBounds);
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        private void btnSave_Enter(object sender, EventArgs e)
        {
            try
            {
                ((Button)sender).ForeColor = Color.Yellow;
                Font f = new Font("Verdana", 16, FontStyle.Bold);
                ((Button)sender).Font = f;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }

        }

        private void btnSave_Leave(object sender, EventArgs e)
        {
            try
            {
                ((Button)sender).ForeColor = Color.White;
                Font f = new Font("Verdana", 9, FontStyle.Bold);
                ((Button)sender).Font = f;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        private void txtCategoryName_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Escape)
                {
                    if (lbCategoryNameAutoComplete.Visible == true)
                    {
                        lbCategoryNameAutoComplete.Visible = false;
                    }
                }
                if (e.KeyCode == Keys.Enter)
                {
                    //if autocomplete is visible, add the words into the query
                    if (lbCategoryNameAutoComplete.Visible == true)
                    {
                        AddAutoSuggestCategoryName((TextBox)sender);
                    }
                    else //else send tabs
                    {
                        SendKeys.Send("{Tab}");
                    }
                }
                else if (e.KeyCode == Keys.Down)
                {
                    //if auto complete is on, activate it
                    if (lbCategoryNameAutoComplete.Visible)
                    {
                        if (lbCategoryNameAutoComplete.Items.Count > 1) lbCategoryNameAutoComplete.SelectedIndex = 1;
                        lbCategoryNameAutoComplete.Focus();
                    }
                    else
                    {
                        SendKeys.Send("{Tab}");
                    }
                }
                else if (e.KeyCode == Keys.Up)
                {
                    //if auto complete is on, activate it
                    if (!lbCategoryNameAutoComplete.Visible)
                    {
                        SendKeys.Send("+{Tab}");
                    }
                }
                else
                {
                    string senderTextBox = ((TextBox)sender).Text;
                    string query = "";
                    int lastIndex = senderTextBox.LastIndexOf(' ');
                    if (lastIndex < 0) lastIndex = 0;

                    query = senderTextBox.Substring(lastIndex).TrimStart();
                    if (query != "")
                    {
                        //Position the item to the screen
                        lbCategoryNameAutoComplete.Size = new Size(200, 200);

                        //populate data
                        lbCategoryNameAutoComplete.Items.Clear();

                        DataSet ds = SPs.FetchAutoCompleteWords(query).GetDataSet();
                        ArrayList arraylist = new ArrayList();
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            arraylist.Add(row["WordText"]);
                        }

                        lbCategoryNameAutoComplete.Items.AddRange(arraylist.ToArray());
                        //lbNameAutoComplete.DataSource = arraylist;
                        //lbNameAutoComplete.Refresh();
                        if (lbCategoryNameAutoComplete.Items.Count > 0)
                        {
                            lbCategoryNameAutoComplete.Visible = true;
                            lbCategoryNameAutoComplete.SelectedIndex = 0;
                            lbCategoryNameAutoComplete.BringToFront();
                        }
                        else
                        {
                            lbCategoryNameAutoComplete.Visible = false;
                        }
                    }
                    else
                    {
                        lbCategoryNameAutoComplete.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        private void AddAutoSuggestCategoryName(TextBox sender)
        {
            try
            {
                string senderTextBox = sender.Text;

                int lastIndex = senderTextBox.LastIndexOf(' ');
                if (lastIndex < 0) lastIndex = 0;
                string textBeforeAppend = "";
                if (lastIndex != 0)
                    textBeforeAppend = senderTextBox.Substring(0, lastIndex + 1);
                lbCategoryNameAutoComplete.Visible = false;
                sender.Text = textBeforeAppend + lbCategoryNameAutoComplete.SelectedItem.ToString();
                sender.Focus();
                sender.Select(sender.Text.Length, 0);
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        private void listBox1CategoryName_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {

                if (e.KeyCode == Keys.Enter)
                {
                    //add item into text...
                    AddAutoSuggestCategoryName(txtCategory);
                }
                else if (e.KeyCode == Keys.Up)
                {
                    if (lbCategoryNameAutoComplete.SelectedIndex == 0)
                    {
                        txtCategory.Focus();
                        txtCategory.Select(txtCategory.Text.Length, 0);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        private void txtItemName_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Escape)
                {
                    if (lbItemNameAutoComplete.Visible == true)
                    {
                        lbItemNameAutoComplete.Visible = false;
                    }
                }
                if (e.KeyCode == Keys.Enter)
                {
                    //if autocomplete is visible, add the words into the query
                    if (lbItemNameAutoComplete.Visible == true)
                    {
                        AddAutoSuggestItemName((TextBox)sender);
                    }
                    else //else send tabs
                    {
                        //lbItemNameAutoComplete.Visible = false;
                        SendKeys.Send("{Tab}");
                    }
                }
                else if (e.KeyCode == Keys.Down)
                {
                    //if auto complete is on, activate it
                    if (lbItemNameAutoComplete.Visible)
                    {
                        if (lbItemNameAutoComplete.Items.Count > 1) lbItemNameAutoComplete.SelectedIndex = 1;
                        lbItemNameAutoComplete.Focus();
                    }
                    else
                    {
                        SendKeys.Send("{Tab}");
                    }
                }
                else if (e.KeyCode == Keys.Up)
                {
                    //if auto complete is on, activate it
                    if (!lbItemNameAutoComplete.Visible)
                    {
                        SendKeys.Send("+{Tab}");
                    }
                }
                else
                {
                    string senderTextBox = ((TextBox)sender).Text;
                    string query = "";
                    int lastIndex = senderTextBox.LastIndexOf(' ');
                    if (lastIndex < 0) lastIndex = 0;

                    query = senderTextBox.Substring(lastIndex).TrimStart();
                    if (query != "")
                    {
                        //Position the item to the screen
                        lbItemNameAutoComplete.Size = new Size(200, 200);

                        //populate data
                        lbItemNameAutoComplete.Items.Clear();

                        DataSet ds = SPs.FetchAutoCompleteWords(query).GetDataSet();
                        ArrayList arraylist = new ArrayList();
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            arraylist.Add(row["WordText"]);
                        }

                        lbItemNameAutoComplete.Items.AddRange(arraylist.ToArray());
                        //lbNameAutoComplete.DataSource = arraylist;
                        //lbNameAutoComplete.Refresh();
                        if (lbItemNameAutoComplete.Items.Count > 0)
                        {
                            lbItemNameAutoComplete.Visible = true;
                            lbItemNameAutoComplete.SelectedIndex = 0;
                            lbItemNameAutoComplete.BringToFront();
                        }
                        else
                        {
                            lbItemNameAutoComplete.Visible = false;
                        }
                    }
                    else
                    {
                        lbItemNameAutoComplete.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        private void AddAutoSuggestItemName(TextBox sender)
        {
            try
            {
                string senderTextBox = sender.Text;

                int lastIndex = senderTextBox.LastIndexOf(' ');
                if (lastIndex < 0) lastIndex = 0;
                string textBeforeAppend = "";
                if (lastIndex != 0)
                    textBeforeAppend = senderTextBox.Substring(0, lastIndex + 1);
                lbItemNameAutoComplete.Visible = false;
                sender.Text = textBeforeAppend + lbItemNameAutoComplete.SelectedItem.ToString();
                sender.Focus();
                sender.Select(sender.Text.Length, 0);
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        private void listBox1ItemName_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    //add item into text...
                    AddAutoSuggestItemName(txtProductName);
                }
                else if (e.KeyCode == Keys.Up)
                {
                    if (lbItemNameAutoComplete.SelectedIndex == 0)
                    {

                        txtProductName.Focus();
                        txtProductName.Select(txtProductName.Text.Length, 0);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);                
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                //check name
                if (txtProductName.Text == "")                
                {
                    
                    txtProductName.Select();
                    return;
                }

                //check price
                decimal price = 0.0M;
                if (!decimal.TryParse(txtRetailPrice.Text.TrimEnd().TrimStart(), out price))
                {
                    
                    MessageBox.Show("ENTER PRICE");
                    txtRetailPrice.Text = "";
                    txtRetailPrice.Select();
                    return;
                }

                //check price
                decimal costprice = 0.0M;

                if (!decimal.TryParse(txtCostPrice.Text.TrimEnd().TrimStart(), out costprice))
                {
                    
                    MessageBox.Show("ENTER COST");
                    txtCostPrice.Text = "";
                    txtCostPrice.Select();
                    return;
                }

                //check category - create a new category if exist

                if (txtCategory.Text == "")
                {
                    
                    txtCategory.Select();
                    MessageBox.Show("ENTER CATEGORY");
                    return;
                }

                Query qr = Category.CreateQuery();
                if (qr.WHERE(Category.Columns.CategoryName, txtCategory.Text).GetCount(Category.Columns.CategoryName) == 0)
                {
                    //save the new category
                    Category cat = new Category();
                    cat.IsNew = true;
                    cat.CategoryName = txtCategory.Text;
                    cat.ItemDepartmentId = "MINIMART";
                    cat.Deleted = false;
                    cat.IsForSale = true;
                    cat.Save("");
                }

                //create the new product
                Item it = new Item();
                it.IsNew = true;
                it.ItemNo = ItemController.getNewItemRefNo();
                it.ItemName = txtProductName.Text;
                it.CategoryName = txtCategory.Text;
                it.Barcode = txtBarcode.Text;
                it.RetailPrice = price;
                it.IsInInventory = true;
                it.Deleted = false;
                it.Save();
                
                CreateNew();
                return;
                
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("ERROR! " + ex.Message);
                return;
            }
        }

        private void CreateNew()
        {
            try
            {
                txtProductNo.Text = ItemController.getNewItemRefNo();
                txtBarcode.Text = "";
                txtProductName.Text = "";
                txtRetailPrice.Text = "0";
                txtCostPrice.Text = "0";
                txtCategory.Select();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        private void frmProductDataEntryFormStyle_Load(object sender, EventArgs e)
        {
            frmPin f = new frmPin();
            f.ShowDialog();
            f.Dispose();

            CreateNew();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("ARE YOU SURE YOU WANT TO CLEAR?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dr == DialogResult.Yes)
            {
                CreateNew();
            }
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            frmItemMst f = new frmItemMst();
            f.ShowDialog();
            f.Dispose();
        }

        private void btnSave_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSave_Click(this, new EventArgs());                
            }
        }

        private void btnSave_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void btnSave_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
    }
}
