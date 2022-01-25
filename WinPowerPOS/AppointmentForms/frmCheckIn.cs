using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using PowerPOS.AppointmentRealTimeController;
using SubSonic;
using AppointmentBook.Model;
using PowerPOS.Container;

namespace WinPowerPOS.AppointmentForms
{
    public partial class frmCheckIn : Form
    {
        public BackgroundWorker SyncAppointmentThread;

        private string ResourceID;
        public bool RoomStatus = false;
        public bool IsSuccessful = false;

        public frmCheckIn()
        {
            InitializeComponent();
        }

        public frmCheckIn(String ResourceID)
        {
            InitializeComponent();
            lblDate.Text = DateTime.Now.ToString("dd MMMM yyyy");
            this.ResourceID = ResourceID;
            BindGrid();
        }

        private void BindGrid()
        {
            Resource rs = new Resource(ResourceID);

            if (rs != null)
            {
                lblPax.Text = rs.Capacity.ToString();

                string query = "select a.Id as AppointmentId, m.NameToAppear, r.ResourceName, a.StartTime,  isnull(u.DisplayName,'') as UserName, " +
                               "     case ISNULL(a.CheckInByWho,'') when '' then 'No' else 'Yes' end as CheckIn, " +
                               "     case ISNULL(a.CheckOutByWho,'') when '' then 'No' else 'Yes' end as CheckOut " +
                               " from [resource] r " +
                               " inner join appointment a on r.ResourceID = a.ResourceID " +
                               " left outer join Membership m on a.MembershipNo = m.MembershipNo " +
                               " left outer join UserMst u on m.SalesPersonID = u.UserName " +
                               " where Convert(date,a.StartTime) = Convert(date,GETDATE()) and r.ResourceID = '' + @ResourceID +'' ";

                QueryCommand qc = new QueryCommand(query);
                qc.AddParameter("@ResourceID", ResourceID);

                DataSet ds = DataService.GetDataSet(qc);

                dgvPreview.DataSource = ds.Tables[0];
                dgvPreview.Refresh();
            }
        }

        private void dgvPreview_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            if (dgvPreview.Columns[e.ColumnIndex].Name == "btnViewAppointment")
            {
                string AppointmendID;
                AppointmendID = dgvPreview.Rows[e.RowIndex].Cells["ID"].Value.ToString(); 
                Appointment appointment = new Appointment(AppointmendID);

                AppointmentBookData _data = new AppointmentBookData();
                
                TimeSlot _timeslot = new TimeSlot();
                _timeslot.StartTime = appointment.StartTime;
                _timeslot.Duration = new TimeSpan(0, appointment.Duration, 0);
                _timeslot.BackColor = Color.FromArgb(appointment.BackColor);
                _timeslot.FontColor = Color.FromArgb(appointment.FontColor);
                _timeslot.Description = appointment.BuildDescription();

                UserMst salesPerson = new UserMst(appointment.SalesPersonID);
                var employee = new Employee
                {
                    Id = salesPerson.UserName,
                    Gender = (salesPerson.Gender ?? false) ? EmployeeGender.Male : EmployeeGender.Female,
                    Name = salesPerson.DisplayName,
                    Image = salesPerson.Image,
                };
                employee.Schedule.Add(_timeslot);
                _data.Employees.Add(employee);

                _timeslot.Employee = employee;

                Resource res = new Resource(appointment.ResourceID);
                var appres = new AppointmentResource
                {
                    ResourceID = res.ResourceID,
                    Name = res.ResourceName
                };

                _data.Resources.Add(appres);

                _timeslot.Resource = appres;

                frmAppointmentEditor _appointmentEditor = new frmAppointmentEditor();
                _appointmentEditor.Member = appointment.Membership;
                _appointmentEditor.ClearServices();

                var items = new AppointmentItemCollection();
                items.Where(AppointmentItem.Columns.AppointmentId, appointment.Id);
                items.Where(AppointmentItem.Columns.Deleted, false);
                items.Load();

                foreach (var item in items)
                    _appointmentEditor.Items.Add(item);

                _appointmentEditor.Description = appointment.Description;

                var _options = new AppointmentBook.AppointmentBookControlOptions();
                _options.IsAppointmentView = true;

                var setting = new AppSetting(AppSetting.Columns.AppSettingKey, "PointOfSale_WorkingHoursStart").AppSettingValue;
                TimeSpan.TryParse(setting, out _options.WorkDayStart);

                setting = new AppSetting(AppSetting.Columns.AppSettingKey, "PointOfSale_WorkingHoursEnd").AppSettingValue;
                TimeSpan.TryParse(setting, out _options.WorkDayEnd);

                setting = new AppSetting(AppSetting.Columns.AppSettingKey, "PointOfSale_MinimumTimeInterval").AppSettingValue;
                TimeSpan timeResolution;
                TimeSpan.TryParse(setting, out timeResolution);
                _options.TimeResolution = (int)timeResolution.TotalMinutes;
                _appointmentEditor.SetOptions(_options);

                _appointmentEditor.SetOptions(_options);
                if (_appointmentEditor.ShowEdit(_data, _timeslot) == DialogResult.OK)
                {
                    var duration = TimeSpan.FromMinutes(_appointmentEditor.Duration);
                    Logger.writeLog(duration.ToString());

                    appointment.Duration = _appointmentEditor.Duration;
                    appointment.Description = _appointmentEditor.Description;
                    appointment.FontColor = _appointmentEditor.FontColor.ToArgb();
                    appointment.BackColor = _appointmentEditor.BackColor.ToArgb();

                    appointment.MembershipNo = (_appointmentEditor.Member == null) ? null : _appointmentEditor.Member.MembershipNo;

                    //AppointmentController.SaveAppointment(appointment, _appointmentEditor.Items);
                    string status = "";
                    if (!AppointmentController.SendAppointment(appointment, _appointmentEditor.Items, SyncAppointmentThread, out status))
                    {
                        MessageBox.Show(status);
                    }
                    //SendAppointment(appointment);
                }
            }
        }

