using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using POSDevices;
using WinPowerPOS.OrderForms;

namespace WinPowerPOS
{
    public partial class frmTouchScreenXML : Form
    {
        TouchScreenHotKeysController MasterControl;
        public POSController pos;
        private DateTime startTime;
        //public bool IsEditMode = false;
        private string PriceMode;
        public frmTouchScreenXML(POSController inPos, string priceMode)
        {
            InitializeComponent();
            Tabel.AutoGenerateColumns = false; startTime = DateTime.Now;
            pos = inPos;
            this.PriceMode = priceMode;
        }


        private void frmTouchScreenXML_Load(object sender, EventArgs e)
        {
            //pos = new POSController();

            for (int i = 0; i < tableLayoutPanel1.ColumnCount; i++)
                for (int j = 0; j < tableLayoutPanel1.RowCount; j++)
                    ((Button)tableLayoutPanel1.GetControlFromPosition(i, j)).Click += new EventHandler(Button_Click);

            MasterControl = new TouchScreenHotKeysController();
            MasterControl.populateItemDepartmentDisplayPanel(tableLayoutPanel1);
            pnlOrder.Show();
            BindGrid();
            
        }

        private void Button_Click(object sender, EventArgs e)
        {
            string status;
            if (!(sender is Button )) return;
            string Value = ((Button)sender).Tag.ToString();
            
                if (Value == null || Value == "") return;

                //department
                if (MasterControl.Stck.Count == 0)
                {
                    MasterControl.populateCategoryDisplayPanel(tableLayoutPanel1, Value);
                    MasterControl.Stck.Push(Value);
                }
                //category
                else if (MasterControl.Stck.Count == 1)
                {
                    MasterControl.populateItemDisplayPanel(tableLayoutPanel1, Value);
                    MasterControl.Stck.Push(Value);
                }
                //item
                else
                {
                    if (!pos.IsRestricted(new Item(((Button)sender).Tag.ToString()), out status))
                    {
                        frmErrorMessageWithTextArea frmError = new frmErrorMessageWithTextArea();
                        frmError.lblMessage = status;
                        frmError.ShowDialog();
                    }
                    else
                    {
                        //add item to orderdet collection
                        if (!pos.AddItemToOrderWithPriceMode(new Item(((Button)sender).Tag.ToString()), 1, 0.00M, true, PriceMode, out status))
                        {
                            MessageBox.Show("Error: " + status);
                        }
                        else
                        {
                            //find item index in orderDet collection
                            Item myItem = new Item(((Button)sender).Tag.ToString());
                            int index = 0;
                            for (int i = 0; i < pos.myOrderDet.Count; i++)
                            {
                                if (pos.myOrderDet[i].ItemNo == myItem.ItemNo)
                                {
                                    index = i;
                                    break;
                                }

                            }

                            //OrderDet myDet = pos.myOrderDet[pos.myOrderDet.Count - 1];
                            OrderDet myDet = pos.myOrderDet[index];
                            PriceDisplay myDisplay = new PriceDisplay();
                            int poleDisplayWidth = 0;
                            if (!int.TryParse(AppSetting.GetSetting(AppSetting.SettingsName.PoleDisplay.PoleDisplayLinesLength), out poleDisplayWidth))
                            {
                                poleDisplayWidth = 20;
                            }
                            pos.CalculateLineAmount(ref myDet);
                            pos.CalculateTotalAmount(out status);
                            myDisplay.ClearScreen();
                            myDisplay.ShowItemPrice(
                                myDet.Item.ItemName,
                                (double)myDet.Item.RetailPrice,
                                (double)pos.CalculateTotalAmount(out status), poleDisplayWidth);
                        }
                    }
                }
                BindGrid();
            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (!MasterControl.GoBack(tableLayoutPanel1))
            {
                MasterControl = new TouchScreenHotKeysController();
                this.Close();
            }

            lblNavigation_Order.Text = MasterControl.GetNavigation();
            
        }

