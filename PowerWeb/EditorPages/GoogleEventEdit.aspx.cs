using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Mediachase.AjaxCalendar;
//using MCAjaxCalendar;
using PowerPOS;


    public partial class EditorPages_GoogleEventEdit : System.Web.UI.Page
    {
        private DataSet ds = new DataSet();

        #region Properties
        public int EventId
        {
            get
            {
                if (Request["EventId"] != null)
                {
                    return int.Parse(Request["EventId"].ToString());
                }
                return -1;
            }
        }

        private DateTime _eventStartDate = DateTime.MinValue;
        public DateTime EventStartDate
        {
            get
            {
                if (EventId > 0)
                {
                    CourseController courseCntr = new CourseController();
                    CourseCollection courseCol = courseCntr.FetchByID(EventId);
                    if (courseCol.Count > 0)
                        _eventStartDate = courseCol[0].StartDate.Value;

                }
                return _eventStartDate;
            }
            set { _eventStartDate = value; }
        }

        #region Recurrence Properties
        private int _recPat = 0;//no recurrence
        public int RecPat
        {
            get
            {
                if (Request["p"] != null)
                    _recPat = int.Parse(Request["p"].ToString());
                return _recPat;
            }
            set { _recPat = value; }
        }

        private int _recSubPat = 0;
        public int RecSubPat
        {
            get
            {
                if (Request["sp"] != null)
                    _recSubPat = int.Parse(Request["sp"].ToString());
                return _recSubPat;
            }
            set { _recSubPat = value; }
        }

        private int _recEndType = 2;//number of occurences
        public int RecEndType
        {
            get
            {
                if (Request["et"] != null)
                    _recEndType = int.Parse(Request["et"].ToString());
                return _recEndType;
            }
            set { _recEndType = value; }
        }

        private int _recCom = 0;
        public int RecCom
        {
            get
            {
                if (Request["rc"] != null)
                    _recCom = int.Parse(Request["rc"].ToString());
                return _recCom;
            }
            set { _recCom = value; }
        }

        private DateTime _recStartDate = DateTime.MinValue;
        public DateTime RecStartDate
        {
            get { return _recStartDate; }
            set { _recStartDate = value; }
        }

        private DateTime _recEndDate = DateTime.MinValue;
        public DateTime RecEndDate
        {
            get { return _recEndDate; }
            set { _recEndDate = value; }
        }

        private int _recEndAfter = 1;
        public int RecEndAfter
        {
            get { return _recEndAfter; }
            set { _recEndAfter = value; }
        }

        private int _recFrequency = 1;
        public int RecFrequency
        {
            get { return _recFrequency; }
            set { _recFrequency = value; }
        }

        private byte _recWeekDays = 1;
        public byte RecWeekDays
        {
            get { return _recWeekDays; }
            set { _recWeekDays = value; }
        }

        private int _recDayOfMonth = 1;
        public int RecDayOfMonth
        {
            get { return _recDayOfMonth; }
            set { _recDayOfMonth = value; }
        }

        private int _recWeekNum = 0;
        public int RecWeekNum
        {
            get { return _recWeekNum; }
            set { _recWeekNum = value; }
        }
        #endregion

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadData();
            if (!IsPostBack)
            {
                //Bind Course Type                
                CourseTypeController ctContrl = new CourseTypeController();                
                ddlCourseCat.DataSource = ctContrl.FetchAll();
                ddlCourseCat.DataTextField = "CourseTypeName";
                ddlCourseCat.DataValueField = "CourseTypeID";
                ddlCourseCat.DataBind();

                //Bind Room Location
                RoomController rmContrl = new RoomController();
                tbPlace.DataSource = rmContrl.FetchAll();
                tbPlace.DataTextField = "RoomName";
                tbPlace.DataValueField = "ID";
                tbPlace.DataBind();
                BindData();
                BindRecurrence();
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            lbBack.Click += new EventHandler(lbBack_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);
            btnSave.Click += new EventHandler(btnSave_Click);
            btnDelete.Click += new EventHandler(btnDelete_Click);
            ddRecType.SelectedIndexChanged += new EventHandler(ddRecType_SelectedIndexChanged);
            cbIsAllDay.CheckedChanged += new EventHandler(cbIsAllDay_CheckedChanged);

        }

        void cbIsAllDay_CheckedChanged(object sender, EventArgs e)
        {
            if (cbIsAllDay.Checked)
            {
                ddStartHour.Visible = false;
                ddStartMinute.Visible = false;
                ddEndHour.Visible = false;
                ddEndMinute.Visible = false;
            }
            else
            {
                ddStartHour.Visible = true;
                ddStartMinute.Visible = true;
                ddEndHour.Visible = true;
                ddEndMinute.Visible = true;
            }
        }

        void ddRecType_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (ddRecType.SelectedValue)
            {
                case "0"://no recurrence
                    RecPat = 0;
                    break;
                case "1"://daily recurrence
                    RecPat = 1;
                    break;
                case "2"://every working day
                    RecPat = 2;
                    RecCom = 2;
                    break;
                case "3"://on Mon Wed Fri
                    RecPat = 2;
                    RecCom = 3;
                    break;
                case "4"://on Tue Thu
                    RecPat = 2;
                    RecCom = 4;
                    break;
                case "5"://weekly
                    RecPat = 2;
                    RecCom = 0;
                    break;
                case "6"://monthly
                    RecPat = 3;
                    break;
                case "7"://yearly
                    RecPat = 4;
                    break;
                default:
                    RecPat = 0;
                    break;
            }
            BindRecurrence();
        }

        void btnDelete_Click(object sender, EventArgs e)
        {
            if (EventId > 0)
            {
                CourseController courseCntr = new CourseController();
                courseCntr.Destroy(EventId);
                //if (DBHelper.IsSQL)
                //{
                //    DBHelper.RunSp("CalendarItems_Delete", DBHelper.mp("@Id", SqlDbType.Int, EventId));
                //}
                //else//dataset
                //{
                //    for (int i = ds.Tables["Items"].Rows.Count - 1; i >= 0; i--)
                //    {
                //        if (ds.Tables["Items"].Rows[i]["Id"].ToString() == EventId.ToString())
                //        {
                //            ds.Tables["Items"].Rows.RemoveAt(i);
                //            ds.Tables["Items"].AcceptChanges();
                //            SaveDatasetToXml();
                //            break;
                //        }
                //    }
                //}
            }

            Response.Redirect("~/CalendarDefault.aspx");
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidatePage())//page is not valid
            {
                lbError.Visible = true;
                return;
            }
            string title = tbTitle.Text.Trim() == "" ? "&nbsp;" : tbTitle.Text.Trim();
            if (ddStartHour.Items.Count == 0 || ddStartMinute.Items.Count == 0 || ddEndHour.Items.Count == 0 || ddEndMinute.Items.Count == 0)
                Response.Redirect("~/CalendarDefault.aspx");
            DateTime sDate = DateTime.Parse(tbStartDate.Text);
            sDate = new DateTime(sDate.Year, sDate.Month, sDate.Day, int.Parse(ddStartHour.SelectedValue), int.Parse(ddStartMinute.SelectedValue), 0);
            DateTime eDate = DateTime.Parse(tbEndDate.Text);
            eDate = new DateTime(eDate.Year, eDate.Month, eDate.Day, int.Parse(ddEndHour.SelectedValue), int.Parse(ddEndMinute.SelectedValue), 0);
            if (sDate > eDate)
            {
                lbError.Text = "Start date must be less than end date, or equal to it. ";
                lbError.Visible = true;
                return;
            }
            string description = tbDescription.Text.Trim() == "" ? "&nbsp;" : tbDescription.Text.Trim();
            int place = Convert.ToInt32(tbPlace.SelectedValue);
            bool isAllDay = cbIsAllDay.Checked;

            int evid = EventId;
            string courseType = ddlCourseCat.SelectedValue;
            CourseController courseCntr = new CourseController();
            if (EventId > 0)
            {

                courseCntr.Update(EventId, title, courseType, description, "", sDate, eDate, isAllDay, place);

            }
            else
            {
                evid = courseCntr.InsertWithID(title, description, courseType, sDate, eDate, isAllDay, place);

                //dr["Title"] = title;
                //dr["StartDate"] = sDate;
                //dr["EndDate"] = eDate;
                //dr["Description"] = description;
                //dr["IsAllDay"] = isAllDay;
                //dr["Place"] = place;
                //ds.Tables["Items"].Rows.Add(dr);
                //ds.Tables["Items"].AcceptChanges();
                //evid = (int)dr["Id"];

            }

            //save recurrence
            //Get the associated Recurrence from the database
            RecurrenceController recContr = new RecurrenceController();
            RecurrenceCollection recCol = courseCntr.FetchByID(evid)[0].RecurrenceRecords();
            #region Define end type
            if (ddRecType.SelectedValue != "0")
            {
                if (rbRecNeverEnd.Checked)
                    RecEndType = 0;
                if (rbRecEndDate.Checked)
                {
                    RecEndType = 1;
                    RecEndDate = DateTime.Parse(tbRecEndDate.Text);
                }
                if (rbRecOccNum.Checked)
                {
                    RecEndType = 2;
                    RecEndAfter = int.Parse(tbRecOccNum.Text);
                }
                RecStartDate = DateTime.Parse(tbRecStartDate.Text);
            }
            #endregion

            switch (ddRecType.SelectedValue)
            {
                case "0"://no recurrence
                    //Remove the recurrence if no recurrence is set
                    if (recCol.Count > 0) // Check whether any recurrence exist with the Course
                    {
                        recContr.Destroy(recCol[0].RecID);
                    }
                    //if (DBHelper.IsSQL)
                    //{
                    //    DBHelper.RunSp("Recurrence_DeleteByItemId", DBHelper.mp("@ItemId", SqlDbType.Int, evid));
                    //}
                    //else
                    //{
                    //    for (int i = ds.Tables["Recurrence"].Rows.Count - 1; i >= 0; i--)
                    //    {
                    //        if (ds.Tables["Recurrence"].Rows[i]["ItemId"].ToString() == evid.ToString())
                    //        {
                    //            ds.Tables["Recurrence"].Rows.RemoveAt(i);
                    //            ds.Tables["Recurrence"].AcceptChanges();
                    //        }
                    //    }
                    //}
                    break;
                case "1"://daily
                    #region Daily

                    if (recCol.Count > 0) // Check whether any recurrence exist with the Course
                    {
                        recContr.Update(recCol[0].RecID, evid, 1, 0, RecEndType, RecStartDate,
                            rbRecOccNum.Checked ? RecEndAfter : (int?)null, int.Parse(tbDailyRec.Text), rbRecEndDate.Checked ? RecEndDate : (DateTime?)null,
                            null, null, null, null);
                    }
                    else //Create a new 
                    {
                        recContr.Insert(evid, 1, 0, RecEndType, RecStartDate,
                            rbRecOccNum.Checked ? RecEndAfter : (int?)null, int.Parse(tbDailyRec.Text), rbRecEndDate.Checked ? RecEndDate : (DateTime?)null,
                            null, null, null, null);
                    }
                    //int index = -1;
                    //for (int i = 0; i < ds.Tables["Recurrence"].Rows.Count; i++)
                    //{
                    //    if (ds.Tables["Recurrence"].Rows[i]["ItemId"].ToString() == evid.ToString())
                    //    {
                    //        index = i;
                    //        break;
                    //    }
                    //}
                    //DataRow dr = null;
                    //if (index >= 0)
                    //    dr = ds.Tables["Recurrence"].Rows[index];
                    //else
                    //{
                    //    dr = ds.Tables["Recurrence"].NewRow();
                    //    dr["ItemId"] = evid;
                    //}
                    //dr["Pattern"] = 1;
                    //dr["Subpattern"] = 0;
                    //dr["EndType"] = RecEndType;
                    //dr["StartDate"] = RecStartDate;
                    //dr["EndDate"] = rbRecEndDate.Checked ? (object)RecEndDate : (object)DBNull.Value;
                    //dr["EndAfter"] = rbRecOccNum.Checked ? (object)RecEndAfter : (object)DBNull.Value;
                    //dr["Frequency"] = int.Parse(tbDailyRec.Text);
                    //dr["WeekDays"] = DBNull.Value;
                    //dr["DayOfMonth"] = DBNull.Value;
                    //dr["WeekNum"] = DBNull.Value;
                    //dr["Comment"] = DBNull.Value;

                    //if (index < 0)
                    //    ds.Tables["Recurrence"].Rows.Add(dr);
                    //ds.Tables["Recurrence"].AcceptChanges();
                    //}
                    #endregion
                    break;
                case "2"://on working days
                    #region On Working days

                    if (recCol.Count > 0) // Check whether any recurrence exist with the Course
                    {
                        recContr.Update(recCol[0].RecID, evid, 2, 0, RecEndType, RecStartDate,
                            rbRecOccNum.Checked ? RecEndAfter : (int?)null, 1, rbRecEndDate.Checked ? RecEndDate : (DateTime?)null,
                            (int?)(DaysOfWeek.Monday | DaysOfWeek.Tuesday | DaysOfWeek.Wednesday | DaysOfWeek.Thursday | DaysOfWeek.Friday), null, null, "2");
                    }
                    else //Create a new 
                    {
                        recContr.Insert(evid, 2, 0, RecEndType, RecStartDate,
                            rbRecOccNum.Checked ? RecEndAfter : (int?)null, 1, rbRecEndDate.Checked ? RecEndDate : (DateTime?)null,
                           (int?)(DaysOfWeek.Monday | DaysOfWeek.Tuesday | DaysOfWeek.Wednesday | DaysOfWeek.Thursday | DaysOfWeek.Friday), null, null, "2");
                    }

                    //    int index = -1;
                    //    for (int i = 0; i < ds.Tables["Recurrence"].Rows.Count; i++)
                    //    {
                    //        if (ds.Tables["Recurrence"].Rows[i]["ItemId"].ToString() == evid.ToString())
                    //        {
                    //            index = i;
                    //            break;
                    //        }
                    //    }
                    //    DataRow dr = null;
                    //    if (index >= 0)
                    //        dr = ds.Tables["Recurrence"].Rows[index];
                    //    else
                    //    {
                    //        dr = ds.Tables["Recurrence"].NewRow();
                    //        dr["ItemId"] = evid;
                    //    }
                    //    dr["Pattern"] = 2;
                    //    dr["Subpattern"] = 0;
                    //    dr["EndType"] = RecEndType;
                    //    dr["StartDate"] = RecStartDate;
                    //    dr["EndDate"] = rbRecEndDate.Checked ? (object)RecEndDate : (object)DBNull.Value;
                    //    dr["EndAfter"] = rbRecOccNum.Checked ? (object)RecEndAfter : (object)DBNull.Value;
                    //    dr["Frequency"] = 1;
                    //    dr["WeekDays"] = DaysOfWeek.Monday | DaysOfWeek.Tuesday | DaysOfWeek.Wednesday | DaysOfWeek.Thursday | DaysOfWeek.Friday;
                    //    dr["DayOfMonth"] = DBNull.Value;
                    //    dr["WeekNum"] = DBNull.Value;
                    //    dr["Comment"] = "2";

                    //    if (index < 0)
                    //        ds.Tables["Recurrence"].Rows.Add(dr);
                    //    ds.Tables["Recurrence"].AcceptChanges();
                    //}
                    #endregion
                    break;
                case "3"://on Mon Wed Fri
                    #region on Mon Wed Fri
                    if (recCol.Count > 0) // Check whether any recurrence exist with the Course
                    {
                        recContr.Update(recCol[0].RecID, evid, 2, 0, RecEndType, RecStartDate,
                            rbRecOccNum.Checked ? RecEndAfter : (int?)null, 1, rbRecEndDate.Checked ? RecEndDate : (DateTime?)null,
                            (int?)(DaysOfWeek.Monday | DaysOfWeek.Wednesday | DaysOfWeek.Friday), null, null, "3");
                    }
                    else //Create a new 
                    {
                        recContr.Insert(evid, 2, 0, RecEndType, RecStartDate,
                            rbRecOccNum.Checked ? RecEndAfter : (int?)null, 1, rbRecEndDate.Checked ? RecEndDate : (DateTime?)null,
                           (int?)(DaysOfWeek.Monday | DaysOfWeek.Wednesday | DaysOfWeek.Friday), null, null, "3");
                    }
                    //else//dataset
                    //{
                    //    int index = -1;
                    //    for (int i = 0; i < ds.Tables["Recurrence"].Rows.Count; i++)
                    //    {
                    //        if (ds.Tables["Recurrence"].Rows[i]["ItemId"].ToString() == evid.ToString())
                    //        {
                    //            index = i;
                    //            break;
                    //        }
                    //    }
                    //    DataRow dr = null;
                    //    if (index >= 0)
                    //        dr = ds.Tables["Recurrence"].Rows[index];
                    //    else
                    //    {
                    //        dr = ds.Tables["Recurrence"].NewRow();
                    //        dr["ItemId"] = evid;
                    //    }
                    //    dr["Pattern"] = 2;
                    //    dr["Subpattern"] = 0;
                    //    dr["EndType"] = RecEndType;
                    //    dr["StartDate"] = RecStartDate;
                    //    dr["EndDate"] = rbRecEndDate.Checked ? (object)RecEndDate : (object)DBNull.Value;
                    //    dr["EndAfter"] = rbRecOccNum.Checked ? (object)RecEndAfter : (object)DBNull.Value;
                    //    dr["Frequency"] = 1;
                    //    dr["WeekDays"] = DaysOfWeek.Monday | DaysOfWeek.Wednesday | DaysOfWeek.Friday;
                    //    dr["DayOfMonth"] = DBNull.Value;
                    //    dr["WeekNum"] = DBNull.Value;
                    //    dr["Comment"] = "3";

                    //    if (index < 0)
                    //        ds.Tables["Recurrence"].Rows.Add(dr);
                    //    ds.Tables["Recurrence"].AcceptChanges();
                    //}
                    #endregion
                    break;
                case "4"://on Tue Thu
                    #region on Tue Thu
                    if (recCol.Count > 0) // Check whether any recurrence exist with the Course
                    {
                        recContr.Update(recCol[0].RecID, evid, 2, 0, RecEndType, RecStartDate,
                            rbRecOccNum.Checked ? RecEndAfter : (int?)null, 1, rbRecEndDate.Checked ? RecEndDate : (DateTime?)null,
                            (int?)(DaysOfWeek.Tuesday | DaysOfWeek.Thursday), null, null, "4");
                    }
                    else //Create a new 
                    {
                        recContr.Insert(evid, 2, 0, RecEndType, RecStartDate,
                            rbRecOccNum.Checked ? RecEndAfter : (int?)null, 1, rbRecEndDate.Checked ? RecEndDate : (DateTime?)null,
                            (int?)(DaysOfWeek.Tuesday | DaysOfWeek.Thursday), null, null, "4");
                    }
                    //    int index = -1;
                    //    for (int i = 0; i < ds.Tables["Recurrence"].Rows.Count; i++)
                    //    {
                    //        if (ds.Tables["Recurrence"].Rows[i]["ItemId"].ToString() == evid.ToString())
                    //        {
                    //            index = i;
                    //            break;
                    //        }
                    //    }
                    //    DataRow dr = null;
                    //    if (index >= 0)
                    //        dr = ds.Tables["Recurrence"].Rows[index];
                    //    else
                    //    {
                    //        dr = ds.Tables["Recurrence"].NewRow();
                    //        dr["ItemId"] = evid;
                    //    }
                    //    dr["Pattern"] = 2;
                    //    dr["Subpattern"] = 0;
                    //    dr["EndType"] = RecEndType;
                    //    dr["StartDate"] = RecStartDate;
                    //    dr["EndDate"] = rbRecEndDate.Checked ? (object)RecEndDate : (object)DBNull.Value;
                    //    dr["EndAfter"] = rbRecOccNum.Checked ? (object)RecEndAfter : (object)DBNull.Value;
                    //    dr["Frequency"] = 1;
                    //    dr["WeekDays"] = DaysOfWeek.Tuesday | DaysOfWeek.Thursday;
                    //    dr["DayOfMonth"] = DBNull.Value;
                    //    dr["WeekNum"] = DBNull.Value;
                    //    dr["Comment"] = "4";

                    //    if (index < 0)
                    //        ds.Tables["Recurrence"].Rows.Add(dr);
                    //    ds.Tables["Recurrence"].AcceptChanges();
                    //}
                    #endregion
                    break;
                case "5"://weekly
                    #region weekly
                    if (recCol.Count > 0) // Check whether any recurrence exist with the Course
                    {
                        recContr.Update(recCol[0].RecID, evid, 2, 0, RecEndType, RecStartDate,
                            rbRecOccNum.Checked ? RecEndAfter : (int?)null, 1, rbRecEndDate.Checked ? RecEndDate : (DateTime?)null,
                            ((byte)0 | (cbWeeklySunday.Checked ? (byte)DaysOfWeek.Sunday : (byte)0) |
                                        (cbWeeklyMonday.Checked ? (byte)DaysOfWeek.Monday : (byte)0) |
                                        (cbWeeklyTuesday.Checked ? (byte)DaysOfWeek.Tuesday : (byte)0) |
                                        (cbWeeklyWednesday.Checked ? (byte)DaysOfWeek.Wednesday : (byte)0) |
                                        (cbWeeklyThursday.Checked ? (byte)DaysOfWeek.Thursday : (byte)0) |
                                        (cbWeeklyFriday.Checked ? (byte)DaysOfWeek.Friday : (byte)0) |
                                        (cbWeeklySaturday.Checked ? (byte)DaysOfWeek.Saturday : (byte)0)), null, null, null);
                    }
                    else //Create a new 
                    {
                        recContr.Insert(evid, 2, 0, RecEndType, RecStartDate,
                            rbRecOccNum.Checked ? RecEndAfter : (int?)null, 1, rbRecEndDate.Checked ? RecEndDate : (DateTime?)null,
                            ((byte)0 | (cbWeeklySunday.Checked ? (byte)DaysOfWeek.Sunday : (byte)0) |
                                        (cbWeeklyMonday.Checked ? (byte)DaysOfWeek.Monday : (byte)0) |
                                        (cbWeeklyTuesday.Checked ? (byte)DaysOfWeek.Tuesday : (byte)0) |
                                        (cbWeeklyWednesday.Checked ? (byte)DaysOfWeek.Wednesday : (byte)0) |
                                        (cbWeeklyThursday.Checked ? (byte)DaysOfWeek.Thursday : (byte)0) |
                                        (cbWeeklyFriday.Checked ? (byte)DaysOfWeek.Friday : (byte)0) |
                                        (cbWeeklySaturday.Checked ? (byte)DaysOfWeek.Saturday : (byte)0)), null, null, null);
                    }
                    //else//dataset
                    //{
                    //    int index = -1;
                    //    for (int i = 0; i < ds.Tables["Recurrence"].Rows.Count; i++)
                    //    {
                    //        if (ds.Tables["Recurrence"].Rows[i]["ItemId"].ToString() == evid.ToString())
                    //        {
                    //            index = i;
                    //            break;
                    //        }
                    //    }
                    //    DataRow dr = null;
                    //    if (index >= 0)
                    //        dr = ds.Tables["Recurrence"].Rows[index];
                    //    else
                    //    {
                    //        dr = ds.Tables["Recurrence"].NewRow();
                    //        dr["ItemId"] = evid;
                    //    }
                    //    dr["Pattern"] = 2;
                    //    dr["Subpattern"] = 0;
                    //    dr["EndType"] = RecEndType;
                    //    dr["StartDate"] = RecStartDate;
                    //    dr["EndDate"] = rbRecEndDate.Checked ? (object)RecEndDate : (object)DBNull.Value;
                    //    dr["EndAfter"] = rbRecOccNum.Checked ? (object)RecEndAfter : (object)DBNull.Value;
                    //    dr["Frequency"] = 1;
                    //    dr["WeekDays"] = (object)((byte)0 | (cbWeeklySunday.Checked ? (byte)DaysOfWeek.Sunday : (byte)0) |
                    //                    (cbWeeklyMonday.Checked ? (byte)DaysOfWeek.Monday : (byte)0) |
                    //                    (cbWeeklyTuesday.Checked ? (byte)DaysOfWeek.Tuesday : (byte)0) |
                    //                    (cbWeeklyWednesday.Checked ? (byte)DaysOfWeek.Wednesday : (byte)0) |
                    //                    (cbWeeklyThursday.Checked ? (byte)DaysOfWeek.Thursday : (byte)0) |
                    //                    (cbWeeklyFriday.Checked ? (byte)DaysOfWeek.Friday : (byte)0) |
                    //                    (cbWeeklySaturday.Checked ? (byte)DaysOfWeek.Saturday : (byte)0));
                    //    dr["DayOfMonth"] = DBNull.Value;
                    //    dr["WeekNum"] = DBNull.Value;
                    //    dr["Comment"] = DBNull.Value;

                    //    if (index < 0)
                    //        ds.Tables["Recurrence"].Rows.Add(dr);
                    //    ds.Tables["Recurrence"].AcceptChanges();
                    //}
                    #endregion
                    break;
                case "6"://monthly
                    #region Monthly
                    if (rbMonthlyRecMonthDay.Checked)//subpattern1 - DayOfMonth
                    {
                        RecDayOfMonth = EventStartDate.Day;
                    }
                    else//subpattern2 - Day in month week
                    {
                        RecWeekNum = (int)GetWeekNumber(EventStartDate);
                        RecWeekDays = (byte)GetDayOfWeekBit(EventStartDate);
                    }
                    if (recCol.Count > 0) // Check whether any recurrence exist with the Course
                    {
                        recContr.Update(recCol[0].RecID, evid, 3, rbMonthlyRecMonthDay.Checked ? 0 : 1, RecEndType, RecStartDate,
                            rbRecOccNum.Checked ? RecEndAfter : (int?)null, int.Parse(tbMonthlyRec.Text), rbRecEndDate.Checked ? RecEndDate : (DateTime?)null,
                            rbMonthlyRecMonthDay.Checked ? (int?)null : RecWeekDays, rbMonthlyRecMonthDay.Checked ? RecDayOfMonth : (int?)null,
                            rbMonthlyRecMonthDay.Checked ? (int?)null : RecWeekNum, null);
                    }
                    else //Create a new 
                    {
                        recContr.Insert(evid, 3, rbMonthlyRecMonthDay.Checked ? 0 : 1, RecEndType, RecStartDate,
                            rbRecOccNum.Checked ? RecEndAfter : (int?)null, int.Parse(tbMonthlyRec.Text), rbRecEndDate.Checked ? RecEndDate : (DateTime?)null,
                            rbMonthlyRecMonthDay.Checked ? (int?)null : RecWeekDays, rbMonthlyRecMonthDay.Checked ? RecDayOfMonth : (int?)null,
                            rbMonthlyRecMonthDay.Checked ? (int?)null : RecWeekNum, null);
                    }
                    //else//dataset
                    //{
                    //    int index = -1;
                    //    for (int i = 0; i < ds.Tables["Recurrence"].Rows.Count; i++)
                    //    {
                    //        if (ds.Tables["Recurrence"].Rows[i]["ItemId"].ToString() == evid.ToString())
                    //        {
                    //            index = i;
                    //            break;
                    //        }
                    //    }
                    //    DataRow dr = null;
                    //    if (index >= 0)
                    //        dr = ds.Tables["Recurrence"].Rows[index];
                    //    else
                    //    {
                    //        dr = ds.Tables["Recurrence"].NewRow();
                    //        dr["ItemId"] = evid;
                    //    }
                    //    dr["Pattern"] = 3;
                    //    dr["Subpattern"] = rbMonthlyRecMonthDay.Checked ? 0 : 1;
                    //    dr["EndType"] = RecEndType;
                    //    dr["StartDate"] = RecStartDate;
                    //    dr["EndDate"] = rbRecEndDate.Checked ? (object)RecEndDate : (object)DBNull.Value;
                    //    dr["EndAfter"] = rbRecOccNum.Checked ? (object)RecEndAfter : (object)DBNull.Value;
                    //    dr["Frequency"] = int.Parse(tbMonthlyRec.Text);
                    //    dr["WeekDays"] = rbMonthlyRecMonthDay.Checked ? (object)DBNull.Value : (object)RecWeekDays;
                    //    dr["DayOfMonth"] = rbMonthlyRecMonthDay.Checked ? (object)RecDayOfMonth : (object)DBNull.Value;
                    //    dr["WeekNum"] = rbMonthlyRecMonthDay.Checked ? (object)DBNull.Value : (object)RecWeekNum;
                    //    dr["Comment"] = DBNull.Value;

                    //    if (index < 0)
                    //        ds.Tables["Recurrence"].Rows.Add(dr);
                    //    ds.Tables["Recurrence"].AcceptChanges();
                    //}
                    #endregion
                    break;
                case "7"://yearly
                    #region Yearly
                    if (recCol.Count > 0) // Check whether any recurrence exist with the Course
                    {
                        recContr.Update(recCol[0].RecID, evid, 4, rbMonthlyRecMonthDay.Checked ? 0 : 1, RecEndType, RecStartDate,
                            rbRecOccNum.Checked ? RecEndAfter : (int?)null, 1, rbRecEndDate.Checked ? RecEndDate : (DateTime?)null,
                            rbYearlyRecMonthDay.Checked ? int.Parse(ddYearlyRecMonth.SelectedValue) : int.Parse(ddYearlyRecWeekDay.SelectedValue),
                            rbYearlyRecMonthDay.Checked ? int.Parse(tbYearlyRecMonthDay.Text) : int.Parse(ddYearlyRecMonth.SelectedValue),
                            rbYearlyRecMonthDay.Checked ? (int?)null : int.Parse(ddYearlyRecWeekNum.SelectedValue),
                            null);
                    }
                    else //Create a new 
                    {
                        recContr.Insert(evid, 4, rbMonthlyRecMonthDay.Checked ? 0 : 1, RecEndType, RecStartDate,
                            rbRecOccNum.Checked ? RecEndAfter : (int?)null, 1, rbRecEndDate.Checked ? RecEndDate : (DateTime?)null,
                            rbYearlyRecMonthDay.Checked ? int.Parse(ddYearlyRecMonth.SelectedValue) : int.Parse(ddYearlyRecWeekDay.SelectedValue),
                            rbYearlyRecMonthDay.Checked ? int.Parse(tbYearlyRecMonthDay.Text) : int.Parse(ddYearlyRecMonth.SelectedValue),
                            rbYearlyRecMonthDay.Checked ? (int?)null : int.Parse(ddYearlyRecWeekNum.SelectedValue), null);
                    }
                    //else//dataset
                    //{
                    //    int index = -1;
                    //    for (int i = 0; i < ds.Tables["Recurrence"].Rows.Count; i++)
                    //    {
                    //        if (ds.Tables["Recurrence"].Rows[i]["ItemId"].ToString() == evid.ToString())
                    //        {
                    //            index = i;
                    //            break;
                    //        }
                    //    }
                    //    DataRow dr = null;
                    //    if (index >= 0)
                    //        dr = ds.Tables["Recurrence"].Rows[index];
                    //    else
                    //    {
                    //        dr = ds.Tables["Recurrence"].NewRow();
                    //        dr["ItemId"] = evid;
                    //    }
                    //    dr["Pattern"] = 4;
                    //    dr["Subpattern"] = rbYearlyRecMonthDay.Checked ? 0 : 1;
                    //    dr["EndType"] = RecEndType;
                    //    dr["StartDate"] = RecStartDate;
                    //    dr["EndDate"] = rbRecEndDate.Checked ? (object)RecEndDate : (object)DBNull.Value;
                    //    dr["EndAfter"] = rbRecOccNum.Checked ? (object)RecEndAfter : (object)DBNull.Value;
                    //    dr["Frequency"] = 1;
                    //    dr["WeekDays"] = rbYearlyRecMonthDay.Checked ? int.Parse(ddYearlyRecMonth.SelectedValue) : int.Parse(ddYearlyRecWeekDay.SelectedValue);
                    //    dr["DayOfMonth"] = rbYearlyRecMonthDay.Checked ? int.Parse(tbYearlyRecMonthDay.Text) : int.Parse(ddYearlyRecMonth.SelectedValue);
                    //    dr["WeekNum"] = rbYearlyRecMonthDay.Checked ? (object)DBNull.Value : int.Parse(ddYearlyRecWeekNum.SelectedValue);
                    //    dr["Comment"] = DBNull.Value;

                    //    if (index < 0)
                    //        ds.Tables["Recurrence"].Rows.Add(dr);
                    //    ds.Tables["Recurrence"].AcceptChanges();
                    //}
                    #endregion
                    break;
                default:
                    break;
            }
            //if (!DBHelper.IsSQL)
            //    SaveDatasetToXml();
            Response.Redirect("~/CalendarDefault.aspx");
        }

        #region Load/save from/to dataset
        //private bool SaveDatasetToXml()
        //{
        //    try
        //    {
        //        ds.WriteXml(HttpContext.Current.Request.MapPath(_itemsVFileName));
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}

        private bool LoadData()
        {
            ds.Clear();
            try
            {
                CourseController itemContr = new CourseController();
                CourseCollection itemCol = itemContr.FetchAll();
                DataTable dt = itemCol.ToDataTable();
                dt.TableName = "Items";
                ds.Tables.Add(dt);
                RecurrenceController recContr = new RecurrenceController();
                RecurrenceCollection recCol = recContr.FetchAll();
                DataTable recTable = recCol.ToDataTable();
                recTable.TableName = "Recurrence";
                ds.Tables.Add(recTable);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        private WeekNumber GetWeekNumber(DateTime day)
        {
            WeekNumber ret;
            int i = -1;
            int d = day.Day;
            while (d > 0)
            {
                d -= 7;
                i++;
            }
            ret = (WeekNumber)Enum.Parse(typeof(WeekNumber), i.ToString());
            return ret;
        }

        private DaysOfWeek GetDayOfWeekBit(DateTime day)
        {
            switch (day.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    return DaysOfWeek.Sunday;
                case DayOfWeek.Monday:
                    return DaysOfWeek.Monday;
                case DayOfWeek.Tuesday:
                    return DaysOfWeek.Tuesday;
                case DayOfWeek.Wednesday:
                    return DaysOfWeek.Wednesday;
                case DayOfWeek.Thursday:
                    return DaysOfWeek.Thursday;
                case DayOfWeek.Friday:
                    return DaysOfWeek.Friday;
                case DayOfWeek.Saturday:
                    return DaysOfWeek.Saturday;
                default:
                    return DaysOfWeek.Sunday;
            }
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/CalendarDefault.aspx");
        }

        void lbBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/CalendarDefault.aspx");
        }

        private void BindData()
        {
            CalendarItem ci = null;
            ddStartHour.Items.Clear();
            ddStartMinute.Items.Clear();
            for (int i = 0; i < 24; i++)
            {
                ddStartHour.Items.Add(new ListItem(i < 10 ? "0" + i.ToString() : i.ToString(), i.ToString()));
            }
            for (int i = 0; i < 60; i++)
            {
                ddStartMinute.Items.Add(new ListItem(i < 10 ? "0" + i.ToString() : i.ToString(), i.ToString()));
            }
            ddEndHour.Items.Clear();
            ddEndMinute.Items.Clear();
            for (int i = 0; i < 24; i++)
            {
                ddEndHour.Items.Add(new ListItem(i < 10 ? "0" + i.ToString() : i.ToString(), i.ToString()));
            }
            for (int i = 0; i < 60; i++)
            {
                ddEndMinute.Items.Add(new ListItem(i < 10 ? "0" + i.ToString() : i.ToString(), i.ToString()));
            }
            if (EventId > 0)
            {
                Page.Title = "Edit Event";
                CourseController courseContr = new CourseController();
                CourseCollection courseCol = courseContr.FetchByID(EventId);
                DateTime isd = courseCol[0].StartDate.Value;
                DateTime ied = courseCol[0].EndDate.Value;
                ci = new CalendarItem(courseCol[0].Id.ToString(), courseCol[0].CourseName,
                    isd.ToString("yyyy.M.d.H.m.s"), ied.ToString("yyyy.M.d.H.m.s"),
                    courseCol[0].Description, courseCol[0].IsAllDay.Value, null);
                EventStartDate = isd;
                tbTitle.Text = ci.Title.Replace("&nbsp;", "");
                ddStartHour.SelectedValue = isd.Hour.ToString();
                ddStartMinute.SelectedValue = isd.Minute.ToString();
                ddEndHour.SelectedValue = ied.Hour.ToString();
                ddEndMinute.SelectedValue = ied.Minute.ToString();
                tbStartDate.Text = isd.ToShortDateString();
                tbEndDate.Text = ied.ToShortDateString();
                tbDescription.Text = ci.Description.Replace("&nbsp;", "");
                cbIsAllDay.Checked = ci.IsAllDay;
                if (cbIsAllDay.Checked)
                {
                    ddStartHour.Visible = false;
                    ddStartMinute.Visible = false;
                    ddEndHour.Visible = false;
                    ddEndMinute.Visible = false;
                }
                else
                {
                    ddStartHour.Visible = true;
                    ddStartMinute.Visible = true;
                    ddEndHour.Visible = true;
                    ddEndMinute.Visible = true;
                }
                tbPlace.SelectedValue = courseCol[0].Place.ToString();
                ddlCourseCat.SelectedValue = courseCol[0].CourseType;
            }
            else//EventId<0
            {
                Page.Title = "Create Event";
                if (!IsPostBack)
                {
                    DateTime sd = DateTime.MinValue;
                    DateTime ed = DateTime.MinValue;
                    bool isAllDay = false;
                    if (Request["StartDate"] != null && DateTime.TryParse(Request["StartDate"].ToString(), System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out sd))
                    {
                        tbStartDate.Text = sd.ToShortDateString();
                        ddStartHour.SelectedValue = sd.Hour.ToString();
                        ddStartMinute.SelectedValue = sd.Minute.ToString();
                    }
                    if (Request["EndDate"] != null && DateTime.TryParse(Request["EndDate"].ToString(), System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out ed))
                    {
                        tbEndDate.Text = ed.ToShortDateString();
                        ddEndHour.SelectedValue = ed.Hour.ToString();
                        ddEndMinute.SelectedValue = ed.Minute.ToString();
                    }
                    if (Request["IsAllDay"] != null && bool.TryParse(Request["IsAllDay"].ToString(), out isAllDay))
                    {
                        cbIsAllDay.Checked = isAllDay;
                        if (cbIsAllDay.Checked)
                        {
                            ddStartHour.Visible = false;
                            ddStartMinute.Visible = false;
                            ddEndHour.Visible = false;
                            ddEndMinute.Visible = false;
                        }
                    }
                    if (Request["Title"] != null && Request["Title"].Length > 0)
                    {
                        tbTitle.Text = Request["Title"];
                    }
                }
            }
        }

        protected void BindRecurrence()
        {
            if (!IsPostBack)//load from database
            {
                CourseController courseContr = new CourseController();
                if (EventId > 0)
                {
                    CourseCollection courseCol = courseContr.FetchByID(EventId);
                    RecurrenceCollection recCol = courseCol[0].RecurrenceRecords();
                    if (recCol.Count > 0)
                    {
                        RecPat = recCol[0].Pattern.Value;
                        RecSubPat = recCol[0].SubPattern.Value;
                        RecEndType = recCol[0].EndType.Value;
                        RecStartDate = recCol[0].StartDate.Value;
                        if (recCol[0].EndDate.HasValue)
                            RecEndDate = recCol[0].EndDate.Value;
                        if (recCol[0].EndAfter.HasValue)
                            RecEndAfter = recCol[0].EndAfter.Value;
                        if (recCol[0].Frequency.HasValue)
                            RecFrequency = recCol[0].Frequency.Value;
                        if (recCol[0].WeekDays.HasValue)
                            RecWeekDays = (byte)recCol[0].WeekDays.Value;
                        if (recCol[0].DayofMonth.HasValue)
                            RecDayOfMonth = recCol[0].DayofMonth.Value;
                        if (recCol[0].WeekNum.HasValue)
                            RecWeekNum = recCol[0].WeekNum.Value;
                        if (recCol[0].Comment != "")
                            RecCom = Convert.ToInt32(recCol[0].Comment);
                    }
                }
            }
            if (RecPat == 0)
            {
                ddRecType.SelectedValue = "0";
                trDailyRec.Visible = false;
                trWorkingDaysRec.Visible = false;
                tr135Rec.Visible = false;
                tr24Rec.Visible = false;
                trWeeklyRec.Visible = false;
                trMonthlyRec.Visible = false;
                trYearlyRec.Visible = false;
                trRecEndType.Visible = false;
            }
            if (EventId > 0)
            {
                Page.Title = "Edit Event";
                if (RecPat == 1)//daily recurrence
                {
                    ddRecType.SelectedValue = "1";
                    trDailyRec.Visible = true;
                    tbDailyRec.Text = RecFrequency.ToString();
                    if (RecFrequency > 1)
                        lbDailyRec.Text = RecFrequency.ToString() + " ";
                    else
                        lbDailyRec.Text = " ";
                    trWorkingDaysRec.Visible = false;
                    tr135Rec.Visible = false;
                    tr24Rec.Visible = false;
                    trWeeklyRec.Visible = false;
                    trMonthlyRec.Visible = false;
                    trYearlyRec.Visible = false;
                    trRecEndType.Visible = true;
                }
                if (RecPat == 2)//weekly recurrence
                {
                    if (RecCom == 0)//weekly
                    {
                        ddRecType.SelectedValue = "5";
                        trDailyRec.Visible = false;
                        trWorkingDaysRec.Visible = false;
                        tr135Rec.Visible = false;
                        tr24Rec.Visible = false;
                        trWeeklyRec.Visible = true;
                        tbWeeklyRec.Text = RecFrequency.ToString();
                        if (RecWeekDays > 0)
                        {
                            if ((RecWeekDays & (byte)DaysOfWeek.Sunday) > 0)
                                cbWeeklySunday.Checked = true;
                            if ((RecWeekDays & (byte)DaysOfWeek.Monday) > 0)
                                cbWeeklyMonday.Checked = true;
                            if ((RecWeekDays & (byte)DaysOfWeek.Tuesday) > 0)
                                cbWeeklyTuesday.Checked = true;
                            if ((RecWeekDays & (byte)DaysOfWeek.Wednesday) > 0)
                                cbWeeklyWednesday.Checked = true;
                            if ((RecWeekDays & (byte)DaysOfWeek.Thursday) > 0)
                                cbWeeklyThursday.Checked = true;
                            if ((RecWeekDays & (byte)DaysOfWeek.Friday) > 0)
                                cbWeeklyFriday.Checked = true;
                            if ((RecWeekDays & (byte)DaysOfWeek.Saturday) > 0)
                                cbWeeklySaturday.Checked = true;
                        }
                        else
                        {
                            if (EventStartDate.DayOfWeek == DayOfWeek.Sunday)
                                cbWeeklySunday.Checked = true;
                            else
                                cbWeeklySunday.Checked = false;
                            if (EventStartDate.DayOfWeek == DayOfWeek.Monday)
                                cbWeeklyMonday.Checked = true;
                            else
                                cbWeeklyMonday.Checked = false;
                            if (EventStartDate.DayOfWeek == DayOfWeek.Tuesday)
                                cbWeeklyTuesday.Checked = true;
                            else
                                cbWeeklyTuesday.Checked = false;
                            if (EventStartDate.DayOfWeek == DayOfWeek.Wednesday)
                                cbWeeklyWednesday.Checked = true;
                            else
                                cbWeeklyWednesday.Checked = false;
                            if (EventStartDate.DayOfWeek == DayOfWeek.Thursday)
                                cbWeeklyThursday.Checked = true;
                            else
                                cbWeeklyThursday.Checked = false;
                            if (EventStartDate.DayOfWeek == DayOfWeek.Friday)
                                cbWeeklyFriday.Checked = true;
                            else
                                cbWeeklyFriday.Checked = false;
                            if (EventStartDate.DayOfWeek == DayOfWeek.Saturday)
                                cbWeeklySaturday.Checked = true;
                            else
                                cbWeeklySaturday.Checked = false;
                        }
                        trMonthlyRec.Visible = false;
                        trYearlyRec.Visible = false;
                        trRecEndType.Visible = true;
                    }
                    if (RecCom == 2)//working days
                    {
                        ddRecType.SelectedValue = "2";
                        trDailyRec.Visible = false;
                        trWorkingDaysRec.Visible = true;
                        tr135Rec.Visible = false;
                        tr24Rec.Visible = false;
                        trWeeklyRec.Visible = false;
                        trMonthlyRec.Visible = false;
                        trYearlyRec.Visible = false;
                        trRecEndType.Visible = true;
                    }
                    if (RecCom == 3)//on Mon Wed Fri
                    {
                        ddRecType.SelectedValue = "3";
                        trDailyRec.Visible = false;
                        trWorkingDaysRec.Visible = false;
                        tr135Rec.Visible = true;
                        tr24Rec.Visible = false;
                        trWeeklyRec.Visible = false;
                        trMonthlyRec.Visible = false;
                        trYearlyRec.Visible = false;
                        trRecEndType.Visible = true;
                    }
                    if (RecCom == 4)//on Tue Thu
                    {
                        ddRecType.SelectedValue = "4";
                        trDailyRec.Visible = false;
                        trWorkingDaysRec.Visible = false;
                        tr135Rec.Visible = false;
                        tr24Rec.Visible = true;
                        trWeeklyRec.Visible = false;
                        trMonthlyRec.Visible = false;
                        trYearlyRec.Visible = false;
                        trRecEndType.Visible = true;
                    }
                }
                if (RecPat == 3)//monthly recurrence
                {
                    ddRecType.SelectedValue = "6";
                    trDailyRec.Visible = false;
                    trWorkingDaysRec.Visible = false;
                    tr135Rec.Visible = false;
                    tr24Rec.Visible = false;
                    trWeeklyRec.Visible = false;
                    trMonthlyRec.Visible = true;
                    tbMonthlyRec.Text = RecFrequency.ToString();
                    if (RecSubPat == 0)
                        rbMonthlyRecMonthDay.Checked = true;
                    else
                        rbMonthlyRecWeekDay.Checked = true;
                    trYearlyRec.Visible = false;
                    trRecEndType.Visible = true;
                }
                if (RecPat == 4)//yearly recurrence
                {
                    ddRecType.SelectedValue = "7";
                    trDailyRec.Visible = false;
                    trWorkingDaysRec.Visible = false;
                    tr135Rec.Visible = false;
                    tr24Rec.Visible = false;
                    trWeeklyRec.Visible = false;
                    trMonthlyRec.Visible = false;
                    trYearlyRec.Visible = true;
                    trRecEndType.Visible = true;
                    if (RecSubPat == 0)
                    {
                        rbYearlyRecMonthDay.Checked = true;
                        ddYearlyRecMonth.SelectedValue = RecWeekDays.ToString();
                        tbYearlyRecMonthDay.Text = RecDayOfMonth.ToString();
                    }
                    else
                    {
                        rbYearlyRecWeekDay.Checked = true;
                        ddYearlyRecWeekDay.SelectedValue = RecWeekDays.ToString();
                        ddYearlyRecMonth.SelectedValue = RecDayOfMonth.ToString();
                        ddYearlyRecWeekNum.SelectedValue = RecWeekNum.ToString();
                    }
                }

                tbRecStartDate.Text = EventStartDate.ToShortDateString();
                switch (RecEndType)
                {
                    case 0://never
                        rbRecNeverEnd.Checked = true;
                        break;
                    case 1://end date
                        rbRecEndDate.Checked = true;
                        tbRecEndDate.Text = RecEndDate.ToShortDateString();
                        break;
                    case 2://number of occurences
                        rbRecOccNum.Checked = true;
                        tbRecOccNum.Text = RecEndAfter.ToString();
                        break;
                    default:
                        rbRecOccNum.Checked = true;
                        tbRecOccNum.Text = RecEndAfter.ToString();
                        break;
                }
            }
            else
            {
                Page.Title = "Create Event";
                if (RecPat == 1)//daily recurrence
                {
                    ddRecType.SelectedValue = "1";
                    trDailyRec.Visible = true;
                    tbDailyRec.Text = "1";
                    trWorkingDaysRec.Visible = false;
                    tr135Rec.Visible = false;
                    tr24Rec.Visible = false;
                    trWeeklyRec.Visible = false;
                    trMonthlyRec.Visible = false;
                    trYearlyRec.Visible = false;
                    trRecEndType.Visible = true;
                }
                if (RecPat == 2)//weekly recurrence
                {
                    if (RecCom == 0)//weekly
                    {
                        ddRecType.SelectedValue = "5";
                        trDailyRec.Visible = false;
                        trWorkingDaysRec.Visible = false;
                        tr135Rec.Visible = false;
                        tr24Rec.Visible = false;
                        trWeeklyRec.Visible = true;
                        tbWeeklyRec.Text = "1";
                        trMonthlyRec.Visible = false;
                        trYearlyRec.Visible = false;
                        trRecEndType.Visible = true;
                    }
                    if (RecCom == 2)//working days
                    {
                        ddRecType.SelectedValue = "2";
                        trDailyRec.Visible = false;
                        trWorkingDaysRec.Visible = true;
                        tr135Rec.Visible = false;
                        tr24Rec.Visible = false;
                        trWeeklyRec.Visible = false;
                        trMonthlyRec.Visible = false;
                        trYearlyRec.Visible = false;
                        trRecEndType.Visible = true;
                    }
                    if (RecCom == 3)//on Mon Wed Fri
                    {
                        ddRecType.SelectedValue = "3";
                        trDailyRec.Visible = false;
                        trWorkingDaysRec.Visible = false;
                        tr135Rec.Visible = true;
                        tr24Rec.Visible = false;
                        trWeeklyRec.Visible = false;
                        trMonthlyRec.Visible = false;
                        trYearlyRec.Visible = false;
                        trRecEndType.Visible = true;
                    }
                    if (RecCom == 4)//on Tue Thu
                    {
                        ddRecType.SelectedValue = "4";
                        trDailyRec.Visible = false;
                        trWorkingDaysRec.Visible = false;
                        tr135Rec.Visible = false;
                        tr24Rec.Visible = true;
                        trWeeklyRec.Visible = false;
                        trMonthlyRec.Visible = false;
                        trYearlyRec.Visible = false;
                        trRecEndType.Visible = true;
                    }
                }
                if (RecPat == 3)//monthly recurrence
                {
                    ddRecType.SelectedValue = "6";
                    trDailyRec.Visible = false;
                    trWorkingDaysRec.Visible = false;
                    tr135Rec.Visible = false;
                    tr24Rec.Visible = false;
                    trWeeklyRec.Visible = false;
                    trMonthlyRec.Visible = true;
                    rbMonthlyRecMonthDay.Checked = true;
                    tbMonthlyRec.Text = "1";
                    trYearlyRec.Visible = false;
                    trRecEndType.Visible = true;
                }
                if (RecPat == 4)//yearly recurrence
                {
                    ddRecType.SelectedValue = "7";
                    trDailyRec.Visible = false;
                    trWorkingDaysRec.Visible = false;
                    tr135Rec.Visible = false;
                    tr24Rec.Visible = false;
                    trWeeklyRec.Visible = false;
                    trMonthlyRec.Visible = false;
                    trYearlyRec.Visible = true;
                    trRecEndType.Visible = true;
                    rbYearlyRecMonthDay.Checked = true;
                    tbYearlyRecMonthDay.Text = "1";
                }

                tbRecStartDate.Text = DateTime.Now.ToShortDateString();
                rbRecOccNum.Checked = true;
                tbRecOccNum.Text = "1";
            }
        }

        protected bool ValidatePage()
        {
            #region Validate item start and end date

            #endregion

            if (ddRecType.SelectedValue == "0")//no recurrence
                return true;

            #region Validate recurrence range and end type
            if (rbRecEndDate.Checked)
            {
                DateTime rsd = DateTime.MinValue;
                DateTime red = DateTime.MinValue;
                if (!DateTime.TryParse(tbRecStartDate.Text, out rsd))
                {
                    lbError.Text = "You should specify recurrence start date in correct format.";
                    return false;
                }
                if (rbRecEndDate.Checked)//recurrence end date
                {
                    if (!DateTime.TryParse(tbRecEndDate.Text, out red))
                    {
                        lbError.Text = "You should specify recurrence end date in correct format.";
                        return false;
                    }
                    if (red.Date <= rsd.Date)
                    {
                        lbError.Text = "Recurrence end date must be greater than recurrence start date.";
                        return false;
                    }
                }
                if (rbRecOccNum.Checked)
                {
                    int recNum = 0;
                    if (!int.TryParse(tbRecOccNum.Text, out recNum) || recNum < 1)
                    {
                        lbError.Text = "You should specify number of occurences in correct format.";
                        return false;
                    }
                }
            }
            #endregion

            int freq = 0;
            switch (ddRecType.SelectedValue)
            {
                case "1"://daily
                    if (!int.TryParse(tbDailyRec.Text, out freq) || freq < 1)
                    {
                        lbError.Text = "You should specify daily recurrence frequency in correct format.";
                        return false;
                    }
                    break;
                case "2"://working days
                case "3"://mon wed fri
                case "4"://tue thu
                    break;
                case "5"://weekly
                    if (!int.TryParse(tbWeeklyRec.Text, out freq) || freq < 1)
                    {
                        lbError.Text = "You should specify weekly recurrence frequency in correct format.";
                        return false;
                    }
                    if (!cbWeeklySunday.Checked && !cbWeeklyMonday.Checked &&
                        !cbWeeklyTuesday.Checked && !cbWeeklyWednesday.Checked &&
                        !cbWeeklyThursday.Checked && !cbWeeklyFriday.Checked && !cbWeeklySaturday.Checked)
                    {
                        lbError.Text = "You should check at least one week day in weekly recurrence.";
                        return false;
                    }
                    break;
                case "6"://monthly
                    if (rbYearlyRecMonthDay.Checked && (!int.TryParse(tbMonthlyRec.Text, out freq) || freq < 1))
                    {
                        lbError.Text = "You should specify monthly recurrence frequency in correct format.";
                        return false;
                    }
                    break;
                case "7"://yearly
                    if (rbYearlyRecMonthDay.Checked && (!int.TryParse(tbYearlyRecMonthDay.Text, out freq) || freq < 1))
                    {
                        lbError.Text = "You should specify yearly recurrence frequency in correct format.";
                        return false;
                    }
                    break;
            }
            return true;
        }
    }

