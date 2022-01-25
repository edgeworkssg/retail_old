using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Transactions;
using System.Collections;
using PowerPOS.Container;
using SubSonic;
using System.ComponentModel;
using System.Net.Mail;
using System.Globalization;
using System.Linq.Expressions;
using System.Linq;

namespace PowerPOS
{
    public static partial class Extension
    {
        public const string DATE_FORMAT_SHORT = "yyyy-MM-dd";

        public const string DATE_FORMAT_LONG = "yyyy-MM-dd HH:mm:ss";

        public const string DATE_FORMAT_LONG_TICK = "yyyy-MM-dd HH:mm:ss.fff";

        public const string DATE_FORMAT_DISPLAY = "d MMM yyyy h:mm:ss tt";

        public const string DATE_FORMAT_DISPLAY_SHORT = "d MMM yyyy";

        /// <summary>
        /// This is an extension method for sorting List object
        /// </summary>
        public static List<T> Sort<T>(this List<T> list, string sortBy, string sortDirection)
        {
            var param = Expression.Parameter(typeof(T), "item");

            var parts = sortBy.Split('.');
            Expression parent = param;

            foreach (var part in parts)
            {
                parent = Expression.Property(parent, part);
            }

            var sortExpression = Expression.Lambda<Func<T, object>>
                (Expression.Convert(parent, typeof(object)), param);

            switch (sortDirection.ToLower())
            {
                case "asc":
                    list = list.AsQueryable<T>().OrderBy<T, object>(sortExpression).ToList();
                    break;
                default:
                    list = list.AsQueryable<T>().OrderByDescending<T, object>(sortExpression).ToList();
                    break;
            }
            return list;
        }

        /// <summary>
        /// This is an extension method for sorting IEnumerable object
        /// </summary>
        public static IEnumerable<T> Sort<T>(this IEnumerable<T> list, string sortBy, string sortDirection)
        {
            var param = Expression.Parameter(typeof(T), "item");

            var parts = sortBy.Split('.');
            Expression parent = param;

            foreach (var part in parts)
            {
                parent = Expression.Property(parent, part);
            }

            var sortExpression = Expression.Lambda<Func<T, object>>
                (Expression.Convert(parent, typeof(object)), param);

            switch (sortDirection.ToLower())
            {
                case "asc":
                    list = list.AsQueryable<T>().OrderBy<T, object>(sortExpression);
                    break;
                default:
                    list = list.AsQueryable<T>().OrderByDescending<T, object>(sortExpression);
                    break;
            }
            return list;
        }

        public static string AsJSArray(this DataTable data)
        {
            string retVal = "[";
            for (int i = 0; i < data.Columns.Count; i++)
                retVal += string.Format("'{0}',", data.Columns[i].ColumnName);
            retVal = retVal.Remove(retVal.LastIndexOf(',')) + "],";

            for (int i = 0; i < data.Rows.Count; i++)
            {
                string rowData = "[";
                for (int j = 0; j < data.Columns.Count; j++)
                {
                    if (j == 0)
                    {
                        rowData += string.Format("'{0}',", (data.Rows[i][j]).ToString());
                    }
                    else
                    {
                        rowData += string.Format("{0},", (data.Rows[i][j]).ToString());
                    }
                }
                rowData = rowData.Remove(rowData.LastIndexOf(',')) + "]";
                retVal += (rowData + ",");
            }
            retVal = retVal.Remove(retVal.LastIndexOf(','));
            return retVal;
        }

        public static string AsJSArrayForTable(this DataTable data)
        {
            string retVal = "[";
            for (int i = 0; i < data.Columns.Count; i++)
                retVal += string.Format("'{0}',", data.Columns[i].ColumnName);
            retVal = retVal.Remove(retVal.LastIndexOf(',')) + "],";

            for (int i = 0; i < data.Rows.Count; i++)
            {
                string rowData = "[";
                for (int j = 0; j < data.Columns.Count; j++)
                {
                    if (data.Columns[j].DataType == typeof(int) ||
                        data.Columns[j].DataType == typeof(decimal) ||
                        data.Columns[j].DataType == typeof(double) ||
                        data.Columns[j].DataType == typeof(float) ||
                        data.Columns[j].DataType == typeof(long) ||
                        data.Columns[j].DataType == typeof(short))
                    {
                        rowData += string.Format("{0},", (data.Rows[i][j]).ToString());
                    }
                    else if (data.Columns[j].DataType == typeof(DateTime))
                    {
                        rowData += string.Format("'{0}',", ((DateTime)data.Rows[i][j]).ToString("dd-MM-yyyy HH:mm"));
                    }
                    else
                    {
                        rowData += string.Format("'{0}',", (data.Rows[i][j]).ToString());
                    }
                }
                rowData = rowData.Remove(rowData.LastIndexOf(',')) + "]";
                retVal += (rowData + ",");
            }
            retVal = retVal.Remove(retVal.LastIndexOf(','));
            return retVal;
        }

