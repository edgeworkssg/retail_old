using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Data;
using SubSonic;
using System.IO;

namespace PowerPOS
{
    public partial class UserMst
    {
        public partial struct UserColumns
        {
            /// <summary>userfld1</summary>
            public static string AssignedOutlet = @"userfld1";

            /// <summary>userfld2</summary>
            public static string GoogleCalendarID2 = @"userfld2";
            
            /// <summary>userfld3</summary>
            public static string GoogleCalendarPrivateKey = @"userfld3";
            
            /// <summary>userfld4</summary>
            public static string GoogleCalendarID1 = @"userfld4";

            /// <summary>userfld5</summary>
            public static string AssignedPOS = @"userfld5";

            /// <summary>userfld6</summary>
            public static string UserToken = @"userfld6";

            /// <summary>userfld7</summary>
            public static string BarcodeToken = @"userfld7";

            /// <summary>userfld8</summary>
            public static string Email = @"userfld8";

            /// <summary>userfld9</summary>
            public static string AuthToken = @"userfld9";

            /// <summary>userint1</summary>
            public static string PointOfSaleID = @"userint1";

            /// <summary>userfloat1</summary>
            public static string BasicSalary = @"userfloat1";

            /// <summary>userfloat2</summary>
            public static string OtherAllowance = @"userfloat2";

            /// <summary>userflag1</summary>
            public static string IsSupplier = @"userflag1";

            /// <summary>userflag2</summary>
            public static string IsRestrictedSupplierList = @"userflag2";
        }

        #region *) Custom Properties

        /// <summary>userfld1 + userfld2</summary>
        public string GoogleCalendarID
        {
            get { return GetColumnValue<string>(UserColumns.GoogleCalendarID1) + GetColumnValue<string>(UserColumns.GoogleCalendarID2); }
            set
            {
                string Col1 = "";
                string Col2 = "";

                if (value.Length <= 50)
                {
                    Col1 = value;
                }
                else
                {
                    Col1 = value.Substring(0, 50);
                    Col2 = value.Substring(50);
                }

                SetColumnValue(UserColumns.GoogleCalendarID1, Col1);
                SetColumnValue(UserColumns.GoogleCalendarID2, Col2);
            }
        }

        /// <summary>userfld3 - Currently Not being used</summary>
        public string GoogleCalendarPrivateKey
        {
            get { return GetColumnValue<string>(UserColumns.GoogleCalendarPrivateKey); }
            set { SetColumnValue(UserColumns.GoogleCalendarPrivateKey, value); }
        }

        /// <summary>Format userfld1 until userfld3 to get Calendar's URL</summary>
        public string GoogleCalendarURL
        {
            get
            {
                string Output
                    = "http://www.google.com/calendar/feeds/"
                    + GoogleCalendarID
                    + "/private-"
                    + GoogleCalendarPrivateKey
                    + "/full";

                return Output;
            }
        }

        public string AssignedOutlet
        {
            get { return GetColumnValue<string>(UserColumns.AssignedOutlet); }
            set { SetColumnValue(UserColumns.AssignedOutlet, value); }
        }

        public string AssignedPOS
        {
            get { return GetColumnValue<string>(UserColumns.AssignedPOS); }
            set { SetColumnValue(UserColumns.AssignedPOS, value); }
        }

