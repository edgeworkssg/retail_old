using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using AppointmentBook.Model;
using AppointmentBook.Properties;
using System.Linq;
using System.Diagnostics;

namespace AppointmentBook
{
    public class AppointmentBookMonthlyRenderer
    {
        private AppointmentBookControlOptions _options;

		private static readonly Pen _pen = new Pen(Color.Empty, 1);
		private static readonly SolidBrush _brush = new SolidBrush(Color.Empty);
		private static readonly StringFormat _stringFormat = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center, Trimming = StringTrimming.EllipsisCharacter };

        public AppointmentBookMonthlyRenderer(AppointmentBookControlOptions options)
		{
			_options = options;
		}
		public AppointmentBookControlOptions Options
		{
			set { _options = value; }
		}

		public void RenderTimeScaleElement(Graphics g, string dayName, int x, int y, int w, int h)
		{
			var rect = new RectangleF(x, y, w, h);
			_brush.Color = Color.Gray;

			g.FillRectangle(_brush, rect);

			_stringFormat.Alignment = StringAlignment.Center;
			_stringFormat.LineAlignment = StringAlignment.Center;

            _brush.Color = Color.White;
            g.DrawString(dayName, _options.TimeMajorFont, _brush, rect, _stringFormat);
		}

		public void RenderTimeScale(Graphics g, DateTime startTime, int x, int y, int w, int h)
		{
			startTime = TimeHelper.Floor(startTime, _options.TimeResolution);
			_pen.Color = Color.Silver;
			_pen.Width = 1;

            int width = ((w) / _options.DayInWeek);
            for (int i = 0; i < _options.DayInWeek; i++)
            {
                string dayName = CultureInfo.CurrentCulture.DateTimeFormat.DayNames[i];
                RenderTimeScaleElement(g, dayName, x + (i * width), y, width, h);
            }
		}

        public void RenderTimeScaleHeader(Graphics g, DateTime startTime, int x, int y, int w, int h)
		{
			var rect = new RectangleF(x, y, w, h);
			_brush.Color = Color.Gray;
			g.FillRectangle(_brush, rect);

			var imageRect = new RectangleF(x + w * 0.05f, y + h * 0.05f, w * 0.9f, h * 0.7f);
			EnframeImage(g, Resources.calendar_icon, imageRect);

			var textFrameRect = new RectangleF(x, y + h * 0.75f, w, h * 0.25f);
			_brush.Color = Color.FromArgb(64, 64, 64);
			g.FillRectangle(_brush, textFrameRect);

			var textRect = new RectangleF(x + w * 0.05f, y + h * 0.75f, w * 0.9f, h * 0.25f);
			_brush.Color = Color.White;
			_stringFormat.Alignment = StringAlignment.Center;
			_stringFormat.LineAlignment = StringAlignment.Center;
			g.DrawString(startTime.ToString("dd MMM"), _options.TimeMajorFont, _brush, textRect, _stringFormat);

			_pen.Color = Color.Silver;
			_pen.Width = 1;
			g.DrawLine(_pen, rect.Right, rect.Top, rect.Right, rect.Bottom);
			g.DrawLine(_pen, rect.Left, rect.Bottom, rect.Right, rect.Bottom);
		}

		public void RenderEmployeeBackground(Graphics g, int x, int y, int w, int h)
		{
			var rect = new RectangleF(x, y, w, h);
			_brush.Color = Color.Gray;
			g.FillRectangle(_brush, rect);

			var textFrameRect = new RectangleF(x, y + h * 0.75f, w, h * 0.25f);
			_brush.Color = Color.FromArgb(64, 64, 64);
			g.FillRectangle(_brush, textFrameRect);

			_pen.Color = Color.Silver;
			g.DrawLine(_pen, rect.Right, rect.Top, rect.Right, rect.Bottom);
			g.DrawLine(_pen, rect.Left, rect.Bottom, rect.Right, rect.Bottom);
		}

		public void RenderEmployee(Graphics g, Employee employee, int x, int y, int w, int h)
		{
			RenderEmployeeBackground(g, x, y, w, h);

			var image = employee.Image ?? ((employee.Gender == EmployeeGender.Female) ? Resources.female_user : Resources.male_user);
			var imageRect = new RectangleF(x + w * 0.05f, y + h * 0.05f, w * 0.9f, h * 0.7f);
			
			EnframeImage(g, image, imageRect);

			var textRect = new RectangleF(x + w * 0.05f, y + h * 0.75f, w * 0.9f, h * 0.25f);
			_brush.Color = Color.White;
			_stringFormat.Alignment = StringAlignment.Center;
			_stringFormat.LineAlignment = StringAlignment.Center;
			g.DrawString(employee.Name, _options.TimeMajorFont, _brush, textRect, _stringFormat);
		}

