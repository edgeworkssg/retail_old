using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PowerInventory
{
    public partial class frmConfirmStockTakeDateTime : Form
    {
        public DateTime stockTakeDate;
        public frmConfirmStockTakeDateTime()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //validation
            int hour = 0; int minute = 0;
            if (!int.TryParse(txtHH.Text, out hour) || !int.TryParse(txtMM.Text, out minute))
            {
                MessageBox.Show("Please specify valid input for the hour and minutes");
                return;
            }
            if (hour < 0 || hour > 24)
            {
                MessageBox.Show("Wrong hour specified, hour must be between 00 to 23");
                return;
            }
            if (minute < 0 || minute > 59)
            {
                MessageBox.Show("Wrong minute specified, minute must be between 00 to 59");
                return;
            }

            DialogResult dr = MessageBox.Show("Are you sure you want to confirm?", "", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                stockTakeDate = dtpInventoryDate.Value.Date;
                stockTakeDate = stockTakeDate.AddHours(hour);
                stockTakeDate = stockTakeDate.AddMinutes(minute);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void frmConfirmStockTakeDateTime_Load(object sender, EventArgs e)
        {

            dtpInventoryDate.Value = stockTakeDate;
            txtHH.Text = stockTakeDate.Hour.ToString();
            txtMM.Text = stockTakeDate.Minute.ToString();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
