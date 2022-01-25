using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic;

namespace PowerPOS
{
    public partial class InventoryLocationController
    {
        public static bool IsHaveLocationGroup(out string status) {
            status = "";
            try
            {
                string query = "Select ISNULL(count(*),0) as CountGroup from InventoryLocation WHERE ISNULL(Deleted,0) = 0 and ISNULL(Userint1,0) <> 0";
                int countGroup = (int)DataService.ExecuteScalar(new QueryCommand(query));

                string query2 = "Select ISNULL(count(*),0) as CountGroup from InventoryLocationGroup WHERE ISNULL(Deleted,0) = 0 ";
                int count2 = (int)DataService.ExecuteScalar(new QueryCommand(query2));

                return (countGroup > 0 && count2 > 0);
            }
            catch (Exception ex) {
                status = ex.Message;
                Logger.writeLog("Error Check Have LocationGrooup" + ex.Message);
                return false;
            }
        }
    }
}
