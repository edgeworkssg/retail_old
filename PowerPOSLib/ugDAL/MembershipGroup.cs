using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic;
using System.Data;

namespace PowerPOS
{
    public partial class MembershipGroup
    {
        public partial struct UserColumns
        {
            /// <summary>userfloat1</summary>
            public static string PointsPercentage = @"userfloat1";

            /// <summary>userfloat2</summary>
            public static string SpendingLimit = @"userfloat2";

            /// <summary>userfld1</summary>
            public static string PriceTier = @"userfld1";
        }

        #region Custom Properties
        /// <summary>userfloat1</summary>
        public decimal PointsPercentage
        {
            get { return GetColumnValue<decimal?>(UserColumns.PointsPercentage).GetValueOrDefault(0); }
            set { SetColumnValue(UserColumns.PointsPercentage, value); }
        }
        public decimal SpendingLimit
        {
            get { return GetColumnValue<decimal?>(UserColumns.SpendingLimit).GetValueOrDefault(0); }
            set { SetColumnValue(UserColumns.SpendingLimit, value); }
        }
        public bool isExceedLimit(decimal value)
        {
            bool exceed = false;
            if (value >= Convert.ToDecimal(this.Userfloat2))
            {
                exceed = true;
            }
            return exceed;

        }

        public string PriceTier
        {
            get { return GetColumnValue<string>(UserColumns.PriceTier); }
            set { SetColumnValue(UserColumns.PriceTier, value); }
        }
        #endregion

    }
}
