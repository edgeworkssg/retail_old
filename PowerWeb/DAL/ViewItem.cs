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
namespace PowerPOS{
    /// <summary>
    /// Strongly-typed collection for the ViewItem class.
    /// </summary>
    [Serializable]
    public partial class ViewItemCollection : ReadOnlyList<ViewItem, ViewItemCollection>
    {        
        public ViewItemCollection() {}
    }
    /// <summary>
    /// This is  Read-only wrapper class for the ViewItem view.
    /// </summary>
    [Serializable]
    public partial class ViewItem : ReadOnlyRecord<ViewItem>, IReadOnlyRecord
    {
    
	    #region Default Settings
	    protected static void SetSQLProps() 
	    {
		    GetTableSchema();
	    }
	    #endregion
        #region Schema Accessor
	    public static TableSchema.Table Schema
        {
            get
            {
                if (BaseSchema == null)
                {
                    SetSQLProps();
                }
                return BaseSchema;
            }
        }
    	
        private static void GetTableSchema() 
        {
            if(!IsSchemaInitialized)
            {
                //Schema declaration
                TableSchema.Table schema = new TableSchema.Table("ViewItem", TableType.View, DataService.GetInstance("PowerPOS"));
                schema.Columns = new TableSchema.TableColumnCollection();
                schema.SchemaName = @"dbo";
                //columns
                
                TableSchema.TableColumn colvarIsForSale = new TableSchema.TableColumn(schema);
                colvarIsForSale.ColumnName = "IsForSale";
                colvarIsForSale.DataType = DbType.Boolean;
                colvarIsForSale.MaxLength = 0;
                colvarIsForSale.AutoIncrement = false;
                colvarIsForSale.IsNullable = false;
                colvarIsForSale.IsPrimaryKey = false;
                colvarIsForSale.IsForeignKey = false;
                colvarIsForSale.IsReadOnly = false;
                
                schema.Columns.Add(colvarIsForSale);
                
                TableSchema.TableColumn colvarIsDiscountable = new TableSchema.TableColumn(schema);
                colvarIsDiscountable.ColumnName = "IsDiscountable";
                colvarIsDiscountable.DataType = DbType.Boolean;
                colvarIsDiscountable.MaxLength = 0;
                colvarIsDiscountable.AutoIncrement = false;
                colvarIsDiscountable.IsNullable = false;
                colvarIsDiscountable.IsPrimaryKey = false;
                colvarIsDiscountable.IsForeignKey = false;
                colvarIsDiscountable.IsReadOnly = false;
                
                schema.Columns.Add(colvarIsDiscountable);
                
                TableSchema.TableColumn colvarCategoryName = new TableSchema.TableColumn(schema);
                colvarCategoryName.ColumnName = "CategoryName";
                colvarCategoryName.DataType = DbType.String;
                colvarCategoryName.MaxLength = 250;
                colvarCategoryName.AutoIncrement = false;
                colvarCategoryName.IsNullable = false;
                colvarCategoryName.IsPrimaryKey = false;
                colvarCategoryName.IsForeignKey = false;
                colvarCategoryName.IsReadOnly = false;
                
                schema.Columns.Add(colvarCategoryName);
                
                TableSchema.TableColumn colvarCategoryId = new TableSchema.TableColumn(schema);
                colvarCategoryId.ColumnName = "Category_ID";
                colvarCategoryId.DataType = DbType.AnsiString;
                colvarCategoryId.MaxLength = 50;
                colvarCategoryId.AutoIncrement = false;
                colvarCategoryId.IsNullable = true;
                colvarCategoryId.IsPrimaryKey = false;
                colvarCategoryId.IsForeignKey = false;
                colvarCategoryId.IsReadOnly = false;
                
                schema.Columns.Add(colvarCategoryId);
                
                TableSchema.TableColumn colvarIsGST = new TableSchema.TableColumn(schema);
                colvarIsGST.ColumnName = "IsGST";
                colvarIsGST.DataType = DbType.Boolean;
                colvarIsGST.MaxLength = 0;
                colvarIsGST.AutoIncrement = false;
                colvarIsGST.IsNullable = false;
                colvarIsGST.IsPrimaryKey = false;
                colvarIsGST.IsForeignKey = false;
                colvarIsGST.IsReadOnly = false;
                
                schema.Columns.Add(colvarIsGST);
                
                TableSchema.TableColumn colvarAccountCategory = new TableSchema.TableColumn(schema);
                colvarAccountCategory.ColumnName = "AccountCategory";
                colvarAccountCategory.DataType = DbType.AnsiString;
                colvarAccountCategory.MaxLength = 250;
                colvarAccountCategory.AutoIncrement = false;
                colvarAccountCategory.IsNullable = true;
                colvarAccountCategory.IsPrimaryKey = false;
                colvarAccountCategory.IsForeignKey = false;
                colvarAccountCategory.IsReadOnly = false;
                
                schema.Columns.Add(colvarAccountCategory);
                
                TableSchema.TableColumn colvarItemNo = new TableSchema.TableColumn(schema);
                colvarItemNo.ColumnName = "ItemNo";
                colvarItemNo.DataType = DbType.AnsiString;
                colvarItemNo.MaxLength = 50;
                colvarItemNo.AutoIncrement = false;
                colvarItemNo.IsNullable = false;
                colvarItemNo.IsPrimaryKey = false;
                colvarItemNo.IsForeignKey = false;
                colvarItemNo.IsReadOnly = false;
                
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
                
                schema.Columns.Add(colvarBarcode);
                
                TableSchema.TableColumn colvarRetailPrice = new TableSchema.TableColumn(schema);
                colvarRetailPrice.ColumnName = "RetailPrice";
                colvarRetailPrice.DataType = DbType.Currency;
                colvarRetailPrice.MaxLength = 0;
                colvarRetailPrice.AutoIncrement = false;
                colvarRetailPrice.IsNullable = false;
                colvarRetailPrice.IsPrimaryKey = false;
                colvarRetailPrice.IsForeignKey = false;
                colvarRetailPrice.IsReadOnly = false;
                
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
                
                schema.Columns.Add(colvarItemDesc);
                
                TableSchema.TableColumn colvarIsInInventory = new TableSchema.TableColumn(schema);
                colvarIsInInventory.ColumnName = "IsInInventory";
                colvarIsInInventory.DataType = DbType.Boolean;
                colvarIsInInventory.MaxLength = 0;
                colvarIsInInventory.AutoIncrement = false;
                colvarIsInInventory.IsNullable = false;
                colvarIsInInventory.IsPrimaryKey = false;
                colvarIsInInventory.IsForeignKey = false;
                colvarIsInInventory.IsReadOnly = false;
                
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
                
                schema.Columns.Add(colvarIsNonDiscountable);
                
                TableSchema.TableColumn colvarBrand = new TableSchema.TableColumn(schema);
                colvarBrand.ColumnName = "Brand";
                colvarBrand.DataType = DbType.String;
                colvarBrand.MaxLength = 50;
                colvarBrand.AutoIncrement = false;
                colvarBrand.IsNullable = true;
                colvarBrand.IsPrimaryKey = false;
                colvarBrand.IsForeignKey = false;
                colvarBrand.IsReadOnly = false;
                
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
                
                schema.Columns.Add(colvarProductLine);
                
                TableSchema.TableColumn colvarRemark = new TableSchema.TableColumn(schema);
                colvarRemark.ColumnName = "Remark";
                colvarRemark.DataType = DbType.String;
                colvarRemark.MaxLength = -1;
                colvarRemark.AutoIncrement = false;
                colvarRemark.IsNullable = true;
                colvarRemark.IsPrimaryKey = false;
                colvarRemark.IsForeignKey = false;
                colvarRemark.IsReadOnly = false;
                
                schema.Columns.Add(colvarRemark);
                
                TableSchema.TableColumn colvarDeleted = new TableSchema.TableColumn(schema);
                colvarDeleted.ColumnName = "Deleted";
                colvarDeleted.DataType = DbType.Boolean;
                colvarDeleted.MaxLength = 0;
                colvarDeleted.AutoIncrement = false;
                colvarDeleted.IsNullable = true;
                colvarDeleted.IsPrimaryKey = false;
                colvarDeleted.IsForeignKey = false;
                colvarDeleted.IsReadOnly = false;
                
                schema.Columns.Add(colvarDeleted);
                
                TableSchema.TableColumn colvarAttributes1 = new TableSchema.TableColumn(schema);
                colvarAttributes1.ColumnName = "Attributes1";
                colvarAttributes1.DataType = DbType.String;
                colvarAttributes1.MaxLength = -1;
                colvarAttributes1.AutoIncrement = false;
                colvarAttributes1.IsNullable = true;
                colvarAttributes1.IsPrimaryKey = false;
                colvarAttributes1.IsForeignKey = false;
                colvarAttributes1.IsReadOnly = false;
                
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
                
                schema.Columns.Add(colvarAttributes6);
                
                TableSchema.TableColumn colvarAttributes8 = new TableSchema.TableColumn(schema);
                colvarAttributes8.ColumnName = "Attributes8";
                colvarAttributes8.DataType = DbType.String;
                colvarAttributes8.MaxLength = -1;
                colvarAttributes8.AutoIncrement = false;
                colvarAttributes8.IsNullable = true;
                colvarAttributes8.IsPrimaryKey = false;
                colvarAttributes8.IsForeignKey = false;
                colvarAttributes8.IsReadOnly = false;
                
                schema.Columns.Add(colvarAttributes8);
                
                TableSchema.TableColumn colvarAttributes7 = new TableSchema.TableColumn(schema);
                colvarAttributes7.ColumnName = "Attributes7";
                colvarAttributes7.DataType = DbType.String;
                colvarAttributes7.MaxLength = -1;
                colvarAttributes7.AutoIncrement = false;
                colvarAttributes7.IsNullable = true;
                colvarAttributes7.IsPrimaryKey = false;
                colvarAttributes7.IsForeignKey = false;
                colvarAttributes7.IsReadOnly = false;
                
                schema.Columns.Add(colvarAttributes7);
                
                TableSchema.TableColumn colvarItemDepartmentId = new TableSchema.TableColumn(schema);
                colvarItemDepartmentId.ColumnName = "ItemDepartmentId";
                colvarItemDepartmentId.DataType = DbType.AnsiString;
                colvarItemDepartmentId.MaxLength = 50;
                colvarItemDepartmentId.AutoIncrement = false;
                colvarItemDepartmentId.IsNullable = true;
                colvarItemDepartmentId.IsPrimaryKey = false;
                colvarItemDepartmentId.IsForeignKey = false;
                colvarItemDepartmentId.IsReadOnly = false;
                
                schema.Columns.Add(colvarItemDepartmentId);
                
                TableSchema.TableColumn colvarDepartmentName = new TableSchema.TableColumn(schema);
                colvarDepartmentName.ColumnName = "DepartmentName";
                colvarDepartmentName.DataType = DbType.String;
                colvarDepartmentName.MaxLength = 50;
                colvarDepartmentName.AutoIncrement = false;
                colvarDepartmentName.IsNullable = false;
                colvarDepartmentName.IsPrimaryKey = false;
                colvarDepartmentName.IsForeignKey = false;
                colvarDepartmentName.IsReadOnly = false;
                
                schema.Columns.Add(colvarDepartmentName);
                
                TableSchema.TableColumn colvarSearch = new TableSchema.TableColumn(schema);
                colvarSearch.ColumnName = "search";
                colvarSearch.DataType = DbType.String;
                colvarSearch.MaxLength = 1005;
                colvarSearch.AutoIncrement = false;
                colvarSearch.IsNullable = true;
                colvarSearch.IsPrimaryKey = false;
                colvarSearch.IsForeignKey = false;
                colvarSearch.IsReadOnly = false;
                
                schema.Columns.Add(colvarSearch);
                
                TableSchema.TableColumn colvarIsServiceItem = new TableSchema.TableColumn(schema);
                colvarIsServiceItem.ColumnName = "IsServiceItem";
                colvarIsServiceItem.DataType = DbType.Boolean;
                colvarIsServiceItem.MaxLength = 0;
                colvarIsServiceItem.AutoIncrement = false;
                colvarIsServiceItem.IsNullable = true;
                colvarIsServiceItem.IsPrimaryKey = false;
                colvarIsServiceItem.IsForeignKey = false;
                colvarIsServiceItem.IsReadOnly = false;
                
                schema.Columns.Add(colvarIsServiceItem);
                
                TableSchema.TableColumn colvarIsCourse = new TableSchema.TableColumn(schema);
                colvarIsCourse.ColumnName = "IsCourse";
                colvarIsCourse.DataType = DbType.Boolean;
                colvarIsCourse.MaxLength = 0;
                colvarIsCourse.AutoIncrement = false;
                colvarIsCourse.IsNullable = true;
                colvarIsCourse.IsPrimaryKey = false;
                colvarIsCourse.IsForeignKey = false;
                colvarIsCourse.IsReadOnly = false;
                
                schema.Columns.Add(colvarIsCourse);
                
                TableSchema.TableColumn colvarCourseTypeID = new TableSchema.TableColumn(schema);
                colvarCourseTypeID.ColumnName = "CourseTypeID";
                colvarCourseTypeID.DataType = DbType.AnsiString;
                colvarCourseTypeID.MaxLength = 50;
                colvarCourseTypeID.AutoIncrement = false;
                colvarCourseTypeID.IsNullable = true;
                colvarCourseTypeID.IsPrimaryKey = false;
                colvarCourseTypeID.IsForeignKey = false;
                colvarCourseTypeID.IsReadOnly = false;
                
                schema.Columns.Add(colvarCourseTypeID);
                
                TableSchema.TableColumn colvarProductionDate = new TableSchema.TableColumn(schema);
                colvarProductionDate.ColumnName = "ProductionDate";
                colvarProductionDate.DataType = DbType.DateTime;
                colvarProductionDate.MaxLength = 0;
                colvarProductionDate.AutoIncrement = false;
                colvarProductionDate.IsNullable = true;
                colvarProductionDate.IsPrimaryKey = false;
                colvarProductionDate.IsForeignKey = false;
                colvarProductionDate.IsReadOnly = false;
                
                schema.Columns.Add(colvarProductionDate);
                
                TableSchema.TableColumn colvarExpr1 = new TableSchema.TableColumn(schema);
                colvarExpr1.ColumnName = "Expr1";
                colvarExpr1.DataType = DbType.Boolean;
                colvarExpr1.MaxLength = 0;
                colvarExpr1.AutoIncrement = false;
                colvarExpr1.IsNullable = true;
                colvarExpr1.IsPrimaryKey = false;
                colvarExpr1.IsForeignKey = false;
                colvarExpr1.IsReadOnly = false;
                
                schema.Columns.Add(colvarExpr1);
                
                TableSchema.TableColumn colvarHasWarranty = new TableSchema.TableColumn(schema);
                colvarHasWarranty.ColumnName = "hasWarranty";
                colvarHasWarranty.DataType = DbType.Boolean;
                colvarHasWarranty.MaxLength = 0;
                colvarHasWarranty.AutoIncrement = false;
                colvarHasWarranty.IsNullable = true;
                colvarHasWarranty.IsPrimaryKey = false;
                colvarHasWarranty.IsForeignKey = false;
                colvarHasWarranty.IsReadOnly = false;
                
                schema.Columns.Add(colvarHasWarranty);
                
                TableSchema.TableColumn colvarIsDelivery = new TableSchema.TableColumn(schema);
                colvarIsDelivery.ColumnName = "IsDelivery";
                colvarIsDelivery.DataType = DbType.Boolean;
                colvarIsDelivery.MaxLength = 0;
                colvarIsDelivery.AutoIncrement = false;
                colvarIsDelivery.IsNullable = true;
                colvarIsDelivery.IsPrimaryKey = false;
                colvarIsDelivery.IsForeignKey = false;
                colvarIsDelivery.IsReadOnly = false;
                
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
                
                schema.Columns.Add(colvarIsCommission);
                
                TableSchema.TableColumn colvarUserflag1 = new TableSchema.TableColumn(schema);
                colvarUserflag1.ColumnName = "Userflag1";
                colvarUserflag1.DataType = DbType.Boolean;
                colvarUserflag1.MaxLength = 0;
                colvarUserflag1.AutoIncrement = false;
                colvarUserflag1.IsNullable = false;
                colvarUserflag1.IsPrimaryKey = false;
                colvarUserflag1.IsForeignKey = false;
                colvarUserflag1.IsReadOnly = false;
                
                schema.Columns.Add(colvarUserflag1);
                
                TableSchema.TableColumn colvarAutoCaptureWeight = new TableSchema.TableColumn(schema);
                colvarAutoCaptureWeight.ColumnName = "AutoCaptureWeight";
                colvarAutoCaptureWeight.DataType = DbType.Boolean;
                colvarAutoCaptureWeight.MaxLength = 0;
                colvarAutoCaptureWeight.AutoIncrement = false;
                colvarAutoCaptureWeight.IsNullable = true;
                colvarAutoCaptureWeight.IsPrimaryKey = false;
                colvarAutoCaptureWeight.IsForeignKey = false;
                colvarAutoCaptureWeight.IsReadOnly = false;
                
                schema.Columns.Add(colvarAutoCaptureWeight);
                
                TableSchema.TableColumn colvarUom = new TableSchema.TableColumn(schema);
                colvarUom.ColumnName = "Uom";
                colvarUom.DataType = DbType.AnsiString;
                colvarUom.MaxLength = 50;
                colvarUom.AutoIncrement = false;
                colvarUom.IsNullable = false;
                colvarUom.IsPrimaryKey = false;
                colvarUom.IsForeignKey = false;
                colvarUom.IsReadOnly = false;
                
                schema.Columns.Add(colvarUom);
                
                
                BaseSchema = schema;
                //add this schema to the provider
                //so we can query it later
                DataService.Providers["PowerPOS"].AddSchema("ViewItem",schema);
            }
        }
        #endregion
        
