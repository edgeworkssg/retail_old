using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using SubSonic;
using System.Collections;
using PowerPOS.Container;
using System.ComponentModel;

namespace PowerPOS
{
    [Serializable]
    public partial class ApplyPromotionController
    {
        POSController pos; //POS business object upon which the promo need to be applied        
        private int CurrentPromoHdrID;
        private string OrderLineID;  //the order line that trigger the promo??        

        public static DataTable viewPromoMasterDetailAny;
        public static bool isPromoUpdated = false;
        public static bool hasOldPromo;

        public ApplyPromotionController(POSController mypos)
        {
            CurrentPromoHdrID = -1;
            OrderLineID = "";
            pos = mypos;
        }

        /*public void AddPromotion(PromoCampaignDet promoDet)
        {
            if (viewPromoMasterDetailAny == null)
                viewPromoMasterDetailAny = new DataTable();

            if (promoDet.AnyQty > 0)
            {
                if (viewPromoMasterDetailAny.Columns.Count == 0)
                {
                    Logger.writeLog("Promo Log - Add Promo New ");
                    string sqlString = @"
                    SELECT h.PromoCampaignHdrID, d.PromoCampaignDetID ,i.ItemNo, i.ItemName,i.CategoryName, ISNULL(d.UnitQty,0) as UnitQty, ISNULL(d.AnyQty,0) as AnyQty, d.MinQuantity
                    , ISNULL(d.PromoPrice,0) as PromoPrice, ISNULL(d.DiscAmount,0) as DiscAmount, ISNULL(d.DiscPercent,DiscPercent) as DiscPercent
                    FROM PromoCampaignDet d 
                    inner join Item i on d.CategoryName = i.CategoryName
                    inner join PromoCampaignHdr h on d.PromoCampaignHdrID = h.PromoCampaignHdrID
                    WHERE ISNULL(d.ItemNo,'') = '' and ISNULL(d.ItemGroupID,'') = '' AND ISNULL(d.Deleted,0) = 0 AND ISNULL(i.Deleted, 0) = 0
	                    AND ISNULL(h.Deleted,0) = 0 and h.CampaignType = 'AnyXOffAllItems' and d.PromoCampaignDetID = @PromoHdrID  
                    UNION
                    SELECT h.PromoCampaignHdrID, d.PromoCampaignDetID,i.ItemNo, i.ItemName,i.CategoryName, (ISNULL(m.UnitQty,0)*ISNULL(d.UnitQty,0)) as UnitQty, ISNULL(d.AnyQty,0) as AnyQty, d.MinQuantity
                    , ISNULL(d.PromoPrice,0) as PromoPrice, ISNULL(d.DiscAmount,0) as DiscAmount, ISNULL(d.DiscPercent,DiscPercent) as DiscPercent
                    FROM PromoCampaignDet d 
                    inner join ItemGroupMap m on d.ItemGroupId = m.ItemGroupID 
                    inner join Item i on m.ItemNo = i.ItemNo
                    inner join PromoCampaignHdr h on d.PromoCampaignHdrID = h.PromoCampaignHdrID
                    WHERE ISNULL(d.ItemNo,'') = '' and ISNULL(d.CategoryName,'') = '' AND ISNULL(d.Deleted,0) = 0 AND ISNULL(i.Deleted, 0) = 0
	                    AND ISNULL(h.Deleted,0) = 0 and h.CampaignType = 'AnyXOffAllItems' AND ISNULL(m.Deleted,0) = 0 and d.PromoCampaignDetID = @PromoHdrID  
                      ";

                    sqlString = sqlString.Replace("@PromoHdrID", promoDet.PromoCampaignDetID.ToString());
                    viewPromoMasterDetailAny = DataService.GetDataSet(new QueryCommand(sqlString)).Tables[0];
                    Logger.writeLog("Promo Log - End Promo New ");
                }
                else
                {
                    if (viewPromoMasterDetailAny.Select("PromoCampaignDetID = " + promoDet.PromoCampaignDetID.ToString()).Length < 1)
                    {
                        //Add 
                        Logger.writeLog("Promo Log - Add Promotion  ");
                        string sqlString = @"
                        SELECT h.PromoCampaignHdrID, d.PromoCampaignDetID ,i.ItemNo, i.ItemName,i.CategoryName, ISNULL(d.UnitQty,0) as UnitQty, ISNULL(d.AnyQty,0) as AnyQty, d.MinQuantity
                        , ISNULL(d.PromoPrice,0) as PromoPrice, ISNULL(d.DiscAmount,0) as DiscAmount, ISNULL(d.DiscPercent,DiscPercent) as DiscPercent
                        FROM PromoCampaignDet d 
                        inner join Item i on d.CategoryName = i.CategoryName
                        inner join PromoCampaignHdr h on d.PromoCampaignHdrID = h.PromoCampaignHdrID
                        WHERE ISNULL(d.ItemNo,'') = '' and ISNULL(d.ItemGroupID,'') = '' AND ISNULL(d.Deleted,0) = 0 AND ISNULL(i.Deleted, 0) = 0
	                        AND ISNULL(h.Deleted,0) = 0 and h.CampaignType = 'AnyXOffAllItems' and d.PromoCampaignDetID = @PromoHdrID  
                        UNION
                        SELECT h.PromoCampaignHdrID, d.PromoCampaignDetID,i.ItemNo, i.ItemName,i.CategoryName, (ISNULL(m.UnitQty,0)*ISNULL(d.UnitQty,0)) as UnitQty, ISNULL(d.AnyQty,0) as AnyQty, d.MinQuantity
                        , ISNULL(d.PromoPrice,0) as PromoPrice, ISNULL(d.DiscAmount,0) as DiscAmount, ISNULL(d.DiscPercent,DiscPercent) as DiscPercent
                        FROM PromoCampaignDet d 
                        inner join ItemGroupMap m on d.ItemGroupId = m.ItemGroupID 
                        inner join Item i on m.ItemNo = i.ItemNo
                        inner join PromoCampaignHdr h on d.PromoCampaignHdrID = h.PromoCampaignHdrID
                        WHERE ISNULL(d.ItemNo,'') = '' and ISNULL(d.CategoryName,'') = '' AND ISNULL(d.Deleted,0) = 0 AND ISNULL(i.Deleted, 0) = 0
	                        AND ISNULL(h.Deleted,0) = 0 and h.CampaignType = 'AnyXOffAllItems' AND ISNULL(m.Deleted,0) = 0 and d.PromoCampaignDetID = @PromoHdrID  
                        ";
                        sqlString = sqlString.Replace("@PromoHdrID", promoDet.PromoCampaignDetID.ToString());
                        DataTable dt = DataService.GetDataSet(new QueryCommand(sqlString)).Tables[0];
                        Logger.writeLog("Promo Log - Finish Query Add Promo ");
                        foreach (DataRow dr in dt.Rows)
                        {
                            viewPromoMasterDetailAny.Rows.Add(dr.ItemArray);
                        }
                        Logger.writeLog("Promo Log - Finish Copy Row ");

                    }
                }
            }
            else
            {
                
                if (viewPromoMasterDetailAny.Columns.Count == 0)
                {
                    Logger.writeLog("Promo Log - Add Promo New ");
                    string sqlString = @"
                    SELECT h.PromoCampaignHdrID, d.PromoCampaignDetID ,d.ItemNo, i.ItemName ,i.CategoryName ,ISNULL(d.UnitQty,0) as UnitQty, ISNULL(d.AnyQty,0) as AnyQty, d.MinQuantity
                    , ISNULL(d.PromoPrice,0) as PromoPrice, ISNULL(d.DiscAmount,0) as DiscAmount, ISNULL(d.DiscPercent,DiscPercent) as DiscPercent
                    FROM PromoCampaignDet d 
                    INNER JOIN Item i on d.ItemNo = i.ItemNo
                    INNER JOIN PromoCampaignHdr h on d.PromoCampaignHdrID = h.PromoCampaignHdrID
                    WHERE ISNULL(d.ItemGroupID,'') = '' AND ISNULL(d.ItemNo,'') <> '' AND ISNULL(d.Deleted,0) = 0 AND ISNULL(i.Deleted, 0) = 0
	                AND ISNULL(h.Deleted,0) = 0 and h.CampaignType = 'AnyXOffAllItems' and d.PromoCampaignDetID = @PromoHdrID
                      ";

                    sqlString = sqlString.Replace("@PromoHdrID", promoDet.PromoCampaignDetID.ToString());
                    viewPromoMasterDetailAny = DataService.GetDataSet(new QueryCommand(sqlString)).Tables[0];
                    Logger.writeLog("Promo Log - End Promo New ");
                }
                else
                {
                    if (viewPromoMasterDetailAny.Select("PromoCampaignDetID = " + promoDet.PromoCampaignDetID.ToString()).Length < 1)
                    {
                        //Add 
                        Logger.writeLog("Promo Log - Add Promotion  ");
                        string sqlString = @"
                        SELECT h.PromoCampaignHdrID, d.PromoCampaignDetID ,d.ItemNo, i.ItemName ,i.CategoryName ,ISNULL(d.UnitQty,0) as UnitQty, ISNULL(d.AnyQty,0) as AnyQty, d.MinQuantity
                        , ISNULL(d.PromoPrice,0) as PromoPrice, ISNULL(d.DiscAmount,0) as DiscAmount, ISNULL(d.DiscPercent,DiscPercent) as DiscPercent
                        FROM PromoCampaignDet d 
                        INNER JOIN Item i on d.ItemNo = i.ItemNo
                        INNER JOIN PromoCampaignHdr h on d.PromoCampaignHdrID = h.PromoCampaignHdrID
                        WHERE ISNULL(d.ItemGroupID,'') = '' AND ISNULL(d.ItemNo,'') <> '' AND ISNULL(d.Deleted,0) = 0 AND ISNULL(i.Deleted, 0) = 0
	                    AND ISNULL(h.Deleted,0) = 0 and h.CampaignType = 'AnyXOffAllItems' and d.PromoCampaignDetID = @PromoHdrID 
                        ";
                        sqlString = sqlString.Replace("@PromoHdrID", promoDet.PromoCampaignDetID.ToString());
                        DataTable dt = DataService.GetDataSet(new QueryCommand(sqlString)).Tables[0];
                        Logger.writeLog("Promo Log - Finish Query Add Promo ");
                        foreach (DataRow dr in dt.Rows)
                        {
                            viewPromoMasterDetailAny.Rows.Add(dr.ItemArray);
                        }
                        Logger.writeLog("Promo Log - Finish Copy Row ");

                    }
                }
            }
                

                
                
                

            
            //viewPromoMasterDetailAny
        }*/

        public static DataTable GetViewPromoMasterDetailAny(string promoCampaignDetID)
        {
            //Change to not get from View

            DataTable dt = new DataTable();
            string sqlString = @"
                SELECT h.PromoCampaignHdrID, d.PromoCampaignDetID ,ISNULL(d.ItemGroupID,0) ItemGroupID, ISNULL(d.CategoryName,'') CategoryName 
                , ISNULL(d.UnitQty,0) as UnitQty, ISNULL(d.AnyQty,0) as AnyQty, d.MinQuantity
                , ISNULL(d.PromoPrice,0) as PromoPrice, ISNULL(d.DiscAmount,0) as DiscAmount, ISNULL(d.DiscPercent,DiscPercent) as DiscPercent
                FROM PromoCampaignDet d 
                inner join PromoCampaignHdr h on d.PromoCampaignHdrID = h.PromoCampaignHdrID
                WHERE ISNULL(d.ItemNo,'') = '' and ISNULL(d.ItemGroupID,'') = '' AND ISNULL(d.Deleted,0) = 0
	                AND ISNULL(h.Deleted,0) = 0 and h.CampaignType = 'AnyXOffAllItems' and d.PromoCampaignDetID = @PromoDetID 
                UNION
                SELECT h.PromoCampaignHdrID, d.PromoCampaignDetID, ISNULL(d.ItemGroupID,0) ItemGroupID, ISNULL(d.CategoryName,'') CategoryName, 
                ISNULL(d.UnitQty,0) as UnitQty, ISNULL(d.AnyQty,0) as AnyQty, d.MinQuantity
                , ISNULL(d.PromoPrice,0) as PromoPrice, ISNULL(d.DiscAmount,0) as DiscAmount, ISNULL(d.DiscPercent,DiscPercent) as DiscPercent
                FROM PromoCampaignDet d 
                inner join PromoCampaignHdr h on d.PromoCampaignHdrID = h.PromoCampaignHdrID
                WHERE ISNULL(d.ItemNo,'') = '' and ISNULL(d.CategoryName,'') = '' AND ISNULL(d.Deleted,0) = 0 
	                AND ISNULL(h.Deleted,0) = 0 and h.CampaignType = 'AnyXOffAllItems' and d.PromoCampaignDetID = @PromoDetID  ";

            sqlString = sqlString.Replace("@PromoDetID", promoCampaignDetID);
            dt = DataService.GetDataSet(new QueryCommand(sqlString)).Tables[0];
            return dt;
            /*if (isPromoUpdated)
            {
                viewPromoMasterDetailAny = new ViewPromoMasterDetailAnyCollection().Load().ToDataTable();
                isPromoUpdated = false;
                return viewPromoMasterDetailAny;
            }
            else
            {
                return viewPromoMasterDetailAny;
            }*/
            //return viewPromoMasterDetailAny;
        }

        public static DataTable GetViewPromoMasterDetailUnitQty(string promoCampaignDetID)
        {
            //Change to not get from View

            DataTable dt = new DataTable();
            string sqlString = @"SELECT h.PromoCampaignHdrID, d.PromoCampaignDetID ,d.ItemNo, ISNULL(d.UnitQty,0) as UnitQty, ISNULL(d.AnyQty,0) as AnyQty, d.MinQuantity
                , ISNULL(d.PromoPrice,0) as PromoPrice, ISNULL(d.DiscAmount,0) as DiscAmount, ISNULL(d.DiscPercent,DiscPercent) as DiscPercent
                FROM PromoCampaignDet d 
                INNER JOIN PromoCampaignHdr h on d.PromoCampaignHdrID = h.PromoCampaignHdrID
                WHERE ISNULL(d.ItemGroupID,'') = '' AND ISNULL(d.ItemNo,'') <> '' AND ISNULL(d.Deleted,0) = 0
	            AND ISNULL(h.Deleted,0) = 0 and h.CampaignType = 'AnyXOffAllItems' and d.PromoCampaignDetID = @PromoDetID 
                 ";

            sqlString = sqlString.Replace("@PromoDetID", promoCampaignDetID);
            dt = DataService.GetDataSet(new QueryCommand(sqlString)).Tables[0];
            return dt;

            //return viewPromoMasterDetailAny;
            /*if (isPromoUpdated)
            {
                viewPromoMasterDetailAny = new ViewPromoMasterDetailAnyCollection().Load().ToDataTable();
                isPromoUpdated = false;
                return viewPromoMasterDetailAny;
            }
            else
            {
                return viewPromoMasterDetailAny;
            }*/
        }


        public void ApplyPromoToOrder()
        {
            OrderDetCollection myOrderLine = pos.FetchUnsavedOrderDet();
            //Apply Any X of All Items        
            Logger.writeLog("Promo Log - 9. Apply Promo");
            ApplyAnyXoffAllItems();
            Logger.writeLog("Promo Log - 9. Finish Apply Promo");

            //Logger.writeLog("Promo Log - Apply Buy X  at Y");
            //Apply Buy X at the price of Y
            //ApplyBuyXAtThePriceOfYDisount();
            
            //Apply item groups   
            //Logger.writeLog("Promo Log - Apply Item Group Discount ");
            /*while (true)
            {
                ViewPromoMasterDetailCollection col = ValidItemGroupPromo(myOrderLine);
                if (col == null || col.Count == 0)
                    break;

                //apply the first one....
                ApplyItemGroupDisount(col[0]);

                
            }*/

            #region *) set possible promo after apply promo
            OrderDetCollection OrderDets = pos.FetchUnsavedOrderDet();
            for (int i = 0; i < OrderDets.Count; i++)
            {
                if (IsValidPromoLineItem(OrderDets[i]))
                {
                    string ItemNoList1 = "'" + OrderDets[i].ItemNo + "'";

                    OrderDets[i].PossibleItemGroupID = "";
                    string sqlCheckItemGroup = "Select Distinct ItemGroupID from ItemGroupMap where deleted = 0 and itemno = '" + OrderDets[i].ItemNo + "'";
                    DataTable dtItemGroup = DataService.GetDataSet(new QueryCommand(sqlCheckItemGroup)).Tables[0];
                    for (int j = 0; j < dtItemGroup.Rows.Count; j++)
                    {
                        if (dtItemGroup.Rows[j][0].ToString() != "0")
                        {
                            OrderDets[i].PossibleItemGroupID += dtItemGroup.Rows[j][0].ToString() + ",";
                        }
                    }
                    OrderDets[i].PossibleItemGroupID = OrderDets[i].PossibleItemGroupID.TrimEnd(',');

                    //GetMember Group
                    int memberGroupID = 0;
                    if (pos.MembershipApplied())
                    {
                        memberGroupID = pos.GetMemberInfo().MembershipGroupId;
                    }

                    string ItemGroupList = "-1";
                    if (OrderDets[i].PossibleItemGroupID != "")
                        ItemGroupList = OrderDets[i].PossibleItemGroupID;
                    DataSet ds = SPs.FetchAllPossiblePromoAnyXofAllItemsMemberGroup(ItemNoList1, pos.MembershipApplied() && pos.GetMemberInfo().MembershipNo != "WALK-IN", PointOfSaleInfo.PointOfSaleID, memberGroupID, OrderDets[i].Item.CategoryName, ItemGroupList).GetDataSet();
                    DataTable dt = ds.Tables[0];

                    //OrderDets[i].IsPromoPossibilityChecked = true;
                    OrderDets[i].PossiblePromoID = "";

                    if (dt.Rows.Count == 0)
                        continue;

                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        if (dt.Rows[j][0].ToString() != "0")
                        {
                            OrderDets[i].PossiblePromoID += dt.Rows[j][0].ToString() + ",";
                        }
                    }
                }
            }

            #endregion
            Logger.writeLog("Promo Log - 9. Apply Promo Nwgative qty");
            ApplyAnyXoffAllItemsNegativeQty();
            Logger.writeLog("Promo Log - 9. Finish Apply Promo Nwgative qty");

