using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace AppointmentBook.Model
{
    public class AppointmentResource
    {
        public string ResourceID;
        public string Name;
        public int Capacity;
        public Image Image;
        public List<TimeSlot> Schedule = new List<TimeSlot>();
        public Dictionary<DateTime, int> TimeSlotScroll = new Dictionary<DateTime, int>();

        public override string ToString()
        {
            return Name;
        }
    }
}
