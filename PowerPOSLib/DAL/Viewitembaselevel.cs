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
    /// Strongly-typed collection for the Viewitembaselevel class.
    /// </summary>
    [Serializable]
    public partial class ViewitembaselevelCollection : ReadOnlyList<Viewitembaselevel, ViewitembaselevelCollection>
    {        
        public ViewitembaselevelCollection() {}
    }
    /// <summary>
    /// This is  Read-only wrapper class for the viewitembaselevel view.
    /// </summary>
    [Serializable]
    public partial class Viewitembaselevel : ReadOnlyRecord<Viewitembaselevel>, IReadOnlyRecord
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
                TableSchema.Table schema = new TableSchema.Table("viewitembaselevel", TableType.View, DataService.GetInstance("PowerPOS"));
                schema.Columns = new TableSchema.TableColumnCollection();
                schema.SchemaName = @"dbo";
                //columns
                
                TableSchema.TableColumn colvarBaseLevelID = new TableSchema.TableColumn(schema);
                colvarBaseLevelID.ColumnName = "BaseLevelID";
                colvarBaseLevelID.DataType = DbType.Int32;
                colvarBaseLevelID.MaxLength = 0;
                colvarBaseLevelID.AutoIncrement = false;
                colvarBaseLevelID.IsNullable = false;
                colvarBaseLevelID.IsPrimaryKey = false;
                colvarBaseLevelID.IsForeignKey = false;
                colvarBaseLevelID.IsReadOnly = false;
                
                schema.Columns.Add(colvarBaseLevelID);
                
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
                
                TableSchema.TableColumn colvarBaseLevelQuantity = new TableSchema.TableColumn(schema);
                colvarBaseLevelQuantity.ColumnName = "BaseLevelQuantity";
                colvarBaseLevelQuantity.DataType = DbType.Int32;
                colvarBaseLevelQuantity.MaxLength = 0;
                colvarBaseLevelQuantity.AutoIncrement = false;
                colvarBaseLevelQuantity.IsNullable = false;
                colvarBaseLevelQuantity.IsPrimaryKey = false;
                colvarBaseLevelQuantity.IsForeignKey = false;
                colvarBaseLevelQuantity.IsReadOnly = false;
                
                schema.Columns.Add(colvarBaseLevelQuantity);
                
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
                
                TableSchema.TableColumn colvarCategory = new TableSchema.TableColumn(schema);
                colvarCategory.ColumnName = "Category";
                colvarCategory.DataType = DbType.String;
                colvarCategory.MaxLength = 250;
                colvarCategory.AutoIncrement = false;
                colvarCategory.IsNullable = false;
                colvarCategory.IsPrimaryKey = false;
                colvarCategory.IsForeignKey = false;
                colvarCategory.IsReadOnly = false;
                
                schema.Columns.Add(colvarCategory);
                
                TableSchema.TableColumn colvarUom = new TableSchema.TableColumn(schema);
                colvarUom.ColumnName = "UOM";
                colvarUom.DataType = DbType.AnsiString;
                colvarUom.MaxLength = 50;
                colvarUom.AutoIncrement = false;
                colvarUom.IsNullable = true;
                colvarUom.IsPrimaryKey = false;
                colvarUom.IsForeignKey = false;
                colvarUom.IsReadOnly = false;
                
                schema.Columns.Add(colvarUom);
                
                
                BaseSchema = schema;
                //add this schema to the provider
                //so we can query it later
                DataService.Providers["PowerPOS"].AddSchema("viewitembaselevel",schema);
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
	    public Viewitembaselevel()
	    {
            SetSQLProps();
            SetDefaults();
            MarkNew();
        }
        public Viewitembaselevel(bool useDatabaseDefaults)
	    {
		    SetSQLProps();
		    if(useDatabaseDefaults)
		    {
				ForceDefaults();
			}
			MarkNew();
	    }
	    
	    public Viewitembaselevel(object keyID)
	    {
		    SetSQLProps();
		    LoadByKey(keyID);
	    }
    	 
	    public Viewitembaselevel(string columnName, object columnValue)
        {
            SetSQLProps();
            LoadByParam(columnName,columnValue);
        }
        
	    #endregion
	    
	    #region Props
	    
          
        [XmlAttribute("BaseLevelID")]
        [Bindable(true)]
        public int BaseLevelID 
	    {
		    get
		    {
			    return GetColumnValue<int>("BaseLevelID");
		    }
            set 
		    {
			    SetColumnValue("BaseLevelID", value);
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
	      
        [XmlAttribute("BaseLevelQuantity")]
        [Bindable(true)]
        public int BaseLevelQuantity 
	    {
		    get
		    {
			    return GetColumnValue<int>("BaseLevelQuantity");
		    }
            set 
		    {
			    SetColumnValue("BaseLevelQuantity", value);
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
	      
        [XmlAttribute("Category")]
        [Bindable(true)]
        public string Category 
	    {
		    get
		    {
			    return GetColumnValue<string>("Category");
		    }
            set 
		    {
			    SetColumnValue("Category", value);
            }
        }
	      
        [XmlAttribute("Uom")]
        [Bindable(true)]
        public string Uom 
	    {
		    get
		    {
			    return GetColumnValue<string>("UOM");
		    }
            set 
		    {
			    SetColumnValue("UOM", value);
            }
        }
	    
	    #endregion
    
	    #region Columns Struct
	    public struct Columns
	    {
		    
		    
            public static string BaseLevelID = @"BaseLevelID";
            
            public static string ItemNo = @"ItemNo";
            
            public static string ItemName = @"ItemName";
            
            public static string BaseLevelQuantity = @"BaseLevelQuantity";
            
            public static string InventoryLocationId = @"InventoryLocationId";
            
            public static string InventoryLocationName = @"InventoryLocationName";
            
            public static string Category = @"Category";
            
            public static string Uom = @"UOM";
            
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
