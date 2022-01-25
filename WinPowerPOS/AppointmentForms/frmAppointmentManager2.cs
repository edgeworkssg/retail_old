using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using AppointmentBook;
using AppointmentBook.Model;
using PowerPOS;
using PowerPOS.AppointmentRealTimeController;
using PowerPOS.Container;
using SubSonic;
using WinPowerPOS.EditBillForms;
using System.Data;
using System.ComponentModel;
using WinPowerPOS.MaintenanceForms;
using System.Threading;

namespace WinPowerPOS.AppointmentForms
{
    public partial class frmAppointmentManager2 : Form
    {
        public BackgroundWorker SyncSalesThread;
        public BackgroundWorker SyncAppointmentThread;
        public BackgroundWorker SyncSendAppointmentThread;
        public BackgroundWorker PanelLoadingThread;
        public frmViewSyncLog fSyncLog;

        private readonly AppointmentBookData _data;
        private readonly AppointmentBookControlOptions _options;
        private DateTime _day;

        private TimeSlot _copiedTimeSlot;
        private bool _cutPaste;

        private readonly frmAppointmentEditor _appointmentEditor = new frmAppointmentEditor();

        private readonly UserMstCollection _salesPersons = new UserMstCollection();

        private readonly ResourceCollection _resources = new ResourceCollection();

        private readonly frmSearchAppointment _searchAppointment = new frmSearchAppointment();

        public DateTime daySelected;

        delegate void SetDayFromMainFormCallBack();

