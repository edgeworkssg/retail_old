using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PowerPOS;

namespace WinPowerPOS.OrderForms
{
    public partial class frmRemoveItem : Form
    {
        public bool IsSuccessful;
        public POSController pos;
        public decimal removedQty;

        public frmRemoveItem()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            IsSuccessful = false;

            this.Close();
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            if (decimal.TryParse(txtQuantity.Text, out removedQty))
            {
                if (removedQty > qty)
                {
                    MessageBox.Show("Quantity entered more than total quantity in the order(" + qty.ToString() + "). Please change");
                    return;
                }
                
                if (removedQty == qty)
                {
                    string[] tmpOrderDetList = OrderDetID.Split(',');

                    for (int i = 0; i < tmpOrderDetList.GetLength(0); i++)
                    {
                        OrderDet od = (OrderDet)pos.myOrderDet.Find(tmpOrderDetList[i]);
                        if (od != null && od.OrderDetID == tmpOrderDetList[i])
                        {
                            pos.myOrderDet.Remove(od);
                        }
                    }
                }
                else
                {
                    
                    string[] tmpOrderDetList = OrderDetID.Split(',');
                    for (int i = 0; i < tmpOrderDetList.GetLength(0); i++)
                    {
                        OrderDet od = (OrderDet)pos.myOrderDet.Find(tmpOrderDetList[i]);
                        if (od != null && od.OrderDetID == tmpOrderDetList[i])
                        {
                            if (od.Quantity <= removedQty)
                            {
                                pos.myOrderDet.Remove(od);
                                removedQty = removedQty - od.Quantity.GetValueOrDefault(0);
                            }
                            else
                            {
                                string status = "";
                                decimal newQty = od.Quantity.GetValueOrDefault(0) - removedQty;
                                pos.ChangeOrderLineQuantity(od.OrderDetID, newQty, false, out status);
                                removedQty = 0;
                            }
                        }
                    }
                }
                pos.UndoPromo();
                pos.ApplyPromo();
                IsSuccessful = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Please enter correct quantity");


            }
        }

        string OrderDetID = "";
        decimal qty = 0;

        private void txtDisc_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                //check barcode
                string itemNo;
                if (ItemController.IsInventoryItemBarcode(txtDisc.Text, out itemNo))
                {
                    Item it = new Item(itemNo);
                    bool isFound = false;
                    OrderDetID = "";
                    qty = 0;
                    foreach (OrderDet od in pos.myOrderDet)
                    {
                        
                        if (od.ItemNo == it.ItemNo && !od.IsVoided)
                        {
                            isFound = true;
                            OrderDetID += OrderDetID == "" ? od.OrderDetID : "," + od.OrderDetID;
                            qty += od.Quantity.GetValueOrDefault(0);
                        }
                    }

                    if (!isFound)
                    {
                        MessageBox.Show("Item not found in the order list");
                    }
                    else
                    {
                        lblItemName.Text = it.ItemName;
                        txtQuantity.Text = "1";
                        pnlItemQuantity.Visible = true;
                        btnSet.Visible = true;
                        txtQuantity.Focus();
                    }
                }
                else
                {
                    MessageBox.Show("Item Not Found");
                }


            }
        }

        private void frmRemoveItem_Load(object sender, EventArgs e)
        {
            pnlItemQuantity.Visible = false;
            btnSet.Visible = false;
            txtDisc.Focus();
        }

        private void txtQuantity_Enter(object sender, EventArgs e)
        {
            frmKeypad fKeypad = new frmKeypad();
            fKeypad.initialValue = txtQuantity.Text;
            fKeypad.textMessage = "Please Insert Quantity";
            fKeypad.ShowDialog();
            if (fKeypad.value != null && fKeypad.value != "")
            {
                decimal tmp = 0;
                if (decimal.TryParse(fKeypad.value, out tmp))
                {
                    txtQuantity.Text = fKeypad.value;
                }
            }
        }
    }
}