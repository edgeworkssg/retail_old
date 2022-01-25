using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Xml;
using System.Xml.Serialization;
using SubSonic;
using SubSonic.Utilities;
using PowerPOS;
using PowerPOS.Container;

namespace PowerPOS
{
   
    public partial class MembershipAttendanceController
    {
        public enum ActivityType
        {
            Login = 0,
            Logout = 1
        }

        //Check if member logged in already
        public static bool IsMemberLoggedInToday(string MembershipNo, DateTime dateFrom, DateTime dateTo)
        {
            MembershipAttendanceCollection mbr = new MembershipAttendanceCollection();
            mbr.Where(MembershipAttendance.Columns.ActivityDateTime, Comparison.GreaterOrEquals, dateFrom);
            mbr.Where(MembershipAttendance.Columns.ActivityDateTime, Comparison.LessOrEquals, dateTo);
            mbr.Where(MembershipAttendance.Columns.MembershipNo, MembershipNo);
            //mbr.Where(MembershipAttendance.Columns.ActivityName, MembershipAttendanceController.ActivityType.Login);
            mbr.Load();

            if (mbr.Count % 2 != 0) //odd number
                return true;

            return false;
        }
        
        public static bool ScanMembership(Membership  m, out string message)
        {
            message = "";
            
                        
            if (m.ExpiryDate < DateTime.Today)
            {
                //expired
                message = "Membership for [" + m.MembershipNo + "] has expired on " + m.ExpiryDate.Value.ToString("dd MMM yyyy");
                return false;
            }
            if (m.ExpiryDate < DateTime.Today.AddDays(7))
            {
                message = "Membership is expiring in " + ((TimeSpan)(m.ExpiryDate - DateTime.Today)).TotalDays + " days time [" + m.ExpiryDate.Value.ToString("dd MMM yyyy") + "]";
            }

            //save
            MembershipAttendance mbr = new MembershipAttendance();
            mbr.UniqueID = Guid.NewGuid();
            mbr.ActivityDateTime = DateTime.Now;
            mbr.MembershipNo = m.MembershipNo;
            mbr.PointOfSaleID = PointOfSaleInfo.PointOfSaleID;            
            mbr.ActivityName = MembershipAttendanceController.ActivityType.Login.ToString();            
            mbr.Deleted = false;
            mbr.Save(UserInfo.username);
            message = "Attendance for " + m.NameToAppear + " is taken successfully";
            return true;                        
        }

        /// <summary>
        /// To Check In / Check Out member
        /// </summary>
        /// <param name="m">Scanned membership</param>
        /// <exception cref="NullReferenceException"></exception>
        public static void ScanMembership(Membership m)
        {
            DateTime startTime, endTime;
            ActivityType activityType;
            ScanMembership(m, out activityType, out startTime, out endTime);
        }

        public static void ScanMembership(Membership m, out ActivityType activityType, out DateTime startTime, out DateTime endTime)
        {
            startTime = DateTime.MinValue;
            endTime = DateTime.MinValue;

            string SQL;

            #region *) Validation: Member is a registered member
            if (m == null)
                throw new NullReferenceException("Cannot find membership");

            if (m.IsNew)
                throw new NullReferenceException("Cannot find membership [" + m.MembershipNo + "]");
            #endregion

            string Tracking;
            ActivityType LastActivity;
            #region *) Fetch: Load Last Activity
            SQL = "SELECT TOP 1 * FROM MembershipAttendance WHERE MembershipNo = '" + m.MembershipNo + "' ORDER BY ActivityDateTime DESC ";
            MembershipAttendance LastLog = new MembershipAttendance();
            LastLog.LoadAndCloseReader(DataService.GetReader(new QueryCommand(SQL)));

            if (LastLog == null || LastLog.IsNew)
            { LastActivity = ActivityType.Logout; Tracking = Guid.NewGuid().ToString(); }
            else if (LastLog.ActivityName == MembershipAttendanceController.ActivityType.Login.ToString())
            { LastActivity = ActivityType.Login; Tracking = LastLog.LockerID; }
            else
            { LastActivity = ActivityType.Logout; Tracking = Guid.NewGuid().ToString(); }
            #endregion

            ActivityType CurrActivity;
            #region *) Fetch: Load Current Activity
            if (LastActivity == ActivityType.Login)
                CurrActivity = ActivityType.Logout;
            else
                CurrActivity = ActivityType.Login;
            #endregion

            #region *) Save: Save Attendance
            MembershipAttendance mbr = new MembershipAttendance();
            mbr.UniqueID = Guid.NewGuid();
            mbr.ActivityDateTime = DateTime.Now;
            mbr.MembershipNo = m.MembershipNo;
            mbr.PointOfSaleID = PointOfSaleInfo.PointOfSaleID;
            mbr.ActivityName = CurrActivity.ToString();
            mbr.LockerID = Tracking;
            mbr.Deleted = false;
            mbr.Save(UserInfo.username);
            #endregion

            activityType = CurrActivity;
            if (CurrActivity == ActivityType.Logout)
            {
                startTime = LastLog.ActivityDateTime;
                endTime = mbr.ActivityDateTime;
            }
        }

