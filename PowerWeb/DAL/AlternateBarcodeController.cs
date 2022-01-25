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
    /// Controller class for AlternateBarcode
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class AlternateBarcodeController
    {
        // Preload our schema..
        AlternateBarcode thisSchemaLoad = new AlternateBarcode();
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
        public AlternateBarcodeCollection FetchAll()
        {
            AlternateBarcodeCollection coll = new AlternateBarcodeCollection();
            Query qry = new Query(AlternateBarcode.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public AlternateBarcodeCollection FetchByID(object BarcodeID)
        {
            AlternateBarcodeCollection coll = new AlternateBarcodeCollection().Where("BarcodeID", BarcodeID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public AlternateBarcodeCollection FetchByQuery(Query qry)
        {
            AlternateBarcodeCollection coll = new AlternateBarcodeCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object BarcodeID)
        {
            return (AlternateBarcode.Delete(BarcodeID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object BarcodeID)
        {
            return (AlternateBarcode.Destroy(BarcodeID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string Barcode,string ItemNo,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,bool? Deleted,DateTime? LastEditDate,DateTime? CreationDate)
	    {
		    AlternateBarcode item = new AlternateBarcode();
		    
            item.Barcode = Barcode;
            
            item.ItemNo = ItemNo;
            
            item.CreatedOn = CreatedOn;
            
            item.CreatedBy = CreatedBy;
            
            item.ModifiedOn = ModifiedOn;
            
            item.ModifiedBy = ModifiedBy;
            
            item.Deleted = Deleted;
            
            item.LastEditDate = LastEditDate;
            
            item.CreationDate = CreationDate;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int BarcodeID,string Barcode,string ItemNo,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,bool? Deleted,DateTime? LastEditDate,DateTime? CreationDate)
	    {
		    AlternateBarcode item = new AlternateBarcode();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.BarcodeID = BarcodeID;
				
			item.Barcode = Barcode;
				
			item.ItemNo = ItemNo;
				
			item.CreatedOn = CreatedOn;
				
			item.CreatedBy = CreatedBy;
				
			item.ModifiedOn = ModifiedOn;
				
			item.ModifiedBy = ModifiedBy;
				
			item.Deleted = Deleted;
				
			item.LastEditDate = LastEditDate;
				
			item.CreationDate = CreationDate;
				
	        item.Save(UserName);
	    }
    }
}
