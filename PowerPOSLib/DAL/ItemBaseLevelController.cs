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
    /// Controller class for ItemBaseLevel
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class ItemBaseLevelController
    {
        // Preload our schema..
        ItemBaseLevel thisSchemaLoad = new ItemBaseLevel();
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
        public ItemBaseLevelCollection FetchAll()
        {
            ItemBaseLevelCollection coll = new ItemBaseLevelCollection();
            Query qry = new Query(ItemBaseLevel.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public ItemBaseLevelCollection FetchByID(object BaseLevelID)
        {
            ItemBaseLevelCollection coll = new ItemBaseLevelCollection().Where("BaseLevelID", BaseLevelID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public ItemBaseLevelCollection FetchByQuery(Query qry)
        {
            ItemBaseLevelCollection coll = new ItemBaseLevelCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object BaseLevelID)
        {
            return (ItemBaseLevel.Delete(BaseLevelID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object BaseLevelID)
        {
            return (ItemBaseLevel.Destroy(BaseLevelID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string ItemNo,int BaseLevelQuantity,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,bool Deleted,int? InventoryLocationID)
	    {
		    ItemBaseLevel item = new ItemBaseLevel();
		    
            item.ItemNo = ItemNo;
            
            item.BaseLevelQuantity = BaseLevelQuantity;
            
            item.CreatedOn = CreatedOn;
            
            item.CreatedBy = CreatedBy;
            
            item.ModifiedOn = ModifiedOn;
            
            item.ModifiedBy = ModifiedBy;
            
            item.Deleted = Deleted;
            
            item.InventoryLocationID = InventoryLocationID;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int BaseLevelID,string ItemNo,int BaseLevelQuantity,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,bool Deleted,int? InventoryLocationID)
	    {
		    ItemBaseLevel item = new ItemBaseLevel();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.BaseLevelID = BaseLevelID;
				
			item.ItemNo = ItemNo;
				
			item.BaseLevelQuantity = BaseLevelQuantity;
				
			item.CreatedOn = CreatedOn;
				
			item.CreatedBy = CreatedBy;
				
			item.ModifiedOn = ModifiedOn;
				
			item.ModifiedBy = ModifiedBy;
				
			item.Deleted = Deleted;
				
			item.InventoryLocationID = InventoryLocationID;
				
	        item.Save(UserName);
	    }
    }
}
