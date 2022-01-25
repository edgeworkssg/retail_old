using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using SubSonic;

namespace PowerPOS
{
    public partial class WarrantyController
    {
        public static DataTable FetchItems(
            string modelNo, string orderDetId, string membershipNo,
            string itemNo, string itemName, string categoryName,
            string productIdentification, DateTime dateOfPurchase,
            DateTime dateExpiry, string remark,
            string sortColumn, string sortDir)
        {

            ViewWarrantyMembershipItemCollection warranties = new ViewWarrantyMembershipItemCollection();

            if (modelNo != "") warranties.Where(ViewWarrantyMembershipItem.Columns.ModelNo, modelNo);
            if (orderDetId != "") warranties.Where(ViewWarrantyMembershipItem.Columns.OrderDetId, orderDetId);
            if (membershipNo != "") warranties.Where(ViewWarrantyMembershipItem.Columns.MembershipNo, membershipNo);

            if (itemNo != "") warranties.Where(ViewWarrantyMembershipItem.Columns.ItemNo, itemNo);
            if (itemName != "") warranties.Where(ViewWarrantyMembershipItem.Columns.ItemName, itemName);
            if (categoryName != "") warranties.Where(ViewWarrantyMembershipItem.Columns.CategoryName, categoryName);

            if (productIdentification != "") warranties.Where(ViewWarrantyMembershipItem.Columns.ProductIdentification, productIdentification);
            if (dateOfPurchase != null) warranties.Where(ViewWarrantyMembershipItem.Columns.DateOfPurchase, dateOfPurchase);
            if (dateExpiry != null) warranties.Where(ViewWarrantyMembershipItem.Columns.DateExpiry, dateExpiry);
            if (remark != "") warranties.Where(ViewWarrantyMembershipItem.Columns.Remark, SubSonic.Comparison.Like, "%" + remark + "%");

            // Sorting.
            SubSonic.TableSchema.TableColumn t = ViewWarrantyMembershipItem.Schema.GetColumn(sortColumn);
            if (t != null)
            {
                if (sortDir.Trim().ToUpper() == "ASC")
                {
                    warranties.OrderByAsc(sortColumn);
                }
                else if (sortDir.Trim().ToUpper() == "DESC")
                {
                    warranties.OrderByDesc(sortColumn);
                }
            }

            return warranties.Load().ToDataTable();
        }

        public static DataTable FetchWarrantyItemsFromOrder(
            string orderHdrId,
            string sortColumn, 
            string sortDir)
        {

            ViewWarrantyMembershipItemOrderCollection warranties = new 
                ViewWarrantyMembershipItemOrderCollection();

            warranties.Where(ViewWarrantyMembershipItemOrder.Columns.OrderHdrID, orderHdrId);
            
            // Sorting.
            SubSonic.TableSchema.TableColumn t = ViewWarrantyMembershipItem.Schema.GetColumn(sortColumn);
            if (t != null)
            {
                if (sortDir.Trim().ToUpper() == "ASC")
                {
                    warranties.OrderByAsc(sortColumn);
                }
                else if (sortDir.Trim().ToUpper() == "DESC")
                {
                    warranties.OrderByDesc(sortColumn);
                }
            }

            return warranties.Load().ToDataTable();
        }

        public static string InsertWithValidation(
            string SerialNo, string ModelNo, string OrderDetId,
            string MembershipNo, string ItemNo, string ProductIdentification,
            DateTime? DateOfPurchase, DateTime? DateExpiry, string Remark,
            string CreatedBy)
        {
            ItemCollection items = new ItemCollection().Where(Item.Columns.ItemNo, ItemNo).Load();
            if (items.Count <= 0) {
                return "Trying to create warranty information for invalid item";
            }

            MembershipCollection memberships = new MembershipCollection().
                Where(Membership.Columns.MembershipNo, MembershipNo).Load();
            if (memberships.Count <= 0)
            {
                return "Trying to create warranty information for a non existent member";
            }
            
            Warranty warranty = new Warranty();

            warranty.SerialNo = SerialNo;
            warranty.ModelNo = ModelNo;
            warranty.OrderDetId = OrderDetId;
            warranty.MembershipNo = MembershipNo;
            warranty.ItemNo = ItemNo;
            warranty.ProductIdentification = ProductIdentification;
            warranty.DateOfPurchase = DateOfPurchase;
            warranty.DateExpiry = DateExpiry;
            warranty.Remark = Remark;
            warranty.CreatedOn = DateTime.Now;
            warranty.CreatedBy = CreatedBy;
            warranty.ModifiedOn = DateTime.Now;
            warranty.ModifiedBy = CreatedBy;

            warranty.Save(CreatedBy);

            return null;
        }

        public static string UpdateWithValidation(
            string SerialNo, string ModelNo, string OrderDetId,
            string MembershipNo, string ItemNo, string ProductIdentification,
            DateTime? DateOfPurchase, DateTime? DateExpiry, string Remark,
            string ModifiedBy)
        {
            ItemCollection items = new ItemCollection().Where(Item.Columns.ItemNo, ItemNo).Load();
            if (items.Count <= 0)
            {
                return "Trying to create warranty information for invalid item";
            }

            MembershipCollection memberships = new MembershipCollection().Where(Membership.Columns.MembershipNo, MembershipNo).Load();
            if (memberships.Count <= 0)
            {
                return "Trying to create warranty information for a non existent member";
            }

            Query qry = new Query("Warranty");

            qry.AddUpdateSetting(Warranty.Columns.SerialNo, SerialNo);
            qry.AddUpdateSetting(Warranty.Columns.ModelNo, ModelNo);
            qry.AddUpdateSetting(Warranty.Columns.OrderDetId, OrderDetId);
            qry.AddUpdateSetting(Warranty.Columns.MembershipNo, MembershipNo);
            qry.AddUpdateSetting(Warranty.Columns.ItemNo, ItemNo);
            qry.AddUpdateSetting(Warranty.Columns.ProductIdentification, ProductIdentification);
            qry.AddUpdateSetting(Warranty.Columns.DateOfPurchase, DateOfPurchase);
            qry.AddUpdateSetting(Warranty.Columns.DateExpiry, DateExpiry);
            qry.AddUpdateSetting(Warranty.Columns.Remark, Remark);
            qry.AddUpdateSetting(Warranty.Columns.ModifiedOn, DateTime.Now);
            qry.AddUpdateSetting(Warranty.Columns.ModifiedBy, ModifiedBy);

            qry.AddWhere(Warranty.Columns.SerialNo, SerialNo);
            qry.Execute();

            return null;
        }
    }
}