        private void SendAppointment(Appointment app)
        {
            //if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.SendAppointmentDirectlyToServer), false))
            //{
                //SyncAppointmentRealTimeController ssc = new SyncAppointmentRealTimeController();

                //List<Guid> hdr = new List<Guid>();
                //hdr.Add(app.Id);
                //string status = "";

                //if (!ssc.SendAppointmentToServer(hdr, out status))
                //{
                //    MessageBox.Show(status);
                //}
            //}
        }

        private void SendAppointment(List<Guid> hdr)
        {
            //if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.SendAppointmentDirectlyToServer), false))
            //{
                //SyncAppointmentRealTimeController ssc = new SyncAppointmentRealTimeController();

                //string status = "";

                //if (!ssc.SendAppointmentToServer(hdr, out status))
                //{
                //    MessageBox.Show(status);
                //}
            //}
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCheckIn_Click(object sender, EventArgs e)
        {
            List<Appointment> objApp = new List<Appointment>();
            try
            {
                foreach (DataGridViewRow row in dgvPreview.Rows)
                {
                    if (Convert.ToBoolean(row.Cells["colChecked"].Value))
                    {
                        var AppointmentID = row.Cells["ID"].Value.ToString();
                        var checkin = row.Cells["CheckIn"].Value.ToString().ToLower();

                        if (checkin == "no")
                        {
                            Appointment appo = new Appointment(AppointmentID);
                            appo.IsServerUpdate = false;
                            appo.CheckInByWho = UserInfo.username;
                            appo.CheckInTime = DateTime.Now;
                            appo.Save(UserInfo.username);

                            objApp.Add(appo);
                        }
                    }
                }
                string status = "";
                if (!AppointmentController.SendAppointment(objApp, SyncAppointmentThread, out status))
                {
                    throw new Exception(status);
                }
                //SendAppointment(objHdr);
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex.Message);
                MessageBox.Show("There is an error and we already logged it.");
            }

            this.Close();
        }

        private void dgvPreview_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string CheckIn = dgvPreview.Rows[e.RowIndex].Cells["CheckIn"].Value.ToString().ToLower();

                if (CheckIn == "yes")
                {
                    DataGridViewCell cell = dgvPreview.Rows[e.RowIndex].Cells["colChecked"];
                    DataGridViewCheckBoxCell chkCell = cell as DataGridViewCheckBoxCell;
                    chkCell.Value = false;
                    chkCell.FlatStyle = FlatStyle.Flat;
                    chkCell.Style.ForeColor = Color.DarkGray;
                    cell.ReadOnly = true;
                }
            }
        }
    }
}
