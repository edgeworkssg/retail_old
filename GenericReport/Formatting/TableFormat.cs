using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace GenericReport
{
    public class TableFormat
    {
        #region fields

        private bool bRepeatHeaderOnNewPage = false;

        // measurement
        private double tableWidth;
        private double tableHeaderRowHeight;
        private double tableDetailRowHeight;

        // cell color
        private CellFormat headerCellFormat;
        private CellFormat detailCellFormat;
        private CellFormat footerCellFormat;

        private Color borderColor;
        private string strBorderColor;
        private string borderStyle;

        // column format
        public List<ColumnFormat> listColumnFormat;

        #endregion

        #region properties
        public Color BorderColor
        {
            set
            {
                borderColor = value;
                strBorderColor = value.ToString().Substring(7).Replace("]", "");
            }
            get
            {
                return borderColor;
            }
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

        public string BorderColorToString
        {            
            get
            {
                return strBorderColor;
            }
        }

        public Color HeaderColor
        {
            set
            {
                headerCellFormat.BackgroundColor = value;
            }

            get
            {
                return headerCellFormat.BackgroundColor;
            }
        }

        public Color DetailColor
        {
            set
            {
                detailCellFormat.BackgroundColor = value;
            }

            get
            {
                return detailCellFormat.BackgroundColor;
            }
        }

        public Color FooterColor
        {
            set
            {
                footerCellFormat.BackgroundColor = value;
            }

            get
            {
                return footerCellFormat.BackgroundColor;
            }
        }

        public CellFormat HeaderCellFormat
        {
            set
            {
                headerCellFormat = value;
            }
            get
            {
                return headerCellFormat;
            }
        }

        public CellFormat DetailCellFormat
        {
            set
            {
                detailCellFormat = value;
            }
            get
            {
                return detailCellFormat;
            }
        }

        public CellFormat FooterCellFormat
        {
            set
            {
                footerCellFormat = value;
            }
            get
            {
                return footerCellFormat;
            }
        }

        public double TableWidth
        {
            set
            {
                if (value > 0)
                    tableWidth = value;
                else
                    MessageBox.Show("Invalid table width!");
            }

            get
            {
                return tableWidth;
            }
        }

        public double TableHeaderRowHeight
        {
            set
            {
                if (value >= 0)
                    tableHeaderRowHeight = value;
                else
                    MessageBox.Show("Invalid table header row height!");
            }

            get
            {
                return tableHeaderRowHeight;
            }
        }

        public double TableDetailRowHeight
        {
            set
            {
                if (value > 0)
                    tableDetailRowHeight = value;
                else
                    MessageBox.Show("Invalid table detail row height!");
            }

            get
            {
                return tableDetailRowHeight;
            }
        }

        public bool RepeatHeaderOnNewPage
        {
            set
            {
                bRepeatHeaderOnNewPage = value;
            }
            get
            {
                return bRepeatHeaderOnNewPage;
            }
        }

        #endregion

        // constructor
        public TableFormat()
        {
            headerCellFormat = new CellFormat();
            detailCellFormat = new CellFormat();
            footerCellFormat = new CellFormat();
            tableWidth = 10.0;
            tableHeaderRowHeight = 2.5;
            tableDetailRowHeight = 2.3;
            headerCellFormat.BackgroundColor = Color.White;
            detailCellFormat.BackgroundColor = Color.White;
            footerCellFormat.BackgroundColor = Color.White;
            BorderColor = Color.Black;
            BorderStyle = "Solid";
            bRepeatHeaderOnNewPage = false;

            listColumnFormat = new List<ColumnFormat>();
        }
    }
}
