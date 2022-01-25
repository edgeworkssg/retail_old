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
    /// Controller class for PostalCodeDB
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class PostalCodeDBController
    {
        // Preload our schema..
        PostalCodeDB thisSchemaLoad = new PostalCodeDB();
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
        public PostalCodeDBCollection FetchAll()
        {
            PostalCodeDBCollection coll = new PostalCodeDBCollection();
            Query qry = new Query(PostalCodeDB.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public PostalCodeDBCollection FetchByID(object Zip)
        {
            PostalCodeDBCollection coll = new PostalCodeDBCollection().Where("ZIP", Zip).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public PostalCodeDBCollection FetchByQuery(Query qry)
        {
            PostalCodeDBCollection coll = new PostalCodeDBCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Zip)
        {
            return (PostalCodeDB.Delete(Zip) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Zip)
        {
            return (PostalCodeDB.Destroy(Zip) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string Country,string Language,int? Id,string Region1,string Region2,string Region3,string Region4,string Zip,string City,string Area1,string Area2,decimal? Lat,decimal? Lng)
	    {
		    PostalCodeDB item = new PostalCodeDB();
		    
            item.Country = Country;
            
            item.Language = Language;
            
            item.Id = Id;
            
            item.Region1 = Region1;
            
            item.Region2 = Region2;
            
            item.Region3 = Region3;
            
            item.Region4 = Region4;
            
            item.Zip = Zip;
            
            item.City = City;
            
            item.Area1 = Area1;
            
            item.Area2 = Area2;
            
            item.Lat = Lat;
            
            item.Lng = Lng;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(string Country,string Language,int? Id,string Region1,string Region2,string Region3,string Region4,string Zip,string City,string Area1,string Area2,decimal? Lat,decimal? Lng)
	    {
		    PostalCodeDB item = new PostalCodeDB();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Country = Country;
				
			item.Language = Language;
				
			item.Id = Id;
				
			item.Region1 = Region1;
				
			item.Region2 = Region2;
				
			item.Region3 = Region3;
				
			item.Region4 = Region4;
				
			item.Zip = Zip;
				
			item.City = City;
				
			item.Area1 = Area1;
				
			item.Area2 = Area2;
				
			item.Lat = Lat;
				
			item.Lng = Lng;
				
	        item.Save(UserName);
	    }
    }
}
