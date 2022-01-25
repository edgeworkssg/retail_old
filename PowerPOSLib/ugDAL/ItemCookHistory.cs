using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic;

namespace PowerPOS
{
    public partial class ItemCookHistory
    {
        public partial struct UserColumns
        {
            /// <summary>Userfld1</summary>
            public static string DocumentNo = @"Userfld1";

            /// <summary>Userfld2</summary>
            public static string Status = @"Userfld2";

            /// <summary>Userfloat1</summary>
            public static string COG = @"Userfloat1";

            
        }

        #region Custom Properties

        /// <summary>
        /// DocumentNo
        /// </summary>
        public string DocumentNo
        {
            get { return GetColumnValue<string>(UserColumns.DocumentNo); }
            set { SetColumnValue(UserColumns.DocumentNo, value); }
        }


        /// <summary>
        /// Status
        /// </summary>
        public string Status
        {
            get { return GetColumnValue<string>(UserColumns.Status); }
            set { SetColumnValue(UserColumns.Status, value); }
        }

        /// <summary>
        /// COG
        /// </summary>
        public decimal COG
        {
            get { return GetColumnValue<decimal>(UserColumns.COG); }
            set { SetColumnValue(UserColumns.COG, value); }
        }

        #endregion
    }
}
