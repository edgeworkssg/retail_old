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
    /// Controller class for QuickAccessButton
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class QuickAccessButtonController
    {
        // Preload our schema..
        QuickAccessButton thisSchemaLoad = new QuickAccessButton();
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
        public QuickAccessButtonCollection FetchAll()
        {
            QuickAccessButtonCollection coll = new QuickAccessButtonCollection();
            Query qry = new Query(QuickAccessButton.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public QuickAccessButtonCollection FetchByID(object QuickAccessButtonID)
        {
            QuickAccessButtonCollection coll = new QuickAccessButtonCollection().Where("QuickAccessButtonID", QuickAccessButtonID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public QuickAccessButtonCollection FetchByQuery(Query qry)
        {
            QuickAccessButtonCollection coll = new QuickAccessButtonCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object QuickAccessButtonID)
        {
            return (QuickAccessButton.Delete(QuickAccessButtonID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object QuickAccessButtonID)
        {
            return (QuickAccessButton.Destroy(QuickAccessButtonID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(Guid QuickAccessButtonID,string ItemNo,Guid QuickAccessCategoryID,string ForeColor,string BackColor,string Label,int Row,int Col,int PriorityLevel,int? PointOfSaleID,bool ShowLabel,bool? Deleted,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy)
	    {
		    QuickAccessButton item = new QuickAccessButton();
		    
            item.QuickAccessButtonID = QuickAccessButtonID;
            
            item.ItemNo = ItemNo;
            
            item.QuickAccessCategoryID = QuickAccessCategoryID;
            
            item.ForeColor = ForeColor;
            
            item.BackColor = BackColor;
            
            item.Label = Label;
            
            item.Row = Row;
            
            item.Col = Col;
            
            item.PriorityLevel = PriorityLevel;
            
            item.PointOfSaleID = PointOfSaleID;
            
            item.ShowLabel = ShowLabel;
            
            item.Deleted = Deleted;
            
            item.CreatedOn = CreatedOn;
            
            item.CreatedBy = CreatedBy;
            
            item.ModifiedOn = ModifiedOn;
            
            item.ModifiedBy = ModifiedBy;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(Guid QuickAccessButtonID,string ItemNo,Guid QuickAccessCategoryID,string ForeColor,string BackColor,string Label,int Row,int Col,int PriorityLevel,int? PointOfSaleID,bool ShowLabel,bool? Deleted,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy)
	    {
		    QuickAccessButton item = new QuickAccessButton();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.QuickAccessButtonID = QuickAccessButtonID;
				
			item.ItemNo = ItemNo;
				
			item.QuickAccessCategoryID = QuickAccessCategoryID;
				
			item.ForeColor = ForeColor;
				
			item.BackColor = BackColor;
				
			item.Label = Label;
				
			item.Row = Row;
				
			item.Col = Col;
				
			item.PriorityLevel = PriorityLevel;
				
			item.PointOfSaleID = PointOfSaleID;
				
			item.ShowLabel = ShowLabel;
				
			item.Deleted = Deleted;
				
			item.CreatedOn = CreatedOn;
				
			item.CreatedBy = CreatedBy;
				
			item.ModifiedOn = ModifiedOn;
				
			item.ModifiedBy = ModifiedBy;
				
	        item.Save(UserName);
	    }
    }
}
