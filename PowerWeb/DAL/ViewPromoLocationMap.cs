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
    /// Strongly-typed collection for the ViewPromoLocationMap class.
    /// </summary>
    [Serializable]
    public partial class ViewPromoLocationMapCollection : ReadOnlyList<ViewPromoLocationMap, ViewPromoLocationMapCollection>
    {        
        public ViewPromoLocationMapCollection() {}
    }
    /// <summary>
    /// This is  Read-only wrapper class for the ViewPromoLocationMap view.
    /// </summary>
    [Serializable]
    public partial class ViewPromoLocationMap : ReadOnlyRecord<ViewPromoLocationMap>, IReadOnlyRecord
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
                TableSchema.Table schema = new TableSchema.Table("ViewPromoLocationMap", TableType.View, DataService.GetInstance("PowerPOS"));
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
                
                TableSchema.TableColumn colvarForAllLocations = new TableSchema.TableColumn(schema);
                colvarForAllLocations.ColumnName = "ForAllLocations";
                colvarForAllLocations.DataType = DbType.Boolean;
                colvarForAllLocations.MaxLength = 0;
                colvarForAllLocations.AutoIncrement = false;
                colvarForAllLocations.IsNullable = true;
                colvarForAllLocations.IsPrimaryKey = false;
                colvarForAllLocations.IsForeignKey = false;
                colvarForAllLocations.IsReadOnly = false;
                
                schema.Columns.Add(colvarForAllLocations);
                
                TableSchema.TableColumn colvarForNonMembersAlso = new TableSchema.TableColumn(schema);
                colvarForNonMembersAlso.ColumnName = "ForNonMembersAlso";
                colvarForNonMembersAlso.DataType = DbType.Boolean;
                colvarForNonMembersAlso.MaxLength = 0;
                colvarForNonMembersAlso.AutoIncrement = false;
                colvarForNonMembersAlso.IsNullable = true;
                colvarForNonMembersAlso.IsPrimaryKey = false;
                colvarForNonMembersAlso.IsForeignKey = false;
                colvarForNonMembersAlso.IsReadOnly = false;
                
                schema.Columns.Add(colvarForNonMembersAlso);
                
                TableSchema.TableColumn colvarPromoLocationMapID = new TableSchema.TableColumn(schema);
                colvarPromoLocationMapID.ColumnName = "PromoLocationMapID";
                colvarPromoLocationMapID.DataType = DbType.Guid;
                colvarPromoLocationMapID.MaxLength = 0;
                colvarPromoLocationMapID.AutoIncrement = false;
                colvarPromoLocationMapID.IsNullable = false;
                colvarPromoLocationMapID.IsPrimaryKey = false;
                colvarPromoLocationMapID.IsForeignKey = false;
                colvarPromoLocationMapID.IsReadOnly = false;
                
                schema.Columns.Add(colvarPromoLocationMapID);
                
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
                
                
                BaseSchema = schema;
                //add this schema to the provider
                //so we can query it later
                DataService.Providers["PowerPOS"].AddSchema("ViewPromoLocationMap",schema);
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
	    public ViewPromoLocationMap()
	    {
            SetSQLProps();
            SetDefaults();
            MarkNew();
        }
        public ViewPromoLocationMap(bool useDatabaseDefaults)
	    {
		    SetSQLProps();
		    if(useDatabaseDefaults)
		    {
				ForceDefaults();
			}
			MarkNew();
	    }
	    
	    public ViewPromoLocationMap(object keyID)
	    {
		    SetSQLProps();
		    LoadByKey(keyID);
	    }
    	 
	    public ViewPromoLocationMap(string columnName, object columnValue)
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
	      
        [XmlAttribute("ForAllLocations")]
        [Bindable(true)]
        public bool? ForAllLocations 
	    {
		    get
		    {
			    return GetColumnValue<bool?>("ForAllLocations");
		    }
            set 
		    {
			    SetColumnValue("ForAllLocations", value);
            }
        }
	      
        [XmlAttribute("ForNonMembersAlso")]
        [Bindable(true)]
        public bool? ForNonMembersAlso 
	    {
		    get
		    {
			    return GetColumnValue<bool?>("ForNonMembersAlso");
		    }
            set 
		    {
			    SetColumnValue("ForNonMembersAlso", value);
            }
        }
	      
        [XmlAttribute("PromoLocationMapID")]
        [Bindable(true)]
        public Guid PromoLocationMapID 
	    {
		    get
		    {
			    return GetColumnValue<Guid>("PromoLocationMapID");
		    }
            set 
		    {
			    SetColumnValue("PromoLocationMapID", value);
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
	    
	    #endregion
    
	    #region Columns Struct
	    public struct Columns
	    {
		    
		    
            public static string PromoCampaignName = @"PromoCampaignName";
            
            public static string CampaignType = @"CampaignType";
            
            public static string DateFrom = @"DateFrom";
            
            public static string DateTo = @"DateTo";
            
            public static string PointOfSaleID = @"PointOfSaleID";
            
            public static string PointOfSaleName = @"PointOfSaleName";
            
            public static string OutletName = @"OutletName";
            
            public static string Enabled = @"Enabled";
            
            public static string ForAllLocations = @"ForAllLocations";
            
            public static string ForNonMembersAlso = @"ForNonMembersAlso";
            
            public static string PromoLocationMapID = @"PromoLocationMapID";
            
            public static string PromoCampaignHdrID = @"PromoCampaignHdrID";
            
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
