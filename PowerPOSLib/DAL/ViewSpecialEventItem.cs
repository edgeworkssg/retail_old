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
    /// Strongly-typed collection for the ViewSpecialEventItem class.
    /// </summary>
    [Serializable]
    public partial class ViewSpecialEventItemCollection : ReadOnlyList<ViewSpecialEventItem, ViewSpecialEventItemCollection>
    {        
        public ViewSpecialEventItemCollection() {}
    }
    /// <summary>
    /// This is  Read-only wrapper class for the ViewSpecialEventItems view.
    /// </summary>
    [Serializable]
    public partial class ViewSpecialEventItem : ReadOnlyRecord<ViewSpecialEventItem>, IReadOnlyRecord
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
                TableSchema.Table schema = new TableSchema.Table("ViewSpecialEventItems", TableType.View, DataService.GetInstance("PowerPOS"));
                schema.Columns = new TableSchema.TableColumnCollection();
                schema.SchemaName = @"dbo";
                //columns
                
                TableSchema.TableColumn colvarEventId = new TableSchema.TableColumn(schema);
                colvarEventId.ColumnName = "EventId";
                colvarEventId.DataType = DbType.Int32;
                colvarEventId.MaxLength = 0;
                colvarEventId.AutoIncrement = false;
                colvarEventId.IsNullable = false;
                colvarEventId.IsPrimaryKey = false;
                colvarEventId.IsForeignKey = false;
                colvarEventId.IsReadOnly = false;
                
                schema.Columns.Add(colvarEventId);
                
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
                
                TableSchema.TableColumn colvarItemPrice = new TableSchema.TableColumn(schema);
                colvarItemPrice.ColumnName = "ItemPrice";
                colvarItemPrice.DataType = DbType.Currency;
                colvarItemPrice.MaxLength = 0;
                colvarItemPrice.AutoIncrement = false;
                colvarItemPrice.IsNullable = true;
                colvarItemPrice.IsPrimaryKey = false;
                colvarItemPrice.IsForeignKey = false;
                colvarItemPrice.IsReadOnly = false;
                
                schema.Columns.Add(colvarItemPrice);
                
                TableSchema.TableColumn colvarEventItemMapID = new TableSchema.TableColumn(schema);
                colvarEventItemMapID.ColumnName = "EventItemMapID";
                colvarEventItemMapID.DataType = DbType.Int32;
                colvarEventItemMapID.MaxLength = 0;
                colvarEventItemMapID.AutoIncrement = false;
                colvarEventItemMapID.IsNullable = false;
                colvarEventItemMapID.IsPrimaryKey = false;
                colvarEventItemMapID.IsForeignKey = false;
                colvarEventItemMapID.IsReadOnly = false;
                
                schema.Columns.Add(colvarEventItemMapID);
                
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
                DataService.Providers["PowerPOS"].AddSchema("ViewSpecialEventItems",schema);
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
	    public ViewSpecialEventItem()
	    {
            SetSQLProps();
            SetDefaults();
            MarkNew();
        }
        public ViewSpecialEventItem(bool useDatabaseDefaults)
	    {
		    SetSQLProps();
		    if(useDatabaseDefaults)
		    {
				ForceDefaults();
			}
			MarkNew();
	    }
	    
	    public ViewSpecialEventItem(object keyID)
	    {
		    SetSQLProps();
		    LoadByKey(keyID);
	    }
    	 
	    public ViewSpecialEventItem(string columnName, object columnValue)
        {
            SetSQLProps();
            LoadByParam(columnName,columnValue);
        }
        
	    #endregion
	    
	    #region Props
	    
          
        [XmlAttribute("EventId")]
        [Bindable(true)]
        public int EventId 
	    {
		    get
		    {
			    return GetColumnValue<int>("EventId");
		    }
            set 
		    {
			    SetColumnValue("EventId", value);
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
	      
        [XmlAttribute("ItemPrice")]
        [Bindable(true)]
        public decimal? ItemPrice 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("ItemPrice");
		    }
            set 
		    {
			    SetColumnValue("ItemPrice", value);
            }
        }
	      
        [XmlAttribute("EventItemMapID")]
        [Bindable(true)]
        public int EventItemMapID 
	    {
		    get
		    {
			    return GetColumnValue<int>("EventItemMapID");
		    }
            set 
		    {
			    SetColumnValue("EventItemMapID", value);
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
		    
		    
            public static string EventId = @"EventId";
            
            public static string ItemNo = @"ItemNo";
            
            public static string ItemName = @"ItemName";
            
            public static string CategoryName = @"CategoryName";
            
            public static string ItemPrice = @"ItemPrice";
            
            public static string EventItemMapID = @"EventItemMapID";
            
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
