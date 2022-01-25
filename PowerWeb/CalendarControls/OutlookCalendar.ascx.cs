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

public partial class CalendarControls_OutlookCalendar : System.Web.UI.UserControl
{
	protected void Page_Load(object sender, EventArgs e)
	{
		oCalendar.MultiDayView.ItemTemplate = "<div style=\"width:100%; height:100%; background-color:#fff; border: solid 1px #000; \">"+
			"<span style='float:left;'><img border='0' src='Images/o_repeat_white.gif' id=\"isRecurring\"/></span>" +
			"<div id=\"title\" mcc_action=\"move\" style=\"font-weight:bold;cursor:move; min-width:20px;\" unselectable=\"on\"></div>"+
			"<div id=\"description\" unselectable=\"on\"></div>"+
			"<div mcc_action=\"resize\" style=\"line-height:1px; font-size:1px; position:absolute; bottom:0px; height:1px;  width:100%; border-bottom: solid 1px #000; z-index:11; cursor:s-resize;\"></div>"+
			"<div style=\"position:absolute; height:100%; width:1px; right:0px; top:0px; z-ndex:11; border-right: solid 1px #000;\"></div></div>";
		oCalendar.MultiDayView.ItemMapping= "[{\"id\":\"title\",\"property\":\"innerHTML\",\"value\":\"Title\"},"+
			"{\"id\":\"description\",\"property\":\"innerHTML\",\"value\":\"Description\"},"+
			"{\"id\":\"isRecurring\", \"property\":\"style.display\", \"value\":\"Extensions==true ? 'block':'none'\"}]";
		oCalendar.MultiDayView.EventBarItemTemplate = "<div style=\"width:100%; height:100%; background-color:#fff; border: solid 1px #000;\">"+
			"<table border='0' cellpadding='0' cellspacing='0'><tr>"+
			"<td><img border='0' src='Images/o_repeat_white.gif' id=\"isRecurring\"/></td>" +
			"<td id=\"title\" mcc_action=\"move\" style=\"font-weight:bold;cursor:move; min-width:20px;\" unselectable=\"on\"></td>"+
			"<td>(</td>"+
			"<td id=\"description\" unselectable=\"on\"></td><td>)</td>"+
			"</tr></table>" +
			"<div style=\"position:absolute; height:100%; width:1px; top:0px; right:0px; border-right:solid 1px black;\"></div>"+
			"<div style=\"position:absolute; height:1px; width:100%;left:0px;  bottom:0px; z-index:11; border-bottom:solid 1px black;\"></div></div>";
		oCalendar.MultiDayView.EventBarItemMapping = "[{\"id\":\"title\",\"property\":\"innerHTML\",\"value\":\"Title\"},"+
			"{\"id\":\"description\",\"property\":\"innerHTML\",\"value\":\"Description\"},"+
			"{\"id\":\"isRecurring\", \"property\":\"style.display\", \"value\":\"Extensions==true ? 'block':'none'\"}]";
		oCalendar.MonthView.ItemTemplate = "<div style=\"width:100%; height:20px; background-color:#fff; border: solid 1px #000; overflow:hidden; table-layout:fixed; \">"+
			"<table border='0'><tr>"+
			"<td><img border='0' src='Images/o_repeat_white.gif' id=\"isRecurring\" style='float:left;'/></td>"+
			"<td id=\"title\" mcc_action=\"move\" style=\"font-weight:bold;cursor:move;\" unselectable=\"on\"></td>" +
			"<td>(</td><td id=\"description\" unselectable=\"on\"></td><td>)</td></tr></table>"+
			"<div style=\"position:absolute; height:100%; width:1px; top:0px; right:0px; border-right:solid 1px black;\"></div>"+
			"<div style=\"position:absolute; height:1px; width:100%;left:0px;  bottom:0px; z-index:11; border-bottom:solid 1px black;\"></div></div>";
		oCalendar.MonthView.ItemMapping= "[{\"id\":\"title\",\"property\":\"innerHTML\",\"value\":\"Title\"},"+
			"{\"id\":\"description\",\"property\":\"innerHTML\",\"value\":\"Description\"},"+
			"{\"id\":\"isRecurring\", \"property\":\"style.display\", \"value\":\"Extensions==true ? 'block':'none'\"}]";
		oCalendar.YearView.ItemTemplate = "<div style=\"width:100%; height:20px; background-color:#fff; border: solid 1px #000; overflow:hidden; table-layout:fixed; \">" +
			"<table border='0'><tr>" +
			"<td><img border='0' src='Images/o_repeat_white.gif' id=\"isRecurring\" style='float:left;'/></td>" +
			"<td id=\"title\" mcc_action=\"move\" style=\"font-weight:bold;\" unselectable=\"on\"></td>" +
			"<td>(</td><td id=\"description\" unselectable=\"on\"></td><td>)</td></tr></table>" +
			"<div style=\"position:absolute; height:100%; width:1px; top:0px; right:0px; border-right:solid 1px black;\"></div>" +
			"<div style=\"position:absolute; height:1px; width:100%;left:0px;  bottom:0px; z-index:11; border-bottom:solid 1px black;\"></div></div>";
		oCalendar.YearView.ItemMapping = oCalendar.MonthView.ItemMapping;
		oCalendar.TaskView.ItemTemplate = "<div id=\"main\" style=\"width:100%; height:100%; border: solid 1px black; background-color:white; padding:4px;\">"+
			"<img border='0' src='Images/o_repeat_white.gif' id=\"isRecurring\" style='float:left;'/>" +
			"<div style=\"position:absolute; left:0px; top:0px; width:15px; height:100%; cursor:move;\" mcc_action=\"move\"></div></div>"+
			"<div style=\"position:absolute; height:100%; width:1px; right:0px; top:0px; border-right: solid 1px black;\"></div>"+
			"<div style=\"position:absolute; height:1px; width:100%; left:0px; bottom:0px; z-index:11; border-bottom: solid 1px black;\"></div>";
		oCalendar.TaskView.ItemMapping = "[{\"id\":\"isRecurring\", \"property\":\"style.display\", \"value\":\"Extensions==true ? 'block':'none'\"},"+
			"{\"id\":\"main\", \"property\":\"title\", \"value\":\"StartDate.format('MM/dd/yy HH:mm')+' - '+item.EndDate.format('MM/dd/yy HH:mm')\"}" +
			"]"; 
		if (!IsPostBack)
		{
			if (Session["Calendar_ViewType"] != null)
			{
				oCalendar.ViewType = (Mediachase.AjaxCalendar.CalendarViewType)Session["Calendar_ViewType"];
				if (Session["Calendar_TaskScale"] != null && oCalendar.ViewType == Mediachase.AjaxCalendar.CalendarViewType.Task)
					oCalendar.TaskView.ScaleType = (Mediachase.AjaxCalendar.TaskScaleType)Session["Calendar_TaskScale"];
			}
			if (Session["Calendar_ViewMode"] != null)
			{
				oCalendar.ViewMode = (Mediachase.AjaxCalendar.ViewModeType)Session["Calendar_ViewMode"];
			}
			if (Session["Calendar_SelectedDate"] != null)
			{
				oCalendar.SelectedDate = (DateTime)Session["Calendar_SelectedDate"];
				cSmall.SelectedDate = oCalendar.SelectedDate;
				cSmall.VisibleDate = cSmall.SelectedDate;
			}
			oCalendar.Refresh();
			BindData();
			
		}
	}

