using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PowerPOS
{
    public partial class frmViewAppointment : Form
    {
        AppointmentManager app;
        public frmViewAppointment(object id)
        {
            InitializeComponent();
            app = new AppointmentManager(id);

        }

        private void frmView_Load(object sender, EventArgs e)
        {

            DateTime  date;
            DateTime.TryParse(app.AppointmentDate, out date);
            lblDate.Text = date.ToLongDateString();
            lblSalesPerson.Text = app.SalesPersonID;
            lblTime.Text = app.StartTime + " - " + app.EndTime;
            lblDesc.Text = app.Description;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
