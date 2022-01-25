namespace PowerPOS
{
    public partial class PurchaseOrderDetail
    {
        public partial struct UserColumns
        {
            /// <summary>Userfld1</summary>
            public static string Status = @"Userfld1";

            /// <summary>Userfld2</summary>
            public static string DiscountDetail = @"Userfld2";

            /// <summary>Userfld3</summary>
            public static string SerialNo = @"Userfld3";

            /// <summary>Userint1</summary>
            public static string QtyApproved = @"Userint1";

            /// <summary>Userint2</summary>
            public static string SalesQty = @"Userint2";

            /// <summary>Userint3</summary>
            public static string RejectQty = @"Userint3";

            /// <summary>Userint2</summary>
            public static string OriginalQuantity = @"Userint4";

            /// <summary>Userint5</summary>
            public static string GSTRule = @"Userint5";
        }

        #region Custom Properties
        /// <summary>
        /// Order Status (Approved, Rejected)
        /// </summary>
        public string Status
        {
            get { return GetColumnValue<string>(UserColumns.Status); }
            set { SetColumnValue(UserColumns.Status, value); }
        }

        /// <summary>
        /// Discount Detail
        /// </summary>
        public string DiscountDetail
        {
            get { return GetColumnValue<string>(UserColumns.DiscountDetail); }
            set { SetColumnValue(UserColumns.DiscountDetail, value); }
        }

        /// <summary>
        /// SerialNo
        /// </summary>
        public string SerialNo
        {
            get { return GetColumnValue<string>(UserColumns.SerialNo); }
            set { SetColumnValue(UserColumns.SerialNo, value); }
        }

        /// <summary>
        /// Quantity Approved
        /// </summary>
        public decimal QtyApproved
        {
            get { return GetColumnValue<decimal>(UserColumns.QtyApproved); }
            set { SetColumnValue(UserColumns.QtyApproved, value); }
        }

        /// <summary>
        /// Sales Qty
        /// </summary>
        public decimal SalesQty
        {
            get { return GetColumnValue<decimal>(UserColumns.SalesQty); }
            set { SetColumnValue(UserColumns.SalesQty, value); }
        }


        /// <summary>
        /// Reject Qty
        /// </summary>
        public decimal RejectQty
        {
            get { return GetColumnValue<decimal>(UserColumns.RejectQty); }
            set { SetColumnValue(UserColumns.RejectQty, value); }
        }

        /// <summary>
        /// Original Quantity
        /// </summary>
        public decimal OriginalQuantity
        {
            get { return GetColumnValue<decimal>(UserColumns.OriginalQuantity); }
            set { SetColumnValue(UserColumns.OriginalQuantity, value); }
        }

        /// <summary>
        /// GSTRule
        /// </summary>
        public int? GSTRule
        {
            get { return GetColumnValue<int?>(UserColumns.GSTRule); }
            set { SetColumnValue(UserColumns.GSTRule, value); }
        }
        #endregion
    }
}
