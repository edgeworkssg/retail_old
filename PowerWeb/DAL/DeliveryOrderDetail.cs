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
	/// Strongly-typed collection for the DeliveryOrderDetail class.
	/// </summary>
    [Serializable]
	public partial class DeliveryOrderDetailCollection : ActiveList<DeliveryOrderDetail, DeliveryOrderDetailCollection>
	{	   
		public DeliveryOrderDetailCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>DeliveryOrderDetailCollection</returns>
		public DeliveryOrderDetailCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                DeliveryOrderDetail o = this[i];
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
	/// This is an ActiveRecord class which wraps the DeliveryOrderDetails table.
	/// </summary>
	[Serializable]
	public partial class DeliveryOrderDetail : ActiveRecord<DeliveryOrderDetail>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public DeliveryOrderDetail()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public DeliveryOrderDetail(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public DeliveryOrderDetail(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public DeliveryOrderDetail(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("DeliveryOrderDetails", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarDetailsID = new TableSchema.TableColumn(schema);
				colvarDetailsID.ColumnName = "DetailsID";
				colvarDetailsID.DataType = DbType.AnsiString;
				colvarDetailsID.MaxLength = 50;
				colvarDetailsID.AutoIncrement = false;
				colvarDetailsID.IsNullable = false;
				colvarDetailsID.IsPrimaryKey = true;
				colvarDetailsID.IsForeignKey = false;
				colvarDetailsID.IsReadOnly = false;
				colvarDetailsID.DefaultSetting = @"";
				colvarDetailsID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDetailsID);
				
				TableSchema.TableColumn colvarItemNo = new TableSchema.TableColumn(schema);
				colvarItemNo.ColumnName = "ItemNo";
				colvarItemNo.DataType = DbType.AnsiString;
				colvarItemNo.MaxLength = 50;
				colvarItemNo.AutoIncrement = false;
				colvarItemNo.IsNullable = true;
				colvarItemNo.IsPrimaryKey = false;
				colvarItemNo.IsForeignKey = false;
				colvarItemNo.IsReadOnly = false;
				colvarItemNo.DefaultSetting = @"";
				colvarItemNo.ForeignKeyTableName = "";
				schema.Columns.Add(colvarItemNo);
				
				TableSchema.TableColumn colvarQuantity = new TableSchema.TableColumn(schema);
				colvarQuantity.ColumnName = "Quantity";
				colvarQuantity.DataType = DbType.Decimal;
				colvarQuantity.MaxLength = 0;
				colvarQuantity.AutoIncrement = false;
				colvarQuantity.IsNullable = true;
				colvarQuantity.IsPrimaryKey = false;
				colvarQuantity.IsForeignKey = false;
				colvarQuantity.IsReadOnly = false;
				colvarQuantity.DefaultSetting = @"";
				colvarQuantity.ForeignKeyTableName = "";
				schema.Columns.Add(colvarQuantity);
				
				TableSchema.TableColumn colvarSerialNumber = new TableSchema.TableColumn(schema);
				colvarSerialNumber.ColumnName = "Serial_Number";
				colvarSerialNumber.DataType = DbType.AnsiString;
				colvarSerialNumber.MaxLength = 50;
				colvarSerialNumber.AutoIncrement = false;
				colvarSerialNumber.IsNullable = true;
				colvarSerialNumber.IsPrimaryKey = false;
				colvarSerialNumber.IsForeignKey = false;
				colvarSerialNumber.IsReadOnly = false;
				colvarSerialNumber.DefaultSetting = @"";
				colvarSerialNumber.ForeignKeyTableName = "";
				schema.Columns.Add(colvarSerialNumber);
				
				TableSchema.TableColumn colvarDohdrid = new TableSchema.TableColumn(schema);
				colvarDohdrid.ColumnName = "DOHDRID";
				colvarDohdrid.DataType = DbType.AnsiString;
				colvarDohdrid.MaxLength = 50;
				colvarDohdrid.AutoIncrement = false;
				colvarDohdrid.IsNullable = true;
				colvarDohdrid.IsPrimaryKey = false;
				colvarDohdrid.IsForeignKey = true;
				colvarDohdrid.IsReadOnly = false;
				colvarDohdrid.DefaultSetting = @"";
				
					colvarDohdrid.ForeignKeyTableName = "DeliveryOrder";
				schema.Columns.Add(colvarDohdrid);
				
				TableSchema.TableColumn colvarOrderDetID = new TableSchema.TableColumn(schema);
				colvarOrderDetID.ColumnName = "OrderDetID";
				colvarOrderDetID.DataType = DbType.AnsiString;
				colvarOrderDetID.MaxLength = 18;
				colvarOrderDetID.AutoIncrement = false;
				colvarOrderDetID.IsNullable = true;
				colvarOrderDetID.IsPrimaryKey = false;
				colvarOrderDetID.IsForeignKey = false;
				colvarOrderDetID.IsReadOnly = false;
				colvarOrderDetID.DefaultSetting = @"";
				colvarOrderDetID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarOrderDetID);
				
				TableSchema.TableColumn colvarRemarks = new TableSchema.TableColumn(schema);
				colvarRemarks.ColumnName = "Remarks";
				colvarRemarks.DataType = DbType.String;
				colvarRemarks.MaxLength = -1;
				colvarRemarks.AutoIncrement = false;
				colvarRemarks.IsNullable = true;
				colvarRemarks.IsPrimaryKey = false;
				colvarRemarks.IsForeignKey = false;
				colvarRemarks.IsReadOnly = false;
				colvarRemarks.DefaultSetting = @"";
				colvarRemarks.ForeignKeyTableName = "";
				schema.Columns.Add(colvarRemarks);
				
				TableSchema.TableColumn colvarCreatedOn = new TableSchema.TableColumn(schema);
				colvarCreatedOn.ColumnName = "CreatedOn";
				colvarCreatedOn.DataType = DbType.DateTime;
				colvarCreatedOn.MaxLength = 0;
				colvarCreatedOn.AutoIncrement = false;
				colvarCreatedOn.IsNullable = false;
				colvarCreatedOn.IsPrimaryKey = false;
				colvarCreatedOn.IsForeignKey = false;
				colvarCreatedOn.IsReadOnly = false;
				
						colvarCreatedOn.DefaultSetting = @"(getdate())";
				colvarCreatedOn.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCreatedOn);
				
				TableSchema.TableColumn colvarModifiedOn = new TableSchema.TableColumn(schema);
				colvarModifiedOn.ColumnName = "ModifiedOn";
				colvarModifiedOn.DataType = DbType.DateTime;
				colvarModifiedOn.MaxLength = 0;
				colvarModifiedOn.AutoIncrement = false;
				colvarModifiedOn.IsNullable = false;
				colvarModifiedOn.IsPrimaryKey = false;
				colvarModifiedOn.IsForeignKey = false;
				colvarModifiedOn.IsReadOnly = false;
				
						colvarModifiedOn.DefaultSetting = @"(getdate())";
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
				
				TableSchema.TableColumn colvarInventoryHdrRefNo = new TableSchema.TableColumn(schema);
				colvarInventoryHdrRefNo.ColumnName = "InventoryHdrRefNo";
				colvarInventoryHdrRefNo.DataType = DbType.AnsiString;
				colvarInventoryHdrRefNo.MaxLength = 50;
				colvarInventoryHdrRefNo.AutoIncrement = false;
				colvarInventoryHdrRefNo.IsNullable = true;
				colvarInventoryHdrRefNo.IsPrimaryKey = false;
				colvarInventoryHdrRefNo.IsForeignKey = false;
				colvarInventoryHdrRefNo.IsReadOnly = false;
				colvarInventoryHdrRefNo.DefaultSetting = @"";
				colvarInventoryHdrRefNo.ForeignKeyTableName = "";
				schema.Columns.Add(colvarInventoryHdrRefNo);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["PowerPOS"].AddSchema("DeliveryOrderDetails",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("DetailsID")]
		[Bindable(true)]
		public string DetailsID 
		{
			get { return GetColumnValue<string>(Columns.DetailsID); }
			set { SetColumnValue(Columns.DetailsID, value); }
		}
		  
		[XmlAttribute("ItemNo")]
		[Bindable(true)]
		public string ItemNo 
		{
			get { return GetColumnValue<string>(Columns.ItemNo); }
			set { SetColumnValue(Columns.ItemNo, value); }
		}
		  
		[XmlAttribute("Quantity")]
		[Bindable(true)]
		public decimal? Quantity 
		{
			get { return GetColumnValue<decimal?>(Columns.Quantity); }
			set { SetColumnValue(Columns.Quantity, value); }
		}
		  
		[XmlAttribute("SerialNumber")]
		[Bindable(true)]
		public string SerialNumber 
		{
			get { return GetColumnValue<string>(Columns.SerialNumber); }
			set { SetColumnValue(Columns.SerialNumber, value); }
		}
		  
		[XmlAttribute("Dohdrid")]
		[Bindable(true)]
		public string Dohdrid 
		{
			get { return GetColumnValue<string>(Columns.Dohdrid); }
			set { SetColumnValue(Columns.Dohdrid, value); }
		}
		  
		[XmlAttribute("OrderDetID")]
		[Bindable(true)]
		public string OrderDetID 
		{
			get { return GetColumnValue<string>(Columns.OrderDetID); }
			set { SetColumnValue(Columns.OrderDetID, value); }
		}
		  
		[XmlAttribute("Remarks")]
		[Bindable(true)]
		public string Remarks 
		{
			get { return GetColumnValue<string>(Columns.Remarks); }
			set { SetColumnValue(Columns.Remarks, value); }
		}
		  
		[XmlAttribute("CreatedOn")]
		[Bindable(true)]
		public DateTime CreatedOn 
		{
			get { return GetColumnValue<DateTime>(Columns.CreatedOn); }
			set { SetColumnValue(Columns.CreatedOn, value); }
		}
		  
		[XmlAttribute("ModifiedOn")]
		[Bindable(true)]
		public DateTime ModifiedOn 
		{
			get { return GetColumnValue<DateTime>(Columns.ModifiedOn); }
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
		  
		[XmlAttribute("Deleted")]
		[Bindable(true)]
		public bool? Deleted 
		{
			get { return GetColumnValue<bool?>(Columns.Deleted); }
			set { SetColumnValue(Columns.Deleted, value); }
		}
		  
		[XmlAttribute("InventoryHdrRefNo")]
		[Bindable(true)]
		public string InventoryHdrRefNo 
		{
			get { return GetColumnValue<string>(Columns.InventoryHdrRefNo); }
			set { SetColumnValue(Columns.InventoryHdrRefNo, value); }
		}
		
		#endregion
		
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a DeliveryOrder ActiveRecord object related to this DeliveryOrderDetail
		/// 
		/// </summary>
		public PowerPOS.DeliveryOrder DeliveryOrder
		{
			get { return PowerPOS.DeliveryOrder.FetchByID(this.Dohdrid); }
			set { SetColumnValue("DOHDRID", value.OrderNumber); }
		}
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(string varDetailsID,string varItemNo,decimal? varQuantity,string varSerialNumber,string varDohdrid,string varOrderDetID,string varRemarks,DateTime varCreatedOn,DateTime varModifiedOn,string varCreatedBy,string varModifiedBy,Guid varUniqueID,bool? varDeleted,string varInventoryHdrRefNo)
		{
			DeliveryOrderDetail item = new DeliveryOrderDetail();
			
			item.DetailsID = varDetailsID;
			
			item.ItemNo = varItemNo;
			
			item.Quantity = varQuantity;
			
			item.SerialNumber = varSerialNumber;
			
			item.Dohdrid = varDohdrid;
			
			item.OrderDetID = varOrderDetID;
			
			item.Remarks = varRemarks;
			
			item.CreatedOn = varCreatedOn;
			
			item.ModifiedOn = varModifiedOn;
			
			item.CreatedBy = varCreatedBy;
			
			item.ModifiedBy = varModifiedBy;
			
			item.UniqueID = varUniqueID;
			
			item.Deleted = varDeleted;
			
			item.InventoryHdrRefNo = varInventoryHdrRefNo;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(string varDetailsID,string varItemNo,decimal? varQuantity,string varSerialNumber,string varDohdrid,string varOrderDetID,string varRemarks,DateTime varCreatedOn,DateTime varModifiedOn,string varCreatedBy,string varModifiedBy,Guid varUniqueID,bool? varDeleted,string varInventoryHdrRefNo)
		{
			DeliveryOrderDetail item = new DeliveryOrderDetail();
			
				item.DetailsID = varDetailsID;
			
				item.ItemNo = varItemNo;
			
				item.Quantity = varQuantity;
			
				item.SerialNumber = varSerialNumber;
			
				item.Dohdrid = varDohdrid;
			
				item.OrderDetID = varOrderDetID;
			
				item.Remarks = varRemarks;
			
				item.CreatedOn = varCreatedOn;
			
				item.ModifiedOn = varModifiedOn;
			
				item.CreatedBy = varCreatedBy;
			
				item.ModifiedBy = varModifiedBy;
			
				item.UniqueID = varUniqueID;
			
				item.Deleted = varDeleted;
			
				item.InventoryHdrRefNo = varInventoryHdrRefNo;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn DetailsIDColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn ItemNoColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn QuantityColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn SerialNumberColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn DohdridColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn OrderDetIDColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn RemarksColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedOnColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedOnColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedByColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedByColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        public static TableSchema.TableColumn UniqueIDColumn
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        public static TableSchema.TableColumn DeletedColumn
        {
            get { return Schema.Columns[12]; }
        }
        
        
        
        public static TableSchema.TableColumn InventoryHdrRefNoColumn
        {
            get { return Schema.Columns[13]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string DetailsID = @"DetailsID";
			 public static string ItemNo = @"ItemNo";
			 public static string Quantity = @"Quantity";
			 public static string SerialNumber = @"Serial_Number";
			 public static string Dohdrid = @"DOHDRID";
			 public static string OrderDetID = @"OrderDetID";
			 public static string Remarks = @"Remarks";
			 public static string CreatedOn = @"CreatedOn";
			 public static string ModifiedOn = @"ModifiedOn";
			 public static string CreatedBy = @"CreatedBy";
			 public static string ModifiedBy = @"ModifiedBy";
			 public static string UniqueID = @"UniqueID";
			 public static string Deleted = @"Deleted";
			 public static string InventoryHdrRefNo = @"InventoryHdrRefNo";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
