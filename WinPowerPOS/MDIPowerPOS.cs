using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using PowerPOS.Container;
using System.Threading;
using System.Globalization;
using System.Resources;
using WinPowerPOS.OrderForms;
using WinPowerPOS.MembershipForm;
using WinPowerPOS.Delivery;
using WinPowerPOS.WarrantyForms;
//using WinPowerPOS.ClassAttendance;

namespace WinPowerPOS
{
    public partial class MDIPowerPOS : Form
    {
        public string orderHdrId;
        public string membershipNo;
        public POSController pos;

        //Forms
        frmCreateWarranty warrantyForm;
        frmDeliverySetup deliveryForm;
        //frmClassAttendance classForm;
        
        public MDIPowerPOS()
        {
            InitializeComponent();
            hasWarranty = false;
        }                

        private void MDIPowerPOS_MdiChildActivate(object sender, EventArgs e)
        {
            MdiChildren[0].Left = 0;
            MdiChildren[0].Top = 0;
            MdiChildren[0].Height = this.Height - 172;
            MdiChildren[0].Width = this.Width -100;
        }

        private void tslWarranty_Click(object sender, EventArgs e)
        {
            if (warrantyForm == null || warrantyForm.IsDisposed)
            {
                // Create a new instance of the child form.
                warrantyForm = new frmCreateWarranty();
                // Make it a child of this MDI form before showing it.
                warrantyForm.MdiParent = this;
                warrantyForm.orderHdrID = orderHdrId;
                warrantyForm.membershipNo = membershipNo;
                warrantyForm.WindowState = FormWindowState.Maximized;
                warrantyForm.Show();
            }
            else
            {
                warrantyForm.BringToFront();
            }
        }

        private void tslDelivery_Click(object sender, EventArgs e)
        {
            if (deliveryForm == null || deliveryForm.IsDisposed)
            {
                // Create a new instance of the child form.
                deliveryForm = new frmDeliverySetup();
                // Make it a child of this MDI form before showing it.
                deliveryForm.MdiParent = this;
                deliveryForm.BringToFront();
                deliveryForm.pos = pos;
                deliveryForm.orderHdrID = orderHdrId;
                deliveryForm.membershipNo = membershipNo;                
                deliveryForm.WindowState = FormWindowState.Maximized;
                deliveryForm.Show();
            }
            else
            {
                deliveryForm.BringToFront();
            }
        }

        private void tlsClasses_Click(object sender, EventArgs e)
        {

        }
        public bool hasWarranty;
        public bool hasDelivery;

        private void MDIPowerPOS_Load(object sender, EventArgs e)
        {                       

            if (!hasDelivery) tslDelivery.Enabled = false;
            if (!hasWarranty) tslWarranty.Enabled = false;
            
            if (hasDelivery)
                tslDelivery_Click(this, new EventArgs());
            if (hasWarranty)
                tslWarranty_Click(this, new EventArgs());
        }        
    }
}