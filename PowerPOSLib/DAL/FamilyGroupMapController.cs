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
    /// Controller class for FamilyGroupMap
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class FamilyGroupMapController
    {
        // Preload our schema..
        FamilyGroupMap thisSchemaLoad = new FamilyGroupMap();
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
        public FamilyGroupMapCollection FetchAll()
        {
            FamilyGroupMapCollection coll = new FamilyGroupMapCollection();
            Query qry = new Query(FamilyGroupMap.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public FamilyGroupMapCollection FetchByID(object FamilyMembershipMapID)
        {
            FamilyGroupMapCollection coll = new FamilyGroupMapCollection().Where("FamilyMembershipMapID", FamilyMembershipMapID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public FamilyGroupMapCollection FetchByQuery(Query qry)
        {
            FamilyGroupMapCollection coll = new FamilyGroupMapCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object FamilyMembershipMapID)
        {
            return (FamilyGroupMap.Delete(FamilyMembershipMapID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object FamilyMembershipMapID)
        {
            return (FamilyGroupMap.Destroy(FamilyMembershipMapID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string FamilyMembershipMapID,string FamilyID,string MembershipNo,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,bool? Deleted)
	    {
		    FamilyGroupMap item = new FamilyGroupMap();
		    
            item.FamilyMembershipMapID = FamilyMembershipMapID;
            
            item.FamilyID = FamilyID;
            
            item.MembershipNo = MembershipNo;
            
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
	    public void Update(string FamilyMembershipMapID,string FamilyID,string MembershipNo,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,bool? Deleted)
	    {
		    FamilyGroupMap item = new FamilyGroupMap();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.FamilyMembershipMapID = FamilyMembershipMapID;
				
			item.FamilyID = FamilyID;
				
			item.MembershipNo = MembershipNo;
				
			item.CreatedOn = CreatedOn;
				
			item.CreatedBy = CreatedBy;
				
			item.ModifiedOn = ModifiedOn;
				
			item.ModifiedBy = ModifiedBy;
				
			item.Deleted = Deleted;
				
	        item.Save(UserName);
	    }
    }
}
