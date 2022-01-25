using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;

/// <summary>
/// Summary description for Recurrence
/// </summary>
public class RecurrenceLogic
{
	#region Properties
	private RecurrencePattern _pattern = RecurrencePattern.None;
	public RecurrencePattern Pattern
	{
		get { return _pattern; }
		set { _pattern = value; }
	}

	private RecurrenceSubPattern _subPattern = RecurrenceSubPattern.SubPattern1;
	public RecurrenceSubPattern SubPattern
	{
		get { return _subPattern; }
		set { _subPattern = value; }
	}

	private RecurrenceEndType _endType = RecurrenceEndType.EndDate;
	public RecurrenceEndType EndType
	{
		get { return _endType; }
		set { _endType = value; }
	}

	private DateTime _startDate = DateTime.Now;
	public DateTime StartDate
	{
		get { return _startDate; }
		set { _startDate = value; }
	}

	private DateTime _endDate = DateTime.Now;
	public DateTime EndDate
	{
		get { return _endDate; }
		set { _endDate = value; }
	}

	private int _endAfter = 1;
	public int EndAfter
	{
		get { return _endAfter; }
		set { _endAfter = value; }
	}

	private int _frequency = 1;
	public int Frequency
	{
		get { return _frequency; }
		set { _frequency = value; }
	}

	private byte _weekDays = 0;
	public byte WeekDays
	{
		get { return _weekDays; }
		set { _weekDays = value; }
	}

	private byte _dayOfMonth = 1;
	public byte DayOfMonth
	{
		get { return _dayOfMonth; }
		set { _dayOfMonth = value; }
	}

	private WeekNumber _weekNum = WeekNumber.First;
	public WeekNumber WeekNum
	{
		get { return _weekNum; }
		set { _weekNum = value; }
	}

	private string _comment = string.Empty;
	public string Comment
	{
		get{ return _comment;}
		set{ _comment = value;}
	}
	#endregion

	public RecurrenceLogic()
	{
				
	}

	public RecurrenceLogic(RecurrencePattern pattern, RecurrenceSubPattern subPattern, RecurrenceEndType endType,
						DateTime startDate, DateTime endDate, int endAfter, int frequency, byte weekDays,
						byte dayOfMonth, WeekNumber weekNum)
	{
		_pattern = pattern;
		_subPattern = subPattern;
		_endType = endType;
		_startDate = startDate;
		_endDate = endDate;
		_endAfter = endAfter;
		_frequency = frequency;
		_dayOfMonth = dayOfMonth;
		_weekNum = weekNum;
	}

	public RecurrenceLogic(RecurrencePattern pattern, RecurrenceSubPattern subPattern, RecurrenceEndType endType,
						DateTime startDate, DateTime endDate, int endAfter, int frequency, byte weekDays,
						byte dayOfMonth, WeekNumber weekNum, string comment)
	{
		_pattern = pattern;
		_subPattern = subPattern;
		_endType = endType;
		_startDate = startDate;
		_endDate = endDate;
		_endAfter = endAfter;
		_frequency = frequency;
		_dayOfMonth = dayOfMonth;
		_weekNum = weekNum;
		_comment = comment;
	}

