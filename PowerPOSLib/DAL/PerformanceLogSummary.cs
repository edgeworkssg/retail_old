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
	/// Strongly-typed collection for the PerformanceLogSummary class.
	/// </summary>
    [Serializable]
	public partial class PerformanceLogSummaryCollection : ActiveList<PerformanceLogSummary, PerformanceLogSummaryCollection>
	{	   
		public PerformanceLogSummaryCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>PerformanceLogSummaryCollection</returns>
		public PerformanceLogSummaryCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                PerformanceLogSummary o = this[i];
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
	/// This is an ActiveRecord class which wraps the PerformanceLogSummary table.
	/// </summary>
	[Serializable]
	public partial class PerformanceLogSummary : ActiveRecord<PerformanceLogSummary>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public PerformanceLogSummary()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public PerformanceLogSummary(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public PerformanceLogSummary(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public PerformanceLogSummary(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("PerformanceLogSummary", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarPerformanceLogSummaryID = new TableSchema.TableColumn(schema);
				colvarPerformanceLogSummaryID.ColumnName = "PerformanceLogSummaryID";
				colvarPerformanceLogSummaryID.DataType = DbType.Guid;
				colvarPerformanceLogSummaryID.MaxLength = 0;
				colvarPerformanceLogSummaryID.AutoIncrement = false;
				colvarPerformanceLogSummaryID.IsNullable = false;
				colvarPerformanceLogSummaryID.IsPrimaryKey = true;
				colvarPerformanceLogSummaryID.IsForeignKey = false;
				colvarPerformanceLogSummaryID.IsReadOnly = false;
				colvarPerformanceLogSummaryID.DefaultSetting = @"";
				colvarPerformanceLogSummaryID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPerformanceLogSummaryID);
				
				TableSchema.TableColumn colvarModuleName = new TableSchema.TableColumn(schema);
				colvarModuleName.ColumnName = "ModuleName";
				colvarModuleName.DataType = DbType.AnsiString;
				colvarModuleName.MaxLength = -1;
				colvarModuleName.AutoIncrement = false;
				colvarModuleName.IsNullable = false;
				colvarModuleName.IsPrimaryKey = false;
				colvarModuleName.IsForeignKey = false;
				colvarModuleName.IsReadOnly = false;
				colvarModuleName.DefaultSetting = @"";
				colvarModuleName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarModuleName);
				
				TableSchema.TableColumn colvarFunctionName = new TableSchema.TableColumn(schema);
				colvarFunctionName.ColumnName = "FunctionName";
				colvarFunctionName.DataType = DbType.AnsiString;
				colvarFunctionName.MaxLength = -1;
				colvarFunctionName.AutoIncrement = false;
				colvarFunctionName.IsNullable = false;
				colvarFunctionName.IsPrimaryKey = false;
				colvarFunctionName.IsForeignKey = false;
				colvarFunctionName.IsReadOnly = false;
				colvarFunctionName.DefaultSetting = @"";
				colvarFunctionName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarFunctionName);
				
				TableSchema.TableColumn colvarPointOfSaleID = new TableSchema.TableColumn(schema);
				colvarPointOfSaleID.ColumnName = "PointOfSaleID";
				colvarPointOfSaleID.DataType = DbType.Int32;
				colvarPointOfSaleID.MaxLength = 0;
				colvarPointOfSaleID.AutoIncrement = false;
				colvarPointOfSaleID.IsNullable = false;
				colvarPointOfSaleID.IsPrimaryKey = false;
				colvarPointOfSaleID.IsForeignKey = false;
				colvarPointOfSaleID.IsReadOnly = false;
				colvarPointOfSaleID.DefaultSetting = @"";
				colvarPointOfSaleID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPointOfSaleID);
				
				TableSchema.TableColumn colvarTimeStamp = new TableSchema.TableColumn(schema);
				colvarTimeStamp.ColumnName = "TimeStamp";
				colvarTimeStamp.DataType = DbType.DateTime;
				colvarTimeStamp.MaxLength = 0;
				colvarTimeStamp.AutoIncrement = false;
				colvarTimeStamp.IsNullable = false;
				colvarTimeStamp.IsPrimaryKey = false;
				colvarTimeStamp.IsForeignKey = false;
				colvarTimeStamp.IsReadOnly = false;
				colvarTimeStamp.DefaultSetting = @"";
				colvarTimeStamp.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTimeStamp);
				
				TableSchema.TableColumn colvarAvgElapsedTime = new TableSchema.TableColumn(schema);
				colvarAvgElapsedTime.ColumnName = "AvgElapsedTime";
				colvarAvgElapsedTime.DataType = DbType.Decimal;
				colvarAvgElapsedTime.MaxLength = 0;
				colvarAvgElapsedTime.AutoIncrement = false;
				colvarAvgElapsedTime.IsNullable = false;
				colvarAvgElapsedTime.IsPrimaryKey = false;
				colvarAvgElapsedTime.IsForeignKey = false;
				colvarAvgElapsedTime.IsReadOnly = false;
				colvarAvgElapsedTime.DefaultSetting = @"";
				colvarAvgElapsedTime.ForeignKeyTableName = "";
				schema.Columns.Add(colvarAvgElapsedTime);
				
				TableSchema.TableColumn colvarMinElapsedTime = new TableSchema.TableColumn(schema);
				colvarMinElapsedTime.ColumnName = "MinElapsedTime";
				colvarMinElapsedTime.DataType = DbType.Decimal;
				colvarMinElapsedTime.MaxLength = 0;
				colvarMinElapsedTime.AutoIncrement = false;
				colvarMinElapsedTime.IsNullable = false;
				colvarMinElapsedTime.IsPrimaryKey = false;
				colvarMinElapsedTime.IsForeignKey = false;
				colvarMinElapsedTime.IsReadOnly = false;
				colvarMinElapsedTime.DefaultSetting = @"";
				colvarMinElapsedTime.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMinElapsedTime);
				
				TableSchema.TableColumn colvarMaxElapsedTime = new TableSchema.TableColumn(schema);
				colvarMaxElapsedTime.ColumnName = "MaxElapsedTime";
				colvarMaxElapsedTime.DataType = DbType.Decimal;
				colvarMaxElapsedTime.MaxLength = 0;
				colvarMaxElapsedTime.AutoIncrement = false;
				colvarMaxElapsedTime.IsNullable = false;
				colvarMaxElapsedTime.IsPrimaryKey = false;
				colvarMaxElapsedTime.IsForeignKey = false;
				colvarMaxElapsedTime.IsReadOnly = false;
				colvarMaxElapsedTime.DefaultSetting = @"";
				colvarMaxElapsedTime.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMaxElapsedTime);
				
				TableSchema.TableColumn colvarTransCount = new TableSchema.TableColumn(schema);
				colvarTransCount.ColumnName = "TransCount";
				colvarTransCount.DataType = DbType.Int32;
				colvarTransCount.MaxLength = 0;
				colvarTransCount.AutoIncrement = false;
				colvarTransCount.IsNullable = false;
				colvarTransCount.IsPrimaryKey = false;
				colvarTransCount.IsForeignKey = false;
				colvarTransCount.IsReadOnly = false;
				colvarTransCount.DefaultSetting = @"";
				colvarTransCount.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTransCount);
				
				TableSchema.TableColumn colvarCreatedOn = new TableSchema.TableColumn(schema);
				colvarCreatedOn.ColumnName = "CreatedOn";
				colvarCreatedOn.DataType = DbType.DateTime;
				colvarCreatedOn.MaxLength = 0;
				colvarCreatedOn.AutoIncrement = false;
				colvarCreatedOn.IsNullable = false;
				colvarCreatedOn.IsPrimaryKey = false;
				colvarCreatedOn.IsForeignKey = false;
				colvarCreatedOn.IsReadOnly = false;
				colvarCreatedOn.DefaultSetting = @"";
				colvarCreatedOn.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCreatedOn);
				
				TableSchema.TableColumn colvarModifiedOn = new TableSchema.TableColumn(schema);
				colvarModifiedOn.ColumnName = "ModifiedOn";
				colvarModifiedOn.DataType = DbType.DateTime;
				colvarModifiedOn.MaxLength = 0;
				colvarModifiedOn.AutoIncrement = false;
				colvarModifiedOn.IsNullable = false;
				colvarModifiedOn.IsPrimaryKey = false;
				colvarModifiedOn.IsForeignKey = false;
				colvarModifiedOn.IsReadOnly = false;
				colvarModifiedOn.DefaultSetting = @"";
				colvarModifiedOn.ForeignKeyTableName = "";
				schema.Columns.Add(colvarModifiedOn);
				
				TableSchema.TableColumn colvarCreatedBy = new TableSchema.TableColumn(schema);
				colvarCreatedBy.ColumnName = "CreatedBy";
				colvarCreatedBy.DataType = DbType.AnsiString;
				colvarCreatedBy.MaxLength = 50;
				colvarCreatedBy.AutoIncrement = false;
				colvarCreatedBy.IsNullable = true;
				colvarCreatedBy.IsPrimaryKey = false;
				colvarCreatedBy.IsForeignKey = false;
				colvarCreatedBy.IsReadOnly = false;
				colvarCreatedBy.DefaultSetting = @"";
				colvarCreatedBy.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCreatedBy);
				
				TableSchema.TableColumn colvarModifiedBy = new TableSchema.TableColumn(schema);
				colvarModifiedBy.ColumnName = "ModifiedBy";
				colvarModifiedBy.DataType = DbType.AnsiString;
				colvarModifiedBy.MaxLength = 50;
				colvarModifiedBy.AutoIncrement = false;
				colvarModifiedBy.IsNullable = true;
				colvarModifiedBy.IsPrimaryKey = false;
				colvarModifiedBy.IsForeignKey = false;
				colvarModifiedBy.IsReadOnly = false;
				colvarModifiedBy.DefaultSetting = @"";
				colvarModifiedBy.ForeignKeyTableName = "";
				schema.Columns.Add(colvarModifiedBy);
				
				TableSchema.TableColumn colvarDeleted = new TableSchema.TableColumn(schema);
				colvarDeleted.ColumnName = "Deleted";
				colvarDeleted.DataType = DbType.Boolean;
				colvarDeleted.MaxLength = 0;
				colvarDeleted.AutoIncrement = false;
				colvarDeleted.IsNullable = true;
				colvarDeleted.IsPrimaryKey = false;
				colvarDeleted.IsForeignKey = false;
				colvarDeleted.IsReadOnly = false;
				colvarDeleted.DefaultSetting = @"";
				colvarDeleted.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDeleted);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["PowerPOS"].AddSchema("PerformanceLogSummary",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("PerformanceLogSummaryID")]
		[Bindable(true)]
		public Guid PerformanceLogSummaryID 
		{
			get { return GetColumnValue<Guid>(Columns.PerformanceLogSummaryID); }
			set { SetColumnValue(Columns.PerformanceLogSummaryID, value); }
		}
		  
		[XmlAttribute("ModuleName")]
		[Bindable(true)]
		public string ModuleName 
		{
			get { return GetColumnValue<string>(Columns.ModuleName); }
			set { SetColumnValue(Columns.ModuleName, value); }
		}
		  
		[XmlAttribute("FunctionName")]
		[Bindable(true)]
		public string FunctionName 
		{
			get { return GetColumnValue<string>(Columns.FunctionName); }
			set { SetColumnValue(Columns.FunctionName, value); }
		}
		  
		[XmlAttribute("PointOfSaleID")]
		[Bindable(true)]
		public int PointOfSaleID 
		{
			get { return GetColumnValue<int>(Columns.PointOfSaleID); }
			set { SetColumnValue(Columns.PointOfSaleID, value); }
		}
		  
		[XmlAttribute("TimeStamp")]
		[Bindable(true)]
		public DateTime TimeStamp 
		{
			get { return GetColumnValue<DateTime>(Columns.TimeStamp); }
			set { SetColumnValue(Columns.TimeStamp, value); }
		}
		  
		[XmlAttribute("AvgElapsedTime")]
		[Bindable(true)]
		public decimal AvgElapsedTime 
		{
			get { return GetColumnValue<decimal>(Columns.AvgElapsedTime); }
			set { SetColumnValue(Columns.AvgElapsedTime, value); }
		}
		  
		[XmlAttribute("MinElapsedTime")]
		[Bindable(true)]
		public decimal MinElapsedTime 
		{
			get { return GetColumnValue<decimal>(Columns.MinElapsedTime); }
			set { SetColumnValue(Columns.MinElapsedTime, value); }
		}
		  
		[XmlAttribute("MaxElapsedTime")]
		[Bindable(true)]
		public decimal MaxElapsedTime 
		{
			get { return GetColumnValue<decimal>(Columns.MaxElapsedTime); }
			set { SetColumnValue(Columns.MaxElapsedTime, value); }
		}
		  
		[XmlAttribute("TransCount")]
		[Bindable(true)]
		public int TransCount 
		{
			get { return GetColumnValue<int>(Columns.TransCount); }
			set { SetColumnValue(Columns.TransCount, value); }
		}
		  
		[XmlAttribute("CreatedOn")]
		[Bindable(true)]
		public DateTime CreatedOn 
		{
			get { return GetColumnValue<DateTime>(Columns.CreatedOn); }
			set { SetColumnValue(Columns.CreatedOn, value); }
		}
		  
		[XmlAttribute("ModifiedOn")]
		[Bindable(true)]
		public DateTime ModifiedOn 
		{
			get { return GetColumnValue<DateTime>(Columns.ModifiedOn); }
			set { SetColumnValue(Columns.ModifiedOn, value); }
		}
		  
		[XmlAttribute("CreatedBy")]
		[Bindable(true)]
		public string CreatedBy 
		{
			get { return GetColumnValue<string>(Columns.CreatedBy); }
			set { SetColumnValue(Columns.CreatedBy, value); }
		}
		  
		[XmlAttribute("ModifiedBy")]
		[Bindable(true)]
		public string ModifiedBy 
		{
			get { return GetColumnValue<string>(Columns.ModifiedBy); }
			set { SetColumnValue(Columns.ModifiedBy, value); }
		}
		  
		[XmlAttribute("Deleted")]
		[Bindable(true)]
		public bool? Deleted 
		{
			get { return GetColumnValue<bool?>(Columns.Deleted); }
			set { SetColumnValue(Columns.Deleted, value); }
		}
		
		#endregion
		
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(Guid varPerformanceLogSummaryID,string varModuleName,string varFunctionName,int varPointOfSaleID,DateTime varTimeStamp,decimal varAvgElapsedTime,decimal varMinElapsedTime,decimal varMaxElapsedTime,int varTransCount,DateTime varCreatedOn,DateTime varModifiedOn,string varCreatedBy,string varModifiedBy,bool? varDeleted)
		{
			PerformanceLogSummary item = new PerformanceLogSummary();
			
			item.PerformanceLogSummaryID = varPerformanceLogSummaryID;
			
			item.ModuleName = varModuleName;
			
			item.FunctionName = varFunctionName;
			
			item.PointOfSaleID = varPointOfSaleID;
			
			item.TimeStamp = varTimeStamp;
			
			item.AvgElapsedTime = varAvgElapsedTime;
			
			item.MinElapsedTime = varMinElapsedTime;
			
			item.MaxElapsedTime = varMaxElapsedTime;
			
			item.TransCount = varTransCount;
			
			item.CreatedOn = varCreatedOn;
			
			item.ModifiedOn = varModifiedOn;
			
			item.CreatedBy = varCreatedBy;
			
			item.ModifiedBy = varModifiedBy;
			
			item.Deleted = varDeleted;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(Guid varPerformanceLogSummaryID,string varModuleName,string varFunctionName,int varPointOfSaleID,DateTime varTimeStamp,decimal varAvgElapsedTime,decimal varMinElapsedTime,decimal varMaxElapsedTime,int varTransCount,DateTime varCreatedOn,DateTime varModifiedOn,string varCreatedBy,string varModifiedBy,bool? varDeleted)
		{
			PerformanceLogSummary item = new PerformanceLogSummary();
			
				item.PerformanceLogSummaryID = varPerformanceLogSummaryID;
			
				item.ModuleName = varModuleName;
			
				item.FunctionName = varFunctionName;
			
				item.PointOfSaleID = varPointOfSaleID;
			
				item.TimeStamp = varTimeStamp;
			
				item.AvgElapsedTime = varAvgElapsedTime;
			
				item.MinElapsedTime = varMinElapsedTime;
			
				item.MaxElapsedTime = varMaxElapsedTime;
			
				item.TransCount = varTransCount;
			
				item.CreatedOn = varCreatedOn;
			
				item.ModifiedOn = varModifiedOn;
			
				item.CreatedBy = varCreatedBy;
			
				item.ModifiedBy = varModifiedBy;
			
				item.Deleted = varDeleted;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn PerformanceLogSummaryIDColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn ModuleNameColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn FunctionNameColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn PointOfSaleIDColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn TimeStampColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn AvgElapsedTimeColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn MinElapsedTimeColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn MaxElapsedTimeColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn TransCountColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedOnColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedOnColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedByColumn
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedByColumn
        {
            get { return Schema.Columns[12]; }
        }
        
        
        
        public static TableSchema.TableColumn DeletedColumn
        {
            get { return Schema.Columns[13]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string PerformanceLogSummaryID = @"PerformanceLogSummaryID";
			 public static string ModuleName = @"ModuleName";
			 public static string FunctionName = @"FunctionName";
			 public static string PointOfSaleID = @"PointOfSaleID";
			 public static string TimeStamp = @"TimeStamp";
			 public static string AvgElapsedTime = @"AvgElapsedTime";
			 public static string MinElapsedTime = @"MinElapsedTime";
			 public static string MaxElapsedTime = @"MaxElapsedTime";
			 public static string TransCount = @"TransCount";
			 public static string CreatedOn = @"CreatedOn";
			 public static string ModifiedOn = @"ModifiedOn";
			 public static string CreatedBy = @"CreatedBy";
			 public static string ModifiedBy = @"ModifiedBy";
			 public static string Deleted = @"Deleted";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
