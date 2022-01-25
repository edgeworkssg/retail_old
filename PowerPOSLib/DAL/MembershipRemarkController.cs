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
    /// Controller class for MembershipRemark
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class MembershipRemarkController
    {
        // Preload our schema..
        MembershipRemark thisSchemaLoad = new MembershipRemark();
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
        public MembershipRemarkCollection FetchAll()
        {
            MembershipRemarkCollection coll = new MembershipRemarkCollection();
            Query qry = new Query(MembershipRemark.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public MembershipRemarkCollection FetchByID(object MemberRemarkId)
        {
            MembershipRemarkCollection coll = new MembershipRemarkCollection().Where("MemberRemarkId", MemberRemarkId).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public MembershipRemarkCollection FetchByQuery(Query qry)
        {
            MembershipRemarkCollection coll = new MembershipRemarkCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object MemberRemarkId)
        {
            return (MembershipRemark.Delete(MemberRemarkId) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object MemberRemarkId)
        {
            return (MembershipRemark.Destroy(MemberRemarkId) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string MemberRemarkId,DateTime RemarkDate,string MembershipNo,int RemarkCategoryId,string Remark,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,bool Deleted)
	    {
		    MembershipRemark item = new MembershipRemark();
		    
            item.MemberRemarkId = MemberRemarkId;
            
            item.RemarkDate = RemarkDate;
            
            item.MembershipNo = MembershipNo;
            
            item.RemarkCategoryId = RemarkCategoryId;
            
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
	    public void Update(string MemberRemarkId,DateTime RemarkDate,string MembershipNo,int RemarkCategoryId,string Remark,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,bool Deleted)
	    {
		    MembershipRemark item = new MembershipRemark();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.MemberRemarkId = MemberRemarkId;
				
			item.RemarkDate = RemarkDate;
				
			item.MembershipNo = MembershipNo;
				
			item.RemarkCategoryId = RemarkCategoryId;
				
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
