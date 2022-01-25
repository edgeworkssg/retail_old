using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using PowerPOS.Container;
using PowerPOS;
using SubSonic;
using System.ComponentModel;
using System.Collections;

namespace PowerPOS
{
    public partial class ReportController
    {
        public static DataTable FetchMembershipTransactionReport(
            bool useStartDate, bool useEndDate,
            DateTime StartDate, DateTime EndDate,
            string MembershipNo, int GroupID,
            string NRIC, string FirstName, string LastName, string NameToAppear,
            string ViewTransactionWithMembershipRefNo,
            string ItemName, string CategoryName, int PointOfSaleID,
            string PointOfSaleName, string outletName, string deptID,
            string Remark, string LineInfo,
            string SortColumn, string SortDir)
        {
            return FetchMembershipTransactionReport(useStartDate, useEndDate,
                StartDate, EndDate,
                MembershipNo, GroupID,
                NRIC, FirstName, LastName, NameToAppear,
                ViewTransactionWithMembershipRefNo,
                ItemName, CategoryName, PointOfSaleID,
                PointOfSaleName, outletName, deptID,
                Remark, LineInfo, "",
                SortColumn, SortDir);
        }

        public static DataTable FetchMembershipTransactionReport(
            bool useStartDate, bool useEndDate,
            DateTime StartDate, DateTime EndDate,
            string MembershipNo, int GroupID,
            string NRIC, string FirstName, string LastName, string NameToAppear,
            string ViewTransactionWithMembershipRefNo,
            string ItemName, string CategoryName, int PointOfSaleID,
            string PointOfSaleName, string outletName, string deptID,
            string Remark, string LineInfo, string Staff,
            string SortColumn, string SortDir)
        {

            string SQL = @"SELECT a.OrderRefNo, a.OrderDate, d.OutletName, d.PointOfSaleName,  
                            a.OrderHdrID, a.NettAmount as Amount, e.ItemName, e.ItemNo, e.CategoryName,  
                            b.Amount AS LineAmount, b.Quantity, b.UnitPrice,  
                            b.IsFreeOfCharge, d.PointOfSaleID,  
                            d.DepartmentID, a.CashierID, b.IsPromo, b.PromoAmount, b.Discount AS Expr17,  
                            b.PromoDiscount, c.MembershipNo, c.MembershipGroupId, c.Title,  
                            c.LastName, c.FirstName, c.ChristianName, c.NameToAppear,  
                            c.Gender, c.DateOfBirth, c.Nationality, c.NRIC, c.Occupation,  
                            c.MaritalStatus, c.Email, c.Block, c.BuildingName, c.StreetName,  
                            c.UnitNo, c.City, c.Country, c.ZipCode, c.Mobile,  
                            c.Office, c.Fax, c.Home, c.ExpiryDate, c.Remarks,  
                            c.SubscriptionDate, c.IsChc, c.Ministry, c.IsStudentCard,  
                            c.CreatedOn, c.CreatedBy, c.ModifiedOn, c.ModifiedBy,  
                            c.Deleted, c.UniqueID, c.GroupName, c.Discount,  
                            c.ChineseName, c.StreetName2, c.Address, c.BirthDayMonth,  
                            c.Name, c.IsVitaMix, c.IsWaterFilter, c.IsJuicePlus,  
                            c.IsYoung, a.Remark, b.userfld4 AS LineInfo, 
                            isnull(nullif(b.userfld1,''),s.SalesPersonId) AS SalesPersonId, 
                            g.DisplayName AS SalesPersonName, f.DisplayName AS CashierName, 
                            Ins.CurrentBalance AS BalancePayment, 
                            e.BalanceQuantity as QtyOnHand  
                            FROM 
                            OrderHdr a with (nolock)
						       Inner Join OrderDet b with (nolock) on a.orderhdrid = b.orderhdrid 
                               inner join viewmembership c  with (nolock) on a.membershipno = c.membershipno 
                               inner join PointOfSale d  with (nolock) on a.pointofsaleid = d.pointofsaleid 
                               inner join SalesCommissionRecord s  with (nolock) on a.orderhdrid = s.orderhdrid 
                               inner join item e  with (nolock) on e.itemno = b.itemno AND e.CategoryName != 'SYSTEM'
                               left join UserMst f  with (nolock) on f.UserName = a.CashierID 
                               left join UserMst g  with (nolock) on g.UserName = isnull(nullif(b.userfld1,''),s.SalesPersonId) 
        				       left join Installment Ins  with (nolock) on Ins.Membershipno = a.membershipno and ins.orderhdrid = a.orderhdrid  
                           WHERE   b.IsVoided = 0 AND a.IsVoided = 0 ";


            if (useStartDate)
            {
                SQL += " AND OrderDate >= '" + StartDate.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
            }
            
            if (useEndDate)
            {
                SQL += " AND OrderDate <= '" + EndDate.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
            }

            if (ViewTransactionWithMembershipRefNo != "")
            {
                SQL += " AND OrderRefNo like '" + ViewTransactionWithMembershipRefNo + "' ";
            }

            if (MembershipNo != "")
            {
                SQL += " AND c.MembershipNo like '" + MembershipNo + "' ";
            }
            if (GroupID != 0)
            {
                SQL += " AND MembershipGroupId = " + GroupID.ToString() + " ";
            }
            if (FirstName != "")
            {
                SQL += " AND ISNULL(FirstName,'') like '" + FirstName + "'";
            }
            if (LastName != "")
            {
                SQL += " AND ISNULL(LastName,'') like '" + LastName + "'";
            }
            if (NameToAppear != "")
            {
                SQL += " AND ISNULL(NameToAppear,'') like '" + NameToAppear + "'";
            }
            if (NRIC != "")
            {
                SQL += " AND ISNULL(NRIC,'') like '" + NRIC + "'";
            }

            if (ItemName != "")
            {
                SQL += " AND ItemName like '" + ItemName + "'";
            }
            if (CategoryName != "")
            {
                SQL += " AND CategoryName like '" + CategoryName + "'";
            }
            if (outletName != "")
            {
                SQL += " AND outletName like '" + outletName + "'";
            }

            if (deptID != "0")
            {
                SQL += " AND DepartmentID like '" + deptID + "'";
            }

            if (PointOfSaleID > 0) //<0 for all
            {
                SQL += " AND PointOfSaleID = " + PointOfSaleID + "";
            }
            else
            {
                if (PointOfSaleName != "")
                {
                    SQL += " AND PointOfSaleName like '%" + PointOfSaleName + "%'";
                }
            }

            if (Remark != "")
            {
                SQL += " AND ISNULL(a.Remark,'') like '" + Remark + "'";
            }

            if (LineInfo != "")
            {
                SQL += " AND ISNULL(b.userfld4,'') like '" + LineInfo + "'";
            }

            if (!string.IsNullOrEmpty(Staff))
            {
                SQL += " AND (a.CashierID LIKE '%" + Staff + "%' OR f.UserName LIKE '%" + Staff + "%' OR isnull(nullif(b.userfld1,''),s.SalesPersonId) LIKE '%" + Staff + "%' OR g.UserName LIKE '%" + Staff + "%') ";
            }

            if (SortColumn != "" && SortDir != "")
            {
                SQL += " Order By " + SortColumn + " " + SortDir;
            }


            QueryCommand cmd = new QueryCommand(SQL, "PowerPOS");
            Logger.writeLog(SQL);
            return DataService.GetDataSet(cmd).Tables[0];
        }


