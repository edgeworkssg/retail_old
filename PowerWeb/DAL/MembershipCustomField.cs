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
namespace PowerPOS
{
	/// <summary>
	/// Strongly-typed collection for the MembershipCustomField class.
	/// </summary>
    [Serializable]
	public partial class MembershipCustomFieldCollection : ActiveList<MembershipCustomField, MembershipCustomFieldCollection>
	{	   
		public MembershipCustomFieldCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>MembershipCustomFieldCollection</returns>
		public MembershipCustomFieldCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                MembershipCustomField o = this[i];
                foreach (SubSonic.Where w in this.wheres)
                {
                    bool remove = false;
                    System.Reflection.PropertyInfo pi = o.GetType().GetProperty(w.ColumnName);
                    if (pi.CanRead)
                    {
                        object val = pi.GetValue(o, null);
                        switch (w.Comparison)
                        {
                            case SubSonic.Comparison.Equals:
                                if (!val.Equals(w.ParameterValue))
                                {
                                    remove = true;
                                }
                                break;
                        }
                    }
                    if (remove)
                    {
                        this.Remove(o);
                        break;
                    }
                }
            }
            return this;
        }
		
		
	}
	/// <summary>
	/// This is an ActiveRecord class which wraps the MembershipCustomFields table.
	/// </summary>
	[Serializable]
	public partial class MembershipCustomField : ActiveRecord<MembershipCustomField>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public MembershipCustomField()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public MembershipCustomField(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public MembershipCustomField(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public MembershipCustomField(string columnName, object columnValue)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByParam(columnName,columnValue);
		}
		
		protected static void SetSQLProps() { GetTableSchema(); }
		
		#endregion
		
		#region Schema and Query Accessor	
		public static Query CreateQuery() { return new Query(Schema); }
		public static TableSchema.Table Schema
		{
			get
			{
				if (BaseSchema == null)
					SetSQLProps();
				return BaseSchema;
			}
		}
		
		private static void GetTableSchema() 
		{
			if(!IsSchemaInitialized)
			{
				//Schema declaration
				TableSchema.Table schema = new TableSchema.Table("MembershipCustomFields", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarMembershipCustomFieldID = new TableSchema.TableColumn(schema);
				colvarMembershipCustomFieldID.ColumnName = "MembershipCustomFieldID";
				colvarMembershipCustomFieldID.DataType = DbType.Guid;
				colvarMembershipCustomFieldID.MaxLength = 0;
				colvarMembershipCustomFieldID.AutoIncrement = false;
				colvarMembershipCustomFieldID.IsNullable = false;
				colvarMembershipCustomFieldID.IsPrimaryKey = true;
				colvarMembershipCustomFieldID.IsForeignKey = false;
				colvarMembershipCustomFieldID.IsReadOnly = false;
				
						colvarMembershipCustomFieldID.DefaultSetting = @"(newid())";
				colvarMembershipCustomFieldID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMembershipCustomFieldID);
				
				TableSchema.TableColumn colvarFieldName = new TableSchema.TableColumn(schema);
				colvarFieldName.ColumnName = "FieldName";
				colvarFieldName.DataType = DbType.AnsiString;
				colvarFieldName.MaxLength = 50;
				colvarFieldName.AutoIncrement = false;
				colvarFieldName.IsNullable = true;
				colvarFieldName.IsPrimaryKey = false;
				colvarFieldName.IsForeignKey = false;
				colvarFieldName.IsReadOnly = false;
				colvarFieldName.DefaultSetting = @"";
				colvarFieldName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarFieldName);
				
				TableSchema.TableColumn colvarLabel = new TableSchema.TableColumn(schema);
				colvarLabel.ColumnName = "Label";
				colvarLabel.DataType = DbType.String;
				colvarLabel.MaxLength = -1;
				colvarLabel.AutoIncrement = false;
				colvarLabel.IsNullable = true;
				colvarLabel.IsPrimaryKey = false;
				colvarLabel.IsForeignKey = false;
				colvarLabel.IsReadOnly = false;
				colvarLabel.DefaultSetting = @"";
				colvarLabel.ForeignKeyTableName = "";
				schema.Columns.Add(colvarLabel);
				
				TableSchema.TableColumn colvarType = new TableSchema.TableColumn(schema);
				colvarType.ColumnName = "Type";
				colvarType.DataType = DbType.AnsiString;
				colvarType.MaxLength = 50;
				colvarType.AutoIncrement = false;
				colvarType.IsNullable = true;
				colvarType.IsPrimaryKey = false;
				colvarType.IsForeignKey = false;
				colvarType.IsReadOnly = false;
				colvarType.DefaultSetting = @"";
				colvarType.ForeignKeyTableName = "";
				schema.Columns.Add(colvarType);
				
				TableSchema.TableColumn colvarEnumList = new TableSchema.TableColumn(schema);
				colvarEnumList.ColumnName = "EnumList";
				colvarEnumList.DataType = DbType.String;
				colvarEnumList.MaxLength = -1;
				colvarEnumList.AutoIncrement = false;
				colvarEnumList.IsNullable = true;
				colvarEnumList.IsPrimaryKey = false;
				colvarEnumList.IsForeignKey = false;
				colvarEnumList.IsReadOnly = false;
				colvarEnumList.DefaultSetting = @"";
				colvarEnumList.ForeignKeyTableName = "";
				schema.Columns.Add(colvarEnumList);
				
				TableSchema.TableColumn colvarDeleted = new TableSchema.TableColumn(schema);
				colvarDeleted.ColumnName = "Deleted";
				colvarDeleted.DataType = DbType.Boolean;
				colvarDeleted.MaxLength = 0;
				colvarDeleted.AutoIncrement = false;
				colvarDeleted.IsNullable = true;
				colvarDeleted.IsPrimaryKey = false;
				colvarDeleted.IsForeignKey = false;
				colvarDeleted.IsReadOnly = false;
				colvarDeleted.DefaultSetting = @"";
				colvarDeleted.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDeleted);
				
				TableSchema.TableColumn colvarCreatedOn = new TableSchema.TableColumn(schema);
				colvarCreatedOn.ColumnName = "CreatedOn";
				colvarCreatedOn.DataType = DbType.DateTime;
				colvarCreatedOn.MaxLength = 0;
				colvarCreatedOn.AutoIncrement = false;
				colvarCreatedOn.IsNullable = true;
				colvarCreatedOn.IsPrimaryKey = false;
				colvarCreatedOn.IsForeignKey = false;
				colvarCreatedOn.IsReadOnly = false;
				colvarCreatedOn.DefaultSetting = @"";
				colvarCreatedOn.ForeignKeyTableName = "";
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
				colvarCreatedBy.DefaultSetting = @"";
				colvarCreatedBy.ForeignKeyTableName = "";
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
				colvarModifiedOn.DefaultSetting = @"";
				colvarModifiedOn.ForeignKeyTableName = "";
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
				colvarModifiedBy.DefaultSetting = @"";
				colvarModifiedBy.ForeignKeyTableName = "";
				schema.Columns.Add(colvarModifiedBy);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["PowerPOS"].AddSchema("MembershipCustomFields",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("MembershipCustomFieldID")]
		[Bindable(true)]
		public Guid MembershipCustomFieldID 
		{
			get { return GetColumnValue<Guid>(Columns.MembershipCustomFieldID); }
			set { SetColumnValue(Columns.MembershipCustomFieldID, value); }
		}
		  
		[XmlAttribute("FieldName")]
		[Bindable(true)]
		public string FieldName 
		{
			get { return GetColumnValue<string>(Columns.FieldName); }
			set { SetColumnValue(Columns.FieldName, value); }
		}
		  
		[XmlAttribute("Label")]
		[Bindable(true)]
		public string Label 
		{
			get { return GetColumnValue<string>(Columns.Label); }
			set { SetColumnValue(Columns.Label, value); }
		}
		  
		[XmlAttribute("Type")]
		[Bindable(true)]
		public string Type 
		{
			get { return GetColumnValue<string>(Columns.Type); }
			set { SetColumnValue(Columns.Type, value); }
		}
		  
		[XmlAttribute("EnumList")]
		[Bindable(true)]
		public string EnumList 
		{
			get { return GetColumnValue<string>(Columns.EnumList); }
			set { SetColumnValue(Columns.EnumList, value); }
		}
		  
		[XmlAttribute("Deleted")]
		[Bindable(true)]
		public bool? Deleted 
		{
			get { return GetColumnValue<bool?>(Columns.Deleted); }
			set { SetColumnValue(Columns.Deleted, value); }
		}
		  
		[XmlAttribute("CreatedOn")]
		[Bindable(true)]
		public DateTime? CreatedOn 
		{
			get { return GetColumnValue<DateTime?>(Columns.CreatedOn); }
			set { SetColumnValue(Columns.CreatedOn, value); }
		}
		  
		[XmlAttribute("CreatedBy")]
		[Bindable(true)]
		public string CreatedBy 
		{
			get { return GetColumnValue<string>(Columns.CreatedBy); }
			set { SetColumnValue(Columns.CreatedBy, value); }
		}
		  
		[XmlAttribute("ModifiedOn")]
		[Bindable(true)]
		public DateTime? ModifiedOn 
		{
			get { return GetColumnValue<DateTime?>(Columns.ModifiedOn); }
			set { SetColumnValue(Columns.ModifiedOn, value); }
		}
		  
		[XmlAttribute("ModifiedBy")]
		[Bindable(true)]
		public string ModifiedBy 
		{
			get { return GetColumnValue<string>(Columns.ModifiedBy); }
			set { SetColumnValue(Columns.ModifiedBy, value); }
		}
		
		#endregion
		
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(Guid varMembershipCustomFieldID,string varFieldName,string varLabel,string varType,string varEnumList,bool? varDeleted,DateTime? varCreatedOn,string varCreatedBy,DateTime? varModifiedOn,string varModifiedBy)
		{
			MembershipCustomField item = new MembershipCustomField();
			
			item.MembershipCustomFieldID = varMembershipCustomFieldID;
			
			item.FieldName = varFieldName;
			
			item.Label = varLabel;
			
			item.Type = varType;
			
			item.EnumList = varEnumList;
			
			item.Deleted = varDeleted;
			
			item.CreatedOn = varCreatedOn;
			
			item.CreatedBy = varCreatedBy;
			
			item.ModifiedOn = varModifiedOn;
			
			item.ModifiedBy = varModifiedBy;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(Guid varMembershipCustomFieldID,string varFieldName,string varLabel,string varType,string varEnumList,bool? varDeleted,DateTime? varCreatedOn,string varCreatedBy,DateTime? varModifiedOn,string varModifiedBy)
		{
			MembershipCustomField item = new MembershipCustomField();
			
				item.MembershipCustomFieldID = varMembershipCustomFieldID;
			
				item.FieldName = varFieldName;
			
				item.Label = varLabel;
			
				item.Type = varType;
			
				item.EnumList = varEnumList;
			
				item.Deleted = varDeleted;
			
				item.CreatedOn = varCreatedOn;
			
				item.CreatedBy = varCreatedBy;
			
				item.ModifiedOn = varModifiedOn;
			
				item.ModifiedBy = varModifiedBy;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn MembershipCustomFieldIDColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn FieldNameColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn LabelColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn TypeColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn EnumListColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn DeletedColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedOnColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedByColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedOnColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedByColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string MembershipCustomFieldID = @"MembershipCustomFieldID";
			 public static string FieldName = @"FieldName";
			 public static string Label = @"Label";
			 public static string Type = @"Type";
			 public static string EnumList = @"EnumList";
			 public static string Deleted = @"Deleted";
			 public static string CreatedOn = @"CreatedOn";
			 public static string CreatedBy = @"CreatedBy";
			 public static string ModifiedOn = @"ModifiedOn";
			 public static string ModifiedBy = @"ModifiedBy";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
