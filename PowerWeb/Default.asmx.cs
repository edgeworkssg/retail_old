using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using PowerPOS;
namespace WebSample
{
	using System;
	using System.Web;
	using System.Collections;
	using System.Web.Services;
	using System.Web.Services.Protocols;
	using System.Collections.Generic;
	using System.Web.Script.Services;
	using System.Xml;
	using Mediachase.AjaxCalendar;
	//using MCAjaxCalendar;
	using System.Data;
    using SubSonic;



	/// <summary>
	/// Summary description for _Default
	/// </summary>
	/// 
	[ScriptService]
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class Default : System.Web.Services.WebService, IItemsWebServiceInterface
	{

        private DataSet ds = new DataSet();
		public Default()
		{

			//Uncomment the following line if using designed components 
			//InitializeComponent(); 

		}

		/*protected int CompareCalendarItemByStartDate(CalendarItem it1, CalendarItem it2)
		{
			string[] date = it1.StartDate.Split(new char[] { '.' });
			DateTime d1 = new DateTime(int.Parse(date[0]), int.Parse(date[1]), int.Parse(date[2]), int.Parse(date[3]), int.Parse(date[4]), int.Parse(date[5]));
			date = it2.StartDate.Split(new char[] { '.' });
			DateTime d2 = new DateTime(int.Parse(date[0]), int.Parse(date[1]), int.Parse(date[2]), int.Parse(date[3]), int.Parse(date[4]), int.Parse(date[5]));
			TimeSpan ts = d1 - d2;
			return (int)ts.TotalMinutes;
		}*/

		#region IItemsWebServiceInterface Members

        [WebMethod]
        public CalendarItem[] LoadItems(string startDate, string endDate, object calendarExtension)
        {
            List<CalendarItem> al = new List<CalendarItem>();
            DateTime viewStartDate = DateTime.Now;
            DateTime viewEndDate = DateTime.Now.AddMinutes(1);
            try
            {
                string[] date = startDate.Split(new char[] { '.' });
                viewStartDate = new DateTime(int.Parse(date[0]), int.Parse(date[1]), int.Parse(date[2]), 0, 0, 0);
                date = endDate.Split(new char[] { '.' });
                viewEndDate = new DateTime(int.Parse(date[0]), int.Parse(date[1]), int.Parse(date[2]), 23, 59, 59);
            }
            catch (Exception e)
            {
            }
            LoadData();
            foreach (DataRow dr in ds.Tables["Items"].Rows)
            {
                CalendarItem it = new CalendarItem(dr["Id"].ToString(), dr["CourseName"].ToString(), ((DateTime)dr["StartDate"]).ToString("yyyy.M.d.H.m.s"), ((DateTime)dr["EndDate"]).ToString("yyyy.M.d.H.m.s"), dr["Description"].ToString(), (bool)dr["IsAllDay"], false);
                DateTime isd = (DateTime)dr["StartDate"];
                DateTime ied = (DateTime)dr["EndDate"];

                int sh = isd.Hour;
                int sm = isd.Minute;

                int itemId = int.Parse(it.Uid);
                RecurrenceLogic rec = null;
                ArrayList recDates = new ArrayList();
                foreach (DataRow recDr in ds.Tables["Recurrence"].Rows)
                {
                    int recItemId = (int)recDr["ItemId"];
                    if (recItemId == itemId)
                    {
                        rec = new RecurrenceLogic();
                        rec.Pattern = (RecurrencePattern)recDr["Pattern"];
                        rec.SubPattern = (RecurrenceSubPattern)recDr["SubPattern"];
                        rec.EndType = (RecurrenceEndType)recDr["EndType"];
                        rec.StartDate = (DateTime)recDr["StartDate"];
                        if (recDr["EndDate"] != DBNull.Value)
                            rec.EndDate = (DateTime)recDr["EndDate"];
                        if (recDr["EndAfter"] != DBNull.Value)
                            rec.EndAfter = (int)recDr["EndAfter"];
                        if (recDr["Frequency"] != DBNull.Value)
                            rec.Frequency = (int)recDr["Frequency"];
                        if (recDr["WeekDays"] != DBNull.Value)
                            rec.WeekDays = (byte)((int)recDr["WeekDays"]);
                        if (recDr["DayOfMonth"] != DBNull.Value)
                            rec.DayOfMonth = (byte)((int)recDr["DayOfMonth"]);
                        if (recDr["WeekNum"] != DBNull.Value)
                            rec.WeekNum = (WeekNumber)recDr["WeekNum"];
                        if (recDr["Comment"] != DBNull.Value)
                            rec.Comment = recDr["Comment"].ToString();
                        recDates = rec.GetRecurrenceDates(viewStartDate, viewEndDate, isd);
                        it.Extensions = (object)true;

                    }
                }
                if (recDates.Count > 0)
                {
                    TimeSpan ts = ied - isd;
                    for (int i = 0; i < recDates.Count; i++)
                    {
                        DateTime dt = (DateTime)recDates[i];
                        DateTime st = new DateTime(dt.Year, dt.Month, dt.Day, sh, sm, 0);
                        DateTime et = st.AddSeconds(ts.TotalSeconds);
                        if (isd.Date == st.Date) continue;
                        CalendarItem ci = new CalendarItem(it.Uid, it.Title, st.ToString("yyyy.M.d.H.m.s"), et.ToString("yyyy.M.d.H.m.s"), it.Description, it.IsAllDay, true);
                        al.Add(ci);
                    }
                }
                if ((isd >= viewStartDate && isd <= viewEndDate) ||
                    (ied >= viewStartDate && ied <= viewEndDate) ||
                    (isd < viewStartDate && ied > viewEndDate))
                {
                    al.Add(it);
                }
            }
            //al.Sort(CompareCalendarItemByStartDate);
            return al.ToArray();
        }

		[WebMethod]
		public void DeleteItem(string uid, object calendarExtension)
		{
			CourseController courseContr = new CourseController();
            courseContr.Destroy(uid);
		}
        [WebMethod]
        public void CreateItem(string title, string startDate, string endDate, string description, bool isAllDay, object extensions, object calendarExtension)
        {
            DateTime isd = DateTime.Now;
            DateTime ied = DateTime.Now.AddMinutes(1);
            try
            {
                string[] date = startDate.Split(new char[] { '.' });
                isd = new DateTime(int.Parse(date[0]), int.Parse(date[1]), int.Parse(date[2]), int.Parse(date[3]), int.Parse(date[4]), int.Parse(date[5]));
                date = endDate.Split(new char[] { '.' });
                ied = new DateTime(int.Parse(date[0]), int.Parse(date[1]), int.Parse(date[2]), int.Parse(date[3]), int.Parse(date[4]), int.Parse(date[5]));
            }
            catch (Exception e)
            {
                Logger.writeLog(e);
            }

            CourseController courseContr = new CourseController();
            if (description == "&nbsp;")
                description = "";
            courseContr.Insert(title, "", description, "G001", isd, ied, isAllDay, null);
        }

		[WebMethod]
		public void UpdateItem(string uid, string title, string startDate, string endDate, 
            string description, bool isAllDay, object extensions, object calendarExtension)
		{
			DateTime isd = DateTime.Now;
			DateTime ied = DateTime.Now.AddMinutes(1);
			try
			{
				string[] date = startDate.Split(new char[] { '.' });
				isd = new DateTime(int.Parse(date[0]), int.Parse(date[1]), int.Parse(date[2]), int.Parse(date[3]), int.Parse(date[4]), int.Parse(date[5]));
				date = endDate.Split(new char[] { '.' });
				ied = new DateTime(int.Parse(date[0]), int.Parse(date[1]), int.Parse(date[2]), int.Parse(date[3]), int.Parse(date[4]), int.Parse(date[5]));
			}
			catch (Exception e)
			{
                Logger.writeLog(e);
			}
            CourseController courseContr = new CourseController();
            if (description == "&nbsp;")
                description = "";
            //courseContr.Update(Convert.ToInt32(uid), title, CourseType, description, "", isd, ied, isAllDay, null);
            Query qr = Course.CreateQuery();
            qr.QueryType = QueryType.Update;
            qr.AddWhere(Course.Columns.Id, Convert.ToInt32(uid));
            qr.AddUpdateSetting(Course.Columns.StartDate, isd);
            qr.AddUpdateSetting(Course.Columns.EndDate, ied);
            qr.Execute();
		}
		#endregion

   		public bool LoadData()
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
				//ds.ReadXml(HttpContext.Current.Request.MapPath(_itemsVFileName));
				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}
		//#endregion
	}
}
