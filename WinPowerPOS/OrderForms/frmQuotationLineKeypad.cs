using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using PowerPOS.Container;
using WinPowerPOS.LoginForms;

namespace WinPowerPOS.OrderForms
{
    public partial class frmQuotationLineKeypad : Form
    {
        public enum EditedField
        {
            Quantity = 1,
            RetailPrice = 2,
            DiscountedPrice = 3
        }
        
        QuotationDet myDet;
        public bool ApplyPromo;
        public EditedField editField;
        public string value;
        public string initialValue;
        public string textMessage;
        public bool IsInteger;
        public QuoteController pos;
        public string LineID;
        public bool AllowChangeUnitPrice;
        public string Discount1;
        public string Discount2;

        private bool EnableSecondDiscount = false;

        public frmQuotationLineKeypad()
        {
            InitializeComponent();
            IsInteger = false;
            AllowChangeUnitPrice = false;
            EnableSecondDiscount = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Discount.EnableSecondDiscount), false);
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            txtEntry.Select();
            SendKeys.Send(((Button)sender).Text);
        }
        
        private void btnDot_Click(object sender, EventArgs e)
        {
            if (!txtEntry.Text.Contains(".") & !IsInteger)
            {
                txtEntry.Focus();
                SendKeys.Send(((Button)sender).Text);
            }
        }

        private void btnCLEAR_Click(object sender, EventArgs e)
        {
            txtEntry.Text = "0";
        }

        private void btnBackSpace_Click(object sender, EventArgs e)
        {
            //txtEntry.Text = txtEntry.Text.Remove(txtEntry.Text.Length - 1);
            txtEntry.Select();
            SendKeys.Send("{BACKSPACE}");
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            value = "";
            this.Close();
        }

        private void btnPlus_Click(object sender, EventArgs e)
        {
            decimal val;
            if (txtEntry.Text == "")
                return;
            val = decimal.Parse(txtEntry.Text);
            val = val + 1;            
            txtEntry.Text = val.ToString();
            txtEntry.Select();
        }

        private void btnMin_Click(object sender, EventArgs e)
        {
            if (txtEntry.Text == "")
                return;
            decimal val;            
                val = decimal.Parse(txtEntry.Text);

                    val = val - 1;
               
                txtEntry.Text = val.ToString();
                txtEntry.Select();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (ApplyLineChanges())
            {
                this.Close();
            }
        }
        private bool AllowGiveDiscount;
        private void frmKeypad_Load(object sender, EventArgs e)
        {
            string status;
            if (!AllowChangeUnitPrice)
            {
                rbPrice.Enabled = false;
                txtRetailPrice.Enabled = false;                
            }

            pnlAdditionalDiscount.Visible = EnableSecondDiscount;

            if (!PrivilegesController.HasPrivilege("Change Unit Price", UserInfo.privileges))
            {
                rbPrice.Enabled = false;
                txtRetailPrice.Enabled = false;                
            }
            myDet = pos.GetLine(LineID, out status);
            if (!PrivilegesController.HasPrivilege("Give Discount", UserInfo.privileges) || myDet.Item.IsNonDiscountable)
            {
                /*
                rbDiscounted.Enabled = false;
                txtDiscounted.Enabled = false;
                //rbQuantity.Checked = true;
                btn10.Enabled = false;
                btn20.Enabled = false;
                btn30.Enabled = false;
                btnMisc.Enabled = false;
                */

                txtDiscounted.Enabled = false;
                rbDiscounted.Enabled = false;
                editField = EditedField.Quantity;
                AllowGiveDiscount = false;
            }
            else
            {
                AllowGiveDiscount = true;
            }

            txtEntry.Text = initialValue;
            lblMessage.Text = textMessage;
            
            
            txtItemNo.Text = myDet.ItemNo.ToString();
            txtItemName.Text = myDet.Item.ItemName;
            txtCategory.Text = myDet.Item.Category.CategoryName;
            txtQty.Text = myDet.Quantity.ToString();
            txtRetailPrice.Text = myDet.UnitPrice.ToString("N2");

            if (myDet.Item.IsServiceItem.HasValue && myDet.Item.IsServiceItem.Value)
            {
                rbPrice.Enabled = true;
                txtRetailPrice.Enabled = true;
            }

            if (myDet.IsPreOrder.HasValue)
                cbOrder.Checked = myDet.IsPreOrder.Value;

            if (!myDet.IsPromo)
            {
                decimal discountedPrice = myDet.UnitPrice - ((myDet.UnitPrice * myDet.Discount) / 100);
                txtDiscounted.Text = CommonUILib.RemoveRoundUp(discountedPrice).ToString("N2");
                if (string.IsNullOrEmpty(myDet.DiscountDetail))
                    txtDiscountDetail.Text = "0%";
                else
                    txtDiscountDetail.Text = myDet.DiscountDetail;

                if (txtDiscountDetail.Text.IndexOf("+") >= 0)
                {
                    string discountDetail = txtDiscountDetail.Text;
                    Discount1 = discountDetail.Remove(discountDetail.IndexOf("+"));
                    Discount2 = discountDetail.Remove(0, discountDetail.IndexOf("+") + 1);
                }
                else
                {
                    Discount1 = txtDiscountDetail.Text;
                    Discount2 = "";
                }

                //txtDiscounted.Text = CommonUILib.RemoveRoundUp(myDet.UnitPrice - ((myDet.UnitPrice * myDet.Discount) / 100)).ToString("N2");
            }
            else
            {
                //if promo disable certain buttons and only allow qty change.
                txtDiscounted.Text = CommonUILib.RemoveRoundUp(myDet.UnitPrice * (decimal)myDet.PromoDiscount / 100).ToString("N2");
                btn10.Visible = false;
                btnMisc.Visible = false;
                btn20.Visible = false;
                btn30.Visible = false;
                txtDiscounted.ReadOnly = true;
                txtQty.Select();
                rbDiscounted.Visible = false;
                rbPrice.Visible = false;              
               
            }
            if (editField == EditedField.DiscountedPrice )
            {
                rbDiscounted.Checked = true;
                txtDiscounted.Select();
            }
            else if (editField == EditedField.RetailPrice )
            {
                if (txtRetailPrice.Enabled)
                {
                    rbPrice.Checked = true;
                    txtRetailPrice.Select();
                }
                else
                {
                    rbQuantity.Checked = true;
                    txtQty.Select();
                }
            }
            else
            {
                rbQuantity.Checked = true;
                txtQty.Select();
            }

            LineInfoCollection lineInfos = new LineInfoCollection();
            lineInfos.Where(LineInfo.Columns.Deleted, false);
            lineInfos.Load();
            lineInfos.Add(new LineInfo { LineInfoName = "" });
            lineInfos.Sort(LineInfo.Columns.LineInfoName, true);

            cmbLineInfo.DataSource = lineInfos;
            cmbLineInfo.DisplayMember = LineInfo.Columns.LineInfoName;
            cmbLineInfo.ValueMember = LineInfo.Columns.LineInfoName;
            cmbLineInfo.Refresh();

            if (!string.IsNullOrEmpty(myDet.LineInfo))
            {
                cmbLineInfo.SelectedValue = myDet.LineInfo;
            }

            txtLineRemark.Text = myDet.Remark;
        }

        //handle line ordering
        private bool ApplyLineChanges()
        {
            if (myDet != null)
            {
                //update qty
                int qty;
                #region *) Validation: Quantity is INT
                if (!int.TryParse(txtQty.Text, out qty))
                {
                    MessageBox.Show("Invalid Qty!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                #endregion

                //update retail price
                decimal RetailPrice;
                #region *) Validation: Retail Price is DECIMAL
                if (!decimal.TryParse(txtRetailPrice.Text, out RetailPrice))
                {
                    MessageBox.Show("Invalid Retail Price!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                #endregion

                decimal DiscountedPrice;
                #region *) Validation: Discounted Price is DECIMAL && is lower/equal than retail price
                if (!decimal.TryParse(txtDiscounted.Text, out DiscountedPrice) || DiscountedPrice > RetailPrice)
                {
                    MessageBox.Show("Invalid Discounted Price!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                #endregion

                Calculate_Discount1(RetailPrice, DiscountedPrice);

                myDet.DiscountDetail = (Discount1 + "+" + Discount2).TrimEnd('+');
                txtDiscountDetail.Text = myDet.DiscountDetail;
                
                //decimal DiscPercent = ((RetailPrice - DiscountedPrice) / (RetailPrice + 0.00001M)) * 100;
                decimal DiscPercent = 0;
                if (RetailPrice != 0)
                    DiscPercent = ((RetailPrice - DiscountedPrice) / RetailPrice) * 100;

                if (!myDet.IsPromo && myDet.Discount != pos.GetPreferredDiscount())
                {
                    #region *) Validation: Discount given is not exceed the Discount Limit
                    UserMst CurrUser = new UserMst(PowerPOS.Container.UserInfo.username);
                    if (!CurrUser.IsAbleToGiveDiscount(DiscPercent))
                    {
                        MessageBox.Show("Discount exceed the discount limit set", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                    #endregion
                }

                myDet.Quantity = qty;
                myDet.UnitPrice = RetailPrice;

                //update discount - IF NON PROMO
                if (!myDet.IsPromo)
                {
                    myDet.Discount = DiscPercent;
                    if (myDet.Discount != pos.GetPreferredDiscount())
                    {
                        myDet.IsSpecial = true;
                    }
                }
                else
                {
                    myDet.Discount = 0;
                }
                myDet.IsPreOrder = cbOrder.Checked;
                pos.CalculateLineAmount(ref myDet);

                myDet.LineInfo = cmbLineInfo.SelectedValue.ToString();
                myDet.Remark = txtLineRemark.Text;

                return true;
            }
            else
            {
                return false;
            }
        }

        private void rbQuantity_CheckedChanged(object sender, EventArgs e)
        {
            if (myDet == null) return;

            if (myDet.IsPromo || myDet.IsFreeOfCharge)
            {
                rbQuantity.Checked = true;
            }

            if (rbPrice.Checked)
            {
                IsInteger = false;
                txtEntry.Text = txtRetailPrice.Text;
                editField = EditedField.RetailPrice;
            }
            else if (rbQuantity.Checked)
            {
                IsInteger = true;
                txtEntry.Text = txtQty.Text;
                editField = EditedField.Quantity;
            }
            else if (rbDiscounted.Checked)
            {
                IsInteger = false;
                txtEntry.Text = txtDiscounted.Text;
                editField = EditedField.DiscountedPrice;
            }

        }

        private void txtEntry_TextChanged(object sender, EventArgs e)
        {
            if (txtEntry.Text == "")
                txtEntry.Text = "0";

            if (rbPrice.Checked)
            {
                decimal val = 0;
                decimal.TryParse(txtEntry.Text, out val);
                txtRetailPrice.Text = val.ToString("N2");
            }
            else if (rbQuantity.Checked)
            {
                IsInteger = true;
                txtQty.Text = txtEntry.Text;
            }
            else if (rbDiscounted.Checked)
            {
                IsInteger = false;
                decimal val = 0;
                decimal.TryParse(txtEntry.Text, out val);
                txtDiscounted.Text = val.ToString("N2");
            }           
        }

        private void btn10_Click(object sender, EventArgs e)
        {
            decimal discount = decimal.Parse(((Button)sender).Tag.ToString());

            #region *) Assert: Discount given is not exceed the Discount Limit
            UserMst CurrUser = new UserMst(PowerPOS.Container.UserInfo.username);
            if (!CurrUser.IsAbleToGiveDiscount(discount))
            {
                MessageBox.Show("Discount exceed the discount limit set", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            #endregion

            if (!AllowGiveDiscount)
            {
                frmSupervisorLogin f = new frmSupervisorLogin();
                f.privilegeName = PrivilegesController.GIVE_DISCOUNT;
                f.ShowDialog();
                if (f.IsAuthorized)
                {
                    txtDiscounted.Enabled = true;
                    rbDiscounted.Enabled = true;
                    AllowGiveDiscount = true;
                }
                else
                {
                    return;
                }
            }

            //myDet.Discount = discount;
            decimal unitPrice;
            if (!decimal.TryParse(txtRetailPrice.Text, out unitPrice))
            {
                unitPrice = myDet.UnitPrice;
            }
            decimal discountedPrice = unitPrice - ((unitPrice * discount) / 100);
            discountedPrice = AddDiscount2(discountedPrice);
            txtDiscounted.Text = discountedPrice.ToString("N2");
            Discount1 = discount.ToString("N0") + "%";
            txtDiscountDetail.Text = (Discount1 + "+" + Discount2).TrimEnd('+');
        }

        private void txtRetailPrice_TextChanged(object sender, EventArgs e)
        {
            decimal retailPrice;
            if (decimal.TryParse(txtRetailPrice.Text, out retailPrice))
            {
                if (!myDet.IsSpecial)
                {
                    txtDiscounted.Text = (retailPrice - retailPrice * pos.GetPreferredDiscount() / 100).ToString("N2");
                }
                else
                {
                    txtDiscounted.Text = (retailPrice - retailPrice * myDet.Discount / 100).ToString("N2");
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            myDet.IsVoided = true;
            this.Close();
        }

        private void btnNoSpecial_Click(object sender, EventArgs e)
        {
            myDet.IsSpecial = false;
            decimal discount = pos.GetPreferredDiscount();
            txtRetailPrice.Text = myDet.UnitPrice.ToString("N2");
            txtDiscounted.Text = (myDet.UnitPrice - myDet.UnitPrice * discount / 100).ToString("N2");
            Discount2 = "";
            Discount1 = discount.ToString("N0") + "%";
            txtDiscountDetail.Text = (Discount1 + "+" + Discount2).TrimEnd('+');
        }

        private void txtRetailPrice_Enter(object sender, EventArgs e)
        {
            rbPrice.Checked = true;
        }

        private void txtDiscounted_Enter(object sender, EventArgs e)
        {
            rbDiscounted.Checked = true;
        }

        private void txtQty_Enter(object sender, EventArgs e)
        {
            rbQuantity.Checked = true;
        }

        private void btnMisc_Click(object sender, EventArgs e)
        {            
            decimal discount;
            frmKeypad f = new frmKeypad();
            //f.initialValue = "";
            f.IsInteger = false;
            f.ShowDialog();
            if (f.value != "" && decimal.TryParse(f.value, out discount) && discount <= 100)
            {
                #region *) Assert: Discount given is not exceed the Discount Limit
                UserMst CurrUser = new UserMst(PowerPOS.Container.UserInfo.username);
                if (!CurrUser.IsAbleToGiveDiscount(discount))
                {
                    MessageBox.Show("Discount exceed the discount limit set", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                #endregion

                //myDet.Discount = discount;
                decimal unitPrice;
                if (!decimal.TryParse(txtRetailPrice.Text, out unitPrice))
                {
                    unitPrice = myDet.UnitPrice;
                }
                decimal discountedPrice = unitPrice - ((unitPrice * discount) / 100);
                discountedPrice = AddDiscount2(discountedPrice);
                txtDiscounted.Text = discountedPrice.ToString("N2");
                Discount1 = discount.ToString("N0") + "%";
                txtDiscountDetail.Text = (Discount1 + "+" + Discount2).TrimEnd('+');
                //txtDiscounted.Text = (unitPrice - ((unitPrice * discount) / 100)).ToString("N4");
            }
        }

        private void btnAddDisc_Click(object sender, EventArgs e)
        {
            string tag = ((Button)sender).Tag.ToString();
            frmKeypad f = new frmKeypad();
            f.IsInteger = false;

            if (!string.IsNullOrEmpty(Discount2) && Discount2.Contains(tag))
            {
                f.initialValue = Discount2.Replace(tag, "");
            }

            f.ShowDialog();
            decimal discount;
            if (!string.IsNullOrEmpty(f.value) && decimal.TryParse(f.value.ToString(), out discount))
            {
                string prevDiscount2 = Discount2;
                decimal discountedPrice;
                decimal.TryParse(txtDiscounted.Text, out discountedPrice);

                // Get discounted price before discount2
                discountedPrice = RemoveDiscount2(discountedPrice);

                if (discount <= 0)
                {
                    Discount2 = "";
                }
                else
                {
                    if (tag == "%")
                    {
                        if (discount <= 100)
                            Discount2 = discount.ToString() + tag;
                        else
                            MessageBox.Show("Discount cannot exceed 100%");
                    }
                    else
                    {
                        Discount2 = tag + discount.ToString();
                    }
                }

                // Get discounted price after discount2
                decimal TotalDiscountedPrice = AddDiscount2(discountedPrice);
                if (Math.Round(TotalDiscountedPrice, 2) < 0)
                {
                    MessageBox.Show(string.Format("Discount amount cannot exceed {0}.", discountedPrice.ToString("N2")));
                    Discount2 = prevDiscount2;
                    return;
                }

                txtDiscounted.Text = CommonUILib.RemoveRoundUp(TotalDiscountedPrice).ToString("N2");
                txtDiscountDetail.Text = (Discount1 + "+" + Discount2).TrimEnd('+');
            }
        }

        private decimal AddDiscount2(decimal price)
        {
            if (!string.IsNullOrEmpty(Discount2))
            {
                if (Discount2.StartsWith("$"))
                {
                    decimal dollar;
                    decimal.TryParse(Discount2.TrimStart('$'), out dollar);
                    price -= dollar;
                    return price;
                }
                else if (Discount2.EndsWith("%"))
                {
                    decimal percent;
                    decimal.TryParse(Discount2.TrimEnd('%'), out percent);
                    price *= (1 - percent / 100);
                    return price;
                }
                return price;
            }
            return price;
        }

        private decimal RemoveDiscount2(decimal price)
        {
            if (!string.IsNullOrEmpty(Discount2))
            {
                if (Discount2.StartsWith("$"))
                {
                    decimal dollar;
                    decimal.TryParse(Discount2.TrimStart('$'), out dollar);
                    price += dollar;
                    return price;
                }
                else if (Discount2.EndsWith("%"))
                {
                    decimal percent;
                    decimal.TryParse(Discount2.TrimEnd('%'), out percent);
                    if (percent == 100)
                        price = myDet.UnitPrice;
                    else
                        price /= (1 - percent / 100);
                    return price;
                }
                return price;
            }
            return price;
        }

        private void txtDiscounted_Leave(object sender, EventArgs e)
        {
            decimal RetailPrice;
            decimal.TryParse(txtRetailPrice.Text, out RetailPrice);
            decimal DiscountedPrice;
            decimal.TryParse(txtDiscounted.Text, out DiscountedPrice);
            Calculate_Discount1(RetailPrice, DiscountedPrice);
            txtDiscountDetail.Text = (Discount1 + "+" + Discount2).TrimEnd('+');
        }

        private void Calculate_Discount1(decimal RetailPrice, decimal TotalDiscountedPrice)
        {
            decimal discPercent1 = 0;
            if (!string.IsNullOrEmpty(Discount2))
            {
                if (Discount2.StartsWith("$"))
                {
                    decimal dollar;
                    decimal.TryParse(Discount2.TrimStart('$'), out dollar);
                    discPercent1 = (1 - ((TotalDiscountedPrice + dollar) / RetailPrice)) * 100;
                }
                else if (Discount2.EndsWith("%"))
                {
                    decimal percent;
                    decimal.TryParse(Discount2.TrimEnd('%'), out percent);
                    discPercent1 = (1 - (TotalDiscountedPrice / ((1 - percent / 100) * RetailPrice))) * 100;
                }
            }
            else
            {
                discPercent1 = (1 - TotalDiscountedPrice / RetailPrice) * 100;
            }
            Discount1 = discPercent1.ToString("N0") + "%";
        }
    }
}