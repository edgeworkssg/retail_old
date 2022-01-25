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
    /// Strongly-typed collection for the ViewWarrantyMembershipItem class.
    /// </summary>
    [Serializable]
    public partial class ViewWarrantyMembershipItemCollection : ReadOnlyList<ViewWarrantyMembershipItem, ViewWarrantyMembershipItemCollection>
    {        
        public ViewWarrantyMembershipItemCollection() {}
    }
    /// <summary>
    /// This is  Read-only wrapper class for the ViewWarrantyMembershipItem view.
    /// </summary>
    [Serializable]
    public partial class ViewWarrantyMembershipItem : ReadOnlyRecord<ViewWarrantyMembershipItem>, IReadOnlyRecord
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
                TableSchema.Table schema = new TableSchema.Table("ViewWarrantyMembershipItem", TableType.View, DataService.GetInstance("PowerPOS"));
                schema.Columns = new TableSchema.TableColumnCollection();
                schema.SchemaName = @"dbo";
                //columns
                
                TableSchema.TableColumn colvarSerialNo = new TableSchema.TableColumn(schema);
                colvarSerialNo.ColumnName = "SerialNo";
                colvarSerialNo.DataType = DbType.AnsiString;
                colvarSerialNo.MaxLength = 50;
                colvarSerialNo.AutoIncrement = false;
                colvarSerialNo.IsNullable = false;
                colvarSerialNo.IsPrimaryKey = false;
                colvarSerialNo.IsForeignKey = false;
                colvarSerialNo.IsReadOnly = false;
                
                schema.Columns.Add(colvarSerialNo);
                
                TableSchema.TableColumn colvarModelNo = new TableSchema.TableColumn(schema);
                colvarModelNo.ColumnName = "ModelNo";
                colvarModelNo.DataType = DbType.AnsiString;
                colvarModelNo.MaxLength = 50;
                colvarModelNo.AutoIncrement = false;
                colvarModelNo.IsNullable = true;
                colvarModelNo.IsPrimaryKey = false;
                colvarModelNo.IsForeignKey = false;
                colvarModelNo.IsReadOnly = false;
                
                schema.Columns.Add(colvarModelNo);
                
                TableSchema.TableColumn colvarOrderDetId = new TableSchema.TableColumn(schema);
                colvarOrderDetId.ColumnName = "OrderDetId";
                colvarOrderDetId.DataType = DbType.AnsiString;
                colvarOrderDetId.MaxLength = 18;
                colvarOrderDetId.AutoIncrement = false;
                colvarOrderDetId.IsNullable = true;
                colvarOrderDetId.IsPrimaryKey = false;
                colvarOrderDetId.IsForeignKey = false;
                colvarOrderDetId.IsReadOnly = false;
                
                schema.Columns.Add(colvarOrderDetId);
                
                TableSchema.TableColumn colvarProductIdentification = new TableSchema.TableColumn(schema);
                colvarProductIdentification.ColumnName = "ProductIdentification";
                colvarProductIdentification.DataType = DbType.String;
                colvarProductIdentification.MaxLength = -1;
                colvarProductIdentification.AutoIncrement = false;
                colvarProductIdentification.IsNullable = true;
                colvarProductIdentification.IsPrimaryKey = false;
                colvarProductIdentification.IsForeignKey = false;
                colvarProductIdentification.IsReadOnly = false;
                
                schema.Columns.Add(colvarProductIdentification);
                
                TableSchema.TableColumn colvarDateOfPurchase = new TableSchema.TableColumn(schema);
                colvarDateOfPurchase.ColumnName = "DateOfPurchase";
                colvarDateOfPurchase.DataType = DbType.DateTime;
                colvarDateOfPurchase.MaxLength = 0;
                colvarDateOfPurchase.AutoIncrement = false;
                colvarDateOfPurchase.IsNullable = true;
                colvarDateOfPurchase.IsPrimaryKey = false;
                colvarDateOfPurchase.IsForeignKey = false;
                colvarDateOfPurchase.IsReadOnly = false;
                
                schema.Columns.Add(colvarDateOfPurchase);
                
                TableSchema.TableColumn colvarDateExpiry = new TableSchema.TableColumn(schema);
                colvarDateExpiry.ColumnName = "DateExpiry";
                colvarDateExpiry.DataType = DbType.DateTime;
                colvarDateExpiry.MaxLength = 0;
                colvarDateExpiry.AutoIncrement = false;
                colvarDateExpiry.IsNullable = true;
                colvarDateExpiry.IsPrimaryKey = false;
                colvarDateExpiry.IsForeignKey = false;
                colvarDateExpiry.IsReadOnly = false;
                
                schema.Columns.Add(colvarDateExpiry);
                
                TableSchema.TableColumn colvarRemark = new TableSchema.TableColumn(schema);
                colvarRemark.ColumnName = "Remark";
                colvarRemark.DataType = DbType.String;
                colvarRemark.MaxLength = -1;
                colvarRemark.AutoIncrement = false;
                colvarRemark.IsNullable = true;
                colvarRemark.IsPrimaryKey = false;
                colvarRemark.IsForeignKey = false;
                colvarRemark.IsReadOnly = false;
                
                schema.Columns.Add(colvarRemark);
                
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
                colvarCreatedBy.DataType = DbType.String;
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
                colvarModifiedBy.DataType = DbType.String;
                colvarModifiedBy.MaxLength = 50;
                colvarModifiedBy.AutoIncrement = false;
                colvarModifiedBy.IsNullable = true;
                colvarModifiedBy.IsPrimaryKey = false;
                colvarModifiedBy.IsForeignKey = false;
                colvarModifiedBy.IsReadOnly = false;
                
                schema.Columns.Add(colvarModifiedBy);
                
                TableSchema.TableColumn colvarMembershipGroupId = new TableSchema.TableColumn(schema);
                colvarMembershipGroupId.ColumnName = "MembershipGroupId";
                colvarMembershipGroupId.DataType = DbType.Int32;
                colvarMembershipGroupId.MaxLength = 0;
                colvarMembershipGroupId.AutoIncrement = false;
                colvarMembershipGroupId.IsNullable = true;
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
                colvarRemarks.DataType = DbType.AnsiString;
                colvarRemarks.MaxLength = 50;
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
                colvarUniqueID.IsNullable = true;
                colvarUniqueID.IsPrimaryKey = false;
                colvarUniqueID.IsForeignKey = false;
                colvarUniqueID.IsReadOnly = false;
                
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
                
                TableSchema.TableColumn colvarUserflag1 = new TableSchema.TableColumn(schema);
                colvarUserflag1.ColumnName = "userflag1";
                colvarUserflag1.DataType = DbType.Boolean;
                colvarUserflag1.MaxLength = 0;
                colvarUserflag1.AutoIncrement = false;
                colvarUserflag1.IsNullable = true;
                colvarUserflag1.IsPrimaryKey = false;
                colvarUserflag1.IsForeignKey = false;
                colvarUserflag1.IsReadOnly = false;
                
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
                
                schema.Columns.Add(colvarUserint5);
                
                TableSchema.TableColumn colvarItemName = new TableSchema.TableColumn(schema);
                colvarItemName.ColumnName = "ItemName";
                colvarItemName.DataType = DbType.String;
                colvarItemName.MaxLength = 300;
                colvarItemName.AutoIncrement = false;
                colvarItemName.IsNullable = true;
                colvarItemName.IsPrimaryKey = false;
                colvarItemName.IsForeignKey = false;
                colvarItemName.IsReadOnly = false;
                
                schema.Columns.Add(colvarItemName);
                
                TableSchema.TableColumn colvarBarcode = new TableSchema.TableColumn(schema);
                colvarBarcode.ColumnName = "Barcode";
                colvarBarcode.DataType = DbType.AnsiString;
                colvarBarcode.MaxLength = 100;
                colvarBarcode.AutoIncrement = false;
                colvarBarcode.IsNullable = true;
                colvarBarcode.IsPrimaryKey = false;
                colvarBarcode.IsForeignKey = false;
                colvarBarcode.IsReadOnly = false;
                
                schema.Columns.Add(colvarBarcode);
                
                TableSchema.TableColumn colvarCategoryName = new TableSchema.TableColumn(schema);
                colvarCategoryName.ColumnName = "CategoryName";
                colvarCategoryName.DataType = DbType.String;
                colvarCategoryName.MaxLength = 250;
                colvarCategoryName.AutoIncrement = false;
                colvarCategoryName.IsNullable = true;
                colvarCategoryName.IsPrimaryKey = false;
                colvarCategoryName.IsForeignKey = false;
                colvarCategoryName.IsReadOnly = false;
                
                schema.Columns.Add(colvarCategoryName);
                
                TableSchema.TableColumn colvarRetailPrice = new TableSchema.TableColumn(schema);
                colvarRetailPrice.ColumnName = "RetailPrice";
                colvarRetailPrice.DataType = DbType.Currency;
                colvarRetailPrice.MaxLength = 0;
                colvarRetailPrice.AutoIncrement = false;
                colvarRetailPrice.IsNullable = true;
                colvarRetailPrice.IsPrimaryKey = false;
                colvarRetailPrice.IsForeignKey = false;
                colvarRetailPrice.IsReadOnly = false;
                
                schema.Columns.Add(colvarRetailPrice);
                
                TableSchema.TableColumn colvarFactoryPrice = new TableSchema.TableColumn(schema);
                colvarFactoryPrice.ColumnName = "FactoryPrice";
                colvarFactoryPrice.DataType = DbType.Currency;
                colvarFactoryPrice.MaxLength = 0;
                colvarFactoryPrice.AutoIncrement = false;
                colvarFactoryPrice.IsNullable = true;
                colvarFactoryPrice.IsPrimaryKey = false;
                colvarFactoryPrice.IsForeignKey = false;
                colvarFactoryPrice.IsReadOnly = false;
                
                schema.Columns.Add(colvarFactoryPrice);
                
                TableSchema.TableColumn colvarMinimumPrice = new TableSchema.TableColumn(schema);
                colvarMinimumPrice.ColumnName = "MinimumPrice";
                colvarMinimumPrice.DataType = DbType.Currency;
                colvarMinimumPrice.MaxLength = 0;
                colvarMinimumPrice.AutoIncrement = false;
                colvarMinimumPrice.IsNullable = true;
                colvarMinimumPrice.IsPrimaryKey = false;
                colvarMinimumPrice.IsForeignKey = false;
                colvarMinimumPrice.IsReadOnly = false;
                
                schema.Columns.Add(colvarMinimumPrice);
                
                TableSchema.TableColumn colvarItemDesc = new TableSchema.TableColumn(schema);
                colvarItemDesc.ColumnName = "ItemDesc";
                colvarItemDesc.DataType = DbType.String;
                colvarItemDesc.MaxLength = 250;
                colvarItemDesc.AutoIncrement = false;
                colvarItemDesc.IsNullable = true;
                colvarItemDesc.IsPrimaryKey = false;
                colvarItemDesc.IsForeignKey = false;
                colvarItemDesc.IsReadOnly = false;
                
                schema.Columns.Add(colvarItemDesc);
                
                TableSchema.TableColumn colvarIsInInventory = new TableSchema.TableColumn(schema);
                colvarIsInInventory.ColumnName = "IsInInventory";
                colvarIsInInventory.DataType = DbType.Boolean;
                colvarIsInInventory.MaxLength = 0;
                colvarIsInInventory.AutoIncrement = false;
                colvarIsInInventory.IsNullable = true;
                colvarIsInInventory.IsPrimaryKey = false;
                colvarIsInInventory.IsForeignKey = false;
                colvarIsInInventory.IsReadOnly = false;
                
                schema.Columns.Add(colvarIsInInventory);
                
                TableSchema.TableColumn colvarIsNonDiscountable = new TableSchema.TableColumn(schema);
                colvarIsNonDiscountable.ColumnName = "IsNonDiscountable";
                colvarIsNonDiscountable.DataType = DbType.Boolean;
                colvarIsNonDiscountable.MaxLength = 0;
                colvarIsNonDiscountable.AutoIncrement = false;
                colvarIsNonDiscountable.IsNullable = true;
                colvarIsNonDiscountable.IsPrimaryKey = false;
                colvarIsNonDiscountable.IsForeignKey = false;
                colvarIsNonDiscountable.IsReadOnly = false;
                
                schema.Columns.Add(colvarIsNonDiscountable);
                
                TableSchema.TableColumn colvarProductLine = new TableSchema.TableColumn(schema);
                colvarProductLine.ColumnName = "ProductLine";
                colvarProductLine.DataType = DbType.AnsiString;
                colvarProductLine.MaxLength = 50;
                colvarProductLine.AutoIncrement = false;
                colvarProductLine.IsNullable = true;
                colvarProductLine.IsPrimaryKey = false;
                colvarProductLine.IsForeignKey = false;
                colvarProductLine.IsReadOnly = false;
                
                schema.Columns.Add(colvarProductLine);
                
                TableSchema.TableColumn colvarProductionDate = new TableSchema.TableColumn(schema);
                colvarProductionDate.ColumnName = "ProductionDate";
                colvarProductionDate.DataType = DbType.DateTime;
                colvarProductionDate.MaxLength = 0;
                colvarProductionDate.AutoIncrement = false;
                colvarProductionDate.IsNullable = true;
                colvarProductionDate.IsPrimaryKey = false;
                colvarProductionDate.IsForeignKey = false;
                colvarProductionDate.IsReadOnly = false;
                
                schema.Columns.Add(colvarProductionDate);
                
                TableSchema.TableColumn colvarIsGST = new TableSchema.TableColumn(schema);
                colvarIsGST.ColumnName = "IsGST";
                colvarIsGST.DataType = DbType.Boolean;
                colvarIsGST.MaxLength = 0;
                colvarIsGST.AutoIncrement = false;
                colvarIsGST.IsNullable = true;
                colvarIsGST.IsPrimaryKey = false;
                colvarIsGST.IsForeignKey = false;
                colvarIsGST.IsReadOnly = false;
                
                schema.Columns.Add(colvarIsGST);
                
                TableSchema.TableColumn colvarItemUniqueId = new TableSchema.TableColumn(schema);
                colvarItemUniqueId.ColumnName = "ItemUniqueId";
                colvarItemUniqueId.DataType = DbType.Guid;
                colvarItemUniqueId.MaxLength = 0;
                colvarItemUniqueId.AutoIncrement = false;
                colvarItemUniqueId.IsNullable = true;
                colvarItemUniqueId.IsPrimaryKey = false;
                colvarItemUniqueId.IsForeignKey = false;
                colvarItemUniqueId.IsReadOnly = false;
                
                schema.Columns.Add(colvarItemUniqueId);
                
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
                
                TableSchema.TableColumn colvarItemNo = new TableSchema.TableColumn(schema);
                colvarItemNo.ColumnName = "ItemNo";
                colvarItemNo.DataType = DbType.AnsiString;
                colvarItemNo.MaxLength = 50;
                colvarItemNo.AutoIncrement = false;
                colvarItemNo.IsNullable = true;
                colvarItemNo.IsPrimaryKey = false;
                colvarItemNo.IsForeignKey = false;
                colvarItemNo.IsReadOnly = false;
                
                schema.Columns.Add(colvarItemNo);
                
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
                
                TableSchema.TableColumn colvarAttributes2 = new TableSchema.TableColumn(schema);
                colvarAttributes2.ColumnName = "Attributes2";
                colvarAttributes2.DataType = DbType.String;
                colvarAttributes2.MaxLength = -1;
                colvarAttributes2.AutoIncrement = false;
                colvarAttributes2.IsNullable = true;
                colvarAttributes2.IsPrimaryKey = false;
                colvarAttributes2.IsForeignKey = false;
                colvarAttributes2.IsReadOnly = false;
                
                schema.Columns.Add(colvarAttributes2);
                
                TableSchema.TableColumn colvarAttributes1 = new TableSchema.TableColumn(schema);
                colvarAttributes1.ColumnName = "Attributes1";
                colvarAttributes1.DataType = DbType.String;
                colvarAttributes1.MaxLength = -1;
                colvarAttributes1.AutoIncrement = false;
                colvarAttributes1.IsNullable = true;
                colvarAttributes1.IsPrimaryKey = false;
                colvarAttributes1.IsForeignKey = false;
                colvarAttributes1.IsReadOnly = false;
                
                schema.Columns.Add(colvarAttributes1);
                
                TableSchema.TableColumn colvarAttributes3 = new TableSchema.TableColumn(schema);
                colvarAttributes3.ColumnName = "Attributes3";
                colvarAttributes3.DataType = DbType.String;
                colvarAttributes3.MaxLength = -1;
                colvarAttributes3.AutoIncrement = false;
                colvarAttributes3.IsNullable = true;
                colvarAttributes3.IsPrimaryKey = false;
                colvarAttributes3.IsForeignKey = false;
                colvarAttributes3.IsReadOnly = false;
                
                schema.Columns.Add(colvarAttributes3);
                
                TableSchema.TableColumn colvarAttributes4 = new TableSchema.TableColumn(schema);
                colvarAttributes4.ColumnName = "Attributes4";
                colvarAttributes4.DataType = DbType.String;
                colvarAttributes4.MaxLength = -1;
                colvarAttributes4.AutoIncrement = false;
                colvarAttributes4.IsNullable = true;
                colvarAttributes4.IsPrimaryKey = false;
                colvarAttributes4.IsForeignKey = false;
                colvarAttributes4.IsReadOnly = false;
                
                schema.Columns.Add(colvarAttributes4);
                
                TableSchema.TableColumn colvarAttributes5 = new TableSchema.TableColumn(schema);
                colvarAttributes5.ColumnName = "Attributes5";
                colvarAttributes5.DataType = DbType.String;
                colvarAttributes5.MaxLength = -1;
                colvarAttributes5.AutoIncrement = false;
                colvarAttributes5.IsNullable = true;
                colvarAttributes5.IsPrimaryKey = false;
                colvarAttributes5.IsForeignKey = false;
                colvarAttributes5.IsReadOnly = false;
                
                schema.Columns.Add(colvarAttributes5);
                
                
                BaseSchema = schema;
                //add this schema to the provider
                //so we can query it later
                DataService.Providers["PowerPOS"].AddSchema("ViewWarrantyMembershipItem",schema);
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
	    public ViewWarrantyMembershipItem()
	    {
            SetSQLProps();
            SetDefaults();
            MarkNew();
        }
        public ViewWarrantyMembershipItem(bool useDatabaseDefaults)
	    {
		    SetSQLProps();
		    if(useDatabaseDefaults)
		    {
				ForceDefaults();
			}
			MarkNew();
	    }
	    
	    public ViewWarrantyMembershipItem(object keyID)
	    {
		    SetSQLProps();
		    LoadByKey(keyID);
	    }
    	 
	    public ViewWarrantyMembershipItem(string columnName, object columnValue)
        {
            SetSQLProps();
            LoadByParam(columnName,columnValue);
        }
        
	    #endregion
	    
	    #region Props
	    
          
        [XmlAttribute("SerialNo")]
        [Bindable(true)]
        public string SerialNo 
	    {
		    get
		    {
			    return GetColumnValue<string>("SerialNo");
		    }
            set 
		    {
			    SetColumnValue("SerialNo", value);
            }
        }
	      
        [XmlAttribute("ModelNo")]
        [Bindable(true)]
        public string ModelNo 
	    {
		    get
		    {
			    return GetColumnValue<string>("ModelNo");
		    }
            set 
		    {
			    SetColumnValue("ModelNo", value);
            }
        }
	      
        [XmlAttribute("OrderDetId")]
        [Bindable(true)]
        public string OrderDetId 
	    {
		    get
		    {
			    return GetColumnValue<string>("OrderDetId");
		    }
            set 
		    {
			    SetColumnValue("OrderDetId", value);
            }
        }
	      
        [XmlAttribute("ProductIdentification")]
        [Bindable(true)]
        public string ProductIdentification 
	    {
		    get
		    {
			    return GetColumnValue<string>("ProductIdentification");
		    }
            set 
		    {
			    SetColumnValue("ProductIdentification", value);
            }
        }
	      
        [XmlAttribute("DateOfPurchase")]
        [Bindable(true)]
        public DateTime? DateOfPurchase 
	    {
		    get
		    {
			    return GetColumnValue<DateTime?>("DateOfPurchase");
		    }
            set 
		    {
			    SetColumnValue("DateOfPurchase", value);
            }
        }
	      
        [XmlAttribute("DateExpiry")]
        [Bindable(true)]
        public DateTime? DateExpiry 
	    {
		    get
		    {
			    return GetColumnValue<DateTime?>("DateExpiry");
		    }
            set 
		    {
			    SetColumnValue("DateExpiry", value);
            }
        }
	      
        [XmlAttribute("Remark")]
        [Bindable(true)]
        public string Remark 
	    {
		    get
		    {
			    return GetColumnValue<string>("Remark");
		    }
            set 
		    {
			    SetColumnValue("Remark", value);
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
	      
        [XmlAttribute("MembershipGroupId")]
        [Bindable(true)]
        public int? MembershipGroupId 
	    {
		    get
		    {
			    return GetColumnValue<int?>("MembershipGroupId");
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
        public Guid? UniqueID 
	    {
		    get
		    {
			    return GetColumnValue<Guid?>("UniqueID");
		    }
            set 
		    {
			    SetColumnValue("UniqueID", value);
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
	      
        [XmlAttribute("Userflag1")]
        [Bindable(true)]
        public bool? Userflag1 
	    {
		    get
		    {
			    return GetColumnValue<bool?>("userflag1");
		    }
            set 
		    {
			    SetColumnValue("userflag1", value);
            }
        }
	      
        [XmlAttribute("Userflag2")]
        [Bindable(true)]
        public bool? Userflag2 
	    {
		    get
		    {
			    return GetColumnValue<bool?>("userflag2");
		    }
            set 
		    {
			    SetColumnValue("userflag2", value);
            }
        }
	      
        [XmlAttribute("Userflag3")]
        [Bindable(true)]
        public bool? Userflag3 
	    {
		    get
		    {
			    return GetColumnValue<bool?>("userflag3");
		    }
            set 
		    {
			    SetColumnValue("userflag3", value);
            }
        }
	      
        [XmlAttribute("Userflag4")]
        [Bindable(true)]
        public bool? Userflag4 
	    {
		    get
		    {
			    return GetColumnValue<bool?>("userflag4");
		    }
            set 
		    {
			    SetColumnValue("userflag4", value);
            }
        }
	      
        [XmlAttribute("Userflag5")]
        [Bindable(true)]
        public bool? Userflag5 
	    {
		    get
		    {
			    return GetColumnValue<bool?>("userflag5");
		    }
            set 
		    {
			    SetColumnValue("userflag5", value);
            }
        }
	      
        [XmlAttribute("Userfloat1")]
        [Bindable(true)]
        public decimal? Userfloat1 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("userfloat1");
		    }
            set 
		    {
			    SetColumnValue("userfloat1", value);
            }
        }
	      
        [XmlAttribute("Userfloat2")]
        [Bindable(true)]
        public decimal? Userfloat2 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("userfloat2");
		    }
            set 
		    {
			    SetColumnValue("userfloat2", value);
            }
        }
	      
        [XmlAttribute("Userfloat3")]
        [Bindable(true)]
        public decimal? Userfloat3 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("userfloat3");
		    }
            set 
		    {
			    SetColumnValue("userfloat3", value);
            }
        }
	      
        [XmlAttribute("Userfloat4")]
        [Bindable(true)]
        public decimal? Userfloat4 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("userfloat4");
		    }
            set 
		    {
			    SetColumnValue("userfloat4", value);
            }
        }
	      
        [XmlAttribute("Userfloat5")]
        [Bindable(true)]
        public decimal? Userfloat5 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("userfloat5");
		    }
            set 
		    {
			    SetColumnValue("userfloat5", value);
            }
        }
	      
        [XmlAttribute("Userint1")]
        [Bindable(true)]
        public int? Userint1 
	    {
		    get
		    {
			    return GetColumnValue<int?>("userint1");
		    }
            set 
		    {
			    SetColumnValue("userint1", value);
            }
        }
	      
        [XmlAttribute("Userint2")]
        [Bindable(true)]
        public int? Userint2 
	    {
		    get
		    {
			    return GetColumnValue<int?>("userint2");
		    }
            set 
		    {
			    SetColumnValue("userint2", value);
            }
        }
	      
        [XmlAttribute("Userint3")]
        [Bindable(true)]
        public int? Userint3 
	    {
		    get
		    {
			    return GetColumnValue<int?>("userint3");
		    }
            set 
		    {
			    SetColumnValue("userint3", value);
            }
        }
	      
        [XmlAttribute("Userint4")]
        [Bindable(true)]
        public int? Userint4 
	    {
		    get
		    {
			    return GetColumnValue<int?>("userint4");
		    }
            set 
		    {
			    SetColumnValue("userint4", value);
            }
        }
	      
        [XmlAttribute("Userint5")]
        [Bindable(true)]
        public int? Userint5 
	    {
		    get
		    {
			    return GetColumnValue<int?>("userint5");
		    }
            set 
		    {
			    SetColumnValue("userint5", value);
            }
        }
	      
        [XmlAttribute("ItemName")]
        [Bindable(true)]
        public string ItemName 
	    {
		    get
		    {
			    return GetColumnValue<string>("ItemName");
		    }
            set 
		    {
			    SetColumnValue("ItemName", value);
            }
        }
	      
        [XmlAttribute("Barcode")]
        [Bindable(true)]
        public string Barcode 
	    {
		    get
		    {
			    return GetColumnValue<string>("Barcode");
		    }
            set 
		    {
			    SetColumnValue("Barcode", value);
            }
        }
	      
        [XmlAttribute("CategoryName")]
        [Bindable(true)]
        public string CategoryName 
	    {
		    get
		    {
			    return GetColumnValue<string>("CategoryName");
		    }
            set 
		    {
			    SetColumnValue("CategoryName", value);
            }
        }
	      
        [XmlAttribute("RetailPrice")]
        [Bindable(true)]
        public decimal? RetailPrice 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("RetailPrice");
		    }
            set 
		    {
			    SetColumnValue("RetailPrice", value);
            }
        }
	      
        [XmlAttribute("FactoryPrice")]
        [Bindable(true)]
        public decimal? FactoryPrice 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("FactoryPrice");
		    }
            set 
		    {
			    SetColumnValue("FactoryPrice", value);
            }
        }
	      
        [XmlAttribute("MinimumPrice")]
        [Bindable(true)]
        public decimal? MinimumPrice 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("MinimumPrice");
		    }
            set 
		    {
			    SetColumnValue("MinimumPrice", value);
            }
        }
	      
        [XmlAttribute("ItemDesc")]
        [Bindable(true)]
        public string ItemDesc 
	    {
		    get
		    {
			    return GetColumnValue<string>("ItemDesc");
		    }
            set 
		    {
			    SetColumnValue("ItemDesc", value);
            }
        }
	      
        [XmlAttribute("IsInInventory")]
        [Bindable(true)]
        public bool? IsInInventory 
	    {
		    get
		    {
			    return GetColumnValue<bool?>("IsInInventory");
		    }
            set 
		    {
			    SetColumnValue("IsInInventory", value);
            }
        }
	      
        [XmlAttribute("IsNonDiscountable")]
        [Bindable(true)]
        public bool? IsNonDiscountable 
	    {
		    get
		    {
			    return GetColumnValue<bool?>("IsNonDiscountable");
		    }
            set 
		    {
			    SetColumnValue("IsNonDiscountable", value);
            }
        }
	      
        [XmlAttribute("ProductLine")]
        [Bindable(true)]
        public string ProductLine 
	    {
		    get
		    {
			    return GetColumnValue<string>("ProductLine");
		    }
            set 
		    {
			    SetColumnValue("ProductLine", value);
            }
        }
	      
        [XmlAttribute("ProductionDate")]
        [Bindable(true)]
        public DateTime? ProductionDate 
	    {
		    get
		    {
			    return GetColumnValue<DateTime?>("ProductionDate");
		    }
            set 
		    {
			    SetColumnValue("ProductionDate", value);
            }
        }
	      
        [XmlAttribute("IsGST")]
        [Bindable(true)]
        public bool? IsGST 
	    {
		    get
		    {
			    return GetColumnValue<bool?>("IsGST");
		    }
            set 
		    {
			    SetColumnValue("IsGST", value);
            }
        }
	      
        [XmlAttribute("ItemUniqueId")]
        [Bindable(true)]
        public Guid? ItemUniqueId 
	    {
		    get
		    {
			    return GetColumnValue<Guid?>("ItemUniqueId");
		    }
            set 
		    {
			    SetColumnValue("ItemUniqueId", value);
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
	      
        [XmlAttribute("ItemNo")]
        [Bindable(true)]
        public string ItemNo 
	    {
		    get
		    {
			    return GetColumnValue<string>("ItemNo");
		    }
            set 
		    {
			    SetColumnValue("ItemNo", value);
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
	      
        [XmlAttribute("Attributes2")]
        [Bindable(true)]
        public string Attributes2 
	    {
		    get
		    {
			    return GetColumnValue<string>("Attributes2");
		    }
            set 
		    {
			    SetColumnValue("Attributes2", value);
            }
        }
	      
        [XmlAttribute("Attributes1")]
        [Bindable(true)]
        public string Attributes1 
	    {
		    get
		    {
			    return GetColumnValue<string>("Attributes1");
		    }
            set 
		    {
			    SetColumnValue("Attributes1", value);
            }
        }
	      
        [XmlAttribute("Attributes3")]
        [Bindable(true)]
        public string Attributes3 
	    {
		    get
		    {
			    return GetColumnValue<string>("Attributes3");
		    }
            set 
		    {
			    SetColumnValue("Attributes3", value);
            }
        }
	      
        [XmlAttribute("Attributes4")]
        [Bindable(true)]
        public string Attributes4 
	    {
		    get
		    {
			    return GetColumnValue<string>("Attributes4");
		    }
            set 
		    {
			    SetColumnValue("Attributes4", value);
            }
        }
	      
        [XmlAttribute("Attributes5")]
        [Bindable(true)]
        public string Attributes5 
	    {
		    get
		    {
			    return GetColumnValue<string>("Attributes5");
		    }
            set 
		    {
			    SetColumnValue("Attributes5", value);
            }
        }
	    
	    #endregion
    
	    #region Columns Struct
	    public struct Columns
	    {
		    
		    
            public static string SerialNo = @"SerialNo";
            
            public static string ModelNo = @"ModelNo";
            
            public static string OrderDetId = @"OrderDetId";
            
            public static string ProductIdentification = @"ProductIdentification";
            
            public static string DateOfPurchase = @"DateOfPurchase";
            
            public static string DateExpiry = @"DateExpiry";
            
            public static string Remark = @"Remark";
            
            public static string CreatedOn = @"CreatedOn";
            
            public static string CreatedBy = @"CreatedBy";
            
            public static string ModifiedOn = @"ModifiedOn";
            
            public static string ModifiedBy = @"ModifiedBy";
            
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
            
            public static string IsStudentCard = @"IsStudentCard";
            
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
            
            public static string ItemName = @"ItemName";
            
            public static string Barcode = @"Barcode";
            
            public static string CategoryName = @"CategoryName";
            
            public static string RetailPrice = @"RetailPrice";
            
            public static string FactoryPrice = @"FactoryPrice";
            
            public static string MinimumPrice = @"MinimumPrice";
            
            public static string ItemDesc = @"ItemDesc";
            
            public static string IsInInventory = @"IsInInventory";
            
            public static string IsNonDiscountable = @"IsNonDiscountable";
            
            public static string ProductLine = @"ProductLine";
            
            public static string ProductionDate = @"ProductionDate";
            
            public static string IsGST = @"IsGST";
            
            public static string ItemUniqueId = @"ItemUniqueId";
            
            public static string MembershipNo = @"MembershipNo";
            
            public static string ItemNo = @"ItemNo";
            
            public static string IsVitaMix = @"IsVitaMix";
            
            public static string IsWaterFilter = @"IsWaterFilter";
            
            public static string IsJuicePlus = @"IsJuicePlus";
            
            public static string IsYoung = @"IsYoung";
            
            public static string Attributes2 = @"Attributes2";
            
            public static string Attributes1 = @"Attributes1";
            
            public static string Attributes3 = @"Attributes3";
            
            public static string Attributes4 = @"Attributes4";
            
            public static string Attributes5 = @"Attributes5";
            
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
