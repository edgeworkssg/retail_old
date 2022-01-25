using System;
using System.Collections.Generic;
using System.Text;
using PowerPOS;
using System.Data;
using PowerPOS.Container;

namespace POSDevices
{

    public class PriceDisplayException : ApplicationException
    {
        // Default constructor
        public PriceDisplayException()
        {
        }
        // Constructor accepting a single string message
        public PriceDisplayException(string message)
            : base(message)
        {
        }
        // Constructor accepting a string message and an
        // inner exception which will be wrapped by this
        // custom exception class
        public PriceDisplayException(string message,
        Exception inner)
            : base(message, inner)
        {
        }
    }   

    public class PriceDisplay
    {
        private String PrinterName;
        private String ClearDisplay;
        public String FirstLineCommand;
        public String SecondLineCommand;
        public bool useWindcor;

        public bool ClearScreen()
        {
            return SendCommandToDisplay(ClearDisplay); 
        }

        public bool SendCommandToDisplay(String s)
        {
            
            if (PrinterName != "") return RawPrinterHelper.SendStringToPrinter(PrinterName, s);

            return true;
            
        }

        public bool ShowItemPrice(String Item, double Price, double Total, int width)
        {
            ClearScreen();
            if (useWindcor)
            {
                bool result = false;
                //Logger.writeLog("FirstLine");
                result = SendCommandToDisplay(ConvertToChar(FirstLineCommand));
                result = SendCommandToDisplay(PadRight(Item, width));
                //Logger.writeLog(PadRight(Item, width));

                result = SendCommandToDisplay(ConvertToChar(SecondLineCommand));
                result = SendCommandToDisplay(PadRight(String.Format("{0:N}", Price), 10) + PadLeft("(" + String.Format("{0:N}", Total) + ")", 10));
                return result;
            }
            else
            {

                return SendCommandToDisplay(PadRight(Item, width) +
                    PadRight(String.Format("{0:N}", Price), 10) + PadLeft("(" + String.Format("{0:N}", Total) + ")", 10));
            }

            //return SendCommandToDisplay(PadRight(Item, 20) + PadRight(String.Format("{0:c}", Price), 10) + PadLeft("("+String.Format("{0:c}", Total)+")", 10));           
        }

        public bool ShowSubTotal(string message, double SubTotal, int width)
        {
            ClearScreen();
            if (useWindcor)
            {
                bool result = false;

                result = SendCommandToDisplay(ConvertToChar(FirstLineCommand));
                result = SendCommandToDisplay(PadRight(message, width));

                result = SendCommandToDisplay(ConvertToChar(SecondLineCommand));
                result = SendCommandToDisplay(PadRight("", 10) + PadLeft("(" + String.Format("{0:N}", SubTotal) + ")", 10));
                return result;
            }
            else
            {
                return SendCommandToDisplay(PadRight(message, width) + PadRight("", 10) + PadLeft("(" + String.Format("{0:N}", SubTotal) + ")", 10));
            }
            //return SendCommandToDisplay(PadRight(message, 20) + PadRight("", 10) + PadLeft("(" + String.Format("{0:c}", SubTotal) + ")", 10));
        }

        private String PadLeft(String S, int n)
        {
            String t = S;
            if (t.Length > n)
            {
                t = t.Substring(0, n);
                //throw new PriceDisplayException("PadLeft!" + S + " to " + n + " char too long!");                
            }
            else
            {
                while (t.Length < n)
                {
                    t = " " + t;
                }
            }
            return t;
        }

        private String PadRight(String S, int n)
        {
            String t = S;
            if (t.Length > n)
            {
                t = S.Substring(0, n);
                //throw new PriceDisplayException("PadRight!" + S + " to " + n + " char too long!");
            }
            else
            {
                while (t.Length < n)
                {
                    t = t + " ";
                }
            }
            return t;
        }

        public bool NextCustomer()
        {
            return SendCommandToDisplay(PadRight("Next Customer Pls", 40));     
        }

