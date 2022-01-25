using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SubSonic;
using PowerPOS;

namespace WinPowerPOS.AppointmentForms
{
    public partial class frmRoomListing : Form
    {
        public frmRoomListing()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmRoomListing_Load(object sender, EventArgs e)
        {
            dateControl.Day = DateTime.Now;
            BindGrid(dateControl.Day);
        }

        private void BindGrid(DateTime Day)
        {
            dgvPreview.DataSource = ResourceController.GetRoomListing(Day);
            dgvPreview.Refresh();

        }

        private void dateControl_Changed(object sender, EventArgs e)
        {
            BindGrid(dateControl.Day);
        }
    }
}
