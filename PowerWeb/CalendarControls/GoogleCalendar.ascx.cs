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
using System.Drawing;

public partial class CalendarControls_GoogleCalendar : System.Web.UI.UserControl
{
	protected void Page_Load(object sender, EventArgs e)
	{
		gCalendar.MultiDayView.ItemTemplate = "<div style=\"background-color: #2952a3; height:1px; font-size:1px; line-height:1px; margin:0px 2px;\"></div>" +
			"<div style=\"background-color: #2952a3; height:1px; font-size:1px; line-height:1px; margin:0px 1px;\"></div>" +
			"<div style=\"background-color:#668CD9; border-left: solid 1px #2952a3; border-right: solid 1px #2952a3; height:100%;\">" +
			"<table border=\"0\" cellpadding=\"2\" width=\"100%\" cellspacing=\"0\" style=\"background-color:rgb(41, 82, 163); width:100%; font-weight:bold; font-size:11px; color:#ffffff; table-layout:fixed; overflow:hidden; font-family:Verdana,Sans-serif;\">" +
			"<tr mcc_action=\"move\"><td  style=\"cursor:move; overflow:hidden;\" unselectable=\"on\"><table><tr><td><span id=\"sHour\" ></span>:<span id=\"sMinute\"></span></td><td>" +
			"<img border='0' width='9px' height='7px' src='Images/g_repeat_white.gif' id=\"isRecurring\"/></td></tr></table></td></tr></table><div id=\"title\" style=\"cursor:default; color:#ffffff; font-family:Verdana,Sans-serif; font-size:11px;\" unselectable=\"on\"></div>" +
			"<div id=\"resizer\" mcc_action=\"resize\" style=\"font-size:5px; line-height:5px; position:absolute; bottom: 5px; height:5px; width:100%; z-index:11; cursor:s-resize;\">" +
			"<table width=\"100%\" height=\"100%\"><tr><td align=\"center\" style=\"color:#ffffff\">--------------</td></tr></table></div></div>" +
			"<div style=\"position:absolute; bottom:0px; width:100%;\"><div style=\"background-color: #2952a3; height:1px; font-size:1px; line-height:1px; border-left: 1px solid  #2952a3; border-right: 1px solid  #2952a3;\"></div></div>"+
			"<img border='0' style='position:absolute; left:0px; bottom:0px;z-index:10;' src='Images/gi_lb.gif'><img border='0' style='position:absolute; right:0px; bottom:0px;' src='Images/gi_rb.gif'>";
		gCalendar.MultiDayView.ItemMapping = "[{\"id\":\"title\",\"property\":\"innerHTML\",\"value\":\"Title\"},"+
			"{\"id\":\"sHour\",\"property\":\"innerHTML\",\"value\":\"StartDate.getHours()<10?'0'+item.StartDate.getHours():item.StartDate.getHours()\"},"+
			"{\"id\":\"sMinute\",\"property\":\"innerHTML\",\"value\":\"StartDate.getMinutes()<10?'0'+item.StartDate.getMinutes():item.StartDate.getMinutes()\"},"+
			"{\"id\":\"isRecurring\", \"property\":\"style.display\", \"value\":\"Extensions==true ? 'block':'none'\"}]";
		gCalendar.MultiDayView.EventBarItemTemplate = "<div style=\"background-color:#668cd9; margin-left:1px; margin-right:1px; line-height:1px; font-size:1px; height:1px; cursor:default;\" unselectable=\"on\"></div>"+
			"<div style=\"background-color:#668cd9; padding-top:1px; height:16px; padding-bottom:1px; text-align:left; color:#fff;\" unselectable=\"on\">"+
			"<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\"><tr style=\"font-family:verdana, sans-serif; font-size:11px;\"><td unselectable=\"on\" id=\"lb\">&nbsp;(</td>"+
			"<td unselectable=\"on\" style=\"cursor:default;\"><span id=\"sTime\"></span></td><td unselectable=\"on\" id=\"rb\">)&nbsp;</td><td id=\"title\" mcc_action=\"move\" style=\"cursor:move; text-align:left;\"></td>"+
			"<td style='padding-left:3px;'><img border='0' width='9px' height='7px' src='Images/g_repeat_white.gif' id=\"isRecurring\"/></td></tr></table></div>"+
			"<div style=\"background-color:#668cd9; margin-left:1px; margin-right:1px; line-height:1px; font-size:1px; height:1px; cursor:default;\" unselectable=\"on\"></div>";
		gCalendar.MultiDayView.EventBarItemMapping = "[{\"id\":\"title\",\"property\":\"innerHTML\",\"value\":\"Title\"},"+
			"{\"id\":\"lb\",\"property\":\"style.display\", \"value\":\"IsAllDay == true ? 'none' : ''\"},"+
			"{\"id\":\"rb\",\"property\":\"style.display\", \"value\":\"IsAllDay == true ? 'none' : ''\"},"+
			"{\"id\":\"sTime\",\"property\":\"innerHTML\", \"value\":\"IsAllDay == false ? ((item.StartDate.getHours()<10?'0'+item.StartDate.getHours():item.StartDate.getHours())+':'+(item.StartDate.getMinutes()<10?'0'+item.StartDate.getMinutes():item.StartDate.getMinutes())):''\"},"+
			" {\"id\":\"isRecurring\", \"property\":\"style.display\", \"value\":\"Extensions==true ? 'block':'none'\"}]";
		gCalendar.MonthView.ItemTemplate = gCalendar.MultiDayView.EventBarItemTemplate;
		gCalendar.MonthView.ItemMapping = gCalendar.MultiDayView.EventBarItemMapping;
		gCalendar.YearView.ItemTemplate = "<div style=\"background-color:#668cd9; margin-left:1px; margin-right:1px; line-height:1px; font-size:1px; height:1px; cursor:default;\" unselectable=\"on\"></div>" +
			"<div style=\"background-color:#668cd9; padding-top:1px; height:16px; padding-bottom:1px; text-align:left; color:#fff;\" unselectable=\"on\">" +
			"<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\"><tr style=\"font-family:verdana, sans-serif; font-size:11px;\"><td unselectable=\"on\" id=\"lb\">&nbsp;(</td>" +
			"<td unselectable=\"on\" style=\"cursor:default;\"><span id=\"sTime\"></span></td><td unselectable=\"on\" id=\"rb\">)&nbsp;</td><td id=\"title\" mcc_action=\"move\" style=\"text-align:left;\"></td>" +
			"<td style='padding-left:3px;'><img border='0' width='9px' height='7px' src='Images/g_repeat_white.gif' id=\"isRecurring\"/></td></tr></table></div>"+
			"<div style=\"background-color:#668cd9; margin-left:1px; margin-right:1px; line-height:1px; font-size:1px; height:1px; cursor:default;\" unselectable=\"on\"></div>";
		gCalendar.YearView.ItemMapping = gCalendar.MultiDayView.EventBarItemMapping;
		gCalendar.TaskView.ItemTemplate = "<div style=\"background-color:#668cd9; margin-left:1px; margin-right:1px; line-height:1px; font-size:1px; height:1px; cursor:default;\" unselectable=\"on\"></div>"+
			"<div id=\"main\" style=\"background-color:#668cd9; width:100%; height:16px;padding:1px;\">" +
			"<img border='0' width='9px' height='7px' src='Images/g_repeat_white.gif' id=\"isRecurring\" style=\"vertical-align:middle;\"/>" +
			"<div style=\"position:absolute; left:0px; top:0px; height:100%; width:15px; cursor:move;\" mcc_action=\"move\"></div></div>"+
			"<div style=\"background-color:#668cd9; margin-left:1px; margin-right:1px; line-height:1px; font-size:1px; height:1px; cursor:default;\" unselectable=\"on\"></div>";
		gCalendar.TaskView.ItemMapping = "[{\"id\":\"isRecurring\", \"property\":\"style.display\", \"value\":\"Extensions==true ? 'block':'none'\"},"+
			"{\"id\":\"main\", \"property\":\"title\", \"value\":\"StartDate.format('MM/dd/yy HH:mm')+' - '+item.EndDate.format('MM/dd/yy HH:mm')\"}" +
			"]";
		if (!this.IsPostBack)
		{
			if (Session["Calendar_ViewType"] != null)
			{
				gCalendar.ViewType = (Mediachase.AjaxCalendar.CalendarViewType)Session["Calendar_ViewType"];
			}
			if (Session["Calendar_ViewMode"] != null)
			{
				gCalendar.ViewMode = (Mediachase.AjaxCalendar.ViewModeType)Session["Calendar_ViewMode"];
			}
			if (Session["Calendar_SelectedDate"] != null)
			{
				gCalendar.SelectedDate = (DateTime)Session["Calendar_SelectedDate"];
			}
			switch (gCalendar.ViewType)
			{
				case Mediachase.AjaxCalendar.CalendarViewType.MultiDay:
					if (gCalendar.ViewMode == Mediachase.AjaxCalendar.ViewModeType.Day)
					{
						lbDay.Font.Bold = true;
						lbDay.ForeColor = Color.Black;
						divDay.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#C3D9FF");
						divDayTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#C3D9FF");
						divDayTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#C3D9FF");
						divWeek.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
						divWeekTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
						divWeekTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
						divMonth.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
						divMonthTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
						divMonthTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
						divYear.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
						divYearTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
						divYearTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
						divTask.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
						divTaskTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
						divTaskTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
						divTaskHour.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
						divTaskHourTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
						divTaskHourTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
						lbWeek.ForeColor = Color.Blue;
						lbMonth.ForeColor = Color.Blue;
						lbYear.ForeColor = Color.Blue;
						lbTask.ForeColor = Color.Blue;
						lbTaskHour.ForeColor = Color.Blue;
					}
					if (gCalendar.ViewMode == Mediachase.AjaxCalendar.ViewModeType.Week)
					{
						lbWeek.Font.Bold = true;
						lbDay.ForeColor = Color.Blue;
						lbWeek.ForeColor = Color.Black;
						divDay.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
						divDayTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
						divDayTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
						divWeek.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#C3D9FF");
						divWeekTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#C3D9FF");
						divWeekTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#C3D9FF");
						divMonth.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
						divMonthTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
						divMonthTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
						divYear.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
						divYearTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
						divYearTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
						divTask.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
						divTaskTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
						divTaskTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
						divTaskHour.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
						divTaskHourTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
						divTaskHourTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
						lbMonth.ForeColor = Color.Blue;
						lbYear.ForeColor = Color.Blue;
						lbTask.ForeColor = Color.Blue;
						lbTaskHour.ForeColor = Color.Blue;
					}
					break;
				case Mediachase.AjaxCalendar.CalendarViewType.Month:
					lbMonth.Font.Bold = true;
					lbDay.ForeColor = Color.Blue;
					lbWeek.ForeColor = Color.Blue;
					lbMonth.ForeColor = Color.Black;
					divDay.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
					divDayTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
					divDayTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
					divWeek.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
					divWeekTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
					divWeekTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
					divMonth.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#C3D9FF");
					divMonthTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#C3D9FF");
					divMonthTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#C3D9FF");
					divYear.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
					divYearTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
					divYearTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
					divTask.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
					divTaskTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
					divTaskTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
					divTaskHour.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
					divTaskHourTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
					divTaskHourTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
					lbYear.ForeColor = Color.Blue;
					lbTask.ForeColor = Color.Blue;
					lbTaskHour.ForeColor = Color.Blue;
					break;
				case Mediachase.AjaxCalendar.CalendarViewType.Year:
					lbYear.Font.Bold = true;
					lbDay.ForeColor = Color.Blue;
					lbWeek.ForeColor = Color.Blue;
					lbMonth.ForeColor = Color.Blue;
					lbYear.ForeColor = Color.Black;
					divDay.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
					divDayTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
					divDayTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
					divWeek.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
					divWeekTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
					divWeekTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
					divMonth.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
					divMonthTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
					divMonthTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
					divYear.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#C3D9FF");
					divYearTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#C3D9FF");
					divYearTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#C3D9FF");
					divTask.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
					divTaskTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
					divTaskTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
					divTaskHour.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
					divTaskHourTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
					divTaskHourTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
					lbTask.ForeColor = Color.Blue;
					lbTaskHour.ForeColor = Color.Blue;
					break;
				case Mediachase.AjaxCalendar.CalendarViewType.Task:
					lbTask.Font.Bold = true;
					lbDay.ForeColor = Color.Blue;
					lbWeek.ForeColor = Color.Blue;
					lbMonth.ForeColor = Color.Blue;
					lbYear.ForeColor = Color.Blue;
					divDay.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
					divDayTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
					divDayTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
					divWeek.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
					divWeekTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
					divWeekTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
					divMonth.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
					divMonthTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
					divMonthTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
					divYear.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
					divYearTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
					divYearTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
					if (Session["Calendar_TaskScale"] != null)
					{
						Mediachase.AjaxCalendar.TaskScaleType taskType = (Mediachase.AjaxCalendar.TaskScaleType)Session["Calendar_TaskScale"];
						switch (taskType)
						{
							case Mediachase.AjaxCalendar.TaskScaleType.Day:
								gCalendar.TaskView.DaysCount = 30;
								gCalendar.TaskView.HeaderDayFormat = "dd";
								gCalendar.TaskView.ScaleType = Mediachase.AjaxCalendar.TaskScaleType.Day;
								divTask.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#C3D9FF");
								divTaskTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#C3D9FF");
								divTaskTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#C3D9FF");
								divTaskHour.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
								divTaskHourTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
								divTaskHourTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
								lbTask.ForeColor = Color.Black;
								lbTask.Font.Bold = true;
								lbTaskHour.ForeColor = Color.Blue;
								lbTaskHour.Font.Bold = false;
								break;
							case Mediachase.AjaxCalendar.TaskScaleType.Hour:
								gCalendar.TaskView.DaysCount = 3;
								gCalendar.TaskView.HeaderDayFormat = "dd, MMMM";
								gCalendar.TaskView.ScaleType = Mediachase.AjaxCalendar.TaskScaleType.Hour;
								divTask.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
								divTaskTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
								divTaskTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
								divTaskHour.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#C3D9FF");
								divTaskHourTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#C3D9FF");
								divTaskHourTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#C3D9FF");
								lbTask.ForeColor = Color.Blue;
								lbTask.Font.Bold = false;
								lbTaskHour.ForeColor = Color.Black;
								lbTaskHour.Font.Bold = true;
								break;
						}
					}
					else
					{
						gCalendar.TaskView.DaysCount = 30;
						gCalendar.TaskView.HeaderDayFormat = "dd";
						gCalendar.TaskView.ScaleType = Mediachase.AjaxCalendar.TaskScaleType.Day;
						divTask.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#C3D9FF");
						divTaskTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#C3D9FF");
						divTaskTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#C3D9FF");
						divTaskHour.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
						divTaskHourTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
						divTaskHourTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
						lbTask.ForeColor = Color.Black;
						lbTask.Font.Bold = true;
						lbTaskHour.ForeColor = Color.Blue;
						lbTaskHour.Font.Bold = false;
					}
					break;
				default:
					break;
			}
			UpdateTodayButton();
			lbDatesRange.Text = GetDatesRange();
			
		}
	}

