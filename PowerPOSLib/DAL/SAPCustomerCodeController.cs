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
    /// Controller class for SAPCustomerCode
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class SAPCustomerCodeController
    {
        // Preload our schema..
        SAPCustomerCode thisSchemaLoad = new SAPCustomerCode();
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
        public SAPCustomerCodeCollection FetchAll()
        {
            SAPCustomerCodeCollection coll = new SAPCustomerCodeCollection();
            Query qry = new Query(SAPCustomerCode.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public SAPCustomerCodeCollection FetchByID(object Id)
        {
            SAPCustomerCodeCollection coll = new SAPCustomerCodeCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public SAPCustomerCodeCollection FetchByQuery(Query qry)
        {
            SAPCustomerCodeCollection coll = new SAPCustomerCodeCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (SAPCustomerCode.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (SAPCustomerCode.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string SalesType,string PaymentType,int? PointOfSaleID,string CustomerCode,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,bool Deleted)
	    {
		    SAPCustomerCode item = new SAPCustomerCode();
		    
            item.SalesType = SalesType;
            
            item.PaymentType = PaymentType;
            
            item.PointOfSaleID = PointOfSaleID;
            
            item.CustomerCode = CustomerCode;
            
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
	    public void Update(int Id,string SalesType,string PaymentType,int? PointOfSaleID,string CustomerCode,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,bool Deleted)
	    {
		    SAPCustomerCode item = new SAPCustomerCode();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.SalesType = SalesType;
				
			item.PaymentType = PaymentType;
				
			item.PointOfSaleID = PointOfSaleID;
				
			item.CustomerCode = CustomerCode;
				
			item.CreatedOn = CreatedOn;
				
			item.CreatedBy = CreatedBy;
				
			item.ModifiedOn = ModifiedOn;
				
			item.ModifiedBy = ModifiedBy;
				
			item.Deleted = Deleted;
				
	        item.Save(UserName);
	    }
    }
}
