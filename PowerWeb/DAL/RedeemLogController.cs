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
    /// Controller class for RedeemLog
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class RedeemLogController
    {
        // Preload our schema..
        RedeemLog thisSchemaLoad = new RedeemLog();
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
        public RedeemLogCollection FetchAll()
        {
            RedeemLogCollection coll = new RedeemLogCollection();
            Query qry = new Query(RedeemLog.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public RedeemLogCollection FetchByID(object RedeemLogId)
        {
            RedeemLogCollection coll = new RedeemLogCollection().Where("RedeemLogId", RedeemLogId).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public RedeemLogCollection FetchByQuery(Query qry)
        {
            RedeemLogCollection coll = new RedeemLogCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object RedeemLogId)
        {
            return (RedeemLog.Delete(RedeemLogId) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object RedeemLogId)
        {
            return (RedeemLog.Destroy(RedeemLogId) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(int RedemptionId,string MembershipNo,DateTime? RedeemDate,decimal? PointsBefore,decimal? PointsAfter,bool IsStockOutAlready,string DeliveryAddress,string ContactNo,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,bool? Deleted)
	    {
		    RedeemLog item = new RedeemLog();
		    
            item.RedemptionId = RedemptionId;
            
            item.MembershipNo = MembershipNo;
            
            item.RedeemDate = RedeemDate;
            
            item.PointsBefore = PointsBefore;
            
            item.PointsAfter = PointsAfter;
            
            item.IsStockOutAlready = IsStockOutAlready;
            
            item.DeliveryAddress = DeliveryAddress;
            
            item.ContactNo = ContactNo;
            
            item.CreatedOn = CreatedOn;
            
            item.CreatedBy = CreatedBy;
            
            item.ModifiedOn = ModifiedOn;
            
            item.ModifiedBy = ModifiedBy;
            
            item.Deleted = Deleted;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int RedeemLogId,int RedemptionId,string MembershipNo,DateTime? RedeemDate,decimal? PointsBefore,decimal? PointsAfter,bool IsStockOutAlready,string DeliveryAddress,string ContactNo,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,bool? Deleted)
	    {
		    RedeemLog item = new RedeemLog();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.RedeemLogId = RedeemLogId;
				
			item.RedemptionId = RedemptionId;
				
			item.MembershipNo = MembershipNo;
				
			item.RedeemDate = RedeemDate;
				
			item.PointsBefore = PointsBefore;
				
			item.PointsAfter = PointsAfter;
				
			item.IsStockOutAlready = IsStockOutAlready;
				
			item.DeliveryAddress = DeliveryAddress;
				
			item.ContactNo = ContactNo;
				
			item.CreatedOn = CreatedOn;
				
			item.CreatedBy = CreatedBy;
				
			item.ModifiedOn = ModifiedOn;
				
			item.ModifiedBy = ModifiedBy;
				
			item.Deleted = Deleted;
				
	        item.Save(UserName);
	    }
    }
}
