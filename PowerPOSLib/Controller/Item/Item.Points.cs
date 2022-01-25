namespace PowerPOS
{
    public partial class Item
    {
        public struct PointMode
        {
            public static string Dollar = "D";
            public static string Times = "T";
            public static string None = "N";
        }

        public partial struct UserColumns
        {
            /// <summary>Userfld9</summary>
            public static string PointRedeemMode = @"Userfld9";
            /// <summary>Userfld10</summary>
            public static string PointGetMode = @"Userfld10";
            /// <summary>Userfloat1</summary>
            public static string PointGetAmount = @"Userfloat1";
            /// <summary>Userfloat2</summary>
            public static string PointRedeemAmount = @"Userfloat2";
            /// <summary>Userfloat3</summary>
            public static string PointUnitPrice = @"Userfloat3";
        }

        #region Custom Properties
        public string PointRedeemMode
        {
            get { return GetColumnValue<string>(UserColumns.PointRedeemMode); }
            set { SetColumnValue(UserColumns.PointRedeemMode, value); }
        }
        public string PointGetMode
        {
            get { return GetColumnValue<string>(UserColumns.PointGetMode); }
            set { SetColumnValue(UserColumns.PointGetMode, value); }
        }

        /// <summary>
        /// UserFloat2; Use 0 to refer to RetailPrice
        /// </summary>
        public decimal PointRedeemAmount
        {
            get { return GetColumnValue<decimal?>(UserColumns.PointRedeemAmount).GetValueOrDefault(0); }
            set
            {
                if (value == 0)
                    SetColumnValue(UserColumns.PointRedeemAmount, null);
                else
                    SetColumnValue(UserColumns.PointRedeemAmount, value);
            }
        }

        /// <summary>
        /// UserFloat1; Use 0 to refer to RetailPrice
        /// </summary>
        public decimal PointGetAmount
        {
            get { return GetColumnValue<decimal?>(UserColumns.PointGetAmount).GetValueOrDefault(0); }
            set
            {
                if (value == 0)
                    SetColumnValue(UserColumns.PointGetAmount, null);
                else
                    SetColumnValue(UserColumns.PointGetAmount, value);
            }
        }

        public decimal? PointUnitPrice
        {
            get { return GetColumnValue<decimal?>(UserColumns.PointUnitPrice); }
            set { SetColumnValue(UserColumns.PointUnitPrice, value); }
        }
        #endregion
    }
}
