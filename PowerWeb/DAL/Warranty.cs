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
	/// Strongly-typed collection for the Warranty class.
	/// </summary>
    [Serializable]
	public partial class WarrantyCollection : ActiveList<Warranty, WarrantyCollection>
	{	   
		public WarrantyCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>WarrantyCollection</returns>
		public WarrantyCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                Warranty o = this[i];
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
	/// This is an ActiveRecord class which wraps the Warranty table.
	/// </summary>
	[Serializable]
	public partial class Warranty : ActiveRecord<Warranty>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public Warranty()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public Warranty(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public Warranty(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public Warranty(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("Warranty", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarWarrantyID = new TableSchema.TableColumn(schema);
				colvarWarrantyID.ColumnName = "WarrantyID";
				colvarWarrantyID.DataType = DbType.Guid;
				colvarWarrantyID.MaxLength = 0;
				colvarWarrantyID.AutoIncrement = false;
				colvarWarrantyID.IsNullable = false;
				colvarWarrantyID.IsPrimaryKey = true;
				colvarWarrantyID.IsForeignKey = false;
				colvarWarrantyID.IsReadOnly = false;
				
						colvarWarrantyID.DefaultSetting = @"(newid())";
				colvarWarrantyID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarWarrantyID);
				
				TableSchema.TableColumn colvarSerialNo = new TableSchema.TableColumn(schema);
				colvarSerialNo.ColumnName = "SerialNo";
				colvarSerialNo.DataType = DbType.AnsiString;
				colvarSerialNo.MaxLength = 50;
				colvarSerialNo.AutoIncrement = false;
				colvarSerialNo.IsNullable = false;
				colvarSerialNo.IsPrimaryKey = false;
				colvarSerialNo.IsForeignKey = false;
				colvarSerialNo.IsReadOnly = false;
				colvarSerialNo.DefaultSetting = @"";
				colvarSerialNo.ForeignKeyTableName = "";
				schema.Columns.Add(colvarSerialNo);
				
				TableSchema.TableColumn colvarModelNo = new TableSchema.TableColumn(schema);
				colvarModelNo.ColumnName = "ModelNo";
				colvarModelNo.DataType = DbType.AnsiString;
				colvarModelNo.MaxLength = 50;
				colvarModelNo.AutoIncrement = false;
				colvarModelNo.IsNullable = true;
				colvarModelNo.IsPrimaryKey = false;
				colvarModelNo.IsForeignKey = false;
				colvarModelNo.IsReadOnly = false;
				colvarModelNo.DefaultSetting = @"";
				colvarModelNo.ForeignKeyTableName = "";
				schema.Columns.Add(colvarModelNo);
				
				TableSchema.TableColumn colvarOrderDetId = new TableSchema.TableColumn(schema);
				colvarOrderDetId.ColumnName = "OrderDetId";
				colvarOrderDetId.DataType = DbType.AnsiString;
				colvarOrderDetId.MaxLength = 18;
				colvarOrderDetId.AutoIncrement = false;
				colvarOrderDetId.IsNullable = true;
				colvarOrderDetId.IsPrimaryKey = false;
				colvarOrderDetId.IsForeignKey = true;
				colvarOrderDetId.IsReadOnly = false;
				colvarOrderDetId.DefaultSetting = @"";
				
					colvarOrderDetId.ForeignKeyTableName = "OrderDet";
				schema.Columns.Add(colvarOrderDetId);
				
				TableSchema.TableColumn colvarMembershipNo = new TableSchema.TableColumn(schema);
				colvarMembershipNo.ColumnName = "MembershipNo";
				colvarMembershipNo.DataType = DbType.AnsiString;
				colvarMembershipNo.MaxLength = 50;
				colvarMembershipNo.AutoIncrement = false;
				colvarMembershipNo.IsNullable = true;
				colvarMembershipNo.IsPrimaryKey = false;
				colvarMembershipNo.IsForeignKey = false;
				colvarMembershipNo.IsReadOnly = false;
				colvarMembershipNo.DefaultSetting = @"";
				colvarMembershipNo.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMembershipNo);
				
				TableSchema.TableColumn colvarItemNo = new TableSchema.TableColumn(schema);
				colvarItemNo.ColumnName = "ItemNo";
				colvarItemNo.DataType = DbType.AnsiString;
				colvarItemNo.MaxLength = 50;
				colvarItemNo.AutoIncrement = false;
				colvarItemNo.IsNullable = true;
				colvarItemNo.IsPrimaryKey = false;
				colvarItemNo.IsForeignKey = true;
				colvarItemNo.IsReadOnly = false;
				colvarItemNo.DefaultSetting = @"";
				
					colvarItemNo.ForeignKeyTableName = "Item";
				schema.Columns.Add(colvarItemNo);
				
				TableSchema.TableColumn colvarProductIdentification = new TableSchema.TableColumn(schema);
				colvarProductIdentification.ColumnName = "ProductIdentification";
				colvarProductIdentification.DataType = DbType.String;
				colvarProductIdentification.MaxLength = -1;
				colvarProductIdentification.AutoIncrement = false;
				colvarProductIdentification.IsNullable = true;
				colvarProductIdentification.IsPrimaryKey = false;
				colvarProductIdentification.IsForeignKey = false;
				colvarProductIdentification.IsReadOnly = false;
				colvarProductIdentification.DefaultSetting = @"";
				colvarProductIdentification.ForeignKeyTableName = "";
				schema.Columns.Add(colvarProductIdentification);
				
				TableSchema.TableColumn colvarDateOfPurchase = new TableSchema.TableColumn(schema);
				colvarDateOfPurchase.ColumnName = "DateOfPurchase";
				colvarDateOfPurchase.DataType = DbType.DateTime;
				colvarDateOfPurchase.MaxLength = 0;
				colvarDateOfPurchase.AutoIncrement = false;
				colvarDateOfPurchase.IsNullable = true;
				colvarDateOfPurchase.IsPrimaryKey = false;
				colvarDateOfPurchase.IsForeignKey = false;
				colvarDateOfPurchase.IsReadOnly = false;
				colvarDateOfPurchase.DefaultSetting = @"";
				colvarDateOfPurchase.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDateOfPurchase);
				
				TableSchema.TableColumn colvarDateExpiry = new TableSchema.TableColumn(schema);
				colvarDateExpiry.ColumnName = "DateExpiry";
				colvarDateExpiry.DataType = DbType.DateTime;
				colvarDateExpiry.MaxLength = 0;
				colvarDateExpiry.AutoIncrement = false;
				colvarDateExpiry.IsNullable = true;
				colvarDateExpiry.IsPrimaryKey = false;
				colvarDateExpiry.IsForeignKey = false;
				colvarDateExpiry.IsReadOnly = false;
				colvarDateExpiry.DefaultSetting = @"";
				colvarDateExpiry.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDateExpiry);
				
				TableSchema.TableColumn colvarRemark = new TableSchema.TableColumn(schema);
				colvarRemark.ColumnName = "Remark";
				colvarRemark.DataType = DbType.String;
				colvarRemark.MaxLength = -1;
				colvarRemark.AutoIncrement = false;
				colvarRemark.IsNullable = true;
				colvarRemark.IsPrimaryKey = false;
				colvarRemark.IsForeignKey = false;
				colvarRemark.IsReadOnly = false;
				colvarRemark.DefaultSetting = @"";
				colvarRemark.ForeignKeyTableName = "";
				schema.Columns.Add(colvarRemark);
				
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
				colvarCreatedBy.DataType = DbType.String;
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
				colvarModifiedBy.DataType = DbType.String;
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
				DataService.Providers["PowerPOS"].AddSchema("Warranty",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("WarrantyID")]
		[Bindable(true)]
		public Guid WarrantyID 
		{
			get { return GetColumnValue<Guid>(Columns.WarrantyID); }
			set { SetColumnValue(Columns.WarrantyID, value); }
		}
		  
		[XmlAttribute("SerialNo")]
		[Bindable(true)]
		public string SerialNo 
		{
			get { return GetColumnValue<string>(Columns.SerialNo); }
			set { SetColumnValue(Columns.SerialNo, value); }
		}
		  
		[XmlAttribute("ModelNo")]
		[Bindable(true)]
		public string ModelNo 
		{
			get { return GetColumnValue<string>(Columns.ModelNo); }
			set { SetColumnValue(Columns.ModelNo, value); }
		}
		  
		[XmlAttribute("OrderDetId")]
		[Bindable(true)]
		public string OrderDetId 
		{
			get { return GetColumnValue<string>(Columns.OrderDetId); }
			set { SetColumnValue(Columns.OrderDetId, value); }
		}
		  
		[XmlAttribute("MembershipNo")]
		[Bindable(true)]
		public string MembershipNo 
		{
			get { return GetColumnValue<string>(Columns.MembershipNo); }
			set { SetColumnValue(Columns.MembershipNo, value); }
		}
		  
		[XmlAttribute("ItemNo")]
		[Bindable(true)]
		public string ItemNo 
		{
			get { return GetColumnValue<string>(Columns.ItemNo); }
			set { SetColumnValue(Columns.ItemNo, value); }
		}
		  
		[XmlAttribute("ProductIdentification")]
		[Bindable(true)]
		public string ProductIdentification 
		{
			get { return GetColumnValue<string>(Columns.ProductIdentification); }
			set { SetColumnValue(Columns.ProductIdentification, value); }
		}
		  
		[XmlAttribute("DateOfPurchase")]
		[Bindable(true)]
		public DateTime? DateOfPurchase 
		{
			get { return GetColumnValue<DateTime?>(Columns.DateOfPurchase); }
			set { SetColumnValue(Columns.DateOfPurchase, value); }
		}
		  
		[XmlAttribute("DateExpiry")]
		[Bindable(true)]
		public DateTime? DateExpiry 
		{
			get { return GetColumnValue<DateTime?>(Columns.DateExpiry); }
			set { SetColumnValue(Columns.DateExpiry, value); }
		}
		  
		[XmlAttribute("Remark")]
		[Bindable(true)]
		public string Remark 
		{
			get { return GetColumnValue<string>(Columns.Remark); }
			set { SetColumnValue(Columns.Remark, value); }
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
		
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a Item ActiveRecord object related to this Warranty
		/// 
		/// </summary>
		public PowerPOS.Item Item
		{
			get { return PowerPOS.Item.FetchByID(this.ItemNo); }
			set { SetColumnValue("ItemNo", value.ItemNo); }
		}
		
		
		/// <summary>
		/// Returns a OrderDet ActiveRecord object related to this Warranty
		/// 
		/// </summary>
		public PowerPOS.OrderDet OrderDet
		{
			get { return PowerPOS.OrderDet.FetchByID(this.OrderDetId); }
			set { SetColumnValue("OrderDetId", value.OrderDetID); }
		}
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(Guid varWarrantyID,string varSerialNo,string varModelNo,string varOrderDetId,string varMembershipNo,string varItemNo,string varProductIdentification,DateTime? varDateOfPurchase,DateTime? varDateExpiry,string varRemark,DateTime? varCreatedOn,string varCreatedBy,DateTime? varModifiedOn,string varModifiedBy)
		{
			Warranty item = new Warranty();
			
			item.WarrantyID = varWarrantyID;
			
			item.SerialNo = varSerialNo;
			
			item.ModelNo = varModelNo;
			
			item.OrderDetId = varOrderDetId;
			
			item.MembershipNo = varMembershipNo;
			
			item.ItemNo = varItemNo;
			
			item.ProductIdentification = varProductIdentification;
			
			item.DateOfPurchase = varDateOfPurchase;
			
			item.DateExpiry = varDateExpiry;
			
			item.Remark = varRemark;
			
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
		public static void Update(Guid varWarrantyID,string varSerialNo,string varModelNo,string varOrderDetId,string varMembershipNo,string varItemNo,string varProductIdentification,DateTime? varDateOfPurchase,DateTime? varDateExpiry,string varRemark,DateTime? varCreatedOn,string varCreatedBy,DateTime? varModifiedOn,string varModifiedBy)
		{
			Warranty item = new Warranty();
			
				item.WarrantyID = varWarrantyID;
			
				item.SerialNo = varSerialNo;
			
				item.ModelNo = varModelNo;
			
				item.OrderDetId = varOrderDetId;
			
				item.MembershipNo = varMembershipNo;
			
				item.ItemNo = varItemNo;
			
				item.ProductIdentification = varProductIdentification;
			
				item.DateOfPurchase = varDateOfPurchase;
			
				item.DateExpiry = varDateExpiry;
			
				item.Remark = varRemark;
			
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
        
        
        public static TableSchema.TableColumn WarrantyIDColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn SerialNoColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn ModelNoColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn OrderDetIdColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn MembershipNoColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn ItemNoColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn ProductIdentificationColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn DateOfPurchaseColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn DateExpiryColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn RemarkColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedOnColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedByColumn
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedOnColumn
        {
            get { return Schema.Columns[12]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedByColumn
        {
            get { return Schema.Columns[13]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string WarrantyID = @"WarrantyID";
			 public static string SerialNo = @"SerialNo";
			 public static string ModelNo = @"ModelNo";
			 public static string OrderDetId = @"OrderDetId";
			 public static string MembershipNo = @"MembershipNo";
			 public static string ItemNo = @"ItemNo";
			 public static string ProductIdentification = @"ProductIdentification";
			 public static string DateOfPurchase = @"DateOfPurchase";
			 public static string DateExpiry = @"DateExpiry";
			 public static string Remark = @"Remark";
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
