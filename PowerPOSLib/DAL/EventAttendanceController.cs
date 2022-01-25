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
    /// Controller class for EventAttendance
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class EventAttendanceController
    {
        // Preload our schema..
        EventAttendance thisSchemaLoad = new EventAttendance();
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
        public EventAttendanceCollection FetchAll()
        {
            EventAttendanceCollection coll = new EventAttendanceCollection();
            Query qry = new Query(EventAttendance.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public EventAttendanceCollection FetchByID(object EventAttendanceID)
        {
            EventAttendanceCollection coll = new EventAttendanceCollection().Where("EventAttendanceID", EventAttendanceID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public EventAttendanceCollection FetchByQuery(Query qry)
        {
            EventAttendanceCollection coll = new EventAttendanceCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object EventAttendanceID)
        {
            return (EventAttendance.Delete(EventAttendanceID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object EventAttendanceID)
        {
            return (EventAttendance.Destroy(EventAttendanceID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(int EventAttendanceID,string MembershipNo,int SpecialEventId,DateTime ArrivalDate,string MovementType,DateTime? CreatedOn,DateTime? CreatedBy,DateTime? ModifiedOn,DateTime? ModifiedBy,bool? Deleted,string UserFld1,string UserFld2,string UserFld3,string UserFld4,string UserFld5)
	    {
		    EventAttendance item = new EventAttendance();
		    
            item.EventAttendanceID = EventAttendanceID;
            
            item.MembershipNo = MembershipNo;
            
            item.SpecialEventId = SpecialEventId;
            
            item.ArrivalDate = ArrivalDate;
            
            item.MovementType = MovementType;
            
            item.CreatedOn = CreatedOn;
            
            item.CreatedBy = CreatedBy;
            
            item.ModifiedOn = ModifiedOn;
            
            item.ModifiedBy = ModifiedBy;
            
            item.Deleted = Deleted;
            
            item.UserFld1 = UserFld1;
            
            item.UserFld2 = UserFld2;
            
            item.UserFld3 = UserFld3;
            
            item.UserFld4 = UserFld4;
            
            item.UserFld5 = UserFld5;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int EventAttendanceID,string MembershipNo,int SpecialEventId,DateTime ArrivalDate,string MovementType,DateTime? CreatedOn,DateTime? CreatedBy,DateTime? ModifiedOn,DateTime? ModifiedBy,bool? Deleted,string UserFld1,string UserFld2,string UserFld3,string UserFld4,string UserFld5)
	    {
		    EventAttendance item = new EventAttendance();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.EventAttendanceID = EventAttendanceID;
				
			item.MembershipNo = MembershipNo;
				
			item.SpecialEventId = SpecialEventId;
				
			item.ArrivalDate = ArrivalDate;
				
			item.MovementType = MovementType;
				
			item.CreatedOn = CreatedOn;
				
			item.CreatedBy = CreatedBy;
				
			item.ModifiedOn = ModifiedOn;
				
			item.ModifiedBy = ModifiedBy;
				
			item.Deleted = Deleted;
				
			item.UserFld1 = UserFld1;
				
			item.UserFld2 = UserFld2;
				
			item.UserFld3 = UserFld3;
				
			item.UserFld4 = UserFld4;
				
			item.UserFld5 = UserFld5;
				
	        item.Save(UserName);
	    }
    }
}
