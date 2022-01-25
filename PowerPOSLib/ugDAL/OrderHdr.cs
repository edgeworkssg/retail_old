using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic;

namespace PowerPOS
{
    public partial class OrderHdr
    {
        public partial struct UserColumns
        {
            /// <summary>Userfld1</summary>
            public static string ProjectName = @"Userfld1";
            /// <summary>Userfld8</summary>
            public static string OrderType = @"Userfld8";
            /// <summary>Userfld9</summary>
            public static string QuotationHdrID = @"Userfld9";
            /// <summary>Userflag2</summary>
            public static string IsExported = @"Userflag2";
            /// <summary>Userfld2</summary>
            public static string NumOfCopies = @"Userfld2";
            /// <summary>Userfld3</summary>
            public static string BillInfo = @"Userfld7";
            /// <summary>Userint1</summary>
            public static string GSTRule = @"Userint1";

            /// <summary>Userfld10</summary>
            public static string ShopifyTransactionID = @"Userfld10";

        }
        #region Custom Properties
        /// <summary>
        /// Line Project Name for each OrderHdr
        /// </summary>
        public string ProjectName
        {
            get { return GetColumnValue<string>(UserColumns.ProjectName); }
            set { SetColumnValue(UserColumns.ProjectName, value); }
        }
        /// <summary>
        /// Cash And Carry (CASH_CARRY) or Pre-Order (PREORDER)
        /// </summary>
        public string OrderType
        {
            get { return GetColumnValue<string>(UserColumns.OrderType); }
            set { SetColumnValue(UserColumns.OrderType, value); }
        }

        public string QuotationHdrID
        {
            get { return GetColumnValue<string>(UserColumns.QuotationHdrID); }
            set { SetColumnValue(UserColumns.QuotationHdrID, value); }
        }

        /// <summary>
        /// Is the Order has been exported to integration file
        /// </summary>
        public bool IsExported
        {
            get { return GetColumnValue<bool>(UserColumns.IsExported); }
            set { SetColumnValue(UserColumns.IsExported, value); }
        }

        /// <summary>
        /// Num Of Copies for SICC Validation Printing
        /// </summary>
        public string NumOfCopies
        {
            get { return GetColumnValue<string>(UserColumns.NumOfCopies); }
            set { SetColumnValue(UserColumns.NumOfCopies, value); }
        }

        /// <summary>
        /// Bill Info for wholesale
        /// </summary>
        public string BillInfo
        {
            get { return GetColumnValue<string>(UserColumns.BillInfo); }
            set { SetColumnValue(UserColumns.BillInfo, value); }
        }

        /// <summary>
        /// ShopifyTransactionID for Shopify Integration
        /// </summary>
        public string ShopifyTransactionID
        {
            get { return GetColumnValue<string>(UserColumns.ShopifyTransactionID); }
            set { SetColumnValue(UserColumns.ShopifyTransactionID, value); }
        }

        /// <summary>
        /// GST Rule
        /// </summary>
        public int GSTRule
        {
            get { return GetColumnValue<int>(UserColumns.GSTRule); }
            set { SetColumnValue(UserColumns.GSTRule, value); }
        }
        #endregion

        #region Custom Properties
        /// <summary>
        /// Check whether PointAllocationLog for this OrderHdrID already exists or not.
        /// </summary>
        /// <returns>boolean</returns>
        public bool IsPointAllocationLogExists()
        {
            try
            {
                string sql = @"
                                IF EXISTS (SELECT * FROM PointAllocationLog WHERE OrderHdrID = '{0}')
                                    SELECT CAST(1 AS bit)
                                ELSE
                                    SELECT CAST(0 AS bit)
                              ";
                sql = string.Format(sql, OrderHdrID);
                object obj = DataService.ExecuteScalar(new QueryCommand(sql, "PowerPOS"));
                if (obj != null && obj is bool && (bool)obj == true)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex.Message);
                return false;
            }
        }
        #endregion
    }
}
