using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.ComponentModel;
using SubSonic;
using System.Data;
using System.Collections;
using PowerPOS.Container;
using System.IO;
using System.Net.NetworkInformation;
using System.Transactions;

/// This file is to define all function to modify points that is located in server

namespace PowerPOS
{
    /// <summary>
    /// Last Update = 14 September 2010 - 17:29
    /// </summary>
    public partial class MembershipController
    {
        private const string StartDateString = "01 Jan 2000";
        public static string POINT_RefID = "POINTS";

        /// <summary>
        /// Raw function to add points to a member (Please provide validation before use this function)
        /// </summary>
        /// <param name="MembershipNo">Customer's membership number</param>
        /// <param name="OrderHdrID">OrderHdrID - for reference only</param>
        /// <param name="TransactionDate">Date of Add Points' transaction</param>
        /// <param name="ValidPeriods">Month based validity time of point system; use 0 for unlimited time</param>
        /// <param name="points">Number of points to be added</param>
        /// <param name="UserName">Name of user who do this transaction</param>
        /// <param name="status">Error string (if any)</param>
        /// <returns>True if success, otherwise False</returns>
        /// <remarks>
        /// *) Points amount that less than 1 will be returned as Error
        /// *) Use 0 in Valid Periods will generate 100 years of validity time
        /// *) If Customer has any active record in Membership Point that match TransactionDate, 
        ///    ValidPeriods will be useless
        /// *) If Customer doesn't have any active record in Membership Point 
        ///    that match TransactionDate, TransactionDate will be a StartValidDate
        /// </remarks>
        public static bool AddPoints_Final(
            string MembershipNo, string OrderHdrID, DateTime TransactionDate, int ValidPeriods,
            decimal points, string UserName, out string status)
        {
            try
            {
                #region *) Validation: Check if points is greater than 0 [Exit if False]
                if (points <= 0)
                { status = "Points should be greater than 0"; return false; }
                #endregion

                DateTime StartValidPeriod, EndValidPeriod;
                #region *) Initialize: Set StartValidPeriod & EndValidPeriod
                if (ValidPeriods < 1)
                {   /// From 1 Jan 2000 till 31 Dec 2099
                    StartValidPeriod = DateTime.Parse(StartDateString);
                    EndValidPeriod = StartValidPeriod.AddYears(100).AddMilliseconds(-1);
                }
                else
                {   /// From TransactionDate till Expiry
                    StartValidPeriod = TransactionDate;
                    EndValidPeriod = StartValidPeriod.AddMonths(ValidPeriods).AddMilliseconds(-1);
                }
                #endregion

                QueryCommandCollection cmd = new QueryCommandCollection();

                #region *) Core: Save (Upsert) Membership Points
                DataSet ds = MembershipPoint.Query().
                  WHERE("StartValidPeriod", Comparison.LessOrEquals, TransactionDate).
                  AND("EndValidPeriod", Comparison.GreaterOrEquals, TransactionDate).
                  AND(MembershipPoint.Columns.MembershipNo, MembershipNo).
                  ORDER_BY("EndValidPeriod", "ASC").
                  ExecuteDataSet();

                #region *) Option 01: Points exist in system [Do Update]
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    int CurrentPointID = int.Parse(ds.Tables[0].Rows[0]["PointID"].ToString());
                    decimal CurrentPoint = decimal.Parse(ds.Tables[0].Rows[0]["Points"].ToString());

                    Query qry = new Query("MembershipPoints");
                    qry.QueryType = QueryType.Update;
                    qry.AddWhere(MembershipPoint.Columns.PointID, CurrentPointID);
                    qry.AddUpdateSetting("Points", CurrentPoint + points);
                    qry.AddUpdateSetting("ModifiedBy", UserName);
                    qry.AddUpdateSetting("ModifiedOn", DateTime.Now);
                    cmd.Add(qry.BuildUpdateCommand());
                }
                #endregion
                #region *) Option 02: Points do not exist in system [Do Create]
                else
                {
                    MembershipPoint mp = new MembershipPoint();
                    mp.StartValidPeriod = StartValidPeriod;
                    mp.EndValidPeriod = EndValidPeriod;
                    mp.MembershipNo = MembershipNo;
                    mp.Points = points;
                    mp.IsNew = true;
                    cmd.Add(mp.GetInsertCommand(UserName));
                }
                #endregion
                #endregion

                #region *) Core: Save Point Allocation Log
                PointAllocationLog PointLogger = new PointAllocationLog();
                PointLogger.AllocationDate = TransactionDate;
                PointLogger.OrderHdrID = OrderHdrID;
                PointLogger.MembershipNo = MembershipNo;
                PointLogger.PointAllocated = points;
                PointLogger.UniqueID = Guid.NewGuid();
                PointLogger.Userfld1 = "";
                PointLogger.Userfld2 = "";
                PointLogger.Userfld3 = "";
                PointLogger.Userfld4 = "";
                PointLogger.Userfld5 = "";
                PointLogger.Userfld6 = "";
                PointLogger.Userfld7 = "";
                PointLogger.Userfld8 = "";
                PointLogger.Userfld9 = "";
                PointLogger.Userfld10 = "";
                cmd.Add(PointLogger.GetInsertCommand(UserName));
                #endregion

                DataService.ExecuteTransaction(cmd);

                status = "";
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = "Some error occured";
                return false;
            }
        }

