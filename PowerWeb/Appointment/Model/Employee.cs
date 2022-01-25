using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;


namespace PowerWeb.Appointment
{
    [Serializable]
	public class Employee
	{
        private string _id;
        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private byte[] _image;
        public byte[] Image
        {
            get { return _image; }
            set { _image = value; }
        }

        public string ImageURL
        {
            get
            {
                string url = "";
                if (_image == null || _image.Length == 0)
                {
                    url = _gender == EmployeeGender.Male ? "images/male_user.png" : "images/female_user.png";
                }
                else
                {
                    url = "data:image/png;base64," + Convert.ToBase64String(_image, 0, _image.Length);
                }
                return url;
            }
        }

        private EmployeeGender _gender = EmployeeGender.Female;
        public EmployeeGender Gender
        {
            get { return _gender; }
            set { _gender = value; }
        }

        private List<TimeSlot> _schedule = new List<TimeSlot>();
        public List<TimeSlot> Schedule
        {
            get 
            {
                if (_schedule == null)
                    _schedule = new List<TimeSlot>();
                return _schedule; 
            }
            set { _schedule = value; }
        }

		public override string ToString()
		{
            return _name;
		}
	}
}
