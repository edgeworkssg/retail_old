using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic;
using System.Data;

namespace PowerPOS
{
    public partial class OutletController
    {
        private static OutletCollection FetchByUserNamePerPOSAssignment(bool includeBreakDown, bool includeALL, string userName)
        {
            OutletCollection data = new OutletCollection();
            try
            {
                UserMst um = new UserMst(userName);
                if (!um.IsNew && !um.Deleted.GetValueOrDefault(false))
                {
                    if (!(um.AssignedPOS + "").ToUpper().Equals("ALL") && !(um.AssignedPOS + "").ToString().Equals(string.Empty))
                    {
                        string[] assignedPOS = um.AssignedPOSList;
                        string sql = @"SELECT  DISTINCT OU.*
                                    FROM	PointOfSale POS
		                                    LEFT JOIN Outlet OU ON OU.OutletName = POS.OutletName
                                    WHERE	ISNULL(POS.Deleted,0) = 0 AND ISNULL(OU.Deleted,0) = 0
		                                    AND POS.PointOfSaleID IN ('{0}')
                                    ORDER BY OU.OutletName";
                        sql = string.Format(sql, string.Join("','", assignedPOS));
                        DataTable dt = new DataTable();
                        dt.Load(DataService.GetReader(new QueryCommand(sql)));
                        data.Load(dt);
                    }
                    else
                    {
                        //data = new OutletController().FetchAll().Where(Outlet.Columns.Deleted, Comparison.Equals, false);
                        string sql2 = @"SELECT  OU.*
                                    FROM	Outlet OU
                                    WHERE	ISNULL(OU.Deleted,0) = 0
		                            ORDER BY OU.OutletName";
                        DataTable dt = new DataTable();
                        dt.Load(DataService.GetReader(new QueryCommand(sql2)));
                        data.Load(dt);
                        if (includeBreakDown)
                            data.Insert(0, new Outlet { OutletName = "ALL - BreakDown" });
                        if(includeALL)
                            data.Insert(0, new Outlet { OutletName = "ALL" });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
            return data;
        }

        private static OutletCollection FetchByUserNamePerOutletAssignment(bool includeBreakDown, bool includeALL, string userName)
        {
            OutletCollection data = new OutletCollection();
            try
            {
                UserMst um = new UserMst(userName);
                if (!um.IsNew && !um.Deleted.GetValueOrDefault(false))
                {
                    if (!(um.AssignedOutlet + "").ToUpper().Equals("ALL") && !(um.AssignedOutlet + "").ToString().Equals(string.Empty))
                    {
                        string[] assignedPOS = um.AssignedOutletList;
                        string sql = @"SELECT  OU.*
                                    FROM	Outlet OU
                                    WHERE	ISNULL(OU.Deleted,0) = 0
		                                    AND OU.OutletName IN ('{0}')
                                    ORDER BY OU.OutletName";
                        sql = string.Format(sql, string.Join("','", assignedPOS));
                        DataTable dt = new DataTable();
                        dt.Load(DataService.GetReader(new QueryCommand(sql)));
                        data.Load(dt);
                    }
                    else
                    {
                        string sql2 = @"SELECT  OU.*
                                    FROM	Outlet OU
                                    WHERE	ISNULL(OU.Deleted,0) = 0
		                            ORDER BY OU.OutletName";
                        //data = new OutletController().FetchAll().Where(Outlet.Columns.Deleted, Comparison.Equals, false);
                        DataTable dt = new DataTable();
                        dt.Load(DataService.GetReader(new QueryCommand(sql2)));
                        data.Load(dt);
                        if (includeBreakDown)
                            data.Insert(0, new Outlet { OutletName = "ALL - BreakDown" });
                        if (includeALL)
                            data.Insert(0, new Outlet { OutletName = "ALL" });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
            return data;
        }

        public static OutletCollection FetchAll(bool includeBreakDown, bool includeAll)
        {
            OutletCollection data = new OutletCollection();

            try
            {
                var theData = new OutletController().FetchAll()
                                                   .Where(o => o.Deleted.GetValueOrDefault(false) == false)
                                                   .OrderBy(o => o.OutletName)
                                                   .ToList()
                                                   .ToDataTable();
                if (includeBreakDown)
                    data.Add(new Outlet { OutletName = "ALL - BreakDown" });
                
                if (includeAll)
                    data.Add(new Outlet { OutletName = "ALL" });

                data.Load(theData);
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }

            return data;
        }

        public static OutletCollection FetchByUserNameForReport(bool includeBreakDown, bool includeALL, string userName)
        {
            OutletCollection data = new OutletCollection();

            try
            {
                bool separateUserForReport = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.User.SeparateUserPerOutletPrivileges), false);
                bool showOutlet = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.User.ShowOutlet), false);
                bool showPOS = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.User.ShowPointOfSale), false);
                if (separateUserForReport)
                {
                    if (showOutlet)
                        data = FetchByUserNamePerOutletAssignment(includeBreakDown, includeALL, userName);
                    else if (showPOS)
                        data = FetchByUserNamePerPOSAssignment(includeBreakDown, includeALL, userName);
                    else
                        data = FetchByUserNamePerOutletAssignment(includeBreakDown, includeALL, userName);
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