        #region Query Accessor
	    public static Query CreateQuery()
	    {
		    return new Query(Schema);
	    }
	    #endregion
	    
	    #region .ctors
	    public ViewItem()
	    {
            SetSQLProps();
            SetDefaults();
            MarkNew();
        }
        public ViewItem(bool useDatabaseDefaults)
	    {
		    SetSQLProps();
		    if(useDatabaseDefaults)
		    {
				ForceDefaults();
			}
			MarkNew();
	    }
	    
	    public ViewItem(object keyID)
	    {
		    SetSQLProps();
		    LoadByKey(keyID);
	    }
    	 
	    public ViewItem(string columnName, object columnValue)
        {
            SetSQLProps();
            LoadByParam(columnName,columnValue);
        }
        
	    #endregion
	    
	    #region Props
	    
          
        [XmlAttribute("IsForSale")]
        [Bindable(true)]
        public bool IsForSale 
	    {
		    get
		    {
			    return GetColumnValue<bool>("IsForSale");
		    }
            set 
		    {
			    SetColumnValue("IsForSale", value);
            }
        }
	      
        [XmlAttribute("IsDiscountable")]
        [Bindable(true)]
        public bool IsDiscountable 
	    {
		    get
		    {
			    return GetColumnValue<bool>("IsDiscountable");
		    }
            set 
		    {
			    SetColumnValue("IsDiscountable", value);
            }
        }
	      
