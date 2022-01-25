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
	/// Strongly-typed collection for the Recurrence class.
	/// </summary>
    [Serializable]
	public partial class RecurrenceCollection : ActiveList<Recurrence, RecurrenceCollection>
	{	   
		public RecurrenceCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>RecurrenceCollection</returns>
		public RecurrenceCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                Recurrence o = this[i];
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
	/// This is an ActiveRecord class which wraps the Recurrence table.
	/// </summary>
	[Serializable]
	public partial class Recurrence : ActiveRecord<Recurrence>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public Recurrence()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public Recurrence(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public Recurrence(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public Recurrence(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("Recurrence", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarRecID = new TableSchema.TableColumn(schema);
				colvarRecID.ColumnName = "RecID";
				colvarRecID.DataType = DbType.Int32;
				colvarRecID.MaxLength = 0;
				colvarRecID.AutoIncrement = true;
				colvarRecID.IsNullable = false;
				colvarRecID.IsPrimaryKey = true;
				colvarRecID.IsForeignKey = false;
				colvarRecID.IsReadOnly = false;
				colvarRecID.DefaultSetting = @"";
				colvarRecID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarRecID);
				
				TableSchema.TableColumn colvarItemID = new TableSchema.TableColumn(schema);
				colvarItemID.ColumnName = "ItemID";
				colvarItemID.DataType = DbType.Int32;
				colvarItemID.MaxLength = 0;
				colvarItemID.AutoIncrement = false;
				colvarItemID.IsNullable = true;
				colvarItemID.IsPrimaryKey = false;
				colvarItemID.IsForeignKey = true;
				colvarItemID.IsReadOnly = false;
				colvarItemID.DefaultSetting = @"";
				
					colvarItemID.ForeignKeyTableName = "Course";
				schema.Columns.Add(colvarItemID);
				
				TableSchema.TableColumn colvarPattern = new TableSchema.TableColumn(schema);
				colvarPattern.ColumnName = "Pattern";
				colvarPattern.DataType = DbType.Int32;
				colvarPattern.MaxLength = 0;
				colvarPattern.AutoIncrement = false;
				colvarPattern.IsNullable = true;
				colvarPattern.IsPrimaryKey = false;
				colvarPattern.IsForeignKey = false;
				colvarPattern.IsReadOnly = false;
				colvarPattern.DefaultSetting = @"";
				colvarPattern.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPattern);
				
				TableSchema.TableColumn colvarSubPattern = new TableSchema.TableColumn(schema);
				colvarSubPattern.ColumnName = "SubPattern";
				colvarSubPattern.DataType = DbType.Int32;
				colvarSubPattern.MaxLength = 0;
				colvarSubPattern.AutoIncrement = false;
				colvarSubPattern.IsNullable = true;
				colvarSubPattern.IsPrimaryKey = false;
				colvarSubPattern.IsForeignKey = false;
				colvarSubPattern.IsReadOnly = false;
				colvarSubPattern.DefaultSetting = @"";
				colvarSubPattern.ForeignKeyTableName = "";
				schema.Columns.Add(colvarSubPattern);
				
				TableSchema.TableColumn colvarEndType = new TableSchema.TableColumn(schema);
				colvarEndType.ColumnName = "EndType";
				colvarEndType.DataType = DbType.Int32;
				colvarEndType.MaxLength = 0;
				colvarEndType.AutoIncrement = false;
				colvarEndType.IsNullable = true;
				colvarEndType.IsPrimaryKey = false;
				colvarEndType.IsForeignKey = false;
				colvarEndType.IsReadOnly = false;
				colvarEndType.DefaultSetting = @"";
				colvarEndType.ForeignKeyTableName = "";
				schema.Columns.Add(colvarEndType);
				
				TableSchema.TableColumn colvarStartDate = new TableSchema.TableColumn(schema);
				colvarStartDate.ColumnName = "StartDate";
				colvarStartDate.DataType = DbType.DateTime;
				colvarStartDate.MaxLength = 0;
				colvarStartDate.AutoIncrement = false;
				colvarStartDate.IsNullable = true;
				colvarStartDate.IsPrimaryKey = false;
				colvarStartDate.IsForeignKey = false;
				colvarStartDate.IsReadOnly = false;
				colvarStartDate.DefaultSetting = @"";
				colvarStartDate.ForeignKeyTableName = "";
				schema.Columns.Add(colvarStartDate);
				
				TableSchema.TableColumn colvarEndAfter = new TableSchema.TableColumn(schema);
				colvarEndAfter.ColumnName = "EndAfter";
				colvarEndAfter.DataType = DbType.Int32;
				colvarEndAfter.MaxLength = 0;
				colvarEndAfter.AutoIncrement = false;
				colvarEndAfter.IsNullable = true;
				colvarEndAfter.IsPrimaryKey = false;
				colvarEndAfter.IsForeignKey = false;
				colvarEndAfter.IsReadOnly = false;
				colvarEndAfter.DefaultSetting = @"";
				colvarEndAfter.ForeignKeyTableName = "";
				schema.Columns.Add(colvarEndAfter);
				
				TableSchema.TableColumn colvarFrequency = new TableSchema.TableColumn(schema);
				colvarFrequency.ColumnName = "Frequency";
				colvarFrequency.DataType = DbType.Int32;
				colvarFrequency.MaxLength = 0;
				colvarFrequency.AutoIncrement = false;
				colvarFrequency.IsNullable = true;
				colvarFrequency.IsPrimaryKey = false;
				colvarFrequency.IsForeignKey = false;
				colvarFrequency.IsReadOnly = false;
				colvarFrequency.DefaultSetting = @"";
				colvarFrequency.ForeignKeyTableName = "";
				schema.Columns.Add(colvarFrequency);
				
				TableSchema.TableColumn colvarEndDate = new TableSchema.TableColumn(schema);
				colvarEndDate.ColumnName = "EndDate";
				colvarEndDate.DataType = DbType.DateTime;
				colvarEndDate.MaxLength = 0;
				colvarEndDate.AutoIncrement = false;
				colvarEndDate.IsNullable = true;
				colvarEndDate.IsPrimaryKey = false;
				colvarEndDate.IsForeignKey = false;
				colvarEndDate.IsReadOnly = false;
				colvarEndDate.DefaultSetting = @"";
				colvarEndDate.ForeignKeyTableName = "";
				schema.Columns.Add(colvarEndDate);
				
				TableSchema.TableColumn colvarWeekDays = new TableSchema.TableColumn(schema);
				colvarWeekDays.ColumnName = "WeekDays";
				colvarWeekDays.DataType = DbType.Int32;
				colvarWeekDays.MaxLength = 0;
				colvarWeekDays.AutoIncrement = false;
				colvarWeekDays.IsNullable = true;
				colvarWeekDays.IsPrimaryKey = false;
				colvarWeekDays.IsForeignKey = false;
				colvarWeekDays.IsReadOnly = false;
				colvarWeekDays.DefaultSetting = @"";
				colvarWeekDays.ForeignKeyTableName = "";
				schema.Columns.Add(colvarWeekDays);
				
				TableSchema.TableColumn colvarDayofMonth = new TableSchema.TableColumn(schema);
				colvarDayofMonth.ColumnName = "DayofMonth";
				colvarDayofMonth.DataType = DbType.Int32;
				colvarDayofMonth.MaxLength = 0;
				colvarDayofMonth.AutoIncrement = false;
				colvarDayofMonth.IsNullable = true;
				colvarDayofMonth.IsPrimaryKey = false;
				colvarDayofMonth.IsForeignKey = false;
				colvarDayofMonth.IsReadOnly = false;
				colvarDayofMonth.DefaultSetting = @"";
				colvarDayofMonth.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDayofMonth);
				
				TableSchema.TableColumn colvarWeekNum = new TableSchema.TableColumn(schema);
				colvarWeekNum.ColumnName = "WeekNum";
				colvarWeekNum.DataType = DbType.Int32;
				colvarWeekNum.MaxLength = 0;
				colvarWeekNum.AutoIncrement = false;
				colvarWeekNum.IsNullable = true;
				colvarWeekNum.IsPrimaryKey = false;
				colvarWeekNum.IsForeignKey = false;
				colvarWeekNum.IsReadOnly = false;
				colvarWeekNum.DefaultSetting = @"";
				colvarWeekNum.ForeignKeyTableName = "";
				schema.Columns.Add(colvarWeekNum);
				
				TableSchema.TableColumn colvarComment = new TableSchema.TableColumn(schema);
				colvarComment.ColumnName = "Comment";
				colvarComment.DataType = DbType.AnsiString;
				colvarComment.MaxLength = 150;
				colvarComment.AutoIncrement = false;
				colvarComment.IsNullable = true;
				colvarComment.IsPrimaryKey = false;
				colvarComment.IsForeignKey = false;
				colvarComment.IsReadOnly = false;
				colvarComment.DefaultSetting = @"";
				colvarComment.ForeignKeyTableName = "";
				schema.Columns.Add(colvarComment);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["PowerPOS"].AddSchema("Recurrence",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("RecID")]
		[Bindable(true)]
		public int RecID 
		{
			get { return GetColumnValue<int>(Columns.RecID); }
			set { SetColumnValue(Columns.RecID, value); }
		}
		  
		[XmlAttribute("ItemID")]
		[Bindable(true)]
		public int? ItemID 
		{
			get { return GetColumnValue<int?>(Columns.ItemID); }
			set { SetColumnValue(Columns.ItemID, value); }
		}
		  
		[XmlAttribute("Pattern")]
		[Bindable(true)]
		public int? Pattern 
		{
			get { return GetColumnValue<int?>(Columns.Pattern); }
			set { SetColumnValue(Columns.Pattern, value); }
		}
		  
		[XmlAttribute("SubPattern")]
		[Bindable(true)]
		public int? SubPattern 
		{
			get { return GetColumnValue<int?>(Columns.SubPattern); }
			set { SetColumnValue(Columns.SubPattern, value); }
		}
		  
		[XmlAttribute("EndType")]
		[Bindable(true)]
		public int? EndType 
		{
			get { return GetColumnValue<int?>(Columns.EndType); }
			set { SetColumnValue(Columns.EndType, value); }
		}
		  
		[XmlAttribute("StartDate")]
		[Bindable(true)]
		public DateTime? StartDate 
		{
			get { return GetColumnValue<DateTime?>(Columns.StartDate); }
			set { SetColumnValue(Columns.StartDate, value); }
		}
		  
		[XmlAttribute("EndAfter")]
		[Bindable(true)]
		public int? EndAfter 
		{
			get { return GetColumnValue<int?>(Columns.EndAfter); }
			set { SetColumnValue(Columns.EndAfter, value); }
		}
		  
		[XmlAttribute("Frequency")]
		[Bindable(true)]
		public int? Frequency 
		{
			get { return GetColumnValue<int?>(Columns.Frequency); }
			set { SetColumnValue(Columns.Frequency, value); }
		}
		  
		[XmlAttribute("EndDate")]
		[Bindable(true)]
		public DateTime? EndDate 
		{
			get { return GetColumnValue<DateTime?>(Columns.EndDate); }
			set { SetColumnValue(Columns.EndDate, value); }
		}
		  
		[XmlAttribute("WeekDays")]
		[Bindable(true)]
		public int? WeekDays 
		{
			get { return GetColumnValue<int?>(Columns.WeekDays); }
			set { SetColumnValue(Columns.WeekDays, value); }
		}
		  
		[XmlAttribute("DayofMonth")]
		[Bindable(true)]
		public int? DayofMonth 
		{
			get { return GetColumnValue<int?>(Columns.DayofMonth); }
			set { SetColumnValue(Columns.DayofMonth, value); }
		}
		  
		[XmlAttribute("WeekNum")]
		[Bindable(true)]
		public int? WeekNum 
		{
			get { return GetColumnValue<int?>(Columns.WeekNum); }
			set { SetColumnValue(Columns.WeekNum, value); }
		}
		  
		[XmlAttribute("Comment")]
		[Bindable(true)]
		public string Comment 
		{
			get { return GetColumnValue<string>(Columns.Comment); }
			set { SetColumnValue(Columns.Comment, value); }
		}
		
		#endregion
		
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a Course ActiveRecord object related to this Recurrence
		/// 
		/// </summary>
		public PowerPOS.Course Course
		{
			get { return PowerPOS.Course.FetchByID(this.ItemID); }
			set { SetColumnValue("ItemID", value.Id); }
		}
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(int? varItemID,int? varPattern,int? varSubPattern,int? varEndType,DateTime? varStartDate,int? varEndAfter,int? varFrequency,DateTime? varEndDate,int? varWeekDays,int? varDayofMonth,int? varWeekNum,string varComment)
		{
			Recurrence item = new Recurrence();
			
			item.ItemID = varItemID;
			
			item.Pattern = varPattern;
			
			item.SubPattern = varSubPattern;
			
			item.EndType = varEndType;
			
			item.StartDate = varStartDate;
			
			item.EndAfter = varEndAfter;
			
			item.Frequency = varFrequency;
			
			item.EndDate = varEndDate;
			
			item.WeekDays = varWeekDays;
			
			item.DayofMonth = varDayofMonth;
			
			item.WeekNum = varWeekNum;
			
			item.Comment = varComment;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varRecID,int? varItemID,int? varPattern,int? varSubPattern,int? varEndType,DateTime? varStartDate,int? varEndAfter,int? varFrequency,DateTime? varEndDate,int? varWeekDays,int? varDayofMonth,int? varWeekNum,string varComment)
		{
			Recurrence item = new Recurrence();
			
				item.RecID = varRecID;
			
				item.ItemID = varItemID;
			
				item.Pattern = varPattern;
			
				item.SubPattern = varSubPattern;
			
				item.EndType = varEndType;
			
				item.StartDate = varStartDate;
			
				item.EndAfter = varEndAfter;
			
				item.Frequency = varFrequency;
			
				item.EndDate = varEndDate;
			
				item.WeekDays = varWeekDays;
			
				item.DayofMonth = varDayofMonth;
			
				item.WeekNum = varWeekNum;
			
				item.Comment = varComment;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn RecIDColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn ItemIDColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn PatternColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn SubPatternColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn EndTypeColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn StartDateColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn EndAfterColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn FrequencyColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn EndDateColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn WeekDaysColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn DayofMonthColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        public static TableSchema.TableColumn WeekNumColumn
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        public static TableSchema.TableColumn CommentColumn
        {
            get { return Schema.Columns[12]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string RecID = @"RecID";
			 public static string ItemID = @"ItemID";
			 public static string Pattern = @"Pattern";
			 public static string SubPattern = @"SubPattern";
			 public static string EndType = @"EndType";
			 public static string StartDate = @"StartDate";
			 public static string EndAfter = @"EndAfter";
			 public static string Frequency = @"Frequency";
			 public static string EndDate = @"EndDate";
			 public static string WeekDays = @"WeekDays";
			 public static string DayofMonth = @"DayofMonth";
			 public static string WeekNum = @"WeekNum";
			 public static string Comment = @"Comment";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
