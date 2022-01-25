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
using System.Reflection;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace PowerPOS
{
    public static partial class Extension
    {
        public static bool IsEqual(this string val, string comparation)
        {
            bool isSame = false;

            isSame = (val + "").ToLower().Trim().Equals((comparation + "").ToLower().Trim());

            return isSame;
        }

        public static string ConvertToJSON(this object obj)
        {
            //var serializer = new JavaScriptSerializer();
            //serializer.RegisterConverters(new [] { new NullPropertiesConverter() });
            //return serializer.Serialize(obj);
            var result = JsonConvert.SerializeObject(obj,
                                        Newtonsoft.Json.Formatting.None,
                                        new JsonSerializerSettings
                                        {
                                            NullValueHandling = NullValueHandling.Ignore
                                        });
            return result;
        }

        public static T ConvertFromJSON<T>(this string json)
        {
            try
            {
                var serializer = new JavaScriptSerializer();

                return serializer.Deserialize<T>(json);
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }

        public static Guid ConvertToGuid(this string txt)
        {
            Guid result = Guid.Empty;

            try
            {
                result = new Guid(txt);
            }
            catch (Exception ex)
            {

            }

            return result;
        }

        public static void InitFromDictionary(this object obj, Dictionary<string, object> data)
        {
            try
            {
                PropertyInfo[] properties = obj.GetType().GetProperties();

                foreach (PropertyInfo property in properties)
                {
                    try
                    {
                        if (!data.Any(x => x.Key.Equals(property.Name, StringComparison.InvariantCultureIgnoreCase)))
                            continue;
                        KeyValuePair<string, object> item = data.First(x => x.Key.Equals(property.Name, StringComparison.InvariantCultureIgnoreCase));
                        Type tPropertyType = obj.GetType().GetProperty(property.Name).PropertyType;
                        Type newT = Nullable.GetUnderlyingType(tPropertyType) ?? tPropertyType;
                        object newA = Convert.ChangeType(item.Value, newT);
                        obj.GetType().GetProperty(property.Name).SetValue(obj, newA, null);
                    }
                    catch (Exception exx)
                    {

                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public static List<T> ConvertToList<T>(this DataTable dt)
        {
            var columnNames = dt.Columns.Cast<DataColumn>()
                    .Select(c => c.ColumnName)
                    .ToList();
            var properties = typeof(T).GetProperties();
            return dt.AsEnumerable().Select(row =>
            {
                var objT = Activator.CreateInstance<T>();
                foreach (var pro in properties)
                {
                    if (columnNames.Contains(pro.Name))
                    {
                        try
                        {
                            PropertyInfo pI = objT.GetType().GetProperty(pro.Name);
                            Type t = Nullable.GetUnderlyingType(pI.PropertyType) ?? pI.PropertyType;
                            pro.SetValue(objT, row[pro.Name] == DBNull.Value ? null : Convert.ChangeType(row[pro.Name], t), null);
                        }
                        catch (Exception ex)
                        {
                            //just skip when failed
                        }
                    }
                }
                return objT;
            }).ToList();
        }
    }
}