        public frmAppointmentManager2(frmViewSyncLog frmSynclog)
        {
            InitializeComponent();

            var ScreenSize = Screen.PrimaryScreen.Bounds;

            if (ScreenSize.Width < 1025)
            {
                dateControl.Width = dateControl.Width / 2;
                panel1.Width = panel1.Width * 2;
            }

            fSyncLog = frmSynclog;

            #region PanelLoading
            //PanelLoadingThread = new BackgroundWorker();
            //PanelLoadingThread.DoWork += new DoWorkEventHandler(PanelLoadingThread_DoWork);
            //PanelLoadingThread.RunWorkerCompleted += new RunWorkerCompletedEventHandler(PanelLoadingThread_RunWorkerCompleted);

            #endregion

            #region Real Time Download Appointment

            ////Init sync
            //SyncDownloadAppointmentThread = new BackgroundWorker();
            //DoWorkEventArgs doe = new DoWorkEventArgs(null);
            //SyncDownloadAppointmentThread.WorkerSupportsCancellation = true;
            //SyncDownloadAppointmentThread.DoWork += new DoWorkEventHandler(SyncDownloadAppointmentThread_DoWork);
            //SyncDownloadAppointmentThread.RunWorkerCompleted += new RunWorkerCompletedEventHandler(SyncDownloadAppointmentThread_RunWorkerCompleted);

            #endregion

            _options = new AppointmentBookControlOptions();
            _options.IsAppointmentView = false;

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
            daySelected = DateTime.Now.Date;

            appointmentBookControl.TimeSlotDragged += TimeSlotDragged;
            appointmentBookControl.TimeSlotResized += TimeSlotResized;
            appointmentBookControl.TimeSlotSelected += TimeSlotSelected;
            appointmentBookControl.TimeSlotDoubleClicked += TimeSlotDoubleClicked;

            appointmentBookResourceControl.TimeSlotDragged += TimeSlotDragged;
            appointmentBookResourceControl.TimeSlotResized += TimeSlotResized;
            appointmentBookResourceControl.TimeSlotSelected += TimeSlotSelectedResource;
            appointmentBookResourceControl.TimeSlotDoubleClicked += TimeSlotResourceDoubleClicked;

            DataTable dt = new DataTable();
            //string SQLString = "Select * from UserMst where Deleted = 0 and IsASalesPerson = 1 and userfld5 = '" + PointOfSaleInfo.PointOfSaleID.ToString() + "' ";
            string SQLString = @"
                                SELECT * FROM UserMst 
                                WHERE Deleted = 0 
                                    AND IsASalesPerson = 1 
                                    AND (ISNULL(userfld5, '') = '' OR ISNULL(userfld5, '') = 'ALL' OR ISNULL(userfld5, '') LIKE '%{0}%') -- PointOfSaleID
                                    AND (ISNULL(userfld1, '') = '' OR ISNULL(userfld1, '') = 'ALL' OR ISNULL(userfld1, '') LIKE '%{1}%') -- OutletName
                                ";
            SQLString = string.Format(SQLString, PointOfSaleInfo.PointOfSaleID, PointOfSaleInfo.OutletName);

            string temp = AppSetting.GetSetting(AppSetting.SettingsName.Appointment.SalesPerson_OrderBy);
            if (temp != null && temp != "")
            {
                SQLString = SQLString + "Order By " + temp + " ASC ";
            }
            else
            {
                SQLString = SQLString + "Order By DisplayName ASC ";
            }
            dt = DataService.GetDataSet(new QueryCommand(SQLString)).Tables[0];
            newAppointmentToolStripMenuItem.Enabled = btnAddAppointment.Enabled = dt.Rows.Count > 0;

            _data = new AppointmentBookData();
            foreach (DataRow dr in dt.Rows)
            {
                UserMst salesPerson = new UserMst("UserName", dr["UserName"].ToString());
                var employee = new Employee
                    {
                        Id = salesPerson.UserName,
                        Gender = (salesPerson.Gender ?? false) ? EmployeeGender.Male : EmployeeGender.Female,
                        Name = salesPerson.DisplayName,
                        Image = salesPerson.Image,
                    };

                _data.Employees.Add(employee);
            }

            DataTable dtroom = new DataTable();
            string SQLString2 = "";

            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Appointment.UseResourceOnAppointment), false))
            {
                SQLString2 = "select * from resource where isnull(deleted,0) = 0 and resourceid <> 'ROOM_DEFAULT'";
            }
            else
            {
                SQLString2 = "select * from resource where resourceid = 'ROOM_DEFAULT'";
            }
            dtroom = DataService.GetDataSet(new QueryCommand(SQLString2)).Tables[0];
            foreach (DataRow drr in dtroom.Rows)
            {
                Resource res = new Resource(drr["ResourceID"].ToString());
                var appres = new AppointmentResource
                        {
                            ResourceID = res.ResourceID,
                            Name = res.ResourceName,
                            Capacity = res.Capacity ?? 0
                        };

                _data.Resources.Add(appres);
            }

            appointmentBookControl.Data = _data;
            appointmentBookControl.Options = _options;

            appointmentBookResourceControl.Data = _data;
            appointmentBookResourceControl.Options = _options;

            dateControl.Day = _day;
        }

        public DateTime Day
        {
            get { return _day; }
            set
            {
                if (value != _day)
                    SetDay(value);
            }
        }

        private void frmAppointmentManager2_Load(object sender, EventArgs e)
        {
            try
            {
                #region realtime

                //if (!PointOfSaleInfo.IntegrateWithInventory)
                //{
                //    pnlProgress.Visible = true;
                //    this.Enabled = false;
                //    SyncDownloadAppointmentThread.RunWorkerAsync();
                //}

                #endregion

                bool isCreateAppointment = false;
                //show hide base on privileges
                DataRow[] dr3 = UserInfo.privileges.Select("privilegeName='" + PrivilegesController.CREATE_APPOINTMENT + "'");
                if (UserInfo.username.ToLower() == "edgeworks" || dr3.Length > 0)
                    isCreateAppointment = true;

                if (!isCreateAppointment)
                {
                    btnAddAppointment.Visible = false;
                }
                else
                {
                    btnAddAppointment.Visible = true;
                }

                string listsearch = AppSetting.GetSetting(AppSetting.SettingsName.Appointment.AppointmentSearchList) + "";
                if (string.IsNullOrEmpty(listsearch))
                    listsearch = "Name,Mobile,NRIC";

                if (!string.IsNullOrEmpty(listsearch))
                {
                    cbSearch.Items.Clear();
                    cbSearch.Items.Add("Please Select");
                    var l = listsearch.Split(',');
                    foreach (string s in l)
                    {
                        if (!string.IsNullOrEmpty(s))
                        {
                            cbSearch.Items.Add(s);
                        }
                    }

                    cbSearch.SelectedIndex = 0;
                }

                SetDay(_day);

                btnAppointmentView.Visible = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Appointment.UseResourceOnAppointment), false);
                btnRoomView.Visible = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Appointment.UseResourceOnAppointment), false);

                appointmentBookControl.Visible = true;
                appointmentBookResourceControl.Visible = false;

                ChangeToAppointmentView(true);
            }
            catch (Exception exception)
            {
                frmError.ShowError(this, exception);
            }
        }

        public void SetDay(DateTime day)
        {
            //pnlProgress.Visible = true;
           
            _day = day.Date;

            DateTime tomorrow = _day + TimeSpan.FromDays(1);

            var appointments = new AppointmentCollection();
            appointments.Where(Appointment.Columns.StartTime, Comparison.GreaterOrEquals, _day);
            appointments.Where(Appointment.Columns.StartTime, Comparison.LessThan, _day + TimeSpan.FromDays(1));
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

                        var reso = new AppointmentResource();
                        reso = _data.Resources.Find(
                                    delegate(AppointmentResource rs)
                                    {
                                        return rs.ResourceID == appointment.ResourceID;
                                    });

                        if (reso == null)
                        {
                            // set to room default
                            Resource Default = new Resource("ROOM_DEFAULT");
                            reso = new AppointmentResource() { ResourceID = Default.ResourceID, Name = Default.ResourceName, Capacity = Default.Capacity ?? 0 };
                        }

                        var timeSlot = new TimeSlot
                            {
                                Id = appointment.Id,
                                BackColor = Color.FromArgb(appointment.BackColor),
                                Description = appointment.BuildDescriptionWithSetting(AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Appointment.UseResourceOnAppointment), false)),
                                Duration = TimeSpan.FromMinutes(appointment.Duration),
                                Employee = employee,
                                Resource = reso,
                                StartTime = appointment.StartTime,
                                FontColor = Color.FromArgb(appointment.FontColor),
                                Check = !string.IsNullOrEmpty(appointment.OrderHdrID) && !(new OrderHdr(appointment.OrderHdrID).IsVoided),
                                IsPartialPayment = isPartialPayment,
                                OutStandingAmount = outstandingAmount,
                                TimeExtension = appointment.TimeExtension ?? 0
                            };

                        employee.Schedule.Add(timeSlot);
                    }
                }
            }

            foreach (var resource in _data.Resources)
            {
                resource.Schedule.Clear();

                foreach (var appointment in appointments)
                {
                    if (appointment.ResourceID == resource.ResourceID)
                    {
                        bool isPartialPayment = false;
                        decimal outstandingAmount = 0;
                        if (!string.IsNullOrEmpty(appointment.OrderHdrID) && !(new OrderHdr(appointment.OrderHdrID).IsVoided))
                            isPartialPayment = AppointmentController.IsPartialPayment(appointment.OrderHdrID, out outstandingAmount);

                        var emplo = new Employee();
                        emplo = _data.Employees.Find(
                                delegate(Employee rs)
                                {
                                    return rs.Id == appointment.SalesPersonID;
                                });

                        var timeSlot = new TimeSlot
                        {
                            Id = appointment.Id,
                            BackColor = Color.FromArgb(appointment.BackColor),
                            Description = appointment.BuildDescriptionResource(),
                            Duration = TimeSpan.FromMinutes(appointment.Duration),
                            Employee = emplo,
                            Resource = resource,
                            StartTime = appointment.StartTime,
                            FontColor = Color.FromArgb(appointment.FontColor),
                            Check = !string.IsNullOrEmpty(appointment.OrderHdrID) && !(new OrderHdr(appointment.OrderHdrID).IsVoided),
                            IsPartialPayment = isPartialPayment,
                            OutStandingAmount = outstandingAmount,
                            TimeExtension = appointment.TimeExtension ?? 0
                        };

                        resource.Schedule.Add(timeSlot);
                    }
                }
            }

            appointmentBookControl.Day = _day;
            appointmentBookResourceControl.Day = _day;
            this.daySelected = _day;


            //pnlProgress.Visible = false;
        }

        private void TimeSlotResized(TimeSlot timeslot)
        {
            string status = "";
            if (timeslot.Duration < TimeSpan.FromMinutes(_options.TimeResolution))
                timeslot.Duration = TimeSpan.FromMinutes(_options.TimeResolution);

            var maxDuration = TimeHelper.GetMaxDuration(timeslot.Employee, _options, timeslot.StartTime);
            if (timeslot.Duration > maxDuration)
                timeslot.Duration = maxDuration;

            var appointment = new Appointment(timeslot.Id) { Duration = (int)timeslot.Duration.TotalMinutes, IsServerUpdate = true };
            appointment.Save();
            if (!SyncSendAppointmentThread.IsBusy)
                SyncSendAppointmentThread.RunWorkerAsync();

            /*if (!AppointmentController.SendAppointment(appointment, SyncAppointmentThread, out status))
            {
                MessageBox.Show(status);
            }*/
        }


        private void TimeSlotDragged(TimeSlot timeSlot)
        {
            try
            {
                string status = "";
                bool isCollide = false;
                if (timeSlot == null)
                    return;

                if (!AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Appointment.DisableTimeCollisionCheck), false))
                {
                    if (!AppointmentController.CheckCollision(timeSlot.Id.ToString()
                        , timeSlot.Employee.Id
                        , timeSlot.StartTime, timeSlot.Duration.Minutes, out status))
                    {
                        isCollide = true;
                        throw new Exception("Appointment collide with others");
                    }
                }

                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Appointment.UseResourceOnAppointment), false))
                {
                    if (!AppointmentController.CheckCollisionResource(timeSlot.Id.ToString()
                        , timeSlot.Employee.Id, timeSlot.Resource.ResourceID
                        , timeSlot.StartTime, timeSlot.Duration.Minutes, out status))
                    {
                        isCollide = isCollide && true;
                        throw new Exception("Appointment collide with others");
                    }
                }

                Appointment appointment1 = new Appointment(timeSlot.Id);
                if (timeSlot.StartTime == appointment1.StartTime &&
                    timeSlot.Employee.Id == appointment1.SalesPersonID
                    && timeSlot.Resource.ResourceID == appointment1.ResourceID
                    )
                    isCollide = true;


                if (!isCollide)
                {
                    var appointment = new Appointment(timeSlot.Id)
                        {
                            StartTime = timeSlot.StartTime,
                            SalesPersonID = timeSlot.Employee.Id,
                            ResourceID = timeSlot.Resource.ResourceID,
                            IsServerUpdate = true
                        };
                    appointment.Save();
                    if (!SyncSendAppointmentThread.IsBusy)
                        SyncSendAppointmentThread.RunWorkerAsync();
                    /*if(!AppointmentController.SendAppointment(appointment, SyncAppointmentThread, out status))
                    {
                        MessageBox.Show(status);
                    }*/
                }
                
                
            }
            catch (Exception exception)
            {
                frmError.ShowError(this, exception);
            }

            SetDay(dateControl.Day);
        }

        private void TimeSlotSelected(TimeSlot timeslot)
        {
            try
            {
                deleteAppointmentToolStripMenuItem.Enabled =
                    editAppointmentToolStripMenuItem.Enabled =
                    copyToolStripMenuItem.Enabled =
                    cutToolStripMenuItem.Enabled =
                    timeslot != null;

                createInvoiceToolStripMenuItem.Visible = timeslot != null && !timeslot.Check;
                showInvoiceToolStripMenuItem.Visible = timeslot != null && timeslot.Check;
                toolStripMenuItem3.Visible = createInvoiceToolStripMenuItem.Visible || showInvoiceToolStripMenuItem.Visible;

                pasteToolStripMenuItem.Enabled = _copiedTimeSlot != null
                                                 && appointmentBookControl.SelectedEmployeeIndex >= 0
                                                 &&
                                                 TimeHelper.IsFree(
                                                     _data.Employees[appointmentBookControl.SelectedEmployeeIndex], _options,
                                                     _day, appointmentBookControl.SelectedTime, _copiedTimeSlot.Duration);
            }
            catch (Exception exception)
            {
                frmError.ShowError(this, exception);
            }
        }

        private void TimeSlotSelectedResource(TimeSlot timeslot)
        {
            try
            {
                deleteAppointmentToolStripMenuItem.Enabled =
                    editAppointmentToolStripMenuItem.Enabled =
                    copyToolStripMenuItem.Enabled =
                    cutToolStripMenuItem.Enabled =
                    timeslot != null;

                createInvoiceToolStripMenuItem.Visible = timeslot != null && !timeslot.Check;
                showInvoiceToolStripMenuItem.Visible = timeslot != null && timeslot.Check;
                toolStripMenuItem3.Visible = createInvoiceToolStripMenuItem.Visible || showInvoiceToolStripMenuItem.Visible;

                pasteToolStripMenuItem.Enabled = _copiedTimeSlot != null
                                                 && appointmentBookResourceControl.SelectedResourceIndex >= 0
                                                 &&
                                                 TimeHelper.IsFreeResource(
                                                     _data.Resources[appointmentBookResourceControl.SelectedResourceIndex], _options,
                                                     _day, appointmentBookControl.SelectedTime, _copiedTimeSlot.Duration);
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
                if (_options.IsAppointmentView)
                {
                    EditAppointmentCommand(appointmentBookControl.SelectedTimeSlot);
                    appointmentBookControl.Invalidate();
                }
                else
                {
                    EditAppointmentCommand(appointmentBookResourceControl.SelectedTimeSlot);
                    appointmentBookControl.Invalidate();
                }
            }
            catch (Exception exception)
            {
                frmError.ShowError(this, exception);
            }
        }

        private void EditAppointmentCommand(TimeSlot timeSlot)
        {
            if (timeSlot != null)
            {
                var appointment = new Appointment(timeSlot.Id);
                var oldDuration = appointment.Duration;
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
                    if (oldDuration != _appointmentEditor.Duration)
                    {
                        var timeExtension = _appointmentEditor.Duration - oldDuration;
                        if (timeExtension > 0)
                            appointment.TimeExtension = timeExtension;
                        else
                            appointment.TimeExtension = null;
                    }


                    Logger.writeLog(duration.ToString());
                    timeSlot.Duration = duration;
                    timeSlot.BackColor = _appointmentEditor.Color;
                    timeSlot.FontColor = _appointmentEditor.FontColor;

                    appointment.Duration = _appointmentEditor.Duration;
                    appointment.Description = _appointmentEditor.Description;
                    appointment.FontColor = timeSlot.FontColor.ToArgb();
                    appointment.BackColor = timeSlot.BackColor.ToArgb();

                    appointment.MembershipNo = (_appointmentEditor.Member == null) ? null : _appointmentEditor.Member.MembershipNo;

                    appointment.IsServerUpdate = true;
                    AppointmentController.SaveAppointment(appointment, _appointmentEditor.Items);

                    if (!SyncSendAppointmentThread.IsBusy)
                        SyncSendAppointmentThread.RunWorkerAsync();
                    //AppointmentController.SaveAppointment(appointment, _appointmentEditor.Items);
                    //string status = "";
                    /*if (!AppointmentController.SendAppointment(appointment, _appointmentEditor.Items, SyncAppointmentThread, out status))
                    {
                        MessageBox.Show(status);
                    }*/

                    timeSlot.Description = appointment.BuildDescription();

                    appointmentBookControl.Invalidate();

                    SetDay(dateControl.Day);

                }
                else
                {
                    appointmentBookControl.Invalidate();
                    SetDay(dateControl.Day);
                }
            }
        }

        private void AddAppointmentCommand(int employeeIndex, TimeSpan proposedTime, bool findNewTime)
        {
            _appointmentEditor.Member = null;
            _appointmentEditor.ClearServices();
            _appointmentEditor.SyncSalesThread = SyncSalesThread;
            _appointmentEditor.SyncAppointmentThread = SyncAppointmentThread;
            _appointmentEditor.ProposedTime = proposedTime;
            _appointmentEditor.FindNewTime = findNewTime;
            if (_appointmentEditor.ShowNew(_data, _day, employeeIndex) == DialogResult.OK)
            {
                var employee = _data.Employees[_appointmentEditor.SelectedEmployeeIndex];
                var duration = TimeSpan.FromMinutes(_appointmentEditor.Duration);
                var startTime = TimeHelper.FindStartTimeForDuration(employee, _options, _day, proposedTime, duration);
                var resource = _data.Resources[_appointmentEditor.SelectedResourcesIndex];

                var timeSlot = new TimeSlot
                {
                    BackColor = _appointmentEditor.Color,
                    Employee = employee,
                    FontColor = _appointmentEditor.FontColor,
                    StartTime = startTime,
                    Duration = duration,
                    Resource = resource
                };

                employee.Schedule.Add(timeSlot);
                resource.Schedule.Add(timeSlot);
                appointmentBookControl.Invalidate();

                var newAppointment = NewAppointmentFromTimeSlot(timeSlot);
                newAppointment.Deleted = false;
                newAppointment.Description = _appointmentEditor.Description;


                if (_appointmentEditor.Member != null)
                    newAppointment.MembershipNo = _appointmentEditor.Member.MembershipNo;

                newAppointment.IsServerUpdate = false;

                Membership mbr = null;
                if (_appointmentEditor.Member != null && _appointmentEditor.Member.IsNew)
                {
                    mbr = _appointmentEditor.Member;
                    mbr.SalesPersonID = newAppointment.SalesPersonID;
                    mbr.Save(UserInfo.username);
                }
                AppointmentController.SaveAppointment(newAppointment, _appointmentEditor.Items);

                if (!SyncSendAppointmentThread.IsBusy)
                    SyncSendAppointmentThread.RunWorkerAsync();
                /*string status = "";
                if (!AppointmentController.SendAppointment(newAppointment, _appointmentEditor.Items, SyncAppointmentThread, mbr, out status))
                {
                    MessageBox.Show(status);
                }*/

                timeSlot.Description = newAppointment.BuildDescription();
                timeSlot.Id = newAppointment.Id;

                SetDay(dateControl.Day);
            }
        }

        private void AddAppointmentCommandResource(int resourceIndex, TimeSpan proposedTime, bool findNewTime)
        {
            _appointmentEditor.Member = null;
            _appointmentEditor.ClearServices();
            _appointmentEditor.SyncSalesThread = SyncSalesThread;
            _appointmentEditor.SyncAppointmentThread = SyncAppointmentThread;
            _appointmentEditor.ProposedTime = proposedTime;
            _appointmentEditor.FindNewTime = findNewTime;
            if (_appointmentEditor.ShowNewResource(_data, _day, resourceIndex) == DialogResult.OK)
            {
                var employee = _data.Employees[_appointmentEditor.SelectedEmployeeIndex];
                var duration = TimeSpan.FromMinutes(_appointmentEditor.Duration);
                var startTime = TimeHelper.FindStartTimeForDuration(employee, _options, _day, proposedTime, duration);
                var resource = _data.Resources[_appointmentEditor.SelectedResourcesIndex];

                var timeSlot = new TimeSlot
                {
                    BackColor = _appointmentEditor.Color,
                    Employee = employee,
                    FontColor = _appointmentEditor.FontColor,
                    StartTime = startTime,
                    Duration = duration,
                    Resource = resource
                };

                employee.Schedule.Add(timeSlot);
                resource.Schedule.Add(timeSlot);
                appointmentBookControl.Invalidate();

                var newAppointment = NewAppointmentFromTimeSlot(timeSlot);
                newAppointment.Deleted = false;
                newAppointment.Description = _appointmentEditor.Description;


                if (_appointmentEditor.Member != null)
                    newAppointment.MembershipNo = _appointmentEditor.Member.MembershipNo;

                newAppointment.IsServerUpdate = false;

                AppointmentController.SaveAppointment(newAppointment, _appointmentEditor.Items);
                if (!SyncSendAppointmentThread.IsBusy)
                    SyncSendAppointmentThread.RunWorkerAsync();
                /*string status = "";
                if (!AppointmentController.SendAppointment(newAppointment, _appointmentEditor.Items, SyncAppointmentThread, out status))
                {
                    MessageBox.Show(status);
                }*/
                
                timeSlot.Description = newAppointment.BuildDescription();
                timeSlot.Id = newAppointment.Id;

                SetDay(dateControl.Day);
            }
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
                ResourceID = timeSlot.Resource.ResourceID,
            };
        }

        private void btnAddAppointment_Click(object sender, EventArgs e)
        {
            try
            {
                AddAppointmentCommand(0, _options.WorkDayStart, false);
            }
            catch (Exception exception)
            {
                frmError.ShowError(this, exception);
            }
        }

        private void newAppointmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                AddAppointmentCommand(appointmentBookControl.SelectedEmployeeIndex, appointmentBookControl.SelectedTime, false);
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
                    //EditAppointmentCommand(timeSlot);
                    EditAppointmentCommand(appointmentBookControl.SelectedTimeSlot);
                else
                    AddAppointmentCommand(appointmentBookControl.SelectedEmployeeIndex, appointmentBookControl.SelectedTime, false);
                appointmentBookControl.Invalidate();
            }
            catch (Exception exception)
            {
                frmError.ShowError(this, exception);
            }
        }

        private void TimeSlotResourceDoubleClicked(TimeSlot timeSlot)
        {
            try
            {
                if (timeSlot != null)
                    //EditAppointmentCommand(timeSlot);
                    EditAppointmentCommand(appointmentBookResourceControl.SelectedTimeSlot);
                else
                    AddAppointmentCommandResource(appointmentBookResourceControl.SelectedResourceIndex, appointmentBookResourceControl.SelectedTime, false);
                appointmentBookResourceControl.Invalidate();

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
                if (_options.IsAppointmentView)
                {
                    DeleteAppointmentCommand(appointmentBookControl.SelectedTimeSlot);
                }
                else
                {
                    DeleteAppointmentCommandResource(appointmentBookResourceControl.SelectedTimeSlot);
                }
            }
            catch (Exception exception)
            {
                frmError.ShowError(this, exception);
            }
        }

        private void DeleteAppointmentCommand(TimeSlot timeSlot)
        {
            if (MessageBox.Show(this, "Delete selected appointment?", "Please confirm", MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question) == DialogResult.Yes)
            {
                timeSlot.Employee.Schedule.Remove(timeSlot);
                //Appointment.Delete(timeSlot.Id);
                Appointment appointment = new Appointment(timeSlot.Id);
                appointment.Deleted = true;
                appointment.IsServerUpdate = true;
                appointment.Save(UserInfo.username);
                string status = "";
                if (!SyncSendAppointmentThread.IsBusy)
                    SyncSendAppointmentThread.RunWorkerAsync();
                /*if (!AppointmentController.SendAppointment(appointment, SyncAppointmentThread, out status))
                {
                    MessageBox.Show(status);
                }*/

                //SetDay(dateControl.Day);
                appointmentBookControl.Invalidate();
            }
        }

        private void DeleteAppointmentCommandResource(TimeSlot timeSlot)
        {
            if (MessageBox.Show(this, "Delete selected appointment?", "Please confirm", MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question) == DialogResult.Yes)
            {
                timeSlot.Resource.Schedule.Remove(timeSlot);
                //Appointment.Delete(timeSlot.Id);
                Appointment appointment = new Appointment(timeSlot.Id);
                appointment.Deleted = true;
                appointment.IsServerUpdate = true;
                appointment.Save(UserInfo.username);
                string status = "";
                if (!SyncSendAppointmentThread.IsBusy)
                    SyncSendAppointmentThread.RunWorkerAsync();
                /*if (!AppointmentController.SendAppointment(appointment, SyncAppointmentThread, out status))
                {
                    MessageBox.Show(status);
                }*/

                //SetDay(dateControl.Day);
                appointmentBookResourceControl.Invalidate();
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

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (_options.IsAppointmentView)
                {
                    _copiedTimeSlot = appointmentBookControl.SelectedTimeSlot;
                    _cutPaste = false;
                }
                else
                {
                    _copiedTimeSlot = appointmentBookResourceControl.SelectedTimeSlot;
                    _cutPaste = false;
                }
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
                if (_options.IsAppointmentView)
                {
                    _copiedTimeSlot = appointmentBookControl.SelectedTimeSlot;
                    _cutPaste = true;
                }
                else
                {
                    _copiedTimeSlot = appointmentBookResourceControl.SelectedTimeSlot;
                    _cutPaste = true;
                }
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
                if (_options.IsAppointmentView)
                {
                    var employeeIndex = appointmentBookControl.SelectedEmployeeIndex;
                    if (employeeIndex < 0 || employeeIndex >= _data.Employees.Count)
                        employeeIndex = 0;

                    var employee = _data.Employees[employeeIndex];

                    TimeSlot newTimeSlot;

                    var appointment = new Appointment(_copiedTimeSlot.Id);

                    if (_cutPaste)
                    {
                        newTimeSlot = _copiedTimeSlot;
                        _copiedTimeSlot.Employee.Schedule.Remove(_copiedTimeSlot);

                        newTimeSlot.StartTime = _day + appointmentBookControl.SelectedTime;
                        newTimeSlot.Employee = employee;
                        employee.Schedule.Add(newTimeSlot);

                        appointment.StartTime = newTimeSlot.StartTime;
                        appointment.SalesPersonID = employee.Id;
                        appointment.IsServerUpdate = true;
                        appointment.Save(UserInfo.username);
                        /*if (!AppointmentController.SendAppointment(appointment, SyncAppointmentThread, out status))
                        {
                            MessageBox.Show(status);
                        }*/
                        if (!SyncSendAppointmentThread.IsBusy)
                            SyncSendAppointmentThread.RunWorkerAsync();


                    }
                    else
                    {
                        newTimeSlot = _copiedTimeSlot.Clone();

                        newTimeSlot.StartTime = _day + appointmentBookControl.SelectedTime;
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
                        /*if (!AppointmentController.SendAppointment(newAppointment, newItems,SyncAppointmentThread, out status))
                        {
                            MessageBox.Show(status);
                        }*/

                        newTimeSlot.Id = newAppointment.Id;
                    }

                    appointmentBookControl.Invalidate();
                }
                else
                {
                    var resourceIndex = appointmentBookResourceControl.SelectedResourceIndex;
                    if (resourceIndex < 0 || resourceIndex >= _data.Employees.Count)
                        resourceIndex = 0;

                    var resource = _data.Resources[resourceIndex];

                    TimeSlot newTimeSlot;

                    var appointment = new Appointment(_copiedTimeSlot.Id);

                    if (_cutPaste)
                    {
                        newTimeSlot = _copiedTimeSlot;
                        _copiedTimeSlot.Employee.Schedule.Remove(_copiedTimeSlot);

                        newTimeSlot.StartTime = _day + appointmentBookControl.SelectedTime;
                        newTimeSlot.Resource = resource;
                        resource.Schedule.Add(newTimeSlot);

                        appointment.StartTime = newTimeSlot.StartTime;
                        appointment.ResourceID = resource.ResourceID;
                        appointment.IsServerUpdate = true;
                        appointment.Save();
                        /*if (!AppointmentController.SendAppointment(appointment, SyncAppointmentThread, out status))
                        {
                            MessageBox.Show(status);
                        }*/
                        if (!SyncSendAppointmentThread.IsBusy)
                            SyncSendAppointmentThread.RunWorkerAsync();
                    }
                    else
                    {
                        newTimeSlot = _copiedTimeSlot.Clone();

                        newTimeSlot.StartTime = _day + appointmentBookControl.SelectedTime;
                        newTimeSlot.Resource = resource;
                        newTimeSlot.Check = false;
                        newTimeSlot.IsPartialPayment = false;
                        newTimeSlot.OutStandingAmount = 0;
                        resource.Schedule.Add(newTimeSlot);

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

                    appointmentBookControl.Invalidate();
                }
                SetDay(dateControl.Day);
            }
            catch (Exception exception)
            {
                frmError.ShowError(this, exception);
            }
        }
        
        private void CreateInvoiceCommand(TimeSlot timeSlot)
        {
            if (timeSlot != null)
            {
                if (FormController.ShowInvoiceWithReturn(timeSlot.Id, SyncSalesThread, SyncAppointmentThread))
                {
                    var appointment = new Appointment(timeSlot.Id);
                    timeSlot.Check = (!string.IsNullOrEmpty(appointment.OrderHdrID) && !(new OrderHdr(appointment.OrderHdrID).IsVoided));
                    if (!SyncSendAppointmentThread.IsBusy)
                        SyncSendAppointmentThread.RunWorkerAsync();
                }
            }
        }

        private void ShowInvoiceCommand(TimeSlot timeSlot)
        {
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

        private void createInvoiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                CreateInvoiceCommand(appointmentBookControl.SelectedTimeSlot);
                appointmentBookControl.Invalidate();
                TimeSlotDragged(appointmentBookControl.SelectedTimeSlot);
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

        private void frmAppointmentManager2_KeyDown(object sender, KeyEventArgs e)
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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                _searchAppointment._txtSearch = this.txtSearch.Text;
                _searchAppointment._SearchType = cbSearch.SelectedItem.ToString();
                _searchAppointment.ShowDialog();

                this.cbSearch.SelectedIndex = 0;
                this.txtSearch.Text = "";
            }
            catch (Exception exception)
            {
                frmError.ShowError(this, exception);
            }
        }

        private void btnAppointmentView_Click(object sender, EventArgs e)
        {
            ChangeToAppointmentView(true);
        }

        private void btnRoomView_Click(object sender, EventArgs e)
        {
            ChangeToAppointmentView(false);
        }

        private void ChangeToAppointmentView(bool IsAppointmentView)
        {
            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Appointment.UseResourceOnAppointment), false))
            {
                if (IsAppointmentView)
                {
                    btnRoomView.Visible = true;
                    btnAppointmentView.Visible = false;
                    appointmentBookControl.Visible = true;
                    appointmentBookResourceControl.Visible = false;

                }
                else
                {
                    btnRoomView.Visible = false;
                    btnAppointmentView.Visible = true;
                    appointmentBookControl.Visible = false;
                    appointmentBookResourceControl.Visible = true;
                }
            }
            else
            {
                IsAppointmentView = true;
            }

            _options.IsAppointmentView = IsAppointmentView;
            appointmentBookControl.Options = _options;
            appointmentBookResourceControl.Options = _options;
        }

        private void frmAppointmentManager2_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        public void SetDayFromMainForm()
        {
            if (this.InvokeRequired)
            {
                SetDayFromMainFormCallBack d = new SetDayFromMainFormCallBack(SetDayFromMainForm);
                this.Invoke(d, new object[] { });
            }
            else
            {
                SetDay(dateControl.Day);
            }
        }

        #region panelloading
        //private void PanelLoadingThread_DoWork(object sender, DoWorkEventArgs e)
        //{ 
        //    try
        //    {
        //        SetDay(dateControl.Day);
        //        e.Result = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.writeLog(ex);
        //    }
        //}

        //private void PanelLoadingThread_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    pnlProgress.Visible = false;
        //    pnlProgress.Invalidate();

        //}
        #endregion

        #region realtimesync obsolete

        //private void SyncDownloadAppointmentThread_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    //this is the thread for syncing purpose

        //    try
        //    {
        //        SyncAppointmentRealTimeController ssc = new SyncAppointmentRealTimeController();
        //        ssc.OnProgressUpdates += scc_OnProgressUpdates;
        //        bool result = ssc.DownloadAppointment(e);
        //        e.Result = result; 
                
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.writeLog(ex);
        //    }
        //}

        //private void SyncDownloadAppointmentThread_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    pnlProgress.Visible = false;
        //    if (!(bool)e.Result)
        //    {
        //        MessageBox.Show("Error sync appointment from the web. Please check your internet connection.");
        //    }
        //    else
        //    {
                
        //        SetDay(_day);
        //    }

        //    this.Enabled = true;
        //}

        //protected void scc_OnProgressUpdates(object sender, string message)
        //{
        //    if (fSyncLog == null)
        //    {
        //        fSyncLog = new frmViewSyncLog();
        //    }
        //    fSyncLog.SyncAppointmentStatus = message;
        //    addLog("appointment", message);
        //}

        //private void addLog(string type, string msg)
        //{

        //    msg = DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss - ") + msg;
           
        //    fSyncLog.AddAppointmentLog(msg);
        //}

        #endregion
    }
}
