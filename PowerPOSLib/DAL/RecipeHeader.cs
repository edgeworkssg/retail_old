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
	/// Strongly-typed collection for the RecipeHeader class.
	/// </summary>
    [Serializable]
	public partial class RecipeHeaderCollection : ActiveList<RecipeHeader, RecipeHeaderCollection>
	{	   
		public RecipeHeaderCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>RecipeHeaderCollection</returns>
		public RecipeHeaderCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                RecipeHeader o = this[i];
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
	/// This is an ActiveRecord class which wraps the RecipeHeader table.
	/// </summary>
	[Serializable]
	public partial class RecipeHeader : ActiveRecord<RecipeHeader>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public RecipeHeader()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public RecipeHeader(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public RecipeHeader(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public RecipeHeader(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("RecipeHeader", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarRecipeHeaderID = new TableSchema.TableColumn(schema);
				colvarRecipeHeaderID.ColumnName = "RecipeHeaderID";
				colvarRecipeHeaderID.DataType = DbType.AnsiString;
				colvarRecipeHeaderID.MaxLength = 64;
				colvarRecipeHeaderID.AutoIncrement = false;
				colvarRecipeHeaderID.IsNullable = false;
				colvarRecipeHeaderID.IsPrimaryKey = true;
				colvarRecipeHeaderID.IsForeignKey = false;
				colvarRecipeHeaderID.IsReadOnly = false;
				colvarRecipeHeaderID.DefaultSetting = @"";
				colvarRecipeHeaderID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarRecipeHeaderID);
				
				TableSchema.TableColumn colvarRecipeName = new TableSchema.TableColumn(schema);
				colvarRecipeName.ColumnName = "RecipeName";
				colvarRecipeName.DataType = DbType.AnsiString;
				colvarRecipeName.MaxLength = 150;
				colvarRecipeName.AutoIncrement = false;
				colvarRecipeName.IsNullable = false;
				colvarRecipeName.IsPrimaryKey = false;
				colvarRecipeName.IsForeignKey = false;
				colvarRecipeName.IsReadOnly = false;
				colvarRecipeName.DefaultSetting = @"";
				colvarRecipeName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarRecipeName);
				
				TableSchema.TableColumn colvarItemNo = new TableSchema.TableColumn(schema);
				colvarItemNo.ColumnName = "ItemNo";
				colvarItemNo.DataType = DbType.AnsiString;
				colvarItemNo.MaxLength = 50;
				colvarItemNo.AutoIncrement = false;
				colvarItemNo.IsNullable = false;
				colvarItemNo.IsPrimaryKey = false;
				colvarItemNo.IsForeignKey = false;
				colvarItemNo.IsReadOnly = false;
				colvarItemNo.DefaultSetting = @"";
				colvarItemNo.ForeignKeyTableName = "";
				schema.Columns.Add(colvarItemNo);
				
				TableSchema.TableColumn colvarCreatedBy = new TableSchema.TableColumn(schema);
				colvarCreatedBy.ColumnName = "CreatedBy";
				colvarCreatedBy.DataType = DbType.String;
				colvarCreatedBy.MaxLength = 200;
				colvarCreatedBy.AutoIncrement = false;
				colvarCreatedBy.IsNullable = true;
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
				colvarModifiedBy.DataType = DbType.String;
				colvarModifiedBy.MaxLength = 200;
				colvarModifiedBy.AutoIncrement = false;
				colvarModifiedBy.IsNullable = true;
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
				colvarModifiedOn.IsNullable = true;
				colvarModifiedOn.IsPrimaryKey = false;
				colvarModifiedOn.IsForeignKey = false;
				colvarModifiedOn.IsReadOnly = false;
				colvarModifiedOn.DefaultSetting = @"";
				colvarModifiedOn.ForeignKeyTableName = "";
				schema.Columns.Add(colvarModifiedOn);
				
				TableSchema.TableColumn colvarUserfld1 = new TableSchema.TableColumn(schema);
				colvarUserfld1.ColumnName = "userfld1";
				colvarUserfld1.DataType = DbType.AnsiString;
				colvarUserfld1.MaxLength = 50;
				colvarUserfld1.AutoIncrement = false;
				colvarUserfld1.IsNullable = true;
				colvarUserfld1.IsPrimaryKey = false;
				colvarUserfld1.IsForeignKey = false;
				colvarUserfld1.IsReadOnly = false;
				colvarUserfld1.DefaultSetting = @"";
				colvarUserfld1.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfld1);
				
				TableSchema.TableColumn colvarUserfld2 = new TableSchema.TableColumn(schema);
				colvarUserfld2.ColumnName = "userfld2";
				colvarUserfld2.DataType = DbType.AnsiString;
				colvarUserfld2.MaxLength = 50;
				colvarUserfld2.AutoIncrement = false;
				colvarUserfld2.IsNullable = true;
				colvarUserfld2.IsPrimaryKey = false;
				colvarUserfld2.IsForeignKey = false;
				colvarUserfld2.IsReadOnly = false;
				colvarUserfld2.DefaultSetting = @"";
				colvarUserfld2.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfld2);
				
				TableSchema.TableColumn colvarUserfld3 = new TableSchema.TableColumn(schema);
				colvarUserfld3.ColumnName = "userfld3";
				colvarUserfld3.DataType = DbType.AnsiString;
				colvarUserfld3.MaxLength = 50;
				colvarUserfld3.AutoIncrement = false;
				colvarUserfld3.IsNullable = true;
				colvarUserfld3.IsPrimaryKey = false;
				colvarUserfld3.IsForeignKey = false;
				colvarUserfld3.IsReadOnly = false;
				colvarUserfld3.DefaultSetting = @"";
				colvarUserfld3.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfld3);
				
				TableSchema.TableColumn colvarUserfld4 = new TableSchema.TableColumn(schema);
				colvarUserfld4.ColumnName = "userfld4";
				colvarUserfld4.DataType = DbType.AnsiString;
				colvarUserfld4.MaxLength = 50;
				colvarUserfld4.AutoIncrement = false;
				colvarUserfld4.IsNullable = true;
				colvarUserfld4.IsPrimaryKey = false;
				colvarUserfld4.IsForeignKey = false;
				colvarUserfld4.IsReadOnly = false;
				colvarUserfld4.DefaultSetting = @"";
				colvarUserfld4.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfld4);
				
				TableSchema.TableColumn colvarUserfld5 = new TableSchema.TableColumn(schema);
				colvarUserfld5.ColumnName = "userfld5";
				colvarUserfld5.DataType = DbType.AnsiString;
				colvarUserfld5.MaxLength = 50;
				colvarUserfld5.AutoIncrement = false;
				colvarUserfld5.IsNullable = true;
				colvarUserfld5.IsPrimaryKey = false;
				colvarUserfld5.IsForeignKey = false;
				colvarUserfld5.IsReadOnly = false;
				colvarUserfld5.DefaultSetting = @"";
				colvarUserfld5.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfld5);
				
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
				DataService.Providers["PowerPOS"].AddSchema("RecipeHeader",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("RecipeHeaderID")]
		[Bindable(true)]
		public string RecipeHeaderID 
		{
			get { return GetColumnValue<string>(Columns.RecipeHeaderID); }
			set { SetColumnValue(Columns.RecipeHeaderID, value); }
		}
		  
		[XmlAttribute("RecipeName")]
		[Bindable(true)]
		public string RecipeName 
		{
			get { return GetColumnValue<string>(Columns.RecipeName); }
			set { SetColumnValue(Columns.RecipeName, value); }
		}
		  
		[XmlAttribute("ItemNo")]
		[Bindable(true)]
		public string ItemNo 
		{
			get { return GetColumnValue<string>(Columns.ItemNo); }
			set { SetColumnValue(Columns.ItemNo, value); }
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
		public DateTime? ModifiedOn 
		{
			get { return GetColumnValue<DateTime?>(Columns.ModifiedOn); }
			set { SetColumnValue(Columns.ModifiedOn, value); }
		}
		  
		[XmlAttribute("Userfld1")]
		[Bindable(true)]
		public string Userfld1 
		{
			get { return GetColumnValue<string>(Columns.Userfld1); }
			set { SetColumnValue(Columns.Userfld1, value); }
		}
		  
		[XmlAttribute("Userfld2")]
		[Bindable(true)]
		public string Userfld2 
		{
			get { return GetColumnValue<string>(Columns.Userfld2); }
			set { SetColumnValue(Columns.Userfld2, value); }
		}
		  
		[XmlAttribute("Userfld3")]
		[Bindable(true)]
		public string Userfld3 
		{
			get { return GetColumnValue<string>(Columns.Userfld3); }
			set { SetColumnValue(Columns.Userfld3, value); }
		}
		  
		[XmlAttribute("Userfld4")]
		[Bindable(true)]
		public string Userfld4 
		{
			get { return GetColumnValue<string>(Columns.Userfld4); }
			set { SetColumnValue(Columns.Userfld4, value); }
		}
		  
		[XmlAttribute("Userfld5")]
		[Bindable(true)]
		public string Userfld5 
		{
			get { return GetColumnValue<string>(Columns.Userfld5); }
			set { SetColumnValue(Columns.Userfld5, value); }
		}
		  
		[XmlAttribute("Deleted")]
		[Bindable(true)]
		public bool? Deleted 
		{
			get { return GetColumnValue<bool?>(Columns.Deleted); }
			set { SetColumnValue(Columns.Deleted, value); }
		}
		
		#endregion
		
		
		#region PrimaryKey Methods		
		
        protected override void SetPrimaryKey(object oValue)
        {
            base.SetPrimaryKey(oValue);
            
            SetPKValues();
        }
        
		
		public PowerPOS.RecipeDetailCollection RecipeDetailRecords()
		{
			return new PowerPOS.RecipeDetailCollection().Where(RecipeDetail.Columns.RecipeHeaderID, RecipeHeaderID).Load();
		}
		#endregion
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(string varRecipeHeaderID,string varRecipeName,string varItemNo,string varCreatedBy,DateTime varCreatedOn,string varModifiedBy,DateTime? varModifiedOn,string varUserfld1,string varUserfld2,string varUserfld3,string varUserfld4,string varUserfld5,bool? varDeleted)
		{
			RecipeHeader item = new RecipeHeader();
			
			item.RecipeHeaderID = varRecipeHeaderID;
			
			item.RecipeName = varRecipeName;
			
			item.ItemNo = varItemNo;
			
			item.CreatedBy = varCreatedBy;
			
			item.CreatedOn = varCreatedOn;
			
			item.ModifiedBy = varModifiedBy;
			
			item.ModifiedOn = varModifiedOn;
			
			item.Userfld1 = varUserfld1;
			
			item.Userfld2 = varUserfld2;
			
			item.Userfld3 = varUserfld3;
			
			item.Userfld4 = varUserfld4;
			
			item.Userfld5 = varUserfld5;
			
			item.Deleted = varDeleted;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(string varRecipeHeaderID,string varRecipeName,string varItemNo,string varCreatedBy,DateTime varCreatedOn,string varModifiedBy,DateTime? varModifiedOn,string varUserfld1,string varUserfld2,string varUserfld3,string varUserfld4,string varUserfld5,bool? varDeleted)
		{
			RecipeHeader item = new RecipeHeader();
			
				item.RecipeHeaderID = varRecipeHeaderID;
			
				item.RecipeName = varRecipeName;
			
				item.ItemNo = varItemNo;
			
				item.CreatedBy = varCreatedBy;
			
				item.CreatedOn = varCreatedOn;
			
				item.ModifiedBy = varModifiedBy;
			
				item.ModifiedOn = varModifiedOn;
			
				item.Userfld1 = varUserfld1;
			
				item.Userfld2 = varUserfld2;
			
				item.Userfld3 = varUserfld3;
			
				item.Userfld4 = varUserfld4;
			
				item.Userfld5 = varUserfld5;
			
				item.Deleted = varDeleted;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn RecipeHeaderIDColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn RecipeNameColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn ItemNoColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedByColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedOnColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedByColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedOnColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld1Column
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld2Column
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld3Column
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld4Column
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld5Column
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        public static TableSchema.TableColumn DeletedColumn
        {
            get { return Schema.Columns[12]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string RecipeHeaderID = @"RecipeHeaderID";
			 public static string RecipeName = @"RecipeName";
			 public static string ItemNo = @"ItemNo";
			 public static string CreatedBy = @"CreatedBy";
			 public static string CreatedOn = @"CreatedOn";
			 public static string ModifiedBy = @"ModifiedBy";
			 public static string ModifiedOn = @"ModifiedOn";
			 public static string Userfld1 = @"userfld1";
			 public static string Userfld2 = @"userfld2";
			 public static string Userfld3 = @"userfld3";
			 public static string Userfld4 = @"userfld4";
			 public static string Userfld5 = @"userfld5";
			 public static string Deleted = @"Deleted";
						
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
