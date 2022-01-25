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
    /// Controller class for PerformanceLog
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class PerformanceLogController
    {
        // Preload our schema..
        PerformanceLog thisSchemaLoad = new PerformanceLog();
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
        public PerformanceLogCollection FetchAll()
        {
            PerformanceLogCollection coll = new PerformanceLogCollection();
            Query qry = new Query(PerformanceLog.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public PerformanceLogCollection FetchByID(object PerformanceLogID)
        {
            PerformanceLogCollection coll = new PerformanceLogCollection().Where("PerformanceLogID", PerformanceLogID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public PerformanceLogCollection FetchByQuery(Query qry)
        {
            PerformanceLogCollection coll = new PerformanceLogCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object PerformanceLogID)
        {
            return (PerformanceLog.Delete(PerformanceLogID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object PerformanceLogID)
        {
            return (PerformanceLog.Destroy(PerformanceLogID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(Guid PerformanceLogID,string ModuleName,string FunctionName,int PointOfSaleID,decimal ElapsedTime,string PrimaryKeyData,DateTime CreatedOn,DateTime ModifiedOn,string CreatedBy,string ModifiedBy,bool? Deleted)
	    {
		    PerformanceLog item = new PerformanceLog();
		    
            item.PerformanceLogID = PerformanceLogID;
            
            item.ModuleName = ModuleName;
            
            item.FunctionName = FunctionName;
            
            item.PointOfSaleID = PointOfSaleID;
            
            item.ElapsedTime = ElapsedTime;
            
            item.PrimaryKeyData = PrimaryKeyData;
            
            item.CreatedOn = CreatedOn;
            
            item.ModifiedOn = ModifiedOn;
            
            item.CreatedBy = CreatedBy;
            
            item.ModifiedBy = ModifiedBy;
            
            item.Deleted = Deleted;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(Guid PerformanceLogID,string ModuleName,string FunctionName,int PointOfSaleID,decimal ElapsedTime,string PrimaryKeyData,DateTime CreatedOn,DateTime ModifiedOn,string CreatedBy,string ModifiedBy,bool? Deleted)
	    {
		    PerformanceLog item = new PerformanceLog();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.PerformanceLogID = PerformanceLogID;
				
			item.ModuleName = ModuleName;
				
			item.FunctionName = FunctionName;
				
			item.PointOfSaleID = PointOfSaleID;
				
			item.ElapsedTime = ElapsedTime;
				
			item.PrimaryKeyData = PrimaryKeyData;
				
			item.CreatedOn = CreatedOn;
				
			item.ModifiedOn = ModifiedOn;
				
			item.CreatedBy = CreatedBy;
				
			item.ModifiedBy = ModifiedBy;
				
			item.Deleted = Deleted;
				
	        item.Save(UserName);
	    }
    }
}
