using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Xml;
using System.Collections.Generic;
using System.Windows.Forms.Layout;
using System.Web;
using System.Collections;
using System.Text;
//using ItemImporter.Controller;

namespace PowerPOS
{
    public class ExcelDataLogic
    {
        public static DataTable OpenExcelFile(string InputPath, string[] ColumnNames, string[] ColumnTypes, out string Status)
        {
            Status = "";

            try
            {
                string ErrorMessage = "(warning)Some error occured." + Environment.NewLine + "@ErrDesc";
                string InputURI;

                #region *) Validation: Check Input Parameter
                if (ColumnNames.Length != ColumnTypes.Length)
                    throw new Exception(ErrorMessage.Replace("@ErrDesc", "Column Types doesn't match with the Column Names").ToString());
                #endregion

                #region *) Validation: File must exist
                if (File.Exists(InputPath))
                {
                    FileInfo Info = new FileInfo(InputPath);

                    if ((Info.Extension.ToLower().CompareTo(".xls") == 0) || (Info.Extension.ToLower().CompareTo(".csv") == 0))
                        InputURI = InputPath;
                    else
                        throw new Exception(ErrorMessage.Replace("@ErrDesc", "File format doesn't match - Need Excel file").ToString());
                }
                else
                    throw new Exception(ErrorMessage.Replace("@ErrDesc", "File doesn't exist").ToString());
                #endregion

                DataTable ExcelRawData;
                #region *) Core: Load Excel Datas to ExcelRawData
                OleDbConnection ExcelConnection = new OleDbConnection(@"Provider=Microsoft.Jet.OleDb.4.0;Data Source=" + InputURI + ";Extended Properties=\"Excel 8.0;HDR=YES;IMEX=1;\"");
                OleDbCommand ExcelCommand = new OleDbCommand("SELECT * FROM [Sheet1$]", ExcelConnection);
                OleDbDataReader ExcelReader;

                ExcelConnection.Open();
                ExcelReader = ExcelCommand.ExecuteReader(CommandBehavior.CloseConnection);

                ExcelRawData = new DataTable("ExcelData");
                ExcelRawData.Load(ExcelReader);
                #endregion
                #region *) Validation: Fix Columns' Name
                foreach (string Counter in ColumnNames)
                {
                    if (ExcelRawData.Columns.Contains(Counter.Replace(" ", "")))
                        ExcelRawData.Columns[Counter.Replace(" ", "")].ColumnName = Counter;
                    else if (ExcelRawData.Columns.Contains(Counter.Replace(" ", "_")))
                        ExcelRawData.Columns[Counter.Replace(" ", "_")].ColumnName = Counter;
                }
                #endregion

                #region *) Repair: Add required columns
                for (int Counter = 0; Counter < ColumnNames.Length; Counter++)
                {
                    if (!ExcelRawData.Columns.Contains(ColumnNames[Counter]))
                        ExcelRawData.Columns.Add(ColumnNames[Counter], Type.GetType(ColumnTypes[Counter]));
                }
                #endregion

                #region *) Repair: Remove unused columns
                for (int Counter = 0; Counter < ExcelRawData.Columns.Count; Counter++)
                {
                    if (!ColumnNames.Contains(ExcelRawData.Columns[Counter].ColumnName))
                    {
                        ExcelRawData.Columns.RemoveAt(Counter);
                        Counter--;
                    }
                }
                #endregion

                #region *) Repair: Resorting columns
                for (int Counter = 0, Fixer = 0; Counter < ColumnNames.Length; Counter++, Fixer++)
                {
                    if (ExcelRawData.Columns.Contains(ColumnNames[Counter]))
                    {
                        ExcelRawData.Columns[ColumnNames[Counter]].SetOrdinal(Fixer);
                    }
                    else
                    {
                        Fixer--;
                        continue;
                    }

                }
                #endregion

                #region *) Repair: Fixed Boolean value
                //for (int Counter = 0; Counter < ColumnNames.Length; Counter++)
                //{
                //    ExcelRawData.Columns[ColumnNames[Counter]].DataType = Type.GetType(ColumnTypes[Counter]);
                //}
                #endregion

                #region *) Repair: Fixed Boolean value
                if (ColumnTypes.Contains("System.Boolean"))
                {
                    foreach (DataRow Rw in ExcelRawData.Rows)
                    {
                        for (int Counter = 0; Counter < ColumnTypes.Length; Counter++)
                        {
                            if (ColumnTypes[Counter] == "System.Boolean")
                            {
                                if (Rw[ColumnNames[Counter]].ToString().ToUpper() == "YES") Rw[ColumnNames[Counter]] = "True";
                                if (Rw[ColumnNames[Counter]].ToString().ToUpper() == "NO") Rw[ColumnNames[Counter]] = "False";
                            }
                        }
                    }
                }
                #endregion

                #region *) [NotAvailable]Repair: Remove null rows =-
                //for (int Counter = 0; Counter < ExcelRawData.Rows.Count; Counter++)
                //{
                //    DataRow Rw = ExcelRawData.Rows[Counter];
                //    if (string.IsNullOrEmpty(Rw["Category"].ToString().Trim()) &&
                //        string.IsNullOrEmpty(Rw["Item Name"].ToString().Trim()))
                //    {
                //        ExcelRawData.Rows.Remove(Rw);
                //        Counter--;
                //    }
                //}
                #endregion

                #region *) Informer: Any data is loaded?
                if (ExcelRawData.Rows.Count < 1)
                    throw new Exception(ErrorMessage.Replace("@ErrDesc", "No data is loaded").ToString());
                #endregion

                DataTable ExcelPolishedData = new DataTable("ExcelData");
                for (int Counter = 0; Counter < ColumnNames.Length; Counter++)
                {
                    ExcelPolishedData.Columns.Add(ColumnNames[Counter], Type.GetType(ColumnTypes[Counter]));
                }

                ExcelPolishedData.Load(ExcelRawData.CreateDataReader());

                return ExcelPolishedData;
            }
            catch (OleDbException X)
            {
                if (X.Message.Contains("is not a valid name."))
                    Status = "(warning)" + X.Message.Replace("$", "");
                else
                    Status = X.Message;

                return null;
            }
            catch (Exception X)
            {
                Status = X.Message;
                return null;
            }
        }

