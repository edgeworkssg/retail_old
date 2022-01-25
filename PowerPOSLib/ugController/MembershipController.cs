using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SubSonic;

/*** Custom Part ***/

namespace PowerPOS
{
    public partial class MembershipController
    {
        public static DataTable FetchAll_ListBoxFormat(string DisplayedColumnName, string ValueColumnName)
        {
            Query Qry = Membership.CreateQuery();
            Qry.SelectList = DisplayedColumnName + "," + ValueColumnName;
            Qry.QueryType = QueryType.Select;

            DataTable Output = new DataTable();
            Output.Load(Qry.ExecuteReader());

            return Output;
        }
    }
}
