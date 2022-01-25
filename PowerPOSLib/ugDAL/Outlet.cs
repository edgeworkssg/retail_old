using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PowerPOS
{
    public partial class Outlet
    {
        public partial struct UserColumns
        {
            /// <summary>Userfld1</summary>
            public static string MallCode = @"Userfld1";

            /// <summary>Userfld1</summary>
            public static string PrefixMembership = @"Userfld2";
        }

        #region Custom Properties
        /// <summary>
        /// Mall Code
        /// </summary>
        public string MallCode
        {
            get { return GetColumnValue<string>(UserColumns.MallCode); }
            set { SetColumnValue(UserColumns.MallCode, value); }
        }

        /// <summary>
        /// Prefix Membership
        /// </summary>
        public string PrefixMembership
        {
            get { return GetColumnValue<string>(UserColumns.PrefixMembership); }
            set { SetColumnValue(UserColumns.PrefixMembership, value); }
        }

        #endregion
    }
}
