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
    /// Controller class for SalesCommissionDetails_Deduction
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class SalesCommissionDetailsDeductionController
    {
        // Preload our schema..
        SalesCommissionDetailsDeduction thisSchemaLoad = new SalesCommissionDetailsDeduction();
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
        public SalesCommissionDetailsDeductionCollection FetchAll()
        {
            SalesCommissionDetailsDeductionCollection coll = new SalesCommissionDetailsDeductionCollection();
            Query qry = new Query(SalesCommissionDetailsDeduction.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public SalesCommissionDetailsDeductionCollection FetchByID(object Id)
        {
            SalesCommissionDetailsDeductionCollection coll = new SalesCommissionDetailsDeductionCollection().Where("ID", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public SalesCommissionDetailsDeductionCollection FetchByQuery(Query qry)
        {
            SalesCommissionDetailsDeductionCollection coll = new SalesCommissionDetailsDeductionCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (SalesCommissionDetailsDeduction.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (SalesCommissionDetailsDeduction.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string Month,string Staff,decimal? Deduction)
	    {
		    SalesCommissionDetailsDeduction item = new SalesCommissionDetailsDeduction();
		    
            item.Month = Month;
            
            item.Staff = Staff;
            
            item.Deduction = Deduction;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,string Month,string Staff,decimal? Deduction)
	    {
		    SalesCommissionDetailsDeduction item = new SalesCommissionDetailsDeduction();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.Month = Month;
				
			item.Staff = Staff;
				
			item.Deduction = Deduction;
				
	        item.Save(UserName);
	    }
    }
}