	protected void Page_Init(object sender, EventArgs e)
	{
		this.cSmall.SelectionChanged += new EventHandler(cSmall_SelectionChanged);
		this.cSmall.VisibleMonthChanged += new MonthChangedEventHandler(cSmall_VisibleMonthChanged);
		this.lbToday.Click += new EventHandler(lbToday_Click);
		this.lbDayView.Click += new EventHandler(lbDayView_Click);
		this.lbWorkWeekView.Click += new EventHandler(lbWorkWeekView_Click);
		this.lbWeekView.Click += new EventHandler(lbWeekView_Click);
		this.lbMonthView.Click += new EventHandler(lbMonthView_Click);
		this.lbYearView.Click += new EventHandler(lbYearView_Click);
		this.lbTaskView.Click += new EventHandler(lbTaskView_Click);
		this.lbTaskHourView.Click += new EventHandler(lbTaskHourView_Click);
	}

	void lbTaskHourView_Click(object sender, EventArgs e)
	{
		oCalendar.ViewType = Mediachase.AjaxCalendar.CalendarViewType.Task;
		oCalendar.TaskView.DaysCount = 3;
		oCalendar.TaskView.HeaderDayFormat = "dd, MMMM";
		oCalendar.TaskView.ScaleType = Mediachase.AjaxCalendar.TaskScaleType.Hour;
		oCalendar.Refresh();
		Session["Calendar_ViewType"] = oCalendar.ViewType;
		Session["Calendar_TaskScale"] = Mediachase.AjaxCalendar.TaskScaleType.Hour;
		Session["Calendar_SelectedDate"] = oCalendar.SelectedDate;
		cSmall.SelectedDate = oCalendar.SelectedDate;
		BindData();
	}

