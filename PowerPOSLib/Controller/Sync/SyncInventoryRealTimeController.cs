using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using PowerPOS;
using PowerPOSLib.PowerPOSSync;
using SubSonic;
using System.Globalization;
using PowerPOS.Container;
using System.Threading;
using PowerPOS.Controller;
using PowerPOS;

namespace PowerPOS.InventoryRealTimeController
{
    public delegate void UpdateProgress(object sender, string message);
    public class SyncInventoryRealTimeController
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
        
        
        private bool getLatestModifiedOnLocal(out DateTime result)
        {
            result = DateTime.Now;
            string sqlString = "Select ISNULL(max(modifiedon),'2011-01-01') from InventoryHdr ";
            object obj = DataService.ExecuteScalar(new QueryCommand(sqlString));
            if (obj != null)
            {
                if (obj != null && obj.ToString() != "")
                    result = (DateTime)obj;
                else
                    return false;

                return true;
            
            }
            return true;
        }

        public bool GetRealTimeInventory()
        {
            try
            {
                #region Load Config
                Load_WS_URL();
                if (OnProgressUpdates != null)
                    OnProgressUpdates(this, "Sync Start");
                PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                ws.Timeout = 100000;
                ws.Url = WS_URL;
                DateTime lastSalesModifiedOn = DateTime.Today;
                int tryTimes = 0;
                bool isSuccessConnecting = false;
                bool result = false;
                int RetrySecConnected = 600;
                int RetrySecDisconnected = 5;
                if (!int.TryParse(AppSetting.GetSetting(AppSetting.SettingsName.Sync.InventoryRetrySecWhenConnected), out RetrySecConnected))
                {
                    RetrySecConnected = 600;
                }
                if (!int.TryParse(AppSetting.GetSetting(AppSetting.SettingsName.Sync.InventoryRetrySecWhenDisconnected), out RetrySecDisconnected))
                {
                    RetrySecDisconnected = 10;
                }
                #endregion
                while (true)
                {

                    DateTime LastModifiedOn = DateTime.Today.AddDays(-7);
                    if (!getLatestModifiedOnLocal(out LastModifiedOn))
                    {
                        if (OnProgressUpdates != null)
                            OnProgressUpdates(this, "Error Getting Last Modified Date on Local");
                        return false;
                    }

                    int totalRecords = 0;
                    isSuccessConnecting = false;

                    while (!isSuccessConnecting)
                    {
                        try
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Sync Inventory Started. Connecting To Server");
                            totalRecords = ws.GetInventoryCountRealTime(LastModifiedOn);
                            isSuccessConnecting = true;
                        }
                        catch (Exception ex)
                        {
                            Logger.writeLog(ex.Message);
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Failed Connect To Server.Retrying in 5 sec");
                            Thread.Sleep(RetrySecDisconnected * 1000);
                        }
                    }

                    int recordsperTime = 100;
                    if (!int.TryParse(AppSetting.GetSetting(AppSetting.SettingsName.Sync.InventorySyncRecordsPerTime), out recordsperTime))
                    {
                        recordsperTime = 100;
                    }
                    int dataGet = 0;

                    if (totalRecords == 0)
                    {
                        return true;
                    }
                    while (dataGet < totalRecords)
                    {
                        if (!getLatestModifiedOnLocal(out LastModifiedOn))
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Error Getting Last Modified Date on Local");
                            return false;
                        }

                        byte[] data = ws.GetInventoryRealTimeData(LastModifiedOn,recordsperTime);

                        DataSet ds = SyncClientController.DecompressDataSetFromByteArray(data);

                        QueryCommandCollection cmd = new QueryCommandCollection();
                        InventoryHdrCollection InventoryHdrs = new InventoryHdrCollection();
                        InventoryHdrs.Load(ds.Tables[0]);

                        if (OnProgressUpdates != null)
                            OnProgressUpdates(this, "Get Inventory records #" + (dataGet + 1) + "-" + (dataGet + InventoryHdrs.Count) + " of " + totalRecords + " to Server...");
                        if (InventoryHdrs.Count > 0)
                        {
                            Query qry = new Query("InventoryHdr");
                            Where whr = new Where();
                            whr.ColumnName = "InventoryHdrRefNo";
                            whr.Comparison = Comparison.Equals;
                            whr.ParameterName = "@InventoryHdrRefNo";

                            SubSonic.TableSchema.TableColumnCollection ColumnList = new SubSonic.TableSchema.TableColumnCollection();
                            ColumnList.AddRange(DataService.GetSchema("InventoryHdr", "PowerPOS").Columns);

                            for (int i = 0; i < InventoryHdrs.Count; i++)
                            {
                                //Get count....
                                InventoryHdrs[i].DirtyColumns.AddRange(ColumnList);
                                whr.ParameterValue = InventoryHdrs[i].InventoryHdrRefNo;
                                QueryCommand mycmd;
                                if (qry.GetCount(InventoryHdr.Columns.InventoryHdrRefNo, whr) > 0)
                                {
                                    mycmd = InventoryHdrs[i].GetUpdateCommand("SYNC");
                                    mycmd.Parameters.GetParameter("@ModifiedOn").ParameterValue = InventoryHdrs[i].ModifiedOn;
                                    mycmd.CommandSql = mycmd.CommandSql.Remove(mycmd.CommandSql.IndexOf("[ModifiedOn]"), 55);
                                    cmd.Add(mycmd);
                                }
                                else
                                {
                                    mycmd = InventoryHdrs[i].GetInsertCommand("SYNC");
                                    mycmd.Parameters.GetParameter("@ModifiedOn").ParameterValue = InventoryHdrs[i].ModifiedOn;
                                    cmd.Add(mycmd);
                                }

                            }

                        }
                        else
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "No InventoryHdr Loaded. Sync Finished");
                            
                        }

