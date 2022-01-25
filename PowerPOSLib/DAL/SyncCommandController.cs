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
    /// Controller class for SyncCommand
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class SyncCommandController
    {
        // Preload our schema..
        SyncCommand thisSchemaLoad = new SyncCommand();
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
        public SyncCommandCollection FetchAll()
        {
            SyncCommandCollection coll = new SyncCommandCollection();
            Query qry = new Query(SyncCommand.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public SyncCommandCollection FetchByID(object SyncCommandID)
        {
            SyncCommandCollection coll = new SyncCommandCollection().Where("SyncCommandID", SyncCommandID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public SyncCommandCollection FetchByQuery(Query qry)
        {
            SyncCommandCollection coll = new SyncCommandCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object SyncCommandID)
        {
            return (SyncCommand.Delete(SyncCommandID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object SyncCommandID)
        {
            return (SyncCommand.Destroy(SyncCommandID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(Guid SyncCommandID,string Description,byte[] TheCommand,DateTime CreatedOn,string CreatedBy,DateTime? ExecutedOn,string ExecutedBy,string Remarks)
	    {
		    SyncCommand item = new SyncCommand();
		    
            item.SyncCommandID = SyncCommandID;
            
            item.Description = Description;
            
            item.TheCommand = TheCommand;
            
            item.CreatedOn = CreatedOn;
            
            item.CreatedBy = CreatedBy;
            
            item.ExecutedOn = ExecutedOn;
            
            item.ExecutedBy = ExecutedBy;
            
            item.Remarks = Remarks;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(Guid SyncCommandID,string Description,byte[] TheCommand,DateTime CreatedOn,string CreatedBy,DateTime? ExecutedOn,string ExecutedBy,string Remarks)
	    {
		    SyncCommand item = new SyncCommand();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.SyncCommandID = SyncCommandID;
				
			item.Description = Description;
				
			item.TheCommand = TheCommand;
				
			item.CreatedOn = CreatedOn;
				
			item.CreatedBy = CreatedBy;
				
			item.ExecutedOn = ExecutedOn;
				
			item.ExecutedBy = ExecutedBy;
				
			item.Remarks = Remarks;
				
	        item.Save(UserName);
	    }
    }
}
