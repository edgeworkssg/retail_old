using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;


namespace PowerWeb.Appointment
{
    [Serializable]
    public class AppointmentForm
    {
        public Guid ID { set; get; }
        public string SalesPersonID { set; get; }
        public string MembershipNo { set; get; }
        public DateTime StartTime { set; get; }
        public string ResourceID { set; get; }
        public int Duration { set; get; }
        public int FontColor { set; get; }
        public int BackColor { set; get; }
        public string Service { set; get; }
        public string Description { set; get; }
    }
}