using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Xml;
using System.Xml.Serialization;
using SubSonic;
using SubSonic.Utilities;

namespace PowerPOS
{
    public partial class UserMstController
    {
        public struct ListBoxColumns
        {
            public static string DisplayedColumnName = "Displayed";
            public static string ValueColumnName = "Value";
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public UserMstCollection FetchSalesPerson()
        {
            UserMstCollection Output = new UserMstCollection()
                .Where(UserMst.Columns.IsASalesPerson, true).Load();
            return Output;
        }

        public DataTable FetchSalesPerson_forListView()
        {
            DataTable Output = new UserMstCollection().Where(UserMst.Columns.IsASalesPerson, true).Load().ToDataTable();

            for (int Counter = Output.Columns.Count - 1; Counter >= 0; Counter--)
                if (Output.Columns[Counter].ColumnName.ToLower().CompareTo(UserMst.Columns.UserName.ToLower()) != 0 && Output.Columns[Counter].ColumnName.ToLower().CompareTo(UserMst.Columns.DisplayName.ToLower()) != 0)
                    Output.Columns.RemoveAt(Counter);

            Output.Columns[UserMst.Columns.UserName].ColumnName = ListBoxColumns.ValueColumnName;
            Output.Columns[UserMst.Columns.DisplayName].ColumnName = ListBoxColumns.DisplayedColumnName;

            return Output;
        }
    }
}
