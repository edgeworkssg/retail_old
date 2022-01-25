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
    /// Strongly-typed collection for the ViewInventoryTransferDetail class.
    /// </summary>
    [Serializable]
    public partial class ViewInventoryTransferDetailCollection : ReadOnlyList<ViewInventoryTransferDetail, ViewInventoryTransferDetailCollection>
    {        
        public ViewInventoryTransferDetailCollection() {}
    }
    /// <summary>
    /// This is  Read-only wrapper class for the ViewInventoryTransferDetail view.
    /// </summary>
    [Serializable]
    public partial class ViewInventoryTransferDetail : ReadOnlyRecord<ViewInventoryTransferDetail>, IReadOnlyRecord
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
                TableSchema.Table schema = new TableSchema.Table("ViewInventoryTransferDetail", TableType.View, DataService.GetInstance("PowerPOS"));
                schema.Columns = new TableSchema.TableColumnCollection();
                schema.SchemaName = @"dbo";
                //columns
                
                TableSchema.TableColumn colvarTransferQty = new TableSchema.TableColumn(schema);
                colvarTransferQty.ColumnName = "TransferQty";
                colvarTransferQty.DataType = DbType.Int32;
                colvarTransferQty.MaxLength = 0;
                colvarTransferQty.AutoIncrement = false;
                colvarTransferQty.IsNullable = true;
                colvarTransferQty.IsPrimaryKey = false;
                colvarTransferQty.IsForeignKey = false;
                colvarTransferQty.IsReadOnly = false;
                
                schema.Columns.Add(colvarTransferQty);
                
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
                
                TableSchema.TableColumn colvarLocationTransferID = new TableSchema.TableColumn(schema);
                colvarLocationTransferID.ColumnName = "LocationTransferID";
                colvarLocationTransferID.DataType = DbType.Int32;
                colvarLocationTransferID.MaxLength = 0;
                colvarLocationTransferID.AutoIncrement = false;
                colvarLocationTransferID.IsNullable = false;
                colvarLocationTransferID.IsPrimaryKey = false;
                colvarLocationTransferID.IsForeignKey = false;
                colvarLocationTransferID.IsReadOnly = false;
                
                schema.Columns.Add(colvarLocationTransferID);
                
                TableSchema.TableColumn colvarFromInventoryLocationID = new TableSchema.TableColumn(schema);
                colvarFromInventoryLocationID.ColumnName = "FromInventoryLocationID";
                colvarFromInventoryLocationID.DataType = DbType.Int32;
                colvarFromInventoryLocationID.MaxLength = 0;
                colvarFromInventoryLocationID.AutoIncrement = false;
                colvarFromInventoryLocationID.IsNullable = true;
                colvarFromInventoryLocationID.IsPrimaryKey = false;
                colvarFromInventoryLocationID.IsForeignKey = false;
                colvarFromInventoryLocationID.IsReadOnly = false;
                
                schema.Columns.Add(colvarFromInventoryLocationID);
                
                TableSchema.TableColumn colvarFromInventoryLocationName = new TableSchema.TableColumn(schema);
                colvarFromInventoryLocationName.ColumnName = "FromInventoryLocationName";
                colvarFromInventoryLocationName.DataType = DbType.AnsiString;
                colvarFromInventoryLocationName.MaxLength = 50;
                colvarFromInventoryLocationName.AutoIncrement = false;
                colvarFromInventoryLocationName.IsNullable = false;
                colvarFromInventoryLocationName.IsPrimaryKey = false;
                colvarFromInventoryLocationName.IsForeignKey = false;
                colvarFromInventoryLocationName.IsReadOnly = false;
                
                schema.Columns.Add(colvarFromInventoryLocationName);
                
                TableSchema.TableColumn colvarToInventoryLocationName = new TableSchema.TableColumn(schema);
                colvarToInventoryLocationName.ColumnName = "ToInventoryLocationName";
                colvarToInventoryLocationName.DataType = DbType.AnsiString;
                colvarToInventoryLocationName.MaxLength = 50;
                colvarToInventoryLocationName.AutoIncrement = false;
                colvarToInventoryLocationName.IsNullable = false;
                colvarToInventoryLocationName.IsPrimaryKey = false;
                colvarToInventoryLocationName.IsForeignKey = false;
                colvarToInventoryLocationName.IsReadOnly = false;
                
                schema.Columns.Add(colvarToInventoryLocationName);
                
                TableSchema.TableColumn colvarToInventoryLocationID = new TableSchema.TableColumn(schema);
                colvarToInventoryLocationID.ColumnName = "ToInventoryLocationID";
                colvarToInventoryLocationID.DataType = DbType.Int32;
                colvarToInventoryLocationID.MaxLength = 0;
                colvarToInventoryLocationID.AutoIncrement = false;
                colvarToInventoryLocationID.IsNullable = true;
                colvarToInventoryLocationID.IsPrimaryKey = false;
                colvarToInventoryLocationID.IsForeignKey = false;
                colvarToInventoryLocationID.IsReadOnly = false;
                
                schema.Columns.Add(colvarToInventoryLocationID);
                
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
                
                TableSchema.TableColumn colvarSearch = new TableSchema.TableColumn(schema);
                colvarSearch.ColumnName = "Search";
                colvarSearch.DataType = DbType.String;
                colvarSearch.MaxLength = 904;
                colvarSearch.AutoIncrement = false;
                colvarSearch.IsNullable = false;
                colvarSearch.IsPrimaryKey = false;
                colvarSearch.IsForeignKey = false;
                colvarSearch.IsReadOnly = false;
                
                schema.Columns.Add(colvarSearch);
                
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
                
                TableSchema.TableColumn colvarTransferFromBy = new TableSchema.TableColumn(schema);
                colvarTransferFromBy.ColumnName = "TransferFromBy";
                colvarTransferFromBy.DataType = DbType.AnsiString;
                colvarTransferFromBy.MaxLength = 50;
                colvarTransferFromBy.AutoIncrement = false;
                colvarTransferFromBy.IsNullable = false;
                colvarTransferFromBy.IsPrimaryKey = false;
                colvarTransferFromBy.IsForeignKey = false;
                colvarTransferFromBy.IsReadOnly = false;
                
                schema.Columns.Add(colvarTransferFromBy);
                
                
                BaseSchema = schema;
                //add this schema to the provider
                //so we can query it later
                DataService.Providers["PowerPOS"].AddSchema("ViewInventoryTransferDetail",schema);
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
	    public ViewInventoryTransferDetail()
	    {
            SetSQLProps();
            SetDefaults();
            MarkNew();
        }
        public ViewInventoryTransferDetail(bool useDatabaseDefaults)
	    {
		    SetSQLProps();
		    if(useDatabaseDefaults)
		    {
				ForceDefaults();
			}
			MarkNew();
	    }
	    
	    public ViewInventoryTransferDetail(object keyID)
	    {
		    SetSQLProps();
		    LoadByKey(keyID);
	    }
    	 
	    public ViewInventoryTransferDetail(string columnName, object columnValue)
        {
            SetSQLProps();
            LoadByParam(columnName,columnValue);
        }
        
	    #endregion
	    
	    #region Props
	    
          
        [XmlAttribute("TransferQty")]
        [Bindable(true)]
        public int? TransferQty 
	    {
		    get
		    {
			    return GetColumnValue<int?>("TransferQty");
		    }
            set 
		    {
			    SetColumnValue("TransferQty", value);
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
	      
        [XmlAttribute("LocationTransferID")]
        [Bindable(true)]
        public int LocationTransferID 
	    {
		    get
		    {
			    return GetColumnValue<int>("LocationTransferID");
		    }
            set 
		    {
			    SetColumnValue("LocationTransferID", value);
            }
        }
	      
        [XmlAttribute("FromInventoryLocationID")]
        [Bindable(true)]
        public int? FromInventoryLocationID 
	    {
		    get
		    {
			    return GetColumnValue<int?>("FromInventoryLocationID");
		    }
            set 
		    {
			    SetColumnValue("FromInventoryLocationID", value);
            }
        }
	      
        [XmlAttribute("FromInventoryLocationName")]
        [Bindable(true)]
        public string FromInventoryLocationName 
	    {
		    get
		    {
			    return GetColumnValue<string>("FromInventoryLocationName");
		    }
            set 
		    {
			    SetColumnValue("FromInventoryLocationName", value);
            }
        }
	      
        [XmlAttribute("ToInventoryLocationName")]
        [Bindable(true)]
        public string ToInventoryLocationName 
	    {
		    get
		    {
			    return GetColumnValue<string>("ToInventoryLocationName");
		    }
            set 
		    {
			    SetColumnValue("ToInventoryLocationName", value);
            }
        }
	      
        [XmlAttribute("ToInventoryLocationID")]
        [Bindable(true)]
        public int? ToInventoryLocationID 
	    {
		    get
		    {
			    return GetColumnValue<int?>("ToInventoryLocationID");
		    }
            set 
		    {
			    SetColumnValue("ToInventoryLocationID", value);
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
	      
        [XmlAttribute("Search")]
        [Bindable(true)]
        public string Search 
	    {
		    get
		    {
			    return GetColumnValue<string>("Search");
		    }
            set 
		    {
			    SetColumnValue("Search", value);
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
	      
        [XmlAttribute("TransferFromBy")]
        [Bindable(true)]
        public string TransferFromBy 
	    {
		    get
		    {
			    return GetColumnValue<string>("TransferFromBy");
		    }
            set 
		    {
			    SetColumnValue("TransferFromBy", value);
            }
        }
	    
	    #endregion
    
	    #region Columns Struct
	    public struct Columns
	    {
		    
		    
            public static string TransferQty = @"TransferQty";
            
            public static string ItemNo = @"ItemNo";
            
            public static string LocationTransferID = @"LocationTransferID";
            
            public static string FromInventoryLocationID = @"FromInventoryLocationID";
            
            public static string FromInventoryLocationName = @"FromInventoryLocationName";
            
            public static string ToInventoryLocationName = @"ToInventoryLocationName";
            
            public static string ToInventoryLocationID = @"ToInventoryLocationID";
            
            public static string ItemName = @"ItemName";
            
            public static string Search = @"Search";
            
            public static string CategoryName = @"CategoryName";
            
            public static string CostOfGoods = @"CostOfGoods";
            
            public static string InventoryDate = @"InventoryDate";
            
            public static string TransferFromBy = @"TransferFromBy";
            
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
