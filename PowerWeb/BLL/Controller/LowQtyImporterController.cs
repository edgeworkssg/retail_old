using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PowerPOS;
using System.Data;
using SubSonic;

namespace PowerWeb.BLL.Controller
{
    public class LowQtyImporterController
    {
        public static DataTable FetchItem(int InventoryLocationID, string itemDeptID, string category)
        {
            DataTable dt = new DataTable();

            try
            {
                string sql = @"
                            DECLARE @ItemDepartmentID NVARCHAR(MAX);
                            DECLARE @CategoryName NVARCHAR(MAX);
                            DECLARE @InventoryLocationID int

                            SET @ItemDepartmentID = '{0}';
                            SET @CategoryName ='{1}';
                            SET @InventoryLocationID = {2};

                             select
                                ID.ItemDepartmentID [Department Code]
                                ,ID.DepartmentName [Department Name]
                                ,C.Category_ID [Category Code]
                                ,C.CategoryName [Category Name]
                                ,iq.ItemNo [Item No], i.ItemName [Item Name]
                                ,iq.TriggerQuantity [Trigger Quantity]
                                ,il.InventoryLocationName [Inventory Location], 
	                            CASE WHEN ISNULL(iq.Deleted,0) = 0 THEN 'No' ELSE 'Yes' END AS [Deleted] 
                                {3} 
                            from ItemQuantityTrigger iq
                            INNER JOIN InventoryLocation il on iq.InventoryLocationID = il.InventoryLocationID
                            INNER JOIN Item I on iq.ItemNo = i.ItemNo
                            LEFT JOIN Category C ON C.CategoryName = I.CategoryName
                            LEFT JOIN ItemDepartment ID ON ID.ItemDepartmentID = C.ItemDepartmentId
                            WHERE ISNULL(il.Deleted,0) = 0 and ISNULL(i.Deleted,0) = 0 and ISNULL(iq.Deleted, 0) = 0
                            AND ID.DepartmentName <> 'SYSTEM'
                            AND (ID.ItemDepartmentID = @ItemDepartmentID OR @ItemDepartmentID = 'ALL')
                            AND (C.CategoryName = @CategoryName OR @CategoryName = 'ALL')
                            AND (il.InventoryLocationID = @InventoryLocationID OR @InventoryLocationID = 0)";

                string userfld = "";

                if (!string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld1)))
                {
                    userfld += " , ISNULL(iq.Userfld1,'') as [" + AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld1) +"] ";
                }

