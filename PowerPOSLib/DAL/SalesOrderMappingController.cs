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
    /// Controller class for SalesOrderMapping
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class SalesOrderMappingController
    {
        // Preload our schema..
        SalesOrderMapping thisSchemaLoad = new SalesOrderMapping();
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
        public SalesOrderMappingCollection FetchAll()
        {
            SalesOrderMappingCollection coll = new SalesOrderMappingCollection();
            Query qry = new Query(SalesOrderMapping.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public SalesOrderMappingCollection FetchByID(object SalesOrderMappingID)
        {
            SalesOrderMappingCollection coll = new SalesOrderMappingCollection().Where("SalesOrderMappingID", SalesOrderMappingID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public SalesOrderMappingCollection FetchByQuery(Query qry)
        {
            SalesOrderMappingCollection coll = new SalesOrderMappingCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object SalesOrderMappingID)
        {
            return (SalesOrderMapping.Delete(SalesOrderMappingID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object SalesOrderMappingID)
        {
            return (SalesOrderMapping.Destroy(SalesOrderMappingID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string OrderDetID,string PurchaseOrderDetRefNo,decimal? Qty,decimal? QtyApproved,bool Deleted,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy)
	    {
		    SalesOrderMapping item = new SalesOrderMapping();
		    
            item.OrderDetID = OrderDetID;
            
            item.PurchaseOrderDetRefNo = PurchaseOrderDetRefNo;
            
            item.Qty = Qty;
            
            item.QtyApproved = QtyApproved;
            
            item.Deleted = Deleted;
            
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
	    public void Update(int SalesOrderMappingID,string OrderDetID,string PurchaseOrderDetRefNo,decimal? Qty,decimal? QtyApproved,bool Deleted,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy)
	    {
		    SalesOrderMapping item = new SalesOrderMapping();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.SalesOrderMappingID = SalesOrderMappingID;
				
			item.OrderDetID = OrderDetID;
				
			item.PurchaseOrderDetRefNo = PurchaseOrderDetRefNo;
				
			item.Qty = Qty;
				
			item.QtyApproved = QtyApproved;
				
			item.Deleted = Deleted;
				
			item.CreatedOn = CreatedOn;
				
			item.CreatedBy = CreatedBy;
				
			item.ModifiedOn = ModifiedOn;
				
			item.ModifiedBy = ModifiedBy;
				
	        item.Save(UserName);
	    }
    }
}
