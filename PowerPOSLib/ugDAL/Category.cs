namespace PowerPOS
{
    public partial class Category
    {
        public partial struct UserColumns
        {
            /// <summary>Userfloat1</summary>
            public static string PAMedifundCap = @"Userfloat1";
            /// <summary>Userfloat2</summary>
            public static string SMFCap = @"Userfloat2";
            /// <summary>Userfloat2</summary>
            public static string GSTPercentage = @"Userfloat3";
            /// <summary>Userflag1</summary>
            public static string IsSellingRestriction = @"Userflag1";
            /// <summary>Userflag2</summary>
            public static string IsOverrideGST = @"Userflag2";
            /// <summary>Userfld1</summary>
            public static string RestrictedTimeStart = @"Userfld1";
            /// <summary>Userfld2</summary>
            public static string RestrictedTimeEnd = @"Userfld2";
            /// <summary>Userfld3</summary>
            public static string BarcodePrefix = @"Userfld3";
            /// <summary>Userfld4</summary>
            public static string LastBarcodeGenerated = @"Userfld4";
            /// <summary>Userflag2</summary>
            public static string AgeRestrictedItems = @"Userint1";
        }

        #region Custom Properties
        /// <summary>
        /// PAMedifundCap
        /// </summary>
        public decimal PAMedifundCap
        {
            get { return GetColumnValue<decimal>(UserColumns.PAMedifundCap); }
            set { SetColumnValue(UserColumns.PAMedifundCap, value); }
        }

        /// <summary>
        /// SMFCap
        /// </summary>
        public decimal SMFCap
        {
            get { return GetColumnValue<decimal>(UserColumns.SMFCap); }
            set { SetColumnValue(UserColumns.SMFCap, value); }
        }

        /// <summary>
        /// GSTPercentage
        /// </summary>
        public decimal GSTPercentage
        {
            get { return GetColumnValue<decimal>(UserColumns.GSTPercentage); }
            set { SetColumnValue(UserColumns.GSTPercentage, value); }
        }

        /// <summary>
        /// IsSellingRestriction
        /// </summary>
        public bool IsSellingRestriction
        {
            get { return GetColumnValue<bool>(UserColumns.IsSellingRestriction); }
            set { SetColumnValue(UserColumns.IsSellingRestriction, value); }
        }

        /// <summary>
        /// IsOverrideGST
        /// </summary>
        public bool IsOverrideGST
        {
            get { return GetColumnValue<bool>(UserColumns.IsOverrideGST); }
            set { SetColumnValue(UserColumns.IsOverrideGST, value); }
        }

        /// <summary>
        /// RestrictedTimeStart
        /// </summary>
        public string RestrictedTimeStart
        {
            get { return GetColumnValue<string>(UserColumns.RestrictedTimeStart); }
            set { SetColumnValue(UserColumns.RestrictedTimeStart, value); }
        }
        /// <summary>
        /// RestrictedTimeEnd
        /// </summary>
        public string RestrictedTimeEnd
        {
            get { return GetColumnValue<string>(UserColumns.RestrictedTimeEnd); }
            set { SetColumnValue(UserColumns.RestrictedTimeEnd, value); }
        }

        /// <summary>
        /// BarcodePrefix
        /// </summary>
        public string BarcodePrefix
        {
            get { return GetColumnValue<string>(UserColumns.BarcodePrefix); }
            set { SetColumnValue(UserColumns.BarcodePrefix, value); }
        }

        /// <summary>
        /// AgeRestrictedItem
        /// </summary>
        public int AgeRestrictedItems
        {
            get { return GetColumnValue<int?>(UserColumns.AgeRestrictedItems).GetValueOrDefault(0); }
            set { SetColumnValue(UserColumns.AgeRestrictedItems, value); }
        }

        /// <summary>
        /// LastBarcodeGenerated
        /// </summary>
        public string LastBarcodeGenerated
        {
            get { return GetColumnValue<string>(UserColumns.LastBarcodeGenerated); }
            set { SetColumnValue(UserColumns.LastBarcodeGenerated, value); }
        }

        #endregion
    }
}
