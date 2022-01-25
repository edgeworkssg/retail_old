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
    public partial class frmMultipleQtyEntry : Form
    {
        public ArrayList itemNos;
        public ArrayList itemNames;
        public ArrayList descriptions;

        public frmMultipleQtyEntry()
        {
            InitializeComponent();
        }
        public Hashtable ht;
        public bool IsSuccessful;
        private void frmMultipleQtyEntry_Load(object sender, EventArgs e)
        {
            try
            {
                IsSuccessful = false;
                if (itemNos == null || itemNames == null || descriptions == null || itemNos.Count == 0)
                    this.Close();

                for (int i = 0; i < itemNos.Count; i++)
                {
                    Label tmp;
                    tmp = new Label();
                    tmp.AutoSize = true;
                    if (i % 2 == 0)
                    {
                        tmp.BackColor = Color.Gainsboro;
                    }
                    tmp.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                            | System.Windows.Forms.AnchorStyles.Left)
                            | System.Windows.Forms.AnchorStyles.Right)));
                    tmp.Text = itemNos[i].ToString();
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
                    if (itemNames[i] != null) tmp.Text = itemNames[i].ToString();
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

                    Item it = new Item(itemNos[i].ToString());
                    Label tmpUOM;
                    tmpUOM = new Label();
                    tmpUOM.AutoSize = true;
                    if (i % 2 == 0)
                    {
                        tmpUOM.BackColor = Color.Gainsboro;
                    }
                    tmpUOM.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                            | System.Windows.Forms.AnchorStyles.Left)
                            | System.Windows.Forms.AnchorStyles.Right)));
                    tmpUOM.Text = String.IsNullOrEmpty(it.Userfld1) ? "" : it.Userfld1;
                    tblQuantity.Controls.Add(tmpUOM, 4, i);
                }
                tblQuantity.RowCount = itemNos.Count;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("Error: " + ex.Message);
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
                //populate hashtable
                ht = new Hashtable();
                TextBox txt; Label tmp;
                //close form and allow hash table to be read by parent form.....
                for (int i = 0; i < itemNos.Count; i++)
                {
                    tmp = (Label)tblQuantity.GetControlFromPosition(0, i);
                    txt = (TextBox)tblQuantity.GetControlFromPosition(3, i);
                    decimal tmpInt;
                    if (decimal.TryParse(txt.Text, out tmpInt))
                    {
                        ht.Add(tmp.Text, tmpInt);
                    }
                    else
                    {
                        MessageBox.Show("Invalid quantity for row " + i.ToString());
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
