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
	/// Strongly-typed collection for the AppSetting class.
	/// </summary>
    [Serializable]
	public partial class AppSettingCollection : ActiveList<AppSetting, AppSettingCollection>
	{	   
		public AppSettingCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>AppSettingCollection</returns>
		public AppSettingCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                AppSetting o = this[i];
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
	/// This is an ActiveRecord class which wraps the AppSetting table.
	/// </summary>
	[Serializable]
	public partial class AppSetting : ActiveRecord<AppSetting>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public AppSetting()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public AppSetting(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public AppSetting(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public AppSetting(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("AppSetting", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarAppSettingId = new TableSchema.TableColumn(schema);
				colvarAppSettingId.ColumnName = "AppSettingId";
				colvarAppSettingId.DataType = DbType.Guid;
				colvarAppSettingId.MaxLength = 0;
				colvarAppSettingId.AutoIncrement = false;
				colvarAppSettingId.IsNullable = false;
				colvarAppSettingId.IsPrimaryKey = true;
				colvarAppSettingId.IsForeignKey = false;
				colvarAppSettingId.IsReadOnly = false;
				
						colvarAppSettingId.DefaultSetting = @"(newid())";
				colvarAppSettingId.ForeignKeyTableName = "";
				schema.Columns.Add(colvarAppSettingId);
				
				TableSchema.TableColumn colvarAppSettingKey = new TableSchema.TableColumn(schema);
				colvarAppSettingKey.ColumnName = "AppSettingKey";
				colvarAppSettingKey.DataType = DbType.AnsiString;
				colvarAppSettingKey.MaxLength = 50;
				colvarAppSettingKey.AutoIncrement = false;
				colvarAppSettingKey.IsNullable = false;
				colvarAppSettingKey.IsPrimaryKey = false;
				colvarAppSettingKey.IsForeignKey = false;
				colvarAppSettingKey.IsReadOnly = false;
				
						colvarAppSettingKey.DefaultSetting = @"(getutcdate())";
				colvarAppSettingKey.ForeignKeyTableName = "";
				schema.Columns.Add(colvarAppSettingKey);
				
				TableSchema.TableColumn colvarAppSettingValue = new TableSchema.TableColumn(schema);
				colvarAppSettingValue.ColumnName = "AppSettingValue";
				colvarAppSettingValue.DataType = DbType.String;
				colvarAppSettingValue.MaxLength = -1;
				colvarAppSettingValue.AutoIncrement = false;
				colvarAppSettingValue.IsNullable = true;
				colvarAppSettingValue.IsPrimaryKey = false;
				colvarAppSettingValue.IsForeignKey = false;
				colvarAppSettingValue.IsReadOnly = false;
				
						colvarAppSettingValue.DefaultSetting = @"(getutcdate())";
				colvarAppSettingValue.ForeignKeyTableName = "";
				schema.Columns.Add(colvarAppSettingValue);
				
				TableSchema.TableColumn colvarOutletName = new TableSchema.TableColumn(schema);
				colvarOutletName.ColumnName = "OutletName";
				colvarOutletName.DataType = DbType.AnsiString;
				colvarOutletName.MaxLength = 50;
				colvarOutletName.AutoIncrement = false;
				colvarOutletName.IsNullable = true;
				colvarOutletName.IsPrimaryKey = false;
				colvarOutletName.IsForeignKey = true;
				colvarOutletName.IsReadOnly = false;
				colvarOutletName.DefaultSetting = @"";
				
					colvarOutletName.ForeignKeyTableName = "Outlet";
				schema.Columns.Add(colvarOutletName);
				
				TableSchema.TableColumn colvarCreatedOn = new TableSchema.TableColumn(schema);
				colvarCreatedOn.ColumnName = "CreatedOn";
				colvarCreatedOn.DataType = DbType.DateTime;
				colvarCreatedOn.MaxLength = 0;
				colvarCreatedOn.AutoIncrement = false;
				colvarCreatedOn.IsNullable = true;
				colvarCreatedOn.IsPrimaryKey = false;
				colvarCreatedOn.IsForeignKey = false;
				colvarCreatedOn.IsReadOnly = false;
				colvarCreatedOn.DefaultSetting = @"";
				colvarCreatedOn.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCreatedOn);
				
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
				
				TableSchema.TableColumn colvarModifiedOn = new TableSchema.TableColumn(schema);
				colvarModifiedOn.ColumnName = "ModifiedOn";
				colvarModifiedOn.DataType = DbType.DateTime;
				colvarModifiedOn.MaxLength = 0;
				colvarModifiedOn.AutoIncrement = false;
				colvarModifiedOn.IsNullable = true;
				colvarModifiedOn.IsPrimaryKey = false;
				colvarModifiedOn.IsForeignKey = false;
				colvarModifiedOn.IsReadOnly = false;
				colvarModifiedOn.DefaultSetting = @"";
				colvarModifiedOn.ForeignKeyTableName = "";
				schema.Columns.Add(colvarModifiedOn);
				
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
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["PowerPOS"].AddSchema("AppSetting",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("AppSettingId")]
		[Bindable(true)]
		public Guid AppSettingId 
		{
			get { return GetColumnValue<Guid>(Columns.AppSettingId); }
			set { SetColumnValue(Columns.AppSettingId, value); }
		}
		  
		[XmlAttribute("AppSettingKey")]
		[Bindable(true)]
		public string AppSettingKey 
		{
			get { return GetColumnValue<string>(Columns.AppSettingKey); }
			set { SetColumnValue(Columns.AppSettingKey, value); }
		}
		  
		[XmlAttribute("AppSettingValue")]
		[Bindable(true)]
		public string AppSettingValue 
		{
			get { return GetColumnValue<string>(Columns.AppSettingValue); }
			set { SetColumnValue(Columns.AppSettingValue, value); }
		}
		  
		[XmlAttribute("OutletName")]
		[Bindable(true)]
		public string OutletName 
		{
			get { return GetColumnValue<string>(Columns.OutletName); }
			set { SetColumnValue(Columns.OutletName, value); }
		}
		  
		[XmlAttribute("CreatedOn")]
		[Bindable(true)]
		public DateTime? CreatedOn 
		{
			get { return GetColumnValue<DateTime?>(Columns.CreatedOn); }
			set { SetColumnValue(Columns.CreatedOn, value); }
		}
		  
		[XmlAttribute("CreatedBy")]
		[Bindable(true)]
		public string CreatedBy 
		{
			get { return GetColumnValue<string>(Columns.CreatedBy); }
			set { SetColumnValue(Columns.CreatedBy, value); }
		}
		  
		[XmlAttribute("ModifiedOn")]
		[Bindable(true)]
		public DateTime? ModifiedOn 
		{
			get { return GetColumnValue<DateTime?>(Columns.ModifiedOn); }
			set { SetColumnValue(Columns.ModifiedOn, value); }
		}
		  
		[XmlAttribute("ModifiedBy")]
		[Bindable(true)]
		public string ModifiedBy 
		{
			get { return GetColumnValue<string>(Columns.ModifiedBy); }
			set { SetColumnValue(Columns.ModifiedBy, value); }
		}
		
		#endregion
		
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a Outlet ActiveRecord object related to this AppSetting
		/// 
		/// </summary>
		public PowerPOS.Outlet Outlet
		{
			get { return PowerPOS.Outlet.FetchByID(this.OutletName); }
			set { SetColumnValue("OutletName", value.OutletName); }
		}
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(Guid varAppSettingId,string varAppSettingKey,string varAppSettingValue,string varOutletName,DateTime? varCreatedOn,string varCreatedBy,DateTime? varModifiedOn,string varModifiedBy)
		{
			AppSetting item = new AppSetting();
			
			item.AppSettingId = varAppSettingId;
			
			item.AppSettingKey = varAppSettingKey;
			
			item.AppSettingValue = varAppSettingValue;
			
			item.OutletName = varOutletName;
			
			item.CreatedOn = varCreatedOn;
			
			item.CreatedBy = varCreatedBy;
			
			item.ModifiedOn = varModifiedOn;
			
			item.ModifiedBy = varModifiedBy;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(Guid varAppSettingId,string varAppSettingKey,string varAppSettingValue,string varOutletName,DateTime? varCreatedOn,string varCreatedBy,DateTime? varModifiedOn,string varModifiedBy)
		{
			AppSetting item = new AppSetting();
			
				item.AppSettingId = varAppSettingId;
			
				item.AppSettingKey = varAppSettingKey;
			
				item.AppSettingValue = varAppSettingValue;
			
				item.OutletName = varOutletName;
			
				item.CreatedOn = varCreatedOn;
			
				item.CreatedBy = varCreatedBy;
			
				item.ModifiedOn = varModifiedOn;
			
				item.ModifiedBy = varModifiedBy;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn AppSettingIdColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn AppSettingKeyColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn AppSettingValueColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn OutletNameColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedOnColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedByColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedOnColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedByColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string AppSettingId = @"AppSettingId";
			 public static string AppSettingKey = @"AppSettingKey";
			 public static string AppSettingValue = @"AppSettingValue";
			 public static string OutletName = @"OutletName";
			 public static string CreatedOn = @"CreatedOn";
			 public static string CreatedBy = @"CreatedBy";
			 public static string ModifiedOn = @"ModifiedOn";
			 public static string ModifiedBy = @"ModifiedBy";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
