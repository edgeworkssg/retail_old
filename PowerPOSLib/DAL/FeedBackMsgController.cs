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
    /// Controller class for FeedBackMsg
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class FeedBackMsgController
    {
        // Preload our schema..
        FeedBackMsg thisSchemaLoad = new FeedBackMsg();
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
        public FeedBackMsgCollection FetchAll()
        {
            FeedBackMsgCollection coll = new FeedBackMsgCollection();
            Query qry = new Query(FeedBackMsg.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public FeedBackMsgCollection FetchByID(object FeedBackID)
        {
            FeedBackMsgCollection coll = new FeedBackMsgCollection().Where("FeedBackID", FeedBackID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public FeedBackMsgCollection FetchByQuery(Query qry)
        {
            FeedBackMsgCollection coll = new FeedBackMsgCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object FeedBackID)
        {
            return (FeedBackMsg.Delete(FeedBackID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object FeedBackID)
        {
            return (FeedBackMsg.Destroy(FeedBackID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(Guid FeedBackID,string FeedBackMsgX,bool? Deleted,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy)
	    {
		    FeedBackMsg item = new FeedBackMsg();
		    
            item.FeedBackID = FeedBackID;
            
            item.FeedBackMsgX = FeedBackMsgX;
            
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
	    public void Update(Guid FeedBackID,string FeedBackMsgX,bool? Deleted,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy)
	    {
		    FeedBackMsg item = new FeedBackMsg();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.FeedBackID = FeedBackID;
				
			item.FeedBackMsgX = FeedBackMsgX;
				
			item.Deleted = Deleted;
				
			item.CreatedOn = CreatedOn;
				
			item.CreatedBy = CreatedBy;
				
			item.ModifiedOn = ModifiedOn;
				
			item.ModifiedBy = ModifiedBy;
				
	        item.Save(UserName);
	    }
    }
}
