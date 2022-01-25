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
    /// Controller class for VoucherStatus
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class VoucherStatusController
    {
        // Preload our schema..
        VoucherStatus thisSchemaLoad = new VoucherStatus();
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
        public VoucherStatusCollection FetchAll()
        {
            VoucherStatusCollection coll = new VoucherStatusCollection();
            Query qry = new Query(VoucherStatus.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public VoucherStatusCollection FetchByID(object VoucherStatusId)
        {
            VoucherStatusCollection coll = new VoucherStatusCollection().Where("VoucherStatusId", VoucherStatusId).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public VoucherStatusCollection FetchByQuery(Query qry)
        {
            VoucherStatusCollection coll = new VoucherStatusCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object VoucherStatusId)
        {
            return (VoucherStatus.Delete(VoucherStatusId) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object VoucherStatusId)
        {
            return (VoucherStatus.Destroy(VoucherStatusId) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string StatusName,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,bool? Deleted)
	    {
		    VoucherStatus item = new VoucherStatus();
		    
            item.StatusName = StatusName;
            
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
	    public void Update(int VoucherStatusId,string StatusName,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,bool? Deleted)
	    {
		    VoucherStatus item = new VoucherStatus();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.VoucherStatusId = VoucherStatusId;
				
			item.StatusName = StatusName;
				
			item.CreatedOn = CreatedOn;
				
			item.CreatedBy = CreatedBy;
				
			item.ModifiedOn = ModifiedOn;
				
			item.ModifiedBy = ModifiedBy;
				
			item.Deleted = Deleted;
				
	        item.Save(UserName);
	    }
    }
}
