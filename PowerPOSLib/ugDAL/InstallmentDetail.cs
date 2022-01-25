using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PowerPOS
{
    public partial class InstallmentDetail
    {
        public partial struct UserColumns
        {
            /// <summary>Userfld1</summary>
            public static string CustomRefNo = @"Userfld1";
        }

        #region Custom Properties

        /// <summary>
        /// CustomRefNo (Userfld1)
        /// </summary>
        public string CustomRefNo
        {
            get { return GetColumnValue<string>(UserColumns.CustomRefNo); }
            set { SetColumnValue(UserColumns.CustomRefNo, value); }
        }

        #endregion
    }
}
