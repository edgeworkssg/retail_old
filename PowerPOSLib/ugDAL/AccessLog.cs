namespace PowerPOS
{
    public partial class AccessLog
    {
        public partial struct UserColumns
        {
            /// <summary>Userfld1</summary>
            public static string TenantCode = @"Userfld1";
        }
        /// <summary>
        /// TenantCode
        /// </summary>
        public string TenantCode
        {
            get { return GetColumnValue<string>(UserColumns.TenantCode); }
            set { SetColumnValue(UserColumns.TenantCode, value); }
        }
    }
}