        // Upgraded Function (from OpenExcelFile)
        public static DataTable OpenExcelFile2(string InputPath, string SheetName, string[] ColumnNames, string[] ColumnTypes, out string Status)
        {
            Status = "";

            try
            {
                string ErrorMessage = "(warning)Some error occured." + Environment.NewLine + "@ErrDesc";
                string InputURI;

                #region *) Validation: Check Input Parameter
                if (ColumnNames.Length != ColumnTypes.Length)
                    throw new Exception(ErrorMessage.Replace("@ErrDesc", "Column Types doesn't match with the Column Names").ToString());
                #endregion

                #region *) Validation: File must exist
                if (File.Exists(InputPath))
                {
                    FileInfo Info = new FileInfo(InputPath);

                    if ((Info.Extension.ToLower().CompareTo(".xls") == 0) || (Info.Extension.ToLower().CompareTo(".csv") == 0))
                        InputURI = InputPath;
                    else
                        throw new Exception(ErrorMessage.Replace("@ErrDesc", "File format doesn't match - Need Excel file").ToString());
                }
                else
                    throw new Exception(ErrorMessage.Replace("@ErrDesc", "File doesn't exist").ToString());
                #endregion

                DataTable ExcelRawData;
                #region *) Core: Load Excel Datas to ExcelRawData
                OleDbConnection ExcelConnection = new OleDbConnection(@"Provider=Microsoft.Jet.OleDb.4.0;Data Source=" + InputURI + ";Extended Properties=Excel 8.0");
                OleDbCommand ExcelCommand = new OleDbCommand("SELECT * FROM [" + SheetName + "$]", ExcelConnection);
                OleDbDataReader ExcelReader;

                ExcelConnection.Open();
                ExcelReader = ExcelCommand.ExecuteReader(CommandBehavior.CloseConnection);

                ExcelRawData = new DataTable("ExcelData");
                ExcelRawData.Load(ExcelReader);
                #endregion
                #region *) Validation: Fix Columns' Name
                foreach (string Counter in ColumnNames)
                {
                    if (ExcelRawData.Columns.Contains(Counter.Replace(" ", "")))
                        ExcelRawData.Columns[Counter.Replace(" ", "")].ColumnName = Counter;
                    else if (ExcelRawData.Columns.Contains(Counter.Replace(" ", "_")))
                        ExcelRawData.Columns[Counter.Replace(" ", "_")].ColumnName = Counter;
                }
                #endregion

                #region *) Repair: Add required columns
                for (int Counter = 0; Counter < ColumnNames.Length; Counter++)
                {
                    if (!ExcelRawData.Columns.Contains(ColumnNames[Counter]))
                        ExcelRawData.Columns.Add(ColumnNames[Counter], Type.GetType(ColumnTypes[Counter]));
                }
                #endregion

                #region *) Repair: Remove unused columns
                for (int Counter = 0; Counter < ExcelRawData.Columns.Count; Counter++)
                {
                    if (!ColumnNames.Contains(ExcelRawData.Columns[Counter].ColumnName))
                    {
                        ExcelRawData.Columns.RemoveAt(Counter);
                        Counter--;
                    }
                }
                #endregion

                #region *) Repair: Resorting columns
                for (int Counter = 0, Fixer = 0; Counter < ColumnNames.Length; Counter++, Fixer++)
                {
                    if (ExcelRawData.Columns.Contains(ColumnNames[Counter]))
                    {
                        ExcelRawData.Columns[ColumnNames[Counter]].SetOrdinal(Fixer);
                    }
                    else
                    {
                        Fixer--;
                        continue;
                    }

                }
                #endregion

                #region *) Repair: Fixed Boolean value
                if (ColumnTypes.Contains("System.Boolean"))
                {
                    foreach (DataRow Rw in ExcelRawData.Rows)
                    {
                        for (int Counter = 0; Counter < ColumnTypes.Length; Counter++)
                        {
                            if (ColumnTypes[Counter] == "System.Boolean")
                            {
                                if (Rw[ColumnNames[Counter]].ToString().ToUpper() == "YES") Rw[ColumnNames[Counter]] = "True";
                                if (Rw[ColumnNames[Counter]].ToString().ToUpper() == "NO") Rw[ColumnNames[Counter]] = "False";
                            }
                        }
                    }
                }
                #endregion

                #region *) [NotAvailable]Repair: Remove null rows =-
                //for (int Counter = 0; Counter < ExcelRawData.Rows.Count; Counter++)
                //{
                //    DataRow Rw = ExcelRawData.Rows[Counter];
                //    if (string.IsNullOrEmpty(Rw["Category"].ToString().Trim()) &&
                //        string.IsNullOrEmpty(Rw["Item Name"].ToString().Trim()))
                //    {
                //        ExcelRawData.Rows.Remove(Rw);
                //        Counter--;
                //    }
                //}
                #endregion

                #region *) Informer: Any data is loaded?
                if (ExcelRawData.Rows.Count < 1)
                    throw new Exception(ErrorMessage.Replace("@ErrDesc", "No data is loaded").ToString());
                #endregion

                DataTable ExcelPolishedData = new DataTable("ExcelData");
                for (int Counter = 0; Counter < ColumnNames.Length; Counter++)
                {
                    ExcelPolishedData.Columns.Add(ColumnNames[Counter], Type.GetType(ColumnTypes[Counter]));
                }

                ExcelPolishedData.Load(ExcelRawData.CreateDataReader());

                return ExcelPolishedData;
            }
            catch (Exception X)
            {
                Status = X.Message;
                return null;
            }
        }

