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
	/// Strongly-typed collection for the Voucher class.
	/// </summary>
    [Serializable]
	public partial class VoucherCollection : ActiveList<Voucher, VoucherCollection>
	{	   
		public VoucherCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>VoucherCollection</returns>
		public VoucherCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                Voucher o = this[i];
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
	/// This is an ActiveRecord class which wraps the Vouchers table.
	/// </summary>
	[Serializable]
	public partial class Voucher : ActiveRecord<Voucher>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public Voucher()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public Voucher(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public Voucher(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public Voucher(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("Vouchers", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarVoucherID = new TableSchema.TableColumn(schema);
				colvarVoucherID.ColumnName = "VoucherID";
				colvarVoucherID.DataType = DbType.Guid;
				colvarVoucherID.MaxLength = 0;
				colvarVoucherID.AutoIncrement = false;
				colvarVoucherID.IsNullable = false;
				colvarVoucherID.IsPrimaryKey = true;
				colvarVoucherID.IsForeignKey = false;
				colvarVoucherID.IsReadOnly = false;
				
						colvarVoucherID.DefaultSetting = @"(newid())";
				colvarVoucherID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarVoucherID);
				
				TableSchema.TableColumn colvarVoucherNo = new TableSchema.TableColumn(schema);
				colvarVoucherNo.ColumnName = "VoucherNo";
				colvarVoucherNo.DataType = DbType.AnsiString;
				colvarVoucherNo.MaxLength = 50;
				colvarVoucherNo.AutoIncrement = false;
				colvarVoucherNo.IsNullable = false;
				colvarVoucherNo.IsPrimaryKey = false;
				colvarVoucherNo.IsForeignKey = false;
				colvarVoucherNo.IsReadOnly = false;
				colvarVoucherNo.DefaultSetting = @"";
				colvarVoucherNo.ForeignKeyTableName = "";
				schema.Columns.Add(colvarVoucherNo);
				
				TableSchema.TableColumn colvarAmount = new TableSchema.TableColumn(schema);
				colvarAmount.ColumnName = "Amount";
				colvarAmount.DataType = DbType.Currency;
				colvarAmount.MaxLength = 0;
				colvarAmount.AutoIncrement = false;
				colvarAmount.IsNullable = false;
				colvarAmount.IsPrimaryKey = false;
				colvarAmount.IsForeignKey = false;
				colvarAmount.IsReadOnly = false;
				colvarAmount.DefaultSetting = @"";
				colvarAmount.ForeignKeyTableName = "";
				schema.Columns.Add(colvarAmount);
				
				TableSchema.TableColumn colvarRemark = new TableSchema.TableColumn(schema);
				colvarRemark.ColumnName = "Remark";
				colvarRemark.DataType = DbType.AnsiString;
				colvarRemark.MaxLength = 250;
				colvarRemark.AutoIncrement = false;
				colvarRemark.IsNullable = true;
				colvarRemark.IsPrimaryKey = false;
				colvarRemark.IsForeignKey = false;
				colvarRemark.IsReadOnly = false;
				colvarRemark.DefaultSetting = @"";
				colvarRemark.ForeignKeyTableName = "";
				schema.Columns.Add(colvarRemark);
				
				TableSchema.TableColumn colvarVoucherStatusId = new TableSchema.TableColumn(schema);
				colvarVoucherStatusId.ColumnName = "VoucherStatusId";
				colvarVoucherStatusId.DataType = DbType.Int32;
				colvarVoucherStatusId.MaxLength = 0;
				colvarVoucherStatusId.AutoIncrement = false;
				colvarVoucherStatusId.IsNullable = false;
				colvarVoucherStatusId.IsPrimaryKey = false;
				colvarVoucherStatusId.IsForeignKey = true;
				colvarVoucherStatusId.IsReadOnly = false;
				colvarVoucherStatusId.DefaultSetting = @"";
				
					colvarVoucherStatusId.ForeignKeyTableName = "VoucherStatus";
				schema.Columns.Add(colvarVoucherStatusId);
				
				TableSchema.TableColumn colvarDateIssued = new TableSchema.TableColumn(schema);
				colvarDateIssued.ColumnName = "DateIssued";
				colvarDateIssued.DataType = DbType.DateTime;
				colvarDateIssued.MaxLength = 0;
				colvarDateIssued.AutoIncrement = false;
				colvarDateIssued.IsNullable = true;
				colvarDateIssued.IsPrimaryKey = false;
				colvarDateIssued.IsForeignKey = false;
				colvarDateIssued.IsReadOnly = false;
				colvarDateIssued.DefaultSetting = @"";
				colvarDateIssued.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDateIssued);
				
				TableSchema.TableColumn colvarDateSold = new TableSchema.TableColumn(schema);
				colvarDateSold.ColumnName = "DateSold";
				colvarDateSold.DataType = DbType.DateTime;
				colvarDateSold.MaxLength = 0;
				colvarDateSold.AutoIncrement = false;
				colvarDateSold.IsNullable = true;
				colvarDateSold.IsPrimaryKey = false;
				colvarDateSold.IsForeignKey = false;
				colvarDateSold.IsReadOnly = false;
				colvarDateSold.DefaultSetting = @"";
				colvarDateSold.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDateSold);
				
				TableSchema.TableColumn colvarDateRedeemed = new TableSchema.TableColumn(schema);
				colvarDateRedeemed.ColumnName = "DateRedeemed";
				colvarDateRedeemed.DataType = DbType.DateTime;
				colvarDateRedeemed.MaxLength = 0;
				colvarDateRedeemed.AutoIncrement = false;
				colvarDateRedeemed.IsNullable = true;
				colvarDateRedeemed.IsPrimaryKey = false;
				colvarDateRedeemed.IsForeignKey = false;
				colvarDateRedeemed.IsReadOnly = false;
				colvarDateRedeemed.DefaultSetting = @"";
				colvarDateRedeemed.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDateRedeemed);
				
				TableSchema.TableColumn colvarExpiryDate = new TableSchema.TableColumn(schema);
				colvarExpiryDate.ColumnName = "ExpiryDate";
				colvarExpiryDate.DataType = DbType.DateTime;
				colvarExpiryDate.MaxLength = 0;
				colvarExpiryDate.AutoIncrement = false;
				colvarExpiryDate.IsNullable = true;
				colvarExpiryDate.IsPrimaryKey = false;
				colvarExpiryDate.IsForeignKey = false;
				colvarExpiryDate.IsReadOnly = false;
				colvarExpiryDate.DefaultSetting = @"";
				colvarExpiryDate.ForeignKeyTableName = "";
				schema.Columns.Add(colvarExpiryDate);
				
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
				colvarDeleted.IsNullable = false;
				colvarDeleted.IsPrimaryKey = false;
				colvarDeleted.IsForeignKey = false;
				colvarDeleted.IsReadOnly = false;
				
						colvarDeleted.DefaultSetting = @"((0))";
				colvarDeleted.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDeleted);
				
				TableSchema.TableColumn colvarRedeemAmount = new TableSchema.TableColumn(schema);
				colvarRedeemAmount.ColumnName = "RedeemAmount";
				colvarRedeemAmount.DataType = DbType.Currency;
				colvarRedeemAmount.MaxLength = 0;
				colvarRedeemAmount.AutoIncrement = false;
				colvarRedeemAmount.IsNullable = true;
				colvarRedeemAmount.IsPrimaryKey = false;
				colvarRedeemAmount.IsForeignKey = false;
				colvarRedeemAmount.IsReadOnly = false;
				colvarRedeemAmount.DefaultSetting = @"";
				colvarRedeemAmount.ForeignKeyTableName = "";
				schema.Columns.Add(colvarRedeemAmount);
				
				TableSchema.TableColumn colvarDateCanceled = new TableSchema.TableColumn(schema);
				colvarDateCanceled.ColumnName = "DateCanceled";
				colvarDateCanceled.DataType = DbType.DateTime;
				colvarDateCanceled.MaxLength = 0;
				colvarDateCanceled.AutoIncrement = false;
				colvarDateCanceled.IsNullable = true;
				colvarDateCanceled.IsPrimaryKey = false;
				colvarDateCanceled.IsForeignKey = false;
				colvarDateCanceled.IsReadOnly = false;
				colvarDateCanceled.DefaultSetting = @"";
				colvarDateCanceled.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDateCanceled);
				
				TableSchema.TableColumn colvarVoucherHeaderID = new TableSchema.TableColumn(schema);
				colvarVoucherHeaderID.ColumnName = "VoucherHeaderID";
				colvarVoucherHeaderID.DataType = DbType.Int32;
				colvarVoucherHeaderID.MaxLength = 0;
				colvarVoucherHeaderID.AutoIncrement = false;
				colvarVoucherHeaderID.IsNullable = true;
				colvarVoucherHeaderID.IsPrimaryKey = false;
				colvarVoucherHeaderID.IsForeignKey = false;
				colvarVoucherHeaderID.IsReadOnly = false;
				colvarVoucherHeaderID.DefaultSetting = @"";
				colvarVoucherHeaderID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarVoucherHeaderID);
				
				TableSchema.TableColumn colvarOutlet = new TableSchema.TableColumn(schema);
				colvarOutlet.ColumnName = "Outlet";
				colvarOutlet.DataType = DbType.AnsiString;
				colvarOutlet.MaxLength = -1;
				colvarOutlet.AutoIncrement = false;
				colvarOutlet.IsNullable = true;
				colvarOutlet.IsPrimaryKey = false;
				colvarOutlet.IsForeignKey = false;
				colvarOutlet.IsReadOnly = false;
				colvarOutlet.DefaultSetting = @"";
				colvarOutlet.ForeignKeyTableName = "";
				schema.Columns.Add(colvarOutlet);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["PowerPOS"].AddSchema("Vouchers",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("VoucherID")]
		[Bindable(true)]
		public Guid VoucherID 
		{
			get { return GetColumnValue<Guid>(Columns.VoucherID); }
			set { SetColumnValue(Columns.VoucherID, value); }
		}
		  
		[XmlAttribute("VoucherNo")]
		[Bindable(true)]
		public string VoucherNo 
		{
			get { return GetColumnValue<string>(Columns.VoucherNo); }
			set { SetColumnValue(Columns.VoucherNo, value); }
		}
		  
		[XmlAttribute("Amount")]
		[Bindable(true)]
		public decimal Amount 
		{
			get { return GetColumnValue<decimal>(Columns.Amount); }
			set { SetColumnValue(Columns.Amount, value); }
		}
		  
		[XmlAttribute("Remark")]
		[Bindable(true)]
		public string Remark 
		{
			get { return GetColumnValue<string>(Columns.Remark); }
			set { SetColumnValue(Columns.Remark, value); }
		}
		  
		[XmlAttribute("VoucherStatusId")]
		[Bindable(true)]
		public int VoucherStatusId 
		{
			get { return GetColumnValue<int>(Columns.VoucherStatusId); }
			set { SetColumnValue(Columns.VoucherStatusId, value); }
		}
		  
		[XmlAttribute("DateIssued")]
		[Bindable(true)]
		public DateTime? DateIssued 
		{
			get { return GetColumnValue<DateTime?>(Columns.DateIssued); }
			set { SetColumnValue(Columns.DateIssued, value); }
		}
		  
		[XmlAttribute("DateSold")]
		[Bindable(true)]
		public DateTime? DateSold 
		{
			get { return GetColumnValue<DateTime?>(Columns.DateSold); }
			set { SetColumnValue(Columns.DateSold, value); }
		}
		  
		[XmlAttribute("DateRedeemed")]
		[Bindable(true)]
		public DateTime? DateRedeemed 
		{
			get { return GetColumnValue<DateTime?>(Columns.DateRedeemed); }
			set { SetColumnValue(Columns.DateRedeemed, value); }
		}
		  
		[XmlAttribute("ExpiryDate")]
		[Bindable(true)]
		public DateTime? ExpiryDate 
		{
			get { return GetColumnValue<DateTime?>(Columns.ExpiryDate); }
			set { SetColumnValue(Columns.ExpiryDate, value); }
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
		public bool Deleted 
		{
			get { return GetColumnValue<bool>(Columns.Deleted); }
			set { SetColumnValue(Columns.Deleted, value); }
		}
		  
		[XmlAttribute("RedeemAmount")]
		[Bindable(true)]
		public decimal? RedeemAmount 
		{
			get { return GetColumnValue<decimal?>(Columns.RedeemAmount); }
			set { SetColumnValue(Columns.RedeemAmount, value); }
		}
		  
		[XmlAttribute("DateCanceled")]
		[Bindable(true)]
		public DateTime? DateCanceled 
		{
			get { return GetColumnValue<DateTime?>(Columns.DateCanceled); }
			set { SetColumnValue(Columns.DateCanceled, value); }
		}
		  
		[XmlAttribute("VoucherHeaderID")]
		[Bindable(true)]
		public int? VoucherHeaderID 
		{
			get { return GetColumnValue<int?>(Columns.VoucherHeaderID); }
			set { SetColumnValue(Columns.VoucherHeaderID, value); }
		}
		  
		[XmlAttribute("Outlet")]
		[Bindable(true)]
		public string Outlet 
		{
			get { return GetColumnValue<string>(Columns.Outlet); }
			set { SetColumnValue(Columns.Outlet, value); }
		}
		
		#endregion
		
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a VoucherStatus ActiveRecord object related to this Voucher
		/// 
		/// </summary>
		public PowerPOS.VoucherStatus VoucherStatus
		{
			get { return PowerPOS.VoucherStatus.FetchByID(this.VoucherStatusId); }
			set { SetColumnValue("VoucherStatusId", value.VoucherStatusId); }
		}
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(Guid varVoucherID,string varVoucherNo,decimal varAmount,string varRemark,int varVoucherStatusId,DateTime? varDateIssued,DateTime? varDateSold,DateTime? varDateRedeemed,DateTime? varExpiryDate,DateTime? varCreatedOn,string varCreatedBy,DateTime? varModifiedOn,string varModifiedBy,bool varDeleted,decimal? varRedeemAmount,DateTime? varDateCanceled,int? varVoucherHeaderID,string varOutlet)
		{
			Voucher item = new Voucher();
			
			item.VoucherID = varVoucherID;
			
			item.VoucherNo = varVoucherNo;
			
			item.Amount = varAmount;
			
			item.Remark = varRemark;
			
			item.VoucherStatusId = varVoucherStatusId;
			
			item.DateIssued = varDateIssued;
			
			item.DateSold = varDateSold;
			
			item.DateRedeemed = varDateRedeemed;
			
			item.ExpiryDate = varExpiryDate;
			
			item.CreatedOn = varCreatedOn;
			
			item.CreatedBy = varCreatedBy;
			
			item.ModifiedOn = varModifiedOn;
			
			item.ModifiedBy = varModifiedBy;
			
			item.Deleted = varDeleted;
			
			item.RedeemAmount = varRedeemAmount;
			
			item.DateCanceled = varDateCanceled;
			
			item.VoucherHeaderID = varVoucherHeaderID;
			
			item.Outlet = varOutlet;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(Guid varVoucherID,string varVoucherNo,decimal varAmount,string varRemark,int varVoucherStatusId,DateTime? varDateIssued,DateTime? varDateSold,DateTime? varDateRedeemed,DateTime? varExpiryDate,DateTime? varCreatedOn,string varCreatedBy,DateTime? varModifiedOn,string varModifiedBy,bool varDeleted,decimal? varRedeemAmount,DateTime? varDateCanceled,int? varVoucherHeaderID,string varOutlet)
		{
			Voucher item = new Voucher();
			
				item.VoucherID = varVoucherID;
			
				item.VoucherNo = varVoucherNo;
			
				item.Amount = varAmount;
			
				item.Remark = varRemark;
			
				item.VoucherStatusId = varVoucherStatusId;
			
				item.DateIssued = varDateIssued;
			
				item.DateSold = varDateSold;
			
				item.DateRedeemed = varDateRedeemed;
			
				item.ExpiryDate = varExpiryDate;
			
				item.CreatedOn = varCreatedOn;
			
				item.CreatedBy = varCreatedBy;
			
				item.ModifiedOn = varModifiedOn;
			
				item.ModifiedBy = varModifiedBy;
			
				item.Deleted = varDeleted;
			
				item.RedeemAmount = varRedeemAmount;
			
				item.DateCanceled = varDateCanceled;
			
				item.VoucherHeaderID = varVoucherHeaderID;
			
				item.Outlet = varOutlet;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn VoucherIDColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn VoucherNoColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn AmountColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn RemarkColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn VoucherStatusIdColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn DateIssuedColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn DateSoldColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn DateRedeemedColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn ExpiryDateColumn
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
        
        
        
        public static TableSchema.TableColumn DeletedColumn
        {
            get { return Schema.Columns[13]; }
        }
        
        
        
        public static TableSchema.TableColumn RedeemAmountColumn
        {
            get { return Schema.Columns[14]; }
        }
        
        
        
        public static TableSchema.TableColumn DateCanceledColumn
        {
            get { return Schema.Columns[15]; }
        }
        
        
        
        public static TableSchema.TableColumn VoucherHeaderIDColumn
        {
            get { return Schema.Columns[16]; }
        }
        
        
        
        public static TableSchema.TableColumn OutletColumn
        {
            get { return Schema.Columns[17]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string VoucherID = @"VoucherID";
			 public static string VoucherNo = @"VoucherNo";
			 public static string Amount = @"Amount";
			 public static string Remark = @"Remark";
			 public static string VoucherStatusId = @"VoucherStatusId";
			 public static string DateIssued = @"DateIssued";
			 public static string DateSold = @"DateSold";
			 public static string DateRedeemed = @"DateRedeemed";
			 public static string ExpiryDate = @"ExpiryDate";
			 public static string CreatedOn = @"CreatedOn";
			 public static string CreatedBy = @"CreatedBy";
			 public static string ModifiedOn = @"ModifiedOn";
			 public static string ModifiedBy = @"ModifiedBy";
			 public static string Deleted = @"Deleted";
			 public static string RedeemAmount = @"RedeemAmount";
			 public static string DateCanceled = @"DateCanceled";
			 public static string VoucherHeaderID = @"VoucherHeaderID";
			 public static string Outlet = @"Outlet";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