            //Apply line discounts
            Logger.writeLog("Promo Log - 9. Apply Promo Line DIscount");
            if (hasOldPromo)
            {
                
                DataSet ds;
                int max = myOrderLine.Count;
                int j = 0;
                int posid = PointOfSaleInfo.PointOfSaleID;
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Invoice.EnableOutletSales), false))
                {
                    if (pos.MembershipApplied())
                    {
                        if (pos.GetMemberInfo() != null && pos.GetMemberInfo().MembershipNo != "" && pos.GetMemberInfo().MembershipNo != "WALK-IN")
                        {
                            //validation if outlet order
                            PointOfSaleCollection posColl = new PointOfSaleCollection();
                            posColl.Where(PointOfSale.UserColumns.LinkedMembershipNo, pos.GetMemberInfo().MembershipNo);
                            posColl.Load();
                            if (posColl.Count > 0)
                                posid = posColl[0].PointOfSaleID;

                        }
                    }
                }

                Logger.writeLog("Promo Log - 9. Loop While Finish Apply Promo Line DIscount");
                while (j < max)
                {
                    //have an if to check if myOrderLine[j] is a valid Item....
                    if (IsValidPromoLineItem(myOrderLine[j]))
                    {
                        ds = SPs.FetchAllPossiblePromoForItem
                            (myOrderLine[j].ItemNo, myOrderLine[j].Item.CategoryName,
                            myOrderLine[j].Quantity.GetIntValue(), pos.MembershipApplied(),
                            posid).GetDataSet();

                        if (ds == null || ds.Tables[0].Rows.Count == 0) //no promo
                        {
                            //no promo needed to be applied
                        }
                        else if (ds.Tables[0].Rows.Count == 1) //1 promo
                        {
                            //just apply the promo                        
                            ApplyPromoToLineItem(myOrderLine[j],
                                new ViewPromoMasterDetail
                                    (ViewPromoMasterDetail.Columns.PromoCampaignHdrID, ((object)ds.Tables[0].Rows[0][0])));
                        }
                        else if (ds.Tables[0].Rows.Count > 1) //conflicting Promo
                        {
                            ViewPromoMasterDetailCollection viewPromoMasterDetails = new ViewPromoMasterDetailCollection();
                            //calculate the best price
                            for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                            {
                                viewPromoMasterDetails.Add(new ViewPromoMasterDetail
                                   (ViewPromoMasterDetail.Columns.PromoCampaignHdrID, (object)ds.Tables[0].Rows[k][0]));
                            }
                            int maxIndex = CalculatePromoValue(myOrderLine[j], viewPromoMasterDetails);
                            //Apply the max promo...
                            if (maxIndex != -1)
                                ApplyPromoToLineItem(myOrderLine[j], viewPromoMasterDetails[maxIndex]);
                        }
                    }
                    j++;
                }
                Logger.writeLog("Promo Log - 9. Finish Loop While Finish Apply Promo Line DIscount");
                
            }
            Logger.writeLog("Promo Log - 9. Finish Apply Promo Line DIscount");
            
        }

        public void ApplyAnyXoffAllItems()
        {
            OrderDetCollection OrderDets = pos.FetchUnsavedOrderDet();
            OrderDets.Sort(OrderDet.Columns.ItemNo, true);

            string status = "";
            if (OrderDets == null || OrderDets.Count == 0)
                return;

            //Set POS Id to use member pointofsale if it was outlet sales
            int posid = PointOfSaleInfo.PointOfSaleID;

            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Invoice.EnableOutletSales), false))
            {
                if (pos.MembershipApplied())
                {
                    if (pos.GetMemberInfo() != null && pos.GetMemberInfo().MembershipNo != "" && pos.GetMemberInfo().MembershipNo != "WALK-IN")
                    {
                        //validation if outlet order
                        PointOfSaleCollection posColl = new PointOfSaleCollection();
                        posColl.Where(PointOfSale.UserColumns.LinkedMembershipNo, pos.GetMemberInfo().MembershipNo);
                        posColl.Load();
                        if (posColl.Count > 0)
                            posid = posColl[0].PointOfSaleID;
                        
                    }
                }
            }


            string ItemNoList = "";
            
            //Get the list of valid promo line item
            //put them in the list of item no

            DataTable dtPromoHdr = new DataTable();
            dtPromoHdr.Columns.Add("PromoCampaignHdrID");
            //dtPromoHdr.Columns.Add("Priority", typeof(int)); 
            
            for (int i = 0; i < OrderDets.Count; i++)
            {
                //Fetch All Possible Promo for each items
                if (OrderDets[i].IsPromoPossibilityChecked)
                {
                    ItemNoList += "'" + OrderDets[i].ItemNo + "',";
                    if (OrderDets[i].PossiblePromoID != null && OrderDets[i].PossiblePromoID != "")
                    {
                        string[] listPromo = OrderDets[i].PossiblePromoID.Split(new string[] {","}, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string s in listPromo)
                        {
                            if (dtPromoHdr.Select("PromoCampaignHdrID = '" + s.Replace("'", "''") + "'").Length <= 0)
                            {
                                DataRow dr = dtPromoHdr.NewRow();
                                dr["PromoCampaignHdrID"] = s;
                                dtPromoHdr.Rows.Add(dr);
                            }
                        }
                    }
                    continue;
                }

                if (IsValidPromoLineItem(OrderDets[i]))
                {
                    ItemNoList += "'" + OrderDets[i].ItemNo + "',";
                    string ItemNoList1 = "'" + OrderDets[i].ItemNo + "'";

                    OrderDets[i].PossibleItemGroupID = "";
                    string sqlCheckItemGroup = "Select Distinct ItemGroupID from ItemGroupMap where deleted = 0 and itemno = '" + OrderDets[i].ItemNo + "'";
                    DataTable dtItemGroup = DataService.GetDataSet(new QueryCommand(sqlCheckItemGroup)).Tables[0];
                    for (int j = 0; j < dtItemGroup.Rows.Count; j++)
                    {
                        if (dtItemGroup.Rows[j][0].ToString() != "0")
                        {
                            OrderDets[i].PossibleItemGroupID += dtItemGroup.Rows[j][0].ToString() + ",";
                        }
                    }
                    OrderDets[i].PossibleItemGroupID = OrderDets[i].PossibleItemGroupID.TrimEnd(',');

                    //GetMember Group
                    int memberGroupID = 0;
                    if (pos.MembershipApplied())
                    {
                        memberGroupID = pos.GetMemberInfo().MembershipGroupId;
                    }

                    string ItemGroupList = "-1";
                    if (OrderDets[i].PossibleItemGroupID != "")
                        ItemGroupList = OrderDets[i].PossibleItemGroupID;
                    DataSet ds = SPs.FetchAllPossiblePromoAnyXofAllItemsMemberGroup(ItemNoList1, pos.MembershipApplied() && pos.GetMemberInfo().MembershipNo != "WALK-IN", posid, memberGroupID, OrderDets[i].Item.CategoryName, ItemGroupList).GetDataSet();
                    DataTable dt = ds.Tables[0];

                    OrderDets[i].IsPromoPossibilityChecked = true;
                    OrderDets[i].PossiblePromoID = "";

                    if (dt.Rows.Count == 0)
                        continue;

                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        if (dt.Rows[j][0].ToString() != "0")
                        {
                            OrderDets[i].PossiblePromoID += dt.Rows[j][0].ToString() + ",";
                            if (dtPromoHdr.Select("PromoCampaignHdrID = '" + dt.Rows[j][0].ToString() + "'").Length <= 0)
                            {
                                DataRow dr = dtPromoHdr.NewRow();
                                dr["PromoCampaignHdrID"] = dt.Rows[j][0].ToString();
                                dtPromoHdr.Rows.Add(dr);
                            }
                        }
                    }

                    

                }
                
            }
            
            ItemNoList = ItemNoList.TrimEnd(','); //ItemNoList.LastIndexOf(',')-1);

            Dictionary<string, decimal> OrderDetList = new Dictionary<string, decimal>();
            
            //sort dtpromohdr
            DataTable dtPromoHdrTemp = new DataTable();
            dtPromoHdrTemp.Columns.Add("PromoCampaignHdrID");
            dtPromoHdrTemp.Columns.Add("Priority", typeof(int)); 
            for (int i = 0; i < dtPromoHdr.Rows.Count; i++)
            {
                PromoCampaignHdr p = new PromoCampaignHdr(Int32.Parse(dtPromoHdr.Rows[i][0].ToString() ?? "0"));
                if (p != null && p.PromoCampaignHdrID != 0)
                {
                    if (dtPromoHdrTemp.Rows.Count == 0)
                    {
                        DataRow dr = dtPromoHdrTemp.NewRow();
                        dr["PromoCampaignHdrID"] = p.PromoCampaignHdrID.ToString();
                        dr["Priority"] = p.Priority;
                        dtPromoHdrTemp.Rows.Add(dr);
                    }
                    else
                    {
                        int loc = 0;
                        bool flag = false;
                        for (int j = 0; j < (dtPromoHdrTemp.Rows.Count); j++)
                        {
                            if (flag)
                                continue;
                            int prior = 0;
                            if (int.TryParse(dtPromoHdrTemp.Rows[j]["Priority"].ToString(), out prior))
                            {
                                if (prior > p.Priority.GetValueOrDefault(1))
                                {
                                    loc = j;
                                    flag = true;
                                    continue;
                                }
                            }
                        }
                        
                        DataRow dr = dtPromoHdrTemp.NewRow();
                        dr["PromoCampaignHdrID"] = p.PromoCampaignHdrID.ToString();
                        dr["Priority"] = p.Priority;
                        if (flag)
                        {
                            dtPromoHdrTemp.Rows.InsertAt(dr, loc);
                        }
                        else
                        {
                            dtPromoHdrTemp.Rows.Add(dr);
                        }
                    }
                }
            }

                for (int i = 0; i < dtPromoHdrTemp.Rows.Count; i++)
                {
                    //Logger.writeLog("Promo Log - Check Promo Hdr " + i.ToString() + " of " + dtPromoHdr.Rows.Count.ToString());
                    int PromoCampaignHdrId = Int32.Parse(dtPromoHdrTemp.Rows[i][0].ToString() ?? "0");
                    PromoCampaignDetCollection promodetail = new PromoCampaignDetCollection();
                    promodetail.Where(PromoCampaignDet.Columns.PromoCampaignHdrID, Comparison.Equals, PromoCampaignHdrId);
                    promodetail.Where(PromoCampaignDet.Columns.Deleted, Comparison.Equals, false);
                    promodetail.OrderByAsc(PromoCampaignDet.Columns.ItemNo);
                    promodetail.Load();

                    bool isPromo = true;
                    while (isPromo)
                    {
                        if (promodetail.Count > 0)
                        {
                            //checking for single promo with minqty

                            ArrayList OrderDetAny = new ArrayList();
                            ArrayList OrderDetQty = new ArrayList();
                            ArrayList ListPromoCampaignDetId = new ArrayList();
                            ArrayList ListPromoCampaignDetIdAny = new ArrayList();
                            int Multiplier = 1000;
                            ArrayList ListCountMatched = new ArrayList();

                            for (int j = 0; j < promodetail.Count; j++)
                            {
                                int CountMatchedItem = 0;
                                if (promodetail[j].AnyQty > 0)
                                {
                                    int k = 0;
                                    int l = 0;
                                    decimal SumQtyAny = 0;

                                    //ViewPromoMasterDetailAnyCollection promoCol = new ViewPromoMasterDetailAnyCollection();
                                    //promoCol.Where(ViewPromoMasterDetailAny.Columns.PromoCampaignDetID, Comparison.Equals, promodetail[j].PromoCampaignDetID);
                                    //promoCol.OrderByAsc(ViewPromoMasterDetailAny.Columns.ItemNo);
                                    //promoCol.Load();


                                    //DataTable tempDT = GetViewPromoMasterDetailAny(promodetail[j].PromoCampaignDetID.ToString()).Select("PromoCampaignDetID='" + promodetail[j].PromoCampaignDetID + "'").CopyToDataTable();


                                    //for (k = 0; k < tempDT.Rows.Count; k++)
                                    //{
                                    for (l = 0; l < OrderDets.Count; l++)
                                    {
                                        if (promodetail[j].CategoryName != null && promodetail[j].CategoryName != "")
                                        {
                                            if (promodetail[j].CategoryName == OrderDets[l].Item.CategoryName
                                            & !OrderDets[l].IsVoided
                                            & !OrderDets[l].IsPromo
                                            & !OrderDets[l].IsFreeOfCharge
                                            & !OrderDets[l].IsSpecial
                                            & (OrderDets[l].PriceMode == "NORMAL" || OrderDets[l].PriceMode == null || OrderDets[l].PriceMode == "")
                                            )
                                            {
                                                decimal LineItemQty = RoundUpQty(OrderDets[l].Quantity.GetValueOrDefault(0));

                                                CountMatchedItem++;
                                                SumQtyAny += LineItemQty;
                                                ListPromoCampaignDetIdAny.Add(promodetail[j].PromoCampaignDetID);
                                                OrderDetAny.Add(l); //store the order line as the important one
                                            }
                                        }
                                        else if (promodetail[j].ItemGroupID.GetValueOrDefault(0) != 0)
                                        {
                                            if (OrderDets[l].PossibleItemGroupID != null && OrderDets[l].PossibleItemGroupID != "")
                                            {
                                                String[] PossibleItemGroupID = OrderDets[l].PossibleItemGroupID.Split(new Char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                                                foreach (string s in PossibleItemGroupID)
                                                {
                                                    int ItemGroupID = 0;
                                                    if (!int.TryParse(s, out ItemGroupID))
                                                        continue;

                                                    if (promodetail[j].ItemGroupID == ItemGroupID
                                                    & !OrderDets[l].IsVoided
                                                    & !OrderDets[l].IsPromo
                                                    & !OrderDets[l].IsFreeOfCharge
                                                    & !OrderDets[l].IsSpecial
                                                    )
                                                    {
                                                        decimal LineItemQty = RoundUpQty(OrderDets[l].Quantity.GetValueOrDefault(0));

                                                        CountMatchedItem++;
                                                        SumQtyAny += LineItemQty;
                                                        ListPromoCampaignDetIdAny.Add(promodetail[j].PromoCampaignDetID);
                                                        OrderDetAny.Add(l); //store the order line as the important one
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    //}

                                    //set multiplier
                                    int multiplierany = Convert.ToInt32(SumQtyAny) / (int)promodetail[j].AnyQty;
                                    if (multiplierany < Multiplier | multiplierany == 0)
                                        Multiplier = multiplierany;


                                    if (OrderDetAny.Count == 0 || SumQtyAny < promodetail[j].AnyQty)
                                    {
                                        isPromo = false;
                                    }

                                    ListCountMatched.Add(CountMatchedItem);
                                }
                                else if (promodetail[j].UnitQty > 0)
                                {
                                    int k = 0;
                                    int l = 0;
                                    decimal SumQtyPromo = 0;
                                    decimal SumQtyOrder = 0;

                                    int minUnitPromoQty = 0;

                                    //ViewPromoMasterDetailAnyCollection promoCol = new ViewPromoMasterDetailAnyCollection();
                                    //promoCol.Where(ViewPromoMasterDetailAny.Columns.PromoCampaignDetID, Comparison.Equals, promodetail[j].PromoCampaignDetID);
                                    //promoCol.OrderByAsc(ViewPromoMasterDetailAny.Columns.ItemNo);
                                    //promoCol.Load();

                                    //DataTable tempDT = GetViewPromoMasterDetailUnitQty(promodetail[j].PromoCampaignDetID.ToString()).Select("PromoCampaignDetID='" + promodetail[j].PromoCampaignDetID + "'", "ItemNo").CopyToDataTable();
                                    DataTable tempDT = GetViewPromoMasterDetailUnitQty(promodetail[j].PromoCampaignDetID.ToString()).Select("PromoCampaignDetID='" + promodetail[j].PromoCampaignDetID + "'", "ItemNo").CopyToDataTable();

                                    for (k = 0; k < tempDT.Rows.Count; k++)
                                    {
                                        SumQtyPromo += tempDT.Rows[k]["UnitQty"].ToString().GetIntValue();

                                        for (l = 0; l < OrderDets.Count; l++)
                                        {
                                            if (tempDT.Rows[k]["ItemNo"].ToString() == OrderDets[l].ItemNo
                                            & tempDT.Rows[k]["UnitQty"].ToString().GetIntValue() <= OrderDets[l].Quantity
                                            & !OrderDets[l].IsVoided
                                            & !OrderDets[l].IsPromo
                                            & !OrderDets[l].IsFreeOfCharge
                                            & !OrderDets[l].IsSpecial
                                            & (OrderDets[l].PriceMode == "NORMAL" || OrderDets[l].PriceMode == null || OrderDets[l].PriceMode == "")
                                            )
                                            {
                                                decimal LineItemQty = OrderDets[l].Quantity.GetValueOrDefault(0);
                                                SumQtyOrder += OrderDets[l].Quantity.GetValueOrDefault(0);
                                                OrderDetQty.Add(l); //store the order line as the important one
                                                ListPromoCampaignDetId.Add(promodetail[j].PromoCampaignDetID);

                                                //set multiplier
                                                int UnitPromoQty = Convert.ToInt32(OrderDets[l].Quantity.GetValueOrDefault(0)) / tempDT.Rows[k]["UnitQty"].ToString().GetIntValue();
                                                if (UnitPromoQty < minUnitPromoQty | minUnitPromoQty == 0)
                                                    minUnitPromoQty = UnitPromoQty;
                                            }
                                        }
                                    }

                                    //set multiplier
                                    if (minUnitPromoQty < Multiplier | minUnitPromoQty == 0)
                                        Multiplier = minUnitPromoQty;

                                    if (OrderDetQty.Count == 0 || SumQtyOrder < SumQtyPromo)
                                    {
                                        isPromo = false;
                                    }

                                    ListCountMatched.Add(CountMatchedItem);
                                }
                            }

                            if (isPromo)
                            {

                                for (int j = 0; j < promodetail.Count; j++)
                                {
                                    if (promodetail[j].AnyQty > 0)
                                    {
                                        ApplyAnyItems(promodetail[j], Multiplier, OrderDetAny, ListPromoCampaignDetIdAny, OrderDets);

                                          
                                    }
                                    else if (promodetail[j].UnitQty > 0)
                                    {
                                        ApplyBundleItems(promodetail[j], Multiplier, OrderDetQty, ListPromoCampaignDetId, OrderDets);
                                    }
                                }

                                //Checking valid promo again
                                if (OrderDetAny.Count == 0 && OrderDetQty.Count == 0)
                                {
                                    isPromo = false;
                                }
                            }
                        }
                        else
                        {
                            isPromo = false;
                        }
                    }

                }

            /*DataSet ds;
            if (ItemNoList != "")
            {
                DataTable orderdt = null;
                //Logger.writeLog("List of ItemNo : " +ItemNoList);
                //Logger.writeLog("Point of Sale : " + PointOfSaleInfo.PointOfSaleID);

                //get all promocampaignheader 
                ds = SPs.FetchAllPossiblePromoAnyXofAllItems(ItemNoList, pos.MembershipApplied() && pos.GetMemberInfo().MembershipNo != "WALK-IN", posid).GetDataSet();
                DataTable dt = ds.Tables[0];
                
                if (dt.Rows.Count == 0)
                    return;

                //looping for every promo
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int PromoCampaignHdrId = Int32.Parse(dt.Rows[i][0].ToString() ?? "0");
                    PromoCampaignDetCollection promodetail = new PromoCampaignDetCollection();
                    promodetail.Where(PromoCampaignDet.Columns.PromoCampaignHdrID, Comparison.Equals, PromoCampaignHdrId);
                    promodetail.Where(PromoCampaignDet.Columns.Deleted, Comparison.Equals, false);
                    promodetail.OrderByAsc(PromoCampaignDet.Columns.ItemNo);
                    promodetail.Load();

                    bool isPromo = true;
                    while (isPromo)
                    {
                        if (promodetail.Count > 0)
                        {
                            //checking for single promo with minqty

                            ArrayList OrderDetAny = new ArrayList();
                            ArrayList OrderDetQty = new ArrayList();
                            ArrayList ListPromoCampaignDetId = new ArrayList();
                            ArrayList ListPromoCampaignDetIdAny = new ArrayList();
                            int Multiplier = 1000;
                            ArrayList ListCountMatched = new ArrayList();

                            for (int j = 0; j < promodetail.Count; j++)
                            {
                                int CountMatchedItem = 0;
                                if (promodetail[j].AnyQty > 0)
                                {
                                    int k = 0;
                                    int l = 0;
                                    decimal SumQtyAny = 0;

                                    //ViewPromoMasterDetailAnyCollection promoCol = new ViewPromoMasterDetailAnyCollection();
                                    //promoCol.Where(ViewPromoMasterDetailAny.Columns.PromoCampaignDetID, Comparison.Equals, promodetail[j].PromoCampaignDetID);
                                    //promoCol.OrderByAsc(ViewPromoMasterDetailAny.Columns.ItemNo);
                                    //promoCol.Load();
                                    
                                    DataTable tempDT = GetViewPromoMasterDetailAny().Select("PromoCampaignDetID='" + promodetail[j].PromoCampaignDetID + "'", "ItemNo").CopyToDataTable();

                                    for (k = 0; k < tempDT.Rows.Count; k++)
                                    {
                                        for (l = 0; l < OrderDets.Count; l++)
                                        {
                                            if (tempDT.Rows[k]["ItemNo"].ToString() == OrderDets[l].ItemNo
                                            & !OrderDets[l].IsVoided
                                            & !OrderDets[l].IsPromo
                                            & !OrderDets[l].IsFreeOfCharge
                                            & !OrderDets[l].IsSpecial
                                            )
                                            {
                                                decimal LineItemQty = OrderDets[l].Quantity.GetValueOrDefault(0);

                                                CountMatchedItem++;
                                                SumQtyAny += OrderDets[l].Quantity.GetValueOrDefault(0);
                                                ListPromoCampaignDetIdAny.Add(promodetail[j].PromoCampaignDetID);
                                                OrderDetAny.Add(l); //store the order line as the important one
                                            }
                                        }
                                    }

                                    //set multiplier
                                    int multiplierany = Convert.ToInt32(SumQtyAny) / (int)promodetail[j].AnyQty;
                                    if (multiplierany < Multiplier | multiplierany == 0)
                                        Multiplier = multiplierany;


                                    if (OrderDetAny.Count == 0 || SumQtyAny < promodetail[j].AnyQty)
                                    {
                                        isPromo = false;
                                    }

                                    ListCountMatched.Add(CountMatchedItem);
                                }
                                else if (promodetail[j].UnitQty > 0)
                                {
                                    int k = 0;
                                    int l = 0;
                                    decimal SumQtyPromo = 0;
                                    decimal SumQtyOrder = 0;

                                    int minUnitPromoQty = 0;

                                    //ViewPromoMasterDetailAnyCollection promoCol = new ViewPromoMasterDetailAnyCollection();
                                    //promoCol.Where(ViewPromoMasterDetailAny.Columns.PromoCampaignDetID, Comparison.Equals, promodetail[j].PromoCampaignDetID);
                                    //promoCol.OrderByAsc(ViewPromoMasterDetailAny.Columns.ItemNo);
                                    //promoCol.Load();

                                    DataTable tempDT = GetViewPromoMasterDetailAny().Select("PromoCampaignDetID='" + promodetail[j].PromoCampaignDetID + "'", "ItemNo").CopyToDataTable();

                                    for (k = 0; k < tempDT.Rows.Count; k++)
                                    {
                                        SumQtyPromo += tempDT.Rows[k]["UnitQty"].ToString().GetIntValue();

                                        for (l = 0; l < OrderDets.Count; l++)
                                        {
                                            if (tempDT.Rows[k]["ItemNo"].ToString() == OrderDets[l].ItemNo
                                            & tempDT.Rows[k]["UnitQty"].ToString().GetIntValue() <= OrderDets[l].Quantity
                                            & !OrderDets[l].IsVoided
                                            & !OrderDets[l].IsPromo
                                            & !OrderDets[l].IsFreeOfCharge
                                                & !OrderDets[l].IsSpecial
                                            )
                                            {
                                                decimal LineItemQty = OrderDets[l].Quantity.GetValueOrDefault(0);
                                                SumQtyOrder += OrderDets[l].Quantity.GetValueOrDefault(0);
                                                OrderDetQty.Add(l); //store the order line as the important one
                                                ListPromoCampaignDetId.Add(promodetail[j].PromoCampaignDetID);

                                                //set multiplier
                                                int UnitPromoQty = Convert.ToInt32(OrderDets[l].Quantity.GetValueOrDefault(0)) / tempDT.Rows[k]["UnitQty"].ToString().GetIntValue();
                                                if (UnitPromoQty < minUnitPromoQty | minUnitPromoQty == 0)
                                                    minUnitPromoQty = UnitPromoQty;
                                            }
                                        }
                                    }

                                    //set multiplier
                                    if (minUnitPromoQty < Multiplier | minUnitPromoQty == 0)
                                        Multiplier = minUnitPromoQty;

                                    if (OrderDetQty.Count == 0 || SumQtyOrder < SumQtyPromo)
                                    {
                                        isPromo = false;
                                    }

                                    ListCountMatched.Add(CountMatchedItem);
                                }
                            }

                            if (isPromo)
                            {
                                for (int j = 0; j < promodetail.Count; j++)
                                {
                                    if (promodetail[j].AnyQty > 0)
                                    {
                                        ApplyAnyItems(promodetail[j], Multiplier, OrderDetAny, ListPromoCampaignDetIdAny, OrderDets);
                                    }
                                    else if (promodetail[j].UnitQty > 0)
                                    {
                                        ApplyBundleItems(promodetail[j], Multiplier, OrderDetQty, ListPromoCampaignDetId, OrderDets);
                                    }
                                }

                                //Checking valid promo again
                                if (OrderDetAny.Count == 0 && OrderDetQty.Count == 0)
                                {
                                    isPromo = false;
                                }
                            }
                        }
                        else
                        {
                            isPromo = false;
                        }
                    }
                    
                }
            }*/
        }

        public void ApplyAnyXoffAllItemsNegativeQty()
        {
            OrderDetCollection OrderDets = pos.FetchUnsavedOrderDet();
            OrderDets.Sort(OrderDet.Columns.ItemNo, true);
            bool findNegativePromo = false;

            string status = "";
            if (OrderDets == null || OrderDets.Count == 0)
                return;

            for (int j = 0; j < OrderDets.Count; j++)
            {
                if (OrderDets[j].Quantity < 0 &
                    !OrderDets[j].IsVoided &
                    !OrderDets[j].IsPromo &
                    !OrderDets[j].IsFreeOfCharge &
                    !string.IsNullOrEmpty(OrderDets[j].PossiblePromoID))
                {
                    findNegativePromo = true;
                    break;
                }
            }

            if (!findNegativePromo)
                return;

            //Set POS Id to use member pointofsale if it was outlet sales
            int posid = PointOfSaleInfo.PointOfSaleID;

            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Invoice.EnableOutletSales), false))
            {
                if (pos.MembershipApplied())
                {
                    if (pos.GetMemberInfo() != null && pos.GetMemberInfo().MembershipNo != "" && pos.GetMemberInfo().MembershipNo != "WALK-IN")
                    {
                        //validation if outlet order
                        PointOfSaleCollection posColl = new PointOfSaleCollection();
                        posColl.Where(PointOfSale.UserColumns.LinkedMembershipNo, pos.GetMemberInfo().MembershipNo);
                        posColl.Load();
                        if (posColl.Count > 0)
                            posid = posColl[0].PointOfSaleID;

                    }
                }
            }


            string ItemNoList = "";

            //Get the list of valid promo line item
            //put them in the list of item no

            DataTable dtPromoHdr = new DataTable();
            dtPromoHdr.Columns.Add("PromoCampaignHdrID");
            //dtPromoHdr.Columns.Add("Priority", typeof(int)); 

            for (int i = 0; i < OrderDets.Count; i++)
            {
                //Fetch All Possible Promo for each items
                if (OrderDets[i].IsPromoPossibilityChecked)
                {
                    ItemNoList += "'" + OrderDets[i].ItemNo + "',";
                    if (OrderDets[i].PossiblePromoID != null && OrderDets[i].PossiblePromoID != "")
                    {
                        string[] listPromo = OrderDets[i].PossiblePromoID.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string s in listPromo)
                        {
                            if (dtPromoHdr.Select("PromoCampaignHdrID = '" + s.Replace("'", "''") + "'").Length <= 0)
                            {
                                DataRow dr = dtPromoHdr.NewRow();
                                dr["PromoCampaignHdrID"] = s;
                                dtPromoHdr.Rows.Add(dr);
                            }
                        }
                    }
                    continue;
                }

                if (IsValidPromoLineItem(OrderDets[i]))
                {
                    ItemNoList += "'" + OrderDets[i].ItemNo + "',";
                    string ItemNoList1 = "'" + OrderDets[i].ItemNo + "'";

                    OrderDets[i].PossibleItemGroupID = "";
                    string sqlCheckItemGroup = "Select Distinct ItemGroupID from ItemGroupMap where deleted = 0 and itemno = '" + OrderDets[i].ItemNo + "'";
                    DataTable dtItemGroup = DataService.GetDataSet(new QueryCommand(sqlCheckItemGroup)).Tables[0];
                    for (int j = 0; j < dtItemGroup.Rows.Count; j++)
                    {
                        if (dtItemGroup.Rows[j][0].ToString() != "0")
                        {
                            OrderDets[i].PossibleItemGroupID += dtItemGroup.Rows[j][0].ToString() + ",";
                        }
                    }
                    OrderDets[i].PossibleItemGroupID = OrderDets[i].PossibleItemGroupID.TrimEnd(',');

                    //GetMember Group
                    int memberGroupID = 0;
                    if (pos.MembershipApplied())
                    {
                        memberGroupID = pos.GetMemberInfo().MembershipGroupId;
                    }

                    string ItemGroupList = "-1";
                    if (OrderDets[i].PossibleItemGroupID != "")
                        ItemGroupList = OrderDets[i].PossibleItemGroupID;
                    DataSet ds = SPs.FetchAllPossiblePromoAnyXofAllItemsMemberGroup(ItemNoList1, pos.MembershipApplied() && pos.GetMemberInfo().MembershipNo != "WALK-IN", posid, memberGroupID, OrderDets[i].Item.CategoryName, ItemGroupList).GetDataSet();
                    DataTable dt = ds.Tables[0];

                    OrderDets[i].IsPromoPossibilityChecked = true;
                    OrderDets[i].PossiblePromoID = "";

                    if (dt.Rows.Count == 0)
                        continue;

                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        if (dt.Rows[j][0].ToString() != "0")
                        {
                            OrderDets[i].PossiblePromoID += dt.Rows[j][0].ToString() + ",";
                            if (dtPromoHdr.Select("PromoCampaignHdrID = '" + dt.Rows[j][0].ToString() + "'").Length <= 0)
                            {
                                DataRow dr = dtPromoHdr.NewRow();
                                dr["PromoCampaignHdrID"] = dt.Rows[j][0].ToString();
                                dtPromoHdr.Rows.Add(dr);
                            }
                        }
                    }
                }
            }

            ItemNoList = ItemNoList.TrimEnd(','); //ItemNoList.LastIndexOf(',')-1);

            Dictionary<string, decimal> OrderDetList = new Dictionary<string, decimal>();

            //sort dtpromohdr
            DataTable dtPromoHdrTemp = new DataTable();
            dtPromoHdrTemp.Columns.Add("PromoCampaignHdrID");
            dtPromoHdrTemp.Columns.Add("Priority", typeof(int));
            for (int i = 0; i < dtPromoHdr.Rows.Count; i++)
            {
                PromoCampaignHdr p = new PromoCampaignHdr(Int32.Parse(dtPromoHdr.Rows[i][0].ToString() ?? "0"));
                if (p != null && p.PromoCampaignHdrID != 0)
                {
                    if (dtPromoHdrTemp.Rows.Count == 0)
                    {
                        DataRow dr = dtPromoHdrTemp.NewRow();
                        dr["PromoCampaignHdrID"] = p.PromoCampaignHdrID.ToString();
                        dr["Priority"] = p.Priority;
                        dtPromoHdrTemp.Rows.Add(dr);
                    }
                    else
                    {
                        int loc = 0;
                        bool flag = false;
                        for (int j = 0; j < (dtPromoHdrTemp.Rows.Count); j++)
                        {
                            if (flag)
                                continue;
                            int prior = 0;
                            if (int.TryParse(dtPromoHdrTemp.Rows[j]["Priority"].ToString(), out prior))
                            {
                                if (prior > p.Priority.GetValueOrDefault(1))
                                {
                                    loc = j;
                                    flag = true;
                                    continue;
                                }
                            }
                        }

                        DataRow dr = dtPromoHdrTemp.NewRow();
                        dr["PromoCampaignHdrID"] = p.PromoCampaignHdrID.ToString();
                        dr["Priority"] = p.Priority;
                        if (flag)
                        {
                            dtPromoHdrTemp.Rows.InsertAt(dr, loc);
                        }
                        else
                        {
                            dtPromoHdrTemp.Rows.Add(dr);
                        }
                    }
                }
            }

            for (int i = 0; i < dtPromoHdrTemp.Rows.Count; i++)
            {
                //Logger.writeLog("Promo Log - Check Promo Hdr " + i.ToString() + " of " + dtPromoHdr.Rows.Count.ToString());
                int PromoCampaignHdrId = Int32.Parse(dtPromoHdrTemp.Rows[i][0].ToString() ?? "0");
                PromoCampaignDetCollection promodetail = new PromoCampaignDetCollection();
                promodetail.Where(PromoCampaignDet.Columns.PromoCampaignHdrID, Comparison.Equals, PromoCampaignHdrId);
                promodetail.Where(PromoCampaignDet.Columns.Deleted, Comparison.Equals, false);
                promodetail.OrderByAsc(PromoCampaignDet.Columns.ItemNo);
                promodetail.Load();

                bool isPromo = true;
                while (isPromo)
                {
                    if (promodetail.Count > 0)
                    {
                        //checking for single promo with minqty

                        ArrayList OrderDetAny = new ArrayList();
                        ArrayList OrderDetQty = new ArrayList();
                        ArrayList ListPromoCampaignDetId = new ArrayList();
                        ArrayList ListPromoCampaignDetIdAny = new ArrayList();
                        int Multiplier = 1000;
                        ArrayList ListCountMatched = new ArrayList();

                        for (int j = 0; j < promodetail.Count; j++)
                        {
                            int CountMatchedItem = 0;
                            if (promodetail[j].AnyQty > 0)
                            {
                                int k = 0;
                                int l = 0;
                                decimal SumQtyAny = 0;

                                //for (k = 0; k < tempDT.Rows.Count; k++)
                                //{
                                for (l = 0; l < OrderDets.Count; l++)
                                {
                                    if (promodetail[j].CategoryName != null && promodetail[j].CategoryName != "")
                                    {
                                        if (promodetail[j].CategoryName == OrderDets[l].Item.CategoryName
                                        & !OrderDets[l].IsVoided
                                        & !OrderDets[l].IsPromo
                                        & !OrderDets[l].IsFreeOfCharge
                                        & !OrderDets[l].IsSpecial
                                        & (OrderDets[l].PriceMode == "NORMAL" || OrderDets[l].PriceMode == null || OrderDets[l].PriceMode == "")
                                        )
                                        {
                                            decimal LineItemQty = RoundUpQty(OrderDets[l].Quantity.GetValueOrDefault(0));

                                            CountMatchedItem++;
                                            SumQtyAny += LineItemQty;
                                            ListPromoCampaignDetIdAny.Add(promodetail[j].PromoCampaignDetID);
                                            OrderDetAny.Add(l); //store the order line as the important one
                                        }
                                    }
                                    else if (promodetail[j].ItemGroupID.GetValueOrDefault(0) != 0)
                                    {
                                        if (OrderDets[l].PossibleItemGroupID != null && OrderDets[l].PossibleItemGroupID != "")
                                        {
                                            String[] PossibleItemGroupID = OrderDets[l].PossibleItemGroupID.Split(new Char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                                            foreach (string s in PossibleItemGroupID)
                                            {
                                                int ItemGroupID = 0;
                                                if (!int.TryParse(s, out ItemGroupID))
                                                    continue;

                                                if (promodetail[j].ItemGroupID == ItemGroupID
                                                & !OrderDets[l].IsVoided
                                                & !OrderDets[l].IsPromo
                                                & !OrderDets[l].IsFreeOfCharge
                                                & !OrderDets[l].IsSpecial
                                                & (OrderDets[l].PriceMode == "NORMAL" || OrderDets[l].PriceMode == null || OrderDets[l].PriceMode == "")
                                                )
                                                {
                                                    decimal LineItemQty = RoundUpQty(OrderDets[l].Quantity.GetValueOrDefault(0));

                                                    CountMatchedItem++;
                                                    SumQtyAny += LineItemQty;
                                                    ListPromoCampaignDetIdAny.Add(promodetail[j].PromoCampaignDetID);
                                                    OrderDetAny.Add(l); //store the order line as the important one
                                                }
                                            }
                                        }
                                    }
                                }
                                //}

                                //set multiplier
                                int multiplierany = Convert.ToInt32(SumQtyAny) / (int)promodetail[j].AnyQty;
                                if (multiplierany < Multiplier | multiplierany == 0)
                                    Multiplier = multiplierany;


                                if (OrderDetAny.Count == 0 || Math.Abs(SumQtyAny) < promodetail[j].AnyQty.GetValueOrDefault(0))
                                {
                                    isPromo = false;
                                }

                                ListCountMatched.Add(CountMatchedItem);
                            }
                            else if (promodetail[j].UnitQty > 0)
                            {
                                int k = 0;
                                int l = 0;
                                decimal SumQtyPromo = 0;
                                decimal SumQtyOrder = 0;

                                int minUnitPromoQty = 0;

                                DataTable tempDT = GetViewPromoMasterDetailUnitQty(promodetail[j].PromoCampaignDetID.ToString()).Select("PromoCampaignDetID='" + promodetail[j].PromoCampaignDetID + "'", "ItemNo").CopyToDataTable();

                                for (k = 0; k < tempDT.Rows.Count; k++)
                                {
                                    SumQtyPromo += tempDT.Rows[k]["UnitQty"].ToString().GetIntValue();

                                    for (l = 0; l < OrderDets.Count; l++)
                                    {
                                        if (tempDT.Rows[k]["ItemNo"].ToString() == OrderDets[l].ItemNo
                                        & tempDT.Rows[k]["UnitQty"].ToString().GetIntValue() <= Math.Abs(OrderDets[l].Quantity.GetValueOrDefault(0))
                                        & !OrderDets[l].IsVoided
                                        & !OrderDets[l].IsPromo
                                        & !OrderDets[l].IsFreeOfCharge
                                        & !OrderDets[l].IsSpecial
                                        & (OrderDets[l].PriceMode == "NORMAL" || OrderDets[l].PriceMode == null || OrderDets[l].PriceMode == "")
                                        )
                                        {
                                            decimal LineItemQty = OrderDets[l].Quantity.GetValueOrDefault(0);
                                            SumQtyOrder += OrderDets[l].Quantity.GetValueOrDefault(0);
                                            OrderDetQty.Add(l); //store the order line as the important one
                                            ListPromoCampaignDetId.Add(promodetail[j].PromoCampaignDetID);

                                            //set multiplier
                                            int UnitPromoQty = Convert.ToInt32(OrderDets[l].Quantity.GetValueOrDefault(0)) / tempDT.Rows[k]["UnitQty"].ToString().GetIntValue();
                                            if (UnitPromoQty < minUnitPromoQty | minUnitPromoQty == 0)
                                                minUnitPromoQty = UnitPromoQty;
                                        }
                                    }
                                }

                                //set multiplier
                                if (minUnitPromoQty < Multiplier | minUnitPromoQty == 0)
                                    Multiplier = minUnitPromoQty;

                                if (OrderDetQty.Count == 0 || Math.Abs(SumQtyOrder) < SumQtyPromo)
                                {
                                    isPromo = false;
                                }

                                ListCountMatched.Add(CountMatchedItem);
                            }
                        }

                        if (isPromo)
                        {

                            for (int j = 0; j < promodetail.Count; j++)
                            {
                                if (promodetail[j].AnyQty > 0)
                                {
                                    ApplyAnyItemsNegativeQty(promodetail[j], Multiplier, OrderDetAny, ListPromoCampaignDetIdAny, OrderDets);


                                }
                                else if (promodetail[j].UnitQty > 0)
                                {
                                    ApplyBundleItemsNegativeQty(promodetail[j], Multiplier, OrderDetQty, ListPromoCampaignDetId, OrderDets);
                                }
                            }

                            //Checking valid promo again
                            if (OrderDetAny.Count == 0 && OrderDetQty.Count == 0)
                            {
                                isPromo = false;
                            }
                        }
                    }
                    else
                    {
                        isPromo = false;
                    }
                }

            }
        }

        private decimal RoundUpQty(decimal initialQty)
        {
            string status;

            decimal TotalAmount = initialQty;
            
            decimal temp = TotalAmount % 1; //return the 0.xxxx of a component
            
            if (temp > 0)
            {

                TotalAmount = TotalAmount - temp + 1; //Direct Round up to nearest Integer                              
            }
            
            return TotalAmount;
        }

        public void ApplyAnyItems(PromoCampaignDet promoDetail, int Multiplier, ArrayList orderLine, ArrayList ListPromoCampaignDetId, OrderDetCollection OrderDets)
        {
            ArrayList MultipleOrderLineID;

            if (orderLine.Count > 0)
            {
                int index = 0;

                double ItemGroupPromoAmount = 0;
                double PromoDiscountPercentage = 0;
                double PromoPrice = 0;
                double PromoDiscountAmount = 0;
                double TotalDiscount = 0;
                decimal anyQty = 0;
                string status = "";

                anyQty = Multiplier * (int)promoDetail.AnyQty;
                PromoPrice = (double)promoDetail.PromoPrice;
                PromoDiscountPercentage = (double)promoDetail.DiscPercent;
                PromoDiscountAmount = (double)promoDetail.DiscAmount;
                MultipleOrderLineID = new ArrayList();
                ItemGroupPromoAmount = Multiplier * PromoPrice;

                double tmp = Multiplier * PromoPrice;
                double tmpPromoAmount = Multiplier * PromoDiscountAmount;
                double TotalOrder = 0;

                while (anyQty > 0)
                {
                    if ((int)ListPromoCampaignDetId[index] == promoDetail.PromoCampaignDetID)
                    {
                        if (IsValidPromoLineItem(OrderDets[(int)orderLine[index]]))
                        {
                            string LineID = OrderDets[(int)orderLine[index]].OrderDetID;
                            string ItemNo = OrderDets[(int)orderLine[index]].ItemNo;
                            MultipleOrderLineID.Add(LineID);

                            decimal promoUntQty = 0;
                            decimal LeftOverQty = 0;

                            if (RoundUpQty(OrderDets[(int)orderLine[index]].Quantity.GetValueOrDefault(0)) >= anyQty)
                            {
                                promoUntQty = anyQty;
                                LeftOverQty = RoundUpQty(OrderDets[(int)orderLine[index]].Quantity.GetValueOrDefault(0)) - promoUntQty;
                            }
                            else
                            {
                                promoUntQty = OrderDets[(int)orderLine[index]].Quantity.GetValueOrDefault(0);
                            }
                            //Logger.writeLog("Start Change Qty");
                            if (LeftOverQty > 0)
                            {
                                pos.ChangeOrderLineQuantity(LineID, promoUntQty, false, out status);
                                pos.SetOrderLineAsPromo(LineID, true, promoDetail.PromoCampaignHdrID, promoDetail.PromoCampaignDetID, out status);
                            }
                            else
                            {
                                pos.SetOrderLineAsPromo(LineID, true, promoDetail.PromoCampaignHdrID, promoDetail.PromoCampaignDetID, out status);
                            }
                            decimal LineQty = promoUntQty;
                            double LinePrice = (double)OrderDets[(int)orderLine[index]].UnitPrice;
                            double LineDiscountedPriceAmount = 0;

                            TotalOrder += LinePrice * Convert.ToDouble(LineQty);

                            //cek if using price or discount
                            if (PromoPrice != 0)
                            {
                                //using price
                                if (LinePrice > 0)
                                {
                                    //Calculate line discount accordingly:      
                                    LineDiscountedPriceAmount = PromoPrice / (int)promoDetail.AnyQty; 
                                    /*if (tmp > (LinePrice * Convert.ToDouble(LineQty)))
                                    {
                                        LineDiscountedPriceAmount = LinePrice;
                                        tmp = tmp - LinePrice * Convert.ToDouble(LineQty);
                                    }
                                    else
                                    {
                                        LineDiscountedPriceAmount = tmp / Convert.ToDouble(promoUntQty);
                                        tmp = 0;
                                    }*/
                                }
                                else
                                {
                                    LineDiscountedPriceAmount = 0;
                                }
                            }
                            else if (PromoDiscountAmount != 0) //using promo discount amount
                            {

                                if (LinePrice > 0)
                                {
                                    if (tmpPromoAmount > (LinePrice * Convert.ToDouble(LineQty)))
                                    {
                                        LineDiscountedPriceAmount = LinePrice;
                                        tmpPromoAmount = tmpPromoAmount - (Convert.ToDouble(LineQty) * LinePrice);
                                    }
                                    else
                                    {
                                        var newTmpPromoAmount = tmpPromoAmount / (Convert.ToDouble(anyQty) / Convert.ToDouble(LineQty));
                                        LineDiscountedPriceAmount = ((LinePrice * Convert.ToDouble(LineQty)) - newTmpPromoAmount) / Convert.ToDouble(promoUntQty);
                                        tmpPromoAmount = tmpPromoAmount - newTmpPromoAmount;

                                        //LineDiscountedPriceAmount = ((LinePrice * Convert.ToDouble(LineQty)) - tmpPromoAmount) / Convert.ToDouble(promoUntQty);
                                        //tmpPromoAmount = 0;
                                    }

                                }
                            }
                            else //using percentage
                            {
                                LineDiscountedPriceAmount = PromoDiscountPercentage;
                            }
                            Logger.writeLog("Start Set Discount ");
                            //cek if using price or discount
                            if (PromoDiscountPercentage > 0)
                            {
                                pos.SetOrderLineToUsePromoPrice(LineID, false);
                                pos.SetOrderLinePromoDiscountNew(LineID, (double)PromoDiscountPercentage, out status);
                            }
                            else
                            {
                                pos.SetOrderLineToUsePromoPrice(LineID, true);
                                pos.SetOrderLinePromoUnitPriceNew(LineID, promoUntQty % (int)promoDetail.AnyQty > 0 ? (decimal)Math.Round(LineDiscountedPriceAmount, 2) : (decimal)LineDiscountedPriceAmount, out status);
                            }

                            TotalDiscount += Math.Round(Convert.ToDouble(promoUntQty) * (promoUntQty % (int)promoDetail.AnyQty > 0 ? (double)Math.Round(LineDiscountedPriceAmount, 2) : (double)LineDiscountedPriceAmount), 2);

                            if (LeftOverQty > 0)
                            {
                                Logger.writeLog("Start Set Left Over ");
                                pos.AddLeftOverItemToOrderForPromoLogic((new Item("ItemNo", pos.GetLineItemNo(LineID))),
                                   LeftOverQty,
                                   pos.GetPreferredDiscount(), false, OrderDets[(int)orderLine[index]].OrderDetDate, OrderDets[(int)orderLine[index]].PossibleItemGroupID, out status);
                            }

                            anyQty -= RoundUpQty(promoUntQty);
                        }

                    }
                    index++;
                }

                if (PromoDiscountAmount > 0)
                {
                    ItemGroupPromoAmount = TotalOrder - (Multiplier * PromoDiscountAmount);
                }

                if (PromoDiscountPercentage == 0 && ItemGroupPromoAmount != TotalDiscount)
                {
                    //Change the last discount to be 
                    //additional with the difference
                    string LastLineID = MultipleOrderLineID[MultipleOrderLineID.Count - 1].ToString();
                    decimal lineUnitQty = pos.GetLineQuantity(LastLineID, out status);

                    double diff = (ItemGroupPromoAmount - TotalDiscount) / Convert.ToDouble(lineUnitQty);

                    decimal lineUnitPrice = pos.GetLinePromoUnitPrice(LastLineID, out status);
                    pos.SetOrderLinePromoUnitPriceNew(LastLineID, (lineUnitPrice + (decimal)diff), out status);
                }
            }
        }

        public void ApplyAnyItemsNegativeQty(PromoCampaignDet promoDetail, int Multiplier, ArrayList orderLine, ArrayList ListPromoCampaignDetId, OrderDetCollection OrderDets)
        {
            ArrayList MultipleOrderLineID;

            if (orderLine.Count > 0)
            {
                int index = 0;

                double ItemGroupPromoAmount = 0;
                double PromoDiscountPercentage = 0;
                double PromoPrice = 0;
                double PromoDiscountAmount = 0;
                double TotalDiscount = 0;
                decimal anyQty = 0;
                string status = "";

                anyQty = Multiplier * (int)promoDetail.AnyQty;
                PromoPrice = (double)promoDetail.PromoPrice;
                PromoDiscountPercentage = (double)promoDetail.DiscPercent;
                PromoDiscountAmount = (double)promoDetail.DiscAmount;
                MultipleOrderLineID = new ArrayList();
                ItemGroupPromoAmount = Multiplier * PromoPrice;

                double tmp = Multiplier * PromoPrice;
                double tmpPromoAmount = Multiplier * PromoDiscountAmount;
                double TotalOrder = 0;

                while (Math.Abs(anyQty) > 0)
                {
                    if ((int)ListPromoCampaignDetId[index] == promoDetail.PromoCampaignDetID)
                    {
                        if (IsValidPromoLineItem(OrderDets[(int)orderLine[index]]))
                        {
                            string LineID = OrderDets[(int)orderLine[index]].OrderDetID;
                            string ItemNo = OrderDets[(int)orderLine[index]].ItemNo;
                            MultipleOrderLineID.Add(LineID);

                            decimal promoUntQty = 0;
                            decimal LeftOverQty = 0;

                            if (Math.Abs(RoundUpQty(OrderDets[(int)orderLine[index]].Quantity.GetValueOrDefault(0))) >= Math.Abs(anyQty))
                            {
                                promoUntQty = anyQty;
                                LeftOverQty = RoundUpQty(OrderDets[(int)orderLine[index]].Quantity.GetValueOrDefault(0)) - promoUntQty;
                            }
                            else
                            {
                                promoUntQty = OrderDets[(int)orderLine[index]].Quantity.GetValueOrDefault(0);
                            }
                            //Logger.writeLog("Start Change Qty");
                            if (Math.Abs(LeftOverQty) > 0)
                            {
                                //pos.ChangeOrderLineQuantity(LineID, promoUntQty, false, out status);
                                pos.ChangeOrderLineQuantityForNegativePromo(LineID, promoUntQty, false, out status);
                                pos.SetOrderLineAsPromo(LineID, true, promoDetail.PromoCampaignHdrID, promoDetail.PromoCampaignDetID, out status);
                            }
                            else
                            {
                                pos.SetOrderLineAsPromo(LineID, true, promoDetail.PromoCampaignHdrID, promoDetail.PromoCampaignDetID, out status);
                            }
                            decimal LineQty = promoUntQty;
                            double LinePrice = (double)OrderDets[(int)orderLine[index]].UnitPrice;
                            double LineDiscountedPriceAmount = 0;

                            TotalOrder += LinePrice * Convert.ToDouble(LineQty);

                            //cek if using price or discount
                            if (PromoPrice != 0)
                            {
                                //using price
                                if (Math.Abs(LinePrice) > 0)
                                {
                                    //Calculate line discount accordingly:      
                                    LineDiscountedPriceAmount = PromoPrice / (int)promoDetail.AnyQty;
                                    /*if (tmp > (LinePrice * Convert.ToDouble(LineQty)))
                                    {
                                        LineDiscountedPriceAmount = LinePrice;
                                        tmp = tmp - LinePrice * Convert.ToDouble(LineQty);
                                    }
                                    else
                                    {
                                        LineDiscountedPriceAmount = tmp / Convert.ToDouble(promoUntQty);
                                        tmp = 0;
                                    }*/
                                }
                                else
                                {
                                    LineDiscountedPriceAmount = 0;
                                }
                            }
                            else if (PromoDiscountAmount != 0) //using promo discount amount
                            {

                                if (Math.Abs(LinePrice) > 0)
                                {
                                    if (Math.Abs(tmpPromoAmount) > Math.Abs(LinePrice * Convert.ToDouble(LineQty)))
                                    {
                                        LineDiscountedPriceAmount = LinePrice;
                                        tmpPromoAmount = tmpPromoAmount - (Convert.ToDouble(LineQty) * LinePrice);
                                    }
                                    else
                                    {
                                        LineDiscountedPriceAmount = ((LinePrice * Convert.ToDouble(LineQty)) - tmpPromoAmount) / Convert.ToDouble(promoUntQty);
                                        tmpPromoAmount = 0;
                                    }

                                }
                            }
                            else //using percentage
                            {
                                LineDiscountedPriceAmount = PromoDiscountPercentage;
                            }
                            Logger.writeLog("Start Set Discount ");
                            //cek if using price or discount
                            if (Math.Abs(PromoDiscountPercentage) > 0)
                            {
                                pos.SetOrderLineToUsePromoPrice(LineID, false);
                                pos.SetOrderLinePromoDiscountNew(LineID, (double)PromoDiscountPercentage, out status);
                            }
                            else
                            {
                                pos.SetOrderLineToUsePromoPrice(LineID, true);
                                pos.SetOrderLinePromoUnitPriceNew(LineID, promoUntQty % (int)promoDetail.AnyQty > 0 ? (decimal)Math.Round(LineDiscountedPriceAmount, 2) : (decimal)LineDiscountedPriceAmount, out status);
                            }

                            TotalDiscount += Math.Round(Convert.ToDouble(promoUntQty) * (promoUntQty % (int)promoDetail.AnyQty > 0 ? (double)Math.Round(LineDiscountedPriceAmount, 2) : (double)LineDiscountedPriceAmount), 2);

                            if (Math.Abs(LeftOverQty) > 0)
                            {
                                Logger.writeLog("Start Set Left Over ");
                                pos.AddLeftOverItemToOrderForPromoLogic((new Item("ItemNo", pos.GetLineItemNo(LineID))),
                                   LeftOverQty,
                                   pos.GetPreferredDiscount(), false, OrderDets[(int)orderLine[index]].OrderDetDate, OrderDets[(int)orderLine[index]].PossibleItemGroupID, out status);
                            }

                            anyQty -= RoundUpQty(promoUntQty);
                        }

                    }
                    index++;
                }

                if (Math.Abs(PromoDiscountAmount) > 0)
                {
                    ItemGroupPromoAmount = TotalOrder - (Multiplier * PromoDiscountAmount);
                }

                if (PromoDiscountPercentage == 0 && ItemGroupPromoAmount != TotalDiscount)
                {
                    //Change the last discount to be 
                    //additional with the difference
                    string LastLineID = MultipleOrderLineID[MultipleOrderLineID.Count - 1].ToString();
                    decimal lineUnitQty = pos.GetLineQuantity(LastLineID, out status);

                    double diff = (ItemGroupPromoAmount - TotalDiscount) / Convert.ToDouble(lineUnitQty);

                    decimal lineUnitPrice = pos.GetLinePromoUnitPrice(LastLineID, out status);
                    pos.SetOrderLinePromoUnitPriceNew(LastLineID, (lineUnitPrice + (decimal)diff), out status);
                }
            }
        }

        public void ApplyAnyItems(PromoCampaignDet promoDetail, int Multiplier, ArrayList orderLine, ArrayList ListPromoCampaignDetId, OrderDetCollection OrderDets, int CountedMatchedItems)
        {
            ArrayList MultipleOrderLineID;

            if (orderLine.Count > 0)
            {
                int index = 0;

                double ItemGroupPromoAmount = 0;
                double PromoDiscountPercentage = 0;
                double PromoPrice = 0;
                double PromoDiscountAmount = 0;
                double TotalDiscount = 0;
                decimal anyQty = 0;
                string status = "";

                anyQty = Multiplier * (int)promoDetail.AnyQty;
                PromoPrice = (double)promoDetail.PromoPrice;
                PromoDiscountPercentage = (double)promoDetail.DiscPercent;
                PromoDiscountAmount = (double)promoDetail.DiscAmount;
                MultipleOrderLineID = new ArrayList();
                ItemGroupPromoAmount = Multiplier * PromoPrice;


                double tmp = Multiplier * PromoPrice;
                double tmpPromoAmount = Multiplier * PromoDiscountAmount;
                double TotalOrder = 0;

                while (anyQty > 0)
                {
                    if ((int)ListPromoCampaignDetId[index] == promoDetail.PromoCampaignDetID)
                    {
                        if (IsValidPromoLineItem(OrderDets[(int)orderLine[index]]))
                        {
                            string LineID = OrderDets[(int)orderLine[index]].OrderDetID;
                            string ItemNo = OrderDets[(int)orderLine[index]].ItemNo;
                            MultipleOrderLineID.Add(LineID);

                            decimal promoUntQty = 0;
                            decimal LeftOverQty = 0;

                            if (OrderDets[(int)orderLine[index]].Quantity >= anyQty)
                            {
                                promoUntQty = anyQty;
                                LeftOverQty = OrderDets[(int)orderLine[index]].Quantity.GetValueOrDefault(0) - promoUntQty;
                            }
                            else
                            {
                                promoUntQty = OrderDets[(int)orderLine[index]].Quantity.GetValueOrDefault(0);
                            }

                            pos.ChangeOrderLineQuantity(LineID, promoUntQty, false, out status);
                            pos.SetOrderLineAsPromo(LineID, true, promoDetail.PromoCampaignHdrID, promoDetail.PromoCampaignDetID, out status);

                            decimal LineQty = promoUntQty;
                            double LinePrice = (double)OrderDets[(int)orderLine[index]].UnitPrice;
                            double LineDiscountedPriceAmount = 0;

                            TotalOrder += LinePrice * Convert.ToDouble(LineQty);

                            //cek if using price or discount
                            if (PromoPrice != 0)
                            {
                                //using price
                                if (LinePrice > 0)
                                {
                                    //Calculate line discount accordingly:      

                                    if (tmp > (LinePrice * Convert.ToDouble(LineQty)))
                                    {
                                        LineDiscountedPriceAmount = LinePrice;
                                        tmp = tmp - LinePrice * Convert.ToDouble(LineQty);
                                    }
                                    else
                                    {
                                        LineDiscountedPriceAmount = tmp / Convert.ToDouble(promoUntQty);
                                        tmp = 0;
                                    }
                                }
                                else
                                {
                                    LineDiscountedPriceAmount = 0;
                                }
                            }
                            else if (PromoDiscountAmount != 0) //using promo discount amount
                            {

                                if (LinePrice > 0)
                                {
                                    if (tmpPromoAmount > (LinePrice * Convert.ToDouble(LineQty)))
                                    {
                                        LineDiscountedPriceAmount = LinePrice;
                                        tmpPromoAmount = tmpPromoAmount - (Convert.ToDouble(LineQty) * LinePrice);
                                    }
                                    else
                                    {
                                        LineDiscountedPriceAmount = ((LinePrice * Convert.ToDouble(LineQty)) - tmpPromoAmount) / Convert.ToDouble(promoUntQty);
                                        tmpPromoAmount = 0;
                                    }

                                }
                            }
                            else //using percentage
                            {
                                LineDiscountedPriceAmount = PromoDiscountPercentage;
                            }

                            //cek if using price or discount
                            if (PromoDiscountPercentage > 0)
                            {
                                pos.SetOrderLineToUsePromoPrice(LineID, false);
                                pos.SetOrderLinePromoDiscountNew(LineID, (double)PromoDiscountPercentage, out status);
                            }
                            else
                            {
                                pos.SetOrderLineToUsePromoPrice(LineID, true);
                                pos.SetOrderLinePromoUnitPriceNew(LineID, (decimal)LineDiscountedPriceAmount, out status);
                            }

                            TotalDiscount += Math.Round(Convert.ToDouble(promoUntQty) * (double)LineDiscountedPriceAmount, 2);

                            if (LeftOverQty > 0)
                            {
                                pos.AddLeftOverItemToOrderForPromoLogic((new Item("ItemNo", pos.GetLineItemNo(LineID))),
                                   LeftOverQty,
                                   pos.GetPreferredDiscount(), false, OrderDets[(int)orderLine[index]].OrderDetDate, OrderDets[(int)orderLine[index]].PossibleItemGroupID, out status);
                            }

                            anyQty -= promoUntQty;
                        }

                    }
                    index++;
                }

                if (PromoDiscountAmount > 0)
                {
                    ItemGroupPromoAmount = TotalOrder - (Multiplier * PromoDiscountAmount);
                }

                if (PromoDiscountPercentage == 0 && ItemGroupPromoAmount != TotalDiscount)
                {
                    //Change the last discount to be 
                    //additional with the difference
                    string LastLineID = MultipleOrderLineID[MultipleOrderLineID.Count - 1].ToString();
                    decimal lineUnitQty = pos.GetLineQuantity(LastLineID, out status);

                    double diff = (ItemGroupPromoAmount - TotalDiscount) / Convert.ToDouble(lineUnitQty);

                    decimal lineUnitPrice = pos.GetLinePromoUnitPrice(LastLineID, out status);
                    pos.SetOrderLinePromoUnitPriceNew(LastLineID, (lineUnitPrice + (decimal)diff), out status);
                }
            }
        }


        public void ApplyBundleItems(PromoCampaignDet promoDetail, int Multiplier, ArrayList orderLine, ArrayList ListPromoCampaignDetId, OrderDetCollection OrderDets)
        {
            ArrayList MultipleOrderLineID;

            if (orderLine.Count > 0)
            {
                int index = 0;

                double ItemGroupPromoAmount = 0;
                double PromoDiscountPercentage = 0;
                double PromoPrice = 0;
                double PromoDiscountAmount = 0;
                double TotalDiscount = 0;

                string status = "";
                int UnitQty = Multiplier * (int)promoDetail.UnitQty;
                PromoPrice = (double)promoDetail.PromoPrice;
                PromoDiscountPercentage = (double)promoDetail.DiscPercent;
                PromoDiscountAmount = (double)promoDetail.DiscAmount;
                MultipleOrderLineID = new ArrayList();
                ItemGroupPromoAmount = Multiplier * PromoPrice;

                double tmp = Multiplier * PromoPrice;
                double tmpPromoAmount = Multiplier * PromoDiscountAmount;
                double TotalOrder = 0;

                for (index = 0; index < orderLine.Count; index++)
                {
                    if ((int)ListPromoCampaignDetId[index] == promoDetail.PromoCampaignDetID)
                    {
                        if (IsValidPromoLineItem(OrderDets[(int)orderLine[index]]))
                        {
                            string LineID = OrderDets[(int)orderLine[index]].OrderDetID;
                            string ItemNo = OrderDets[(int)orderLine[index]].ItemNo;
                            MultipleOrderLineID.Add(LineID);

                            //ViewPromoMasterDetailAnyCollection detail = new ViewPromoMasterDetailAnyCollection();
                            //detail.Where(ViewPromoMasterDetailAny.Columns.PromoCampaignDetID, Comparison.Equals, promoDetail.PromoCampaignDetID);
                            //detail.Where(ViewPromoMasterDetailAny.Columns.ItemNo, Comparison.Equals, ItemNo);
                            //detail.Load();

                            DataTable tempDT = GetViewPromoMasterDetailUnitQty(promoDetail.PromoCampaignDetID.ToString()).Select( "ItemNo='" + ItemNo + "'").CopyToDataTable();

                            //decimal promoUntQty = Multiplier * (int)detail[0].UnitQty;
                            decimal promoUntQty = Multiplier * tempDT.Rows[0]["UnitQty"].ToString().GetIntValue();
                            decimal LeftOverQty = 0;

                            if (OrderDets[(int)orderLine[index]].Quantity >= promoUntQty)
                            {
                                LeftOverQty = OrderDets[(int)orderLine[index]].Quantity.GetValueOrDefault(0) - promoUntQty;
                            }

                            pos.ChangeOrderLineQuantityForPromoLogic(LineID, promoUntQty, false, out status);
                            pos.SetOrderLineAsPromo(LineID, true, promoDetail.PromoCampaignHdrID, promoDetail.PromoCampaignDetID, out status);

                            decimal LineQty = OrderDets[(int)orderLine[index]].Quantity.GetValueOrDefault(0);
                            double LinePrice = (double)OrderDets[(int)orderLine[index]].UnitPrice;
                            double LineDiscountedPriceAmount = 0;

                            TotalOrder += LinePrice * Convert.ToDouble(LineQty);

                            //cek if using price or discount
                            if (PromoPrice != 0)
                            {
                                //using price
                                if (LinePrice > 0)
                                {
                                    //Calculate line discount accordingly:                             
                                    if (tmp > (LinePrice * Convert.ToDouble(LineQty)))
                                    {
                                        LineDiscountedPriceAmount = LinePrice;
                                        tmp = tmp - LinePrice * Convert.ToDouble(LineQty);
                                    }
                                    else
                                    {
                                        LineDiscountedPriceAmount = tmp / Convert.ToDouble(promoUntQty);
                                        tmp = 0;
                                    }
                                }
                                else
                                {
                                    LineDiscountedPriceAmount = 0;
                                }
                            }
                            else if (PromoDiscountAmount != 0) //using promo discount amount
                            {

                                if (LinePrice > 0)
                                {
                                    if (tmpPromoAmount > (LinePrice * Convert.ToDouble(LineQty)))
                                    {
                                        LineDiscountedPriceAmount = LinePrice;
                                        tmpPromoAmount = tmpPromoAmount - (Convert.ToDouble(LineQty) * LinePrice);
                                    }
                                    else
                                    {
                                        LineDiscountedPriceAmount = ((LinePrice * Convert.ToDouble(LineQty)) - tmpPromoAmount) / Convert.ToDouble(promoUntQty);
                                        tmpPromoAmount = 0;
                                    }

                                }
                            }
                            else //using percentage
                            {
                                LineDiscountedPriceAmount = PromoDiscountPercentage;
                            }

                            //cek if using price or discount
                            if (PromoDiscountPercentage > 0)
                            {
                                pos.SetOrderLineToUsePromoPrice(LineID, false);
                                pos.SetOrderLinePromoDiscountNew(LineID, (double)PromoDiscountPercentage, out status);
                            }
                            else
                            {
                                pos.SetOrderLineToUsePromoPrice(LineID, true);
                                pos.SetOrderLinePromoUnitPriceNew(LineID, (decimal)LineDiscountedPriceAmount, out status);
                            }

                            TotalDiscount += Math.Round(Convert.ToDouble(promoUntQty) * (double)LineDiscountedPriceAmount, 2);

                            if (LeftOverQty > 0)
                            {
                                pos.AddLeftOverItemToOrderForPromoLogic((new Item("ItemNo", pos.GetLineItemNo(LineID))),
                                   LeftOverQty,
                                   pos.GetPreferredDiscount(), false, OrderDets[(int)orderLine[index]].OrderDetDate, OrderDets[(int)orderLine[index]].PossibleItemGroupID, out status);
                            }
                        }
                    }
                }

                if (PromoDiscountAmount > 0)
                {
                    ItemGroupPromoAmount = TotalOrder - (Multiplier * PromoDiscountAmount);
                }

                //if (ItemGroupPromoAmount != TotalDiscount) << code before fixing
                if (PromoDiscountPercentage == 0 && ItemGroupPromoAmount != TotalDiscount)
                {
                    //Change the last discount to be 
                    //additional with the difference
                    string LastLineID = MultipleOrderLineID[MultipleOrderLineID.Count - 1].ToString();
                    decimal lineUnitQty = pos.GetLineQuantity(LastLineID, out status);

                    double diff = (ItemGroupPromoAmount - TotalDiscount) / Convert.ToDouble(lineUnitQty);

                    decimal lineUnitPrice = pos.GetLinePromoUnitPrice(LastLineID, out status);
                    pos.SetOrderLinePromoUnitPriceNew(LastLineID, (lineUnitPrice + (decimal)diff), out status);
                }
            }
        }

        public void ApplyBundleItemsNegativeQty(PromoCampaignDet promoDetail, int Multiplier, ArrayList orderLine, ArrayList ListPromoCampaignDetId, OrderDetCollection OrderDets)
        {
            ArrayList MultipleOrderLineID;

            if (orderLine.Count > 0)
            {
                int index = 0;

                double ItemGroupPromoAmount = 0;
                double PromoDiscountPercentage = 0;
                double PromoPrice = 0;
                double PromoDiscountAmount = 0;
                double TotalDiscount = 0;

                string status = "";
                int UnitQty = Multiplier * (int)promoDetail.UnitQty;
                PromoPrice = (double)promoDetail.PromoPrice;
                PromoDiscountPercentage = (double)promoDetail.DiscPercent;
                PromoDiscountAmount = (double)promoDetail.DiscAmount;
                MultipleOrderLineID = new ArrayList();
                ItemGroupPromoAmount = Multiplier * PromoPrice;

                double tmp = Multiplier * PromoPrice;
                double tmpPromoAmount = Multiplier * PromoDiscountAmount;
                double TotalOrder = 0;

                for (index = 0; index < orderLine.Count; index++)
                {
                    if ((int)ListPromoCampaignDetId[index] == promoDetail.PromoCampaignDetID)
                    {
                        if (IsValidPromoLineItem(OrderDets[(int)orderLine[index]]))
                        {
                            string LineID = OrderDets[(int)orderLine[index]].OrderDetID;
                            string ItemNo = OrderDets[(int)orderLine[index]].ItemNo;
                            MultipleOrderLineID.Add(LineID);

                            //ViewPromoMasterDetailAnyCollection detail = new ViewPromoMasterDetailAnyCollection();
                            //detail.Where(ViewPromoMasterDetailAny.Columns.PromoCampaignDetID, Comparison.Equals, promoDetail.PromoCampaignDetID);
                            //detail.Where(ViewPromoMasterDetailAny.Columns.ItemNo, Comparison.Equals, ItemNo);
                            //detail.Load();

                            DataTable tempDT = GetViewPromoMasterDetailUnitQty(promoDetail.PromoCampaignDetID.ToString()).Select("ItemNo='" + ItemNo + "'").CopyToDataTable();

                            //decimal promoUntQty = Multiplier * (int)detail[0].UnitQty;
                            decimal promoUntQty = Multiplier * tempDT.Rows[0]["UnitQty"].ToString().GetIntValue();
                            decimal LeftOverQty = 0;

                            if (Math.Abs(OrderDets[(int)orderLine[index]].Quantity.GetValueOrDefault(0)) >= Math.Abs(promoUntQty))
                            {
                                LeftOverQty = OrderDets[(int)orderLine[index]].Quantity.GetValueOrDefault(0) - promoUntQty;
                            }

                            pos.ChangeOrderLineQuantityForPromoLogic(LineID, promoUntQty, false, out status);
                            pos.SetOrderLineAsPromo(LineID, true, promoDetail.PromoCampaignHdrID, promoDetail.PromoCampaignDetID, out status);

                            decimal LineQty = OrderDets[(int)orderLine[index]].Quantity.GetValueOrDefault(0);
                            double LinePrice = (double)OrderDets[(int)orderLine[index]].UnitPrice;
                            double LineDiscountedPriceAmount = 0;

                            TotalOrder += LinePrice * Convert.ToDouble(LineQty);

                            //cek if using price or discount
                            if (PromoPrice != 0)
                            {
                                //using price
                                if (LinePrice > 0)
                                {
                                    //Calculate line discount accordingly:                             
                                    if (Math.Abs(tmp) > Math.Abs(LinePrice * Convert.ToDouble(LineQty)))
                                    {
                                        LineDiscountedPriceAmount = LinePrice;
                                        tmp = tmp - LinePrice * Convert.ToDouble(LineQty);
                                    }
                                    else
                                    {
                                        LineDiscountedPriceAmount = tmp / Convert.ToDouble(promoUntQty);
                                        tmp = 0;
                                    }
                                }
                                else
                                {
                                    LineDiscountedPriceAmount = 0;
                                }
                            }
                            else if (PromoDiscountAmount != 0) //using promo discount amount
                            {

                                if (LinePrice > 0)
                                {
                                    if (Math.Abs(tmpPromoAmount) > Math.Abs(LinePrice * Convert.ToDouble(LineQty)))
                                    {
                                        LineDiscountedPriceAmount = LinePrice;
                                        tmpPromoAmount = tmpPromoAmount - (Convert.ToDouble(LineQty) * LinePrice);
                                    }
                                    else
                                    {
                                        LineDiscountedPriceAmount = ((LinePrice * Convert.ToDouble(LineQty)) - tmpPromoAmount) / Convert.ToDouble(promoUntQty);
                                        tmpPromoAmount = 0;
                                    }

                                }
                            }
                            else //using percentage
                            {
                                LineDiscountedPriceAmount = PromoDiscountPercentage;
                            }

                            //cek if using price or discount
                            if (PromoDiscountPercentage > 0)
                            {
                                pos.SetOrderLineToUsePromoPrice(LineID, false);
                                pos.SetOrderLinePromoDiscountNew(LineID, (double)PromoDiscountPercentage, out status);
                            }
                            else
                            {
                                pos.SetOrderLineToUsePromoPrice(LineID, true);
                                pos.SetOrderLinePromoUnitPriceNew(LineID, (decimal)LineDiscountedPriceAmount, out status);
                            }

                            TotalDiscount += Math.Round(Convert.ToDouble(promoUntQty) * (double)LineDiscountedPriceAmount, 2);

                            if (Math.Abs(LeftOverQty) > 0)
                            {
                                pos.AddLeftOverItemToOrderForPromoLogic((new Item("ItemNo", pos.GetLineItemNo(LineID))),
                                   LeftOverQty,
                                   pos.GetPreferredDiscount(), false, OrderDets[(int)orderLine[index]].OrderDetDate, OrderDets[(int)orderLine[index]].PossibleItemGroupID, out status);
                            }
                        }
                    }
                }

                if (PromoDiscountAmount > 0)
                {
                    ItemGroupPromoAmount = TotalOrder - (Multiplier * PromoDiscountAmount);
                }

                //if (ItemGroupPromoAmount != TotalDiscount) << code before fixing
                if (PromoDiscountPercentage == 0 && ItemGroupPromoAmount != TotalDiscount)
                {
                    //Change the last discount to be 
                    //additional with the difference
                    string LastLineID = MultipleOrderLineID[MultipleOrderLineID.Count - 1].ToString();
                    decimal lineUnitQty = pos.GetLineQuantity(LastLineID, out status);

                    double diff = (ItemGroupPromoAmount - TotalDiscount) / Convert.ToDouble(lineUnitQty);

                    decimal lineUnitPrice = pos.GetLinePromoUnitPrice(LastLineID, out status);
                    pos.SetOrderLinePromoUnitPriceNew(LastLineID, (lineUnitPrice + (decimal)diff), out status);
                }
            }
        }

        public bool UndoPromoToOrder()
        {
            string status = "";
            pos.DeleteAllIsPromoFOCLine();
            OrderDetCollection myOrderLine = pos.FetchUnsavedOrderDet();
            for (int j = 0; j < myOrderLine.Count; j++)
            {
                if (myOrderLine[j].IsPromo && myOrderLine[j].IsSpecial)
                {
                    continue;
                }

                bool isRefund = !string.IsNullOrEmpty(myOrderLine[j].ReturnedReceiptNo) || !string.IsNullOrEmpty(myOrderLine[j].RefundOrderDetID);
                decimal promoDiscount = Convert.ToDecimal(myOrderLine[j].PromoDiscount);

                //if (myOrderLine[j].Quantity < 0 && !myOrderLine[j].IsVoided)
                //if ((myOrderLine[j].Quantity < 0 && !isRefund) && !myOrderLine[j].IsVoided)  // If refunded item, allow to undo promo even if it's negative qty
                //{
                //    continue;
                //}

                if (myOrderLine[j].IsPromo)
                {
                    pos.SetOrderLineAsPromo(myOrderLine[j].OrderDetID, false, 0, 0, out status);
                    if (isRefund)
                    //if (myOrderLine[j].Quantity < 0 && !myOrderLine[j].IsVoided || isRefund)   // If it's refunded item, apply the discount from promo
                    {
                        //pos.applyDiscount(promoDiscount);
                        if (!pos.ChangeOrderLineDiscount(myOrderLine[j].OrderDetID, promoDiscount, false, false, out status))
                        {
                            throw new Exception("status");
                        }
                    }
                }
            }
            pos.TryMergeALLOrderLine();
            return true;
        }

        private void ApplyPromoToLineItem(OrderDet orderDet, ViewPromoMasterDetail viewPromoMasterDetail)
        {
            switch (viewPromoMasterDetail.CampaignType)
            {
                case PromotionAdminController.BuyXGetYFree:
                    ApplyBuyXGetYFree(orderDet, viewPromoMasterDetail);
                    break;
                case PromotionAdminController.DiscountByCategory:
                    ApplyDirectDiscount(orderDet, viewPromoMasterDetail);
                    break;
                case PromotionAdminController.DiscountByItem:
                    ApplyDirectDiscount(orderDet, viewPromoMasterDetail);
                    break;
                case PromotionAdminController.MultiTierDiscount:
                    ApplyMultiTierDiscount(orderDet, viewPromoMasterDetail);
                    break;
            }
        }

        private int CalculatePromoValue(
            OrderDet myOrderLine,
            ViewPromoMasterDetailCollection viewPromoMasterDetails)
        {
            decimal value = 0, maxvalue = 0;
            int maxIndex = -1;
            string status;
            for (int i = 0; i < viewPromoMasterDetails.Count; i++)
            {
                switch (viewPromoMasterDetails[i].CampaignType)
                {
                    case PromotionAdminController.BuyXGetYFree:
                        value = CalculateBuyXGetYFreeByItem(myOrderLine, viewPromoMasterDetails[i], out status);
                        break;
                    case PromotionAdminController.DiscountByCategory:
                        value = CalculateDiscountByCategoryByItem(myOrderLine, viewPromoMasterDetails[i], out status);
                        break;
                    case PromotionAdminController.DiscountByItem:
                        value = CalculateDiscountByItemByItem(myOrderLine, viewPromoMasterDetails[i], out status);
                        break;
                    case PromotionAdminController.MultiTierDiscount:
                        value = CalculateDiscountTierByItem(myOrderLine, viewPromoMasterDetails[i], out status);
                        break;
                }
                if (value > maxvalue)
                {
                    maxvalue = value;
                    maxIndex = i;
                }
            }
            return maxIndex;
        }

        private decimal CalculateBuyXGetYFreeByItem
            (OrderDet myOrderLine, ViewPromoMasterDetail pHdr, out string status)
        {
            if (pHdr.CampaignType != PromotionAdminController.BuyXGetYFree)
            {
                status = "You have entered an incorrect promo";
                return -1;
            }


            //check item
            if (pHdr.ItemNo != myOrderLine.ItemNo)
            {
                status = "Incorrect Item";
                return -1;
            }
            //calculate
            //calculate total free items
            int TotalFreeItem = ((int)Math.Floor((double)(myOrderLine.Quantity / pHdr.UnitQty.Value))) * pHdr.FreeQty.Value;

            //calculate value, promohdrid, promodetid
            status = "";
            return (TotalFreeItem * (new Item("ItemNo", pHdr.FreeItemNo)).RetailPrice);

        }

        private bool ApplyBuyXGetYFree
            (OrderDet myOrderLine, ViewPromoMasterDetail myPromo)
        {
            try
            {
                string status;
                int TotalFreeItem;

                //calculate total free items

                TotalFreeItem = ((int)Math.Floor((double)(myOrderLine.Quantity / myPromo.UnitQty.Value))) * myPromo.FreeQty.Value;

                if (TotalFreeItem > 0)
                {
                    decimal LeftOverQty = myOrderLine.Quantity.GetValueOrDefault(0) % myPromo.UnitQty.Value;

                    //add a new orderline with FOC set = 1 
                    pos.SetOrderLinePromoDiscount(myOrderLine.OrderDetID, 0, out status);
                    pos.AddFOCPromoItem((new Item("ItemNo", myPromo.FreeItemNo)), TotalFreeItem, myOrderLine.OrderDetDate, myPromo, out status);
                    pos.ChangeOrderLineQuantity(myOrderLine.OrderDetID, ((int)Math.Floor((double)(myOrderLine.Quantity / myPromo.UnitQty.Value))) * myPromo.UnitQty.Value, false, out status);
                    pos.SetOrderLineAsPromo(myOrderLine.OrderDetID, true, myPromo.PromoCampaignHdrID, myPromo.PromoCampaignDetID, out status);

                    //Count left overs.... and create a new item line

                    if (LeftOverQty > 0)
                        pos.AddLeftOverItemToOrder((new Item("ItemNo", myOrderLine.ItemNo)),
                            LeftOverQty, pos.GetPreferredDiscount(), false, myOrderLine.OrderDetDate, out status);
                }
                return true;

            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return false;
            }
        }

        private bool ApplyDirectDiscount
                (OrderDet myOrderLine, ViewPromoMasterDetail myPromo)
        {
            try
            {
                string status;
                //check membership map
                PromoMembershipMapCollection prm = new PromoMembershipMapCollection();
                if (pos.CurrentMember != null)
                {
                    prm.Where(PromoMembershipMap.Columns.MembershipGroupID,
                        pos.CurrentMember.MembershipGroupId);
                    prm.Where(PromoMembershipMap.Columns.PromoCampaignHdrID,
                        myPromo.PromoCampaignHdrID);
                    prm.Where(PromoMembershipMap.Columns.Deleted, false);
                    prm.Load();
                }

                if (prm.Count > 0)
                {
                    if (prm[0].UseMembershipPrice)
                    {
                        if (prm[0].MembershipPrice < 0)
                        {
                            return false;
                        }
                        pos.SetOrderLineAsPromo(myOrderLine.OrderDetID, true,
                            myPromo.PromoCampaignHdrID, myPromo.PromoCampaignDetID, out status);
                        pos.SetOrderLineToUsePromoPrice(myOrderLine.OrderDetID, true);
                        pos.SetOrderLinePromoUnitPrice(myOrderLine.OrderDetID,
                            prm[0].MembershipPrice, out status);
                    }
                    else
                    {
                        if (prm[0].MembershipDiscount < 0 | prm[0].MembershipDiscount > 100)
                        {
                            return false;
                        }
                        pos.SetOrderLineAsPromo
                            (myOrderLine.OrderDetID, true, myPromo.PromoCampaignHdrID,
                             myPromo.PromoCampaignDetID, out status);
                        pos.SetOrderLineToUsePromoPrice(myOrderLine.OrderDetID, false);
                        pos.SetOrderLinePromoDiscount
                            (myOrderLine.OrderDetID, (double)prm[0].MembershipDiscount, out status);
                    }
                }
                else
                {
                    if (myPromo.UsePrice)
                    {
                        if (myPromo.PromoPrice < 0)
                        {
                            return false;
                        }

                        pos.SetOrderLineAsPromo(myOrderLine.OrderDetID, true, myPromo.PromoCampaignHdrID, myPromo.PromoCampaignDetID, out status);
                        pos.SetOrderLineToUsePromoPrice(myOrderLine.OrderDetID, true);

                        pos.SetOrderLinePromoUnitPrice(myOrderLine.OrderDetID,
                             myPromo.PromoPrice.Value, out status);

                    }
                    else
                    {
                        if (myPromo.PromoDiscount < 0 | myPromo.PromoDiscount > 100)
                        {
                            return false;
                        }

                        pos.SetOrderLineAsPromo
                            (myOrderLine.OrderDetID, true, myPromo.PromoCampaignHdrID,
                             myPromo.PromoCampaignDetID, out status);

                        pos.SetOrderLineToUsePromoPrice(myOrderLine.OrderDetID, false);
                        pos.SetOrderLinePromoDiscount(myOrderLine.OrderDetID,
                            (double)myPromo.PromoDiscount, out status);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return false;
            }
        }

        private bool ApplyMultiTierDiscount
            (OrderDet myOrderLine, ViewPromoMasterDetail myPromo)
        {
            try
            {
                string status;
                PromoDiscountTierCollection discountTier = new PromoDiscountTierCollection();
                discountTier.Where(PromoDiscountTier.Columns.PromoCampaignHdrID, myPromo.PromoCampaignHdrID);
                discountTier.Where(PromoDiscountTier.Columns.WhichQty, Comparison.LessOrEquals, myOrderLine.Quantity);
                discountTier.OrderByAsc("WhichQty");
                discountTier.Load();
                if (discountTier.Count > 0)
                {
                    double discount = discountTier[discountTier.Count - 1].Discount;
                    pos.SetOrderLineAsPromo(myOrderLine.OrderDetID, true, myPromo.PromoCampaignHdrID, myPromo.PromoCampaignDetID, out status);
                    pos.SetOrderLinePromoDiscount(myOrderLine.OrderDetID, discount, out status);
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return false;
            }
            /*
             * 
             * --The complicated version--
             * 
            decimal TotalAmtAfterPromo;                        


            //get free quantity
            MinQuantity = myPromo.MinQuantity;
            int LineQty = myOrderLine.Quantity;
            decimal LinePrice = myOrderLine.UnitPrice;            
                

            //Pull the DiscountTier
            PromoDiscountTierCollection discountTier = new PromoDiscountTierCollection();
            discountTier.Where(PromoDiscountTier.Columns.PromoCampaignHdrID, MaxPromoHdrID);
            discountTier.OrderByAsc("WhichQty");
            discountTier.Load();
            int maxTierQty = discountTier[discountTier.Count - 1].WhichQty;
            int k = 0;
            int IndexQty = 1;                
            //Pull the discounts
            TotalAmtAfterPromo = 0;
            
                while (IndexQty <= LineQty)
                {
                    //Fetch IndexQty from the collection and check if there is any discount...
                    if (discountTier[k].WhichQty < IndexQty)
                    {
                        k++;
                        if (k == discountTier.Count)
                            break;
                    }
                    else if (discountTier[k].WhichQty == IndexQty)
                    {
                        //Discount Tier WhichQty >= IndexQty
                        TotalAmtAfterPromo += LinePrice * (1 - ((decimal)discountTier[k].Discount / 100));
                        IndexQty += 1;
                    }
                    else
                    {
                        IndexQty += 1;
                        TotalAmtAfterPromo += LinePrice;
                    }
                }
            

            double discount = (double)(((LineQty * LinePrice - TotalAmtAfterPromo) / (LinePrice * LineQty)) * 100);
             
            pos.SetOrderLineAsPromo(OrderLineID, true, out status);
            pos.SetOrderLinePromoDiscount(OrderLineID, discount, out status);
            */
        }

        private decimal CalculateDiscountByCategoryByItem(
            OrderDet myOrderLine, ViewPromoMasterDetail pHdr, out string status)
        {
            try
            {
                if (pHdr.CampaignType != PromotionAdminController.DiscountByCategory)
                {
                    status = "You have entered an incorrect promo";
                    return -1;
                }

                //check item
                if (pHdr.CategoryName != myOrderLine.Item.CategoryName)
                {
                    status = "Incorrect Category";
                    return -1;
                }
                //The value will be how much discount you will get
                status = "";
                PromoMembershipMapCollection prm = new PromoMembershipMapCollection();
                if (pos.CurrentMember != null)
                {
                    prm.Where(PromoMembershipMap.Columns.MembershipGroupID,
                        pos.CurrentMember.MembershipGroupId);
                    prm.Where(PromoMembershipMap.Columns.PromoCampaignHdrID,
                        pHdr.PromoCampaignHdrID);
                    prm.Where(PromoMembershipMap.Columns.Deleted, false);
                    prm.Load();
                }
                decimal value;
                if (prm.Count > 0)
                {
                    if (prm[0].UseMembershipPrice)
                    {
                        //use membership price
                        value = myOrderLine.Quantity.GetValueOrDefault(0) *
                                 myOrderLine.OriginalRetailPrice -
                                     myOrderLine.Quantity.GetValueOrDefault(0) *
                                 prm[0].MembershipPrice;
                    }
                    else
                    {
                        //use discount...
                        value = (decimal)(prm[0].MembershipDiscount / 100) * myOrderLine.Quantity.GetValueOrDefault(0) *
                                 myOrderLine.Item.RetailPrice;
                    }
                }
                else
                {
                    if (pHdr.UsePrice)
                    {
                        //use price
                        value = myOrderLine.Quantity.GetValueOrDefault(0) *
                                 myOrderLine.OriginalRetailPrice -
                                myOrderLine.Quantity.GetValueOrDefault(0) *
                                 (decimal)pHdr.PromoPrice;
                    }
                    else
                    {
                        //use discount...
                        value = (decimal)(pHdr.PromoDiscount / 100) * myOrderLine.Quantity.GetValueOrDefault(0) *
                                 myOrderLine.Item.RetailPrice;
                    }
                }
                return value;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return -1;
            }

        }

        private decimal CalculateDiscountByItemByItem
            (OrderDet myOrderLine, ViewPromoMasterDetail pHdr, out string status)
        {
            try
            {
                if (pHdr.CampaignType != PromotionAdminController.DiscountByItem)
                {
                    status = "You have entered an incorrect promo";
                    return -1;
                }

                //check item
                /*if (pHdr.ItemNo != myOrderLine.ItemNo)
                {
                    status = "Incorrect Item";
                    return -1;
                }*/

                //The value will be how much discount you will get
                status = "";
                PromoMembershipMapCollection prm = new PromoMembershipMapCollection();
                if (pos.CurrentMember != null)
                {
                    prm.Where(PromoMembershipMap.Columns.MembershipGroupID,
                        pos.CurrentMember.MembershipGroupId);
                    prm.Where(PromoMembershipMap.Columns.PromoCampaignHdrID,
                        pHdr.PromoCampaignHdrID);
                    prm.Where(PromoMembershipMap.Columns.Deleted, false);
                    prm.Load();
                }
                decimal value;
                if (prm.Count > 0)
                {
                    if (prm[0].UseMembershipPrice)
                    {
                        //use membership price
                        value = myOrderLine.Quantity.GetValueOrDefault(0) *
                                 myOrderLine.OriginalRetailPrice -
                                     myOrderLine.Quantity.GetValueOrDefault(0) *
                                 prm[0].MembershipPrice;
                    }
                    else
                    {
                        //use discount...
                        value = (decimal)(prm[0].MembershipDiscount / 100) * myOrderLine.Quantity.GetValueOrDefault(0) *
                                 myOrderLine.Item.RetailPrice;
                    }
                }
                else
                {
                    if (pHdr.UsePrice)
                    {
                        //use price
                        value = myOrderLine.Quantity.GetValueOrDefault(0) *
                                 myOrderLine.OriginalRetailPrice -
                                myOrderLine.Quantity.GetValueOrDefault(0) *
                                 (decimal)pHdr.PromoPrice;
                    }
                    else
                    {
                        //use discount...
                        value = (decimal)(pHdr.PromoDiscount / 100) * myOrderLine.Quantity.GetValueOrDefault(0) *
                                 myOrderLine.Item.RetailPrice;
                    }
                }
                return value;

            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return -1;
            }
        }

        private decimal CalculateDiscountTierByItem
            (OrderDet myOrderLine, ViewPromoMasterDetail pHdr, out string status)
        {
            try
            {
                decimal value = 0;
                status = "";
                PromoDiscountTierCollection discountTier = new PromoDiscountTierCollection();
                discountTier.Where(PromoDiscountTier.Columns.PromoCampaignHdrID, pHdr.PromoCampaignHdrID);
                discountTier.Where(PromoDiscountTier.Columns.WhichQty, Comparison.LessOrEquals, myOrderLine.Quantity);
                discountTier.OrderByAsc("WhichQty");
                discountTier.Load();
                if (discountTier.Count > 0)
                {
                    decimal discount = (decimal)discountTier[discountTier.Count - 1].Discount;
                    value = (discount / 100) * myOrderLine.Quantity.GetValueOrDefault(0) * myOrderLine.UnitPrice;
                }

                return value;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return -1;
            }
            /*
             * --Complicated version
             * 
            decimal Value;
            if (myOrderLine.Quantity > pHdr.MinQuantity)
            {
                int IndexQty = pHdr.MinQuantity.Value + 1;
                //Pull the DiscountTier
                PromoDiscountTierCollection discountTier = new PromoDiscountTierCollection();
                discountTier.Where(PromoDiscountTier.Columns.PromoCampaignHdrID, pHdr.PromoCampaignHdrID);
                discountTier.OrderByAsc("WhichQty");
                discountTier.Load();

                int k = 0;

                //Pull the discounts
                Value = pHdr.MinQuantity.Value * myOrderLine.UnitPrice;

                while (IndexQty <= myOrderLine.Quantity)
                {
                    //Fetch IndexQty from the collection and check if there is any discount...
                    if (discountTier[k].WhichQty < IndexQty)
                    {
                        k++;
                        if (k == discountTier.Count)
                            break;
                    }
                    else
                    {
                        //Discount Tier WhichQty >= IndexQty
                        Value += myOrderLine.UnitPrice * (1 - ((decimal)discountTier[k].Discount / 100));
                        IndexQty += 1;
                    }
                }
                Value = myOrderLine.Quantity * myOrderLine.UnitPrice - Value;
            }
            else
            {
                Value = 0;
            }
            status = "";
            return Value;*/
        }

        private bool IsValidPromoLineItem
            (OrderDet myOrderLine)
        {
            //Check if item order line is valid for PROMO
            return (!myOrderLine.IsVoided &
                !myOrderLine.IsPromo &
                !myOrderLine.IsFreeOfCharge &
                !myOrderLine.IsPromoFreeOfCharge &
                !myOrderLine.IsExchange &
                !myOrderLine.IsSpecial &
                (myOrderLine.PriceMode == "NORMAL" || myOrderLine.PriceMode == null || myOrderLine.PriceMode == "")
                );
        }

        //Get valid promo from orderDets
        private ViewPromoMasterDetailCollection
            ValidItemGroupPromo(OrderDetCollection OrderDets)
        {
            if (OrderDets == null || OrderDets.Count <= 0)
                return null;

            string ItemNoList = "";
            for (int i = 0; i < OrderDets.Count; i++)
            {
                if (IsValidPromoLineItem(OrderDets[i]))
                    ItemNoList += "'" + OrderDets[i].ItemNo + "',";
            }
            ItemNoList = ItemNoList.TrimEnd(','); //ItemNoList.LastIndexOf(',')-1);
            DataSet ds;
            if (ItemNoList != "")
            {
                ds = SPs.FetchAllPossibleItemGroupPromo
                    (ItemNoList, pos.MembershipApplied(),
                    PointOfSaleInfo.PointOfSaleID).GetDataSet();

                DataRow[] dr;

                ArrayList arToBeDeleted = new ArrayList();
                for (int i = 0; i < OrderDets.Count; i++)
                {
                    if (!OrderDets[i].IsVoided)
                    {
                        //find those whose item is in the order list and the quantity sufficient....
                        dr = ds.Tables[0].Select("ItemNo=" + "'" + OrderDets[i].ItemNo + "' AND UnitQty >" + OrderDets[i].Quantity);
                        for (int j = 0; j < dr.Length; j++)
                            arToBeDeleted.Add(dr[j]["PromoCampaignHdrID"].ToString());
                    }
                }
                int k = ds.Tables[0].Rows.Count - 1;
                while (k >= 0)
                {
                    for (int m = 0; m < arToBeDeleted.Count; m++)
                    {
                        if (ds.Tables[0].Rows[k].RowState != DataRowState.Deleted
                            && arToBeDeleted[m].ToString() == ds.Tables[0].Rows[k]["PromoCampaignHdrID"].ToString()
                            )
                        {
                            ds.Tables[0].Rows[k].Delete();
                        }
                    }
                    k--;
                }
                ViewPromoMasterDetailCollection vr = new ViewPromoMasterDetailCollection();
                ArrayList arAddedList = new ArrayList();
                for (int m = 0; m < ds.Tables[0].Rows.Count; m++)
                {
                    if (ds.Tables[0].Rows[m].RowState != DataRowState.Deleted)
                    {
                        bool Duplicate = false;
                        for (int t = 0; t < arAddedList.Count; t++)
                        {
                            if (arAddedList[t].ToString() == ds.Tables[0].Rows[m]["PromoCampaignHdrID"].ToString())
                            {
                                Duplicate = true;
                                break;
                            }
                        }
                        if (!Duplicate)
                        {
                            vr.AddRange(new ViewPromoMasterDetailCollection().
                                Where(ViewPromoMasterDetail.Columns.PromoCampaignHdrID,
                                int.Parse(ds.Tables[0].Rows[m]["PromoCampaignHdrID"].ToString())).Load());
                            arAddedList.Add(ds.Tables[0].Rows[m]["PromoCampaignHdrID"].ToString());
                        }
                    }
                }
                return vr;
            }
            else
            {
                return null;
            }
        }


        private bool ApplyItemGroupDisount
            (ViewPromoMasterDetail myPromo)
        {
            //Create copy of OrderDet
            OrderDetCollection OrderDets = new OrderDetCollection();
            OrderDets.CopyFrom(pos.FetchUnsavedOrderDet());
            //Sort order det by itemno
            OrderDets.Sort(OrderDet.Columns.ItemNo, true);

            int k = 0;
            int l = 0;

            decimal UnitPromoQty, minUnitPromoQty = 0;
            double SumOfQtyTimesRetailPrice = 0;
            ItemGroupMapCollection ItemsInTheGroup;

            //Pull out the Item Group as specified in the promotion
            ItemsInTheGroup = new ItemGroupMapCollection();
            ItemsInTheGroup.Where(ItemGroupMap.Columns.ItemGroupID, myPromo.ItemGroupID);
            ItemsInTheGroup.OrderByAsc("ItemNo");
            ItemsInTheGroup.Load();

            double ItemGroupPromoAmount, PromoDiscountPercentage;

            ArrayList OrderLineIDs = new ArrayList();
            ArrayList ItemPromoUnitQty = new ArrayList();

            //Go through OrderDets and Item Group and find matches
            while (k < ItemsInTheGroup.Count & l < OrderDets.Count)
            {

                //If the item is found in the group and the quantity fulfill the requirement
                if (
                    ItemsInTheGroup[k].ItemNo == OrderDets[l].ItemNo
                    & ItemsInTheGroup[k].UnitQty <= OrderDets[l].Quantity
                    & !OrderDets[l].IsVoided
                    & !OrderDets[l].IsPromo
                    & !OrderDets[l].IsFreeOfCharge
                    )
                {
                    decimal LineItemQty = OrderDets[l].Quantity.GetValueOrDefault(0);

                    SumOfQtyTimesRetailPrice +=  Convert.ToDouble(LineItemQty) * (double)OrderDets[l].UnitPrice;

                    OrderLineIDs.Add(l); //store the order line as the important one


                    UnitPromoQty = (LineItemQty) / ItemsInTheGroup[k].UnitQty; //take the unit quantity and keep
                    ItemPromoUnitQty.Add(ItemsInTheGroup[k].UnitQty);

                    if (UnitPromoQty < minUnitPromoQty | minUnitPromoQty == 0)
                        minUnitPromoQty = UnitPromoQty;
                    k++; l++;
                }
                else
                {
                    if (String.Compare(ItemsInTheGroup[k].ItemNo, OrderDets[l].ItemNo) >= 0)
                        l++;

                    else if (String.Compare(ItemsInTheGroup[k].ItemNo, OrderDets[l].ItemNo) <= 0)
                        k++;
                }
            }

            //Promo amount....
            //Check membership group mapping....
            //check membership map
            PromoMembershipMapCollection prm = new PromoMembershipMapCollection();
            if (pos.CurrentMember != null)
            {
                prm.Where(PromoMembershipMap.Columns.MembershipGroupID,
                    pos.CurrentMember.MembershipGroupId);
                prm.Where(PromoMembershipMap.Columns.PromoCampaignHdrID,
                    myPromo.PromoCampaignHdrID);
                prm.Where(PromoMembershipMap.Columns.Deleted, false);
                prm.Load();
            }

            if (prm.Count > 0)
            {
                if (prm[0].UseMembershipPrice)
                {
                    ItemGroupPromoAmount = (double)prm[0].MembershipPrice;
                    PromoDiscountPercentage = 0;
                }
                else
                {
                    ItemGroupPromoAmount = 0;
                    PromoDiscountPercentage = (double)prm[0].MembershipDiscount;
                }
            }
            else
            {
                ItemGroupPromoAmount = (double)myPromo.PromoPrice.Value;
                PromoDiscountPercentage = (double)myPromo.PromoDiscount;
            }

            MultipleOrderLineID = new ArrayList();

            double TotalDiscount = 0;
            double tmp = ItemGroupPromoAmount * Convert.ToDouble(minUnitPromoQty);
            //loop through order line that is valid for the promotion
            for (l = 0; l < OrderLineIDs.Count; l++)
            {
                string LineID = OrderDets[(int)OrderLineIDs[l]].OrderDetID;
                if (IsValidPromoLineItem(OrderDets[(int)OrderLineIDs[l]]))
                {
                    MultipleOrderLineID.Add(LineID);
                    decimal LineQty = OrderDets[(int)OrderLineIDs[l]].Quantity.GetValueOrDefault(0);
                    double LinePrice = (double)OrderDets[(int)OrderLineIDs[l]].UnitPrice;
                    double LineDiscountedPriceAmount;

                    if (ItemGroupPromoAmount != 0)
                    {
                        //using price
                        if (LinePrice > 0)
                        {
                            //Calculate line discount accordingly:                             
                            if (tmp > (LinePrice * Convert.ToDouble(LineQty)))
                            {
                                LineDiscountedPriceAmount = LinePrice;
                                tmp = tmp - LinePrice * Convert.ToDouble(LineQty);
                            }
                            else
                            {
                                LineDiscountedPriceAmount = tmp / (Convert.ToDouble(minUnitPromoQty) * (int)ItemPromoUnitQty[l]);
                                tmp = 0;
                            }
                            /*
                            LineDiscountAmount =  (double)((
                                ((((LineQty * LinePrice) /
                                SumOfQtyTimesRetailPrice) * ItemGroupPromoAmount) /
                                (int)ItemPromoUnitQty[l])));                            
                             */
                        }
                        else
                        {
                            LineDiscountedPriceAmount = 0;
                        }
                    }
                    else //using percentage
                    {
                        LineDiscountedPriceAmount = PromoDiscountPercentage;
                    }

                    string status;
                    pos.ChangeOrderLineQuantity(LineID, minUnitPromoQty * (int)ItemPromoUnitQty[l], false, out status);
                    pos.SetOrderLineAsPromo(LineID, true, myPromo.PromoCampaignHdrID, myPromo.PromoCampaignDetID, out status);

                    //cek if using price or discount
                    if (PromoDiscountPercentage > 0)
                    {
                        pos.SetOrderLineToUsePromoPrice(LineID, false);
                        pos.SetOrderLinePromoDiscount(LineID, (double)PromoDiscountPercentage, out status);
                    }
                    else
                    {
                        pos.SetOrderLineToUsePromoPrice(LineID, true);
                        pos.SetOrderLinePromoUnitPrice(LineID, (decimal)LineDiscountedPriceAmount, out status);
                    }


                    //pos.SetOrderLinePromoDiscount(LineID, (double)LineDiscountAmount, out status);                    
                    TotalDiscount += Math.Round((int)ItemPromoUnitQty[l] * LineDiscountedPriceAmount, 2);

                    decimal LeftOverQty = LineQty - (minUnitPromoQty * (int)ItemPromoUnitQty[l]);
                    if (LeftOverQty > 0)
                    {
                        pos.AddLeftOverItemToOrder((new Item("ItemNo", pos.GetLineItemNo(LineID))),
                            LeftOverQty,
                            pos.GetPreferredDiscount(), false, OrderDets[(int)OrderLineIDs[l]].OrderDetDate, out status);
                    }
                }
            }

            /**/
            if (ItemGroupPromoAmount != TotalDiscount)
            {
                //Change the last discount to be 
                //additional with the difference
                string status;
                double diff = ItemGroupPromoAmount - TotalDiscount;
                string LastLineID = MultipleOrderLineID[MultipleOrderLineID.Count - 1].ToString();
                decimal lineUnitPrice = pos.GetLinePromoUnitPrice(LastLineID, out status);
                pos.SetOrderLinePromoUnitPrice(LastLineID, (lineUnitPrice + (decimal)diff), out status);
            }

            return true;
        }

        //Get valid promo from orderDets
        public ViewPromoMasterDetail
            ValidBuyXAtThePriceOfYPromo
            (OrderDetCollection OrderDets, out ArrayList FOCLineID, out ArrayList PromoLineID,
            out decimal FOCPromoQty, out decimal MultiplierQty)
        {
            int minOrderDetIndex = -1;
            decimal discountValue = 0;
            FOCPromoQty = 0;
            int tmpFOC;
            MultiplierQty = 0;
            ArrayList tmpFOCLineIDs;
            FOCLineID = null;
            PromoLineID = null;
            if (OrderDets == null || OrderDets.Count <= 0)
                return null;

            string ItemNoList = "";
            //Get the list of valid promo line item
            //put them in the list of item no
            for (int i = 0; i < OrderDets.Count; i++)
            {
                if (IsValidPromoLineItem(OrderDets[i]))
                    ItemNoList += "'" + OrderDets[i].ItemNo + "',";
            }
            ItemNoList = ItemNoList.TrimEnd(','); //ItemNoList.LastIndexOf(',')-1);
            DataSet ds;

            //pull out the valid promo with SPs
            if (ItemNoList != "")
            {
                ds = SPs.FetchAllPossibleBuyXatPriceOfYPromo
                    (ItemNoList, pos.MembershipApplied(),
                    PointOfSaleInfo.PointOfSaleID).GetDataSet();

                DataTable dt = ds.Tables[0];

                if (dt.Rows.Count == 0)
                    return null;

                DataTable orderdt = OrderDets.ToDataTable();


                ArrayList ar = new ArrayList();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ar.Add(dt.Rows[i][0].ToString());
                }

                //For every promo
                ViewPromoMasterDetailCollection promoCol = new ViewPromoMasterDetailCollection();
                promoCol.Where(PromoCampaignHdr.Columns.PromoCampaignHdrID, Comparison.In, ar);
                promoCol.Load();

                if (promoCol.Count == 0)
                    return null;

                //for every promo
                for (int i = 0; i < promoCol.Count; i++)
                {
                    //load the itemgroupmap......
                    ItemGroupMapCollection mpr = new ItemGroupMapCollection();
                    mpr.Where(ItemGroupMap.Columns.ItemGroupID, promoCol[i].ItemGroupID);
                    mpr.OrderByAsc(ItemGroupMap.Columns.ItemNo);
                    mpr.Load();

                    string itemList = "";
                    for (int j = 0; j < mpr.Count; j++)
                    {
                        itemList += "'" + mpr[j].ItemNo + "',";
                    }
                    itemList.TrimEnd(',');

                    DataRow[] dr = orderdt.Select("ItemNo In (" + itemList + ") AND IsVoided=false AND IsPromo=false", "UnitPrice asc");

                    //scan through dr to find total item count.......
                    int TotalCount = 0;
                    for (int j = 0; j < dr.Length; j++)
                    {
                        TotalCount += int.Parse(dr[j]["Quantity"].ToString());
                    }

                    //calculate total discount and minimum
                    if (promoCol[i].MinQuantity <= TotalCount) //is it eligible?
                    {
                        //valid Promo!
                        int multiplier = ((int)Math.Floor((decimal)TotalCount / promoCol[i].MinQuantity));
                        int FOCQty = multiplier * promoCol[i].FreeQty.Value; //actual free quantity given

                        tmpFOC = FOCQty;
                        decimal currentDiscountValue = 0.0M;

                        //mark the Promo lines
                        int PromoQty = multiplier * promoCol[i].MinQuantity;
                        int TotalQty = 0;
                        ArrayList tmpPromoID = new ArrayList();
                        for (int j = 0; j < dr.Length; j++)
                        {
                            int currentQty = int.Parse(dr[j]["Quantity"].ToString());
                            /*
                            if (PromoQty >= currentQty)
                            {
                                
                            }
                            else if (PromoQty < currentQty) //Quantity is in sufficient
                            {
                                
                            }*/
                            tmpPromoID.Add(dr[j]["OrderDetID"].ToString());
                            PromoQty -= currentQty;
                            if (PromoQty <= 0)
                            {
                                break;
                            }
                        }

                        //assign total FOC value.....
                        //calculate total top up value.... 
                        tmpFOCLineIDs = new ArrayList();


                        //For every row that is part of the promo...
                        //from the top all the way until 
                        for (int j = 0; j < dr.Length; j++)
                        {
                            int currentQty = int.Parse(dr[j]["Quantity"].ToString());
                            tmpFOCLineIDs.Add(dr[j]["OrderDetID"].ToString()); //need to split later
                            if (FOCQty >= currentQty)
                            {
                                currentDiscountValue += currentQty * decimal.Parse(dr[j]["UnitPrice"].ToString());
                                //tmpFOCLineIDs.Add(dr[j]["OrderDetID"].ToString());
                                FOCQty -= currentQty;
                            }
                            else
                            {
                                currentDiscountValue += FOCQty * decimal.Parse(dr[j]["UnitPrice"].ToString());
                                //tmpFOCLineIDs.Add(dr[j]["OrderDetID"].ToString());
                                FOCQty = 0;
                            }

                            if (FOCQty == 0)
                            {
                                if (discountValue <= currentDiscountValue)
                                {
                                    discountValue = currentDiscountValue;
                                    minOrderDetIndex = i;
                                    FOCLineID = tmpFOCLineIDs;
                                    PromoLineID = tmpPromoID;
                                    FOCPromoQty = tmpFOC;
                                    MultiplierQty = multiplier;
                                }
                                break;
                            }
                        }
                    }
                }
                if (minOrderDetIndex != -1)
                    return promoCol[minOrderDetIndex];
            }
            return null;
        }

        private bool ApplyBuyXAtThePriceOfYDisount()
        {
            ArrayList FOCLineID, PromoLineID;
            decimal FOCQty, MultiplierQty, tmpQty, finalTmpQty;

            //Get the list of the stuff
            ViewPromoMasterDetail myPromo =
                ValidBuyXAtThePriceOfYPromo
                (pos.FetchUnsavedOrderDet(), out FOCLineID, out PromoLineID, out FOCQty, out MultiplierQty);

            string status;

            if (PromoLineID == null || PromoLineID.Count == 0 || FOCLineID == null || FOCLineID.Count == 0)
            {
                return false;
            }

            tmpQty = MultiplierQty * myPromo.MinQuantity;
            //Split & Mark Promo Line ID
            for (int i = 0; i < PromoLineID.Count - 1; i++)
            {

                //mark as promo
                pos.SetOrderLineAsPromo
                    (PromoLineID[i].ToString(), true,
                    myPromo.PromoCampaignHdrID,
                    myPromo.PromoCampaignDetID, out status);
                pos.SetOrderLinePromoAmountToUseDefaultUnitPrice(PromoLineID[i].ToString(), out status);


                //split if qty is less than multiplier qty
                decimal tmpLineQty = pos.GetLineQuantity(PromoLineID[i].ToString(), out status);
                if (tmpLineQty > MultiplierQty)
                {
                    pos.ChangeOrderLineQuantity(PromoLineID[i].ToString(), MultiplierQty, false, out status);
                    pos.AddLeftOverItemToOrder(new Item(pos.GetLineItemNo(PromoLineID[i].ToString())), tmpLineQty - MultiplierQty,
                        pos.GetPreferredDiscount(), false, pos.GetLineOrderDetDate(PromoLineID[PromoLineID.Count - 1].ToString(), out status), out status);
                }
                tmpQty -= tmpLineQty;


            }

            //check final one if need splitting....
            finalTmpQty = pos.GetLineQuantity(PromoLineID[PromoLineID.Count - 1].ToString(), out status);
            pos.SetOrderLineAsPromo(PromoLineID[PromoLineID.Count - 1].ToString(), true,
                 myPromo.PromoCampaignHdrID,
                 myPromo.PromoCampaignDetID, out status);
            pos.SetOrderLinePromoAmountToUseDefaultUnitPrice(PromoLineID[PromoLineID.Count - 1].ToString(), out status);

            if (finalTmpQty > tmpQty) //if need splitting....
            {

                if (tmpQty > MultiplierQty * myPromo.MinQuantity)
                {
                    tmpQty = MultiplierQty * myPromo.MinQuantity;
                }
                //Change total quantity to be floor (multiplier x Minimum qty)                
                pos.ChangeOrderLineQuantity(PromoLineID[PromoLineID.Count - 1].ToString(), tmpQty, false, out status);

                //Create a new line item to put in the left over quantity
                pos.AddLeftOverItemToOrder(
                    new Item(pos.GetLineItemNo(PromoLineID[PromoLineID.Count - 1].ToString())),
                    (finalTmpQty - tmpQty), pos.GetPreferredDiscount(), false,
                    pos.GetLineOrderDetDate(PromoLineID[PromoLineID.Count - 1].ToString(), out status), out status);
            }

            //mark extra ones as FOC            
            tmpQty = FOCQty;
            //for every item, set as FOC
            for (int i = 0; i < FOCLineID.Count - 1; i++)
            {
                //pos.SetOrderLineAsPromo(FOCLineID[i].ToString(), true, myPromo.PromoCampaignHdrID,
                //    myPromo.PromoCampaignDetID, out status);
                //pos.SetOrderLinePromoAmountToUseDefaultUnitPrice(FOCLineID[i].ToString(), out status);
                pos.SetOrderLinePromoDiscount(FOCLineID[i].ToString(), 100.0, out status);
                tmpQty -= pos.GetLineQuantity(FOCLineID[i].ToString(), out status);
            }


            //for the last one, check if split is needed
            finalTmpQty = pos.GetLineQuantity(FOCLineID[FOCLineID.Count - 1].ToString(), out status);

            if (tmpQty <= finalTmpQty)
            {
                //Change Qty
                //pos.SetOrderLineAsPromo(FOCLineID[FOCLineID.Count - 1].ToString(), true, myPromo.PromoCampaignHdrID, myPromo.PromoCampaignDetID, out status);
                //pos.SetOrderLinePromoAmountToUseDefaultUnitPrice(FOCLineID[FOCLineID.Count - 1].ToString(), out status);
                //add a new promo line item with the remaining Qty
                if (tmpQty != finalTmpQty)
                {
                    pos.ChangeOrderLineQuantity(FOCLineID[FOCLineID.Count - 1].ToString(), tmpQty, false, out status);
                    pos.SetOrderLinePromoDiscount(FOCLineID[FOCLineID.Count - 1].ToString(), 100.0, out status);
                    string newID = pos.AddNewPromoLineItem(new Item(pos.GetLineItemNo(FOCLineID[FOCLineID.Count - 1].ToString())),
                        finalTmpQty - tmpQty, 0, pos.GetLineOrderDetDate(FOCLineID[FOCLineID.Count - 1].ToString(), out status), myPromo, out status);
                    pos.SetOrderLinePromoAmountToUseDefaultUnitPrice(newID, out status);

                }
                else
                {
                    pos.ChangeOrderLineQuantity(FOCLineID[FOCLineID.Count - 1].ToString(), tmpQty, false, out status);
                    pos.SetOrderLinePromoDiscount(FOCLineID[FOCLineID.Count - 1].ToString(), 100.0, out status);
                }
            }

            return true;
        }

        #region "OLD CODE"

        #region "Calculate Promo"
        /*
            #region "Utility"

        private static void SortDataTable(DataTable dt, string sort)
        {
            DataTable newDT = dt.Clone();
            int rowCount = dt.Rows.Count;

            DataRow[] foundRows = dt.Select(null, sort); // Sort with Column name 
            for (int i = 0; i < rowCount; i++)
            {
                object[] arr = new object[dt.Columns.Count];
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    arr[j] = foundRows[i][j];
                }
                DataRow data_row = newDT.NewRow();
                data_row.ItemArray = arr;
                newDT.Rows.Add(data_row);
            }

            //clear the incoming dt 
            dt.Rows.Clear();

            for (int i = 0; i < newDT.Rows.Count; i++)
            {
                object[] arr = new object[dt.Columns.Count];
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    arr[j] = newDT.Rows[i][j];
                }

                DataRow data_row = dt.NewRow();
                data_row.ItemArray = arr;
                dt.Rows.Add(data_row);
            }

        }
                private bool IsValidPromoLineItem(DataRow dr, out bool IsVoided, out bool IsPromo, out bool IsFreeOfCharge)
                {
                    //Check if item order line is valid for PROMO
                    if (bool.TryParse(dr["IsVoided"].ToString(), out IsVoided) &
                        !IsVoided &
                        bool.TryParse(dr["IsPromo"].ToString(), out IsPromo) &
                        !IsPromo &
                        bool.TryParse(dr["IsFreeOfCharge"].ToString(), out IsFreeOfCharge) &
                        !IsFreeOfCharge
                        )
                    {
                        return true;
                    }
                    else
                    {
                        //IsPromo = false;
                        //IsFreeOfCharge = false;
                        return false;
                    }
                }
            #endregion
        
            private void CalculateMaxPromoValue(out string status)
            {
                CurrentPromoHdrID = -1;
                OrderLineID = "";
                MaxPromoHdrID = -1;
                MaxValue = 0;
                status = "";    

                //Clear current order from Promo
             
                //----------CHECK ALL POSSIBLE PROMO AND GET THE MAX VALUE AND PROMOHDRID-------------------------

                //Buy 1 Get 1 Free
                CalculateValueForBuyXGetYFree(out status);

                //Discount By Category
                CalculateValueDiscountByCategory(out status);

                //Discount By Category
                CalculateValueDiscountByItem(out status);

                //Discount by Item Group
                CalculateValueItemGroupDisount(out status);

                CalculateValueForMultiTierDiscount(out status);
            }


            private bool CalculateValueForBuyXGetYFree(out string status)
            {
                Query qry;            
                status = "";
                bool PromoFound = false;
                //Check buy X get Y free
                qry = new Query("ViewPromoMasterDetail");

                DataSet ds = qry.WHERE(ViewPromoMasterDetail.Columns.DateFrom, Comparison.LessOrEquals, DateTime.Now).AND(ViewPromoMasterDetail.Columns.DateTo, Comparison.GreaterOrEquals, DateTime.Now).AND(ViewPromoMasterDetail.Columns.CampaignType, PromotionAdminController.BuyXGetYFree).ExecuteDataSet();
                DataTable dtPromo = ds.Tables[0];
                DataRow[] drPromo;

                int UnitQty;
                int UnitFreeQty;
                string FreeItemNo;
                int CurrentItemQty;
                int TotalFreeItem;
                decimal Value;
                string tmpOrderLineID;
                DataTable dtOrderLine;
                bool IsVoided, IsPromo, IsFreeOfCharge;

                dtOrderLine = pos.FetchUnSavedOrderItems(out status);

                for (int j = 0; j < dtOrderLine.Rows.Count; j++)
                {
                    if (IsValidPromoLineItem(dtOrderLine.Rows[j], out IsVoided,out IsPromo, out IsFreeOfCharge))
                    {
                        drPromo = dtPromo.Select("ItemNo = '" + dtOrderLine.Rows[j]["ItemNo"].ToString() + "'");

                        //There can be more than 1 promo for the same item...
                        for (int i = 0; i < drPromo.Length; i++)
                        {
                            CalculateBuy1Get1FreeByItem(drPromo, dtOrderLine, j, i, out UnitQty, out UnitFreeQty, out FreeItemNo, out CurrentItemQty, out TotalFreeItem, out Value, out tmpOrderLineID);


                            //compare with current valuation....
                            if (Value > MaxValue)
                            {
                                MaxValue = Value;
                                MaxPromoHdrID = int.Parse(drPromo[i]["PromoCampaignHdrID"].ToString());
                                OrderLineID = tmpOrderLineID;
                                PromoFound = true;
                                PromoType = PromotionAdminController.BuyXGetYFree;
                            }
                        }
                    }
                }
                return PromoFound;
            }

        private static void CalculateBuy1Get1FreeByItem
            (DataRow[] drPromo, DataTable dtOrderLine, 
            int j, int i, out int UnitQty, out int UnitFreeQty, 
            out string FreeItemNo, out int CurrentItemQty, 
            out int TotalFreeItem, out decimal Value, out string tmpOrderLineID)
        {
            //get free quantity
            UnitQty = (int)drPromo[i]["UnitQty"];
            UnitFreeQty = (int)drPromo[i]["FreeQty"];
            FreeItemNo = drPromo[i]["FreeItemNo"].ToString();

            CurrentItemQty = int.Parse(dtOrderLine.Rows[j]["Quantity"].ToString());

            //calculate total free items
            TotalFreeItem = ((int)Math.Floor((double)(CurrentItemQty / UnitQty))) * UnitFreeQty;
            tmpOrderLineID = dtOrderLine.Rows[j]["ID"].ToString();
            //calculate value, promohdrid, promodetid
            Value = TotalFreeItem * (new Item("ItemNo", FreeItemNo)).RetailPrice;
        }

        
            private bool CalculateValueDiscountByCategory(out string status)
            {
                //Fetch from Promo Campaign, what are the discounts that belongs to this category
                Query qry;
                status = ""; 
                decimal Value = 0;
                string tmpOrderLineID = "";
                bool PromoFound = false;
                
                qry = new Query("ViewPromoMasterDetail");

                DataSet ds = qry.WHERE(ViewPromoMasterDetail.Columns.DateFrom, Comparison.LessOrEquals, DateTime.Now).AND(ViewPromoMasterDetail.Columns.DateTo, Comparison.GreaterOrEquals, DateTime.Now).AND(ViewPromoMasterDetail.Columns.CampaignType, PromotionAdminController.DiscountByCategory).ExecuteDataSet();
                DataTable dtPromo = ds.Tables[0];
                DataRow[] drPromo;

                DataTable dtOrderLine;
                bool IsVoided, IsPromo, IsFreeOfCharge;

                decimal PromoDiscount, UnitPrice, Quantity;
                     
                dtOrderLine = pos.FetchUnSavedOrderItems(out status);

                for (int j = 0; j < dtOrderLine.Rows.Count; j++)
                {
                    if (IsValidPromoLineItem(dtOrderLine.Rows[j], out IsVoided, out IsPromo, out IsFreeOfCharge))
                    {
                        
                        //Find Promo with the category of the current item....
                        drPromo = dtPromo.Select("CategoryName = '" + dtOrderLine.Rows[j]["CategoryName"].ToString() + "'");           
                        
                        for (int i = 0; i < drPromo.Length; i++)
                        {

                            if (decimal.TryParse(drPromo[i]["PromoDiscount"].ToString(), out PromoDiscount) 
                                & decimal.TryParse(dtOrderLine.Rows[j]["Quantity"].ToString(), out Quantity)
                                & decimal.TryParse(dtOrderLine.Rows[j]["Price"].ToString(), out UnitPrice)
                                )
                            {
                                //The value will be how much discount you will get
                                Value = (PromoDiscount / 100) * Quantity * UnitPrice;
                                tmpOrderLineID = dtOrderLine.Rows[j]["ID"].ToString();
                            }

                            //compare with current valuation....
                            if (Value > MaxValue)
                            {
                                MaxValue = Value;
                                MaxPromoHdrID = int.Parse(drPromo[i]["PromoCampaignHdrID"].ToString());
                                OrderLineID = tmpOrderLineID;
                                PromoFound = true;
                                PromoType = PromotionAdminController.DiscountByCategory;
                            }
                        }
                    }
                }
                return PromoFound;
            }
     
            private bool CalculateValueDiscountByItem(out string status)
        {
            //Fetch from Promo Campaign, what are the discounts that belongs to this category
            Query qry;
            status = "";
            decimal Value = 0;
            string tmpOrderLineID = "";
            bool PromoFound = false;

            qry = new Query("ViewPromoMasterDetail");

            DataSet ds = qry.WHERE(ViewPromoMasterDetail.Columns.DateFrom, Comparison.LessOrEquals, DateTime.Now).AND(ViewPromoMasterDetail.Columns.DateTo, Comparison.GreaterOrEquals, DateTime.Now).AND(ViewPromoMasterDetail.Columns.CampaignType, PromotionAdminController.DiscountByItem).ExecuteDataSet();
            DataTable dtPromo = ds.Tables[0];
            DataRow[] drPromo;

            DataTable dtOrderLine;
            bool IsVoided, IsPromo, IsFreeOfCharge;

            decimal PromoDiscount, UnitPrice, Quantity;

            dtOrderLine = pos.FetchUnSavedOrderItems(out status);

            for (int j = 0; j < dtOrderLine.Rows.Count; j++)
            {
                if (IsValidPromoLineItem(dtOrderLine.Rows[j], out IsVoided, out IsPromo, out IsFreeOfCharge))
                {

                    //Find Promo with the category of the current item....
                    drPromo = dtPromo.Select("ItemNo = '" + dtOrderLine.Rows[j]["ItemNo"].ToString() + "'");

                    for (int i = 0; i < drPromo.Length; i++)
                    {

                        if (decimal.TryParse(drPromo[i]["PromoDiscount"].ToString(), out PromoDiscount)
                            & decimal.TryParse(dtOrderLine.Rows[j]["Quantity"].ToString(), out Quantity)
                            & decimal.TryParse(dtOrderLine.Rows[j]["Price"].ToString(), out UnitPrice)
                            )
                        {
                            //The value will be how much discount you will get
                            Value = (PromoDiscount / 100) * Quantity * UnitPrice;
                            tmpOrderLineID = dtOrderLine.Rows[j]["ID"].ToString();
                        }

                        //compare with current valuation....
                        if (Value > MaxValue)
                        {
                            MaxValue = Value;
                            MaxPromoHdrID = int.Parse(drPromo[i]["PromoCampaignHdrID"].ToString());
                            OrderLineID = tmpOrderLineID;
                            PromoFound = true;
                            PromoType = PromotionAdminController.DiscountByCategory;
                        }
                    }
                }
            }
            return PromoFound;
        }

        private bool ApplyItemGroupDisount()
        {
            Query qry;

            qry = new Query("ViewPromoMasterDetail");
            string status;

            DataSet dsPromo =
                qry.WHERE(ViewPromoMasterDetail.Columns.PromoCampaignHdrID, MaxPromoHdrID).ExecuteDataSet();
            DataTable dtPromo = dsPromo.Tables[0];
            DataRow[] drPromo;

            DataTable dtOrderLine;
            dtOrderLine = pos.FetchUnSavedOrderItems(out status);
            SortDataTable(dtOrderLine, "ItemNo");

            drPromo = dtPromo.Select();
            int k = 0;
            int l = 0;

            int UnitPromoQty, minUnitPromoQty = 0;
            decimal SumOfQtyTimesRetailPrice = 0;
            ItemGroupMapCollection ItemsInTheGroup;

            ItemsInTheGroup = new ItemGroupMapCollection();
            int ItemGroupID = int.Parse(drPromo[0]["ItemGroupID"].ToString());
            ItemsInTheGroup.Where(ItemGroupMap.Columns.ItemGroupID, ItemGroupID);
            ItemsInTheGroup.OrderByAsc("ItemNo");
            ItemsInTheGroup.Load();
            decimal ItemGroupPromoAmount;
            ArrayList OrderLineIDs = new ArrayList();
            ArrayList ItemPromoUnitQty = new ArrayList();
            //Go through both sorted table and find matches
            while (k < ItemsInTheGroup.Count & l < dtOrderLine.Rows.Count)
            {
                if (
                    ItemsInTheGroup[k].ItemNo == dtOrderLine.Rows[l]["ItemNo"].ToString()
                    & ItemsInTheGroup[k].UnitQty <= int.Parse(dtOrderLine.Rows[l]["Quantity"].ToString())
                    & !bool.Parse(dtOrderLine.Rows[l]["IsVoided"].ToString())
                    )
                {
                    int LineItemQty = int.Parse(dtOrderLine.Rows[l]["Quantity"].ToString());

                    SumOfQtyTimesRetailPrice += LineItemQty * decimal.Parse((dtOrderLine.Rows[l]["Price"].ToString()));
                    OrderLineIDs.Add(l);
                    UnitPromoQty = LineItemQty / ItemsInTheGroup[k].UnitQty;
                    ItemPromoUnitQty.Add(ItemsInTheGroup[k].UnitQty);
                    if (UnitPromoQty < minUnitPromoQty | minUnitPromoQty == 0)
                        minUnitPromoQty = UnitPromoQty;
                    k++; l++;
                }
                else
                {
                    if (String.Compare(ItemsInTheGroup[k].ItemNo, dtOrderLine.Rows[l]["ItemNo"].ToString()) >= 0)
                        l++;

                    else if (String.Compare(ItemsInTheGroup[k].ItemNo, dtOrderLine.Rows[l]["ItemNo"].ToString()) <= 0)
                        k++;
                }
            }
            decimal.TryParse(drPromo[0]["PromoPrice"].ToString(), out ItemGroupPromoAmount);
            MultipleOrderLineID = new ArrayList();
            for (l = 0; l < OrderLineIDs.Count; l++)
            {
                string LineID = dtOrderLine.Rows[(int)OrderLineIDs[l]]["ID"].ToString();
                MultipleOrderLineID.Add(LineID);
                int LineQty = int.Parse(dtOrderLine.Rows[(int)OrderLineIDs[l]]["Quantity"].ToString());
                decimal LinePrice = decimal.Parse(dtOrderLine.Rows[(int)OrderLineIDs[l]]["Price"].ToString());
                double LineDiscountAmount;
                if (ItemGroupPromoAmount != 0)
                    LineDiscountAmount = (double)((LinePrice - ((((LineQty * LinePrice) / SumOfQtyTimesRetailPrice) * ItemGroupPromoAmount) / (int)ItemPromoUnitQty[l])) / LinePrice) * 100;
                else
                    double.TryParse(drPromo[0]["PromoDiscount"].ToString(), out LineDiscountAmount);

                pos.ChangeOrderLineQuantity(LineID, minUnitPromoQty * (int)ItemPromoUnitQty[l], false, out status);
                pos.SetOrderLineAsPromo(LineID, true, out status);
                pos.SetOrderLinePromoDiscount(LineID, LineDiscountAmount, out status);

                int LeftOverQty = LineQty - (minUnitPromoQty * (int)ItemPromoUnitQty[l]);
                if (LeftOverQty > 0)
                {
                    pos.AddItemToOrder((new Item("ItemNo", pos.GetLineItemNo(LineID))),
                        LeftOverQty,
                        pos.GetPreferredDiscount(), false, out status);
                }

            }

            CurrentPromoHdrID = MaxPromoHdrID;

            return true;
        }


        /*
        private bool CalculateValueForMultiTierDiscount(out string status)
        {
            Query qry;
            status = "";
            bool PromoFound = false;
            //Check buy X get Y free
            qry = new Query("ViewPromoMasterDetail");

            DataSet ds = qry.WHERE(ViewPromoMasterDetail.Columns.DateFrom, Comparison.LessOrEquals, DateTime.Now).AND(ViewPromoMasterDetail.Columns.DateTo, Comparison.GreaterOrEquals, DateTime.Now).AND(ViewPromoMasterDetail.Columns.CampaignType, PromotionAdminController.MultiTierDiscount).ExecuteDataSet();
            DataTable dtPromo = ds.Tables[0];
            DataRow[] drPromo;

            int MinQuantity, PromoCampaignHdrID;
            decimal Value;
            string tmpOrderLineID;
            DataTable dtOrderLine;
            bool IsVoided, IsPromo, IsFreeOfCharge;

            dtOrderLine = pos.FetchUnSavedOrderItems(out status);

            for (int j = 0; j < dtOrderLine.Rows.Count; j++)
            {
                if (IsValidPromoLineItem(dtOrderLine.Rows[j], out IsVoided, out IsPromo, out IsFreeOfCharge))
                {
                    drPromo = dtPromo.Select("ItemNo = '" + dtOrderLine.Rows[j]["ItemNo"].ToString() + "'");

                    //There can be more than 1 promo for the same item...
                    for (int i = 0; i < drPromo.Length; i++)
                    {
                        //get free quantity
                        MinQuantity = (int)drPromo[i]["MinQuantity"];
                        int LineQty = int.Parse(dtOrderLine.Rows[j]["Quantity"].ToString());
                        decimal LinePrice = decimal.Parse(dtOrderLine.Rows[j]["Price"].ToString());

                        if (LineQty > MinQuantity)
                        {
                            int IndexQty = MinQuantity + 1;

                            PromoCampaignHdrID = (int)drPromo[i]["PromoCampaignHdrID"];

                            //Pull the DiscountTier
                            PromoDiscountTierCollection discountTier = new PromoDiscountTierCollection();
                            discountTier.Where(PromoDiscountTier.Columns.PromoCampaignHdrID, PromoCampaignHdrID);
                            discountTier.OrderByAsc("WhichQty");
                            discountTier.Load();

                            int k = 0;
                            
                            //Pull the discounts
                            Value = MinQuantity * LinePrice;

                            while (IndexQty <= LineQty)
                            {
                                //Fetch IndexQty from the collection and check if there is any discount...
                                if (discountTier[k].WhichQty < IndexQty)
                                {
                                    k++;
                                    if (k == discountTier.Count)
                                        break;
                                }
                                else
                                {
                                    //Discount Tier WhichQty >= IndexQty
                                    Value += LinePrice * (1-((decimal)discountTier[k].Discount/100));
                                    IndexQty += 1;
                                }                                                                
                            }
                            Value = LineQty * LinePrice - Value;
                            //Line ID
                            tmpOrderLineID = dtOrderLine.Rows[j]["ID"].ToString();
                            //compare with current valuation....
                            if (Value > MaxValue)
                            {
                                MaxValue = Value;
                                MaxPromoHdrID = int.Parse(drPromo[i]["PromoCampaignHdrID"].ToString());
                                OrderLineID = tmpOrderLineID;
                                PromoFound = true;
                                PromoType = PromotionAdminController.MultiTierDiscount;
                            }
                        }
                    }
                }
            }
            return PromoFound;
        }
        #endregion

        #region "APPLY PROMO"
            public void ApplyPromo()
            {
                string status  = "";
                
                CalculateMaxPromoValue(out status);

                if (MaxPromoHdrID > 0)
                {
                    
                    switch (PromoType)
                    {
                        case PromotionAdminController.BuyXGetYFree:
                            ApplyBuyXGetYFree();
                            break;
                        case PromotionAdminController.DiscountByCategory:
                            ApplyDiscountByCategory();
                            break;
                        case PromotionAdminController.DiscountByItem:
                            ApplyDiscountByItem();
                            break;
                        case PromotionAdminController.ItemGroupPriceDiscount:
                            ApplyItemGroupDisount();
                            break;
                        case PromotionAdminController.MultiTierDiscount:
                            ApplyMultiTierDiscount();
                            break;
                    }
                }
            }       
            private bool ApplyBuyXGetYFree() 
               {
                   Query qry;
                   //Check buy X get Y free
                   qry = new Query("ViewPromoMasterDetail");
                   string status;
                   int UnitQty;
                   int UnitFreeQty;
                   string FreeItemNo;
                   int CurrentItemQty = pos.GetLineQuantity(OrderLineID, out status);
                   int TotalFreeItem;

                   DataSet ds =
                       qry.WHERE(ViewPromoMasterDetail.Columns.PromoCampaignHdrID, MaxPromoHdrID).ExecuteDataSet();
                   DataTable dtPromo = ds.Tables[0];
                   DataRow[] dr;

                   dr = dtPromo.Select();
                   //get free quantity
                   UnitQty = (int)dr[0]["UnitQty"];
                   UnitFreeQty = (int)dr[0]["FreeQty"];
                   FreeItemNo = dr[0]["FreeItemNo"].ToString();

                                  
                   //calculate total free items
                   TotalFreeItem = ((int)Math.Floor((double)(CurrentItemQty / UnitQty))) * UnitFreeQty;               
                   
                   //add a new orderline with FOC set = 1               
                   pos.AddFOCPromoItem((new Item("ItemNo", FreeItemNo)), TotalFreeItem, DateTime.Now, out status);               
                   pos.ChangeOrderLineQuantity(OrderLineID,((int)Math.Floor((double)(CurrentItemQty / UnitQty))) * UnitQty ,false, out status);
                   pos.SetOrderLineAsPromo(OrderLineID, true, out status);
                   //Count left overs.... and create a new item line
                   int LeftOverQty = CurrentItemQty % UnitQty;
                   if (LeftOverQty > 0)
                       pos.AddItemToOrder((new Item("ItemNo", pos.GetLineItemNo(OrderLineID))), LeftOverQty, pos.GetPreferredDiscount(), false, out status);
                   
                   CurrentPromoHdrID = MaxPromoHdrID;

                   return true;
               }
  
            


            private bool ApplyDiscountByCategory()
            {
                Query qry;
                //Check buy X get Y free
                qry = new Query("ViewPromoMasterDetail");
                string status;

                DataSet dsPromo =
                    qry.WHERE(ViewPromoMasterDetail.Columns.PromoCampaignHdrID, MaxPromoHdrID).ExecuteDataSet();
                DataTable dtPromo = dsPromo.Tables[0];
                DataRow[] drPromo;

                drPromo = dtPromo.Select();
                
                double Discount = double.Parse(drPromo[0]["PromoDiscount"].ToString());

                pos.SetOrderLineAsPromo(OrderLineID, true, out status);
                pos.SetOrderLinePromoDiscount(OrderLineID, Discount, out status);
                
                CurrentPromoHdrID = MaxPromoHdrID;

                return true;
            }
            private bool ApplyDiscountByItem()
            {
                Query qry;
                //Check buy X get Y free
                qry = new Query("ViewPromoMasterDetail");
                string status;

                DataSet dsPromo =
                    qry.WHERE(ViewPromoMasterDetail.Columns.PromoCampaignHdrID, MaxPromoHdrID).ExecuteDataSet();
                DataTable dtPromo = dsPromo.Tables[0];
                DataRow[] drPromo;

                drPromo = dtPromo.Select();

                double Discount = double.Parse(drPromo[0]["PromoDiscount"].ToString());

                pos.SetOrderLineAsPromo(OrderLineID, true, out status);
                pos.SetOrderLinePromoDiscount(OrderLineID, Discount, out status);

                CurrentPromoHdrID = MaxPromoHdrID;

                return true;
            }
          
            private bool ApplyMultiTierDiscount()
            {
                Query qry;
                string status;                
                int MinQuantity;
                qry = new Query("ViewPromoMasterDetail");

                DataSet ds = qry.WHERE(ViewPromoMasterDetail.Columns.PromoCampaignHdrID, MaxPromoHdrID).ExecuteDataSet();
                DataTable dtPromo = ds.Tables[0];
                DataRow[] drPromo;
                
                decimal TotalAmtAfterPromo;                                      

                //dtOrderLine = pos.FetchUnSavedOrderItems(out status);
                //DataRow[] drOrderLine = dtOrderLine.Select("ID = ");
           
                drPromo = dtPromo.Select();


                //get free quantity
                MinQuantity = (int)drPromo[0]["MinQuantity"];
                int LineQty = pos.GetLineQuantity(OrderLineID, out status);
                decimal LinePrice = pos.GetLineUnitPrice(OrderLineID, out status); 

                if (LineQty > MinQuantity)
                {
                    int IndexQty = MinQuantity + 1;

                    //PromoCampaignHdrID = (int)drPromo[0]["PromoCampaignHdrID"];

                    //Pull the DiscountTier
                    PromoDiscountTierCollection discountTier = new PromoDiscountTierCollection();
                    discountTier.Where(PromoDiscountTier.Columns.PromoCampaignHdrID, MaxPromoHdrID);
                    discountTier.OrderByAsc("WhichQty");
                    discountTier.Load();

                    int k = 0;

                    //Pull the discounts
                    TotalAmtAfterPromo = MinQuantity * LinePrice;

                    while (IndexQty <= LineQty)
                    {
                        //Fetch IndexQty from the collection and check if there is any discount...
                        if (discountTier[k].WhichQty < IndexQty)
                        {
                            k++;
                            if (k == discountTier.Count)
                                break;
                        }
                        else
                        {
                            //Discount Tier WhichQty >= IndexQty
                            TotalAmtAfterPromo += LinePrice * (1 - ((decimal)discountTier[k].Discount / 100));
                            IndexQty += 1;
                        }
                    }

                    double discount = (double)(((LineQty * LinePrice - TotalAmtAfterPromo) / (LinePrice * LineQty))*100);
                    pos.SetOrderLineAsPromo(OrderLineID, true, out status);
                    pos.SetOrderLinePromoDiscount(OrderLineID, discount, out status);

                    CurrentPromoHdrID = MaxPromoHdrID;

                    return true;
                }
                else
                {
                    return false;
                }
            }
        */
        #endregion

        #region "Undo PROMO"
        /*
            public void UndoCurrentPromo()
            {
                if (CurrentPromoHdrID != -1)
                {
                    switch (PromoType) 
                    {
                        case PromotionAdminController.BuyXGetYFree:
                            UndoBuyXGetYFree();
                            break;
                        case PromotionAdminController.DiscountByCategory:
                            UndoDiscountByCategory();
                            break;
                        case PromotionAdminController.DiscountByItem:
                            UndoDiscountByItem();
                            break;
                        case PromotionAdminController.ItemGroupPriceDiscount:
                            UndoItemGroupPriceDiscount();
                            break;
                        case PromotionAdminController.MultiTierDiscount:
                            UndoMultiTierDiscount();
                            break;
                    }
                }
            }
             */
        ArrayList MultipleOrderLineID;
        private void UndoItemGroupPriceDiscount()
        {
            string status;
            for (int j = 0; j < MultipleOrderLineID.Count; j++)
            {
                pos.SetOrderLineAsPromo(MultipleOrderLineID[j].ToString(), false, 0, 0, out status);
                pos.TryMergeOrderLine(MultipleOrderLineID[j].ToString());
            }
        }
        private void UndoBuyXGetYFree()
        {
            string status = "";
            pos.DeleteAllIsPromoFOCLine();
            pos.SetOrderLineAsPromo(OrderLineID, false, 0, 0, out status);

            //merge items with same item no.....
            pos.TryMergeOrderLine(OrderLineID);
        }
        private void UndoDiscountByCategory()
        {
            string status = "";
            pos.SetOrderLineAsPromo(OrderLineID, false, 0, 0, out status);
        }
        private void UndoDiscountByItem()
        {
            string status = "";
            pos.SetOrderLineAsPromo(OrderLineID, false, 0, 0, out status);
        }
        private void UndoMultiTierDiscount()
        {
            string status = "";
            pos.SetOrderLineAsPromo(OrderLineID, false, 0, 0, out status);
        }
        #endregion

        #region "Getters"
        public string GetCurrentPromoLineID()
        {
            return OrderLineID;
        }
        #endregion

        internal bool GetCurrentPromoInfo(out string campaignName, out string campaignType)
        {
            campaignName = "";
            campaignType = "";
            if (CurrentPromoHdrID != -1)
            {
                PromoCampaignHdr hdr = new PromoCampaignHdr(CurrentPromoHdrID);
                if (hdr.IsLoaded)
                {
                    campaignName = hdr.PromoCampaignName;
                    campaignType = hdr.CampaignType;
                    return true;
                }
            }
            return false;
        }
        #endregion
    }
    /*
private OrderDet ConvertDataRowToOrderDet(DataRow dr)
{
    bool IsVoided, IsPromo, IsFreeOfCharge;
    OrderDet myOrderDet = new OrderDet();
    myOrderDet.ItemNo =  dr["ItemNo"].ToString();
    myOrderDet.Quantity = int.Parse(dr["ItemNo"].ToString());
    bool.TryParse(dr["IsVoided"].ToString(), out IsVoided);
    bool.TryParse(dr["IsPromo"].ToString(), out IsPromo);
    bool.TryParse(dr["IsFreeOfCharge"].ToString(), out IsFreeOfCharge);
    myOrderDet.IsVoided = IsVoided;
    myOrderDet.IsPromo = IsPromo;
    myOrderDet.IsFreeOfCharge = IsFreeOfCharge;
    if (myOrderDet.IsPromo)
    {
        myOrderDet.PromoDiscount = double.Parse(dr["Discount"].ToString());
        myOrderDet.PromoAmount = decimal.Parse(dr["Amount"].ToString());
    }
    else
    {
        myOrderDet.Discount = decimal.Parse(dr["Discount"].ToString());
        myOrderDet.Amount  = decimal.Parse(dr["Amount"].ToString());
    }

}*/
    public class myOrderDetComparer : IComparer<PowerPOS.OrderDet>
    {

        // Calls CaseInsensitiveComparer.Compare with the parameters reversed.
        int IComparer<PowerPOS.OrderDet>.Compare(PowerPOS.OrderDet x, PowerPOS.OrderDet y)
        {
            return string.Compare(((OrderDet)x).ItemNo, ((OrderDet)y).ItemNo);
        }

    }

}
