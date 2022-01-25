using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PowerReport
{
    public partial class CommissionReport
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
        public string Attributes1;
        public string Attributes2;
        public string Attributes3;
        public string Attributes4;
        public string Attributes5;
        public string Attributes6;
        public string Attributes7;
        public string Attributes8;
        public string ItemRemark;
        public bool Deleted;
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
        public bool IsDelivery;
        public bool IsCommission;

        public string Att1Name;
        public string Att2Name;
        public string Att3Name;
        public string Att4Name;
        public string Att5Name;
        public string Att6Name;
        public string Att7Name;
        public string Att8Name;

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

        public CommissionReport
            (DateTime StartDate, DateTime EndDate, int POSID, bool InventoryOnly, bool ServiceOnly)
            //(int POSID, bool IncludeDeletedItem, bool IncludeVoidedItem, string SearchQuery)
        {
        }

    }
}
