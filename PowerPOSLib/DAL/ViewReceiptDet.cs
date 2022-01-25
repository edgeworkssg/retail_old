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
    /// Strongly-typed collection for the ViewReceiptDet class.
    /// </summary>
    [Serializable]
    public partial class ViewReceiptDetCollection : ReadOnlyList<ViewReceiptDet, ViewReceiptDetCollection>
    {        
        public ViewReceiptDetCollection() {}
    }
    /// <summary>
    /// This is  Read-only wrapper class for the ViewReceiptDet view.
    /// </summary>
    [Serializable]
    public partial class ViewReceiptDet : ReadOnlyRecord<ViewReceiptDet>, IReadOnlyRecord
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
                TableSchema.Table schema = new TableSchema.Table("ViewReceiptDet", TableType.View, DataService.GetInstance("PowerPOS"));
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
                
                TableSchema.TableColumn colvarReceiptDetID = new TableSchema.TableColumn(schema);
                colvarReceiptDetID.ColumnName = "ReceiptDetID";
                colvarReceiptDetID.DataType = DbType.AnsiString;
                colvarReceiptDetID.MaxLength = 18;
                colvarReceiptDetID.AutoIncrement = false;
                colvarReceiptDetID.IsNullable = false;
                colvarReceiptDetID.IsPrimaryKey = false;
                colvarReceiptDetID.IsForeignKey = false;
                colvarReceiptDetID.IsReadOnly = false;
                
                schema.Columns.Add(colvarReceiptDetID);
                
                TableSchema.TableColumn colvarPaymentType = new TableSchema.TableColumn(schema);
                colvarPaymentType.ColumnName = "PaymentType";
                colvarPaymentType.DataType = DbType.AnsiString;
                colvarPaymentType.MaxLength = 50;
                colvarPaymentType.AutoIncrement = false;
                colvarPaymentType.IsNullable = false;
                colvarPaymentType.IsPrimaryKey = false;
                colvarPaymentType.IsForeignKey = false;
                colvarPaymentType.IsReadOnly = false;
                
                schema.Columns.Add(colvarPaymentType);
                
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
                DataService.Providers["PowerPOS"].AddSchema("ViewReceiptDet",schema);
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
	    public ViewReceiptDet()
	    {
            SetSQLProps();
            SetDefaults();
            MarkNew();
        }
        public ViewReceiptDet(bool useDatabaseDefaults)
	    {
		    SetSQLProps();
		    if(useDatabaseDefaults)
		    {
				ForceDefaults();
			}
			MarkNew();
	    }
	    
	    public ViewReceiptDet(object keyID)
	    {
		    SetSQLProps();
		    LoadByKey(keyID);
	    }
    	 
	    public ViewReceiptDet(string columnName, object columnValue)
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
	      
        [XmlAttribute("ReceiptDetID")]
        [Bindable(true)]
        public string ReceiptDetID 
	    {
		    get
		    {
			    return GetColumnValue<string>("ReceiptDetID");
		    }
            set 
		    {
			    SetColumnValue("ReceiptDetID", value);
            }
        }
	      
        [XmlAttribute("PaymentType")]
        [Bindable(true)]
        public string PaymentType 
	    {
		    get
		    {
			    return GetColumnValue<string>("PaymentType");
		    }
            set 
		    {
			    SetColumnValue("PaymentType", value);
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
            
            public static string PointOfSaleID = @"PointOfSaleID";
            
            public static string ReceiptDetID = @"ReceiptDetID";
            
            public static string PaymentType = @"PaymentType";
            
            public static string Amount = @"Amount";
            
            public static string IsVoided = @"IsVoided";
            
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