        [XmlAttribute("CategoryName")]
        [Bindable(true)]
        public string CategoryName 
	    {
		    get
		    {
			    return GetColumnValue<string>("CategoryName");
		    }
            set 
		    {
			    SetColumnValue("CategoryName", value);
            }
        }
	      
        [XmlAttribute("CategoryId")]
        [Bindable(true)]
        public string CategoryId 
	    {
		    get
		    {
			    return GetColumnValue<string>("Category_ID");
		    }
            set 
		    {
			    SetColumnValue("Category_ID", value);
            }
        }
	      
        [XmlAttribute("IsGST")]
        [Bindable(true)]
        public bool IsGST 
	    {
		    get
		    {
			    return GetColumnValue<bool>("IsGST");
		    }
            set 
		    {
			    SetColumnValue("IsGST", value);
            }
        }
	      
        [XmlAttribute("AccountCategory")]
        [Bindable(true)]
        public string AccountCategory 
	    {
		    get
		    {
			    return GetColumnValue<string>("AccountCategory");
		    }
            set 
		    {
			    SetColumnValue("AccountCategory", value);
            }
        }
	      
        [XmlAttribute("ItemNo")]
        [Bindable(true)]
        public string ItemNo 
	    {
		    get
		    {
			    return GetColumnValue<string>("ItemNo");
		    }
            set 
		    {
			    SetColumnValue("ItemNo", value);
            }
        }
	      
