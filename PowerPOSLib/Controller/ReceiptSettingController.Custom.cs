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
using PowerPOS.Container;

namespace PowerPOS
{

    public partial class ReceiptSettingController
    {
        public static ReceiptSetting GetReceiptPrinterSetting()
        {
            ReceiptSettingCollection r = new ReceiptSettingCollection();
            r.OrderByAsc(ReceiptSetting.Columns.CreatedOn);
            r.Load();
            if (r.Count > 0)
            {
                return r[0];
            }
            else
            {
                return new ReceiptSetting();
            }
        }

        public static void SaveReceiptPrinterSetting
            (bool printReceipt, bool useOutletAddress, 
             string address1, string address2, 
             string address3, string address4, 
             bool showMembership, bool showSalesPerson, 
             string salesPersonTitle,
             string termCondition1, string termCondition2, 
             string termCondition3, 
             string termCondition4, string termCondition5, 
             string termCondition6, int PaperSize, int NumOfCopies)
        {
            PrintSettingInfo.receiptSetting.PrintReceipt = printReceipt;
            PrintSettingInfo.receiptSetting.UseOutletAddress = useOutletAddress;
            PrintSettingInfo.receiptSetting.ReceiptAddress1 = address1;
            PrintSettingInfo.receiptSetting.ReceiptAddress2 = address2;
            PrintSettingInfo.receiptSetting.ReceiptAddress3 = address3;
            PrintSettingInfo.receiptSetting.ReceiptAddress4 = address4;
            PrintSettingInfo.receiptSetting.ShowMembershipInfo = showMembership;
            PrintSettingInfo.receiptSetting.ShowSalesPersonInfo = showSalesPerson;
            PrintSettingInfo.receiptSetting.SalesPersonTitle = salesPersonTitle;
            PrintSettingInfo.receiptSetting.TermCondition1 = termCondition1;
            PrintSettingInfo.receiptSetting.TermCondition2 = termCondition2;
            PrintSettingInfo.receiptSetting.TermCondition3 = termCondition3;
            PrintSettingInfo.receiptSetting.TermCondition4 = termCondition4;
            PrintSettingInfo.receiptSetting.TermCondition5 = termCondition5;
            PrintSettingInfo.receiptSetting.TermCondition6 = termCondition6;
            PrintSettingInfo.receiptSetting.PaperSize = PaperSize;
            PrintSettingInfo.receiptSetting.NumOfCopies = NumOfCopies;
            PrintSettingInfo.receiptSetting.Save();
        }
    }
}

