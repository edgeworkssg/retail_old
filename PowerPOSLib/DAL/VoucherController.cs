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
    /// Controller class for Vouchers
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class VoucherController
    {
        // Preload our schema..
        Voucher thisSchemaLoad = new Voucher();
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
        public VoucherCollection FetchAll()
        {
            VoucherCollection coll = new VoucherCollection();
            Query qry = new Query(Voucher.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public VoucherCollection FetchByID(object VoucherID)
        {
            VoucherCollection coll = new VoucherCollection().Where("VoucherID", VoucherID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public VoucherCollection FetchByQuery(Query qry)
        {
            VoucherCollection coll = new VoucherCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object VoucherID)
        {
            return (Voucher.Delete(VoucherID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object VoucherID)
        {
            return (Voucher.Destroy(VoucherID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(Guid VoucherID,string VoucherNo,decimal Amount,string Remark,int VoucherStatusId,DateTime? DateIssued,DateTime? DateSold,DateTime? DateRedeemed,DateTime? ExpiryDate,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,bool Deleted,decimal? RedeemAmount,DateTime? DateCanceled,int? VoucherHeaderID,string Outlet)
	    {
		    Voucher item = new Voucher();
		    
            item.VoucherID = VoucherID;
            
            item.VoucherNo = VoucherNo;
            
            item.Amount = Amount;
            
            item.Remark = Remark;
            
            item.VoucherStatusId = VoucherStatusId;
            
            item.DateIssued = DateIssued;
            
            item.DateSold = DateSold;
            
            item.DateRedeemed = DateRedeemed;
            
            item.ExpiryDate = ExpiryDate;
            
            item.CreatedOn = CreatedOn;
            
            item.CreatedBy = CreatedBy;
            
            item.ModifiedOn = ModifiedOn;
            
            item.ModifiedBy = ModifiedBy;
            
            item.Deleted = Deleted;
            
            item.RedeemAmount = RedeemAmount;
            
            item.DateCanceled = DateCanceled;
            
            item.VoucherHeaderID = VoucherHeaderID;
            
            item.Outlet = Outlet;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(Guid VoucherID,string VoucherNo,decimal Amount,string Remark,int VoucherStatusId,DateTime? DateIssued,DateTime? DateSold,DateTime? DateRedeemed,DateTime? ExpiryDate,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,bool Deleted,decimal? RedeemAmount,DateTime? DateCanceled,int? VoucherHeaderID,string Outlet)
	    {
		    Voucher item = new Voucher();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.VoucherID = VoucherID;
				
			item.VoucherNo = VoucherNo;
				
			item.Amount = Amount;
				
			item.Remark = Remark;
				
			item.VoucherStatusId = VoucherStatusId;
				
			item.DateIssued = DateIssued;
				
			item.DateSold = DateSold;
				
			item.DateRedeemed = DateRedeemed;
				
			item.ExpiryDate = ExpiryDate;
				
			item.CreatedOn = CreatedOn;
				
			item.CreatedBy = CreatedBy;
				
			item.ModifiedOn = ModifiedOn;
				
			item.ModifiedBy = ModifiedBy;
				
			item.Deleted = Deleted;
				
			item.RedeemAmount = RedeemAmount;
				
			item.DateCanceled = DateCanceled;
				
			item.VoucherHeaderID = VoucherHeaderID;
				
			item.Outlet = Outlet;
				
	        item.Save(UserName);
	    }
    }
}
