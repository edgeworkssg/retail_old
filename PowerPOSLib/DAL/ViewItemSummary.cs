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
    /// Strongly-typed collection for the ViewItemSummary class.
    /// </summary>
    [Serializable]
    public partial class ViewItemSummaryCollection : ReadOnlyList<ViewItemSummary, ViewItemSummaryCollection>
    {        
        public ViewItemSummaryCollection() {}
    }
    /// <summary>
    /// This is  Read-only wrapper class for the ViewItemSummary view.
    /// </summary>
    [Serializable]
    public partial class ViewItemSummary : ReadOnlyRecord<ViewItemSummary>, IReadOnlyRecord
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
                TableSchema.Table schema = new TableSchema.Table("ViewItemSummary", TableType.View, DataService.GetInstance("PowerPOS"));
                schema.Columns = new TableSchema.TableColumnCollection();
                schema.SchemaName = @"dbo";
                //columns
                
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
                
                TableSchema.TableColumn colvarInventoryLocationGroupID = new TableSchema.TableColumn(schema);
                colvarInventoryLocationGroupID.ColumnName = "InventoryLocationGroupID";
                colvarInventoryLocationGroupID.DataType = DbType.Int32;
                colvarInventoryLocationGroupID.MaxLength = 0;
                colvarInventoryLocationGroupID.AutoIncrement = false;
                colvarInventoryLocationGroupID.IsNullable = false;
                colvarInventoryLocationGroupID.IsPrimaryKey = false;
                colvarInventoryLocationGroupID.IsForeignKey = false;
                colvarInventoryLocationGroupID.IsReadOnly = false;
                
                schema.Columns.Add(colvarInventoryLocationGroupID);
                
                TableSchema.TableColumn colvarInventoryLocationGroupName = new TableSchema.TableColumn(schema);
                colvarInventoryLocationGroupName.ColumnName = "InventoryLocationGroupName";
                colvarInventoryLocationGroupName.DataType = DbType.AnsiString;
                colvarInventoryLocationGroupName.MaxLength = 50;
                colvarInventoryLocationGroupName.AutoIncrement = false;
                colvarInventoryLocationGroupName.IsNullable = false;
                colvarInventoryLocationGroupName.IsPrimaryKey = false;
                colvarInventoryLocationGroupName.IsForeignKey = false;
                colvarInventoryLocationGroupName.IsReadOnly = false;
                
                schema.Columns.Add(colvarInventoryLocationGroupName);
                
                TableSchema.TableColumn colvarItemBalanceQty = new TableSchema.TableColumn(schema);
                colvarItemBalanceQty.ColumnName = "ItemBalanceQty";
                colvarItemBalanceQty.DataType = DbType.Double;
                colvarItemBalanceQty.MaxLength = 0;
                colvarItemBalanceQty.AutoIncrement = false;
                colvarItemBalanceQty.IsNullable = false;
                colvarItemBalanceQty.IsPrimaryKey = false;
                colvarItemBalanceQty.IsForeignKey = false;
                colvarItemBalanceQty.IsReadOnly = false;
                
                schema.Columns.Add(colvarItemBalanceQty);
                
                TableSchema.TableColumn colvarItemCostPrice = new TableSchema.TableColumn(schema);
                colvarItemCostPrice.ColumnName = "ItemCostPrice";
                colvarItemCostPrice.DataType = DbType.Currency;
                colvarItemCostPrice.MaxLength = 0;
                colvarItemCostPrice.AutoIncrement = false;
                colvarItemCostPrice.IsNullable = false;
                colvarItemCostPrice.IsPrimaryKey = false;
                colvarItemCostPrice.IsForeignKey = false;
                colvarItemCostPrice.IsReadOnly = false;
                
                schema.Columns.Add(colvarItemCostPrice);
                
                TableSchema.TableColumn colvarLocBalanceQty = new TableSchema.TableColumn(schema);
                colvarLocBalanceQty.ColumnName = "LocBalanceQty";
                colvarLocBalanceQty.DataType = DbType.Double;
                colvarLocBalanceQty.MaxLength = 0;
                colvarLocBalanceQty.AutoIncrement = false;
                colvarLocBalanceQty.IsNullable = false;
                colvarLocBalanceQty.IsPrimaryKey = false;
                colvarLocBalanceQty.IsForeignKey = false;
                colvarLocBalanceQty.IsReadOnly = false;
                
                schema.Columns.Add(colvarLocBalanceQty);
                
                TableSchema.TableColumn colvarLocCostPrice = new TableSchema.TableColumn(schema);
                colvarLocCostPrice.ColumnName = "LocCostPrice";
                colvarLocCostPrice.DataType = DbType.Currency;
                colvarLocCostPrice.MaxLength = 0;
                colvarLocCostPrice.AutoIncrement = false;
                colvarLocCostPrice.IsNullable = false;
                colvarLocCostPrice.IsPrimaryKey = false;
                colvarLocCostPrice.IsForeignKey = false;
                colvarLocCostPrice.IsReadOnly = false;
                
                schema.Columns.Add(colvarLocCostPrice);
                
                TableSchema.TableColumn colvarLocGroupBalanceQty = new TableSchema.TableColumn(schema);
                colvarLocGroupBalanceQty.ColumnName = "LocGroupBalanceQty";
                colvarLocGroupBalanceQty.DataType = DbType.Double;
                colvarLocGroupBalanceQty.MaxLength = 0;
                colvarLocGroupBalanceQty.AutoIncrement = false;
                colvarLocGroupBalanceQty.IsNullable = false;
                colvarLocGroupBalanceQty.IsPrimaryKey = false;
                colvarLocGroupBalanceQty.IsForeignKey = false;
                colvarLocGroupBalanceQty.IsReadOnly = false;
                
                schema.Columns.Add(colvarLocGroupBalanceQty);
                
                TableSchema.TableColumn colvarLocGroupCostPrice = new TableSchema.TableColumn(schema);
                colvarLocGroupCostPrice.ColumnName = "LocGroupCostPrice";
                colvarLocGroupCostPrice.DataType = DbType.Currency;
                colvarLocGroupCostPrice.MaxLength = 0;
                colvarLocGroupCostPrice.AutoIncrement = false;
                colvarLocGroupCostPrice.IsNullable = false;
                colvarLocGroupCostPrice.IsPrimaryKey = false;
                colvarLocGroupCostPrice.IsForeignKey = false;
                colvarLocGroupCostPrice.IsReadOnly = false;
                
                schema.Columns.Add(colvarLocGroupCostPrice);
                
                
                BaseSchema = schema;
                //add this schema to the provider
                //so we can query it later
                DataService.Providers["PowerPOS"].AddSchema("ViewItemSummary",schema);
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
	    public ViewItemSummary()
	    {
            SetSQLProps();
            SetDefaults();
            MarkNew();
        }
        public ViewItemSummary(bool useDatabaseDefaults)
	    {
		    SetSQLProps();
		    if(useDatabaseDefaults)
		    {
				ForceDefaults();
			}
			MarkNew();
	    }
	    
	    public ViewItemSummary(object keyID)
	    {
		    SetSQLProps();
		    LoadByKey(keyID);
	    }
    	 
	    public ViewItemSummary(string columnName, object columnValue)
        {
            SetSQLProps();
            LoadByParam(columnName,columnValue);
        }
        
	    #endregion
	    
	    #region Props
	    
          
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
	      
        [XmlAttribute("InventoryLocationGroupID")]
        [Bindable(true)]
        public int InventoryLocationGroupID 
	    {
		    get
		    {
			    return GetColumnValue<int>("InventoryLocationGroupID");
		    }
            set 
		    {
			    SetColumnValue("InventoryLocationGroupID", value);
            }
        }
	      
        [XmlAttribute("InventoryLocationGroupName")]
        [Bindable(true)]
        public string InventoryLocationGroupName 
	    {
		    get
		    {
			    return GetColumnValue<string>("InventoryLocationGroupName");
		    }
            set 
		    {
			    SetColumnValue("InventoryLocationGroupName", value);
            }
        }
	      
        [XmlAttribute("ItemBalanceQty")]
        [Bindable(true)]
        public double ItemBalanceQty 
	    {
		    get
		    {
			    return GetColumnValue<double>("ItemBalanceQty");
		    }
            set 
		    {
			    SetColumnValue("ItemBalanceQty", value);
            }
        }
	      
        [XmlAttribute("ItemCostPrice")]
        [Bindable(true)]
        public decimal ItemCostPrice 
	    {
		    get
		    {
			    return GetColumnValue<decimal>("ItemCostPrice");
		    }
            set 
		    {
			    SetColumnValue("ItemCostPrice", value);
            }
        }
	      
        [XmlAttribute("LocBalanceQty")]
        [Bindable(true)]
        public double LocBalanceQty 
	    {
		    get
		    {
			    return GetColumnValue<double>("LocBalanceQty");
		    }
            set 
		    {
			    SetColumnValue("LocBalanceQty", value);
            }
        }
	      
        [XmlAttribute("LocCostPrice")]
        [Bindable(true)]
        public decimal LocCostPrice 
	    {
		    get
		    {
			    return GetColumnValue<decimal>("LocCostPrice");
		    }
            set 
		    {
			    SetColumnValue("LocCostPrice", value);
            }
        }
	      
        [XmlAttribute("LocGroupBalanceQty")]
        [Bindable(true)]
        public double LocGroupBalanceQty 
	    {
		    get
		    {
			    return GetColumnValue<double>("LocGroupBalanceQty");
		    }
            set 
		    {
			    SetColumnValue("LocGroupBalanceQty", value);
            }
        }
	      
        [XmlAttribute("LocGroupCostPrice")]
        [Bindable(true)]
        public decimal LocGroupCostPrice 
	    {
		    get
		    {
			    return GetColumnValue<decimal>("LocGroupCostPrice");
		    }
            set 
		    {
			    SetColumnValue("LocGroupCostPrice", value);
            }
        }
	    
	    #endregion
    
	    #region Columns Struct
	    public struct Columns
	    {
		    
		    
            public static string ItemNo = @"ItemNo";
            
            public static string ItemName = @"ItemName";
            
            public static string CategoryName = @"CategoryName";
            
            public static string InventoryLocationID = @"InventoryLocationID";
            
            public static string InventoryLocationName = @"InventoryLocationName";
            
            public static string InventoryLocationGroupID = @"InventoryLocationGroupID";
            
            public static string InventoryLocationGroupName = @"InventoryLocationGroupName";
            
            public static string ItemBalanceQty = @"ItemBalanceQty";
            
            public static string ItemCostPrice = @"ItemCostPrice";
            
            public static string LocBalanceQty = @"LocBalanceQty";
            
            public static string LocCostPrice = @"LocCostPrice";
            
            public static string LocGroupBalanceQty = @"LocGroupBalanceQty";
            
            public static string LocGroupCostPrice = @"LocGroupCostPrice";
            
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
