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
    /// Strongly-typed collection for the ViewInventoryTransferDiscrepancy class.
    /// </summary>
    [Serializable]
    public partial class ViewInventoryTransferDiscrepancyCollection : ReadOnlyList<ViewInventoryTransferDiscrepancy, ViewInventoryTransferDiscrepancyCollection>
    {        
        public ViewInventoryTransferDiscrepancyCollection() {}
    }
    /// <summary>
    /// This is  Read-only wrapper class for the ViewInventoryTransferDiscrepancy view.
    /// </summary>
    [Serializable]
    public partial class ViewInventoryTransferDiscrepancy : ReadOnlyRecord<ViewInventoryTransferDiscrepancy>, IReadOnlyRecord
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
                TableSchema.Table schema = new TableSchema.Table("ViewInventoryTransferDiscrepancy", TableType.View, DataService.GetInstance("PowerPOS"));
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
                
                TableSchema.TableColumn colvarDiscrepancyID = new TableSchema.TableColumn(schema);
                colvarDiscrepancyID.ColumnName = "DiscrepancyID";
                colvarDiscrepancyID.DataType = DbType.Int32;
                colvarDiscrepancyID.MaxLength = 0;
                colvarDiscrepancyID.AutoIncrement = false;
                colvarDiscrepancyID.IsNullable = false;
                colvarDiscrepancyID.IsPrimaryKey = false;
                colvarDiscrepancyID.IsForeignKey = false;
                colvarDiscrepancyID.IsReadOnly = false;
                
                schema.Columns.Add(colvarDiscrepancyID);
                
                TableSchema.TableColumn colvarDiscrepancyReason = new TableSchema.TableColumn(schema);
                colvarDiscrepancyReason.ColumnName = "DiscrepancyReason";
                colvarDiscrepancyReason.DataType = DbType.AnsiString;
                colvarDiscrepancyReason.MaxLength = 50;
                colvarDiscrepancyReason.AutoIncrement = false;
                colvarDiscrepancyReason.IsNullable = false;
                colvarDiscrepancyReason.IsPrimaryKey = false;
                colvarDiscrepancyReason.IsForeignKey = false;
                colvarDiscrepancyReason.IsReadOnly = false;
                
                schema.Columns.Add(colvarDiscrepancyReason);
                
                TableSchema.TableColumn colvarDiscrepancyRemark = new TableSchema.TableColumn(schema);
                colvarDiscrepancyRemark.ColumnName = "DiscrepancyRemark";
                colvarDiscrepancyRemark.DataType = DbType.AnsiString;
                colvarDiscrepancyRemark.MaxLength = 50;
                colvarDiscrepancyRemark.AutoIncrement = false;
                colvarDiscrepancyRemark.IsNullable = false;
                colvarDiscrepancyRemark.IsPrimaryKey = false;
                colvarDiscrepancyRemark.IsForeignKey = false;
                colvarDiscrepancyRemark.IsReadOnly = false;
                
                schema.Columns.Add(colvarDiscrepancyRemark);
                
                TableSchema.TableColumn colvarItemNo = new TableSchema.TableColumn(schema);
                colvarItemNo.ColumnName = "ItemNo";
                colvarItemNo.DataType = DbType.AnsiString;
                colvarItemNo.MaxLength = 50;
                colvarItemNo.AutoIncrement = false;
                colvarItemNo.IsNullable = false;
                colvarItemNo.IsPrimaryKey = false;
                colvarItemNo.IsForeignKey = false;
                colvarItemNo.IsReadOnly = false;
                
                schema.Columns.Add(colvarItemNo);
                
                TableSchema.TableColumn colvarDiscrepancyQuantity = new TableSchema.TableColumn(schema);
                colvarDiscrepancyQuantity.ColumnName = "DiscrepancyQuantity";
                colvarDiscrepancyQuantity.DataType = DbType.Int32;
                colvarDiscrepancyQuantity.MaxLength = 0;
                colvarDiscrepancyQuantity.AutoIncrement = false;
                colvarDiscrepancyQuantity.IsNullable = false;
                colvarDiscrepancyQuantity.IsPrimaryKey = false;
                colvarDiscrepancyQuantity.IsForeignKey = false;
                colvarDiscrepancyQuantity.IsReadOnly = false;
                
                schema.Columns.Add(colvarDiscrepancyQuantity);
                
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
                
                TableSchema.TableColumn colvarItemName = new TableSchema.TableColumn(schema);
                colvarItemName.ColumnName = "ItemName";
                colvarItemName.DataType = DbType.String;
                colvarItemName.MaxLength = 300;
                colvarItemName.AutoIncrement = false;
                colvarItemName.IsNullable = false;
                colvarItemName.IsPrimaryKey = false;
                colvarItemName.IsForeignKey = false;
                colvarItemName.IsReadOnly = false;
                
                schema.Columns.Add(colvarItemName);
                
                TableSchema.TableColumn colvarCategoryName = new TableSchema.TableColumn(schema);
                colvarCategoryName.ColumnName = "CategoryName";
                colvarCategoryName.DataType = DbType.String;
                colvarCategoryName.MaxLength = 250;
                colvarCategoryName.AutoIncrement = false;
                colvarCategoryName.IsNullable = false;
                colvarCategoryName.IsPrimaryKey = false;
                colvarCategoryName.IsForeignKey = false;
                colvarCategoryName.IsReadOnly = false;
                
                schema.Columns.Add(colvarCategoryName);
                
                TableSchema.TableColumn colvarCostOfGoods = new TableSchema.TableColumn(schema);
                colvarCostOfGoods.ColumnName = "CostOfGoods";
                colvarCostOfGoods.DataType = DbType.Currency;
                colvarCostOfGoods.MaxLength = 0;
                colvarCostOfGoods.AutoIncrement = false;
                colvarCostOfGoods.IsNullable = true;
                colvarCostOfGoods.IsPrimaryKey = false;
                colvarCostOfGoods.IsForeignKey = false;
                colvarCostOfGoods.IsReadOnly = false;
                
                schema.Columns.Add(colvarCostOfGoods);
                
                TableSchema.TableColumn colvarIsClosed = new TableSchema.TableColumn(schema);
                colvarIsClosed.ColumnName = "IsClosed";
                colvarIsClosed.DataType = DbType.Boolean;
                colvarIsClosed.MaxLength = 0;
                colvarIsClosed.AutoIncrement = false;
                colvarIsClosed.IsNullable = true;
                colvarIsClosed.IsPrimaryKey = false;
                colvarIsClosed.IsForeignKey = false;
                colvarIsClosed.IsReadOnly = false;
                
                schema.Columns.Add(colvarIsClosed);
                
                
                BaseSchema = schema;
                //add this schema to the provider
                //so we can query it later
                DataService.Providers["PowerPOS"].AddSchema("ViewInventoryTransferDiscrepancy",schema);
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
	    public ViewInventoryTransferDiscrepancy()
	    {
            SetSQLProps();
            SetDefaults();
            MarkNew();
        }
        public ViewInventoryTransferDiscrepancy(bool useDatabaseDefaults)
	    {
		    SetSQLProps();
		    if(useDatabaseDefaults)
		    {
				ForceDefaults();
			}
			MarkNew();
	    }
	    
	    public ViewInventoryTransferDiscrepancy(object keyID)
	    {
		    SetSQLProps();
		    LoadByKey(keyID);
	    }
    	 
	    public ViewInventoryTransferDiscrepancy(string columnName, object columnValue)
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
	      
        [XmlAttribute("DiscrepancyID")]
        [Bindable(true)]
        public int DiscrepancyID 
	    {
		    get
		    {
			    return GetColumnValue<int>("DiscrepancyID");
		    }
            set 
		    {
			    SetColumnValue("DiscrepancyID", value);
            }
        }
	      
        [XmlAttribute("DiscrepancyReason")]
        [Bindable(true)]
        public string DiscrepancyReason 
	    {
		    get
		    {
			    return GetColumnValue<string>("DiscrepancyReason");
		    }
            set 
		    {
			    SetColumnValue("DiscrepancyReason", value);
            }
        }
	      
        [XmlAttribute("DiscrepancyRemark")]
        [Bindable(true)]
        public string DiscrepancyRemark 
	    {
		    get
		    {
			    return GetColumnValue<string>("DiscrepancyRemark");
		    }
            set 
		    {
			    SetColumnValue("DiscrepancyRemark", value);
            }
        }
	      
        [XmlAttribute("ItemNo")]
        [Bindable(true)]
        public string ItemNo 
	    {
		    get
		    {
			    return GetColumnValue<string>("ItemNo");
		    }
            set 
		    {
			    SetColumnValue("ItemNo", value);
            }
        }
	      
        [XmlAttribute("DiscrepancyQuantity")]
        [Bindable(true)]
        public int DiscrepancyQuantity 
	    {
		    get
		    {
			    return GetColumnValue<int>("DiscrepancyQuantity");
		    }
            set 
		    {
			    SetColumnValue("DiscrepancyQuantity", value);
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
	      
        [XmlAttribute("ItemName")]
        [Bindable(true)]
        public string ItemName 
	    {
		    get
		    {
			    return GetColumnValue<string>("ItemName");
		    }
            set 
		    {
			    SetColumnValue("ItemName", value);
            }
        }
	      
        [XmlAttribute("CategoryName")]
        [Bindable(true)]
        public string CategoryName 
	    {
		    get
		    {
			    return GetColumnValue<string>("CategoryName");
		    }
            set 
		    {
			    SetColumnValue("CategoryName", value);
            }
        }
	      
        [XmlAttribute("CostOfGoods")]
        [Bindable(true)]
        public decimal? CostOfGoods 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("CostOfGoods");
		    }
            set 
		    {
			    SetColumnValue("CostOfGoods", value);
            }
        }
	      
        [XmlAttribute("IsClosed")]
        [Bindable(true)]
        public bool? IsClosed 
	    {
		    get
		    {
			    return GetColumnValue<bool?>("IsClosed");
		    }
            set 
		    {
			    SetColumnValue("IsClosed", value);
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
            
            public static string DiscrepancyID = @"DiscrepancyID";
            
            public static string DiscrepancyReason = @"DiscrepancyReason";
            
            public static string DiscrepancyRemark = @"DiscrepancyRemark";
            
            public static string ItemNo = @"ItemNo";
            
            public static string DiscrepancyQuantity = @"DiscrepancyQuantity";
            
            public static string LocationTransferID = @"LocationTransferID";
            
            public static string ItemName = @"ItemName";
            
            public static string CategoryName = @"CategoryName";
            
            public static string CostOfGoods = @"CostOfGoods";
            
            public static string IsClosed = @"IsClosed";
            
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
