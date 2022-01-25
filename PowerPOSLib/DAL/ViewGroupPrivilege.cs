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
    /// Strongly-typed collection for the ViewGroupPrivilege class.
    /// </summary>
    [Serializable]
    public partial class ViewGroupPrivilegeCollection : ReadOnlyList<ViewGroupPrivilege, ViewGroupPrivilegeCollection>
    {        
        public ViewGroupPrivilegeCollection() {}
    }
    /// <summary>
    /// This is  Read-only wrapper class for the ViewGroupPrivileges view.
    /// </summary>
    [Serializable]
    public partial class ViewGroupPrivilege : ReadOnlyRecord<ViewGroupPrivilege>, IReadOnlyRecord
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
                TableSchema.Table schema = new TableSchema.Table("ViewGroupPrivileges", TableType.View, DataService.GetInstance("PowerPOS"));
                schema.Columns = new TableSchema.TableColumnCollection();
                schema.SchemaName = @"dbo";
                //columns
                
                TableSchema.TableColumn colvarGroupName = new TableSchema.TableColumn(schema);
                colvarGroupName.ColumnName = "GroupName";
                colvarGroupName.DataType = DbType.AnsiString;
                colvarGroupName.MaxLength = 50;
                colvarGroupName.AutoIncrement = false;
                colvarGroupName.IsNullable = false;
                colvarGroupName.IsPrimaryKey = false;
                colvarGroupName.IsForeignKey = false;
                colvarGroupName.IsReadOnly = false;
                
                schema.Columns.Add(colvarGroupName);
                
                TableSchema.TableColumn colvarPrivilegeName = new TableSchema.TableColumn(schema);
                colvarPrivilegeName.ColumnName = "PrivilegeName";
                colvarPrivilegeName.DataType = DbType.AnsiString;
                colvarPrivilegeName.MaxLength = 50;
                colvarPrivilegeName.AutoIncrement = false;
                colvarPrivilegeName.IsNullable = false;
                colvarPrivilegeName.IsPrimaryKey = false;
                colvarPrivilegeName.IsForeignKey = false;
                colvarPrivilegeName.IsReadOnly = false;
                
                schema.Columns.Add(colvarPrivilegeName);
                
                TableSchema.TableColumn colvarFormName = new TableSchema.TableColumn(schema);
                colvarFormName.ColumnName = "FormName";
                colvarFormName.DataType = DbType.AnsiString;
                colvarFormName.MaxLength = 50;
                colvarFormName.AutoIncrement = false;
                colvarFormName.IsNullable = true;
                colvarFormName.IsPrimaryKey = false;
                colvarFormName.IsForeignKey = false;
                colvarFormName.IsReadOnly = false;
                
                schema.Columns.Add(colvarFormName);
                
                TableSchema.TableColumn colvarGroupDescription = new TableSchema.TableColumn(schema);
                colvarGroupDescription.ColumnName = "GroupDescription";
                colvarGroupDescription.DataType = DbType.AnsiString;
                colvarGroupDescription.MaxLength = 250;
                colvarGroupDescription.AutoIncrement = false;
                colvarGroupDescription.IsNullable = true;
                colvarGroupDescription.IsPrimaryKey = false;
                colvarGroupDescription.IsForeignKey = false;
                colvarGroupDescription.IsReadOnly = false;
                
                schema.Columns.Add(colvarGroupDescription);
                
                
                BaseSchema = schema;
                //add this schema to the provider
                //so we can query it later
                DataService.Providers["PowerPOS"].AddSchema("ViewGroupPrivileges",schema);
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
	    public ViewGroupPrivilege()
	    {
            SetSQLProps();
            SetDefaults();
            MarkNew();
        }
        public ViewGroupPrivilege(bool useDatabaseDefaults)
	    {
		    SetSQLProps();
		    if(useDatabaseDefaults)
		    {
				ForceDefaults();
			}
			MarkNew();
	    }
	    
	    public ViewGroupPrivilege(object keyID)
	    {
		    SetSQLProps();
		    LoadByKey(keyID);
	    }
    	 
	    public ViewGroupPrivilege(string columnName, object columnValue)
        {
            SetSQLProps();
            LoadByParam(columnName,columnValue);
        }
        
	    #endregion
	    
	    #region Props
	    
          
        [XmlAttribute("GroupName")]
        [Bindable(true)]
        public string GroupName 
	    {
		    get
		    {
			    return GetColumnValue<string>("GroupName");
		    }
            set 
		    {
			    SetColumnValue("GroupName", value);
            }
        }
	      
        [XmlAttribute("PrivilegeName")]
        [Bindable(true)]
        public string PrivilegeName 
	    {
		    get
		    {
			    return GetColumnValue<string>("PrivilegeName");
		    }
            set 
		    {
			    SetColumnValue("PrivilegeName", value);
            }
        }
	      
        [XmlAttribute("FormName")]
        [Bindable(true)]
        public string FormName 
	    {
		    get
		    {
			    return GetColumnValue<string>("FormName");
		    }
            set 
		    {
			    SetColumnValue("FormName", value);
            }
        }
	      
        [XmlAttribute("GroupDescription")]
        [Bindable(true)]
        public string GroupDescription 
	    {
		    get
		    {
			    return GetColumnValue<string>("GroupDescription");
		    }
            set 
		    {
			    SetColumnValue("GroupDescription", value);
            }
        }
	    
	    #endregion
    
	    #region Columns Struct
	    public struct Columns
	    {
		    
		    
            public static string GroupName = @"GroupName";
            
            public static string PrivilegeName = @"PrivilegeName";
            
            public static string FormName = @"FormName";
            
            public static string GroupDescription = @"GroupDescription";
            
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
