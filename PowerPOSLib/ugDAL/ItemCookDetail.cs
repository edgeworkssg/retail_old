using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PowerPOS
{
    public partial class ItemCookDetail
    {
        public partial struct UserColumns
        {
            /// <summary>Userflag1</summary>
            public static string IsLoadFromRecipe = @"Userflag1";
        }

        #region Custom Properties

        /// <summary>
        /// DocumentNo
        /// </summary>
        public bool IsLoadFromRecipe
        {
            get { return GetColumnValue<bool?>(UserColumns.IsLoadFromRecipe).GetValueOrDefault(false); }
            set { SetColumnValue(UserColumns.IsLoadFromRecipe, value); }
        }
        #endregion
    }
}
