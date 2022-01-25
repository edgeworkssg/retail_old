using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic;

namespace PowerPOS
{
    public class AppointmentSync
    {
        public const string XMLFILENAME = "\\WS.XML";
        public static string WS_URL;

        public static bool Load_WS_URL()
        {
            try
            {
                //if it does not exist in database, load from text file
                //this is for backward compatibility 
                if (AppSetting.GetSetting("WS_URL") != null)
                {
                    WS_URL = AppSetting.GetSetting("WS_URL").ToString();
                    return true;
                }
                else
                {
                    DataSet ds = new DataSet();
                    ds.ReadXml(AppDomain.CurrentDomain.BaseDirectory + XMLFILENAME);
                    WS_URL = ds.Tables[0].Rows[0]["URL"].ToString();
                    return true;
                }
            }
            catch (Exception ex) //system exception
            {
                Logger.writeLog("Load_WS_URL");
                Logger.writeLog(ex);
                return false;
            }
        }

        public static bool SyncAppointment(DateTime startDate, DateTime endDate)
        {
            startDate = startDate.AddDays(-2);
            endDate = endDate.AddDays(2);
            bool isSuccess = false;
            string status = "";
            isSuccess = UploadAppointment(startDate, endDate, out status);
            if(isSuccess)
                isSuccess = SyncClientController.GetAppointments(false);

            return isSuccess;
        }

        public static bool UploadAppointment(DateTime startDate, DateTime endDate, out string status)
        {
            bool isSuccess = false;
            status = "";

            try
            {
                Load_WS_URL();
                PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                ws.Timeout = 100000;
                ws.Url = WS_URL;

                string query = "SELECT * FROM Appointment WHERE CAST(StartTime AS DATE) BETWEEN CAST('{0}' AS DATE) AND CAST('{1}' AS DATE)";
                query = string.Format(query, startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"));
                DataTable dtHdr = new DataTable();
                dtHdr.Load(DataService.GetReader(new QueryCommand(query)));

                query = @"SELECT  AI.* 
                            FROM	Appointment AP
		                            INNER JOIN AppointmentItem AI ON AP.Id = AI.AppointmentId
                            WHERE CAST(AP.StartTime AS DATE) BETWEEN CAST('{0}' AS DATE) AND CAST('{1}' AS DATE)";
                query = string.Format(query, startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"));
                DataTable dtDet = new DataTable();
                dtDet.Load(DataService.GetReader(new QueryCommand(query)));

                dtHdr.TableName = "Appointment";
                dtDet.TableName = "AppointmentItem";

                isSuccess = ws.UploadTables(dtHdr, "Id", "Id", false, out status);
                if (isSuccess)
                    isSuccess = ws.UploadTables(dtDet, "Id", "Id", false, out status);
                else
                    throw new Exception(status);
                if (!isSuccess)
                    throw new Exception(status);
            }
            catch (Exception ex)
            {
                isSuccess = false;
                status = "ERROR SYNC APPOINTMENT " + ex.Message;
                Logger.writeLog(ex);
            }

            return isSuccess;
        }

        public static bool UploadAppointment(out string status)
        {
            bool isSuccess = false;
            status = "";

            try
            {
                Load_WS_URL();
                PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                ws.Timeout = 100000;
                ws.Url = WS_URL;

                string query = "SELECT * FROM Appointment where isServerUpdate = 1 ";
                DataTable dtHdr = new DataTable();
                dtHdr.Load(DataService.GetReader(new QueryCommand(query)));

                query = @"SELECT  AI.* 
                            FROM	Appointment AP
		                            INNER JOIN AppointmentItem AI ON AP.Id = AI.AppointmentId
                            AND AP.IsServerUpdate = 1";
                DataTable dtDet = new DataTable();
                dtDet.Load(DataService.GetReader(new QueryCommand(query)));

                dtHdr.TableName = "Appointment";
                dtDet.TableName = "AppointmentItem";

                isSuccess = ws.UploadTables(dtHdr, "Id", "Id", false, out status);
                if (isSuccess)
                    isSuccess = ws.UploadTables(dtDet, "Id", "Id", false, out status);
                else
                    throw new Exception(status);
                if (!isSuccess)
                    throw new Exception(status);
            }
            catch (Exception ex)
            {
                isSuccess = false;
                status = "ERROR SYNC APPOINTMENT " + ex.Message;
                Logger.writeLog(ex);
            }

            return isSuccess;
        }

        public static DataTable FetchAppointmentNotInServerWithPOSID(DateTime StartDate, int numofRecords,
            int PointOfSaleID)
        {
            try
            {
                string sqlString = "Select Top " + numofRecords + " * from Appointment where ISNULL(IsServerUpdate,0) = 1 and pointofsaleid = " + PointOfSaleID.ToString() +
                                    "Order By ModifiedOn ASC ";
                DataTable dt = DataService.GetDataSet(new QueryCommand(sqlString)).Tables[0];

                return dt;


            }
            catch (Exception ex)
            {
                Logger.writeLog("Get Order List from server:" + ex.Message, true);
                return null;
            }
        }
    }
}
