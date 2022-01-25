using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using PowerPOS;
using ERPIntegration.API;
using ERPIntegration.API.ACCPAC;
using ERPIntegration.API.MONEYWORKS;
using ERPIntegration.API.SYNERGIX;
using ERPIntegration.API.NAVISION;

namespace ERPIntegration
{
    class Program
    {
        struct MessageType
        {
            public static string INFO = "ERP_INTEGRATION_INFO";
            public static string ERROR = "ERP_INTEGRATION_ERROR";
        }

        static void Main(string[] args)
        {
            Helper.WriteLog("Application started.", MessageType.INFO);

            string[] availableActions = { 
                "EXPORT_ORDER_PAYMENT", 
                "EXPORT_ORDER_RPT", 
                "EXPORT_INVENTORY",
                "IMPORT_INVENTORY",
                "IMPORT_ITEM", 
                "IMPORT_MEMBER",
                "DO_STOCK_TAKE",
                "EXPORT_GOODS_ORDER",
                "EXPORT_STOCK_RETURN",
                "IMPORT_ORDER_APPROVAL"
            };

            string type = ConfigurationManager.AppSettings["Integration_Type"];
            string action = ConfigurationManager.AppSettings["Integration_Action"];
            if (string.IsNullOrEmpty(action))
            {
                Helper.WriteLog("Configuration for 'Integration_Action' is not found.", MessageType.ERROR);
                return;
            }
            else if (!Array.Exists(availableActions, s => s.ToUpper().Trim() == action.ToUpper().Trim()))
            {
                Helper.WriteLog("Incorrect configuration for 'Integration_Action'. Choose one of the following: " + Environment.NewLine + string.Join(Environment.NewLine, availableActions), MessageType.ERROR);
                return;
            }

            string status="";
            bool statusBool = false;
            IERPIntegration erp;
            if (!createIntegrationObject(args, type, out erp))
            {
                Helper.WriteLog("Incorrect configuration for 'Integration_Type'.", MessageType.ERROR);
                return;
            }

            if (action == "EXPORT_ORDER_PAYMENT")
            {
                statusBool = erp.ExportOrderPayment(out status);
            }
            else if (action == "EXPORT_ORDER_RPT")
            {
                statusBool = erp.ExportOrderRpt(out status);                
            }
            else if (action == "EXPORT_INVENTORY")
            {
                statusBool = erp.ExportInventory(out status);
            }
            else if (action == "DO_STOCK_TAKE")
            {
                statusBool = erp.DoStockTake(out status);                
            }
            else if (action == "IMPORT_MEMBER")
            {
                statusBool = erp.ImportMember(out status);
            }
            else if (action == "IMPORT_ITEM")
            {
                statusBool = erp.ImportItem(out status);
            }
            else if (action == "IMPORT_INVENTORY")
            {
                statusBool = erp.ImportInventory(out status);
            }
            else if (action == "IMPORT_MEMBER")
            {
                statusBool = erp.ImportMember(out status);
            }
            else if (action == "EXPORT_GOODS_ORDER")
            {
                statusBool = erp.ExportGoodsOrder(out status);
            }
            else if (action == "EXPORT_STOCK_RETURN")
            {
                statusBool = erp.ExportStockReturn(out status);
            }
            else if (action == "IMPORT_ORDER_APPROVAL")
            {
                statusBool = erp.ImportOrderApproval(out status);
            }

            if (statusBool)
                Helper.WriteLog("Application completed.", MessageType.INFO);
            else
                Helper.WriteLog("Application completed. "+ status, MessageType.ERROR);
        }

        public static bool createIntegrationObject(string[] args,string type, out IERPIntegration result)
        {
            bool resultBool=true;
            List<string> arguments = new List<string>(args); ;
            switch (type)
            {
                case "MONEYWORKS":
                    result= new Moneyworks();break;
                case "SYNERGIX":
                    result = new Synergix(); break;
                case "ACCPAC":
                    result = new Accpac(arguments);
                    break;
                case "NAVISION":
                    result = new Navision(arguments); break;
                default:
                    result = null;
                    resultBool = false; break;
            }
            return resultBool;
        }
    }

    

}
