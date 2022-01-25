using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic;

namespace PowerPOS
{
    public partial class AuditLogController
    {
        public static void AddLog(string operation, string tableName, string primaryKeyCol, string primaryKeyVal, 
                                  string oldValues, string newValues, string remarks)
        {
            AuditLog log = new AuditLog();
            log.AuditLogID = Guid.NewGuid();
            log.LogDate = DateTime.Now;
            log.Operation = operation;
            log.TableName = tableName;
            log.PrimaryKeyCol = primaryKeyCol;
            log.PrimaryKeyVal = primaryKeyVal;
            log.OldValues = oldValues;
            log.NewValues = newValues;
            log.Remarks = remarks;
            log.Save();
        }
    }
}
