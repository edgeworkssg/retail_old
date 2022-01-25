using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using SubSonic;

namespace WinPowerPOS.PromoAdmin
{
    public partial class frmListOfDiscountByItemGroup : Form
    {
        public frmListOfDiscountByItemGroup()
        {
            InitializeComponent();
            dgvCampaignList.AutoGenerateColumns = false;
        }


        private void frmListOfDiscountByItemGroup_Load(object sender, EventArgs e)
        {
            //BindGrid();
        }

        private void btnStartDate_Click(object sender, EventArgs e)
        {
            dgvCampaignList.DataSource =
                PromotionAdminController.FetchDiscountByItemGroupPromo
                (true, false, dtpFrom.Value, dtpTo.Value, 
                DateTime.MinValue,DateTime.MaxValue, "",0,0);
            dgvCampaignList.Refresh();
        }

        private void btnEndDate_Click(object sender, EventArgs e)
        {
            dgvCampaignList.DataSource =
                PromotionAdminController.FetchDiscountByItemGroupPromo
                (false, true, DateTime.MinValue, DateTime.MaxValue,
                dtpFrom.Value, dtpTo.Value,"",0,0);
            dgvCampaignList.Refresh();
        }

        private void btnName_Click(object sender, EventArgs e)
        {
            dgvCampaignList.DataSource =
                   PromotionAdminController.FetchDiscountByItemGroupPromo
                   (false, false, dtpFrom.Value, dtpTo.Value,
                   DateTime.MinValue, DateTime.MaxValue, txtMisc.Text,0,0);
            dgvCampaignList.Refresh();
        }

        private void btnDiscount_Click(object sender, EventArgs e)
        {
            decimal disc;
            if (decimal.TryParse(txtMisc.Text, out disc))
            {
                dgvCampaignList.DataSource =
                                PromotionAdminController.FetchDiscountByItemGroupPromo
                                (false, false, dtpFrom.Value, dtpTo.Value,
                                DateTime.MinValue, DateTime.MaxValue, "", disc,0);
                dgvCampaignList.Refresh();
            }
            else
            {
                MessageBox.Show("Please specify correct discount in number format");
                txtMisc.Select();
            }
        }

        private void btnPrice_Click(object sender, EventArgs e)
        {
            decimal price;
            if (decimal.TryParse(txtMisc.Text, out price))
            {
                dgvCampaignList.DataSource =
                                PromotionAdminController.FetchDiscountByItemGroupPromo
                                (false, false, dtpFrom.Value, dtpTo.Value,
                                DateTime.MinValue, DateTime.MaxValue, "", 0, price);
                dgvCampaignList.Refresh();
            }
            else
            {
                MessageBox.Show("Please specify correct discount in number format");
                txtMisc.Select();
            }

        }

        private void dgvCampaignList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == 0)
            {
                //
                //Warning
                DialogResult dr = MessageBox.Show("This action cant be undone. Are you sure you want to proceed?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dr == DialogResult.Yes)
                {
                    //execute delete command.....
                    int promoCampaignHdrID = (int)dgvCampaignList.Rows[e.RowIndex].Cells["PromoCampaignHdrID"].Value;
                    PromotionAdminController.EnablePromo(promoCampaignHdrID, false);
                    
                    int itemGroupID = (int)dgvCampaignList.Rows[e.RowIndex].Cells["ItemGroupID"].Value;
                    
                    //mark item group as deleted ....
                    Query qr = ItemGroup.CreateQuery();
                    qr.QueryType = QueryType.Update;
                    qr.AddUpdateSetting("Deleted", true);
                    qr.AddWhere(ItemGroup.Columns.ItemGroupId, itemGroupID);
                    qr.Execute();

                    //Delete the row.
                    dgvCampaignList.Rows.RemoveAt(e.RowIndex);
                }
            }
            else if (e.ColumnIndex == 7)
            {
                frmPromoLocation f = new frmPromoLocation();
                f.promoCampaignHdrID = int.Parse(dgvCampaignList.Rows[e.RowIndex].Cells["PromoCampaignHdrID"].Value.ToString());
                f.ShowDialog();
                f.Dispose();
            }
                
            else 
            {
                frmViewItemGroupPromo f = new frmViewItemGroupPromo();
                f.PromoCampaignHdrID = (int)dgvCampaignList.Rows[e.RowIndex].Cells["PromoCampaignHdrID"].Value;
                f.ShowDialog();
                f.Dispose();
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            frmGenerateItemGroupPromo f = new frmGenerateItemGroupPromo();
            f.ShowDialog();
            f.Dispose();
        }

    }
}
