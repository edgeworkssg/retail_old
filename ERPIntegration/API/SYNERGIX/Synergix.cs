using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using ERPIntegration.API.SYNERGIX.response;
using ERPIntegration.API.SYNERGIX.request;
using PowerPOS;
using PowerPOS.Container;
using System.Data;
using SubSonic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Threading;
using System.Collections;
using RKLib.ExportData;
using System.Net.Mail;

namespace ERPIntegration.API.SYNERGIX
{
    class Synergix : IERPIntegration
    {
        public PowerPOS.POSController pos;
        public decimal totalDiskon = 0;
        public decimal totalPajak = 0;
        public decimal totalHargaBarang = 0;
        public int NoDetilTransaction;
        public string jsonstring;
        public string filePath;
        public string invoice_no;
        public string orderhdr_id;
        public int invoiceIndexProcess;
        public int paymentNo;
        public List<string> listInvoiceSent = new List<string>();
        public List<string> listInvoiceFail = new List<string>();
        public List<string> listInvoiceFailReason = new List<string>();

        struct MessageType
        {
            public static string INFO = "ERP_INTEGRATION_INFO";
            public static string ERROR = "ERP_INTEGRATION_ERROR";
        }

        #region IERPIntegration Members

        public Synergix()
        {

        }

        public bool DoStockTake(out string status)
        {
            status = "no implementation";
            return true;
        }

        #region Sync Order 
        public bool ExportOrderPayment(out string status)
        {
            DateTime StartDate;
            DateTime EndDate;
            string strStartDate = ConfigurationManager.AppSettings["StartDate"];
            string strEndDate = ConfigurationManager.AppSettings["EndDate"];

            if (!DateTime.TryParse(strStartDate, out StartDate) || string.IsNullOrEmpty(strStartDate))
            {
                DateTime startDate = DateTime.Today.AddDays(-1);
                strStartDate = startDate.ToString("yyyy-MM-dd HH:mm:ss");
                DateTime.TryParse(strStartDate, out StartDate);
            }

            if (!DateTime.TryParse(strEndDate, out EndDate) || string.IsNullOrEmpty(strEndDate))
            {
                DateTime endDate = DateTime.Today.Add(TimeSpan.Parse("23:59:59"));
                strEndDate = endDate.ToString("yyyy-MM-dd HH:mm:ss");
                DateTime.TryParse(strEndDate, out EndDate);
            }
            Logger.writeLog("Sale Sync Start Date :" + StartDate.ToString("yyyy-MM-dd HH:mm:ss"));
            Logger.writeLog("Sale Sync End Date :" + EndDate.ToString("yyyy-MM-dd HH:mm:ss"));

           

            bool result = processAllInvoice(StartDate, EndDate, out status);
            status = "";

            if (result)
            {
                //setCounter(0);
                AppSetting.SetSetting(AppSetting.SettingsName.Integration.NeedToSyncSale, "False");
            }
            
                
            return result;
        }

        private int getCurrentNumberOfRetry()
        {
            string tmpCounter = AppSetting.GetSetting("Synergix_SaleRetryTimes");
            int counter = 0;
            if (!int.TryParse(tmpCounter, out counter))
                counter = 0;
            return counter;
        }

        private void setCounter(int currentCounter)
        {
            //currentCounter++;
            AppSetting.SetSetting("Synergix_SaleRetryTimes", currentCounter.ToString());
        }

