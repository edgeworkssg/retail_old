using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using AppointmentBook.Model;

namespace AppointmentBook
{
	//public delegate void TimeSlotEventHandler(TimeSlot timeSlot);

	public partial class AppointmentBookResourceControl : UserControl
	{
		private AppointmentBookControlOptions _options = new AppointmentBookControlOptions();
		private readonly AppointmentBookResourceRenderer _renderer;

		private AppointmentBookData _data = new AppointmentBookData();
		private DateTime _day = DateTime.Now.Date;

		private static readonly SolidBrush _brush = new SolidBrush(Color.Empty);

		private int _selectedResourceIndex = -1;
	
		private TimeSlot _hoveredTimeSlot;
		private TimeSlot _selectedTimeSlot;
		private TimeSlot _draggedTimeSlot;
		private TimeSlot _dragTargetTimeSlot;
		private TimeSlot _resizedTimeSlot;
        
        private Point _dragStartPos;
		private Point _draggedTimeSlotStartPos;
		private TimeSpan _selectedTime;
		private TimeSpan _resizedTimeSlotDuration;

		public TimeSlotEventHandler TimeSlotHovered;
		public TimeSlotEventHandler TimeSlotSelected;
		public TimeSlotEventHandler TimeSlotDoubleClicked;
		public TimeSlotEventHandler TimeSlotDragged;
		public TimeSlotEventHandler TimeSlotResized;

		public AppointmentBookData Data
		{
			get { return _data; }
			set
			{
				_data = value;
				SetScroll();
				Invalidate();
			}
		}

		public AppointmentBookControlOptions Options
		{
			get { return _options; }
			set
			{
				_options = value;
				_renderer.Options = _options;
				SetScroll();
				Invalidate();
			}
		}

		public TimeSlot HoveredTimeSlot
		{
			get { return _hoveredTimeSlot; }
		}

		public TimeSlot SelectedTimeSlot
		{
			get { return _selectedTimeSlot; }
		}

		public DateTime Day
		{
			get { return _day; }
			set
			{
				_day = value;
				
				_selectedTimeSlot = null;
				_dragTargetTimeSlot = null;
				_draggedTimeSlot = null;
				_selectedResourceIndex = -1;
				_selectedTime = _options.WorkDayStart;

				SetScroll();
				Invalidate();
			}
		}

		public int SelectedResourceIndex
		{
            get { return _selectedResourceIndex; }
		}

		public TimeSpan SelectedTime
		{
			get { return _selectedTime; }
		}

        public int TotalCapacity 
        {
            get 
            {
                int objReturn = 0;

                foreach (var resource in _data.Resources)
                    objReturn += resource.Capacity;

                return objReturn;
            }
        }

		public AppointmentBookResourceControl()
		{
			SetStyle(ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
			
			InitializeComponent();

			_renderer = new AppointmentBookResourceRenderer(_options);
			SetScroll();
		}

		protected override void OnPaintBackground(PaintEventArgs e)
		{}

		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
			e.Graphics.TextRenderingHint = TextRenderingHint.SystemDefault;
			e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

			e.Graphics.Clear(BackColor);

			var startTime = _day + _options.WorkDayStart;

            _renderer.RenderSchedules(e.Graphics, _data.Resources, _hoveredTimeSlot, _selectedTimeSlot, _resizedTimeSlot, _draggedTimeSlot != null, startTime, _options.TimeScaleWidth - hScrollBar.Value,
                                      _options.HeaderHeight - vScrollBar.Value);


            _renderer.RenderResources(e.Graphics, _data.Resources, _options.TimeScaleWidth - hScrollBar.Value, 0);


            //render background if not draw resource top on it
            var ew = _options.TimeScaleWidth + TotalCapacity * _options.ScheduleColumnWidth;
            if (Width > ew)
                _renderer.RenderResourcesBackground(e.Graphics, ew, 0, Width - ew, _options.HeaderHeight);

            //for drawing the time scale on left side
            _renderer.RenderTimeScale(e.Graphics, startTime, 0, _options.HeaderHeight - vScrollBar.Value, _options.TimeScaleWidth,
                                      (int)((_options.WorkDayEnd - _options.WorkDayStart).TotalHours * _options.PixelsPerHour));

            //for drawing the time scale on top left side
            _renderer.RenderTimeScaleHeader(e.Graphics, startTime, 0, 0, _options.TimeScaleWidth, _options.HeaderHeight);

            if (_dragTargetTimeSlot != null)
                _renderer.RenderTimeSlot(e.Graphics, _dragTargetTimeSlot, false, false, false, 80, _dragTargetTimeSlot.Region.X,
                                         _dragTargetTimeSlot.Region.Y, _dragTargetTimeSlot.Region.Width, _dragTargetTimeSlot.Region.Height);

            if (_draggedTimeSlot != null)
                _renderer.RenderTimeSlot(e.Graphics, _draggedTimeSlot, false, false, false, 255, _draggedTimeSlot.Region.X,
                                         _draggedTimeSlot.Region.Y, _draggedTimeSlot.Region.Width, _draggedTimeSlot.Region.Height);

            if (vScrollBar.Visible && hScrollBar.Visible)
            {
                _brush.Color = SystemColors.Control;
                e.Graphics.FillRectangle(_brush, vScrollBar.Left - 1, hScrollBar.Top - 1, vScrollBar.Width + 1, hScrollBar.Height + 1);
            }

		}

