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
    /// Strongly-typed collection for the ViewInventoryActivityByItemNo class.
    /// </summary>
    [Serializable]
    public partial class ViewInventoryActivityByItemNoCollection : ReadOnlyList<ViewInventoryActivityByItemNo, ViewInventoryActivityByItemNoCollection>
    {        
        public ViewInventoryActivityByItemNoCollection() {}
    }
    /// <summary>
    /// This is  Read-only wrapper class for the ViewInventoryActivityByItemNo view.
    /// </summary>
    [Serializable]
    public partial class ViewInventoryActivityByItemNo : ReadOnlyRecord<ViewInventoryActivityByItemNo>, IReadOnlyRecord
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
                TableSchema.Table schema = new TableSchema.Table("ViewInventoryActivityByItemNo", TableType.View, DataService.GetInstance("PowerPOS"));
                schema.Columns = new TableSchema.TableColumnCollection();
                schema.SchemaName = @"dbo";
                //columns
                
                TableSchema.TableColumn colvarInventoryHdrRefNo = new TableSchema.TableColumn(schema);
                colvarInventoryHdrRefNo.ColumnName = "InventoryHdrRefNo";
                colvarInventoryHdrRefNo.DataType = DbType.AnsiString;
                colvarInventoryHdrRefNo.MaxLength = 50;
                colvarInventoryHdrRefNo.AutoIncrement = false;
                colvarInventoryHdrRefNo.IsNullable = false;
                colvarInventoryHdrRefNo.IsPrimaryKey = false;
                colvarInventoryHdrRefNo.IsForeignKey = false;
                colvarInventoryHdrRefNo.IsReadOnly = false;
                
                schema.Columns.Add(colvarInventoryHdrRefNo);
                
                TableSchema.TableColumn colvarInventoryDate = new TableSchema.TableColumn(schema);
                colvarInventoryDate.ColumnName = "InventoryDate";
                colvarInventoryDate.DataType = DbType.DateTime;
                colvarInventoryDate.MaxLength = 0;
                colvarInventoryDate.AutoIncrement = false;
                colvarInventoryDate.IsNullable = false;
                colvarInventoryDate.IsPrimaryKey = false;
                colvarInventoryDate.IsForeignKey = false;
                colvarInventoryDate.IsReadOnly = false;
                
                schema.Columns.Add(colvarInventoryDate);
                
                TableSchema.TableColumn colvarMovementType = new TableSchema.TableColumn(schema);
                colvarMovementType.ColumnName = "MovementType";
                colvarMovementType.DataType = DbType.AnsiString;
                colvarMovementType.MaxLength = 50;
                colvarMovementType.AutoIncrement = false;
                colvarMovementType.IsNullable = true;
                colvarMovementType.IsPrimaryKey = false;
                colvarMovementType.IsForeignKey = false;
                colvarMovementType.IsReadOnly = false;
                
                schema.Columns.Add(colvarMovementType);
                
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
                
                TableSchema.TableColumn colvarQuantity = new TableSchema.TableColumn(schema);
                colvarQuantity.ColumnName = "Quantity";
                colvarQuantity.DataType = DbType.Int32;
                colvarQuantity.MaxLength = 0;
                colvarQuantity.AutoIncrement = false;
                colvarQuantity.IsNullable = true;
                colvarQuantity.IsPrimaryKey = false;
                colvarQuantity.IsForeignKey = false;
                colvarQuantity.IsReadOnly = false;
                
                schema.Columns.Add(colvarQuantity);
                
                TableSchema.TableColumn colvarCostOfGoods = new TableSchema.TableColumn(schema);
                colvarCostOfGoods.ColumnName = "CostOfGoods";
                colvarCostOfGoods.DataType = DbType.Currency;
                colvarCostOfGoods.MaxLength = 0;
                colvarCostOfGoods.AutoIncrement = false;
                colvarCostOfGoods.IsNullable = true;
                colvarCostOfGoods.IsPrimaryKey = false;
                colvarCostOfGoods.IsForeignKey = false;
                colvarCostOfGoods.IsReadOnly = false;
                
                schema.Columns.Add(colvarCostOfGoods);
                
                TableSchema.TableColumn colvarStockOutReasonID = new TableSchema.TableColumn(schema);
                colvarStockOutReasonID.ColumnName = "StockOutReasonID";
                colvarStockOutReasonID.DataType = DbType.Int32;
                colvarStockOutReasonID.MaxLength = 0;
                colvarStockOutReasonID.AutoIncrement = false;
                colvarStockOutReasonID.IsNullable = true;
                colvarStockOutReasonID.IsPrimaryKey = false;
                colvarStockOutReasonID.IsForeignKey = false;
                colvarStockOutReasonID.IsReadOnly = false;
                
                schema.Columns.Add(colvarStockOutReasonID);
                
                
                BaseSchema = schema;
                //add this schema to the provider
                //so we can query it later
                DataService.Providers["PowerPOS"].AddSchema("ViewInventoryActivityByItemNo",schema);
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
	    public ViewInventoryActivityByItemNo()
	    {
            SetSQLProps();
            SetDefaults();
            MarkNew();
        }
        public ViewInventoryActivityByItemNo(bool useDatabaseDefaults)
	    {
		    SetSQLProps();
		    if(useDatabaseDefaults)
		    {
				ForceDefaults();
			}
			MarkNew();
	    }
	    
	    public ViewInventoryActivityByItemNo(object keyID)
	    {
		    SetSQLProps();
		    LoadByKey(keyID);
	    }
    	 
	    public ViewInventoryActivityByItemNo(string columnName, object columnValue)
        {
            SetSQLProps();
            LoadByParam(columnName,columnValue);
        }
        
	    #endregion
	    
	    #region Props
	    
          
        [XmlAttribute("InventoryHdrRefNo")]
        [Bindable(true)]
        public string InventoryHdrRefNo 
	    {
		    get
		    {
			    return GetColumnValue<string>("InventoryHdrRefNo");
		    }
            set 
		    {
			    SetColumnValue("InventoryHdrRefNo", value);
            }
        }
	      
        [XmlAttribute("InventoryDate")]
        [Bindable(true)]
        public DateTime InventoryDate 
	    {
		    get
		    {
			    return GetColumnValue<DateTime>("InventoryDate");
		    }
            set 
		    {
			    SetColumnValue("InventoryDate", value);
            }
        }
	      
        [XmlAttribute("MovementType")]
        [Bindable(true)]
        public string MovementType 
	    {
		    get
		    {
			    return GetColumnValue<string>("MovementType");
		    }
            set 
		    {
			    SetColumnValue("MovementType", value);
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
	      
        [XmlAttribute("Quantity")]
        [Bindable(true)]
        public int? Quantity 
	    {
		    get
		    {
			    return GetColumnValue<int?>("Quantity");
		    }
            set 
		    {
			    SetColumnValue("Quantity", value);
            }
        }
	      
        [XmlAttribute("CostOfGoods")]
        [Bindable(true)]
        public decimal? CostOfGoods 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("CostOfGoods");
		    }
            set 
		    {
			    SetColumnValue("CostOfGoods", value);
            }
        }
	      
        [XmlAttribute("StockOutReasonID")]
        [Bindable(true)]
        public int? StockOutReasonID 
	    {
		    get
		    {
			    return GetColumnValue<int?>("StockOutReasonID");
		    }
            set 
		    {
			    SetColumnValue("StockOutReasonID", value);
            }
        }
	    
	    #endregion
    
	    #region Columns Struct
	    public struct Columns
	    {
		    
		    
            public static string InventoryHdrRefNo = @"InventoryHdrRefNo";
            
            public static string InventoryDate = @"InventoryDate";
            
            public static string MovementType = @"MovementType";
            
            public static string InventoryLocationID = @"InventoryLocationID";
            
            public static string ItemNo = @"ItemNo";
            
            public static string Quantity = @"Quantity";
            
            public static string CostOfGoods = @"CostOfGoods";
            
            public static string StockOutReasonID = @"StockOutReasonID";
            
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
