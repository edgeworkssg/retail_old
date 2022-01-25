using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SubSonic;

namespace PowerPOS
{
    public class RecipeDeduction
    {
        public string ItemNo { get; set; }
        public decimal Quantity { get; set; }
    }

    public class RecipeData
    {
        public string RecipeHeaderID { set; get; }
        public string MainItemNo { set; get; }
        public string MainItemName { set; get; }
        public string RecipeDetailID { set; get; }
        public string ItemNo { set; get; }
        public string ItemName { set; get; }
    }

    public class RecipeTreeNode
    {
        public string MainItemNo { set; get; }
        public bool IsVisited { set; get; }
        private List<RecipeTreeNode> _childData = new List<RecipeTreeNode>();
        public List<RecipeTreeNode> ChildData
        {
            get { return _childData; }
            set { _childData = value; }
        }

        public void Print(string param)
        {
            Console.WriteLine(MainItemNo + param+(_childData.Count==0 ? "[END HERE]" : ""));
            foreach (var item in _childData)
                item.Print(param + "~" + MainItemNo);
        }
    }

    public class Paired
    {
        public string MainItemNo { set; get; }
        public string ChildItemNo { set; get; }
        public override string ToString()
        {
            return MainItemNo + " ~ " + ChildItemNo;
        }
    }

    public partial class RecipeController
    {
//        public static bool ValidateRecipeAttributeSetup(string itemNo, List<string> attributeGroups, out string status)
//        {
//            bool isSuccess = false;
//            status = "";

//            try
//            {
//                string sql = @"
//SELECT   DISTINCT 
//		 AG.AttributesGroupCode
//		,AG.AttributesGroupName
//		,ATT.AttributesCode
//		,ATT.AttributesName
//		,RH.RecipeHeaderID
//FROM	AttributesGroup AG
//		INNER JOIN Attributes ATT ON ATT.AttributesGroupCode = AG.AttributesGroupCode
//		INNER JOIN RecipeHeader RH ON RH.ItemNo = ATT.AttributesCode
//		INNER JOIN RecipeDetail RD ON RD.RecipeHeaderID = RH.RecipeHeaderID 
//									  AND ISNULL(RD.Deleted,0) = 0
//									  AND RD.Qty < 0
//WHERE	ISNULL(ATT.Deleted,0) = 0
//		AND ISNULL(RH.Deleted,0) = 0
//		AND AG.AttributesGroupCode IN ('{0}')";
//                sql = string.Format(sql, string.Join("','", attributeGroups.ToArray()));
//                DataTable dtRecipeHdr = new DataTable();
//                dtRecipeHdr.Load(DataService.GetReader(new QueryCommand(sql)));

//                if (dtRecipeHdr.Rows.Count == 0)
//                    return true;

//                RecipeDetailCollection materials = ItemController.FetchItemMaterial(itemNo, out status);
//                if (!string.IsNullOrEmpty(status))
//                    throw new Exception(status);

//                List<string> errorList = new List<string>();
//                for (int i = 0; i < dtRecipeHdr.Rows.Count; i++)
//                {
//                    string attributesName = dtRecipeHdr.Rows[i]["AttributesName"] + "";
//                    string attributesGroupName = dtRecipeHdr.Rows[i]["AttributesGroupName"] + "";
//                    string recipeHeaderID = dtRecipeHdr.Rows[i]["RecipeHeaderID"] + "";
//                    string error = "";
//                    if (materials.Count == 0)
//                    {
//                        error = string.Format("- Attribute {0} in the Attributes Group {1} is not valid in this item", 
//                            attributesName, attributesGroupName);
//                    }
//                    else
//                    {
//                        sql = @"SELECT   RD.RecipeHeaderID
//		                                ,RD.RecipeDetailID
//		                                ,RD.ItemNo
//		                                ,I.ItemName
//		                                ,RD.Qty
//		                                ,RD.UOM
//                                FROM	RecipeDetail RD
//		                                INNER JOIN Item I ON I.ItemNo = RD.ItemNo AND ISNULL(I.Deleted,0) = 0
//                                WHERE	ISNULL(RD.Deleted,0) = 0
//		                                AND RD.Qty < 0
//		                                AND RD.RecipeHeaderID = '{0}'";
//                        sql = string.Format(sql, recipeHeaderID);
//                        DataTable dtRecipe = new DataTable();
//                        dtRecipe.Load(DataService.GetReader(new QueryCommand(sql)));

//                        bool isError = false;
//                        for (int j = 0; j < dtRecipe.Rows.Count; j++)
//                        {
//                            string recipeItemNo = dtRecipe.Rows[j]["ItemNo"] + "";
//                            string recipeUOM = dtRecipe.Rows[j]["UOM"] + "";
//                            decimal recipeQty = Math.Abs((decimal)dtRecipe.Rows[j]["Qty"]);

//                            var rd = materials.Where(o => o.ItemNo == recipeItemNo).FirstOrDefault();

//                            if (rd != null)
//                            {
//                                if (recipeUOM != rd.Uom
//                                    && !string.IsNullOrEmpty(recipeUOM)
//                                    && !string.IsNullOrEmpty(rd.Uom))
//                                {
//                                    recipeQty = recipeQty * ItemController.GetConversionRate(recipeItemNo, recipeUOM, rd.Uom);
//                                }

//                                if (recipeQty > rd.Qty)
//                                {
//                                    isError = true;
//                                    break;
//                                }
//                            }
//                            else
//                            {
//                                error = string.Format("- Attribute {0} in the Attributes Group {1} is not valid in this item",
//                                   attributesName, attributesGroupName);
//                            }
//                        }
//                        if (isError)
//                        {
//                            error = string.Format("- Attribute {0} in the Attributes Group {1} is not valid in this item",
//                                attributesName, attributesGroupName); 
//                        }
//                    }
//                    if (!string.IsNullOrEmpty(error))
//                        errorList.Add(error);
//                }

//                isSuccess = errorList.Count == 0;
//                if (!isSuccess)
//                    status = "<br/>" + string.Join("<br/>", errorList.ToArray());

//            }
//            catch (Exception ex)
//            {
//                isSuccess = false;
//                status = ex.Message;
//                Logger.writeLog(ex);
//            }


//            return isSuccess;
//        }

