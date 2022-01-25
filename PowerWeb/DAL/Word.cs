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
	/// Strongly-typed collection for the Word class.
	/// </summary>
    [Serializable]
	public partial class WordCollection : ActiveList<Word, WordCollection>
	{	   
		public WordCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>WordCollection</returns>
		public WordCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                Word o = this[i];
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
	/// This is an ActiveRecord class which wraps the Word table.
	/// </summary>
	[Serializable]
	public partial class Word : ActiveRecord<Word>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public Word()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public Word(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public Word(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public Word(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("Word", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarWordGuid = new TableSchema.TableColumn(schema);
				colvarWordGuid.ColumnName = "WordGuid";
				colvarWordGuid.DataType = DbType.Guid;
				colvarWordGuid.MaxLength = 0;
				colvarWordGuid.AutoIncrement = false;
				colvarWordGuid.IsNullable = false;
				colvarWordGuid.IsPrimaryKey = true;
				colvarWordGuid.IsForeignKey = false;
				colvarWordGuid.IsReadOnly = false;
				colvarWordGuid.DefaultSetting = @"";
				colvarWordGuid.ForeignKeyTableName = "";
				schema.Columns.Add(colvarWordGuid);
				
				TableSchema.TableColumn colvarWordText = new TableSchema.TableColumn(schema);
				colvarWordText.ColumnName = "WordText";
				colvarWordText.DataType = DbType.String;
				colvarWordText.MaxLength = 30;
				colvarWordText.AutoIncrement = false;
				colvarWordText.IsNullable = false;
				colvarWordText.IsPrimaryKey = false;
				colvarWordText.IsForeignKey = false;
				colvarWordText.IsReadOnly = false;
				colvarWordText.DefaultSetting = @"";
				colvarWordText.ForeignKeyTableName = "";
				schema.Columns.Add(colvarWordText);
				
				TableSchema.TableColumn colvarWordLength = new TableSchema.TableColumn(schema);
				colvarWordLength.ColumnName = "WordLength";
				colvarWordLength.DataType = DbType.Byte;
				colvarWordLength.MaxLength = 0;
				colvarWordLength.AutoIncrement = false;
				colvarWordLength.IsNullable = false;
				colvarWordLength.IsPrimaryKey = false;
				colvarWordLength.IsForeignKey = false;
				colvarWordLength.IsReadOnly = false;
				colvarWordLength.DefaultSetting = @"";
				colvarWordLength.ForeignKeyTableName = "";
				schema.Columns.Add(colvarWordLength);
				
				TableSchema.TableColumn colvarSoundexGroup = new TableSchema.TableColumn(schema);
				colvarSoundexGroup.ColumnName = "SoundexGroup";
				colvarSoundexGroup.DataType = DbType.String;
				colvarSoundexGroup.MaxLength = 1;
				colvarSoundexGroup.AutoIncrement = false;
				colvarSoundexGroup.IsNullable = false;
				colvarSoundexGroup.IsPrimaryKey = false;
				colvarSoundexGroup.IsForeignKey = false;
				colvarSoundexGroup.IsReadOnly = false;
				colvarSoundexGroup.DefaultSetting = @"";
				colvarSoundexGroup.ForeignKeyTableName = "";
				schema.Columns.Add(colvarSoundexGroup);
				
				TableSchema.TableColumn colvarSoundexValue = new TableSchema.TableColumn(schema);
				colvarSoundexValue.ColumnName = "SoundexValue";
				colvarSoundexValue.DataType = DbType.Int16;
				colvarSoundexValue.MaxLength = 0;
				colvarSoundexValue.AutoIncrement = false;
				colvarSoundexValue.IsNullable = false;
				colvarSoundexValue.IsPrimaryKey = false;
				colvarSoundexValue.IsForeignKey = false;
				colvarSoundexValue.IsReadOnly = false;
				colvarSoundexValue.DefaultSetting = @"";
				colvarSoundexValue.ForeignKeyTableName = "";
				schema.Columns.Add(colvarSoundexValue);
				
				TableSchema.TableColumn colvarGroupId = new TableSchema.TableColumn(schema);
				colvarGroupId.ColumnName = "GroupId";
				colvarGroupId.DataType = DbType.Int16;
				colvarGroupId.MaxLength = 0;
				colvarGroupId.AutoIncrement = false;
				colvarGroupId.IsNullable = true;
				colvarGroupId.IsPrimaryKey = false;
				colvarGroupId.IsForeignKey = false;
				colvarGroupId.IsReadOnly = false;
				colvarGroupId.DefaultSetting = @"";
				colvarGroupId.ForeignKeyTableName = "";
				schema.Columns.Add(colvarGroupId);
				
				TableSchema.TableColumn colvarIsPalindrome = new TableSchema.TableColumn(schema);
				colvarIsPalindrome.ColumnName = "IsPalindrome";
				colvarIsPalindrome.DataType = DbType.Boolean;
				colvarIsPalindrome.MaxLength = 0;
				colvarIsPalindrome.AutoIncrement = false;
				colvarIsPalindrome.IsNullable = false;
				colvarIsPalindrome.IsPrimaryKey = false;
				colvarIsPalindrome.IsForeignKey = false;
				colvarIsPalindrome.IsReadOnly = false;
				
						colvarIsPalindrome.DefaultSetting = @"((0))";
				colvarIsPalindrome.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIsPalindrome);
				
				TableSchema.TableColumn colvarInUnabr = new TableSchema.TableColumn(schema);
				colvarInUnabr.ColumnName = "InUnabr";
				colvarInUnabr.DataType = DbType.Boolean;
				colvarInUnabr.MaxLength = 0;
				colvarInUnabr.AutoIncrement = false;
				colvarInUnabr.IsNullable = false;
				colvarInUnabr.IsPrimaryKey = false;
				colvarInUnabr.IsForeignKey = false;
				colvarInUnabr.IsReadOnly = false;
				
						colvarInUnabr.DefaultSetting = @"((0))";
				colvarInUnabr.ForeignKeyTableName = "";
				schema.Columns.Add(colvarInUnabr);
				
				TableSchema.TableColumn colvarInAntworth = new TableSchema.TableColumn(schema);
				colvarInAntworth.ColumnName = "InAntworth";
				colvarInAntworth.DataType = DbType.Boolean;
				colvarInAntworth.MaxLength = 0;
				colvarInAntworth.AutoIncrement = false;
				colvarInAntworth.IsNullable = false;
				colvarInAntworth.IsPrimaryKey = false;
				colvarInAntworth.IsForeignKey = false;
				colvarInAntworth.IsReadOnly = false;
				
						colvarInAntworth.DefaultSetting = @"((0))";
				colvarInAntworth.ForeignKeyTableName = "";
				schema.Columns.Add(colvarInAntworth);
				
				TableSchema.TableColumn colvarInCRL = new TableSchema.TableColumn(schema);
				colvarInCRL.ColumnName = "InCRL";
				colvarInCRL.DataType = DbType.Boolean;
				colvarInCRL.MaxLength = 0;
				colvarInCRL.AutoIncrement = false;
				colvarInCRL.IsNullable = false;
				colvarInCRL.IsPrimaryKey = false;
				colvarInCRL.IsForeignKey = false;
				colvarInCRL.IsReadOnly = false;
				
						colvarInCRL.DefaultSetting = @"((0))";
				colvarInCRL.ForeignKeyTableName = "";
				schema.Columns.Add(colvarInCRL);
				
				TableSchema.TableColumn colvarInRoget = new TableSchema.TableColumn(schema);
				colvarInRoget.ColumnName = "InRoget";
				colvarInRoget.DataType = DbType.Boolean;
				colvarInRoget.MaxLength = 0;
				colvarInRoget.AutoIncrement = false;
				colvarInRoget.IsNullable = false;
				colvarInRoget.IsPrimaryKey = false;
				colvarInRoget.IsForeignKey = false;
				colvarInRoget.IsReadOnly = false;
				
						colvarInRoget.DefaultSetting = @"((0))";
				colvarInRoget.ForeignKeyTableName = "";
				schema.Columns.Add(colvarInRoget);
				
				TableSchema.TableColumn colvarInUnix = new TableSchema.TableColumn(schema);
				colvarInUnix.ColumnName = "InUnix";
				colvarInUnix.DataType = DbType.Boolean;
				colvarInUnix.MaxLength = 0;
				colvarInUnix.AutoIncrement = false;
				colvarInUnix.IsNullable = false;
				colvarInUnix.IsPrimaryKey = false;
				colvarInUnix.IsForeignKey = false;
				colvarInUnix.IsReadOnly = false;
				
						colvarInUnix.DefaultSetting = @"((0))";
				colvarInUnix.ForeignKeyTableName = "";
				schema.Columns.Add(colvarInUnix);
				
				TableSchema.TableColumn colvarInKnuthBritish = new TableSchema.TableColumn(schema);
				colvarInKnuthBritish.ColumnName = "InKnuthBritish";
				colvarInKnuthBritish.DataType = DbType.Boolean;
				colvarInKnuthBritish.MaxLength = 0;
				colvarInKnuthBritish.AutoIncrement = false;
				colvarInKnuthBritish.IsNullable = false;
				colvarInKnuthBritish.IsPrimaryKey = false;
				colvarInKnuthBritish.IsForeignKey = false;
				colvarInKnuthBritish.IsReadOnly = false;
				
						colvarInKnuthBritish.DefaultSetting = @"((0))";
				colvarInKnuthBritish.ForeignKeyTableName = "";
				schema.Columns.Add(colvarInKnuthBritish);
				
				TableSchema.TableColumn colvarInKnuth = new TableSchema.TableColumn(schema);
				colvarInKnuth.ColumnName = "InKnuth";
				colvarInKnuth.DataType = DbType.Boolean;
				colvarInKnuth.MaxLength = 0;
				colvarInKnuth.AutoIncrement = false;
				colvarInKnuth.IsNullable = false;
				colvarInKnuth.IsPrimaryKey = false;
				colvarInKnuth.IsForeignKey = false;
				colvarInKnuth.IsReadOnly = false;
				
						colvarInKnuth.DefaultSetting = @"((0))";
				colvarInKnuth.ForeignKeyTableName = "";
				schema.Columns.Add(colvarInKnuth);
				
				TableSchema.TableColumn colvarInEnglex = new TableSchema.TableColumn(schema);
				colvarInEnglex.ColumnName = "InEnglex";
				colvarInEnglex.DataType = DbType.Boolean;
				colvarInEnglex.MaxLength = 0;
				colvarInEnglex.AutoIncrement = false;
				colvarInEnglex.IsNullable = false;
				colvarInEnglex.IsPrimaryKey = false;
				colvarInEnglex.IsForeignKey = false;
				colvarInEnglex.IsReadOnly = false;
				
						colvarInEnglex.DefaultSetting = @"((0))";
				colvarInEnglex.ForeignKeyTableName = "";
				schema.Columns.Add(colvarInEnglex);
				
				TableSchema.TableColumn colvarInShakespeare = new TableSchema.TableColumn(schema);
				colvarInShakespeare.ColumnName = "InShakespeare";
				colvarInShakespeare.DataType = DbType.Boolean;
				colvarInShakespeare.MaxLength = 0;
				colvarInShakespeare.AutoIncrement = false;
				colvarInShakespeare.IsNullable = false;
				colvarInShakespeare.IsPrimaryKey = false;
				colvarInShakespeare.IsForeignKey = false;
				colvarInShakespeare.IsReadOnly = false;
				
						colvarInShakespeare.DefaultSetting = @"((0))";
				colvarInShakespeare.ForeignKeyTableName = "";
				schema.Columns.Add(colvarInShakespeare);
				
				TableSchema.TableColumn colvarInPocket = new TableSchema.TableColumn(schema);
				colvarInPocket.ColumnName = "InPocket";
				colvarInPocket.DataType = DbType.Boolean;
				colvarInPocket.MaxLength = 0;
				colvarInPocket.AutoIncrement = false;
				colvarInPocket.IsNullable = false;
				colvarInPocket.IsPrimaryKey = false;
				colvarInPocket.IsForeignKey = false;
				colvarInPocket.IsReadOnly = false;
				
						colvarInPocket.DefaultSetting = @"((0))";
				colvarInPocket.ForeignKeyTableName = "";
				schema.Columns.Add(colvarInPocket);
				
				TableSchema.TableColumn colvarInUUNet = new TableSchema.TableColumn(schema);
				colvarInUUNet.ColumnName = "InUUNet";
				colvarInUUNet.DataType = DbType.Boolean;
				colvarInUUNet.MaxLength = 0;
				colvarInUUNet.AutoIncrement = false;
				colvarInUUNet.IsNullable = false;
				colvarInUUNet.IsPrimaryKey = false;
				colvarInUUNet.IsForeignKey = false;
				colvarInUUNet.IsReadOnly = false;
				
						colvarInUUNet.DefaultSetting = @"((0))";
				colvarInUUNet.ForeignKeyTableName = "";
				schema.Columns.Add(colvarInUUNet);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["PowerPOS"].AddSchema("Word",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("WordGuid")]
		[Bindable(true)]
		public Guid WordGuid 
		{
			get { return GetColumnValue<Guid>(Columns.WordGuid); }
			set { SetColumnValue(Columns.WordGuid, value); }
		}
		  
		[XmlAttribute("WordText")]
		[Bindable(true)]
		public string WordText 
		{
			get { return GetColumnValue<string>(Columns.WordText); }
			set { SetColumnValue(Columns.WordText, value); }
		}
		  
		[XmlAttribute("WordLength")]
		[Bindable(true)]
		public byte WordLength 
		{
			get { return GetColumnValue<byte>(Columns.WordLength); }
			set { SetColumnValue(Columns.WordLength, value); }
		}
		  
		[XmlAttribute("SoundexGroup")]
		[Bindable(true)]
		public string SoundexGroup 
		{
			get { return GetColumnValue<string>(Columns.SoundexGroup); }
			set { SetColumnValue(Columns.SoundexGroup, value); }
		}
		  
		[XmlAttribute("SoundexValue")]
		[Bindable(true)]
		public short SoundexValue 
		{
			get { return GetColumnValue<short>(Columns.SoundexValue); }
			set { SetColumnValue(Columns.SoundexValue, value); }
		}
		  
		[XmlAttribute("GroupId")]
		[Bindable(true)]
		public short? GroupId 
		{
			get { return GetColumnValue<short?>(Columns.GroupId); }
			set { SetColumnValue(Columns.GroupId, value); }
		}
		  
		[XmlAttribute("IsPalindrome")]
		[Bindable(true)]
		public bool IsPalindrome 
		{
			get { return GetColumnValue<bool>(Columns.IsPalindrome); }
			set { SetColumnValue(Columns.IsPalindrome, value); }
		}
		  
		[XmlAttribute("InUnabr")]
		[Bindable(true)]
		public bool InUnabr 
		{
			get { return GetColumnValue<bool>(Columns.InUnabr); }
			set { SetColumnValue(Columns.InUnabr, value); }
		}
		  
		[XmlAttribute("InAntworth")]
		[Bindable(true)]
		public bool InAntworth 
		{
			get { return GetColumnValue<bool>(Columns.InAntworth); }
			set { SetColumnValue(Columns.InAntworth, value); }
		}
		  
		[XmlAttribute("InCRL")]
		[Bindable(true)]
		public bool InCRL 
		{
			get { return GetColumnValue<bool>(Columns.InCRL); }
			set { SetColumnValue(Columns.InCRL, value); }
		}
		  
		[XmlAttribute("InRoget")]
		[Bindable(true)]
		public bool InRoget 
		{
			get { return GetColumnValue<bool>(Columns.InRoget); }
			set { SetColumnValue(Columns.InRoget, value); }
		}
		  
		[XmlAttribute("InUnix")]
		[Bindable(true)]
		public bool InUnix 
		{
			get { return GetColumnValue<bool>(Columns.InUnix); }
			set { SetColumnValue(Columns.InUnix, value); }
		}
		  
		[XmlAttribute("InKnuthBritish")]
		[Bindable(true)]
		public bool InKnuthBritish 
		{
			get { return GetColumnValue<bool>(Columns.InKnuthBritish); }
			set { SetColumnValue(Columns.InKnuthBritish, value); }
		}
		  
		[XmlAttribute("InKnuth")]
		[Bindable(true)]
		public bool InKnuth 
		{
			get { return GetColumnValue<bool>(Columns.InKnuth); }
			set { SetColumnValue(Columns.InKnuth, value); }
		}
		  
		[XmlAttribute("InEnglex")]
		[Bindable(true)]
		public bool InEnglex 
		{
			get { return GetColumnValue<bool>(Columns.InEnglex); }
			set { SetColumnValue(Columns.InEnglex, value); }
		}
		  
		[XmlAttribute("InShakespeare")]
		[Bindable(true)]
		public bool InShakespeare 
		{
			get { return GetColumnValue<bool>(Columns.InShakespeare); }
			set { SetColumnValue(Columns.InShakespeare, value); }
		}
		  
		[XmlAttribute("InPocket")]
		[Bindable(true)]
		public bool InPocket 
		{
			get { return GetColumnValue<bool>(Columns.InPocket); }
			set { SetColumnValue(Columns.InPocket, value); }
		}
		  
		[XmlAttribute("InUUNet")]
		[Bindable(true)]
		public bool InUUNet 
		{
			get { return GetColumnValue<bool>(Columns.InUUNet); }
			set { SetColumnValue(Columns.InUUNet, value); }
		}
		
		#endregion
		
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(Guid varWordGuid,string varWordText,byte varWordLength,string varSoundexGroup,short varSoundexValue,short? varGroupId,bool varIsPalindrome,bool varInUnabr,bool varInAntworth,bool varInCRL,bool varInRoget,bool varInUnix,bool varInKnuthBritish,bool varInKnuth,bool varInEnglex,bool varInShakespeare,bool varInPocket,bool varInUUNet)
		{
			Word item = new Word();
			
			item.WordGuid = varWordGuid;
			
			item.WordText = varWordText;
			
			item.WordLength = varWordLength;
			
			item.SoundexGroup = varSoundexGroup;
			
			item.SoundexValue = varSoundexValue;
			
			item.GroupId = varGroupId;
			
			item.IsPalindrome = varIsPalindrome;
			
			item.InUnabr = varInUnabr;
			
			item.InAntworth = varInAntworth;
			
			item.InCRL = varInCRL;
			
			item.InRoget = varInRoget;
			
			item.InUnix = varInUnix;
			
			item.InKnuthBritish = varInKnuthBritish;
			
			item.InKnuth = varInKnuth;
			
			item.InEnglex = varInEnglex;
			
			item.InShakespeare = varInShakespeare;
			
			item.InPocket = varInPocket;
			
			item.InUUNet = varInUUNet;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(Guid varWordGuid,string varWordText,byte varWordLength,string varSoundexGroup,short varSoundexValue,short? varGroupId,bool varIsPalindrome,bool varInUnabr,bool varInAntworth,bool varInCRL,bool varInRoget,bool varInUnix,bool varInKnuthBritish,bool varInKnuth,bool varInEnglex,bool varInShakespeare,bool varInPocket,bool varInUUNet)
		{
			Word item = new Word();
			
				item.WordGuid = varWordGuid;
			
				item.WordText = varWordText;
			
				item.WordLength = varWordLength;
			
				item.SoundexGroup = varSoundexGroup;
			
				item.SoundexValue = varSoundexValue;
			
				item.GroupId = varGroupId;
			
				item.IsPalindrome = varIsPalindrome;
			
				item.InUnabr = varInUnabr;
			
				item.InAntworth = varInAntworth;
			
				item.InCRL = varInCRL;
			
				item.InRoget = varInRoget;
			
				item.InUnix = varInUnix;
			
				item.InKnuthBritish = varInKnuthBritish;
			
				item.InKnuth = varInKnuth;
			
				item.InEnglex = varInEnglex;
			
				item.InShakespeare = varInShakespeare;
			
				item.InPocket = varInPocket;
			
				item.InUUNet = varInUUNet;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn WordGuidColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn WordTextColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn WordLengthColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn SoundexGroupColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn SoundexValueColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn GroupIdColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn IsPalindromeColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn InUnabrColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn InAntworthColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn InCRLColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn InRogetColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        public static TableSchema.TableColumn InUnixColumn
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        public static TableSchema.TableColumn InKnuthBritishColumn
        {
            get { return Schema.Columns[12]; }
        }
        
        
        
        public static TableSchema.TableColumn InKnuthColumn
        {
            get { return Schema.Columns[13]; }
        }
        
        
        
        public static TableSchema.TableColumn InEnglexColumn
        {
            get { return Schema.Columns[14]; }
        }
        
        
        
        public static TableSchema.TableColumn InShakespeareColumn
        {
            get { return Schema.Columns[15]; }
        }
        
        
        
        public static TableSchema.TableColumn InPocketColumn
        {
            get { return Schema.Columns[16]; }
        }
        
        
        
        public static TableSchema.TableColumn InUUNetColumn
        {
            get { return Schema.Columns[17]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string WordGuid = @"WordGuid";
			 public static string WordText = @"WordText";
			 public static string WordLength = @"WordLength";
			 public static string SoundexGroup = @"SoundexGroup";
			 public static string SoundexValue = @"SoundexValue";
			 public static string GroupId = @"GroupId";
			 public static string IsPalindrome = @"IsPalindrome";
			 public static string InUnabr = @"InUnabr";
			 public static string InAntworth = @"InAntworth";
			 public static string InCRL = @"InCRL";
			 public static string InRoget = @"InRoget";
			 public static string InUnix = @"InUnix";
			 public static string InKnuthBritish = @"InKnuthBritish";
			 public static string InKnuth = @"InKnuth";
			 public static string InEnglex = @"InEnglex";
			 public static string InShakespeare = @"InShakespeare";
			 public static string InPocket = @"InPocket";
			 public static string InUUNet = @"InUUNet";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
