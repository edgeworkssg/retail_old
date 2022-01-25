using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using WinPowerPOS.MembershipForms;
using System.Configuration;
using Features = PowerPOS.Feature;

namespace WinPowerPOS.OrderForms
{
    public partial class frmBreakPointValue : Form
    {
        ItemController obj = new ItemController();
        public string ItemNo;
        public string ItemName;
        public frmBreakPointValue()
        {
            InitializeComponent();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            Decimal BreakPointPrice = Convert.ToDecimal(this.txtBreakPointPrice.Text);
            Decimal GetPoint = Convert.ToDecimal(this.txtGetpoint.Text);            
            Decimal Output = 0;
            String Status = "";
            ////Update this function in Server side database through webservice
            Features.Package.UpdItemPointBreak(GetPoint, BreakPointPrice, ItemNo, out Output, out Status);
            /// Update this function in Clinet side database
            obj.UpDateItemPoint(GetPoint, BreakPointPrice, ItemNo);
            this.Close();
        }        
        private void frmBreakPointValue_Load(object sender, EventArgs e)
        {
            this.lblItemName.Text = ItemName;
        }
    }
}
