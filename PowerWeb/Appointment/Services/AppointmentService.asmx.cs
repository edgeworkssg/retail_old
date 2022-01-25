using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using PowerPOS;
using System.Web.Script.Serialization;
using System.Data;
using SubSonic;
using PowerPOSLib.Container;
using PowerPOSLib.Helper;
using PowerPOS.Container;
using Newtonsoft.Json;
using System.Web.Script.Services;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Configuration;
using AppointmentBook;
using System.Threading;
using System.Diagnostics;

namespace PowerWeb.Appointment.Services
{
    /// <summary>
    /// Summary description for MobileWS
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class AppointmentService : System.Web.Services.WebService
    {
        #region *) E-Appointment

        [WebMethod(CacheDuration = 0)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string FetchMembership(string search)
        {
            string result = "";
            var jss = new JavaScriptSerializer();
            try
            {
                string sql = @"SELECT  M.MembershipNo
		                        ,M.NameToAppear
		                        ,M.NRIC
		                        ,M.Mobile
		                        ,ISNULL(M.StreetName,'')+' '+ISNULL(M.StreetName2,'') Address
                        FROM	Membership M
                        WHERE	ISNULL(M.Deleted,0) = 0
		                        AND (  ISNULL(M.MembershipNo,'') LIKE '%{0}%'
			                        OR ISNULL(M.NameToAppear,'') LIKE '%{0}%'
			                        OR ISNULL(M.NRIC,'') LIKE '%{0}%'
			                        OR ISNULL(M.Mobile,'') LIKE '%{0}%'
			                        OR ISNULL(M.StreetName,'') LIKE '%{0}%'
                                    OR ISNULL(M.StreetName2,'') LIKE '%{0}%')
                        ORDER BY M.MembershipNo";
                sql = string.Format(sql, search);
                DataTable dt = new DataTable();
                dt.Load(DataService.GetReader(new QueryCommand(sql)));
                result = (ConvertToGoogleJSon(dt)).Replace("\\", string.Empty);
            }
            catch (Exception ex)
            {
                result = jss.Serialize("Error :" + ex.Message);
                Logger.writeLog(ex);
            }
            return result;
        }

        [WebMethod(CacheDuration = 0)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string FetchOutletSales(string pointOfSaleID)
        {
            string result = "";
            var jss = new JavaScriptSerializer();
            try
            {
                string sql = @"select a.UserName
                                     , a.DisplayName 
	                                 , a.userfld1
                                  from UserMst a
                                 where a.IsASalesPerson = 1
                                   and (a.Deleted is null or a.Deleted = 0)";
                //sql = string.Format(sql, search);

                if (pointOfSaleID.Equals("0") == false) {
                    //sql += "and a." + UserMst.UserColumns.AssignedPOS + " like '%" + pointOfSaleID + "%'";
                    PointOfSale pos = new PointOfSale(pointOfSaleID);
                    sql += string.Format("AND (ISNULL(a.{0}, '') = '' OR ISNULL(a.{0}, '') = 'ALL' OR a.{0} LIKE '%{1}%') ", UserMst.UserColumns.AssignedPOS, pointOfSaleID);
                    sql += string.Format("AND (ISNULL(a.{0}, '') = '' OR ISNULL(a.{0}, '') = 'ALL' OR a.{0} LIKE '%{1}%') ", UserMst.UserColumns.AssignedOutlet, pos.OutletName);
                } 
                DataTable dt = new DataTable();
                dt.Load(DataService.GetReader(new QueryCommand(sql)));
                //result = (ConvertToGoogleJSon(dt)).Replace("\\", string.Empty);
                result = JsonConvert.SerializeObject(dt);
            }
            catch (Exception ex)
            {
                result = jss.Serialize("Error :" + ex.Message);
                Logger.writeLog(ex);
            }
            return result;
        }

        [WebMethod(CacheDuration = 0)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string FetchMembershipGroup()
        {
            string result = "";
            var jss = new JavaScriptSerializer();
            try
            {
                string sql = @"select MembershipGroupId, GroupName
                                  from MembershipGroup
                                 where isnull(Deleted,0) = 0";

                DataTable dt = new DataTable();
                dt.Load(DataService.GetReader(new QueryCommand(sql)));
                result = JsonConvert.SerializeObject(dt);
            }
            catch (Exception ex)
            {
                result = jss.Serialize("Error :" + ex.Message);
                Logger.writeLog(ex);
            }
            return result;
        }

        [WebMethod(CacheDuration = 0)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string FetchResources()
        {
            string result = "";
            var jss = new JavaScriptSerializer();
            try
            {
                string sql = "";
                
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Appointment.UseResourceOnAppointment), false))
                {
                    sql = @"select a.ResourceName
                                     , a.ResourceId 
	                              from [Resource] a
                                 where (a.Deleted is null or a.Deleted = 0) and ResourceID != 'ROOM_DEFAULT'";
                }
                else
                {
                    sql = @"select a.ResourceName
                                     , a.ResourceId 
	                              from [Resource] a
                                 where ResourceID = 'ROOM_DEFAULT'";
                }
                //sql = string.Format(sql, search);

                DataTable dt = new DataTable();
                dt.Load(DataService.GetReader(new QueryCommand(sql)));
                
                result = JsonConvert.SerializeObject(dt);
            }
            catch (Exception ex)
            {
                result = jss.Serialize("Error :" + ex.Message);
                Logger.writeLog(ex);
            }
            return result;
        }

        [WebMethod(CacheDuration = 0)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public MembershipData FetchMembershipByMembershipNo(string membershipNo)
        {
            string status = "success";
            Membership member = null;
            try
            {
                MembershipCollection col = new MembershipCollection();

                col.Where(Membership.Columns.MembershipNo, membershipNo);
                col.Where(Membership.Columns.Deleted, false);
                col.Load();

                if (col.Count() == 0)
                    throw new Exception(LanguageManager.GetTranslation("Membership not found"));

                member = col[0];
            }
            catch (Exception ex)
            {
                status = ex.Message;
                Logger.writeLog(ex);
            }

            var result = new MembershipData
            {
                MembershipNo = member == null ? "" : member.MembershipNo,
                NameToAppear = member == null ? "" : member.NameToAppear,
                Mobile = member == null ? "" : member.Mobile,
                Email = member == null ? "" : member.Email,
                NRIC = member == null ? "" : member.Nric,
                StreetName = member == null ? "" : member.StreetName,
                StreetName2 = member == null ? "" : member.StreetName2,
                Status = status,
                MembershipGroupId = member == null ? 0 : member.MembershipGroupId
            };

            return result;
        }

        [WebMethod(CacheDuration = 0)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string SaveAppointment(string data)
        {
            string result = "success";
            try
            {
                string decoded = HttpUtility.UrlDecode(data, Encoding.UTF8);
                Dictionary<string, string> values = JsonConvert.DeserializeObject<Dictionary<string, string>>(decoded);
                string ID = values["ID"];
                string SalesPerson = values["SalesPerson"];
                string Service = values["Service"];
                string Date = values["Date"];
                string Hour = values["Hour"];
                string Minute = values["Minute"];
                string AMPM = values["AMPM"];
                string Duration = values["Duration"];
                string Remark = values["Remark"];
                string MembershipNo = values["MembershipNo"];
                string Name = values["Name"]; ;
                string NRIC = values["NRIC"];
                string Mobile = values["Mobile"];
                string Email = values["Email"];
                string Address = values["Address"];
                string Address2 = values["Address2"];
                string rawPointOfSaleID = values["PointOfSaleID"];
                string ResourceId = values["ResourceId"];
                int pointOfSaleID = 0;
                try {
                    pointOfSaleID = Convert.ToInt32(rawPointOfSaleID);
                }
                catch(Exception ex) {
                    Logger.writeLog(ex.StackTrace);
                }

                string rawMembershipGroupID = values["MembershipGroupID"];
                int MembershipGroupID = 0;
                try
                {
                    MembershipGroupID = Convert.ToInt32(rawMembershipGroupID);
                }
                catch (Exception ex)
                {
                    Logger.writeLog(ex.StackTrace);
                }


                QueryCommandCollection qmc = new QueryCommandCollection();

                #region *) Set Membership
                Membership mp = new Membership(Membership.Columns.MembershipNo, MembershipNo);
                if (string.IsNullOrEmpty(MembershipNo) || mp.IsNew)
                {
                    string query = @"SELECT  POS.PointOfSaleID, POS.MembershipCode
                            FROM	PointOfSale POS
		                            INNER JOIN Setting ST ON ST.PointOfSaleID = POS.PointOfSaleID";
                    string membershipCode = "E";
                    DataTable dt = new DataTable();
                    dt.Load(DataService.GetReader(new QueryCommand(query)));
                    if (dt.Rows.Count > 0)
                        membershipCode = (string)dt.Rows[0]["MembershipCode"];
                    mp.MembershipNo = MembershipController.getNewMembershipNo(membershipCode);
                    mp.NameToAppear = Name;
                    mp.Nric = NRIC;
                    mp.Mobile = Mobile;
                    mp.Email = Email;
                    mp.StreetName = Address;
                    mp.StreetName2 = Address2;
                    mp.Gender = "M";
                    mp.SubscriptionDate = DateTime.Now;
                    mp.ExpiryDate = DateTime.Now.AddYears(1);
                    mp.DateOfBirth = DateTime.Now;
                    mp.Deleted = false;
                    mp.UniqueID = Guid.NewGuid();
                    if (MembershipGroupID == 0)
                        mp.MembershipGroupId = new MembershipGroupController()
                            .FetchAll()
                            .Where(o => o.Deleted.GetValueOrDefault(false) == false)
                            .OrderBy(o => o.MembershipGroupId)
                            .FirstOrDefault()
                            .MembershipGroupId;
                    else
                        mp.MembershipGroupId = MembershipGroupID;

                    #region *) Validation Check Exisiting NRIC & Mobile No

                    bool isExist = false;
                    string message = "";

                    if (!string.IsNullOrEmpty(mp.Nric))
                    {
                        Membership currNRIC = new Membership(Membership.Columns.Nric, mp.Nric);
                        if (!currNRIC.IsNew && !currNRIC.Deleted.GetValueOrDefault(false))
                        {
                            message = LanguageManager.GetTranslation("Membership with same NRIC Already exist");
                            isExist = true;
                        }
                    }

                    if (!string.IsNullOrEmpty(mp.Mobile))
                    {
                        Membership currMobile = new Membership(Membership.Columns.Mobile, mp.Mobile);
                        if (!currMobile.IsNew && !currMobile.Deleted.GetValueOrDefault(false))
                        {
                            message += Environment.NewLine + LanguageManager.GetTranslation("Membership with same Mobile No Already exist");
                            isExist = true;
                        }
                    }

                    if (isExist)
                    {
                        throw new Exception(message);
                    }

                    #endregion

                    qmc.Add(mp.GetInsertCommand("WEB"));
                }
                #endregion

                #region *) Set Appointment
                int duration = Convert.ToInt32(Duration);
                DateTime startTime = DateTime.Now;
                string dateStr = string.Format("{0} {1}:{2} {3}", Date, Hour, Minute, AMPM);
                DateTime.TryParseExact(dateStr, "dd MMM yyyy hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out startTime);

                int oldDuration = 0;
                var theApp = new PowerPOS.Appointment();
                if (ID != "0" && !string.IsNullOrEmpty(ID))
                {
                    theApp = new PowerPOS.Appointment(PowerPOS.Appointment.Columns.Id, ID);
                    if (theApp.Deleted.GetValueOrDefault(false))
                    {
                        if (!theApp.IsNew)
                        {
                            throw new Exception(LanguageManager.GetTranslation("Selected Appointment has been deleted.")+Environment.NewLine+LanguageManager.GetTranslation("Please close the form and refresh page"));
                        }
                    }

                    oldDuration = theApp.Duration;
                }
                PowerPOS.AppointmentItem theAppItem = new PowerPOS.AppointmentItem();

                if (theApp.IsNew)
                    theApp.Id = Guid.NewGuid();
                theAppItem = theApp.AppointmentItemRecords().OrderByDesc(AppointmentItem.Columns.CreatedOn).ToList().FirstOrDefault();
                if (theAppItem == null)
                {
                    theAppItem = new AppointmentItem();
                    theAppItem.AppointmentId = theApp.Id;
                    theAppItem.Id = Guid.NewGuid();
                    theAppItem.Deleted = false;
                }
                theAppItem.ItemNo = Service;
                Item it = new Item(Service);

                if (it != null && !it.IsNew && theAppItem.Quantity == 0)
                {
                    theAppItem.Quantity = 1;
                    theAppItem.UnitPrice = it.RetailPrice;
                }

                theApp.SalesPersonID = SalesPerson;
                theApp.ResourceID = ResourceId;
                theApp.StartTime = startTime;
                theApp.Duration = duration;
                theApp.MembershipNo = mp.MembershipNo;
                theApp.Description = Remark;
                theApp.BackColor = ((Color)(new ColorConverter().ConvertFromString("#488FCD"))).ToArgb();
                theApp.FontColor = ((Color)(new ColorConverter().ConvertFromString("#FFFFFF"))).ToArgb();
                theApp.Deleted = false;
                theApp.PointOfSaleID = pointOfSaleID;
                theApp.IsServerUpdate = true;

                if (oldDuration != duration)
                {
                    var timeExtension = duration = oldDuration;
                    if (timeExtension > 0)
                        theApp.TimeExtension = timeExtension;
                }

                string msg = "";

                if (theApp.ResourceID != null && theApp.ResourceID != "")
                {
                    if (!AppointmentController.CheckCollision(theApp.Id.ToString(), theApp.SalesPersonID, theApp.StartTime, theApp.Duration, out msg))
                    {
                        throw new Exception(msg);
                    }
                }
                else
                {
                    if (!AppointmentController.CheckCollision(theApp.Id.ToString(), theApp.SalesPersonID, theApp.StartTime, theApp.Duration, out msg))
                    {
                        throw new Exception(msg);
                    }
                }

                if(AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Appointment.UseResourceOnAppointment), false))
                {
                    if (!AppointmentController.CheckCollisionResource(theApp.Id.ToString(), theApp.SalesPersonID, theApp.ResourceID, theApp.StartTime, theApp.Duration, out msg))
                    {
                        throw new Exception(msg);
                    }
                }

                if (theApp.IsNew)
                    qmc.Add(theApp.GetInsertCommand("WEB"));
                else
                    qmc.Add(theApp.GetUpdateCommand("WEB"));

                if (theAppItem.IsNew)
                    qmc.Add(theAppItem.GetInsertCommand("WEB"));
                else
                    qmc.Add(theAppItem.GetUpdateCommand("WEB"));

                DataService.ExecuteTransaction(qmc);
                #endregion
            }
            catch (Exception ex)
            {
                result = "Error : " + ex.Message;
                Logger.writeLog(ex);
            }
            return result;
        }

        [WebMethod(CacheDuration = 0)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string DeleteAppointment(string appointmentID)
        {
            string result = "success";
            try
            {
                PowerPOS.Appointment appData = new PowerPOS.Appointment(PowerPOS.Appointment.Columns.Id, appointmentID);
                if (!appData.IsNew)
                {
                    if (!appData.Deleted.GetValueOrDefault(false))
                    {
                        appData.Deleted = true;
                        appData.Save("WEB");
                    }
                    else
                    {
                        throw new Exception(LanguageManager.GetTranslation("Selected Appointment has been deleted.") + Environment.NewLine + LanguageManager.GetTranslation("Please close the form and refresh page"));
                    }
                }
                else
                {
                    throw new Exception(LanguageManager.GetTranslation("Appointment ID didn't exist"));
                }
            }
            catch (Exception ex)
            {
                result = LanguageManager.GetTranslation("Error : ") + ex.Message;
                Logger.writeLog(ex);
            }
            return result;
        }

        [WebMethod(CacheDuration = 0)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public AppointmentData FetchAppointmentByID(string appointmentID)
        {
            AppointmentData data = new AppointmentData();
            PowerPOS.Appointment appData = new PowerPOS.Appointment(PowerPOS.Appointment.Columns.Id, appointmentID);
            if (!appData.IsNew)
            {
                data.SalesPerson = appData.SalesPersonID;
                data.Service = appData.Service;
                data.Date = appData.StartTime.ToString("dd MMM yyyy");
                data.Hour = appData.StartTime.ToString("hh");
                data.Minute = appData.StartTime.ToString("mm");
                data.AMPM = appData.StartTime.ToString("tt");
                data.Duration = appData.Duration.ToString();
                data.Remark = appData.Description;
                data.MembershipNo = appData.MembershipNo;
                data.PointOfSaleID = appData.PointOfSaleID;
                data.ResourceId = appData.ResourceID;
                data.Status = "success";
            }
            else
            {
                data.Status = LanguageManager.GetTranslation("Appointment not found!");
            }
            return data;
        }

        [WebMethod(CacheDuration = 0)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public ServiceResult FetchCalendarTable(string date, string flag, String pointOfSaleID)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                int validPointOfSaleID = 0;
                try
                {
                    validPointOfSaleID = int.Parse(pointOfSaleID);
                }
                catch (Exception) { }

                #region *) Parse Date Input
                string finalScript = "";
                DateTime theDate = date.GetDateTimeValue("dd MMM yyyy");
                if (flag.ToUpper() == "NEXT")
                    theDate = theDate.AddDays(1).Date;
                else if (flag.ToUpper() == "PREV")
                    theDate = theDate.AddDays(-1).Date;
                else if (flag.ToUpper() == "TODAY")
                    theDate = DateTime.Now.Date;
                result.Date = theDate.ToString("dd MMM yyyy");

                #endregion

                #region *) Load UserMst

                DataTable empDt = new DataTable();
                string query = "SELECT * FROM UserMst WHERE Deleted = 0 AND IsASalesPerson = 1";

                if (pointOfSaleID != null && string.IsNullOrEmpty(pointOfSaleID) == false)
                {
                    if (pointOfSaleID.Equals("0") == false) {
                        //query = query + " AND " + UserMst.UserColumns.AssignedPOS + "='" + pointOfSaleID + "' ";
                        PointOfSale pos = new PointOfSale(pointOfSaleID);
                        query += string.Format("AND (ISNULL({0}, '') = '' OR ISNULL({0}, '') = 'ALL' OR {0} LIKE '%{1}%') ", UserMst.UserColumns.AssignedPOS, pointOfSaleID);
                        query += string.Format("AND (ISNULL({0}, '') = '' OR ISNULL({0}, '') = 'ALL' OR {0} LIKE '%{1}%') ", UserMst.UserColumns.AssignedOutlet, pos.OutletName);
                    }
                }

                string orderBy = AppSetting.GetSetting(AppSetting.SettingsName.Appointment.SalesPerson_OrderBy);
                if (!string.IsNullOrEmpty(orderBy))
                    query = query + "ORDER BY " + orderBy + " ASC ";
                else
                    query = query + "ORDER BY UserName ASC ";
                empDt = DataService.GetDataSet(new QueryCommand(query)).Tables[0];

                #endregion

                #region *) Load Appointment

                var appointments = new AppointmentCollection();
                appointments.Where(PowerPOS.Appointment.Columns.StartTime, Comparison.GreaterOrEquals, theDate);
                appointments.Where(PowerPOS.Appointment.Columns.StartTime, Comparison.LessThan, theDate + TimeSpan.FromDays(1));
                appointments.Where(PowerPOS.Appointment.Columns.Deleted, false);
                if (validPointOfSaleID != 0)
                {
                    appointments.Where(PowerPOS.Appointment.Columns.PointOfSaleID, validPointOfSaleID);
                }
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

                #region *) Create Calendar Table

                string startTime = (string)ConfigurationManager.AppSettings["AppointmentStartTime"];
                string endTime = (string)ConfigurationManager.AppSettings["AppointmentEndTime"];
                AppointmentBookControlOptions options = new AppointmentBookControlOptions();
                string scriptEmployee = "<tr>";
                string scriptTimescale = "";
                string scriptSchedule = "";
                Dictionary<TimeSpan, List<string>> tableSch = new Dictionary<TimeSpan, List<string>>();
                int timeUnit = GetTimeUnitInterval();

                var workDayStart = options.WorkDayStart;
                var workDayEnd = options.WorkDayEnd;

                if (!string.IsNullOrEmpty(startTime))
                    workDayStart = new TimeSpan(Convert.ToInt32(startTime.Split(':')[0]), Convert.ToInt32(startTime.Split(':')[1]), 0);
                if (!string.IsNullOrEmpty(endTime))
                    workDayEnd = new TimeSpan(Convert.ToInt32(endTime.Split(':')[0]), Convert.ToInt32(endTime.Split(':')[1]), 0);

                string onClick = "OpenForm('0','{0}','{1}','{2}'); return false;";
                string dateStr = theDate.ToString("dd MMM yyyy");
                string timeStr = DateTime.Now.Date.Add(workDayStart).ToString("hh_mm_tt");
                //btnNew.Attributes["OnClick"] = string.Format(onClick, "", dateStr, timeStr);

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
                                           .Where(o => o.StartTime.Date == theDate.Date
                                                  && workDayStart >= o.StartTime.TimeOfDay
                                                  && workDayStart < o.StartTime.Add(o.Duration).TimeOfDay)
                                          .FirstOrDefault();
                        if (schedules == null)
                        {
                            Debug.WriteLine("SCHEDULES IS NULL BRO...");
                            string tdScript = "<td onclick=\"OpenForm('0','{0}','{1}','{2}');\"></td>";
                            dateStr = theDate.ToString("dd MMM yyyy");
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
                                scriptSchedule += string.Format("<td  vertical-align: \"middle\"; align=\"center\"; valign=\"middle\"; class='appTitle'; onclick=\"OpenForm('{2}','','','');\" style=\"background-color: {0}; border: 1px solid {1}; color: {1};height:{4}px !important;\"><div style=\"overflow-x:hidden; overflow-y:auto; max-height:{4}px;\">{5}{3}</div></td>"
                                                            , ColorTranslator.ToHtml(schedules.BackColor)
                                                            , ColorTranslator.ToHtml(schedules.FontColor)
                                                            , schedules.Id
                                                            , desc
                                                            , maxHeight
                                                            , imgCheck);
                            }
                            else
                            {
                                scriptSchedule += string.Format("<td align=\"center\"; valign=\"middle\"; rowspan='{4}'; class='appTitle'; onclick=\"OpenForm('{2}','','','');\" style=\"background-color: {0}; border: 1px solid {1}; color: {1};height:{5}px !important;\"><div style=\"overflow-x:hidden; overflow-y:auto; max-height:{5}px;\">{6}{3}</div></td>"
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

                finalScript = string.Format(SCRIPT_Main,
                    theDate.ToString("dd MMM"), scriptEmployee, scriptTimescale, scriptSchedule);
                result.Status = "success";
                result.Content = finalScript;
                #endregion
            }
            catch (Exception ex)
            {
                result.Status = "Server Error Occured: " + ex.Message;
                Logger.writeLog(ex);
            }


            return result;
        }

        public int GetTimeUnitInterval()
        {
            int TimeUnit = 0; 

            if(!int.TryParse(AppSetting.GetSetting(AppSetting.SettingsName.Appointment.MinimumIntervalWeb), out TimeUnit))
            {
                TimeUnit = 30; //default
            }

            return TimeUnit;
        }

        public string ConvertToGoogleJSon(DataTable dt)
        {
            string result = "";
            List<string> listCols = new List<string>();
            List<string> listVal = new List<string>();
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                string col = string.Format("{{\"id\":\"\",\"label\":\"{0}\",\"pattern\":\"\",\"type\":\"string\"}}", dt.Columns[i].ColumnName);
                listCols.Add(col);
            }
            for (int row = 0; row < dt.Rows.Count; row++)
            {
                List<string> listValCol = new List<string>();
                for (int col = 0; col < dt.Columns.Count; col++)
                {
                    string valCol = string.Format("{{\"v\":\"{0}\",\"f\":null}}", dt.Rows[row][col] + "");
                    listValCol.Add(valCol);
                }
                string val = string.Format("{{\"c\":[{0}]}}", string.Join(",", listValCol.ToArray()));
                listVal.Add(val);
            }
            result = string.Format("{{\"cols\": [{0}],\"rows\": [{1}]}}",
                string.Join(",", listCols.ToArray()),
                string.Join(",", listVal.ToArray()));

            return result;
        }

        #region *) Template
        public const string SCRIPT_Main = @"<div id='demo' class='fixedTable'>
                                            <div class='fixed-icon'>
                                                <img src='images/calendar_icon.png' height='40px' width='40px' style='margin: 30px 0px 0px 30px' />
                                                <div style='margin-top: 29px;height:20px !important;' class='employeeName'>
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

        #endregion
    }

    public class OutletSales {
        public string UserName { get; set; }
        public string NameToAppear { get; set; }
    }

    public class ServiceResult
    {
        public string Content { get; set; }
        public string Status { set; get; }
        public string Date { set; get; }
    }

    public class MembershipData
    {
        public string MembershipNo { set; get; }
        public string NRIC { set; get; }
        public string Mobile { set; get; }
        public string NameToAppear { set; get; }
        public string StreetName { set; get; }
        public string StreetName2 { set; get; }
        public string Email { set; get; }
        public string Status { set; get; }
        public int MembershipGroupId { get; set; }
    }

    public class AppointmentData
    {
        public string ID { set; get; }
        public string SalesPerson { set; get; }
        public string Date { set; get; }
        public string Hour { set; get; }
        public string Minute { set; get; }
        public string AMPM { set; get; }
        public string Duration { set; get; }
        public string Remark { set; get; }
        public string Service { set; get; }
        public string MembershipNo { set; get; }
        public string Status { set; get; }
        public int? PointOfSaleID { get; set; }
        public string ResourceId { get; set; }
    }
}
