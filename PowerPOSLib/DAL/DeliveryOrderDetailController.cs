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
    /// Controller class for DeliveryOrderDetails
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class DeliveryOrderDetailController
    {
        // Preload our schema..
        DeliveryOrderDetail thisSchemaLoad = new DeliveryOrderDetail();
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
        public DeliveryOrderDetailCollection FetchAll()
        {
            DeliveryOrderDetailCollection coll = new DeliveryOrderDetailCollection();
            Query qry = new Query(DeliveryOrderDetail.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public DeliveryOrderDetailCollection FetchByID(object DetailsID)
        {
            DeliveryOrderDetailCollection coll = new DeliveryOrderDetailCollection().Where("DetailsID", DetailsID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public DeliveryOrderDetailCollection FetchByQuery(Query qry)
        {
            DeliveryOrderDetailCollection coll = new DeliveryOrderDetailCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object DetailsID)
        {
            return (DeliveryOrderDetail.Delete(DetailsID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object DetailsID)
        {
            return (DeliveryOrderDetail.Destroy(DetailsID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string DetailsID,string ItemNo,decimal? Quantity,string SerialNumber,string Dohdrid,string OrderDetID,string Remarks,DateTime CreatedOn,DateTime ModifiedOn,string CreatedBy,string ModifiedBy,Guid UniqueID,bool? Deleted,string InventoryHdrRefNo)
	    {
		    DeliveryOrderDetail item = new DeliveryOrderDetail();
		    
            item.DetailsID = DetailsID;
            
            item.ItemNo = ItemNo;
            
            item.Quantity = Quantity;
            
            item.SerialNumber = SerialNumber;
            
            item.Dohdrid = Dohdrid;
            
            item.OrderDetID = OrderDetID;
            
            item.Remarks = Remarks;
            
            item.CreatedOn = CreatedOn;
            
            item.ModifiedOn = ModifiedOn;
            
            item.CreatedBy = CreatedBy;
            
            item.ModifiedBy = ModifiedBy;
            
            item.UniqueID = UniqueID;
            
            item.Deleted = Deleted;
            
            item.InventoryHdrRefNo = InventoryHdrRefNo;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(string DetailsID,string ItemNo,decimal? Quantity,string SerialNumber,string Dohdrid,string OrderDetID,string Remarks,DateTime CreatedOn,DateTime ModifiedOn,string CreatedBy,string ModifiedBy,Guid UniqueID,bool? Deleted,string InventoryHdrRefNo)
	    {
		    DeliveryOrderDetail item = new DeliveryOrderDetail();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.DetailsID = DetailsID;
				
			item.ItemNo = ItemNo;
				
			item.Quantity = Quantity;
				
			item.SerialNumber = SerialNumber;
				
			item.Dohdrid = Dohdrid;
				
			item.OrderDetID = OrderDetID;
				
			item.Remarks = Remarks;
				
			item.CreatedOn = CreatedOn;
				
			item.ModifiedOn = ModifiedOn;
				
			item.CreatedBy = CreatedBy;
				
			item.ModifiedBy = ModifiedBy;
				
			item.UniqueID = UniqueID;
				
			item.Deleted = Deleted;
				
			item.InventoryHdrRefNo = InventoryHdrRefNo;
				
	        item.Save(UserName);
	    }
    }
}
