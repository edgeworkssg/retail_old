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
    /// Strongly-typed collection for the ViewPromoMasterDetailAny class.
    /// </summary>
    [Serializable]
    public partial class ViewPromoMasterDetailAnyCollection : ReadOnlyList<ViewPromoMasterDetailAny, ViewPromoMasterDetailAnyCollection>
    {        
        public ViewPromoMasterDetailAnyCollection() {}
    }
    /// <summary>
    /// This is  Read-only wrapper class for the ViewPromoMasterDetailAny view.
    /// </summary>
    [Serializable]
    public partial class ViewPromoMasterDetailAny : ReadOnlyRecord<ViewPromoMasterDetailAny>, IReadOnlyRecord
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
                TableSchema.Table schema = new TableSchema.Table("ViewPromoMasterDetailAny", TableType.View, DataService.GetInstance("PowerPOS"));
                schema.Columns = new TableSchema.TableColumnCollection();
                schema.SchemaName = @"dbo";
                //columns
                
                TableSchema.TableColumn colvarPromoCampaignHdrID = new TableSchema.TableColumn(schema);
                colvarPromoCampaignHdrID.ColumnName = "PromoCampaignHdrID";
                colvarPromoCampaignHdrID.DataType = DbType.Int32;
                colvarPromoCampaignHdrID.MaxLength = 0;
                colvarPromoCampaignHdrID.AutoIncrement = false;
                colvarPromoCampaignHdrID.IsNullable = false;
                colvarPromoCampaignHdrID.IsPrimaryKey = false;
                colvarPromoCampaignHdrID.IsForeignKey = false;
                colvarPromoCampaignHdrID.IsReadOnly = false;
                
                schema.Columns.Add(colvarPromoCampaignHdrID);
                
                TableSchema.TableColumn colvarPromoCampaignDetID = new TableSchema.TableColumn(schema);
                colvarPromoCampaignDetID.ColumnName = "PromoCampaignDetID";
                colvarPromoCampaignDetID.DataType = DbType.Int32;
                colvarPromoCampaignDetID.MaxLength = 0;
                colvarPromoCampaignDetID.AutoIncrement = false;
                colvarPromoCampaignDetID.IsNullable = false;
                colvarPromoCampaignDetID.IsPrimaryKey = false;
                colvarPromoCampaignDetID.IsForeignKey = false;
                colvarPromoCampaignDetID.IsReadOnly = false;
                
                schema.Columns.Add(colvarPromoCampaignDetID);
                
                TableSchema.TableColumn colvarItemNo = new TableSchema.TableColumn(schema);
                colvarItemNo.ColumnName = "ItemNo";
                colvarItemNo.DataType = DbType.AnsiString;
                colvarItemNo.MaxLength = 50;
                colvarItemNo.AutoIncrement = false;
                colvarItemNo.IsNullable = true;
                colvarItemNo.IsPrimaryKey = false;
                colvarItemNo.IsForeignKey = false;
                colvarItemNo.IsReadOnly = false;
                
                schema.Columns.Add(colvarItemNo);
                
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
                
                TableSchema.TableColumn colvarUnitQty = new TableSchema.TableColumn(schema);
                colvarUnitQty.ColumnName = "UnitQty";
                colvarUnitQty.DataType = DbType.Int32;
                colvarUnitQty.MaxLength = 0;
                colvarUnitQty.AutoIncrement = false;
                colvarUnitQty.IsNullable = true;
                colvarUnitQty.IsPrimaryKey = false;
                colvarUnitQty.IsForeignKey = false;
                colvarUnitQty.IsReadOnly = false;
                
                schema.Columns.Add(colvarUnitQty);
                
                TableSchema.TableColumn colvarAnyQty = new TableSchema.TableColumn(schema);
                colvarAnyQty.ColumnName = "AnyQty";
                colvarAnyQty.DataType = DbType.Int32;
                colvarAnyQty.MaxLength = 0;
                colvarAnyQty.AutoIncrement = false;
                colvarAnyQty.IsNullable = false;
                colvarAnyQty.IsPrimaryKey = false;
                colvarAnyQty.IsForeignKey = false;
                colvarAnyQty.IsReadOnly = false;
                
                schema.Columns.Add(colvarAnyQty);
                
                TableSchema.TableColumn colvarMinQuantity = new TableSchema.TableColumn(schema);
                colvarMinQuantity.ColumnName = "MinQuantity";
                colvarMinQuantity.DataType = DbType.Int32;
                colvarMinQuantity.MaxLength = 0;
                colvarMinQuantity.AutoIncrement = false;
                colvarMinQuantity.IsNullable = false;
                colvarMinQuantity.IsPrimaryKey = false;
                colvarMinQuantity.IsForeignKey = false;
                colvarMinQuantity.IsReadOnly = false;
                
                schema.Columns.Add(colvarMinQuantity);
                
                TableSchema.TableColumn colvarPromoPrice = new TableSchema.TableColumn(schema);
                colvarPromoPrice.ColumnName = "PromoPrice";
                colvarPromoPrice.DataType = DbType.Currency;
                colvarPromoPrice.MaxLength = 0;
                colvarPromoPrice.AutoIncrement = false;
                colvarPromoPrice.IsNullable = false;
                colvarPromoPrice.IsPrimaryKey = false;
                colvarPromoPrice.IsForeignKey = false;
                colvarPromoPrice.IsReadOnly = false;
                
                schema.Columns.Add(colvarPromoPrice);
                
                TableSchema.TableColumn colvarDiscAmount = new TableSchema.TableColumn(schema);
                colvarDiscAmount.ColumnName = "DiscAmount";
                colvarDiscAmount.DataType = DbType.Currency;
                colvarDiscAmount.MaxLength = 0;
                colvarDiscAmount.AutoIncrement = false;
                colvarDiscAmount.IsNullable = false;
                colvarDiscAmount.IsPrimaryKey = false;
                colvarDiscAmount.IsForeignKey = false;
                colvarDiscAmount.IsReadOnly = false;
                
                schema.Columns.Add(colvarDiscAmount);
                
                TableSchema.TableColumn colvarDiscPercent = new TableSchema.TableColumn(schema);
                colvarDiscPercent.ColumnName = "DiscPercent";
                colvarDiscPercent.DataType = DbType.Decimal;
                colvarDiscPercent.MaxLength = 0;
                colvarDiscPercent.AutoIncrement = false;
                colvarDiscPercent.IsNullable = true;
                colvarDiscPercent.IsPrimaryKey = false;
                colvarDiscPercent.IsForeignKey = false;
                colvarDiscPercent.IsReadOnly = false;
                
                schema.Columns.Add(colvarDiscPercent);
                
                
                BaseSchema = schema;
                //add this schema to the provider
                //so we can query it later
                DataService.Providers["PowerPOS"].AddSchema("ViewPromoMasterDetailAny",schema);
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
	    public ViewPromoMasterDetailAny()
	    {
            SetSQLProps();
            SetDefaults();
            MarkNew();
        }
        public ViewPromoMasterDetailAny(bool useDatabaseDefaults)
	    {
		    SetSQLProps();
		    if(useDatabaseDefaults)
		    {
				ForceDefaults();
			}
			MarkNew();
	    }
	    
	    public ViewPromoMasterDetailAny(object keyID)
	    {
		    SetSQLProps();
		    LoadByKey(keyID);
	    }
    	 
	    public ViewPromoMasterDetailAny(string columnName, object columnValue)
        {
            SetSQLProps();
            LoadByParam(columnName,columnValue);
        }
        
	    #endregion
	    
	    #region Props
	    
          
        [XmlAttribute("PromoCampaignHdrID")]
        [Bindable(true)]
        public int PromoCampaignHdrID 
	    {
		    get
		    {
			    return GetColumnValue<int>("PromoCampaignHdrID");
		    }
            set 
		    {
			    SetColumnValue("PromoCampaignHdrID", value);
            }
        }
	      
        [XmlAttribute("PromoCampaignDetID")]
        [Bindable(true)]
        public int PromoCampaignDetID 
	    {
		    get
		    {
			    return GetColumnValue<int>("PromoCampaignDetID");
		    }
            set 
		    {
			    SetColumnValue("PromoCampaignDetID", value);
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
	      
        [XmlAttribute("UnitQty")]
        [Bindable(true)]
        public int? UnitQty 
	    {
		    get
		    {
			    return GetColumnValue<int?>("UnitQty");
		    }
            set 
		    {
			    SetColumnValue("UnitQty", value);
            }
        }
	      
        [XmlAttribute("AnyQty")]
        [Bindable(true)]
        public int AnyQty 
	    {
		    get
		    {
			    return GetColumnValue<int>("AnyQty");
		    }
            set 
		    {
			    SetColumnValue("AnyQty", value);
            }
        }
	      
        [XmlAttribute("MinQuantity")]
        [Bindable(true)]
        public int MinQuantity 
	    {
		    get
		    {
			    return GetColumnValue<int>("MinQuantity");
		    }
            set 
		    {
			    SetColumnValue("MinQuantity", value);
            }
        }
	      
        [XmlAttribute("PromoPrice")]
        [Bindable(true)]
        public decimal PromoPrice 
	    {
		    get
		    {
			    return GetColumnValue<decimal>("PromoPrice");
		    }
            set 
		    {
			    SetColumnValue("PromoPrice", value);
            }
        }
	      
        [XmlAttribute("DiscAmount")]
        [Bindable(true)]
        public decimal DiscAmount 
	    {
		    get
		    {
			    return GetColumnValue<decimal>("DiscAmount");
		    }
            set 
		    {
			    SetColumnValue("DiscAmount", value);
            }
        }
	      
        [XmlAttribute("DiscPercent")]
        [Bindable(true)]
        public decimal? DiscPercent 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("DiscPercent");
		    }
            set 
		    {
			    SetColumnValue("DiscPercent", value);
            }
        }
	    
	    #endregion
    
	    #region Columns Struct
	    public struct Columns
	    {
		    
		    
            public static string PromoCampaignHdrID = @"PromoCampaignHdrID";
            
            public static string PromoCampaignDetID = @"PromoCampaignDetID";
            
            public static string ItemNo = @"ItemNo";
            
            public static string ItemName = @"ItemName";
            
            public static string UnitQty = @"UnitQty";
            
            public static string AnyQty = @"AnyQty";
            
            public static string MinQuantity = @"MinQuantity";
            
            public static string PromoPrice = @"PromoPrice";
            
            public static string DiscAmount = @"DiscAmount";
            
            public static string DiscPercent = @"DiscPercent";
            
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
