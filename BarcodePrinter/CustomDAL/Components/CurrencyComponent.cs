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
    public class CurrencyComponent : PrintedComponent
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

        public CurrencyComponent(string Name, Rectangle PrintedLocation, Font TextFont, StringAlignment TextAllign)
        {
            this.Name = Name;
            this.PrintedLocation = PrintedLocation;
            this.TextFont = TextFont;
            this.TextAllign = TextAllign;
            this.Value = (0).ToString("C2");
        }

        public void Print(ref PrintPageEventArgs e)
        {
            StringFormat StringAllign = new StringFormat(StringFormat.GenericTypographic);
            StringAllign.Alignment = TextAllign;

            if (Value == null || Value == "") return;

            e.Graphics.DrawString(Value, TextFont, Brushes.Black, PrintedLocation);

            //int TextHeight = (int)e.Graphics.MeasureString(PrintedItem, printFont, PageWidth).Height;
            //e.Graphics.DrawString(PrintedItem, PrintedFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, TextHeight));
            //y += TextHeight;
        }

        public void Paint(ref PaintEventArgs e)
        {
            StringFormat StringAllign = new StringFormat(StringFormat.GenericTypographic);
            StringAllign.Alignment = TextAllign;

            if (Name == null || Name == "") return;

            e.Graphics.DrawString(Name, TextFont, Brushes.Black, PrintedLocation);

            //int TextHeight = (int)e.Graphics.MeasureString(PrintedItem, printFont, PageWidth).Height;
            //e.Graphics.DrawString(PrintedItem, PrintedFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, TextHeight));
            //y += TextHeight;
        }

        public void SetValue(object Value)
        {
            if (Value == null) Value = (0).ToString("C2");
            
            decimal Temp =0;
            if (decimal.TryParse(Value.ToString(), out Temp))
            {
                this.Value = Temp.ToString("C2");
            }
            else
                this.Value = (0).ToString("C2");
        }
    }
}
