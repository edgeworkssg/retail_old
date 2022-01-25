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
    public class TextComponent : PrintedComponent
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

        public TextComponent(string Name, Rectangle PrintedLocation, Font TextFont, StringAlignment TextAllign)
        {
            this.Name = Name;
            this.PrintedLocation = PrintedLocation;
            this.PrintedFont = TextFont;
            this.TextAllign = TextAllign;
            this.Value = "";
        }

        //public void Print(ref PrintPageEventArgs e)
        //{
        //    if (isBarcode)
        //        PrintBarcode(ref e);
        //    else
        //        PrintText(ref e);
        //}

        //private void Print(ref PrintPageEventArgs e)
        //{
        //    if (Value == null || Value == "") return;

        //    Image OriBarcodeImg = Code128Rendering.MakeBarcodeImage(Value, 1, true);

        //    Bitmap DstBarcodeImg = new Bitmap(OriBarcodeImg.Width, PrintedLocation.Height);
        //    Graphics aa = Graphics.FromImage(DstBarcodeImg);
        //    for (int Counter = 0; Counter < DstBarcodeImg.Height; Counter += OriBarcodeImg.Height)
        //        aa.DrawImage(OriBarcodeImg, 0, Counter, OriBarcodeImg.Width, OriBarcodeImg.Height);
        //    aa.Dispose();

        //    int srcWidth = DstBarcodeImg.Width;
        //    int dstWidth = PrintedLocation.Width;
        //    PrintedLocation = new Rectangle(((dstWidth - srcWidth) / 2), PrintedLocation.Y, srcWidth, DstBarcodeImg.Height);

        //    e.Graphics.DrawImage(DstBarcodeImg, PrintedLocation);
        //}

        public void Print(ref PrintPageEventArgs e)
        {
            StringFormat StringAllign = new StringFormat(StringFormat.GenericTypographic);
            StringAllign.Alignment = TextAllign;

            if (Value == null || Value == "") return;
            if (TextFont.FontFamily.Name.Contains("Free3"))
            {
                Value = "*" + Value + "*";
            }
            e.Graphics.DrawString(Value, PrintedFont, Brushes.Black, PrintedLocation);

            //int TextHeight = (int)e.Graphics.MeasureString(PrintedItem, printFont, PageWidth).Height;
            //e.Graphics.DrawString(PrintedItem, PrintedFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, TextHeight));
            //y += TextHeight;
        }

        public void Paint(ref PaintEventArgs e)
        {
            StringFormat StringAllign = new StringFormat(StringFormat.GenericTypographic);
            StringAllign.Alignment = TextAllign;

            if (Name == null || Name == "") return;
            if (TextFont.FontFamily.Name.Contains("Free3"))
            {
                Name = "*" + Name + "*";
            }
            e.Graphics.DrawString(Name, TextFont, Brushes.Black, PrintedLocation);

            //int TextHeight = (int)e.Graphics.MeasureString(PrintedItem, printFont, PageWidth).Height;
            //e.Graphics.DrawString(PrintedItem, PrintedFont, Brushes.Black, new RectangleF(Padding, y, PageWidth - Padding, TextHeight));
            //y += TextHeight;
        }

        public void SetValue(object Value)
        {
            if (Value == null) Value = "";
            this.Value = Value.ToString();
        }
    }
}
