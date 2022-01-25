using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Configuration;
using PowerPOS;
using System.Data;
using SubSonic;

namespace ERPIntegration.API
{
    class Import_Item
    {
        struct MessageType
        {
            public static string INFO = "IMPORT_ITEM_INFO";
            public static string ERROR = "IMPORT_ITEM_ERROR";
        }

        public static bool DoImport(string directory)
        {
            bool proceed = true;

            try
            {
                Helper.WriteLog("Starting Import Item module", MessageType.INFO);

                DirectoryInfo fileDir = new DirectoryInfo(directory);
                DirectoryInfo backupDir;
                if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["Item_BackupDirectory"]))
                    backupDir = new DirectoryInfo(Path.Combine(directory, "backup"));
                else
                    backupDir = new DirectoryInfo(ConfigurationManager.AppSettings["Item_BackupDirectory"]);

                //string itemDepartmentID = ConfigurationManager.AppSettings["Item_DefaultItemDepartmentID"];

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

                //if (string.IsNullOrEmpty(itemDepartmentID))
                //{
                //    Helper.WriteLog("Please specify the default ItemDepartmentID in config file first." + Environment.NewLine + "Key=\"Item_DefaultItemDepartmentID\"", MessageType.ERROR);
                //    proceed = false;
                //}

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
                            else
                            {
                                Helper.WriteLog(string.Format("File {0} has been imported to DataTable successfully.", file.Name), MessageType.INFO);
                                Helper.WriteLog(string.Format("Found {0} record(s).", dtResult.Rows.Count.ToString()), MessageType.INFO);

                                QueryCommandCollection cmdColl = new QueryCommandCollection();
                                List<string> newItemNos = new List<string>();

                                for (int j = 0; j < dtResult.Rows.Count; j++)
                                {
                                    //Helper.WriteLog(string.Format("Preparing record #{0}.", (j + 1).ToString()), MessageType.INFO);
                                    DataRow row = dtResult.Rows[j];

                                    if (row["POSITEMNO"].ToString().Trim() == "" || row["DESC"].ToString().Trim() == "" || row["ITEMCAT"].ToString().Trim() == "" || row["BRAND"].ToString().Trim() == "")
                                    {
                                        Helper.WriteLog(string.Format("Record #{0} is excluded. Reason: 'POSITEMNO' or 'DESC' or 'ITEMCAT' or 'BRAND' column is empty.", (j + 1).ToString()), MessageType.INFO);
                                    }
                                    else
                                    {
                                        string itemNo = row["POSITEMNO"].ToString().Trim();
                                        if (!newItemNos.Contains(itemNo))
                                        {
                                            string itemName = row["DESC"].ToString().Trim();
                                            //string categoryName = row["ITEMCAT"].ToString().Trim();
                                            string categoryName = row["BRAND"].ToString().Trim();
                                            string itemDepartmentID = row["ITEMCAT"].ToString().Trim();

                                            string barcode = (dtResult.Columns.Contains("FMTITEMNO")) ? row["FMTITEMNO"].ToString().Trim() : "";
                                            bool deleted = (dtResult.Columns.Contains("INACTIVE")) ? AppSetting.CastBool(row["INACTIVE"].ToString().Trim(), false) : false;

                                            string remark = "";
                                            remark += (dtResult.Columns.Contains("COMMENT1")) ? row["COMMENT1"].ToString().Trim() : "";
                                            remark += (dtResult.Columns.Contains("COMMENT2")) ? (" " + row["COMMENT2"].ToString().Trim()) : "";
                                            remark += (dtResult.Columns.Contains("COMMENT3")) ? (" " + row["COMMENT3"].ToString().Trim()) : "";
                                            remark += (dtResult.Columns.Contains("COMMENT4")) ? (" " + row["COMMENT4"].ToString().Trim()) : "";
                                            remark = remark.Trim();

                                            string brand = (dtResult.Columns.Contains("BRAND")) ? row["BRAND"].ToString().Trim() : "";

                                            decimal unitPrice = 0;
                                            if (dtResult.Columns.Contains("UNITPRICE"))
                                            {
                                                decimal.TryParse(row["UNITPRICE"].ToString().Trim(), out unitPrice);
                                            }

                                            #region *) Get item attributes
                                            string Attr1Col = ConfigurationManager.AppSettings["Item_Attr1Col"];
                                            string Attr2Col = ConfigurationManager.AppSettings["Item_Attr2Col"];
                                            string Attr3Col = ConfigurationManager.AppSettings["Item_Attr3Col"];
                                            string Attr4Col = ConfigurationManager.AppSettings["Item_Attr4Col"];
                                            string Attr5Col = ConfigurationManager.AppSettings["Item_Attr5Col"];
                                            string Attr6Col = ConfigurationManager.AppSettings["Item_Attr6Col"];
                                            string Attr7Col = ConfigurationManager.AppSettings["Item_Attr7Col"];
                                            string Attr8Col = ConfigurationManager.AppSettings["Item_Attr8Col"];

                                            string attributes1 = "";
                                            if (!string.IsNullOrEmpty(Attr1Col))
                                                attributes1 = (dtResult.Columns.Contains(Attr1Col)) ? row[Attr1Col].ToString().Trim() : "";

                                            string attributes2 = "";
                                            if (!string.IsNullOrEmpty(Attr2Col))
                                                attributes2 = (dtResult.Columns.Contains(Attr2Col)) ? row[Attr2Col].ToString().Trim() : "";

                                            string attributes3 = "";
                                            if (!string.IsNullOrEmpty(Attr3Col))
                                                attributes3 = (dtResult.Columns.Contains(Attr3Col)) ? row[Attr3Col].ToString().Trim() : "";

                                            string attributes4 = "";
                                            if (!string.IsNullOrEmpty(Attr4Col))
                                                attributes4 = (dtResult.Columns.Contains(Attr4Col)) ? row[Attr4Col].ToString().Trim() : "";

                                            string attributes5 = "";
                                            if (!string.IsNullOrEmpty(Attr5Col))
                                                attributes5 = (dtResult.Columns.Contains(Attr5Col)) ? row[Attr5Col].ToString().Trim() : "";

                                            string attributes6 = "";
                                            if (!string.IsNullOrEmpty(Attr6Col))
                                                attributes6 = (dtResult.Columns.Contains(Attr6Col)) ? row[Attr6Col].ToString().Trim() : "";

                                            string attributes7 = "";
                                            if (!string.IsNullOrEmpty(Attr7Col))
                                                attributes7 = (dtResult.Columns.Contains(Attr7Col)) ? row[Attr7Col].ToString().Trim() : "";

                                            string attributes8 = "";
                                            if (!string.IsNullOrEmpty(Attr8Col))
                                                attributes8 = (dtResult.Columns.Contains(Attr8Col)) ? row[Attr8Col].ToString().Trim() : "";

                                            #endregion

                                            #region *) Check Category, if not exist, create it first
                                            Category category = new Category(categoryName);
                                            if (string.IsNullOrEmpty(category.CategoryName))
                                            {
                                                // The CategoryName doesn't exist, insert it
                                                Helper.WriteLog(string.Format("CategoryName \"{0}\" does not exist. It will be created automatically.", categoryName), MessageType.INFO);

                                                // Check for ItemDepartmentID
                                                ItemDepartment itemDept = new ItemDepartment(itemDepartmentID);
                                                if (string.IsNullOrEmpty(itemDept.ItemDepartmentID))
                                                {
                                                    // The ItemDepartmentID doesn't exist, insert it
                                                    Helper.WriteLog(string.Format("ItemDepartmentID \"{0}\" does not exist. It will be created automatically.", itemDepartmentID), MessageType.INFO);

                                                    itemDept.ItemDepartmentID = itemDepartmentID;
                                                    itemDept.DepartmentName = itemDepartmentID;
                                                    itemDept.Deleted = false;
                                                    DataService.ExecuteQuery(itemDept.GetInsertCommand("ERPIntegration"));

                                                    Helper.WriteLog(string.Format("ItemDepartmentID \"{0}\" is created successfully.", itemDepartmentID), MessageType.INFO);
                                                }

                                                category.CategoryName = categoryName;
                                                category.Remarks = "";
                                                category.CategoryId = "";
                                                category.IsDiscountable = true;
                                                category.IsForSale = true;
                                                category.IsGST = true;
                                                category.AccountCategory = "";
                                                category.ItemDepartmentId = itemDepartmentID;
                                                category.Deleted = false;
                                                DataService.ExecuteQuery(category.GetInsertCommand("ERPIntegration"));

                                                Helper.WriteLog(string.Format("CategoryName \"{0}\" is created successfully.", categoryName), MessageType.INFO);
                                            }
                                            #endregion

                                            Item item = new Item(itemNo);
                                            item.ItemNo = itemNo;
                                            item.ItemName = itemName;
                                            item.CategoryName = categoryName;
                                            item.Barcode = barcode;
                                            item.Deleted = deleted;
                                            item.Remark = remark;
                                            item.Brand = brand;
                                            item.RetailPrice = unitPrice;
                                            item.Attributes1 = attributes1;
                                            item.Attributes2 = attributes2;
                                            item.Attributes3 = attributes3;
                                            item.Attributes4 = attributes4;
                                            item.Attributes5 = attributes5;
                                            item.Attributes6 = attributes6;
                                            item.Attributes7 = attributes7;
                                            item.Attributes8 = attributes8;

                                            if (item.IsNew)
                                            {
                                                item.IsServiceItem = false;
                                                item.IsInInventory = true;
                                                item.UniqueID = Guid.NewGuid();
                                                item.GSTRule = AppSetting.CastInt(ConfigurationManager.AppSettings["Item_DefaultGSTRule"], 2);
                                                item.IsCommission = true;
                                                newItemNos.Add(item.ItemNo);
                                            }

                                            QueryCommand cmd = item.GetSaveCommand("ERPIntegration");
                                            if (cmd != null) cmdColl.Add(cmd);
                                        }
                                    }
                                }

                                // Execute the update
                                Helper.WriteLog("Saving to database...", MessageType.INFO);
                                DataService.ExecuteTransaction(cmdColl);
                                Helper.WriteLog("Saving to database completed successfully.", MessageType.INFO);
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
                        Helper.WriteLog("All file(s) have been imported successfully.", MessageType.INFO);
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
