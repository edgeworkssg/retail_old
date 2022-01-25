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
    /// Controller class for Building
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class BuildingController
    {
        // Preload our schema..
        Building thisSchemaLoad = new Building();
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
        public BuildingCollection FetchAll()
        {
            BuildingCollection coll = new BuildingCollection();
            Query qry = new Query(Building.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public BuildingCollection FetchByID(object BuildingName)
        {
            BuildingCollection coll = new BuildingCollection().Where("Building_Name", BuildingName).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public BuildingCollection FetchByQuery(Query qry)
        {
            BuildingCollection coll = new BuildingCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object BuildingName)
        {
            return (Building.Delete(BuildingName) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object BuildingName)
        {
            return (Building.Destroy(BuildingName) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string BuildingName,string City,string Country,string AddressLine1,string AddressLine2,int? PinCode)
	    {
		    Building item = new Building();
		    
            item.BuildingName = BuildingName;
            
            item.City = City;
            
            item.Country = Country;
            
            item.AddressLine1 = AddressLine1;
            
            item.AddressLine2 = AddressLine2;
            
            item.PinCode = PinCode;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(string BuildingName,string City,string Country,string AddressLine1,string AddressLine2,int? PinCode)
	    {
		    Building item = new Building();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.BuildingName = BuildingName;
				
			item.City = City;
				
			item.Country = Country;
				
			item.AddressLine1 = AddressLine1;
				
			item.AddressLine2 = AddressLine2;
				
			item.PinCode = PinCode;
				
	        item.Save(UserName);
	    }
    }
}