        private void Tabel_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
                string status;
                if (Tabel.Columns[e.ColumnIndex].Name == Quantity.Name)
                {
                    OrderForms.frmKeypad t = new WinPowerPOS.OrderForms.frmKeypad();
                    t.IsInteger = true;
                    t.initialValue = Tabel[e.ColumnIndex, e.RowIndex].Value.ToString();
                    t.textMessage = "Set Qty:" + Tabel.Rows[e.RowIndex].Cells["ItemName"].Value.ToString();
                    t.ShowDialog();
                    int newQty = 0;
                    if (t.value != "" && int.TryParse(t.value, out newQty))
                    {

                        string LineID = Tabel.Rows[e.RowIndex].Cells["ID"].Value.ToString();
                        if (!pos.ChangeOrderLineQuantity(LineID, newQty, false, out status))
                        {
                            MessageBox.Show("Error:" + status);
                        }
                        BindGrid();
                        //MasterControl.ChangeQuantity(Tabel.Rows[e.RowIndex].Cells["ItemNo"].Value.ToString(), int.Parse(t.value));
                        //Tabel.DataSource = MasterControl.GetAllData();
                    }
                }
                else if (Tabel.Columns[e.ColumnIndex].Name == Price.Name)
                {
                    string selectedItemNo = Tabel.Rows[e.RowIndex].Cells["ItemNo"].Value.ToString();
                    Item atp = new Item(selectedItemNo);
                    if (atp.IsLoaded && atp.IsServiceItem.HasValue && atp.IsServiceItem.Value)
                    {
                        OrderForms.frmKeypad t = new WinPowerPOS.OrderForms.frmKeypad();
                        t.IsInteger = false;
                        t.initialValue = Tabel[e.ColumnIndex, e.RowIndex].Value.ToString();
                        t.textMessage = "Set Price:" + Tabel.Rows[e.RowIndex].Cells["ItemName"].Value.ToString();
                        t.ShowDialog();
                        decimal newPrice = 0;
                        if (t.value != "" && decimal.TryParse(t.value, out newPrice))
                        {
                            string LineID = Tabel.Rows[e.RowIndex].Cells["ID"].Value.ToString();
                            if (!pos.ChangeOrderLineUnitPrice(LineID, newPrice, out status))
                            {
                                MessageBox.Show("Error:" + status);
                            }
                            BindGrid();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void BindGrid()
        {
            try
            {
                string status;
                Tabel.DataSource = pos.FetchUnSavedOrderItems(out status);
                Tabel.Refresh();

                lblNavigation_Order.Text = MasterControl.GetNavigation();
            }
            catch (Exception eX)
            {
                Logger.writeLog(eX);
                MessageBox.Show(eX.Message);
            }        
        }

        private void btnDeleteLine_Click(object sender, EventArgs e)
        {
            try
            {
                string status;
                if (Tabel.SelectedCells.Count > 0)
                {
                    string LineID = Tabel.Rows[Tabel.SelectedCells[0].RowIndex].Cells["ID"].Value.ToString();
                    bool isvoided = pos.IsVoided(LineID, out status);
                    if (status != "")
                    {
                        MessageBox.Show(status);
                        return;
                    }
                    if (!pos.SetVoidOrderLine(LineID, !isvoided, out status))
                    {
                        MessageBox.Show("Error: " + status);
                    }
                    BindGrid();
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show(ex.Message);
            }
        }

        private void Tabel_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            string status;
            try
            {
                if (pos.IsVoided(Tabel.Rows[e.RowIndex].Cells["ID"].Value.ToString(), out status))
                {
                    Font f = Tabel.DefaultCellStyle.Font;
                    for (int i = 0; i < Tabel.ColumnCount; i++)
                    {
                        if ((Tabel.Columns[i].Visible))
                        {
                            Tabel.Rows[e.RowIndex].Cells[i].Style.ForeColor = System.Drawing.Color.White;
                            Tabel.Rows[e.RowIndex].Cells[i].Style.BackColor = System.Drawing.Color.DarkRed;
                            Tabel.Rows[e.RowIndex].Cells[i].Style.Font = new Font(f, FontStyle.Strikeout);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show(ex.Message);
            }
               
        }
        
    }
}
