using System.Collections.Generic;
using System;

namespace PowerWeb.Appointment
{
    [Serializable]
	public class AppointmentBookData
	{
        private List<Employee> _employees;
        public List<Employee> Employees
        {
            set
            {
                _employees = value;
            }
            get
            {
                if (_employees == null)
                    _employees = new List<Employee>();
                return _employees;
            }
        }
	}
}