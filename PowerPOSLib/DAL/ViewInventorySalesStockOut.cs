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
    /// Strongly-typed collection for the ViewInventorySalesStockOut class.
    /// </summary>
    [Serializable]
    public partial class ViewInventorySalesStockOutCollection : ReadOnlyList<ViewInventorySalesStockOut, ViewInventorySalesStockOutCollection>
    {        
        public ViewInventorySalesStockOutCollection() {}
    }
    /// <summary>
    /// This is  Read-only wrapper class for the ViewInventorySalesStockOut view.
    /// </summary>
    [Serializable]
    public partial class ViewInventorySalesStockOut : ReadOnlyRecord<ViewInventorySalesStockOut>, IReadOnlyRecord
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
                TableSchema.Table schema = new TableSchema.Table("ViewInventorySalesStockOut", TableType.View, DataService.GetInstance("PowerPOS"));
                schema.Columns = new TableSchema.TableColumnCollection();
                schema.SchemaName = @"dbo";
                //columns
                
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
                
                TableSchema.TableColumn colvarCostofgoods = new TableSchema.TableColumn(schema);
                colvarCostofgoods.ColumnName = "costofgoods";
                colvarCostofgoods.DataType = DbType.Currency;
                colvarCostofgoods.MaxLength = 0;
                colvarCostofgoods.AutoIncrement = false;
                colvarCostofgoods.IsNullable = true;
                colvarCostofgoods.IsPrimaryKey = false;
                colvarCostofgoods.IsForeignKey = false;
                colvarCostofgoods.IsReadOnly = false;
                
                schema.Columns.Add(colvarCostofgoods);
                
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
                
                
                BaseSchema = schema;
                //add this schema to the provider
                //so we can query it later
                DataService.Providers["PowerPOS"].AddSchema("ViewInventorySalesStockOut",schema);
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
	    public ViewInventorySalesStockOut()
	    {
            SetSQLProps();
            SetDefaults();
            MarkNew();
        }
        public ViewInventorySalesStockOut(bool useDatabaseDefaults)
	    {
		    SetSQLProps();
		    if(useDatabaseDefaults)
		    {
				ForceDefaults();
			}
			MarkNew();
	    }
	    
	    public ViewInventorySalesStockOut(object keyID)
	    {
		    SetSQLProps();
		    LoadByKey(keyID);
	    }
    	 
	    public ViewInventorySalesStockOut(string columnName, object columnValue)
        {
            SetSQLProps();
            LoadByParam(columnName,columnValue);
        }
        
	    #endregion
	    
	    #region Props
	    
          
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
	      
        [XmlAttribute("Costofgoods")]
        [Bindable(true)]
        public decimal? Costofgoods 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("costofgoods");
		    }
            set 
		    {
			    SetColumnValue("costofgoods", value);
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
	    
	    #endregion
    
	    #region Columns Struct
	    public struct Columns
	    {
		    
		    
            public static string Quantity = @"Quantity";
            
            public static string Costofgoods = @"costofgoods";
            
            public static string ItemNo = @"ItemNo";
            
            public static string InventoryHdrRefNo = @"InventoryHdrRefNo";
            
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
