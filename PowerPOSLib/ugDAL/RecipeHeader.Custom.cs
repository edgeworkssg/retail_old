using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic;
using System.Xml.Serialization;

namespace PowerPOS
{
    public partial class RecipeHeader
    {
        public partial struct UserColumns
        {
            /// <summary>userfld1</summary>
            public static string Type = @"userfld1";
        }

        #region Custom Properties

        [XmlAttribute("Type")]
        public string Type
        {
            get { return GetColumnValue<string>(UserColumns.Type); }
            set { SetColumnValue(UserColumns.Type, value); }
        }


        #endregion
    }
}
