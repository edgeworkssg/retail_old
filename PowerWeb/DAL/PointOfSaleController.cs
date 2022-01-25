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
    /// Controller class for PointOfSale
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class PointOfSaleController
    {
        // Preload our schema..
        PointOfSale thisSchemaLoad = new PointOfSale();
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
        public PointOfSaleCollection FetchAll()
        {
            PointOfSaleCollection coll = new PointOfSaleCollection();
            Query qry = new Query(PointOfSale.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public PointOfSaleCollection FetchByID(object PointOfSaleID)
        {
            PointOfSaleCollection coll = new PointOfSaleCollection().Where("PointOfSaleID", PointOfSaleID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public PointOfSaleCollection FetchByQuery(Query qry)
        {
            PointOfSaleCollection coll = new PointOfSaleCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object PointOfSaleID)
        {
            return (PointOfSale.Delete(PointOfSaleID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object PointOfSaleID)
        {
            return (PointOfSale.Destroy(PointOfSaleID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string PointOfSaleName,string PointOfSaleDescription,string OutletName,string PhoneNo,int? DepartmentID,Guid QuickAccessGroupID,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,bool? Deleted,string Userfld1,string Userfld2,string Userfld3,string Userfld4,string Userfld5,string Userfld6,string Userfld7,string Userfld8,string Userfld9,string Userfld10,bool? Userflag1,bool? Userflag2,bool? Userflag3,bool? Userflag4,bool? Userflag5,decimal? Userfloat1,decimal? Userfloat2,decimal? Userfloat3,decimal? Userfloat4,decimal? Userfloat5,int? Userint1,int? Userint2,int? Userint3,int? Userint4,int? Userint5,string MembershipCode,string TenantMachineID,string ApiKey,string InterfaceValidationStatus,string Mall,string RetailerName,string RetailerLevel,string ShopNo,string RetailerContactPerson,string RetailerContactNo,string RetailerEmail,string InterfaceDevTeam,string VendorContactPersonName,string VendorContactNo,string VendorContactName,DateTime? BusinessStartDate,DateTime? BusinessEndDate,string OptionX,string TenantCompanyName,string RetailerDesignation,string VendorEmail,string POSType,string POSBrand,string Posos,string POSSoftware,int? NoOfPOS)
	    {
		    PointOfSale item = new PointOfSale();
		    
            item.PointOfSaleName = PointOfSaleName;
            
            item.PointOfSaleDescription = PointOfSaleDescription;
            
            item.OutletName = OutletName;
            
            item.PhoneNo = PhoneNo;
            
            item.DepartmentID = DepartmentID;
            
            item.QuickAccessGroupID = QuickAccessGroupID;
            
            item.CreatedOn = CreatedOn;
            
            item.CreatedBy = CreatedBy;
            
            item.ModifiedOn = ModifiedOn;
            
            item.ModifiedBy = ModifiedBy;
            
            item.Deleted = Deleted;
            
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
            
            item.Userflag1 = Userflag1;
            
            item.Userflag2 = Userflag2;
            
            item.Userflag3 = Userflag3;
            
            item.Userflag4 = Userflag4;
            
            item.Userflag5 = Userflag5;
            
            item.Userfloat1 = Userfloat1;
            
            item.Userfloat2 = Userfloat2;
            
            item.Userfloat3 = Userfloat3;
            
            item.Userfloat4 = Userfloat4;
            
            item.Userfloat5 = Userfloat5;
            
            item.Userint1 = Userint1;
            
            item.Userint2 = Userint2;
            
            item.Userint3 = Userint3;
            
            item.Userint4 = Userint4;
            
            item.Userint5 = Userint5;
            
            item.MembershipCode = MembershipCode;
            
            item.TenantMachineID = TenantMachineID;
            
            item.ApiKey = ApiKey;
            
            item.InterfaceValidationStatus = InterfaceValidationStatus;
            
            item.Mall = Mall;
            
            item.RetailerName = RetailerName;
            
            item.RetailerLevel = RetailerLevel;
            
            item.ShopNo = ShopNo;
            
            item.RetailerContactPerson = RetailerContactPerson;
            
            item.RetailerContactNo = RetailerContactNo;
            
            item.RetailerEmail = RetailerEmail;
            
            item.InterfaceDevTeam = InterfaceDevTeam;
            
            item.VendorContactPersonName = VendorContactPersonName;
            
            item.VendorContactNo = VendorContactNo;
            
            item.VendorContactName = VendorContactName;
            
            item.BusinessStartDate = BusinessStartDate;
            
            item.BusinessEndDate = BusinessEndDate;
            
            item.OptionX = OptionX;
            
            item.TenantCompanyName = TenantCompanyName;
            
            item.RetailerDesignation = RetailerDesignation;
            
            item.VendorEmail = VendorEmail;
            
            item.POSType = POSType;
            
            item.POSBrand = POSBrand;
            
            item.Posos = Posos;
            
            item.POSSoftware = POSSoftware;
            
            item.NoOfPOS = NoOfPOS;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int PointOfSaleID,string PointOfSaleName,string PointOfSaleDescription,string OutletName,string PhoneNo,int? DepartmentID,Guid QuickAccessGroupID,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,bool? Deleted,string Userfld1,string Userfld2,string Userfld3,string Userfld4,string Userfld5,string Userfld6,string Userfld7,string Userfld8,string Userfld9,string Userfld10,bool? Userflag1,bool? Userflag2,bool? Userflag3,bool? Userflag4,bool? Userflag5,decimal? Userfloat1,decimal? Userfloat2,decimal? Userfloat3,decimal? Userfloat4,decimal? Userfloat5,int? Userint1,int? Userint2,int? Userint3,int? Userint4,int? Userint5,string MembershipCode,string TenantMachineID,string ApiKey,string InterfaceValidationStatus,string Mall,string RetailerName,string RetailerLevel,string ShopNo,string RetailerContactPerson,string RetailerContactNo,string RetailerEmail,string InterfaceDevTeam,string VendorContactPersonName,string VendorContactNo,string VendorContactName,DateTime? BusinessStartDate,DateTime? BusinessEndDate,string OptionX,string TenantCompanyName,string RetailerDesignation,string VendorEmail,string POSType,string POSBrand,string Posos,string POSSoftware,int? NoOfPOS)
	    {
		    PointOfSale item = new PointOfSale();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.PointOfSaleID = PointOfSaleID;
				
			item.PointOfSaleName = PointOfSaleName;
				
			item.PointOfSaleDescription = PointOfSaleDescription;
				
			item.OutletName = OutletName;
				
			item.PhoneNo = PhoneNo;
				
			item.DepartmentID = DepartmentID;
				
			item.QuickAccessGroupID = QuickAccessGroupID;
				
			item.CreatedOn = CreatedOn;
				
			item.CreatedBy = CreatedBy;
				
			item.ModifiedOn = ModifiedOn;
				
			item.ModifiedBy = ModifiedBy;
				
			item.Deleted = Deleted;
				
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
				
			item.Userflag1 = Userflag1;
				
			item.Userflag2 = Userflag2;
				
			item.Userflag3 = Userflag3;
				
			item.Userflag4 = Userflag4;
				
			item.Userflag5 = Userflag5;
				
			item.Userfloat1 = Userfloat1;
				
			item.Userfloat2 = Userfloat2;
				
			item.Userfloat3 = Userfloat3;
				
			item.Userfloat4 = Userfloat4;
				
			item.Userfloat5 = Userfloat5;
				
			item.Userint1 = Userint1;
				
			item.Userint2 = Userint2;
				
			item.Userint3 = Userint3;
				
			item.Userint4 = Userint4;
				
			item.Userint5 = Userint5;
				
			item.MembershipCode = MembershipCode;
				
			item.TenantMachineID = TenantMachineID;
				
			item.ApiKey = ApiKey;
				
			item.InterfaceValidationStatus = InterfaceValidationStatus;
				
			item.Mall = Mall;
				
			item.RetailerName = RetailerName;
				
			item.RetailerLevel = RetailerLevel;
				
			item.ShopNo = ShopNo;
				
			item.RetailerContactPerson = RetailerContactPerson;
				
			item.RetailerContactNo = RetailerContactNo;
				
			item.RetailerEmail = RetailerEmail;
				
			item.InterfaceDevTeam = InterfaceDevTeam;
				
			item.VendorContactPersonName = VendorContactPersonName;
				
			item.VendorContactNo = VendorContactNo;
				
			item.VendorContactName = VendorContactName;
				
			item.BusinessStartDate = BusinessStartDate;
				
			item.BusinessEndDate = BusinessEndDate;
				
			item.OptionX = OptionX;
				
			item.TenantCompanyName = TenantCompanyName;
				
			item.RetailerDesignation = RetailerDesignation;
				
			item.VendorEmail = VendorEmail;
				
			item.POSType = POSType;
				
			item.POSBrand = POSBrand;
				
			item.Posos = Posos;
				
			item.POSSoftware = POSSoftware;
				
			item.NoOfPOS = NoOfPOS;
				
	        item.Save(UserName);
	    }
    }
}
