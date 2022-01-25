using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PowerPOS;
using Microsoft.Reporting.WinForms;
using System.Data;
using PowerPOS.Container;
using System.Drawing;
using System.IO;
using System.Configuration;
using GenericReport;

namespace GenericReport
{
    public class PageTypeA5 : PageTemplate
    {
        POSController pos;
        Report frmReport;
        string strReportLogoName;
        string strLogoPath;
        Image imgLogo;
        Textbox txtReportType;
        bool bAutoPrintReport;

        // content
        Table tblOrderDetails;
        Table tblPaymentDetails;
        Table tblHeaderLeft;
        Table tblHeaderRight;
        Table tblFooter;

        Textbox txtTotal;
        Textbox txtTotalValue;
        Textbox txtCompanyName;

        // headers
        DataTable hdrOrderDetails;
        DataTable hdrPaymentDetails;
        DataTable hdrHeaderLeft;
        DataTable hdrHeaderRight;
        DataTable hdrFooter;

        // column formats
        List<ColumnFormat> cfmtOrderDetails;
        List<ColumnFormat> cfmtPaymentDetails;
        List<ColumnFormat> cfmtHeaderLeft;
        List<ColumnFormat> cfmtHeaderRight;
        List<ColumnFormat> cfmtFooter;

        #region *) Definition: Table Column Name, Size, and Data
        // tblOrderDetails
        List<String> listColumnsOrderDetails;
        string[] ColumnsOrderDetails = { "ITEM CODE", "DESCRIPTION", "QTY", "DISC (%)", "UNIT PRICE", "LINE TOTAL" };
        double[] WidthOrderDetails = { 2.0, 6.00, 0.7, 1.00, 1.10, 1.5 };
        ElementPosition posOrderDetails;
        TableFormat tblfmtOrderDetails;
        
        // tblPaymentDetails
        List<String> listColumnsPaymentDetails;
        string[] ColumnsPaymentDetails = {"PAYMENT", "AMOUNT"};
        double[] WidthPaymentDetails = {1.45, 1.3 };
        ElementPosition posPaymentDetails;
        TableFormat tblfmtPaymentDetails;

        // tblFooter
        List<String> listColumnsFooter;
        string[] ColumnsFooter = {"Terms & Conditions"};
        double[] WidthFooter = { 7.5 };
        ElementPosition posFooter;
        TableFormat tblfmtFooter;

        // tblHeaderRight
        List<String> listColumnsHeaderRight;
        string[] ColumnsHeaderRight = { "Data", "Value" };
        double[] WidthHeaderRight = { 1.7, 3.15 };
        ElementPosition posHeaderRight;
        TableFormat tblfmtHeaderRight;

        // tblHeaderLeft
        List<String> listColumnsHeaderLeft;
        string[] ColumnsHeaderLeft = { "Value" };
        double[] WidthHeaderLeft = { 4.75 };
        ElementPosition posHeaderLeft;
        TableFormat tblfmtHeaderLeft;

        #endregion

        #region *) Definition: Size and Positions
        private double vFontSize = 6;

        #region -) Report Type
        private double fsizeReportType = 10;
        private double posReportType_X = 8.00;
        private double posReportType_Y = 0.75;
        private double sizeReportType_Height = 0.50;
        private double sizeReportType_Width = 2.5;
        #endregion
        #region -) Total Amount
        private double sizeTotalAmount_X = 9.70;
        private double sizeTotalAmount_Y = 3.75;
        private double sizeTotalAmount_Height = 0.50;
        private double sizeTotalAmount_WidthLabel = 1.1;
        private double sizeTotalAmount_WidthAmount = 1.5;
        #endregion
        #endregion

