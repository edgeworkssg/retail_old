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
    /// Controller class for StockStaging
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class StockStagingController
    {
        // Preload our schema..
        StockStaging thisSchemaLoad = new StockStaging();
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
        public StockStagingCollection FetchAll()
        {
            StockStagingCollection coll = new StockStagingCollection();
            Query qry = new Query(StockStaging.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public StockStagingCollection FetchByID(object Id)
        {
            StockStagingCollection coll = new StockStagingCollection().Where("ID", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public StockStagingCollection FetchByQuery(Query qry)
        {
            StockStagingCollection coll = new StockStagingCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (StockStaging.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (StockStaging.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(DateTime? InventoryDate,string ItemNo,int? InventoryLocationID,double? BalanceQty,decimal? CostPriceByItem,decimal? CostPriceByItemInvLoc,decimal? CostPriceByItemInvGroup)
	    {
		    StockStaging item = new StockStaging();
		    
            item.InventoryDate = InventoryDate;
            
            item.ItemNo = ItemNo;
            
            item.InventoryLocationID = InventoryLocationID;
            
            item.BalanceQty = BalanceQty;
            
            item.CostPriceByItem = CostPriceByItem;
            
            item.CostPriceByItemInvLoc = CostPriceByItemInvLoc;
            
            item.CostPriceByItemInvGroup = CostPriceByItemInvGroup;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,DateTime? InventoryDate,string ItemNo,int? InventoryLocationID,double? BalanceQty,decimal? CostPriceByItem,decimal? CostPriceByItemInvLoc,decimal? CostPriceByItemInvGroup)
	    {
		    StockStaging item = new StockStaging();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.InventoryDate = InventoryDate;
				
			item.ItemNo = ItemNo;
				
			item.InventoryLocationID = InventoryLocationID;
				
			item.BalanceQty = BalanceQty;
				
			item.CostPriceByItem = CostPriceByItem;
				
			item.CostPriceByItemInvLoc = CostPriceByItemInvLoc;
				
			item.CostPriceByItemInvGroup = CostPriceByItemInvGroup;
				
	        item.Save(UserName);
	    }
    }
}
