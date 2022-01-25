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
    /// Controller class for UOMConversionDet
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class UOMConversionDetController
    {
        // Preload our schema..
        UOMConversionDet thisSchemaLoad = new UOMConversionDet();
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
        public UOMConversionDetCollection FetchAll()
        {
            UOMConversionDetCollection coll = new UOMConversionDetCollection();
            Query qry = new Query(UOMConversionDet.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public UOMConversionDetCollection FetchByID(object ConversionDetID)
        {
            UOMConversionDetCollection coll = new UOMConversionDetCollection().Where("ConversionDetID", ConversionDetID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public UOMConversionDetCollection FetchByQuery(Query qry)
        {
            UOMConversionDetCollection coll = new UOMConversionDetCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object ConversionDetID)
        {
            return (UOMConversionDet.Delete(ConversionDetID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object ConversionDetID)
        {
            return (UOMConversionDet.Destroy(ConversionDetID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(int ConversionHdrID,string FromUOM,string ToUOM,decimal ConversionRate,string CreatedBy,DateTime? CreatedOn,string ModifiedBy,DateTime? ModifiedOn,bool Deleted,string Remark)
	    {
		    UOMConversionDet item = new UOMConversionDet();
		    
            item.ConversionHdrID = ConversionHdrID;
            
            item.FromUOM = FromUOM;
            
            item.ToUOM = ToUOM;
            
            item.ConversionRate = ConversionRate;
            
            item.CreatedBy = CreatedBy;
            
            item.CreatedOn = CreatedOn;
            
            item.ModifiedBy = ModifiedBy;
            
            item.ModifiedOn = ModifiedOn;
            
            item.Deleted = Deleted;
            
            item.Remark = Remark;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int ConversionDetID,int ConversionHdrID,string FromUOM,string ToUOM,decimal ConversionRate,string CreatedBy,DateTime? CreatedOn,string ModifiedBy,DateTime? ModifiedOn,bool Deleted,string Remark)
	    {
		    UOMConversionDet item = new UOMConversionDet();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.ConversionDetID = ConversionDetID;
				
			item.ConversionHdrID = ConversionHdrID;
				
			item.FromUOM = FromUOM;
				
			item.ToUOM = ToUOM;
				
			item.ConversionRate = ConversionRate;
				
			item.CreatedBy = CreatedBy;
				
			item.CreatedOn = CreatedOn;
				
			item.ModifiedBy = ModifiedBy;
				
			item.ModifiedOn = ModifiedOn;
				
			item.Deleted = Deleted;
				
			item.Remark = Remark;
				
	        item.Save(UserName);
	    }
    }
}
