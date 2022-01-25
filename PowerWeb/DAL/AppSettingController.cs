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
    /// Controller class for AppSetting
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class AppSettingController
    {
        // Preload our schema..
        AppSetting thisSchemaLoad = new AppSetting();
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
        public AppSettingCollection FetchAll()
        {
            AppSettingCollection coll = new AppSettingCollection();
            Query qry = new Query(AppSetting.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public AppSettingCollection FetchByID(object AppSettingId)
        {
            AppSettingCollection coll = new AppSettingCollection().Where("AppSettingId", AppSettingId).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public AppSettingCollection FetchByQuery(Query qry)
        {
            AppSettingCollection coll = new AppSettingCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object AppSettingId)
        {
            return (AppSetting.Delete(AppSettingId) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object AppSettingId)
        {
            return (AppSetting.Destroy(AppSettingId) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(Guid AppSettingId,string AppSettingKey,string AppSettingValue,string OutletName,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy)
	    {
		    AppSetting item = new AppSetting();
		    
            item.AppSettingId = AppSettingId;
            
            item.AppSettingKey = AppSettingKey;
            
            item.AppSettingValue = AppSettingValue;
            
            item.OutletName = OutletName;
            
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
	    public void Update(Guid AppSettingId,string AppSettingKey,string AppSettingValue,string OutletName,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy)
	    {
		    AppSetting item = new AppSetting();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.AppSettingId = AppSettingId;
				
			item.AppSettingKey = AppSettingKey;
				
			item.AppSettingValue = AppSettingValue;
				
			item.OutletName = OutletName;
				
			item.CreatedOn = CreatedOn;
				
			item.CreatedBy = CreatedBy;
				
			item.ModifiedOn = ModifiedOn;
				
			item.ModifiedBy = ModifiedBy;
				
	        item.Save(UserName);
	    }
    }
}
