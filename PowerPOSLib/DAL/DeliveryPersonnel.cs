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
	/// Strongly-typed collection for the DeliveryPersonnel class.
	/// </summary>
    [Serializable]
	public partial class DeliveryPersonnelCollection : ActiveList<DeliveryPersonnel, DeliveryPersonnelCollection>
	{	   
		public DeliveryPersonnelCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>DeliveryPersonnelCollection</returns>
		public DeliveryPersonnelCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                DeliveryPersonnel o = this[i];
                foreach (SubSonic.Where w in this.wheres)
                {
                    bool remove = false;
                    System.Reflection.PropertyInfo pi = o.GetType().GetProperty(w.ColumnName);
                    if (pi.CanRead)
                    {
                        object val = pi.GetValue(o, null);
                        switch (w.Comparison)
                        {
                            case SubSonic.Comparison.Equals:
                                if (!val.Equals(w.ParameterValue))
                                {
                                    remove = true;
                                }
                                break;
                        }
                    }
                    if (remove)
                    {
                        this.Remove(o);
                        break;
                    }
                }
            }
            return this;
        }
		
		
	}
	/// <summary>
	/// This is an ActiveRecord class which wraps the Delivery_Personnel table.
	/// </summary>
	[Serializable]
	public partial class DeliveryPersonnel : ActiveRecord<DeliveryPersonnel>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public DeliveryPersonnel()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public DeliveryPersonnel(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public DeliveryPersonnel(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public DeliveryPersonnel(string columnName, object columnValue)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByParam(columnName,columnValue);
		}
		
		protected static void SetSQLProps() { GetTableSchema(); }
		
		#endregion
		
		#region Schema and Query Accessor	
		public static Query CreateQuery() { return new Query(Schema); }
		public static TableSchema.Table Schema
		{
			get
			{
				if (BaseSchema == null)
					SetSQLProps();
				return BaseSchema;
			}
		}
		
		private static void GetTableSchema() 
		{
			if(!IsSchemaInitialized)
			{
				//Schema declaration
				TableSchema.Table schema = new TableSchema.Table("Delivery_Personnel", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarPersonnelId = new TableSchema.TableColumn(schema);
				colvarPersonnelId.ColumnName = "Personnel_ID";
				colvarPersonnelId.DataType = DbType.Int32;
				colvarPersonnelId.MaxLength = 0;
				colvarPersonnelId.AutoIncrement = false;
				colvarPersonnelId.IsNullable = false;
				colvarPersonnelId.IsPrimaryKey = true;
				colvarPersonnelId.IsForeignKey = false;
				colvarPersonnelId.IsReadOnly = false;
				colvarPersonnelId.DefaultSetting = @"";
				colvarPersonnelId.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPersonnelId);
				
				TableSchema.TableColumn colvarPersonnelName = new TableSchema.TableColumn(schema);
				colvarPersonnelName.ColumnName = "Personnel_Name";
				colvarPersonnelName.DataType = DbType.AnsiString;
				colvarPersonnelName.MaxLength = 150;
				colvarPersonnelName.AutoIncrement = false;
				colvarPersonnelName.IsNullable = true;
				colvarPersonnelName.IsPrimaryKey = false;
				colvarPersonnelName.IsForeignKey = false;
				colvarPersonnelName.IsReadOnly = false;
				colvarPersonnelName.DefaultSetting = @"";
				colvarPersonnelName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPersonnelName);
				
				TableSchema.TableColumn colvarRemarks = new TableSchema.TableColumn(schema);
				colvarRemarks.ColumnName = "Remarks";
				colvarRemarks.DataType = DbType.AnsiString;
				colvarRemarks.MaxLength = 200;
				colvarRemarks.AutoIncrement = false;
				colvarRemarks.IsNullable = true;
				colvarRemarks.IsPrimaryKey = false;
				colvarRemarks.IsForeignKey = false;
				colvarRemarks.IsReadOnly = false;
				colvarRemarks.DefaultSetting = @"";
				colvarRemarks.ForeignKeyTableName = "";
				schema.Columns.Add(colvarRemarks);
				
				TableSchema.TableColumn colvarTimeFrom = new TableSchema.TableColumn(schema);
				colvarTimeFrom.ColumnName = "Time_From";
				colvarTimeFrom.DataType = DbType.DateTime;
				colvarTimeFrom.MaxLength = 0;
				colvarTimeFrom.AutoIncrement = false;
				colvarTimeFrom.IsNullable = true;
				colvarTimeFrom.IsPrimaryKey = false;
				colvarTimeFrom.IsForeignKey = false;
				colvarTimeFrom.IsReadOnly = false;
				colvarTimeFrom.DefaultSetting = @"";
				colvarTimeFrom.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTimeFrom);
				
				TableSchema.TableColumn colvarTimeTo = new TableSchema.TableColumn(schema);
				colvarTimeTo.ColumnName = "Time_To";
				colvarTimeTo.DataType = DbType.DateTime;
				colvarTimeTo.MaxLength = 0;
				colvarTimeTo.AutoIncrement = false;
				colvarTimeTo.IsNullable = true;
				colvarTimeTo.IsPrimaryKey = false;
				colvarTimeTo.IsForeignKey = false;
				colvarTimeTo.IsReadOnly = false;
				colvarTimeTo.DefaultSetting = @"";
				colvarTimeTo.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTimeTo);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["PowerPOS"].AddSchema("Delivery_Personnel",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("PersonnelId")]
		[Bindable(true)]
		public int PersonnelId 
		{
			get { return GetColumnValue<int>(Columns.PersonnelId); }
			set { SetColumnValue(Columns.PersonnelId, value); }
		}
		  
		[XmlAttribute("PersonnelName")]
		[Bindable(true)]
		public string PersonnelName 
		{
			get { return GetColumnValue<string>(Columns.PersonnelName); }
			set { SetColumnValue(Columns.PersonnelName, value); }
		}
		  
		[XmlAttribute("Remarks")]
		[Bindable(true)]
		public string Remarks 
		{
			get { return GetColumnValue<string>(Columns.Remarks); }
			set { SetColumnValue(Columns.Remarks, value); }
		}
		  
		[XmlAttribute("TimeFrom")]
		[Bindable(true)]
		public DateTime? TimeFrom 
		{
			get { return GetColumnValue<DateTime?>(Columns.TimeFrom); }
			set { SetColumnValue(Columns.TimeFrom, value); }
		}
		  
		[XmlAttribute("TimeTo")]
		[Bindable(true)]
		public DateTime? TimeTo 
		{
			get { return GetColumnValue<DateTime?>(Columns.TimeTo); }
			set { SetColumnValue(Columns.TimeTo, value); }
		}
		
		#endregion
		
		
		#region PrimaryKey Methods		
		
        protected override void SetPrimaryKey(object oValue)
        {
            base.SetPrimaryKey(oValue);
            
            SetPKValues();
        }
        
		
		public PowerPOS.DeliveryOrderCollection DeliveryOrderRecords()
		{
			return new PowerPOS.DeliveryOrderCollection().Where(DeliveryOrder.Columns.PersonAssigned, PersonnelId).Load();
		}
		#endregion
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(int varPersonnelId,string varPersonnelName,string varRemarks,DateTime? varTimeFrom,DateTime? varTimeTo)
		{
			DeliveryPersonnel item = new DeliveryPersonnel();
			
			item.PersonnelId = varPersonnelId;
			
			item.PersonnelName = varPersonnelName;
			
			item.Remarks = varRemarks;
			
			item.TimeFrom = varTimeFrom;
			
			item.TimeTo = varTimeTo;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varPersonnelId,string varPersonnelName,string varRemarks,DateTime? varTimeFrom,DateTime? varTimeTo)
		{
			DeliveryPersonnel item = new DeliveryPersonnel();
			
				item.PersonnelId = varPersonnelId;
			
				item.PersonnelName = varPersonnelName;
			
				item.Remarks = varRemarks;
			
				item.TimeFrom = varTimeFrom;
			
				item.TimeTo = varTimeTo;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn PersonnelIdColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn PersonnelNameColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn RemarksColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn TimeFromColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn TimeToColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string PersonnelId = @"Personnel_ID";
			 public static string PersonnelName = @"Personnel_Name";
			 public static string Remarks = @"Remarks";
			 public static string TimeFrom = @"Time_From";
			 public static string TimeTo = @"Time_To";
						
		}
		#endregion
		
		#region Update PK Collections
		
        public void SetPKValues()
        {
}
        #endregion
    
        #region Deep Save
		
        public void DeepSave()
        {
            Save();
            
}
        #endregion
	}
}
