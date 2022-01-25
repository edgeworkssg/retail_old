using System;
using System.Drawing;

namespace AppointmentBook
{
	public class AppointmentBookControlOptions
	{
		public int PixelsPerHour = 150;
		public int TimeResolution = 10;
		public int TimeScaleWidth = 100;
		public int ScheduleColumnWidth = 150;
        public int DayInWeek = 7;
        public int TimeHeaderHeight = 60;
        public int HeaderHeight = 120;
        public int TimeSlotHeight = 24;
        public int TimeSlotMax = 4;
        public int MonthlyDateHeaderHeight = 11;
        public int MonthlyHeaderHeight = 30;
        public int MonthlyScheduleHeight = 107;
        public bool IsAppointmentView = true;

		public int TimeIntervalHeight
		{
			get { return PixelsPerHour * TimeResolution / 60; }
		}

		public TimeSpan WorkDayStart = TimeSpan.FromHours(10);
		public TimeSpan WorkDayEnd = TimeSpan.FromHours(19);

		public readonly Font TimeSlotFont = new Font("Tahoma", 9);
		public readonly Font TimeSlotSmallFont = new Font("Tahoma", 7.5f);
        public readonly Font TimeSlotVerySmallFont = new Font("Tahoma", 6.5f);
		public readonly Font TimeMajorFont = new Font("Tahoma", 10, FontStyle.Bold);
		public readonly Font TimeMinorFont = new Font("Tahoma", 10);


        public DateTime GetMonthlyStartDate(DateTime startTime)
        {
            DateTime theDate = new DateTime(startTime.Year, startTime.Month, 1);
            int firstDayOfMonth = (int)theDate.DayOfWeek;
            int startDayDiff = -1 * (firstDayOfMonth % this.DayInWeek);
            int endDayDiff = this.DayInWeek - 1 - ((int)theDate.AddMonths(1).AddDays(-1).DayOfWeek);
            DateTime theDateStart = theDate.AddDays(startDayDiff);
            DateTime theDateEnd = theDate.AddMonths(1).AddDays(-1).AddDays(endDayDiff);

            if ((theDateEnd - theDateStart).Days / this.DayInWeek == 4)
            {
                theDateEnd = theDateEnd.AddDays(7);
            }
            else if ((theDateEnd - theDateStart).Days / this.DayInWeek == 3)
            {
                theDateStart = theDateStart.AddDays(-7);
                theDateEnd = theDateEnd.AddDays(7);
            }

            return theDateStart;
        }

        public DateTime GetMonthlyEndDate(DateTime startTime)
        {
            DateTime theDate = new DateTime(startTime.Year, startTime.Month, 1);
            int firstDayOfMonth = (int)theDate.DayOfWeek;
            int startDayDiff = -1 * (firstDayOfMonth % this.DayInWeek);
            int endDayDiff = this.DayInWeek - 1 - ((int)theDate.AddMonths(1).AddDays(-1).DayOfWeek);
            DateTime theDateStart = theDate.AddDays(startDayDiff);
            DateTime theDateEnd = theDate.AddMonths(1).AddDays(-1).AddDays(endDayDiff);

            if ((theDateEnd - theDateStart).Days / this.DayInWeek == 4)
            {
                theDateEnd = theDateEnd.AddDays(7);
            }
            else if ((theDateEnd - theDateStart).Days / this.DayInWeek == 3)
            {
                theDateStart = theDateStart.AddDays(-7);
                theDateEnd = theDateEnd.AddDays(7);
            }

            return theDateEnd;
        }
	}
}