        [XmlAttribute("ItemName")]
        [Bindable(true)]
        public string ItemName 
	    {
		    get
		    {
			    return GetColumnValue<string>("ItemName");
		    }
            set 
		    {
			    SetColumnValue("ItemName", value);
            }
        }
	      
        [XmlAttribute("Barcode")]
        [Bindable(true)]
        public string Barcode 
	    {
		    get
		    {
			    return GetColumnValue<string>("Barcode");
		    }
            set 
		    {
			    SetColumnValue("Barcode", value);
            }
        }
	      
        [XmlAttribute("RetailPrice")]
        [Bindable(true)]
        public decimal RetailPrice 
	    {
		    get
		    {
			    return GetColumnValue<decimal>("RetailPrice");
		    }
            set 
		    {
			    SetColumnValue("RetailPrice", value);
            }
        }
	      
        [XmlAttribute("FactoryPrice")]
        [Bindable(true)]
        public decimal FactoryPrice 
	    {
		    get
		    {
			    return GetColumnValue<decimal>("FactoryPrice");
		    }
            set 
		    {
			    SetColumnValue("FactoryPrice", value);
            }
        }
	      
        [XmlAttribute("MinimumPrice")]
        [Bindable(true)]
        public decimal MinimumPrice 
	    {
		    get
		    {
			    return GetColumnValue<decimal>("MinimumPrice");
		    }
            set 
		    {
			    SetColumnValue("MinimumPrice", value);
            }
        }
	      
