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
    /// Controller class for PromoCampaignDet
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class PromoCampaignDetController
    {
        // Preload our schema..
        PromoCampaignDet thisSchemaLoad = new PromoCampaignDet();
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
        public PromoCampaignDetCollection FetchAll()
        {
            PromoCampaignDetCollection coll = new PromoCampaignDetCollection();
            Query qry = new Query(PromoCampaignDet.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public PromoCampaignDetCollection FetchByID(object PromoCampaignDetID)
        {
            PromoCampaignDetCollection coll = new PromoCampaignDetCollection().Where("PromoCampaignDetID", PromoCampaignDetID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public PromoCampaignDetCollection FetchByQuery(Query qry)
        {
            PromoCampaignDetCollection coll = new PromoCampaignDetCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object PromoCampaignDetID)
        {
            return (PromoCampaignDet.Delete(PromoCampaignDetID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object PromoCampaignDetID)
        {
            return (PromoCampaignDet.Destroy(PromoCampaignDetID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(int PromoCampaignHdrID,int? ItemGroupID,string ItemNo,string CategoryName,int? FromQuantity,int? ToQuantity,int MinQuantity,int? UnitQty,string CreatedBy,DateTime? CreatedOn,string ModifiedBy,DateTime? ModifiedOn,bool? Deleted,string Userfld1,string Userfld2,string Userfld3,string Userfld4,string Userfld5,string Userfld6,string Userfld7,string Userfld8,string Userfld9,string Userfld10,bool? Userflag1,bool? Userflag2,bool? Userflag3,bool? Userflag4,bool? Userflag5,decimal? Userfloat1,decimal? Userfloat2,decimal? Userfloat3,decimal? Userfloat4,decimal? Userfloat5,int? Userint1,int? Userint2,int? Userint3,int? Userint4,int? Userint5,decimal? Promoprice,int? AnyQty,decimal? DiscPercent,decimal? DiscAmount)
	    {
		    PromoCampaignDet item = new PromoCampaignDet();
		    
            item.PromoCampaignHdrID = PromoCampaignHdrID;
            
            item.ItemGroupID = ItemGroupID;
            
            item.ItemNo = ItemNo;
            
            item.CategoryName = CategoryName;
            
            item.FromQuantity = FromQuantity;
            
            item.ToQuantity = ToQuantity;
            
            item.MinQuantity = MinQuantity;
            
            item.UnitQty = UnitQty;
            
            item.CreatedBy = CreatedBy;
            
            item.CreatedOn = CreatedOn;
            
            item.ModifiedBy = ModifiedBy;
            
            item.ModifiedOn = ModifiedOn;
            
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
            
            item.Promoprice = Promoprice;
            
            item.AnyQty = AnyQty;
            
            item.DiscPercent = DiscPercent;
            
            item.DiscAmount = DiscAmount;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int PromoCampaignDetID,int PromoCampaignHdrID,int? ItemGroupID,string ItemNo,string CategoryName,int? FromQuantity,int? ToQuantity,int MinQuantity,int? UnitQty,string CreatedBy,DateTime? CreatedOn,string ModifiedBy,DateTime? ModifiedOn,bool? Deleted,string Userfld1,string Userfld2,string Userfld3,string Userfld4,string Userfld5,string Userfld6,string Userfld7,string Userfld8,string Userfld9,string Userfld10,bool? Userflag1,bool? Userflag2,bool? Userflag3,bool? Userflag4,bool? Userflag5,decimal? Userfloat1,decimal? Userfloat2,decimal? Userfloat3,decimal? Userfloat4,decimal? Userfloat5,int? Userint1,int? Userint2,int? Userint3,int? Userint4,int? Userint5,decimal? Promoprice,int? AnyQty,decimal? DiscPercent,decimal? DiscAmount)
	    {
		    PromoCampaignDet item = new PromoCampaignDet();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.PromoCampaignDetID = PromoCampaignDetID;
				
			item.PromoCampaignHdrID = PromoCampaignHdrID;
				
			item.ItemGroupID = ItemGroupID;
				
			item.ItemNo = ItemNo;
				
			item.CategoryName = CategoryName;
				
			item.FromQuantity = FromQuantity;
				
			item.ToQuantity = ToQuantity;
				
			item.MinQuantity = MinQuantity;
				
			item.UnitQty = UnitQty;
				
			item.CreatedBy = CreatedBy;
				
			item.CreatedOn = CreatedOn;
				
			item.ModifiedBy = ModifiedBy;
				
			item.ModifiedOn = ModifiedOn;
				
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
				
			item.Promoprice = Promoprice;
				
			item.AnyQty = AnyQty;
				
			item.DiscPercent = DiscPercent;
				
			item.DiscAmount = DiscAmount;
				
	        item.Save(UserName);
	    }
    }
}
