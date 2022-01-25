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
    /// Strongly-typed collection for the ViewMembershipUpgrade class.
    /// </summary>
    [Serializable]
    public partial class ViewMembershipUpgradeCollection : ReadOnlyList<ViewMembershipUpgrade, ViewMembershipUpgradeCollection>
    {        
        public ViewMembershipUpgradeCollection() {}
    }
    /// <summary>
    /// This is  Read-only wrapper class for the ViewMembershipUpgrades view.
    /// </summary>
    [Serializable]
    public partial class ViewMembershipUpgrade : ReadOnlyRecord<ViewMembershipUpgrade>, IReadOnlyRecord
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
                TableSchema.Table schema = new TableSchema.Table("ViewMembershipUpgrades", TableType.View, DataService.GetInstance("PowerPOS"));
                schema.Columns = new TableSchema.TableColumnCollection();
                schema.SchemaName = @"dbo";
                //columns
                
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
                
                TableSchema.TableColumn colvarMobile = new TableSchema.TableColumn(schema);
                colvarMobile.ColumnName = "Mobile";
                colvarMobile.DataType = DbType.AnsiString;
                colvarMobile.MaxLength = 50;
                colvarMobile.AutoIncrement = false;
                colvarMobile.IsNullable = true;
                colvarMobile.IsPrimaryKey = false;
                colvarMobile.IsForeignKey = false;
                colvarMobile.IsReadOnly = false;
                
                schema.Columns.Add(colvarMobile);
                
                TableSchema.TableColumn colvarHome = new TableSchema.TableColumn(schema);
                colvarHome.ColumnName = "Home";
                colvarHome.DataType = DbType.AnsiString;
                colvarHome.MaxLength = 50;
                colvarHome.AutoIncrement = false;
                colvarHome.IsNullable = true;
                colvarHome.IsPrimaryKey = false;
                colvarHome.IsForeignKey = false;
                colvarHome.IsReadOnly = false;
                
                schema.Columns.Add(colvarHome);
                
                TableSchema.TableColumn colvarStreetName = new TableSchema.TableColumn(schema);
                colvarStreetName.ColumnName = "StreetName";
                colvarStreetName.DataType = DbType.AnsiString;
                colvarStreetName.MaxLength = -1;
                colvarStreetName.AutoIncrement = false;
                colvarStreetName.IsNullable = true;
                colvarStreetName.IsPrimaryKey = false;
                colvarStreetName.IsForeignKey = false;
                colvarStreetName.IsReadOnly = false;
                
                schema.Columns.Add(colvarStreetName);
                
                TableSchema.TableColumn colvarStreetName2 = new TableSchema.TableColumn(schema);
                colvarStreetName2.ColumnName = "StreetName2";
                colvarStreetName2.DataType = DbType.AnsiString;
                colvarStreetName2.MaxLength = -1;
                colvarStreetName2.AutoIncrement = false;
                colvarStreetName2.IsNullable = true;
                colvarStreetName2.IsPrimaryKey = false;
                colvarStreetName2.IsForeignKey = false;
                colvarStreetName2.IsReadOnly = false;
                
                schema.Columns.Add(colvarStreetName2);
                
                TableSchema.TableColumn colvarZipCode = new TableSchema.TableColumn(schema);
                colvarZipCode.ColumnName = "ZipCode";
                colvarZipCode.DataType = DbType.AnsiString;
                colvarZipCode.MaxLength = 50;
                colvarZipCode.AutoIncrement = false;
                colvarZipCode.IsNullable = true;
                colvarZipCode.IsPrimaryKey = false;
                colvarZipCode.IsForeignKey = false;
                colvarZipCode.IsReadOnly = false;
                
                schema.Columns.Add(colvarZipCode);
                
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
                
                TableSchema.TableColumn colvarIsVitaMix = new TableSchema.TableColumn(schema);
                colvarIsVitaMix.ColumnName = "IsVitaMix";
                colvarIsVitaMix.DataType = DbType.Boolean;
                colvarIsVitaMix.MaxLength = 0;
                colvarIsVitaMix.AutoIncrement = false;
                colvarIsVitaMix.IsNullable = true;
                colvarIsVitaMix.IsPrimaryKey = false;
                colvarIsVitaMix.IsForeignKey = false;
                colvarIsVitaMix.IsReadOnly = false;
                
                schema.Columns.Add(colvarIsVitaMix);
                
                TableSchema.TableColumn colvarIsWaterFilter = new TableSchema.TableColumn(schema);
                colvarIsWaterFilter.ColumnName = "IsWaterFilter";
                colvarIsWaterFilter.DataType = DbType.Boolean;
                colvarIsWaterFilter.MaxLength = 0;
                colvarIsWaterFilter.AutoIncrement = false;
                colvarIsWaterFilter.IsNullable = true;
                colvarIsWaterFilter.IsPrimaryKey = false;
                colvarIsWaterFilter.IsForeignKey = false;
                colvarIsWaterFilter.IsReadOnly = false;
                
                schema.Columns.Add(colvarIsWaterFilter);
                
                TableSchema.TableColumn colvarIsYoung = new TableSchema.TableColumn(schema);
                colvarIsYoung.ColumnName = "IsYoung";
                colvarIsYoung.DataType = DbType.Boolean;
                colvarIsYoung.MaxLength = 0;
                colvarIsYoung.AutoIncrement = false;
                colvarIsYoung.IsNullable = true;
                colvarIsYoung.IsPrimaryKey = false;
                colvarIsYoung.IsForeignKey = false;
                colvarIsYoung.IsReadOnly = false;
                
                schema.Columns.Add(colvarIsYoung);
                
                TableSchema.TableColumn colvarIsJuicePlus = new TableSchema.TableColumn(schema);
                colvarIsJuicePlus.ColumnName = "IsJuicePlus";
                colvarIsJuicePlus.DataType = DbType.Boolean;
                colvarIsJuicePlus.MaxLength = 0;
                colvarIsJuicePlus.AutoIncrement = false;
                colvarIsJuicePlus.IsNullable = true;
                colvarIsJuicePlus.IsPrimaryKey = false;
                colvarIsJuicePlus.IsForeignKey = false;
                colvarIsJuicePlus.IsReadOnly = false;
                
                schema.Columns.Add(colvarIsJuicePlus);
                
                TableSchema.TableColumn colvarEmail = new TableSchema.TableColumn(schema);
                colvarEmail.ColumnName = "Email";
                colvarEmail.DataType = DbType.AnsiString;
                colvarEmail.MaxLength = 50;
                colvarEmail.AutoIncrement = false;
                colvarEmail.IsNullable = true;
                colvarEmail.IsPrimaryKey = false;
                colvarEmail.IsForeignKey = false;
                colvarEmail.IsReadOnly = false;
                
                schema.Columns.Add(colvarEmail);
                
                TableSchema.TableColumn colvarChineseName = new TableSchema.TableColumn(schema);
                colvarChineseName.ColumnName = "ChineseName";
                colvarChineseName.DataType = DbType.String;
                colvarChineseName.MaxLength = 50;
                colvarChineseName.AutoIncrement = false;
                colvarChineseName.IsNullable = true;
                colvarChineseName.IsPrimaryKey = false;
                colvarChineseName.IsForeignKey = false;
                colvarChineseName.IsReadOnly = false;
                
                schema.Columns.Add(colvarChineseName);
                
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
                
                TableSchema.TableColumn colvarChristianName = new TableSchema.TableColumn(schema);
                colvarChristianName.ColumnName = "ChristianName";
                colvarChristianName.DataType = DbType.AnsiString;
                colvarChristianName.MaxLength = 80;
                colvarChristianName.AutoIncrement = false;
                colvarChristianName.IsNullable = true;
                colvarChristianName.IsPrimaryKey = false;
                colvarChristianName.IsForeignKey = false;
                colvarChristianName.IsReadOnly = false;
                
                schema.Columns.Add(colvarChristianName);
                
                TableSchema.TableColumn colvarDateOfBirth = new TableSchema.TableColumn(schema);
                colvarDateOfBirth.ColumnName = "DateOfBirth";
                colvarDateOfBirth.DataType = DbType.DateTime;
                colvarDateOfBirth.MaxLength = 0;
                colvarDateOfBirth.AutoIncrement = false;
                colvarDateOfBirth.IsNullable = true;
                colvarDateOfBirth.IsPrimaryKey = false;
                colvarDateOfBirth.IsForeignKey = false;
                colvarDateOfBirth.IsReadOnly = false;
                
                schema.Columns.Add(colvarDateOfBirth);
                
                TableSchema.TableColumn colvarOccupation = new TableSchema.TableColumn(schema);
                colvarOccupation.ColumnName = "Occupation";
                colvarOccupation.DataType = DbType.AnsiString;
                colvarOccupation.MaxLength = 100;
                colvarOccupation.AutoIncrement = false;
                colvarOccupation.IsNullable = true;
                colvarOccupation.IsPrimaryKey = false;
                colvarOccupation.IsForeignKey = false;
                colvarOccupation.IsReadOnly = false;
                
                schema.Columns.Add(colvarOccupation);
                
                TableSchema.TableColumn colvarRemarks = new TableSchema.TableColumn(schema);
                colvarRemarks.ColumnName = "Remarks";
                colvarRemarks.DataType = DbType.String;
                colvarRemarks.MaxLength = -1;
                colvarRemarks.AutoIncrement = false;
                colvarRemarks.IsNullable = true;
                colvarRemarks.IsPrimaryKey = false;
                colvarRemarks.IsForeignKey = false;
                colvarRemarks.IsReadOnly = false;
                
                schema.Columns.Add(colvarRemarks);
                
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
                
                TableSchema.TableColumn colvarOrderDate = new TableSchema.TableColumn(schema);
                colvarOrderDate.ColumnName = "OrderDate";
                colvarOrderDate.DataType = DbType.DateTime;
                colvarOrderDate.MaxLength = 0;
                colvarOrderDate.AutoIncrement = false;
                colvarOrderDate.IsNullable = false;
                colvarOrderDate.IsPrimaryKey = false;
                colvarOrderDate.IsForeignKey = false;
                colvarOrderDate.IsReadOnly = false;
                
                schema.Columns.Add(colvarOrderDate);
                
                
                BaseSchema = schema;
                //add this schema to the provider
                //so we can query it later
                DataService.Providers["PowerPOS"].AddSchema("ViewMembershipUpgrades",schema);
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
	    public ViewMembershipUpgrade()
	    {
            SetSQLProps();
            SetDefaults();
            MarkNew();
        }
        public ViewMembershipUpgrade(bool useDatabaseDefaults)
	    {
		    SetSQLProps();
		    if(useDatabaseDefaults)
		    {
				ForceDefaults();
			}
			MarkNew();
	    }
	    
	    public ViewMembershipUpgrade(object keyID)
	    {
		    SetSQLProps();
		    LoadByKey(keyID);
	    }
    	 
	    public ViewMembershipUpgrade(string columnName, object columnValue)
        {
            SetSQLProps();
            LoadByParam(columnName,columnValue);
        }
        
	    #endregion
	    
	    #region Props
	    
          
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
	      
        [XmlAttribute("Mobile")]
        [Bindable(true)]
        public string Mobile 
	    {
		    get
		    {
			    return GetColumnValue<string>("Mobile");
		    }
            set 
		    {
			    SetColumnValue("Mobile", value);
            }
        }
	      
        [XmlAttribute("Home")]
        [Bindable(true)]
        public string Home 
	    {
		    get
		    {
			    return GetColumnValue<string>("Home");
		    }
            set 
		    {
			    SetColumnValue("Home", value);
            }
        }
	      
        [XmlAttribute("StreetName")]
        [Bindable(true)]
        public string StreetName 
	    {
		    get
		    {
			    return GetColumnValue<string>("StreetName");
		    }
            set 
		    {
			    SetColumnValue("StreetName", value);
            }
        }
	      
        [XmlAttribute("StreetName2")]
        [Bindable(true)]
        public string StreetName2 
	    {
		    get
		    {
			    return GetColumnValue<string>("StreetName2");
		    }
            set 
		    {
			    SetColumnValue("StreetName2", value);
            }
        }
	      
        [XmlAttribute("ZipCode")]
        [Bindable(true)]
        public string ZipCode 
	    {
		    get
		    {
			    return GetColumnValue<string>("ZipCode");
		    }
            set 
		    {
			    SetColumnValue("ZipCode", value);
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
	      
        [XmlAttribute("IsVitaMix")]
        [Bindable(true)]
        public bool? IsVitaMix 
	    {
		    get
		    {
			    return GetColumnValue<bool?>("IsVitaMix");
		    }
            set 
		    {
			    SetColumnValue("IsVitaMix", value);
            }
        }
	      
        [XmlAttribute("IsWaterFilter")]
        [Bindable(true)]
        public bool? IsWaterFilter 
	    {
		    get
		    {
			    return GetColumnValue<bool?>("IsWaterFilter");
		    }
            set 
		    {
			    SetColumnValue("IsWaterFilter", value);
            }
        }
	      
        [XmlAttribute("IsYoung")]
        [Bindable(true)]
        public bool? IsYoung 
	    {
		    get
		    {
			    return GetColumnValue<bool?>("IsYoung");
		    }
            set 
		    {
			    SetColumnValue("IsYoung", value);
            }
        }
	      
        [XmlAttribute("IsJuicePlus")]
        [Bindable(true)]
        public bool? IsJuicePlus 
	    {
		    get
		    {
			    return GetColumnValue<bool?>("IsJuicePlus");
		    }
            set 
		    {
			    SetColumnValue("IsJuicePlus", value);
            }
        }
	      
        [XmlAttribute("Email")]
        [Bindable(true)]
        public string Email 
	    {
		    get
		    {
			    return GetColumnValue<string>("Email");
		    }
            set 
		    {
			    SetColumnValue("Email", value);
            }
        }
	      
        [XmlAttribute("ChineseName")]
        [Bindable(true)]
        public string ChineseName 
	    {
		    get
		    {
			    return GetColumnValue<string>("ChineseName");
		    }
            set 
		    {
			    SetColumnValue("ChineseName", value);
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
	      
        [XmlAttribute("ChristianName")]
        [Bindable(true)]
        public string ChristianName 
	    {
		    get
		    {
			    return GetColumnValue<string>("ChristianName");
		    }
            set 
		    {
			    SetColumnValue("ChristianName", value);
            }
        }
	      
        [XmlAttribute("DateOfBirth")]
        [Bindable(true)]
        public DateTime? DateOfBirth 
	    {
		    get
		    {
			    return GetColumnValue<DateTime?>("DateOfBirth");
		    }
            set 
		    {
			    SetColumnValue("DateOfBirth", value);
            }
        }
	      
        [XmlAttribute("Occupation")]
        [Bindable(true)]
        public string Occupation 
	    {
		    get
		    {
			    return GetColumnValue<string>("Occupation");
		    }
            set 
		    {
			    SetColumnValue("Occupation", value);
            }
        }
	      
        [XmlAttribute("Remarks")]
        [Bindable(true)]
        public string Remarks 
	    {
		    get
		    {
			    return GetColumnValue<string>("Remarks");
		    }
            set 
		    {
			    SetColumnValue("Remarks", value);
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
	      
        [XmlAttribute("OrderDate")]
        [Bindable(true)]
        public DateTime OrderDate 
	    {
		    get
		    {
			    return GetColumnValue<DateTime>("OrderDate");
		    }
            set 
		    {
			    SetColumnValue("OrderDate", value);
            }
        }
	    
	    #endregion
    
	    #region Columns Struct
	    public struct Columns
	    {
		    
		    
            public static string MembershipNo = @"MembershipNo";
            
            public static string GroupName = @"GroupName";
            
            public static string NameToAppear = @"NameToAppear";
            
            public static string Mobile = @"Mobile";
            
            public static string Home = @"Home";
            
            public static string StreetName = @"StreetName";
            
            public static string StreetName2 = @"StreetName2";
            
            public static string ZipCode = @"ZipCode";
            
            public static string Nric = @"NRIC";
            
            public static string IsVitaMix = @"IsVitaMix";
            
            public static string IsWaterFilter = @"IsWaterFilter";
            
            public static string IsYoung = @"IsYoung";
            
            public static string IsJuicePlus = @"IsJuicePlus";
            
            public static string Email = @"Email";
            
            public static string ChineseName = @"ChineseName";
            
            public static string FirstName = @"FirstName";
            
            public static string LastName = @"LastName";
            
            public static string ChristianName = @"ChristianName";
            
            public static string DateOfBirth = @"DateOfBirth";
            
            public static string Occupation = @"Occupation";
            
            public static string Remarks = @"Remarks";
            
            public static string OrderHdrID = @"OrderHdrID";
            
            public static string OrderDate = @"OrderDate";
            
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