        [XmlAttribute("ItemDesc")]
        [Bindable(true)]
        public string ItemDesc 
	    {
		    get
		    {
			    return GetColumnValue<string>("ItemDesc");
		    }
            set 
		    {
			    SetColumnValue("ItemDesc", value);
            }
        }
	      
        [XmlAttribute("IsInInventory")]
        [Bindable(true)]
        public bool IsInInventory 
	    {
		    get
		    {
			    return GetColumnValue<bool>("IsInInventory");
		    }
            set 
		    {
			    SetColumnValue("IsInInventory", value);
            }
        }
	      
        [XmlAttribute("IsNonDiscountable")]
        [Bindable(true)]
        public bool IsNonDiscountable 
	    {
		    get
		    {
			    return GetColumnValue<bool>("IsNonDiscountable");
		    }
            set 
		    {
			    SetColumnValue("IsNonDiscountable", value);
            }
        }
	      
        [XmlAttribute("Brand")]
        [Bindable(true)]
        public string Brand 
	    {
		    get
		    {
			    return GetColumnValue<string>("Brand");
		    }
            set 
		    {
			    SetColumnValue("Brand", value);
            }
        }
	      
        [XmlAttribute("ProductLine")]
        [Bindable(true)]
        public string ProductLine 
	    {
		    get
		    {
			    return GetColumnValue<string>("ProductLine");
		    }
            set 
		    {
			    SetColumnValue("ProductLine", value);
            }
        }
	      
