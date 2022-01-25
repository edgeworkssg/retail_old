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
    /// Strongly-typed collection for the ViewPackageRedemption class.
    /// </summary>
    [Serializable]
    public partial class ViewPackageRedemptionCollection : ReadOnlyList<ViewPackageRedemption, ViewPackageRedemptionCollection>
    {        
        public ViewPackageRedemptionCollection() {}
    }
    /// <summary>
    /// This is  Read-only wrapper class for the ViewPackageRedemption view.
    /// </summary>
    [Serializable]
    public partial class ViewPackageRedemption : ReadOnlyRecord<ViewPackageRedemption>, IReadOnlyRecord
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
                TableSchema.Table schema = new TableSchema.Table("ViewPackageRedemption", TableType.View, DataService.GetInstance("PowerPOS"));
                schema.Columns = new TableSchema.TableColumnCollection();
                schema.SchemaName = @"dbo";
                //columns
                
                TableSchema.TableColumn colvarDisplayName = new TableSchema.TableColumn(schema);
                colvarDisplayName.ColumnName = "DisplayName";
                colvarDisplayName.DataType = DbType.AnsiString;
                colvarDisplayName.MaxLength = 50;
                colvarDisplayName.AutoIncrement = false;
                colvarDisplayName.IsNullable = true;
                colvarDisplayName.IsPrimaryKey = false;
                colvarDisplayName.IsForeignKey = false;
                colvarDisplayName.IsReadOnly = false;
                
                schema.Columns.Add(colvarDisplayName);
                
                TableSchema.TableColumn colvarIsASalesPerson = new TableSchema.TableColumn(schema);
                colvarIsASalesPerson.ColumnName = "IsASalesPerson";
                colvarIsASalesPerson.DataType = DbType.Boolean;
                colvarIsASalesPerson.MaxLength = 0;
                colvarIsASalesPerson.AutoIncrement = false;
                colvarIsASalesPerson.IsNullable = false;
                colvarIsASalesPerson.IsPrimaryKey = false;
                colvarIsASalesPerson.IsForeignKey = false;
                colvarIsASalesPerson.IsReadOnly = false;
                
                schema.Columns.Add(colvarIsASalesPerson);
                
                TableSchema.TableColumn colvarSalesPersonGroupID = new TableSchema.TableColumn(schema);
                colvarSalesPersonGroupID.ColumnName = "SalesPersonGroupID";
                colvarSalesPersonGroupID.DataType = DbType.Int32;
                colvarSalesPersonGroupID.MaxLength = 0;
                colvarSalesPersonGroupID.AutoIncrement = false;
                colvarSalesPersonGroupID.IsNullable = false;
                colvarSalesPersonGroupID.IsPrimaryKey = false;
                colvarSalesPersonGroupID.IsForeignKey = false;
                colvarSalesPersonGroupID.IsReadOnly = false;
                
                schema.Columns.Add(colvarSalesPersonGroupID);
                
                TableSchema.TableColumn colvarPackageRedeemID = new TableSchema.TableColumn(schema);
                colvarPackageRedeemID.ColumnName = "PackageRedeemID";
                colvarPackageRedeemID.DataType = DbType.Int32;
                colvarPackageRedeemID.MaxLength = 0;
                colvarPackageRedeemID.AutoIncrement = false;
                colvarPackageRedeemID.IsNullable = false;
                colvarPackageRedeemID.IsPrimaryKey = false;
                colvarPackageRedeemID.IsForeignKey = false;
                colvarPackageRedeemID.IsReadOnly = false;
                
                schema.Columns.Add(colvarPackageRedeemID);
                
                TableSchema.TableColumn colvarPackageRedeemDate = new TableSchema.TableColumn(schema);
                colvarPackageRedeemDate.ColumnName = "PackageRedeemDate";
                colvarPackageRedeemDate.DataType = DbType.DateTime;
                colvarPackageRedeemDate.MaxLength = 0;
                colvarPackageRedeemDate.AutoIncrement = false;
                colvarPackageRedeemDate.IsNullable = false;
                colvarPackageRedeemDate.IsPrimaryKey = false;
                colvarPackageRedeemDate.IsForeignKey = false;
                colvarPackageRedeemDate.IsReadOnly = false;
                
                schema.Columns.Add(colvarPackageRedeemDate);
                
                TableSchema.TableColumn colvarStylistId = new TableSchema.TableColumn(schema);
                colvarStylistId.ColumnName = "StylistId";
                colvarStylistId.DataType = DbType.AnsiString;
                colvarStylistId.MaxLength = 50;
                colvarStylistId.AutoIncrement = false;
                colvarStylistId.IsNullable = false;
                colvarStylistId.IsPrimaryKey = false;
                colvarStylistId.IsForeignKey = false;
                colvarStylistId.IsReadOnly = false;
                
                schema.Columns.Add(colvarStylistId);
                
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
                
                TableSchema.TableColumn colvarMembershipNo = new TableSchema.TableColumn(schema);
                colvarMembershipNo.ColumnName = "MembershipNo";
                colvarMembershipNo.DataType = DbType.AnsiString;
                colvarMembershipNo.MaxLength = 50;
                colvarMembershipNo.AutoIncrement = false;
                colvarMembershipNo.IsNullable = true;
                colvarMembershipNo.IsPrimaryKey = false;
                colvarMembershipNo.IsForeignKey = false;
                colvarMembershipNo.IsReadOnly = false;
                
                schema.Columns.Add(colvarMembershipNo);
                
                TableSchema.TableColumn colvarName = new TableSchema.TableColumn(schema);
                colvarName.ColumnName = "Name";
                colvarName.DataType = DbType.String;
                colvarName.MaxLength = 50;
                colvarName.AutoIncrement = false;
                colvarName.IsNullable = true;
                colvarName.IsPrimaryKey = false;
                colvarName.IsForeignKey = false;
                colvarName.IsReadOnly = false;
                
                schema.Columns.Add(colvarName);
                
                TableSchema.TableColumn colvarCreatedOn = new TableSchema.TableColumn(schema);
                colvarCreatedOn.ColumnName = "CreatedOn";
                colvarCreatedOn.DataType = DbType.DateTime;
                colvarCreatedOn.MaxLength = 0;
                colvarCreatedOn.AutoIncrement = false;
                colvarCreatedOn.IsNullable = true;
                colvarCreatedOn.IsPrimaryKey = false;
                colvarCreatedOn.IsForeignKey = false;
                colvarCreatedOn.IsReadOnly = false;
                
                schema.Columns.Add(colvarCreatedOn);
                
                TableSchema.TableColumn colvarCreatedBy = new TableSchema.TableColumn(schema);
                colvarCreatedBy.ColumnName = "CreatedBy";
                colvarCreatedBy.DataType = DbType.AnsiString;
                colvarCreatedBy.MaxLength = 50;
                colvarCreatedBy.AutoIncrement = false;
                colvarCreatedBy.IsNullable = true;
                colvarCreatedBy.IsPrimaryKey = false;
                colvarCreatedBy.IsForeignKey = false;
                colvarCreatedBy.IsReadOnly = false;
                
                schema.Columns.Add(colvarCreatedBy);
                
                TableSchema.TableColumn colvarModifiedOn = new TableSchema.TableColumn(schema);
                colvarModifiedOn.ColumnName = "ModifiedOn";
                colvarModifiedOn.DataType = DbType.DateTime;
                colvarModifiedOn.MaxLength = 0;
                colvarModifiedOn.AutoIncrement = false;
                colvarModifiedOn.IsNullable = true;
                colvarModifiedOn.IsPrimaryKey = false;
                colvarModifiedOn.IsForeignKey = false;
                colvarModifiedOn.IsReadOnly = false;
                
                schema.Columns.Add(colvarModifiedOn);
                
                TableSchema.TableColumn colvarModifiedBy = new TableSchema.TableColumn(schema);
                colvarModifiedBy.ColumnName = "ModifiedBy";
                colvarModifiedBy.DataType = DbType.AnsiString;
                colvarModifiedBy.MaxLength = 50;
                colvarModifiedBy.AutoIncrement = false;
                colvarModifiedBy.IsNullable = true;
                colvarModifiedBy.IsPrimaryKey = false;
                colvarModifiedBy.IsForeignKey = false;
                colvarModifiedBy.IsReadOnly = false;
                
                schema.Columns.Add(colvarModifiedBy);
                
                TableSchema.TableColumn colvarDeleted = new TableSchema.TableColumn(schema);
                colvarDeleted.ColumnName = "Deleted";
                colvarDeleted.DataType = DbType.Boolean;
                colvarDeleted.MaxLength = 0;
                colvarDeleted.AutoIncrement = false;
                colvarDeleted.IsNullable = false;
                colvarDeleted.IsPrimaryKey = false;
                colvarDeleted.IsForeignKey = false;
                colvarDeleted.IsReadOnly = false;
                
                schema.Columns.Add(colvarDeleted);
                
                TableSchema.TableColumn colvarUniqueID = new TableSchema.TableColumn(schema);
                colvarUniqueID.ColumnName = "UniqueID";
                colvarUniqueID.DataType = DbType.Guid;
                colvarUniqueID.MaxLength = 0;
                colvarUniqueID.AutoIncrement = false;
                colvarUniqueID.IsNullable = false;
                colvarUniqueID.IsPrimaryKey = false;
                colvarUniqueID.IsForeignKey = false;
                colvarUniqueID.IsReadOnly = false;
                
                schema.Columns.Add(colvarUniqueID);
                
                
                BaseSchema = schema;
                //add this schema to the provider
                //so we can query it later
                DataService.Providers["PowerPOS"].AddSchema("ViewPackageRedemption",schema);
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
	    public ViewPackageRedemption()
	    {
            SetSQLProps();
            SetDefaults();
            MarkNew();
        }
        public ViewPackageRedemption(bool useDatabaseDefaults)
	    {
		    SetSQLProps();
		    if(useDatabaseDefaults)
		    {
				ForceDefaults();
			}
			MarkNew();
	    }
	    
	    public ViewPackageRedemption(object keyID)
	    {
		    SetSQLProps();
		    LoadByKey(keyID);
	    }
    	 
	    public ViewPackageRedemption(string columnName, object columnValue)
        {
            SetSQLProps();
            LoadByParam(columnName,columnValue);
        }
        
	    #endregion
	    
	    #region Props
	    
          
        [XmlAttribute("DisplayName")]
        [Bindable(true)]
        public string DisplayName 
	    {
		    get
		    {
			    return GetColumnValue<string>("DisplayName");
		    }
            set 
		    {
			    SetColumnValue("DisplayName", value);
            }
        }
	      
        [XmlAttribute("IsASalesPerson")]
        [Bindable(true)]
        public bool IsASalesPerson 
	    {
		    get
		    {
			    return GetColumnValue<bool>("IsASalesPerson");
		    }
            set 
		    {
			    SetColumnValue("IsASalesPerson", value);
            }
        }
	      
        [XmlAttribute("SalesPersonGroupID")]
        [Bindable(true)]
        public int SalesPersonGroupID 
	    {
		    get
		    {
			    return GetColumnValue<int>("SalesPersonGroupID");
		    }
            set 
		    {
			    SetColumnValue("SalesPersonGroupID", value);
            }
        }
	      
        [XmlAttribute("PackageRedeemID")]
        [Bindable(true)]
        public int PackageRedeemID 
	    {
		    get
		    {
			    return GetColumnValue<int>("PackageRedeemID");
		    }
            set 
		    {
			    SetColumnValue("PackageRedeemID", value);
            }
        }
	      
        [XmlAttribute("PackageRedeemDate")]
        [Bindable(true)]
        public DateTime PackageRedeemDate 
	    {
		    get
		    {
			    return GetColumnValue<DateTime>("PackageRedeemDate");
		    }
            set 
		    {
			    SetColumnValue("PackageRedeemDate", value);
            }
        }
	      
        [XmlAttribute("StylistId")]
        [Bindable(true)]
        public string StylistId 
	    {
		    get
		    {
			    return GetColumnValue<string>("StylistId");
		    }
            set 
		    {
			    SetColumnValue("StylistId", value);
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
	      
        [XmlAttribute("Name")]
        [Bindable(true)]
        public string Name 
	    {
		    get
		    {
			    return GetColumnValue<string>("Name");
		    }
            set 
		    {
			    SetColumnValue("Name", value);
            }
        }
	      
        [XmlAttribute("CreatedOn")]
        [Bindable(true)]
        public DateTime? CreatedOn 
	    {
		    get
		    {
			    return GetColumnValue<DateTime?>("CreatedOn");
		    }
            set 
		    {
			    SetColumnValue("CreatedOn", value);
            }
        }
	      
        [XmlAttribute("CreatedBy")]
        [Bindable(true)]
        public string CreatedBy 
	    {
		    get
		    {
			    return GetColumnValue<string>("CreatedBy");
		    }
            set 
		    {
			    SetColumnValue("CreatedBy", value);
            }
        }
	      
        [XmlAttribute("ModifiedOn")]
        [Bindable(true)]
        public DateTime? ModifiedOn 
	    {
		    get
		    {
			    return GetColumnValue<DateTime?>("ModifiedOn");
		    }
            set 
		    {
			    SetColumnValue("ModifiedOn", value);
            }
        }
	      
        [XmlAttribute("ModifiedBy")]
        [Bindable(true)]
        public string ModifiedBy 
	    {
		    get
		    {
			    return GetColumnValue<string>("ModifiedBy");
		    }
            set 
		    {
			    SetColumnValue("ModifiedBy", value);
            }
        }
	      
        [XmlAttribute("Deleted")]
        [Bindable(true)]
        public bool Deleted 
	    {
		    get
		    {
			    return GetColumnValue<bool>("Deleted");
		    }
            set 
		    {
			    SetColumnValue("Deleted", value);
            }
        }
	      
        [XmlAttribute("UniqueID")]
        [Bindable(true)]
        public Guid UniqueID 
	    {
		    get
		    {
			    return GetColumnValue<Guid>("UniqueID");
		    }
            set 
		    {
			    SetColumnValue("UniqueID", value);
            }
        }
	    
	    #endregion
    
	    #region Columns Struct
	    public struct Columns
	    {
		    
		    
            public static string DisplayName = @"DisplayName";
            
            public static string IsASalesPerson = @"IsASalesPerson";
            
            public static string SalesPersonGroupID = @"SalesPersonGroupID";
            
            public static string PackageRedeemID = @"PackageRedeemID";
            
            public static string PackageRedeemDate = @"PackageRedeemDate";
            
            public static string StylistId = @"StylistId";
            
            public static string Amount = @"Amount";
            
            public static string MembershipNo = @"MembershipNo";
            
            public static string Name = @"Name";
            
            public static string CreatedOn = @"CreatedOn";
            
            public static string CreatedBy = @"CreatedBy";
            
            public static string ModifiedOn = @"ModifiedOn";
            
            public static string ModifiedBy = @"ModifiedBy";
            
            public static string Deleted = @"Deleted";
            
            public static string UniqueID = @"UniqueID";
            
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
