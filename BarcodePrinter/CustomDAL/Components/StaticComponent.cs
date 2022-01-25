using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace BarcodePrinter
{
    [Serializable]
    public partial class StaticComponent : PrintedComponent
    {
        private string _Name;
        public Rectangle PrintedLocation;
        private Font TextFont;
        public StringAlignment TextAllign;
        public string Value;

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        public int Left
        {
            get { return PrintedLocation.Left; }
            set { PrintedLocation = new Rectangle(value, PrintedLocation.Top, PrintedLocation.Width, PrintedLocation.Height); }
        }
        public int Top
        {
            get { return PrintedLocation.Top; }
            set { PrintedLocation = new Rectangle(PrintedLocation.Left, value, PrintedLocation.Width, PrintedLocation.Height); }
        }
        public int Width
        {
            get { return PrintedLocation.Width; }
            set { PrintedLocation = new Rectangle(PrintedLocation.Left, PrintedLocation.Top, value, PrintedLocation.Height); }
        }
        public int Height
        {
            get { return PrintedLocation.Height; }
            set { PrintedLocation = new Rectangle(PrintedLocation.Left, PrintedLocation.Top, PrintedLocation.Width, value); }
        }
        public Font PrintedFont
        {
            get { return TextFont; }
            set { TextFont = value; }
        }

        public StaticComponent(string Name, Rectangle PrintedLocation, Font TextFont, StringAlignment TextAllign)
        {
            this.Name = Name;
            this.PrintedLocation = PrintedLocation;
            this.PrintedFont = TextFont;
            this.TextAllign = TextAllign;
            this.Value = "";
        }

        public void Print(ref PrintPageEventArgs e)
        {
            StringFormat StringAllign = new StringFormat(StringFormat.GenericTypographic);
            StringAllign.Alignment = TextAllign;

            e.Graphics.DrawString(_Name, PrintedFont, Brushes.Black, PrintedLocation);
        }

        public void Paint(ref PaintEventArgs e)
        {
            StringFormat StringAllign = new StringFormat(StringFormat.GenericTypographic);
            StringAllign.Alignment = TextAllign;

            e.Graphics.DrawString(Name, TextFont, Brushes.Black, PrintedLocation);
        }

        public void SetValue(object Value)
        {
            if (Value == null) Value = "";
            this.Value = Value.ToString();
        }
    }
}

