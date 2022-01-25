using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace GenericReport
{
    public class PageFormat
    {
        // formatting: page layout
        private double leftMargin;
        private double rightMargin;
        private double bottomMargin;
        private double topMargin;
        private double pageWidth;
        private double pageHeight;
        private double bodyWidth;
        private double bodyHeight;

        // formatting: header
        private Color backgroundColorHeader;
        private Color textColorHeader;
        private int fontWeightHeader;
        private FontStyle fontStyleHeader;
        private FontFamily fontFamilyHeader;
        private string fontSizeHeader;
        private string textAlignHeader;

        // formatting: body
        private Color backgroundColorBody;
        private Color textColorHeaderBody;
        private int fontWeightBody;
        private FontStyle fontStyleBody;
        private FontFamily fontFamilyBody;
        private string fontSizeBody;
        private string textAlignBody;

        private Color backgroundColorAltBody;
        private Color textColorHeaderAltBody;

        // formatting: footer
        private Color backgroundColorFooter;
        private Color textColorHeaderFooter;
        private int fontWeightFooter;
        private FontStyle fontStyleFooter;
        private FontFamily fontFamilyFooter;
        private string fontSizeFooter;
        private string textAlignFooter;

        #region properties: page layout

        public double LeftMargin
        {
            set
            {                
                if (value >= 0)
                    leftMargin = value;
                
                else
                    MessageBox.Show("Invalid left margin!");
            }

            get
            {
                return leftMargin;
            }
        }

        public double RightMargin
        {
            set
            {
                if (value >= 0)
                    rightMargin = value;
                
                else
                    MessageBox.Show("Invalid right margin!");
            }

            get
            {
                return rightMargin;
            }
        }

        public double TopMargin
        {
            set
            {
                if (value >= 0)
                    topMargin = value;
                
                else
                    MessageBox.Show("Invalid top margin!");
            }

            get
            {
                return topMargin;
            }
        }

        public double BottomMargin
        {
            set
            {
                if (value >= 0)
                    bottomMargin = value;
                
                else
                    MessageBox.Show("Invalid bottom margin!");
            }

            get
            {
                return bottomMargin;
            }
        }

        public double PageWidth
        {
            set
            {
                if (value > 0)
                    pageWidth = value;
                

                else
                    MessageBox.Show("Invalid page width!");
            }

            get
            {
                return pageWidth;
            }
        }

        public double PageHeight
        {
            set
            {
                if (value >= 0)
                    pageHeight = value;
                
                else
                    MessageBox.Show("Invalid page height!");
            }

            get
            {
                return pageHeight;
            }
        }

        public double BodyWidth
        {
            set
            {
                if (value >= 0)
                    bodyWidth = value;
                

                else
                    MessageBox.Show("Invalid body width!");
            }

            get
            {
                return bodyWidth;
            }
        }

        public double BodyHeight
        {
            set
            {
                if (value >= 0)
                    bodyHeight = value;                

                else
                    MessageBox.Show("Invalid body height!");
            }

            get
            {
                return bodyHeight;
            }
        }

        #endregion

        #region properties: header

        public Color BackgroundColorHeader
        {
            set
            {
                backgroundColorHeader = value;
            }

            get
            {
                return backgroundColorHeader;
            }
        }

        public Color TextColorHeader
        {
            set
            {
                textColorHeader = value;
            }

            get
            {
                return textColorHeader;
            }
        }

        public int FontWeightHeader
        {
            set
            {
                if (value >= 0)
                    fontWeightHeader = value;
                else
                    MessageBox.Show("Invalid font-weight set!");
            }

            get
            {
                return fontWeightHeader;
            }
        }

        public FontStyle FontStyleHeader
        {
            set
            {
                fontStyleHeader = value;
            }

            get
            {
                return fontStyleHeader;
            }
        }

        public FontFamily FontFamilyHeader
        {
            set
            {
                fontFamilyHeader = value;
            }

            get
            {
                return fontFamilyHeader;
            }
        }

        public string FontSizeHeader
        {
            set
            {
                int fontSize = 0;
                if(int.TryParse(value, out fontSize))
                    fontSizeHeader = value;
                else
                    MessageBox.Show("Invalid font size!");
            }

            get
            {
                return fontSizeHeader;
            }
        }

        public string TextAlignHeader
        {
            set
            {
                textAlignHeader = value;
            }

            get
            {
                return textAlignHeader;
            }
        }

        #endregion

        #region properties: body

        public Color BackgroundColorBody
        {
            set
            {
                backgroundColorBody = value;
            }

            get
            {
                return backgroundColorBody;
            }
        }


        public Color TextColorHeaderBody
        {
            set
            {
                textColorHeaderBody = value;
            }

            get
            {
                return textColorHeaderBody;
            }
        }

        public int FontWeightBody
        {
            set
            {
                if (value >= 0)
                    fontWeightBody = value;
                else
                    MessageBox.Show("Invalid font-weight set!");
            }

            get
            {
                return fontWeightBody;
            }
        }

        public FontStyle FontStyleBody
        {
            set
            {
                fontStyleBody = value;
            }

            get
            {
                return fontStyleBody;
            }
        }

        public FontFamily FontFamilyBody
        {
            set
            {
                fontFamilyBody = value;
            }

            get
            {
                return fontFamilyBody;
            }
        }

        public string FontSizeBody
        {
            set
            {
                int fontSize = 0;
                if (int.TryParse(value, out fontSize))
                    fontSizeBody = value;
                else
                    MessageBox.Show("Invalid font size!");
            }

            get
            {
                return fontSizeBody;
            }
        }

        public string TextAlignBody
        {
            set
            {
                textAlignBody = value;
            }

            get
            {
                return textAlignBody;
            }
        }

        public Color BackgroundColorAltBody
        {
            set
            {
                backgroundColorAltBody = value;
            }

            get
            {
                return backgroundColorAltBody;
            }
        }

        public Color TextColorHeaderAltBody
        {
            set
            {
                textColorHeaderAltBody = value;
            }

            get
            {
                return textColorHeaderAltBody;
            }
        }

        #endregion

        #region properties: footer

        public Color BackgroundColorFooter
        {
            set
            {
                backgroundColorFooter = value;
            }

            get
            {
                return backgroundColorFooter;
            }
        }

        public Color TextColorHeaderFooter
        {
            set
            {
                textColorHeaderFooter = value;
            }

            get
            {
                return textColorHeaderFooter;
            }
        }

        public int FontWeightFooter
        {
            set
            {
                if (value >= 0)
                    fontWeightFooter = value;
                else
                    MessageBox.Show("Invalid font-weight set!");
            }

            get
            {
                return fontWeightFooter;
            }
        }

        public FontStyle FontStyleFooter
        {
            set
            {
                fontStyleFooter = value;
            }

            get
            {
                return fontStyleFooter;
            }
        }

        public FontFamily FontFamilyFooter
        {
            set
            {
                fontFamilyFooter = value;
            }

            get
            {
                return fontFamilyFooter;
            }
        }

        public string FontSizeFooter
        {
            set
            {
                int fontSize = 0;
                if (int.TryParse(value, out fontSize))
                    fontSizeFooter = value;
                else
                    MessageBox.Show("Invalid font size!");
            }

            get
            {
                return fontSizeFooter;
            }
        }

        public string TextAlignFooter
        {
            set
            {
                textAlignFooter = value;
            }

            get
            {
                return textAlignFooter;
            }
        }

        #endregion

        public PageFormat()
        {
            leftMargin = 2;
            rightMargin = 2;
            bottomMargin = 2;
            topMargin = 2;
            pageWidth = 18;
            pageHeight = 21;
            bodyWidth = 16;
            bodyHeight = 19;

            backgroundColorHeader = Color.FromArgb(238, 238, 238); //gray
            textColorHeader = Color.FromArgb(0, 0, 0);
            fontWeightHeader = 100;
            fontStyleHeader = FontStyle.Regular;
            fontFamilyHeader = new FontFamily("Arial");
            fontSizeHeader = "8pt";
            textAlignHeader = "Left";

            backgroundColorBody = Color.FromArgb(255, 255, 255);
            textColorHeaderBody = Color.FromArgb(0, 0, 0);
            fontWeightBody = 100;
            fontStyleBody = FontStyle.Regular;
            fontFamilyBody = new FontFamily("Arial");
            fontSizeBody = "8pt";
            textAlignBody = "Left";

            backgroundColorAltBody = Color.FromArgb(238, 238, 238); //gray
            textColorHeaderAltBody = Color.FromArgb(0, 0, 0);

            backgroundColorFooter = Color.FromArgb(255, 255, 255);
            textColorHeaderFooter = Color.FromArgb(0, 0, 0);
            fontWeightFooter = 100;
            fontStyleFooter = FontStyle.Regular;
            fontFamilyFooter = new FontFamily("Arial");
            fontSizeFooter = "8pt";
            textAlignFooter = "Left";
        }        

    }
}
