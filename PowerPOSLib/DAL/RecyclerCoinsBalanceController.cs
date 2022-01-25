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
    /// Controller class for RecyclerCoinsBalance
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class RecyclerCoinsBalanceController
    {
        // Preload our schema..
        RecyclerCoinsBalance thisSchemaLoad = new RecyclerCoinsBalance();
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
        public RecyclerCoinsBalanceCollection FetchAll()
        {
            RecyclerCoinsBalanceCollection coll = new RecyclerCoinsBalanceCollection();
            Query qry = new Query(RecyclerCoinsBalance.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public RecyclerCoinsBalanceCollection FetchByID(object DeviceID)
        {
            RecyclerCoinsBalanceCollection coll = new RecyclerCoinsBalanceCollection().Where("DeviceID", DeviceID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public RecyclerCoinsBalanceCollection FetchByQuery(Query qry)
        {
            RecyclerCoinsBalanceCollection coll = new RecyclerCoinsBalanceCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object DeviceID)
        {
            return (RecyclerCoinsBalance.Delete(DeviceID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object DeviceID)
        {
            return (RecyclerCoinsBalance.Destroy(DeviceID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string DeviceID,int? R1Denomination,int? R1Quantity,int? R2Denomination,int? R2Quantity,int? R3Denomination,int? R3Quantity,int? R4Denomination,int? R4Quantity,int? R5Denomination,int? R5Quantity,int? R6Denomination,int? R6Quantity,int? R7Denomination,int? R7Quantity,int? R8Denomination,int? R8Quantity,int? R9Denomination,int? R9Quantity,int? R10Denomination,int? R10Quantity,string LastMachineEventID)
	    {
		    RecyclerCoinsBalance item = new RecyclerCoinsBalance();
		    
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
            
            item.R6Denomination = R6Denomination;
            
            item.R6Quantity = R6Quantity;
            
            item.R7Denomination = R7Denomination;
            
            item.R7Quantity = R7Quantity;
            
            item.R8Denomination = R8Denomination;
            
            item.R8Quantity = R8Quantity;
            
            item.R9Denomination = R9Denomination;
            
            item.R9Quantity = R9Quantity;
            
            item.R10Denomination = R10Denomination;
            
            item.R10Quantity = R10Quantity;
            
            item.LastMachineEventID = LastMachineEventID;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(string DeviceID,int? R1Denomination,int? R1Quantity,int? R2Denomination,int? R2Quantity,int? R3Denomination,int? R3Quantity,int? R4Denomination,int? R4Quantity,int? R5Denomination,int? R5Quantity,int? R6Denomination,int? R6Quantity,int? R7Denomination,int? R7Quantity,int? R8Denomination,int? R8Quantity,int? R9Denomination,int? R9Quantity,int? R10Denomination,int? R10Quantity,string LastMachineEventID)
	    {
		    RecyclerCoinsBalance item = new RecyclerCoinsBalance();
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
				
			item.R6Denomination = R6Denomination;
				
			item.R6Quantity = R6Quantity;
				
			item.R7Denomination = R7Denomination;
				
			item.R7Quantity = R7Quantity;
				
			item.R8Denomination = R8Denomination;
				
			item.R8Quantity = R8Quantity;
				
			item.R9Denomination = R9Denomination;
				
			item.R9Quantity = R9Quantity;
				
			item.R10Denomination = R10Denomination;
				
			item.R10Quantity = R10Quantity;
				
			item.LastMachineEventID = LastMachineEventID;
				
	        item.Save(UserName);
	    }
    }
}
