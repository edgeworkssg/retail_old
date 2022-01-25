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
    /// Controller class for ItemCostPrice
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class ItemCostPriceController
    {
        // Preload our schema..
        ItemCostPrice thisSchemaLoad = new ItemCostPrice();
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
        public ItemCostPriceCollection FetchAll()
        {
            ItemCostPriceCollection coll = new ItemCostPriceCollection();
            Query qry = new Query(ItemCostPrice.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public ItemCostPriceCollection FetchByID(object ItemCostPriceID)
        {
            ItemCostPriceCollection coll = new ItemCostPriceCollection().Where("ItemCostPriceID", ItemCostPriceID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public ItemCostPriceCollection FetchByQuery(Query qry)
        {
            ItemCostPriceCollection coll = new ItemCostPriceCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object ItemCostPriceID)
        {
            return (ItemCostPrice.Delete(ItemCostPriceID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object ItemCostPriceID)
        {
            return (ItemCostPrice.Destroy(ItemCostPriceID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string ItemNo,int CurrencyId,decimal CostPrice,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,bool Deleted)
	    {
		    ItemCostPrice item = new ItemCostPrice();
		    
            item.ItemNo = ItemNo;
            
            item.CurrencyId = CurrencyId;
            
            item.CostPrice = CostPrice;
            
            item.CreatedOn = CreatedOn;
            
            item.CreatedBy = CreatedBy;
            
            item.ModifiedOn = ModifiedOn;
            
            item.ModifiedBy = ModifiedBy;
            
            item.Deleted = Deleted;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int ItemCostPriceID,string ItemNo,int CurrencyId,decimal CostPrice,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,bool Deleted)
	    {
		    ItemCostPrice item = new ItemCostPrice();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.ItemCostPriceID = ItemCostPriceID;
				
			item.ItemNo = ItemNo;
				
			item.CurrencyId = CurrencyId;
				
			item.CostPrice = CostPrice;
				
			item.CreatedOn = CreatedOn;
				
			item.CreatedBy = CreatedBy;
				
			item.ModifiedOn = ModifiedOn;
				
			item.ModifiedBy = ModifiedBy;
				
			item.Deleted = Deleted;
				
	        item.Save(UserName);
	    }
    }
}
