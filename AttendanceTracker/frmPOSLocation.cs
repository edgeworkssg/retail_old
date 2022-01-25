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

namespace AttendanceTracker
{
    public partial class frmPOSLocation : Form
    {
        public frmPOSLocation()
        {
            InitializeComponent();
        }

        private void frmPOSLocation_Load(object sender, EventArgs e)
        {
            cmbPOS.DataSource = PointOfSaleController.FetchPointOfSaleNames();
            cmbPOS.Refresh();
            cmbPOS.SelectedItem = PointOfSaleInfo.PointOfSaleName;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {                        
             PointOfSale pr = new PointOfSale(PointOfSale.Columns.PointOfSaleName, cmbPOS.SelectedItem.ToString());
             if (!pr.IsNew)
             {
                 PointOfSaleController.SavePointOfSaleID(pr.PointOfSaleID);
                 PointOfSaleController.GetPointOfSaleInfo();
                 this.Close();
             }                        
        }
    }
}
