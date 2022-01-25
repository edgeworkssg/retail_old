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
    /// Controller class for WarningMsg
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class WarningMsgController
    {
        // Preload our schema..
        WarningMsg thisSchemaLoad = new WarningMsg();
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
        public WarningMsgCollection FetchAll()
        {
            WarningMsgCollection coll = new WarningMsgCollection();
            Query qry = new Query(WarningMsg.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public WarningMsgCollection FetchByID(object UniqueID)
        {
            WarningMsgCollection coll = new WarningMsgCollection().Where("UniqueID", UniqueID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public WarningMsgCollection FetchByQuery(Query qry)
        {
            WarningMsgCollection coll = new WarningMsgCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object UniqueID)
        {
            return (WarningMsg.Delete(UniqueID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object UniqueID)
        {
            return (WarningMsg.Destroy(UniqueID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(Guid UniqueID,string WarningMessage,int PointOfSaleID,string Remark,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,bool? Deleted)
	    {
		    WarningMsg item = new WarningMsg();
		    
            item.UniqueID = UniqueID;
            
            item.WarningMessage = WarningMessage;
            
            item.PointOfSaleID = PointOfSaleID;
            
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
	    public void Update(Guid UniqueID,string WarningMessage,int PointOfSaleID,string Remark,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,bool? Deleted)
	    {
		    WarningMsg item = new WarningMsg();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.UniqueID = UniqueID;
				
			item.WarningMessage = WarningMessage;
				
			item.PointOfSaleID = PointOfSaleID;
				
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
