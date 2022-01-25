using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PowerPOS;

namespace PowerPOS
{
    public partial class frmAddAppointment : Form
    {
        public frmAddAppointment()
        {
            InitializeComponent();
        }

        public frmAddAppointment(DateTime date)
        {
            InitializeComponent();
            dtpDate.MinDate = DateTime.Now;
            try
            {
                dtpDate.Value = date;
            }
            catch { dtpDate.Value = DateTime.Now; }

            
        }


        private void frmAdd_Load(object sender, EventArgs e)
        {
            UserMstCollection salesPersons = new UserMstCollection();
            salesPersons.Where(UserMst.Columns.IsASalesPerson, true);
            salesPersons.Load();

            cboSalesPersons.DataSource = salesPersons;
            cboSalesPersons.DisplayMember = UserMst.Columns.DisplayName;
            cboSalesPersons.ValueMember = UserMst.Columns.UserName;
            //for (int i = 0; i < salesPersons.Count; i++)
            //{
            //    DataGridViewColumn column = new DataGridViewColumn();
            //    column.Tag = salesPersons[i].UserName;
            //    column.HeaderText = salesPersons[i].DisplayName;
            //    column.CellTemplate = new DataGridViewTextBoxCell();
            //    cbo
            //}

            cboStart2.SelectedIndex = 0;
            cboEnd2.SelectedIndex = 0;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboStart1.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select start time.");
                    return;
                }
                if (cboEnd1.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select end time.");
                    return;
                }

                DateTime start;
                DateTime end;

                DateTime.TryParse(string.Format("{0}:{1}", cboStart1.Text, cboStart2.Text), out start);
                DateTime.TryParse(string.Format("{0}:{1}", cboEnd1.Text, cboEnd2.Text), out end);

                if (end < start)
                {
                    MessageBox.Show("End Time should not be less than Start Time.");
                    return;
                }

                if (end == start)
                {
                    MessageBox.Show("Start Time and End Time should not be equal.");
                    return;
                }
                if (txtDesc.Text.Trim() == "")
                {
                    MessageBox.Show("Description is required.");
                    return;
                }
                AppointmentManagerCollection apps = new AppointmentManagerCollection();
                apps.Where(AppointmentManager.Columns.AppointmentDate, dtpDate.Value).Where(AppointmentManager.Columns.SalesPersonID, cboSalesPersons.SelectedValue.ToString()).Load();

                if (apps != null && apps.Count != 0)
                {
                    for (int i = 0; i < apps.Count; i++)
                    {
                        DateTime tempStart;
                        DateTime tempEnd;

                        DateTime.TryParse(apps[i].StartTime, out tempStart);
                        DateTime.TryParse(apps[i].EndTime, out tempEnd);

                        if (start.TimeOfDay > tempStart.TimeOfDay && start.TimeOfDay < tempEnd.TimeOfDay)
                        {
                            MessageBox.Show(string.Format("Overlaps in the existing record for {0}, from {1} to {2}.", cboSalesPersons.SelectedValue.ToString(), start.ToString("hh:mm"), end.ToString("hh:mm")));
                            return;
                        }
                        if (end.TimeOfDay > tempStart.TimeOfDay && end.TimeOfDay < tempEnd.TimeOfDay)
                        {
                            MessageBox.Show(string.Format("Overlaps in the existing record for {0}, from {1} to {2}.", cboSalesPersons.SelectedValue.ToString(), start.ToString("hh:mm"), end.ToString("hh:mm")));
                            return;
                        }
                    }
                }


                AppointmentManager app = new AppointmentManager();
                app.AppointmentDate = dtpDate.Value.ToString();
                app.SalesPersonID = cboSalesPersons.SelectedValue.ToString();
                app.StartTime = string.Format("{0}:{1}", cboStart1.Text, cboStart2.Text);
                app.EndTime = string.Format("{0}:{1}", cboEnd1.Text, cboEnd2.Text);
                app.Description = txtDesc.Text.Trim();
                app.Save();

                this.Dispose();
                this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
