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
    /// Strongly-typed collection for the ViewDailyTransactionBySalesPerson class.
    /// </summary>
    [Serializable]
    public partial class ViewDailyTransactionBySalesPersonCollection : ReadOnlyList<ViewDailyTransactionBySalesPerson, ViewDailyTransactionBySalesPersonCollection>
    {        
        public ViewDailyTransactionBySalesPersonCollection() {}
    }
    /// <summary>
    /// This is  Read-only wrapper class for the ViewDailyTransactionBySalesPerson view.
    /// </summary>
    [Serializable]
    public partial class ViewDailyTransactionBySalesPerson : ReadOnlyRecord<ViewDailyTransactionBySalesPerson>, IReadOnlyRecord
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
                TableSchema.Table schema = new TableSchema.Table("ViewDailyTransactionBySalesPerson", TableType.View, DataService.GetInstance("PowerPOS"));
                schema.Columns = new TableSchema.TableColumnCollection();
                schema.SchemaName = @"dbo";
                //columns
                
                TableSchema.TableColumn colvarOrderdate = new TableSchema.TableColumn(schema);
                colvarOrderdate.ColumnName = "orderdate";
                colvarOrderdate.DataType = DbType.DateTime;
                colvarOrderdate.MaxLength = 0;
                colvarOrderdate.AutoIncrement = false;
                colvarOrderdate.IsNullable = true;
                colvarOrderdate.IsPrimaryKey = false;
                colvarOrderdate.IsForeignKey = false;
                colvarOrderdate.IsReadOnly = false;
                
                schema.Columns.Add(colvarOrderdate);
                
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
                
                TableSchema.TableColumn colvarServiceamount = new TableSchema.TableColumn(schema);
                colvarServiceamount.ColumnName = "serviceamount";
                colvarServiceamount.DataType = DbType.Currency;
                colvarServiceamount.MaxLength = 0;
                colvarServiceamount.AutoIncrement = false;
                colvarServiceamount.IsNullable = true;
                colvarServiceamount.IsPrimaryKey = false;
                colvarServiceamount.IsForeignKey = false;
                colvarServiceamount.IsReadOnly = false;
                
                schema.Columns.Add(colvarServiceamount);
                
                TableSchema.TableColumn colvarProductamount = new TableSchema.TableColumn(schema);
                colvarProductamount.ColumnName = "productamount";
                colvarProductamount.DataType = DbType.Currency;
                colvarProductamount.MaxLength = 0;
                colvarProductamount.AutoIncrement = false;
                colvarProductamount.IsNullable = true;
                colvarProductamount.IsPrimaryKey = false;
                colvarProductamount.IsForeignKey = false;
                colvarProductamount.IsReadOnly = false;
                
                schema.Columns.Add(colvarProductamount);
                
                TableSchema.TableColumn colvarAmount = new TableSchema.TableColumn(schema);
                colvarAmount.ColumnName = "Amount";
                colvarAmount.DataType = DbType.Currency;
                colvarAmount.MaxLength = 0;
                colvarAmount.AutoIncrement = false;
                colvarAmount.IsNullable = true;
                colvarAmount.IsPrimaryKey = false;
                colvarAmount.IsForeignKey = false;
                colvarAmount.IsReadOnly = false;
                
                schema.Columns.Add(colvarAmount);
                
                
                BaseSchema = schema;
                //add this schema to the provider
                //so we can query it later
                DataService.Providers["PowerPOS"].AddSchema("ViewDailyTransactionBySalesPerson",schema);
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
	    public ViewDailyTransactionBySalesPerson()
	    {
            SetSQLProps();
            SetDefaults();
            MarkNew();
        }
        public ViewDailyTransactionBySalesPerson(bool useDatabaseDefaults)
	    {
		    SetSQLProps();
		    if(useDatabaseDefaults)
		    {
				ForceDefaults();
			}
			MarkNew();
	    }
	    
	    public ViewDailyTransactionBySalesPerson(object keyID)
	    {
		    SetSQLProps();
		    LoadByKey(keyID);
	    }
    	 
	    public ViewDailyTransactionBySalesPerson(string columnName, object columnValue)
        {
            SetSQLProps();
            LoadByParam(columnName,columnValue);
        }
        
	    #endregion
	    
	    #region Props
	    
          
        [XmlAttribute("Orderdate")]
        [Bindable(true)]
        public DateTime? Orderdate 
	    {
		    get
		    {
			    return GetColumnValue<DateTime?>("orderdate");
		    }
            set 
		    {
			    SetColumnValue("orderdate", value);
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
	      
        [XmlAttribute("Serviceamount")]
        [Bindable(true)]
        public decimal? Serviceamount 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("serviceamount");
		    }
            set 
		    {
			    SetColumnValue("serviceamount", value);
            }
        }
	      
        [XmlAttribute("Productamount")]
        [Bindable(true)]
        public decimal? Productamount 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("productamount");
		    }
            set 
		    {
			    SetColumnValue("productamount", value);
            }
        }
	      
        [XmlAttribute("Amount")]
        [Bindable(true)]
        public decimal? Amount 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("Amount");
		    }
            set 
		    {
			    SetColumnValue("Amount", value);
            }
        }
	    
	    #endregion
    
	    #region Columns Struct
	    public struct Columns
	    {
		    
		    
            public static string Orderdate = @"orderdate";
            
            public static string DisplayName = @"DisplayName";
            
            public static string PointOfSaleName = @"PointOfSaleName";
            
            public static string OutletName = @"OutletName";
            
            public static string PointOfSaleID = @"PointOfSaleID";
            
            public static string Serviceamount = @"serviceamount";
            
            public static string Productamount = @"productamount";
            
            public static string Amount = @"Amount";
            
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
