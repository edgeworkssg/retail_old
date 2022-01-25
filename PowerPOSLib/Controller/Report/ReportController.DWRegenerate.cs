using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using PowerPOS.Container;
using PowerPOS;
using SubSonic;
using System.ComponentModel;
using System.Linq;

namespace PowerPOS
{
    public partial class ReportController
    {
        public static void RegenerateDWData(DWType[] types, DateTime startDate, DateTime endDate, string outlet)
        {
            try
            {
                var listRegenerateDate = new List<RegenerateDate>();

                bool useDataWarehouse = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Report.UseDataWarehouse), false);
                bool isNeedRegenerate = false;

                if (useDataWarehouse)
                {
                    string sql = @"
                    SELECT  *
                    FROM	DW_RegenerateDate
                    WHERE  (@Outlet = '' OR 
		                    @Outlet = 'ALL' OR 
		                    @Outlet = 'ALL-BreakDown' OR
		                    @Outlet = 'ALL - BreakDown' OR
		                    @Outlet = OutletName)
		                    AND CAST(OrderDate AS DATE) BETWEEN @StartDate AND @EndDate";

                    QueryCommand qc = new QueryCommand(sql);
                    qc.AddParameter("@Outlet", outlet);
                    qc.AddParameter("@StartDate", startDate);
                    qc.AddParameter("@EndDate", endDate);

                    DataTable dtRegenerate = new DataTable();
                    dtRegenerate.Load(DataService.GetReader(qc));

                    Logger.writeLog("RegenerateDWData dtRegenerate.Rows.Count:" + dtRegenerate.Rows.Count);
                    if (dtRegenerate.Rows.Count > 0)
                    {
                        isNeedRegenerate = true;
                        for (int i = 0; i < dtRegenerate.Rows.Count; i++)
                        {
                            var theDate = ((DateTime)dtRegenerate.Rows[i]["OrderDate"]);
                            listRegenerateDate.Add(new RegenerateDate { StartDate = theDate, EndDate = theDate });
                        }
                    }
                }
                else
                {
                    isNeedRegenerate = true;
                    listRegenerateDate.Add(new RegenerateDate { StartDate = startDate, EndDate = endDate });
                }

                Logger.writeLog("RegenerateDWData isNeedRegenerate:" + isNeedRegenerate);
                if (isNeedRegenerate)
                {
                    Logger.writeLog("RegenerateDWData types:" + types.Length);
                    if (types.Contains(DWType.ALL) || types.Contains(DWType.DW_HourlyProductSales))
                    {
                        foreach (var dt in listRegenerateDate)
                        {
                            string sqlUpdate = @"[dbo].[UPDATE_DW_HourlyProductSales]
                                @StartDate = @StartDate_,
                                @EndDate = @EndDate_,
                                @Outlet = @Outlet_";
                            QueryCommand qcUpdate = new QueryCommand(sqlUpdate);
                            qcUpdate.AddParameter("@StartDate_", dt.StartDate.ToString("yyyy-MM-dd"));
                            qcUpdate.AddParameter("@EndDate_", dt.EndDate.ToString("yyyy-MM-dd"));
                            qcUpdate.AddParameter("@Outlet_", outlet);

                            DataService.ExecuteQuery(qcUpdate);
                            Logger.writeLog(string.Format("RegenerateDWData UPDATE_DW_HourlyProductSales {0}/{1}", dt.StartDate, dt.EndDate));
                        }
                    }

                    if (types.Contains(DWType.ALL) || types.Contains(DWType.DW_HourlySales))
                    {
                        foreach (var dt in listRegenerateDate)
                        {
                            string sqlUpdate = @"[dbo].[UPDATE_DW_HourlySales]
                                @StartDate = @StartDate_,
                                @EndDate = @EndDate_,
                                @Outlet = @Outlet_";
                            QueryCommand qcUpdate = new QueryCommand(sqlUpdate);
                            qcUpdate.AddParameter("@StartDate_", dt.StartDate.ToString("yyyy-MM-dd"));
                            qcUpdate.AddParameter("@EndDate_", dt.EndDate.ToString("yyyy-MM-dd"));
                            qcUpdate.AddParameter("@Outlet_", outlet);

                            DataService.ExecuteQuery(qcUpdate);
                            Logger.writeLog(string.Format("RegenerateDWData UPDATE_DW_HourlySales {0}/{1}", dt.StartDate, dt.EndDate));
                        }
                    }

                    if (types.Contains(DWType.ALL))
                    {
                        string sqlDelete = @"
                        DELETE	DW_RegenerateDate
                        WHERE  (@Outlet = '' OR 
		                        @Outlet = 'ALL' OR 
		                        @Outlet = 'ALL-BreakDown' OR
		                        @Outlet = 'ALL - BreakDown' OR
		                        @Outlet = OutletName)
		                        AND CAST(OrderDate AS DATE) BETWEEN @StartDate AND @EndDate";

                        QueryCommand qc = new QueryCommand(sqlDelete);
                        qc.AddParameter("@Outlet", outlet);
                        qc.AddParameter("@StartDate", startDate.ToString("yyyy-MM-dd"));
                        qc.AddParameter("@EndDate", endDate.ToString("yyyy-MM-dd"));

                        DataService.ExecuteQuery(qc);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }
    }

    public enum DWType
    {
        ALL,
        DW_HourlyPayment,
        DW_HourlyProductSales,
        DW_HourlySales,
        DW_InvItemBalMonthly,
        DW_InvLocBalMonthly
    }

    public class RegenerateDate
    {
        public DateTime StartDate { set; get; }
        public DateTime EndDate { set; get; }
    }
}
