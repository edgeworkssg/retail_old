using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace BarcodePrinter
{
    public class Code123Auto
    {
        private Image DrawHTMLBarcode_Code128Auto(string data,
                            string humanReadable,
                            string units,
                            int minBarWidth,
                            int width, int height,
                            string textLocation,
                            string textAlignment,
                            string textStyle,
                            string foreColor,
                            string backColor)
        {
            return DrawBarcode_Code128Auto(data,
                         humanReadable,
                         units,
                         minBarWidth,
                         width, height,
                          textLocation,
                          textAlignment,
                          textStyle,
                          foreColor,
                          backColor,
                         "html");
        }

        public Image DrawBarcode_Code128Auto(string data,
                            string humanReadable,
                            string units,
                            int minBarWidth,
                            int width, int height,
                            string textLocation,
                            string textAlignment,
                            string textStyle,
                            string foreColor,
                            string backColor,
                            string mode)
        {

            if (foreColor == "")
                foreColor = "black";
            if (backColor == "")
                backColor = "white";

            if (textLocation == "")
                textLocation = "bottom";
            else if (textLocation != "bottom" && textLocation != "top")
                textLocation = "bottom";
            if (textAlignment == "")
                textAlignment = "center";
            else if (textAlignment != "center" && textAlignment != "left" && textAlignment != "right")
                textAlignment = "center";
            if (textStyle == "")
                textStyle = "";
            if (height == 0)
                height = 1;
            else if (height <= 0 || height > 50)
                height = 25;
            if (width == 0)
                width = 2;
            else if (width <= 0 || width > 15)
                width = 3;
            if (minBarWidth == 0)
                minBarWidth = 0;
            else if (minBarWidth < 0 || minBarWidth > 2)
                minBarWidth = 0;
            if (units == "")
                units = "in";
            else if (units != "in" && units != "cm")
                units = "in";
            if (humanReadable == "")
                humanReadable = "yes";
            else if (humanReadable != "yes" && humanReadable != "no")
                humanReadable = "yes";

            string encodedData = EncodeCode128Auto(data);
            //string humanReadableText = "";
            int encodedLength = 0;
            //int thinLength = 0;
            //decimal thickLength = 0;
            decimal totalLength = 0;
            int incrementWidth = 0;
            //decimal swing = 1;
            //string result="";
            int barWidth = 0;
            //decimal thickWidth=0;
            //string svg;

            //Image img = new Image();


            encodedLength = encodedData.Length;
            totalLength = encodedLength;

            if (minBarWidth > 0)
            {
                barWidth = minBarWidth;
                width = Convert.ToInt32(Math.Round(barWidth * totalLength, 0));
            }
            else
                barWidth = 1;  //Convert.ToInt32(Math.Round((width * 50 / totalLength), 0));

            int start = 0;
            Bitmap b = new Bitmap(encodedData.Length * barWidth, height);
            System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
            System.Drawing.Graphics formGraphics;
            //formGraphics = this.CreateGraphics();
            formGraphics = Graphics.FromImage(b);

            for (int x = 0; x < encodedData.Length; x++)
            {
                string brush;

                if (encodedData.Substring(x, 1) == "b")
                    myBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
                else
                    myBrush = new System.Drawing.SolidBrush(System.Drawing.Color.White);

                formGraphics.FillRectangle(myBrush, new Rectangle(incrementWidth, 0, (int)barWidth, height));
                incrementWidth = incrementWidth + barWidth;
                /*if (mode=="html")
                  result=result
                       +'<span style="border-left:'
                       +barWidth
                       +units
                       +' solid ' 
                       +brush
                       +';height:'
                       +height
                       +units+';display:inline-block;"></span>';
                */

            }



            return (Image)b;
        }



        int getCode128AutoValue(int inputvalue)
        {
            if (inputvalue <= 94 + 32 && inputvalue >= 0 + 32)
                inputvalue = inputvalue - 32;
            else if (inputvalue <= 106 + 32 + 100 && inputvalue >= 95 + 32 + 100)
                inputvalue = inputvalue - 32 - 100;
            else
                inputvalue = -1;

            return inputvalue;
        }

        string filterInput(string data)
        {
            string Result = "";
            int datalength = data.Length;
            for (int x = 0; x < datalength; x++)
            {
                if (data[x] >= 0 && data[x] <= 127)
                {
                    Result = Result + data.Substring(x, 1);
                }
            }

            return Result;
        }

        int detectAllNumbers(string data)
        {
            var Result = "";
            var allnumbers = 1;

            var datalength = data.Length;

            if (datalength % 2 == 1)
                allnumbers = 0;
            else
            {
                for (int x = 0; x < datalength; x++)
                {
                    char barcodechar = data[x];
                    if ((barcodechar <= 57) && (barcodechar >= 48))
                    {
                    }
                    else
                        allnumbers = 0;
                }
            }

            return allnumbers;

        }

        private string addShift(string data)
        {
            int datalength = 0;
            string strResult = "";
            string shiftchar = Char.ConvertFromUtf32(230);

            datalength = data.Length;

            for (int x = 0; x < datalength; x++)
            {
                string barcodechar = data.Substring(x, 1);
                char barcodevalue = barcodechar[0];
                if ((barcodevalue <= 31) && (barcodevalue >= 0))
                {

                    strResult = strResult + shiftchar;
                    barcodechar = Char.ConvertFromUtf32(barcodechar[0] + 96);
                    strResult = strResult + barcodechar;
                }
                else
                    strResult = strResult + barcodechar;
            }

            return strResult;

        }

        private int ScanAhead_8orMore_Numbers(string data, int x)
        {
            int numNumbers = 0;
            int exitx = 0;
            while ((x < data.Length) && (exitx == 0))
            {
                string barcodechar = data.Substring(x, 1);
                char barcodevalue = barcodechar[0];
                if (barcodevalue >= 48 && barcodevalue <= 57)
                    numNumbers = numNumbers + 1;
                else
                    exitx = 1;

                x = x + 1;

            }
            if (numNumbers > 8)
            {
                if (numNumbers % 2 == 1)
                    numNumbers = numNumbers - 1;
            }
            return numNumbers;

        }

        private string getAutoSwitchingAB(string data)
        {

            int datalength = 0;
            string strResult = "";
            string shiftchar = Char.ConvertFromUtf32(230);

            datalength = data.Length;
            string barcodechar = "";
            int x = 0;

            for (x = 0; x < datalength; x++)
            {
                barcodechar = data.Substring(x, 1);
                char barcodevalue = barcodechar[0];

                if (barcodevalue == 31)
                {
                    barcodechar = Char.ConvertFromUtf32(barcodechar[0] + 96 + 100);
                    strResult = strResult + barcodechar;
                }
                else if (barcodevalue == 127)
                {
                    barcodechar = Char.ConvertFromUtf32(barcodechar[0] + 100);
                    strResult = strResult + barcodechar;
                }
                else
                {
                    int num = ScanAhead_8orMore_Numbers(data, x);

                    if (num >= 8)
                    {
                        strResult = OptimizeNumbers(data, x, strResult, num);
                        x = x + num;
                        x = x - 1;
                    }
                    else
                        strResult = strResult + barcodechar;
                }

            }
            return strResult;
        }

        private string OptimizeNumbers(string data, int x, string strResult, int num)
        {

            string BtoC = Char.ConvertFromUtf32(231);
            strResult = strResult + BtoC;

            int endpoint = x + num;
            while (x < endpoint)
            {
                int twonum = int.Parse(data.Substring(x, 2));
                strResult = strResult + getCode128CCharacterAuto(twonum);
                x = x + 2;
            }

            var CtoB = Char.ConvertFromUtf32(232);
            strResult = strResult + CtoB;
            return strResult;
        }

        private string getCode128CCharacterAuto(int inputvalue)
        {

            if ((inputvalue <= 94) && (inputvalue >= 0))
                inputvalue = inputvalue + 32;
            else if ((inputvalue <= 106) && (inputvalue >= 95))
                inputvalue = inputvalue + 32 + 100;
            else
                inputvalue = -1;


            return Char.ConvertFromUtf32(inputvalue);

        }

        string generateCheckDigit_Code128ABAuto(string data)
        {
            int datalength = 0;
            int Sum = 104;
            int Result = -1;
            string strResult = "";

            datalength = data.Length;

            int num = 0;
            int Weight = 1;

            int x = 0;
            while (x < data.Length)
            {
                num = ScanAhead_8orMore_Numbers(data, x);
                if (num >= 8)
                {
                    int endpoint = x + num;

                    var BtoC = 99;
                    Sum = Sum + (BtoC * (Weight));
                    Weight = Weight + 1;

                    while (x < endpoint)
                    {
                        num = int.Parse(data.Substring(x, 2));
                        Sum = Sum + (num * (Weight));
                        x = x + 2;
                        Weight = Weight + 1;

                    }
                    int CtoB = 100;
                    Sum = Sum + (CtoB * (Weight));
                    Weight = Weight + 1;

                }
                else
                {
                    num = data[x];
                    Sum = Sum + (getCode128ABValueAuto(num) * (Weight));
                    x = x + 1;
                    Weight = Weight + 1;

                }
            }
            Result = Sum % 103;
            strResult = getCode128ABCharacterAuto(Result);
            return strResult;
        }

        private string getCode128ABCharacterAuto(int inputvalue)
        {
            if ((inputvalue <= 94) && (inputvalue >= 0))
                inputvalue = inputvalue + 32;
            else if ((inputvalue <= 106) && (inputvalue >= 95))
                inputvalue = inputvalue + 100 + 32;
            else
                inputvalue = -1;


            return Char.ConvertFromUtf32(inputvalue);

        }

        private int getCode128ABValueAuto(int inputchar)
        {

            int returnvalue = 0;

            if ((inputchar <= 31) && (inputchar >= 0))
                returnvalue = (inputchar + 64);
            else if ((inputchar <= 127) && (inputchar >= 32))
                returnvalue = (inputchar - 32);
            else if (inputchar == 230)
                returnvalue = 98;
            else
                returnvalue = -1;

            return returnvalue;

        }

        private string generateCheckDigit_Code128CAuto(string data)
        {
            int datalength = 0;
            int Sum = 105;
            int Result = -1;
            string strResult = "";

            datalength = data.Length;

            int x = 0;
            int Weight = 1;
            int num = 0;

            for (x = 0; x < data.Length; x = x + 2)
            {
                num = int.Parse(data.Substring(x, 2));
                Sum = Sum + (num * Weight);
                Weight = Weight + 1;
            }

            Result = Sum % 103;
            strResult = getCode128CCharacter(Result);
            return strResult;
        }

        string getCode128CCharacter(int inputvalue)
        {
            if ((inputvalue <= 94) && (inputvalue >= 0))
                inputvalue = inputvalue + 32;
            else if ((inputvalue <= 106) && (inputvalue >= 95))
                inputvalue = inputvalue + 32 + 100;
            else
                inputvalue = -1;

            return Char.ConvertFromUtf32(inputvalue);


        }
        string ConnectCode_Encode_Code128Auto(string data)
        {

            string cd = "";
            string Result = "";
            string shiftdata = "";

            string filtereddata = filterInput(data);

            int filteredlength = filtereddata.Length;
            if (filteredlength > 254)
            {
                filtereddata = filtereddata.Substring(0, 254);
            }

            if (detectAllNumbers(filtereddata) == 0)
            {
                filtereddata = addShift(filtereddata);
                cd = generateCheckDigit_Code128ABAuto(filtereddata);

                filtereddata = getAutoSwitchingAB(filtereddata);

                filtereddata = filtereddata + cd;
                Result = filtereddata;

                int startc = 236;
                int stopc = 238;
                Result = Char.ConvertFromUtf32(startc) + Result + Char.ConvertFromUtf32(stopc);
            }
            else
            {
                cd = generateCheckDigit_Code128CAuto(filtereddata);
                var lenFiltered = filtereddata.Length;

                for (int x = 0; x < lenFiltered; x = x + 2)
                {
                    string tstr = filtereddata.Substring(x, 2);
                    int num = int.Parse(tstr);
                    Result = Result + getCode128CCharacterAuto(num);
                }


                Result = Result + cd;
                int startc = 237;
                int stopc = 238;
                Result = Char.ConvertFromUtf32(startc) + Result + Char.ConvertFromUtf32(stopc);

            }
            // here don't yet
            //Result=html_decode(html_escape(Result));	               
            return Result;
        }


        public string EncodeCode128Auto(string data)
        {
            var fontOutput = ConnectCode_Encode_Code128Auto(data);
            var output = "";
            var pattern = "";
            for (int x = 0; x < fontOutput.Length; x++)
            {
                switch (getCode128AutoValue(fontOutput.Substring(x, 1)[0]))
                {
                    case 0:
                        pattern = "bbwbbwwbbww";
                        break;
                    case 1:
                        pattern = "bbwwbbwbbww";
                        break;
                    case 2:
                        pattern = "bbwwbbwwbbw";
                        break;
                    case 3:
                        pattern = "bwwbwwbbwww";
                        break;
                    case 4:
                        pattern = "bwwbwwwbbww";
                        break;
                    case 5:
                        pattern = "bwwwbwwbbww";
                        break;
                    case 6:
                        pattern = "bwwbbwwbwww";
                        break;
                    case 7:
                        pattern = "bwwbbwwwbww";
                        break;
                    case 8:
                        pattern = "bwwwbbwwbww";
                        break;
                    case 9:
                        pattern = "bbwwbwwbwww";
                        break;
                    case 10:
                        pattern = "bbwwbwwwbww";
                        break;
                    case 11:
                        pattern = "bbwwwbwwbww";
                        break;
                    case 12:
                        pattern = "bwbbwwbbbww";
                        break;
                    case 13:
                        pattern = "bwwbbwbbbww";
                        break;
                    case 14:
                        pattern = "bwwbbwwbbbw";
                        break;
                    case 15:
                        pattern = "bwbbbwwbbww";
                        break;
                    case 16:
                        pattern = "bwwbbbwbbww";
                        break;
                    case 17:
                        pattern = "bwwbbbwwbbw";
                        break;
                    case 18:
                        pattern = "bbwwbbbwwbw";
                        break;
                    case 19:
                        pattern = "bbwwbwbbbww";
                        break;
                    case 20:
                        pattern = "bbwwbwwbbbw";
                        break;
                    case 21:
                        pattern = "bbwbbbwwbww";
                        break;
                    case 22:
                        pattern = "bbwwbbbwbww";
                        break;
                    case 23:
                        pattern = "bbbwbbwbbbw";
                        break;
                    case 24:
                        pattern = "bbbwbwwbbww";
                        break;
                    case 25:
                        pattern = "bbbwwbwbbww";
                        break;
                    case 26:
                        pattern = "bbbwwbwwbbw";
                        break;
                    case 27:
                        pattern = "bbbwbbwwbww";
                        break;
                    case 28:
                        pattern = "bbbwwbbwbww";
                        break;
                    case 29:
                        pattern = "bbbwwbbwwbw";
                        break;
                    case 30:
                        pattern = "bbwbbwbbwww";
                        break;
                    case 31:
                        pattern = "bbwbbwwwbbw";
                        break;
                    case 32:
                        pattern = "bbwwwbbwbbw";
                        break;
                    case 33:
                        pattern = "bwbwwwbbwww";
                        break;
                    case 34:
                        pattern = "bwwwbwbbwww";
                        break;
                    case 35:
                        pattern = "bwwwbwwwbbw";
                        break;
                    case 36:
                        pattern = "bwbbwwwbwww";
                        break;
                    case 37:
                        pattern = "bwwwbbwbwww";
                        break;
                    case 38:
                        pattern = "bwwwbbwwwbw";
                        break;
                    case 39:
                        pattern = "bbwbwwwbwww";
                        break;
                    case 40:
                        pattern = "bbwwwbwbwww";
                        break;
                    case 41:
                        pattern = "bbwwwbwwwbw";
                        break;
                    case 42:
                        pattern = "bwbbwbbbwww";
                        break;
                    case 43:
                        pattern = "bwbbwwwbbbw";
                        break;
                    case 44:
                        pattern = "bwwwbbwbbbw";
                        break;
                    case 45:
                        pattern = "bwbbbwbbwww";
                        break;
                    case 46:
                        pattern = "bwbbbwwwbbw";
                        break;
                    case 47:
                        pattern = "bwwwbbbwbbw";
                        break;
                    case 48:
                        pattern = "bbbwbbbwbbw";
                        break;
                    case 49:
                        pattern = "bbwbwwwbbbw";
                        break;
                    case 50:
                        pattern = "bbwwwbwbbbw";
                        break;
                    case 51:
                        pattern = "bbwbbbwbwww";
                        break;
                    case 52:
                        pattern = "bbwbbbwwwbw";
                        break;
                    case 53:
                        pattern = "bbwbbbwbbbw";
                        break;
                    case 54:
                        pattern = "bbbwbwbbwww";
                        break;
                    case 55:
                        pattern = "bbbwbwwwbbw";
                        break;
                    case 56:
                        pattern = "bbbwwwbwbbw";
                        break;
                    case 57:
                        pattern = "bbbwbbwbwww";
                        break;
                    case 58:
                        pattern = "bbbwbbwwwbw";
                        break;
                    case 59:
                        pattern = "bbbwwwbbwbw";
                        break;
                    case 60:
                        pattern = "bbbwbbbbwbw";
                        break;
                    case 61:
                        pattern = "bbwwbwwwwbw";
                        break;
                    case 62:
                        pattern = "bbbbwwwbwbw";
                        break;
                    case 63:
                        pattern = "bwbwwbbwwww";
                        break;
                    case 64:
                        pattern = "bwbwwwwbbww";
                        break;
                    case 65:
                        pattern = "bwwbwbbwwww";
                        break;
                    case 66:
                        pattern = "bwwbwwwwbbw";
                        break;
                    case 67:
                        pattern = "bwwwwbwbbww";
                        break;
                    case 68:
                        pattern = "bwwwwbwwbbw";
                        break;
                    case 69:
                        pattern = "bwbbwwbwwww";
                        break;
                    case 70:
                        pattern = "bwbbwwwwbww";
                        break;
                    case 71:
                        pattern = "bwwbbwbwwww";
                        break;
                    case 72:
                        pattern = "bwwbbwwwwbw";
                        break;
                    case 73:
                        pattern = "bwwwwbbwbww";
                        break;
                    case 74:
                        pattern = "bwwwwbbwwbw";
                        break;
                    case 75:
                        pattern = "bbwwwwbwwbw";
                        break;
                    case 76:
                        pattern = "bbwwbwbwwww";
                        break;
                    case 77:
                        pattern = "bbbbwbbbwbw";
                        break;
                    case 78:
                        pattern = "bbwwwwbwbww";
                        break;
                    case 79:
                        pattern = "bwwwbbbbwbw";
                        break;
                    case 80:
                        pattern = "bwbwwbbbbww";
                        break;
                    case 81:
                        pattern = "bwwbwbbbbww";
                        break;
                    case 82:
                        pattern = "bwwbwwbbbbw";
                        break;
                    case 83:
                        pattern = "bwbbbbwwbww";
                        break;
                    case 84:
                        pattern = "bwwbbbbwbww";
                        break;
                    case 85:
                        pattern = "bwwbbbbwwbw";
                        break;
                    case 86:
                        pattern = "bbbbwbwwbww";
                        break;
                    case 87:
                        pattern = "bbbbwwbwbww";
                        break;
                    case 88:
                        pattern = "bbbbwwbwwbw";
                        break;
                    case 89:
                        pattern = "bbwbbwbbbbw";
                        break;
                    case 90:
                        pattern = "bbwbbbbwbbw";
                        break;
                    case 91:
                        pattern = "bbbbwbbwbbw";
                        break;
                    case 92:
                        pattern = "bwbwbbbbwww";
                        break;
                    case 93:
                        pattern = "bwbwwwbbbbw";
                        break;
                    case 94:
                        pattern = "bwwwbwbbbbw";
                        break;
                    case 95:
                        pattern = "bwbbbbwbwww";
                        break;
                    case 96:
                        pattern = "bwbbbbwwwbw";
                        break;
                    case 97:
                        pattern = "bbbbwbwbwww";
                        break;
                    case 98:
                        pattern = "bbbbwbwwwbw";
                        break;
                    case 99:
                        pattern = "bwbbbwbbbbw";
                        break;
                    case 100:
                        pattern = "bwbbbbwbbbw";
                        break;
                    case 101:
                        pattern = "bbbwbwbbbbw";
                        break;
                    case 102:
                        pattern = "bbbbwbwbbbw";
                        break;
                    case 103:
                        pattern = "bbwbwwwwbww";
                        break;
                    case 104:
                        pattern = "bbwbwwbwwww";
                        break;
                    case 105:
                        pattern = "bbwbwwbbbww";
                        break;
                    case 106:
                        pattern = "bbwwwbbbwbwbb";
                        break;
                    default: break;
                }
                output = output + pattern;
            }
            return output;
        }
    }
}
