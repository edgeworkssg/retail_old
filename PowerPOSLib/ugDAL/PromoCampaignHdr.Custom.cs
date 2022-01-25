using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic;
using System.Data;

namespace PowerPOS
{
    public partial class PromoCampaignHdr
    {

        public partial struct UserColumns
        {
            /// <summary>Userint1</summary>
            public static string MembershipGroupID = @"Userfld1";
        }

        #region Custom Properties
        /// <summary>
        /// ListMembershipGroupID
        /// </summary>
        public string MembershipGroupID
        {
            get { return GetColumnValue<string>(UserColumns.MembershipGroupID); }
            set { SetColumnValue(UserColumns.MembershipGroupID, value); }
        }

        public List<MembershipGroup> ListMembershipGroup
        {
            get
            {
                var result = new List<MembershipGroup>();

                try
                {
                    var groupID = MembershipGroupID + "";

                    string sql = "SELECT * FROM MembershipGroup WHERE Deleted = 0";

                    if (!string.IsNullOrEmpty(groupID) && !groupID.ToUpper().Equals("0"))
                    {
                        var listID = new List<string>();
                        foreach (var item in groupID.Split(','))
                        {
                            if (item.GetIntValue() != 0 && !listID.Contains(item.GetIntValue()+""))
                                listID.Add(item.GetIntValue() + "");
                        }
                        sql += string.Format(" AND MembershipGroupID IN ('{0}')", string.Join("','", listID.ToArray()));
                    }
                    DataTable dt = new DataTable();
                    dt.Load(DataService.GetReader(new QueryCommand(sql)));
                    MembershipGroupCollection mColl = new MembershipGroupCollection();
                    mColl.Load(dt);
                    result = mColl.ToList();
                }
                catch (Exception ex)
                {
                    Logger.writeLog(ex);
                }

                return result;
            }
        }

        #endregion    
    }
}
