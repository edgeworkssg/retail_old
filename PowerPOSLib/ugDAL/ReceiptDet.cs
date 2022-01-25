namespace PowerPOS
{
    public partial class ReceiptDet
    {
        public partial struct UserColumns
        {
            /// <summary>Userfloat1</summary>
            public static string ExtraChargeAmount = @"Userfloat1";

            /// <summary>Userfloat2</summary>
            public static string ForeignCurrencyRate = @"Userfloat2";

            /// <summary>Userfloat3</summary>
            public static string ForeignAmountReceived = @"Userfloat3";

            /// <summary>Userfloat4</summary>
            public static string ForeignAmountReturned = @"Userfloat4";

            /// <summary>Userfdl1</summary>
            public static string ForeignCurrencyCode = @"userfld1";

            // userfld2 = BankName

            /// <summary>userfld3</summary>
            public static string NETSResponseInfo = @"userfld3";
        }

        #region Custom Properties
        /// <summary>
        /// Amount of extra charge inclusive in this payment
        /// </summary>
        public decimal? ExtraChargeAmount
        {
            get { return GetColumnValue<decimal?>(UserColumns.ExtraChargeAmount); }
            set { SetColumnValue(UserColumns.ExtraChargeAmount, value); }
        }

        /// <summary>
        /// Foreign Currency Rate
        /// </summary>
        public decimal? ForeignCurrencyRate
        {
            get { return GetColumnValue<decimal?>(UserColumns.ForeignCurrencyRate); }
            set { SetColumnValue(UserColumns.ForeignCurrencyRate, value); }
        }

        /// <summary>
        /// Foreign Amount Received
        /// </summary>
        public decimal? ForeignAmountReceived
        {
            get { return GetColumnValue<decimal?>(UserColumns.ForeignAmountReceived); }
            set { SetColumnValue(UserColumns.ForeignAmountReceived, value); }
        }

        /// <summary>
        /// Foreign Amount Returned
        /// </summary>
        public decimal? ForeignAmountReturned
        {
            get { return GetColumnValue<decimal?>(UserColumns.ForeignAmountReturned); }
            set { SetColumnValue(UserColumns.ForeignAmountReturned, value); }
        }

        /// <summary>
        /// Foreign Currency Code
        /// </summary>
        public string ForeignCurrencyCode
        {
            get { return GetColumnValue<string>(UserColumns.ForeignCurrencyCode); }
            set { SetColumnValue(UserColumns.ForeignCurrencyCode, value); }
        }

        /// <summary>
        /// NETS Response Info
        /// </summary>
        public string NETSResponseInfo
        {
            get { return GetColumnValue<string>(UserColumns.NETSResponseInfo); }
            set { SetColumnValue(UserColumns.NETSResponseInfo, value); }
        }
        #endregion
    }
}