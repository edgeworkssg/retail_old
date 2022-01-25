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
    /// Controller class for AppointmentManager
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class AppointmentManagerController
    {
        // Preload our schema..
        AppointmentManager thisSchemaLoad = new AppointmentManager();
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
        public AppointmentManagerCollection FetchAll()
        {
            AppointmentManagerCollection coll = new AppointmentManagerCollection();
            Query qry = new Query(AppointmentManager.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public AppointmentManagerCollection FetchByID(object AppointmentID)
        {
            AppointmentManagerCollection coll = new AppointmentManagerCollection().Where("AppointmentID", AppointmentID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public AppointmentManagerCollection FetchByQuery(Query qry)
        {
            AppointmentManagerCollection coll = new AppointmentManagerCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object AppointmentID)
        {
            return (AppointmentManager.Delete(AppointmentID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object AppointmentID)
        {
            return (AppointmentManager.Destroy(AppointmentID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string AppointmentDate,string StartTime,string EndTime,string Description,string SalesPersonID)
	    {
		    AppointmentManager item = new AppointmentManager();
		    
            item.AppointmentDate = AppointmentDate;
            
            item.StartTime = StartTime;
            
            item.EndTime = EndTime;
            
            item.Description = Description;
            
            item.SalesPersonID = SalesPersonID;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int AppointmentID,string AppointmentDate,string StartTime,string EndTime,string Description,string SalesPersonID)
	    {
		    AppointmentManager item = new AppointmentManager();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.AppointmentID = AppointmentID;
				
			item.AppointmentDate = AppointmentDate;
				
			item.StartTime = StartTime;
				
			item.EndTime = EndTime;
				
			item.Description = Description;
				
			item.SalesPersonID = SalesPersonID;
				
	        item.Save(UserName);
	    }
    }
}
