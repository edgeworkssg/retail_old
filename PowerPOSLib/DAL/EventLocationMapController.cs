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
    /// Controller class for EventLocationMap
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class EventLocationMapController
    {
        // Preload our schema..
        EventLocationMap thisSchemaLoad = new EventLocationMap();
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
        public EventLocationMapCollection FetchAll()
        {
            EventLocationMapCollection coll = new EventLocationMapCollection();
            Query qry = new Query(EventLocationMap.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public EventLocationMapCollection FetchByID(object EventLocationMapID)
        {
            EventLocationMapCollection coll = new EventLocationMapCollection().Where("EventLocationMapID", EventLocationMapID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public EventLocationMapCollection FetchByQuery(Query qry)
        {
            EventLocationMapCollection coll = new EventLocationMapCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object EventLocationMapID)
        {
            return (EventLocationMap.Delete(EventLocationMapID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object EventLocationMapID)
        {
            return (EventLocationMap.Destroy(EventLocationMapID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(int PointOfSaleID,int EventID,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,bool? Deleted,Guid UniqueID,string UserFld1,string UserFld2,string UserFld3,string UserFld4,string UserFld5,string UserFld6,string UserFld7,string UserFld8,string UserFld9,string UserFld10,decimal? UserFloat1,decimal? UserFloat2,decimal? UserFloat3,decimal? UserFloat4,decimal? UserFloat5,int? UserInt1,int? UserInt2,int? UserInt3,int? UserInt4,int? UserInt5,bool? UserFlag1,bool? UserFlag2,bool? UserFlag3,bool? UserFlag4,bool? UserFlag5)
	    {
		    EventLocationMap item = new EventLocationMap();
		    
            item.PointOfSaleID = PointOfSaleID;
            
            item.EventID = EventID;
            
            item.CreatedOn = CreatedOn;
            
            item.CreatedBy = CreatedBy;
            
            item.ModifiedOn = ModifiedOn;
            
            item.ModifiedBy = ModifiedBy;
            
            item.Deleted = Deleted;
            
            item.UniqueID = UniqueID;
            
            item.UserFld1 = UserFld1;
            
            item.UserFld2 = UserFld2;
            
            item.UserFld3 = UserFld3;
            
            item.UserFld4 = UserFld4;
            
            item.UserFld5 = UserFld5;
            
            item.UserFld6 = UserFld6;
            
            item.UserFld7 = UserFld7;
            
            item.UserFld8 = UserFld8;
            
            item.UserFld9 = UserFld9;
            
            item.UserFld10 = UserFld10;
            
            item.UserFloat1 = UserFloat1;
            
            item.UserFloat2 = UserFloat2;
            
            item.UserFloat3 = UserFloat3;
            
            item.UserFloat4 = UserFloat4;
            
            item.UserFloat5 = UserFloat5;
            
            item.UserInt1 = UserInt1;
            
            item.UserInt2 = UserInt2;
            
            item.UserInt3 = UserInt3;
            
            item.UserInt4 = UserInt4;
            
            item.UserInt5 = UserInt5;
            
            item.UserFlag1 = UserFlag1;
            
            item.UserFlag2 = UserFlag2;
            
            item.UserFlag3 = UserFlag3;
            
            item.UserFlag4 = UserFlag4;
            
            item.UserFlag5 = UserFlag5;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int EventLocationMapID,int PointOfSaleID,int EventID,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,bool? Deleted,Guid UniqueID,string UserFld1,string UserFld2,string UserFld3,string UserFld4,string UserFld5,string UserFld6,string UserFld7,string UserFld8,string UserFld9,string UserFld10,decimal? UserFloat1,decimal? UserFloat2,decimal? UserFloat3,decimal? UserFloat4,decimal? UserFloat5,int? UserInt1,int? UserInt2,int? UserInt3,int? UserInt4,int? UserInt5,bool? UserFlag1,bool? UserFlag2,bool? UserFlag3,bool? UserFlag4,bool? UserFlag5)
	    {
		    EventLocationMap item = new EventLocationMap();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.EventLocationMapID = EventLocationMapID;
				
			item.PointOfSaleID = PointOfSaleID;
				
			item.EventID = EventID;
				
			item.CreatedOn = CreatedOn;
				
			item.CreatedBy = CreatedBy;
				
			item.ModifiedOn = ModifiedOn;
				
			item.ModifiedBy = ModifiedBy;
				
			item.Deleted = Deleted;
				
			item.UniqueID = UniqueID;
				
			item.UserFld1 = UserFld1;
				
			item.UserFld2 = UserFld2;
				
			item.UserFld3 = UserFld3;
				
			item.UserFld4 = UserFld4;
				
			item.UserFld5 = UserFld5;
				
			item.UserFld6 = UserFld6;
				
			item.UserFld7 = UserFld7;
				
			item.UserFld8 = UserFld8;
				
			item.UserFld9 = UserFld9;
				
			item.UserFld10 = UserFld10;
				
			item.UserFloat1 = UserFloat1;
				
			item.UserFloat2 = UserFloat2;
				
			item.UserFloat3 = UserFloat3;
				
			item.UserFloat4 = UserFloat4;
				
			item.UserFloat5 = UserFloat5;
				
			item.UserInt1 = UserInt1;
				
			item.UserInt2 = UserInt2;
				
			item.UserInt3 = UserInt3;
				
			item.UserInt4 = UserInt4;
				
			item.UserInt5 = UserInt5;
				
			item.UserFlag1 = UserFlag1;
				
			item.UserFlag2 = UserFlag2;
				
			item.UserFlag3 = UserFlag3;
				
			item.UserFlag4 = UserFlag4;
				
			item.UserFlag5 = UserFlag5;
				
	        item.Save(UserName);
	    }
    }
}
