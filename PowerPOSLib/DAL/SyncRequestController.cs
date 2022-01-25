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
    /// Controller class for SyncRequest
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class SyncRequestController
    {
        // Preload our schema..
        SyncRequest thisSchemaLoad = new SyncRequest();
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
        public SyncRequestCollection FetchAll()
        {
            SyncRequestCollection coll = new SyncRequestCollection();
            Query qry = new Query(SyncRequest.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public SyncRequestCollection FetchByID(object SyncReqID)
        {
            SyncRequestCollection coll = new SyncRequestCollection().Where("SyncReqID", SyncReqID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public SyncRequestCollection FetchByQuery(Query qry)
        {
            SyncRequestCollection coll = new SyncRequestCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object SyncReqID)
        {
            return (SyncRequest.Delete(SyncReqID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object SyncReqID)
        {
            return (SyncRequest.Destroy(SyncReqID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(Guid SyncReqID,DateTime? StartDate,DateTime? EndDate,int PointOfSaleId,bool? Deleted,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy)
	    {
		    SyncRequest item = new SyncRequest();
		    
            item.SyncReqID = SyncReqID;
            
            item.StartDate = StartDate;
            
            item.EndDate = EndDate;
            
            item.PointOfSaleId = PointOfSaleId;
            
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
	    public void Update(Guid SyncReqID,DateTime? StartDate,DateTime? EndDate,int PointOfSaleId,bool? Deleted,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy)
	    {
		    SyncRequest item = new SyncRequest();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.SyncReqID = SyncReqID;
				
			item.StartDate = StartDate;
				
			item.EndDate = EndDate;
				
			item.PointOfSaleId = PointOfSaleId;
				
			item.Deleted = Deleted;
				
			item.CreatedOn = CreatedOn;
				
			item.CreatedBy = CreatedBy;
				
			item.ModifiedOn = ModifiedOn;
				
			item.ModifiedBy = ModifiedBy;
				
	        item.Save(UserName);
	    }
    }
}