	void lbTaskView_Click(object sender, EventArgs e)
	{
		oCalendar.ViewType = Mediachase.AjaxCalendar.CalendarViewType.Task;
		oCalendar.TaskView.DaysCount = 30;
		oCalendar.TaskView.HeaderDayFormat = "dd";
		oCalendar.TaskView.ScaleType = Mediachase.AjaxCalendar.TaskScaleType.Day;
		oCalendar.Refresh();
		Session["Calendar_ViewType"] = oCalendar.ViewType;
		Session["Calendar_TaskScale"] = Mediachase.AjaxCalendar.TaskScaleType.Day;
		Session["Calendar_SelectedDate"] = oCalendar.SelectedDate;
		cSmall.SelectedDate = oCalendar.SelectedDate;
		BindData();
	}

	void lbYearView_Click(object sender, EventArgs e)
	{
		oCalendar.ViewType = Mediachase.AjaxCalendar.CalendarViewType.Year;
		oCalendar.Refresh();
		Session["Calendar_ViewType"] = oCalendar.ViewType;
		Session["Calendar_SelectedDate"] = oCalendar.SelectedDate;
		cSmall.SelectedDate = oCalendar.SelectedDate;
		BindData();
	}

	void lbMonthView_Click(object sender, EventArgs e)
	{
		oCalendar.ViewType = Mediachase.AjaxCalendar.CalendarViewType.Month;
		oCalendar.Refresh();
		Session["Calendar_ViewType"] = oCalendar.ViewType;
		Session["Calendar_SelectedDate"] = oCalendar.SelectedDate;
		cSmall.SelectedDate = oCalendar.SelectedDate;
		BindData();
	}

	void lbWeekView_Click(object sender, EventArgs e)
	{
		oCalendar.ViewType = Mediachase.AjaxCalendar.CalendarViewType.MultiDay;
		oCalendar.ViewMode = Mediachase.AjaxCalendar.ViewModeType.Week;
		oCalendar.Refresh();
		Session["Calendar_ViewType"] = oCalendar.ViewType;
		Session["Calendar_ViewMode"] = oCalendar.ViewMode;
		Session["Calendar_SelectedDate"] = oCalendar.SelectedDate;
		cSmall.SelectedDate = oCalendar.SelectedDate;
		BindData();
	}

	void lbWorkWeekView_Click(object sender, EventArgs e)
	{
		oCalendar.ViewType = Mediachase.AjaxCalendar.CalendarViewType.MultiDay;
		oCalendar.ViewMode = Mediachase.AjaxCalendar.ViewModeType.WorkWeek;
		oCalendar.Refresh();
		Session["Calendar_ViewType"] = oCalendar.ViewType;
		Session["Calendar_ViewMode"] = oCalendar.ViewMode;
		Session["Calendar_SelectedDate"] = oCalendar.SelectedDate;
		cSmall.SelectedDate = oCalendar.SelectedDate;
		BindData();
	}

	void lbDayView_Click(object sender, EventArgs e)
	{
		oCalendar.ViewType = Mediachase.AjaxCalendar.CalendarViewType.MultiDay;
		oCalendar.ViewMode = Mediachase.AjaxCalendar.ViewModeType.Day;
		oCalendar.Refresh();
		Session["Calendar_ViewType"] = oCalendar.ViewType;
		Session["Calendar_ViewMode"] = oCalendar.ViewMode;
		Session["Calendar_SelectedDate"] = oCalendar.SelectedDate;
		cSmall.SelectedDate = oCalendar.SelectedDate;
		BindData();
	}

