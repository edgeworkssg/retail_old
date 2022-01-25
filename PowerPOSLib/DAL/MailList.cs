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
	/// Strongly-typed collection for the MailList class.
	/// </summary>
	[Serializable]
	public partial class MailListCollection : ActiveList<MailList, MailListCollection> 
	{	   
		public MailListCollection() {}

	}

	/// <summary>
	/// This is an ActiveRecord class which wraps the MailList table.
	/// </summary>
	[Serializable]
	public partial class MailList : ActiveRecord<MailList>
	{
		#region .ctors and Default Settings
		
		public MailList()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}

		
		private void InitSetDefaults() { SetDefaults(); }

		
		public MailList(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}

		public MailList(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}

		 
		public MailList(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("MailList", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarMembershipNo = new TableSchema.TableColumn(schema);
				colvarMembershipNo.ColumnName = "MembershipNo";
				colvarMembershipNo.DataType = DbType.String;
				colvarMembershipNo.MaxLength = 200;
				colvarMembershipNo.AutoIncrement = false;
				colvarMembershipNo.IsNullable = false;
				colvarMembershipNo.IsPrimaryKey = true;
				colvarMembershipNo.IsForeignKey = false;
				colvarMembershipNo.IsReadOnly = false;
				colvarMembershipNo.DefaultSetting = @"";
				colvarMembershipNo.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMembershipNo);
				
				TableSchema.TableColumn colvarFirstName = new TableSchema.TableColumn(schema);
				colvarFirstName.ColumnName = "FirstName";
				colvarFirstName.DataType = DbType.String;
				colvarFirstName.MaxLength = 200;
				colvarFirstName.AutoIncrement = false;
				colvarFirstName.IsNullable = true;
				colvarFirstName.IsPrimaryKey = false;
				colvarFirstName.IsForeignKey = false;
				colvarFirstName.IsReadOnly = false;
				colvarFirstName.DefaultSetting = @"";
				colvarFirstName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarFirstName);
				
				TableSchema.TableColumn colvarTotalPoints = new TableSchema.TableColumn(schema);
				colvarTotalPoints.ColumnName = "TotalPoints";
				colvarTotalPoints.DataType = DbType.Currency;
				colvarTotalPoints.MaxLength = 0;
				colvarTotalPoints.AutoIncrement = false;
				colvarTotalPoints.IsNullable = true;
				colvarTotalPoints.IsPrimaryKey = false;
				colvarTotalPoints.IsForeignKey = false;
				colvarTotalPoints.IsReadOnly = false;
				colvarTotalPoints.DefaultSetting = @"";
				colvarTotalPoints.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTotalPoints);
				
				TableSchema.TableColumn colvarTotalAmount = new TableSchema.TableColumn(schema);
				colvarTotalAmount.ColumnName = "TotalAmount";
				colvarTotalAmount.DataType = DbType.Currency;
				colvarTotalAmount.MaxLength = 0;
				colvarTotalAmount.AutoIncrement = false;
				colvarTotalAmount.IsNullable = true;
				colvarTotalAmount.IsPrimaryKey = false;
				colvarTotalAmount.IsForeignKey = false;
				colvarTotalAmount.IsReadOnly = false;
				colvarTotalAmount.DefaultSetting = @"";
				colvarTotalAmount.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTotalAmount);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["PowerPOS"].AddSchema("MailList",schema);
			}

		}

		#endregion
		
		#region Props
		
		  
		[XmlAttribute("MembershipNo")]
		public string MembershipNo 
		{
			get { return GetColumnValue<string>("MembershipNo"); }

			set { SetColumnValue("MembershipNo", value); }

		}

		  
		[XmlAttribute("FirstName")]
		public string FirstName 
		{
			get { return GetColumnValue<string>("FirstName"); }

			set { SetColumnValue("FirstName", value); }

		}

		  
		[XmlAttribute("TotalPoints")]
		public decimal? TotalPoints 
		{
			get { return GetColumnValue<decimal?>("TotalPoints"); }

			set { SetColumnValue("TotalPoints", value); }

		}

		  
		[XmlAttribute("TotalAmount")]
		public decimal? TotalAmount 
		{
			get { return GetColumnValue<decimal?>("TotalAmount"); }

			set { SetColumnValue("TotalAmount", value); }

		}

		
		#endregion
		
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(string varMembershipNo,string varFirstName,decimal? varTotalPoints,decimal? varTotalAmount)
		{
			MailList item = new MailList();
			
			item.MembershipNo = varMembershipNo;
			
			item.FirstName = varFirstName;
			
			item.TotalPoints = varTotalPoints;
			
			item.TotalAmount = varTotalAmount;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}

		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(string varMembershipNo,string varFirstName,decimal? varTotalPoints,decimal? varTotalAmount)
		{
			MailList item = new MailList();
			
				item.MembershipNo = varMembershipNo;
				
				item.FirstName = varFirstName;
				
				item.TotalPoints = varTotalPoints;
				
				item.TotalAmount = varTotalAmount;
				
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}

		#endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string MembershipNo = @"MembershipNo";
			 public static string FirstName = @"FirstName";
			 public static string TotalPoints = @"TotalPoints";
			 public static string TotalAmount = @"TotalAmount";
						
		}

		#endregion
	}

}

