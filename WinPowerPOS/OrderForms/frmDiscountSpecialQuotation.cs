using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using LanguageManager = WinPowerPOS.Properties.Language;

namespace WinPowerPOS.OrderForms
{
    public partial class frmDiscountSpecialQuotation : Form
    {
        public QuoteController pos;
        public bool IsSuccessful;
        public decimal discApplied;
        public string LineID;

        public frmDiscountSpecialQuotation()
        {
            InitializeComponent();
        }

        private void btnDiscount_Click(object sender, EventArgs e)
        {
            try
            {
                string status1 = "";
                QuotationDet tempDet = pos.GetLine(LineID, out status1);

                string DiscountName = ((Button)sender).Tag.ToString();
                pos.applyDiscountOrderDet(DiscountName, ref tempDet);
                this.Close();
            }
            catch (Exception X)
            {
                CommonUILib.HandleException(X);
                this.Close();
            }
        }

        /*private bool ApplyDiscounts(decimal disc)
        {
            try
            {
                #region *) Assert: Discount given is not exceed the Discount Limit
                UserMst CurrUser = new UserMst(PowerPOS.Container.UserInfo.username);
                if (!CurrUser.isAbleToGiveDiscount(disc))
                    throw new Exception("(warning)" + LanguageManager.Discount_exceed_the_discount_limit_set_);
                #endregion

                pos.applyDiscount(disc);
                pos.ApplyMembershipDiscount();
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
        }*/

        private void button6_Click(object sender, EventArgs e)
        {
            IsSuccessful = false;
            this.Close();
        }

        private void frmDiscounts_Load(object sender, EventArgs e)
        {

            //load category list from database
            SpecialDiscountCollection sp = new SpecialDiscountCollection();
            sp.Where(SpecialDiscount.Columns.Deleted, false);
            sp.Where(SpecialDiscount.Columns.Enabled, true);
            sp.Where(SpecialDiscount.Columns.StartDate, SubSonic.Comparison.LessOrEquals, DateTime.Now);
            sp.Where(SpecialDiscount.Columns.EndDate, SubSonic.Comparison.GreaterOrEquals, DateTime.Now.AddDays(-1));
            sp.Where(SpecialDiscount.Columns.IsBankPromo, false);
            sp.OrderByAsc(SpecialDiscount.Columns.PriorityLevel);
            sp.Load();

            //populate the flow lay out with programmable button
            for (int i = 0; i < sp.Count; i++)
            {
                Button b = new Button();
                b.Width = 110;
                b.Height = 45;
                b.BackColor = System.Drawing.Color.Orange;
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
            try
            {
                //if using keyed in discount value, calculate the percentage first
                decimal discAmount = 0;
                if (!decimal.TryParse(txtDiscAmt.Text, out discAmount))
                    throw new Exception("(error)" + LanguageManager.Please_enter_a_valid_amount);
                string status = "";
                string lineDiscountID = pos.IsItemIsInOrderLine("LINE_DISCOUNT");
                if ( lineDiscountID == "")
                {
                    if (pos.AddItemToOrder(new Item("LINE_DISCOUNT"), -1, 0, false, out status))
                    {                
                        string lastID = pos.myOrderDet[pos.myOrderDet.Count -1].OrderDetID;
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
            catch (Exception X)
            {
                CommonUILib.HandleException(X);
            }
        }
    }
}
