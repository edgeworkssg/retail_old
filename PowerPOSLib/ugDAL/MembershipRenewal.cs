using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PowerPOS
{
    public partial class MembershipRenewal
    {
        public partial struct UserColumns
        {
            /// <summary>Userint1</summary>
            public static string NewMembershipGroupID = @"Userint1";
            public static string OldExpiryDate = @"UserFld1";
        }

        #region Custom Properties
        /// <summary>
        /// New Membership Group ID
        /// </summary>
        public int NewMembershipGroupID
        {
            get { return GetColumnValue<int>(UserColumns.NewMembershipGroupID); }
            set { SetColumnValue(UserColumns.NewMembershipGroupID, value); }
        }

        public DateTime OldExpiryDate       
        {
            get { return Convert.ToDateTime(GetColumnValue<string>(UserColumns.OldExpiryDate)); }
            set { SetColumnValue(UserColumns.OldExpiryDate, value.ToString("yyyy-MM-dd HH:mm:ss.fff")); }
        }
        #endregion

    }
}
