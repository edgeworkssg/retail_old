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
    /// Controller class for LineInfo
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class LineInfoController
    {
        // Preload our schema..
        LineInfo thisSchemaLoad = new LineInfo();
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
        public LineInfoCollection FetchAll()
        {
            LineInfoCollection coll = new LineInfoCollection();
            Query qry = new Query(LineInfo.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public LineInfoCollection FetchByID(object LineInfoID)
        {
            LineInfoCollection coll = new LineInfoCollection().Where("LineInfoID", LineInfoID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public LineInfoCollection FetchByQuery(Query qry)
        {
            LineInfoCollection coll = new LineInfoCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object LineInfoID)
        {
            return (LineInfo.Delete(LineInfoID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object LineInfoID)
        {
            return (LineInfo.Destroy(LineInfoID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string LineInfoName,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,Guid? UniqueID,string Userfld1,string Userfld2,string Userfld3,bool? Deleted)
	    {
		    LineInfo item = new LineInfo();
		    
            item.LineInfoName = LineInfoName;
            
            item.CreatedOn = CreatedOn;
            
            item.CreatedBy = CreatedBy;
            
            item.ModifiedOn = ModifiedOn;
            
            item.ModifiedBy = ModifiedBy;
            
            item.UniqueID = UniqueID;
            
            item.Userfld1 = Userfld1;
            
            item.Userfld2 = Userfld2;
            
            item.Userfld3 = Userfld3;
            
            item.Deleted = Deleted;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int LineInfoID,string LineInfoName,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,Guid? UniqueID,string Userfld1,string Userfld2,string Userfld3,bool? Deleted)
	    {
		    LineInfo item = new LineInfo();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.LineInfoID = LineInfoID;
				
			item.LineInfoName = LineInfoName;
				
			item.CreatedOn = CreatedOn;
				
			item.CreatedBy = CreatedBy;
				
			item.ModifiedOn = ModifiedOn;
				
			item.ModifiedBy = ModifiedBy;
				
			item.UniqueID = UniqueID;
				
			item.Userfld1 = Userfld1;
				
			item.Userfld2 = Userfld2;
				
			item.Userfld3 = Userfld3;
				
			item.Deleted = Deleted;
				
	        item.Save(UserName);
	    }
    }
}
