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
    /// Controller class for ItemDepartment
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class ItemDepartmentController
    {
        // Preload our schema..
        ItemDepartment thisSchemaLoad = new ItemDepartment();
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
        public ItemDepartmentCollection FetchAll()
        {
            ItemDepartmentCollection coll = new ItemDepartmentCollection();
            Query qry = new Query(ItemDepartment.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public ItemDepartmentCollection FetchByID(object ItemDepartmentID)
        {
            ItemDepartmentCollection coll = new ItemDepartmentCollection().Where("ItemDepartmentID", ItemDepartmentID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public ItemDepartmentCollection FetchByQuery(Query qry)
        {
            ItemDepartmentCollection coll = new ItemDepartmentCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object ItemDepartmentID)
        {
            return (ItemDepartment.Delete(ItemDepartmentID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object ItemDepartmentID)
        {
            return (ItemDepartment.Destroy(ItemDepartmentID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string ItemDepartmentID,string DepartmentName,string Remark,DateTime? CreatedOn,DateTime? ModifiedOn,string CreatedBy,string ModifiedBy,bool? Deleted,int? DepartmentOrder)
	    {
		    ItemDepartment item = new ItemDepartment();
		    
            item.ItemDepartmentID = ItemDepartmentID;
            
            item.DepartmentName = DepartmentName;
            
            item.Remark = Remark;
            
            item.CreatedOn = CreatedOn;
            
            item.ModifiedOn = ModifiedOn;
            
            item.CreatedBy = CreatedBy;
            
            item.ModifiedBy = ModifiedBy;
            
            item.Deleted = Deleted;
            
            item.DepartmentOrder = DepartmentOrder;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(string ItemDepartmentID,string DepartmentName,string Remark,DateTime? CreatedOn,DateTime? ModifiedOn,string CreatedBy,string ModifiedBy,bool? Deleted,int? DepartmentOrder)
	    {
		    ItemDepartment item = new ItemDepartment();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.ItemDepartmentID = ItemDepartmentID;
				
			item.DepartmentName = DepartmentName;
				
			item.Remark = Remark;
				
			item.CreatedOn = CreatedOn;
				
			item.ModifiedOn = ModifiedOn;
				
			item.CreatedBy = CreatedBy;
				
			item.ModifiedBy = ModifiedBy;
				
			item.Deleted = Deleted;
				
			item.DepartmentOrder = DepartmentOrder;
				
	        item.Save(UserName);
	    }
    }
}
