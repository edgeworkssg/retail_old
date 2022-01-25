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
	/// Strongly-typed collection for the Membership class.
	/// </summary>
    [Serializable]
	public partial class MembershipCollection : ActiveList<Membership, MembershipCollection>
	{	   
		public MembershipCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>MembershipCollection</returns>
		public MembershipCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                Membership o = this[i];
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
	/// This is an ActiveRecord class which wraps the Membership table.
	/// </summary>
	[Serializable]
	public partial class Membership : ActiveRecord<Membership>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public Membership()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public Membership(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public Membership(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public Membership(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("Membership", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarMembershipNo = new TableSchema.TableColumn(schema);
				colvarMembershipNo.ColumnName = "MembershipNo";
				colvarMembershipNo.DataType = DbType.AnsiString;
				colvarMembershipNo.MaxLength = 50;
				colvarMembershipNo.AutoIncrement = false;
				colvarMembershipNo.IsNullable = false;
				colvarMembershipNo.IsPrimaryKey = true;
				colvarMembershipNo.IsForeignKey = false;
				colvarMembershipNo.IsReadOnly = false;
				colvarMembershipNo.DefaultSetting = @"";
				colvarMembershipNo.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMembershipNo);
				
				TableSchema.TableColumn colvarMembershipGroupId = new TableSchema.TableColumn(schema);
				colvarMembershipGroupId.ColumnName = "MembershipGroupId";
				colvarMembershipGroupId.DataType = DbType.Int32;
				colvarMembershipGroupId.MaxLength = 0;
				colvarMembershipGroupId.AutoIncrement = false;
				colvarMembershipGroupId.IsNullable = false;
				colvarMembershipGroupId.IsPrimaryKey = false;
				colvarMembershipGroupId.IsForeignKey = true;
				colvarMembershipGroupId.IsReadOnly = false;
				colvarMembershipGroupId.DefaultSetting = @"";
				
					colvarMembershipGroupId.ForeignKeyTableName = "MembershipGroup";
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
				colvarTitle.DefaultSetting = @"";
				colvarTitle.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTitle);
				
				TableSchema.TableColumn colvarLastName = new TableSchema.TableColumn(schema);
				colvarLastName.ColumnName = "LastName";
				colvarLastName.DataType = DbType.String;
				colvarLastName.MaxLength = 80;
				colvarLastName.AutoIncrement = false;
				colvarLastName.IsNullable = true;
				colvarLastName.IsPrimaryKey = false;
				colvarLastName.IsForeignKey = false;
				colvarLastName.IsReadOnly = false;
				colvarLastName.DefaultSetting = @"";
				colvarLastName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarLastName);
				
				TableSchema.TableColumn colvarFirstName = new TableSchema.TableColumn(schema);
				colvarFirstName.ColumnName = "FirstName";
				colvarFirstName.DataType = DbType.String;
				colvarFirstName.MaxLength = 80;
				colvarFirstName.AutoIncrement = false;
				colvarFirstName.IsNullable = true;
				colvarFirstName.IsPrimaryKey = false;
				colvarFirstName.IsForeignKey = false;
				colvarFirstName.IsReadOnly = false;
				colvarFirstName.DefaultSetting = @"";
				colvarFirstName.ForeignKeyTableName = "";
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
				colvarChristianName.DefaultSetting = @"";
				colvarChristianName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarChristianName);
				
				TableSchema.TableColumn colvarChineseName = new TableSchema.TableColumn(schema);
				colvarChineseName.ColumnName = "ChineseName";
				colvarChineseName.DataType = DbType.String;
				colvarChineseName.MaxLength = 50;
				colvarChineseName.AutoIncrement = false;
				colvarChineseName.IsNullable = true;
				colvarChineseName.IsPrimaryKey = false;
				colvarChineseName.IsForeignKey = false;
				colvarChineseName.IsReadOnly = false;
				colvarChineseName.DefaultSetting = @"";
				colvarChineseName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarChineseName);
				
				TableSchema.TableColumn colvarNameToAppear = new TableSchema.TableColumn(schema);
				colvarNameToAppear.ColumnName = "NameToAppear";
				colvarNameToAppear.DataType = DbType.String;
				colvarNameToAppear.MaxLength = 200;
				colvarNameToAppear.AutoIncrement = false;
				colvarNameToAppear.IsNullable = true;
				colvarNameToAppear.IsPrimaryKey = false;
				colvarNameToAppear.IsForeignKey = false;
				colvarNameToAppear.IsReadOnly = false;
				colvarNameToAppear.DefaultSetting = @"";
				colvarNameToAppear.ForeignKeyTableName = "";
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
				colvarGender.DefaultSetting = @"";
				colvarGender.ForeignKeyTableName = "";
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
				colvarDateOfBirth.DefaultSetting = @"";
				colvarDateOfBirth.ForeignKeyTableName = "";
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
				colvarNationality.DefaultSetting = @"";
				colvarNationality.ForeignKeyTableName = "";
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
				colvarNric.DefaultSetting = @"";
				colvarNric.ForeignKeyTableName = "";
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
				colvarOccupation.DefaultSetting = @"";
				colvarOccupation.ForeignKeyTableName = "";
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
				colvarMaritalStatus.DefaultSetting = @"";
				colvarMaritalStatus.ForeignKeyTableName = "";
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
				colvarEmail.DefaultSetting = @"";
				colvarEmail.ForeignKeyTableName = "";
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
				colvarBlock.DefaultSetting = @"";
				colvarBlock.ForeignKeyTableName = "";
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
				colvarBuildingName.DefaultSetting = @"";
				colvarBuildingName.ForeignKeyTableName = "";
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
				colvarStreetName.DefaultSetting = @"";
				colvarStreetName.ForeignKeyTableName = "";
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
				colvarStreetName2.DefaultSetting = @"";
				colvarStreetName2.ForeignKeyTableName = "";
				schema.Columns.Add(colvarStreetName2);
				
				TableSchema.TableColumn colvarUnitNo = new TableSchema.TableColumn(schema);
				colvarUnitNo.ColumnName = "UnitNo";
				colvarUnitNo.DataType = DbType.AnsiString;
				colvarUnitNo.MaxLength = 50;
				colvarUnitNo.AutoIncrement = false;
				colvarUnitNo.IsNullable = true;
				colvarUnitNo.IsPrimaryKey = false;
				colvarUnitNo.IsForeignKey = false;
				colvarUnitNo.IsReadOnly = false;
				colvarUnitNo.DefaultSetting = @"";
				colvarUnitNo.ForeignKeyTableName = "";
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
				colvarCity.DefaultSetting = @"";
				colvarCity.ForeignKeyTableName = "";
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
				colvarCountry.DefaultSetting = @"";
				colvarCountry.ForeignKeyTableName = "";
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
				colvarZipCode.DefaultSetting = @"";
				colvarZipCode.ForeignKeyTableName = "";
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
				colvarMobile.DefaultSetting = @"";
				colvarMobile.ForeignKeyTableName = "";
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
				colvarOffice.DefaultSetting = @"";
				colvarOffice.ForeignKeyTableName = "";
				schema.Columns.Add(colvarOffice);
				
				TableSchema.TableColumn colvarFax = new TableSchema.TableColumn(schema);
				colvarFax.ColumnName = "Fax";
				colvarFax.DataType = DbType.AnsiString;
				colvarFax.MaxLength = 50;
				colvarFax.AutoIncrement = false;
				colvarFax.IsNullable = true;
				colvarFax.IsPrimaryKey = false;
				colvarFax.IsForeignKey = false;
				colvarFax.IsReadOnly = false;
				colvarFax.DefaultSetting = @"";
				colvarFax.ForeignKeyTableName = "";
				schema.Columns.Add(colvarFax);
				
				TableSchema.TableColumn colvarHome = new TableSchema.TableColumn(schema);
				colvarHome.ColumnName = "Home";
				colvarHome.DataType = DbType.AnsiString;
				colvarHome.MaxLength = 50;
				colvarHome.AutoIncrement = false;
				colvarHome.IsNullable = true;
				colvarHome.IsPrimaryKey = false;
				colvarHome.IsForeignKey = false;
				colvarHome.IsReadOnly = false;
				colvarHome.DefaultSetting = @"";
				colvarHome.ForeignKeyTableName = "";
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
				colvarExpiryDate.DefaultSetting = @"";
				colvarExpiryDate.ForeignKeyTableName = "";
				schema.Columns.Add(colvarExpiryDate);
				
				TableSchema.TableColumn colvarRemarks = new TableSchema.TableColumn(schema);
				colvarRemarks.ColumnName = "Remarks";
				colvarRemarks.DataType = DbType.String;
				colvarRemarks.MaxLength = -1;
				colvarRemarks.AutoIncrement = false;
				colvarRemarks.IsNullable = true;
				colvarRemarks.IsPrimaryKey = false;
				colvarRemarks.IsForeignKey = false;
				colvarRemarks.IsReadOnly = false;
				colvarRemarks.DefaultSetting = @"";
				colvarRemarks.ForeignKeyTableName = "";
				schema.Columns.Add(colvarRemarks);
				
				TableSchema.TableColumn colvarSubscriptionDate = new TableSchema.TableColumn(schema);
				colvarSubscriptionDate.ColumnName = "SubscriptionDate";
				colvarSubscriptionDate.DataType = DbType.DateTime;
				colvarSubscriptionDate.MaxLength = 0;
				colvarSubscriptionDate.AutoIncrement = false;
				colvarSubscriptionDate.IsNullable = true;
				colvarSubscriptionDate.IsPrimaryKey = false;
				colvarSubscriptionDate.IsForeignKey = false;
				colvarSubscriptionDate.IsReadOnly = false;
				colvarSubscriptionDate.DefaultSetting = @"";
				colvarSubscriptionDate.ForeignKeyTableName = "";
				schema.Columns.Add(colvarSubscriptionDate);
				
				TableSchema.TableColumn colvarIsChc = new TableSchema.TableColumn(schema);
				colvarIsChc.ColumnName = "IsChc";
				colvarIsChc.DataType = DbType.Boolean;
				colvarIsChc.MaxLength = 0;
				colvarIsChc.AutoIncrement = false;
				colvarIsChc.IsNullable = true;
				colvarIsChc.IsPrimaryKey = false;
				colvarIsChc.IsForeignKey = false;
				colvarIsChc.IsReadOnly = false;
				colvarIsChc.DefaultSetting = @"";
				colvarIsChc.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIsChc);
				
				TableSchema.TableColumn colvarMinistry = new TableSchema.TableColumn(schema);
				colvarMinistry.ColumnName = "Ministry";
				colvarMinistry.DataType = DbType.AnsiString;
				colvarMinistry.MaxLength = 100;
				colvarMinistry.AutoIncrement = false;
				colvarMinistry.IsNullable = true;
				colvarMinistry.IsPrimaryKey = false;
				colvarMinistry.IsForeignKey = false;
				colvarMinistry.IsReadOnly = false;
				colvarMinistry.DefaultSetting = @"";
				colvarMinistry.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMinistry);
				
				TableSchema.TableColumn colvarIsStudentCard = new TableSchema.TableColumn(schema);
				colvarIsStudentCard.ColumnName = "IsStudentCard";
				colvarIsStudentCard.DataType = DbType.Boolean;
				colvarIsStudentCard.MaxLength = 0;
				colvarIsStudentCard.AutoIncrement = false;
				colvarIsStudentCard.IsNullable = true;
				colvarIsStudentCard.IsPrimaryKey = false;
				colvarIsStudentCard.IsForeignKey = false;
				colvarIsStudentCard.IsReadOnly = false;
				colvarIsStudentCard.DefaultSetting = @"";
				colvarIsStudentCard.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIsStudentCard);
				
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
				
				TableSchema.TableColumn colvarUniqueID = new TableSchema.TableColumn(schema);
				colvarUniqueID.ColumnName = "UniqueID";
				colvarUniqueID.DataType = DbType.Guid;
				colvarUniqueID.MaxLength = 0;
				colvarUniqueID.AutoIncrement = false;
				colvarUniqueID.IsNullable = false;
				colvarUniqueID.IsPrimaryKey = false;
				colvarUniqueID.IsForeignKey = false;
				colvarUniqueID.IsReadOnly = false;
				
						colvarUniqueID.DefaultSetting = @"(newid())";
				colvarUniqueID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUniqueID);
				
				TableSchema.TableColumn colvarUserfld1 = new TableSchema.TableColumn(schema);
				colvarUserfld1.ColumnName = "userfld1";
				colvarUserfld1.DataType = DbType.String;
				colvarUserfld1.MaxLength = 50;
				colvarUserfld1.AutoIncrement = false;
				colvarUserfld1.IsNullable = true;
				colvarUserfld1.IsPrimaryKey = false;
				colvarUserfld1.IsForeignKey = false;
				colvarUserfld1.IsReadOnly = false;
				colvarUserfld1.DefaultSetting = @"";
				colvarUserfld1.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfld1);
				
				TableSchema.TableColumn colvarUserfld2 = new TableSchema.TableColumn(schema);
				colvarUserfld2.ColumnName = "userfld2";
				colvarUserfld2.DataType = DbType.String;
				colvarUserfld2.MaxLength = 50;
				colvarUserfld2.AutoIncrement = false;
				colvarUserfld2.IsNullable = true;
				colvarUserfld2.IsPrimaryKey = false;
				colvarUserfld2.IsForeignKey = false;
				colvarUserfld2.IsReadOnly = false;
				colvarUserfld2.DefaultSetting = @"";
				colvarUserfld2.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfld2);
				
				TableSchema.TableColumn colvarUserfld3 = new TableSchema.TableColumn(schema);
				colvarUserfld3.ColumnName = "userfld3";
				colvarUserfld3.DataType = DbType.String;
				colvarUserfld3.MaxLength = 50;
				colvarUserfld3.AutoIncrement = false;
				colvarUserfld3.IsNullable = true;
				colvarUserfld3.IsPrimaryKey = false;
				colvarUserfld3.IsForeignKey = false;
				colvarUserfld3.IsReadOnly = false;
				colvarUserfld3.DefaultSetting = @"";
				colvarUserfld3.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfld3);
				
				TableSchema.TableColumn colvarUserfld4 = new TableSchema.TableColumn(schema);
				colvarUserfld4.ColumnName = "userfld4";
				colvarUserfld4.DataType = DbType.String;
				colvarUserfld4.MaxLength = 50;
				colvarUserfld4.AutoIncrement = false;
				colvarUserfld4.IsNullable = true;
				colvarUserfld4.IsPrimaryKey = false;
				colvarUserfld4.IsForeignKey = false;
				colvarUserfld4.IsReadOnly = false;
				colvarUserfld4.DefaultSetting = @"";
				colvarUserfld4.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfld4);
				
				TableSchema.TableColumn colvarUserfld5 = new TableSchema.TableColumn(schema);
				colvarUserfld5.ColumnName = "userfld5";
				colvarUserfld5.DataType = DbType.String;
				colvarUserfld5.MaxLength = 50;
				colvarUserfld5.AutoIncrement = false;
				colvarUserfld5.IsNullable = true;
				colvarUserfld5.IsPrimaryKey = false;
				colvarUserfld5.IsForeignKey = false;
				colvarUserfld5.IsReadOnly = false;
				colvarUserfld5.DefaultSetting = @"";
				colvarUserfld5.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfld5);
				
				TableSchema.TableColumn colvarUserfld6 = new TableSchema.TableColumn(schema);
				colvarUserfld6.ColumnName = "userfld6";
				colvarUserfld6.DataType = DbType.String;
				colvarUserfld6.MaxLength = 50;
				colvarUserfld6.AutoIncrement = false;
				colvarUserfld6.IsNullable = true;
				colvarUserfld6.IsPrimaryKey = false;
				colvarUserfld6.IsForeignKey = false;
				colvarUserfld6.IsReadOnly = false;
				colvarUserfld6.DefaultSetting = @"";
				colvarUserfld6.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfld6);
				
				TableSchema.TableColumn colvarUserfld7 = new TableSchema.TableColumn(schema);
				colvarUserfld7.ColumnName = "userfld7";
				colvarUserfld7.DataType = DbType.String;
				colvarUserfld7.MaxLength = 50;
				colvarUserfld7.AutoIncrement = false;
				colvarUserfld7.IsNullable = true;
				colvarUserfld7.IsPrimaryKey = false;
				colvarUserfld7.IsForeignKey = false;
				colvarUserfld7.IsReadOnly = false;
				colvarUserfld7.DefaultSetting = @"";
				colvarUserfld7.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfld7);
				
				TableSchema.TableColumn colvarUserfld8 = new TableSchema.TableColumn(schema);
				colvarUserfld8.ColumnName = "userfld8";
				colvarUserfld8.DataType = DbType.String;
				colvarUserfld8.MaxLength = 50;
				colvarUserfld8.AutoIncrement = false;
				colvarUserfld8.IsNullable = true;
				colvarUserfld8.IsPrimaryKey = false;
				colvarUserfld8.IsForeignKey = false;
				colvarUserfld8.IsReadOnly = false;
				colvarUserfld8.DefaultSetting = @"";
				colvarUserfld8.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfld8);
				
				TableSchema.TableColumn colvarUserfld9 = new TableSchema.TableColumn(schema);
				colvarUserfld9.ColumnName = "userfld9";
				colvarUserfld9.DataType = DbType.String;
				colvarUserfld9.MaxLength = 50;
				colvarUserfld9.AutoIncrement = false;
				colvarUserfld9.IsNullable = true;
				colvarUserfld9.IsPrimaryKey = false;
				colvarUserfld9.IsForeignKey = false;
				colvarUserfld9.IsReadOnly = false;
				colvarUserfld9.DefaultSetting = @"";
				colvarUserfld9.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfld9);
				
				TableSchema.TableColumn colvarUserfld10 = new TableSchema.TableColumn(schema);
				colvarUserfld10.ColumnName = "userfld10";
				colvarUserfld10.DataType = DbType.String;
				colvarUserfld10.MaxLength = 50;
				colvarUserfld10.AutoIncrement = false;
				colvarUserfld10.IsNullable = true;
				colvarUserfld10.IsPrimaryKey = false;
				colvarUserfld10.IsForeignKey = false;
				colvarUserfld10.IsReadOnly = false;
				colvarUserfld10.DefaultSetting = @"";
				colvarUserfld10.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfld10);
				
				TableSchema.TableColumn colvarUserflag1 = new TableSchema.TableColumn(schema);
				colvarUserflag1.ColumnName = "userflag1";
				colvarUserflag1.DataType = DbType.Boolean;
				colvarUserflag1.MaxLength = 0;
				colvarUserflag1.AutoIncrement = false;
				colvarUserflag1.IsNullable = true;
				colvarUserflag1.IsPrimaryKey = false;
				colvarUserflag1.IsForeignKey = false;
				colvarUserflag1.IsReadOnly = false;
				colvarUserflag1.DefaultSetting = @"";
				colvarUserflag1.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserflag1);
				
				TableSchema.TableColumn colvarUserflag2 = new TableSchema.TableColumn(schema);
				colvarUserflag2.ColumnName = "userflag2";
				colvarUserflag2.DataType = DbType.Boolean;
				colvarUserflag2.MaxLength = 0;
				colvarUserflag2.AutoIncrement = false;
				colvarUserflag2.IsNullable = true;
				colvarUserflag2.IsPrimaryKey = false;
				colvarUserflag2.IsForeignKey = false;
				colvarUserflag2.IsReadOnly = false;
				colvarUserflag2.DefaultSetting = @"";
				colvarUserflag2.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserflag2);
				
				TableSchema.TableColumn colvarUserflag3 = new TableSchema.TableColumn(schema);
				colvarUserflag3.ColumnName = "userflag3";
				colvarUserflag3.DataType = DbType.Boolean;
				colvarUserflag3.MaxLength = 0;
				colvarUserflag3.AutoIncrement = false;
				colvarUserflag3.IsNullable = true;
				colvarUserflag3.IsPrimaryKey = false;
				colvarUserflag3.IsForeignKey = false;
				colvarUserflag3.IsReadOnly = false;
				colvarUserflag3.DefaultSetting = @"";
				colvarUserflag3.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserflag3);
				
				TableSchema.TableColumn colvarUserflag4 = new TableSchema.TableColumn(schema);
				colvarUserflag4.ColumnName = "userflag4";
				colvarUserflag4.DataType = DbType.Boolean;
				colvarUserflag4.MaxLength = 0;
				colvarUserflag4.AutoIncrement = false;
				colvarUserflag4.IsNullable = true;
				colvarUserflag4.IsPrimaryKey = false;
				colvarUserflag4.IsForeignKey = false;
				colvarUserflag4.IsReadOnly = false;
				colvarUserflag4.DefaultSetting = @"";
				colvarUserflag4.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserflag4);
				
				TableSchema.TableColumn colvarUserflag5 = new TableSchema.TableColumn(schema);
				colvarUserflag5.ColumnName = "userflag5";
				colvarUserflag5.DataType = DbType.Boolean;
				colvarUserflag5.MaxLength = 0;
				colvarUserflag5.AutoIncrement = false;
				colvarUserflag5.IsNullable = true;
				colvarUserflag5.IsPrimaryKey = false;
				colvarUserflag5.IsForeignKey = false;
				colvarUserflag5.IsReadOnly = false;
				colvarUserflag5.DefaultSetting = @"";
				colvarUserflag5.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserflag5);
				
				TableSchema.TableColumn colvarUserfloat1 = new TableSchema.TableColumn(schema);
				colvarUserfloat1.ColumnName = "userfloat1";
				colvarUserfloat1.DataType = DbType.Currency;
				colvarUserfloat1.MaxLength = 0;
				colvarUserfloat1.AutoIncrement = false;
				colvarUserfloat1.IsNullable = true;
				colvarUserfloat1.IsPrimaryKey = false;
				colvarUserfloat1.IsForeignKey = false;
				colvarUserfloat1.IsReadOnly = false;
				colvarUserfloat1.DefaultSetting = @"";
				colvarUserfloat1.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfloat1);
				
				TableSchema.TableColumn colvarUserfloat2 = new TableSchema.TableColumn(schema);
				colvarUserfloat2.ColumnName = "userfloat2";
				colvarUserfloat2.DataType = DbType.Currency;
				colvarUserfloat2.MaxLength = 0;
				colvarUserfloat2.AutoIncrement = false;
				colvarUserfloat2.IsNullable = true;
				colvarUserfloat2.IsPrimaryKey = false;
				colvarUserfloat2.IsForeignKey = false;
				colvarUserfloat2.IsReadOnly = false;
				colvarUserfloat2.DefaultSetting = @"";
				colvarUserfloat2.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfloat2);
				
				TableSchema.TableColumn colvarUserfloat3 = new TableSchema.TableColumn(schema);
				colvarUserfloat3.ColumnName = "userfloat3";
				colvarUserfloat3.DataType = DbType.Currency;
				colvarUserfloat3.MaxLength = 0;
				colvarUserfloat3.AutoIncrement = false;
				colvarUserfloat3.IsNullable = true;
				colvarUserfloat3.IsPrimaryKey = false;
				colvarUserfloat3.IsForeignKey = false;
				colvarUserfloat3.IsReadOnly = false;
				colvarUserfloat3.DefaultSetting = @"";
				colvarUserfloat3.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfloat3);
				
				TableSchema.TableColumn colvarUserfloat4 = new TableSchema.TableColumn(schema);
				colvarUserfloat4.ColumnName = "userfloat4";
				colvarUserfloat4.DataType = DbType.Currency;
				colvarUserfloat4.MaxLength = 0;
				colvarUserfloat4.AutoIncrement = false;
				colvarUserfloat4.IsNullable = true;
				colvarUserfloat4.IsPrimaryKey = false;
				colvarUserfloat4.IsForeignKey = false;
				colvarUserfloat4.IsReadOnly = false;
				colvarUserfloat4.DefaultSetting = @"";
				colvarUserfloat4.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfloat4);
				
				TableSchema.TableColumn colvarUserfloat5 = new TableSchema.TableColumn(schema);
				colvarUserfloat5.ColumnName = "userfloat5";
				colvarUserfloat5.DataType = DbType.Currency;
				colvarUserfloat5.MaxLength = 0;
				colvarUserfloat5.AutoIncrement = false;
				colvarUserfloat5.IsNullable = true;
				colvarUserfloat5.IsPrimaryKey = false;
				colvarUserfloat5.IsForeignKey = false;
				colvarUserfloat5.IsReadOnly = false;
				colvarUserfloat5.DefaultSetting = @"";
				colvarUserfloat5.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfloat5);
				
				TableSchema.TableColumn colvarUserint1 = new TableSchema.TableColumn(schema);
				colvarUserint1.ColumnName = "userint1";
				colvarUserint1.DataType = DbType.Int32;
				colvarUserint1.MaxLength = 0;
				colvarUserint1.AutoIncrement = false;
				colvarUserint1.IsNullable = true;
				colvarUserint1.IsPrimaryKey = false;
				colvarUserint1.IsForeignKey = false;
				colvarUserint1.IsReadOnly = false;
				colvarUserint1.DefaultSetting = @"";
				colvarUserint1.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserint1);
				
				TableSchema.TableColumn colvarUserint2 = new TableSchema.TableColumn(schema);
				colvarUserint2.ColumnName = "userint2";
				colvarUserint2.DataType = DbType.Int32;
				colvarUserint2.MaxLength = 0;
				colvarUserint2.AutoIncrement = false;
				colvarUserint2.IsNullable = true;
				colvarUserint2.IsPrimaryKey = false;
				colvarUserint2.IsForeignKey = false;
				colvarUserint2.IsReadOnly = false;
				colvarUserint2.DefaultSetting = @"";
				colvarUserint2.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserint2);
				
				TableSchema.TableColumn colvarUserint3 = new TableSchema.TableColumn(schema);
				colvarUserint3.ColumnName = "userint3";
				colvarUserint3.DataType = DbType.Int32;
				colvarUserint3.MaxLength = 0;
				colvarUserint3.AutoIncrement = false;
				colvarUserint3.IsNullable = true;
				colvarUserint3.IsPrimaryKey = false;
				colvarUserint3.IsForeignKey = false;
				colvarUserint3.IsReadOnly = false;
				colvarUserint3.DefaultSetting = @"";
				colvarUserint3.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserint3);
				
				TableSchema.TableColumn colvarUserint4 = new TableSchema.TableColumn(schema);
				colvarUserint4.ColumnName = "userint4";
				colvarUserint4.DataType = DbType.Int32;
				colvarUserint4.MaxLength = 0;
				colvarUserint4.AutoIncrement = false;
				colvarUserint4.IsNullable = true;
				colvarUserint4.IsPrimaryKey = false;
				colvarUserint4.IsForeignKey = false;
				colvarUserint4.IsReadOnly = false;
				colvarUserint4.DefaultSetting = @"";
				colvarUserint4.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserint4);
				
				TableSchema.TableColumn colvarUserint5 = new TableSchema.TableColumn(schema);
				colvarUserint5.ColumnName = "userint5";
				colvarUserint5.DataType = DbType.Int32;
				colvarUserint5.MaxLength = 0;
				colvarUserint5.AutoIncrement = false;
				colvarUserint5.IsNullable = true;
				colvarUserint5.IsPrimaryKey = false;
				colvarUserint5.IsForeignKey = false;
				colvarUserint5.IsReadOnly = false;
				colvarUserint5.DefaultSetting = @"";
				colvarUserint5.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserint5);
				
				TableSchema.TableColumn colvarIsVitaMix = new TableSchema.TableColumn(schema);
				colvarIsVitaMix.ColumnName = "IsVitaMix";
				colvarIsVitaMix.DataType = DbType.Boolean;
				colvarIsVitaMix.MaxLength = 0;
				colvarIsVitaMix.AutoIncrement = false;
				colvarIsVitaMix.IsNullable = true;
				colvarIsVitaMix.IsPrimaryKey = false;
				colvarIsVitaMix.IsForeignKey = false;
				colvarIsVitaMix.IsReadOnly = false;
				colvarIsVitaMix.DefaultSetting = @"";
				colvarIsVitaMix.ForeignKeyTableName = "";
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
				colvarIsWaterFilter.DefaultSetting = @"";
				colvarIsWaterFilter.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIsWaterFilter);
				
				TableSchema.TableColumn colvarIsJuicePlus = new TableSchema.TableColumn(schema);
				colvarIsJuicePlus.ColumnName = "IsJuicePlus";
				colvarIsJuicePlus.DataType = DbType.Boolean;
				colvarIsJuicePlus.MaxLength = 0;
				colvarIsJuicePlus.AutoIncrement = false;
				colvarIsJuicePlus.IsNullable = true;
				colvarIsJuicePlus.IsPrimaryKey = false;
				colvarIsJuicePlus.IsForeignKey = false;
				colvarIsJuicePlus.IsReadOnly = false;
				colvarIsJuicePlus.DefaultSetting = @"";
				colvarIsJuicePlus.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIsJuicePlus);
				
				TableSchema.TableColumn colvarIsYoung = new TableSchema.TableColumn(schema);
				colvarIsYoung.ColumnName = "IsYoung";
				colvarIsYoung.DataType = DbType.Boolean;
				colvarIsYoung.MaxLength = 0;
				colvarIsYoung.AutoIncrement = false;
				colvarIsYoung.IsNullable = true;
				colvarIsYoung.IsPrimaryKey = false;
				colvarIsYoung.IsForeignKey = false;
				colvarIsYoung.IsReadOnly = false;
				colvarIsYoung.DefaultSetting = @"";
				colvarIsYoung.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIsYoung);
				
				TableSchema.TableColumn colvarSalesPersonID = new TableSchema.TableColumn(schema);
				colvarSalesPersonID.ColumnName = "SalesPersonID";
				colvarSalesPersonID.DataType = DbType.AnsiString;
				colvarSalesPersonID.MaxLength = 50;
				colvarSalesPersonID.AutoIncrement = false;
				colvarSalesPersonID.IsNullable = true;
				colvarSalesPersonID.IsPrimaryKey = false;
				colvarSalesPersonID.IsForeignKey = true;
				colvarSalesPersonID.IsReadOnly = false;
				colvarSalesPersonID.DefaultSetting = @"";
				
					colvarSalesPersonID.ForeignKeyTableName = "UserMst";
				schema.Columns.Add(colvarSalesPersonID);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["PowerPOS"].AddSchema("Membership",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("MembershipNo")]
		[Bindable(true)]
		public string MembershipNo 
		{
			get { return GetColumnValue<string>(Columns.MembershipNo); }
			set { SetColumnValue(Columns.MembershipNo, value); }
		}
		  
		[XmlAttribute("MembershipGroupId")]
		[Bindable(true)]
		public int MembershipGroupId 
		{
			get { return GetColumnValue<int>(Columns.MembershipGroupId); }
			set { SetColumnValue(Columns.MembershipGroupId, value); }
		}
		  
		[XmlAttribute("Title")]
		[Bindable(true)]
		public string Title 
		{
			get { return GetColumnValue<string>(Columns.Title); }
			set { SetColumnValue(Columns.Title, value); }
		}
		  
		[XmlAttribute("LastName")]
		[Bindable(true)]
		public string LastName 
		{
			get { return GetColumnValue<string>(Columns.LastName); }
			set { SetColumnValue(Columns.LastName, value); }
		}
		  
		[XmlAttribute("FirstName")]
		[Bindable(true)]
		public string FirstName 
		{
			get { return GetColumnValue<string>(Columns.FirstName); }
			set { SetColumnValue(Columns.FirstName, value); }
		}
		  
		[XmlAttribute("ChristianName")]
		[Bindable(true)]
		public string ChristianName 
		{
			get { return GetColumnValue<string>(Columns.ChristianName); }
			set { SetColumnValue(Columns.ChristianName, value); }
		}
		  
		[XmlAttribute("ChineseName")]
		[Bindable(true)]
		public string ChineseName 
		{
			get { return GetColumnValue<string>(Columns.ChineseName); }
			set { SetColumnValue(Columns.ChineseName, value); }
		}
		  
		[XmlAttribute("NameToAppear")]
		[Bindable(true)]
		public string NameToAppear 
		{
			get { return GetColumnValue<string>(Columns.NameToAppear); }
			set { SetColumnValue(Columns.NameToAppear, value); }
		}
		  
		[XmlAttribute("Gender")]
		[Bindable(true)]
		public string Gender 
		{
			get { return GetColumnValue<string>(Columns.Gender); }
			set { SetColumnValue(Columns.Gender, value); }
		}
		  
		[XmlAttribute("DateOfBirth")]
		[Bindable(true)]
		public DateTime? DateOfBirth 
		{
			get { return GetColumnValue<DateTime?>(Columns.DateOfBirth); }
			set { SetColumnValue(Columns.DateOfBirth, value); }
		}
		  
		[XmlAttribute("Nationality")]
		[Bindable(true)]
		public string Nationality 
		{
			get { return GetColumnValue<string>(Columns.Nationality); }
			set { SetColumnValue(Columns.Nationality, value); }
		}
		  
		[XmlAttribute("Nric")]
		[Bindable(true)]
		public string Nric 
		{
			get { return GetColumnValue<string>(Columns.Nric); }
			set { SetColumnValue(Columns.Nric, value); }
		}
		  
		[XmlAttribute("Occupation")]
		[Bindable(true)]
		public string Occupation 
		{
			get { return GetColumnValue<string>(Columns.Occupation); }
			set { SetColumnValue(Columns.Occupation, value); }
		}
		  
		[XmlAttribute("MaritalStatus")]
		[Bindable(true)]
		public string MaritalStatus 
		{
			get { return GetColumnValue<string>(Columns.MaritalStatus); }
			set { SetColumnValue(Columns.MaritalStatus, value); }
		}
		  
		[XmlAttribute("Email")]
		[Bindable(true)]
		public string Email 
		{
			get { return GetColumnValue<string>(Columns.Email); }
			set { SetColumnValue(Columns.Email, value); }
		}
		  
		[XmlAttribute("Block")]
		[Bindable(true)]
		public string Block 
		{
			get { return GetColumnValue<string>(Columns.Block); }
			set { SetColumnValue(Columns.Block, value); }
		}
		  
		[XmlAttribute("BuildingName")]
		[Bindable(true)]
		public string BuildingName 
		{
			get { return GetColumnValue<string>(Columns.BuildingName); }
			set { SetColumnValue(Columns.BuildingName, value); }
		}
		  
		[XmlAttribute("StreetName")]
		[Bindable(true)]
		public string StreetName 
		{
			get { return GetColumnValue<string>(Columns.StreetName); }
			set { SetColumnValue(Columns.StreetName, value); }
		}
		  
		[XmlAttribute("StreetName2")]
		[Bindable(true)]
		public string StreetName2 
		{
			get { return GetColumnValue<string>(Columns.StreetName2); }
			set { SetColumnValue(Columns.StreetName2, value); }
		}
		  
		[XmlAttribute("UnitNo")]
		[Bindable(true)]
		public string UnitNo 
		{
			get { return GetColumnValue<string>(Columns.UnitNo); }
			set { SetColumnValue(Columns.UnitNo, value); }
		}
		  
		[XmlAttribute("City")]
		[Bindable(true)]
		public string City 
		{
			get { return GetColumnValue<string>(Columns.City); }
			set { SetColumnValue(Columns.City, value); }
		}
		  
		[XmlAttribute("Country")]
		[Bindable(true)]
		public string Country 
		{
			get { return GetColumnValue<string>(Columns.Country); }
			set { SetColumnValue(Columns.Country, value); }
		}
		  
		[XmlAttribute("ZipCode")]
		[Bindable(true)]
		public string ZipCode 
		{
			get { return GetColumnValue<string>(Columns.ZipCode); }
			set { SetColumnValue(Columns.ZipCode, value); }
		}
		  
		[XmlAttribute("Mobile")]
		[Bindable(true)]
		public string Mobile 
		{
			get { return GetColumnValue<string>(Columns.Mobile); }
			set { SetColumnValue(Columns.Mobile, value); }
		}
		  
		[XmlAttribute("Office")]
		[Bindable(true)]
		public string Office 
		{
			get { return GetColumnValue<string>(Columns.Office); }
			set { SetColumnValue(Columns.Office, value); }
		}
		  
		[XmlAttribute("Fax")]
		[Bindable(true)]
		public string Fax 
		{
			get { return GetColumnValue<string>(Columns.Fax); }
			set { SetColumnValue(Columns.Fax, value); }
		}
		  
		[XmlAttribute("Home")]
		[Bindable(true)]
		public string Home 
		{
			get { return GetColumnValue<string>(Columns.Home); }
			set { SetColumnValue(Columns.Home, value); }
		}
		  
		[XmlAttribute("ExpiryDate")]
		[Bindable(true)]
		public DateTime? ExpiryDate 
		{
			get { return GetColumnValue<DateTime?>(Columns.ExpiryDate); }
			set { SetColumnValue(Columns.ExpiryDate, value); }
		}
		  
		[XmlAttribute("Remarks")]
		[Bindable(true)]
		public string Remarks 
		{
			get { return GetColumnValue<string>(Columns.Remarks); }
			set { SetColumnValue(Columns.Remarks, value); }
		}
		  
		[XmlAttribute("SubscriptionDate")]
		[Bindable(true)]
		public DateTime? SubscriptionDate 
		{
			get { return GetColumnValue<DateTime?>(Columns.SubscriptionDate); }
			set { SetColumnValue(Columns.SubscriptionDate, value); }
		}
		  
		[XmlAttribute("IsChc")]
		[Bindable(true)]
		public bool? IsChc 
		{
			get { return GetColumnValue<bool?>(Columns.IsChc); }
			set { SetColumnValue(Columns.IsChc, value); }
		}
		  
		[XmlAttribute("Ministry")]
		[Bindable(true)]
		public string Ministry 
		{
			get { return GetColumnValue<string>(Columns.Ministry); }
			set { SetColumnValue(Columns.Ministry, value); }
		}
		  
		[XmlAttribute("IsStudentCard")]
		[Bindable(true)]
		public bool? IsStudentCard 
		{
			get { return GetColumnValue<bool?>(Columns.IsStudentCard); }
			set { SetColumnValue(Columns.IsStudentCard, value); }
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
		  
		[XmlAttribute("Deleted")]
		[Bindable(true)]
		public bool? Deleted 
		{
			get { return GetColumnValue<bool?>(Columns.Deleted); }
			set { SetColumnValue(Columns.Deleted, value); }
		}
		  
		[XmlAttribute("UniqueID")]
		[Bindable(true)]
		public Guid UniqueID 
		{
			get { return GetColumnValue<Guid>(Columns.UniqueID); }
			set { SetColumnValue(Columns.UniqueID, value); }
		}
		  
		[XmlAttribute("Userfld1")]
		[Bindable(true)]
		public string Userfld1 
		{
			get { return GetColumnValue<string>(Columns.Userfld1); }
			set { SetColumnValue(Columns.Userfld1, value); }
		}
		  
		[XmlAttribute("Userfld2")]
		[Bindable(true)]
		public string Userfld2 
		{
			get { return GetColumnValue<string>(Columns.Userfld2); }
			set { SetColumnValue(Columns.Userfld2, value); }
		}
		  
		[XmlAttribute("Userfld3")]
		[Bindable(true)]
		public string Userfld3 
		{
			get { return GetColumnValue<string>(Columns.Userfld3); }
			set { SetColumnValue(Columns.Userfld3, value); }
		}
		  
		[XmlAttribute("Userfld4")]
		[Bindable(true)]
		public string Userfld4 
		{
			get { return GetColumnValue<string>(Columns.Userfld4); }
			set { SetColumnValue(Columns.Userfld4, value); }
		}
		  
		[XmlAttribute("Userfld5")]
		[Bindable(true)]
		public string Userfld5 
		{
			get { return GetColumnValue<string>(Columns.Userfld5); }
			set { SetColumnValue(Columns.Userfld5, value); }
		}
		  
		[XmlAttribute("Userfld6")]
		[Bindable(true)]
		public string Userfld6 
		{
			get { return GetColumnValue<string>(Columns.Userfld6); }
			set { SetColumnValue(Columns.Userfld6, value); }
		}
		  
		[XmlAttribute("Userfld7")]
		[Bindable(true)]
		public string Userfld7 
		{
			get { return GetColumnValue<string>(Columns.Userfld7); }
			set { SetColumnValue(Columns.Userfld7, value); }
		}
		  
		[XmlAttribute("Userfld8")]
		[Bindable(true)]
		public string Userfld8 
		{
			get { return GetColumnValue<string>(Columns.Userfld8); }
			set { SetColumnValue(Columns.Userfld8, value); }
		}
		  
		[XmlAttribute("Userfld9")]
		[Bindable(true)]
		public string Userfld9 
		{
			get { return GetColumnValue<string>(Columns.Userfld9); }
			set { SetColumnValue(Columns.Userfld9, value); }
		}
		  
		[XmlAttribute("Userfld10")]
		[Bindable(true)]
		public string Userfld10 
		{
			get { return GetColumnValue<string>(Columns.Userfld10); }
			set { SetColumnValue(Columns.Userfld10, value); }
		}
		  
		[XmlAttribute("Userflag1")]
		[Bindable(true)]
		public bool? Userflag1 
		{
			get { return GetColumnValue<bool?>(Columns.Userflag1); }
			set { SetColumnValue(Columns.Userflag1, value); }
		}
		  
		[XmlAttribute("Userflag2")]
		[Bindable(true)]
		public bool? Userflag2 
		{
			get { return GetColumnValue<bool?>(Columns.Userflag2); }
			set { SetColumnValue(Columns.Userflag2, value); }
		}
		  
		[XmlAttribute("Userflag3")]
		[Bindable(true)]
		public bool? Userflag3 
		{
			get { return GetColumnValue<bool?>(Columns.Userflag3); }
			set { SetColumnValue(Columns.Userflag3, value); }
		}
		  
		[XmlAttribute("Userflag4")]
		[Bindable(true)]
		public bool? Userflag4 
		{
			get { return GetColumnValue<bool?>(Columns.Userflag4); }
			set { SetColumnValue(Columns.Userflag4, value); }
		}
		  
		[XmlAttribute("Userflag5")]
		[Bindable(true)]
		public bool? Userflag5 
		{
			get { return GetColumnValue<bool?>(Columns.Userflag5); }
			set { SetColumnValue(Columns.Userflag5, value); }
		}
		  
		[XmlAttribute("Userfloat1")]
		[Bindable(true)]
		public decimal? Userfloat1 
		{
			get { return GetColumnValue<decimal?>(Columns.Userfloat1); }
			set { SetColumnValue(Columns.Userfloat1, value); }
		}
		  
		[XmlAttribute("Userfloat2")]
		[Bindable(true)]
		public decimal? Userfloat2 
		{
			get { return GetColumnValue<decimal?>(Columns.Userfloat2); }
			set { SetColumnValue(Columns.Userfloat2, value); }
		}
		  
		[XmlAttribute("Userfloat3")]
		[Bindable(true)]
		public decimal? Userfloat3 
		{
			get { return GetColumnValue<decimal?>(Columns.Userfloat3); }
			set { SetColumnValue(Columns.Userfloat3, value); }
		}
		  
		[XmlAttribute("Userfloat4")]
		[Bindable(true)]
		public decimal? Userfloat4 
		{
			get { return GetColumnValue<decimal?>(Columns.Userfloat4); }
			set { SetColumnValue(Columns.Userfloat4, value); }
		}
		  
		[XmlAttribute("Userfloat5")]
		[Bindable(true)]
		public decimal? Userfloat5 
		{
			get { return GetColumnValue<decimal?>(Columns.Userfloat5); }
			set { SetColumnValue(Columns.Userfloat5, value); }
		}
		  
		[XmlAttribute("Userint1")]
		[Bindable(true)]
		public int? Userint1 
		{
			get { return GetColumnValue<int?>(Columns.Userint1); }
			set { SetColumnValue(Columns.Userint1, value); }
		}
		  
		[XmlAttribute("Userint2")]
		[Bindable(true)]
		public int? Userint2 
		{
			get { return GetColumnValue<int?>(Columns.Userint2); }
			set { SetColumnValue(Columns.Userint2, value); }
		}
		  
		[XmlAttribute("Userint3")]
		[Bindable(true)]
		public int? Userint3 
		{
			get { return GetColumnValue<int?>(Columns.Userint3); }
			set { SetColumnValue(Columns.Userint3, value); }
		}
		  
		[XmlAttribute("Userint4")]
		[Bindable(true)]
		public int? Userint4 
		{
			get { return GetColumnValue<int?>(Columns.Userint4); }
			set { SetColumnValue(Columns.Userint4, value); }
		}
		  
		[XmlAttribute("Userint5")]
		[Bindable(true)]
		public int? Userint5 
		{
			get { return GetColumnValue<int?>(Columns.Userint5); }
			set { SetColumnValue(Columns.Userint5, value); }
		}
		  
		[XmlAttribute("IsVitaMix")]
		[Bindable(true)]
		public bool? IsVitaMix 
		{
			get { return GetColumnValue<bool?>(Columns.IsVitaMix); }
			set { SetColumnValue(Columns.IsVitaMix, value); }
		}
		  
		[XmlAttribute("IsWaterFilter")]
		[Bindable(true)]
		public bool? IsWaterFilter 
		{
			get { return GetColumnValue<bool?>(Columns.IsWaterFilter); }
			set { SetColumnValue(Columns.IsWaterFilter, value); }
		}
		  
		[XmlAttribute("IsJuicePlus")]
		[Bindable(true)]
		public bool? IsJuicePlus 
		{
			get { return GetColumnValue<bool?>(Columns.IsJuicePlus); }
			set { SetColumnValue(Columns.IsJuicePlus, value); }
		}
		  
		[XmlAttribute("IsYoung")]
		[Bindable(true)]
		public bool? IsYoung 
		{
			get { return GetColumnValue<bool?>(Columns.IsYoung); }
			set { SetColumnValue(Columns.IsYoung, value); }
		}
		  
		[XmlAttribute("SalesPersonID")]
		[Bindable(true)]
		public string SalesPersonID 
		{
			get { return GetColumnValue<string>(Columns.SalesPersonID); }
			set { SetColumnValue(Columns.SalesPersonID, value); }
		}
		
		#endregion
		
		
		#region PrimaryKey Methods		
		
        protected override void SetPrimaryKey(object oValue)
        {
            base.SetPrimaryKey(oValue);
            
            SetPKValues();
        }
        
		
		public PowerPOS.AppointmentCollection AppointmentRecords()
		{
			return new PowerPOS.AppointmentCollection().Where(Appointment.Columns.MembershipNo, MembershipNo).Load();
		}
		public PowerPOS.DeliveryOrderCollection DeliveryOrderRecords()
		{
			return new PowerPOS.DeliveryOrderCollection().Where(DeliveryOrder.Columns.MembershipNo, MembershipNo).Load();
		}
		public PowerPOS.EventAttendanceCollection EventAttendanceRecords()
		{
			return new PowerPOS.EventAttendanceCollection().Where(EventAttendance.Columns.MembershipNo, MembershipNo).Load();
		}
		public PowerPOS.MembershipRemarkCollection MembershipRemarkRecords()
		{
			return new PowerPOS.MembershipRemarkCollection().Where(MembershipRemark.Columns.MembershipNo, MembershipNo).Load();
		}
		#endregion
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a MembershipGroup ActiveRecord object related to this Membership
		/// 
		/// </summary>
		public PowerPOS.MembershipGroup MembershipGroup
		{
			get { return PowerPOS.MembershipGroup.FetchByID(this.MembershipGroupId); }
			set { SetColumnValue("MembershipGroupId", value.MembershipGroupId); }
		}
		
		
		/// <summary>
		/// Returns a UserMst ActiveRecord object related to this Membership
		/// 
		/// </summary>
		public PowerPOS.UserMst UserMst
		{
			get { return PowerPOS.UserMst.FetchByID(this.SalesPersonID); }
			set { SetColumnValue("SalesPersonID", value.UserName); }
		}
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(string varMembershipNo,int varMembershipGroupId,string varTitle,string varLastName,string varFirstName,string varChristianName,string varChineseName,string varNameToAppear,string varGender,DateTime? varDateOfBirth,string varNationality,string varNric,string varOccupation,string varMaritalStatus,string varEmail,string varBlock,string varBuildingName,string varStreetName,string varStreetName2,string varUnitNo,string varCity,string varCountry,string varZipCode,string varMobile,string varOffice,string varFax,string varHome,DateTime? varExpiryDate,string varRemarks,DateTime? varSubscriptionDate,bool? varIsChc,string varMinistry,bool? varIsStudentCard,DateTime? varCreatedOn,string varCreatedBy,DateTime? varModifiedOn,string varModifiedBy,bool? varDeleted,Guid varUniqueID,string varUserfld1,string varUserfld2,string varUserfld3,string varUserfld4,string varUserfld5,string varUserfld6,string varUserfld7,string varUserfld8,string varUserfld9,string varUserfld10,bool? varUserflag1,bool? varUserflag2,bool? varUserflag3,bool? varUserflag4,bool? varUserflag5,decimal? varUserfloat1,decimal? varUserfloat2,decimal? varUserfloat3,decimal? varUserfloat4,decimal? varUserfloat5,int? varUserint1,int? varUserint2,int? varUserint3,int? varUserint4,int? varUserint5,bool? varIsVitaMix,bool? varIsWaterFilter,bool? varIsJuicePlus,bool? varIsYoung,string varSalesPersonID)
		{
			Membership item = new Membership();
			
			item.MembershipNo = varMembershipNo;
			
			item.MembershipGroupId = varMembershipGroupId;
			
			item.Title = varTitle;
			
			item.LastName = varLastName;
			
			item.FirstName = varFirstName;
			
			item.ChristianName = varChristianName;
			
			item.ChineseName = varChineseName;
			
			item.NameToAppear = varNameToAppear;
			
			item.Gender = varGender;
			
			item.DateOfBirth = varDateOfBirth;
			
			item.Nationality = varNationality;
			
			item.Nric = varNric;
			
			item.Occupation = varOccupation;
			
			item.MaritalStatus = varMaritalStatus;
			
			item.Email = varEmail;
			
			item.Block = varBlock;
			
			item.BuildingName = varBuildingName;
			
			item.StreetName = varStreetName;
			
			item.StreetName2 = varStreetName2;
			
			item.UnitNo = varUnitNo;
			
			item.City = varCity;
			
			item.Country = varCountry;
			
			item.ZipCode = varZipCode;
			
			item.Mobile = varMobile;
			
			item.Office = varOffice;
			
			item.Fax = varFax;
			
			item.Home = varHome;
			
			item.ExpiryDate = varExpiryDate;
			
			item.Remarks = varRemarks;
			
			item.SubscriptionDate = varSubscriptionDate;
			
			item.IsChc = varIsChc;
			
			item.Ministry = varMinistry;
			
			item.IsStudentCard = varIsStudentCard;
			
			item.CreatedOn = varCreatedOn;
			
			item.CreatedBy = varCreatedBy;
			
			item.ModifiedOn = varModifiedOn;
			
			item.ModifiedBy = varModifiedBy;
			
			item.Deleted = varDeleted;
			
			item.UniqueID = varUniqueID;
			
			item.Userfld1 = varUserfld1;
			
			item.Userfld2 = varUserfld2;
			
			item.Userfld3 = varUserfld3;
			
			item.Userfld4 = varUserfld4;
			
			item.Userfld5 = varUserfld5;
			
			item.Userfld6 = varUserfld6;
			
			item.Userfld7 = varUserfld7;
			
			item.Userfld8 = varUserfld8;
			
			item.Userfld9 = varUserfld9;
			
			item.Userfld10 = varUserfld10;
			
			item.Userflag1 = varUserflag1;
			
			item.Userflag2 = varUserflag2;
			
			item.Userflag3 = varUserflag3;
			
			item.Userflag4 = varUserflag4;
			
			item.Userflag5 = varUserflag5;
			
			item.Userfloat1 = varUserfloat1;
			
			item.Userfloat2 = varUserfloat2;
			
			item.Userfloat3 = varUserfloat3;
			
			item.Userfloat4 = varUserfloat4;
			
			item.Userfloat5 = varUserfloat5;
			
			item.Userint1 = varUserint1;
			
			item.Userint2 = varUserint2;
			
			item.Userint3 = varUserint3;
			
			item.Userint4 = varUserint4;
			
			item.Userint5 = varUserint5;
			
			item.IsVitaMix = varIsVitaMix;
			
			item.IsWaterFilter = varIsWaterFilter;
			
			item.IsJuicePlus = varIsJuicePlus;
			
			item.IsYoung = varIsYoung;
			
			item.SalesPersonID = varSalesPersonID;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(string varMembershipNo,int varMembershipGroupId,string varTitle,string varLastName,string varFirstName,string varChristianName,string varChineseName,string varNameToAppear,string varGender,DateTime? varDateOfBirth,string varNationality,string varNric,string varOccupation,string varMaritalStatus,string varEmail,string varBlock,string varBuildingName,string varStreetName,string varStreetName2,string varUnitNo,string varCity,string varCountry,string varZipCode,string varMobile,string varOffice,string varFax,string varHome,DateTime? varExpiryDate,string varRemarks,DateTime? varSubscriptionDate,bool? varIsChc,string varMinistry,bool? varIsStudentCard,DateTime? varCreatedOn,string varCreatedBy,DateTime? varModifiedOn,string varModifiedBy,bool? varDeleted,Guid varUniqueID,string varUserfld1,string varUserfld2,string varUserfld3,string varUserfld4,string varUserfld5,string varUserfld6,string varUserfld7,string varUserfld8,string varUserfld9,string varUserfld10,bool? varUserflag1,bool? varUserflag2,bool? varUserflag3,bool? varUserflag4,bool? varUserflag5,decimal? varUserfloat1,decimal? varUserfloat2,decimal? varUserfloat3,decimal? varUserfloat4,decimal? varUserfloat5,int? varUserint1,int? varUserint2,int? varUserint3,int? varUserint4,int? varUserint5,bool? varIsVitaMix,bool? varIsWaterFilter,bool? varIsJuicePlus,bool? varIsYoung,string varSalesPersonID)
		{
			Membership item = new Membership();
			
				item.MembershipNo = varMembershipNo;
			
				item.MembershipGroupId = varMembershipGroupId;
			
				item.Title = varTitle;
			
				item.LastName = varLastName;
			
				item.FirstName = varFirstName;
			
				item.ChristianName = varChristianName;
			
				item.ChineseName = varChineseName;
			
				item.NameToAppear = varNameToAppear;
			
				item.Gender = varGender;
			
				item.DateOfBirth = varDateOfBirth;
			
				item.Nationality = varNationality;
			
				item.Nric = varNric;
			
				item.Occupation = varOccupation;
			
				item.MaritalStatus = varMaritalStatus;
			
				item.Email = varEmail;
			
				item.Block = varBlock;
			
				item.BuildingName = varBuildingName;
			
				item.StreetName = varStreetName;
			
				item.StreetName2 = varStreetName2;
			
				item.UnitNo = varUnitNo;
			
				item.City = varCity;
			
				item.Country = varCountry;
			
				item.ZipCode = varZipCode;
			
				item.Mobile = varMobile;
			
				item.Office = varOffice;
			
				item.Fax = varFax;
			
				item.Home = varHome;
			
				item.ExpiryDate = varExpiryDate;
			
				item.Remarks = varRemarks;
			
				item.SubscriptionDate = varSubscriptionDate;
			
				item.IsChc = varIsChc;
			
				item.Ministry = varMinistry;
			
				item.IsStudentCard = varIsStudentCard;
			
				item.CreatedOn = varCreatedOn;
			
				item.CreatedBy = varCreatedBy;
			
				item.ModifiedOn = varModifiedOn;
			
				item.ModifiedBy = varModifiedBy;
			
				item.Deleted = varDeleted;
			
				item.UniqueID = varUniqueID;
			
				item.Userfld1 = varUserfld1;
			
				item.Userfld2 = varUserfld2;
			
				item.Userfld3 = varUserfld3;
			
				item.Userfld4 = varUserfld4;
			
				item.Userfld5 = varUserfld5;
			
				item.Userfld6 = varUserfld6;
			
				item.Userfld7 = varUserfld7;
			
				item.Userfld8 = varUserfld8;
			
				item.Userfld9 = varUserfld9;
			
				item.Userfld10 = varUserfld10;
			
				item.Userflag1 = varUserflag1;
			
				item.Userflag2 = varUserflag2;
			
				item.Userflag3 = varUserflag3;
			
				item.Userflag4 = varUserflag4;
			
				item.Userflag5 = varUserflag5;
			
				item.Userfloat1 = varUserfloat1;
			
				item.Userfloat2 = varUserfloat2;
			
				item.Userfloat3 = varUserfloat3;
			
				item.Userfloat4 = varUserfloat4;
			
				item.Userfloat5 = varUserfloat5;
			
				item.Userint1 = varUserint1;
			
				item.Userint2 = varUserint2;
			
				item.Userint3 = varUserint3;
			
				item.Userint4 = varUserint4;
			
				item.Userint5 = varUserint5;
			
				item.IsVitaMix = varIsVitaMix;
			
				item.IsWaterFilter = varIsWaterFilter;
			
				item.IsJuicePlus = varIsJuicePlus;
			
				item.IsYoung = varIsYoung;
			
				item.SalesPersonID = varSalesPersonID;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn MembershipNoColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn MembershipGroupIdColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn TitleColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn LastNameColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn FirstNameColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn ChristianNameColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn ChineseNameColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn NameToAppearColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn GenderColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn DateOfBirthColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn NationalityColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        public static TableSchema.TableColumn NricColumn
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        public static TableSchema.TableColumn OccupationColumn
        {
            get { return Schema.Columns[12]; }
        }
        
        
        
        public static TableSchema.TableColumn MaritalStatusColumn
        {
            get { return Schema.Columns[13]; }
        }
        
        
        
        public static TableSchema.TableColumn EmailColumn
        {
            get { return Schema.Columns[14]; }
        }
        
        
        
        public static TableSchema.TableColumn BlockColumn
        {
            get { return Schema.Columns[15]; }
        }
        
        
        
        public static TableSchema.TableColumn BuildingNameColumn
        {
            get { return Schema.Columns[16]; }
        }
        
        
        
        public static TableSchema.TableColumn StreetNameColumn
        {
            get { return Schema.Columns[17]; }
        }
        
        
        
        public static TableSchema.TableColumn StreetName2Column
        {
            get { return Schema.Columns[18]; }
        }
        
        
        
        public static TableSchema.TableColumn UnitNoColumn
        {
            get { return Schema.Columns[19]; }
        }
        
        
        
        public static TableSchema.TableColumn CityColumn
        {
            get { return Schema.Columns[20]; }
        }
        
        
        
        public static TableSchema.TableColumn CountryColumn
        {
            get { return Schema.Columns[21]; }
        }
        
        
        
        public static TableSchema.TableColumn ZipCodeColumn
        {
            get { return Schema.Columns[22]; }
        }
        
        
        
        public static TableSchema.TableColumn MobileColumn
        {
            get { return Schema.Columns[23]; }
        }
        
        
        
        public static TableSchema.TableColumn OfficeColumn
        {
            get { return Schema.Columns[24]; }
        }
        
        
        
        public static TableSchema.TableColumn FaxColumn
        {
            get { return Schema.Columns[25]; }
        }
        
        
        
        public static TableSchema.TableColumn HomeColumn
        {
            get { return Schema.Columns[26]; }
        }
        
        
        
        public static TableSchema.TableColumn ExpiryDateColumn
        {
            get { return Schema.Columns[27]; }
        }
        
        
        
        public static TableSchema.TableColumn RemarksColumn
        {
            get { return Schema.Columns[28]; }
        }
        
        
        
        public static TableSchema.TableColumn SubscriptionDateColumn
        {
            get { return Schema.Columns[29]; }
        }
        
        
        
        public static TableSchema.TableColumn IsChcColumn
        {
            get { return Schema.Columns[30]; }
        }
        
        
        
        public static TableSchema.TableColumn MinistryColumn
        {
            get { return Schema.Columns[31]; }
        }
        
        
        
        public static TableSchema.TableColumn IsStudentCardColumn
        {
            get { return Schema.Columns[32]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedOnColumn
        {
            get { return Schema.Columns[33]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedByColumn
        {
            get { return Schema.Columns[34]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedOnColumn
        {
            get { return Schema.Columns[35]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedByColumn
        {
            get { return Schema.Columns[36]; }
        }
        
        
        
        public static TableSchema.TableColumn DeletedColumn
        {
            get { return Schema.Columns[37]; }
        }
        
        
        
        public static TableSchema.TableColumn UniqueIDColumn
        {
            get { return Schema.Columns[38]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld1Column
        {
            get { return Schema.Columns[39]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld2Column
        {
            get { return Schema.Columns[40]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld3Column
        {
            get { return Schema.Columns[41]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld4Column
        {
            get { return Schema.Columns[42]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld5Column
        {
            get { return Schema.Columns[43]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld6Column
        {
            get { return Schema.Columns[44]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld7Column
        {
            get { return Schema.Columns[45]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld8Column
        {
            get { return Schema.Columns[46]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld9Column
        {
            get { return Schema.Columns[47]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld10Column
        {
            get { return Schema.Columns[48]; }
        }
        
        
        
        public static TableSchema.TableColumn Userflag1Column
        {
            get { return Schema.Columns[49]; }
        }
        
        
        
        public static TableSchema.TableColumn Userflag2Column
        {
            get { return Schema.Columns[50]; }
        }
        
        
        
        public static TableSchema.TableColumn Userflag3Column
        {
            get { return Schema.Columns[51]; }
        }
        
        
        
        public static TableSchema.TableColumn Userflag4Column
        {
            get { return Schema.Columns[52]; }
        }
        
        
        
        public static TableSchema.TableColumn Userflag5Column
        {
            get { return Schema.Columns[53]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfloat1Column
        {
            get { return Schema.Columns[54]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfloat2Column
        {
            get { return Schema.Columns[55]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfloat3Column
        {
            get { return Schema.Columns[56]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfloat4Column
        {
            get { return Schema.Columns[57]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfloat5Column
        {
            get { return Schema.Columns[58]; }
        }
        
        
        
        public static TableSchema.TableColumn Userint1Column
        {
            get { return Schema.Columns[59]; }
        }
        
        
        
        public static TableSchema.TableColumn Userint2Column
        {
            get { return Schema.Columns[60]; }
        }
        
        
        
        public static TableSchema.TableColumn Userint3Column
        {
            get { return Schema.Columns[61]; }
        }
        
        
        
        public static TableSchema.TableColumn Userint4Column
        {
            get { return Schema.Columns[62]; }
        }
        
        
        
        public static TableSchema.TableColumn Userint5Column
        {
            get { return Schema.Columns[63]; }
        }
        
        
        
        public static TableSchema.TableColumn IsVitaMixColumn
        {
            get { return Schema.Columns[64]; }
        }
        
        
        
        public static TableSchema.TableColumn IsWaterFilterColumn
        {
            get { return Schema.Columns[65]; }
        }
        
        
        
        public static TableSchema.TableColumn IsJuicePlusColumn
        {
            get { return Schema.Columns[66]; }
        }
        
        
        
        public static TableSchema.TableColumn IsYoungColumn
        {
            get { return Schema.Columns[67]; }
        }
        
        
        
        public static TableSchema.TableColumn SalesPersonIDColumn
        {
            get { return Schema.Columns[68]; }
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
			 public static string ChineseName = @"ChineseName";
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
			 public static string StreetName2 = @"StreetName2";
			 public static string UnitNo = @"UnitNo";
			 public static string City = @"City";
			 public static string Country = @"Country";
			 public static string ZipCode = @"ZipCode";
			 public static string Mobile = @"Mobile";
			 public static string Office = @"Office";
			 public static string Fax = @"Fax";
			 public static string Home = @"Home";
			 public static string ExpiryDate = @"ExpiryDate";
			 public static string Remarks = @"Remarks";
			 public static string SubscriptionDate = @"SubscriptionDate";
			 public static string IsChc = @"IsChc";
			 public static string Ministry = @"Ministry";
			 public static string IsStudentCard = @"IsStudentCard";
			 public static string CreatedOn = @"CreatedOn";
			 public static string CreatedBy = @"CreatedBy";
			 public static string ModifiedOn = @"ModifiedOn";
			 public static string ModifiedBy = @"ModifiedBy";
			 public static string Deleted = @"Deleted";
			 public static string UniqueID = @"UniqueID";
			 public static string Userfld1 = @"userfld1";
			 public static string Userfld2 = @"userfld2";
			 public static string Userfld3 = @"userfld3";
			 public static string Userfld4 = @"userfld4";
			 public static string Userfld5 = @"userfld5";
			 public static string Userfld6 = @"userfld6";
			 public static string Userfld7 = @"userfld7";
			 public static string Userfld8 = @"userfld8";
			 public static string Userfld9 = @"userfld9";
			 public static string Userfld10 = @"userfld10";
			 public static string Userflag1 = @"userflag1";
			 public static string Userflag2 = @"userflag2";
			 public static string Userflag3 = @"userflag3";
			 public static string Userflag4 = @"userflag4";
			 public static string Userflag5 = @"userflag5";
			 public static string Userfloat1 = @"userfloat1";
			 public static string Userfloat2 = @"userfloat2";
			 public static string Userfloat3 = @"userfloat3";
			 public static string Userfloat4 = @"userfloat4";
			 public static string Userfloat5 = @"userfloat5";
			 public static string Userint1 = @"userint1";
			 public static string Userint2 = @"userint2";
			 public static string Userint3 = @"userint3";
			 public static string Userint4 = @"userint4";
			 public static string Userint5 = @"userint5";
			 public static string IsVitaMix = @"IsVitaMix";
			 public static string IsWaterFilter = @"IsWaterFilter";
			 public static string IsJuicePlus = @"IsJuicePlus";
			 public static string IsYoung = @"IsYoung";
			 public static string SalesPersonID = @"SalesPersonID";
						
		}
		#endregion
		
		#region Update PK Collections
		
        public void SetPKValues()
        {
}
        #endregion
    
        #region Deep Save
		
        public void DeepSave()
        {
            Save();
            
}
        #endregion
	}
}