        // functions
        public override void CreateReport(POSController posController, bool reprint, string reportType)
        {
            pos = posController;
            bReprint = reprint;

            // prepare tables
            FillReportTables();
            SetTableColumnWidth();
            SetTablePosition();
            SetAllTableFormat();
            FormatAllTableColumns();
            AddReportLogo();
            
            listPageBodyTables.Add(tblOrderDetails);            
            listPageBodyTables.Add(tblHeaderLeft);
            listPageBodyTables.Add(tblHeaderRight);
            listPageBodyTables.Add(tblFooter);
            listPageBodyTables.Add(tblPaymentDetails);

            if (!string.IsNullOrEmpty(reportType))
            {
                txtReportType.TextName = "txtReportType";
                txtReportType.TextValue = reportType;
                txtReportType.TextWidth = sizeReportType_Width;
                txtReportType.TextHeight = sizeReportType_Height;
                txtReportType.posTxt.x = posReportType_X;
                txtReportType.posTxt.y = posReportType_Y;
                txtReportType.cellFormat.FontSize = fsizeReportType;
                txtReportType.cellFormat.FontColor = Color.Black;
                txtReportType.cellFormat.FontWeight = "Bold";
                txtReportType.cellFormat.TextAlignment = "Right";
                txtReportType.cellFormat.BorderStyle = BorderStyle.None;

                listPageTextboxes.Add(txtReportType);
            }

            if (bReprint)
                listPageTextboxes.Add(txtReprint);
            if (pos.IsVoided())
                listPageTextboxes.Add(txtVoided);

            InitializePageCreation();
            if (imgLogo != null)
                page.AddImageToPage(imgLogo);
            
            CreatePage();
            ShowReport(frmReport.reportViewer);
            
            if (bAutoPrintReport == true)
            {
                ReportPrintDocument print = new ReportPrintDocument(frmReport.reportViewer.LocalReport);
                //print.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("A5", 148, 21);
                //print.PrinterSettings.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("A5", 148, 21);
                print.Print();
            }
            else
                frmReport.ShowDialog();
        }

