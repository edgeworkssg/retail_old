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
    /// Controller class for FeedBackLog
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class FeedBackLogController
    {
        // Preload our schema..
        FeedBackLog thisSchemaLoad = new FeedBackLog();
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
        public FeedBackLogCollection FetchAll()
        {
            FeedBackLogCollection coll = new FeedBackLogCollection();
            Query qry = new Query(FeedBackLog.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public FeedBackLogCollection FetchByID(object FeedBackLogId)
        {
            FeedBackLogCollection coll = new FeedBackLogCollection().Where("FeedBackLogId", FeedBackLogId).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public FeedBackLogCollection FetchByQuery(Query qry)
        {
            FeedBackLogCollection coll = new FeedBackLogCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object FeedBackLogId)
        {
            return (FeedBackLog.Delete(FeedBackLogId) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object FeedBackLogId)
        {
            return (FeedBackLog.Destroy(FeedBackLogId) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(Guid FeedBackLogId,string MembershipNo,string Name,string ContactNo,Guid FeedBackId,string Remark,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,bool? Deleted)
	    {
		    FeedBackLog item = new FeedBackLog();
		    
            item.FeedBackLogId = FeedBackLogId;
            
            item.MembershipNo = MembershipNo;
            
            item.Name = Name;
            
            item.ContactNo = ContactNo;
            
            item.FeedBackId = FeedBackId;
            
            item.Remark = Remark;
            
            item.CreatedOn = CreatedOn;
            
            item.CreatedBy = CreatedBy;
            
            item.ModifiedOn = ModifiedOn;
            
            item.ModifiedBy = ModifiedBy;
            
            item.Deleted = Deleted;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(Guid FeedBackLogId,string MembershipNo,string Name,string ContactNo,Guid FeedBackId,string Remark,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,bool? Deleted)
	    {
		    FeedBackLog item = new FeedBackLog();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.FeedBackLogId = FeedBackLogId;
				
			item.MembershipNo = MembershipNo;
				
			item.Name = Name;
				
			item.ContactNo = ContactNo;
				
			item.FeedBackId = FeedBackId;
				
			item.Remark = Remark;
				
			item.CreatedOn = CreatedOn;
				
			item.CreatedBy = CreatedBy;
				
			item.ModifiedOn = ModifiedOn;
				
			item.ModifiedBy = ModifiedBy;
				
			item.Deleted = Deleted;
				
	        item.Save(UserName);
	    }
    }
}
