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
	/// Strongly-typed collection for the ItemSupplierMap class.
	/// </summary>
    [Serializable]
	public partial class ItemSupplierMapCollection : ActiveList<ItemSupplierMap, ItemSupplierMapCollection>
	{	   
		public ItemSupplierMapCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>ItemSupplierMapCollection</returns>
		public ItemSupplierMapCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                ItemSupplierMap o = this[i];
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
	/// This is an ActiveRecord class which wraps the ItemSupplierMap table.
	/// </summary>
	[Serializable]
	public partial class ItemSupplierMap : ActiveRecord<ItemSupplierMap>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public ItemSupplierMap()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public ItemSupplierMap(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public ItemSupplierMap(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public ItemSupplierMap(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("ItemSupplierMap", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarItemSupplierMapID = new TableSchema.TableColumn(schema);
				colvarItemSupplierMapID.ColumnName = "ItemSupplierMapID";
				colvarItemSupplierMapID.DataType = DbType.Int32;
				colvarItemSupplierMapID.MaxLength = 0;
				colvarItemSupplierMapID.AutoIncrement = true;
				colvarItemSupplierMapID.IsNullable = false;
				colvarItemSupplierMapID.IsPrimaryKey = true;
				colvarItemSupplierMapID.IsForeignKey = false;
				colvarItemSupplierMapID.IsReadOnly = false;
				colvarItemSupplierMapID.DefaultSetting = @"";
				colvarItemSupplierMapID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarItemSupplierMapID);
				
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
				
				TableSchema.TableColumn colvarSupplierID = new TableSchema.TableColumn(schema);
				colvarSupplierID.ColumnName = "SupplierID";
				colvarSupplierID.DataType = DbType.Int32;
				colvarSupplierID.MaxLength = 0;
				colvarSupplierID.AutoIncrement = false;
				colvarSupplierID.IsNullable = false;
				colvarSupplierID.IsPrimaryKey = false;
				colvarSupplierID.IsForeignKey = true;
				colvarSupplierID.IsReadOnly = false;
				colvarSupplierID.DefaultSetting = @"";
				
					colvarSupplierID.ForeignKeyTableName = "Supplier";
				schema.Columns.Add(colvarSupplierID);
				
				TableSchema.TableColumn colvarCostPrice = new TableSchema.TableColumn(schema);
				colvarCostPrice.ColumnName = "CostPrice";
				colvarCostPrice.DataType = DbType.Currency;
				colvarCostPrice.MaxLength = 0;
				colvarCostPrice.AutoIncrement = false;
				colvarCostPrice.IsNullable = false;
				colvarCostPrice.IsPrimaryKey = false;
				colvarCostPrice.IsForeignKey = false;
				colvarCostPrice.IsReadOnly = false;
				colvarCostPrice.DefaultSetting = @"";
				colvarCostPrice.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCostPrice);
				
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
				colvarDeleted.DefaultSetting = @"";
				colvarDeleted.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDeleted);
				
				TableSchema.TableColumn colvarCurrency = new TableSchema.TableColumn(schema);
				colvarCurrency.ColumnName = "Currency";
				colvarCurrency.DataType = DbType.String;
				colvarCurrency.MaxLength = 50;
				colvarCurrency.AutoIncrement = false;
				colvarCurrency.IsNullable = true;
				colvarCurrency.IsPrimaryKey = false;
				colvarCurrency.IsForeignKey = false;
				colvarCurrency.IsReadOnly = false;
				colvarCurrency.DefaultSetting = @"";
				colvarCurrency.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCurrency);
				
				TableSchema.TableColumn colvarGst = new TableSchema.TableColumn(schema);
				colvarGst.ColumnName = "GST";
				colvarGst.DataType = DbType.Decimal;
				colvarGst.MaxLength = 0;
				colvarGst.AutoIncrement = false;
				colvarGst.IsNullable = true;
				colvarGst.IsPrimaryKey = false;
				colvarGst.IsForeignKey = false;
				colvarGst.IsReadOnly = false;
				colvarGst.DefaultSetting = @"";
				colvarGst.ForeignKeyTableName = "";
				schema.Columns.Add(colvarGst);
				
				TableSchema.TableColumn colvarGSTRule = new TableSchema.TableColumn(schema);
				colvarGSTRule.ColumnName = "GSTRule";
				colvarGSTRule.DataType = DbType.Int32;
				colvarGSTRule.MaxLength = 0;
				colvarGSTRule.AutoIncrement = false;
				colvarGSTRule.IsNullable = true;
				colvarGSTRule.IsPrimaryKey = false;
				colvarGSTRule.IsForeignKey = false;
				colvarGSTRule.IsReadOnly = false;
				colvarGSTRule.DefaultSetting = @"";
				colvarGSTRule.ForeignKeyTableName = "";
				schema.Columns.Add(colvarGSTRule);
				
				TableSchema.TableColumn colvarPackingSize1 = new TableSchema.TableColumn(schema);
				colvarPackingSize1.ColumnName = "PackingSize1";
				colvarPackingSize1.DataType = DbType.String;
				colvarPackingSize1.MaxLength = -1;
				colvarPackingSize1.AutoIncrement = false;
				colvarPackingSize1.IsNullable = true;
				colvarPackingSize1.IsPrimaryKey = false;
				colvarPackingSize1.IsForeignKey = false;
				colvarPackingSize1.IsReadOnly = false;
				colvarPackingSize1.DefaultSetting = @"";
				colvarPackingSize1.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPackingSize1);
				
				TableSchema.TableColumn colvarPackingSize2 = new TableSchema.TableColumn(schema);
				colvarPackingSize2.ColumnName = "PackingSize2";
				colvarPackingSize2.DataType = DbType.String;
				colvarPackingSize2.MaxLength = -1;
				colvarPackingSize2.AutoIncrement = false;
				colvarPackingSize2.IsNullable = true;
				colvarPackingSize2.IsPrimaryKey = false;
				colvarPackingSize2.IsForeignKey = false;
				colvarPackingSize2.IsReadOnly = false;
				colvarPackingSize2.DefaultSetting = @"";
				colvarPackingSize2.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPackingSize2);
				
				TableSchema.TableColumn colvarPackingSize3 = new TableSchema.TableColumn(schema);
				colvarPackingSize3.ColumnName = "PackingSize3";
				colvarPackingSize3.DataType = DbType.String;
				colvarPackingSize3.MaxLength = -1;
				colvarPackingSize3.AutoIncrement = false;
				colvarPackingSize3.IsNullable = true;
				colvarPackingSize3.IsPrimaryKey = false;
				colvarPackingSize3.IsForeignKey = false;
				colvarPackingSize3.IsReadOnly = false;
				colvarPackingSize3.DefaultSetting = @"";
				colvarPackingSize3.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPackingSize3);
				
				TableSchema.TableColumn colvarPackingSize4 = new TableSchema.TableColumn(schema);
				colvarPackingSize4.ColumnName = "PackingSize4";
				colvarPackingSize4.DataType = DbType.String;
				colvarPackingSize4.MaxLength = -1;
				colvarPackingSize4.AutoIncrement = false;
				colvarPackingSize4.IsNullable = true;
				colvarPackingSize4.IsPrimaryKey = false;
				colvarPackingSize4.IsForeignKey = false;
				colvarPackingSize4.IsReadOnly = false;
				colvarPackingSize4.DefaultSetting = @"";
				colvarPackingSize4.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPackingSize4);
				
				TableSchema.TableColumn colvarPackingSize5 = new TableSchema.TableColumn(schema);
				colvarPackingSize5.ColumnName = "PackingSize5";
				colvarPackingSize5.DataType = DbType.String;
				colvarPackingSize5.MaxLength = -1;
				colvarPackingSize5.AutoIncrement = false;
				colvarPackingSize5.IsNullable = true;
				colvarPackingSize5.IsPrimaryKey = false;
				colvarPackingSize5.IsForeignKey = false;
				colvarPackingSize5.IsReadOnly = false;
				colvarPackingSize5.DefaultSetting = @"";
				colvarPackingSize5.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPackingSize5);
				
				TableSchema.TableColumn colvarPackingSize6 = new TableSchema.TableColumn(schema);
				colvarPackingSize6.ColumnName = "PackingSize6";
				colvarPackingSize6.DataType = DbType.String;
				colvarPackingSize6.MaxLength = -1;
				colvarPackingSize6.AutoIncrement = false;
				colvarPackingSize6.IsNullable = true;
				colvarPackingSize6.IsPrimaryKey = false;
				colvarPackingSize6.IsForeignKey = false;
				colvarPackingSize6.IsReadOnly = false;
				colvarPackingSize6.DefaultSetting = @"";
				colvarPackingSize6.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPackingSize6);
				
				TableSchema.TableColumn colvarPackingSize7 = new TableSchema.TableColumn(schema);
				colvarPackingSize7.ColumnName = "PackingSize7";
				colvarPackingSize7.DataType = DbType.String;
				colvarPackingSize7.MaxLength = -1;
				colvarPackingSize7.AutoIncrement = false;
				colvarPackingSize7.IsNullable = true;
				colvarPackingSize7.IsPrimaryKey = false;
				colvarPackingSize7.IsForeignKey = false;
				colvarPackingSize7.IsReadOnly = false;
				colvarPackingSize7.DefaultSetting = @"";
				colvarPackingSize7.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPackingSize7);
				
				TableSchema.TableColumn colvarPackingSize8 = new TableSchema.TableColumn(schema);
				colvarPackingSize8.ColumnName = "PackingSize8";
				colvarPackingSize8.DataType = DbType.String;
				colvarPackingSize8.MaxLength = -1;
				colvarPackingSize8.AutoIncrement = false;
				colvarPackingSize8.IsNullable = true;
				colvarPackingSize8.IsPrimaryKey = false;
				colvarPackingSize8.IsForeignKey = false;
				colvarPackingSize8.IsReadOnly = false;
				colvarPackingSize8.DefaultSetting = @"";
				colvarPackingSize8.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPackingSize8);
				
				TableSchema.TableColumn colvarPackingSize9 = new TableSchema.TableColumn(schema);
				colvarPackingSize9.ColumnName = "PackingSize9";
				colvarPackingSize9.DataType = DbType.String;
				colvarPackingSize9.MaxLength = -1;
				colvarPackingSize9.AutoIncrement = false;
				colvarPackingSize9.IsNullable = true;
				colvarPackingSize9.IsPrimaryKey = false;
				colvarPackingSize9.IsForeignKey = false;
				colvarPackingSize9.IsReadOnly = false;
				colvarPackingSize9.DefaultSetting = @"";
				colvarPackingSize9.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPackingSize9);
				
				TableSchema.TableColumn colvarPackingSize10 = new TableSchema.TableColumn(schema);
				colvarPackingSize10.ColumnName = "PackingSize10";
				colvarPackingSize10.DataType = DbType.String;
				colvarPackingSize10.MaxLength = -1;
				colvarPackingSize10.AutoIncrement = false;
				colvarPackingSize10.IsNullable = true;
				colvarPackingSize10.IsPrimaryKey = false;
				colvarPackingSize10.IsForeignKey = false;
				colvarPackingSize10.IsReadOnly = false;
				colvarPackingSize10.DefaultSetting = @"";
				colvarPackingSize10.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPackingSize10);
				
				TableSchema.TableColumn colvarPackingSizeUOM1 = new TableSchema.TableColumn(schema);
				colvarPackingSizeUOM1.ColumnName = "PackingSizeUOM1";
				colvarPackingSizeUOM1.DataType = DbType.Decimal;
				colvarPackingSizeUOM1.MaxLength = 0;
				colvarPackingSizeUOM1.AutoIncrement = false;
				colvarPackingSizeUOM1.IsNullable = true;
				colvarPackingSizeUOM1.IsPrimaryKey = false;
				colvarPackingSizeUOM1.IsForeignKey = false;
				colvarPackingSizeUOM1.IsReadOnly = false;
				colvarPackingSizeUOM1.DefaultSetting = @"";
				colvarPackingSizeUOM1.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPackingSizeUOM1);
				
				TableSchema.TableColumn colvarPackingSizeUOM2 = new TableSchema.TableColumn(schema);
				colvarPackingSizeUOM2.ColumnName = "PackingSizeUOM2";
				colvarPackingSizeUOM2.DataType = DbType.Decimal;
				colvarPackingSizeUOM2.MaxLength = 0;
				colvarPackingSizeUOM2.AutoIncrement = false;
				colvarPackingSizeUOM2.IsNullable = true;
				colvarPackingSizeUOM2.IsPrimaryKey = false;
				colvarPackingSizeUOM2.IsForeignKey = false;
				colvarPackingSizeUOM2.IsReadOnly = false;
				colvarPackingSizeUOM2.DefaultSetting = @"";
				colvarPackingSizeUOM2.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPackingSizeUOM2);
				
				TableSchema.TableColumn colvarPackingSizeUOM3 = new TableSchema.TableColumn(schema);
				colvarPackingSizeUOM3.ColumnName = "PackingSizeUOM3";
				colvarPackingSizeUOM3.DataType = DbType.Decimal;
				colvarPackingSizeUOM3.MaxLength = 0;
				colvarPackingSizeUOM3.AutoIncrement = false;
				colvarPackingSizeUOM3.IsNullable = true;
				colvarPackingSizeUOM3.IsPrimaryKey = false;
				colvarPackingSizeUOM3.IsForeignKey = false;
				colvarPackingSizeUOM3.IsReadOnly = false;
				colvarPackingSizeUOM3.DefaultSetting = @"";
				colvarPackingSizeUOM3.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPackingSizeUOM3);
				
				TableSchema.TableColumn colvarPackingSizeUOM4 = new TableSchema.TableColumn(schema);
				colvarPackingSizeUOM4.ColumnName = "PackingSizeUOM4";
				colvarPackingSizeUOM4.DataType = DbType.Decimal;
				colvarPackingSizeUOM4.MaxLength = 0;
				colvarPackingSizeUOM4.AutoIncrement = false;
				colvarPackingSizeUOM4.IsNullable = true;
				colvarPackingSizeUOM4.IsPrimaryKey = false;
				colvarPackingSizeUOM4.IsForeignKey = false;
				colvarPackingSizeUOM4.IsReadOnly = false;
				colvarPackingSizeUOM4.DefaultSetting = @"";
				colvarPackingSizeUOM4.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPackingSizeUOM4);
				
				TableSchema.TableColumn colvarPackingSizeUOM5 = new TableSchema.TableColumn(schema);
				colvarPackingSizeUOM5.ColumnName = "PackingSizeUOM5";
				colvarPackingSizeUOM5.DataType = DbType.Decimal;
				colvarPackingSizeUOM5.MaxLength = 0;
				colvarPackingSizeUOM5.AutoIncrement = false;
				colvarPackingSizeUOM5.IsNullable = true;
				colvarPackingSizeUOM5.IsPrimaryKey = false;
				colvarPackingSizeUOM5.IsForeignKey = false;
				colvarPackingSizeUOM5.IsReadOnly = false;
				colvarPackingSizeUOM5.DefaultSetting = @"";
				colvarPackingSizeUOM5.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPackingSizeUOM5);
				
				TableSchema.TableColumn colvarPackingSizeUOM6 = new TableSchema.TableColumn(schema);
				colvarPackingSizeUOM6.ColumnName = "PackingSizeUOM6";
				colvarPackingSizeUOM6.DataType = DbType.Decimal;
				colvarPackingSizeUOM6.MaxLength = 0;
				colvarPackingSizeUOM6.AutoIncrement = false;
				colvarPackingSizeUOM6.IsNullable = true;
				colvarPackingSizeUOM6.IsPrimaryKey = false;
				colvarPackingSizeUOM6.IsForeignKey = false;
				colvarPackingSizeUOM6.IsReadOnly = false;
				colvarPackingSizeUOM6.DefaultSetting = @"";
				colvarPackingSizeUOM6.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPackingSizeUOM6);
				
				TableSchema.TableColumn colvarPackingSizeUOM7 = new TableSchema.TableColumn(schema);
				colvarPackingSizeUOM7.ColumnName = "PackingSizeUOM7";
				colvarPackingSizeUOM7.DataType = DbType.Decimal;
				colvarPackingSizeUOM7.MaxLength = 0;
				colvarPackingSizeUOM7.AutoIncrement = false;
				colvarPackingSizeUOM7.IsNullable = true;
				colvarPackingSizeUOM7.IsPrimaryKey = false;
				colvarPackingSizeUOM7.IsForeignKey = false;
				colvarPackingSizeUOM7.IsReadOnly = false;
				colvarPackingSizeUOM7.DefaultSetting = @"";
				colvarPackingSizeUOM7.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPackingSizeUOM7);
				
				TableSchema.TableColumn colvarPackingSizeUOM8 = new TableSchema.TableColumn(schema);
				colvarPackingSizeUOM8.ColumnName = "PackingSizeUOM8";
				colvarPackingSizeUOM8.DataType = DbType.Decimal;
				colvarPackingSizeUOM8.MaxLength = 0;
				colvarPackingSizeUOM8.AutoIncrement = false;
				colvarPackingSizeUOM8.IsNullable = true;
				colvarPackingSizeUOM8.IsPrimaryKey = false;
				colvarPackingSizeUOM8.IsForeignKey = false;
				colvarPackingSizeUOM8.IsReadOnly = false;
				colvarPackingSizeUOM8.DefaultSetting = @"";
				colvarPackingSizeUOM8.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPackingSizeUOM8);
				
				TableSchema.TableColumn colvarPackingSizeUOM9 = new TableSchema.TableColumn(schema);
				colvarPackingSizeUOM9.ColumnName = "PackingSizeUOM9";
				colvarPackingSizeUOM9.DataType = DbType.Decimal;
				colvarPackingSizeUOM9.MaxLength = 0;
				colvarPackingSizeUOM9.AutoIncrement = false;
				colvarPackingSizeUOM9.IsNullable = true;
				colvarPackingSizeUOM9.IsPrimaryKey = false;
				colvarPackingSizeUOM9.IsForeignKey = false;
				colvarPackingSizeUOM9.IsReadOnly = false;
				colvarPackingSizeUOM9.DefaultSetting = @"";
				colvarPackingSizeUOM9.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPackingSizeUOM9);
				
				TableSchema.TableColumn colvarPackingSizeUOM10 = new TableSchema.TableColumn(schema);
				colvarPackingSizeUOM10.ColumnName = "PackingSizeUOM10";
				colvarPackingSizeUOM10.DataType = DbType.Decimal;
				colvarPackingSizeUOM10.MaxLength = 0;
				colvarPackingSizeUOM10.AutoIncrement = false;
				colvarPackingSizeUOM10.IsNullable = true;
				colvarPackingSizeUOM10.IsPrimaryKey = false;
				colvarPackingSizeUOM10.IsForeignKey = false;
				colvarPackingSizeUOM10.IsReadOnly = false;
				colvarPackingSizeUOM10.DefaultSetting = @"";
				colvarPackingSizeUOM10.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPackingSizeUOM10);
				
				TableSchema.TableColumn colvarCostPrice1 = new TableSchema.TableColumn(schema);
				colvarCostPrice1.ColumnName = "CostPrice1";
				colvarCostPrice1.DataType = DbType.Currency;
				colvarCostPrice1.MaxLength = 0;
				colvarCostPrice1.AutoIncrement = false;
				colvarCostPrice1.IsNullable = true;
				colvarCostPrice1.IsPrimaryKey = false;
				colvarCostPrice1.IsForeignKey = false;
				colvarCostPrice1.IsReadOnly = false;
				colvarCostPrice1.DefaultSetting = @"";
				colvarCostPrice1.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCostPrice1);
				
				TableSchema.TableColumn colvarCostPrice2 = new TableSchema.TableColumn(schema);
				colvarCostPrice2.ColumnName = "CostPrice2";
				colvarCostPrice2.DataType = DbType.Currency;
				colvarCostPrice2.MaxLength = 0;
				colvarCostPrice2.AutoIncrement = false;
				colvarCostPrice2.IsNullable = true;
				colvarCostPrice2.IsPrimaryKey = false;
				colvarCostPrice2.IsForeignKey = false;
				colvarCostPrice2.IsReadOnly = false;
				colvarCostPrice2.DefaultSetting = @"";
				colvarCostPrice2.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCostPrice2);
				
				TableSchema.TableColumn colvarCostPrice3 = new TableSchema.TableColumn(schema);
				colvarCostPrice3.ColumnName = "CostPrice3";
				colvarCostPrice3.DataType = DbType.Currency;
				colvarCostPrice3.MaxLength = 0;
				colvarCostPrice3.AutoIncrement = false;
				colvarCostPrice3.IsNullable = true;
				colvarCostPrice3.IsPrimaryKey = false;
				colvarCostPrice3.IsForeignKey = false;
				colvarCostPrice3.IsReadOnly = false;
				colvarCostPrice3.DefaultSetting = @"";
				colvarCostPrice3.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCostPrice3);
				
				TableSchema.TableColumn colvarCostPrice4 = new TableSchema.TableColumn(schema);
				colvarCostPrice4.ColumnName = "CostPrice4";
				colvarCostPrice4.DataType = DbType.Currency;
				colvarCostPrice4.MaxLength = 0;
				colvarCostPrice4.AutoIncrement = false;
				colvarCostPrice4.IsNullable = true;
				colvarCostPrice4.IsPrimaryKey = false;
				colvarCostPrice4.IsForeignKey = false;
				colvarCostPrice4.IsReadOnly = false;
				colvarCostPrice4.DefaultSetting = @"";
				colvarCostPrice4.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCostPrice4);
				
				TableSchema.TableColumn colvarCostPrice5 = new TableSchema.TableColumn(schema);
				colvarCostPrice5.ColumnName = "CostPrice5";
				colvarCostPrice5.DataType = DbType.Currency;
				colvarCostPrice5.MaxLength = 0;
				colvarCostPrice5.AutoIncrement = false;
				colvarCostPrice5.IsNullable = true;
				colvarCostPrice5.IsPrimaryKey = false;
				colvarCostPrice5.IsForeignKey = false;
				colvarCostPrice5.IsReadOnly = false;
				colvarCostPrice5.DefaultSetting = @"";
				colvarCostPrice5.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCostPrice5);
				
				TableSchema.TableColumn colvarCostPrice6 = new TableSchema.TableColumn(schema);
				colvarCostPrice6.ColumnName = "CostPrice6";
				colvarCostPrice6.DataType = DbType.Currency;
				colvarCostPrice6.MaxLength = 0;
				colvarCostPrice6.AutoIncrement = false;
				colvarCostPrice6.IsNullable = true;
				colvarCostPrice6.IsPrimaryKey = false;
				colvarCostPrice6.IsForeignKey = false;
				colvarCostPrice6.IsReadOnly = false;
				colvarCostPrice6.DefaultSetting = @"";
				colvarCostPrice6.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCostPrice6);
				
				TableSchema.TableColumn colvarCostPrice7 = new TableSchema.TableColumn(schema);
				colvarCostPrice7.ColumnName = "CostPrice7";
				colvarCostPrice7.DataType = DbType.Currency;
				colvarCostPrice7.MaxLength = 0;
				colvarCostPrice7.AutoIncrement = false;
				colvarCostPrice7.IsNullable = true;
				colvarCostPrice7.IsPrimaryKey = false;
				colvarCostPrice7.IsForeignKey = false;
				colvarCostPrice7.IsReadOnly = false;
				colvarCostPrice7.DefaultSetting = @"";
				colvarCostPrice7.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCostPrice7);
				
				TableSchema.TableColumn colvarCostPrice8 = new TableSchema.TableColumn(schema);
				colvarCostPrice8.ColumnName = "CostPrice8";
				colvarCostPrice8.DataType = DbType.Currency;
				colvarCostPrice8.MaxLength = 0;
				colvarCostPrice8.AutoIncrement = false;
				colvarCostPrice8.IsNullable = true;
				colvarCostPrice8.IsPrimaryKey = false;
				colvarCostPrice8.IsForeignKey = false;
				colvarCostPrice8.IsReadOnly = false;
				colvarCostPrice8.DefaultSetting = @"";
				colvarCostPrice8.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCostPrice8);
				
				TableSchema.TableColumn colvarCostPrice9 = new TableSchema.TableColumn(schema);
				colvarCostPrice9.ColumnName = "CostPrice9";
				colvarCostPrice9.DataType = DbType.Currency;
				colvarCostPrice9.MaxLength = 0;
				colvarCostPrice9.AutoIncrement = false;
				colvarCostPrice9.IsNullable = true;
				colvarCostPrice9.IsPrimaryKey = false;
				colvarCostPrice9.IsForeignKey = false;
				colvarCostPrice9.IsReadOnly = false;
				colvarCostPrice9.DefaultSetting = @"";
				colvarCostPrice9.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCostPrice9);
				
				TableSchema.TableColumn colvarCostPrice10 = new TableSchema.TableColumn(schema);
				colvarCostPrice10.ColumnName = "CostPrice10";
				colvarCostPrice10.DataType = DbType.Currency;
				colvarCostPrice10.MaxLength = 0;
				colvarCostPrice10.AutoIncrement = false;
				colvarCostPrice10.IsNullable = true;
				colvarCostPrice10.IsPrimaryKey = false;
				colvarCostPrice10.IsForeignKey = false;
				colvarCostPrice10.IsReadOnly = false;
				colvarCostPrice10.DefaultSetting = @"";
				colvarCostPrice10.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCostPrice10);
				
				TableSchema.TableColumn colvarIsPreferredSupplier = new TableSchema.TableColumn(schema);
				colvarIsPreferredSupplier.ColumnName = "IsPreferredSupplier";
				colvarIsPreferredSupplier.DataType = DbType.Boolean;
				colvarIsPreferredSupplier.MaxLength = 0;
				colvarIsPreferredSupplier.AutoIncrement = false;
				colvarIsPreferredSupplier.IsNullable = true;
				colvarIsPreferredSupplier.IsPrimaryKey = false;
				colvarIsPreferredSupplier.IsForeignKey = false;
				colvarIsPreferredSupplier.IsReadOnly = false;
				colvarIsPreferredSupplier.DefaultSetting = @"";
				colvarIsPreferredSupplier.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIsPreferredSupplier);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["PowerPOS"].AddSchema("ItemSupplierMap",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("ItemSupplierMapID")]
		[Bindable(true)]
		public int ItemSupplierMapID 
		{
			get { return GetColumnValue<int>(Columns.ItemSupplierMapID); }
			set { SetColumnValue(Columns.ItemSupplierMapID, value); }
		}
		  
		[XmlAttribute("ItemNo")]
		[Bindable(true)]
		public string ItemNo 
		{
			get { return GetColumnValue<string>(Columns.ItemNo); }
			set { SetColumnValue(Columns.ItemNo, value); }
		}
		  
		[XmlAttribute("SupplierID")]
		[Bindable(true)]
		public int SupplierID 
		{
			get { return GetColumnValue<int>(Columns.SupplierID); }
			set { SetColumnValue(Columns.SupplierID, value); }
		}
		  
		[XmlAttribute("CostPrice")]
		[Bindable(true)]
		public decimal CostPrice 
		{
			get { return GetColumnValue<decimal>(Columns.CostPrice); }
			set { SetColumnValue(Columns.CostPrice, value); }
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
		  
		[XmlAttribute("Currency")]
		[Bindable(true)]
		public string Currency 
		{
			get { return GetColumnValue<string>(Columns.Currency); }
			set { SetColumnValue(Columns.Currency, value); }
		}
		  
		[XmlAttribute("Gst")]
		[Bindable(true)]
		public decimal? Gst 
		{
			get { return GetColumnValue<decimal?>(Columns.Gst); }
			set { SetColumnValue(Columns.Gst, value); }
		}
		  
		[XmlAttribute("GSTRule")]
		[Bindable(true)]
		public int? GSTRule 
		{
			get { return GetColumnValue<int?>(Columns.GSTRule); }
			set { SetColumnValue(Columns.GSTRule, value); }
		}
		  
		[XmlAttribute("PackingSize1")]
		[Bindable(true)]
		public string PackingSize1 
		{
			get { return GetColumnValue<string>(Columns.PackingSize1); }
			set { SetColumnValue(Columns.PackingSize1, value); }
		}
		  
		[XmlAttribute("PackingSize2")]
		[Bindable(true)]
		public string PackingSize2 
		{
			get { return GetColumnValue<string>(Columns.PackingSize2); }
			set { SetColumnValue(Columns.PackingSize2, value); }
		}
		  
		[XmlAttribute("PackingSize3")]
		[Bindable(true)]
		public string PackingSize3 
		{
			get { return GetColumnValue<string>(Columns.PackingSize3); }
			set { SetColumnValue(Columns.PackingSize3, value); }
		}
		  
		[XmlAttribute("PackingSize4")]
		[Bindable(true)]
		public string PackingSize4 
		{
			get { return GetColumnValue<string>(Columns.PackingSize4); }
			set { SetColumnValue(Columns.PackingSize4, value); }
		}
		  
		[XmlAttribute("PackingSize5")]
		[Bindable(true)]
		public string PackingSize5 
		{
			get { return GetColumnValue<string>(Columns.PackingSize5); }
			set { SetColumnValue(Columns.PackingSize5, value); }
		}
		  
		[XmlAttribute("PackingSize6")]
		[Bindable(true)]
		public string PackingSize6 
		{
			get { return GetColumnValue<string>(Columns.PackingSize6); }
			set { SetColumnValue(Columns.PackingSize6, value); }
		}
		  
		[XmlAttribute("PackingSize7")]
		[Bindable(true)]
		public string PackingSize7 
		{
			get { return GetColumnValue<string>(Columns.PackingSize7); }
			set { SetColumnValue(Columns.PackingSize7, value); }
		}
		  
		[XmlAttribute("PackingSize8")]
		[Bindable(true)]
		public string PackingSize8 
		{
			get { return GetColumnValue<string>(Columns.PackingSize8); }
			set { SetColumnValue(Columns.PackingSize8, value); }
		}
		  
		[XmlAttribute("PackingSize9")]
		[Bindable(true)]
		public string PackingSize9 
		{
			get { return GetColumnValue<string>(Columns.PackingSize9); }
			set { SetColumnValue(Columns.PackingSize9, value); }
		}
		  
		[XmlAttribute("PackingSize10")]
		[Bindable(true)]
		public string PackingSize10 
		{
			get { return GetColumnValue<string>(Columns.PackingSize10); }
			set { SetColumnValue(Columns.PackingSize10, value); }
		}
		  
		[XmlAttribute("PackingSizeUOM1")]
		[Bindable(true)]
		public decimal? PackingSizeUOM1 
		{
			get { return GetColumnValue<decimal?>(Columns.PackingSizeUOM1); }
			set { SetColumnValue(Columns.PackingSizeUOM1, value); }
		}
		  
		[XmlAttribute("PackingSizeUOM2")]
		[Bindable(true)]
		public decimal? PackingSizeUOM2 
		{
			get { return GetColumnValue<decimal?>(Columns.PackingSizeUOM2); }
			set { SetColumnValue(Columns.PackingSizeUOM2, value); }
		}
		  
		[XmlAttribute("PackingSizeUOM3")]
		[Bindable(true)]
		public decimal? PackingSizeUOM3 
		{
			get { return GetColumnValue<decimal?>(Columns.PackingSizeUOM3); }
			set { SetColumnValue(Columns.PackingSizeUOM3, value); }
		}
		  
		[XmlAttribute("PackingSizeUOM4")]
		[Bindable(true)]
		public decimal? PackingSizeUOM4 
		{
			get { return GetColumnValue<decimal?>(Columns.PackingSizeUOM4); }
			set { SetColumnValue(Columns.PackingSizeUOM4, value); }
		}
		  
		[XmlAttribute("PackingSizeUOM5")]
		[Bindable(true)]
		public decimal? PackingSizeUOM5 
		{
			get { return GetColumnValue<decimal?>(Columns.PackingSizeUOM5); }
			set { SetColumnValue(Columns.PackingSizeUOM5, value); }
		}
		  
		[XmlAttribute("PackingSizeUOM6")]
		[Bindable(true)]
		public decimal? PackingSizeUOM6 
		{
			get { return GetColumnValue<decimal?>(Columns.PackingSizeUOM6); }
			set { SetColumnValue(Columns.PackingSizeUOM6, value); }
		}
		  
		[XmlAttribute("PackingSizeUOM7")]
		[Bindable(true)]
		public decimal? PackingSizeUOM7 
		{
			get { return GetColumnValue<decimal?>(Columns.PackingSizeUOM7); }
			set { SetColumnValue(Columns.PackingSizeUOM7, value); }
		}
		  
		[XmlAttribute("PackingSizeUOM8")]
		[Bindable(true)]
		public decimal? PackingSizeUOM8 
		{
			get { return GetColumnValue<decimal?>(Columns.PackingSizeUOM8); }
			set { SetColumnValue(Columns.PackingSizeUOM8, value); }
		}
		  
		[XmlAttribute("PackingSizeUOM9")]
		[Bindable(true)]
		public decimal? PackingSizeUOM9 
		{
			get { return GetColumnValue<decimal?>(Columns.PackingSizeUOM9); }
			set { SetColumnValue(Columns.PackingSizeUOM9, value); }
		}
		  
		[XmlAttribute("PackingSizeUOM10")]
		[Bindable(true)]
		public decimal? PackingSizeUOM10 
		{
			get { return GetColumnValue<decimal?>(Columns.PackingSizeUOM10); }
			set { SetColumnValue(Columns.PackingSizeUOM10, value); }
		}
		  
		[XmlAttribute("CostPrice1")]
		[Bindable(true)]
		public decimal? CostPrice1 
		{
			get { return GetColumnValue<decimal?>(Columns.CostPrice1); }
			set { SetColumnValue(Columns.CostPrice1, value); }
		}
		  
		[XmlAttribute("CostPrice2")]
		[Bindable(true)]
		public decimal? CostPrice2 
		{
			get { return GetColumnValue<decimal?>(Columns.CostPrice2); }
			set { SetColumnValue(Columns.CostPrice2, value); }
		}
		  
		[XmlAttribute("CostPrice3")]
		[Bindable(true)]
		public decimal? CostPrice3 
		{
			get { return GetColumnValue<decimal?>(Columns.CostPrice3); }
			set { SetColumnValue(Columns.CostPrice3, value); }
		}
		  
		[XmlAttribute("CostPrice4")]
		[Bindable(true)]
		public decimal? CostPrice4 
		{
			get { return GetColumnValue<decimal?>(Columns.CostPrice4); }
			set { SetColumnValue(Columns.CostPrice4, value); }
		}
		  
		[XmlAttribute("CostPrice5")]
		[Bindable(true)]
		public decimal? CostPrice5 
		{
			get { return GetColumnValue<decimal?>(Columns.CostPrice5); }
			set { SetColumnValue(Columns.CostPrice5, value); }
		}
		  
		[XmlAttribute("CostPrice6")]
		[Bindable(true)]
		public decimal? CostPrice6 
		{
			get { return GetColumnValue<decimal?>(Columns.CostPrice6); }
			set { SetColumnValue(Columns.CostPrice6, value); }
		}
		  
		[XmlAttribute("CostPrice7")]
		[Bindable(true)]
		public decimal? CostPrice7 
		{
			get { return GetColumnValue<decimal?>(Columns.CostPrice7); }
			set { SetColumnValue(Columns.CostPrice7, value); }
		}
		  
		[XmlAttribute("CostPrice8")]
		[Bindable(true)]
		public decimal? CostPrice8 
		{
			get { return GetColumnValue<decimal?>(Columns.CostPrice8); }
			set { SetColumnValue(Columns.CostPrice8, value); }
		}
		  
		[XmlAttribute("CostPrice9")]
		[Bindable(true)]
		public decimal? CostPrice9 
		{
			get { return GetColumnValue<decimal?>(Columns.CostPrice9); }
			set { SetColumnValue(Columns.CostPrice9, value); }
		}
		  
		[XmlAttribute("CostPrice10")]
		[Bindable(true)]
		public decimal? CostPrice10 
		{
			get { return GetColumnValue<decimal?>(Columns.CostPrice10); }
			set { SetColumnValue(Columns.CostPrice10, value); }
		}
		  
		[XmlAttribute("IsPreferredSupplier")]
		[Bindable(true)]
		public bool? IsPreferredSupplier 
		{
			get { return GetColumnValue<bool?>(Columns.IsPreferredSupplier); }
			set { SetColumnValue(Columns.IsPreferredSupplier, value); }
		}
		
		#endregion
		
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a Item ActiveRecord object related to this ItemSupplierMap
		/// 
		/// </summary>
		public PowerPOS.Item Item
		{
			get { return PowerPOS.Item.FetchByID(this.ItemNo); }
			set { SetColumnValue("ItemNo", value.ItemNo); }
		}
		
		
		/// <summary>
		/// Returns a Supplier ActiveRecord object related to this ItemSupplierMap
		/// 
		/// </summary>
		public PowerPOS.Supplier Supplier
		{
			get { return PowerPOS.Supplier.FetchByID(this.SupplierID); }
			set { SetColumnValue("SupplierID", value.SupplierID); }
		}
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(string varItemNo,int varSupplierID,decimal varCostPrice,DateTime? varCreatedOn,string varCreatedBy,DateTime? varModifiedOn,string varModifiedBy,bool varDeleted,string varCurrency,decimal? varGst,int? varGSTRule,string varPackingSize1,string varPackingSize2,string varPackingSize3,string varPackingSize4,string varPackingSize5,string varPackingSize6,string varPackingSize7,string varPackingSize8,string varPackingSize9,string varPackingSize10,decimal? varPackingSizeUOM1,decimal? varPackingSizeUOM2,decimal? varPackingSizeUOM3,decimal? varPackingSizeUOM4,decimal? varPackingSizeUOM5,decimal? varPackingSizeUOM6,decimal? varPackingSizeUOM7,decimal? varPackingSizeUOM8,decimal? varPackingSizeUOM9,decimal? varPackingSizeUOM10,decimal? varCostPrice1,decimal? varCostPrice2,decimal? varCostPrice3,decimal? varCostPrice4,decimal? varCostPrice5,decimal? varCostPrice6,decimal? varCostPrice7,decimal? varCostPrice8,decimal? varCostPrice9,decimal? varCostPrice10,bool? varIsPreferredSupplier)
		{
			ItemSupplierMap item = new ItemSupplierMap();
			
			item.ItemNo = varItemNo;
			
			item.SupplierID = varSupplierID;
			
			item.CostPrice = varCostPrice;
			
			item.CreatedOn = varCreatedOn;
			
			item.CreatedBy = varCreatedBy;
			
			item.ModifiedOn = varModifiedOn;
			
			item.ModifiedBy = varModifiedBy;
			
			item.Deleted = varDeleted;
			
			item.Currency = varCurrency;
			
			item.Gst = varGst;
			
			item.GSTRule = varGSTRule;
			
			item.PackingSize1 = varPackingSize1;
			
			item.PackingSize2 = varPackingSize2;
			
			item.PackingSize3 = varPackingSize3;
			
			item.PackingSize4 = varPackingSize4;
			
			item.PackingSize5 = varPackingSize5;
			
			item.PackingSize6 = varPackingSize6;
			
			item.PackingSize7 = varPackingSize7;
			
			item.PackingSize8 = varPackingSize8;
			
			item.PackingSize9 = varPackingSize9;
			
			item.PackingSize10 = varPackingSize10;
			
			item.PackingSizeUOM1 = varPackingSizeUOM1;
			
			item.PackingSizeUOM2 = varPackingSizeUOM2;
			
			item.PackingSizeUOM3 = varPackingSizeUOM3;
			
			item.PackingSizeUOM4 = varPackingSizeUOM4;
			
			item.PackingSizeUOM5 = varPackingSizeUOM5;
			
			item.PackingSizeUOM6 = varPackingSizeUOM6;
			
			item.PackingSizeUOM7 = varPackingSizeUOM7;
			
			item.PackingSizeUOM8 = varPackingSizeUOM8;
			
			item.PackingSizeUOM9 = varPackingSizeUOM9;
			
			item.PackingSizeUOM10 = varPackingSizeUOM10;
			
			item.CostPrice1 = varCostPrice1;
			
			item.CostPrice2 = varCostPrice2;
			
			item.CostPrice3 = varCostPrice3;
			
			item.CostPrice4 = varCostPrice4;
			
			item.CostPrice5 = varCostPrice5;
			
			item.CostPrice6 = varCostPrice6;
			
			item.CostPrice7 = varCostPrice7;
			
			item.CostPrice8 = varCostPrice8;
			
			item.CostPrice9 = varCostPrice9;
			
			item.CostPrice10 = varCostPrice10;
			
			item.IsPreferredSupplier = varIsPreferredSupplier;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varItemSupplierMapID,string varItemNo,int varSupplierID,decimal varCostPrice,DateTime? varCreatedOn,string varCreatedBy,DateTime? varModifiedOn,string varModifiedBy,bool varDeleted,string varCurrency,decimal? varGst,int? varGSTRule,string varPackingSize1,string varPackingSize2,string varPackingSize3,string varPackingSize4,string varPackingSize5,string varPackingSize6,string varPackingSize7,string varPackingSize8,string varPackingSize9,string varPackingSize10,decimal? varPackingSizeUOM1,decimal? varPackingSizeUOM2,decimal? varPackingSizeUOM3,decimal? varPackingSizeUOM4,decimal? varPackingSizeUOM5,decimal? varPackingSizeUOM6,decimal? varPackingSizeUOM7,decimal? varPackingSizeUOM8,decimal? varPackingSizeUOM9,decimal? varPackingSizeUOM10,decimal? varCostPrice1,decimal? varCostPrice2,decimal? varCostPrice3,decimal? varCostPrice4,decimal? varCostPrice5,decimal? varCostPrice6,decimal? varCostPrice7,decimal? varCostPrice8,decimal? varCostPrice9,decimal? varCostPrice10,bool? varIsPreferredSupplier)
		{
			ItemSupplierMap item = new ItemSupplierMap();
			
				item.ItemSupplierMapID = varItemSupplierMapID;
			
				item.ItemNo = varItemNo;
			
				item.SupplierID = varSupplierID;
			
				item.CostPrice = varCostPrice;
			
				item.CreatedOn = varCreatedOn;
			
				item.CreatedBy = varCreatedBy;
			
				item.ModifiedOn = varModifiedOn;
			
				item.ModifiedBy = varModifiedBy;
			
				item.Deleted = varDeleted;
			
				item.Currency = varCurrency;
			
				item.Gst = varGst;
			
				item.GSTRule = varGSTRule;
			
				item.PackingSize1 = varPackingSize1;
			
				item.PackingSize2 = varPackingSize2;
			
				item.PackingSize3 = varPackingSize3;
			
				item.PackingSize4 = varPackingSize4;
			
				item.PackingSize5 = varPackingSize5;
			
				item.PackingSize6 = varPackingSize6;
			
				item.PackingSize7 = varPackingSize7;
			
				item.PackingSize8 = varPackingSize8;
			
				item.PackingSize9 = varPackingSize9;
			
				item.PackingSize10 = varPackingSize10;
			
				item.PackingSizeUOM1 = varPackingSizeUOM1;
			
				item.PackingSizeUOM2 = varPackingSizeUOM2;
			
				item.PackingSizeUOM3 = varPackingSizeUOM3;
			
				item.PackingSizeUOM4 = varPackingSizeUOM4;
			
				item.PackingSizeUOM5 = varPackingSizeUOM5;
			
				item.PackingSizeUOM6 = varPackingSizeUOM6;
			
				item.PackingSizeUOM7 = varPackingSizeUOM7;
			
				item.PackingSizeUOM8 = varPackingSizeUOM8;
			
				item.PackingSizeUOM9 = varPackingSizeUOM9;
			
				item.PackingSizeUOM10 = varPackingSizeUOM10;
			
				item.CostPrice1 = varCostPrice1;
			
				item.CostPrice2 = varCostPrice2;
			
				item.CostPrice3 = varCostPrice3;
			
				item.CostPrice4 = varCostPrice4;
			
				item.CostPrice5 = varCostPrice5;
			
				item.CostPrice6 = varCostPrice6;
			
				item.CostPrice7 = varCostPrice7;
			
				item.CostPrice8 = varCostPrice8;
			
				item.CostPrice9 = varCostPrice9;
			
				item.CostPrice10 = varCostPrice10;
			
				item.IsPreferredSupplier = varIsPreferredSupplier;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn ItemSupplierMapIDColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn ItemNoColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn SupplierIDColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn CostPriceColumn
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
        
        
        
        public static TableSchema.TableColumn CurrencyColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn GstColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        public static TableSchema.TableColumn GSTRuleColumn
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        public static TableSchema.TableColumn PackingSize1Column
        {
            get { return Schema.Columns[12]; }
        }
        
        
        
        public static TableSchema.TableColumn PackingSize2Column
        {
            get { return Schema.Columns[13]; }
        }
        
        
        
        public static TableSchema.TableColumn PackingSize3Column
        {
            get { return Schema.Columns[14]; }
        }
        
        
        
        public static TableSchema.TableColumn PackingSize4Column
        {
            get { return Schema.Columns[15]; }
        }
        
        
        
        public static TableSchema.TableColumn PackingSize5Column
        {
            get { return Schema.Columns[16]; }
        }
        
        
        
        public static TableSchema.TableColumn PackingSize6Column
        {
            get { return Schema.Columns[17]; }
        }
        
        
        
        public static TableSchema.TableColumn PackingSize7Column
        {
            get { return Schema.Columns[18]; }
        }
        
        
        
        public static TableSchema.TableColumn PackingSize8Column
        {
            get { return Schema.Columns[19]; }
        }
        
        
        
        public static TableSchema.TableColumn PackingSize9Column
        {
            get { return Schema.Columns[20]; }
        }
        
        
        
        public static TableSchema.TableColumn PackingSize10Column
        {
            get { return Schema.Columns[21]; }
        }
        
        
        
        public static TableSchema.TableColumn PackingSizeUOM1Column
        {
            get { return Schema.Columns[22]; }
        }
        
        
        
        public static TableSchema.TableColumn PackingSizeUOM2Column
        {
            get { return Schema.Columns[23]; }
        }
        
        
        
        public static TableSchema.TableColumn PackingSizeUOM3Column
        {
            get { return Schema.Columns[24]; }
        }
        
        
        
        public static TableSchema.TableColumn PackingSizeUOM4Column
        {
            get { return Schema.Columns[25]; }
        }
        
        
        
        public static TableSchema.TableColumn PackingSizeUOM5Column
        {
            get { return Schema.Columns[26]; }
        }
        
        
        
        public static TableSchema.TableColumn PackingSizeUOM6Column
        {
            get { return Schema.Columns[27]; }
        }
        
        
        
        public static TableSchema.TableColumn PackingSizeUOM7Column
        {
            get { return Schema.Columns[28]; }
        }
        
        
        
        public static TableSchema.TableColumn PackingSizeUOM8Column
        {
            get { return Schema.Columns[29]; }
        }
        
        
        
        public static TableSchema.TableColumn PackingSizeUOM9Column
        {
            get { return Schema.Columns[30]; }
        }
        
        
        
        public static TableSchema.TableColumn PackingSizeUOM10Column
        {
            get { return Schema.Columns[31]; }
        }
        
        
        
        public static TableSchema.TableColumn CostPrice1Column
        {
            get { return Schema.Columns[32]; }
        }
        
        
        
        public static TableSchema.TableColumn CostPrice2Column
        {
            get { return Schema.Columns[33]; }
        }
        
        
        
        public static TableSchema.TableColumn CostPrice3Column
        {
            get { return Schema.Columns[34]; }
        }
        
        
        
        public static TableSchema.TableColumn CostPrice4Column
        {
            get { return Schema.Columns[35]; }
        }
        
        
        
        public static TableSchema.TableColumn CostPrice5Column
        {
            get { return Schema.Columns[36]; }
        }
        
        
        
        public static TableSchema.TableColumn CostPrice6Column
        {
            get { return Schema.Columns[37]; }
        }
        
        
        
        public static TableSchema.TableColumn CostPrice7Column
        {
            get { return Schema.Columns[38]; }
        }
        
        
        
        public static TableSchema.TableColumn CostPrice8Column
        {
            get { return Schema.Columns[39]; }
        }
        
        
        
        public static TableSchema.TableColumn CostPrice9Column
        {
            get { return Schema.Columns[40]; }
        }
        
        
        
        public static TableSchema.TableColumn CostPrice10Column
        {
            get { return Schema.Columns[41]; }
        }
        
        
        
        public static TableSchema.TableColumn IsPreferredSupplierColumn
        {
            get { return Schema.Columns[42]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string ItemSupplierMapID = @"ItemSupplierMapID";
			 public static string ItemNo = @"ItemNo";
			 public static string SupplierID = @"SupplierID";
			 public static string CostPrice = @"CostPrice";
			 public static string CreatedOn = @"CreatedOn";
			 public static string CreatedBy = @"CreatedBy";
			 public static string ModifiedOn = @"ModifiedOn";
			 public static string ModifiedBy = @"ModifiedBy";
			 public static string Deleted = @"Deleted";
			 public static string Currency = @"Currency";
			 public static string Gst = @"GST";
			 public static string GSTRule = @"GSTRule";
			 public static string PackingSize1 = @"PackingSize1";
			 public static string PackingSize2 = @"PackingSize2";
			 public static string PackingSize3 = @"PackingSize3";
			 public static string PackingSize4 = @"PackingSize4";
			 public static string PackingSize5 = @"PackingSize5";
			 public static string PackingSize6 = @"PackingSize6";
			 public static string PackingSize7 = @"PackingSize7";
			 public static string PackingSize8 = @"PackingSize8";
			 public static string PackingSize9 = @"PackingSize9";
			 public static string PackingSize10 = @"PackingSize10";
			 public static string PackingSizeUOM1 = @"PackingSizeUOM1";
			 public static string PackingSizeUOM2 = @"PackingSizeUOM2";
			 public static string PackingSizeUOM3 = @"PackingSizeUOM3";
			 public static string PackingSizeUOM4 = @"PackingSizeUOM4";
			 public static string PackingSizeUOM5 = @"PackingSizeUOM5";
			 public static string PackingSizeUOM6 = @"PackingSizeUOM6";
			 public static string PackingSizeUOM7 = @"PackingSizeUOM7";
			 public static string PackingSizeUOM8 = @"PackingSizeUOM8";
			 public static string PackingSizeUOM9 = @"PackingSizeUOM9";
			 public static string PackingSizeUOM10 = @"PackingSizeUOM10";
			 public static string CostPrice1 = @"CostPrice1";
			 public static string CostPrice2 = @"CostPrice2";
			 public static string CostPrice3 = @"CostPrice3";
			 public static string CostPrice4 = @"CostPrice4";
			 public static string CostPrice5 = @"CostPrice5";
			 public static string CostPrice6 = @"CostPrice6";
			 public static string CostPrice7 = @"CostPrice7";
			 public static string CostPrice8 = @"CostPrice8";
			 public static string CostPrice9 = @"CostPrice9";
			 public static string CostPrice10 = @"CostPrice10";
			 public static string IsPreferredSupplier = @"IsPreferredSupplier";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
