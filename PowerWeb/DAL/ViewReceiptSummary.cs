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
    /// Strongly-typed collection for the ViewReceiptSummary class.
    /// </summary>
    [Serializable]
    public partial class ViewReceiptSummaryCollection : ReadOnlyList<ViewReceiptSummary, ViewReceiptSummaryCollection>
    {        
        public ViewReceiptSummaryCollection() {}
    }
    /// <summary>
    /// This is  Read-only wrapper class for the ViewReceiptSummary view.
    /// </summary>
    [Serializable]
    public partial class ViewReceiptSummary : ReadOnlyRecord<ViewReceiptSummary>, IReadOnlyRecord
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
                TableSchema.Table schema = new TableSchema.Table("ViewReceiptSummary", TableType.View, DataService.GetInstance("PowerPOS"));
                schema.Columns = new TableSchema.TableColumnCollection();
                schema.SchemaName = @"dbo";
                //columns
                
                TableSchema.TableColumn colvarReceiptDate = new TableSchema.TableColumn(schema);
                colvarReceiptDate.ColumnName = "ReceiptDate";
                colvarReceiptDate.DataType = DbType.DateTime;
                colvarReceiptDate.MaxLength = 0;
                colvarReceiptDate.AutoIncrement = false;
                colvarReceiptDate.IsNullable = false;
                colvarReceiptDate.IsPrimaryKey = false;
                colvarReceiptDate.IsForeignKey = false;
                colvarReceiptDate.IsReadOnly = false;
                
                schema.Columns.Add(colvarReceiptDate);
                
                TableSchema.TableColumn colvarReceiptRefNo = new TableSchema.TableColumn(schema);
                colvarReceiptRefNo.ColumnName = "ReceiptRefNo";
                colvarReceiptRefNo.DataType = DbType.AnsiString;
                colvarReceiptRefNo.MaxLength = 50;
                colvarReceiptRefNo.AutoIncrement = false;
                colvarReceiptRefNo.IsNullable = false;
                colvarReceiptRefNo.IsPrimaryKey = false;
                colvarReceiptRefNo.IsForeignKey = false;
                colvarReceiptRefNo.IsReadOnly = false;
                
                schema.Columns.Add(colvarReceiptRefNo);
                
                TableSchema.TableColumn colvarIsVoided = new TableSchema.TableColumn(schema);
                colvarIsVoided.ColumnName = "IsVoided";
                colvarIsVoided.DataType = DbType.Boolean;
                colvarIsVoided.MaxLength = 0;
                colvarIsVoided.AutoIncrement = false;
                colvarIsVoided.IsNullable = false;
                colvarIsVoided.IsPrimaryKey = false;
                colvarIsVoided.IsForeignKey = false;
                colvarIsVoided.IsReadOnly = false;
                
                schema.Columns.Add(colvarIsVoided);
                
                TableSchema.TableColumn colvarTotalReceiptAmount = new TableSchema.TableColumn(schema);
                colvarTotalReceiptAmount.ColumnName = "TotalReceiptAmount";
                colvarTotalReceiptAmount.DataType = DbType.Currency;
                colvarTotalReceiptAmount.MaxLength = 0;
                colvarTotalReceiptAmount.AutoIncrement = false;
                colvarTotalReceiptAmount.IsNullable = true;
                colvarTotalReceiptAmount.IsPrimaryKey = false;
                colvarTotalReceiptAmount.IsForeignKey = false;
                colvarTotalReceiptAmount.IsReadOnly = false;
                
                schema.Columns.Add(colvarTotalReceiptAmount);
                
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
                
                TableSchema.TableColumn colvarReceiptHdrID = new TableSchema.TableColumn(schema);
                colvarReceiptHdrID.ColumnName = "ReceiptHdrID";
                colvarReceiptHdrID.DataType = DbType.AnsiString;
                colvarReceiptHdrID.MaxLength = 14;
                colvarReceiptHdrID.AutoIncrement = false;
                colvarReceiptHdrID.IsNullable = false;
                colvarReceiptHdrID.IsPrimaryKey = false;
                colvarReceiptHdrID.IsForeignKey = false;
                colvarReceiptHdrID.IsReadOnly = false;
                
                schema.Columns.Add(colvarReceiptHdrID);
                
                
                BaseSchema = schema;
                //add this schema to the provider
                //so we can query it later
                DataService.Providers["PowerPOS"].AddSchema("ViewReceiptSummary",schema);
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
	    public ViewReceiptSummary()
	    {
            SetSQLProps();
            SetDefaults();
            MarkNew();
        }
        public ViewReceiptSummary(bool useDatabaseDefaults)
	    {
		    SetSQLProps();
		    if(useDatabaseDefaults)
		    {
				ForceDefaults();
			}
			MarkNew();
	    }
	    
	    public ViewReceiptSummary(object keyID)
	    {
		    SetSQLProps();
		    LoadByKey(keyID);
	    }
    	 
	    public ViewReceiptSummary(string columnName, object columnValue)
        {
            SetSQLProps();
            LoadByParam(columnName,columnValue);
        }
        
	    #endregion
	    
	    #region Props
	    
          
        [XmlAttribute("ReceiptDate")]
        [Bindable(true)]
        public DateTime ReceiptDate 
	    {
		    get
		    {
			    return GetColumnValue<DateTime>("ReceiptDate");
		    }
            set 
		    {
			    SetColumnValue("ReceiptDate", value);
            }
        }
	      
        [XmlAttribute("ReceiptRefNo")]
        [Bindable(true)]
        public string ReceiptRefNo 
	    {
		    get
		    {
			    return GetColumnValue<string>("ReceiptRefNo");
		    }
            set 
		    {
			    SetColumnValue("ReceiptRefNo", value);
            }
        }
	      
        [XmlAttribute("IsVoided")]
        [Bindable(true)]
        public bool IsVoided 
	    {
		    get
		    {
			    return GetColumnValue<bool>("IsVoided");
		    }
            set 
		    {
			    SetColumnValue("IsVoided", value);
            }
        }
	      
        [XmlAttribute("TotalReceiptAmount")]
        [Bindable(true)]
        public decimal? TotalReceiptAmount 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("TotalReceiptAmount");
		    }
            set 
		    {
			    SetColumnValue("TotalReceiptAmount", value);
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
	      
        [XmlAttribute("ReceiptHdrID")]
        [Bindable(true)]
        public string ReceiptHdrID 
	    {
		    get
		    {
			    return GetColumnValue<string>("ReceiptHdrID");
		    }
            set 
		    {
			    SetColumnValue("ReceiptHdrID", value);
            }
        }
	    
	    #endregion
    
	    #region Columns Struct
	    public struct Columns
	    {
		    
		    
            public static string ReceiptDate = @"ReceiptDate";
            
            public static string ReceiptRefNo = @"ReceiptRefNo";
            
            public static string IsVoided = @"IsVoided";
            
            public static string TotalReceiptAmount = @"TotalReceiptAmount";
            
            public static string CashierID = @"CashierID";
            
            public static string ReceiptHdrID = @"ReceiptHdrID";
            
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
