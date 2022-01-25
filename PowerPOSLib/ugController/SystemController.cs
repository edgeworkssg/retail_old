using System.Data.SqlClient;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PowerPOS
{
    /// <summary>
    /// A Class that control the Point of Sales as whole 
    /// </summary>
    public partial class SystemController
    {
        /// <summary>
        /// To check if the system have expiry date, and if the expiry date has been passed
        /// </summary>
        /// <param name="DataServiceProvider">Please input the [DataServiceProvider] value on AppSetting</param>
        /// <returns>False if Input parameter is Null/Empty</returns>
        public static bool isExpired(string DataServiceProvider)
        {
            try
            {
                if (DataServiceProvider == null) return false;
                if (DataServiceProvider == "") return false;

                int month = int.Parse(DataServiceProvider.Substring(0, 2));
                int year = int.Parse(DataServiceProvider.Substring(2, 4));
                int day = int.Parse(DataServiceProvider.Substring(6, 2));

                return (DateTime.Now > new DateTime(year, month, day));
            }
            catch (Exception X)
            {
                Logger.writeLog(X);
                return false;
            }
        }

        /// <summary>
        /// Check if the database stated in Connection String is installed
        /// </summary>
        /// <param name="ConStr">Connection string to be checked</param>
        public static bool isDatabaseInstalled(string ConStr)
        {
            try
            {
                string DBName;
                SqlConnectionStringBuilder aa = new SqlConnectionStringBuilder(ConStr);
                DBName = aa.InitialCatalog;
                aa.InitialCatalog = "master";
                return (new Server(new ServerConnection(new SqlConnection(aa.ConnectionString)))).Databases.Contains(DBName);
            }
            catch (Exception X)
            {
                throw X;
            }
        }
        public static bool isDatabaseInstalled(string ServerName, string DatabaseName)
        {
            try
            {
                return (new Server(ServerName)).Databases.Contains(DatabaseName);
            }
            catch (Exception X)
            {
                throw X;
            }
        }

        public static void CheckDatabaseVersion()
        {

        }

        public static string DatabaseVersion
        {
            get
            {
                AppSetting version = new AppSetting();
                version.LoadVersion();
                if (version == null || version.IsNew)
                    return "0";

                return version.AppSettingValue;
            }
        }

        public static string SystemVersion
        {
            get { return (new Version()).AssemblyVersion; }
        }


        public void CheckFirstTimeUse()
        {
            #region *) Check the First Time Run
            try
            {
                string ConStr = SubSonic.DataService.Provider.DefaultConnectionString;
                bool FirstTimeUse = true;
                FirstTimeUse = FirstTimeUse && !System.Data.SqlCustomTools.IsDatabaseExist(ConStr);

                AppSetting version = new AppSetting();
                version.LoadSetting(AppSetting.SettingsName.Database_Version);

                FirstTimeUse = FirstTimeUse && version == null;

                if (!System.Data.SqlCustomTools.IsDatabaseExist(ConStr))
                {
                    //frmFirstTimeUse inst = new frmFirstTimeUse(ConStr);
                    //if (inst.ShowDialog() != DialogResult.Yes)
                    //{
                    //    MessageBox.Show("Error in doing Configuration for first time use\nPlease contact our support team");
                    //    return;
                    //}
                }
            }
            catch (Exception ex)
            {
                Logger.writeLogToFile(ex);
            }
            #endregion

        }
    }
}
