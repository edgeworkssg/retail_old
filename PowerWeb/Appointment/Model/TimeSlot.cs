using System;
using System.Drawing;

namespace PowerWeb.Appointment
{
	public class TimeSlot
	{
        private Guid _id;
        public Guid Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private DateTime _startTime;
        public DateTime StartTime
        {
            get { return _startTime; }
            set { _startTime = value; }
        }

        private TimeSpan _duration;
        public TimeSpan Duration
        {
            get { return _duration; }
            set { _duration = value; }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        private Color _backColor;
        public Color BackColor
        {
            get { return _backColor; }
            set { _backColor = value; }
        }

        private Color _fontColor;
        public Color FontColor
        {
            get { return _fontColor; }
            set { _fontColor = value; }
        }

        private bool _check;
        public bool Check
        {
            get { return _check; }
            set { _check = value; }
        }

        private string _completeDescription;
        public string CompleteDescription
        {
            get { return _completeDescription; }
            set { _completeDescription = value; }
        }

        private Rectangle _region;
        public Rectangle Region
        {
            get { return _region; }
            set { _region = value; }
        }

        private Employee _employee;
        public Employee Employee
        {
            get { return _employee; }
            set { _employee = value; }
        }

        private string _organization;
        public string Organization
        {
            get { return _organization; }
            set { _organization = value; }
        }

        private string _pickupLocation;
        public string PickupLocation
        {
            get { return _pickupLocation; }
            set { _pickupLocation = value; }
        }

        private int _noOfChildren;
        public int NoOfChildren
        {
            get { return _noOfChildren; }
            set { _noOfChildren = value; }
        }

        private bool _isPartialPayment = false;
        public bool IsPartialPayment
        {
            get { return _isPartialPayment; }
            set { _isPartialPayment = value; }
        }

        private decimal _outStandingAmount = 0;
        public decimal OutStandingAmount
        {
            get { return _outStandingAmount; }
            set { _outStandingAmount = value; }
        }

        public TimeSlot Clone()
        {
            return new TimeSlot
                {
                    Id = Id,
                    BackColor = BackColor,
                    Description = Description,
                    Duration = Duration,
                    Employee = Employee,
                    FontColor = FontColor,
                    Region = Region,
                    StartTime = StartTime,
                    Check = Check,
                    CompleteDescription = CompleteDescription,
                    IsPartialPayment = IsPartialPayment,
                    OutStandingAmount = OutStandingAmount
                };
        }
	}
}