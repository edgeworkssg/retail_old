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
	/// Strongly-typed collection for the ScheduledDiscount class.
	/// </summary>
    [Serializable]
	public partial class ScheduledDiscountCollection : ActiveList<ScheduledDiscount, ScheduledDiscountCollection>
	{	   
		public ScheduledDiscountCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>ScheduledDiscountCollection</returns>
		public ScheduledDiscountCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                ScheduledDiscount o = this[i];
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
	/// This is an ActiveRecord class which wraps the ScheduledDiscount table.
	/// </summary>
	[Serializable]
	public partial class ScheduledDiscount : ActiveRecord<ScheduledDiscount>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public ScheduledDiscount()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public ScheduledDiscount(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public ScheduledDiscount(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public ScheduledDiscount(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("ScheduledDiscount", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarScheduleDiscountID = new TableSchema.TableColumn(schema);
				colvarScheduleDiscountID.ColumnName = "ScheduleDiscountID";
				colvarScheduleDiscountID.DataType = DbType.Guid;
				colvarScheduleDiscountID.MaxLength = 0;
				colvarScheduleDiscountID.AutoIncrement = false;
				colvarScheduleDiscountID.IsNullable = false;
				colvarScheduleDiscountID.IsPrimaryKey = true;
				colvarScheduleDiscountID.IsForeignKey = false;
				colvarScheduleDiscountID.IsReadOnly = false;
				
						colvarScheduleDiscountID.DefaultSetting = @"(newid())";
				colvarScheduleDiscountID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarScheduleDiscountID);
				
				TableSchema.TableColumn colvarDiscountName = new TableSchema.TableColumn(schema);
				colvarDiscountName.ColumnName = "DiscountName";
				colvarDiscountName.DataType = DbType.AnsiString;
				colvarDiscountName.MaxLength = 50;
				colvarDiscountName.AutoIncrement = false;
				colvarDiscountName.IsNullable = false;
				colvarDiscountName.IsPrimaryKey = false;
				colvarDiscountName.IsForeignKey = false;
				colvarDiscountName.IsReadOnly = false;
				colvarDiscountName.DefaultSetting = @"";
				colvarDiscountName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDiscountName);
				
				TableSchema.TableColumn colvarStartDate = new TableSchema.TableColumn(schema);
				colvarStartDate.ColumnName = "StartDate";
				colvarStartDate.DataType = DbType.DateTime;
				colvarStartDate.MaxLength = 0;
				colvarStartDate.AutoIncrement = false;
				colvarStartDate.IsNullable = false;
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
				colvarEndDate.IsNullable = false;
				colvarEndDate.IsPrimaryKey = false;
				colvarEndDate.IsForeignKey = false;
				colvarEndDate.IsReadOnly = false;
				colvarEndDate.DefaultSetting = @"";
				colvarEndDate.ForeignKeyTableName = "";
				schema.Columns.Add(colvarEndDate);
				
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
				
				TableSchema.TableColumn colvarDiscountedPrice = new TableSchema.TableColumn(schema);
				colvarDiscountedPrice.ColumnName = "DiscountedPrice";
				colvarDiscountedPrice.DataType = DbType.Currency;
				colvarDiscountedPrice.MaxLength = 0;
				colvarDiscountedPrice.AutoIncrement = false;
				colvarDiscountedPrice.IsNullable = false;
				colvarDiscountedPrice.IsPrimaryKey = false;
				colvarDiscountedPrice.IsForeignKey = false;
				colvarDiscountedPrice.IsReadOnly = false;
				colvarDiscountedPrice.DefaultSetting = @"";
				colvarDiscountedPrice.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDiscountedPrice);
				
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
				
				TableSchema.TableColumn colvarMembershipDiscountPercentage = new TableSchema.TableColumn(schema);
				colvarMembershipDiscountPercentage.ColumnName = "MembershipDiscountPercentage";
				colvarMembershipDiscountPercentage.DataType = DbType.Decimal;
				colvarMembershipDiscountPercentage.MaxLength = 0;
				colvarMembershipDiscountPercentage.AutoIncrement = false;
				colvarMembershipDiscountPercentage.IsNullable = false;
				colvarMembershipDiscountPercentage.IsPrimaryKey = false;
				colvarMembershipDiscountPercentage.IsForeignKey = false;
				colvarMembershipDiscountPercentage.IsReadOnly = false;
				colvarMembershipDiscountPercentage.DefaultSetting = @"";
				colvarMembershipDiscountPercentage.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMembershipDiscountPercentage);
				
				TableSchema.TableColumn colvarDiscountType = new TableSchema.TableColumn(schema);
				colvarDiscountType.ColumnName = "DiscountType";
				colvarDiscountType.DataType = DbType.AnsiString;
				colvarDiscountType.MaxLength = 5;
				colvarDiscountType.AutoIncrement = false;
				colvarDiscountType.IsNullable = false;
				colvarDiscountType.IsPrimaryKey = false;
				colvarDiscountType.IsForeignKey = false;
				colvarDiscountType.IsReadOnly = false;
				colvarDiscountType.DefaultSetting = @"";
				colvarDiscountType.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDiscountType);
				
				TableSchema.TableColumn colvarMembershipDiscountType = new TableSchema.TableColumn(schema);
				colvarMembershipDiscountType.ColumnName = "MembershipDiscountType";
				colvarMembershipDiscountType.DataType = DbType.AnsiString;
				colvarMembershipDiscountType.MaxLength = 5;
				colvarMembershipDiscountType.AutoIncrement = false;
				colvarMembershipDiscountType.IsNullable = false;
				colvarMembershipDiscountType.IsPrimaryKey = false;
				colvarMembershipDiscountType.IsForeignKey = false;
				colvarMembershipDiscountType.IsReadOnly = false;
				colvarMembershipDiscountType.DefaultSetting = @"";
				colvarMembershipDiscountType.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMembershipDiscountType);
				
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
				
				TableSchema.TableColumn colvarUserFld1 = new TableSchema.TableColumn(schema);
				colvarUserFld1.ColumnName = "UserFld1";
				colvarUserFld1.DataType = DbType.AnsiString;
				colvarUserFld1.MaxLength = 50;
				colvarUserFld1.AutoIncrement = false;
				colvarUserFld1.IsNullable = true;
				colvarUserFld1.IsPrimaryKey = false;
				colvarUserFld1.IsForeignKey = false;
				colvarUserFld1.IsReadOnly = false;
				colvarUserFld1.DefaultSetting = @"";
				colvarUserFld1.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserFld1);
				
				TableSchema.TableColumn colvarUserFld2 = new TableSchema.TableColumn(schema);
				colvarUserFld2.ColumnName = "UserFld2";
				colvarUserFld2.DataType = DbType.AnsiString;
				colvarUserFld2.MaxLength = 50;
				colvarUserFld2.AutoIncrement = false;
				colvarUserFld2.IsNullable = true;
				colvarUserFld2.IsPrimaryKey = false;
				colvarUserFld2.IsForeignKey = false;
				colvarUserFld2.IsReadOnly = false;
				colvarUserFld2.DefaultSetting = @"";
				colvarUserFld2.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserFld2);
				
				TableSchema.TableColumn colvarUserFld3 = new TableSchema.TableColumn(schema);
				colvarUserFld3.ColumnName = "UserFld3";
				colvarUserFld3.DataType = DbType.AnsiString;
				colvarUserFld3.MaxLength = 50;
				colvarUserFld3.AutoIncrement = false;
				colvarUserFld3.IsNullable = true;
				colvarUserFld3.IsPrimaryKey = false;
				colvarUserFld3.IsForeignKey = false;
				colvarUserFld3.IsReadOnly = false;
				colvarUserFld3.DefaultSetting = @"";
				colvarUserFld3.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserFld3);
				
				TableSchema.TableColumn colvarUserFld4 = new TableSchema.TableColumn(schema);
				colvarUserFld4.ColumnName = "UserFld4";
				colvarUserFld4.DataType = DbType.AnsiString;
				colvarUserFld4.MaxLength = 50;
				colvarUserFld4.AutoIncrement = false;
				colvarUserFld4.IsNullable = true;
				colvarUserFld4.IsPrimaryKey = false;
				colvarUserFld4.IsForeignKey = false;
				colvarUserFld4.IsReadOnly = false;
				colvarUserFld4.DefaultSetting = @"";
				colvarUserFld4.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserFld4);
				
				TableSchema.TableColumn colvarUserFld5 = new TableSchema.TableColumn(schema);
				colvarUserFld5.ColumnName = "UserFld5";
				colvarUserFld5.DataType = DbType.AnsiString;
				colvarUserFld5.MaxLength = 50;
				colvarUserFld5.AutoIncrement = false;
				colvarUserFld5.IsNullable = true;
				colvarUserFld5.IsPrimaryKey = false;
				colvarUserFld5.IsForeignKey = false;
				colvarUserFld5.IsReadOnly = false;
				colvarUserFld5.DefaultSetting = @"";
				colvarUserFld5.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserFld5);
				
				TableSchema.TableColumn colvarUserFlag1 = new TableSchema.TableColumn(schema);
				colvarUserFlag1.ColumnName = "UserFlag1";
				colvarUserFlag1.DataType = DbType.Boolean;
				colvarUserFlag1.MaxLength = 0;
				colvarUserFlag1.AutoIncrement = false;
				colvarUserFlag1.IsNullable = true;
				colvarUserFlag1.IsPrimaryKey = false;
				colvarUserFlag1.IsForeignKey = false;
				colvarUserFlag1.IsReadOnly = false;
				colvarUserFlag1.DefaultSetting = @"";
				colvarUserFlag1.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserFlag1);
				
				TableSchema.TableColumn colvarUserFlag2 = new TableSchema.TableColumn(schema);
				colvarUserFlag2.ColumnName = "UserFlag2";
				colvarUserFlag2.DataType = DbType.Boolean;
				colvarUserFlag2.MaxLength = 0;
				colvarUserFlag2.AutoIncrement = false;
				colvarUserFlag2.IsNullable = true;
				colvarUserFlag2.IsPrimaryKey = false;
				colvarUserFlag2.IsForeignKey = false;
				colvarUserFlag2.IsReadOnly = false;
				colvarUserFlag2.DefaultSetting = @"";
				colvarUserFlag2.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserFlag2);
				
				TableSchema.TableColumn colvarUserFlag3 = new TableSchema.TableColumn(schema);
				colvarUserFlag3.ColumnName = "UserFlag3";
				colvarUserFlag3.DataType = DbType.Boolean;
				colvarUserFlag3.MaxLength = 0;
				colvarUserFlag3.AutoIncrement = false;
				colvarUserFlag3.IsNullable = true;
				colvarUserFlag3.IsPrimaryKey = false;
				colvarUserFlag3.IsForeignKey = false;
				colvarUserFlag3.IsReadOnly = false;
				colvarUserFlag3.DefaultSetting = @"";
				colvarUserFlag3.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserFlag3);
				
				TableSchema.TableColumn colvarUserFlag4 = new TableSchema.TableColumn(schema);
				colvarUserFlag4.ColumnName = "UserFlag4";
				colvarUserFlag4.DataType = DbType.Boolean;
				colvarUserFlag4.MaxLength = 0;
				colvarUserFlag4.AutoIncrement = false;
				colvarUserFlag4.IsNullable = true;
				colvarUserFlag4.IsPrimaryKey = false;
				colvarUserFlag4.IsForeignKey = false;
				colvarUserFlag4.IsReadOnly = false;
				colvarUserFlag4.DefaultSetting = @"";
				colvarUserFlag4.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserFlag4);
				
				TableSchema.TableColumn colvarUserFlag5 = new TableSchema.TableColumn(schema);
				colvarUserFlag5.ColumnName = "UserFlag5";
				colvarUserFlag5.DataType = DbType.Boolean;
				colvarUserFlag5.MaxLength = 0;
				colvarUserFlag5.AutoIncrement = false;
				colvarUserFlag5.IsNullable = true;
				colvarUserFlag5.IsPrimaryKey = false;
				colvarUserFlag5.IsForeignKey = false;
				colvarUserFlag5.IsReadOnly = false;
				colvarUserFlag5.DefaultSetting = @"";
				colvarUserFlag5.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserFlag5);
				
				TableSchema.TableColumn colvarUserFloat1 = new TableSchema.TableColumn(schema);
				colvarUserFloat1.ColumnName = "UserFloat1";
				colvarUserFloat1.DataType = DbType.Currency;
				colvarUserFloat1.MaxLength = 0;
				colvarUserFloat1.AutoIncrement = false;
				colvarUserFloat1.IsNullable = true;
				colvarUserFloat1.IsPrimaryKey = false;
				colvarUserFloat1.IsForeignKey = false;
				colvarUserFloat1.IsReadOnly = false;
				colvarUserFloat1.DefaultSetting = @"";
				colvarUserFloat1.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserFloat1);
				
				TableSchema.TableColumn colvarUserFloat2 = new TableSchema.TableColumn(schema);
				colvarUserFloat2.ColumnName = "UserFloat2";
				colvarUserFloat2.DataType = DbType.Currency;
				colvarUserFloat2.MaxLength = 0;
				colvarUserFloat2.AutoIncrement = false;
				colvarUserFloat2.IsNullable = true;
				colvarUserFloat2.IsPrimaryKey = false;
				colvarUserFloat2.IsForeignKey = false;
				colvarUserFloat2.IsReadOnly = false;
				colvarUserFloat2.DefaultSetting = @"";
				colvarUserFloat2.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserFloat2);
				
				TableSchema.TableColumn colvarUserFloat3 = new TableSchema.TableColumn(schema);
				colvarUserFloat3.ColumnName = "UserFloat3";
				colvarUserFloat3.DataType = DbType.Currency;
				colvarUserFloat3.MaxLength = 0;
				colvarUserFloat3.AutoIncrement = false;
				colvarUserFloat3.IsNullable = true;
				colvarUserFloat3.IsPrimaryKey = false;
				colvarUserFloat3.IsForeignKey = false;
				colvarUserFloat3.IsReadOnly = false;
				colvarUserFloat3.DefaultSetting = @"";
				colvarUserFloat3.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserFloat3);
				
				TableSchema.TableColumn colvarUserFloat4 = new TableSchema.TableColumn(schema);
				colvarUserFloat4.ColumnName = "UserFloat4";
				colvarUserFloat4.DataType = DbType.Currency;
				colvarUserFloat4.MaxLength = 0;
				colvarUserFloat4.AutoIncrement = false;
				colvarUserFloat4.IsNullable = true;
				colvarUserFloat4.IsPrimaryKey = false;
				colvarUserFloat4.IsForeignKey = false;
				colvarUserFloat4.IsReadOnly = false;
				colvarUserFloat4.DefaultSetting = @"";
				colvarUserFloat4.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserFloat4);
				
				TableSchema.TableColumn colvarUserFloat5 = new TableSchema.TableColumn(schema);
				colvarUserFloat5.ColumnName = "UserFloat5";
				colvarUserFloat5.DataType = DbType.Currency;
				colvarUserFloat5.MaxLength = 0;
				colvarUserFloat5.AutoIncrement = false;
				colvarUserFloat5.IsNullable = true;
				colvarUserFloat5.IsPrimaryKey = false;
				colvarUserFloat5.IsForeignKey = false;
				colvarUserFloat5.IsReadOnly = false;
				colvarUserFloat5.DefaultSetting = @"";
				colvarUserFloat5.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserFloat5);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["PowerPOS"].AddSchema("ScheduledDiscount",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("ScheduleDiscountID")]
		[Bindable(true)]
		public Guid ScheduleDiscountID 
		{
			get { return GetColumnValue<Guid>(Columns.ScheduleDiscountID); }
			set { SetColumnValue(Columns.ScheduleDiscountID, value); }
		}
		  
		[XmlAttribute("DiscountName")]
		[Bindable(true)]
		public string DiscountName 
		{
			get { return GetColumnValue<string>(Columns.DiscountName); }
			set { SetColumnValue(Columns.DiscountName, value); }
		}
		  
		[XmlAttribute("StartDate")]
		[Bindable(true)]
		public DateTime StartDate 
		{
			get { return GetColumnValue<DateTime>(Columns.StartDate); }
			set { SetColumnValue(Columns.StartDate, value); }
		}
		  
		[XmlAttribute("EndDate")]
		[Bindable(true)]
		public DateTime EndDate 
		{
			get { return GetColumnValue<DateTime>(Columns.EndDate); }
			set { SetColumnValue(Columns.EndDate, value); }
		}
		  
		[XmlAttribute("ItemNo")]
		[Bindable(true)]
		public string ItemNo 
		{
			get { return GetColumnValue<string>(Columns.ItemNo); }
			set { SetColumnValue(Columns.ItemNo, value); }
		}
		  
		[XmlAttribute("DiscountedPrice")]
		[Bindable(true)]
		public decimal DiscountedPrice 
		{
			get { return GetColumnValue<decimal>(Columns.DiscountedPrice); }
			set { SetColumnValue(Columns.DiscountedPrice, value); }
		}
		  
		[XmlAttribute("MembershipPrice")]
		[Bindable(true)]
		public decimal MembershipPrice 
		{
			get { return GetColumnValue<decimal>(Columns.MembershipPrice); }
			set { SetColumnValue(Columns.MembershipPrice, value); }
		}
		  
		[XmlAttribute("DiscountPercentage")]
		[Bindable(true)]
		public decimal DiscountPercentage 
		{
			get { return GetColumnValue<decimal>(Columns.DiscountPercentage); }
			set { SetColumnValue(Columns.DiscountPercentage, value); }
		}
		  
		[XmlAttribute("MembershipDiscountPercentage")]
		[Bindable(true)]
		public decimal MembershipDiscountPercentage 
		{
			get { return GetColumnValue<decimal>(Columns.MembershipDiscountPercentage); }
			set { SetColumnValue(Columns.MembershipDiscountPercentage, value); }
		}
		  
		[XmlAttribute("DiscountType")]
		[Bindable(true)]
		public string DiscountType 
		{
			get { return GetColumnValue<string>(Columns.DiscountType); }
			set { SetColumnValue(Columns.DiscountType, value); }
		}
		  
		[XmlAttribute("MembershipDiscountType")]
		[Bindable(true)]
		public string MembershipDiscountType 
		{
			get { return GetColumnValue<string>(Columns.MembershipDiscountType); }
			set { SetColumnValue(Columns.MembershipDiscountType, value); }
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
		  
		[XmlAttribute("UserFld1")]
		[Bindable(true)]
		public string UserFld1 
		{
			get { return GetColumnValue<string>(Columns.UserFld1); }
			set { SetColumnValue(Columns.UserFld1, value); }
		}
		  
		[XmlAttribute("UserFld2")]
		[Bindable(true)]
		public string UserFld2 
		{
			get { return GetColumnValue<string>(Columns.UserFld2); }
			set { SetColumnValue(Columns.UserFld2, value); }
		}
		  
		[XmlAttribute("UserFld3")]
		[Bindable(true)]
		public string UserFld3 
		{
			get { return GetColumnValue<string>(Columns.UserFld3); }
			set { SetColumnValue(Columns.UserFld3, value); }
		}
		  
		[XmlAttribute("UserFld4")]
		[Bindable(true)]
		public string UserFld4 
		{
			get { return GetColumnValue<string>(Columns.UserFld4); }
			set { SetColumnValue(Columns.UserFld4, value); }
		}
		  
		[XmlAttribute("UserFld5")]
		[Bindable(true)]
		public string UserFld5 
		{
			get { return GetColumnValue<string>(Columns.UserFld5); }
			set { SetColumnValue(Columns.UserFld5, value); }
		}
		  
		[XmlAttribute("UserFlag1")]
		[Bindable(true)]
		public bool? UserFlag1 
		{
			get { return GetColumnValue<bool?>(Columns.UserFlag1); }
			set { SetColumnValue(Columns.UserFlag1, value); }
		}
		  
		[XmlAttribute("UserFlag2")]
		[Bindable(true)]
		public bool? UserFlag2 
		{
			get { return GetColumnValue<bool?>(Columns.UserFlag2); }
			set { SetColumnValue(Columns.UserFlag2, value); }
		}
		  
		[XmlAttribute("UserFlag3")]
		[Bindable(true)]
		public bool? UserFlag3 
		{
			get { return GetColumnValue<bool?>(Columns.UserFlag3); }
			set { SetColumnValue(Columns.UserFlag3, value); }
		}
		  
		[XmlAttribute("UserFlag4")]
		[Bindable(true)]
		public bool? UserFlag4 
		{
			get { return GetColumnValue<bool?>(Columns.UserFlag4); }
			set { SetColumnValue(Columns.UserFlag4, value); }
		}
		  
		[XmlAttribute("UserFlag5")]
		[Bindable(true)]
		public bool? UserFlag5 
		{
			get { return GetColumnValue<bool?>(Columns.UserFlag5); }
			set { SetColumnValue(Columns.UserFlag5, value); }
		}
		  
		[XmlAttribute("UserFloat1")]
		[Bindable(true)]
		public decimal? UserFloat1 
		{
			get { return GetColumnValue<decimal?>(Columns.UserFloat1); }
			set { SetColumnValue(Columns.UserFloat1, value); }
		}
		  
		[XmlAttribute("UserFloat2")]
		[Bindable(true)]
		public decimal? UserFloat2 
		{
			get { return GetColumnValue<decimal?>(Columns.UserFloat2); }
			set { SetColumnValue(Columns.UserFloat2, value); }
		}
		  
		[XmlAttribute("UserFloat3")]
		[Bindable(true)]
		public decimal? UserFloat3 
		{
			get { return GetColumnValue<decimal?>(Columns.UserFloat3); }
			set { SetColumnValue(Columns.UserFloat3, value); }
		}
		  
		[XmlAttribute("UserFloat4")]
		[Bindable(true)]
		public decimal? UserFloat4 
		{
			get { return GetColumnValue<decimal?>(Columns.UserFloat4); }
			set { SetColumnValue(Columns.UserFloat4, value); }
		}
		  
		[XmlAttribute("UserFloat5")]
		[Bindable(true)]
		public decimal? UserFloat5 
		{
			get { return GetColumnValue<decimal?>(Columns.UserFloat5); }
			set { SetColumnValue(Columns.UserFloat5, value); }
		}
		
		#endregion
		
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(Guid varScheduleDiscountID,string varDiscountName,DateTime varStartDate,DateTime varEndDate,string varItemNo,decimal varDiscountedPrice,decimal varMembershipPrice,decimal varDiscountPercentage,decimal varMembershipDiscountPercentage,string varDiscountType,string varMembershipDiscountType,DateTime? varCreatedOn,string varCreatedBy,DateTime? varModifiedOn,string varModifiedBy,bool? varDeleted,string varUserFld1,string varUserFld2,string varUserFld3,string varUserFld4,string varUserFld5,bool? varUserFlag1,bool? varUserFlag2,bool? varUserFlag3,bool? varUserFlag4,bool? varUserFlag5,decimal? varUserFloat1,decimal? varUserFloat2,decimal? varUserFloat3,decimal? varUserFloat4,decimal? varUserFloat5)
		{
			ScheduledDiscount item = new ScheduledDiscount();
			
			item.ScheduleDiscountID = varScheduleDiscountID;
			
			item.DiscountName = varDiscountName;
			
			item.StartDate = varStartDate;
			
			item.EndDate = varEndDate;
			
			item.ItemNo = varItemNo;
			
			item.DiscountedPrice = varDiscountedPrice;
			
			item.MembershipPrice = varMembershipPrice;
			
			item.DiscountPercentage = varDiscountPercentage;
			
			item.MembershipDiscountPercentage = varMembershipDiscountPercentage;
			
			item.DiscountType = varDiscountType;
			
			item.MembershipDiscountType = varMembershipDiscountType;
			
			item.CreatedOn = varCreatedOn;
			
			item.CreatedBy = varCreatedBy;
			
			item.ModifiedOn = varModifiedOn;
			
			item.ModifiedBy = varModifiedBy;
			
			item.Deleted = varDeleted;
			
			item.UserFld1 = varUserFld1;
			
			item.UserFld2 = varUserFld2;
			
			item.UserFld3 = varUserFld3;
			
			item.UserFld4 = varUserFld4;
			
			item.UserFld5 = varUserFld5;
			
			item.UserFlag1 = varUserFlag1;
			
			item.UserFlag2 = varUserFlag2;
			
			item.UserFlag3 = varUserFlag3;
			
			item.UserFlag4 = varUserFlag4;
			
			item.UserFlag5 = varUserFlag5;
			
			item.UserFloat1 = varUserFloat1;
			
			item.UserFloat2 = varUserFloat2;
			
			item.UserFloat3 = varUserFloat3;
			
			item.UserFloat4 = varUserFloat4;
			
			item.UserFloat5 = varUserFloat5;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(Guid varScheduleDiscountID,string varDiscountName,DateTime varStartDate,DateTime varEndDate,string varItemNo,decimal varDiscountedPrice,decimal varMembershipPrice,decimal varDiscountPercentage,decimal varMembershipDiscountPercentage,string varDiscountType,string varMembershipDiscountType,DateTime? varCreatedOn,string varCreatedBy,DateTime? varModifiedOn,string varModifiedBy,bool? varDeleted,string varUserFld1,string varUserFld2,string varUserFld3,string varUserFld4,string varUserFld5,bool? varUserFlag1,bool? varUserFlag2,bool? varUserFlag3,bool? varUserFlag4,bool? varUserFlag5,decimal? varUserFloat1,decimal? varUserFloat2,decimal? varUserFloat3,decimal? varUserFloat4,decimal? varUserFloat5)
		{
			ScheduledDiscount item = new ScheduledDiscount();
			
				item.ScheduleDiscountID = varScheduleDiscountID;
			
				item.DiscountName = varDiscountName;
			
				item.StartDate = varStartDate;
			
				item.EndDate = varEndDate;
			
				item.ItemNo = varItemNo;
			
				item.DiscountedPrice = varDiscountedPrice;
			
				item.MembershipPrice = varMembershipPrice;
			
				item.DiscountPercentage = varDiscountPercentage;
			
				item.MembershipDiscountPercentage = varMembershipDiscountPercentage;
			
				item.DiscountType = varDiscountType;
			
				item.MembershipDiscountType = varMembershipDiscountType;
			
				item.CreatedOn = varCreatedOn;
			
				item.CreatedBy = varCreatedBy;
			
				item.ModifiedOn = varModifiedOn;
			
				item.ModifiedBy = varModifiedBy;
			
				item.Deleted = varDeleted;
			
				item.UserFld1 = varUserFld1;
			
				item.UserFld2 = varUserFld2;
			
				item.UserFld3 = varUserFld3;
			
				item.UserFld4 = varUserFld4;
			
				item.UserFld5 = varUserFld5;
			
				item.UserFlag1 = varUserFlag1;
			
				item.UserFlag2 = varUserFlag2;
			
				item.UserFlag3 = varUserFlag3;
			
				item.UserFlag4 = varUserFlag4;
			
				item.UserFlag5 = varUserFlag5;
			
				item.UserFloat1 = varUserFloat1;
			
				item.UserFloat2 = varUserFloat2;
			
				item.UserFloat3 = varUserFloat3;
			
				item.UserFloat4 = varUserFloat4;
			
				item.UserFloat5 = varUserFloat5;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn ScheduleDiscountIDColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn DiscountNameColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn StartDateColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn EndDateColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn ItemNoColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn DiscountedPriceColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn MembershipPriceColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn DiscountPercentageColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn MembershipDiscountPercentageColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn DiscountTypeColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn MembershipDiscountTypeColumn
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
        
        
        
        public static TableSchema.TableColumn UserFld1Column
        {
            get { return Schema.Columns[16]; }
        }
        
        
        
        public static TableSchema.TableColumn UserFld2Column
        {
            get { return Schema.Columns[17]; }
        }
        
        
        
        public static TableSchema.TableColumn UserFld3Column
        {
            get { return Schema.Columns[18]; }
        }
        
        
        
        public static TableSchema.TableColumn UserFld4Column
        {
            get { return Schema.Columns[19]; }
        }
        
        
        
        public static TableSchema.TableColumn UserFld5Column
        {
            get { return Schema.Columns[20]; }
        }
        
        
        
        public static TableSchema.TableColumn UserFlag1Column
        {
            get { return Schema.Columns[21]; }
        }
        
        
        
        public static TableSchema.TableColumn UserFlag2Column
        {
            get { return Schema.Columns[22]; }
        }
        
        
        
        public static TableSchema.TableColumn UserFlag3Column
        {
            get { return Schema.Columns[23]; }
        }
        
        
        
        public static TableSchema.TableColumn UserFlag4Column
        {
            get { return Schema.Columns[24]; }
        }
        
        
        
        public static TableSchema.TableColumn UserFlag5Column
        {
            get { return Schema.Columns[25]; }
        }
        
        
        
        public static TableSchema.TableColumn UserFloat1Column
        {
            get { return Schema.Columns[26]; }
        }
        
        
        
        public static TableSchema.TableColumn UserFloat2Column
        {
            get { return Schema.Columns[27]; }
        }
        
        
        
        public static TableSchema.TableColumn UserFloat3Column
        {
            get { return Schema.Columns[28]; }
        }
        
        
        
        public static TableSchema.TableColumn UserFloat4Column
        {
            get { return Schema.Columns[29]; }
        }
        
        
        
        public static TableSchema.TableColumn UserFloat5Column
        {
            get { return Schema.Columns[30]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string ScheduleDiscountID = @"ScheduleDiscountID";
			 public static string DiscountName = @"DiscountName";
			 public static string StartDate = @"StartDate";
			 public static string EndDate = @"EndDate";
			 public static string ItemNo = @"ItemNo";
			 public static string DiscountedPrice = @"DiscountedPrice";
			 public static string MembershipPrice = @"MembershipPrice";
			 public static string DiscountPercentage = @"DiscountPercentage";
			 public static string MembershipDiscountPercentage = @"MembershipDiscountPercentage";
			 public static string DiscountType = @"DiscountType";
			 public static string MembershipDiscountType = @"MembershipDiscountType";
			 public static string CreatedOn = @"CreatedOn";
			 public static string CreatedBy = @"CreatedBy";
			 public static string ModifiedOn = @"ModifiedOn";
			 public static string ModifiedBy = @"ModifiedBy";
			 public static string Deleted = @"Deleted";
			 public static string UserFld1 = @"UserFld1";
			 public static string UserFld2 = @"UserFld2";
			 public static string UserFld3 = @"UserFld3";
			 public static string UserFld4 = @"UserFld4";
			 public static string UserFld5 = @"UserFld5";
			 public static string UserFlag1 = @"UserFlag1";
			 public static string UserFlag2 = @"UserFlag2";
			 public static string UserFlag3 = @"UserFlag3";
			 public static string UserFlag4 = @"UserFlag4";
			 public static string UserFlag5 = @"UserFlag5";
			 public static string UserFloat1 = @"UserFloat1";
			 public static string UserFloat2 = @"UserFloat2";
			 public static string UserFloat3 = @"UserFloat3";
			 public static string UserFloat4 = @"UserFloat4";
			 public static string UserFloat5 = @"UserFloat5";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
