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
    /// Strongly-typed collection for the ViewDWHourlySalesTodaySalesSrc class.
    /// </summary>
    [Serializable]
    public partial class ViewDWHourlySalesTodaySalesSrcCollection : ReadOnlyList<ViewDWHourlySalesTodaySalesSrc, ViewDWHourlySalesTodaySalesSrcCollection>
    {        
        public ViewDWHourlySalesTodaySalesSrcCollection() {}
    }
    /// <summary>
    /// This is  Read-only wrapper class for the viewDW_HourlySales_today_sales_src view.
    /// </summary>
    [Serializable]
    public partial class ViewDWHourlySalesTodaySalesSrc : ReadOnlyRecord<ViewDWHourlySalesTodaySalesSrc>, IReadOnlyRecord
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
                TableSchema.Table schema = new TableSchema.Table("viewDW_HourlySales_today_sales_src", TableType.View, DataService.GetInstance("PowerPOS"));
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
                
                TableSchema.TableColumn colvarOutletname = new TableSchema.TableColumn(schema);
                colvarOutletname.ColumnName = "outletname";
                colvarOutletname.DataType = DbType.AnsiString;
                colvarOutletname.MaxLength = 50;
                colvarOutletname.AutoIncrement = false;
                colvarOutletname.IsNullable = true;
                colvarOutletname.IsPrimaryKey = false;
                colvarOutletname.IsForeignKey = false;
                colvarOutletname.IsReadOnly = false;
                
                schema.Columns.Add(colvarOutletname);
                
                TableSchema.TableColumn colvarBill = new TableSchema.TableColumn(schema);
                colvarBill.ColumnName = "bill";
                colvarBill.DataType = DbType.Int32;
                colvarBill.MaxLength = 0;
                colvarBill.AutoIncrement = false;
                colvarBill.IsNullable = true;
                colvarBill.IsPrimaryKey = false;
                colvarBill.IsForeignKey = false;
                colvarBill.IsReadOnly = false;
                
                schema.Columns.Add(colvarBill);
                
                TableSchema.TableColumn colvarNettamount = new TableSchema.TableColumn(schema);
                colvarNettamount.ColumnName = "nettamount";
                colvarNettamount.DataType = DbType.Currency;
                colvarNettamount.MaxLength = 0;
                colvarNettamount.AutoIncrement = false;
                colvarNettamount.IsNullable = true;
                colvarNettamount.IsPrimaryKey = false;
                colvarNettamount.IsForeignKey = false;
                colvarNettamount.IsReadOnly = false;
                
                schema.Columns.Add(colvarNettamount);
                
                
                BaseSchema = schema;
                //add this schema to the provider
                //so we can query it later
                DataService.Providers["PowerPOS"].AddSchema("viewDW_HourlySales_today_sales_src",schema);
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
	    public ViewDWHourlySalesTodaySalesSrc()
	    {
            SetSQLProps();
            SetDefaults();
            MarkNew();
        }
        public ViewDWHourlySalesTodaySalesSrc(bool useDatabaseDefaults)
	    {
		    SetSQLProps();
		    if(useDatabaseDefaults)
		    {
				ForceDefaults();
			}
			MarkNew();
	    }
	    
	    public ViewDWHourlySalesTodaySalesSrc(object keyID)
	    {
		    SetSQLProps();
		    LoadByKey(keyID);
	    }
    	 
	    public ViewDWHourlySalesTodaySalesSrc(string columnName, object columnValue)
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
	      
        [XmlAttribute("Outletname")]
        [Bindable(true)]
        public string Outletname 
	    {
		    get
		    {
			    return GetColumnValue<string>("outletname");
		    }
            set 
		    {
			    SetColumnValue("outletname", value);
            }
        }
	      
        [XmlAttribute("Bill")]
        [Bindable(true)]
        public int? Bill 
	    {
		    get
		    {
			    return GetColumnValue<int?>("bill");
		    }
            set 
		    {
			    SetColumnValue("bill", value);
            }
        }
	      
        [XmlAttribute("Nettamount")]
        [Bindable(true)]
        public decimal? Nettamount 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("nettamount");
		    }
            set 
		    {
			    SetColumnValue("nettamount", value);
            }
        }
	    
	    #endregion
    
	    #region Columns Struct
	    public struct Columns
	    {
		    
		    
            public static string Orderdate = @"orderdate";
            
            public static string Outletname = @"outletname";
            
            public static string Bill = @"bill";
            
            public static string Nettamount = @"nettamount";
            
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
