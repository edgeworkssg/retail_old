using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using LanguageManager = WinPowerPOS.Properties.Language;
using WinPowerPOS.LoginForms;

namespace WinPowerPOS.OrderForms
{
    public partial class frmDiscountsQuotation : Form
    {
        public QuoteController pos;
        public bool IsSuccessful;
        public decimal discApplied;
        public string discChoosed;

        private bool EnableSecondDiscount = false;

        public frmDiscountsQuotation()
        {
            InitializeComponent();
            EnableSecondDiscount = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Discount.EnableSecondDiscount), false);
        }

        private void btnDiscount_Click(object sender, EventArgs e)
        {
            try
            {
                
                    string DiscountName = ((Button)sender).Tag.ToString();
                    discChoosed = ((Button)sender).Text.ToString();
                    if (DiscountName != "0")
                    {
                        SpecialDiscount sd = new SpecialDiscount("DiscountName", DiscountName);
                        if (sd != null && sd.DiscountName != "")
                        {
                            //check bank Promo or not
                            if ((bool)sd.IsBankPromo)
                            {
                                  int qty = 0;
                                  using (var numpad = new frmKeypad())
                                  {
                                      numpad.textMessage = "ENTER QTY";
                                      numpad.ShowDialog();
                                      int.TryParse(numpad.value, out qty);
                                      for (int i = 0; i < qty; i++)
                                      {
                                          ApplyBankPromo(sd.DiscountName);
                                      }
                                  }
                            }
                            else
                            {
                                  
                                if (pos.CalculateGrossAmount() > sd.MinimumSpending)
                                {
                                    //pos.clearDiscount(0);
                                    ApplyDiscounts(sd.DiscountName);
                                    IsSuccessful = true;
                                    this.Close();
                                }
                                else
                                { IsSuccessful = false; this.Close(); }
                            }
                        }
                        else
                        {
                            MessageBox.Show(LanguageManager.Error_encountered_while_applying_discounts_,
                                "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        ApplyBlankDiscounts(0);
                        IsSuccessful = true;
                        this.Close();
                    }
                
            }
            catch (Exception X)
            {
                CommonUILib.HandleException(X);
            }
        }

        private bool ApplyDiscounts(decimal disc)
        {
            try
            {
                #region *) Assert: Discount given is not exceed the Discount Limit
                UserMst CurrUser = new UserMst(PowerPOS.Container.UserInfo.username);
                if (!CurrUser.IsAbleToGiveDiscount(disc))
                    throw new Exception("(warning)" + LanguageManager.Discount_exceed_the_discount_limit_set_);
                #endregion

                pos.applyDiscount(disc);
                //pos.ApplyMembershipDiscount();
                IsSuccessful = true;
                discApplied = disc;
            }
            catch (Exception ex)
            {
                #region *) Handler for new type of exceptions
                /// The reason is to reduce the number of double MessageBox apperance
                if (ex.Message.StartsWith("(warning)") || ex.Message.StartsWith("(error)"))
                    throw ex;
                #endregion

                Logger.writeLog(ex);
                return false;
            }

            return true;
        }

        private bool ApplyBlankDiscounts(decimal disc)
        {
            try
            {
                #region *) Assert: Discount given is not exceed the Discount Limit
                UserMst CurrUser = new UserMst(PowerPOS.Container.UserInfo.username);
                if (!CurrUser.IsAbleToGiveDiscount(disc))
                    throw new Exception("(warning)" + LanguageManager.Discount_exceed_the_discount_limit_set_);
                #endregion

                Logger.writeLog("Remove Overall Discount");
                pos.clearDiscount(disc);
                //pos.ApplyMembershipDiscount();
                IsSuccessful = true;
                //discApplied = disc;
            }
            catch (Exception ex)
            {
                #region *) Handler for new type of exceptions
                /// The reason is to reduce the number of double MessageBox apperance
                if (ex.Message.StartsWith("(warning)") || ex.Message.StartsWith("(error)"))
                    throw ex;
                #endregion

                Logger.writeLog(ex);
                return false;
            }

            return true;
        }

        private bool ApplyDiscounts(string discName)
        {
            try
            {
                #region *) Assert: Discount given is not exceed the Discount Limit
                UserMst CurrUser = new UserMst(PowerPOS.Container.UserInfo.username);
                //if (!CurrUser.isAbleToGiveDiscount(disc))
                //    throw new Exception("(warning)" + LanguageManager.Discount_exceed_the_discount_limit_set_);
                #endregion

                if (!(discName == "No Discount"))
                {
                    bool isOverwriteExistingPromo = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Promo.IsOverwriteExistingPromoOnButtonDiscountAll), false);
                    pos.applyDiscount(discName, isOverwriteExistingPromo);
                    //pos.applyDiscount(discName);
                    //pos.ApplyMembershipDiscount();
                    IsSuccessful = true;
                }
                //discApplied = disc;
            }
            catch (Exception ex)
            {
                #region *) Handler for new type of exceptions
                /// The reason is to reduce the number of double MessageBox apperance
                if (ex.Message.StartsWith("(warning)") || ex.Message.StartsWith("(error)"))
                    throw ex;
                #endregion

                Logger.writeLog(ex);
                return false;
            }

            return true;
        }

        private bool ApplyBankPromo(string discName)
        {
            try
            {
                if (!(discName == "No Discount"))
                {
                    decimal BankDisc = 0;
                    SpecialDiscount sd = new SpecialDiscount("DiscountName",discName);
                    
                    if (sd != null && sd.DiscountName != "")
                    {
                        if (pos.CalculateGrossAmount() > sd.MinimumSpending)
                        {
                            pos.applyBankDiscount(sd.DiscountPercentage, sd.DiscountName);
                            IsSuccessful = true;
                            this.Close();
                        }
                        else
                        {
                            IsSuccessful = false;
                            MessageBox.Show("Total Amount is still less than Minimum Spending");
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("No Bank Discount Set Up");
                        IsSuccessful = false;
                        this.Close();
                    }
                    
                }
                
            }
            catch (Exception ex)
            {
                #region *) Handler for new type of exceptions
                /// The reason is to reduce the number of double MessageBox apperance
                if (ex.Message.StartsWith("(warning)") || ex.Message.StartsWith("(error)"))
                    throw ex;
                #endregion

                Logger.writeLog(ex);
                return false;
            }

            return true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            IsSuccessful = false;
            this.Close();
        }

        private void frmDiscounts_Load(object sender, EventArgs e)
        {
            pnlAdditionalDiscount.Visible = EnableSecondDiscount;

            //load category list from database
            SpecialDiscountCollection sp = new SpecialDiscountCollection();
            sp.Where(SpecialDiscount.Columns.Deleted, false);
            sp.Where(SpecialDiscount.Columns.Enabled, true);
            sp.Where(SpecialDiscount.Columns.StartDate, SubSonic.Comparison.LessOrEquals, DateTime.Now);
            sp.Where(SpecialDiscount.Columns.EndDate, SubSonic.Comparison.GreaterOrEquals, DateTime.Now.AddDays(-1));
            sp.OrderByAsc(SpecialDiscount.Columns.PriorityLevel);
            sp.Load();

            //populate the flow lay out with programmable button
            for (int i = 0; i < sp.Count; i++)
            {
                Button b = new Button();
                b.Width = 110;
                b.Height = 45;
                if (!(bool)sp[i].IsBankPromo)
                {
                    b.BackColor = System.Drawing.Color.Orange;
                }
                else
                {
                    b.BackColor = System.Drawing.Color.DeepSkyBlue;
                }

                b.Font = new Font("Verdana", 9, FontStyle.Bold);
                if (sp[i].ShowLabel)
                {
                    b.Text = sp[i].DiscountName;                    
                }
                else
                {
                    b.Text = sp[i].DiscountPercentage.ToString("N2") + "%";
                }
                b.Tag = sp[i].DiscountName;
                
                b.Click += delegate
                {
                    btnDiscount_Click(b, new EventArgs());
                };
                flowLayoutPanel1.Controls.Add(b);
            }


        }
        private void btnCat_Click(object sender, EventArgs e)
        {
            MessageBox.Show(((Button)sender).Tag.ToString());
        }

        private void btnApplySpecifiedDiscount_Click(object sender, EventArgs e)
        {

        }

        private void btnBankDiscount_Click(object sender, EventArgs e)
        {
            
        }

        private void btnApplySpecifiedDiscount_Click_1(object sender, EventArgs e)
        {
            try
            {
                frmSupervisorLogin f = new frmSupervisorLogin();
                f.privilegeName = PrivilegesController.GIVE_DISCOUNT;
                f.ShowDialog();
                if (!f.IsAuthorized)
                {
                    f.Dispose();
                    return;
                }
                else
                {
                    //if using keyed in discount value, calculate the percentage first
                    decimal discAmount = 0;
                    if (!decimal.TryParse(txtDiscAmt.Text, out discAmount))
                        throw new Exception("(error)" + LanguageManager.Please_enter_a_valid_amount);
                    string status = "";
                    string lineDiscountID = pos.IsItemIsInOrderLine("LINE_DISCOUNT");
                    if (lineDiscountID == "")
                    {
                        if (pos.AddItemToOrder(new Item("LINE_DISCOUNT"), -1, 0, false, out status))
                        {
                            string lastID = pos.myOrderDet[pos.myOrderDet.Count - 1].OrderDetID;
                            if (pos.ChangeOrderLineUnitPrice(lastID, discAmount, out status))
                            {
                                IsSuccessful = true;
                                this.Close();
                            }
                            else
                            {
                                MessageBox.Show("Error:" + status);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Error:" + status);
                        }
                    }
                    else
                    {
                        if (pos.ChangeOrderLineUnitPrice(lineDiscountID, discAmount, out status))
                        {
                            IsSuccessful = true;
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Error:" + status);
                        }
                    }
                }

            }
            catch (Exception X)
            {
                CommonUILib.HandleException(X);
            }
        }

        private void btnAddDisc_Click(object sender, EventArgs e)
        {
            string tag = ((Button)sender).Tag.ToString();
            frmKeypad f = new frmKeypad();
            f.IsInteger = false;

            string Discount2 = "";

            if (!string.IsNullOrEmpty(Discount2) && Discount2.Contains(tag))
            {
                f.initialValue = Discount2.Replace(tag, "");
            }

            f.ShowDialog();
            decimal discount;
            if (!string.IsNullOrEmpty(f.value) && decimal.TryParse(f.value.ToString(), out discount))
            {
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

                List<string> status;
                bool success = pos.ApplyDiscountLevel2(Discount2, out status);
                if (status.Count > 0)
                {
                    string st = string.Join("\n", status.ToArray());
                    MessageBox.Show(st);
                }

                if (status.Count == pos.myOrderDet.Count)
                {
                    success = false;
                }
                if (success)
                {
                    IsSuccessful = true;
                    this.Close();
                }
            }
        }
    }
}
