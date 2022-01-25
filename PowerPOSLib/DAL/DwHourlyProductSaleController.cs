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
    /// Controller class for DW_HourlyProductSales
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class DwHourlyProductSaleController
    {
        // Preload our schema..
        DwHourlyProductSale thisSchemaLoad = new DwHourlyProductSale();
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
        public DwHourlyProductSaleCollection FetchAll()
        {
            DwHourlyProductSaleCollection coll = new DwHourlyProductSaleCollection();
            Query qry = new Query(DwHourlyProductSale.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public DwHourlyProductSaleCollection FetchByID(object Id)
        {
            DwHourlyProductSaleCollection coll = new DwHourlyProductSaleCollection().Where("ID", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public DwHourlyProductSaleCollection FetchByQuery(Query qry)
        {
            DwHourlyProductSaleCollection coll = new DwHourlyProductSaleCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (DwHourlyProductSale.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (DwHourlyProductSale.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(DateTime Orderdate,string Outletname,string Itemno,decimal? Quantity,decimal? Amount,int? Regenerate,DateTime? LastUpdateOn)
	    {
		    DwHourlyProductSale item = new DwHourlyProductSale();
		    
            item.Orderdate = Orderdate;
            
            item.Outletname = Outletname;
            
            item.Itemno = Itemno;
            
            item.Quantity = Quantity;
            
            item.Amount = Amount;
            
            item.Regenerate = Regenerate;
            
            item.LastUpdateOn = LastUpdateOn;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,DateTime Orderdate,string Outletname,string Itemno,decimal? Quantity,decimal? Amount,int? Regenerate,DateTime? LastUpdateOn)
	    {
		    DwHourlyProductSale item = new DwHourlyProductSale();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.Orderdate = Orderdate;
				
			item.Outletname = Outletname;
				
			item.Itemno = Itemno;
				
			item.Quantity = Quantity;
				
			item.Amount = Amount;
				
			item.Regenerate = Regenerate;
				
			item.LastUpdateOn = LastUpdateOn;
				
	        item.Save(UserName);
	    }
    }
}
