using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PowerPOS;

namespace ERPIntegration.API
{
    class Helper
    {
        public static void WriteLog(string message, string msgtype)
        {
            Console.WriteLine(message);
            Logger.writeLog(msgtype + ": " + message);

            //string source = FTPSettings.Default.EventLogName;
            //if (string.IsNullOrEmpty(source))
            //{
            //    source = Assembly.GetExecutingAssembly().GetName().Name;
            //}

            //if (!EventLog.SourceExists(source))
            //    EventLog.CreateEventSource(source, "Application");

            //EventLog.WriteEntry(source, message, logType);
            //(new Helper()).WriteLog(message);
        }
    }
}