        /// <summary>
        /// Raw function to deduct points from a member (Please provide validation before use this function)
        /// </summary>
        /// <param name="MembershipNo">Customer's membership number</param>
        /// <param name="OrderHdrID">OrderHdrID - for reference only</param>
        /// <param name="TransactionDate">Date of Deduct Points' transaction</param>
        /// <param name="points">Number of points to be deducted</param>
        /// <param name="UserName">Name of user who do this transaction</param>
        /// <param name="status">Error string (if any)</param>
        /// <returns>True if success, otherwise False</returns>
        /// <remarks>
        /// *) Points amount that less than 1 will be returned as Error
        /// *) If customer's active point not suficient, system will return an Error
        /// *) If customer has more than 1 active point set, system will deduct
        ///    one-by-one started from the one with earlier EndValidDate
        /// </remarks>
        public static bool DeductPoints_Final(
            string MembershipNo, string OrderHdrID, DateTime TransactionDate,
            decimal points, string packageRefNo, string UserName, out string status)
        {
            try
            {
                decimal InitialPoints = points;
                #region *) Validation: Check if points is greater than 0 [Exit if False]
                if (points <= 0)
                { status = "Points should be greater than 0"; return false; }
                #endregion

                QueryCommandCollection cmd = new QueryCommandCollection();

                #region *) Core: Save (Update) Membership Points
                DataSet ds = MembershipPoint.Query().
                  WHERE("StartValidPeriod", Comparison.LessOrEquals, TransactionDate).
                  AND("EndValidPeriod", Comparison.GreaterOrEquals, TransactionDate).
                  AND(MembershipPoint.Columns.MembershipNo, MembershipNo).
                  AND(MembershipPoint.Columns.Userfld1, packageRefNo).
                  ORDER_BY("EndValidPeriod", "ASC").
                  ExecuteDataSet();

                #region *) Option 01: Points exist in system [Do Update]
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    int CurrentPointID;
                    decimal CurrentPoint;

                    int i = 0;
                    while (points > 0)
                    {
                        #region ^) Terminator: Point insufficient
                        if (points > 0 & i == ds.Tables[0].Rows.Count)
                        {
                            status = "Insufficient points to be deducted";
                            cmd.Clear();
                            return false;
                        }
                        #endregion

                        CurrentPoint = decimal.Parse(ds.Tables[0].Rows[i]["Points"].ToString());
                        CurrentPointID = int.Parse(ds.Tables[0].Rows[i]["PointID"].ToString());

                        if (points > CurrentPoint)
                        {
                            //do update 
                            //and continue the iteration
                            //points = 0 - points to be deducted is bigger than the existing points
                            Query qry = new Query("MembershipPoints");
                            qry.AddWhere(MembershipPoint.Columns.PointID, CurrentPointID);
                            qry.AddUpdateSetting("Points", 0);
                            cmd.Add(qry.BuildUpdateCommand());
                            points = points - CurrentPoint;
                        }
                        else
                        {
                            //do update 
                            //points = points - deducted points  
                            Query qry = new Query("MembershipPoints");
                            qry.AddWhere(MembershipPoint.Columns.PointID, CurrentPointID);
                            qry.AddUpdateSetting("Points", CurrentPoint - points);
                            cmd.Add(qry.BuildUpdateCommand());
                            points = 0;
                        }
                        i += 1;
                    }
                }
                #endregion
                #region *) Option 02: Points do not exist in system [Send Error]
                else
                {
                    status = "Insufficient points to be deducted";
                    return false;
                }
                #endregion
                #endregion

                #region *) Core: Save Point Allocation Log
                PointAllocationLog PointLogger = new PointAllocationLog();
                PointLogger.AllocationDate = TransactionDate;
                PointLogger.OrderHdrID = OrderHdrID;
                PointLogger.MembershipNo = MembershipNo;
                PointLogger.PointAllocated = 0 - InitialPoints;
                PointLogger.UniqueID = Guid.NewGuid();
                PointLogger.Userfld1 = packageRefNo;
                PointLogger.Userfld2 = "";
                PointLogger.Userfld3 = "";
                PointLogger.Userfld4 = "";
                PointLogger.Userfld5 = "";
                PointLogger.Userfld6 = "";
                PointLogger.Userfld7 = "";
                PointLogger.Userfld8 = "";
                PointLogger.Userfld9 = "";
                PointLogger.Userfld10 = "";
                cmd.Add(PointLogger.GetInsertCommand(UserName));
                #endregion

                DataService.ExecuteTransaction(cmd);

                status = "";
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = "Some error occured";
                return false;
            }
        }

        /// <summary>
        /// Get the points amount that a member have
        /// </summary>
        /// <param name="membershipNo">Customer's membership number</param>
        /// <param name="CurrentDate">Date of points enquiry</param>
        /// <param name="status">Error message (If any)</param>
        /// <returns>Current member's points</returns>
        /// <remarks>
        /// Just relocated from MembershipController.Custom.cs (Not modified)
        /// </remarks>
        public static decimal GetCurrentPoint(string membershipNo, DateTime CurrentDate, out string status)
        {
            try
            {
                //fetch SUM of points
                //where startvalidperiod < current date and
                //endvalidperiod > current date
                //Check if MembershipNo is valid in the system
                //Membership member = new Membership(Membership.Columns.MembershipNo, membershipNo);

                //if (!member.IsLoaded)
                //{
                //    status = "Membership Number [" + membershipNo + "] do not exist.";
                //    return 0;
                //}

                //if (string.IsNullOrEmpty (member.MembershipNo))
                //{
                //    status = "Membership Number [" + membershipNo + "] do not exist.";
                //    return 0;
                //}

                string sPoints = MembershipPoint.Query().WHERE(MembershipPoint.Columns.MembershipNo, membershipNo).
                    AND(MembershipPoint.Columns.StartValidPeriod, Comparison.LessOrEquals, CurrentDate).
                    AND(MembershipPoint.Columns.EndValidPeriod, Comparison.GreaterOrEquals, CurrentDate).
                    AND(MembershipPoint.Columns.Userfld2, Comparison.Equals, Item.PointMode.Dollar).
                    GetSum("Points").ToString();

                if (string.IsNullOrEmpty(sPoints))
                {
                    Logger.writeLog("Input M'ship No = " + membershipNo + "; Loaded M'ship No = " + membershipNo + "; Date = " + CurrentDate.ToString());
                    throw new Exception("(String)Point is NULL");
                }

                decimal points = 0;

                if (!decimal.TryParse(sPoints, out points))
                    throw new Exception("Convertion Failed, Point is " + sPoints);

                status = "";
                return points;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return 0.0M;
            }
        }

        public static decimal GetAllocatedPoint(string OrderHdrID)
        {
            try
            {
                string QueryStr = "SELECT SUM(PointAllocated) AS ResultValue FROM PointAllocationLog WHERE OrderHdrID = @OrderHdrID";
                QueryCommand cmd = new QueryCommand(QueryStr);
                cmd.Parameters.Add("@OrderHdrID", OrderHdrID);
                decimal AllocatedPoints = 0;
                decimal.TryParse(DataService.ExecuteScalar(cmd).ToString(), out AllocatedPoints);

                return AllocatedPoints;
            }
            catch (Exception X)
            {
                Logger.writeLog(X);
                return 0;
            }
        }
        public static DataTable GetHistory_Point_WebSiteWithOption(bool showAll, string MembershipNo, string PackageName
           , out DateTime StartValidPeriod, out DateTime EndValidPeriod, out decimal RemainingPoint, out decimal Balance)
        {
            StartValidPeriod = new DateTime(2000, 1, 1);
            EndValidPeriod = new DateTime(2100, 1, 1, 23, 59, 59);
            RemainingPoint = 0;
            Balance = 0;

            try
            {
                string QryStr;
                QueryCommand cmd;
                IDataReader rdr;
                Balance = 0;

                if (PackageName == POINT_RefID)
                {
                    QryStr =
                        "SELECT MIN(StartValidPeriod) AS StartValidPeriod, MAX(EndValidPeriod) AS EndValidPeriod, SUM(Points) AS RemainingPoint " +
                        "FROM MembershipPoints " +
                        "WHERE MembershipNo = @MembershipNo AND userfld1 = @PackageName";
                }
                else
                {
                    QryStr = @"DECLARE @ItemName NVARCHAR(500)
                                SET @ItemName = (SELECT TOP 1 ItemNo FROM Item WHERE ItemName = @PackageName)

                                IF(@ItemName IS NULL) BEGIN
	                                SET @ItemName = @PackageName;
                                END

                                SELECT   MIN(StartValidPeriod) AS StartValidPeriod
		                                ,MAX(EndValidPeriod) AS EndValidPeriod
		                                ,SUM(Points) AS RemainingPoint
                                FROM	MembershipPoints MP
                                WHERE	MP.MembershipNo = @MembershipNo
		                                AND MP.userfld1 = @ItemName";
                }

                cmd = new QueryCommand(QryStr);
                cmd.AddParameter("@MembershipNo", MembershipNo, DbType.String);
                cmd.AddParameter("@PackageName", PackageName, DbType.String);
                rdr = DataService.GetReader(cmd);
                if (rdr.Read())
                {
                    StartValidPeriod = rdr.IsDBNull(0) ? new DateTime(2000, 1, 1) : rdr.GetDateTime(0);
                    EndValidPeriod = rdr.IsDBNull(1) ? new DateTime(2100, 1, 1, 23, 59, 59) : rdr.GetDateTime(1);
                    RemainingPoint = rdr.IsDBNull(2) ? 0 : rdr.GetDecimal(2);
                }

                if (PackageName == POINT_RefID)
                {
                    QryStr =
                        "SELECT TOP 50 AllocationDate AS AllocationDate, OrderHdrID AS RefNo, ISNULL(userfld2,'') AS Stylist, PointAllocated AS Amount " +
                        "FROM PointAllocationLog " +
                        "WHERE MembershipNo = @MembershipNo AND userfld1 = @PackageName " +
                        "ORDER BY AllocationDate DESC";

                    if (showAll)
                    {
                        QryStr = "SELECT AllocationDate AS AllocationDate, OrderHdrID AS RefNo, ISNULL(userfld2,'') AS Stylist, PointAllocated AS Amount " +
                        "FROM PointAllocationLog " +
                        "WHERE MembershipNo = @MembershipNo AND userfld1 = @PackageName " +
                        "ORDER BY AllocationDate DESC";
                    }
                }
                else
                {
                    QryStr = @"DECLARE @ItemName NVARCHAR(500)
                                SET @ItemName = (SELECT TOP 1 ItemNo FROM Item WHERE ItemName = @PackageName)

                                IF(@ItemName IS NULL) BEGIN
	                                SET @ItemName = @PackageName;
                                END

                                SELECT   TOP 50 
		                                 PAL.AllocationDate AS AllocationDate
		                                ,OrderHdrID AS RefNo
		                                ,ISNULL(userfld2,'') AS Stylist
		                                , PointAllocated AS Amount
                                FROM	PointAllocationLog PAL
                                WHERE	PAL.MembershipNo = @MembershipNo
		                                AND PAL.userfld1 = @ItemName
                                ORDER BY PAL.AllocationDate DESC";

                    if (showAll)
                    {
                        QryStr = @"DECLARE @ItemName NVARCHAR(500)
                                SET @ItemName = (SELECT TOP 1 ItemNo FROM Item WHERE ItemName = @PackageName)

                                IF(@ItemName IS NULL) BEGIN
	                                SET @ItemName = @PackageName;
                                END

                                SELECT  PAL.AllocationDate AS AllocationDate
		                                ,OrderHdrID AS RefNo
		                                ,ISNULL(userfld2,'') AS Stylist
		                                , PointAllocated AS Amount
                                FROM	PointAllocationLog PAL
                                WHERE	PAL.MembershipNo = @MembershipNo
		                                AND PAL.userfld1 = @ItemName
                                ORDER BY PAL.AllocationDate DESC";
                    }
                }

                cmd = new QueryCommand(QryStr);
                cmd.AddParameter("@MembershipNo", MembershipNo, DbType.String);
                cmd.AddParameter("@PackageName", PackageName, DbType.String);

                DataTable dt = new DataTable("History");
                dt.Load(DataService.GetReader(cmd));

                // Calculate balance from sum of all PointAllocated values
                QryStr = @"DECLARE @ItemName NVARCHAR(500)
                                SET @ItemName = (SELECT TOP 1 ItemNo FROM Item WHERE ItemName = @PackageName)

                                IF(@ItemName IS NULL) BEGIN
	                                SET @ItemName = @PackageName;
                                END

                                SELECT  PointAllocated AS Amount
                                FROM	PointAllocationLog PAL
                                WHERE	PAL.MembershipNo = @MembershipNo
		                                AND PAL.userfld1 = @ItemName
                                ORDER BY PAL.AllocationDate DESC";

                cmd = new QueryCommand(QryStr);
                cmd.AddParameter("@MembershipNo", MembershipNo, DbType.String);
                cmd.AddParameter("@PackageName", PackageName, DbType.String);

                DataTable dtBalance = new DataTable("Balance");
                dtBalance.Load(DataService.GetReader(cmd));
                Balance = Convert.ToDecimal(dtBalance.Compute("SUM(Amount)", string.Empty));

                return dt;
            }
            catch (Exception X)
            {
                Logger.writeLog(X);

                return null;
            }
        }
        public static DataTable GetHistory_Point_WebSite(string MembershipNo, string PackageName
            , out DateTime StartValidPeriod, out DateTime EndValidPeriod, out decimal RemainingPoint)
        {
            StartValidPeriod = new DateTime(2000, 1, 1);
            EndValidPeriod = new DateTime(2100, 1, 1, 23, 59, 59);
            RemainingPoint = 0;

            try
            {
                string QryStr;
                QueryCommand cmd;
                IDataReader rdr;

                if (PackageName == POINT_RefID)
                {
                    QryStr =
                        "SELECT MIN(StartValidPeriod) AS StartValidPeriod, MAX(EndValidPeriod) AS EndValidPeriod, SUM(Points) AS RemainingPoint " +
                        "FROM MembershipPoints " +
                        "WHERE MembershipNo = @MembershipNo AND userfld1 = @PackageName";
                }
                else
                {
                    //QryStr =
                    //    "SELECT MIN(StartValidPeriod) AS StartValidPeriod, MAX(EndValidPeriod) AS EndValidPeriod, SUM(Points) AS RemainingPoint " +
                    //    "FROM MembershipPoints " +
                    //    "WHERE MembershipNo = @MembershipNo "+
                    //    " AND (CASE WHEN userfld1 LIKE '%|OPP|%' THEN LEFT(userfld1,CHARINDEX('|OPP|',userfld1)-1) ELSE userfld1 END) "
                    //        + " = ISNULL((" +
                    //        "SELECT TOP 1 ItemNo " +
                    //        "FROM Item " +
                    //        "WHERE ItemName = @PackageName AND Deleted = 0),@PackageName)";
                    QryStr = @"DECLARE @ItemName NVARCHAR(500)
                                SET @ItemName = (SELECT TOP 1 ItemNo FROM Item WHERE ItemName = @PackageName)

                                IF(@ItemName IS NULL) BEGIN
	                                SET @ItemName = @PackageName;
                                END

                                SELECT   MIN(StartValidPeriod) AS StartValidPeriod
		                                ,MAX(EndValidPeriod) AS EndValidPeriod
		                                ,SUM(Points) AS RemainingPoint
                                FROM	MembershipPoints MP
                                WHERE	MP.MembershipNo = @MembershipNo
		                                AND MP.userfld1 = @ItemName";
                }

                cmd = new QueryCommand(QryStr);
                cmd.AddParameter("@MembershipNo", MembershipNo, DbType.String);
                cmd.AddParameter("@PackageName", PackageName, DbType.String);
                rdr = DataService.GetReader(cmd);
                if (rdr.Read())
                {
                    StartValidPeriod = rdr.IsDBNull(0) ? new DateTime(2000, 1, 1) : rdr.GetDateTime(0);
                    EndValidPeriod = rdr.IsDBNull(1) ? new DateTime(2100, 1, 1, 23, 59, 59) : rdr.GetDateTime(1);
                    RemainingPoint = rdr.IsDBNull(2) ? 0 : rdr.GetDecimal(2);
                }

                if (PackageName == POINT_RefID)
                {
                    QryStr =
                        "SELECT TOP 50 AllocationDate AS AllocationDate, OrderHdrID AS RefNo, ISNULL(userfld2,'') AS Stylist, PointAllocated AS Amount " +
                        "FROM PointAllocationLog " +
                        "WHERE MembershipNo = @MembershipNo AND userfld1 = @PackageName " +
                        "ORDER BY AllocationDate DESC";
                }
                else
                {
                    //QryStr =
                    //    "SELECT TOP 50 AllocationDate AS AllocationDate, OrderHdrID AS RefNo, ISNULL(userfld2,'') AS Stylist, PointAllocated AS Amount " +
                    //    "FROM PointAllocationLog " +
                    //    "WHERE MembershipNo = @MembershipNo "+
                    //    " AND (CASE WHEN userfld1 LIKE '%|OPP|%' THEN LEFT(userfld1,CHARINDEX('|OPP|',userfld1)-1) ELSE userfld1 END) "
                    //        +" = ISNULL((" +
                    //        "SELECT TOP 1 ItemNo " +
                    //        "FROM Item " +
                    //        "WHERE ItemName = @PackageName AND Deleted = 0),@PackageName) " +
                    //    "ORDER BY AllocationDate DESC";
                    QryStr = @"DECLARE @ItemName NVARCHAR(500)
                                SET @ItemName = (SELECT TOP 1 ItemNo FROM Item WHERE ItemName = @PackageName)

                                IF(@ItemName IS NULL) BEGIN
	                                SET @ItemName = @PackageName;
                                END

                                SELECT   TOP 50 
		                                 PAL.AllocationDate AS AllocationDate
		                                ,OrderHdrID AS RefNo
		                                ,ISNULL(userfld2,'') AS Stylist
		                                , PointAllocated AS Amount
                                FROM	PointAllocationLog PAL
                                WHERE	PAL.MembershipNo = @MembershipNo
		                                AND PAL.userfld1 = @ItemName
                                ORDER BY PAL.AllocationDate DESC";
                }

                cmd = new QueryCommand(QryStr);
                cmd.AddParameter("@MembershipNo", MembershipNo, DbType.String);
                cmd.AddParameter("@PackageName", PackageName, DbType.String);

                DataTable dt = new DataTable("History");
                dt.Load(DataService.GetReader(cmd));

                return dt;
            }
            catch (Exception X)
            {
                Logger.writeLog(X);

                return null;
            }
        }
        public static DataTable GetHistory_Times_WebSite(string MembershipNo, string PackageName
            , out DateTime StartValidPeriod, out DateTime EndValidPeriod, out decimal RemainingPoint)
        {
            StartValidPeriod = new DateTime(2000, 1, 1);
            EndValidPeriod = new DateTime(2100, 1, 1);
            RemainingPoint = -1;

            try
            {
                string QryStr;
                QueryCommand cmd;
                IDataReader rdr;

                QryStr =
                    "SELECT MIN(StartValidPeriod) AS StartValidPeriod, MAX(EndValidPeriod) AS EndValidPeriod, SUM(Points) AS RemainingPoint " +
                    "FROM MembershipPoints " +
                    "WHERE MembershipNo = @MembershipNo AND userfld1 = (" +
                        "SELECT TOP 1 ItemNo " +
                        "FROM Item " +
                        "WHERE ItemName = @PackageName)";

                cmd = new QueryCommand(QryStr);
                cmd.AddParameter("@MembershipNo", MembershipNo, DbType.String);
                cmd.AddParameter("@PackageName", PackageName, DbType.String);

                rdr = DataService.GetReader(cmd);
                if (!rdr.Read()) return null;

                StartValidPeriod = rdr.GetDateTime(0);
                EndValidPeriod = rdr.GetDateTime(1);
                RemainingPoint = rdr.GetDecimal(2);

                QryStr =
                    "SELECT AllocationDate AS AllocationDate, OrderHdrID AS RefNo, userfld2 AS Stylist, PointAllocated AS Amount " +
                    "FROM PointAllocationLog " +
                    "WHERE MembershipNo = @MembershipNo AND userfld1 = (" +
                        "SELECT TOP 1 ItemNo " +
                        "FROM Item " +
                        "WHERE ItemName = @PackageName)";

                cmd = new QueryCommand(QryStr);
                cmd.AddParameter("@MembershipNo", MembershipNo, DbType.String);
                cmd.AddParameter("@PackageName", PackageName, DbType.String);

                DataTable dt = new DataTable("History");
                dt.Load(DataService.GetReader(cmd));

                return dt;
            }
            catch (Exception X)
            {
                Logger.writeLog(X);

                return null;
            }
        }

        public static decimal FetchRemainingInstallmentBalance(string MembershipNo)
        {
            try
            {
                DataTable dt = new DataTable();

                //string SQL = "SELECT MembershipNo as MembershipNo, NameToAppear as NameToAppear FROM Membership";

                string SQL =
                "SELECT dt.MembershipNo as MembershipNo, m.NameToAppear as NameToAppear, SUM(credit) as Credit,"
                + "SUM(debit) as Debit, SUM(credit - debit) as Balance, m.Home as Home, m.Mobile as Mobile FROM"
                + "("
                + "    select MembershipNo,isnull(SUM(amount),0.00) as credit, 0 as debit from"
                + "        OrderHdr a inner join ReceiptDet b "
                + "        on a.OrderHdrID = b.ReceiptHdrID  "
                + "        where "
                + "        a.IsVoided=0 and PaymentType = 'INSTALLMENT' "
                + "        group by MembershipNo "
                + "UNION ALL"
                + "    select MembershipNo, 0 as credit, isnull(sum(amount),0.00) as debit"
                + "        from OrderHdr a inner join OrderDet b on"
                + "        a.OrderHdrID = b.OrderHdrID "
                + "        where "
                + "        a.IsVoided=0 and b.IsVoided=0 "
                + "        and ItemNo = 'INST_PAYMENT' "
                + "        group by MembershipNo"
                + ") AS dt inner join Membership m on dt.MembershipNo = m.MembershipNo"
                //+ " where dt.MembershipNo LIKE '%" + MembershipNo + "%' "
                + " where dt.MembershipNo = '" + MembershipNo + "' "
                + " group by dt.MembershipNo, m.NameToAppear, m.home, m.mobile"
                + " HAVING SUM(credit - debit) <> 0";


                dt.Load(SubSonic.DataService.GetReader(new QueryCommand(SQL)));
                if (dt.Rows.Count > 0)
                {
                    decimal rs = 0;
                    if (decimal.TryParse(dt.Rows[0]["Balance"].ToString(), out rs))
                    {
                        return rs;
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return 0;
                }


            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return 0;
            }
        }

        #region Moved to PackageController
        //public static int UpdatePoints(DataTable PointData, string OrderHdrID, DateTime TransactionDate
        //    , int ValidPeriods, string MembershipNo, string SalesPersonID, string UserName, bool isSendToServer, out string Status)
        //{
        //    Status = "";
        //    try
        //    {
        //        if (PointData.Columns.Count != 3)
        //            throw new Exception("Input data format is wrong. Need 3 Columns (string,decimal,string)");

        //        if (PointData.Columns[0].DataType != Type.GetType("System.String"))
        //            throw new Exception("Input data format is wrong. Need 3 Columns (string,decimal,string)");
        //        if (PointData.Columns[1].DataType != Type.GetType("System.Decimal"))
        //            throw new Exception("Input data format is wrong. Need 3 Columns (string,decimal,string)");
        //        if (PointData.Columns[2].DataType != Type.GetType("System.String"))
        //            throw new Exception("Input data format is wrong. Need 3 Columns (string,decimal,string)");

        //        if (PointData.Rows.Count < 1)
        //            return 0;

        //        if (isSendToServer)
        //        {
        //            try
        //            {
        //                SyncClientController.Load_WS_URL();
        //                PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
        //                ws.Timeout = 100000;
        //                ws.Url = SyncClientController.WS_URL;
        //                return ws.UpdatePoints(PointData, OrderHdrID, TransactionDate, ValidPeriods
        //                    , MembershipNo, SalesPersonID, UserName, out  Status);
        //            }
        //            catch
        //            {
        //                Status = "Cannot connect to server";
        //                return 0;
        //            }
        //        }
        //        else
        //        {
        //            DateTime StartValidPeriod, EndValidPeriod;
        //            #region *) Initialize: Set StartValidPeriod & EndValidPeriod
        //            if (ValidPeriods < 1)
        //            {   /// From 1 Jan 2000 till 31 Dec 2099
        //                StartValidPeriod = DateTime.Parse(StartDateString);
        //                EndValidPeriod = StartValidPeriod.AddYears(100).AddMilliseconds(-1);
        //            }
        //            else
        //            {   /// From TransactionDate till Expiry
        //                StartValidPeriod = TransactionDate;
        //                EndValidPeriod = StartValidPeriod.AddMonths(ValidPeriods).AddMilliseconds(-1);
        //            }
        //            #endregion

        //            SortedList<string, decimal> Datas = new SortedList<string, decimal>();
        //            QueryCommandCollection Cmds = new QueryCommandCollection();
        //            foreach (DataRow Rw in PointData.Rows)
        //            {
        //                decimal DiffPoint = 0;
        //                if (!decimal.TryParse(Rw[1].ToString(), out DiffPoint))
        //                    throw new Exception("Cannot parse Point Value for Package " + Rw[0].ToString());

        //                if (DiffPoint == 0) continue;

        //                DataSet ds = MembershipPoint.Query().
        //                    WHERE("StartValidPeriod", Comparison.LessOrEquals, TransactionDate).
        //                    AND("EndValidPeriod", Comparison.GreaterOrEquals, TransactionDate).
        //                    AND(MembershipPoint.Columns.MembershipNo, MembershipNo).
        //                    AND(MembershipPoint.Columns.Userfld1, Rw[0].ToString()).
        //                    ORDER_BY("EndValidPeriod", "ASC").
        //                    ExecuteDataSet();

        //                if (DiffPoint > 0)
        //                {
        //                    #region *) Option 01: Points exist in system [Do Update]
        //                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //                    {
        //                        int CurrentPointID = int.Parse(ds.Tables[0].Rows[0]["PointID"].ToString());
        //                        decimal CurrentPoint = decimal.Parse(ds.Tables[0].Rows[0]["Points"].ToString());

        //                        Query qry = new Query("MembershipPoints");
        //                        qry.QueryType = QueryType.Update;
        //                        qry.AddWhere(MembershipPoint.Columns.PointID, CurrentPointID);
        //                        qry.AddUpdateSetting("Points", CurrentPoint + DiffPoint);
        //                        qry.AddUpdateSetting("ModifiedBy", UserName);
        //                        qry.AddUpdateSetting("ModifiedOn", DateTime.Now);
        //                        Cmds.Add(qry.BuildUpdateCommand());
        //                    }
        //                    #endregion
        //                    #region *) Option 02: Points do not exist in system [Do Create]
        //                    else
        //                    {
        //                        MembershipPoint mp = new MembershipPoint();
        //                        mp.StartValidPeriod = StartValidPeriod;
        //                        mp.EndValidPeriod = EndValidPeriod;
        //                        mp.MembershipNo = MembershipNo;
        //                        mp.Points = DiffPoint;
        //                        mp.Userfld1 = Rw[0].ToString();
        //                        mp.Userfld2 = Rw[2].ToString();
        //                        mp.IsNew = true;
        //                        Cmds.Add(mp.GetInsertCommand(UserName));
        //                    }
        //                    #endregion
        //                }
        //                else if (DiffPoint < 0)
        //                {
        //                    decimal absDiffPoint = Math.Abs(DiffPoint);

        //                    #region *) Option 01: Points exist in system [Do Update]
        //                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //                    {
        //                        int CurrentPointID;
        //                        decimal CurrentPoint;

        //                        int i = 0;
        //                        while (absDiffPoint > 0)
        //                        {
        //                            #region ^) Terminator: Point insufficient
        //                            if (absDiffPoint > 0 & i == ds.Tables[0].Rows.Count)
        //                                throw new Exception("Insufficient points to be deducted");
        //                            #endregion

        //                            CurrentPoint = decimal.Parse(ds.Tables[0].Rows[i]["Points"].ToString());
        //                            CurrentPointID = int.Parse(ds.Tables[0].Rows[i]["PointID"].ToString());

        //                            if (absDiffPoint >= CurrentPoint)
        //                            {
        //                                //do update 
        //                                //and continue the iteration
        //                                //points = 0 - points to be deducted is bigger than the existing points
        //                                Query qry = new Query("MembershipPoints");
        //                                qry.AddWhere(MembershipPoint.Columns.PointID, CurrentPointID);
        //                                qry.AddUpdateSetting("Points", 0);
        //                                Cmds.Add(qry.BuildUpdateCommand());
        //                                absDiffPoint = absDiffPoint - CurrentPoint;
        //                            }
        //                            else
        //                            {
        //                                //do update 
        //                                //points = points - deducted points  
        //                                Query qry = new Query("MembershipPoints");
        //                                qry.AddWhere(MembershipPoint.Columns.PointID, CurrentPointID);
        //                                qry.AddUpdateSetting("Points", CurrentPoint - absDiffPoint);
        //                                Cmds.Add(qry.BuildUpdateCommand());
        //                                absDiffPoint = 0;
        //                            }
        //                            i += 1;
        //                        }
        //                    }
        //                    #endregion
        //                    #region *) Option 02: Points do not exist in system [Send Error]
        //                    else
        //                    {
        //                        throw new Exception("Insufficient points to be deducted");
        //                    }
        //                    #endregion


        //                }

        //                #region *) Core: Save Point Allocation Log
        //                PointAllocationLog PointLogger = new PointAllocationLog();
        //                PointLogger.AllocationDate = TransactionDate;
        //                PointLogger.OrderHdrID = OrderHdrID;
        //                PointLogger.MembershipNo = MembershipNo;
        //                PointLogger.PointAllocated = DiffPoint;
        //                PointLogger.UniqueID = Guid.NewGuid();
        //                PointLogger.Userfld1 = Rw[0].ToString();
        //                PointLogger.Userfld2 = SalesPersonID;
        //                PointLogger.Userfld3 = "";
        //                PointLogger.Userfld4 = "";
        //                PointLogger.Userfld5 = "";
        //                PointLogger.Userfld6 = "";
        //                PointLogger.Userfld7 = "";
        //                PointLogger.Userfld8 = "";
        //                PointLogger.Userfld9 = "";
        //                PointLogger.Userfld10 = "";
        //                Cmds.Add(PointLogger.GetInsertCommand(UserName));
        //                #endregion
        //            }

        //            DataService.ExecuteTransaction(Cmds);
        //        }

        //        return PointData.Rows.Count;
        //    }
        //    catch (Exception X)
        //    {
        //        Status = X.Message;
        //        return 0;
        //    }
        //}

        //public static bool RevertPoints(string OrderHdrID, DateTime TransactionDate, string MembershipNo, string SalesPersonID, string UserName, out string Status)
        //{
        //    Status = "";
        //    try
        //    {
        //        string QueryStr =
        //            "SELECT PointAllocationLog.userfld1,0-PointAllocated,Item.userfld10 " +
        //            "FROM PointAllocationLog " +
        //                "INNER JOIN Item ON PointAllocationLog.userfld1 = Item.ItemNo " +
        //            //"FROM MembershipPoints " +
        //            //    "INNER JOIN PointAllocationLog ON MembershipPoints.MembershipNo = PointAllocationLog.MembershipNo AND MembershipPoints.userfld1 = PointAllocationLog.userfld1 " +
        //            "WHERE PointAllocationLog.OrderHdrID = @OrderHdrID ";

        //        QueryCommand Cmd = new QueryCommand(QueryStr);
        //        Cmd.AddParameter("@OrderHdrID", OrderHdrID, DbType.String);
        //        Cmd.AddParameter("@MembershipNo", MembershipNo, DbType.String);

        //        DataTable dt = new DataTable("Revert");
        //        dt.Load(DataService.GetReader(Cmd));

        //        if (dt.Rows.Count < 1) return true;

        //        bool Result = (PackageController.UpdateAll(dt, OrderHdrID, TransactionDate, 0, MembershipNo, SalesPersonID, UserName, out Status));

        //        return Result;
        //    }
        //    catch (Exception X)
        //    {
        //        Status = X.Message;
        //        return false;
        //    }

        //}
        #endregion
    }
}
