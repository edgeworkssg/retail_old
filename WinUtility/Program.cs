using System;
using System.Collections.Generic;
using System.Windows.Forms;
using PowerPOS;
using System.Threading;
using System.Globalization;
using PowerPOS.Container;

namespace WinUtility
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //DecryptDefaultConnString();
            LoadCultureCode();
            DoLogin();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmTestGrid());
        }

        private static void LoadCultureCode()
        {
            #region Load Culture Code
            string CultureCode = AppSetting.GetSetting(AppSetting.SettingsName.CultureCode);

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
                    //MessageBox.Show(LanguageManager.Invalid_Culture_Code_Specified_in_Setting_ + ex.Message);
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

        private static void DoLogin()
        {
            string status, role, DeptID;
            UserMst user;

            PointOfSaleController.GetPointOfSaleInfo();
            if (UserController.login("edgeworks", "pressingon", LoginType.Login, out user, out role, out DeptID, out status))
            {

                PointOfSale pos = new PointOfSale(PointOfSale.Columns.PointOfSaleID, PointOfSaleInfo.PointOfSaleID);
                UserInfo.username = user.UserName;
                UserInfo.role = role;
                UserInfo.displayName = user.DisplayName;
                UserInfo.SalesPersonGroupID = user.SalesPersonGroupID;
                LanguageInfo.LangCode = "en-US";
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(LanguageInfo.LangCode);
                AccessLogController.AddLog(AccessSource.POS, UserInfo.username, "-", "Login", "");
                UserInfo.privileges = UserController.FetchGroupPrivilegesWithUsername(UserInfo.role, UserInfo.username);
            }
        }
    }
}