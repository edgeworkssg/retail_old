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
    /// Controller class for DeliveryOrder
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class DeliveryOrderController
    {
        // Preload our schema..
        DeliveryOrder thisSchemaLoad = new DeliveryOrder();
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
        public DeliveryOrderCollection FetchAll()
        {
            DeliveryOrderCollection coll = new DeliveryOrderCollection();
            Query qry = new Query(DeliveryOrder.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public DeliveryOrderCollection FetchByID(object OrderNumber)
        {
            DeliveryOrderCollection coll = new DeliveryOrderCollection().Where("OrderNumber", OrderNumber).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public DeliveryOrderCollection FetchByQuery(Query qry)
        {
            DeliveryOrderCollection coll = new DeliveryOrderCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object OrderNumber)
        {
            return (DeliveryOrder.Delete(OrderNumber) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object OrderNumber)
        {
            return (DeliveryOrder.Destroy(OrderNumber) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string OrderNumber,DateTime? DeliveryDate,int? PersonAssigned,string DeliveryAddress,DateTime? TimeSlotFrom,DateTime? TimeSlotTo,string Status,string SalesOrderRefNo,string PurchaseOrderRefNo,bool? Deleted,DateTime? CreatedOn,DateTime? ModifiedOn,string CreatedBy,string ModifiedBy,string MembershipNo,string RecipientName,string MobileNo,string HomeNo,string PostalCode,string UnitNo,string Remark,Guid UniqueID,bool? IsVendorDelivery,string DeliveryOutlet,bool? IsDelivered,bool? IsServerUpdate,DateTime? DeliveredDate,string Userfld1,string Userfld2,string Userfld3,string Userfld4,string Userfld5,string Userfld6,string Userfld7,string Userfld8,string Userfld9,string Userfld10,decimal? Userfloat1,decimal? Userfloat2,decimal? Userfloat3,decimal? Userfloat4,decimal? Userfloat5,bool? Userflag1,bool? Userflag2,bool? Userflag3,bool? Userflag4,bool? Userflag5,int? Userint1,int? Userint2,int? Userint3,int? Userint4,int? Userint5)
	    {
		    DeliveryOrder item = new DeliveryOrder();
		    
            item.OrderNumber = OrderNumber;
            
            item.DeliveryDate = DeliveryDate;
            
            item.PersonAssigned = PersonAssigned;
            
            item.DeliveryAddress = DeliveryAddress;
            
            item.TimeSlotFrom = TimeSlotFrom;
            
            item.TimeSlotTo = TimeSlotTo;
            
            item.Status = Status;
            
            item.SalesOrderRefNo = SalesOrderRefNo;
            
            item.PurchaseOrderRefNo = PurchaseOrderRefNo;
            
            item.Deleted = Deleted;
            
            item.CreatedOn = CreatedOn;
            
            item.ModifiedOn = ModifiedOn;
            
            item.CreatedBy = CreatedBy;
            
            item.ModifiedBy = ModifiedBy;
            
            item.MembershipNo = MembershipNo;
            
            item.RecipientName = RecipientName;
            
            item.MobileNo = MobileNo;
            
            item.HomeNo = HomeNo;
            
            item.PostalCode = PostalCode;
            
            item.UnitNo = UnitNo;
            
            item.Remark = Remark;
            
            item.UniqueID = UniqueID;
            
            item.IsVendorDelivery = IsVendorDelivery;
            
            item.DeliveryOutlet = DeliveryOutlet;
            
            item.IsDelivered = IsDelivered;
            
            item.IsServerUpdate = IsServerUpdate;
            
            item.DeliveredDate = DeliveredDate;
            
            item.Userfld1 = Userfld1;
            
            item.Userfld2 = Userfld2;
            
            item.Userfld3 = Userfld3;
            
            item.Userfld4 = Userfld4;
            
            item.Userfld5 = Userfld5;
            
            item.Userfld6 = Userfld6;
            
            item.Userfld7 = Userfld7;
            
            item.Userfld8 = Userfld8;
            
            item.Userfld9 = Userfld9;
            
            item.Userfld10 = Userfld10;
            
            item.Userfloat1 = Userfloat1;
            
            item.Userfloat2 = Userfloat2;
            
            item.Userfloat3 = Userfloat3;
            
            item.Userfloat4 = Userfloat4;
            
            item.Userfloat5 = Userfloat5;
            
            item.Userflag1 = Userflag1;
            
            item.Userflag2 = Userflag2;
            
            item.Userflag3 = Userflag3;
            
            item.Userflag4 = Userflag4;
            
            item.Userflag5 = Userflag5;
            
            item.Userint1 = Userint1;
            
            item.Userint2 = Userint2;
            
            item.Userint3 = Userint3;
            
            item.Userint4 = Userint4;
            
            item.Userint5 = Userint5;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(string OrderNumber,DateTime? DeliveryDate,int? PersonAssigned,string DeliveryAddress,DateTime? TimeSlotFrom,DateTime? TimeSlotTo,string Status,string SalesOrderRefNo,string PurchaseOrderRefNo,bool? Deleted,DateTime? CreatedOn,DateTime? ModifiedOn,string CreatedBy,string ModifiedBy,string MembershipNo,string RecipientName,string MobileNo,string HomeNo,string PostalCode,string UnitNo,string Remark,Guid UniqueID,bool? IsVendorDelivery,string DeliveryOutlet,bool? IsDelivered,bool? IsServerUpdate,DateTime? DeliveredDate,string Userfld1,string Userfld2,string Userfld3,string Userfld4,string Userfld5,string Userfld6,string Userfld7,string Userfld8,string Userfld9,string Userfld10,decimal? Userfloat1,decimal? Userfloat2,decimal? Userfloat3,decimal? Userfloat4,decimal? Userfloat5,bool? Userflag1,bool? Userflag2,bool? Userflag3,bool? Userflag4,bool? Userflag5,int? Userint1,int? Userint2,int? Userint3,int? Userint4,int? Userint5)
	    {
		    DeliveryOrder item = new DeliveryOrder();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.OrderNumber = OrderNumber;
				
			item.DeliveryDate = DeliveryDate;
				
			item.PersonAssigned = PersonAssigned;
				
			item.DeliveryAddress = DeliveryAddress;
				
			item.TimeSlotFrom = TimeSlotFrom;
				
			item.TimeSlotTo = TimeSlotTo;
				
			item.Status = Status;
				
			item.SalesOrderRefNo = SalesOrderRefNo;
				
			item.PurchaseOrderRefNo = PurchaseOrderRefNo;
				
			item.Deleted = Deleted;
				
			item.CreatedOn = CreatedOn;
				
			item.ModifiedOn = ModifiedOn;
				
			item.CreatedBy = CreatedBy;
				
			item.ModifiedBy = ModifiedBy;
				
			item.MembershipNo = MembershipNo;
				
			item.RecipientName = RecipientName;
				
			item.MobileNo = MobileNo;
				
			item.HomeNo = HomeNo;
				
			item.PostalCode = PostalCode;
				
			item.UnitNo = UnitNo;
				
			item.Remark = Remark;
				
			item.UniqueID = UniqueID;
				
			item.IsVendorDelivery = IsVendorDelivery;
				
			item.DeliveryOutlet = DeliveryOutlet;
				
			item.IsDelivered = IsDelivered;
				
			item.IsServerUpdate = IsServerUpdate;
				
			item.DeliveredDate = DeliveredDate;
				
			item.Userfld1 = Userfld1;
				
			item.Userfld2 = Userfld2;
				
			item.Userfld3 = Userfld3;
				
			item.Userfld4 = Userfld4;
				
			item.Userfld5 = Userfld5;
				
			item.Userfld6 = Userfld6;
				
			item.Userfld7 = Userfld7;
				
			item.Userfld8 = Userfld8;
				
			item.Userfld9 = Userfld9;
				
			item.Userfld10 = Userfld10;
				
			item.Userfloat1 = Userfloat1;
				
			item.Userfloat2 = Userfloat2;
				
			item.Userfloat3 = Userfloat3;
				
			item.Userfloat4 = Userfloat4;
				
			item.Userfloat5 = Userfloat5;
				
			item.Userflag1 = Userflag1;
				
			item.Userflag2 = Userflag2;
				
			item.Userflag3 = Userflag3;
				
			item.Userflag4 = Userflag4;
				
			item.Userflag5 = Userflag5;
				
			item.Userint1 = Userint1;
				
			item.Userint2 = Userint2;
				
			item.Userint3 = Userint3;
				
			item.Userint4 = Userint4;
				
			item.Userint5 = Userint5;
				
	        item.Save(UserName);
	    }
    }
}
