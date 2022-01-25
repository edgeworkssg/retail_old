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
    /// Controller class for UOMConversionHdr
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class UOMConversionHdrController
    {
        // Preload our schema..
        UOMConversionHdr thisSchemaLoad = new UOMConversionHdr();
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
        public UOMConversionHdrCollection FetchAll()
        {
            UOMConversionHdrCollection coll = new UOMConversionHdrCollection();
            Query qry = new Query(UOMConversionHdr.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public UOMConversionHdrCollection FetchByID(object ConversionHdrID)
        {
            UOMConversionHdrCollection coll = new UOMConversionHdrCollection().Where("ConversionHdrID", ConversionHdrID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public UOMConversionHdrCollection FetchByQuery(Query qry)
        {
            UOMConversionHdrCollection coll = new UOMConversionHdrCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object ConversionHdrID)
        {
            return (UOMConversionHdr.Delete(ConversionHdrID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object ConversionHdrID)
        {
            return (UOMConversionHdr.Destroy(ConversionHdrID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string ItemNo,string CreatedBy,DateTime? CreatedOn,string ModifiedBy,DateTime? ModifiedOn,bool Deleted)
	    {
		    UOMConversionHdr item = new UOMConversionHdr();
		    
            item.ItemNo = ItemNo;
            
            item.CreatedBy = CreatedBy;
            
            item.CreatedOn = CreatedOn;
            
            item.ModifiedBy = ModifiedBy;
            
            item.ModifiedOn = ModifiedOn;
            
            item.Deleted = Deleted;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int ConversionHdrID,string ItemNo,string CreatedBy,DateTime? CreatedOn,string ModifiedBy,DateTime? ModifiedOn,bool Deleted)
	    {
		    UOMConversionHdr item = new UOMConversionHdr();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.ConversionHdrID = ConversionHdrID;
				
			item.ItemNo = ItemNo;
				
			item.CreatedBy = CreatedBy;
				
			item.CreatedOn = CreatedOn;
				
			item.ModifiedBy = ModifiedBy;
				
			item.ModifiedOn = ModifiedOn;
				
			item.Deleted = Deleted;
				
	        item.Save(UserName);
	    }
    }
}
