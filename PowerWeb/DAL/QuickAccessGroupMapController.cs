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
    /// Controller class for QuickAccessGroupMap
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class QuickAccessGroupMapController
    {
        // Preload our schema..
        QuickAccessGroupMap thisSchemaLoad = new QuickAccessGroupMap();
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
        public QuickAccessGroupMapCollection FetchAll()
        {
            QuickAccessGroupMapCollection coll = new QuickAccessGroupMapCollection();
            Query qry = new Query(QuickAccessGroupMap.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public QuickAccessGroupMapCollection FetchByID(object QuickAccessGroupMapID)
        {
            QuickAccessGroupMapCollection coll = new QuickAccessGroupMapCollection().Where("QuickAccessGroupMapID", QuickAccessGroupMapID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public QuickAccessGroupMapCollection FetchByQuery(Query qry)
        {
            QuickAccessGroupMapCollection coll = new QuickAccessGroupMapCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object QuickAccessGroupMapID)
        {
            return (QuickAccessGroupMap.Delete(QuickAccessGroupMapID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object QuickAccessGroupMapID)
        {
            return (QuickAccessGroupMap.Destroy(QuickAccessGroupMapID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(Guid QuickAccessGroupMapID,Guid QuickAccessGroupID,Guid QuickAccessCategoryID,bool? Deleted,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy)
	    {
		    QuickAccessGroupMap item = new QuickAccessGroupMap();
		    
            item.QuickAccessGroupMapID = QuickAccessGroupMapID;
            
            item.QuickAccessGroupID = QuickAccessGroupID;
            
            item.QuickAccessCategoryID = QuickAccessCategoryID;
            
            item.Deleted = Deleted;
            
            item.CreatedOn = CreatedOn;
            
            item.CreatedBy = CreatedBy;
            
            item.ModifiedOn = ModifiedOn;
            
            item.ModifiedBy = ModifiedBy;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(Guid QuickAccessGroupMapID,Guid QuickAccessGroupID,Guid QuickAccessCategoryID,bool? Deleted,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy)
	    {
		    QuickAccessGroupMap item = new QuickAccessGroupMap();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.QuickAccessGroupMapID = QuickAccessGroupMapID;
				
			item.QuickAccessGroupID = QuickAccessGroupID;
				
			item.QuickAccessCategoryID = QuickAccessCategoryID;
				
			item.Deleted = Deleted;
				
			item.CreatedOn = CreatedOn;
				
			item.CreatedBy = CreatedBy;
				
			item.ModifiedOn = ModifiedOn;
				
			item.ModifiedBy = ModifiedBy;
				
	        item.Save(UserName);
	    }
    }
}
