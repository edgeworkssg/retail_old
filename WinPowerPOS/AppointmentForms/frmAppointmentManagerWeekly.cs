using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using PowerPOS;
using PowerPOS.Container;
using AppointmentBook;
using AppointmentBook.Model;
using SubSonic;
using WinPowerPOS.EditBillForms;
using System.Configuration;
using System.ComponentModel;


namespace WinPowerPOS.AppointmentForms
{
    public partial class frmAppointmentManagerWeekly : Form
    {
        public BackgroundWorker SyncSalesThread;
        public BackgroundWorker SyncAppointmentThread;
        public BackgroundWorker SyncSendAppointmentThread;

        #region *) Fields

        private AppointmentBookData _data;
        private readonly AppointmentBookControlOptions _options;
        private DateTime _day;
        public DateTime daySelected;

        private TimeSlot _copiedTimeSlot;
        private bool _cutPaste;

        private readonly frmAppointmentEditorWeekly _appointmentEditor = new frmAppointmentEditorWeekly();

        private readonly UserMstCollection _salesPersons = new UserMstCollection();

        #endregion

        #region *) Constructor

        public frmAppointmentManagerWeekly()
        {
            InitializeComponent();

            _options = new AppointmentBookControlOptions();

            var setting = new AppSetting(AppSetting.Columns.AppSettingKey, "PointOfSale_WorkingHoursStart").AppSettingValue;
            TimeSpan.TryParse(setting, out _options.WorkDayStart);

            setting = new AppSetting(AppSetting.Columns.AppSettingKey, "PointOfSale_WorkingHoursEnd").AppSettingValue;
            TimeSpan.TryParse(setting, out _options.WorkDayEnd);

            setting = new AppSetting(AppSetting.Columns.AppSettingKey, "PointOfSale_MinimumTimeInterval").AppSettingValue;
            TimeSpan timeResolution;
            TimeSpan.TryParse(setting, out timeResolution);
            _options.TimeResolution = (int)timeResolution.TotalMinutes;
            _appointmentEditor.SetOptions(_options);

            _day = DateTime.Now.Date;

            appointmentBookControl.TimeSlotDragged += TimeSlotDragged;
            appointmentBookControl.TimeSlotResized += TimeSlotResized;
            appointmentBookControl.TimeSlotSelected += TimeSlotSelected;
            appointmentBookControl.TimeSlotDoubleClicked += TimeSlotDoubleClicked;
            appointmentBookControl.NewTimeSlotDoubleClicked += NewTimeSlotDoubleClicked;
            DataTable dt = new DataTable();
            //string SQLString = "Select * from UserMst where Deleted = 0 and IsASalesPerson = 1 and Userfld5 = '" + PointOfSaleInfo.PointOfSaleID.ToString() + "' ";
            string SQLString = @"
                                SELECT * FROM UserMst 
                                WHERE Deleted = 0 
                                    AND IsASalesPerson = 1 
                                    AND (ISNULL(userfld5, '') = '' OR ISNULL(userfld5, '') = 'ALL' OR userfld5 LIKE '%{0}%') -- PointOfSaleID
                                    AND (ISNULL(userfld1, '') = '' OR ISNULL(userfld1, '') = 'ALL' OR userfld1 LIKE '%{1}%') -- OutletName
                                ";
            SQLString = string.Format(SQLString, PointOfSaleInfo.PointOfSaleID, PointOfSaleInfo.OutletName);

            string temp = AppSetting.GetSetting(AppSetting.SettingsName.Appointment.SalesPerson_OrderBy);
            if (temp != null && temp != "")
            {
                SQLString = SQLString + "Order By " + temp + " ASC ";
            }
            else
            {
                SQLString = SQLString + "Order By UserName ASC ";
            }
            dt = DataService.GetDataSet(new QueryCommand(SQLString)).Tables[0];
            newAppointmentToolStripMenuItem.Enabled = btnAddAppointment.Enabled = dt.Rows.Count > 0;

            string userGroupSQL = @"
            SELECT  DISTINCT UG.*
            FROM	UserGroup UG
		            INNER JOIN UserMst UM ON UM.GroupName = UG.GroupID
            WHERE	ISNULL(UG.Deleted,0) = 0
		            AND ISNULL(UM.Deleted,0) = 0
		            AND ISNULL(UM.IsASalesPerson,0) = 1";
            var dtUserGroup = DataService.GetDataSet(new QueryCommand(userGroupSQL)).Tables[0];
            //cmbUserGroup.ValueMember = UserGroup.Columns.GroupID;
            //cmbUserGroup.DisplayMember = UserGroup.Columns.GroupName;
            //cmbUserGroup.DataSource = userGroup;
            foreach (DataRow dr in dtUserGroup.Rows)
            {
                CCBoxItem item = new CCBoxItem((string)dr["GroupName"], (int)dr["GroupID"]);
                cmbUserGroup.Items.Add(item);
            }
            cmbUserGroup.MaxDropDownItems = 5;
            cmbUserGroup.DisplayMember = "Name";
            cmbUserGroup.ValueSeparator = ", ";
            for (int i = 0; i < cmbUserGroup.Items.Count; i++)
                cmbUserGroup.SetItemChecked(i, true);

            //var users = new UserMstController().FetchAll().Where(o => o.Deleted.GetValueOrDefault(false) == false).ToList();
            //cmbUser.ValueMember = UserMst.Columns.UserName;
            //cmbUser.DisplayMember = UserMst.Columns.DisplayName;
            //cmbUser.DataSource = users;
            //for (int i = 0; i < users.Count; i++)
            //{
            //    CCBoxItem item = new CCBoxItem(users[i].UserName,i);
            //    cmbUser.Items.Add(item);
            //}
            cmbUser.MaxDropDownItems = 5;
            cmbUser.DisplayMember = "Name";
            cmbUser.ValueSeparator = ", ";

            _data = new AppointmentBookData();
            int count = 0;
            foreach (DataRow dr in dt.Rows)
            {
                UserMst salesPerson = new UserMst("UserName", dr["UserName"].ToString());
                var employee = new Employee
                {
                    Id = salesPerson.UserName,
                    Gender = (salesPerson.Gender ?? false) ? EmployeeGender.Male : EmployeeGender.Female,
                    Name = salesPerson.UserName,
                    Image = salesPerson.Image,
                };
                _data.Employees.Add(employee);
                CCBoxItem item = new CCBoxItem(salesPerson.UserName, count);
                cmbUser.Items.Add(item);
                count++;
            }
            for (int i = 0; i < cmbUser.Items.Count; i++)
                cmbUser.SetItemChecked(i, true);
            appointmentBookControl.Data = _data;
            appointmentBookControl.Options = _options;

            dateControl.Day = _day;
        }

