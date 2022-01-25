using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GenericReport
{
    public class Image
    {
        private string imgName;
        private string imgPath;
        private string imgFileName;
        private string imgMIMEType;
        public ElementPosition posImg;
        private string imgSizing;

        private double imgHeight;
        private double imgWidth;
        
        public Image()
        {
            imgHeight = 0.0;
            imgWidth = 0.0;
            imgPath = "";
            imgName = "";
            imgFileName = "";
            imgSizing = Sizing.AutoSize;
            posImg = new ElementPosition();
        }

        public string ImageName
        {
            set
            {
                imgName = value;
            }
            get
            {
                return imgName;
            }
        }

        public string ImagePath
        {
            set
            {
                imgPath = value;
            }
            get
            {
                return imgPath;
            }
        }

        public string ImageFileName
        {
            set
            {
                imgFileName = value;
            }
            get
            {
                return imgFileName;
            }
        }

        public string ImageMIMEType
        {
            set
            {
                imgMIMEType = value;
            }
            get
            {
                return imgMIMEType;
            }
        }

        public string ImageSizing
        {
            set
            {
                imgSizing = value;
            }
            get
            {
                return imgSizing;
            }
        }

        public double ImageHeight
        {
            set
            {
                if (value >= 0)
                    imgHeight = value;
                else
                    MessageBox.Show("Invalid image height! Height = " + value.ToString(), "ERROR");
            }
            get
            {
                return imgHeight;
            }
        }

        public double ImageWidth
        {
            set
            {
                if (value >= 0)
                    imgWidth = value;
                else
                    MessageBox.Show("Invalid image width! Width = " + value.ToString(), "ERROR");
            }
            get
            {
                return imgWidth;
            }
        }
    }
}
