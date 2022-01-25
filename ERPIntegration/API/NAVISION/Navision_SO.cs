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
    public static class Navision_SO
    {
        struct MessageType
        {
            public static string INFO = "NAV_SO_INFO";
            public static string ERROR = "NAV_SO_ERROR";
        }

        private static PurchaseOrderHeaderCollection GetOrderHeaders(DateTime lastModifiedOn)
        {
            string sql = @"
                            SELECT PurchaseOrderHeaderRefNo, ModifiedOn 
                            FROM PurchaseOrderHeader 
                            WHERE ModifiedOn > @LastMod 
                                AND userfld1 = 'Submitted' 
                                AND userfld2 = 'Replenish'
                            ORDER BY ModifiedOn
                          ";
            QueryCommand cmd = new QueryCommand(sql, "PowerPOS");
            cmd.Parameters.Add("@LastMod", lastModifiedOn, DbType.DateTime);

            PurchaseOrderHeaderCollection orderColl = new PurchaseOrderHeaderCollection();
            orderColl.Load(DataService.GetDataSet(cmd).Tables[0]);

            return orderColl;
        }

        private static void GenerateOrder(PurchaseOrderHeaderCollection orderColl, string fileName, bool isSeparateOutputFolder)
        {
            Console.WriteLine();
            Helper.WriteLog("Generating csv file for Order...", MessageType.INFO);

            //List<string> OutputFileValue = new List<string>();
            DataTable OutputFileValue = new DataTable();
            OutputFileValue.Columns.Add("InventoryLocationName", typeof(String));
            OutputFileValue.Columns.Add("Value", typeof(String));
            DateTime lastMod = DateTime.Now;

            for (int i = 0; i < orderColl.Count; i++)
            {
                Helper.WriteLog(string.Format("Processing order #{0} : {1}.", (i + 1).ToString(), orderColl[i].PurchaseOrderHeaderRefNo), MessageType.INFO);
                string sql = @"
                                SELECT WH.InventoryLocationName,
                                       'Haisia' -- 1 owner_code
                                       + ',' + '""' + LEFT(REPLACE(WH.InventoryLocationName, '""', '\""'), 10) + '""' -- 2 whs_code
                                       + ',' -- 3 order_no
                                       + ',' + POH.PurchaseOrderHeaderRefNo -- 4 order_ref
                                       + ',WO' + ISNULL(SUP.userfld3, '') -- 5 trans code
                                       + ',' + '""' + LEFT(REPLACE(CUST.InventoryLocationName, '""', '\""'), 10) + '""' -- 6 cust_code
                                       + ',' + '""' + LEFT(REPLACE(ISNULL(CUST.userfld1, ''), '""', '\""'), 128) + '""' -- 7 cust_name
                                       + ',' + '""' + LEFT(REPLACE(ISNULL(CUST.userfld2, ''), '""', '\""'), 128) + '""' -- 8 delivery address1
                                       + ',' + '""' + LEFT(REPLACE(ISNULL(CUST.userfld3, ''), '""', '\""'), 128) + '""' -- 9 delivery address2
                                       + ',' + '""' + LEFT(REPLACE(ISNULL(CUST.userfld4, ''), '""', '\""'), 128) + '""' -- 10 delivery address3
                                       + ',NA' -- 11 delivery_route
                                       + ',' + CONVERT(varchar(50), POH.PurchaseOrderDate, 103) -- 12 order_date
                                       + ',' + CONVERT(varchar(50), POH.DateNeededBy, 103) -- 13 requested_delivery_date
                                       + ',' + '""' + LEFT(REPLACE(REPLACE(REPLACE(ISNULL(POH.Remark, ''), '""', '\""'), CHAR(13), ''), CHAR(10), ''), 256) + '""' -- 14 so_remarks
                                       + ',' + '""' + LEFT(REPLACE(CASE WHEN DI.ItemNo IS NULL THEN ITM.Attributes1 ELSE DI.Attributes1 END, '""', '\""'), 25) + '""' -- 15 prod_code
                                       + ',' -- 16 Child_code
                                       + ',' -- 17 Buying Qty
                                       + ',' -- 18 Buying UOM
                                       + ',' + CONVERT(varchar(max), CONVERT(decimal(18, 5), CASE WHEN DI.ItemNo IS NULL THEN POD.Quantity ELSE POD.Quantity * ITM.userfloat5 END)) -- 19 qty_ordered
                                       + ',' + '""' + LEFT(REPLACE(ISNULL(CASE WHEN DI.ItemNo IS NULL THEN ITM.Attributes2 ELSE DI.Attributes2 END, ''), '""', '\""'), 25) + '""' -- 20 ordered UoM
                                       + ',' + '""' + LEFT(REPLACE(REPLACE(REPLACE(ISNULL(POD.Remark, ''), '""', '\""'), CHAR(13), ''), CHAR(10), ''), 128) + '""' -- 21 line_remark
                                       + ',' + SUBSTRING(POD.PurchaseOrderDetailRefNo, CHARINDEX('.', POD.PurchaseOrderDetailRefNo) + 1, 99) -- 22 original_doc_line_no
                                FROM PurchaseOrderHeader POH
                                    INNER JOIN PurchaseOrderDetail POD ON POD.PurchaseOrderHeaderRefNo = POH.PurchaseOrderHeaderRefNo
                                    INNER JOIN Supplier SUP ON SUP.SupplierID = POH.SupplierID 
                                    INNER JOIN InventoryLocation WH ON WH.InventoryLocationID = SUP.userint2
                                    INNER JOIN InventoryLocation CUST ON CUST.InventoryLocationID = POH.InventoryLocationID
                                    INNER JOIN Item ITM ON ITM.ItemNo = POD.ItemNo
                                    LEFT JOIN Item DI ON ITM.Userflag7 = 1 AND ITM.Userfld8 = DI.ItemNo
                                WHERE POH.PurchaseOrderHeaderRefNo = '{0}' 
                              ";
                sql = string.Format(sql, orderColl[i].PurchaseOrderHeaderRefNo);
                DataTable dt = DataService.GetDataSet(new QueryCommand(sql, "PowerPOS")).Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        OutputFileValue.Rows.Add(dr[0] + "", dr[1] + "");
                    }
                }
                lastMod = orderColl[i].ModifiedOn.Value;
            }

            //if (OutputFileValue.Count > 0)
            //{
            //    #region *) Output: Create transaction file
            //    Helper.WriteLog("Saving csv file for Order...", MessageType.INFO);
            //    using (FileStream stream = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
            //    {
            //        TextWriter tw = new StreamWriter(stream);
            //        tw.Write(string.Join(Environment.NewLine, OutputFileValue.ToArray()));
            //        tw.Flush();
            //        tw.Close();
            //    }
            //    #endregion

            //    AppSetting.SetSetting("NAV_SO_LASTMODIFIEDON", lastMod.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            //}

            if (OutputFileValue.Rows.Count > 0)
            {
                #region *) Output: Create transaction file
                Helper.WriteLog("Saving csv file for Order...", MessageType.INFO);

                string newFileName = "";
                if (isSeparateOutputFolder)
                {
                    DataTable dtWH = OutputFileValue.DefaultView.ToTable(true, "InventoryLocationName");
                    foreach (DataRow drWH in dtWH.Rows)
                    {
                        // Put the file in different folder based on the InventoryLocationName
                        string dir = Path.Combine(Path.GetDirectoryName(fileName), "POS_SO_" + drWH[0]);
                        DirectoryInfo dirOutput = new DirectoryInfo(dir);
                        if (!dirOutput.Exists)
                        {
                            dirOutput.Create();
                        }

                        newFileName = Path.Combine(dir, Path.GetFileName(fileName));

                        using (FileStream stream = new FileStream(newFileName, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                        {
                            TextWriter tw = new StreamWriter(stream);
                            foreach (DataRow dr in OutputFileValue.Select("InventoryLocationName = '" + drWH[0] + "'"))
                            {
                                tw.Write(dr[1] + Environment.NewLine);
                            }
                            tw.Flush();
                            tw.Close();
                        }
                    }
                }
                else
                {
                    using (FileStream stream = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                    {
                        TextWriter tw = new StreamWriter(stream);
                        foreach (DataRow dr in OutputFileValue.Rows)
                        {
                            tw.Write(dr[1] + Environment.NewLine);
                        }
                        tw.Flush();
                        tw.Close();
                    }
                }
                
                #endregion

                AppSetting.SetSetting("NAV_SO_LASTMODIFIEDON", lastMod.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            }

            Helper.WriteLog("Generating csv file for Order completed successfully.", MessageType.INFO);
        }

        public static bool Generate_NAV_SO(List<string> arguments, out string status)
        {
            // NAV_SO - Warehouse Transfer

            status = "";
            bool result = false;

            try
            {
                string localDirectory = ConfigurationManager.AppSettings["LocalDirectory"];
                bool isSeparateOutputFolder = AppSetting.CastBool(ConfigurationManager.AppSettings["SeparateOutputFolderForEachWarehouse"], false);

                DateTime lastModifiedOn;
                string strModified = AppSetting.GetSetting("NAV_SO_LASTMODIFIEDON");
                if (string.IsNullOrEmpty(strModified))
                {
                    string ar = arguments.Find(s => s.ToLower().StartsWith("-date:"));
                    if (!string.IsNullOrEmpty(ar))
                    {
                        DateTime tmpDate;
                        if (DateTime.TryParse(ar.Remove(0, 6), out tmpDate))
                        {
                            lastModifiedOn = tmpDate;
                        }
                        else
                        {
                            status = "Invalid date provided in arguments.";
                            return false;
                        }
                    }
                    else
                    {
                        status = "No date provided in arguments.";
                        return false;
                    }
                }
                else
                {
                    DateTime tmpDate;
                    if (DateTime.TryParse(strModified, out tmpDate))
                    {
                        lastModifiedOn = tmpDate;
                    }
                    else
                    {
                        status = "Invalid date, please check NAV_SO_LASTMODIFIEDON value in AppSetting.";
                        return false;
                    }
                }

                Helper.WriteLog("Starting Export Warehouse Transfer module", MessageType.INFO);

                DirectoryInfo fileDir = new DirectoryInfo(localDirectory);
                if (!fileDir.Exists)
                {
                    Helper.WriteLog(string.Format("Directory {0} does not exist.", localDirectory), MessageType.ERROR);
                    return false;
                }

                PurchaseOrderHeaderCollection orderColl = GetOrderHeaders(lastModifiedOn);
                Helper.WriteLog(string.Format("Found {0} order(s) to export.", orderColl.Count), MessageType.INFO);

                if (orderColl.Count > 0)
                {
                    string orderFile = Path.Combine(fileDir.FullName, "SO_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".csv");
                    GenerateOrder(orderColl, orderFile, isSeparateOutputFolder);
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
