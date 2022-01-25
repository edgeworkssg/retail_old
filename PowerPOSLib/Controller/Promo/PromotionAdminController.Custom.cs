using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using SubSonic;
using System.Collections;
using PowerPOS.Container;
using System.ComponentModel;
using System.Linq;

namespace PowerPOS
{
    [System.ComponentModel.DataObject]
    public partial class PromotionAdminController
    {
        #region "Generic for all"
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public void DeletePromotionCampaign(object PromoCampaignHdrID)
        {
            //Set as Disable instead
            PromoCampaignHdr pHdr = new PromoCampaignHdr(PromoCampaignHdrID);
            pHdr.Enabled = false;
            pHdr.Save("System");
        }

        public static bool IsCampaignNameNotUsed(string CampaignName)
        {
            Where whr = new Where();
            whr.ColumnName = PromoCampaignHdr.Columns.PromoCampaignName;
            whr.Comparison = Comparison.Equals;
            whr.ParameterName = "@" + PromoCampaignHdr.Columns.PromoCampaignName;
            whr.ParameterValue = CampaignName;
            whr.TableName = "PromoCampaignHdr";

            int cnt = new Query("PromoCampaignHdr").GetCount(PromoCampaignHdr.Columns.PromoCampaignName, whr);
            if (cnt == 0)
                return true;
            return false;
        }

        public static System.Web.UI.WebControls.ListItem[] FetchCampaignTypeDropDownList()
        {
            System.Web.UI.WebControls.ListItem[] ls = new System.Web.UI.WebControls.ListItem[6];

            ls[0] = new System.Web.UI.WebControls.ListItem();
            ls[0].Text = "ALL";
            ls[0].Value = "";

            ls[1] = new System.Web.UI.WebControls.ListItem();
            ls[1].Text = "Buy 1 Get 1 Free";
            ls[1].Value = PromotionAdminController.BuyXGetYFree;

            ls[2] = new System.Web.UI.WebControls.ListItem();
            ls[2].Text = "Discount  By Item";
            ls[2].Value = PromotionAdminController.DiscountByItem;

            ls[3] = new System.Web.UI.WebControls.ListItem();
            ls[3].Text = "Discount By Category";
            ls[3].Value = PromotionAdminController.DiscountByCategory;

            ls[4] = new System.Web.UI.WebControls.ListItem();
            ls[4].Text = "Item Group Discount";
            ls[4].Value = PromotionAdminController.ItemGroupPriceDiscount;

            ls[5] = new System.Web.UI.WebControls.ListItem();
            ls[5].Text = "Multitier Discount";
            ls[5].Value = PromotionAdminController.MultiTierDiscount;

            return ls;
        }

        public static DataTable FetchAllActivePromo
            (bool useStartDate, bool useEndDate,
                DateTime startDate, DateTime endDate,
                string campaignName, string campaignType)
        {
            PromoCampaignHdrCollection col = new PromoCampaignHdrCollection();
            col.Where(PromoCampaignHdr.Columns.Enabled, true);
            if (useStartDate)
            {
                col.Where(PromoCampaignHdr.Columns.DateFrom,
                    Comparison.GreaterOrEquals, startDate);
            }
            if (useEndDate)
            {
                col.Where(PromoCampaignHdr.Columns.DateTo,
                    Comparison.LessOrEquals, endDate);
            }
            if (campaignName != "")
            {
                col.Where(PromoCampaignHdr.Columns.PromoCampaignName, Comparison.Like, "%" + campaignName + "%");
            }
            if (campaignType != "")
            {
                col.Where(PromoCampaignHdr.Columns.CampaignType, Comparison.Like, "%" + campaignType + "%");
            }
            col.OrderByDesc(PromoCampaignHdr.Columns.DateFrom);
            col.Load();
            return col.ToDataTable();
        }

        public static DataTable FetchDiscountByItemPromo
            (bool useStartDate, bool useEndDate,
                DateTime startDateFrom, DateTime endDateFrom,
                DateTime startDateTo, DateTime endDateTo,
                string campaignName, decimal discount)
        {
            ViewPromotionsByItemCollection col = new ViewPromotionsByItemCollection();

            //col.Where(ViewPromotionsByItem.Columns.Enabled, true);
            if (useStartDate)
            {
                col.Where(ViewPromotionsByItem.Columns.DateFrom,
                    Comparison.GreaterOrEquals, startDateFrom);
                col.Where(ViewPromotionsByItem.Columns.DateFrom,
                    Comparison.LessOrEquals, endDateFrom);
            }

            if (useEndDate)
            {
                col.Where(ViewPromotionsByItem.Columns.DateTo,
                    Comparison.GreaterOrEquals, startDateTo);
                col.Where(ViewPromotionsByItem.Columns.DateTo,
                    Comparison.LessOrEquals, endDateTo);
            }
            if (campaignName != "")
            {
                col.Where(ViewPromotionsByItem.Columns.PromoCampaignName,
                    Comparison.Like, "%" + campaignName + "%");
            }

            if (discount != 0)
            {
                col.Where(ViewPromotionsByItem.Columns.PromoDiscount,
                 Comparison.Like, discount);
            }

            col.Where(ViewPromotionsByItem.Columns.CampaignType, PromotionAdminController.DiscountByItem);

            col.OrderByDesc(ViewPromotionsByItem.Columns.PromoCampaignHdrID);
            col.Load();
            return col.ToDataTable();
        }

        public static DataTable FetchDiscountByItemGroupPromo
            (bool useStartDate, bool useEndDate,
                DateTime startDateFrom, DateTime endDateFrom,
                DateTime startDateTo, DateTime endDateTo,
                string campaignName, decimal discount, decimal price)
        {
            ViewPromoMasterDetailCollection col = new ViewPromoMasterDetailCollection();

            col.Where(ViewPromotionsByItem.Columns.Enabled, true);
            if (useStartDate)
            {
                col.Where(ViewPromoMasterDetail.Columns.DateFrom,
                    Comparison.GreaterOrEquals, startDateFrom);
                col.Where(ViewPromoMasterDetail.Columns.DateFrom,
                    Comparison.LessOrEquals, endDateFrom);
            }

            if (useEndDate)
            {
                col.Where(ViewPromoMasterDetail.Columns.DateTo,
                    Comparison.GreaterOrEquals, startDateTo);
                col.Where(ViewPromoMasterDetail.Columns.DateTo,
                    Comparison.LessOrEquals, endDateTo);
            }
            if (campaignName != "")
            {
                col.Where(ViewPromoMasterDetail.Columns.PromoCampaignName,
                    Comparison.Like, "%" + campaignName + "%");
            }

            if (price != 0)
            {
                col.Where(ViewPromoMasterDetail.Columns.PromoPrice, price);
            }

            if (discount != 0)
            {
                col.Where(ViewPromoMasterDetail.Columns.PromoDiscount, discount);
            }

            col.Where(ViewPromotionsByItem.Columns.CampaignType,
                PromotionAdminController.ItemGroupPriceDiscount);

            col.OrderByDesc(ViewPromotionsByItem.Columns.PromoCampaignHdrID);
            col.Load();
            DataTable dt = col.ToDataTable();

            dt.Columns.Add("Items");
            dt.Columns.Add("Value");

            ArrayList ar = new ArrayList();

            //List out ItemGroupMaps
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ar.Add((int)dt.Rows[i]["ItemGroupID"]);
            }


            ViewItemGroupCollection ig = new ViewItemGroupCollection();
            /*ig.Where(ViewItemGroup.Columns.ItemGroupID, Comparison.In, ar);
            //ig.OrderByAsc(ViewItemGroup.Columns.ItemGroupID);
            ig.Load();*/

            Query qr = ViewItemGroup.CreateQuery();
            ig.Load(qr.IN(ViewItemGroup.Columns.ItemGroupID, ar).ORDER_BY(ViewItemGroup.Columns.ItemGroupID, "ASC").ExecuteDataSet().Tables[0]);

            //
            int index = 0, itemgroupID;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                itemgroupID = (int)dt.Rows[i]["ItemGroupID"];
                index = ig.Find(ViewItemGroup.Columns.ItemGroupID, itemgroupID);
                //loop through while still the same itemgroupid

                while (index >= 0 && index < ig.Count && itemgroupID == ig[index].ItemGroupID)
                {
                    dt.Rows[i]["Items"] += ig[index].ItemName + ", ";
                    index++;
                }
                if (dt.Rows[i]["Items"].ToString().Length >= 2)
                    dt.Rows[i]["Items"] = dt.Rows[i]["Items"].ToString().Substring(0, dt.Rows[i]["Items"].ToString().Length - 2);

