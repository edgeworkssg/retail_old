using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using PowerPOS;
using ERPIntegration.API;

namespace ERPIntegration.API.ACCPAC
{
    public class Accpac:IERPIntegration
    {
        private List<string> arguments;
        private bool useFTP;
        private string localDirectory;
        private string serverDirectory;

        public Accpac(List<string> args)
        {
            this.arguments = args;
            useFTP = AppSetting.CastBool(ConfigurationManager.AppSettings["UseFTP"], false);
            localDirectory = ConfigurationManager.AppSettings["LocalDirectory"];
            serverDirectory = ConfigurationManager.AppSettings["ServerDirectory"];
        }

        #region IERPIntegration Members

        public bool DoStockTake(out string status)
        {
            if (preAction(out status))
            {
                if (useFTP)
                {                    
                    FTP.StartTransfer("download", localDirectory, serverDirectory);
                }

                // Process the files
                Do_Stock_Take.DoStockTake(localDirectory);
                status = "done";
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ExportOrderPayment(out string status)
        {           

            if (preAction(out status))
            {
                DateTime orderDate = DateTime.Today;

                string ar = arguments.Find(s => s.ToLower().StartsWith("-date:"));
                if (!string.IsNullOrEmpty(ar))
                {
                    DateTime tmpDate;
                    if (DateTime.TryParse(ar.Remove(0, 6), out tmpDate))
                    {
                        orderDate = tmpDate;
                    }
                    else
                    {
                        //Helper.WriteLog("Invalid date provided in arguments.", MessageType.ERROR);
                        status = "Invalid date provided in arguments.";
                        return false;
                    }
                }

                // Generate the xls files
                Export_Order_Payment.DoExport(localDirectory, orderDate);

                if (useFTP)
                {
                    string[] localDirs = localDirectory.Split('|');
                    string[] serverDirs = serverDirectory.Split('|');
                    for (int i = 0; i < localDirs.Length; i++)
                    {
                        FTP.StartTransfer("upload", localDirs[i], (i < serverDirs.Length) ? serverDirs[i] : serverDirs[serverDirs.Length - 1]);
                    }
                }
                status = "done";
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ExportInventory(out string status)
        {
            status = "no implementation";
            return true;
        }

        public bool ImportInventory(out string status)
        {
            status = "no implementation";
            return true;
        }

        public bool ImportItem(out string status)
        {
            if (preAction(out status))
            {
                if (useFTP)
                {
                    FTP.StartTransfer("download", localDirectory, serverDirectory);
                }

                // Process the files
                status = "Done";
                return Import_Item.DoImport(localDirectory);
            }
            else
            {
                return false;
            }
        }

        public bool ImportMember(out string status)
        {
            if (useFTP)
            {
                FTP.StartTransfer("download", localDirectory, serverDirectory);
            }


            // Process the files
            status = "Done";
            return Do_Stock_Take.DoStockTake(localDirectory);
        }

        public bool Connect(out string status)
        {
            status = "no implementation";
            return true;
        }

        public bool checkStatus(out string status)
        {
            status = "no implementation";
            return true;
        }

        private bool preAction(out string status)
        {
            if (string.IsNullOrEmpty(localDirectory))
            {
                status = "Configuration for 'LocalDirectory' is not found.";
                //Helper.WriteLog("Configuration for 'LocalDirectory' is not found.", MessageType.ERROR);
                return false;
            }

            string serverDirectory = ConfigurationManager.AppSettings["ServerDirectory"];
            if (string.IsNullOrEmpty(serverDirectory))
            {
                status = "Configuration for 'ServerDirectory' is not found.";
                //Helper.WriteLog("Configuration for 'ServerDirectory' is not found.", MessageType.ERROR);
                return false;
            }
            status = "ok";
            return true;
        }


        public bool ExportOrderRpt(out string status)
        {
            if (string.IsNullOrEmpty(localDirectory))
            {
                //Helper.WriteLog("Configuration for 'LocalDirectory' is not found.", MessageType.ERROR);
                status= "Configuration for 'LocalDirectory' is not found.";
                return false;
            }
            DateTime orderDate = DateTime.Today;

            string ar = arguments.Find(s => s.ToLower().StartsWith("-date:"));
            if (!string.IsNullOrEmpty(ar))
            {
                DateTime tmpDate;
                if (DateTime.TryParse(ar.Remove(0, 6), out tmpDate))
                {
                    orderDate = tmpDate;
                }
                else
                {
                    //Helper.WriteLog("Invalid date provided in arguments.", MessageType.ERROR);
                    status= "Invalid date provided in arguments.";
                    return false;
                }
            }

            // Generate the xls files
            Export_Order_Payment.DoExportRpt(localDirectory, orderDate);

            if (useFTP)
            {
                string[] localDirs = localDirectory.Split('|');
                string[] serverDirs = serverDirectory.Split('|');
                for (int i = 0; i < localDirs.Length; i++)
                {
                    FTP.StartTransfer("upload", localDirs[i], (i < serverDirs.Length) ? serverDirs[i] : serverDirs[serverDirs.Length - 1]);
                }
            }
            status = "";
            return true;
        }

        public bool ExportGoodsOrder(out string status)
        {
            status = "no implementation";
            return true;
        }

        public bool ExportStockReturn(out string status)
        {
            status = "no implementation";
            return true;
        }

        public bool ImportOrderApproval(out string status)
        {
            status = "no implementation";
            return true;
        }

        #endregion
    }
}
