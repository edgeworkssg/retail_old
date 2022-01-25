using System;
using System.Drawing;

namespace AppointmentBook.Model
{
	public class TimeSlot
	{
		public Guid Id;
		public DateTime StartTime;
		public TimeSpan Duration;
		public string Description;
		public Color BackColor;
		public Color FontColor;
		public bool Check;
        public string CompleteDescription;
        public int TimeExtension;

		public Rectangle Region;
		public Employee Employee;
        public AppointmentResource Resource;

        public string Organization;
        public string PickupLocation;
        public int NoOfChildren;

        public bool IsPartialPayment = false;
        public decimal OutStandingAmount = 0;

        public Int32 xPosition = 0;
        public Int32 yPosition = 0;

		public TimeSlot Clone()
		{
			return new TimeSlot
				{
					Id = Id,
					BackColor = BackColor,
					Description = Description,
					Duration = Duration,
					Employee = Employee,
                    Resource = Resource,
					FontColor = FontColor,
					Region = Region,
					StartTime = StartTime,
					Check = Check,
                    CompleteDescription = CompleteDescription,
                    IsPartialPayment = IsPartialPayment,
                    OutStandingAmount = OutStandingAmount,
                    xPosition = xPosition,
                    yPosition = yPosition,
                    TimeExtension = TimeExtension
				};
		}
	}
}