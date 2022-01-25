using System;
using System.Data;
using System.Data.OleDb;
using System.Collections.Generic;
using System.IO;

namespace PowerPOS
{
    public partial class ExcelController
    {
        public static bool ImportExcelXLS(string FileName, out DataTable Message, bool withColumnHeader)
        {
            Message = null;

            DataSet Result = new DataSet();
            if (!ImportExcelXLS(FileName, out Result, withColumnHeader))
                return false;

            if (Result == null || Result.Tables.Count < 1)
                throw new Exception("(error)No sheet is found. Please check the Excel file.");

            if (Result.Tables.Contains("Sheet1"))
                Message = Result.Tables["Sheet1"];
            else
                Message = Result.Tables[0];

            return true;
        }

        public static bool ImportExcelXLS(string FileName, out DataSet Message, bool withColumnHeader)
        {
            Message = null;

            string HDR = withColumnHeader ? "Yes" : "No";
            string strConn;
            if (Path.GetExtension(FileName).ToLower() == ".xls")
                strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FileName + ";Extended Properties=\"Excel 8.0;HDR=" + HDR + ";IMEX=1\"";
            else
                strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + FileName + ";Extended Properties=\"Excel 12.0 Xml;HDR=" + HDR.ToUpper() + "\"";

            DataSet output = new DataSet();

            using (OleDbConnection conn = new OleDbConnection(strConn))
            {
                conn.Open();

                DataTable dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });

                List<string> addedSheet = new List<string>();
                foreach (DataRow row in dt.Rows)
                {
                    string SheetName = "";
                    try
                    {
                        SheetName = row["TABLE_NAME"].ToString().Replace("'", "").Replace("$", "");

                        if (addedSheet.Contains(SheetName)) continue;

                        OleDbCommand cmd = new OleDbCommand("SELECT * FROM [" + SheetName + "$]", conn);
                        cmd.CommandType = CommandType.Text;

                        DataTable outputTable = new DataTable(SheetName);
                        output.Tables.Add(outputTable);
                        new OleDbDataAdapter(cmd).Fill(outputTable);
                        addedSheet.Add(SheetName);
                    }
                    catch
                    {
                        if (output.Tables.Contains(SheetName))
                            output.Tables.Remove(SheetName);
                        if (addedSheet.Contains(SheetName))
                            addedSheet.Remove(SheetName);
                    }
                }
            }
            Message = output;
            return true;
        }

        public static bool ImportExcelCSV(string filepath, out DataTable dt, bool withColumnHeader)
        {
            DataTable importedData = new DataTable();
            dt = null;

            #region *) CSV -> DataTable
            try
            {
                char[] IMPORT_DELIMITER = { ',' };

                string line;
                string[] splitted;
                // Read the file and display it line by line.
                using (System.IO.StreamReader file =
                    new System.IO.StreamReader(filepath))
                {
                    if (withColumnHeader)
                    {
                        if (!string.IsNullOrEmpty(line = file.ReadLine()))
                        {
                            splitted = line.Split(IMPORT_DELIMITER);

                            foreach (string strCounter in splitted)
                                importedData.Columns.Add(strCounter.Replace("\"", ""), Type.GetType("System.String"));
                        }
                        else
                        {
                            return false;
                            /// Please handle this scenario..
                            /// In case the first line on CVS is null
                        }
                    }
                    else
                    {
                        //return false;
                        /// Please handle this scenario..
                        /// In case the first line on CVS is null
                    }

                    int rowNumber = 0;
                    while ((line = file.ReadLine()) != null)
                    {
                        rowNumber += 1;

                        splitted = line.Replace("\"", "").Split(IMPORT_DELIMITER/*, importedData.Columns.Count*/);
                        
                        if (splitted.Length > importedData.Columns.Count)
                        {
                            for (int Counter = importedData.Columns.Count; Counter < splitted.Length; Counter++)
                            {
                                importedData.Columns.Add("Column" + Counter.ToString("N0").PadLeft(2, '0'));
                            }
                        }

                        importedData.Rows.Add(splitted);
                    }
                    dt = importedData;
                }
                return true;
            }
            catch
            { return false; }
            #endregion
        }


        public static bool ImportExcelCSVWithDelimiter(char delimiter, string filepath, out DataTable dt, bool withColumnHeader)
        {
            DataTable importedData = new DataTable();
            dt = null;

            #region *) CSV -> DataTable
            try
            {
                char[] IMPORT_DELIMITER = { delimiter };

                string line;
                string[] splitted;
                // Read the file and display it line by line.
                using (System.IO.StreamReader file =
                    new System.IO.StreamReader(filepath))
                {
                    if (withColumnHeader)
                    {
                        if (!string.IsNullOrEmpty(line = file.ReadLine()))
                        {
                            splitted = line.Split(IMPORT_DELIMITER);

                            foreach (string strCounter in splitted)
                                importedData.Columns.Add(strCounter.Replace("\"", ""), Type.GetType("System.String"));
                        }
                        else
                        {
                            return false;
                            /// Please handle this scenario..
                            /// In case the first line on CVS is null
                        }
                    }
                    else
                    {
                        //return false;
                        /// Please handle this scenario..
                        /// In case the first line on CVS is null
                    }

                    int rowNumber = 0;
                    while ((line = file.ReadLine()) != null)
                    {
                        rowNumber += 1;

                        splitted = line.Replace("\"", "").Split(IMPORT_DELIMITER/*, importedData.Columns.Count*/);

                        if (splitted.Length > importedData.Columns.Count)
                        {
                            for (int Counter = importedData.Columns.Count; Counter < splitted.Length; Counter++)
                            {
                                importedData.Columns.Add("Column" + Counter.ToString("N0").PadLeft(2, '0'));
                            }
                        }

                        importedData.Rows.Add(splitted);
                    }
                    dt = importedData;
                }
                return true;
            }
            catch(Exception ex)
            {
                Logger.writeLog(ex);
                return false; 
            }
            #endregion
        }
    }
}