	public ArrayList GetRecurrenceDates(DateTime sDate, DateTime eDate, DateTime itemStartDate)
	{
		ArrayList al = new ArrayList();
		DateTime sd = StartDate;
		int recNum = 0;
		int wNum = 0;
		if (Pattern == RecurrencePattern.None)
			return al;
		while (sd.Date <= eDate.Date)
		{
			if (Pattern == RecurrencePattern.Daily)
			{
				//every i-th day
				if (SubPattern == RecurrenceSubPattern.SubPattern1)
				{
					if (sd.Date == itemStartDate.Date)
					{
						sd = sd.AddDays(Frequency);
						continue;
					}

					al.Add(sd);
					sd = sd.AddDays(Frequency);
					recNum++;
					if (EndType == RecurrenceEndType.EndDate && sd.Date > EndDate.Date)
						break;
					if (EndType == RecurrenceEndType.NumberOfOccurrences && recNum >= EndAfter)
						break;
				}
				//every working day
				if (SubPattern == RecurrenceSubPattern.SubPattern2)
				{
					if (sd.DayOfWeek == DayOfWeek.Saturday || sd.DayOfWeek == DayOfWeek.Sunday)
					{
						sd = sd.AddDays(1);
						continue;
					}
					if (sd.Date == itemStartDate.Date)
					{
						sd = sd.AddDays(1);
						continue;
					}
					al.Add(sd);
					sd = sd.AddDays(1);
					recNum++;
					if (EndType == RecurrenceEndType.EndDate && sd.Date > EndDate.Date)
						break;
					if (EndType == RecurrenceEndType.NumberOfOccurrences && recNum >= EndAfter)
						break;
				}
			}
			if (Pattern == RecurrencePattern.Weekly)
			{
				wNum = GetRelativeWeekNumber(sDate, sd);
				if (sd.Date == itemStartDate.Date)
				{
					sd = sd.AddDays(1);
					continue;
				}
				if (wNum % Frequency == 0)
				{
					if (((WeekDays & (byte)DaysOfWeek.Monday) > 0) && sd.DayOfWeek == DayOfWeek.Monday)
					{
						al.Add(sd);
						recNum++;
					}
					if (((WeekDays & (byte)DaysOfWeek.Tuesday) > 0) && sd.DayOfWeek == DayOfWeek.Tuesday)
					{
						al.Add(sd);
						recNum++;
					}
					if (((WeekDays & (byte)DaysOfWeek.Wednesday) > 0) && sd.DayOfWeek == DayOfWeek.Wednesday)
					{
						al.Add(sd);
						sd = sd.AddDays(1);
						recNum++;
					}
					if (((WeekDays & (byte)DaysOfWeek.Thursday) > 0) && sd.DayOfWeek == DayOfWeek.Thursday)
					{
						al.Add(sd);
						recNum++;
					}
					if (((WeekDays & (byte)DaysOfWeek.Friday) > 0) && sd.DayOfWeek == DayOfWeek.Friday)
					{
						al.Add(sd);
						recNum++;
					}
					if (((WeekDays & (byte)DaysOfWeek.Saturday) > 0) && sd.DayOfWeek == DayOfWeek.Saturday)
					{
						al.Add(sd);
						recNum++;
					}
					if (((WeekDays & (byte)DaysOfWeek.Sunday) > 0) && sd.DayOfWeek == DayOfWeek.Sunday)
					{
						al.Add(sd);
						recNum++;
					}
					sd = sd.AddDays(1);
					if (EndType == RecurrenceEndType.EndDate && sd.Date > EndDate.Date)
						break;
					if (EndType == RecurrenceEndType.NumberOfOccurrences && recNum >= EndAfter)
						break;
					continue;
				}
				sd = sd.AddDays(1);
				continue;
			}
			if (Pattern == RecurrencePattern.Monthly)
			{
				//every j-th day of the i-th month
				if (SubPattern == RecurrenceSubPattern.SubPattern1)
				{
					if (sd.Date == itemStartDate.Date)
					{
						sd = sd.AddDays(1);
						continue;
					}
					int month = GetRelativeMonthNumber(StartDate, sd);
					if (month % Frequency == 0 && sd.Date == StartDate.AddMonths((int)(month)).Date)
					{
						al.Add(sd);
						recNum++;
						sd = sd.AddDays(1);
						if (EndType == RecurrenceEndType.NumberOfOccurrences && recNum >= EndAfter)
							break;
						if (EndType == RecurrenceEndType.EndDate && sd.Date > EndDate.Date)
							break;
						continue;
					}
					sd = sd.AddDays(1);
					if (EndType == RecurrenceEndType.EndDate && sd.Date > EndDate.Date)
						break;
					continue;
				}
				//every j-th weekday of i-th month
				if (SubPattern == RecurrenceSubPattern.SubPattern2)
				{
					if (sd.Date == itemStartDate.Date)
					{
						sd = sd.AddDays(1);
						continue;
					}
					if (GetDayOfWeekBit(sd) == (DaysOfWeek)WeekDays)
					{
						WeekNumber week = GetWeekNumber(sd);
						if ((week == WeekNum) || 
							(week == WeekNumber.Fourth && WeekNum == WeekNumber.Last && GetWeekNumber(sd.AddDays(7)) == WeekNumber.First))
						{
							int month = GetRelativeMonthNumber(StartDate, sd);
							if (month % Frequency == 0)
							{
								al.Add(sd);
								recNum++;
								if (EndType == RecurrenceEndType.NumberOfOccurrences && recNum >= EndAfter)
									break;
							}
						}
					}
					sd = sd.AddDays(1);
					if (EndType == RecurrenceEndType.EndDate && sd.Date > EndDate.Date)
						break;
					continue;
				}
			}
			if (Pattern == RecurrencePattern.Yearly)
			{
				//on j-th day in jan/feb/.../dec
				if (SubPattern == RecurrenceSubPattern.SubPattern1)
				{
					byte day = DayOfMonth;
					byte month = WeekDays;
					if (sd.Date == itemStartDate.Date)
					{
						sd = sd.AddDays(1);
						continue;
					}
					if (sd.Month == (int)month && sd.Day == (int)day)
					{
						al.Add(sd);
						recNum++;
						sd = sd.AddDays(1);
						if (EndType == RecurrenceEndType.EndDate && sd.Date > EndDate.Date)
							break;
						if (EndType == RecurrenceEndType.NumberOfOccurrences && recNum >= EndAfter)
							break;
						continue;
					}
					sd = sd.AddDays(1);
				}
				//on j-th weekday in jan/feb/.../dec
				if (SubPattern == RecurrenceSubPattern.SubPattern2)
				{
					if (sd.Date == itemStartDate.Date)
					{
						sd = sd.AddDays(1);
						continue;
					}
					int month = DayOfMonth;
					
					if (GetDayOfWeekBit(sd) == (DaysOfWeek)WeekDays && sd.Month == month)
					{
						WeekNumber week = GetWeekNumber(sd);
						if ((week == WeekNum) ||
							((week == WeekNumber.Fourth && WeekNum==WeekNumber.Last) && 
							GetWeekNumber(sd.AddDays(7)) == WeekNumber.First))
						{
							al.Add(sd);
							recNum++;
							sd = sd.AddDays(1);
							if (EndType == RecurrenceEndType.EndDate && sd.Date > EndDate.Date)
								break;
							if (EndType == RecurrenceEndType.NumberOfOccurrences && recNum >= EndAfter)
								break;
							continue;
						}
					}
					sd = sd.AddDays(1);
					if (EndType == RecurrenceEndType.EndDate && sd.Date > EndDate.Date)
						break;
					continue;
				}
			}
		}
		for (int i = al.Count-1; i>=0 ; i--)
		{
			DateTime dt = (DateTime)al[i];
			if (dt.Date < sDate.Date || dt.Date > eDate.Date || dt.Date == itemStartDate.Date)
				al.RemoveAt(i);
		}
		return al;
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

	private int GetRelativeWeekNumber(DateTime day1, DateTime day2)
	{
		int week = 0;
		DateTime curDate = day1;
		TimeSpan span = day2 - day1;
		if (span.Days >= 7)
		{
			while (curDate.DayOfWeek != DayOfWeek.Monday)
			{
				curDate = curDate.AddDays(1);
				span = span.Subtract(new TimeSpan(1, 0, 0, 0));
			}
			if (curDate != day1)
				week++;
			week += span.Days / 7;
		}
		else
		{
			while (curDate.DayOfWeek != DayOfWeek.Monday && curDate < day2)
			{
				curDate = curDate.AddDays(1);
			}
			if (curDate != day2 || (curDate != day1 && curDate.DayOfWeek == DayOfWeek.Monday))
				week++;
		}
		return week;
	}

	private static int GetRelativeMonthNumber(DateTime day1, DateTime day2)
	{
		int month = 0;
		DateTime curDate = day1;
		TimeSpan span = day2 - day1;
		while (curDate < day2)
		{
			int daysLeft = DateTime.DaysInMonth(curDate.Year, curDate.Month) - curDate.Day;
			if (span.Days > daysLeft)
			{
				month++;
				curDate = curDate.AddDays(daysLeft + 1);
				span = span.Subtract(new TimeSpan(daysLeft + 1, 0, 0, 0));
			}
			else
				break;
		}
		return month;
	}
}

public enum RecurrencePattern
{
	None = 0,
	Daily = 1,
	Weekly = 2,
	Monthly = 3,
	Yearly = 4
}

public enum RecurrenceSubPattern
{
	SubPattern1 = 0,
	SubPattern2 = 1
}

public enum RecurrenceEndType
{
	Never = 0,
	EndDate = 1,
	NumberOfOccurrences = 2
}

public enum WeekNumber
{
	First = 0,
	Second  = 1,
	Third = 2,
	Fourth = 3,
	Last = 4
}

public enum DaysOfWeek
{
	Sunday = 0x01,
	Monday = 0x02,
	Tuesday = 0x04,
	Wednesday = 0x08,
	Thursday = 0x10,
	Friday = 0x20,
	Saturday = 0x40
}



