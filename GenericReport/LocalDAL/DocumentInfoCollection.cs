using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PowerPOS;
using System.Data;

namespace LocalDAL
{
    public class DocumentInfoCollection
    {
        public List<DocumentInfo> Obj;

        public DocumentInfoCollection(string DocumentType, POSController pos)
        {
            Obj = new List<DocumentInfo>();
            OrderDetCollection Orders= pos.FetchUnsavedOrderDet();

            for (int Counter = 0; Counter < Orders.Count; Counter++)
            {
                DocumentInfo OneObj = new DocumentInfo();
                OneObj.DocumentType = DocumentType;
                OneObj.DocumentDate = pos.GetOrderDate();
                OneObj.DocumentNo = pos.GetSavedRefNo();

                Membership Member = pos.GetMemberInfo();
                OneObj.MembershipInfo = Member.NameToAppear;
                OneObj.MembershipAddress = "";
                if (Member.StreetName != null && Member.StreetName != "") OneObj.MembershipAddress = OneObj.MembershipAddress + "~" + Member.StreetName;
                if (Member.StreetName2 != null && Member.StreetName2 != "") OneObj.MembershipAddress = OneObj.MembershipAddress + "~" + Member.StreetName2;
                if (Member.ZipCode != null && Member.ZipCode != "") OneObj.MembershipAddress = OneObj.MembershipAddress + "~" + Member.ZipCode;
                if (Member.Mobile != null && Member.Mobile != "") OneObj.MembershipAddress = OneObj.MembershipAddress + "~" + Member.Mobile;
                OneObj.MembershipAddress = OneObj.MembershipAddress.Trim('~').Replace("~", Environment.NewLine);

                CompanyCollection TheCompany = new CompanyCollection();
                TheCompany.Load();
                OneObj.CompanyName = TheCompany[0].ReceiptName;

                ReceiptSettingCollection TheAddress = new ReceiptSettingCollection();
                TheAddress.Load();
                OneObj.CompanyAddress = "";
                if (TheAddress[0].ReceiptAddress1 != null && TheAddress[0].ReceiptAddress1 != "") OneObj.CompanyAddress = OneObj.CompanyAddress + "~" + TheAddress[0].ReceiptAddress1;
                if (TheAddress[0].ReceiptAddress2 != null && TheAddress[0].ReceiptAddress2 != "") OneObj.CompanyAddress = OneObj.CompanyAddress + "~" + TheAddress[0].ReceiptAddress2;
                if (TheAddress[0].ReceiptAddress3 != null && TheAddress[0].ReceiptAddress3 != "") OneObj.CompanyAddress = OneObj.CompanyAddress + "~" + TheAddress[0].ReceiptAddress3;
                if (TheAddress[0].ReceiptAddress4 != null && TheAddress[0].ReceiptAddress4 != "") OneObj.CompanyAddress = OneObj.CompanyAddress + "~" + TheAddress[0].ReceiptAddress4;
                OneObj.CompanyAddress = OneObj.CompanyAddress.Trim('~').Replace("~", Environment.NewLine);

                OneObj.TermsCondition = "";
                if (TheAddress[0].TermCondition1 != null && TheAddress[0].TermCondition1 != "") OneObj.TermsCondition = OneObj.TermsCondition + "~" + TheAddress[0].TermCondition1;
                if (TheAddress[0].TermCondition2 != null && TheAddress[0].TermCondition2 != "") OneObj.TermsCondition = OneObj.TermsCondition + "~" + TheAddress[0].TermCondition2;
                if (TheAddress[0].TermCondition3 != null && TheAddress[0].TermCondition3 != "") OneObj.TermsCondition = OneObj.TermsCondition + "~" + TheAddress[0].TermCondition3;
                if (TheAddress[0].TermCondition4 != null && TheAddress[0].TermCondition4 != "") OneObj.TermsCondition = OneObj.TermsCondition + "~" + TheAddress[0].TermCondition4;
                if (TheAddress[0].TermCondition5 != null && TheAddress[0].TermCondition5 != "") OneObj.TermsCondition = OneObj.TermsCondition + "~" + TheAddress[0].TermCondition5;
                if (TheAddress[0].TermCondition6 != null && TheAddress[0].TermCondition6 != "") OneObj.TermsCondition = OneObj.TermsCondition + "~" + TheAddress[0].TermCondition6;
                OneObj.TermsCondition = OneObj.TermsCondition.Trim('~').Replace("~", Environment.NewLine);

                OneObj.LogoURL = "";

                OneObj.ItemCode = Orders[Counter].ItemNo;
                OneObj.Description = Orders[Counter].Item.ItemName;
                OneObj.Quantity = Orders[Counter].Quantity.GetValueOrDefault(0);
                OneObj.DiscPercent = Orders[Counter].Discount ;
                OneObj.UnitPrice = Orders[Counter].UnitPrice;
                OneObj.LineTotal = Orders[Counter].Amount;

                OneObj.Remarks = pos.GetRemarks();
            }

        }

        public DataTable toDataTable()
        {
            DataTable Out = new DataTable();
            Out.Columns.Add("DocumentType", Type.GetType("System.String"));
            Out.Columns.Add("DocumentDate", Type.GetType("System.DateTime"));
            Out.Columns.Add("DocumentNo", Type.GetType("System.String"));
            Out.Columns.Add("MembershipInfo", Type.GetType("System.String"));
            Out.Columns.Add("MembershipAddress", Type.GetType("System.String"));
            Out.Columns.Add("CompanyName", Type.GetType("System.String"));
            Out.Columns.Add("CompanyAddress", Type.GetType("System.String"));
            Out.Columns.Add("TermsCondition", Type.GetType("System.String"));
            Out.Columns.Add("LogoURL", Type.GetType("System.String"));
            Out.Columns.Add("ItemCode", Type.GetType("System.String"));
            Out.Columns.Add("Description", Type.GetType("System.String"));
            Out.Columns.Add("Quantity", Type.GetType("System.Int32"));
            Out.Columns.Add("DiscPercent", Type.GetType("System.Decimal"));
            Out.Columns.Add("UnitPrice", Type.GetType("System.Decimal"));
            Out.Columns.Add("LineTotal", Type.GetType("System.Decimal"));
            Out.Columns.Add("Remarks", Type.GetType("System.String"));

            for (int Counter = 0; Counter < Obj.Count; Counter++)
            {
                DocumentInfo zz = Obj[Counter];
                Out.Rows.Add(new object[]{zz.DocumentType,zz.DocumentDate,zz.DocumentNo
                    ,zz.MembershipInfo,zz.MembershipAddress,zz.CompanyName,zz.CompanyAddress
                    ,zz.TermsCondition,zz.LogoURL,zz.ItemCode,zz.Description,zz.Quantity,
                    zz.DiscPercent,zz.UnitPrice,zz.LineTotal,zz.Remarks });
            }

            return Out;
        }
    }
}