	protected void Page_PreRender(object sender, EventArgs e)
	{
		switch (gCalendar.ViewMode)
		{
			case Mediachase.AjaxCalendar.ViewModeType.Day:
				gCalendar.MultiDayView.HeaderDateFormat = "dddd";
				break;
			case Mediachase.AjaxCalendar.ViewModeType.Custom:
			case Mediachase.AjaxCalendar.ViewModeType.Week:
			case Mediachase.AjaxCalendar.ViewModeType.WorkWeek:
				gCalendar.MultiDayView.HeaderDateFormat = "ddd, d\\/MM";
				break;
			default:
				break;
		}
		
	}

	protected string GetDatesRange()
	{
		switch (gCalendar.ViewType)
		{
			case Mediachase.AjaxCalendar.CalendarViewType.MultiDay:
				if(gCalendar.ViewMode == Mediachase.AjaxCalendar.ViewModeType.Day)
					return gCalendar.SelectedDate.ToString("d MMM yyyy");
				if (gCalendar.ViewMode == Mediachase.AjaxCalendar.ViewModeType.Week)
				{
					DateTime sd = gCalendar.SelectedDate;
					DateTime ed = gCalendar.SelectedDate;
					while ((int)sd.DayOfWeek != (int)gCalendar.MultiDayView.WeekStartDay)
					{
						sd = sd.AddDays(-1);
					}
					ed = sd.AddDays(6);
					if (sd.Month == ed.Month && sd.Year == ed.Year)
						return sd.ToString("dd") + " - " + ed.ToString("d MMM yyyy");
					if((sd.Month!=ed.Month && sd.Year == ed.Year) || (sd.Year!=ed.Year))
						return sd.ToString("d MMM yyyy") + " - " + ed.ToString("d MMM yyyy");
				}
				if (gCalendar.ViewMode == Mediachase.AjaxCalendar.ViewModeType.WorkWeek)
				{
					DateTime sd = gCalendar.SelectedDate;
					DateTime ed = gCalendar.SelectedDate;
					while ((int)sd.DayOfWeek != (int)DayOfWeek.Monday)
					{
						sd = sd.AddDays(-1);
					}
					ed = sd.AddDays(4);
					if (sd.Month == ed.Month && sd.Year == ed.Year)
						return sd.ToString("dd") + " - " + ed.ToString("d MMM yyyy");
					if ((sd.Month != ed.Month && sd.Year == ed.Year) || (sd.Year != ed.Year))
						return sd.ToString("d MMM yyyy") + " - " + ed.ToString("d MMM yyyy");
				}
				return string.Empty;
			case Mediachase.AjaxCalendar.CalendarViewType.Month:
				return gCalendar.SelectedDate.ToString("MMMM yyyy");
			case Mediachase.AjaxCalendar.CalendarViewType.Year:
				return gCalendar.SelectedDate.ToString("yyyy");
			case Mediachase.AjaxCalendar.CalendarViewType.Task:
				return gCalendar.TaskView.ViewStartDate.ToString("dd MMMM") + " - " + gCalendar.TaskView.ViewEndDate.ToString("dd MMMM");
			default:
				return string.Empty;
		}
	}

