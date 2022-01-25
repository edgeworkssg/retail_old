using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PowerPOS
{
    public partial class StockTransferDet
    {
        public partial struct UserColumns
        {
            /// <summary>Userfoat1</summary>
            public static string GSTAmount = @"Userfloat1";

            /// <summary>Userint1</summary>
            public static string GSTRule = @"Userint1";
        }

        #region Custom Properties

        /// <summary>
        /// GSTAmount (Userfloat1)
        /// </summary>
        public decimal? GSTAmount
        {
            get { return GetColumnValue<decimal?>(UserColumns.GSTAmount); }
            set { SetColumnValue(UserColumns.GSTAmount, value); }
        }

        /// <summary>
        /// GSTRule (Userint1)
        /// </summary>
        public int? InventoryHdrRefNoTo
        {
            get { return GetColumnValue<int?>(UserColumns.GSTRule); }
            set { SetColumnValue(UserColumns.GSTRule, value); }
        }
        #endregion
    }
}
