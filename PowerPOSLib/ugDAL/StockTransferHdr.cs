using System;

namespace PowerPOS
{
    public partial class StockTransferHdr
    {
        public partial struct UserColumns
        {
            /// <summary>Userfld1</summary>
            public static string InventoryHdrRefNoFrom = @"Userfld1";

            /// <summary>Userfld2</summary>
            public static string InventoryHdrRefNoTo = @"Userfld2";

            /// <summary>Userfld3</summary>
            public static string ReceivedBy = @"Userfld3";
            
            /// <summary>Userfld4</summary>
            public static string CreditInvoiceNo = @"Userfld4";

            /// <summary>Userfld5</summary>
            public static string InvoiceNo = @"Userfld5";

            /// <summary>Userfld6</summary>
            public static string ApprovalDate = @"Userfld6";

            /// <summary>Userfld7</summary>
            public static string ApprovedBy = @"Userfld7";

            /// <summary>Userfld9</summary>
            public static string PriceLevel = @"Userfld9";

            /// <summary>Userint1</summary>
            public static string ReturnToWarehouseID = @"Userint1";

            /// <summary>Userint2</summary>
            public static string ReturnToSupplierID = @"Userint2";

            /// <summary>Userflag1</summary>
            public static string AutoStockIn = @"Userflag1";
        }

        #region Custom Properties

        /// <summary>
        /// PriceLevel (Userfld1)
        /// </summary>
        public string InventoryHdrRefNoFrom
        {
            get { return GetColumnValue<string>(UserColumns.InventoryHdrRefNoFrom); }
            set { SetColumnValue(UserColumns.InventoryHdrRefNoFrom, value); }
        }

        /// <summary>
        /// PriceLevel (Userfld2)
        /// </summary>
        public string InventoryHdrRefNoTo
        {
            get { return GetColumnValue<string>(UserColumns.InventoryHdrRefNoTo); }
            set { SetColumnValue(UserColumns.InventoryHdrRefNoTo, value); }
        }

        /// <summary>
        /// PriceLevel (Userfld3)
        /// </summary>
        public string ReceivedBy
        {
            get { return GetColumnValue<string>(UserColumns.ReceivedBy); }
            set { SetColumnValue(UserColumns.ReceivedBy, value); }
        }

        /// <summary>
        /// CreditInvoiceNo (Userfld4)
        /// </summary>
        public string CreditInvoiceNo
        {
            get { return GetColumnValue<string>(UserColumns.CreditInvoiceNo); }
            set { SetColumnValue(UserColumns.CreditInvoiceNo, value); }
        }

        /// <summary>
        /// CreditInvoiceNo (Userfld5)
        /// </summary>
        public string InvoiceNo
        {
            get { return GetColumnValue<string>(UserColumns.InvoiceNo); }
            set { SetColumnValue(UserColumns.InvoiceNo, value); }
        }

        /// <summary>
        /// PriceLevel (Userfld9)
        /// </summary>
        public string PriceLevel
        {
            get { return GetColumnValue<string>(UserColumns.PriceLevel); }
            set { SetColumnValue(UserColumns.PriceLevel, value); }
        }

        /// <summary>
        /// WarehouseID (Userint1)
        /// </summary>
        public int ReturnToWarehouseID
        {
            get { return GetColumnValue<int>(UserColumns.ReturnToWarehouseID); }
            set { SetColumnValue(UserColumns.ReturnToWarehouseID, value); }
        }

        /// <summary>
        /// PriceLevel (Userint2)
        /// </summary>
        public int ReturnToSupplierID
        {
            get { return GetColumnValue<int>(UserColumns.ReturnToSupplierID); }
            set { SetColumnValue(UserColumns.ReturnToSupplierID, value); }
        }

        /// <summary>
        /// Auto Stock In (Userflag1)
        /// </summary>
        public bool AutoStockIn
        {
            get { return GetColumnValue<bool>(UserColumns.AutoStockIn); }
            set { SetColumnValue(UserColumns.AutoStockIn, value); }
        }

        /// <summary>
        /// Date and time when the order is approved or rejected.
        /// </summary>
        public DateTime ApprovalDate
        {
            get { return Convert.ToDateTime(GetColumnValue<string>(UserColumns.ApprovalDate)); }
            set { SetColumnValue(UserColumns.ApprovalDate, value.ToString("yyyy-MM-dd HH:mm:ss.fff")); }
        }

        /// <summary>
        /// Approved By
        /// </summary>
        public string ApprovedBy
        {
            get { return GetColumnValue<string>(UserColumns.ApprovedBy); }
            set { SetColumnValue(UserColumns.ApprovedBy, value); }
        }
        #endregion
    }
}
