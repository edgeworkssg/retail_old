using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PowerPOS
{
    public partial class FileAttachment
    {
        public partial struct UserColumns
        {
            /// <summary>Userfld1</summary>
            public static string SizeInText = @"Userfld1";
        }

        #region *) Custom Properties

        /// <summary>
        /// UOM
        /// </summary>
        public string SizeInText
        {
            get { return GetColumnValue<string>(UserColumns.SizeInText); }
            set { SetColumnValue(UserColumns.SizeInText, value); }
        }

        #endregion
    }
}
