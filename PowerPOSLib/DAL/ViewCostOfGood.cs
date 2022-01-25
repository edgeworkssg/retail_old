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
    /// Strongly-typed collection for the ViewCostOfGood class.
    /// </summary>
    [Serializable]
    public partial class ViewCostOfGoodCollection : ReadOnlyList<ViewCostOfGood, ViewCostOfGoodCollection>
    {        
        public ViewCostOfGoodCollection() {}
    }
    /// <summary>
    /// This is  Read-only wrapper class for the ViewCostOfGoods view.
    /// </summary>
    [Serializable]
    public partial class ViewCostOfGood : ReadOnlyRecord<ViewCostOfGood>, IReadOnlyRecord
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
                TableSchema.Table schema = new TableSchema.Table("ViewCostOfGoods", TableType.View, DataService.GetInstance("PowerPOS"));
                schema.Columns = new TableSchema.TableColumnCollection();
                schema.SchemaName = @"dbo";
                //columns
                
                TableSchema.TableColumn colvarCostWithQty = new TableSchema.TableColumn(schema);
                colvarCostWithQty.ColumnName = "CostWithQty";
                colvarCostWithQty.DataType = DbType.Currency;
                colvarCostWithQty.MaxLength = 0;
                colvarCostWithQty.AutoIncrement = false;
                colvarCostWithQty.IsNullable = true;
                colvarCostWithQty.IsPrimaryKey = false;
                colvarCostWithQty.IsForeignKey = false;
                colvarCostWithQty.IsReadOnly = false;
                
                schema.Columns.Add(colvarCostWithQty);
                
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
                
                TableSchema.TableColumn colvarQuantity = new TableSchema.TableColumn(schema);
                colvarQuantity.ColumnName = "Quantity";
                colvarQuantity.DataType = DbType.Int32;
                colvarQuantity.MaxLength = 0;
                colvarQuantity.AutoIncrement = false;
                colvarQuantity.IsNullable = false;
                colvarQuantity.IsPrimaryKey = false;
                colvarQuantity.IsForeignKey = false;
                colvarQuantity.IsReadOnly = false;
                
                schema.Columns.Add(colvarQuantity);
                
                TableSchema.TableColumn colvarInventoryMonth = new TableSchema.TableColumn(schema);
                colvarInventoryMonth.ColumnName = "InventoryMonth";
                colvarInventoryMonth.DataType = DbType.Int32;
                colvarInventoryMonth.MaxLength = 0;
                colvarInventoryMonth.AutoIncrement = false;
                colvarInventoryMonth.IsNullable = true;
                colvarInventoryMonth.IsPrimaryKey = false;
                colvarInventoryMonth.IsForeignKey = false;
                colvarInventoryMonth.IsReadOnly = false;
                
                schema.Columns.Add(colvarInventoryMonth);
                
                TableSchema.TableColumn colvarInventoryLocationId = new TableSchema.TableColumn(schema);
                colvarInventoryLocationId.ColumnName = "InventoryLocationId";
                colvarInventoryLocationId.DataType = DbType.Int32;
                colvarInventoryLocationId.MaxLength = 0;
                colvarInventoryLocationId.AutoIncrement = false;
                colvarInventoryLocationId.IsNullable = true;
                colvarInventoryLocationId.IsPrimaryKey = false;
                colvarInventoryLocationId.IsForeignKey = false;
                colvarInventoryLocationId.IsReadOnly = false;
                
                schema.Columns.Add(colvarInventoryLocationId);
                
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
                
                
                BaseSchema = schema;
                //add this schema to the provider
                //so we can query it later
                DataService.Providers["PowerPOS"].AddSchema("ViewCostOfGoods",schema);
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
	    public ViewCostOfGood()
	    {
            SetSQLProps();
            SetDefaults();
            MarkNew();
        }
        public ViewCostOfGood(bool useDatabaseDefaults)
	    {
		    SetSQLProps();
		    if(useDatabaseDefaults)
		    {
				ForceDefaults();
			}
			MarkNew();
	    }
	    
	    public ViewCostOfGood(object keyID)
	    {
		    SetSQLProps();
		    LoadByKey(keyID);
	    }
    	 
	    public ViewCostOfGood(string columnName, object columnValue)
        {
            SetSQLProps();
            LoadByParam(columnName,columnValue);
        }
        
	    #endregion
	    
	    #region Props
	    
          
        [XmlAttribute("CostWithQty")]
        [Bindable(true)]
        public decimal? CostWithQty 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("CostWithQty");
		    }
            set 
		    {
			    SetColumnValue("CostWithQty", value);
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
	      
        [XmlAttribute("Quantity")]
        [Bindable(true)]
        public int Quantity 
	    {
		    get
		    {
			    return GetColumnValue<int>("Quantity");
		    }
            set 
		    {
			    SetColumnValue("Quantity", value);
            }
        }
	      
        [XmlAttribute("InventoryMonth")]
        [Bindable(true)]
        public int? InventoryMonth 
	    {
		    get
		    {
			    return GetColumnValue<int?>("InventoryMonth");
		    }
            set 
		    {
			    SetColumnValue("InventoryMonth", value);
            }
        }
	      
        [XmlAttribute("InventoryLocationId")]
        [Bindable(true)]
        public int? InventoryLocationId 
	    {
		    get
		    {
			    return GetColumnValue<int?>("InventoryLocationId");
		    }
            set 
		    {
			    SetColumnValue("InventoryLocationId", value);
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
	    
	    #endregion
    
	    #region Columns Struct
	    public struct Columns
	    {
		    
		    
            public static string CostWithQty = @"CostWithQty";
            
            public static string CostOfGoods = @"CostOfGoods";
            
            public static string Quantity = @"Quantity";
            
            public static string InventoryMonth = @"InventoryMonth";
            
            public static string InventoryLocationId = @"InventoryLocationId";
            
            public static string ItemNo = @"ItemNo";
            
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