                if (!string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld2)))
                {
                    userfld += " , ISNULL(iq.Userfld2,'') as [" + AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld2) +"] ";
                }

                if (!string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld3)))
                {
                    userfld += " , ISNULL(iq.Userfld3,'') as [" + AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld3) +"] ";
                }

                if (!string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld4)))
                {
                    userfld += " , ISNULL(iq.Userfld4,'') as [" + AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld4) +"] ";
                }

                if (!string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld5)))
                {
                    userfld += " , ISNULL(iq.Userfld5,'') as [" + AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld5) +"] ";
                }

                if (!string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld6)))
                {
                    userfld += " , ISNULL(iq.Userfld6,'') as [" + AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld6) +"] ";
                }

                if (!string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld7)))
                {
                    userfld += " , ISNULL(iq.Userfld7,'') as [" + AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld7) +"] ";
                }

                if (!string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld8)))
                {
                    userfld += " , ISNULL(iq.Userfld8,'') as [" + AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld8) +"] ";
                }

                if (!string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld9)))
                {
                    userfld += " , ISNULL(iq.Userfld9,'') as [" + AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld9) +"] ";
                }

                if (!string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld10)))
                {
                    userfld += " , ISNULL(iq.Userfld10,'') as [" + AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld10) +"] ";
                }

                sql = string.Format(sql, itemDeptID, category, InventoryLocationID.ToString(), userfld);
               
                dt.Load(DataService.GetReader(new QueryCommand(sql)));
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }

            return dt;
        }

        public static DataTable FetchItemWithExisting(int InventoryLocationID, string itemDeptID, string category, bool isExistingItemOnly)
        {
            DataTable dt = new DataTable();

            try
            {
                string sql = "";
                
                sql = @"DECLARE @ItemDepartmentID NVARCHAR(MAX);
                        DECLARE @CategoryName NVARCHAR(MAX);
                        DECLARE @InventoryLocationID int

                        SET @ItemDepartmentID = '{0}';
                        SET @CategoryName ='{1}';
                        SET @InventoryLocationID = {2};

                         select
                            ID.ItemDepartmentID [Department Code]
                            ,ID.DepartmentName [Department Name]
                            ,C.Category_ID [Category Code]
                            ,C.CategoryName [Category Name]
                            ,i.ItemNo [Item No], i.ItemName [Item Name]
                            ,iq.TriggerQuantity [Trigger Quantity]
                            ,il.InventoryLocationName [Inventory Location], 
                            CASE WHEN ISNULL(iq.Deleted,0) = 0 THEN 'No' ELSE 'Yes' END AS [Deleted] 
                            {3} 
                        from Item I 
                        LEFT JOIN ItemQuantityTrigger iq on iq.ItemNo = i.ItemNo
                        LEFT JOIN InventoryLocation il on iq.InventoryLocationID = il.InventoryLocationID                            
                        LEFT JOIN Category C ON C.CategoryName = I.CategoryName
                        LEFT JOIN ItemDepartment ID ON ID.ItemDepartmentID = C.ItemDepartmentId
                        WHERE ISNULL(il.Deleted,0) = 0 and ISNULL(i.Deleted,0) = 0 and ISNULL(iq.Deleted, 0) = 0 {4}
                        AND ID.DepartmentName <> 'SYSTEM'
                        AND (ID.ItemDepartmentID = @ItemDepartmentID OR @ItemDepartmentID = 'ALL')
                        AND (C.CategoryName = @CategoryName OR @CategoryName = 'ALL')
                        AND (il.InventoryLocationID = @InventoryLocationID OR @InventoryLocationID = 0)";

                string exist = "";
                if (isExistingItemOnly)
                    exist = "AND iq.ItemNo IS NOT NULL ";

                string userfld = "";

                if (!string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld1)))
                {
                    userfld += " , ISNULL(iq.Userfld1,'') as [" + AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld1) + "] ";
                }

                if (!string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld2)))
                {
                    userfld += " , ISNULL(iq.Userfld2,'') as [" + AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld2) + "] ";
                }

                if (!string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld3)))
                {
                    userfld += " , ISNULL(iq.Userfld3,'') as [" + AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld3) + "] ";
                }

                if (!string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld4)))
                {
                    userfld += " , ISNULL(iq.Userfld4,'') as [" + AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld4) + "] ";
                }

                if (!string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld5)))
                {
                    userfld += " , ISNULL(iq.Userfld5,'') as [" + AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld5) + "] ";
                }

                if (!string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld6)))
                {
                    userfld += " , ISNULL(iq.Userfld6,'') as [" + AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld6) + "] ";
                }

                if (!string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld7)))
                {
                    userfld += " , ISNULL(iq.Userfld7,'') as [" + AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld7) + "] ";
                }

                if (!string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld8)))
                {
                    userfld += " , ISNULL(iq.Userfld8,'') as [" + AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld8) + "] ";
                }

                if (!string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld9)))
                {
                    userfld += " , ISNULL(iq.Userfld9,'') as [" + AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld9) + "] ";
                }

                if (!string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld10)))
                {
                    userfld += " , ISNULL(iq.Userfld10,'') as [" + AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld10) + "] ";
                }

                sql = string.Format(sql, itemDeptID, category, InventoryLocationID.ToString(), userfld, exist);

                dt.Load(DataService.GetReader(new QueryCommand(sql)));
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }

            return dt;
        }

        public static DataTable FetchItemWardTopUp(int InventoryLocationID)
        {
            DataTable dt = new DataTable();

            try
            {
                #region Set Userfld1
                //Get userfld for DepartmentOU 
                AppSettingCollection qr1 = new AppSettingCollection();
                qr1.Where(AppSetting.Columns.AppSettingValue, "Department OU");
                qr1.Load();

                string userfldDepartmentOU = "'' as [Department OU]";
                if (qr1.Count() > 0)
                {
                    string userfld1 = qr1[0].AppSettingKey;
                    userfldDepartmentOU = "ISNULL(iq." + userfld1.Substring(6, userfld1.Length - 6) + ",'') as [Department OU]";
                }

                string userfldNursingOu = "";
                AppSettingCollection qr2 = new AppSettingCollection();
                qr2.Where(AppSetting.Columns.AppSettingValue, "Nursing OU");
                qr2.Load();

                if (qr2.Count() > 0)
                {
                    string userfld1 = qr2[0].AppSettingKey;
                    userfldNursingOu = "ISNULL(iq." + userfld1.Substring(6, userfld1.Length - 6) + ",'') as [Nursing OU]";
                }

                string userfldParValue = "";
                AppSettingCollection qr3 = new AppSettingCollection();
                qr3.Where(AppSetting.Columns.AppSettingValue, "Par Value");
                qr3.Load();

                if (qr3.Count() > 0)
                {
                    string userfld1 = qr3[0].AppSettingKey;
                    userfldParValue = "ISNULL(iq." + userfld1.Substring(6, userfld1.Length - 6) + ",'') as [Par Value]";
                }
                #endregion

                string sql = @"
                            DECLARE @InventoryLocationID int

                            SET @InventoryLocationID = {0};

                            select {1}, {2},
	                            i.Attributes1 as [Plant], i.Attributes2 as [Storage Location], i.ItemNo as [Material Code],
	                            i.ItemName as [Material Description], {3} 
                            from ItemQuantityTrigger iq 
	                            inner join Item i on iq.ItemNo = i.ItemNo
                            where iq.InventoryLocationID = @InventoryLocationID 
	                            and ISNULL(i.Deleted,0) = 0 and ISNULL(iq.Deleted,0) = 0";

                

                sql = string.Format(sql, InventoryLocationID.ToString(), userfldDepartmentOU, userfldNursingOu, userfldParValue);

                dt.Load(DataService.GetReader(new QueryCommand(sql)));
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }

            return dt;
        }

        public static bool ImportData(string userName, DataTable data, out DataTable result, out string message)
        {
            bool isSuccess = false;
            result = data.Copy();
            message = "";

            try
            {
                result.Columns.Add("Status", typeof(string));
                QueryCommandCollection qmc = new QueryCommandCollection();

                #region *) Validation
                List<string> theItemNos = new List<string>();
                List<string> theBarcodes = new List<string>();
                List<CheckItemQtyTrigger> check = new List<CheckItemQtyTrigger>();
                for (int i = 0; i < result.Rows.Count; i++)
                {
                    bool isValid = true;
                    string itemNo = (string)result.Rows[i]["Item No"];
                    string triggerQuantity = (string)result.Rows[i]["Trigger Quantity"];
                    string InventoryLocationName = (string)result.Rows[i]["Inventory Location"];
                    string deleted = (string)result.Rows[i]["Deleted"];
                    string status = "";

                    if (string.IsNullOrEmpty(itemNo))
                    {
                        status = "- ItemNo Cannot Empty\n";
                        isValid = false;
                    }
                    else
                    {
                        Item it = new Item(itemNo.Trim());

                        if (it.IsNew || it.Deleted == true)
                        {
                            status = "- Item not found\n";
                            isValid = false;
                        }
                    }

                    //if (theItemNos.Contains(itemNo.Trim()))
                    //{
                    //    status = "- Duplicated ItemNo\n";
                    //    isValid = false;
                    //}

                    if (string.IsNullOrEmpty(triggerQuantity) && triggerQuantity.GetIntValue() <= 0)
                    {
                        status = "- Trigger Quantity Cannot Less Equals Than Zero\n";
                        isValid = false;
                    }

                    int invlodID = 0;

                    if (!string.IsNullOrEmpty(InventoryLocationName))
                    {
                       
                        var inv = new InventoryLocation(InventoryLocation.Columns.InventoryLocationName, InventoryLocationName.Trim());
                        if (inv.IsNew)
                        {
                            status = "- Inventory Location not found";
                            isValid = false;
                        }
                        else
                        {
                            invlodID = inv.InventoryLocationID;

                            var found = check.Where(f => f.ItemNo == itemNo && f.InventoryLocationID == invlodID).ToList();

                            if (found.Count > 0)
                            {
                                status = "- Duplicate Item No and Inventory Location";
                                isValid = false;
                            }
                        }
                    }
                    else
                    {
                        status = "- Inventory Location Name Cannot Empty\n";
                        isValid = false;
                    }

                    result.Rows[i]["Status"] = status;
                    //if(!deleted.GetBoolValue(false))
                    //    theItemNos.Add(itemNo.Trim());

                    if (isValid)
                    {
                        theItemNos.Add(itemNo);
                        check.Add(new CheckItemQtyTrigger() { ItemNo = itemNo, InventoryLocationID = invlodID });
                    }
                }

                for (int i = 0; i < result.Rows.Count; i++)
                {
                    string err = (string)result.Rows[i]["Status"];
                    if (string.IsNullOrEmpty(err))
                    {
                        string itemNo = (string)result.Rows[i]["Item No"];
                        string triggerQuantity = (string)result.Rows[i]["Trigger Quantity"];
                        string InventoryLocationName = (string)result.Rows[i]["Inventory Location"];
                        InventoryLocation inv = new InventoryLocation(InventoryLocation.Columns.InventoryLocationName, InventoryLocationName.Trim());
                        string deleted = (string)result.Rows[i]["Deleted"];

                        ItemQuantityTriggerCollection col = new ItemQuantityTriggerCollection();
                        col.Where(ItemQuantityTrigger.Columns.ItemNo, itemNo);
                        col.Where(ItemQuantityTrigger.Columns.InventoryLocationID, inv.InventoryLocationID);
                        col.Where(ItemQuantityTrigger.Columns.Deleted, false);
                        col.Load();

                        ItemQuantityTrigger itq;
                        if (col.Count() > 0)
                            itq = col[0];
                        else
                            itq = new ItemQuantityTrigger();

                        itq.ItemNo = itemNo;
                        itq.TriggerQuantity = triggerQuantity.GetIntValue();
                        itq.InventoryLocationID = inv.InventoryLocationID;
                        itq.Deleted = deleted.ToUpper().Equals("YES");

                        if (!string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld1)))
                        {
                            itq.Userfld1 = (string)result.Rows[i][AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld1)];
                        }

                        if (!string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld2)))
                        {
                            itq.Userfld2 = (string)result.Rows[i][AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld2)];  
                        }

                        if (!string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld3)))
                        {
                            itq.Userfld3 = (string)result.Rows[i][AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld3)];
                        }

                        if (!string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld4)))
                        {
                            itq.Userfld4 = (string)result.Rows[i][AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld4)];
                        }

                        if (!string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld5)))
                        {
                            itq.Userfld5 = (string)result.Rows[i][AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld5)];
                        }

                        if (!string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld6)))
                        {
                            itq.Userfld6 = (string)result.Rows[i][AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld6)];
                        }

                        if (!string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld7)))
                        {
                            itq.Userfld7 = (string)result.Rows[i][AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld7)];
                        }

                        if (!string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld8)))
                        {
                            itq.Userfld8 = (string)result.Rows[i][AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld8)];
                        }

                        if (!string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld9)))
                        {
                            itq.Userfld9 = (string)result.Rows[i][AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld9)];
                        }

                        if (!string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld10)))
                        {
                            itq.Userfld10 = (string)result.Rows[i][AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld10)];
                        }

                        if(itq.IsNew)
                            qmc.Add(itq.GetInsertCommand(string.Format("{0} via WEB Low Qty Importer", userName)));
                        else
                            qmc.Add(itq.GetUpdateCommand(string.Format("{0} via WEB Low Qty Importer", userName)));
                        
                    }
                }

                #endregion

                if (qmc.Count > 0)
                    DataService.ExecuteTransaction(qmc);
                isSuccess = true;
            }
            catch (Exception ex)
            {
                isSuccess = false;
                message = "ERROR : " + ex.Message;
                Logger.writeLog(ex);
            }

            return isSuccess;
        }

        public static bool ImportStockTakeData(string userName, DataTable data, string outletname, out DataTable result, out string message)
        {
            bool isSuccess = false;
            result = data.Copy();
            message = "";

            try
            {
                result.Columns.Add("Status", typeof(string));
                QueryCommandCollection qmc = new QueryCommandCollection();

                #region *) Validation
                List<string> theItemNos = new List<string>();
                List<string> theBarcodes = new List<string>();
                string sqlDeleteItem = "Update Item set Deleted = 1 ";
                qmc.Add(new QueryCommand(sqlDeleteItem));

                string sqlDelete = "Update OutletGroupItemMap set isItemDeleted = 1 where outletname = '" + outletname + "'";
                qmc.Add(new QueryCommand(sqlDelete));
                for (int i = 0; i < result.Rows.Count; i++)
                {
                    bool isValid = true;
                    string itemNo = (string)result.Rows[i]["Material"];
                    string itemName = (string)result.Rows[i]["Material Description"];
                    string plnt = (string)result.Rows[i]["Plnt"];
                    string SLoc = (string)result.Rows[i]["SLoc"];
                    string itemUOM = (string)result.Rows[i]["BUn"];
                    string unrestricted = (string)result.Rows[i]["Unrestricted"];

                    
                    //string deleted = (string)result.Rows[i]["Deleted"];
                    string status = "";

                    Outlet ou = new Outlet(outletname);
                    if (ou != null && ou.OutletName != "")
                    {

                    }
                    else
                    {
                        status = "- OutletName is wrong \n";
                        isValid = false;
                    }

                    if (string.IsNullOrEmpty(itemNo))
                    {
                        status = "- ItemNo Cannot Empty\n";
                        isValid = false;
                    }
                    /*else
                    {
                        Item it = new Item(itemNo.Trim());

                        if (it.IsNew)
                        {
                            if (it.IsNew)
                            {
                                it.ItemNo = itemNo.Trim();
                                it.ItemName = itemName.Trim();
                                it.UOM = itemUOM.Trim();
                                it.RetailPrice = 0;
                                it.FactoryPrice = 0;
                                it.GSTRule = 3;
                                it.CategoryName = "Product";
                                it.Barcode = "";
                                it.Attributes1 = plnt;
                                it.Attributes2 = SLoc;
                            }
                            else
                            {
                                status = "- Item not found\n";
                                isValid = false;
                            }
                        }
                    }*/

                    Decimal restrict = 0;
                    if (!Decimal.TryParse(unrestricted.Replace(",","").Replace("\"",""), out restrict))
                    {
                        status = "- Error parse unrestricted\n";
                        isValid = false;
                    }
                    result.Rows[i]["Status"] = status;
                }
                // set deleted = true for all item in this outlet
                

                DataView view = new DataView(result);
                DataTable distinctValue = view.ToTable(true, "Plnt", "SLoc", "Material", "Material Description", "BUn", "Status");
                Logger.writeLog("Total Distinct Rows : " + distinctValue.Rows.Count.ToString());
                for (int i = 0; i < distinctValue.Rows.Count; i++)
                {
                    string err = (string)distinctValue.Rows[i]["Status"];
                    if (string.IsNullOrEmpty(err))
                    {
                        string itemNo = (string)distinctValue.Rows[i]["Material"];
                        string itemName = (string)distinctValue.Rows[i]["Material Description"];
                        string plnt = (string)distinctValue.Rows[i]["Plnt"];
                        string SLoc = (string)distinctValue.Rows[i]["SLoc"];
                        string itemUOM = (string)distinctValue.Rows[i]["BUn"];

                        if (theItemNos.Contains(itemNo.Trim()))
                            continue;

                        Item it = new Item(itemNo.Trim());

                        if (it.IsNew )
                        {
                            if (it.IsNew)
                            {
                                it.ItemNo = itemNo.Trim();
                                it.ItemName = itemName.Trim();
                                it.UOM = itemUOM.Trim();
                                it.RetailPrice = 0;
                                it.FactoryPrice = 0;
                                it.GSTRule = 3;
                                it.CategoryName = "Product";
                                it.Barcode = itemNo.Trim();
                                it.Attributes1 = plnt;
                                it.Attributes2 = SLoc;
                                it.IsInInventory = true;
                                it.IsServiceItem = false;
                                it.IsCommission = true;
                                it.Userfld9 = "N";
                                it.Userfld10 = "N";
                                it.Deleted = true;
                                it.ItemDesc = "";
                                qmc.Add(it.GetInsertCommand(string.Format("{0} via WEB Stock Take Importer", userName)));

                                OutletGroupItemMapCollection col = new OutletGroupItemMapCollection();
                                col.Where(OutletGroupItemMap.Columns.ItemNo, itemNo.Trim());
                                col.Where(OutletGroupItemMap.Columns.OutletName, outletname.Trim());
                                col.Load();

                                if (col.Count > 0)
                                {
                                    OutletGroupItemMap og = col[0];
                                    //og.ItemNo = itemNo.Trim();
                                    //og.OutletName = outletname;
                                    og.RetailPrice = 0;
                                    og.Deleted = false;
                                    og.IsItemDeleted = false;
                                    qmc.Add(og.GetUpdateCommand(string.Format("{0} via WEB Stock Take Importer", userName)));
                                }
                                else
                                {
                                    OutletGroupItemMap og = new OutletGroupItemMap();
                                    og.ItemNo = itemNo.Trim();
                                    og.OutletName = outletname;
                                    og.RetailPrice = 0;
                                    og.Deleted = false;
                                    og.IsItemDeleted = false;
                                    qmc.Add(og.GetInsertCommand(string.Format("{0} via WEB Stock Take Importer", userName)));
                                }
                            }
                        }
                        else
                        {
                            it.ItemName = itemName.Trim();
                            it.UOM = itemUOM.Trim();
                            it.RetailPrice = 0;
                            it.FactoryPrice = 0;
                            it.GSTRule = 3;
                            it.CategoryName = "Product";
                            it.Barcode = itemNo.Trim();
                            it.Attributes1 = plnt;
                            it.Attributes2 = SLoc;
                            it.IsInInventory = true;
                            it.IsServiceItem = false;
                            it.IsCommission = true;
                            it.Userfld9 = "N";
                            it.Userfld10 = "N";
                            it.Deleted = true;
                            it.ItemDesc = "";
                            //it.Attributes1 = "";
                            //it.Attributes2 = "";

                            qmc.Add(it.GetUpdateCommand(string.Format("{0} via WEB Stock Take Importer", userName)));

                            OutletGroupItemMapCollection col = new OutletGroupItemMapCollection();
                            col.Where(OutletGroupItemMap.Columns.ItemNo, itemNo.Trim());
                            col.Where(OutletGroupItemMap.Columns.OutletName, outletname.Trim());
                            col.Load();

                            if (col.Count > 0)
                            {
                                OutletGroupItemMap og = col[0];
                                //og.ItemNo = itemNo.Trim();
                                //og.OutletName = outletname;
                                //og.RetailPrice = 0;
                                //og.Deleted = false;
                                //og.IsItemDeleted = false;
                                QueryCommand qc = new QueryCommand("Update OutletGroupItemMap set isitemdeleted = 0, deleted = 0, modifiedon = GETDATE() where outletgroupitemmapid = " + og.OutletGroupItemMapID.ToString());
                                //og.GetUpdateCommand(string.Format("{0} via WEB Stock Take Importer", userName));
                                qmc.Add(qc);
                                
                            }
                            else
                            {
                                OutletGroupItemMap og = new OutletGroupItemMap();
                                og.ItemNo = itemNo.Trim();
                                og.OutletName = outletname;
                                og.RetailPrice = 0;
                                og.Deleted = false;
                                og.IsItemDeleted = false;
                                qmc.Add(og.GetInsertCommand(string.Format("{0} via WEB Stock Take Importer", userName)));
                            }
                        }
                        theItemNos.Add(itemNo.Trim());

                        //**adjust stock on hand*/
                        var unrestricted = result.Select("Material = '" + itemNo + "'");
                        Decimal decUnrestriced = 0;
                        foreach (DataRow row in unrestricted)
                        {
                            decUnrestriced += row["Unrestricted"].ToString().Replace(",", "").Replace("\"", "").GetDecimalValue();
                        }

 //                       Decimal.TryParse(unrestricted, out decUnrestriced);
                        InventoryLocation inv = new InventoryLocation(InventoryLocation.Columns.InventoryLocationName, outletname);
                        ItemSummaryCollection ism = new ItemSummaryCollection();
                        ism.Where(ItemSummary.Columns.ItemNo, itemNo);
                        ism.Where(ItemSummary.Columns.InventoryLocationID, inv.InventoryLocationID);
                        ism.Load();
                        //DataTable soh = ReportController.FetchStockReportItemSummarySingleItem_FIFO(itemNo, inv.InventoryLocationID, true);

                        Decimal onhand = 0;
                        if(ism.Count > 0)
                            onhand = (decimal)ism[0].BalanceQty.GetValueOrDefault(0);

                        if (decUnrestriced != onhand)
                        { 
                            /*adjust inventory*/
                            decimal diff = decUnrestriced - onhand;
                            StockTakeController ctl = new StockTakeController();
                            if (diff != 0)
                                ctl.CorrectStockTakeDiscrepancySingleItem(userName, inv.InventoryLocationID, itemNo, diff);

                        }
                        
                    }
                }

                #endregion

                if (qmc.Count > 0)
                    DataService.ExecuteTransaction(qmc);
                isSuccess = true;
            }
            catch (Exception ex)
            {
                isSuccess = false;
                message = "ERROR : " + ex.Message;
                Logger.writeLog(ex);
            }

            return isSuccess;
        }

        public static bool ImportWardTopUpdata(string userName, DataTable data,int InventoryLocationID, out DataTable result, out string message)
        { 
            bool isSuccess = false;
            result = data.Copy();
            message = "";

            try
            {
                result.Columns.Add("Status", typeof(string));
                QueryCommandCollection qmc = new QueryCommandCollection();

                #region *) Validation
                //List<string> theItemNos = new List<string>();

                string outletName = "";
                OutletCollection oc = new OutletCollection();
                oc.Where(Outlet.Columns.InventoryLocationID, InventoryLocationID);
                oc.Where(Outlet.Columns.Deleted, false);
                oc.Load();

                if (oc.Count() > 0)
                {
                    outletName = oc[0].OutletName;
                }

                string sqlDeleteItem = "Update Item set Deleted = 1 ";
                qmc.Add(new QueryCommand(sqlDeleteItem));

                string sqlDelete = "Update OutletGroupItemMap set isItemDeleted = 1 where outletname = '" + outletName + "'";
                qmc.Add(new QueryCommand(sqlDelete));

                for (int i = 0; i < result.Rows.Count; i++)
                {
                    bool isValid = true;
                    string itemNo = (string)result.Rows[i]["Material Code"];
                    string itemName = (string)result.Rows[i]["Material Description"];

                    string status = "";

                    if(oc.Count() == 0)
                    {
                        status = "- Outlet with Inventory Location ID : " + InventoryLocationID.ToString() +" Not exist\n";
                        isValid = false;
                    }

                    if (string.IsNullOrEmpty(itemNo))
                    {
                        status = "- ItemNo Cannot Empty\n";
                        isValid = false;
                    }
                    
                    result.Rows[i]["Status"] = status;
                }
                #endregion

                #region setuserfld

                //Get userfld for DepartmentOU 
                AppSettingCollection qr1 = new AppSettingCollection();
                qr1.Where(AppSetting.Columns.AppSettingValue, "Department OU");
                qr1.Load();

                string userfldDepartmentOU = "";
                if (qr1.Count() > 0)
                {
                    string userfld1 = qr1[0].AppSettingKey;
                    userfldDepartmentOU = userfld1.Substring(6, userfld1.Length - 6);
                }

                string userfldNursingOu = "";
                AppSettingCollection qr2 = new AppSettingCollection();
                qr2.Where(AppSetting.Columns.AppSettingValue, "Nursing OU");
                qr2.Load();

                if (qr2.Count() > 0)
                {
                    string userfld1 = qr2[0].AppSettingKey;
                    userfldNursingOu = userfld1.Substring(6, userfld1.Length - 6);
                }

                string userfldParValue = "";
                AppSettingCollection qr3 = new AppSettingCollection();
                qr3.Where(AppSetting.Columns.AppSettingValue, "Par Value");
                qr3.Load();

                if (qr3.Count() > 0)
                {
                    string userfld1 = qr3[0].AppSettingKey;
                    userfldParValue = userfld1.Substring(6, userfld1.Length - 6);
                }
                #endregion



                #region Import Data
                for (int i = 0; i < result.Rows.Count; i++)
                {
                    string err = (string)result.Rows[i]["Status"];
                    if (string.IsNullOrEmpty(err))
                    {
                        string DepartmentOU = (string)result.Rows[i]["Department OU"];
                        string NursingOU = (string)result.Rows[i]["Nursing OU"];
                        string Plant = (string)result.Rows[i]["Plant"];
                        string StorageLocation = (string)result.Rows[i]["Storage Location"];
                        string ItemNo = (string)result.Rows[i]["Material Code"];
                        string ItemName = (string)result.Rows[i]["Material Description"];
                        string ParValue = (string)result.Rows[i]["Par Value"];
                        string UOM = (string)result.Rows[i]["UOM"];
                        

                        InventoryLocation inv = new InventoryLocation(InventoryLocationID);

                        Item it = new Item(ItemNo.Trim());

                        if (it.IsNew)
                        {
                            if (it.IsNew)
                            {
                                it.ItemNo = ItemNo.Trim();
                                it.ItemName = ItemName.Trim();
                                it.RetailPrice = 0;
                                it.FactoryPrice = 0;
                                it.GSTRule = 3;
                                it.CategoryName = "Product";
                                it.Barcode = ItemNo.Trim();
                                it.Attributes1 = Plant;
                                it.Attributes2 = StorageLocation;
                                it.IsInInventory = true;
                                it.IsServiceItem = false;
                                it.IsCommission = true;
                                it.Userfld9 = "N";
                                it.Userfld10 = "N";
                                it.Deleted = true;
                                it.ItemDesc = "";
                                //it.UOM = UOM;
                                qmc.Add(it.GetInsertCommand(string.Format("{0} via WEB Ward Top Up Importer", userName)));

                                OutletGroupItemMap og = new OutletGroupItemMap();
                                og.ItemNo = ItemNo.Trim();
                                og.OutletName = outletName;
                                og.RetailPrice = 0;
                                og.CostPrice = 0;
                                og.Deleted = false;
                                og.IsItemDeleted = false;
                                qmc.Add(og.GetInsertCommand(string.Format("{0} via WEB Ward Top Up Importer", userName)));
                            }

                        }
                        else
                        {
                            it.ItemName = ItemName.Trim();
                            it.RetailPrice = 0;
                            it.FactoryPrice = 0;
                            it.GSTRule = 3;
                            it.CategoryName = "Product";
                            it.Barcode = ItemNo.Trim();
                            it.Attributes1 = Plant;
                            it.Attributes2 = StorageLocation;
                            it.IsInInventory = true;
                            it.IsServiceItem = false;
                            it.IsCommission = true;
                            it.Userfld9 = "N";
                            it.Userfld10 = "N";
                            it.Deleted = true;
                            it.ItemDesc = "";
                            //it.UOM = UOM;
                            qmc.Add(it.GetUpdateCommand(string.Format("{0} via WEB Ward Top Up Importer", userName)));

                            OutletGroupItemMapCollection col = new OutletGroupItemMapCollection();
                            col.Where(OutletGroupItemMap.Columns.ItemNo, ItemNo.Trim());
                            col.Where(OutletGroupItemMap.Columns.OutletName, outletName.Trim());
                            col.Load();

                            if (col.Count > 0)
                            {
                                OutletGroupItemMap og = col[0];
                                //og.RetailPrice = 0;
                                //og.Deleted = false;
                                //og.IsItemDeleted = false;
                                QueryCommand qc = new QueryCommand("Update OutletGroupItemMap set isitemdeleted = 0, deleted = 0, modifiedon = GETDATE() where outletgroupitemmapid = " + og.OutletGroupItemMapID.ToString());
                                //og.GetUpdateCommand(string.Format("{0} via WEB Stock Take Importer", userName));
                                qmc.Add(qc);

                            }
                            else
                            {
                                OutletGroupItemMap og = new OutletGroupItemMap();
                                og.ItemNo = ItemNo.Trim();
                                og.OutletName = outletName;
                                og.RetailPrice = 0;
                                og.CostPrice = 0;
                                og.Deleted = false;
                                og.IsItemDeleted = false;
                                qmc.Add(og.GetInsertCommand(string.Format("{0} via WEB Ward Top Up Importer", userName)));
                            }
                        }

                        ItemQuantityTriggerCollection iqcol = new ItemQuantityTriggerCollection();
                        iqcol.Where(ItemQuantityTrigger.Columns.ItemNo, ItemNo);
                        iqcol.Where(ItemQuantityTrigger.Columns.InventoryLocationID, inv.InventoryLocationID);
                        iqcol.Where(ItemQuantityTrigger.Columns.Deleted, false);
                        iqcol.Load();

                        ItemQuantityTrigger itq;
                        if (iqcol.Count() > 0)
                        {
                            itq = iqcol[0];
                        }
                        else
                        {
                            itq = new ItemQuantityTrigger();
                            itq.InventoryLocationID = inv.InventoryLocationID;
                            itq.ItemNo = ItemNo;
                            itq.TriggerQuantity = 0;
                            itq.Deleted = false;
                        }

                        if (!string.IsNullOrEmpty(userfldDepartmentOU))
                        {
                             itq.SetColumnValue(userfldDepartmentOU, DepartmentOU);
                        }

                        if (!string.IsNullOrEmpty(userfldNursingOu))
                        {
                            itq.SetColumnValue(userfldNursingOu, NursingOU);
                        }

                        if (!string.IsNullOrEmpty(userfldParValue))
                        {
                            itq.SetColumnValue(userfldParValue, ParValue + " " + UOM);
                        }

                        if(itq.IsNew)
                            qmc.Add(itq.GetInsertCommand(string.Format("{0} via WEB Ward Top Up Importer", userName)));
                        else
                            qmc.Add(itq.GetUpdateCommand(string.Format("{0} via WEB Ward Top Up Importer", userName)));
                    }
                }
                #endregion

                if (qmc.Count > 0)
                    DataService.ExecuteTransaction(qmc);
                isSuccess = true;
            }
            catch (Exception ex)
            {
                isSuccess = false;
                message = "ERROR : " + ex.Message;
                Logger.writeLog(ex);
            }

            return isSuccess;
        }


    }

    public class CheckItemQtyTrigger
    {
        public string ItemNo { get; set; }
        public int InventoryLocationID { get; set; }
    }
}
