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
    /// Controller class for HotKeysItem
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class HotKeysItemController
    {
        // Preload our schema..
        HotKeysItem thisSchemaLoad = new HotKeysItem();
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
        public HotKeysItemCollection FetchAll()
        {
            HotKeysItemCollection coll = new HotKeysItemCollection();
            Query qry = new Query(HotKeysItem.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public HotKeysItemCollection FetchByID(object KeyCode)
        {
            HotKeysItemCollection coll = new HotKeysItemCollection().Where("KeyCode", KeyCode).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public HotKeysItemCollection FetchByQuery(Query qry)
        {
            HotKeysItemCollection coll = new HotKeysItemCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object KeyCode)
        {
            return (HotKeysItem.Delete(KeyCode) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object KeyCode)
        {
            return (HotKeysItem.Destroy(KeyCode) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string HotKeyID,string KeyCode,string KeyName,string ItemNo,string ItemName,string CreatedBy,DateTime CreatedOn,string ModifiedBy,DateTime ModifiedOn)
	    {
		    HotKeysItem item = new HotKeysItem();
		    
            item.HotKeyID = HotKeyID;
            
            item.KeyCode = KeyCode;
            
            item.KeyName = KeyName;
            
            item.ItemNo = ItemNo;
            
            item.ItemName = ItemName;
            
            item.CreatedBy = CreatedBy;
            
            item.CreatedOn = CreatedOn;
            
            item.ModifiedBy = ModifiedBy;
            
            item.ModifiedOn = ModifiedOn;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(string HotKeyID,string KeyCode,string KeyName,string ItemNo,string ItemName,string CreatedBy,DateTime CreatedOn,string ModifiedBy,DateTime ModifiedOn)
	    {
		    HotKeysItem item = new HotKeysItem();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.HotKeyID = HotKeyID;
				
			item.KeyCode = KeyCode;
				
			item.KeyName = KeyName;
				
			item.ItemNo = ItemNo;
				
			item.ItemName = ItemName;
				
			item.CreatedBy = CreatedBy;
				
			item.CreatedOn = CreatedOn;
				
			item.ModifiedBy = ModifiedBy;
				
			item.ModifiedOn = ModifiedOn;
				
	        item.Save(UserName);
	    }
    }
}