	protected DateTime GetViewStartDate()
	{
		DateTime dt = gCalendar.SelectedDate;
		switch (gCalendar.ViewType)
		{
			case Mediachase.AjaxCalendar.CalendarViewType.MultiDay:
				if (gCalendar.ViewMode == Mediachase.AjaxCalendar.ViewModeType.Day)
					return dt;
				if (gCalendar.ViewMode == Mediachase.AjaxCalendar.ViewModeType.Week)
				{
					DateTime sd = dt;
					DateTime ed = dt;
					while ((int)sd.DayOfWeek != (int)gCalendar.MultiDayView.WeekStartDay)
					{
						sd = sd.AddDays(-1);
					}
					dt = sd;
				}
				if (gCalendar.ViewMode == Mediachase.AjaxCalendar.ViewModeType.WorkWeek)
				{
					DateTime sd = dt;
					DateTime ed = dt;
					while ((int)sd.DayOfWeek != (int)DayOfWeek.Monday)
					{
						sd = sd.AddDays(-1);
					}
					dt = sd;
				}
				break;
			case Mediachase.AjaxCalendar.CalendarViewType.Month:
				while (dt.Day != 1)
				{
					dt = new DateTime(dt.Year, dt.Month, 1);
				}
				break;
			case Mediachase.AjaxCalendar.CalendarViewType.Year:
				while (dt.Day != 1 && dt.Month != 1)
				{
					dt = new DateTime(dt.Year, 1, 1);
				}
				break;
			case Mediachase.AjaxCalendar.CalendarViewType.Task:
				return dt;
			default:
				break;

		}
		return dt;
	}