       public static DataTable FetchMembershipProjectTransactionDetailedReport(
       bool useStartDate, bool useEndDate,
       DateTime StartDate, DateTime EndDate,
       string MembershipNo, int GroupID,
       string ProjectName, string FirstName, string LastName, string NameToAppear,
       string ViewTransactionWithMembershipRefNo,
       string ItemName, string CategoryName, int PointOfSaleID,
       string PointOfSaleName, string outletName, string deptID,
       string SortColumn, string SortDir)
        {

            string SQL = "SELECT a.userfld1 as ProjectName, a.OrderRefNo, a.OrderDate, d.OutletName, d.PointOfSaleName,  " +
                           " a.OrderHdrID, a.NettAmount as Amount, e.ItemName, e.ItemNo, e.CategoryName,  " +
                           " b.Amount AS LineAmount, b.Quantity, b.UnitPrice,  " +
                           " b.IsFreeOfCharge, d.PointOfSaleID,  " +
                           " d.DepartmentID, a.CashierID, b.IsPromo, b.PromoAmount, b.Discount AS Expr17,  " +
                           " b.PromoDiscount, c.MembershipNo, c.MembershipGroupId, c.Title,  " +
                           " c.LastName, c.FirstName, c.ChristianName, c.NameToAppear,  " +
                           " c.Gender, c.DateOfBirth, c.Nationality, c.NRIC, c.Occupation,  " +
                           " c.MaritalStatus, c.Email, c.Block, c.BuildingName, c.StreetName,  " +
                           " c.UnitNo, c.City, c.Country, c.ZipCode, c.Mobile,  " +
                           " c.Office, c.Fax, c.Home, c.ExpiryDate, c.Remarks,  " +
                           " c.SubscriptionDate, c.IsChc, c.Ministry, c.IsStudentCard,  " +
                           " c.CreatedOn, c.CreatedBy, c.ModifiedOn, c.ModifiedBy,  " +
                           " c.Deleted, c.UniqueID, c.GroupName, c.Discount,  " +
                           " c.ChineseName, c.StreetName2, c.Address, c.BirthDayMonth,  " +
                           " c.Name, c.IsVitaMix, c.IsWaterFilter, c.IsJuicePlus,  " +
                           " c.IsYoung, a.remark " +
                           " FROM " +
                           "OrderHdr a Inner Join OrderDet b on a.orderhdrid = b.orderhdrid " +
                           "inner join viewmembership c on a.membershipno = c.membershipno " +
                           "inner join PointOfSale d on a.pointofsaleid = d.pointofsaleid " +
                           "inner join item e on e.itemno = b.itemno " +
                           "WHERE   b.IsVoided = 0 AND a.IsVoided = 0 " +
                           "AND ISNULL(a.Userfld1,'') <> ''";



            if (useStartDate)
            {
                SQL += " AND OrderDate >= '" + StartDate.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
            }
            else if (useEndDate)
            {
                SQL += " AND OrderDate <= '" + EndDate.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
            }

            if (ViewTransactionWithMembershipRefNo != "")
            {
                SQL += " AND OrderRefNo like '" + ViewTransactionWithMembershipRefNo + "' ";
            }

            if (MembershipNo != "")
            {
                SQL += " AND c.MembershipNo like '%" + MembershipNo + "'% ";
            }
            if (GroupID != 0)
            {
                SQL += " AND MembershipGroupId = '%" + GroupID.ToString() + "%' ";
            }
            if (FirstName != "")
            {
                SQL += " AND FirstName like '%" + FirstName + "%'";
            }
            if (LastName != "")
            {
                SQL += " AND LastName like '%" + LastName + "%'";
            }
            if (NameToAppear != "")
            {
                SQL += " AND NameToAppear like '%" + NameToAppear + "%'";
            }
            if (ProjectName != "")
            {
                SQL += " AND a.Userfld1 like '%" + ProjectName + "%'";
            }

            if (ItemName != "")
            {
                SQL += " AND ItemName like '%" + ItemName + "'";
            }
            if (CategoryName != "")
            {
                SQL += " AND CategoryName like '" + CategoryName + "'";
            }
            if (outletName != "")
            {
                SQL += " AND outletName like '" + outletName + "'";
            }

            if (deptID != "0")
            {
                SQL += " AND DepartmentID like '" + deptID + "'";
            }

            if (PointOfSaleID > 0) //<0 for all
            {
                SQL += " AND PointOfSaleID = " + PointOfSaleID + "";
            }
            else
            {
                if (PointOfSaleName != "")
                {
                    SQL += " AND PointOfSaleName like '%" + PointOfSaleName + "%'";
                }
            }
            if (SortColumn != "" && SortDir != "")
            {
                SQL += " Order By " + SortColumn + " " + SortDir;
            }

            QueryCommand cmd = new QueryCommand(SQL, "PowerPOS");

            return DataService.GetDataSet(cmd).Tables[0];
        }