        public static bool ValidateRecipeSetup(string attributeCode, List<RecipeDetail> details, out string status)
        {
            bool isSuccess = true;
            status = "";

            try
            {
                if (details.Count == 0) return true;

                List<string> errorList = new List<string>();
                DataTable dtItem = new DataTable();
                string sql = @"
DECLARE @AttributeCode NVARCHAR(500);
SET @AttributeCode = '{0}';

SELECT   DISTINCT I.ItemNo MainItemNo
		,I.ItemName MainItemName
		,ATT.AttributesCode
		,ISNULL(RECIPE.RecipeItemNo,'') RecipeItemNo
		,ISNULL(RECIPE.RecipeItemName,'') RecipeItemName
		,ISNULL(RECIPE.RecipeItemUOM,'') RecipeItemUOM
		,ISNULL(RECIPE.RecipeQty,0) RecipeQty
		,ISNULL(RECIPE.RecipeUOM,'') RecipeUOM
FROM	Item I
		INNER JOIN ItemAttributesMap IAM ON IAM.ItemNo = I.ItemNo
		INNER JOIN AttributesGroup AG ON AG.AttributesGroupCode = IAM.AttributesGroupCode
		INNER JOIN Attributes ATT ON ATT.AttributesGroupCode = AG.AttributesGroupCode
		LEFT JOIN (
			SELECT   MainItem.ItemNo MainItemNo
					,MainItem.ItemName MainItemName
					,RD.ItemNo RecipeItemNo
					,RecipeItem.ItemName RecipeItemName
					,RecipeItem.Userfld1 RecipeItemUOM
					,RD.Qty RecipeQty
					,RD.UOM RecipeUOM
			FROM	Item MainItem
					INNER JOIN RecipeHeader RH ON RH.ItemNo = MainItem.ItemNo
					INNER JOIN RecipeDetail RD ON RD.RecipeHeaderID = RH.RecipeHeaderID
					INNER JOIN Item RecipeItem ON RecipeItem.ItemNo = RD.ItemNo
			WHERE	ISNULL(MainItem.Deleted,0) = 0
					AND ISNULL(RH.Deleted,0) = 0
					AND ISNULL(RD.Deleted,0) = 0
		) RECIPE ON RECIPE.MainItemNo = I.ItemNo
WHERE	ISNULL(I.Deleted,0) = 0
        AND ISNULL(IAM.Deleted,0) = 0
		AND ATT.AttributesCode = @AttributeCode";
                sql = string.Format(sql, attributeCode);
                dtItem.Load(DataService.GetReader(new QueryCommand(sql)));

                var listMainItem = (from o in dtItem.AsEnumerable()
                                    select new
                                    {
                                        MainItemNo = o.Field<string>("MainItemNo"),
                                        MainItemName = o.Field<string>("MainItemName")
                                    }).Distinct().ToList();

                foreach (var mainItem in listMainItem)
                {
                    foreach (var rd in details)
                    {
                        if (rd.Qty < 0)
                        {
                            var mainItemDet = (from o in dtItem.AsEnumerable()
                                               where o.Field<string>("MainItemNo") == mainItem.MainItemNo
                                                     && o.Field<string>("RecipeItemNo") == rd.ItemNo
                                               select new
                                               {
                                                   RecipeItemNo = o.Field<string>("RecipeItemNo"),
                                                   RecipeItemName = o.Field<string>("RecipeItemName"),
                                                   RecipeQty = o.Field<decimal?>("RecipeQty").GetValueOrDefault(0),
                                                   RecipeUOM = o.Field<string>("RecipeUOM")
                                               }).FirstOrDefault();

                            string error = "";
                            Item materialItem = new Item(rd.ItemNo);
                            if (mainItemDet == null)
                            {
                                error = string.Format("- Quantity of material {0} not valid for item {1}",
                                    materialItem.ItemName, mainItem.MainItemName);
                            }
                            else
                            {
                                decimal rdQty = Math.Abs(rd.Qty);

                                if (mainItemDet.RecipeUOM != rd.Uom &&
                                    !string.IsNullOrEmpty(mainItemDet.RecipeUOM) &&
                                    !string.IsNullOrEmpty(rd.Uom))
                                {
                                    rdQty = rdQty * ItemController.GetConversionRate(rd.ItemNo, rd.Uom, mainItemDet.RecipeUOM);
                                }

                                if (rdQty > mainItemDet.RecipeQty)
                                {
                                    error = string.Format("- Quantity of material {0} not valid for item {1}",
                                       materialItem.ItemName, mainItem.MainItemName);
                                }
                            }
                            if (!string.IsNullOrEmpty(error))
                                errorList.Add(error);
                        }
                    } 
                }

                isSuccess = errorList.Count == 0;
                if(!isSuccess)
                    status = "<br/>"+string.Join("<br/>", errorList.ToArray());
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }

            return isSuccess;
        }

