using SubSonic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PowerPOS.Factory
{
    public partial class Membership
    {
        public static PowerPOS.Membership GetMember(string Search
            , bool SearchMembershipNo, bool SearchNRIC, bool SearchMobileNo)
        {
            string SQL;

            if (Search == null || Search.Trim() == "")
                throw new Exception("(pause)");

            PowerPOS.Membership Rst;

            #region*) Fetch: Search by Membership No
            if (SearchMembershipNo)
            {
                SQL = "SELECT * FROM Membership WHERE MembershipNo = '" + Search + "' AND (Deleted IS NULL OR Deleted = 0)";
              
                Rst = new PowerPOS.Membership();
                Rst.LoadAndCloseReader(DataService.GetReader(new QueryCommand(SQL)));
                
                if (Rst != null && !Rst.IsNew)
                    return Rst;
            }
            #endregion

            #region *) Fetch: Search by NRIC
            if (SearchNRIC)
            {
                SQL = "SELECT * FROM Membership WHERE NRIC = '" + Search + "' AND (Deleted IS NULL OR Deleted = 0)";
               
                Rst = new PowerPOS.Membership();
                Rst.LoadAndCloseReader(DataService.GetReader(new QueryCommand(SQL)));

                if (Rst != null && !Rst.IsNew)
                    return Rst;
            }
            #endregion

            #region *) Fetch: Search by Mobile No
            if (SearchMobileNo)
            {
                SQL = "SELECT * FROM Membership WHERE Mobile = '" + Search + "' AND (Deleted IS NULL OR Deleted = 0)";

                Rst = new PowerPOS.Membership();
                Rst.LoadAndCloseReader(DataService.GetReader(new QueryCommand(SQL)));

                if (Rst != null && !Rst.IsNew)
                    return Rst;
            }
            #endregion

            throw new Exception("(warning)Cannot find member");
        }
    }
}
