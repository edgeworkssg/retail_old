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

namespace PowerInventory
{
    public partial class frmAddItemGroup : Form
    {
        public int Qty;
        public int PromoCampaignHdrId;
        public string Barcode;

        public frmAddItemGroup()
        {
            InitializeComponent();
        }

        private void btnAddItems_Click(object sender, EventArgs e)
        {
            Qty = Int32.Parse(txtQty.Text.ToString().Trim());
            this.Close();
        }

        private void btnCancelAdd_Click(object sender, EventArgs e)
        {
            Qty = 0;
            this.Close();
        }

        private void frmAddPromoItem_Load(object sender, EventArgs e)
        {
            ItemGroupCollection promo = new ItemGroupCollection();
            promo.Where(ItemGroup.Columns.Barcode, Comparison.Equals, Barcode);
            promo.Load();

            if (promo.Count > 0)
            {
                lblPromoName.Text = promo[0].ItemGroupName;
                lblPromoCode.Text = promo[0].Barcode ?? "-";
                PromoCampaignHdrId = promo[0].ItemGroupId;

                ItemGroupMapCollection det = new ItemGroupMapCollection();
                det.Where(ItemGroupMap.Columns.ItemGroupID, Comparison.Equals, PromoCampaignHdrId);
                det.Where(ItemGroupMap.Columns.Deleted, Comparison.Equals, false);
                det.Load();

                if (det.Count > 0)
                {
                    lblQty.Text = det[0].UnitQty.ToString();
                }


            }
        }

        private void txtQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

    }
}
