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
    /// Controller class for Course
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class CourseController
    {
        // Preload our schema..
        Course thisSchemaLoad = new Course();
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
        public CourseCollection FetchAll()
        {
            CourseCollection coll = new CourseCollection();
            Query qry = new Query(Course.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public CourseCollection FetchByID(object Id)
        {
            CourseCollection coll = new CourseCollection().Where("ID", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public CourseCollection FetchByQuery(Query qry)
        {
            CourseCollection coll = new CourseCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (Course.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (Course.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string CourseName,string CourseType,string Description,string Members,DateTime? StartDate,DateTime? EndDate,bool? IsAllDay,int? Place)
	    {
		    Course item = new Course();
		    
            item.CourseName = CourseName;
            
            item.CourseType = CourseType;
            
            item.Description = Description;
            
            item.Members = Members;
            
            item.StartDate = StartDate;
            
            item.EndDate = EndDate;
            
            item.IsAllDay = IsAllDay;
            
            item.Place = Place;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,string CourseName,string CourseType,string Description,string Members,DateTime? StartDate,DateTime? EndDate,bool? IsAllDay,int? Place)
	    {
		    Course item = new Course();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.CourseName = CourseName;
				
			item.CourseType = CourseType;
				
			item.Description = Description;
				
			item.Members = Members;
				
			item.StartDate = StartDate;
				
			item.EndDate = EndDate;
				
			item.IsAllDay = IsAllDay;
				
			item.Place = Place;
				
	        item.Save(UserName);
	    }
    }
}
