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
    /// Strongly-typed collection for the ViewEZLinkMessageParameter class.
    /// </summary>
    [Serializable]
    public partial class ViewEZLinkMessageParameterCollection : ReadOnlyList<ViewEZLinkMessageParameter, ViewEZLinkMessageParameterCollection>
    {        
        public ViewEZLinkMessageParameterCollection() {}
    }
    /// <summary>
    /// This is  Read-only wrapper class for the ViewEZLinkMessageParameter view.
    /// </summary>
    [Serializable]
    public partial class ViewEZLinkMessageParameter : ReadOnlyRecord<ViewEZLinkMessageParameter>, IReadOnlyRecord
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
                TableSchema.Table schema = new TableSchema.Table("ViewEZLinkMessageParameter", TableType.View, DataService.GetInstance("PowerPOS"));
                schema.Columns = new TableSchema.TableColumnCollection();
                schema.SchemaName = @"dbo";
                //columns
                
                TableSchema.TableColumn colvarParameterName = new TableSchema.TableColumn(schema);
                colvarParameterName.ColumnName = "ParameterName";
                colvarParameterName.DataType = DbType.AnsiString;
                colvarParameterName.MaxLength = 50;
                colvarParameterName.AutoIncrement = false;
                colvarParameterName.IsNullable = false;
                colvarParameterName.IsPrimaryKey = false;
                colvarParameterName.IsForeignKey = false;
                colvarParameterName.IsReadOnly = false;
                
                schema.Columns.Add(colvarParameterName);
                
                TableSchema.TableColumn colvarParameterLength = new TableSchema.TableColumn(schema);
                colvarParameterLength.ColumnName = "ParameterLength";
                colvarParameterLength.DataType = DbType.Int32;
                colvarParameterLength.MaxLength = 0;
                colvarParameterLength.AutoIncrement = false;
                colvarParameterLength.IsNullable = false;
                colvarParameterLength.IsPrimaryKey = false;
                colvarParameterLength.IsForeignKey = false;
                colvarParameterLength.IsReadOnly = false;
                
                schema.Columns.Add(colvarParameterLength);
                
                TableSchema.TableColumn colvarMapID = new TableSchema.TableColumn(schema);
                colvarMapID.ColumnName = "MapID";
                colvarMapID.DataType = DbType.Int32;
                colvarMapID.MaxLength = 0;
                colvarMapID.AutoIncrement = false;
                colvarMapID.IsNullable = false;
                colvarMapID.IsPrimaryKey = false;
                colvarMapID.IsForeignKey = false;
                colvarMapID.IsReadOnly = false;
                
                schema.Columns.Add(colvarMapID);
                
                TableSchema.TableColumn colvarStartingIndex = new TableSchema.TableColumn(schema);
                colvarStartingIndex.ColumnName = "startingIndex";
                colvarStartingIndex.DataType = DbType.Int32;
                colvarStartingIndex.MaxLength = 0;
                colvarStartingIndex.AutoIncrement = false;
                colvarStartingIndex.IsNullable = false;
                colvarStartingIndex.IsPrimaryKey = false;
                colvarStartingIndex.IsForeignKey = false;
                colvarStartingIndex.IsReadOnly = false;
                
                schema.Columns.Add(colvarStartingIndex);
                
                TableSchema.TableColumn colvarMsgName = new TableSchema.TableColumn(schema);
                colvarMsgName.ColumnName = "MsgName";
                colvarMsgName.DataType = DbType.AnsiString;
                colvarMsgName.MaxLength = 50;
                colvarMsgName.AutoIncrement = false;
                colvarMsgName.IsNullable = false;
                colvarMsgName.IsPrimaryKey = false;
                colvarMsgName.IsForeignKey = false;
                colvarMsgName.IsReadOnly = false;
                
                schema.Columns.Add(colvarMsgName);
                
                
                BaseSchema = schema;
                //add this schema to the provider
                //so we can query it later
                DataService.Providers["PowerPOS"].AddSchema("ViewEZLinkMessageParameter",schema);
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
	    public ViewEZLinkMessageParameter()
	    {
            SetSQLProps();
            SetDefaults();
            MarkNew();
        }
        public ViewEZLinkMessageParameter(bool useDatabaseDefaults)
	    {
		    SetSQLProps();
		    if(useDatabaseDefaults)
		    {
				ForceDefaults();
			}
			MarkNew();
	    }
	    
	    public ViewEZLinkMessageParameter(object keyID)
	    {
		    SetSQLProps();
		    LoadByKey(keyID);
	    }
    	 
	    public ViewEZLinkMessageParameter(string columnName, object columnValue)
        {
            SetSQLProps();
            LoadByParam(columnName,columnValue);
        }
        
	    #endregion
	    
	    #region Props
	    
          
        [XmlAttribute("ParameterName")]
        [Bindable(true)]
        public string ParameterName 
	    {
		    get
		    {
			    return GetColumnValue<string>("ParameterName");
		    }
            set 
		    {
			    SetColumnValue("ParameterName", value);
            }
        }
	      
        [XmlAttribute("ParameterLength")]
        [Bindable(true)]
        public int ParameterLength 
	    {
		    get
		    {
			    return GetColumnValue<int>("ParameterLength");
		    }
            set 
		    {
			    SetColumnValue("ParameterLength", value);
            }
        }
	      
        [XmlAttribute("MapID")]
        [Bindable(true)]
        public int MapID 
	    {
		    get
		    {
			    return GetColumnValue<int>("MapID");
		    }
            set 
		    {
			    SetColumnValue("MapID", value);
            }
        }
	      
        [XmlAttribute("StartingIndex")]
        [Bindable(true)]
        public int StartingIndex 
	    {
		    get
		    {
			    return GetColumnValue<int>("startingIndex");
		    }
            set 
		    {
			    SetColumnValue("startingIndex", value);
            }
        }
	      
        [XmlAttribute("MsgName")]
        [Bindable(true)]
        public string MsgName 
	    {
		    get
		    {
			    return GetColumnValue<string>("MsgName");
		    }
            set 
		    {
			    SetColumnValue("MsgName", value);
            }
        }
	    
	    #endregion
    
	    #region Columns Struct
	    public struct Columns
	    {
		    
		    
            public static string ParameterName = @"ParameterName";
            
            public static string ParameterLength = @"ParameterLength";
            
            public static string MapID = @"MapID";
            
            public static string StartingIndex = @"startingIndex";
            
            public static string MsgName = @"MsgName";
            
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
