using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic;

namespace PowerPOS
{
    public partial class PerformanceLogController
    {
        public static void AddLog(DateTime startTime, DateTime endTime, string moduleName, string functionName, 
            int pointOfSaleID, string primaryKeyData, string username)
        {
            try
            {
                decimal processMs = Convert.ToDecimal((endTime - startTime).TotalMilliseconds);
                PerformanceLog pLog = new PerformanceLog();
                pLog.PerformanceLogID = Guid.NewGuid();
                pLog.ModuleName = moduleName;
                pLog.FunctionName = functionName;
                pLog.PointOfSaleID = pointOfSaleID;
                pLog.ElapsedTime = Convert.ToDecimal(processMs);
                pLog.PrimaryKeyData = primaryKeyData;
                pLog.Deleted = false;
                pLog.Save(username);

                PerformanceLogSummary pLogSumm;
                PerformanceLogSummaryCollection pLogSummColl = new PerformanceLogSummaryCollection();
                pLogSummColl.Where(PerformanceLogSummary.Columns.ModuleName, moduleName);
                pLogSummColl.Where(PerformanceLogSummary.Columns.FunctionName, functionName);
                pLogSummColl.Where(PerformanceLogSummary.Columns.PointOfSaleID, pointOfSaleID);
                pLogSummColl.Where("TimeStamp", endTime.ToString("yyyy-MM-dd HH:00:00.000"));
                pLogSummColl.Load();
                if (pLogSummColl != null && pLogSummColl.Count > 0)
                {
                    pLogSumm = pLogSummColl[0];
                    pLogSumm.AvgElapsedTime = ((pLogSumm.AvgElapsedTime * pLogSumm.TransCount) + processMs) / (pLogSumm.TransCount + 1);
                    if (processMs < pLogSumm.MinElapsedTime) pLogSumm.MinElapsedTime = processMs;
                    if (processMs > pLogSumm.MaxElapsedTime) pLogSumm.MaxElapsedTime = processMs;
                    pLogSumm.TransCount += 1;
                    pLogSumm.Save(username);
                }
                else
                {
                    pLogSumm = new PerformanceLogSummary();
                    pLogSumm.PerformanceLogSummaryID = Guid.NewGuid();
                    pLogSumm.ModuleName = moduleName;
                    pLogSumm.FunctionName = functionName;
                    pLogSumm.PointOfSaleID = pointOfSaleID;
                    pLogSumm.TimeStamp = DateTime.Parse(endTime.ToString("yyyy-MM-dd HH:00:00.000"));
                    pLogSumm.AvgElapsedTime = processMs;
                    pLogSumm.MinElapsedTime = processMs;
                    pLogSumm.MaxElapsedTime = processMs;
                    pLogSumm.TransCount = 1;
                    pLogSumm.Deleted = false;
                    pLogSumm.Save(username);
                }

                QueryCommand cmd = DeleteOldLogCommand(moduleName, functionName, pointOfSaleID);
                if (cmd != null)
                    DataService.ExecuteQuery(cmd);
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        public static QueryCommand DeleteOldLogCommand(string moduleName, string functionName, int pointOfSaleID)
        {
            try
            {
                int maxRecord = 1000;
                string sql = @"
                                DELETE FROM PerformanceLog WHERE ModuleName = '{0}' AND FunctionName = '{1}' AND PointOfSaleID = {2}
                                    AND PerformanceLogID NOT IN (
                                            SELECT TOP {3} PerformanceLogID 
                                            FROM PerformanceLog 
                                            WHERE ModuleName = '{0}' AND FunctionName = '{1}' AND PointOfSaleID = {2} 
                                            ORDER BY CreatedOn DESC
                                        )
                              ";
                sql = string.Format(sql, moduleName, functionName, pointOfSaleID, maxRecord);
                return new QueryCommand(sql, "PowerPOS");
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
        }
    }
}
