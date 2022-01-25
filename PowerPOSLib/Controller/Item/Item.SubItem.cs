namespace PowerPOS
{
    public partial class Item
    {
        public partial struct UserColumns
        {
            /// <summary>Userflag1</summary>
            public static string hasSubItem = @"Userflag1";
        }

        #region Custom Properties
        public bool? hasSubItem
        {
            get { return GetColumnValue<bool?>(UserColumns.hasSubItem); }
            set { SetColumnValue(UserColumns.hasSubItem, value.GetValueOrDefault(false)); }
        }
        #endregion
    }
}
