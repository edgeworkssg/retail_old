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
    /// Controller class for GuestBook
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class GuestBookController
    {
        // Preload our schema..
        GuestBook thisSchemaLoad = new GuestBook();
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
        public GuestBookCollection FetchAll()
        {
            GuestBookCollection coll = new GuestBookCollection();
            Query qry = new Query(GuestBook.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public GuestBookCollection FetchByID(object GuestBookID)
        {
            GuestBookCollection coll = new GuestBookCollection().Where("GuestBookID", GuestBookID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public GuestBookCollection FetchByQuery(Query qry)
        {
            GuestBookCollection coll = new GuestBookCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object GuestBookID)
        {
            return (GuestBook.Delete(GuestBookID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object GuestBookID)
        {
            return (GuestBook.Destroy(GuestBookID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string MembershipNo,DateTime? InTime,DateTime? OutTime,string CreatedBy,DateTime? CreatedOn,string ModifiedBy,DateTime? ModifiedOn,bool? Deleted,string OutletName,string Userfld1,string Userfld2,string Userfld3,string Userfld4,string Userfld5,string Userfld6,string Userfld7,string Userfld8,string Userfld9,string Userfld10)
	    {
		    GuestBook item = new GuestBook();
		    
            item.MembershipNo = MembershipNo;
            
            item.InTime = InTime;
            
            item.OutTime = OutTime;
            
            item.CreatedBy = CreatedBy;
            
            item.CreatedOn = CreatedOn;
            
            item.ModifiedBy = ModifiedBy;
            
            item.ModifiedOn = ModifiedOn;
            
            item.Deleted = Deleted;
            
            item.OutletName = OutletName;
            
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
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int GuestBookID,string MembershipNo,DateTime? InTime,DateTime? OutTime,string CreatedBy,DateTime? CreatedOn,string ModifiedBy,DateTime? ModifiedOn,bool? Deleted,string OutletName,string Userfld1,string Userfld2,string Userfld3,string Userfld4,string Userfld5,string Userfld6,string Userfld7,string Userfld8,string Userfld9,string Userfld10)
	    {
		    GuestBook item = new GuestBook();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.GuestBookID = GuestBookID;
				
			item.MembershipNo = MembershipNo;
				
			item.InTime = InTime;
				
			item.OutTime = OutTime;
				
			item.CreatedBy = CreatedBy;
				
			item.CreatedOn = CreatedOn;
				
			item.ModifiedBy = ModifiedBy;
				
			item.ModifiedOn = ModifiedOn;
				
			item.Deleted = Deleted;
				
			item.OutletName = OutletName;
				
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
				
	        item.Save(UserName);
	    }
    }
}
