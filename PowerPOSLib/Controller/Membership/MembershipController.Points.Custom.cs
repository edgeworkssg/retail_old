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

namespace PowerPOS
{
    public partial class MembershipController
    {

        public const string REDEEM_BARCODE = "REDEEM";     //            
        public const string RENEWAL_BARCODE = "RENEWAL";
        public static decimal POINT_PERCENTAGE = 1.0M;

        public bool redeemPoint
            (int redemptionItemID, string membershipNo, DateTime redemptionDate,
            string redeemBy, string deliveryAddress, string contactNo, out string status)
        {
            try
            {
                status = "";

                //check if redemptionItem is valid for given date
                RedemptionItem redeemItem = new RedemptionItem(redemptionItemID);
                if (redeemItem.IsNew || !redeemItem.IsLoaded)
                {
                    status = "Redemption Item does not exist";
                    return false;
                }
                if (redeemItem.ValidStartDate > redemptionDate ||
                    redeemItem.ValidEndDate < redemptionDate)
                {
                    status = "The item requested is no longer valid for redemption.";
                    return false;
                }

                //check if member has enough points
                decimal points = GetCurrentPoint(membershipNo, redemptionDate, out status);
                if (status != "")
                {
                    return false;
                }

                if (points < redeemItem.PointRequired)
                {
                    status = "Insufficient point to redeem the item";
                    return false;
                }
                //deduct point
                if (!DeductPoints(membershipNo, redemptionDate, redeemItem.PointRequired, out status))
                {
                    return false;
                }

                //log it down
                RedeemLog rdLog = new RedeemLog();
                rdLog.RedeemDate = redemptionDate;
                rdLog.RedemptionId = redemptionItemID;
                rdLog.PointsBefore = points;
                rdLog.MembershipNo = membershipNo;
                rdLog.PointsAfter = GetCurrentPoint(membershipNo, redemptionDate.AddSeconds(10), out status);
                rdLog.ContactNo = contactNo;
                rdLog.DeliveryAddress = deliveryAddress;
                rdLog.IsNew = true;
                rdLog.Save(redeemBy);

                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.ToString();
                return false;
            }
        }
        public bool UndoRedemption(int redeemLogID, string username, out string status)
        {
            try
            {
                status = "";

                //read log file
                RedeemLog rd = new RedeemLog(redeemLogID);
                if (rd.IsNew || !rd.IsLoaded)
                {
                    status = "Redemption record does not exist in the server";
                    return false;
                }
                if (rd.Deleted == true)
                {
                    status = "Redemption record has already been cancelled before";
                    return false;
                }
                if (!rd.RedeemDate.HasValue)
                {
                    throw new Exception("Redemption date is not specified");
                }

                if (!rd.PointsBefore.HasValue)
                {
                    throw new Exception("PointsBefore is not specified");
                }
                if (!rd.PointsAfter.HasValue)
                {
                    throw new Exception("PointsAfter is not specified");
                }

                //add back the point
                if (!AddPoints(
                    rd.MembershipNo,
                    new DateTime(rd.RedeemDate.Value.Year, 1, 1),
                    new DateTime(rd.RedeemDate.Value.Year + 1, 1, 1),
                    rd.PointsBefore.Value - rd.PointsAfter.Value, "",
                    username, out status))
                {
                    return false;
                }

                rd.Deleted = true;
                rd.Save();
                //remove delivery request
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return false;
            }
        }
        public static bool AddPoints(
            string membershipno, DateTime StartValidPeriod,
            DateTime EndValidPeriod, decimal points, string OrderHdrID, string UserName, out string status)
        {
            try
            {
                //Check if MembershipNo is valid in the system
                Membership member = new Membership(Membership.Columns.MembershipNo, membershipno);

                if (!member.IsLoaded)
                {
                    status = "Membership Number do not exist.";
                    return false;
                }

                //points exist in system? - must be the same date range
                DataSet ds = MembershipPoint.Query().
                    WHERE("StartValidPeriod", StartValidPeriod).
                    AND("EndValidPeriod", EndValidPeriod).
                    AND(PowerPOS.MembershipPoint.Columns.MembershipNo, member.MembershipNo).ExecuteDataSet();

                QueryCommandCollection cmd = new QueryCommandCollection();

                //yes it is existing
                if (ds != null &&
                    ds.Tables.Count > 0 &&
                    ds.Tables[0].Rows.Count > 0)
                {
                    decimal CurrentPoint =
                        decimal.Parse(ds.Tables[0].Rows[0]["Points"].ToString());
                    int CurrentPointID =
                        int.Parse(ds.Tables[0].Rows[0]["PointID"].ToString());

                    //do update 
                    //points = points + added points   
                    Query qry = new Query("MembershipPoints");
                    qry.QueryType = QueryType.Update;
                    qry.AddWhere(MembershipPoint.Columns.PointID, CurrentPointID);
                    qry.AddUpdateSetting("Points", CurrentPoint + points);
                    qry.AddUpdateSetting("ModifiedBy", UserName);
                    qry.AddUpdateSetting("ModifiedOn", DateTime.Now);
                    cmd.Add(qry.BuildUpdateCommand());
                }
                else
                {
                    //else do insert
                    MembershipPoint mp = new MembershipPoint();
                    mp.StartValidPeriod = StartValidPeriod;
                    mp.EndValidPeriod = EndValidPeriod;
                    mp.MembershipNo = member.MembershipNo;
                    mp.Points = points;
                    mp.IsNew = true;
                    cmd.Add(mp.GetInsertCommand(UserName));

                    /*
                    MembershipPoint.Create(
                        StartValidPeriod, EndValidPeriod, member.MembershipNo, 
                        points, DateTime.Now,
                        UserInfo.username, DateTime.Now, UserInfo.username, "", "", 
                        "", "", "", "", "", "", "", "", null, null, null, null, null, 
                        null, null, null, null, null, null, null, null, null, null);
                     */
                }
                if (OrderHdrID != "")
                {
                    Query qry = new Query("OrderHdr");
                    qry.QueryType = QueryType.Update;
                    qry.AddWhere(OrderHdr.Columns.OrderHdrID, OrderHdrID);
                    qry.AddUpdateSetting(OrderHdr.Columns.IsPointAllocated, true);
                    cmd.Add(qry.BuildUpdateCommand());
                }

                DataService.ExecuteTransaction(cmd);

                status = "";
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return false;
            }
        }
        public static bool DeductPoints(
            string membershipno, DateTime DeductionDate,
            decimal points, out string status)
        {

            try
            {
                //Check if MembershipNo is valid in the system
                Membership member = new Membership(Membership.Columns.MembershipNo, membershipno);

                if (!member.IsLoaded)
                {
                    status = "Membership Number do not exist.";
                    return false;
                }

                //do update 
                //points = points - deduction points
                //points exist in system?

                DataSet ds = MembershipPoint.Query().
                    WHERE("StartValidPeriod", Comparison.LessOrEquals, DeductionDate).
                    AND("EndValidPeriod", Comparison.GreaterOrEquals, DeductionDate).
                    AND(MembershipPoint.Columns.MembershipNo, member.MembershipNo).ORDER_BY("EndValidPeriod", "ASC").
                    ExecuteDataSet();

                //yes it is existing
                if (ds != null &&
                    ds.Tables.Count > 0 &&
                    ds.Tables[0].Rows.Count > 0)
                {
                    decimal CurrentPoint;
                    int CurrentPointID;

                    QueryCommandCollection cmd = new QueryCommandCollection();
                    int i = 0;
                    while (points > 0)
                    {
                        if (points > 0 & i == ds.Tables[0].Rows.Count)
                        {
                            status = "Insufficient points to be deducted";
                            cmd.Clear();
                            return false;
                        }
                        CurrentPoint =
                            decimal.Parse(ds.Tables[0].Rows[i]["Points"].ToString());

                        CurrentPointID =
                            int.Parse(ds.Tables[0].Rows[i]["PointID"].ToString());

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
                    SubSonic.DataService.ExecuteTransaction(cmd);
                }

                status = "";
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return false;
            }
        }
        public static bool UpdateRenewalExpiryDate()
        {
            QueryCommandCollection cmd = new QueryCommandCollection();

            MembershipRenewalCollection rw = new MembershipRenewalCollection();
            rw.Where("Deleted", false);
            rw.Load();
            for (int i = 0; i < rw.Count; i++)
            {
                //perform renewal.
                Membership mbr = new Membership("MembershipNo", rw[i].MembershipNo);
                if (!mbr.IsNew)
                {
                    mbr.ExpiryDate = rw[i].NewExpiryDate;
                    cmd.Add(mbr.GetUpdateCommand("SYSTEM"));
                }
                rw[i].Deleted = true;
                cmd.Add(rw[i].GetUpdateCommand("SYSTEM"));
            }

            //Execute transaction
            DataService.ExecuteTransaction(cmd);

            return true;
        }
        // Run in scheduler
        public static bool AllocatePointsFromSales()
        {
            try
            {
                string status;

                OrderHdrCollection oHdr = new OrderHdrCollection();
                oHdr.Where(OrderHdr.Columns.MembershipNo, Comparison.IsNot, null);
                oHdr.Where(OrderHdr.Columns.IsPointAllocated, false);
                oHdr.Where(OrderHdr.Columns.IsVoided, false);
                oHdr.OrderByAsc(OrderHdr.Columns.OrderDate);
                oHdr.Load();
                DateTime StartDate;
                Membership m;

                for (int i = 0; i < oHdr.Count; i++)
                {
                    m = new Membership(Membership.Columns.MembershipNo, oHdr[i].MembershipNo);
                    if (m.ExpiryDate.HasValue)
                    {
                        StartDate = new DateTime(DateTime.Today.Year, 1, 1);
                        if (m.MembershipGroup.Userfloat1.HasValue)
                        {
                            POINT_PERCENTAGE = m.MembershipGroup.Userfloat1.Value;
                        }
                        else
                        {
                            POINT_PERCENTAGE = 0.0M;
                        }
                        if (MembershipController.AddPoints(oHdr[i].MembershipNo,
                            StartDate,
                            StartDate.AddYears(1),
                            oHdr[i].NettAmount * POINT_PERCENTAGE, oHdr[i].OrderHdrID, "SYSTEM", out status))
                        {

                            PointAllocationLog.Insert(oHdr[i].OrderDate, oHdr[i].OrderHdrID,
                                oHdr[i].MembershipNo, oHdr[i].NettAmount * POINT_PERCENTAGE,
                                DateTime.Now, DateTime.Now, "SYSTEM", "SYSTEM", Guid.NewGuid(),
                                "", "", "", "", "", "", "", "", "", "", null, null, null, null,
                                null, null, null, null, null, null, null, null, null, null, null);
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return false;
            }
        }

        public static DataTable FetchValidMembersPointsByDate(
                DateTime startPeriod, DateTime endPeriod,
                string membershipno, string groupname, string nametoappear,
                string firstname, string lastname, string nric, string SortColumn, string SortDir)
        {
            return FetchValidMembersPointsByDate(
                startPeriod, endPeriod,
                membershipno, groupname, nametoappear,
                firstname, lastname, nric, SortColumn, SortDir, false);
        }

        public static DataTable FetchValidMembersPointsByDate(
                DateTime startPeriod, DateTime endPeriod,
                string membershipno, string groupname, string nametoappear,
                string firstname, string lastname, string nric, string SortColumn, string SortDir, bool includeZeroPoints)
        {
            if (membershipno == "") { membershipno = "%"; }
            if (groupname == "") { groupname = "%"; }
            if (nametoappear == "") { nametoappear = "%"; }
            if (firstname == "") { firstname = "%"; }
            if (lastname == "") { lastname = "%"; }
            if (nric == "") { nric = "%"; }

            #region -= SQL String =-
            string sqlFetchData =
                "SELECT SUM(MembershipPoints.Points) as TotalPoints " +
                    ", Membership.MembershipNo " +
                    ", MembershipPoints.userfld1 AS PointID, Item.ItemName " +
                    ", MembershipPoints.userfld2 AS PointType " +
                    ", MembershipPoints.EndValidPeriod AS ExpiryDate " +
                    ", MembershipGroup.GroupName " +
                    ", Membership.NameToAppear, Membership.FirstName " +
                    ", Membership.LastName, Membership.NRIC,isnull(Item.Userfloat3,0) as BreakdownPrice, isnull(Item.Userfloat3,1) * SUM(MembershipPoints.Points) as TotalValue " +
                "FROM Membership " +
                    "INNER JOIN MembershipGroup ON Membership.MembershipGroupId = MembershipGroup.MembershipGroupId  " +
                    "INNER JOIN MembershipPoints ON Membership.MembershipNo = MembershipPoints.MembershipNo " +
                    "INNER JOIN Item ON MembershipPoints.userfld1 = Item.ItemNo  " +
                "WHERE StartValidPeriod <= @startValidPeriod AND EndValidPeriod >= @endValidPeriod " +
                    "AND Membership.MembershipNo LIKE @membershipno  " +
                    "AND MembershipGroup.GroupName LIKE @groupname " +
                    "AND Membership.NameToAppear LIKE @nametoappear " +
                    "AND ISNULL(Membership.FirstName,'') LIKE @firstname " +
                    "AND ISNULL(Membership.LastName,'') LIKE @lastname " +
                    "AND ISNULL(Membership.NRIC,'') LIKE @nric " +
                "GROUP BY Membership.MembershipNo, MembershipPoints.userfld1, Item.ItemName, MembershipPoints.userfld2, MembershipGroup.GroupName, " +
                    "Membership.NameToAppear,Membership.FirstName, MembershipPoints.EndValidPeriod, " +
                    "Membership.LastName, Membership.NRIC,item.userfloat3 ";

            if (!includeZeroPoints)
                sqlFetchData += "HAVING SUM(MembershipPoints.Points) > 0 ";

            if (SortColumn == "MembershipNo" && SortDir == "DESC")
                sqlFetchData += "ORDER BY RANK() OVER (ORDER BY Membership.MembershipNo DESC)";
            else if (SortColumn == "MembershipNo" && SortDir == "ASC")
                sqlFetchData += "ORDER BY RANK() OVER (ORDER BY Membership.MembershipNo ASC)";
            else if (SortColumn == "GroupName" && SortDir == "DESC")
                sqlFetchData += "ORDER BY RANK() OVER (ORDER BY MembershipGroup.GroupName DESC)";
            else if (SortColumn == "GroupName" && SortDir == "ASC")
                sqlFetchData += "ORDER BY RANK() OVER (ORDER BY MembershipGroup.GroupName ASC)";
            else if (SortColumn == "NameToAppear" && SortDir == "DESC")
                sqlFetchData += "ORDER BY RANK() OVER (ORDER BY Membership.NameToAppear DESC)";
            else if (SortColumn == "NameToAppear" && SortDir == "ASC")
                sqlFetchData += "ORDER BY RANK() OVER (ORDER BY Membership.NameToAppear ASC)";
            else if (SortColumn == "FirstName" && SortDir == "DESC")
                sqlFetchData += "ORDER BY RANK() OVER (ORDER BY Membership.FirstName DESC)";
            else if (SortColumn == "FirstName" && SortDir == "ASC")
                sqlFetchData += "ORDER BY RANK() OVER (ORDER BY Membership.FirstName ASC)";
            else if (SortColumn == "LastName" && SortDir == "DESC")
                sqlFetchData += "ORDER BY RANK() OVER (ORDER BY Membership.LastName DESC)";
            else if (SortColumn == "LastName" && SortDir == "ASC")
                sqlFetchData += "ORDER BY RANK() OVER (ORDER BY Membership.LastName ASC)";
            else if (SortColumn == "NRIC" && SortDir == "DESC")
                sqlFetchData += "ORDER BY RANK() OVER (ORDER BY Membership.Nric DESC)";
            else if (SortColumn == "NRIC" && SortDir == "ASC")
                sqlFetchData += "ORDER BY RANK() OVER (ORDER BY Membership.Nric ASC)";
            else if (SortColumn == "TotalPoints" && SortDir == "DESC")
                sqlFetchData += "ORDER BY RANK() OVER (ORDER BY MembershipPoints.Points DESC)";
            else if (SortColumn == "TotalPoints" && SortDir == "ASC")
                sqlFetchData += "ORDER BY RANK() OVER (ORDER BY MembershipPoints.Points ASC)";
            else
                sqlFetchData += "ORDER BY RANK() OVER (ORDER BY Membership.MembershipNo ASC)";
            #endregion

            QueryCommand cmdFetchData = new QueryCommand(sqlFetchData);
            cmdFetchData.AddParameter("@startValidPeriod", startPeriod, DbType.DateTime);
            cmdFetchData.AddParameter("@endValidPeriod", endPeriod, DbType.DateTime);
            cmdFetchData.AddParameter("@membershipno", membershipno, DbType.String);
            cmdFetchData.AddParameter("@groupname", groupname, DbType.String);
            cmdFetchData.AddParameter("@nametoappear", nametoappear, DbType.String);
            cmdFetchData.AddParameter("@firstname", firstname, DbType.String);
            cmdFetchData.AddParameter("@lastname", lastname, DbType.String);
            cmdFetchData.AddParameter("@nric", nric, DbType.String);
            cmdFetchData.AddParameter("@sortby", SortColumn, DbType.String);
            cmdFetchData.AddParameter("@sortdir", SortDir, DbType.String);

            DataTable Output = new DataTable();
            Output.Load(DataService.GetReader(cmdFetchData));

            return Output;

            //DataSet ds = SPs.FetchValidMembersPointsByDate
            //                    (startPeriod, endPeriod, membershipno, groupname,
            //                    nametoappear, firstname, lastname, nric,
            //                    SortColumn, SortDir).GetDataSet();


            //return ds.Tables[0];
        }

        public static string[] getRemainingPackageList(string MembershipNo)
        {
            return getRemainingPackageList(MembershipNo, false);
        }

        public static string[] getRemainingPackageList(string MembershipNo, bool includeZeroRemaining)
        {
            List<string> Result = new List<string>();

            try
            {
                string QryStr;
                //IDataReader rdr;


                QryStr =
                    "SELECT DISTINCT PointAllocationLog.userfld1,ISNULL(Item.userfld10,'" + Item.PointMode.Dollar + "'), ISNULL(Item.ItemName,PointAllocationLog.userfld1) " +
                    "FROM PointAllocationLog " +
                        "LEFT JOIN Item ON PointAllocationLog.userfld1 = Item.ItemNo " +
                    "WHERE PointAllocationLog.userfld1 IS NOT NULL AND PointAllocationLog.userfld1 <> '' " +
                        " AND ISNULL(Item.Deleted,0) = 0 " +
                        " AND MembershipNo = '@MembershipNo' ".Replace("@MembershipNo", MembershipNo) +
                    "group by PointAllocationLog.userfld1, " +
                        "ISNULL(Item.userfld10,'D'), ISNULL(Item.ItemName,PointAllocationLog.userfld1) ";

                if (!includeZeroRemaining)
                    QryStr += "having sum(pointallocated) > 0 ";

                QryStr += "ORDER BY PointAllocationLog.userfld1,ISNULL(Item.userfld10,'" + Item.PointMode.Dollar + "'), ISNULL(Item.ItemName,PointAllocationLog.userfld1) ";
                //rdr = DataService.GetReader(new QueryCommand(QryStr));

                //while (rdr.Read())
                //{
                //    string Temp = rdr.GetString(0);
                //    if (!Result.Contains(Temp))
                //        Result.Add(Temp);
                //}

                QueryCommand cmd = new QueryCommand(QryStr, "PowerPOS");

                DataTable dt = DataService.GetDataSet(cmd).Tables[0];

                //loop through data table
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string line_result = "";
                    //if the userfld1 contain the string for open package, update it
                    if (dt.Rows[i][0].ToString().Contains("|OPP|") && dt.Rows[i][0].ToString().IndexOf("|OPP|") > 0)
                    {
                        //load from database
                        Item myItem = new Item(dt.Rows[i][0].ToString().Substring(0, dt.Rows[i][0].ToString().IndexOf("|OPP|")));
                        if (myItem.IsOpenPricePackage)
                        {
                            line_result = Item.PointMode.Times + dt.Rows[i][2].ToString(); // myItem.ItemName;
                            Result.Add(line_result);
                            continue;
                        }
                    }
                    else
                    {
                        line_result = dt.Rows[i][1].ToString() + dt.Rows[i][2].ToString();
                        Result.Add(line_result);
                    }
                    //add the result to the array
                }

                return Result.ToArray();
            }
            catch (Exception X)
            {
                Logger.writeLog(X);

                return null;
                //return (new List<string>()).ToArray();
            }
        }

        public static DataTable FetchValidMembersPointsByDate(
                DateTime startPeriod, DateTime endPeriod,
                string membershipno, string groupname, string nametoappear,
                string firstname, string lastname, string nric, string SortColumn, string SortDir, string PointPackage
            )
        {
            if (membershipno == "") { membershipno = "%"; }
            if (groupname == "") { groupname = "%"; }
            if (nametoappear == "") { nametoappear = "%"; }
            if (firstname == "") { firstname = "%"; }
            if (lastname == "") { lastname = "%"; }
            if (nric == "") { nric = "%"; }

            #region -= SQL String =-
            string sqlFetchData =
                //"DECLARE @startValidPeriod "
                "SELECT SUM(pa.PointAllocated) as TotalPoints " +
                    ", Membership.MembershipNo " +
                    ", MembershipPoints.userfld1 AS PointID, " +
                    //" (case when (Item.ItemName IS NULL) THEN (SELECT Item.ItemName FROM Item  WHERE Item.ItemNo LIKE LEFT(MembershipPoints.userfld1,charindex('|', MembershipPoints.userfld1) - 1) ) ELSE Item.ItemName END) ItemName " +
                    "  (case when (Item.ItemName IS NULL) THEN (SELECT Item.ItemName FROM Item WHERE Item.ItemNo LIKE CASE WHEN charindex('|', MembershipPoints.userfld1) > 0 then LEFT(MembershipPoints.userfld1,charindex('|', MembershipPoints.userfld1) - 1) else MembershipPoints.userfld1 END) ELSE Item.ItemName END) ItemName " +
                    ", MembershipPoints.userfld2 AS PointType " +
                    ", MembershipGroup.GroupName " +
                    ", Membership.NameToAppear, Membership.FirstName " +
                    ", Membership.LastName, Membership.NRIC, " +
                    " CASE WHEN (Item.ItemName IS NULL) THEN SUM(isnull(MembershipPoints.userfloat1,0) * MembershipPoints.Points) " +
                    " / (CASE when ISNULL(SUM(MembershipPoints.Points),1) > 0 then ISNULL(SUM(MembershipPoints.Points),1) else 1 end ) ELSE isnull(Item.Userfloat3,0) END as BreakdownPrice,  " +
                    " CASE WHEN '" + PointPackage + "' = 'D' THEN (SELECT TOP 1 userfloat1 FROM item WHERE itemno = MembershipPoints.userfld1) ELSE  " +
                    " CASE WHEN (Item.ItemName IS NULL) THEN isnull(MembershipPoints.userfloat1,0) * sum(pa.pointallocated) " +
                    " ELSE isnull(Item.Userfloat3,1) * sum(pa.pointallocated) END  END as TotalValue  " +
                "FROM Membership " +
                    "INNER JOIN MembershipGroup ON Membership.MembershipGroupId = MembershipGroup.MembershipGroupId  " +
                    "RIGHT JOIN MembershipPoints ON Membership.MembershipNo = MembershipPoints.MembershipNo " +
                    "LEFT JOIN Item ON MembershipPoints.userfld1 = Item.ItemNo  " +
                    "LEFT JOIN PointAllocationLog pa on membership.membershipno = pa.membershipno and membershippoints.userfld1 = pa.userfld1 " +
                "WHERE StartValidPeriod <= @startValidPeriod AND EndValidPeriod >= @endValidPeriod " +
                    "AND pa.AllocationDate <= @endValidPeriod " +
                    "AND Membership.MembershipNo LIKE @membershipno AND MembershipPoints.userfld1 <> '' " +
                    "AND MembershipGroup.GroupName LIKE @groupname " +
                    "AND ISNULL(Membership.NameToAppear,'') LIKE @nametoappear " +
                    "AND ISNULL(Membership.FirstName,'') LIKE @firstname " +
                    "AND ISNULL(Membership.LastName,'') LIKE @lastname " +
                    "AND ISNULL(Membership.NRIC,'') LIKE @nric AND ISNULL(MembershipPoints.Userfld2,'') = '" + PointPackage + "' " +
                "GROUP BY Membership.MembershipNo, MembershipPoints.userfld1, Item.ItemName, MembershipPoints.userfld2, MembershipGroup.GroupName, " +
                    "Membership.NameToAppear,Membership.FirstName, " +
                    "Membership.LastName, Membership.NRIC,item.userfloat3, MembershipPoints.Userfloat1, MembershipPoints.Userfloat3 ";

            if (SortColumn == "MembershipNo" && SortDir == "DESC")
                sqlFetchData += "ORDER BY RANK() OVER (ORDER BY Membership.MembershipNo DESC)";
            else if (SortColumn == "MembershipNo" && SortDir == "ASC")
                sqlFetchData += "ORDER BY RANK() OVER (ORDER BY Membership.MembershipNo ASC)";
            else if (SortColumn == "GroupName" && SortDir == "DESC")
                sqlFetchData += "ORDER BY RANK() OVER (ORDER BY MembershipGroup.GroupName DESC)";
            else if (SortColumn == "GroupName" && SortDir == "ASC")
                sqlFetchData += "ORDER BY RANK() OVER (ORDER BY MembershipGroup.GroupName ASC)";
            else if (SortColumn == "NameToAppear" && SortDir == "DESC")
                sqlFetchData += "ORDER BY RANK() OVER (ORDER BY Membership.NameToAppear DESC)";
            else if (SortColumn == "NameToAppear" && SortDir == "ASC")
                sqlFetchData += "ORDER BY RANK() OVER (ORDER BY Membership.NameToAppear ASC)";
            else if (SortColumn == "FirstName" && SortDir == "DESC")
                sqlFetchData += "ORDER BY RANK() OVER (ORDER BY Membership.FirstName DESC)";
            else if (SortColumn == "FirstName" && SortDir == "ASC")
                sqlFetchData += "ORDER BY RANK() OVER (ORDER BY Membership.FirstName ASC)";
            else if (SortColumn == "LastName" && SortDir == "DESC")
                sqlFetchData += "ORDER BY RANK() OVER (ORDER BY Membership.LastName DESC)";
            else if (SortColumn == "LastName" && SortDir == "ASC")
                sqlFetchData += "ORDER BY RANK() OVER (ORDER BY Membership.LastName ASC)";
            else if (SortColumn == "NRIC" && SortDir == "DESC")
                sqlFetchData += "ORDER BY RANK() OVER (ORDER BY Membership.Nric DESC)";
            else if (SortColumn == "NRIC" && SortDir == "ASC")
                sqlFetchData += "ORDER BY RANK() OVER (ORDER BY Membership.Nric ASC)";
            else if (SortColumn == "TotalPoints" && SortDir == "DESC")
                sqlFetchData += "ORDER BY RANK() OVER (ORDER BY MembershipPoints.Points DESC)";
            else if (SortColumn == "TotalPoints" && SortDir == "ASC")
                sqlFetchData += "ORDER BY RANK() OVER (ORDER BY MembershipPoints.Points ASC)";
            else
                sqlFetchData += "ORDER BY RANK() OVER (ORDER BY Membership.MembershipNo ASC)";
            #endregion

            QueryCommand cmdFetchData = new QueryCommand(sqlFetchData);
            cmdFetchData.AddParameter("@startValidPeriod", startPeriod, DbType.DateTime);
            cmdFetchData.AddParameter("@endValidPeriod", endPeriod, DbType.DateTime);
            cmdFetchData.AddParameter("@membershipno", membershipno, DbType.String);
            cmdFetchData.AddParameter("@groupname", groupname, DbType.String);
            cmdFetchData.AddParameter("@nametoappear", nametoappear, DbType.String);
            cmdFetchData.AddParameter("@firstname", firstname, DbType.String);
            cmdFetchData.AddParameter("@lastname", lastname, DbType.String);
            cmdFetchData.AddParameter("@nric", nric, DbType.String);
            cmdFetchData.AddParameter("@sortby", SortColumn, DbType.String);
            cmdFetchData.AddParameter("@sortdir", SortDir, DbType.String);

            DataTable Output = new DataTable();
            Output.Load(DataService.GetReader(cmdFetchData));

            return Output;

            //DataSet ds = SPs.FetchValidMembersPointsByDate
            //                    (startPeriod, endPeriod, membershipno, groupname,
            //                    nametoappear, firstname, lastname, nric,
            //                    SortColumn, SortDir).GetDataSet();


            //return ds.Tables[0];
        }

        #region "AHAVA Point System"
        public static bool IsValidToMakeRedemptionAHAVA(string membershipNo, DateTime redeemDate, out string status)
        {
            Membership m = new Membership(Membership.Columns.MembershipNo, membershipNo);
            if (!m.IsLoaded)
            {
                status = "Members does not exist";
                return false;
            }

            /*Check birthday month*/
            if (!m.DateOfBirth.HasValue || m.DateOfBirth.Value.Month != redeemDate.Month)
            {
                if (m.DateOfBirth.HasValue)
                {
                    status = "Wrong birthday month. This member birthday month is " + m.DateOfBirth.Value.Month;
                }
                else
                {
                    status = "Wrong birthday month. This member birthday is not specified.";
                }
                return false;
            }
            status = "";
            return true;
        }
        public static bool hasAlreadyPerformedRedemptionForAHAVA(string membershipno)
        {
            DataSet ds = new Query("RedemptionLog").WHERE(RedemptionLog.Columns.MembershipNo, membershipno)
                .AND(RedemptionLog.Columns.RedemptionDate,
                    Comparison.GreaterOrEquals, new DateTime(DateTime.Now.Year, 1, 1))
                .AND(RedemptionLog.Columns.RedemptionDate,
                    Comparison.LessOrEquals, new DateTime(DateTime.Now.Year, 12, 1))
                .ExecuteDataSet();

            if (ds.Tables[0].Rows.Count > 0)
                return true;
            else
                return false;
        }


        public static bool processAHAVARedemptionLog()
        {

            //Check on the server side, things that are not updated
            string status;

            RedemptionLogCollection rdp = new RedemptionLogCollection();
            rdp.Where(RedemptionLog.Columns.Updated, false);
            rdp.Load();

            for (int i = 0; i < rdp.Count; i++)
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    if (DeductPoints(rdp[i].MembershipNo, rdp[i].RedemptionDate.Value, rdp[i].Points, out status))
                    {
                        Query qr = RedemptionLog.CreateQuery();
                        qr.AddUpdateSetting(RedemptionLog.Columns.Updated, true);
                        qr.AddWhere(RedemptionLog.Columns.RedemptionLogID, rdp[i].RedemptionLogID);
                        qr.Execute();
                        //rdp[i].Updated = true;
                        //rdp[i].Save();
                        ts.Complete();
                    }
                }
            }
            //
            return true;
        }
        public static DataTable FetchAHAVARedemptionLog(
            bool useStartDate, bool useEndDate,
            DateTime StartDate, DateTime EndDate,
            string MembershipNo, string membershipGroup,
            string NameToAppear, string FirstName, string LastName, string NRIC,
            string PointOfSaleName, string OutletName,
            string SortColumn, string SortDir)
        {
            ViewRedemptionLogCollection myViewRedemptionLog = new ViewRedemptionLogCollection();
            if (useStartDate & useEndDate)
            {
                myViewRedemptionLog.BetweenAnd(ViewRedemptionLog.Columns.RedemptionDate, StartDate, EndDate);
            }
            else if (useStartDate)
            {
                myViewRedemptionLog.Where(ViewRedemptionLog.Columns.RedemptionDate, SubSonic.Comparison.GreaterOrEquals, StartDate);
            }
            else if (useEndDate)
            {
                myViewRedemptionLog.Where(ViewRedemptionLog.Columns.RedemptionDate, SubSonic.Comparison.LessOrEquals, EndDate);
            }
            if (MembershipNo != "")
            {
                myViewRedemptionLog.Where(ViewRedemptionLog.Columns.MembershipNo, SubSonic.Comparison.Like, MembershipNo);
            }

            if (membershipGroup != "")
            {
                myViewRedemptionLog.Where(ViewRedemptionLog.Columns.GroupName, SubSonic.Comparison.Like, membershipGroup);
            }

            if (NameToAppear != "")
            {
                myViewRedemptionLog.Where(ViewRedemptionLog.Columns.NameToAppear, SubSonic.Comparison.Like, "%" + NameToAppear + "%");
            }

            if (FirstName != "")
            {
                myViewRedemptionLog.Where(ViewRedemptionLog.Columns.FirstName, SubSonic.Comparison.Like, "%" + FirstName + "%");
            }
            if (LastName != "")
            {
                myViewRedemptionLog.Where(ViewRedemptionLog.Columns.LastName, SubSonic.Comparison.Like, "%" + LastName + "%");
            }
            if (NRIC != "")
            {
                myViewRedemptionLog.Where(ViewRedemptionLog.Columns.Nric, SubSonic.Comparison.Like, "%" + NRIC + "%");
            }
            if (PointOfSaleName != "")
            {
                myViewRedemptionLog.Where(ViewRedemptionLog.Columns.PointOfSaleName, SubSonic.Comparison.Like, "%" + PointOfSaleName + "%");
            }

            if (OutletName != "")
            {
                myViewRedemptionLog.Where(ViewRedemptionLog.Columns.OutletName, SubSonic.Comparison.Like, "%" + OutletName + "%");
            }

            SubSonic.TableSchema.TableColumn t = ViewRedemptionLog.Schema.GetColumn(SortColumn);

            if (t != null)
            {
                if (SortDir.Trim() == "ASC")
                {
                    myViewRedemptionLog.OrderByAsc(SortColumn);
                }
                else if (SortDir.Trim() == "DESC")
                {
                    myViewRedemptionLog.OrderByDesc(SortColumn);
                }
                else
                {
                    myViewRedemptionLog.OrderByDesc("RedemptionDate");
                }

            }
            else
            {
                myViewRedemptionLog.OrderByDesc("RedemptionDate");
            }

            return myViewRedemptionLog.Load().ToDataTable();
        }

        public static bool RedeemAHAVAMemberPoints
           (string orderHdrId, string membershipNo, DateTime redeemDate, out string status)
        {
            Membership m = new Membership(Membership.Columns.MembershipNo, membershipNo);
            if (!m.IsLoaded)
            {
                status = "Members does not exist";
                return false;
            }

            /*Check birthday month*/
            if (!m.DateOfBirth.HasValue || m.DateOfBirth.Value.Month != redeemDate.Month)
            {
                status = "Wrong birthday month";
                return false;
            }


            //Check how much points available
            decimal points = MembershipController.GetCurrentPoint(membershipNo, DateTime.Now, out status);
            //using (TransactionScope ts = new TransactionScope())
            //{
            //deduct points accordingly
            if (DeductPoints(membershipNo, DateTime.Now, points, out status)
                && InsertAHAVARedemptionLog(orderHdrId, membershipNo, points, false))
            {
                status = "";
                //ts.Complete();
                return true;
            }
            else
            {
                status = "Unable to deduct points." + status;
                return false;
            }
            //}            
        }

        //update redemption log - pos confirm order will call this
        public static bool InsertAHAVARedemptionLog(string orderHdrId, string membershipNo, decimal points, bool deductedAtServer)
        {
            try
            {
                //perform database insert
                //Update Redemption Log
                RedemptionLog.Insert
                    (DateTime.Now, "", orderHdrId, membershipNo, points, deductedAtServer,
                    DateTime.Now, DateTime.Now, "System", "System", Guid.NewGuid(), "", "", "", "", "", "", "", "", "", "", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);

                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return false;
            }
        }
        #endregion

    }
}