        public static DataTable FetchMembershipProjectTransactionReport(
            bool useStartDate, bool useEndDate,
            DateTime StartDate, DateTime EndDate,
            string MembershipNo, int GroupID,
            string ProjectName, string FirstName, string LastName, string NameToAppear,
            string ViewTransactionWithMembershipRefNo, int PointOfSaleID,
            string PointOfSaleName, string outletName, string deptID,
            string SortColumn, string SortDir)
            {

               string SQL = "SELECT a.userfld1 as ProjectName, a.OrderRefNo, a.OrderDate, d.OutletName, d.PointOfSaleName,  " +
                              " a.OrderHdrID, a.NettAmount as NettAmount, a.GrossAmount as GrossAmount, " +
                              " d.PointOfSaleID,  " +
                              " d.DepartmentID, a.CashierID,  " +
                              " c.MembershipNo, c.MembershipGroupId, c.Title,  " +
                              " c.LastName, c.FirstName, c.ChristianName, c.NameToAppear,  " +
                              " c.Gender, c.DateOfBirth, c.Nationality, c.NRIC, c.Occupation,  " +
                              " c.MaritalStatus, c.Email, c.Block, c.BuildingName, c.StreetName,  " +
                              " c.UnitNo, c.City, c.Country, c.ZipCode, c.Mobile,  " +
                              " c.Office, c.Fax, c.Home, c.ExpiryDate, c.Remarks,  " +
                              " c.SubscriptionDate, c.IsChc, c.Ministry, c.IsStudentCard,  " +
                              " c.CreatedOn, c.CreatedBy, c.ModifiedOn, c.ModifiedBy,  " +
                              " c.Deleted, c.UniqueID, c.GroupName, c.Discount,  " +
                              " c.ChineseName, c.StreetName2, c.Address, c.BirthDayMonth,  " +
                              " c.Name, c.IsVitaMix, c.IsWaterFilter, c.IsJuicePlus,  " +
                              " c.IsYoung, a.remark " +
                              " FROM OrderHdr a " +
                              "inner join viewmembership c on a.membershipno = c.membershipno " +
                              "inner join PointOfSale d on a.pointofsaleid = d.pointofsaleid " +
                              "WHERE a.IsVoided = 0 " +
                              "AND ISNULL(a.Userfld1,'') <> ''";

               if (useStartDate)
               {
                   SQL += " AND OrderDate >= '" + StartDate.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
               }
               else if (useEndDate)
               {
                   SQL += " AND OrderDate <= '" + EndDate.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
               }

               if (ViewTransactionWithMembershipRefNo != "")
               {
                   SQL += " AND OrderRefNo like '" + ViewTransactionWithMembershipRefNo + "' ";
               }

               if (MembershipNo != "")
               {
                   SQL += " AND c.MembershipNo like '%" + MembershipNo + "'% ";
               }
               if (GroupID != 0)
               {
                   SQL += " AND MembershipGroupId = '%" + GroupID.ToString() + "%' ";
               }
               if (FirstName != "")
               {
                   SQL += " AND FirstName like '%" + FirstName + "%'";
               }
               if (LastName != "")
               {
                   SQL += " AND LastName like '%" + LastName + "%'";
               }
               if (NameToAppear != "")
               {
                   SQL += " AND NameToAppear like N'%" + NameToAppear + "%'";
               }
               if (ProjectName != "")
               {
                   SQL += " AND a.Userfld1 like '%" + ProjectName + "%'";
               }

               if (outletName != "")
               {
                   SQL += " AND outletName like '" + outletName + "'";
               }

               if (deptID != "0")
               {
                   SQL += " AND DepartmentID like '" + deptID + "'";
               }

               if (PointOfSaleID > 0) //<0 for all
               {
                   SQL += " AND PointOfSaleID = " + PointOfSaleID + "";
               }
               else
               {
                   if (PointOfSaleName != "")
                   {
                       SQL += " AND PointOfSaleName like '%" + PointOfSaleName + "%'";
                   }
               }
               if (SortColumn != "" && SortDir != "")
               {
                   SQL += " Order By " + SortColumn + " " + SortDir;
               }

               QueryCommand cmd = new QueryCommand(SQL, "PowerPOS");

               return DataService.GetDataSet(cmd).Tables[0];
           }
        /*
        public static DataTable FetchMembershipProductSalesReport(
            bool useStartSalesDate, bool useEndSalesDate, 
            DateTime startSalesDate, DateTime endSalesDate,
            ArrayList ItemList,
            bool useStartMembershipDate, bool useEndMembershipDate,
            DateTime StartMembershipDate, DateTime EndMembershipDate,
            bool useStartBirthDate, bool useEndBirthDate,
            DateTime StartBirthDate, DateTime EndBirthDate,
            bool useBirthDayMonth, int BirthdayMonth,
            string ViewMembershipNo, int ViewGroupID, string NRIC, string gender, string name,
            string address, string SortColumn, string SortDir)
        {

            if (ItemList.Count <= 0) return null;
            
            //Construct Item List
            string sqlItemList = "";
            for (int i = 0; i < ItemList.Count - 1; i++)
            {
                sqlItemList += "'" + ItemList[i] + "',";
            }
            sqlItemList += "'" + ItemList[ItemList.Count - 1] + "'";
                        

            //Construct Select List
            //Membership Fields, 
            string selectList = "MembershipNo, GroupName, NameToAppear, Mobile,home, streetname, streetname2, zipcode,NRIC,IsVitaMix,IsWaterFilter,IsYoung,IsJuicePlus,email,ChineseName,FirstName,LastName,ChristianName,DateOfBirth,Occupation,remarks,Deleted";
            string[] captionList = {"Card No","Card Type", "Name","Mobile", "Home", "Address1", "Address2", "Postal Code", "NRIC", "VitaMix Cust", "Water Filter Cust", "Young Cust", "Juice Plus Cust", "email", "Chinese Name", "First Name", "Last Name", "Christian Name", "Date Of Birth", "Occupation", "Remarks", "Deleted"};
            //select
            string filterExpression = "";

            //Sales Date
            if (useStartSalesDate)
            {
                filterExpression += " " + OrderDet.Columns.OrderDetDate + " >= '" + startSalesDate.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            }
            if (useEndSalesDate)
            {
                filterExpression += " " + OrderDet.Columns.OrderDetDate + " <= '" + endSalesDate.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            }

            //Membership Date
            if (useStartMembershipDate)
            {
                filterExpression += " " + ViewMembership.Columns.SubscriptionDate + " >= '" + StartMembershipDate.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            }
            if (useEndMembershipDate)
            {
                filterExpression += " " + ViewMembership.Columns.SubscriptionDate + " <= '" + EndMembershipDate.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            }

            //Birth Date
            if (useStartBirthDate)
                filterExpression += " " + ViewMembership.Columns.DateOfBirth + " >= '" + StartBirthDate.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            else if (useEndBirthDate)
                filterExpression += " " + ViewMembership.Columns.DateOfBirth + " <= '" + EndBirthDate.ToString("yyyy-MM-dd HH:mm:ss") + "'";

            if (useBirthDayMonth)
            {
                filterExpression += " " + ViewMembership.Columns.BirthDayMonth + " = " + BirthdayMonth;
            }

            //Membership No
            if (ViewMembershipNo != "")
                filterExpression += " " + ViewMembership.Columns.MembershipNo + " LIKE '" + ViewMembershipNo + "'";

            //Group ID
            if (ViewGroupID != 0)
                filterExpression += " " + ViewMembership.Columns.MembershipGroupId + " = " + ViewGroupID;

            //Gender
            if (gender != "" & gender != "ALL")
                filterExpression += " " + ViewMembership.Columns.Gender + " = '" + gender + "'";

            //Names
            if (name != "")
            {
                filterExpression += " " + ViewMembership.Columns.Name + " LIKE '%" + name + "%'";
            }

            //Address
            if (address != "")
            {
                filterExpression += " " + ViewMembership.Columns.Address + " LIKE '%" + address + "%'";
            }

            if (NRIC != "")
                filterExpression += " " + ViewMembership.Columns.Nric + " LIKE '%" + NRIC + "%'";

            if (filterExpression.Trim() != "")
            {
                filterExpression = " AND " + filterExpression;
            }
            //Execute
            DataTable dt = SPs.FetchMembershipProductSales(sqlItemList, selectList, filterExpression).GetDataSet().Tables[0];

            //set caption
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                if (i < captionList.Length)
                {
                    dt.Columns[i].Caption = captionList[i];
                }
            }

            const int startDataColumn = 8;
            //Reform the table into the correct arraylist format...
            for (int i = 0; i < ItemList.Count; i++)
            {
                DataColumn tmp = new DataColumn(ItemList[i].ToString(), System.Type.GetType("System.Int32"));
                dt.Columns.Add(tmp);
                //tmp.ExtendedProperties.Add("money", true);
                tmp.DefaultValue = 0.0M;
                for (int k = 0; k < dt.Rows.Count; k++)
                {
                    dt.Rows[k][tmp] = 0.0;
                }                
                tmp.SetOrdinal(startDataColumn + i);
            }
            
            //copy data....
            int p = dt.Rows.Count-1;
            while (p > 0)
            {
                //compare with Row before....
                if (dt.Rows[p]["MembershipNo"].ToString() == dt.Rows[p - 1]["MembershipNo"].ToString())
                {
                    dt.Rows[p][dt.Rows[p - 1]["ItemNo"].ToString()] = dt.Rows[p - 1]["Amount"];
                    dt.Rows.RemoveAt(p - 1);
                    p--;
                }
                else
                {
                    //move up
                    dt.Rows[p][dt.Rows[p]["ItemNo"].ToString()] = dt.Rows[p]["Amount"];
                    p--;
                }
            }            
            dt.Columns.Remove("ItemNo");
            dt.Columns.Remove("Amount");

            
            return dt;
        }
        */
        public static DataTable FetchMembershipProductSalesReport(
            bool useStartSalesDate, bool useEndSalesDate,
            DateTime startSalesDate, DateTime endSalesDate,
            ArrayList ItemList,
            bool useStartMembershipDate, bool useEndMembershipDate,
            DateTime StartMembershipDate, DateTime EndMembershipDate,
            bool useStartBirthDate, bool useEndBirthDate,
            DateTime StartBirthDate, DateTime EndBirthDate,
            bool useBirthDayMonth, int BirthdayMonth,
            string ViewMembershipNo, int ViewGroupID, string NRIC, string gender, string name,
            string address, string SortColumn, string SortDir)
        {

            if (ItemList.Count <= 0) return null;

            //Construct Item List
            string sqlItemList = "";
            for (int i = 0; i < ItemList.Count - 1; i++)
            {
                sqlItemList += "'" + ItemList[i] + "',";
            }
            sqlItemList += "'" + ItemList[ItemList.Count - 1] + "'";


            //Construct Select List
            //Membership Fields, 
            //string selectList = "MembershipNo, GroupName, NameToAppear, Mobile,home, streetname, streetname2, zipcode,NRIC,IsVitaMix,IsWaterFilter,IsYoung,IsJuicePlus,email,ChineseName,FirstName,LastName,ChristianName,DateOfBirth,Occupation,remarks,Deleted";
            //string[] captionList = { "Card No", "Card Type", "Name", "Mobile", "Home", "Address1", "Address2", "Postal Code", "NRIC", "VitaMix Cust", "Water Filter Cust", "Young Cust", "Juice Plus Cust", "email", "Chinese Name", "First Name", "Last Name", "Christian Name", "Date Of Birth", "Occupation", "Remarks", "Deleted" };
            string selectList = "MembershipNo, GroupName, NameToAppear, Mobile,home, streetname, streetname2, zipcode,NRIC,email,ChineseName,FirstName,LastName,ChristianName,DateOfBirth,Occupation,remarks,Deleted";
            string[] captionList = { "Card No", "Card Type", "Name", "Mobile", "Home", "Address1", "Address2", "Postal Code", "NRIC", "email", "Chinese Name", "First Name", "Last Name", "Christian Name", "Date Of Birth", "Occupation", "Remarks", "Deleted" };
            //select
            string filterExpression = "";
            ArrayList filter = new ArrayList();
            //Sales Date
            if (useStartSalesDate)
            {
                filter.Add(" " + OrderHdr.Columns.OrderDate + " >= '" + startSalesDate.ToString("yyyy-MM-dd HH:mm:ss") + "'");
            }
            if (useEndSalesDate)
            {
                filter.Add(" " + OrderHdr.Columns.OrderDate + " <= '" + endSalesDate.ToString("yyyy-MM-dd HH:mm:ss") + "'");
            }

            //Membership Date
            if (useStartMembershipDate)
            {
                filter.Add(" " + ViewMembership.Columns.SubscriptionDate + " >= '" + StartMembershipDate.ToString("yyyy-MM-dd HH:mm:ss") + "'");
            }
            if (useEndMembershipDate)
            {
                filter.Add(" " + ViewMembership.Columns.SubscriptionDate + " <= '" + EndMembershipDate.ToString("yyyy-MM-dd HH:mm:ss") + "'");
            }

            //Birth Date
            if (useStartBirthDate)
            {
                filter.Add(" " + ViewMembership.Columns.DateOfBirth + " >= '" + StartBirthDate.ToString("yyyy-MM-dd HH:mm:ss") + "'");
            }
            else if (useEndBirthDate)
            {
                filter.Add(" " + ViewMembership.Columns.DateOfBirth + " <= '" + EndBirthDate.ToString("yyyy-MM-dd HH:mm:ss") + "'");
            }

            if (useBirthDayMonth)
            {
                filter.Add(" " + ViewMembership.Columns.BirthDayMonth + " = " + BirthdayMonth);
            }

            //Membership No
            if (ViewMembershipNo != "")
            {
                filter.Add(" " + ViewMembership.Columns.MembershipNo + " LIKE '" + ViewMembershipNo + "'");
            }

            //Group ID
            if (ViewGroupID != 0)
            {
                filter.Add(" " + ViewMembership.Columns.MembershipGroupId + " = " + ViewGroupID);
            }

            //Gender
            if (gender != "" & gender != "ALL")
            {
                filter.Add(" " + ViewMembership.Columns.Gender + " = '" + gender + "'");
            }

            //Names
            if (name != "")
            {
                filter.Add(" " + ViewMembership.Columns.Name + " LIKE N'%" + name + "%'");
            }

            //Address
            if (address != "")
            {
                filter.Add(" " + ViewMembership.Columns.Address + " LIKE '%" + address + "%'");
            }

            if (NRIC != "")
            {
                filter.Add(" " + ViewMembership.Columns.Nric + " LIKE '%" + NRIC + "%'");
            }

            if (filter.Count > 0)
            {
                for (int i = 0; i < filter.Count; i++)
                {
                    filterExpression += "AND" + filter[i].ToString();
                }
            }
            //Execute
            DataTable dt = SPs.FetchMembershipProductSales(sqlItemList, selectList, filterExpression).GetDataSet().Tables[0];

            //set caption
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                if (i < captionList.Length)
                {
                    dt.Columns[i].Caption = captionList[i];
                }
            }

