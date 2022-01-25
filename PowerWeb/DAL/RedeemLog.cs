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
	/// Strongly-typed collection for the RedeemLog class.
	/// </summary>
    [Serializable]
	public partial class RedeemLogCollection : ActiveList<RedeemLog, RedeemLogCollection>
	{	   
		public RedeemLogCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>RedeemLogCollection</returns>
		public RedeemLogCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                RedeemLog o = this[i];
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
	/// This is an ActiveRecord class which wraps the RedeemLog table.
	/// </summary>
	[Serializable]
	public partial class RedeemLog : ActiveRecord<RedeemLog>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public RedeemLog()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public RedeemLog(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public RedeemLog(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public RedeemLog(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("RedeemLog", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarRedeemLogId = new TableSchema.TableColumn(schema);
				colvarRedeemLogId.ColumnName = "RedeemLogId";
				colvarRedeemLogId.DataType = DbType.Int32;
				colvarRedeemLogId.MaxLength = 0;
				colvarRedeemLogId.AutoIncrement = true;
				colvarRedeemLogId.IsNullable = false;
				colvarRedeemLogId.IsPrimaryKey = true;
				colvarRedeemLogId.IsForeignKey = false;
				colvarRedeemLogId.IsReadOnly = false;
				colvarRedeemLogId.DefaultSetting = @"";
				colvarRedeemLogId.ForeignKeyTableName = "";
				schema.Columns.Add(colvarRedeemLogId);
				
				TableSchema.TableColumn colvarRedemptionId = new TableSchema.TableColumn(schema);
				colvarRedemptionId.ColumnName = "RedemptionId";
				colvarRedemptionId.DataType = DbType.Int32;
				colvarRedemptionId.MaxLength = 0;
				colvarRedemptionId.AutoIncrement = false;
				colvarRedemptionId.IsNullable = false;
				colvarRedemptionId.IsPrimaryKey = false;
				colvarRedemptionId.IsForeignKey = false;
				colvarRedemptionId.IsReadOnly = false;
				colvarRedemptionId.DefaultSetting = @"";
				colvarRedemptionId.ForeignKeyTableName = "";
				schema.Columns.Add(colvarRedemptionId);
				
				TableSchema.TableColumn colvarMembershipNo = new TableSchema.TableColumn(schema);
				colvarMembershipNo.ColumnName = "MembershipNo";
				colvarMembershipNo.DataType = DbType.AnsiString;
				colvarMembershipNo.MaxLength = 50;
				colvarMembershipNo.AutoIncrement = false;
				colvarMembershipNo.IsNullable = false;
				colvarMembershipNo.IsPrimaryKey = false;
				colvarMembershipNo.IsForeignKey = false;
				colvarMembershipNo.IsReadOnly = false;
				colvarMembershipNo.DefaultSetting = @"";
				colvarMembershipNo.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMembershipNo);
				
				TableSchema.TableColumn colvarRedeemDate = new TableSchema.TableColumn(schema);
				colvarRedeemDate.ColumnName = "RedeemDate";
				colvarRedeemDate.DataType = DbType.DateTime;
				colvarRedeemDate.MaxLength = 0;
				colvarRedeemDate.AutoIncrement = false;
				colvarRedeemDate.IsNullable = true;
				colvarRedeemDate.IsPrimaryKey = false;
				colvarRedeemDate.IsForeignKey = false;
				colvarRedeemDate.IsReadOnly = false;
				colvarRedeemDate.DefaultSetting = @"";
				colvarRedeemDate.ForeignKeyTableName = "";
				schema.Columns.Add(colvarRedeemDate);
				
				TableSchema.TableColumn colvarPointsBefore = new TableSchema.TableColumn(schema);
				colvarPointsBefore.ColumnName = "PointsBefore";
				colvarPointsBefore.DataType = DbType.Currency;
				colvarPointsBefore.MaxLength = 0;
				colvarPointsBefore.AutoIncrement = false;
				colvarPointsBefore.IsNullable = true;
				colvarPointsBefore.IsPrimaryKey = false;
				colvarPointsBefore.IsForeignKey = false;
				colvarPointsBefore.IsReadOnly = false;
				colvarPointsBefore.DefaultSetting = @"";
				colvarPointsBefore.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPointsBefore);
				
				TableSchema.TableColumn colvarPointsAfter = new TableSchema.TableColumn(schema);
				colvarPointsAfter.ColumnName = "PointsAfter";
				colvarPointsAfter.DataType = DbType.Currency;
				colvarPointsAfter.MaxLength = 0;
				colvarPointsAfter.AutoIncrement = false;
				colvarPointsAfter.IsNullable = true;
				colvarPointsAfter.IsPrimaryKey = false;
				colvarPointsAfter.IsForeignKey = false;
				colvarPointsAfter.IsReadOnly = false;
				colvarPointsAfter.DefaultSetting = @"";
				colvarPointsAfter.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPointsAfter);
				
				TableSchema.TableColumn colvarIsStockOutAlready = new TableSchema.TableColumn(schema);
				colvarIsStockOutAlready.ColumnName = "IsStockOutAlready";
				colvarIsStockOutAlready.DataType = DbType.Boolean;
				colvarIsStockOutAlready.MaxLength = 0;
				colvarIsStockOutAlready.AutoIncrement = false;
				colvarIsStockOutAlready.IsNullable = false;
				colvarIsStockOutAlready.IsPrimaryKey = false;
				colvarIsStockOutAlready.IsForeignKey = false;
				colvarIsStockOutAlready.IsReadOnly = false;
				
						colvarIsStockOutAlready.DefaultSetting = @"((0))";
				colvarIsStockOutAlready.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIsStockOutAlready);
				
				TableSchema.TableColumn colvarDeliveryAddress = new TableSchema.TableColumn(schema);
				colvarDeliveryAddress.ColumnName = "DeliveryAddress";
				colvarDeliveryAddress.DataType = DbType.AnsiString;
				colvarDeliveryAddress.MaxLength = -1;
				colvarDeliveryAddress.AutoIncrement = false;
				colvarDeliveryAddress.IsNullable = true;
				colvarDeliveryAddress.IsPrimaryKey = false;
				colvarDeliveryAddress.IsForeignKey = false;
				colvarDeliveryAddress.IsReadOnly = false;
				colvarDeliveryAddress.DefaultSetting = @"";
				colvarDeliveryAddress.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDeliveryAddress);
				
				TableSchema.TableColumn colvarContactNo = new TableSchema.TableColumn(schema);
				colvarContactNo.ColumnName = "ContactNo";
				colvarContactNo.DataType = DbType.AnsiString;
				colvarContactNo.MaxLength = -1;
				colvarContactNo.AutoIncrement = false;
				colvarContactNo.IsNullable = true;
				colvarContactNo.IsPrimaryKey = false;
				colvarContactNo.IsForeignKey = false;
				colvarContactNo.IsReadOnly = false;
				colvarContactNo.DefaultSetting = @"";
				colvarContactNo.ForeignKeyTableName = "";
				schema.Columns.Add(colvarContactNo);
				
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
				DataService.Providers["PowerPOS"].AddSchema("RedeemLog",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("RedeemLogId")]
		[Bindable(true)]
		public int RedeemLogId 
		{
			get { return GetColumnValue<int>(Columns.RedeemLogId); }
			set { SetColumnValue(Columns.RedeemLogId, value); }
		}
		  
		[XmlAttribute("RedemptionId")]
		[Bindable(true)]
		public int RedemptionId 
		{
			get { return GetColumnValue<int>(Columns.RedemptionId); }
			set { SetColumnValue(Columns.RedemptionId, value); }
		}
		  
		[XmlAttribute("MembershipNo")]
		[Bindable(true)]
		public string MembershipNo 
		{
			get { return GetColumnValue<string>(Columns.MembershipNo); }
			set { SetColumnValue(Columns.MembershipNo, value); }
		}
		  
		[XmlAttribute("RedeemDate")]
		[Bindable(true)]
		public DateTime? RedeemDate 
		{
			get { return GetColumnValue<DateTime?>(Columns.RedeemDate); }
			set { SetColumnValue(Columns.RedeemDate, value); }
		}
		  
		[XmlAttribute("PointsBefore")]
		[Bindable(true)]
		public decimal? PointsBefore 
		{
			get { return GetColumnValue<decimal?>(Columns.PointsBefore); }
			set { SetColumnValue(Columns.PointsBefore, value); }
		}
		  
		[XmlAttribute("PointsAfter")]
		[Bindable(true)]
		public decimal? PointsAfter 
		{
			get { return GetColumnValue<decimal?>(Columns.PointsAfter); }
			set { SetColumnValue(Columns.PointsAfter, value); }
		}
		  
		[XmlAttribute("IsStockOutAlready")]
		[Bindable(true)]
		public bool IsStockOutAlready 
		{
			get { return GetColumnValue<bool>(Columns.IsStockOutAlready); }
			set { SetColumnValue(Columns.IsStockOutAlready, value); }
		}
		  
		[XmlAttribute("DeliveryAddress")]
		[Bindable(true)]
		public string DeliveryAddress 
		{
			get { return GetColumnValue<string>(Columns.DeliveryAddress); }
			set { SetColumnValue(Columns.DeliveryAddress, value); }
		}
		  
		[XmlAttribute("ContactNo")]
		[Bindable(true)]
		public string ContactNo 
		{
			get { return GetColumnValue<string>(Columns.ContactNo); }
			set { SetColumnValue(Columns.ContactNo, value); }
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
		
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(int varRedemptionId,string varMembershipNo,DateTime? varRedeemDate,decimal? varPointsBefore,decimal? varPointsAfter,bool varIsStockOutAlready,string varDeliveryAddress,string varContactNo,DateTime? varCreatedOn,string varCreatedBy,DateTime? varModifiedOn,string varModifiedBy,bool? varDeleted)
		{
			RedeemLog item = new RedeemLog();
			
			item.RedemptionId = varRedemptionId;
			
			item.MembershipNo = varMembershipNo;
			
			item.RedeemDate = varRedeemDate;
			
			item.PointsBefore = varPointsBefore;
			
			item.PointsAfter = varPointsAfter;
			
			item.IsStockOutAlready = varIsStockOutAlready;
			
			item.DeliveryAddress = varDeliveryAddress;
			
			item.ContactNo = varContactNo;
			
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
		public static void Update(int varRedeemLogId,int varRedemptionId,string varMembershipNo,DateTime? varRedeemDate,decimal? varPointsBefore,decimal? varPointsAfter,bool varIsStockOutAlready,string varDeliveryAddress,string varContactNo,DateTime? varCreatedOn,string varCreatedBy,DateTime? varModifiedOn,string varModifiedBy,bool? varDeleted)
		{
			RedeemLog item = new RedeemLog();
			
				item.RedeemLogId = varRedeemLogId;
			
				item.RedemptionId = varRedemptionId;
			
				item.MembershipNo = varMembershipNo;
			
				item.RedeemDate = varRedeemDate;
			
				item.PointsBefore = varPointsBefore;
			
				item.PointsAfter = varPointsAfter;
			
				item.IsStockOutAlready = varIsStockOutAlready;
			
				item.DeliveryAddress = varDeliveryAddress;
			
				item.ContactNo = varContactNo;
			
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
        
        
        public static TableSchema.TableColumn RedeemLogIdColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn RedemptionIdColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn MembershipNoColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn RedeemDateColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn PointsBeforeColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn PointsAfterColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn IsStockOutAlreadyColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn DeliveryAddressColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn ContactNoColumn
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
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string RedeemLogId = @"RedeemLogId";
			 public static string RedemptionId = @"RedemptionId";
			 public static string MembershipNo = @"MembershipNo";
			 public static string RedeemDate = @"RedeemDate";
			 public static string PointsBefore = @"PointsBefore";
			 public static string PointsAfter = @"PointsAfter";
			 public static string IsStockOutAlready = @"IsStockOutAlready";
			 public static string DeliveryAddress = @"DeliveryAddress";
			 public static string ContactNo = @"ContactNo";
			 public static string CreatedOn = @"CreatedOn";
			 public static string CreatedBy = @"CreatedBy";
			 public static string ModifiedOn = @"ModifiedOn";
			 public static string ModifiedBy = @"ModifiedBy";
			 public static string Deleted = @"Deleted";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