	void lbToday_Click(object sender, EventArgs e)
	{
		cSmall.SelectedDate = oCalendar.SelectedDate = DateTime.Now;
		Session["Calendar_SelectedDate"] = oCalendar.SelectedDate;
		oCalendar.Refresh();
		BindData();
		cSmall_SelectionChanged(sender, e);
		cSmall.SelectedDate = DateTime.Now;

	}

	void cSmall_VisibleMonthChanged(object sender, MonthChangedEventArgs e)
	{
		oCalendar.SelectedDate = cSmall.SelectedDate = e.NewDate;
		oCalendar.ViewType = Mediachase.AjaxCalendar.CalendarViewType.Month;
		oCalendar.Refresh();
		Session["Calendar_ViewType"] = oCalendar.ViewType;
		Session["Calendar_ViewMode"] = oCalendar.ViewMode;
		Session["Calendar_SelectedDate"] = oCalendar.SelectedDate;
		BindData();
		
	}

	void cSmall_SelectionChanged(object sender, EventArgs e)
	{
		oCalendar.SelectedDate = cSmall.SelectedDate;
		oCalendar.Refresh();
		Session["Calendar_ViewType"] = oCalendar.ViewType;
		Session["Calendar_ViewMode"] = oCalendar.ViewMode;
		Session["Calendar_SelectedDate"] = oCalendar.SelectedDate;
		BindData();
	}

	private void BindData()
	{
		lbDatesRange.Text = GetDatesRange();
		switch (oCalendar.ViewType)
		{
			case Mediachase.AjaxCalendar.CalendarViewType.MultiDay:
				if (oCalendar.ViewMode == Mediachase.AjaxCalendar.ViewModeType.Day)
				{
					lbDayView.BackColor = ColorTranslator.FromHtml("#FFC070");
					lbWeekView.BackColor = Color.Empty;
					lbWorkWeekView.BackColor = Color.Empty;
					lbMonthView.BackColor = Color.Empty;
					lbYearView.BackColor = Color.Empty;
					lbTaskView.BackColor = Color.Empty;
					lbTaskHourView.BackColor = Color.Empty;
				}
				if (oCalendar.ViewMode == Mediachase.AjaxCalendar.ViewModeType.WorkWeek)
				{
					lbDayView.BackColor = Color.Empty;
					lbWeekView.BackColor = Color.Empty;
					lbWorkWeekView.BackColor = ColorTranslator.FromHtml("#FFC070");
					lbMonthView.BackColor = Color.Empty;
					lbYearView.BackColor = Color.Empty;
					lbTaskView.BackColor = Color.Empty;
					lbTaskHourView.BackColor = Color.Empty;
				}
				if (oCalendar.ViewMode == Mediachase.AjaxCalendar.ViewModeType.Week)
				{
					lbDayView.BackColor = Color.Empty;
					lbWeekView.BackColor = ColorTranslator.FromHtml("#FFC070");
					lbWorkWeekView.BackColor = Color.Empty;
					lbMonthView.BackColor = Color.Empty;
					lbYearView.BackColor = Color.Empty;
					lbTaskView.BackColor = Color.Empty;
					lbTaskHourView.BackColor = Color.Empty;
				}
				break;
			case Mediachase.AjaxCalendar.CalendarViewType.Month:
				lbDayView.BackColor = Color.Empty;
				lbWeekView.BackColor = Color.Empty;
				lbWorkWeekView.BackColor = Color.Empty;
				lbMonthView.BackColor = ColorTranslator.FromHtml("#FFC070");
				lbYearView.BackColor = Color.Empty;
				lbTaskHourView.BackColor = Color.Empty;
				lbTaskView.BackColor = Color.Empty;
				break;
			case Mediachase.AjaxCalendar.CalendarViewType.Year:
				lbDayView.BackColor = Color.Empty;
				lbWeekView.BackColor = Color.Empty;
				lbWorkWeekView.BackColor = Color.Empty;
				lbMonthView.BackColor = Color.Empty;
				lbYearView.BackColor = ColorTranslator.FromHtml("#FFC070");
				lbTaskView.BackColor = Color.Empty;
				lbTaskHourView.BackColor = Color.Empty;
				break;
			case Mediachase.AjaxCalendar.CalendarViewType.Task:
				lbDayView.BackColor = Color.Empty;
				lbWeekView.BackColor = Color.Empty;
				lbWorkWeekView.BackColor = Color.Empty;
				lbMonthView.BackColor = Color.Empty;
				lbYearView.BackColor = Color.Empty;
				if (Session["Calendar_TaskScale"] != null)
				{
					Mediachase.AjaxCalendar.TaskScaleType taskType = (Mediachase.AjaxCalendar.TaskScaleType)Session["Calendar_TaskScale"];
					switch (taskType)
					{
						case Mediachase.AjaxCalendar.TaskScaleType.Day:
							lbTaskView.BackColor = ColorTranslator.FromHtml("#FFC070");
							lbTaskHourView.BackColor = Color.Empty;
							oCalendar.TaskView.DaysCount = 30;
							oCalendar.TaskView.HeaderDayFormat = "dd";
							oCalendar.TaskView.ScaleType = Mediachase.AjaxCalendar.TaskScaleType.Day;
							break;
						case Mediachase.AjaxCalendar.TaskScaleType.Hour:
							lbTaskView.BackColor = Color.Empty;
							lbTaskHourView.BackColor = ColorTranslator.FromHtml("#FFC070");
							oCalendar.TaskView.DaysCount = 3;
							oCalendar.TaskView.HeaderDayFormat = "dd, MMMM";
							oCalendar.TaskView.ScaleType = Mediachase.AjaxCalendar.TaskScaleType.Hour;
							break;
					}
				}
				else
				{
					lbTaskView.BackColor = ColorTranslator.FromHtml("#FFC070");
					lbTaskHourView.BackColor = Color.Empty;
					oCalendar.TaskView.DaysCount = 30;
					oCalendar.TaskView.HeaderDayFormat = "dd";
					oCalendar.TaskView.ScaleType = Mediachase.AjaxCalendar.TaskScaleType.Day;
				}
				//lbTaskView.BackColor = ColorTranslator.FromHtml("#FFC070");
				break;
		}
	}

