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
    /// Controller class for EventItemMap
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class EventItemMapController
    {
        // Preload our schema..
        EventItemMap thisSchemaLoad = new EventItemMap();
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
        public EventItemMapCollection FetchAll()
        {
            EventItemMapCollection coll = new EventItemMapCollection();
            Query qry = new Query(EventItemMap.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public EventItemMapCollection FetchByID(object EventItemMapID)
        {
            EventItemMapCollection coll = new EventItemMapCollection().Where("EventItemMapID", EventItemMapID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public EventItemMapCollection FetchByQuery(Query qry)
        {
            EventItemMapCollection coll = new EventItemMapCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object EventItemMapID)
        {
            return (EventItemMap.Delete(EventItemMapID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object EventItemMapID)
        {
            return (EventItemMap.Destroy(EventItemMapID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(int EventId,string ItemNo,decimal? ItemPrice,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,bool? Deleted,Guid UniqueID,string UserFld1,string UserFld2,string UserFld3,string UserFld4,string UserFld5,string UserFld6,string UserFld7,string UserFld8,string UserFld9,string UserFld10,decimal? UserFloat1,decimal? UserFloat2,decimal? UserFloat3,decimal? UserFloat4,decimal? UserFloat5,bool? UserFlag1,bool? UserFlag2,bool? UserFlag3,bool? UserFlag4,bool? UserFlag5,int? UserInt1,int? UserInt2,int? UserInt3,int? UserInt4,int? UserInt5)
	    {
		    EventItemMap item = new EventItemMap();
		    
            item.EventId = EventId;
            
            item.ItemNo = ItemNo;
            
            item.ItemPrice = ItemPrice;
            
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
            
            item.UserFlag1 = UserFlag1;
            
            item.UserFlag2 = UserFlag2;
            
            item.UserFlag3 = UserFlag3;
            
            item.UserFlag4 = UserFlag4;
            
            item.UserFlag5 = UserFlag5;
            
            item.UserInt1 = UserInt1;
            
            item.UserInt2 = UserInt2;
            
            item.UserInt3 = UserInt3;
            
            item.UserInt4 = UserInt4;
            
            item.UserInt5 = UserInt5;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int EventItemMapID,int EventId,string ItemNo,decimal? ItemPrice,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,bool? Deleted,Guid UniqueID,string UserFld1,string UserFld2,string UserFld3,string UserFld4,string UserFld5,string UserFld6,string UserFld7,string UserFld8,string UserFld9,string UserFld10,decimal? UserFloat1,decimal? UserFloat2,decimal? UserFloat3,decimal? UserFloat4,decimal? UserFloat5,bool? UserFlag1,bool? UserFlag2,bool? UserFlag3,bool? UserFlag4,bool? UserFlag5,int? UserInt1,int? UserInt2,int? UserInt3,int? UserInt4,int? UserInt5)
	    {
		    EventItemMap item = new EventItemMap();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.EventItemMapID = EventItemMapID;
				
			item.EventId = EventId;
				
			item.ItemNo = ItemNo;
				
			item.ItemPrice = ItemPrice;
				
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
				
			item.UserFlag1 = UserFlag1;
				
			item.UserFlag2 = UserFlag2;
				
			item.UserFlag3 = UserFlag3;
				
			item.UserFlag4 = UserFlag4;
				
			item.UserFlag5 = UserFlag5;
				
			item.UserInt1 = UserInt1;
				
			item.UserInt2 = UserInt2;
				
			item.UserInt3 = UserInt3;
				
			item.UserInt4 = UserInt4;
				
			item.UserInt5 = UserInt5;
				
	        item.Save(UserName);
	    }
    }
}
