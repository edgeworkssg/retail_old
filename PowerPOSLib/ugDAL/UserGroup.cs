using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SubSonic;

namespace PowerPOS
{
    public partial class UserGroup
    {
        public partial struct UserColumns
        {
            /// <summary>userfloat1</summary>
            public static string DiscountLimitPercent = @"userfloat1";

            /// <summary>Userfld1</summary>
            public static string AssignedOutlet = @"Userfld1";

            /// <summary>Userfld2</summary>
            public static string PriceRestrictedTo = @"Userfld2";
        }

        #region Custom Properties
        /// <summary>userfloat1</summary>
        public decimal? DiscountLimitPercent
        {
            get { return GetColumnValue<decimal?>(UserColumns.DiscountLimitPercent); }
            set { SetColumnValue(UserColumns.DiscountLimitPercent, value); }
        }

        public string AssignedOutlet
        {
            get { return GetColumnValue<string>(UserColumns.AssignedOutlet); }
            set { SetColumnValue(UserColumns.AssignedOutlet, value); }
        }
        public string[] AssignedOutletList
        {
            get
            {
                string[] outlets = new string[1];

                try
                {
                    string outletStr = AssignedOutlet + "";
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
                AssignedOutlet = string.Join(",", value);
            }
        }

        public string PriceRestrictedTo
        {
            get { return GetColumnValue<string>(UserColumns.PriceRestrictedTo); }
            set { SetColumnValue(UserColumns.PriceRestrictedTo, value); }
        }

        #endregion
    }
}
