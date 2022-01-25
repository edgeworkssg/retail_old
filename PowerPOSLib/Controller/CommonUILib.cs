using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Collections;
using System.IO;

namespace PowerPOS
{
    public class CommonUILib
    {
        public static void ShowErrorMessage(string msg)
        {
            MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
        }
        public static DataTable DataTableConvertBoolColumnToYesOrNo(DataTable dt)
        {
            ArrayList booleanCols = new ArrayList();
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                if (dt.Columns[i].DataType == System.Type.GetType("System.Boolean"))
                {
                    booleanCols.Add(i);
                }
            }

            for (int i = 0; i < booleanCols.Count; i++)
            {
                dt.Columns.Add(dt.Columns[(int)booleanCols[i]].ColumnName + "Str");
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < booleanCols.Count; j++)
                {

                    string colName = dt.Columns[(int)booleanCols[j]].ColumnName + "Str";
                    if (dt.Rows[i][(int)booleanCols[j]] is bool)
                    {
                        dt.Rows[i][colName] = GetYesOrNoFromBool((bool)dt.Rows[i][(int)booleanCols[j]]);
                    }
                    else
                    {
                        dt.Rows[i][colName] = "No"; //treat null as no...
                    }
                }
            }
            for (int i = 0; i < booleanCols.Count; i++)
            {
                string colName = dt.Columns[(int)booleanCols[i]].ColumnName;
                string newCol = dt.Columns[(int)booleanCols[i]].ColumnName + "Str";
                dt.Columns.Remove(colName);
                dt.Columns[newCol].SetOrdinal((int)booleanCols[i]);
                dt.Columns[(int)booleanCols[i]].ColumnName = colName;
            }

            return dt;
        }
        public static bool ShowAreYouSure()
        {
            DialogResult dr = MessageBox.Show("Are you sure you want to proceed?", "", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public static frmTransparent shadow;
        public static void displayTransparent()
        {
            if (shadow == null || shadow.IsDisposed)
            {
                shadow = new frmTransparent();
                shadow.Show();
            }
            shadow.Visible = true;
            shadow.BringToFront();
        }
        public static void hideTransparent()
        {
            if (shadow == null) shadow = new frmTransparent();
            shadow.Visible = false;
        }
        public static decimal RemoveRoundUp(decimal d)
        {
            if (d.ToString().LastIndexOf('.') != -1 &&
                d.ToString().LastIndexOf('.') + 3 < d.ToString().Length)
            {
                return decimal.Parse
                    (d.ToString().Substring(0,
                    d.ToString().LastIndexOf('.') + 3));
            }
            return d;
        }
        public static void FormatDateFilter(ref DateTimePicker dtpStartDate, ref DateTimePicker dtpEndDate)
        {
            dtpStartDate.Value = DateTime.Now;
            dtpStartDate.Value = dtpStartDate.Value.AddSeconds(-dtpStartDate.Value.TimeOfDay.TotalSeconds + 1);

            //begin from end date 23:59:59
            dtpEndDate.Value = DateTime.Now;
            dtpEndDate.Value = dtpEndDate.Value.AddSeconds(86399 - dtpEndDate.Value.TimeOfDay.TotalSeconds);

            return;
        }
        
        public static string GetYesOrNoFromBool(bool BoolVar)
        {
            if (BoolVar)
                return "Yes";
            else
                return "No";
        }

        public static bool ValidateTextBoxAsUnsignedNumeric(TextBox txt, out int result)
        {            
            return (int.TryParse(txt.Text, out result) && result >= 0);
        }

        public static bool ValidateTextBoxAsUnsignedNumericAllowNegative(TextBox txt, out int result)
        {
            return (int.TryParse(txt.Text, out result));
        }

        public static bool ValidateTextBoxAsUnsignedDecimal(TextBox txt, out decimal result)
        {
            return (decimal.TryParse(txt.Text, out result) && result >= 0);
        }

        public static bool ValidateTextBoxAsUnsignedDecimalAllowNegative(TextBox txt, out decimal result)
        {
            return (decimal.TryParse(txt.Text, out result));
        }

        public static void HandleException(Exception X)
        {
            if (X.Message.StartsWith("(error)"))
            { MessageBox.Show(X.Message.Replace("(error)", ""), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            else if (X.Message.StartsWith("(warning)"))
            { MessageBox.Show(X.Message.Replace("(warning)", ""), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
            else
            {
                Logger.writeLog(X);
                MessageBox.Show("Some error happened and we have logged it.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Converts a given delimited file into a dataset. 
        /// Assumes that the first line    
        /// of the text file contains the column names.
        /// </summary>
        /// <param name="File">The name of the file to open</param>    
        /// <param name="TableName">The name of the 
        /// Table to be made within the DataSet returned</param>
        /// <param name="delimiter">The string to delimit by</param>
        /// <returns></returns>  
        public static DataSet Convert(Stream f,
         string TableName, string delimiter)
        {
            //The DataSet to Return
            DataSet result = new DataSet();

            //Open the file in a stream reader.
            StreamReader s = new StreamReader(f);

            //Split the first line into the columns       
            string[] columns = s.ReadLine().Split(delimiter.ToCharArray());

            //Add the new DataTable to the RecordSet
            result.Tables.Add(TableName);

            //Cycle the colums, adding those that don't exist yet 
            //and sequencing the one that do.
            foreach (string col in columns)
            {
                bool added = false;
                string next = "";
                int i = 0;
                while (!added)
                {
                    //Build the column name and remove any unwanted characters.
                    string columnname = col + next;
                    columnname = columnname.Replace("#", "");
                    columnname = columnname.Replace("'", "");
                    columnname = columnname.Replace("&", "");

                    //See if the column already exists
                    if (!result.Tables[TableName].Columns.Contains(columnname))
                    {
                        //if it doesn't then we add it here and mark it as added
                        result.Tables[TableName].Columns.Add(columnname);
                        added = true;
                    }
                    else
                    {
                        //if it did exist then we increment the sequencer and try again.
                        i++;
                        next = "_" + i.ToString();
                    }
                }
            }

            //Read the rest of the data in the file.        
            string AllData = s.ReadToEnd();

            //Split off each row at the Carriage Return/Line Feed
            //Default line ending in most windows exports.  
            //You may have to edit this to match your particular file.
            //This will work for Excel, Access, etc. default exports.
            string[] rows = AllData.Split("\r\n".ToCharArray());

            //Now add each row to the DataSet        
            foreach (string r in rows)
            {
                if (r != "")
                {
                    //Split the row at the delimiter.
                    string[] items = r.Split(delimiter.ToCharArray());

                    //Add the item
                    result.Tables[TableName].Rows.Add(items);
                }
            }

            //Return the imported data.        
            return result;
        }

    }
}
