using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using PowerPOS.Container;

namespace WinPowerPOS.PromoAdmin
{
    public partial class frmPromotionList : Form
    {
        public frmPromotionList()
        {
            InitializeComponent();
        }

        private void frmPromotionList_Load(object sender, EventArgs e)
        {
            PointOfSale posInfo = new PointOfSale(PointOfSaleInfo.PointOfSaleID);
            LblOutlet.Text = posInfo.OutletName;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string search = txtSearch.Text ?? "%";
            string date = "";
            if (dtpDate.Checked)
                date = dtpDate.Text;
            PointOfSale posInfo = new PointOfSale(PointOfSaleInfo.PointOfSaleID);
            string orderBy = "PromoCampaignHdrID";
            string sortOrder = "ASC";
            DataTable dt = PromotionAdminController.SearchActivePromoWithOutlet(search, date, posInfo.OutletName, orderBy, sortOrder);
            dt = CommonUILib.DataTableConvertBoolColumnToYesOrNo(dt);

            dgvPromotionList.DataSource = dt;
            dgvPromotionList.Refresh();
        }

        private void dgvPromotionList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex >= 0)
            {
                frmViewPromotionDetails f = new frmViewPromotionDetails();
                f.PromoCampaignHdrID = Int32.Parse(dgvPromotionList.Rows[e.RowIndex].Cells[1].Value.ToString());
                CommonUILib.displayTransparent(); f.ShowDialog(); CommonUILib.hideTransparent();
            }

        }

        private void dgvPromotionList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (this.dgvPromotionList.Columns[e.ColumnIndex].Name == "ApplicableTo")
            {
                if (e.Value != null)
                {
                    // Check for the string "pink" in the cell.
                    string stringValue = (string)e.Value;
                    stringValue = stringValue.ToLower();
                    if ((stringValue.ToLower().Equals("yes")))
                    {
                        e.Value = "All";
                    }
                    else
                    {
                        e.Value = "Member";
                    }

                }
            }
        }
    }
}
