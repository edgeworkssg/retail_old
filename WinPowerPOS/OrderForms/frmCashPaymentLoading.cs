using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using System.Threading;
namespace WinPowerPOS.OrderForms
{
    public partial class frmCashPaymentLoading : Form
    {

        public bool isSuccessful;
        public decimal Amount;
        //public CashRecycler cashRecycler;
        public decimal AmountReceived;
        public decimal changeAmount;
        public POSController pos;

        public frmCashPaymentLoading(string instruction)
        {
            InitializeComponent(instruction);
        }

        private void frmLoading_Load(object sender, EventArgs e)
        {
            pnlProgress.Visible = true;
        }

        private void frmCashPaymentLoading_Load(object sender, EventArgs e)
        {
            /*isSuccessful = false;
            string status = "";
            //try to connect one more time to reconnect 
            try 
            {
                if (!cashRecycler.getStatus())
                {
                    cashRecycler.init(out status);
                    cashRecycler.connect(out status);
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex.Message);
                MessageBox.Show("Cash Recycler Machine is down. Please check your connection.");
                isSuccessful = false;
                this.Close();
            }
            //check if status is up
            if (!cashRecycler.getStatus())
            {
                MessageBox.Show("Cash Recycler Machine is down. Please check your connection.");
                isSuccessful = false;
                this.Close();
            }

            //continue and start deposit if connected
            //int amount = Convert.ToInt32(Amount * 100);
            try
            {
                cashRecycler.startDeposit(Amount, out status);
                lblAmount.Text = "$ " + Amount.ToString("N2");
                lblReceived.Text = "$ " + cashRecycler.getDepositedAmount().ToString("N2");
                bwDeposit.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error doing deposit. " + ex.Message);
                isSuccessful = false;
                this.Close();
            }
            */
        }

        private void bwDeposit_DoWork(object sender, DoWorkEventArgs e)
        {
            /*while (true)
            {
                if (cashRecycler.getDepositedAmount() >= Amount)
                {
                    break;
                }
                Thread.Sleep(1500);
            }*/
        }

        private void bwDeposit_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            /*string status = "";
            AmountReceived = cashRecycler.getDepositedAmount();
            cashRecycler.stopDeposit(out status);
            isSuccessful = pos.AddReceiptLinePayment(AmountReceived, POSController.PAY_CASH, "", 0, "", 0, out changeAmount, out status);
            bool IsPointAllocationSuccess;
            if (!pos.ConfirmOrder(true, out IsPointAllocationSuccess, out status))
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
                MessageBox.Show(status, "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                isSuccessful = false;
                this.Close();
            }
            this.Close();*/
        }

        

    }
}
