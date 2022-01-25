namespace PowerPOS
{
    public partial class CounterCloseLog
    {
        public partial struct UserColumns
        {
            /// <summary>Userfld3</summary>
            public static string Pay5Recorded = @"Userfld3";
            /// <summary>Userfld4</summary>
            public static string Pay5Collected = @"Userfld4";
            /// <summary>Userfld5</summary>
            public static string Pay6Recorded = @"Userfld5";
            /// <summary>Userfld6</summary>
            public static string Pay6Collected = @"Userfld6";

            /// <summary>Userfloat1</summary>
            public static string ChequeRecorded = @"Userfloat1";
            /// <summary>Userfloat2</summary>
            public static string ChequeCollected = @"Userfloat2";
            /// <summary>Userfloat3</summary>
            public static string PointRecorded = @"Userfloat3";
            /// <summary>Userfloat4</summary>
            public static string PackageRecorded = @"Userfloat4";

            // NOTE: Userfloat5 is used for installment amount (see PrintController.GetCounterClose())

            /// <summary>Userfloat6</summary>
            public static string SMFRecorded = @"Userfloat6";
            /// <summary>Userfloat7</summary>
            public static string SMFCollected = @"Userfloat7";
            /// <summary>Userfloat8</summary>
            public static string PAMedRecorded = @"Userfloat8";
            /// <summary>Userfloat9</summary>
            public static string PAMedCollected = @"Userfloat9";
            /// <summary>Userfloat10</summary>
            public static string PWFRecorded = @"Userfloat10";
            /// <summary>Userfloat11</summary>
            public static string PWFCollected = @"Userfloat11";
        }

        #region Custom Properties
        /// <summary>Userfld3</summary>
        public decimal? Pay5Recorded
        {
            get
            {
                string s = GetColumnValue<string>(UserColumns.Pay5Recorded);
                decimal tmp = 0.0M;
                if (decimal.TryParse(s, out tmp))
                {
                    return tmp;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (value != null && value.ToString() != "")
                {
                    SetColumnValue(UserColumns.Pay5Recorded, value.ToString());
                }
                else
                {
                    SetColumnValue(UserColumns.Pay5Recorded, null);
                }
            }
        }
        /// <summary>Userfld4</summary>
        public decimal? Pay5Collected
        {
            get
            {
                string s = GetColumnValue<string>(UserColumns.Pay5Collected);
                decimal tmp = 0.0M;
                if (decimal.TryParse(s, out tmp))
                {
                    return tmp;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (value != null && value.ToString() != "")
                {
                    SetColumnValue(UserColumns.Pay5Collected, value.ToString());
                }
                else
                {
                    SetColumnValue(UserColumns.Pay5Collected, null);
                }
            }
        }
        /// <summary>Userfld5</summary>
        public decimal? Pay6Recorded
        {
            get
            {
                string s = GetColumnValue<string>(UserColumns.Pay6Recorded);
                decimal tmp = 0.0M;
                if (decimal.TryParse(s, out tmp))
                {
                    return tmp;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (value != null && value.ToString() != "")
                {
                    SetColumnValue(UserColumns.Pay6Recorded, value.ToString());
                }
                else
                {
                    SetColumnValue(UserColumns.Pay6Recorded, null);
                }
            }
        }
        /// <summary>Userfld6</summary>
        public decimal? Pay6Collected
        {
            get
            {
                string s = GetColumnValue<string>(UserColumns.Pay6Collected);
                decimal tmp = 0.0M;
                if (decimal.TryParse(s, out tmp))
                {
                    return tmp;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (value != null && value.ToString() != "")
                {
                    SetColumnValue(UserColumns.Pay6Collected, value.ToString());
                }
                else
                {
                    SetColumnValue(UserColumns.Pay6Collected, null);
                }
            }
        }
        /// <summary>Userfloat1</summary>
        public decimal? ChequeRecorded
        {
            get { return GetColumnValue<decimal?>(UserColumns.ChequeRecorded); }
            set { SetColumnValue(UserColumns.ChequeRecorded, value); }
        }
        /// <summary>Userfloat2</summary>
        public decimal? ChequeCollected
        {
            get { return GetColumnValue<decimal?>(UserColumns.ChequeCollected); }
            set { SetColumnValue(UserColumns.ChequeCollected, value); }
        }
        /// <summary>Userfloat3</summary>
        public decimal? PointRecorded
        {
            get { return GetColumnValue<decimal?>(UserColumns.PointRecorded); }
            set { SetColumnValue(UserColumns.PointRecorded, value); }
        }
        /// <summary>Userfloat4</summary>
        public decimal? PackageRecorded
        {
            get { return GetColumnValue<decimal?>(UserColumns.PackageRecorded); }
            set { SetColumnValue(UserColumns.PackageRecorded, value); }
        }
        /// <summary>Userfloat6</summary>
        public decimal? SMFRecorded
        {
            get { return GetColumnValue<decimal?>(UserColumns.SMFRecorded); }
            set { SetColumnValue(UserColumns.SMFRecorded, value); }
        }
        /// <summary>Userfloat7</summary>
        public decimal? SMFCollected
        {
            get { return GetColumnValue<decimal?>(UserColumns.SMFCollected); }
            set { SetColumnValue(UserColumns.SMFCollected, value); }
        }
        /// <summary>Userfloat8</summary>
        public decimal? PAMedRecorded
        {
            get { return GetColumnValue<decimal?>(UserColumns.PAMedRecorded); }
            set { SetColumnValue(UserColumns.PAMedRecorded, value); }
        }
        /// <summary>Userfloat9</summary>
        public decimal? PAMedCollected
        {
            get { return GetColumnValue<decimal?>(UserColumns.PAMedCollected); }
            set { SetColumnValue(UserColumns.PAMedCollected, value); }
        }
        /// <summary>Userfloat10</summary>
        public decimal? PWFRecorded
        {
            get { return GetColumnValue<decimal?>(UserColumns.PWFRecorded); }
            set { SetColumnValue(UserColumns.PWFRecorded, value); }
        }
        /// <summary>Userfloat11</summary>
        public decimal? PWFCollected
        {
            get { return GetColumnValue<decimal?>(UserColumns.PWFCollected); }
            set { SetColumnValue(UserColumns.PWFCollected, value); }
        }
        #endregion
    }
}
