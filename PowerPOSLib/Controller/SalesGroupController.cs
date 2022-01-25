using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SubSonic;

namespace PowerPOSLib.Controller
{
    public partial class SalesGroupController
    {
        public DataTable FetchCommissionBaseOnPercentageList()
        {
            string strSql = "select Text = a.GroupName ";
            strSql += ", Value = a.GroupID ";
            strSql += "from UserGroup a ";
            strSql += "order by a.GroupName asc ";

            DataSet ds = DataService.GetDataSet(new QueryCommand(strSql, "PowerPOS"));
            //ds.Tables[0].Merge(ds.Tables[1]);
            return ds.Tables[0];
        }
    }
}
