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
    /// Strongly-typed collection for the ViewSaveItem class.
    /// </summary>
    [Serializable]
    public partial class ViewSaveItemCollection : ReadOnlyList<ViewSaveItem, ViewSaveItemCollection>
    {        
        public ViewSaveItemCollection() {}
    }
    /// <summary>
    /// This is  Read-only wrapper class for the ViewSaveItem view.
    /// </summary>
    [Serializable]
    public partial class ViewSaveItem : ReadOnlyRecord<ViewSaveItem>, IReadOnlyRecord
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
                TableSchema.Table schema = new TableSchema.Table("ViewSaveItem", TableType.View, DataService.GetInstance("PowerPOS"));
                schema.Columns = new TableSchema.TableColumnCollection();
                schema.SchemaName = @"dbo";
                //columns
                
                TableSchema.TableColumn colvarMaxDate = new TableSchema.TableColumn(schema);
                colvarMaxDate.ColumnName = "MaxDate";
                colvarMaxDate.DataType = DbType.DateTime;
                colvarMaxDate.MaxLength = 0;
                colvarMaxDate.AutoIncrement = false;
                colvarMaxDate.IsNullable = true;
                colvarMaxDate.IsPrimaryKey = false;
                colvarMaxDate.IsForeignKey = false;
                colvarMaxDate.IsReadOnly = false;
                
                schema.Columns.Add(colvarMaxDate);
                
                TableSchema.TableColumn colvarSaveName = new TableSchema.TableColumn(schema);
                colvarSaveName.ColumnName = "SaveName";
                colvarSaveName.DataType = DbType.AnsiString;
                colvarSaveName.MaxLength = 50;
                colvarSaveName.AutoIncrement = false;
                colvarSaveName.IsNullable = false;
                colvarSaveName.IsPrimaryKey = false;
                colvarSaveName.IsForeignKey = false;
                colvarSaveName.IsReadOnly = false;
                
                schema.Columns.Add(colvarSaveName);
                
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
                
                TableSchema.TableColumn colvarTransferFrom = new TableSchema.TableColumn(schema);
                colvarTransferFrom.ColumnName = "TransferFrom";
                colvarTransferFrom.DataType = DbType.Int32;
                colvarTransferFrom.MaxLength = 0;
                colvarTransferFrom.AutoIncrement = false;
                colvarTransferFrom.IsNullable = true;
                colvarTransferFrom.IsPrimaryKey = false;
                colvarTransferFrom.IsForeignKey = false;
                colvarTransferFrom.IsReadOnly = false;
                
                schema.Columns.Add(colvarTransferFrom);
                
                TableSchema.TableColumn colvarTransferTo = new TableSchema.TableColumn(schema);
                colvarTransferTo.ColumnName = "TransferTo";
                colvarTransferTo.DataType = DbType.Int32;
                colvarTransferTo.MaxLength = 0;
                colvarTransferTo.AutoIncrement = false;
                colvarTransferTo.IsNullable = true;
                colvarTransferTo.IsPrimaryKey = false;
                colvarTransferTo.IsForeignKey = false;
                colvarTransferTo.IsReadOnly = false;
                
                schema.Columns.Add(colvarTransferTo);
                
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
                
                
                BaseSchema = schema;
                //add this schema to the provider
                //so we can query it later
                DataService.Providers["PowerPOS"].AddSchema("ViewSaveItem",schema);
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
	    public ViewSaveItem()
	    {
            SetSQLProps();
            SetDefaults();
            MarkNew();
        }
        public ViewSaveItem(bool useDatabaseDefaults)
	    {
		    SetSQLProps();
		    if(useDatabaseDefaults)
		    {
				ForceDefaults();
			}
			MarkNew();
	    }
	    
	    public ViewSaveItem(object keyID)
	    {
		    SetSQLProps();
		    LoadByKey(keyID);
	    }
    	 
	    public ViewSaveItem(string columnName, object columnValue)
        {
            SetSQLProps();
            LoadByParam(columnName,columnValue);
        }
        
	    #endregion
	    
	    #region Props
	    
          
        [XmlAttribute("MaxDate")]
        [Bindable(true)]
        public DateTime? MaxDate 
	    {
		    get
		    {
			    return GetColumnValue<DateTime?>("MaxDate");
		    }
            set 
		    {
			    SetColumnValue("MaxDate", value);
            }
        }
	      
        [XmlAttribute("SaveName")]
        [Bindable(true)]
        public string SaveName 
	    {
		    get
		    {
			    return GetColumnValue<string>("SaveName");
		    }
            set 
		    {
			    SetColumnValue("SaveName", value);
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
	      
        [XmlAttribute("TransferFrom")]
        [Bindable(true)]
        public int? TransferFrom 
	    {
		    get
		    {
			    return GetColumnValue<int?>("TransferFrom");
		    }
            set 
		    {
			    SetColumnValue("TransferFrom", value);
            }
        }
	      
        [XmlAttribute("TransferTo")]
        [Bindable(true)]
        public int? TransferTo 
	    {
		    get
		    {
			    return GetColumnValue<int?>("TransferTo");
		    }
            set 
		    {
			    SetColumnValue("TransferTo", value);
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
	    
	    #endregion
    
	    #region Columns Struct
	    public struct Columns
	    {
		    
		    
            public static string MaxDate = @"MaxDate";
            
            public static string SaveName = @"SaveName";
            
            public static string StockOutReasonID = @"StockOutReasonID";
            
            public static string InventoryLocationID = @"InventoryLocationID";
            
            public static string TransferFrom = @"TransferFrom";
            
            public static string TransferTo = @"TransferTo";
            
            public static string MovementType = @"MovementType";
            
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
