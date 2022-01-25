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
    /// Controller class for MembershipRenewal
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class MembershipRenewalController
    {
        // Preload our schema..
        MembershipRenewal thisSchemaLoad = new MembershipRenewal();
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
        public MembershipRenewalCollection FetchAll()
        {
            MembershipRenewalCollection coll = new MembershipRenewalCollection();
            Query qry = new Query(MembershipRenewal.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public MembershipRenewalCollection FetchByID(object RenewalID)
        {
            MembershipRenewalCollection coll = new MembershipRenewalCollection().Where("renewalID", RenewalID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public MembershipRenewalCollection FetchByQuery(Query qry)
        {
            MembershipRenewalCollection coll = new MembershipRenewalCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object RenewalID)
        {
            return (MembershipRenewal.Delete(RenewalID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object RenewalID)
        {
            return (MembershipRenewal.Destroy(RenewalID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string MembershipNo,DateTime NewExpiryDate,string OrderHdrId,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,bool? Deleted,Guid UniqueId,string UserFld1,string UserFld2,string UserFld3,string UserFld4,string UserFld5,string UserFld6,string UserFld7,string UserFld8,string UserFld9,string UserFld10,bool? UserFlag1,bool? UserFlag2,bool? UserFlag3,bool? UserFlag4,bool? UserFlag5,decimal? UserFloat1,decimal? UserFloat2,decimal? UserFloat3,decimal? UserFloat4,decimal? UserFloat5,int? UserInt1,int? UserInt2,int? UserInt3,int? UserInt4,int? UserInt5)
	    {
		    MembershipRenewal item = new MembershipRenewal();
		    
            item.MembershipNo = MembershipNo;
            
            item.NewExpiryDate = NewExpiryDate;
            
            item.OrderHdrId = OrderHdrId;
            
            item.CreatedOn = CreatedOn;
            
            item.CreatedBy = CreatedBy;
            
            item.ModifiedOn = ModifiedOn;
            
            item.ModifiedBy = ModifiedBy;
            
            item.Deleted = Deleted;
            
            item.UniqueId = UniqueId;
            
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
	    public void Update(int RenewalID,string MembershipNo,DateTime NewExpiryDate,string OrderHdrId,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,bool? Deleted,Guid UniqueId,string UserFld1,string UserFld2,string UserFld3,string UserFld4,string UserFld5,string UserFld6,string UserFld7,string UserFld8,string UserFld9,string UserFld10,bool? UserFlag1,bool? UserFlag2,bool? UserFlag3,bool? UserFlag4,bool? UserFlag5,decimal? UserFloat1,decimal? UserFloat2,decimal? UserFloat3,decimal? UserFloat4,decimal? UserFloat5,int? UserInt1,int? UserInt2,int? UserInt3,int? UserInt4,int? UserInt5)
	    {
		    MembershipRenewal item = new MembershipRenewal();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.RenewalID = RenewalID;
				
			item.MembershipNo = MembershipNo;
				
			item.NewExpiryDate = NewExpiryDate;
				
			item.OrderHdrId = OrderHdrId;
				
			item.CreatedOn = CreatedOn;
				
			item.CreatedBy = CreatedBy;
				
			item.ModifiedOn = ModifiedOn;
				
			item.ModifiedBy = ModifiedBy;
				
			item.Deleted = Deleted;
				
			item.UniqueId = UniqueId;
				
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
				
			item.UserInt1 = UserInt1;
				
			item.UserInt2 = UserInt2;
				
			item.UserInt3 = UserInt3;
				
			item.UserInt4 = UserInt4;
				
			item.UserInt5 = UserInt5;
				
	        item.Save(UserName);
	    }
    }
}
