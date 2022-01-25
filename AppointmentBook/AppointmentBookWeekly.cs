using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AppointmentBook.Model;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Diagnostics;

namespace AppointmentBook
{
    public delegate void NewTimeSlotEventHandler(int empIndex, DateTime date);

    public delegate void NewTimeSlotSelectedEventHandler(int empIndex, TimeSlot timeSlot);

    public partial class AppointmentBookWeekly : UserControl
    {
        #region *) Fields

        private AppointmentBookControlOptions _options = new AppointmentBookControlOptions();
        private readonly AppointmentBookWeeklyRenderer _renderer;

        private DateTime _day = DateTime.Now.Date;

        private AppointmentBookData _data = new AppointmentBookData();
        private static readonly SolidBrush _brush = new SolidBrush(Color.Empty);

        private int _selectedEmployeeIndex = -1;
        private int _upDownPosition = 0;
        private int _hoveredEmpIndex = -1;
        private int? _selectedTimeIndex = null;
        private DateTime _hoveredDateTime = DateTime.Now;
        private TimeSlot _hoveredTimeSlot;
        private TimeSlot _selectedTimeSlot;
        private TimeSlot _draggedTimeSlot;
        private TimeSlot _dragTargetTimeSlot;
        private TimeSlot _resizedTimeSlot;

        private Point _dragStartPos;
        private Point _draggedTimeSlotStartPos;
        private TimeSpan _selectedTime;
        private TimeSpan _resizedTimeSlotDuration;
        private DateTime _selectedDate = DateTime.Now;

        public TimeSlotEventHandler TimeSlotHovered;
        public NewTimeSlotSelectedEventHandler TimeSlotSelected;
        public TimeSlotEventHandler TimeSlotDoubleClicked;
        public TimeSlotEventHandler TimeSlotDragged;
        public TimeSlotEventHandler TimeSlotResized;
        public NewTimeSlotEventHandler NewTimeSlotDoubleClicked;

        #endregion

        #region *) Properties

        public int? SelectedTimeIndex
        {
            get { return _selectedTimeIndex; }
            set { _selectedTimeIndex = value; }
        }

        public DateTime SelectedDate
        {
            get { return _selectedDate; }
        }

        public int SelectedEmployeeIndex
        {
            get { return _selectedEmployeeIndex; }
        }

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

