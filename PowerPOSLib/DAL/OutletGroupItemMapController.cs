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
    /// Controller class for OutletGroupItemMap
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class OutletGroupItemMapController
    {
        // Preload our schema..
        OutletGroupItemMap thisSchemaLoad = new OutletGroupItemMap();
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
        public OutletGroupItemMapCollection FetchAll()
        {
            OutletGroupItemMapCollection coll = new OutletGroupItemMapCollection();
            Query qry = new Query(OutletGroupItemMap.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public OutletGroupItemMapCollection FetchByID(object OutletGroupItemMapID)
        {
            OutletGroupItemMapCollection coll = new OutletGroupItemMapCollection().Where("OutletGroupItemMapID", OutletGroupItemMapID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public OutletGroupItemMapCollection FetchByQuery(Query qry)
        {
            OutletGroupItemMapCollection coll = new OutletGroupItemMapCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object OutletGroupItemMapID)
        {
            return (OutletGroupItemMap.Delete(OutletGroupItemMapID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object OutletGroupItemMapID)
        {
            return (OutletGroupItemMap.Destroy(OutletGroupItemMapID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(int? OutletGroupID,string OutletName,string ItemNo,decimal RetailPrice,decimal CostPrice,bool Deleted,DateTime? ModifiedOn,string ModifiedBy,DateTime? CreatedOn,string CreatedBy,bool? IsItemDeleted,decimal? P1,decimal? P2,decimal? P3,decimal? P4,decimal? P5,string Userfld1,string Userfld2,string Userfld3,string Userfld4,string Userfld5,string Userfld6,string Userfld7,string Userfld8,string Userfld9,string Userfld10,decimal? Userfloat1,decimal? Userfloat2,decimal? Userfloat3,decimal? Userfloat4,decimal? Userfloat5,bool? Userflag1,bool? Userflag2,bool? Userflag3,bool? Userflag4,bool? Userflag5,int? Userint1,int? Userint2,int? Userint3,int? Userint4,int? Userint5)
	    {
		    OutletGroupItemMap item = new OutletGroupItemMap();
		    
            item.OutletGroupID = OutletGroupID;
            
            item.OutletName = OutletName;
            
            item.ItemNo = ItemNo;
            
            item.RetailPrice = RetailPrice;
            
            item.CostPrice = CostPrice;
            
            item.Deleted = Deleted;
            
            item.ModifiedOn = ModifiedOn;
            
            item.ModifiedBy = ModifiedBy;
            
            item.CreatedOn = CreatedOn;
            
            item.CreatedBy = CreatedBy;
            
            item.IsItemDeleted = IsItemDeleted;
            
            item.P1 = P1;
            
            item.P2 = P2;
            
            item.P3 = P3;
            
            item.P4 = P4;
            
            item.P5 = P5;
            
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
            
            item.Userfloat1 = Userfloat1;
            
            item.Userfloat2 = Userfloat2;
            
            item.Userfloat3 = Userfloat3;
            
            item.Userfloat4 = Userfloat4;
            
            item.Userfloat5 = Userfloat5;
            
            item.Userflag1 = Userflag1;
            
            item.Userflag2 = Userflag2;
            
            item.Userflag3 = Userflag3;
            
            item.Userflag4 = Userflag4;
            
            item.Userflag5 = Userflag5;
            
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
	    public void Update(int OutletGroupItemMapID,int? OutletGroupID,string OutletName,string ItemNo,decimal RetailPrice,decimal CostPrice,bool Deleted,DateTime? ModifiedOn,string ModifiedBy,DateTime? CreatedOn,string CreatedBy,bool? IsItemDeleted,decimal? P1,decimal? P2,decimal? P3,decimal? P4,decimal? P5,string Userfld1,string Userfld2,string Userfld3,string Userfld4,string Userfld5,string Userfld6,string Userfld7,string Userfld8,string Userfld9,string Userfld10,decimal? Userfloat1,decimal? Userfloat2,decimal? Userfloat3,decimal? Userfloat4,decimal? Userfloat5,bool? Userflag1,bool? Userflag2,bool? Userflag3,bool? Userflag4,bool? Userflag5,int? Userint1,int? Userint2,int? Userint3,int? Userint4,int? Userint5)
	    {
		    OutletGroupItemMap item = new OutletGroupItemMap();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.OutletGroupItemMapID = OutletGroupItemMapID;
				
			item.OutletGroupID = OutletGroupID;
				
			item.OutletName = OutletName;
				
			item.ItemNo = ItemNo;
				
			item.RetailPrice = RetailPrice;
				
			item.CostPrice = CostPrice;
				
			item.Deleted = Deleted;
				
			item.ModifiedOn = ModifiedOn;
				
			item.ModifiedBy = ModifiedBy;
				
			item.CreatedOn = CreatedOn;
				
			item.CreatedBy = CreatedBy;
				
			item.IsItemDeleted = IsItemDeleted;
				
			item.P1 = P1;
				
			item.P2 = P2;
				
			item.P3 = P3;
				
			item.P4 = P4;
				
			item.P5 = P5;
				
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
				
			item.Userfloat1 = Userfloat1;
				
			item.Userfloat2 = Userfloat2;
				
			item.Userfloat3 = Userfloat3;
				
			item.Userfloat4 = Userfloat4;
				
			item.Userfloat5 = Userfloat5;
				
			item.Userflag1 = Userflag1;
				
			item.Userflag2 = Userflag2;
				
			item.Userflag3 = Userflag3;
				
			item.Userflag4 = Userflag4;
				
			item.Userflag5 = Userflag5;
				
			item.Userint1 = Userint1;
				
			item.Userint2 = Userint2;
				
			item.Userint3 = Userint3;
				
			item.Userint4 = Userint4;
				
			item.Userint5 = Userint5;
				
	        item.Save(UserName);
	    }
    }
}
