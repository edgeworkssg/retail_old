using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using PowerPOS;
using PowerPOS.Container;
using System.Threading;
using SubSonic;
using System.Web.Script.Serialization;

namespace PowerPOSLib.RatingController
{
    public delegate void UpdateProgress(object sender, string message);

    public class SyncRatingRealTimeController
    {
        public const string XMLFILENAME = "\\WS.XML";
        public string WS_URL = "http://localhost:51831/PowerPOSWeb/Synchronization/Synchronization.asmx";

        public bool Load_WS_URL()
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

        public event UpdateProgress OnProgressUpdates;

        public bool SendRealTimeRating()
        {
            int retryDC = 5; // default

            #region Load Setting

            try
            {
                retryDC = int.Parse(AppSetting.GetSetting(AppSetting.SettingsName.Sync.RatingRetrySecWhenDisconnected));
            }
            catch (Exception e)
            {
                Logger.writeLog(e);
            }

            #endregion

            retryDC = retryDC * 1000;

            try
            {
                #region Load Config
                Load_WS_URL();
                if (OnProgressUpdates != null)
                    OnProgressUpdates(this, "Sync Start");
                PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                ws.Timeout = 100000; // 10s
                ws.Url = WS_URL;
                DateTime lastModifiedOn = DateTime.Today;
                int tryTimes = 0;
                bool isSuccessConnecting = false;
                #endregion
                while (true)
                {
                    if (OnProgressUpdates != null)
                        OnProgressUpdates(this, "Sync Rating Started. Connecting to Server");

                    tryTimes = 0;
                    isSuccessConnecting = false;

                    #region Connecting & Get Last TimeStamp
                    while (!isSuccessConnecting)
                    {
                        tryTimes++;
                        try
                        {
                            isSuccessConnecting = ws.getRatingLastTimeStamp(PointOfSaleInfo.PointOfSaleID, out lastModifiedOn);

                        }
                        catch (Exception ex)
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Failed To Connect To Server. Retrying in 5 sec.");
                            Logger.writeLog("Error Connecting To Server. " + ex.Message);

                            Thread.Sleep(retryDC);
                        }
                    }
                    #endregion

                    if (OnProgressUpdates != null)
                        OnProgressUpdates(this, "Server is Connected");

                    if (isSuccessConnecting)
                    {
                        if (OnProgressUpdates != null)
                            OnProgressUpdates(this, "Last Modified On from Server : " + lastModifiedOn.ToString("yyyy-MM-dd HH:mm:ss.fff"));

                        List<RatingClz> data = new List<RatingClz>();
                        string statusMsg;
                        if (!getLocalRatingDataCount(lastModifiedOn, data, out statusMsg))
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Error Get Local Data Count. " + statusMsg);
                            return false;
                        }

                        if (data.Count == 0)
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "All data has been sent successfully to Server. Sync Rating is completed.");

                            return true;
                        }

                        if (OnProgressUpdates != null)
                            OnProgressUpdates(this, "Total Records to be Synced : " + data.Count);

                        #region init variables
                        int dataSentCount = 0;
                        int recordPerTimes = 0;
                        string tempRecordPerTimes = AppSetting.GetSetting(AppSetting.SettingsName.Sync.SyncRatingRecordsPerTime);
                        if (!int.TryParse(tempRecordPerTimes, out recordPerTimes))
                        {
                            recordPerTimes = 100; // default
                        }
                        #endregion

                        dataSentCount = data.Count >= recordPerTimes ? recordPerTimes : data.Count;
                        List<RatingClz> dataSent = new List<RatingClz>();

                        for (int i = 0; i < dataSentCount; i++)
                        {
                            RatingClz rc = data[i];
                            dataSent.Add(rc);
                        }

                        ws.SyncRating(new JavaScriptSerializer().Serialize(dataSent));
                    }
                }
            }
            catch (Exception ex1) { Logger.writeLog(ex1.Message); return false; }
        }

        public bool getLocalRatingDataCount(DateTime lastModifiedOn, List<RatingClz> data, out string statusMsg)
        {
            statusMsg = "";
            try
            {
                QueryCommand cmd = new QueryCommand(String.Format("SELECT * FROM rating WHERE ModifiedOn > CONVERT(DATETIME, '{0}', 105) AND POSID = {1} Order By ModifiedOn ASC", lastModifiedOn.ToString("dd-MM-yyyy HH:mm:ss.fff"), PointOfSaleInfo.PointOfSaleID));
                DataSet ds = SubSonic.DataService.GetDataSet(cmd);

                var tbl = ds.Tables[0];
                for (int i = 0; i < tbl.Rows.Count; i++)
                {
                    var row = tbl.Rows[i];
                    data.Add(new RatingClz 
                    { 
                        POSID = Int32.Parse(row["POSID"].ToString()),
                        Rating = Int32.Parse(row["Rating"].ToString()),
                        Staff = row["Staff"].ToString(),
                        Timestamp = ((DateTime)row["Timestamp"]).ToString("dd-MM-yyyy HH:mm:ss.fff"),
                        CreatedOn = ((DateTime)row["CreatedOn"]).ToString("dd-MM-yyyy HH:mm:ss.fff"),
                        CreatedBy = row["CreatedBy"].ToString(),
                        ModifiedOn = ((DateTime)row["ModifiedOn"]).ToString("dd-MM-yyyy HH:mm:ss.fff"),
                        ModifiedBy = row["ModifiedBy"].ToString(),
                        Deleted = row["Deleted"].ToString() == "1",
                        UniqueId = row["UniqueId"].ToString(),
                        OrderHdrID = row["OrderHdrID"].ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                data.Clear();
                statusMsg = "";
                return false;
            }

            return true;
        }

        public class RatingClz
        {
            public int RatingID { get; set; }
            public int POSID { get; set; }
            public int Rating { get; set; }
            public string Staff { get; set; }
            public string Timestamp { get; set; }
            public string CreatedOn { get; set; }
            public string CreatedBy { get; set; }
            public string ModifiedOn { get; set; }
            public string ModifiedBy { get; set; }
            public bool Deleted { get; set; }
            public string UniqueId { get; set; }
            public string OrderHdrID { get; set; }
        }
    }
}