        public DateTime Day
        {
            get { return _day; }
            set
            {
                int diff = _day.Date.Subtract(value.Date).Days;

                if (_selectedTimeIndex.HasValue)
                    _selectedTimeIndex += diff;

                _day = value;

                _selectedTimeSlot = null;
                _dragTargetTimeSlot = null;
                _draggedTimeSlot = null;
                _selectedEmployeeIndex = -1;
                _selectedTime = _options.WorkDayStart;

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

        public TimeSpan SelectedTime
        {
            get { return _selectedTime; }
        }

        #endregion

        #region *) Constructor

        public AppointmentBookWeekly()
        {
            SetStyle(ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
            InitializeComponent();

            _renderer = new AppointmentBookWeeklyRenderer(_options);
        }

        #endregion

        #region *) Method

        private void SetScroll()
        {
            if (_data == null || _options == null)
                return;

            var w = this.Width - (_options.ScheduleColumnWidth + 17);
            var h = (int)((_data.Employees.Count * _options.HeaderHeight) - _options.TimeHeaderHeight);

            if (w <= 20 || h <= 20)
            {
                vScrollBar.Visible = false;
                vScrollBar.Value = 0;
            }
            else
            {
                bool vScroll = h > Height - _options.HeaderHeight;
                bool hScroll = w > Width - _options.TimeScaleWidth;

                if (vScroll)
                    hScroll = w > Width - _options.TimeScaleWidth - vScrollBar.Width;

                vScrollBar.Visible = vScroll;

                if (vScroll)
                {
                    vScrollBar.SetBounds(Width - vScrollBar.Width, _options.TimeHeaderHeight,
                                         SystemInformation.VerticalScrollBarWidth, Height - _options.TimeHeaderHeight);

                    var maxH = h;

                    if (vScrollBar.Value > maxH)
                        vScrollBar.Value = maxH;

                    vScrollBar.Maximum = maxH;
                    vScrollBar.LargeChange = Height - _options.HeaderHeight;
                    vScrollBar.SmallChange = _options.TimeIntervalHeight;

                    if (vScrollBar.Value + vScrollBar.LargeChange > vScrollBar.Maximum)
                        vScrollBar.Value = vScrollBar.Maximum - vScrollBar.LargeChange;
                }
                else 
                    vScrollBar.Value = 0;
            }
        }

        private DateTime GetTimeFromX(int x)
        {
            //return _day + _options.WorkDayStart + TimeSpan.FromHours((y - _options.HeaderHeight + vScrollBar.Value) / (float)_options.PixelsPerHour);
            int width = ((this.Width - (_options.ScheduleColumnWidth + 17)) / _options.DayInWeek);
            int add = (x - _options.ScheduleColumnWidth) / width;
            DateTime dt = _day.AddDays(add);

            return dt;

        }

        public int GetEmployeeIndex(int y)
        {
            if (y < _options.TimeHeaderHeight)
                return -1;

            return (y - _options.TimeHeaderHeight + vScrollBar.Value) / _options.HeaderHeight;
        }

        public int GetScheduleIndex(int y)
        {
            int employeeIndex = GetEmployeeIndex(y);
            if (employeeIndex < 0)
                return employeeIndex;
            int index = (y - (employeeIndex * _options.HeaderHeight) - _options.TimeHeaderHeight + vScrollBar.Value) / _options.TimeSlotHeight;

            return index;
        }

        public TimeSlot GetTimeSlotAt(int x, int y, out int upDownArrowPossition)
        {
            upDownArrowPossition = 0;
            int employeeIndex = GetEmployeeIndex(y);
            _hoveredEmpIndex = employeeIndex;
            if (employeeIndex < 0 || employeeIndex >= _data.Employees.Count)
                return null;
            int scheduleIndex = GetScheduleIndex(y);
            DateTime selectedDate = GetTimeFromX(x);
            _hoveredDateTime = selectedDate;
            var schedules = _data.Employees[employeeIndex].Schedule.Where(o => o.StartTime.Date == selectedDate.Date).ToList();
            if (scheduleIndex >= _options.TimeSlotMax
                && schedules.Count > _options.TimeSlotMax)
                upDownArrowPossition = GetUpDownArrowPosition(x);
            if (scheduleIndex < 0 
                || scheduleIndex >= _options.TimeSlotMax 
                || scheduleIndex >= schedules.Count)
                return null;

            try
            {
                return schedules[scheduleIndex + (_data.Employees[employeeIndex].TimeSlotScroll[selectedDate.Date]*-1)];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return schedules[scheduleIndex];
            }
        }

        public int GetUpDownArrowPosition(int x)
        {
            int upDownArrowPossition = 0;
            int width = ((this.Width - (_options.ScheduleColumnWidth + 17)) / _options.DayInWeek);
            int add = (x - _options.ScheduleColumnWidth) / width;
            int diff = x - ((add * width) + _options.ScheduleColumnWidth);

            upDownArrowPossition = diff < width / 2 ? 1 : -1;

            return upDownArrowPossition;
        }

        public TimeSlot GetDragTargetTimeSlotAt(int x, int y)
        {
            //if (hScrollBar.Visible)
            //    x += hScrollBar.Value;

            if (x < _options.TimeScaleWidth || x >= _options.TimeScaleWidth + _data.Employees.Count * _options.ScheduleColumnWidth || y < _options.HeaderHeight)
                return null;

            int employeeIndex = (x - _options.TimeScaleWidth) / _options.ScheduleColumnWidth;
            var time = GetTimeFromX(y);

            var timeLimitAbove = _day + _options.WorkDayStart;
            var timeLimitBelow = _day + _options.WorkDayEnd;

            foreach (var exisTingtimeSlot in _data.Employees[employeeIndex].Schedule)
            {
                if (exisTingtimeSlot == _selectedTimeSlot)
                    continue;

                var timeSlotStartTime = exisTingtimeSlot.StartTime;
                var timeSlotEndTime = exisTingtimeSlot.StartTime + exisTingtimeSlot.Duration;

                if (timeSlotStartTime <= time && timeSlotEndTime > time)
                    return null;

                if (timeSlotStartTime >= time && timeSlotStartTime < timeLimitBelow)
                    timeLimitBelow = timeSlotStartTime;

                if (timeSlotEndTime < time && timeSlotEndTime > timeLimitAbove)
                    timeLimitAbove = timeSlotEndTime;
            }

            if ((timeLimitBelow - timeLimitAbove) < _draggedTimeSlot.Duration)
                return null;

            var result = _draggedTimeSlot.Clone();

            var newTimeSlotTime = TimeHelper.Round(time + TimeSpan.FromHours((_draggedTimeSlotStartPos.Y - _dragStartPos.Y) / (float)_options.PixelsPerHour),
                    _options.TimeResolution);

            if (newTimeSlotTime < timeLimitAbove)
                newTimeSlotTime = timeLimitAbove;

            if (newTimeSlotTime + _draggedTimeSlot.Duration > timeLimitBelow)
                newTimeSlotTime = timeLimitBelow - _draggedTimeSlot.Duration;

            result.StartTime = newTimeSlotTime;
            result.Region.X = _options.TimeScaleWidth + employeeIndex * _options.ScheduleColumnWidth;
            result.Region.Y = (int)(_options.HeaderHeight + (newTimeSlotTime - _day - _options.WorkDayStart).TotalHours * _options.PixelsPerHour) - vScrollBar.Value;

            return result;
        }

        public bool HandleHovered(TimeSlot timeSlot, int x, int y)
        {
            if (timeSlot == null)
                return false;

            return x > timeSlot.Region.X + timeSlot.Region.Width / 3 && x < timeSlot.Region.X + 2 * timeSlot.Region.Width / 3 &&
                   y >= timeSlot.Region.Bottom - 13 && y < timeSlot.Region.Bottom;
        }

        private int TimeSlotComparer(TimeSlot timeSlot1, TimeSlot timeSlot2)
        {
            return timeSlot1.StartTime.CompareTo(timeSlot2.StartTime);
        }

        #endregion

        #region *) Event Handler

        #region *) Scroll

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            SetScroll();
        }

        private void vScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            Invalidate(new Rectangle(0, _options.TimeHeaderHeight, Width, Height - _options.TimeHeaderHeight));
            //this.Invalidate();
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            if (vScrollBar.Visible)
            {
                var v = vScrollBar.Value - e.Delta;
                if (v < 0) v = 0;
                if (v > vScrollBar.Maximum - vScrollBar.LargeChange) v = vScrollBar.Maximum - vScrollBar.LargeChange;
                vScrollBar.Value = v;
                vScrollBar_Scroll(vScrollBar, null);
            }
        }

        #endregion

        #region *) Mouse

        protected override void OnMouseMove(MouseEventArgs e)
        {
            var invalidRegion = new Region();

            if (_resizedTimeSlot != null)
            {
                //var newDuration = _resizedTimeSlotDuration + TimeSpan.FromHours((e.Y - _dragStartPos.Y) / (float)_options.PixelsPerHour);
                //newDuration = TimeSpan.FromMinutes(Math.Round(newDuration.TotalMinutes / _options.TimeResolution) * _options.TimeResolution);

                //if (newDuration.TotalMinutes < _options.TimeResolution)
                //    newDuration = TimeSpan.FromMinutes(_options.TimeResolution);

                //var maxDuration = TimeHelper.GetMaxDuration(_resizedTimeSlot.Employee, _options, _resizedTimeSlot.StartTime);

                //if (newDuration > maxDuration)
                //    newDuration = maxDuration;

                //invalidRegion.Union(_resizedTimeSlot.Region);

                //_resizedTimeSlot.Duration = newDuration;

                //invalidRegion.Union(_resizedTimeSlot.Region);
            }
            else if (_draggedTimeSlot != null)
            {
                //invalidRegion.Union(_draggedTimeSlot.Region);
                //_draggedTimeSlot.Region.Location = new Point(e.X - _dragStartPos.X + _draggedTimeSlotStartPos.X,
                //                                             e.Y - _dragStartPos.Y + _draggedTimeSlotStartPos.Y);

                //invalidRegion.Union(_draggedTimeSlot.Region);

                //if (_dragTargetTimeSlot != null)
                //    Invalidate(_dragTargetTimeSlot.Region);

                //_dragTargetTimeSlot = GetDragTargetTimeSlotAt(e.X, e.Y);

                //if (_dragTargetTimeSlot != null)
                //    invalidRegion.Union(_dragTargetTimeSlot.Region);

                //if (e.Y > Height && vScrollBar.Visible)
                //{
                //    var newValue = vScrollBar.Value + 10;
                //    if (newValue > vScrollBar.Maximum - vScrollBar.LargeChange)
                //        newValue = vScrollBar.Maximum - vScrollBar.LargeChange;
                //    vScrollBar.Value = newValue;
                //}

                //else if (e.Y < _options.HeaderHeight && vScrollBar.Visible)
                //{
                //    var newValue = vScrollBar.Value - 10;
                //    if (newValue < 0)
                //        newValue = 0;
                //    vScrollBar.Value = newValue;
                //}

                ////if (e.X > Width && hScrollBar.Visible)
                ////{
                ////    var newValue = hScrollBar.Value + 10;
                ////    if (newValue > hScrollBar.Maximum - hScrollBar.LargeChange)
                ////        newValue = hScrollBar.Maximum - hScrollBar.LargeChange;
                ////    hScrollBar.Value = newValue;
                ////}

                //else if (e.X < _options.TimeScaleWidth && hScrollBar.Visible)
                //{
                //    var newValue = hScrollBar.Value - 10;
                //    if (newValue < 0)
                //        newValue = 0;
                //    hScrollBar.Value = newValue;
                //}
            }
            else
            {
                try
                {
                    int upDownPosition = 0;
                    var hoveredTimeSlot = GetTimeSlotAt(e.X, e.Y, out upDownPosition);
                    if (hoveredTimeSlot != _hoveredTimeSlot || upDownPosition != _upDownPosition)
                    {
                        if (_hoveredTimeSlot != null)
                            invalidRegion.Union(_hoveredTimeSlot.Region);

                        if (hoveredTimeSlot != null)
                        {
                            //toolTip.ToolTipTitle = hoveredTimeSlot.Description;
                            
                            string outstanding = "";
                            if (hoveredTimeSlot.OutStandingAmount > 0)
                                outstanding = string.Format("\nOutstanding:${0}", hoveredTimeSlot.OutStandingAmount.ToString("N2"));
                            toolTip.SetToolTip(this,
                                               string.Format("{2}\nStart time: {0}\nDuration: {1}{3}",
                                               hoveredTimeSlot.StartTime.ToShortTimeString(),
                                               hoveredTimeSlot.Duration.ToString(),
                                               hoveredTimeSlot.CompleteDescription,
                                               outstanding));
                            invalidRegion.Union(hoveredTimeSlot.Region);
                        }
                        else
                        {
                            toolTip.Hide(this);
                        }

                        this.Cursor = (upDownPosition != 0 || hoveredTimeSlot != null) ? Cursors.Hand : Cursors.Arrow;
                        _hoveredTimeSlot = hoveredTimeSlot;
                        _upDownPosition = upDownPosition;
                        Invalidate(invalidRegion);
                        //Invalidate(new Rectangle(0, _options.TimeHeaderHeight, Width, Height - _options.TimeHeaderHeight));
                        //Invalidate();
                        //if (TimeSlotHovered != null)
                        //    TimeSlotHovered(hoveredTimeSlot);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                //if (HandleHovered(_hoveredTimeSlot, e.X, e.Y))
                //    Cursor = Cursors.SizeNS;
                //else
                //    Cursor = Cursors.Default;
            }

            //Invalidate(invalidRegion);
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            Focus();
            if (e.Button == MouseButtons.Left)
            {
                int upDownPosition = 0;
                int empIndex = GetEmployeeIndex(e.Y);
                DateTime selectedDate = GetTimeFromX(e.X);
                var timeSlot = GetTimeSlotAt(e.X, e.Y, out upDownPosition);
                if (upDownPosition != 0)
                {
                    //if (empIndex >= 0 && empIndex < _data.Employees.Count)
                    //{
                    //    var appOnDate = _data.Employees[empIndex].Schedule.Where(o => o.StartTime.Date == selectedDate.Date).OrderByDescending(o => o.StartTime).ToList();
                    //    if (appOnDate.Count > _options.TimeSlotMax)
                    //    {
                    //        if ((appOnDate.Count + upDownPosition
                    //            + _data.Employees[empIndex].TimeSlotScroll[selectedDate.Date]
                    //            >= _options.TimeSlotMax)
                    //            && _data.Employees[empIndex].TimeSlotScroll[selectedDate.Date] + upDownPosition <= 0)
                    //        {
                    //            _data.Employees[empIndex].TimeSlotScroll[selectedDate.Date] += upDownPosition;
                    //        }
                    //        MessageBox.Show(_data.Employees[empIndex].TimeSlotScroll[selectedDate.Date].ToString());
                    //    }
                    //}
                    return;
                }
                if (timeSlot != null && TimeSlotDoubleClicked != null)
                {
                    TimeSlotDoubleClicked(_hoveredTimeSlot);
                    return;
                }
                if (timeSlot == null && NewTimeSlotDoubleClicked != null)
                {
                    if (empIndex >= 0 && empIndex < _data.Employees.Count)
                    {
                        var theLastAppoinment = _data.Employees[empIndex].Schedule.Where(o => o.StartTime.Date == selectedDate.Date).OrderByDescending(o => o.StartTime).ToList().FirstOrDefault();
                        if (theLastAppoinment != null)
                            selectedDate = theLastAppoinment.StartTime.Add(theLastAppoinment.Duration);
                        else
                            selectedDate = selectedDate.Date.AddHours(8);
                        NewTimeSlotDoubleClicked(empIndex, selectedDate);
                    }
                    return;
                }
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            Focus();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            Focus();
            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
            {
                int upDownPosition = 0;
                int empIndex = GetEmployeeIndex(e.Y);
                DateTime selectedDate = GetTimeFromX(e.X);
                var timeSlot = GetTimeSlotAt(e.X, e.Y, out upDownPosition);
                if (empIndex >= 0 && empIndex < _data.Employees.Count)
                {
                    var theLastAppoinment = _data.Employees[empIndex].Schedule.Where(o => o.StartTime.Date == selectedDate.Date).OrderByDescending(o => o.StartTime).ToList().FirstOrDefault();
                    if (theLastAppoinment != null)
                        selectedDate = theLastAppoinment.StartTime.Add(theLastAppoinment.Duration);
                    else
                        selectedDate = selectedDate.Date.AddHours(8);
                }
                _selectedTimeSlot = timeSlot;
                _selectedEmployeeIndex = empIndex;
                _selectedDate = selectedDate;


                if (upDownPosition != 0 && e.Button == MouseButtons.Left)
                {
                    if (empIndex >= 0 && empIndex < _data.Employees.Count)
                    {
                        var appOnDate = _data.Employees[empIndex].Schedule.Where(o => o.StartTime.Date == selectedDate.Date).OrderByDescending(o => o.StartTime).ToList();
                        if (appOnDate.Count > _options.TimeSlotMax)
                        {
                            if ((appOnDate.Count + upDownPosition
                                + _data.Employees[empIndex].TimeSlotScroll[selectedDate.Date]
                                >= _options.TimeSlotMax)
                                && _data.Employees[empIndex].TimeSlotScroll[selectedDate.Date] + upDownPosition <= 0)
                            {
                                _data.Employees[empIndex].TimeSlotScroll[selectedDate.Date] += upDownPosition;
                                this.Invalidate();
                                return;
                            }
                        }
                    }
                }
                this.Invalidate();
                //MessageBox.Show(empIndex.ToString());
                if (TimeSlotSelected != null)
                    TimeSlotSelected(empIndex, timeSlot);
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            /*
            var invalidRegion = new Region();

            if (_draggedTimeSlot != null)
            {
                invalidRegion.Union(_draggedTimeSlot.Region);
                _draggedTimeSlot = null;

                if (_dragTargetTimeSlot != null)
                {
                    var employee = _data.Employees[(_dragTargetTimeSlot.Region.X + - _options.TimeScaleWidth) /
                                        _options.ScheduleColumnWidth];

                    _dragTargetTimeSlot.Employee = employee;
                    employee.Schedule.Add(_dragTargetTimeSlot);
                    employee.Schedule.Sort(TimeSlotComparer);

                    invalidRegion.Union(_dragTargetTimeSlot.Region);

                    _selectedTimeSlot.Employee.Schedule.Remove(_selectedTimeSlot);
                    invalidRegion.Union(_selectedTimeSlot.Region);
                    _selectedTimeSlot = _dragTargetTimeSlot;

                    if (TimeSlotDragged != null)
                        TimeSlotDragged(_dragTargetTimeSlot);

                    _dragTargetTimeSlot = null;
                }
            }

            if (_resizedTimeSlot != null)
            {
                if (TimeSlotResized != null)
                    TimeSlotResized(_resizedTimeSlot);

                _resizedTimeSlot = null;
            }


            Invalidate(invalidRegion);
             */
        }

        #endregion

        #region *) Paint

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.TextRenderingHint = TextRenderingHint.SystemDefault;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

            e.Graphics.Clear(BackColor);
            int scrollW = vScrollBar.Visible ? vScrollBar.Width : 0;
            var startTime = _day;

            _renderer.RenderEmployees(e.Graphics, _data.Employees, 0, _options.TimeHeaderHeight - vScrollBar.Value);

            _renderer.RenderSchedules(e.Graphics, _data.Employees, _hoveredTimeSlot, _selectedTimeSlot,
                _resizedTimeSlot, _draggedTimeSlot != null, startTime,
                _options.ScheduleColumnWidth, _options.TimeHeaderHeight - vScrollBar.Value,
                this.Width - (_options.ScheduleColumnWidth + scrollW), _upDownPosition, _hoveredDateTime, _hoveredEmpIndex, _selectedTimeIndex.GetValueOrDefault(-1));

            _renderer.RenderTimeScaleHeader(e.Graphics, startTime, 0, 0,
                _options.ScheduleColumnWidth, _options.TimeHeaderHeight);

            _renderer.RenderTimeScale(e.Graphics, startTime, _options.ScheduleColumnWidth, 0,
                this.Width - (_options.ScheduleColumnWidth + scrollW), _options.TimeHeaderHeight);
        }

        #endregion

        #endregion
    }
}
