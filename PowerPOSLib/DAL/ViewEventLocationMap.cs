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
    /// Strongly-typed collection for the ViewEventLocationMap class.
    /// </summary>
    [Serializable]
    public partial class ViewEventLocationMapCollection : ReadOnlyList<ViewEventLocationMap, ViewEventLocationMapCollection>
    {        
        public ViewEventLocationMapCollection() {}
    }
    /// <summary>
    /// This is  Read-only wrapper class for the ViewEventLocationMap view.
    /// </summary>
    [Serializable]
    public partial class ViewEventLocationMap : ReadOnlyRecord<ViewEventLocationMap>, IReadOnlyRecord
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
                TableSchema.Table schema = new TableSchema.Table("ViewEventLocationMap", TableType.View, DataService.GetInstance("PowerPOS"));
                schema.Columns = new TableSchema.TableColumnCollection();
                schema.SchemaName = @"dbo";
                //columns
                
                TableSchema.TableColumn colvarEventID = new TableSchema.TableColumn(schema);
                colvarEventID.ColumnName = "EventID";
                colvarEventID.DataType = DbType.Int32;
                colvarEventID.MaxLength = 0;
                colvarEventID.AutoIncrement = false;
                colvarEventID.IsNullable = false;
                colvarEventID.IsPrimaryKey = false;
                colvarEventID.IsForeignKey = false;
                colvarEventID.IsReadOnly = false;
                
                schema.Columns.Add(colvarEventID);
                
                TableSchema.TableColumn colvarEventLocationMapID = new TableSchema.TableColumn(schema);
                colvarEventLocationMapID.ColumnName = "EventLocationMapID";
                colvarEventLocationMapID.DataType = DbType.Int32;
                colvarEventLocationMapID.MaxLength = 0;
                colvarEventLocationMapID.AutoIncrement = false;
                colvarEventLocationMapID.IsNullable = false;
                colvarEventLocationMapID.IsPrimaryKey = false;
                colvarEventLocationMapID.IsForeignKey = false;
                colvarEventLocationMapID.IsReadOnly = false;
                
                schema.Columns.Add(colvarEventLocationMapID);
                
                TableSchema.TableColumn colvarPointOfSaleID = new TableSchema.TableColumn(schema);
                colvarPointOfSaleID.ColumnName = "PointOfSaleID";
                colvarPointOfSaleID.DataType = DbType.Int32;
                colvarPointOfSaleID.MaxLength = 0;
                colvarPointOfSaleID.AutoIncrement = false;
                colvarPointOfSaleID.IsNullable = false;
                colvarPointOfSaleID.IsPrimaryKey = false;
                colvarPointOfSaleID.IsForeignKey = false;
                colvarPointOfSaleID.IsReadOnly = false;
                
                schema.Columns.Add(colvarPointOfSaleID);
                
                TableSchema.TableColumn colvarPointOfSaleName = new TableSchema.TableColumn(schema);
                colvarPointOfSaleName.ColumnName = "PointOfSaleName";
                colvarPointOfSaleName.DataType = DbType.AnsiString;
                colvarPointOfSaleName.MaxLength = 50;
                colvarPointOfSaleName.AutoIncrement = false;
                colvarPointOfSaleName.IsNullable = false;
                colvarPointOfSaleName.IsPrimaryKey = false;
                colvarPointOfSaleName.IsForeignKey = false;
                colvarPointOfSaleName.IsReadOnly = false;
                
                schema.Columns.Add(colvarPointOfSaleName);
                
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
                
                TableSchema.TableColumn colvarOutletName = new TableSchema.TableColumn(schema);
                colvarOutletName.ColumnName = "OutletName";
                colvarOutletName.DataType = DbType.AnsiString;
                colvarOutletName.MaxLength = 50;
                colvarOutletName.AutoIncrement = false;
                colvarOutletName.IsNullable = false;
                colvarOutletName.IsPrimaryKey = false;
                colvarOutletName.IsForeignKey = false;
                colvarOutletName.IsReadOnly = false;
                
                schema.Columns.Add(colvarOutletName);
                
                
                BaseSchema = schema;
                //add this schema to the provider
                //so we can query it later
                DataService.Providers["PowerPOS"].AddSchema("ViewEventLocationMap",schema);
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
	    public ViewEventLocationMap()
	    {
            SetSQLProps();
            SetDefaults();
            MarkNew();
        }
        public ViewEventLocationMap(bool useDatabaseDefaults)
	    {
		    SetSQLProps();
		    if(useDatabaseDefaults)
		    {
				ForceDefaults();
			}
			MarkNew();
	    }
	    
	    public ViewEventLocationMap(object keyID)
	    {
		    SetSQLProps();
		    LoadByKey(keyID);
	    }
    	 
	    public ViewEventLocationMap(string columnName, object columnValue)
        {
            SetSQLProps();
            LoadByParam(columnName,columnValue);
        }
        
	    #endregion
	    
	    #region Props
	    
          
        [XmlAttribute("EventID")]
        [Bindable(true)]
        public int EventID 
	    {
		    get
		    {
			    return GetColumnValue<int>("EventID");
		    }
            set 
		    {
			    SetColumnValue("EventID", value);
            }
        }
	      
        [XmlAttribute("EventLocationMapID")]
        [Bindable(true)]
        public int EventLocationMapID 
	    {
		    get
		    {
			    return GetColumnValue<int>("EventLocationMapID");
		    }
            set 
		    {
			    SetColumnValue("EventLocationMapID", value);
            }
        }
	      
        [XmlAttribute("PointOfSaleID")]
        [Bindable(true)]
        public int PointOfSaleID 
	    {
		    get
		    {
			    return GetColumnValue<int>("PointOfSaleID");
		    }
            set 
		    {
			    SetColumnValue("PointOfSaleID", value);
            }
        }
	      
        [XmlAttribute("PointOfSaleName")]
        [Bindable(true)]
        public string PointOfSaleName 
	    {
		    get
		    {
			    return GetColumnValue<string>("PointOfSaleName");
		    }
            set 
		    {
			    SetColumnValue("PointOfSaleName", value);
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
	      
        [XmlAttribute("OutletName")]
        [Bindable(true)]
        public string OutletName 
	    {
		    get
		    {
			    return GetColumnValue<string>("OutletName");
		    }
            set 
		    {
			    SetColumnValue("OutletName", value);
            }
        }
	    
	    #endregion
    
	    #region Columns Struct
	    public struct Columns
	    {
		    
		    
            public static string EventID = @"EventID";
            
            public static string EventLocationMapID = @"EventLocationMapID";
            
            public static string PointOfSaleID = @"PointOfSaleID";
            
            public static string PointOfSaleName = @"PointOfSaleName";
            
            public static string Deleted = @"Deleted";
            
            public static string OutletName = @"OutletName";
            
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
