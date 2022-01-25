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
    /// Controller class for Item
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class ItemController
    {
        // Preload our schema..
        Item thisSchemaLoad = new Item();
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
        public ItemCollection FetchAll()
        {
            ItemCollection coll = new ItemCollection();
            Query qry = new Query(Item.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public ItemCollection FetchByID(object ItemNo)
        {
            ItemCollection coll = new ItemCollection().Where("ItemNo", ItemNo).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public ItemCollection FetchByQuery(Query qry)
        {
            ItemCollection coll = new ItemCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object ItemNo)
        {
            return (Item.Delete(ItemNo) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object ItemNo)
        {
            return (Item.Destroy(ItemNo) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string ItemNo,string ItemName,string Barcode,string CategoryName,decimal RetailPrice,decimal FactoryPrice,decimal MinimumPrice,string ItemDesc,bool? IsServiceItem,bool IsInInventory,bool IsNonDiscountable,bool? IsCourse,string CourseTypeID,string Brand,string ProductLine,string Attributes1,string Attributes2,string Attributes3,string Attributes4,string Attributes5,string Attributes6,string Attributes7,string Attributes8,string Remark,DateTime? ProductionDate,bool? IsGST,bool? HasWarranty,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,Guid UniqueID,bool? Deleted,string Userfld1,string Userfld2,string Userfld3,string Userfld4,string Userfld5,string Userfld6,string Userfld7,string Userfld8,string Userfld9,string Userfld10,bool? Userflag1,bool? Userflag2,bool? Userflag3,bool? Userflag4,bool? Userflag5,decimal? Userfloat1,decimal? Userfloat2,decimal? Userfloat3,decimal? Userfloat4,decimal? Userfloat5,int? Userint1,int? Userint2,int? Userint3,int? Userint4,int? Userint5,bool? IsDelivery,int? GSTRule,bool? IsVitaMix,bool? IsWaterFilter,bool? IsYoung,bool? IsJuicePlus,bool? IsCommission,decimal? Userfloat6,decimal? Userfloat7,decimal? Userfloat8,decimal? Userfloat9,decimal? Userfloat10,byte[] ItemImage,decimal? AvgCostPrice,double? BalanceQuantity,bool? Userflag6,bool? Userflag7,bool? Userflag8,bool? Userflag9,bool? Userflag10)
	    {
		    Item item = new Item();
		    
            item.ItemNo = ItemNo;
            
            item.ItemName = ItemName;
            
            item.Barcode = Barcode;
            
            item.CategoryName = CategoryName;
            
            item.RetailPrice = RetailPrice;
            
            item.FactoryPrice = FactoryPrice;
            
            item.MinimumPrice = MinimumPrice;
            
            item.ItemDesc = ItemDesc;
            
            item.IsServiceItem = IsServiceItem;
            
            item.IsInInventory = IsInInventory;
            
            item.IsNonDiscountable = IsNonDiscountable;
            
            item.IsCourse = IsCourse;
            
            item.CourseTypeID = CourseTypeID;
            
            item.Brand = Brand;
            
            item.ProductLine = ProductLine;
            
            item.Attributes1 = Attributes1;
            
            item.Attributes2 = Attributes2;
            
            item.Attributes3 = Attributes3;
            
            item.Attributes4 = Attributes4;
            
            item.Attributes5 = Attributes5;
            
            item.Attributes6 = Attributes6;
            
            item.Attributes7 = Attributes7;
            
            item.Attributes8 = Attributes8;
            
            item.Remark = Remark;
            
            item.ProductionDate = ProductionDate;
            
            item.IsGST = IsGST;
            
            item.HasWarranty = HasWarranty;
            
            item.CreatedOn = CreatedOn;
            
            item.CreatedBy = CreatedBy;
            
            item.ModifiedOn = ModifiedOn;
            
            item.ModifiedBy = ModifiedBy;
            
            item.UniqueID = UniqueID;
            
            item.Deleted = Deleted;
            
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
            
            item.IsDelivery = IsDelivery;
            
            item.GSTRule = GSTRule;
            
            item.IsVitaMix = IsVitaMix;
            
            item.IsWaterFilter = IsWaterFilter;
            
            item.IsYoung = IsYoung;
            
            item.IsJuicePlus = IsJuicePlus;
            
            item.IsCommission = IsCommission;
            
            item.Userfloat6 = Userfloat6;
            
            item.Userfloat7 = Userfloat7;
            
            item.Userfloat8 = Userfloat8;
            
            item.Userfloat9 = Userfloat9;
            
            item.Userfloat10 = Userfloat10;
            
            item.ItemImage = ItemImage;
            
            item.AvgCostPrice = AvgCostPrice;
            
            item.BalanceQuantity = BalanceQuantity;
            
            item.Userflag6 = Userflag6;
            
            item.Userflag7 = Userflag7;
            
            item.Userflag8 = Userflag8;
            
            item.Userflag9 = Userflag9;
            
            item.Userflag10 = Userflag10;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(string ItemNo,string ItemName,string Barcode,string CategoryName,decimal RetailPrice,decimal FactoryPrice,decimal MinimumPrice,string ItemDesc,bool? IsServiceItem,bool IsInInventory,bool IsNonDiscountable,bool? IsCourse,string CourseTypeID,string Brand,string ProductLine,string Attributes1,string Attributes2,string Attributes3,string Attributes4,string Attributes5,string Attributes6,string Attributes7,string Attributes8,string Remark,DateTime? ProductionDate,bool? IsGST,bool? HasWarranty,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,Guid UniqueID,bool? Deleted,string Userfld1,string Userfld2,string Userfld3,string Userfld4,string Userfld5,string Userfld6,string Userfld7,string Userfld8,string Userfld9,string Userfld10,bool? Userflag1,bool? Userflag2,bool? Userflag3,bool? Userflag4,bool? Userflag5,decimal? Userfloat1,decimal? Userfloat2,decimal? Userfloat3,decimal? Userfloat4,decimal? Userfloat5,int? Userint1,int? Userint2,int? Userint3,int? Userint4,int? Userint5,bool? IsDelivery,int? GSTRule,bool? IsVitaMix,bool? IsWaterFilter,bool? IsYoung,bool? IsJuicePlus,bool? IsCommission,decimal? Userfloat6,decimal? Userfloat7,decimal? Userfloat8,decimal? Userfloat9,decimal? Userfloat10,byte[] ItemImage,decimal? AvgCostPrice,double? BalanceQuantity,bool? Userflag6,bool? Userflag7,bool? Userflag8,bool? Userflag9,bool? Userflag10)
	    {
		    Item item = new Item();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.ItemNo = ItemNo;
				
			item.ItemName = ItemName;
				
			item.Barcode = Barcode;
				
			item.CategoryName = CategoryName;
				
			item.RetailPrice = RetailPrice;
				
			item.FactoryPrice = FactoryPrice;
				
			item.MinimumPrice = MinimumPrice;
				
			item.ItemDesc = ItemDesc;
				
			item.IsServiceItem = IsServiceItem;
				
			item.IsInInventory = IsInInventory;
				
			item.IsNonDiscountable = IsNonDiscountable;
				
			item.IsCourse = IsCourse;
				
			item.CourseTypeID = CourseTypeID;
				
			item.Brand = Brand;
				
			item.ProductLine = ProductLine;
				
			item.Attributes1 = Attributes1;
				
			item.Attributes2 = Attributes2;
				
			item.Attributes3 = Attributes3;
				
			item.Attributes4 = Attributes4;
				
			item.Attributes5 = Attributes5;
				
			item.Attributes6 = Attributes6;
				
			item.Attributes7 = Attributes7;
				
			item.Attributes8 = Attributes8;
				
			item.Remark = Remark;
				
			item.ProductionDate = ProductionDate;
				
			item.IsGST = IsGST;
				
			item.HasWarranty = HasWarranty;
				
			item.CreatedOn = CreatedOn;
				
			item.CreatedBy = CreatedBy;
				
			item.ModifiedOn = ModifiedOn;
				
			item.ModifiedBy = ModifiedBy;
				
			item.UniqueID = UniqueID;
				
			item.Deleted = Deleted;
				
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
				
			item.IsDelivery = IsDelivery;
				
			item.GSTRule = GSTRule;
				
			item.IsVitaMix = IsVitaMix;
				
			item.IsWaterFilter = IsWaterFilter;
				
			item.IsYoung = IsYoung;
				
			item.IsJuicePlus = IsJuicePlus;
				
			item.IsCommission = IsCommission;
				
			item.Userfloat6 = Userfloat6;
				
			item.Userfloat7 = Userfloat7;
				
			item.Userfloat8 = Userfloat8;
				
			item.Userfloat9 = Userfloat9;
				
			item.Userfloat10 = Userfloat10;
				
			item.ItemImage = ItemImage;
				
			item.AvgCostPrice = AvgCostPrice;
				
			item.BalanceQuantity = BalanceQuantity;
				
			item.Userflag6 = Userflag6;
				
			item.Userflag7 = Userflag7;
				
			item.Userflag8 = Userflag8;
				
			item.Userflag9 = Userflag9;
				
			item.Userflag10 = Userflag10;
				
	        item.Save(UserName);
	    }
    }
}
