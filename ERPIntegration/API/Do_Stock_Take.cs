using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Configuration;
using PowerPOS;
using System.Data;
using SubSonic;
using PowerPOS.Container;

namespace ERPIntegration.API
{
    class Do_Stock_Take
    {
        struct MessageType
        {
            public static string INFO = "STOCK_TAKE_INFO";
            public static string ERROR = "STOCK_TAKE_ERROR";
        }

        public static bool DoStockTake(string directory)
        {
            bool proceed = true;
            string status;

            try
            {
                Helper.WriteLog("Starting Stock Take module", MessageType.INFO);

                DirectoryInfo fileDir = new DirectoryInfo(directory);
                DirectoryInfo backupDir;
                if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["StockTake_BackupDirectory"]))
                    backupDir = new DirectoryInfo(Path.Combine(directory, "backup"));
                else
                    backupDir = new DirectoryInfo(ConfigurationManager.AppSettings["StockTake_BackupDirectory"]);

                if (!fileDir.Exists)
                {
                    Helper.WriteLog("Directory does not exist.", MessageType.ERROR);
                    proceed = false;
                }

                if (!backupDir.Exists)
                {
                    try
                    {
                        backupDir.Create();
                    }
                    catch
                    {
                        Helper.WriteLog("Failed to create backup directory.", MessageType.ERROR);
                        proceed = false;
                    }
                }

