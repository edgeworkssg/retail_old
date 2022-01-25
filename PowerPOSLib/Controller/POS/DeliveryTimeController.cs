using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using PowerPOS.Container;

namespace PowerPOS
{
    public class DeliveryTimeController
    {
        public static bool LoadDeliveryTimes(out string status)
        {
            try
            {
                status = "";
                if (PointOfSaleInfo.DeliveryTimes == null)
                {
                    string path = AppDomain.CurrentDomain.BaseDirectory + "\\DeliveryTime.xml";
                    if (File.Exists(path))
                    {
                        DataSet ds = new DataSet();
                        ds.ReadXml(path);
                        PointOfSaleInfo.DeliveryTimes = ds.Tables[0];
                    }
                    else
                    {
                        DataTable dt = new DataTable();
                        dt.Columns.Add("Value", Type.GetType("System.String"));
                        dt.Columns.Add("Text", Type.GetType("System.String"));
                        dt.Rows.Add("10:00-13:00", "10am - 1pm");
                        dt.Rows.Add("13:00-15:00", "1pm - 3pm");
                        dt.Rows.Add("15:00-17:00", "3pm - 5pm");
                        PointOfSaleInfo.DeliveryTimes = dt;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return false;
            }
        }
    }
}
