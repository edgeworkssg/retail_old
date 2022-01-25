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
    /// Strongly-typed collection for the ViewMembership class.
    /// </summary>
    [Serializable]
    public partial class ViewMembershipCollection : ReadOnlyList<ViewMembership, ViewMembershipCollection>
    {        
        public ViewMembershipCollection() {}
    }
    /// <summary>
    /// This is  Read-only wrapper class for the ViewMembership view.
    /// </summary>
    [Serializable]
    public partial class ViewMembership : ReadOnlyRecord<ViewMembership>, IReadOnlyRecord
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
                TableSchema.Table schema = new TableSchema.Table("ViewMembership", TableType.View, DataService.GetInstance("PowerPOS"));
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
                
                TableSchema.TableColumn colvarFax = new TableSchema.TableColumn(schema);
                colvarFax.ColumnName = "Fax";
                colvarFax.DataType = DbType.AnsiString;
                colvarFax.MaxLength = 50;
                colvarFax.AutoIncrement = false;
                colvarFax.IsNullable = true;
                colvarFax.IsPrimaryKey = false;
                colvarFax.IsForeignKey = false;
                colvarFax.IsReadOnly = false;
                
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
                colvarRemarks.DataType = DbType.String;
                colvarRemarks.MaxLength = -1;
                colvarRemarks.AutoIncrement = false;
                colvarRemarks.IsNullable = true;
                colvarRemarks.IsPrimaryKey = false;
                colvarRemarks.IsForeignKey = false;
                colvarRemarks.IsReadOnly = false;
                
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
                colvarDeleted.IsNullable = true;
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
                
                TableSchema.TableColumn colvarDiscount = new TableSchema.TableColumn(schema);
                colvarDiscount.ColumnName = "Discount";
                colvarDiscount.DataType = DbType.Double;
                colvarDiscount.MaxLength = 0;
                colvarDiscount.AutoIncrement = false;
                colvarDiscount.IsNullable = false;
                colvarDiscount.IsPrimaryKey = false;
                colvarDiscount.IsForeignKey = false;
                colvarDiscount.IsReadOnly = false;
                
                schema.Columns.Add(colvarDiscount);
                
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
                
                TableSchema.TableColumn colvarAddress = new TableSchema.TableColumn(schema);
                colvarAddress.ColumnName = "Address";
                colvarAddress.DataType = DbType.AnsiString;
                colvarAddress.MaxLength = -1;
                colvarAddress.AutoIncrement = false;
                colvarAddress.IsNullable = false;
                colvarAddress.IsPrimaryKey = false;
                colvarAddress.IsForeignKey = false;
                colvarAddress.IsReadOnly = false;
                
                schema.Columns.Add(colvarAddress);
                
                TableSchema.TableColumn colvarBirthDayMonth = new TableSchema.TableColumn(schema);
                colvarBirthDayMonth.ColumnName = "BirthDayMonth";
                colvarBirthDayMonth.DataType = DbType.Int32;
                colvarBirthDayMonth.MaxLength = 0;
                colvarBirthDayMonth.AutoIncrement = false;
                colvarBirthDayMonth.IsNullable = true;
                colvarBirthDayMonth.IsPrimaryKey = false;
                colvarBirthDayMonth.IsForeignKey = false;
                colvarBirthDayMonth.IsReadOnly = false;
                
                schema.Columns.Add(colvarBirthDayMonth);
                
                TableSchema.TableColumn colvarName = new TableSchema.TableColumn(schema);
                colvarName.ColumnName = "Name";
                colvarName.DataType = DbType.String;
                colvarName.MaxLength = 374;
                colvarName.AutoIncrement = false;
                colvarName.IsNullable = false;
                colvarName.IsPrimaryKey = false;
                colvarName.IsForeignKey = false;
                colvarName.IsReadOnly = false;
                
                schema.Columns.Add(colvarName);
                
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
                
                TableSchema.TableColumn colvarSalesPersonID = new TableSchema.TableColumn(schema);
                colvarSalesPersonID.ColumnName = "SalesPersonID";
                colvarSalesPersonID.DataType = DbType.AnsiString;
                colvarSalesPersonID.MaxLength = 50;
                colvarSalesPersonID.AutoIncrement = false;
                colvarSalesPersonID.IsNullable = true;
                colvarSalesPersonID.IsPrimaryKey = false;
                colvarSalesPersonID.IsForeignKey = false;
                colvarSalesPersonID.IsReadOnly = false;
                
                schema.Columns.Add(colvarSalesPersonID);
                
                TableSchema.TableColumn colvarUserfld1 = new TableSchema.TableColumn(schema);
                colvarUserfld1.ColumnName = "userfld1";
                colvarUserfld1.DataType = DbType.String;
                colvarUserfld1.MaxLength = 50;
                colvarUserfld1.AutoIncrement = false;
                colvarUserfld1.IsNullable = true;
                colvarUserfld1.IsPrimaryKey = false;
                colvarUserfld1.IsForeignKey = false;
                colvarUserfld1.IsReadOnly = false;
                
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
                
                schema.Columns.Add(colvarUserfld10);
                
                
                BaseSchema = schema;
                //add this schema to the provider
                //so we can query it later
                DataService.Providers["PowerPOS"].AddSchema("ViewMembership",schema);
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
	    public ViewMembership()
	    {
            SetSQLProps();
            SetDefaults();
            MarkNew();
        }
        public ViewMembership(bool useDatabaseDefaults)
	    {
		    SetSQLProps();
		    if(useDatabaseDefaults)
		    {
				ForceDefaults();
			}
			MarkNew();
	    }
	    
	    public ViewMembership(object keyID)
	    {
		    SetSQLProps();
		    LoadByKey(keyID);
	    }
    	 
	    public ViewMembership(string columnName, object columnValue)
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
	      
        [XmlAttribute("Fax")]
        [Bindable(true)]
        public string Fax 
	    {
		    get
		    {
			    return GetColumnValue<string>("Fax");
		    }
            set 
		    {
			    SetColumnValue("Fax", value);
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
	      
        [XmlAttribute("SubscriptionDate")]
        [Bindable(true)]
        public DateTime? SubscriptionDate 
	    {
		    get
		    {
			    return GetColumnValue<DateTime?>("SubscriptionDate");
		    }
            set 
		    {
			    SetColumnValue("SubscriptionDate", value);
            }
        }
	      
        [XmlAttribute("IsChc")]
        [Bindable(true)]
        public bool? IsChc 
	    {
		    get
		    {
			    return GetColumnValue<bool?>("IsChc");
		    }
            set 
		    {
			    SetColumnValue("IsChc", value);
            }
        }
	      
        [XmlAttribute("Ministry")]
        [Bindable(true)]
        public string Ministry 
	    {
		    get
		    {
			    return GetColumnValue<string>("Ministry");
		    }
            set 
		    {
			    SetColumnValue("Ministry", value);
            }
        }
	      
        [XmlAttribute("IsStudentCard")]
        [Bindable(true)]
        public bool? IsStudentCard 
	    {
		    get
		    {
			    return GetColumnValue<bool?>("IsStudentCard");
		    }
            set 
		    {
			    SetColumnValue("IsStudentCard", value);
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
	      
        [XmlAttribute("Discount")]
        [Bindable(true)]
        public double Discount 
	    {
		    get
		    {
			    return GetColumnValue<double>("Discount");
		    }
            set 
		    {
			    SetColumnValue("Discount", value);
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
	      
        [XmlAttribute("Address")]
        [Bindable(true)]
        public string Address 
	    {
		    get
		    {
			    return GetColumnValue<string>("Address");
		    }
            set 
		    {
			    SetColumnValue("Address", value);
            }
        }
	      
        [XmlAttribute("BirthDayMonth")]
        [Bindable(true)]
        public int? BirthDayMonth 
	    {
		    get
		    {
			    return GetColumnValue<int?>("BirthDayMonth");
		    }
            set 
		    {
			    SetColumnValue("BirthDayMonth", value);
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
	      
        [XmlAttribute("SalesPersonID")]
        [Bindable(true)]
        public string SalesPersonID 
	    {
		    get
		    {
			    return GetColumnValue<string>("SalesPersonID");
		    }
            set 
		    {
			    SetColumnValue("SalesPersonID", value);
            }
        }
	      
        [XmlAttribute("Userfld1")]
        [Bindable(true)]
        public string Userfld1 
	    {
		    get
		    {
			    return GetColumnValue<string>("userfld1");
		    }
            set 
		    {
			    SetColumnValue("userfld1", value);
            }
        }
	      
        [XmlAttribute("Userfld2")]
        [Bindable(true)]
        public string Userfld2 
	    {
		    get
		    {
			    return GetColumnValue<string>("userfld2");
		    }
            set 
		    {
			    SetColumnValue("userfld2", value);
            }
        }
	      
        [XmlAttribute("Userfld3")]
        [Bindable(true)]
        public string Userfld3 
	    {
		    get
		    {
			    return GetColumnValue<string>("userfld3");
		    }
            set 
		    {
			    SetColumnValue("userfld3", value);
            }
        }
	      
        [XmlAttribute("Userfld4")]
        [Bindable(true)]
        public string Userfld4 
	    {
		    get
		    {
			    return GetColumnValue<string>("userfld4");
		    }
            set 
		    {
			    SetColumnValue("userfld4", value);
            }
        }
	      
        [XmlAttribute("Userfld5")]
        [Bindable(true)]
        public string Userfld5 
	    {
		    get
		    {
			    return GetColumnValue<string>("userfld5");
		    }
            set 
		    {
			    SetColumnValue("userfld5", value);
            }
        }
	      
        [XmlAttribute("Userfld6")]
        [Bindable(true)]
        public string Userfld6 
	    {
		    get
		    {
			    return GetColumnValue<string>("userfld6");
		    }
            set 
		    {
			    SetColumnValue("userfld6", value);
            }
        }
	      
        [XmlAttribute("Userfld7")]
        [Bindable(true)]
        public string Userfld7 
	    {
		    get
		    {
			    return GetColumnValue<string>("userfld7");
		    }
            set 
		    {
			    SetColumnValue("userfld7", value);
            }
        }
	      
        [XmlAttribute("Userfld8")]
        [Bindable(true)]
        public string Userfld8 
	    {
		    get
		    {
			    return GetColumnValue<string>("userfld8");
		    }
            set 
		    {
			    SetColumnValue("userfld8", value);
            }
        }
	      
        [XmlAttribute("Userfld9")]
        [Bindable(true)]
        public string Userfld9 
	    {
		    get
		    {
			    return GetColumnValue<string>("userfld9");
		    }
            set 
		    {
			    SetColumnValue("userfld9", value);
            }
        }
	      
        [XmlAttribute("Userfld10")]
        [Bindable(true)]
        public string Userfld10 
	    {
		    get
		    {
			    return GetColumnValue<string>("userfld10");
		    }
            set 
		    {
			    SetColumnValue("userfld10", value);
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
            
            public static string GroupName = @"GroupName";
            
            public static string Discount = @"Discount";
            
            public static string ChineseName = @"ChineseName";
            
            public static string StreetName2 = @"StreetName2";
            
            public static string Address = @"Address";
            
            public static string BirthDayMonth = @"BirthDayMonth";
            
            public static string Name = @"Name";
            
            public static string IsVitaMix = @"IsVitaMix";
            
            public static string IsWaterFilter = @"IsWaterFilter";
            
            public static string IsJuicePlus = @"IsJuicePlus";
            
            public static string IsYoung = @"IsYoung";
            
            public static string SalesPersonID = @"SalesPersonID";
            
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