                //
                if (dt.Rows[i]["PromoPrice"] != null
                    && dt.Rows[i]["PromoPrice"] is decimal
                    && (decimal)dt.Rows[i]["PromoPrice"] != 0)
                {
                    dt.Rows[i]["Value"] = "$" + ((decimal)dt.Rows[i]["PromoPrice"]).ToString("N2");
                }
                else if (dt.Rows[i]["PromoDiscount"] != null
                    && dt.Rows[i]["PromoDiscount"] is double
                    && (double)dt.Rows[i]["PromoDiscount"] != 0)
                {
                    dt.Rows[i]["Value"] = ((double)dt.Rows[i]["PromoDiscount"]).ToString("N2") + "%";
                }
            }
            return dt;
        }

        public static bool AddPromoLocationMap(int PromoCampaignHdrID, int PointOfSaleID)
        {
            //Check Uniqueness
            DataSet ds = PromoLocationMap.CreateQuery().
                            WHERE("PointOfSaleID = " + PointOfSaleID).
                            AND("PromoCampaignHdrID = " + PromoCampaignHdrID).AND("deleted=false").
                            ORDER_BY("ModifiedOn", "Desc").
                            ExecuteDataSet();

            string status = "";

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 0)
            {

                //Do Create
                PromoLocationMap myMap = new PromoLocationMap();
                myMap.PromoCampaignHdrID = PromoCampaignHdrID;
                myMap.PointOfSaleID = PointOfSaleID;
                myMap.Deleted = false;
                myMap.Save();
                return true;
            }
            return true;
        }

        public static DataTable FetchPromoLocationMap
            (
                DateTime CurrentDate,
                string campaignName,
                string campaignType,
                int PromoCampaignHdrID)
        {
            ViewPromoLocationMapCollection col = new ViewPromoLocationMapCollection();
            col.Where(ViewPromoLocationMap.Columns.Enabled, true);
            col.Where(PromoCampaignHdr.Columns.DateFrom, Comparison.LessOrEquals, CurrentDate);
            col.Where(PromoCampaignHdr.Columns.DateTo, Comparison.GreaterOrEquals, CurrentDate);
            if (campaignName != "")
            {
                col.Where(ViewPromoLocationMap.Columns.PromoCampaignName, Comparison.Like, "%" + campaignName + "%");
            }
            if (campaignType != "")
            {
                col.Where(ViewPromoLocationMap.Columns.CampaignType, Comparison.Like, "%" + campaignType + "%");
            }
            if (PromoCampaignHdrID != 0)
            {
                col.Where(ViewPromoLocationMap.Columns.PromoCampaignHdrID, PromoCampaignHdrID);
            }
            col.OrderByDesc(ViewPromoLocationMap.Columns.DateFrom);
            col.Load();

            return col.ToDataTable();
        }

        public static DataTable FetchPromoMembershipMap
            (
                DateTime CurrentDate,
                string campaignName,
                string campaignType,
                int PromoCampaignHdrID)
        {
            ViewPromoMembershipMapCollection col = new ViewPromoMembershipMapCollection();
            col.Where(ViewPromoMembershipMap.Columns.Enabled, true);
            col.Where(PromoCampaignHdr.Columns.DateFrom, Comparison.LessOrEquals, CurrentDate);
            col.Where(PromoCampaignHdr.Columns.DateTo, Comparison.GreaterOrEquals, CurrentDate);
            if (campaignName != "")
            {
                col.Where(ViewPromoMembershipMap.Columns.PromoCampaignName, Comparison.Like, "%" + campaignName + "%");
            }
            if (campaignType != "")
            {
                col.Where(ViewPromoMembershipMap.Columns.CampaignType, Comparison.Like, "%" + campaignType + "%");
            }
            if (PromoCampaignHdrID != 0)
            {
                col.Where(ViewPromoMembershipMap.Columns.PromoCampaignHdrID, PromoCampaignHdrID);
            }
            col.OrderByDesc(ViewPromoMembershipMap.Columns.DateFrom);
            col.Load();

            return col.ToDataTable();
        }

        public static bool AddPromoMembershipMap
            (int PromoCampaignHdrID, int MembershipGroupID, decimal MembershipPrice,
             decimal MembershipDiscount, bool UseMembershipPrice)
        {
            //Check Uniqueness
            DataSet ds = PromoMembershipMap.CreateQuery().
                            WHERE("MembershipGroupID = " + MembershipGroupID).
                            AND("PromoCampaignHdrID = " + PromoCampaignHdrID).AND("deleted=false").
                            ORDER_BY("ModifiedOn", "Desc").
                            ExecuteDataSet();

            string status = "";

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 0)
            {

                //Do Create
                PromoMembershipMap myMap = new PromoMembershipMap();
                myMap.PromoCampaignHdrID = PromoCampaignHdrID;
                myMap.MembershipGroupID = MembershipGroupID;
                myMap.MembershipPrice = MembershipPrice;
                myMap.MembershipDiscount = MembershipDiscount;
                myMap.UseMembershipPrice = UseMembershipPrice;
                myMap.Deleted = false;
                myMap.Save();
            }
            else
            {
                //Do update
                PromoMembershipMap myMap = new PromoMembershipMap(ds.Tables[0].Rows[0]["PromoMembershipID"].ToString());
                myMap.MembershipPrice = MembershipPrice;
                myMap.MembershipDiscount = MembershipDiscount;
                myMap.UseMembershipPrice = UseMembershipPrice;
                myMap.Save();
                return true;
            }
            return true;
        }

        public static DataTable FetchAllPromoByDate(DateTime CurrentDate)
        {
            PromoCampaignHdrCollection col = new PromoCampaignHdrCollection();
            col.Where(PromoCampaignHdr.Columns.DateFrom, Comparison.LessOrEquals, CurrentDate);
            col.Where(PromoCampaignHdr.Columns.DateTo, Comparison.GreaterOrEquals, CurrentDate);
            col.Where(PromoCampaignHdr.Columns.Enabled, true);
            col.Where(PromoCampaignHdr.Columns.Deleted, Comparison.NotEquals, true);
            col.Load();
            return col.ToDataTable();
        }

        public static void EnablePromo(int HdrID, bool promoEnabled)
        {
            Query qry = new Query("PromoCampaignHdr");
            qry.AddWhere(PromoCampaignHdr.Columns.PromoCampaignHdrID, HdrID);
            qry.AddUpdateSetting(PromoCampaignHdr.Columns.Enabled, promoEnabled);
            qry.Execute();

            return;
        }

        public static bool PromoChangePeriod(int HdrID, DateTime dateFrom, DateTime dateTo)
        {
            try
            {
                Query qry = new Query("PromoCampaignHdr");
                qry.AddWhere(PromoCampaignHdr.Columns.PromoCampaignHdrID, HdrID);
                qry.AddUpdateSetting(PromoCampaignHdr.Columns.DateFrom, dateFrom);
                qry.AddUpdateSetting(PromoCampaignHdr.Columns.DateTo, dateTo);
                qry.Execute();

                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return false;
            }
        }

        public static bool PromoChangeForNonMembersAlso(int HdrID, bool membersOnly)
        {
            try
            {
                Query qry = new Query("PromoCampaignHdr");
                qry.AddWhere(PromoCampaignHdr.Columns.PromoCampaignHdrID, HdrID);
                qry.AddUpdateSetting(PromoCampaignHdr.Columns.ForNonMembersAlso, membersOnly);

                qry.Execute();

                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return false;
            }
        }
        #endregion

        #region "BuyXGetYFree"
        public const string BuyXGetYFree = "BuyXGetYFree";
        public static bool InsertBuyXGetYFree
            (string CampaignName, DateTime startDate, DateTime endDate,
            string PurchasedItemNo, int unitQty, string FreeItemNo, int freeQty,
            bool ForNonMembersAlso)
        {
            try
            {
                if (!IsCampaignNameNotUsed(CampaignName))
                    return false;

                //This is what the customer is Getting
                PromoCampaignHdr hdr = new PromoCampaignHdr();
                hdr.PromoCampaignName = CampaignName;
                hdr.DateFrom = startDate;
                hdr.DateTo = endDate;
                hdr.FreeItemNo = FreeItemNo;
                hdr.FreeQty = freeQty;
                hdr.CampaignType = BuyXGetYFree;
                hdr.Enabled = true;
                hdr.ForNonMembersAlso = ForNonMembersAlso;
                hdr.Save(UserInfo.username);

                //This is what the customer is buying
                PromoCampaignDet det = new PromoCampaignDet();
                det.ItemNo = PurchasedItemNo;
                det.UnitQty = unitQty;
                det.PromoCampaignHdrID = (int)hdr.GetPrimaryKeyValue();
                det.Save(UserInfo.username);



                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return false;
            }
        }
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public DataTable ViewBuyXGetYFree(DateTime startDate, DateTime endDate)
        {
            PromoCampaignHdrCollection hdr = new PromoCampaignHdrCollection();
            PromoCampaignDetCollection det;
            hdr.Where(PromoCampaignHdr.Columns.DateFrom, Comparison.GreaterOrEquals, startDate);
            hdr.Where(PromoCampaignHdr.Columns.DateFrom, Comparison.LessOrEquals, endDate);
            hdr.Where(PromoCampaignHdr.Columns.CampaignType, BuyXGetYFree);
            hdr.Where(PromoCampaignHdr.Columns.Enabled, true);
            hdr.Load();

            DataTable dt = new DataTable();
            DataRow dr;

            dt.Columns.Add(PromoCampaignHdr.Columns.DateFrom);
            dt.Columns.Add(PromoCampaignHdr.Columns.DateTo);
            dt.Columns.Add(PromoCampaignHdr.Columns.PromoCampaignHdrID);
            dt.Columns.Add(PromoCampaignHdr.Columns.PromoCampaignName);
            dt.Columns.Add(PromoCampaignHdr.Columns.CampaignType);
            dt.Columns.Add("FreeItemName");
            dt.Columns.Add(PromoCampaignHdr.Columns.FreeQty);
            dt.Columns.Add("PurchasedItemName");
            dt.Columns.Add(PromoCampaignDet.Columns.UnitQty);

            for (int i = 0; i < hdr.Count; i++)
            {
                det = new PromoCampaignDetCollection();
                det.Where("PromoCampaignHdrID", hdr[i].PromoCampaignHdrID);
                det.Load();
                for (int j = 0; j < det.Count; j++)
                {
                    dr = dt.NewRow();
                    dr["DateFrom"] = hdr[i].DateFrom.ToString("dd MMM yyyy");
                    dr["DateTo"] = hdr[i].DateTo.ToString("dd MMM yyyy");
                    dr["PromoCampaignHdrID"] = hdr[i].PromoCampaignHdrID;
                    dr["PromoCampaignName"] = hdr[i].PromoCampaignName;
                    dr["CampaignType"] = hdr[i].CampaignType;
                    dr["FreeItemName"] = new Item(hdr[i].FreeItemNo).ItemName;
                    dr["FreeQty"] = hdr[i].FreeQty;
                    dr["PurchasedItemName"] = new Item(det[j].ItemNo).ItemName;
                    dr["UnitQty"] = det[j].UnitQty;

                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }


        #endregion

        #region "DiscountByCategory"
        public const string DiscountByCategory = "DiscountByCategory";
        public static bool InsertDiscountByCategory(
            string CampaignName,
            DateTime startDate,
            DateTime endDate,
            string CategoryName, double discount, bool ForNonMembersAlso)
        {
            try
            {
                if (!IsCampaignNameNotUsed(CampaignName))
                    return false;

                //This is what the customer is Getting
                PromoCampaignHdr hdr = new PromoCampaignHdr();
                hdr.PromoCampaignName = CampaignName;
                hdr.DateFrom = startDate;
                hdr.DateTo = endDate;
                hdr.PromoDiscount = discount;
                hdr.CampaignType = DiscountByCategory;
                hdr.Enabled = true;
                hdr.ForNonMembersAlso = ForNonMembersAlso;
                hdr.Deleted = false;
                hdr.Save(UserInfo.username);

                //This is what the customer is buying
                PromoCampaignDet det = new PromoCampaignDet();
                det.CategoryName = CategoryName;
                det.MinQuantity = 0;
                det.PromoCampaignHdrID = (int)hdr.GetPrimaryKeyValue();
                det.Deleted = false;
                det.Save(UserInfo.username);

                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return false;
            }
        }
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public DataTable ViewDiscountByCategory(DateTime startDate, DateTime endDate)
        {
            PromoCampaignHdrCollection hdr = new PromoCampaignHdrCollection();
            PromoCampaignDetCollection det;
            hdr.Where(PromoCampaignHdr.Columns.DateFrom, Comparison.GreaterOrEquals, startDate);
            hdr.Where(PromoCampaignHdr.Columns.DateFrom, Comparison.LessOrEquals, endDate);
            hdr.Where(PromoCampaignHdr.Columns.CampaignType, DiscountByCategory);
            hdr.Where(PromoCampaignHdr.Columns.Enabled, true);
            hdr.Load();

            DataTable dt = new DataTable();
            DataRow dr;

            dt.Columns.Add(PromoCampaignHdr.Columns.DateFrom);
            dt.Columns.Add(PromoCampaignHdr.Columns.DateTo);
            dt.Columns.Add(PromoCampaignHdr.Columns.PromoCampaignHdrID);
            dt.Columns.Add(PromoCampaignHdr.Columns.PromoCampaignName);
            dt.Columns.Add(PromoCampaignHdr.Columns.CampaignType);
            dt.Columns.Add(PromoCampaignDet.Columns.CategoryName);
            dt.Columns.Add(PromoCampaignHdr.Columns.PromoPrice);
            dt.Columns.Add(PromoCampaignHdr.Columns.PromoDiscount);

            for (int i = 0; i < hdr.Count; i++)
            {
                det = new PromoCampaignDetCollection();
                det.Where(PromoCampaignDet.Columns.PromoCampaignHdrID, hdr[i].PromoCampaignHdrID);
                det.Load();
                for (int j = 0; j < det.Count; j++)
                {
                    dr = dt.NewRow();
                    dr["DateFrom"] = hdr[i].DateFrom.ToString("dd MMM yyyy");
                    dr["DateTo"] = hdr[i].DateTo.ToString("dd MMM yyyy");
                    dr["PromoCampaignHdrID"] = hdr[i].PromoCampaignHdrID;
                    dr["PromoCampaignName"] = hdr[i].PromoCampaignName;
                    dr["CampaignType"] = hdr[i].CampaignType;
                    dr["CategoryName"] = det[j].CategoryName;
                    dr["PromoDiscount"] = hdr[i].PromoDiscount;

                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }

        #endregion

        #region "DiscountByItem"
        public const string DiscountByItem = "DiscountByItem";
        public static bool InsertDiscountByItem(
            string CampaignName,
            DateTime startDate,
            DateTime endDate,
            string ItemNo, double discount, bool ForNonMembersAlso)
        {
            try
            {
                if (!IsCampaignNameNotUsed(CampaignName))
                    return false;

                //This is what the customer is Getting
                PromoCampaignHdr hdr = new PromoCampaignHdr();
                hdr.PromoCampaignName = CampaignName;
                hdr.DateFrom = startDate;
                hdr.DateTo = endDate;
                hdr.Enabled = true;
                hdr.PromoDiscount = discount;
                hdr.CampaignType = DiscountByItem;
                hdr.ForNonMembersAlso = ForNonMembersAlso;
                hdr.Deleted = false;
                hdr.Save(UserInfo.username);

                //This is what the customer is buying
                PromoCampaignDet det = new PromoCampaignDet();
                det.ItemNo = ItemNo;
                det.MinQuantity = 0;
                det.PromoCampaignHdrID = (int)hdr.GetPrimaryKeyValue();
                det.Deleted = false;
                det.Save(UserInfo.username);

                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return false;
            }
        }

        public static bool InsertDiscountByItemByBatch(
            string CampaignName,
            DateTime startDate,
            DateTime endDate,
            ArrayList ItemNos, double discount, bool ForNonMembersAlso)
        {
            try
            {
                QueryCommandCollection cmd = new QueryCommandCollection();
                if (!IsCampaignNameNotUsed(CampaignName))
                {
                    return false;
                }

                //This is what the customer is Getting
                PromoCampaignHdr hdr = new PromoCampaignHdr();
                hdr.PromoCampaignName = CampaignName;
                hdr.DateFrom = startDate;
                hdr.DateTo = endDate;
                hdr.Enabled = true;
                hdr.PromoDiscount = discount;
                hdr.CampaignType = DiscountByItem;
                hdr.ForNonMembersAlso = ForNonMembersAlso;
                hdr.Deleted = false;
                hdr.Save();
                //cmd.Add(hdr.GetSaveCommand());

                //This is what the customer is buying
                for (int j = 0; j < ItemNos.Count; j++)
                {
                    PromoCampaignDet det = new PromoCampaignDet();
                    det.ItemNo = ItemNos[j].ToString();
                    det.MinQuantity = 0;
                    det.PromoCampaignHdrID = (int)hdr.GetPrimaryKeyValue();
                    det.Deleted = false;
                    cmd.Add(det.GetSaveCommand());
                }
                SubSonic.DataService.ExecuteTransaction(cmd);

                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return false;
            }
        }
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public DataTable ViewDiscountByItemName(DateTime startDate, DateTime endDate)
        {
            PromoCampaignHdrCollection hdr = new PromoCampaignHdrCollection();
            PromoCampaignDetCollection det;
            hdr.Where(PromoCampaignHdr.Columns.DateFrom, Comparison.GreaterOrEquals, startDate);
            hdr.Where(PromoCampaignHdr.Columns.DateFrom, Comparison.LessOrEquals, endDate);
            hdr.Where(PromoCampaignHdr.Columns.DateFrom, Comparison.GreaterOrEquals, startDate);
            hdr.Where(PromoCampaignHdr.Columns.DateFrom, Comparison.LessOrEquals, endDate);
            hdr.Where(PromoCampaignHdr.Columns.CampaignType, DiscountByItem);
            hdr.Where(PromoCampaignHdr.Columns.Enabled, true);
            hdr.Load();

            DataTable dt = new DataTable();
            DataRow dr;

            dt.Columns.Add(PromoCampaignHdr.Columns.DateFrom);
            dt.Columns.Add(PromoCampaignHdr.Columns.DateTo);
            dt.Columns.Add(PromoCampaignHdr.Columns.PromoCampaignHdrID);
            dt.Columns.Add(PromoCampaignHdr.Columns.PromoCampaignName);
            dt.Columns.Add(PromoCampaignHdr.Columns.CampaignType);
            dt.Columns.Add(Item.Columns.ItemName);
            dt.Columns.Add(PromoCampaignHdr.Columns.PromoPrice);
            dt.Columns.Add(PromoCampaignHdr.Columns.PromoDiscount);

            for (int i = 0; i < hdr.Count; i++)
            {
                det = new PromoCampaignDetCollection();
                det.Where(PromoCampaignDet.Columns.PromoCampaignHdrID, hdr[i].PromoCampaignHdrID);
                det.Load();
                for (int j = 0; j < det.Count; j++)
                {
                    dr = dt.NewRow();
                    dr["DateFrom"] = hdr[i].DateFrom.ToString("dd MMM yyyy");
                    dr["DateTo"] = hdr[i].DateTo.ToString("dd MMM yyyy");
                    dr["PromoCampaignHdrID"] = hdr[i].PromoCampaignHdrID;
                    dr["PromoCampaignName"] = hdr[i].PromoCampaignName;
                    dr["CampaignType"] = hdr[i].CampaignType;
                    if (det[j].Item != null)
                        dr["ItemName"] = det[j].Item.ItemName;
                    dr["PromoDiscount"] = hdr[i].PromoDiscount;

                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }

        #endregion

        #region "ItemGroupPriceDiscount"
        public const string ItemGroupPriceDiscount = "ItemGroupPriceDiscount";

        public static bool InsertItemGroupPriceDiscount(
            string CampaignName,
            DateTime startDate,
            DateTime endDate,
            string ItemGroupName,
            decimal Price,
            double discount, bool ForNonMembersAlso
            )
        {
            try
            {
                if (!IsCampaignNameNotUsed(CampaignName))
                    return false;

                //Check whether Group Name already existed....                
                ItemGroup ig = new ItemGroup("ItemGroupName", ItemGroupName);
                if (ig.IsNew)
                {
                    return false;
                }
                //if it is not new, ignore the datatable...

                //This is what the customer is Getting
                PromoCampaignHdr hdr = new PromoCampaignHdr();
                hdr.PromoCampaignName = CampaignName;
                hdr.DateFrom = startDate;
                hdr.DateTo = endDate;
                hdr.PromoDiscount = discount;
                hdr.PromoPrice = Price;
                hdr.Deleted = false;
                hdr.Enabled = true;
                hdr.CampaignType = ItemGroupPriceDiscount;
                hdr.ForNonMembersAlso = ForNonMembersAlso;
                hdr.Save(UserInfo.username);

                //This is what the customer is buying
                PromoCampaignDet det = new PromoCampaignDet();

                det.PromoCampaignHdrID = (int)hdr.GetPrimaryKeyValue();
                det.ItemGroupID = (int)ig.GetPrimaryKeyValue();
                det.Deleted = false;
                det.Save(UserInfo.username);

                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return false;
            }
        }

        public static bool InsertItemGroupPriceDiscount(
            string CampaignName,
            DateTime startDate,
            DateTime endDate,
            string ItemGroupName,
            string ItemGroupBarcode,
            ItemCollection ItemGroup,
            decimal Price,
            double discount, bool ForNonMembersAlso
            )
        {
            try
            {
                if (!IsCampaignNameNotUsed(CampaignName))
                    return false;

                //Check whether Group Name already existed....                
                ItemGroup ig = new ItemGroup("ItemGroupName", ItemGroupName);
                if (ig.IsNew)
                {
                    ig.ItemGroupName = ItemGroupName;
                    ig.Barcode = ItemGroupBarcode;
                    ig.Deleted = false;
                    ig.Save();
                    //create item group map
                    for (int p = 0; p < ItemGroup.Count; p++)
                    {
                        ItemGroupMap iMap = new ItemGroupMap();
                        iMap.ItemNo = ItemGroup[p].ItemNo;
                        iMap.ItemGroupID = (int)ig.GetPrimaryKeyValue();
                        iMap.UnitQty = (int)ItemGroup[p].Userfloat1;
                        iMap.Deleted = false;
                        iMap.Save();
                    }
                }

                //if it is not new, ignore the datatable...

                //This is what the customer is Getting
                PromoCampaignHdr hdr = new PromoCampaignHdr();
                hdr.PromoCampaignName = CampaignName;
                hdr.DateFrom = startDate;
                hdr.DateTo = endDate;
                hdr.PromoDiscount = discount;
                hdr.PromoPrice = Price;

                hdr.Enabled = true;
                hdr.CampaignType = ItemGroupPriceDiscount;
                hdr.ForNonMembersAlso = ForNonMembersAlso;
                hdr.Deleted = false;
                hdr.Save(UserInfo.username);

                //This is what the customer is buying
                PromoCampaignDet det = new PromoCampaignDet();

                det.PromoCampaignHdrID = (int)hdr.GetPrimaryKeyValue();
                det.ItemGroupID = (int)ig.GetPrimaryKeyValue();
                det.Deleted = false;
                det.Save(UserInfo.username);

                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return false;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public DataTable ViewItemGroupPriceDiscount(DateTime startDate, DateTime endDate)
        {
            PromoCampaignHdrCollection hdr = new PromoCampaignHdrCollection();
            PromoCampaignDetCollection det;
            hdr.Where(PromoCampaignHdr.Columns.DateFrom, Comparison.GreaterOrEquals, startDate);
            hdr.Where(PromoCampaignHdr.Columns.DateFrom, Comparison.LessOrEquals, endDate);
            hdr.Where(PromoCampaignHdr.Columns.CampaignType, ItemGroupPriceDiscount);
            hdr.Where(PromoCampaignHdr.Columns.Enabled, true);
            hdr.Load();

            DataTable dt = new DataTable();
            DataRow dr;

            dt.Columns.Add(PromoCampaignHdr.Columns.DateFrom);
            dt.Columns.Add(PromoCampaignHdr.Columns.DateTo);
            dt.Columns.Add(PromoCampaignHdr.Columns.PromoCampaignHdrID);
            dt.Columns.Add(PromoCampaignHdr.Columns.PromoCampaignName);
            dt.Columns.Add(PromoCampaignHdr.Columns.CampaignType);
            dt.Columns.Add("ItemGroupName");
            dt.Columns.Add(PromoCampaignHdr.Columns.PromoPrice);
            dt.Columns.Add(PromoCampaignHdr.Columns.PromoDiscount);

            for (int i = 0; i < hdr.Count; i++)
            {
                det = hdr[i].PromoCampaignDetRecords();
                for (int j = 0; j < det.Count; j++)
                {
                    dr = dt.NewRow();
                    dr["DateFrom"] = hdr[i].DateFrom.ToString("dd MMM yyyy");
                    dr["DateTo"] = hdr[i].DateTo.ToString("dd MMM yyyy");
                    dr["PromoCampaignHdrID"] = hdr[i].PromoCampaignHdrID;
                    dr["PromoCampaignName"] = hdr[i].PromoCampaignName;
                    dr["CampaignType"] = hdr[i].CampaignType;
                    ItemGroup myItemGrp = det[j].ItemGroup;
                    if (myItemGrp != null)
                        dr["ItemGroupName"] = myItemGrp.ItemGroupName;
                    if (hdr[i].PromoPrice != null)
                        dr["PromoPrice"] = hdr[i].PromoPrice.Value;
                    if (hdr[i].PromoDiscount != null)
                        dr["PromoDiscount"] = hdr[i].PromoDiscount;

                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }

        #endregion

        #region "Multitier Discount"

        public const string MultiTierDiscount = "MultiTierDiscount";
        public static bool InsertMultiTierDiscount
            (string CampaignName, DateTime startDate, DateTime endDate,
                string ItemNo, DataTable dt, bool ForNonMembersAlso)
        {
            try
            {
                if (!IsCampaignNameNotUsed(CampaignName))
                    return false;
                //This is what the customer is Getting
                PromoCampaignHdr hdr = new PromoCampaignHdr();
                hdr.PromoCampaignName = CampaignName;
                hdr.DateFrom = startDate;
                hdr.DateTo = endDate;
                hdr.Enabled = true;
                hdr.CampaignType = MultiTierDiscount;
                hdr.ForNonMembersAlso = ForNonMembersAlso;
                hdr.Save(UserInfo.username);

                //This is what the customer is buying
                PromoCampaignDet det = new PromoCampaignDet();
                det.ItemNo = ItemNo;
                det.MinQuantity = 0;
                det.PromoCampaignHdrID = (int)hdr.GetPrimaryKeyValue();
                det.Save(UserInfo.username);

                //input a table....
                PromoDiscountTier promoTier;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    promoTier = new PromoDiscountTier();
                    promoTier.WhichQty = int.Parse(dt.Rows[i]["TresholdQty"].ToString());
                    promoTier.Discount = double.Parse(dt.Rows[i]["Discount"].ToString());
                    promoTier.PromoCampaignHdrID = (int)hdr.GetPrimaryKeyValue();
                    promoTier.Save(UserInfo.username);
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return false;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public DataTable ViewMultiTierDiscount(DateTime startDate, DateTime endDate)
        {
            PromoCampaignHdrCollection hdr = new PromoCampaignHdrCollection();
            PromoCampaignDetCollection det;
            hdr.Where(PromoCampaignHdr.Columns.DateFrom, Comparison.GreaterOrEquals, startDate);
            hdr.Where(PromoCampaignHdr.Columns.DateFrom, Comparison.LessOrEquals, endDate);
            hdr.Where(PromoCampaignHdr.Columns.CampaignType, MultiTierDiscount);
            hdr.Where(PromoCampaignHdr.Columns.Enabled, true);
            hdr.Load();

            DataTable dt = new DataTable();
            DataRow dr;

            dt.Columns.Add(PromoCampaignHdr.Columns.DateFrom);
            dt.Columns.Add(PromoCampaignHdr.Columns.DateTo);
            dt.Columns.Add(PromoCampaignHdr.Columns.PromoCampaignHdrID);
            dt.Columns.Add(PromoCampaignHdr.Columns.PromoCampaignName);
            dt.Columns.Add(PromoCampaignHdr.Columns.CampaignType);
            dt.Columns.Add(PromoCampaignDet.Columns.MinQuantity);

            for (int i = 0; i < hdr.Count; i++)
            {
                det = hdr[i].PromoCampaignDetRecords();
                for (int j = 0; j < det.Count; j++)
                {
                    dr = dt.NewRow();
                    dr["DateFrom"] = hdr[i].DateFrom.ToString("dd MMM yyyy");
                    dr["DateTo"] = hdr[i].DateTo.ToString("dd MMM yyyy");
                    dr["PromoCampaignHdrID"] = hdr[i].PromoCampaignHdrID;
                    dr["PromoCampaignName"] = hdr[i].PromoCampaignName;
                    dr["CampaignType"] = hdr[i].CampaignType;
                    dr["MinQuantity"] = det[j].MinQuantity;

                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }


        #endregion

        #region "Buy X at the price of Y"

        public const string BuyXatThePriceOfY = "BuyXatThePriceOfY";

        public static bool InsertBuyXatThePriceOfY
            (string CampaignName,
            DateTime startDate,
            DateTime endDate,
            string ItemGroupName,
            int QuantityToBeBought,
            int FreeQty,
            double discount, bool ForNonMembersAlso)
        {
            try
            {
                if (!IsCampaignNameNotUsed(CampaignName))
                    return false;

                //Check whether Group Name already existed....                
                ItemGroup ig = new ItemGroup("ItemGroupName", ItemGroupName);
                if (ig.IsNew)
                {
                    return false;
                }
                //if it is not new, ignore the datatable...

                //This is what the customer is Getting
                PromoCampaignHdr hdr = new PromoCampaignHdr();
                hdr.PromoCampaignName = CampaignName;
                hdr.DateFrom = startDate;
                hdr.DateTo = endDate;
                hdr.FreeQty = FreeQty;
                hdr.Enabled = true;
                hdr.CampaignType = BuyXatThePriceOfY;
                hdr.ForNonMembersAlso = ForNonMembersAlso;
                hdr.Deleted = false;
                hdr.Save(UserInfo.username);

                //This is what the customer is buying
                PromoCampaignDet det = new PromoCampaignDet();

                det.PromoCampaignHdrID = (int)hdr.GetPrimaryKeyValue();
                det.ItemGroupID = (int)ig.GetPrimaryKeyValue();
                det.MinQuantity = QuantityToBeBought;
                det.Deleted = false;
                det.Save(UserInfo.username);
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return false;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public DataTable ViewBuyXatThePriceOfY(DateTime startDate, DateTime endDate)
        {
            PromoCampaignHdrCollection hdr = new PromoCampaignHdrCollection();
            PromoCampaignDetCollection det;
            hdr.Where(PromoCampaignHdr.Columns.DateFrom, Comparison.GreaterOrEquals, startDate);
            hdr.Where(PromoCampaignHdr.Columns.DateFrom, Comparison.LessOrEquals, endDate);
            hdr.Where(PromoCampaignHdr.Columns.CampaignType, BuyXatThePriceOfY);
            hdr.Where(PromoCampaignHdr.Columns.Enabled, true);
            hdr.Load();

            DataTable dt = new DataTable();
            DataRow dr;

            dt.Columns.Add(PromoCampaignHdr.Columns.DateFrom);
            dt.Columns.Add(PromoCampaignHdr.Columns.DateTo);
            dt.Columns.Add(PromoCampaignHdr.Columns.PromoCampaignHdrID);
            dt.Columns.Add(PromoCampaignHdr.Columns.PromoCampaignName);
            dt.Columns.Add(PromoCampaignHdr.Columns.CampaignType);
            /*
            dt.Columns.Add(PromoCampaignDet.Columns.CategoryName);
            dt.Columns.Add(PromoCampaignHdr.Columns.PromoPrice);
            dt.Columns.Add(PromoCampaignHdr.Columns.PromoDiscount);
            */
            for (int i = 0; i < hdr.Count; i++)
            {
                det = new PromoCampaignDetCollection();
                det.Where(PromoCampaignDet.Columns.PromoCampaignHdrID, hdr[i].PromoCampaignHdrID);
                det.Load();
                for (int j = 0; j < det.Count; j++)
                {
                    dr = dt.NewRow();
                    dr["DateFrom"] = hdr[i].DateFrom.ToString("dd MMM yyyy");
                    dr["DateTo"] = hdr[i].DateTo.ToString("dd MMM yyyy");
                    dr["PromoCampaignHdrID"] = hdr[i].PromoCampaignHdrID;
                    dr["PromoCampaignName"] = hdr[i].PromoCampaignName;
                    dr["CampaignType"] = hdr[i].CampaignType;
                    dr["CategoryName"] = det[j].CategoryName;
                    dr["PromoDiscount"] = hdr[i].PromoDiscount;

                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }

        #endregion

        public static void DeletePromoLocationMap(int promoCampaignHdrID, int posID)
        {
            Query qr = PromoLocationMap.CreateQuery();
            qr.QueryType = QueryType.Update;
            qr.AddWhere(PromoLocationMap.Columns.PromoCampaignHdrID, promoCampaignHdrID);
            qr.AddWhere(PromoLocationMap.Columns.PointOfSaleID, posID);
            qr.AddUpdateSetting(PromoLocationMap.Columns.Deleted, true);
            qr.Execute();
        }

        #region "Product Setup Promotion"
        public static DataTable SearchActivePromoWithOutlet(string search, string promoDate, string outletName, string orderBy, string SortDir)
        {
            string query = @"SELECT * 
                        FROM	(
                        SELECT DISTINCT
		                         PCH.PromoCampaignHdrID
		                        ,ISNULL(PCH.PromoCode, '') AS PromoCode
		                        ,PCH.PromoCampaignName
		                        ,ISNULL(PCH.Priority, 0) AS Priority
		                        ,PCH.DateFrom
		                        ,PCH.DateTo
		                        ,PCH.ForNonMembersAlso
		                        ,PCH.PromoPrice
		                        ,PCH.PromoDiscount
		                        ,PO.OutletName
		                        ,ISNULL(PCD.CategoryName,'') AS CategoryName
		                        ,ISNULL(PCD.ItemNo,'') AS ItemNo
		                        ,ISNULL(PCD.UnitQty, 0) AS UnitQty
		                        ,ISNULL(I.ItemName,'') AS ItemName
		                        ,CASE WHEN PCH.PromoPrice > 0 THEN '$ ' + CONVERT(VARCHAR(50), PCH.PromoPrice) 
			                          ELSE CONVERT(varchar(50), PCH.PromoDiscount) + '%' END AS [PriceDiscount]
		                        ,ISNULL(PCH.IsRestricHour, 0) AS IsRestrictHour
		                        ,dbo.GetApplicableDaysPromo(PCH.PromoCampaignHdrID,0) AS ApplicableDays 
                        FROM	PromoCampaignHdr PCH 
		                        INNER JOIN PromoCampaignDet PCD ON PCH.PromoCampaignHdrID = PCD.PromoCampaignHdrID 
		                        LEFT JOIN Item I ON I.ItemNo = PCD.ItemNo 
                                LEFT JOIN AlternateBarcode AB on AB.ItemNo = PCD.ItemNo 
		                        INNER JOIN (
			                        SELECT 
			                          PromoCampaignHdrID,
			                          STUFF((
				                        SELECT ', ' + TAB.OutletName 
				                        FROM (
					                        SELECT   PO.PromoCampaignHdrID
							                        ,OU.OutletName
					                        FROM	PromoOutlet PO
							                        INNER JOIN Outlet OU ON OU.OutletName = PO.OutletName   
					                        WHERE	ISNULL(PO.Deleted,0) = 0 
				                        ) TAB
				                        WHERE (TAB.PromoCampaignHdrID = Results.PromoCampaignHdrID) 
				                        FOR XML PATH(''),TYPE).value('(./text())[1]','NVARCHAR(MAX)')
			                          ,1,2,'') AS OutletName
			                        FROM (
				                        SELECT   PO.PromoCampaignHdrID
						                        ,OU.OutletName
				                        FROM	PromoOutlet PO
						                        INNER JOIN Outlet OU ON OU.OutletName = PO.OutletName
				                        WHERE	ISNULL(PO.Deleted,0) = 0
				                        ) Results
			                        GROUP BY Results.PromoCampaignHdrID		
		                        ) PO ON PO.PromoCampaignHdrID = PCH.PromoCampaignHdrID
                            LEFT JOIN (
			                       SELECT 
	                                    ItemGroupID,
	                                    STUFF((
	                                    SELECT ', ' + TAB.Barcode 
	                                    FROM (
		                                    SELECT   IGM.ItemGroupID 
				                                    ,I.Barcode 
		                                    FROM	ItemGroupMap IGM
				                                    INNER JOIN Item I ON IGM.ItemNo = I.ItemNo 
		                                    WHERE	ISNULL(IGM.Deleted,0) = 0 
	                                    ) TAB
	                                    WHERE (TAB.ItemGroupID = Results.ItemGroupID) 
	                                    FOR XML PATH(''),TYPE).value('(./text())[1]','NVARCHAR(MAX)')
	                                    ,1,2,'') AS ItemGroupList,
	                                    STUFF((
	                                    SELECT ', ' + TAB.ItemName 
	                                    FROM (
		                                    SELECT   IGM.ItemGroupID 
				                                    ,I.ItemName 
		                                    FROM	ItemGroupMap IGM
				                                    INNER JOIN Item I ON IGM.ItemNo = I.ItemNo 
		                                    WHERE	ISNULL(IGM.Deleted,0) = 0 
	                                    ) TAB
	                                    WHERE (TAB.ItemGroupID = Results.ItemGroupID) 
	                                    FOR XML PATH(''),TYPE).value('(./text())[1]','NVARCHAR(MAX)')
	                                    ,1,2,'') AS ItemNameList,
                                        STUFF((
	                                    SELECT ', ' + TAB.Barcode 
	                                    FROM (
		                                    SELECT   IGM.ItemGroupID 
				                                    ,I.Barcode 
		                                    FROM	ItemGroupMap IGM
				                                    INNER JOIN AlternateBarcode I ON IGM.ItemNo = I.ItemNo 
		                                    WHERE	ISNULL(IGM.Deleted,0) = 0 
	                                    ) TAB
	                                    WHERE (TAB.ItemGroupID = Results.ItemGroupID) 
	                                    FOR XML PATH(''),TYPE).value('(./text())[1]','NVARCHAR(MAX)')
	                                    ,1,2,'') AS ItemAlternateBarcodeList
                                    FROM ItemGroup Results
                                    GROUP BY Results.ItemGroupID	
		                        ) IGM ON PCD.ItemGroupID = IGM.ItemGroupID 
                        WHERE	ISNULL(PCH.Deleted,0) = 0 
		                        AND ISNULL(PCD.deleted,0) = 0 
		                        AND (ISNULL(PCH.PromoCode,'') + ' ' + 
			                         PCH.PromoCampaignName + ' ' + 
			                         ISNULL(CONVERT(VARCHAR(50), PCH.PromoPrice), '') + ' ' + 
			                         ISNULL(CONVERT(VARCHAR(50), PCH.PromoDiscount), '')) + ISNULL(I.ItemNo,'') + ' ' + ISNULL(I.ItemName,'') + 
                                     ' ' + ISNULL(I.Barcode,'') + ' ' + ISNULL(IGM.ItemGroupList,'') + ' ' + ISNULL(IGM.ItemAlternateBarcodeList,'') + ' ' + ISNULL(AB.Barcode,'')  LIKE '%' + @Search + '%' 
                                {0} {1}   
                         ) TAB ORDER BY {2} {3}";
            string dateQuery = "";
            if (!string.IsNullOrEmpty(promoDate)) 
            {
                DateTime promodt = DateTime.Parse(promoDate);
                dateQuery = "AND PCH.DateFrom < '" + promodt.ToString("yyyy-MM-dd") + "' AND PCH.DateTo > '" + promodt.ToString("yyyy-MM-dd") + "'";
            }

            string outletQuery = "";
            if(outletName != "-1")
            {
                outletQuery = "AND LTRIM(RTRIM(PO.OutletName)) like '%" + outletName.Trim() + "%'";
            }

            string finalQuery = string.Format(query, dateQuery, outletQuery, orderBy, SortDir);

            QueryCommand cmd = new QueryCommand(finalQuery);
            cmd.Parameters.Add("@Search", search, DbType.String);

            DataTable dt = new DataTable();
            dt.Load(DataService.GetReader(cmd));

            //mark true false to be yes no....
            dt = CommonUILib.DataTableConvertBoolColumnToYesOrNo(dt);
            
            return dt;
        }

        public static bool CheckDuplicatePromoCode(string promoCode, int promoCampaignHdrID) 
        {
            bool objReturn = false;
            if (promoCampaignHdrID == 0)
            {
                /*if new*/
                string query = "select ISNULL(MAX(PromoCampaignHdrID),0) as promocampaignhdrid " +
                                "from PromoCampaignHdr";
                QueryCommand cmd = new QueryCommand(query);
                DataTable dt = new DataTable();
                dt.Load(DataService.GetReader(cmd));

                
                if (dt.Rows[0][0] != null && dt.Rows[0][0].ToString() != "{}")
                    promoCampaignHdrID = Int32.Parse(dt.Rows[0][0].ToString()) +1 ;
            }

            string sqlstring =
                "DECLARE @PromoCode varchar(50) " +
                "Declare @PromoCampaignHdrId int " +
                "set @PromoCode = '" + promoCode + "' " +
                "set @PromoCampaignHdrId = " + promoCampaignHdrID + " " +
                "select PromoCode from PromoCampaignHdr where PromoCode = @PromoCode and ISNULL(Deleted,0) = 0 and PromoCampaignHdrID <> @PromoCampaignHdrId " +
                "union " +
                "Select Barcode from PromoCampaignHdr where Barcode = @PromoCode and ISNULL(Deleted,0) = 0 and PromoCampaignHdrID <> @PromoCampaignHdrId " +
                "union " +
                "Select barcode from Item where Barcode = @PromoCode and ISNULL(Deleted,0) = 0 " +
                "union " +
                "select barcode from AlternateBarcode where Barcode = @PromoCode and ISNULL(Deleted,0) = 0 " +
                "union " +
                "select barcode from ItemGroup where Barcode = @PromoCode and ISNULL(Deleted,0) = 0 ";

            DataTable d = new DataTable();
            d.Load(DataService.GetReader(new QueryCommand(sqlstring)));

            return d.Rows.Count > 0;
        }

        public static bool CheckDuplicateBarcodePromo(string barcode, int promoCampaignHdrID, List<string> outletList)
        {
            bool objReturn = false;
            if (promoCampaignHdrID == 0)
            {
                /*if new*/
                string query = "select ISNULL(MAX(PromoCampaignHdrID),0) as promocampaignhdrid " +
                                "from PromoCampaignHdr";
                QueryCommand cmd = new QueryCommand(query);
                DataTable dt = new DataTable();
                dt.Load(DataService.GetReader(cmd));


                if (dt.Rows[0][0] != null && dt.Rows[0][0].ToString() != "{}")
                    promoCampaignHdrID = Int32.Parse(dt.Rows[0][0].ToString()) + 1;
            }

            string strOutlet = "'" + string.Join("','", outletList.ToArray()) + "'";

            string sqlstring =
                "DECLARE @Barcode varchar(50) " +
                "Declare @PromoCampaignHdrId int " +
                "set @Barcode = '" + barcode + "' " +
                "set @PromoCampaignHdrId = " + promoCampaignHdrID + " " +
                //"select Barcode from PromoCampaignHdr where Barcode = @Barcode and ISNULL(Deleted,0) = 0 and PromoCampaignHdrID <> @PromoCampaignHdrId " +
                "select Barcode from PromoCampaignHdr pch, PromoOutlet po where pch.PromoCampaignHdrID = po.PromoCampaignHdrID and pch.Barcode = @Barcode and ISNULL(pch.Deleted,0) = 0 and ISNULL(po.Deleted,0) = 0 and pch.PromoCampaignHdrID <> @PromoCampaignHdrId and po.OutletName in ({0}) " +
                "union " +
                //"Select PromoCode from PromoCampaignHdr where PromoCode = @Barcode  and ISNULL(Deleted,0) = 0 and promocampaignhdrid <> @PromoCampaignHdrId " +
                "select PromoCode from PromoCampaignHdr pch, PromoOutlet po where pch.PromoCampaignHdrID = po.PromoCampaignHdrID and pch.PromoCode = @Barcode and ISNULL(pch.Deleted,0) = 0 and ISNULL(po.Deleted,0) = 0 and pch.PromoCampaignHdrID <> @PromoCampaignHdrId and po.OutletName in ({0}) " +
                "union " +
                "Select barcode from Item where Barcode = @Barcode  and ISNULL(Deleted,0) = 0 " +
                "union " +
                "select barcode from AlternateBarcode where Barcode = @Barcode  and ISNULL(Deleted,0) = 0 " +
                "union " +
                "select barcode from ItemGroup where Barcode = @Barcode  and ISNULL(Deleted,0) = 0";

            sqlstring = string.Format(sqlstring, strOutlet);
            DataTable d = new DataTable();
            d.Load(DataService.GetReader(new QueryCommand(sqlstring)));

            return d.Rows.Count > 0;
        }

        public static DataTable SearchItemGroup(string search)
        {

            string query = "select distinct h.ItemGroupId, h.ItemGroupname " +
                            "from ItemGroup h " +
                           "WHERE ISNULL(h.Deleted,0) = 0 " +
                            "AND (h.ItemGroupname) LIKE '%' + @Search + '%' ";

            query += "ORDER BY ItemGroupId ";

            QueryCommand cmd = new QueryCommand(query);
            cmd.Parameters.Add("@Search", search, DbType.String);

            DataTable dt = new DataTable();
            dt.Load(DataService.GetReader(cmd));

            return dt;
        }

        public static DataTable SearchItemGroup(string search, bool isAllOutlet, OutletCollection lst)
        {
            QueryCommand cmd;
            Logger.writeLog("SearchItem");
            if (isAllOutlet)
            {
                string query = "select distinct h.ItemGroupId, h.ItemGroupname " +
                                "from ItemGroup h " +
                                @"LEFT JOIN (
			                       SELECT 
	                                    ItemGroupID,
	                                    STUFF((
	                                    SELECT ', ' + TAB.Barcode 
	                                    FROM (
		                                    SELECT   IGM.ItemGroupID 
				                                    ,I.Barcode 
		                                    FROM	ItemGroupMap IGM
				                                    INNER JOIN Item I ON IGM.ItemNo = I.ItemNo 
		                                    WHERE	ISNULL(IGM.Deleted,0) = 0 
	                                    ) TAB
	                                    WHERE (TAB.ItemGroupID = Results.ItemGroupID) 
	                                    FOR XML PATH(''),TYPE).value('(./text())[1]','NVARCHAR(MAX)')
	                                    ,1,2,'') AS ItemGroupList 
                                    FROM ItemGroup Results
                                    GROUP BY Results.ItemGroupID	
		                        ) IGM ON h.ItemGroupID = IGM.ItemGroupID 
                                LEFT JOIN (
		                            SELECT 
	                                    ItemGroupID,
	                                    STUFF((
	                                    SELECT ', ' + TAB.Barcode 
	                                    FROM (
		                                    SELECT   IGM.ItemGroupID 
				                                    ,I.Barcode 
		                                    FROM	ItemGroupMap IGM
				                                    INNER JOIN AlternateBarcode I ON IGM.ItemNo = I.ItemNo 
		                                    WHERE	ISNULL(IGM.Deleted,0) = 0 
	                                    ) TAB
	                                    WHERE (TAB.ItemGroupID = Results.ItemGroupID) 
	                                    FOR XML PATH(''),TYPE).value('(./text())[1]','NVARCHAR(MAX)')
	                                    ,1,2,'') AS ItemGroupList 
                                    FROM ItemGroup Results
                                    GROUP BY Results.ItemGroupID	
	                            ) IGM2 ON h.ItemGroupID = IGM2.ItemGroupID " +
                               "WHERE ISNULL(h.Deleted,0) = 0 " +
                                "AND (h.ItemGroupname) + ' ' + ISNULL(IGM.ItemGroupList,'')  + ' ' + ISNULL(IGM2.ItemGroupList,'') LIKE '%' + @Search + '%' ";

                query += "ORDER BY ItemGroupId ";
                cmd = new QueryCommand(query);
                cmd.Parameters.Add("@Search", search, DbType.String);

                Logger.writeLog(query);

            }
            else
            {
                string outletList = "";
                foreach (Outlet s in lst)
                {
                    outletList += "'" + s.OutletName + "',";
                }
                if (outletList.Length > 0)
                {
                    outletList = outletList.Substring(0, outletList.Length - 1);
                }

                string query = @"select distinct h.ItemGroupId, h.ItemGroupname 
                                    from ItemGroup h " +
                                @"LEFT JOIN (
			                       SELECT 
	                                    ItemGroupID,
	                                    STUFF((
	                                    SELECT ', ' + TAB.Barcode 
	                                    FROM (
		                                    SELECT   IGM.ItemGroupID 
				                                    ,I.Barcode 
		                                    FROM	ItemGroupMap IGM
				                                    INNER JOIN Item I ON IGM.ItemNo = I.ItemNo 
		                                    WHERE	ISNULL(IGM.Deleted,0) = 0 
	                                    ) TAB
	                                    WHERE (TAB.ItemGroupID = Results.ItemGroupID) 
	                                    FOR XML PATH(''),TYPE).value('(./text())[1]','NVARCHAR(MAX)')
	                                    ,1,2,'') AS ItemGroupList 
                                    FROM ItemGroup Results
                                    GROUP BY Results.ItemGroupID	
		                        ) IGM ON h.ItemGroupID = IGM.ItemGroupID 
                                    LEFT JOIN (
		                            SELECT 
	                                    ItemGroupID,
	                                    STUFF((
	                                    SELECT ', ' + TAB.Barcode 
	                                    FROM (
		                                    SELECT   IGM.ItemGroupID 
				                                    ,I.Barcode 
		                                    FROM	ItemGroupMap IGM
				                                    INNER JOIN AlternateBarcode I ON IGM.ItemNo = I.ItemNo 
		                                    WHERE	ISNULL(IGM.Deleted,0) = 0 
	                                    ) TAB
	                                    WHERE (TAB.ItemGroupID = Results.ItemGroupID) 
	                                    FOR XML PATH(''),TYPE).value('(./text())[1]','NVARCHAR(MAX)')
	                                    ,1,2,'') AS ItemGroupList 
                                    FROM ItemGroup Results
                                    GROUP BY Results.ItemGroupID	
	                            ) IGM2 ON h.ItemGroupID = IGM2.ItemGroupID  " +
                                    @"WHERE ISNULL(h.Deleted,0) = 0 
                                    AND (h.ItemGroupname) + ' ' + ISNULL(IGM.ItemGroupList,'')  + ' ' + ISNULL(IGM2.ItemGroupList,'') LIKE '%' + @Search + '%'
	                                AND h.ItemGroupID in (Select ItemGroupID from PromoCampaignHdr ph
	                                LEFT JOIN PromoCampaignDet pd on ph.promocampaignhdrid = pd.promocampaignhdrid and pd.Deleted = 0
	                                LEFT JOIN PromoOutlet po on ph.PromoCampaignHdrID = po.PromoCampaignHdrID and po.Deleted = 0 
	                                WHERE isnull(pd.ItemGroupID,'') <> '' and ph.Deleted = 0 and po.OutletName in (" + outletList + @") )
                                UNION ALL
	                                select distinct h.ItemGroupId, h.ItemGroupname 
                                    from ItemGroup h "
                                    + 
                                    @"LEFT JOIN (
			                           SELECT 
	                                        ItemGroupID,
	                                        STUFF((
	                                        SELECT ', ' + TAB.Barcode 
	                                        FROM (
		                                        SELECT   IGM.ItemGroupID 
				                                        ,I.Barcode 
		                                        FROM	ItemGroupMap IGM
				                                        INNER JOIN Item I ON IGM.ItemNo = I.ItemNo 
		                                        WHERE	ISNULL(IGM.Deleted,0) = 0 
	                                        ) TAB
	                                        WHERE (TAB.ItemGroupID = Results.ItemGroupID) 
	                                        FOR XML PATH(''),TYPE).value('(./text())[1]','NVARCHAR(MAX)')
	                                        ,1,2,'') AS ItemGroupList 
                                        FROM ItemGroup Results
                                        GROUP BY Results.ItemGroupID	
		                            ) IGM ON h.ItemGroupID = IGM.ItemGroupID 
                               LEFT JOIN (
		                            SELECT 
	                                    ItemGroupID,
	                                    STUFF((
	                                    SELECT ', ' + TAB.Barcode 
	                                    FROM (
		                                    SELECT   IGM.ItemGroupID 
				                                    ,I.Barcode 
		                                    FROM	ItemGroupMap IGM
				                                    INNER JOIN AlternateBarcode I ON IGM.ItemNo = I.ItemNo 
		                                    WHERE	ISNULL(IGM.Deleted,0) = 0 
	                                    ) TAB
	                                    WHERE (TAB.ItemGroupID = Results.ItemGroupID) 
	                                    FOR XML PATH(''),TYPE).value('(./text())[1]','NVARCHAR(MAX)')
	                                    ,1,2,'') AS ItemGroupList 
                                    FROM ItemGroup Results
                                    GROUP BY Results.ItemGroupID	
	                            ) IGM2 ON h.ItemGroupID = IGM2.ItemGroupID  " +
                                    @"WHERE ISNULL(h.Deleted,0) = 0 
                                    AND (h.ItemGroupname) + ' ' + ISNULL(IGM.ItemGroupList,'')  + ' ' + ISNULL(IGM2.ItemGroupList,'') LIKE '%' + @Search + '%'
	                                AND h.ItemGroupID not in (Select ItemGroupID from PromoCampaignHdr ph
	                                LEFT JOIN PromoCampaignDet pd on ph.promocampaignhdrid = pd.promocampaignhdrid and pd.Deleted = 0
	                                LEFT JOIN PromoOutlet po on ph.PromoCampaignHdrID = po.PromoCampaignHdrID and po.Deleted = 0 
	                                WHERE isnull(pd.ItemGroupID,'') <> '' and ph.Deleted = 0)";

                query += "ORDER BY ItemGroupId ";
                cmd = new QueryCommand(query);
                cmd.Parameters.Add("@Search", search, DbType.String);

                Logger.writeLog(query);
            }
            
            DataTable dt = new DataTable();
            dt.Load(DataService.GetReader(cmd));

            return dt;
        }

        public static DataTable SearchItemGroupByUsername (string search, string username)
        {
            List<string> ou = new List<string>();
            var outletList = OutletController.FetchByUserNameForReport(false, false, username);

            foreach (Outlet o in outletList)
            { 
                ou.Add("'" + o.OutletName.Replace("'","''") + "'");
            }

            string outlet = string.Join(",", ou.ToArray());

            
            string query = @"--declare @OutletList varchar(max)

                            --select @OutletList = userfld1 from usermst where username = '@Username'
                            
                            Select distinct * 
                            from
                            (
                            select distinct h.ItemGroupId, h.ItemGroupname 
                            from ItemGroup h 
                            WHERE ISNULL(h.Deleted,0) = 0 
                            and h.ItemGroupId not in 
                            (select ItemgroupId
	                            from PromoCampaignDet pd
	                            inner join promocampaignhdr ph on ph.promocampaignhdrid = pd.promocampaignhdrid and ISNULL(ph.Deleted,0) = 0)
                            AND (h.ItemGroupname) LIKE '%' + '{0}' + '%' 
                            UNION
                            select distinct h.ItemGroupId, h.ItemGroupname 
                            from ItemGroup h 
                            WHERE ISNULL(h.Deleted,0) = 0 
                            and h.ItemGroupId in 
                            (select ItemgroupId
	                            from PromoCampaignDet pd
	                            inner join promocampaignhdr ph on ph.promocampaignhdrid = pd.promocampaignhdrid and ISNULL(ph.Deleted,0) = 0
	                            inner join promooutlet po on po.promocampaignhdrid = ph.promocampaignhdrid
	                            where po.OutletName in ({1}))
                            AND (h.ItemGroupname) LIKE '%' + '{0}' + '%' 
                            ) a
                            ORDER BY ItemGroupId 
                            ";


            query = string.Format(query, search, outlet);
            QueryCommand cmd = new QueryCommand(query);
 
            DataTable dt = new DataTable();
            dt.Load(DataService.GetReader(cmd));

            return dt;
        }

        public static bool CheckDuplicateBarcodeItemGroup(string barcode, int ItemGroupID)
        {
            bool objReturn = false;
            if (ItemGroupID == 0)
            {
                /*if new*/
                string query = "select ISNULL(MAX(ItemGroupID),0) as ItemGroupID " +
                                "from ItemGroup";
                QueryCommand cmd = new QueryCommand(query);
                DataTable dt = new DataTable();
                dt.Load(DataService.GetReader(cmd));


                if (dt.Rows[0][0] != null && dt.Rows[0][0].ToString() != "{}")
                    ItemGroupID = Int32.Parse(dt.Rows[0][0].ToString()) + 1;
            }

            string sqlstring =
                "DECLARE @Barcode varchar(50) " +
                "Declare @ItemGroupID int " +
                "set @Barcode = '" + barcode + "' " +
                "set @ItemGroupID = " + ItemGroupID.ToString() + " " +
                "select barcode from ItemGroup where Barcode = @Barcode and ISNULL(Deleted,0) = 0 and ItemGroupID <> @ItemGroupID " +
                "union " +
                "select Barcode from PromoCampaignHdr where Barcode = @Barcode and ISNULL(Deleted,0) = 0 " +
                "union " +
                "Select PromoCode from PromoCampaignHdr where PromoCode = @Barcode and ISNULL(Deleted,0) = 0 " +
                "union " +
                "Select barcode from Item where Barcode = @Barcode  and ISNULL(Deleted,0) = 0 " +
                "union " +
                "select barcode from AlternateBarcode where Barcode = @Barcode  and ISNULL(Deleted,0) = 0 ";

            DataTable d = new DataTable();
            d.Load(DataService.GetReader(new QueryCommand(sqlstring)));

            return d.Rows.Count > 0;
        }

        public static bool CheckDuplicateItemGroupName(string ItemGroupName, int ItemGroupID)
        {
            bool objReturn = false;
            if (ItemGroupID == 0)
            {
                /*if new*/
                string query = "select ISNULL(MAX(ItemGroupID),0) as ItemGroupID " +
                                "from ItemGroup";
                QueryCommand cmd = new QueryCommand(query);
                DataTable dt = new DataTable();
                dt.Load(DataService.GetReader(cmd));


                if (dt.Rows[0][0] != null && dt.Rows[0][0].ToString() != "{}")
                    ItemGroupID = Int32.Parse(dt.Rows[0][0].ToString()) + 1;
            }

            string sqlstring =
                "DECLARE @ItemGroupName varchar(50) " +
                "Declare @ItemGroupID int " +
                "set @ItemGroupName = '" + ItemGroupName + "' " +
                "set @ItemGroupID = " + ItemGroupID.ToString() + " " +
                "select ItemGroupName from ItemGroup where ItemGroupName = @ItemGroupName and ISNULL(Deleted,0) = 0 and ItemGroupID <> @ItemGroupID";

            DataTable d = new DataTable();
            d.Load(DataService.GetReader(new QueryCommand(sqlstring)));

            return d.Rows.Count > 0;
        }

        #endregion
    }
}
