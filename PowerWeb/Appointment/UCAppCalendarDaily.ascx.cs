using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using AppointmentBook;
using System.Drawing;
using PowerPOS;
using SubSonic;
using System.Globalization;

namespace PowerWeb.Appointment
{
    public partial class UCAppCalendarDaily : System.Web.UI.UserControl
    {
        #region *) Template
        public const string SCRIPT_Main = @"<div id='demo' class='fixedTable'>
                                            <div class='fixed-icon'>
                                                <img src='images/calendar_icon.png' height='40px' width='40px' style='margin: 30px 0px 0px 30px' />
                                                <div style='margin-top: 29px;' class='employeeName'>
                                                    {0}
                                                </div>
                                            </div>
                                            <header class='fixedTable-header'>
                                            <table class='table table-bordered'>
                                              <thead>
		                                        {1}
                                              </thead>
                                            </table>
                                          </header>
                                            <aside class='fixedTable-sidebar'>
                                            <table class='table table-bordered'>
                                              <tbody>
	                                          {2}
                                              </tbody>
                                            </table>
                                          </aside>
                                            <div class='fixedTable-body'>
                                                <table class='table table-bordered'>
                                                    <tbody>
				                                        {3}
                                                    </tbody>
                                                </table>
                                            </div>
                                        </div>";
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                bool isCreateAppot = false;
                var table = (DataTable)Session["privileges"];
                if (table != null)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        if (row["privilegename"].ToString().ToLower() == PrivilegesController.CREATE_APPOINTMENT.ToLower())
                        {
                            isCreateAppot = true;
                        }
                    }
                }
                
                isCreateAppointment.Value = isCreateAppot.ToString();
                if (!isCreateAppot)
                {
                    trNewAppointment.Visible = false;
                }
                else
                {
                    trNewAppointment.Visible = true;
                }
            //    btnToday_Click(btnToday, new EventArgs());
            //}
            //else
            //{
            //    string parameter = Request["__EVENTARGUMENT"];
            //    string target = Request["__EVENTTARGET"];
            //    if (target == "LoadCalendar")
            //        LoadCalendar();
            }
        }

        public void BindCalendar(DateTime day, AppointmentBookData data)
        {

            string startTime = (string)ConfigurationManager.AppSettings["AppointmentStartTime"];
            string endTime = (string)ConfigurationManager.AppSettings["AppointmentEndTime"];
            AppointmentBookControlOptions options = new AppointmentBookControlOptions();
            string scriptEmployee = "<tr>";
            string scriptTimescale = "";
            string scriptSchedule = "";
            Dictionary<TimeSpan, List<string>> tableSch = new Dictionary<TimeSpan, List<string>>();
            int timeUnit = 30;

            var workDayStart = options.WorkDayStart;
            var workDayEnd = options.WorkDayEnd;

            if (!string.IsNullOrEmpty(startTime))
                workDayStart = new TimeSpan(Convert.ToInt32(startTime.Split(':')[0]), Convert.ToInt32(startTime.Split(':')[1]), 0);
            if (!string.IsNullOrEmpty(endTime))
                workDayEnd = new TimeSpan(Convert.ToInt32(endTime.Split(':')[0]), Convert.ToInt32(endTime.Split(':')[1]), 0);

            string onClick = "OpenForm('0','{0}','{1}','{2}'); return false;";
            string dateStr = day.ToString("dd MMM yyyy");
            string timeStr = DateTime.Now.Date.Add(workDayStart).ToString("hh_mm_tt");
            btnNew.Attributes["OnClick"] = string.Format(onClick, "", dateStr, timeStr);

            while (workDayStart <= workDayEnd)
            {
                string singleTimescale = @"<tr class='timeScaleCol'>
                                              <td>{0}</td>
                                           </tr>";
                if (workDayEnd.Minutes == 0)
                    singleTimescale = string.Format(singleTimescale, "<b>" + (DateTime.Now.Date.Add(workDayStart)).ToString("hh:mm tt") + "</b>");
                else
                    singleTimescale = string.Format(singleTimescale, (DateTime.Now.Date.Add(workDayStart)).ToString("hh:mm tt"));
                scriptTimescale += Environment.NewLine + singleTimescale;

                List<string> listSch = new List<string>();
                scriptSchedule += Environment.NewLine + "<tr>";
                foreach (var emp in data.Employees)
                {
                    var schedules = emp.Schedule
                                       .Where(o => o.StartTime.Date == day.Date
                                              && workDayStart >= o.StartTime.TimeOfDay
                                              && workDayStart < o.StartTime.Add(o.Duration).TimeOfDay)
                                      .FirstOrDefault();
                    if (schedules == null)
                    {
                        string tdScript = "<td onclick=\"OpenForm('0','{0}','{1}','{2}');\"></td>";
                        dateStr = day.ToString("dd MMM yyyy");
                        timeStr = DateTime.Now.Date.Add(workDayStart).ToString("hh_mm_tt");
                        tdScript = string.Format(tdScript, emp.Id, dateStr, timeStr);
                        scriptSchedule += Environment.NewLine + tdScript;
                    }
                    else if (schedules.StartTime.TimeOfDay == workDayStart)
                    {
                        int rowSpan = ((int)schedules.Duration.TotalMinutes) / timeUnit;
                        if (rowSpan <= 0)
                            rowSpan = 1;
                        string desc = schedules.CompleteDescription;
                        int maxHeight = rowSpan * 40;
                        string imgCheck = "";
                        if (schedules.Check)
                            imgCheck = "<img src='../Appointment/Images/Check_small.png' height='16px' width='16px' />";
                        if (rowSpan <= 1)
                        {
                            scriptSchedule += string.Format("<td  vertical-align: \"middle\"; align=\"center\"; valign=\"middle\"; class='appTitle'; onclick=\"OpenForm('{2}','','','');\" style=\"background-color: {0}; border: 1px solid {1}; color: {1};\"><div style=\"overflow-x:hidden; overflow-y:auto; max-height:{4}px;\">{5}{3}</div></td>"
                                                        , ColorTranslator.ToHtml(schedules.BackColor)
                                                        , ColorTranslator.ToHtml(schedules.FontColor)
                                                        , schedules.Id
                                                        , desc
                                                        , maxHeight
                                                        , imgCheck);
                        }
                        else
                        {
                            scriptSchedule += string.Format("<td align=\"center\"; valign=\"middle\"; rowspan='{4}'; class='appTitle'; onclick=\"OpenForm('{2}','','','');\" style=\"background-color: {0}; border: 1px solid {1}; color: {1};\"><div style=\"overflow-x:hidden; overflow-y:auto; max-height:{5}px;\">{6}{3}</div></td>"
                                                        , ColorTranslator.ToHtml(schedules.BackColor)
                                                        , ColorTranslator.ToHtml(schedules.FontColor)
                                                        , schedules.Id
                                                        , desc
                                                        , rowSpan
                                                        , maxHeight
                                                        , imgCheck);
                        }
                    }
                    else
                    {
                        scriptSchedule += Environment.NewLine;
                    }
                }
                scriptSchedule += Environment.NewLine + "</tr>";
                workDayStart += TimeSpan.FromMinutes(timeUnit);
            }
            foreach (var emp in data.Employees)
            {

                string singleEmployee = @"<th class='employeeCol'>
                                                 <img src='{0}' height='100px' width='100px' />
                                                <div class='employeeName'>
                                                    {1}
                                                </div>
                                          </th>";
                singleEmployee = string.Format(singleEmployee, emp.ImageURL, emp.Name);
                scriptEmployee += Environment.NewLine + singleEmployee;
            }
            scriptEmployee += Environment.NewLine + "</tr>";

            string finalScript = string.Format(SCRIPT_Main,
                day.ToString("dd MMM"), scriptEmployee, scriptTimescale, scriptSchedule);
            //ltContent.Text = finalScript;
        }

        public void LoadCalendar()
        {
            DateTime day = DateTime.Now.Date;
            DateTime.TryParseExact(txtStartDate.Text, "dd MMM yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out day);

            #region *) Load UserMst

            DataTable empDt = new DataTable();
            string query = "SELECT * FROM UserMst WHERE Deleted = 0 AND IsASalesPerson = 1";
            string orderBy = AppSetting.GetSetting(AppSetting.SettingsName.Appointment.SalesPerson_OrderBy);
            if (!string.IsNullOrEmpty(orderBy))
                query = query + "ORDER BY " + orderBy + " ASC ";
            else
                query = query + "ORDER BY UserName ASC ";
            empDt = DataService.GetDataSet(new QueryCommand(query)).Tables[0];

            #endregion

            #region *) Load Appointment

            var appointments = new AppointmentCollection();
            appointments.Where(PowerPOS.Appointment.Columns.StartTime, Comparison.GreaterOrEquals, day);
            appointments.Where(PowerPOS.Appointment.Columns.StartTime, Comparison.LessThan, day + TimeSpan.FromDays(1));
            appointments.Where(PowerPOS.Appointment.Columns.Deleted, false);
            appointments.OrderByAsc(PowerPOS.Appointment.Columns.StartTime);
            appointments.Load();

            #endregion

            #region *) Create AppointmentBookData

            AppointmentBookData data = new AppointmentBookData();
            foreach (DataRow dr in empDt.Rows)
            {
                UserMst salesPerson = new UserMst("UserName", dr["UserName"].ToString());
                var employee = new Employee
                {
                    Id = salesPerson.UserName,
                    Gender = (salesPerson.Gender ?? false) ? EmployeeGender.Male : EmployeeGender.Female,
                    Name = salesPerson.UserName,
                    Image = salesPerson.ImageData
                };
                employee.Schedule.Clear();
                foreach (var appointment in appointments)
                {
                    if (appointment.SalesPersonID == employee.Id)
                    {
                        bool isPartialPayment = false;
                        decimal outstandingAmount = 0;
                        var oh = new OrderHdr(appointment.OrderHdrID);
                        if (!string.IsNullOrEmpty(appointment.OrderHdrID) && (!oh.IsVoided && !oh.IsNew))
                            isPartialPayment = AppointmentController.IsPartialPayment(appointment.OrderHdrID, out outstandingAmount);
                        var timeSlot = new TimeSlot
                        {
                            Id = appointment.Id,
                            BackColor = Color.FromArgb(appointment.BackColor),
                            CompleteDescription = appointment.BuildCompleteDescriptionWEB(),
                            Description = appointment.BuildCompleteDescriptionWEB(),
                            Duration = TimeSpan.FromMinutes(appointment.Duration),
                            Employee = employee,
                            StartTime = appointment.StartTime,
                            FontColor = Color.FromArgb(appointment.FontColor),
                            Check = !string.IsNullOrEmpty(appointment.OrderHdrID) && (!oh.IsVoided && !oh.IsNew),
                            Organization = appointment.Organization,
                            PickupLocation = appointment.PickUpLocation,
                            NoOfChildren = appointment.NoOfChildren.GetValueOrDefault(0),
                            IsPartialPayment = isPartialPayment,
                            OutStandingAmount = outstandingAmount
                        };
                        employee.Schedule.Add(timeSlot);
                    }
                }
                data.Employees.Add(employee);
            }

            #endregion

            BindCalendar(day, data);
        }

        protected void btnFind_Click(object sender, EventArgs e)
        {
            LoadCalendar();
        }

        protected void btnPrev_Click(object sender, EventArgs e)
        {
            DateTime day = DateTime.Now.Date;
            DateTime.TryParseExact(txtStartDate.Text, "dd MMM yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out day);
            day = day.AddDays(-1);
            txtStartDate.Text = day.ToString("dd MMM yyyy");
            LoadCalendar();
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            DateTime day = DateTime.Now.Date;
            DateTime.TryParseExact(txtStartDate.Text, "dd MMM yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out day);
            day = day.AddDays(1);
            txtStartDate.Text = day.ToString("dd MMM yyyy");
            LoadCalendar();
        }

        protected void btnToday_Click(object sender, EventArgs e)
        {
            txtStartDate.Text = DateTime.Now.ToString("dd MMM yyyy");
            LoadCalendar();
        }

        protected void ddlSalesPerson_Init(object sender, EventArgs e)
        {
            #region *) Load UserMst

            DataTable empDt = new DataTable();
            string query = "SELECT * FROM UserMst WHERE Deleted = 0 AND IsASalesPerson = 1";
            string orderBy = AppSetting.GetSetting(AppSetting.SettingsName.Appointment.SalesPerson_OrderBy);
            if (!string.IsNullOrEmpty(orderBy))
                query = query + "ORDER BY " + orderBy + " ASC ";
            else
                query = query + "ORDER BY UserName ASC ";
            empDt = DataService.GetDataSet(new QueryCommand(query)).Tables[0];

            #endregion

            DropDownList ddl = (DropDownList)sender;
            ddl.DataSource = empDt;
            ddl.DataBind();
        }

        protected void ddlPointOFSaleID_Init(object sender, EventArgs e)
        {
            #region *) Load Point of Sale ID

            DataTable empDt = new DataTable();
            string query = @"
                select * from (
	                -- select '0' as Value, ' Select All' as Text

	                -- UNION ALL   

	                SELECT PointOfSaleID as Value, PointOfSaleName as Text FROM PointOfSale WHERE ( Deleted = 0 or Deleted is null )
                ) as x order by x.Text asc 
            ";
            //string orderBy = AppSetting.GetSetting(AppSetting.SettingsName.Appointment.SalesPerson_OrderBy);
            //if (!string.IsNullOrEmpty(orderBy))
            //    query = query + "ORDER BY " + orderBy + " ASC ";
            //else
            //    query = query + "ORDER BY UserName ASC ";
            empDt = DataService.GetDataSet(new QueryCommand(query)).Tables[0];

            #endregion

            DropDownList ddl = (DropDownList)sender;
            ddl.DataSource = empDt;
            ddl.DataBind();
        }

        protected void ddlServices_Init(object sender, EventArgs e)
        {
            #region *) Load Services
            string sql = @"SELECT  I.ItemNo
                            ,I.ItemName+' ['+ItemNo+']' ItemName
                    FROM	Item I
                    WHERE	I.Deleted = 0
                            AND I.IsServiceItem = 1 
                            AND I.IsInInventory <> 1 
                            AND I.CategoryName <> 'SYSTEM'
                    ORDER BY I.ItemName";
            DataTable dtItem = new DataTable();
            dtItem.Load(DataService.GetReader(new QueryCommand(sql)));
            ddlServices.DataSource = dtItem;
            ddlServices.DataBind();
            #endregion

            DropDownList ddl = (DropDownList)sender;
            ddl.DataSource = dtItem;
            ddl.DataBind();
        }
    }
}