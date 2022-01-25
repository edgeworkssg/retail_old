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
    /// Strongly-typed collection for the ViewStockTake class.
    /// </summary>
    [Serializable]
    public partial class ViewStockTakeCollection : ReadOnlyList<ViewStockTake, ViewStockTakeCollection>
    {        
        public ViewStockTakeCollection() {}
    }
    /// <summary>
    /// This is  Read-only wrapper class for the ViewStockTake view.
    /// </summary>
    [Serializable]
    public partial class ViewStockTake : ReadOnlyRecord<ViewStockTake>, IReadOnlyRecord
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
                TableSchema.Table schema = new TableSchema.Table("ViewStockTake", TableType.View, DataService.GetInstance("PowerPOS"));
                schema.Columns = new TableSchema.TableColumnCollection();
                schema.SchemaName = @"dbo";
                //columns
                
                TableSchema.TableColumn colvarStockTakeDate = new TableSchema.TableColumn(schema);
                colvarStockTakeDate.ColumnName = "StockTakeDate";
                colvarStockTakeDate.DataType = DbType.DateTime;
                colvarStockTakeDate.MaxLength = 0;
                colvarStockTakeDate.AutoIncrement = false;
                colvarStockTakeDate.IsNullable = false;
                colvarStockTakeDate.IsPrimaryKey = false;
                colvarStockTakeDate.IsForeignKey = false;
                colvarStockTakeDate.IsReadOnly = false;
                
                schema.Columns.Add(colvarStockTakeDate);
                
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
                
                TableSchema.TableColumn colvarStockTakeID = new TableSchema.TableColumn(schema);
                colvarStockTakeID.ColumnName = "StockTakeID";
                colvarStockTakeID.DataType = DbType.Int32;
                colvarStockTakeID.MaxLength = 0;
                colvarStockTakeID.AutoIncrement = false;
                colvarStockTakeID.IsNullable = false;
                colvarStockTakeID.IsPrimaryKey = false;
                colvarStockTakeID.IsForeignKey = false;
                colvarStockTakeID.IsReadOnly = false;
                
                schema.Columns.Add(colvarStockTakeID);
                
                TableSchema.TableColumn colvarInventoryLocationID = new TableSchema.TableColumn(schema);
                colvarInventoryLocationID.ColumnName = "InventoryLocationID";
                colvarInventoryLocationID.DataType = DbType.Int32;
                colvarInventoryLocationID.MaxLength = 0;
                colvarInventoryLocationID.AutoIncrement = false;
                colvarInventoryLocationID.IsNullable = false;
                colvarInventoryLocationID.IsPrimaryKey = false;
                colvarInventoryLocationID.IsForeignKey = false;
                colvarInventoryLocationID.IsReadOnly = false;
                
                schema.Columns.Add(colvarInventoryLocationID);
                
                TableSchema.TableColumn colvarStockTakeQty = new TableSchema.TableColumn(schema);
                colvarStockTakeQty.ColumnName = "StockTakeQty";
                colvarStockTakeQty.DataType = DbType.Decimal;
                colvarStockTakeQty.MaxLength = 0;
                colvarStockTakeQty.AutoIncrement = false;
                colvarStockTakeQty.IsNullable = true;
                colvarStockTakeQty.IsPrimaryKey = false;
                colvarStockTakeQty.IsForeignKey = false;
                colvarStockTakeQty.IsReadOnly = false;
                
                schema.Columns.Add(colvarStockTakeQty);
                
                TableSchema.TableColumn colvarTakenBy = new TableSchema.TableColumn(schema);
                colvarTakenBy.ColumnName = "TakenBy";
                colvarTakenBy.DataType = DbType.AnsiString;
                colvarTakenBy.MaxLength = 50;
                colvarTakenBy.AutoIncrement = false;
                colvarTakenBy.IsNullable = false;
                colvarTakenBy.IsPrimaryKey = false;
                colvarTakenBy.IsForeignKey = false;
                colvarTakenBy.IsReadOnly = false;
                
                schema.Columns.Add(colvarTakenBy);
                
                TableSchema.TableColumn colvarVerifiedBy = new TableSchema.TableColumn(schema);
                colvarVerifiedBy.ColumnName = "VerifiedBy";
                colvarVerifiedBy.DataType = DbType.AnsiString;
                colvarVerifiedBy.MaxLength = 50;
                colvarVerifiedBy.AutoIncrement = false;
                colvarVerifiedBy.IsNullable = false;
                colvarVerifiedBy.IsPrimaryKey = false;
                colvarVerifiedBy.IsForeignKey = false;
                colvarVerifiedBy.IsReadOnly = false;
                
                schema.Columns.Add(colvarVerifiedBy);
                
                TableSchema.TableColumn colvarAuthorizedBy = new TableSchema.TableColumn(schema);
                colvarAuthorizedBy.ColumnName = "AuthorizedBy";
                colvarAuthorizedBy.DataType = DbType.AnsiString;
                colvarAuthorizedBy.MaxLength = 50;
                colvarAuthorizedBy.AutoIncrement = false;
                colvarAuthorizedBy.IsNullable = false;
                colvarAuthorizedBy.IsPrimaryKey = false;
                colvarAuthorizedBy.IsForeignKey = false;
                colvarAuthorizedBy.IsReadOnly = false;
                
                schema.Columns.Add(colvarAuthorizedBy);
                
                TableSchema.TableColumn colvarIsAdjusted = new TableSchema.TableColumn(schema);
                colvarIsAdjusted.ColumnName = "IsAdjusted";
                colvarIsAdjusted.DataType = DbType.Boolean;
                colvarIsAdjusted.MaxLength = 0;
                colvarIsAdjusted.AutoIncrement = false;
                colvarIsAdjusted.IsNullable = false;
                colvarIsAdjusted.IsPrimaryKey = false;
                colvarIsAdjusted.IsForeignKey = false;
                colvarIsAdjusted.IsReadOnly = false;
                
                schema.Columns.Add(colvarIsAdjusted);
                
                TableSchema.TableColumn colvarRemark = new TableSchema.TableColumn(schema);
                colvarRemark.ColumnName = "Remark";
                colvarRemark.DataType = DbType.AnsiString;
                colvarRemark.MaxLength = 150;
                colvarRemark.AutoIncrement = false;
                colvarRemark.IsNullable = true;
                colvarRemark.IsPrimaryKey = false;
                colvarRemark.IsForeignKey = false;
                colvarRemark.IsReadOnly = false;
                
                schema.Columns.Add(colvarRemark);
                
                TableSchema.TableColumn colvarCostOfGoods = new TableSchema.TableColumn(schema);
                colvarCostOfGoods.ColumnName = "CostOfGoods";
                colvarCostOfGoods.DataType = DbType.Currency;
                colvarCostOfGoods.MaxLength = 0;
                colvarCostOfGoods.AutoIncrement = false;
                colvarCostOfGoods.IsNullable = false;
                colvarCostOfGoods.IsPrimaryKey = false;
                colvarCostOfGoods.IsForeignKey = false;
                colvarCostOfGoods.IsReadOnly = false;
                
                schema.Columns.Add(colvarCostOfGoods);
                
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
                
                TableSchema.TableColumn colvarInventoryLocationName = new TableSchema.TableColumn(schema);
                colvarInventoryLocationName.ColumnName = "InventoryLocationName";
                colvarInventoryLocationName.DataType = DbType.AnsiString;
                colvarInventoryLocationName.MaxLength = 50;
                colvarInventoryLocationName.AutoIncrement = false;
                colvarInventoryLocationName.IsNullable = false;
                colvarInventoryLocationName.IsPrimaryKey = false;
                colvarInventoryLocationName.IsForeignKey = false;
                colvarInventoryLocationName.IsReadOnly = false;
                
                schema.Columns.Add(colvarInventoryLocationName);
                
                TableSchema.TableColumn colvarAdjustmentHdrRefNo = new TableSchema.TableColumn(schema);
                colvarAdjustmentHdrRefNo.ColumnName = "AdjustmentHdrRefNo";
                colvarAdjustmentHdrRefNo.DataType = DbType.AnsiString;
                colvarAdjustmentHdrRefNo.MaxLength = 50;
                colvarAdjustmentHdrRefNo.AutoIncrement = false;
                colvarAdjustmentHdrRefNo.IsNullable = true;
                colvarAdjustmentHdrRefNo.IsPrimaryKey = false;
                colvarAdjustmentHdrRefNo.IsForeignKey = false;
                colvarAdjustmentHdrRefNo.IsReadOnly = false;
                
                schema.Columns.Add(colvarAdjustmentHdrRefNo);
                
                TableSchema.TableColumn colvarBalQtyAtEntry = new TableSchema.TableColumn(schema);
                colvarBalQtyAtEntry.ColumnName = "BalQtyAtEntry";
                colvarBalQtyAtEntry.DataType = DbType.Decimal;
                colvarBalQtyAtEntry.MaxLength = 0;
                colvarBalQtyAtEntry.AutoIncrement = false;
                colvarBalQtyAtEntry.IsNullable = true;
                colvarBalQtyAtEntry.IsPrimaryKey = false;
                colvarBalQtyAtEntry.IsForeignKey = false;
                colvarBalQtyAtEntry.IsReadOnly = false;
                
                schema.Columns.Add(colvarBalQtyAtEntry);
                
                TableSchema.TableColumn colvarAdjustmentQty = new TableSchema.TableColumn(schema);
                colvarAdjustmentQty.ColumnName = "AdjustmentQty";
                colvarAdjustmentQty.DataType = DbType.Decimal;
                colvarAdjustmentQty.MaxLength = 0;
                colvarAdjustmentQty.AutoIncrement = false;
                colvarAdjustmentQty.IsNullable = true;
                colvarAdjustmentQty.IsPrimaryKey = false;
                colvarAdjustmentQty.IsForeignKey = false;
                colvarAdjustmentQty.IsReadOnly = false;
                
                schema.Columns.Add(colvarAdjustmentQty);
                
                TableSchema.TableColumn colvarMarked = new TableSchema.TableColumn(schema);
                colvarMarked.ColumnName = "Marked";
                colvarMarked.DataType = DbType.Boolean;
                colvarMarked.MaxLength = 0;
                colvarMarked.AutoIncrement = false;
                colvarMarked.IsNullable = true;
                colvarMarked.IsPrimaryKey = false;
                colvarMarked.IsForeignKey = false;
                colvarMarked.IsReadOnly = false;
                
                schema.Columns.Add(colvarMarked);
                
                TableSchema.TableColumn colvarBatchNo = new TableSchema.TableColumn(schema);
                colvarBatchNo.ColumnName = "BatchNo";
                colvarBatchNo.DataType = DbType.AnsiString;
                colvarBatchNo.MaxLength = 50;
                colvarBatchNo.AutoIncrement = false;
                colvarBatchNo.IsNullable = true;
                colvarBatchNo.IsPrimaryKey = false;
                colvarBatchNo.IsForeignKey = false;
                colvarBatchNo.IsReadOnly = false;
                
                schema.Columns.Add(colvarBatchNo);
                
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
                
                
                BaseSchema = schema;
                //add this schema to the provider
                //so we can query it later
                DataService.Providers["PowerPOS"].AddSchema("ViewStockTake",schema);
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
	    public ViewStockTake()
	    {
            SetSQLProps();
            SetDefaults();
            MarkNew();
        }
        public ViewStockTake(bool useDatabaseDefaults)
	    {
		    SetSQLProps();
		    if(useDatabaseDefaults)
		    {
				ForceDefaults();
			}
			MarkNew();
	    }
	    
	    public ViewStockTake(object keyID)
	    {
		    SetSQLProps();
		    LoadByKey(keyID);
	    }
    	 
	    public ViewStockTake(string columnName, object columnValue)
        {
            SetSQLProps();
            LoadByParam(columnName,columnValue);
        }
        
	    #endregion
	    
	    #region Props
	    
          
        [XmlAttribute("StockTakeDate")]
        [Bindable(true)]
        public DateTime StockTakeDate 
	    {
		    get
		    {
			    return GetColumnValue<DateTime>("StockTakeDate");
		    }
            set 
		    {
			    SetColumnValue("StockTakeDate", value);
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
	      
        [XmlAttribute("StockTakeID")]
        [Bindable(true)]
        public int StockTakeID 
	    {
		    get
		    {
			    return GetColumnValue<int>("StockTakeID");
		    }
            set 
		    {
			    SetColumnValue("StockTakeID", value);
            }
        }
	      
        [XmlAttribute("InventoryLocationID")]
        [Bindable(true)]
        public int InventoryLocationID 
	    {
		    get
		    {
			    return GetColumnValue<int>("InventoryLocationID");
		    }
            set 
		    {
			    SetColumnValue("InventoryLocationID", value);
            }
        }
	      
        [XmlAttribute("StockTakeQty")]
        [Bindable(true)]
        public decimal? StockTakeQty 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("StockTakeQty");
		    }
            set 
		    {
			    SetColumnValue("StockTakeQty", value);
            }
        }
	      
        [XmlAttribute("TakenBy")]
        [Bindable(true)]
        public string TakenBy 
	    {
		    get
		    {
			    return GetColumnValue<string>("TakenBy");
		    }
            set 
		    {
			    SetColumnValue("TakenBy", value);
            }
        }
	      
        [XmlAttribute("VerifiedBy")]
        [Bindable(true)]
        public string VerifiedBy 
	    {
		    get
		    {
			    return GetColumnValue<string>("VerifiedBy");
		    }
            set 
		    {
			    SetColumnValue("VerifiedBy", value);
            }
        }
	      
        [XmlAttribute("AuthorizedBy")]
        [Bindable(true)]
        public string AuthorizedBy 
	    {
		    get
		    {
			    return GetColumnValue<string>("AuthorizedBy");
		    }
            set 
		    {
			    SetColumnValue("AuthorizedBy", value);
            }
        }
	      
        [XmlAttribute("IsAdjusted")]
        [Bindable(true)]
        public bool IsAdjusted 
	    {
		    get
		    {
			    return GetColumnValue<bool>("IsAdjusted");
		    }
            set 
		    {
			    SetColumnValue("IsAdjusted", value);
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
	      
        [XmlAttribute("CostOfGoods")]
        [Bindable(true)]
        public decimal CostOfGoods 
	    {
		    get
		    {
			    return GetColumnValue<decimal>("CostOfGoods");
		    }
            set 
		    {
			    SetColumnValue("CostOfGoods", value);
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
	      
        [XmlAttribute("InventoryLocationName")]
        [Bindable(true)]
        public string InventoryLocationName 
	    {
		    get
		    {
			    return GetColumnValue<string>("InventoryLocationName");
		    }
            set 
		    {
			    SetColumnValue("InventoryLocationName", value);
            }
        }
	      
        [XmlAttribute("AdjustmentHdrRefNo")]
        [Bindable(true)]
        public string AdjustmentHdrRefNo 
	    {
		    get
		    {
			    return GetColumnValue<string>("AdjustmentHdrRefNo");
		    }
            set 
		    {
			    SetColumnValue("AdjustmentHdrRefNo", value);
            }
        }
	      
        [XmlAttribute("BalQtyAtEntry")]
        [Bindable(true)]
        public decimal? BalQtyAtEntry 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("BalQtyAtEntry");
		    }
            set 
		    {
			    SetColumnValue("BalQtyAtEntry", value);
            }
        }
	      
        [XmlAttribute("AdjustmentQty")]
        [Bindable(true)]
        public decimal? AdjustmentQty 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("AdjustmentQty");
		    }
            set 
		    {
			    SetColumnValue("AdjustmentQty", value);
            }
        }
	      
        [XmlAttribute("Marked")]
        [Bindable(true)]
        public bool? Marked 
	    {
		    get
		    {
			    return GetColumnValue<bool?>("Marked");
		    }
            set 
		    {
			    SetColumnValue("Marked", value);
            }
        }
	      
        [XmlAttribute("BatchNo")]
        [Bindable(true)]
        public string BatchNo 
	    {
		    get
		    {
			    return GetColumnValue<string>("BatchNo");
		    }
            set 
		    {
			    SetColumnValue("BatchNo", value);
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
	    
	    #endregion
    
	    #region Columns Struct
	    public struct Columns
	    {
		    
		    
            public static string StockTakeDate = @"StockTakeDate";
            
            public static string ItemNo = @"ItemNo";
            
            public static string StockTakeID = @"StockTakeID";
            
            public static string InventoryLocationID = @"InventoryLocationID";
            
            public static string StockTakeQty = @"StockTakeQty";
            
            public static string TakenBy = @"TakenBy";
            
            public static string VerifiedBy = @"VerifiedBy";
            
            public static string AuthorizedBy = @"AuthorizedBy";
            
            public static string IsAdjusted = @"IsAdjusted";
            
            public static string Remark = @"Remark";
            
            public static string CostOfGoods = @"CostOfGoods";
            
            public static string ItemName = @"ItemName";
            
            public static string CategoryName = @"CategoryName";
            
            public static string IsInInventory = @"IsInInventory";
            
            public static string InventoryLocationName = @"InventoryLocationName";
            
            public static string AdjustmentHdrRefNo = @"AdjustmentHdrRefNo";
            
            public static string BalQtyAtEntry = @"BalQtyAtEntry";
            
            public static string AdjustmentQty = @"AdjustmentQty";
            
            public static string Marked = @"Marked";
            
            public static string BatchNo = @"BatchNo";
            
            public static string Deleted = @"Deleted";
            
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
