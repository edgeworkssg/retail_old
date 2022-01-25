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
    /// Controller class for TestTable
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class TestTableController
    {
        // Preload our schema..
        TestTable thisSchemaLoad = new TestTable();
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
        public TestTableCollection FetchAll()
        {
            TestTableCollection coll = new TestTableCollection();
            Query qry = new Query(TestTable.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public TestTableCollection FetchByID(object Column1)
        {
            TestTableCollection coll = new TestTableCollection().Where("Column1", Column1).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public TestTableCollection FetchByQuery(Query qry)
        {
            TestTableCollection coll = new TestTableCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Column1)
        {
            return (TestTable.Delete(Column1) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Column1)
        {
            return (TestTable.Destroy(Column1) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string Column1,string Column2)
	    {
		    TestTable item = new TestTable();
		    
            item.Column1 = Column1;
            
            item.Column2 = Column2;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(string Column1,string Column2)
	    {
		    TestTable item = new TestTable();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Column1 = Column1;
				
			item.Column2 = Column2;
				
	        item.Save(UserName);
	    }
    }
}
