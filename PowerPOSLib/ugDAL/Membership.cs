using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic;
using System.Data;

namespace PowerPOS
{
    public partial class Membership
    {
        public partial struct UserColumns
        {
            /// <summary>Userfld1</summary>
            public static string Child1Surname = @"Userfld1";
            /// <summary>Userfld2</summary>
            public static string Child1GivenName = @"Userfld2";
            /// <summary>Userfld3</summary>
            public static string Child1DateOfBirth = @"Userfld3";
            /// <summary>Userfld4</summary>
            public static string Child2Surname = @"Userfld4";
            /// <summary>Userfld5</summary>
            public static string Child2GivenName = @"Userfld5";
            /// <summary>Userfld6</summary>
            public static string Child2DateOfBirth = @"Userfld6";
            /// <summary>Userfld7</summary>
            public static string KnowFrom= @"Userfld7";
            /// <summary>Userfld8</summary>
            public static string LastSyncPointsDate = @"Userfld8";
            /// <summary>Userfld9</summary>
            public static string PassCode = @"Userfld9";
            /// <summary>Userfld10</summary>
            public static string TagNo = @"Userfld10";

            /// <summary>Userint1</summary>
            public static string PointOfSaleID = @"Userint1";

            /// <summary>Userflag1</summary>
            public static string IsCreatedInWeb = @"Userflag1";
        }

        #region Custom Properties

        /// <summary>
        /// IsCreatedInWeb
        /// </summary>
        public bool IsCreatedInWeb
        {
            get { return GetColumnValue<bool?>(UserColumns.IsCreatedInWeb).GetValueOrDefault(false); }
            set { SetColumnValue(UserColumns.IsCreatedInWeb, value); }
        }

        /// <summary>
        /// PointOfSaleID
        /// </summary>
        public int PointOfSaleID
        {
            get { return GetColumnValue<int?>(UserColumns.PointOfSaleID).GetValueOrDefault(0); }
            set { SetColumnValue(UserColumns.PointOfSaleID, value); }
        }

        /// <summary>
        /// Line Project Name for each OrderHdr
        /// </summary>
        public string Child1Surname
        {
            get { return GetColumnValue<string>(UserColumns.Child1Surname); }
            set { SetColumnValue(UserColumns.Child1Surname, value); }
        }
        /// <summary>
        /// Cash And Carry (CASH_CARRY) or Pre-Order (PREORDER)
        /// </summary>
        public string Child1GivenName
        {
            get { return GetColumnValue<string>(UserColumns.Child1GivenName); }
            set { SetColumnValue(UserColumns.Child1GivenName, value); }
        }
        /// <summary>
        /// Is the Order has been exported to integration file
        /// </summary>
        public string Child1DateOfBirth
        {
            get { return GetColumnValue<string>(UserColumns.Child1DateOfBirth); }
            set { SetColumnValue(UserColumns.Child1DateOfBirth, value); }
        }

        /// <summary>
        /// Line Project Name for each OrderHdr
        /// </summary>
        public string Child2Surname
        {
            get { return GetColumnValue<string>(UserColumns.Child2Surname); }
            set { SetColumnValue(UserColumns.Child2Surname, value); }
        }
        /// <summary>
        /// Cash And Carry (CASH_CARRY) or Pre-Order (PREORDER)
        /// </summary>
        public string Child2GivenName
        {
            get { return GetColumnValue<string>(UserColumns.Child2GivenName); }
            set { SetColumnValue(UserColumns.Child2GivenName, value); }
        }
        /// <summary>
        /// Is the Order has been exported to integration file
        /// </summary>
        public string Child2DateOfBirth
        {
            get { return GetColumnValue<string>(UserColumns.Child2DateOfBirth); }
            set { SetColumnValue(UserColumns.Child2DateOfBirth, value); }
        }

        /// <summary>
        /// Know From
        /// </summary>
        public string KnowFrom
        {
            get { return GetColumnValue<string>(UserColumns.KnowFrom); }
            set { SetColumnValue(UserColumns.KnowFrom, value); }
        }

        /// <summary>
        /// LastSyncPointsDate
        /// </summary>
        public string LastSyncPointsDate
        {
            get { return GetColumnValue<string>(UserColumns.LastSyncPointsDate); }
            set { SetColumnValue(UserColumns.LastSyncPointsDate, value); }
        }

        /// <summary>
        /// PassCode
        /// </summary>
        public string PassCode
        {
            get { return GetColumnValue<string>(UserColumns.PassCode); }
            set { SetColumnValue(UserColumns.PassCode, value); }
        }

        /// <summary>
        /// Tag No
        /// </summary>
        public string TagNo
        {
            get { return GetColumnValue<string>(UserColumns.TagNo); }
            set { SetColumnValue(UserColumns.TagNo, value); }
        }

        #endregion

        public DataTable GetPastTransaction(int TOP)
        {
            return GetPastTransaction(TOP, false);
        }

