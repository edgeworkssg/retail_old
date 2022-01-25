using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using PowerPOS.Container;
using PowerPOS;
using SubSonic;
using System.ComponentModel;
using System.IO;
using PowerPOS.Controller;
using System.Configuration;
using System.Reflection;
using Ionic.Zip;
using System.Diagnostics;

namespace PowerPOS
{
    public partial class DbUtilityController
    {
        //public static string DBNAME = "Coffeeshop";
        public const int DELETE_N_MONTHS_OLDER_DATA = 1;
        /*
        public const string XMLFILENAME = "Config.xml";
        public static void setDBName(string name)
        {
            DBNAME = name;
        }

        public bool LoadDBName()
        {
            //open XML file and read the value.... Use the slow XML...
            try
            {
                DataSet ds = new DataSet();
                ds.ReadXml(AppDomain.CurrentDomain.BaseDirectory + XMLFILENAME);
                DBNAME = ds.Tables[0].Rows[0]["DBNAME"].ToString();
                return true;
            }
            catch (Exception ex) //system exception
            {
                Logger.writeLog("LoadDBName");
                Logger.writeLog(ex);
                return false;
            }
        }
        */

        public static bool ClearSalesAndInventory()
        {
            try
            {
                string SQL = "delete from CashRecording; " +
                                "delete from InstallmentDetail; " +
                                "delete from Installment; " +
                                "delete from DeliveryOrderDetails; " +
                                "delete from DeliveryOrder; " +
                                "delete from MembershipAttendance; " +
                                "delete from StockTake; " +
                                "delete from inventoryhdr; " +
                                "delete from CounterCloseLog; " +
                                "delete from OrderHdr; ";

                QueryCommand cmd = new QueryCommand(SQL, "PowerPOS");
                DataService.ExecuteQuery(cmd);
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return false;
            }
        }

        public static bool ClearAllItemAndMembership()
        {
            try
            {
                string SQL = "delete from QuickAccessButton; " +
                                "delete from QuickAccessGroupMap; " +
                                "delete from quickaccesscategory; " +
                                "delete from QuickAccessButton; " +
                                "delete from ItemQuantityTrigger; " +
                                "delete from CashRecording; " +
                                "delete from ItemGroupMap; " +
                                "delete from ItemGroup; " +
                                "delete from PromoMembershipMap; " +
                                "delete from PurchaseOrderDetail; " +
                                "delete from PromoCampaignDet; " +
                                "delete from PromoCampaignHdr; " +
                                "delete from DeliveryOrderDetails; " +
                                "delete from DeliveryOrder; " +
                                "delete from InstallmentDetail; " +
                                "delete from Installment; " +
                                "delete from MembershipAttendance; " +
                                "delete from StockTake; " +
                                "delete from inventoryhdr; " +
                                "delete from CounterCloseLog; " +
                                "delete from OrderHdr; " +
                                "delete from Membership where not MembershipNo = 'WALK-IN' " +
                                "delete from item where not CategoryName = 'SYSTEM'; " +
                                "delete from Category where not CategoryName = 'System'; " +
                                "delete from itemDepartment where not ItemDepartmentID = 'System'; ";
                QueryCommand cmd = new QueryCommand(SQL, "PowerPOS");
                DataService.ExecuteQuery(cmd);
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return false;
            }
        }


        public static void SetIsStockOutRunningFalse()
        {
            try
            {
                #region *) set setting is stock out running to false

                if (AppSetting.CastBool(AppSetting.GetSetting("IsStockOutRunning"), false))
                    AppSetting.SetSetting("IsStockOutRunning", "False");
                
                #endregion
            }
            catch (Exception ex2)
            {
                Logger.writeLog(">>> Error Update setting ISSTOCKOUTRUNNING DB");
                Logger.writeLog(ex2);
            }
        }

        public static void UpdateDBStructure()
        {
            try
            {
                Logger.writeLog(">>> Start Upgrading DB");
                string sqlDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase) + "\\Update Database\\DBSchemaUpdate";
                string archiveDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase) + "\\Update Database\\Archive";

