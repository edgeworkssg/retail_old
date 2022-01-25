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
    /// Strongly-typed collection for the ViewUser class.
    /// </summary>
    [Serializable]
    public partial class ViewUserCollection : ReadOnlyList<ViewUser, ViewUserCollection>
    {        
        public ViewUserCollection() {}
    }
    /// <summary>
    /// This is  Read-only wrapper class for the ViewUser view.
    /// </summary>
    [Serializable]
    public partial class ViewUser : ReadOnlyRecord<ViewUser>, IReadOnlyRecord
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
                TableSchema.Table schema = new TableSchema.Table("ViewUser", TableType.View, DataService.GetInstance("PowerPOS"));
                schema.Columns = new TableSchema.TableColumnCollection();
                schema.SchemaName = @"dbo";
                //columns
                
                TableSchema.TableColumn colvarUserName = new TableSchema.TableColumn(schema);
                colvarUserName.ColumnName = "UserName";
                colvarUserName.DataType = DbType.AnsiString;
                colvarUserName.MaxLength = 50;
                colvarUserName.AutoIncrement = false;
                colvarUserName.IsNullable = false;
                colvarUserName.IsPrimaryKey = false;
                colvarUserName.IsForeignKey = false;
                colvarUserName.IsReadOnly = false;
                
                schema.Columns.Add(colvarUserName);
                
                TableSchema.TableColumn colvarDisplayName = new TableSchema.TableColumn(schema);
                colvarDisplayName.ColumnName = "DisplayName";
                colvarDisplayName.DataType = DbType.AnsiString;
                colvarDisplayName.MaxLength = 50;
                colvarDisplayName.AutoIncrement = false;
                colvarDisplayName.IsNullable = true;
                colvarDisplayName.IsPrimaryKey = false;
                colvarDisplayName.IsForeignKey = false;
                colvarDisplayName.IsReadOnly = false;
                
                schema.Columns.Add(colvarDisplayName);
                
                TableSchema.TableColumn colvarPassword = new TableSchema.TableColumn(schema);
                colvarPassword.ColumnName = "Password";
                colvarPassword.DataType = DbType.AnsiString;
                colvarPassword.MaxLength = 250;
                colvarPassword.AutoIncrement = false;
                colvarPassword.IsNullable = false;
                colvarPassword.IsPrimaryKey = false;
                colvarPassword.IsForeignKey = false;
                colvarPassword.IsReadOnly = false;
                
                schema.Columns.Add(colvarPassword);
                
                TableSchema.TableColumn colvarRemark = new TableSchema.TableColumn(schema);
                colvarRemark.ColumnName = "Remark";
                colvarRemark.DataType = DbType.String;
                colvarRemark.MaxLength = -1;
                colvarRemark.AutoIncrement = false;
                colvarRemark.IsNullable = true;
                colvarRemark.IsPrimaryKey = false;
                colvarRemark.IsForeignKey = false;
                colvarRemark.IsReadOnly = false;
                
                schema.Columns.Add(colvarRemark);
                
                TableSchema.TableColumn colvarSalesPersonGroupID = new TableSchema.TableColumn(schema);
                colvarSalesPersonGroupID.ColumnName = "SalesPersonGroupID";
                colvarSalesPersonGroupID.DataType = DbType.Int32;
                colvarSalesPersonGroupID.MaxLength = 0;
                colvarSalesPersonGroupID.AutoIncrement = false;
                colvarSalesPersonGroupID.IsNullable = false;
                colvarSalesPersonGroupID.IsPrimaryKey = false;
                colvarSalesPersonGroupID.IsForeignKey = false;
                colvarSalesPersonGroupID.IsReadOnly = false;
                
                schema.Columns.Add(colvarSalesPersonGroupID);
                
                TableSchema.TableColumn colvarDepartmentID = new TableSchema.TableColumn(schema);
                colvarDepartmentID.ColumnName = "DepartmentID";
                colvarDepartmentID.DataType = DbType.Int32;
                colvarDepartmentID.MaxLength = 0;
                colvarDepartmentID.AutoIncrement = false;
                colvarDepartmentID.IsNullable = true;
                colvarDepartmentID.IsPrimaryKey = false;
                colvarDepartmentID.IsForeignKey = false;
                colvarDepartmentID.IsReadOnly = false;
                
                schema.Columns.Add(colvarDepartmentID);
                
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
                
                TableSchema.TableColumn colvarGroupDeleted = new TableSchema.TableColumn(schema);
                colvarGroupDeleted.ColumnName = "GroupDeleted";
                colvarGroupDeleted.DataType = DbType.Boolean;
                colvarGroupDeleted.MaxLength = 0;
                colvarGroupDeleted.AutoIncrement = false;
                colvarGroupDeleted.IsNullable = true;
                colvarGroupDeleted.IsPrimaryKey = false;
                colvarGroupDeleted.IsForeignKey = false;
                colvarGroupDeleted.IsReadOnly = false;
                
                schema.Columns.Add(colvarGroupDeleted);
                
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
                
                TableSchema.TableColumn colvarGroupID = new TableSchema.TableColumn(schema);
                colvarGroupID.ColumnName = "GroupID";
                colvarGroupID.DataType = DbType.Int32;
                colvarGroupID.MaxLength = 0;
                colvarGroupID.AutoIncrement = false;
                colvarGroupID.IsNullable = false;
                colvarGroupID.IsPrimaryKey = false;
                colvarGroupID.IsForeignKey = false;
                colvarGroupID.IsReadOnly = false;
                
                schema.Columns.Add(colvarGroupID);
                
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
                
                TableSchema.TableColumn colvarSalesPersonGroupName = new TableSchema.TableColumn(schema);
                colvarSalesPersonGroupName.ColumnName = "SalesPersonGroupName";
                colvarSalesPersonGroupName.DataType = DbType.AnsiString;
                colvarSalesPersonGroupName.MaxLength = 50;
                colvarSalesPersonGroupName.AutoIncrement = false;
                colvarSalesPersonGroupName.IsNullable = false;
                colvarSalesPersonGroupName.IsPrimaryKey = false;
                colvarSalesPersonGroupName.IsForeignKey = false;
                colvarSalesPersonGroupName.IsReadOnly = false;
                
                schema.Columns.Add(colvarSalesPersonGroupName);
                
                TableSchema.TableColumn colvarIsASalesPerson = new TableSchema.TableColumn(schema);
                colvarIsASalesPerson.ColumnName = "IsASalesPerson";
                colvarIsASalesPerson.DataType = DbType.Boolean;
                colvarIsASalesPerson.MaxLength = 0;
                colvarIsASalesPerson.AutoIncrement = false;
                colvarIsASalesPerson.IsNullable = false;
                colvarIsASalesPerson.IsPrimaryKey = false;
                colvarIsASalesPerson.IsForeignKey = false;
                colvarIsASalesPerson.IsReadOnly = false;
                
                schema.Columns.Add(colvarIsASalesPerson);
                
                
                BaseSchema = schema;
                //add this schema to the provider
                //so we can query it later
                DataService.Providers["PowerPOS"].AddSchema("ViewUser",schema);
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
	    public ViewUser()
	    {
            SetSQLProps();
            SetDefaults();
            MarkNew();
        }
        public ViewUser(bool useDatabaseDefaults)
	    {
		    SetSQLProps();
		    if(useDatabaseDefaults)
		    {
				ForceDefaults();
			}
			MarkNew();
	    }
	    
	    public ViewUser(object keyID)
	    {
		    SetSQLProps();
		    LoadByKey(keyID);
	    }
    	 
	    public ViewUser(string columnName, object columnValue)
        {
            SetSQLProps();
            LoadByParam(columnName,columnValue);
        }
        
	    #endregion
	    
	    #region Props
	    
          
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
	      
        [XmlAttribute("DisplayName")]
        [Bindable(true)]
        public string DisplayName 
	    {
		    get
		    {
			    return GetColumnValue<string>("DisplayName");
		    }
            set 
		    {
			    SetColumnValue("DisplayName", value);
            }
        }
	      
        [XmlAttribute("Password")]
        [Bindable(true)]
        public string Password 
	    {
		    get
		    {
			    return GetColumnValue<string>("Password");
		    }
            set 
		    {
			    SetColumnValue("Password", value);
            }
        }
	      
        [XmlAttribute("Remark")]
        [Bindable(true)]
        public string Remark 
	    {
		    get
		    {
			    return GetColumnValue<string>("Remark");
		    }
            set 
		    {
			    SetColumnValue("Remark", value);
            }
        }
	      
        [XmlAttribute("SalesPersonGroupID")]
        [Bindable(true)]
        public int SalesPersonGroupID 
	    {
		    get
		    {
			    return GetColumnValue<int>("SalesPersonGroupID");
		    }
            set 
		    {
			    SetColumnValue("SalesPersonGroupID", value);
            }
        }
	      
        [XmlAttribute("DepartmentID")]
        [Bindable(true)]
        public int? DepartmentID 
	    {
		    get
		    {
			    return GetColumnValue<int?>("DepartmentID");
		    }
            set 
		    {
			    SetColumnValue("DepartmentID", value);
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
	      
        [XmlAttribute("GroupDeleted")]
        [Bindable(true)]
        public bool? GroupDeleted 
	    {
		    get
		    {
			    return GetColumnValue<bool?>("GroupDeleted");
		    }
            set 
		    {
			    SetColumnValue("GroupDeleted", value);
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
	      
        [XmlAttribute("GroupID")]
        [Bindable(true)]
        public int GroupID 
	    {
		    get
		    {
			    return GetColumnValue<int>("GroupID");
		    }
            set 
		    {
			    SetColumnValue("GroupID", value);
            }
        }
	      
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
	      
        [XmlAttribute("SalesPersonGroupName")]
        [Bindable(true)]
        public string SalesPersonGroupName 
	    {
		    get
		    {
			    return GetColumnValue<string>("SalesPersonGroupName");
		    }
            set 
		    {
			    SetColumnValue("SalesPersonGroupName", value);
            }
        }
	      
        [XmlAttribute("IsASalesPerson")]
        [Bindable(true)]
        public bool IsASalesPerson 
	    {
		    get
		    {
			    return GetColumnValue<bool>("IsASalesPerson");
		    }
            set 
		    {
			    SetColumnValue("IsASalesPerson", value);
            }
        }
	    
	    #endregion
    
	    #region Columns Struct
	    public struct Columns
	    {
		    
		    
            public static string UserName = @"UserName";
            
            public static string DisplayName = @"DisplayName";
            
            public static string Password = @"Password";
            
            public static string Remark = @"Remark";
            
            public static string SalesPersonGroupID = @"SalesPersonGroupID";
            
            public static string DepartmentID = @"DepartmentID";
            
            public static string GroupDescription = @"GroupDescription";
            
            public static string GroupDeleted = @"GroupDeleted";
            
            public static string Deleted = @"Deleted";
            
            public static string GroupID = @"GroupID";
            
            public static string GroupName = @"GroupName";
            
            public static string SalesPersonGroupName = @"SalesPersonGroupName";
            
            public static string IsASalesPerson = @"IsASalesPerson";
            
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