        public string[] AssignedOutletList
        {
            get
            {
                string[] outlets = new string[1];
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.User.UseUserGroupOutletAssignment), false))
                {
                    outlets = this.UserGroup.AssignedOutletList;
                }
                else
                {
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
                }
                return outlets;
            }
            set
            {
                AssignedOutlet = string.Join(",", value);
            }
        }

        public string[] AssignedPOSList
        {
            get
            {
                string[] pointOfSales = new string[1];
                try
                {
                    string posStr = AssignedPOS + "";
                    if (posStr.ToUpper().Equals("ALL"))
                    {
                        string sql = @"SELECT  TOP 1 STUFF((SELECT DISTINCT ',' + CAST(PointOfSaleID AS VARCHAR(20))
		                                          FROM	PointOfSale
		                                          WHERE	ISNULL(Deleted,0) = 0
                                                  FOR XML PATH (''))
                                                  , 1, 1, '') AS PointOfSaleID
                                        FROM	PointOfSale POS 
                                        WHERE	ISNULL(POS.Deleted,0) = 0";
                        DataTable dt = new DataTable();
                        dt.Load(DataService.GetReader(new QueryCommand(sql)));
                        if (dt.Rows.Count > 0)
                            posStr = dt.Rows[0]["PointOfSaleID"] + "";
                    }
                    pointOfSales = posStr.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                }
                catch (Exception ex)
                {
                    Logger.writeLog(ex);
                }
                return pointOfSales;
            }
            set
            {
                AssignedPOS = string.Join(",", value);
            }
        }

        public int[] AssignedOutletGroupList
        {
            get
            {
                List<int> outletGroups = new List<int>();
                string[] outlets = AssignedOutletList;

                if (outlets.Length > 0)
                {
                    for (int i = 0; i < outlets.Length; i++)
                    {
                        outlets[i] = "'" + outlets[i] + "'";
                    }

                    string sql = @"
                                SELECT og.OutletGroupID, og.OutletGroupName, ISNULL(ou1.OutletCount, 0) AS TotalOutletCount, ISNULL(ou2.OutletCount, 0) AS AssignedOutletCount
                                FROM OutletGroup og
                                    LEFT JOIN (
                                        SELECT OutletGroupID, COUNT(*) OutletCount FROM Outlet WHERE Deleted = 0 GROUP BY OutletGroupID
                                    ) ou1 ON og.OutletGroupID = ou1.OutletGroupID
                                    LEFT JOIN (
                                        SELECT OutletGroupID, COUNT(*) OutletCount FROM Outlet WHERE Deleted = 0 AND OutletName IN ({0}) GROUP BY OutletGroupID
                                    ) ou2 ON og.OutletGroupID = ou2.OutletGroupID
                                WHERE og.Deleted = 0 
                                    AND ISNULL(ou1.OutletCount, 0) = ISNULL(ou2.OutletCount, 0)
                                    AND ISNULL(ou1.OutletCount, 0) > 0
                                ORDER BY og.OutletGroupName
                              ";
                    sql = string.Format(sql, string.Join(",", outlets));
                    DataTable dt = new DataTable();
                    dt.Load(DataService.GetReader(new QueryCommand(sql, "PowerPOS")));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        outletGroups.Add(dt.Rows[i]["OutletGroupID"].ToString().GetIntValue());
                    }
                }

                return outletGroups.ToArray();
            }
        }

        private Image _image;
        public Image Image
        {
            get
            {
                if (_image == null)
                {
                    if (ImageData != null)
                        _image = Image.FromStream(new MemoryStream(ImageData));
                }
                return _image;
            }
            set
            {
                _image = value;
                using (var stream = new MemoryStream())
                {
                    value.Save(stream, ImageFormat.Png);
                    ImageData = stream.ToArray();
                }
            }
        }

        /// <summary>
        /// PointOfSaleID
        /// </summary>
        public int PointOfSaleID
        {
            get { return GetColumnValue<int>(UserColumns.PointOfSaleID); }
            set { SetColumnValue(UserColumns.PointOfSaleID, value); }
        }

        /// <summary>
        /// UserToken, for auto-login
        /// </summary>
        public string UserToken
        {
            get { return GetColumnValue<string>(UserColumns.UserToken); }
            set { SetColumnValue(UserColumns.UserToken, value); }
        }

        /// <summary>
        /// UserToken, for auto-login
        /// </summary>
        public string BarcodeToken
        {
            get { return GetColumnValue<string>(UserColumns.BarcodeToken); }
            set { SetColumnValue(UserColumns.BarcodeToken, value); }
        }

        /// <summary>
        /// Email
        /// </summary>
        public string Email
        {
            get { return GetColumnValue<string>(UserColumns.Email); }
            set { SetColumnValue(UserColumns.Email, value); }
        }

        /// <summary>
        /// AuthToken
        /// </summary>
        public string AuthToken
        {
            get { return GetColumnValue<string>(UserColumns.AuthToken); }
            set { SetColumnValue(UserColumns.AuthToken, value); }
        }

        /// <summary>
        /// Basic Salary
        /// </summary>
        public decimal BasicSalary
        {
            get { return GetColumnValue<decimal?>(UserColumns.BasicSalary).GetValueOrDefault(0); }
            set { SetColumnValue(UserColumns.BasicSalary, value); }
        }

        /// <summary>
        /// Other Allowance
        /// </summary>
        public decimal OtherAllowance
        {
            get { return GetColumnValue<decimal?>(UserColumns.OtherAllowance).GetValueOrDefault(0); }
            set { SetColumnValue(UserColumns.OtherAllowance, value); }
        }

        #endregion

        /// <summary>
        /// Is Supplier (userflag1)
        /// </summary>
        public bool IsSupplier
        {
            get { return GetColumnValue<bool>(UserColumns.IsSupplier); }
            set { SetColumnValue(UserColumns.IsSupplier, value); }
        }

        /// <summary>
        /// Is Restricted Supplier List (userflag2)
        /// </summary>
        public bool IsRestrictedSupplierList
        {
            get { return GetColumnValue<bool>(UserColumns.IsRestrictedSupplierList); }
            set { SetColumnValue(UserColumns.IsRestrictedSupplierList, value); }
        }

        #region *) Custom Method

        public Uri GetGoogleCalendarURI(Color DisplayedColor)
        {
            if (string.IsNullOrEmpty(GoogleCalendarID)) return null;

            string BasicURI = "https://www.google.com/calendar/embed?";
            string calTitle = null;

            string gURI =
                BasicURI
                + (string.IsNullOrEmpty(calTitle) ? "showTitle=0&" : "title=" + calTitle + "&")
                + "mode=WEEK&";

            gURI = gURI + "src=" + GoogleCalendarID + "&color=%23" + DisplayedColor.ToArgb().ToString("X").PadLeft(6, '0');

            return new Uri(gURI);
        }

        public bool IsAbleToGiveDiscount(decimal PercentageToBeChecked)
        {
            PowerPOS.UserGroup theGroup = UserGroup;
            
            if (!theGroup.DiscountLimitPercent.HasValue) return true;

            return (PercentageToBeChecked <= theGroup.DiscountLimitPercent.Value);
        }

        public bool IsHavePrivilegesFor(string privilegesName)
        {
            bool result = false;

            try
            {
                string sql =  @"SELECT  CAST(CASE WHEN COUNT(*) = 0 THEN 0 ELSE 1 END AS BIT) IsHavePrivilege
                                FROM	UserMst UM
		                                LEFT JOIN UserGroup UG ON UG.GroupID = UM.GroupName
		                                LEFT JOIN GroupUserPrivilege GUP ON GUP.GroupID = UG.GroupID
		                                LEFT JOIN UserPrivilege UP ON UP.UserPrivilegeID = GUP.UserPrivilegeID
                                WHERE	ISNULL(UM.Deleted,0) = 0
		                                AND ISNULL(UG.Deleted,0) = 0 
		                                AND ISNULL(GUP.Deleted,0) = 0
		                                AND ISNULL(UP.Deleted,0) = 0
		                                AND UM.UserName = '{0}'
		                                AND UP.PrivilegeName = '{1}'";
                sql = string.Format(sql, this.UserName, privilegesName);
                DataTable dt = new DataTable();
                dt.Load(DataService.GetReader(new QueryCommand(sql)));
                if (dt.Rows.Count > 0)
                    result = (bool)dt.Rows[0]["IsHavePrivilege"];
            }
            catch (Exception ex)
            {
                result = false;
                Logger.writeLog(ex);
            }

            return result;
        }

        public DataTable GetSupplierList()
        {
            try
            {
                string sql = "select SupplierID, SupplierName, ISNULL(Deleted, 0) AS Deleted from Supplier where ISNULL(Deleted, 0) = 0 and ISNULL(" + Supplier.UserColumns.LinkedUser + ",'') = '" + UserName + "' order by SupplierName";
                DataSet ds = DataService.GetDataSet(new QueryCommand(sql, "PowerPOS"));
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return new DataTable();
            }
        }

        #endregion
    }
}
