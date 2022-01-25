using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PowerPOS
{
    public partial class PointOfSale
    {
        public partial struct UserColumns
        {
            /// <summary>Userfld4</summary>
            public static string PriceSchemeID = @"Userfld4";

            /// <summary>Userfld10</summary>
            public static string LinkedMembershipNo = @"Userfld10";
        }

        /// <summary>
        /// PriceSchemeID
        /// </summary>
        public string PriceSchemeID
        {
            get { return GetColumnValue<string>(UserColumns.PriceSchemeID); }
            set { SetColumnValue(UserColumns.PriceSchemeID, value); }
        }

        public string LinkedMembershipNo
        {
            get { return GetColumnValue<string>(UserColumns.LinkedMembershipNo); }
            set { SetColumnValue(UserColumns.LinkedMembershipNo, value); }
        }
    }
}
