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
	/// Strongly-typed collection for the QuickAccessCategory class.
	/// </summary>
    [Serializable]
	public partial class QuickAccessCategoryCollection : ActiveList<QuickAccessCategory, QuickAccessCategoryCollection>
	{	   
		public QuickAccessCategoryCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>QuickAccessCategoryCollection</returns>
		public QuickAccessCategoryCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                QuickAccessCategory o = this[i];
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
	/// This is an ActiveRecord class which wraps the QuickAccessCategory table.
	/// </summary>
	[Serializable]
	public partial class QuickAccessCategory : ActiveRecord<QuickAccessCategory>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public QuickAccessCategory()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public QuickAccessCategory(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public QuickAccessCategory(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public QuickAccessCategory(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("QuickAccessCategory", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarQuickAccessCategoryId = new TableSchema.TableColumn(schema);
				colvarQuickAccessCategoryId.ColumnName = "QuickAccessCategoryId";
				colvarQuickAccessCategoryId.DataType = DbType.Guid;
				colvarQuickAccessCategoryId.MaxLength = 0;
				colvarQuickAccessCategoryId.AutoIncrement = false;
				colvarQuickAccessCategoryId.IsNullable = false;
				colvarQuickAccessCategoryId.IsPrimaryKey = true;
				colvarQuickAccessCategoryId.IsForeignKey = false;
				colvarQuickAccessCategoryId.IsReadOnly = false;
				
						colvarQuickAccessCategoryId.DefaultSetting = @"(newid())";
				colvarQuickAccessCategoryId.ForeignKeyTableName = "";
				schema.Columns.Add(colvarQuickAccessCategoryId);
				
				TableSchema.TableColumn colvarQuickAccessCatName = new TableSchema.TableColumn(schema);
				colvarQuickAccessCatName.ColumnName = "QuickAccessCatName";
				colvarQuickAccessCatName.DataType = DbType.String;
				colvarQuickAccessCatName.MaxLength = 50;
				colvarQuickAccessCatName.AutoIncrement = false;
				colvarQuickAccessCatName.IsNullable = false;
				colvarQuickAccessCatName.IsPrimaryKey = false;
				colvarQuickAccessCatName.IsForeignKey = false;
				colvarQuickAccessCatName.IsReadOnly = false;
				colvarQuickAccessCatName.DefaultSetting = @"";
				colvarQuickAccessCatName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarQuickAccessCatName);
				
				TableSchema.TableColumn colvarPriorityLevel = new TableSchema.TableColumn(schema);
				colvarPriorityLevel.ColumnName = "PriorityLevel";
				colvarPriorityLevel.DataType = DbType.Int32;
				colvarPriorityLevel.MaxLength = 0;
				colvarPriorityLevel.AutoIncrement = false;
				colvarPriorityLevel.IsNullable = false;
				colvarPriorityLevel.IsPrimaryKey = false;
				colvarPriorityLevel.IsForeignKey = false;
				colvarPriorityLevel.IsReadOnly = false;
				colvarPriorityLevel.DefaultSetting = @"";
				colvarPriorityLevel.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPriorityLevel);
				
				TableSchema.TableColumn colvarPointOfSaleID = new TableSchema.TableColumn(schema);
				colvarPointOfSaleID.ColumnName = "PointOfSaleID";
				colvarPointOfSaleID.DataType = DbType.Int32;
				colvarPointOfSaleID.MaxLength = 0;
				colvarPointOfSaleID.AutoIncrement = false;
				colvarPointOfSaleID.IsNullable = true;
				colvarPointOfSaleID.IsPrimaryKey = false;
				colvarPointOfSaleID.IsForeignKey = false;
				colvarPointOfSaleID.IsReadOnly = false;
				colvarPointOfSaleID.DefaultSetting = @"";
				colvarPointOfSaleID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPointOfSaleID);
				
				TableSchema.TableColumn colvarForeColor = new TableSchema.TableColumn(schema);
				colvarForeColor.ColumnName = "ForeColor";
				colvarForeColor.DataType = DbType.AnsiString;
				colvarForeColor.MaxLength = 50;
				colvarForeColor.AutoIncrement = false;
				colvarForeColor.IsNullable = true;
				colvarForeColor.IsPrimaryKey = false;
				colvarForeColor.IsForeignKey = false;
				colvarForeColor.IsReadOnly = false;
				colvarForeColor.DefaultSetting = @"";
				colvarForeColor.ForeignKeyTableName = "";
				schema.Columns.Add(colvarForeColor);
				
				TableSchema.TableColumn colvarBackColor = new TableSchema.TableColumn(schema);
				colvarBackColor.ColumnName = "BackColor";
				colvarBackColor.DataType = DbType.AnsiString;
				colvarBackColor.MaxLength = 50;
				colvarBackColor.AutoIncrement = false;
				colvarBackColor.IsNullable = true;
				colvarBackColor.IsPrimaryKey = false;
				colvarBackColor.IsForeignKey = false;
				colvarBackColor.IsReadOnly = false;
				colvarBackColor.DefaultSetting = @"";
				colvarBackColor.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBackColor);
				
				TableSchema.TableColumn colvarLabel = new TableSchema.TableColumn(schema);
				colvarLabel.ColumnName = "Label";
				colvarLabel.DataType = DbType.AnsiString;
				colvarLabel.MaxLength = 50;
				colvarLabel.AutoIncrement = false;
				colvarLabel.IsNullable = true;
				colvarLabel.IsPrimaryKey = false;
				colvarLabel.IsForeignKey = false;
				colvarLabel.IsReadOnly = false;
				colvarLabel.DefaultSetting = @"";
				colvarLabel.ForeignKeyTableName = "";
				schema.Columns.Add(colvarLabel);
				
				TableSchema.TableColumn colvarShowLabel = new TableSchema.TableColumn(schema);
				colvarShowLabel.ColumnName = "ShowLabel";
				colvarShowLabel.DataType = DbType.Boolean;
				colvarShowLabel.MaxLength = 0;
				colvarShowLabel.AutoIncrement = false;
				colvarShowLabel.IsNullable = false;
				colvarShowLabel.IsPrimaryKey = false;
				colvarShowLabel.IsForeignKey = false;
				colvarShowLabel.IsReadOnly = false;
				
						colvarShowLabel.DefaultSetting = @"((0))";
				colvarShowLabel.ForeignKeyTableName = "";
				schema.Columns.Add(colvarShowLabel);
				
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
				DataService.Providers["PowerPOS"].AddSchema("QuickAccessCategory",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("QuickAccessCategoryId")]
		[Bindable(true)]
		public Guid QuickAccessCategoryId 
		{
			get { return GetColumnValue<Guid>(Columns.QuickAccessCategoryId); }
			set { SetColumnValue(Columns.QuickAccessCategoryId, value); }
		}
		  
		[XmlAttribute("QuickAccessCatName")]
		[Bindable(true)]
		public string QuickAccessCatName 
		{
			get { return GetColumnValue<string>(Columns.QuickAccessCatName); }
			set { SetColumnValue(Columns.QuickAccessCatName, value); }
		}
		  
		[XmlAttribute("PriorityLevel")]
		[Bindable(true)]
		public int PriorityLevel 
		{
			get { return GetColumnValue<int>(Columns.PriorityLevel); }
			set { SetColumnValue(Columns.PriorityLevel, value); }
		}
		  
		[XmlAttribute("PointOfSaleID")]
		[Bindable(true)]
		public int? PointOfSaleID 
		{
			get { return GetColumnValue<int?>(Columns.PointOfSaleID); }
			set { SetColumnValue(Columns.PointOfSaleID, value); }
		}
		  
		[XmlAttribute("ForeColor")]
		[Bindable(true)]
		public string ForeColor 
		{
			get { return GetColumnValue<string>(Columns.ForeColor); }
			set { SetColumnValue(Columns.ForeColor, value); }
		}
		  
		[XmlAttribute("BackColor")]
		[Bindable(true)]
		public string BackColor 
		{
			get { return GetColumnValue<string>(Columns.BackColor); }
			set { SetColumnValue(Columns.BackColor, value); }
		}
		  
		[XmlAttribute("Label")]
		[Bindable(true)]
		public string Label 
		{
			get { return GetColumnValue<string>(Columns.Label); }
			set { SetColumnValue(Columns.Label, value); }
		}
		  
		[XmlAttribute("ShowLabel")]
		[Bindable(true)]
		public bool ShowLabel 
		{
			get { return GetColumnValue<bool>(Columns.ShowLabel); }
			set { SetColumnValue(Columns.ShowLabel, value); }
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
		
		
		#region PrimaryKey Methods		
		
        protected override void SetPrimaryKey(object oValue)
        {
            base.SetPrimaryKey(oValue);
            
            SetPKValues();
        }
        
		
		public PowerPOS.QuickAccessButtonCollection QuickAccessButtonRecords()
		{
			return new PowerPOS.QuickAccessButtonCollection().Where(QuickAccessButton.Columns.QuickAccessCategoryID, QuickAccessCategoryId).Load();
		}
		public PowerPOS.QuickAccessButtonCollection QuickAccessButtonRecordsFromQuickAccessCategory()
		{
			return new PowerPOS.QuickAccessButtonCollection().Where(QuickAccessButton.Columns.QuickAccessCategoryID, QuickAccessCategoryId).Load();
		}
		public PowerPOS.QuickAccessGroupMapCollection QuickAccessGroupMapRecords()
		{
			return new PowerPOS.QuickAccessGroupMapCollection().Where(QuickAccessGroupMap.Columns.QuickAccessCategoryID, QuickAccessCategoryId).Load();
		}
		#endregion
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(Guid varQuickAccessCategoryId,string varQuickAccessCatName,int varPriorityLevel,int? varPointOfSaleID,string varForeColor,string varBackColor,string varLabel,bool varShowLabel,bool? varDeleted,DateTime? varCreatedOn,string varCreatedBy,DateTime? varModifiedOn,string varModifiedBy)
		{
			QuickAccessCategory item = new QuickAccessCategory();
			
			item.QuickAccessCategoryId = varQuickAccessCategoryId;
			
			item.QuickAccessCatName = varQuickAccessCatName;
			
			item.PriorityLevel = varPriorityLevel;
			
			item.PointOfSaleID = varPointOfSaleID;
			
			item.ForeColor = varForeColor;
			
			item.BackColor = varBackColor;
			
			item.Label = varLabel;
			
			item.ShowLabel = varShowLabel;
			
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
		public static void Update(Guid varQuickAccessCategoryId,string varQuickAccessCatName,int varPriorityLevel,int? varPointOfSaleID,string varForeColor,string varBackColor,string varLabel,bool varShowLabel,bool? varDeleted,DateTime? varCreatedOn,string varCreatedBy,DateTime? varModifiedOn,string varModifiedBy)
		{
			QuickAccessCategory item = new QuickAccessCategory();
			
				item.QuickAccessCategoryId = varQuickAccessCategoryId;
			
				item.QuickAccessCatName = varQuickAccessCatName;
			
				item.PriorityLevel = varPriorityLevel;
			
				item.PointOfSaleID = varPointOfSaleID;
			
				item.ForeColor = varForeColor;
			
				item.BackColor = varBackColor;
			
				item.Label = varLabel;
			
				item.ShowLabel = varShowLabel;
			
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
        
        
        public static TableSchema.TableColumn QuickAccessCategoryIdColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn QuickAccessCatNameColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn PriorityLevelColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn PointOfSaleIDColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn ForeColorColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn BackColorColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn LabelColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn ShowLabelColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn DeletedColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedOnColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedByColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedOnColumn
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedByColumn
        {
            get { return Schema.Columns[12]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string QuickAccessCategoryId = @"QuickAccessCategoryId";
			 public static string QuickAccessCatName = @"QuickAccessCatName";
			 public static string PriorityLevel = @"PriorityLevel";
			 public static string PointOfSaleID = @"PointOfSaleID";
			 public static string ForeColor = @"ForeColor";
			 public static string BackColor = @"BackColor";
			 public static string Label = @"Label";
			 public static string ShowLabel = @"ShowLabel";
			 public static string Deleted = @"Deleted";
			 public static string CreatedOn = @"CreatedOn";
			 public static string CreatedBy = @"CreatedBy";
			 public static string ModifiedOn = @"ModifiedOn";
			 public static string ModifiedBy = @"ModifiedBy";
						
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
