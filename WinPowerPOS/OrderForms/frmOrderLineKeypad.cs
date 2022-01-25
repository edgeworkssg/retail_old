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
using WeighingMachine;
using System.Threading;
using SubSonic;

namespace WinPowerPOS.OrderForms
{
    public partial class frmOrderLineKeypad : Form
    {
        public enum EditedField
        {
            Quantity = 1,
            RetailPrice = 2,
            DiscountedPrice = 3,
            NoOfTimes = 4,
            ContainerWeight = 5
        }

        OrderDet myDet;
        public bool ApplyPromo;
        public EditedField editField;
        public string value;
        public string initialValue;
        public string textMessage;
        public bool IsInteger;
        public POSController pos;
        public string LineID;
        public bool AllowChangeUnitPrice;
        public string Discount1;
        public string Discount2;
        public bool isCancel = false;
        public bool isUserTokenForDiscountScanned = false;

        private bool EnableSecondDiscount = false;
        private bool isError = false;
        private bool IsUseWeight = false;
        private bool IsUseWeighingMachine = false;
        private bool isUsingLowQuantityPrompt = false;
        private bool showPackingSize = false;
        private decimal OldRetailPrice = 0;
        private decimal currentDiscount = 0;

        private bool IsUseContainerWeight = false;

        public frmOrderLineKeypad()
        {
            InitializeComponent();
            IsInteger = false;
            AllowChangeUnitPrice = false;
            EnableSecondDiscount = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Discount.EnableSecondDiscount), false);
            IsUseWeight = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Invoice.UseWeight), false);
            IsUseWeighingMachine = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.WeighingMachine.UseWeighingMachine), false);
            isUsingLowQuantityPrompt = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.LowQuantityPromptInSalesScreen), false);
            showPackingSize = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.SalesInvoice.ShowUomInEditQtyForm), false);
            IsUseContainerWeight = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Invoice.UseContainerWeight), false);

            btnGetWeight.Visible = IsUseWeight && IsUseWeighingMachine; //add usecontainerweight?
            lblUOM.Visible = (IsUseWeight || showPackingSize);
            txtUOM.Visible = (IsUseWeight || showPackingSize);
            lblPackingSize.Visible = showPackingSize;
            cmbPackingSize.Visible = showPackingSize;

            //IsUserContainerWeight
            lblContainerWeight.Visible = IsUseContainerWeight;
            txtContainerWeight.Visible = IsUseContainerWeight;
            rbContainerWeight.Visible = IsUseContainerWeight;
            lblTotalWeight.Visible = IsUseContainerWeight;
            txtTotalWeight.Visible = IsUseContainerWeight;

            lblStock.Visible = isUsingLowQuantityPrompt;
            txtStockBalance.Visible = isUsingLowQuantityPrompt;
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            if (isError)
                btnCLEAR_Click(sender, e);
            txtEntry.Select();
            SendKeys.Send(((Button)sender).Text);
            isError = false;
        }

        private void btnDot_Click(object sender, EventArgs e)
        {
            if (!txtEntry.Text.Contains(".") & !IsInteger)
            {
                txtEntry.Focus();
                SendKeys.Send(((Button)sender).Text);
                isError = false;
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
            isCancel = true;
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

            txtEntry.Text = initialValue;
            lblMessage.Text = textMessage;

            myDet = pos.GetLine(LineID, out status);
            txtItemNo.Text = myDet.ItemNo.ToString();
            txtItemName.Text = myDet.Item.ItemName;
            txtCategory.Text = myDet.Item.Category.CategoryName;
            txtQty.Text = myDet.Quantity.ToString();
            txtRetailPrice.Text = myDet.UnitPrice.ToString("N");
            OldRetailPrice = myDet.UnitPrice;
            txtUOM.Text = myDet.Item.UOM;

            if (myDet.Item.IsOpenPricePackage && string.IsNullOrEmpty(myDet.PointItemNo))
            {
                rbPrice.Enabled = true;
                txtRetailPrice.Enabled = true;
                lblNumOfTimes.Visible = true;
                txtNumOfTimes.Visible = true;
                txtNumOfTimes.Text = myDet.PointGetAmount.ToString("N0");
                rbNumOfTimes.Visible = true;
            }

            if (myDet.Item.IsServiceItem.HasValue && myDet.Item.IsServiceItem.Value)
            {
                rbPrice.Enabled = true;
                txtRetailPrice.Enabled = true;
            }

            if (myDet.IsPreOrder.HasValue)
                cbOrder.Checked = myDet.IsPreOrder.Value;

            if (!myDet.Item.IsNonDiscountable)
            {

                if (!PrivilegesController.HasPrivilege("Give Discount", UserInfo.privileges) && !isUserTokenForDiscountScanned)
                {

                    txtDiscounted.Enabled = false;
                    rbDiscounted.Enabled = false;
                    //editField = EditedField.Quantity;
                    AllowGiveDiscount = false;
                }
                else
                {
                    if ((myDet.SpecialDiscount == "P1"
                                || myDet.SpecialDiscount == "P2"
                                || myDet.SpecialDiscount == "P3"
                                || myDet.SpecialDiscount == "P4"
                                || myDet.SpecialDiscount == "P5"))
                    {
                        txtDiscounted.Enabled = false;
                        rbDiscounted.Enabled = false;
                        editField = EditedField.Quantity;
                        AllowGiveDiscount = false;
                    }
                    else
                    {
                        AllowGiveDiscount = true;
                    }
                }

            }
            else
            {
                txtDiscounted.Enabled = false;
                rbDiscounted.Enabled = false;
                editField = EditedField.Quantity;
                AllowGiveDiscount = false;
            }


            if (!myDet.IsPromo)
            {
                decimal discountedPrice = myDet.UnitPrice - ((myDet.UnitPrice * myDet.Discount) / 100);
                txtDiscounted.Text = GetDiscountedPrice(discountedPrice);
                currentDiscount = decimal.Parse(GetDiscountedPrice(discountedPrice));
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
            }
            else
            {
                //if promo disable certain buttons and only allow qty change.
                txtDiscounted.Text = GetDiscountedPrice(myDet.UnitPrice - (myDet.UnitPrice * (decimal)myDet.PromoDiscount / 100));
                currentDiscount = decimal.Parse(GetDiscountedPrice(myDet.UnitPrice - (myDet.UnitPrice * (decimal)myDet.PromoDiscount / 100)));
                txtDiscountDetail.Text = ((decimal)myDet.PromoDiscount).ToString("N") + "%";
                if (myDet.Discount2Percent > 0)
                    txtDiscountDetail.Text = txtDiscountDetail.Text + "+" + myDet.Discount2Percent.ToString("N2") + "%";
                if (myDet.Discount2Dollar > 0)
                    txtDiscountDetail.Text = txtDiscountDetail.Text + "+ $" + myDet.Discount2Dollar.ToString("N");
                btn10.Visible = false;
                btnMisc.Visible = false;
                btn20.Visible = false;
                btn30.Visible = false;
                txtDiscounted.ReadOnly = true;
                txtQty.Select();
                rbDiscounted.Visible = false;
                rbPrice.Visible = false;

            }

            if (pos.IsUserHaveAuthorizationChangePrice(UserInfo.username))
            {
                rbPrice.Enabled = true;
                txtRetailPrice.Enabled = true;
            }

            if (editField == EditedField.DiscountedPrice)
            {
                rbDiscounted.Checked = true;
                txtDiscounted.Select();
                txtDiscounted.SelectionStart = 0;
                txtDiscounted.SelectionLength = txtDiscounted.Text.Length;
            }
            else if (editField == EditedField.RetailPrice)
            {
                if (txtRetailPrice.Enabled)
                {
                    rbPrice.Checked = true;
                    txtRetailPrice.Select();
                    txtRetailPrice.SelectionStart = 0;
                    txtRetailPrice.SelectionLength = txtRetailPrice.Text.Length;
                }
                else
                {
                    rbQuantity.Checked = true;
                    txtQty.Select();
                    txtQty.SelectionStart = 0;
                    txtQty.SelectionLength = txtQty.Text.Length;
                }
            }
            else
            {
                rbQuantity.Checked = true;
                txtQty.Select();
                txtQty.SelectionStart = 0;
                txtQty.SelectionLength = txtQty.Text.Length;
            }

            if (myDet.FinalPrice > 0)
            {
                rbPrice.Enabled = false;
                txtRetailPrice.Enabled = false;
                txtDiscounted.Enabled = false;
                rbDiscounted.Enabled = false;
            }

            if (isUsingLowQuantityPrompt)
            {
                txtStockBalance.Text = ItemController.GetStockOnHand(myDet.ItemNo, PointOfSaleInfo.InventoryLocationID).ToString("N2");
                if (ItemController.IsLowQuantityItem(myDet.ItemNo, PointOfSaleInfo.InventoryLocationID, myDet.Quantity.GetValueOrDefault(0)))
                {
                    txtStockBalance.BackColor = Color.Red;
                }
                else
                {
                    txtStockBalance.BackColor = Color.White;
                }
            }

            if (showPackingSize)
            {
                Load_PackingSize(myDet.ItemNo);
                if (!string.IsNullOrEmpty(myDet.PackingSize))
                {
                    cmbPackingSize.Text = myDet.PackingSize;
                    if (myDet.PackingQty.HasValue) txtQty.Text = myDet.PackingQty.ToString();
                    if (rbQuantity.Checked) txtEntry.Text = txtQty.Text;
                }
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

        private decimal RecalculateDiscountedPrice(decimal _discount1, string _discount2, decimal _retailPrice, decimal _quantity)
        {
            decimal result = 0;
            decimal _discPercent1 = 0;
            result = _retailPrice * _quantity;
            result = result - (result * _discount1 / 100);
            if (_discount2.StartsWith("$"))
            {
                decimal dollar;
                decimal.TryParse(_discount2.TrimStart('$'), out dollar);
                result = result - dollar;
                result = result / _quantity;
            }

            return result;
        }


        //handle line ordering
        private bool ApplyLineChanges()
        {
            if (myDet != null)
            {
                //update qty
                decimal qty = 0;
                int qtyInteger = 0;
                #region *) Validation: Quantity is INT
                if (IsUseWeight)
                {
                    if (!decimal.TryParse(txtQty.Text, out qty))
                    {
                        MessageBox.Show("Invalid Qty!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtQty.Select();
                        txtQty.SelectionStart = 0;
                        txtQty.SelectionLength = txtQty.Text.Length;
                        isError = true;
                        return false;
                    }
                }
                else
                {
                    if (!int.TryParse(txtQty.Text, out qtyInteger))
                    {
                        MessageBox.Show("Invalid Qty!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtQty.Select();
                        txtQty.SelectionStart = 0;
                        txtQty.SelectionLength = txtQty.Text.Length;
                        isError = true;
                        return false;
                    }
                    qty = qtyInteger;
                }
                if (qty == 0)
                {
                    MessageBox.Show("Qty cannot be 0!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    rbQuantity.Checked = true;
                    //txtQty.SelectAll();
                    txtQty.Select();
                    txtQty.SelectionStart = 0;
                    txtQty.SelectionLength = txtQty.Text.Length;
                    isError = true;
                    return false;
                }
                #endregion

                #region *) Convert qty base on selected packing size
                if (showPackingSize && cmbPackingSize.SelectedValue is decimal)
                {
                    myDet.PackingSize = cmbPackingSize.Text;
                    myDet.PackingQty = qty;
                    qty = qty * (decimal)cmbPackingSize.SelectedValue;
                }
                else
                {
                    myDet.PackingSize = null;
                    myDet.PackingQty = null;
                }
                #endregion

                //update retail price
                decimal RetailPrice;
                #region *) Validation: Retail Price is DECIMAL
                if (!decimal.TryParse(txtRetailPrice.Text, out RetailPrice))
                {
                    MessageBox.Show("Invalid Retail Price!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    rbPrice.Checked = true;
                    txtRetailPrice.Select();
                    txtRetailPrice.SelectionStart = 0;
                    txtRetailPrice.SelectionLength = txtRetailPrice.Text.Length;
                    isError = true;
                    return false;
                }
                else
                {

                    if (txtRetailPrice.Enabled == true)
                    {
                        string status;
                        UserMst CurrUser = new UserMst(UserInfo.username);
                        if (!pos.IsAuthorizedChangePriceManually(myDet.Item, UserInfo.username, RetailPrice, out status))
                        {
                            frmSupervisorLogin f = new frmSupervisorLogin();
                            f.StartPosition = FormStartPosition.CenterParent;
                            f.NeedSomeoneElseToVerify = true;
                            f.OnlyCheckLogin = true;
                            f.ShowDialog();
                            if (f.IsAuthorized)
                            {
                                if (!pos.IsAuthorizedChangePriceManually(myDet.Item, f.mySupervisor, RetailPrice, out status))
                                {
                                    MessageBox.Show(string.Format("Login {0} not authorized to change price.", f.mySupervisor));
                                    rbPrice.Checked = true;
                                    txtRetailPrice.Select();
                                    txtRetailPrice.SelectionStart = 0;
                                    txtRetailPrice.SelectionLength = txtRetailPrice.Text.Length;
                                    isError = true;
                                    return false;
                                }
                                else
                                {
                                    string msg = string.Format("Change Price Item {0} from {1} to {2} for transaction {3}", myDet.ItemNo, OldRetailPrice.ToString("N2"), RetailPrice.ToString("N2"), pos.myOrderHdr.OrderHdrID);
                                    AccessLogController.AddLogHelper(PointOfSaleInfo.PointOfSaleID, AccessSource.POS, UserInfo.username, f.mySupervisor, "Change Price", msg);
                                }
                            }
                            else
                            {
                                rbPrice.Checked = true;
                                txtRetailPrice.Select();
                                txtRetailPrice.SelectionStart = 0;
                                txtRetailPrice.SelectionLength = txtRetailPrice.Text.Length;
                                isError = true;
                                return false;
                            }

                            f.Dispose();
                        }
                        else
                        {
                            string msg = string.Format("Change Price Item {0} from {1} to {2} for transaction {3}", myDet.ItemNo, OldRetailPrice.ToString("N2"), RetailPrice.ToString("N2"), pos.myOrderHdr.OrderHdrID);
                            AccessLogController.AddLogHelper(PointOfSaleInfo.PointOfSaleID, AccessSource.POS, UserInfo.username, "", "Change Price", msg);
                        }
                    }
                }
                #endregion

                decimal DiscountedPrice;
                #region *) Validation: Discounted Price is DECIMAL && is lower/equal than retail price
                if (!decimal.TryParse(txtDiscounted.Text, out DiscountedPrice))
                {
                    MessageBox.Show("Invalid Discounted Price!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    rbDiscounted.Checked = true;
                    txtDiscounted.Select();
                    txtDiscounted.SelectionStart = 0;
                    txtDiscounted.SelectionLength = txtDiscounted.Text.Length;
                    isError = true;
                    return false;
                }
                #endregion
                if (!AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.SalesInvoice.AllowEditPriceTwoDecimal), false))
                {
                    RetailPrice = Math.Round(RetailPrice, 2);
                    DiscountedPrice = Math.Round(DiscountedPrice, 2);
                }

                int NumOfTimes = 0;
                #region *) Validation: Num of times is INT
                if (string.IsNullOrEmpty(myDet.PointItemNo))
                {
                    if (myDet.Item.IsOpenPricePackage &&
                        !int.TryParse(txtNumOfTimes.Text, out NumOfTimes))
                    {
                        MessageBox.Show("Invalid Num of times!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    if (myDet.Item.IsOpenPricePackage && NumOfTimes <= 0)
                    {
                        MessageBox.Show("Invalid Num of times!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    if (myDet.Item.IsOpenPricePackage)
                    {
                        myDet.PointGetAmount = (decimal)NumOfTimes;
                    }
                }
                #endregion

                decimal discPercent1 = 0;
                Calculate_Discount1(RetailPrice, DiscountedPrice, out discPercent1);

                myDet.DiscountDetail = (Discount1 + "+" + Discount2).TrimEnd('+');
                txtDiscountDetail.Text = myDet.DiscountDetail;

                //Recalculate Discounted Price
                if (!String.IsNullOrEmpty(Discount2) && Discount2.Contains("$"))
                    DiscountedPrice = RecalculateDiscountedPrice(discPercent1, Discount2, RetailPrice, qty);

                //decimal DiscPercent = ((RetailPrice - DiscountedPrice) / (RetailPrice + 0.00001M)) * 100;
                decimal DiscPercent = 0;
                if (RetailPrice != 0)
                {
                    if (myDet.FinalPrice > 0)
                        DiscPercent = ((RetailPrice - myDet.FinalPrice) / RetailPrice) * 100;
                    else
                        DiscPercent = ((RetailPrice - DiscountedPrice) / RetailPrice) * 100;
                }
                myDet.Discount = DiscPercent;
                if (!myDet.IsPromo && myDet.Discount != pos.GetPreferredDiscount())
                {
                    #region *) Validation: Discount given is not exceed the Discount Limit
                    UserMst CurrUser = new UserMst(PowerPOS.Container.UserInfo.username);
                    if (!CurrUser.IsAbleToGiveDiscount(DiscPercent))
                    {
                        MessageBox.Show("Discount exceed the discount limit set", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        rbDiscounted.Checked = true;
                        txtDiscounted.Select();
                        return false;
                    }
                    #endregion
                }

                //check if package
                if (myDet.IsPackageRedeemed)
                {
                    myDet.PackageBreakdownAmount = myDet.PackageBreakdownAmount / (myDet.Quantity ?? 1) * qty;
                }
                decimal originalQty = myDet.Quantity ?? 0;
                myDet.Quantity = qty;
                myDet.UnitPrice = RetailPrice;

                myDet.IsPriceManuallyChange = false;
                if (RetailPrice != myDet.Item.RetailPrice && ((myDet.Item.IsServiceItem.HasValue && myDet.Item.IsServiceItem.Value) || myDet.Item.IsInInventory))
                {
                    myDet.IsPriceManuallyChange = true;
                }

                if (myDet.Item.IsNonDiscountable)
                {
                    myDet.Discount = 0;
                }
                else
                {
                    if (!myDet.IsPromo)
                    {

                        if (Math.Round(currentDiscount,2) != DiscountedPrice)
                        {
                            myDet.IsSpecial = true;
                        }
                    }

                    //update discount - IF NON PROMO
                    //if (!myDet.IsPromo)
                    //{
                    //    myDet.Discount = DiscPercent;
                    //    if (myDet.Discount != pos.GetPreferredDiscount())
                    //    {
                    //        myDet.IsSpecial = true;
                    //    }
                    //    else
                    //    {
                    //        myDet.Discount = 0;
                    //    }
                    //}

                }

                myDet.IsPreOrder = cbOrder.Checked;
                pos.CalculateLineAmount(ref myDet);

                for (int x = 0; x < pos.myOrderDet.Count; x++)
                {
                    if (pos.myOrderDet[x].ItemNo == myDet.ItemNo && pos.myOrderDet[x].Item.IsInInventory == true && pos.myOrderDet[x].Item.IsServiceItem == true)
                    {
                        bool addRemarks = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.EnableKeyInOpenPriceItemName), false);
                        if (addRemarks)
                        {
                            if (pos.myOrderDet[x].Remark == null || pos.myOrderDet[x].Remark == "")
                            {
                                frmKeyboard keyOpenPriceRemarks = new frmKeyboard();
                                keyOpenPriceRemarks.IsInteger = false;
                                keyOpenPriceRemarks.textMessage = "Enter Remarks";
                                keyOpenPriceRemarks.ShowDialog();
                                pos.myOrderDet[x].Remark += keyOpenPriceRemarks.value;

                            }
                        }
                    }
                }

                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.LowQuantityPromptInSalesScreen), false))
                {
                    decimal minQty = ItemController.GetMinQty(myDet.ItemNo, PointOfSaleInfo.InventoryLocationID);
                    decimal stockOnHand = ItemController.GetStockOnHand(myDet.ItemNo, PointOfSaleInfo.InventoryLocationID);
                    decimal tmpQty = 0;

                    foreach (var b in pos.FetchUnsavedOrderDet())
                    {
                        if (b.ItemNo.Equals(myDet.ItemNo))
                        {
                            if (b.Quantity.HasValue)
                                tmpQty += b.Quantity.Value;
                            break;
                        }
                    }

                    if (stockOnHand - tmpQty <= minQty && minQty > 0)
                    {
                        MessageBox.Show(myDet.Item.ItemName + " is only left with " + (ItemController.GetStockOnHand(myDet.ItemNo, PointOfSaleInfo.InventoryLocationID) - tmpQty).ToString() + " qty.");
                    }

                    //if (ItemController.IsLowQuantityItem(myDet.ItemNo, PointOfSaleInfo.InventoryLocationID, qty))
                    //{
                    //    MessageBox.Show("This item is only left with " + ItemController.GetStockOnHand(myDet.ItemNo, PointOfSaleInfo.InventoryLocationID).ToString() + " qty.");
                    //}
                }


                if (ApplyPromo)
                {
                    pos.UndoPromo();
                    pos.ApplyMembershipDiscount();
                    pos.ApplyPromo();
                }

                if (!string.IsNullOrEmpty(myDet.LineInfo))
                {
                    if (cmbLineInfo.Items.Contains(myDet.LineInfo))
                    {
                        myDet.LineInfo = cmbLineInfo.SelectedValue.ToString();
                    }
                }

                if (!string.IsNullOrEmpty(txtLineRemark.Text) && myDet.Remark != txtLineRemark.Text)
                    myDet.Remark += txtLineRemark.Text;

                return true;
            }
            else
            {
                return false;
            }
        }

        private void rbContainerWeight_CheckedChanged(object sender, EventArgs e)
        {
            if (rbPrice.Checked)
            {
                IsInteger = false;
                txtEntry.Text = txtRetailPrice.Text;
                editField = EditedField.RetailPrice;
            }
            else if (rbQuantity.Checked)
            {
                IsInteger = !IsUseWeight;
                txtEntry.Text = txtQty.Text;
                editField = EditedField.Quantity;
            }
            else if (rbDiscounted.Checked)
            {
                IsInteger = false;
                txtEntry.Text = txtDiscounted.Text;
                editField = EditedField.DiscountedPrice;
            }
            else if (rbNumOfTimes.Checked)
            {
                IsInteger = true;
                txtEntry.Text = txtNumOfTimes.Text;
                editField = EditedField.NoOfTimes;
            }
            else if (rbContainerWeight.Checked)
            {
                IsInteger = false;
                txtEntry.Text = txtContainerWeight.Text;
                editField = EditedField.ContainerWeight;
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
                IsInteger = !IsUseWeight;
                txtEntry.Text = txtQty.Text;
                editField = EditedField.Quantity;
            }
            else if (rbDiscounted.Checked)
            {
                IsInteger = false;
                txtEntry.Text = txtDiscounted.Text;
                editField = EditedField.DiscountedPrice;
            }
            else if (rbNumOfTimes.Checked)
            {
                IsInteger = true;
                txtEntry.Text = txtNumOfTimes.Text;
                editField = EditedField.NoOfTimes;
            }
            else if (rbContainerWeight.Checked)
            {
                IsInteger = false;
                txtEntry.Text = txtContainerWeight.Text;
                editField = EditedField.ContainerWeight;
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
                IsInteger = !IsUseWeight;
                txtQty.Text = txtEntry.Text;
            }
            else if (rbDiscounted.Checked)
            {
                IsInteger = false;
                decimal val = 0;
                decimal.TryParse(txtEntry.Text, out val);
                txtDiscounted.Text = GetDiscountedPrice(val);
            }
            else if (rbNumOfTimes.Checked)
            {
                IsInteger = true;
                int val = txtEntry.Text.GetIntValue();
                txtNumOfTimes.Text = val.ToString();
            }
            else if (rbContainerWeight.Checked)
            {
                IsInteger = false;
                decimal val = 0;
                decimal.TryParse(txtEntry.Text, out val);
                txtContainerWeight.Text = val.ToString();
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
                f.StartPosition = FormStartPosition.CenterParent;
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
            txtDiscounted.Text = GetDiscountedPrice(discountedPrice);
            Discount1 = discount.ToString("N0") + "%";
            txtDiscountDetail.Text = (Discount1 + "+" + Discount2).TrimEnd('+');
        }

        private void txtRetailPrice_TextChanged(object sender, EventArgs e)
        {
            decimal retailPrice;
            if (decimal.TryParse(txtRetailPrice.Text, out retailPrice))
            {
                if (!myDet.Item.IsNonDiscountable)
                {
                    if (!myDet.IsSpecial)
                    {
                        txtDiscounted.Text = GetDiscountedPrice(retailPrice - retailPrice * pos.GetPreferredDiscount() / 100);
                    }
                    else
                    {
                        txtDiscounted.Text = GetDiscountedPrice(retailPrice - retailPrice * myDet.Discount / 100);
                    }
                }
                else
                {
                    txtDiscounted.Text = GetDiscountedPrice(retailPrice);
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
            if (!myDet.IsPromo)
            {
                txtRetailPrice.Text = myDet.UnitPrice.ToString("N");
                txtDiscounted.Text = GetDiscountedPrice(myDet.UnitPrice - myDet.UnitPrice * discount / 100);
                Discount2 = "";
                Discount1 = discount.ToString("N0") + "%";
                myDet.Discount2Percent = 0;
                myDet.Discount2Dollar = 0;
                txtDiscountDetail.Text = (Discount1 + "+" + Discount2).TrimEnd('+');
            }
            else
            {
                txtRetailPrice.Text = myDet.PromoUnitPrice.ToString("N");
                txtDiscounted.Text = GetDiscountedPrice(myDet.UnitPrice - myDet.UnitPrice * discount / 100);
                Discount2 = "";
                Discount1 = discount.ToString("N0") + "%";
                txtDiscountDetail.Text = (((decimal)myDet.PromoDiscount).ToString("N2") + "%" + "+" + Discount2).TrimEnd('+');

            }
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

        private void txtContainerWeight_Enter(object sender, EventArgs e)
        {
            rbContainerWeight.Checked = true;
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
                txtDiscounted.Text = GetDiscountedPrice(discountedPrice);
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

                txtDiscounted.Text = GetDiscountedPrice(TotalDiscountedPrice);
                if (!myDet.IsPromo)
                {
                    txtDiscountDetail.Text = (Discount1 + "+" + Discount2).TrimEnd('+');
                }
                else
                {
                    txtDiscountDetail.Text = myDet.PromoDiscount.ToString("N2") + "%" + "+" + Discount2;
                }
                if (tag == "%")
                {
                    myDet.Discount2Percent = discount;
                    myDet.Discount2Dollar = 0;
                }
                else
                {
                    myDet.Discount2Percent = 0;
                    myDet.Discount2Dollar = discount;
                }
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
            decimal discPercent1;

            UserMst user = new UserMst(UserInfo.username);
            if (user != null && user.UserGroup.DiscountLimitPercent != null)
            {
                decimal realdiscountedprice = RetailPrice - (RetailPrice * (user.UserGroup.DiscountLimitPercent ?? 0) / 100);
                if (DiscountedPrice < realdiscountedprice)
                {
                    MessageBox.Show("(Warning) Discount exceed the discount limit set");
                    txtDiscounted.Text = txtRetailPrice.Text;
                }
                else
                {
                    Calculate_Discount1(RetailPrice, DiscountedPrice, out discPercent1);
                    txtDiscountDetail.Text = (Discount1 + "+" + Discount2).TrimEnd('+');
                }
            }
            else
            {
                Calculate_Discount1(RetailPrice, DiscountedPrice, out discPercent1);
                txtDiscountDetail.Text = (Discount1 + "+" + Discount2).TrimEnd('+');
            }
        }

        private void Calculate_Discount1(decimal RetailPrice, decimal TotalDiscountedPrice, out decimal discPercent1)
        {
            string a = "";
            a.GetIntValue();
            discPercent1 = 0;
            if (!string.IsNullOrEmpty(Discount2))
            {

                if (Discount2.StartsWith("$"))
                {
                    decimal grossAmount = RetailPrice * myDet.Quantity.GetValueOrDefault(0);
                    decimal dollar;
                    decimal.TryParse(Discount2.TrimStart('$'), out dollar);
                    discPercent1 = (1 - ((myDet.Amount + dollar) / grossAmount)) * 100;
                }
                else if (Discount2.EndsWith("%"))
                {
                    decimal percent;
                    decimal.TryParse(Discount2.TrimEnd('%'), out percent);
                    if (percent != 100)
                    {
                        discPercent1 = (1 - (TotalDiscountedPrice / ((1 - percent / 100) * RetailPrice))) * 100;
                    }
                    else
                    {
                        discPercent1 = 100;
                    }
                }
            }
            else
            {
                if (RetailPrice != 0)
                    discPercent1 = (1 - TotalDiscountedPrice / RetailPrice) * 100;
            }
            Discount1 = discPercent1.ToString("N0") + "%";
        }

        private string GetDiscountedPrice(decimal discountedPrice)
        {
            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Order.DoNotRoundDiscountedPrice), false))
            {
                return Math.Round(discountedPrice, 4).Normalize().ToString();
            }
            else
            {
                //return CommonUILib.RemoveRoundUp(discountedPrice).ToString("N2");
                return discountedPrice.ToString("N");
            }
        }

        private void btnGetWeight_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            frmLoadWeighing frm = new frmLoadWeighing(txtUOM.Text);
            frm.ShowDialog();
            if (frm.IsSuccess)
            {
                txtQty.Text = frm.Weight.ToString("N4");
                txtTotalWeight.Text = frm.Weight.ToString("N4");
                this.ActiveControl = txtContainerWeight;
            }
            else
                MessageBox.Show(frm.Status);

            //Weighing wm = new Weighing();
            //string comPort = AppSetting.GetSetting(AppSetting.SettingsName.WeighingMachine.COMPort);
            //string cmd = AppSetting.GetSetting(AppSetting.SettingsName.WeighingMachine.CommandToExecute);
            //bool isSuccess = false;
            //string result = "";
            //isSuccess = wm.GetWeight(comPort, cmd+"\r\n", out result);
            //Logger.writeLog(">> Weighing Machine Result :" + result);
            //int reTry = 0;
            //while (!isSuccess && reTry < 3)
            //{
            //    Thread.Sleep(3000);
            //    reTry++;
            //    isSuccess = wm.GetWeight(comPort, cmd, out result);
            //    Logger.writeLog(">> Weighing Machine Result :" + result);
            //}
            //if (isSuccess)
            //{
            //    try
            //    {
            //        //data = "0.21KG\r\nS00";
            //        result = result.Split(new string[] { "\r\n" }, StringSplitOptions.None)[0];
            //        Logger.writeLog(">> Weighing Machine Result (Splitted):" + result);
            //        string weightStr = "";
            //        string uomStr = "";
            //        for (int k = 0; k < result.Length; k++)
            //        {
            //            if (result[k] == ' ') break;
            //            if (char.IsNumber(result[k]) || result[k] == '.')
            //                weightStr += result[k].ToString();
            //            else
            //                uomStr += result[k].ToString();
            //        }
            //        uomStr = uomStr.Trim().Trim(("\r\n").ToCharArray()).Trim().Trim(("\r").ToCharArray()).Trim(("\n").ToCharArray());
            //        if (!uomStr.ToLower().Trim().Equals(txtUOM.Text.ToLower().Trim()))
            //            MessageBox.Show(string.Format("Weighing machine UOM ({0}) is not tally with Product UOM ({1})", uomStr, txtUOM.Text));
            //        else
            //            txtQty.Text = weightStr;
            //    }
            //    catch (Exception ex)
            //    {
            //        Logger.writeLog(ex);
            //        MessageBox.Show("Please try again");
            //    }
            //}
            //else
            //{
            //    Logger.writeLog("Error Weighing : "+result);
            //    MessageBox.Show("Please try again");
            //}
            this.Enabled = true;
            //frm.Hide();
            //CommonUILib.hideTransparent();
        }

        private void txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Invoice.EnterAsOK), false))
                {
                    btnOK_Click(btnOK, new EventArgs());
                }
            }
        }

        private void txtQty_Leave(object sender, EventArgs e)
        {

        }

        private void txtQty_TextChanged(object sender, EventArgs e)
        {
            if (isUsingLowQuantityPrompt)
            {
                txtStockBalance.Text = ItemController.GetStockOnHand(myDet.ItemNo, PointOfSaleInfo.InventoryLocationID).ToString("N2");
                decimal qty1 = 0;
                if (decimal.TryParse(txtQty.Text, out qty1))
                {
                    if (ItemController.IsLowQuantityItem(myDet.ItemNo, PointOfSaleInfo.InventoryLocationID, qty1))
                    {
                        txtStockBalance.BackColor = Color.Red;
                    }
                    else
                    {
                        txtStockBalance.BackColor = Color.White;
                    }
                }
            }
        }

        private void txtNumOfTimes_Enter(object sender, EventArgs e)
        {
            rbNumOfTimes.Checked = true;
        }

        private void Load_PackingSize(string itemNo)
        {
            ItemSupplierMapCollection ismColl = new ItemSupplierMapCollection();
            ismColl.Where(ItemSupplierMap.Columns.ItemNo, itemNo);
            ismColl.Where(ItemSupplierMap.Columns.Deleted, false);
            ismColl.Load();
            cmbPackingSize.Items.Clear();
            //List<KeyValuePair<string, decimal>> list = new List<KeyValuePair<string, decimal>>();
            //Dictionary<string, decimal> list = new Dictionary<string, decimal>();
            DataTable dt = new DataTable();
            dt.Columns.Add("Size", Type.GetType("System.String"));
            dt.Columns.Add("Qty", Type.GetType("System.Decimal"));

            foreach (var ism in ismColl)
            {
                //list.Add(new KeyValuePair<string, decimal>(ism.PackingSize1, ism.PackingSizeUOM1.GetValueOrDefault(0)));
                //list.Add(ism.PackingSize1, ism.PackingSizeUOM1.GetValueOrDefault(0));
                if (dt.Select("Size = '" + ism.PackingSize1 + "'").Length == 0)
                {
                    dt.Rows.Add(ism.PackingSize1.ToString(), ism.PackingSizeUOM1.GetValueOrDefault(0));
                    dt.Rows.Add(ism.PackingSize2.ToString(), ism.PackingSizeUOM2.GetValueOrDefault(0));
                    dt.Rows.Add(ism.PackingSize3.ToString(), ism.PackingSizeUOM3.GetValueOrDefault(0));
                    dt.Rows.Add(ism.PackingSize4.ToString(), ism.PackingSizeUOM4.GetValueOrDefault(0));
                    dt.Rows.Add(ism.PackingSize5.ToString(), ism.PackingSizeUOM5.GetValueOrDefault(0));
                    dt.Rows.Add(ism.PackingSize6.ToString(), ism.PackingSizeUOM6.GetValueOrDefault(0));
                    dt.Rows.Add(ism.PackingSize7.ToString(), ism.PackingSizeUOM7.GetValueOrDefault(0));
                    dt.Rows.Add(ism.PackingSize8.ToString(), ism.PackingSizeUOM8.GetValueOrDefault(0));
                    dt.Rows.Add(ism.PackingSize9.ToString(), ism.PackingSizeUOM9.GetValueOrDefault(0));
                    dt.Rows.Add(ism.PackingSize10.ToString(), ism.PackingSizeUOM10.GetValueOrDefault(0));
                }
            }

            dt = dt.DeleteRow("Size", "");
            dt.Rows.InsertAt(dt.NewRow(), 0);
            cmbPackingSize.DataSource = dt;
            cmbPackingSize.DisplayMember = "Size";
            cmbPackingSize.ValueMember = "Qty";
        }

        private void txtContainerWeight_TextChanged(object sender, EventArgs e)
        {
            decimal total = 0;
            decimal container = 0;
            decimal.TryParse(txtTotalWeight.Text, out total);
            decimal.TryParse(txtContainerWeight.Text, out container);

            txtQty.Text = (total - container).ToString();
        }
    }
}