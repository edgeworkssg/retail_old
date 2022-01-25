using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using SubSonic;

namespace PowerPOS.Report
{
    public class TransactionDetailReportController : ReportController
    {
        public struct SpecialCodes
        {
        }

        public DataTable FetchAll(DateTime StartDate, DateTime EndDate, int PosID, string OutletName, string DeptID, string SortColumn, string SortDir, object SearchValue)
        {
            string strQuery =
                "SELECT OrderDet.ItemNo, OrderDet.OrderDetDate, OrderDet.Quantity, OrderDet.UnitPrice, OrderDet.Discount" +
                    ", OrderDet.Amount, OrderDet.IsPromo, OrderDet.PromoDiscount, OrderDet.PromoAmount" +
                    ", OrderDet.IsPromoFreeOfCharge, OrderDet.IsFreeOfCharge, PointOfSale.PointOfSaleName" +
                    ", OrderHdr.OrderRefNo, Item.ItemName, OrderHdr.NettAmount, OrderHdr.IsVoided" +
                    ", OrderDet.IsVoided AS IsLineVoided, Category.ItemDepartmentID, Item.CategoryName"+
                    ", PointOfSale.OutletName, PointOfSale.DepartmentID, PointOfSale.PointOfSaleID"+
                    ", OrderDet.InventoryHdrRefNo " +
                "FROM OrderDet " +
                    "INNER JOIN OrderHdr ON OrderDet.OrderHdrID = OrderHdr.OrderHdrID " +
                    "INNER JOIN PointOfSale ON OrderHdr.PointOfSaleID = PointOfSale.PointOfSaleID " +
                    "INNER JOIN Item ON OrderDet.ItemNo = Item.ItemNo " +
                    "INNER JOIN Category ON Item.CategoryName = Category.CategoryName " +
                "WHERE OrderDet.OrderDetDate BETWEEN @StartDate AND @EndDate " +
                    "AND PointOfSale.OutletName LIKE @OutletName " +
                    "AND (" +
                        "OrderDet.ItemNo LIKE @Search " +
                        "OR Item.ItemName LIKE @Search " +
                        "OR Item.CategoryName LIKE @Search " +
                        "OR Category.ItemDepartmentID LIKE @Search " +
                        ") ";

            if (PosID != 0) strQuery += " AND PointOfSale.PointOfSaleID = " + PosID.ToString("N0");

            QueryCommand Cmd = new QueryCommand(strQuery);
            Cmd.AddParameter("@StartDate", StartDate, DbType.DateTime);
            Cmd.AddParameter("@EndDate", EndDate, DbType.String);
            Cmd.AddParameter("@OutletName", "%" + OutletName + "%", DbType.String);
            Cmd.AddParameter("@Search", "%" + SearchValue.ToString() + "%", DbType.String);

            DataTable Output = new DataTable();
            Output.Load(DataService.GetReader(Cmd));

            return Output;
        }

        public DataTable FetchSpecial(int SpecialCode, object SearchValue)
        {
            //ItemCollection SpecialItems = new ItemCollection();
            //if (SpecialCode == SpecialCodes.Show_Error_Barcode)
            //{
            //    for (int Counter = 0; Counter < _Item.Count; Counter++)
            //    {
            //        Item InvestigatedItem = _Item[Counter];

            //        if (SpecialItems.Contains(InvestigatedItem)) continue;
            //        if (string.IsNullOrEmpty(InvestigatedItem.Barcode)) { SpecialItems.Add(InvestigatedItem); continue; }
            //        if (_Item.Count(Fnc => Fnc.Barcode == InvestigatedItem.Barcode) > 1) { SpecialItems.Add(InvestigatedItem); continue; }
            //    }
            //}

            //ItemCollection myItems = new ItemCollection();

            //myItems.AddRange(SpecialItems.Where(Fnc
            //    => Fnc.ItemName.ToLower().Contains(SearchValue.ToString().ToLower())
            //    || Fnc.CategoryName.Contains(SearchValue.ToString().ToLower())
            //    || (Fnc.ItemDesc != null && Fnc.ItemDesc.ToLower().Contains(SearchValue.ToString().ToLower()))
            //    || (Fnc.Barcode != null && Fnc.Barcode.ToLower().Contains(SearchValue.ToString().ToLower()))
            //    || (Fnc.Attributes1 != null && Fnc.Attributes1.ToLower().Contains(SearchValue.ToString().ToLower()))
            //    || (Fnc.Attributes2 != null && Fnc.Attributes2.ToLower().Contains(SearchValue.ToString().ToLower()))
            //    || (Fnc.Attributes3 != null && Fnc.Attributes3.ToLower().Contains(SearchValue.ToString().ToLower()))
            //    || (Fnc.Attributes4 != null && Fnc.Attributes4.ToLower().Contains(SearchValue.ToString().ToLower()))
            //    || (Fnc.Attributes5 != null && Fnc.Attributes5.ToLower().Contains(SearchValue.ToString().ToLower()))
            //    ));

            //return AddDepartmentColumn(myItems.ToDataTable());
            return null;
        }
    }
}
