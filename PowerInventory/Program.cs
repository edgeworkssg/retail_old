using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using PowerInventory.InventoryForms;
using PowerPOS;
using System.Threading;
using System.Globalization;
using LanguageManager = PowerInventory.Properties.InventoryLanguage;

namespace PowerInventory
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
            //Application.Run(new Support.frmStockReport());
            /*
            if (DateTime.Now < new DateTime(2009, 11, 1))
            {
                Application.Run(new frmMain());
                Application.Run(new frmCheckStockOnHandCorrectness());
            }*/
        }
        public static void LoadCultureCode()
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
                    Application.DoEvents();
                }
                catch (Exception ex)
                {
                    Logger.writeLog("Invalid Culture Code Specified in Setting!" + ex.Message);

                }
            }
            #endregion
        }
    }
}
