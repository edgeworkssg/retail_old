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
    /// Controller class for PackageRedemptionLog
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class PackageRedemptionLogController
    {
        // Preload our schema..
        PackageRedemptionLog thisSchemaLoad = new PackageRedemptionLog();
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
        public PackageRedemptionLogCollection FetchAll()
        {
            PackageRedemptionLogCollection coll = new PackageRedemptionLogCollection();
            Query qry = new Query(PackageRedemptionLog.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public PackageRedemptionLogCollection FetchByID(object PackageRedeemID)
        {
            PackageRedemptionLogCollection coll = new PackageRedemptionLogCollection().Where("PackageRedeemID", PackageRedeemID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public PackageRedemptionLogCollection FetchByQuery(Query qry)
        {
            PackageRedemptionLogCollection coll = new PackageRedemptionLogCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object PackageRedeemID)
        {
            return (PackageRedemptionLog.Delete(PackageRedeemID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object PackageRedeemID)
        {
            return (PackageRedemptionLog.Destroy(PackageRedeemID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(DateTime PackageRedeemDate,string StylistId,decimal Amount,string MembershipNo,string Name,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,bool Deleted,Guid UniqueID)
	    {
		    PackageRedemptionLog item = new PackageRedemptionLog();
		    
            item.PackageRedeemDate = PackageRedeemDate;
            
            item.StylistId = StylistId;
            
            item.Amount = Amount;
            
            item.MembershipNo = MembershipNo;
            
            item.Name = Name;
            
            item.CreatedOn = CreatedOn;
            
            item.CreatedBy = CreatedBy;
            
            item.ModifiedOn = ModifiedOn;
            
            item.ModifiedBy = ModifiedBy;
            
            item.Deleted = Deleted;
            
            item.UniqueID = UniqueID;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int PackageRedeemID,DateTime PackageRedeemDate,string StylistId,decimal Amount,string MembershipNo,string Name,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,bool Deleted,Guid UniqueID)
	    {
		    PackageRedemptionLog item = new PackageRedemptionLog();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.PackageRedeemID = PackageRedeemID;
				
			item.PackageRedeemDate = PackageRedeemDate;
				
			item.StylistId = StylistId;
				
			item.Amount = Amount;
				
			item.MembershipNo = MembershipNo;
				
			item.Name = Name;
				
			item.CreatedOn = CreatedOn;
				
			item.CreatedBy = CreatedBy;
				
			item.ModifiedOn = ModifiedOn;
				
			item.ModifiedBy = ModifiedBy;
				
			item.Deleted = Deleted;
				
			item.UniqueID = UniqueID;
				
	        item.Save(UserName);
	    }
    }
}
