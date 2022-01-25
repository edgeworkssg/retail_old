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
    /// Strongly-typed collection for the ViewTransactionWithMembership class.
    /// </summary>
    [Serializable]
    public partial class ViewTransactionWithMembershipCollection : ReadOnlyList<ViewTransactionWithMembership, ViewTransactionWithMembershipCollection>
    {        
        public ViewTransactionWithMembershipCollection() {}
    }
    /// <summary>
    /// This is  Read-only wrapper class for the ViewTransactionWithMembership view.
    /// </summary>
    [Serializable]
    public partial class ViewTransactionWithMembership : ReadOnlyRecord<ViewTransactionWithMembership>, IReadOnlyRecord
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
                TableSchema.Table schema = new TableSchema.Table("ViewTransactionWithMembership", TableType.View, DataService.GetInstance("PowerPOS"));
                schema.Columns = new TableSchema.TableColumnCollection();
                schema.SchemaName = @"dbo";
                //columns
                
                TableSchema.TableColumn colvarOrderRefNo = new TableSchema.TableColumn(schema);
                colvarOrderRefNo.ColumnName = "OrderRefNo";
                colvarOrderRefNo.DataType = DbType.AnsiString;
                colvarOrderRefNo.MaxLength = 50;
                colvarOrderRefNo.AutoIncrement = false;
                colvarOrderRefNo.IsNullable = false;
                colvarOrderRefNo.IsPrimaryKey = false;
                colvarOrderRefNo.IsForeignKey = false;
                colvarOrderRefNo.IsReadOnly = false;
                
                schema.Columns.Add(colvarOrderRefNo);
                
                TableSchema.TableColumn colvarOrderDate = new TableSchema.TableColumn(schema);
                colvarOrderDate.ColumnName = "OrderDate";
                colvarOrderDate.DataType = DbType.DateTime;
                colvarOrderDate.MaxLength = 0;
                colvarOrderDate.AutoIncrement = false;
                colvarOrderDate.IsNullable = false;
                colvarOrderDate.IsPrimaryKey = false;
                colvarOrderDate.IsForeignKey = false;
                colvarOrderDate.IsReadOnly = false;
                
                schema.Columns.Add(colvarOrderDate);
                
                TableSchema.TableColumn colvarOutletName = new TableSchema.TableColumn(schema);
                colvarOutletName.ColumnName = "OutletName";
                colvarOutletName.DataType = DbType.AnsiString;
                colvarOutletName.MaxLength = 50;
                colvarOutletName.AutoIncrement = false;
                colvarOutletName.IsNullable = false;
                colvarOutletName.IsPrimaryKey = false;
                colvarOutletName.IsForeignKey = false;
                colvarOutletName.IsReadOnly = false;
                
                schema.Columns.Add(colvarOutletName);
                
                TableSchema.TableColumn colvarPointOfSaleName = new TableSchema.TableColumn(schema);
                colvarPointOfSaleName.ColumnName = "PointOfSaleName";
                colvarPointOfSaleName.DataType = DbType.AnsiString;
                colvarPointOfSaleName.MaxLength = 50;
                colvarPointOfSaleName.AutoIncrement = false;
                colvarPointOfSaleName.IsNullable = false;
                colvarPointOfSaleName.IsPrimaryKey = false;
                colvarPointOfSaleName.IsForeignKey = false;
                colvarPointOfSaleName.IsReadOnly = false;
                
                schema.Columns.Add(colvarPointOfSaleName);
                
                TableSchema.TableColumn colvarOrderHdrID = new TableSchema.TableColumn(schema);
                colvarOrderHdrID.ColumnName = "OrderHdrID";
                colvarOrderHdrID.DataType = DbType.AnsiString;
                colvarOrderHdrID.MaxLength = 14;
                colvarOrderHdrID.AutoIncrement = false;
                colvarOrderHdrID.IsNullable = false;
                colvarOrderHdrID.IsPrimaryKey = false;
                colvarOrderHdrID.IsForeignKey = false;
                colvarOrderHdrID.IsReadOnly = false;
                
                schema.Columns.Add(colvarOrderHdrID);
                
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
                
                TableSchema.TableColumn colvarItemName = new TableSchema.TableColumn(schema);
                colvarItemName.ColumnName = "ItemName";
                colvarItemName.DataType = DbType.String;
                colvarItemName.MaxLength = 300;
                colvarItemName.AutoIncrement = false;
                colvarItemName.IsNullable = false;
                colvarItemName.IsPrimaryKey = false;
                colvarItemName.IsForeignKey = false;
                colvarItemName.IsReadOnly = false;
                
                schema.Columns.Add(colvarItemName);
                
                TableSchema.TableColumn colvarItemNo = new TableSchema.TableColumn(schema);
                colvarItemNo.ColumnName = "ItemNo";
                colvarItemNo.DataType = DbType.AnsiString;
                colvarItemNo.MaxLength = 50;
                colvarItemNo.AutoIncrement = false;
                colvarItemNo.IsNullable = false;
                colvarItemNo.IsPrimaryKey = false;
                colvarItemNo.IsForeignKey = false;
                colvarItemNo.IsReadOnly = false;
                
                schema.Columns.Add(colvarItemNo);
                
                TableSchema.TableColumn colvarCategoryName = new TableSchema.TableColumn(schema);
                colvarCategoryName.ColumnName = "CategoryName";
                colvarCategoryName.DataType = DbType.String;
                colvarCategoryName.MaxLength = 250;
                colvarCategoryName.AutoIncrement = false;
                colvarCategoryName.IsNullable = false;
                colvarCategoryName.IsPrimaryKey = false;
                colvarCategoryName.IsForeignKey = false;
                colvarCategoryName.IsReadOnly = false;
                
                schema.Columns.Add(colvarCategoryName);
                
                TableSchema.TableColumn colvarLineAmount = new TableSchema.TableColumn(schema);
                colvarLineAmount.ColumnName = "LineAmount";
                colvarLineAmount.DataType = DbType.Currency;
                colvarLineAmount.MaxLength = 0;
                colvarLineAmount.AutoIncrement = false;
                colvarLineAmount.IsNullable = false;
                colvarLineAmount.IsPrimaryKey = false;
                colvarLineAmount.IsForeignKey = false;
                colvarLineAmount.IsReadOnly = false;
                
                schema.Columns.Add(colvarLineAmount);
                
                TableSchema.TableColumn colvarQuantity = new TableSchema.TableColumn(schema);
                colvarQuantity.ColumnName = "Quantity";
                colvarQuantity.DataType = DbType.Int32;
                colvarQuantity.MaxLength = 0;
                colvarQuantity.AutoIncrement = false;
                colvarQuantity.IsNullable = false;
                colvarQuantity.IsPrimaryKey = false;
                colvarQuantity.IsForeignKey = false;
                colvarQuantity.IsReadOnly = false;
                
                schema.Columns.Add(colvarQuantity);
                
                TableSchema.TableColumn colvarUnitPrice = new TableSchema.TableColumn(schema);
                colvarUnitPrice.ColumnName = "UnitPrice";
                colvarUnitPrice.DataType = DbType.Currency;
                colvarUnitPrice.MaxLength = 0;
                colvarUnitPrice.AutoIncrement = false;
                colvarUnitPrice.IsNullable = false;
                colvarUnitPrice.IsPrimaryKey = false;
                colvarUnitPrice.IsForeignKey = false;
                colvarUnitPrice.IsReadOnly = false;
                
                schema.Columns.Add(colvarUnitPrice);
                
                TableSchema.TableColumn colvarIsFreeOfCharge = new TableSchema.TableColumn(schema);
                colvarIsFreeOfCharge.ColumnName = "IsFreeOfCharge";
                colvarIsFreeOfCharge.DataType = DbType.Boolean;
                colvarIsFreeOfCharge.MaxLength = 0;
                colvarIsFreeOfCharge.AutoIncrement = false;
                colvarIsFreeOfCharge.IsNullable = false;
                colvarIsFreeOfCharge.IsPrimaryKey = false;
                colvarIsFreeOfCharge.IsForeignKey = false;
                colvarIsFreeOfCharge.IsReadOnly = false;
                
                schema.Columns.Add(colvarIsFreeOfCharge);
                
                TableSchema.TableColumn colvarPointOfSaleID = new TableSchema.TableColumn(schema);
                colvarPointOfSaleID.ColumnName = "PointOfSaleID";
                colvarPointOfSaleID.DataType = DbType.Int32;
                colvarPointOfSaleID.MaxLength = 0;
                colvarPointOfSaleID.AutoIncrement = false;
                colvarPointOfSaleID.IsNullable = false;
                colvarPointOfSaleID.IsPrimaryKey = false;
                colvarPointOfSaleID.IsForeignKey = false;
                colvarPointOfSaleID.IsReadOnly = false;
                
                schema.Columns.Add(colvarPointOfSaleID);
                
                TableSchema.TableColumn colvarDepartmentID = new TableSchema.TableColumn(schema);
                colvarDepartmentID.ColumnName = "DepartmentID";
                colvarDepartmentID.DataType = DbType.Int32;
                colvarDepartmentID.MaxLength = 0;
                colvarDepartmentID.AutoIncrement = false;
                colvarDepartmentID.IsNullable = true;
                colvarDepartmentID.IsPrimaryKey = false;
                colvarDepartmentID.IsForeignKey = false;
                colvarDepartmentID.IsReadOnly = false;
                
                schema.Columns.Add(colvarDepartmentID);
                
                TableSchema.TableColumn colvarCashierID = new TableSchema.TableColumn(schema);
                colvarCashierID.ColumnName = "CashierID";
                colvarCashierID.DataType = DbType.AnsiString;
                colvarCashierID.MaxLength = 50;
                colvarCashierID.AutoIncrement = false;
                colvarCashierID.IsNullable = false;
                colvarCashierID.IsPrimaryKey = false;
                colvarCashierID.IsForeignKey = false;
                colvarCashierID.IsReadOnly = false;
                
                schema.Columns.Add(colvarCashierID);
                
                TableSchema.TableColumn colvarIsPromo = new TableSchema.TableColumn(schema);
                colvarIsPromo.ColumnName = "IsPromo";
                colvarIsPromo.DataType = DbType.Boolean;
                colvarIsPromo.MaxLength = 0;
                colvarIsPromo.AutoIncrement = false;
                colvarIsPromo.IsNullable = false;
                colvarIsPromo.IsPrimaryKey = false;
                colvarIsPromo.IsForeignKey = false;
                colvarIsPromo.IsReadOnly = false;
                
                schema.Columns.Add(colvarIsPromo);
                
                TableSchema.TableColumn colvarPromoAmount = new TableSchema.TableColumn(schema);
                colvarPromoAmount.ColumnName = "PromoAmount";
                colvarPromoAmount.DataType = DbType.Currency;
                colvarPromoAmount.MaxLength = 0;
                colvarPromoAmount.AutoIncrement = false;
                colvarPromoAmount.IsNullable = false;
                colvarPromoAmount.IsPrimaryKey = false;
                colvarPromoAmount.IsForeignKey = false;
                colvarPromoAmount.IsReadOnly = false;
                
                schema.Columns.Add(colvarPromoAmount);
                
                TableSchema.TableColumn colvarExpr17 = new TableSchema.TableColumn(schema);
                colvarExpr17.ColumnName = "Expr17";
                colvarExpr17.DataType = DbType.Currency;
                colvarExpr17.MaxLength = 0;
                colvarExpr17.AutoIncrement = false;
                colvarExpr17.IsNullable = false;
                colvarExpr17.IsPrimaryKey = false;
                colvarExpr17.IsForeignKey = false;
                colvarExpr17.IsReadOnly = false;
                
                schema.Columns.Add(colvarExpr17);
                
                TableSchema.TableColumn colvarPromoDiscount = new TableSchema.TableColumn(schema);
                colvarPromoDiscount.ColumnName = "PromoDiscount";
                colvarPromoDiscount.DataType = DbType.Double;
                colvarPromoDiscount.MaxLength = 0;
                colvarPromoDiscount.AutoIncrement = false;
                colvarPromoDiscount.IsNullable = false;
                colvarPromoDiscount.IsPrimaryKey = false;
                colvarPromoDiscount.IsForeignKey = false;
                colvarPromoDiscount.IsReadOnly = false;
                
                schema.Columns.Add(colvarPromoDiscount);
                
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
                
                
                BaseSchema = schema;
                //add this schema to the provider
                //so we can query it later
                DataService.Providers["PowerPOS"].AddSchema("ViewTransactionWithMembership",schema);
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
	    public ViewTransactionWithMembership()
	    {
            SetSQLProps();
            SetDefaults();
            MarkNew();
        }
        public ViewTransactionWithMembership(bool useDatabaseDefaults)
	    {
		    SetSQLProps();
		    if(useDatabaseDefaults)
		    {
				ForceDefaults();
			}
			MarkNew();
	    }
	    
	    public ViewTransactionWithMembership(object keyID)
	    {
		    SetSQLProps();
		    LoadByKey(keyID);
	    }
    	 
	    public ViewTransactionWithMembership(string columnName, object columnValue)
        {
            SetSQLProps();
            LoadByParam(columnName,columnValue);
        }
        
	    #endregion
	    
	    #region Props
	    
          
        [XmlAttribute("OrderRefNo")]
        [Bindable(true)]
        public string OrderRefNo 
	    {
		    get
		    {
			    return GetColumnValue<string>("OrderRefNo");
		    }
            set 
		    {
			    SetColumnValue("OrderRefNo", value);
            }
        }
	      
        [XmlAttribute("OrderDate")]
        [Bindable(true)]
        public DateTime OrderDate 
	    {
		    get
		    {
			    return GetColumnValue<DateTime>("OrderDate");
		    }
            set 
		    {
			    SetColumnValue("OrderDate", value);
            }
        }
	      
        [XmlAttribute("OutletName")]
        [Bindable(true)]
        public string OutletName 
	    {
		    get
		    {
			    return GetColumnValue<string>("OutletName");
		    }
            set 
		    {
			    SetColumnValue("OutletName", value);
            }
        }
	      
        [XmlAttribute("PointOfSaleName")]
        [Bindable(true)]
        public string PointOfSaleName 
	    {
		    get
		    {
			    return GetColumnValue<string>("PointOfSaleName");
		    }
            set 
		    {
			    SetColumnValue("PointOfSaleName", value);
            }
        }
	      
        [XmlAttribute("OrderHdrID")]
        [Bindable(true)]
        public string OrderHdrID 
	    {
		    get
		    {
			    return GetColumnValue<string>("OrderHdrID");
		    }
            set 
		    {
			    SetColumnValue("OrderHdrID", value);
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
	      
        [XmlAttribute("LineAmount")]
        [Bindable(true)]
        public decimal LineAmount 
	    {
		    get
		    {
			    return GetColumnValue<decimal>("LineAmount");
		    }
            set 
		    {
			    SetColumnValue("LineAmount", value);
            }
        }
	      
        [XmlAttribute("Quantity")]
        [Bindable(true)]
        public int Quantity 
	    {
		    get
		    {
			    return GetColumnValue<int>("Quantity");
		    }
            set 
		    {
			    SetColumnValue("Quantity", value);
            }
        }
	      
        [XmlAttribute("UnitPrice")]
        [Bindable(true)]
        public decimal UnitPrice 
	    {
		    get
		    {
			    return GetColumnValue<decimal>("UnitPrice");
		    }
            set 
		    {
			    SetColumnValue("UnitPrice", value);
            }
        }
	      
        [XmlAttribute("IsFreeOfCharge")]
        [Bindable(true)]
        public bool IsFreeOfCharge 
	    {
		    get
		    {
			    return GetColumnValue<bool>("IsFreeOfCharge");
		    }
            set 
		    {
			    SetColumnValue("IsFreeOfCharge", value);
            }
        }
	      
        [XmlAttribute("PointOfSaleID")]
        [Bindable(true)]
        public int PointOfSaleID 
	    {
		    get
		    {
			    return GetColumnValue<int>("PointOfSaleID");
		    }
            set 
		    {
			    SetColumnValue("PointOfSaleID", value);
            }
        }
	      
        [XmlAttribute("DepartmentID")]
        [Bindable(true)]
        public int? DepartmentID 
	    {
		    get
		    {
			    return GetColumnValue<int?>("DepartmentID");
		    }
            set 
		    {
			    SetColumnValue("DepartmentID", value);
            }
        }
	      
        [XmlAttribute("CashierID")]
        [Bindable(true)]
        public string CashierID 
	    {
		    get
		    {
			    return GetColumnValue<string>("CashierID");
		    }
            set 
		    {
			    SetColumnValue("CashierID", value);
            }
        }
	      
        [XmlAttribute("IsPromo")]
        [Bindable(true)]
        public bool IsPromo 
	    {
		    get
		    {
			    return GetColumnValue<bool>("IsPromo");
		    }
            set 
		    {
			    SetColumnValue("IsPromo", value);
            }
        }
	      
        [XmlAttribute("PromoAmount")]
        [Bindable(true)]
        public decimal PromoAmount 
	    {
		    get
		    {
			    return GetColumnValue<decimal>("PromoAmount");
		    }
            set 
		    {
			    SetColumnValue("PromoAmount", value);
            }
        }
	      
        [XmlAttribute("Expr17")]
        [Bindable(true)]
        public decimal Expr17 
	    {
		    get
		    {
			    return GetColumnValue<decimal>("Expr17");
		    }
            set 
		    {
			    SetColumnValue("Expr17", value);
            }
        }
	      
        [XmlAttribute("PromoDiscount")]
        [Bindable(true)]
        public double PromoDiscount 
	    {
		    get
		    {
			    return GetColumnValue<double>("PromoDiscount");
		    }
            set 
		    {
			    SetColumnValue("PromoDiscount", value);
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
	    
	    #endregion
    
	    #region Columns Struct
	    public struct Columns
	    {
		    
		    
            public static string OrderRefNo = @"OrderRefNo";
            
            public static string OrderDate = @"OrderDate";
            
            public static string OutletName = @"OutletName";
            
            public static string PointOfSaleName = @"PointOfSaleName";
            
            public static string OrderHdrID = @"OrderHdrID";
            
            public static string Amount = @"Amount";
            
            public static string ItemName = @"ItemName";
            
            public static string ItemNo = @"ItemNo";
            
            public static string CategoryName = @"CategoryName";
            
            public static string LineAmount = @"LineAmount";
            
            public static string Quantity = @"Quantity";
            
            public static string UnitPrice = @"UnitPrice";
            
            public static string IsFreeOfCharge = @"IsFreeOfCharge";
            
            public static string PointOfSaleID = @"PointOfSaleID";
            
            public static string DepartmentID = @"DepartmentID";
            
            public static string CashierID = @"CashierID";
            
            public static string IsPromo = @"IsPromo";
            
            public static string PromoAmount = @"PromoAmount";
            
            public static string Expr17 = @"Expr17";
            
            public static string PromoDiscount = @"PromoDiscount";
            
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
