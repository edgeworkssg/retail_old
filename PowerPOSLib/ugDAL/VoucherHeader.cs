using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic;
using System.Data;

namespace PowerPOS
{
    public partial class VoucherHeader
    {
        #region *) Custom Properties

        public string[] AssignedOutletList
        {
            get
            {
                string[] outlets = new string[1];
                try
                {
                    string outletStr = Outlet + "";
                    if (outletStr.ToUpper().Equals("ALL"))
                    {
                        string sql = @"SELECT  TOP 1 STUFF((SELECT DISTINCT ',' + OutletName
		                                          FROM	Outlet
		                                          WHERE	Deleted = 0
                                                  FOR XML PATH (''))
                                                  , 1, 1, '') AS OutletName
                                        FROM	Outlet OU 
                                        WHERE	OU.Deleted = 0";
                        DataTable dt = new DataTable();
                        dt.Load(DataService.GetReader(new QueryCommand(sql)));
                        if (dt.Rows.Count > 0)
                            outletStr = dt.Rows[0]["OutletName"] + "";
                    }
                    outlets = outletStr.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                }
                catch (Exception ex)
                {
                    Logger.writeLog(ex);
                }
                return outlets;
            }
            set
            {
                Outlet = string.Join(",", value);
            }
        }

        #endregion
    }
}
