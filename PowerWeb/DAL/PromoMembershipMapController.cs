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
    /// Controller class for PromoMembershipMap
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class PromoMembershipMapController
    {
        // Preload our schema..
        PromoMembershipMap thisSchemaLoad = new PromoMembershipMap();
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
        public PromoMembershipMapCollection FetchAll()
        {
            PromoMembershipMapCollection coll = new PromoMembershipMapCollection();
            Query qry = new Query(PromoMembershipMap.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public PromoMembershipMapCollection FetchByID(object PromoMembershipID)
        {
            PromoMembershipMapCollection coll = new PromoMembershipMapCollection().Where("PromoMembershipID", PromoMembershipID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public PromoMembershipMapCollection FetchByQuery(Query qry)
        {
            PromoMembershipMapCollection coll = new PromoMembershipMapCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object PromoMembershipID)
        {
            return (PromoMembershipMap.Delete(PromoMembershipID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object PromoMembershipID)
        {
            return (PromoMembershipMap.Destroy(PromoMembershipID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(Guid PromoMembershipID,decimal MembershipPrice,decimal MembershipDiscount,bool UseMembershipPrice,int PromoCampaignHdrID,int MembershipGroupID,DateTime? CreatedOn,DateTime? ModifiedOn,string CreatedBy,string ModifiedBy,bool? Deleted)
	    {
		    PromoMembershipMap item = new PromoMembershipMap();
		    
            item.PromoMembershipID = PromoMembershipID;
            
            item.MembershipPrice = MembershipPrice;
            
            item.MembershipDiscount = MembershipDiscount;
            
            item.UseMembershipPrice = UseMembershipPrice;
            
            item.PromoCampaignHdrID = PromoCampaignHdrID;
            
            item.MembershipGroupID = MembershipGroupID;
            
            item.CreatedOn = CreatedOn;
            
            item.ModifiedOn = ModifiedOn;
            
            item.CreatedBy = CreatedBy;
            
            item.ModifiedBy = ModifiedBy;
            
            item.Deleted = Deleted;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(Guid PromoMembershipID,decimal MembershipPrice,decimal MembershipDiscount,bool UseMembershipPrice,int PromoCampaignHdrID,int MembershipGroupID,DateTime? CreatedOn,DateTime? ModifiedOn,string CreatedBy,string ModifiedBy,bool? Deleted)
	    {
		    PromoMembershipMap item = new PromoMembershipMap();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.PromoMembershipID = PromoMembershipID;
				
			item.MembershipPrice = MembershipPrice;
				
			item.MembershipDiscount = MembershipDiscount;
				
			item.UseMembershipPrice = UseMembershipPrice;
				
			item.PromoCampaignHdrID = PromoCampaignHdrID;
				
			item.MembershipGroupID = MembershipGroupID;
				
			item.CreatedOn = CreatedOn;
				
			item.ModifiedOn = ModifiedOn;
				
			item.CreatedBy = CreatedBy;
				
			item.ModifiedBy = ModifiedBy;
				
			item.Deleted = Deleted;
				
	        item.Save(UserName);
	    }
    }
}