        public static bool ExportToCSV(DataTable dt, string filepath)
        {
            if (dt == null)
            {
                return false;
            }
            StreamWriter f = File.CreateText(filepath);
            int i;
            for (i = 0; i < dt.Columns.Count - 1; i++)
            {
                //if (dt.Columns[i].ColumnName != "UniqueID") 
                f.Write("\"" + dt.Columns[i].ColumnName + "\",");
            }
            f.WriteLine("\"" + dt.Columns[i].ColumnName + "\"");

            for (int j = 0; j < dt.Rows.Count; ++j)
            {

                for (i = 0; i < dt.Columns.Count - 1; i++)
                {
                    //if (dt.Columns[i].ColumnName != "UniqueID") 
                    f.Write("\"" + dt.Rows[j][i].ToString().Replace(",", "") + "\",");
                }
                f.WriteLine("\"" + dt.Rows[j][i].ToString().Replace(",", "") + "\"");
            }
            f.Close();
            return false;
        }

        public static bool ImportFromExcel(string filepath, out DataTable dt, bool hasColumnHeader)
        {
            dt = null;
            DataSet ds = ExcelController.ImportExcelXML(filepath, hasColumnHeader, false);

            return true;
        }

        public static List<string> OpenExcelFileGetColumnHeaderList(string InputPath, out string Status)
        {
            try
            {
                List<string> result = new List<string>();

                string ErrorMessage = "(warning)Some error occured." + Environment.NewLine + "@ErrDesc";

                string InputURI;
                #region *) Validation: File must exist
                if (File.Exists(InputPath))
                {
                    FileInfo Info = new FileInfo(InputPath);

                    if ((Info.Extension.ToLower().CompareTo(".xls") == 0) || (Info.Extension.ToLower().CompareTo(".csv") == 0))
                        InputURI = InputPath;
                    else
                        throw new Exception(ErrorMessage.Replace("@ErrDesc", "File format doesn't match - Need Excel file").ToString());
                }
                else
                    throw new Exception(ErrorMessage.Replace("@ErrDesc", "File doesn't exist").ToString());
                #endregion

                DataTable ExcelRawData;
                #region *) Core: Load Excel Datas to ExcelRawData
                OleDbConnection ExcelConnection = new OleDbConnection(@"Provider=Microsoft.Jet.OleDb.4.0;Data Source=" + InputURI + ";Extended Properties=\"Excel 8.0;HDR=YES;IMEX=1;\"");
                OleDbCommand ExcelCommand = new OleDbCommand("SELECT TOP 1 * FROM [Sheet1$]", ExcelConnection);
                OleDbDataReader ExcelReader;

                ExcelConnection.Open();
                ExcelReader = ExcelCommand.ExecuteReader(CommandBehavior.CloseConnection);
                ExcelRawData = new DataTable("ExcelData");
                ExcelRawData.Load(ExcelReader);
                #endregion

                foreach (DataColumn eachColumn in ExcelRawData.Columns)
                {
                    result.Add(eachColumn.Caption);
                }

                Status = string.Empty;
                return result;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                Status = ex.Message;
                return null;
            }
        }
    }
}