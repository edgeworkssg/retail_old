using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PowerPOS.Container;
using PowerPOS;
using Features = PowerPOS.Feature;

namespace WinPowerPOS.OrderForms
{
    public partial class frmInsertPurchaseOrder : Form
    {

        DataTable dt = new DataTable();
        public PowerPOS.POSController pos;
        //PowerPOS.DateBlockingController DBlock;
        public bool IsSuccessful;

        public frmInsertPurchaseOrder()
        {
            InitializeComponent();
            //cmbType.Items.Add("---Please Select---");
            string status;
            
        }

        private void frmTrackDelivery_Load(object sender, EventArgs e)
        {

            try
            {
                
            }
            catch (Exception X)
            {
                if (X.Message.StartsWith("(warning)"))
                {
                    MessageBox.Show(X.Message.Replace("(warning)", ""), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (X.Message.StartsWith("(error)"))
                {
                    MessageBox.Show(X.Message.Replace("(error)", ""), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    PowerPOS.Logger.writeLog(X);
                    MessageBox.Show("Some error occurred. Please contact your administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        

        private void btnDelivery_Click(object sender, EventArgs e)
        {
            if (pos.AddPurchaseOrderNo(txtPurchaseOrder.Text))
            {
                IsSuccessful = true;
                this.Close();
            }

        }       

        private void btnCancel_Click(object sender, EventArgs e)
        {
            IsSuccessful = false;
            this.Close();
        }
              

    }
}