        public bool CounterClose()
        {
            ClearScreen();
            return SendCommandToDisplay(PadRight("Counter Closed", 40));           
        }

        public bool ShowTotal(double Total)
        {

            ClearScreen();
            if (useWindcor)
            {
                bool result = false;

                result = SendCommandToDisplay(ConvertToChar(FirstLineCommand));
                result = SendCommandToDisplay(PadRight("Total Amt Payable:", 20));

                result = SendCommandToDisplay(ConvertToChar(SecondLineCommand));
                result = SendCommandToDisplay(PadRight(String.Format("{0:N}", Total), 20));
                return result;
            }
            else
            {
                return SendCommandToDisplay(PadRight("Total Amt Payable:", 20) + PadRight(String.Format("{0:N}", Total), 20));
            }
        }

        public bool ShowChange(double Change)
        {
            ClearScreen();
            if (useWindcor)
            {
                bool result = false;

                result = SendCommandToDisplay(ConvertToChar(FirstLineCommand));
                result = SendCommandToDisplay(PadRight("Change:", 20));

                result = SendCommandToDisplay(ConvertToChar(SecondLineCommand));
                result = SendCommandToDisplay(PadRight(String.Format("{0:N}", Change), 20));
                return result;
            }
            else
            {
                return SendCommandToDisplay(PadRight("Change:", 20) + PadRight(String.Format("{0:N}", Change), 20));
            }
        }

        public static string ConvertToChar(string s)
        {
            String C;
            int i, j;
            char c;
            string TempC = "";
            C = s;
            i = 0;
            while (i != -1)
            {
                j = i;
                i = C.IndexOf(";", j);
                if (i != -1)
                {
                    c = (char)Int16.Parse((C.Substring(j, i - j)));
                    TempC = TempC + new String(c, 1);
                    i++;
                }
            }
            return TempC;
        }

        public PriceDisplay()
        {
            try
            {
                DataSet ds = new DataSet();
                String C, TempC;
                int i, j;
                char c;

                ds.ReadXml(AppDomain.CurrentDomain.BaseDirectory + "\\POSDevices\\PriceDisplay.xml");

                PrinterName = ds.Tables[0].Rows[0]["PrinterName"].ToString();
                C = ds.Tables[0].Rows[0]["ClearScreen"].ToString();
                TempC = "";
                i = 0;
                while (i != -1)
                {
                    j = i;
                    i = C.IndexOf(";", j);
                    if (i != -1)
                    {                        
                        c = (char)Int16.Parse((C.Substring(j, i - j)));                     
                        TempC = TempC + new String(c, 1);
                        i++;
                    }
                }
                ClearDisplay = C + TempC;
                
            }
            catch (Exception E) 
            {
                //throw new PriceDisplayException("Initialise Price Display failed!", E);
                PowerPOS.Logger.writeLog(E);
            }
        }

        public PriceDisplay(string PName, string clearscreencode)
        {
            try
            {
                DataSet ds = new DataSet();
                String C, TempC;
                int i, j;
                char c;

                //ds.ReadXml(AppDomain.CurrentDomain.BaseDirectory + "\\POSDevices\\PriceDisplay.xml");

                PrinterName = PName;
                C = clearscreencode;
                TempC = "";
                i = 0;
                while (i != -1)
                {
                    j = i;
                    i = C.IndexOf(";", j);
                    if (i != -1)
                    {
                        c = (char)Int16.Parse((C.Substring(j, i - j)));
                        TempC = TempC + new String(c, 1);
                        i++;
                    }
                }
                ClearDisplay = C + TempC;

            }
            catch (Exception E)
            {
                //throw new PriceDisplayException("Initialise Price Display failed!", E);
                PowerPOS.Logger.writeLog(E);
            }
        }

        public void setPrinterName(string PName)
        {
            PrinterName = PName;
        }
    }

}
