using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PowerPOS;
using System.Configuration;
using System.IO;
using SubSonic;
using System.Data;

namespace ERPIntegration.API.NAVISION
{
    public static class Navision_DO
    {
        const byte COL_order_ref = 3;
        const byte COL_Delivery_Posted_Date = 13;
        const byte COL_so_remarks = 14;
        const byte COL_qty_shipped = 17;
        const byte COL_line_remark = 19;
        const byte COL_original_doc_line_no = 20;

        struct MessageType
        {
            public static string INFO = "NAV_DO_INFO";
            public static string ERROR = "NAV_DO_ERROR";
        }

        private static void ImportOrder(FileInfo[] files, bool autoStockIn)
        {
            Console.WriteLine();
            Helper.WriteLog("Importing csv file...", MessageType.INFO);

            for (int i = 0; i < files.Length; i++)
            {
                FileInfo file = files[i];

                Helper.WriteLog(string.Format("Processing file #{0} : {1}.", (i + 1).ToString(), file.Name), MessageType.INFO);

                DataTable dt;
                if (ExcelController.ImportExcelCSVWithDelimiter(',', file.FullName, out dt, false))
                {
                    List<string> strCommand = new List<string>();
                    List<string> uniqueRefNo = new List<string>();
                    DateTime timestamp = DateTime.Now;

                    foreach (DataRow dr in dt.Rows)
                    {
                        string poRefNo = dr[COL_order_ref] + "";
                        if (!uniqueRefNo.Contains(poRefNo))
                            uniqueRefNo.Add(poRefNo);

                        string strDate = dr[COL_Delivery_Posted_Date] + "";
                        DateTime approvedDate;
                        if (!DateTime.TryParse(strDate, out approvedDate))
                        {
                            approvedDate = timestamp;
                        }

                        string headerRemark = dr[COL_so_remarks] + "";

                        string strQty = dr[COL_qty_shipped] + "";
                        decimal approvedQty;
                        if (!decimal.TryParse(strQty, out approvedQty))
                        {
                            throw new Exception(string.Format("Invalid number value on column {0}", (COL_qty_shipped + 1).ToString()));
                        }

                        string lineRemark = dr[COL_line_remark] + "";

                        string strLineNo = dr[COL_original_doc_line_no] + "";
                        string detID = poRefNo + "." + strLineNo;

                        string sqlHdr = string.Format("UPDATE PurchaseOrderHeader SET userfld3 = '{0}', userfld4 = 'WMS', Remark = '{1}' WHERE PurchaseOrderHeaderRefNo = '{2}'",
                                                        approvedDate.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                                                        headerRemark,
                                                        poRefNo);
                        if (!strCommand.Contains(sqlHdr))
                            strCommand.Add(sqlHdr);

                        string sqlDet = @"
                                            UPDATE POD 
                                            SET POD.userint1 = CASE WHEN DI.ItemNo IS NULL THEN {0} ELSE {0} / ITM.userfloat5 END, 
                                                POD.Remark = '{1}' 
                                            FROM PurchaseOrderDetail POD
                                                INNER JOIN Item ITM ON ITM.ItemNo = POD.ItemNo
                                                LEFT JOIN Item DI ON ITM.Userflag7 = 1 AND ITM.Userfld8 = DI.ItemNo
                                            WHERE POD.PurchaseOrderDetailRefNo = '{2}'
                                         ";
                        sqlDet = string.Format(sqlDet,
                                                approvedQty.ToString(),
                                                lineRemark,
                                                detID);
                        if (!strCommand.Contains(sqlDet))
                            strCommand.Add(sqlDet);
                    }

                    foreach (string refNo in uniqueRefNo)
                    {
                        string sqlStatus = @"
                                                UPDATE PurchaseOrderDetail SET userint1 = 0 WHERE PurchaseOrderHeaderRefNo = '{0}' AND userint1 IS NULL

                                                UPDATE PurchaseOrderDetail 
                                                SET userfld1 = CASE WHEN userint1 > 0 THEN 'Approved' ELSE 'Rejected' END,
                                                    ModifiedOn = GETDATE(),
                                                    ModifiedBy = 'WMS'
                                                WHERE PurchaseOrderHeaderRefNo = '{0}'

                                                UPDATE PurchaseOrderHeader
                                                SET userfld1 = CASE WHEN EXISTS(SELECT * FROM PurchaseOrderDetail WHERE PurchaseOrderHeaderRefNo = '{0}' AND userfld1 = 'Approved') THEN 'Approved' ELSE 'Rejected' END,
                                                    ModifiedOn = GETDATE(),
                                                    ModifiedBy = 'WMS'
                                                WHERE PurchaseOrderHeaderRefNo = '{0}'
                                            ";
                        sqlStatus = string.Format(sqlStatus, refNo);
                        strCommand.Add(sqlStatus);
                    }

                    QueryCommandCollection cmdColl = new QueryCommandCollection();
                    foreach (string sql in strCommand)
                    {
                        cmdColl.Add(new QueryCommand(sql, "PowerPOS"));
                    }

                    DataService.ExecuteTransaction(cmdColl);

                    if (autoStockIn)
                    {
                        cmdColl.Clear();
                        foreach (string refNo in uniqueRefNo)
                        {
                            PurchaseOrderController poCtrl = new PurchaseOrderController(refNo);
                            if (poCtrl != null && poCtrl.GetPOHeader().PurchaseOrderHeaderRefNo == refNo)
                            {
                                QueryCommandCollection tmpColl;
                                if (PurchaseOrderController.StockInFromPurchaseOrderHeader(poCtrl, "NAV_DO", 0, poCtrl.GetInventoryLocationID(), false, false, "", out tmpColl))
                                    cmdColl.AddRange(tmpColl);
                            }
                        }

                        DataService.ExecuteTransaction(cmdColl);
                    }

                    // Move file to archive folder
                    string newdir = Path.Combine(file.Directory.FullName, "Archive");
                    if (File.Exists(Path.Combine(newdir, file.Name)))
                        File.Delete(Path.Combine(newdir, file.Name));
                    file.MoveTo(Path.Combine(newdir, file.Name));
                }
                else
                {
                    string newdir = Path.Combine(file.Directory.FullName, "Failed");
                    if (File.Exists(Path.Combine(newdir, file.Name)))
                        File.Delete(Path.Combine(newdir, file.Name));
                    file.MoveTo(Path.Combine(newdir, file.Name));
                    throw new Exception(string.Format("Fail to read the {0} file. Please check the log for more information.", file.Name));
                }
            }

            Helper.WriteLog("Importing csv file completed successfully.", MessageType.INFO);

        }