        #endregion

        #region *) Method

        public void SetDate(DateTime date)
        {
            dateControl.Day = date;
            daySelected = date;
            SetDay(date);
        }

        public int? SelectedTimeIndex
        {
            get { return appointmentBookControl.SelectedTimeIndex; }
            set { appointmentBookControl.SelectedTimeIndex = value; }
        }

        public void SetDay(DateTime day)
        {
            pnlProgress.Visible = true;

            _day = day.Date;

            var appointments = new AppointmentCollection();
            appointments.Where(Appointment.Columns.StartTime, Comparison.GreaterOrEquals, _day);
            appointments.Where(Appointment.Columns.StartTime, Comparison.LessOrEquals, _day.AddDays(_options.DayInWeek));
            appointments.Where(Appointment.Columns.Deleted, false);
            appointments.OrderByAsc(Appointment.Columns.StartTime);
            appointments.Load();

            foreach (var employee in _data.Employees)
            {
                employee.Schedule.Clear();
                foreach (var appointment in appointments)
                {
                    if (appointment.SalesPersonID == employee.Id)
                    {
                        bool isPartialPayment = false;
                        decimal outstandingAmount = 0;
                        if (!string.IsNullOrEmpty(appointment.OrderHdrID) && !(new OrderHdr(appointment.OrderHdrID).IsVoided))
                            isPartialPayment = AppointmentController.IsPartialPayment(appointment.OrderHdrID, out outstandingAmount);

                        var timeSlot = new TimeSlot
                        {
                            Id = appointment.Id,
                            BackColor = Color.FromArgb(appointment.BackColor),
                            CompleteDescription = appointment.BuildCompleteDescriptionWEEKLY(),
                            Description = appointment.Description,
                            Duration = TimeSpan.FromMinutes(appointment.Duration),
                            Employee = employee,
                            StartTime = appointment.StartTime,
                            FontColor = Color.FromArgb(appointment.FontColor),
                            Check = !string.IsNullOrEmpty(appointment.OrderHdrID) && !(new OrderHdr(appointment.OrderHdrID).IsVoided),
                            Organization = appointment.Organization,
                            PickupLocation = appointment.PickUpLocation,
                            NoOfChildren = appointment.NoOfChildren.GetValueOrDefault(0),
                            IsPartialPayment = isPartialPayment,
                            OutStandingAmount = outstandingAmount
                        };
                        employee.Schedule.Add(timeSlot);
                    }
                }
                var oldTimeSlotScroll = employee.TimeSlotScroll;
                var newTimeSlotScroll = new Dictionary<DateTime, int>();
                for (int i = 0; i < _options.DayInWeek; i++)
                {
                    newTimeSlotScroll.Add(_day.AddDays(i), 0);
                }
                employee.TimeSlotScroll = newTimeSlotScroll;
            }

            appointmentBookControl.Day = _day;
            daySelected = _day;

            pnlProgress.Visible = false;
        }

