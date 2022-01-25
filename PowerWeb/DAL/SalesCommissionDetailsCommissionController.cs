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
    /// Controller class for SalesCommissionDetails_Commission
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class SalesCommissionDetailsCommissionController
    {
        // Preload our schema..
        SalesCommissionDetailsCommission thisSchemaLoad = new SalesCommissionDetailsCommission();
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
        public SalesCommissionDetailsCommissionCollection FetchAll()
        {
            SalesCommissionDetailsCommissionCollection coll = new SalesCommissionDetailsCommissionCollection();
            Query qry = new Query(SalesCommissionDetailsCommission.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public SalesCommissionDetailsCommissionCollection FetchByID(object Id)
        {
            SalesCommissionDetailsCommissionCollection coll = new SalesCommissionDetailsCommissionCollection().Where("ID", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public SalesCommissionDetailsCommissionCollection FetchByQuery(Query qry)
        {
            SalesCommissionDetailsCommissionCollection coll = new SalesCommissionDetailsCommissionCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (SalesCommissionDetailsCommission.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (SalesCommissionDetailsCommission.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string Month,string Staff,string Scheme,string CommissionText,decimal? CommissionValue)
	    {
		    SalesCommissionDetailsCommission item = new SalesCommissionDetailsCommission();
		    
            item.Month = Month;
            
            item.Staff = Staff;
            
            item.Scheme = Scheme;
            
            item.CommissionText = CommissionText;
            
            item.CommissionValue = CommissionValue;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,string Month,string Staff,string Scheme,string CommissionText,decimal? CommissionValue)
	    {
		    SalesCommissionDetailsCommission item = new SalesCommissionDetailsCommission();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.Month = Month;
				
			item.Staff = Staff;
				
			item.Scheme = Scheme;
				
			item.CommissionText = CommissionText;
				
			item.CommissionValue = CommissionValue;
				
	        item.Save(UserName);
	    }
    }
}
