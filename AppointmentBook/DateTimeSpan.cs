using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppointmentBook
{
    public class DateTimeSpan
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public bool Intersects(DateTimeSpan dateToCompare)
        {
            if (this.Start > this.End || dateToCompare.Start > dateToCompare.End)
                return false;

            if (this.Start == this.End || dateToCompare.Start == dateToCompare.End)
                return false; // No actual date range

            if (this.Start == dateToCompare.Start || this.End == dateToCompare.End)
                return true; // If any set is the same time, then by default there must be some overlap. 

            if (this.Start < dateToCompare.Start)
            {
                if (this.End > dateToCompare.Start && this.End < dateToCompare.End)
                    return true; // Condition 1

                if (this.End > dateToCompare.End)
                    return true; // Condition 3
            }
            else
            {
                if (dateToCompare.End > this.Start && dateToCompare.End < this.End)
                    return true; // Condition 2

                if (dateToCompare.End > this.End)
                    return true; // Condition 4
            }

            return false;
        }
    }
}
