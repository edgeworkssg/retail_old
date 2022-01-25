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
    public partial class frmViewItemGroupPromo : Form
    {
        public int PromoCampaignHdrID;
        public frmViewItemGroupPromo()
        {
           
            InitializeComponent();            
            dgvItems.AutoGenerateColumns = false;            
        }        

        
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private int ItemGroupID;
        private void frmProductSalesReport_Load(object sender, EventArgs e)
        {
            PromoCampaignHdr hdr = new PromoCampaignHdr(PromoCampaignHdrID);
            if (!hdr.IsNew)
            {
                txtCampaignName.Text = hdr.PromoCampaignName;
                dtpStartDate.Value = hdr.DateFrom;
                dtpEndDate.Value = hdr.DateTo;

                PromoCampaignDetCollection det = hdr.PromoCampaignDetRecords();
                if (hdr.PromoPrice.HasValue) 
                    txtPrice.Text = hdr.PromoPrice.Value.ToString("N2");
                if (det.Count > 0)
                {
                    txtBarcodeItemGroup.Text = det[0].ItemGroup.Barcode;
                    ViewItemGroupCollection v = new ViewItemGroupCollection();                    
                    v.Where(ViewItemGroup.Columns.ItemGroupID, det[0].ItemGroupID);
                    v.Load();
                    if (det[0].ItemGroupID.HasValue) ItemGroupID = det[0].ItemGroupID.Value;
                    dgvItems.DataSource = v;
                    dgvItems.Refresh();
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            decimal promoPrice = 0;
            if (!decimal.TryParse(txtPrice.Text, out promoPrice))
            {
                MessageBox.Show("Please specify valid promotion price");
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
            qr.AddUpdateSetting(PromoCampaignHdr.Columns.PromoPrice, promoPrice);
            qr.Execute();

            //update barcode
            qr = ItemGroup.CreateQuery();
            qr.QueryType = QueryType.Update;
            qr.AddWhere(ItemGroup.Columns.ItemGroupId, ItemGroupID);
            qr.AddUpdateSetting(ItemGroup.Columns.Barcode, txtBarcodeItemGroup.Text);
            qr.Execute();

            this.Close();            
        }        
    }
}
