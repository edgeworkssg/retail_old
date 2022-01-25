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
    [Serializable]
    public partial class MembershipTapController
    {
        public static string URL = "http://fit360.edgeworks.com.sg/Fit360.asmx";
        public static decimal checkMembershipTapAmount(string membershipNo, out string status)
        {
            status = "";
            decimal tap = 0;
            MembershipTap mb = new MembershipTap(MembershipTap.Columns.MembershipNo, membershipNo);
            if (mb != null && !mb.IsNew)
            {
                tap = mb.Amount;
            }
            else
            {
                status = "Membership No does not exist.";
            }
            return tap;
        }

        public static bool adjustTap
            (string membershipNo, decimal amount, string username,
            out decimal remainingTap, out string status)
        {
            status = "";
            remainingTap = 0;
            MembershipTap mb = new MembershipTap(MembershipTap.Columns.MembershipNo, membershipNo);

            mb.MembershipNo = membershipNo;
            mb.Amount += amount;

            mb.Save("SYSTEM");
            remainingTap = mb.Amount;

            //record.....
            MembershipTapsLogController mbr = new MembershipTapsLogController();
            mbr.Insert(mb.MembershipNo, DateTime.Now, username, amount, DateTime.Now, username, DateTime.Now, username, false);

            return true;
        }

        public static QueryCommandCollection adjustTap
            (string membershipNo, decimal amount, string username, out string status)
        {
            QueryCommandCollection cmd = new QueryCommandCollection();
            status = "";

            MembershipTap mb = new MembershipTap(MembershipTap.Columns.MembershipNo, membershipNo);

            mb.MembershipNo = membershipNo;
            mb.Amount += amount;
            cmd.Add(mb.GetSaveCommand(username));

            MembershipTapsLog v = new MembershipTapsLog();
            v.MembershipNo = mb.MembershipNo;
            v.UserName = username;
            v.Amount = amount;
            v.ActivityDate = DateTime.Now;
            v.Deleted = false;
            cmd.Add(v.GetInsertCommand(username));

            return cmd;
        }

        public static DataTable FetchMembershipTapReport
           (bool useStartMembershipDate, bool useEndMembershipDate,
           DateTime StartMembershipDate, DateTime EndMembershipDate,
           bool useStartBirthDate, bool useEndBirthDate,
           DateTime StartBirthDate, DateTime EndBirthDate,
           string StartMembershipNo, string EndMembershipNo, int ViewGroupID,
           string SortColumn, string SortDir)
        {
            ViewMembershipTapCollection myViewMembershipTap = new ViewMembershipTapCollection();

            //Membership Date
            if (useStartMembershipDate & useEndMembershipDate)
                myViewMembershipTap.BetweenAnd
                    (ViewMembershipTap.Columns.ExpiryDate, StartMembershipDate, EndMembershipDate);
            else if (useStartMembershipDate)
                myViewMembershipTap.Where(ViewMembershipTap.Columns.ExpiryDate, SubSonic.Comparison.GreaterOrEquals, StartMembershipDate);
            else if (useEndMembershipDate)
                myViewMembershipTap.Where(ViewMembershipTap.Columns.ExpiryDate, SubSonic.Comparison.LessOrEquals, EndMembershipDate);

            //Birth Date
            if (useStartBirthDate & useEndBirthDate)
                myViewMembershipTap.BetweenAnd
                    (ViewMembershipTap.Columns.DateOfBirth, StartBirthDate, EndBirthDate);
            else if (useStartBirthDate)
                myViewMembershipTap.Where(ViewMembershipTap.Columns.DateOfBirth, SubSonic.Comparison.GreaterOrEquals, StartBirthDate);
            else if (useEndBirthDate)
                myViewMembershipTap.Where(ViewMembershipTap.Columns.DateOfBirth, SubSonic.Comparison.LessOrEquals, EndBirthDate);

            //Membership No
            if (StartMembershipNo != "")
                myViewMembershipTap.Where(ViewMembershipTap.Columns.MembershipNo, SubSonic.Comparison.GreaterOrEquals, StartMembershipNo);

            //Membership No
            if (EndMembershipNo != "")
                myViewMembershipTap.Where(ViewMembershipTap.Columns.MembershipNo, SubSonic.Comparison.LessOrEquals, EndMembershipNo);

            //Group ID
            if (ViewGroupID != 0)
                myViewMembershipTap.Where(ViewMembershipTap.Columns.MembershipGroupId, SubSonic.Comparison.Equals, ViewGroupID);

            SubSonic.TableSchema.TableColumn t = ViewMembershipTap.Schema.GetColumn(SortColumn);

            if (t != null)
            {
                if (SortDir.Trim() == "ASC")
                {
                    myViewMembershipTap.OrderByAsc(SortColumn);
                }
                else if (SortDir.Trim() == "DESC")
                {
                    myViewMembershipTap.OrderByDesc(SortColumn);
                }
            }

            return myViewMembershipTap.Load().ToDataTable();
        }

        public static DataTable FetchMembershipTapReport
          (bool useStartMembershipDate, bool useEndMembershipDate,
          DateTime StartMembershipDate, DateTime EndMembershipDate,
          bool useStartBirthDate, bool useEndBirthDate,
          DateTime StartBirthDate, DateTime EndBirthDate,
          string ViewMembershipTapNo, int ViewGroupID, string NRIC, string gender, string MaritalStatus,
          string FirstName, string LastName, string ChristianName,
          string NameToAppear, string Nationality, string Occupation,
          string StreetName, string BuildingName, string Block, string UnitNo,
          string Email, string Home, string Office,
          string Mobile, string ZipCode, string Country, string City,
          string Remarks, string SortColumn, string SortDir)
        {
            ViewMembershipTapCollection myViewMembershipTap = new ViewMembershipTapCollection();

            //Membership Date
            if (useStartMembershipDate & useEndMembershipDate)
                myViewMembershipTap.BetweenAnd
                    (ViewMembershipTap.Columns.ExpiryDate, StartMembershipDate, EndMembershipDate);
            else if (useStartMembershipDate)
                myViewMembershipTap.Where(ViewMembershipTap.Columns.ExpiryDate, SubSonic.Comparison.GreaterOrEquals, StartMembershipDate);
            else if (useEndMembershipDate)
                myViewMembershipTap.Where(ViewMembershipTap.Columns.ExpiryDate, SubSonic.Comparison.LessOrEquals, EndMembershipDate);

            //Birth Date
            if (useStartBirthDate & useEndBirthDate)
                myViewMembershipTap.BetweenAnd
                    (ViewMembershipTap.Columns.DateOfBirth, StartBirthDate, EndBirthDate);
            else if (useStartBirthDate)
                myViewMembershipTap.Where(ViewMembershipTap.Columns.DateOfBirth, SubSonic.Comparison.GreaterOrEquals, StartBirthDate);
            else if (useEndBirthDate)
                myViewMembershipTap.Where(ViewMembershipTap.Columns.DateOfBirth, SubSonic.Comparison.LessOrEquals, EndBirthDate);

            //Membership No
            if (ViewMembershipTapNo != "")
                myViewMembershipTap.Where(ViewMembershipTap.Columns.MembershipNo, SubSonic.Comparison.Like, ViewMembershipTapNo);

            //Group ID
            if (ViewGroupID != 0)
                myViewMembershipTap.Where(ViewMembershipTap.Columns.MembershipGroupId, SubSonic.Comparison.Equals, ViewGroupID);

            //Gender
            if (gender != "" & gender != "ALL")
                myViewMembershipTap.Where(ViewMembershipTap.Columns.Gender, SubSonic.Comparison.Like, gender);

            if (MaritalStatus != "")
                myViewMembershipTap.Where(ViewMembershipTap.Columns.MaritalStatus, SubSonic.Comparison.Like, MaritalStatus);

            //Names
            if (FirstName != "")
                myViewMembershipTap.Where(ViewMembershipTap.Columns.FirstName, SubSonic.Comparison.Like, "%" + FirstName + "%");

            if (LastName != "")
                myViewMembershipTap.Where(ViewMembershipTap.Columns.LastName, SubSonic.Comparison.Like, LastName);

            if (ChristianName != "")
                myViewMembershipTap.Where(ViewMembershipTap.Columns.ChristianName, SubSonic.Comparison.Like, "%" + ChristianName + "%");

            if (NameToAppear != "")
                myViewMembershipTap.Where(ViewMembershipTap.Columns.NameToAppear, SubSonic.Comparison.Like, "%" + NameToAppear + "%");

            //Nationality
            if (Nationality != "")
                myViewMembershipTap.Where(ViewMembershipTap.Columns.Nationality, SubSonic.Comparison.Like, "%" + Nationality + "%");

            //Job
            if (Occupation != "")
                myViewMembershipTap.Where(ViewMembershipTap.Columns.Occupation, SubSonic.Comparison.Like, "%" + Occupation + "%");

            //Address
            if (StreetName != "")
                myViewMembershipTap.Where(ViewMembershipTap.Columns.StreetName, SubSonic.Comparison.Like, "%" + StreetName + "%");

            if (BuildingName != "")
                myViewMembershipTap.Where(ViewMembershipTap.Columns.BuildingName, SubSonic.Comparison.Like, "%" + BuildingName + "%");

            if (Block != "")
                myViewMembershipTap.Where(ViewMembershipTap.Columns.Block, SubSonic.Comparison.Like, "%" + Block + "%");

            if (UnitNo != "")
                myViewMembershipTap.Where(ViewMembershipTap.Columns.UnitNo, SubSonic.Comparison.Like, "%" + UnitNo + "%");

            if (Email != "")
                myViewMembershipTap.Where(ViewMembershipTap.Columns.Email, SubSonic.Comparison.Like, "%" + Email + "%");

            if (Home != "")
                myViewMembershipTap.Where(ViewMembershipTap.Columns.Home, SubSonic.Comparison.Like, "%" + Home + "%");

            if (Office != "")
                myViewMembershipTap.Where(ViewMembershipTap.Columns.Office, SubSonic.Comparison.Like, "%" + Office + "%");

            if (Mobile != "")
                myViewMembershipTap.Where(ViewMembershipTap.Columns.Mobile, SubSonic.Comparison.Like, "%" + Mobile + "%");

            if (ZipCode != "")
                myViewMembershipTap.Where(ViewMembershipTap.Columns.ZipCode, SubSonic.Comparison.Like, "%" + ZipCode + "%");

            if (Country != "")
                myViewMembershipTap.Where(ViewMembershipTap.Columns.Country, SubSonic.Comparison.Like, "%" + Country + "%");

            if (City != "")
                myViewMembershipTap.Where(ViewMembershipTap.Columns.City, SubSonic.Comparison.Like, "%" + City + "%");


            if (NRIC != "")
                myViewMembershipTap.Where(ViewMembershipTap.Columns.Nric, SubSonic.Comparison.Like, "%" + NRIC + "%");

            /*
            //Remarks
            if (Remarks != "")
                myViewMembershipTap.Where(ViewMembershipTap.Columns.Remarks, SubSonic.Comparison.Like, "%" + Remarks + "%");
            */

            SubSonic.TableSchema.TableColumn t = ViewMembershipTap.Schema.GetColumn(SortColumn);

            if (t != null)
            {
                if (SortDir.Trim() == "ASC")
                {
                    myViewMembershipTap.OrderByAsc(SortColumn);
                }
                else if (SortDir.Trim() == "DESC")
                {
                    myViewMembershipTap.OrderByDesc(SortColumn);
                }

            }

            return myViewMembershipTap.Load().ToDataTable();

        }

        public static DataTable FetchMembershipTapReport
           (string ViewMembershipTapNo, string NRIC, string NameToAppear, string SortColumn, string SortDir)
        {
            ViewMembershipTapCollection myViewMembershipTap = new ViewMembershipTapCollection();

            //Membership No
            if (ViewMembershipTapNo != "")
                myViewMembershipTap.Where(ViewMembershipTap.Columns.MembershipNo, SubSonic.Comparison.Like, "%" + ViewMembershipTapNo + "%");

            if (NameToAppear != "")
                myViewMembershipTap.Where(ViewMembershipTap.Columns.NameToAppear, SubSonic.Comparison.Like, "%" + NameToAppear + "%");

            if (NRIC != "")
                myViewMembershipTap.Where(ViewMembershipTap.Columns.Nric, SubSonic.Comparison.Like, "%" + NRIC + "%");

            myViewMembershipTap.Where(ViewMembershipTap.Columns.Amount, Comparison.GreaterThan, 0);

            SubSonic.TableSchema.TableColumn t = ViewMembershipTap.Schema.GetColumn(SortColumn);

            if (t != null)
            {
                if (SortDir.Trim() == "ASC")
                {
                    myViewMembershipTap.OrderByAsc(SortColumn);
                }
                else if (SortDir.Trim() == "DESC")
                {
                    myViewMembershipTap.OrderByDesc(SortColumn);
                }

            }

            return myViewMembershipTap.Load().ToDataTable();

        }

        public static DataTable FetchMembershipTapLogReport
            (string ViewMembershipTapsLogNo, string NRIC, string NameToAppear,
            string SortColumn, string SortDir)
        {
            ViewMembershipTapsLogCollection myViewMembershipTapsLog = new ViewMembershipTapsLogCollection();

            //Membership No
            if (ViewMembershipTapsLogNo != "")
                myViewMembershipTapsLog.Where(ViewMembershipTapsLog.Columns.MembershipNo, SubSonic.Comparison.Like, "%" + ViewMembershipTapsLogNo + "%");

            if (NameToAppear != "")
                myViewMembershipTapsLog.Where(ViewMembershipTapsLog.Columns.NameToAppear, SubSonic.Comparison.Like, "%" + NameToAppear + "%");

            if (NRIC != "")
                myViewMembershipTapsLog.Where(ViewMembershipTapsLog.Columns.Nric, SubSonic.Comparison.Like, "%" + NRIC + "%");

            SubSonic.TableSchema.TableColumn t = ViewMembershipTapsLog.Schema.GetColumn(SortColumn);

            if (t != null)
            {
                if (SortDir.Trim() == "ASC")
                {
                    myViewMembershipTapsLog.OrderByAsc(SortColumn);
                }
                else if (SortDir.Trim() == "DESC")
                {
                    myViewMembershipTapsLog.OrderByDesc(SortColumn);
                }

            }

            return myViewMembershipTapsLog.Load().ToDataTable();

        }



    }
}