        private void FillReportTables()
        {
            DataTable dtOrderDetails = new DataTable();
            DataTable dtPaymentDetails = new DataTable();
            DataTable dtHeaderLeft = new DataTable();
            DataTable dtHeaderRight = new DataTable();
            DataTable dtFooter = new DataTable();

            #region Fill Section Header
            DataRow drHeader = null;
            drHeader = hdrOrderDetails.NewRow();
            for (int i = 0; i < ColumnsOrderDetails.Length; i++)               
                drHeader[i] = ColumnsOrderDetails[i];
            
            hdrOrderDetails.Rows.Add(drHeader);

            drHeader = hdrPaymentDetails.NewRow();
            for (int i = 0; i < ColumnsPaymentDetails.Length; i++)            
                drHeader[i] = ColumnsPaymentDetails[i];
            
            hdrPaymentDetails.Rows.Add(drHeader);

            drHeader = hdrHeaderRight.NewRow();
            for (int i = 0; i < ColumnsHeaderRight.Length; i++)        
                drHeader[i] = ColumnsHeaderRight[i];
            
            hdrHeaderRight.Rows.Add(drHeader);

            drHeader = hdrHeaderLeft.NewRow();
            for (int i = 0; i < ColumnsHeaderLeft.Length; i++)
                drHeader[i] = ColumnsHeaderLeft[i];

            hdrHeaderLeft.Rows.Add(drHeader);

            drHeader = hdrFooter.NewRow();
            for (int i = 0; i < ColumnsFooter.Length; i++)                
                drHeader[i] = ColumnsFooter[i];
            
            hdrFooter.Rows.Add(drHeader);

            #endregion

            #region table names
            dtOrderDetails.TableName = "tblOrderDetails";
            dtPaymentDetails.TableName = "tblPaymentDetails";
            dtHeaderLeft.TableName = "tblHeaderLeft";
            dtHeaderRight.TableName = "tblHeaderRight";
            dtFooter.TableName = "tblFooter";

            #endregion

            #region create the datatables with the required columns

            for (int i = 0; i < listColumnsOrderDetails.Count; i++)
                dtOrderDetails.Columns.Add("Column" + i.ToString());

            for (int i = 0; i < listColumnsPaymentDetails.Count; i++)
                dtPaymentDetails.Columns.Add("Column" + i.ToString());

            for (int i = 0; i < listColumnsHeaderRight.Count; i++)
                dtHeaderRight.Columns.Add("Column" + i.ToString());

            for (int i = 0; i < listColumnsHeaderLeft.Count; i++)
                dtHeaderLeft.Columns.Add("Column" + i.ToString());

            for (int i = 0; i < listColumnsFooter.Count; i++)
                dtFooter.Columns.Add("Column" + i.ToString());

            // special case: column name for header (left) is the company name
            string strCompanyName = CompanyInfo.CompanyName;
            List<string> listCompanyAddress = new List<string>();
            string hold = "";
            if (!string.IsNullOrEmpty(PrintSettingInfo.receiptSetting.ReceiptAddress1.Trim()))
            {
                hold = PrintSettingInfo.receiptSetting.ReceiptAddress1;
                listCompanyAddress.Add(hold);
            }
            if (!string.IsNullOrEmpty(PrintSettingInfo.receiptSetting.ReceiptAddress2.Trim()))
            {
                hold = PrintSettingInfo.receiptSetting.ReceiptAddress2;
                listCompanyAddress.Add(hold);
            }
            if (!string.IsNullOrEmpty(PrintSettingInfo.receiptSetting.ReceiptAddress3.Trim()))
            {
                hold = PrintSettingInfo.receiptSetting.ReceiptAddress3;
                listCompanyAddress.Add(hold);
            }
            if (!string.IsNullOrEmpty(PrintSettingInfo.receiptSetting.ReceiptAddress4.Trim()))
            {
                hold = PrintSettingInfo.receiptSetting.ReceiptAddress4;
                listCompanyAddress.Add(hold);
            }
            
            #endregion

            string status = "";
            OrderDetCollection orderDetails = pos.FetchUnsavedOrderDet();

            ReceiptDetCollection receiptDetails = pos.FetchUnsavedReceipt(out status);

            #region add data for order details

            DataRow drOrderDetails = null;            

            for (int i = 0; i < orderDetails.Count; i++)
            {
                if (!orderDetails[i].IsVoided)
                {
                    drOrderDetails = dtOrderDetails.NewRow();
                    drOrderDetails[0] = orderDetails[i].ItemNo;
                    drOrderDetails[1] = orderDetails[i].Item.ItemName;
                    drOrderDetails[2] = orderDetails[i].Quantity;
                    drOrderDetails[3] = String.Format("{0:0.00}", orderDetails[i].Discount);
                    drOrderDetails[4] = String.Format("{0:$0.00}", orderDetails[i].UnitPrice);
                    drOrderDetails[5] = String.Format("{0:$0.00}", orderDetails[i].Amount);
                    dtOrderDetails.Rows.Add(drOrderDetails);
                }
            }

            /* Parameterized */
            txtTotal.TextName = "txtTotal";
            txtTotal.TextHeight = sizeTotalAmount_Height;
            txtTotal.TextWidth = sizeTotalAmount_WidthLabel;
            txtTotal.TextValue = "TOTAL";
            txtTotal.posTxt.x = sizeTotalAmount_X;
            txtTotal.posTxt.y = sizeTotalAmount_Y;
            txtTotal.cellFormat.FontFamily = new FontFamily("Arial");
            txtTotal.cellFormat.FontSize = vFontSize;
            txtTotal.cellFormat.FontWeight = "Bold";

            txtTotalValue.TextName = "txtTotalValue";
            txtTotalValue.TextHeight = sizeTotalAmount_Height;
            txtTotalValue.TextWidth = sizeTotalAmount_WidthAmount;
            txtTotalValue.TextValue = String.Format("{0:$0.00}", pos.CalculateTotalAmount(out status));
            txtTotalValue.posTxt.x = txtTotal.posTxt.x + txtTotal.TextWidth;
            txtTotalValue.posTxt.y = txtTotal.posTxt.y;
            txtTotalValue.cellFormat.FontFamily = new FontFamily("Arial");
            txtTotalValue.cellFormat.FontSize = vFontSize;
            txtTotalValue.cellFormat.FontWeight = "Bold";
            txtTotalValue.cellFormat.TextAlignment = "Right";
            /* End of Parameterized */

            listPageTextboxes.Add(txtTotal);
            listPageTextboxes.Add(txtTotalValue);            

            #endregion

            #region add data for payment mode
            DataRow drPaymentDetails = null;           

            for (int i = 0; i < receiptDetails.Count; i++)
            {
                drPaymentDetails = dtPaymentDetails.NewRow();
                drPaymentDetails[0] = receiptDetails[i].PaymentType;
                drPaymentDetails[1] = String.Format("{0:$0.00}", receiptDetails[i].Amount);
                dtPaymentDetails.Rows.Add(drPaymentDetails);
            }

            #endregion

            #region add data to header (left)

            DataRow drHeaderLeft = null;
            //drHeaderLeft[0] = strCompanyName;
            //dtHeaderLeft.Rows.Add(drHeaderLeft);

            txtCompanyName.TextName = "txtComanyName";
            txtCompanyName.TextHeight = 0.50;
            txtCompanyName.TextWidth = WidthHeaderLeft[0];
            txtCompanyName.TextValue = strCompanyName.ToUpper();
            txtCompanyName.posTxt.x = 0.0;
            txtCompanyName.posTxt.y = 1.55;
            txtCompanyName.cellFormat.FontFamily = new FontFamily("Arial");
            txtCompanyName.cellFormat.FontSize = vFontSize;
            txtCompanyName.cellFormat.FontWeight = "Bold";
            txtCompanyName.cellFormat.FontColor = Color.Purple;
            txtCompanyName.cellFormat.BorderStyle = "None";
            listPageTextboxes.Add(txtCompanyName);

            for (int i = 0; i < listCompanyAddress.Count; i++)
            {
                drHeaderLeft = dtHeaderLeft.NewRow();
                drHeaderLeft[0] = listCompanyAddress[i];
                dtHeaderLeft.Rows.Add(drHeaderLeft);
            }
            
            #endregion

            #region add data to header (right)

            DataRow drHeaderRight = dtHeaderRight.NewRow();
            drHeaderRight[0] = "Invoice Date";
            DateTime dt = pos.GetOrderDate();
            drHeaderRight[1] = dt.ToString("MMMMM dd, yyyy");
            dtHeaderRight.Rows.Add(drHeaderRight);

            drHeaderRight = dtHeaderRight.NewRow();
            drHeaderRight[0] = "Invoice No.";
            drHeaderRight[1] = pos.GetUnsavedRefNo();
            dtHeaderRight.Rows.Add(drHeaderRight);

            Membership member = pos.GetMemberInfo();
            string strInvoiceFor = "";
            string strInvoiceForAddress = "";
            if (member == null)
            {                
                // do nothing
            }

            else
            {
                if (member.NameToAppear == null)
                    strInvoiceFor = "";
                else
                {
                    strInvoiceFor = member.NameToAppear;
                    hold = "";
                    if (member.StreetName != null)
                    {
                        if (!string.IsNullOrEmpty(member.StreetName.Trim()))
                        {
                            hold = member.StreetName + ", ";
                            strInvoiceForAddress += hold;
                        }
                    }
                    if (member.StreetName2 != null)
                    {
                        if (!string.IsNullOrEmpty(member.StreetName2.Trim()))
                        {
                            hold = member.StreetName2 + ", ";
                            strInvoiceForAddress += hold;
                        }
                    }
                    if (member.City != null)
                    {
                        if (!string.IsNullOrEmpty(member.City.Trim()))
                        {
                            hold = member.City + ", ";
                            strInvoiceForAddress += hold;
                        }
                    }
                    if (member.Country != null)
                    {
                        if (!string.IsNullOrEmpty(member.Country.Trim()))
                        {
                            hold = member.Country;
                            strInvoiceForAddress += hold;
                        }
                    }
                    if (member.ZipCode != null)
                    {
                        if (!string.IsNullOrEmpty(member.ZipCode.Trim()))
                            strInvoiceForAddress += " - " + member.ZipCode;
                    }                   
                }
            }
                                        
            drHeaderRight = dtHeaderRight.NewRow();
            drHeaderRight[0] = "FOR:";
            drHeaderRight[1] = strInvoiceFor;
            dtHeaderRight.Rows.Add(drHeaderRight);

            drHeaderRight = dtHeaderRight.NewRow();
            drHeaderRight[0] = "";
            drHeaderRight[1] = strInvoiceForAddress;
            dtHeaderRight.Rows.Add(drHeaderRight);

            #endregion

            #region add data to footer

            DataRow drFooter = null;            
            /*drFooter[0] = "Terms & Conditions";
            dtFooter.Rows.Add(drFooter);*/

            drFooter = dtFooter.NewRow();
            string strFooter = "";

            if (!string.IsNullOrEmpty(PrintSettingInfo.receiptSetting.TermCondition1.Trim()))
            {
                hold = PrintSettingInfo.receiptSetting.TermCondition1 + ". ";
                strFooter += hold;
            }
            if (!string.IsNullOrEmpty(PrintSettingInfo.receiptSetting.TermCondition2.Trim()))
            {
                hold = PrintSettingInfo.receiptSetting.TermCondition2 + ". ";
                strFooter += hold;
            }
            if (!string.IsNullOrEmpty(PrintSettingInfo.receiptSetting.TermCondition3.Trim()))
            {
                hold = PrintSettingInfo.receiptSetting.TermCondition3 + ". ";
                strFooter += hold;
            }
            if (!string.IsNullOrEmpty(PrintSettingInfo.receiptSetting.TermCondition4.Trim()))
            {
                hold = PrintSettingInfo.receiptSetting.TermCondition4 + ".";
                strFooter += hold;
            }
            if (!string.IsNullOrEmpty(PrintSettingInfo.receiptSetting.TermCondition5.Trim()))
            {
                hold = PrintSettingInfo.receiptSetting.TermCondition5 + ".";
                strFooter += hold;
            }
            if (!string.IsNullOrEmpty(PrintSettingInfo.receiptSetting.TermCondition6.Trim()))
            {
                hold = PrintSettingInfo.receiptSetting.TermCondition6 + ".";
                strFooter += hold;
            }

            drFooter[0] = strFooter;
            dtFooter.Rows.Add(drFooter);

            #endregion

            #region assign datatables to respective tables
            tblOrderDetails.dt = dtOrderDetails;
            tblOrderDetails.dtHeader = hdrOrderDetails;
            tblPaymentDetails.dt = dtPaymentDetails;
            tblPaymentDetails.dtHeader = hdrPaymentDetails;
            tblHeaderLeft.dt = dtHeaderLeft;
            tblHeaderLeft.dtHeader = null;
            tblHeaderRight.dt = dtHeaderRight;
            tblHeaderRight.dtHeader = hdrHeaderRight;
            tblFooter.dt = dtFooter;
            tblFooter.dtHeader = hdrFooter;

            #endregion
        }
        private void SetTableColumnWidth()
        {
            for (int i = 0; i < WidthOrderDetails.Length; i++)
                tblOrderDetails.listColumnWidth.Add(WidthOrderDetails[i]);

            for (int i = 0; i < WidthPaymentDetails.Length; i++)
                tblPaymentDetails.listColumnWidth.Add(WidthPaymentDetails[i]);

            for (int i = 0; i < WidthHeaderLeft.Length; i++)
                tblHeaderLeft.listColumnWidth.Add(WidthHeaderLeft[i]);

            for (int i = 0; i < WidthHeaderRight.Length; i++)
                tblHeaderRight.listColumnWidth.Add(WidthHeaderRight[i]);

            for (int i = 0; i < WidthFooter.Length; i++)
                tblFooter.listColumnWidth.Add(WidthFooter[i]);
            
        }
        private void SetTablePosition()
        {
            tblOrderDetails.tablePosition = posOrderDetails;
            tblPaymentDetails.tablePosition = posPaymentDetails;
            tblHeaderLeft.tablePosition = posHeaderLeft;
            tblHeaderRight.tablePosition = posHeaderRight;
            tblFooter.tablePosition = posFooter;
        }
        private void SetAllTableFormat()
        {
            tblfmtOrderDetails.TableDetailRowHeight = 0.5;
            tblfmtOrderDetails.TableHeaderRowHeight = 0.5;
            tblfmtOrderDetails.HeaderCellFormat.FontFamily = new FontFamily("Arial");
            tblfmtOrderDetails.HeaderCellFormat.FontSize = vFontSize;
            tblfmtOrderDetails.HeaderCellFormat.FontWeight = "Bold";
            tblfmtOrderDetails.RepeatHeaderOnNewPage = true;
            tblfmtOrderDetails.HeaderColor = Color.LightGray;
            tblfmtOrderDetails.HeaderCellFormat.FontSize = vFontSize;
            tblfmtOrderDetails.DetailCellFormat.FontSize = vFontSize;

            tblfmtPaymentDetails.TableDetailRowHeight = 0.25;
            tblfmtPaymentDetails.TableHeaderRowHeight = 0.25;
            tblfmtPaymentDetails.HeaderCellFormat.FontWeight = "Bold";
            tblfmtPaymentDetails.HeaderColor = Color.LightGray;
            tblfmtPaymentDetails.HeaderCellFormat.FontSize = vFontSize;
            tblfmtPaymentDetails.DetailCellFormat.FontSize = vFontSize;

            tblfmtHeaderLeft.TableDetailRowHeight = 0.15;
            tblfmtHeaderLeft.TableHeaderRowHeight = 0.0;
            tblfmtHeaderLeft.HeaderCellFormat.BorderStyle = "None";
            tblfmtHeaderLeft.DetailCellFormat.BorderStyle = "None";
            tblfmtHeaderLeft.DetailCellFormat.FontWeight = "Bold";
            tblfmtHeaderLeft.HeaderCellFormat.FontSize = vFontSize;
            tblfmtHeaderLeft.DetailCellFormat.FontSize = vFontSize;

            tblfmtHeaderRight.TableDetailRowHeight = 0.25;
            tblfmtHeaderRight.TableHeaderRowHeight = 0.0;
            tblfmtHeaderRight.HeaderCellFormat.BorderStyle = "None";
            tblfmtHeaderRight.DetailCellFormat.BorderStyle = "None";
            tblfmtHeaderRight.HeaderCellFormat.FontSize = vFontSize;
            tblfmtHeaderRight.DetailCellFormat.FontSize = vFontSize;

            tblfmtFooter.TableDetailRowHeight = 0.5;
            tblfmtFooter.TableHeaderRowHeight = 0.25;
            tblfmtFooter.HeaderCellFormat.BorderStyle = "None";
            tblfmtFooter.DetailCellFormat.BorderStyle = "None";
            tblfmtFooter.HeaderCellFormat.FontWeight = "Bold";
            tblfmtFooter.HeaderCellFormat.FontSize = vFontSize;
            tblfmtFooter.DetailCellFormat.FontSize = vFontSize;

            tblOrderDetails.tblFormat = tblfmtOrderDetails;
            tblPaymentDetails.tblFormat = tblfmtPaymentDetails;
            tblHeaderLeft.tblFormat = tblfmtHeaderLeft;
            tblHeaderRight.tblFormat = tblfmtHeaderRight;
            tblFooter.tblFormat = tblfmtFooter;
        }
        private void FormatAllTableColumns()
        {
            ColumnFormat format = new ColumnFormat();
            
            // table: orderdetails
            
            format.columnName = "Column2";
            format.cellFormat = new CellFormat(tblOrderDetails.tblFormat.DetailCellFormat);
            format.cellFormat.TextAlignment = "Center";
            cfmtOrderDetails.Add(format);

            format = new ColumnFormat();
            format.columnName = "Column3";
            format.cellFormat = new CellFormat(tblOrderDetails.tblFormat.DetailCellFormat);
            format.cellFormat.TextAlignment = "Center";
            cfmtOrderDetails.Add(format);

            format = new ColumnFormat();
            format.columnName = "Column4";
            format.cellFormat = new CellFormat(tblOrderDetails.tblFormat.DetailCellFormat);
            format.cellFormat.TextAlignment = "Right";
            cfmtOrderDetails.Add(format);

            format = new ColumnFormat();
            format.columnName = "Column5";
            format.cellFormat = new CellFormat(tblOrderDetails.tblFormat.DetailCellFormat);
            format.cellFormat.TextAlignment = "Right";
            cfmtOrderDetails.Add(format);

            // table: HeaderRight
            format = new ColumnFormat();
            format.columnName = "Column0";
            format.cellFormat = new CellFormat(tblHeaderRight.tblFormat.DetailCellFormat);
            format.cellFormat.FontWeight = "Bold";
            format.cellFormat.FontStyle = "Italic";
            cfmtHeaderRight.Add(format);

            // table: PaymentDetails
            format = new ColumnFormat();
            format.columnName = "Column1";
            format.cellFormat = new CellFormat(tblPaymentDetails.tblFormat.DetailCellFormat);
            format.cellFormat.TextAlignment = "Right";
            cfmtPaymentDetails.Add(format);

            tblOrderDetails.tblFormat.listColumnFormat = cfmtOrderDetails;
            tblPaymentDetails.tblFormat.listColumnFormat = cfmtPaymentDetails;
            tblHeaderRight.tblFormat.listColumnFormat = cfmtHeaderRight;
        }
        private void AddReportLogo()
        {            
            if(File.Exists(strLogoPath))
            {
                listPageImages.Add(imgLogo);                   
            }
        }

