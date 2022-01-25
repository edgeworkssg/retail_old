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
	/// Strongly-typed collection for the QuickAccessGroupMap class.
	/// </summary>
    [Serializable]
	public partial class QuickAccessGroupMapCollection : ActiveList<QuickAccessGroupMap, QuickAccessGroupMapCollection>
	{	   
		public QuickAccessGroupMapCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>QuickAccessGroupMapCollection</returns>
		public QuickAccessGroupMapCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                QuickAccessGroupMap o = this[i];
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
	/// This is an ActiveRecord class which wraps the QuickAccessGroupMap table.
	/// </summary>
	[Serializable]
	public partial class QuickAccessGroupMap : ActiveRecord<QuickAccessGroupMap>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public QuickAccessGroupMap()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public QuickAccessGroupMap(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public QuickAccessGroupMap(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public QuickAccessGroupMap(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("QuickAccessGroupMap", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarQuickAccessGroupMapID = new TableSchema.TableColumn(schema);
				colvarQuickAccessGroupMapID.ColumnName = "QuickAccessGroupMapID";
				colvarQuickAccessGroupMapID.DataType = DbType.Guid;
				colvarQuickAccessGroupMapID.MaxLength = 0;
				colvarQuickAccessGroupMapID.AutoIncrement = false;
				colvarQuickAccessGroupMapID.IsNullable = false;
				colvarQuickAccessGroupMapID.IsPrimaryKey = true;
				colvarQuickAccessGroupMapID.IsForeignKey = false;
				colvarQuickAccessGroupMapID.IsReadOnly = false;
				
						colvarQuickAccessGroupMapID.DefaultSetting = @"(newid())";
				colvarQuickAccessGroupMapID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarQuickAccessGroupMapID);
				
				TableSchema.TableColumn colvarQuickAccessGroupID = new TableSchema.TableColumn(schema);
				colvarQuickAccessGroupID.ColumnName = "QuickAccessGroupID";
				colvarQuickAccessGroupID.DataType = DbType.Guid;
				colvarQuickAccessGroupID.MaxLength = 0;
				colvarQuickAccessGroupID.AutoIncrement = false;
				colvarQuickAccessGroupID.IsNullable = false;
				colvarQuickAccessGroupID.IsPrimaryKey = false;
				colvarQuickAccessGroupID.IsForeignKey = true;
				colvarQuickAccessGroupID.IsReadOnly = false;
				colvarQuickAccessGroupID.DefaultSetting = @"";
				
					colvarQuickAccessGroupID.ForeignKeyTableName = "QuickAccessGroup";
				schema.Columns.Add(colvarQuickAccessGroupID);
				
				TableSchema.TableColumn colvarQuickAccessCategoryID = new TableSchema.TableColumn(schema);
				colvarQuickAccessCategoryID.ColumnName = "QuickAccessCategoryID";
				colvarQuickAccessCategoryID.DataType = DbType.Guid;
				colvarQuickAccessCategoryID.MaxLength = 0;
				colvarQuickAccessCategoryID.AutoIncrement = false;
				colvarQuickAccessCategoryID.IsNullable = false;
				colvarQuickAccessCategoryID.IsPrimaryKey = false;
				colvarQuickAccessCategoryID.IsForeignKey = true;
				colvarQuickAccessCategoryID.IsReadOnly = false;
				colvarQuickAccessCategoryID.DefaultSetting = @"";
				
					colvarQuickAccessCategoryID.ForeignKeyTableName = "QuickAccessCategory";
				schema.Columns.Add(colvarQuickAccessCategoryID);
				
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
				DataService.Providers["PowerPOS"].AddSchema("QuickAccessGroupMap",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("QuickAccessGroupMapID")]
		[Bindable(true)]
		public Guid QuickAccessGroupMapID 
		{
			get { return GetColumnValue<Guid>(Columns.QuickAccessGroupMapID); }
			set { SetColumnValue(Columns.QuickAccessGroupMapID, value); }
		}
		  
		[XmlAttribute("QuickAccessGroupID")]
		[Bindable(true)]
		public Guid QuickAccessGroupID 
		{
			get { return GetColumnValue<Guid>(Columns.QuickAccessGroupID); }
			set { SetColumnValue(Columns.QuickAccessGroupID, value); }
		}
		  
		[XmlAttribute("QuickAccessCategoryID")]
		[Bindable(true)]
		public Guid QuickAccessCategoryID 
		{
			get { return GetColumnValue<Guid>(Columns.QuickAccessCategoryID); }
			set { SetColumnValue(Columns.QuickAccessCategoryID, value); }
		}
		  
		[XmlAttribute("Deleted")]
		[Bindable(true)]
		public bool? Deleted 
		{
			get { return GetColumnValue<bool?>(Columns.Deleted); }
			set { SetColumnValue(Columns.Deleted, value); }
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
		/// Returns a QuickAccessCategory ActiveRecord object related to this QuickAccessGroupMap
		/// 
		/// </summary>
		public PowerPOS.QuickAccessCategory QuickAccessCategory
		{
			get { return PowerPOS.QuickAccessCategory.FetchByID(this.QuickAccessCategoryID); }
			set { SetColumnValue("QuickAccessCategoryID", value.QuickAccessCategoryId); }
		}
		
		
		/// <summary>
		/// Returns a QuickAccessGroup ActiveRecord object related to this QuickAccessGroupMap
		/// 
		/// </summary>
		public PowerPOS.QuickAccessGroup QuickAccessGroup
		{
			get { return PowerPOS.QuickAccessGroup.FetchByID(this.QuickAccessGroupID); }
			set { SetColumnValue("QuickAccessGroupID", value.QuickAccessGroupId); }
		}
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(Guid varQuickAccessGroupMapID,Guid varQuickAccessGroupID,Guid varQuickAccessCategoryID,bool? varDeleted,DateTime? varCreatedOn,string varCreatedBy,DateTime? varModifiedOn,string varModifiedBy)
		{
			QuickAccessGroupMap item = new QuickAccessGroupMap();
			
			item.QuickAccessGroupMapID = varQuickAccessGroupMapID;
			
			item.QuickAccessGroupID = varQuickAccessGroupID;
			
			item.QuickAccessCategoryID = varQuickAccessCategoryID;
			
			item.Deleted = varDeleted;
			
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
		public static void Update(Guid varQuickAccessGroupMapID,Guid varQuickAccessGroupID,Guid varQuickAccessCategoryID,bool? varDeleted,DateTime? varCreatedOn,string varCreatedBy,DateTime? varModifiedOn,string varModifiedBy)
		{
			QuickAccessGroupMap item = new QuickAccessGroupMap();
			
				item.QuickAccessGroupMapID = varQuickAccessGroupMapID;
			
				item.QuickAccessGroupID = varQuickAccessGroupID;
			
				item.QuickAccessCategoryID = varQuickAccessCategoryID;
			
				item.Deleted = varDeleted;
			
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
        
        
        public static TableSchema.TableColumn QuickAccessGroupMapIDColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn QuickAccessGroupIDColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn QuickAccessCategoryIDColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn DeletedColumn
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
			 public static string QuickAccessGroupMapID = @"QuickAccessGroupMapID";
			 public static string QuickAccessGroupID = @"QuickAccessGroupID";
			 public static string QuickAccessCategoryID = @"QuickAccessCategoryID";
			 public static string Deleted = @"Deleted";
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
