using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using PowerPOS;

namespace PowerReport
{
    public partial class ProductSales : POSReport
    {
        #region #) Skeleton for the Report
        int PointOfSaleID;
        string PointOfSaleName;
        string OutletName;
        int InventoryLocationID;
        string InventoryLocationName;

        public int ItemDepartmentID;
        public string DepartmentName;
        public string CategoryName;
        public string ItemNo;
        public string ItemName;
        public string Barcode;
        //public decimal RetailPrice;
        //public decimal FactoryPrice;
        public string ItemDesc;
        public bool IsServiceItem;
        public bool IsInInventory;
        public bool IsNonDiscountable;
        public bool IsDelivery;
        public bool IsCommission;
        public string ItemRemark;
        public bool Deleted;
        public string Attributes1;
        public string Attributes2;
        public string Attributes3;
        public string Attributes4;
        public string Attributes5;
        public string Attributes6;
        public string Attributes7;
        public string Attributes8;
        public string userfld1;
        public string userfld2;
        public string userfld3;
        public string userfld4;
        public string userfld5;
        public string userfld6;
        public string userfld7;
        public string userfld8;
        public string userfld9;
        public string userfld10;
        public bool userflag1;
        public bool userflag2;
        public bool userflag3;
        public bool userflag4;
        public bool userflag5;
        public decimal userfloat1;
        public decimal userfloat2;
        public decimal userfloat3;
        public decimal userfloat4;
        public decimal userfloat5;
        public int userint1;
        public int userint2;
        public int userint3;
        public int userint4;
        public int userint5;

        public DateTime OrderDate;
        public int SoldQuantity;
        public decimal LineTotalFinalPrice;
        public decimal LineTotalGstAmount;
        public decimal LineTotalDiscountAmount;
        public decimal LineTotalOriginalPrice;
        public decimal LineTotalCostPrice;

        public string SalesPersonID;
        public string SalesPersonName;
        public bool isVoided;
        #endregion

        public struct InternalParameter
        {
            /* This struct is created so that all parameter inside here will not appear on the Crystal Report
             * The benefit is, there will be less error prone when Support make the Report */

            public string ReportPath;

            public int POSID;
            public bool ShowCostPrice;

            public string SearchQuery;

            public string Att1Name;
            public string Att2Name;
            public string Att3Name;
            public string Att4Name;
            public string Att5Name;
            public string Att6Name;
            public string Att7Name;
            public string Att8Name;
        }
        InternalParameter _InternalParameter;

        public ProductSales(string ReportPath, int POSID, bool ShowCostPrice, string SearchQuery)
        {
            _InternalParameter = new InternalParameter();
            _InternalParameter.ReportPath = ReportPath;
            _InternalParameter.POSID = POSID;
            _InternalParameter.ShowCostPrice = ShowCostPrice;
            _InternalParameter.SearchQuery = SearchQuery;
        }


