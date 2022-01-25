using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Web.UI;
using AppointmentBook;
using PowerPOS;
using SubSonic;
using SubSonic.Utilities;

namespace PowerWeb.Appointment
{
	public partial class AppointmentView : Page
	{
		private readonly UserMstCollection salesPersons = new UserMstCollection();

		int _posId;
		DateTime _date = DateTime.Today;

		protected void Page_Load(object sender, EventArgs e)
		{
			var pointsOfSale = new PointOfSaleCollection();
			pointsOfSale.Load();

			if (Page.IsPostBack)
			{
				_posId = int.Parse(cbPointOfSales.SelectedValue);
			}
			else
			{
				if (pointsOfSale.Count > 0)
				{
					_posId = pointsOfSale[0].PointOfSaleID;
					Utility.LoadDropDown(cbPointOfSales, pointsOfSale, PointOfSale.Columns.PointOfSaleName,
					                     PointOfSale.Columns.PointOfSaleID, _posId.ToString());
				}
				Calendar1.SelectedDate = DateTime.Today;
			}

		}
		protected void Page_LoadComplete()
		{
			GenerateView(_posId, _date);
		}

		private void GenerateView(int posId, DateTime date)
		{
			var options = new AppointmentBookControlOptions();

			var setting = new AppSetting(AppSetting.Columns.AppSettingKey, "PointOfSale_WorkingHoursStart").AppSettingValue;
			TimeSpan.TryParse(setting, out options.WorkDayStart);

			setting = new AppSetting(AppSetting.Columns.AppSettingKey, "PointOfSale_WorkingHoursEnd").AppSettingValue;
			TimeSpan.TryParse(setting, out options.WorkDayEnd);

			setting = new AppSetting(AppSetting.Columns.AppSettingKey, "PointOfSale_MinimumTimeInterval").AppSettingValue;
			TimeSpan timeResolution;
			TimeSpan.TryParse(setting, out timeResolution);
			options.TimeResolution = (int) timeResolution.TotalMinutes;

			salesPersons.Where(UserMst.Columns.Deleted, false);
			salesPersons.Where(UserMst.Columns.IsASalesPerson, true);
			salesPersons.Where(UserMst.UserColumns.AssignedPOS, posId);
			salesPersons.Load();


			var appointments = new AppointmentCollection();
			appointments.Where(PowerPOS.Appointment.Columns.Deleted, false);
			appointments.Where(PowerPOS.Appointment.Columns.StartTime, Comparison.GreaterOrEquals, date);
			appointments.Where(PowerPOS.Appointment.Columns.StartTime, Comparison.LessThan, date + TimeSpan.FromDays(1));
			appointments.OrderByAsc(PowerPOS.Appointment.Columns.StartTime);
			appointments.Load();

			var table = new Dictionary<TimeSpan, List<string>>();

			var time = options.WorkDayStart;
			while (time < options.WorkDayEnd)
			{
				table.Add(time, new List<string>());
				time += TimeSpan.FromMinutes(options.TimeResolution);
			}

			foreach (var salesPerson in salesPersons)
			{
				time = options.WorkDayStart;
				while (time < options.WorkDayEnd)
				{
					bool found = false;
					foreach (var appointment in appointments)
					{
						if (appointment.SalesPersonID == salesPerson.UserName)
						{
							if (TimeHelper.Floor(appointment.StartTime, options.TimeResolution) - date == time)
							{
								var rowSpan = Math.Ceiling((float) appointment.Duration / options.TimeResolution);

								var style = string.Format("background-color:{0}; border-color: {1}; color:{2}",
								                          ColorTranslator.ToHtml(Color.FromArgb(appointment.BackColor)),
								                          ColorTranslator.ToHtml(ColorHelper.ModifyColor(
									                          Color.FromArgb(appointment.BackColor), 0.8f)),
								                          ColorTranslator.ToHtml(Color.FromArgb(appointment.FontColor)));

								var title =
									Server.HtmlEncode(string.Format("Start time: {0}\nDuration: {1}\n{2}", appointment.StartTime.ToShortTimeString(),
									                                TimeSpan.FromMinutes(appointment.Duration), appointment.BuildDescription()));

								var check = (appointment.OrderHdrID != null && !(new OrderHdr(appointment.OrderHdrID).IsVoided));

								table[time].Add(string.Format("<td class='timeSlot{4}' style='{0}' rowspan='{1}' title='{3}'>{2}</td>", style,
								                              rowSpan,
								                              Server.HtmlEncode(appointment.BuildDescription()), title,
								                              (check ? " timeSlotCheck" : "")));
								found = true;
								break;
							}

							if (TimeHelper.Floor(appointment.StartTime, options.TimeResolution) - date < time &&
							    TimeHelper.Ceiling(appointment.StartTime + TimeSpan.FromMinutes(appointment.Duration),
							                       options.TimeResolution) - date > time)
							{
								table[time].Add("");
								found = true;
								break;
							}
						}
					}
					if (!found)
						table[time].Add("<td>&nbsp;</td>");


					time += TimeSpan.FromMinutes(options.TimeResolution);
				}
			}

			var tableHtml = new StringBuilder();

			tableHtml.Append(
				"<tr><td class='calendarImage'><img src='../Images/calendar_icon.png' width='64px' height='64px'></td>");
			foreach (var salesPerson in salesPersons)
			{
				string imageSrc;
				if (salesPerson.ImageData != null)
					imageSrc = "UserImage.ashx?ID=" + Server.UrlPathEncode(salesPerson.UserName);
				else
					imageSrc = (salesPerson.Gender ?? false) ? "../Images/male_user.png" : "../Images/female_user.png";

				tableHtml.AppendFormat("<td class='employeeImage'><img src='{0}' height='72px'></td>", imageSrc);
			}
			tableHtml.AppendLine("</tr>");

			tableHtml.Append("<tr><td class='timeHeader'>" + date.ToString("dd MMM yyyy") + "</td>");
			foreach (var salesPerson in salesPersons)
			{
				tableHtml.Append("<td class='employeeName'>" + Server.HtmlEncode(salesPerson.DisplayName) + "</td>");
			}
			tableHtml.AppendLine("</tr>");

			foreach (var row in table)
			{
				tableHtml.Append("<tr>");

				if (row.Key.Minutes == 0)
					tableHtml.Append("<td class='timeMajor'>" + (date + row.Key).ToString("HH:mm") + "</td>");
				else
					tableHtml.Append("<td class='timeMinor'>" + (date + row.Key).ToString(":mm") + "</td>");

				foreach (var cell in row.Value)
					tableHtml.Append(cell);
				tableHtml.AppendLine("</tr>");
			}

			AppointmentView2.InnerHtml = string.Format("<table class='appointmentTable'>\n{0}\n</table>", tableHtml);
		}

		protected void Calendar1_SelectionChanged(object sender, EventArgs e)
		{
			_date = Calendar1.SelectedDate;
		}
	}
}

