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
    /// Strongly-typed collection for the ViewPromoMembershipMap class.
    /// </summary>
    [Serializable]
    public partial class ViewPromoMembershipMapCollection : ReadOnlyList<ViewPromoMembershipMap, ViewPromoMembershipMapCollection>
    {        
        public ViewPromoMembershipMapCollection() {}
    }
    /// <summary>
    /// This is  Read-only wrapper class for the ViewPromoMembershipMap view.
    /// </summary>
    [Serializable]
    public partial class ViewPromoMembershipMap : ReadOnlyRecord<ViewPromoMembershipMap>, IReadOnlyRecord
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
                TableSchema.Table schema = new TableSchema.Table("ViewPromoMembershipMap", TableType.View, DataService.GetInstance("PowerPOS"));
                schema.Columns = new TableSchema.TableColumnCollection();
                schema.SchemaName = @"dbo";
                //columns
                
                TableSchema.TableColumn colvarPromoCampaignName = new TableSchema.TableColumn(schema);
                colvarPromoCampaignName.ColumnName = "PromoCampaignName";
                colvarPromoCampaignName.DataType = DbType.AnsiString;
                colvarPromoCampaignName.MaxLength = 50;
                colvarPromoCampaignName.AutoIncrement = false;
                colvarPromoCampaignName.IsNullable = false;
                colvarPromoCampaignName.IsPrimaryKey = false;
                colvarPromoCampaignName.IsForeignKey = false;
                colvarPromoCampaignName.IsReadOnly = false;
                
                schema.Columns.Add(colvarPromoCampaignName);
                
                TableSchema.TableColumn colvarCampaignType = new TableSchema.TableColumn(schema);
                colvarCampaignType.ColumnName = "CampaignType";
                colvarCampaignType.DataType = DbType.AnsiString;
                colvarCampaignType.MaxLength = 50;
                colvarCampaignType.AutoIncrement = false;
                colvarCampaignType.IsNullable = false;
                colvarCampaignType.IsPrimaryKey = false;
                colvarCampaignType.IsForeignKey = false;
                colvarCampaignType.IsReadOnly = false;
                
                schema.Columns.Add(colvarCampaignType);
                
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
                
                TableSchema.TableColumn colvarDateFrom = new TableSchema.TableColumn(schema);
                colvarDateFrom.ColumnName = "DateFrom";
                colvarDateFrom.DataType = DbType.DateTime;
                colvarDateFrom.MaxLength = 0;
                colvarDateFrom.AutoIncrement = false;
                colvarDateFrom.IsNullable = false;
                colvarDateFrom.IsPrimaryKey = false;
                colvarDateFrom.IsForeignKey = false;
                colvarDateFrom.IsReadOnly = false;
                
                schema.Columns.Add(colvarDateFrom);
                
                TableSchema.TableColumn colvarDateTo = new TableSchema.TableColumn(schema);
                colvarDateTo.ColumnName = "DateTo";
                colvarDateTo.DataType = DbType.DateTime;
                colvarDateTo.MaxLength = 0;
                colvarDateTo.AutoIncrement = false;
                colvarDateTo.IsNullable = false;
                colvarDateTo.IsPrimaryKey = false;
                colvarDateTo.IsForeignKey = false;
                colvarDateTo.IsReadOnly = false;
                
                schema.Columns.Add(colvarDateTo);
                
                TableSchema.TableColumn colvarEnabled = new TableSchema.TableColumn(schema);
                colvarEnabled.ColumnName = "Enabled";
                colvarEnabled.DataType = DbType.Boolean;
                colvarEnabled.MaxLength = 0;
                colvarEnabled.AutoIncrement = false;
                colvarEnabled.IsNullable = false;
                colvarEnabled.IsPrimaryKey = false;
                colvarEnabled.IsForeignKey = false;
                colvarEnabled.IsReadOnly = false;
                
                schema.Columns.Add(colvarEnabled);
                
                TableSchema.TableColumn colvarPromoMembershipID = new TableSchema.TableColumn(schema);
                colvarPromoMembershipID.ColumnName = "PromoMembershipID";
                colvarPromoMembershipID.DataType = DbType.Guid;
                colvarPromoMembershipID.MaxLength = 0;
                colvarPromoMembershipID.AutoIncrement = false;
                colvarPromoMembershipID.IsNullable = false;
                colvarPromoMembershipID.IsPrimaryKey = false;
                colvarPromoMembershipID.IsForeignKey = false;
                colvarPromoMembershipID.IsReadOnly = false;
                
                schema.Columns.Add(colvarPromoMembershipID);
                
                TableSchema.TableColumn colvarMembershipPrice = new TableSchema.TableColumn(schema);
                colvarMembershipPrice.ColumnName = "MembershipPrice";
                colvarMembershipPrice.DataType = DbType.Currency;
                colvarMembershipPrice.MaxLength = 0;
                colvarMembershipPrice.AutoIncrement = false;
                colvarMembershipPrice.IsNullable = false;
                colvarMembershipPrice.IsPrimaryKey = false;
                colvarMembershipPrice.IsForeignKey = false;
                colvarMembershipPrice.IsReadOnly = false;
                
                schema.Columns.Add(colvarMembershipPrice);
                
                TableSchema.TableColumn colvarGroupName = new TableSchema.TableColumn(schema);
                colvarGroupName.ColumnName = "GroupName";
                colvarGroupName.DataType = DbType.AnsiString;
                colvarGroupName.MaxLength = 50;
                colvarGroupName.AutoIncrement = false;
                colvarGroupName.IsNullable = false;
                colvarGroupName.IsPrimaryKey = false;
                colvarGroupName.IsForeignKey = false;
                colvarGroupName.IsReadOnly = false;
                
                schema.Columns.Add(colvarGroupName);
                
                TableSchema.TableColumn colvarMembershipDiscount = new TableSchema.TableColumn(schema);
                colvarMembershipDiscount.ColumnName = "MembershipDiscount";
                colvarMembershipDiscount.DataType = DbType.Decimal;
                colvarMembershipDiscount.MaxLength = 0;
                colvarMembershipDiscount.AutoIncrement = false;
                colvarMembershipDiscount.IsNullable = false;
                colvarMembershipDiscount.IsPrimaryKey = false;
                colvarMembershipDiscount.IsForeignKey = false;
                colvarMembershipDiscount.IsReadOnly = false;
                
                schema.Columns.Add(colvarMembershipDiscount);
                
                TableSchema.TableColumn colvarUseMembershipPrice = new TableSchema.TableColumn(schema);
                colvarUseMembershipPrice.ColumnName = "UseMembershipPrice";
                colvarUseMembershipPrice.DataType = DbType.Boolean;
                colvarUseMembershipPrice.MaxLength = 0;
                colvarUseMembershipPrice.AutoIncrement = false;
                colvarUseMembershipPrice.IsNullable = false;
                colvarUseMembershipPrice.IsPrimaryKey = false;
                colvarUseMembershipPrice.IsForeignKey = false;
                colvarUseMembershipPrice.IsReadOnly = false;
                
                schema.Columns.Add(colvarUseMembershipPrice);
                
                
                BaseSchema = schema;
                //add this schema to the provider
                //so we can query it later
                DataService.Providers["PowerPOS"].AddSchema("ViewPromoMembershipMap",schema);
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
	    public ViewPromoMembershipMap()
	    {
            SetSQLProps();
            SetDefaults();
            MarkNew();
        }
        public ViewPromoMembershipMap(bool useDatabaseDefaults)
	    {
		    SetSQLProps();
		    if(useDatabaseDefaults)
		    {
				ForceDefaults();
			}
			MarkNew();
	    }
	    
	    public ViewPromoMembershipMap(object keyID)
	    {
		    SetSQLProps();
		    LoadByKey(keyID);
	    }
    	 
	    public ViewPromoMembershipMap(string columnName, object columnValue)
        {
            SetSQLProps();
            LoadByParam(columnName,columnValue);
        }
        
	    #endregion
	    
	    #region Props
	    
          
        [XmlAttribute("PromoCampaignName")]
        [Bindable(true)]
        public string PromoCampaignName 
	    {
		    get
		    {
			    return GetColumnValue<string>("PromoCampaignName");
		    }
            set 
		    {
			    SetColumnValue("PromoCampaignName", value);
            }
        }
	      
        [XmlAttribute("CampaignType")]
        [Bindable(true)]
        public string CampaignType 
	    {
		    get
		    {
			    return GetColumnValue<string>("CampaignType");
		    }
            set 
		    {
			    SetColumnValue("CampaignType", value);
            }
        }
	      
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
	      
        [XmlAttribute("DateFrom")]
        [Bindable(true)]
        public DateTime DateFrom 
	    {
		    get
		    {
			    return GetColumnValue<DateTime>("DateFrom");
		    }
            set 
		    {
			    SetColumnValue("DateFrom", value);
            }
        }
	      
        [XmlAttribute("DateTo")]
        [Bindable(true)]
        public DateTime DateTo 
	    {
		    get
		    {
			    return GetColumnValue<DateTime>("DateTo");
		    }
            set 
		    {
			    SetColumnValue("DateTo", value);
            }
        }
	      
        [XmlAttribute("Enabled")]
        [Bindable(true)]
        public bool Enabled 
	    {
		    get
		    {
			    return GetColumnValue<bool>("Enabled");
		    }
            set 
		    {
			    SetColumnValue("Enabled", value);
            }
        }
	      
        [XmlAttribute("PromoMembershipID")]
        [Bindable(true)]
        public Guid PromoMembershipID 
	    {
		    get
		    {
			    return GetColumnValue<Guid>("PromoMembershipID");
		    }
            set 
		    {
			    SetColumnValue("PromoMembershipID", value);
            }
        }
	      
        [XmlAttribute("MembershipPrice")]
        [Bindable(true)]
        public decimal MembershipPrice 
	    {
		    get
		    {
			    return GetColumnValue<decimal>("MembershipPrice");
		    }
            set 
		    {
			    SetColumnValue("MembershipPrice", value);
            }
        }
	      
        [XmlAttribute("GroupName")]
        [Bindable(true)]
        public string GroupName 
	    {
		    get
		    {
			    return GetColumnValue<string>("GroupName");
		    }
            set 
		    {
			    SetColumnValue("GroupName", value);
            }
        }
	      
        [XmlAttribute("MembershipDiscount")]
        [Bindable(true)]
        public decimal MembershipDiscount 
	    {
		    get
		    {
			    return GetColumnValue<decimal>("MembershipDiscount");
		    }
            set 
		    {
			    SetColumnValue("MembershipDiscount", value);
            }
        }
	      
        [XmlAttribute("UseMembershipPrice")]
        [Bindable(true)]
        public bool UseMembershipPrice 
	    {
		    get
		    {
			    return GetColumnValue<bool>("UseMembershipPrice");
		    }
            set 
		    {
			    SetColumnValue("UseMembershipPrice", value);
            }
        }
	    
	    #endregion
    
	    #region Columns Struct
	    public struct Columns
	    {
		    
		    
            public static string PromoCampaignName = @"PromoCampaignName";
            
            public static string CampaignType = @"CampaignType";
            
            public static string PromoCampaignHdrID = @"PromoCampaignHdrID";
            
            public static string DateFrom = @"DateFrom";
            
            public static string DateTo = @"DateTo";
            
            public static string Enabled = @"Enabled";
            
            public static string PromoMembershipID = @"PromoMembershipID";
            
            public static string MembershipPrice = @"MembershipPrice";
            
            public static string GroupName = @"GroupName";
            
            public static string MembershipDiscount = @"MembershipDiscount";
            
            public static string UseMembershipPrice = @"UseMembershipPrice";
            
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
