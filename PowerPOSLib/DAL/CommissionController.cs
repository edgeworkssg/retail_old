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
    /// Controller class for Commission
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class CommissionController
    {
        // Preload our schema..
        Commission thisSchemaLoad = new Commission();
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
        public CommissionCollection FetchAll()
        {
            CommissionCollection coll = new CommissionCollection();
            Query qry = new Query(Commission.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public CommissionCollection FetchByID(object CommissionID)
        {
            CommissionCollection coll = new CommissionCollection().Where("CommissionID", CommissionID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public CommissionCollection FetchByQuery(Query qry)
        {
            CommissionCollection coll = new CommissionCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object CommissionID)
        {
            return (Commission.Delete(CommissionID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object CommissionID)
        {
            return (Commission.Destroy(CommissionID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(int? SalesGroupID,string CommissionType,string CategoryName,string ItemNo,string CommissionBasedOn,double? QuantityFrom,double? QuantityTo,decimal? AmountCommission,decimal? AmountFrom,decimal? AmountTo,double? PercentageCommission,string CreatedBy,DateTime? CreatedOn,string ModifiedBy,DateTime? ModifiedOn)
	    {
		    Commission item = new Commission();
		    
            item.SalesGroupID = SalesGroupID;
            
            item.CommissionType = CommissionType;
            
            item.CategoryName = CategoryName;
            
            item.ItemNo = ItemNo;
            
            item.CommissionBasedOn = CommissionBasedOn;
            
            item.QuantityFrom = QuantityFrom;
            
            item.QuantityTo = QuantityTo;
            
            item.AmountCommission = AmountCommission;
            
            item.AmountFrom = AmountFrom;
            
            item.AmountTo = AmountTo;
            
            item.PercentageCommission = PercentageCommission;
            
            item.CreatedBy = CreatedBy;
            
            item.CreatedOn = CreatedOn;
            
            item.ModifiedBy = ModifiedBy;
            
            item.ModifiedOn = ModifiedOn;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int CommissionID,int? SalesGroupID,string CommissionType,string CategoryName,string ItemNo,string CommissionBasedOn,double? QuantityFrom,double? QuantityTo,decimal? AmountCommission,decimal? AmountFrom,decimal? AmountTo,double? PercentageCommission,string CreatedBy,DateTime? CreatedOn,string ModifiedBy,DateTime? ModifiedOn)
	    {
		    Commission item = new Commission();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.CommissionID = CommissionID;
				
			item.SalesGroupID = SalesGroupID;
				
			item.CommissionType = CommissionType;
				
			item.CategoryName = CategoryName;
				
			item.ItemNo = ItemNo;
				
			item.CommissionBasedOn = CommissionBasedOn;
				
			item.QuantityFrom = QuantityFrom;
				
			item.QuantityTo = QuantityTo;
				
			item.AmountCommission = AmountCommission;
				
			item.AmountFrom = AmountFrom;
				
			item.AmountTo = AmountTo;
				
			item.PercentageCommission = PercentageCommission;
				
			item.CreatedBy = CreatedBy;
				
			item.CreatedOn = CreatedOn;
				
			item.ModifiedBy = ModifiedBy;
				
			item.ModifiedOn = ModifiedOn;
				
	        item.Save(UserName);
	    }
    }
}