        //public static MembershipCollection FetchLoggedInMembers()
        //{
        //    string SQL = "SELECT MembershipNo, MAX(ActivityDateTime) FROM MembershipAttendance ORDER BY ActivityDateTime";
        //}

        public static ViewMembershipAttendanceCollection FetchAttendanceList
            (DateTime dateFrom, DateTime dateTo, int PointOfSaleID, string name, string membershipno)
        {
            try
            {

                ViewMembershipAttendanceCollection col = new ViewMembershipAttendanceCollection();
                col.Where(ViewMembershipAttendance.Columns.ActivityDateTime, Comparison.GreaterOrEquals, dateFrom);
                col.Where(ViewMembershipAttendance.Columns.ActivityDateTime, Comparison.LessOrEquals, dateTo);
                if (PointOfSaleID != 0) col.Where(ViewMembershipAttendance.Columns.PointOfSaleID, PointOfSaleID);
                if (name != "")
                {
                    col.Where(ViewMembership.Columns.Name, Comparison.Like, "%" + name + "%");
                }
                if (membershipno != "")
                {
                    col.Where(ViewMembership.Columns.MembershipNo, Comparison.Like, "%" + membershipno + "%");
                }
                col.OrderByDesc(ViewMembershipAttendance.Columns.ActivityDateTime);
                col.Load();

                return col;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
        }

        public static DataTable FetchMembershipAttendanceReport
            (DateTime dateFrom, DateTime dateTo, int PointOfSaleID, string name, string membershipno)
        {
            try
            {
                string sql = @"
                                DECLARE @StartDate datetime, 
                                        @EndDate datetime, 
                                        @PointOfSaleID int, 
                                        @Name nvarchar(50), 
                                        @MembershipNo varchar(50) 

                                SET @StartDate = '{0}' 
                                SET @EndDate = '{1}' 
                                SET @PointOfSaleID = {2} 
                                SET @Name = '%' + '{3}' + '%' 
                                SET @MembershipNo = '%' + '{4}' + '%' 

                                SELECT  MA.AttendanceDate, MA.LoginTime, MA.LogoutTime, 
                                CASE 
                                    WHEN MA.LogoutTime IS NULL THEN RIGHT('0' + CAST(DATEPART(DD, GETDATE() - LoginTime) - 1 AS VARCHAR(2)), 2) + 'd ' + RIGHT('0' + CAST(DATEPART(HH, GETDATE() - LoginTime) AS VARCHAR(2)), 2) + 'h '+ RIGHT('0' + CAST(DATEPART(MI, GETDATE() - LoginTime) AS VARCHAR(2)), 2) + 'm '+ RIGHT('0' + CAST(DATEPART(SS, GETDATE() - LoginTime) AS VARCHAR(2)), 2) + 's'
                                    ELSE RIGHT('0' + CAST(DATEPART(DD, LogoutTime - LoginTime) - 1 AS VARCHAR(2)), 2) + 'd ' + RIGHT('0' + CAST(DATEPART(HH, LogoutTime - LoginTime) AS VARCHAR(2)), 2) + 'h '+ RIGHT('0' + CAST(DATEPART(MI, LogoutTime - LoginTime) AS VARCHAR(2)), 2) + 'm '+ RIGHT('0' + CAST(DATEPART(SS, LogoutTime - LoginTime) AS VARCHAR(2)), 2) + 's'
                                END Duration, 
                                MA.PointOfSaleID, MA.Deleted AS AttendanceDeleted, POS.PointOfSaleName, 
                                POS.OutletName, VM.MembershipNo, VM.MembershipGroupId, 
                                VM.Title, VM.LastName, VM.FirstName, VM.ChristianName, 
                                VM.NameToAppear, VM.Gender, VM.DateOfBirth, VM.Nationality, VM.NRIC, 
                                VM.Occupation, VM.MaritalStatus, VM.Email, VM.Block, VM.BuildingName, 
                                VM.StreetName, VM.UnitNo, VM.City, VM.Country, VM.ZipCode, 
                                VM.Mobile, VM.Office, VM.Home, VM.ExpiryDate, VM.Remarks, 
                                VM.CreatedOn, VM.CreatedBy, VM.ModifiedOn, VM.ModifiedBy, 
                                VM.Deleted, VM.UniqueID, MA.LockerID, VM.Fax, 
                                VM.SubscriptionDate, VM.GroupName, VM.Discount, VM.ChineseName, 
                                VM.StreetName2, VM.Address, VM.BirthDayMonth, VM.Name 
                        FROM 
                            ( 
                                SELECT MembershipNo, PointOfSaleID, 
			                        CAST(ActivityDateTime AS date) AS AttendanceDate, 
			                        MIN(CASE WHEN ActivityName = 'Login' THEN ActivityDateTime ELSE NULL END) LoginTime, 
                                    MAX(CASE WHEN ActivityName = 'Logout' THEN ActivityDateTime ELSE NULL END) LogoutTime, 
			                        Deleted, LockerID 
                                FROM  MembershipAttendance 
                                GROUP BY MembershipNo, PointOfSaleID, CAST(ActivityDateTime AS date), Deleted, LockerID 
                            ) MA 
                        INNER JOIN PointOfSale POS ON MA.PointOfSaleID = POS.PointOfSaleID 
                        INNER JOIN ViewMembership VM ON MA.MembershipNo = VM.MembershipNo 
                        WHERE   MA.AttendanceDate BETWEEN @StartDate AND @EndDate 
                                AND (@PointOfSaleID = 0 OR MA.PointOfSaleID = @PointOfSaleID) 
                                AND VM.Name LIKE @Name 
                                AND VM.MembershipNo LIKE @MembershipNo 
                        ORDER BY MA.LoginTime  
                              ";
                sql = string.Format(sql, dateFrom.ToString("yyyy-MM-dd HH:mm:ss"),
                                         dateTo.ToString("yyyy-MM-dd HH:mm:ss"),
                                         PointOfSaleID.ToString(),
                                         name,
                                         membershipno);
                DataTable dt = DataService.GetDataSet(new QueryCommand(sql, "PowerPOS")).Tables[0];
                return dt;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
        }

        /// <summary>
        /// Fetch all members that has checked in and never check out
        /// </summary>
        /// <returns></returns>
        public static DataTable FetchActiveMember()
        {
            string SQL =
                "SELECT Membership.MembershipNo, NameToAppear MembershipName, MAX(ActivityDateTime) LoginTime "+
                    ", RIGHT('0' + CAST(DATEPART(DD, GETDATE() - MAX(ActivityDateTime)) - 1 AS VARCHAR(2)), 2) + 'd ' " +
                    "+ RIGHT('0' + CAST(DATEPART(HH, GETDATE() - MAX(ActivityDateTime)) AS VARCHAR(2)), 2) + 'h ' " +
                    "+ RIGHT('0' + CAST(DATEPART(MI, GETDATE() - MAX(ActivityDateTime)) AS VARCHAR(2)), 2) + 'm ' " +
                    "+ RIGHT('0' + CAST(DATEPART(SS, GETDATE() - MAX(ActivityDateTime)) AS VARCHAR(2)), 2) + 's' Duration " +
                    ", CASE WHEN SUM(CASE WHEN ActivityName = 'Login' THEN 1 ELSE -1 END) > 0 THEN 'Login' ELSE 'Logout' END ActivityType " +
                "FROM MembershipAttendance " +
                    "INNER JOIN Membership ON MembershipAttendance.MembershipNo = Membership.MembershipNo " +
                "GROUP BY Membership.MembershipNo, NameToAppear " +
                "HAVING SUM(CASE WHEN ActivityName = 'Login' THEN 1 ELSE -1 END) > 0 ";

            DataTable Dt = new DataTable();
            Dt.Load(DataService.GetReader(new QueryCommand(SQL)));

            return Dt;
        }
        public static DataTable FetchActiveMemberBefore(DateTime Limit)
        {
            string SQL =
                "SELECT Membership.MembershipNo, NameToAppear MembershipName, MAX(ActivityDateTime) LoginTime "+
                    ", RIGHT('0' + CAST(DATEPART(DD, GETDATE() - MAX(ActivityDateTime)) - 1 AS VARCHAR(2)), 2) + 'd ' " +
                    "+ RIGHT('0' + CAST(DATEPART(HH, GETDATE() - MAX(ActivityDateTime)) AS VARCHAR(2)), 2) + 'h ' " +
                    "+ RIGHT('0' + CAST(DATEPART(MI, GETDATE() - MAX(ActivityDateTime)) AS VARCHAR(2)), 2) + 'm ' " +
                    "+ RIGHT('0' + CAST(DATEPART(SS, GETDATE() - MAX(ActivityDateTime)) AS VARCHAR(2)), 2) + 's' Duration " +
                    ", CASE WHEN SUM(CASE WHEN ActivityName = 'Login' THEN 1 ELSE -1 END) > 0 THEN 'Login' ELSE 'Logout' END ActivityType " +
                "FROM MembershipAttendance " +
                    "INNER JOIN Membership ON MembershipAttendance.MembershipNo = Membership.MembershipNo " +
                "GROUP BY Membership.MembershipNo, NameToAppear " +
                "HAVING SUM(CASE WHEN ActivityName = 'Login' THEN 1 ELSE -1 END) > 0 " +
                    "AND MAX(ActivityDateTime) < '" + Limit.ToString("yyyy-MM-dd HH:mm:ss") + "' ";

            DataTable Dt = new DataTable();
            Dt.Load(DataService.GetReader(new QueryCommand(SQL)));

            return Dt;
        }

        /// <summary>
        /// Get Checked Out member
        /// </summary>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <param name="POSID"></param>
        /// <returns>LockerID, MembershipNo, MembershipName, LoginTime, LogoutTime, Duration</returns>
        public static DataTable FetchCheckOutMember(DateTime StartDate, DateTime EndDate, int POSID)
        {
            string SQL =
                "DECLARE @POSID INT; " +
                "DECLARE @StartDate DATETIME; " +
                "DECLARE @EndDate DATETIME; " +
                "SET @POSID = " + POSID.ToString() + " " +
                "SET @StartDate = '" + StartDate.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                "SET @EndDate = '" + EndDate.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                "SELECT * " +
                    ", RIGHT('0' + CAST(DATEPART(DD, LogoutTime - LoginTime) - 1 AS VARCHAR(2)), 2) + 'd ' " +
					"+ RIGHT('0' + CAST(DATEPART(HH, LogoutTime - LoginTime) AS VARCHAR(2)), 2) + 'h ' " +
					"+ RIGHT('0' + CAST(DATEPART(MI, LogoutTime - LoginTime) AS VARCHAR(2)), 2) + 'm ' " +
                    "+ RIGHT('0' + CAST(DATEPART(SS, LogoutTime - LoginTime) AS VARCHAR(2)), 2) + 's' Duration " +
                "FROM "+
                "( " +
                    "SELECT LockerID, Membership.MembershipNo, NameToAppear MembershipName " +
                        ", MIN(CASE WHEN ActivityName = 'Login' THEN ActivityDateTime ELSE NULL END) LoginTime " +
                        ", MAX(CASE WHEN ActivityName = 'Logout' THEN ActivityDateTime ELSE NULL END) LogoutTime " +
                    "FROM MembershipAttendance " +
                        "INNER JOIN Membership ON MembershipAttendance.MembershipNo = Membership.MembershipNo " +
                    "WHERE PointOfSaleID = @POSID " +
                        "AND ActivityDateTime BETWEEN @StartDate AND @EndDate " +
                    "GROUP BY LockerID, Membership.MembershipNo, NameToAppear " +
                ") DT " +
                "WHERE LogoutTime IS NOT NULL ";

            DataTable Dt = new DataTable();
            Dt.Load(DataService.GetReader(new QueryCommand(SQL)));

            return Dt;
        }

        /// <summary>
        /// Get the "all time" last check out member
        /// </summary>
        /// <returns>MembershipNo, MembershipName, LoginTime, LogoutTime, Duration</returns>
        public static DataRow FetchLastCheckOutMember()
        {
            string SQL =
                "SELECT * " +
                    ", RIGHT('0' + CAST(DATEPART(DD, LogoutTime - LoginTime) - 1 AS VARCHAR(2)), 2) + 'd ' " +
                    "+ RIGHT('0' + CAST(DATEPART(HH, LogoutTime - LoginTime) AS VARCHAR(2)), 2) + 'h ' " +
                    "+ RIGHT('0' + CAST(DATEPART(MI, LogoutTime - LoginTime) AS VARCHAR(2)), 2) + 'm ' " +
                    "+ RIGHT('0' + CAST(DATEPART(SS, LogoutTime - LoginTime) AS VARCHAR(2)), 2) + 's' Duration " +
                "FROM " +
                "( " +
                    "SELECT Membership.MembershipNo, NameToAppear MembershipName " +
                        ", MIN(CASE WHEN ActivityName = 'Login' THEN ActivityDateTime ELSE NULL END) LoginTime " +
                        ", MAX(CASE WHEN ActivityName = 'Logout' THEN ActivityDateTime ELSE NULL END) LogoutTime " +
                    "FROM MembershipAttendance " +
                        "INNER JOIN Membership ON MembershipAttendance.MembershipNo = Membership.MembershipNo " +
                    "GROUP BY LockerID, Membership.MembershipNo, NameToAppear " +
                ") DT " +
                "WHERE LogoutTime IS NOT NULL " +
                "ORDER BY LogoutTime DESC ";

            DataTable Dt = new DataTable();
            Dt.Load(DataService.GetReader(new QueryCommand(SQL)));

            if (Dt.Rows.Count <= 0)
                return null;
            else
                return Dt.Rows[0];
        }
        /// <summary>
        /// Get the last check out member since the Time Limit
        /// </summary>
        /// <param name="Limit">Only look for information after this Date/Time</param>
        /// <returns>MembershipNo, MembershipName, LoginTime, LogoutTime, Duration</returns>
        public static DataRow FetchLastCheckOutMemberSince(DateTime Limit)
        {
            string SQL =
                "SELECT * " +
                    ", RIGHT('0' + CAST(DATEPART(DD, LogoutTime - LoginTime) - 1 AS VARCHAR(2)), 2) + 'd ' " +
                    "+ RIGHT('0' + CAST(DATEPART(HH, LogoutTime - LoginTime) AS VARCHAR(2)), 2) + 'h ' " +
                    "+ RIGHT('0' + CAST(DATEPART(MI, LogoutTime - LoginTime) AS VARCHAR(2)), 2) + 'm ' " +
                    "+ RIGHT('0' + CAST(DATEPART(SS, LogoutTime - LoginTime) AS VARCHAR(2)), 2) + 's' Duration " +
                "FROM " +
                "( " +
                    "SELECT Membership.MembershipNo, NameToAppear MembershipName " +
                        ", MIN(CASE WHEN ActivityName = 'Login' THEN ActivityDateTime ELSE NULL END) LoginTime " +
                        ", MAX(CASE WHEN ActivityName = 'Logout' THEN ActivityDateTime ELSE NULL END) LogoutTime " +
                    "FROM MembershipAttendance " +
                        "INNER JOIN Membership ON MembershipAttendance.MembershipNo = Membership.MembershipNo " +
                    "GROUP BY LockerID, Membership.MembershipNo, NameToAppear " +
                ") DT " +
                "WHERE LogoutTime IS NOT NULL " +
                    "AND LogoutTime >= '" + Limit.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                "ORDER BY LogoutTime DESC ";

            DataTable Dt = new DataTable();
            Dt.Load(DataService.GetReader(new QueryCommand(SQL)));

            if (Dt.Rows.Count <= 0)
                return null;
            else
                return Dt.Rows[0];
        }

        /// <summary>
        /// Auto Check Out all member that has been inside before the specified date limit. 
        /// Check out time will be on 23:59:59 of the login day
        /// </summary>
        /// <param name="Limit">Only check out all active member before this Date/Time</param>
        public static void AutoCheckoutMemberBefore(DateTime Limit)
        {
            string SQL =
                "INSERT INTO MembershipAttendance " +
                "(MembershipNo, ActivityDateTime, ActivityName, LockerID, PointOfSaleID, UniqueID" +
                    ", CreatedOn, CreatedBy, ModifiedOn, ModifiedBy, Deleted) " +
                "SELECT MembershipNo, DATEADD(SECOND, -1, DATEADD(DAY, 1, CAST(CAST(LoginTime AS DATE) AS DATETIME))) " +
                    ", 'Logout', LockerID, PointOfSaleID, NEWID(), GETDATE(), '" + UserInfo.username + "', GETDATE() "+
                    ", '" + UserInfo.username + "', 0 " +
                "FROM " +
                "( " +
                    "SELECT LockerID, PointOfSaleID, MembershipNo " +
                        ", MIN(CASE WHEN ActivityName = 'Login' THEN ActivityDateTime ELSE NULL END) LoginTime " +
                        ", MAX(CASE WHEN ActivityName = 'Logout' THEN ActivityDateTime ELSE NULL END) LogoutTime " +
                    "FROM MembershipAttendance " +
                    "GROUP BY LockerID, PointOfSaleID, MembershipNo " +
                ") DT " +
                "WHERE LogoutTime IS NULL " +
                    "AND LoginTime < '" + Limit.ToString("yyyy-MM-dd HH:mm:ss") + "' ";

            DataService.ExecuteQuery(new QueryCommand(SQL));
        }
    }        
}
