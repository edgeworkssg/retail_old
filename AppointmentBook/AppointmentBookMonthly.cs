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
    public delegate void TimeSelectedHandler(DateTime date);

    public partial class AppointmentBookMonthly : UserControl
    {
        #region *) Fields

        private AppointmentBookControlOptions _options = new AppointmentBookControlOptions();
        private readonly AppointmentBookMonthlyRenderer _renderer;

        private DateTime _day = DateTime.Now.Date;

        private AppointmentBookData _data = new AppointmentBookData();
        private static readonly SolidBrush _brush = new SolidBrush(Color.Empty);

        private int _selectedEmployeeIndex = -1;
        private int _upDownPosition = 0;
        private int _hoveredEmpIndex = -1;
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
        public TimeSelectedHandler TimeDoubleClicked;
        public TimeSelectedHandler ShowMoreTimeDoubleClicked;

        #endregion

        #region *) Properties

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

        public AppointmentBookMonthly()
        {
            SetStyle(ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
            InitializeComponent();

            _renderer = new AppointmentBookMonthlyRenderer(_options);
        }

        #endregion

        #region *) Method

        public void SetScroll()
        {
            if (_data == null || _options == null)
                return;

            var w = this.Width - (_options.ScheduleColumnWidth + 17);
            var h = (int)((6 * _options.MonthlyScheduleHeight) - _options.MonthlyHeaderHeight);

            if (w <= 20 || h <= 20)
            {
                vScrollBar.Visible = false;
                vScrollBar.Value = 0;
            }
            else
            {
                bool vScroll = h > Height - _options.MonthlyHeaderHeight;
                vScrollBar.Visible = vScroll;

                if (vScroll)
                {
                    vScrollBar.SetBounds(Width - vScrollBar.Width, _options.MonthlyHeaderHeight,
                                         SystemInformation.VerticalScrollBarWidth, Height - _options.MonthlyHeaderHeight);

                    var maxH = h;

                    if (vScrollBar.Value > maxH)
                        vScrollBar.Value = maxH;

                    vScrollBar.Maximum = maxH + _options.MonthlyHeaderHeight;
                    vScrollBar.LargeChange = Height - _options.MonthlyHeaderHeight;
                    vScrollBar.SmallChange = _options.MonthlyScheduleHeight;

                    if (vScrollBar.Value + vScrollBar.LargeChange > vScrollBar.Maximum)
                        vScrollBar.Value = vScrollBar.Maximum - vScrollBar.LargeChange;
                }
                else
                    vScrollBar.Value = 0;
            }
        }

        public int GetIndexOfX(int x)
        {
            int xIndex = -1;
            int scrollW = vScrollBar.Visible ? vScrollBar.Width : 0;
            int width = (this.Width-scrollW) / _options.DayInWeek;
            xIndex = x / width;
            if (xIndex >= _options.DayInWeek)
                xIndex = -1;
            return xIndex;
        }

        public int GetIndexOfY(int y)
        {
            int yIndex = -1;

            yIndex = (y - _options.MonthlyHeaderHeight + vScrollBar.Value) / _options.MonthlyScheduleHeight;
            if (yIndex >= 6)
                yIndex = -1;
            return yIndex;
        }

        public int GetScheduleIndex(int y)
        {
            int index = -1;
            int yIndex = GetIndexOfY(y);
            int diffHeight = y + vScrollBar.Value 
                - (yIndex * _options.MonthlyScheduleHeight)
                - _options.MonthlyDateHeaderHeight
                - _options.MonthlyHeaderHeight;
            if (diffHeight > 0)
                index = diffHeight / _options.TimeSlotHeight;
            return index;
        }

        public DateTime GetTimeFromXY(int x, int y)
        {
            DateTime date = DateTime.Now;
            int xIndex = GetIndexOfX(x);
            int yIndex = GetIndexOfY(y);

            int dayAdd = (yIndex * _options.DayInWeek) + xIndex;
            DateTime theDateStart = _options.GetMonthlyStartDate(_day);
            DateTime theDateEnd = _options.GetMonthlyEndDate(_day);
            date = theDateStart.AddDays(dayAdd);
            return date;
        }

        public TimeSlot GetTimeSlotAt(int x, int y, out int upDownArrowPossition)
        {
            upDownArrowPossition = 0;

            var selectedDate = GetTimeFromXY(x, y);
            var schIndex = GetScheduleIndex(y);

            var allSchedule = new List<TimeSlot>();
            foreach (var emp in _data.Employees)
                allSchedule.AddRange(emp.Schedule);
            var todaySchedule = (from o in allSchedule
                                 where o.StartTime.Date == selectedDate.Date
                                 orderby o.StartTime ascending, o.Employee.Name ascending
                                 select o).ToList();

            if (schIndex < 0)
            {
                return null;
            }
            else if (schIndex >= _options.TimeSlotMax - 1 && (todaySchedule.Count - (_options.TimeSlotMax) > 0))
            {
                upDownArrowPossition = 1;
                return null;
            }
            else
            {
                if (schIndex < todaySchedule.Count)
                    return todaySchedule[schIndex];
                else
                    return null;
            }
        }

        public int TimeSlotComparer(TimeSlot timeSlot1, TimeSlot timeSlot2)
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
            this.Invalidate();
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

            }
            else if (_draggedTimeSlot != null)
            {

            }
            else
            {
                try
                {
                    int upDownPosition = 0;
                    var hoveredTimeSlot = GetTimeSlotAt(e.X, e.Y, out upDownPosition);
                    var hoveredDateTime = GetTimeFromXY(e.X, e.Y);
                    if (hoveredTimeSlot != _hoveredTimeSlot 
                        || upDownPosition != _upDownPosition
                        || hoveredDateTime != _hoveredDateTime)
                    {
                        if (_hoveredTimeSlot != null)
                            invalidRegion.Union(_hoveredTimeSlot.Region);

                        if (hoveredTimeSlot != null)
                        {
                            string outstanding = "";
                            if (hoveredTimeSlot.OutStandingAmount > 0)
                                outstanding = string.Format("\nOutstanding:${0}", hoveredTimeSlot.OutStandingAmount.ToString("N2"));
                            toolTip.SetToolTip(this,
                                               string.Format("{2}\nStart time: {0}\nDuration: {1}{3}",
                                               hoveredTimeSlot.StartTime.ToShortTimeString(),
                                               hoveredTimeSlot.Duration.ToString(),
                                               string.Format("[{0}] {1}", hoveredTimeSlot.Employee.Name, hoveredTimeSlot.CompleteDescription)
                                               ,outstanding));
                            invalidRegion.Union(hoveredTimeSlot.Region);
                        }
                        else
                        {
                            toolTip.Hide(this);
                        }

                        this.Cursor = (upDownPosition != 0 || hoveredTimeSlot != null) ? Cursors.Hand : Cursors.Arrow;
                        _hoveredTimeSlot = hoveredTimeSlot;
                        _upDownPosition = upDownPosition;
                        _hoveredDateTime = hoveredDateTime;
                        this.Invalidate();
                        //if (TimeSlotHovered != null)
                        //    TimeSlotHovered(hoveredTimeSlot);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            Focus();
            if (e.Button == MouseButtons.Left)
            {
                int upDownPosition = 0;
                DateTime selectedDate = GetTimeFromXY(e.X, e.Y);
                var timeSlot = GetTimeSlotAt(e.X, e.Y, out upDownPosition);
                if (upDownPosition != 0 && TimeDoubleClicked!=null)
                {
                    ShowMoreTimeDoubleClicked(selectedDate);
                    return;
                }
                if (timeSlot != null && TimeSlotDoubleClicked != null)
                {
                    TimeSlotDoubleClicked(timeSlot);
                    return;
                }
                if (TimeDoubleClicked != null)
                {
                    TimeDoubleClicked(selectedDate);
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
                //MessageBox.Show(GetScheduleIndex(e.Y).ToString());

                int upDownPosition = 0;
                var timeSlot = GetTimeSlotAt(e.X, e.Y, out upDownPosition);
                var selectedDate = GetTimeFromXY(e.X, e.Y);
                //MessageBox.Show(selectedDate.Date.ToShortDateString());
                _selectedTimeSlot = timeSlot;
                _selectedDate = selectedDate;

                if (upDownPosition != 0 && TimeDoubleClicked != null)
                {
                    ShowMoreTimeDoubleClicked(selectedDate);
                    return;
                }
                else
                {
                    if (TimeSlotSelected != null)
                        TimeSlotSelected(0, timeSlot);
                    this.Invalidate();
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {

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
            var startTime = new DateTime(_day.Year, _day.Month, 1);

            _renderer.RenderSchedules(e.Graphics, _data.Employees, _hoveredTimeSlot, _selectedTimeSlot,
                _resizedTimeSlot, _draggedTimeSlot != null, startTime,
                0, _options.MonthlyHeaderHeight - vScrollBar.Value, this.Width - scrollW,
                _upDownPosition, _hoveredDateTime, _hoveredEmpIndex);
            _renderer.RenderTimeScale(e.Graphics, startTime, 0, 0, this.Width - scrollW, _options.MonthlyHeaderHeight);
        }

        #endregion

        #endregion
    }
}
