using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic;

namespace PowerPOS
{
    public partial class PointTempLogController
    {
        /// <summary>
        /// Delete PointTempLog data where OrderHdrID already exists in PointAllocationLog
        /// </summary>
        public static void Clean()
        {
            string sql = "DELETE FROM PointTempLog WHERE OrderHdrID IN (SELECT OrderHdrID FROM PointAllocationLog)";
            DataService.ExecuteQuery(new QueryCommand(sql, "PowerPOS"));
        }
    }
}
