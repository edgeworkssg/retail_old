using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Configuration;
using PowerPOS;
using System.Data;
using SubSonic;
using NPOI.HSSF.Model; // InternalWorkbook
using NPOI.HSSF.UserModel; // HSSFWorkbook, HSSFSheet
using CrystalDecisions;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace ERPIntegration.API
{
    class Export_Order_Payment
    {
        struct MessageType
        {
            public static string INFO = "EXPORT_ORDER_PAYMENT_INFO";
            public static string ERROR = "EXPORT_ORDER_PAYMENT_ERROR";
        }

        public static bool DoExport(string directory, DateTime orderDate)
        {
            bool proceed = true;
            string dirOrder, dirPayment;

            try
            {
                Helper.WriteLog("Starting Export Order & Payment module", MessageType.INFO);

                if (directory.Contains('|'))
                {
                    string[] tmpDir = directory.Split('|');
                    dirOrder = tmpDir[0];
                    dirPayment = tmpDir[1];
                }
                else
                {
                    dirOrder = directory;
                    dirPayment = directory;
                }

                DirectoryInfo fileDirOrder = new DirectoryInfo(dirOrder);
                DirectoryInfo fileDirPayment = new DirectoryInfo(dirPayment);

                if (!fileDirOrder.Exists)
                {
                    Helper.WriteLog(string.Format("Directory {0} does not exist.", dirOrder), MessageType.ERROR);
                    proceed = false;
                }

                if (!fileDirPayment.Exists)
                {
                    Helper.WriteLog(string.Format("Directory {0} does not exist.", dirPayment), MessageType.ERROR);
                    proceed = false;
                }

                if (!proceed)
                {
                    return false;
                }
                else
                {
                    OrderHdrCollection orderColl = GetOrderHeaders(orderDate);
                    Helper.WriteLog(string.Format("Found {0} order(s) to export.", orderColl.Count), MessageType.INFO);

                    if (orderColl.Count > 0)
                    {
                        string orderFile = Path.Combine(fileDirOrder.FullName, "OR_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xls");
                        GenerateOrderXLS(orderColl, orderFile);

                        string paymentFile = Path.Combine(fileDirPayment.FullName, "PY_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xls");
                        GeneratePaymentXLS(orderColl, paymentFile);
                    }

                    // Update LastSalesCutOffDate in AppSetting to DateTime.Now
                    string cutoffDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    AppSetting.SetSetting("LastSalesCutOffDate", cutoffDate);

                    return true;
                }
            }
            catch (Exception ex)
            {
                Helper.WriteLog(ex.ToString(), MessageType.ERROR);
                return false;
            }
        }

        public static bool DoExportRpt(string directory, DateTime orderDate)
        {
            bool proceed = true;
            string dirOrder, dirPayment;

            try
            {
                Helper.WriteLog("Starting Export Order & Payment module", MessageType.INFO);

                if (directory.Contains('|'))
                {
                    string[] tmpDir = directory.Split('|');
                    dirOrder = tmpDir[0];
                    dirPayment = tmpDir[1];
                }
                else
                {
                    dirOrder = directory;
                    dirPayment = directory;
                }

                DirectoryInfo fileDirOrder = new DirectoryInfo(dirOrder);
                DirectoryInfo fileDirPayment = new DirectoryInfo(dirPayment);

                if (!fileDirOrder.Exists)
                {
                    Helper.WriteLog(string.Format("Directory {0} does not exist.", dirOrder), MessageType.ERROR);
                    proceed = false;
                }

                if (!fileDirPayment.Exists)
                {
                    Helper.WriteLog(string.Format("Directory {0} does not exist.", dirPayment), MessageType.ERROR);
                    proceed = false;
                }

                bool exportOrder = AppSetting.CastBool(ConfigurationManager.AppSettings["ERP_Order_Export"], true);
                bool exportPayment = AppSetting.CastBool(ConfigurationManager.AppSettings["ERP_Payment_Export"], true);

                if (!proceed)
                {
                    return false;
                }
                else
                {

                    OrderHdrCollection orderColl = GetOrderHeaders(orderDate);
                    Helper.WriteLog(string.Format("Found {0} order(s) to export.", orderColl.Count), MessageType.INFO);

                    if (orderColl.Count > 0)
                    {
                        string orderFile = Path.Combine(fileDirOrder.FullName, "OR_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xls");
                        GenerateOrderXLSRpt(orderColl, orderFile);

                        /*string paymentFile = Path.Combine(fileDirPayment.FullName, "PY_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xls");
                        GeneratePaymentXLS(orderColl, paymentFile);*/
                    }

                    // Update LastSalesCutOffDate in AppSetting to DateTime.Now
                    string cutoffDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    AppSetting.SetSetting("LastSalesCutOffDate", cutoffDate);

                    return true;
                }
            }
            catch (Exception ex)
            {
                Helper.WriteLog(ex.ToString(), MessageType.ERROR);
                return false;
            }
        }

        private static OrderHdrCollection GetOrderHeaders(DateTime orderDate)
        {
            DateTime startDate = orderDate.Date;
            DateTime endDate = orderDate.Date.Add(TimeSpan.Parse("23:59:59"));

            OrderHdrCollection orderColl = new OrderHdrCollection();
            //orderColl.Where("OrderDate", Comparison.GreaterOrEquals, startDate);
            //orderColl.Where("OrderDate", Comparison.LessOrEquals, endDate);
            //orderColl.Where("IsVoided", false);
            //orderColl.Load();

            string sql = @"
                            SELECT * 
                            FROM OrderHdr 
                            WHERE OrderDate BETWEEN @StartDate AND @EndDate 
                                AND IsVoided = 0 AND ISNULL(" + OrderHdr.UserColumns.IsExported + @", 0) = 0
                            ORDER BY OrderDate
                         ";
            QueryCommand cmd = new QueryCommand(sql, "PowerPOS");
            cmd.Parameters.Add("@StartDate", startDate, DbType.DateTime);
            cmd.Parameters.Add("@EndDate", endDate, DbType.DateTime);
            orderColl.Load(DataService.GetReader(cmd));

            return orderColl;
        }

        private static void GenerateOrderXLS(OrderHdrCollection orderColl, string file)
        {
            Console.WriteLine();
            Helper.WriteLog("Generating xls file for Order...", MessageType.INFO);

            var workbook = HSSFWorkbook.Create(InternalWorkbook.CreateWorkbook());
            var sheet = workbook.CreateSheet("Sheet1");
            int rowindex = 0;
            NPOI.SS.UserModel.IRow row;

            string spcBarcodeColHdr = ConfigurationManager.AppSettings["Order_SpecialBarcodeColumnHeader"];
            if (string.IsNullOrEmpty(spcBarcodeColHdr)) spcBarcodeColHdr = "WEIGHT";

            #region *) Generate Column Header (for both OrderHdr and OrderDet)
            // Column Header for OrderHdr
            row = sheet.CreateRow(rowindex);
            rowindex++;
            row.CreateCell(0).SetCellValue("RECTYPE");
            row.CreateCell(1).SetCellValue("ORDNUMBER");
            row.CreateCell(2).SetCellValue("ORDTYPE");
            row.CreateCell(3).SetCellValue("CUSTOMER");
            row.CreateCell(4).SetCellValue("BILNAME");
            row.CreateCell(5).SetCellValue("SHPADDR1");
            row.CreateCell(6).SetCellValue("SHPADDR2");
            row.CreateCell(7).SetCellValue("SHPADDR3");
            row.CreateCell(8).SetCellValue("SHPADDR4");
            row.CreateCell(9).SetCellValue("SHPCITY");
            row.CreateCell(10).SetCellValue("SHPSTATE");
            row.CreateCell(11).SetCellValue("SHPZIP");
            row.CreateCell(12).SetCellValue("SHPCOUNTRY");
            row.CreateCell(13).SetCellValue("SHPPHONE");
            row.CreateCell(14).SetCellValue("SHPFAX");
            row.CreateCell(15).SetCellValue("PONUMBER");
            row.CreateCell(16).SetCellValue("REFERENCE");
            row.CreateCell(17).SetCellValue("ORDDATE");
            row.CreateCell(18).SetCellValue("EXPDATE");
            row.CreateCell(19).SetCellValue("LOCATION");
            row.CreateCell(20).SetCellValue("DESC");
            row.CreateCell(21).SetCellValue("APPLYDOC");
            row.CreateCell(22).SetCellValue("SALESPERSON");
            row.CreateCell(23).SetCellValue("EXPSYS");

            // Column Header for OrderDet
            row = sheet.CreateRow(rowindex);
            rowindex++;
            row.CreateCell(0).SetCellValue("RECTYPE");
            row.CreateCell(1).SetCellValue("ORDNUMBER");
            row.CreateCell(2).SetCellValue("LINENUM");
            row.CreateCell(3).SetCellValue("ITEM");
            row.CreateCell(4).SetCellValue("DESC");
            row.CreateCell(5).SetCellValue("LOCATION");
            row.CreateCell(6).SetCellValue("EXPDATE");
            row.CreateCell(7).SetCellValue("QTYORDERED");
            row.CreateCell(8).SetCellValue("ORDUNIT");
            row.CreateCell(9).SetCellValue("UNITPRICE");
            row.CreateCell(10).SetCellValue("REMARK1");
            row.CreateCell(11).SetCellValue("ITEMTYPE");
            row.CreateCell(12).SetCellValue("FLAG");
            row.CreateCell(13).SetCellValue("REMARK2");
            row.CreateCell(14).SetCellValue("LINEINFO");
            row.CreateCell(15).SetCellValue(spcBarcodeColHdr);
            row.CreateCell(16).SetCellValue("BARCODE");

            #endregion

            for (int i = 0; i < orderColl.Count; i++)
            {
                OrderHdr orderHdr = orderColl[i];
                OrderDetCollection orderDets = orderHdr.OrderDetRecords();

                Helper.WriteLog(string.Format("Processing order #{0} : {1}.", (i + 1).ToString(), orderHdr.OrderHdrID), MessageType.INFO);

                #region *) Normal / Refund / Exchange ?
                string ordType = "";
                POSController pos = new POSController(orderHdr.OrderHdrID);
                if (pos.IsReturnedItemsOnly())
                    ordType = "REFUND";
                else if (pos.HasReturnedItems())
                    ordType = "EXCHANGE";
                #endregion

                #region *) Get delivery information
                string deliAddress = "";
                string deliPostalCode = "";
                string deliMobileNo = "";
                string deliHomeNo = "";
                string deliDate = "";
                DeliveryOrder doHdr = new DeliveryOrder(DeliveryOrder.Columns.SalesOrderRefNo, orderHdr.OrderHdrID);
                if (doHdr != null && doHdr.SalesOrderRefNo == orderHdr.OrderHdrID)
                {
                    deliAddress = doHdr.DeliveryAddress ?? "";
                    deliPostalCode = doHdr.PostalCode ?? "";
                    deliMobileNo = doHdr.MobileNo ?? "";
                    deliHomeNo = doHdr.HomeNo ?? "";
                    deliDate = doHdr.DeliveryDate.HasValue ? doHdr.DeliveryDate.Value.ToString("yyyyMMdd") : "";
                }
                #endregion

                if (ordType != "EXCHANGE")
                {
                    #region *) Generate OrderHdr row
                    //POSController pos = new POSController(orderHdr.OrderHdrID);
                    row = sheet.CreateRow(rowindex);
                    rowindex++;

                    row.CreateCell(0).SetCellValue(1);  // RECTYPE
                    row.CreateCell(1).SetCellValue(orderHdr.Userfld5);  // ORDNUMBER

                    if (ordType == "REFUND")
                    {
                        // 3-Refund
                        row.CreateCell(2).SetCellValue(3);  // ORDTYPE
                    }
                    else
                    {
                        // 1-Order
                        row.CreateCell(2).SetCellValue(1);  // ORDTYPE
                    }

                    row.CreateCell(3).SetCellValue(orderHdr.MembershipNo);  // CUSTOMER

                    Membership member = new Membership(orderHdr.MembershipNo);
                    row.CreateCell(4).SetCellValue(member.NameToAppear);  // BILNAME
                    row.CreateCell(5).SetCellValue(deliAddress);  // SHPADDR1
                    row.CreateCell(6).SetCellValue("");  // SHPADDR2
                    row.CreateCell(7).SetCellValue("");  // SHPADDR3
                    row.CreateCell(8).SetCellValue("");  // SHPADDR4
                    row.CreateCell(9).SetCellValue("");  // SHPCITY
                    row.CreateCell(10).SetCellValue("");  // SHPSTATE
                    row.CreateCell(11).SetCellValue(deliPostalCode);  // SHPZIP
                    row.CreateCell(12).SetCellValue("");  // SHPCOUNTRY
                    row.CreateCell(13).SetCellValue((deliMobileNo + "/" + deliHomeNo).Trim('/'));  // SHPPHONE
                    row.CreateCell(14).SetCellValue("");  // SHPFAX
                    row.CreateCell(15).SetCellValue("");  // PONUMBER
                    row.CreateCell(16).SetCellValue("");  // REFERENCE
                    row.CreateCell(17).SetCellValue(orderHdr.OrderDate.ToString("yyyyMMdd"));  // ORDDATE
                    row.CreateCell(18).SetCellValue(deliDate);  // EXPDATE

                    row.CreateCell(19).SetCellValue(orderHdr.PointOfSale.Outlet.InventoryLocation.InventoryLocationName);  // LOCATION
                    row.CreateCell(20).SetCellValue("");  // DESC

                    string retRecNo = "";  // for invoice no that exists in POS
                    foreach (OrderDet od in orderDets)
                    {
                        if (!string.IsNullOrEmpty(od.ReturnedReceiptNo) && !string.IsNullOrEmpty(POSController.GetOrderHdrIDByCustomReceiptNo(od.ReturnedReceiptNo)))
                        {
                            retRecNo = od.ReturnedReceiptNo;
                            break;
                        }
                    }
                    row.CreateCell(21).SetCellValue(retRecNo);  // APPLYDOC
                    row.CreateCell(22).SetCellValue(new SalesCommissionRecord("OrderHdrID", orderHdr.OrderHdrID).SalesPersonID);  // SALESPERSON
                    row.CreateCell(23).SetCellValue("P");  // EXPSYS // Fixed to P as of now
                    #endregion

                    #region *) Calculate Unit Price and Rounding from OrderDet
                    decimal totalAmt = 0, totalAmtRounded = 0, rounding = 0;
                    foreach (OrderDet od in orderDets)
                    {
                        totalAmt += od.Amount;
                    }
                    totalAmt = Math.Round(totalAmt, 2, MidpointRounding.AwayFromZero);

                    foreach (OrderDet od in orderDets)
                    {
                        if (od.ItemNo != POSController.ROUNDING_ITEM)
                        {
                            if (od.Quantity == 0)
                                od.UnitPrice = 0;
                            else
                                od.UnitPrice = Math.Round(od.Amount / od.Quantity.GetValueOrDefault(0), 2, MidpointRounding.AwayFromZero);
                            totalAmtRounded += (od.UnitPrice * od.Quantity.GetValueOrDefault(0));
                        }
                    }

                    if (totalAmt != totalAmtRounded)
                    {
                        rounding = Math.Abs(totalAmt) - Math.Abs(totalAmtRounded);

                        OrderDet tmpOD = orderDets.SingleOrDefault(od => od.ItemNo == POSController.ROUNDING_ITEM);
                        if (tmpOD != null && tmpOD.ItemNo == POSController.ROUNDING_ITEM)
                        {
                            tmpOD.UnitPrice = rounding;
                        }
                        else
                        {
                            tmpOD = new OrderDet();
                            tmpOD.OrderHdrID = orderHdr.OrderHdrID;
                            tmpOD.ItemNo = POSController.ROUNDING_ITEM;
                            tmpOD.Quantity = 1;
                            tmpOD.UnitPrice = rounding;
                            tmpOD.IsVoided = false;
                            orderDets.Add(tmpOD);
                        }
                    }
                    #endregion

                    #region *) Generate OrderDet rows
                    for (int j = 0; j < orderDets.Count; j++)
                    {
                        OrderDet orderDet = orderDets[j];
                        if (orderDet.IsVoided) continue;

                        row = sheet.CreateRow(rowindex);
                        rowindex++;
                        
                        row.CreateCell(0).SetCellValue(2);  // RECTYPE
                        row.CreateCell(1).SetCellValue(orderDet.OrderHdr.Userfld5);  // ORDNUMBER
                        row.CreateCell(2).SetCellValue(j + 1);  // LINENUM
                        if (orderDet.ItemNo != "LINE_DISCOUNT")
                        {
                            row.CreateCell(3).SetCellValue(orderDet.ItemNo);  // ITEM
                            row.CreateCell(4).SetCellValue(orderDet.Item.ItemName);  // DESC
                        }
                        else
                        {
                            row.CreateCell(3).SetCellValue("DISCOUNT");  // ITEM
                            row.CreateCell(4).SetCellValue("DISCOUNT");  // DESC
                        }
                        //row.CreateCell(5).SetCellValue("3SHR");  // LOCATION
                        row.CreateCell(5).SetCellValue(orderHdr.PointOfSale.Outlet.InventoryLocation.InventoryLocationName);  // LOCATION
                        row.CreateCell(6).SetCellValue(deliDate);  // EXPDATE

                        row.CreateCell(7).SetCellValue(Convert.ToDouble(Math.Abs(orderDet.Quantity.GetValueOrDefault(0))));  // QTYORDERED
                        row.CreateCell(8).SetCellValue("");  // ORDUNIT

                        //row.CreateCell(9).SetCellValue(Convert.ToDouble(orderDet.UnitPrice));  // UNITPRICE
                        if (orderDet.ItemNo == POSController.ROUNDING_ITEM)
                            row.CreateCell(9).SetCellValue(Convert.ToDouble(Math.Abs(orderDet.UnitPrice)));  // UNITPRICE
                        else
                            row.CreateCell(9).SetCellValue(Convert.ToDouble(orderDet.UnitPrice));  // UNITPRICE

                        row.CreateCell(10).SetCellValue(orderDet.Remark);  // REMARK1
                        row.CreateCell(11).SetCellValue(orderDet.Item.IsInInventory ? 1 : 2);  // ITEMTYPE

                        if (orderDet.ItemNo == POSController.ROUNDING_ITEM)
                        {
                            if (orderDet.UnitPrice < 0)
                                row.CreateCell(12).SetCellValue("ROUNDDOWN");  // FLAG
                            else
                                row.CreateCell(12).SetCellValue("ROUNDUP");  // FLAG
                        }
                        else
                        {
                            row.CreateCell(12).SetCellValue("");  // FLAG
                        }

                        string extInvNo = "";  // for invoice no that does not exist in POS
                        if (!string.IsNullOrEmpty(orderDet.ReturnedReceiptNo) && string.IsNullOrEmpty(POSController.GetOrderHdrIDByCustomReceiptNo(orderDet.ReturnedReceiptNo)))
                        {
                            extInvNo = orderDet.ReturnedReceiptNo;
                        }
                        row.CreateCell(13).SetCellValue(extInvNo);  // REMARK2
                        row.CreateCell(14).SetCellValue(orderDet.LineInfo);  // LINEINFO
                        row.CreateCell(15).SetCellValue(orderDet.SpecialBarcode);  // spcBarcodeColHdr
                        row.CreateCell(16).SetCellValue(orderDet.Item.Barcode);  // BARCODE
                    }
                    #endregion
                }
                else // For exchange transaction, split the transaction into 2 order records
                {
                    #region *) Split the OrderDets between (+) and (-) quantity (returned items)
                    OrderDetCollection orderDetsExc = new OrderDetCollection();
                    OrderDetCollection orderDetsRet = new OrderDetCollection();
                    foreach (OrderDet od in orderDets)
                    {
                        if (!od.IsVoided)
                        {
                            if (od.Quantity >= 0)
                                orderDetsExc.Add(od);
                            else
                                orderDetsRet.Add(od);
                        }
                    }
                    #endregion

                    #region *) Calculate Unit Price and Rounding from OrderDet
                    decimal totalAmt = 0, totalAmtRounded = 0, rounding = 0;
                    foreach (OrderDet od in orderDets)
                    {
                        totalAmt += od.Amount;
                    }
                    totalAmt = Math.Round(totalAmt, 2, MidpointRounding.AwayFromZero);

                    foreach (OrderDet od in orderDets)
                    {
                        if (od.ItemNo != POSController.ROUNDING_ITEM)
                        {
                            if (od.Quantity == 0)
                                od.UnitPrice = 0;
                            else
                                od.UnitPrice = Math.Round(od.Amount / od.Quantity.GetValueOrDefault(0), 2, MidpointRounding.AwayFromZero);
                            totalAmtRounded += (od.UnitPrice * od.Quantity.GetValueOrDefault(0));
                        }
                    }

                    if (totalAmt != totalAmtRounded)
                    {
                        rounding = Math.Abs(totalAmt) - Math.Abs(totalAmtRounded);

                        if (orderDetsExc.Count > 0)
                        {
                            OrderDet tmpOD = orderDetsExc.SingleOrDefault(od => od.ItemNo == POSController.ROUNDING_ITEM);
                            if (tmpOD != null && tmpOD.ItemNo == POSController.ROUNDING_ITEM)
                            {
                                tmpOD.UnitPrice = rounding;
                            }
                            else
                            {
                                tmpOD = new OrderDet();
                                tmpOD.OrderHdrID = orderHdr.OrderHdrID;
                                tmpOD.ItemNo = POSController.ROUNDING_ITEM;
                                tmpOD.Quantity = 1;
                                tmpOD.UnitPrice = rounding;
                                tmpOD.IsVoided = false;
                                orderDetsExc.Add(tmpOD);
                            }
                        }
                        else
                        {
                            OrderDet tmpOD = orderDetsRet.SingleOrDefault(od => od.ItemNo == POSController.ROUNDING_ITEM);
                            if (tmpOD != null && tmpOD.ItemNo == POSController.ROUNDING_ITEM)
                            {
                                tmpOD.UnitPrice = rounding;
                            }
                            else
                            {
                                tmpOD = new OrderDet();
                                tmpOD.OrderHdrID = orderHdr.OrderHdrID;
                                tmpOD.ItemNo = POSController.ROUNDING_ITEM;
                                tmpOD.Quantity = 1;
                                tmpOD.UnitPrice = rounding;
                                tmpOD.IsVoided = false;
                                orderDetsRet.Add(tmpOD);
                            }
                        }
                    }
                    #endregion

                    if (orderDetsExc.Count > 0)
                    {
                        #region *) Generate OrderHdr row for exchange items
                        row = sheet.CreateRow(rowindex);
                        rowindex++;

                        row.CreateCell(0).SetCellValue(1);  // RECTYPE
                        row.CreateCell(1).SetCellValue(orderHdr.Userfld5);  // ORDNUMBER
                        row.CreateCell(2).SetCellValue(1);  // ORDTYPE = 1-Order
                        row.CreateCell(3).SetCellValue(orderHdr.MembershipNo);  // CUSTOMER

                        Membership member = new Membership(orderHdr.MembershipNo);
                        row.CreateCell(4).SetCellValue(member.NameToAppear);  // BILNAME
                        row.CreateCell(5).SetCellValue(deliAddress);  // SHPADDR1
                        row.CreateCell(6).SetCellValue("");  // SHPADDR2
                        row.CreateCell(7).SetCellValue("");  // SHPADDR3
                        row.CreateCell(8).SetCellValue("");  // SHPADDR4
                        row.CreateCell(9).SetCellValue("");  // SHPCITY
                        row.CreateCell(10).SetCellValue("");  // SHPSTATE
                        row.CreateCell(11).SetCellValue(deliPostalCode);  // SHPZIP
                        row.CreateCell(12).SetCellValue("");  // SHPCOUNTRY
                        row.CreateCell(13).SetCellValue((deliMobileNo + "/" + deliHomeNo).Trim('/'));  // SHPPHONE
                        row.CreateCell(14).SetCellValue("");  // SHPFAX
                        row.CreateCell(15).SetCellValue("");  // PONUMBER
                        row.CreateCell(16).SetCellValue("");  // REFERENCE
                        row.CreateCell(17).SetCellValue(orderHdr.OrderDate.ToString("yyyyMMdd"));  // ORDDATE
                        row.CreateCell(18).SetCellValue(deliDate);  // EXPDATE

                        row.CreateCell(19).SetCellValue(orderHdr.PointOfSale.Outlet.InventoryLocation.InventoryLocationName);  // LOCATION
                        row.CreateCell(20).SetCellValue("");  // DESC
                        row.CreateCell(21).SetCellValue("");  // APPLYDOC
                        row.CreateCell(22).SetCellValue(new SalesCommissionRecord("OrderHdrID", orderHdr.OrderHdrID).SalesPersonID);  // SALESPERSON
                        row.CreateCell(23).SetCellValue("P");  // EXPSYS // Fixed to P as of now
                        #endregion

                        #region *) Generate OrderDet rows for exchange items
                        for (int j = 0; j < orderDetsExc.Count; j++)
                        {
                            OrderDet orderDet = orderDetsExc[j];
                            if (orderDet.IsVoided) continue;

                            row = sheet.CreateRow(rowindex);
                            rowindex++;

                            row.CreateCell(0).SetCellValue(2);  // RECTYPE
                            row.CreateCell(1).SetCellValue(orderDet.OrderHdr.Userfld5);  // ORDNUMBER
                            row.CreateCell(2).SetCellValue(j + 1);  // LINENUM
                            if (orderDet.ItemNo != "LINE_DISCOUNT")
                            {
                                row.CreateCell(3).SetCellValue(orderDet.ItemNo);  // ITEM
                                row.CreateCell(4).SetCellValue(orderDet.Item.ItemName);  // DESC
                            }
                            else
                            {
                                row.CreateCell(3).SetCellValue("DISCOUNT");  // ITEM
                                row.CreateCell(4).SetCellValue("DISCOUNT");  // DESC
                            }
                            //row.CreateCell(5).SetCellValue("3SHR");  // LOCATION
                            row.CreateCell(5).SetCellValue(orderHdr.PointOfSale.Outlet.InventoryLocation.InventoryLocationName);  // LOCATION
                            row.CreateCell(6).SetCellValue(deliDate);  // EXPDATE

                            row.CreateCell(7).SetCellValue(Convert.ToDouble(Math.Abs(orderDet.Quantity.GetValueOrDefault(0))));  // QTYORDERED
                            row.CreateCell(8).SetCellValue("");  // ORDUNIT

                            //row.CreateCell(9).SetCellValue(Convert.ToDouble(orderDet.UnitPrice));  // UNITPRICE
                            if (orderDet.ItemNo == POSController.ROUNDING_ITEM)
                                row.CreateCell(9).SetCellValue(Convert.ToDouble(Math.Abs(orderDet.UnitPrice)));  // UNITPRICE
                            else
                                row.CreateCell(9).SetCellValue(Convert.ToDouble(orderDet.UnitPrice));  // UNITPRICE

                            row.CreateCell(10).SetCellValue(orderDet.Remark);  // REMARK1
                            row.CreateCell(11).SetCellValue(orderDet.Item.IsInInventory ? 1 : 2);  // ITEMTYPE

                            if (orderDet.ItemNo == POSController.ROUNDING_ITEM)
                            {
                                if (orderDet.UnitPrice < 0)
                                    row.CreateCell(12).SetCellValue("ROUNDDOWN");  // FLAG
                                else
                                    row.CreateCell(12).SetCellValue("ROUNDUP");  // FLAG
                            }
                            else
                            {
                                row.CreateCell(12).SetCellValue("");  // FLAG
                            }

                            row.CreateCell(13).SetCellValue("");  // REMARK2
                            row.CreateCell(14).SetCellValue(orderDet.LineInfo);  // LINEINFO
                            row.CreateCell(15).SetCellValue(orderDet.SpecialBarcode);  // spcBarcodeColHdr
                            row.CreateCell(16).SetCellValue(orderDet.Item.Barcode);  // BARCODE
                        }
                        #endregion
                    }

                    if (orderDetsRet.Count > 0)
                    {
                        #region *) Generate OrderHdr row for returned items
                        row = sheet.CreateRow(rowindex);
                        rowindex++;

                        row.CreateCell(0).SetCellValue(1);  // RECTYPE
                        row.CreateCell(1).SetCellValue(orderHdr.Userfld5 + "RF");  // ORDNUMBER
                        row.CreateCell(2).SetCellValue(3);  // ORDTYPE = 3-Refund
                        row.CreateCell(3).SetCellValue(orderHdr.MembershipNo);  // CUSTOMER

                        Membership member = new Membership(orderHdr.MembershipNo);
                        row.CreateCell(4).SetCellValue(member.NameToAppear);  // BILNAME
                        row.CreateCell(5).SetCellValue("");  // SHPADDR1
                        row.CreateCell(6).SetCellValue("");  // SHPADDR2
                        row.CreateCell(7).SetCellValue("");  // SHPADDR3
                        row.CreateCell(8).SetCellValue("");  // SHPADDR4
                        row.CreateCell(9).SetCellValue("");  // SHPCITY
                        row.CreateCell(10).SetCellValue("");  // SHPSTATE
                        row.CreateCell(11).SetCellValue("");  // SHPZIP
                        row.CreateCell(12).SetCellValue("");  // SHPCOUNTRY
                        row.CreateCell(13).SetCellValue("");  // SHPPHONE
                        row.CreateCell(14).SetCellValue("");  // SHPFAX
                        row.CreateCell(15).SetCellValue("");  // PONUMBER
                        row.CreateCell(16).SetCellValue("");  // REFERENCE
                        row.CreateCell(17).SetCellValue(orderHdr.OrderDate.ToString("yyyyMMdd"));  // ORDDATE
                        row.CreateCell(18).SetCellValue("");  // EXPDATE

                        row.CreateCell(19).SetCellValue(orderHdr.PointOfSale.Outlet.InventoryLocation.InventoryLocationName);  // LOCATION
                        row.CreateCell(20).SetCellValue("");  // DESC

                        string retRecNo = "";  // for invoice no that exists in POS
                        foreach (OrderDet od in orderDetsRet)
                        {
                            if (!string.IsNullOrEmpty(od.ReturnedReceiptNo) && !string.IsNullOrEmpty(POSController.GetOrderHdrIDByCustomReceiptNo(od.ReturnedReceiptNo)))
                            {
                                retRecNo = od.ReturnedReceiptNo;
                                break;
                            }
                        }
                        row.CreateCell(21).SetCellValue(retRecNo);  // APPLYDOC
                        row.CreateCell(22).SetCellValue(new SalesCommissionRecord("OrderHdrID", orderHdr.OrderHdrID).SalesPersonID);  // SALESPERSON
                        row.CreateCell(23).SetCellValue("P");  // EXPSYS // Fixed to P as of now
                        #endregion

                        #region *) Generate OrderDet rows for returned items
                        for (int j = 0; j < orderDetsRet.Count; j++)
                        {
                            OrderDet orderDet = orderDetsRet[j];
                            if (orderDet.IsVoided) continue;

                            row = sheet.CreateRow(rowindex);
                            rowindex++;

                            row.CreateCell(0).SetCellValue(2);  // RECTYPE
                            row.CreateCell(1).SetCellValue(orderDet.OrderHdr.Userfld5 + "RF");  // ORDNUMBER
                            row.CreateCell(2).SetCellValue(j + 1);  // LINENUM
                            if (orderDet.ItemNo != "LINE_DISCOUNT")
                            {
                                row.CreateCell(3).SetCellValue(orderDet.ItemNo);  // ITEM
                                row.CreateCell(4).SetCellValue(orderDet.Item.ItemName);  // DESC
                            }
                            else
                            {
                                row.CreateCell(3).SetCellValue("DISCOUNT");  // ITEM
                                row.CreateCell(4).SetCellValue("DISCOUNT");  // DESC
                            }
                            //row.CreateCell(5).SetCellValue("3SHR");  // LOCATION
                            row.CreateCell(5).SetCellValue(orderHdr.PointOfSale.Outlet.InventoryLocation.InventoryLocationName);  // LOCATION
                            row.CreateCell(6).SetCellValue(deliDate);  // EXPDATE

                            row.CreateCell(7).SetCellValue(Convert.ToDouble(Math.Abs(orderDet.Quantity.GetValueOrDefault(0))));  // QTYORDERED
                            row.CreateCell(8).SetCellValue("");  // ORDUNIT

                            //row.CreateCell(9).SetCellValue(Convert.ToDouble(orderDet.UnitPrice));  // UNITPRICE
                            if (orderDet.ItemNo == POSController.ROUNDING_ITEM)
                                row.CreateCell(9).SetCellValue(Convert.ToDouble(Math.Abs(orderDet.UnitPrice)));  // UNITPRICE
                            else
                                row.CreateCell(9).SetCellValue(Convert.ToDouble(orderDet.UnitPrice));  // UNITPRICE

                            row.CreateCell(10).SetCellValue(orderDet.Remark);  // REMARK1
                            row.CreateCell(11).SetCellValue(orderDet.Item.IsInInventory ? 1 : 2);  // ITEMTYPE

                            if (orderDet.ItemNo == POSController.ROUNDING_ITEM)
                            {
                                if (orderDet.UnitPrice < 0)
                                    row.CreateCell(12).SetCellValue("ROUNDDOWN");  // FLAG
                                else
                                    row.CreateCell(12).SetCellValue("ROUNDUP");  // FLAG
                            }
                            else
                            {
                                row.CreateCell(12).SetCellValue("");  // FLAG
                            }

                            string extInvNo = "";  // for invoice no that does not exist in POS
                            if (!string.IsNullOrEmpty(orderDet.ReturnedReceiptNo) && string.IsNullOrEmpty(POSController.GetOrderHdrIDByCustomReceiptNo(orderDet.ReturnedReceiptNo)))
                            {
                                extInvNo = orderDet.ReturnedReceiptNo;
                            }
                            row.CreateCell(13).SetCellValue(extInvNo);  // REMARK2
                            row.CreateCell(14).SetCellValue(orderDet.LineInfo);  // LINEINFO
                            row.CreateCell(15).SetCellValue(orderDet.SpecialBarcode);  // spcBarcodeColHdr
                            row.CreateCell(16).SetCellValue(orderDet.Item.Barcode);  // BARCODE
                        }
                        #endregion
                    }
                }
            }

            Helper.WriteLog("Saving xls file for Order...", MessageType.INFO);
            using (FileStream fs = new FileStream(file, FileMode.Create, FileAccess.Write))
            {
                workbook.Write(fs);
            }
            Helper.WriteLog(string.Format("Save complete. File name: {0}", Path.GetFileName(file)), MessageType.INFO);

            #region *) Set OrderHdr.IsExported = true (OrderHdr.Userflag2)
            Helper.WriteLog("Marking Order as exported...", MessageType.INFO);
            QueryCommandCollection cmdColl = new QueryCommandCollection();
            foreach (OrderHdr oh in orderColl)
            {
                Query qry = new Query("OrderHdr");
                qry.QueryType = QueryType.Update;
                qry.AddUpdateSetting(OrderHdr.UserColumns.IsExported, true);
                qry.AddWhere(OrderHdr.Columns.OrderHdrID, oh.OrderHdrID);
                cmdColl.Add(qry.BuildUpdateCommand());
            }
            DataService.ExecuteTransaction(cmdColl);
            Helper.WriteLog("Complete marking Order as exported", MessageType.INFO);
            #endregion

            Helper.WriteLog("Generating xls file for Order completed successfully.", MessageType.INFO);
        }

        private static void GeneratePaymentXLS(OrderHdrCollection orderColl, string file)
        {
            Console.WriteLine();
            Helper.WriteLog("Generating xls file for Payment...", MessageType.INFO);

            var workbook = HSSFWorkbook.Create(InternalWorkbook.CreateWorkbook());
            var sheet = workbook.CreateSheet("Sheet1");
            int rowindex = 0;
            NPOI.SS.UserModel.IRow row;

            #region *) Generate Column Header
            row = sheet.CreateRow(rowindex);
            rowindex++;
            row.CreateCell(0).SetCellValue("IDBANK");
            row.CreateCell(1).SetCellValue("IDCUST");
            row.CreateCell(2).SetCellValue("TEXTRMIT");
            row.CreateCell(3).SetCellValue("TXTRMITREF");
            row.CreateCell(4).SetCellValue("DATERMIT");
            row.CreateCell(5).SetCellValue("DATEBUS");
            row.CreateCell(6).SetCellValue("CODEPAYM");
            row.CreateCell(7).SetCellValue("IDRMIT");
            row.CreateCell(8).SetCellValue("AMTRMIT");
            row.CreateCell(9).SetCellValue("IDINVCMTCH");
            #endregion

            for (int i = 0; i < orderColl.Count; i++)
            {
                OrderHdr orderHdr = orderColl[i];
                OrderDetCollection orderDets = orderHdr.OrderDetRecords();
                ReceiptHdrCollection receiptHdrs = orderHdr.ReceiptHdrRecords();
                ReceiptDetCollection receiptDets = new ReceiptDetCollection();

                string ordType = "";
                POSController pos = new POSController(orderHdr.OrderHdrID);
                if (pos.IsReturnedItemsOnly())
                    ordType = "REFUND";
                else if (pos.HasReturnedItems())
                    ordType = "EXCHANGE";

                if (ordType == "REFUND")
                {
                    Helper.WriteLog(string.Format("Skipping payment for order #{0} : {1}. Reason: Refund transaction.", (i + 1).ToString(), orderHdr.OrderHdrID), MessageType.INFO);
                    continue;
                }

                Helper.WriteLog(string.Format("Processing payment for order #{0} : {1}.", (i + 1).ToString(), orderHdr.OrderHdrID), MessageType.INFO);

                foreach (ReceiptHdr receiptHdr in receiptHdrs)
                {
                    if (receiptHdr.IsVoided) continue;
                    ReceiptDetCollection tmpDets = receiptHdr.ReceiptDetRecords();
                    receiptDets.AddRange(tmpDets);
                }

                decimal totalDetAmount = 0, detAmount = 0;
                foreach (OrderDet od in orderDets)
                {
                    if (od.IsVoided) continue;
                    if (od.Quantity >= 0) totalDetAmount += od.Amount;
                }
                totalDetAmount = Math.Round(totalDetAmount, 2, MidpointRounding.AwayFromZero);
                
                bool isExchange = false;
                if (ordType == "EXCHANGE")
                {
                    // IF Exchange transaction then get the payment value from sum of OrderDet.Amount, and divide it by num of payment type
                    isExchange = true;

                    if (receiptDets.Count != 0)
                    {
                        detAmount = Math.Round(totalDetAmount / receiptDets.Count, 2, MidpointRounding.AwayFromZero);
                    }
                }

                #region *) Get Rounding amount in OrderDet
                decimal roundingAmt = 0;
                foreach (OrderDet od in orderDets)
                {
                    if (od.ItemNo == POSController.ROUNDING_ITEM)
                    {
                        roundingAmt += od.Amount;
                    }
                }
                #endregion

                #region *) Compare total amount in OrderDet & ReceiptDet
                decimal totalRcpAmount = 0;
                foreach (ReceiptDet rd in receiptDets)
                {
                    totalRcpAmount += rd.Amount;
                }
                // If total tally, no need to add rounding amount anymore
                if (totalDetAmount == totalRcpAmount) roundingAmt = 0;
                #endregion

                #region *) Generate Payment rows
                for (int j = 0; j < receiptDets.Count; j++)
                {
                    ReceiptDet receiptDet = receiptDets[j];
                    
                    row = sheet.CreateRow(rowindex);
                    rowindex++;

                    row.CreateCell(0).SetCellValue(receiptDet.PaymentType);  // IDBANK
                    row.CreateCell(1).SetCellValue(receiptDet.ReceiptHdr.OrderHdr.MembershipNo);  // IDCUST
                    row.CreateCell(2).SetCellValue("");  // TEXTRMIT
                    row.CreateCell(3).SetCellValue("");  // TXTRMITREF
                    row.CreateCell(4).SetCellValue(receiptDet.ReceiptHdr.ReceiptDate.ToString("yyyyMMdd"));  // DATERMIT
                    row.CreateCell(5).SetCellValue(receiptDet.ReceiptHdr.ReceiptDate.ToString("yyyyMMdd"));  // DATEBUS
                    row.CreateCell(6).SetCellValue(receiptDet.PaymentType);  // CODEPAYM
                    row.CreateCell(7).SetCellValue("");  // IDRMIT

                    if (!isExchange)
                    {
                        row.CreateCell(8).SetCellValue(Math.Round(Convert.ToDouble(receiptDet.Amount + roundingAmt), 2, MidpointRounding.AwayFromZero));  // AMTRMIT
                    }
                    else
                    {
                        if (j < receiptDets.Count-1)
                            row.CreateCell(8).SetCellValue(Convert.ToDouble(detAmount));  // AMTRMIT
                        else
                            row.CreateCell(8).SetCellValue(Convert.ToDouble(totalDetAmount - (detAmount * j)));  // AMTRMIT
                    }

                    row.CreateCell(9).SetCellValue(receiptDet.ReceiptHdr.OrderHdr.Userfld5);  // IDINVCMTCH
                }
                #endregion
            }

            Helper.WriteLog("Saving xls file for Payment...", MessageType.INFO);
            using (FileStream fs = new FileStream(file, FileMode.Create, FileAccess.Write))
            {
                workbook.Write(fs);
            }
            Helper.WriteLog(string.Format("Save complete. File name: {0}", Path.GetFileName(file)), MessageType.INFO);

            Helper.WriteLog("Generating xls file for Payment completed successfully.", MessageType.INFO);
        }

        private static void GenerateOrderXLSRpt(OrderHdrCollection orderColl, string file)
        {
            Console.WriteLine();
            Helper.WriteLog("Generating xls file for Order...", MessageType.INFO);

            string template = ConfigurationManager.AppSettings["Order_TemplateFile"];
            if (string.IsNullOrEmpty(template))
            {
                throw new Exception("Template file for Order is not specified.");
            }
            if (!File.Exists(template))
            {
                throw new Exception("Template file for Order is not found.");
            }

            string exportFormat = ConfigurationManager.AppSettings["ExportFormat"];
            if (string.IsNullOrEmpty(exportFormat)) exportFormat = "ExcelNoFormatting";

            string CATRemarks_SvcCharge = ConfigurationManager.AppSettings["CATRemarks_SvcCharge"];
            string InclGST_OrderDet = ConfigurationManager.AppSettings["InclGST_OrderDet"];
            string InclGST_OrderSubTotal = ConfigurationManager.AppSettings["InclGST_OrderSubTotal"];

            DataTable dt = new DataTable();

            for (int i = 0; i < orderColl.Count; i++)
            {
                OrderHdr orderHdr = orderColl[i];

                Helper.WriteLog(string.Format("Processing order #{0} : {1}.", (i + 1).ToString(), orderHdr.OrderHdrID), MessageType.INFO);

                #region *) SQL String
                string sql = @"
                    SELECT oh.OrderHdrID OH_OrderHdrID, oh.OrderRefNo OH_OrderRefNo, oh.Discount OH_Discount, 
                        oh.InventoryHdrRefNo OH_InventoryHdrRefNo, oh.CashierID OH_CashierID, oh.PointOfSaleID OH_PointOfSaleID, 
                        oh.OrderDate OH_OrderDate, oh.GrossAmount OH_GrossAmount, oh.NettAmount OH_NettAmount, 
                        oh.DiscountAmount OH_DiscountAmount, oh.GST OH_GST, oh.IsVoided OH_IsVoided, oh.MembershipNo OH_MembershipNo, 
                        oh.Remark OH_Remark, oh.CreatedOn OH_CreatedOn, oh.CreatedBy OH_CreatedBy, 
                        oh.ModifiedOn OH_ModifiedOn, oh.ModifiedBy OH_ModifiedBy, oh.UniqueID OH_UniqueID, 
                        oh.userfld1 OH_userfld1, oh.userfld2 OH_userfld2, oh.userfld3 OH_userfld3, 
                        oh.userfld4 OH_userfld4, oh.userfld5 OH_userfld5, oh.userfld6 OH_userfld6, oh.userfld7 OH_userfld7, 
                        oh.userfld8 OH_userfld8, oh.userfld9 OH_userfld9, oh.userfld10 OH_userfld10, oh.userflag1 OH_userflag1, 
                        oh.userflag2 OH_userflag2, oh.userflag3 OH_userflag3, oh.userflag4 OH_userflag4, oh.userflag5 OH_userflag5, 
                        oh.userfloat1 OH_userfloat1, oh.userfloat2 OH_userfloat2, oh.userfloat3 OH_userfloat3, oh.userfloat4 OH_userfloat4, 
                        oh.userfloat5 OH_userfloat5, oh.userint1 OH_userint1, oh.userint2 OH_userint2, oh.userint3 OH_userint3, 
                        oh.userint4 OH_userint4, oh.userint5 OH_userint5, OH.GstAmount, 
                        od.OrderDetID OD_OrderDetID, od.ItemNo OD_ItemNo, od.OrderDetDate OD_OrderDetDate, od.Quantity OD_Quantity, 
                        od.UnitPrice OD_UnitPrice, od.Discount OD_Discount, od.Amount OD_Amount, od.GrossSales OD_GrossSales, 
                        od.IsFreeOfCharge OD_IsFreeOfCharge, od.CostOfGoodSold OD_CostOfGoodSold, od.IsPromo OD_IsPromo, 
                        od.PromoDiscount OD_PromoDiscount, od.PromoAmount OD_PromoAmount, od.IsPromoFreeOfCharge OD_IsPromoFreeOfCharge, 
                        od.IsVoided OD_IsVoided, od.IsSpecial OD_IsSpecial, od.Remark OD_Remark, od.OrderHdrID OD_OrderHdrID, 
                        od.IsPreOrder OD_IsPreOrder, od.GiveCommission OD_GiveCommission, od.InventoryHdrRefNo OH_InventoryHdrRefNo, 
                        od.IsExchange OD_IsExchange,  od.ExchangeDetRefNo OD_ExchangeDetRefNo, od.OriginalRetailPrice OD_OriginalRetailPrice, 
                        od.ModifiedOn OD_ModifiedOn, 
                        od.ModifiedBy OD_ModifiedBy, od.CreatedBy OD_CreatedBy, od.CreatedOn OD_CreatedOn, od.UniqueID OD_UniqueID, 
                        od.userfld1 OD_userfld1, 
                        od.userfld2 OD_userfld2, od.userfld3 OD_userfld3, od.userfld4 OD_userfld4, od.userfld5 OD_userfld5, 
                        od.userfld6 OD_userfld6, od.userfld7 OD_userfld7, od.userfld8 OD_userfld8, od.userfld9 OD_userfld9, 
                        od.userfld10 OD_userfld10, od.userflag1 OD_userflag1, od.userflag2 OD_userflag2, od.userflag3 OD_userflag3, 
                        od.userflag4 OD_userflag4, od.userflag5 OD_userflag5, od.userfloat1 OD_userfloat1, od.userfloat2 OD_userfloat2, 
                        od.userfloat3 OD_userfloat3, od.userfloat4 OD_userfloat4, od.userfloat5 OD_userfloat5, od.userint1 OD_userint1, 
                        od.userint2 OD_userint2, od.userint3 OD_userint3, od.userint4 OD_userint4, od.userint5 OD_userint5,
                        od.gstAmount OD_GSTAmount,       
                        mbr.FirstName MBR_FirstName, mbr.LastName MBR_LastName, mbr.NameToAppear MBR_NameToAppear, 
                        mbr.Gender MBR_Gender, mbr.DateOfBirth MBR_DateOfBirth, mbr.NRIC MBR_NRIC, mbr.Email MBR_Email, 
                        mbr.Block MBR_Block, mbr.BuildingName MBR_BuildingName, mbr.StreetName MBR_StreetName, 
                        mbr.UnitNo MBR_UnitNo, mbr.City MBR_City, mbr.Country MBR_Country, mbr.ZipCode MBR_ZipCode, 
                        mbr.Mobile MBR_Mobile, mbr.Office MBR_Office, mbr.Home MBR_Home, mbr.ExpiryDate MBR_ExpiryDate, 
                        mbr.Remarks MBR_Remarks, 
                        itm.ItemName ITM_ItemName, itm.Barcode ITM_Barcode, itm.CategoryName ITM_CategoryName, 
                        itm.RetailPrice ITM_RetailPrice, itm.FactoryPrice ITM_FactoryPrice, itm.Userfld1 ITM_Userfld1, 
                        itm.Attributes1 ITM_Attributes1, itm.Attributes2 ITM_Attributes2, itm.Attributes3 ITM_Attributes3,
                        itm.Attributes4 ITM_Attributes4, itm.Attributes5 ITM_Attributes5, itm.Attributes6 ITM_Attributes6, 
                        itm.Attributes7 ITM_Attributes7, itm.Attributes8 ITM_Attributes8, 
                        itm.IsInInventory as ITM_IsInInventory, itm.IsServiceItem ITM_IsServiceItem,
                        iSNULL(itm.Userfld9,'') as ITM_PointRedeemable, ISNULL(itm.Userfld10,'') as ITM_PointType, 
                        cat.Remarks CAT_Remarks, 
                        RD_PaymentType = STUFF((SELECT ', ' + rd.PaymentType 
                            FROM ReceiptDet rd 
                                INNER JOIN ReceiptHdr rh ON rh.ReceiptHdrID = rd.ReceiptHdrID 
                            WHERE rh.OrderHdrID = oh.OrderHdrID 
                            FOR XML PATH('')), 1, 2, ''), 
                        INCL_GST = '" + InclGST_OrderDet + @"' 
                    FROM OrderHdr oh 
                        INNER JOIN OrderDet od ON od.OrderHdrID = oh.OrderHdrID 
                        LEFT JOIN Membership mbr ON mbr.MembershipNo = oh.MembershipNo 
                        LEFT JOIN Item itm ON itm.ItemNo = od.ItemNo 
                        LEFT JOIN Category cat ON cat.CategoryName = itm.CategoryName 
                    WHERE oh.IsVoided = 0 AND od.IsVoided = 0 AND oh.OrderHdrID = '" + orderHdr.OrderHdrID + "'";
                #endregion

                QueryCommand cmd = new QueryCommand(sql, "PowerPOS");
                DataTable dtemp = DataService.GetDataSet(cmd).Tables[0];

                if (dtemp.Rows.Count > 0)
                {
                    DataRow firstRow = dtemp.Rows[0];

                    /*OrderSubTotalCollection ostColl = new OrderSubTotalCollection();
                    ostColl.Where("Deleted", false);
                    ostColl.Where("OrderHdrID", orderHdr.OrderHdrID);
                    ostColl.Load();

                    foreach (var ost in ostColl)
                    {
                        DataRow row = dtemp.NewRow();
                        row["OH_OrderRefNo"] = firstRow["OH_OrderRefNo"];
                        row["RD_PaymentType"] = firstRow["RD_PaymentType"];
                        row["ITM_ItemName"] = ost.Name;
                        row["OD_Amount"] = (!string.IsNullOrEmpty(ost.SubTotalType) && ost.SubTotalType.ToLower() == "discount" ? -ost.DiscDollar : ost.DiscDollar);
                        row["OH_OrderDate"] = firstRow["OH_OrderDate"];
                        row["CAT_Remarks"] = (!string.IsNullOrEmpty(ost.Name) && ost.Name.ToLower() == "svc charge" ? CATRemarks_SvcCharge : "");
                        row["OD_Quantity"] = 1;
                        row["INCL_GST"] = InclGST_OrderSubTotal;
                        dtemp.Rows.Add(row);
                    }*/

                    dt.Merge(dtemp, true, MissingSchemaAction.Add);
                }
            }

            Helper.WriteLog("Saving xls file for Order...", MessageType.INFO);

            ReportDocument rpt = new ReportDocument();
            rpt.Load(template);
            rpt.SetDataSource(dt);
            //rpt.PrintOptions.PrinterName = "Receipt";
            //rpt.PrintToPrinter(1, false, 0, 0);
            if (exportFormat == "ExcelWithFormatting")
                rpt.ExportToDisk(ExportFormatType.Excel, file);
            else
                rpt.ExportToDisk(ExportFormatType.ExcelRecord, file);

            Helper.WriteLog(string.Format("Save complete. File name: {0}", Path.GetFileName(file)), MessageType.INFO);

            Helper.WriteLog("Generating xls file for Order completed successfully.", MessageType.INFO);
        }

        //private static string GetMiddleDigitsFromSpecialBarcode(string specialBarcode)
        //{
        //    if (!string.IsNullOrEmpty(specialBarcode))
        //    {
        //        bool useSpecialBarcode = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.SpecialBarcode.UseSpecialBarcode), false);
        //        string checkstring = AppSetting.GetSetting(AppSetting.SettingsName.SpecialBarcode.BarcodeCheckValue).ToString();
        //        if (useSpecialBarcode && checkstring == "")
        //        {
        //            Helper.WriteLog("Barcode Check Value setting is missing.", MessageType.ERROR);
        //            return "";
        //        }

        //        if (useSpecialBarcode && specialBarcode.Substring(0, checkstring.Length) == checkstring)
        //        {
        //            int itemstart = Convert.ToInt16(AppSetting.GetSetting(AppSetting.SettingsName.SpecialBarcode.ItemDigitStart));
        //            int itemlength = Convert.ToInt16(AppSetting.GetSetting(AppSetting.SettingsName.SpecialBarcode.ItemDigitLength));
        //            int pricestart = Convert.ToInt16(AppSetting.GetSetting(AppSetting.SettingsName.SpecialBarcode.PriceDigitStart));
        //            int pricelength = Convert.ToInt16(AppSetting.GetSetting(AppSetting.SettingsName.SpecialBarcode.PriceDigitLength));

        //            int minLength = 0;
        //            if (itemstart > pricestart)
        //            {
        //                minLength = itemstart - 1 + itemlength;
        //            }
        //            else
        //            {
        //                minLength = pricestart - 1 + pricelength;
        //            }

        //            if (specialBarcode.Length < minLength)
        //            {
        //                Helper.WriteLog("Error. Special Barcode Length is Wrong. Please check the settings.", MessageType.ERROR);
        //                return "";
        //            }

        //            string result = "";
        //            try
        //            {
        //                if (itemstart < pricestart)
        //                    result = specialBarcode.Remove(pricestart - 1, pricelength).Remove(itemstart - 1, itemlength).Remove(0, checkstring.Length);
        //                else
        //                    result = specialBarcode.Remove(itemstart - 1, itemlength).Remove(pricestart - 1, pricelength).Remove(0, checkstring.Length);
        //            }
        //            catch (Exception ex)
        //            {
        //                Helper.WriteLog(string.Format("Error parsing the special barcode string : {0}.", specialBarcode), MessageType.ERROR);
        //                Helper.WriteLog(ex.ToString(), MessageType.ERROR);
        //            }

        //            return result;
        //        }
        //    }
        //    return "";
        //}
    }
}