            const int startDataColumn = 8;
            //Reform the table into the correct arraylist format...
            //Set all values to be zero...
            for (int i = 0; i < ItemList.Count; i++)
            {
                DataColumn tmp = new DataColumn(ItemList[i].ToString(), System.Type.GetType("System.Decimal"));
                dt.Columns.Add(tmp);
                for (int k = 0; k < dt.Rows.Count; k++)
                {
                    if (dt.Rows[k]["ItemNo"].ToString() == ItemList[i].ToString())
                    {
                        dt.Rows[k][tmp] = (Decimal)dt.Rows[k]["Total"];
                    }
                    else
                    {
                        dt.Rows[k][tmp] = 0; //initial value....
                    }
                }
                tmp.SetOrdinal(startDataColumn + i);
            }

            //copy data....
            int p = dt.Rows.Count - 1;
            while (p > 0)
            {
                //compare with Row before if the same....
                if (dt.Rows[p]["MembershipNo"].ToString() == dt.Rows[p - 1]["MembershipNo"].ToString())
                {
                    for (int t = startDataColumn; t < startDataColumn + ItemList.Count; t++)
                    {
                        //copy all the value to the top...
                        dt.Rows[p - 1][t] =
                            (int)dt.Rows[p - 1][t] +
                            (int)dt.Rows[p][t];
                    }
                    dt.Rows.RemoveAt(p);
                }
                p--;
            }
            dt.Columns.Remove("ItemNo");
            dt.Columns.Remove("Total");


