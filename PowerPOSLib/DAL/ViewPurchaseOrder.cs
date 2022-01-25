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
    /// Strongly-typed collection for the ViewPurchaseOrder class.
    /// </summary>
    [Serializable]
    public partial class ViewPurchaseOrderCollection : ReadOnlyList<ViewPurchaseOrder, ViewPurchaseOrderCollection>
    {        
        public ViewPurchaseOrderCollection() {}
    }
    /// <summary>
    /// This is  Read-only wrapper class for the ViewPurchaseOrder view.
    /// </summary>
    [Serializable]
    public partial class ViewPurchaseOrder : ReadOnlyRecord<ViewPurchaseOrder>, IReadOnlyRecord
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
                TableSchema.Table schema = new TableSchema.Table("ViewPurchaseOrder", TableType.View, DataService.GetInstance("PowerPOS"));
                schema.Columns = new TableSchema.TableColumnCollection();
                schema.SchemaName = @"dbo";
                //columns
                
                TableSchema.TableColumn colvarPurchaseOrderHeaderRefNo = new TableSchema.TableColumn(schema);
                colvarPurchaseOrderHeaderRefNo.ColumnName = "PurchaseOrderHeaderRefNo";
                colvarPurchaseOrderHeaderRefNo.DataType = DbType.AnsiString;
                colvarPurchaseOrderHeaderRefNo.MaxLength = 50;
                colvarPurchaseOrderHeaderRefNo.AutoIncrement = false;
                colvarPurchaseOrderHeaderRefNo.IsNullable = false;
                colvarPurchaseOrderHeaderRefNo.IsPrimaryKey = false;
                colvarPurchaseOrderHeaderRefNo.IsForeignKey = false;
                colvarPurchaseOrderHeaderRefNo.IsReadOnly = false;
                
                schema.Columns.Add(colvarPurchaseOrderHeaderRefNo);
                
                TableSchema.TableColumn colvarPurchaseOrderDate = new TableSchema.TableColumn(schema);
                colvarPurchaseOrderDate.ColumnName = "PurchaseOrderDate";
                colvarPurchaseOrderDate.DataType = DbType.DateTime;
                colvarPurchaseOrderDate.MaxLength = 0;
                colvarPurchaseOrderDate.AutoIncrement = false;
                colvarPurchaseOrderDate.IsNullable = false;
                colvarPurchaseOrderDate.IsPrimaryKey = false;
                colvarPurchaseOrderDate.IsForeignKey = false;
                colvarPurchaseOrderDate.IsReadOnly = false;
                
                schema.Columns.Add(colvarPurchaseOrderDate);
                
                TableSchema.TableColumn colvarUserName = new TableSchema.TableColumn(schema);
                colvarUserName.ColumnName = "UserName";
                colvarUserName.DataType = DbType.AnsiString;
                colvarUserName.MaxLength = 50;
                colvarUserName.AutoIncrement = false;
                colvarUserName.IsNullable = true;
                colvarUserName.IsPrimaryKey = false;
                colvarUserName.IsForeignKey = false;
                colvarUserName.IsReadOnly = false;
                
                schema.Columns.Add(colvarUserName);
                
                TableSchema.TableColumn colvarInventoryLocationName = new TableSchema.TableColumn(schema);
                colvarInventoryLocationName.ColumnName = "InventoryLocationName";
                colvarInventoryLocationName.DataType = DbType.AnsiString;
                colvarInventoryLocationName.MaxLength = 50;
                colvarInventoryLocationName.AutoIncrement = false;
                colvarInventoryLocationName.IsNullable = true;
                colvarInventoryLocationName.IsPrimaryKey = false;
                colvarInventoryLocationName.IsForeignKey = false;
                colvarInventoryLocationName.IsReadOnly = false;
                
                schema.Columns.Add(colvarInventoryLocationName);
                
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
                
                TableSchema.TableColumn colvarPrice = new TableSchema.TableColumn(schema);
                colvarPrice.ColumnName = "Price";
                colvarPrice.DataType = DbType.Currency;
                colvarPrice.MaxLength = 0;
                colvarPrice.AutoIncrement = false;
                colvarPrice.IsNullable = false;
                colvarPrice.IsPrimaryKey = false;
                colvarPrice.IsForeignKey = false;
                colvarPrice.IsReadOnly = false;
                
                schema.Columns.Add(colvarPrice);
                
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
                colvarInventoryLocationID.IsNullable = true;
                colvarInventoryLocationID.IsPrimaryKey = false;
                colvarInventoryLocationID.IsForeignKey = false;
                colvarInventoryLocationID.IsReadOnly = false;
                
                schema.Columns.Add(colvarInventoryLocationID);
                
                TableSchema.TableColumn colvarPurchaseOrderDetailRefNo = new TableSchema.TableColumn(schema);
                colvarPurchaseOrderDetailRefNo.ColumnName = "PurchaseOrderDetailRefNo";
                colvarPurchaseOrderDetailRefNo.DataType = DbType.AnsiString;
                colvarPurchaseOrderDetailRefNo.MaxLength = 50;
                colvarPurchaseOrderDetailRefNo.AutoIncrement = false;
                colvarPurchaseOrderDetailRefNo.IsNullable = false;
                colvarPurchaseOrderDetailRefNo.IsPrimaryKey = false;
                colvarPurchaseOrderDetailRefNo.IsForeignKey = false;
                colvarPurchaseOrderDetailRefNo.IsReadOnly = false;
                
                schema.Columns.Add(colvarPurchaseOrderDetailRefNo);
                
                
                BaseSchema = schema;
                //add this schema to the provider
                //so we can query it later
                DataService.Providers["PowerPOS"].AddSchema("ViewPurchaseOrder",schema);
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
	    public ViewPurchaseOrder()
	    {
            SetSQLProps();
            SetDefaults();
            MarkNew();
        }
        public ViewPurchaseOrder(bool useDatabaseDefaults)
	    {
		    SetSQLProps();
		    if(useDatabaseDefaults)
		    {
				ForceDefaults();
			}
			MarkNew();
	    }
	    
	    public ViewPurchaseOrder(object keyID)
	    {
		    SetSQLProps();
		    LoadByKey(keyID);
	    }
    	 
	    public ViewPurchaseOrder(string columnName, object columnValue)
        {
            SetSQLProps();
            LoadByParam(columnName,columnValue);
        }
        
	    #endregion
	    
	    #region Props
	    
          
        [XmlAttribute("PurchaseOrderHeaderRefNo")]
        [Bindable(true)]
        public string PurchaseOrderHeaderRefNo 
	    {
		    get
		    {
			    return GetColumnValue<string>("PurchaseOrderHeaderRefNo");
		    }
            set 
		    {
			    SetColumnValue("PurchaseOrderHeaderRefNo", value);
            }
        }
	      
        [XmlAttribute("PurchaseOrderDate")]
        [Bindable(true)]
        public DateTime PurchaseOrderDate 
	    {
		    get
		    {
			    return GetColumnValue<DateTime>("PurchaseOrderDate");
		    }
            set 
		    {
			    SetColumnValue("PurchaseOrderDate", value);
            }
        }
	      
        [XmlAttribute("UserName")]
        [Bindable(true)]
        public string UserName 
	    {
		    get
		    {
			    return GetColumnValue<string>("UserName");
		    }
            set 
		    {
			    SetColumnValue("UserName", value);
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
	      
        [XmlAttribute("Price")]
        [Bindable(true)]
        public decimal Price 
	    {
		    get
		    {
			    return GetColumnValue<decimal>("Price");
		    }
            set 
		    {
			    SetColumnValue("Price", value);
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
        public int? InventoryLocationID 
	    {
		    get
		    {
			    return GetColumnValue<int?>("InventoryLocationID");
		    }
            set 
		    {
			    SetColumnValue("InventoryLocationID", value);
            }
        }
	      
        [XmlAttribute("PurchaseOrderDetailRefNo")]
        [Bindable(true)]
        public string PurchaseOrderDetailRefNo 
	    {
		    get
		    {
			    return GetColumnValue<string>("PurchaseOrderDetailRefNo");
		    }
            set 
		    {
			    SetColumnValue("PurchaseOrderDetailRefNo", value);
            }
        }
	    
	    #endregion
    
	    #region Columns Struct
	    public struct Columns
	    {
		    
		    
            public static string PurchaseOrderHeaderRefNo = @"PurchaseOrderHeaderRefNo";
            
            public static string PurchaseOrderDate = @"PurchaseOrderDate";
            
            public static string UserName = @"UserName";
            
            public static string InventoryLocationName = @"InventoryLocationName";
            
            public static string ItemNo = @"ItemNo";
            
            public static string ItemName = @"ItemName";
            
            public static string Quantity = @"Quantity";
            
            public static string Price = @"Price";
            
            public static string CategoryName = @"CategoryName";
            
            public static string InventoryLocationID = @"InventoryLocationID";
            
            public static string PurchaseOrderDetailRefNo = @"PurchaseOrderDetailRefNo";
            
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