        bool processAllInvoice(DateTime StartTime, DateTime EndTime, out string status)
        {
            status = "";
            bool result = true;
            Sales sale = null;
            NoDetilTransaction = 1;

            try
            {
                string query = @"
                select oh.orderhdrid,oh.nettamount,oh.membershipno,
                    oh.OrderDate, oh.pointofsaleid as POSID, oh.userfld5 as ReceiptNo,oh.OrderRefNo,oh.NettAmount,oh.GrossAmount,
                    po.PointOfSaleName,po.userfld7 as detilaccount,po.userfld8 as namecode,
                    ISNULL(NULLIF(i.Barcode,' '),i.ItemNo) as ItemNo, i.ItemName ,
                    isnull(nullif(oh.isvoided,' '), od.isvoided) as IsVoided,oh.ModifiedOn,i.IsInInventory, isnull(i.userflag7,0) as IsNIP, 
                    od.OrderDetID,od.Amount,od.UnitPrice,od.userfloat4 as DiscountItem,od.GrossSales, od.Quantity, ISNULL(NULLIF(od.userfld4,' '),'') as LineInfo,
                    od.GSTAmount,ISNULL(nullif(od.userfld13,' '),'') as ReceiptRefund,(od.UnitPrice*od.Quantity+od.GSTAmount-od.Amount) as DiscountPerDetail,
                    rd.PaymentType,i.userfld1 as UOM,i.Attributes6 as UOM_TYPE,i.Attributes5 as ITEM_TYPE,oh.Remark as RemarkHeader,od.Discount as DiskonPerBarang,od.Remark as RemarkDetail,od.Amount as AmountDetail,i.ItemNo as ITEM_NO,i.userfld8                   
                from orderdet od
                inner join orderhdr oh on od.OrderHdrID = oh.OrderHdrID
                inner join item i on od.ItemNo = i.ItemNo
                inner join pointofsale po on po.pointofsaleid = oh.PointOfSaleID
                left join (
                    select receipthdrid, MAX(paymenttype) PaymentType from receiptdet group by ReceiptHdrID
                )rd on oh.OrderHdrID = rd.receipthdrid
                where oh.modifiedon between '{0}' and '{1}'
                    and ISNULL(oh.{2},0)=0
                    and oh.isvoided = 0 
                    and i.CategoryName != 'SYSTEM'
                    and oh.PointOfSaleId= {3}
                    order by oh.modifiedon asc,oh.OrderRefNo
                ";
                query = string.Format(query, StartTime.ToString("yyyy-MM-dd HH:mm:ss"), EndTime.ToString("yyyy-MM-dd HH:mm:ss"), OrderHdr.UserColumns.IsExported, ConfigurationManager.AppSettings["PosID"]);

                DataTable dt = DataService.GetDataSet(new QueryCommand(query)).Tables[0];

                if (dt.Rows.Count > 0)
                {
                    paymentNo = 1;
                    listInvoiceFail.Clear();
                    listInvoiceSent.Clear();
                    listInvoiceFailReason.Clear();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (invoice_no != dt.Rows[i]["ReceiptNo"].ToString())
                        {
                            if (sale == null)
                            {
                                invoice_no = dt.Rows[i]["ReceiptNo"].ToString();
                                orderhdr_id = dt.Rows[i]["OrderHdrId"].ToString();
                                invoiceIndexProcess = i;
                                //init invoice header and first invoice detil                                
                                sale = new Sales();
                                sale.ar_invoice_detail = new List<InvoiceDetail>();
                                sale.company_code = ConfigurationManager.AppSettings["company_code"];
                                InvoiceHeader hi;
                                if (createInvoiceHeader(dt.Rows[i], out hi, out status))
                                    sale.ar_invoice_header = hi;
                                else
                                    return false;

                                sale.ar_invoice_header = hi;


                                //=======================================
                                //masukkan detil pertama dari invoice baru
                                //=======================================

                                InvoiceDetail di;
                                if (createInvoiceDetail(dt.Rows[i], out di, out status, false))
                                    sale.ar_invoice_detail.Add(di);
                                else
                                    return false;
                            }
                            else
                            {
                                // add Rounding if exist
                                Decimal roundingVal;
                                string statusRounding;
                                if (findIsRounding(invoice_no, out statusRounding, out roundingVal))
                                {
                                    InvoiceDetail diRounding;
                                    createInvoiceDetailRounding(out diRounding, roundingVal);
                                    sale.ar_invoice_detail.Add(diRounding);
                                }

                                //================================
                                //send to server or save into file
                                //================================
                                if (totalDiskon < 0)
                                    totalDiskon = -1 * totalDiskon;
                                sale.ar_invoice_header.total_discount_amt = this.totalDiskon;
                                if (totalPajak < 0)
                                    totalPajak = -1 * totalPajak;
                                sale.ar_invoice_header.total_sales_tax_amt = decimal.Round(totalPajak, 2, MidpointRounding.AwayFromZero);
                                sale.ar_invoice_header.total_pre_tax_amt = decimal.Round(totalHargaBarang - totalPajak, 2, MidpointRounding.AwayFromZero);
                                sale.ar_invoice_header.total_after_tax_amt = decimal.Round(totalHargaBarang, 2, MidpointRounding.AwayFromZero);
                                sale.ar_invoice_header.payment_amt = decimal.Round(totalHargaBarang, 2, MidpointRounding.AwayFromZero);
                                //sale.ar_invoice_header.payment_no = paymentNo; paymentNo++;
                                jsonstring = JsonConvert.SerializeObject(sale);
                                filePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\invoice_error\\" + invoice_no + ".json";
                                if (sendOrderPaymentToServer(jsonstring, out status))                                
                                {
                                    setOrderHdrFlagAsExported(orderhdr_id, status);
                                    Helper.WriteLog(invoice_no + " successfully sent", MessageType.INFO);
                                    if (listInvoiceSent.IndexOf(invoice_no) == -1)
                                        listInvoiceSent.Add(invoice_no);
                                }
                                else
                                {
                                    File.WriteAllText(filePath, jsonstring);
                                    Helper.WriteLog(invoice_no + " fail sent", MessageType.ERROR);
                                    if (listInvoiceFail.IndexOf(invoice_no) == -1)
                                    {
                                        listInvoiceFail.Add(invoice_no);
                                        listInvoiceFailReason.Add(status);
                                    }
                                    //sendEmailError(dt.Rows, i - 1, status);
                                    //return false; ERROR THEN KEEP GOING
                                }
                                invoice_no = dt.Rows[i]["ReceiptNo"].ToString();
                                orderhdr_id = dt.Rows[i]["OrderHdrId"].ToString();
                                invoiceIndexProcess = i;
                                sale = null;
                                totalDiskon = 0;
                                totalPajak = 0;
                                totalHargaBarang = 0;
                                NoDetilTransaction = 1;

                                //invoice_no = dt.Rows[i]["ReceiptNo"].ToString();
                                sale = new Sales();
                                sale.ar_invoice_detail = new List<InvoiceDetail>();
                                sale.company_code = ConfigurationManager.AppSettings["company_code"];
                                InvoiceHeader hi;
                                if (createInvoiceHeader(dt.Rows[i], out hi, out status))
                                    sale.ar_invoice_header = hi;
                                else
                                    return false;

                                sale.ar_invoice_header = hi;
                                //=======================================
                                //masukkan detil pertama dari invoice baru
                                //=======================================

                                InvoiceDetail di;
                                if (createInvoiceDetail(dt.Rows[i], out di, out status, false))
                                    sale.ar_invoice_detail.Add(di);
                                else
                                    return false;

                            }
                        }
                        else
                        {
                            //=======================================
                            //masukkan detil 
                            //=======================================

                            InvoiceDetail di;
                            if (createInvoiceDetail(dt.Rows[i], out di, out status, true))
                                sale.ar_invoice_detail.Add(di);
                            else
                                return false;
                        }
                    }
                    // add Rounding if exist
                    Decimal roundingVal2;
                    string statusRounding2;
                    if (findIsRounding(invoice_no, out statusRounding2, out roundingVal2))
                    {
                        InvoiceDetail diRounding;
                        createInvoiceDetailRounding(out diRounding, roundingVal2);
                        sale.ar_invoice_detail.Add(diRounding);
                    }

                    //================================
                    //send to server or save into file
                    //================================
                    if (totalDiskon < 0)
                        totalDiskon = -1 * totalDiskon;
                    sale.ar_invoice_header.total_discount_amt = totalDiskon;
                    if (totalPajak < 0)
                        totalPajak = -1 * totalPajak;
                    sale.ar_invoice_header.total_sales_tax_amt = decimal.Round(totalPajak, 2, MidpointRounding.AwayFromZero);
                    sale.ar_invoice_header.total_pre_tax_amt = decimal.Round(totalHargaBarang - totalPajak, 2, MidpointRounding.AwayFromZero);
                    sale.ar_invoice_header.total_after_tax_amt = decimal.Round(totalHargaBarang, 2, MidpointRounding.AwayFromZero);
                    sale.ar_invoice_header.payment_amt = decimal.Round(totalHargaBarang, 2, MidpointRounding.AwayFromZero);
                    //sale.ar_invoice_header.payment_no = paymentNo; paymentNo++;
                    jsonstring = JsonConvert.SerializeObject(sale);
                    filePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\invoice_error\\" + invoice_no + ".json";
                    
                    if (sendOrderPaymentToServer(jsonstring, out status))
                    {
                        setOrderHdrFlagAsExported(orderhdr_id, status);
                        Helper.WriteLog(invoice_no + " successfully sent", MessageType.INFO);
                        if (listInvoiceSent.IndexOf(invoice_no) == -1)
                            listInvoiceSent.Add(invoice_no);
                    }
                    else
                    {
                        File.WriteAllText(filePath, jsonstring);
                        Helper.WriteLog(invoice_no + " fail sent", MessageType.ERROR);
                        if (listInvoiceFail.IndexOf(invoice_no) == -1)
                        {
                            listInvoiceFail.Add(invoice_no);
                            listInvoiceFailReason.Add(status);
                        }
                        //sendEmailError(dt.Rows, invoiceIndexProcess,status);
                        //return false;
                    }
                }
                else
                {
                    result = true;
                    status = "no transaction";
                    Helper.WriteLog("No Transaction found", MessageType.INFO);
                }
                if (listInvoiceFail.Count > 0)
                {
                    //Generate Report 
                    string filename = "";
                    GenerateTotalProductNotSentReport(StartTime, EndTime, ConfigurationManager.AppSettings["POSID"], out filename);

                    sendEmailError(dt.Rows, invoiceIndexProcess, status, filename);
                }

                return result;
            }
            catch (Exception ex)
            {
                status = ex.Message;
                return false;
            }

        }

