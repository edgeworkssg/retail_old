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
using System.Configuration;
using POSDevices;

namespace WinPowerPOS.Reports
{
    public partial class frmZ2Closing : Form
    {
        public frmZ2Closing()
        {
            InitializeComponent();
            dgvList.AutoGenerateColumns = false;
        }

        private void frmZ2Closing_Load(object sender, EventArgs e)
        {
            lblPOS.Text = PointOfSaleInfo.PointOfSaleName;
            Z2ClosingLog total;
            CounterCloseLogCollection coll = Z2ClosingLogController.GetSettlementForZ2Closings(PointOfSaleInfo.PointOfSaleID, out total);
            dgvList.DataSource = coll;
            dgvList.Refresh();
            if (total != null)
            {
                lblStartTime.Text = total.StartTime.ToString("dd MMM yyyy HH:mm");
                lblEndTime.Text = total.EndTime.ToString("dd MMM yyyy HH:mm");
                lblTotalCollected.Text = "$" + total.TotalActualCollected.ToString("N2");
                lblTotalRecorded.Text = "$" + total.TotalSystemRecorded.ToString("N2");
                lblVariance.Text = "$" + total.Variance.ToString("N2");
            } 
        }

        private void btnZ2Closing_Click(object sender, EventArgs e)
        {
            if (dgvList.Rows.Count == 0)
            {
                MessageBox.Show("Need to have one or more Closing to do Z2 Closing ");
                return;
            }
            Z2ClosingLog z2Log;
            if (Z2ClosingLogController.PerformZ2Closing(PointOfSaleInfo.PointOfSaleID, out z2Log))
            {
                POSDevices.POSDeviceController.PrintZ2Report(z2Log);

                this.Close();
            }
            else
            {
                MessageBox.Show("Error encountered in closing. Please contact your administrator.");
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 6)
            {
                bool PrintProductSalesReport = false;
                PrintProductSalesReport = (AppSetting.GetSettingFromDBAndConfigFile("PrintProductSalesReport") != null && AppSetting.GetSettingFromDBAndConfigFile("PrintProductSalesReport").ToString().ToUpper() == "YES");
                bool printDiscount = (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Print.PrintDiscountOnCounterCloseReport), false));
                POSDeviceController.PrintCloseCounterReport(new CounterCloseLog(CounterCloseLog.Columns.CounterCloseID, dgvList.Rows[e.RowIndex].Cells[5].Value.ToString()), PrintProductSalesReport, printDiscount);
            }
            else if (e.ColumnIndex == 7)
            {
                //get Counter Close Log Object
                CounterCloseLog c = new CounterCloseLog(CounterCloseLog.Columns.CounterCloseID, dgvList.Rows[e.RowIndex].Cells[5].Value.ToString());

                POSDevices.POSDeviceController.PrintEJCloseCounterReport(c);
            }
        }
    }
}
