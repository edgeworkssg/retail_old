using System;
using AppointmentBook.Model;
using System.Collections.Generic;


namespace AppointmentBook
{
	public static class TimeHelper
	{
		public static DateTime Floor(DateTime value, int timeResolution)
		{
			return value.Date + TimeSpan.FromMinutes((int)Math.Floor((value - value.Date).TotalMinutes / timeResolution) * timeResolution);
		}

		public static DateTime Ceiling(DateTime value, int timeResolution)
		{
			return value.Date + TimeSpan.FromMinutes((int)Math.Ceiling((value - value.Date).TotalMinutes / timeResolution) * timeResolution);
		}

		public static DateTime Round(DateTime value, int timeResolution)
		{
			return value.Date + TimeSpan.FromMinutes((int)Math.Round((value - value.Date).TotalMinutes / timeResolution) * timeResolution);
		}

		public static DateTime FindStartTimeForDuration(Employee employee, AppointmentBookControlOptions options, DateTime day, TimeSpan proposedTime, TimeSpan duration)
		{
			var schedule = employee.Schedule;

			if (IsFree(employee, options, day, proposedTime, duration))
				return day + proposedTime;

			if (schedule.Count == 0)
				return day + options.WorkDayStart;

			for (int i = 0; i <= schedule.Count; i++)
			{
				if (i == schedule.Count)
				{
					if (Floor(day + options.WorkDayEnd, options.TimeResolution) -
					    Ceiling(schedule[i - 1].StartTime + schedule[i - 1].Duration, options.TimeResolution) >= duration)
						return Ceiling(schedule[i - 1].StartTime + schedule[i - 1].Duration, options.TimeResolution);
				}
				else
				{
					var timeSlot = employee.Schedule[i];

					if (i == 0)
					{
						if (Ceiling(timeSlot.StartTime, options.TimeResolution) -
						    Floor(day + options.WorkDayStart, options.TimeResolution) >= duration)
							return Floor(day + options.WorkDayStart, options.TimeResolution);
					}
					else
					{
						if (Floor(timeSlot.StartTime, options.TimeResolution) -
						    Ceiling(schedule[i - 1].StartTime + schedule[i - 1].Duration, options.TimeResolution) >= duration)
						{
							var time = Ceiling(schedule[i - 1].StartTime + schedule[i - 1].Duration, options.TimeResolution);
							if (time >= day + options.WorkDayStart && time + duration < day + options.WorkDayEnd)

								return time;
						}
					}
				}
			}

			throw new ApplicationException("There is no free slot for the desired duration");
		}

		public static bool IsFree(Employee employee, AppointmentBookControlOptions options,
		                          DateTime day, TimeSpan startTime, TimeSpan duration)
		{
			if (startTime < options.WorkDayStart || startTime + duration > options.WorkDayEnd)
				return false;

			foreach (var timeSlot in employee.Schedule)
				if (timeSlot.StartTime < day + startTime + duration && day + startTime < timeSlot.StartTime + timeSlot.Duration)
					return false;

			return true;
		}

        public static bool IsFreeResource(AppointmentResource resource, AppointmentBookControlOptions options,
                                  DateTime day, TimeSpan startTime, TimeSpan duration)
        {
            if (startTime < options.WorkDayStart || startTime + duration > options.WorkDayEnd)
                return false;

            foreach (var timeSlot in resource.Schedule)
                if (timeSlot.StartTime < day + startTime + duration && day + startTime < timeSlot.StartTime + timeSlot.Duration)
                    return false;

            return true;
        }

		public static TimeSpan FindLongestFreeInterval(Employee employee, AppointmentBookControlOptions options, DateTime day)
		{
			var schedule = employee.Schedule;

			if (schedule.Count == 0)
				return options.WorkDayEnd - options.WorkDayStart;

			var longestFreeInterval = TimeSpan.FromMinutes(0);
			
			for (int i = 0; i <= schedule.Count; i++)
			{
				TimeSpan freeInterval;
				if (i == schedule.Count)
					freeInterval = Floor(day + options.WorkDayEnd, options.TimeResolution) -
					               Ceiling(schedule[i - 1].StartTime + schedule[i - 1].Duration, options.TimeResolution);
				else
				{
					var timeSlot = employee.Schedule[i];

					if (i == 0)
						freeInterval = Ceiling(timeSlot.StartTime, options.TimeResolution) -
						               Floor(day + options.WorkDayStart, options.TimeResolution);
					else
						freeInterval = Floor(timeSlot.StartTime, options.TimeResolution) -
						               Ceiling(schedule[i - 1].StartTime + schedule[i - 1].Duration, options.TimeResolution);
				}
				if (freeInterval > longestFreeInterval)
					longestFreeInterval = freeInterval;
			}
			
			return longestFreeInterval;
		}

		public static TimeSpan GetMaxDuration(Employee employee, AppointmentBookControlOptions options, DateTime startTime)
		{
			foreach (var timeSlot in employee.Schedule)
			{
				if (timeSlot.StartTime < startTime && timeSlot.StartTime + timeSlot.Duration > startTime)
					return TimeSpan.Zero;

				if (timeSlot.StartTime > startTime)
					return timeSlot.StartTime - startTime;
			}
			return startTime.Date + options.WorkDayEnd - startTime;
		}

        public static TimeSpan GetMaxDurationResource(AppointmentResource resource, AppointmentBookControlOptions options, DateTime startTime)
        {
            foreach (var timeSlot in resource.Schedule)
            {
                if (timeSlot.StartTime < startTime && timeSlot.StartTime + timeSlot.Duration > startTime)
                    return TimeSpan.Zero;

                if (timeSlot.StartTime > startTime)
                    return timeSlot.StartTime - startTime;
            }
            return startTime.Date + options.WorkDayEnd - startTime;
        }


        public static TimeSlot GetConflictedTimeSlot(TimeSlot timeSlot, List<TimeSlot> schedule)
        {
            TimeSlot objReturn = null;

            foreach (var ab in schedule)
            {
                if (ab.Id == timeSlot.Id) continue;

                //condition so comparing only once
                if (ab.StartTime > timeSlot.StartTime) continue;

                var dtrA = new DateTimeSpan();
                dtrA.Start = timeSlot.StartTime;
                dtrA.End = timeSlot.StartTime.AddTicks(timeSlot.Duration.Ticks);

                var dtrB = new DateTimeSpan();
                dtrB.Start = ab.StartTime;
                dtrB.End = ab.StartTime.AddTicks(ab.Duration.Ticks);

                if (dtrA.Intersects(dtrB))
                {
                    objReturn = ab;
                }
            }

            return objReturn;
        }

        public static bool IsTimeSlotConflicted(TimeSlot ta, TimeSlot tb)
        {
            //condition so comparing only once
            var dtrA = new DateTimeSpan();
            dtrA.Start = ta.StartTime;
            dtrA.End = ta.StartTime.AddTicks(ta.Duration.Ticks);

            var dtrB = new DateTimeSpan();
            dtrB.Start = tb.StartTime;
            dtrB.End = tb.StartTime.AddTicks(tb.Duration.Ticks);

            if (dtrA.Intersects(dtrB))
            {
                return true;
            }
            

            return false;
        }
    }
}
