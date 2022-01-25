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
    /// Strongly-typed collection for the ViewQuickAccessGroupMap class.
    /// </summary>
    [Serializable]
    public partial class ViewQuickAccessGroupMapCollection : ReadOnlyList<ViewQuickAccessGroupMap, ViewQuickAccessGroupMapCollection>
    {        
        public ViewQuickAccessGroupMapCollection() {}
    }
    /// <summary>
    /// This is  Read-only wrapper class for the ViewQuickAccessGroupMap view.
    /// </summary>
    [Serializable]
    public partial class ViewQuickAccessGroupMap : ReadOnlyRecord<ViewQuickAccessGroupMap>, IReadOnlyRecord
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
                TableSchema.Table schema = new TableSchema.Table("ViewQuickAccessGroupMap", TableType.View, DataService.GetInstance("PowerPOS"));
                schema.Columns = new TableSchema.TableColumnCollection();
                schema.SchemaName = @"dbo";
                //columns
                
                TableSchema.TableColumn colvarQuickAccessGroupName = new TableSchema.TableColumn(schema);
                colvarQuickAccessGroupName.ColumnName = "QuickAccessGroupName";
                colvarQuickAccessGroupName.DataType = DbType.AnsiString;
                colvarQuickAccessGroupName.MaxLength = 250;
                colvarQuickAccessGroupName.AutoIncrement = false;
                colvarQuickAccessGroupName.IsNullable = true;
                colvarQuickAccessGroupName.IsPrimaryKey = false;
                colvarQuickAccessGroupName.IsForeignKey = false;
                colvarQuickAccessGroupName.IsReadOnly = false;
                
                schema.Columns.Add(colvarQuickAccessGroupName);
                
                TableSchema.TableColumn colvarQuickAccessGroupMapID = new TableSchema.TableColumn(schema);
                colvarQuickAccessGroupMapID.ColumnName = "QuickAccessGroupMapID";
                colvarQuickAccessGroupMapID.DataType = DbType.Guid;
                colvarQuickAccessGroupMapID.MaxLength = 0;
                colvarQuickAccessGroupMapID.AutoIncrement = false;
                colvarQuickAccessGroupMapID.IsNullable = false;
                colvarQuickAccessGroupMapID.IsPrimaryKey = false;
                colvarQuickAccessGroupMapID.IsForeignKey = false;
                colvarQuickAccessGroupMapID.IsReadOnly = false;
                
                schema.Columns.Add(colvarQuickAccessGroupMapID);
                
                TableSchema.TableColumn colvarQuickAccessCatName = new TableSchema.TableColumn(schema);
                colvarQuickAccessCatName.ColumnName = "QuickAccessCatName";
                colvarQuickAccessCatName.DataType = DbType.String;
                colvarQuickAccessCatName.MaxLength = 50;
                colvarQuickAccessCatName.AutoIncrement = false;
                colvarQuickAccessCatName.IsNullable = false;
                colvarQuickAccessCatName.IsPrimaryKey = false;
                colvarQuickAccessCatName.IsForeignKey = false;
                colvarQuickAccessCatName.IsReadOnly = false;
                
                schema.Columns.Add(colvarQuickAccessCatName);
                
                TableSchema.TableColumn colvarQuickAccessGroupID = new TableSchema.TableColumn(schema);
                colvarQuickAccessGroupID.ColumnName = "QuickAccessGroupID";
                colvarQuickAccessGroupID.DataType = DbType.Guid;
                colvarQuickAccessGroupID.MaxLength = 0;
                colvarQuickAccessGroupID.AutoIncrement = false;
                colvarQuickAccessGroupID.IsNullable = false;
                colvarQuickAccessGroupID.IsPrimaryKey = false;
                colvarQuickAccessGroupID.IsForeignKey = false;
                colvarQuickAccessGroupID.IsReadOnly = false;
                
                schema.Columns.Add(colvarQuickAccessGroupID);
                
                TableSchema.TableColumn colvarQuickAccessCategoryID = new TableSchema.TableColumn(schema);
                colvarQuickAccessCategoryID.ColumnName = "QuickAccessCategoryID";
                colvarQuickAccessCategoryID.DataType = DbType.Guid;
                colvarQuickAccessCategoryID.MaxLength = 0;
                colvarQuickAccessCategoryID.AutoIncrement = false;
                colvarQuickAccessCategoryID.IsNullable = false;
                colvarQuickAccessCategoryID.IsPrimaryKey = false;
                colvarQuickAccessCategoryID.IsForeignKey = false;
                colvarQuickAccessCategoryID.IsReadOnly = false;
                
                schema.Columns.Add(colvarQuickAccessCategoryID);
                
                TableSchema.TableColumn colvarCreatedOn = new TableSchema.TableColumn(schema);
                colvarCreatedOn.ColumnName = "CreatedOn";
                colvarCreatedOn.DataType = DbType.DateTime;
                colvarCreatedOn.MaxLength = 0;
                colvarCreatedOn.AutoIncrement = false;
                colvarCreatedOn.IsNullable = true;
                colvarCreatedOn.IsPrimaryKey = false;
                colvarCreatedOn.IsForeignKey = false;
                colvarCreatedOn.IsReadOnly = false;
                
                schema.Columns.Add(colvarCreatedOn);
                
                TableSchema.TableColumn colvarCreatedBy = new TableSchema.TableColumn(schema);
                colvarCreatedBy.ColumnName = "CreatedBy";
                colvarCreatedBy.DataType = DbType.AnsiString;
                colvarCreatedBy.MaxLength = 50;
                colvarCreatedBy.AutoIncrement = false;
                colvarCreatedBy.IsNullable = true;
                colvarCreatedBy.IsPrimaryKey = false;
                colvarCreatedBy.IsForeignKey = false;
                colvarCreatedBy.IsReadOnly = false;
                
                schema.Columns.Add(colvarCreatedBy);
                
                TableSchema.TableColumn colvarModifiedOn = new TableSchema.TableColumn(schema);
                colvarModifiedOn.ColumnName = "ModifiedOn";
                colvarModifiedOn.DataType = DbType.DateTime;
                colvarModifiedOn.MaxLength = 0;
                colvarModifiedOn.AutoIncrement = false;
                colvarModifiedOn.IsNullable = true;
                colvarModifiedOn.IsPrimaryKey = false;
                colvarModifiedOn.IsForeignKey = false;
                colvarModifiedOn.IsReadOnly = false;
                
                schema.Columns.Add(colvarModifiedOn);
                
                TableSchema.TableColumn colvarModifiedBy = new TableSchema.TableColumn(schema);
                colvarModifiedBy.ColumnName = "ModifiedBy";
                colvarModifiedBy.DataType = DbType.AnsiString;
                colvarModifiedBy.MaxLength = 50;
                colvarModifiedBy.AutoIncrement = false;
                colvarModifiedBy.IsNullable = true;
                colvarModifiedBy.IsPrimaryKey = false;
                colvarModifiedBy.IsForeignKey = false;
                colvarModifiedBy.IsReadOnly = false;
                
                schema.Columns.Add(colvarModifiedBy);
                
                
                BaseSchema = schema;
                //add this schema to the provider
                //so we can query it later
                DataService.Providers["PowerPOS"].AddSchema("ViewQuickAccessGroupMap",schema);
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
	    public ViewQuickAccessGroupMap()
	    {
            SetSQLProps();
            SetDefaults();
            MarkNew();
        }
        public ViewQuickAccessGroupMap(bool useDatabaseDefaults)
	    {
		    SetSQLProps();
		    if(useDatabaseDefaults)
		    {
				ForceDefaults();
			}
			MarkNew();
	    }
	    
	    public ViewQuickAccessGroupMap(object keyID)
	    {
		    SetSQLProps();
		    LoadByKey(keyID);
	    }
    	 
	    public ViewQuickAccessGroupMap(string columnName, object columnValue)
        {
            SetSQLProps();
            LoadByParam(columnName,columnValue);
        }
        
	    #endregion
	    
	    #region Props
	    
          
        [XmlAttribute("QuickAccessGroupName")]
        [Bindable(true)]
        public string QuickAccessGroupName 
	    {
		    get
		    {
			    return GetColumnValue<string>("QuickAccessGroupName");
		    }
            set 
		    {
			    SetColumnValue("QuickAccessGroupName", value);
            }
        }
	      
        [XmlAttribute("QuickAccessGroupMapID")]
        [Bindable(true)]
        public Guid QuickAccessGroupMapID 
	    {
		    get
		    {
			    return GetColumnValue<Guid>("QuickAccessGroupMapID");
		    }
            set 
		    {
			    SetColumnValue("QuickAccessGroupMapID", value);
            }
        }
	      
        [XmlAttribute("QuickAccessCatName")]
        [Bindable(true)]
        public string QuickAccessCatName 
	    {
		    get
		    {
			    return GetColumnValue<string>("QuickAccessCatName");
		    }
            set 
		    {
			    SetColumnValue("QuickAccessCatName", value);
            }
        }
	      
        [XmlAttribute("QuickAccessGroupID")]
        [Bindable(true)]
        public Guid QuickAccessGroupID 
	    {
		    get
		    {
			    return GetColumnValue<Guid>("QuickAccessGroupID");
		    }
            set 
		    {
			    SetColumnValue("QuickAccessGroupID", value);
            }
        }
	      
        [XmlAttribute("QuickAccessCategoryID")]
        [Bindable(true)]
        public Guid QuickAccessCategoryID 
	    {
		    get
		    {
			    return GetColumnValue<Guid>("QuickAccessCategoryID");
		    }
            set 
		    {
			    SetColumnValue("QuickAccessCategoryID", value);
            }
        }
	      
        [XmlAttribute("CreatedOn")]
        [Bindable(true)]
        public DateTime? CreatedOn 
	    {
		    get
		    {
			    return GetColumnValue<DateTime?>("CreatedOn");
		    }
            set 
		    {
			    SetColumnValue("CreatedOn", value);
            }
        }
	      
        [XmlAttribute("CreatedBy")]
        [Bindable(true)]
        public string CreatedBy 
	    {
		    get
		    {
			    return GetColumnValue<string>("CreatedBy");
		    }
            set 
		    {
			    SetColumnValue("CreatedBy", value);
            }
        }
	      
        [XmlAttribute("ModifiedOn")]
        [Bindable(true)]
        public DateTime? ModifiedOn 
	    {
		    get
		    {
			    return GetColumnValue<DateTime?>("ModifiedOn");
		    }
            set 
		    {
			    SetColumnValue("ModifiedOn", value);
            }
        }
	      
        [XmlAttribute("ModifiedBy")]
        [Bindable(true)]
        public string ModifiedBy 
	    {
		    get
		    {
			    return GetColumnValue<string>("ModifiedBy");
		    }
            set 
		    {
			    SetColumnValue("ModifiedBy", value);
            }
        }
	    
	    #endregion
    
	    #region Columns Struct
	    public struct Columns
	    {
		    
		    
            public static string QuickAccessGroupName = @"QuickAccessGroupName";
            
            public static string QuickAccessGroupMapID = @"QuickAccessGroupMapID";
            
            public static string QuickAccessCatName = @"QuickAccessCatName";
            
            public static string QuickAccessGroupID = @"QuickAccessGroupID";
            
            public static string QuickAccessCategoryID = @"QuickAccessCategoryID";
            
            public static string CreatedOn = @"CreatedOn";
            
            public static string CreatedBy = @"CreatedBy";
            
            public static string ModifiedOn = @"ModifiedOn";
            
            public static string ModifiedBy = @"ModifiedBy";
            
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
