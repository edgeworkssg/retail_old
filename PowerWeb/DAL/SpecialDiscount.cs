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
	/// Strongly-typed collection for the SpecialDiscount class.
	/// </summary>
    [Serializable]
	public partial class SpecialDiscountCollection : ActiveList<SpecialDiscount, SpecialDiscountCollection>
	{	   
		public SpecialDiscountCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>SpecialDiscountCollection</returns>
		public SpecialDiscountCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                SpecialDiscount o = this[i];
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
	/// This is an ActiveRecord class which wraps the SpecialDiscounts table.
	/// </summary>
	[Serializable]
	public partial class SpecialDiscount : ActiveRecord<SpecialDiscount>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public SpecialDiscount()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public SpecialDiscount(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public SpecialDiscount(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public SpecialDiscount(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("SpecialDiscounts", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarSpecialDiscountID = new TableSchema.TableColumn(schema);
				colvarSpecialDiscountID.ColumnName = "SpecialDiscountID";
				colvarSpecialDiscountID.DataType = DbType.Guid;
				colvarSpecialDiscountID.MaxLength = 0;
				colvarSpecialDiscountID.AutoIncrement = false;
				colvarSpecialDiscountID.IsNullable = false;
				colvarSpecialDiscountID.IsPrimaryKey = false;
				colvarSpecialDiscountID.IsForeignKey = false;
				colvarSpecialDiscountID.IsReadOnly = false;
				
						colvarSpecialDiscountID.DefaultSetting = @"(newid())";
				colvarSpecialDiscountID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarSpecialDiscountID);
				
				TableSchema.TableColumn colvarDiscountName = new TableSchema.TableColumn(schema);
				colvarDiscountName.ColumnName = "DiscountName";
				colvarDiscountName.DataType = DbType.String;
				colvarDiscountName.MaxLength = 50;
				colvarDiscountName.AutoIncrement = false;
				colvarDiscountName.IsNullable = false;
				colvarDiscountName.IsPrimaryKey = true;
				colvarDiscountName.IsForeignKey = false;
				colvarDiscountName.IsReadOnly = false;
				colvarDiscountName.DefaultSetting = @"";
				colvarDiscountName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDiscountName);
				
				TableSchema.TableColumn colvarDiscountPercentage = new TableSchema.TableColumn(schema);
				colvarDiscountPercentage.ColumnName = "DiscountPercentage";
				colvarDiscountPercentage.DataType = DbType.Decimal;
				colvarDiscountPercentage.MaxLength = 0;
				colvarDiscountPercentage.AutoIncrement = false;
				colvarDiscountPercentage.IsNullable = false;
				colvarDiscountPercentage.IsPrimaryKey = false;
				colvarDiscountPercentage.IsForeignKey = false;
				colvarDiscountPercentage.IsReadOnly = false;
				colvarDiscountPercentage.DefaultSetting = @"";
				colvarDiscountPercentage.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDiscountPercentage);
				
				TableSchema.TableColumn colvarShowLabel = new TableSchema.TableColumn(schema);
				colvarShowLabel.ColumnName = "ShowLabel";
				colvarShowLabel.DataType = DbType.Boolean;
				colvarShowLabel.MaxLength = 0;
				colvarShowLabel.AutoIncrement = false;
				colvarShowLabel.IsNullable = false;
				colvarShowLabel.IsPrimaryKey = false;
				colvarShowLabel.IsForeignKey = false;
				colvarShowLabel.IsReadOnly = false;
				
						colvarShowLabel.DefaultSetting = @"((1))";
				colvarShowLabel.ForeignKeyTableName = "";
				schema.Columns.Add(colvarShowLabel);
				
				TableSchema.TableColumn colvarPriorityLevel = new TableSchema.TableColumn(schema);
				colvarPriorityLevel.ColumnName = "PriorityLevel";
				colvarPriorityLevel.DataType = DbType.Int32;
				colvarPriorityLevel.MaxLength = 0;
				colvarPriorityLevel.AutoIncrement = false;
				colvarPriorityLevel.IsNullable = false;
				colvarPriorityLevel.IsPrimaryKey = false;
				colvarPriorityLevel.IsForeignKey = false;
				colvarPriorityLevel.IsReadOnly = false;
				colvarPriorityLevel.DefaultSetting = @"";
				colvarPriorityLevel.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPriorityLevel);
				
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
				
				TableSchema.TableColumn colvarUseSPP = new TableSchema.TableColumn(schema);
				colvarUseSPP.ColumnName = "UseSPP";
				colvarUseSPP.DataType = DbType.Boolean;
				colvarUseSPP.MaxLength = 0;
				colvarUseSPP.AutoIncrement = false;
				colvarUseSPP.IsNullable = true;
				colvarUseSPP.IsPrimaryKey = false;
				colvarUseSPP.IsForeignKey = false;
				colvarUseSPP.IsReadOnly = false;
				colvarUseSPP.DefaultSetting = @"";
				colvarUseSPP.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUseSPP);
				
				TableSchema.TableColumn colvarEnabled = new TableSchema.TableColumn(schema);
				colvarEnabled.ColumnName = "Enabled";
				colvarEnabled.DataType = DbType.Boolean;
				colvarEnabled.MaxLength = 0;
				colvarEnabled.AutoIncrement = false;
				colvarEnabled.IsNullable = true;
				colvarEnabled.IsPrimaryKey = false;
				colvarEnabled.IsForeignKey = false;
				colvarEnabled.IsReadOnly = false;
				colvarEnabled.DefaultSetting = @"";
				colvarEnabled.ForeignKeyTableName = "";
				schema.Columns.Add(colvarEnabled);
				
				TableSchema.TableColumn colvarApplicableToAllItem = new TableSchema.TableColumn(schema);
				colvarApplicableToAllItem.ColumnName = "ApplicableToAllItem";
				colvarApplicableToAllItem.DataType = DbType.Boolean;
				colvarApplicableToAllItem.MaxLength = 0;
				colvarApplicableToAllItem.AutoIncrement = false;
				colvarApplicableToAllItem.IsNullable = true;
				colvarApplicableToAllItem.IsPrimaryKey = false;
				colvarApplicableToAllItem.IsForeignKey = false;
				colvarApplicableToAllItem.IsReadOnly = false;
				colvarApplicableToAllItem.DefaultSetting = @"";
				colvarApplicableToAllItem.ForeignKeyTableName = "";
				schema.Columns.Add(colvarApplicableToAllItem);
				
				TableSchema.TableColumn colvarStartDate = new TableSchema.TableColumn(schema);
				colvarStartDate.ColumnName = "StartDate";
				colvarStartDate.DataType = DbType.DateTime;
				colvarStartDate.MaxLength = 0;
				colvarStartDate.AutoIncrement = false;
				colvarStartDate.IsNullable = true;
				colvarStartDate.IsPrimaryKey = false;
				colvarStartDate.IsForeignKey = false;
				colvarStartDate.IsReadOnly = false;
				colvarStartDate.DefaultSetting = @"";
				colvarStartDate.ForeignKeyTableName = "";
				schema.Columns.Add(colvarStartDate);
				
				TableSchema.TableColumn colvarEndDate = new TableSchema.TableColumn(schema);
				colvarEndDate.ColumnName = "EndDate";
				colvarEndDate.DataType = DbType.DateTime;
				colvarEndDate.MaxLength = 0;
				colvarEndDate.AutoIncrement = false;
				colvarEndDate.IsNullable = true;
				colvarEndDate.IsPrimaryKey = false;
				colvarEndDate.IsForeignKey = false;
				colvarEndDate.IsReadOnly = false;
				colvarEndDate.DefaultSetting = @"";
				colvarEndDate.ForeignKeyTableName = "";
				schema.Columns.Add(colvarEndDate);
				
				TableSchema.TableColumn colvarMinimumSpending = new TableSchema.TableColumn(schema);
				colvarMinimumSpending.ColumnName = "MinimumSpending";
				colvarMinimumSpending.DataType = DbType.Decimal;
				colvarMinimumSpending.MaxLength = 0;
				colvarMinimumSpending.AutoIncrement = false;
				colvarMinimumSpending.IsNullable = true;
				colvarMinimumSpending.IsPrimaryKey = false;
				colvarMinimumSpending.IsForeignKey = false;
				colvarMinimumSpending.IsReadOnly = false;
				colvarMinimumSpending.DefaultSetting = @"";
				colvarMinimumSpending.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMinimumSpending);
				
				TableSchema.TableColumn colvarIsBankPromo = new TableSchema.TableColumn(schema);
				colvarIsBankPromo.ColumnName = "isBankPromo";
				colvarIsBankPromo.DataType = DbType.Boolean;
				colvarIsBankPromo.MaxLength = 0;
				colvarIsBankPromo.AutoIncrement = false;
				colvarIsBankPromo.IsNullable = true;
				colvarIsBankPromo.IsPrimaryKey = false;
				colvarIsBankPromo.IsForeignKey = false;
				colvarIsBankPromo.IsReadOnly = false;
				colvarIsBankPromo.DefaultSetting = @"";
				colvarIsBankPromo.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIsBankPromo);
				
				TableSchema.TableColumn colvarDiscountLabel = new TableSchema.TableColumn(schema);
				colvarDiscountLabel.ColumnName = "DiscountLabel";
				colvarDiscountLabel.DataType = DbType.AnsiString;
				colvarDiscountLabel.MaxLength = 50;
				colvarDiscountLabel.AutoIncrement = false;
				colvarDiscountLabel.IsNullable = true;
				colvarDiscountLabel.IsPrimaryKey = false;
				colvarDiscountLabel.IsForeignKey = false;
				colvarDiscountLabel.IsReadOnly = false;
				colvarDiscountLabel.DefaultSetting = @"";
				colvarDiscountLabel.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDiscountLabel);
				
				TableSchema.TableColumn colvarAssignedOutlet = new TableSchema.TableColumn(schema);
				colvarAssignedOutlet.ColumnName = "AssignedOutlet";
				colvarAssignedOutlet.DataType = DbType.AnsiString;
				colvarAssignedOutlet.MaxLength = -1;
				colvarAssignedOutlet.AutoIncrement = false;
				colvarAssignedOutlet.IsNullable = true;
				colvarAssignedOutlet.IsPrimaryKey = false;
				colvarAssignedOutlet.IsForeignKey = false;
				colvarAssignedOutlet.IsReadOnly = false;
				colvarAssignedOutlet.DefaultSetting = @"";
				colvarAssignedOutlet.ForeignKeyTableName = "";
				schema.Columns.Add(colvarAssignedOutlet);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["PowerPOS"].AddSchema("SpecialDiscounts",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("SpecialDiscountID")]
		[Bindable(true)]
		public Guid SpecialDiscountID 
		{
			get { return GetColumnValue<Guid>(Columns.SpecialDiscountID); }
			set { SetColumnValue(Columns.SpecialDiscountID, value); }
		}
		  
		[XmlAttribute("DiscountName")]
		[Bindable(true)]
		public string DiscountName 
		{
			get { return GetColumnValue<string>(Columns.DiscountName); }
			set { SetColumnValue(Columns.DiscountName, value); }
		}
		  
		[XmlAttribute("DiscountPercentage")]
		[Bindable(true)]
		public decimal DiscountPercentage 
		{
			get { return GetColumnValue<decimal>(Columns.DiscountPercentage); }
			set { SetColumnValue(Columns.DiscountPercentage, value); }
		}
		  
		[XmlAttribute("ShowLabel")]
		[Bindable(true)]
		public bool ShowLabel 
		{
			get { return GetColumnValue<bool>(Columns.ShowLabel); }
			set { SetColumnValue(Columns.ShowLabel, value); }
		}
		  
		[XmlAttribute("PriorityLevel")]
		[Bindable(true)]
		public int PriorityLevel 
		{
			get { return GetColumnValue<int>(Columns.PriorityLevel); }
			set { SetColumnValue(Columns.PriorityLevel, value); }
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
		  
		[XmlAttribute("UseSPP")]
		[Bindable(true)]
		public bool? UseSPP 
		{
			get { return GetColumnValue<bool?>(Columns.UseSPP); }
			set { SetColumnValue(Columns.UseSPP, value); }
		}
		  
		[XmlAttribute("Enabled")]
		[Bindable(true)]
		public bool? Enabled 
		{
			get { return GetColumnValue<bool?>(Columns.Enabled); }
			set { SetColumnValue(Columns.Enabled, value); }
		}
		  
		[XmlAttribute("ApplicableToAllItem")]
		[Bindable(true)]
		public bool? ApplicableToAllItem 
		{
			get { return GetColumnValue<bool?>(Columns.ApplicableToAllItem); }
			set { SetColumnValue(Columns.ApplicableToAllItem, value); }
		}
		  
		[XmlAttribute("StartDate")]
		[Bindable(true)]
		public DateTime? StartDate 
		{
			get { return GetColumnValue<DateTime?>(Columns.StartDate); }
			set { SetColumnValue(Columns.StartDate, value); }
		}
		  
		[XmlAttribute("EndDate")]
		[Bindable(true)]
		public DateTime? EndDate 
		{
			get { return GetColumnValue<DateTime?>(Columns.EndDate); }
			set { SetColumnValue(Columns.EndDate, value); }
		}
		  
		[XmlAttribute("MinimumSpending")]
		[Bindable(true)]
		public decimal? MinimumSpending 
		{
			get { return GetColumnValue<decimal?>(Columns.MinimumSpending); }
			set { SetColumnValue(Columns.MinimumSpending, value); }
		}
		  
		[XmlAttribute("IsBankPromo")]
		[Bindable(true)]
		public bool? IsBankPromo 
		{
			get { return GetColumnValue<bool?>(Columns.IsBankPromo); }
			set { SetColumnValue(Columns.IsBankPromo, value); }
		}
		  
		[XmlAttribute("DiscountLabel")]
		[Bindable(true)]
		public string DiscountLabel 
		{
			get { return GetColumnValue<string>(Columns.DiscountLabel); }
			set { SetColumnValue(Columns.DiscountLabel, value); }
		}
		  
		[XmlAttribute("AssignedOutlet")]
		[Bindable(true)]
		public string AssignedOutlet 
		{
			get { return GetColumnValue<string>(Columns.AssignedOutlet); }
			set { SetColumnValue(Columns.AssignedOutlet, value); }
		}
		
		#endregion
		
		
		#region PrimaryKey Methods		
		
        protected override void SetPrimaryKey(object oValue)
        {
            base.SetPrimaryKey(oValue);
            
            SetPKValues();
        }
        
		
		public PowerPOS.SpecialDiscountDetailCollection SpecialDiscountDetailRecords()
		{
			return new PowerPOS.SpecialDiscountDetailCollection().Where(SpecialDiscountDetail.Columns.DiscountName, DiscountName).Load();
		}
		#endregion
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(Guid varSpecialDiscountID,string varDiscountName,decimal varDiscountPercentage,bool varShowLabel,int varPriorityLevel,bool? varDeleted,DateTime? varCreatedOn,string varCreatedBy,DateTime? varModifiedOn,string varModifiedBy,bool? varUseSPP,bool? varEnabled,bool? varApplicableToAllItem,DateTime? varStartDate,DateTime? varEndDate,decimal? varMinimumSpending,bool? varIsBankPromo,string varDiscountLabel,string varAssignedOutlet)
		{
			SpecialDiscount item = new SpecialDiscount();
			
			item.SpecialDiscountID = varSpecialDiscountID;
			
			item.DiscountName = varDiscountName;
			
			item.DiscountPercentage = varDiscountPercentage;
			
			item.ShowLabel = varShowLabel;
			
			item.PriorityLevel = varPriorityLevel;
			
			item.Deleted = varDeleted;
			
			item.CreatedOn = varCreatedOn;
			
			item.CreatedBy = varCreatedBy;
			
			item.ModifiedOn = varModifiedOn;
			
			item.ModifiedBy = varModifiedBy;
			
			item.UseSPP = varUseSPP;
			
			item.Enabled = varEnabled;
			
			item.ApplicableToAllItem = varApplicableToAllItem;
			
			item.StartDate = varStartDate;
			
			item.EndDate = varEndDate;
			
			item.MinimumSpending = varMinimumSpending;
			
			item.IsBankPromo = varIsBankPromo;
			
			item.DiscountLabel = varDiscountLabel;
			
			item.AssignedOutlet = varAssignedOutlet;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(Guid varSpecialDiscountID,string varDiscountName,decimal varDiscountPercentage,bool varShowLabel,int varPriorityLevel,bool? varDeleted,DateTime? varCreatedOn,string varCreatedBy,DateTime? varModifiedOn,string varModifiedBy,bool? varUseSPP,bool? varEnabled,bool? varApplicableToAllItem,DateTime? varStartDate,DateTime? varEndDate,decimal? varMinimumSpending,bool? varIsBankPromo,string varDiscountLabel,string varAssignedOutlet)
		{
			SpecialDiscount item = new SpecialDiscount();
			
				item.SpecialDiscountID = varSpecialDiscountID;
			
				item.DiscountName = varDiscountName;
			
				item.DiscountPercentage = varDiscountPercentage;
			
				item.ShowLabel = varShowLabel;
			
				item.PriorityLevel = varPriorityLevel;
			
				item.Deleted = varDeleted;
			
				item.CreatedOn = varCreatedOn;
			
				item.CreatedBy = varCreatedBy;
			
				item.ModifiedOn = varModifiedOn;
			
				item.ModifiedBy = varModifiedBy;
			
				item.UseSPP = varUseSPP;
			
				item.Enabled = varEnabled;
			
				item.ApplicableToAllItem = varApplicableToAllItem;
			
				item.StartDate = varStartDate;
			
				item.EndDate = varEndDate;
			
				item.MinimumSpending = varMinimumSpending;
			
				item.IsBankPromo = varIsBankPromo;
			
				item.DiscountLabel = varDiscountLabel;
			
				item.AssignedOutlet = varAssignedOutlet;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn SpecialDiscountIDColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn DiscountNameColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn DiscountPercentageColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn ShowLabelColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn PriorityLevelColumn
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
        
        
        
        public static TableSchema.TableColumn CreatedByColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedOnColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedByColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn UseSPPColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        public static TableSchema.TableColumn EnabledColumn
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        public static TableSchema.TableColumn ApplicableToAllItemColumn
        {
            get { return Schema.Columns[12]; }
        }
        
        
        
        public static TableSchema.TableColumn StartDateColumn
        {
            get { return Schema.Columns[13]; }
        }
        
        
        
        public static TableSchema.TableColumn EndDateColumn
        {
            get { return Schema.Columns[14]; }
        }
        
        
        
        public static TableSchema.TableColumn MinimumSpendingColumn
        {
            get { return Schema.Columns[15]; }
        }
        
        
        
        public static TableSchema.TableColumn IsBankPromoColumn
        {
            get { return Schema.Columns[16]; }
        }
        
        
        
        public static TableSchema.TableColumn DiscountLabelColumn
        {
            get { return Schema.Columns[17]; }
        }
        
        
        
        public static TableSchema.TableColumn AssignedOutletColumn
        {
            get { return Schema.Columns[18]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string SpecialDiscountID = @"SpecialDiscountID";
			 public static string DiscountName = @"DiscountName";
			 public static string DiscountPercentage = @"DiscountPercentage";
			 public static string ShowLabel = @"ShowLabel";
			 public static string PriorityLevel = @"PriorityLevel";
			 public static string Deleted = @"Deleted";
			 public static string CreatedOn = @"CreatedOn";
			 public static string CreatedBy = @"CreatedBy";
			 public static string ModifiedOn = @"ModifiedOn";
			 public static string ModifiedBy = @"ModifiedBy";
			 public static string UseSPP = @"UseSPP";
			 public static string Enabled = @"Enabled";
			 public static string ApplicableToAllItem = @"ApplicableToAllItem";
			 public static string StartDate = @"StartDate";
			 public static string EndDate = @"EndDate";
			 public static string MinimumSpending = @"MinimumSpending";
			 public static string IsBankPromo = @"isBankPromo";
			 public static string DiscountLabel = @"DiscountLabel";
			 public static string AssignedOutlet = @"AssignedOutlet";
						
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
