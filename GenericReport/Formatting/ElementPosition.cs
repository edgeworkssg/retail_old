using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericReport
{
    public class ElementPosition
    {
        public double x;
        public double y;

        public ElementPosition()
        {
            x = 0.0;
            y = 0.0;
        }

        public ElementPosition(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
