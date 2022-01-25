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
    /// Controller class for PerformanceLogSummary
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class PerformanceLogSummaryController
    {
        // Preload our schema..
        PerformanceLogSummary thisSchemaLoad = new PerformanceLogSummary();
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
        public PerformanceLogSummaryCollection FetchAll()
        {
            PerformanceLogSummaryCollection coll = new PerformanceLogSummaryCollection();
            Query qry = new Query(PerformanceLogSummary.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public PerformanceLogSummaryCollection FetchByID(object PerformanceLogSummaryID)
        {
            PerformanceLogSummaryCollection coll = new PerformanceLogSummaryCollection().Where("PerformanceLogSummaryID", PerformanceLogSummaryID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public PerformanceLogSummaryCollection FetchByQuery(Query qry)
        {
            PerformanceLogSummaryCollection coll = new PerformanceLogSummaryCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object PerformanceLogSummaryID)
        {
            return (PerformanceLogSummary.Delete(PerformanceLogSummaryID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object PerformanceLogSummaryID)
        {
            return (PerformanceLogSummary.Destroy(PerformanceLogSummaryID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(Guid PerformanceLogSummaryID,string ModuleName,string FunctionName,int PointOfSaleID,DateTime TimeStamp,decimal AvgElapsedTime,decimal MinElapsedTime,decimal MaxElapsedTime,int TransCount,DateTime CreatedOn,DateTime ModifiedOn,string CreatedBy,string ModifiedBy,bool? Deleted)
	    {
		    PerformanceLogSummary item = new PerformanceLogSummary();
		    
            item.PerformanceLogSummaryID = PerformanceLogSummaryID;
            
            item.ModuleName = ModuleName;
            
            item.FunctionName = FunctionName;
            
            item.PointOfSaleID = PointOfSaleID;
            
            item.TimeStamp = TimeStamp;
            
            item.AvgElapsedTime = AvgElapsedTime;
            
            item.MinElapsedTime = MinElapsedTime;
            
            item.MaxElapsedTime = MaxElapsedTime;
            
            item.TransCount = TransCount;
            
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
	    public void Update(Guid PerformanceLogSummaryID,string ModuleName,string FunctionName,int PointOfSaleID,DateTime TimeStamp,decimal AvgElapsedTime,decimal MinElapsedTime,decimal MaxElapsedTime,int TransCount,DateTime CreatedOn,DateTime ModifiedOn,string CreatedBy,string ModifiedBy,bool? Deleted)
	    {
		    PerformanceLogSummary item = new PerformanceLogSummary();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.PerformanceLogSummaryID = PerformanceLogSummaryID;
				
			item.ModuleName = ModuleName;
				
			item.FunctionName = FunctionName;
				
			item.PointOfSaleID = PointOfSaleID;
				
			item.TimeStamp = TimeStamp;
				
			item.AvgElapsedTime = AvgElapsedTime;
				
			item.MinElapsedTime = MinElapsedTime;
				
			item.MaxElapsedTime = MaxElapsedTime;
				
			item.TransCount = TransCount;
				
			item.CreatedOn = CreatedOn;
				
			item.ModifiedOn = ModifiedOn;
				
			item.CreatedBy = CreatedBy;
				
			item.ModifiedBy = ModifiedBy;
				
			item.Deleted = Deleted;
				
	        item.Save(UserName);
	    }
    }
}
