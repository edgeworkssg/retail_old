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
    /// Controller class for RecyclerNotesBalance
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class RecyclerNotesBalanceController
    {
        // Preload our schema..
        RecyclerNotesBalance thisSchemaLoad = new RecyclerNotesBalance();
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
        public RecyclerNotesBalanceCollection FetchAll()
        {
            RecyclerNotesBalanceCollection coll = new RecyclerNotesBalanceCollection();
            Query qry = new Query(RecyclerNotesBalance.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public RecyclerNotesBalanceCollection FetchByID(object DeviceID)
        {
            RecyclerNotesBalanceCollection coll = new RecyclerNotesBalanceCollection().Where("DeviceID", DeviceID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public RecyclerNotesBalanceCollection FetchByQuery(Query qry)
        {
            RecyclerNotesBalanceCollection coll = new RecyclerNotesBalanceCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object DeviceID)
        {
            return (RecyclerNotesBalance.Delete(DeviceID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object DeviceID)
        {
            return (RecyclerNotesBalance.Destroy(DeviceID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string DeviceID,int? R1Denomination,int? R1Quantity,int? R2Denomination,int? R2Quantity,int? R3Denomination,int? R3Quantity,int? R4Denomination,int? R4Quantity,int? R5Denomination,int? R5Quantity,int? C1Denomination,int? C1Quantity,int? C2Denomination,int? C2Quantity,int? C3Denomination,int? C3Quantity,int? C4Denomination,int? C4Quantity,int? C5Denomination,int? C5Quantity,int? C6Denomination,int? C6Quantity,int? C7Denomination,int? C7Quantity,int? C8Denomination,int? C8Quantity,int? C9Denomination,int? C9Quantity,int? C10Denomination,int? C10Quantity,string LastMachineEventID)
	    {
		    RecyclerNotesBalance item = new RecyclerNotesBalance();
		    
            item.DeviceID = DeviceID;
            
            item.R1Denomination = R1Denomination;
            
            item.R1Quantity = R1Quantity;
            
            item.R2Denomination = R2Denomination;
            
            item.R2Quantity = R2Quantity;
            
            item.R3Denomination = R3Denomination;
            
            item.R3Quantity = R3Quantity;
            
            item.R4Denomination = R4Denomination;
            
            item.R4Quantity = R4Quantity;
            
            item.R5Denomination = R5Denomination;
            
            item.R5Quantity = R5Quantity;
            
            item.C1Denomination = C1Denomination;
            
            item.C1Quantity = C1Quantity;
            
            item.C2Denomination = C2Denomination;
            
            item.C2Quantity = C2Quantity;
            
            item.C3Denomination = C3Denomination;
            
            item.C3Quantity = C3Quantity;
            
            item.C4Denomination = C4Denomination;
            
            item.C4Quantity = C4Quantity;
            
            item.C5Denomination = C5Denomination;
            
            item.C5Quantity = C5Quantity;
            
            item.C6Denomination = C6Denomination;
            
            item.C6Quantity = C6Quantity;
            
            item.C7Denomination = C7Denomination;
            
            item.C7Quantity = C7Quantity;
            
            item.C8Denomination = C8Denomination;
            
            item.C8Quantity = C8Quantity;
            
            item.C9Denomination = C9Denomination;
            
            item.C9Quantity = C9Quantity;
            
            item.C10Denomination = C10Denomination;
            
            item.C10Quantity = C10Quantity;
            
            item.LastMachineEventID = LastMachineEventID;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(string DeviceID,int? R1Denomination,int? R1Quantity,int? R2Denomination,int? R2Quantity,int? R3Denomination,int? R3Quantity,int? R4Denomination,int? R4Quantity,int? R5Denomination,int? R5Quantity,int? C1Denomination,int? C1Quantity,int? C2Denomination,int? C2Quantity,int? C3Denomination,int? C3Quantity,int? C4Denomination,int? C4Quantity,int? C5Denomination,int? C5Quantity,int? C6Denomination,int? C6Quantity,int? C7Denomination,int? C7Quantity,int? C8Denomination,int? C8Quantity,int? C9Denomination,int? C9Quantity,int? C10Denomination,int? C10Quantity,string LastMachineEventID)
	    {
		    RecyclerNotesBalance item = new RecyclerNotesBalance();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.DeviceID = DeviceID;
				
			item.R1Denomination = R1Denomination;
				
			item.R1Quantity = R1Quantity;
				
			item.R2Denomination = R2Denomination;
				
			item.R2Quantity = R2Quantity;
				
			item.R3Denomination = R3Denomination;
				
			item.R3Quantity = R3Quantity;
				
			item.R4Denomination = R4Denomination;
				
			item.R4Quantity = R4Quantity;
				
			item.R5Denomination = R5Denomination;
				
			item.R5Quantity = R5Quantity;
				
			item.C1Denomination = C1Denomination;
				
			item.C1Quantity = C1Quantity;
				
			item.C2Denomination = C2Denomination;
				
			item.C2Quantity = C2Quantity;
				
			item.C3Denomination = C3Denomination;
				
			item.C3Quantity = C3Quantity;
				
			item.C4Denomination = C4Denomination;
				
			item.C4Quantity = C4Quantity;
				
			item.C5Denomination = C5Denomination;
				
			item.C5Quantity = C5Quantity;
				
			item.C6Denomination = C6Denomination;
				
			item.C6Quantity = C6Quantity;
				
			item.C7Denomination = C7Denomination;
				
			item.C7Quantity = C7Quantity;
				
			item.C8Denomination = C8Denomination;
				
			item.C8Quantity = C8Quantity;
				
			item.C9Denomination = C9Denomination;
				
			item.C9Quantity = C9Quantity;
				
			item.C10Denomination = C10Denomination;
				
			item.C10Quantity = C10Quantity;
				
			item.LastMachineEventID = LastMachineEventID;
				
	        item.Save(UserName);
	    }
    }
}
