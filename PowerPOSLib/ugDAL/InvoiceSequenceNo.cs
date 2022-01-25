using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PowerPOS
{
    public partial class InvoiceSequenceNo
    {
        public struct UserColumns
        {
            /// <summary>Userfld1</summary>
            public static string IsCreditNote = @"Userflag1";
        }

        #region Custom Properties

        /// <summary>
        /// ReasonType
        /// </summary>
        public bool IsCreditNote
        {
            get { return GetColumnValue<bool>(UserColumns.IsCreditNote); }
            set { SetColumnValue(UserColumns.IsCreditNote, value); }
        }
        #endregion
    }
}
