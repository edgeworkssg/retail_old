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
	/// Strongly-typed collection for the Dashboard class.
	/// </summary>
    [Serializable]
	public partial class DashboardCollection : ActiveList<Dashboard, DashboardCollection>
	{	   
		public DashboardCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>DashboardCollection</returns>
		public DashboardCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                Dashboard o = this[i];
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
	/// This is an ActiveRecord class which wraps the Dashboard table.
	/// </summary>
	[Serializable]
	public partial class Dashboard : ActiveRecord<Dashboard>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public Dashboard()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public Dashboard(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public Dashboard(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public Dashboard(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("Dashboard", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarId = new TableSchema.TableColumn(schema);
				colvarId.ColumnName = "ID";
				colvarId.DataType = DbType.Int32;
				colvarId.MaxLength = 0;
				colvarId.AutoIncrement = true;
				colvarId.IsNullable = false;
				colvarId.IsPrimaryKey = true;
				colvarId.IsForeignKey = false;
				colvarId.IsReadOnly = false;
				colvarId.DefaultSetting = @"";
				colvarId.ForeignKeyTableName = "";
				schema.Columns.Add(colvarId);
				
				TableSchema.TableColumn colvarTitle = new TableSchema.TableColumn(schema);
				colvarTitle.ColumnName = "Title";
				colvarTitle.DataType = DbType.String;
				colvarTitle.MaxLength = 500;
				colvarTitle.AutoIncrement = false;
				colvarTitle.IsNullable = true;
				colvarTitle.IsPrimaryKey = false;
				colvarTitle.IsForeignKey = false;
				colvarTitle.IsReadOnly = false;
				colvarTitle.DefaultSetting = @"";
				colvarTitle.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTitle);
				
				TableSchema.TableColumn colvarSubTitle = new TableSchema.TableColumn(schema);
				colvarSubTitle.ColumnName = "SubTitle";
				colvarSubTitle.DataType = DbType.String;
				colvarSubTitle.MaxLength = 500;
				colvarSubTitle.AutoIncrement = false;
				colvarSubTitle.IsNullable = true;
				colvarSubTitle.IsPrimaryKey = false;
				colvarSubTitle.IsForeignKey = false;
				colvarSubTitle.IsReadOnly = false;
				colvarSubTitle.DefaultSetting = @"";
				colvarSubTitle.ForeignKeyTableName = "";
				schema.Columns.Add(colvarSubTitle);
				
				TableSchema.TableColumn colvarDescription = new TableSchema.TableColumn(schema);
				colvarDescription.ColumnName = "Description";
				colvarDescription.DataType = DbType.String;
				colvarDescription.MaxLength = -1;
				colvarDescription.AutoIncrement = false;
				colvarDescription.IsNullable = true;
				colvarDescription.IsPrimaryKey = false;
				colvarDescription.IsForeignKey = false;
				colvarDescription.IsReadOnly = false;
				colvarDescription.DefaultSetting = @"";
				colvarDescription.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDescription);
				
				TableSchema.TableColumn colvarPlotType = new TableSchema.TableColumn(schema);
				colvarPlotType.ColumnName = "PlotType";
				colvarPlotType.DataType = DbType.AnsiString;
				colvarPlotType.MaxLength = 200;
				colvarPlotType.AutoIncrement = false;
				colvarPlotType.IsNullable = true;
				colvarPlotType.IsPrimaryKey = false;
				colvarPlotType.IsForeignKey = false;
				colvarPlotType.IsReadOnly = false;
				colvarPlotType.DefaultSetting = @"";
				colvarPlotType.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPlotType);
				
				TableSchema.TableColumn colvarPlotOption = new TableSchema.TableColumn(schema);
				colvarPlotOption.ColumnName = "PlotOption";
				colvarPlotOption.DataType = DbType.String;
				colvarPlotOption.MaxLength = -1;
				colvarPlotOption.AutoIncrement = false;
				colvarPlotOption.IsNullable = true;
				colvarPlotOption.IsPrimaryKey = false;
				colvarPlotOption.IsForeignKey = false;
				colvarPlotOption.IsReadOnly = false;
				colvarPlotOption.DefaultSetting = @"";
				colvarPlotOption.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPlotOption);
				
				TableSchema.TableColumn colvarWidth = new TableSchema.TableColumn(schema);
				colvarWidth.ColumnName = "Width";
				colvarWidth.DataType = DbType.AnsiString;
				colvarWidth.MaxLength = 500;
				colvarWidth.AutoIncrement = false;
				colvarWidth.IsNullable = true;
				colvarWidth.IsPrimaryKey = false;
				colvarWidth.IsForeignKey = false;
				colvarWidth.IsReadOnly = false;
				colvarWidth.DefaultSetting = @"";
				colvarWidth.ForeignKeyTableName = "";
				schema.Columns.Add(colvarWidth);
				
				TableSchema.TableColumn colvarHeight = new TableSchema.TableColumn(schema);
				colvarHeight.ColumnName = "Height";
				colvarHeight.DataType = DbType.AnsiString;
				colvarHeight.MaxLength = 500;
				colvarHeight.AutoIncrement = false;
				colvarHeight.IsNullable = true;
				colvarHeight.IsPrimaryKey = false;
				colvarHeight.IsForeignKey = false;
				colvarHeight.IsReadOnly = false;
				colvarHeight.DefaultSetting = @"";
				colvarHeight.ForeignKeyTableName = "";
				schema.Columns.Add(colvarHeight);
				
				TableSchema.TableColumn colvarSQLString = new TableSchema.TableColumn(schema);
				colvarSQLString.ColumnName = "SQLString";
				colvarSQLString.DataType = DbType.String;
				colvarSQLString.MaxLength = -1;
				colvarSQLString.AutoIncrement = false;
				colvarSQLString.IsNullable = true;
				colvarSQLString.IsPrimaryKey = false;
				colvarSQLString.IsForeignKey = false;
				colvarSQLString.IsReadOnly = false;
				colvarSQLString.DefaultSetting = @"";
				colvarSQLString.ForeignKeyTableName = "";
				schema.Columns.Add(colvarSQLString);
				
				TableSchema.TableColumn colvarIsInline = new TableSchema.TableColumn(schema);
				colvarIsInline.ColumnName = "IsInline";
				colvarIsInline.DataType = DbType.Boolean;
				colvarIsInline.MaxLength = 0;
				colvarIsInline.AutoIncrement = false;
				colvarIsInline.IsNullable = true;
				colvarIsInline.IsPrimaryKey = false;
				colvarIsInline.IsForeignKey = false;
				colvarIsInline.IsReadOnly = false;
				colvarIsInline.DefaultSetting = @"";
				colvarIsInline.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIsInline);
				
				TableSchema.TableColumn colvarBreakAfter = new TableSchema.TableColumn(schema);
				colvarBreakAfter.ColumnName = "BreakAfter";
				colvarBreakAfter.DataType = DbType.Boolean;
				colvarBreakAfter.MaxLength = 0;
				colvarBreakAfter.AutoIncrement = false;
				colvarBreakAfter.IsNullable = true;
				colvarBreakAfter.IsPrimaryKey = false;
				colvarBreakAfter.IsForeignKey = false;
				colvarBreakAfter.IsReadOnly = false;
				colvarBreakAfter.DefaultSetting = @"";
				colvarBreakAfter.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBreakAfter);
				
				TableSchema.TableColumn colvarBreakBefore = new TableSchema.TableColumn(schema);
				colvarBreakBefore.ColumnName = "BreakBefore";
				colvarBreakBefore.DataType = DbType.Boolean;
				colvarBreakBefore.MaxLength = 0;
				colvarBreakBefore.AutoIncrement = false;
				colvarBreakBefore.IsNullable = true;
				colvarBreakBefore.IsPrimaryKey = false;
				colvarBreakBefore.IsForeignKey = false;
				colvarBreakBefore.IsReadOnly = false;
				colvarBreakBefore.DefaultSetting = @"";
				colvarBreakBefore.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBreakBefore);
				
				TableSchema.TableColumn colvarColumnStyle = new TableSchema.TableColumn(schema);
				colvarColumnStyle.ColumnName = "ColumnStyle";
				colvarColumnStyle.DataType = DbType.String;
				colvarColumnStyle.MaxLength = -1;
				colvarColumnStyle.AutoIncrement = false;
				colvarColumnStyle.IsNullable = true;
				colvarColumnStyle.IsPrimaryKey = false;
				colvarColumnStyle.IsForeignKey = false;
				colvarColumnStyle.IsReadOnly = false;
				colvarColumnStyle.DefaultSetting = @"";
				colvarColumnStyle.ForeignKeyTableName = "";
				schema.Columns.Add(colvarColumnStyle);
				
				TableSchema.TableColumn colvarIsEnable = new TableSchema.TableColumn(schema);
				colvarIsEnable.ColumnName = "IsEnable";
				colvarIsEnable.DataType = DbType.Boolean;
				colvarIsEnable.MaxLength = 0;
				colvarIsEnable.AutoIncrement = false;
				colvarIsEnable.IsNullable = true;
				colvarIsEnable.IsPrimaryKey = false;
				colvarIsEnable.IsForeignKey = false;
				colvarIsEnable.IsReadOnly = false;
				colvarIsEnable.DefaultSetting = @"";
				colvarIsEnable.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIsEnable);
				
				TableSchema.TableColumn colvarDisplayOrder = new TableSchema.TableColumn(schema);
				colvarDisplayOrder.ColumnName = "DisplayOrder";
				colvarDisplayOrder.DataType = DbType.Int32;
				colvarDisplayOrder.MaxLength = 0;
				colvarDisplayOrder.AutoIncrement = false;
				colvarDisplayOrder.IsNullable = true;
				colvarDisplayOrder.IsPrimaryKey = false;
				colvarDisplayOrder.IsForeignKey = false;
				colvarDisplayOrder.IsReadOnly = false;
				colvarDisplayOrder.DefaultSetting = @"";
				colvarDisplayOrder.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDisplayOrder);
				
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
				
				TableSchema.TableColumn colvarUniqueID = new TableSchema.TableColumn(schema);
				colvarUniqueID.ColumnName = "UniqueID";
				colvarUniqueID.DataType = DbType.Guid;
				colvarUniqueID.MaxLength = 0;
				colvarUniqueID.AutoIncrement = false;
				colvarUniqueID.IsNullable = true;
				colvarUniqueID.IsPrimaryKey = false;
				colvarUniqueID.IsForeignKey = false;
				colvarUniqueID.IsReadOnly = false;
				colvarUniqueID.DefaultSetting = @"";
				colvarUniqueID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUniqueID);
				
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
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["PowerPOS"].AddSchema("Dashboard",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("Id")]
		[Bindable(true)]
		public int Id 
		{
			get { return GetColumnValue<int>(Columns.Id); }
			set { SetColumnValue(Columns.Id, value); }
		}
		  
		[XmlAttribute("Title")]
		[Bindable(true)]
		public string Title 
		{
			get { return GetColumnValue<string>(Columns.Title); }
			set { SetColumnValue(Columns.Title, value); }
		}
		  
		[XmlAttribute("SubTitle")]
		[Bindable(true)]
		public string SubTitle 
		{
			get { return GetColumnValue<string>(Columns.SubTitle); }
			set { SetColumnValue(Columns.SubTitle, value); }
		}
		  
		[XmlAttribute("Description")]
		[Bindable(true)]
		public string Description 
		{
			get { return GetColumnValue<string>(Columns.Description); }
			set { SetColumnValue(Columns.Description, value); }
		}
		  
		[XmlAttribute("PlotType")]
		[Bindable(true)]
		public string PlotType 
		{
			get { return GetColumnValue<string>(Columns.PlotType); }
			set { SetColumnValue(Columns.PlotType, value); }
		}
		  
		[XmlAttribute("PlotOption")]
		[Bindable(true)]
		public string PlotOption 
		{
			get { return GetColumnValue<string>(Columns.PlotOption); }
			set { SetColumnValue(Columns.PlotOption, value); }
		}
		  
		[XmlAttribute("Width")]
		[Bindable(true)]
		public string Width 
		{
			get { return GetColumnValue<string>(Columns.Width); }
			set { SetColumnValue(Columns.Width, value); }
		}
		  
		[XmlAttribute("Height")]
		[Bindable(true)]
		public string Height 
		{
			get { return GetColumnValue<string>(Columns.Height); }
			set { SetColumnValue(Columns.Height, value); }
		}
		  
		[XmlAttribute("SQLString")]
		[Bindable(true)]
		public string SQLString 
		{
			get { return GetColumnValue<string>(Columns.SQLString); }
			set { SetColumnValue(Columns.SQLString, value); }
		}
		  
		[XmlAttribute("IsInline")]
		[Bindable(true)]
		public bool? IsInline 
		{
			get { return GetColumnValue<bool?>(Columns.IsInline); }
			set { SetColumnValue(Columns.IsInline, value); }
		}
		  
		[XmlAttribute("BreakAfter")]
		[Bindable(true)]
		public bool? BreakAfter 
		{
			get { return GetColumnValue<bool?>(Columns.BreakAfter); }
			set { SetColumnValue(Columns.BreakAfter, value); }
		}
		  
		[XmlAttribute("BreakBefore")]
		[Bindable(true)]
		public bool? BreakBefore 
		{
			get { return GetColumnValue<bool?>(Columns.BreakBefore); }
			set { SetColumnValue(Columns.BreakBefore, value); }
		}
		  
		[XmlAttribute("ColumnStyle")]
		[Bindable(true)]
		public string ColumnStyle 
		{
			get { return GetColumnValue<string>(Columns.ColumnStyle); }
			set { SetColumnValue(Columns.ColumnStyle, value); }
		}
		  
		[XmlAttribute("IsEnable")]
		[Bindable(true)]
		public bool? IsEnable 
		{
			get { return GetColumnValue<bool?>(Columns.IsEnable); }
			set { SetColumnValue(Columns.IsEnable, value); }
		}
		  
		[XmlAttribute("DisplayOrder")]
		[Bindable(true)]
		public int? DisplayOrder 
		{
			get { return GetColumnValue<int?>(Columns.DisplayOrder); }
			set { SetColumnValue(Columns.DisplayOrder, value); }
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
		  
		[XmlAttribute("UniqueID")]
		[Bindable(true)]
		public Guid? UniqueID 
		{
			get { return GetColumnValue<Guid?>(Columns.UniqueID); }
			set { SetColumnValue(Columns.UniqueID, value); }
		}
		  
		[XmlAttribute("Deleted")]
		[Bindable(true)]
		public bool? Deleted 
		{
			get { return GetColumnValue<bool?>(Columns.Deleted); }
			set { SetColumnValue(Columns.Deleted, value); }
		}
		
		#endregion
		
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(string varTitle,string varSubTitle,string varDescription,string varPlotType,string varPlotOption,string varWidth,string varHeight,string varSQLString,bool? varIsInline,bool? varBreakAfter,bool? varBreakBefore,string varColumnStyle,bool? varIsEnable,int? varDisplayOrder,DateTime? varCreatedOn,string varCreatedBy,DateTime? varModifiedOn,string varModifiedBy,Guid? varUniqueID,bool? varDeleted)
		{
			Dashboard item = new Dashboard();
			
			item.Title = varTitle;
			
			item.SubTitle = varSubTitle;
			
			item.Description = varDescription;
			
			item.PlotType = varPlotType;
			
			item.PlotOption = varPlotOption;
			
			item.Width = varWidth;
			
			item.Height = varHeight;
			
			item.SQLString = varSQLString;
			
			item.IsInline = varIsInline;
			
			item.BreakAfter = varBreakAfter;
			
			item.BreakBefore = varBreakBefore;
			
			item.ColumnStyle = varColumnStyle;
			
			item.IsEnable = varIsEnable;
			
			item.DisplayOrder = varDisplayOrder;
			
			item.CreatedOn = varCreatedOn;
			
			item.CreatedBy = varCreatedBy;
			
			item.ModifiedOn = varModifiedOn;
			
			item.ModifiedBy = varModifiedBy;
			
			item.UniqueID = varUniqueID;
			
			item.Deleted = varDeleted;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,string varTitle,string varSubTitle,string varDescription,string varPlotType,string varPlotOption,string varWidth,string varHeight,string varSQLString,bool? varIsInline,bool? varBreakAfter,bool? varBreakBefore,string varColumnStyle,bool? varIsEnable,int? varDisplayOrder,DateTime? varCreatedOn,string varCreatedBy,DateTime? varModifiedOn,string varModifiedBy,Guid? varUniqueID,bool? varDeleted)
		{
			Dashboard item = new Dashboard();
			
				item.Id = varId;
			
				item.Title = varTitle;
			
				item.SubTitle = varSubTitle;
			
				item.Description = varDescription;
			
				item.PlotType = varPlotType;
			
				item.PlotOption = varPlotOption;
			
				item.Width = varWidth;
			
				item.Height = varHeight;
			
				item.SQLString = varSQLString;
			
				item.IsInline = varIsInline;
			
				item.BreakAfter = varBreakAfter;
			
				item.BreakBefore = varBreakBefore;
			
				item.ColumnStyle = varColumnStyle;
			
				item.IsEnable = varIsEnable;
			
				item.DisplayOrder = varDisplayOrder;
			
				item.CreatedOn = varCreatedOn;
			
				item.CreatedBy = varCreatedBy;
			
				item.ModifiedOn = varModifiedOn;
			
				item.ModifiedBy = varModifiedBy;
			
				item.UniqueID = varUniqueID;
			
				item.Deleted = varDeleted;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn IdColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn TitleColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn SubTitleColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn DescriptionColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn PlotTypeColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn PlotOptionColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn WidthColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn HeightColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn SQLStringColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn IsInlineColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn BreakAfterColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        public static TableSchema.TableColumn BreakBeforeColumn
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        public static TableSchema.TableColumn ColumnStyleColumn
        {
            get { return Schema.Columns[12]; }
        }
        
        
        
        public static TableSchema.TableColumn IsEnableColumn
        {
            get { return Schema.Columns[13]; }
        }
        
        
        
        public static TableSchema.TableColumn DisplayOrderColumn
        {
            get { return Schema.Columns[14]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedOnColumn
        {
            get { return Schema.Columns[15]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedByColumn
        {
            get { return Schema.Columns[16]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedOnColumn
        {
            get { return Schema.Columns[17]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedByColumn
        {
            get { return Schema.Columns[18]; }
        }
        
        
        
        public static TableSchema.TableColumn UniqueIDColumn
        {
            get { return Schema.Columns[19]; }
        }
        
        
        
        public static TableSchema.TableColumn DeletedColumn
        {
            get { return Schema.Columns[20]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"ID";
			 public static string Title = @"Title";
			 public static string SubTitle = @"SubTitle";
			 public static string Description = @"Description";
			 public static string PlotType = @"PlotType";
			 public static string PlotOption = @"PlotOption";
			 public static string Width = @"Width";
			 public static string Height = @"Height";
			 public static string SQLString = @"SQLString";
			 public static string IsInline = @"IsInline";
			 public static string BreakAfter = @"BreakAfter";
			 public static string BreakBefore = @"BreakBefore";
			 public static string ColumnStyle = @"ColumnStyle";
			 public static string IsEnable = @"IsEnable";
			 public static string DisplayOrder = @"DisplayOrder";
			 public static string CreatedOn = @"CreatedOn";
			 public static string CreatedBy = @"CreatedBy";
			 public static string ModifiedOn = @"ModifiedOn";
			 public static string ModifiedBy = @"ModifiedBy";
			 public static string UniqueID = @"UniqueID";
			 public static string Deleted = @"Deleted";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
