namespace PowerPOS
{
    public partial class QuotationDet
    {
        public partial struct UserColumns
        {
            /// <summary>Userfld1</summary>
            public static string SalesPerson = @"Userfld1";
            /// <summary>Userfld4</summary>
            public static string LineInfo = @"Userfld4";
            /// <summary>Userfld8</summary>
            public static string DiscountDetail = @"Userfld8";
        }

        #region Custom Properties

        /// <summary>
        /// Line Sales Person for each OrderDet
        /// </summary>
        public string SalesPerson
        {
            get { return GetColumnValue<string>(UserColumns.SalesPerson); }
            set { SetColumnValue(UserColumns.SalesPerson, value); }
        }

        public string LineInfo
        {
            get { return GetColumnValue<string>(UserColumns.LineInfo); }
            set { SetColumnValue(UserColumns.LineInfo, value); }
        }

        public string DiscountDetail
        {
            get { return GetColumnValue<string>(UserColumns.DiscountDetail); }
            set { SetColumnValue(UserColumns.DiscountDetail, value); }
        }

        #endregion
    }
}
