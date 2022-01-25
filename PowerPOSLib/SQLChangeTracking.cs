using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SubSonic;
using System.Collections;

namespace PowerPOS
{
    // SQLChangeTracking v 1.3 (9 April 2013)
    public class SQLChangeTracking
    {
        // Will be using the current provider of Subsonic
        static string providerName = "PowerPOS";

        public static bool EnableChangeTracking()
        {
            try
            {
                // Get DB Name
                QueryCommand getDBName = new QueryCommand("SELECT DB_NAME()", providerName);
                object resultDBName = DataService.ExecuteScalar(getDBName);

                // Alter DB
                QueryCommand enableCT = new QueryCommand("ALTER DATABASE " + resultDBName.ToString() + " " +
                    "SET CHANGE_TRACKING = ON (AUTO_CLEANUP = ON, CHANGE_RETENTION = 14 DAYS); ", providerName);

                DataService.ExecuteQuery(enableCT);

                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return false;
            }
        }

        public static bool DisableChangeTracking()
        {
            try
            {
                // Get DB Name
                QueryCommand getDBName = new QueryCommand("SELECT DB_NAME()", providerName);
                object resultDBName = DataService.ExecuteScalar(getDBName);

                // Alter DB
                QueryCommand enableCT = new QueryCommand("ALTER DATABASE " + resultDBName.ToString() + " " +
                    "SET CHANGE_TRACKING = OFF; ", providerName);

                DataService.ExecuteQuery(enableCT);

                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return false;
            }
        }

        public static bool EnableChangeTrackingTable(string tableName)
        {
            try
            {
                // Alter Table
                QueryCommand enableCT = new QueryCommand("ALTER TABLE " + tableName + " " +
                    "ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = ON); ", providerName);

                DataService.ExecuteQuery(enableCT);

                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return false;
            }
        }

        public static bool DisableChangeTrackingTable(string tableName)
        {
            try
            {
                // Alter Table
                QueryCommand enableCT = new QueryCommand("ALTER TABLE " + tableName + " " +
                    "DISABLE CHANGE_TRACKING; ", providerName);

                DataService.ExecuteQuery(enableCT);

                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return false;
            }
        }

