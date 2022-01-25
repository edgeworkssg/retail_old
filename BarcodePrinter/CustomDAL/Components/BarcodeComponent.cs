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
    public class BarcodeComponent : PrintedComponent
    {
        private string _Name;
        public Rectangle PrintedLocation;
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
            get { return new Font("Arial Narrow", 8); }
            set { }
        }

        public BarcodeComponent(string Name, Rectangle PrintedLocation, StringAlignment TextAllign)
        {
            this.Name = Name;
            this.PrintedLocation = PrintedLocation;
            this.TextAllign = TextAllign;
            this.Value = "";
        }

        public void Print(ref PrintPageEventArgs e)
        {
            if (Value == null || Value == "") return;

            double modifier = (double)Width / 10;

            /// Do not change the original measurement, please.. (T_T) You will regret it.. 
            Rectangle TempPrintLocation = PrintedLocation;

            /*BarcodeLib.Barcode b = new BarcodeLib.Barcode();
            b.Alignment = BarcodeLib.AlignmentPositions.CENTER;
            //b.IncludeLabel = true;
            b.RotateFlipType = RotateFlipType.RotateNoneFlipNone;
            b.LabelPosition = BarcodeLib.LabelPositions.BOTTOMCENTER;
            Image OriBarcodeImg = b.Encode(BarcodeLib.TYPE.CODE128, Value, Color.Black, Color.White, (int)Math.Ceiling(modifier * b.Width), PrintedLocation.Height == 0 ? (int)Math.Ceiling(modifier * b.Height) : PrintedLocation.Height);

            e.Graphics.DrawImage(OriBarcodeImg, PrintedLocation.X - 50, PrintedLocation.Y);*/
            Code123Auto code = new Code123Auto();
            Image barcode = code.DrawBarcode_Code128Auto(Value, "yes", "in", 0, 0, Height, "bottom", "left", "", "black", "white", "html");
            e.Graphics.DrawImage(barcode, PrintedLocation.X, PrintedLocation.Y);



        }
        public void Paint(ref PaintEventArgs e)
        {
            if (Name == null || Name == "") return;

            double modifier = (double)Width / 10;
            
            /// Do not change the original measurement, please.. (T_T) You will regret it.. 
            Rectangle TempPrintLocation = PrintedLocation;

            BarcodeLib.Barcode b = new BarcodeLib.Barcode();
            b.Alignment = BarcodeLib.AlignmentPositions.CENTER;
            //b.IncludeLabel = true;
            b.RotateFlipType = RotateFlipType.RotateNoneFlipNone;
            b.LabelPosition = BarcodeLib.LabelPositions.BOTTOMCENTER;
            Image OriBarcodeImg = b.Encode(BarcodeLib.TYPE.CODE128, Name, Color.Black, Color.White, (int)Math.Ceiling(modifier * b.Width), PrintedLocation.Height == 0 ? (int)Math.Ceiling(modifier * b.Height) : PrintedLocation.Height);

            e.Graphics.DrawImage(OriBarcodeImg, PrintedLocation.X, PrintedLocation.Y);


//<<<<<<< .mine
//            DstBarcodeImg = new Bitmap(DstBarcodeImg, new Size(DstBarcodeImg.Width/2, DstBarcodeImg.Height/2));
            
//            e.Graphics.DrawImage(DstBarcodeImg, TempPrintLocation);
//=======
//            //Image OriBarcodeImg = Code128Rendering.MakeBarcodeImage(Name, 1, true);

//            //Bitmap DstBarcodeImg = new Bitmap(OriBarcodeImg.Width, PrintedLocation.Height);
//            //Graphics aa = Graphics.FromImage(DstBarcodeImg);
//            //for (int Counter = 0; Counter < DstBarcodeImg.Height; Counter += OriBarcodeImg.Height)
//            //    aa.DrawImage(OriBarcodeImg, 0, Counter, OriBarcodeImg.Width, OriBarcodeImg.Height);
//            //aa.Dispose();

//            //int srcWidth = DstBarcodeImg.Width;
//            //int dstWidth = PrintedLocation.Width;
//            //TempPrintLocation = new Rectangle(((dstWidth - srcWidth) / 2), PrintedLocation.Y, srcWidth, DstBarcodeImg.Height);

//            //e.Graphics.DrawImage(DstBarcodeImg, TempPrintLocation);
//>>>>>>> .r5
        }

        public void SetValue(object Value)
        {
            if (Value == null) Value = "";
            this.Value = Value.ToString();
        }
    }
}
