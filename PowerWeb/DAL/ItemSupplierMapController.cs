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
    /// Controller class for ItemSupplierMap
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class ItemSupplierMapController
    {
        // Preload our schema..
        ItemSupplierMap thisSchemaLoad = new ItemSupplierMap();
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
        public ItemSupplierMapCollection FetchAll()
        {
            ItemSupplierMapCollection coll = new ItemSupplierMapCollection();
            Query qry = new Query(ItemSupplierMap.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public ItemSupplierMapCollection FetchByID(object ItemSupplierMapID)
        {
            ItemSupplierMapCollection coll = new ItemSupplierMapCollection().Where("ItemSupplierMapID", ItemSupplierMapID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public ItemSupplierMapCollection FetchByQuery(Query qry)
        {
            ItemSupplierMapCollection coll = new ItemSupplierMapCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object ItemSupplierMapID)
        {
            return (ItemSupplierMap.Delete(ItemSupplierMapID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object ItemSupplierMapID)
        {
            return (ItemSupplierMap.Destroy(ItemSupplierMapID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string ItemNo,int SupplierID,decimal CostPrice,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,bool Deleted,string Currency,decimal? Gst,int? GSTRule,string PackingSize1,string PackingSize2,string PackingSize3,string PackingSize4,string PackingSize5,string PackingSize6,string PackingSize7,string PackingSize8,string PackingSize9,string PackingSize10,decimal? PackingSizeUOM1,decimal? PackingSizeUOM2,decimal? PackingSizeUOM3,decimal? PackingSizeUOM4,decimal? PackingSizeUOM5,decimal? PackingSizeUOM6,decimal? PackingSizeUOM7,decimal? PackingSizeUOM8,decimal? PackingSizeUOM9,decimal? PackingSizeUOM10,decimal? CostPrice1,decimal? CostPrice2,decimal? CostPrice3,decimal? CostPrice4,decimal? CostPrice5,decimal? CostPrice6,decimal? CostPrice7,decimal? CostPrice8,decimal? CostPrice9,decimal? CostPrice10,bool? IsPreferredSupplier)
	    {
		    ItemSupplierMap item = new ItemSupplierMap();
		    
            item.ItemNo = ItemNo;
            
            item.SupplierID = SupplierID;
            
            item.CostPrice = CostPrice;
            
            item.CreatedOn = CreatedOn;
            
            item.CreatedBy = CreatedBy;
            
            item.ModifiedOn = ModifiedOn;
            
            item.ModifiedBy = ModifiedBy;
            
            item.Deleted = Deleted;
            
            item.Currency = Currency;
            
            item.Gst = Gst;
            
            item.GSTRule = GSTRule;
            
            item.PackingSize1 = PackingSize1;
            
            item.PackingSize2 = PackingSize2;
            
            item.PackingSize3 = PackingSize3;
            
            item.PackingSize4 = PackingSize4;
            
            item.PackingSize5 = PackingSize5;
            
            item.PackingSize6 = PackingSize6;
            
            item.PackingSize7 = PackingSize7;
            
            item.PackingSize8 = PackingSize8;
            
            item.PackingSize9 = PackingSize9;
            
            item.PackingSize10 = PackingSize10;
            
            item.PackingSizeUOM1 = PackingSizeUOM1;
            
            item.PackingSizeUOM2 = PackingSizeUOM2;
            
            item.PackingSizeUOM3 = PackingSizeUOM3;
            
            item.PackingSizeUOM4 = PackingSizeUOM4;
            
            item.PackingSizeUOM5 = PackingSizeUOM5;
            
            item.PackingSizeUOM6 = PackingSizeUOM6;
            
            item.PackingSizeUOM7 = PackingSizeUOM7;
            
            item.PackingSizeUOM8 = PackingSizeUOM8;
            
            item.PackingSizeUOM9 = PackingSizeUOM9;
            
            item.PackingSizeUOM10 = PackingSizeUOM10;
            
            item.CostPrice1 = CostPrice1;
            
            item.CostPrice2 = CostPrice2;
            
            item.CostPrice3 = CostPrice3;
            
            item.CostPrice4 = CostPrice4;
            
            item.CostPrice5 = CostPrice5;
            
            item.CostPrice6 = CostPrice6;
            
            item.CostPrice7 = CostPrice7;
            
            item.CostPrice8 = CostPrice8;
            
            item.CostPrice9 = CostPrice9;
            
            item.CostPrice10 = CostPrice10;
            
            item.IsPreferredSupplier = IsPreferredSupplier;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int ItemSupplierMapID,string ItemNo,int SupplierID,decimal CostPrice,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,bool Deleted,string Currency,decimal? Gst,int? GSTRule,string PackingSize1,string PackingSize2,string PackingSize3,string PackingSize4,string PackingSize5,string PackingSize6,string PackingSize7,string PackingSize8,string PackingSize9,string PackingSize10,decimal? PackingSizeUOM1,decimal? PackingSizeUOM2,decimal? PackingSizeUOM3,decimal? PackingSizeUOM4,decimal? PackingSizeUOM5,decimal? PackingSizeUOM6,decimal? PackingSizeUOM7,decimal? PackingSizeUOM8,decimal? PackingSizeUOM9,decimal? PackingSizeUOM10,decimal? CostPrice1,decimal? CostPrice2,decimal? CostPrice3,decimal? CostPrice4,decimal? CostPrice5,decimal? CostPrice6,decimal? CostPrice7,decimal? CostPrice8,decimal? CostPrice9,decimal? CostPrice10,bool? IsPreferredSupplier)
	    {
		    ItemSupplierMap item = new ItemSupplierMap();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.ItemSupplierMapID = ItemSupplierMapID;
				
			item.ItemNo = ItemNo;
				
			item.SupplierID = SupplierID;
				
			item.CostPrice = CostPrice;
				
			item.CreatedOn = CreatedOn;
				
			item.CreatedBy = CreatedBy;
				
			item.ModifiedOn = ModifiedOn;
				
			item.ModifiedBy = ModifiedBy;
				
			item.Deleted = Deleted;
				
			item.Currency = Currency;
				
			item.Gst = Gst;
				
			item.GSTRule = GSTRule;
				
			item.PackingSize1 = PackingSize1;
				
			item.PackingSize2 = PackingSize2;
				
			item.PackingSize3 = PackingSize3;
				
			item.PackingSize4 = PackingSize4;
				
			item.PackingSize5 = PackingSize5;
				
			item.PackingSize6 = PackingSize6;
				
			item.PackingSize7 = PackingSize7;
				
			item.PackingSize8 = PackingSize8;
				
			item.PackingSize9 = PackingSize9;
				
			item.PackingSize10 = PackingSize10;
				
			item.PackingSizeUOM1 = PackingSizeUOM1;
				
			item.PackingSizeUOM2 = PackingSizeUOM2;
				
			item.PackingSizeUOM3 = PackingSizeUOM3;
				
			item.PackingSizeUOM4 = PackingSizeUOM4;
				
			item.PackingSizeUOM5 = PackingSizeUOM5;
				
			item.PackingSizeUOM6 = PackingSizeUOM6;
				
			item.PackingSizeUOM7 = PackingSizeUOM7;
				
			item.PackingSizeUOM8 = PackingSizeUOM8;
				
			item.PackingSizeUOM9 = PackingSizeUOM9;
				
			item.PackingSizeUOM10 = PackingSizeUOM10;
				
			item.CostPrice1 = CostPrice1;
				
			item.CostPrice2 = CostPrice2;
				
			item.CostPrice3 = CostPrice3;
				
			item.CostPrice4 = CostPrice4;
				
			item.CostPrice5 = CostPrice5;
				
			item.CostPrice6 = CostPrice6;
				
			item.CostPrice7 = CostPrice7;
				
			item.CostPrice8 = CostPrice8;
				
			item.CostPrice9 = CostPrice9;
				
			item.CostPrice10 = CostPrice10;
				
			item.IsPreferredSupplier = IsPreferredSupplier;
				
	        item.Save(UserName);
	    }
    }
}
