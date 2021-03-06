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
// <auto-generated />
namespace PowerPOS
{
	/// <summary>
	/// Strongly-typed collection for the Attribute class.
	/// </summary>
    [Serializable]
	public partial class AttributeCollection : ActiveList<Attribute, AttributeCollection>
	{	   
		public AttributeCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>AttributeCollection</returns>
		public AttributeCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                Attribute o = this[i];
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
	/// This is an ActiveRecord class which wraps the Attributes table.
	/// </summary>
	[Serializable]
	public partial class Attribute : ActiveRecord<Attribute>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public Attribute()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public Attribute(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public Attribute(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public Attribute(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("Attributes", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarAttributesCode = new TableSchema.TableColumn(schema);
				colvarAttributesCode.ColumnName = "AttributesCode";
				colvarAttributesCode.DataType = DbType.AnsiString;
				colvarAttributesCode.MaxLength = 10;
				colvarAttributesCode.AutoIncrement = false;
				colvarAttributesCode.IsNullable = false;
				colvarAttributesCode.IsPrimaryKey = true;
				colvarAttributesCode.IsForeignKey = false;
				colvarAttributesCode.IsReadOnly = false;
				colvarAttributesCode.DefaultSetting = @"";
				colvarAttributesCode.ForeignKeyTableName = "";
				schema.Columns.Add(colvarAttributesCode);
				
				TableSchema.TableColumn colvarAttributesName = new TableSchema.TableColumn(schema);
				colvarAttributesName.ColumnName = "AttributesName";
				colvarAttributesName.DataType = DbType.String;
				colvarAttributesName.MaxLength = 250;
				colvarAttributesName.AutoIncrement = false;
				colvarAttributesName.IsNullable = false;
				colvarAttributesName.IsPrimaryKey = false;
				colvarAttributesName.IsForeignKey = false;
				colvarAttributesName.IsReadOnly = false;
				colvarAttributesName.DefaultSetting = @"";
				colvarAttributesName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarAttributesName);
				
				TableSchema.TableColumn colvarAttributesGroupCode = new TableSchema.TableColumn(schema);
				colvarAttributesGroupCode.ColumnName = "AttributesGroupCode";
				colvarAttributesGroupCode.DataType = DbType.AnsiString;
				colvarAttributesGroupCode.MaxLength = 10;
				colvarAttributesGroupCode.AutoIncrement = false;
				colvarAttributesGroupCode.IsNullable = false;
				colvarAttributesGroupCode.IsPrimaryKey = false;
				colvarAttributesGroupCode.IsForeignKey = true;
				colvarAttributesGroupCode.IsReadOnly = false;
				colvarAttributesGroupCode.DefaultSetting = @"";
				
					colvarAttributesGroupCode.ForeignKeyTableName = "AttributesGroup";
				schema.Columns.Add(colvarAttributesGroupCode);
				
				TableSchema.TableColumn colvarCalType = new TableSchema.TableColumn(schema);
				colvarCalType.ColumnName = "CalType";
				colvarCalType.DataType = DbType.AnsiString;
				colvarCalType.MaxLength = 50;
				colvarCalType.AutoIncrement = false;
				colvarCalType.IsNullable = false;
				colvarCalType.IsPrimaryKey = false;
				colvarCalType.IsForeignKey = false;
				colvarCalType.IsReadOnly = false;
				colvarCalType.DefaultSetting = @"";
				colvarCalType.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCalType);
				
				TableSchema.TableColumn colvarCalAmount = new TableSchema.TableColumn(schema);
				colvarCalAmount.ColumnName = "CalAmount";
				colvarCalAmount.DataType = DbType.Currency;
				colvarCalAmount.MaxLength = 0;
				colvarCalAmount.AutoIncrement = false;
				colvarCalAmount.IsNullable = false;
				colvarCalAmount.IsPrimaryKey = false;
				colvarCalAmount.IsForeignKey = false;
				colvarCalAmount.IsReadOnly = false;
				colvarCalAmount.DefaultSetting = @"";
				colvarCalAmount.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCalAmount);
				
				TableSchema.TableColumn colvarBillPrint = new TableSchema.TableColumn(schema);
				colvarBillPrint.ColumnName = "BillPrint";
				colvarBillPrint.DataType = DbType.Boolean;
				colvarBillPrint.MaxLength = 0;
				colvarBillPrint.AutoIncrement = false;
				colvarBillPrint.IsNullable = false;
				colvarBillPrint.IsPrimaryKey = false;
				colvarBillPrint.IsForeignKey = false;
				colvarBillPrint.IsReadOnly = false;
				colvarBillPrint.DefaultSetting = @"";
				colvarBillPrint.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBillPrint);
				
