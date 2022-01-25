using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;


namespace AppointmentBook.Model
{
	public class Employee
	{
		public string Id;
		public string Name;
		public Image Image;
		public EmployeeGender Gender = EmployeeGender.Female;
		public List<TimeSlot> Schedule = new List<TimeSlot>();
        public Dictionary<DateTime, int> TimeSlotScroll = new Dictionary<DateTime, int>();




		public override string ToString()
		{
			return Name;
		}
	}
}
