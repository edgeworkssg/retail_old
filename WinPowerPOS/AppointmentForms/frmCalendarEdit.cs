using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PowerPOS;

namespace WinPowerPOS.AppointmentForms
{
    public partial class frmCalendarEdit : Form
    {
        private AppointmentController CommandoRoom = new AppointmentController();

        private UserMst SalesPerson;

        private int dgDeleteButton=0;
        private int dgEditButton = 0;
        private int dgEventID = 0;

        public frmCalendarEdit(string SalesPersonName)
        {
            InitializeComponent();

            #region *) Load Sales Person 
            /// If error, put null
            SalesPerson = new UserMst(SalesPersonName);
            if (SalesPerson.IsNew)
            {
                MessageBox.Show("Sales Person [" + SalesPersonName + "] is not found");
                SalesPerson = null;
            }
            #endregion
        }

        private void frmCalendarEdit_Load(object sender, EventArgs e)
        {
            Tabel.AutoGenerateColumns = false;

            #region Setting: Set dgvc Index [Not Important to be seen - Believe me :)]
            dgEditButton = Tabel.Columns[dgvbEdit.Name].Index;
            dgDeleteButton = Tabel.Columns[dgvbDelete.Name].Index;
            dgEventID = Tabel.Columns[dgvcID.Name].Index;
            #endregion

            #region *) Validation: No SalesPerson means error
            if (SalesPerson == null)
                this.Close();
            #endregion

            #region *) Load all Rooms 
            RoomController myRoomController = new RoomController();
            iRoom.DataSource = myRoomController.FetchAll_forListView();
            iRoom.DisplayMember = RoomController.listboxDisplayedColumnName;
            iRoom.ValueMember = RoomController.listboxValueColumnName;
            #endregion

            iTime.Text = DateTime.Now.ToString("HHmm");

            SetDisplayForNewRecord();
        }

        private void iAppointment_Validating(object sender, CancelEventArgs e)
        {
            DoTimeValidation();
        }
        private void DoTimeValidation()
        {
            //int MinutesToAdd = 0;
            //if (iAppointment.Value.Minute >= 30)
            //    MinutesToAdd = 30;

            //iAppointment.Value =
            //    iAppointment.Value.Date
            //    .AddHours(iAppointment.Value.Hour)
            //    .AddMinutes(MinutesToAdd);

            DataTable dtDuration = new DataTable();
            dtDuration.Columns.Add("Displayed", Type.GetType("System.String"));
            dtDuration.Columns.Add("Value", Type.GetType("System.Int32"));

            for (int Counter = 0; Counter < 48; Counter++)
            {
                string Desc = iAppointment.Value.AddMinutes(Counter * 30).ToString("dd MMMM yyyy - HH:mm");
                if (Counter < 2)
                    Desc += "  (" + (Counter * 30).ToString() + " mins)";
                else if (Counter == 2)
                    Desc += "  (1 hr)";
                else if (Counter % 2 == 0)
                    Desc += "  (" + (Counter / 2).ToString("N0") + " hrs)";
                else
                    Desc += "  (" + ((decimal)Counter / 2).ToString("N1") + " hrs)";

                dtDuration.Rows.Add(new object[] { Desc, Counter * 30 });
            }

            iDuration.DataSource = dtDuration;
            iDuration.DisplayMember = "Displayed";
            iDuration.ValueMember = "Value";
        }

        private void Tabel_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            #region *) Handle: Edit Button 
            if (e.ColumnIndex == dgEditButton)
            {
                SetDisplayForEditRecord(Tabel[dgEventID, e.RowIndex].Value.ToString());
            }
            #endregion
            #region *) Handle: Delete Button 
            else if (e.ColumnIndex == dgDeleteButton)
            {
                DeleteRecord(Tabel[dgEventID, e.RowIndex].Value.ToString());
            }
            #endregion
        }

        private void SetDisplayForNewRecord()
        {
            iEventID.Text = "";
            iAppointment.Value = DateTime.Now.AddMinutes(30);
            DoTimeValidation();
            if (iDuration.Items.Count > 0) iDuration.SelectedIndex = 0;
            if (iRoom.Items.Count > 0) iRoom.SelectedIndex = 0;
            iMembership.Text = "";
            lblMembershipInfo.Text = "[NONE]";
            lblMembershipInfo.Tag = null;
        }
        private void SetDisplayForEditRecord(string EventID)
        {
            try
            {
                Appointment selAppointment = CommandoRoom.RetrieveAppointment(EventID);

                iEventID.Text = selAppointment.ID;
                iAppointment.Value = selAppointment.AppointmentTime;
                DoTimeValidation();
                if (iDuration.Items.Count > 0) iDuration.SelectedIndex = (int)Math.Ceiling((double)selAppointment.Duration / 30);
                // if (iRoom.Items.Contains(selAppointment.RoomID))
                iRoom.SelectedValue = selAppointment.RoomID;
                if ((iRoom.SelectedItem == null)) iRoom.SelectedIndex = 0;
                iMembership.Text = "";
                Membership selMember = selAppointment.Membership;
                if (selMember == null || selMember.IsNew)
                {
                    lblMembershipInfo.Text = "[NONE]";
                    lblMembershipInfo.Tag = null;
                }
                else
                {
                    lblMembershipInfo.Text = selMember.MembershipNo + " - " + selMember.NameToAppear;
                    lblMembershipInfo.Tag = selMember;
                }
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
        }

        private void DeleteRecord(string EventID)
        {
            CommandoRoom.DeleteAppointment(EventID);
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            SetDisplayForNewRecord();
        }

        private void myCalendar_DateChanged(object sender, DateRangeEventArgs e)
        {
            iTime.Text = iTime.Text.PadLeft(4, '0');
            iAppointment.Value = myCalendar.SelectionStart.Date;
            iAppointment.Value = iAppointment.Value.AddHours(int.Parse(iTime.Text.Replace(":", "").Substring(0, 2))).AddMinutes(int.Parse(iTime.Text.Replace(":", "").Substring(2)));

            DoTimeValidation();

            #region *) Load all Appointments
            Tabel.DataSource = CommandoRoom.getAllAppointment(DateTime.Today, DateTime.Today.AddDays(1), SalesPerson.UserName).ToDataTable();
            #endregion
        }

        private void iTime_Validating(object sender, CancelEventArgs e)
        {
            iTime.Text = iTime.Text.Replace(":", "").PadLeft(4, '0');

            if (int.Parse(iTime.Text.Replace(":", "")) >= 2400 || int.Parse(iTime.Text.Replace(":", "")) % 100 > 59)
            {
                MessageBox.Show("Wrong time format. System only accept time from 00:00 until 23:59");
                e.Cancel = true;
            }
            else
            {
                iAppointment.Value = myCalendar.SelectionStart.Date;
                iAppointment.Value = iAppointment.Value.AddHours(int.Parse(iTime.Text.Replace(":", "").Substring(0, 2))).AddMinutes(int.Parse(iTime.Text.Replace(":", "").Substring(2)));

                DoTimeValidation();
            }
        }
    }
}
