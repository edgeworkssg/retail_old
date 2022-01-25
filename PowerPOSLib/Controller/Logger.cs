using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using PowerPOS.Container;
using SubSonic;

namespace PowerPOS
{
    public class Logger
    {
        private static String logname;
        public static bool writeLogToFile(Exception E)
        {
            try
            {
                if (logname == "") { logname = "log"; }
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
                myLogger.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " > " + E.ToString() + "Inner Exception:" + E.InnerException);
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
        public String getlogname()
        {
            return logname;
        }

        public void setlogname(String LogName)
        {
            logname = LogName;
        }
        public static bool writeLog(Exception E)
        {
            try
            {
                //write to DB first
                //SPs.WriteLog(E.ToString() + "Innr:" + E.InnerException).Execute();
                PowerLog pw = new PowerLog();
                pw.LogMsg = E.ToString() + "Innr:" + E.InnerException;
                pw.LogDate = DateTime.Now;
                pw.Save("SYSTEM");
                        
                return true;
            }
            catch (Exception ex)
            {
                //if DB failed, write to text file
                writeLogToFile(ex);
                return false;                
            }
            
        }

        public static bool writeLog(String msg)
        {
            try
            {
                //write to DB first
                //SPs.WriteLog(msg).Execute();
                
                PowerLog pw = new PowerLog();
                pw.LogMsg = msg;
                pw.LogDate = DateTime.Now;
                pw.Save("SYSTEM");
                

                return true;
            }
            catch (Exception ex)
            {
                //if DB failed, write to text file
                writeLogToFile(ex);
                return false;
            }
        }

        public static bool writeLog(String msg, bool IsWarning)
        {
            try
            {
                //write to DB first
                //SPs.WriteLog(msg).Execute();

                PowerLog pw = new PowerLog();
                pw.LogMsg = msg;
                pw.LogDate = DateTime.Now;
                pw.Save("SYSTEM");

                if (IsWarning)
                {
                    WarningMsg w = new WarningMsg();
                    w.UniqueID = Guid.NewGuid();
                    w.WarningMessage = msg;
                    w.PointOfSaleID = PointOfSaleInfo.PointOfSaleID;
                    w.Save("SYSTEM");
                }

                return true;
            }
            catch (Exception ex)
            {
                //if DB failed, write to text file
                writeLogToFile(ex);
                return false;
            }
        }


        /*public static bool writeLogToFile(Exception E)
        {
            try
            {
                if (logname == "") { logname = "log"; }
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
                myLogger.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " > " + E.ToString() + "Inner Exception:"+ E.InnerException);
                myLogger.Close();
                return true;
            }
            catch
            {
                //Dont do anything if fail...
                //throw;                  
                return false;
            }
        }*/
        public static bool writeLogToFile(string E)
        {
            try
            {
                if (logname == "") { logname = "log"; }
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

        public static bool writeStaffAssistLog(string type, string message)
        {
            try
            {
                QueryCommand cmd = new QueryCommand("INSERT INTO StaffAssistLog (LogDate, Type, Msg, PointOfSaleID) VALUES (@LogDate, @Type, @Msg, @PointOfSaleID)");
                cmd.AddParameter("LogDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                cmd.AddParameter("Type", type);
                cmd.AddParameter("Msg", message);
                cmd.AddParameter("PointOfSaleID", PointOfSaleInfo.PointOfSaleID);

                DataService.ExecuteQuery(cmd);
            }
            catch (Exception ex)
            {
                writeLog(ex);
            }

            return true;
        }

        public Logger()
        {
            logname = "log"; // default value
        }
    }
}
