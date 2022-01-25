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
    /// Controller class for AppSetting_Tombstone
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class AppSettingTombstoneController
    {
        // Preload our schema..
        AppSettingTombstone thisSchemaLoad = new AppSettingTombstone();
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
        public AppSettingTombstoneCollection FetchAll()
        {
            AppSettingTombstoneCollection coll = new AppSettingTombstoneCollection();
            Query qry = new Query(AppSettingTombstone.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public AppSettingTombstoneCollection FetchByID(object AppSettingId)
        {
            AppSettingTombstoneCollection coll = new AppSettingTombstoneCollection().Where("AppSettingId", AppSettingId).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public AppSettingTombstoneCollection FetchByQuery(Query qry)
        {
            AppSettingTombstoneCollection coll = new AppSettingTombstoneCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object AppSettingId)
        {
            return (AppSettingTombstone.Delete(AppSettingId) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object AppSettingId)
        {
            return (AppSettingTombstone.Destroy(AppSettingId) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(Guid AppSettingId,DateTime? DeletionDate)
	    {
		    AppSettingTombstone item = new AppSettingTombstone();
		    
            item.AppSettingId = AppSettingId;
            
            item.DeletionDate = DeletionDate;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(Guid AppSettingId,DateTime? DeletionDate)
	    {
		    AppSettingTombstone item = new AppSettingTombstone();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.AppSettingId = AppSettingId;
				
			item.DeletionDate = DeletionDate;
				
	        item.Save(UserName);
	    }
    }
}