        private Appointment NewAppointmentFromTimeSlot(TimeSlot timeSlot)
        {
            return new Appointment
            {
                Description = timeSlot.Description,
                Duration = (int)timeSlot.Duration.TotalMinutes,
                SalesPersonID = timeSlot.Employee.Id,
                StartTime = timeSlot.StartTime,
                BackColor = timeSlot.BackColor.ToArgb(),
                FontColor = timeSlot.FontColor.ToArgb(),
                Organization = timeSlot.Organization,
                PickUpLocation = timeSlot.PickupLocation,
                NoOfChildren = timeSlot.NoOfChildren,
                ResourceID = timeSlot.Resource.ResourceID
            };
        }

        private void EditAppointmentCommand(TimeSlot timeSlot)
        {
            if (timeSlot != null)
            {
                var appointment = new Appointment(timeSlot.Id);
                _appointmentEditor.Member = appointment.Membership;
                _appointmentEditor.ClearServices();
                _appointmentEditor.SyncSalesThread = SyncSalesThread;
                _appointmentEditor.SyncAppointmentThread = SyncAppointmentThread;

                var items = new AppointmentItemCollection();
                items.Where(AppointmentItem.Columns.AppointmentId, appointment.Id);
                items.Where(AppointmentItem.Columns.Deleted, false);
                items.Load();

                foreach (var item in items)
                    _appointmentEditor.Items.Add(item);

                _appointmentEditor.Description = appointment.Description;

                if (_appointmentEditor.ShowEdit(_data, timeSlot) == DialogResult.OK)
                {
                    var duration = TimeSpan.FromMinutes(_appointmentEditor.Duration);
                    Logger.writeLog(duration.ToString());
                    timeSlot.Duration = duration;
                    timeSlot.BackColor = _appointmentEditor.Color;
                    timeSlot.FontColor = _appointmentEditor.FontColor;

                    appointment.StartTime = _appointmentEditor.SelectedDate;
                    appointment.Duration = _appointmentEditor.Duration;
                    appointment.Description = _appointmentEditor.Description;
                    appointment.FontColor = timeSlot.FontColor.ToArgb();
                    appointment.BackColor = timeSlot.BackColor.ToArgb();
                    appointment.Organization = _appointmentEditor.Organization;
                    appointment.PickUpLocation = _appointmentEditor.PickupLocation;
                    appointment.NoOfChildren = _appointmentEditor.NoOfChildren;

                    appointment.MembershipNo = (_appointmentEditor.Member == null) ? null : _appointmentEditor.Member.MembershipNo;

                    //if (!AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Appointment.DisableTimeCollisionCheck), false))
                    //{
                    //    string status = "";
                    //    if (!AppointmentController.CheckCollision(appointment.Id.ToString(), appointment.SalesPersonID, appointment.StartTime, _appointmentEditor.Duration, out status))
                    //    {
                    //        MessageBox.Show(status);
                    //        return;
                    //    }
                    //}

                    appointment.IsServerUpdate = true;
                    AppointmentController.SaveAppointment(appointment, _appointmentEditor.Items);

                    if (!SyncSendAppointmentThread.IsBusy)
                        SyncSendAppointmentThread.RunWorkerAsync();
                    //string status = "";
                    //if (!AppointmentController.SendAppointment(appointment, _appointmentEditor.Items, SyncAppointmentThread, out status))
                    //{
                    //    MessageBox.Show(status);
                    //}

                    timeSlot.CompleteDescription = appointment.BuildCompleteDescriptionWEEKLY();
                    timeSlot.Description = appointment.Description;

                    appointmentBookControl.Invalidate();

                }
            }
        }

