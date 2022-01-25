using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Transactions;
using System.Collections;
using PowerPOS.Container;
using SubSonic;
using System.ComponentModel;
namespace PowerPOS
{
    /// <remarks>
    ///     *) Fully Migrated
    /// </remarks>
    public partial class PackageController
    {
        /// <summary>
        /// Will be used for next version - To change the dataTable to array specific
        /// </summary>
        private struct PointInformations
        {
            public string PointID;
            public string ItemNo;
            public decimal Points;
            public PointInformations(string iPointID, string iItemNo, decimal iPoints)
            {
                PointID = iPointID;
                ItemNo = iItemNo;
                Points = iPoints;
            }
        }


        public static bool UpdateAll(DataTable PointData, string OrderHdrID, DateTime TransactionDate
            , int ValidPeriods, string MembershipNo, string SalesPersonID, string UserName
            , out decimal InitialPoint, out decimal DiffPoint, out string Status)
        {
            InitialPoint = 0;
            DiffPoint = 0;
            Status = "";

            try
            {
                DateTime StartValidPeriod, EndValidPeriod;
                #region *) Initialize: Set StartValidPeriod & EndValidPeriod
                StartValidPeriod = TransactionDate;
                if (ValidPeriods < 1)
                {   /// Valid Periods = 100 Years
                    EndValidPeriod = StartValidPeriod.AddYears(100).AddMilliseconds(-1);
                }
                else
                {   /// From TransactionDate till Expiry
                    EndValidPeriod = StartValidPeriod.AddMonths(ValidPeriods).AddMilliseconds(-1);
                }
                #endregion

                #region *) Initialize: Set InitialPoint (Dollar Type only)
                string GetInitialPoint =
                   "SELECT ISNULL(SUM(Points),0) FROM MembershipPoints " +
                   "WHERE StartValidPeriod <= @StartDate AND EndValidPeriod >= @EndDate " +
                       "AND MembershipNo = @MembershipNo AND userfld2 = @PointType";
                QueryCommand CmdInitialPoint = new QueryCommand(GetInitialPoint);
                CmdInitialPoint.AddParameter("@StartDate", TransactionDate, DbType.DateTime);
                CmdInitialPoint.AddParameter("@EndDate", TransactionDate, DbType.DateTime);
                CmdInitialPoint.AddParameter("@MembershipNo", MembershipNo, DbType.String);
                CmdInitialPoint.AddParameter("@PointType", Item.PointMode.Dollar, DbType.String);
                InitialPoint = (decimal)SubSonic.DataService.ExecuteScalar(CmdInitialPoint);
                #endregion

                QueryCommandCollection Cmds = new QueryCommandCollection();
                foreach (DataRow Rw in PointData.Rows)
                {
                    decimal OneDiffPoint = 0;
                    #region *) Initialize: Parse OneDiffPoint (Total Points changed for 1 transaction)
                    if (!decimal.TryParse(Rw[1].ToString(), out OneDiffPoint))
                        throw new Exception("(warning)Cannot parse Point Value for Package " + Rw[0].ToString());
                    #endregion

                    #region *) Core: If PointType == Dollar, append to DiffPoint
                    if (Rw[2].ToString() == Item.PointMode.Dollar)
                        DiffPoint += OneDiffPoint;
                    #endregion

                    if (OneDiffPoint >= 0)
                    {
                        #region *) Core: Upsert MembershipPoint
                        string AddQuery =
                            "IF(( " +
                                "SELECT COUNT(*) " +
                                "FROM MembershipPoints " +
                                "WHERE MembershipNo = @MembershipNo " +
                                    "AND StartValidPeriod <= @TransactionDate " +
                                    "AND EndValidPeriod >= @TransactionDate " +
                                    "AND " + MembershipPoint.UserColumns.PackageItemNo + " = @ItemNo " +
                                ")>0) " +
                            "BEGIN " +
                                "UPDATE MembershipPoints " +
                                "SET Points = Points + @DiffPoints, EndValidPeriod=@EndValidPeriod,ModifiedOn = GETDATE(), ModifiedBy = @UserName " +
                                "WHERE PointID = " +
                                    "(SELECT TOP 1 PointID " +
                                    "FROM MembershipPoints " +
                                    "WHERE MembershipNo = @MembershipNo " +
                                        "AND StartValidPeriod <= @TransactionDate " +
                                        "AND EndValidPeriod >= @TransactionDate " +
                                        "AND " + MembershipPoint.UserColumns.PackageItemNo + " = @ItemNo " +
                                        //"AND Points <> 0 " +
                                    "ORDER BY StartValidPeriod,PointID) " +
                            "END " +
                            "ELSE " +
                            "BEGIN " +
                                "INSERT INTO [MembershipPoints] " +
                                "([StartValidPeriod],[EndValidPeriod],[MembershipNo],[Points],[CreatedOn],[CreatedBy],[ModifiedOn],[ModifiedBy],[" + MembershipPoint.UserColumns.PackageItemNo + "],[" + MembershipPoint.UserColumns.PackageType + "],[" + MembershipPoint.UserColumns.CourseBreakdownPrice + "]) " +
                                "VALUES( " +
                            //"SELECT " +
                                "@StartValidPeriod,@EndValidPeriod,@MembershipNo,@DiffPoints,@TransactionDate,@UserName,GETDATE(),@UserName,@ItemNo,@PointType,@PointUnitPrice" +
                                    //"(SELECT ISNULL(MAX(" + Item.UserColumns.PointUnitPrice + "),0) FROM Item WHERE [ItemNo] = @ItemNo) " +
                                ") " +
                            "END";

                        #region *) Assign Breakdown Price

                        decimal pointUnitPrice = 0;
                        try
                        {
                            string itemNo = Rw[0].ToString();
                            string pointType = Rw[2].ToString();
                            if (pointType == "T")
                            {
                                if (itemNo.Contains("|OPP|"))
                                {
                                    pointUnitPrice = itemNo.Split('|')[2].Substring(0, 12).GetDecimalValue();
                                    pointUnitPrice = pointUnitPrice / OneDiffPoint;
                                }
                                else
                                {
                                    Item theItem = new Item(Item.Columns.ItemNo, itemNo);
                                    pointUnitPrice = theItem.PointUnitPrice.GetValueOrDefault(0);
                                }
                            }
                        }
                        catch (Exception exx)
                        {
                            Logger.writeLog(exx);
                        }
                        #endregion

                        QueryCommand AddCmd = new QueryCommand(AddQuery);
                        AddCmd.AddParameter("@TransactionDate", TransactionDate, DbType.DateTime);
                        AddCmd.AddParameter("@StartValidPeriod", StartValidPeriod, DbType.DateTime);
                        AddCmd.AddParameter("@EndValidPeriod", EndValidPeriod, DbType.DateTime);
                        AddCmd.AddParameter("@MembershipNo", MembershipNo, DbType.String);
                        AddCmd.AddParameter("@DiffPoints", OneDiffPoint, DbType.Decimal);
                        AddCmd.AddParameter("@ItemNo", Rw[0].ToString(), DbType.String);
                        AddCmd.AddParameter("@PointType", Rw[2].ToString(), DbType.String);
                        AddCmd.AddParameter("@UserName", UserName, DbType.String);
                        AddCmd.AddParameter("@PointUnitPrice", pointUnitPrice, DbType.Decimal);

                        Cmds.Add(AddCmd);
                        #endregion
                    }
                    else if (OneDiffPoint < 0)
                    {
                        decimal absDiffPoint = Math.Abs(OneDiffPoint);

                        //DataSet ds = MembershipPoint.Query().
                        //    WHERE("StartValidPeriod", Comparison.LessOrEquals, TransactionDate).
                        //    AND("EndValidPeriod", Comparison.GreaterOrEquals, TransactionDate).
                        //    AND(MembershipPoint.Columns.MembershipNo, MembershipNo).
                        //    AND(MembershipPoint.UserColumns.PackageItemNo, Rw[2].ToString().IndexOf("OPP") > 0 ? Rw[2].ToString().Remove(0, 1) : Rw[0].ToString()).
                        //    ORDER_BY("StartValidPeriod", "ASC").
                        //    ExecuteDataSet();

                        string sql = @"/* Check the points to be deducted */
                                        SELECT * 
                                        FROM MembershipPoints 
                                        WHERE StartValidPeriod <= '{0}' 
                                            AND EndValidPeriod >= '{0}' 
                                            AND MembershipNo = '{1}' 
                                            AND userfld1 = '{2}' 
                                        ORDER BY StartValidPeriod ASC
                                     ";
                        sql = string.Format(sql,
                            TransactionDate.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                            MembershipNo,
                            Rw[2].ToString().IndexOf("OPP") > 0 ? Rw[2].ToString().Remove(0, 1).Replace("'", "''") : Rw[0].ToString().Replace("'", "''"));

                        DataSet ds = DataService.GetDataSet(new QueryCommand(sql, "PowerPOS"));

                        #region *) Option 01: Points exist in system [Do Update]
                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            int CurrentPointID;
                            decimal CurrentPoint;

                            int i = 0;
                            while (absDiffPoint > 0)
                            {
                                #region ^) Terminator: Point insufficient
                                if (absDiffPoint > 0 & i == ds.Tables[0].Rows.Count)
                                    throw new Exception("(warning)Insufficient points to be deducted");
                                #endregion

                                CurrentPoint = decimal.Parse(ds.Tables[0].Rows[i]["Points"].ToString());
                                CurrentPointID = int.Parse(ds.Tables[0].Rows[i]["PointID"].ToString());

                                if (absDiffPoint >= CurrentPoint)
                                {
                                    string MinQuery =
                                        "UPDATE MembershipPoints " +
                                        "SET Points = Points - @DiffPoints, ModifiedOn = GETDATE(), ModifiedBy = @ModifiedBy " +
                                        "WHERE PointID = @PointID";
                                    QueryCommand MinCmd = new QueryCommand(MinQuery);
                                    MinCmd.AddParameter("@DiffPoints", CurrentPoint, DbType.Decimal);
                                    //MinCmd.AddParameter("@ModifiedOn", DateTime.Now, DbType.DateTime);
                                    MinCmd.AddParameter("@ModifiedBy", UserName, DbType.String);
                                    MinCmd.AddParameter("@PointID", CurrentPointID, DbType.Int32);

                                    //qry.AddUpdateSetting("Points", 0);
                                    //Cmds.Add(qry.BuildDeleteCommand());
                                    Cmds.Add(MinCmd);
                                    absDiffPoint = absDiffPoint - CurrentPoint;
                                }
                                else
                                {

                                    string MinQuery =
                                        "UPDATE MembershipPoints " +
                                        "SET Points = Points - @DiffPoints, ModifiedOn = GETDATE(), ModifiedBy = @ModifiedBy " +
                                        "WHERE PointID = @PointID";
                                    QueryCommand MinCmd = new QueryCommand(MinQuery);
                                    MinCmd.AddParameter("@DiffPoints", absDiffPoint, DbType.Decimal);
                                    //MinCmd.AddParameter("@ModifiedOn", DateTime.Now, DbType.DateTime);
                                    MinCmd.AddParameter("@ModifiedBy", UserName, DbType.String);
                                    MinCmd.AddParameter("@PointID", CurrentPointID, DbType.Int32);


                                    //Cmds.Add(qry.BuildUpdateCommand());
                                    Cmds.Add(MinCmd);

                                    absDiffPoint = 0;
                                }
                                i += 1;
                            }
                        }
                        #endregion
                        #region *) Option 02: Points do not exist in system [Send Error]
                        else
                        {
                            Logger.writeLog(sql);
                            throw new Exception("(warning)Insufficient points to be deducted");
                        }
                        #endregion
                    }

                    #region *) Core: Save Point Allocation Log
                    PointAllocationLog PointLogger = new PointAllocationLog();
                    PointLogger.AllocationDate = TransactionDate;
                    PointLogger.OrderHdrID = OrderHdrID;
                    PointLogger.MembershipNo = MembershipNo;
                    PointLogger.PointAllocated = OneDiffPoint;
                    PointLogger.UniqueID = Guid.NewGuid();
                    PointLogger.Userfld1 = Rw[2].ToString().IndexOf("OPP") > 0 ? Rw[2].ToString().Remove(0, 1) : Rw[0].ToString();    
                    PointLogger.Userfld2 = SalesPersonID;
                    PointLogger.Userfld3 = "";
                    PointLogger.Userfld4 = "";
                    PointLogger.Userfld5 = "";
                    PointLogger.Userfld6 = "";
                    PointLogger.Userfld7 = "";
                    PointLogger.Userfld8 = "";
                    PointLogger.Userfld9 = "";
                    PointLogger.Userfld10 = "";
                    Cmds.Add(PointLogger.GetInsertCommand(UserName));
                    #endregion
                }
                /*string DelQuery =
                    "DELETE FROM MembershipPoints " +
                    "WHERE Points = 0 AND MembershipNo = @MembershipNo";
                QueryCommand DelCmd = new QueryCommand(DelQuery);
                DelCmd.AddParameter("@MembershipNo", MembershipNo, DbType.String);
                Cmds.Add(DelCmd);*/
                DataService.ExecuteTransaction(Cmds);
                return true;
            }
            catch (Exception X)
            {
                InitialPoint = 0;
                DiffPoint = 0;
                Status = X.Message;
                Logger.writeLog(X);
                return false;
            }
        }
        public static bool UpdateAll(DataTable PointData, string OrderHdrID, DateTime TransactionDate
           , int ValidPeriods, string MembershipNo, string SalesPersonID, string UserName
           , out string Status)
        {
            decimal InitialPoint = 0;
            decimal DiffPoint = 0;
            return UpdateAll(PointData, OrderHdrID, TransactionDate, ValidPeriods, MembershipNo, SalesPersonID, UserName, out InitialPoint, out DiffPoint, out Status);
        }