        public override DataTable GetData()
        {
            string sqlString;
            #region #) SQL String
            sqlString =
                "SELECT X.*, Y.DisplayName AS SalesPersonName, 0 AS LineTotalCostPrice " +
                "FROM " +
                "( " +
                    "SELECT LP.PointOfSaleID, LP.PointOfSaleName, LO.OutletName, LI.InventoryLocationID, LI.InventoryLocationName " +
                        ", ID.ItemDepartmentID, ID.DepartmentName, IC.CategoryName, II.ItemNo, II.ItemName, ISNULL(II.Barcode, '') Barcode " +
                        ", ISNULL(ItemDesc, '') ItemDesc, IsServiceItem, IsInInventory, IsNonDiscountable, ISNULL(IsDelivery, 0) IsDelivery " +
                        ", IsCommission, ISNULL(II.Remark, '') ItemRemark, II.Deleted " +
                        ", ISNULL(Attributes1, '') Attributes1, ISNULL(Attributes2, '') Attributes2, ISNULL(Attributes3, '') Attributes3 " +
                        ", ISNULL(Attributes4, '') Attributes4, ISNULL(Attributes5, '') Attributes5, ISNULL(Attributes6, '') Attributes6 " +
                        ", ISNULL(Attributes7, '') Attributes7, ISNULL(Attributes8, '') Attributes8 " +
                        ", ISNULL(II.UserFld1, '') UserFld1, ISNULL(II.UserFld2, '') UserFld2, ISNULL(II.UserFld3, '') UserFld3 " +
                        ", ISNULL(II.UserFld4, '') UserFld4, ISNULL(II.UserFld5, '') UserFld5, ISNULL(II.UserFld6, '') UserFld6 " +
                        ", ISNULL(II.UserFld7, '') UserFld7, ISNULL(II.UserFld8, '') UserFld8, ISNULL(II.UserFld9, '') UserFld9 " +
                        ", ISNULL(II.UserFld10, '') UserFld10, ISNULL(II.UserFlag1, '') UserFlag1, ISNULL(II.UserFlag2, '') UserFlag2 " +
                        ", ISNULL(II.UserFlag3, '') UserFlag3, ISNULL(II.UserFlag4, '') UserFlag4, ISNULL(II.UserFlag5, '') UserFlag5 " +
                        ", ISNULL(II.UserFloat1, '') UserFloat1, ISNULL(II.UserFloat2, '') UserFloat2, ISNULL(II.UserFloat3, '') UserFloat3 " +
                        ", ISNULL(II.UserFloat4, '') UserFloat4, ISNULL(II.UserFloat5, '') UserFloat5, ISNULL(II.UserInt1, '') UserInt1 " +
                        ", ISNULL(II.UserInt2, '') UserInt2, ISNULL(II.UserInt3, '') UserInt3, ISNULL(II.UserInt4, '') UserInt4 " +
                        ", ISNULL(II.UserInt5, '') UserInt5 " +
                        ", OrderDate, Quantity SoldQuantity, OD.Amount LineTotalFinalPrice, OD.GSTAmount LineTotalGstAmount " +
                        ", (OD.Quantity * OD.UnitPrice * OD.Discount / 100) LineTotalDiscountAmount, (OD.Quantity * OD.UnitPrice) LineTotalOriginalPrice " +
                        ", ISNULL(NULLIF(OD.UserFld1, ''), ISNULL(SCR.SalesPersonID, '')) AS SalesPersonID " +
                        ", (OD.IsVoided & OH.IsVoided) AS IsVoided " +
                    "FROM OrderDet OD " +
                        "INNER JOIN OrderHdr OH ON OD.OrderHdrID = OH.OrderHdrID " +
                        "LEFT OUTER JOIN SalesCommissionRecord SCR ON OH.OrderHdrID = SCR.OrderHdrID " +
                        "INNER JOIN Item II ON OD.ItemNo = II.ItemNo " +
                        "INNER JOIN Category IC ON II.CategoryName = IC.CategoryName " +
                        "INNER JOIN ItemDepartment ID ON IC.ItemDepartmentId = ID.ItemDepartmentID " +
                        "INNER JOIN PointOfSale LP ON OH.PointOfSaleID = LP.PointOfSaleID " +
                        "INNER JOIN Outlet LO ON LP.OutletName = LO.OutletName " +
                        "INNER JOIN InventoryLocation LI ON LO.InventoryLocationID = LI.InventoryLocationID " +
                    "WHERE #AddSearch " +
                ") X LEFT OUTER JOIN UserMst Y ON X.SalesPersonID = Y.UserName ";
            #endregion

            string AddSearch = "";
            #region *) Set search parameter
            AddSearch = "OH.IsVoided = 0 AND OD.IsVoided = 0 ";
            if (_InternalParameter.POSID > 0) AddSearch += "AND LP.PointOfSaleID = " + _InternalParameter.POSID.ToString() + " ";

            sqlString = sqlString.Replace("#AddSearch", AddSearch);
            #endregion

            DataTable Output = new DataTable();
            Output.Load(SubSonic.DataService.GetReader(new SubSonic.QueryCommand(sqlString)));

            return Output;
        }

        public override void AssignData(ref ReportDocument TheReport)
        {
            TheReport.SetDataSource(this.GetData());
            //TheReport.DataDefinition.ParameterFields[0].value
            //if (TheReport.ParameterFields.Contains("Att1Name")) TheReport.ParameterFields["Att1Name"] = "";
        }
    }
}