            return dt;
        }

        public static DataTable FetchMembershipReport
                    (bool useStartExpiryDate, bool useEndExpiryDate,
                    DateTime startExpiryDate, DateTime endExpiryDate,
                    bool useStartBirthDate, bool useEndBirthDate,
                    DateTime StartBirthDate, DateTime EndBirthDate,
                    bool useBirthDayMonth, int BirthdayMonth,
                    string ViewMembershipNoFrom, string ViewMembershipNoTo,
                    int ViewGroupID, string NRIC, string gender, string name,
                    string address, string mobileNo, string homeNo, string stylist, string email, string SortColumn, string SortDir)
        {
            ViewMembershipCollection myViewMembership = new ViewMembershipCollection();

            //Membership expire Date
            if (useStartExpiryDate)
            {
                myViewMembership.Where(ViewMembership.Columns.SubscriptionDate, SubSonic.Comparison.GreaterOrEquals, startExpiryDate);
            }
            if (useEndExpiryDate)
            {
                myViewMembership.Where(ViewMembership.Columns.SubscriptionDate, SubSonic.Comparison.LessOrEquals, endExpiryDate);
            }

            //Birth Date
            if (useStartBirthDate)
            {
                myViewMembership.Where(ViewMembership.Columns.DateOfBirth, SubSonic.Comparison.GreaterOrEquals, StartBirthDate);
            }
            if (useEndBirthDate)
            {
                myViewMembership.Where(ViewMembership.Columns.DateOfBirth, SubSonic.Comparison.LessOrEquals, EndBirthDate);
            }

            if (useBirthDayMonth)
            {
                myViewMembership.Where(ViewMembership.Columns.BirthDayMonth, BirthdayMonth);
            }

            if (stylist != "")
            {
                myViewMembership.Where(ViewMembership.Columns.SalesPersonID, stylist);
            }
            //Membership No
            ViewMembershipNoFrom = ViewMembershipNoFrom.Trim();
            if (ViewMembershipNoFrom != "" && ViewMembershipNoTo != "")
            {
                myViewMembership.Where(ViewMembership.Columns.MembershipNo, SubSonic.Comparison.GreaterOrEquals, ViewMembershipNoFrom);
                myViewMembership.Where(ViewMembership.Columns.MembershipNo, SubSonic.Comparison.LessOrEquals, ViewMembershipNoTo);
            }
            else if (ViewMembershipNoFrom != "")
            {
                myViewMembership.Where(ViewMembership.Columns.MembershipNo, SubSonic.Comparison.Like, "%" + ViewMembershipNoFrom + "%");
            }
            else if (ViewMembershipNoTo != "")
            {
                myViewMembership.Where(ViewMembership.Columns.MembershipNo, SubSonic.Comparison.Like, "%" + ViewMembershipNoFrom + "%");
            }


            //Group ID
            if (ViewGroupID != 0)
                myViewMembership.Where(ViewMembership.Columns.MembershipGroupId, SubSonic.Comparison.Equals, ViewGroupID);

            //Gender
            if (gender != "" & gender != "ALL")
                myViewMembership.Where(ViewMembership.Columns.Gender, gender);

            //Names
            name = name.Trim();
            if (name != "")
            {
                myViewMembership.Where(ViewMembership.Columns.Name, SubSonic.Comparison.Like, "%" + name + "%");
            }

            //Address
            address = address.Trim();
            if (address != "")
            {
                myViewMembership.Where(ViewMembership.Columns.Address, SubSonic.Comparison.Like, "%" + address + "%");
            }
            NRIC = NRIC.Trim();
            if (NRIC != "")
                myViewMembership.Where(ViewMembership.Columns.Nric, SubSonic.Comparison.Like, "%" + NRIC + "%");

            mobileNo = mobileNo.Trim();
            if (mobileNo != "")
            {
                myViewMembership.Where(ViewMembership.Columns.Mobile, SubSonic.Comparison.Like, "%" + mobileNo + "%");
            }
            homeNo = homeNo.Trim();
            if (homeNo != "")
            {
                myViewMembership.Where(ViewMembership.Columns.Home, SubSonic.Comparison.Like, "%" + homeNo + "%");
            }
            email = email.Trim();
            if (email != "")
            {
                myViewMembership.Where(ViewMembership.Columns.Email, SubSonic.Comparison.Like, "%" + email + "%");
            }
            SubSonic.TableSchema.TableColumn t = ViewMembership.Schema.GetColumn(SortColumn);

            if (t != null)
            {
                if (SortDir.Trim() == "ASC")
                {
                    myViewMembership.OrderByAsc(SortColumn);
                }
                else if (SortDir.Trim() == "DESC")
                {
                    myViewMembership.OrderByDesc(SortColumn);
                }
            }
            return myViewMembership.Load().ToDataTable();
        }