		#region Scrolling

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			SetScroll();
		}

		private void SetScroll()
		{
			if (_data == null || _options == null)
				return;

            var w = TotalCapacity * _options.ScheduleColumnWidth;
			var h = (int) ((_options.WorkDayEnd - _options.WorkDayStart).TotalHours * _options.PixelsPerHour);

			if (w <= 20 || h <= 20)
			{
				vScrollBar.Visible = hScrollBar.Visible = false;
				vScrollBar.Value = hScrollBar.Value = 0;
			}
			else
			{
				bool vScroll = h > Height - _options.HeaderHeight;
				bool hScroll = w > Width - _options.TimeScaleWidth;

				if (hScroll)
					vScroll = h > Height - _options.HeaderHeight - hScrollBar.Height;

				if (vScroll)
					hScroll = w > Width - _options.TimeScaleWidth - vScrollBar.Width;

				vScrollBar.Visible = vScroll;
				hScrollBar.Visible = hScroll;

				if (vScroll)
				{
					vScrollBar.SetBounds(Width - vScrollBar.Width, _options.HeaderHeight,
					                     SystemInformation.VerticalScrollBarWidth, Height - _options.HeaderHeight - (hScroll ? hScrollBar.Height : 0));

					var maxH = h + (hScroll ? hScrollBar.Height : 0);

					if (vScrollBar.Value > maxH)
						vScrollBar.Value = maxH;

					vScrollBar.Maximum = maxH;
					vScrollBar.LargeChange = Height - _options.HeaderHeight;
					vScrollBar.SmallChange = _options.TimeIntervalHeight;

					if (vScrollBar.Value + vScrollBar.LargeChange > vScrollBar.Maximum)
						vScrollBar.Value = vScrollBar.Maximum - vScrollBar.LargeChange;
				}
				else vScrollBar.Value = 0;

				if (hScroll)
				{
					hScrollBar.SetBounds(_options.TimeScaleWidth, Height - hScrollBar.Height,
					                     Width - _options.TimeScaleWidth - (vScroll ? vScrollBar.Width : 0), SystemInformation.HorizontalScrollBarHeight);

					var maxW = w + (vScroll ? vScrollBar.Width : 0);

					if (hScrollBar.Value > maxW)
						hScrollBar.Value = maxW;

					hScrollBar.Maximum = maxW;
					hScrollBar.LargeChange = Width - _options.TimeScaleWidth;
					hScrollBar.SmallChange = _options.ScheduleColumnWidth;

					if (hScrollBar.Value + hScrollBar.LargeChange > hScrollBar.Maximum)
						hScrollBar.Value = hScrollBar.Maximum - hScrollBar.LargeChange;
				}
				else hScrollBar.Value = 0;
			}
		}

		private void vScrollBar_Scroll(object sender, ScrollEventArgs e)
		{
			Invalidate(new Rectangle(0, _options.HeaderHeight, Width, Height - _options.HeaderHeight));
		}

		private void hScrollBar_Scroll(object sender, ScrollEventArgs e)
		{
			Invalidate(new Rectangle(_options.TimeScaleWidth, 0, Width - _options.TimeScaleWidth, Height));
		}

