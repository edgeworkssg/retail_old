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
    public partial class frmListOfDiscountByItem : Form
    {
        public frmListOfDiscountByItem()
        {
            InitializeComponent();
            dgvCampaignList.AutoGenerateColumns = false;
        }


        private void frmListOfDiscountByItem_Load(object sender, EventArgs e)
        {
            //BindGrid();
        }

        private void btnStartDate_Click(object sender, EventArgs e)
        {
            dgvCampaignList.DataSource =
                PromotionAdminController.FetchDiscountByItemPromo
                (true, false, dtpFrom.Value, dtpTo.Value, DateTime.MinValue,DateTime.MaxValue, "", 0);

            dgvCampaignList.Refresh();
        }

        private void btnEndDate_Click(object sender, EventArgs e)
        {
            dgvCampaignList.DataSource =
                PromotionAdminController.FetchDiscountByItemPromo
                (false, true, DateTime.MinValue, DateTime.MaxValue, 
                dtpFrom.Value, dtpTo.Value, "", 0);

            dgvCampaignList.Refresh();
        }

        private void btnName_Click(object sender, EventArgs e)
        {
            dgvCampaignList.DataSource =
                PromotionAdminController.FetchDiscountByItemPromo
                (false, false, DateTime.MinValue, DateTime.MaxValue,
                DateTime.MinValue, DateTime.MaxValue, txtMisc.Text, 0);

            dgvCampaignList.Refresh();
        }

        private void btnDiscount_Click(object sender, EventArgs e)
        {
            decimal disc;
            if (decimal.TryParse(txtMisc.Text, out disc))
            {
                dgvCampaignList.DataSource =
                    PromotionAdminController.FetchDiscountByItemPromo
                    (false, false, DateTime.MinValue, DateTime.MaxValue,
                    DateTime.MinValue, DateTime.MaxValue, "", disc);

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
            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex == 0)
                {
                    //Warning
                    DialogResult dr = MessageBox.Show("This action cant be undone. Are you sure you want to proceed?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dr == DialogResult.Yes)
                    {
                        //execute delete command.....
                        PromoCampaignDet.Destroy(dgvCampaignList.Rows[e.RowIndex].Cells["PromoCampaignDetID"].Value);

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
                    frmViewDiscountByItemPromo f = new frmViewDiscountByItemPromo();
                    f.PromoCampaignHdrID = (int)dgvCampaignList.Rows[e.RowIndex].Cells["PromoCampaignHdrID"].Value;
                    f.ShowDialog();
                    f.Dispose();
                }
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            frmGenerateDiscount f = new frmGenerateDiscount();
            f.ShowDialog();
            f.Dispose();
        }

    }
}
