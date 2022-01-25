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
    /// Controller class for AppliedPromo_Tombstone
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class AppliedPromoTombstoneController
    {
        // Preload our schema..
        AppliedPromoTombstone thisSchemaLoad = new AppliedPromoTombstone();
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
        public AppliedPromoTombstoneCollection FetchAll()
        {
            AppliedPromoTombstoneCollection coll = new AppliedPromoTombstoneCollection();
            Query qry = new Query(AppliedPromoTombstone.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public AppliedPromoTombstoneCollection FetchByID(object AppliedPromoID)
        {
            AppliedPromoTombstoneCollection coll = new AppliedPromoTombstoneCollection().Where("AppliedPromoID", AppliedPromoID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public AppliedPromoTombstoneCollection FetchByQuery(Query qry)
        {
            AppliedPromoTombstoneCollection coll = new AppliedPromoTombstoneCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object AppliedPromoID)
        {
            return (AppliedPromoTombstone.Delete(AppliedPromoID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object AppliedPromoID)
        {
            return (AppliedPromoTombstone.Destroy(AppliedPromoID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(int AppliedPromoID,DateTime? DeletionDate)
	    {
		    AppliedPromoTombstone item = new AppliedPromoTombstone();
		    
            item.AppliedPromoID = AppliedPromoID;
            
            item.DeletionDate = DeletionDate;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int AppliedPromoID,DateTime? DeletionDate)
	    {
		    AppliedPromoTombstone item = new AppliedPromoTombstone();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.AppliedPromoID = AppliedPromoID;
				
			item.DeletionDate = DeletionDate;
				
	        item.Save(UserName);
	    }
    }
}
