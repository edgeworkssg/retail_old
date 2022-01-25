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

namespace WinPowerPOS
{
    public partial class frmSetExhangeRate : Form
    {
        public frmSetExhangeRate()
        {
            InitializeComponent();
        }

        private void frmSetExhangeRate_Load(object sender, EventArgs e)
        {
            //Load for hashtable if any
            ExchangeRateController ctrl = new ExchangeRateController();
            ctrl.Load("ExchangeRate");
            Hashtable h = ctrl.GetHashTable();

            //Load currency list
            CurrencyCollection col = new CurrencyCollection();
            col.Where(Currency.Columns.Deleted,false);
            col.Load();
            
            for (int i = 0; i < col.Count; i++)
            {
                //
                if (i >= 15) break;

                this.Controls.Find("lblSetExchangeRate" + (i + 1).ToString(),true)[0].Text = col[i].CurrencyCode;
                this.Controls.Find("lblSetExchangeRate" + (i + 1).ToString(), true)[0].Enabled = true;
                this.Controls.Find("txtExchangeRate" + (i + 1).ToString(), true)[0].Enabled = true;
                


                if (h != null && h.ContainsKey(col[i].CurrencyCode))
                {
                    this.Controls.Find("txtExchangeRate" + (i + 1).ToString(), true)[0].Text = h[col[i].CurrencyCode].ToString();
                }
                else 
                {
                    this.Controls.Find("txtExchangeRate" + (i + 1).ToString(), true)[0].Text = "0.0";
                }
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSet_Click(object sender, EventArgs e)
        {                        
            //Loop through
            Hashtable ht = new Hashtable();
            for (int i = 0; i < 15; i++)
            {
                if (this.Controls.Find("lblSetExchangeRate" + (i + 1).ToString(), true)[0].Enabled == true)
                {
                    decimal tmp;
                    if (decimal.TryParse(this.Controls.Find("txtExchangeRate" + (i + 1).ToString(), true)[0].Text, out tmp)
                        && tmp >=0.0M)
                    {
                        ht.Add(
                            this.Controls.Find("lblSetExchangeRate" + (i + 1).ToString(),
                            true)[0].Text,
                            tmp);
                    }
                    else
                    {
                        MessageBox.Show("Please enter positive decimal value for exchange rate");
                        return;
                    }
                }
            }

            ExchangeRateController ctrl = new ExchangeRateController();
            ctrl.SetHashTable(ht);
            ctrl.Save("ExchangeRate");
            MessageBox.Show("Exchange Rate set successfully");
            this.Close();
        }
    }
}
