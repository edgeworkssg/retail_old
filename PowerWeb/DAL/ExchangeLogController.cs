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
    /// Controller class for ExchangeLog
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class ExchangeLogController
    {
        // Preload our schema..
        ExchangeLog thisSchemaLoad = new ExchangeLog();
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
        public ExchangeLogCollection FetchAll()
        {
            ExchangeLogCollection coll = new ExchangeLogCollection();
            Query qry = new Query(ExchangeLog.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public ExchangeLogCollection FetchByID(object ExchangeLogID)
        {
            ExchangeLogCollection coll = new ExchangeLogCollection().Where("ExchangeLogID", ExchangeLogID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public ExchangeLogCollection FetchByQuery(Query qry)
        {
            ExchangeLogCollection coll = new ExchangeLogCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object ExchangeLogID)
        {
            return (ExchangeLog.Delete(ExchangeLogID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object ExchangeLogID)
        {
            return (ExchangeLog.Destroy(ExchangeLogID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(DateTime ExchangeDateTime,string OrderHdrID,string ItemNo,int Qty,string NewOrderHdrID,string NewItemNo,int NewQty,string CashierID,string OutletName,int PointOfSaleID,bool UndoStockOut,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,Guid UniqueID,bool? IsInventoryUpdated,string Userfld1,string Userfld2,string Userfld3,string Userfld4,string Userfld5,string Userfld6,string Userfld7,string Userfld8,string Userfld9,string Userfld10,bool? Userflag1,bool? Userflag2,bool? Userflag3,bool? Userflag4,bool? Userflag5,decimal? Userfloat1,decimal? Userfloat2,decimal? Userfloat3,decimal? Userfloat4,decimal? Userfloat5,int? Userint1,int? Userint2,int? Userint3,int? Userint4,int? Userint5)
	    {
		    ExchangeLog item = new ExchangeLog();
		    
            item.ExchangeDateTime = ExchangeDateTime;
            
            item.OrderHdrID = OrderHdrID;
            
            item.ItemNo = ItemNo;
            
            item.Qty = Qty;
            
            item.NewOrderHdrID = NewOrderHdrID;
            
            item.NewItemNo = NewItemNo;
            
            item.NewQty = NewQty;
            
            item.CashierID = CashierID;
            
            item.OutletName = OutletName;
            
            item.PointOfSaleID = PointOfSaleID;
            
            item.UndoStockOut = UndoStockOut;
            
            item.CreatedOn = CreatedOn;
            
            item.CreatedBy = CreatedBy;
            
            item.ModifiedOn = ModifiedOn;
            
            item.ModifiedBy = ModifiedBy;
            
            item.UniqueID = UniqueID;
            
            item.IsInventoryUpdated = IsInventoryUpdated;
            
            item.Userfld1 = Userfld1;
            
            item.Userfld2 = Userfld2;
            
            item.Userfld3 = Userfld3;
            
            item.Userfld4 = Userfld4;
            
            item.Userfld5 = Userfld5;
            
            item.Userfld6 = Userfld6;
            
            item.Userfld7 = Userfld7;
            
            item.Userfld8 = Userfld8;
            
            item.Userfld9 = Userfld9;
            
            item.Userfld10 = Userfld10;
            
            item.Userflag1 = Userflag1;
            
            item.Userflag2 = Userflag2;
            
            item.Userflag3 = Userflag3;
            
            item.Userflag4 = Userflag4;
            
            item.Userflag5 = Userflag5;
            
            item.Userfloat1 = Userfloat1;
            
            item.Userfloat2 = Userfloat2;
            
            item.Userfloat3 = Userfloat3;
            
            item.Userfloat4 = Userfloat4;
            
            item.Userfloat5 = Userfloat5;
            
            item.Userint1 = Userint1;
            
            item.Userint2 = Userint2;
            
            item.Userint3 = Userint3;
            
            item.Userint4 = Userint4;
            
            item.Userint5 = Userint5;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int ExchangeLogID,DateTime ExchangeDateTime,string OrderHdrID,string ItemNo,int Qty,string NewOrderHdrID,string NewItemNo,int NewQty,string CashierID,string OutletName,int PointOfSaleID,bool UndoStockOut,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,Guid UniqueID,bool? IsInventoryUpdated,string Userfld1,string Userfld2,string Userfld3,string Userfld4,string Userfld5,string Userfld6,string Userfld7,string Userfld8,string Userfld9,string Userfld10,bool? Userflag1,bool? Userflag2,bool? Userflag3,bool? Userflag4,bool? Userflag5,decimal? Userfloat1,decimal? Userfloat2,decimal? Userfloat3,decimal? Userfloat4,decimal? Userfloat5,int? Userint1,int? Userint2,int? Userint3,int? Userint4,int? Userint5)
	    {
		    ExchangeLog item = new ExchangeLog();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.ExchangeLogID = ExchangeLogID;
				
			item.ExchangeDateTime = ExchangeDateTime;
				
			item.OrderHdrID = OrderHdrID;
				
			item.ItemNo = ItemNo;
				
			item.Qty = Qty;
				
			item.NewOrderHdrID = NewOrderHdrID;
				
			item.NewItemNo = NewItemNo;
				
			item.NewQty = NewQty;
				
			item.CashierID = CashierID;
				
			item.OutletName = OutletName;
				
			item.PointOfSaleID = PointOfSaleID;
				
			item.UndoStockOut = UndoStockOut;
				
			item.CreatedOn = CreatedOn;
				
			item.CreatedBy = CreatedBy;
				
			item.ModifiedOn = ModifiedOn;
				
			item.ModifiedBy = ModifiedBy;
				
			item.UniqueID = UniqueID;
				
			item.IsInventoryUpdated = IsInventoryUpdated;
				
			item.Userfld1 = Userfld1;
				
			item.Userfld2 = Userfld2;
				
			item.Userfld3 = Userfld3;
				
			item.Userfld4 = Userfld4;
				
			item.Userfld5 = Userfld5;
				
			item.Userfld6 = Userfld6;
				
			item.Userfld7 = Userfld7;
				
			item.Userfld8 = Userfld8;
				
			item.Userfld9 = Userfld9;
				
			item.Userfld10 = Userfld10;
				
			item.Userflag1 = Userflag1;
				
			item.Userflag2 = Userflag2;
				
			item.Userflag3 = Userflag3;
				
			item.Userflag4 = Userflag4;
				
			item.Userflag5 = Userflag5;
				
			item.Userfloat1 = Userfloat1;
				
			item.Userfloat2 = Userfloat2;
				
			item.Userfloat3 = Userfloat3;
				
			item.Userfloat4 = Userfloat4;
				
			item.Userfloat5 = Userfloat5;
				
			item.Userint1 = Userint1;
				
			item.Userint2 = Userint2;
				
			item.Userint3 = Userint3;
				
			item.Userint4 = Userint4;
				
			item.Userint5 = Userint5;
				
	        item.Save(UserName);
	    }
    }
}
