using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using PowerPOSLib.Controller;
using System.Collections;
using WinPowerPOS.OrderForms;

namespace WinPowerPOS
{
    public partial class frmDeclareForeignCurrency : Form
    {
        public DateTime startDate, endDate;
        public int pointOfSaleID;
        public frmDeclareForeignCurrency()
        {
            InitializeComponent();
        }
        private void txtKeyPadOpen_Click(object sender, EventArgs e)
        {
            //prompt keypad
            frmKeypad f = new frmKeypad();
            f.IsInteger = false;
            f.initialValue = ((TextBox)sender).Text;
            f.ShowDialog();

            ((TextBox)sender).Text = f.value.ToString();
            //txt100_Validating(this, new CancelEventArgs());
        }

        private void frmSetExhangeRate_Load(object sender, EventArgs e)
        {                        
            //Load currency list
            CurrencyCollection col = new CurrencyCollection();
            col.Where(Currency.Columns.Deleted,false);
            col.Load();

            DataTable dt = ReceiptController.FetchForeignCurrencyPayment(startDate, endDate, pointOfSaleID);

            for (int i = 0; i < col.Count; i++)
            {
                //
                if (i >= 15) break;
                this.Controls.Find("gbForeignCurrency" + (i + 1).ToString(), true)[0].Text = col[i].CurrencyCode;
                this.Controls.Find("gbForeignCurrency" + (i + 1).ToString(), true)[0].Enabled = true;
                this.Controls.Find("lblSetExchangeRate" + (i + 1).ToString(),true)[0].Text = col[i].CurrencyCode;
                this.Controls.Find("lblSetExchangeRate" + (i + 1).ToString(), true)[0].Enabled = true;
                this.Controls.Find("txtExchangeRate" + (i + 1).ToString(), true)[0].Enabled = true;

                try
                {
                    object obj = dt.Compute("SUM(ForeignCurrencyAmount)", "ForeignCurrencyCode = '" + col[i].CurrencyCode + "'");
                    if (obj != null)
                    {
                        this.Controls.Find("lblSystemAmount" + (i + 1).ToString(), true)[0].Text = ((Decimal)obj).ToString("C").Replace("$", "");
                    }
                }
                catch (Exception ex)
                {
                    Logger.writeLog(ex);
                }
                //this.Controls.Find("txtExchangeRate" + (i + 1).ToString(), true)[0].Text = "0.0";                 
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Foreign Currency settlement are not enabled for Demo version. Get the full version for foreign currency settlement");
            //Loop through
            /*
            Hashtable ht = new Hashtable();
            for (int i = 0; i < 15; i++)
            {
                if (this.Controls.Find("lblSetExchangeRate" + (i + 1).ToString(), true)[0].Enabled == true)
                {
                    decimal tmp;
                    if (decimal.TryParse(this.Controls.Find("txtExchangeRate" + (i + 1).ToString(), true)[0].Text, out tmp))
                    {
                        ht.Add(
                            this.Controls.Find("lblSetExchangeRate" + (i + 1).ToString(),
                            true)[0].Text,
                            tmp);
                    }
                    else
                    {
                        MessageBox.Show("Please enter monetary value in the textbox");
                        return;
                    }
                }
            }
            */
            //MessageBox.Show("Declaration Completed");
            //this.Close();
        }

        private void txtExchangeRate1_TextChanged(object sender, EventArgs e)
        {
            Label lblSystemAmount,lblDefi;

            string id = ((TextBox)sender).Name.Replace("txtExchangeRate", "");

            lblSystemAmount = (Label)(this.Controls.Find("lblSystemAmount" + id, true)[0]);
            lblDefi = (Label)(this.Controls.Find("lblDefi" + id, true)[0]);
            
            decimal amount;
            if (Decimal.TryParse(((TextBox)sender).Text, out amount))
            {
                decimal systemAmount = 0.0M, defi=0.0M;
                if (Decimal.TryParse(lblSystemAmount.Text, out systemAmount))
                {
                    defi = amount - systemAmount;
                    lblDefi.Text = (defi).ToString("N2");
                    GroupBox gb = (GroupBox)(this.Controls.Find("gbForeignCurrency" + id, true)[0]);
                    if (defi > 0)
                    {
                        gb.BackColor = System.Drawing.Color.LightGreen;
                    }
                    else if (defi < 0)
                    {
                        gb.BackColor = System.Drawing.Color.Salmon;   
                    }
                    else if (defi == 0)
                    {
                        gb.BackColor = System.Drawing.Color.Transparent;   
                    }                    
                }
                else
                {
                    lblDefi.Text = "-";
                    GroupBox gb = (GroupBox)(this.Controls.Find("gbForeignCurrency" + id, true)[0]);
                    gb.BackColor = System.Drawing.Color.Salmon;   
                }
            }
            else
            {
                lblDefi.Text = "-";
                GroupBox gb = (GroupBox)(this.Controls.Find("gbForeignCurrency" + id, true)[0]);
                gb.BackColor = System.Drawing.Color.Salmon;   
            }
        }
    }
}
