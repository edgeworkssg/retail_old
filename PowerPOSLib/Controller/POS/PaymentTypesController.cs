using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic;
using System.Data;
using PowerPOS.Container;

namespace PowerPOS
{
    public class PaymentTypesController
    {        
        public static bool LoadPaymentTypes(out string status)
        {
            try
            {
                status = "";
                if (PointOfSaleInfo.PaymentTypes == null)
                {                 
                    DataSet ds = new DataSet();
                    ds.ReadXml(AppDomain.CurrentDomain.BaseDirectory + "\\PaymentTypes.xml");
                    PointOfSaleInfo.PaymentTypes = ds.Tables[0];
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
        public static string FetchPaymentByID(string ID)
        {
            if (PointOfSaleInfo.PaymentTypes == null)
            {
                return "";
            }
            DataRow[] dr = PointOfSaleInfo.PaymentTypes.Select("ID = '" + ID.ToString() + "'");
            if (dr.Length > 0)
            {
                return dr[0]["Name"].ToString();
            }
            else
            {
                return "";
            }
        }
    }
}
