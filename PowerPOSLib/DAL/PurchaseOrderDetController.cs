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
    /// Controller class for PurchaseOrderDet
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class PurchaseOrderDetController
    {
        // Preload our schema..
        PurchaseOrderDet thisSchemaLoad = new PurchaseOrderDet();
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
        public PurchaseOrderDetCollection FetchAll()
        {
            PurchaseOrderDetCollection coll = new PurchaseOrderDetCollection();
            Query qry = new Query(PurchaseOrderDet.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public PurchaseOrderDetCollection FetchByID(object PurchaseOrderDetRefNo)
        {
            PurchaseOrderDetCollection coll = new PurchaseOrderDetCollection().Where("PurchaseOrderDetRefNo", PurchaseOrderDetRefNo).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public PurchaseOrderDetCollection FetchByQuery(Query qry)
        {
            PurchaseOrderDetCollection coll = new PurchaseOrderDetCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object PurchaseOrderDetRefNo)
        {
            return (PurchaseOrderDet.Delete(PurchaseOrderDetRefNo) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object PurchaseOrderDetRefNo)
        {
            return (PurchaseOrderDet.Destroy(PurchaseOrderDetRefNo) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string PurchaseOrderDetRefNo,string ItemNo,string PurchaseOrderHdrRefNo,DateTime? ExpiryDate,decimal? Quantity,decimal? RemainingQty,decimal FactoryPrice,double Gst,decimal CostOfGoods,string Remark,decimal? Discount,string CreatedBy,DateTime? CreatedOn,string ModifiedBy,DateTime? ModifiedOn,Guid UniqueID,string Userfld1,string Userfld2,string Userfld3,string Userfld4,string Userfld5,string Userfld6,string Userfld7,string Userfld8,string Userfld9,string Userfld10,bool? Userflag1,bool? Userflag2,bool? Userflag3,bool? Userflag4,bool? Userflag5,decimal? Userfloat1,decimal? Userfloat2,decimal? Userfloat3,decimal? Userfloat4,decimal? Userfloat5,int? Userint1,int? Userint2,int? Userint3,int? Userint4,int? Userint5,int BalanceBefore,int BalanceAfter,bool? Deleted,string Stockinrefno,bool? Isdiscrepancy,decimal? Userfloat6,decimal? Userfloat7,decimal? Userfloat8,decimal? Userfloat9,decimal? Userfloat10)
	    {
		    PurchaseOrderDet item = new PurchaseOrderDet();
		    
            item.PurchaseOrderDetRefNo = PurchaseOrderDetRefNo;
            
            item.ItemNo = ItemNo;
            
            item.PurchaseOrderHdrRefNo = PurchaseOrderHdrRefNo;
            
            item.ExpiryDate = ExpiryDate;
            
            item.Quantity = Quantity;
            
            item.RemainingQty = RemainingQty;
            
            item.FactoryPrice = FactoryPrice;
            
            item.Gst = Gst;
            
            item.CostOfGoods = CostOfGoods;
            
            item.Remark = Remark;
            
            item.Discount = Discount;
            
            item.CreatedBy = CreatedBy;
            
            item.CreatedOn = CreatedOn;
            
            item.ModifiedBy = ModifiedBy;
            
            item.ModifiedOn = ModifiedOn;
            
            item.UniqueID = UniqueID;
            
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
            
            item.BalanceBefore = BalanceBefore;
            
            item.BalanceAfter = BalanceAfter;
            
            item.Deleted = Deleted;
            
            item.Stockinrefno = Stockinrefno;
            
            item.Isdiscrepancy = Isdiscrepancy;
            
            item.Userfloat6 = Userfloat6;
            
            item.Userfloat7 = Userfloat7;
            
            item.Userfloat8 = Userfloat8;
            
            item.Userfloat9 = Userfloat9;
            
            item.Userfloat10 = Userfloat10;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(string PurchaseOrderDetRefNo,string ItemNo,string PurchaseOrderHdrRefNo,DateTime? ExpiryDate,decimal? Quantity,decimal? RemainingQty,decimal FactoryPrice,double Gst,decimal CostOfGoods,string Remark,decimal? Discount,string CreatedBy,DateTime? CreatedOn,string ModifiedBy,DateTime? ModifiedOn,Guid UniqueID,string Userfld1,string Userfld2,string Userfld3,string Userfld4,string Userfld5,string Userfld6,string Userfld7,string Userfld8,string Userfld9,string Userfld10,bool? Userflag1,bool? Userflag2,bool? Userflag3,bool? Userflag4,bool? Userflag5,decimal? Userfloat1,decimal? Userfloat2,decimal? Userfloat3,decimal? Userfloat4,decimal? Userfloat5,int? Userint1,int? Userint2,int? Userint3,int? Userint4,int? Userint5,int BalanceBefore,int BalanceAfter,bool? Deleted,string Stockinrefno,bool? Isdiscrepancy,decimal? Userfloat6,decimal? Userfloat7,decimal? Userfloat8,decimal? Userfloat9,decimal? Userfloat10)
	    {
		    PurchaseOrderDet item = new PurchaseOrderDet();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.PurchaseOrderDetRefNo = PurchaseOrderDetRefNo;
				
			item.ItemNo = ItemNo;
				
			item.PurchaseOrderHdrRefNo = PurchaseOrderHdrRefNo;
				
			item.ExpiryDate = ExpiryDate;
				
			item.Quantity = Quantity;
				
			item.RemainingQty = RemainingQty;
				
			item.FactoryPrice = FactoryPrice;
				
			item.Gst = Gst;
				
			item.CostOfGoods = CostOfGoods;
				
			item.Remark = Remark;
				
			item.Discount = Discount;
				
			item.CreatedBy = CreatedBy;
				
			item.CreatedOn = CreatedOn;
				
			item.ModifiedBy = ModifiedBy;
				
			item.ModifiedOn = ModifiedOn;
				
			item.UniqueID = UniqueID;
				
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
				
			item.BalanceBefore = BalanceBefore;
				
			item.BalanceAfter = BalanceAfter;
				
			item.Deleted = Deleted;
				
			item.Stockinrefno = Stockinrefno;
				
			item.Isdiscrepancy = Isdiscrepancy;
				
			item.Userfloat6 = Userfloat6;
				
			item.Userfloat7 = Userfloat7;
				
			item.Userfloat8 = Userfloat8;
				
			item.Userfloat9 = Userfloat9;
				
			item.Userfloat10 = Userfloat10;
				
	        item.Save(UserName);
	    }
    }
}
