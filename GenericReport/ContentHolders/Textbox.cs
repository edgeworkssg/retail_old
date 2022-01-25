using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GenericReport
{
    public class Textbox
    {
        private string txtName;
        private double txtHeight;
        private double txtWidth;
        private string txtValue;
        public ElementPosition posTxt;
        public CellFormat cellFormat;

        public Textbox()
        {
            txtName = "";
            txtHeight = 0.0;
            txtWidth = 0.0;
            posTxt = new ElementPosition();
            cellFormat = new CellFormat();
        }

        public string TextName
        {
            set
            {
                txtName = value;
            }
            get
            {
                return txtName;
            }
        }

        public string TextValue
        {
            set
            {
                txtValue = value;
            }
            get
            {
                return txtValue;
            }
        }

        public double TextHeight
        {
            set
            {
                if (value >= 0)
                    txtHeight = value;
                else
                    MessageBox.Show("Invalid text height! Height = " + value.ToString(), "ERROR");
            }
            get
            {
                return txtHeight;
            }
        }

        public double TextWidth
        {
            set
            {
                if (value >= 0)
                    txtWidth = value;
                else
                    MessageBox.Show("Invalid text width! Width = " + value.ToString(), "ERROR");
            }
            get
            {
                return txtWidth;
            }
        }
    }
}
