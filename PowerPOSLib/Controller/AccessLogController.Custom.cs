using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic;
using System.Data;
using System.IO;

namespace PowerPOS
{
    public enum AccessSource { POS, WEB, WEBServices }
    public partial class AccessLogController
    {
        public const string XMLFILENAME = "\\WS.XML";
        public static string WS_URL = "http://localhost:51831/PowerPOSWeb/Synchronization/Synchronization.asmx";

        public static bool Load_WS_URL()
        {
            try
            {
                DataSet ds = new DataSet();
                ds.ReadXml(AppDomain.CurrentDomain.BaseDirectory + XMLFILENAME);
                WS_URL = ds.Tables[0].Rows[0]["URL"].ToString();
                return true;
            }
            catch (Exception ex) //system exception
            {
                Logger.writeLog("Load_WS_URL");
                Logger.writeLog(ex);
                return false;
            }
        }

        public static void AddLog(AccessSource src, string userName, string secondUserName, string accessTpe, string remarks)
        {
            /*try
            {
                var theSetting = new SettingController().FetchAll().FirstOrDefault();
                int posID = 0;
                if (theSetting != null)
                    posID = theSetting.PointOfSaleID;

                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.SyncAccessLog), false))
                {
                    AddLogHelper(posID, src, userName, secondUserName, accessTpe, remarks);
                }
                else
                {
                    if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + XMLFILENAME))
                    {
                        try
                        {
                            Load_WS_URL();
                            PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                            ws.Timeout = 100000;
                            ws.Url = WS_URL;
                            PowerPOSLib.PowerPOSSync.AccessSource theSource = PowerPOSLib.PowerPOSSync.AccessSource.POS;
                            if (src == AccessSource.WEB)
                                theSource = PowerPOSLib.PowerPOSSync.AccessSource.WEB;
                            else if (src == AccessSource.WEBServices)
                                theSource = PowerPOSLib.PowerPOSSync.AccessSource.WEBServices;
                            else if (src == AccessSource.POS)
                                theSource = PowerPOSLib.PowerPOSSync.AccessSource.POS;
                            if (theSetting != null)
                                posID = theSetting.PointOfSaleID;
                            
                            ws.AddAccessLog(posID, theSource, userName, secondUserName, accessTpe, remarks);
                        }
                        catch (Exception ex)
                        {
                            Logger.writeLog(ex);
                        }
                    }
                    else
                    {
                        AddLogHelper(posID, src, userName, secondUserName, accessTpe, remarks);
                    }
                }
            }
            catch (Exception ex2)
            {
                Logger.writeLog(ex2);
            }*/
        }

        public static void AddAccessLog(AccessSource src, string userName, string secondUserName, string accessTpe, string remarks)
        {
            try
            {
                var theSetting = new SettingController().FetchAll().FirstOrDefault();
                int posID = 0;
                if (theSetting != null)
                    posID = theSetting.PointOfSaleID;

                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.SyncAccessLog), false))
                {
                    AddLogHelper(posID, src, userName, secondUserName, accessTpe, remarks);
                }
                else
                {
                    if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + XMLFILENAME))
                    {
                        try
                        {
                            Load_WS_URL();
                            PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                            ws.Timeout = 100000;
                            ws.Url = WS_URL;
                            PowerPOSLib.PowerPOSSync.AccessSource theSource = PowerPOSLib.PowerPOSSync.AccessSource.POS;
                            if (src == AccessSource.WEB)
                                theSource = PowerPOSLib.PowerPOSSync.AccessSource.WEB;
                            else if (src == AccessSource.WEBServices)
                                theSource = PowerPOSLib.PowerPOSSync.AccessSource.WEBServices;
                            else if (src == AccessSource.POS)
                                theSource = PowerPOSLib.PowerPOSSync.AccessSource.POS;
                            if (theSetting != null)
                                posID = theSetting.PointOfSaleID;

                            ws.AddAccessLog(posID, theSource, userName, secondUserName, accessTpe, remarks);
                        }
                        catch (Exception ex)
                        {
                            Logger.writeLog(ex);
                        }
                    }
                    else
                    {
                        AddLogHelper(posID, src, userName, secondUserName, accessTpe, remarks);
                    }
                }
            }
            catch (Exception ex2)
            {
                Logger.writeLog(ex2);
            }
        }

        public static void AddLogHelper(int pointOfSaleID, AccessSource src, string userName, string secondUserName, string accessTpe, string remarks)
        {
            try
            {
                AccessLog al = new AccessLog();
                al.AccessLogID = Guid.NewGuid();
                al.PointOfSaleID = pointOfSaleID;
                al.Deleted = false;
                al.AccessDate = DateTime.Now;
                al.LoginName = userName;
                al.SecondLoginName = secondUserName;
                al.AccessType = accessTpe;
                al.AccessSource = src.ToString();
                al.Remarks = remarks;
                var qc = al.GetInsertCommand(userName);
                DataService.ExecuteQuery(qc);
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        public static void AddTenantHistory(string tenantCode, int pointOfSaleID, 
            AccessSource src, string userName, string logValue)
        {
            try
            {
                AccessLog al = new AccessLog();
                al.AccessLogID = Guid.NewGuid();
                al.TenantCode = tenantCode;
                al.PointOfSaleID = pointOfSaleID;
                al.AccessSource = src.ToString();
                al.LoginName = userName;
                al.AccessType = logValue;
                al.AccessDate = DateTime.Now;
                al.SecondLoginName = "-";
                al.Remarks = "Tenant History";
                al.Deleted = false;
                var qc = al.GetInsertCommand(userName);
                DataService.ExecuteQuery(qc);
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }
    }
}
