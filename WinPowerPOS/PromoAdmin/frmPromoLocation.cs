using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PowerPOS;

namespace WinPowerPOS.PromoAdmin
{
    public partial class frmPromoLocation : Form
    {
        public int promoCampaignHdrID;
        public frmPromoLocation()
        {
            InitializeComponent();
            dgvPOS.AutoGenerateColumns = false;
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            /*
            bool hasTicked = false;
            for (int i = 0; i < dgvPOS.Rows.Count; i++)
            {
                if (dgvPOS.Rows[i].Cells[0].Value is bool
                    && (bool)dgvPOS.Rows[i].Cells[0].Value == true)
                {
                    hasTicked = true;
                }
            }
            if (!hasTicked)
            {
                MessageBox.Show("There is no location selected. Select the point of sale you wish to assign the location");
                return;
            }*/
            int posID;
            //Check if any checkbox checked....
            for (int i = 0; i < dgvPOS.Rows.Count; i++)
            {
                posID = (int)dgvPOS.Rows[i].Cells["PointOfSaleID"].Value;
                if (dgvPOS.Rows[i].Cells[0].Value is bool
                    && (bool)dgvPOS.Rows[i].Cells[0].Value == true)
                {
                    
                    PromotionAdminController.AddPromoLocationMap(promoCampaignHdrID, posID);
                }
                else
                {

                    //mark as deleted if any
                    
                    PromotionAdminController.DeletePromoLocationMap(promoCampaignHdrID, posID);
                }
            }
            
            MessageBox.Show("Promo Location is set successfully");

            return;
        }

        private void frmPromoLocation_Load(object sender, EventArgs e)
        {
            //Load Promo Info....
            PromoCampaignHdr p = new PromoCampaignHdr(promoCampaignHdrID);
            if (p.IsLoaded && !p.IsNew)
            {
                lblPromo.Text = p.PromoCampaignName;

                //Load Point Of Sale....
                PointOfSaleCollection t = new PointOfSaleCollection();
                t.Where(PointOfSale.Columns.Deleted, false);
                t.Load();

                dgvPOS.DataSource = t.ToDataTable();
                dgvPOS.Refresh();

                PromoLocationMapCollection plMap = new PromoLocationMapCollection();
                plMap.Where(PromoLocationMap.Columns.PromoCampaignHdrID, promoCampaignHdrID);
                plMap.Where(PromoLocationMap.Columns.Deleted, false);
                plMap.Load();

                for (int i = 0; i < plMap.Count; i++)
                {
                    for (int j = 0; j < dgvPOS.Rows.Count; j++)
                    {
                        if (dgvPOS.Rows[j].Cells["PointOfSaleID"].Value.ToString() == plMap[i].PointOfSaleID.ToString())
                        {
                            dgvPOS.Rows[j].Cells[0].Value = true;
                        }
                    }
                }
            }            
        }

        private void dgvPOS_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == 0)
            {
                if (dgvPOS.Rows[e.RowIndex].Cells[0].Value == null
                    || dgvPOS.Rows[e.RowIndex].Cells[0].Value.ToString().ToLower() == "false")
                {
                    dgvPOS.Rows[e.RowIndex].Cells[0].Value = true;
                }
                else
                {
                    dgvPOS.Rows[e.RowIndex].Cells[0].Value = false;
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
