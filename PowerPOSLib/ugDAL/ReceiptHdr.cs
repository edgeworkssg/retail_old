using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PowerPOS
{
    public partial class ReceiptHdr
    {
        public partial struct UserColumns
        {
            /// <summary>userfld1</summary>
            public static string DeliveryDate = @"userfld1";
            /// <summary>userfld2</summary>
            public static string DeliveryTime = @"userfld2";
        }

        #region Custom Properties

        /// <summary>
        /// DeliveryDate (userfld1)
        /// </summary>
        public string DeliveryDate
        {
            get { return GetColumnValue<string>(UserColumns.DeliveryDate); }
            set { SetColumnValue(UserColumns.DeliveryDate, value); }
        }
        /// <summary>
        /// DeliveryTime (userfld2)
        /// </summary>
        public string DeliveryTime
        {
            get { return GetColumnValue<string>(UserColumns.DeliveryTime); }
            set { SetColumnValue(UserColumns.DeliveryTime, value); }
        }

        #endregion
    }
}
