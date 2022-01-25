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
	/// Strongly-typed collection for the EZLinkUnCfmLog class.
	/// </summary>
    [Serializable]
	public partial class EZLinkUnCfmLogCollection : ActiveList<EZLinkUnCfmLog, EZLinkUnCfmLogCollection>
	{	   
		public EZLinkUnCfmLogCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>EZLinkUnCfmLogCollection</returns>
		public EZLinkUnCfmLogCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                EZLinkUnCfmLog o = this[i];
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
	/// This is an ActiveRecord class which wraps the EZLinkUnCfmLog table.
	/// </summary>
	[Serializable]
	public partial class EZLinkUnCfmLog : ActiveRecord<EZLinkUnCfmLog>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public EZLinkUnCfmLog()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public EZLinkUnCfmLog(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public EZLinkUnCfmLog(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public EZLinkUnCfmLog(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("EZLinkUnCfmLog", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarRecordID = new TableSchema.TableColumn(schema);
				colvarRecordID.ColumnName = "RecordID";
				colvarRecordID.DataType = DbType.Int32;
				colvarRecordID.MaxLength = 0;
				colvarRecordID.AutoIncrement = true;
				colvarRecordID.IsNullable = false;
				colvarRecordID.IsPrimaryKey = true;
				colvarRecordID.IsForeignKey = false;
				colvarRecordID.IsReadOnly = false;
				colvarRecordID.DefaultSetting = @"";
				colvarRecordID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarRecordID);
				
				TableSchema.TableColumn colvarCardID = new TableSchema.TableColumn(schema);
				colvarCardID.ColumnName = "CardID";
				colvarCardID.DataType = DbType.AnsiString;
				colvarCardID.MaxLength = 50;
				colvarCardID.AutoIncrement = false;
				colvarCardID.IsNullable = false;
				colvarCardID.IsPrimaryKey = false;
				colvarCardID.IsForeignKey = false;
				colvarCardID.IsReadOnly = false;
				colvarCardID.DefaultSetting = @"";
				colvarCardID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCardID);
				
				TableSchema.TableColumn colvarOrderDate = new TableSchema.TableColumn(schema);
				colvarOrderDate.ColumnName = "OrderDate";
				colvarOrderDate.DataType = DbType.AnsiString;
				colvarOrderDate.MaxLength = 14;
				colvarOrderDate.AutoIncrement = false;
				colvarOrderDate.IsNullable = false;
				colvarOrderDate.IsPrimaryKey = false;
				colvarOrderDate.IsForeignKey = false;
				colvarOrderDate.IsReadOnly = false;
				colvarOrderDate.DefaultSetting = @"";
				colvarOrderDate.ForeignKeyTableName = "";
				schema.Columns.Add(colvarOrderDate);
				
				TableSchema.TableColumn colvarUnConfirmAmt = new TableSchema.TableColumn(schema);
				colvarUnConfirmAmt.ColumnName = "UnConfirmAmt";
				colvarUnConfirmAmt.DataType = DbType.Currency;
				colvarUnConfirmAmt.MaxLength = 0;
				colvarUnConfirmAmt.AutoIncrement = false;
				colvarUnConfirmAmt.IsNullable = false;
				colvarUnConfirmAmt.IsPrimaryKey = false;
				colvarUnConfirmAmt.IsForeignKey = false;
				colvarUnConfirmAmt.IsReadOnly = false;
				colvarUnConfirmAmt.DefaultSetting = @"";
				colvarUnConfirmAmt.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUnConfirmAmt);
				
				TableSchema.TableColumn colvarReceiptNo = new TableSchema.TableColumn(schema);
				colvarReceiptNo.ColumnName = "ReceiptNo";
				colvarReceiptNo.DataType = DbType.AnsiString;
				colvarReceiptNo.MaxLength = 50;
				colvarReceiptNo.AutoIncrement = false;
				colvarReceiptNo.IsNullable = true;
				colvarReceiptNo.IsPrimaryKey = false;
				colvarReceiptNo.IsForeignKey = false;
				colvarReceiptNo.IsReadOnly = false;
				colvarReceiptNo.DefaultSetting = @"";
				colvarReceiptNo.ForeignKeyTableName = "";
				schema.Columns.Add(colvarReceiptNo);
				
				TableSchema.TableColumn colvarDeleted = new TableSchema.TableColumn(schema);
				colvarDeleted.ColumnName = "Deleted";
				colvarDeleted.DataType = DbType.Boolean;
				colvarDeleted.MaxLength = 0;
				colvarDeleted.AutoIncrement = false;
				colvarDeleted.IsNullable = false;
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
				
				TableSchema.TableColumn colvarUniqueID = new TableSchema.TableColumn(schema);
				colvarUniqueID.ColumnName = "UniqueID";
				colvarUniqueID.DataType = DbType.Guid;
				colvarUniqueID.MaxLength = 0;
				colvarUniqueID.AutoIncrement = false;
				colvarUniqueID.IsNullable = false;
				colvarUniqueID.IsPrimaryKey = false;
				colvarUniqueID.IsForeignKey = false;
				colvarUniqueID.IsReadOnly = false;
				
						colvarUniqueID.DefaultSetting = @"(newid())";
				colvarUniqueID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUniqueID);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["PowerPOS"].AddSchema("EZLinkUnCfmLog",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("RecordID")]
		[Bindable(true)]
		public int RecordID 
		{
			get { return GetColumnValue<int>(Columns.RecordID); }
			set { SetColumnValue(Columns.RecordID, value); }
		}
		  
		[XmlAttribute("CardID")]
		[Bindable(true)]
		public string CardID 
		{
			get { return GetColumnValue<string>(Columns.CardID); }
			set { SetColumnValue(Columns.CardID, value); }
		}
		  
		[XmlAttribute("OrderDate")]
		[Bindable(true)]
		public string OrderDate 
		{
			get { return GetColumnValue<string>(Columns.OrderDate); }
			set { SetColumnValue(Columns.OrderDate, value); }
		}
		  
		[XmlAttribute("UnConfirmAmt")]
		[Bindable(true)]
		public decimal UnConfirmAmt 
		{
			get { return GetColumnValue<decimal>(Columns.UnConfirmAmt); }
			set { SetColumnValue(Columns.UnConfirmAmt, value); }
		}
		  
		[XmlAttribute("ReceiptNo")]
		[Bindable(true)]
		public string ReceiptNo 
		{
			get { return GetColumnValue<string>(Columns.ReceiptNo); }
			set { SetColumnValue(Columns.ReceiptNo, value); }
		}
		  
		[XmlAttribute("Deleted")]
		[Bindable(true)]
		public bool Deleted 
		{
			get { return GetColumnValue<bool>(Columns.Deleted); }
			set { SetColumnValue(Columns.Deleted, value); }
		}
		  
		[XmlAttribute("CreatedOn")]
		[Bindable(true)]
		public DateTime? CreatedOn 
		{
			get { return GetColumnValue<DateTime?>(Columns.CreatedOn); }
			set { SetColumnValue(Columns.CreatedOn, value); }
		}
		  
		[XmlAttribute("ModifiedOn")]
		[Bindable(true)]
		public DateTime? ModifiedOn 
		{
			get { return GetColumnValue<DateTime?>(Columns.ModifiedOn); }
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
		  
		[XmlAttribute("UniqueID")]
		[Bindable(true)]
		public Guid UniqueID 
		{
			get { return GetColumnValue<Guid>(Columns.UniqueID); }
			set { SetColumnValue(Columns.UniqueID, value); }
		}
		
		#endregion
		
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(string varCardID,string varOrderDate,decimal varUnConfirmAmt,string varReceiptNo,bool varDeleted,DateTime? varCreatedOn,DateTime? varModifiedOn,string varCreatedBy,string varModifiedBy,Guid varUniqueID)
		{
			EZLinkUnCfmLog item = new EZLinkUnCfmLog();
			
			item.CardID = varCardID;
			
			item.OrderDate = varOrderDate;
			
			item.UnConfirmAmt = varUnConfirmAmt;
			
			item.ReceiptNo = varReceiptNo;
			
			item.Deleted = varDeleted;
			
			item.CreatedOn = varCreatedOn;
			
			item.ModifiedOn = varModifiedOn;
			
			item.CreatedBy = varCreatedBy;
			
			item.ModifiedBy = varModifiedBy;
			
			item.UniqueID = varUniqueID;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varRecordID,string varCardID,string varOrderDate,decimal varUnConfirmAmt,string varReceiptNo,bool varDeleted,DateTime? varCreatedOn,DateTime? varModifiedOn,string varCreatedBy,string varModifiedBy,Guid varUniqueID)
		{
			EZLinkUnCfmLog item = new EZLinkUnCfmLog();
			
				item.RecordID = varRecordID;
			
				item.CardID = varCardID;
			
				item.OrderDate = varOrderDate;
			
				item.UnConfirmAmt = varUnConfirmAmt;
			
				item.ReceiptNo = varReceiptNo;
			
				item.Deleted = varDeleted;
			
				item.CreatedOn = varCreatedOn;
			
				item.ModifiedOn = varModifiedOn;
			
				item.CreatedBy = varCreatedBy;
			
				item.ModifiedBy = varModifiedBy;
			
				item.UniqueID = varUniqueID;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn RecordIDColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn CardIDColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn OrderDateColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn UnConfirmAmtColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn ReceiptNoColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn DeletedColumn
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
        
        
        
        public static TableSchema.TableColumn UniqueIDColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string RecordID = @"RecordID";
			 public static string CardID = @"CardID";
			 public static string OrderDate = @"OrderDate";
			 public static string UnConfirmAmt = @"UnConfirmAmt";
			 public static string ReceiptNo = @"ReceiptNo";
			 public static string Deleted = @"Deleted";
			 public static string CreatedOn = @"CreatedOn";
			 public static string ModifiedOn = @"ModifiedOn";
			 public static string CreatedBy = @"CreatedBy";
			 public static string ModifiedBy = @"ModifiedBy";
			 public static string UniqueID = @"UniqueID";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
