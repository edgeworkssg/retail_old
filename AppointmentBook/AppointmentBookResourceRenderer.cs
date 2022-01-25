using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using AppointmentBook.Model;
using AppointmentBook.Properties;


namespace AppointmentBook
{
	public class AppointmentBookResourceRenderer
	{
		private AppointmentBookControlOptions _options;

		private static readonly Pen _pen = new Pen(Color.Empty, 1);
		private static readonly SolidBrush _brush = new SolidBrush(Color.Empty);
		private static readonly StringFormat _stringFormat = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center, Trimming = StringTrimming.EllipsisCharacter };

        public AppointmentBookResourceRenderer(AppointmentBookControlOptions options)
		{
			_options = options;
		}

		public AppointmentBookControlOptions Options
		{
			set { _options = value; }
		}

		public void RenderTimeSlot(Graphics g, TimeSlot timeSlot, bool hovered, bool selected, bool resized, byte alpha, int x, int y, int w, int h)
		{
            //if (!_options.IsAppointmentView)
            //    w = timeSlot.Resource.Capacity * w;

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

            if (timeSlot.TimeExtension > 0)
            {
                if (w < Resources.plus.Width || h < Resources.plus.Height)
                    g.DrawImage(Resources.plus_small, x + w - Resources.plus_small.Width, y + Resources.Check_small.Height + 5);
				else
                    g.DrawImage(Resources.plus, x + w - Resources.plus.Width, y + Resources.Check.Height + 5);

			}

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

		public void RenderTimeScaleElement(Graphics g, DateTime time, int x, int y, int w, int h)
		{
			var rect = new RectangleF(x, y, w, h);
			_brush.Color = Color.Gray;

			g.FillRectangle(_brush, rect);

			_stringFormat.Alignment = StringAlignment.Center;
			_stringFormat.LineAlignment = StringAlignment.Center;

			if (time.Minute == 0)
			{
				_brush.Color = Color.White;
				g.DrawString(time.ToString("h:mm tt", new CultureInfo("en-US")), _options.TimeMajorFont, _brush, rect, _stringFormat);
			}
			else
			{
				_brush.Color = Color.White;
				g.DrawString(time.ToString("h:mm"), _options.TimeMinorFont, _brush, rect, _stringFormat);
			}
		}

		public void RenderTimeScale(Graphics g, DateTime startTime, int x, int y, int w, int h)
		{
			startTime = TimeHelper.Floor(startTime, _options.TimeResolution);

			var cy = y;
	
			_pen.Color = Color.Silver;
			_pen.Width = 1;

			while (cy < y + h)
			{
				RenderTimeScaleElement(g, startTime, x, cy, w, _options.TimeIntervalHeight);

				cy += _options.TimeIntervalHeight;
				g.DrawLine(_pen, x, cy, x + w, cy);
				startTime += TimeSpan.FromMinutes(_options.TimeResolution);
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

		
        public void RenderResourcesBackground(Graphics g, int x, int y, int w, int h)
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

		public static void EnframeImage(Graphics g, Image image, RectangleF rect)
		{
			var sw = rect.Width / image.Width;
			var sh = rect.Height / image.Height;
			var s = Math.Min(sw, sh);
			g.DrawImage(image, rect.X + (rect.Width - image.Width * s) / 2, rect.Y + (rect.Height - image.Height * s) / 2,
			            image.Width * s, image.Height * s);
		}

		public void RenderSchedule(Graphics g, int Capacity, List<TimeSlot> schedule, TimeSlot hoveredTimeSlot, TimeSlot selectedTimeSlot, TimeSlot resizedTimeSlot, bool drag, DateTime startTime, int x, int y, int w, int h)
		{
			startTime = TimeHelper.Floor(startTime, _options.TimeResolution);
            schedule.Sort(delegate(TimeSlot ps1, TimeSlot ps2) {
                    int xdiff = DateTime.Compare(ps1.StartTime, ps2.StartTime);
                    if (xdiff == 0) return ps1.Id.CompareTo(ps2.Id);
                    else return xdiff;
                });
			var currentTime = startTime;
			var cy = y;
            var cx = x;

			_pen.Color = Color.WhiteSmoke;
			_pen.Width = 1;

            for (int i = 0; i < Capacity; i++)
            {
                while (cy < y + h)
                {
                    var rect = new RectangleF(cx, cy, w, _options.TimeIntervalHeight);
                    _brush.Color = Color.LightGray;
                    g.FillRectangle(_brush, rect);

                    currentTime += TimeSpan.FromMinutes(_options.TimeResolution);
                    cy += _options.TimeIntervalHeight;
                    g.DrawLine(_pen, cx, cy, cx + w, cy);
                }
                cy = y;
                cx += w;
            }

            TimeSlot lastTimeSlot = new TimeSlot();

            for(int i = 0; i< schedule.Count; i++)
            {
                var timeSlot = schedule[i];

                byte alpha = 255;
                if (timeSlot == selectedTimeSlot && drag)
                    alpha = 80;

                if (i == 0)
                {
                   if (timeSlot.StartTime + timeSlot.Duration > startTime && timeSlot.StartTime.Date <= currentTime)
                        RenderTimeSlot(g, timeSlot, timeSlot == hoveredTimeSlot, timeSlot == selectedTimeSlot, timeSlot == resizedTimeSlot, alpha, x, y + (int)((timeSlot.StartTime - startTime).TotalHours * _options.PixelsPerHour), w,
                                       (int)(timeSlot.Duration.TotalHours * _options.PixelsPerHour));
                   timeSlot.xPosition = x;
                   timeSlot.yPosition = y + (int)((timeSlot.StartTime - startTime).TotalHours * _options.PixelsPerHour);
                   lastTimeSlot = timeSlot;
                }
                else
                {
                    var wx = x;
                    if (TimeHelper.IsTimeSlotConflicted(timeSlot, lastTimeSlot))
                    {
                        wx = lastTimeSlot.xPosition + w;
                        if (timeSlot.StartTime + timeSlot.Duration > startTime && timeSlot.StartTime.Date <= currentTime)
                            RenderTimeSlot(g, timeSlot, timeSlot == hoveredTimeSlot, timeSlot == selectedTimeSlot, timeSlot == resizedTimeSlot, alpha, wx, y + (int)((timeSlot.StartTime - startTime).TotalHours * _options.PixelsPerHour), w,
                                           (int)(timeSlot.Duration.TotalHours * _options.PixelsPerHour));
                    }
                    else
                    {
                        if (timeSlot.StartTime + timeSlot.Duration > startTime && timeSlot.StartTime.Date <= currentTime)
                            RenderTimeSlot(g, timeSlot, timeSlot == hoveredTimeSlot, timeSlot == selectedTimeSlot, timeSlot == resizedTimeSlot, alpha, x, y + (int)((timeSlot.StartTime - startTime).TotalHours * _options.PixelsPerHour), w,
                                           (int)(timeSlot.Duration.TotalHours * _options.PixelsPerHour));
                    }
                    timeSlot.xPosition = wx;
                    timeSlot.yPosition = y + (int)((timeSlot.StartTime - startTime).TotalHours * _options.PixelsPerHour);
                    lastTimeSlot = timeSlot;
                }
            }
		}


        public void RenderSchedules(Graphics g, List<AppointmentResource> resources, TimeSlot hoveredTimeSlot, TimeSlot selectedTimeSlot, TimeSlot resizedTimeSlot, bool drag, DateTime startTime, int x, int y)
		{
           
            foreach(var resource in resources)
            {
                if (resource.ResourceID != "ROOM_DEFAULT")
                {
                    var rect = new Rectangle(x, y,
                                             _options.ScheduleColumnWidth,
                                             (int)((_options.WorkDayEnd - _options.WorkDayStart).TotalHours * _options.PixelsPerHour));

                    RenderSchedule(g, resource.Capacity, resource.Schedule, hoveredTimeSlot, selectedTimeSlot, resizedTimeSlot, drag, startTime, rect.X, rect.Y, rect.Width, rect.Height);

                    _pen.Color = Color.WhiteSmoke;
                    _pen.Width = 1;
                    g.DrawLine(_pen, rect.Right, rect.Top, rect.Right, rect.Bottom);

                    x += resource.Capacity * _options.ScheduleColumnWidth;
                }
            }
            
		}

        public void RenderResources(Graphics g, List<AppointmentResource> resources, int x, int y)
        {
            foreach (var resource in resources)
            {
                var width = resource.Capacity * _options.ScheduleColumnWidth;

                var rect = new Rectangle(x, y, width, _options.HeaderHeight);
                RenderResources(g, resource, rect.X, rect.Y, rect.Width, rect.Height);

                x += width;
            }

        }

        public void RenderResources(Graphics g, AppointmentResource resource, int x, int y, int w, int h)
        {
            RenderResourcesBackground(g, x, y, w, h);

            var textRect = new RectangleF(x + w * 0.05f, y, w * 0.9f, h);
            _brush.Color = Color.White;
            _stringFormat.Alignment = StringAlignment.Center;
            _stringFormat.LineAlignment = StringAlignment.Center;
            g.DrawString(resource.Name, _options.TimeMajorFont, _brush, textRect, _stringFormat);
        }

        public void RenderResourceBackground(Graphics g, int x, int y, int w, int h)
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
	}
}