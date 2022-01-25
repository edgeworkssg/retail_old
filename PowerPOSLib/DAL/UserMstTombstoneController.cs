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
    /// Controller class for UserMst_Tombstone
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class UserMstTombstoneController
    {
        // Preload our schema..
        UserMstTombstone thisSchemaLoad = new UserMstTombstone();
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
        public UserMstTombstoneCollection FetchAll()
        {
            UserMstTombstoneCollection coll = new UserMstTombstoneCollection();
            Query qry = new Query(UserMstTombstone.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public UserMstTombstoneCollection FetchByID(object UserName)
        {
            UserMstTombstoneCollection coll = new UserMstTombstoneCollection().Where("UserName", UserName).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public UserMstTombstoneCollection FetchByQuery(Query qry)
        {
            UserMstTombstoneCollection coll = new UserMstTombstoneCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object UserName)
        {
            return (UserMstTombstone.Delete(UserName) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object UserName)
        {
            return (UserMstTombstone.Destroy(UserName) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string UserName,DateTime? DeletionDate)
	    {
		    UserMstTombstone item = new UserMstTombstone();
		    
            item.UserName = UserName;
            
            item.DeletionDate = DeletionDate;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(string UserName,DateTime? DeletionDate)
	    {
		    UserMstTombstone item = new UserMstTombstone();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.UserName = UserName;
				
			item.DeletionDate = DeletionDate;
				
	        item.Save(UserName);
	    }
    }
}