        public static bool Import_NAV_DO(List<string> arguments, out string status)
        {
            // POS_DO - SO Shipment Confirm

            status = "";
            bool result = false;

            try
            {
                string localDirectory = ConfigurationManager.AppSettings["LocalDirectory"];
                bool autoStockIn = AppSetting.CastBool(ConfigurationManager.AppSettings["NAV_DO_AutoStockIn"], false);

                Helper.WriteLog("Starting Import SO Shipment module", MessageType.INFO);

                DirectoryInfo fileDir = new DirectoryInfo(localDirectory);
                if (!fileDir.Exists)
                {
                    Helper.WriteLog(string.Format("Directory {0} does not exist.", localDirectory), MessageType.ERROR);
                    return false;
                }

                DirectoryInfo dirArchive = new DirectoryInfo(Path.Combine(localDirectory, "Archive"));
                if (!dirArchive.Exists)
                {
                    dirArchive.Create();
                }
                DirectoryInfo dirFailed = new DirectoryInfo(Path.Combine(localDirectory, "Failed"));
                if (!dirFailed.Exists)
                {
                    dirFailed.Create();
                }

                var files = fileDir.GetFiles("DO*.csv");

                Helper.WriteLog(string.Format("Found {0} file(s) to import.", files.Length), MessageType.INFO);

                if (files.Length > 0)
                {
                    ImportOrder(files, autoStockIn);
                }

                result = true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
            }

            return result;
        }
    }
}
