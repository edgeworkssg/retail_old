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
	/// Strongly-typed collection for the PerformanceLog class.
	/// </summary>
    [Serializable]
	public partial class PerformanceLogCollection : ActiveList<PerformanceLog, PerformanceLogCollection>
	{	   
		public PerformanceLogCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>PerformanceLogCollection</returns>
		public PerformanceLogCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                PerformanceLog o = this[i];
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
	/// This is an ActiveRecord class which wraps the PerformanceLog table.
	/// </summary>
	[Serializable]
	public partial class PerformanceLog : ActiveRecord<PerformanceLog>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public PerformanceLog()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public PerformanceLog(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public PerformanceLog(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public PerformanceLog(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("PerformanceLog", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarPerformanceLogID = new TableSchema.TableColumn(schema);
				colvarPerformanceLogID.ColumnName = "PerformanceLogID";
				colvarPerformanceLogID.DataType = DbType.Guid;
				colvarPerformanceLogID.MaxLength = 0;
				colvarPerformanceLogID.AutoIncrement = false;
				colvarPerformanceLogID.IsNullable = false;
				colvarPerformanceLogID.IsPrimaryKey = true;
				colvarPerformanceLogID.IsForeignKey = false;
				colvarPerformanceLogID.IsReadOnly = false;
				colvarPerformanceLogID.DefaultSetting = @"";
				colvarPerformanceLogID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPerformanceLogID);
				
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
				
				TableSchema.TableColumn colvarElapsedTime = new TableSchema.TableColumn(schema);
				colvarElapsedTime.ColumnName = "ElapsedTime";
				colvarElapsedTime.DataType = DbType.Decimal;
				colvarElapsedTime.MaxLength = 0;
				colvarElapsedTime.AutoIncrement = false;
				colvarElapsedTime.IsNullable = false;
				colvarElapsedTime.IsPrimaryKey = false;
				colvarElapsedTime.IsForeignKey = false;
				colvarElapsedTime.IsReadOnly = false;
				colvarElapsedTime.DefaultSetting = @"";
				colvarElapsedTime.ForeignKeyTableName = "";
				schema.Columns.Add(colvarElapsedTime);
				
				TableSchema.TableColumn colvarPrimaryKeyData = new TableSchema.TableColumn(schema);
				colvarPrimaryKeyData.ColumnName = "PrimaryKeyData";
				colvarPrimaryKeyData.DataType = DbType.AnsiString;
				colvarPrimaryKeyData.MaxLength = -1;
				colvarPrimaryKeyData.AutoIncrement = false;
				colvarPrimaryKeyData.IsNullable = true;
				colvarPrimaryKeyData.IsPrimaryKey = false;
				colvarPrimaryKeyData.IsForeignKey = false;
				colvarPrimaryKeyData.IsReadOnly = false;
				colvarPrimaryKeyData.DefaultSetting = @"";
				colvarPrimaryKeyData.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPrimaryKeyData);
				
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
				DataService.Providers["PowerPOS"].AddSchema("PerformanceLog",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("PerformanceLogID")]
		[Bindable(true)]
		public Guid PerformanceLogID 
		{
			get { return GetColumnValue<Guid>(Columns.PerformanceLogID); }
			set { SetColumnValue(Columns.PerformanceLogID, value); }
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
		  
		[XmlAttribute("ElapsedTime")]
		[Bindable(true)]
		public decimal ElapsedTime 
		{
			get { return GetColumnValue<decimal>(Columns.ElapsedTime); }
			set { SetColumnValue(Columns.ElapsedTime, value); }
		}
		  
		[XmlAttribute("PrimaryKeyData")]
		[Bindable(true)]
		public string PrimaryKeyData 
		{
			get { return GetColumnValue<string>(Columns.PrimaryKeyData); }
			set { SetColumnValue(Columns.PrimaryKeyData, value); }
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
		public static void Insert(Guid varPerformanceLogID,string varModuleName,string varFunctionName,int varPointOfSaleID,decimal varElapsedTime,string varPrimaryKeyData,DateTime varCreatedOn,DateTime varModifiedOn,string varCreatedBy,string varModifiedBy,bool? varDeleted)
		{
			PerformanceLog item = new PerformanceLog();
			
			item.PerformanceLogID = varPerformanceLogID;
			
			item.ModuleName = varModuleName;
			
			item.FunctionName = varFunctionName;
			
			item.PointOfSaleID = varPointOfSaleID;
			
			item.ElapsedTime = varElapsedTime;
			
			item.PrimaryKeyData = varPrimaryKeyData;
			
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
		public static void Update(Guid varPerformanceLogID,string varModuleName,string varFunctionName,int varPointOfSaleID,decimal varElapsedTime,string varPrimaryKeyData,DateTime varCreatedOn,DateTime varModifiedOn,string varCreatedBy,string varModifiedBy,bool? varDeleted)
		{
			PerformanceLog item = new PerformanceLog();
			
				item.PerformanceLogID = varPerformanceLogID;
			
				item.ModuleName = varModuleName;
			
				item.FunctionName = varFunctionName;
			
				item.PointOfSaleID = varPointOfSaleID;
			
				item.ElapsedTime = varElapsedTime;
			
				item.PrimaryKeyData = varPrimaryKeyData;
			
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
        
        
        public static TableSchema.TableColumn PerformanceLogIDColumn
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
        
        
        
        public static TableSchema.TableColumn ElapsedTimeColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn PrimaryKeyDataColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedOnColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedOnColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedByColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedByColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn DeletedColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string PerformanceLogID = @"PerformanceLogID";
			 public static string ModuleName = @"ModuleName";
			 public static string FunctionName = @"FunctionName";
			 public static string PointOfSaleID = @"PointOfSaleID";
			 public static string ElapsedTime = @"ElapsedTime";
			 public static string PrimaryKeyData = @"PrimaryKeyData";
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
