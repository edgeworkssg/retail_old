namespace PowerPOS
{
    public partial class PointAllocationLog
    {
        public partial struct UserColumns
        {
            /// <summary>userfld1</summary>
            public static string PointItemNo = @"userfld1";
            /// <summary>userfld2</summary>
            public static string SalesPerson = @"userfld2";
        }

        #region Custom Properties
        public string PointItemNo
        {
            get { return GetColumnValue<string>(UserColumns.PointItemNo); }
            set { SetColumnValue(UserColumns.PointItemNo, value); }
        }
        public string SalesPerson
        {
            get { return GetColumnValue<string>(UserColumns.SalesPerson); }
            set { SetColumnValue(UserColumns.SalesPerson, value); }
        }
        #endregion
    }
}
