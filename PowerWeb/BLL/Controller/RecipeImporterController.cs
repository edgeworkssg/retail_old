using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using PowerPOS;
using SubSonic;

public class RecipeImporterController
{
    public static DataTable FetchItem(string itemDeptID, string category)
    {
        DataTable dt = new DataTable();

        try
        {
            string sql = @"
                            DECLARE @ItemDepartmentID NVARCHAR(200);
                            DECLARE @CategoryName NVARCHAR(200);

                            SET @ItemDepartmentID = '{0}';
                            SET @CategoryName ='{1}';

                            SELECT   I.ItemDepartmentID [Department Code]
	                                ,I.DepartmentName [Department]
	                                ,I.CategoryID [Category Code]
	                                ,I.CategoryName Category
	                                ,I.ItemNo
	                                ,I.ItemName
	                                ,IRD.ItemNo MaterialItemNo
	                                ,IRD.ItemName MaterialItemName
	                                ,RD.Qty
	                                ,RD.UOM
	                                ,CASE WHEN ISNULL(RD.IsPacking,0) = 0 THEN 'No' ELSE 'Yes' END IsPacking
                            FROM	RecipeHeader RH
	                                INNER JOIN RecipeDetail RD ON RD.RecipeHeaderID = RH.RecipeHeaderID
	                                INNER JOIN (
		                                SELECT   CAST(ID.ItemDepartmentID AS NVARCHAR(500)) ItemDepartmentID
				                                ,ID.DepartmentName
				                                ,CAST(CTG.ItemDepartmentID AS NVARCHAR(500)) CategoryID
				                                ,CTG.CategoryName
				                                ,I.ItemNo
				                                ,I.ItemName
				                                ,'Product' [Type]
		                                FROM	ItemDepartment ID
				                                INNER JOIN Category CTG ON CTG.ItemDepartmentID = ID.ItemDepartmentID
				                                INNER JOIN Item I ON I.CategoryName = CTG.CategoryName
		                                WHERE	ISNULL(ID.Deleted,0) = 0 AND ISNULL(CTG.Deleted,0) = 0
	                                ) I ON I.ItemNo = RH.ItemNo
	                                INNER JOIN Item IRD ON IRD.ItemNo = RD.ItemNo
                            WHERE	ISNULL(RH.Deleted,0) = 0
	                                AND ISNULL(RD.Deleted,0) = 0
	                                AND ISNULL(IRD.Deleted,0) = 0
	                                AND (I.ItemDepartmentID = @ItemDepartmentID OR @ItemDepartmentID = 'ALL')
	                                AND (I.CategoryName = @CategoryName OR @CategoryName = 'ALL')
                            ORDER BY I.Type, I.ItemDepartmentID, I.CategoryID, I.ItemNo, IRD.ItemNo";
            sql = string.Format(sql, itemDeptID, category);
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
            Dictionary<string, List<RecipeDetail>> rawData = new Dictionary<string, List<RecipeDetail>>();
            QueryCommandCollection qmc = new QueryCommandCollection();

            #region *) Validation
            bool isValidationSuccess = true;
            result.Columns.Add("Status", typeof(string));
            for (int i = 0; i < result.Rows.Count; i++)
            {
                string type = "Product";
                bool isValid = true;
                string status = "";
                string itemNo = (string)result.Rows[i]["ItemNo"];
                string materialItemNo = (string)result.Rows[i]["MaterialItemNo"];
                decimal qty = ((string)result.Rows[i]["Qty"]).GetDecimalValue();
                string uom = (string)result.Rows[i]["UOM"];
                bool isPacking = ((string)result.Rows[i]["IsPacking"]).ToUpper().Equals("YES");

                Item theItem = new Item(Item.Columns.ItemNo, itemNo);
                Item theMaterialItem = new Item(Item.Columns.ItemNo, materialItemNo);
                if (theItem.IsNew || theItem.Deleted.GetValueOrDefault(false))
                {
                    status = "- Item Not Found\n";
                    isValid = false;
                    isValidationSuccess = false;
                }

                if (theMaterialItem.IsNew || theMaterialItem.Deleted.GetValueOrDefault(false))
                {
                    status = "- Material Item Not Found\n";
                    isValid = false;
                    isValidationSuccess = false;
                }

                result.Rows[i]["Status"] = status;
                if (isValid)
                {
                    RecipeDetail rd = new RecipeDetail();
                    rd.ItemNo = materialItemNo;
                    rd.Qty = qty;
                    rd.Uom = uom;
                    rd.IsPacking = isPacking;
                    if (rawData.ContainsKey(itemNo))
                        rawData[itemNo].Add(rd);
                    else
                    {
                        List<RecipeDetail> rDetail = new List<RecipeDetail>();
                        rDetail.Add(rd);
                        rawData.Add(itemNo, rDetail);
                    }
                }
            }

            foreach (var rh in rawData)
            {
                string type = "Product";
                Item theItem = new Item(Item.Columns.ItemNo, rh.Key);

                var negativeDet = (from o in rh.Value
                                   where o.Qty < 0
                                   select o).ToList();

                if (negativeDet.Count == 0) continue;

                isValidationSuccess = false;
                for (int i = 0; i < result.Rows.Count; i++)
                {
                    string itemNo = (string)result.Rows[i]["ItemNo"];
                    string materialItemNo = (string)result.Rows[i]["MaterialItemNo"];
                    var nd = (from o in negativeDet
                              where o.ItemNo == materialItemNo
                              select o).FirstOrDefault();
                    if (itemNo == rh.Key && nd != null)
                        result.Rows[i]["Status"] = "Negative quantity is not allowed";
                }
            }

            if (!isValidationSuccess)
                throw new Exception("Validation failed");

            #endregion

            #region *) Confirm Data

            int rhCount = 0;
            foreach (var item in rawData)
            {
                rhCount++;
                RecipeHeader rh = new RecipeHeader(RecipeHeader.Columns.ItemNo, item.Key);
                var qrColl = new Query("RecipeHeader");
                qrColl.AddWhere(RecipeHeader.Columns.ItemNo, Comparison.Equals, item.Key);
                var rhColl = new RecipeHeaderController().FetchByQuery(qrColl).ToList();
                for (int i = 0; i < rhColl.Count; i++)
                {
                    if (rhColl[i].RecipeHeaderID != rh.RecipeHeaderID && !rh.IsNew)
                    {
                        rhColl[i].Deleted = true;
                        qmc.Add(rhColl[i].GetUpdateCommand(string.Format("{0} via WEB Recipe Importer", userName)));
                    }
                }

                string type = "Product";
                Item theItem = new Item(Item.Columns.ItemNo, item.Key);

                if (rh.IsNew)
                {
                    rh.RecipeHeaderID = RecipeController.GetNewRecipeHeaderID(rhCount);
                    rh.RecipeName = rh.RecipeHeaderID;
                    rh.ItemNo = item.Key;
                    rh.Type = type;
                    rh.Deleted = false;
                    qmc.Add(rh.GetInsertCommand(string.Format("{0} via WEB Recipe Importer", userName)));
                    for (int i = 0; i < item.Value.Count; i++)
                    {
                        item.Value[i].Deleted = false;
                        item.Value[i].RecipeHeaderID = rh.RecipeHeaderID;
                        item.Value[i].RecipeDetailID = rh.RecipeHeaderID + "." + i.ToString("000");
                        qmc.Add(item.Value[i].GetInsertCommand(string.Format("{0} via WEB Recipe Importer", userName)));
                    }
                }
                else
                {
                    rh.Type = type;
                    rh.Deleted = false;
                    qmc.Add(rh.GetUpdateCommand(string.Format("{0} via WEB Recipe Importer", userName)));

                    var theDetail = rh.RecipeDetailRecords();
                    foreach (var rd in theDetail)
                    {
                        if (item.Value.Where(o => o.ItemNo == rd.ItemNo).FirstOrDefault() == null)
                        {
                            rd.Deleted = true;
                            qmc.Add(rd.GetUpdateCommand(string.Format("{0} via WEB Recipe Importer", userName)));
                        }
                    }

                    for (int i = 0; i < item.Value.Count; i++)
                    {
                        var query = new Query("RecipeDetail");
                        query.AddWhere(RecipeDetail.Columns.RecipeHeaderID, Comparison.Equals, rh.RecipeHeaderID);
                        query.AddWhere(RecipeDetail.Columns.ItemNo, Comparison.Equals, item.Value[i].ItemNo);
                        var existingData = new RecipeDetailController().FetchByQuery(query).FirstOrDefault();
                        if (existingData == null)
                        {
                            existingData = new RecipeDetail();
                            existingData.RecipeHeaderID = rh.RecipeHeaderID;
                            existingData.RecipeDetailID = RecipeController.GetNewRecipeDetailID(rh.RecipeHeaderID, i);
                        }
                        existingData.Deleted = false;
                        existingData.ItemNo = item.Value[i].ItemNo;
                        existingData.Qty = item.Value[i].Qty;
                        existingData.Uom = item.Value[i].Uom;
                        existingData.IsPacking = item.Value[i].IsPacking;
                        if (existingData.IsNew)
                            qmc.Add(existingData.GetInsertCommand(string.Format("{0} via WEB Recipe Importer", userName)));
                        else
                            qmc.Add(existingData.GetUpdateCommand(string.Format("{0} via WEB Recipe Importer", userName)));
                    }
                }
            }

            #endregion

            if (qmc.Count > 0)
                DataService.ExecuteTransaction(qmc);

            #region *) Cyclic Detection

            var mainItemNoList = (from o in result.AsEnumerable()
                                  select o.Field<string>("ItemNo")).Distinct().ToList();

            QueryCommandCollection qmcCyclic = new QueryCommandCollection();
            Dictionary<string, string> cyclicList = new Dictionary<string, string>();
            foreach (var itemNo in mainItemNoList)
            {
                string cyclicItemNo = "";
                if (RecipeController.IsCylicRecipe(itemNo, out cyclicItemNo))
                    cyclicList.Add(itemNo, cyclicItemNo);
            }

            foreach (var item in cyclicList)
            {
                for (int i = 0; i < result.Rows.Count; i++)
                {
                    if ((result.Rows[i]["ItemNo"] + "") == item.Key)
                    {
                        string sqlUpdateHDR = string.Format("UPDATE RecipeHeader SET Deleted = 1, ModifiedOn = GETDATE(), ModifiedBy = 'Cyclic Recipe Found : {1}' WHERE ItemNo = '{0}'", item.Key, item.Value);
                        result.Rows[i]["Status"] = "Cyclic Recipe Found :" + item.Value;
                        qmcCyclic.Add(new QueryCommand(sqlUpdateHDR));
                    }
                }
            }

            if (qmcCyclic.Count > 0)
                DataService.ExecuteTransaction(qmcCyclic);

            #endregion

            isSuccess = true;
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
            isSuccess = false;
        }

        return isSuccess;
    }
}