        public PageTypeA5()
            : base()
        {
            frmReport = new Report();

            string str = ConfigurationSettings.AppSettings["AutoPrint"];
            if (str == null || str.ToLower() == "true")
                bAutoPrintReport = true;

            else
                bAutoPrintReport = false;

            txtTotal = new Textbox();
            txtTotalValue = new Textbox();
            txtCompanyName = new Textbox();
            txtReportType = new Textbox();

            #region logo details

            strLogoPath = ConfigurationSettings.AppSettings["LogoPath"];
            strReportLogoName = ConfigurationSettings.AppSettings["LogoFileName"];
            strLogoPath += "\\" + strReportLogoName;

            if (File.Exists(strLogoPath))
            {
                imgLogo = new Image();
                string name = "logo";
                imgLogo.ImageName = name;
                imgLogo.ImageMIMEType = MIMEType.JPEG;
                imgLogo.ImageSizing = Sizing.FitProportional;
                imgLogo.ImagePath = strLogoPath;
                imgLogo.ImageHeight = 1.8;
                imgLogo.ImageWidth = 6.0;
                imgLogo.posImg.x = 0.0;
                imgLogo.posImg.y = 0.0;
            }
            else
                imgLogo = null;

            #endregion

            listColumnsOrderDetails = new List<string>();
            listColumnsPaymentDetails = new List<string>();
            listColumnsHeaderRight = new List<string>();
            listColumnsHeaderLeft = new List<string>();
            listColumnsFooter = new List<string>();

            tblOrderDetails = new Table();
            tblPaymentDetails = new Table();
            tblHeaderLeft = new Table();
            tblHeaderRight = new Table();
            tblFooter = new Table();
            cfmtOrderDetails = new List<ColumnFormat>();
            cfmtPaymentDetails = new List<ColumnFormat>();
            cfmtHeaderRight = new List<ColumnFormat>();
            cfmtHeaderLeft = new List<ColumnFormat>();
            cfmtFooter = new List<ColumnFormat>();

            #region position of tables
            posOrderDetails = new ElementPosition(0.0, 2.75);
            tblfmtOrderDetails = new TableFormat();
            posPaymentDetails = new ElementPosition(9.5, 5.25); /// Originally 5.2
            tblfmtPaymentDetails = new TableFormat();
            posFooter = new ElementPosition(0.0, 5.25);
            tblfmtFooter = new TableFormat();
            posHeaderRight = new ElementPosition(7.5, 1.75);
            tblfmtHeaderRight = new TableFormat();
            posHeaderLeft = new ElementPosition(0.0, 1.98);
            tblfmtHeaderLeft = new TableFormat();

            #endregion
            // table headers
            hdrOrderDetails = new DataTable();
            hdrPaymentDetails = new DataTable();
            hdrHeaderLeft = new DataTable();
            hdrHeaderRight = new DataTable();
            hdrFooter = new DataTable();            

            #region column names of tables

            string helper = "";
            for (int i = 0; i < ColumnsOrderDetails.Length; i++)
            {
                helper = ColumnsOrderDetails[i];
                listColumnsOrderDetails.Add(helper);
                hdrOrderDetails.Columns.Add("Columns" + i.ToString());
            }

            for (int i = 0; i < ColumnsPaymentDetails.Length; i++)
            {
                helper = ColumnsPaymentDetails[i];
                listColumnsPaymentDetails.Add(helper);
                hdrPaymentDetails.Columns.Add("Columns" + i.ToString());
            }

            for (int i = 0; i < ColumnsHeaderRight.Length; i++)
            {
                helper = ColumnsHeaderRight[i];
                listColumnsHeaderRight.Add(helper);
                hdrHeaderRight.Columns.Add("Columns" + i.ToString());
            }

            for (int i = 0; i < ColumnsHeaderLeft.Length; i++)
            {
                helper = ColumnsHeaderLeft[i];
                listColumnsHeaderLeft.Add(helper);
                hdrHeaderLeft.Columns.Add("Columns" + i.ToString());
            }

            for (int i = 0; i < ColumnsFooter.Length; i++)
            {
                helper = ColumnsFooter[i];
                listColumnsFooter.Add(helper);
                hdrFooter.Columns.Add("Columns" + i.ToString());
            }

            #endregion
           
            base.pageName = "A5";
            //PageFormat pageFormat = new PageFormat();
            base.pageFormat.PageWidth = 14.85;
            base.pageFormat.PageHeight = 21.00;//Originally 10.50
            
            base.pageFormat.LeftMargin = 1.15;
            base.pageFormat.RightMargin = 1.35;
            base.pageFormat.TopMargin = 1.27;
            base.pageFormat.BottomMargin = 1.27;

            base.pageFormat.BodyHeight = 12.31;
            base.pageFormat.BodyWidth = 8;
/*
            base.pageFormat.FontSizeBody = "4";
            base.pageFormat.FontSizeFooter = "4";
            base.pageFormat.FontSizeHeader = "4";
 * */
            
        }        
    }
}
