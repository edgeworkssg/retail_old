using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SubSonic;

namespace PowerPOS
{
    public partial class GuestBookController
    {
        public static DataTable GetGuestBookData(DateTime StartTime, DateTime EndTime, string Search)
        {
            if (string.IsNullOrEmpty(Search))
                Search = "%";
            string query = "select m.MembershipNo, m.NameToAppear, m.NRIC, ISNULL(m.Mobile,m.Home) as ContactNo, g.InTime, g.OutTime, m.Nationality, m.Remarks, g.OutletName " +
                          "from Guestbook g " +
                          "inner join membership m on g.membershipno = m.membershipno " +
                          "where isnull(g.deleted,0) = 0  and ISNULL(m.deleted,0) = 0 " +
                          "and cast(Intime as date) >= '" + StartTime.ToString("yyyy-MM-dd") + "' and cast(Intime as date) <= '" + EndTime.AddDays(1).ToString("yyyy-MM-dd") +"' " +
                          "and Isnull(m.membershipno,'') + ' ' + ISNULL(m.NameToAppear,'') + ' ' + ISNULL(g.OutletName,'') LIKE '%" + Search +"%'";
            
            DataSet ds = DataService.GetDataSet(new QueryCommand(query));

            return ds.Tables[0];
        }
    }
}
