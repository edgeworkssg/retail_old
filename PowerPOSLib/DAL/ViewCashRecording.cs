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
    /// Strongly-typed collection for the ViewCashRecording class.
    /// </summary>
    [Serializable]
    public partial class ViewCashRecordingCollection : ReadOnlyList<ViewCashRecording, ViewCashRecordingCollection>
    {        
        public ViewCashRecordingCollection() {}
    }
    /// <summary>
    /// This is  Read-only wrapper class for the ViewCashRecording view.
    /// </summary>
    [Serializable]
    public partial class ViewCashRecording : ReadOnlyRecord<ViewCashRecording>, IReadOnlyRecord
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
                TableSchema.Table schema = new TableSchema.Table("ViewCashRecording", TableType.View, DataService.GetInstance("PowerPOS"));
                schema.Columns = new TableSchema.TableColumnCollection();
                schema.SchemaName = @"dbo";
                //columns
                
                TableSchema.TableColumn colvarCashRecordingTime = new TableSchema.TableColumn(schema);
                colvarCashRecordingTime.ColumnName = "CashRecordingTime";
                colvarCashRecordingTime.DataType = DbType.DateTime;
                colvarCashRecordingTime.MaxLength = 0;
                colvarCashRecordingTime.AutoIncrement = false;
                colvarCashRecordingTime.IsNullable = false;
                colvarCashRecordingTime.IsPrimaryKey = false;
                colvarCashRecordingTime.IsForeignKey = false;
                colvarCashRecordingTime.IsReadOnly = false;
                
                schema.Columns.Add(colvarCashRecordingTime);
                
                TableSchema.TableColumn colvarCashRecRefNo = new TableSchema.TableColumn(schema);
                colvarCashRecRefNo.ColumnName = "CashRecRefNo";
                colvarCashRecRefNo.DataType = DbType.AnsiString;
                colvarCashRecRefNo.MaxLength = 50;
                colvarCashRecRefNo.AutoIncrement = false;
                colvarCashRecRefNo.IsNullable = false;
                colvarCashRecRefNo.IsPrimaryKey = false;
                colvarCashRecRefNo.IsForeignKey = false;
                colvarCashRecRefNo.IsReadOnly = false;
                
                schema.Columns.Add(colvarCashRecRefNo);
                
                TableSchema.TableColumn colvarCashRecordingTypeName = new TableSchema.TableColumn(schema);
                colvarCashRecordingTypeName.ColumnName = "CashRecordingTypeName";
                colvarCashRecordingTypeName.DataType = DbType.AnsiString;
                colvarCashRecordingTypeName.MaxLength = 50;
                colvarCashRecordingTypeName.AutoIncrement = false;
                colvarCashRecordingTypeName.IsNullable = false;
                colvarCashRecordingTypeName.IsPrimaryKey = false;
                colvarCashRecordingTypeName.IsForeignKey = false;
                colvarCashRecordingTypeName.IsReadOnly = false;
                
                schema.Columns.Add(colvarCashRecordingTypeName);
                
                TableSchema.TableColumn colvarAmount = new TableSchema.TableColumn(schema);
                colvarAmount.ColumnName = "amount";
                colvarAmount.DataType = DbType.Currency;
                colvarAmount.MaxLength = 0;
                colvarAmount.AutoIncrement = false;
                colvarAmount.IsNullable = false;
                colvarAmount.IsPrimaryKey = false;
                colvarAmount.IsForeignKey = false;
                colvarAmount.IsReadOnly = false;
                
                schema.Columns.Add(colvarAmount);
                
                TableSchema.TableColumn colvarCashierName = new TableSchema.TableColumn(schema);
                colvarCashierName.ColumnName = "CashierName";
                colvarCashierName.DataType = DbType.AnsiString;
                colvarCashierName.MaxLength = 50;
                colvarCashierName.AutoIncrement = false;
                colvarCashierName.IsNullable = false;
                colvarCashierName.IsPrimaryKey = false;
                colvarCashierName.IsForeignKey = false;
                colvarCashierName.IsReadOnly = false;
                
                schema.Columns.Add(colvarCashierName);
                
                TableSchema.TableColumn colvarSupervisorName = new TableSchema.TableColumn(schema);
                colvarSupervisorName.ColumnName = "SupervisorName";
                colvarSupervisorName.DataType = DbType.AnsiString;
                colvarSupervisorName.MaxLength = 50;
                colvarSupervisorName.AutoIncrement = false;
                colvarSupervisorName.IsNullable = false;
                colvarSupervisorName.IsPrimaryKey = false;
                colvarSupervisorName.IsForeignKey = false;
                colvarSupervisorName.IsReadOnly = false;
                
                schema.Columns.Add(colvarSupervisorName);
                
                TableSchema.TableColumn colvarRemark = new TableSchema.TableColumn(schema);
                colvarRemark.ColumnName = "Remark";
                colvarRemark.DataType = DbType.AnsiString;
                colvarRemark.MaxLength = 250;
                colvarRemark.AutoIncrement = false;
                colvarRemark.IsNullable = true;
                colvarRemark.IsPrimaryKey = false;
                colvarRemark.IsForeignKey = false;
                colvarRemark.IsReadOnly = false;
                
                schema.Columns.Add(colvarRemark);
                
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
                DataService.Providers["PowerPOS"].AddSchema("ViewCashRecording",schema);
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
	    public ViewCashRecording()
	    {
            SetSQLProps();
            SetDefaults();
            MarkNew();
        }
        public ViewCashRecording(bool useDatabaseDefaults)
	    {
		    SetSQLProps();
		    if(useDatabaseDefaults)
		    {
				ForceDefaults();
			}
			MarkNew();
	    }
	    
	    public ViewCashRecording(object keyID)
	    {
		    SetSQLProps();
		    LoadByKey(keyID);
	    }
    	 
	    public ViewCashRecording(string columnName, object columnValue)
        {
            SetSQLProps();
            LoadByParam(columnName,columnValue);
        }
        
	    #endregion
	    
	    #region Props
	    
          
        [XmlAttribute("CashRecordingTime")]
        [Bindable(true)]
        public DateTime CashRecordingTime 
	    {
		    get
		    {
			    return GetColumnValue<DateTime>("CashRecordingTime");
		    }
            set 
		    {
			    SetColumnValue("CashRecordingTime", value);
            }
        }
	      
        [XmlAttribute("CashRecRefNo")]
        [Bindable(true)]
        public string CashRecRefNo 
	    {
		    get
		    {
			    return GetColumnValue<string>("CashRecRefNo");
		    }
            set 
		    {
			    SetColumnValue("CashRecRefNo", value);
            }
        }
	      
        [XmlAttribute("CashRecordingTypeName")]
        [Bindable(true)]
        public string CashRecordingTypeName 
	    {
		    get
		    {
			    return GetColumnValue<string>("CashRecordingTypeName");
		    }
            set 
		    {
			    SetColumnValue("CashRecordingTypeName", value);
            }
        }
	      
        [XmlAttribute("Amount")]
        [Bindable(true)]
        public decimal Amount 
	    {
		    get
		    {
			    return GetColumnValue<decimal>("amount");
		    }
            set 
		    {
			    SetColumnValue("amount", value);
            }
        }
	      
        [XmlAttribute("CashierName")]
        [Bindable(true)]
        public string CashierName 
	    {
		    get
		    {
			    return GetColumnValue<string>("CashierName");
		    }
            set 
		    {
			    SetColumnValue("CashierName", value);
            }
        }
	      
        [XmlAttribute("SupervisorName")]
        [Bindable(true)]
        public string SupervisorName 
	    {
		    get
		    {
			    return GetColumnValue<string>("SupervisorName");
		    }
            set 
		    {
			    SetColumnValue("SupervisorName", value);
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
		    
		    
            public static string CashRecordingTime = @"CashRecordingTime";
            
            public static string CashRecRefNo = @"CashRecRefNo";
            
            public static string CashRecordingTypeName = @"CashRecordingTypeName";
            
            public static string Amount = @"amount";
            
            public static string CashierName = @"CashierName";
            
            public static string SupervisorName = @"SupervisorName";
            
            public static string Remark = @"Remark";
            
            public static string PointOfSaleName = @"PointOfSaleName";
            
            public static string PointOfSaleID = @"PointOfSaleID";
            
            public static string DepartmentId = @"DepartmentId";
            
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
