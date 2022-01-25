using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PowerPOS
{
    public partial class OutletGroupItemMap
    {
        public partial struct UserColumns
        {
            /// <summary>Userfld1</summary>
            public static string ItemName = @"Userfld1";
        }

        #region Custom Properties

        public string ItemName
        {
            get
            {
                return GetColumnValue<string>(UserColumns.ItemName);                
            }
            set
            {
                SetColumnValue(UserColumns.ItemName, value);
            }
        }

        #endregion
    }
}