        private void AddAppointmentCommand(int employeeIndex, TimeSpan proposedTime, DateTime selectedDate)
        {
            _appointmentEditor.Member = null;
            _appointmentEditor.ClearServices();
            _appointmentEditor.SyncSalesThread = SyncSalesThread;
            _appointmentEditor.SyncAppointmentThread = SyncAppointmentThread;

            if (_appointmentEditor.ShowNew(_data, selectedDate, employeeIndex) == DialogResult.OK)
            {
                var employee = _data.Employees[_appointmentEditor.SelectedEmployeeIndex];
                var duration = TimeSpan.FromMinutes(_appointmentEditor.Duration);
                //var startTime = TimeHelper.FindStartTimeForDuration(employee, _options, selectedDate, proposedTime, duration);
                var startTime = _appointmentEditor.SelectedDate;

                //if (!AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Appointment.DisableTimeCollisionCheck), false))
                //{
                //    string status = "";
                //    if (!AppointmentController.CheckCollision("", employee.Id, startTime, _appointmentEditor.Duration, out status))
                //    {
                //        MessageBox.Show(status);
                //        return;
                //    }
                //}

                Resource res = new Resource("ROOM_DEFAULT");
                var appres = new AppointmentResource
                {
                    ResourceID = res.ResourceID,
                    Name = res.ResourceName,
                    Capacity = res.Capacity ?? 0
                };

                var timeSlot = new TimeSlot
                {
                    BackColor = _appointmentEditor.Color,
                    Employee = employee,
                    FontColor = _appointmentEditor.FontColor,
                    StartTime = startTime,
                    Duration = duration,
                    Description = _appointmentEditor.Description,
                    Organization = _appointmentEditor.Organization,
                    PickupLocation = _appointmentEditor.PickupLocation,
                    NoOfChildren = _appointmentEditor.NoOfChildren,
                    Resource = appres
                };

                employee.Schedule.Add(timeSlot);
                appointmentBookControl.Invalidate();

                var newAppointment = NewAppointmentFromTimeSlot(timeSlot);
                newAppointment.Deleted = false;

                if (_appointmentEditor.Member != null)
                    newAppointment.MembershipNo = _appointmentEditor.Member.MembershipNo;

                newAppointment.IsServerUpdate = true;
                AppointmentController.SaveAppointment(newAppointment, _appointmentEditor.Items);

                if (!SyncSendAppointmentThread.IsBusy)
                    SyncSendAppointmentThread.RunWorkerAsync();
                /*string status = "";
                if (!AppointmentController.SendAppointment(newAppointment, _appointmentEditor.Items, SyncAppointmentThread, out status))
                {
                    MessageBox.Show(status);
                }*/

                timeSlot.CompleteDescription = newAppointment.BuildCompleteDescriptionWEEKLY();
                timeSlot.Description = newAppointment.Description;

                timeSlot.Id = newAppointment.Id;
            }
        }

        private void DeleteAppointmentCommand(TimeSlot timeSlot)
        {
            if (timeSlot == null)
                return;
            if (MessageBox.Show(this, "Delete selected appointment?", "Please confirm", MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question) == DialogResult.Yes)
            {
                timeSlot.Employee.Schedule.Remove(timeSlot);
                Appointment.Delete(timeSlot.Id);
                SetDay(_day);
                appointmentBookControl.Invalidate();
            }
        }