		protected override void OnMouseWheel(MouseEventArgs e)
		{
			base.OnMouseWheel(e);
			var v = vScrollBar.Value - e.Delta;
			if (v < 0) v = 0;
			if (v > vScrollBar.Maximum - vScrollBar.LargeChange) v = vScrollBar.Maximum - vScrollBar.LargeChange;
			vScrollBar.Value = v;
			vScrollBar_Scroll(vScrollBar, null);
		}

		#endregion

		public TimeSlot GetTimeSlotAt(int x, int y)
		{
            if (y < _options.HeaderHeight)
                return null;

            int resourceIndex = GetResourceIndex(x, false);
            if (resourceIndex == -1)
                return null;

            var time = GetTimeFromY(y);
            foreach (var timeSlot in _data.Resources[resourceIndex].Schedule)
            {
                if ((timeSlot.StartTime <= time && timeSlot.StartTime + timeSlot.Duration > time) &&(x >= timeSlot.xPosition && x <= timeSlot.xPosition + _options.ScheduleColumnWidth) )
                    return timeSlot;
            }

            return null;
		}

		public bool HandleHovered(TimeSlot timeSlot, int x, int y)
		{
			if (timeSlot == null)
				return false;

			return x > timeSlot.Region.X + timeSlot.Region.Width / 3 && x < timeSlot.Region.X + 2 * timeSlot.Region.Width / 3 &&
			       y >= timeSlot.Region.Bottom - 13 && y < timeSlot.Region.Bottom;
		}

		public int GetResourceIndex(int x, bool IsWithHScrollBar)
		{
            int objReturn = 0;

            if (IsWithHScrollBar)
            {
                if (x < _options.TimeScaleWidth ||
                    x >= _options.TimeScaleWidth + GetTotalCapacity() * _options.ScheduleColumnWidth)
                    return -1;
            }
            else
            {
                if (x < _options.TimeScaleWidth ||
                    x + hScrollBar.Value >= _options.TimeScaleWidth + GetTotalCapacity() * _options.ScheduleColumnWidth)
                    return -1;
            }

            x = (x - _options.TimeScaleWidth + hScrollBar.Value) / _options.ScheduleColumnWidth;

            int kk = 0;
            int jj = 0;
            if (_data.Resources.Count > 1)
            {
                for (int i = 0; i < _data.Resources.Count; i++)
                {
                    jj += _data.Resources[i].Capacity;

                    if (jj > x)
                    {
                        objReturn = i;
                        break;
                    }
                }
            }

			return objReturn;
		}

