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
    /// Strongly-typed collection for the ViewItemGroupMap class.
    /// </summary>
    [Serializable]
    public partial class ViewItemGroupMapCollection : ReadOnlyList<ViewItemGroupMap, ViewItemGroupMapCollection>
    {        
        public ViewItemGroupMapCollection() {}
    }
    /// <summary>
    /// This is  Read-only wrapper class for the ViewItemGroupMap view.
    /// </summary>
    [Serializable]
    public partial class ViewItemGroupMap : ReadOnlyRecord<ViewItemGroupMap>, IReadOnlyRecord
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
                TableSchema.Table schema = new TableSchema.Table("ViewItemGroupMap", TableType.View, DataService.GetInstance("PowerPOS"));
                schema.Columns = new TableSchema.TableColumnCollection();
                schema.SchemaName = @"dbo";
                //columns
                
                TableSchema.TableColumn colvarItemGroupName = new TableSchema.TableColumn(schema);
                colvarItemGroupName.ColumnName = "ItemGroupName";
                colvarItemGroupName.DataType = DbType.AnsiString;
                colvarItemGroupName.MaxLength = 50;
                colvarItemGroupName.AutoIncrement = false;
                colvarItemGroupName.IsNullable = false;
                colvarItemGroupName.IsPrimaryKey = false;
                colvarItemGroupName.IsForeignKey = false;
                colvarItemGroupName.IsReadOnly = false;
                
                schema.Columns.Add(colvarItemGroupName);
                
                TableSchema.TableColumn colvarItemGroupId = new TableSchema.TableColumn(schema);
                colvarItemGroupId.ColumnName = "ItemGroupId";
                colvarItemGroupId.DataType = DbType.Int32;
                colvarItemGroupId.MaxLength = 0;
                colvarItemGroupId.AutoIncrement = false;
                colvarItemGroupId.IsNullable = false;
                colvarItemGroupId.IsPrimaryKey = false;
                colvarItemGroupId.IsForeignKey = false;
                colvarItemGroupId.IsReadOnly = false;
                
                schema.Columns.Add(colvarItemGroupId);
                
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
                
                TableSchema.TableColumn colvarUnitQty = new TableSchema.TableColumn(schema);
                colvarUnitQty.ColumnName = "UnitQty";
                colvarUnitQty.DataType = DbType.Int32;
                colvarUnitQty.MaxLength = 0;
                colvarUnitQty.AutoIncrement = false;
                colvarUnitQty.IsNullable = false;
                colvarUnitQty.IsPrimaryKey = false;
                colvarUnitQty.IsForeignKey = false;
                colvarUnitQty.IsReadOnly = false;
                
                schema.Columns.Add(colvarUnitQty);
                
                
                BaseSchema = schema;
                //add this schema to the provider
                //so we can query it later
                DataService.Providers["PowerPOS"].AddSchema("ViewItemGroupMap",schema);
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
	    public ViewItemGroupMap()
	    {
            SetSQLProps();
            SetDefaults();
            MarkNew();
        }
        public ViewItemGroupMap(bool useDatabaseDefaults)
	    {
		    SetSQLProps();
		    if(useDatabaseDefaults)
		    {
				ForceDefaults();
			}
			MarkNew();
	    }
	    
	    public ViewItemGroupMap(object keyID)
	    {
		    SetSQLProps();
		    LoadByKey(keyID);
	    }
    	 
	    public ViewItemGroupMap(string columnName, object columnValue)
        {
            SetSQLProps();
            LoadByParam(columnName,columnValue);
        }
        
	    #endregion
	    
	    #region Props
	    
          
        [XmlAttribute("ItemGroupName")]
        [Bindable(true)]
        public string ItemGroupName 
	    {
		    get
		    {
			    return GetColumnValue<string>("ItemGroupName");
		    }
            set 
		    {
			    SetColumnValue("ItemGroupName", value);
            }
        }
	      
        [XmlAttribute("ItemGroupId")]
        [Bindable(true)]
        public int ItemGroupId 
	    {
		    get
		    {
			    return GetColumnValue<int>("ItemGroupId");
		    }
            set 
		    {
			    SetColumnValue("ItemGroupId", value);
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
	      
        [XmlAttribute("UnitQty")]
        [Bindable(true)]
        public int UnitQty 
	    {
		    get
		    {
			    return GetColumnValue<int>("UnitQty");
		    }
            set 
		    {
			    SetColumnValue("UnitQty", value);
            }
        }
	    
	    #endregion
    
	    #region Columns Struct
	    public struct Columns
	    {
		    
		    
            public static string ItemGroupName = @"ItemGroupName";
            
            public static string ItemGroupId = @"ItemGroupId";
            
            public static string ItemName = @"ItemName";
            
            public static string UnitQty = @"UnitQty";
            
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
