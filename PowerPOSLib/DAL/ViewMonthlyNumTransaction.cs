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
    /// Strongly-typed collection for the ViewMonthlyNumTransaction class.
    /// </summary>
    [Serializable]
    public partial class ViewMonthlyNumTransactionCollection : ReadOnlyList<ViewMonthlyNumTransaction, ViewMonthlyNumTransactionCollection>
    {        
        public ViewMonthlyNumTransactionCollection() {}
    }
    /// <summary>
    /// This is  Read-only wrapper class for the ViewMonthlyNumTransaction view.
    /// </summary>
    [Serializable]
    public partial class ViewMonthlyNumTransaction : ReadOnlyRecord<ViewMonthlyNumTransaction>, IReadOnlyRecord
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
                TableSchema.Table schema = new TableSchema.Table("ViewMonthlyNumTransaction", TableType.View, DataService.GetInstance("PowerPOS"));
                schema.Columns = new TableSchema.TableColumnCollection();
                schema.SchemaName = @"dbo";
                //columns
                
                TableSchema.TableColumn colvarOrdermonth = new TableSchema.TableColumn(schema);
                colvarOrdermonth.ColumnName = "ordermonth";
                colvarOrdermonth.DataType = DbType.Int32;
                colvarOrdermonth.MaxLength = 0;
                colvarOrdermonth.AutoIncrement = false;
                colvarOrdermonth.IsNullable = true;
                colvarOrdermonth.IsPrimaryKey = false;
                colvarOrdermonth.IsForeignKey = false;
                colvarOrdermonth.IsReadOnly = false;
                
                schema.Columns.Add(colvarOrdermonth);
                
                TableSchema.TableColumn colvarOrderYear = new TableSchema.TableColumn(schema);
                colvarOrderYear.ColumnName = "OrderYear";
                colvarOrderYear.DataType = DbType.Int32;
                colvarOrderYear.MaxLength = 0;
                colvarOrderYear.AutoIncrement = false;
                colvarOrderYear.IsNullable = true;
                colvarOrderYear.IsPrimaryKey = false;
                colvarOrderYear.IsForeignKey = false;
                colvarOrderYear.IsReadOnly = false;
                
                schema.Columns.Add(colvarOrderYear);
                
                TableSchema.TableColumn colvarNumTransaction = new TableSchema.TableColumn(schema);
                colvarNumTransaction.ColumnName = "Num_Transaction";
                colvarNumTransaction.DataType = DbType.Int32;
                colvarNumTransaction.MaxLength = 0;
                colvarNumTransaction.AutoIncrement = false;
                colvarNumTransaction.IsNullable = true;
                colvarNumTransaction.IsPrimaryKey = false;
                colvarNumTransaction.IsForeignKey = false;
                colvarNumTransaction.IsReadOnly = false;
                
                schema.Columns.Add(colvarNumTransaction);
                
                TableSchema.TableColumn colvarTotalQty = new TableSchema.TableColumn(schema);
                colvarTotalQty.ColumnName = "Total_Qty";
                colvarTotalQty.DataType = DbType.Int32;
                colvarTotalQty.MaxLength = 0;
                colvarTotalQty.AutoIncrement = false;
                colvarTotalQty.IsNullable = true;
                colvarTotalQty.IsPrimaryKey = false;
                colvarTotalQty.IsForeignKey = false;
                colvarTotalQty.IsReadOnly = false;
                
                schema.Columns.Add(colvarTotalQty);
                
                TableSchema.TableColumn colvarTotalSales = new TableSchema.TableColumn(schema);
                colvarTotalSales.ColumnName = "Total_Sales";
                colvarTotalSales.DataType = DbType.Currency;
                colvarTotalSales.MaxLength = 0;
                colvarTotalSales.AutoIncrement = false;
                colvarTotalSales.IsNullable = true;
                colvarTotalSales.IsPrimaryKey = false;
                colvarTotalSales.IsForeignKey = false;
                colvarTotalSales.IsReadOnly = false;
                
                schema.Columns.Add(colvarTotalSales);
                
                
                BaseSchema = schema;
                //add this schema to the provider
                //so we can query it later
                DataService.Providers["PowerPOS"].AddSchema("ViewMonthlyNumTransaction",schema);
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
	    public ViewMonthlyNumTransaction()
	    {
            SetSQLProps();
            SetDefaults();
            MarkNew();
        }
        public ViewMonthlyNumTransaction(bool useDatabaseDefaults)
	    {
		    SetSQLProps();
		    if(useDatabaseDefaults)
		    {
				ForceDefaults();
			}
			MarkNew();
	    }
	    
	    public ViewMonthlyNumTransaction(object keyID)
	    {
		    SetSQLProps();
		    LoadByKey(keyID);
	    }
    	 
	    public ViewMonthlyNumTransaction(string columnName, object columnValue)
        {
            SetSQLProps();
            LoadByParam(columnName,columnValue);
        }
        
	    #endregion
	    
	    #region Props
	    
          
        [XmlAttribute("Ordermonth")]
        [Bindable(true)]
        public int? Ordermonth 
	    {
		    get
		    {
			    return GetColumnValue<int?>("ordermonth");
		    }
            set 
		    {
			    SetColumnValue("ordermonth", value);
            }
        }
	      
        [XmlAttribute("OrderYear")]
        [Bindable(true)]
        public int? OrderYear 
	    {
		    get
		    {
			    return GetColumnValue<int?>("OrderYear");
		    }
            set 
		    {
			    SetColumnValue("OrderYear", value);
            }
        }
	      
        [XmlAttribute("NumTransaction")]
        [Bindable(true)]
        public int? NumTransaction 
	    {
		    get
		    {
			    return GetColumnValue<int?>("Num_Transaction");
		    }
            set 
		    {
			    SetColumnValue("Num_Transaction", value);
            }
        }
	      
        [XmlAttribute("TotalQty")]
        [Bindable(true)]
        public int? TotalQty 
	    {
		    get
		    {
			    return GetColumnValue<int?>("Total_Qty");
		    }
            set 
		    {
			    SetColumnValue("Total_Qty", value);
            }
        }
	      
        [XmlAttribute("TotalSales")]
        [Bindable(true)]
        public decimal? TotalSales 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("Total_Sales");
		    }
            set 
		    {
			    SetColumnValue("Total_Sales", value);
            }
        }
	    
	    #endregion
    
	    #region Columns Struct
	    public struct Columns
	    {
		    
		    
            public static string Ordermonth = @"ordermonth";
            
            public static string OrderYear = @"OrderYear";
            
            public static string NumTransaction = @"Num_Transaction";
            
            public static string TotalQty = @"Total_Qty";
            
            public static string TotalSales = @"Total_Sales";
            
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