        private void CreateInvoiceCommand(TimeSlot timeSlot)
        {
            if (timeSlot == null)
                return;

            if (FormController.ShowInvoiceWithReturn(timeSlot.Id, SyncSalesThread, SyncAppointmentThread))
            {
                var appointment = new Appointment(timeSlot.Id); ;
                timeSlot.Check = (!string.IsNullOrEmpty(appointment.OrderHdrID) && !(new OrderHdr(appointment.OrderHdrID).IsVoided));
                SetDay(dateControl.Day);
            }
        }

        private void ShowInvoiceCommand(TimeSlot timeSlot)
        {
            if (timeSlot == null)
                return;
            var appointment = new Appointment(timeSlot.Id);
            if (!string.IsNullOrEmpty(appointment.OrderHdrID))
            {
                var order = new OrderHdr(appointment.OrderHdrID);
                if (!order.IsVoided)
                {
                    var orderForm = new frmViewBillDetail { OrderHdrID = appointment.OrderHdrID };
                    orderForm.ShowDialog();
                }
            }
        }

        public void SetDayFromMainForm()
        {
            //pnlProgress.Visible = true;
            //this.Enabled = false;
            SetDay(dateControl.Day);
            //this.Enabled = true;
            //pnlProgress.Visible = false;
        }

        #endregion

        #region *) Event Handler

        private void btnAddAppointment_Click(object sender, EventArgs e)
        {
            try
            {
                AddAppointmentCommand(0, _options.WorkDayStart, _day);
            }
            catch (Exception exception)
            {
                frmError.ShowError(this, exception);
            }
        }

        private void dateControl_Changed(object sender, EventArgs e)
        {
            try
            {
                daySelected = dateControl.Day;
                SetDay(dateControl.Day);
            }
            catch (Exception exception)
            {
                frmError.ShowError(this, exception);
            }
        }

        private void TimeSlotResized(TimeSlot timeslot)
        {
            if (timeslot.Duration < TimeSpan.FromMinutes(_options.TimeResolution))
                timeslot.Duration = TimeSpan.FromMinutes(_options.TimeResolution);

            var maxDuration = TimeHelper.GetMaxDuration(timeslot.Employee, _options, timeslot.StartTime);
            if (timeslot.Duration > maxDuration)
                timeslot.Duration = maxDuration;

            var appointment = new Appointment(timeslot.Id) { Duration = (int)timeslot.Duration.TotalMinutes };
            appointment.IsServerUpdate = true;
            appointment.Save();
            if (!SyncSendAppointmentThread.IsBusy)
                SyncSendAppointmentThread.RunWorkerAsync();

            /*string status = "";
            if (!AppointmentController.SendAppointment(appointment, SyncAppointmentThread, out status))
            {
                MessageBox.Show(status);
            }*/
        }

        private void TimeSlotDragged(TimeSlot timeSlot)
        {
            try
            {
                var appointment = new Appointment(timeSlot.Id)
                {
                    StartTime = timeSlot.StartTime,
                    SalesPersonID = timeSlot.Employee.Id
                };
                appointment.IsServerUpdate = true;
                appointment.Save();
                if (!SyncSendAppointmentThread.IsBusy)
                    SyncSendAppointmentThread.RunWorkerAsync();

                /*string status = "";
                if (!AppointmentController.SendAppointment(appointment, SyncAppointmentThread, out status))
                {
                    MessageBox.Show(status);
                }*/
            }
            catch (Exception exception)
            {
                frmError.ShowError(this, exception);
            }
        }