		public static void EnframeImage(Graphics g, Image image, RectangleF rect)
		{
			var sw = rect.Width / image.Width;
			var sh = rect.Height / image.Height;
			var s = Math.Min(sw, sh);
			g.DrawImage(image, rect.X + (rect.Width - image.Width * s) / 2, rect.Y + (rect.Height - image.Height * s) / 2,
			            image.Width * s, image.Height * s);
		}

        public void RenderTimeSlot(Graphics g, TimeSlot timeSlot, bool hovered, bool selected, bool resized, byte alpha, int x, int y, int w, int h)
        {
            timeSlot.Region = new Rectangle(x, y, w, h);

            x += 2;
            y += 2;
            w -= 4;
            h -= 4;

            if (w < 16 || h < 16)
                return;

            const int corner = 7;
            const int corner2 = corner * 2;

            var path = new GraphicsPath();

            path.AddArc(x, y, corner2, corner2, 180, 90);
            path.AddLine(x + corner, y, x + w - corner, y);
            path.AddArc(x + w - corner2, y, corner2, corner2, 270, 90);
            path.AddLine(x + w, y + corner, x + w, y + h - corner);
            path.AddArc(x + w - corner2, y + h - corner2, corner2, corner2, 0, 90);
            path.AddLine(x + w - corner, y + h, x + corner, y + h);
            path.AddArc(x, y + h - corner2, corner2, corner2, 90, 90);
            path.CloseFigure();

            var path2 = new GraphicsPath();

            path2.AddArc(x, y, corner2, corner2, 180, 90);
            path2.AddLine(x + corner, y, x + w - corner, y);
            path2.AddArc(x + w - corner2, y, corner2, corner2, 270, 90);
            path2.AddLine(x + w, y + corner, x + w, y + h / 2);
            path2.AddCurve(new[]
				{
					new Point(x + w, y + h / 2),
					new Point(x + 2 * w / 3, (int) (y + h / 2 + w * 0.06f)),
					new Point(x + w / 3, (int) (y + h / 2 + w * 0.06f)),
					new Point(x, y + h / 2)
				});
            path2.CloseFigure();

            _brush.Color = Color.FromArgb(alpha, timeSlot.BackColor);
            g.FillPath(_brush, path);

            _brush.Color = Color.FromArgb(alpha, ColorHelper.ModifyColor(timeSlot.BackColor, 1.1f));
            g.FillPath(_brush, path2);

            Color borderColor;

            if (selected)
                borderColor = Color.FromArgb(alpha, ColorHelper.ModifyColor(timeSlot.BackColor, 0.5f));
            else
                borderColor = Color.FromArgb(alpha, ColorHelper.ModifyColor(timeSlot.BackColor, 0.9f));

            if (hovered)
                borderColor = Color.FromArgb(alpha, ColorHelper.ModifyColor(borderColor, 0.8f));

            _pen.Width = 3;
            _pen.Color = borderColor;
            g.DrawPath(_pen, path);

            if (timeSlot.Check)
            {
                if (w < Resources.Check.Width || h < Resources.Check.Height)
                    g.DrawImage(Resources.Check_small, x + w - Resources.Check_small.Width, y);
                else
                    g.DrawImage(Resources.Check, x + w - Resources.Check.Width, y);

            }

            var textRect = new Rectangle(x + corner, y + corner, w - corner * 2, h - corner * 2);

            Font font = _options.TimeSlotFont;

            var size = g.MeasureString(timeSlot.Description, font, textRect.Width);

            if (size.Height > textRect.Height)
            {
                font = _options.TimeSlotSmallFont;
                size = g.MeasureString(timeSlot.Description, font, textRect.Width);
            }
            if (size.Height > textRect.Height)
            {
                textRect.Inflate(4, 4);
                size = g.MeasureString(timeSlot.Description, font, textRect.Width);
            }

            if (size.Height > textRect.Height)
                _stringFormat.LineAlignment = StringAlignment.Near;
            else
                _stringFormat.LineAlignment = StringAlignment.Center;

            _brush.Color = Color.FromArgb(alpha, timeSlot.FontColor);

            g.DrawString(timeSlot.Description, font, _brush, textRect, _stringFormat);

            if (hovered || resized)
            {
                var handleRect = new Rectangle(x + w / 3, y + h - 12, w / 3, 12);

                _brush.Color = Color.FromArgb(alpha, ColorHelper.ModifyColor(timeSlot.BackColor, 1.3f));
                g.FillRectangle(_brush, handleRect);

                _pen.Width = 2;
                _pen.Color = borderColor;
                g.DrawRectangle(_pen, handleRect);

                _pen.Width = 3;
                g.DrawLine(_pen, handleRect.X + 7, handleRect.Y + 6, handleRect.Right - 8,
                           handleRect.Y + 6);
            }
        }

