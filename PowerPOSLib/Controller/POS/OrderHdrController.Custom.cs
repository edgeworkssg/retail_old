using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.ComponentModel;
using SubSonic;


namespace PowerPOS
{

    /// <summary>
    /// Controller class for Order
    /// </summary>        
    public partial class OrderHdrController
    {
        public DataTable FetchOrderSummary
            (bool useStartDate, bool useEndDate, DateTime StartDate, DateTime EndDate,
            string RefNo, string Cashier, string remark, int PointOfSaleID, string status)
        {
            OrderHdrCollection rHdr = new OrderHdrCollection();
            if (RefNo != "")
            {
                rHdr.Where(OrderHdr.Columns.OrderHdrID, RefNo);
            }
            else
            {
                if (useStartDate & useEndDate)
                {
                    rHdr.BetweenAnd(OrderHdr.Columns.OrderDate, StartDate, EndDate);
                }
                else if (useStartDate)
                {
                    rHdr.Where(OrderHdr.Columns.OrderDate, SubSonic.Comparison.GreaterOrEquals, StartDate);
                }
                else if (useEndDate)
                {
                    rHdr.Where(OrderHdr.Columns.OrderDate, SubSonic.Comparison.LessOrEquals, EndDate);
                }

                //Filter datatable....
                if (status.ToLower() == "voided")
                {
                    rHdr.Where(OrderHdr.Columns.IsVoided, true);
                }
                else if (status.ToLower() == "not voided")
                {
                    rHdr.Where(OrderHdr.Columns.IsVoided, false);
                }
                if (remark != "") rHdr.Where(OrderHdr.Columns.Remark, SubSonic.Comparison.Like, "%" + remark + "%");
                if (Cashier != "") rHdr.Where(OrderHdr.Columns.CashierID, SubSonic.Comparison.Like, "%" + Cashier + "%");
                rHdr.Where(OrderHdr.Columns.PointOfSaleID, PointOfSaleID);
            }
            rHdr.OrderByDesc(OrderHdr.Columns.OrderDate);
            rHdr.Load();

            return rHdr.ToDataTable();

            /*
            ReceiptHdrCollection rHdr = new ReceiptHdrCollection();
            DataTable dTable = new DataTable();                
            DataRow dr;
                
            dTable.Columns.Add("ReceiptHdrId");
            dTable.Columns.Add("OrderRefNo");                
            dTable.Columns.Add("Amount");
            dTable.Columns.Add("OrderDate");
            dTable.Columns.Add("CashierID");
            dTable.Columns.Add("orderHdrID");
            dTable.Columns.Add("IsVoided");

            if (RefNo != "")
            {
                rHdr.Where(ReceiptHdr.Columns.OrderHdrID, RefNo);
            }
            else
            {
                if (useStartDate & useEndDate)
                {
                    rHdr.BetweenAnd(ReceiptHdr.Columns.ReceiptDate, StartDate, EndDate);
                }
                else if (useStartDate)
                {
                    rHdr.Where(ReceiptHdr.Columns.ReceiptDate, SubSonic.Comparison.GreaterOrEquals, StartDate);
                }
                else if (useEndDate)
                {
                    rHdr.Where(ReceiptHdr.Columns.ReceiptDate, SubSonic.Comparison.LessOrEquals, EndDate);
                }
                //Filter datatable....
                if (status.ToLower() == "voided")
                {
                    rHdr.Where(ReceiptHdr.Columns.IsVoided, true);
                }
                else if (status.ToLower() == "not voided")
                {
                    rHdr.Where(ReceiptHdr.Columns.IsVoided, false);
                }
                rHdr.Where(ReceiptHdr.Columns.CashierID, SubSonic.Comparison.Like, "%" + Cashier + "%");
                rHdr.Where(ReceiptHdr.Columns.PointOfSaleID, PointOfSaleID);
            }
            rHdr.OrderByDesc(ReceiptHdr.Columns.ReceiptDate);
            rHdr.Load();
            OrderHdr tmpOrderHdr;
            for (int i = 0; i < rHdr.Count; i++)
            {
                dr = dTable.NewRow();
                tmpOrderHdr = new OrderHdr(rHdr[i].OrderHdrID);
                if (tmpOrderHdr != null)
                {
                    dr["ReceiptHdrId"] = rHdr[i].ReceiptHdrID;
                    dr["OrderRefNo"] = tmpOrderHdr.OrderRefNo;
                    dr["Amount"] = rHdr[i].Amount.ToString("N2");                        
                    dr["OrderDate"] = tmpOrderHdr.OrderDate;
                    dr["CashierID"] = rHdr[i].CashierID;
                    dr["orderHdrID"] = rHdr[i].OrderHdrID;
                    dr["IsVoided"] = tmpOrderHdr.IsVoided;
                    dTable.Rows.Add(dr);
                }
            }
                
            return dTable;
          */
        }

