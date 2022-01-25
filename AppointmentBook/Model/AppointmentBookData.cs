using System.Collections.Generic;

namespace AppointmentBook.Model
{
	public class AppointmentBookData
	{
		public List<Employee> Employees = new List<Employee>();

        public List<AppointmentResource> Resources = new List<AppointmentResource>();
	}
}