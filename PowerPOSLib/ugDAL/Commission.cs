namespace PowerPOS
{
    public partial class Commission
    {
        public partial struct UserColumns
        {
            /// <summary>Userflag1</summary>
            public static string Portion2ndSalesPerson = @"Userflag1";
            /// <summary>Userflag2</summary>
            public static string DeductAmountFromFirstSalesPerson = @"Userflag2";

            /// <summary>Userfld1</summary>
            public static string PortionMode = @"Userfld1";

            /// <summary>Userfloat1</summary>
            public static string PortionPercentage = @"Userfloat1";
            /// <summary>Userfloat2</summary>
            public static string PortionFixedAmount = @"Userfloat2";
        }

        #region Custom Properties
        /// <summary>
        /// Portion2ndSalesPerson
        /// </summary>
        public bool Portion2ndSalesPerson
        {
            get { return GetColumnValue<bool?>(UserColumns.Portion2ndSalesPerson).GetValueOrDefault(false); }
            set { SetColumnValue(UserColumns.Portion2ndSalesPerson, value); }
        }

        /// <summary>
        /// DeductAmountFromFirstSalesPerson
        /// </summary>
        public bool DeductAmountFromFirstSalesPerson
        {
            get { return GetColumnValue<bool?>(UserColumns.DeductAmountFromFirstSalesPerson).GetValueOrDefault(false); }
            set { SetColumnValue(UserColumns.DeductAmountFromFirstSalesPerson, value); }
        }

        /// <summary>
        /// PortionMode
        /// </summary>
        public string PortionMode
        {
            get { return GetColumnValue<string>(UserColumns.PortionMode); }
            set { SetColumnValue(UserColumns.PortionMode, value); }
        }

        /// <summary>
        /// Percentage
        /// </summary>
        public decimal? PortionPercentage
        {
            get { return GetColumnValue<decimal?>(UserColumns.PortionPercentage); }
            set { SetColumnValue(UserColumns.PortionPercentage, value); }
        }

        /// <summary>
        /// Percentage
        /// </summary>
        public decimal? PortionFixedAmount
        {
            get { return GetColumnValue<decimal?>(UserColumns.PortionFixedAmount); }
            set { SetColumnValue(UserColumns.PortionFixedAmount, value); }
        }

        #endregion
    }
}
