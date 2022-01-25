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
    /// Controller class for PreOrderRecord
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class PreOrderRecordController
    {
        // Preload our schema..
        PreOrderRecord thisSchemaLoad = new PreOrderRecord();
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
        public PreOrderRecordCollection FetchAll()
        {
            PreOrderRecordCollection coll = new PreOrderRecordCollection();
            Query qry = new Query(PreOrderRecord.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public PreOrderRecordCollection FetchByID(object PreOrderLogID)
        {
            PreOrderRecordCollection coll = new PreOrderRecordCollection().Where("PreOrderLogID", PreOrderLogID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public PreOrderRecordCollection FetchByQuery(Query qry)
        {
            PreOrderRecordCollection coll = new PreOrderRecordCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object PreOrderLogID)
        {
            return (PreOrderRecord.Delete(PreOrderLogID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object PreOrderLogID)
        {
            return (PreOrderRecord.Destroy(PreOrderLogID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string Name,string ContactNo,string CollectionLocation,string Remark,string OrderHdrID,DateTime? CreatedOn,DateTime? ModifiedOn,string CreatedBy,string ModifiedBy,bool? Deleted,Guid UniqueID)
	    {
		    PreOrderRecord item = new PreOrderRecord();
		    
            item.Name = Name;
            
            item.ContactNo = ContactNo;
            
            item.CollectionLocation = CollectionLocation;
            
            item.Remark = Remark;
            
            item.OrderHdrID = OrderHdrID;
            
            item.CreatedOn = CreatedOn;
            
            item.ModifiedOn = ModifiedOn;
            
            item.CreatedBy = CreatedBy;
            
            item.ModifiedBy = ModifiedBy;
            
            item.Deleted = Deleted;
            
            item.UniqueID = UniqueID;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int PreOrderLogID,string Name,string ContactNo,string CollectionLocation,string Remark,string OrderHdrID,DateTime? CreatedOn,DateTime? ModifiedOn,string CreatedBy,string ModifiedBy,bool? Deleted,Guid UniqueID)
	    {
		    PreOrderRecord item = new PreOrderRecord();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.PreOrderLogID = PreOrderLogID;
				
			item.Name = Name;
				
			item.ContactNo = ContactNo;
				
			item.CollectionLocation = CollectionLocation;
				
			item.Remark = Remark;
				
			item.OrderHdrID = OrderHdrID;
				
			item.CreatedOn = CreatedOn;
				
			item.ModifiedOn = ModifiedOn;
				
			item.CreatedBy = CreatedBy;
				
			item.ModifiedBy = ModifiedBy;
				
			item.Deleted = Deleted;
				
			item.UniqueID = UniqueID;
				
	        item.Save(UserName);
	    }
    }
}
