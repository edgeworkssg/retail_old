using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace GenericReport
{
    public class CellFormat
    {
        private double nPaddingLeft;
        private double nPaddingRight;
        private double nPaddingTop;
        private double nPaddingBottom;

        private Color backgroundColor;
        private string strBackgroundColor;

        private Color borderColor;
        private string strBorderColor;

        private Color fontColor;
        private string strFontColor;

        private double borderWidth;
        private string borderStyle;
        private string textAlignment;

        private string fontStyle;
        private FontFamily fontFamily;
        private double fontSize;
        private string fontWeight;

        public CellFormat()
        {
            nPaddingLeft = 2;
            nPaddingRight = 2;
            nPaddingTop = 2;
            nPaddingBottom = 2;

            BackgroundColor = Color.White;
            BorderColor = Color.Black;
            FontColor = Color.Black;
            BorderStyle = "Solid";
            TextAlignment = "Left";

            FontFamily = new FontFamily("Arial");
            fontSize = 8.0;
            fontWeight = "Normal";
            FontStyle = "Normal";
        }

        public CellFormat(CellFormat copy)
        {
            nPaddingBottom = copy.PaddingBottom;
            nPaddingLeft = copy.PaddingLeft;
            nPaddingRight = copy.PaddingRight;
            nPaddingTop = copy.PaddingTop;

            BackgroundColor = copy.BackgroundColor;
            BorderColor = copy.BorderColor;
            FontColor = copy.FontColor;
            borderWidth = copy.borderWidth;
            BorderStyle = copy.BorderStyle;

            TextAlignment = copy.TextAlignment;
            FontWeight = copy.FontWeight;
            FontStyle = copy.FontStyle;
            FontFamily = copy.FontFamily;
            FontSize = copy.FontSize;
        }

        public string BorderStyle
        {
            set
            {
                borderStyle = value;
            }
            get
            {
                return borderStyle;
            }
        }

        public string TextAlignment
        {
            set
            {
                textAlignment = value;
            }
            get
            {
                return textAlignment;
            }
        }

        public string FontWeight
        {
            set
            {                
                fontWeight = value;                
            }

            get
            {
                return fontWeight;
            }
        }

        public string FontStyle
        {
            set
            {
                fontStyle = value;
            }

            get
            {
                return fontStyle;
            }
        }

        public FontFamily FontFamily
        {
            set
            {
                fontFamily = value;
            }

            get
            {
                return fontFamily;
            }
        }

        public double FontSize
        {
            set
            {
                fontSize = value;                
            }

            get
            {
                return fontSize;
            }
        }


        public Color BorderColor
        {
            set
            {
                borderColor = value;
                strBorderColor = ColorTranslator.ToHtml(value);
            }
            get
            {
                return borderColor;
            }
        }        

        public string BorderColorToString
        {
            get
            {
                return strBorderColor;
            }
        }

        public Color FontColor
        {
            set
            {
                fontColor = value;
                strFontColor = ColorTranslator.ToHtml(value);
            }
            get
            {
                return fontColor;
            }
        }

        public string FontColorToString
        {
            get
            {
                return strFontColor;
            }
        }

        public double PaddingLeft
        {
            set
            {
                if (value >= 0)
                    nPaddingLeft = value;
                else
                    MessageBox.Show("Invalid PaddingLeft value!");
            }

            get
            {
                return nPaddingLeft;
            }
        }

        public double PaddingRight
        {
            set
            {
                if (value >= 0)
                    nPaddingRight = value;
                else
                    MessageBox.Show("Invalid PaddingRight value!");
            }

            get
            {
                return nPaddingRight;
            }
        }

        public double PaddingTop
        {
            set
            {
                if (value >= 0)
                    nPaddingTop = value;
                else
                    MessageBox.Show("Invalid PaddingTop value!");
            }

            get
            {
                return nPaddingTop;
            }
        }

        public double PaddingBottom
        {
            set
            {
                if (value >= 0)
                    nPaddingBottom = value;
                else
                    MessageBox.Show("Invalid PaddingBottom value!");
            }

            get
            {
                return nPaddingBottom;
            }
        }

        public Color BackgroundColor
        {
            set
            {
                backgroundColor = value;
                strBackgroundColor = ColorTranslator.ToHtml(value);
                    // value.ToString().Substring(7).Replace("]", "");
            }

            get
            {
                return backgroundColor;
            }
        }

        public string BackgroundColorColorToString
        {
            get
            {
                return strBackgroundColor;
            }
        }


    }
}
