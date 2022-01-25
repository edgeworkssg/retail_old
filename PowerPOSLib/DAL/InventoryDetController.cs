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
    /// Controller class for InventoryDet
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class InventoryDetController
    {
        // Preload our schema..
        InventoryDet thisSchemaLoad = new InventoryDet();
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
        public InventoryDetCollection FetchAll()
        {
            InventoryDetCollection coll = new InventoryDetCollection();
            Query qry = new Query(InventoryDet.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public InventoryDetCollection FetchByID(object InventoryDetRefNo)
        {
            InventoryDetCollection coll = new InventoryDetCollection().Where("InventoryDetRefNo", InventoryDetRefNo).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public InventoryDetCollection FetchByQuery(Query qry)
        {
            InventoryDetCollection coll = new InventoryDetCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object InventoryDetRefNo)
        {
            return (InventoryDet.Delete(InventoryDetRefNo) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object InventoryDetRefNo)
        {
            return (InventoryDet.Destroy(InventoryDetRefNo) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string InventoryDetRefNo,string ItemNo,string InventoryHdrRefNo,DateTime? ExpiryDate,decimal? Quantity,decimal? RemainingQty,decimal FactoryPrice,double Gst,decimal CostOfGoods,string StockInRefNo,bool IsDiscrepancy,string Remark,decimal? Discount,string CreatedBy,DateTime? CreatedOn,string ModifiedBy,DateTime? ModifiedOn,Guid UniqueID,string Userfld1,string Userfld2,string Userfld3,string Userfld4,string Userfld5,string Userfld6,string Userfld7,string Userfld8,string Userfld9,string Userfld10,bool? Userflag1,bool? Userflag2,bool? Userflag3,bool? Userflag4,bool? Userflag5,decimal? Userfloat1,decimal? Userfloat2,decimal? Userfloat3,decimal? Userfloat4,decimal? Userfloat5,int? Userint1,int? Userint2,int? Userint3,int? Userint4,int? Userint5,int BalanceBefore,int BalanceAfter,bool? Deleted,decimal? CostPriceByItemInvLoc,decimal? CostPriceByItem,double? GSTAmount,decimal? CostPriceByItemInvGroup)
	    {
		    InventoryDet item = new InventoryDet();
		    
            item.InventoryDetRefNo = InventoryDetRefNo;
            
            item.ItemNo = ItemNo;
            
            item.InventoryHdrRefNo = InventoryHdrRefNo;
            
            item.ExpiryDate = ExpiryDate;
            
            item.Quantity = Quantity;
            
            item.RemainingQty = RemainingQty;
            
            item.FactoryPrice = FactoryPrice;
            
            item.Gst = Gst;
            
            item.CostOfGoods = CostOfGoods;
            
            item.StockInRefNo = StockInRefNo;
            
            item.IsDiscrepancy = IsDiscrepancy;
            
            item.Remark = Remark;
            
            item.Discount = Discount;
            
            item.CreatedBy = CreatedBy;
            
            item.CreatedOn = CreatedOn;
            
            item.ModifiedBy = ModifiedBy;
            
            item.ModifiedOn = ModifiedOn;
            
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
            
            item.BalanceBefore = BalanceBefore;
            
            item.BalanceAfter = BalanceAfter;
            
            item.Deleted = Deleted;
            
            item.CostPriceByItemInvLoc = CostPriceByItemInvLoc;
            
            item.CostPriceByItem = CostPriceByItem;
            
            item.GSTAmount = GSTAmount;
            
            item.CostPriceByItemInvGroup = CostPriceByItemInvGroup;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(string InventoryDetRefNo,string ItemNo,string InventoryHdrRefNo,DateTime? ExpiryDate,decimal? Quantity,decimal? RemainingQty,decimal FactoryPrice,double Gst,decimal CostOfGoods,string StockInRefNo,bool IsDiscrepancy,string Remark,decimal? Discount,string CreatedBy,DateTime? CreatedOn,string ModifiedBy,DateTime? ModifiedOn,Guid UniqueID,string Userfld1,string Userfld2,string Userfld3,string Userfld4,string Userfld5,string Userfld6,string Userfld7,string Userfld8,string Userfld9,string Userfld10,bool? Userflag1,bool? Userflag2,bool? Userflag3,bool? Userflag4,bool? Userflag5,decimal? Userfloat1,decimal? Userfloat2,decimal? Userfloat3,decimal? Userfloat4,decimal? Userfloat5,int? Userint1,int? Userint2,int? Userint3,int? Userint4,int? Userint5,int BalanceBefore,int BalanceAfter,bool? Deleted,decimal? CostPriceByItemInvLoc,decimal? CostPriceByItem,double? GSTAmount,decimal? CostPriceByItemInvGroup)
	    {
		    InventoryDet item = new InventoryDet();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.InventoryDetRefNo = InventoryDetRefNo;
				
			item.ItemNo = ItemNo;
				
			item.InventoryHdrRefNo = InventoryHdrRefNo;
				
			item.ExpiryDate = ExpiryDate;
				
			item.Quantity = Quantity;
				
			item.RemainingQty = RemainingQty;
				
			item.FactoryPrice = FactoryPrice;
				
			item.Gst = Gst;
				
			item.CostOfGoods = CostOfGoods;
				
			item.StockInRefNo = StockInRefNo;
				
			item.IsDiscrepancy = IsDiscrepancy;
				
			item.Remark = Remark;
				
			item.Discount = Discount;
				
			item.CreatedBy = CreatedBy;
				
			item.CreatedOn = CreatedOn;
				
			item.ModifiedBy = ModifiedBy;
				
			item.ModifiedOn = ModifiedOn;
				
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
				
			item.BalanceBefore = BalanceBefore;
				
			item.BalanceAfter = BalanceAfter;
				
			item.Deleted = Deleted;
				
			item.CostPriceByItemInvLoc = CostPriceByItemInvLoc;
				
			item.CostPriceByItem = CostPriceByItem;
				
			item.GSTAmount = GSTAmount;
				
			item.CostPriceByItemInvGroup = CostPriceByItemInvGroup;
				
	        item.Save(UserName);
	    }
    }
}
