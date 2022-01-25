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
using SubSonic;

namespace WinPowerPOS.PromoAdmin
{
    public partial class frmViewPromotionDetails : Form
    {
        public int PromoCampaignHdrID;
        public frmViewPromotionDetails()
        {
            InitializeComponent();
        }

        private void frmViewPromotion_Load(object sender, EventArgs e)
        {
            PromoCampaignHdr header = new PromoCampaignHdr(PromoCampaignHdrID);
            PointOfSale posInfo = new PointOfSale(PointOfSaleInfo.PointOfSaleID);

            lblPromoCode.Text = header.PromoCode;
            lblPromoName.Text = header.PromoCampaignName;
            lblBarcode.Text = header.Barcode;
            lblOutlet.Text = posInfo.OutletName;
            lblDateFrom.Text = header.DateFrom.ToString("dddd, dd MMMM yyyy");
            lblDateTo.Text = header.DateTo.ToString("dddd, dd MMMM yyyy");
            if ((bool)header.ForNonMembersAlso)
            {
                rbAll.Checked = true;
            }
            else {
                rbMemberOnly.Checked = true;
            }

            if (header.IsRestricHour == true)
            {
                cbIsRestrictHour.Checked = true;
                lblRestrictHourStart.Text = header.RestrictHourStart.Value.ToString("hh:mm tt");

                lblRestrictHourEnd.Enabled = true;
                lblRestrictHourEnd.Text = header.RestrictHourEnd.Value.ToString("hh:mm tt");
            }

            PromoDaysMapCollection dayscol = new PromoDaysMapCollection();
            dayscol.Where(PromoDaysMap.Columns.PromoCampaignHdrID, Comparison.Equals, PromoCampaignHdrID);
            dayscol.Where(PromoDaysMap.Columns.Deleted, Comparison.Equals, false);
            dayscol.Load();

            if (dayscol.Count() > 0)
            {
                foreach (PromoDaysMap d in dayscol)
                {
                    switch (d.DaysPromo)
                    {
                        case "Monday": cbDays.SetItemCheckState(0,CheckState.Checked); break;
                        case "Tuesday": cbDays.SetItemCheckState(1, CheckState.Checked); break;
                        case "Wednesday": cbDays.SetItemCheckState(2, CheckState.Checked); break;
                        case "Thursday": cbDays.SetItemCheckState(3, CheckState.Checked); break;
                        case "Friday": cbDays.SetItemCheckState(4, CheckState.Checked); break;
                        case "Saturday": cbDays.SetItemCheckState(5, CheckState.Checked); break;
                        case "Sunday": cbDays.SetItemCheckState(6, CheckState.Checked); break;
                    }
                }
            }

            /*details*/
            PromoCampaignDetCollection col = new PromoCampaignDetCollection();
            col.Where(PromoCampaignDet.Columns.PromoCampaignHdrID, Comparison.Equals, PromoCampaignHdrID);
            col.Where(PromoCampaignDet.Columns.Deleted, Comparison.Equals, false);
            col.Load();

            DataTable dt = new DataTable();
            dt = new DataTable("PromoCampaignDet");
            dt.Columns.Add("ItemNo");
            dt.Columns.Add("ItemName");
            dt.Columns.Add("UnitQty");
            dt.Columns.Add("AnyQty");
            dt.Columns.Add("CategoryName");
            dt.Columns.Add("RetailPrice");
            dt.Columns.Add("PromoPrice");
            dt.Columns.Add("DiscPercent");
            dt.Columns.Add("DiscAmount");
            
            foreach (PromoCampaignDet d in col)
            {
                decimal UnitPrice = 0;
                string ItemName = "";
                string ItemNo = "";

                if (d.ItemGroupID != null && d.ItemGroupID != 0)
                {
                    ItemGroup ig = new ItemGroup(d.ItemGroupID);
                    if (ig != null)
                    {
                        ItemName = ig.ItemGroupName;
                    }
                }
                else
                {
                    if (d.ItemNo != null && d.ItemNo != "")
                    {
                        Item item = new Item(d.ItemNo);
                        ItemNo = d.ItemNo;
                        if (item != null)
                        {
                            ItemName = item.ItemName;
                            UnitPrice = item.RetailPrice;
                        }
                    }
                    else
                    {
                        ItemNo = "*";
                        string query = "Select ISNULL(MAX(RetailPrice),0) As RetailPrice " +
                                        "from Item where CategoryName = '" + d.CategoryName + "'";
                        QueryCommand cmd = new QueryCommand(query, "PowerPOS");
                        DataTable dq = new DataTable();
                        dq.Load(DataService.GetReader(cmd));

                        if (dq != null && dq.Rows.Count > 0)
                        {
                            ItemName = "*";
                            UnitPrice = Decimal.Parse(dq.Rows[0][0].ToString());
                        }

                    }
                }

                string minQuantity = d.MinQuantity == -1 ? "" : d.MinQuantity.ToString();
                decimal retailPrice = (int)d.UnitQty * UnitPrice;
                dt.Rows.Add(ItemNo, ItemName, d.UnitQty, d.AnyQty, d.CategoryName, retailPrice.ToString("N2"), Convert.ToDecimal(d.PromoPrice).ToString("N2"),  Convert.ToDecimal(d.DiscPercent).ToString("N2"), Convert.ToDecimal( d.DiscAmount).ToString("N2"));
            }

            dgvDetails.DataSource = dt;
            dgvDetails.Refresh();
            dgvDetails.ReadOnly = true;


        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