		private DateTime GetTimeFromY(int y)
		{
			return _day + _options.WorkDayStart + TimeSpan.FromHours((y - _options.HeaderHeight + vScrollBar.Value) / (float) _options.PixelsPerHour);
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			var invalidRegion = new Region();

			if (_resizedTimeSlot != null)
			{
				var newDuration = _resizedTimeSlotDuration + TimeSpan.FromHours((e.Y - _dragStartPos.Y) / (float)_options.PixelsPerHour);
				newDuration = TimeSpan.FromMinutes(Math.Round(newDuration.TotalMinutes / _options.TimeResolution) * _options.TimeResolution);

				if (newDuration.TotalMinutes < _options.TimeResolution)
					newDuration = TimeSpan.FromMinutes(_options.TimeResolution);

				var maxDuration = TimeHelper.GetMaxDurationResource(_resizedTimeSlot.Resource, _options, _resizedTimeSlot.StartTime);

				if (newDuration > maxDuration)
					newDuration = maxDuration;

				invalidRegion.Union(_resizedTimeSlot.Region);
	
				_resizedTimeSlot.Duration = newDuration;

				invalidRegion.Union(_resizedTimeSlot.Region);
			}
			else if (_draggedTimeSlot != null)
			{
				invalidRegion.Union(_draggedTimeSlot.Region);
				_draggedTimeSlot.Region.Location = new Point(e.X - _dragStartPos.X + _draggedTimeSlotStartPos.X,
															 e.Y - _dragStartPos.Y + _draggedTimeSlotStartPos.Y);

				invalidRegion.Union(_draggedTimeSlot.Region);

				if (_dragTargetTimeSlot != null)
					Invalidate(_dragTargetTimeSlot.Region);

				_dragTargetTimeSlot = GetDragTargetTimeSlotAt(e.X, e.Y);

				if (_dragTargetTimeSlot != null)
					invalidRegion.Union(_dragTargetTimeSlot.Region);

				if (e.Y > Height && vScrollBar.Visible)
				{
					var newValue = vScrollBar.Value + 10;
					if (newValue > vScrollBar.Maximum - vScrollBar.LargeChange)
						newValue = vScrollBar.Maximum - vScrollBar.LargeChange;
					vScrollBar.Value = newValue;
				}

				else if (e.Y < _options.HeaderHeight && vScrollBar.Visible)
				{
					var newValue = vScrollBar.Value - 10;
					if (newValue < 0)
						newValue = 0;
					vScrollBar.Value = newValue;
				}

				if (e.X > Width && hScrollBar.Visible)
				{
					var newValue = hScrollBar.Value + 10;
					if (newValue > hScrollBar.Maximum - hScrollBar.LargeChange)
						newValue = hScrollBar.Maximum - hScrollBar.LargeChange;
					hScrollBar.Value = newValue;
				}

				else if (e.X < _options.TimeScaleWidth && hScrollBar.Visible)
				{
					var newValue = hScrollBar.Value - 10;
					if (newValue < 0)
						newValue = 0;
					hScrollBar.Value = newValue;
				}
			}
			else
			{
				var hoveredTimeSlot = GetTimeSlotAt(e.X, e.Y);
				if (hoveredTimeSlot != _hoveredTimeSlot)
				{
					if (_hoveredTimeSlot != null)
						invalidRegion.Union(_hoveredTimeSlot.Region);

					if (hoveredTimeSlot != null)
					{
						toolTip.ToolTipTitle = hoveredTimeSlot.Description;
						toolTip.SetToolTip(this,
						                   string.Format("Start time: {0}\nDuration: {1}", hoveredTimeSlot.StartTime.ToShortTimeString(),
						                                 hoveredTimeSlot.Duration.ToString()));
						invalidRegion.Union(hoveredTimeSlot.Region);
					}

					_hoveredTimeSlot = hoveredTimeSlot;

					if (TimeSlotHovered != null)
						TimeSlotHovered(hoveredTimeSlot);
				}

				if (HandleHovered(_hoveredTimeSlot, e.X, e.Y))
					Cursor = Cursors.SizeNS;
				else
					Cursor = Cursors.Default;
			}

			Invalidate(invalidRegion);
		}

		protected override void OnMouseDoubleClick(MouseEventArgs e)
		{
			Focus();
			if (e.Button == MouseButtons.Left)
			{
                //shortcut to set timeslot with something
                if(_hoveredTimeSlot == null)
                    _hoveredTimeSlot = GetTimeSlotAt(e.X, e.Y);

				if (TimeSlotDoubleClicked != null)
					TimeSlotDoubleClicked(_hoveredTimeSlot);
			}
		}

