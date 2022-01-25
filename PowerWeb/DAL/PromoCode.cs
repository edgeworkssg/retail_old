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
	/// Strongly-typed collection for the PromoCode class.
	/// </summary>
    [Serializable]
	public partial class PromoCodeCollection : ActiveList<PromoCode, PromoCodeCollection>
	{	   
		public PromoCodeCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>PromoCodeCollection</returns>
		public PromoCodeCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                PromoCode o = this[i];
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
	/// This is an ActiveRecord class which wraps the PromoCode table.
	/// </summary>
	[Serializable]
	public partial class PromoCode : ActiveRecord<PromoCode>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public PromoCode()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public PromoCode(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public PromoCode(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public PromoCode(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("PromoCode", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarPromoCodeID = new TableSchema.TableColumn(schema);
				colvarPromoCodeID.ColumnName = "PromoCodeID";
				colvarPromoCodeID.DataType = DbType.Int32;
				colvarPromoCodeID.MaxLength = 0;
				colvarPromoCodeID.AutoIncrement = true;
				colvarPromoCodeID.IsNullable = false;
				colvarPromoCodeID.IsPrimaryKey = true;
				colvarPromoCodeID.IsForeignKey = false;
				colvarPromoCodeID.IsReadOnly = false;
				colvarPromoCodeID.DefaultSetting = @"";
				colvarPromoCodeID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPromoCodeID);
				
				TableSchema.TableColumn colvarPromoCodeName = new TableSchema.TableColumn(schema);
				colvarPromoCodeName.ColumnName = "PromoCodeName";
				colvarPromoCodeName.DataType = DbType.String;
				colvarPromoCodeName.MaxLength = -1;
				colvarPromoCodeName.AutoIncrement = false;
				colvarPromoCodeName.IsNullable = false;
				colvarPromoCodeName.IsPrimaryKey = false;
				colvarPromoCodeName.IsForeignKey = false;
				colvarPromoCodeName.IsReadOnly = false;
				colvarPromoCodeName.DefaultSetting = @"";
				colvarPromoCodeName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPromoCodeName);
				
				TableSchema.TableColumn colvarValidStartDate = new TableSchema.TableColumn(schema);
				colvarValidStartDate.ColumnName = "ValidStartDate";
				colvarValidStartDate.DataType = DbType.DateTime;
				colvarValidStartDate.MaxLength = 0;
				colvarValidStartDate.AutoIncrement = false;
				colvarValidStartDate.IsNullable = true;
				colvarValidStartDate.IsPrimaryKey = false;
				colvarValidStartDate.IsForeignKey = false;
				colvarValidStartDate.IsReadOnly = false;
				colvarValidStartDate.DefaultSetting = @"";
				colvarValidStartDate.ForeignKeyTableName = "";
				schema.Columns.Add(colvarValidStartDate);
				
				TableSchema.TableColumn colvarValidEndDate = new TableSchema.TableColumn(schema);
				colvarValidEndDate.ColumnName = "ValidEndDate";
				colvarValidEndDate.DataType = DbType.DateTime;
				colvarValidEndDate.MaxLength = 0;
				colvarValidEndDate.AutoIncrement = false;
				colvarValidEndDate.IsNullable = true;
				colvarValidEndDate.IsPrimaryKey = false;
				colvarValidEndDate.IsForeignKey = false;
				colvarValidEndDate.IsReadOnly = false;
				colvarValidEndDate.DefaultSetting = @"";
				colvarValidEndDate.ForeignKeyTableName = "";
				schema.Columns.Add(colvarValidEndDate);
				
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
				DataService.Providers["PowerPOS"].AddSchema("PromoCode",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("PromoCodeID")]
		[Bindable(true)]
		public int PromoCodeID 
		{
			get { return GetColumnValue<int>(Columns.PromoCodeID); }
			set { SetColumnValue(Columns.PromoCodeID, value); }
		}
		  
		[XmlAttribute("PromoCodeName")]
		[Bindable(true)]
		public string PromoCodeName 
		{
			get { return GetColumnValue<string>(Columns.PromoCodeName); }
			set { SetColumnValue(Columns.PromoCodeName, value); }
		}
		  
		[XmlAttribute("ValidStartDate")]
		[Bindable(true)]
		public DateTime? ValidStartDate 
		{
			get { return GetColumnValue<DateTime?>(Columns.ValidStartDate); }
			set { SetColumnValue(Columns.ValidStartDate, value); }
		}
		  
		[XmlAttribute("ValidEndDate")]
		[Bindable(true)]
		public DateTime? ValidEndDate 
		{
			get { return GetColumnValue<DateTime?>(Columns.ValidEndDate); }
			set { SetColumnValue(Columns.ValidEndDate, value); }
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
		
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(string varPromoCodeName,DateTime? varValidStartDate,DateTime? varValidEndDate,bool? varDeleted,DateTime? varCreatedOn,string varCreatedBy,DateTime? varModifiedOn,string varModifiedBy)
		{
			PromoCode item = new PromoCode();
			
			item.PromoCodeName = varPromoCodeName;
			
			item.ValidStartDate = varValidStartDate;
			
			item.ValidEndDate = varValidEndDate;
			
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
		public static void Update(int varPromoCodeID,string varPromoCodeName,DateTime? varValidStartDate,DateTime? varValidEndDate,bool? varDeleted,DateTime? varCreatedOn,string varCreatedBy,DateTime? varModifiedOn,string varModifiedBy)
		{
			PromoCode item = new PromoCode();
			
				item.PromoCodeID = varPromoCodeID;
			
				item.PromoCodeName = varPromoCodeName;
			
				item.ValidStartDate = varValidStartDate;
			
				item.ValidEndDate = varValidEndDate;
			
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
        
        
        public static TableSchema.TableColumn PromoCodeIDColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn PromoCodeNameColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn ValidStartDateColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn ValidEndDateColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn DeletedColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedOnColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedByColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedOnColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedByColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string PromoCodeID = @"PromoCodeID";
			 public static string PromoCodeName = @"PromoCodeName";
			 public static string ValidStartDate = @"ValidStartDate";
			 public static string ValidEndDate = @"ValidEndDate";
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