        void setOrderHdrFlagAsExported(string invoice_no, string receipt_no)
        {
            OrderHdr oh = new OrderHdr(invoice_no);
            if (!oh.IsNew)
            {
                QueryCommandCollection cmdCol = new QueryCommandCollection();
                oh.IsExported = true;
                oh.Userfld1 = receipt_no;
                cmdCol.Add(oh.GetUpdateCommand("edgeworks"));
                DataService.ExecuteTransaction(cmdCol);
            }
        }

        bool GenerateTotalProductNotSentReport(DateTime startDate, DateTime endDate, string POSID , out string fileName)
        {
            fileName = "";

            try
            {
                string query = @"
                select ISNULL(NULLIF(i.Barcode,' '),i.ItemNo) as ItemNo, i.ItemName, sum(od.Quantity) as Quantity, sum(od.amount) as Amount               
                from orderdet od
                inner join orderhdr oh on od.OrderHdrID = oh.OrderHdrID
                inner join item i on od.ItemNo = i.ItemNo
                inner join pointofsale po on po.pointofsaleid = oh.PointOfSaleID
                where oh.modifiedon between '{0}' and '{1}'
                    and ISNULL(oh.{2},0)=0
                    and oh.isvoided = 0 
                    and i.CategoryName != 'SYSTEM'
                    and oh.PointOfSaleId= {3}
                    Group by ISNULL(NULLIF(i.Barcode,' '),i.ItemNo), i.ItemName 
                    order by ISNULL(NULLIF(i.Barcode,' '),i.ItemNo) 
                ";
                query = string.Format(query, startDate.ToString("yyyy-MM-dd HH:mm:ss"), endDate.ToString("yyyy-MM-dd HH:mm:ss"), OrderHdr.UserColumns.IsExported, POSID);

                DataTable dt = DataService.GetDataSet(new QueryCommand(query)).Tables[0];

                fileName = @"\ProductSalesReport\ProductSalesNotSent" + DateTime.Now.ToString("dd-MMM-yyyy-HHmmss") + ".xls";
                this.ExportToExcel(dt, fileName);
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog("Error CreatingExcel " + ex.Message);
                fileName = "";
                return false;
            }
        }

        private bool ExportToExcel(DataTable dt, string filepath)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                ArrayList headerName = new ArrayList();

                foreach (DataColumn Col in dt.Columns)
                    headerName.Add(Col.ColumnName);

