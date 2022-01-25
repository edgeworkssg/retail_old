using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PowerPOS.Container;
using System.Collections;
using PowerPOSReports;
using PowerPOS;
using WinPowerPOS.OrderForms;
using SubSonic;

namespace WinPowerPOS.PromoAdmin
{
    public partial class frmViewDiscountByItemPromo : Form
    {
        public int PromoCampaignHdrID;
        public frmViewDiscountByItemPromo()
        {
           
            InitializeComponent();            
            dgvItems.AutoGenerateColumns = false;            
        }        

        
        private void btnClose_Click(object sender, EventArgs e)
        {
            
            this.Close();
        }
        
        private void frmProductSalesReport_Load(object sender, EventArgs e)
        {
            PromoCampaignHdr hdr = new PromoCampaignHdr(PromoCampaignHdrID);
            if (!hdr.IsNew)
            {
                txtCampaignName.Text = hdr.PromoCampaignName;
                dtpStartDate.Value = hdr.DateFrom;
                dtpEndDate.Value = hdr.DateTo;

                //PromoCampaignDetCollection det = hdr.PromoCampaignDetRecords();
                ViewPromotionsByItemCollection det = new ViewPromotionsByItemCollection();
                det.Where("PromoCampaignHdrID", PromoCampaignHdrID);
                det.Load();
                txtDiscount.Text = hdr.PromoDiscount.ToString("N2");
                if (det.Count > 0)
                {                                     
                    dgvItems.DataSource = det.ToDataTable();
                    dgvItems.Refresh();
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            decimal promoDisc = 0;
            if (!decimal.TryParse(txtDiscount.Text, out promoDisc))
            {
                MessageBox.Show("Please specify valid promotion discount");
                txtPrice.Focus();
                return;
            }

            Query qr = PromoCampaignHdr.CreateQuery();
            qr.QueryType = QueryType.Update;

            qr.AddWhere(PromoCampaignHdr.Columns.PromoCampaignHdrID, PromoCampaignHdrID);

            //update start date and end date
            //update price
            qr.AddUpdateSetting(PromoCampaignHdr.Columns.DateFrom, dtpStartDate.Value);
            qr.AddUpdateSetting(PromoCampaignHdr.Columns.DateTo, dtpEndDate.Value);
            qr.AddUpdateSetting(PromoCampaignHdr.Columns.PromoDiscount, promoDisc);
            qr.Execute();

            this.Close();            
        }        
    }
}