	protected void UpdateTodayButton()
	{
		switch (gCalendar.ViewType)
		{
			case Mediachase.AjaxCalendar.CalendarViewType.MultiDay:
				if (gCalendar.ViewMode == Mediachase.AjaxCalendar.ViewModeType.Day)
				{
					if (gCalendar.SelectedDate.Date == DateTime.Now.Date)
						btnToday.Enabled = false;
					else
						btnToday.Enabled = true;
				}
				if (gCalendar.ViewMode == Mediachase.AjaxCalendar.ViewModeType.Week)
				{
					DateTime sd = GetViewStartDate();
					DateTime ed = sd.AddDays(6);
					if (DateTime.Now.Date >= sd.Date && DateTime.Now.Date <= ed.Date)
						btnToday.Enabled = false;
					else
						btnToday.Enabled = true;
				}
				if (gCalendar.ViewMode == Mediachase.AjaxCalendar.ViewModeType.WorkWeek)
				{
					DateTime sd = GetViewStartDate();
					DateTime ed = sd.AddDays(4);
					if (DateTime.Now.Date >= sd.Date && DateTime.Now.Date <= ed.Date)
						btnToday.Enabled = false;
					else
						btnToday.Enabled = true;
				}
				break;
			case Mediachase.AjaxCalendar.CalendarViewType.Month:
				if(DateTime.Now.Month == gCalendar.SelectedDate.Month && 
					DateTime.Now.Year == gCalendar.SelectedDate.Year)
					btnToday.Enabled = false;
				else
					btnToday.Enabled = true;
				break;
			case Mediachase.AjaxCalendar.CalendarViewType.Year:
				if (DateTime.Now.Year == gCalendar.SelectedDate.Year)
					btnToday.Enabled = false;
				else
					btnToday.Enabled = true;
				break;
			case Mediachase.AjaxCalendar.CalendarViewType.Task:
				if (DateTime.Now.Date >= gCalendar.TaskView.ViewStartDate.Date &&
					DateTime.Now.Date <= gCalendar.TaskView.ViewEndDate.Date)
					btnToday.Enabled = false;
				else
					btnToday.Enabled = true;
				break;
			default:
				break;
		}	
	}

