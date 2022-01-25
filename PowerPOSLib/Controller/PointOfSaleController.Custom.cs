using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic;
using System.Data;

namespace PowerPOS
{
    public partial class PointOfSaleController
    {
        public static string FetchTenantMachineID()
        {
            string result = "";

            int startFrom = (AppSetting.GetSetting(AppSetting.SettingsName.MallIntegration.TenantIDStartFrom) + "").GetIntValue();
            int increment = (AppSetting.GetSetting(AppSetting.SettingsName.MallIntegration.TenantIDIncrement) + "").GetIntValue();
            if (increment == 0)
                increment = 1;

            try
            {
                string sql = @"SELECT MAX(CAST(ISNULL(TenantMachineID,'0') AS INT)) TenantMachineID
                            FROM PointOfSale
                            WHERE	ISNULL(Deleted,0) = 0";
                DataTable dt = new DataTable();
                dt.Load(DataService.GetReader(new QueryCommand(sql)));
                if (dt.Rows.Count > 0)
                {
                    int currentNo = (dt.Rows[0]["TenantMachineID"] + "").GetIntValue();
                    if (currentNo != 0)
                        startFrom = currentNo;
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
            result = (startFrom + increment).ToString();

            return result;
        }

        private static PointOfSaleCollection FetchByUserNamePerPOSAssignment(bool includeBreakDown, bool includeALL, string userName, string outletName)
        {
            PointOfSaleCollection data = new PointOfSaleCollection();

            try
            {
                UserMst um = new UserMst(userName + "");
                if (!um.IsNew && !um.Deleted.GetValueOrDefault(false))
                {
                    string sql = "";

                    DataTable dt = new DataTable();
                    if (!(um.AssignedPOS + "").ToUpper().Equals("ALL") && !(um.AssignedPOS + "").ToString().Equals(string.Empty))
                    {
                        string[] assignedPOS = um.AssignedPOSList;
                        sql = @"SELECT   POS.*
                                    FROM	PointOfSale POS
		                                    LEFT JOIN Outlet OU ON OU.OutletName = POS.OutletName
                                    WHERE	ISNULL(POS.Deleted,0) = 0 AND ISNULL(OU.Deleted,0) = 0
		                                    AND POS.PointOfSaleID IN ('{0}')
                                            AND (OU.OutletName = '{1}' OR '{1}' IN ('ALL','ALL - BreakDown',''))
                                    ORDER BY POS.PointOfSaleName";
                        sql = string.Format(sql, string.Join("','", assignedPOS), outletName);
                        dt.Load(DataService.GetReader(new QueryCommand(sql)));
                        data.Load(dt);
                    }
                    else
                    {
                        sql = @"SELECT   POS.*
                                    FROM	PointOfSale POS
		                                    LEFT JOIN Outlet OU ON OU.OutletName = POS.OutletName
                                    WHERE	ISNULL(POS.Deleted,0) = 0 AND ISNULL(OU.Deleted,0) = 0
                                            AND (OU.OutletName = '{0}' OR '{0}' IN ('ALL','ALL - BreakDown',''))
                                    ORDER BY POS.PointOfSaleName";
                        sql = string.Format(sql, outletName);
                        dt.Load(DataService.GetReader(new QueryCommand(sql)));
                        data.Load(dt);
                        if(includeBreakDown)
                            data.Insert(0, new PointOfSale { PointOfSaleID = -1, PointOfSaleName = "ALL - BreakDown" });
                        if(includeALL)
                            data.Insert(0, new PointOfSale { PointOfSaleID = 0, PointOfSaleName = "ALL" });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }

            return data;
        }

        private static PointOfSaleCollection FetchByUserNamePerOutletAssigment(bool includeBreakDown, bool includeALL, string userName, string outletName)
        {
            PointOfSaleCollection data = new PointOfSaleCollection();

            try
            {
                UserMst um = new UserMst(userName + "");
                if (!um.IsNew && !um.Deleted.GetValueOrDefault(false))
                {
                    string sql = "";

                    DataTable dt = new DataTable();
                    if (!(um.AssignedOutlet + "").ToUpper().Equals("ALL") && !(um.AssignedOutlet + "").ToString().Equals(string.Empty))
                    {
                        string[] assignedPOS = um.AssignedOutletList;
                        sql = @"SELECT   POS.*
                                    FROM	PointOfSale POS
		                                    LEFT JOIN Outlet OU ON OU.OutletName = POS.OutletName
                                    WHERE	ISNULL(POS.Deleted,0) = 0 AND ISNULL(OU.Deleted,0) = 0
		                                    AND OU.OutletName IN ('{0}')
                                            AND (OU.OutletName = '{1}' OR '{1}' IN ('ALL','ALL - BreakDown',''))
                                    ORDER BY POS.PointOfSaleName";
                        sql = string.Format(sql, string.Join("','", assignedPOS), outletName);
                        dt.Load(DataService.GetReader(new QueryCommand(sql)));
                        data.Load(dt);

                        if ((um.AssignedPOS + "").ToUpper().Equals("") || (um.AssignedPOS + "").ToUpper().Equals("ALL"))
                        {
                            if (includeBreakDown)
                                data.Insert(0, new PointOfSale { PointOfSaleID = -1, PointOfSaleName = "ALL - BreakDown" });
                            if (includeALL)
                                data.Insert(0, new PointOfSale { PointOfSaleID = 0, PointOfSaleName = "ALL" });
                        }
                    }
                    else
                    {
                        sql = @"SELECT   POS.*
                                    FROM	PointOfSale POS
		                                    LEFT JOIN Outlet OU ON OU.OutletName = POS.OutletName
                                    WHERE	ISNULL(POS.Deleted,0) = 0 AND ISNULL(OU.Deleted,0) = 0
                                            AND (OU.OutletName = '{0}' OR '{0}' IN ('ALL','ALL - BreakDown',''))
                                    ORDER BY POS.PointOfSaleName";
                        sql = string.Format(sql, outletName);
                        dt.Load(DataService.GetReader(new QueryCommand(sql)));
                        data.Load(dt);
                        if (includeBreakDown)
                            data.Insert(0, new PointOfSale { PointOfSaleID = -1, PointOfSaleName = "ALL - BreakDown" });
                        if (includeALL)
                            data.Insert(0, new PointOfSale { PointOfSaleID = 0, PointOfSaleName = "ALL" });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }

            return data;
        }

        public static PointOfSaleCollection FetchAll(bool includeBreakDown, bool includeAll)
        {
            PointOfSaleCollection data = new PointOfSaleCollection();

            try
            {
                var theData = new PointOfSaleController().FetchAll()
                                                   .Where(o => o.Deleted.GetValueOrDefault(false) == false)
                                                   .OrderBy(o => o.PointOfSaleName)
                                                   .ToList()
                                                   .ToDataTable();
                if (includeBreakDown)
                    data.Add(new PointOfSale { PointOfSaleID = -1, PointOfSaleName = "ALL - BreakDown" });

                if (includeAll)
                    data.Add(new PointOfSale { PointOfSaleID = 0, PointOfSaleName = "ALL" });

                data.Load(theData);
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }

            return data;
        }

        public static PointOfSaleCollection FetchByUserNameForReport(bool includeBreakDown, bool includeALL, string userName, string outletName)
        {
            PointOfSaleCollection data = new PointOfSaleCollection();

            try
            {
                bool separateUserForReport = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.User.SeparateUserPerOutletPrivileges), false);
                bool showOutlet = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.User.ShowOutlet),false);
                bool showPOS = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.User.ShowPointOfSale),false);
                if (separateUserForReport)
                {
                    if (showPOS)
                        data = FetchByUserNamePerPOSAssignment(includeBreakDown, includeALL, userName, outletName);
                    else if (showOutlet)
                        data = FetchByUserNamePerOutletAssigment(includeBreakDown, includeALL, userName, outletName);
                    else
                        data = FetchByUserNamePerOutletAssigment(includeBreakDown, includeALL, userName, outletName);
                }
                else
                {
                    data = FetchAll(includeBreakDown, includeALL);
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }

            return data;
        }
    }
}
