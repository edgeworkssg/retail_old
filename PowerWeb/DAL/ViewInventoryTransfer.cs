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
    /// Strongly-typed collection for the ViewInventoryTransfer class.
    /// </summary>
    [Serializable]
    public partial class ViewInventoryTransferCollection : ReadOnlyList<ViewInventoryTransfer, ViewInventoryTransferCollection>
    {        
        public ViewInventoryTransferCollection() {}
    }
    /// <summary>
    /// This is  Read-only wrapper class for the ViewInventoryTransfer view.
    /// </summary>
    [Serializable]
    public partial class ViewInventoryTransfer : ReadOnlyRecord<ViewInventoryTransfer>, IReadOnlyRecord
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
                TableSchema.Table schema = new TableSchema.Table("ViewInventoryTransfer", TableType.View, DataService.GetInstance("PowerPOS"));
                schema.Columns = new TableSchema.TableColumnCollection();
                schema.SchemaName = @"dbo";
                //columns
                
                TableSchema.TableColumn colvarFromInventoryLocationName = new TableSchema.TableColumn(schema);
                colvarFromInventoryLocationName.ColumnName = "FromInventoryLocationName";
                colvarFromInventoryLocationName.DataType = DbType.AnsiString;
                colvarFromInventoryLocationName.MaxLength = 50;
                colvarFromInventoryLocationName.AutoIncrement = false;
                colvarFromInventoryLocationName.IsNullable = false;
                colvarFromInventoryLocationName.IsPrimaryKey = false;
                colvarFromInventoryLocationName.IsForeignKey = false;
                colvarFromInventoryLocationName.IsReadOnly = false;
                
                schema.Columns.Add(colvarFromInventoryLocationName);
                
                TableSchema.TableColumn colvarToInventoryLocationName = new TableSchema.TableColumn(schema);
                colvarToInventoryLocationName.ColumnName = "ToInventoryLocationName";
                colvarToInventoryLocationName.DataType = DbType.AnsiString;
                colvarToInventoryLocationName.MaxLength = 50;
                colvarToInventoryLocationName.AutoIncrement = false;
                colvarToInventoryLocationName.IsNullable = false;
                colvarToInventoryLocationName.IsPrimaryKey = false;
                colvarToInventoryLocationName.IsForeignKey = false;
                colvarToInventoryLocationName.IsReadOnly = false;
                
                schema.Columns.Add(colvarToInventoryLocationName);
                
                TableSchema.TableColumn colvarTransferFromBy = new TableSchema.TableColumn(schema);
                colvarTransferFromBy.ColumnName = "TransferFromBy";
                colvarTransferFromBy.DataType = DbType.AnsiString;
                colvarTransferFromBy.MaxLength = 50;
                colvarTransferFromBy.AutoIncrement = false;
                colvarTransferFromBy.IsNullable = false;
                colvarTransferFromBy.IsPrimaryKey = false;
                colvarTransferFromBy.IsForeignKey = false;
                colvarTransferFromBy.IsReadOnly = false;
                
                schema.Columns.Add(colvarTransferFromBy);
                
                TableSchema.TableColumn colvarTransferReceivedBy = new TableSchema.TableColumn(schema);
                colvarTransferReceivedBy.ColumnName = "TransferReceivedBy";
                colvarTransferReceivedBy.DataType = DbType.AnsiString;
                colvarTransferReceivedBy.MaxLength = 50;
                colvarTransferReceivedBy.AutoIncrement = false;
                colvarTransferReceivedBy.IsNullable = true;
                colvarTransferReceivedBy.IsPrimaryKey = false;
                colvarTransferReceivedBy.IsForeignKey = false;
                colvarTransferReceivedBy.IsReadOnly = false;
                
                schema.Columns.Add(colvarTransferReceivedBy);
                
                TableSchema.TableColumn colvarIsReceived = new TableSchema.TableColumn(schema);
                colvarIsReceived.ColumnName = "IsReceived";
                colvarIsReceived.DataType = DbType.Boolean;
                colvarIsReceived.MaxLength = 0;
                colvarIsReceived.AutoIncrement = false;
                colvarIsReceived.IsNullable = false;
                colvarIsReceived.IsPrimaryKey = false;
                colvarIsReceived.IsForeignKey = false;
                colvarIsReceived.IsReadOnly = false;
                
                schema.Columns.Add(colvarIsReceived);
                
                TableSchema.TableColumn colvarRemark = new TableSchema.TableColumn(schema);
                colvarRemark.ColumnName = "Remark";
                colvarRemark.DataType = DbType.String;
                colvarRemark.MaxLength = -1;
                colvarRemark.AutoIncrement = false;
                colvarRemark.IsNullable = false;
                colvarRemark.IsPrimaryKey = false;
                colvarRemark.IsForeignKey = false;
                colvarRemark.IsReadOnly = false;
                
                schema.Columns.Add(colvarRemark);
                
                TableSchema.TableColumn colvarInventoryHdrRefNo = new TableSchema.TableColumn(schema);
                colvarInventoryHdrRefNo.ColumnName = "InventoryHdrRefNo";
                colvarInventoryHdrRefNo.DataType = DbType.AnsiString;
                colvarInventoryHdrRefNo.MaxLength = 50;
                colvarInventoryHdrRefNo.AutoIncrement = false;
                colvarInventoryHdrRefNo.IsNullable = false;
                colvarInventoryHdrRefNo.IsPrimaryKey = false;
                colvarInventoryHdrRefNo.IsForeignKey = false;
                colvarInventoryHdrRefNo.IsReadOnly = false;
                
                schema.Columns.Add(colvarInventoryHdrRefNo);
                
                TableSchema.TableColumn colvarInventoryDate = new TableSchema.TableColumn(schema);
                colvarInventoryDate.ColumnName = "InventoryDate";
                colvarInventoryDate.DataType = DbType.DateTime;
                colvarInventoryDate.MaxLength = 0;
                colvarInventoryDate.AutoIncrement = false;
                colvarInventoryDate.IsNullable = false;
                colvarInventoryDate.IsPrimaryKey = false;
                colvarInventoryDate.IsForeignKey = false;
                colvarInventoryDate.IsReadOnly = false;
                
                schema.Columns.Add(colvarInventoryDate);
                
                TableSchema.TableColumn colvarFromInventoryLocationID = new TableSchema.TableColumn(schema);
                colvarFromInventoryLocationID.ColumnName = "FromInventoryLocationID";
                colvarFromInventoryLocationID.DataType = DbType.Int32;
                colvarFromInventoryLocationID.MaxLength = 0;
                colvarFromInventoryLocationID.AutoIncrement = false;
                colvarFromInventoryLocationID.IsNullable = true;
                colvarFromInventoryLocationID.IsPrimaryKey = false;
                colvarFromInventoryLocationID.IsForeignKey = false;
                colvarFromInventoryLocationID.IsReadOnly = false;
                
                schema.Columns.Add(colvarFromInventoryLocationID);
                
                TableSchema.TableColumn colvarToInventoryLocationID = new TableSchema.TableColumn(schema);
                colvarToInventoryLocationID.ColumnName = "ToInventoryLocationID";
                colvarToInventoryLocationID.DataType = DbType.Int32;
                colvarToInventoryLocationID.MaxLength = 0;
                colvarToInventoryLocationID.AutoIncrement = false;
                colvarToInventoryLocationID.IsNullable = true;
                colvarToInventoryLocationID.IsPrimaryKey = false;
                colvarToInventoryLocationID.IsForeignKey = false;
                colvarToInventoryLocationID.IsReadOnly = false;
                
                schema.Columns.Add(colvarToInventoryLocationID);
                
                TableSchema.TableColumn colvarFromInventoryHdrRefNo = new TableSchema.TableColumn(schema);
                colvarFromInventoryHdrRefNo.ColumnName = "FromInventoryHdrRefNo";
                colvarFromInventoryHdrRefNo.DataType = DbType.AnsiString;
                colvarFromInventoryHdrRefNo.MaxLength = 50;
                colvarFromInventoryHdrRefNo.AutoIncrement = false;
                colvarFromInventoryHdrRefNo.IsNullable = false;
                colvarFromInventoryHdrRefNo.IsPrimaryKey = false;
                colvarFromInventoryHdrRefNo.IsForeignKey = false;
                colvarFromInventoryHdrRefNo.IsReadOnly = false;
                
                schema.Columns.Add(colvarFromInventoryHdrRefNo);
                
                TableSchema.TableColumn colvarLocationTransferID = new TableSchema.TableColumn(schema);
                colvarLocationTransferID.ColumnName = "LocationTransferID";
                colvarLocationTransferID.DataType = DbType.Int32;
                colvarLocationTransferID.MaxLength = 0;
                colvarLocationTransferID.AutoIncrement = false;
                colvarLocationTransferID.IsNullable = false;
                colvarLocationTransferID.IsPrimaryKey = false;
                colvarLocationTransferID.IsForeignKey = false;
                colvarLocationTransferID.IsReadOnly = false;
                
                schema.Columns.Add(colvarLocationTransferID);
                
                
                BaseSchema = schema;
                //add this schema to the provider
                //so we can query it later
                DataService.Providers["PowerPOS"].AddSchema("ViewInventoryTransfer",schema);
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
	    public ViewInventoryTransfer()
	    {
            SetSQLProps();
            SetDefaults();
            MarkNew();
        }
        public ViewInventoryTransfer(bool useDatabaseDefaults)
	    {
		    SetSQLProps();
		    if(useDatabaseDefaults)
		    {
				ForceDefaults();
			}
			MarkNew();
	    }
	    
	    public ViewInventoryTransfer(object keyID)
	    {
		    SetSQLProps();
		    LoadByKey(keyID);
	    }
    	 
	    public ViewInventoryTransfer(string columnName, object columnValue)
        {
            SetSQLProps();
            LoadByParam(columnName,columnValue);
        }
        
	    #endregion
	    
	    #region Props
	    
          
        [XmlAttribute("FromInventoryLocationName")]
        [Bindable(true)]
        public string FromInventoryLocationName 
	    {
		    get
		    {
			    return GetColumnValue<string>("FromInventoryLocationName");
		    }
            set 
		    {
			    SetColumnValue("FromInventoryLocationName", value);
            }
        }
	      
        [XmlAttribute("ToInventoryLocationName")]
        [Bindable(true)]
        public string ToInventoryLocationName 
	    {
		    get
		    {
			    return GetColumnValue<string>("ToInventoryLocationName");
		    }
            set 
		    {
			    SetColumnValue("ToInventoryLocationName", value);
            }
        }
	      
        [XmlAttribute("TransferFromBy")]
        [Bindable(true)]
        public string TransferFromBy 
	    {
		    get
		    {
			    return GetColumnValue<string>("TransferFromBy");
		    }
            set 
		    {
			    SetColumnValue("TransferFromBy", value);
            }
        }
	      
        [XmlAttribute("TransferReceivedBy")]
        [Bindable(true)]
        public string TransferReceivedBy 
	    {
		    get
		    {
			    return GetColumnValue<string>("TransferReceivedBy");
		    }
            set 
		    {
			    SetColumnValue("TransferReceivedBy", value);
            }
        }
	      
        [XmlAttribute("IsReceived")]
        [Bindable(true)]
        public bool IsReceived 
	    {
		    get
		    {
			    return GetColumnValue<bool>("IsReceived");
		    }
            set 
		    {
			    SetColumnValue("IsReceived", value);
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
	      
        [XmlAttribute("InventoryHdrRefNo")]
        [Bindable(true)]
        public string InventoryHdrRefNo 
	    {
		    get
		    {
			    return GetColumnValue<string>("InventoryHdrRefNo");
		    }
            set 
		    {
			    SetColumnValue("InventoryHdrRefNo", value);
            }
        }
	      
        [XmlAttribute("InventoryDate")]
        [Bindable(true)]
        public DateTime InventoryDate 
	    {
		    get
		    {
			    return GetColumnValue<DateTime>("InventoryDate");
		    }
            set 
		    {
			    SetColumnValue("InventoryDate", value);
            }
        }
	      
        [XmlAttribute("FromInventoryLocationID")]
        [Bindable(true)]
        public int? FromInventoryLocationID 
	    {
		    get
		    {
			    return GetColumnValue<int?>("FromInventoryLocationID");
		    }
            set 
		    {
			    SetColumnValue("FromInventoryLocationID", value);
            }
        }
	      
        [XmlAttribute("ToInventoryLocationID")]
        [Bindable(true)]
        public int? ToInventoryLocationID 
	    {
		    get
		    {
			    return GetColumnValue<int?>("ToInventoryLocationID");
		    }
            set 
		    {
			    SetColumnValue("ToInventoryLocationID", value);
            }
        }
	      
        [XmlAttribute("FromInventoryHdrRefNo")]
        [Bindable(true)]
        public string FromInventoryHdrRefNo 
	    {
		    get
		    {
			    return GetColumnValue<string>("FromInventoryHdrRefNo");
		    }
            set 
		    {
			    SetColumnValue("FromInventoryHdrRefNo", value);
            }
        }
	      
        [XmlAttribute("LocationTransferID")]
        [Bindable(true)]
        public int LocationTransferID 
	    {
		    get
		    {
			    return GetColumnValue<int>("LocationTransferID");
		    }
            set 
		    {
			    SetColumnValue("LocationTransferID", value);
            }
        }
	    
	    #endregion
    
	    #region Columns Struct
	    public struct Columns
	    {
		    
		    
            public static string FromInventoryLocationName = @"FromInventoryLocationName";
            
            public static string ToInventoryLocationName = @"ToInventoryLocationName";
            
            public static string TransferFromBy = @"TransferFromBy";
            
            public static string TransferReceivedBy = @"TransferReceivedBy";
            
            public static string IsReceived = @"IsReceived";
            
            public static string Remark = @"Remark";
            
            public static string InventoryHdrRefNo = @"InventoryHdrRefNo";
            
            public static string InventoryDate = @"InventoryDate";
            
            public static string FromInventoryLocationID = @"FromInventoryLocationID";
            
            public static string ToInventoryLocationID = @"ToInventoryLocationID";
            
            public static string FromInventoryHdrRefNo = @"FromInventoryHdrRefNo";
            
            public static string LocationTransferID = @"LocationTransferID";
            
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
