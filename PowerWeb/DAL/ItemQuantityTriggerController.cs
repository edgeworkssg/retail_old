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
    /// Controller class for ItemQuantityTrigger
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class ItemQuantityTriggerController
    {
        // Preload our schema..
        ItemQuantityTrigger thisSchemaLoad = new ItemQuantityTrigger();
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
        public ItemQuantityTriggerCollection FetchAll()
        {
            ItemQuantityTriggerCollection coll = new ItemQuantityTriggerCollection();
            Query qry = new Query(ItemQuantityTrigger.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public ItemQuantityTriggerCollection FetchByID(object TriggerID)
        {
            ItemQuantityTriggerCollection coll = new ItemQuantityTriggerCollection().Where("TriggerID", TriggerID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public ItemQuantityTriggerCollection FetchByQuery(Query qry)
        {
            ItemQuantityTriggerCollection coll = new ItemQuantityTriggerCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object TriggerID)
        {
            return (ItemQuantityTrigger.Delete(TriggerID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object TriggerID)
        {
            return (ItemQuantityTrigger.Destroy(TriggerID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string ItemNo,int TriggerQuantity,string TriggerLevel,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,bool Deleted,int? InventoryLocationID,string Userfld1,string Userfld2,string Userfld3,string Userfld4,string Userfld5,string Userfld6,string Userfld7,string Userfld8,string Userfld9,string Userfld10)
	    {
		    ItemQuantityTrigger item = new ItemQuantityTrigger();
		    
            item.ItemNo = ItemNo;
            
            item.TriggerQuantity = TriggerQuantity;
            
            item.TriggerLevel = TriggerLevel;
            
            item.CreatedOn = CreatedOn;
            
            item.CreatedBy = CreatedBy;
            
            item.ModifiedOn = ModifiedOn;
            
            item.ModifiedBy = ModifiedBy;
            
            item.Deleted = Deleted;
            
            item.InventoryLocationID = InventoryLocationID;
            
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
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int TriggerID,string ItemNo,int TriggerQuantity,string TriggerLevel,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,bool Deleted,int? InventoryLocationID,string Userfld1,string Userfld2,string Userfld3,string Userfld4,string Userfld5,string Userfld6,string Userfld7,string Userfld8,string Userfld9,string Userfld10)
	    {
		    ItemQuantityTrigger item = new ItemQuantityTrigger();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.TriggerID = TriggerID;
				
			item.ItemNo = ItemNo;
				
			item.TriggerQuantity = TriggerQuantity;
				
			item.TriggerLevel = TriggerLevel;
				
			item.CreatedOn = CreatedOn;
				
			item.CreatedBy = CreatedBy;
				
			item.ModifiedOn = ModifiedOn;
				
			item.ModifiedBy = ModifiedBy;
				
			item.Deleted = Deleted;
				
			item.InventoryLocationID = InventoryLocationID;
				
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
				
	        item.Save(UserName);
	    }
    }
}