        // Return NULL means Change Tracking is Disabled on the DB
        public static long? GetChangeTrackingVersion()
        {
            try
            {
                long serverVersion = -1;
                QueryCommand getCT = new QueryCommand("SELECT CHANGE_TRACKING_CURRENT_VERSION()", providerName);
                object resultCT = DataService.ExecuteScalar(getCT);
                if (resultCT.ToString() != string.Empty)
                {
                    long.TryParse(resultCT.ToString(), out serverVersion);
                    return serverVersion;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
        }

        // Return NULL means invalid table 
        public static long? GetMinValidVersion(string tableName)
        {
            try
            {
                long minValidVersion = -1;
                QueryCommand getCT = new QueryCommand("SELECT TOP 1 MIN_VALID_VERSION FROM SYS.CHANGE_TRACKING_TABLES CTT " +
                    "INNER JOIN SYS.OBJECTS OBJ ON CTT.OBJECT_ID = OBJ.OBJECT_ID WHERE OBJ.NAME = '" + tableName + "'; ", providerName);
                object resultCT = DataService.ExecuteScalar(getCT);
                if (resultCT.ToString() != string.Empty)
                {
                    long.TryParse(resultCT.ToString(), out minValidVersion);
                    return minValidVersion;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
        }

        // Return NULL means Result has no table (Table doesn't exist)
        public static DataSet GetChanges(string tableName, long lastSyncVersion, string primaryKeyName)
        {
            try
            {
                string sqlGetCTChanges = "SELECT SYS_CHANGE_VERSION, SYS_CHANGE_CREATION_VERSION, SYS_CHANGE_OPERATION, " +
                    "SYS_CHANGE_COLUMNS, SYS_CHANGE_CONTEXT, '" + primaryKeyName + "' AS PK, TBL.* " +
                    "FROM CHANGETABLE(CHANGES " + tableName + ", " + lastSyncVersion.ToString() + ") CHG " +
                    "LEFT JOIN " + tableName + " TBL ON CHG." + primaryKeyName + " = TBL." + primaryKeyName; // this code can be sql injected
                QueryCommand getCTChanges = new QueryCommand(sqlGetCTChanges, providerName);
                DataSet resultDataSet = DataService.GetDataSet(getCTChanges);
                if (resultDataSet.Tables.Count > 0)
                {
                    resultDataSet.Tables[0].TableName = tableName;
                    return resultDataSet;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
        }

        // Return NULL means Result has no table (Table doesn't exist) 
        // SYS_CHANGE_OPERATION hardcoded to 'i'
        public static DataSet GetFullTable(string tableName, string primaryKeyName)
        {
            try
            {
                string sqlGetCTChanges = "SELECT 'SYS_CHANGE_VERSION', 'SYS_CHANGE_CREATION_VERSION', 'i' SYS_CHANGE_OPERATION, " +
                    "'SYS_CHANGE_COLUMNS', 'SYS_CHANGE_CONTEXT', '" + primaryKeyName + "' AS PK, TBL.* " +
                    "FROM " + tableName + " TBL"; // this code can be sql injected
                QueryCommand getCTChanges = new QueryCommand(sqlGetCTChanges, providerName);
                DataSet resultDataSet = DataService.GetDataSet(getCTChanges);
                if (resultDataSet.Tables.Count > 0)
                {
                    resultDataSet.Tables[0].TableName = tableName;
                    return resultDataSet;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
        }

        // Return NULL means Result has no table (Table doesn't exist) 
        // SYS_CHANGE_OPERATION hardcoded to 'ie' 
        public static DataSet GetFullTableOperationIfExists(string tableName, string primaryKeyName)
        {
            try
            {
                string sqlGetCTChanges = "SELECT 'SYS_CHANGE_VERSION', 'SYS_CHANGE_CREATION_VERSION', 'ie' SYS_CHANGE_OPERATION, " +
                    "'SYS_CHANGE_COLUMNS', 'SYS_CHANGE_CONTEXT', '" + primaryKeyName + "' AS PK, TBL.* " +
                    "FROM " + tableName + " TBL"; // this code can be sql injected
                QueryCommand getCTChanges = new QueryCommand(sqlGetCTChanges, providerName);
                DataSet resultDataSet = DataService.GetDataSet(getCTChanges);
                if (resultDataSet.Tables.Count > 0)
                {
                    resultDataSet.Tables[0].TableName = tableName;
                    return resultDataSet;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
        }

        // Return QueryCommandCollection with Count = 0 means DataSet has no table or row (Table or Row doesn't exist)
        // Return NULL means Error
        // DataSet has to be generated from GetChanges(tableName, lastSyncVersion, primaryKeyName)
        public static QueryCommandCollection TransformDataSetChangeTrackingChangesToQueryCommandCollection(DataSet dataSet,
            bool IsPKAutoGenerated)
        {
            QueryCommandCollection cmdCol = new QueryCommandCollection();

            try
            {
                if (dataSet.Tables.Count > 0)
                {
                    string tableName = dataSet.Tables[0].TableName;

                    if (IsPKAutoGenerated)
                    {
                        QueryCommand cmdIdentityOn = new QueryCommand("SET IDENTITY_INSERT " + tableName + " ON;", providerName);
                        cmdCol.Add(cmdIdentityOn);
                    }

                    if (dataSet.Tables[0].Rows.Count > 0)
                    {
                        // Build Column List, and Param List
                        string ColumnList = "";
                        string ParameterList = "";
                        List<string> Columns = new List<string>(), Parameters = new List<string>();
                        for (int i = 6; i < dataSet.Tables[0].Columns.Count - 1; i++)
                        {
                            ColumnList += dataSet.Tables[0].Columns[i].ColumnName + ",";
                            ParameterList += "@" + dataSet.Tables[0].Columns[i].ColumnName + ",";
                            Columns.Add(dataSet.Tables[0].Columns[i].ColumnName);
                            Parameters.Add("@" + dataSet.Tables[0].Columns[i].ColumnName);
                        }
                        ColumnList += dataSet.Tables[0].Columns[dataSet.Tables[0].Columns.Count - 1].ColumnName;
                        ParameterList += "@" + dataSet.Tables[0].Columns[dataSet.Tables[0].Columns.Count - 1].ColumnName;
                        Columns.Add(dataSet.Tables[0].Columns[dataSet.Tables[0].Columns.Count - 1].ColumnName);
                        Parameters.Add("@" + dataSet.Tables[0].Columns[dataSet.Tables[0].Columns.Count - 1].ColumnName);

                        // Iterate Rows and Build Command (Insert, Update, Delete)
                        for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
                        {
                            string operation = string.Empty;
                            operation = dataSet.Tables[0].Rows[i]["SYS_CHANGE_OPERATION"].ToString().ToLower();

                            string sqlEachCTRow = string.Empty;
                            QueryCommand cmdEachCTRow = new QueryCommand(sqlEachCTRow, providerName);

                            if (operation == "i")
                            {
                                #region Operation Insert
                                sqlEachCTRow = "INSERT INTO " + tableName + " (" + ColumnList + ") VALUES (" + ParameterList + ")";

                                if (Columns.Count == Parameters.Count)
                                {
                                    for (int j = 0; j < Columns.Count; j++)
                                    {
                                        if (dataSet.Tables[0].Columns[Columns[j]].DataType == System.Type.GetType("System.Guid"))
                                        {
                                            cmdEachCTRow.Parameters.Add(Parameters[j],
                                                new Guid(dataSet.Tables[0].Rows[i][Columns[j]].ToString()),
                                                DataService.GetSchema(tableName, "PowerPOS").GetColumn(Columns[j].ToString()).DataType);
                                        }
                                        else
                                        {
                                            cmdEachCTRow.Parameters.Add(Parameters[j], dataSet.Tables[0].Rows[i][Columns[j]],
                                                DataService.GetSchema(tableName, "PowerPOS").GetColumn(Columns[j].ToString()).DataType);
                                        }
                                    }
                                }
                                else
                                {
                                    throw new Exception("TransformDataSetChangeTrackingChangesToQueryCommandCollection - Columns and Parameters Count is Different");
                                }

                                cmdEachCTRow.CommandSql = sqlEachCTRow; // rebind sql text
                                #endregion
                            }
                            else if (operation == "u")
                            {
                                #region Operation Update
                                string PK = dataSet.Tables[0].Rows[i]["PK"].ToString();
                                sqlEachCTRow = "UPDATE " + tableName + " SET ";

                                if (Columns.Count == Parameters.Count)
                                {
                                    for (int j = 0; j < Columns.Count - 1; j++)
                                    {
                                        if (IsPKAutoGenerated)
                                        {
                                            if (Columns[j] == PK)
                                            {
                                                continue;
                                            }
                                        }

                                        sqlEachCTRow += Columns[j] + " = " + Parameters[j] + ", ";
                                        if (dataSet.Tables[0].Columns[Columns[j]].DataType == System.Type.GetType("System.Guid"))
                                        {
                                            cmdEachCTRow.Parameters.Add(Parameters[j],
                                                new Guid(dataSet.Tables[0].Rows[i][Columns[j]].ToString()),
                                                DataService.GetSchema(tableName, "PowerPOS").GetColumn(Columns[j].ToString()).DataType);
                                        }
                                        else
                                        {
                                            cmdEachCTRow.Parameters.Add(Parameters[j], dataSet.Tables[0].Rows[i][Columns[j]],
                                                DataService.GetSchema(tableName, "PowerPOS").GetColumn(Columns[j].ToString()).DataType);
                                        }
                                    }

                                    if (IsPKAutoGenerated)
                                    {
                                        if (Columns[Columns.Count - 1] == PK)
                                        {

                                        }
                                        else
                                        {
                                            sqlEachCTRow += Columns[Columns.Count - 1] + " = " + Parameters[Parameters.Count - 1];
                                            if (dataSet.Tables[0].Columns[Columns[Columns.Count - 1]].DataType == System.Type.GetType("System.Guid"))
                                            {
                                                cmdEachCTRow.Parameters.Add(Parameters[Columns.Count - 1],
                                                    new Guid(dataSet.Tables[0].Rows[i][Columns[Columns.Count - 1]].ToString()),
                                                    DataService.GetSchema(tableName, "PowerPOS").GetColumn(Columns[Columns.Count - 1].ToString()).DataType);
                                            }
                                            else
                                            {
                                                cmdEachCTRow.Parameters.Add(Parameters[Columns.Count - 1], dataSet.Tables[0].Rows[i][Columns[Columns.Count - 1]],
                                                    DataService.GetSchema(tableName, "PowerPOS").GetColumn(Columns[Columns.Count - 1].ToString()).DataType);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        sqlEachCTRow += Columns[Columns.Count - 1] + " = " + Parameters[Parameters.Count - 1];
                                        if (dataSet.Tables[0].Columns[Columns[Columns.Count - 1]].DataType == System.Type.GetType("System.Guid"))
                                        {
                                            cmdEachCTRow.Parameters.Add(Parameters[Columns.Count - 1],
                                                new Guid(dataSet.Tables[0].Rows[i][Columns[Columns.Count - 1]].ToString()),
                                                DataService.GetSchema(tableName, "PowerPOS").GetColumn(Columns[Columns.Count - 1].ToString()).DataType);
                                        }
                                        else
                                        {
                                            cmdEachCTRow.Parameters.Add(Parameters[Columns.Count - 1], dataSet.Tables[0].Rows[i][Columns[Columns.Count - 1]],
                                                DataService.GetSchema(tableName, "PowerPOS").GetColumn(Columns[Columns.Count - 1].ToString()).DataType);
                                        }
                                    }
                                }
                                else
                                {
                                    throw new Exception("TransformDataSetChangeTrackingChangesToQueryCommandCollection - Columns and Parameters Count is Different");
                                }

                                sqlEachCTRow += " WHERE " + PK + " = @" + PK;
                                if (dataSet.Tables[0].Columns[PK].DataType == System.Type.GetType("System.Guid"))
                                {
                                    cmdEachCTRow.Parameters.Add("@" + PK,
                                        new Guid(dataSet.Tables[0].Rows[i][PK].ToString()),
                                        DataService.GetSchema(tableName, "PowerPOS").GetColumn(PK).DataType);
                                }
                                else
                                {
                                    cmdEachCTRow.Parameters.Add("@" + PK, dataSet.Tables[0].Rows[i][PK],
                                        DataService.GetSchema(tableName, "PowerPOS").GetColumn(PK).DataType);
                                }

                                cmdEachCTRow.CommandSql = sqlEachCTRow; // rebind sql text
                                #endregion
                            }
                            else if (operation == "d")
                            {
                                // No Deletion Method
                                continue;
                            }
                            else if (operation == "ie") // Custom Operation for Insert or Update If Exists
                            {
                                #region Operation Upsert

                                // Building SQL String
                                string PK = dataSet.Tables[0].Rows[i]["PK"].ToString();
                                sqlEachCTRow = "IF EXISTS ( SELECT TOP 1 " + PK + " FROM " + tableName + " WHERE " + PK
                                    + " = @" + PK + " ) " +
                                    "BEGIN " +
                                        "UPDATE " + tableName + " SET ";

                                for (int j = 0; j < Columns.Count - 1; j++)
                                {
                                    if (Columns[j] == PK)
                                    {
                                        continue;
                                    }

                                    sqlEachCTRow += Columns[j] + " = " + Parameters[j] + ", ";
                                }
                                if (!(Columns[Columns.Count - 1] == PK))
                                {
                                    sqlEachCTRow += Columns[Columns.Count - 1] + " = " + Parameters[Parameters.Count - 1];
                                }

                                sqlEachCTRow = sqlEachCTRow + " " +
                                        "WHERE " + PK + " = @" + PK + " " +
                                    "END " +
                                    "ELSE " +
                                    "BEGIN " +
                                        "INSERT INTO " + tableName + " (" + ColumnList + ") VALUES (" + ParameterList + ") " +
                                    "END";

                                cmdEachCTRow.CommandSql = sqlEachCTRow; // rebind sql query

                                // Passing All Columns As Parameters
                                for (int j = 0; j < Columns.Count; j++)
                                {
                                    if (dataSet.Tables[0].Columns[Columns[j]].DataType == System.Type.GetType("System.Guid"))
                                    {
                                        cmdEachCTRow.Parameters.Add(Parameters[j],
                                            new Guid(dataSet.Tables[0].Rows[i][Columns[j]].ToString()),
                                            DataService.GetSchema(tableName, "PowerPOS").GetColumn(Columns[j].ToString()).DataType);
                                    }
                                    else
                                    {
                                        cmdEachCTRow.Parameters.Add(Parameters[j], dataSet.Tables[0].Rows[i][Columns[j]],
                                            DataService.GetSchema(tableName, "PowerPOS").GetColumn(Columns[j].ToString()).DataType);
                                    }

                                    //verina add to debug
                                    if (tableName == "OrderHdr" && Parameters[j] == "@OrderHdrID")
                                        sqlEachCTRow += " INSERT INTO PowerLog ([LogDate],[LogMsg]) VALUES (GETDATE() ,' " + dataSet.Tables[0].Rows[i][Columns[j]] + "')";
                                    cmdEachCTRow.CommandSql = sqlEachCTRow;
                                }

                                #endregion
                            }
                            else if (operation == "custom1") // Custom Operation for OrderHdr Update UserFld1, UserFld2, UserFld3, UserFld6
                            {
                                // Building SQL String
                                sqlEachCTRow =
                                    "IF EXISTS ( SELECT OrderHdrID FROM OrderHdr WHERE OrderHdrID = @OrderHdrID ) " +
                                    "BEGIN " +
                                        "IF EXISTS ( SELECT OrderHdrID FROM OrderHdr WHERE OrderHdrID = @OrderHdrID " +
                                            "AND userfld1 = @UserFld1 AND userfld2 = @UserFld2 " +
                                            "AND userfld3 = @UserFld3 AND userfld6 = @UserFld6 AND Remark = @Remark ) " +
                                        "BEGIN " +
                                            "SELECT '0' " +
                                        "END " +
                                        "ELSE " +
                                        "BEGIN " +
                                            "UPDATE OrderHdr " +
                                                "SET userfld1 = @UserFld1, userfld2 = @UserFld2, " +
                                                    "userfld3 = @UserFld3, userfld6 = @UserFld6, " +
                                                    "Remark = @Remark " +
                                                "WHERE OrderHdrID = @OrderHdrID " +
                                        "END " +
                                    "END " +
                                    "ELSE " +
                                    "BEGIN " +
                                        "SELECT '0' " +
                                    "END";

                                // Rebind SQL Query
                                cmdEachCTRow.CommandSql = sqlEachCTRow;

                                // Bind Params to Command
                                cmdEachCTRow.Parameters.Add("@OrderHdrID", dataSet.Tables[0].Rows[i]["OrderHdrID"],
                                    DataService.GetSchema(tableName, providerName).GetColumn("OrderHdrID").DataType);
                                cmdEachCTRow.Parameters.Add("@UserFld1", dataSet.Tables[0].Rows[i]["UserFld1"],
                                    DataService.GetSchema(tableName, providerName).GetColumn("UserFld1").DataType);
                                cmdEachCTRow.Parameters.Add("@UserFld2", dataSet.Tables[0].Rows[i]["UserFld2"],
                                    DataService.GetSchema(tableName, providerName).GetColumn("UserFld2").DataType);
                                cmdEachCTRow.Parameters.Add("@UserFld3", dataSet.Tables[0].Rows[i]["UserFld3"],
                                    DataService.GetSchema(tableName, providerName).GetColumn("UserFld3").DataType);
                                cmdEachCTRow.Parameters.Add("@UserFld6", dataSet.Tables[0].Rows[i]["UserFld6"],
                                    DataService.GetSchema(tableName, providerName).GetColumn("UserFld6").DataType);
                                cmdEachCTRow.Parameters.Add("@Remark", dataSet.Tables[0].Rows[i]["Remark"],
                                    DataService.GetSchema(tableName, providerName).GetColumn("Remark").DataType);
                            }

                            cmdCol.Add(cmdEachCTRow);
                        }
                    }

                    if (IsPKAutoGenerated)
                    {
                        QueryCommand cmdIdentityOff = new QueryCommand("SET IDENTITY_INSERT " + tableName + " OFF;", providerName);
                        cmdCol.Add(cmdIdentityOff);
                    }
                }

                return cmdCol;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
        }

        // Return QueryCommandCollection with Count = 0 means DataSet has no table or row (Table or Row doesn't exist)
        // Return NULL means Error
        // This is a special case, for AutoIncrement PK table, and has UniqueID
        // To solve two client having the same ID upload problem, so use UniqueID instead of PK
        // Will insert without identity insert, and will update and upsert using UniqueID as ID
        public static QueryCommandCollection TransformDataSetChangeTrackingChangesToQueryCommandCollectionWithUniqueIDAsPK
            (DataSet dataSet)
        {
            QueryCommandCollection cmdCol = new QueryCommandCollection();

            try
            {
                if (dataSet.Tables.Count > 0)
                {
                    string tableName = dataSet.Tables[0].TableName;

                    if (dataSet.Tables[0].Rows.Count > 0)
                    {
                        // Build Column List, and Param List (without PK)
                        string ColumnList = "";
                        string ParameterList = "";
                        List<string> Columns = new List<string>(), Parameters = new List<string>();
                        for (int i = 6; i < dataSet.Tables[0].Columns.Count - 1; i++)
                        {
                            string PK = dataSet.Tables[0].Rows[0]["PK"].ToString();
                            if (dataSet.Tables[0].Columns[i].ColumnName == PK)
                            {
                                continue;
                            }

                            ColumnList += dataSet.Tables[0].Columns[i].ColumnName + ",";
                            ParameterList += "@" + dataSet.Tables[0].Columns[i].ColumnName + ",";
                            Columns.Add(dataSet.Tables[0].Columns[i].ColumnName);
                            Parameters.Add("@" + dataSet.Tables[0].Columns[i].ColumnName);
                        }
                        ColumnList += dataSet.Tables[0].Columns[dataSet.Tables[0].Columns.Count - 1].ColumnName;
                        ParameterList += "@" + dataSet.Tables[0].Columns[dataSet.Tables[0].Columns.Count - 1].ColumnName;
                        Columns.Add(dataSet.Tables[0].Columns[dataSet.Tables[0].Columns.Count - 1].ColumnName);
                        Parameters.Add("@" + dataSet.Tables[0].Columns[dataSet.Tables[0].Columns.Count - 1].ColumnName);

                        // Iterate Rows and Build Command (Insert, Update, Delete)
                        for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
                        {
                            string operation = string.Empty;
                            operation = dataSet.Tables[0].Rows[i]["SYS_CHANGE_OPERATION"].ToString().ToLower();

                            string sqlEachCTRow = string.Empty;
                            QueryCommand cmdEachCTRow = new QueryCommand(sqlEachCTRow, providerName);

                            if (operation == "i")
                            {
                                #region Operation Insert
                                sqlEachCTRow = "INSERT INTO " + tableName + " (" + ColumnList + ") VALUES (" + ParameterList + ")";

                                if (Columns.Count == Parameters.Count)
                                {
                                    for (int j = 0; j < Columns.Count; j++)
                                    {
                                        if (dataSet.Tables[0].Columns[Columns[j]].DataType == System.Type.GetType("System.Guid"))
                                        {
                                            cmdEachCTRow.Parameters.Add(Parameters[j],
                                                new Guid(dataSet.Tables[0].Rows[i][Columns[j]].ToString()),
                                                DataService.GetSchema(tableName, "PowerPOS").GetColumn(Columns[j].ToString()).DataType);
                                        }
                                        else
                                        {
                                            cmdEachCTRow.Parameters.Add(Parameters[j], dataSet.Tables[0].Rows[i][Columns[j]],
                                                DataService.GetSchema(tableName, "PowerPOS").GetColumn(Columns[j].ToString()).DataType);
                                        }
                                    }
                                }
                                else
                                {
                                    throw new Exception("TransformDataSetChangeTrackingChangesToQueryCommandCollection - Columns and Parameters Count is Different");
                                }

                                cmdEachCTRow.CommandSql = sqlEachCTRow; // rebind sql text
                                #endregion
                            }
                            else if (operation == "u")
                            {
                                #region Operation Update
                                sqlEachCTRow = "UPDATE " + tableName + " SET ";

                                if (Columns.Count == Parameters.Count)
                                {
                                    for (int j = 0; j < Columns.Count - 1; j++)
                                    {
                                        if (Columns[j].ToLower() == "uniqueid")
                                        {
                                            continue;
                                        }

                                        sqlEachCTRow += Columns[j] + " = " + Parameters[j] + ", ";
                                        if (dataSet.Tables[0].Columns[Columns[j]].DataType == System.Type.GetType("System.Guid"))
                                        {
                                            cmdEachCTRow.Parameters.Add(Parameters[j],
                                                new Guid(dataSet.Tables[0].Rows[i][Columns[j]].ToString()),
                                                DataService.GetSchema(tableName, "PowerPOS").GetColumn(Columns[j].ToString()).DataType);
                                        }
                                        else
                                        {
                                            cmdEachCTRow.Parameters.Add(Parameters[j], dataSet.Tables[0].Rows[i][Columns[j]],
                                                DataService.GetSchema(tableName, "PowerPOS").GetColumn(Columns[j].ToString()).DataType);
                                        }
                                    }

                                    if (Columns[Columns.Count - 1].ToLower() == "uniqueid")
                                    {

                                    }
                                    else
                                    {
                                        sqlEachCTRow += Columns[Columns.Count - 1] + " = " + Parameters[Parameters.Count - 1];
                                        if (dataSet.Tables[0].Columns[Columns[Columns.Count - 1]].DataType == System.Type.GetType("System.Guid"))
                                        {
                                            cmdEachCTRow.Parameters.Add(Parameters[Columns.Count - 1],
                                                new Guid(dataSet.Tables[0].Rows[i][Columns[Columns.Count - 1]].ToString()),
                                                DataService.GetSchema(tableName, "PowerPOS").GetColumn(Columns[Columns.Count - 1].ToString()).DataType);
                                        }
                                        else
                                        {
                                            cmdEachCTRow.Parameters.Add(Parameters[Columns.Count - 1], dataSet.Tables[0].Rows[i][Columns[Columns.Count - 1]],
                                                DataService.GetSchema(tableName, "PowerPOS").GetColumn(Columns[Columns.Count - 1].ToString()).DataType);
                                        }
                                    }
                                }
                                else
                                {
                                    throw new Exception("TransformDataSetChangeTrackingChangesToQueryCommandCollection - Columns and Parameters Count is Different");
                                }

                                sqlEachCTRow += " WHERE UniqueID = @UniqueID";
                                if (dataSet.Tables[0].Columns["uniqueid"].DataType == System.Type.GetType("System.Guid"))
                                {
                                    cmdEachCTRow.Parameters.Add("@UniqueID",
                                        new Guid(dataSet.Tables[0].Rows[i]["UniqueID"].ToString()),
                                        DataService.GetSchema(tableName, "PowerPOS").GetColumn("UniqueID").DataType);
                                }
                                else
                                {
                                    cmdEachCTRow.Parameters.Add("@UniqueID", dataSet.Tables[0].Rows[i]["UniqueID"],
                                        DataService.GetSchema(tableName, "PowerPOS").GetColumn("UniqueID").DataType);
                                }

                                cmdEachCTRow.CommandSql = sqlEachCTRow; // rebind sql text
                                #endregion
                            }
                            else if (operation == "d")
                            {
                                // No Deletion Method
                                continue;
                            }
                            else if (operation == "ie") // Custom Operation for Insert or Update If Exists
                            {
                                // Building SQL String
                                sqlEachCTRow = "IF EXISTS ( SELECT TOP 1 UniqueID FROM " + tableName + " WHERE UniqueID "
                                    + " = @UniqueID ) " +
                                    "BEGIN " +
                                        "UPDATE " + tableName + " SET ";

                                for (int j = 0; j < Columns.Count - 1; j++)
                                {
                                    sqlEachCTRow += Columns[j] + " = " + Parameters[j] + ", ";
                                }
                                sqlEachCTRow += Columns[Columns.Count - 1] + " = " + Parameters[Parameters.Count - 1];

                                sqlEachCTRow = sqlEachCTRow + " " +
                                        "WHERE UniqueID = @UniqueID " +
                                    "END " +
                                    "ELSE " +
                                    "BEGIN " +
                                        "INSERT INTO " + tableName + " (" + ColumnList + ") VALUES (" + ParameterList + ") " +
                                    "END";

                                cmdEachCTRow.CommandSql = sqlEachCTRow; // rebind sql query

                                // Passing All Columns As Parameters
                                for (int j = 0; j < Columns.Count; j++)
                                {
                                    if (dataSet.Tables[0].Columns[Columns[j]].DataType == System.Type.GetType("System.Guid"))
                                    {
                                        cmdEachCTRow.Parameters.Add(Parameters[j],
                                            new Guid(dataSet.Tables[0].Rows[i][Columns[j]].ToString()),
                                            DataService.GetSchema(tableName, "PowerPOS").GetColumn(Columns[j].ToString()).DataType);
                                    }
                                    else
                                    {
                                        cmdEachCTRow.Parameters.Add(Parameters[j], dataSet.Tables[0].Rows[i][Columns[j]],
                                            DataService.GetSchema(tableName, "PowerPOS").GetColumn(Columns[j].ToString()).DataType);
                                    }

                                    //verina add to debug
                                    if (tableName == "OrderHdr" && Parameters[j] == "@OrderHdrID")
                                        sqlEachCTRow += " INSERT INTO PowerLog ([LogDate],[LogMsg]) VALUES (GETDATE() ,' " + dataSet.Tables[0].Rows[i][Columns[j]] + "')";
                                    cmdEachCTRow.CommandSql = sqlEachCTRow;
                                }
                            }



                            cmdCol.Add(cmdEachCTRow);
                        }
                    }
                }

                return cmdCol;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
        }

        #region Custom Order Tables (Get Full Table)

        // Get OrderHdr FilteredBy PosID, ViewBillLimit, and IsVoided False
        public static DataSet GetOrderHdrByPosIDViewBillLimitAndIsVoided(int posId, int viewBillLimit)
        {
            try
            {
                string sqlGetCTChanges = "SELECT 'SYS_CHANGE_VERSION', 'SYS_CHANGE_CREATION_VERSION', 'custom1' SYS_CHANGE_OPERATION, " +
                    "'SYS_CHANGE_COLUMNS', 'SYS_CHANGE_CONTEXT', 'OrderHdrID' AS PK, TBL.OrderHdrID, TBL.UserFld1, TBL.UserFld2, " +
                    "TBL.UserFld3, TBL.UserFld6, TBL.Remark " +
                    "FROM OrderHdr TBL WHERE TBL.OrderDate > '" + DateTime.Now.AddDays(-viewBillLimit).ToString("yyyy-MM-dd HH:mm") + "' " +
                    "AND TBL.IsVoided = 0 AND TBL.PointOfSaleID = '" + posId.ToString() + "'"; // this code can be sql injected
                QueryCommand getCTChanges = new QueryCommand(sqlGetCTChanges, providerName);
                DataSet resultDataSet = DataService.GetDataSet(getCTChanges);
                if (resultDataSet.Tables.Count > 0)
                {
                    resultDataSet.Tables[0].TableName = "OrderHdr";
                    return resultDataSet;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
        }

        // Compare OrderHdr OrderDate
        public static DataSet GetOrderHdrFullTable(DateTime lastClosingDate)
        {
            try
            {
                string sqlGetCTChanges = "SELECT 'SYS_CHANGE_VERSION', 'SYS_CHANGE_CREATION_VERSION', 'ie' SYS_CHANGE_OPERATION, " +
                    "'SYS_CHANGE_COLUMNS', 'SYS_CHANGE_CONTEXT', 'OrderHdrID' AS PK, TBL.* " +
                    "FROM OrderHdr TBL WHERE TBL.OrderDate > '" + lastClosingDate.ToString("yyyy-MM-dd HH:mm") + "'"; // this code can be sql injected
                QueryCommand getCTChanges = new QueryCommand(sqlGetCTChanges, providerName);
                DataSet resultDataSet = DataService.GetDataSet(getCTChanges);
                if (resultDataSet.Tables.Count > 0)
                {
                    resultDataSet.Tables[0].TableName = "OrderHdr";
                    return resultDataSet;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
        }

        // Compare OrderHdr OrderDate
        public static DataSet GetOrderDetFullTable(DateTime lastClosingDate)
        {
            try
            {
                string sqlGetCTChanges = "SELECT 'SYS_CHANGE_VERSION', 'SYS_CHANGE_CREATION_VERSION', 'ie' SYS_CHANGE_OPERATION, " +
                    "'SYS_CHANGE_COLUMNS', 'SYS_CHANGE_CONTEXT', 'OrderDetID' AS PK, TBL.* " +
                    "FROM OrderDet TBL INNER JOIN OrderHdr OH ON TBL.OrderHdrID = OH.OrderHdrID " +
                    "WHERE OH.OrderDate > '" + lastClosingDate.ToString("yyyy-MM-dd HH:mm") + "'"; // this code can be sql injected
                QueryCommand getCTChanges = new QueryCommand(sqlGetCTChanges, providerName);
                DataSet resultDataSet = DataService.GetDataSet(getCTChanges);
                if (resultDataSet.Tables.Count > 0)
                {
                    resultDataSet.Tables[0].TableName = "OrderDet";
                    return resultDataSet;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
        }

        // Compare OrderHdr OrderDate
        public static DataSet GetReceiptHdrFullTable(DateTime lastClosingDate)
        {
            try
            {
                string sqlGetCTChanges = "SELECT 'SYS_CHANGE_VERSION', 'SYS_CHANGE_CREATION_VERSION', 'ie' SYS_CHANGE_OPERATION, " +
                    "'SYS_CHANGE_COLUMNS', 'SYS_CHANGE_CONTEXT', 'ReceiptHdrID' AS PK, TBL.* " +
                    "FROM ReceiptHdr TBL INNER JOIN OrderHdr OH ON TBL.OrderHdrID = OH.OrderHdrID " +
                    "WHERE OH.OrderDate > '" + lastClosingDate.ToString("yyyy-MM-dd HH:mm") + "'"; // this code can be sql injected
                QueryCommand getCTChanges = new QueryCommand(sqlGetCTChanges, providerName);
                DataSet resultDataSet = DataService.GetDataSet(getCTChanges);
                if (resultDataSet.Tables.Count > 0)
                {
                    resultDataSet.Tables[0].TableName = "ReceiptHdr";
                    return resultDataSet;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
        }

        // Compare OrderHdr OrderDate
        public static DataSet GetReceiptDetFullTable(DateTime lastClosingDate)
        {
            try
            {
                string sqlGetCTChanges = "SELECT 'SYS_CHANGE_VERSION', 'SYS_CHANGE_CREATION_VERSION', 'ie' SYS_CHANGE_OPERATION, " +
                    "'SYS_CHANGE_COLUMNS', 'SYS_CHANGE_CONTEXT', 'ReceiptDetID' AS PK, TBL.* " +
                    "FROM ReceiptDet TBL INNER JOIN ReceiptHdr RH ON TBL.ReceiptHdrID = RH.ReceiptHdrID " +
                    "INNER JOIN OrderHdr OH ON RH.OrderHdrID = OH.OrderHdrID " +
                    "WHERE OH.OrderDate > '" + lastClosingDate.ToString("yyyy-MM-dd HH:mm") + "'"; // this code can be sql injected
                QueryCommand getCTChanges = new QueryCommand(sqlGetCTChanges, providerName);
                DataSet resultDataSet = DataService.GetDataSet(getCTChanges);
                if (resultDataSet.Tables.Count > 0)
                {
                    resultDataSet.Tables[0].TableName = "ReceiptDet";
                    return resultDataSet;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
        }

        // Compare OrderHdr OrderDate
        public static DataSet GetSalesCommissionRecordFullTable(DateTime lastClosingDate)
        {
            try
            {
                string sqlGetCTChanges = "SELECT 'SYS_CHANGE_VERSION', 'SYS_CHANGE_CREATION_VERSION', 'ie' SYS_CHANGE_OPERATION, " +
                    "'SYS_CHANGE_COLUMNS', 'SYS_CHANGE_CONTEXT', 'CommissionRecordID' AS PK, TBL.* " +
                    "FROM SalesCommissionRecord TBL INNER JOIN OrderHdr OH ON TBL.OrderHdrID = OH.OrderHdrID " +
                    "WHERE OH.OrderDate > '" + lastClosingDate.ToString("yyyy-MM-dd HH:mm") + "'"; // this code can be sql injected
                QueryCommand getCTChanges = new QueryCommand(sqlGetCTChanges, providerName);
                DataSet resultDataSet = DataService.GetDataSet(getCTChanges);
                if (resultDataSet.Tables.Count > 0)
                {
                    resultDataSet.Tables[0].TableName = "SalesCommissionRecord";
                    return resultDataSet;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
        }

        // Compare WarningMsg CreatedOn
        public static DataSet GetWarningMsgFullTable(DateTime lastClosingDate)
        {
            try
            {
                string sqlGetCTChanges = "SELECT 'SYS_CHANGE_VERSION', 'SYS_CHANGE_CREATION_VERSION', 'ie' SYS_CHANGE_OPERATION, " +
                    "'SYS_CHANGE_COLUMNS', 'SYS_CHANGE_CONTEXT', 'UniqueID' AS PK, TBL.* " +
                    "FROM WarningMsg TBL WHERE TBL.CreatedOn > '" + lastClosingDate.ToString("yyyy-MM-dd HH:mm") + "'";
                QueryCommand getCTChanges = new QueryCommand(sqlGetCTChanges, providerName);
                DataSet resultDataSet = DataService.GetDataSet(getCTChanges);
                if (resultDataSet.Tables.Count > 0)
                {
                    resultDataSet.Tables[0].TableName = "WarningMsg";
                    return resultDataSet;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
        }

        // Compare MembershipUpgradeLog ModifiedOn
        public static DataSet GetMembershipUpgradeLogFullTable(DateTime lastClosingDate)
        {
            try
            {
                string sqlGetCTChanges = "SELECT 'SYS_CHANGE_VERSION', 'SYS_CHANGE_CREATION_VERSION', 'ie' SYS_CHANGE_OPERATION, " +
                    "'SYS_CHANGE_COLUMNS', 'SYS_CHANGE_CONTEXT', 'OrderHdrID' AS PK, TBL.* " +
                    "FROM MembershipUpgradeLog TBL WHERE TBL.ModifiedOn > '" + lastClosingDate.ToString("yyyy-MM-dd HH:mm") + "'";
                QueryCommand getCTChanges = new QueryCommand(sqlGetCTChanges, providerName);
                DataSet resultDataSet = DataService.GetDataSet(getCTChanges);
                if (resultDataSet.Tables.Count > 0)
                {
                    resultDataSet.Tables[0].TableName = "MembershipUpgradeLog";
                    return resultDataSet;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
        }

        // Compare CashRecording ModifiedOn
        public static DataSet GetCashRecordingFullTable(DateTime lastClosingDate)
        {
            try
            {
                string sqlGetCTChanges = "SELECT 'SYS_CHANGE_VERSION', 'SYS_CHANGE_CREATION_VERSION', 'ie' SYS_CHANGE_OPERATION, " +
                    "'SYS_CHANGE_COLUMNS', 'SYS_CHANGE_CONTEXT', 'CashRecID' AS PK, TBL.* " +
                    "FROM CashRecording TBL WHERE TBL.ModifiedOn > '" + lastClosingDate.ToString("yyyy-MM-dd HH:mm") + "'";
                QueryCommand getCTChanges = new QueryCommand(sqlGetCTChanges, providerName);
                DataSet resultDataSet = DataService.GetDataSet(getCTChanges);
                if (resultDataSet.Tables.Count > 0)
                {
                    resultDataSet.Tables[0].TableName = "CashRecording";
                    return resultDataSet;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
        }

        // Compare CounterCloseLog ModifiedOn
        public static DataSet GetCounterCloseLogFullTable(DateTime lastClosingDate)
        {
            try
            {
                string sqlGetCTChanges = "SELECT 'SYS_CHANGE_VERSION', 'SYS_CHANGE_CREATION_VERSION', 'ie' SYS_CHANGE_OPERATION, " +
                    "'SYS_CHANGE_COLUMNS', 'SYS_CHANGE_CONTEXT', 'CounterCloseLogID' AS PK, TBL.* " +
                    "FROM CounterCloseLog TBL WHERE TBL.ModifiedOn > '" + lastClosingDate.ToString("yyyy-MM-dd HH:mm") + "'";
                QueryCommand getCTChanges = new QueryCommand(sqlGetCTChanges, providerName);
                DataSet resultDataSet = DataService.GetDataSet(getCTChanges);
                if (resultDataSet.Tables.Count > 0)
                {
                    resultDataSet.Tables[0].TableName = "CounterCloseLog";
                    return resultDataSet;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
        }

        // Compare CounterCloseLog ModifiedOn
        public static DataSet GetCounterCloseDetFullTable(DateTime lastClosingDate)
        {
            try
            {
                string sqlGetCTChanges = "SELECT 'SYS_CHANGE_VERSION', 'SYS_CHANGE_CREATION_VERSION', 'ie' SYS_CHANGE_OPERATION, " +
                    "'SYS_CHANGE_COLUMNS', 'SYS_CHANGE_CONTEXT', 'CounterCloseLogID' AS PK, TBL.* " +
                    "FROM CounterCloseDet TBL WHERE TBL.ModifiedOn > '" + lastClosingDate.ToString("yyyy-MM-dd HH:mm") + "'";
                QueryCommand getCTChanges = new QueryCommand(sqlGetCTChanges, providerName);
                DataSet resultDataSet = DataService.GetDataSet(getCTChanges);
                if (resultDataSet.Tables.Count > 0)
                {
                    resultDataSet.Tables[0].TableName = "CounterCloseDet";
                    return resultDataSet;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
        }

        // Compare SpecialActivityLog ModifiedOn
        public static DataSet GetSpecialActivityLogFullTable(DateTime lastClosingDate)
        {
            try
            {
                string sqlGetCTChanges = "SELECT 'SYS_CHANGE_VERSION', 'SYS_CHANGE_CREATION_VERSION', 'ie' SYS_CHANGE_OPERATION, " +
                    "'SYS_CHANGE_COLUMNS', 'SYS_CHANGE_CONTEXT', 'SpecialActivityLogID' AS PK, TBL.* " +
                    "FROM SpecialActivityLog TBL WHERE TBL.ModifiedOn > '" + lastClosingDate.ToString("yyyy-MM-dd HH:mm") + "'";
                QueryCommand getCTChanges = new QueryCommand(sqlGetCTChanges, providerName);
                DataSet resultDataSet = DataService.GetDataSet(getCTChanges);
                if (resultDataSet.Tables.Count > 0)
                {
                    resultDataSet.Tables[0].TableName = "SpecialActivityLog";
                    return resultDataSet;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
        }

        // Compare PreOrderRecord ModifiedOn
        public static DataSet GetPreOrderRecordFullTable(DateTime lastClosingDate)
        {
            try
            {
                string sqlGetCTChanges = "SELECT 'SYS_CHANGE_VERSION', 'SYS_CHANGE_CREATION_VERSION', 'ie' SYS_CHANGE_OPERATION, " +
                    "'SYS_CHANGE_COLUMNS', 'SYS_CHANGE_CONTEXT', 'PreOrderLogID' AS PK, TBL.* " +
                    "FROM PreOrderRecord TBL WHERE TBL.ModifiedOn > '" + lastClosingDate.ToString("yyyy-MM-dd HH:mm") + "'";
                QueryCommand getCTChanges = new QueryCommand(sqlGetCTChanges, providerName);
                DataSet resultDataSet = DataService.GetDataSet(getCTChanges);
                if (resultDataSet.Tables.Count > 0)
                {
                    resultDataSet.Tables[0].TableName = "PreOrderRecord";
                    return resultDataSet;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
        }

        // Using old method (view), from Synclientcontroller
        public static DataSet GetMembershipFullTable(DateTime lastClosingDate)
        {
            try
            {
                string sqlGetCTChanges = "SELECT 'SYS_CHANGE_VERSION', 'SYS_CHANGE_CREATION_VERSION', 'ie' SYS_CHANGE_OPERATION, " +
                    "'SYS_CHANGE_COLUMNS', 'SYS_CHANGE_CONTEXT', 'MembershipNo' AS PK, TBL.* " +
                    "FROM Membership TBL WHERE TBL.ModifiedOn > '" + lastClosingDate.ToString("yyyy-MM-dd HH:mm") + "'";
                QueryCommand getCTChanges = new QueryCommand(sqlGetCTChanges, providerName);
                DataSet resultDataSet = DataService.GetDataSet(getCTChanges);
                if (resultDataSet.Tables.Count > 0)
                {
                    resultDataSet.Tables[0].TableName = "Membership";
                    return resultDataSet;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
        }
        #region Old Method
        // Use View to Retrieve New MembershipNo List
        //Query qr = ViewTransactionWithMembership.CreateQuery();
        //qr.QueryType = QueryType.Select;
        //qr.SelectList = "MembershipNo";
        //qr.AddWhere(ViewTransactionWithMembership.Columns.OrderDate, Comparison.GreaterOrEquals, lastClosingDate);
        //qr.AddWhere(ViewTransactionWithMembership.Columns.OrderDate, Comparison.LessOrEquals, DateTime.Now);
        //qr.AddWhere(ViewTransactionWithMembership.Columns.ItemNo, "MEMBER");
        //DataSet dsList = qr.ExecuteDataSet();
        //ArrayList membershipList = new ArrayList();
        //for (int k = 0; k < dsList.Tables[0].Rows.Count; k++)
        //{
        //    membershipList.Add(dsList.Tables[0].Rows[k][0].ToString());
        //}

        //MembershipCollection myMembers = new MembershipCollection();
        //myMembers.Where(Membership.Columns.MembershipNo, Comparison.In, membershipList);
        //myMembers.Where(Membership.Columns.CreatedOn, Comparison.GreaterOrEquals, lastClosingDate);
        //myMembers.Where(Membership.Columns.CreatedOn, Comparison.LessOrEquals, DateTime.Now);
        //myMembers.Load();

        //DataTable dt = myMembers.ToDataTable();
        //dt.TableName = "Membership";
        //DataColumn col = dt.Columns.Add("PK");
        //col.SetOrdinal(0);
        //col = dt.Columns.Add("SYS_CHANGE_CONTEXT");
        //col.SetOrdinal(0);
        //col = dt.Columns.Add("SYS_CHANGE_COLUMNS");
        //col.SetOrdinal(0);
        //col = dt.Columns.Add("SYS_CHANGE_OPERATION");
        //col.SetOrdinal(0);
        //col = dt.Columns.Add("SYS_CHANGE_CREATION_VERSION");
        //col.SetOrdinal(0);
        //col = dt.Columns.Add("SYS_CHANGE_VERSION");
        //col.SetOrdinal(0);

        //foreach (DataRow dr in dt.Rows)
        //{
        //    dr["SYS_CHANGE_OPERATION"] = "ie";
        //    dr["PK"] = "MembershipNo";
        //    dr.AcceptChanges();
        //    dt.AcceptChanges();
        //}

        //DataSet resultDataSet = new DataSet();
        //resultDataSet.Tables.Add(dt);
        //if (resultDataSet.Tables.Count > 0)
        //{
        //    resultDataSet.Tables[0].TableName = "Membership";
        //    return resultDataSet;
        //}
        //else
        //{
        //    return null;
        //}
        #endregion

        // Compare MembershipRenewal ModifiedOn
        public static DataSet GetMembershipRenewalFullTable(DateTime lastClosingDate)
        {
            try
            {
                string sqlGetCTChanges = "SELECT 'SYS_CHANGE_VERSION', 'SYS_CHANGE_CREATION_VERSION', 'ie' SYS_CHANGE_OPERATION, " +
                    "'SYS_CHANGE_COLUMNS', 'SYS_CHANGE_CONTEXT', 'renewalID' AS PK, TBL.* " +
                    "FROM MembershipRenewal TBL WHERE TBL.ModifiedOn > '" + lastClosingDate.ToString("yyyy-MM-dd HH:mm") + "'";
                QueryCommand getCTChanges = new QueryCommand(sqlGetCTChanges, providerName);
                DataSet resultDataSet = DataService.GetDataSet(getCTChanges);
                if (resultDataSet.Tables.Count > 0)
                {
                    resultDataSet.Tables[0].TableName = "MembershipRenewal";
                    return resultDataSet;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
        }

        // Compare PackageRedemptionLog ModifiedOn
        public static DataSet GetPackageRedemptionLogFullTable(DateTime lastClosingDate)
        {
            try
            {
                string sqlGetCTChanges = "SELECT 'SYS_CHANGE_VERSION', 'SYS_CHANGE_CREATION_VERSION', 'ie' SYS_CHANGE_OPERATION, " +
                    "'SYS_CHANGE_COLUMNS', 'SYS_CHANGE_CONTEXT', 'PackageRedeemID' AS PK, TBL.* " +
                    "FROM PackageRedemptionLog TBL WHERE TBL.ModifiedOn > '" + lastClosingDate.ToString("yyyy-MM-dd HH:mm") + "'";
                QueryCommand getCTChanges = new QueryCommand(sqlGetCTChanges, providerName);
                DataSet resultDataSet = DataService.GetDataSet(getCTChanges);
                if (resultDataSet.Tables.Count > 0)
                {
                    resultDataSet.Tables[0].TableName = "PackageRedemptionLog";
                    return resultDataSet;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
        }

        // Return NULL means Result has no table (Table doesn't exist) 
        // SYS_CHANGE_OPERATION hardcoded to 'i'
        public static DataSet GetInventoryHdrFullTable(DateTime StartDate)
        {
            try
            {
                string sqlGetCTChanges = "SELECT 'SYS_CHANGE_VERSION', 'SYS_CHANGE_CREATION_VERSION', 'i' SYS_CHANGE_OPERATION, " +
                    "'SYS_CHANGE_COLUMNS', 'SYS_CHANGE_CONTEXT', 'InventoryHdrRefNo' AS PK, TBL.* " +
                    "FROM InventoryHdr TBL where InventoryDate > '" + StartDate.ToString("yyyy-MM-dd") + "'"; // this code can be sql injected
                QueryCommand getCTChanges = new QueryCommand(sqlGetCTChanges, providerName);
                DataSet resultDataSet = DataService.GetDataSet(getCTChanges);
                if (resultDataSet.Tables.Count > 0)
                {
                    resultDataSet.Tables[0].TableName = "InventoryHdr";
                    return resultDataSet;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
        }

        public static DataSet GetInventoryDetFullTable(DateTime StartDate)
        {
            try
            {
                string sqlGetCTChanges = "SELECT 'SYS_CHANGE_VERSION', 'SYS_CHANGE_CREATION_VERSION', 'i' SYS_CHANGE_OPERATION, " +
                    "'SYS_CHANGE_COLUMNS', 'SYS_CHANGE_CONTEXT', 'InventoryDetRefNo' AS PK, TBL.* " +
                    "FROM InventoryDet TBL where InventoryHdrRefNo in (Select InventoryHdrRefNo from InventoryHdr where InventoryDate > '" + StartDate.ToString("yyyy-MM-dd") + "')"; // this code can be sql injected
                QueryCommand getCTChanges = new QueryCommand(sqlGetCTChanges, providerName);
                DataSet resultDataSet = DataService.GetDataSet(getCTChanges);
                if (resultDataSet.Tables.Count > 0)
                {
                    resultDataSet.Tables[0].TableName = "InventoryDet";
                    return resultDataSet;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
        }

        #endregion
    }
}
