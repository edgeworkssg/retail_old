using SubSonic;
using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PowerPOS
{
    public partial class Installment
    {
        public const string CreditName = "INSTALLMENT";
        public const string CreditPaymentName = "INST_PAYMENT";
        public const string CreditNote = "CREDIT_NOTE";

        public partial struct UserColumns
        {
            /// <summary>Userfld1</summary>
            public static string CustomRefNo = @"Userfld1";

            public static string TotalCreditNote = @"Userfloat1";
        }

        #region Custom Properties

        /// <summary>
        /// CustomRefNo (Userfld1)
        /// </summary>
        public string CustomRefNo
        {
            get { return GetColumnValue<string>(UserColumns.CustomRefNo); }
            set { SetColumnValue(UserColumns.CustomRefNo, value); }
        }

        /// <summary>
        /// CustomRefNo (Userfloat1)
        /// </summary>
        public string TotalCreditNote
        {
            get { return GetColumnValue<string>(UserColumns.TotalCreditNote); }
            set { SetColumnValue(UserColumns.TotalCreditNote, value); }
        }

        #endregion


        #region *) OBSOLETE
        //public static string GetMemberHistory_SQL(string membershipNo, DateTime startDate, DateTime endDate)
        //{
        //    return
        //        "DECLARE @credit AS VARCHAR(50); " +
        //        "DECLARE @creditpayment AS VARCHAR(50); " +
        //        "DECLARE @startdate AS DATETIME; " +
        //        "DECLARE @enddate AS DATETIME; " +
        //        "DECLARE @membershipno AS VARCHAR(50); " +
        //        " " +
        //        "SET @credit = '" + CreditName + "'; " +
        //        "SET @creditpayment = '" + CreditPaymentName + "' " +
        //        "SET @startdate = '" + startDate.ToString("yyyy-MM-dd HH:mm:ss") + "'; " +
        //        "SET @enddate = '" + endDate.ToString("yyyy-MM-dd HH:mm:ss") + "'; " +
        //        "SET @membershipno = '" + membershipNo + "' " +
        //        " " +
        //        "SELECT OrderHdrID AS ReceiptNo, ISNULL(OH.userfld5,OH.OrderHdrID) as OrderRefNo, ISNULL(OH.userfld5,OH.OrderHdrID) AS PaymentRefNo, OrderHdrID as PaymentFor, OrderDate AS ReceiptDate " +
        //            ", ISNULL(SUM(Amount),0.00) AS Credit, 0.00 AS Debit " +
        //        "FROM OrderHdr OH " +
        //            "INNER JOIN ReceiptDet RD ON OH.OrderHdrID = RD.ReceiptHdrID " +
        //            "INNER JOIN Membership MM on MM.MembershipNo = OH.MembershipNo " +
        //        "WHERE OH.IsVoided = 0 " +
        //            "AND PaymentType = @credit " +
        //            "AND OrderDate >= @startdate AND OrderDate <= @enddate " +
        //            "AND MM.MembershipNo = @membershipno " +
        //        "GROUP BY OrderDate, OrderHdrID, OH.UserFld5 " +
        //        " " +
        //        "UNION ALL " +
        //        " " +
        //        "SELECT OH.OrderHdrId as ReceiptNo, ISNULL(OH.userfld5,OH.OrderHdrID) as OrderRefNo, " + 
        //        "(SELECT ISNULL(OrderHdr.UserFld5,OrderHdrId) FROM OrderHdr WHERE OrderHdr.OrderHdrId= OD." + OrderDet.UserColumns.InstRefNo + ") AS PaymentRefno, OD." + OrderDet.UserColumns.InstRefNo + " as PaymentFor, OrderDate as ReceiptDate " +
        //            ",0.00 as Credit, ISNULL(SUM(AMOUNT),0.00) as debit  " +
        //        "FROM OrderHdr OH " +
        //            "INNER JOIN OrderDet OD ON OH.OrderHdrID = OD.OrderHdrID " +
        //            "INNER JOIN Membership MM on MM.MembershipNo = OH.MembershipNo " +
        //        "WHERE OH.IsVoided = 0 and OD.IsVoided = 0 " +
        //        "AND MM.MembershipNo = @membershipno " +
        //        "AND ItemNo = @creditpayment " +
        //        "AND OrderDate >= @startdate AND OrderDate <= @enddate " +
        //        "GROUP BY OH.OrderHdrID, OD." + OrderDet.UserColumns.InstRefNo + ", OrderDate, OH.UserFld5 ";
        //}
        #endregion

        public static string GetMemberHistory_SQL(string membershipNo, DateTime startDate, DateTime endDate)
        {
            string sql;
            sql = @"
                    DECLARE @membershipNo varchar(50) 
                    SET @membershipNo = '{0}' 

                    SELECT id.OrderHdrID AS ReceiptNo, 
                           id.OrderHdrID AS OrderRefNo, 
                           id.userfld1 AS CustomOrderRefNo, 
                           ih.OrderHdrID AS PaymentFor, 
                           ih.OrderHdrID AS PaymentRefNo, 
                           ih.userfld1 AS CustomPaymentRefNo, 
                           id.CreatedOn AS ReceiptDate, 
                           CASE WHEN id.InstallmentAmount > 0 THEN id.InstallmentAmount ELSE 0 END Credit, 
                           CASE WHEN id.InstallmentAmount < 0 THEN -id.InstallmentAmount ELSE 0 END Debit 
                    FROM Installment ih 
                        INNER JOIN InstallmentDetail id ON id.InstallmentRefNo = ih.InstallmentRefNo 
                    WHERE ih.MembershipNo = @membershipNo 
                        AND ih.Deleted = 0 AND id.Deleted = 0 
                    ORDER BY id.OrderHdrID 
                   ";
            sql = string.Format(sql, membershipNo);
            return sql;
        }

        public static DataTable GetMemberHistory(string membershipNo, DateTime startDate, DateTime endDate)
        {
            string SQLString = GetMemberHistory_SQL(membershipNo, startDate, endDate);

            DataTable DT = new DataTable();
            DT.Load(DataService.GetReader(new QueryCommand(SQLString)));

            return DT;
        }

        
        #region *) OBSOLETE
        //public static string GetOutstandingBalances_SQL(string OrderHdrID, DateTime ReportDate)
        //{
        //    return
        //        "DECLARE @credit AS VARCHAR(50); " +
        //        "DECLARE @creditpayment AS VARCHAR(50); " +
        //        "DECLARE @ReportDate AS DATETIME; " +
        //        "DECLARE @OrderHdrID AS VARCHAR(50); " +
        //        " " +
        //        "SET @credit = '" + CreditName + "'; " +
        //        "SET @creditpayment = '" + CreditPaymentName + "' " +
        //        "SET @ReportDate = '" + ReportDate.ToString("yyyy-MM-dd HH:mm:ss") + "'; " +
        //        "SET @OrderHdrID = '" + OrderHdrID + "'; " +
        //        " " +
        //        "SELECT ReceiptNo, ISNULL(SUM(Credit),0.00) AS Credit, ISNULL(SUM(Debit),0.00) AS Debit " +
        //        "FROM " +
        //        "( " +
        //            "SELECT OrderHdrID AS ReceiptNo " +
        //                ", ISNULL(SUM(Amount),0.00) AS Credit, 0.00 AS Debit " +
        //            "FROM OrderHdr OH " +
        //                "INNER JOIN ReceiptDet RD ON OH.OrderHdrID = RD.ReceiptHdrID " +
        //            "WHERE OH.IsVoided = 0 " +
        //                "AND PaymentType = @credit " +
        //                "AND OrderDate <= @ReportDate " +
        //                "AND OH.OrderHdrID LIKE @OrderHdrID " +
        //            "GROUP BY OrderHdrID " +
        //            " " +
        //            "UNION ALL " +
        //            " " +
        //            "SELECT OD." + OrderDet.UserColumns.InstRefNo + " as ReceiptNo " +
        //                ",0.00 as Credit, ISNULL(SUM(AMOUNT),0.00) as Debit  " +
        //            "FROM OrderHdr OH " +
        //                "INNER JOIN OrderDet OD ON OH.OrderHdrID = OD.OrderHdrID " +
        //            "WHERE OH.IsVoided = 0 AND OD.IsVoided = 0 " +
        //                "AND ItemNo = @creditpayment " +
        //                "AND OrderDate <= @ReportDate " +
        //                "AND OD." + OrderDet.UserColumns.InstRefNo + " LIKE @OrderHdrID " +
        //            "GROUP BY OD." + OrderDet.UserColumns.InstRefNo + " " +
        //        ") DT " +
        //        "GROUP BY ReceiptNo ";

        //}

        //public static decimal GetOutstandingBalance(string OrderHdrID, DateTime ReportDate)
        //{
        //    string SQLString = GetOutstandingBalances_SQL(OrderHdrID, ReportDate);

        //    DataTable DT = new DataTable();
        //    DT.Load(DataService.GetReader(new QueryCommand(SQLString)));

        //    decimal Rst = 0;
        //    for (int Counter = 0; Counter < DT.Rows.Count; Counter++)
        //    {
        //        Rst += (decimal)DT.Rows[Counter]["Credit"];
        //        Rst -= (decimal)DT.Rows[Counter]["Debit"];
        //    }

        //        return Rst;

        //}
        #endregion

        public static decimal GetOutstandingBalance(string OrderHdrID, DateTime ReportDate)
        {
            Installment ins = new Installment("OrderHdrID", OrderHdrID);
            if (ins != null && ins.OrderHdrId == OrderHdrID)
                return ins.CurrentBalance.GetValueOrDefault(0);
            else
                return 0;
        }

        public static decimal GetOutstandingBalancePerMember(string MembershipNo)
        {
            decimal balance = 0;
            InstallmentCollection insColl = new InstallmentCollection();
            insColl.Where("MembershipNo", MembershipNo);
            insColl.Where("Deleted", false);
            insColl.Load();

            foreach (Installment ins in insColl)
            {
                balance += ins.CurrentBalance.GetValueOrDefault(0);
            }

            return balance;
        }

        public static DataTable GetMemberInstallmentHistory(string ID)
        {
            string SQLString =
            "DECLARE @credit AS VARCHAR(50); " +
            "DECLARE @creditpayment AS VARCHAR(50); " +
            "DECLARE @startdate AS DATETIME; " +
            "DECLARE @enddate AS DATETIME; " +
            "DECLARE @membershipno AS VARCHAR(50); " +
            "SET @credit = 'INSTALLMENT'; " +
            "SET @creditpayment = 'INST_PAYMENT' " +
            "SET @startdate = '1990-01-01 00:00:00'; " +
            "SET @enddate = '2100-01-01 00:00:00'; " +
            "SET @membershipno = '" + ID + "'; " +
            "SELECT OH.OrderDate,MM.MembershipNo, MM.NameToAppear Name,OrderHdrID, OH.UserFld5 AS ReceiptNo, OH.UserFld5 AS PaymentFor, OrderDate AS ReceiptDate ," +
            "ISNULL(SUM(Amount),0.00) AS Credit, 0.00 AS Debit, LP.PointOfSaleName Outlet " +
            "FROM OrderHdr OH " +
            "INNER JOIN ReceiptDet RD ON OH.OrderHdrID = RD.ReceiptHdrID " +
            "INNER JOIN Membership MM on MM.MembershipNo = OH.MembershipNo " +
            "INNER JOIN PointOfSale LP ON LP.PointOfSaleID=OH.PointOfSaleID " +
            "WHERE OH.IsVoided = 0 " +
            "AND PaymentType = @credit " +
            "AND OrderDate >= @startdate AND OrderDate <= @enddate " +
            "AND MM.MembershipNo = @membershipno " +
            "GROUP BY OrderDate, OrderHdrID, OH.UserFld5 ,MM.MembershipNo, MM.NameToAppear,LP.PointOfSaleName " +

            "UNION ALL  " +

            "SELECT OH.OrderDate,MM.MembershipNo, MM.NameToAppear Name, OH.OrderHdrId, OH.UserFld5 as ReceiptNo, " +
            "(SELECT OrderHdr.UserFld5 FROM OrderHdr WHERE OrderHdr.OrderHdrId= OD.Userfld3) AS PaymentFor, " +
            "OrderDate as ReceiptDate ,0.00 as Credit, ISNULL(SUM(AMOUNT),0.00) as debit  , LP.PointOfSaleName Outlet " +
            "FROM OrderHdr OH " +
            "INNER JOIN OrderDet OD ON OH.OrderHdrID = OD.OrderHdrID " +
            "INNER JOIN Membership MM on MM.MembershipNo = OH.MembershipNo " +
            "INNER JOIN PointOfSale LP ON LP.PointOfSaleID=OH.PointOfSaleID " +
            "WHERE OH.IsVoided = 0 and OD.IsVoided = 0 " +
            "AND MM.MembershipNo = @membershipno " +
            "AND ItemNo = @creditpayment AND OrderDate >= @startdate " +
            "AND OrderDate <= @enddate " +
            "GROUP BY OH.OrderHdrID,OH.UserFld5 ,OD.Userfld3, OrderDate,MM.MembershipNo, MM.NameToAppear, LP.PointOfSaleName ";


            DataTable DT = new DataTable();
            DT.Load(DataService.GetReader(new QueryCommand(SQLString)));

            return DT;
        }
    }
}
