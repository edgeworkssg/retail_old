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
    /// Controller class for Recurrence
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class RecurrenceController
    {
        // Preload our schema..
        Recurrence thisSchemaLoad = new Recurrence();
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
        public RecurrenceCollection FetchAll()
        {
            RecurrenceCollection coll = new RecurrenceCollection();
            Query qry = new Query(Recurrence.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public RecurrenceCollection FetchByID(object RecID)
        {
            RecurrenceCollection coll = new RecurrenceCollection().Where("RecID", RecID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public RecurrenceCollection FetchByQuery(Query qry)
        {
            RecurrenceCollection coll = new RecurrenceCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object RecID)
        {
            return (Recurrence.Delete(RecID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object RecID)
        {
            return (Recurrence.Destroy(RecID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(int? ItemID,int? Pattern,int? SubPattern,int? EndType,DateTime? StartDate,int? EndAfter,int? Frequency,DateTime? EndDate,int? WeekDays,int? DayofMonth,int? WeekNum,string Comment)
	    {
		    Recurrence item = new Recurrence();
		    
            item.ItemID = ItemID;
            
            item.Pattern = Pattern;
            
            item.SubPattern = SubPattern;
            
            item.EndType = EndType;
            
            item.StartDate = StartDate;
            
            item.EndAfter = EndAfter;
            
            item.Frequency = Frequency;
            
            item.EndDate = EndDate;
            
            item.WeekDays = WeekDays;
            
            item.DayofMonth = DayofMonth;
            
            item.WeekNum = WeekNum;
            
            item.Comment = Comment;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int RecID,int? ItemID,int? Pattern,int? SubPattern,int? EndType,DateTime? StartDate,int? EndAfter,int? Frequency,DateTime? EndDate,int? WeekDays,int? DayofMonth,int? WeekNum,string Comment)
	    {
		    Recurrence item = new Recurrence();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.RecID = RecID;
				
			item.ItemID = ItemID;
				
			item.Pattern = Pattern;
				
			item.SubPattern = SubPattern;
				
			item.EndType = EndType;
				
			item.StartDate = StartDate;
				
			item.EndAfter = EndAfter;
				
			item.Frequency = Frequency;
				
			item.EndDate = EndDate;
				
			item.WeekDays = WeekDays;
				
			item.DayofMonth = DayofMonth;
				
			item.WeekNum = WeekNum;
				
			item.Comment = Comment;
				
	        item.Save(UserName);
	    }
    }
}