	protected void Page_Init(object sender, EventArgs e)
	{
		ibNext.Click += new ImageClickEventHandler(ibNext_Click);
		ibPrev.Click += new ImageClickEventHandler(ibPrev_Click);
		lbDay.Click += new EventHandler(lbDay_Click);
		lbWeek.Click += new EventHandler(lbWeek_Click);
		lbMonth.Click += new EventHandler(lbMonth_Click);
		btnToday.Click += new EventHandler(btnToday_Click);
		lbYear.Click += new EventHandler(lbYear_Click);
		lbTask.Click += new EventHandler(lbTask_Click);
		lbTaskHour.Click += new EventHandler(lbTaskHour_Click);
		gCalendar.SelectedViewChange+=new EventHandler<Mediachase.AjaxCalendar.CalendarViewSelectEventArgs>(gCalendar_SelectedViewChange);
	}

	void lbTaskHour_Click(object sender, EventArgs e)
	{
		gCalendar.ViewType = Mediachase.AjaxCalendar.CalendarViewType.Task;
		Session["Calendar_ViewType"] = gCalendar.ViewType;
		gCalendar.Refresh();
		gCalendar.TaskView.ScaleType = Mediachase.AjaxCalendar.TaskScaleType.Hour;
		gCalendar.TaskView.DaysCount = 3;
		gCalendar.TaskView.HeaderDayFormat = "dd, MMMM";
		Session["Calendar_TaskScale"] = Mediachase.AjaxCalendar.TaskScaleType.Hour;
		UpdateTodayButton();
		lbDatesRange.Text = GetDatesRange();
		lbDay.Font.Bold = false;
		lbDay.ForeColor = Color.Blue;
		lbWeek.Font.Bold = false;
		lbWeek.ForeColor = Color.Blue;
		lbMonth.Font.Bold = false;
		lbMonth.ForeColor = Color.Blue;
		lbYear.Font.Bold = false;
		lbYear.ForeColor = Color.Blue;
		lbTaskHour.Font.Bold = true;
		lbTaskHour.ForeColor = Color.Black;
		lbTask.ForeColor = Color.Blue;
		lbTask.Font.Bold = false;
		divDay.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divDayTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divDayTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divWeek.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divWeekTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divWeekTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divMonth.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divMonthTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divMonthTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divYear.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divYearTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divYearTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divTask.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divTaskTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divTaskTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divTaskHour.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#C3D9FF");
		divTaskHourTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#C3D9FF");
		divTaskHourTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#C3D9FF");
	}

