using System.Xml;
using System.Xml.Serialization;
using SubSonic;
using SubSonic.Utilities;
using System.ComponentModel;


namespace PowerPOS
{
    public partial class InventoryLocation
    {
        public partial struct UserColumns
        {
            /// <summary>Userfld1</summary>
            public static string DisplayedName = @"Userfld1";
            /// <summary>Userfld2</summary>
            public static string Address1 = @"Userfld2";
            /// <summary>Userfld3</summary>
            public static string Address2 = @"Userfld3";
            /// <summary>Userfld4</summary>
            public static string Address3 = @"Userfld4";
            /// <summary>Userfld5</summary>
            public static string City = @"Userfld5";
            /// <summary>Userfld6</summary>
            public static string Country = @"Userfld6";
            /// <summary>Userfld7</summary>
            public static string PostalCode = @"Userfld7";
            /// <summary>Userfld8</summary>
            public static string DefaultPriceLevel = @"Userfld8";
            /// <summary>Userflag1</summary>
            public static string IsFrozen = @"Userflag1";
            /// <summary>Userflag2</summary>
            //public static string IsAvailableForIntegration = @"Userflag2"; -- used in Retail trunk, put here to prevent it from being used again
            /// <summary>Userflag3</summary>
            public static string DontUpdateItemAvgCostPrice = @"Userflag3";
            /// <summary>Userint1</summary>
            public static string InventoryLocationGroupID = @"Userint1";
        }

        #region Custom Properties

        /// <summary>
        /// InventoryLocationGroupID
        /// </summary>
        public int InventoryLocationGroupID
        {
            get { return GetColumnValue<int?>(UserColumns.InventoryLocationGroupID).GetValueOrDefault(0); }
            set { SetColumnValue(UserColumns.InventoryLocationGroupID, value); }
        }

        /// <summary>
        /// DisplayedName
        /// </summary>
        public string DisplayedName
        {
            get { return GetColumnValue<string>(UserColumns.DisplayedName); }
            set { SetColumnValue(UserColumns.DisplayedName, value); }
        }

        /// <summary>
        /// Address1
        /// </summary>
        public string Address1
        {
            get { return GetColumnValue<string>(UserColumns.Address1); }
            set { SetColumnValue(UserColumns.Address1, value); }
        }

        /// <summary>
        /// Address2
        /// </summary>
        public string Address2
        {
            get { return GetColumnValue<string>(UserColumns.Address2); }
            set { SetColumnValue(UserColumns.Address2, value); }
        }

        /// <summary>
        /// Address3
        /// </summary>
        public string Address3
        {
            get { return GetColumnValue<string>(UserColumns.Address3); }
            set { SetColumnValue(UserColumns.Address3, value); }
        }

        /// <summary>
        /// City
        /// </summary>
        public string City
        {
            get { return GetColumnValue<string>(UserColumns.City); }
            set { SetColumnValue(UserColumns.City, value); }
        }

        /// <summary>
        /// Country
        /// </summary>
        public string Country
        {
            get { return GetColumnValue<string>(UserColumns.Country); }
            set { SetColumnValue(UserColumns.Country, value); }
        }

        /// <summary>
        /// PostalCode
        /// </summary>
        public string PostalCode
        {
            get { return GetColumnValue<string>(UserColumns.PostalCode); }
            set { SetColumnValue(UserColumns.PostalCode, value); }
        }

        /// <summary>
        /// DefaultPriceLevel (Userfld8)
        /// </summary>
        public string DefaultPriceLevel
        {
            get { return GetColumnValue<string>(UserColumns.DefaultPriceLevel); }
            set { SetColumnValue(UserColumns.DefaultPriceLevel, value); }
        }

        /// <summary>
        /// IsFrozen
        /// </summary>
        public bool IsFrozen
        {
            get { return GetColumnValue<bool>(UserColumns.IsFrozen); }
            set { SetColumnValue(UserColumns.IsFrozen, value); }
        }

        /// <summary>
        /// DontUpdateItemAvgCostPrice (Userflag3)
        /// </summary>
        public bool? DontUpdateItemAvgCostPrice
        {
            get { return GetColumnValue<bool?>(UserColumns.DontUpdateItemAvgCostPrice); }
            set { SetColumnValue(UserColumns.DontUpdateItemAvgCostPrice, value); }
        }
        #endregion
    }
}
