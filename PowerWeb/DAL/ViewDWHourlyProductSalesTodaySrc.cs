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
    /// Strongly-typed collection for the ViewDWHourlyProductSalesTodaySrc class.
    /// </summary>
    [Serializable]
    public partial class ViewDWHourlyProductSalesTodaySrcCollection : ReadOnlyList<ViewDWHourlyProductSalesTodaySrc, ViewDWHourlyProductSalesTodaySrcCollection>
    {        
        public ViewDWHourlyProductSalesTodaySrcCollection() {}
    }
    /// <summary>
    /// This is  Read-only wrapper class for the viewDW_HourlyProductSales_today_src view.
    /// </summary>
    [Serializable]
    public partial class ViewDWHourlyProductSalesTodaySrc : ReadOnlyRecord<ViewDWHourlyProductSalesTodaySrc>, IReadOnlyRecord
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
                TableSchema.Table schema = new TableSchema.Table("viewDW_HourlyProductSales_today_src", TableType.View, DataService.GetInstance("PowerPOS"));
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
                
                TableSchema.TableColumn colvarItemno = new TableSchema.TableColumn(schema);
                colvarItemno.ColumnName = "itemno";
                colvarItemno.DataType = DbType.AnsiString;
                colvarItemno.MaxLength = 50;
                colvarItemno.AutoIncrement = false;
                colvarItemno.IsNullable = true;
                colvarItemno.IsPrimaryKey = false;
                colvarItemno.IsForeignKey = false;
                colvarItemno.IsReadOnly = false;
                
                schema.Columns.Add(colvarItemno);
                
                TableSchema.TableColumn colvarQuantity = new TableSchema.TableColumn(schema);
                colvarQuantity.ColumnName = "quantity";
                colvarQuantity.DataType = DbType.Decimal;
                colvarQuantity.MaxLength = 0;
                colvarQuantity.AutoIncrement = false;
                colvarQuantity.IsNullable = true;
                colvarQuantity.IsPrimaryKey = false;
                colvarQuantity.IsForeignKey = false;
                colvarQuantity.IsReadOnly = false;
                
                schema.Columns.Add(colvarQuantity);
                
                TableSchema.TableColumn colvarAmount = new TableSchema.TableColumn(schema);
                colvarAmount.ColumnName = "amount";
                colvarAmount.DataType = DbType.Currency;
                colvarAmount.MaxLength = 0;
                colvarAmount.AutoIncrement = false;
                colvarAmount.IsNullable = true;
                colvarAmount.IsPrimaryKey = false;
                colvarAmount.IsForeignKey = false;
                colvarAmount.IsReadOnly = false;
                
                schema.Columns.Add(colvarAmount);
                
                TableSchema.TableColumn colvarRegenerate = new TableSchema.TableColumn(schema);
                colvarRegenerate.ColumnName = "regenerate";
                colvarRegenerate.DataType = DbType.Int32;
                colvarRegenerate.MaxLength = 0;
                colvarRegenerate.AutoIncrement = false;
                colvarRegenerate.IsNullable = false;
                colvarRegenerate.IsPrimaryKey = false;
                colvarRegenerate.IsForeignKey = false;
                colvarRegenerate.IsReadOnly = false;
                
                schema.Columns.Add(colvarRegenerate);
                
                
                BaseSchema = schema;
                //add this schema to the provider
                //so we can query it later
                DataService.Providers["PowerPOS"].AddSchema("viewDW_HourlyProductSales_today_src",schema);
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
	    public ViewDWHourlyProductSalesTodaySrc()
	    {
            SetSQLProps();
            SetDefaults();
            MarkNew();
        }
        public ViewDWHourlyProductSalesTodaySrc(bool useDatabaseDefaults)
	    {
		    SetSQLProps();
		    if(useDatabaseDefaults)
		    {
				ForceDefaults();
			}
			MarkNew();
	    }
	    
	    public ViewDWHourlyProductSalesTodaySrc(object keyID)
	    {
		    SetSQLProps();
		    LoadByKey(keyID);
	    }
    	 
	    public ViewDWHourlyProductSalesTodaySrc(string columnName, object columnValue)
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
	      
        [XmlAttribute("Itemno")]
        [Bindable(true)]
        public string Itemno 
	    {
		    get
		    {
			    return GetColumnValue<string>("itemno");
		    }
            set 
		    {
			    SetColumnValue("itemno", value);
            }
        }
	      
        [XmlAttribute("Quantity")]
        [Bindable(true)]
        public decimal? Quantity 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("quantity");
		    }
            set 
		    {
			    SetColumnValue("quantity", value);
            }
        }
	      
        [XmlAttribute("Amount")]
        [Bindable(true)]
        public decimal? Amount 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("amount");
		    }
            set 
		    {
			    SetColumnValue("amount", value);
            }
        }
	      
        [XmlAttribute("Regenerate")]
        [Bindable(true)]
        public int Regenerate 
	    {
		    get
		    {
			    return GetColumnValue<int>("regenerate");
		    }
            set 
		    {
			    SetColumnValue("regenerate", value);
            }
        }
	    
	    #endregion
    
	    #region Columns Struct
	    public struct Columns
	    {
		    
		    
            public static string Orderdate = @"orderdate";
            
            public static string Outletname = @"outletname";
            
            public static string Itemno = @"itemno";
            
            public static string Quantity = @"quantity";
            
            public static string Amount = @"amount";
            
            public static string Regenerate = @"regenerate";
            
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