        [XmlAttribute("Remark")]
        [Bindable(true)]
        public string Remark 
	    {
		    get
		    {
			    return GetColumnValue<string>("Remark");
		    }
            set 
		    {
			    SetColumnValue("Remark", value);
            }
        }
	      
        [XmlAttribute("Deleted")]
        [Bindable(true)]
        public bool? Deleted 
	    {
		    get
		    {
			    return GetColumnValue<bool?>("Deleted");
		    }
            set 
		    {
			    SetColumnValue("Deleted", value);
            }
        }
	      
        [XmlAttribute("Attributes1")]
        [Bindable(true)]
        public string Attributes1 
	    {
		    get
		    {
			    return GetColumnValue<string>("Attributes1");
		    }
            set 
		    {
			    SetColumnValue("Attributes1", value);
            }
        }
	      
        [XmlAttribute("Attributes2")]
        [Bindable(true)]
        public string Attributes2 
	    {
		    get
		    {
			    return GetColumnValue<string>("Attributes2");
		    }
            set 
		    {
			    SetColumnValue("Attributes2", value);
            }
        }
	      
        [XmlAttribute("Attributes3")]
        [Bindable(true)]
        public string Attributes3 
	    {
		    get
		    {
			    return GetColumnValue<string>("Attributes3");
		    }
            set 
		    {
			    SetColumnValue("Attributes3", value);
            }
        }
	      
        [XmlAttribute("Attributes4")]
        [Bindable(true)]
        public string Attributes4 
	    {
		    get
		    {
			    return GetColumnValue<string>("Attributes4");
		    }
            set 
		    {
			    SetColumnValue("Attributes4", value);
            }
        }
	      
        [XmlAttribute("Attributes5")]
        [Bindable(true)]
        public string Attributes5 
	    {
		    get
		    {
			    return GetColumnValue<string>("Attributes5");
		    }
            set 
		    {
			    SetColumnValue("Attributes5", value);
            }
        }
	      
        [XmlAttribute("Attributes6")]
        [Bindable(true)]
        public string Attributes6 
	    {
		    get
		    {
			    return GetColumnValue<string>("Attributes6");
		    }
            set 
		    {
			    SetColumnValue("Attributes6", value);
            }
        }
	      
        [XmlAttribute("Attributes8")]
        [Bindable(true)]
        public string Attributes8 
	    {
		    get
		    {
			    return GetColumnValue<string>("Attributes8");
		    }
            set 
		    {
			    SetColumnValue("Attributes8", value);
            }
        }
	      