	protected string GetDatesRange()
	{
		switch (oCalendar.ViewType)
		{
			case Mediachase.AjaxCalendar.CalendarViewType.MultiDay:
				if (oCalendar.ViewMode == Mediachase.AjaxCalendar.ViewModeType.Day)
					return oCalendar.SelectedDate.ToString("d MMMM yyyy");
				if (oCalendar.ViewMode == Mediachase.AjaxCalendar.ViewModeType.Week ||
					oCalendar.ViewMode == Mediachase.AjaxCalendar.ViewModeType.WorkWeek)
				{
					DateTime sd = oCalendar.SelectedDate;
					DateTime ed = oCalendar.SelectedDate;
					while ((int)sd.DayOfWeek != (int)oCalendar.MultiDayView.WeekStartDay)
					{
						sd = sd.AddDays(-1);
					}
					if (oCalendar.ViewMode == Mediachase.AjaxCalendar.ViewModeType.Week)
						ed = sd.AddDays(6);
					if (oCalendar.ViewMode == Mediachase.AjaxCalendar.ViewModeType.WorkWeek)
						ed = sd.AddDays(4);
					if (sd.Year == ed.Year)
						return sd.ToString("d MMMM") + " - " + ed.ToString("d MMMM");
					if ((sd.Year != ed.Year))
						return sd.ToString("d MMMM yyyy") + " - " + ed.ToString("d MMMM yyyy");
				}
				return string.Empty;
			case Mediachase.AjaxCalendar.CalendarViewType.Month:
				return oCalendar.SelectedDate.ToString("MMMM yyyy");
			case Mediachase.AjaxCalendar.CalendarViewType.Year:
				return oCalendar.SelectedDate.ToString("yyyy");
			case Mediachase.AjaxCalendar.CalendarViewType.Task:
				return oCalendar.TaskView.ViewStartDate.ToString("d MMMM") + " - " + oCalendar.TaskView.ViewEndDate.ToString("d MMMM");
			default:
				return string.Empty;
		}
	}
}