        public DataTable FetchOrderSummary
            (bool useStartDate, bool useEndDate, DateTime StartDate, DateTime EndDate, string RefNo, string Cashier, int PointOfSaleID)
        {

            ReceiptHdrCollection rHdr = new ReceiptHdrCollection();
            DataTable dTable = new DataTable();
            DataRow dr;

            dTable.Columns.Add("ReceiptHdrId");
            dTable.Columns.Add("OrderRefNo");
            dTable.Columns.Add("Amount");
            dTable.Columns.Add("OrderDate");
            dTable.Columns.Add("CashierID");
            dTable.Columns.Add("orderHdrID");
            dTable.Columns.Add("IsVoided");

            if (RefNo != "")
            {
                rHdr.Where(ReceiptHdr.Columns.ReceiptRefNo, RefNo);
            }
            else
            {
                if (useStartDate & useEndDate)
                {
                    rHdr.BetweenAnd(ReceiptHdr.Columns.ReceiptDate, StartDate, EndDate);
                }
                else if (useStartDate)
                {
                    rHdr.Where(ReceiptHdr.Columns.ReceiptDate, SubSonic.Comparison.GreaterOrEquals, StartDate);
                }
                else if (useEndDate)
                {
                    rHdr.Where(ReceiptHdr.Columns.ReceiptDate, SubSonic.Comparison.LessOrEquals, EndDate);
                }
                rHdr.Where(ReceiptHdr.Columns.CashierID, SubSonic.Comparison.Like, "%" + Cashier + "%");
                rHdr.Where(ReceiptHdr.Columns.PointOfSaleID, PointOfSaleID);
            }
            rHdr.OrderByDesc(ReceiptHdr.Columns.ReceiptDate);
            rHdr.Load();
            OrderHdr tmpOrderHdr;
            for (int i = 0; i < rHdr.Count; i++)
            {
                dr = dTable.NewRow();
                tmpOrderHdr = new OrderHdr(rHdr[i].OrderHdrID);
                if (tmpOrderHdr != null)
                {
                    dr["ReceiptHdrId"] = rHdr[i].ReceiptHdrID;
                    dr["OrderRefNo"] = tmpOrderHdr.OrderRefNo;
                    dr["Amount"] = rHdr[i].Amount.ToString("N2");
                    dr["OrderDate"] = tmpOrderHdr.OrderDate;
                    dr["CashierID"] = rHdr[i].CashierID;
                    dr["orderHdrID"] = rHdr[i].OrderHdrID;
                    dr["IsVoided"] = tmpOrderHdr.IsVoided;
                    dTable.Rows.Add(dr);
                }
            }
            return dTable;
        }

        public static bool IsOrderInventoryHasBeenDeductedCompletely(string OrderHdrID)
        {
            Query qr = OrderDet.CreateQuery();
            qr.QueryType = QueryType.Select;
            qr.AddWhere(OrderDet.Columns.OrderHdrID, OrderHdrID);
            qr.AddWhere(OrderDet.Columns.IsVoided, false);
            qr.SelectList = "InventoryHdrRefNo";
            DataTable dt = qr.ExecuteDataSet().Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["InventoryHdrRefNo"].ToString() == "" | dt.Rows[i]["InventoryHdrRefNo"].ToString() == null)
                {
                    return false;
                }
            }
            return true;
        }
        public static bool IsOrderInventoryHasBeenDeductedCompletely(DateTime startDate, DateTime endDate)
        {
            Query qr1 = OrderHdr.CreateQuery();
            qr1.QueryType = QueryType.Select;
            qr1.AddWhere(OrderHdr.Columns.OrderDate, Comparison.GreaterOrEquals, startDate);
            qr1.AddWhere(OrderHdr.Columns.OrderDate, Comparison.LessOrEquals, endDate);
            qr1.AddWhere(OrderHdr.Columns.IsVoided, false);
            qr1.SelectList = "OrderHdrID";
            DataTable dt = qr1.ExecuteDataSet().Tables[0];
            bool result = true;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                result &= IsOrderInventoryHasBeenDeductedCompletely(dt.Rows[i][0].ToString());
            }
            return result;
        }
    }
}
