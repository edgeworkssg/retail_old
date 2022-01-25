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
	/// Strongly-typed collection for the MembershipRenewal class.
	/// </summary>
    [Serializable]
	public partial class MembershipRenewalCollection : ActiveList<MembershipRenewal, MembershipRenewalCollection>
	{	   
		public MembershipRenewalCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>MembershipRenewalCollection</returns>
		public MembershipRenewalCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                MembershipRenewal o = this[i];
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
	/// This is an ActiveRecord class which wraps the MembershipRenewal table.
	/// </summary>
	[Serializable]
	public partial class MembershipRenewal : ActiveRecord<MembershipRenewal>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public MembershipRenewal()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public MembershipRenewal(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public MembershipRenewal(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public MembershipRenewal(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("MembershipRenewal", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarRenewalID = new TableSchema.TableColumn(schema);
				colvarRenewalID.ColumnName = "renewalID";
				colvarRenewalID.DataType = DbType.Int32;
				colvarRenewalID.MaxLength = 0;
				colvarRenewalID.AutoIncrement = true;
				colvarRenewalID.IsNullable = false;
				colvarRenewalID.IsPrimaryKey = true;
				colvarRenewalID.IsForeignKey = false;
				colvarRenewalID.IsReadOnly = false;
				colvarRenewalID.DefaultSetting = @"";
				colvarRenewalID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarRenewalID);
				
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
				
				TableSchema.TableColumn colvarNewExpiryDate = new TableSchema.TableColumn(schema);
				colvarNewExpiryDate.ColumnName = "NewExpiryDate";
				colvarNewExpiryDate.DataType = DbType.DateTime;
				colvarNewExpiryDate.MaxLength = 0;
				colvarNewExpiryDate.AutoIncrement = false;
				colvarNewExpiryDate.IsNullable = false;
				colvarNewExpiryDate.IsPrimaryKey = false;
				colvarNewExpiryDate.IsForeignKey = false;
				colvarNewExpiryDate.IsReadOnly = false;
				colvarNewExpiryDate.DefaultSetting = @"";
				colvarNewExpiryDate.ForeignKeyTableName = "";
				schema.Columns.Add(colvarNewExpiryDate);
				
				TableSchema.TableColumn colvarOrderHdrId = new TableSchema.TableColumn(schema);
				colvarOrderHdrId.ColumnName = "OrderHdrId";
				colvarOrderHdrId.DataType = DbType.AnsiString;
				colvarOrderHdrId.MaxLength = 14;
				colvarOrderHdrId.AutoIncrement = false;
				colvarOrderHdrId.IsNullable = false;
				colvarOrderHdrId.IsPrimaryKey = false;
				colvarOrderHdrId.IsForeignKey = false;
				colvarOrderHdrId.IsReadOnly = false;
				colvarOrderHdrId.DefaultSetting = @"";
				colvarOrderHdrId.ForeignKeyTableName = "";
				schema.Columns.Add(colvarOrderHdrId);
				
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
				
				TableSchema.TableColumn colvarUniqueId = new TableSchema.TableColumn(schema);
				colvarUniqueId.ColumnName = "UniqueId";
				colvarUniqueId.DataType = DbType.Guid;
				colvarUniqueId.MaxLength = 0;
				colvarUniqueId.AutoIncrement = false;
				colvarUniqueId.IsNullable = false;
				colvarUniqueId.IsPrimaryKey = false;
				colvarUniqueId.IsForeignKey = false;
				colvarUniqueId.IsReadOnly = false;
				colvarUniqueId.DefaultSetting = @"";
				colvarUniqueId.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUniqueId);
				
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
				
				TableSchema.TableColumn colvarUserFld6 = new TableSchema.TableColumn(schema);
				colvarUserFld6.ColumnName = "UserFld6";
				colvarUserFld6.DataType = DbType.AnsiString;
				colvarUserFld6.MaxLength = 50;
				colvarUserFld6.AutoIncrement = false;
				colvarUserFld6.IsNullable = true;
				colvarUserFld6.IsPrimaryKey = false;
				colvarUserFld6.IsForeignKey = false;
				colvarUserFld6.IsReadOnly = false;
				colvarUserFld6.DefaultSetting = @"";
				colvarUserFld6.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserFld6);
				
				TableSchema.TableColumn colvarUserFld7 = new TableSchema.TableColumn(schema);
				colvarUserFld7.ColumnName = "UserFld7";
				colvarUserFld7.DataType = DbType.AnsiString;
				colvarUserFld7.MaxLength = 50;
				colvarUserFld7.AutoIncrement = false;
				colvarUserFld7.IsNullable = true;
				colvarUserFld7.IsPrimaryKey = false;
				colvarUserFld7.IsForeignKey = false;
				colvarUserFld7.IsReadOnly = false;
				colvarUserFld7.DefaultSetting = @"";
				colvarUserFld7.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserFld7);
				
				TableSchema.TableColumn colvarUserFld8 = new TableSchema.TableColumn(schema);
				colvarUserFld8.ColumnName = "UserFld8";
				colvarUserFld8.DataType = DbType.AnsiString;
				colvarUserFld8.MaxLength = 50;
				colvarUserFld8.AutoIncrement = false;
				colvarUserFld8.IsNullable = true;
				colvarUserFld8.IsPrimaryKey = false;
				colvarUserFld8.IsForeignKey = false;
				colvarUserFld8.IsReadOnly = false;
				colvarUserFld8.DefaultSetting = @"";
				colvarUserFld8.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserFld8);
				
				TableSchema.TableColumn colvarUserFld9 = new TableSchema.TableColumn(schema);
				colvarUserFld9.ColumnName = "UserFld9";
				colvarUserFld9.DataType = DbType.AnsiString;
				colvarUserFld9.MaxLength = 50;
				colvarUserFld9.AutoIncrement = false;
				colvarUserFld9.IsNullable = true;
				colvarUserFld9.IsPrimaryKey = false;
				colvarUserFld9.IsForeignKey = false;
				colvarUserFld9.IsReadOnly = false;
				colvarUserFld9.DefaultSetting = @"";
				colvarUserFld9.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserFld9);
				
				TableSchema.TableColumn colvarUserFld10 = new TableSchema.TableColumn(schema);
				colvarUserFld10.ColumnName = "UserFld10";
				colvarUserFld10.DataType = DbType.AnsiString;
				colvarUserFld10.MaxLength = 50;
				colvarUserFld10.AutoIncrement = false;
				colvarUserFld10.IsNullable = true;
				colvarUserFld10.IsPrimaryKey = false;
				colvarUserFld10.IsForeignKey = false;
				colvarUserFld10.IsReadOnly = false;
				colvarUserFld10.DefaultSetting = @"";
				colvarUserFld10.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserFld10);
				
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
				
				TableSchema.TableColumn colvarUserInt1 = new TableSchema.TableColumn(schema);
				colvarUserInt1.ColumnName = "UserInt1";
				colvarUserInt1.DataType = DbType.Int32;
				colvarUserInt1.MaxLength = 0;
				colvarUserInt1.AutoIncrement = false;
				colvarUserInt1.IsNullable = true;
				colvarUserInt1.IsPrimaryKey = false;
				colvarUserInt1.IsForeignKey = false;
				colvarUserInt1.IsReadOnly = false;
				colvarUserInt1.DefaultSetting = @"";
				colvarUserInt1.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserInt1);
				
				TableSchema.TableColumn colvarUserInt2 = new TableSchema.TableColumn(schema);
				colvarUserInt2.ColumnName = "UserInt2";
				colvarUserInt2.DataType = DbType.Int32;
				colvarUserInt2.MaxLength = 0;
				colvarUserInt2.AutoIncrement = false;
				colvarUserInt2.IsNullable = true;
				colvarUserInt2.IsPrimaryKey = false;
				colvarUserInt2.IsForeignKey = false;
				colvarUserInt2.IsReadOnly = false;
				colvarUserInt2.DefaultSetting = @"";
				colvarUserInt2.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserInt2);
				
				TableSchema.TableColumn colvarUserInt3 = new TableSchema.TableColumn(schema);
				colvarUserInt3.ColumnName = "UserInt3";
				colvarUserInt3.DataType = DbType.Int32;
				colvarUserInt3.MaxLength = 0;
				colvarUserInt3.AutoIncrement = false;
				colvarUserInt3.IsNullable = true;
				colvarUserInt3.IsPrimaryKey = false;
				colvarUserInt3.IsForeignKey = false;
				colvarUserInt3.IsReadOnly = false;
				colvarUserInt3.DefaultSetting = @"";
				colvarUserInt3.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserInt3);
				
				TableSchema.TableColumn colvarUserInt4 = new TableSchema.TableColumn(schema);
				colvarUserInt4.ColumnName = "UserInt4";
				colvarUserInt4.DataType = DbType.Int32;
				colvarUserInt4.MaxLength = 0;
				colvarUserInt4.AutoIncrement = false;
				colvarUserInt4.IsNullable = true;
				colvarUserInt4.IsPrimaryKey = false;
				colvarUserInt4.IsForeignKey = false;
				colvarUserInt4.IsReadOnly = false;
				colvarUserInt4.DefaultSetting = @"";
				colvarUserInt4.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserInt4);
				
				TableSchema.TableColumn colvarUserInt5 = new TableSchema.TableColumn(schema);
				colvarUserInt5.ColumnName = "UserInt5";
				colvarUserInt5.DataType = DbType.Int32;
				colvarUserInt5.MaxLength = 0;
				colvarUserInt5.AutoIncrement = false;
				colvarUserInt5.IsNullable = true;
				colvarUserInt5.IsPrimaryKey = false;
				colvarUserInt5.IsForeignKey = false;
				colvarUserInt5.IsReadOnly = false;
				colvarUserInt5.DefaultSetting = @"";
				colvarUserInt5.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserInt5);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["PowerPOS"].AddSchema("MembershipRenewal",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("RenewalID")]
		[Bindable(true)]
		public int RenewalID 
		{
			get { return GetColumnValue<int>(Columns.RenewalID); }
			set { SetColumnValue(Columns.RenewalID, value); }
		}
		  
		[XmlAttribute("MembershipNo")]
		[Bindable(true)]
		public string MembershipNo 
		{
			get { return GetColumnValue<string>(Columns.MembershipNo); }
			set { SetColumnValue(Columns.MembershipNo, value); }
		}
		  
		[XmlAttribute("NewExpiryDate")]
		[Bindable(true)]
		public DateTime NewExpiryDate 
		{
			get { return GetColumnValue<DateTime>(Columns.NewExpiryDate); }
			set { SetColumnValue(Columns.NewExpiryDate, value); }
		}
		  
		[XmlAttribute("OrderHdrId")]
		[Bindable(true)]
		public string OrderHdrId 
		{
			get { return GetColumnValue<string>(Columns.OrderHdrId); }
			set { SetColumnValue(Columns.OrderHdrId, value); }
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
		  
		[XmlAttribute("UniqueId")]
		[Bindable(true)]
		public Guid UniqueId 
		{
			get { return GetColumnValue<Guid>(Columns.UniqueId); }
			set { SetColumnValue(Columns.UniqueId, value); }
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
		  
		[XmlAttribute("UserFld6")]
		[Bindable(true)]
		public string UserFld6 
		{
			get { return GetColumnValue<string>(Columns.UserFld6); }
			set { SetColumnValue(Columns.UserFld6, value); }
		}
		  
		[XmlAttribute("UserFld7")]
		[Bindable(true)]
		public string UserFld7 
		{
			get { return GetColumnValue<string>(Columns.UserFld7); }
			set { SetColumnValue(Columns.UserFld7, value); }
		}
		  
		[XmlAttribute("UserFld8")]
		[Bindable(true)]
		public string UserFld8 
		{
			get { return GetColumnValue<string>(Columns.UserFld8); }
			set { SetColumnValue(Columns.UserFld8, value); }
		}
		  
		[XmlAttribute("UserFld9")]
		[Bindable(true)]
		public string UserFld9 
		{
			get { return GetColumnValue<string>(Columns.UserFld9); }
			set { SetColumnValue(Columns.UserFld9, value); }
		}
		  
		[XmlAttribute("UserFld10")]
		[Bindable(true)]
		public string UserFld10 
		{
			get { return GetColumnValue<string>(Columns.UserFld10); }
			set { SetColumnValue(Columns.UserFld10, value); }
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
		  
		[XmlAttribute("UserInt1")]
		[Bindable(true)]
		public int? UserInt1 
		{
			get { return GetColumnValue<int?>(Columns.UserInt1); }
			set { SetColumnValue(Columns.UserInt1, value); }
		}
		  
		[XmlAttribute("UserInt2")]
		[Bindable(true)]
		public int? UserInt2 
		{
			get { return GetColumnValue<int?>(Columns.UserInt2); }
			set { SetColumnValue(Columns.UserInt2, value); }
		}
		  
		[XmlAttribute("UserInt3")]
		[Bindable(true)]
		public int? UserInt3 
		{
			get { return GetColumnValue<int?>(Columns.UserInt3); }
			set { SetColumnValue(Columns.UserInt3, value); }
		}
		  
		[XmlAttribute("UserInt4")]
		[Bindable(true)]
		public int? UserInt4 
		{
			get { return GetColumnValue<int?>(Columns.UserInt4); }
			set { SetColumnValue(Columns.UserInt4, value); }
		}
		  
		[XmlAttribute("UserInt5")]
		[Bindable(true)]
		public int? UserInt5 
		{
			get { return GetColumnValue<int?>(Columns.UserInt5); }
			set { SetColumnValue(Columns.UserInt5, value); }
		}
		
		#endregion
		
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(string varMembershipNo,DateTime varNewExpiryDate,string varOrderHdrId,DateTime? varCreatedOn,string varCreatedBy,DateTime? varModifiedOn,string varModifiedBy,bool? varDeleted,Guid varUniqueId,string varUserFld1,string varUserFld2,string varUserFld3,string varUserFld4,string varUserFld5,string varUserFld6,string varUserFld7,string varUserFld8,string varUserFld9,string varUserFld10,bool? varUserFlag1,bool? varUserFlag2,bool? varUserFlag3,bool? varUserFlag4,bool? varUserFlag5,decimal? varUserFloat1,decimal? varUserFloat2,decimal? varUserFloat3,decimal? varUserFloat4,decimal? varUserFloat5,int? varUserInt1,int? varUserInt2,int? varUserInt3,int? varUserInt4,int? varUserInt5)
		{
			MembershipRenewal item = new MembershipRenewal();
			
			item.MembershipNo = varMembershipNo;
			
			item.NewExpiryDate = varNewExpiryDate;
			
			item.OrderHdrId = varOrderHdrId;
			
			item.CreatedOn = varCreatedOn;
			
			item.CreatedBy = varCreatedBy;
			
			item.ModifiedOn = varModifiedOn;
			
			item.ModifiedBy = varModifiedBy;
			
			item.Deleted = varDeleted;
			
			item.UniqueId = varUniqueId;
			
			item.UserFld1 = varUserFld1;
			
			item.UserFld2 = varUserFld2;
			
			item.UserFld3 = varUserFld3;
			
			item.UserFld4 = varUserFld4;
			
			item.UserFld5 = varUserFld5;
			
			item.UserFld6 = varUserFld6;
			
			item.UserFld7 = varUserFld7;
			
			item.UserFld8 = varUserFld8;
			
			item.UserFld9 = varUserFld9;
			
			item.UserFld10 = varUserFld10;
			
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
			
			item.UserInt1 = varUserInt1;
			
			item.UserInt2 = varUserInt2;
			
			item.UserInt3 = varUserInt3;
			
			item.UserInt4 = varUserInt4;
			
			item.UserInt5 = varUserInt5;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varRenewalID,string varMembershipNo,DateTime varNewExpiryDate,string varOrderHdrId,DateTime? varCreatedOn,string varCreatedBy,DateTime? varModifiedOn,string varModifiedBy,bool? varDeleted,Guid varUniqueId,string varUserFld1,string varUserFld2,string varUserFld3,string varUserFld4,string varUserFld5,string varUserFld6,string varUserFld7,string varUserFld8,string varUserFld9,string varUserFld10,bool? varUserFlag1,bool? varUserFlag2,bool? varUserFlag3,bool? varUserFlag4,bool? varUserFlag5,decimal? varUserFloat1,decimal? varUserFloat2,decimal? varUserFloat3,decimal? varUserFloat4,decimal? varUserFloat5,int? varUserInt1,int? varUserInt2,int? varUserInt3,int? varUserInt4,int? varUserInt5)
		{
			MembershipRenewal item = new MembershipRenewal();
			
				item.RenewalID = varRenewalID;
			
				item.MembershipNo = varMembershipNo;
			
				item.NewExpiryDate = varNewExpiryDate;
			
				item.OrderHdrId = varOrderHdrId;
			
				item.CreatedOn = varCreatedOn;
			
				item.CreatedBy = varCreatedBy;
			
				item.ModifiedOn = varModifiedOn;
			
				item.ModifiedBy = varModifiedBy;
			
				item.Deleted = varDeleted;
			
				item.UniqueId = varUniqueId;
			
				item.UserFld1 = varUserFld1;
			
				item.UserFld2 = varUserFld2;
			
				item.UserFld3 = varUserFld3;
			
				item.UserFld4 = varUserFld4;
			
				item.UserFld5 = varUserFld5;
			
				item.UserFld6 = varUserFld6;
			
				item.UserFld7 = varUserFld7;
			
				item.UserFld8 = varUserFld8;
			
				item.UserFld9 = varUserFld9;
			
				item.UserFld10 = varUserFld10;
			
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
			
				item.UserInt1 = varUserInt1;
			
				item.UserInt2 = varUserInt2;
			
				item.UserInt3 = varUserInt3;
			
				item.UserInt4 = varUserInt4;
			
				item.UserInt5 = varUserInt5;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn RenewalIDColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn MembershipNoColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn NewExpiryDateColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn OrderHdrIdColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedOnColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedByColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedOnColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedByColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn DeletedColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn UniqueIdColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn UserFld1Column
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        public static TableSchema.TableColumn UserFld2Column
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        public static TableSchema.TableColumn UserFld3Column
        {
            get { return Schema.Columns[12]; }
        }
        
        
        
        public static TableSchema.TableColumn UserFld4Column
        {
            get { return Schema.Columns[13]; }
        }
        
        
        
        public static TableSchema.TableColumn UserFld5Column
        {
            get { return Schema.Columns[14]; }
        }
        
        
        
        public static TableSchema.TableColumn UserFld6Column
        {
            get { return Schema.Columns[15]; }
        }
        
        
        
        public static TableSchema.TableColumn UserFld7Column
        {
            get { return Schema.Columns[16]; }
        }
        
        
        
        public static TableSchema.TableColumn UserFld8Column
        {
            get { return Schema.Columns[17]; }
        }
        
        
        
        public static TableSchema.TableColumn UserFld9Column
        {
            get { return Schema.Columns[18]; }
        }
        
        
        
        public static TableSchema.TableColumn UserFld10Column
        {
            get { return Schema.Columns[19]; }
        }
        
        
        
        public static TableSchema.TableColumn UserFlag1Column
        {
            get { return Schema.Columns[20]; }
        }
        
        
        
        public static TableSchema.TableColumn UserFlag2Column
        {
            get { return Schema.Columns[21]; }
        }
        
        
        
        public static TableSchema.TableColumn UserFlag3Column
        {
            get { return Schema.Columns[22]; }
        }
        
        
        
        public static TableSchema.TableColumn UserFlag4Column
        {
            get { return Schema.Columns[23]; }
        }
        
        
        
        public static TableSchema.TableColumn UserFlag5Column
        {
            get { return Schema.Columns[24]; }
        }
        
        
        
        public static TableSchema.TableColumn UserFloat1Column
        {
            get { return Schema.Columns[25]; }
        }
        
        
        
        public static TableSchema.TableColumn UserFloat2Column
        {
            get { return Schema.Columns[26]; }
        }
        
        
        
        public static TableSchema.TableColumn UserFloat3Column
        {
            get { return Schema.Columns[27]; }
        }
        
        
        
        public static TableSchema.TableColumn UserFloat4Column
        {
            get { return Schema.Columns[28]; }
        }
        
        
        
        public static TableSchema.TableColumn UserFloat5Column
        {
            get { return Schema.Columns[29]; }
        }
        
        
        
        public static TableSchema.TableColumn UserInt1Column
        {
            get { return Schema.Columns[30]; }
        }
        
        
        
        public static TableSchema.TableColumn UserInt2Column
        {
            get { return Schema.Columns[31]; }
        }
        
        
        
        public static TableSchema.TableColumn UserInt3Column
        {
            get { return Schema.Columns[32]; }
        }
        
        
        
        public static TableSchema.TableColumn UserInt4Column
        {
            get { return Schema.Columns[33]; }
        }
        
        
        
        public static TableSchema.TableColumn UserInt5Column
        {
            get { return Schema.Columns[34]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string RenewalID = @"renewalID";
			 public static string MembershipNo = @"MembershipNo";
			 public static string NewExpiryDate = @"NewExpiryDate";
			 public static string OrderHdrId = @"OrderHdrId";
			 public static string CreatedOn = @"CreatedOn";
			 public static string CreatedBy = @"CreatedBy";
			 public static string ModifiedOn = @"ModifiedOn";
			 public static string ModifiedBy = @"ModifiedBy";
			 public static string Deleted = @"Deleted";
			 public static string UniqueId = @"UniqueId";
			 public static string UserFld1 = @"UserFld1";
			 public static string UserFld2 = @"UserFld2";
			 public static string UserFld3 = @"UserFld3";
			 public static string UserFld4 = @"UserFld4";
			 public static string UserFld5 = @"UserFld5";
			 public static string UserFld6 = @"UserFld6";
			 public static string UserFld7 = @"UserFld7";
			 public static string UserFld8 = @"UserFld8";
			 public static string UserFld9 = @"UserFld9";
			 public static string UserFld10 = @"UserFld10";
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
			 public static string UserInt1 = @"UserInt1";
			 public static string UserInt2 = @"UserInt2";
			 public static string UserInt3 = @"UserInt3";
			 public static string UserInt4 = @"UserInt4";
			 public static string UserInt5 = @"UserInt5";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
