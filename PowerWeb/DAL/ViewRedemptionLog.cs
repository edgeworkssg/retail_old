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
    /// Strongly-typed collection for the ViewRedemptionLog class.
    /// </summary>
    [Serializable]
    public partial class ViewRedemptionLogCollection : ReadOnlyList<ViewRedemptionLog, ViewRedemptionLogCollection>
    {        
        public ViewRedemptionLogCollection() {}
    }
    /// <summary>
    /// This is  Read-only wrapper class for the ViewRedemptionLog view.
    /// </summary>
    [Serializable]
    public partial class ViewRedemptionLog : ReadOnlyRecord<ViewRedemptionLog>, IReadOnlyRecord
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
                TableSchema.Table schema = new TableSchema.Table("ViewRedemptionLog", TableType.View, DataService.GetInstance("PowerPOS"));
                schema.Columns = new TableSchema.TableColumnCollection();
                schema.SchemaName = @"dbo";
                //columns
                
                TableSchema.TableColumn colvarRedemptionDate = new TableSchema.TableColumn(schema);
                colvarRedemptionDate.ColumnName = "RedemptionDate";
                colvarRedemptionDate.DataType = DbType.DateTime;
                colvarRedemptionDate.MaxLength = 0;
                colvarRedemptionDate.AutoIncrement = false;
                colvarRedemptionDate.IsNullable = true;
                colvarRedemptionDate.IsPrimaryKey = false;
                colvarRedemptionDate.IsForeignKey = false;
                colvarRedemptionDate.IsReadOnly = false;
                
                schema.Columns.Add(colvarRedemptionDate);
                
                TableSchema.TableColumn colvarMembershipNo = new TableSchema.TableColumn(schema);
                colvarMembershipNo.ColumnName = "MembershipNo";
                colvarMembershipNo.DataType = DbType.AnsiString;
                colvarMembershipNo.MaxLength = 50;
                colvarMembershipNo.AutoIncrement = false;
                colvarMembershipNo.IsNullable = false;
                colvarMembershipNo.IsPrimaryKey = false;
                colvarMembershipNo.IsForeignKey = false;
                colvarMembershipNo.IsReadOnly = false;
                
                schema.Columns.Add(colvarMembershipNo);
                
                TableSchema.TableColumn colvarPoints = new TableSchema.TableColumn(schema);
                colvarPoints.ColumnName = "Points";
                colvarPoints.DataType = DbType.Currency;
                colvarPoints.MaxLength = 0;
                colvarPoints.AutoIncrement = false;
                colvarPoints.IsNullable = false;
                colvarPoints.IsPrimaryKey = false;
                colvarPoints.IsForeignKey = false;
                colvarPoints.IsReadOnly = false;
                
                schema.Columns.Add(colvarPoints);
                
                TableSchema.TableColumn colvarOrderHdrID = new TableSchema.TableColumn(schema);
                colvarOrderHdrID.ColumnName = "OrderHdrID";
                colvarOrderHdrID.DataType = DbType.AnsiString;
                colvarOrderHdrID.MaxLength = 14;
                colvarOrderHdrID.AutoIncrement = false;
                colvarOrderHdrID.IsNullable = false;
                colvarOrderHdrID.IsPrimaryKey = false;
                colvarOrderHdrID.IsForeignKey = false;
                colvarOrderHdrID.IsReadOnly = false;
                
                schema.Columns.Add(colvarOrderHdrID);
                
                TableSchema.TableColumn colvarNameToAppear = new TableSchema.TableColumn(schema);
                colvarNameToAppear.ColumnName = "NameToAppear";
                colvarNameToAppear.DataType = DbType.AnsiString;
                colvarNameToAppear.MaxLength = 80;
                colvarNameToAppear.AutoIncrement = false;
                colvarNameToAppear.IsNullable = true;
                colvarNameToAppear.IsPrimaryKey = false;
                colvarNameToAppear.IsForeignKey = false;
                colvarNameToAppear.IsReadOnly = false;
                
                schema.Columns.Add(colvarNameToAppear);
                
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
                
                TableSchema.TableColumn colvarLastName = new TableSchema.TableColumn(schema);
                colvarLastName.ColumnName = "LastName";
                colvarLastName.DataType = DbType.AnsiString;
                colvarLastName.MaxLength = 80;
                colvarLastName.AutoIncrement = false;
                colvarLastName.IsNullable = true;
                colvarLastName.IsPrimaryKey = false;
                colvarLastName.IsForeignKey = false;
                colvarLastName.IsReadOnly = false;
                
                schema.Columns.Add(colvarLastName);
                
                TableSchema.TableColumn colvarFirstName = new TableSchema.TableColumn(schema);
                colvarFirstName.ColumnName = "FirstName";
                colvarFirstName.DataType = DbType.AnsiString;
                colvarFirstName.MaxLength = 80;
                colvarFirstName.AutoIncrement = false;
                colvarFirstName.IsNullable = true;
                colvarFirstName.IsPrimaryKey = false;
                colvarFirstName.IsForeignKey = false;
                colvarFirstName.IsReadOnly = false;
                
                schema.Columns.Add(colvarFirstName);
                
                TableSchema.TableColumn colvarNettAmount = new TableSchema.TableColumn(schema);
                colvarNettAmount.ColumnName = "NettAmount";
                colvarNettAmount.DataType = DbType.Currency;
                colvarNettAmount.MaxLength = 0;
                colvarNettAmount.AutoIncrement = false;
                colvarNettAmount.IsNullable = false;
                colvarNettAmount.IsPrimaryKey = false;
                colvarNettAmount.IsForeignKey = false;
                colvarNettAmount.IsReadOnly = false;
                
                schema.Columns.Add(colvarNettAmount);
                
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
                
                TableSchema.TableColumn colvarNric = new TableSchema.TableColumn(schema);
                colvarNric.ColumnName = "NRIC";
                colvarNric.DataType = DbType.AnsiString;
                colvarNric.MaxLength = 50;
                colvarNric.AutoIncrement = false;
                colvarNric.IsNullable = true;
                colvarNric.IsPrimaryKey = false;
                colvarNric.IsForeignKey = false;
                colvarNric.IsReadOnly = false;
                
                schema.Columns.Add(colvarNric);
                
                
                BaseSchema = schema;
                //add this schema to the provider
                //so we can query it later
                DataService.Providers["PowerPOS"].AddSchema("ViewRedemptionLog",schema);
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
	    public ViewRedemptionLog()
	    {
            SetSQLProps();
            SetDefaults();
            MarkNew();
        }
        public ViewRedemptionLog(bool useDatabaseDefaults)
	    {
		    SetSQLProps();
		    if(useDatabaseDefaults)
		    {
				ForceDefaults();
			}
			MarkNew();
	    }
	    
	    public ViewRedemptionLog(object keyID)
	    {
		    SetSQLProps();
		    LoadByKey(keyID);
	    }
    	 
	    public ViewRedemptionLog(string columnName, object columnValue)
        {
            SetSQLProps();
            LoadByParam(columnName,columnValue);
        }
        
	    #endregion
	    
	    #region Props
	    
          
        [XmlAttribute("RedemptionDate")]
        [Bindable(true)]
        public DateTime? RedemptionDate 
	    {
		    get
		    {
			    return GetColumnValue<DateTime?>("RedemptionDate");
		    }
            set 
		    {
			    SetColumnValue("RedemptionDate", value);
            }
        }
	      
        [XmlAttribute("MembershipNo")]
        [Bindable(true)]
        public string MembershipNo 
	    {
		    get
		    {
			    return GetColumnValue<string>("MembershipNo");
		    }
            set 
		    {
			    SetColumnValue("MembershipNo", value);
            }
        }
	      
        [XmlAttribute("Points")]
        [Bindable(true)]
        public decimal Points 
	    {
		    get
		    {
			    return GetColumnValue<decimal>("Points");
		    }
            set 
		    {
			    SetColumnValue("Points", value);
            }
        }
	      
        [XmlAttribute("OrderHdrID")]
        [Bindable(true)]
        public string OrderHdrID 
	    {
		    get
		    {
			    return GetColumnValue<string>("OrderHdrID");
		    }
            set 
		    {
			    SetColumnValue("OrderHdrID", value);
            }
        }
	      
        [XmlAttribute("NameToAppear")]
        [Bindable(true)]
        public string NameToAppear 
	    {
		    get
		    {
			    return GetColumnValue<string>("NameToAppear");
		    }
            set 
		    {
			    SetColumnValue("NameToAppear", value);
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
	      
        [XmlAttribute("LastName")]
        [Bindable(true)]
        public string LastName 
	    {
		    get
		    {
			    return GetColumnValue<string>("LastName");
		    }
            set 
		    {
			    SetColumnValue("LastName", value);
            }
        }
	      
        [XmlAttribute("FirstName")]
        [Bindable(true)]
        public string FirstName 
	    {
		    get
		    {
			    return GetColumnValue<string>("FirstName");
		    }
            set 
		    {
			    SetColumnValue("FirstName", value);
            }
        }
	      
        [XmlAttribute("NettAmount")]
        [Bindable(true)]
        public decimal NettAmount 
	    {
		    get
		    {
			    return GetColumnValue<decimal>("NettAmount");
		    }
            set 
		    {
			    SetColumnValue("NettAmount", value);
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
	      
        [XmlAttribute("Nric")]
        [Bindable(true)]
        public string Nric 
	    {
		    get
		    {
			    return GetColumnValue<string>("NRIC");
		    }
            set 
		    {
			    SetColumnValue("NRIC", value);
            }
        }
	    
	    #endregion
    
	    #region Columns Struct
	    public struct Columns
	    {
		    
		    
            public static string RedemptionDate = @"RedemptionDate";
            
            public static string MembershipNo = @"MembershipNo";
            
            public static string Points = @"Points";
            
            public static string OrderHdrID = @"OrderHdrID";
            
            public static string NameToAppear = @"NameToAppear";
            
            public static string GroupName = @"GroupName";
            
            public static string LastName = @"LastName";
            
            public static string FirstName = @"FirstName";
            
            public static string NettAmount = @"NettAmount";
            
            public static string PointOfSaleName = @"PointOfSaleName";
            
            public static string OutletName = @"OutletName";
            
            public static string Nric = @"NRIC";
            
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
