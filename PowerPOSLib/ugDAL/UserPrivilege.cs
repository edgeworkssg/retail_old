using System.Text;
using SubSonic;
using System.Data;

namespace PowerPOS
{
    public partial class UserPrivilege
    {
        public partial struct UserColumns
        {
            /// <summary>Userfld1</summary>
            public static string IsSuperAdmin = @"Userflag1";

            /// <summary>Userflag2</summary>
            public static string IsRetail = @"Userflag2";

            /// <summary>Userflag3</summary>
            public static string IsWholesale = @"Userflag3";

            /// <summary>Userflag4</summary>
            public static string IsBeauty = @"Userflag4";

            /// <summary>Userfld1</summary>
            public static string PrivilegeCategory = @"Userfld1";

            /// <summary>Userint2</summary>
            public static string UpdatedFlag = @"Userint2";
        }

        #region Custom Properties

        /// <summary>
        /// IsSuperAdmin
        /// </summary>
        public bool IsSuperAdmin
        {
            get { return GetColumnValue<bool?>(UserColumns.IsSuperAdmin).GetValueOrDefault(false); }
            set { SetColumnValue(UserColumns.IsSuperAdmin, value); }
        }


        /// <summary>
        /// IsSuperAdmin
        /// </summary>
        public bool IsRetail
        {
            get { return GetColumnValue<bool?>(UserColumns.IsRetail).GetValueOrDefault(false); }
            set { SetColumnValue(UserColumns.IsRetail, value); }
        }

        /// <summary>
        /// IsSuperAdmin
        /// </summary>
        public bool IsWholesale
        {
            get { return GetColumnValue<bool?>(UserColumns.IsWholesale).GetValueOrDefault(false); }
            set { SetColumnValue(UserColumns.IsWholesale, value); }
        }

        /// <summary>
        /// IsSuperAdmin
        /// </summary>
        public bool IsBeauty
        {
            get { return GetColumnValue<bool?>(UserColumns.IsBeauty).GetValueOrDefault(false); }
            set { SetColumnValue(UserColumns.IsBeauty, value); }
        }

        /// <summary>
        /// POSType
        /// </summary>
        public string PrivilegeCategory
        {
            get { return GetColumnValue<string>(UserColumns.PrivilegeCategory); }
            set { SetColumnValue(UserColumns.PrivilegeCategory, value); }
        }

        /// <summary>
        /// UpdatedFlag (Userint2)
        /// </summary>
        public int UpdatedFlag
        {
            get { return GetColumnValue<int>(UserColumns.UpdatedFlag); }
            set { SetColumnValue(UserColumns.UpdatedFlag, value); }
        }
        #endregion
    }
}
