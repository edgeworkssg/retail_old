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
    /// Strongly-typed collection for the ViewMembershipTap class.
    /// </summary>
    [Serializable]
    public partial class ViewMembershipTapCollection : ReadOnlyList<ViewMembershipTap, ViewMembershipTapCollection>
    {        
        public ViewMembershipTapCollection() {}
    }
    /// <summary>
    /// This is  Read-only wrapper class for the ViewMembershipTap view.
    /// </summary>
    [Serializable]
    public partial class ViewMembershipTap : ReadOnlyRecord<ViewMembershipTap>, IReadOnlyRecord
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
                TableSchema.Table schema = new TableSchema.Table("ViewMembershipTap", TableType.View, DataService.GetInstance("PowerPOS"));
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
                
                TableSchema.TableColumn colvarMembershipGroupId = new TableSchema.TableColumn(schema);
                colvarMembershipGroupId.ColumnName = "MembershipGroupId";
                colvarMembershipGroupId.DataType = DbType.Int32;
                colvarMembershipGroupId.MaxLength = 0;
                colvarMembershipGroupId.AutoIncrement = false;
                colvarMembershipGroupId.IsNullable = false;
                colvarMembershipGroupId.IsPrimaryKey = false;
                colvarMembershipGroupId.IsForeignKey = false;
                colvarMembershipGroupId.IsReadOnly = false;
                
                schema.Columns.Add(colvarMembershipGroupId);
                
                TableSchema.TableColumn colvarTitle = new TableSchema.TableColumn(schema);
                colvarTitle.ColumnName = "Title";
                colvarTitle.DataType = DbType.AnsiString;
                colvarTitle.MaxLength = 5;
                colvarTitle.AutoIncrement = false;
                colvarTitle.IsNullable = true;
                colvarTitle.IsPrimaryKey = false;
                colvarTitle.IsForeignKey = false;
                colvarTitle.IsReadOnly = false;
                
                schema.Columns.Add(colvarTitle);
                
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
                
                TableSchema.TableColumn colvarGender = new TableSchema.TableColumn(schema);
                colvarGender.ColumnName = "Gender";
                colvarGender.DataType = DbType.String;
                colvarGender.MaxLength = 1;
                colvarGender.AutoIncrement = false;
                colvarGender.IsNullable = true;
                colvarGender.IsPrimaryKey = false;
                colvarGender.IsForeignKey = false;
                colvarGender.IsReadOnly = false;
                
                schema.Columns.Add(colvarGender);
                
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
                
                TableSchema.TableColumn colvarNationality = new TableSchema.TableColumn(schema);
                colvarNationality.ColumnName = "Nationality";
                colvarNationality.DataType = DbType.AnsiString;
                colvarNationality.MaxLength = 50;
                colvarNationality.AutoIncrement = false;
                colvarNationality.IsNullable = true;
                colvarNationality.IsPrimaryKey = false;
                colvarNationality.IsForeignKey = false;
                colvarNationality.IsReadOnly = false;
                
                schema.Columns.Add(colvarNationality);
                
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
                
                TableSchema.TableColumn colvarMaritalStatus = new TableSchema.TableColumn(schema);
                colvarMaritalStatus.ColumnName = "MaritalStatus";
                colvarMaritalStatus.DataType = DbType.AnsiString;
                colvarMaritalStatus.MaxLength = 50;
                colvarMaritalStatus.AutoIncrement = false;
                colvarMaritalStatus.IsNullable = true;
                colvarMaritalStatus.IsPrimaryKey = false;
                colvarMaritalStatus.IsForeignKey = false;
                colvarMaritalStatus.IsReadOnly = false;
                
                schema.Columns.Add(colvarMaritalStatus);
                
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
                
                TableSchema.TableColumn colvarBlock = new TableSchema.TableColumn(schema);
                colvarBlock.ColumnName = "Block";
                colvarBlock.DataType = DbType.AnsiString;
                colvarBlock.MaxLength = 50;
                colvarBlock.AutoIncrement = false;
                colvarBlock.IsNullable = true;
                colvarBlock.IsPrimaryKey = false;
                colvarBlock.IsForeignKey = false;
                colvarBlock.IsReadOnly = false;
                
                schema.Columns.Add(colvarBlock);
                
                TableSchema.TableColumn colvarBuildingName = new TableSchema.TableColumn(schema);
                colvarBuildingName.ColumnName = "BuildingName";
                colvarBuildingName.DataType = DbType.AnsiString;
                colvarBuildingName.MaxLength = 50;
                colvarBuildingName.AutoIncrement = false;
                colvarBuildingName.IsNullable = true;
                colvarBuildingName.IsPrimaryKey = false;
                colvarBuildingName.IsForeignKey = false;
                colvarBuildingName.IsReadOnly = false;
                
                schema.Columns.Add(colvarBuildingName);
                
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
                
                TableSchema.TableColumn colvarUnitNo = new TableSchema.TableColumn(schema);
                colvarUnitNo.ColumnName = "UnitNo";
                colvarUnitNo.DataType = DbType.AnsiString;
                colvarUnitNo.MaxLength = 50;
                colvarUnitNo.AutoIncrement = false;
                colvarUnitNo.IsNullable = true;
                colvarUnitNo.IsPrimaryKey = false;
                colvarUnitNo.IsForeignKey = false;
                colvarUnitNo.IsReadOnly = false;
                
                schema.Columns.Add(colvarUnitNo);
                
                TableSchema.TableColumn colvarCity = new TableSchema.TableColumn(schema);
                colvarCity.ColumnName = "City";
                colvarCity.DataType = DbType.AnsiString;
                colvarCity.MaxLength = 50;
                colvarCity.AutoIncrement = false;
                colvarCity.IsNullable = true;
                colvarCity.IsPrimaryKey = false;
                colvarCity.IsForeignKey = false;
                colvarCity.IsReadOnly = false;
                
                schema.Columns.Add(colvarCity);
                
                TableSchema.TableColumn colvarCountry = new TableSchema.TableColumn(schema);
                colvarCountry.ColumnName = "Country";
                colvarCountry.DataType = DbType.AnsiString;
                colvarCountry.MaxLength = 50;
                colvarCountry.AutoIncrement = false;
                colvarCountry.IsNullable = true;
                colvarCountry.IsPrimaryKey = false;
                colvarCountry.IsForeignKey = false;
                colvarCountry.IsReadOnly = false;
                
                schema.Columns.Add(colvarCountry);
                
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
                
                TableSchema.TableColumn colvarOffice = new TableSchema.TableColumn(schema);
                colvarOffice.ColumnName = "Office";
                colvarOffice.DataType = DbType.AnsiString;
                colvarOffice.MaxLength = 50;
                colvarOffice.AutoIncrement = false;
                colvarOffice.IsNullable = true;
                colvarOffice.IsPrimaryKey = false;
                colvarOffice.IsForeignKey = false;
                colvarOffice.IsReadOnly = false;
                
                schema.Columns.Add(colvarOffice);
                
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
                
                TableSchema.TableColumn colvarExpiryDate = new TableSchema.TableColumn(schema);
                colvarExpiryDate.ColumnName = "ExpiryDate";
                colvarExpiryDate.DataType = DbType.DateTime;
                colvarExpiryDate.MaxLength = 0;
                colvarExpiryDate.AutoIncrement = false;
                colvarExpiryDate.IsNullable = true;
                colvarExpiryDate.IsPrimaryKey = false;
                colvarExpiryDate.IsForeignKey = false;
                colvarExpiryDate.IsReadOnly = false;
                
                schema.Columns.Add(colvarExpiryDate);
                
                TableSchema.TableColumn colvarRemarks = new TableSchema.TableColumn(schema);
                colvarRemarks.ColumnName = "Remarks";
                colvarRemarks.DataType = DbType.AnsiString;
                colvarRemarks.MaxLength = 50;
                colvarRemarks.AutoIncrement = false;
                colvarRemarks.IsNullable = true;
                colvarRemarks.IsPrimaryKey = false;
                colvarRemarks.IsForeignKey = false;
                colvarRemarks.IsReadOnly = false;
                
                schema.Columns.Add(colvarRemarks);
                
                TableSchema.TableColumn colvarDeleted = new TableSchema.TableColumn(schema);
                colvarDeleted.ColumnName = "Deleted";
                colvarDeleted.DataType = DbType.Boolean;
                colvarDeleted.MaxLength = 0;
                colvarDeleted.AutoIncrement = false;
                colvarDeleted.IsNullable = true;
                colvarDeleted.IsPrimaryKey = false;
                colvarDeleted.IsForeignKey = false;
                colvarDeleted.IsReadOnly = false;
                
                schema.Columns.Add(colvarDeleted);
                
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
                
                TableSchema.TableColumn colvarAmount = new TableSchema.TableColumn(schema);
                colvarAmount.ColumnName = "Amount";
                colvarAmount.DataType = DbType.Currency;
                colvarAmount.MaxLength = 0;
                colvarAmount.AutoIncrement = false;
                colvarAmount.IsNullable = true;
                colvarAmount.IsPrimaryKey = false;
                colvarAmount.IsForeignKey = false;
                colvarAmount.IsReadOnly = false;
                
                schema.Columns.Add(colvarAmount);
                
                TableSchema.TableColumn colvarTapID = new TableSchema.TableColumn(schema);
                colvarTapID.ColumnName = "TapID";
                colvarTapID.DataType = DbType.Guid;
                colvarTapID.MaxLength = 0;
                colvarTapID.AutoIncrement = false;
                colvarTapID.IsNullable = true;
                colvarTapID.IsPrimaryKey = false;
                colvarTapID.IsForeignKey = false;
                colvarTapID.IsReadOnly = false;
                
                schema.Columns.Add(colvarTapID);
                
                
                BaseSchema = schema;
                //add this schema to the provider
                //so we can query it later
                DataService.Providers["PowerPOS"].AddSchema("ViewMembershipTap",schema);
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
	    public ViewMembershipTap()
	    {
            SetSQLProps();
            SetDefaults();
            MarkNew();
        }
        public ViewMembershipTap(bool useDatabaseDefaults)
	    {
		    SetSQLProps();
		    if(useDatabaseDefaults)
		    {
				ForceDefaults();
			}
			MarkNew();
	    }
	    
	    public ViewMembershipTap(object keyID)
	    {
		    SetSQLProps();
		    LoadByKey(keyID);
	    }
    	 
	    public ViewMembershipTap(string columnName, object columnValue)
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
	      
        [XmlAttribute("MembershipGroupId")]
        [Bindable(true)]
        public int MembershipGroupId 
	    {
		    get
		    {
			    return GetColumnValue<int>("MembershipGroupId");
		    }
            set 
		    {
			    SetColumnValue("MembershipGroupId", value);
            }
        }
	      
        [XmlAttribute("Title")]
        [Bindable(true)]
        public string Title 
	    {
		    get
		    {
			    return GetColumnValue<string>("Title");
		    }
            set 
		    {
			    SetColumnValue("Title", value);
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
	      
        [XmlAttribute("Gender")]
        [Bindable(true)]
        public string Gender 
	    {
		    get
		    {
			    return GetColumnValue<string>("Gender");
		    }
            set 
		    {
			    SetColumnValue("Gender", value);
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
	      
        [XmlAttribute("Nationality")]
        [Bindable(true)]
        public string Nationality 
	    {
		    get
		    {
			    return GetColumnValue<string>("Nationality");
		    }
            set 
		    {
			    SetColumnValue("Nationality", value);
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
	      
        [XmlAttribute("MaritalStatus")]
        [Bindable(true)]
        public string MaritalStatus 
	    {
		    get
		    {
			    return GetColumnValue<string>("MaritalStatus");
		    }
            set 
		    {
			    SetColumnValue("MaritalStatus", value);
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
	      
        [XmlAttribute("Block")]
        [Bindable(true)]
        public string Block 
	    {
		    get
		    {
			    return GetColumnValue<string>("Block");
		    }
            set 
		    {
			    SetColumnValue("Block", value);
            }
        }
	      
        [XmlAttribute("BuildingName")]
        [Bindable(true)]
        public string BuildingName 
	    {
		    get
		    {
			    return GetColumnValue<string>("BuildingName");
		    }
            set 
		    {
			    SetColumnValue("BuildingName", value);
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
	      
        [XmlAttribute("UnitNo")]
        [Bindable(true)]
        public string UnitNo 
	    {
		    get
		    {
			    return GetColumnValue<string>("UnitNo");
		    }
            set 
		    {
			    SetColumnValue("UnitNo", value);
            }
        }
	      
        [XmlAttribute("City")]
        [Bindable(true)]
        public string City 
	    {
		    get
		    {
			    return GetColumnValue<string>("City");
		    }
            set 
		    {
			    SetColumnValue("City", value);
            }
        }
	      
        [XmlAttribute("Country")]
        [Bindable(true)]
        public string Country 
	    {
		    get
		    {
			    return GetColumnValue<string>("Country");
		    }
            set 
		    {
			    SetColumnValue("Country", value);
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
	      
        [XmlAttribute("Office")]
        [Bindable(true)]
        public string Office 
	    {
		    get
		    {
			    return GetColumnValue<string>("Office");
		    }
            set 
		    {
			    SetColumnValue("Office", value);
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
	      
        [XmlAttribute("ExpiryDate")]
        [Bindable(true)]
        public DateTime? ExpiryDate 
	    {
		    get
		    {
			    return GetColumnValue<DateTime?>("ExpiryDate");
		    }
            set 
		    {
			    SetColumnValue("ExpiryDate", value);
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
	      
        [XmlAttribute("Deleted")]
        [Bindable(true)]
        public bool? Deleted 
	    {
		    get
		    {
			    return GetColumnValue<bool?>("Deleted");
		    }
            set 
		    {
			    SetColumnValue("Deleted", value);
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
	      
        [XmlAttribute("Amount")]
        [Bindable(true)]
        public decimal? Amount 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("Amount");
		    }
            set 
		    {
			    SetColumnValue("Amount", value);
            }
        }
	      
        [XmlAttribute("TapID")]
        [Bindable(true)]
        public Guid? TapID 
	    {
		    get
		    {
			    return GetColumnValue<Guid?>("TapID");
		    }
            set 
		    {
			    SetColumnValue("TapID", value);
            }
        }
	    
	    #endregion
    
	    #region Columns Struct
	    public struct Columns
	    {
		    
		    
            public static string MembershipNo = @"MembershipNo";
            
            public static string MembershipGroupId = @"MembershipGroupId";
            
            public static string Title = @"Title";
            
            public static string LastName = @"LastName";
            
            public static string FirstName = @"FirstName";
            
            public static string ChristianName = @"ChristianName";
            
            public static string NameToAppear = @"NameToAppear";
            
            public static string Gender = @"Gender";
            
            public static string DateOfBirth = @"DateOfBirth";
            
            public static string Nationality = @"Nationality";
            
            public static string Nric = @"NRIC";
            
            public static string Occupation = @"Occupation";
            
            public static string MaritalStatus = @"MaritalStatus";
            
            public static string Email = @"Email";
            
            public static string Block = @"Block";
            
            public static string BuildingName = @"BuildingName";
            
            public static string StreetName = @"StreetName";
            
            public static string UnitNo = @"UnitNo";
            
            public static string City = @"City";
            
            public static string Country = @"Country";
            
            public static string ZipCode = @"ZipCode";
            
            public static string Mobile = @"Mobile";
            
            public static string Office = @"Office";
            
            public static string Home = @"Home";
            
            public static string ExpiryDate = @"ExpiryDate";
            
            public static string Remarks = @"Remarks";
            
            public static string Deleted = @"Deleted";
            
            public static string GroupName = @"GroupName";
            
            public static string Amount = @"Amount";
            
            public static string TapID = @"TapID";
            
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
