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
    /// Controller class for Warranty
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class WarrantyController
    {
        // Preload our schema..
        Warranty thisSchemaLoad = new Warranty();
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
        public WarrantyCollection FetchAll()
        {
            WarrantyCollection coll = new WarrantyCollection();
            Query qry = new Query(Warranty.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public WarrantyCollection FetchByID(object WarrantyID)
        {
            WarrantyCollection coll = new WarrantyCollection().Where("WarrantyID", WarrantyID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public WarrantyCollection FetchByQuery(Query qry)
        {
            WarrantyCollection coll = new WarrantyCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object WarrantyID)
        {
            return (Warranty.Delete(WarrantyID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object WarrantyID)
        {
            return (Warranty.Destroy(WarrantyID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(Guid WarrantyID,string SerialNo,string ModelNo,string OrderDetId,string MembershipNo,string ItemNo,string ProductIdentification,DateTime? DateOfPurchase,DateTime? DateExpiry,string Remark,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy)
	    {
		    Warranty item = new Warranty();
		    
            item.WarrantyID = WarrantyID;
            
            item.SerialNo = SerialNo;
            
            item.ModelNo = ModelNo;
            
            item.OrderDetId = OrderDetId;
            
            item.MembershipNo = MembershipNo;
            
            item.ItemNo = ItemNo;
            
            item.ProductIdentification = ProductIdentification;
            
            item.DateOfPurchase = DateOfPurchase;
            
            item.DateExpiry = DateExpiry;
            
            item.Remark = Remark;
            
            item.CreatedOn = CreatedOn;
            
            item.CreatedBy = CreatedBy;
            
            item.ModifiedOn = ModifiedOn;
            
            item.ModifiedBy = ModifiedBy;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(Guid WarrantyID,string SerialNo,string ModelNo,string OrderDetId,string MembershipNo,string ItemNo,string ProductIdentification,DateTime? DateOfPurchase,DateTime? DateExpiry,string Remark,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy)
	    {
		    Warranty item = new Warranty();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.WarrantyID = WarrantyID;
				
			item.SerialNo = SerialNo;
				
			item.ModelNo = ModelNo;
				
			item.OrderDetId = OrderDetId;
				
			item.MembershipNo = MembershipNo;
				
			item.ItemNo = ItemNo;
				
			item.ProductIdentification = ProductIdentification;
				
			item.DateOfPurchase = DateOfPurchase;
				
			item.DateExpiry = DateExpiry;
				
			item.Remark = Remark;
				
			item.CreatedOn = CreatedOn;
				
			item.CreatedBy = CreatedBy;
				
			item.ModifiedOn = ModifiedOn;
				
			item.ModifiedBy = ModifiedBy;
				
	        item.Save(UserName);
	    }
    }
}
