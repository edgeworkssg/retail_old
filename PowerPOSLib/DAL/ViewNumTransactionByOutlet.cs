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
    /// Strongly-typed collection for the ViewNumTransactionByOutlet class.
    /// </summary>
    [Serializable]
    public partial class ViewNumTransactionByOutletCollection : ReadOnlyList<ViewNumTransactionByOutlet, ViewNumTransactionByOutletCollection>
    {        
        public ViewNumTransactionByOutletCollection() {}
    }
    /// <summary>
    /// This is  Read-only wrapper class for the ViewNumTransactionByOutlet view.
    /// </summary>
    [Serializable]
    public partial class ViewNumTransactionByOutlet : ReadOnlyRecord<ViewNumTransactionByOutlet>, IReadOnlyRecord
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
                TableSchema.Table schema = new TableSchema.Table("ViewNumTransactionByOutlet", TableType.View, DataService.GetInstance("PowerPOS"));
                schema.Columns = new TableSchema.TableColumnCollection();
                schema.SchemaName = @"dbo";
                //columns
                
                TableSchema.TableColumn colvarOrderDate = new TableSchema.TableColumn(schema);
                colvarOrderDate.ColumnName = "OrderDate";
                colvarOrderDate.DataType = DbType.DateTime;
                colvarOrderDate.MaxLength = 0;
                colvarOrderDate.AutoIncrement = false;
                colvarOrderDate.IsNullable = true;
                colvarOrderDate.IsPrimaryKey = false;
                colvarOrderDate.IsForeignKey = false;
                colvarOrderDate.IsReadOnly = false;
                
                schema.Columns.Add(colvarOrderDate);
                
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
                DataService.Providers["PowerPOS"].AddSchema("ViewNumTransactionByOutlet",schema);
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
	    public ViewNumTransactionByOutlet()
	    {
            SetSQLProps();
            SetDefaults();
            MarkNew();
        }
        public ViewNumTransactionByOutlet(bool useDatabaseDefaults)
	    {
		    SetSQLProps();
		    if(useDatabaseDefaults)
		    {
				ForceDefaults();
			}
			MarkNew();
	    }
	    
	    public ViewNumTransactionByOutlet(object keyID)
	    {
		    SetSQLProps();
		    LoadByKey(keyID);
	    }
    	 
	    public ViewNumTransactionByOutlet(string columnName, object columnValue)
        {
            SetSQLProps();
            LoadByParam(columnName,columnValue);
        }
        
	    #endregion
	    
	    #region Props
	    
          
        [XmlAttribute("OrderDate")]
        [Bindable(true)]
        public DateTime? OrderDate 
	    {
		    get
		    {
			    return GetColumnValue<DateTime?>("OrderDate");
		    }
            set 
		    {
			    SetColumnValue("OrderDate", value);
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
		    
		    
            public static string OrderDate = @"OrderDate";
            
            public static string NumTransaction = @"Num_Transaction";
            
            public static string TotalQty = @"Total_Qty";
            
            public static string TotalSales = @"Total_Sales";
            
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