        public static bool RevertPoints(string OrderHdrID, DateTime TransactionDate, string MembershipNo, string SalesPersonID, string UserName, out string Status)
        {
            Status = "";
            try
            {
                string QueryStr =
                    "SELECT PointAllocationLog." + PointAllocationLog.UserColumns.PointItemNo + ", 0 - PointAllocated,MembershipPoints." + MembershipPoint.UserColumns.PackageType + " " +
                    "FROM PointAllocationLog " +
                        "INNER JOIN MembershipPoints ON PointAllocationLog." + PointAllocationLog.UserColumns.PointItemNo + " = MembershipPoints." + MembershipPoint.UserColumns.PackageItemNo +
                    " WHERE PointAllocationLog.OrderHdrID = @OrderHdrID AND PointAllocationLog.MembershipNo = MembershipPoints.MembershipNo And PointAllocationLog.MembershipNo = @MembershipNo ";
                QueryCommand Cmd = new QueryCommand(QueryStr);
                Cmd.AddParameter("@OrderHdrID", OrderHdrID, DbType.String);
                Cmd.AddParameter("@MembershipNo", MembershipNo, DbType.String);
                DataTable dt = new DataTable("Revert");
                dt.Load(DataService.GetReader(Cmd));

                if (dt.Rows.Count < 1) return true;

                bool Result = (PackageController.UpdateAll(dt, OrderHdrID, TransactionDate, 0, MembershipNo, SalesPersonID, UserName, out Status));

                return Result;
            }
            catch (Exception X)
            {
                Status = X.Message;
                return false;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="membershipNo"></param>
        /// <param name="CurrentDate"></param>
        /// <param name="Result">
        /// DataTable (CurrentAmounts) contain Columns (StartValidPeriod[String], RefNo[String], Points[Decimal]) Ordered By StartValidPeriod & PointID
        /// </param>
        /// <returns>False if any error is occurred</returns>
        public static bool GetCurrentAmount_Points(string membershipNo, DateTime CurrentDate, out DataTable Output, out string Status)
        {
            Output = new DataTable("CurrentAmounts");
            Status = "";

            try
            {
                string QueryStr =
                    "SELECT StartValidPeriod, MembershipPoints." + MembershipPoint.UserColumns.PackageItemNo + " AS RefNo, Points " +
                    "FROM MembershipPoints LEFT JOIN " +
                        "Item ON MembershipPoints." + MembershipPoint.UserColumns.PackageItemNo + " = Item.ItemNo " +
                    "WHERE MembershipNo = @MembershipNo AND MembershipPoints." + MembershipPoint.UserColumns.PackageType + " = @PointType " +
                        "AND StartValidPeriod <= @CurrentDate AND EndValidPeriod >= @CurrentDate " +
                        "AND MembershipPoints." + MembershipPoint.UserColumns.PackageItemNo + " <> '' " +
                        "AND Points > 0 " +
                    "ORDER BY StartValidPeriod, PointID ";

                QueryCommand cmd = new QueryCommand(QueryStr);
                cmd.AddParameter("@MembershipNo", membershipNo, System.Data.DbType.String);
                cmd.AddParameter("@CurrentDate", CurrentDate, System.Data.DbType.DateTime);
                cmd.AddParameter("@PointType", Item.PointMode.Dollar, System.Data.DbType.String);

                IDataReader Rdr = DataService.GetReader(cmd);
                if (Rdr != null) Output.Load(Rdr);

                return true;
            }
            catch (Exception X)
            {
                Status = "";
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="membershipNo"></param>
        /// <param name="CurrentDate"></param>
        /// <param name="Result">
        /// DataTable (CurrentAmounts) contain Columns (StartValidPeriod[String], RefNo[String], Points[Decimal]) Ordered By StartValidPeriod & PointID
        /// </param>
        /// <returns>False if any error is occurred</returns>
        public static decimal GetCurrentAmountPerItem(string membershipNo, DateTime CurrentDate, string ItemNo, out string Status)
        {
            decimal result = 0;
            Status = "";

            try
            {
                string QueryStr = "";
                    //"SELECT  Points " +
                    //"FROM MembershipPoints LEFT JOIN " +
                    //    "Item ON MembershipPoints." + MembershipPoint.UserColumns.PackageItemNo + " = Item.ItemNo " +
                    //"WHERE MembershipNo = @MembershipNo " +
                    //    "AND StartValidPeriod <= @CurrentDate AND EndValidPeriod >= @CurrentDate AND Item.ItemName = N'" + ItemNo + "' ";
                    //"ORDER BY StartValidPeriod, PointID ";

                QueryStr = @"DECLARE @ItemName NVARCHAR(500)
                            SET @ItemName = (SELECT TOP 1 ItemNo FROM Item WHERE ItemName = N'{0}')

                            IF(@ItemName IS NULL) BEGIN
	                            SET @ItemName = N'{0}';
                            END

                            SELECT  SUM(Points) AS Points
                            FROM	MembershipPoints MP
                            WHERE	MP.MembershipNo = '{1}'
		                            AND MP.StartValidPeriod <= '{2}' 
		                            AND MP.EndValidPeriod >= '{2}'
		                            AND MP.userfld1 = @ItemName";
                QueryStr = string.Format(QueryStr, ItemNo, membershipNo, CurrentDate.ToString("yyyy-MM-dd HH:mm:ss"));

                QueryCommand cmd = new QueryCommand(QueryStr);
                //cmd.AddParameter("@MembershipNo", membershipNo, System.Data.DbType.String);
                //cmd.AddParameter("@CurrentDate", CurrentDate, System.Data.DbType.DateTime);
                //cmd.AddParameter("@PointType", Item.PointMode.Dollar, System.Data.DbType.String);

                object tmp = DataService.ExecuteScalar(cmd);
                if (!decimal.TryParse(tmp.ToString(), out result))
                {
                    return 0;
                }

                return result;
            }
            catch (Exception X)
            {
                Status = "Error Getting Current Point Balance";
                return 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="membershipNo"></param>
        /// <param name="CurrentDate"></param>
        /// <param name="Result">
        /// </param>
        /// <returns>False if any error is occurred</returns>
        public static bool GetCurrentAmount(string membershipNo, DateTime CurrentDate, string PackageRefNo, out decimal Output, out string Status)
        {
            try
            {
                Status = "";
                Output = 0;

                string QueryStr =
                 "SELECT ISNULL(SUM(Points),0) " +
                 "FROM MembershipPoints " +
                 "WHERE MembershipNo = @MembershipNo " +
                     "AND StartValidPeriod <= @CurrentDate AND EndValidPeriod >= @CurrentDate " +
                     "AND MembershipPoints." + MembershipPoint.UserColumns.PackageItemNo + " = @PackageRefNo";

                QueryCommand cmd = new QueryCommand(QueryStr);
                cmd.AddParameter("@MembershipNo", membershipNo, System.Data.DbType.String);
                cmd.AddParameter("@CurrentDate", CurrentDate, System.Data.DbType.DateTime);
                cmd.AddParameter("@PackageRefNo", PackageRefNo, System.Data.DbType.String);

                string sPoints = DataService.ExecuteScalar(cmd).ToString();

                if (string.IsNullOrEmpty(sPoints))
                    throw new Exception("(error)Loading package failed;\nInput M'ship No = " + membershipNo + ";\nPackage Ref No=" + PackageRefNo + ";\nDate = " + CurrentDate.ToString());

                if (!decimal.TryParse(sPoints, out Output))
                    throw new Exception("(error)Point convertion from string to decimal is failed, Point is " + sPoints);

                return true;
            }
            catch (Exception X)
            {
                Output = 0;
                Status = X.Message;
                Logger.writeLog(X);
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="membershipNo"></param>
        /// <param name="CurrentDate"></param>
        /// <param name="Result">
        /// </param>
        /// <returns>False if any error is occurred</returns>
        public static bool GetCurrentAmount(string membershipNo, DateTime CurrentDate, out DataTable Output, out string Status)
        {
            try
            {
                Output = null;
                Status = "";

                string QueryStr = "";
                    //"SELECT ISNULL(Item.ItemName,MembershipPoints." + MembershipPoint.UserColumns.PackageItemNo + ") AS RefNo, SUM(Points) AS Points, MembershipPoints." + MembershipPoint.UserColumns.PackageType + " AS PointType " +
                    //"FROM MembershipPoints LEFT JOIN " +
                    //    "Item ON MembershipPoints." + MembershipPoint.UserColumns.PackageItemNo + " = Item.ItemNo " +
                    //"WHERE MembershipNo = @MembershipNo " +
                    //    "AND StartValidPeriod <= @CurrentDate AND EndValidPeriod >= @CurrentDate " +
                    //"GROUP BY Item.ItemName, MembershipPoints." + MembershipPoint.UserColumns.PackageItemNo + ", MembershipPoints." + MembershipPoint.UserColumns.PackageType + "";

                QueryStr = @"
                SELECT	 ISNULL(Item.ItemName,MembershipPoints.userfld1) AS RefNo
		                ,SUM(Points) AS Points
		                ,MembershipPoints.userfld2 AS PointType 
                        ,MembershipPoints.userfld1
                FROM	MembershipPoints 
		                LEFT JOIN Item ON 
			                (CASE WHEN MembershipPoints.userfld1  LIKE '%|OPP|%' 
				                  THEN LEFT(MembershipPoints.userfld1 ,CHARINDEX('|OPP|',MembershipPoints.userfld1 )-1) 
				                  ELSE MembershipPoints.userfld1  END)
			                = Item.ItemNo 
                WHERE	ISNULL(MembershipPoints.userfld1,'') <> ''
	                    AND MembershipNo = @MembershipNo 
		                AND StartValidPeriod <= @CurrentDate AND EndValidPeriod >= @CurrentDate 
                GROUP BY Item.ItemName, MembershipPoints.userfld1, MembershipPoints.userfld2
                HAVING SUM(Points) <> 0";

                QueryCommand cmd = new QueryCommand(QueryStr);
                cmd.AddParameter("@MembershipNo", membershipNo, System.Data.DbType.String);
                cmd.AddParameter("@CurrentDate", CurrentDate, System.Data.DbType.DateTime);

                DataTable Result = new DataTable("CurrentAmounts");

                IDataReader Rdr = DataService.GetReader(cmd);
                if (Rdr != null) Result.Load(Rdr);

                Output = Result;
                return true;
            }
            catch (Exception X)
            {
                Output = null;
                Status = X.Message;
                Logger.writeLog(X);
                return false;
            }
        }
        /// <summary>
        /// Update Item Table when Get Point and Break Dowm Price  Value reach at 1
        /// </summary>
        /// <param name="TimeGet"> Time get point value</param>
        /// <param name="BreakDownPrice"> Break Down Price value</param>
        /// <param name="ItemNo"> Item No Value</param>
        /// <returns></returns>
        public static bool updateMemberShipPackage(Decimal TimeGet, Decimal BreakDownPrice, String ItemNo, out decimal Output, out string Status)
        {
            try
            {
                Status = "";
                Output = 0;
                String MyUpDateQuery = "UpDate Item Set [userfloat1]=@userfloat1,userfloat3=@userfloat3 Where [ItemNo]=@ItemNo";
                QueryCommand Qcmd = new QueryCommand(MyUpDateQuery);
                Qcmd.AddParameter("@userfloat1", TimeGet,DbType.Decimal);
                Qcmd.AddParameter("@userfloat3", BreakDownPrice, DbType.Decimal);
                Qcmd.AddParameter("@ItemNo", ItemNo, DbType.String);
                DataService.ExecuteScalar(Qcmd);
                string sPoints = "";
                if (string.IsNullOrEmpty(sPoints))
                    throw new Exception("(error)Loading package failed;\nInput Item No = " );
                if (!decimal.TryParse(sPoints, out Output))
                    throw new Exception("(error)Point convertion from string to decimal is failed, Point is " + sPoints);
                return true;
            }
            catch (Exception X)
            {
                Output = 0;
                Status = X.Message;
                Logger.writeLog(X);
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="membershipNo"></param>
        /// <param name="CurrentDate"></param>
        /// <param name="Result">
        /// </param>
        /// <returns>False if any error is occurred</returns>
        public static bool GetCurrentBreakdown(string membershipNo, DateTime CurrentDate, string PackageRefNo, out decimal Output, out string Status)
        {
            try
            {
                Status = "";
                Output = 0;

                string QueryStr =
                 "SELECT ISNULL(MAX(" + MembershipPoint.UserColumns.CourseBreakdownPrice + "),0) " +
                 "FROM MembershipPoints " +
                 "WHERE MembershipNo = @MembershipNo " +
                     "AND StartValidPeriod <= @CurrentDate AND EndValidPeriod >= @CurrentDate " +
                     "AND MembershipPoints." + MembershipPoint.UserColumns.PackageItemNo + " = @PackageRefNo";

                QueryCommand cmd = new QueryCommand(QueryStr);
                cmd.AddParameter("@MembershipNo", membershipNo, System.Data.DbType.String);
                cmd.AddParameter("@CurrentDate", CurrentDate, System.Data.DbType.DateTime);
                cmd.AddParameter("@PackageRefNo", PackageRefNo, System.Data.DbType.String);

                string sPoints = DataService.ExecuteScalar(cmd).ToString();

                if (string.IsNullOrEmpty(sPoints))
                    throw new Exception("(error)Loading package failed;\nInput M'ship No = " + membershipNo + ";\nPackage Ref No=" + PackageRefNo + ";\nDate = " + CurrentDate.ToString());

                if (!decimal.TryParse(sPoints, out Output))
                    throw new Exception("(error)Point convertion from string to decimal is failed, Point is " + sPoints);

                return true;
            }
            catch (Exception X)
            {
                Output = 0;
                Status = X.Message;
                Logger.writeLog(X);
                return false;
            }
        }


    }
}
