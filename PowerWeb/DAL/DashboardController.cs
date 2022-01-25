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
    /// Controller class for Dashboard
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class DashboardController
    {
        // Preload our schema..
        Dashboard thisSchemaLoad = new Dashboard();
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
        public DashboardCollection FetchAll()
        {
            DashboardCollection coll = new DashboardCollection();
            Query qry = new Query(Dashboard.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public DashboardCollection FetchByID(object Id)
        {
            DashboardCollection coll = new DashboardCollection().Where("ID", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public DashboardCollection FetchByQuery(Query qry)
        {
            DashboardCollection coll = new DashboardCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (Dashboard.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (Dashboard.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string Title,string SubTitle,string Description,string PlotType,string PlotOption,string Width,string Height,string SQLString,bool? IsInline,bool? BreakAfter,bool? BreakBefore,string ColumnStyle,bool? IsEnable,int? DisplayOrder,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,Guid? UniqueID,bool? Deleted)
	    {
		    Dashboard item = new Dashboard();
		    
            item.Title = Title;
            
            item.SubTitle = SubTitle;
            
            item.Description = Description;
            
            item.PlotType = PlotType;
            
            item.PlotOption = PlotOption;
            
            item.Width = Width;
            
            item.Height = Height;
            
            item.SQLString = SQLString;
            
            item.IsInline = IsInline;
            
            item.BreakAfter = BreakAfter;
            
            item.BreakBefore = BreakBefore;
            
            item.ColumnStyle = ColumnStyle;
            
            item.IsEnable = IsEnable;
            
            item.DisplayOrder = DisplayOrder;
            
            item.CreatedOn = CreatedOn;
            
            item.CreatedBy = CreatedBy;
            
            item.ModifiedOn = ModifiedOn;
            
            item.ModifiedBy = ModifiedBy;
            
            item.UniqueID = UniqueID;
            
            item.Deleted = Deleted;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,string Title,string SubTitle,string Description,string PlotType,string PlotOption,string Width,string Height,string SQLString,bool? IsInline,bool? BreakAfter,bool? BreakBefore,string ColumnStyle,bool? IsEnable,int? DisplayOrder,DateTime? CreatedOn,string CreatedBy,DateTime? ModifiedOn,string ModifiedBy,Guid? UniqueID,bool? Deleted)
	    {
		    Dashboard item = new Dashboard();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.Title = Title;
				
			item.SubTitle = SubTitle;
				
			item.Description = Description;
				
			item.PlotType = PlotType;
				
			item.PlotOption = PlotOption;
				
			item.Width = Width;
				
			item.Height = Height;
				
			item.SQLString = SQLString;
				
			item.IsInline = IsInline;
				
			item.BreakAfter = BreakAfter;
				
			item.BreakBefore = BreakBefore;
				
			item.ColumnStyle = ColumnStyle;
				
			item.IsEnable = IsEnable;
				
			item.DisplayOrder = DisplayOrder;
				
			item.CreatedOn = CreatedOn;
				
			item.CreatedBy = CreatedBy;
				
			item.ModifiedOn = ModifiedOn;
				
			item.ModifiedBy = ModifiedBy;
				
			item.UniqueID = UniqueID;
				
			item.Deleted = Deleted;
				
	        item.Save(UserName);
	    }
    }
}
