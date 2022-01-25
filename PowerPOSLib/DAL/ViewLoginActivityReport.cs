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
    /// Strongly-typed collection for the ViewLoginActivityReport class.
    /// </summary>
    [Serializable]
    public partial class ViewLoginActivityReportCollection : ReadOnlyList<ViewLoginActivityReport, ViewLoginActivityReportCollection>
    {        
        public ViewLoginActivityReportCollection() {}
    }
    /// <summary>
    /// This is  Read-only wrapper class for the ViewLoginActivityReport view.
    /// </summary>
    [Serializable]
    public partial class ViewLoginActivityReport : ReadOnlyRecord<ViewLoginActivityReport>, IReadOnlyRecord
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
                TableSchema.Table schema = new TableSchema.Table("ViewLoginActivityReport", TableType.View, DataService.GetInstance("PowerPOS"));
                schema.Columns = new TableSchema.TableColumnCollection();
                schema.SchemaName = @"dbo";
                //columns
                
                TableSchema.TableColumn colvarLoginActivityID = new TableSchema.TableColumn(schema);
                colvarLoginActivityID.ColumnName = "LoginActivityID";
                colvarLoginActivityID.DataType = DbType.Guid;
                colvarLoginActivityID.MaxLength = 0;
                colvarLoginActivityID.AutoIncrement = false;
                colvarLoginActivityID.IsNullable = false;
                colvarLoginActivityID.IsPrimaryKey = false;
                colvarLoginActivityID.IsForeignKey = false;
                colvarLoginActivityID.IsReadOnly = false;
                
                schema.Columns.Add(colvarLoginActivityID);
                
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
                
                TableSchema.TableColumn colvarLoginType = new TableSchema.TableColumn(schema);
                colvarLoginType.ColumnName = "LoginType";
                colvarLoginType.DataType = DbType.AnsiString;
                colvarLoginType.MaxLength = 50;
                colvarLoginType.AutoIncrement = false;
                colvarLoginType.IsNullable = false;
                colvarLoginType.IsPrimaryKey = false;
                colvarLoginType.IsForeignKey = false;
                colvarLoginType.IsReadOnly = false;
                
                schema.Columns.Add(colvarLoginType);
                
                TableSchema.TableColumn colvarLoginDateTime = new TableSchema.TableColumn(schema);
                colvarLoginDateTime.ColumnName = "LoginDateTime";
                colvarLoginDateTime.DataType = DbType.DateTime;
                colvarLoginDateTime.MaxLength = 0;
                colvarLoginDateTime.AutoIncrement = false;
                colvarLoginDateTime.IsNullable = false;
                colvarLoginDateTime.IsPrimaryKey = false;
                colvarLoginDateTime.IsForeignKey = false;
                colvarLoginDateTime.IsReadOnly = false;
                
                schema.Columns.Add(colvarLoginDateTime);
                
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
                
                TableSchema.TableColumn colvarDepartmentId = new TableSchema.TableColumn(schema);
                colvarDepartmentId.ColumnName = "DepartmentId";
                colvarDepartmentId.DataType = DbType.Int32;
                colvarDepartmentId.MaxLength = 0;
                colvarDepartmentId.AutoIncrement = false;
                colvarDepartmentId.IsNullable = true;
                colvarDepartmentId.IsPrimaryKey = false;
                colvarDepartmentId.IsForeignKey = false;
                colvarDepartmentId.IsReadOnly = false;
                
                schema.Columns.Add(colvarDepartmentId);
                
                
                BaseSchema = schema;
                //add this schema to the provider
                //so we can query it later
                DataService.Providers["PowerPOS"].AddSchema("ViewLoginActivityReport",schema);
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
	    public ViewLoginActivityReport()
	    {
            SetSQLProps();
            SetDefaults();
            MarkNew();
        }
        public ViewLoginActivityReport(bool useDatabaseDefaults)
	    {
		    SetSQLProps();
		    if(useDatabaseDefaults)
		    {
				ForceDefaults();
			}
			MarkNew();
	    }
	    
	    public ViewLoginActivityReport(object keyID)
	    {
		    SetSQLProps();
		    LoadByKey(keyID);
	    }
    	 
	    public ViewLoginActivityReport(string columnName, object columnValue)
        {
            SetSQLProps();
            LoadByParam(columnName,columnValue);
        }
        
	    #endregion
	    
	    #region Props
	    
          
        [XmlAttribute("LoginActivityID")]
        [Bindable(true)]
        public Guid LoginActivityID 
	    {
		    get
		    {
			    return GetColumnValue<Guid>("LoginActivityID");
		    }
            set 
		    {
			    SetColumnValue("LoginActivityID", value);
            }
        }
	      
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
	      
        [XmlAttribute("LoginType")]
        [Bindable(true)]
        public string LoginType 
	    {
		    get
		    {
			    return GetColumnValue<string>("LoginType");
		    }
            set 
		    {
			    SetColumnValue("LoginType", value);
            }
        }
	      
        [XmlAttribute("LoginDateTime")]
        [Bindable(true)]
        public DateTime LoginDateTime 
	    {
		    get
		    {
			    return GetColumnValue<DateTime>("LoginDateTime");
		    }
            set 
		    {
			    SetColumnValue("LoginDateTime", value);
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
	      
        [XmlAttribute("DepartmentId")]
        [Bindable(true)]
        public int? DepartmentId 
	    {
		    get
		    {
			    return GetColumnValue<int?>("DepartmentId");
		    }
            set 
		    {
			    SetColumnValue("DepartmentId", value);
            }
        }
	    
	    #endregion
    
	    #region Columns Struct
	    public struct Columns
	    {
		    
		    
            public static string LoginActivityID = @"LoginActivityID";
            
            public static string UserName = @"UserName";
            
            public static string LoginType = @"LoginType";
            
            public static string LoginDateTime = @"LoginDateTime";
            
            public static string PointOfSaleName = @"PointOfSaleName";
            
            public static string PointOfSaleID = @"PointOfSaleID";
            
            public static string OutletName = @"OutletName";
            
            public static string DepartmentId = @"DepartmentId";
            
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
