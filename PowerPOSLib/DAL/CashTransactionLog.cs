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
	/// Strongly-typed collection for the CashTransactionLog class.
	/// </summary>
    [Serializable]
	public partial class CashTransactionLogCollection : ActiveList<CashTransactionLog, CashTransactionLogCollection>
	{	   
		public CashTransactionLogCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>CashTransactionLogCollection</returns>
		public CashTransactionLogCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                CashTransactionLog o = this[i];
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
	/// This is an ActiveRecord class which wraps the CashTransactionLog table.
	/// </summary>
	[Serializable]
	public partial class CashTransactionLog : ActiveRecord<CashTransactionLog>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public CashTransactionLog()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public CashTransactionLog(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public CashTransactionLog(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public CashTransactionLog(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("CashTransactionLog", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarId = new TableSchema.TableColumn(schema);
				colvarId.ColumnName = "ID";
				colvarId.DataType = DbType.Int32;
				colvarId.MaxLength = 0;
				colvarId.AutoIncrement = true;
				colvarId.IsNullable = false;
				colvarId.IsPrimaryKey = true;
				colvarId.IsForeignKey = false;
				colvarId.IsReadOnly = false;
				colvarId.DefaultSetting = @"";
				colvarId.ForeignKeyTableName = "";
				schema.Columns.Add(colvarId);
				
				TableSchema.TableColumn colvarTransactionID = new TableSchema.TableColumn(schema);
				colvarTransactionID.ColumnName = "TransactionID";
				colvarTransactionID.DataType = DbType.AnsiString;
				colvarTransactionID.MaxLength = 20;
				colvarTransactionID.AutoIncrement = false;
				colvarTransactionID.IsNullable = false;
				colvarTransactionID.IsPrimaryKey = false;
				colvarTransactionID.IsForeignKey = false;
				colvarTransactionID.IsReadOnly = false;
				colvarTransactionID.DefaultSetting = @"";
				colvarTransactionID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTransactionID);
				
				TableSchema.TableColumn colvarTransactionType = new TableSchema.TableColumn(schema);
				colvarTransactionType.ColumnName = "TransactionType";
				colvarTransactionType.DataType = DbType.AnsiString;
				colvarTransactionType.MaxLength = 20;
				colvarTransactionType.AutoIncrement = false;
				colvarTransactionType.IsNullable = false;
				colvarTransactionType.IsPrimaryKey = false;
				colvarTransactionType.IsForeignKey = false;
				colvarTransactionType.IsReadOnly = false;
				colvarTransactionType.DefaultSetting = @"";
				colvarTransactionType.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTransactionType);
				
				TableSchema.TableColumn colvarCurrency = new TableSchema.TableColumn(schema);
				colvarCurrency.ColumnName = "Currency";
				colvarCurrency.DataType = DbType.AnsiString;
				colvarCurrency.MaxLength = 20;
				colvarCurrency.AutoIncrement = false;
				colvarCurrency.IsNullable = false;
				colvarCurrency.IsPrimaryKey = false;
				colvarCurrency.IsForeignKey = false;
				colvarCurrency.IsReadOnly = false;
				colvarCurrency.DefaultSetting = @"";
				colvarCurrency.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCurrency);
				
				TableSchema.TableColumn colvarDenomination = new TableSchema.TableColumn(schema);
				colvarDenomination.ColumnName = "Denomination";
				colvarDenomination.DataType = DbType.Int32;
				colvarDenomination.MaxLength = 0;
				colvarDenomination.AutoIncrement = false;
				colvarDenomination.IsNullable = false;
				colvarDenomination.IsPrimaryKey = false;
				colvarDenomination.IsForeignKey = false;
				colvarDenomination.IsReadOnly = false;
				colvarDenomination.DefaultSetting = @"";
				colvarDenomination.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDenomination);
				
				TableSchema.TableColumn colvarQuantity = new TableSchema.TableColumn(schema);
				colvarQuantity.ColumnName = "Quantity";
				colvarQuantity.DataType = DbType.Int32;
				colvarQuantity.MaxLength = 0;
				colvarQuantity.AutoIncrement = false;
				colvarQuantity.IsNullable = false;
				colvarQuantity.IsPrimaryKey = false;
				colvarQuantity.IsForeignKey = false;
				colvarQuantity.IsReadOnly = false;
				colvarQuantity.DefaultSetting = @"";
				colvarQuantity.ForeignKeyTableName = "";
				schema.Columns.Add(colvarQuantity);
				
				TableSchema.TableColumn colvarAmount = new TableSchema.TableColumn(schema);
				colvarAmount.ColumnName = "Amount";
				colvarAmount.DataType = DbType.Decimal;
				colvarAmount.MaxLength = 0;
				colvarAmount.AutoIncrement = false;
				colvarAmount.IsNullable = true;
				colvarAmount.IsPrimaryKey = false;
				colvarAmount.IsForeignKey = false;
				colvarAmount.IsReadOnly = false;
				colvarAmount.DefaultSetting = @"";
				colvarAmount.ForeignKeyTableName = "";
				schema.Columns.Add(colvarAmount);
				
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
				DataService.Providers["PowerPOS"].AddSchema("CashTransactionLog",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("Id")]
		[Bindable(true)]
		public int Id 
		{
			get { return GetColumnValue<int>(Columns.Id); }
			set { SetColumnValue(Columns.Id, value); }
		}
		  
		[XmlAttribute("TransactionID")]
		[Bindable(true)]
		public string TransactionID 
		{
			get { return GetColumnValue<string>(Columns.TransactionID); }
			set { SetColumnValue(Columns.TransactionID, value); }
		}
		  
		[XmlAttribute("TransactionType")]
		[Bindable(true)]
		public string TransactionType 
		{
			get { return GetColumnValue<string>(Columns.TransactionType); }
			set { SetColumnValue(Columns.TransactionType, value); }
		}
		  
		[XmlAttribute("Currency")]
		[Bindable(true)]
		public string Currency 
		{
			get { return GetColumnValue<string>(Columns.Currency); }
			set { SetColumnValue(Columns.Currency, value); }
		}
		  
		[XmlAttribute("Denomination")]
		[Bindable(true)]
		public int Denomination 
		{
			get { return GetColumnValue<int>(Columns.Denomination); }
			set { SetColumnValue(Columns.Denomination, value); }
		}
		  
		[XmlAttribute("Quantity")]
		[Bindable(true)]
		public int Quantity 
		{
			get { return GetColumnValue<int>(Columns.Quantity); }
			set { SetColumnValue(Columns.Quantity, value); }
		}
		  
		[XmlAttribute("Amount")]
		[Bindable(true)]
		public decimal? Amount 
		{
			get { return GetColumnValue<decimal?>(Columns.Amount); }
			set { SetColumnValue(Columns.Amount, value); }
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
		public static void Insert(string varTransactionID,string varTransactionType,string varCurrency,int varDenomination,int varQuantity,decimal? varAmount,bool? varDeleted,DateTime? varCreatedOn,string varCreatedBy,DateTime? varModifiedOn,string varModifiedBy)
		{
			CashTransactionLog item = new CashTransactionLog();
			
			item.TransactionID = varTransactionID;
			
			item.TransactionType = varTransactionType;
			
			item.Currency = varCurrency;
			
			item.Denomination = varDenomination;
			
			item.Quantity = varQuantity;
			
			item.Amount = varAmount;
			
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
		public static void Update(int varId,string varTransactionID,string varTransactionType,string varCurrency,int varDenomination,int varQuantity,decimal? varAmount,bool? varDeleted,DateTime? varCreatedOn,string varCreatedBy,DateTime? varModifiedOn,string varModifiedBy)
		{
			CashTransactionLog item = new CashTransactionLog();
			
				item.Id = varId;
			
				item.TransactionID = varTransactionID;
			
				item.TransactionType = varTransactionType;
			
				item.Currency = varCurrency;
			
				item.Denomination = varDenomination;
			
				item.Quantity = varQuantity;
			
				item.Amount = varAmount;
			
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
        
        
        public static TableSchema.TableColumn IdColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn TransactionIDColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn TransactionTypeColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn CurrencyColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn DenominationColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn QuantityColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn AmountColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn DeletedColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedOnColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedByColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedOnColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedByColumn
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"ID";
			 public static string TransactionID = @"TransactionID";
			 public static string TransactionType = @"TransactionType";
			 public static string Currency = @"Currency";
			 public static string Denomination = @"Denomination";
			 public static string Quantity = @"Quantity";
			 public static string Amount = @"Amount";
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
