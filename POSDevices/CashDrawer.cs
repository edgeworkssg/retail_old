using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using PowerPOS.Container;
using System.Management;


namespace POSDevices
{

    public class OpenDrawerException : ApplicationException
    {
        // Default constructor
        public OpenDrawerException()
        {
        }
        // Constructor accepting a single string message
        public OpenDrawerException(string message)
            : base(message)
        {
        }
        // Constructor accepting a string message and an
        // inner exception which will be wrapped by this
        // custom exception class
        public OpenDrawerException(string message,
        Exception inner)
            : base(message, inner)
        {
        }
    }
    public class CashDrawerSetup
    {
        public static String PrinterName;
        public static String KickDrawer;

        public static void LoadCashDrawerSetup()
        {
            try
            {
                DataSet ds = new DataSet();
                ds.ReadXml(AppDomain.CurrentDomain.BaseDirectory + "\\POSDevices\\ReceiptPrinter.xml");
                PrinterName = ds.Tables[0].Rows[0].ItemArray[0].ToString();
                KickDrawer = ds.Tables[0].Rows[0].ItemArray[1].ToString();

            }
            catch (Exception E)
            {
                throw new PrintReceiptException("Initialise Cash Drawer failed!", E);
            }
        }

    }
    public class CashDrawer
    {
        public bool isPrinterValid(string _PrinterName)
        {
            ManagementScope scope = new ManagementScope(@"\root\cimv2");
            scope.Connect();
            ManagementObjectSearcher searcher = new
            ManagementObjectSearcher("SELECT * FROM Win32_Printer");
            bool res = false;
            foreach (ManagementObject printer in searcher.Get())
            {
                string printerName = printer["Name"].ToString().ToLower();
                if (printerName.Equals(_PrinterName))
                {
                    if (printer["WorkOffline"].ToString().ToLower().Equals("true"))
                    {
                        res = false;
                    }
                    else
                    {
                        res = true;
                    }
                }
            }
            return res;
        }

        public void OpenDrawer()
        {
            try
            {
                String PrinterName;
                String KickDrawer;
                String KickDrawerCmd = "";
                char c;
                if (CashDrawerSetup.PrinterName == null) 
                    CashDrawerSetup.LoadCashDrawerSetup();
                PrinterName = CashDrawerSetup.PrinterName;
                KickDrawer = CashDrawerSetup.KickDrawer;
                
                //Dont kick if no printer....
                if (PrinterName == "") return;

                //if (!isPrinterValid(PrinterName)) return;



                int i, j;
                i = 0;
                while (i != -1)
                {
                    j = i;
                    i = KickDrawer.IndexOf(";", j);
                    if (i != -1)
                    {
                        //Console.WriteLine((KickDrawer.Substring(j, i - j)));
                        //Console.WriteLine(Int16.Parse((KickDrawer.Substring(j, i - j))));
                        c = (char)Int16.Parse((KickDrawer.Substring(j, i - j)));
                        //Console.WriteLine(c);
                        KickDrawerCmd = KickDrawerCmd + new String(c, 1);
                        Console.WriteLine(KickDrawerCmd);
                        i++;
                    }
                }
                RawPrinterHelper.SendStringToPrinter(PrinterName, KickDrawerCmd);
            }
            catch (Exception e)
            {
                throw new OpenDrawerException("Open drawer failed!", e);
            }
        }
        public void OpenDrawerYifei()
        {           
            try
            {
                DataSet ds = new DataSet();
                String PrinterName;
                String KickDrawer;
                String KickDrawerCmd = "";
                char c;
                if ((ReceiptSizes)Enum.ToObject(typeof(ReceiptSizes), 
                    PrintSettingInfo.receiptSetting.PaperSize.Value) != ReceiptSizes.Receipt)
                {
                    return;
                }

                int i, j;
                ds.ReadXml(AppDomain.CurrentDomain.BaseDirectory + "\\POSDevices\\ReceiptPrinter.xml");
                PrinterName = ds.Tables[0].Rows[0].ItemArray[0].ToString();
                KickDrawer = ds.Tables[0].Rows[0].ItemArray[1].ToString();
                i = 0;
                while (i != -1)
                {
                    j = i;
                    i = KickDrawer.IndexOf(";", j);
                    if (i != -1)
                    {
                        //Console.WriteLine((KickDrawer.Substring(j, i - j)));
                        //Console.WriteLine(Int16.Parse((KickDrawer.Substring(j, i - j))));
                        c = (char)Int16.Parse((KickDrawer.Substring(j, i - j)));
                        //Console.WriteLine(c);
                        KickDrawerCmd = KickDrawerCmd + new String(c, 1);
                        Console.WriteLine(KickDrawerCmd);
                        i++;
                    }
                }
                RawPrinterHelper.SendStringToPrinter(PrinterName, KickDrawerCmd);
            }
            catch (Exception e)
            {
                throw new OpenDrawerException("Open drawer failed!", e);
            }
        }
        public void OpenDrawer(String datafilename)
        {
            try
            {

                DataSet ds = new DataSet();
                String PrinterName;
                String KickDrawer;
                String KickDrawerCmd = "";
                char c;


                int i, j;
                ds.ReadXml(datafilename);
                PrinterName = ds.Tables[0].Rows[0]["PrinterName"].ToString();
                KickDrawer = ds.Tables[0].Rows[0]["KickDrawer"].ToString();
                i = 0;
                while (i != -1)
                {
                    j = i;
                    i = KickDrawer.IndexOf(";", j);
                    if (i != -1)
                    {
                        c = (char)Int16.Parse((KickDrawer.Substring(j, i - j)));
                        KickDrawerCmd = KickDrawerCmd + new String(c, 1);
                        Console.WriteLine(KickDrawerCmd);
                        i++;
                    }
                }
                RawPrinterHelper.SendStringToPrinter(PrinterName, KickDrawerCmd);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public void OpenDrawer(String PrinterName, String KickDrawer)
        {
            try
            {
                RawPrinterHelper.SendStringToPrinter(PrinterName, KickDrawer);
            }
            catch (Exception e)
            {
                throw new OpenDrawerException("Initialise OpenDrawer Failed!", e);
            }
        }

    }
}