        [XmlAttribute("Attributes7")]
        [Bindable(true)]
        public string Attributes7 
	    {
		    get
		    {
			    return GetColumnValue<string>("Attributes7");
		    }
            set 
		    {
			    SetColumnValue("Attributes7", value);
            }
        }
	      
        [XmlAttribute("ItemDepartmentId")]
        [Bindable(true)]
        public string ItemDepartmentId 
	    {
		    get
		    {
			    return GetColumnValue<string>("ItemDepartmentId");
		    }
            set 
		    {
			    SetColumnValue("ItemDepartmentId", value);
            }
        }
	      
        [XmlAttribute("DepartmentName")]
        [Bindable(true)]
        public string DepartmentName 
	    {
		    get
		    {
			    return GetColumnValue<string>("DepartmentName");
		    }
            set 
		    {
			    SetColumnValue("DepartmentName", value);
            }
        }
	      
        [XmlAttribute("Search")]
        [Bindable(true)]
        public string Search 
	    {
		    get
		    {
			    return GetColumnValue<string>("search");
		    }
            set 
		    {
			    SetColumnValue("search", value);
            }
        }
	      
        [XmlAttribute("IsServiceItem")]
        [Bindable(true)]
        public bool? IsServiceItem 
	    {
		    get
		    {
			    return GetColumnValue<bool?>("IsServiceItem");
		    }
            set 
		    {
			    SetColumnValue("IsServiceItem", value);
            }
        }
	      
        [XmlAttribute("IsCourse")]
        [Bindable(true)]
        public bool? IsCourse 
	    {
		    get
		    {
			    return GetColumnValue<bool?>("IsCourse");
		    }
            set 
		    {
			    SetColumnValue("IsCourse", value);
            }
        }
	      
        [XmlAttribute("CourseTypeID")]
        [Bindable(true)]
        public string CourseTypeID 
	    {
		    get
		    {
			    return GetColumnValue<string>("CourseTypeID");
		    }
            set 
		    {
			    SetColumnValue("CourseTypeID", value);
            }
        }
	      
        [XmlAttribute("ProductionDate")]
        [Bindable(true)]
        public DateTime? ProductionDate 
	    {
		    get
		    {
			    return GetColumnValue<DateTime?>("ProductionDate");
		    }
            set 
		    {
			    SetColumnValue("ProductionDate", value);
            }
        }
	      
        [XmlAttribute("Expr1")]
        [Bindable(true)]
        public bool? Expr1 
	    {
		    get
		    {
			    return GetColumnValue<bool?>("Expr1");
		    }
            set 
		    {
			    SetColumnValue("Expr1", value);
            }
        }
	      
        [XmlAttribute("HasWarranty")]
        [Bindable(true)]
        public bool? HasWarranty 
	    {
		    get
		    {
			    return GetColumnValue<bool?>("hasWarranty");
		    }
            set 
		    {
			    SetColumnValue("hasWarranty", value);
            }
        }
	      
        [XmlAttribute("IsDelivery")]
        [Bindable(true)]
        public bool? IsDelivery 
	    {
		    get
		    {
			    return GetColumnValue<bool?>("IsDelivery");
		    }
            set 
		    {
			    SetColumnValue("IsDelivery", value);
            }
        }
	      
        [XmlAttribute("GSTRule")]
        [Bindable(true)]
        public int? GSTRule 
	    {
		    get
		    {
			    return GetColumnValue<int?>("GSTRule");
		    }
            set 
		    {
			    SetColumnValue("GSTRule", value);
            }
        }
	      
        [XmlAttribute("IsVitaMix")]
        [Bindable(true)]
        public bool? IsVitaMix 
	    {
		    get
		    {
			    return GetColumnValue<bool?>("IsVitaMix");
		    }
            set 
		    {
			    SetColumnValue("IsVitaMix", value);
            }
        }
	      
        [XmlAttribute("IsWaterFilter")]
        [Bindable(true)]
        public bool? IsWaterFilter 
	    {
		    get
		    {
			    return GetColumnValue<bool?>("IsWaterFilter");
		    }
            set 
		    {
			    SetColumnValue("IsWaterFilter", value);
            }
        }
	      
        [XmlAttribute("IsYoung")]
        [Bindable(true)]
        public bool? IsYoung 
	    {
		    get
		    {
			    return GetColumnValue<bool?>("IsYoung");
		    }
            set 
		    {
			    SetColumnValue("IsYoung", value);
            }
        }
	      
