using PowerPOS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinPowerPOS.AppointmentForms
{
    public partial class frmCalendarMaster : Form
    {
        ArrayList entryList = new ArrayList();

        private AppointmentController CommandoRoom = new AppointmentController(); /*(^_^)*/

        #region -= Settings =-
        private const string Google_ApplicationName = "Edgeworks-Retail";
        private DayOfWeek FirstDayOfTheWeek = DayOfWeek.Sunday;
        #endregion

        public frmCalendarMaster()
        {
            InitializeComponent();
        }

        #region From GoogleCalendar Sample
        //private void calendarControl_DateSelected(object sender, System.Windows.Forms.DateRangeEventArgs e)
        //{

        //    this.DayEvents.Items.Clear();

        //    ArrayList results = new ArrayList(5);
        //    foreach (EventEntry entry in this.entryList)
        //    {
        //        // let's find the entries for that date

        //        if (entry.Times.Count > 0)
        //        {
        //            foreach (When w in entry.Times)
        //            {
        //                if (e.Start.Date == w.StartTime.Date ||
        //                    e.Start.Date == w.EndTime.Date)
        //                {
        //                    results.Add(entry);
        //                    break;
        //                }
        //            }
        //        }
        //    }

        //    foreach (EventEntry entry in results)
        //    {
        //        ListViewItem item = new ListViewItem(entry.Title.Text);
        //        item.SubItems.Add(entry.Authors[0].Name);
        //        if (entry.Times.Count > 0)
        //        {
        //            item.SubItems.Add(entry.Times[0].StartTime.TimeOfDay.ToString());
        //            item.SubItems.Add(entry.Times[0].EndTime.TimeOfDay.ToString());
        //        }

        //        this.DayEvents.Items.Add(item);
        //    }
        //}
        #endregion

        private void frmCalendarTest_Load(object sender, EventArgs e)
        {
            Tabel.AutoGenerateColumns = true;

            #region *) Load SalesPerson ComboBox 
            UserMstController myUserMstController = new UserMstController();
            tSalesPerson.DataSource = myUserMstController.FetchSalesPerson_forListView();
            tSalesPerson.DisplayMember = UserMstController.ListBoxColumns.DisplayedColumnName;
            tSalesPerson.ValueMember = UserMstController.ListBoxColumns.ValueColumnName;
            #endregion

            BindGrid();
        }

        private void rbWeekly_CheckedChanged(object sender, EventArgs e)
        {
            detWeekly.Enabled = rbWeekly.Checked;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        /// <remarks> 2 ToDo Lists </remarks>
        private void BindGrid()
        {
            lblSalesPerson.Text = "-";
            lblSalesPerson.Tag = null;

            // ToDo: Put Loading screen

            try
            {
                #region *) Clear DataGridView
                Tabel.DataSource = null;
                Tabel.Rows.Clear();
                Tabel.Columns.Clear();
                #endregion

                #region *) ReAssign Data
                DateTime SelectedDate = myCalendar.SelectionStart;
                string SelectedSalesPerson = tSalesPerson.SelectedValue.ToString();
                Tabel.DataSource = CommandoRoom.getAllAppointment(SelectedDate, SelectedDate.AddDays(6), SelectedSalesPerson)
                    .ToCalendarDataTable_showMemberName(SelectedDate, SelectedDate.AddDays(6), 7, 22, 30);
                #endregion

                #region *) ReFormat Column Header Text 
                for (int Counter = 1; Counter < Tabel.Columns.Count; Counter++)
                {
                    Tabel.Columns[Counter].HeaderText = DateTime.Parse(Tabel.Columns[Counter].HeaderText).ToString("MMM dd");
                }
                #endregion

                #region *) Prevent Tabel for Sorting
                for (int Counter = 0; Counter < Tabel.Columns.Count; Counter++)
                    Tabel.Columns[Counter].SortMode = DataGridViewColumnSortMode.NotSortable;
                #endregion

                lblSalesPerson.Text = tSalesPerson.SelectedText.ToString();
                lblSalesPerson.Tag = tSalesPerson.SelectedValue.ToString();
            }
            catch (Exception X)
            {
                if (X.Message.StartsWith("(error)"))
                {
                    MessageBox.Show(X.Message.Replace("(error)", ""), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (X.Message.StartsWith("(warning)"))
                {
                    MessageBox.Show(X.Message.Replace("(warning)", ""), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("Some error occured. Please contact your administrator", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Logger.writeLog(X);
                }
            }

            // ToDo: Remove Loading Screen
        }

        private void Tabel_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 1) return;

            if (lblSalesPerson.Tag == null) return;

            string selSalesPerson = lblSalesPerson.Tag.ToString();

            DateTime selTime = DateTime.Now;
            #region *) Retrieve selected Date/Time from Tabel
            if (!DateTime.TryParse(Tabel.Columns[e.ColumnIndex].DataPropertyName + " " + Tabel[0, e.RowIndex].Value.ToString(), out selTime))
            {
                MessageBox.Show("System cannot recognize [" + Tabel.Columns[e.ColumnIndex].DataPropertyName + " " + Tabel[0, e.RowIndex].Value.ToString() + "] as a Date/Time format"
                    , "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            #endregion

            using (frmCalendarEdit instance = new frmCalendarEdit(selSalesPerson/*, selTime, 30*/))
            {
                instance.ShowDialog();
                //MessageBox.Show(selTime.ToString("dd MMMM yyyy - HH:mm"));
            }
        }
    }
}