        public void RenderTimeSlotMonthly(Graphics g, TimeSlot timeSlot, bool hovered, bool selected, bool resized, byte alpha, int x, int y, int w, int h, bool drawBorder)
        {
            var rect = new Rectangle(x+1, y+1, w-2,h-2);
            timeSlot.Region = rect;
            _brush.Color = timeSlot.BackColor;
            g.FillRectangle(_brush, rect);

            if (drawBorder)
            {
                var prevColor = _pen.Color;
                _pen.Width = _pen.Width + 1;

                if(selected)
                    _pen.Color = Color.FromArgb(alpha, ColorHelper.ModifyColor(timeSlot.BackColor, 0.4f));
                else
                    _pen.Color = Color.FromArgb(alpha, ColorHelper.ModifyColor(timeSlot.BackColor, 0.9f));
                if(hovered)
                    _pen.Color = Color.FromArgb(alpha, ColorHelper.ModifyColor(timeSlot.BackColor, 0.6f));

                g.DrawRectangle(_pen, rect.X, rect.Y, rect.Width, rect.Height);
                _pen.Color = prevColor;
                _pen.Width = _pen.Width - 1;
            }

            _stringFormat.Alignment = StringAlignment.Center;
            _stringFormat.LineAlignment = StringAlignment.Center;

            _brush.Color = timeSlot.FontColor;

            if (timeSlot.Check)
            {
                if (timeSlot.IsPartialPayment)
                {
                    g.DrawImage(Resources.CheckY_small, x + w - Resources.CheckY_small.Width - 3, y + 5);
                    rect.Width = rect.Width - Resources.CheckY_small.Width - 3;
                }
                else
                {
                    g.DrawImage(Resources.Check_small, x + w - Resources.Check_small.Width - 3, y + 5);
                    rect.Width = rect.Width - Resources.Check_small.Width - 3;
                }

                g.DrawString(string.Format("[{0}] {1}", timeSlot.Employee.Name,timeSlot.CompleteDescription), 
                    _options.TimeSlotVerySmallFont, _brush, rect, _stringFormat);
            }
            else
            {
                g.DrawString(string.Format("[{0}] {1}", timeSlot.Employee.Name, timeSlot.CompleteDescription), 
                    _options.TimeSlotVerySmallFont, _brush, rect, _stringFormat);
            }
        }

        public void RenderUpDownArrow(Graphics g, string text, byte alpha, int x, int y, int width, int height, int upDownPosition)
        {
            _brush.Color = Color.Gray;
            var rect = new RectangleF(x + 1, y + 1, width-2, (height) - 2);
            g.FillRectangle(_brush, rect);

            var prevColor = _pen.Color;
            _pen.Width = _pen.Width + 1;
            
            if(upDownPosition == 1)
                _pen.Color = Color.FromArgb(alpha, ColorHelper.ModifyColor(_brush.Color, 0.5f));
            else
                _pen.Color = Color.FromArgb(alpha, ColorHelper.ModifyColor(_brush.Color, 0.8f));
            g.DrawRectangle(_pen, rect.X, rect.Y, rect.Width, rect.Height);
            _pen.Color = prevColor;
            _pen.Width = _pen.Width - 1;

            GraphicsPath leftArrow = new GraphicsPath();
            PointF pLeft = new PointF(rect.X + 5, rect.Y + 4);
            PointF pLeft2 = new PointF(pLeft.X + 24, pLeft.Y);
            PointF pLeft3 = new PointF(pLeft.X + 12, pLeft.Y + (rect.Height - 8));
            leftArrow.AddLine(pLeft, pLeft2);
            leftArrow.AddLine(pLeft2, pLeft3);
            leftArrow.AddLine(pLeft3, pLeft);

            GraphicsPath rightArrow = new GraphicsPath();
            PointF pRight = new PointF(pLeft.X + rect.Width - 35, pLeft.Y);
            PointF pRight2 = new PointF(pLeft2.X + rect.Width - 35, pLeft2.Y);
            PointF pRight3 = new PointF(pLeft3.X + rect.Width - 35, pLeft3.Y);
            rightArrow.AddLine(pRight, pRight2);
            rightArrow.AddLine(pRight2, pRight3);
            rightArrow.AddLine(pRight3, pRight);

            g.FillPath((upDownPosition == 1) ? Brushes.Gainsboro : Brushes.DarkGray, leftArrow);
            g.FillPath((upDownPosition == 1) ? Brushes.Gainsboro : Brushes.DarkGray, rightArrow);

            _brush.Color = Color.WhiteSmoke;
            var textRect = new RectangleF(rect.X+30, rect.Y, rect.Width-60, rect.Height);
            g.DrawString(text, _options.TimeSlotSmallFont, _brush, textRect, _stringFormat);
        }

