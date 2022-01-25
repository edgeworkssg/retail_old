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
    /// Controller class for PackageDet
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class PackageDetController
    {
        // Preload our schema..
        PackageDet thisSchemaLoad = new PackageDet();
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
        public PackageDetCollection FetchAll()
        {
            PackageDetCollection coll = new PackageDetCollection();
            Query qry = new Query(PackageDet.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public PackageDetCollection FetchByID(object PackageDetID)
        {
            PackageDetCollection coll = new PackageDetCollection().Where("PackageDetID", PackageDetID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public PackageDetCollection FetchByQuery(Query qry)
        {
            PackageDetCollection coll = new PackageDetCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object PackageDetID)
        {
            return (PackageDet.Delete(PackageDetID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object PackageDetID)
        {
            return (PackageDet.Destroy(PackageDetID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string PackageDetID,string OrderDetID,string PackageHdrID,string Remark,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,bool Deleted)
	    {
		    PackageDet item = new PackageDet();
		    
            item.PackageDetID = PackageDetID;
            
            item.OrderDetID = OrderDetID;
            
            item.PackageHdrID = PackageHdrID;
            
            item.Remark = Remark;
            
            item.CreatedOn = CreatedOn;
            
            item.CreatedBy = CreatedBy;
            
            item.ModifiedOn = ModifiedOn;
            
            item.ModifiedBy = ModifiedBy;
            
            item.Deleted = Deleted;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(string PackageDetID,string OrderDetID,string PackageHdrID,string Remark,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,bool Deleted)
	    {
		    PackageDet item = new PackageDet();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.PackageDetID = PackageDetID;
				
			item.OrderDetID = OrderDetID;
				
			item.PackageHdrID = PackageHdrID;
				
			item.Remark = Remark;
				
			item.CreatedOn = CreatedOn;
				
			item.CreatedBy = CreatedBy;
				
			item.ModifiedOn = ModifiedOn;
				
			item.ModifiedBy = ModifiedBy;
				
			item.Deleted = Deleted;
				
	        item.Save(UserName);
	    }
    }
}