        public static string GetNewRecipeHeaderID()
        {
            string objReturn = "R1";
            var strquery = "select max(isnull(cast(right(recipeheaderid, len(recipeheaderid) -1) as int), 0)) from recipeheader";
            DataSet ds = DataService.GetDataSet(new QueryCommand(strquery));

            if (ds.Tables[0].Rows.Count > 0 && !string.IsNullOrEmpty(ds.Tables[0].Rows[0][0].ToString()))
            {
                string ID = ds.Tables[0].Rows[0][0].ToString();
                int index = Int32.Parse(ID);
                objReturn = 'R' + (index + 1).ToString();
            }

            return objReturn;
        }

        public static string GetNewRecipeHeaderID(int add)
        {
            string objReturn;
            int index = 0;
            try
            {
                string sql = @"SELECT	MAX(ISNULL(CAST(RIGHT(RecipeHeaderId, LEN(RecipeHeaderId) -1) AS INT), 0)) ID
                           FROM	RecipeHeader";
                DataTable dt = new DataTable();
                dt.Load(DataService.GetReader(new QueryCommand(sql)));
                if (dt.Rows.Count > 0)
                    index = (dt.Rows[0]["ID"] + "").GetIntValue();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
            objReturn = 'R' + (index + 1 + add).ToString();
            return objReturn;
        }

        public static string GetNewRecipeDetailID(string HeaderID)
        {
            string objReturn = HeaderID + ".001";
            var strquery = "select Max(RecipeDetailID) as MaxReceiptDetailID from RecipeDetail where RecipeHeaderID = '" + HeaderID + "'";
            DataSet ds = DataService.GetDataSet(new QueryCommand(strquery));

            if (ds.Tables[0].Rows.Count > 0 && !string.IsNullOrEmpty(ds.Tables[0].Rows[0][0].ToString()))
            {
                var ID = ds.Tables[0].Rows[0][0].ToString().Split('.');
                var index = Int32.Parse(ID[1]);
                objReturn = HeaderID + '.' + (index + 1).ToString("000");
            }

            return objReturn;
        }

        public static string GetNewRecipeDetailID(string headerID, int add)
        {
            string id = "";

            string sql = @"SELECT  ISNULL(MAX(CAST(REPLACE(RecipeDetailID,RecipeHeaderID+'.','') AS INT)),0) RecipeDetailID
                            FROM   RecipeDetail 
                            WHERE  RecipeHeaderID = '{0}'";

            sql = string.Format(sql, headerID);
            DataTable dt = new DataTable();
            dt.Load(DataService.GetReader(new QueryCommand(sql)));
            int number = 0;

            if (dt.Rows.Count > 0)
            {
                number = (dt.Rows[0]["RecipeDetailID"] + "").GetIntValue();
            }
            number += (add + 1);
            id = headerID + "." + number.ToString("000");
            return id;
        }

        public static List<RecipeDeduction> GetRecipeDeductionList(string mainItemNo, decimal mainQuantity, int invLocationID, bool deductFirstItem, bool deductPackingMaterial, out bool isSuccess, out string status)
        {
            List<RecipeDeduction> data = new List<RecipeDeduction>();
            isSuccess = false;
            status = "";

            try
            {
                string cyclicRecipe = "";
                Item theItem = new Item(mainItemNo);
                if (RecipeController.IsCylicRecipe(mainItemNo, out cyclicRecipe))
                    throw new Exception(string.Format("Cyclic recipe found for item {0}", theItem.ItemName));
                if (!theItem.IsNew && !theItem.Deleted.GetValueOrDefault(false))
                {
                    RecipeDetailCollection materials = ItemController.FetchItemMaterial(theItem.ItemNo, out status);
                    if (!string.IsNullOrEmpty(status))
                        throw new Exception(status);
                    bool enableRecipe = (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Recipe.EnableRecipeManagement), false));

                    if (materials.Count == 0 || !enableRecipe)
                    {
                        if (theItem.IsInInventory && deductFirstItem)
                        {
                            data.Add(new RecipeDeduction
                            {
                                ItemNo = theItem.ItemNo,
                                Quantity = mainQuantity
                            });
                        }
                    }
                    else
                    {
                        decimal leftOverQty = mainQuantity;
                        if (theItem.IsInInventory && deductFirstItem)
                        {
                            decimal balanceQty = InventoryController.GetStockBalanceQtyByItem(theItem.ItemNo, invLocationID, out status);
                            if (!string.IsNullOrEmpty(status))
                                throw new Exception(status);
                            if (balanceQty < 0)
                                balanceQty = 0;

                            leftOverQty = balanceQty - mainQuantity;
                            if (leftOverQty < 0)
                            {
                                leftOverQty = leftOverQty * -1;
                                if (balanceQty > 0)
                                {
                                    data.Add(new RecipeDeduction
                                    {
                                        ItemNo = theItem.ItemNo,
                                        Quantity = balanceQty
                                    });
                                }
                            }
                            else
                            {
                                leftOverQty = 0;
                                data.Add(new RecipeDeduction
                                {
                                    ItemNo = theItem.ItemNo,
                                    Quantity = mainQuantity
                                });
                            }
                        }

                        if (leftOverQty > 0)
                        {
                            foreach (RecipeDetail rd in materials)
                            {
                                if (rd.IsPacking.GetValueOrDefault(true) && !deductPackingMaterial)
                                    continue;
                                Item rdItem = new Item(rd.ItemNo);

                                decimal baseQty = rd.Qty * ItemController.GetConversionRate(rd.ItemNo, rd.Uom, rdItem.UOM);
                                decimal recipeQty = leftOverQty * baseQty;
                                var childData = GetRecipeDeductionList(rd.ItemNo, recipeQty, invLocationID, true, deductPackingMaterial, out isSuccess, out status);
                                if (!isSuccess)
                                    throw new Exception(status);
                                else
                                    data.AddRange(childData);
                            }
                        }
                    }
                }

                isSuccess = true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                isSuccess = false;
            }

