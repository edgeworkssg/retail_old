using System;
using System.Windows.Forms;
using PowerPOS;

namespace WinPowerPOS.OrderForms
{
    /// <summary>
    /// This form will ask User to choose between commiting the sales as normal sales or delivery. 
    /// - Normal Sales option will put mark in OrderDet.InventoryHdrRefNo as empty string 
    /// - Delivery option will put mark in OrderDet.InventoryHdrRefNo as 'DELIVERY' 
    /// - Cancel / Closing the form will return Dialog Result as DialogResult.Cancel 
    /// </summary>
    public partial class frmSelectPrintTemplate : Form
    {
        public POSController pos;

        /// <summary>
        /// This form will ask User to choose between commiting the sales as normal sales or delivery. 
        /// - Normal Sales option will put mark in OrderDet.InventoryHdrRefNo as empty string 
        /// - Delivery option will put mark in OrderDet.InventoryHdrRefNo as 'DELIVERY' 
        /// - Cancel / Closing the form will return Dialog Result as DialogResult.Cancel 
        /// </summary>
        /// <param name="pos">Active POSController</param>
        public frmSelectPrintTemplate(POSController pos)
        {
            InitializeComponent();

            this.pos = pos;
        }

        private void frmSelectPrintTemplate_Load(object sender, EventArgs e)
        {
            btnDelivery.Enabled = true;
            lblDeliveryWarning.Text = "";

            if (!pos.MembershipApplied() || pos.GetMemberInfo().MembershipNo.ToLower() == "walk-in")
            {
                btnDelivery.Enabled = false;
                lblDeliveryWarning.Text = "Notes: You can choose delivery by assigning member into the sales";
            }
            else
            {
                bool AnyError = false;

                OrderDetCollection MyOrderDet = pos.myOrderDet;
                for (int Counter = 0; Counter < MyOrderDet.Count; Counter++)
                    if (MyOrderDet[Counter].Quantity < 0)
                    { AnyError = true; break; }

                if (AnyError)
                {
                    btnDelivery.Enabled = false;
                    lblDeliveryWarning.Text = "Notes: You cannot choose delivery if there are sales with negative quantity";
                }
            }
        }

        private void btnSales_Click(object sender, EventArgs e)
        {
            pos.SetAsDelivery(false);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnDelivery_Click(object sender, EventArgs e)
        {
            pos.SetAsDelivery(true);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