        private void TimeSlotSelected(int empIndex, TimeSlot timeslot)
        {
            try
            {
                newAppointmentToolStripMenuItem.Enabled = (empIndex >= 0 && empIndex < _data.Employees.Count);
                deleteAppointmentToolStripMenuItem.Enabled = timeslot != null;
                copyToolStripMenuItem.Enabled = timeslot != null;
                cutToolStripMenuItem.Enabled = timeslot != null;
                createInvoiceToolStripMenuItem.Enabled = timeslot != null;
                showInvoiceToolStripMenuItem.Enabled = timeslot != null;
                editAppointmentToolStripMenuItem.Enabled = timeslot != null;

                pasteToolStripMenuItem.Enabled = _copiedTimeSlot != null
                                                 && appointmentBookControl.SelectedEmployeeIndex >= 0;

                createInvoiceToolStripMenuItem.Visible = timeslot != null && !timeslot.Check;
                showInvoiceToolStripMenuItem.Visible = timeslot != null && timeslot.Check;
                toolStripSeparator4.Visible = (timeslot != null && !timeslot.Check) || (timeslot != null && timeslot.Check);
            }
            catch (Exception exception)
            {
                frmError.ShowError(this, exception);
            }
        }

        private void TimeSlotDoubleClicked(TimeSlot timeSlot)
        {
            try
            {
                if (timeSlot != null)
                    EditAppointmentCommand(timeSlot);
                //else
                //    AddAppointmentCommand(appointmentBookControl.SelectedEmployeeIndex, appointmentBookControl.SelectedTime);
                SetDay(_day);
                appointmentBookControl.Invalidate();
            }
            catch (Exception exception)
            {
                frmError.ShowError(this, exception);
            }
        }

        private void NewTimeSlotDoubleClicked(int empIndex, DateTime selectedDate)
        {
            AddAppointmentCommand(empIndex, selectedDate.TimeOfDay, selectedDate);
            SetDay(_day);
            appointmentBookControl.Invalidate();
        }

        private void frmAppointmentManagerWeekly_Load(object sender, EventArgs e)
        {
            try
            {
                bool isCreateAppointment = false;
                DataRow[] dr3 = UserInfo.privileges.Select("privilegeName='" + PrivilegesController.CREATE_APPOINTMENT + "'");
                if (UserInfo.username.ToLower() == "edgeworks" || dr3.Length > 0)
                    isCreateAppointment = true;

                if (!isCreateAppointment)
                {
                    btnAddAppointment.Visible = false;
                    btnDelete.Visible = false;
                }
                else
                {
                    btnAddAppointment.Visible = true;
                    btnDelete.Visible = true;
                }
                SetDay(_day);
                #region Tickle2
                string IsUseForTickle = ConfigurationManager.AppSettings["IsUseForTickle"];
                if (!string.IsNullOrEmpty(IsUseForTickle) && IsUseForTickle.ToLower().Equals("true"))
                {
                    btnAddAppointment.Text = "Booking";
                    lblUserGroup.Text = "Type";
                    lblUser.Text = "Products";
                }
                #endregion
            }
            catch (Exception exception)
            {
                frmError.ShowError(this, exception);
            }
        }

