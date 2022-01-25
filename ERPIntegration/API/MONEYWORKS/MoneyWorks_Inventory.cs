using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;
using System.Xml;
using PowerPOS;
using PowerPOS.Container;
using System.Data;
using SubSonic;
using System.Net;

namespace ERPIntegration.API.MONEYWORKS
{
    public static class MoneyWorks_Inventory
    {
        struct MessageType
        {
            public static string INFO = "STOCK_TAKE_INFO";
            public static string ERROR = "STOCK_TAKE_ERROR";
        }

        public static bool DoSyncInventory(out string status)
        {
            //getXmlByTable("product", "");//use to grab product data
            string paramSearch = "location=\"" + ConfigurationManager.AppSettings["Location"] + "\"";
            if (getXmlByTable("inventory", paramSearch,out status))
            {
                return updateInventory();
            }
            else
            {
                return false;
            }
        }

        public static bool updateInventory()
        {
            bool result = true;
            string filePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\table.xml";
            XmlDocument xml = new XmlDocument();
            xml.Load(filePath);
            XmlNodeList inventories = xml.DocumentElement.SelectNodes("/table/inventory");

            int invLocID = int.Parse(ConfigurationManager.AppSettings["LocationID"]);
            InventoryController invCtrl = new InventoryController();
            invCtrl = new InventoryController(InventorySettings.CostingMethod);
            invCtrl.SetInventoryLocation(invLocID);
            invCtrl.SetInventoryDate(DateTime.Now);
            invCtrl.SetRemark("Stock Take from ERPIntegration");

            if (inventories.Count > 0)
            {
                Console.WriteLine("Found {0} inventory",inventories.Count);
                Logger.writeLog(string.Format("Found {0} inventory", inventories.Count));
                foreach (XmlNode inv in inventories)
                {
                    string status = "";
                    string seq = inv["productseq"].InnerText;
                    string qty = inv["qty"].InnerText;
                    decimal qtyd = decimal.Parse(qty);
                    string itemcode = getItemCodeBySeq(seq);
                    bool r= invCtrl.AddItemIntoInventoryUsingAltCost(itemcode, qtyd, false, out status);
                    if (r)
                    {
                        Console.WriteLine("{0} was added", itemcode);
                        Logger.writeLog(string.Format("{0} was added", itemcode));
                    }
                    else
                    {
                        Console.WriteLine("{0} fail added", itemcode);
                        Logger.writeLog(string.Format("{0} fail added", itemcode));
                    }
                }

                // Create Stock Take Entries
                string status2 = "";
                if (!invCtrl.CreateStockTakeEntries("SYSTEM", "SYSTEM", "SYSTEM", true, out status2))
                {
                    Helper.WriteLog("Error encountered while creating stock take entries: " + status2, MessageType.ERROR);
                    //return false;
                }
                Console.WriteLine("CreateStockTakeEntries done");
                Logger.writeLog("CreateStockTakeEntries done");

                // Adjust Stock Take
                StockTakeController ct = new StockTakeController();
                if (!ct.CorrectStockTakeDiscrepancy("SYSTEM", invLocID))
                {
                    Helper.WriteLog("Error encountered while Adjusting Stock Discrepancy. Please contact your system administrator.", MessageType.ERROR);
                    //return false;
                }
                Console.WriteLine("CorrectStockTakeDiscrepancy done");
                Logger.writeLog("CorrectStockTakeDiscrepancy done");
            }
            return result;
        }

        public static string getItemCodeBySeq(string seq)
        {
            string sql = "SELECT ITEMNO FROM ITEM WHERE Attributes6='{0}'";
            sql = string.Format(sql, seq);
            DataTable dt = DataService.GetDataSet(new QueryCommand(sql)).Tables[0];
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["ITEMNO"].ToString();
            }
            else
                return "";
        }

        public static bool getXmlByTable(string tablename, string paramsearch,out string status)
        {
            bool resultBool = false;
            string result = "";            
            string xmlString = "";
            string baseUrl = "";
            
            Console.WriteLine("Get inventory data");
            Logger.writeLog("Get inventory data");

            if (ConfigurationManager.AppSettings["SSL"] == "true")
                baseUrl = "https://";
            else
                baseUrl = "http://";

            baseUrl += ConfigurationManager.AppSettings["Server"] + ":"
                + ConfigurationManager.AppSettings["Port"] + "/Rest/"
                + ConfigurationManager.AppSettings["Username"] + ":"
                + ConfigurationManager.AppSettings["Password"] + "@"
                + System.Uri.EscapeUriString(ConfigurationManager.AppSettings["Document"])
                + "/export/table=" + tablename + "&format=xml-verbose"
                ;
            if (paramsearch != "")
            {
                baseUrl = baseUrl + "&search=" + System.Uri.EscapeDataString(paramsearch);
            }


            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(baseUrl);
            request.Method = "GET";

            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream responseStream = response.GetResponseStream();
                    string responseStr = new StreamReader(responseStream).ReadToEnd();
                    status = responseStr;
                    string filePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\table.xml";
                    File.WriteAllText(filePath, status);
                    Console.WriteLine("Get inventory success");
                    Logger.writeLog("Get inventory success");
                    resultBool = true;
                }
                else
                {
                    Console.WriteLine("Get inventory empty result");
                    Logger.writeLog("Get inventory empty result");
                    status = "query inventory empty result";
                    resultBool = false;
                }
            }
            catch (Exception e)
            {
                status = e.Message;
                Console.WriteLine("Get inventory faile, {0}",status);
                Logger.writeLog(string.Format("Get inventory faile, {0}", status));
                resultBool = false;
            }
            return resultBool;
        }    
    }
}
