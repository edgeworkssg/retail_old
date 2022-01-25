using System.Threading;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using WinPowerPOS.LoginForms;
using PowerPOS;
using System.Configuration;
using System.Diagnostics;
using LanguageManager = WinPowerPOS.Properties.Language;
using System.Globalization;
using System.Reflection;
using WinPowerPOS.KioskForms;

namespace WinPowerPOS
{
    static class Program
    {
        private static bool getPrevInstance()
        {
            //get the name of current process, i,e the process 
            //name of this current application

            string currPrsName = Process.GetCurrentProcess().ProcessName;

            //Get the name of all processes having the 
            //same name as this process name 
            Process[] allProcessWithThisName
                         = Process.GetProcessesByName(currPrsName);

            //if more than one process is running return true.
            //which means already previous instance of the application 
            //is running
            if (allProcessWithThisName.Length > 1)
            {
                MessageBox.Show("Only one instance of this application is allowed to run. Application Terminated.");
                return true; // Yes Previous Instance Exist
            }
            else
            {
                return false; //No Prev Instance Running
            }
        }
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                #region *) Set Default Icon
                try
                {
                    typeof(Form).GetField("defaultIcon",
                            BindingFlags.NonPublic | BindingFlags.Static)
                        .SetValue(null, Properties.Resources.start_icon);
                }
                catch (Exception ex)
                {
                    Logger.writeLog(ex);
                }
                #endregion

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

               


                

                if (args.Length == 0)
                {
                    DecryptDefaultConnString();
                    LoadCultureCode();
                    
                    #region *) Check the First Time Run
                    try
                    {
                        string ConStr = SubSonic.DataService.Provider.DefaultConnectionString;
                        if (!System.Data.SqlCustomTools.IsDatabaseExist(ConStr))
                        {
                            
                            MessageBox.Show("Database error. Please contact our support team");
                            return;

                            //frmFirstTimeUse inst = new frmFirstTimeUse(ConStr);
                            //if (inst.ShowDialog() != DialogResult.Yes)
                            //{
                            //    MessageBox.Show("Error in doing configuration for first time use\nPlease contact our support team");
                            //    return;
                            //}
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.writeLogToFile(ex);
                    }
                    #endregion

                    if (!getPrevInstance())
                    {
                        Application.Run(new frmLoadingScreen());
                        //PointOfSaleController.GetPointOfSaleInfo();   
                        Application.Run(new frmMain());
                        //Application.Run(new Attendance.frmAttendance());
                    }
                }
                else
                {
                    PointOfSaleController.GetPointOfSaleInfo();
                    if (args.Length == 1 && args[0].ToLower() == "sync")
                    {
                        SyncClientController.Load_WS_URL();
                        Application.Run(new frmSync());
                    }
                    else if (args.Length == 1 && args[0].ToLower() == "selfservicekiosk")
                    {
                        DecryptDefaultConnString();
                        LoadCultureCode();

                        #region *) Check the First Time Run
                        try
                        {
                            string ConStr = SubSonic.DataService.Provider.DefaultConnectionString;
                            if (!System.Data.SqlCustomTools.IsDatabaseExist(ConStr))
                            {

                                MessageBox.Show("Database error. Please contact our support team");
                                return;

                                //frmFirstTimeUse inst = new frmFirstTimeUse(ConStr);
                                //if (inst.ShowDialog() != DialogResult.Yes)
                                //{
                                //    MessageBox.Show("Error in doing configuration for first time use\nPlease contact our support team");
                                //    return;
                                //}
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.writeLogToFile(ex);
                        }
                        #endregion

                        if (!getPrevInstance())
                        {
                            Application.Run(new frmLoadingScreen());

                            Application.Run(new frmStartKiosk());
                        }
                    }
                    //else if (args.Length == 1 && args[0].ToLower() == "admin")
                    //{
                    //    DecryptDefaultConnString();
                    //    LoadCultureCode();
                    //    Application.Run(new frmMainAdmin());
                    //}
                }
            }
            catch (Exception X)
            {
                Logger.writeLog("Program Crashed, information on the next line.");
                Logger.writeLog(X);
            }
        }

        private static void LoadCultureCode()
        {
            #region Load Culture Code
            string CultureCode =
                  AppSetting.GetSetting(AppSetting.SettingsName.CultureCode);

            if (CultureCode != null)
            {
                try
                {
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(CultureCode);
                    //Thread.CurrentThread.CurrentCulture = new CultureInfo(CultureCode);
                }
                catch (Exception ex)
                {
                    Logger.writeLog("Invalid Culture Code Specified in Setting!" + ex.Message);
                    MessageBox.Show(LanguageManager.Invalid_Culture_Code_Specified_in_Setting_ + ex.Message);

                }
            }
            #endregion
        }

        private static void DecryptDefaultConnString()
        {
            try
            {
                string[] splitDataSource = SubSonic.DataService.Provider.DefaultConnectionString.Split(';');
                string splitPassword = string.Empty;
                foreach (string item in splitDataSource)
                {
                    if (item.ToLower().Contains("password="))
                    {
                        splitPassword = item.Replace("password=", string.Empty);
                    }
                }
                 string decryptedPassword = "";
                if (splitPassword != "")
                {
                    decryptedPassword = EncryptionLib.ChiperHelper.TripleDESEncryption.Decrypt(splitPassword, Properties.Settings.Default.SensitiveSalt);
                }
                int index = 0;
                foreach (string item in splitDataSource)
                {
                    if (item.ToLower().Contains("password="))
                    {
                        splitDataSource[index] = string.Format("password={0}", decryptedPassword);
                    }
                    index++;
                }
                //splitDataSource[splitDataSource.Length - 1] = string.Format("{0}{1}", splitDataSource[splitDataSource.Length - 1].Substring(0, 9), decryptedPassword);
                SubSonic.DataService.Provider.DefaultConnectionString = string.Join(";", splitDataSource);
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex.Message);
            }
        }
        private static void DeleteOldDatabaseBackUp() //created by Graham P. 3.5.2012
        {
            try
            {
                int months = 0;
                string path;
                try
                {
                    months = int.Parse(ConfigurationManager.AppSettings["BackUpPeriod"]);
                    path = ConfigurationManager.AppSettings["BackUpDirectory"];
                }
                catch
                {
                    return;
                    //path = Environment.CurrentDirectory + "\\Backup\\";
                    //months = 3; //default
                }

                string[] filePaths = System.IO.Directory.GetFiles(path);
                foreach (string file in filePaths)
                {
                    if (System.IO.File.GetLastWriteTime(file) <= DateTime.Now.AddMonths(-months))
                    {
                        System.IO.File.Delete(file);
                    }

                }
            }
            catch
            {
            }
        }
    }
}