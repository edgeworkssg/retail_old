using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using PowerPOS;
using SubSonic;

namespace PowerWeb
{
    public class SyncTables
    {
        public static bool UploadTable(DataTable table, string primaryKey, string updateKey, bool isPKAutoGenerate, out string status)
        {
            bool isSuccess = false;
            status = "";

            try
            {
                QueryCommandCollection qmc = new QueryCommandCollection();
                for (int row = 0; row < table.Rows.Count; row++)
                {
                    string sqlCheckExist = string.Format("SELECT TOP 1 {0} FROM {1} WHERE {0}='{2}'", updateKey, table.TableName, table.Rows[row][updateKey]);
                    DataTable dtCheckExist = new DataTable();
                    dtCheckExist.Load(DataService.GetReader(new QueryCommand(sqlCheckExist)));
                    bool isUpdate = (dtCheckExist.Rows.Count > 0);
                    if (isUpdate)
                    {
                        if (table.Columns.Contains("ModifiedOn"))
                        {
                            sqlCheckExist = string.Format("SELECT CAST(CASE WHEN ModifiedOn > '{3}' THEN 1 ELSE 0 END AS BIT) IsSkip FROM {1} WHERE {0}='{2}'",
                                updateKey, table.TableName, table.Rows[row][updateKey], table.Rows[row]["ModifiedOn"]);
                            dtCheckExist = new DataTable();
                            dtCheckExist.Load(DataService.GetReader(new QueryCommand(sqlCheckExist)));
                            bool isSkip = false;
                            if (dtCheckExist.Rows.Count > 0)
                                isSkip = (bool)dtCheckExist.Rows[0]["IsSkip"];
                            if (isSkip) continue;
                        }
                    }


                    List<string> insertColDef = new List<string>();
                    List<string> insertValDef = new List<string>();
                    List<string> updateColDef = new List<string>();
                    for (int col = 0; col < table.Columns.Count; col++)
                    {
                        string val = "";

                        if (string.IsNullOrEmpty(table.Rows[row][col].ToString()))
                            val = "NULL";
                        else
                            val = string.Format("'{0}'", table.Rows[row][col].ToString());

                        if (!isUpdate)
                        {
                            if (!isPKAutoGenerate ||
                                primaryKey.ToLower() != table.Columns[col].ColumnName.ToLower())
                            {
                                insertColDef.Add(table.Columns[col].ColumnName);
                                insertValDef.Add(val);
                            }
                        }
                        else
                        {
                            if (updateKey.ToLower() != table.Columns[col].ColumnName.ToLower())
                            {
                                string update = string.Format("{0} = {1}"
                                                , table.Columns[col].ColumnName
                                                , val);
                                updateColDef.Add(update);
                            }
                        }
                    }
                    string sqlScript = "";
                    if (isUpdate)
                    {



                        sqlScript = "UPDATE {0} SET {1} WHERE {2}='{3}'";
                        sqlScript = string.Format(sqlScript,
                                                  table.TableName,
                                                  string.Join(",", updateColDef.ToArray()),
                                                  updateKey,
                                                  table.Rows[row][updateKey].ToString());
                    }
                    else
                    {
                        sqlScript = "INSERT INTO {0} ({1}) VALUES ({2})";
                        sqlScript = string.Format(sqlScript, 
                                                  table.TableName, 
                                                  string.Join(",", insertColDef.ToArray()), 
                                                  string.Join(",", insertValDef.ToArray()));
                    }
                    qmc.Add(new QueryCommand(sqlScript));
                }
                DataService.ExecuteTransaction(qmc);
                isSuccess = true;
            }
            catch (Exception ex)
            {
                isSuccess = false;
                status = "ERROR : " + ex.Message;
                Logger.writeLog(ex);
            }

            return isSuccess;
        }
    }
}
