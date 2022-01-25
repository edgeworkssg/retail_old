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
    /// Controller class for SpecialDiscounts
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class SpecialDiscountController
    {
        // Preload our schema..
        SpecialDiscount thisSchemaLoad = new SpecialDiscount();
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
        public SpecialDiscountCollection FetchAll()
        {
            SpecialDiscountCollection coll = new SpecialDiscountCollection();
            Query qry = new Query(SpecialDiscount.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public SpecialDiscountCollection FetchByID(object DiscountName)
        {
            SpecialDiscountCollection coll = new SpecialDiscountCollection().Where("DiscountName", DiscountName).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public SpecialDiscountCollection FetchByQuery(Query qry)
        {
            SpecialDiscountCollection coll = new SpecialDiscountCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object DiscountName)
        {
            return (SpecialDiscount.Delete(DiscountName) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object DiscountName)
        {
            return (SpecialDiscount.Destroy(DiscountName) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(Guid SpecialDiscountID,string DiscountName,decimal DiscountPercentage,bool ShowLabel,int PriorityLevel,bool? Deleted,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,bool? UseSPP,bool? Enabled,bool? ApplicableToAllItem,DateTime? StartDate,DateTime? EndDate,decimal? MinimumSpending,bool? IsBankPromo,string DiscountLabel,string AssignedOutlet)
	    {
		    SpecialDiscount item = new SpecialDiscount();
		    
            item.SpecialDiscountID = SpecialDiscountID;
            
            item.DiscountName = DiscountName;
            
            item.DiscountPercentage = DiscountPercentage;
            
            item.ShowLabel = ShowLabel;
            
            item.PriorityLevel = PriorityLevel;
            
            item.Deleted = Deleted;
            
            item.CreatedOn = CreatedOn;
            
            item.CreatedBy = CreatedBy;
            
            item.ModifiedOn = ModifiedOn;
            
            item.ModifiedBy = ModifiedBy;
            
            item.UseSPP = UseSPP;
            
            item.Enabled = Enabled;
            
            item.ApplicableToAllItem = ApplicableToAllItem;
            
            item.StartDate = StartDate;
            
            item.EndDate = EndDate;
            
            item.MinimumSpending = MinimumSpending;
            
            item.IsBankPromo = IsBankPromo;
            
            item.DiscountLabel = DiscountLabel;
            
            item.AssignedOutlet = AssignedOutlet;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(Guid SpecialDiscountID,string DiscountName,decimal DiscountPercentage,bool ShowLabel,int PriorityLevel,bool? Deleted,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,bool? UseSPP,bool? Enabled,bool? ApplicableToAllItem,DateTime? StartDate,DateTime? EndDate,decimal? MinimumSpending,bool? IsBankPromo,string DiscountLabel,string AssignedOutlet)
	    {
		    SpecialDiscount item = new SpecialDiscount();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.SpecialDiscountID = SpecialDiscountID;
				
			item.DiscountName = DiscountName;
				
			item.DiscountPercentage = DiscountPercentage;
				
			item.ShowLabel = ShowLabel;
				
			item.PriorityLevel = PriorityLevel;
				
			item.Deleted = Deleted;
				
			item.CreatedOn = CreatedOn;
				
			item.CreatedBy = CreatedBy;
				
			item.ModifiedOn = ModifiedOn;
				
			item.ModifiedBy = ModifiedBy;
				
			item.UseSPP = UseSPP;
				
			item.Enabled = Enabled;
				
			item.ApplicableToAllItem = ApplicableToAllItem;
				
			item.StartDate = StartDate;
				
			item.EndDate = EndDate;
				
			item.MinimumSpending = MinimumSpending;
				
			item.IsBankPromo = IsBankPromo;
				
			item.DiscountLabel = DiscountLabel;
				
			item.AssignedOutlet = AssignedOutlet;
				
	        item.Save(UserName);
	    }
    }
}