        public static DataTable FetchMembershipReport
            (bool useStartExpiryDate, bool useEndExpiryDate,
            DateTime startExpiryDate, DateTime endExpiryDate,
            bool useStartSubsDate, bool useEndSubsDate,
            DateTime startSubsDate, DateTime endSubsDate,
            bool useStartBirthDate, bool useEndBirthDate,
            DateTime StartBirthDate, DateTime EndBirthDate,
            bool useBirthDayMonth, int BirthdayMonth,
            string ViewMembershipNoFrom, string ViewMembershipNoTo,
            int ViewGroupID, string NRIC, string gender, string name,
            string address, string mobileNo, string homeNo, string stylist, string email, string SortColumn, string SortDir)
        {
            ViewMembershipCollection myViewMembership = new ViewMembershipCollection();

            //Membership expire Date
            if (useStartExpiryDate)
            {
                myViewMembership.Where(ViewMembership.Columns.ExpiryDate, SubSonic.Comparison.GreaterOrEquals, startExpiryDate);
            }
            if (useEndExpiryDate)
            {
                myViewMembership.Where(ViewMembership.Columns.ExpiryDate, SubSonic.Comparison.LessOrEquals, endExpiryDate);
            }

            //Membership subs Date
            if (useStartSubsDate)
            {
                myViewMembership.Where(ViewMembership.Columns.SubscriptionDate, SubSonic.Comparison.GreaterOrEquals, startSubsDate);
            }
            if (useEndSubsDate)
            {
                myViewMembership.Where(ViewMembership.Columns.SubscriptionDate, SubSonic.Comparison.LessOrEquals, endSubsDate);
            }

            //Birth Date
            if (useStartBirthDate)
            {
                myViewMembership.Where(ViewMembership.Columns.DateOfBirth, SubSonic.Comparison.GreaterOrEquals, StartBirthDate);
            }
            if (useEndBirthDate)
            {
                myViewMembership.Where(ViewMembership.Columns.DateOfBirth, SubSonic.Comparison.LessOrEquals, EndBirthDate);
            }

            if (useBirthDayMonth)
            {
                myViewMembership.Where(ViewMembership.Columns.BirthDayMonth, BirthdayMonth);
            }

            if (stylist != "")
            {
                myViewMembership.Where(ViewMembership.Columns.SalesPersonID, stylist);
            }
            //Membership No
            ViewMembershipNoFrom = ViewMembershipNoFrom.Trim();
            if (ViewMembershipNoFrom != "" && ViewMembershipNoTo != "")
            {
                myViewMembership.Where(ViewMembership.Columns.MembershipNo, SubSonic.Comparison.GreaterOrEquals, ViewMembershipNoFrom);
                myViewMembership.Where(ViewMembership.Columns.MembershipNo, SubSonic.Comparison.LessOrEquals, ViewMembershipNoTo);
            }
            else if (ViewMembershipNoFrom != "")
            {
                myViewMembership.Where(ViewMembership.Columns.MembershipNo, SubSonic.Comparison.Like, "%" + ViewMembershipNoFrom + "%");
            }
            else if (ViewMembershipNoTo != "")
            {
                myViewMembership.Where(ViewMembership.Columns.MembershipNo, SubSonic.Comparison.Like, "%" + ViewMembershipNoFrom + "%");
            }


            //Group ID
            if (ViewGroupID != 0)
                myViewMembership.Where(ViewMembership.Columns.MembershipGroupId, SubSonic.Comparison.Equals, ViewGroupID);

            //Gender
            if (gender != "" & gender != "ALL")
                myViewMembership.Where(ViewMembership.Columns.Gender, gender);

            //Names
            name = name.Trim();
            if (name != "")
            {
                myViewMembership.Where(ViewMembership.Columns.Name, SubSonic.Comparison.Like, "%" + name + "%");
            }

            //Address
            address = address.Trim();
            if (address != "")
            {
                myViewMembership.Where(ViewMembership.Columns.Address, SubSonic.Comparison.Like, "%" + address + "%");
            }
            NRIC = NRIC.Trim();
            if (NRIC != "")
                myViewMembership.Where(ViewMembership.Columns.Nric, SubSonic.Comparison.Like, "%" + NRIC + "%");

            mobileNo = mobileNo.Trim();
            if (mobileNo != "")
            {
                myViewMembership.Where(ViewMembership.Columns.Mobile, SubSonic.Comparison.Like, "%" + mobileNo + "%");
            }
            homeNo = homeNo.Trim();
            if (homeNo != "")
            {
                myViewMembership.Where(ViewMembership.Columns.Home, SubSonic.Comparison.Like, "%" + homeNo + "%");
            }
            email = email.Trim();
            if (email != "")
            {
                myViewMembership.Where(ViewMembership.Columns.Email, SubSonic.Comparison.Like, "%" + email + "%");
            }
            SubSonic.TableSchema.TableColumn t = ViewMembership.Schema.GetColumn(SortColumn);

            if (t != null)
            {
                if (SortDir.Trim() == "ASC")
                {
                    myViewMembership.OrderByAsc(SortColumn);
                }
                else if (SortDir.Trim() == "DESC")
                {
                    myViewMembership.OrderByDesc(SortColumn);
                }
            }
            return myViewMembership.Load().ToDataTable();
        }
        /*
        public static DataTable FetchMembershipReport
            (bool useStartMembershipDate, bool useEndMembershipDate,
            DateTime StartMembershipDate, DateTime EndMembershipDate,
            bool useStartBirthDate, bool useEndBirthDate,
            DateTime StartBirthDate, DateTime EndBirthDate,
            bool useBirthDayMonth, int BirthdayMonth,
            string ViewMembershipNo, int ViewGroupID, string NRIC, string gender, string MaritalStatus,
            string FirstName, string LastName, string ChristianName, string ChineseName,
            string NameToAppear, string Nationality, string Occupation,
            string StreetName, string BuildingName, string Block, string UnitNo,
            string Email, string Home, string Office,
            string Mobile, string ZipCode, string Country, string City, 
            string SkinType, string Remarks,
            string SortColumn, string SortDir)
        {
            ViewMembershipCollection myViewMembership = new ViewMembershipCollection();

            //Membership Date
            if (useStartMembershipDate & useEndMembershipDate)
                myViewMembership.BetweenAnd
                    (ViewMembership.Columns.ExpiryDate, StartMembershipDate, EndMembershipDate);            
            else if (useStartMembershipDate)            
                myViewMembership.Where(ViewMembership.Columns.ExpiryDate, SubSonic.Comparison.GreaterOrEquals, StartMembershipDate);            
            else if (useEndMembershipDate)            
                myViewMembership.Where(ViewMembership.Columns.ExpiryDate, SubSonic.Comparison.LessOrEquals, EndMembershipDate);
            
            //Birth Date
            if (useStartBirthDate & useEndBirthDate)            
                myViewMembership.BetweenAnd
                    (ViewMembership.Columns.DateOfBirth, StartBirthDate, EndBirthDate);            
            else if (useStartBirthDate)            
                myViewMembership.Where(ViewMembership.Columns.DateOfBirth, SubSonic.Comparison.GreaterOrEquals, StartBirthDate);            
            else if (useEndBirthDate)            
                myViewMembership.Where(ViewMembership.Columns.DateOfBirth, SubSonic.Comparison.LessOrEquals, EndBirthDate);

            if (useBirthDayMonth)
            {
                myViewMembership.Where(ViewMembership.Columns.BirthDayMonth, BirthdayMonth);
            }

            //Membership No
            if (ViewMembershipNo != "")            
                myViewMembership.Where(ViewMembership.Columns.MembershipNo, SubSonic.Comparison.Like, ViewMembershipNo);

            //Group ID
            if (ViewGroupID != 0)
                myViewMembership.Where(ViewMembership.Columns.MembershipGroupId, SubSonic.Comparison.Equals, ViewGroupID);
            
            //Gender
            if (gender != "" & gender != "ALL")            
                myViewMembership.Where(ViewMembership.Columns.Gender, SubSonic.Comparison.Like, gender);

            if (MaritalStatus != "")
                myViewMembership.Where(ViewMembership.Columns.MaritalStatus, SubSonic.Comparison.Like, MaritalStatus);

            //Names
            
            if (FirstName != "")
                myViewMembership.Where(ViewMembership.Columns.FirstName, SubSonic.Comparison.Like, "%" + FirstName + "%");

            if (LastName != "")
                myViewMembership.Where(ViewMembership.Columns.LastName, SubSonic.Comparison.Like, LastName);

            if (ChristianName != "")
                myViewMembership.Where(ViewMembership.Columns.ChristianName, SubSonic.Comparison.Like, "%" + ChristianName + "%");

            if (ChineseName != "")
                myViewMembership.Where(ViewMembership.Columns.ChineseName, SubSonic.Comparison.Like, "%" + ChineseName + "%");            
            if (NameToAppear != "")
            {
                Where whr = new Where();
                whr.Condition = Where.WhereCondition.OR;
                
                myViewMembership.Where(ViewMembership.Columns.NameToAppear, SubSonic.Comparison.Like, "%" + NameToAppear + "%");
            }

            //Nationality
            if (Nationality != "")
                myViewMembership.Where(ViewMembership.Columns.Nationality, SubSonic.Comparison.Like, "%" + Nationality + "%");            
            
            //Job
            if (Occupation != "")
                myViewMembership.Where(ViewMembership.Columns.Occupation, SubSonic.Comparison.Like, "%" + Occupation + "%");

            //Address
            if (StreetName != "")
            {
                myViewMembership.Where(ViewMembership.Columns.Address, SubSonic.Comparison.Like, "%" + StreetName + "%");                
            }

            if (BuildingName != "")
                myViewMembership.Where(ViewMembership.Columns.BuildingName, SubSonic.Comparison.Like, "%" + BuildingName + "%");

            if (Block != "")
                myViewMembership.Where(ViewMembership.Columns.Block, SubSonic.Comparison.Like, "%" + Block + "%");

            if (UnitNo != "")
                myViewMembership.Where(ViewMembership.Columns.UnitNo, SubSonic.Comparison.Like, "%" + UnitNo + "%");            

            if (Email != "")
                myViewMembership.Where(ViewMembership.Columns.Email, SubSonic.Comparison.Like, "%" + Email + "%");

            if (Home != "")
                myViewMembership.Where(ViewMembership.Columns.Home, SubSonic.Comparison.Like, "%" + Home + "%");

            if (Office != "")
                myViewMembership.Where(ViewMembership.Columns.Office, SubSonic.Comparison.Like, "%" + Office + "%");

            if (Mobile != "")
                myViewMembership.Where(ViewMembership.Columns.Mobile, SubSonic.Comparison.Like, "%" + Mobile + "%");

            if (ZipCode != "")
                myViewMembership.Where(ViewMembership.Columns.ZipCode, SubSonic.Comparison.Like, "%" + ZipCode + "%");

            if (Country != "")
                myViewMembership.Where(ViewMembership.Columns.Country, SubSonic.Comparison.Like, "%" + Country + "%");

            if (City != "")
                myViewMembership.Where(ViewMembership.Columns.City, SubSonic.Comparison.Like, "%" + City + "%");
                        
            if (NRIC != "")
                myViewMembership.Where(ViewMembership.Columns.Nric, SubSonic.Comparison.Like, "%" + NRIC + "%");


            //Remarks
            if (Remarks != "")
                myViewMembership.Where(ViewMembership.Columns.Remarks, SubSonic.Comparison.Like, "%" + Remarks + "%");  


            SubSonic.TableSchema.TableColumn t = ViewMembership.Schema.GetColumn(SortColumn);

            if (t != null)
            {
                if (SortDir.Trim() == "ASC")
                {
                    myViewMembership.OrderByAsc(SortColumn);
                }
                else if (SortDir.Trim() == "DESC")
                {
                    myViewMembership.OrderByDesc(SortColumn);
                }

            }

            return myViewMembership.Load().ToDataTable();

        }
        */
        public static DataTable FetchMembershipReport
            (bool useStartMembershipDate, bool useEndMembershipDate,
            DateTime StartMembershipDate, DateTime EndMembershipDate,
            bool useStartBirthDate, bool useEndBirthDate,
            DateTime StartBirthDate, DateTime EndBirthDate,
            string StartMembershipNo, string EndMembershipNo, int ViewGroupID,
            string SortColumn, string SortDir)
        {
            ViewMembershipCollection myViewMembership = new ViewMembershipCollection();

            //Membership Date
            if (useStartMembershipDate & useEndMembershipDate)
                myViewMembership.BetweenAnd
                    (ViewMembership.Columns.ExpiryDate, StartMembershipDate, EndMembershipDate);
            else if (useStartMembershipDate)
                myViewMembership.Where(ViewMembership.Columns.ExpiryDate, SubSonic.Comparison.GreaterOrEquals, StartMembershipDate);
            else if (useEndMembershipDate)
                myViewMembership.Where(ViewMembership.Columns.ExpiryDate, SubSonic.Comparison.LessOrEquals, EndMembershipDate);

            //Birth Date
            if (useStartBirthDate & useEndBirthDate)
                myViewMembership.BetweenAnd
                    (ViewMembership.Columns.DateOfBirth, StartBirthDate, EndBirthDate);
            else if (useStartBirthDate)
                myViewMembership.Where(ViewMembership.Columns.DateOfBirth, SubSonic.Comparison.GreaterOrEquals, StartBirthDate);
            else if (useEndBirthDate)
                myViewMembership.Where(ViewMembership.Columns.DateOfBirth, SubSonic.Comparison.LessOrEquals, EndBirthDate);

            //Membership No
            if (StartMembershipNo != "")
                myViewMembership.Where(ViewMembership.Columns.MembershipNo, SubSonic.Comparison.GreaterOrEquals, StartMembershipNo);

            //Membership No
            if (EndMembershipNo != "")
                myViewMembership.Where(ViewMembership.Columns.MembershipNo, SubSonic.Comparison.LessOrEquals, EndMembershipNo);

            //Group ID
            if (ViewGroupID != 0)
                myViewMembership.Where(ViewMembership.Columns.MembershipGroupId, SubSonic.Comparison.Equals, ViewGroupID);

            SubSonic.TableSchema.TableColumn t = ViewMembership.Schema.GetColumn(SortColumn);

            if (t != null)
            {
                if (SortDir.Trim() == "ASC")
                {
                    myViewMembership.OrderByAsc(SortColumn);
                }
                else if (SortDir.Trim() == "DESC")
                {
                    myViewMembership.OrderByDesc(SortColumn);
                }
            }

            return myViewMembership.Load().ToDataTable();

        }