        public DataTable GetPastTransaction(int TOP, bool excludeSystemItem)
        {
            string sql = @"SELECT TOP {0}
		                         OD.OrderHdrID
		                        ,OH.OrderDate
		                        ,OD.Amount
		                        ,I.ItemName + ' ' + ISNULL(OD.Remark,'') AS ItemName 
		                        ,OD.ItemNo
		                        ,OD.Quantity
                                ,OD.UnitPrice
		                        ,UM.UserName AS SalesPerson
                                ,ISNULL(UM.DisplayName,'') as SalesPersonDisplayName
		                        ,OD.userfld20 AS Assistant
                                ,ISNULL(UM2.DisplayName,'') as AssistantDisplayName
		                        ,OU.OutletName
		                        ,POS.PointOfSaleName
		                        ,OH.CashierID
		                        ,OD.Remark
                        FROM	OrderHdr OH
		                        INNER JOIN PointOfSale POS ON POS.PointOfSaleID = OH.PointOfSaleID
		                        INNER JOIN Outlet OU ON OU.OutletName= POS.OutletName
		                        INNER JOIN OrderDet OD ON OH.OrderHdrID = OD.OrderHdrID
		                        INNER JOIN Item I ON OD.ItemNo = I.ItemNo
		                        INNER JOIN SalesCommissionRecord SCR ON OH.OrderHdrID = SCR.OrderHdrID
                                LEFT OUTER JOIN UserMst UM ON UM.UserName = ISNULL(NULLIF(OD.userfld1,''),SCR.SalesPersonID)
								LEFT OUTER JOIN UserMst UM2 ON (UM2.UserName = OD.userfld20 AND ISNULL(OD.userfld20,'') <> '')
                        WHERE 1 = 1
	                          AND MembershipNo = '{1}'  
	                          AND I.ItemNo <> 'INST_PAYMENT' ";

            if (excludeSystemItem)
            {
                sql += @"AND I.CategoryName <> 'SYSTEM' ";
            }
            
            sql += @" ORDER BY OrderDate DESC";
            sql = string.Format(sql, TOP, MembershipNo);
            DataTable dt = new DataTable();
            dt.Load(DataService.GetReader(new QueryCommand(sql)));
            return dt;
        }

        public DataTable GetPastTransaction(DateTime startDate, DateTime endDate)
        {
            return GetPastTransaction(startDate, endDate, false);
        }

        public DataTable GetPastTransaction(DateTime startDate, DateTime endDate, bool excludeSystemItem)
        {
            string sql = @"SELECT OD.OrderHdrID
		                        ,OH.OrderDate
		                        ,OD.Amount
		                        ,I.ItemName
		                        ,OD.ItemNo
		                        ,OD.Quantity
		                        ,SCR.SalesPersonID AS SalesPerson
		                        ,OD.userfld1 AS Assistant
		                        ,OU.OutletName
		                        ,POS.PointOfSaleName
		                        ,OH.CashierID
		                        ,OD.Remark
                        FROM	OrderHdr OH
		                        INNER JOIN PointOfSale POS ON POS.PointOfSaleID = OH.PointOfSaleID
		                        INNER JOIN Outlet OU ON OU.OutletName= POS.OutletName
		                        INNER JOIN OrderDet OD ON OH.OrderHdrID = OD.OrderHdrID
		                        INNER JOIN Item I ON OD.ItemNo = I.ItemNo
		                        INNER JOIN SalesCommissionRecord SCR ON OH.OrderHdrID = SCR.OrderHdrID
                        WHERE 1 = 1
	                          AND MembershipNo = '{0}'  
                              AND OH.OrderDate BETWEEN '{1}' AND '{2}' 
	                          AND I.ItemNo <> 'INST_PAYMENT' ";

            if (excludeSystemItem)
            {
                sql += @"AND I.CategoryName <> 'SYSTEM' ";
            }

            sql += @" ORDER BY OrderDate DESC";
            sql = string.Format(sql, MembershipNo, startDate.ToString("yyyy-MM-dd HH:mm:ss.fff"), endDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            DataTable dt = new DataTable();
            dt.Load(DataService.GetReader(new QueryCommand(sql)));
            return dt;
        }

        public DataTable GetPastTransaction(int TOP, string OutletName)
        {
            string sql = @"SELECT TOP {0}
		                         OD.OrderHdrID
		                        ,OH.OrderDate
		                        ,OD.Amount
		                        ,I.ItemName + ' ' + ISNULL(OD.Remark,'') AS ItemName 
		                        ,OD.ItemNo
		                        ,OD.Quantity
                                ,OD.UnitPrice
		                        ,UM.UserName AS SalesPerson
                                ,ISNULL(UM.DisplayName,'') as SalesPersonDisplayName
		                        ,OD.userfld20 AS Assistant
                                ,ISNULL(UM2.DisplayName,'') as AssistantDisplayName
		                        ,OU.OutletName
		                        ,POS.PointOfSaleName
		                        ,OH.CashierID
		                        ,OD.Remark
                        FROM	OrderHdr OH
		                        INNER JOIN PointOfSale POS ON POS.PointOfSaleID = OH.PointOfSaleID
		                        INNER JOIN Outlet OU ON OU.OutletName = POS.OutletName
		                        INNER JOIN OrderDet OD ON OH.OrderHdrID = OD.OrderHdrID
		                        INNER JOIN Item I ON OD.ItemNo = I.ItemNo
		                        INNER JOIN SalesCommissionRecord SCR ON OH.OrderHdrID = SCR.OrderHdrID
                                LEFT OUTER JOIN UserMst UM ON UM.UserName = ISNULL(NULLIF(OD.userfld1,''),SCR.SalesPersonID)
								LEFT OUTER JOIN UserMst UM2 ON (UM2.UserName = OD.userfld20 AND ISNULL(OD.userfld20,'') <> '')
                        WHERE 1 = 1
                              AND OH.IsVoided = 0 
	                          AND MembershipNo = '{1}'  
	                          AND I.ItemNo <> 'INST_PAYMENT'  
                              AND I.CategoryName <> 'SYSTEM' ";

            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Membership.AllowShowSalesOutlet), false))
            {
                sql += @" AND OU.OutletName = '"+ OutletName +"' ";
            }