		protected override void OnMouseEnter(EventArgs e)
		{
			Focus();
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
			{
                _selectedResourceIndex = GetResourceIndex(e.X, false);
				_selectedTime = TimeHelper.Floor(GetTimeFromY(e.Y), _options.TimeResolution) - _day;

				if (!HandleHovered(_hoveredTimeSlot, e.X, e.Y))
				{
					var selectedTimeSlot = GetTimeSlotAt(e.X, e.Y);

					if (selectedTimeSlot != null)
					{
						_draggedTimeSlot = selectedTimeSlot.Clone();
						_dragStartPos = new Point(e.X, e.Y);
						_draggedTimeSlotStartPos = _draggedTimeSlot.Region.Location;
					}

					if (_selectedTimeSlot != null) Invalidate(_selectedTimeSlot.Region);
					if (selectedTimeSlot != null) Invalidate(selectedTimeSlot.Region);

					_selectedTimeSlot = selectedTimeSlot;

					if (TimeSlotSelected != null)
						TimeSlotSelected(selectedTimeSlot);
				}
				else
				{
					_dragStartPos = new Point(e.X, e.Y);
					_resizedTimeSlot = _hoveredTimeSlot;
					_resizedTimeSlotDuration = _resizedTimeSlot.Duration;
				}
			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			var invalidRegion = new Region();

            //_dragTargetTimeSlot = GetDragTargetTimeSlotAt(e.X, e.Y);

			if (_draggedTimeSlot != null)
			{
				invalidRegion.Union(_draggedTimeSlot.Region);
				_draggedTimeSlot = null;

				if (_dragTargetTimeSlot != null)
				{
                    var indexResources = GetResourceIndex(e.X, false);
					var resource = _data.Resources[indexResources];

                    _dragTargetTimeSlot.Resource = resource;
                    resource.Schedule.Add(_dragTargetTimeSlot);
                    resource.Schedule.Sort(TimeSlotComparer);

					invalidRegion.Union(_dragTargetTimeSlot.Region);

					_selectedTimeSlot.Resource.Schedule.Remove(_selectedTimeSlot);
					invalidRegion.Union(_selectedTimeSlot.Region);

					_selectedTimeSlot = _dragTargetTimeSlot;
                    var tmpTimeSlot = _selectedTimeSlot;

					if (TimeSlotDragged != null)
						TimeSlotDragged(_dragTargetTimeSlot);

					_dragTargetTimeSlot = null;

                    _selectedTimeSlot = tmpTimeSlot; 
				}
			}

			if (_resizedTimeSlot != null)
			{
				if (TimeSlotResized != null)
					TimeSlotResized(_resizedTimeSlot);

				_resizedTimeSlot = null;
			}


			Invalidate(invalidRegion);
		}

		private int TimeSlotComparer(TimeSlot timeSlot1, TimeSlot timeSlot2)
		{
			return timeSlot1.StartTime.CompareTo(timeSlot2.StartTime);
		}

		public TimeSlot GetDragTargetTimeSlotAt(int x, int y)
		{
			if (hScrollBar.Visible)
				x += hScrollBar.Value;

			if (x < _options.TimeScaleWidth || x >= _options.TimeScaleWidth + GetTotalCapacity() * _options.ScheduleColumnWidth || y < _options.HeaderHeight)
				return null;

            int resourceIndex = GetResourceIndex(x, true);
            int capacity = 0;
            for (int i = 0; i < resourceIndex; i++)
            {
                capacity += _data.Resources[i].Capacity;
            }
            
			var time = GetTimeFromY(y);

			var timeLimitAbove = _day + _options.WorkDayStart;
			var timeLimitBelow = _day + _options.WorkDayEnd;

            foreach (var exisTingtimeSlot in _data.Resources[resourceIndex].Schedule)
			{
				if (exisTingtimeSlot == _selectedTimeSlot)
					continue;

				var timeSlotStartTime = exisTingtimeSlot.StartTime;
				var timeSlotEndTime = exisTingtimeSlot.StartTime + exisTingtimeSlot.Duration;

                //if (timeSlotStartTime <= time && timeSlotEndTime > time)
                //    return null;

				if (timeSlotStartTime >= time && timeSlotStartTime < timeLimitBelow)
					timeLimitBelow = timeSlotStartTime;
	
				if (timeSlotEndTime < time && timeSlotEndTime > timeLimitAbove)
					timeLimitAbove = timeSlotEndTime;
			}

			if ((timeLimitBelow - timeLimitAbove) < _draggedTimeSlot.Duration)
				return null;

			var result = _draggedTimeSlot.Clone();

			var newTimeSlotTime = TimeHelper.Round(time + TimeSpan.FromHours((_draggedTimeSlotStartPos.Y - _dragStartPos.Y) / (float) _options.PixelsPerHour),
					_options.TimeResolution);
			
			if (newTimeSlotTime < timeLimitAbove)
				newTimeSlotTime = timeLimitAbove;

			if (newTimeSlotTime + _draggedTimeSlot.Duration > timeLimitBelow)
				newTimeSlotTime = timeLimitBelow - _draggedTimeSlot.Duration;

			result.StartTime = newTimeSlotTime;
			//result.Region.X = _options.TimeScaleWidth + resourceIndex * _options.ScheduleColumnWidth - hScrollBar.Value;
            result.Region.X = _options.TimeScaleWidth + (capacity * _options.ScheduleColumnWidth) - hScrollBar.Value;
			result.Region.Y = (int)(_options.HeaderHeight + (newTimeSlotTime - _day - _options.WorkDayStart).TotalHours * _options.PixelsPerHour) - vScrollBar.Value;

			return result;
		}

        public int GetTotalCapacity()
        { 
            int objReturn = 0;
            foreach(var resource in _data.Resources)
            {
                objReturn += resource.Capacity;    
            }


            return objReturn;
        }
	}
}