	void lbTask_Click(object sender, EventArgs e)
	{
		gCalendar.ViewType = Mediachase.AjaxCalendar.CalendarViewType.Task;
		Session["Calendar_ViewType"] = gCalendar.ViewType;
		Session["Calendar_TaskScale"] = Mediachase.AjaxCalendar.TaskScaleType.Day;
		gCalendar.TaskView.DaysCount = 30;
		gCalendar.TaskView.HeaderDayFormat = "dd";
		gCalendar.TaskView.ScaleType = Mediachase.AjaxCalendar.TaskScaleType.Day;
		gCalendar.Refresh();
		UpdateTodayButton();
		lbDatesRange.Text = GetDatesRange();
		lbDay.Font.Bold = false;
		lbDay.ForeColor = Color.Blue;
		lbWeek.Font.Bold = false;
		lbWeek.ForeColor = Color.Blue;
		lbMonth.Font.Bold = false;
		lbMonth.ForeColor = Color.Blue;
		lbYear.Font.Bold = false;
		lbYear.ForeColor = Color.Blue;
		lbTask.Font.Bold = true;
		lbTask.ForeColor = Color.Black;
		lbTaskHour.ForeColor = Color.Blue;
		lbTaskHour.Font.Bold = false;
		divDay.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divDayTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divDayTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divWeek.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divWeekTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divWeekTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divMonth.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divMonthTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divMonthTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divYear.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divYearTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divYearTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divTask.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#C3D9FF");
		divTaskTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#C3D9FF");
		divTaskTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#C3D9FF");
		divTaskHour.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divTaskHourTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divTaskHourTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
	}

	void gCalendar_SelectedViewChange(object sender, Mediachase.AjaxCalendar.CalendarViewSelectEventArgs e)
	{
		switch (e.NewViewType)
		{
			case Mediachase.AjaxCalendar.CalendarViewType.MultiDay:
				lbDay.Font.Bold = true;
				lbDay.ForeColor = Color.Black;
				lbWeek.ForeColor = Color.Blue;
				lbWeek.Font.Bold = false;
				lbMonth.ForeColor = Color.Blue;
				lbMonth.Font.Bold = false;
				lbYear.ForeColor = Color.Blue;
				lbTask.ForeColor = Color.Blue;
				lbTask.Font.Bold = false;
				lbTaskHour.ForeColor = Color.Blue;
				lbTaskHour.Font.Bold = false;
				gCalendar.ViewMode = Mediachase.AjaxCalendar.ViewModeType.Day;
				gCalendar.ViewType = Mediachase.AjaxCalendar.CalendarViewType.MultiDay;
				gCalendar.Refresh();
				divDay.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#C3D9FF");
				divDayTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#C3D9FF");
				divDayTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#C3D9FF");
				divWeek.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
				divWeekTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
				divWeekTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
				divMonth.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
				divMonthTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
				divMonthTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
				divYear.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
				divYearTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
				divYearTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
				divTask.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
				divTaskTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
				divTaskTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
				divTaskHour.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
				divTaskHourTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
				divTaskHourTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
				break;
			case Mediachase.AjaxCalendar.CalendarViewType.Month:
				lbMonth.Font.Bold = true;
				lbDay.ForeColor = Color.Blue;
				lbWeek.ForeColor = Color.Blue;
				lbMonth.ForeColor = Color.Black;
				lbYear.ForeColor = Color.Blue;
				lbYear.Font.Bold = false;
				lbTaskHour.ForeColor = Color.Blue;
				lbTaskHour.Font.Bold = false;
				divDay.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
				divDayTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
				divDayTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
				divWeek.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
				divWeekTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
				divWeekTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
				divMonth.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#C3D9FF");
				divMonthTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#C3D9FF");
				divMonthTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#C3D9FF");
				divYear.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
				divYearTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
				divYearTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
				divTask.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
				divTaskTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
				divTaskTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
				divTaskHour.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
				divTaskHourTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
				divTaskHourTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
				break;
		}
		Session["Calendar_ViewType"] = gCalendar.ViewType;
		Session["Calendar_ViewMode"] = gCalendar.ViewMode;
		Session["Calendar_SelectedDate"] = gCalendar.SelectedDate;
		UpdateTodayButton();
		lbDatesRange.Text = GetDatesRange();
	}