        public void RenderSchedule(Graphics g, List<Employee> employees, TimeSlot hoveredTimeSlot, TimeSlot selectedTimeSlot, TimeSlot resizedTimeSlot, bool drag, DateTime startTime, DateTime currentDate, int x, int y, int w, int h, int upDownPosition, DateTime hoveredDate)
        {
            var rect = new RectangleF(x, y, w, h);
            var dateLabelRect = new RectangleF(x, y, w / 5, _options.MonthlyDateHeaderHeight);

            _pen.Color = Color.WhiteSmoke;
            _pen.Width = 1;
            g.DrawRectangle(_pen, rect.X, rect.Y, rect.Width, rect.Height);

            if (startTime.Month == currentDate.Month)
            {
                _brush.Color = Color.LightGray;
                g.FillRectangle(_brush, rect);
            }
            else
            {
                _brush.Color = ColorHelper.ModifyColor(Color.LightGray, 0.9f);
                g.FillRectangle(_brush, rect); 
            }

            _stringFormat.Alignment = StringAlignment.Center;
            _stringFormat.LineAlignment = StringAlignment.Center;
            _brush.Color = Color.Black;
            if(startTime.Day == 1)
                g.DrawString(startTime.ToString("dd/MMM"), _options.TimeSlotVerySmallFont, _brush, dateLabelRect, _stringFormat);
            else
                g.DrawString(startTime.ToString("dd"), _options.TimeSlotVerySmallFont, _brush, dateLabelRect, _stringFormat);

            var allSchedule = new List<TimeSlot>();
            foreach (var emp in employees)
                allSchedule.AddRange(emp.Schedule);
            var todaySchedule = (from o in allSchedule
                                 where o.StartTime.Date == startTime.Date
                                 orderby o.StartTime ascending, o.Employee.Name ascending
                                 select o).ToList();
            int slotCount = todaySchedule.Count > _options.TimeSlotMax ? _options.TimeSlotMax-1 : todaySchedule.Count;
            byte alpha = 255;
            Debug.WriteLine(string.Format("Date:{0}, ScheduleCount:{1}, SlotCount:{2} ",
                startTime.ToShortDateString(), todaySchedule.Count, slotCount));
            for (int i = 0; i < slotCount; i++)
            {
                RenderTimeSlotMonthly(g, todaySchedule[i],
                    todaySchedule[i] == hoveredTimeSlot,
                    todaySchedule[i] == selectedTimeSlot,
                    todaySchedule[i] == resizedTimeSlot,
                    alpha, x, _options.MonthlyDateHeaderHeight + y + (i * _options.TimeSlotHeight),
                    w, _options.TimeSlotHeight, true);
            }
            if (todaySchedule.Count - (_options.TimeSlotMax - 1) > 0)
            {
                upDownPosition = hoveredDate.Date == startTime.Date ? upDownPosition : 0;
                int moreBook = todaySchedule.Count - (_options.TimeSlotMax - 1);
                RenderUpDownArrow(g, string.Format("See {0} more booking{1}", moreBook, (moreBook==1?"" : "s")),
                    alpha, x, _options.MonthlyDateHeaderHeight + y + ((slotCount) * _options.TimeSlotHeight), w, _options.TimeSlotHeight, upDownPosition);
            }
        }

        public void RenderSchedules(Graphics g, List<Employee> employees, TimeSlot hoveredTimeSlot, TimeSlot selectedTimeSlot, TimeSlot resizedTimeSlot, bool drag, DateTime startTime, int x, int y, int w, int upDownPosition, DateTime hoveredDate, int hoveredEmp)
		{
            int counter = 0;
            DateTime theDateStart = _options.GetMonthlyStartDate(startTime);
            DateTime theDateEnd = _options.GetMonthlyEndDate(startTime);

            int yPos = y;
            int xPos = x;
            int width = ((w) / _options.DayInWeek);
            while (theDateStart.Date <= theDateEnd.Date)
            {
                RenderSchedule(g, employees, hoveredTimeSlot, selectedTimeSlot, resizedTimeSlot, drag
                    , theDateStart,startTime, xPos, yPos, width, _options.MonthlyScheduleHeight, upDownPosition, hoveredDate);
                if (counter == _options.DayInWeek-1)
                {
                    counter = 0;
                    xPos = x;
                    yPos += _options.MonthlyScheduleHeight;
                }
                else
                {
                    counter++;
                    xPos += width;
                }
                theDateStart = theDateStart.AddDays(1);
            }
		}

		public void RenderEmployees(Graphics g, List<Employee> employees, int x, int y)
		{
			foreach (var employee in employees)
			{
                var rect = new Rectangle(x, y, _options.ScheduleColumnWidth, _options.HeaderHeight);
				RenderEmployee(g, employee, rect.X, rect.Y, rect.Width, rect.Height);

                y += _options.HeaderHeight;
			}
		}
    }
}
