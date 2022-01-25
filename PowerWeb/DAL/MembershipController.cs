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
    /// Controller class for Membership
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class MembershipController
    {
        // Preload our schema..
        Membership thisSchemaLoad = new Membership();
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
        public MembershipCollection FetchAll()
        {
            MembershipCollection coll = new MembershipCollection();
            Query qry = new Query(Membership.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public MembershipCollection FetchByID(object MembershipNo)
        {
            MembershipCollection coll = new MembershipCollection().Where("MembershipNo", MembershipNo).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public MembershipCollection FetchByQuery(Query qry)
        {
            MembershipCollection coll = new MembershipCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object MembershipNo)
        {
            return (Membership.Delete(MembershipNo) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object MembershipNo)
        {
            return (Membership.Destroy(MembershipNo) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string MembershipNo,int MembershipGroupId,string Title,string LastName,string FirstName,string ChristianName,string ChineseName,string NameToAppear,string Gender,DateTime? DateOfBirth,string Nationality,string Nric,string Occupation,string MaritalStatus,string Email,string Block,string BuildingName,string StreetName,string StreetName2,string UnitNo,string City,string Country,string ZipCode,string Mobile,string Office,string Fax,string Home,DateTime? ExpiryDate,string Remarks,DateTime? SubscriptionDate,bool? IsChc,string Ministry,bool? IsStudentCard,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,bool? Deleted,Guid UniqueID,string Userfld1,string Userfld2,string Userfld3,string Userfld4,string Userfld5,string Userfld6,string Userfld7,string Userfld8,string Userfld9,string Userfld10,bool? Userflag1,bool? Userflag2,bool? Userflag3,bool? Userflag4,bool? Userflag5,decimal? Userfloat1,decimal? Userfloat2,decimal? Userfloat3,decimal? Userfloat4,decimal? Userfloat5,int? Userint1,int? Userint2,int? Userint3,int? Userint4,int? Userint5,bool? IsVitaMix,bool? IsWaterFilter,bool? IsJuicePlus,bool? IsYoung,string SalesPersonID)
	    {
		    Membership item = new Membership();
		    
            item.MembershipNo = MembershipNo;
            
            item.MembershipGroupId = MembershipGroupId;
            
            item.Title = Title;
            
            item.LastName = LastName;
            
            item.FirstName = FirstName;
            
            item.ChristianName = ChristianName;
            
            item.ChineseName = ChineseName;
            
            item.NameToAppear = NameToAppear;
            
            item.Gender = Gender;
            
            item.DateOfBirth = DateOfBirth;
            
            item.Nationality = Nationality;
            
            item.Nric = Nric;
            
            item.Occupation = Occupation;
            
            item.MaritalStatus = MaritalStatus;
            
            item.Email = Email;
            
            item.Block = Block;
            
            item.BuildingName = BuildingName;
            
            item.StreetName = StreetName;
            
            item.StreetName2 = StreetName2;
            
            item.UnitNo = UnitNo;
            
            item.City = City;
            
            item.Country = Country;
            
            item.ZipCode = ZipCode;
            
            item.Mobile = Mobile;
            
            item.Office = Office;
            
            item.Fax = Fax;
            
            item.Home = Home;
            
            item.ExpiryDate = ExpiryDate;
            
            item.Remarks = Remarks;
            
            item.SubscriptionDate = SubscriptionDate;
            
            item.IsChc = IsChc;
            
            item.Ministry = Ministry;
            
            item.IsStudentCard = IsStudentCard;
            
            item.CreatedOn = CreatedOn;
            
            item.CreatedBy = CreatedBy;
            
            item.ModifiedOn = ModifiedOn;
            
            item.ModifiedBy = ModifiedBy;
            
            item.Deleted = Deleted;
            
            item.UniqueID = UniqueID;
            
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
            
            item.IsVitaMix = IsVitaMix;
            
            item.IsWaterFilter = IsWaterFilter;
            
            item.IsJuicePlus = IsJuicePlus;
            
            item.IsYoung = IsYoung;
            
            item.SalesPersonID = SalesPersonID;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(string MembershipNo,int MembershipGroupId,string Title,string LastName,string FirstName,string ChristianName,string ChineseName,string NameToAppear,string Gender,DateTime? DateOfBirth,string Nationality,string Nric,string Occupation,string MaritalStatus,string Email,string Block,string BuildingName,string StreetName,string StreetName2,string UnitNo,string City,string Country,string ZipCode,string Mobile,string Office,string Fax,string Home,DateTime? ExpiryDate,string Remarks,DateTime? SubscriptionDate,bool? IsChc,string Ministry,bool? IsStudentCard,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,bool? Deleted,Guid UniqueID,string Userfld1,string Userfld2,string Userfld3,string Userfld4,string Userfld5,string Userfld6,string Userfld7,string Userfld8,string Userfld9,string Userfld10,bool? Userflag1,bool? Userflag2,bool? Userflag3,bool? Userflag4,bool? Userflag5,decimal? Userfloat1,decimal? Userfloat2,decimal? Userfloat3,decimal? Userfloat4,decimal? Userfloat5,int? Userint1,int? Userint2,int? Userint3,int? Userint4,int? Userint5,bool? IsVitaMix,bool? IsWaterFilter,bool? IsJuicePlus,bool? IsYoung,string SalesPersonID)
	    {
		    Membership item = new Membership();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.MembershipNo = MembershipNo;
				
			item.MembershipGroupId = MembershipGroupId;
				
			item.Title = Title;
				
			item.LastName = LastName;
				
			item.FirstName = FirstName;
				
			item.ChristianName = ChristianName;
				
			item.ChineseName = ChineseName;
				
			item.NameToAppear = NameToAppear;
				
			item.Gender = Gender;
				
			item.DateOfBirth = DateOfBirth;
				
			item.Nationality = Nationality;
				
			item.Nric = Nric;
				
			item.Occupation = Occupation;
				
			item.MaritalStatus = MaritalStatus;
				
			item.Email = Email;
				
			item.Block = Block;
				
			item.BuildingName = BuildingName;
				
			item.StreetName = StreetName;
				
			item.StreetName2 = StreetName2;
				
			item.UnitNo = UnitNo;
				
			item.City = City;
				
			item.Country = Country;
				
			item.ZipCode = ZipCode;
				
			item.Mobile = Mobile;
				
			item.Office = Office;
				
			item.Fax = Fax;
				
			item.Home = Home;
				
			item.ExpiryDate = ExpiryDate;
				
			item.Remarks = Remarks;
				
			item.SubscriptionDate = SubscriptionDate;
				
			item.IsChc = IsChc;
				
			item.Ministry = Ministry;
				
			item.IsStudentCard = IsStudentCard;
				
			item.CreatedOn = CreatedOn;
				
			item.CreatedBy = CreatedBy;
				
			item.ModifiedOn = ModifiedOn;
				
			item.ModifiedBy = ModifiedBy;
				
			item.Deleted = Deleted;
				
			item.UniqueID = UniqueID;
				
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
				
			item.IsVitaMix = IsVitaMix;
				
			item.IsWaterFilter = IsWaterFilter;
				
			item.IsJuicePlus = IsJuicePlus;
				
			item.IsYoung = IsYoung;
				
			item.SalesPersonID = SalesPersonID;
				
	        item.Save(UserName);
	    }
    }
}
