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
	/// Strongly-typed collection for the ItemsNew class.
	/// </summary>
    [Serializable]
	public partial class ItemsNewCollection : ActiveList<ItemsNew, ItemsNewCollection>
	{	   
		public ItemsNewCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>ItemsNewCollection</returns>
		public ItemsNewCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                ItemsNew o = this[i];
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
	/// This is an ActiveRecord class which wraps the ItemsNew table.
	/// </summary>
	[Serializable]
	public partial class ItemsNew : ActiveRecord<ItemsNew>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public ItemsNew()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public ItemsNew(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public ItemsNew(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public ItemsNew(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("ItemsNew", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarId = new TableSchema.TableColumn(schema);
				colvarId.ColumnName = "ID";
				colvarId.DataType = DbType.Int32;
				colvarId.MaxLength = 0;
				colvarId.AutoIncrement = false;
				colvarId.IsNullable = false;
				colvarId.IsPrimaryKey = true;
				colvarId.IsForeignKey = false;
				colvarId.IsReadOnly = false;
				colvarId.DefaultSetting = @"";
				colvarId.ForeignKeyTableName = "";
				schema.Columns.Add(colvarId);
				
				TableSchema.TableColumn colvarTitle = new TableSchema.TableColumn(schema);
				colvarTitle.ColumnName = "Title";
				colvarTitle.DataType = DbType.AnsiString;
				colvarTitle.MaxLength = 100;
				colvarTitle.AutoIncrement = false;
				colvarTitle.IsNullable = true;
				colvarTitle.IsPrimaryKey = false;
				colvarTitle.IsForeignKey = false;
				colvarTitle.IsReadOnly = false;
				colvarTitle.DefaultSetting = @"";
				colvarTitle.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTitle);
				
				TableSchema.TableColumn colvarDescription = new TableSchema.TableColumn(schema);
				colvarDescription.ColumnName = "Description";
				colvarDescription.DataType = DbType.AnsiString;
				colvarDescription.MaxLength = 200;
				colvarDescription.AutoIncrement = false;
				colvarDescription.IsNullable = true;
				colvarDescription.IsPrimaryKey = false;
				colvarDescription.IsForeignKey = false;
				colvarDescription.IsReadOnly = false;
				colvarDescription.DefaultSetting = @"";
				colvarDescription.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDescription);
				
				TableSchema.TableColumn colvarMembers = new TableSchema.TableColumn(schema);
				colvarMembers.ColumnName = "members";
				colvarMembers.DataType = DbType.AnsiString;
				colvarMembers.MaxLength = 250;
				colvarMembers.AutoIncrement = false;
				colvarMembers.IsNullable = true;
				colvarMembers.IsPrimaryKey = false;
				colvarMembers.IsForeignKey = false;
				colvarMembers.IsReadOnly = false;
				colvarMembers.DefaultSetting = @"";
				colvarMembers.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMembers);
				
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
				
				TableSchema.TableColumn colvarIsAllDay = new TableSchema.TableColumn(schema);
				colvarIsAllDay.ColumnName = "IsAllDay";
				colvarIsAllDay.DataType = DbType.Boolean;
				colvarIsAllDay.MaxLength = 0;
				colvarIsAllDay.AutoIncrement = false;
				colvarIsAllDay.IsNullable = true;
				colvarIsAllDay.IsPrimaryKey = false;
				colvarIsAllDay.IsForeignKey = false;
				colvarIsAllDay.IsReadOnly = false;
				colvarIsAllDay.DefaultSetting = @"";
				colvarIsAllDay.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIsAllDay);
				
				TableSchema.TableColumn colvarPlace = new TableSchema.TableColumn(schema);
				colvarPlace.ColumnName = "Place";
				colvarPlace.DataType = DbType.Int32;
				colvarPlace.MaxLength = 0;
				colvarPlace.AutoIncrement = false;
				colvarPlace.IsNullable = true;
				colvarPlace.IsPrimaryKey = false;
				colvarPlace.IsForeignKey = false;
				colvarPlace.IsReadOnly = false;
				colvarPlace.DefaultSetting = @"";
				colvarPlace.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPlace);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["PowerPOS"].AddSchema("ItemsNew",schema);
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
		  
		[XmlAttribute("Description")]
		[Bindable(true)]
		public string Description 
		{
			get { return GetColumnValue<string>(Columns.Description); }
			set { SetColumnValue(Columns.Description, value); }
		}
		  
		[XmlAttribute("Members")]
		[Bindable(true)]
		public string Members 
		{
			get { return GetColumnValue<string>(Columns.Members); }
			set { SetColumnValue(Columns.Members, value); }
		}
		  
		[XmlAttribute("StartDate")]
		[Bindable(true)]
		public DateTime? StartDate 
		{
			get { return GetColumnValue<DateTime?>(Columns.StartDate); }
			set { SetColumnValue(Columns.StartDate, value); }
		}
		  
		[XmlAttribute("EndDate")]
		[Bindable(true)]
		public DateTime? EndDate 
		{
			get { return GetColumnValue<DateTime?>(Columns.EndDate); }
			set { SetColumnValue(Columns.EndDate, value); }
		}
		  
		[XmlAttribute("IsAllDay")]
		[Bindable(true)]
		public bool? IsAllDay 
		{
			get { return GetColumnValue<bool?>(Columns.IsAllDay); }
			set { SetColumnValue(Columns.IsAllDay, value); }
		}
		  
		[XmlAttribute("Place")]
		[Bindable(true)]
		public int? Place 
		{
			get { return GetColumnValue<int?>(Columns.Place); }
			set { SetColumnValue(Columns.Place, value); }
		}
		
		#endregion
		
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(int varId,string varTitle,string varDescription,string varMembers,DateTime? varStartDate,DateTime? varEndDate,bool? varIsAllDay,int? varPlace)
		{
			ItemsNew item = new ItemsNew();
			
			item.Id = varId;
			
			item.Title = varTitle;
			
			item.Description = varDescription;
			
			item.Members = varMembers;
			
			item.StartDate = varStartDate;
			
			item.EndDate = varEndDate;
			
			item.IsAllDay = varIsAllDay;
			
			item.Place = varPlace;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,string varTitle,string varDescription,string varMembers,DateTime? varStartDate,DateTime? varEndDate,bool? varIsAllDay,int? varPlace)
		{
			ItemsNew item = new ItemsNew();
			
				item.Id = varId;
			
				item.Title = varTitle;
			
				item.Description = varDescription;
			
				item.Members = varMembers;
			
				item.StartDate = varStartDate;
			
				item.EndDate = varEndDate;
			
				item.IsAllDay = varIsAllDay;
			
				item.Place = varPlace;
			
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
        
        
        
        public static TableSchema.TableColumn DescriptionColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn MembersColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn StartDateColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn EndDateColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn IsAllDayColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn PlaceColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"ID";
			 public static string Title = @"Title";
			 public static string Description = @"Description";
			 public static string Members = @"members";
			 public static string StartDate = @"StartDate";
			 public static string EndDate = @"EndDate";
			 public static string IsAllDay = @"IsAllDay";
			 public static string Place = @"Place";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