				TableSchema.TableColumn colvarUserField1 = new TableSchema.TableColumn(schema);
				colvarUserField1.ColumnName = "UserField1";
				colvarUserField1.DataType = DbType.String;
				colvarUserField1.MaxLength = 50;
				colvarUserField1.AutoIncrement = false;
				colvarUserField1.IsNullable = true;
				colvarUserField1.IsPrimaryKey = false;
				colvarUserField1.IsForeignKey = false;
				colvarUserField1.IsReadOnly = false;
				colvarUserField1.DefaultSetting = @"";
				colvarUserField1.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserField1);
				
				TableSchema.TableColumn colvarUserField2 = new TableSchema.TableColumn(schema);
				colvarUserField2.ColumnName = "UserField2";
				colvarUserField2.DataType = DbType.String;
				colvarUserField2.MaxLength = 50;
				colvarUserField2.AutoIncrement = false;
				colvarUserField2.IsNullable = true;
				colvarUserField2.IsPrimaryKey = false;
				colvarUserField2.IsForeignKey = false;
				colvarUserField2.IsReadOnly = false;
				colvarUserField2.DefaultSetting = @"";
				colvarUserField2.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserField2);
				
				TableSchema.TableColumn colvarUserField3 = new TableSchema.TableColumn(schema);
				colvarUserField3.ColumnName = "UserField3";
				colvarUserField3.DataType = DbType.String;
				colvarUserField3.MaxLength = 50;
				colvarUserField3.AutoIncrement = false;
				colvarUserField3.IsNullable = true;
				colvarUserField3.IsPrimaryKey = false;
				colvarUserField3.IsForeignKey = false;
				colvarUserField3.IsReadOnly = false;
				colvarUserField3.DefaultSetting = @"";
				colvarUserField3.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserField3);
				
				TableSchema.TableColumn colvarUserField4 = new TableSchema.TableColumn(schema);
				colvarUserField4.ColumnName = "UserField4";
				colvarUserField4.DataType = DbType.String;
				colvarUserField4.MaxLength = 50;
				colvarUserField4.AutoIncrement = false;
				colvarUserField4.IsNullable = true;
				colvarUserField4.IsPrimaryKey = false;
				colvarUserField4.IsForeignKey = false;
				colvarUserField4.IsReadOnly = false;
				colvarUserField4.DefaultSetting = @"";
				colvarUserField4.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserField4);
				
				TableSchema.TableColumn colvarUserField5 = new TableSchema.TableColumn(schema);
				colvarUserField5.ColumnName = "UserField5";
				colvarUserField5.DataType = DbType.String;
				colvarUserField5.MaxLength = 50;
				colvarUserField5.AutoIncrement = false;
				colvarUserField5.IsNullable = true;
				colvarUserField5.IsPrimaryKey = false;
				colvarUserField5.IsForeignKey = false;
				colvarUserField5.IsReadOnly = false;
				colvarUserField5.DefaultSetting = @"";
				colvarUserField5.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserField5);
				
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
				DataService.Providers["PowerPOS"].AddSchema("Attributes",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("AttributesCode")]
		[Bindable(true)]
		public string AttributesCode 
		{
			get { return GetColumnValue<string>(Columns.AttributesCode); }
			set { SetColumnValue(Columns.AttributesCode, value); }
		}
		  
		[XmlAttribute("AttributesName")]
		[Bindable(true)]
		public string AttributesName 
		{
			get { return GetColumnValue<string>(Columns.AttributesName); }
			set { SetColumnValue(Columns.AttributesName, value); }
		}
		  
		[XmlAttribute("AttributesGroupCode")]
		[Bindable(true)]
		public string AttributesGroupCode 
		{
			get { return GetColumnValue<string>(Columns.AttributesGroupCode); }
			set { SetColumnValue(Columns.AttributesGroupCode, value); }
		}
		  
		[XmlAttribute("CalType")]
		[Bindable(true)]
		public string CalType 
		{
			get { return GetColumnValue<string>(Columns.CalType); }
			set { SetColumnValue(Columns.CalType, value); }
		}
		  
		[XmlAttribute("CalAmount")]
		[Bindable(true)]
		public decimal CalAmount 
		{
			get { return GetColumnValue<decimal>(Columns.CalAmount); }
			set { SetColumnValue(Columns.CalAmount, value); }
		}
		  
		[XmlAttribute("BillPrint")]
		[Bindable(true)]
		public bool BillPrint 
		{
			get { return GetColumnValue<bool>(Columns.BillPrint); }
			set { SetColumnValue(Columns.BillPrint, value); }
		}
		  
		[XmlAttribute("UserField1")]
		[Bindable(true)]
		public string UserField1 
		{
			get { return GetColumnValue<string>(Columns.UserField1); }
			set { SetColumnValue(Columns.UserField1, value); }
		}
		  
		[XmlAttribute("UserField2")]
		[Bindable(true)]
		public string UserField2 
		{
			get { return GetColumnValue<string>(Columns.UserField2); }
			set { SetColumnValue(Columns.UserField2, value); }
		}
		  
		[XmlAttribute("UserField3")]
		[Bindable(true)]
		public string UserField3 
		{
			get { return GetColumnValue<string>(Columns.UserField3); }
			set { SetColumnValue(Columns.UserField3, value); }
		}
		  
		[XmlAttribute("UserField4")]
		[Bindable(true)]
		public string UserField4 
		{
			get { return GetColumnValue<string>(Columns.UserField4); }
			set { SetColumnValue(Columns.UserField4, value); }
		}
		  
		[XmlAttribute("UserField5")]
		[Bindable(true)]
		public string UserField5 
		{
			get { return GetColumnValue<string>(Columns.UserField5); }
			set { SetColumnValue(Columns.UserField5, value); }
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
        
		
		public PowerPOS.OrderDetAttributeCollection OrderDetAttributes()
		{
			return new PowerPOS.OrderDetAttributeCollection().Where(OrderDetAttribute.Columns.AttributesCode, AttributesCode).Load();
		}
		#endregion
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a AttributesGroup ActiveRecord object related to this Attribute
		/// 
		/// </summary>
		public PowerPOS.AttributesGroup AttributesGroup
		{
			get { return PowerPOS.AttributesGroup.FetchByID(this.AttributesGroupCode); }
			set { SetColumnValue("AttributesGroupCode", value.AttributesGroupCode); }
		}
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(string varAttributesCode,string varAttributesName,string varAttributesGroupCode,string varCalType,decimal varCalAmount,bool varBillPrint,string varUserField1,string varUserField2,string varUserField3,string varUserField4,string varUserField5,DateTime? varCreatedOn,string varCreatedBy,DateTime? varModifiedOn,string varModifiedBy,bool? varDeleted)
		{
			Attribute item = new Attribute();
			
			item.AttributesCode = varAttributesCode;
			
			item.AttributesName = varAttributesName;
			
			item.AttributesGroupCode = varAttributesGroupCode;
			
			item.CalType = varCalType;
			
			item.CalAmount = varCalAmount;
			
			item.BillPrint = varBillPrint;
			
			item.UserField1 = varUserField1;
			
			item.UserField2 = varUserField2;
			
			item.UserField3 = varUserField3;
			
			item.UserField4 = varUserField4;
			
			item.UserField5 = varUserField5;
			
			item.CreatedOn = varCreatedOn;
			
			item.CreatedBy = varCreatedBy;
			
			item.ModifiedOn = varModifiedOn;
			
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
		public static void Update(string varAttributesCode,string varAttributesName,string varAttributesGroupCode,string varCalType,decimal varCalAmount,bool varBillPrint,string varUserField1,string varUserField2,string varUserField3,string varUserField4,string varUserField5,DateTime? varCreatedOn,string varCreatedBy,DateTime? varModifiedOn,string varModifiedBy,bool? varDeleted)
		{
			Attribute item = new Attribute();
			
				item.AttributesCode = varAttributesCode;
			
				item.AttributesName = varAttributesName;
			
				item.AttributesGroupCode = varAttributesGroupCode;
			
				item.CalType = varCalType;
			
				item.CalAmount = varCalAmount;
			
				item.BillPrint = varBillPrint;
			
				item.UserField1 = varUserField1;
			
				item.UserField2 = varUserField2;
			
				item.UserField3 = varUserField3;
			
				item.UserField4 = varUserField4;
			
				item.UserField5 = varUserField5;
			
				item.CreatedOn = varCreatedOn;
			
				item.CreatedBy = varCreatedBy;
			
				item.ModifiedOn = varModifiedOn;
			
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
        
        
        public static TableSchema.TableColumn AttributesCodeColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn AttributesNameColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn AttributesGroupCodeColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn CalTypeColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn CalAmountColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn BillPrintColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn UserField1Column
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn UserField2Column
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn UserField3Column
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn UserField4Column
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn UserField5Column
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedOnColumn
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedByColumn
        {
            get { return Schema.Columns[12]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedOnColumn
        {
            get { return Schema.Columns[13]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedByColumn
        {
            get { return Schema.Columns[14]; }
        }
        
        
        
        public static TableSchema.TableColumn DeletedColumn
        {
            get { return Schema.Columns[15]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string AttributesCode = @"AttributesCode";
			 public static string AttributesName = @"AttributesName";
			 public static string AttributesGroupCode = @"AttributesGroupCode";
			 public static string CalType = @"CalType";
			 public static string CalAmount = @"CalAmount";
			 public static string BillPrint = @"BillPrint";
			 public static string UserField1 = @"UserField1";
			 public static string UserField2 = @"UserField2";
			 public static string UserField3 = @"UserField3";
			 public static string UserField4 = @"UserField4";
			 public static string UserField5 = @"UserField5";
			 public static string CreatedOn = @"CreatedOn";
			 public static string CreatedBy = @"CreatedBy";
			 public static string ModifiedOn = @"ModifiedOn";
			 public static string ModifiedBy = @"ModifiedBy";
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
