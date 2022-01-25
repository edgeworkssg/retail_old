using System;
using System.Collections.Generic;
using System.Text;
using SubSonic;

namespace PowerPOS
{
    public partial class AttributesController
    {
        public static string getNewAttRefNo()
        {
            int runningNo = 0;
            int PointOfSaleID = 1;
            string Qry =
                "SELECT ISNULL(MAX(CAST(SUBSTRING(AttributesCode,2,50) AS INT)),'0') " +
                "FROM Attributes WHERE AttributesCode LIKE 'Z[0123456789]%'";
            QueryCommand Cmd = new QueryCommand(Qry);
            object obj = DataService.ExecuteScalar(Cmd);
            if (obj == null)
                return "Z00000001";
            else if (!"0123456789".Contains(obj.ToString()[0].ToString()))
                return "Z00000001";
            else
                return "Z" + ((int.Parse(obj.ToString())) + 1).ToString().PadLeft(8, '0');
        }
    }
}