        private void frmAppointmentManagerWeekly_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Right)
            {
                dateControl.Day += TimeSpan.FromDays(1);
                e.Handled = true;
            }
            else if (e.KeyData == Keys.Left)
            {
                dateControl.Day -= TimeSpan.FromDays(1);
                e.Handled = true;
            }
        }

        private void newAppointmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                AddAppointmentCommand(appointmentBookControl.SelectedEmployeeIndex,
                    appointmentBookControl.SelectedDate.TimeOfDay, appointmentBookControl.SelectedDate);
                //AddAppointmentCommand(appointmentBookControl.SelectedEmployeeIndex, appointmentBookControl.SelectedTime);
            }
            catch (Exception exception)
            {
                frmError.ShowError(this, exception);
            }
        }

        private void deleteAppointmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DeleteAppointmentCommand(appointmentBookControl.SelectedTimeSlot);
            }
            catch (Exception exception)
            {
                frmError.ShowError(this, exception);
            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                _copiedTimeSlot = appointmentBookControl.SelectedTimeSlot;
                _cutPaste = false;
            }
            catch (Exception exception)
            {
                frmError.ShowError(this, exception);
            }
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                _copiedTimeSlot = appointmentBookControl.SelectedTimeSlot;
                _cutPaste = true;
            }
            catch (Exception exception)
            {
                frmError.ShowError(this, exception);
            }
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string status = "";
                var employeeIndex = appointmentBookControl.SelectedEmployeeIndex;
                if (employeeIndex < 0 || employeeIndex >= _data.Employees.Count)
                    employeeIndex = 0;

                var employee = _data.Employees[employeeIndex];

                TimeSlot newTimeSlot;

                var theDate = appointmentBookControl.SelectedDate.Date.Add(_copiedTimeSlot.StartTime.TimeOfDay);
                theDate = AppointmentController.GetSuggestedDate(_copiedTimeSlot.Employee.Name, theDate, _copiedTimeSlot.Duration.Minutes);
                var appointment = new Appointment(_copiedTimeSlot.Id);

                if (_cutPaste)
                {
                    newTimeSlot = _copiedTimeSlot;
                    _copiedTimeSlot.Employee.Schedule.Remove(_copiedTimeSlot);

                    newTimeSlot.StartTime = theDate;
                    newTimeSlot.Employee = employee;
                    employee.Schedule.Add(newTimeSlot);

                    appointment.StartTime = newTimeSlot.StartTime;
                    appointment.SalesPersonID = employee.Id;

                    appointment.IsServerUpdate = true;
                    appointment.Save(UserInfo.username);
                    if (!SyncSendAppointmentThread.IsBusy)
                        SyncSendAppointmentThread.RunWorkerAsync();

                    /*if (!AppointmentController.SendAppointment(appointment, SyncAppointmentThread, out status))
                    {
                        MessageBox.Show(status);
                    }*/
                }
                else
                {
                    newTimeSlot = _copiedTimeSlot.Clone();

                    newTimeSlot.StartTime = theDate;
                    newTimeSlot.Employee = employee;
                    newTimeSlot.Check = false;
                    newTimeSlot.IsPartialPayment = false;
                    newTimeSlot.OutStandingAmount = 0;
                    employee.Schedule.Add(newTimeSlot);

                    var newAppointment = NewAppointmentFromTimeSlot(newTimeSlot);
                    newAppointment.OrderHdrID = null;
                    newAppointment.MembershipNo = appointment.MembershipNo;
                    newAppointment.Deleted = false;

                    var items = new AppointmentItemCollection();
                    items.Where(AppointmentItem.Columns.AppointmentId, appointment.Id);
                    items.Where(AppointmentItem.Columns.Deleted, false);
                    items.Load();

                    var newItems = new List<AppointmentItem>();

                    foreach (var item in items)
                        newItems.Add(new AppointmentItem { ItemNo = item.ItemNo, Quantity = item.Quantity, UnitPrice = item.UnitPrice, Deleted = false });

                    newAppointment.IsServerUpdate = true;
                    AppointmentController.SaveAppointment(newAppointment, newItems);
                    if (!SyncSendAppointmentThread.IsBusy)
                        SyncSendAppointmentThread.RunWorkerAsync();

                    /*if (!AppointmentController.SendAppointment(newAppointment, newItems, SyncAppointmentThread, out status))
                    {
                        MessageBox.Show(status);
                    }*/

                    newTimeSlot.Id = newAppointment.Id;
                }
                SetDay(_day);
                appointmentBookControl.Invalidate();
            }
            catch (Exception exception)
            {
                frmError.ShowError(this, exception);
            }
        }

        private void createInvoiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                CreateInvoiceCommand(appointmentBookControl.SelectedTimeSlot);
                appointmentBookControl.Invalidate();
            }
            catch (Exception exception)
            {
                frmError.ShowError(this, exception);
            }
        }

        private void showInvoiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ShowInvoiceCommand(appointmentBookControl.SelectedTimeSlot);
            }
            catch (Exception exception)
            {
                frmError.ShowError(this, exception);
            }
        }

        private void editAppointmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                EditAppointmentCommand(appointmentBookControl.SelectedTimeSlot);
                appointmentBookControl.Invalidate();
            }
            catch (Exception exception)
            {
                frmError.ShowError(this, exception);
            }
        }

        private void cmbUserGroup_DropDownClosed(object sender, EventArgs e)
        {
            List<string> listUserGroupID = new List<string>();
            for (int i = 0; i < cmbUserGroup.CheckedItems.Count; i++)
            {
                CCBoxItem item = cmbUserGroup.CheckedItems[i] as CCBoxItem;
                listUserGroupID.Add(item.Name.ToString());
            }

            //string query = string.Format("SELECT UserName, DisplayName FROM UserMst UM INNER JOIN UserGroup UG ON UM.GroupName = UG.GroupID WHERE  UM.IsASalesPerson = 1 AND UM.Deleted = 0 AND UM.Userfld5 = '{1}' AND UG.GroupName IN ('{0}')",
            //    string.Join("','", listUserGroupID.ToArray()), PointOfSaleInfo.PointOfSaleID.ToString());
            string query = @"
                            SELECT UserName, DisplayName 
                            FROM UserMst UM 
                                INNER JOIN UserGroup UG ON UM.GroupName = UG.GroupID 
                            WHERE UM.IsASalesPerson = 1 
                                AND UM.Deleted = 0 
                                AND (ISNULL(UM.userfld5, '') = '' OR ISNULL(UM.userfld5, '') = 'ALL' OR UM.userfld5 LIKE '%{1}%') -- PointOfSaleID 
                                AND (ISNULL(UM.userfld1, '') = '' OR ISNULL(UM.userfld1, '') = 'ALL' OR UM.userfld1 LIKE '%{2}%') -- OutletName
                                AND UG.GroupName IN ('{0}')
                            ";
            query = string.Format(query, string.Join("','", listUserGroupID.ToArray()), PointOfSaleInfo.PointOfSaleID, PointOfSaleInfo.OutletName);

            string temp = AppSetting.GetSetting(AppSetting.SettingsName.Appointment.SalesPerson_OrderBy);
            if (temp != null && temp != "")
                query = query + " ORDER BY UM." + temp + " ASC ";
            else
                query = query + " ORDER BY UM.UserName ASC ";

            DataTable dt = new DataTable();
            dt = DataService.GetDataSet(new QueryCommand(query)).Tables[0];

            cmbUser.Items.Clear();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                CCBoxItem item = new CCBoxItem(dt.Rows[i][0].ToString(), i);
                cmbUser.Items.Add(item);
            }
            for (int i = 0; i < cmbUser.Items.Count; i++)
                cmbUser.SetItemChecked(i, true);
            cmbUser_DropDownClosed(cmbUser, e);
        }

        private void cmbUser_DropDownClosed(object sender, EventArgs e)
        {
            List<string> listUser = new List<string>();
            for (int i = 0; i < cmbUser.CheckedItems.Count; i++)
            {
                CCBoxItem item = cmbUser.CheckedItems[i] as CCBoxItem;
                listUser.Add(item.Name.ToString());
            }
            string query = string.Format("SELECT * FROM UserMst WHERE UserName IN ('{0}')",
                string.Join("','", listUser.ToArray()));
            string temp = AppSetting.GetSetting(AppSetting.SettingsName.Appointment.SalesPerson_OrderBy);
            if (temp != null && temp != "")
                query = query + " ORDER BY " + temp + " ASC ";
            else
                query = query + " ORDER BY UserName ASC ";

            DataTable dt = new DataTable();
            dt = DataService.GetDataSet(new QueryCommand(query)).Tables[0];

            _data = new AppointmentBookData();
            foreach (DataRow dr in dt.Rows)
            {
                UserMst salesPerson = new UserMst("UserName", dr["UserName"].ToString());
                var employee = new Employee
                {
                    Id = salesPerson.UserName,
                    Gender = (salesPerson.Gender ?? false) ? EmployeeGender.Male : EmployeeGender.Female,
                    Name = salesPerson.UserName,
                    Image = salesPerson.Image,
                };
                _data.Employees.Add(employee);
            }
            appointmentBookControl.Data = _data;
            SetDay(_day);
        }

        private void btnMonthly_Click(object sender, EventArgs e)
        {
            var frmAppointmentMonthly = new frmAppointmentManagerMonthly();
            this.Close();
            frmAppointmentMonthly.ShowDialog();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (appointmentBookControl.SelectedTimeSlot == null)
                    MessageBox.Show("Please select the appointment");
                else
                    DeleteAppointmentCommand(appointmentBookControl.SelectedTimeSlot);
            }
            catch (Exception exception)
            {
                frmError.ShowError(this, exception);
            }
        }

        #endregion
    }
}
