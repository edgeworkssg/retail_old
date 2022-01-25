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
    /// Controller class for Appointment
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class AppointmentController
    {
        // Preload our schema..
        Appointment thisSchemaLoad = new Appointment();
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
        public AppointmentCollection FetchAll()
        {
            AppointmentCollection coll = new AppointmentCollection();
            Query qry = new Query(Appointment.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public AppointmentCollection FetchByID(object Id)
        {
            AppointmentCollection coll = new AppointmentCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public AppointmentCollection FetchByQuery(Query qry)
        {
            AppointmentCollection coll = new AppointmentCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (Appointment.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (Appointment.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(Guid Id,DateTime StartTime,int Duration,string Description,int BackColor,int FontColor,string SalesPersonID,string MembershipNo,string OrderHdrID,string CreatedBy,DateTime? CreatedOn,string ModifiedBy,DateTime? ModifiedOn,bool? Deleted,string Organization,string PickUpLocation,int? NoOfChildren,int? PointOfSaleID,string ResourceID,string CheckInByWho,string CheckOutByWho,DateTime? CheckInTime,DateTime? CheckOutTime,string Remark,bool? IsServerUpdate,int? TimeExtension)
	    {
		    Appointment item = new Appointment();
		    
            item.Id = Id;
            
            item.StartTime = StartTime;
            
            item.Duration = Duration;
            
            item.Description = Description;
            
            item.BackColor = BackColor;
            
            item.FontColor = FontColor;
            
            item.SalesPersonID = SalesPersonID;
            
            item.MembershipNo = MembershipNo;
            
            item.OrderHdrID = OrderHdrID;
            
            item.CreatedBy = CreatedBy;
            
            item.CreatedOn = CreatedOn;
            
            item.ModifiedBy = ModifiedBy;
            
            item.ModifiedOn = ModifiedOn;
            
            item.Deleted = Deleted;
            
            item.Organization = Organization;
            
            item.PickUpLocation = PickUpLocation;
            
            item.NoOfChildren = NoOfChildren;
            
            item.PointOfSaleID = PointOfSaleID;
            
            item.ResourceID = ResourceID;
            
            item.CheckInByWho = CheckInByWho;
            
            item.CheckOutByWho = CheckOutByWho;
            
            item.CheckInTime = CheckInTime;
            
            item.CheckOutTime = CheckOutTime;
            
            item.Remark = Remark;
            
            item.IsServerUpdate = IsServerUpdate;
            
            item.TimeExtension = TimeExtension;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(Guid Id,DateTime StartTime,int Duration,string Description,int BackColor,int FontColor,string SalesPersonID,string MembershipNo,string OrderHdrID,string CreatedBy,DateTime? CreatedOn,string ModifiedBy,DateTime? ModifiedOn,bool? Deleted,string Organization,string PickUpLocation,int? NoOfChildren,int? PointOfSaleID,string ResourceID,string CheckInByWho,string CheckOutByWho,DateTime? CheckInTime,DateTime? CheckOutTime,string Remark,bool? IsServerUpdate,int? TimeExtension)
	    {
		    Appointment item = new Appointment();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.StartTime = StartTime;
				
			item.Duration = Duration;
				
			item.Description = Description;
				
			item.BackColor = BackColor;
				
			item.FontColor = FontColor;
				
			item.SalesPersonID = SalesPersonID;
				
			item.MembershipNo = MembershipNo;
				
			item.OrderHdrID = OrderHdrID;
				
			item.CreatedBy = CreatedBy;
				
			item.CreatedOn = CreatedOn;
				
			item.ModifiedBy = ModifiedBy;
				
			item.ModifiedOn = ModifiedOn;
				
			item.Deleted = Deleted;
				
			item.Organization = Organization;
				
			item.PickUpLocation = PickUpLocation;
				
			item.NoOfChildren = NoOfChildren;
				
			item.PointOfSaleID = PointOfSaleID;
				
			item.ResourceID = ResourceID;
				
			item.CheckInByWho = CheckInByWho;
				
			item.CheckOutByWho = CheckOutByWho;
				
			item.CheckInTime = CheckInTime;
				
			item.CheckOutTime = CheckOutTime;
				
			item.Remark = Remark;
				
			item.IsServerUpdate = IsServerUpdate;
				
			item.TimeExtension = TimeExtension;
				
	        item.Save(UserName);
	    }
    }
}
