using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic;
using System.Data;

namespace PowerPOS
{
    public partial class RetailerLevelController
    {
        public static List<string> FetchMallName()
        {
            List<string> retVal = new List<string>();

            try
            {
                string sql = @"SELECT  DISTINCT ISNULL(Userfld1,'') MallName
                                FROM	RetailerLevel 
                                WHERE	ISNULL(Deleted,0) = 0 
                                        AND ISNULL(Userfld1,'') <> ''
                                ORDER BY MallName";
                DataTable dt = new DataTable();
                dt.Load(DataService.GetReader(new QueryCommand(sql)));
                if (dt.Columns.Count > 0)
                    retVal = dt.SelectColumnAsList(0);
                //else
                //    retVal.Add("NO-MALLNAME");
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }

            return retVal;
        }

        public static List<string> FetchRetailerLevel(string mallName)
        {
            List<string> retVal = new List<string>();

            try
            {
                string sql = @"SELECT DISTINCT ShopLevel 
                                FROM	RetailerLevel 
                                WHERE	ISNULL(Deleted,0) = 0 
                                        AND (ISNULL(Userfld1,'') = N'{0}' OR N'{0}' = 'ALL')
                                ORDER BY ShopLevel";
                sql = string.Format(sql, mallName);
                DataTable dt = new DataTable();
                dt.Load(DataService.GetReader(new QueryCommand(sql)));
                if (dt.Columns.Count > 0)
                    retVal = dt.SelectColumnAsList(0);
                //else
                //    retVal.Add("NO-LEVEL");
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }

            return retVal;
        }

        public static List<string> FetchRetailerLevel()
        {
            List<string> retVal = new List<string>();

            try
            {
                string sql = @"SELECT DISTINCT ShopLevel 
                                FROM	RetailerLevel 
                                WHERE	ISNULL(Deleted,0) = 0 
                                ORDER BY ShopLevel";
                DataTable dt = new DataTable();
                dt.Load(DataService.GetReader(new QueryCommand(sql)));
                if (dt.Columns.Count > 0)
                    retVal = dt.SelectColumnAsList(0);
                //else
                //    retVal.Add("NO-LEVEL");
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }

            return retVal;
        }

        public static List<string> FetchShopNo()
        {
            List<string> retVal = new List<string>();

            try
            {
                string sql =  @"SELECT  ShopLevel, ShopNo 
                                FROM	RetailerLevel 
                                WHERE	ISNULL(Deleted,0) = 0 
                                ORDER BY ShopLevel, ShopNo";
                DataTable dt = new DataTable();
                dt.Load(DataService.GetReader(new QueryCommand(sql)));
                if (dt.Columns.Count > 0)
                    retVal = dt.SelectColumnAsList(1);
                //else
                //    retVal.Add("NO-SHOP");
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }

            return retVal;
        }

        public static List<string> FetchShopNo(string mallName, string retailerLevel)
        {
            List<string> retVal = new List<string>();

            try
            {
                string sql = @"SELECT  ShopLevel, ShopNo 
                                FROM	RetailerLevel 
                                WHERE	ISNULL(Deleted,0) = 0 
		                                AND ShopLevel = '{0}'
                                        AND ISNULL(Userfld1,'') = '{1}'
                                ORDER BY ShopLevel, ShopNo";
                sql = string.Format(sql, retailerLevel, mallName);
                DataTable dt = new DataTable();
                dt.Load(DataService.GetReader(new QueryCommand(sql)));
                if (dt.Columns.Count > 0)
                    retVal = dt.SelectColumnAsList(1);
                //else
                //    retVal.Add("NO-SHOP");
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }

            return retVal;
        }
    }
}