                        #region *) Item Set Meal Map
                        InventoryDetCollection InventoryDets = new InventoryDetCollection();
                        InventoryDets.Load(ds.Tables[1]);

                        if (InventoryDets.Count > 0)
                        {
                            Query qry = new Query("InventoryDet");
                            Where whr = new Where();
                            whr.ColumnName = "InventoryDetRefNo";
                            whr.Comparison = Comparison.Equals;
                            whr.ParameterName = "@InventoryDetRefNo";

                            SubSonic.TableSchema.TableColumnCollection ColumnList = new SubSonic.TableSchema.TableColumnCollection();
                            ColumnList.AddRange(DataService.GetSchema("InventoryDet", "PowerPOS").Columns);

                            for (int i = 0; i < InventoryDets.Count; i++)
                            {
                                //Get count....
                                InventoryDets[i].DirtyColumns.AddRange(ColumnList);
                                whr.ParameterValue = InventoryDets[i].InventoryDetRefNo;
                                QueryCommand mycmd;
                                if (qry.GetCount(InventoryDet.Columns.InventoryDetRefNo, whr) > 0)
                                {
                                    mycmd = InventoryDets[i].GetUpdateCommand("SYNC");
                                    mycmd.CommandSql = mycmd.CommandSql.Remove(mycmd.CommandSql.IndexOf("[ModifiedBy]"), 55);
                                    cmd.Add(mycmd);
                                }
                                else
                                {
                                    mycmd = InventoryDets[i].GetInsertCommand("SYNC");
                                    cmd.Add(mycmd);
                                }

                            }
                        }
                        #endregion

                        
                        if (cmd.Count > 0) DataService.ExecuteTransaction(cmd);
                        if (OnProgressUpdates != null)
                            OnProgressUpdates(this, "get Inventory records #" + (dataGet + 1) + "-" + (dataGet + InventoryHdrs.Count) + " of " + totalRecords + " to Server Successful");
                        dataGet += recordsperTime;
                    }
                       
                    

                }
                
            }
            catch (Exception ex1) { Logger.writeLog(ex1.Message); return false; }
        }

        public bool GetRealTimeStockTake()
        {
            try
            {
                #region Load Config
                Load_WS_URL();
                if (OnProgressUpdates != null)
                    OnProgressUpdates(this, "Sync Start");
                PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                ws.Timeout = 100000;
                ws.Url = WS_URL;
                DateTime lastSalesModifiedOn = DateTime.Today;
                int tryTimes = 0;
                bool isSuccessConnecting = false;
                bool result = false;
                int RetrySecConnected = 600;
                int RetrySecDisconnected = 5;
                int recordsperTime = 100;
                if (!int.TryParse(AppSetting.GetSetting(AppSetting.SettingsName.Sync.InventoryRetrySecWhenConnected), out RetrySecConnected))
                {
                    RetrySecConnected = 600;
                }
                if (!int.TryParse(AppSetting.GetSetting(AppSetting.SettingsName.Sync.InventoryRetrySecWhenDisconnected), out RetrySecDisconnected))
                {
                    RetrySecDisconnected = 10;
                }
                if (!int.TryParse(AppSetting.GetSetting(AppSetting.SettingsName.Sync.InventorySyncRecordsPerTime), out recordsperTime))
                    recordsperTime = 100;

                bool onlyDownloadOwnLocation = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.OnlyAllowInCurrentInventoryLocation), false);
                #endregion
                while (true)
                {
                    #region *) Download Stock Take

                    try
                    {
                        DateTime lastModifiedOnCategory = DateTime.Now.AddDays(-7);
                        if (onlyDownloadOwnLocation)
                        {
                            if (!SyncRealTimeController.GetLatestModifiedOnLocalByInvLoc("StockTake", out lastModifiedOnCategory))
                            {
                                if (OnProgressUpdates != null)
                                    OnProgressUpdates(this, "Error Getting Last Modified Date of the StockTake on Local");
                                Thread.Sleep(RetrySecDisconnected * 1000);
                                continue;
                            }
                        }
                        else
                        {
                            if (!SyncRealTimeController.GetLatestModifiedOnLocal("StockTake", out lastModifiedOnCategory))
                            {
                                if (OnProgressUpdates != null)
                                    OnProgressUpdates(this, "Error Getting Last Modified Date of the StockTake on Local");
                                Thread.Sleep(RetrySecDisconnected * 1000);
                                continue;
                            }
                        }


                        int totalRecordCtg = 0;
                        if (onlyDownloadOwnLocation)
                        {
                            totalRecordCtg = ws.FetchRecordNoByTimestampByInvLocID("StockTake", lastModifiedOnCategory, PointOfSaleInfo.InventoryLocationID);
                        }
                        else
                        {
                            totalRecordCtg = ws.FetchRecordNoByTimestamp("StockTake", lastModifiedOnCategory);
                        }
                        if (totalRecordCtg > 0)
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Start Sync Stock Take");
                            int dataGetCtg = 0;

                            int recordPerTimeCtg = totalRecordCtg < recordsperTime ? totalRecordCtg : recordsperTime;

                            while (dataGetCtg < totalRecordCtg)
                            {
                                if (onlyDownloadOwnLocation)
                                {
                                    if (!SyncRealTimeController.GetLatestModifiedOnLocalByInvLoc("StockTake", out lastModifiedOnCategory))
                                    {
                                        if (OnProgressUpdates != null)
                                            OnProgressUpdates(this, "Error Getting Last Modified Date of the ItemSummary on Local");
                                        Thread.Sleep(RetrySecDisconnected * 1000);
                                        continue;
                                    }
                                }
                                else
                                {
                                    if (!SyncRealTimeController.GetLatestModifiedOnLocal("StockTake", out lastModifiedOnCategory))
                                    {
                                        if (OnProgressUpdates != null)
                                            OnProgressUpdates(this, "Error Getting Last Modified Date of the ItemSummary on Local");
                                        Thread.Sleep(RetrySecDisconnected * 1000);
                                        continue;
                                    }
                                }
                                if (onlyDownloadOwnLocation)
                                {
                                    var dataCtg = ws.FetchDataSetByTimestamp("StockTake", "StockTakeID", lastModifiedOnCategory, recordsperTime, true, PointOfSaleInfo.InventoryLocationID, false, 0);
                                    var dsCtg = SyncClientController.DecompressDataSetFromByteArray(dataCtg);
                                    if (SyncRealTimeController.DownloadData(dsCtg, "StockTake", "StockTakeID", true))
                                    {
                                        dataGetCtg += dsCtg.Tables[0].Rows.Count;
                                        if (dataGetCtg > totalRecordCtg)
                                            dataGetCtg = totalRecordCtg;
                                        if (OnProgressUpdates != null)
                                            OnProgressUpdates(this, string.Format("Download StockTake data. {0} of {1} ", dataGetCtg, totalRecordCtg));
                                    }
                                    else
                                    {
                                        if (OnProgressUpdates != null)
                                            OnProgressUpdates(this, string.Format(">> Failed download StockTake"));
                                        Thread.Sleep(RetrySecDisconnected * 1000);
                                    }
                                }
                                else
                                {
                                    var dataCtg = ws.FetchDataSetByTimestamp("StockTake", "StockTakeID", lastModifiedOnCategory, recordsperTime, false, PointOfSaleInfo.InventoryLocationID, false, 0);
                                    var dsCtg = SyncClientController.DecompressDataSetFromByteArray(dataCtg);
                                    if (SyncRealTimeController.DownloadData(dsCtg, "StockTake", "StockTakeID", true))
                                    {
                                        dataGetCtg += dsCtg.Tables[0].Rows.Count;
                                        if (dataGetCtg > totalRecordCtg)
                                            dataGetCtg = totalRecordCtg;
                                        if (OnProgressUpdates != null)
                                            OnProgressUpdates(this, string.Format("Download StockTake data. {0} of {1} ", dataGetCtg, totalRecordCtg));
                                    }
                                    else
                                    {
                                        if (OnProgressUpdates != null)
                                            OnProgressUpdates(this, string.Format(">> Failed download StockTake"));
                                        Thread.Sleep(RetrySecDisconnected * 1000);
                                    }
                                }
                            }
                            if (OnProgressUpdates != null)
                            {
                                OnProgressUpdates(this, "All StockTake have been synchronized successfully.");
                                return true;
                            }
                        }
                        else
                        {
                            return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.writeLog(ex);
                        string msg = "Error downloading StockTake. Server is disconnected, will keep trying every {0} seconds until server is connected";
                        msg = string.Format(msg, RetrySecDisconnected);
                        if (OnProgressUpdates != null)
                            OnProgressUpdates(this, msg);
                        Thread.Sleep(RetrySecDisconnected * 1000);
                        continue;
                    }
                    #endregion
                }

            }
            catch (Exception ex1) { Logger.writeLog(ex1.Message); return false; }
        }

        public bool GetPurchaseOrder()
        {
            try
            {
                #region Load Config
                Load_WS_URL();
                if (OnProgressUpdates != null)
                    OnProgressUpdates(this, "Sync Start");
                PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                ws.Timeout = 100000;
                ws.Url = WS_URL;
                DateTime lastSalesModifiedOn = DateTime.Today;
                int tryTimes = 0;
                bool isSuccessConnecting = false;
                bool result = false;
                int RetrySecConnected = 600;
                int RetrySecDisconnected = 5;
                int recordsperTime = 100;
                if (!int.TryParse(AppSetting.GetSetting(AppSetting.SettingsName.Sync.InventoryRetrySecWhenConnected), out RetrySecConnected))
                {
                    RetrySecConnected = 600;
                }
                if (!int.TryParse(AppSetting.GetSetting(AppSetting.SettingsName.Sync.InventoryRetrySecWhenDisconnected), out RetrySecDisconnected))
                {
                    RetrySecDisconnected = 10;
                }
                if (!int.TryParse(AppSetting.GetSetting(AppSetting.SettingsName.Sync.InventorySyncRecordsPerTime), out recordsperTime))
                    recordsperTime = 100;

                bool onlyDownloadOwnLocation = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.OnlyAllowInCurrentInventoryLocation), false);
                #endregion
                while (true)
                {
                    #region *) Download Stock Take

                    try
                    {
                        DateTime lastModifiedOnCategory = DateTime.Now.AddDays(-7);
                        if (onlyDownloadOwnLocation)
                        {
                            if (!SyncRealTimeController.GetLatestModifiedOnLocalByInvLoc("PurchaseOrderHdr", out lastModifiedOnCategory))
                            {
                                if (OnProgressUpdates != null)
                                    OnProgressUpdates(this, "Error Getting Last Modified Date of the PurchaseOrderHdr on Local");
                                Thread.Sleep(RetrySecDisconnected * 1000);
                                continue;
                            }
                        }
                        else
                        {
                            if (!SyncRealTimeController.GetLatestModifiedOnLocal("PurchaseOrderHdr", out lastModifiedOnCategory))
                            {
                                if (OnProgressUpdates != null)
                                    OnProgressUpdates(this, "Error Getting Last Modified Date of the PurchaseOrderHdr on Local");
                                Thread.Sleep(RetrySecDisconnected * 1000);
                                continue;
                            }
                        }


                        int totalRecordCtg = 0;
                        if (onlyDownloadOwnLocation)
                        {
                            totalRecordCtg = ws.FetchRecordNoByTimestampByInvLocID("PurchaseOrderHdr", lastModifiedOnCategory, PointOfSaleInfo.InventoryLocationID);
                        }
                        else
                        {
                            totalRecordCtg = ws.FetchRecordNoByTimestamp("PurchaseOrderHdr", lastModifiedOnCategory);
                        }
                        if (totalRecordCtg > 0)
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Start Sync Purchase Order");
                            int dataGetCtg = 0;

                            int recordPerTimeCtg = totalRecordCtg < recordsperTime ? totalRecordCtg : recordsperTime;

                            while (dataGetCtg < totalRecordCtg)
                            {
                                if (onlyDownloadOwnLocation)
                                {
                                    if (!SyncRealTimeController.GetLatestModifiedOnLocalByInvLoc("PurchaseOrderHdr", out lastModifiedOnCategory))
                                    {
                                        if (OnProgressUpdates != null)
                                            OnProgressUpdates(this, "Error Getting Last Modified Date of the PurchaseOrderHdr on Local");
                                        Thread.Sleep(RetrySecDisconnected * 1000);
                                        continue;
                                    }
                                }
                                else
                                {
                                    if (!SyncRealTimeController.GetLatestModifiedOnLocal("PurchaseOrderHdr", out lastModifiedOnCategory))
                                    {
                                        if (OnProgressUpdates != null)
                                            OnProgressUpdates(this, "Error Getting Last Modified Date of the PurchaseOrderHdr on Local");
                                        Thread.Sleep(RetrySecDisconnected * 1000);
                                        continue;
                                    }
                                }
                                 System.Transactions.TransactionOptions to = new System.Transactions.TransactionOptions();
                                to.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
                                using (System.Transactions.TransactionScope transScope =
                                new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Required, to))
                                {
                                    if (onlyDownloadOwnLocation)
                                    {
                                        var dataCtg = ws.GetPurchaseOrderRealTimeData(lastModifiedOnCategory, recordsperTime, PointOfSaleInfo.InventoryLocationID);
                                        var dsCtg = SyncClientController.DecompressDataSetFromByteArray(dataCtg);
                                        if (SyncRealTimeController.DownloadData(dsCtg, "PurchaseOrderHdr", "PurchaseOrderHdrRefNo", false))
                                        {
                                            DataTable dt = dsCtg.Tables[1].Copy();
                                            DataSet dsPurchaseOrderDet = new DataSet();
                                            dsPurchaseOrderDet.Tables.Add(dt);
                                            SyncRealTimeController.DownloadData(dsPurchaseOrderDet, "PurchaseOrderDet", "PurchaseOrderDetRefNo", false);

                                            dataGetCtg += dsCtg.Tables[0].Rows.Count;
                                            if (dataGetCtg > totalRecordCtg)
                                                dataGetCtg = totalRecordCtg;
                                            if (OnProgressUpdates != null)
                                                OnProgressUpdates(this, string.Format("Download PurchaseOrderHdr data. {0} of {1} ", dataGetCtg, totalRecordCtg));
                                        }
                                        else
                                        {
                                            if (OnProgressUpdates != null)
                                                OnProgressUpdates(this, string.Format(">> Failed download PurchaseOrderHdr"));
                                            Thread.Sleep(RetrySecDisconnected * 1000);
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        var dataCtg = ws.GetPurchaseOrderRealTimeData(lastModifiedOnCategory, recordsperTime, 0); ;
                                        var dsCtg = SyncClientController.DecompressDataSetFromByteArray(dataCtg);
                                        if (SyncRealTimeController.DownloadData(dsCtg, "PurchaseOrderHdr", "PurchaseOrderHdrRefNo", false))
                                        {
                                            DataTable dt = dsCtg.Tables[1].Copy();
                                            DataSet dsPurchaseOrderDet = new DataSet();
                                            dsPurchaseOrderDet.Tables.Add(dt);
                                            SyncRealTimeController.DownloadData(dsPurchaseOrderDet, "PurchaseOrderDet", "PurchaseOrderDetRefNo", false);

                                            dataGetCtg += dsCtg.Tables[0].Rows.Count;
                                            if (dataGetCtg > totalRecordCtg)
                                                dataGetCtg = totalRecordCtg;
                                            if (OnProgressUpdates != null)
                                                OnProgressUpdates(this, string.Format("Download PurchaseOrderHdr data. {0} of {1} ", dataGetCtg, totalRecordCtg));
                                        }
                                        else
                                        {
                                            if (OnProgressUpdates != null)
                                                OnProgressUpdates(this, string.Format(">> Failed download PurchaseOrderHdr"));
                                            Thread.Sleep(RetrySecDisconnected * 1000);
                                            return false;
                                        }
                                    }
                                    transScope.Complete();
                                }
                            }
                            if (OnProgressUpdates != null)
                            {
                                OnProgressUpdates(this, "All PurchaseOrderHdr have been synchronized successfully.");
                                return true;
                            }
                        }
                        else
                        {
                            OnProgressUpdates(this, "All PurchaseOrderHdr have been synchronized successfully.");
                            return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.writeLog(ex);
                        string msg = "Error downloading StockTake. Server is disconnected, will keep trying every {0} seconds until server is connected";
                        msg = string.Format(msg, RetrySecDisconnected);
                        if (OnProgressUpdates != null)
                            OnProgressUpdates(this, msg);
                        Thread.Sleep(RetrySecDisconnected * 1000);
                        continue;
                    }
                    #endregion
                }

            }
            catch (Exception ex1) { Logger.writeLog(ex1.Message); return false; }
        }
        

    }
}

;