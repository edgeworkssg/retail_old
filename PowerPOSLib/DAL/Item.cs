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
	/// Strongly-typed collection for the Item class.
	/// </summary>
    [Serializable]
	public partial class ItemCollection : ActiveList<Item, ItemCollection>
	{	   
		public ItemCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>ItemCollection</returns>
		public ItemCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                Item o = this[i];
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
	/// This is an ActiveRecord class which wraps the Item table.
	/// </summary>
	[Serializable]
	public partial class Item : ActiveRecord<Item>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public Item()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public Item(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public Item(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public Item(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("Item", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarItemNo = new TableSchema.TableColumn(schema);
				colvarItemNo.ColumnName = "ItemNo";
				colvarItemNo.DataType = DbType.AnsiString;
				colvarItemNo.MaxLength = 50;
				colvarItemNo.AutoIncrement = false;
				colvarItemNo.IsNullable = false;
				colvarItemNo.IsPrimaryKey = true;
				colvarItemNo.IsForeignKey = false;
				colvarItemNo.IsReadOnly = false;
				colvarItemNo.DefaultSetting = @"";
				colvarItemNo.ForeignKeyTableName = "";
				schema.Columns.Add(colvarItemNo);
				
				TableSchema.TableColumn colvarItemName = new TableSchema.TableColumn(schema);
				colvarItemName.ColumnName = "ItemName";
				colvarItemName.DataType = DbType.String;
				colvarItemName.MaxLength = 300;
				colvarItemName.AutoIncrement = false;
				colvarItemName.IsNullable = false;
				colvarItemName.IsPrimaryKey = false;
				colvarItemName.IsForeignKey = false;
				colvarItemName.IsReadOnly = false;
				colvarItemName.DefaultSetting = @"";
				colvarItemName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarItemName);
				
				TableSchema.TableColumn colvarBarcode = new TableSchema.TableColumn(schema);
				colvarBarcode.ColumnName = "Barcode";
				colvarBarcode.DataType = DbType.AnsiString;
				colvarBarcode.MaxLength = 100;
				colvarBarcode.AutoIncrement = false;
				colvarBarcode.IsNullable = true;
				colvarBarcode.IsPrimaryKey = false;
				colvarBarcode.IsForeignKey = false;
				colvarBarcode.IsReadOnly = false;
				colvarBarcode.DefaultSetting = @"";
				colvarBarcode.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBarcode);
				
				TableSchema.TableColumn colvarCategoryName = new TableSchema.TableColumn(schema);
				colvarCategoryName.ColumnName = "CategoryName";
				colvarCategoryName.DataType = DbType.String;
				colvarCategoryName.MaxLength = 250;
				colvarCategoryName.AutoIncrement = false;
				colvarCategoryName.IsNullable = false;
				colvarCategoryName.IsPrimaryKey = false;
				colvarCategoryName.IsForeignKey = true;
				colvarCategoryName.IsReadOnly = false;
				colvarCategoryName.DefaultSetting = @"";
				
					colvarCategoryName.ForeignKeyTableName = "Category";
				schema.Columns.Add(colvarCategoryName);
				
				TableSchema.TableColumn colvarRetailPrice = new TableSchema.TableColumn(schema);
				colvarRetailPrice.ColumnName = "RetailPrice";
				colvarRetailPrice.DataType = DbType.Currency;
				colvarRetailPrice.MaxLength = 0;
				colvarRetailPrice.AutoIncrement = false;
				colvarRetailPrice.IsNullable = false;
				colvarRetailPrice.IsPrimaryKey = false;
				colvarRetailPrice.IsForeignKey = false;
				colvarRetailPrice.IsReadOnly = false;
				
						colvarRetailPrice.DefaultSetting = @"((0))";
				colvarRetailPrice.ForeignKeyTableName = "";
				schema.Columns.Add(colvarRetailPrice);
				
				TableSchema.TableColumn colvarFactoryPrice = new TableSchema.TableColumn(schema);
				colvarFactoryPrice.ColumnName = "FactoryPrice";
				colvarFactoryPrice.DataType = DbType.Currency;
				colvarFactoryPrice.MaxLength = 0;
				colvarFactoryPrice.AutoIncrement = false;
				colvarFactoryPrice.IsNullable = false;
				colvarFactoryPrice.IsPrimaryKey = false;
				colvarFactoryPrice.IsForeignKey = false;
				colvarFactoryPrice.IsReadOnly = false;
				
						colvarFactoryPrice.DefaultSetting = @"((0))";
				colvarFactoryPrice.ForeignKeyTableName = "";
				schema.Columns.Add(colvarFactoryPrice);
				
				TableSchema.TableColumn colvarMinimumPrice = new TableSchema.TableColumn(schema);
				colvarMinimumPrice.ColumnName = "MinimumPrice";
				colvarMinimumPrice.DataType = DbType.Currency;
				colvarMinimumPrice.MaxLength = 0;
				colvarMinimumPrice.AutoIncrement = false;
				colvarMinimumPrice.IsNullable = false;
				colvarMinimumPrice.IsPrimaryKey = false;
				colvarMinimumPrice.IsForeignKey = false;
				colvarMinimumPrice.IsReadOnly = false;
				
						colvarMinimumPrice.DefaultSetting = @"((0))";
				colvarMinimumPrice.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMinimumPrice);
				
				TableSchema.TableColumn colvarItemDesc = new TableSchema.TableColumn(schema);
				colvarItemDesc.ColumnName = "ItemDesc";
				colvarItemDesc.DataType = DbType.String;
				colvarItemDesc.MaxLength = 250;
				colvarItemDesc.AutoIncrement = false;
				colvarItemDesc.IsNullable = true;
				colvarItemDesc.IsPrimaryKey = false;
				colvarItemDesc.IsForeignKey = false;
				colvarItemDesc.IsReadOnly = false;
				colvarItemDesc.DefaultSetting = @"";
				colvarItemDesc.ForeignKeyTableName = "";
				schema.Columns.Add(colvarItemDesc);
				
				TableSchema.TableColumn colvarIsServiceItem = new TableSchema.TableColumn(schema);
				colvarIsServiceItem.ColumnName = "IsServiceItem";
				colvarIsServiceItem.DataType = DbType.Boolean;
				colvarIsServiceItem.MaxLength = 0;
				colvarIsServiceItem.AutoIncrement = false;
				colvarIsServiceItem.IsNullable = true;
				colvarIsServiceItem.IsPrimaryKey = false;
				colvarIsServiceItem.IsForeignKey = false;
				colvarIsServiceItem.IsReadOnly = false;
				colvarIsServiceItem.DefaultSetting = @"";
				colvarIsServiceItem.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIsServiceItem);
				
				TableSchema.TableColumn colvarIsInInventory = new TableSchema.TableColumn(schema);
				colvarIsInInventory.ColumnName = "IsInInventory";
				colvarIsInInventory.DataType = DbType.Boolean;
				colvarIsInInventory.MaxLength = 0;
				colvarIsInInventory.AutoIncrement = false;
				colvarIsInInventory.IsNullable = false;
				colvarIsInInventory.IsPrimaryKey = false;
				colvarIsInInventory.IsForeignKey = false;
				colvarIsInInventory.IsReadOnly = false;
				
						colvarIsInInventory.DefaultSetting = @"((0))";
				colvarIsInInventory.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIsInInventory);
				
				TableSchema.TableColumn colvarIsNonDiscountable = new TableSchema.TableColumn(schema);
				colvarIsNonDiscountable.ColumnName = "IsNonDiscountable";
				colvarIsNonDiscountable.DataType = DbType.Boolean;
				colvarIsNonDiscountable.MaxLength = 0;
				colvarIsNonDiscountable.AutoIncrement = false;
				colvarIsNonDiscountable.IsNullable = false;
				colvarIsNonDiscountable.IsPrimaryKey = false;
				colvarIsNonDiscountable.IsForeignKey = false;
				colvarIsNonDiscountable.IsReadOnly = false;
				
						colvarIsNonDiscountable.DefaultSetting = @"((0))";
				colvarIsNonDiscountable.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIsNonDiscountable);
				
				TableSchema.TableColumn colvarIsCourse = new TableSchema.TableColumn(schema);
				colvarIsCourse.ColumnName = "IsCourse";
				colvarIsCourse.DataType = DbType.Boolean;
				colvarIsCourse.MaxLength = 0;
				colvarIsCourse.AutoIncrement = false;
				colvarIsCourse.IsNullable = true;
				colvarIsCourse.IsPrimaryKey = false;
				colvarIsCourse.IsForeignKey = false;
				colvarIsCourse.IsReadOnly = false;
				colvarIsCourse.DefaultSetting = @"";
				colvarIsCourse.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIsCourse);
				
				TableSchema.TableColumn colvarCourseTypeID = new TableSchema.TableColumn(schema);
				colvarCourseTypeID.ColumnName = "CourseTypeID";
				colvarCourseTypeID.DataType = DbType.AnsiString;
				colvarCourseTypeID.MaxLength = 50;
				colvarCourseTypeID.AutoIncrement = false;
				colvarCourseTypeID.IsNullable = true;
				colvarCourseTypeID.IsPrimaryKey = false;
				colvarCourseTypeID.IsForeignKey = true;
				colvarCourseTypeID.IsReadOnly = false;
				colvarCourseTypeID.DefaultSetting = @"";
				
					colvarCourseTypeID.ForeignKeyTableName = "CourseType";
				schema.Columns.Add(colvarCourseTypeID);
				
				TableSchema.TableColumn colvarBrand = new TableSchema.TableColumn(schema);
				colvarBrand.ColumnName = "Brand";
				colvarBrand.DataType = DbType.String;
				colvarBrand.MaxLength = 50;
				colvarBrand.AutoIncrement = false;
				colvarBrand.IsNullable = true;
				colvarBrand.IsPrimaryKey = false;
				colvarBrand.IsForeignKey = false;
				colvarBrand.IsReadOnly = false;
				colvarBrand.DefaultSetting = @"";
				colvarBrand.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBrand);
				
				TableSchema.TableColumn colvarProductLine = new TableSchema.TableColumn(schema);
				colvarProductLine.ColumnName = "ProductLine";
				colvarProductLine.DataType = DbType.AnsiString;
				colvarProductLine.MaxLength = 50;
				colvarProductLine.AutoIncrement = false;
				colvarProductLine.IsNullable = true;
				colvarProductLine.IsPrimaryKey = false;
				colvarProductLine.IsForeignKey = false;
				colvarProductLine.IsReadOnly = false;
				colvarProductLine.DefaultSetting = @"";
				colvarProductLine.ForeignKeyTableName = "";
				schema.Columns.Add(colvarProductLine);
				
				TableSchema.TableColumn colvarAttributes1 = new TableSchema.TableColumn(schema);
				colvarAttributes1.ColumnName = "Attributes1";
				colvarAttributes1.DataType = DbType.String;
				colvarAttributes1.MaxLength = -1;
				colvarAttributes1.AutoIncrement = false;
				colvarAttributes1.IsNullable = true;
				colvarAttributes1.IsPrimaryKey = false;
				colvarAttributes1.IsForeignKey = false;
				colvarAttributes1.IsReadOnly = false;
				colvarAttributes1.DefaultSetting = @"";
				colvarAttributes1.ForeignKeyTableName = "";
				schema.Columns.Add(colvarAttributes1);
				
				TableSchema.TableColumn colvarAttributes2 = new TableSchema.TableColumn(schema);
				colvarAttributes2.ColumnName = "Attributes2";
				colvarAttributes2.DataType = DbType.String;
				colvarAttributes2.MaxLength = -1;
				colvarAttributes2.AutoIncrement = false;
				colvarAttributes2.IsNullable = true;
				colvarAttributes2.IsPrimaryKey = false;
				colvarAttributes2.IsForeignKey = false;
				colvarAttributes2.IsReadOnly = false;
				colvarAttributes2.DefaultSetting = @"";
				colvarAttributes2.ForeignKeyTableName = "";
				schema.Columns.Add(colvarAttributes2);
				
				TableSchema.TableColumn colvarAttributes3 = new TableSchema.TableColumn(schema);
				colvarAttributes3.ColumnName = "Attributes3";
				colvarAttributes3.DataType = DbType.String;
				colvarAttributes3.MaxLength = -1;
				colvarAttributes3.AutoIncrement = false;
				colvarAttributes3.IsNullable = true;
				colvarAttributes3.IsPrimaryKey = false;
				colvarAttributes3.IsForeignKey = false;
				colvarAttributes3.IsReadOnly = false;
				colvarAttributes3.DefaultSetting = @"";
				colvarAttributes3.ForeignKeyTableName = "";
				schema.Columns.Add(colvarAttributes3);
				
				TableSchema.TableColumn colvarAttributes4 = new TableSchema.TableColumn(schema);
				colvarAttributes4.ColumnName = "Attributes4";
				colvarAttributes4.DataType = DbType.String;
				colvarAttributes4.MaxLength = -1;
				colvarAttributes4.AutoIncrement = false;
				colvarAttributes4.IsNullable = true;
				colvarAttributes4.IsPrimaryKey = false;
				colvarAttributes4.IsForeignKey = false;
				colvarAttributes4.IsReadOnly = false;
				colvarAttributes4.DefaultSetting = @"";
				colvarAttributes4.ForeignKeyTableName = "";
				schema.Columns.Add(colvarAttributes4);
				
				TableSchema.TableColumn colvarAttributes5 = new TableSchema.TableColumn(schema);
				colvarAttributes5.ColumnName = "Attributes5";
				colvarAttributes5.DataType = DbType.String;
				colvarAttributes5.MaxLength = -1;
				colvarAttributes5.AutoIncrement = false;
				colvarAttributes5.IsNullable = true;
				colvarAttributes5.IsPrimaryKey = false;
				colvarAttributes5.IsForeignKey = false;
				colvarAttributes5.IsReadOnly = false;
				colvarAttributes5.DefaultSetting = @"";
				colvarAttributes5.ForeignKeyTableName = "";
				schema.Columns.Add(colvarAttributes5);
				
				TableSchema.TableColumn colvarAttributes6 = new TableSchema.TableColumn(schema);
				colvarAttributes6.ColumnName = "Attributes6";
				colvarAttributes6.DataType = DbType.String;
				colvarAttributes6.MaxLength = -1;
				colvarAttributes6.AutoIncrement = false;
				colvarAttributes6.IsNullable = true;
				colvarAttributes6.IsPrimaryKey = false;
				colvarAttributes6.IsForeignKey = false;
				colvarAttributes6.IsReadOnly = false;
				colvarAttributes6.DefaultSetting = @"";
				colvarAttributes6.ForeignKeyTableName = "";
				schema.Columns.Add(colvarAttributes6);
				
				TableSchema.TableColumn colvarAttributes7 = new TableSchema.TableColumn(schema);
				colvarAttributes7.ColumnName = "Attributes7";
				colvarAttributes7.DataType = DbType.String;
				colvarAttributes7.MaxLength = -1;
				colvarAttributes7.AutoIncrement = false;
				colvarAttributes7.IsNullable = true;
				colvarAttributes7.IsPrimaryKey = false;
				colvarAttributes7.IsForeignKey = false;
				colvarAttributes7.IsReadOnly = false;
				colvarAttributes7.DefaultSetting = @"";
				colvarAttributes7.ForeignKeyTableName = "";
				schema.Columns.Add(colvarAttributes7);
				
				TableSchema.TableColumn colvarAttributes8 = new TableSchema.TableColumn(schema);
				colvarAttributes8.ColumnName = "Attributes8";
				colvarAttributes8.DataType = DbType.String;
				colvarAttributes8.MaxLength = -1;
				colvarAttributes8.AutoIncrement = false;
				colvarAttributes8.IsNullable = true;
				colvarAttributes8.IsPrimaryKey = false;
				colvarAttributes8.IsForeignKey = false;
				colvarAttributes8.IsReadOnly = false;
				colvarAttributes8.DefaultSetting = @"";
				colvarAttributes8.ForeignKeyTableName = "";
				schema.Columns.Add(colvarAttributes8);
				
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
				
				TableSchema.TableColumn colvarProductionDate = new TableSchema.TableColumn(schema);
				colvarProductionDate.ColumnName = "ProductionDate";
				colvarProductionDate.DataType = DbType.DateTime;
				colvarProductionDate.MaxLength = 0;
				colvarProductionDate.AutoIncrement = false;
				colvarProductionDate.IsNullable = true;
				colvarProductionDate.IsPrimaryKey = false;
				colvarProductionDate.IsForeignKey = false;
				colvarProductionDate.IsReadOnly = false;
				colvarProductionDate.DefaultSetting = @"";
				colvarProductionDate.ForeignKeyTableName = "";
				schema.Columns.Add(colvarProductionDate);
				
				TableSchema.TableColumn colvarIsGST = new TableSchema.TableColumn(schema);
				colvarIsGST.ColumnName = "IsGST";
				colvarIsGST.DataType = DbType.Boolean;
				colvarIsGST.MaxLength = 0;
				colvarIsGST.AutoIncrement = false;
				colvarIsGST.IsNullable = true;
				colvarIsGST.IsPrimaryKey = false;
				colvarIsGST.IsForeignKey = false;
				colvarIsGST.IsReadOnly = false;
				colvarIsGST.DefaultSetting = @"";
				colvarIsGST.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIsGST);
				
				TableSchema.TableColumn colvarHasWarranty = new TableSchema.TableColumn(schema);
				colvarHasWarranty.ColumnName = "hasWarranty";
				colvarHasWarranty.DataType = DbType.Boolean;
				colvarHasWarranty.MaxLength = 0;
				colvarHasWarranty.AutoIncrement = false;
				colvarHasWarranty.IsNullable = true;
				colvarHasWarranty.IsPrimaryKey = false;
				colvarHasWarranty.IsForeignKey = false;
				colvarHasWarranty.IsReadOnly = false;
				colvarHasWarranty.DefaultSetting = @"";
				colvarHasWarranty.ForeignKeyTableName = "";
				schema.Columns.Add(colvarHasWarranty);
				
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
				
				TableSchema.TableColumn colvarUserfld1 = new TableSchema.TableColumn(schema);
				colvarUserfld1.ColumnName = "userfld1";
				colvarUserfld1.DataType = DbType.AnsiString;
				colvarUserfld1.MaxLength = 50;
				colvarUserfld1.AutoIncrement = false;
				colvarUserfld1.IsNullable = true;
				colvarUserfld1.IsPrimaryKey = false;
				colvarUserfld1.IsForeignKey = false;
				colvarUserfld1.IsReadOnly = false;
				colvarUserfld1.DefaultSetting = @"";
				colvarUserfld1.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfld1);
				
				TableSchema.TableColumn colvarUserfld2 = new TableSchema.TableColumn(schema);
				colvarUserfld2.ColumnName = "userfld2";
				colvarUserfld2.DataType = DbType.AnsiString;
				colvarUserfld2.MaxLength = 50;
				colvarUserfld2.AutoIncrement = false;
				colvarUserfld2.IsNullable = true;
				colvarUserfld2.IsPrimaryKey = false;
				colvarUserfld2.IsForeignKey = false;
				colvarUserfld2.IsReadOnly = false;
				colvarUserfld2.DefaultSetting = @"";
				colvarUserfld2.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfld2);
				
				TableSchema.TableColumn colvarUserfld3 = new TableSchema.TableColumn(schema);
				colvarUserfld3.ColumnName = "userfld3";
				colvarUserfld3.DataType = DbType.AnsiString;
				colvarUserfld3.MaxLength = 50;
				colvarUserfld3.AutoIncrement = false;
				colvarUserfld3.IsNullable = true;
				colvarUserfld3.IsPrimaryKey = false;
				colvarUserfld3.IsForeignKey = false;
				colvarUserfld3.IsReadOnly = false;
				colvarUserfld3.DefaultSetting = @"";
				colvarUserfld3.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfld3);
				
				TableSchema.TableColumn colvarUserfld4 = new TableSchema.TableColumn(schema);
				colvarUserfld4.ColumnName = "userfld4";
				colvarUserfld4.DataType = DbType.AnsiString;
				colvarUserfld4.MaxLength = 50;
				colvarUserfld4.AutoIncrement = false;
				colvarUserfld4.IsNullable = true;
				colvarUserfld4.IsPrimaryKey = false;
				colvarUserfld4.IsForeignKey = false;
				colvarUserfld4.IsReadOnly = false;
				colvarUserfld4.DefaultSetting = @"";
				colvarUserfld4.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfld4);
				
				TableSchema.TableColumn colvarUserfld5 = new TableSchema.TableColumn(schema);
				colvarUserfld5.ColumnName = "userfld5";
				colvarUserfld5.DataType = DbType.AnsiString;
				colvarUserfld5.MaxLength = 50;
				colvarUserfld5.AutoIncrement = false;
				colvarUserfld5.IsNullable = true;
				colvarUserfld5.IsPrimaryKey = false;
				colvarUserfld5.IsForeignKey = false;
				colvarUserfld5.IsReadOnly = false;
				colvarUserfld5.DefaultSetting = @"";
				colvarUserfld5.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfld5);
				
				TableSchema.TableColumn colvarUserfld6 = new TableSchema.TableColumn(schema);
				colvarUserfld6.ColumnName = "userfld6";
				colvarUserfld6.DataType = DbType.AnsiString;
				colvarUserfld6.MaxLength = 50;
				colvarUserfld6.AutoIncrement = false;
				colvarUserfld6.IsNullable = true;
				colvarUserfld6.IsPrimaryKey = false;
				colvarUserfld6.IsForeignKey = false;
				colvarUserfld6.IsReadOnly = false;
				colvarUserfld6.DefaultSetting = @"";
				colvarUserfld6.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfld6);
				
				TableSchema.TableColumn colvarUserfld7 = new TableSchema.TableColumn(schema);
				colvarUserfld7.ColumnName = "userfld7";
				colvarUserfld7.DataType = DbType.AnsiString;
				colvarUserfld7.MaxLength = 50;
				colvarUserfld7.AutoIncrement = false;
				colvarUserfld7.IsNullable = true;
				colvarUserfld7.IsPrimaryKey = false;
				colvarUserfld7.IsForeignKey = false;
				colvarUserfld7.IsReadOnly = false;
				colvarUserfld7.DefaultSetting = @"";
				colvarUserfld7.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfld7);
				
				TableSchema.TableColumn colvarUserfld8 = new TableSchema.TableColumn(schema);
				colvarUserfld8.ColumnName = "userfld8";
				colvarUserfld8.DataType = DbType.AnsiString;
				colvarUserfld8.MaxLength = 50;
				colvarUserfld8.AutoIncrement = false;
				colvarUserfld8.IsNullable = true;
				colvarUserfld8.IsPrimaryKey = false;
				colvarUserfld8.IsForeignKey = false;
				colvarUserfld8.IsReadOnly = false;
				colvarUserfld8.DefaultSetting = @"";
				colvarUserfld8.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfld8);
				
				TableSchema.TableColumn colvarUserfld9 = new TableSchema.TableColumn(schema);
				colvarUserfld9.ColumnName = "userfld9";
				colvarUserfld9.DataType = DbType.AnsiString;
				colvarUserfld9.MaxLength = 50;
				colvarUserfld9.AutoIncrement = false;
				colvarUserfld9.IsNullable = true;
				colvarUserfld9.IsPrimaryKey = false;
				colvarUserfld9.IsForeignKey = false;
				colvarUserfld9.IsReadOnly = false;
				colvarUserfld9.DefaultSetting = @"";
				colvarUserfld9.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfld9);
				
				TableSchema.TableColumn colvarUserfld10 = new TableSchema.TableColumn(schema);
				colvarUserfld10.ColumnName = "userfld10";
				colvarUserfld10.DataType = DbType.AnsiString;
				colvarUserfld10.MaxLength = 50;
				colvarUserfld10.AutoIncrement = false;
				colvarUserfld10.IsNullable = true;
				colvarUserfld10.IsPrimaryKey = false;
				colvarUserfld10.IsForeignKey = false;
				colvarUserfld10.IsReadOnly = false;
				colvarUserfld10.DefaultSetting = @"";
				colvarUserfld10.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfld10);
				
				TableSchema.TableColumn colvarUserflag1 = new TableSchema.TableColumn(schema);
				colvarUserflag1.ColumnName = "userflag1";
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
				colvarUserflag2.ColumnName = "userflag2";
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
				colvarUserflag3.ColumnName = "userflag3";
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
				colvarUserflag4.ColumnName = "userflag4";
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
				colvarUserflag5.ColumnName = "userflag5";
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
				colvarUserfloat1.ColumnName = "userfloat1";
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
				colvarUserfloat2.ColumnName = "userfloat2";
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
				colvarUserfloat3.ColumnName = "userfloat3";
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
				colvarUserfloat4.ColumnName = "userfloat4";
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
				colvarUserfloat5.ColumnName = "userfloat5";
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
				colvarUserint1.ColumnName = "userint1";
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
				colvarUserint2.ColumnName = "userint2";
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
				colvarUserint3.ColumnName = "userint3";
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
				colvarUserint4.ColumnName = "userint4";
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
				colvarUserint5.ColumnName = "userint5";
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
				
				TableSchema.TableColumn colvarIsDelivery = new TableSchema.TableColumn(schema);
				colvarIsDelivery.ColumnName = "IsDelivery";
				colvarIsDelivery.DataType = DbType.Boolean;
				colvarIsDelivery.MaxLength = 0;
				colvarIsDelivery.AutoIncrement = false;
				colvarIsDelivery.IsNullable = true;
				colvarIsDelivery.IsPrimaryKey = false;
				colvarIsDelivery.IsForeignKey = false;
				colvarIsDelivery.IsReadOnly = false;
				colvarIsDelivery.DefaultSetting = @"";
				colvarIsDelivery.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIsDelivery);
				
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
				
				TableSchema.TableColumn colvarIsVitaMix = new TableSchema.TableColumn(schema);
				colvarIsVitaMix.ColumnName = "IsVitaMix";
				colvarIsVitaMix.DataType = DbType.Boolean;
				colvarIsVitaMix.MaxLength = 0;
				colvarIsVitaMix.AutoIncrement = false;
				colvarIsVitaMix.IsNullable = true;
				colvarIsVitaMix.IsPrimaryKey = false;
				colvarIsVitaMix.IsForeignKey = false;
				colvarIsVitaMix.IsReadOnly = false;
				colvarIsVitaMix.DefaultSetting = @"";
				colvarIsVitaMix.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIsVitaMix);
				
				TableSchema.TableColumn colvarIsWaterFilter = new TableSchema.TableColumn(schema);
				colvarIsWaterFilter.ColumnName = "IsWaterFilter";
				colvarIsWaterFilter.DataType = DbType.Boolean;
				colvarIsWaterFilter.MaxLength = 0;
				colvarIsWaterFilter.AutoIncrement = false;
				colvarIsWaterFilter.IsNullable = true;
				colvarIsWaterFilter.IsPrimaryKey = false;
				colvarIsWaterFilter.IsForeignKey = false;
				colvarIsWaterFilter.IsReadOnly = false;
				colvarIsWaterFilter.DefaultSetting = @"";
				colvarIsWaterFilter.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIsWaterFilter);
				
				TableSchema.TableColumn colvarIsYoung = new TableSchema.TableColumn(schema);
				colvarIsYoung.ColumnName = "IsYoung";
				colvarIsYoung.DataType = DbType.Boolean;
				colvarIsYoung.MaxLength = 0;
				colvarIsYoung.AutoIncrement = false;
				colvarIsYoung.IsNullable = true;
				colvarIsYoung.IsPrimaryKey = false;
				colvarIsYoung.IsForeignKey = false;
				colvarIsYoung.IsReadOnly = false;
				colvarIsYoung.DefaultSetting = @"";
				colvarIsYoung.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIsYoung);
				
				TableSchema.TableColumn colvarIsJuicePlus = new TableSchema.TableColumn(schema);
				colvarIsJuicePlus.ColumnName = "IsJuicePlus";
				colvarIsJuicePlus.DataType = DbType.Boolean;
				colvarIsJuicePlus.MaxLength = 0;
				colvarIsJuicePlus.AutoIncrement = false;
				colvarIsJuicePlus.IsNullable = true;
				colvarIsJuicePlus.IsPrimaryKey = false;
				colvarIsJuicePlus.IsForeignKey = false;
				colvarIsJuicePlus.IsReadOnly = false;
				colvarIsJuicePlus.DefaultSetting = @"";
				colvarIsJuicePlus.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIsJuicePlus);
				
				TableSchema.TableColumn colvarIsCommission = new TableSchema.TableColumn(schema);
				colvarIsCommission.ColumnName = "IsCommission";
				colvarIsCommission.DataType = DbType.Boolean;
				colvarIsCommission.MaxLength = 0;
				colvarIsCommission.AutoIncrement = false;
				colvarIsCommission.IsNullable = true;
				colvarIsCommission.IsPrimaryKey = false;
				colvarIsCommission.IsForeignKey = false;
				colvarIsCommission.IsReadOnly = false;
				colvarIsCommission.DefaultSetting = @"";
				colvarIsCommission.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIsCommission);
				
				TableSchema.TableColumn colvarItemImage = new TableSchema.TableColumn(schema);
				colvarItemImage.ColumnName = "ItemImage";
				colvarItemImage.DataType = DbType.Binary;
				colvarItemImage.MaxLength = -1;
				colvarItemImage.AutoIncrement = false;
				colvarItemImage.IsNullable = true;
				colvarItemImage.IsPrimaryKey = false;
				colvarItemImage.IsForeignKey = false;
				colvarItemImage.IsReadOnly = false;
				colvarItemImage.DefaultSetting = @"";
				colvarItemImage.ForeignKeyTableName = "";
				schema.Columns.Add(colvarItemImage);
				
				TableSchema.TableColumn colvarUserfloat6 = new TableSchema.TableColumn(schema);
				colvarUserfloat6.ColumnName = "Userfloat6";
				colvarUserfloat6.DataType = DbType.Currency;
				colvarUserfloat6.MaxLength = 0;
				colvarUserfloat6.AutoIncrement = false;
				colvarUserfloat6.IsNullable = true;
				colvarUserfloat6.IsPrimaryKey = false;
				colvarUserfloat6.IsForeignKey = false;
				colvarUserfloat6.IsReadOnly = false;
				colvarUserfloat6.DefaultSetting = @"";
				colvarUserfloat6.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfloat6);
				
				TableSchema.TableColumn colvarUserfloat7 = new TableSchema.TableColumn(schema);
				colvarUserfloat7.ColumnName = "Userfloat7";
				colvarUserfloat7.DataType = DbType.Currency;
				colvarUserfloat7.MaxLength = 0;
				colvarUserfloat7.AutoIncrement = false;
				colvarUserfloat7.IsNullable = true;
				colvarUserfloat7.IsPrimaryKey = false;
				colvarUserfloat7.IsForeignKey = false;
				colvarUserfloat7.IsReadOnly = false;
				colvarUserfloat7.DefaultSetting = @"";
				colvarUserfloat7.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfloat7);
				
				TableSchema.TableColumn colvarUserfloat8 = new TableSchema.TableColumn(schema);
				colvarUserfloat8.ColumnName = "Userfloat8";
				colvarUserfloat8.DataType = DbType.Currency;
				colvarUserfloat8.MaxLength = 0;
				colvarUserfloat8.AutoIncrement = false;
				colvarUserfloat8.IsNullable = true;
				colvarUserfloat8.IsPrimaryKey = false;
				colvarUserfloat8.IsForeignKey = false;
				colvarUserfloat8.IsReadOnly = false;
				colvarUserfloat8.DefaultSetting = @"";
				colvarUserfloat8.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfloat8);
				
				TableSchema.TableColumn colvarUserfloat9 = new TableSchema.TableColumn(schema);
				colvarUserfloat9.ColumnName = "Userfloat9";
				colvarUserfloat9.DataType = DbType.Currency;
				colvarUserfloat9.MaxLength = 0;
				colvarUserfloat9.AutoIncrement = false;
				colvarUserfloat9.IsNullable = true;
				colvarUserfloat9.IsPrimaryKey = false;
				colvarUserfloat9.IsForeignKey = false;
				colvarUserfloat9.IsReadOnly = false;
				colvarUserfloat9.DefaultSetting = @"";
				colvarUserfloat9.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfloat9);
				
				TableSchema.TableColumn colvarUserfloat10 = new TableSchema.TableColumn(schema);
				colvarUserfloat10.ColumnName = "Userfloat10";
				colvarUserfloat10.DataType = DbType.Currency;
				colvarUserfloat10.MaxLength = 0;
				colvarUserfloat10.AutoIncrement = false;
				colvarUserfloat10.IsNullable = true;
				colvarUserfloat10.IsPrimaryKey = false;
				colvarUserfloat10.IsForeignKey = false;
				colvarUserfloat10.IsReadOnly = false;
				colvarUserfloat10.DefaultSetting = @"";
				colvarUserfloat10.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfloat10);
				
				TableSchema.TableColumn colvarAvgCostPrice = new TableSchema.TableColumn(schema);
				colvarAvgCostPrice.ColumnName = "AvgCostPrice";
				colvarAvgCostPrice.DataType = DbType.Currency;
				colvarAvgCostPrice.MaxLength = 0;
				colvarAvgCostPrice.AutoIncrement = false;
				colvarAvgCostPrice.IsNullable = true;
				colvarAvgCostPrice.IsPrimaryKey = false;
				colvarAvgCostPrice.IsForeignKey = false;
				colvarAvgCostPrice.IsReadOnly = false;
				colvarAvgCostPrice.DefaultSetting = @"";
				colvarAvgCostPrice.ForeignKeyTableName = "";
				schema.Columns.Add(colvarAvgCostPrice);
				
				TableSchema.TableColumn colvarBalanceQuantity = new TableSchema.TableColumn(schema);
				colvarBalanceQuantity.ColumnName = "BalanceQuantity";
				colvarBalanceQuantity.DataType = DbType.Double;
				colvarBalanceQuantity.MaxLength = 0;
				colvarBalanceQuantity.AutoIncrement = false;
				colvarBalanceQuantity.IsNullable = true;
				colvarBalanceQuantity.IsPrimaryKey = false;
				colvarBalanceQuantity.IsForeignKey = false;
				colvarBalanceQuantity.IsReadOnly = false;
				colvarBalanceQuantity.DefaultSetting = @"";
				colvarBalanceQuantity.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBalanceQuantity);

                TableSchema.TableColumn colvarUserflag6 = new TableSchema.TableColumn(schema);
                colvarUserflag6.ColumnName = "userflag6";
                colvarUserflag6.DataType = DbType.Boolean;
                colvarUserflag6.MaxLength = 0;
                colvarUserflag6.AutoIncrement = false;
                colvarUserflag6.IsNullable = true;
                colvarUserflag6.IsPrimaryKey = false;
                colvarUserflag6.IsForeignKey = false;
                colvarUserflag6.IsReadOnly = false;
                colvarUserflag6.DefaultSetting = @"";
                colvarUserflag6.ForeignKeyTableName = "";
                schema.Columns.Add(colvarUserflag6);

                TableSchema.TableColumn colvarUserflag7 = new TableSchema.TableColumn(schema);
                colvarUserflag7.ColumnName = "userflag7";
                colvarUserflag7.DataType = DbType.Boolean;
                colvarUserflag7.MaxLength = 0;
                colvarUserflag7.AutoIncrement = false;
                colvarUserflag7.IsNullable = true;
                colvarUserflag7.IsPrimaryKey = false;
                colvarUserflag7.IsForeignKey = false;
                colvarUserflag7.IsReadOnly = false;
                colvarUserflag7.DefaultSetting = @"";
                colvarUserflag7.ForeignKeyTableName = "";
                schema.Columns.Add(colvarUserflag7);

                TableSchema.TableColumn colvarUserflag8 = new TableSchema.TableColumn(schema);
                colvarUserflag8.ColumnName = "userflag8";
                colvarUserflag8.DataType = DbType.Boolean;
                colvarUserflag8.MaxLength = 0;
                colvarUserflag8.AutoIncrement = false;
                colvarUserflag8.IsNullable = true;
                colvarUserflag8.IsPrimaryKey = false;
                colvarUserflag8.IsForeignKey = false;
                colvarUserflag8.IsReadOnly = false;
                colvarUserflag8.DefaultSetting = @"";
                colvarUserflag8.ForeignKeyTableName = "";
                schema.Columns.Add(colvarUserflag8);

                TableSchema.TableColumn colvarUserflag9 = new TableSchema.TableColumn(schema);
                colvarUserflag9.ColumnName = "userflag9";
                colvarUserflag9.DataType = DbType.Boolean;
                colvarUserflag9.MaxLength = 0;
                colvarUserflag9.AutoIncrement = false;
                colvarUserflag9.IsNullable = true;
                colvarUserflag9.IsPrimaryKey = false;
                colvarUserflag9.IsForeignKey = false;
                colvarUserflag9.IsReadOnly = false;
                colvarUserflag9.DefaultSetting = @"";
                colvarUserflag9.ForeignKeyTableName = "";
                schema.Columns.Add(colvarUserflag9);

                TableSchema.TableColumn colvarUserflag10 = new TableSchema.TableColumn(schema);
                colvarUserflag10.ColumnName = "userflag10";
                colvarUserflag10.DataType = DbType.Boolean;
                colvarUserflag10.MaxLength = 0;
                colvarUserflag10.AutoIncrement = false;
                colvarUserflag10.IsNullable = true;
                colvarUserflag10.IsPrimaryKey = false;
                colvarUserflag10.IsForeignKey = false;
                colvarUserflag10.IsReadOnly = false;
                colvarUserflag10.DefaultSetting = @"";
                colvarUserflag10.ForeignKeyTableName = "";
                schema.Columns.Add(colvarUserflag10);

				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["PowerPOS"].AddSchema("Item",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("ItemNo")]
		[Bindable(true)]
		public string ItemNo 
		{
			get { return GetColumnValue<string>(Columns.ItemNo); }
			set { SetColumnValue(Columns.ItemNo, value); }
		}
		  
		[XmlAttribute("ItemName")]
		[Bindable(true)]
		public string ItemName 
		{
			get { return GetColumnValue<string>(Columns.ItemName); }
			set { SetColumnValue(Columns.ItemName, value); }
		}
		  
		[XmlAttribute("Barcode")]
		[Bindable(true)]
		public string Barcode 
		{
			get { return GetColumnValue<string>(Columns.Barcode); }
			set { SetColumnValue(Columns.Barcode, value); }
		}
		  
		[XmlAttribute("CategoryName")]
		[Bindable(true)]
		public string CategoryName 
		{
			get { return GetColumnValue<string>(Columns.CategoryName); }
			set { SetColumnValue(Columns.CategoryName, value); }
		}
		  
		[XmlAttribute("RetailPrice")]
		[Bindable(true)]
		public decimal RetailPrice 
		{
			get { return GetColumnValue<decimal>(Columns.RetailPrice); }
			set { SetColumnValue(Columns.RetailPrice, value); }
		}
		  
		[XmlAttribute("FactoryPrice")]
		[Bindable(true)]
		public decimal FactoryPrice 
		{
			get { return GetColumnValue<decimal>(Columns.FactoryPrice); }
			set { SetColumnValue(Columns.FactoryPrice, value); }
		}
		  
		[XmlAttribute("MinimumPrice")]
		[Bindable(true)]
		public decimal MinimumPrice 
		{
			get { return GetColumnValue<decimal>(Columns.MinimumPrice); }
			set { SetColumnValue(Columns.MinimumPrice, value); }
		}
		  
		[XmlAttribute("ItemDesc")]
		[Bindable(true)]
		public string ItemDesc 
		{
			get { return GetColumnValue<string>(Columns.ItemDesc); }
			set { SetColumnValue(Columns.ItemDesc, value); }
		}
		  
		[XmlAttribute("IsServiceItem")]
		[Bindable(true)]
		public bool? IsServiceItem 
		{
			get { return GetColumnValue<bool?>(Columns.IsServiceItem); }
			set { SetColumnValue(Columns.IsServiceItem, value); }
		}
		  
		[XmlAttribute("IsInInventory")]
		[Bindable(true)]
		public bool IsInInventory 
		{
			get { return GetColumnValue<bool>(Columns.IsInInventory); }
			set { SetColumnValue(Columns.IsInInventory, value); }
		}
		  
		[XmlAttribute("IsNonDiscountable")]
		[Bindable(true)]
		public bool IsNonDiscountable 
		{
			get { return GetColumnValue<bool>(Columns.IsNonDiscountable); }
			set { SetColumnValue(Columns.IsNonDiscountable, value); }
		}
		  
		[XmlAttribute("IsCourse")]
		[Bindable(true)]
		public bool? IsCourse 
		{
			get { return GetColumnValue<bool?>(Columns.IsCourse); }
			set { SetColumnValue(Columns.IsCourse, value); }
		}
		  
		[XmlAttribute("CourseTypeID")]
		[Bindable(true)]
		public string CourseTypeID 
		{
			get { return GetColumnValue<string>(Columns.CourseTypeID); }
			set { SetColumnValue(Columns.CourseTypeID, value); }
		}
		  
		[XmlAttribute("Brand")]
		[Bindable(true)]
		public string Brand 
		{
			get { return GetColumnValue<string>(Columns.Brand); }
			set { SetColumnValue(Columns.Brand, value); }
		}
		  
		[XmlAttribute("ProductLine")]
		[Bindable(true)]
		public string ProductLine 
		{
			get { return GetColumnValue<string>(Columns.ProductLine); }
			set { SetColumnValue(Columns.ProductLine, value); }
		}
		  
		[XmlAttribute("Attributes1")]
		[Bindable(true)]
		public string Attributes1 
		{
			get { return GetColumnValue<string>(Columns.Attributes1); }
			set { SetColumnValue(Columns.Attributes1, value); }
		}
		  
		[XmlAttribute("Attributes2")]
		[Bindable(true)]
		public string Attributes2 
		{
			get { return GetColumnValue<string>(Columns.Attributes2); }
			set { SetColumnValue(Columns.Attributes2, value); }
		}
		  
		[XmlAttribute("Attributes3")]
		[Bindable(true)]
		public string Attributes3 
		{
			get { return GetColumnValue<string>(Columns.Attributes3); }
			set { SetColumnValue(Columns.Attributes3, value); }
		}
		  
		[XmlAttribute("Attributes4")]
		[Bindable(true)]
		public string Attributes4 
		{
			get { return GetColumnValue<string>(Columns.Attributes4); }
			set { SetColumnValue(Columns.Attributes4, value); }
		}
		  
		[XmlAttribute("Attributes5")]
		[Bindable(true)]
		public string Attributes5 
		{
			get { return GetColumnValue<string>(Columns.Attributes5); }
			set { SetColumnValue(Columns.Attributes5, value); }
		}
		  
		[XmlAttribute("Attributes6")]
		[Bindable(true)]
		public string Attributes6 
		{
			get { return GetColumnValue<string>(Columns.Attributes6); }
			set { SetColumnValue(Columns.Attributes6, value); }
		}
		  
		[XmlAttribute("Attributes7")]
		[Bindable(true)]
		public string Attributes7 
		{
			get { return GetColumnValue<string>(Columns.Attributes7); }
			set { SetColumnValue(Columns.Attributes7, value); }
		}
		  
		[XmlAttribute("Attributes8")]
		[Bindable(true)]
		public string Attributes8 
		{
			get { return GetColumnValue<string>(Columns.Attributes8); }
			set { SetColumnValue(Columns.Attributes8, value); }
		}
		  
		[XmlAttribute("Remark")]
		[Bindable(true)]
		public string Remark 
		{
			get { return GetColumnValue<string>(Columns.Remark); }
			set { SetColumnValue(Columns.Remark, value); }
		}
		  
		[XmlAttribute("ProductionDate")]
		[Bindable(true)]
		public DateTime? ProductionDate 
		{
			get { return GetColumnValue<DateTime?>(Columns.ProductionDate); }
			set { SetColumnValue(Columns.ProductionDate, value); }
		}
		  
		[XmlAttribute("IsGST")]
		[Bindable(true)]
		public bool? IsGST 
		{
			get { return GetColumnValue<bool?>(Columns.IsGST); }
			set { SetColumnValue(Columns.IsGST, value); }
		}
		  
		[XmlAttribute("HasWarranty")]
		[Bindable(true)]
		public bool? HasWarranty 
		{
			get { return GetColumnValue<bool?>(Columns.HasWarranty); }
			set { SetColumnValue(Columns.HasWarranty, value); }
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
		  
		[XmlAttribute("IsDelivery")]
		[Bindable(true)]
		public bool? IsDelivery 
		{
			get { return GetColumnValue<bool?>(Columns.IsDelivery); }
			set { SetColumnValue(Columns.IsDelivery, value); }
		}
		  
		[XmlAttribute("GSTRule")]
		[Bindable(true)]
		public int? GSTRule 
		{
			get { return GetColumnValue<int?>(Columns.GSTRule); }
			set { SetColumnValue(Columns.GSTRule, value); }
		}
		  
		[XmlAttribute("IsVitaMix")]
		[Bindable(true)]
		public bool? IsVitaMix 
		{
			get { return GetColumnValue<bool?>(Columns.IsVitaMix); }
			set { SetColumnValue(Columns.IsVitaMix, value); }
		}
		  
		[XmlAttribute("IsWaterFilter")]
		[Bindable(true)]
		public bool? IsWaterFilter 
		{
			get { return GetColumnValue<bool?>(Columns.IsWaterFilter); }
			set { SetColumnValue(Columns.IsWaterFilter, value); }
		}
		  
		[XmlAttribute("IsYoung")]
		[Bindable(true)]
		public bool? IsYoung 
		{
			get { return GetColumnValue<bool?>(Columns.IsYoung); }
			set { SetColumnValue(Columns.IsYoung, value); }
		}
		  
		[XmlAttribute("IsJuicePlus")]
		[Bindable(true)]
		public bool? IsJuicePlus 
		{
			get { return GetColumnValue<bool?>(Columns.IsJuicePlus); }
			set { SetColumnValue(Columns.IsJuicePlus, value); }
		}
		  
		[XmlAttribute("IsCommission")]
		[Bindable(true)]
		public bool? IsCommission 
		{
			get { return GetColumnValue<bool?>(Columns.IsCommission); }
			set { SetColumnValue(Columns.IsCommission, value); }
		}
		  
		[XmlAttribute("ItemImage")]
		[Bindable(true)]
		public byte[] ItemImage 
		{
			get { return GetColumnValue<byte[]>(Columns.ItemImage); }
			set { SetColumnValue(Columns.ItemImage, value); }
		}
		  
		[XmlAttribute("Userfloat6")]
		[Bindable(true)]
		public decimal? Userfloat6 
		{
			get { return GetColumnValue<decimal?>(Columns.Userfloat6); }
			set { SetColumnValue(Columns.Userfloat6, value); }
		}
		  
		[XmlAttribute("Userfloat7")]
		[Bindable(true)]
		public decimal? Userfloat7 
		{
			get { return GetColumnValue<decimal?>(Columns.Userfloat7); }
			set { SetColumnValue(Columns.Userfloat7, value); }
		}
		  
		[XmlAttribute("Userfloat8")]
		[Bindable(true)]
		public decimal? Userfloat8 
		{
			get { return GetColumnValue<decimal?>(Columns.Userfloat8); }
			set { SetColumnValue(Columns.Userfloat8, value); }
		}
		  
		[XmlAttribute("Userfloat9")]
		[Bindable(true)]
		public decimal? Userfloat9 
		{
			get { return GetColumnValue<decimal?>(Columns.Userfloat9); }
			set { SetColumnValue(Columns.Userfloat9, value); }
		}
		  
		[XmlAttribute("Userfloat10")]
		[Bindable(true)]
		public decimal? Userfloat10 
		{
			get { return GetColumnValue<decimal?>(Columns.Userfloat10); }
			set { SetColumnValue(Columns.Userfloat10, value); }
		}
		  
		[XmlAttribute("AvgCostPrice")]
		[Bindable(true)]
		public decimal? AvgCostPrice 
		{
			get { return GetColumnValue<decimal?>(Columns.AvgCostPrice); }
			set { SetColumnValue(Columns.AvgCostPrice, value); }
		}
		  
		[XmlAttribute("BalanceQuantity")]
		[Bindable(true)]
		public double? BalanceQuantity 
		{
			get { return GetColumnValue<double?>(Columns.BalanceQuantity); }
			set { SetColumnValue(Columns.BalanceQuantity, value); }
		}

        [XmlAttribute("Userflag6")]
        [Bindable(true)]
        public bool? Userflag6
        {
            get { return GetColumnValue<bool?>(Columns.Userflag6); }
            set { SetColumnValue(Columns.Userflag6, value); }
        }

        [XmlAttribute("Userflag7")]
        [Bindable(true)]
        public bool? Userflag7
        {
            get { return GetColumnValue<bool?>(Columns.Userflag7); }
            set { SetColumnValue(Columns.Userflag7, value); }
        }

        [XmlAttribute("Userflag8")]
        [Bindable(true)]
        public bool? Userflag8
        {
            get { return GetColumnValue<bool?>(Columns.Userflag8); }
            set { SetColumnValue(Columns.Userflag8, value); }
        }

        [XmlAttribute("Userflag9")]
        [Bindable(true)]
        public bool? Userflag9
        {
            get { return GetColumnValue<bool?>(Columns.Userflag9); }
            set { SetColumnValue(Columns.Userflag9, value); }
        }

        [XmlAttribute("Userflag10")]
        [Bindable(true)]
        public bool? Userflag10
        {
            get { return GetColumnValue<bool?>(Columns.Userflag10); }
            set { SetColumnValue(Columns.Userflag10, value); }
        }
		
		#endregion
		
		
		#region PrimaryKey Methods		
		
        protected override void SetPrimaryKey(object oValue)
        {
            base.SetPrimaryKey(oValue);
            
            SetPKValues();
        }
        
		
		public PowerPOS.AlternateBarcodeCollection AlternateBarcodeRecords()
		{
			return new PowerPOS.AlternateBarcodeCollection().Where(AlternateBarcode.Columns.ItemNo, ItemNo).Load();
		}
		public PowerPOS.EventItemMapCollection EventItemMapRecords()
		{
			return new PowerPOS.EventItemMapCollection().Where(EventItemMap.Columns.ItemNo, ItemNo).Load();
		}
		public PowerPOS.InventoryDetCollection InventoryDetRecords()
		{
			return new PowerPOS.InventoryDetCollection().Where(InventoryDet.Columns.ItemNo, ItemNo).Load();
		}
		public PowerPOS.ItemSupplierMapCollection ItemSupplierMapRecords()
		{
			return new PowerPOS.ItemSupplierMapCollection().Where(ItemSupplierMap.Columns.ItemNo, ItemNo).Load();
		}
		public PowerPOS.ItemCostPriceCollection ItemCostPriceRecords()
		{
			return new PowerPOS.ItemCostPriceCollection().Where(ItemCostPrice.Columns.ItemNo, ItemNo).Load();
		}
		public PowerPOS.ItemGroupMapCollection ItemGroupMapRecords()
		{
			return new PowerPOS.ItemGroupMapCollection().Where(ItemGroupMap.Columns.ItemNo, ItemNo).Load();
		}
		public PowerPOS.ItemQuantityTriggerCollection ItemQuantityTriggerRecords()
		{
			return new PowerPOS.ItemQuantityTriggerCollection().Where(ItemQuantityTrigger.Columns.ItemNo, ItemNo).Load();
		}
		public PowerPOS.ItemSummaryCollection ItemSummaryRecords()
		{
			return new PowerPOS.ItemSummaryCollection().Where(ItemSummary.Columns.ItemNo, ItemNo).Load();
		}
		public PowerPOS.OrderDetCollection OrderDetRecords()
		{
			return new PowerPOS.OrderDetCollection().Where(OrderDet.Columns.ItemNo, ItemNo).Load();
		}
		public PowerPOS.PromoCampaignDetCollection PromoCampaignDetRecords()
		{
			return new PowerPOS.PromoCampaignDetCollection().Where(PromoCampaignDet.Columns.ItemNo, ItemNo).Load();
		}
		public PowerPOS.PromoCampaignHdrCollection PromoCampaignHdrRecords()
		{
			return new PowerPOS.PromoCampaignHdrCollection().Where(PromoCampaignHdr.Columns.FreeItemNo, ItemNo).Load();
		}
		public PowerPOS.PurchaseOrderDetCollection PurchaseOrderDetRecords()
		{
			return new PowerPOS.PurchaseOrderDetCollection().Where(PurchaseOrderDet.Columns.ItemNo, ItemNo).Load();
		}
		public PowerPOS.PurchaseOrderDetailCollection PurchaseOrderDetailRecords()
		{
			return new PowerPOS.PurchaseOrderDetailCollection().Where(PurchaseOrderDetail.Columns.ItemNo, ItemNo).Load();
		}
		public PowerPOS.QuickAccessButtonCollection QuickAccessButtonRecords()
		{
			return new PowerPOS.QuickAccessButtonCollection().Where(QuickAccessButton.Columns.ItemNo, ItemNo).Load();
		}
		public PowerPOS.RedemptionItemCollection RedemptionItemRecords()
		{
			return new PowerPOS.RedemptionItemCollection().Where(RedemptionItem.Columns.ItemNo, ItemNo).Load();
		}
		public PowerPOS.WarrantyCollection WarrantyRecords()
		{
			return new PowerPOS.WarrantyCollection().Where(Warranty.Columns.ItemNo, ItemNo).Load();
		}
		#endregion
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a Category ActiveRecord object related to this Item
		/// 
		/// </summary>
		public PowerPOS.Category Category
		{
			get { return PowerPOS.Category.FetchByID(this.CategoryName); }
			set { SetColumnValue("CategoryName", value.CategoryName); }
		}
		
		
		/// <summary>
		/// Returns a CourseType ActiveRecord object related to this Item
		/// 
		/// </summary>
		public PowerPOS.CourseType CourseType
		{
			get { return PowerPOS.CourseType.FetchByID(this.CourseTypeID); }
			set { SetColumnValue("CourseTypeID", value.CourseTypeID); }
		}
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(string varItemNo,string varItemName,string varBarcode,string varCategoryName,decimal varRetailPrice,decimal varFactoryPrice,decimal varMinimumPrice,string varItemDesc,bool? varIsServiceItem,bool varIsInInventory,bool varIsNonDiscountable,bool? varIsCourse,string varCourseTypeID,string varBrand,string varProductLine,string varAttributes1,string varAttributes2,string varAttributes3,string varAttributes4,string varAttributes5,string varAttributes6,string varAttributes7,string varAttributes8,string varRemark,DateTime? varProductionDate,bool? varIsGST,bool? varHasWarranty,DateTime? varCreatedOn,string varCreatedBy,DateTime? varModifiedOn,string varModifiedBy,Guid varUniqueID,bool? varDeleted,string varUserfld1,string varUserfld2,string varUserfld3,string varUserfld4,string varUserfld5,string varUserfld6,string varUserfld7,string varUserfld8,string varUserfld9,string varUserfld10,bool? varUserflag1,bool? varUserflag2,bool? varUserflag3,bool? varUserflag4,bool? varUserflag5,decimal? varUserfloat1,decimal? varUserfloat2,decimal? varUserfloat3,decimal? varUserfloat4,decimal? varUserfloat5,int? varUserint1,int? varUserint2,int? varUserint3,int? varUserint4,int? varUserint5,bool? varIsDelivery,int? varGSTRule,bool? varIsVitaMix,bool? varIsWaterFilter,bool? varIsYoung,bool? varIsJuicePlus,bool? varIsCommission,byte[] varItemImage,decimal? varUserfloat6,decimal? varUserfloat7,decimal? varUserfloat8,decimal? varUserfloat9,decimal? varUserfloat10,decimal? varAvgCostPrice,double? varBalanceQuantity,bool? varUserflag6,bool? varUserflag7,bool? varUserflag8,bool? varUserflag9,bool? varUserflag10)
		{
			Item item = new Item();
			
			item.ItemNo = varItemNo;
			
			item.ItemName = varItemName;
			
			item.Barcode = varBarcode;
			
			item.CategoryName = varCategoryName;
			
			item.RetailPrice = varRetailPrice;
			
			item.FactoryPrice = varFactoryPrice;
			
			item.MinimumPrice = varMinimumPrice;
			
			item.ItemDesc = varItemDesc;
			
			item.IsServiceItem = varIsServiceItem;
			
			item.IsInInventory = varIsInInventory;
			
			item.IsNonDiscountable = varIsNonDiscountable;
			
			item.IsCourse = varIsCourse;
			
			item.CourseTypeID = varCourseTypeID;
			
			item.Brand = varBrand;
			
			item.ProductLine = varProductLine;
			
			item.Attributes1 = varAttributes1;
			
			item.Attributes2 = varAttributes2;
			
			item.Attributes3 = varAttributes3;
			
			item.Attributes4 = varAttributes4;
			
			item.Attributes5 = varAttributes5;
			
			item.Attributes6 = varAttributes6;
			
			item.Attributes7 = varAttributes7;
			
			item.Attributes8 = varAttributes8;
			
			item.Remark = varRemark;
			
			item.ProductionDate = varProductionDate;
			
			item.IsGST = varIsGST;
			
			item.HasWarranty = varHasWarranty;
			
			item.CreatedOn = varCreatedOn;
			
			item.CreatedBy = varCreatedBy;
			
			item.ModifiedOn = varModifiedOn;
			
			item.ModifiedBy = varModifiedBy;
			
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
			
			item.IsDelivery = varIsDelivery;
			
			item.GSTRule = varGSTRule;
			
			item.IsVitaMix = varIsVitaMix;
			
			item.IsWaterFilter = varIsWaterFilter;
			
			item.IsYoung = varIsYoung;
			
			item.IsJuicePlus = varIsJuicePlus;
			
			item.IsCommission = varIsCommission;
			
			item.ItemImage = varItemImage;
			
			item.Userfloat6 = varUserfloat6;
			
			item.Userfloat7 = varUserfloat7;
			
			item.Userfloat8 = varUserfloat8;
			
			item.Userfloat9 = varUserfloat9;
			
			item.Userfloat10 = varUserfloat10;
			
			item.AvgCostPrice = varAvgCostPrice;
			
			item.BalanceQuantity = varBalanceQuantity;

            item.Userflag6 = varUserflag6;

            item.Userflag7 = varUserflag7;

            item.Userflag8 = varUserflag8;

            item.Userflag9 = varUserflag9;

            item.Userflag10 = varUserflag10;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(string varItemNo,string varItemName,string varBarcode,string varCategoryName,decimal varRetailPrice,decimal varFactoryPrice,decimal varMinimumPrice,string varItemDesc,bool? varIsServiceItem,bool varIsInInventory,bool varIsNonDiscountable,bool? varIsCourse,string varCourseTypeID,string varBrand,string varProductLine,string varAttributes1,string varAttributes2,string varAttributes3,string varAttributes4,string varAttributes5,string varAttributes6,string varAttributes7,string varAttributes8,string varRemark,DateTime? varProductionDate,bool? varIsGST,bool? varHasWarranty,DateTime? varCreatedOn,string varCreatedBy,DateTime? varModifiedOn,string varModifiedBy,Guid varUniqueID,bool? varDeleted,string varUserfld1,string varUserfld2,string varUserfld3,string varUserfld4,string varUserfld5,string varUserfld6,string varUserfld7,string varUserfld8,string varUserfld9,string varUserfld10,bool? varUserflag1,bool? varUserflag2,bool? varUserflag3,bool? varUserflag4,bool? varUserflag5,decimal? varUserfloat1,decimal? varUserfloat2,decimal? varUserfloat3,decimal? varUserfloat4,decimal? varUserfloat5,int? varUserint1,int? varUserint2,int? varUserint3,int? varUserint4,int? varUserint5,bool? varIsDelivery,int? varGSTRule,bool? varIsVitaMix,bool? varIsWaterFilter,bool? varIsYoung,bool? varIsJuicePlus,bool? varIsCommission,byte[] varItemImage,decimal? varUserfloat6,decimal? varUserfloat7,decimal? varUserfloat8,decimal? varUserfloat9,decimal? varUserfloat10,decimal? varAvgCostPrice,double? varBalanceQuantity,bool? varUserflag6,bool? varUserflag7,bool? varUserflag8,bool? varUserflag9,bool? varUserflag10)
		{
			Item item = new Item();
			
				item.ItemNo = varItemNo;
			
				item.ItemName = varItemName;
			
				item.Barcode = varBarcode;
			
				item.CategoryName = varCategoryName;
			
				item.RetailPrice = varRetailPrice;
			
				item.FactoryPrice = varFactoryPrice;
			
				item.MinimumPrice = varMinimumPrice;
			
				item.ItemDesc = varItemDesc;
			
				item.IsServiceItem = varIsServiceItem;
			
				item.IsInInventory = varIsInInventory;
			
				item.IsNonDiscountable = varIsNonDiscountable;
			
				item.IsCourse = varIsCourse;
			
				item.CourseTypeID = varCourseTypeID;
			
				item.Brand = varBrand;
			
				item.ProductLine = varProductLine;
			
				item.Attributes1 = varAttributes1;
			
				item.Attributes2 = varAttributes2;
			
				item.Attributes3 = varAttributes3;
			
				item.Attributes4 = varAttributes4;
			
				item.Attributes5 = varAttributes5;
			
				item.Attributes6 = varAttributes6;
			
				item.Attributes7 = varAttributes7;
			
				item.Attributes8 = varAttributes8;
			
				item.Remark = varRemark;
			
				item.ProductionDate = varProductionDate;
			
				item.IsGST = varIsGST;
			
				item.HasWarranty = varHasWarranty;
			
				item.CreatedOn = varCreatedOn;
			
				item.CreatedBy = varCreatedBy;
			
				item.ModifiedOn = varModifiedOn;
			
				item.ModifiedBy = varModifiedBy;
			
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
			
				item.IsDelivery = varIsDelivery;
			
				item.GSTRule = varGSTRule;
			
				item.IsVitaMix = varIsVitaMix;
			
				item.IsWaterFilter = varIsWaterFilter;
			
				item.IsYoung = varIsYoung;
			
				item.IsJuicePlus = varIsJuicePlus;
			
				item.IsCommission = varIsCommission;
			
				item.ItemImage = varItemImage;
			
				item.Userfloat6 = varUserfloat6;
			
				item.Userfloat7 = varUserfloat7;
			
				item.Userfloat8 = varUserfloat8;
			
				item.Userfloat9 = varUserfloat9;
			
				item.Userfloat10 = varUserfloat10;
			
				item.AvgCostPrice = varAvgCostPrice;
			
				item.BalanceQuantity = varBalanceQuantity;

                item.Userflag6 = varUserflag6;

                item.Userflag7 = varUserflag7;

                item.Userflag8 = varUserflag8;

                item.Userflag9 = varUserflag9;

                item.Userflag10 = varUserflag10;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn ItemNoColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn ItemNameColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn BarcodeColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn CategoryNameColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn RetailPriceColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn FactoryPriceColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn MinimumPriceColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn ItemDescColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn IsServiceItemColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn IsInInventoryColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn IsNonDiscountableColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        public static TableSchema.TableColumn IsCourseColumn
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        public static TableSchema.TableColumn CourseTypeIDColumn
        {
            get { return Schema.Columns[12]; }
        }
        
        
        
        public static TableSchema.TableColumn BrandColumn
        {
            get { return Schema.Columns[13]; }
        }
        
        
        
        public static TableSchema.TableColumn ProductLineColumn
        {
            get { return Schema.Columns[14]; }
        }
        
        
        
        public static TableSchema.TableColumn Attributes1Column
        {
            get { return Schema.Columns[15]; }
        }
        
        
        
        public static TableSchema.TableColumn Attributes2Column
        {
            get { return Schema.Columns[16]; }
        }
        
        
        
        public static TableSchema.TableColumn Attributes3Column
        {
            get { return Schema.Columns[17]; }
        }
        
        
        
        public static TableSchema.TableColumn Attributes4Column
        {
            get { return Schema.Columns[18]; }
        }
        
        
        
        public static TableSchema.TableColumn Attributes5Column
        {
            get { return Schema.Columns[19]; }
        }
        
        
        
        public static TableSchema.TableColumn Attributes6Column
        {
            get { return Schema.Columns[20]; }
        }
        
        
        
        public static TableSchema.TableColumn Attributes7Column
        {
            get { return Schema.Columns[21]; }
        }
        
        
        
        public static TableSchema.TableColumn Attributes8Column
        {
            get { return Schema.Columns[22]; }
        }
        
        
        
        public static TableSchema.TableColumn RemarkColumn
        {
            get { return Schema.Columns[23]; }
        }
        
        
        
        public static TableSchema.TableColumn ProductionDateColumn
        {
            get { return Schema.Columns[24]; }
        }
        
        
        
        public static TableSchema.TableColumn IsGSTColumn
        {
            get { return Schema.Columns[25]; }
        }
        
        
        
        public static TableSchema.TableColumn HasWarrantyColumn
        {
            get { return Schema.Columns[26]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedOnColumn
        {
            get { return Schema.Columns[27]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedByColumn
        {
            get { return Schema.Columns[28]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedOnColumn
        {
            get { return Schema.Columns[29]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedByColumn
        {
            get { return Schema.Columns[30]; }
        }
        
        
        
        public static TableSchema.TableColumn UniqueIDColumn
        {
            get { return Schema.Columns[31]; }
        }
        
        
        
        public static TableSchema.TableColumn DeletedColumn
        {
            get { return Schema.Columns[32]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld1Column
        {
            get { return Schema.Columns[33]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld2Column
        {
            get { return Schema.Columns[34]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld3Column
        {
            get { return Schema.Columns[35]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld4Column
        {
            get { return Schema.Columns[36]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld5Column
        {
            get { return Schema.Columns[37]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld6Column
        {
            get { return Schema.Columns[38]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld7Column
        {
            get { return Schema.Columns[39]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld8Column
        {
            get { return Schema.Columns[40]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld9Column
        {
            get { return Schema.Columns[41]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld10Column
        {
            get { return Schema.Columns[42]; }
        }
        
        
        
        public static TableSchema.TableColumn Userflag1Column
        {
            get { return Schema.Columns[43]; }
        }
        
        
        
        public static TableSchema.TableColumn Userflag2Column
        {
            get { return Schema.Columns[44]; }
        }
        
        
        
        public static TableSchema.TableColumn Userflag3Column
        {
            get { return Schema.Columns[45]; }
        }
        
        
        
        public static TableSchema.TableColumn Userflag4Column
        {
            get { return Schema.Columns[46]; }
        }
        
        
        
        public static TableSchema.TableColumn Userflag5Column
        {
            get { return Schema.Columns[47]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfloat1Column
        {
            get { return Schema.Columns[48]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfloat2Column
        {
            get { return Schema.Columns[49]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfloat3Column
        {
            get { return Schema.Columns[50]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfloat4Column
        {
            get { return Schema.Columns[51]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfloat5Column
        {
            get { return Schema.Columns[52]; }
        }
        
        
        
        public static TableSchema.TableColumn Userint1Column
        {
            get { return Schema.Columns[53]; }
        }
        
        
        
        public static TableSchema.TableColumn Userint2Column
        {
            get { return Schema.Columns[54]; }
        }
        
        
        
        public static TableSchema.TableColumn Userint3Column
        {
            get { return Schema.Columns[55]; }
        }
        
        
        
        public static TableSchema.TableColumn Userint4Column
        {
            get { return Schema.Columns[56]; }
        }
        
        
        
        public static TableSchema.TableColumn Userint5Column
        {
            get { return Schema.Columns[57]; }
        }
        
        
        
        public static TableSchema.TableColumn IsDeliveryColumn
        {
            get { return Schema.Columns[58]; }
        }
        
        
        
        public static TableSchema.TableColumn GSTRuleColumn
        {
            get { return Schema.Columns[59]; }
        }
        
        
        
        public static TableSchema.TableColumn IsVitaMixColumn
        {
            get { return Schema.Columns[60]; }
        }
        
        
        
        public static TableSchema.TableColumn IsWaterFilterColumn
        {
            get { return Schema.Columns[61]; }
        }
        
        
        
        public static TableSchema.TableColumn IsYoungColumn
        {
            get { return Schema.Columns[62]; }
        }
        
        
        
        public static TableSchema.TableColumn IsJuicePlusColumn
        {
            get { return Schema.Columns[63]; }
        }
        
        
        
        public static TableSchema.TableColumn IsCommissionColumn
        {
            get { return Schema.Columns[64]; }
        }
        
        
        
        public static TableSchema.TableColumn ItemImageColumn
        {
            get { return Schema.Columns[65]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfloat6Column
        {
            get { return Schema.Columns[66]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfloat7Column
        {
            get { return Schema.Columns[67]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfloat8Column
        {
            get { return Schema.Columns[68]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfloat9Column
        {
            get { return Schema.Columns[69]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfloat10Column
        {
            get { return Schema.Columns[70]; }
        }
        
        
        
        public static TableSchema.TableColumn AvgCostPriceColumn
        {
            get { return Schema.Columns[71]; }
        }
        
        
        
        public static TableSchema.TableColumn BalanceQuantityColumn
        {
            get { return Schema.Columns[72]; }
        }


        public static TableSchema.TableColumn Userflag6Column
        {
            get { return Schema.Columns[73]; }
        }



        public static TableSchema.TableColumn Userflag7Column
        {
            get { return Schema.Columns[74]; }
        }



        public static TableSchema.TableColumn Userflag8Column
        {
            get { return Schema.Columns[75]; }
        }



        public static TableSchema.TableColumn Userflag9Column
        {
            get { return Schema.Columns[76]; }
        }



        public static TableSchema.TableColumn Userflag10Column
        {
            get { return Schema.Columns[77]; }
        }



        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string ItemNo = @"ItemNo";
			 public static string ItemName = @"ItemName";
			 public static string Barcode = @"Barcode";
			 public static string CategoryName = @"CategoryName";
			 public static string RetailPrice = @"RetailPrice";
			 public static string FactoryPrice = @"FactoryPrice";
			 public static string MinimumPrice = @"MinimumPrice";
			 public static string ItemDesc = @"ItemDesc";
			 public static string IsServiceItem = @"IsServiceItem";
			 public static string IsInInventory = @"IsInInventory";
			 public static string IsNonDiscountable = @"IsNonDiscountable";
			 public static string IsCourse = @"IsCourse";
			 public static string CourseTypeID = @"CourseTypeID";
			 public static string Brand = @"Brand";
			 public static string ProductLine = @"ProductLine";
			 public static string Attributes1 = @"Attributes1";
			 public static string Attributes2 = @"Attributes2";
			 public static string Attributes3 = @"Attributes3";
			 public static string Attributes4 = @"Attributes4";
			 public static string Attributes5 = @"Attributes5";
			 public static string Attributes6 = @"Attributes6";
			 public static string Attributes7 = @"Attributes7";
			 public static string Attributes8 = @"Attributes8";
			 public static string Remark = @"Remark";
			 public static string ProductionDate = @"ProductionDate";
			 public static string IsGST = @"IsGST";
			 public static string HasWarranty = @"hasWarranty";
			 public static string CreatedOn = @"CreatedOn";
			 public static string CreatedBy = @"CreatedBy";
			 public static string ModifiedOn = @"ModifiedOn";
			 public static string ModifiedBy = @"ModifiedBy";
			 public static string UniqueID = @"UniqueID";
			 public static string Deleted = @"Deleted";
			 public static string Userfld1 = @"userfld1";
			 public static string Userfld2 = @"userfld2";
			 public static string Userfld3 = @"userfld3";
			 public static string Userfld4 = @"userfld4";
			 public static string Userfld5 = @"userfld5";
			 public static string Userfld6 = @"userfld6";
			 public static string Userfld7 = @"userfld7";
			 public static string Userfld8 = @"userfld8";
			 public static string Userfld9 = @"userfld9";
			 public static string Userfld10 = @"userfld10";
			 public static string Userflag1 = @"userflag1";
			 public static string Userflag2 = @"userflag2";
			 public static string Userflag3 = @"userflag3";
			 public static string Userflag4 = @"userflag4";
			 public static string Userflag5 = @"userflag5";
			 public static string Userfloat1 = @"userfloat1";
			 public static string Userfloat2 = @"userfloat2";
			 public static string Userfloat3 = @"userfloat3";
			 public static string Userfloat4 = @"userfloat4";
			 public static string Userfloat5 = @"userfloat5";
			 public static string Userint1 = @"userint1";
			 public static string Userint2 = @"userint2";
			 public static string Userint3 = @"userint3";
			 public static string Userint4 = @"userint4";
			 public static string Userint5 = @"userint5";
			 public static string IsDelivery = @"IsDelivery";
			 public static string GSTRule = @"GSTRule";
			 public static string IsVitaMix = @"IsVitaMix";
			 public static string IsWaterFilter = @"IsWaterFilter";
			 public static string IsYoung = @"IsYoung";
			 public static string IsJuicePlus = @"IsJuicePlus";
			 public static string IsCommission = @"IsCommission";
			 public static string ItemImage = @"ItemImage";
			 public static string Userfloat6 = @"Userfloat6";
			 public static string Userfloat7 = @"Userfloat7";
			 public static string Userfloat8 = @"Userfloat8";
			 public static string Userfloat9 = @"Userfloat9";
			 public static string Userfloat10 = @"Userfloat10";
			 public static string AvgCostPrice = @"AvgCostPrice";
			 public static string BalanceQuantity = @"BalanceQuantity";
             public static string Userflag6 = @"userflag6";
             public static string Userflag7 = @"userflag7";
             public static string Userflag8 = @"userflag8";
             public static string Userflag9 = @"userflag9";
             public static string Userflag10 = @"userflag10";
						
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
