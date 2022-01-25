using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using PowerPOSLib.Container;
using Newtonsoft.Json;
using SubSonic;
using PowerPOS;
using PowerPOS.Container;
using System.Web.SessionState;

namespace PowerWeb.API.Sales.UpdateBalancePayment
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class Pay : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            JsonResult result = new JsonResult();
            string invoiceNo = context.Request.Params["InvoiceNo"] ?? "";
            string paymentMode = context.Request.Params["PaymentMode"] ?? "";
            string remark = context.Request.Params["Remark"] ?? "";
            decimal paymentAmount = Convert.ToDecimal(context.Request.Params["PaymentAmount"] ?? "0");
            decimal balancePayment = Convert.ToDecimal((context.Request.Params["BalancePayment"] ?? "0").ToString().Replace("$ ", ""));
            int pointOfSaleID = Convert.ToInt32(context.Request.Params["PointOfSaleID"] ?? "0");
            string membershipNo = context.Request.Params["MembershipNo"] ?? "";
            string username = context.Session["username"].ToString();

            try
            {
                QueryCommandCollection cmds = new QueryCommandCollection();
                QueryCommand cmdOrderHdr;
                QueryCommand cmdReceiptHdr;
                QueryCommand cmdOrderDet;
                QueryCommand cmdReceiptDet;

                OrderHdr refOH = new OrderHdr(invoiceNo);

                if (paymentAmount == 0)
                {
                    result.Status = false;
                    result.Message = "Payment amount cannot be zero";
                }
                else if (paymentAmount > balancePayment)
                {
                    result.Status = false;
                    result.Message = "Payment amount more than installment.";
                }
                else if (string.IsNullOrEmpty(refOH.OrderHdrID))
                {
                    result.Status = false;
                    result.Message = "Reference Invoice Number not found.";
                }
                else
                {
                    PowerPOS.OrderHdr orderHdr = new PowerPOS.OrderHdr();
                    orderHdr.IsNew = true;
                    orderHdr.OrderHdrID = (new POSController()).CreateNewOrderNoForWeb(pointOfSaleID);
                    orderHdr.OrderRefNo = "OR" + orderHdr.OrderHdrID;
                    orderHdr.Userfld5 = orderHdr.OrderRefNo;
                    orderHdr.Discount = 0;
                    orderHdr.CashierID = username;
                    orderHdr.PointOfSaleID = pointOfSaleID;
                    orderHdr.OrderDate = DateTime.Now;
                    orderHdr.GrossAmount = paymentAmount;
                    orderHdr.NettAmount = paymentAmount;
                    orderHdr.DiscountAmount = 0;
                    orderHdr.Gst = 7;
                    orderHdr.IsVoided = false;
                    orderHdr.MembershipNo = membershipNo;
                    orderHdr.UniqueID = Guid.NewGuid();
                    orderHdr.IsPointAllocated = true;
                    orderHdr.GSTAmount = 0;
                    orderHdr.Remark = "Reference: " + refOH.Userfld5;
                    orderHdr.Userfld10 = "ORDER_PAYMENT";

                    ReceiptHdr receiptHdr = new ReceiptHdr();
                    receiptHdr.IsNew = true;
                    receiptHdr.ReceiptHdrID = orderHdr.OrderHdrID;
                    receiptHdr.OrderHdrID = orderHdr.OrderHdrID;
                    receiptHdr.ReceiptRefNo = "RCP" + orderHdr.OrderHdrID;
                    receiptHdr.ReceiptDate = orderHdr.OrderDate;
                    receiptHdr.CashierID = orderHdr.CashierID;
                    receiptHdr.PointOfSaleID = orderHdr.PointOfSaleID;
                    receiptHdr.Amount = orderHdr.NettAmount;
                    receiptHdr.AmountBeforeRounding = 0;
                    receiptHdr.IsVoided = false;
                    receiptHdr.UniqueID = Guid.NewGuid();
                    receiptHdr.Remark = remark;
                    receiptHdr.Userfld10 = "ORDER_PAYMENT";

                    PowerPOS.OrderDet orderDet = new PowerPOS.OrderDet();
                    orderDet.IsNew = true;
                    orderDet.OrderDetID = orderHdr.OrderHdrID + ".0";
                    orderDet.OrderHdrID = orderHdr.OrderHdrID;
                    orderDet.ItemNo = "INST_PAYMENT";
                    orderDet.OrderDetDate = orderHdr.OrderDate;
                    orderDet.Quantity = 1;
                    orderDet.UnitPrice = paymentAmount;
                    orderDet.Discount = 0;
                    orderDet.Amount = orderDet.Quantity.GetValueOrDefault(0) * orderDet.UnitPrice;
                    orderDet.GrossSales = orderDet.Amount;
                    orderDet.IsPreOrder = false;
                    orderDet.IsVoided = false;
                    orderDet.IsSpecial = false;
                    orderDet.IsPromoFreeOfCharge = false;
                    orderDet.CostOfGoodSold = 0;
                    orderDet.IsFreeOfCharge = false;
                    orderDet.UniqueID = Guid.NewGuid();
                    orderDet.Userfld3 = invoiceNo;
                    orderDet.Userfld10 = "ORDER_PAYMENT";

                    ReceiptDet receiptDet = new ReceiptDet();
                    receiptDet.IsNew = true;
                    receiptDet.ReceiptDetID = receiptHdr.ReceiptHdrID + ".0";
                    receiptDet.ReceiptHdrID = receiptHdr.ReceiptHdrID;
                    receiptDet.PaymentType = paymentMode;
                    receiptDet.Amount = paymentAmount;
                    receiptDet.Change = 0;
                    receiptDet.UniqueID = Guid.NewGuid();
                    receiptDet.Userfld10 = "ORDER_PAYMENT";

                    cmdOrderHdr = orderHdr.GetInsertCommand(username);
                    cmdReceiptHdr = receiptHdr.GetInsertCommand(username);
                    cmdOrderDet = orderDet.GetInsertCommand(username);
                    cmdReceiptDet = receiptDet.GetInsertCommand(username);

                    cmds.Add(cmdOrderHdr);
                    cmds.Add(cmdReceiptHdr);
                    cmds.Add(cmdOrderDet);
                    cmds.Add(cmdReceiptDet);

                    string strSql = "update OrderHdr set userfld10 = 'ORDER_PAYMENT' where OrderHdrID = '" + orderHdr.OrderHdrID + "'";
                    QueryCommand cmdOrderHdrPost = new QueryCommand(strSql);
                    cmds.Add(cmdOrderHdrPost);

                    #region *) Update DepositAmount in OrderDet
                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Order.AutoAssignDepositUponInstPayment), false))
                    {
                        OrderDetCollection refOD = refOH.OrderDetRecords();
                        decimal remainingAmt = paymentAmount;
                        foreach (OrderDet od in refOD)
                        {
                            if (od.Amount == null) od.Amount = 0;
                            if (od.DepositAmount == null) od.DepositAmount = 0;

                            if (od.Amount > 0 && od.Amount > od.DepositAmount)
                            {
                                decimal discrepancy = od.Amount - od.DepositAmount;
                                if (discrepancy <= remainingAmt)
                                {
                                    od.DepositAmount += discrepancy;
                                    remainingAmt -= discrepancy;
                                }
                                else
                                {
                                    od.DepositAmount += remainingAmt;
                                    remainingAmt = 0;
                                }

                                // Update the DeliveryOrderDetail so that the OrderDet.DepositAmount will be included in sync to server.
                                // (see SyncClientController.SendDeliveryOrderToServer)
                                DeliveryOrderDetail dod = new DeliveryOrderDetail(DeliveryOrderDetail.Columns.OrderDetID, od.OrderDetID);
                                cmds.Add(dod.GetUpdateCommand(UserInfo.username));

                                if (remainingAmt <= 0) break;
                            }
                        }
                        cmds.AddRange(refOD.GetSaveCommands(UserInfo.username));
                    }
                    #endregion

                    DataService.ExecuteTransaction(cmds);

                    result.Status = true;
                    result.Message = "Payment has been processed.";
                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
                if (ex.InnerException != null)
                {
                    result.Details = ex.InnerException.Message;

                    if (ex.InnerException.InnerException != null)
                    {
                        result.Details += "\n" + ex.InnerException.InnerException.Message;
                    }
                }
            }

            context.Response.ContentType = "application/json";
            context.Response.Write(JsonConvert.SerializeObject(result));
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