                if (!proceed)
                {
                    return false;
                }
                else
                {
                    List<FileInfo> files = new List<FileInfo>(fileDir.GetFiles("*.xls"));
                    files = files.OrderBy(f => f.LastWriteTime).ToList();

                    Helper.WriteLog(string.Format("Found {0} file(s) to import.", files.Count.ToString()), MessageType.INFO);

                    if (files.Count > 0)
                    {
                        #region *) Assign Setting: Costing Method
                        string tmpCostingMethod = AppSetting.GetSetting(AppSetting.SettingsName.Inventory.CostingMethod);
                        if (string.IsNullOrEmpty(tmpCostingMethod))
                        {
                            tmpCostingMethod = InventoryController.CostingTypes.FIFO;
                        }
                        tmpCostingMethod = tmpCostingMethod.ToLower();
                        if (tmpCostingMethod == InventoryController.CostingTypes.FIFO)
                            InventorySettings.CostingMethod = CostingMethods.FIFO;
                        else if (tmpCostingMethod == InventoryController.CostingTypes.FixedAvg)
                            InventorySettings.CostingMethod = CostingMethods.FixedAvg;
                        else if (tmpCostingMethod == InventoryController.CostingTypes.WeightedAvg)
                            InventorySettings.CostingMethod = CostingMethods.WeightedAvg;
                        #endregion

                        for (int i = 0; i < files.Count; i++)
                        {
                            FileInfo file = files[i];

                            Console.WriteLine();
                            Helper.WriteLog(string.Format("Processing file #{0} : {1}.", (i + 1).ToString(), file.Name), MessageType.INFO);

                            DataTable dtResult = new DataTable();
                            bool importSuccess = ExcelController.ImportExcelXLS(file.FullName, out dtResult, true);

                            if (!importSuccess)
                            {
                                Helper.WriteLog(string.Format("Fail to import file {0} into DataTable.", file.Name), MessageType.ERROR);
                                return false;
                            }
                            else if (!dtResult.Columns.Contains("FMTITEMNO") || !dtResult.Columns.Contains("QTYONHAND") || !dtResult.Columns.Contains("LOCATION"))
                            {
                                Helper.WriteLog("Template is incorrect. Please make sure all required columns are present.", MessageType.ERROR);
                                return false;
                            }
                            else
                            {
                                Helper.WriteLog(string.Format("File {0} has been imported to DataTable successfully.", file.Name), MessageType.INFO);
                                Helper.WriteLog(string.Format("Found {0} record(s).", dtResult.Rows.Count.ToString()), MessageType.INFO);

                                DataView view = dtResult.DefaultView;
                                DataTable dtLocation = view.ToTable(true, "LOCATION");

                                foreach (DataRow drLoc in dtLocation.Rows)
                                {
                                    string locName = drLoc["LOCATION"].ToString();

                                    int invLocID = new InventoryLocation("InventoryLocationName", locName).InventoryLocationID;
                                    if (invLocID == 0)
                                    {
                                        Helper.WriteLog(string.Format("Location {0} is not found.", locName), MessageType.ERROR);
                                        return false;
                                    }

                                    InventoryController invCtrl = new InventoryController();
                                    invCtrl = new InventoryController(InventorySettings.CostingMethod);
                                    invCtrl.SetInventoryLocation(invLocID);
                                    invCtrl.SetInventoryDate(DateTime.Now);
                                    invCtrl.SetRemark("Stock Take from ERPIntegration");

                                    QueryCommandCollection cmdColl = new QueryCommandCollection();
                                    List<string> newItemNos = new List<string>();

                                    DataRow[] rows = dtResult.Select(string.Format("LOCATION = '{0}'", locName), "FMTITEMNO ASC");

                                    for (int j = 0; j < rows.Length; j++)
                                    {
                                        //Helper.WriteLog(string.Format("Preparing record #{0}.", (j + 1).ToString()), MessageType.INFO);
                                        DataRow row = rows[j];

                                        // Column list : FMTITEMNO | QTYONHAND | LOCATION

                                        if (row["FMTITEMNO"].ToString().Trim() == "" || row["QTYONHAND"].ToString().Trim() == "" || row["LOCATION"].ToString().Trim() == "")
                                        {
                                            Helper.WriteLog(string.Format("Record #{0} is excluded. Reason: 'FMTITEMNO' or 'QTYONHAND' or 'LOCATION' column is empty.", (j + 1).ToString()), MessageType.INFO);
                                        }
                                        else
                                        {
                                            //string barcode = row["FMTITEMNO"].ToString().Trim();
                                            //string itemNo = new Item("Barcode", barcode).ItemNo;
                                            //if (string.IsNullOrEmpty(itemNo))
                                            //{
                                            //    Helper.WriteLog(string.Format("Barcode number {0} is not found.", barcode), MessageType.ERROR);
                                            //    return false;
                                            //}
                                            string itemNo = row["FMTITEMNO"].ToString().Trim();
                                            string location = row["LOCATION"].ToString().Trim();
                                            if (!newItemNos.Contains(itemNo + "|" + location))
                                            {
                                                // Check whether there is unadjusted stock take
                                                string LastStockTakeDateStr = StockTakeController.GetUnAdjustedStockTakeDate(invLocID);
                                                if (LastStockTakeDateStr != "")
                                                {
                                                    DateTime lastStockTakeDate;
                                                    if (DateTime.TryParse(LastStockTakeDateStr, out lastStockTakeDate))
                                                    {
                                                        if (DateTime.Compare(lastStockTakeDate, invCtrl.GetInventoryDate()) != 0)
                                                        {
                                                            Helper.WriteLog(string.Format("There is an unadjusted stock take for location {0} with different Stock Take Date. No inventory movement is allowed! Please adjust stock take first", locName), MessageType.ERROR);
                                                            return false;
                                                        }
                                                    }
                                                }

                                                string onHandStr = row["QTYONHAND"].ToString().Trim();
                                                decimal onHand = 0;
                                                decimal.TryParse(row["QTYONHAND"].ToString().Trim(), out onHand);

                                                invCtrl.AddItemIntoInventoryUsingAltCost(itemNo, onHand, false, out status);
                                                newItemNos.Add(itemNo + "|" + location);
                                            }
                                        }
                                    }

                                    // Create Stock Take Entries
                                    if (!invCtrl.CreateStockTakeEntries("SYSTEM", "SYSTEM", "SYSTEM", true, out status))
                                    {
                                        Helper.WriteLog("Error encountered while creating stock take entries: " + status, MessageType.ERROR);
                                        return false;
                                    }

                                    // Adjust Stock Take
                                    StockTakeController ct = new StockTakeController();
                                    if (!ct.CorrectStockTakeDiscrepancy("SYSTEM", invLocID))
                                    {
                                        Helper.WriteLog("Error encountered while Adjusting Stock Discrepancy. Please contact your system administrator.", MessageType.ERROR);
                                        return false;
                                    }

                                    Helper.WriteLog(string.Format("Stock Take completed successfully for location {0}", locName), MessageType.INFO);
                                }
                            }

                            // After finish with the file, move to backup folder
                            FileInfo backupFile = new FileInfo(Path.Combine(backupDir.FullName, file.Name));
                            if (backupFile.Exists)
                            {
                                backupFile.Delete();
                            }
                            file.MoveTo(backupFile.FullName);

                            Helper.WriteLog(string.Format("File {0} has been moved to backup directory.", file.Name), MessageType.INFO);

                        }

                        Console.WriteLine();
                        Helper.WriteLog("All file(s) have been processed successfully.", MessageType.INFO);
                    }

                    return true;

                }
            }
            catch (Exception ex)
            {
                Helper.WriteLog(ex.ToString(), MessageType.ERROR);
                return false;
            }
        }
    }
}
