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
    /// Controller class for DW_HourlySales
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class DwHourlySaleController
    {
        // Preload our schema..
        DwHourlySale thisSchemaLoad = new DwHourlySale();
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
        public DwHourlySaleCollection FetchAll()
        {
            DwHourlySaleCollection coll = new DwHourlySaleCollection();
            Query qry = new Query(DwHourlySale.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public DwHourlySaleCollection FetchByID(object Id)
        {
            DwHourlySaleCollection coll = new DwHourlySaleCollection().Where("ID", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public DwHourlySaleCollection FetchByQuery(Query qry)
        {
            DwHourlySaleCollection coll = new DwHourlySaleCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (DwHourlySale.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (DwHourlySale.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(DateTime Orderdate,string Outletname,int? Pax,int? Bill,decimal? Grossamount,decimal? Disc,decimal? Afterdisc,decimal? Svccharge,decimal? Befgst,decimal? Gst,decimal? Rounding,decimal? Nettamount,decimal? PointSale,decimal? InstallmentPaymentSale,int? Regenerate,DateTime? LastUpdateOn)
	    {
		    DwHourlySale item = new DwHourlySale();
		    
            item.Orderdate = Orderdate;
            
            item.Outletname = Outletname;
            
            item.Pax = Pax;
            
            item.Bill = Bill;
            
            item.Grossamount = Grossamount;
            
            item.Disc = Disc;
            
            item.Afterdisc = Afterdisc;
            
            item.Svccharge = Svccharge;
            
            item.Befgst = Befgst;
            
            item.Gst = Gst;
            
            item.Rounding = Rounding;
            
            item.Nettamount = Nettamount;
            
            item.PointSale = PointSale;
            
            item.InstallmentPaymentSale = InstallmentPaymentSale;
            
            item.Regenerate = Regenerate;
            
            item.LastUpdateOn = LastUpdateOn;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,DateTime Orderdate,string Outletname,int? Pax,int? Bill,decimal? Grossamount,decimal? Disc,decimal? Afterdisc,decimal? Svccharge,decimal? Befgst,decimal? Gst,decimal? Rounding,decimal? Nettamount,decimal? PointSale,decimal? InstallmentPaymentSale,int? Regenerate,DateTime? LastUpdateOn)
	    {
		    DwHourlySale item = new DwHourlySale();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.Orderdate = Orderdate;
				
			item.Outletname = Outletname;
				
			item.Pax = Pax;
				
			item.Bill = Bill;
				
			item.Grossamount = Grossamount;
				
			item.Disc = Disc;
				
			item.Afterdisc = Afterdisc;
				
			item.Svccharge = Svccharge;
				
			item.Befgst = Befgst;
				
			item.Gst = Gst;
				
			item.Rounding = Rounding;
				
			item.Nettamount = Nettamount;
				
			item.PointSale = PointSale;
				
			item.InstallmentPaymentSale = InstallmentPaymentSale;
				
			item.Regenerate = Regenerate;
				
			item.LastUpdateOn = LastUpdateOn;
				
	        item.Save(UserName);
	    }
    }
}