            sql += @" ORDER BY OrderDate DESC";
            sql = string.Format(sql, TOP, MembershipNo);
            DataTable dt = new DataTable();
            dt.Load(DataService.GetReader(new QueryCommand(sql)));
            return dt;
        }

        public MembershipPointCollection GetPoints(string ItemNo)
        {
            try
            {
                string sqlFetch =
                    "SELECT * " +
                    "FROM MembershipPoints " +
                    "WHERE MembershipNo = '" + MembershipNo + "' " +
                        "AND " + MembershipPoint.UserColumns.PackageItemNo + " = '" + ItemNo + "' ";

                QueryCommand Cmd = new QueryCommand(sqlFetch);

                MembershipPointCollection Output = new MembershipPointCollection();
                Output.LoadAndCloseReader(DataService.GetReader(Cmd));
                return Output;
            }
            catch (Exception X)
            {
                Logger.writeLog(X);
                return null;
            }
        }

        public void AdjustPoint(string ItemNo, decimal NewPoint, string UserName)
        {
            if (IsNew || !IsLoaded) throw new Exception("(error)Membership is not registered. Please create the member.");

            Item LoadedItem = new Item(ItemNo);
            if (LoadedItem.IsNew || !LoadedItem.IsLoaded) throw new Exception("(error)Item is not registered. Please add the item");
            if (LoadedItem.PointGetMode == null || (LoadedItem.PointGetMode != Item.PointMode.Dollar && LoadedItem.PointGetMode != Item.PointMode.Times)) throw new Exception("(error)This item is neither Point/Package. Please check if you choose the correct Item");

            MembershipPointCollection loadedPoints = GetPoints(ItemNo);
            if (loadedPoints == null || loadedPoints.Count < 1) throw new Exception("(error)Points is not registered. Please add the points from POS.");

            MembershipPoint UpdatedPoint = loadedPoints[0];

            decimal diff = NewPoint - UpdatedPoint.Points;
            if (diff == 0) throw new Exception ("(error)New point is no different with current point");

            UpdatedPoint.Points = NewPoint;

            PointAllocationLog newLog = new PointAllocationLog();
            newLog.AllocationDate = DateTime.Now;
            newLog.OrderHdrID = "Adjustment";
            newLog.MembershipNo = MembershipNo;
            newLog.PointAllocated = diff;
            newLog.UniqueID = Guid.NewGuid();
            newLog.PointItemNo = LoadedItem.ItemNo;
            newLog.SalesPerson = UserName;

            QueryCommandCollection cmds = new QueryCommandCollection();
            cmds.Add(UpdatedPoint.GetSaveCommand());
            cmds.Add(newLog.GetSaveCommand());

            DataService.ExecuteTransaction(cmds);
        }
        public decimal GetTotalSpent()
        {
            string sqlFetch = string.Format("SELECT SUM(ReceiptDet.Amount) AS TotalAmount FROM OrderHdr " +
                 "LEFT OUTER JOIN ReceiptHdr ON OrderHdr.OrderHdrID = ReceiptHdr.OrderHdrID " +
                 "RIGHT OUTER JOIN ReceiptDet ON ReceiptHdr.ReceiptHdrID = ReceiptDet.ReceiptHdrID " +
                 "WHERE(OrderHdr.MembershipNo = '{0}')", this.MembershipNo);
            var result = DataService.ExecuteScalar(new QueryCommand(sqlFetch));
            decimal total = decimal.Zero;
            if (!String.IsNullOrEmpty(result.ToString()))
            {
                total = Convert.ToDecimal(result);
            }
            return total;
        }
    }
}
