using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic;
using PowerPOS.Container;

namespace PowerPOS
{
    public partial class Z2ClosingLogController
    {
        public static DateTime GetLastZ2ClosingTime(int PointOfSaleID)
        {
            try
            {
                Query qr = Z2ClosingLog.CreateQuery();
                qr.AddWhere(Z2ClosingLog.Columns.PointOfSaleID, PointOfSaleID);
                qr.QueryType = QueryType.Select;
                qr.SelectList = "endtime";
                qr.Top = "1";
                qr.OrderBy = OrderBy.Desc("endtime");
                Object obj = qr.ExecuteScalar();
                if (obj != null && obj is DateTime)
                {
                    return (DateTime)obj;
                }
                else
                {
                    return new DateTime(1979, 11, 3);
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return new DateTime(1979, 11, 3);
            }

        }
        public static CounterCloseLogCollection GetSettlementForZ2Closings(int PointOfSaleID, out Z2ClosingLog total)
        {
            try
            {
                //get the last timing of the z2 closing
                DateTime lastZ2Closing = GetLastZ2ClosingTime(PointOfSaleID);

                //load all the closing between then up to the current time
                CounterCloseLogCollection ct = new CounterCloseLogCollection();
                ct.Where(CounterCloseLog.Columns.PointOfSaleID, PointOfSaleID);
                ct.Where(CounterCloseLog.Columns.EndTime, Comparison.GreaterThan, lastZ2Closing);
                ct.Where(CounterCloseLog.Columns.EndTime, Comparison.LessOrEquals, DateTime.Now);
                ct.Load();

                Z2ClosingLog newLog = new Z2ClosingLog();
                newLog.Z2ClosingLogID =
                    PowerPOS.DbUtilityController.CreateNewGenericHdrRefNo
                    ("Z2ClosingLog", "Z2ClosingLogID", PointOfSaleID);
                if (ct.Count > 0)
                {
                    newLog.StartTime = ct[0].StartTime;
                    newLog.EndTime = ct[ct.Count - 1].EndTime;
                    for (int i = 0; i < ct.Count; i++)
                    {
                        //sum each column together
                        newLog.TotalActualCollected += ct[i].TotalActualCollected;
                        newLog.TotalSystemRecorded += ct[i].TotalSystemRecorded;
                        newLog.Variance += ct[i].Variance;
                        newLog.CashCollected += ct[i].CashCollected;
                        newLog.CashIn += ct[i].CashIn;
                        newLog.CashOut += ct[i].CashOut;
                        newLog.CashRecorded += ct[i].CashRecorded;
                        newLog.ClosingCashOut += ct[i].ClosingCashOut;
                        newLog.FloatBalance += ct[i].FloatBalance;
                        newLog.OpeningBalance += ct[i].OpeningBalance;
                        newLog.PointOfSaleID = PointOfSaleID;
                    }
                }
                else
                {
                    newLog.StartTime = DateTime.Today;
                    newLog.EndTime = DateTime.Today.AddDays(1).AddMinutes(-1);
                }
                total = newLog;
                return ct;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                total = null;
                return null;
            }
        }
        public static bool PerformZ2Closing(int PointOfSaleID, out Z2ClosingLog newLog)
        {
            newLog = new Z2ClosingLog();
            try
            {
                //get the last timing of the z2 closing
                DateTime lastZ2Closing = GetLastZ2ClosingTime(PointOfSaleID);

                //load all the closing between then up to the current time
                CounterCloseLogCollection ct = new CounterCloseLogCollection();
                ct.Where(CounterCloseLog.Columns.PointOfSaleID, PointOfSaleID);
                ct.Where(CounterCloseLog.Columns.EndTime, Comparison.GreaterThan, lastZ2Closing);
                ct.Where(CounterCloseLog.Columns.EndTime, Comparison.LessOrEquals, DateTime.Now);
                ct.Load();


                newLog.Z2ClosingLogID =
                    PowerPOS.DbUtilityController.CreateNewGenericHdrRefNo
                    ("Z2ClosingLog", "Z2ClosingLogID", PointOfSaleID);
                if (ct.Count > 0)
                {
                    newLog.StartTime = ct[0].StartTime;
                    newLog.EndTime = ct[ct.Count - 1].EndTime;

                    for (int i = 0; i < ct.Count; i++)
                    {
                        //sum each column together
                        newLog.TotalActualCollected += ct[i].TotalActualCollected;
                        newLog.TotalSystemRecorded += ct[i].TotalSystemRecorded;
                        newLog.Variance += ct[i].Variance;
                        newLog.CashCollected += ct[i].CashCollected;
                        newLog.CashIn += ct[i].CashIn;
                        newLog.CashOut += ct[i].CashOut;
                        newLog.CashRecorded += ct[i].CashRecorded;
                        newLog.ClosingCashOut += ct[i].ClosingCashOut;
                        newLog.FloatBalance += ct[i].FloatBalance;
                        newLog.OpeningBalance += ct[i].OpeningBalance;
                        newLog.PointOfSaleID = PointOfSaleID;

                    }
                }
                else
                {
                    return false;
                }
                newLog.UniqueID = Guid.NewGuid();
                newLog.Cashier = UserInfo.username;
                //save to disk
                newLog.Save(UserInfo.username);

                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return false;
            }
        }
    }
}