	void lbYear_Click(object sender, EventArgs e)
	{
		gCalendar.ViewType = Mediachase.AjaxCalendar.CalendarViewType.Year;
		Session["Calendar_ViewType"] = gCalendar.ViewType;
		gCalendar.Refresh();
		if (gCalendar.SelectedDate.Year == DateTime.Now.Year)
			btnToday.Enabled = false;
		else
			btnToday.Enabled = true;
		lbDatesRange.Text = GetDatesRange();//Calendar.SelectedDate.ToString("yyyy");
		lbDay.Font.Bold = false;
		lbDay.ForeColor = Color.Blue;
		lbWeek.Font.Bold = false;
		lbWeek.ForeColor = Color.Blue;
		lbMonth.Font.Bold = false;
		lbMonth.ForeColor = Color.Blue;
		lbYear.Font.Bold = true;
		lbYear.ForeColor = Color.Black;
		lbTask.Font.Bold = false;
		lbTask.ForeColor = Color.Blue;
		lbTaskHour.ForeColor = Color.Blue;
		lbTaskHour.Font.Bold = false;
		divDay.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divDayTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divDayTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divWeek.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divWeekTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divWeekTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divMonth.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divMonthTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divMonthTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divYear.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#C3D9FF");
		divYearTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#C3D9FF");
		divYearTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#C3D9FF");
		divTask.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divTaskTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divTaskTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divTaskHour.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divTaskHourTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divTaskHourTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
	}

	void btnToday_Click(object sender, EventArgs e)
	{
		gCalendar.SelectedDate = DateTime.Now;
		gCalendar.Refresh();
		lbDatesRange.Text = GetDatesRange();
		btnToday.Enabled = false;
	}

	void lbMonth_Click(object sender, EventArgs e)
	{
		gCalendar.ViewType = Mediachase.AjaxCalendar.CalendarViewType.Month;
		Session["Calendar_ViewType"] = gCalendar.ViewType;
		gCalendar.Refresh();
		if (gCalendar.SelectedDate.Month == DateTime.Now.Month &&
			gCalendar.SelectedDate.Year == DateTime.Now.Year)
			btnToday.Enabled = false;
		else
			btnToday.Enabled = true;
		lbDatesRange.Text = GetDatesRange();
		lbDay.Font.Bold = false;
		lbDay.ForeColor = Color.Blue;
		lbWeek.Font.Bold = false;
		lbWeek.ForeColor = Color.Blue;
		lbMonth.Font.Bold = true;
		lbMonth.ForeColor = Color.Black;
		lbYear.Font.Bold = false;
		lbYear.ForeColor = Color.Blue;
		lbTask.Font.Bold = false;
		lbTask.ForeColor = Color.Blue;
		lbTaskHour.ForeColor = Color.Blue;
		lbTaskHour.Font.Bold = false;
		divDay.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divDayTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divDayTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divWeek.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divWeekTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divWeekTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divMonth.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#C3D9FF");
		divMonthTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#C3D9FF");
		divMonthTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#C3D9FF");
		divYear.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divYearTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divYearTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divTask.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divTaskTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divTaskTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divTaskHour.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divTaskHourTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divTaskHourTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
	}

	void lbWeek_Click(object sender, EventArgs e)
	{
		gCalendar.ViewType = Mediachase.AjaxCalendar.CalendarViewType.MultiDay;
		gCalendar.ViewMode = Mediachase.AjaxCalendar.ViewModeType.Week;
		gCalendar.Refresh();
		Session["Calendar_ViewMode"] = Mediachase.AjaxCalendar.ViewModeType.Week;
		Session["Calendar_ViewType"] = gCalendar.ViewType;

		DateTime sd = GetViewStartDate();
		DateTime ed = sd.AddDays(6);
		if (DateTime.Now >= sd && DateTime.Now <= ed)
			btnToday.Enabled = false;
		else
			btnToday.Enabled = true;
		lbDatesRange.Text = GetDatesRange();
		lbDay.Font.Bold = false;
		lbDay.ForeColor = Color.Blue;
		lbWeek.Font.Bold = true;
		lbWeek.ForeColor = Color.Black;
		lbMonth.Font.Bold = false;
		lbMonth.ForeColor = Color.Blue;
		lbYear.Font.Bold = false;
		lbYear.ForeColor = Color.Blue;
		lbTask.Font.Bold = false;
		lbTask.ForeColor = Color.Blue;
		lbTaskHour.ForeColor = Color.Blue;
		lbTaskHour.Font.Bold = false;
		divDay.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divDayTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divDayTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divWeek.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#C3D9FF");
		divWeekTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#C3D9FF");
		divWeekTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#C3D9FF");
		divMonth.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divMonthTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divMonthTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divYear.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divYearTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divYearTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divTask.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divTaskTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divTaskTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divTaskHour.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divTaskHourTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divTaskHourTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");

	}

