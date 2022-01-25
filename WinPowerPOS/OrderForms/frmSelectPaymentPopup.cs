using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using POSDevices;   
using WinPowerPOS.Reports;
using PowerPOS.Container;
using System.Configuration;
using Features = PowerPOS.Feature;
namespace WinPowerPOS.OrderForms
{
    public partial class frmSelectPaymentPopup : Form
    {
        public POSController pos;
        
        public bool isSuccessful;
        public decimal amount;
        string status;
        public string PaymentTypeSelected;

        bool enableNetsCreditCardTerminal = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EnableCreditCardIntegration), false);
        string netsCreditCardPayment = (AppSetting.GetSetting(AppSetting.SettingsName.Payment.PaymentNETSCC_Grouped) + "").Trim().ToUpper();
        string netsCC_VISA = AppSetting.GetSetting(AppSetting.SettingsName.Payment.PaymentNETSCC_VISA);
        string netsCC_MASTER = AppSetting.GetSetting(AppSetting.SettingsName.Payment.PaymentNETSCC_MASTER);
        string netsCC_AMEX = AppSetting.GetSetting(AppSetting.SettingsName.Payment.PaymentNETSCC_AMEX);
        string netsCC_DINERS = AppSetting.GetSetting(AppSetting.SettingsName.Payment.PaymentNETSCC_DINERS);
        string netsCC_JCB = AppSetting.GetSetting(AppSetting.SettingsName.Payment.PaymentNETSCC_JCB);

        bool enableNETSIntegration = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EnableNETSIntegration), false);
        string netsATMCardPayment = POSController.PAY_NETSATMCard;
        string netsFlashPayPayment = POSController.PAY_NETSFlashPay;
        string netsCashCardPayment = POSController.PAY_NETSCashCard;
        
        public frmSelectPaymentPopup()
        {
            InitializeComponent();
            //isPartialPayment = false;
//            LoadPaymentTypeLabels();
        }
        private const int TOTAL_BUTTONS = 6;
        private void LoadPaymentTypeLabels()
        {

            Button b = new Button();
            b.Width = 134;
            b.Height = 77;
            b.BackColor = System.Drawing.Color.Orange;
            b.Font = new Font("Verdana", 9, FontStyle.Bold);
            b.Text = "CASH";
            b.Tag = "CASH";
            b.Click += delegate
            {
                btnMakePayment_Click(b, new EventArgs());
            };
            flowLayoutPanel1.Controls.Add(b);
            for (int i = 0; i < PointOfSaleInfo.PaymentTypes.Rows.Count; i++)
            {
                string paymentName = PointOfSaleInfo.PaymentTypes.Rows[i][1].ToString();
                if (enableNetsCreditCardTerminal &&
                        (paymentName == netsCC_VISA || paymentName == netsCC_MASTER || paymentName == netsCC_AMEX
                        || paymentName == netsCC_DINERS || paymentName == netsCC_JCB))
                {
                    continue;
                }

                if (enableNetsCreditCardTerminal && paymentName == "NETS")
                {
                    //iff NETS create 3 button for ATMCard,  Flash Pay and Cash Card
                    Button bATMCard = new Button();
                    bATMCard.Width = 134;
                    bATMCard.Height = 77;
                    bATMCard.BackColor = System.Drawing.Color.Orange;
                    bATMCard.Font = new Font("Verdana", 9, FontStyle.Bold);
                    bATMCard.Text = netsATMCardPayment;
                    bATMCard.Tag = netsATMCardPayment;
                    bATMCard.Click += delegate
                    {
                        btnMakePayment_Click(bATMCard, new EventArgs());
                    };
                    flowLayoutPanel1.Controls.Add(bATMCard);

                    Button bFlashPay = new Button();
                    bFlashPay.Width = 134;
                    bFlashPay.Height = 77;
                    bFlashPay.BackColor = System.Drawing.Color.Orange;
                    bFlashPay.Font = new Font("Verdana", 9, FontStyle.Bold);
                    bFlashPay.Text = netsFlashPayPayment;
                    bFlashPay.Tag = netsFlashPayPayment;
                    bFlashPay.Click += delegate
                    {
                        btnMakePayment_Click(bFlashPay, new EventArgs());
                    };
                    flowLayoutPanel1.Controls.Add(bFlashPay);

                    Button bCashCard = new Button();
                    bCashCard.Width = 134;
                    bCashCard.Height = 77;
                    bCashCard.BackColor = System.Drawing.Color.Orange;
                    bCashCard.Font = new Font("Verdana", 9, FontStyle.Bold);
                    bCashCard.Text = netsCashCardPayment;
                    bCashCard.Tag = netsCashCardPayment;
                    bCashCard.Click += delegate
                    {
                        btnMakePayment_Click(bCashCard, new EventArgs());
                    };
                    flowLayoutPanel1.Controls.Add(bCashCard);

                    continue;
                }

                Button b1 = new Button();
                b1.Width = 134;
                b1.Height = 77;
                b1.BackColor = System.Drawing.Color.Orange;
                b1.Font = new Font("Verdana", 9, FontStyle.Bold);
                b1.Text = PointOfSaleInfo.PaymentTypes.Rows[i][1].ToString();
                b1.Tag = PointOfSaleInfo.PaymentTypes.Rows[i][1].ToString();
                b1.Click += delegate
                {
                    btnMakePayment_Click(b1, new EventArgs());
                };
                flowLayoutPanel1.Controls.Add(b1);
            }

            if (enableNetsCreditCardTerminal)
            {
                Button bCreditCard = new Button();
                bCreditCard.Width = 134;
                bCreditCard.Height = 77;
                bCreditCard.BackColor = System.Drawing.Color.Orange;
                bCreditCard.Font = new Font("Verdana", 9, FontStyle.Bold);
                bCreditCard.Text = netsCreditCardPayment;
                bCreditCard.Tag = netsCreditCardPayment;
                bCreditCard.Click += delegate
                {
                    btnMakePayment_Click(bCreditCard, new EventArgs());
                };
                flowLayoutPanel1.Controls.Add(bCreditCard);
            }
        }
        
        private void btnMakePayment_Click(object sender, EventArgs e)
        {
            if (((Button)sender).Tag.ToString() == "CASH")
            {
                frmKeypad fKeypad = new frmKeypad();
                fKeypad.initialValue = amount.ToString("N2");
                fKeypad.ShowDialog();

                if (fKeypad.DialogResult == DialogResult.OK)
                {
                    decimal enteredAmount = 0;
                    if (decimal.TryParse(fKeypad.value, out enteredAmount))
                    {
                        if (enteredAmount < amount)
                        {
                            MessageBox.Show("Entered Amount less than payment amount");
                            return;
                        }
                        else
                        {
                            isSuccessful = true;
                            PaymentTypeSelected = ((Button)sender).Tag.ToString();
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please Enter Correct Amount");
                        return;
                    }
                }
                else
                {
                    isSuccessful = false;
                    this.Close();
                }
            }
            else
            {
                isSuccessful = true;
                PaymentTypeSelected = ((Button)sender).Tag.ToString();
                this.Close();
            }
            
        }
        
        private void frmSelectPayment_Load(object sender, EventArgs e)
        {
            isSuccessful = false;
            //this.lblRefNo.Text = pos.GetUnsavedRefNo().Substring(13);
            //lblNumOfItems.Text = pos.GetSumOfItemQuantity().ToString();
            LoadPaymentTypeLabels();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            isSuccessful = false;
            this.Close();
        }

        private void btnOtherPayment_Click(object sender, EventArgs e)
        {
          
        }
    }
}
