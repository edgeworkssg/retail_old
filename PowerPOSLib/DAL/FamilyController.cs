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
    /// Controller class for Family
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class FamilyController
    {
        // Preload our schema..
        Family thisSchemaLoad = new Family();
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
        public FamilyCollection FetchAll()
        {
            FamilyCollection coll = new FamilyCollection();
            Query qry = new Query(Family.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public FamilyCollection FetchByID(object FamilyID)
        {
            FamilyCollection coll = new FamilyCollection().Where("FamilyID", FamilyID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public FamilyCollection FetchByQuery(Query qry)
        {
            FamilyCollection coll = new FamilyCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object FamilyID)
        {
            return (Family.Delete(FamilyID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object FamilyID)
        {
            return (Family.Destroy(FamilyID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string FamilyID,string FamilyName,string Remark,DateTime? CreatedOn,DateTime? ModifiedOn,string CreatedBy,string ModifiedBy,bool? Deleted)
	    {
		    Family item = new Family();
		    
            item.FamilyID = FamilyID;
            
            item.FamilyName = FamilyName;
            
            item.Remark = Remark;
            
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
	    public void Update(string FamilyID,string FamilyName,string Remark,DateTime? CreatedOn,DateTime? ModifiedOn,string CreatedBy,string ModifiedBy,bool? Deleted)
	    {
		    Family item = new Family();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.FamilyID = FamilyID;
				
			item.FamilyName = FamilyName;
				
			item.Remark = Remark;
				
			item.CreatedOn = CreatedOn;
				
			item.ModifiedOn = ModifiedOn;
				
			item.CreatedBy = CreatedBy;
				
			item.ModifiedBy = ModifiedBy;
				
			item.Deleted = Deleted;
				
	        item.Save(UserName);
	    }
    }
}