                sqlDirectory = sqlDirectory.Replace("file:\\", "");
                archiveDirectory = archiveDirectory.Replace("file:\\", "");
                if (Directory.Exists(sqlDirectory))
                {
                    string[] filePaths = Directory.GetFiles(sqlDirectory, "*.sql");
                    foreach (var theSql in filePaths)
                    {
                        try
                        {
                            if (File.Exists(theSql))
                            {
                                string sql = File.ReadAllText(theSql);
                                QueryCommand cmd = new QueryCommand(sql, "PowerPOS");
                                DataService.ExecuteQuery(cmd);
                                Logger.writeLog(">> DB Upgrade Success : " + theSql);

                                if (!Directory.Exists(archiveDirectory))
                                    Directory.CreateDirectory(archiveDirectory);
                                string newPath = archiveDirectory + "\\" + Path.GetFileName(theSql);
                                
                                if (File.Exists(newPath))
                                    File.Delete(newPath);
                                
                                File.Move(theSql, newPath);

                                if (File.Exists(theSql))
                                    File.Delete(theSql);
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.writeLog(">> DB Upgrade Failed : " + theSql);
                            Logger.writeLog(ex);
                        }
                    }
                }
                Logger.writeLog(">>> Finish Upgrading DB");
            }
            catch (Exception ex2)
            {
                Logger.writeLog(">>> Error Upgrading DB");
                Logger.writeLog(ex2);
            }
        }

