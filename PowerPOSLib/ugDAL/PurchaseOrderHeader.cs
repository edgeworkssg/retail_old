using System;

namespace PowerPOS
{
    public partial class PurchaseOrderHeader
    {
        public partial struct UserColumns
        {
            /// <summary>Userfld1</summary>
            public static string Status = @"Userfld1";
            /// <summary>Userfld2</summary>
            public static string POType = @"Userfld2";
            /// <summary>Userfld3</summary>
            public static string ApprovalDate = @"Userfld3";
            /// <summary>Userfld4</summary>
            public static string ApprovedBy = @"Userfld4";
            /// <summary>Userfld5</summary>
            public static string SpecialValidFrom = @"Userfld5";
            /// <summary>Userfld6</summary>
            public static string SpecialValidTo = @"Userfld6";
            /// <summary>Userfld7</summary>
            public static string ApprovalStatus = @"Userfld7";
            /// <summary>Userfld8</summary>
            public static string SalesPersonID = @"Userfld8";
            /// <summary>Userfld9</summary>
            public static string PriceLevel = @"Userfld9";
            /// <summary>Userint1</summary>
            public static string ReasonID = @"Userint1";
            /// <summary>Userint2</summary>
            public static string DestInventoryLocationID = @"Userint2";
            /// <summary>Userint3</summary>
            public static string WarehouseID = @"Userint3";
            /// <summary>Userflag1</summary>
            public static string IsAutoStockIn = @"Userflag1";
        }

        #region Custom Properties
        /// <summary>
        /// Order Status (Pending, Submitted, Approved, Rejected)
        /// </summary>
        public string Status
        {
            get { return GetColumnValue<string>(UserColumns.Status); }
            set { SetColumnValue(UserColumns.Status, value); }
        }

        /// <summary>
        /// POType (Order, Return)
        /// </summary>
        public string POType
        {
            get { return GetColumnValue<string>(UserColumns.POType); }
            set { SetColumnValue(UserColumns.POType, value); }
        }

        /// <summary>
        /// Reason for Stock Out / Return
        /// </summary>
        public int ReasonID
        {
            get { return GetColumnValue<int>(UserColumns.ReasonID); }
            set { SetColumnValue(UserColumns.ReasonID, value); }
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

        /// <summary>
        /// Approval Status
        /// </summary>
        public string ApprovalStatus
        {
            get { return GetColumnValue<string>(UserColumns.ApprovalStatus); }
            set { SetColumnValue(UserColumns.ApprovalStatus, value); }
        }

        /// <summary>
        /// Destination Inventory Location
        /// </summary>
        public int DestInventoryLocationID
        {
            get { return GetColumnValue<int>(UserColumns.DestInventoryLocationID); }
            set { SetColumnValue(UserColumns.DestInventoryLocationID, value); }
        }

        /// <summary>
        /// Returns a InventoryLocation ActiveRecords object related to this PurchaseOrderHeader
        /// </summary>
        public InventoryLocation DestInventoryLocation
        {
            get { return new InventoryLocation(DestInventoryLocationID); }
        }

        /// <summary>
        /// Returns a InventoryStockOutReason ActiveRecords object related to this PurchaseOrderHeader
        /// </summary>
        public InventoryStockOutReason InventoryStockOutReason
        {
            get { return new InventoryStockOutReason(ReasonID); }
        }

        /// <summary>
        /// Valid From for Special Order.
        /// </summary>
        public DateTime SpecialValidFrom
        {
            get { return Convert.ToDateTime(GetColumnValue<string>(UserColumns.SpecialValidFrom)); }
            set { SetColumnValue(UserColumns.SpecialValidFrom, value.ToString("yyyy-MM-dd HH:mm:ss.fff")); }
        }

        /// <summary>
        /// Valid To for Special Order.
        /// </summary>
        public DateTime SpecialValidTo
        {
            get { return Convert.ToDateTime(GetColumnValue<string>(UserColumns.SpecialValidTo)); }
            set { SetColumnValue(UserColumns.SpecialValidTo, value.ToString("yyyy-MM-dd HH:mm:ss.fff")); }
        }

        public string SalesPersonID
        {
            get { return GetColumnValue<string>(UserColumns.SalesPersonID); }
            set { SetColumnValue(UserColumns.SalesPersonID, value); }
        }

        public UserMst SalesPerson
        {
            get { return string.IsNullOrEmpty(SalesPersonID) ? null : new UserMst(SalesPersonID); }
        }

        /// <summary>
        /// WarehouseID (Userint3)
        /// The InventoryLocationID of the warehouse for this goods order
        /// </summary>
        public int WarehouseID
        {
            get { return GetColumnValue<int>(UserColumns.WarehouseID); }
            set { SetColumnValue(UserColumns.WarehouseID, value); }
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
        /// Auto Stock In.
        /// </summary>
        public bool? IsAutoStockIn
        {
            get { return GetColumnValue<bool?>(UserColumns.IsAutoStockIn); }
            set { SetColumnValue(UserColumns.IsAutoStockIn, value); }
        }

        #endregion
    }
}
