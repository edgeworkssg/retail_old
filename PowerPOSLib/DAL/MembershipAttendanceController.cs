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
    /// Controller class for MembershipAttendance
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class MembershipAttendanceController
    {
        // Preload our schema..
        MembershipAttendance thisSchemaLoad = new MembershipAttendance();
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
        public MembershipAttendanceCollection FetchAll()
        {
            MembershipAttendanceCollection coll = new MembershipAttendanceCollection();
            Query qry = new Query(MembershipAttendance.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public MembershipAttendanceCollection FetchByID(object AttendanceID)
        {
            MembershipAttendanceCollection coll = new MembershipAttendanceCollection().Where("AttendanceID", AttendanceID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public MembershipAttendanceCollection FetchByQuery(Query qry)
        {
            MembershipAttendanceCollection coll = new MembershipAttendanceCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object AttendanceID)
        {
            return (MembershipAttendance.Delete(AttendanceID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object AttendanceID)
        {
            return (MembershipAttendance.Destroy(AttendanceID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string MembershipNo,DateTime ActivityDateTime,string ActivityName,string LockerID,int PointOfSaleID,Guid? UniqueID,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,bool? Deleted,string UserFld1,string UserFld2,string UserFld3,string UserFld4,string UserFld5,bool? UserFlag1,bool? UserFlag2,bool? UserFlag3,bool? UserFlag4,bool? UserFlag5,double? UserFloat1,double? UserFloat2,double? UserFloat3,double? UserFloat4,double? UserFloat5)
	    {
		    MembershipAttendance item = new MembershipAttendance();
		    
            item.MembershipNo = MembershipNo;
            
            item.ActivityDateTime = ActivityDateTime;
            
            item.ActivityName = ActivityName;
            
            item.LockerID = LockerID;
            
            item.PointOfSaleID = PointOfSaleID;
            
            item.UniqueID = UniqueID;
            
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
            
            item.UserFlag1 = UserFlag1;
            
            item.UserFlag2 = UserFlag2;
            
            item.UserFlag3 = UserFlag3;
            
            item.UserFlag4 = UserFlag4;
            
            item.UserFlag5 = UserFlag5;
            
            item.UserFloat1 = UserFloat1;
            
            item.UserFloat2 = UserFloat2;
            
            item.UserFloat3 = UserFloat3;
            
            item.UserFloat4 = UserFloat4;
            
            item.UserFloat5 = UserFloat5;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int AttendanceID,string MembershipNo,DateTime ActivityDateTime,string ActivityName,string LockerID,int PointOfSaleID,Guid? UniqueID,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,bool? Deleted,string UserFld1,string UserFld2,string UserFld3,string UserFld4,string UserFld5,bool? UserFlag1,bool? UserFlag2,bool? UserFlag3,bool? UserFlag4,bool? UserFlag5,double? UserFloat1,double? UserFloat2,double? UserFloat3,double? UserFloat4,double? UserFloat5)
	    {
		    MembershipAttendance item = new MembershipAttendance();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.AttendanceID = AttendanceID;
				
			item.MembershipNo = MembershipNo;
				
			item.ActivityDateTime = ActivityDateTime;
				
			item.ActivityName = ActivityName;
				
			item.LockerID = LockerID;
				
			item.PointOfSaleID = PointOfSaleID;
				
			item.UniqueID = UniqueID;
				
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
				
			item.UserFlag1 = UserFlag1;
				
			item.UserFlag2 = UserFlag2;
				
			item.UserFlag3 = UserFlag3;
				
			item.UserFlag4 = UserFlag4;
				
			item.UserFlag5 = UserFlag5;
				
			item.UserFloat1 = UserFloat1;
				
			item.UserFloat2 = UserFloat2;
				
			item.UserFloat3 = UserFloat3;
				
			item.UserFloat4 = UserFloat4;
				
			item.UserFloat5 = UserFloat5;
				
	        item.Save(UserName);
	    }
    }
}
