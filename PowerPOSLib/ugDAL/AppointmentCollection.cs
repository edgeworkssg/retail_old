using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Google.GData.Calendar;
using Google.GData.Client;
using Google.GData.Extensions;
using System.Data;

namespace PowerPOS
{
    [Serializable]
    public partial class AppointmentCollection : List<Appointment>
    {
        #region -= Settings =-
        private const string format_DateTime = "MMM dd, yyyy";
        private const string NoMemberText = "[Walk In]";
        #endregion

        private string Username;
        private string Password;

        #region -= Constructor =-
        public AppointmentCollection() { }
        public AppointmentCollection(string Username, string Password) { this.Username = Username; this.Password = Password; }
        public void SetCredential(string Username, string Password) { this.Username = Username; this.Password = Password; }
        #endregion

        public void Load(DateTime StartDate, DateTime EndDate, string SalesPerson)
        {
            this.Clear();

            CalendarService service;
            #region *) Retrieve: Get Google Calendar's Service 
            service = GoogleController.GetCalendarServices(Username, Password);
            #endregion

            string GoogleCalendarURL;
            #region *) Retrieve: GoogleCalendar's URL - From UserMst Database 
            UserMst currSalesPerson = new UserMst(SalesPerson);
            GoogleCalendarURL = currSalesPerson.GoogleCalendarURL;
            #endregion

            #region *) Fix Parameter: Start Date & End Date 
            StartDate = StartDate.Date;
            EndDate = EndDate.Date.AddDays(1);
            #endregion

            EventFeed calFeed = null;
            #region *) Retrieve: all Events - From GoogleCalendar 
            EventQuery query = new EventQuery();
            query.Uri = new Uri(GoogleCalendarURL);
            query.StartTime = StartDate;
            query.EndTime = EndDate;

            try
            {
                calFeed = service.Query(query) as EventFeed;
            }
            catch (Google.GData.Client.GDataRequestException X)
            {
                Logger.writeLog(X);
                throw new Exception("(error)Cannot login to google");
            }
            #endregion

            #region *) Process Data: Convert to Appointment Collection - Save to inMemory Variable 
            while (calFeed != null && calFeed.Entries.Count > 0)
            {
                foreach (EventEntry entry in calFeed.Entries.Cast<EventEntry>())
                {
                    if (entry.Times.Count > 0)
                    {
                        this.Add(new Appointment(entry, SalesPerson));
                    }
                }
                // just query the same query again.
                if (calFeed.NextChunk != null)
                {
                    query.Uri = new Uri(calFeed.NextChunk);
                    calFeed = service.Query(query) as EventFeed;
                }
                else
                    calFeed = null;
            }
            #endregion
        }

        public void Load(DateTime ApointmentDate, string SalesPersonID)
        {
        }

        public static int sortByDate(Appointment first, Appointment second)
        {
            if (first == null)
            {
                if (second == null)
                {
                    // If first is null and second is null, they're
                    // equal. 
                    return 0;
                }
                else
                {
                    // If first is null and second is not null, second
                    // is greater. 
                    return -1;
                }
            }
            else
            {
                // If first is not null...
                //
                if (second == null)
                // ...and second is null, first is greater.
                {
                    return 1;
                }
                else
                {
                    int Output = first.AppointmentTime.CompareTo(second.AppointmentTime);

                    if (Output == 0)
                    {
                        return first.Duration - second.Duration;
                    }
                    else
                    {
                        return Output;
                    }
                }
            }
        }

        public DataTable ToDataTable()
        {
            DataTable Output = new DataTable("Output");
            Output.Columns.Add("ID", Type.GetType("System.String"));
            Output.Columns.Add("MembershipNo", Type.GetType("System.String"));
            Output.Columns.Add("MembershipName", Type.GetType("System.String"));
            Output.Columns.Add("RoomID", Type.GetType("System.Int32"));
            Output.Columns.Add("RoomName", Type.GetType("System.String"));
            Output.Columns.Add("AppointmentTime", Type.GetType("System.DateTime"));
            Output.Columns.Add("Duration", Type.GetType("System.Int32"));

            for (int Counter = 0; Counter < this.Count; Counter++)
            {
                Appointment currApp = this[Counter];
                DataRow Rw = Output.NewRow();
                Rw[0] = currApp.ID;
                Rw[1] = currApp.MembershipNo;
               
                Membership currMember = currApp.Membership;
                if (currMember != null) Rw[2] = currMember.NameToAppear;
                
                Rw[3] = currApp.RoomID;
                
                Room currRoom = currApp.Room;
                if (currRoom != null) Rw[4] = currApp.Room.RoomName;
                
                Rw[5] = currApp.AppointmentTime;
                Rw[6] = currApp.Duration;
                Output.Rows.Add(Rw);
            }

            return Output;
        }