        public static DataTable Transpose(this DataTable inputTable)
        {
            DataTable outputTable = new DataTable();

            // Add columns by looping rows

            // Header row's first column is same as in inputTable
            outputTable.Columns.Add(inputTable.Columns[0].ColumnName.ToString());

            // Header row's second column onwards, 'inputTable's first column taken
            foreach (DataRow inRow in inputTable.Rows)
            {
                string newColName = inRow[0].ToString();
                outputTable.Columns.Add(newColName);
            }

            // Add rows by looping columns        
            for (int rCount = 1; rCount <= inputTable.Columns.Count - 1; rCount++)
            {
                DataRow newRow = outputTable.NewRow();

                // First column is inputTable's Header row's second column
                newRow[0] = inputTable.Columns[rCount].ColumnName.ToString();
                for (int cCount = 0; cCount <= inputTable.Rows.Count - 1; cCount++)
                {
                    string colValue = inputTable.Rows[cCount][rCount].ToString();
                    newRow[cCount + 1] = colValue;
                }
                outputTable.Rows.Add(newRow);
            }

            return outputTable;
        }

        public static decimal Normalize(this decimal value)
        {
            return value / 1.000000000000000000000000000000000m;
        }

        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            PropertyDescriptorCollection properties =
                TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }

        public static List<string> SelectColumnAsList(this DataTable data, int colIndex)
        {
            List<string> retVal = new List<string>();

            try
            {
                for (int i = 0; i < data.Rows.Count; i++)
                    retVal.Add((data.Rows[i][colIndex] + ""));
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
            return retVal;
        }

        public static DataTable DeleteRow(this DataTable data, string column, object value)
        {
            var result = data.Copy();
            for (int i = result.Rows.Count - 1; i >= 0; i--)
            {
                DataRow dr = result.Rows[i];
                if (dr[column].ToString() == value.ToString())
                    dr.Delete();
            }
            return result;
        }

        public static DataTable RemoveExisting(this DataTable val, string tableName, string primaryKey)
        {
            var data = val.Copy();
            for (int i = 0; i < val.Rows.Count; i++)
            {
                string ops = val.Rows[i]["SYS_CHANGE_OPERATION"] + "";
                object pkValue = val.Rows[i][primaryKey];
                string query = string.Format("SELECT TOP 1 {0} AS ID FROM {1} WHERE {0}='{2}'", primaryKey, tableName, pkValue);
                DataTable pkData = new DataTable();
                pkData.Load(SubSonic.DataService.GetReader(new SubSonic.QueryCommand(query, "PowerPOS")));
                if (pkData.Rows.Count > 0 && ops.ToLower().Equals("i"))
                    data.Rows[i]["SYS_CHANGE_OPERATION"] = "U";
            }

            return data;
        }

        public static DateTime GetDateTimeValue(this string val, string format)
        {
            if (string.IsNullOrEmpty(format))
                format = "yyyy-MM-dd";

            DateTime dt = DateTime.Now;
            if (!DateTime.TryParseExact(val, format,
                                          CultureInfo.InvariantCulture,
                                          DateTimeStyles.None,
                                          out dt))
            {
                if (!DateTime.TryParseExact(val, format,
                                              CultureInfo.CurrentCulture,
                                              DateTimeStyles.None,
                                              out dt))
                {
                    dt = DateTime.Now;
                }
            }
            return dt;
        }

        public static DateTime GetDateValueOfLongString(this string txt)
        {
            DateTime result = DateTime.Now;

            if (!DateTime.TryParseExact(txt, DATE_FORMAT_LONG, CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
            {
                DateTime.TryParseExact(txt, DATE_FORMAT_LONG_TICK, CultureInfo.InvariantCulture, DateTimeStyles.None, out result);
            }

            return result;
        }

        public static string GetStringValue(this decimal val, string format)
        {
            bool isUseWeight = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Invoice.UseWeight), false);
            string result = val.ToString("N0");
            if (isUseWeight)
                result = val.ToString(format);
            return result;
        }

        public static string Left(this string str, int length)
        {
            if (string.IsNullOrEmpty(str))
                str = "";
            return str.Substring(0, Math.Min(length, str.Length));
        }

        public static string Right(this string str, int length)
        {
            str = (str ?? string.Empty);
            return (str.Length >= length)
                ? str.Substring(str.Length - length, length)
                : str;
        }

        public static decimal GetDecimalValue(this string val)
        {
            decimal result = 0;
            //decimal.TryParse(val, out result);
            decimal.TryParse(val, NumberStyles.Any, new CultureInfo("en-US"), out result);
            return result;
        }

        public static decimal? GetDecimalNullValue(this string val)
        {
            decimal? result = null;

            decimal res = 0;
            if (decimal.TryParse(val, out res))
                result = res;
            return result;
        }

        public static int GetIntValue(this string val)
        {
            int result = 0;
            int.TryParse(val, out result);
            return result;
        }

        public static int GetIntValue(this decimal? val)
        {
            return val.GetValueOrDefault(0).GetIntValue();
        }

        public static int GetIntValue(this decimal val)
        {
            int result = 0;

            try
            {
                result = Convert.ToInt32(val);
            }
            catch (Exception ex)
            {

            }

            return result;
        }

        public static bool GetBoolValue(this string val, bool defaultVal)
        {
            bool result = defaultVal;

            if (val.ToUpper().Equals("YES") || val.ToUpper().Equals("TRUE") || val.ToUpper().Equals("1"))
                result = true;

            return result;
        }

        public static string StringToBinary(this string data)
        {
            StringBuilder sb = new StringBuilder();

            foreach (char c in data.ToCharArray())
            {
                sb.Append(Convert.ToString(c, 2).PadLeft(8, '0'));
            }
            return sb.ToString();
        }

        public static string BinaryToString(this string data)
        {
            List<Byte> byteList = new List<Byte>();

            for (int i = 0; i < data.Length; i += 8)
            {
                byteList.Add(Convert.ToByte(data.Substring(i, 8), 2));
            }
            return Encoding.ASCII.GetString(byteList.ToArray());
        }

        public static bool IsValidEmail(this string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public static string AsSingleLineString(this string[] array, string separator)
        {
            string result = "";
            if (array != null)
                result = string.Join(separator, array);
            return result;
        }

        public static double GetDoubleValue(this string txt)
        {
            double val = 0;
            double.TryParse(txt, out val);
            return val;
        }
        public static int GetInt32Value(this string txt)
        {
            int val = 0;
            int.TryParse(txt, out val);
            return val;
        }

        public static int GetInt32Value(this string txt, int defaultVal)
        {
            int val = defaultVal;
            if (!int.TryParse(txt, out val))
                val = defaultVal;
            return val;
        }
        public static PurchaseOrderDetCollection SortByRefNo(this PurchaseOrderDetCollection coll)
        {
            try
            {
                PurchaseOrderDetCollection tmp = new PurchaseOrderDetCollection();
                DataTable dt = coll.ToDataTable();
                dt.Columns.Add("OrderSeq", typeof(Int32));

                for (int i = 0; i < coll.Count; i++)
                {
                    int seq = 0;
                    if (!string.IsNullOrEmpty(coll[i].PurchaseOrderDetRefNo))
                    {
                        var splitid = coll[i].PurchaseOrderDetRefNo.Split('.');
                        int.TryParse(splitid[1], out seq);
                    }
                    dt.Rows[i]["OrderSeq"] = seq;
                }

                DataView dv = dt.DefaultView;
                dv.Sort = "OrderSeq ASC";

                tmp.Load(dv.ToTable());
                return tmp;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return coll;
            }
        }
    }
}
