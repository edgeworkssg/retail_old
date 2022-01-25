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
    /// Controller class for AuditLog
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class AuditLogController
    {
        // Preload our schema..
        AuditLog thisSchemaLoad = new AuditLog();
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
        public AuditLogCollection FetchAll()
        {
            AuditLogCollection coll = new AuditLogCollection();
            Query qry = new Query(AuditLog.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public AuditLogCollection FetchByID(object AuditLogID)
        {
            AuditLogCollection coll = new AuditLogCollection().Where("AuditLogID", AuditLogID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public AuditLogCollection FetchByQuery(Query qry)
        {
            AuditLogCollection coll = new AuditLogCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object AuditLogID)
        {
            return (AuditLog.Delete(AuditLogID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object AuditLogID)
        {
            return (AuditLog.Destroy(AuditLogID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(Guid AuditLogID,DateTime? LogDate,string Operation,string TableName,string PrimaryKeyCol,string PrimaryKeyVal,string OldValues,string NewValues,string Remarks)
	    {
		    AuditLog item = new AuditLog();
		    
            item.AuditLogID = AuditLogID;
            
            item.LogDate = LogDate;
            
            item.Operation = Operation;
            
            item.TableName = TableName;
            
            item.PrimaryKeyCol = PrimaryKeyCol;
            
            item.PrimaryKeyVal = PrimaryKeyVal;
            
            item.OldValues = OldValues;
            
            item.NewValues = NewValues;
            
            item.Remarks = Remarks;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(Guid AuditLogID,DateTime? LogDate,string Operation,string TableName,string PrimaryKeyCol,string PrimaryKeyVal,string OldValues,string NewValues,string Remarks)
	    {
		    AuditLog item = new AuditLog();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.AuditLogID = AuditLogID;
				
			item.LogDate = LogDate;
				
			item.Operation = Operation;
				
			item.TableName = TableName;
				
			item.PrimaryKeyCol = PrimaryKeyCol;
				
			item.PrimaryKeyVal = PrimaryKeyVal;
				
			item.OldValues = OldValues;
				
			item.NewValues = NewValues;
				
			item.Remarks = Remarks;
				
	        item.Save(UserName);
	    }
    }
}
