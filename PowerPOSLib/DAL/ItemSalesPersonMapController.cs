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
    /// Controller class for ItemSalesPersonMap
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class ItemSalesPersonMapController
    {
        // Preload our schema..
        ItemSalesPersonMap thisSchemaLoad = new ItemSalesPersonMap();
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
        public ItemSalesPersonMapCollection FetchAll()
        {
            ItemSalesPersonMapCollection coll = new ItemSalesPersonMapCollection();
            Query qry = new Query(ItemSalesPersonMap.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public ItemSalesPersonMapCollection FetchByID(object Id)
        {
            ItemSalesPersonMapCollection coll = new ItemSalesPersonMapCollection().Where("ID", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public ItemSalesPersonMapCollection FetchByQuery(Query qry)
        {
            ItemSalesPersonMapCollection coll = new ItemSalesPersonMapCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (ItemSalesPersonMap.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (ItemSalesPersonMap.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(Guid Id,string SalesPerson,string ItemNo,decimal? Commission,string Remarks,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,bool? Deleted)
	    {
		    ItemSalesPersonMap item = new ItemSalesPersonMap();
		    
            item.Id = Id;
            
            item.SalesPerson = SalesPerson;
            
            item.ItemNo = ItemNo;
            
            item.Commission = Commission;
            
            item.Remarks = Remarks;
            
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
	    public void Update(Guid Id,string SalesPerson,string ItemNo,decimal? Commission,string Remarks,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,bool? Deleted)
	    {
		    ItemSalesPersonMap item = new ItemSalesPersonMap();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.SalesPerson = SalesPerson;
				
			item.ItemNo = ItemNo;
				
			item.Commission = Commission;
				
			item.Remarks = Remarks;
				
			item.CreatedOn = CreatedOn;
				
			item.CreatedBy = CreatedBy;
				
			item.ModifiedOn = ModifiedOn;
				
			item.ModifiedBy = ModifiedBy;
				
			item.Deleted = Deleted;
				
	        item.Save(UserName);
	    }
    }
}
