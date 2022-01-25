namespace PowerPOS
{
    public partial class OrderHdr
    {
        public partial struct UserColumns
        {
            /// <summary>Userfloat1</summary>
            public static string PointAmount = @"Userfloat1";
            /// <summary>Userfloat2</summary>
            public static string InitialPoint = @"Userfloat2";
            /// <summary>Userfloat3</summary>
            public static string PendingReward = @"Userfloat3";
            /// <summary>Userfloat4</summary>
            public static string TotalReward = @"Userfloat4";
        }

        #region Custom Properties
        public decimal? PointAmount
        {
            get { return GetColumnValue<decimal?>(UserColumns.PointAmount); }
            set { SetColumnValue(UserColumns.PointAmount, value); }
        }
        public decimal? InitialPoint
        {
            get { return GetColumnValue<decimal?>(UserColumns.InitialPoint); }
            set { SetColumnValue(UserColumns.InitialPoint, value); }
        }
        public decimal? PendingReward
        {
            get { return GetColumnValue<decimal?>(UserColumns.PendingReward); }
            set { SetColumnValue(UserColumns.PendingReward, value); }
        }
        public decimal? TotalReward
        {
            get { return GetColumnValue<decimal?>(UserColumns.TotalReward); }
            set { SetColumnValue(UserColumns.TotalReward, value); }
        }
        #endregion
    }
}