	void lbDay_Click(object sender, EventArgs e)
	{
		gCalendar.ViewType = Mediachase.AjaxCalendar.CalendarViewType.MultiDay;
		Session["Calendar_ViewType"] = gCalendar.ViewType;
		gCalendar.ViewMode = Mediachase.AjaxCalendar.ViewModeType.Day;
		Session["Calendar_ViewMode"] = gCalendar.ViewMode;
		gCalendar.Refresh();
		if (gCalendar.SelectedDate.Day == DateTime.Now.Day &&
			gCalendar.SelectedDate.Month == DateTime.Now.Month &&
			gCalendar.SelectedDate.Year == DateTime.Now.Year)
			btnToday.Enabled = false;
		else
			btnToday.Enabled = true;
		lbDatesRange.Text = GetDatesRange();
		lbDay.Font.Bold = true;
		lbDay.ForeColor = Color.Black;
		lbWeek.Font.Bold = false;
		lbWeek.ForeColor = Color.Blue;
		lbMonth.Font.Bold = false;
		lbMonth.ForeColor = Color.Blue;
		lbYear.Font.Bold = false;
		lbYear.ForeColor = Color.Blue;
		lbTask.Font.Bold = false;
		lbTask.ForeColor = Color.Blue;
		lbTaskHour.ForeColor = Color.Blue;
		lbTaskHour.Font.Bold = false;
		divDay.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#C3D9FF");
		divDayTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#C3D9FF");
		divDayTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#C3D9FF");
		divWeek.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divWeekTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divWeekTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divMonth.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divMonthTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divMonthTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divYear.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divYearTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divYearTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divTask.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divTaskTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divTaskTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divTaskHour.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divTaskHourTop1.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
		divTaskHourTop2.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E8EEF7");
	}

	void ibPrev_Click(object sender, ImageClickEventArgs e)
	{
		switch (this.gCalendar.ViewType)
		{
			case Mediachase.AjaxCalendar.CalendarViewType.MultiDay:
				if (gCalendar.ViewMode == Mediachase.AjaxCalendar.ViewModeType.Day)
				{
					gCalendar.SelectedDate = gCalendar.SelectedDate.AddDays(-1);
				}
				if (gCalendar.ViewMode == Mediachase.AjaxCalendar.ViewModeType.Week ||
					gCalendar.ViewMode == Mediachase.AjaxCalendar.ViewModeType.WorkWeek)
				{
					gCalendar.SelectedDate = gCalendar.SelectedDate.AddDays(-7);
				}
				if (gCalendar.ViewMode == Mediachase.AjaxCalendar.ViewModeType.Custom)
				{
					gCalendar.SelectedDate = gCalendar.SelectedDate.AddDays(-1 * gCalendar.MultiDayView.DaysCount);
				}
				break;
			case Mediachase.AjaxCalendar.CalendarViewType.Month:
				gCalendar.SelectedDate = gCalendar.SelectedDate.AddMonths(-1);
				break;
			case Mediachase.AjaxCalendar.CalendarViewType.Year:
				gCalendar.SelectedDate = gCalendar.SelectedDate.AddYears(-1);
				break;
			case Mediachase.AjaxCalendar.CalendarViewType.Task:
				gCalendar.SelectedDate = gCalendar.SelectedDate.AddDays(-gCalendar.TaskView.DaysCount);
				break;
		}
		gCalendar.Refresh();
		Session["Calendar_SelectedDate"] = gCalendar.SelectedDate;
		UpdateTodayButton();
		lbDatesRange.Text = GetDatesRange();
	}

	void ibNext_Click(object sender, ImageClickEventArgs e)
	{
		switch (this.gCalendar.ViewType)
		{
			case Mediachase.AjaxCalendar.CalendarViewType.MultiDay:
				if (gCalendar.ViewMode == Mediachase.AjaxCalendar.ViewModeType.Day)
				{
					gCalendar.SelectedDate = gCalendar.SelectedDate.AddDays(1);
				}
				if (gCalendar.ViewMode == Mediachase.AjaxCalendar.ViewModeType.Week ||
					gCalendar.ViewMode == Mediachase.AjaxCalendar.ViewModeType.WorkWeek)
				{
					gCalendar.SelectedDate = gCalendar.SelectedDate.AddDays(7);
				}
				if (gCalendar.ViewMode == Mediachase.AjaxCalendar.ViewModeType.Custom)
				{
					gCalendar.SelectedDate = gCalendar.SelectedDate.AddDays(gCalendar.MultiDayView.DaysCount);
				}
				break;
			case Mediachase.AjaxCalendar.CalendarViewType.Month:
				gCalendar.SelectedDate = gCalendar.SelectedDate.AddMonths(1);
				break;
			case Mediachase.AjaxCalendar.CalendarViewType.Year:
				gCalendar.SelectedDate = gCalendar.SelectedDate.AddYears(1);
				break;
			case Mediachase.AjaxCalendar.CalendarViewType.Task:
				gCalendar.SelectedDate = gCalendar.SelectedDate.AddDays(gCalendar.TaskView.DaysCount);
				break;
		}
		gCalendar.Refresh();
		Session["Calendar_SelectedDate"] = gCalendar.SelectedDate;
		UpdateTodayButton();
		lbDatesRange.Text = GetDatesRange();
	}
}
