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
	/// Strongly-typed collection for the HotKeysCategory class.
	/// </summary>
    [Serializable]
	public partial class HotKeysCategoryCollection : ActiveList<HotKeysCategory, HotKeysCategoryCollection>
	{	   
		public HotKeysCategoryCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>HotKeysCategoryCollection</returns>
		public HotKeysCategoryCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                HotKeysCategory o = this[i];
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
	/// This is an ActiveRecord class which wraps the HotKeysCategory table.
	/// </summary>
	[Serializable]
	public partial class HotKeysCategory : ActiveRecord<HotKeysCategory>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public HotKeysCategory()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public HotKeysCategory(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public HotKeysCategory(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public HotKeysCategory(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("HotKeysCategory", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarHotKeyID = new TableSchema.TableColumn(schema);
				colvarHotKeyID.ColumnName = "HotKeyID";
				colvarHotKeyID.DataType = DbType.AnsiString;
				colvarHotKeyID.MaxLength = 64;
				colvarHotKeyID.AutoIncrement = false;
				colvarHotKeyID.IsNullable = false;
				colvarHotKeyID.IsPrimaryKey = false;
				colvarHotKeyID.IsForeignKey = false;
				colvarHotKeyID.IsReadOnly = false;
				colvarHotKeyID.DefaultSetting = @"";
				colvarHotKeyID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarHotKeyID);
				
				TableSchema.TableColumn colvarKeyCode = new TableSchema.TableColumn(schema);
				colvarKeyCode.ColumnName = "KeyCode";
				colvarKeyCode.DataType = DbType.AnsiString;
				colvarKeyCode.MaxLength = 100;
				colvarKeyCode.AutoIncrement = false;
				colvarKeyCode.IsNullable = false;
				colvarKeyCode.IsPrimaryKey = true;
				colvarKeyCode.IsForeignKey = false;
				colvarKeyCode.IsReadOnly = false;
				colvarKeyCode.DefaultSetting = @"";
				colvarKeyCode.ForeignKeyTableName = "";
				schema.Columns.Add(colvarKeyCode);
				
				TableSchema.TableColumn colvarKeyName = new TableSchema.TableColumn(schema);
				colvarKeyName.ColumnName = "KeyName";
				colvarKeyName.DataType = DbType.AnsiString;
				colvarKeyName.MaxLength = 100;
				colvarKeyName.AutoIncrement = false;
				colvarKeyName.IsNullable = false;
				colvarKeyName.IsPrimaryKey = false;
				colvarKeyName.IsForeignKey = false;
				colvarKeyName.IsReadOnly = false;
				colvarKeyName.DefaultSetting = @"";
				colvarKeyName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarKeyName);
				
				TableSchema.TableColumn colvarCategoryName = new TableSchema.TableColumn(schema);
				colvarCategoryName.ColumnName = "CategoryName";
				colvarCategoryName.DataType = DbType.AnsiString;
				colvarCategoryName.MaxLength = 100;
				colvarCategoryName.AutoIncrement = false;
				colvarCategoryName.IsNullable = false;
				colvarCategoryName.IsPrimaryKey = false;
				colvarCategoryName.IsForeignKey = false;
				colvarCategoryName.IsReadOnly = false;
				colvarCategoryName.DefaultSetting = @"";
				colvarCategoryName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCategoryName);
				
				TableSchema.TableColumn colvarCreatedBy = new TableSchema.TableColumn(schema);
				colvarCreatedBy.ColumnName = "CreatedBy";
				colvarCreatedBy.DataType = DbType.AnsiString;
				colvarCreatedBy.MaxLength = 100;
				colvarCreatedBy.AutoIncrement = false;
				colvarCreatedBy.IsNullable = false;
				colvarCreatedBy.IsPrimaryKey = false;
				colvarCreatedBy.IsForeignKey = false;
				colvarCreatedBy.IsReadOnly = false;
				colvarCreatedBy.DefaultSetting = @"";
				colvarCreatedBy.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCreatedBy);
				
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
				
				TableSchema.TableColumn colvarModifiedBy = new TableSchema.TableColumn(schema);
				colvarModifiedBy.ColumnName = "ModifiedBy";
				colvarModifiedBy.DataType = DbType.AnsiString;
				colvarModifiedBy.MaxLength = 100;
				colvarModifiedBy.AutoIncrement = false;
				colvarModifiedBy.IsNullable = false;
				colvarModifiedBy.IsPrimaryKey = false;
				colvarModifiedBy.IsForeignKey = false;
				colvarModifiedBy.IsReadOnly = false;
				colvarModifiedBy.DefaultSetting = @"";
				colvarModifiedBy.ForeignKeyTableName = "";
				schema.Columns.Add(colvarModifiedBy);
				
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
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["PowerPOS"].AddSchema("HotKeysCategory",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("HotKeyID")]
		[Bindable(true)]
		public string HotKeyID 
		{
			get { return GetColumnValue<string>(Columns.HotKeyID); }
			set { SetColumnValue(Columns.HotKeyID, value); }
		}
		  
		[XmlAttribute("KeyCode")]
		[Bindable(true)]
		public string KeyCode 
		{
			get { return GetColumnValue<string>(Columns.KeyCode); }
			set { SetColumnValue(Columns.KeyCode, value); }
		}
		  
		[XmlAttribute("KeyName")]
		[Bindable(true)]
		public string KeyName 
		{
			get { return GetColumnValue<string>(Columns.KeyName); }
			set { SetColumnValue(Columns.KeyName, value); }
		}
		  
		[XmlAttribute("CategoryName")]
		[Bindable(true)]
		public string CategoryName 
		{
			get { return GetColumnValue<string>(Columns.CategoryName); }
			set { SetColumnValue(Columns.CategoryName, value); }
		}
		  
		[XmlAttribute("CreatedBy")]
		[Bindable(true)]
		public string CreatedBy 
		{
			get { return GetColumnValue<string>(Columns.CreatedBy); }
			set { SetColumnValue(Columns.CreatedBy, value); }
		}
		  
		[XmlAttribute("CreatedOn")]
		[Bindable(true)]
		public DateTime CreatedOn 
		{
			get { return GetColumnValue<DateTime>(Columns.CreatedOn); }
			set { SetColumnValue(Columns.CreatedOn, value); }
		}
		  
		[XmlAttribute("ModifiedBy")]
		[Bindable(true)]
		public string ModifiedBy 
		{
			get { return GetColumnValue<string>(Columns.ModifiedBy); }
			set { SetColumnValue(Columns.ModifiedBy, value); }
		}
		  
		[XmlAttribute("ModifiedOn")]
		[Bindable(true)]
		public DateTime ModifiedOn 
		{
			get { return GetColumnValue<DateTime>(Columns.ModifiedOn); }
			set { SetColumnValue(Columns.ModifiedOn, value); }
		}
		
		#endregion
		
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(string varHotKeyID,string varKeyCode,string varKeyName,string varCategoryName,string varCreatedBy,DateTime varCreatedOn,string varModifiedBy,DateTime varModifiedOn)
		{
			HotKeysCategory item = new HotKeysCategory();
			
			item.HotKeyID = varHotKeyID;
			
			item.KeyCode = varKeyCode;
			
			item.KeyName = varKeyName;
			
			item.CategoryName = varCategoryName;
			
			item.CreatedBy = varCreatedBy;
			
			item.CreatedOn = varCreatedOn;
			
			item.ModifiedBy = varModifiedBy;
			
			item.ModifiedOn = varModifiedOn;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(string varHotKeyID,string varKeyCode,string varKeyName,string varCategoryName,string varCreatedBy,DateTime varCreatedOn,string varModifiedBy,DateTime varModifiedOn)
		{
			HotKeysCategory item = new HotKeysCategory();
			
				item.HotKeyID = varHotKeyID;
			
				item.KeyCode = varKeyCode;
			
				item.KeyName = varKeyName;
			
				item.CategoryName = varCategoryName;
			
				item.CreatedBy = varCreatedBy;
			
				item.CreatedOn = varCreatedOn;
			
				item.ModifiedBy = varModifiedBy;
			
				item.ModifiedOn = varModifiedOn;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn HotKeyIDColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn KeyCodeColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn KeyNameColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn CategoryNameColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedByColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedOnColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedByColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedOnColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string HotKeyID = @"HotKeyID";
			 public static string KeyCode = @"KeyCode";
			 public static string KeyName = @"KeyName";
			 public static string CategoryName = @"CategoryName";
			 public static string CreatedBy = @"CreatedBy";
			 public static string CreatedOn = @"CreatedOn";
			 public static string ModifiedBy = @"ModifiedBy";
			 public static string ModifiedOn = @"ModifiedOn";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
