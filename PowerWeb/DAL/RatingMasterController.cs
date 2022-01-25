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
    /// Controller class for RatingMaster
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class RatingMasterController
    {
        // Preload our schema..
        RatingMaster thisSchemaLoad = new RatingMaster();
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
        public RatingMasterCollection FetchAll()
        {
            RatingMasterCollection coll = new RatingMasterCollection();
            Query qry = new Query(RatingMaster.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public RatingMasterCollection FetchByID(object Rating)
        {
            RatingMasterCollection coll = new RatingMasterCollection().Where("Rating", Rating).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public RatingMasterCollection FetchByQuery(Query qry)
        {
            RatingMasterCollection coll = new RatingMasterCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Rating)
        {
            return (RatingMaster.Delete(Rating) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Rating)
        {
            return (RatingMaster.Destroy(Rating) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string RatingName,byte[] RatingImage,string RatingType,int? Weight,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,bool? Deleted,string RatingImageUrl)
	    {
		    RatingMaster item = new RatingMaster();
		    
            item.RatingName = RatingName;
            
            item.RatingImage = RatingImage;
            
            item.RatingType = RatingType;
            
            item.Weight = Weight;
            
            item.CreatedOn = CreatedOn;
            
            item.CreatedBy = CreatedBy;
            
            item.ModifiedOn = ModifiedOn;
            
            item.ModifiedBy = ModifiedBy;
            
            item.Deleted = Deleted;
            
            item.RatingImageUrl = RatingImageUrl;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Rating,string RatingName,byte[] RatingImage,string RatingType,int? Weight,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,bool? Deleted,string RatingImageUrl)
	    {
		    RatingMaster item = new RatingMaster();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Rating = Rating;
				
			item.RatingName = RatingName;
				
			item.RatingImage = RatingImage;
				
			item.RatingType = RatingType;
				
			item.Weight = Weight;
				
			item.CreatedOn = CreatedOn;
				
			item.CreatedBy = CreatedBy;
				
			item.ModifiedOn = ModifiedOn;
				
			item.ModifiedBy = ModifiedBy;
				
			item.Deleted = Deleted;
				
			item.RatingImageUrl = RatingImageUrl;
				
	        item.Save(UserName);
	    }
    }
}
