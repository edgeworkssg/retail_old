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

using PowerPOS;

	public partial class EditorPages_OutlookEventEdit : System.Web.UI.Page
	{
		private DataSet ds = new DataSet();
		//private string _itemsVFileName = HttpContext.Current.Request.ApplicationPath + "/CalendarItems.xml";

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
			get { return _recPat; }
			set { _recPat = value; }
		}

		private int _recSubPat = 0;
		public int RecSubPat
		{
			get { return _recSubPat; }
			set { _recSubPat = value; }
		}

		private int _recEndType = 2;//number of occurences
		public int RecEndType
		{
			get { return _recEndType; }
			set { _recEndType = value; }
		}

		private int _recCom = 0;
		public int RecCom
		{
			get { return _recCom; }
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

		protected void Page_Load(object sender, EventArgs e)
		{
			LoadData();
			if (EventId > 0)
			{
				Title = "Edit event";
			}
			else
			{
				Title = "Create event";
			}
			if (!IsPostBack)
			{
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
			this.rblRecType.SelectedIndexChanged += new EventHandler(rblRecType_SelectedIndexChanged);
			this.lbCancel.Click += new EventHandler(lbCancel_Click);
			this.lbSave.Click += new EventHandler(lbSave_Click);
			this.lbDelete.Click += new EventHandler(lbDelete_Click);
		}

		void lbDelete_Click(object sender, EventArgs e)
		{
			if (EventId > 0)
			{
                CourseController courseCntr = new CourseController();
                courseCntr.Destroy(EventId);
			}

            Response.Redirect("~/CalendarDefault.aspx");
		}

		void lbSave_Click(object sender, EventArgs e)
		{
			if (!ValidatePage())
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
			string description = tbDescription.Text.Trim() == "" ? "&nbsp;" : tbDescription.Text.Trim();
			int place = Convert.ToInt32(tbPlace.SelectedValue);
			bool isAllDay = cbIsAllDay.Checked;

			int evid = EventId;
            CourseController courseCntr = new CourseController();
			if (EventId > 0)
			{
                courseCntr.Update(EventId, title, "", description, "", sDate, eDate, isAllDay, place);
			}
			else
			{
                evid = courseCntr.InsertWithID(title, description, "", sDate, eDate, isAllDay, place);
			}

            RecurrenceController recContr = new RecurrenceController();
            RecurrenceCollection recCol = courseCntr.FetchByID(evid)[0].RecurrenceRecords();

			if (rblRecType.Items[0].Selected)//no recurrence
			{
                if (recCol.Count > 0) // Check whether any recurrence exist with the Course
                {
                    recContr.Destroy(recCol[0].RecID);
                }
				Response.Redirect("~/CalendarDefault.aspx");
			}

			//save recurrence
            
			#region Define end type
			if (!rblRecType.Items[0].Selected)
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

			if (rblRecType.Items[1].Selected)//daily recurrence
			{
				if(recCol.Count > 0) // Check whether any recurrence exist with the Course
					{
                        recContr.Update(recCol[0].RecID, evid, rbRecDailyDay.Checked ? 1 : 2, 0, RecEndType, RecStartDate,
                            rbRecOccNum.Checked ? RecEndAfter : (int?)null, rbRecDailyDay.Checked ? int.Parse(tbDailyRec.Text) : 1, rbRecEndDate.Checked ? RecEndDate : (DateTime?)null,
                            null, null, null, rbRecDailyDay.Checked ? null : "2");
                    }
                    else //Create a new 
                    {
                        recContr.Insert(evid, rbRecDailyDay.Checked ? 1 : 2, 0, RecEndType, RecStartDate,
                            rbRecOccNum.Checked ? RecEndAfter : (int?)null, rbRecDailyDay.Checked ? int.Parse(tbDailyRec.Text) : 1, rbRecEndDate.Checked ? RecEndDate : (DateTime?)null,
                            null, null, null, rbRecDailyDay.Checked ? null : "2");
                    }
             	Response.Redirect("~/CalendarDefault.aspx");
			}
			if (rblRecType.Items[2].Selected)//weekly recurrence
			{
                if (recCol.Count > 0) // Check whether any recurrence exist with the Course
                {
                    recContr.Update(recCol[0].RecID, evid, 2, 0, RecEndType, RecStartDate,
                        rbRecOccNum.Checked ? RecEndAfter : (int?)null, 1, rbRecEndDate.Checked ? RecEndDate : (DateTime?)null,
                        ((byte)0 | (cbRecWeeklySunday.Checked ? (byte)DaysOfWeek.Sunday : (byte)0) |
									(cbRecWeeklyMonday.Checked ? (byte)DaysOfWeek.Monday : (byte)0) |
									(cbRecWeeklyTuesday.Checked ? (byte)DaysOfWeek.Tuesday : (byte)0) |
									(cbRecWeeklyWednesday.Checked ? (byte)DaysOfWeek.Wednesday : (byte)0) |
									(cbRecWeeklyThursday.Checked ? (byte)DaysOfWeek.Thursday : (byte)0) |
									(cbRecWeeklyFriday.Checked ? (byte)DaysOfWeek.Friday : (byte)0) |
									(cbRecWeeklySaturday.Checked ? (byte)DaysOfWeek.Saturday : (byte)0)),
                                    null, null, null);
                }
                else //Create a new 
                {
                    recContr.Insert(evid, 2, 0, RecEndType, RecStartDate,
                        rbRecOccNum.Checked ? RecEndAfter : (int?)null, 1, rbRecEndDate.Checked ? RecEndDate : (DateTime?)null,
                        ((byte)0 | (cbRecWeeklySunday.Checked ? (byte)DaysOfWeek.Sunday : (byte)0) |
									(cbRecWeeklyMonday.Checked ? (byte)DaysOfWeek.Monday : (byte)0) |
									(cbRecWeeklyTuesday.Checked ? (byte)DaysOfWeek.Tuesday : (byte)0) |
									(cbRecWeeklyWednesday.Checked ? (byte)DaysOfWeek.Wednesday : (byte)0) |
									(cbRecWeeklyThursday.Checked ? (byte)DaysOfWeek.Thursday : (byte)0) |
									(cbRecWeeklyFriday.Checked ? (byte)DaysOfWeek.Friday : (byte)0) |
									(cbRecWeeklySaturday.Checked ? (byte)DaysOfWeek.Saturday : (byte)0)),
                                    null, null, null);
                }


                Response.Redirect("~/CalendarDefault.aspx");
			}
			if (rblRecType.Items[3].Selected)//monthly recurrence
			{
				if (rbRecMonthDayNum.Checked)//subpattern1 - DayOfMonth
				{
					RecDayOfMonth = int.Parse(tbRecMonthDayNum.Text);
				}
				else//subpattern2 - Day in month week
				{
					RecWeekNum = int.Parse(ddRecMonthWeekNum.SelectedValue);
					RecWeekDays = byte.Parse(ddRecMonthWeekDay.SelectedValue);
				}

                if (recCol.Count > 0) // Check whether any recurrence exist with the Course
                {
                    recContr.Update(recCol[0].RecID, evid, 3, rbRecMonthDayNum.Checked ? 0 : 1, RecEndType, RecStartDate,
                        rbRecOccNum.Checked ? RecEndAfter : (int?)null, int.Parse(tbRecMonthFreq.Text), rbRecEndDate.Checked ? RecEndDate : (DateTime?)null,
                        rbRecMonthDayNum.Checked ? (int?) null : RecWeekDays,
                                    rbRecMonthDayNum.Checked ? RecDayOfMonth : (int?)null,
                                    rbRecMonthDayNum.Checked ? (int?)null : RecWeekNum, null);
                }
                else //Create a new 
                {
                    recContr.Insert(evid, 3, rbRecMonthDayNum.Checked ? 0 : 1, RecEndType, RecStartDate,
                        rbRecOccNum.Checked ? RecEndAfter : (int?)null, int.Parse(tbRecMonthFreq.Text), rbRecEndDate.Checked ? RecEndDate : (DateTime?)null,
                        rbRecMonthDayNum.Checked ? (int?) null : RecWeekDays,
                                    rbRecMonthDayNum.Checked ? RecDayOfMonth : (int?)null,
                                    rbRecMonthDayNum.Checked ? (int?)null : RecWeekNum, null);
                }
				Response.Redirect("~/CalendarDefault.aspx");
			}
			if (rblRecType.Items[4].Selected)//yearly recurrence
			{
                if (recCol.Count > 0) // Check whether any recurrence exist with the Course
                {
                    recContr.Update(recCol[0].RecID, evid, 4, rbRecYearDay.Checked ? 0 : 1, RecEndType, RecStartDate,
                        rbRecOccNum.Checked ? RecEndAfter : (int?)null, 1, rbRecEndDate.Checked ? RecEndDate : (DateTime?)null,
                        rbRecYearDay.Checked ? int.Parse(ddRecYearMonth.SelectedValue) : int.Parse(ddRecYearWeekDay.SelectedValue),
                                    rbRecYearDay.Checked ? int.Parse(tbRecYearDayNum.Text) : int.Parse(ddRecYearMonth.SelectedValue),
                                    rbRecYearDay.Checked ? (int?)null : int.Parse(ddRecYearWeekNum.SelectedValue), null);
                }
                else //Create a new 
                {
                    recContr.Insert(evid, 4, rbRecYearDay.Checked ? 0 : 1, RecEndType, RecStartDate,
                        rbRecOccNum.Checked ? RecEndAfter : (int?)null, 1, rbRecEndDate.Checked ? RecEndDate : (DateTime?)null,
                        rbRecYearDay.Checked ? int.Parse(ddRecYearMonth.SelectedValue) : int.Parse(ddRecYearWeekDay.SelectedValue),
                                    rbRecYearDay.Checked ? int.Parse(tbRecYearDayNum.Text) : int.Parse(ddRecYearMonth.SelectedValue),
                                    rbRecYearDay.Checked ? (int?)null : int.Parse(ddRecYearWeekNum.SelectedValue), null);
                }
				Response.Redirect("~/CalendarDefault.aspx");
			}
            Response.Redirect("~/CalendarDefault.aspx");
		}

		void lbCancel_Click(object sender, EventArgs e)
		{
            Response.Redirect("~/CalendarDefault.aspx");
		}

		void rblRecType_SelectedIndexChanged(object sender, EventArgs e)
		{
			lbError.Visible = false;
			switch (rblRecType.SelectedValue)
			{
				case "0"://no recurrence
					RecPat = 0;
					break;
				case "1"://daily recurrence
					RecPat = 1;
					break;
				case "2"://weekly
					RecPat = 2;
					RecCom = 0;
					break;
				case "3"://Monthly
					RecPat = 3;
					break;
				case "4"://yearly
					RecPat = 4;
					break;
				default:
					RecPat = 0;
					break;
			}
			BindRecurrence();
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
               

            }
            else//EventId<0
            {
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
                }
            }
		}

		protected void BindRecurrence()
		{
            if (!IsPostBack)
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
				rblRecType.Items[0].Selected = true;
				tdRecDaily.Visible = false;
				tdRecWeekly.Visible = false;
				tdRecMonthly.Visible = false;
				tdRecYearly.Visible = false;
				trRecEndType.Visible = false;
			}

			if (EventId > 0)
			{
				Page.Title = "Edit Event";
				if (RecPat == 1)//daily recurrence
				{
					rblRecType.Items[1].Selected = true;
					tdRecDaily.Visible = true;
					tbDailyRec.Text = RecFrequency.ToString();
					tdRecWeekly.Visible = false;
					tdRecMonthly.Visible = false;
					tdRecYearly.Visible = false;
					rbRecDailyDay.Checked = true;
					trRecEndType.Visible = true;
				}
				if (RecPat == 2)//weekly recurrence
				{
					if (RecCom == 0)//weekly
					{
						rblRecType.Items[2].Selected = true;
						tdRecDaily.Visible = false;
						tdRecWeekly.Visible = true;
						tdRecMonthly.Visible = false;
						tdRecYearly.Visible = false;
						tbWeeklyRec.Text = RecFrequency.ToString();
						if (RecWeekDays > 0)
						{
							if ((RecWeekDays & (byte)DaysOfWeek.Sunday) > 0)
								cbRecWeeklySunday.Checked = true;
							if ((RecWeekDays & (byte)DaysOfWeek.Monday) > 0)
								cbRecWeeklyMonday.Checked = true;
							if ((RecWeekDays & (byte)DaysOfWeek.Tuesday) > 0)
								cbRecWeeklyTuesday.Checked = true;
							if ((RecWeekDays & (byte)DaysOfWeek.Wednesday) > 0)
								cbRecWeeklyWednesday.Checked = true;
							if ((RecWeekDays & (byte)DaysOfWeek.Thursday) > 0)
								cbRecWeeklyThursday.Checked = true;
							if ((RecWeekDays & (byte)DaysOfWeek.Friday) > 0)
								cbRecWeeklyFriday.Checked = true;
							if ((RecWeekDays & (byte)DaysOfWeek.Saturday) > 0)
								cbRecWeeklySaturday.Checked = true;
						}
						else
						{
							if (EventStartDate.DayOfWeek == DayOfWeek.Sunday)
								cbRecWeeklySunday.Checked = true;
							else
								cbRecWeeklySunday.Checked = false;
							if (EventStartDate.DayOfWeek == DayOfWeek.Monday)
								cbRecWeeklyMonday.Checked = true;
							else
								cbRecWeeklyMonday.Checked = false;
							if (EventStartDate.DayOfWeek == DayOfWeek.Tuesday)
								cbRecWeeklyTuesday.Checked = true;
							else
								cbRecWeeklyTuesday.Checked = false;
							if (EventStartDate.DayOfWeek == DayOfWeek.Wednesday)
								cbRecWeeklyWednesday.Checked = true;
							else
								cbRecWeeklyWednesday.Checked = false;
							if (EventStartDate.DayOfWeek == DayOfWeek.Thursday)
								cbRecWeeklyThursday.Checked = true;
							else
								cbRecWeeklyThursday.Checked = false;
							if (EventStartDate.DayOfWeek == DayOfWeek.Friday)
								cbRecWeeklyFriday.Checked = true;
							else
								cbRecWeeklyFriday.Checked = false;
							if (EventStartDate.DayOfWeek == DayOfWeek.Saturday)
								cbRecWeeklySaturday.Checked = true;
							else
								cbRecWeeklySaturday.Checked = false;
						}
						trRecEndType.Visible = true;
					}
					if (RecCom == 2)//working days
					{
						rblRecType.Items[1].Selected = true;
						tdRecDaily.Visible = true;
						tdRecWeekly.Visible = false;
						tdRecMonthly.Visible = false;
						tdRecYearly.Visible = false;
						rbRecDailyWorkingDays.Checked = true;
						trRecEndType.Visible = true;
					}
					if (RecCom == 3)//on Mon Wed Fri
					{
						rblRecType.Items[2].Selected = true;
						tdRecDaily.Visible = false;
						tdRecWeekly.Visible = true;
						tdRecMonthly.Visible = false;
						tdRecYearly.Visible = false;

						cbRecWeeklySunday.Checked = false;
						cbRecWeeklyMonday.Checked = true;
						cbRecWeeklyTuesday.Checked = false;
						cbRecWeeklyWednesday.Checked = true;
						cbRecWeeklyThursday.Checked = false;
						cbRecWeeklyFriday.Checked = true;
						cbRecWeeklySaturday.Checked = false;

						tbWeeklyRec.Text = RecFrequency.ToString();
						trRecEndType.Visible = true;
					}
					if (RecCom == 4)//on Tue Thu
					{
						rblRecType.Items[2].Selected = true;
						tdRecDaily.Visible = false;
						tdRecWeekly.Visible = true;
						tdRecMonthly.Visible = false;
						tdRecYearly.Visible = false;

						cbRecWeeklySunday.Checked = false;
						cbRecWeeklyMonday.Checked = false;
						cbRecWeeklyTuesday.Checked = true;
						cbRecWeeklyWednesday.Checked = false;
						cbRecWeeklyThursday.Checked = true;
						cbRecWeeklyFriday.Checked = false;
						cbRecWeeklySaturday.Checked = false;

						tbWeeklyRec.Text = RecFrequency.ToString();
						trRecEndType.Visible = true;
					}
				}
				if (RecPat == 3)//monthly recurrence
				{
					rblRecType.Items[3].Selected = true;
					tdRecDaily.Visible = false;
					tdRecWeekly.Visible = false;
					tdRecMonthly.Visible = true;
					if (RecSubPat == 0)
					{
						rbRecMonthDayNum.Checked = true;
						tbRecMonthFreq.Text = RecFrequency.ToString();
						tbRecMonthDayNum.Text = RecDayOfMonth.ToString();
					}
					else
					{
						rbRecMonthWeekDay.Checked = true;
						ddRecMonthWeekDay.SelectedValue = RecWeekDays.ToString();
						tbRecMonthFreq.Text = RecFrequency.ToString();
						ddRecMonthWeekNum.SelectedValue = RecDayOfMonth.ToString();
					}
					tdRecYearly.Visible = false;
					trRecEndType.Visible = true;
				}
				if (RecPat == 4)//yearly recurrence
				{
					rblRecType.Items[4].Selected = true;
					tdRecDaily.Visible = false;
					tdRecWeekly.Visible = false;
					tdRecMonthly.Visible = false;
					tdRecYearly.Visible = true;
					trRecEndType.Visible = true;
					if (RecSubPat == 0)
					{
						rbRecYearDay.Checked = true;
						tbRecYearDayNum.Text = RecDayOfMonth.ToString();
						ddRecYearMonth.SelectedValue = RecWeekDays.ToString();
					}
					else
					{
						rbRecYearWeekDay.Checked = true;
						ddRecYearWeekDay.SelectedValue = RecWeekDays.ToString();
						ddRecYearMonth.SelectedValue = RecDayOfMonth.ToString();
						ddRecYearWeekNum.SelectedValue = RecWeekNum.ToString();
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
			}//EventId>0
			else
			{
				Page.Title = "Create Event";
				if (RecPat == 1)//daily
				{
					rblRecType.Items[1].Selected = true;
					tdRecDaily.Visible = true;
					tdRecWeekly.Visible = false;
					tdRecMonthly.Visible = false;
					tdRecYearly.Visible = false;
					rbRecDailyDay.Checked = true;
					trRecEndType.Visible = true;
					tbDailyRec.Text = "1";
				}
				if (RecPat == 2)//weekly
				{
					if (RecCom == 0)//clear weekly
					{
						rblRecType.Items[2].Selected = true;
						tdRecDaily.Visible = false;
						tdRecWeekly.Visible = true;
						tdRecMonthly.Visible = false;
						tdRecYearly.Visible = false;
						tbWeeklyRec.Text = "1";
						trRecEndType.Visible = true;
					}
					if (RecCom == 2)//working days
					{
						rblRecType.Items[1].Selected = true;
						tdRecDaily.Visible = true;
						tdRecWeekly.Visible = false;
						tdRecMonthly.Visible = false;
						tdRecYearly.Visible = false;
						rbRecDailyWorkingDays.Checked = true;
						trRecEndType.Visible = true;
					}
					if (RecCom == 3)//on Mon Wed Fri
					{
						rblRecType.Items[2].Selected = true;
						tdRecDaily.Visible = false;
						tdRecWeekly.Visible = true;
						tdRecMonthly.Visible = false;
						tdRecYearly.Visible = false;

						cbRecWeeklySunday.Checked = false;
						cbRecWeeklyMonday.Checked = true;
						cbRecWeeklyTuesday.Checked = false;
						cbRecWeeklyWednesday.Checked = true;
						cbRecWeeklyThursday.Checked = false;
						cbRecWeeklyFriday.Checked = true;
						cbRecWeeklySaturday.Checked = false;

						tbWeeklyRec.Text = "1";
						trRecEndType.Visible = true;
					}
					if (RecCom == 4)//on Tue Thu
					{
						rblRecType.Items[2].Selected = true;
						tdRecDaily.Visible = false;
						tdRecWeekly.Visible = true;
						tdRecMonthly.Visible = false;
						tdRecYearly.Visible = false;

						cbRecWeeklySunday.Checked = false;
						cbRecWeeklyMonday.Checked = false;
						cbRecWeeklyTuesday.Checked = true;
						cbRecWeeklyWednesday.Checked = false;
						cbRecWeeklyThursday.Checked = true;
						cbRecWeeklyFriday.Checked = false;
						cbRecWeeklySaturday.Checked = false;

						tbWeeklyRec.Text = "1";
						trRecEndType.Visible = true;
					}
				}
				if (RecPat == 3)//monthly recurrence
				{
					rblRecType.Items[3].Selected = true;
					tdRecDaily.Visible = false;
					tdRecWeekly.Visible = false;
					tdRecMonthly.Visible = true;
					rbRecMonthDayNum.Checked = true;
					tbRecMonthFreq.Text = RecFrequency.ToString();
					tbRecMonthDayNum.Text = RecDayOfMonth.ToString();
					tdRecYearly.Visible = false;
					trRecEndType.Visible = true;
				}
				if (RecPat == 4)//yearly recurrence
				{
					rblRecType.Items[4].Selected = true;
					tdRecDaily.Visible = false;
					tdRecWeekly.Visible = false;
					tdRecMonthly.Visible = false;
					tdRecYearly.Visible = true;
					trRecEndType.Visible = true;
					rbRecYearDay.Checked = true;
					tbRecYearDayNum.Text = DateTime.Now.Day.ToString();
					ddRecYearMonth.SelectedValue = DateTime.Now.Month.ToString();
				}
				tbRecStartDate.Text = DateTime.Now.ToShortDateString();
				rbRecOccNum.Checked = true;
				tbRecOccNum.Text = "1";
			}
		}

		protected bool ValidatePage()
		{
			DateTime sDate = DateTime.Now;
			DateTime eDate = DateTime.Now;
			int temp = 0;
			if (!DateTime.TryParse(tbStartDate.Text, out sDate))
			{
				lbError.Text = "You should specify event start date in correct format";
				return false;
			}
			if (!DateTime.TryParse(tbEndDate.Text, out eDate))
			{
				lbError.Text = "You should specify event end date in correct format";
				return false;
			}

			sDate = new DateTime(sDate.Year, sDate.Month, sDate.Day, int.Parse(ddStartHour.SelectedValue), int.Parse(ddStartMinute.SelectedValue), 0);
			eDate = new DateTime(eDate.Year, eDate.Month, eDate.Day, int.Parse(ddEndHour.SelectedValue), int.Parse(ddEndMinute.SelectedValue), 0);
			if (sDate > eDate)
			{
				lbError.Text = "Event start date should be less than event end date, or equal to it.";
				return false;
			}

			if (rblRecType.Items[0].Selected)//no recurrence
				return true;

			if (rblRecType.Items[1].Selected)//daily recurrence
			{
				if (rbRecDailyDay.Checked)
				{
					if (!int.TryParse(tbDailyRec.Text, out temp) || temp <= 0)
					{
						lbError.Text = "You should specify daily recurrence frequency in correct format.";
						return false;
					}
				}
			}
			if (rblRecType.Items[2].Selected)//weekly recurrence
			{
				if (!int.TryParse(tbWeeklyRec.Text, out temp) || temp <= 0)
				{
					lbError.Text = "You should specify weekly recurrence frequency in correct format.";
					return false;
				}
				if (!(cbRecWeeklyFriday.Checked || cbRecWeeklyMonday.Checked ||
					cbRecWeeklySaturday.Checked || cbRecWeeklySunday.Checked ||
					cbRecWeeklyThursday.Checked || cbRecWeeklyTuesday.Checked ||
					cbRecWeeklyWednesday.Checked))
				{
					lbError.Text = "You should select at least one week day in weekly recurrence.";
					return false;
				}
			}
			if (rblRecType.Items[3].Selected)//monthly recurrence
			{
				if (rbRecMonthDayNum.Checked)
				{
					if (!int.TryParse(tbRecMonthDayNum.Text, out temp) || temp <= 0)
					{
						lbError.Text = "You should specify month day in monthly recurrence in correct format.";
						return false;
					}
				}
				if (!int.TryParse(tbRecMonthFreq.Text, out temp) || temp <= 0)
				{
					lbError.Text = "You should specify frequency in monthly recurrence in correct format.";
					return false;
				}
			}
			if (rblRecType.Items[4].Selected)//yearly recurrence
			{
				if (rbRecMonthDayNum.Checked)
				{
					if (!int.TryParse(tbRecYearDayNum.Text, out temp) || temp <= 0 || temp > 31)
					{
						lbError.Text = "You should specify frequency in yearly recurrence in correct format.";
						return false;
					}
				}
			}

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
			return true;
		}

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
	}

