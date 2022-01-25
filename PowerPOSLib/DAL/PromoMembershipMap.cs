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
	/// Strongly-typed collection for the PromoMembershipMap class.
	/// </summary>
    [Serializable]
	public partial class PromoMembershipMapCollection : ActiveList<PromoMembershipMap, PromoMembershipMapCollection>
	{	   
		public PromoMembershipMapCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>PromoMembershipMapCollection</returns>
		public PromoMembershipMapCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                PromoMembershipMap o = this[i];
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
	/// This is an ActiveRecord class which wraps the PromoMembershipMap table.
	/// </summary>
	[Serializable]
	public partial class PromoMembershipMap : ActiveRecord<PromoMembershipMap>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public PromoMembershipMap()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public PromoMembershipMap(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public PromoMembershipMap(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public PromoMembershipMap(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("PromoMembershipMap", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarPromoMembershipID = new TableSchema.TableColumn(schema);
				colvarPromoMembershipID.ColumnName = "PromoMembershipID";
				colvarPromoMembershipID.DataType = DbType.Guid;
				colvarPromoMembershipID.MaxLength = 0;
				colvarPromoMembershipID.AutoIncrement = false;
				colvarPromoMembershipID.IsNullable = false;
				colvarPromoMembershipID.IsPrimaryKey = true;
				colvarPromoMembershipID.IsForeignKey = false;
				colvarPromoMembershipID.IsReadOnly = false;
				
						colvarPromoMembershipID.DefaultSetting = @"(newid())";
				colvarPromoMembershipID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPromoMembershipID);
				
				TableSchema.TableColumn colvarMembershipPrice = new TableSchema.TableColumn(schema);
				colvarMembershipPrice.ColumnName = "MembershipPrice";
				colvarMembershipPrice.DataType = DbType.Currency;
				colvarMembershipPrice.MaxLength = 0;
				colvarMembershipPrice.AutoIncrement = false;
				colvarMembershipPrice.IsNullable = false;
				colvarMembershipPrice.IsPrimaryKey = false;
				colvarMembershipPrice.IsForeignKey = false;
				colvarMembershipPrice.IsReadOnly = false;
				colvarMembershipPrice.DefaultSetting = @"";
				colvarMembershipPrice.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMembershipPrice);
				
				TableSchema.TableColumn colvarMembershipDiscount = new TableSchema.TableColumn(schema);
				colvarMembershipDiscount.ColumnName = "MembershipDiscount";
				colvarMembershipDiscount.DataType = DbType.Decimal;
				colvarMembershipDiscount.MaxLength = 0;
				colvarMembershipDiscount.AutoIncrement = false;
				colvarMembershipDiscount.IsNullable = false;
				colvarMembershipDiscount.IsPrimaryKey = false;
				colvarMembershipDiscount.IsForeignKey = false;
				colvarMembershipDiscount.IsReadOnly = false;
				
						colvarMembershipDiscount.DefaultSetting = @"((0))";
				colvarMembershipDiscount.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMembershipDiscount);
				
				TableSchema.TableColumn colvarUseMembershipPrice = new TableSchema.TableColumn(schema);
				colvarUseMembershipPrice.ColumnName = "UseMembershipPrice";
				colvarUseMembershipPrice.DataType = DbType.Boolean;
				colvarUseMembershipPrice.MaxLength = 0;
				colvarUseMembershipPrice.AutoIncrement = false;
				colvarUseMembershipPrice.IsNullable = false;
				colvarUseMembershipPrice.IsPrimaryKey = false;
				colvarUseMembershipPrice.IsForeignKey = false;
				colvarUseMembershipPrice.IsReadOnly = false;
				
						colvarUseMembershipPrice.DefaultSetting = @"((1))";
				colvarUseMembershipPrice.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUseMembershipPrice);
				
				TableSchema.TableColumn colvarPromoCampaignHdrID = new TableSchema.TableColumn(schema);
				colvarPromoCampaignHdrID.ColumnName = "PromoCampaignHdrID";
				colvarPromoCampaignHdrID.DataType = DbType.Int32;
				colvarPromoCampaignHdrID.MaxLength = 0;
				colvarPromoCampaignHdrID.AutoIncrement = false;
				colvarPromoCampaignHdrID.IsNullable = false;
				colvarPromoCampaignHdrID.IsPrimaryKey = false;
				colvarPromoCampaignHdrID.IsForeignKey = false;
				colvarPromoCampaignHdrID.IsReadOnly = false;
				colvarPromoCampaignHdrID.DefaultSetting = @"";
				colvarPromoCampaignHdrID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPromoCampaignHdrID);
				
				TableSchema.TableColumn colvarMembershipGroupID = new TableSchema.TableColumn(schema);
				colvarMembershipGroupID.ColumnName = "MembershipGroupID";
				colvarMembershipGroupID.DataType = DbType.Int32;
				colvarMembershipGroupID.MaxLength = 0;
				colvarMembershipGroupID.AutoIncrement = false;
				colvarMembershipGroupID.IsNullable = false;
				colvarMembershipGroupID.IsPrimaryKey = false;
				colvarMembershipGroupID.IsForeignKey = false;
				colvarMembershipGroupID.IsReadOnly = false;
				colvarMembershipGroupID.DefaultSetting = @"";
				colvarMembershipGroupID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMembershipGroupID);
				
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
				
				TableSchema.TableColumn colvarDeleted = new TableSchema.TableColumn(schema);
				colvarDeleted.ColumnName = "Deleted";
				colvarDeleted.DataType = DbType.Boolean;
				colvarDeleted.MaxLength = 0;
				colvarDeleted.AutoIncrement = false;
				colvarDeleted.IsNullable = true;
				colvarDeleted.IsPrimaryKey = false;
				colvarDeleted.IsForeignKey = false;
				colvarDeleted.IsReadOnly = false;
				
						colvarDeleted.DefaultSetting = @"((0))";
				colvarDeleted.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDeleted);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["PowerPOS"].AddSchema("PromoMembershipMap",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("PromoMembershipID")]
		[Bindable(true)]
		public Guid PromoMembershipID 
		{
			get { return GetColumnValue<Guid>(Columns.PromoMembershipID); }
			set { SetColumnValue(Columns.PromoMembershipID, value); }
		}
		  
		[XmlAttribute("MembershipPrice")]
		[Bindable(true)]
		public decimal MembershipPrice 
		{
			get { return GetColumnValue<decimal>(Columns.MembershipPrice); }
			set { SetColumnValue(Columns.MembershipPrice, value); }
		}
		  
		[XmlAttribute("MembershipDiscount")]
		[Bindable(true)]
		public decimal MembershipDiscount 
		{
			get { return GetColumnValue<decimal>(Columns.MembershipDiscount); }
			set { SetColumnValue(Columns.MembershipDiscount, value); }
		}
		  
		[XmlAttribute("UseMembershipPrice")]
		[Bindable(true)]
		public bool UseMembershipPrice 
		{
			get { return GetColumnValue<bool>(Columns.UseMembershipPrice); }
			set { SetColumnValue(Columns.UseMembershipPrice, value); }
		}
		  
		[XmlAttribute("PromoCampaignHdrID")]
		[Bindable(true)]
		public int PromoCampaignHdrID 
		{
			get { return GetColumnValue<int>(Columns.PromoCampaignHdrID); }
			set { SetColumnValue(Columns.PromoCampaignHdrID, value); }
		}
		  
		[XmlAttribute("MembershipGroupID")]
		[Bindable(true)]
		public int MembershipGroupID 
		{
			get { return GetColumnValue<int>(Columns.MembershipGroupID); }
			set { SetColumnValue(Columns.MembershipGroupID, value); }
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
		public static void Insert(Guid varPromoMembershipID,decimal varMembershipPrice,decimal varMembershipDiscount,bool varUseMembershipPrice,int varPromoCampaignHdrID,int varMembershipGroupID,DateTime? varCreatedOn,DateTime? varModifiedOn,string varCreatedBy,string varModifiedBy,bool? varDeleted)
		{
			PromoMembershipMap item = new PromoMembershipMap();
			
			item.PromoMembershipID = varPromoMembershipID;
			
			item.MembershipPrice = varMembershipPrice;
			
			item.MembershipDiscount = varMembershipDiscount;
			
			item.UseMembershipPrice = varUseMembershipPrice;
			
			item.PromoCampaignHdrID = varPromoCampaignHdrID;
			
			item.MembershipGroupID = varMembershipGroupID;
			
			item.CreatedOn = varCreatedOn;
			
			item.ModifiedOn = varModifiedOn;
			
			item.CreatedBy = varCreatedBy;
			
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
		public static void Update(Guid varPromoMembershipID,decimal varMembershipPrice,decimal varMembershipDiscount,bool varUseMembershipPrice,int varPromoCampaignHdrID,int varMembershipGroupID,DateTime? varCreatedOn,DateTime? varModifiedOn,string varCreatedBy,string varModifiedBy,bool? varDeleted)
		{
			PromoMembershipMap item = new PromoMembershipMap();
			
				item.PromoMembershipID = varPromoMembershipID;
			
				item.MembershipPrice = varMembershipPrice;
			
				item.MembershipDiscount = varMembershipDiscount;
			
				item.UseMembershipPrice = varUseMembershipPrice;
			
				item.PromoCampaignHdrID = varPromoCampaignHdrID;
			
				item.MembershipGroupID = varMembershipGroupID;
			
				item.CreatedOn = varCreatedOn;
			
				item.ModifiedOn = varModifiedOn;
			
				item.CreatedBy = varCreatedBy;
			
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
        
        
        public static TableSchema.TableColumn PromoMembershipIDColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn MembershipPriceColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn MembershipDiscountColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn UseMembershipPriceColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn PromoCampaignHdrIDColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn MembershipGroupIDColumn
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
        
        
        
        public static TableSchema.TableColumn DeletedColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string PromoMembershipID = @"PromoMembershipID";
			 public static string MembershipPrice = @"MembershipPrice";
			 public static string MembershipDiscount = @"MembershipDiscount";
			 public static string UseMembershipPrice = @"UseMembershipPrice";
			 public static string PromoCampaignHdrID = @"PromoCampaignHdrID";
			 public static string MembershipGroupID = @"MembershipGroupID";
			 public static string CreatedOn = @"CreatedOn";
			 public static string ModifiedOn = @"ModifiedOn";
			 public static string CreatedBy = @"CreatedBy";
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
