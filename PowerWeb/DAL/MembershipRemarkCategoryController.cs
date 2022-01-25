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
    /// Controller class for MembershipRemarkCategory
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class MembershipRemarkCategoryController
    {
        // Preload our schema..
        MembershipRemarkCategory thisSchemaLoad = new MembershipRemarkCategory();
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
        public MembershipRemarkCategoryCollection FetchAll()
        {
            MembershipRemarkCategoryCollection coll = new MembershipRemarkCategoryCollection();
            Query qry = new Query(MembershipRemarkCategory.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public MembershipRemarkCategoryCollection FetchByID(object RemarkCategoryId)
        {
            MembershipRemarkCategoryCollection coll = new MembershipRemarkCategoryCollection().Where("RemarkCategoryId", RemarkCategoryId).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public MembershipRemarkCategoryCollection FetchByQuery(Query qry)
        {
            MembershipRemarkCategoryCollection coll = new MembershipRemarkCategoryCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object RemarkCategoryId)
        {
            return (MembershipRemarkCategory.Delete(RemarkCategoryId) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object RemarkCategoryId)
        {
            return (MembershipRemarkCategory.Destroy(RemarkCategoryId) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string RemarkCategoryName,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,bool Deleted)
	    {
		    MembershipRemarkCategory item = new MembershipRemarkCategory();
		    
            item.RemarkCategoryName = RemarkCategoryName;
            
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
	    public void Update(int RemarkCategoryId,string RemarkCategoryName,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,bool Deleted)
	    {
		    MembershipRemarkCategory item = new MembershipRemarkCategory();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.RemarkCategoryId = RemarkCategoryId;
				
			item.RemarkCategoryName = RemarkCategoryName;
				
			item.CreatedOn = CreatedOn;
				
			item.CreatedBy = CreatedBy;
				
			item.ModifiedOn = ModifiedOn;
				
			item.ModifiedBy = ModifiedBy;
				
			item.Deleted = Deleted;
				
	        item.Save(UserName);
	    }
    }
}