        public static void DoDBBackUp(string path, string BackupReason)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            try
            {
                string existingConnectionString = DataService.GetInstance("POWERPOS").DefaultConnectionString;
                System.Data.SqlClient.SqlConnection tmp = new System.Data.SqlClient.SqlConnection(existingConnectionString);

                //SPs.Backupdb(tmp.Database, path).Execute();
                string SQL = "declare  @file nvarchar(200); " +
                                "declare @backupName nvarchar(300); " +
                                "SELECT '" + path + "AutoBackup - ' + CompanyName + ' - ' + ISNULL(PointOfSaleName, '') + ' - ' + convert(varchar(4), datepart(yyyy,getdate())) + convert(varchar(2), datepart(MM,getdate())) + convert(varchar(2), datepart(dd,getdate())) + convert(varchar(2), datepart(HH,getdate())) + convert(varchar(2), datepart(mm,getdate())) + convert(varchar(2), datepart(ss,getdate())) + ' - " + BackupReason + ".bak' " +
                                "FROM Company, Setting LEFT OUTER JOIN PointOfSale ON Setting.PointOfSaleID = PointOfSale.PointOfSaleID ";
                //"set @file = '" + path + tmp.Database + "' + convert(varchar(4), datepart(yyyy,getdate())) + convert(varchar(2), datepart(MM,getdate())) + convert(varchar(2), datepart(dd,getdate())) + convert(varchar(2), datepart(HH,getdate())) + convert(varchar(2), datepart(mm,getdate())) + convert(varchar(2), datepart(ss,getdate())) + '.bak'; " +

                object FileName = DataService.ExecuteScalar(new QueryCommand(SQL, "PowerPOS"));

                SQL = string.Format("declare  @file nvarchar(300); declare  @backupName nvarchar(100); SET @file= '{0}'", FileName.ToString()) +
                    "set @backupName = '" + tmp.Database + "' + N'-Full Database Backup'; " +
                    "BACKUP DATABASE " + tmp.Database + " TO  DISK = @file WITH NOFORMAT, INIT,  NAME = @backupName, SKIP, NOREWIND, NOUNLOAD,  STATS = 10 ";

                DataService.ExecuteQuery(new QueryCommand(SQL, "PowerPOS"));

                using (ZipFile zip = new ZipFile())
                {
                    zip.AddFile(FileName.ToString());
                    zip.Save(FileName.ToString().Replace(".bak", ".zip"));
                }
                File.Delete(FileName.ToString());
            }
            catch (Exception ex)
            {
                Logger.writeLog("Unable to backup data." + ex.Message, true);
            }
        }

        public static void DeleteOldData(int howManyMonthsBehind)
        {
            string[,] tables = { { "OrderSubTotal", "CreatedOn" },
                                 { "OrderDetAttributes", "CreatedOn" },
                                 { "OrderHdr", "OrderDate" }, 
                                 { "InventoryHdr", "InventoryDate" }, 
                                 { "PurchaseOrderHeader", "PurchaseOrderDate" }, 
                                 { "CounterCloseLog", "EndTime" }, 
                                 { "CashRecording", "CashRecordingTime" }, 
                                 { "StockTake", "StockTakeDate" },
                                 { "LoginActivity", "LoginDateTime" },
                                 { "SpecialActivityLog", "ActivityTime" },
                                 { "VoidLog", "CreatedOn" },
                                 { "EZLinkUnCfmLog", "OrderDate" },
                                 { "MeterReading", "ReadingDate" },
                                 { "RentalCollection", "CollectionDate" },
                                 { "PowerLog", "LogDate" },
                                 { "EZLinkMsgLog", "msgDate" },
                                 { "WarningMsg", "CreatedOn" }
                                  };

            DateTime CutOffDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            if (howManyMonthsBehind > 0)
                howManyMonthsBehind = -howManyMonthsBehind;

            if (howManyMonthsBehind != 0)
            {
                CutOffDate = CutOffDate.AddMonths(howManyMonthsBehind);
            }


            for (int i = 0; i < tables.Length / 2; i++)
            {
                Query qr = new Query(tables[i, 0]);
                qr.QueryType = QueryType.Delete;
                if (howManyMonthsBehind != 0)
                {
                    qr.AddWhere(tables[i, 1], Comparison.LessOrEquals, CutOffDate);
                }
                qr.CommandTimeout = 500000;
                qr.Execute();
            }
        }

        public static void CleanUpAllTablesForMultiLocationClient()
        {
            string[] tables = 
            {
                "LoginActivity","Camera","InventoryStockOutReason","Stall","SyncRequest",
                "PointOfSale","Outlet","InventoryLocation",                                                
                "PromoCampaignDet","PromoCampaignHdr","PromoDiscountTier",                
                "AlternateBarcode","EventItemMap","EventLocationMap",
                "SpecialEvent","ItemGroupMap","ItemGroup","ItemAttributesMap","ItemSetMealMap",
                "Item","Category","GST","Bank",
                "RentalPaymentType","MeterReadingCategory","GroupUserPrivilege",
                "UserPrivilege","UserMst","UserGroup","Department"};

            for (int i = 0; i < tables.Length; i++)
            {
                Query qr = new Query(tables[i]);
                qr.QueryType = QueryType.Delete;
                qr.CommandTimeout = 500000;
                qr.Execute();
                Logger.writeLog(tables[i] + " completely deleted");
            }

        }

        public static void UpdateSettingsToVersion2_0()
        {
            string FileName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase) + "\\Update Database\\Version 2.0\\Run Update.bat";
            File.Exists(FileName);
            System.Diagnostics.Process.Start(FileName);
        }

        public static string CreateNewGenericHdrRefNo(string TableName, string KeyColumnName, int PointOfSaleID)
        {
            int runningNo = 0;

            IDataReader ds = PowerPOS.SPs.GetNewGenericHdrNoByPointOfSaleID(TableName, KeyColumnName,
                PointOfSaleID.ToString()).GetReader();
            while (ds.Read())
            {
                if (ds.GetValue(0) != null)
                {
                    if (!int.TryParse(ds.GetValue(0).ToString(), out runningNo))
                    {
                        runningNo = 0;
                    }
                }
                else
                {
                    runningNo = 0;
                }
            }
            ds.Close();
            runningNo += 1;

            //YYMMDDSSSSNNNN                
            //YY - year
            //MM - month
            //DD - day
            //SSSS - PointOfSale ID
            //NNNN - Running No
            return DateTime.Now.ToString("yyMMdd") +
                PointOfSaleID.ToString().PadLeft(4, '0') +
                runningNo.ToString().PadLeft(4, '0');
        }

    }
}