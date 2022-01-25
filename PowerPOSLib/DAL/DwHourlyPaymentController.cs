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
    /// Controller class for DW_HourlyPayment
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class DwHourlyPaymentController
    {
        // Preload our schema..
        DwHourlyPayment thisSchemaLoad = new DwHourlyPayment();
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
        public DwHourlyPaymentCollection FetchAll()
        {
            DwHourlyPaymentCollection coll = new DwHourlyPaymentCollection();
            Query qry = new Query(DwHourlyPayment.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public DwHourlyPaymentCollection FetchByID(object Id)
        {
            DwHourlyPaymentCollection coll = new DwHourlyPaymentCollection().Where("ID", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public DwHourlyPaymentCollection FetchByQuery(Query qry)
        {
            DwHourlyPaymentCollection coll = new DwHourlyPaymentCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (DwHourlyPayment.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (DwHourlyPayment.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(DateTime OrderDate,string OutletName,decimal? PayByCash,decimal? Pay01,decimal? Pay02,decimal? Pay03,decimal? Pay04,decimal? Pay05,decimal? Pay06,decimal? Pay07,decimal? Pay08,decimal? Pay09,decimal? Pay10,decimal? Pay11,decimal? Pay12,decimal? Pay13,decimal? Pay14,decimal? Pay15,decimal? Pay16,decimal? Pay17,decimal? Pay18,decimal? Pay19,decimal? Pay20,decimal? Pay21,decimal? Pay22,decimal? Pay23,decimal? Pay24,decimal? Pay25,decimal? Pay26,decimal? Pay27,decimal? Pay28,decimal? Pay29,decimal? Pay30,decimal? Pay31,decimal? Pay32,decimal? Pay33,decimal? Pay34,decimal? Pay35,decimal? Pay36,decimal? Pay37,decimal? Pay38,decimal? Pay39,decimal? Pay40,decimal? PayOthers,decimal? Totalpayment,decimal? PointAllocated,decimal? PayByInstallment,decimal? PayByPoint,int? Regenerate,DateTime? LastUpdateOn)
	    {
		    DwHourlyPayment item = new DwHourlyPayment();
		    
            item.OrderDate = OrderDate;
            
            item.OutletName = OutletName;
            
            item.PayByCash = PayByCash;
            
            item.Pay01 = Pay01;
            
            item.Pay02 = Pay02;
            
            item.Pay03 = Pay03;
            
            item.Pay04 = Pay04;
            
            item.Pay05 = Pay05;
            
            item.Pay06 = Pay06;
            
            item.Pay07 = Pay07;
            
            item.Pay08 = Pay08;
            
            item.Pay09 = Pay09;
            
            item.Pay10 = Pay10;
            
            item.Pay11 = Pay11;
            
            item.Pay12 = Pay12;
            
            item.Pay13 = Pay13;
            
            item.Pay14 = Pay14;
            
            item.Pay15 = Pay15;
            
            item.Pay16 = Pay16;
            
            item.Pay17 = Pay17;
            
            item.Pay18 = Pay18;
            
            item.Pay19 = Pay19;
            
            item.Pay20 = Pay20;
            
            item.Pay21 = Pay21;
            
            item.Pay22 = Pay22;
            
            item.Pay23 = Pay23;
            
            item.Pay24 = Pay24;
            
            item.Pay25 = Pay25;
            
            item.Pay26 = Pay26;
            
            item.Pay27 = Pay27;
            
            item.Pay28 = Pay28;
            
            item.Pay29 = Pay29;
            
            item.Pay30 = Pay30;
            
            item.Pay31 = Pay31;
            
            item.Pay32 = Pay32;
            
            item.Pay33 = Pay33;
            
            item.Pay34 = Pay34;
            
            item.Pay35 = Pay35;
            
            item.Pay36 = Pay36;
            
            item.Pay37 = Pay37;
            
            item.Pay38 = Pay38;
            
            item.Pay39 = Pay39;
            
            item.Pay40 = Pay40;
            
            item.PayOthers = PayOthers;
            
            item.Totalpayment = Totalpayment;
            
            item.PointAllocated = PointAllocated;
            
            item.PayByInstallment = PayByInstallment;
            
            item.PayByPoint = PayByPoint;
            
            item.Regenerate = Regenerate;
            
            item.LastUpdateOn = LastUpdateOn;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,DateTime OrderDate,string OutletName,decimal? PayByCash,decimal? Pay01,decimal? Pay02,decimal? Pay03,decimal? Pay04,decimal? Pay05,decimal? Pay06,decimal? Pay07,decimal? Pay08,decimal? Pay09,decimal? Pay10,decimal? Pay11,decimal? Pay12,decimal? Pay13,decimal? Pay14,decimal? Pay15,decimal? Pay16,decimal? Pay17,decimal? Pay18,decimal? Pay19,decimal? Pay20,decimal? Pay21,decimal? Pay22,decimal? Pay23,decimal? Pay24,decimal? Pay25,decimal? Pay26,decimal? Pay27,decimal? Pay28,decimal? Pay29,decimal? Pay30,decimal? Pay31,decimal? Pay32,decimal? Pay33,decimal? Pay34,decimal? Pay35,decimal? Pay36,decimal? Pay37,decimal? Pay38,decimal? Pay39,decimal? Pay40,decimal? PayOthers,decimal? Totalpayment,decimal? PointAllocated,decimal? PayByInstallment,decimal? PayByPoint,int? Regenerate,DateTime? LastUpdateOn)
	    {
		    DwHourlyPayment item = new DwHourlyPayment();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.OrderDate = OrderDate;
				
			item.OutletName = OutletName;
				
			item.PayByCash = PayByCash;
				
			item.Pay01 = Pay01;
				
			item.Pay02 = Pay02;
				
			item.Pay03 = Pay03;
				
			item.Pay04 = Pay04;
				
			item.Pay05 = Pay05;
				
			item.Pay06 = Pay06;
				
			item.Pay07 = Pay07;
				
			item.Pay08 = Pay08;
				
			item.Pay09 = Pay09;
				
			item.Pay10 = Pay10;
				
			item.Pay11 = Pay11;
				
			item.Pay12 = Pay12;
				
			item.Pay13 = Pay13;
				
			item.Pay14 = Pay14;
				
			item.Pay15 = Pay15;
				
			item.Pay16 = Pay16;
				
			item.Pay17 = Pay17;
				
			item.Pay18 = Pay18;
				
			item.Pay19 = Pay19;
				
			item.Pay20 = Pay20;
				
			item.Pay21 = Pay21;
				
			item.Pay22 = Pay22;
				
			item.Pay23 = Pay23;
				
			item.Pay24 = Pay24;
				
			item.Pay25 = Pay25;
				
			item.Pay26 = Pay26;
				
			item.Pay27 = Pay27;
				
			item.Pay28 = Pay28;
				
			item.Pay29 = Pay29;
				
			item.Pay30 = Pay30;
				
			item.Pay31 = Pay31;
				
			item.Pay32 = Pay32;
				
			item.Pay33 = Pay33;
				
			item.Pay34 = Pay34;
				
			item.Pay35 = Pay35;
				
			item.Pay36 = Pay36;
				
			item.Pay37 = Pay37;
				
			item.Pay38 = Pay38;
				
			item.Pay39 = Pay39;
				
			item.Pay40 = Pay40;
				
			item.PayOthers = PayOthers;
				
			item.Totalpayment = Totalpayment;
				
			item.PointAllocated = PointAllocated;
				
			item.PayByInstallment = PayByInstallment;
				
			item.PayByPoint = PayByPoint;
				
			item.Regenerate = Regenerate;
				
			item.LastUpdateOn = LastUpdateOn;
				
	        item.Save(UserName);
	    }
    }
}
