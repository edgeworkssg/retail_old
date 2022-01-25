using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PowerPOS
{
    public partial class RetailerLevel
    {
        public partial struct UserColumns
        {
            /// <summary>Userfld1</summary>
            public static string MallName = @"Userfld1";

            /// <summary>Userfld2</summary>
            public static string ShopArea = @"Userfld2";

            /// <summary>Userfld3</summary>
            public static string Attribute1 = @"Userfld3";

            /// <summary>Userfld4</summary>
            public static string Attribute2 = @"Userfld4";
        }

        #region Custom Properties
        /// <summary>
        /// Mall Name
        /// </summary>
        public string MallName
        {
            get { return GetColumnValue<string>(UserColumns.MallName); }
            set { SetColumnValue(UserColumns.MallName, value); }
        }

        /// <summary>
        /// ShopArea
        /// </summary>
        public string ShopArea
        {
            get { return GetColumnValue<string>(UserColumns.ShopArea); }
            set { SetColumnValue(UserColumns.ShopArea, value); }
        }

        /// <summary>
        /// Attribute1
        /// </summary>
        public string Attribute1
        {
            get { return GetColumnValue<string>(UserColumns.Attribute1); }
            set { SetColumnValue(UserColumns.Attribute1, value); }
        }

        /// <summary>
        /// Attribute2
        /// </summary>
        public string Attribute2
        {
            get { return GetColumnValue<string>(UserColumns.Attribute2); }
            set { SetColumnValue(UserColumns.Attribute2, value); }
        }

        #endregion
    }
}
