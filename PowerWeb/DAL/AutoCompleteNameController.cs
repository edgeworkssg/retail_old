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
    /// Controller class for AutoCompleteNames
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class AutoCompleteNameController
    {
        // Preload our schema..
        AutoCompleteName thisSchemaLoad = new AutoCompleteName();
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
        public AutoCompleteNameCollection FetchAll()
        {
            AutoCompleteNameCollection coll = new AutoCompleteNameCollection();
            Query qry = new Query(AutoCompleteName.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public AutoCompleteNameCollection FetchByID(object Names)
        {
            AutoCompleteNameCollection coll = new AutoCompleteNameCollection().Where("Names", Names).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public AutoCompleteNameCollection FetchByQuery(Query qry)
        {
            AutoCompleteNameCollection coll = new AutoCompleteNameCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Names)
        {
            return (AutoCompleteName.Delete(Names) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Names)
        {
            return (AutoCompleteName.Destroy(Names) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string Names)
	    {
		    AutoCompleteName item = new AutoCompleteName();
		    
            item.Names = Names;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(string Names)
	    {
		    AutoCompleteName item = new AutoCompleteName();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Names = Names;
				
	        item.Save(UserName);
	    }
    }
}
