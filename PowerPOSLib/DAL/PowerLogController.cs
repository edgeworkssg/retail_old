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
    /// Controller class for PowerLog
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class PowerLogController
    {
        // Preload our schema..
        PowerLog thisSchemaLoad = new PowerLog();
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
        public PowerLogCollection FetchAll()
        {
            PowerLogCollection coll = new PowerLogCollection();
            Query qry = new Query(PowerLog.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public PowerLogCollection FetchByID(object LogId)
        {
            PowerLogCollection coll = new PowerLogCollection().Where("LogId", LogId).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public PowerLogCollection FetchByQuery(Query qry)
        {
            PowerLogCollection coll = new PowerLogCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object LogId)
        {
            return (PowerLog.Delete(LogId) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object LogId)
        {
            return (PowerLog.Destroy(LogId) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(DateTime LogDate,string LogMsg)
	    {
		    PowerLog item = new PowerLog();
		    
            item.LogDate = LogDate;
            
            item.LogMsg = LogMsg;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int LogId,DateTime LogDate,string LogMsg)
	    {
		    PowerLog item = new PowerLog();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.LogId = LogId;
				
			item.LogDate = LogDate;
				
			item.LogMsg = LogMsg;
				
	        item.Save(UserName);
	    }
    }
}