                ArrayList ColumnList = new ArrayList();
                for (int m = 0; m < dt.Columns.Count; m++)
                {
                    ColumnList.Add(m);
                }
                RKLib.ExportData.Export a = new RKLib.ExportData.Export("Win");
                a.ExportDetails(dt, (int[])ColumnList.ToArray(typeof(int)),
                    (string[])headerName.ToArray(typeof(string)),
                    RKLib.ExportData.Export.ExportFormat.Excel, filepath);
                return true;
            }
            return false;
        }

        void sendEmailError(string modulename, string errordetil)
        {
            string subject = "NOTIFICATION FOR HAISIA INTEGRATION";
            StringBuilder strTextBody = new StringBuilder();

            strTextBody.Append("Dear Admin, " + Environment.NewLine);
            strTextBody.Append(" " + Environment.NewLine);
            strTextBody.Append("There are error detected when " + modulename + " to Haisia. Detail are " + Environment.NewLine);
            strTextBody.Append(" " + Environment.NewLine);
            strTextBody.Append("Server exception are :" + errordetil + Environment.NewLine);
            strTextBody.Append(" " + Environment.NewLine);
            strTextBody.Append("Please check the error. Thank you");

           

            MailSender.sendEmail(subject, strTextBody, "");

        }

        bool createInvoiceHeader(DataRow row, out InvoiceHeader hi, out string status)
        {
            hi = new InvoiceHeader();
            hi.invoice_no = row["ReceiptNo"].ToString();

            //validation
            DateTime dtOrder;
            if (!DateTime.TryParse(row["OrderDate"].ToString(), out dtOrder))
            {
                status = "Error. Cannot Parse OrderDate";
                return false;
            }

            decimal nettAmount;
            if (!decimal.TryParse(row["nettamount"].ToString(), out nettAmount))
            {
                status = "Error. Cannot Parse nettamount";
                return false;
            }

            hi.invoice_date = dtOrder.ToString("ddMMyyyy");
            string membershipno = row["membershipno"].ToString();
            if (membershipno == "WALK-IN")
                hi.customer_code = ConfigurationManager.AppSettings["walk_in_code"];//sementara
            else
                hi.customer_code = membershipno;
            hi.generate_receipt = true;
            if (row["PaymentType"].ToString().ToLower() == "installment")
                hi.generate_receipt = false;

            if (nettAmount == 0)
                hi.generate_receipt = false;

            
            hi.currency_code = ConfigurationManager.AppSettings["currency_code"];
            hi.exchange_rate = 1; //sementara
            hi.sales_tax_code = ConfigurationManager.AppSettings["sales_tax_code"];
            hi.external_remarks = row["RemarkHeader"].ToString();
            // hitung diskon,pajak dan harga
            decimal tmpDiskon;
            if (!decimal.TryParse(row["DiscountPerDetail"].ToString(), out tmpDiskon))
            {
                status = "Error. Cannot Parse OrderDate";
                return false;
            }
            if (tmpDiskon < 0)
                tmpDiskon = -1 * tmpDiskon;
            tmpDiskon = Decimal.Round(tmpDiskon, 2, MidpointRounding.AwayFromZero);
            totalDiskon = totalDiskon + tmpDiskon;
            decimal tmpPajak;
            if (!decimal.TryParse(row["GSTAmount"].ToString(), out tmpPajak))
            {
                status = "Error. Cannot Parse GSTAmount";
                return false;
            }
            totalPajak = totalPajak + Decimal.Round(tmpPajak, 2, MidpointRounding.AwayFromZero);
            decimal tmpHarga;
            if (!decimal.TryParse(row["UnitPrice"].ToString(), out tmpHarga))
            {
                status = "Error. Cannot Parse UnitPrice";
                return false;
            }

            decimal tmpAmount;
            if (!decimal.TryParse(row["AmountDetail"].ToString(), out tmpAmount))
            {
                status = "Error. Cannot Parse AmountDetail";
                return false;
            }
            totalHargaBarang = totalHargaBarang + tmpAmount;

            hi.total_discount_amt = 0;//sementara akumulatif
            hi.total_pre_tax_amt = 0;//sementara akumulatif
            hi.total_sales_tax_amt = 0;//sementara akumulatif

            
            hi.total_after_tax_amt = Decimal.Round(nettAmount, 2, MidpointRounding.AwayFromZero);//sementara akumulatif
            hi.payment_amt = Decimal.Round(nettAmount, 2, MidpointRounding.AwayFromZero);//sementara akumulatif
            status = "";
            return true;
        }

        void createInvoiceDetailRounding(out InvoiceDetail di, decimal roundingVal)
        {
            di = new InvoiceDetail();
            di.line_item_no = NoDetilTransaction; NoDetilTransaction++;
            di.item_type = "S";
            di.item_code = ConfigurationManager.AppSettings["rounding_item_code"]; ;
            di.item_remarks = "";
            di.qty = 1;
            di.uom_type = "B";
            di.pack_size_code = "";
            di.unit_price = roundingVal;
            di.base_extended_amt = roundingVal;
            di.discount_amt = 0;
            di.sales_tax_amt = 0;
            di.is_rounding = true;
            if (roundingVal < 0)
            {
                di.pre_tax_amt = -1 * roundingVal;
                di.dr_cr_type = "D";
            }
            else
            {
                di.pre_tax_amt = roundingVal;
                di.dr_cr_type = "C";
            }
            totalHargaBarang += roundingVal;

        }



        bool createInvoiceDetail(DataRow row, out InvoiceDetail di, out string status, bool akumulasiTotal)
        {
            di = new InvoiceDetail();
            di.line_item_no = NoDetilTransaction; NoDetilTransaction++;
            di.item_type = row["ITEM_TYPE"].ToString();
            di.item_remarks = row["RemarkDetail"].ToString();
            decimal qtyd;
            if (!decimal.TryParse(row["Quantity"].ToString(), out qtyd))
            {
                status = "Error. Cannot Parse Quantity";
                return false;
            }
            di.qty = Math.Round(qtyd,1);
            di.uom_type = row["UOM_TYPE"].ToString();
            bool isNIP;
            string isinv = row["IsNIP"].ToString();
            if (!bool.TryParse(isinv, out isNIP))
            {
                status = "Error. Cannot Parse IsInInventory";
                return false;
            }
            if (!isNIP)
            {
                di.item_code = row["ITEM_NO"].ToString();
                di.pack_size_code = "";
            }
            else
            {
                di.item_code = row["userfld8"].ToString();
                di.pack_size_code = row["UOM"].ToString();
            }
            decimal unitPrice;
            if (!decimal.TryParse(row["UnitPrice"].ToString(), out unitPrice))
            {
                status = "Error. Cannot Parse UnitPrice";
                return false;
            }
            di.unit_price = Decimal.Round(unitPrice, 2, MidpointRounding.AwayFromZero);

            decimal tmpAmount;
            if (!decimal.TryParse(row["AmountDetail"].ToString(), out tmpAmount))
            {
                status = "Error. Cannot Parse AmountDetail";
                return false;
            }
            if (akumulasiTotal)
                totalHargaBarang = totalHargaBarang + tmpAmount;

            di.base_extended_amt = Decimal.Round(unitPrice * qtyd, 2, MidpointRounding.AwayFromZero);//sementara unknown

            decimal tmpDiskon;
            if (!decimal.TryParse(row["DiscountPerDetail"].ToString(), out tmpDiskon))
            {
                status = "Error. Cannot Parse OrderDate";
                return false;
            }
            if (tmpDiskon < 0)
                tmpDiskon = -1 * tmpDiskon;
            //tmpDiskon = Decimal.Round(tmpDiskon, 2, MidpointRounding.AwayFromZero);
            di.discount_amt = tmpDiskon;
            if (akumulasiTotal)
                totalDiskon = totalDiskon + tmpDiskon;

            decimal gstAmount;
            if (!decimal.TryParse(row["GSTAmount"].ToString(), out gstAmount))
            {
                status = "Error. Cannot Parse GSTAmount";
                return false;
            }
            di.sales_tax_amt = gstAmount;// Decimal.Round(gstAmount, 2, MidpointRounding.AwayFromZero);
            if (akumulasiTotal)
                totalPajak = totalPajak + gstAmount;//Decimal.Round(gstAmount, 2, MidpointRounding.AwayFromZero);

            di.pre_tax_amt = di.base_extended_amt - di.discount_amt;// Decimal.Round(di.base_extended_amt - di.discount_amt, 2, MidpointRounding.AwayFromZero);
            string statusRounding;
            decimal roundval;
            findIsRounding(row["ReceiptNo"].ToString(), out statusRounding, out roundval);
            di.is_rounding = false;//
            di.dr_cr_type = "C";
            status = "";
            return true;
        }

        bool findIsRounding(string invoiceNo, out string statusRounding, out decimal roundVal)
        {
            //string query = "SELECT ReceiptNo,UnitPrice FROM OrderDet WHERE UserFld5='{0}'";
            string query = "SELECT b.unitprice, b.ItemNo FROM OrderHdr a,orderdet b WHERE a.UserFld5='{0}' and a.OrderHdrID=b.OrderHdrID and b.ItemNo='R0001'";
            query = string.Format(query, invoiceNo);
            DataTable dt = DataService.GetDataSet(new QueryCommand(query)).Tables[0];
            if (dt.Rows.Count > 0)
            {
                if (!decimal.TryParse(dt.Rows[0]["UnitPrice"].ToString(), out roundVal))
                {
                    statusRounding = "C";
                }
                if (roundVal < 0)
                    statusRounding = "D";
                else
                    statusRounding = "C";
                return true;
            }
            else
            {
                statusRounding = "C";
                roundVal = 0;
                return false;
            }
        }

        #endregion


        #region Product
        bool isProductValid(Product p, out string status)
        {
            if (String.IsNullOrEmpty(p.item_code)) { status = "item code is empty"; return false; }
            //else if (String.IsNullOrEmpty(p.base_uom)) { status = "base uom is empty"; return false; }
            //else if (String.IsNullOrEmpty(p.item_type)) { status = "item type is empty"; return false; }
            else if (String.IsNullOrEmpty(p.inventory_class)) { status = "inventory class is empty"; return false; }
            else { status = "ok"; return true; }
        }

        public bool IsItemInfoStillSame(Product p, List<Product_Price> prices)
        {
            try
            {
                Item item = new Item(p.item_code);
                if (item.IsNew)
                    return false;
                /*if (p.item_type == "I")
                            item.CategoryName = p.inventory_category;
                        else
                            item.CategoryName = "SERVICE";*/

                if (item.ItemName != p.item_desc + " " + p.base_uom)
                    return false;

                if (item.Userfld1 != p.base_uom)
                    return false;

                if (item.Barcode != (String.IsNullOrEmpty(p.item_barcode) ? p.item_code : p.item_barcode))
                    return false;

                if (item.CategoryName != getCategoryName(p.inventory_class, p.inventory_brand_desc)) 
                    return false;

                Product_Price price = findProductPrice(prices, p.item_code, "B", "");

                if (price != null && item.RetailPrice != price.unit_price)
                    return false;

                return IsNIPSame(p, prices);

                
            }
            catch (Exception ex)
            {
                Logger.writeLog("Validate Item Error." + ex.Message);
                return false;
            }
        }

        public bool IsNIPSame(Product p, List<Product_Price> prices)
        {
            try
            {
                for (int i = 0; i < p.pack_sizes.Count; i++)
                {
                    string nipcode = p.item_code + p.pack_sizes[i].pack_size_code;
                    string desc = p.item_desc + " " + p.pack_sizes[i].pack_size_code;
                    string fld8 = p.item_code;
                    decimal float5 = p.pack_sizes[i].qty_in_base_uom;

                    Item item = new Item(nipcode);
                    if (item.IsNew)
                        return false;

                    if (item.Deleted != p.is_suspend)
                        return false;

                    if (item.ItemName != desc)
                        return false;

                    
                    Product_Price price = findProductPrice(prices, p.item_code, "P", p.pack_sizes[i].pack_size_code);
                    if (price == null && item.RetailPrice != 0)
                        return false;

                    if (price != null && item.RetailPrice != price.unit_price)
                        return false;
                }
                

                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog("Validate Item Error." + ex.Message);
                return false;
            }
        }



        public bool ImportItem(out string status)
        {
            bool result = false;
            List<Product_Price> prices = new List<Product_Price>();

            if (getItemPrices(out prices, out status))
            {
                if (updateItem(prices, out status))
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            else
            {
                result = false;
            }

            if (!result)
            {
                sendEmailError("Import Product Item", status);
            }
            return result;
        }

        public bool updateItem(List<Product_Price> prices, out string status)
        {
            string respCall;
            string companyCode = ConfigurationManager.AppSettings["company_code"];
            List<string> listItemSuccess = new List<string>();
            List<string> listItemFail = new List<string>();
            List<string> listItemFailReason = new List<string>();

            string reqBody = @"
                {{
                     ""company_code"": ""{0}""                  
                }}
            ";
            reqBody = string.Format(reqBody, companyCode);



            if (Synergix_http.callAPI(Synergix_http.CommandType.POST, "products", reqBody, out respCall))
            {
                Products products = JsonConvert.DeserializeObject<Products>(respCall);
                if (products.products.Count > 0)
                {
                    try
                    {
                        QueryCommandCollection cmdCol = new QueryCommandCollection();
                        for (int i = 0; i < products.products.Count; i++)
                        {
                            Product p = products.products[i];

                            string errProdValid;
                            if (!isProductValid(p, out errProdValid))
                            {
                                listItemFail.Add(p.item_code);
                                listItemFailReason.Add(errProdValid);
                                continue;
                            }

                            if (IsItemInfoStillSame(p, prices))
                            {
                                //listItemFail.Add(p.item_code);
                                //listItemFailReason.Add(errProdValid);
                                continue;
                            }

                            Helper.WriteLog("Upsert Item" + p.item_code, MessageType.INFO);

                            upsertCategory(p);

                            Item item = new Item(p.item_code);
                            if (item.IsNew)
                            {
                                item.ItemNo = p.item_code;
                                item.UniqueID = Guid.NewGuid();
                            }
                            item.Userflag1 = false;
                            item.Userfld1 = p.base_uom;
                            item.Barcode = String.IsNullOrEmpty(p.item_barcode) ? p.item_code : p.item_barcode;// generateBarcode();
                            if (p.item_type == "I")
                                item.CategoryName = getCategoryName(p.inventory_class, p.inventory_brand_desc);
                            else
                                item.CategoryName = "SERVICE";
                            //item.MinimumPrice = 0;
                            item.ItemName = p.item_desc + " " + p.base_uom;
                            item.Attributes1 = p.item_type;
                            //item.Attributes5 = p.item_type;
                            item.Attributes6 = "B";
                            item.IsInInventory = true;
                            item.IsServiceItem = false;
                            item.PointGetAmount = 0;
                            item.PointGetMode = Item.PointMode.None;
                            item.PointRedeemAmount = 0;
                            item.GSTRule = 2;
                            item.Deleted = p.is_suspend;

                            Helper.WriteLog("Upsert Item 1" + p.item_code, MessageType.INFO);

                            Product_Price price = findProductPrice(prices, p.item_code, "B", "");
                            if (price == null)
                            {
                                //OPEN PRICE PRODUCT
                                item.RetailPrice = 0;// price.unit_price;
                                item.Attributes7 = null;// price.start_date;
                                item.Attributes8 = null;// price.end_date;
                                item.Attributes6 = "B";// price.uom_type;
                                item.Attributes5 = "I";// price.item_type;

                                item.IsInInventory = true;
                                item.IsServiceItem = true;
                                item.PointGetAmount = 0;
                                item.PointGetMode = Item.PointMode.None;
                                item.PointRedeemAmount = 0;
                                item.PointRedeemMode = Item.PointMode.None;
                                item.Userfloat3 = null; /// Course Breakdown Price

                            }
                            else
                            {

                                Helper.WriteLog("Upsert Item 2" + p.item_code, MessageType.INFO);
                                //ORDINARY PRODUCT
                                int resultCompare = 0;
                                if (!item.IsNew)
                                {
                                    if (item.Attributes8 != null && price.end_date != null)
                                    {
                                        DateTime dtExisting = convertToDatetime(item.Attributes7);
                                        DateTime dtProductprice = convertToDatetime(price.start_date);
                                        resultCompare = DateTime.Compare(dtProductprice, dtExisting);
                                    }
                                }

                                if (resultCompare >= 0)
                                {
                                    item.RetailPrice = price.unit_price;
                                    item.Attributes7 = price.start_date;
                                    item.Attributes8 = price.end_date;
                                    item.Attributes6 = price.uom_type;
                                    item.Attributes5 = price.item_type;
                                }
                            }

                            cmdCol.Clear();
                            if (item.IsNew)
                            {
                                cmdCol.Add(item.GetInsertCommand("edgeworks"));
                                Helper.WriteLog("Insert Item " + item.ItemNo, MessageType.INFO);
                            }
                            else
                            {
                                cmdCol.Add(item.GetUpdateCommand("edgeworks"));
                                Helper.WriteLog("Update Item " + item.ItemNo, MessageType.INFO);
                            }

                            try
                            {
                                DataService.ExecuteTransaction(cmdCol);
                                listItemSuccess.Add(p.item_code);
                                if (p.pack_sizes.Count > 0)
                                    upsertPackSize(p, prices);
                            }
                            catch (Exception e)
                            {
                                listItemFail.Add(p.item_code);
                                listItemFailReason.Add(e.Message);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        status = e.Message;
                        return false;
                    }
                }
                if (listItemFail.Count > 0)
                {
                    string err = "";
                    if (listItemSuccess.Count > 0)
                        err = "Succeded Imported Item are " + String.Join(",", listItemSuccess.ToArray()) + Environment.NewLine;
                    err = err + "Unsuccessfull Item are " + String.Join(",", listItemFail.ToArray()) + Environment.NewLine;
                    err = err + "Detail are: " + Environment.NewLine;
                    for (int i = 0; i < listItemFail.Count; i++)
                    {
                        err = err + "- " + listItemFail[i] + " : " + listItemFailReason[i] + Environment.NewLine;
                    }
                    sendEmailError("Import Product Item", err);
                }

                status = "ok";
                return true;
            }
            else
            {
                status = respCall;
                return false;
            }
        }

        public Product_Price findProductPrice(List<Product_Price> prices, string itemcode, string uom_type, string base_uom)
        {
            List<Product_Price> result = new List<Product_Price>();
            for (int i = 0; i < prices.Count; i++)
            {
                if (uom_type == "P")
                {
                    // LOOKING FOR NIP ITEM PRICE
                    if (itemcode == prices[i].item_code && prices[i].pack_size_code == base_uom && prices[i].uom_type == uom_type)
                    {
                        result.Add(prices[i]);
                    }
                }
                else
                {
                    // LOOKING FOR BASE ITEM PRICE
                    if (itemcode == prices[i].item_code && prices[i].uom_type == uom_type)
                    {
                        result.Add(prices[i]);
                    }
                }
            }
            if (result.Count == 0)
                return null;
            else if (result.Count == 1)
                return result[0];
            else
            {
                // There're more than 1 price,
                // Pick one the latest
                Product_Price latestPrices = result[0];
                for (int i = 1; i < result.Count; i++)
                {
                    DateTime dtLatest = convertToDatetime(latestPrices.start_date);
                    DateTime dtCurrent = convertToDatetime(result[i].start_date);
                    int resultCompare = DateTime.Compare(dtLatest, dtCurrent);
                    if (resultCompare < 0)
                        latestPrices = result[i];
                }
                return latestPrices;
            }
        }

        public bool getItemPrices(out List<Product_Price> result, out string status)
        {
            result = new List<Product_Price>();
            string respCall;
            string companyCode = ConfigurationManager.AppSettings["company_code"];
            string currencyCode = ConfigurationManager.AppSettings["currency_code"];

            string reqBody = @"
                {{
                     ""company_code"": ""{0}"",
                     ""currency_code"": ""{1}""                  
                }}
            ";
            reqBody = string.Format(reqBody, companyCode, currencyCode);

            if (Synergix_http.callAPI(Synergix_http.CommandType.POST, "product_pricings", reqBody, out respCall))
            {
                Product_Prices products_price = JsonConvert.DeserializeObject<Product_Prices>(respCall);
                if (products_price.product_pricings.Count > 0)
                {

                    result = (List<Product_Price>)products_price.product_pricings;

                }
                status = "ok";
                return true;
            }
            else
            {
                status = respCall;
                return false;
            }
        }

        public bool updateItemPrice(out string status)
        {
            string respCall;
            string companyCode = ConfigurationManager.AppSettings["company_code"];
            string currencyCode = ConfigurationManager.AppSettings["currency_code"];

            string reqBody = @"
                {{
                     ""company_code"": ""{0}"",
                     ""currency_code"": ""{1}""                  
                }}
            ";
            reqBody = string.Format(reqBody, companyCode, currencyCode);

            if (Synergix_http.callAPI(Synergix_http.CommandType.POST, "product_pricings", reqBody, out respCall))
            {
                Product_Prices products_price = JsonConvert.DeserializeObject<Product_Prices>(respCall);
                if (products_price.product_pricings.Count > 0)
                {
                    QueryCommandCollection cmdCol = new QueryCommandCollection();
                    for (int i = 0; i < products_price.product_pricings.Count; i++)
                    {
                        Product_Price p = products_price.product_pricings[i];

                        string id;
                        if (!string.IsNullOrEmpty(p.pack_size_code))
                            id = p.item_code + "_" + p.pack_size_code;
                        else
                            id = p.item_code;

                        cmdCol.Clear();
                        Item item = new Item(id);

                        if (!item.IsNew)
                        {
                            int resultCompare = 0;
                            if (item.Attributes8 != null && p.end_date != null)
                            {
                                DateTime dtExisting = convertToDatetime(item.Attributes8);
                                DateTime dtProductprice = convertToDatetime(p.end_date);
                                resultCompare = DateTime.Compare(dtProductprice, dtExisting);
                            }

                            if (resultCompare >= 0)
                            {
                                item.RetailPrice = p.unit_price;
                                item.Attributes7 = p.start_date;
                                item.Attributes8 = p.end_date;
                                item.Attributes6 = p.uom_type;
                                item.Attributes5 = p.item_type;

                                cmdCol.Add(item.GetUpdateCommand("edgeworks"));
                                DataService.ExecuteTransaction(cmdCol);
                            }
                        }
                    }
                }
                status = "ok";
                return true;
            }
            else
            {
                status = respCall;
                return false;
            }
        }


        void sendEmailError(DataRowCollection Rows, int indexFail, string status_error, string attachmentFileName)
        {
            List<string> listNotSend = new List<string>();
            string invoiceFail = String.Join(",", listInvoiceFail.ToArray());
            for (int i = 0; i < Rows.Count; i++)
            {
                if (listInvoiceFail.IndexOf(Rows[i]["ReceiptNo"].ToString()) == -1)
                {
                    if (listInvoiceSent.IndexOf(Rows[i]["ReceiptNo"].ToString()) == -1)
                    {
                        if (listNotSend.IndexOf(Rows[i]["ReceiptNo"].ToString()) == -1)
                            listNotSend.Add(Rows[i]["ReceiptNo"].ToString());
                    }
                }
            }
            string Subject = "NOTIFICATION FOR HAISIA INTEGRATION";
            StringBuilder strTextBody = new StringBuilder();

            strTextBody.Append("Dear Admin, " + Environment.NewLine);
            strTextBody.Append(" " + Environment.NewLine);
            strTextBody.Append("There are error detected when send data to Haisia. Detail are " + Environment.NewLine);
            if (listInvoiceSent.Count > 0)
                strTextBody.Append("- Invoice success to send " + String.Join(",", listInvoiceSent.ToArray()) + Environment.NewLine);
            strTextBody.Append("- Invoice fail to send " + invoiceFail + Environment.NewLine);
            if (listNotSend.Count > 0)
                strTextBody.Append("- Invoice not processed yet " + String.Join(",", listNotSend.ToArray()) + Environment.NewLine);
            strTextBody.Append(" " + Environment.NewLine);
            strTextBody.Append("Server exception are :" + Environment.NewLine);
            for (int ir = 0; ir < listInvoiceFail.Count; ir++)
            {
                strTextBody.Append("- " + listInvoiceFail[ir] + " : " + listInvoiceFailReason[ir] + Environment.NewLine);
            }
            strTextBody.Append(" " + Environment.NewLine);
            strTextBody.Append("Please check the error. Thank you");

            MailSender.sendEmail(Subject, strTextBody,attachmentFileName);
        }


        #endregion


        public bool ExportInventory(out string status)
        {
            status = "no implementation";
            return true;
        }

        public bool ImportInventory(out string status)
        {
            status = "no implementation";
            return true;
        }

        

        private DateTime convertToDatetime(string dts)
        {
            if (String.IsNullOrEmpty(dts))
                return new DateTime(2010, 01, 01);


            string d = dts.Substring(0, 2);
            string m = dts.Substring(2, 2);
            string y = dts.Substring(4, 4);
            int di, mi, yi;
            int.TryParse(d, out di);
            int.TryParse(m, out mi);
            int.TryParse(y, out yi);
            return new DateTime(yi, mi, di);
        }

        public void upsertPackSize(Product p, List<Product_Price> prices)
        {
            QueryCommandCollection cmdCol = new QueryCommandCollection();
            for (int i = 0; i < p.pack_sizes.Count; i++)
            {


                string nipcode = p.item_code + p.pack_sizes[i].pack_size_code;
                string desc = p.item_desc + " " + p.pack_sizes[i].pack_size_code;
                string fld8 = p.item_code;
                decimal float5 = p.pack_sizes[i].qty_in_base_uom;

                Item item = new Item(nipcode);
                if (item.IsNew)
                {
                    item.ItemNo = nipcode;
                    item.UniqueID = Guid.NewGuid();
                }
                item.Barcode = nipcode;
                item.IsInInventory = false;
                item.IsServiceItem = false;
                item.NonInventoryProduct = true;
                item.PointGetAmount = 0;
                item.PointGetMode = Item.PointMode.None;
                item.PointRedeemAmount = 0;
                item.Userfloat3 = null; /// Course Breakdown Price
                item.DeductedItem = p.item_code;
                item.Userfld8 = p.item_code;
                item.Userfld1 = p.pack_sizes[i].pack_size_code;
                item.Userfloat5 = p.pack_sizes[i].qty_in_base_uom;
                item.Attributes1 = p.inventory_brand;
                item.Attributes5 = p.item_type;
                item.Attributes6 = "P";
                item.GSTRule = 1;

                //if (p.item_type == "I")
                item.CategoryName = getCategoryName(p.inventory_class, p.inventory_brand_desc);
                //else
                //    item.CategoryName = "SERVICE";

                item.ItemName = desc;
                item.Deleted = p.is_suspend;

                Product_Price price = findProductPrice(prices, p.item_code, "P", p.pack_sizes[i].pack_size_code);
                if (price == null)
                {
                    //OPEN PRICE ITEM
                    item.RetailPrice = 0;// price.unit_price;
                    item.Attributes7 = null;// price.start_date;
                    item.Attributes8 = null;// price.end_date;
                    item.Attributes6 = "B";// price.uom_type;
                    item.Attributes5 = "I";// price.item_type;

                    item.IsInInventory = true;
                    item.IsServiceItem = true;
                    item.PointGetAmount = 0;
                    item.PointGetMode = Item.PointMode.None;
                    item.PointRedeemAmount = 0;
                    item.PointRedeemMode = Item.PointMode.None;
                    item.Userfloat3 = null; /// Course Breakdown Price
                }
                else
                {
                    //ORDINARY NIP 
                    int resultCompare = 0;
                    if (!item.IsNew)
                    {
                        if (item.Attributes8 != null && price.end_date != null)
                        {
                            DateTime dtExisting = convertToDatetime(item.Attributes8);
                            DateTime dtProductprice = convertToDatetime(price.end_date);
                            resultCompare = DateTime.Compare(dtProductprice, dtExisting);
                        }
                    }

                    if (resultCompare >= 0)
                    {
                        item.RetailPrice = price.unit_price;
                        item.Attributes7 = price.start_date;
                        item.Attributes8 = price.end_date;
                        item.Attributes6 = price.uom_type;
                        item.Attributes5 = price.item_type;
                    }
                }

                cmdCol.Clear();
                if (item.IsNew)
                    cmdCol.Add(item.GetInsertCommand("edgeworks"));
                else
                    cmdCol.Add(item.GetUpdateCommand("edgeworks"));
                DataService.ExecuteTransaction(cmdCol);
            }
        }

        public int upsertMembershipGroup(Customer c)
        {
            if (string.IsNullOrEmpty(c.customer_group_code))
                return -1;

            //check is membergroup exist
            Query qr = new Query("MembershipGroup");
            qr.AddWhere(MembershipGroup.Columns.GroupName, c.customer_group_code);
            qr.QueryType = QueryType.Select;
            DataSet ds = qr.ExecuteDataSet();
            if (ds.Tables[0].Rows.Count == 0)
            {
                QueryCommandCollection cmdCol = new QueryCommandCollection();
                MembershipGroup mg = new MembershipGroup();
                if (mg.IsNew)
                {
                    mg.IsNew = true;
                    mg.GroupName = c.customer_group_code;
                    mg.CreatedBy = "edgeworks";
                    mg.CreatedOn = DateTime.Now;
                    mg.ModifiedBy = "edgeworks";
                    mg.ModifiedOn = DateTime.Now;
                    mg.Deleted = false;

                    cmdCol.Add(mg.GetInsertCommand("edgeworks"));
                    DataService.ExecuteTransaction(cmdCol);

                    Query qr2 = new Query("MembershipGroup");
                    qr2.AddWhere(MembershipGroup.Columns.GroupName, c.customer_group_code);
                    qr2.QueryType = QueryType.Select;
                    DataSet ds2 = qr.ExecuteDataSet();
                    string s2 = ds2.Tables[0].Rows[0][MembershipGroup.Columns.MembershipGroupId].ToString();
                    int r2 = int.Parse(s2);
                    return r2;
                }
                else
                    return mg.MembershipGroupId;
            }
            else
            {
                string s = ds.Tables[0].Rows[0][MembershipGroup.Columns.MembershipGroupId].ToString();
                int r = int.Parse(s);
                return r;
            }
        }

        public void upsertItemDepartment(Product p)
        {
            if (p.inventory_class == null)
                return;

            QueryCommandCollection cmdCol = new QueryCommandCollection();
            ItemDepartment itemDepart = new ItemDepartment(getItemDeptName(p.inventory_class, p.inventory_category));

            if (!itemDepart.IsNew)
                return;
            
            itemDepart.ItemDepartmentID = getItemDeptName(p.inventory_class, p.inventory_category);
            itemDepart.DepartmentName = getItemDeptName(p.inventory_class, p.inventory_category);
            itemDepart.Deleted = false;

            if (itemDepart.IsNew)
                cmdCol.Add(itemDepart.GetInsertCommand("edgeworks"));
            else
                cmdCol.Add(itemDepart.GetUpdateCommand("edgeworks"));
            DataService.ExecuteTransaction(cmdCol);
        }

        private string getItemDeptName(string inventoryclass, string inventorycategory)
        {
            if (String.IsNullOrEmpty(inventorycategory))
                return inventoryclass;

            return inventoryclass + "-" + inventorycategory;
        }

        private string getCategoryName(string inventoryclass, string brandName)
        {
            if (String.IsNullOrEmpty(brandName))
                return inventoryclass;

            return inventoryclass + "-" + brandName;
        }

        public void upsertCategory(Product p)
        {
            if (string.IsNullOrEmpty(p.inventory_class) && string.IsNullOrEmpty(p.inventory_brand_desc))
                return;

            upsertItemDepartment(p);

            QueryCommandCollection cmdCol = new QueryCommandCollection();

            Category cat = new Category(getCategoryName(p.inventory_class, p.inventory_brand_desc));

            if (!cat.IsNew)
                return;

            if (p.item_type == "I")
            {
                cat.CategoryName = getCategoryName(p.inventory_class, p.inventory_brand_desc);
                cat.CategoryId = getCategoryName(p.inventory_class, p.inventory_brand_desc);
                cat.Deleted = false;
                cat.ItemDepartmentId = getItemDeptName(p.inventory_class, p.inventory_category);
                
            }
            /*else
            {
                cat.CategoryName = "SERVICE";
                cat.CategoryId = "SERVICE";
                cat.Deleted = false;
                cat.ItemDepartmentId = getItemDeptName(p.inventory_class, p.inventory_category);
            }*/
            if (cat.IsNew)
                cmdCol.Add(cat.GetInsertCommand("edgeworks"));
            else
                cmdCol.Add(cat.GetUpdateCommand("edgeworks"));
            DataService.ExecuteTransaction(cmdCol);

        }

        public bool ImportMember(out string status)
        {
            string respCall;

            string companyCode = ConfigurationManager.AppSettings["company_code"];
            string currencyCode = ConfigurationManager.AppSettings["currency_code"];
            string taxCode = ConfigurationManager.AppSettings["sales_tax_code"];

            string reqBody = @"
                {{
                     ""company_code"": ""{0}"",
                     ""currency_code"": ""{1}"",
                     ""sales_tax_code"": ""{2}""                    
                }}
            ";
            reqBody = string.Format(reqBody, companyCode, currencyCode, taxCode);

            string nric = "";
            string email = "";
            string mobNumber = "";
            string code = "";
            string duplicate = "";

            if (Synergix_http.callAPI(Synergix_http.CommandType.POST, "customers", reqBody, out respCall))
            {
                Customers customers = JsonConvert.DeserializeObject<Customers>(respCall);
                MembershipController memberCtl = new MembershipController();
                for (int i = 0; i < customers.customers.Count; i++)
                {
                    try
                    {
                        Customer c = customers.customers[i];
                        code = c.customer_code;
                        if (c.contacts.Count > 0)
                            if (c.contacts[0].phone_numbers.Count > 0)
                                mobNumber = c.contacts[0].phone_numbers[0];
                        bool nricExists = (!string.IsNullOrEmpty(nric) && MembershipController.IsNRICAlreadyExist(nric, out duplicate));
                        bool emailExists = (!string.IsNullOrEmpty(email) && MembershipController.IsEmailAlreadyExist(email, out duplicate));

                        int groupId = upsertMembershipGroup(c);

                        Membership member = new Membership(c.customer_code);
                        //member.MembershipNo = MembershipController.getNewMembershipNo("E");
                        member.MembershipNo = c.customer_code;
                        member.NameToAppear = c.customer_name;
                        member.FirstName = c.customer_name;
                        member.Email = c.customer_code;
                        member.SubscriptionDate = new DateTime(2010,12,01);
                        member.ExpiryDate = new DateTime(2100, 12, 01);
                        member.Deleted = false;
                        member.IsCreatedInWeb = false;
                        if (groupId != -1)
                            member.MembershipGroupId = groupId;
                        else
                            member.MembershipGroupId = int.Parse(ConfigurationManager.AppSettings["default_member_group"]);
                        member.UniqueID = Guid.NewGuid();
                        if (c.is_suspend)
                            member.Block = "1";
                        else
                            member.Block = "0";

                        //if (!nricExists && !emailExists)
                        if (member.IsNew)
                            DataService.ExecuteQuery(member.GetInsertCommand("edgeworks"));
                        else
                            DataService.ExecuteQuery(member.GetUpdateCommand("edgeworks"));
                    }
                    catch (Exception e)
                    {
                        sendEmailError("Import Customer Data", e.Message);
                    }
                }

                status = "";
                return true;
            }
            else
            {
                status = "FAIL CALL API : " + respCall;
                sendEmailError("Import Customer Data", status);
                return false;
            }
        }

        public bool sendOrderPaymentToServer(string json, out string status)
        {
            bool result = false;
            status = "";
            string baseUrl = ConfigurationManager.AppSettings["synergix_server"]
                + "submit_ar_invoice_receipt?securityToken="
                + ConfigurationManager.AppSettings["synergix_token"]
                ;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(baseUrl);
            byte[] bytes;
            bytes = System.Text.Encoding.ASCII.GetBytes(json);
            request.ContentType = "application/json";
            request.ContentLength = bytes.Length;
            request.Method = "POST";

            HttpWebResponse response;
            try
            {
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
                response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream responseStream = response.GetResponseStream();
                    string responseStr = new StreamReader(responseStream).ReadToEnd();
                    status = responseStr;
                    OrderPaymentSucces osuccess = JsonConvert.DeserializeObject<OrderPaymentSucces>(status);
                    OrderPaymentError oerror = JsonConvert.DeserializeObject<OrderPaymentError>(status);
                    if (osuccess.invoice_no != null)
                    {
                        result = true;
                        status = osuccess.receipt_no;
                    }
                    else
                    {
                        result = false;
                        if (oerror.error_message != null)
                        {
                            status = String.Join(",", oerror.error_message.ToArray<string>());
                        }
                    }
                }
                else
                    status = "";
            }
            catch (WebException ex)
            {
                string errorweb;
                using (var stream = ex.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    errorweb = reader.ReadToEnd();
                }
                OrderPaymentError oerror = JsonConvert.DeserializeObject<OrderPaymentError>(errorweb);
                if (oerror.error_message != null)
                    status = String.Join(",", oerror.error_message.ToArray<string>());
                else
                    status = errorweb;
                result = false;
            }
            catch (Exception e)
            {
                status = e.Message;
                return false;
            }
            return result;
        }

        public bool Connect(out string status)
        {
            status = "no implementation";
            return true;
        }

        public bool checkStatus(out string status)
        {
            status = "no implementation";
            return true;
        }


        public bool ExportOrderRpt(out string status)
        {
            status = "no implementation";
            return true;
        }

        public bool ExportGoodsOrder(out string status)
        {
            status = "no implementation";
            return true;
        }

        public bool ExportStockReturn(out string status)
        {
            status = "no implementation";
            return true;
        }

        public bool ImportOrderApproval(out string status)
        {
            status = "no implementation";
            return true;
        }

        #endregion

        private string generateBarcode()
        {
            int runningNo = 0;
            int length = 1;
            string result = "";

            string lastBarcode = AppSetting.GetSetting(AppSetting.SettingsName.Item.LastBarcodeGenerated);
            if (!string.IsNullOrEmpty(lastBarcode))
                length = lastBarcode.Length;

            int.TryParse(lastBarcode, out runningNo);

            string prefix = AppSetting.CastString(AppSetting.GetSetting(AppSetting.SettingsName.Item.BarcodePrefix));

            while (true)
            {
                runningNo += 1;
                result = prefix + runningNo.ToString().PadLeft(length, '0');
                Item itm = new Item(Item.Columns.Barcode, result);
                if (itm == null || string.IsNullOrEmpty(itm.Barcode) || itm.Barcode != result)
                    break;
            }
            return result;
        }

        private void UpdateLastGeneratedBarcode()
        {

        }
    }


}
