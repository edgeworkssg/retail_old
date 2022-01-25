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
    /// Controller class for ScheduledDiscount
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class ScheduledDiscountController
    {
        // Preload our schema..
        ScheduledDiscount thisSchemaLoad = new ScheduledDiscount();
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
        public ScheduledDiscountCollection FetchAll()
        {
            ScheduledDiscountCollection coll = new ScheduledDiscountCollection();
            Query qry = new Query(ScheduledDiscount.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public ScheduledDiscountCollection FetchByID(object ScheduleDiscountID)
        {
            ScheduledDiscountCollection coll = new ScheduledDiscountCollection().Where("ScheduleDiscountID", ScheduleDiscountID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public ScheduledDiscountCollection FetchByQuery(Query qry)
        {
            ScheduledDiscountCollection coll = new ScheduledDiscountCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object ScheduleDiscountID)
        {
            return (ScheduledDiscount.Delete(ScheduleDiscountID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object ScheduleDiscountID)
        {
            return (ScheduledDiscount.Destroy(ScheduleDiscountID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(Guid ScheduleDiscountID,string DiscountName,DateTime StartDate,DateTime EndDate,string ItemNo,decimal DiscountedPrice,decimal MembershipPrice,decimal DiscountPercentage,decimal MembershipDiscountPercentage,string DiscountType,string MembershipDiscountType,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,bool? Deleted,string UserFld1,string UserFld2,string UserFld3,string UserFld4,string UserFld5,bool? UserFlag1,bool? UserFlag2,bool? UserFlag3,bool? UserFlag4,bool? UserFlag5,decimal? UserFloat1,decimal? UserFloat2,decimal? UserFloat3,decimal? UserFloat4,decimal? UserFloat5)
	    {
		    ScheduledDiscount item = new ScheduledDiscount();
		    
            item.ScheduleDiscountID = ScheduleDiscountID;
            
            item.DiscountName = DiscountName;
            
            item.StartDate = StartDate;
            
            item.EndDate = EndDate;
            
            item.ItemNo = ItemNo;
            
            item.DiscountedPrice = DiscountedPrice;
            
            item.MembershipPrice = MembershipPrice;
            
            item.DiscountPercentage = DiscountPercentage;
            
            item.MembershipDiscountPercentage = MembershipDiscountPercentage;
            
            item.DiscountType = DiscountType;
            
            item.MembershipDiscountType = MembershipDiscountType;
            
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
	    public void Update(Guid ScheduleDiscountID,string DiscountName,DateTime StartDate,DateTime EndDate,string ItemNo,decimal DiscountedPrice,decimal MembershipPrice,decimal DiscountPercentage,decimal MembershipDiscountPercentage,string DiscountType,string MembershipDiscountType,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,bool? Deleted,string UserFld1,string UserFld2,string UserFld3,string UserFld4,string UserFld5,bool? UserFlag1,bool? UserFlag2,bool? UserFlag3,bool? UserFlag4,bool? UserFlag5,decimal? UserFloat1,decimal? UserFloat2,decimal? UserFloat3,decimal? UserFloat4,decimal? UserFloat5)
	    {
		    ScheduledDiscount item = new ScheduledDiscount();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.ScheduleDiscountID = ScheduleDiscountID;
				
			item.DiscountName = DiscountName;
				
			item.StartDate = StartDate;
				
			item.EndDate = EndDate;
				
			item.ItemNo = ItemNo;
				
			item.DiscountedPrice = DiscountedPrice;
				
			item.MembershipPrice = MembershipPrice;
				
			item.DiscountPercentage = DiscountPercentage;
				
			item.MembershipDiscountPercentage = MembershipDiscountPercentage;
				
			item.DiscountType = DiscountType;
				
			item.MembershipDiscountType = MembershipDiscountType;
				
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
