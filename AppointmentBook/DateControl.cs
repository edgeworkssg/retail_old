using System;
using System.Windows.Forms;

namespace AppointmentBook
{
	public partial class DateControl  : UserControl
	{
		private DateTime _day = DateTime.Now.Date;
		private string _format = "dddd";

		public event EventHandler Changed;

		public DateTime Day
		{
			get { return _day; }
			set { SetDay(value); }
		}

		public string Format
		{
			get { return _format; }
			set
			{
				_format = value;
				SetDay(_day);
			}
		}

        public DateControl()
		{
			InitializeComponent();
			OnResize(null);
		}

		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged(e);
			OnResize(null);
		}

		private void SetDay(DateTime day)
		{
			_day = day.Date;
			lblDate.Text = _day.ToString(_format);
			dtpDate.Value = _day;

			if (Changed != null)
				Changed(this, null);
		}

		private void btnPreviousDay_Click(object sender, EventArgs e)
		{
			SetDay(_day - TimeSpan.FromDays(1));
		}

		private void btnNextDay_Click(object sender, EventArgs e)
		{
			SetDay(_day + TimeSpan.FromDays(1));
		}

		protected override void OnResize(EventArgs e)
		{
			var btnWidth = Height;
			if (Width - btnWidth * 2 < 100)
				btnWidth = (Width - 100) / 2;

			btnPreviousDay.SetBounds(0, 0, btnWidth, Height);
			btnNextDay.SetBounds(Width - btnWidth, 0, btnWidth, Height);
			lblDate.SetBounds(btnWidth, 0, Width - btnWidth * 2, Height/2);
			dtpDate.SetBounds(btnWidth + 8, Height/2, Width - btnWidth * 2 - 16, Height / 2);
		}

		private void dtpDate_ValueChanged(object sender, EventArgs e)
		{
			SetDay(dtpDate.Value.Date);
		}
	}
}
