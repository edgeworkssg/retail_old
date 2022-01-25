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
    /// Strongly-typed collection for the ViewTransactionWithSalesPerson class.
    /// </summary>
    [Serializable]
    public partial class ViewTransactionWithSalesPersonCollection : ReadOnlyList<ViewTransactionWithSalesPerson, ViewTransactionWithSalesPersonCollection>
    {        
        public ViewTransactionWithSalesPersonCollection() {}
    }
    /// <summary>
    /// This is  Read-only wrapper class for the ViewTransactionWithSalesPerson view.
    /// </summary>
    [Serializable]
    public partial class ViewTransactionWithSalesPerson : ReadOnlyRecord<ViewTransactionWithSalesPerson>, IReadOnlyRecord
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
                TableSchema.Table schema = new TableSchema.Table("ViewTransactionWithSalesPerson", TableType.View, DataService.GetInstance("PowerPOS"));
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
                
                TableSchema.TableColumn colvarAmount = new TableSchema.TableColumn(schema);
                colvarAmount.ColumnName = "Amount";
                colvarAmount.DataType = DbType.Currency;
                colvarAmount.MaxLength = 0;
                colvarAmount.AutoIncrement = false;
                colvarAmount.IsNullable = false;
                colvarAmount.IsPrimaryKey = false;
                colvarAmount.IsForeignKey = false;
                colvarAmount.IsReadOnly = false;
                
                schema.Columns.Add(colvarAmount);
                
                TableSchema.TableColumn colvarOrderDate = new TableSchema.TableColumn(schema);
                colvarOrderDate.ColumnName = "OrderDate";
                colvarOrderDate.DataType = DbType.DateTime;
                colvarOrderDate.MaxLength = 0;
                colvarOrderDate.AutoIncrement = false;
                colvarOrderDate.IsNullable = false;
                colvarOrderDate.IsPrimaryKey = false;
                colvarOrderDate.IsForeignKey = false;
                colvarOrderDate.IsReadOnly = false;
                
                schema.Columns.Add(colvarOrderDate);
                
                TableSchema.TableColumn colvarOrderRefNo = new TableSchema.TableColumn(schema);
                colvarOrderRefNo.ColumnName = "OrderRefNo";
                colvarOrderRefNo.DataType = DbType.AnsiString;
                colvarOrderRefNo.MaxLength = 50;
                colvarOrderRefNo.AutoIncrement = false;
                colvarOrderRefNo.IsNullable = false;
                colvarOrderRefNo.IsPrimaryKey = false;
                colvarOrderRefNo.IsForeignKey = false;
                colvarOrderRefNo.IsReadOnly = false;
                
                schema.Columns.Add(colvarOrderRefNo);
                
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
                
                TableSchema.TableColumn colvarCashierID = new TableSchema.TableColumn(schema);
                colvarCashierID.ColumnName = "CashierID";
                colvarCashierID.DataType = DbType.AnsiString;
                colvarCashierID.MaxLength = 50;
                colvarCashierID.AutoIncrement = false;
                colvarCashierID.IsNullable = false;
                colvarCashierID.IsPrimaryKey = false;
                colvarCashierID.IsForeignKey = false;
                colvarCashierID.IsReadOnly = false;
                
                schema.Columns.Add(colvarCashierID);
                
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
                
                TableSchema.TableColumn colvarOrderHdrID = new TableSchema.TableColumn(schema);
                colvarOrderHdrID.ColumnName = "OrderHdrID";
                colvarOrderHdrID.DataType = DbType.AnsiString;
                colvarOrderHdrID.MaxLength = 14;
                colvarOrderHdrID.AutoIncrement = false;
                colvarOrderHdrID.IsNullable = false;
                colvarOrderHdrID.IsPrimaryKey = false;
                colvarOrderHdrID.IsForeignKey = false;
                colvarOrderHdrID.IsReadOnly = false;
                
                schema.Columns.Add(colvarOrderHdrID);
                
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
                DataService.Providers["PowerPOS"].AddSchema("ViewTransactionWithSalesPerson",schema);
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
	    public ViewTransactionWithSalesPerson()
	    {
            SetSQLProps();
            SetDefaults();
            MarkNew();
        }
        public ViewTransactionWithSalesPerson(bool useDatabaseDefaults)
	    {
		    SetSQLProps();
		    if(useDatabaseDefaults)
		    {
				ForceDefaults();
			}
			MarkNew();
	    }
	    
	    public ViewTransactionWithSalesPerson(object keyID)
	    {
		    SetSQLProps();
		    LoadByKey(keyID);
	    }
    	 
	    public ViewTransactionWithSalesPerson(string columnName, object columnValue)
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
	      
        [XmlAttribute("Amount")]
        [Bindable(true)]
        public decimal Amount 
	    {
		    get
		    {
			    return GetColumnValue<decimal>("Amount");
		    }
            set 
		    {
			    SetColumnValue("Amount", value);
            }
        }
	      
        [XmlAttribute("OrderDate")]
        [Bindable(true)]
        public DateTime OrderDate 
	    {
		    get
		    {
			    return GetColumnValue<DateTime>("OrderDate");
		    }
            set 
		    {
			    SetColumnValue("OrderDate", value);
            }
        }
	      
        [XmlAttribute("OrderRefNo")]
        [Bindable(true)]
        public string OrderRefNo 
	    {
		    get
		    {
			    return GetColumnValue<string>("OrderRefNo");
		    }
            set 
		    {
			    SetColumnValue("OrderRefNo", value);
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
	      
        [XmlAttribute("CashierID")]
        [Bindable(true)]
        public string CashierID 
	    {
		    get
		    {
			    return GetColumnValue<string>("CashierID");
		    }
            set 
		    {
			    SetColumnValue("CashierID", value);
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
	      
        [XmlAttribute("OrderHdrID")]
        [Bindable(true)]
        public string OrderHdrID 
	    {
		    get
		    {
			    return GetColumnValue<string>("OrderHdrID");
		    }
            set 
		    {
			    SetColumnValue("OrderHdrID", value);
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
		    
		    
            public static string GroupName = @"GroupName";
            
            public static string Amount = @"Amount";
            
            public static string OrderDate = @"OrderDate";
            
            public static string OrderRefNo = @"OrderRefNo";
            
            public static string PointOfSaleName = @"PointOfSaleName";
            
            public static string OutletName = @"OutletName";
            
            public static string CashierID = @"CashierID";
            
            public static string PointOfSaleID = @"PointOfSaleID";
            
            public static string OrderHdrID = @"OrderHdrID";
            
            public static string DepartmentID = @"DepartmentID";
            
            public static string UserName = @"UserName";
            
            public static string DisplayName = @"DisplayName";
            
            public static string Remark = @"Remark";
            
            public static string IsASalesPerson = @"IsASalesPerson";
            
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
