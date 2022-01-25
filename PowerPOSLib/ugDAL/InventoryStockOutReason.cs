using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PowerPOS
{
    public partial class InventoryStockOutReason
    {
        public struct UserColumns
        {
            /// <summary>Userfld1</summary>
            public static string ReasonType = @"Userfld1";
        }

        #region Custom Properties

        /// <summary>
        /// ReasonType
        /// </summary>
        public string ReasonType
        {
            get { return GetColumnValue<string>(UserColumns.ReasonType); }
            set { SetColumnValue(UserColumns.ReasonType, value); }
        }
        #endregion
    }
}
