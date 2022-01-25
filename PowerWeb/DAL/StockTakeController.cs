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
    /// Controller class for StockTake
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class StockTakeController
    {
        // Preload our schema..
        StockTake thisSchemaLoad = new StockTake();
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
        public StockTakeCollection FetchAll()
        {
            StockTakeCollection coll = new StockTakeCollection();
            Query qry = new Query(StockTake.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public StockTakeCollection FetchByID(object StockTakeID)
        {
            StockTakeCollection coll = new StockTakeCollection().Where("StockTakeID", StockTakeID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public StockTakeCollection FetchByQuery(Query qry)
        {
            StockTakeCollection coll = new StockTakeCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object StockTakeID)
        {
            return (StockTake.Delete(StockTakeID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object StockTakeID)
        {
            return (StockTake.Destroy(StockTakeID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(DateTime StockTakeDate,string ItemNo,int InventoryLocationID,decimal? StockTakeQty,string TakenBy,string VerifiedBy,string AuthorizedBy,bool IsAdjusted,string Remark,decimal CostOfGoods,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,string AdjustmentHdrRefNo,string Userfld1,string Userfld2,string Userfld3,string Userfld4,string Userfld5,string Userfld6,string Userfld7,string Userfld8,string Userfld9,string Userfld10,bool? Userflag1,bool? Userflag2,bool? Userflag3,bool? Userflag4,bool? Userflag5,decimal? Userfloat1,decimal? Userfloat2,decimal? Userfloat3,decimal? Userfloat4,decimal? Userfloat5,int? Userint1,int? Userint2,int? Userint3,int? Userint4,int? Userint5,decimal? BalQtyAtEntry,decimal? AdjustmentQty,bool? Marked)
	    {
		    StockTake item = new StockTake();
		    
            item.StockTakeDate = StockTakeDate;
            
            item.ItemNo = ItemNo;
            
            item.InventoryLocationID = InventoryLocationID;
            
            item.StockTakeQty = StockTakeQty;
            
            item.TakenBy = TakenBy;
            
            item.VerifiedBy = VerifiedBy;
            
            item.AuthorizedBy = AuthorizedBy;
            
            item.IsAdjusted = IsAdjusted;
            
            item.Remark = Remark;
            
            item.CostOfGoods = CostOfGoods;
            
            item.CreatedOn = CreatedOn;
            
            item.CreatedBy = CreatedBy;
            
            item.ModifiedOn = ModifiedOn;
            
            item.ModifiedBy = ModifiedBy;
            
            item.AdjustmentHdrRefNo = AdjustmentHdrRefNo;
            
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
            
            item.BalQtyAtEntry = BalQtyAtEntry;
            
            item.AdjustmentQty = AdjustmentQty;
            
            item.Marked = Marked;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int StockTakeID,DateTime StockTakeDate,string ItemNo,int InventoryLocationID,decimal? StockTakeQty,string TakenBy,string VerifiedBy,string AuthorizedBy,bool IsAdjusted,string Remark,decimal CostOfGoods,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,string AdjustmentHdrRefNo,string Userfld1,string Userfld2,string Userfld3,string Userfld4,string Userfld5,string Userfld6,string Userfld7,string Userfld8,string Userfld9,string Userfld10,bool? Userflag1,bool? Userflag2,bool? Userflag3,bool? Userflag4,bool? Userflag5,decimal? Userfloat1,decimal? Userfloat2,decimal? Userfloat3,decimal? Userfloat4,decimal? Userfloat5,int? Userint1,int? Userint2,int? Userint3,int? Userint4,int? Userint5,decimal? BalQtyAtEntry,decimal? AdjustmentQty,bool? Marked)
	    {
		    StockTake item = new StockTake();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.StockTakeID = StockTakeID;
				
			item.StockTakeDate = StockTakeDate;
				
			item.ItemNo = ItemNo;
				
			item.InventoryLocationID = InventoryLocationID;
				
			item.StockTakeQty = StockTakeQty;
				
			item.TakenBy = TakenBy;
				
			item.VerifiedBy = VerifiedBy;
				
			item.AuthorizedBy = AuthorizedBy;
				
			item.IsAdjusted = IsAdjusted;
				
			item.Remark = Remark;
				
			item.CostOfGoods = CostOfGoods;
				
			item.CreatedOn = CreatedOn;
				
			item.CreatedBy = CreatedBy;
				
			item.ModifiedOn = ModifiedOn;
				
			item.ModifiedBy = ModifiedBy;
				
			item.AdjustmentHdrRefNo = AdjustmentHdrRefNo;
				
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
				
			item.BalQtyAtEntry = BalQtyAtEntry;
				
			item.AdjustmentQty = AdjustmentQty;
				
			item.Marked = Marked;
				
	        item.Save(UserName);
	    }
    }
}