        [XmlAttribute("IsJuicePlus")]
        [Bindable(true)]
        public bool? IsJuicePlus 
	    {
		    get
		    {
			    return GetColumnValue<bool?>("IsJuicePlus");
		    }
            set 
		    {
			    SetColumnValue("IsJuicePlus", value);
            }
        }
	      
        [XmlAttribute("IsCommission")]
        [Bindable(true)]
        public bool? IsCommission 
	    {
		    get
		    {
			    return GetColumnValue<bool?>("IsCommission");
		    }
            set 
		    {
			    SetColumnValue("IsCommission", value);
            }
        }
	      
        [XmlAttribute("Userflag1")]
        [Bindable(true)]
        public bool Userflag1 
	    {
		    get
		    {
			    return GetColumnValue<bool>("Userflag1");
		    }
            set 
		    {
			    SetColumnValue("Userflag1", value);
            }
        }
	      
        [XmlAttribute("AutoCaptureWeight")]
        [Bindable(true)]
        public bool? AutoCaptureWeight 
	    {
		    get
		    {
			    return GetColumnValue<bool?>("AutoCaptureWeight");
		    }
            set 
		    {
			    SetColumnValue("AutoCaptureWeight", value);
            }
        }
	      
        [XmlAttribute("Uom")]
        [Bindable(true)]
        public string Uom 
	    {
		    get
		    {
			    return GetColumnValue<string>("Uom");
		    }
            set 
		    {
			    SetColumnValue("Uom", value);
            }
        }
	    
	    #endregion
    
	    #region Columns Struct
	    public struct Columns
	    {
		    
		    
            public static string IsForSale = @"IsForSale";
            
            public static string IsDiscountable = @"IsDiscountable";
            
            public static string CategoryName = @"CategoryName";
            
            public static string CategoryId = @"Category_ID";
            
            public static string IsGST = @"IsGST";
            
            public static string AccountCategory = @"AccountCategory";
            
            public static string ItemNo = @"ItemNo";
            
            public static string ItemName = @"ItemName";
            
            public static string Barcode = @"Barcode";
            
            public static string RetailPrice = @"RetailPrice";
            
            public static string FactoryPrice = @"FactoryPrice";
            
            public static string MinimumPrice = @"MinimumPrice";
            
            public static string ItemDesc = @"ItemDesc";
            
            public static string IsInInventory = @"IsInInventory";
            
            public static string IsNonDiscountable = @"IsNonDiscountable";
            
            public static string Brand = @"Brand";
            
            public static string ProductLine = @"ProductLine";
            
            public static string Remark = @"Remark";
            
            public static string Deleted = @"Deleted";
            
            public static string Attributes1 = @"Attributes1";
            
            public static string Attributes2 = @"Attributes2";
            
            public static string Attributes3 = @"Attributes3";
            
            public static string Attributes4 = @"Attributes4";
            
            public static string Attributes5 = @"Attributes5";
            
            public static string Attributes6 = @"Attributes6";
            
            public static string Attributes8 = @"Attributes8";
            
            public static string Attributes7 = @"Attributes7";
            
            public static string ItemDepartmentId = @"ItemDepartmentId";
            
            public static string DepartmentName = @"DepartmentName";
            
            public static string Search = @"search";
            
            public static string IsServiceItem = @"IsServiceItem";
            
            public static string IsCourse = @"IsCourse";
            
            public static string CourseTypeID = @"CourseTypeID";
            
            public static string ProductionDate = @"ProductionDate";
            
            public static string Expr1 = @"Expr1";
            
            public static string HasWarranty = @"hasWarranty";
            
            public static string IsDelivery = @"IsDelivery";
            
            public static string GSTRule = @"GSTRule";
            
            public static string IsVitaMix = @"IsVitaMix";
            
            public static string IsWaterFilter = @"IsWaterFilter";
            
            public static string IsYoung = @"IsYoung";
            
            public static string IsJuicePlus = @"IsJuicePlus";
            
            public static string IsCommission = @"IsCommission";
            
            public static string Userflag1 = @"Userflag1";
            
            public static string AutoCaptureWeight = @"AutoCaptureWeight";
            
            public static string Uom = @"Uom";
            
	    }
	    #endregion
	    
	    
	    #region IAbstractRecord Members
        public new CT GetColumnValue<CT>(string columnName) {
            return base.GetColumnValue<CT>(columnName);
        }
        public object GetColumnValue(string columnName) {
            return base.GetColumnValue<object>(columnName);
        }
        #endregion
	    
    }
}
