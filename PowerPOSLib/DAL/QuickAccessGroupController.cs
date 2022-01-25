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
    /// Controller class for QuickAccessGroup
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class QuickAccessGroupController
    {
        // Preload our schema..
        QuickAccessGroup thisSchemaLoad = new QuickAccessGroup();
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
        public QuickAccessGroupCollection FetchAll()
        {
            QuickAccessGroupCollection coll = new QuickAccessGroupCollection();
            Query qry = new Query(QuickAccessGroup.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public QuickAccessGroupCollection FetchByID(object QuickAccessGroupId)
        {
            QuickAccessGroupCollection coll = new QuickAccessGroupCollection().Where("QuickAccessGroupId", QuickAccessGroupId).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public QuickAccessGroupCollection FetchByQuery(Query qry)
        {
            QuickAccessGroupCollection coll = new QuickAccessGroupCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object QuickAccessGroupId)
        {
            return (QuickAccessGroup.Delete(QuickAccessGroupId) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object QuickAccessGroupId)
        {
            return (QuickAccessGroup.Destroy(QuickAccessGroupId) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(Guid QuickAccessGroupId,string QuickAccessGroupName,bool? Deleted,DateTime? CreatedOn,DateTime? ModifiedOn,string CreatedBy,string ModifiedBy)
	    {
		    QuickAccessGroup item = new QuickAccessGroup();
		    
            item.QuickAccessGroupId = QuickAccessGroupId;
            
            item.QuickAccessGroupName = QuickAccessGroupName;
            
            item.Deleted = Deleted;
            
            item.CreatedOn = CreatedOn;
            
            item.ModifiedOn = ModifiedOn;
            
            item.CreatedBy = CreatedBy;
            
            item.ModifiedBy = ModifiedBy;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(Guid QuickAccessGroupId,string QuickAccessGroupName,bool? Deleted,DateTime? CreatedOn,DateTime? ModifiedOn,string CreatedBy,string ModifiedBy)
	    {
		    QuickAccessGroup item = new QuickAccessGroup();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.QuickAccessGroupId = QuickAccessGroupId;
				
			item.QuickAccessGroupName = QuickAccessGroupName;
				
			item.Deleted = Deleted;
				
			item.CreatedOn = CreatedOn;
				
			item.ModifiedOn = ModifiedOn;
				
			item.CreatedBy = CreatedBy;
				
			item.ModifiedBy = ModifiedBy;
				
	        item.Save(UserName);
	    }
    }
}