        public static DataTable FetchCustomerPurchaseBehaviorReport
            (DateTime startDate, DateTime EndDate
            , string CategoryName, string ItemName, string PointOfSaleName, string OutletName,
            string membershipNo, string NameToAppear, string FirstName, string LastName, int MembershipGroupID,
            string SortBy, string SortDir)
        {
            if (OutletName == "ALL")
            {
                OutletName = "%";
            }
            if (PointOfSaleName == "ALL")
            {
                PointOfSaleName = "%";
            }
            return SPs.FetchCustomerPurchaseBehaviorReport
                (startDate, EndDate, CategoryName, ItemName,
                PointOfSaleName, OutletName, membershipNo, NameToAppear, FirstName, LastName,MembershipGroupID, SortBy, SortDir).GetDataSet().Tables[0];

        }

        public static DataTable FetchCustomerPurchaseBehaviorReportByCategory(DateTime startDate, DateTime endDate, String CategoryName)
        {
            QueryCommand cmd = new QueryCommand(
                "select  " +
                "	sum(lineamount) as TotalPurchased, " +
                "	sum(Quantity) as TotalItemBought, " +
                "	Count(distinct OrderHdrID) as NumberOfTransaction, " +
                "	sum(lineamount)/(Count(distinct OrderHdrID)+0.0001) as AvgAmountPerTransaction, " +
                "	(sum(lineamount)/(sum(Quantity)+0.0001)) as AvgAmountPerItem, " +
                "	nametoappear, membershipno, Email,mobile, FirstName,LastName,StreetName,City, " +
                "	Country,dateofbirth, expirydate " +
                "from viewtransactionwithmembership " +
                "where orderdate > @startdate and  " +
                "orderdate < @enddate " +
                "and CategoryName like @CategoryName " +
                "group by nametoappear, membershipno, Email,mobile, FirstName,LastName,StreetName,City,Country,dateofbirth, expirydate " +
                "ORDER BY " +
                "	CASE    WHEN @sortby = 'TotalPurchased' and @sortdir = 'DESC'  " +
                "			THEN rank() over (order by sum(lineamount) desc) " +
                "			WHEN @sortby = 'TotalPurchased' and @sortdir = 'ASC'  " +
                "			THEN rank() over (order by sum(lineamount) asc) " +
                "			WHEN @sortby = 'TotalItemBought' and @sortdir = 'DESC'  " +
                "			THEN rank() over (order by sum(Quantity) desc) " +
                "			WHEN @sortby = 'TotalItemBought' and @sortdir = 'ASC'  " +
                "			THEN rank() over (order by sum(Quantity) asc)				 " +
                "			WHEN @sortby = 'NumberOfTransaction' and @sortdir = 'DESC'  " +
                "			THEN rank() over (order by Count(distinct OrderHdrID) desc) " +
                "			WHEN @sortby = 'NumberOfTransaction' and @sortdir = 'ASC'  " +
                "			THEN rank() over (order by Count(distinct OrderHdrID) asc) " +
                "			WHEN @sortby = 'AvgAmountPerTransaction' and @sortdir = 'DESC'  " +
                "			THEN rank() over (order by (sum(lineamount)/(Count(distinct OrderHdrID)+0.0001)) desc) " +
                "			WHEN @sortby = 'AvgAmountPerTransaction' and @sortdir = 'ASC'  " +
                "			THEN rank() over (order by (sum(lineamount)/(Count(distinct OrderHdrID)+0.0001)) asc) " +
                "			WHEN @sortby = 'AvgAmountPerItem' and @sortdir = 'DESC'  " +
                "			THEN rank() over (order by (sum(lineamount)/(sum(Quantity)+0.0001)) desc) " +
                "			WHEN @sortby = 'AvgAmountPerItem' and @sortdir = 'ASC'  " +
                "			THEN rank() over (order by (sum(lineamount)/(sum(Quantity)+0.0001)) asc) " +
                "			WHEN @sortby = 'nametoappear' and @sortdir = 'DESC'  " +
                "			THEN rank() over (order by nametoappear desc) " +
                "			WHEN @sortby = 'nametoappear' and @sortdir = 'ASC'  " +
                "			THEN rank() over (order by nametoappear asc) " +
                "		    WHEN @sortby = 'membershipno' and @sortdir = 'DESC'  " +
                "			THEN rank() over (order by membershipno desc)				 " +
                "			WHEN @sortby = 'membershipno' and @sortdir = 'ASC'  " +
                "			THEN rank() over (order by membershipno ASC)	 " +
                "			ELSE rank() over (order by sum(lineamount) DESC)			 " +
                "	END ", 
                "PowerPOS");

            cmd.AddParameter("@startdate", startDate, DbType.DateTime, null, null);
            cmd.AddParameter("@enddate", endDate, DbType.DateTime, null, null);
            cmd.AddParameter("@CategoryName", CategoryName, DbType.AnsiString, null, null);
            cmd.AddParameter("@sortby", "", DbType.AnsiString, null, null);
            cmd.AddParameter("@sortdir", "", DbType.AnsiString, null, null);

            return DataService.GetDataSet(cmd).Tables[0];
        }

