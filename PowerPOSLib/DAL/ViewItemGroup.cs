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
    /// Strongly-typed collection for the ViewItemGroup class.
    /// </summary>
    [Serializable]
    public partial class ViewItemGroupCollection : ReadOnlyList<ViewItemGroup, ViewItemGroupCollection>
    {        
        public ViewItemGroupCollection() {}
    }
    /// <summary>
    /// This is  Read-only wrapper class for the ViewItemGroup view.
    /// </summary>
    [Serializable]
    public partial class ViewItemGroup : ReadOnlyRecord<ViewItemGroup>, IReadOnlyRecord
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
                TableSchema.Table schema = new TableSchema.Table("ViewItemGroup", TableType.View, DataService.GetInstance("PowerPOS"));
                schema.Columns = new TableSchema.TableColumnCollection();
                schema.SchemaName = @"dbo";
                //columns
                
                TableSchema.TableColumn colvarItemGroupMapID = new TableSchema.TableColumn(schema);
                colvarItemGroupMapID.ColumnName = "ItemGroupMapID";
                colvarItemGroupMapID.DataType = DbType.Int32;
                colvarItemGroupMapID.MaxLength = 0;
                colvarItemGroupMapID.AutoIncrement = false;
                colvarItemGroupMapID.IsNullable = false;
                colvarItemGroupMapID.IsPrimaryKey = false;
                colvarItemGroupMapID.IsForeignKey = false;
                colvarItemGroupMapID.IsReadOnly = false;
                
                schema.Columns.Add(colvarItemGroupMapID);
                
                TableSchema.TableColumn colvarItemGroupID = new TableSchema.TableColumn(schema);
                colvarItemGroupID.ColumnName = "ItemGroupID";
                colvarItemGroupID.DataType = DbType.Int32;
                colvarItemGroupID.MaxLength = 0;
                colvarItemGroupID.AutoIncrement = false;
                colvarItemGroupID.IsNullable = false;
                colvarItemGroupID.IsPrimaryKey = false;
                colvarItemGroupID.IsForeignKey = false;
                colvarItemGroupID.IsReadOnly = false;
                
                schema.Columns.Add(colvarItemGroupID);
                
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
                
                TableSchema.TableColumn colvarUnitQty = new TableSchema.TableColumn(schema);
                colvarUnitQty.ColumnName = "UnitQty";
                colvarUnitQty.DataType = DbType.Int32;
                colvarUnitQty.MaxLength = 0;
                colvarUnitQty.AutoIncrement = false;
                colvarUnitQty.IsNullable = false;
                colvarUnitQty.IsPrimaryKey = false;
                colvarUnitQty.IsForeignKey = false;
                colvarUnitQty.IsReadOnly = false;
                
                schema.Columns.Add(colvarUnitQty);
                
                TableSchema.TableColumn colvarExpr1 = new TableSchema.TableColumn(schema);
                colvarExpr1.ColumnName = "Expr1";
                colvarExpr1.DataType = DbType.AnsiString;
                colvarExpr1.MaxLength = 50;
                colvarExpr1.AutoIncrement = false;
                colvarExpr1.IsNullable = false;
                colvarExpr1.IsPrimaryKey = false;
                colvarExpr1.IsForeignKey = false;
                colvarExpr1.IsReadOnly = false;
                
                schema.Columns.Add(colvarExpr1);
                
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
                
                TableSchema.TableColumn colvarIsGST = new TableSchema.TableColumn(schema);
                colvarIsGST.ColumnName = "IsGST";
                colvarIsGST.DataType = DbType.Boolean;
                colvarIsGST.MaxLength = 0;
                colvarIsGST.AutoIncrement = false;
                colvarIsGST.IsNullable = true;
                colvarIsGST.IsPrimaryKey = false;
                colvarIsGST.IsForeignKey = false;
                colvarIsGST.IsReadOnly = false;
                
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
                
                schema.Columns.Add(colvarHasWarranty);
                
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
                
                TableSchema.TableColumn colvarItemGroupMapDeleted = new TableSchema.TableColumn(schema);
                colvarItemGroupMapDeleted.ColumnName = "ItemGroupMapDeleted";
                colvarItemGroupMapDeleted.DataType = DbType.Boolean;
                colvarItemGroupMapDeleted.MaxLength = 0;
                colvarItemGroupMapDeleted.AutoIncrement = false;
                colvarItemGroupMapDeleted.IsNullable = true;
                colvarItemGroupMapDeleted.IsPrimaryKey = false;
                colvarItemGroupMapDeleted.IsForeignKey = false;
                colvarItemGroupMapDeleted.IsReadOnly = false;
                
                schema.Columns.Add(colvarItemGroupMapDeleted);
                
                TableSchema.TableColumn colvarItemGroupName = new TableSchema.TableColumn(schema);
                colvarItemGroupName.ColumnName = "ItemGroupName";
                colvarItemGroupName.DataType = DbType.AnsiString;
                colvarItemGroupName.MaxLength = 50;
                colvarItemGroupName.AutoIncrement = false;
                colvarItemGroupName.IsNullable = false;
                colvarItemGroupName.IsPrimaryKey = false;
                colvarItemGroupName.IsForeignKey = false;
                colvarItemGroupName.IsReadOnly = false;
                
                schema.Columns.Add(colvarItemGroupName);
                
                TableSchema.TableColumn colvarItemGroupBarcode = new TableSchema.TableColumn(schema);
                colvarItemGroupBarcode.ColumnName = "ItemGroupBarcode";
                colvarItemGroupBarcode.DataType = DbType.AnsiString;
                colvarItemGroupBarcode.MaxLength = 50;
                colvarItemGroupBarcode.AutoIncrement = false;
                colvarItemGroupBarcode.IsNullable = true;
                colvarItemGroupBarcode.IsPrimaryKey = false;
                colvarItemGroupBarcode.IsForeignKey = false;
                colvarItemGroupBarcode.IsReadOnly = false;
                
                schema.Columns.Add(colvarItemGroupBarcode);
                
                TableSchema.TableColumn colvarItemGroupDeleted = new TableSchema.TableColumn(schema);
                colvarItemGroupDeleted.ColumnName = "ItemGroupDeleted";
                colvarItemGroupDeleted.DataType = DbType.Boolean;
                colvarItemGroupDeleted.MaxLength = 0;
                colvarItemGroupDeleted.AutoIncrement = false;
                colvarItemGroupDeleted.IsNullable = true;
                colvarItemGroupDeleted.IsPrimaryKey = false;
                colvarItemGroupDeleted.IsForeignKey = false;
                colvarItemGroupDeleted.IsReadOnly = false;
                
                schema.Columns.Add(colvarItemGroupDeleted);
                
                
                BaseSchema = schema;
                //add this schema to the provider
                //so we can query it later
                DataService.Providers["PowerPOS"].AddSchema("ViewItemGroup",schema);
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
	    public ViewItemGroup()
	    {
            SetSQLProps();
            SetDefaults();
            MarkNew();
        }
        public ViewItemGroup(bool useDatabaseDefaults)
	    {
		    SetSQLProps();
		    if(useDatabaseDefaults)
		    {
				ForceDefaults();
			}
			MarkNew();
	    }
	    
	    public ViewItemGroup(object keyID)
	    {
		    SetSQLProps();
		    LoadByKey(keyID);
	    }
    	 
	    public ViewItemGroup(string columnName, object columnValue)
        {
            SetSQLProps();
            LoadByParam(columnName,columnValue);
        }
        
	    #endregion
	    
	    #region Props
	    
          
        [XmlAttribute("ItemGroupMapID")]
        [Bindable(true)]
        public int ItemGroupMapID 
	    {
		    get
		    {
			    return GetColumnValue<int>("ItemGroupMapID");
		    }
            set 
		    {
			    SetColumnValue("ItemGroupMapID", value);
            }
        }
	      
        [XmlAttribute("ItemGroupID")]
        [Bindable(true)]
        public int ItemGroupID 
	    {
		    get
		    {
			    return GetColumnValue<int>("ItemGroupID");
		    }
            set 
		    {
			    SetColumnValue("ItemGroupID", value);
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
	      
        [XmlAttribute("UnitQty")]
        [Bindable(true)]
        public int UnitQty 
	    {
		    get
		    {
			    return GetColumnValue<int>("UnitQty");
		    }
            set 
		    {
			    SetColumnValue("UnitQty", value);
            }
        }
	      
        [XmlAttribute("Expr1")]
        [Bindable(true)]
        public string Expr1 
	    {
		    get
		    {
			    return GetColumnValue<string>("Expr1");
		    }
            set 
		    {
			    SetColumnValue("Expr1", value);
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
	      
        [XmlAttribute("IsGST")]
        [Bindable(true)]
        public bool? IsGST 
	    {
		    get
		    {
			    return GetColumnValue<bool?>("IsGST");
		    }
            set 
		    {
			    SetColumnValue("IsGST", value);
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
	      
        [XmlAttribute("ItemGroupMapDeleted")]
        [Bindable(true)]
        public bool? ItemGroupMapDeleted 
	    {
		    get
		    {
			    return GetColumnValue<bool?>("ItemGroupMapDeleted");
		    }
            set 
		    {
			    SetColumnValue("ItemGroupMapDeleted", value);
            }
        }
	      
        [XmlAttribute("ItemGroupName")]
        [Bindable(true)]
        public string ItemGroupName 
	    {
		    get
		    {
			    return GetColumnValue<string>("ItemGroupName");
		    }
            set 
		    {
			    SetColumnValue("ItemGroupName", value);
            }
        }
	      
        [XmlAttribute("ItemGroupBarcode")]
        [Bindable(true)]
        public string ItemGroupBarcode 
	    {
		    get
		    {
			    return GetColumnValue<string>("ItemGroupBarcode");
		    }
            set 
		    {
			    SetColumnValue("ItemGroupBarcode", value);
            }
        }
	      
        [XmlAttribute("ItemGroupDeleted")]
        [Bindable(true)]
        public bool? ItemGroupDeleted 
	    {
		    get
		    {
			    return GetColumnValue<bool?>("ItemGroupDeleted");
		    }
            set 
		    {
			    SetColumnValue("ItemGroupDeleted", value);
            }
        }
	    
	    #endregion
    
	    #region Columns Struct
	    public struct Columns
	    {
		    
		    
            public static string ItemGroupMapID = @"ItemGroupMapID";
            
            public static string ItemGroupID = @"ItemGroupID";
            
            public static string ItemNo = @"ItemNo";
            
            public static string UnitQty = @"UnitQty";
            
            public static string Expr1 = @"Expr1";
            
            public static string ItemName = @"ItemName";
            
            public static string Barcode = @"Barcode";
            
            public static string CategoryName = @"CategoryName";
            
            public static string RetailPrice = @"RetailPrice";
            
            public static string FactoryPrice = @"FactoryPrice";
            
            public static string MinimumPrice = @"MinimumPrice";
            
            public static string ItemDesc = @"ItemDesc";
            
            public static string IsInInventory = @"IsInInventory";
            
            public static string IsNonDiscountable = @"IsNonDiscountable";
            
            public static string IsCourse = @"IsCourse";
            
            public static string IsDelivery = @"IsDelivery";
            
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
            
            public static string Deleted = @"Deleted";
            
            public static string ItemGroupMapDeleted = @"ItemGroupMapDeleted";
            
            public static string ItemGroupName = @"ItemGroupName";
            
            public static string ItemGroupBarcode = @"ItemGroupBarcode";
            
            public static string ItemGroupDeleted = @"ItemGroupDeleted";
            
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