        public DataTable ToCalendarDataTable(DateTime StartDate, DateTime EndDate, int StartHour, int EndHour, int TimeSlotDuration)
        {
            if (this.Count < 1)
                return null;

            DataTable Output = new DataTable();

            #region *) Insert Columns
            Output.Columns.Add("Time", Type.GetType("System.String"));

            for (DateTime Counter = StartDate.Date; Counter.CompareTo(EndDate.Date) <= 0; Counter = Counter.AddDays(1))
            {
                Output.Columns.Add(Counter.ToString(format_DateTime), Type.GetType("System.String"));
            }
            #endregion

            #region *) Insert Rows
            for (DateTime Counter = StartDate.Date.AddHours(StartHour);
                Counter.AddMinutes(TimeSlotDuration).CompareTo(StartDate.Date.AddHours(EndHour)) <= 0;
                Counter = Counter.AddMinutes(TimeSlotDuration))
            {
                DataRow newRow = Output.NewRow();
                newRow[0] = Counter.ToString("HH:mm");

                for (int Counter2 = 1; Counter2 < Output.Columns.Count; Counter2++)
                {

                    List<Appointment> selAppointment = new List<Appointment>(this.Where(Fnc
                        => Fnc.AppointmentTime.Date.ToString(format_DateTime) == Output.Columns[Counter2].ColumnName
                        && Fnc.AppointmentTime.AddMinutes(Fnc.Duration).Hour >= Counter.Hour && Fnc.AppointmentTime.AddMinutes(Fnc.Duration).Minute >= Counter.Minute
                        && Fnc.AppointmentTime.Hour <= Counter.AddMinutes(TimeSlotDuration).Hour && Fnc.AppointmentTime.Minute <= Counter.AddMinutes(TimeSlotDuration).Minute));

                    if (selAppointment == null || selAppointment.Count<1)
                    {
                        newRow[Counter2] = null;
                    }
                    else
                    {
                        newRow[Counter2] = "";
                        foreach (Appointment oneAppointment in selAppointment)
                            newRow[Counter2] += (string.IsNullOrEmpty(newRow[Counter2].ToString()) ? "" : ", ") + oneAppointment.ID;
                    }
                }

                Output.Rows.Add(newRow);

            }
            #endregion

            return Output;
        }

        public DataTable ToCalendarDataTable_showMemberName(DateTime StartDate, DateTime EndDate, int StartHour, int EndHour, int TimeSlotDuration)
        {
            if (this.Count < 1)
                return null;

            DataTable Output = new DataTable();

            #region *) Insert Columns 
            Output.Columns.Add("Time", Type.GetType("System.String"));

            for (DateTime Counter = StartDate.Date; Counter.CompareTo(EndDate.Date) <= 0; Counter = Counter.AddDays(1))
            {
                Output.Columns.Add(Counter.ToString(format_DateTime), Type.GetType("System.String"));
            }
            #endregion

            #region *) Insert Rows 
            for (DateTime Counter = StartDate.Date.AddHours(StartHour);
                Counter.AddMinutes(TimeSlotDuration).CompareTo(StartDate.Date.AddHours(EndHour)) < 0;
                Counter = Counter.AddMinutes(TimeSlotDuration))
            {
                DataRow newRow = Output.NewRow();
                newRow[0] = Counter.ToString("HH:mm");
                
                for (int Counter2 = 1; Counter2 < Output.Columns.Count; Counter2++)
                {
                    List<Appointment> selAppointment = new List<Appointment>(this.Where(Fnc
                        => Fnc.AppointmentTime.Date.ToString(format_DateTime) == Output.Columns[Counter2].ColumnName
                        && Fnc.AppointmentTime.AddMinutes(Fnc.Duration).CompareTo(new DateTime(Fnc.AppointmentTime.Year, Fnc.AppointmentTime.Month, Fnc.AppointmentTime.Day, Counter.Hour, Counter.Minute, 0)) > 0
                        && Fnc.AppointmentTime.CompareTo(new DateTime(Fnc.AppointmentTime.Year, Fnc.AppointmentTime.Month, Fnc.AppointmentTime.Day, Counter.AddMinutes(TimeSlotDuration).Hour, Counter.AddMinutes(TimeSlotDuration).Minute, 0)) < 0));

                    if (selAppointment == null || selAppointment.Count < 1)
                    {
                        newRow[Counter2] = null;
                    }
                    else
                    {
                        newRow[Counter2] = "";
                        foreach (Appointment oneAppointment in selAppointment)
                        {
                            //Membership selMember = oneAppointment.Membership;
                            Membership selMember = Membership.FetchByID(oneAppointment.MembershipNo);
                            if (selMember == null || string.IsNullOrEmpty(selMember.NameToAppear))
                            {
                                newRow[Counter2] += (string.IsNullOrEmpty(newRow[Counter2].ToString()) ? "" : ", ") + NoMemberText;
                            }
                            else
                            {
                                newRow[Counter2] += (string.IsNullOrEmpty(newRow[Counter2].ToString()) ? "" : ", ") + selMember.NameToAppear;
                            }
                        }
                    }
                }

                Output.Rows.Add(newRow);

            }
            #endregion

            return Output;
        }
    }

}
