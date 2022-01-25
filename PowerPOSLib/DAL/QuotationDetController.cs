using System; 
using System.Text; 
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration; 
using System.Xml; 
using System.Xml.Serialization;
using SubSonic; 
using SubSonic.Utilities;
namespace PowerPOS
{
    /// <summary>
    /// Controller class for QuotationDet
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class QuotationDetController
    {
        // Preload our schema..
        QuotationDet thisSchemaLoad = new QuotationDet();
        private string userName = String.Empty;
        protected string UserName
        {
            get
            {
				if (userName.Length == 0) 
				{
    				if (System.Web.HttpContext.Current != null)
    				{
						userName=System.Web.HttpContext.Current.User.Identity.Name;
					}
					else
					{
						userName=System.Threading.Thread.CurrentPrincipal.Identity.Name;
					}
				}
				return userName;
            }
        }
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public QuotationDetCollection FetchAll()
        {
            QuotationDetCollection coll = new QuotationDetCollection();
            Query qry = new Query(QuotationDet.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public QuotationDetCollection FetchByID(object OrderDetID)
        {
            QuotationDetCollection coll = new QuotationDetCollection().Where("OrderDetID", OrderDetID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public QuotationDetCollection FetchByQuery(Query qry)
        {
            QuotationDetCollection coll = new QuotationDetCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object OrderDetID)
        {
            return (QuotationDet.Delete(OrderDetID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object OrderDetID)
        {
            return (QuotationDet.Destroy(OrderDetID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string OrderDetID,string ItemNo,DateTime OrderDetDate,decimal Quantity,decimal UnitPrice,decimal Discount,decimal Amount,decimal? GrossSales,bool IsFreeOfCharge,decimal? CostOfGoodSold,bool IsPromo,double PromoDiscount,decimal PromoUnitPrice,decimal PromoAmount,bool IsPromoFreeOfCharge,bool? UsePromoPrice,int? PromoHdrID,int? PromoDetID,string VoucherNo,bool IsVoided,bool IsSpecial,string Remark,bool? IsEventPrice,int? SpecialEventID,string OrderHdrID,bool? IsPreOrder,bool IsExchange,bool? IsPendingCollection,bool? GiveCommission,string InventoryHdrRefNo,string ExchangeDetRefNo,decimal OriginalRetailPrice,DateTime? ModifiedOn,string ModifiedBy,string CreatedBy,DateTime? CreatedOn,Guid UniqueID,string Userfld1,string Userfld2,string Userfld3,string Userfld4,string Userfld5,string Userfld6,string Userfld7,string Userfld8,string Userfld9,string Userfld10,bool? Userflag1,bool? Userflag2,bool? Userflag3,bool? Userflag4,bool? Userflag5,decimal? Userfloat1,decimal? Userfloat2,decimal? Userfloat3,decimal? Userfloat4,decimal? Userfloat5,int? Userint1,int? Userint2,int? Userint3,int? Userint4,int? Userint5,decimal? GSTAmount)
	    {
		    QuotationDet item = new QuotationDet();
		    
            item.OrderDetID = OrderDetID;
            
            item.ItemNo = ItemNo;
            
            item.OrderDetDate = OrderDetDate;
            
            item.Quantity = Quantity;
            
            item.UnitPrice = UnitPrice;
            
            item.Discount = Discount;
            
            item.Amount = Amount;
            
            item.GrossSales = GrossSales;
            
            item.IsFreeOfCharge = IsFreeOfCharge;
            
            item.CostOfGoodSold = CostOfGoodSold;
            
            item.IsPromo = IsPromo;
            
            item.PromoDiscount = PromoDiscount;
            
            item.PromoUnitPrice = PromoUnitPrice;
            
            item.PromoAmount = PromoAmount;
            
            item.IsPromoFreeOfCharge = IsPromoFreeOfCharge;
            
            item.UsePromoPrice = UsePromoPrice;
            
            item.PromoHdrID = PromoHdrID;
            
            item.PromoDetID = PromoDetID;
            
            item.VoucherNo = VoucherNo;
            
            item.IsVoided = IsVoided;
            
            item.IsSpecial = IsSpecial;
            
            item.Remark = Remark;
            
            item.IsEventPrice = IsEventPrice;
            
            item.SpecialEventID = SpecialEventID;
            
            item.OrderHdrID = OrderHdrID;
            
            item.IsPreOrder = IsPreOrder;
            
            item.IsExchange = IsExchange;
            
            item.IsPendingCollection = IsPendingCollection;
            
            item.GiveCommission = GiveCommission;
            
            item.InventoryHdrRefNo = InventoryHdrRefNo;
            
            item.ExchangeDetRefNo = ExchangeDetRefNo;
            
            item.OriginalRetailPrice = OriginalRetailPrice;
            
            item.ModifiedOn = ModifiedOn;
            
            item.ModifiedBy = ModifiedBy;
            
            item.CreatedBy = CreatedBy;
            
            item.CreatedOn = CreatedOn;
            
            item.UniqueID = UniqueID;
            
            item.Userfld1 = Userfld1;
            
            item.Userfld2 = Userfld2;
            
            item.Userfld3 = Userfld3;
            
            item.Userfld4 = Userfld4;
            
            item.Userfld5 = Userfld5;
            
            item.Userfld6 = Userfld6;
            
            item.Userfld7 = Userfld7;
            
            item.Userfld8 = Userfld8;
            
            item.Userfld9 = Userfld9;
            
            item.Userfld10 = Userfld10;
            
            item.Userflag1 = Userflag1;
            
            item.Userflag2 = Userflag2;
            
            item.Userflag3 = Userflag3;
            
            item.Userflag4 = Userflag4;
            
            item.Userflag5 = Userflag5;
            
            item.Userfloat1 = Userfloat1;
            
            item.Userfloat2 = Userfloat2;
            
            item.Userfloat3 = Userfloat3;
            
            item.Userfloat4 = Userfloat4;
            
            item.Userfloat5 = Userfloat5;
            
            item.Userint1 = Userint1;
            
            item.Userint2 = Userint2;
            
            item.Userint3 = Userint3;
            
            item.Userint4 = Userint4;
            
            item.Userint5 = Userint5;
            
            item.GSTAmount = GSTAmount;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(string OrderDetID,string ItemNo,DateTime OrderDetDate,decimal Quantity,decimal UnitPrice,decimal Discount,decimal Amount,decimal? GrossSales,bool IsFreeOfCharge,decimal? CostOfGoodSold,bool IsPromo,double PromoDiscount,decimal PromoUnitPrice,decimal PromoAmount,bool IsPromoFreeOfCharge,bool? UsePromoPrice,int? PromoHdrID,int? PromoDetID,string VoucherNo,bool IsVoided,bool IsSpecial,string Remark,bool? IsEventPrice,int? SpecialEventID,string OrderHdrID,bool? IsPreOrder,bool IsExchange,bool? IsPendingCollection,bool? GiveCommission,string InventoryHdrRefNo,string ExchangeDetRefNo,decimal OriginalRetailPrice,DateTime? ModifiedOn,string ModifiedBy,string CreatedBy,DateTime? CreatedOn,Guid UniqueID,string Userfld1,string Userfld2,string Userfld3,string Userfld4,string Userfld5,string Userfld6,string Userfld7,string Userfld8,string Userfld9,string Userfld10,bool? Userflag1,bool? Userflag2,bool? Userflag3,bool? Userflag4,bool? Userflag5,decimal? Userfloat1,decimal? Userfloat2,decimal? Userfloat3,decimal? Userfloat4,decimal? Userfloat5,int? Userint1,int? Userint2,int? Userint3,int? Userint4,int? Userint5,decimal? GSTAmount)
	    {
		    QuotationDet item = new QuotationDet();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.OrderDetID = OrderDetID;
				
			item.ItemNo = ItemNo;
				
			item.OrderDetDate = OrderDetDate;
				
			item.Quantity = Quantity;
				
			item.UnitPrice = UnitPrice;
				
			item.Discount = Discount;
				
			item.Amount = Amount;
				
			item.GrossSales = GrossSales;
				
			item.IsFreeOfCharge = IsFreeOfCharge;
				
			item.CostOfGoodSold = CostOfGoodSold;
				
			item.IsPromo = IsPromo;
				
			item.PromoDiscount = PromoDiscount;
				
			item.PromoUnitPrice = PromoUnitPrice;
				
			item.PromoAmount = PromoAmount;
				
			item.IsPromoFreeOfCharge = IsPromoFreeOfCharge;
				
			item.UsePromoPrice = UsePromoPrice;
				
			item.PromoHdrID = PromoHdrID;
				
			item.PromoDetID = PromoDetID;
				
			item.VoucherNo = VoucherNo;
				
			item.IsVoided = IsVoided;
				
			item.IsSpecial = IsSpecial;
				
			item.Remark = Remark;
				
			item.IsEventPrice = IsEventPrice;
				
			item.SpecialEventID = SpecialEventID;
				
			item.OrderHdrID = OrderHdrID;
				
			item.IsPreOrder = IsPreOrder;
				
			item.IsExchange = IsExchange;
				
			item.IsPendingCollection = IsPendingCollection;
				
			item.GiveCommission = GiveCommission;
				
			item.InventoryHdrRefNo = InventoryHdrRefNo;
				
			item.ExchangeDetRefNo = ExchangeDetRefNo;
				
			item.OriginalRetailPrice = OriginalRetailPrice;
				
			item.ModifiedOn = ModifiedOn;
				
			item.ModifiedBy = ModifiedBy;
				
			item.CreatedBy = CreatedBy;
				
			item.CreatedOn = CreatedOn;
				
			item.UniqueID = UniqueID;
				
			item.Userfld1 = Userfld1;
				
			item.Userfld2 = Userfld2;
				
			item.Userfld3 = Userfld3;
				
			item.Userfld4 = Userfld4;
				
			item.Userfld5 = Userfld5;
				
			item.Userfld6 = Userfld6;
				
			item.Userfld7 = Userfld7;
				
			item.Userfld8 = Userfld8;
				
			item.Userfld9 = Userfld9;
				
			item.Userfld10 = Userfld10;
				
			item.Userflag1 = Userflag1;
				
			item.Userflag2 = Userflag2;
				
			item.Userflag3 = Userflag3;
				
			item.Userflag4 = Userflag4;
				
			item.Userflag5 = Userflag5;
				
			item.Userfloat1 = Userfloat1;
				
			item.Userfloat2 = Userfloat2;
				
			item.Userfloat3 = Userfloat3;
				
			item.Userfloat4 = Userfloat4;
				
			item.Userfloat5 = Userfloat5;
				
			item.Userint1 = Userint1;
				
			item.Userint2 = Userint2;
				
			item.Userint3 = Userint3;
				
			item.Userint4 = Userint4;
				
			item.Userint5 = Userint5;
				
			item.GSTAmount = GSTAmount;
				
	        item.Save(UserName);
	    }
    }
}