        public static DataTable FetchCustomerPurchaseGroupByCategory
            (DateTime startDate, DateTime endDate)
        {
            DataTable dt = new DataTable("PurchaseByCategory");
            dt.Columns.Add("MembershipNo");
            dt.Columns.Add("NameToAppear");
            CategoryCollection CatCol = new CategoryCollection();
            CatCol.Load();

            Logger.writeLog("Get Data Category list ");
            for (int i = 0; i < CatCol.Count; i++)
            {
                dt.Columns.Add(new DataColumn(CatCol[i].CategoryName, Type.GetType("System.Decimal")));
            }
            Logger.writeLog("Finish Get Data Category list " + CatCol.Count.ToString());

            dt.Columns.Add(new DataColumn("Total", Type.GetType("System.Decimal")));
            DataTable[] InvBalQty;
            InvBalQty = new DataTable[CatCol.Count];

            //Assign purchase of every category
            for (int i = 2; i < dt.Columns.Count - 1; i++)
            {
                Logger.writeLog("Get Data Category for " + dt.Columns[i].ColumnName);
                InvBalQty[i - 2] =
                    ReportController.FetchCustomerPurchaseBehaviorReportByCategory(
                        startDate, endDate, dt.Columns[i].ColumnName);

                InvBalQty[i - 2].TableName = dt.Columns[i].ColumnName;

                Logger.writeLog("Finish Get Data Category for " + dt.Columns[i].ColumnName);
            }
            Logger.writeLog("Get Data Member list ");
            MembershipCollection myMember = new MembershipCollection();
            myMember.OrderByAsc("MembershipNo");
            myMember.Load();
            Logger.writeLog("Finish Get Data Member list ");


            dt.Columns.Add("FirstName");
            dt.Columns.Add("LastName");
            dt.Columns.Add("Email");
            if (dt.Columns.IndexOf("Mobile") >= 0)
                dt.Columns.Remove("Mobile");
            dt.Columns.Add("Mobile");
            dt.Columns.Add("Home");
            dt.Columns.Add("Office");
            dt.Columns.Add("StreetName");
            dt.Columns.Add("ZipCode");
            dt.Columns.Add("City");
            dt.Columns.Add("Country");

            DataRow[] dr;
            DataRow dtRow;

            Logger.writeLog("assign data member and total ");
            for (int j = 0; j < myMember.Count; j++)
            {
                dtRow = dt.NewRow();
                dtRow["MembershipNo"] = myMember[j].MembershipNo;
                dtRow["NameToAppear"] = myMember[j].NameToAppear;
                dtRow["FirstName"] = myMember[j].FirstName;
                dtRow["LastName"] = myMember[j].LastName;
                dtRow["Email"] = myMember[j].Email;
                dtRow["Mobile"] = String.IsNullOrEmpty(myMember[j].Mobile) ? "" : myMember[j].Mobile;
                dtRow["Home"] = myMember[j].Home;
                dtRow["Office"] = myMember[j].Office;
                dtRow["StreetName"] = myMember[j].StreetName;
                dtRow["ZipCode"] = myMember[j].ZipCode;
                dtRow["City"] = myMember[j].City;
                dtRow["Country"] = myMember[j].Country;

                for (int i = 0; i < InvBalQty.Length; i++)
                {
                    dr = InvBalQty[i].Select("MembershipNo = '" + myMember[j].MembershipNo + "'");
                    if (dr != null && dr.Length > 0)
                    {
                        dtRow[InvBalQty[i].TableName] = dr[0]["TotalPurchased"].ToString();
                    }
                    else
                    {
                        dtRow[InvBalQty[i].TableName] = "0";
                    }
                }
                dt.Rows.Add(dtRow);
            }
            Logger.writeLog("Finish assign data member and total ");

            for (int i = 0; i < dt.Rows.Count; i++)
            {

                decimal total = 0;
                for (int k = 0; k < CatCol.Count; k++)
                {
                    decimal tmpAmt = 0;
                    if (decimal.TryParse(dt.Rows[i][k + 2].ToString(),out tmpAmt))
                        total += tmpAmt;
                }
                dt.Rows[i]["Total"] = total;
            }

            return dt;
        }
    }
}