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
    /// Controller class for CommissionBasedOnQty
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class CommissionBasedOnQtyController
    {
        // Preload our schema..
        CommissionBasedOnQty thisSchemaLoad = new CommissionBasedOnQty();
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
        public CommissionBasedOnQtyCollection FetchAll()
        {
            CommissionBasedOnQtyCollection coll = new CommissionBasedOnQtyCollection();
            Query qry = new Query(CommissionBasedOnQty.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public CommissionBasedOnQtyCollection FetchByID(object UniqueID)
        {
            CommissionBasedOnQtyCollection coll = new CommissionBasedOnQtyCollection().Where("UniqueID", UniqueID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public CommissionBasedOnQtyCollection FetchByQuery(Query qry)
        {
            CommissionBasedOnQtyCollection coll = new CommissionBasedOnQtyCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object UniqueID)
        {
            return (CommissionBasedOnQty.Delete(UniqueID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object UniqueID)
        {
            return (CommissionBasedOnQty.Destroy(UniqueID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(int SalesGroupID,string ItemNo,decimal Quantity,decimal AmountCommission,string CreatedBy,DateTime CreatedOn,string ModifiedBy,DateTime? ModifiedOn,string CommissionType)
	    {
		    CommissionBasedOnQty item = new CommissionBasedOnQty();
		    
            item.SalesGroupID = SalesGroupID;
            
            item.ItemNo = ItemNo;
            
            item.Quantity = Quantity;
            
            item.AmountCommission = AmountCommission;
            
            item.CreatedBy = CreatedBy;
            
            item.CreatedOn = CreatedOn;
            
            item.ModifiedBy = ModifiedBy;
            
            item.ModifiedOn = ModifiedOn;
            
            item.CommissionType = CommissionType;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int UniqueID,int SalesGroupID,string ItemNo,decimal Quantity,decimal AmountCommission,string CreatedBy,DateTime CreatedOn,string ModifiedBy,DateTime? ModifiedOn,string CommissionType)
	    {
		    CommissionBasedOnQty item = new CommissionBasedOnQty();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.UniqueID = UniqueID;
				
			item.SalesGroupID = SalesGroupID;
				
			item.ItemNo = ItemNo;
				
			item.Quantity = Quantity;
				
			item.AmountCommission = AmountCommission;
				
			item.CreatedBy = CreatedBy;
				
			item.CreatedOn = CreatedOn;
				
			item.ModifiedBy = ModifiedBy;
				
			item.ModifiedOn = ModifiedOn;
				
			item.CommissionType = CommissionType;
				
	        item.Save(UserName);
	    }
    }
}
