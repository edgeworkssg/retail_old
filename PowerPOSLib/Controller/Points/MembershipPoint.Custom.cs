namespace PowerPOS
{
    public partial class MembershipPoint
    {
        public partial struct UserColumns
        {
            /// <summary>userfld1</summary>
            public static string PackageItemNo = @"userfld1";
            /// <summary>userfld2</summary>
            public static string PackageType = @"userfld2";
            /// <summary>userfloat1: Only applicable on Course</summary>
            public static string CourseBreakdownPrice = @"userfloat1";
        }

        #region Custom Properties
        public string PackageItemNo
        {
            get { return GetColumnValue<string>(UserColumns.PackageItemNo); }
            set { SetColumnValue(UserColumns.PackageItemNo, value); }
        }
        public string PackageType
        {
            get { return GetColumnValue<string>(UserColumns.PackageType); }
            set { SetColumnValue(UserColumns.PackageType, value); }
        }
        public decimal? CourseBreakdownPrice
        {
            get { return GetColumnValue<decimal?>(UserColumns.CourseBreakdownPrice); }
            set { SetColumnValue(UserColumns.CourseBreakdownPrice, value); }
        }
        #endregion
    }
}
