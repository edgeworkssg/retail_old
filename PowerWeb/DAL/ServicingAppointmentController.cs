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
    /// Controller class for ServicingAppointment
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class ServicingAppointmentController
    {
        // Preload our schema..
        ServicingAppointment thisSchemaLoad = new ServicingAppointment();
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
        public ServicingAppointmentCollection FetchAll()
        {
            ServicingAppointmentCollection coll = new ServicingAppointmentCollection();
            Query qry = new Query(ServicingAppointment.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public ServicingAppointmentCollection FetchByID(object ServiceRefNo)
        {
            ServicingAppointmentCollection coll = new ServicingAppointmentCollection().Where("ServiceRefNo", ServiceRefNo).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public ServicingAppointmentCollection FetchByQuery(Query qry)
        {
            ServicingAppointmentCollection coll = new ServicingAppointmentCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object ServiceRefNo)
        {
            return (ServicingAppointment.Delete(ServiceRefNo) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object ServiceRefNo)
        {
            return (ServicingAppointment.Destroy(ServiceRefNo) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string ServiceRefNo,DateTime? DateOfServicing,string Location,DateTime? PreferredTiming,string SerialNo,string Status,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy)
	    {
		    ServicingAppointment item = new ServicingAppointment();
		    
            item.ServiceRefNo = ServiceRefNo;
            
            item.DateOfServicing = DateOfServicing;
            
            item.Location = Location;
            
            item.PreferredTiming = PreferredTiming;
            
            item.SerialNo = SerialNo;
            
            item.Status = Status;
            
            item.CreatedOn = CreatedOn;
            
            item.CreatedBy = CreatedBy;
            
            item.ModifiedOn = ModifiedOn;
            
            item.ModifiedBy = ModifiedBy;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(string ServiceRefNo,DateTime? DateOfServicing,string Location,DateTime? PreferredTiming,string SerialNo,string Status,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy)
	    {
		    ServicingAppointment item = new ServicingAppointment();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.ServiceRefNo = ServiceRefNo;
				
			item.DateOfServicing = DateOfServicing;
				
			item.Location = Location;
				
			item.PreferredTiming = PreferredTiming;
				
			item.SerialNo = SerialNo;
				
			item.Status = Status;
				
			item.CreatedOn = CreatedOn;
				
			item.CreatedBy = CreatedBy;
				
			item.ModifiedOn = ModifiedOn;
				
			item.ModifiedBy = ModifiedBy;
				
	        item.Save(UserName);
	    }
    }
}
