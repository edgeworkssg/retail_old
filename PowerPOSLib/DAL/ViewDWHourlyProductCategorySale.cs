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
    /// Strongly-typed collection for the ViewDWHourlyProductCategorySale class.
    /// </summary>
    [Serializable]
    public partial class ViewDWHourlyProductCategorySaleCollection : ReadOnlyList<ViewDWHourlyProductCategorySale, ViewDWHourlyProductCategorySaleCollection>
    {        
        public ViewDWHourlyProductCategorySaleCollection() {}
    }
    /// <summary>
    /// This is  Read-only wrapper class for the viewDW_HourlyProductCategorySales view.
    /// </summary>
    [Serializable]
    public partial class ViewDWHourlyProductCategorySale : ReadOnlyRecord<ViewDWHourlyProductCategorySale>, IReadOnlyRecord
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
                TableSchema.Table schema = new TableSchema.Table("viewDW_HourlyProductCategorySales", TableType.View, DataService.GetInstance("PowerPOS"));
                schema.Columns = new TableSchema.TableColumnCollection();
                schema.SchemaName = @"dbo";
                //columns
                
                TableSchema.TableColumn colvarOrderdate = new TableSchema.TableColumn(schema);
                colvarOrderdate.ColumnName = "orderdate";
                colvarOrderdate.DataType = DbType.DateTime;
                colvarOrderdate.MaxLength = 0;
                colvarOrderdate.AutoIncrement = false;
                colvarOrderdate.IsNullable = false;
                colvarOrderdate.IsPrimaryKey = false;
                colvarOrderdate.IsForeignKey = false;
                colvarOrderdate.IsReadOnly = false;
                
                schema.Columns.Add(colvarOrderdate);
                
                TableSchema.TableColumn colvarOutletname = new TableSchema.TableColumn(schema);
                colvarOutletname.ColumnName = "outletname";
                colvarOutletname.DataType = DbType.AnsiString;
                colvarOutletname.MaxLength = 50;
                colvarOutletname.AutoIncrement = false;
                colvarOutletname.IsNullable = false;
                colvarOutletname.IsPrimaryKey = false;
                colvarOutletname.IsForeignKey = false;
                colvarOutletname.IsReadOnly = false;
                
                schema.Columns.Add(colvarOutletname);
                
                TableSchema.TableColumn colvarCategoryname = new TableSchema.TableColumn(schema);
                colvarCategoryname.ColumnName = "categoryname";
                colvarCategoryname.DataType = DbType.String;
                colvarCategoryname.MaxLength = 250;
                colvarCategoryname.AutoIncrement = false;
                colvarCategoryname.IsNullable = true;
                colvarCategoryname.IsPrimaryKey = false;
                colvarCategoryname.IsForeignKey = false;
                colvarCategoryname.IsReadOnly = false;
                
                schema.Columns.Add(colvarCategoryname);
                
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
                
                
                BaseSchema = schema;
                //add this schema to the provider
                //so we can query it later
                DataService.Providers["PowerPOS"].AddSchema("viewDW_HourlyProductCategorySales",schema);
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
	    public ViewDWHourlyProductCategorySale()
	    {
            SetSQLProps();
            SetDefaults();
            MarkNew();
        }
        public ViewDWHourlyProductCategorySale(bool useDatabaseDefaults)
	    {
		    SetSQLProps();
		    if(useDatabaseDefaults)
		    {
				ForceDefaults();
			}
			MarkNew();
	    }
	    
	    public ViewDWHourlyProductCategorySale(object keyID)
	    {
		    SetSQLProps();
		    LoadByKey(keyID);
	    }
    	 
	    public ViewDWHourlyProductCategorySale(string columnName, object columnValue)
        {
            SetSQLProps();
            LoadByParam(columnName,columnValue);
        }
        
	    #endregion
	    
	    #region Props
	    
          
        [XmlAttribute("Orderdate")]
        [Bindable(true)]
        public DateTime Orderdate 
	    {
		    get
		    {
			    return GetColumnValue<DateTime>("orderdate");
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
	      
        [XmlAttribute("Categoryname")]
        [Bindable(true)]
        public string Categoryname 
	    {
		    get
		    {
			    return GetColumnValue<string>("categoryname");
		    }
            set 
		    {
			    SetColumnValue("categoryname", value);
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
	    
	    #endregion
    
	    #region Columns Struct
	    public struct Columns
	    {
		    
		    
            public static string Orderdate = @"orderdate";
            
            public static string Outletname = @"outletname";
            
            public static string Categoryname = @"categoryname";
            
            public static string Quantity = @"quantity";
            
            public static string Amount = @"amount";
            
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