            return data;
        }

        public static bool IsCylicRecipe(string itemNo, out string cyclicItemNo)
        {
            bool isCyclic = false;
            cyclicItemNo = "";
            try
            {
                string status = "";

                List<Paired> pairedItems = new List<Paired>();

                #region *) Fetch Recipe Combination
                
                List<string> processedItems = new List<string>();
                List<string> unprocessedItems = new List<string>();

                unprocessedItems.Add(itemNo);
                while (unprocessedItems.Count != 0)
                {
                    string itemNoToCheck = unprocessedItems.FirstOrDefault();
                    unprocessedItems.Remove(itemNoToCheck);

                    if (!processedItems.Contains(itemNoToCheck))
                    {
                        var materialHeader = ItemController.FetchItemMaterial(itemNoToCheck, out status);
                        if (!string.IsNullOrEmpty(status))
                            throw new Exception(status);
                        processedItems.Add(itemNoToCheck);

                        foreach (var mtrHdrItem in materialHeader)
                        {
                            var materialDet = ItemController.FetchItemMaterial(mtrHdrItem.ItemNo, out status);
                            if (!string.IsNullOrEmpty(status))
                                throw new Exception(status);
                            if (materialDet.Count > 0 && !unprocessedItems.Contains(mtrHdrItem.ItemNo))
                                unprocessedItems.Add(mtrHdrItem.ItemNo);

                            pairedItems.Add(new Paired
                            {
                                MainItemNo = itemNoToCheck,
                                ChildItemNo = mtrHdrItem.ItemNo
                            });
                        }
                    }
                }

                #endregion

                foreach (var item in pairedItems)
                {
                    if (item.MainItemNo.ToLower().Equals(item.ChildItemNo.ToLower()))
                    {
                        cyclicItemNo = item.ChildItemNo;
                        isCyclic = true;
                        break;
                    }
                }
                if (!isCyclic)
                {
                    var recipeTree = GetRecipeTree(itemNo, pairedItems);
                    var allPath = GetAllPossiblePath(recipeTree);

                    string cyclicRecipePath = "";

                    foreach (var path in allPath)
                    {
                        for (int i = 0; i < path.Count; i++)
                        {
                            for (int j = i + 1; j < path.Count; j++)
                            {
                                if (path[i].ToLower().Equals(path[j].ToLower()))
                                {
                                    cyclicRecipePath = string.Join(",", path.ToArray());
                                    isCyclic = true;
                                    cyclicItemNo = path[j];
                                    Logger.writeLog(">>> Recipe Cyclic Detected:"+cyclicRecipePath);

                                    break;
                                }
                            }
                            if (isCyclic)
                                break;
                        }
                        if (isCyclic)
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }

            return isCyclic;
        }

        private static List<List<string>> GetAllPossiblePath(RecipeTreeNode treeNode)
        {
            List<List<string>> result = new List<List<string>>();
            while (treeNode.IsVisited == false)
            {
                string resultString = GetPathHelper(treeNode);
                if (resultString.EndsWith("[[END HERE]]"))
                {
                    //Console.WriteLine(resultString.Replace("[[SPLIT]]", ","));
                    var resultItem = resultString.Replace("[[END HERE]]","").Split(new string[] { "[[SPLIT]]" }, StringSplitOptions.None).ToList();
                    result.Add(resultItem);
                }
            }

            return result;
        }

        private static string GetPathHelper(RecipeTreeNode treeNode)
        {
            string result = "";
            if (treeNode.ChildData.Count == 0)
                result += treeNode.MainItemNo + "[[END HERE]]";
            else
                result += treeNode.MainItemNo + "[[SPLIT]]";

            var unvisitiedChild = treeNode.ChildData.Where(o => o.IsVisited == false).FirstOrDefault();
            if (unvisitiedChild == null)
                treeNode.IsVisited = true;
            else
                result+=GetPathHelper(unvisitiedChild);

            return result;
        }

        private static RecipeTreeNode GetRecipeTree(string key, List<Paired> dataSet)
        {
            RecipeTreeNode result = new RecipeTreeNode();
            var selections = dataSet.Where(o => o.MainItemNo.ToLower() == key.ToLower()).ToList();
            result.MainItemNo = key;
            if (selections.Count > 0)
            {
                var newDataSet = dataSet.ToList();
                List<RecipeTreeNode> childData = new List<RecipeTreeNode>();
                foreach (var sel in selections)
                {
                    newDataSet.RemoveAll(o => o.MainItemNo.ToLower() == sel.MainItemNo.ToLower()
                                        && o.ChildItemNo.ToLower() == sel.ChildItemNo.ToLower());
                }
                foreach (var sel in selections)
                {
                    childData.Add(GetRecipeTree(sel.ChildItemNo, newDataSet));
                }
                result.ChildData = childData;
            }
            return result;
        }

        public static decimal GetTotalRecipeCost(string recipeHeaderID)
        {
            decimal totalCost = 0;

            try
            {
                //bool isDeducRecipeSuccess = false;
                //string statusDeductRecipe = "";
                //var recipeDetList = RecipeController.GetRecipeDeductionList(item.ItemNo,
                //    Convert.ToDouble(1), data.InventoryLocationID,
                //    true, false, out isDeducRecipeSuccess, out statusDeductRecipe);
                //if (!isDeducRecipeSuccess)
                //    throw new Exception(statusDeductRecipe);
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }

            return totalCost;
        }

        public static decimal GetTotalCostRecipeHeader(string RecipeHeaderID)
        {
            decimal objReturn = 0;

            try
            {
                RecipeDetailCollection col = new RecipeDetailCollection();
                col.Where(RecipeDetail.Columns.RecipeHeaderID, RecipeHeaderID);
                col.Where(RecipeDetail.Columns.Deleted, false);
                col.Load();

                for (int i = 0; i < col.Count; i++)
                {
                    var subTotal = Math.Round(GetTotalCostRecipeDet(col[i].RecipeDetailID, 0), 4);
                    objReturn += subTotal;
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog("Error calculate cost " + RecipeHeaderID + " : " + ex.Message);
            }

            return objReturn;
        }

        public static decimal GetTotalCostRecipeDet(string RecipeDetID, int maxdeep)
        {
            decimal objReturn = 0;

            try
            {
                RecipeDetail rd = new RecipeDetail(RecipeDetID);

                decimal costprice = GetTotalCostItemNo(rd.ItemNo, maxdeep);

                string query = @"
                    select cast(ISNULL(ud.ConversionRate,1) as decimal(18,4)) as convrate
                    from recipedetail rd 
                    inner join item i on rd.itemno = i.itemno and ISNULL(i.Deleted, 0) = 0
                    left join UOMConversionHdr uh on uh.ItemNo = rd.ItemNo and ISNULL(uh.Deleted,0) = 0
                    left join UOMConversionDet ud on uh.ConversionHdrID = ud.ConversionHdrID and  ISNULL(ud.Deleted,0) = 0 and ud.FromUOM = i.Userfld1 and ud.ToUOM = rd.UOM
                    where rd.RecipeDetailID = '{0}' and ISNULL(rd.Deleted,0) = 0
                    ";

                query = string.Format(query, RecipeDetID);
                decimal convrate = DataService.ExecuteScalar(new QueryCommand(query)).ToString().GetDecimalValue();

                objReturn = costprice * ((decimal)rd.Qty / convrate);
            }
            catch (Exception ex)
            {
                Logger.writeLog("error calculate cost recipe detail :" + ex.Message);
            }

            return objReturn;
        }

        public static decimal GetTotalCostItemNo(string ItemNo, int maxdeep)
        {
            decimal objReturn = 0;
            try
            {
                if (maxdeep <= 3)
                {

                    Item rd = new Item(ItemNo);

                    if (rd != null && !rd.IsNew)
                    {
                        /*cek if detail is a recipe too the go iterate the detail*/
                        RecipeHeaderCollection rhcol = new RecipeHeaderCollection();
                        rhcol.Where(RecipeHeader.Columns.ItemNo, rd.ItemNo);
                        rhcol.Where(RecipeHeader.Columns.Deleted, false);
                        rhcol.Load();

                        if (rhcol.Count() > 0)
                        {
                            string RecipeHeaderID = rhcol[0].RecipeHeaderID;
                            maxdeep++;

                            RecipeDetailCollection rdcol = new RecipeDetailCollection();
                            rdcol.Where(RecipeDetail.Columns.RecipeHeaderID, RecipeHeaderID);
                            rdcol.Where(RecipeDetail.Columns.Deleted, false);
                            rdcol.Load();

                            if (rdcol.Count() > 0)
                            {
                                for (int i = 0; i < rdcol.Count; i++)
                                {
                                    objReturn += GetTotalCostRecipeDet(rdcol[i].RecipeDetailID, maxdeep);
                                }
                            }
                        }
                        else
                        {
                            string query = @"SELECT	ISNULL(MAX(ISM.CostPrice),MAX(It.FactoryPrice)) CostPrice
		                                    ,MAX(ISNULL(SUP.Userfld2,'')) Currency
                                    FROM	Item It
		                                    LEFT JOIN ItemSupplierMap ISM ON ISM.ItemNo = It.ItemNo
										                                     AND ISNULL(ISM.Deleted,0)= 0
		                                    LEFT JOIN Supplier SUP ON SUP.SupplierID = ISM.SupplierID
                                    WHERE It.Itemno = '{0}'
                                    GROUP BY It.ItemNo";
                            query = string.Format(query, ItemNo);
                            DataTable dt = new DataTable();
                            dt.Load(DataService.GetReader(new QueryCommand(query)));
                            decimal cost = 0;
                            if (dt.Rows.Count > 0)
                            {
                                cost = (dt.Rows[0]["CostPrice"] + "").GetDecimalValue();
                                //string curr = (dt.Rows[0]["Currency"] + "");
                                //Currency cr = new Currency(Currency.Columns.CurrencyCode, curr);
                                //if (!cr.IsNew)
                                //{
                                //    var rate = cr.ExchangeRateToDefaultCurrency;
                                //    Logger.writeLog(string.Format("Currency Rate {0} : {1}", cr.CurrencyCode, rate));
                                //    cost = cost * rate;
                                //}
                                //else
                                //{
                                //    Logger.writeLog("Currency Not Found : " + curr);
                                //}
                            }

                            objReturn += cost;
                        }
                    }
                }
                else 
                {
                    //it is for recipe that have same item for recipe header and detail
                    string query = @"SELECT	ISNULL(MAX(ISM.CostPrice),MAX(It.FactoryPrice)) CostPrice
		                                    ,MAX(ISNULL(SUP.Userfld2,'')) Currency
                                    FROM	Item It
		                                    LEFT JOIN ItemSupplierMap ISM ON ISM.ItemNo = It.ItemNo
										                                     AND ISNULL(ISM.Deleted,0)= 0
		                                    LEFT JOIN Supplier SUP ON SUP.SupplierID = ISM.SupplierID
                                    WHERE It.Itemno = '{0}'
                                    GROUP BY It.ItemNo";
                    query = string.Format(query, ItemNo);
                    DataTable dt = new DataTable();
                    dt.Load(DataService.GetReader(new QueryCommand(query)));
                    decimal cost = 0;
                    if (dt.Rows.Count > 0)
                    {
                        cost = (dt.Rows[0]["CostPrice"] + "").GetDecimalValue();
                        //string curr = (dt.Rows[0]["Currency"] + "");
                        //Currency cr = new Currency(Currency.Columns.CurrencyCode, curr);
                        //if (!cr.IsNew)
                        //{
                        //    var rate = cr.ExchangeRateToDefaultCurrency;
                        //    Logger.writeLog(string.Format("Currency Rate {0} : {1}", cr.CurrencyCode, rate));
                        //    cost = cost * rate;
                        //}
                        //else
                        //{
                        //    Logger.writeLog("Currency Not Found : " + curr);
                        //}
                    }
                   
                    objReturn += cost;
                
                }

            }
            catch (Exception ex)
            {
                Logger.writeLog("error calculate cost recipe detail :");
                Logger.writeLog(ex);
            }


            return objReturn;
        }
    }
}
