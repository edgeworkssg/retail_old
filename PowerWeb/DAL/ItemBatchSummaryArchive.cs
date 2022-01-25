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
	/// Strongly-typed collection for the ItemBatchSummaryArchive class.
	/// </summary>
    [Serializable]
	public partial class ItemBatchSummaryArchiveCollection : ActiveList<ItemBatchSummaryArchive, ItemBatchSummaryArchiveCollection>
	{	   
		public ItemBatchSummaryArchiveCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>ItemBatchSummaryArchiveCollection</returns>
		public ItemBatchSummaryArchiveCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                ItemBatchSummaryArchive o = this[i];
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
	/// This is an ActiveRecord class which wraps the ItemBatchSummaryArchive table.
	/// </summary>
	[Serializable]
	public partial class ItemBatchSummaryArchive : ActiveRecord<ItemBatchSummaryArchive>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public ItemBatchSummaryArchive()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public ItemBatchSummaryArchive(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public ItemBatchSummaryArchive(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public ItemBatchSummaryArchive(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("ItemBatchSummaryArchive", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarItemBatchSummaryID = new TableSchema.TableColumn(schema);
				colvarItemBatchSummaryID.ColumnName = "ItemBatchSummaryID";
				colvarItemBatchSummaryID.DataType = DbType.AnsiString;
				colvarItemBatchSummaryID.MaxLength = 200;
				colvarItemBatchSummaryID.AutoIncrement = false;
				colvarItemBatchSummaryID.IsNullable = false;
				colvarItemBatchSummaryID.IsPrimaryKey = true;
				colvarItemBatchSummaryID.IsForeignKey = false;
				colvarItemBatchSummaryID.IsReadOnly = false;
				colvarItemBatchSummaryID.DefaultSetting = @"";
				colvarItemBatchSummaryID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarItemBatchSummaryID);
				
				TableSchema.TableColumn colvarItemNo = new TableSchema.TableColumn(schema);
				colvarItemNo.ColumnName = "ItemNo";
				colvarItemNo.DataType = DbType.AnsiString;
				colvarItemNo.MaxLength = 50;
				colvarItemNo.AutoIncrement = false;
				colvarItemNo.IsNullable = false;
				colvarItemNo.IsPrimaryKey = false;
				colvarItemNo.IsForeignKey = true;
				colvarItemNo.IsReadOnly = false;
				colvarItemNo.DefaultSetting = @"";
				
					colvarItemNo.ForeignKeyTableName = "Item";
				schema.Columns.Add(colvarItemNo);
				
				TableSchema.TableColumn colvarInventoryLocationID = new TableSchema.TableColumn(schema);
				colvarInventoryLocationID.ColumnName = "InventoryLocationID";
				colvarInventoryLocationID.DataType = DbType.Int32;
				colvarInventoryLocationID.MaxLength = 0;
				colvarInventoryLocationID.AutoIncrement = false;
				colvarInventoryLocationID.IsNullable = true;
				colvarInventoryLocationID.IsPrimaryKey = false;
				colvarInventoryLocationID.IsForeignKey = true;
				colvarInventoryLocationID.IsReadOnly = false;
				colvarInventoryLocationID.DefaultSetting = @"";
				
					colvarInventoryLocationID.ForeignKeyTableName = "InventoryLocation";
				schema.Columns.Add(colvarInventoryLocationID);
				
				TableSchema.TableColumn colvarInventoryDetRefNo = new TableSchema.TableColumn(schema);
				colvarInventoryDetRefNo.ColumnName = "InventoryDetRefNo";
				colvarInventoryDetRefNo.DataType = DbType.AnsiString;
				colvarInventoryDetRefNo.MaxLength = 50;
				colvarInventoryDetRefNo.AutoIncrement = false;
				colvarInventoryDetRefNo.IsNullable = true;
				colvarInventoryDetRefNo.IsPrimaryKey = false;
				colvarInventoryDetRefNo.IsForeignKey = false;
				colvarInventoryDetRefNo.IsReadOnly = false;
				colvarInventoryDetRefNo.DefaultSetting = @"";
				colvarInventoryDetRefNo.ForeignKeyTableName = "";
				schema.Columns.Add(colvarInventoryDetRefNo);
				
				TableSchema.TableColumn colvarBatchNo = new TableSchema.TableColumn(schema);
				colvarBatchNo.ColumnName = "BatchNo";
				colvarBatchNo.DataType = DbType.AnsiString;
				colvarBatchNo.MaxLength = 50;
				colvarBatchNo.AutoIncrement = false;
				colvarBatchNo.IsNullable = true;
				colvarBatchNo.IsPrimaryKey = false;
				colvarBatchNo.IsForeignKey = false;
				colvarBatchNo.IsReadOnly = false;
				colvarBatchNo.DefaultSetting = @"";
				colvarBatchNo.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBatchNo);
				
				TableSchema.TableColumn colvarReceivingDate = new TableSchema.TableColumn(schema);
				colvarReceivingDate.ColumnName = "ReceivingDate";
				colvarReceivingDate.DataType = DbType.DateTime;
				colvarReceivingDate.MaxLength = 0;
				colvarReceivingDate.AutoIncrement = false;
				colvarReceivingDate.IsNullable = true;
				colvarReceivingDate.IsPrimaryKey = false;
				colvarReceivingDate.IsForeignKey = false;
				colvarReceivingDate.IsReadOnly = false;
				colvarReceivingDate.DefaultSetting = @"";
				colvarReceivingDate.ForeignKeyTableName = "";
				schema.Columns.Add(colvarReceivingDate);
				
				TableSchema.TableColumn colvarQty = new TableSchema.TableColumn(schema);
				colvarQty.ColumnName = "Qty";
				colvarQty.DataType = DbType.Double;
				colvarQty.MaxLength = 0;
				colvarQty.AutoIncrement = false;
				colvarQty.IsNullable = true;
				colvarQty.IsPrimaryKey = false;
				colvarQty.IsForeignKey = false;
				colvarQty.IsReadOnly = false;
				colvarQty.DefaultSetting = @"";
				colvarQty.ForeignKeyTableName = "";
				schema.Columns.Add(colvarQty);
				
				TableSchema.TableColumn colvarRemainingQty = new TableSchema.TableColumn(schema);
				colvarRemainingQty.ColumnName = "RemainingQty";
				colvarRemainingQty.DataType = DbType.Double;
				colvarRemainingQty.MaxLength = 0;
				colvarRemainingQty.AutoIncrement = false;
				colvarRemainingQty.IsNullable = true;
				colvarRemainingQty.IsPrimaryKey = false;
				colvarRemainingQty.IsForeignKey = false;
				colvarRemainingQty.IsReadOnly = false;
				colvarRemainingQty.DefaultSetting = @"";
				colvarRemainingQty.ForeignKeyTableName = "";
				schema.Columns.Add(colvarRemainingQty);
				
				TableSchema.TableColumn colvarCostPrice = new TableSchema.TableColumn(schema);
				colvarCostPrice.ColumnName = "CostPrice";
				colvarCostPrice.DataType = DbType.Currency;
				colvarCostPrice.MaxLength = 0;
				colvarCostPrice.AutoIncrement = false;
				colvarCostPrice.IsNullable = true;
				colvarCostPrice.IsPrimaryKey = false;
				colvarCostPrice.IsForeignKey = false;
				colvarCostPrice.IsReadOnly = false;
				colvarCostPrice.DefaultSetting = @"";
				colvarCostPrice.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCostPrice);
				
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
				
				TableSchema.TableColumn colvarCreatedBy = new TableSchema.TableColumn(schema);
				colvarCreatedBy.ColumnName = "CreatedBy";
				colvarCreatedBy.DataType = DbType.String;
				colvarCreatedBy.MaxLength = 500;
				colvarCreatedBy.AutoIncrement = false;
				colvarCreatedBy.IsNullable = true;
				colvarCreatedBy.IsPrimaryKey = false;
				colvarCreatedBy.IsForeignKey = false;
				colvarCreatedBy.IsReadOnly = false;
				colvarCreatedBy.DefaultSetting = @"";
				colvarCreatedBy.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCreatedBy);
				
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
				
				TableSchema.TableColumn colvarModifiedBy = new TableSchema.TableColumn(schema);
				colvarModifiedBy.ColumnName = "ModifiedBy";
				colvarModifiedBy.DataType = DbType.String;
				colvarModifiedBy.MaxLength = 500;
				colvarModifiedBy.AutoIncrement = false;
				colvarModifiedBy.IsNullable = true;
				colvarModifiedBy.IsPrimaryKey = false;
				colvarModifiedBy.IsForeignKey = false;
				colvarModifiedBy.IsReadOnly = false;
				colvarModifiedBy.DefaultSetting = @"";
				colvarModifiedBy.ForeignKeyTableName = "";
				schema.Columns.Add(colvarModifiedBy);
				
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
				
				TableSchema.TableColumn colvarUniqueID = new TableSchema.TableColumn(schema);
				colvarUniqueID.ColumnName = "UniqueID";
				colvarUniqueID.DataType = DbType.Guid;
				colvarUniqueID.MaxLength = 0;
				colvarUniqueID.AutoIncrement = false;
				colvarUniqueID.IsNullable = false;
				colvarUniqueID.IsPrimaryKey = false;
				colvarUniqueID.IsForeignKey = false;
				colvarUniqueID.IsReadOnly = false;
				colvarUniqueID.DefaultSetting = @"";
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
				
				TableSchema.TableColumn colvarUserfld1 = new TableSchema.TableColumn(schema);
				colvarUserfld1.ColumnName = "Userfld1";
				colvarUserfld1.DataType = DbType.String;
				colvarUserfld1.MaxLength = 500;
				colvarUserfld1.AutoIncrement = false;
				colvarUserfld1.IsNullable = true;
				colvarUserfld1.IsPrimaryKey = false;
				colvarUserfld1.IsForeignKey = false;
				colvarUserfld1.IsReadOnly = false;
				colvarUserfld1.DefaultSetting = @"";
				colvarUserfld1.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfld1);
				
				TableSchema.TableColumn colvarUserfld2 = new TableSchema.TableColumn(schema);
				colvarUserfld2.ColumnName = "Userfld2";
				colvarUserfld2.DataType = DbType.String;
				colvarUserfld2.MaxLength = 500;
				colvarUserfld2.AutoIncrement = false;
				colvarUserfld2.IsNullable = true;
				colvarUserfld2.IsPrimaryKey = false;
				colvarUserfld2.IsForeignKey = false;
				colvarUserfld2.IsReadOnly = false;
				colvarUserfld2.DefaultSetting = @"";
				colvarUserfld2.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfld2);
				
				TableSchema.TableColumn colvarUserfld3 = new TableSchema.TableColumn(schema);
				colvarUserfld3.ColumnName = "Userfld3";
				colvarUserfld3.DataType = DbType.String;
				colvarUserfld3.MaxLength = 500;
				colvarUserfld3.AutoIncrement = false;
				colvarUserfld3.IsNullable = true;
				colvarUserfld3.IsPrimaryKey = false;
				colvarUserfld3.IsForeignKey = false;
				colvarUserfld3.IsReadOnly = false;
				colvarUserfld3.DefaultSetting = @"";
				colvarUserfld3.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfld3);
				
				TableSchema.TableColumn colvarUserfld4 = new TableSchema.TableColumn(schema);
				colvarUserfld4.ColumnName = "Userfld4";
				colvarUserfld4.DataType = DbType.String;
				colvarUserfld4.MaxLength = 500;
				colvarUserfld4.AutoIncrement = false;
				colvarUserfld4.IsNullable = true;
				colvarUserfld4.IsPrimaryKey = false;
				colvarUserfld4.IsForeignKey = false;
				colvarUserfld4.IsReadOnly = false;
				colvarUserfld4.DefaultSetting = @"";
				colvarUserfld4.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfld4);
				
				TableSchema.TableColumn colvarUserfld5 = new TableSchema.TableColumn(schema);
				colvarUserfld5.ColumnName = "Userfld5";
				colvarUserfld5.DataType = DbType.String;
				colvarUserfld5.MaxLength = 500;
				colvarUserfld5.AutoIncrement = false;
				colvarUserfld5.IsNullable = true;
				colvarUserfld5.IsPrimaryKey = false;
				colvarUserfld5.IsForeignKey = false;
				colvarUserfld5.IsReadOnly = false;
				colvarUserfld5.DefaultSetting = @"";
				colvarUserfld5.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfld5);
				
				TableSchema.TableColumn colvarUserfld6 = new TableSchema.TableColumn(schema);
				colvarUserfld6.ColumnName = "Userfld6";
				colvarUserfld6.DataType = DbType.String;
				colvarUserfld6.MaxLength = 500;
				colvarUserfld6.AutoIncrement = false;
				colvarUserfld6.IsNullable = true;
				colvarUserfld6.IsPrimaryKey = false;
				colvarUserfld6.IsForeignKey = false;
				colvarUserfld6.IsReadOnly = false;
				colvarUserfld6.DefaultSetting = @"";
				colvarUserfld6.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfld6);
				
				TableSchema.TableColumn colvarUserfld7 = new TableSchema.TableColumn(schema);
				colvarUserfld7.ColumnName = "Userfld7";
				colvarUserfld7.DataType = DbType.String;
				colvarUserfld7.MaxLength = 500;
				colvarUserfld7.AutoIncrement = false;
				colvarUserfld7.IsNullable = true;
				colvarUserfld7.IsPrimaryKey = false;
				colvarUserfld7.IsForeignKey = false;
				colvarUserfld7.IsReadOnly = false;
				colvarUserfld7.DefaultSetting = @"";
				colvarUserfld7.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfld7);
				
				TableSchema.TableColumn colvarUserfld8 = new TableSchema.TableColumn(schema);
				colvarUserfld8.ColumnName = "Userfld8";
				colvarUserfld8.DataType = DbType.String;
				colvarUserfld8.MaxLength = 500;
				colvarUserfld8.AutoIncrement = false;
				colvarUserfld8.IsNullable = true;
				colvarUserfld8.IsPrimaryKey = false;
				colvarUserfld8.IsForeignKey = false;
				colvarUserfld8.IsReadOnly = false;
				colvarUserfld8.DefaultSetting = @"";
				colvarUserfld8.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfld8);
				
				TableSchema.TableColumn colvarUserfld9 = new TableSchema.TableColumn(schema);
				colvarUserfld9.ColumnName = "Userfld9";
				colvarUserfld9.DataType = DbType.String;
				colvarUserfld9.MaxLength = 500;
				colvarUserfld9.AutoIncrement = false;
				colvarUserfld9.IsNullable = true;
				colvarUserfld9.IsPrimaryKey = false;
				colvarUserfld9.IsForeignKey = false;
				colvarUserfld9.IsReadOnly = false;
				colvarUserfld9.DefaultSetting = @"";
				colvarUserfld9.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfld9);
				
				TableSchema.TableColumn colvarUserfld10 = new TableSchema.TableColumn(schema);
				colvarUserfld10.ColumnName = "Userfld10";
				colvarUserfld10.DataType = DbType.String;
				colvarUserfld10.MaxLength = 500;
				colvarUserfld10.AutoIncrement = false;
				colvarUserfld10.IsNullable = true;
				colvarUserfld10.IsPrimaryKey = false;
				colvarUserfld10.IsForeignKey = false;
				colvarUserfld10.IsReadOnly = false;
				colvarUserfld10.DefaultSetting = @"";
				colvarUserfld10.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfld10);
				
				TableSchema.TableColumn colvarUserflag1 = new TableSchema.TableColumn(schema);
				colvarUserflag1.ColumnName = "Userflag1";
				colvarUserflag1.DataType = DbType.Boolean;
				colvarUserflag1.MaxLength = 0;
				colvarUserflag1.AutoIncrement = false;
				colvarUserflag1.IsNullable = true;
				colvarUserflag1.IsPrimaryKey = false;
				colvarUserflag1.IsForeignKey = false;
				colvarUserflag1.IsReadOnly = false;
				colvarUserflag1.DefaultSetting = @"";
				colvarUserflag1.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserflag1);
				
				TableSchema.TableColumn colvarUserflag2 = new TableSchema.TableColumn(schema);
				colvarUserflag2.ColumnName = "Userflag2";
				colvarUserflag2.DataType = DbType.Boolean;
				colvarUserflag2.MaxLength = 0;
				colvarUserflag2.AutoIncrement = false;
				colvarUserflag2.IsNullable = true;
				colvarUserflag2.IsPrimaryKey = false;
				colvarUserflag2.IsForeignKey = false;
				colvarUserflag2.IsReadOnly = false;
				colvarUserflag2.DefaultSetting = @"";
				colvarUserflag2.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserflag2);
				
				TableSchema.TableColumn colvarUserflag3 = new TableSchema.TableColumn(schema);
				colvarUserflag3.ColumnName = "Userflag3";
				colvarUserflag3.DataType = DbType.Boolean;
				colvarUserflag3.MaxLength = 0;
				colvarUserflag3.AutoIncrement = false;
				colvarUserflag3.IsNullable = true;
				colvarUserflag3.IsPrimaryKey = false;
				colvarUserflag3.IsForeignKey = false;
				colvarUserflag3.IsReadOnly = false;
				colvarUserflag3.DefaultSetting = @"";
				colvarUserflag3.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserflag3);
				
				TableSchema.TableColumn colvarUserflag4 = new TableSchema.TableColumn(schema);
				colvarUserflag4.ColumnName = "Userflag4";
				colvarUserflag4.DataType = DbType.Boolean;
				colvarUserflag4.MaxLength = 0;
				colvarUserflag4.AutoIncrement = false;
				colvarUserflag4.IsNullable = true;
				colvarUserflag4.IsPrimaryKey = false;
				colvarUserflag4.IsForeignKey = false;
				colvarUserflag4.IsReadOnly = false;
				colvarUserflag4.DefaultSetting = @"";
				colvarUserflag4.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserflag4);
				
				TableSchema.TableColumn colvarUserflag5 = new TableSchema.TableColumn(schema);
				colvarUserflag5.ColumnName = "Userflag5";
				colvarUserflag5.DataType = DbType.Boolean;
				colvarUserflag5.MaxLength = 0;
				colvarUserflag5.AutoIncrement = false;
				colvarUserflag5.IsNullable = true;
				colvarUserflag5.IsPrimaryKey = false;
				colvarUserflag5.IsForeignKey = false;
				colvarUserflag5.IsReadOnly = false;
				colvarUserflag5.DefaultSetting = @"";
				colvarUserflag5.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserflag5);
				
				TableSchema.TableColumn colvarUserfloat1 = new TableSchema.TableColumn(schema);
				colvarUserfloat1.ColumnName = "Userfloat1";
				colvarUserfloat1.DataType = DbType.Currency;
				colvarUserfloat1.MaxLength = 0;
				colvarUserfloat1.AutoIncrement = false;
				colvarUserfloat1.IsNullable = true;
				colvarUserfloat1.IsPrimaryKey = false;
				colvarUserfloat1.IsForeignKey = false;
				colvarUserfloat1.IsReadOnly = false;
				colvarUserfloat1.DefaultSetting = @"";
				colvarUserfloat1.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfloat1);
				
				TableSchema.TableColumn colvarUserfloat2 = new TableSchema.TableColumn(schema);
				colvarUserfloat2.ColumnName = "Userfloat2";
				colvarUserfloat2.DataType = DbType.Currency;
				colvarUserfloat2.MaxLength = 0;
				colvarUserfloat2.AutoIncrement = false;
				colvarUserfloat2.IsNullable = true;
				colvarUserfloat2.IsPrimaryKey = false;
				colvarUserfloat2.IsForeignKey = false;
				colvarUserfloat2.IsReadOnly = false;
				colvarUserfloat2.DefaultSetting = @"";
				colvarUserfloat2.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfloat2);
				
				TableSchema.TableColumn colvarUserfloat3 = new TableSchema.TableColumn(schema);
				colvarUserfloat3.ColumnName = "Userfloat3";
				colvarUserfloat3.DataType = DbType.Currency;
				colvarUserfloat3.MaxLength = 0;
				colvarUserfloat3.AutoIncrement = false;
				colvarUserfloat3.IsNullable = true;
				colvarUserfloat3.IsPrimaryKey = false;
				colvarUserfloat3.IsForeignKey = false;
				colvarUserfloat3.IsReadOnly = false;
				colvarUserfloat3.DefaultSetting = @"";
				colvarUserfloat3.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfloat3);
				
				TableSchema.TableColumn colvarUserfloat4 = new TableSchema.TableColumn(schema);
				colvarUserfloat4.ColumnName = "Userfloat4";
				colvarUserfloat4.DataType = DbType.Currency;
				colvarUserfloat4.MaxLength = 0;
				colvarUserfloat4.AutoIncrement = false;
				colvarUserfloat4.IsNullable = true;
				colvarUserfloat4.IsPrimaryKey = false;
				colvarUserfloat4.IsForeignKey = false;
				colvarUserfloat4.IsReadOnly = false;
				colvarUserfloat4.DefaultSetting = @"";
				colvarUserfloat4.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfloat4);
				
				TableSchema.TableColumn colvarUserfloat5 = new TableSchema.TableColumn(schema);
				colvarUserfloat5.ColumnName = "Userfloat5";
				colvarUserfloat5.DataType = DbType.Currency;
				colvarUserfloat5.MaxLength = 0;
				colvarUserfloat5.AutoIncrement = false;
				colvarUserfloat5.IsNullable = true;
				colvarUserfloat5.IsPrimaryKey = false;
				colvarUserfloat5.IsForeignKey = false;
				colvarUserfloat5.IsReadOnly = false;
				colvarUserfloat5.DefaultSetting = @"";
				colvarUserfloat5.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfloat5);
				
				TableSchema.TableColumn colvarUserint1 = new TableSchema.TableColumn(schema);
				colvarUserint1.ColumnName = "Userint1";
				colvarUserint1.DataType = DbType.Int32;
				colvarUserint1.MaxLength = 0;
				colvarUserint1.AutoIncrement = false;
				colvarUserint1.IsNullable = true;
				colvarUserint1.IsPrimaryKey = false;
				colvarUserint1.IsForeignKey = false;
				colvarUserint1.IsReadOnly = false;
				colvarUserint1.DefaultSetting = @"";
				colvarUserint1.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserint1);
				
				TableSchema.TableColumn colvarUserint2 = new TableSchema.TableColumn(schema);
				colvarUserint2.ColumnName = "Userint2";
				colvarUserint2.DataType = DbType.Int32;
				colvarUserint2.MaxLength = 0;
				colvarUserint2.AutoIncrement = false;
				colvarUserint2.IsNullable = true;
				colvarUserint2.IsPrimaryKey = false;
				colvarUserint2.IsForeignKey = false;
				colvarUserint2.IsReadOnly = false;
				colvarUserint2.DefaultSetting = @"";
				colvarUserint2.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserint2);
				
				TableSchema.TableColumn colvarUserint3 = new TableSchema.TableColumn(schema);
				colvarUserint3.ColumnName = "Userint3";
				colvarUserint3.DataType = DbType.Int32;
				colvarUserint3.MaxLength = 0;
				colvarUserint3.AutoIncrement = false;
				colvarUserint3.IsNullable = true;
				colvarUserint3.IsPrimaryKey = false;
				colvarUserint3.IsForeignKey = false;
				colvarUserint3.IsReadOnly = false;
				colvarUserint3.DefaultSetting = @"";
				colvarUserint3.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserint3);
				
				TableSchema.TableColumn colvarUserint4 = new TableSchema.TableColumn(schema);
				colvarUserint4.ColumnName = "Userint4";
				colvarUserint4.DataType = DbType.Int32;
				colvarUserint4.MaxLength = 0;
				colvarUserint4.AutoIncrement = false;
				colvarUserint4.IsNullable = true;
				colvarUserint4.IsPrimaryKey = false;
				colvarUserint4.IsForeignKey = false;
				colvarUserint4.IsReadOnly = false;
				colvarUserint4.DefaultSetting = @"";
				colvarUserint4.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserint4);
				
				TableSchema.TableColumn colvarUserint5 = new TableSchema.TableColumn(schema);
				colvarUserint5.ColumnName = "Userint5";
				colvarUserint5.DataType = DbType.Int32;
				colvarUserint5.MaxLength = 0;
				colvarUserint5.AutoIncrement = false;
				colvarUserint5.IsNullable = true;
				colvarUserint5.IsPrimaryKey = false;
				colvarUserint5.IsForeignKey = false;
				colvarUserint5.IsReadOnly = false;
				colvarUserint5.DefaultSetting = @"";
				colvarUserint5.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserint5);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["PowerPOS"].AddSchema("ItemBatchSummaryArchive",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("ItemBatchSummaryID")]
		[Bindable(true)]
		public string ItemBatchSummaryID 
		{
			get { return GetColumnValue<string>(Columns.ItemBatchSummaryID); }
			set { SetColumnValue(Columns.ItemBatchSummaryID, value); }
		}
		  
		[XmlAttribute("ItemNo")]
		[Bindable(true)]
		public string ItemNo 
		{
			get { return GetColumnValue<string>(Columns.ItemNo); }
			set { SetColumnValue(Columns.ItemNo, value); }
		}
		  
		[XmlAttribute("InventoryLocationID")]
		[Bindable(true)]
		public int? InventoryLocationID 
		{
			get { return GetColumnValue<int?>(Columns.InventoryLocationID); }
			set { SetColumnValue(Columns.InventoryLocationID, value); }
		}
		  
		[XmlAttribute("InventoryDetRefNo")]
		[Bindable(true)]
		public string InventoryDetRefNo 
		{
			get { return GetColumnValue<string>(Columns.InventoryDetRefNo); }
			set { SetColumnValue(Columns.InventoryDetRefNo, value); }
		}
		  
		[XmlAttribute("BatchNo")]
		[Bindable(true)]
		public string BatchNo 
		{
			get { return GetColumnValue<string>(Columns.BatchNo); }
			set { SetColumnValue(Columns.BatchNo, value); }
		}
		  
		[XmlAttribute("ReceivingDate")]
		[Bindable(true)]
		public DateTime? ReceivingDate 
		{
			get { return GetColumnValue<DateTime?>(Columns.ReceivingDate); }
			set { SetColumnValue(Columns.ReceivingDate, value); }
		}
		  
		[XmlAttribute("Qty")]
		[Bindable(true)]
		public double? Qty 
		{
			get { return GetColumnValue<double?>(Columns.Qty); }
			set { SetColumnValue(Columns.Qty, value); }
		}
		  
		[XmlAttribute("RemainingQty")]
		[Bindable(true)]
		public double? RemainingQty 
		{
			get { return GetColumnValue<double?>(Columns.RemainingQty); }
			set { SetColumnValue(Columns.RemainingQty, value); }
		}
		  
		[XmlAttribute("CostPrice")]
		[Bindable(true)]
		public decimal? CostPrice 
		{
			get { return GetColumnValue<decimal?>(Columns.CostPrice); }
			set { SetColumnValue(Columns.CostPrice, value); }
		}
		  
		[XmlAttribute("Remark")]
		[Bindable(true)]
		public string Remark 
		{
			get { return GetColumnValue<string>(Columns.Remark); }
			set { SetColumnValue(Columns.Remark, value); }
		}
		  
		[XmlAttribute("CreatedBy")]
		[Bindable(true)]
		public string CreatedBy 
		{
			get { return GetColumnValue<string>(Columns.CreatedBy); }
			set { SetColumnValue(Columns.CreatedBy, value); }
		}
		  
		[XmlAttribute("CreatedOn")]
		[Bindable(true)]
		public DateTime? CreatedOn 
		{
			get { return GetColumnValue<DateTime?>(Columns.CreatedOn); }
			set { SetColumnValue(Columns.CreatedOn, value); }
		}
		  
		[XmlAttribute("ModifiedBy")]
		[Bindable(true)]
		public string ModifiedBy 
		{
			get { return GetColumnValue<string>(Columns.ModifiedBy); }
			set { SetColumnValue(Columns.ModifiedBy, value); }
		}
		  
		[XmlAttribute("ModifiedOn")]
		[Bindable(true)]
		public DateTime? ModifiedOn 
		{
			get { return GetColumnValue<DateTime?>(Columns.ModifiedOn); }
			set { SetColumnValue(Columns.ModifiedOn, value); }
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
		  
		[XmlAttribute("Userfld1")]
		[Bindable(true)]
		public string Userfld1 
		{
			get { return GetColumnValue<string>(Columns.Userfld1); }
			set { SetColumnValue(Columns.Userfld1, value); }
		}
		  
		[XmlAttribute("Userfld2")]
		[Bindable(true)]
		public string Userfld2 
		{
			get { return GetColumnValue<string>(Columns.Userfld2); }
			set { SetColumnValue(Columns.Userfld2, value); }
		}
		  
		[XmlAttribute("Userfld3")]
		[Bindable(true)]
		public string Userfld3 
		{
			get { return GetColumnValue<string>(Columns.Userfld3); }
			set { SetColumnValue(Columns.Userfld3, value); }
		}
		  
		[XmlAttribute("Userfld4")]
		[Bindable(true)]
		public string Userfld4 
		{
			get { return GetColumnValue<string>(Columns.Userfld4); }
			set { SetColumnValue(Columns.Userfld4, value); }
		}
		  
		[XmlAttribute("Userfld5")]
		[Bindable(true)]
		public string Userfld5 
		{
			get { return GetColumnValue<string>(Columns.Userfld5); }
			set { SetColumnValue(Columns.Userfld5, value); }
		}
		  
		[XmlAttribute("Userfld6")]
		[Bindable(true)]
		public string Userfld6 
		{
			get { return GetColumnValue<string>(Columns.Userfld6); }
			set { SetColumnValue(Columns.Userfld6, value); }
		}
		  
		[XmlAttribute("Userfld7")]
		[Bindable(true)]
		public string Userfld7 
		{
			get { return GetColumnValue<string>(Columns.Userfld7); }
			set { SetColumnValue(Columns.Userfld7, value); }
		}
		  
		[XmlAttribute("Userfld8")]
		[Bindable(true)]
		public string Userfld8 
		{
			get { return GetColumnValue<string>(Columns.Userfld8); }
			set { SetColumnValue(Columns.Userfld8, value); }
		}
		  
		[XmlAttribute("Userfld9")]
		[Bindable(true)]
		public string Userfld9 
		{
			get { return GetColumnValue<string>(Columns.Userfld9); }
			set { SetColumnValue(Columns.Userfld9, value); }
		}
		  
		[XmlAttribute("Userfld10")]
		[Bindable(true)]
		public string Userfld10 
		{
			get { return GetColumnValue<string>(Columns.Userfld10); }
			set { SetColumnValue(Columns.Userfld10, value); }
		}
		  
		[XmlAttribute("Userflag1")]
		[Bindable(true)]
		public bool? Userflag1 
		{
			get { return GetColumnValue<bool?>(Columns.Userflag1); }
			set { SetColumnValue(Columns.Userflag1, value); }
		}
		  
		[XmlAttribute("Userflag2")]
		[Bindable(true)]
		public bool? Userflag2 
		{
			get { return GetColumnValue<bool?>(Columns.Userflag2); }
			set { SetColumnValue(Columns.Userflag2, value); }
		}
		  
		[XmlAttribute("Userflag3")]
		[Bindable(true)]
		public bool? Userflag3 
		{
			get { return GetColumnValue<bool?>(Columns.Userflag3); }
			set { SetColumnValue(Columns.Userflag3, value); }
		}
		  
		[XmlAttribute("Userflag4")]
		[Bindable(true)]
		public bool? Userflag4 
		{
			get { return GetColumnValue<bool?>(Columns.Userflag4); }
			set { SetColumnValue(Columns.Userflag4, value); }
		}
		  
		[XmlAttribute("Userflag5")]
		[Bindable(true)]
		public bool? Userflag5 
		{
			get { return GetColumnValue<bool?>(Columns.Userflag5); }
			set { SetColumnValue(Columns.Userflag5, value); }
		}
		  
		[XmlAttribute("Userfloat1")]
		[Bindable(true)]
		public decimal? Userfloat1 
		{
			get { return GetColumnValue<decimal?>(Columns.Userfloat1); }
			set { SetColumnValue(Columns.Userfloat1, value); }
		}
		  
		[XmlAttribute("Userfloat2")]
		[Bindable(true)]
		public decimal? Userfloat2 
		{
			get { return GetColumnValue<decimal?>(Columns.Userfloat2); }
			set { SetColumnValue(Columns.Userfloat2, value); }
		}
		  
		[XmlAttribute("Userfloat3")]
		[Bindable(true)]
		public decimal? Userfloat3 
		{
			get { return GetColumnValue<decimal?>(Columns.Userfloat3); }
			set { SetColumnValue(Columns.Userfloat3, value); }
		}
		  
		[XmlAttribute("Userfloat4")]
		[Bindable(true)]
		public decimal? Userfloat4 
		{
			get { return GetColumnValue<decimal?>(Columns.Userfloat4); }
			set { SetColumnValue(Columns.Userfloat4, value); }
		}
		  
		[XmlAttribute("Userfloat5")]
		[Bindable(true)]
		public decimal? Userfloat5 
		{
			get { return GetColumnValue<decimal?>(Columns.Userfloat5); }
			set { SetColumnValue(Columns.Userfloat5, value); }
		}
		  
		[XmlAttribute("Userint1")]
		[Bindable(true)]
		public int? Userint1 
		{
			get { return GetColumnValue<int?>(Columns.Userint1); }
			set { SetColumnValue(Columns.Userint1, value); }
		}
		  
		[XmlAttribute("Userint2")]
		[Bindable(true)]
		public int? Userint2 
		{
			get { return GetColumnValue<int?>(Columns.Userint2); }
			set { SetColumnValue(Columns.Userint2, value); }
		}
		  
		[XmlAttribute("Userint3")]
		[Bindable(true)]
		public int? Userint3 
		{
			get { return GetColumnValue<int?>(Columns.Userint3); }
			set { SetColumnValue(Columns.Userint3, value); }
		}
		  
		[XmlAttribute("Userint4")]
		[Bindable(true)]
		public int? Userint4 
		{
			get { return GetColumnValue<int?>(Columns.Userint4); }
			set { SetColumnValue(Columns.Userint4, value); }
		}
		  
		[XmlAttribute("Userint5")]
		[Bindable(true)]
		public int? Userint5 
		{
			get { return GetColumnValue<int?>(Columns.Userint5); }
			set { SetColumnValue(Columns.Userint5, value); }
		}
		
		#endregion
		
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a InventoryLocation ActiveRecord object related to this ItemBatchSummaryArchive
		/// 
		/// </summary>
		public PowerPOS.InventoryLocation InventoryLocation
		{
			get { return PowerPOS.InventoryLocation.FetchByID(this.InventoryLocationID); }
			set { SetColumnValue("InventoryLocationID", value.InventoryLocationID); }
		}
		
		
		/// <summary>
		/// Returns a Item ActiveRecord object related to this ItemBatchSummaryArchive
		/// 
		/// </summary>
		public PowerPOS.Item Item
		{
			get { return PowerPOS.Item.FetchByID(this.ItemNo); }
			set { SetColumnValue("ItemNo", value.ItemNo); }
		}
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(string varItemBatchSummaryID,string varItemNo,int? varInventoryLocationID,string varInventoryDetRefNo,string varBatchNo,DateTime? varReceivingDate,double? varQty,double? varRemainingQty,decimal? varCostPrice,string varRemark,string varCreatedBy,DateTime? varCreatedOn,string varModifiedBy,DateTime? varModifiedOn,Guid varUniqueID,bool? varDeleted,string varUserfld1,string varUserfld2,string varUserfld3,string varUserfld4,string varUserfld5,string varUserfld6,string varUserfld7,string varUserfld8,string varUserfld9,string varUserfld10,bool? varUserflag1,bool? varUserflag2,bool? varUserflag3,bool? varUserflag4,bool? varUserflag5,decimal? varUserfloat1,decimal? varUserfloat2,decimal? varUserfloat3,decimal? varUserfloat4,decimal? varUserfloat5,int? varUserint1,int? varUserint2,int? varUserint3,int? varUserint4,int? varUserint5)
		{
			ItemBatchSummaryArchive item = new ItemBatchSummaryArchive();
			
			item.ItemBatchSummaryID = varItemBatchSummaryID;
			
			item.ItemNo = varItemNo;
			
			item.InventoryLocationID = varInventoryLocationID;
			
			item.InventoryDetRefNo = varInventoryDetRefNo;
			
			item.BatchNo = varBatchNo;
			
			item.ReceivingDate = varReceivingDate;
			
			item.Qty = varQty;
			
			item.RemainingQty = varRemainingQty;
			
			item.CostPrice = varCostPrice;
			
			item.Remark = varRemark;
			
			item.CreatedBy = varCreatedBy;
			
			item.CreatedOn = varCreatedOn;
			
			item.ModifiedBy = varModifiedBy;
			
			item.ModifiedOn = varModifiedOn;
			
			item.UniqueID = varUniqueID;
			
			item.Deleted = varDeleted;
			
			item.Userfld1 = varUserfld1;
			
			item.Userfld2 = varUserfld2;
			
			item.Userfld3 = varUserfld3;
			
			item.Userfld4 = varUserfld4;
			
			item.Userfld5 = varUserfld5;
			
			item.Userfld6 = varUserfld6;
			
			item.Userfld7 = varUserfld7;
			
			item.Userfld8 = varUserfld8;
			
			item.Userfld9 = varUserfld9;
			
			item.Userfld10 = varUserfld10;
			
			item.Userflag1 = varUserflag1;
			
			item.Userflag2 = varUserflag2;
			
			item.Userflag3 = varUserflag3;
			
			item.Userflag4 = varUserflag4;
			
			item.Userflag5 = varUserflag5;
			
			item.Userfloat1 = varUserfloat1;
			
			item.Userfloat2 = varUserfloat2;
			
			item.Userfloat3 = varUserfloat3;
			
			item.Userfloat4 = varUserfloat4;
			
			item.Userfloat5 = varUserfloat5;
			
			item.Userint1 = varUserint1;
			
			item.Userint2 = varUserint2;
			
			item.Userint3 = varUserint3;
			
			item.Userint4 = varUserint4;
			
			item.Userint5 = varUserint5;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(string varItemBatchSummaryID,string varItemNo,int? varInventoryLocationID,string varInventoryDetRefNo,string varBatchNo,DateTime? varReceivingDate,double? varQty,double? varRemainingQty,decimal? varCostPrice,string varRemark,string varCreatedBy,DateTime? varCreatedOn,string varModifiedBy,DateTime? varModifiedOn,Guid varUniqueID,bool? varDeleted,string varUserfld1,string varUserfld2,string varUserfld3,string varUserfld4,string varUserfld5,string varUserfld6,string varUserfld7,string varUserfld8,string varUserfld9,string varUserfld10,bool? varUserflag1,bool? varUserflag2,bool? varUserflag3,bool? varUserflag4,bool? varUserflag5,decimal? varUserfloat1,decimal? varUserfloat2,decimal? varUserfloat3,decimal? varUserfloat4,decimal? varUserfloat5,int? varUserint1,int? varUserint2,int? varUserint3,int? varUserint4,int? varUserint5)
		{
			ItemBatchSummaryArchive item = new ItemBatchSummaryArchive();
			
				item.ItemBatchSummaryID = varItemBatchSummaryID;
			
				item.ItemNo = varItemNo;
			
				item.InventoryLocationID = varInventoryLocationID;
			
				item.InventoryDetRefNo = varInventoryDetRefNo;
			
				item.BatchNo = varBatchNo;
			
				item.ReceivingDate = varReceivingDate;
			
				item.Qty = varQty;
			
				item.RemainingQty = varRemainingQty;
			
				item.CostPrice = varCostPrice;
			
				item.Remark = varRemark;
			
				item.CreatedBy = varCreatedBy;
			
				item.CreatedOn = varCreatedOn;
			
				item.ModifiedBy = varModifiedBy;
			
				item.ModifiedOn = varModifiedOn;
			
				item.UniqueID = varUniqueID;
			
				item.Deleted = varDeleted;
			
				item.Userfld1 = varUserfld1;
			
				item.Userfld2 = varUserfld2;
			
				item.Userfld3 = varUserfld3;
			
				item.Userfld4 = varUserfld4;
			
				item.Userfld5 = varUserfld5;
			
				item.Userfld6 = varUserfld6;
			
				item.Userfld7 = varUserfld7;
			
				item.Userfld8 = varUserfld8;
			
				item.Userfld9 = varUserfld9;
			
				item.Userfld10 = varUserfld10;
			
				item.Userflag1 = varUserflag1;
			
				item.Userflag2 = varUserflag2;
			
				item.Userflag3 = varUserflag3;
			
				item.Userflag4 = varUserflag4;
			
				item.Userflag5 = varUserflag5;
			
				item.Userfloat1 = varUserfloat1;
			
				item.Userfloat2 = varUserfloat2;
			
				item.Userfloat3 = varUserfloat3;
			
				item.Userfloat4 = varUserfloat4;
			
				item.Userfloat5 = varUserfloat5;
			
				item.Userint1 = varUserint1;
			
				item.Userint2 = varUserint2;
			
				item.Userint3 = varUserint3;
			
				item.Userint4 = varUserint4;
			
				item.Userint5 = varUserint5;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn ItemBatchSummaryIDColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn ItemNoColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn InventoryLocationIDColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn InventoryDetRefNoColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn BatchNoColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn ReceivingDateColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn QtyColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn RemainingQtyColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn CostPriceColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn RemarkColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedByColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedOnColumn
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedByColumn
        {
            get { return Schema.Columns[12]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedOnColumn
        {
            get { return Schema.Columns[13]; }
        }
        
        
        
        public static TableSchema.TableColumn UniqueIDColumn
        {
            get { return Schema.Columns[14]; }
        }
        
        
        
        public static TableSchema.TableColumn DeletedColumn
        {
            get { return Schema.Columns[15]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld1Column
        {
            get { return Schema.Columns[16]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld2Column
        {
            get { return Schema.Columns[17]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld3Column
        {
            get { return Schema.Columns[18]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld4Column
        {
            get { return Schema.Columns[19]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld5Column
        {
            get { return Schema.Columns[20]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld6Column
        {
            get { return Schema.Columns[21]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld7Column
        {
            get { return Schema.Columns[22]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld8Column
        {
            get { return Schema.Columns[23]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld9Column
        {
            get { return Schema.Columns[24]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld10Column
        {
            get { return Schema.Columns[25]; }
        }
        
        
        
        public static TableSchema.TableColumn Userflag1Column
        {
            get { return Schema.Columns[26]; }
        }
        
        
        
        public static TableSchema.TableColumn Userflag2Column
        {
            get { return Schema.Columns[27]; }
        }
        
        
        
        public static TableSchema.TableColumn Userflag3Column
        {
            get { return Schema.Columns[28]; }
        }
        
        
        
        public static TableSchema.TableColumn Userflag4Column
        {
            get { return Schema.Columns[29]; }
        }
        
        
        
        public static TableSchema.TableColumn Userflag5Column
        {
            get { return Schema.Columns[30]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfloat1Column
        {
            get { return Schema.Columns[31]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfloat2Column
        {
            get { return Schema.Columns[32]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfloat3Column
        {
            get { return Schema.Columns[33]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfloat4Column
        {
            get { return Schema.Columns[34]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfloat5Column
        {
            get { return Schema.Columns[35]; }
        }
        
        
        
        public static TableSchema.TableColumn Userint1Column
        {
            get { return Schema.Columns[36]; }
        }
        
        
        
        public static TableSchema.TableColumn Userint2Column
        {
            get { return Schema.Columns[37]; }
        }
        
        
        
        public static TableSchema.TableColumn Userint3Column
        {
            get { return Schema.Columns[38]; }
        }
        
        
        
        public static TableSchema.TableColumn Userint4Column
        {
            get { return Schema.Columns[39]; }
        }
        
        
        
        public static TableSchema.TableColumn Userint5Column
        {
            get { return Schema.Columns[40]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string ItemBatchSummaryID = @"ItemBatchSummaryID";
			 public static string ItemNo = @"ItemNo";
			 public static string InventoryLocationID = @"InventoryLocationID";
			 public static string InventoryDetRefNo = @"InventoryDetRefNo";
			 public static string BatchNo = @"BatchNo";
			 public static string ReceivingDate = @"ReceivingDate";
			 public static string Qty = @"Qty";
			 public static string RemainingQty = @"RemainingQty";
			 public static string CostPrice = @"CostPrice";
			 public static string Remark = @"Remark";
			 public static string CreatedBy = @"CreatedBy";
			 public static string CreatedOn = @"CreatedOn";
			 public static string ModifiedBy = @"ModifiedBy";
			 public static string ModifiedOn = @"ModifiedOn";
			 public static string UniqueID = @"UniqueID";
			 public static string Deleted = @"Deleted";
			 public static string Userfld1 = @"Userfld1";
			 public static string Userfld2 = @"Userfld2";
			 public static string Userfld3 = @"Userfld3";
			 public static string Userfld4 = @"Userfld4";
			 public static string Userfld5 = @"Userfld5";
			 public static string Userfld6 = @"Userfld6";
			 public static string Userfld7 = @"Userfld7";
			 public static string Userfld8 = @"Userfld8";
			 public static string Userfld9 = @"Userfld9";
			 public static string Userfld10 = @"Userfld10";
			 public static string Userflag1 = @"Userflag1";
			 public static string Userflag2 = @"Userflag2";
			 public static string Userflag3 = @"Userflag3";
			 public static string Userflag4 = @"Userflag4";
			 public static string Userflag5 = @"Userflag5";
			 public static string Userfloat1 = @"Userfloat1";
			 public static string Userfloat2 = @"Userfloat2";
			 public static string Userfloat3 = @"Userfloat3";
			 public static string Userfloat4 = @"Userfloat4";
			 public static string Userfloat5 = @"Userfloat5";
			 public static string Userint1 = @"Userint1";
			 public static string Userint2 = @"Userint2";
			 public static string Userint3 = @"Userint3";
			 public static string Userint4 = @"Userint4";
			 public static string Userint5 = @"Userint5";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
