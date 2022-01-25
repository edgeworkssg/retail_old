using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;

namespace PowerPOS.Nets
{
    /// <summary>
    /// Class contains generic 
    /// </summary>
    class NetsLogger
    {

        private static String logname;
        public static bool writeLog(string E)
        {
            try
            {
                if (logname == "") { logname = "NETS_log"; }
                StreamWriter myLogger;
                String logdir;
                logdir = AppDomain.CurrentDomain.BaseDirectory + "\\log";
                if (!Directory.Exists(logdir))
                {
                    Directory.CreateDirectory(logdir);
                }

                if (File.Exists(logdir + "\\" + logname + DateTime.Today.ToString("_yyyy-MM-dd") + ".log"))
                {
                    myLogger = File.AppendText(logdir + "\\" + logname + DateTime.Today.ToString("_yyyy-MM-dd") + ".log");
                }
                else
                {
                    myLogger = File.CreateText(logdir + "\\" + logname + DateTime.Today.ToString("_yyyy-MM-dd") + ".log");
                }
                myLogger.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " > " + E);
                myLogger.Close();
                return true;
            }
            catch
            {
                //Dont do anything if fail...
                //throw;                  
                return false;
            }
        }
    }
}
