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
	/// Strongly-typed collection for the AlternateBarcode class.
	/// </summary>
    [Serializable]
	public partial class AlternateBarcodeCollection : ActiveList<AlternateBarcode, AlternateBarcodeCollection>
	{	   
		public AlternateBarcodeCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>AlternateBarcodeCollection</returns>
		public AlternateBarcodeCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                AlternateBarcode o = this[i];
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
	/// This is an ActiveRecord class which wraps the AlternateBarcode table.
	/// </summary>
	[Serializable]
	public partial class AlternateBarcode : ActiveRecord<AlternateBarcode>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public AlternateBarcode()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public AlternateBarcode(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public AlternateBarcode(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public AlternateBarcode(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("AlternateBarcode", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarBarcodeID = new TableSchema.TableColumn(schema);
				colvarBarcodeID.ColumnName = "BarcodeID";
				colvarBarcodeID.DataType = DbType.Int32;
				colvarBarcodeID.MaxLength = 0;
				colvarBarcodeID.AutoIncrement = true;
				colvarBarcodeID.IsNullable = false;
				colvarBarcodeID.IsPrimaryKey = true;
				colvarBarcodeID.IsForeignKey = false;
				colvarBarcodeID.IsReadOnly = false;
				colvarBarcodeID.DefaultSetting = @"";
				colvarBarcodeID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBarcodeID);
				
				TableSchema.TableColumn colvarBarcode = new TableSchema.TableColumn(schema);
				colvarBarcode.ColumnName = "Barcode";
				colvarBarcode.DataType = DbType.AnsiString;
				colvarBarcode.MaxLength = 50;
				colvarBarcode.AutoIncrement = false;
				colvarBarcode.IsNullable = false;
				colvarBarcode.IsPrimaryKey = false;
				colvarBarcode.IsForeignKey = false;
				colvarBarcode.IsReadOnly = false;
				colvarBarcode.DefaultSetting = @"";
				colvarBarcode.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBarcode);
				
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
				
				TableSchema.TableColumn colvarLastEditDate = new TableSchema.TableColumn(schema);
				colvarLastEditDate.ColumnName = "LastEditDate";
				colvarLastEditDate.DataType = DbType.DateTime;
				colvarLastEditDate.MaxLength = 0;
				colvarLastEditDate.AutoIncrement = false;
				colvarLastEditDate.IsNullable = true;
				colvarLastEditDate.IsPrimaryKey = false;
				colvarLastEditDate.IsForeignKey = false;
				colvarLastEditDate.IsReadOnly = false;
				
						colvarLastEditDate.DefaultSetting = @"(getutcdate())";
				colvarLastEditDate.ForeignKeyTableName = "";
				schema.Columns.Add(colvarLastEditDate);
				
				TableSchema.TableColumn colvarCreationDate = new TableSchema.TableColumn(schema);
				colvarCreationDate.ColumnName = "CreationDate";
				colvarCreationDate.DataType = DbType.DateTime;
				colvarCreationDate.MaxLength = 0;
				colvarCreationDate.AutoIncrement = false;
				colvarCreationDate.IsNullable = true;
				colvarCreationDate.IsPrimaryKey = false;
				colvarCreationDate.IsForeignKey = false;
				colvarCreationDate.IsReadOnly = false;
				
						colvarCreationDate.DefaultSetting = @"(getutcdate())";
				colvarCreationDate.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCreationDate);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["PowerPOS"].AddSchema("AlternateBarcode",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("BarcodeID")]
		[Bindable(true)]
		public int BarcodeID 
		{
			get { return GetColumnValue<int>(Columns.BarcodeID); }
			set { SetColumnValue(Columns.BarcodeID, value); }
		}
		  
		[XmlAttribute("Barcode")]
		[Bindable(true)]
		public string Barcode 
		{
			get { return GetColumnValue<string>(Columns.Barcode); }
			set { SetColumnValue(Columns.Barcode, value); }
		}
		  
		[XmlAttribute("ItemNo")]
		[Bindable(true)]
		public string ItemNo 
		{
			get { return GetColumnValue<string>(Columns.ItemNo); }
			set { SetColumnValue(Columns.ItemNo, value); }
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
		  
		[XmlAttribute("LastEditDate")]
		[Bindable(true)]
		public DateTime? LastEditDate 
		{
			get { return GetColumnValue<DateTime?>(Columns.LastEditDate); }
			set { SetColumnValue(Columns.LastEditDate, value); }
		}
		  
		[XmlAttribute("CreationDate")]
		[Bindable(true)]
		public DateTime? CreationDate 
		{
			get { return GetColumnValue<DateTime?>(Columns.CreationDate); }
			set { SetColumnValue(Columns.CreationDate, value); }
		}
		
		#endregion
		
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(string varBarcode,string varItemNo,DateTime? varCreatedOn,string varCreatedBy,DateTime? varModifiedOn,string varModifiedBy,bool? varDeleted,DateTime? varLastEditDate,DateTime? varCreationDate)
		{
			AlternateBarcode item = new AlternateBarcode();
			
			item.Barcode = varBarcode;
			
			item.ItemNo = varItemNo;
			
			item.CreatedOn = varCreatedOn;
			
			item.CreatedBy = varCreatedBy;
			
			item.ModifiedOn = varModifiedOn;
			
			item.ModifiedBy = varModifiedBy;
			
			item.Deleted = varDeleted;
			
			item.LastEditDate = varLastEditDate;
			
			item.CreationDate = varCreationDate;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varBarcodeID,string varBarcode,string varItemNo,DateTime? varCreatedOn,string varCreatedBy,DateTime? varModifiedOn,string varModifiedBy,bool? varDeleted,DateTime? varLastEditDate,DateTime? varCreationDate)
		{
			AlternateBarcode item = new AlternateBarcode();
			
				item.BarcodeID = varBarcodeID;
			
				item.Barcode = varBarcode;
			
				item.ItemNo = varItemNo;
			
				item.CreatedOn = varCreatedOn;
			
				item.CreatedBy = varCreatedBy;
			
				item.ModifiedOn = varModifiedOn;
			
				item.ModifiedBy = varModifiedBy;
			
				item.Deleted = varDeleted;
			
				item.LastEditDate = varLastEditDate;
			
				item.CreationDate = varCreationDate;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn BarcodeIDColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn BarcodeColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn ItemNoColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedOnColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedByColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedOnColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedByColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn DeletedColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn LastEditDateColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn CreationDateColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string BarcodeID = @"BarcodeID";
			 public static string Barcode = @"Barcode";
			 public static string ItemNo = @"ItemNo";
			 public static string CreatedOn = @"CreatedOn";
			 public static string CreatedBy = @"CreatedBy";
			 public static string ModifiedOn = @"ModifiedOn";
			 public static string ModifiedBy = @"ModifiedBy";
			 public static string Deleted = @"Deleted";
			 public static string LastEditDate = @"LastEditDate";
			 public static string CreationDate = @"CreationDate";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
