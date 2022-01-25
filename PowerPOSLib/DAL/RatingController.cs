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
    /// Controller class for Rating
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class RatingController
    {
        // Preload our schema..
        Rating thisSchemaLoad = new Rating();
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
        public RatingCollection FetchAll()
        {
            RatingCollection coll = new RatingCollection();
            Query qry = new Query(Rating.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public RatingCollection FetchByID(object RatingID)
        {
            RatingCollection coll = new RatingCollection().Where("RatingID", RatingID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public RatingCollection FetchByQuery(Query qry)
        {
            RatingCollection coll = new RatingCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object RatingID)
        {
            return (Rating.Delete(RatingID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object RatingID)
        {
            return (Rating.Destroy(RatingID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(int? Posid,int? RatingX,string Staff,DateTime? Timestamp,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,bool? Deleted,string UniqueId)
	    {
		    Rating item = new Rating();
		    
            item.Posid = Posid;
            
            item.RatingX = RatingX;
            
            item.Staff = Staff;
            
            item.Timestamp = Timestamp;
            
            item.CreatedOn = CreatedOn;
            
            item.CreatedBy = CreatedBy;
            
            item.ModifiedOn = ModifiedOn;
            
            item.ModifiedBy = ModifiedBy;
            
            item.Deleted = Deleted;
            
            item.UniqueId = UniqueId;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int RatingID,int? Posid,int? RatingX,string Staff,DateTime? Timestamp,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,bool? Deleted,string UniqueId)
	    {
		    Rating item = new Rating();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.RatingID = RatingID;
				
			item.Posid = Posid;
				
			item.RatingX = RatingX;
				
			item.Staff = Staff;
				
			item.Timestamp = Timestamp;
				
			item.CreatedOn = CreatedOn;
				
			item.CreatedBy = CreatedBy;
				
			item.ModifiedOn = ModifiedOn;
				
			item.ModifiedBy = ModifiedBy;
				
			item.Deleted = Deleted;
				
			item.UniqueId = UniqueId;
				
	        item.Save(UserName);
	    }
    }
}
