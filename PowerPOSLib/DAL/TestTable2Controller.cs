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
    /// Controller class for TestTable2
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class TestTable2Controller
    {
        // Preload our schema..
        TestTable2 thisSchemaLoad = new TestTable2();
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
        public TestTable2Collection FetchAll()
        {
            TestTable2Collection coll = new TestTable2Collection();
            Query qry = new Query(TestTable2.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public TestTable2Collection FetchByID(object Column4)
        {
            TestTable2Collection coll = new TestTable2Collection().Where("Column4", Column4).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public TestTable2Collection FetchByQuery(Query qry)
        {
            TestTable2Collection coll = new TestTable2Collection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Column4)
        {
            return (TestTable2.Delete(Column4) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Column4)
        {
            return (TestTable2.Destroy(Column4) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string Column4,string Column1)
	    {
		    TestTable2 item = new TestTable2();
		    
            item.Column4 = Column4;
            
            item.Column1 = Column1;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(string Column4,string Column1)
	    {
		    TestTable2 item = new TestTable2();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Column4 = Column4;
				
			item.Column1 = Column1;
				
	        item.Save(UserName);
	    }
    }
}
