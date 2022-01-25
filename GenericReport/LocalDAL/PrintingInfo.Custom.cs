using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PowerPOS;
using System.IO;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using PowerPOS.Container;
using SubSonic;

namespace GenericReport.LocalDAL
{
    public partial class PrintingInfo
    {
        public void AssignDataForQuotation(string DocumentType, QuoteController pos, string LogoURL, out int LogoWidth, out int LogoHeight, bool reprint)
        {
            //Obj = new List<DocumentInfo>();
            QuotationDetCollection Orders = pos.FetchUnsavedOrderDet();
            if (Orders.Count > 0)
            {
                if (Orders[0].Userint4 != null)
                {
                    Orders.Sort("Userint4", true);
                }
                else
                {
                    Orders.Sort(OrderDet.Columns.CreatedOn, true);
                }
            }

            #region *) Assign Total Outstanding

            decimal totalOutStanding = 0;

            string membershipno = pos.myOrderHdr.MembershipNo;

            if (!string.IsNullOrEmpty(membershipno) && membershipno != "WALK-IN")
            {
                DataTable source = Installment.GetMemberHistory(membershipno, new DateTime(1990, 1, 1), new DateTime(2100, 1, 1));

                object obj;
                decimal debitTotal = 0.00M;
                obj = source.Compute("SUM(debit)", "");
                if (obj is decimal) debitTotal = (decimal)obj;
                decimal creditTotal = 0.00M;
                obj = source.Compute("SUM(credit)", "");

                if (obj is decimal) creditTotal = (decimal)obj;
                decimal plusminus = debitTotal - creditTotal;
                totalOutStanding = -1 * plusminus;

            }
            #endregion

            #region *) Assign Delivery Detail

            var qr = new Query("DeliveryOrder");
            qr.AddWhere(DeliveryOrder.Columns.SalesOrderRefNo, pos.myOrderHdr.OrderHdrID);
            var doData = new DeliveryOrderController().FetchByQuery(qr).Where(o => !o.Deleted.GetValueOrDefault(false)).ToList();

            for (int i = 0; i < doData.Count; i++)
            {
                DeliveryInfoRow doRow = DeliveryInfo.NewDeliveryInfoRow();
                doRow.Receipent = string.IsNullOrEmpty(doData[i].RecipientName) ? "KIV" : doData[i].RecipientName;
                doRow.MobileNo = string.IsNullOrEmpty(doData[i].MobileNo) ? "KIV" : doData[i].MobileNo;
                doRow.HomeNo = string.IsNullOrEmpty(doData[i].HomeNo) ? "KIV" : doData[i].HomeNo;
                doRow.PostalCode = string.IsNullOrEmpty(doData[i].PostalCode) ? "KIV" : doData[i].PostalCode;
                doRow.DeliveryAddress = string.IsNullOrEmpty(doData[i].DeliveryAddress) ? "KIV" : doData[i].DeliveryAddress;
                doRow.DeliveryDate = doData[i].DeliveryDate.HasValue ? doData[i].DeliveryDate.GetValueOrDefault(DateTime.Now).ToString("dd-MM-yyyy") : "KIV";
                doRow.DeliveryTime = (!doData[i].TimeSlotFrom.HasValue || !doData[i].TimeSlotTo.HasValue)
                    ? "KIV" : string.Format("{0} - {1}",
                    doData[i].TimeSlotFrom.GetValueOrDefault(DateTime.Now).ToString("hh:mm tt"),
                    doData[i].TimeSlotTo.GetValueOrDefault(DateTime.Now).ToString("hh:mm tt"));
                doRow.DeliveryNo = "Delivery #" + (i + 1);
                doRow.Remarks = doData[i].Remark;
                List<string> itemNoList = new List<string>();
                List<string> itemNameList = new List<string>();
                List<string> itemQuantityList = new List<string>();

                var doDetData = doData[i].DeliveryOrderDetails().ToList();
                for (int j = 0; j < doDetData.Count; j++)
                {
                    var theItem = new Item(Item.Columns.ItemNo, doDetData[j].ItemNo);
                    itemNoList.Add(doDetData[j].ItemNo);
                    itemNameList.Add(theItem.ItemName);
                    itemQuantityList.Add(doDetData[j].Quantity.GetValueOrDefault(0).ToString("N0"));
                }

                doRow.ItemNo = string.Join(Environment.NewLine, itemNoList.ToArray());
                doRow.ItemName = string.Join(Environment.NewLine, itemNameList.ToArray());
                doRow.Quantity = string.Join(Environment.NewLine, itemQuantityList.ToArray());
                DeliveryInfo.AddDeliveryInfoRow(doRow);
            }

            #endregion

            LogoWidth = 0;
            LogoHeight = 0;

            bool isRedeem = false;

            for (int Counter = 0; Counter < Orders.Count; Counter++)
            {
                if (Orders[Counter].IsVoided) continue;

                DocumentInfoRow OneObj = DocumentInfo.NewDocumentInfoRow();
                OneObj.IsUsingDelivery = doData.Count > 0 ? 1 : 0;
                OneObj.TotalOutstanding = totalOutStanding;

                OneObj.DocumentType = DocumentType;
                OneObj.DocumentDate = pos.GetOrderDate();
                OneObj.DocumentNo = pos.GetCustomizedRefNo();

                //print document status - reprint and or voided
                OneObj.DocumentStatus = "";
                if (pos.IsVoided())
                    OneObj.DocumentStatus += "~" + "VOIDED";
                if (reprint)
                    OneObj.DocumentStatus += "~" + "REPRINT";
                OneObj.DocumentStatus = OneObj.DocumentStatus.Trim('~').Replace("~", Environment.NewLine);

                //sales person
                OneObj.DocumentSalesPerson = "";

                Membership Member = pos.GetMemberInfo();
                if (Member.MembershipNo.ToLower() != "walk-in")
                {
                    OneObj.MFirstName = Member.FirstName;
                    OneObj.MLastName = Member.LastName;
                    OneObj.MGender = Member.Gender;
                    OneObj.MMobile = Member.Mobile;
                    OneObj.MNRIC = Member.Nric;
                    OneObj.MStreetName = Member.StreetName;
                    OneObj.MStreetName2 = Member.StreetName2;
                    OneObj.MUnitNo = Member.UnitNo;
                    OneObj.MZipCode = Member.ZipCode;


                    OneObj.MembershipInfo = Member.NameToAppear;
                    //adding NRIC to membership info line
                    if (Member.Nric != null && Member.Nric != "")
                        OneObj.MembershipInfo = OneObj.MembershipInfo + "~" + Member.Nric;

                    OneObj.MembershipInfo = OneObj.MembershipInfo.Trim('~').Replace("~", Environment.NewLine);
                    OneObj.CustomerName = (Member.FirstName + " " + Member.LastName).Trim();

                    //address information
                    OneObj.MembershipAddress = "";
                    if (Member.StreetName != null && Member.StreetName != "") OneObj.MembershipAddress = OneObj.MembershipAddress + "~" + Member.StreetName;
                    if (Member.StreetName2 != null && Member.StreetName2 != "") OneObj.MembershipAddress = OneObj.MembershipAddress + "~" + Member.StreetName2;
                    if (Member.City != null && Member.City != "") OneObj.MembershipAddress = OneObj.MembershipAddress + "~" + Member.City;
                    if (Member.City != null && Member.City != "" && Member.ZipCode != null && Member.ZipCode != "")
                        OneObj.MembershipAddress = OneObj.MembershipAddress + ",";
                    else
                        OneObj.MembershipAddress = OneObj.MembershipAddress + "~";
                    if (Member.ZipCode != null && Member.ZipCode != "") OneObj.MembershipAddress = OneObj.MembershipAddress + Member.ZipCode;
                    if (Member.Mobile != null && Member.Mobile != "") OneObj.MembershipAddress = OneObj.MembershipAddress + "~" + Member.Mobile;
                    OneObj.MembershipAddress = OneObj.MembershipAddress.Trim('~').Replace("~", Environment.NewLine);
                    OneObj.MembershipNo = Member.MembershipNo;

                    OneObj.Home = "";
                    if (Member.Home != null && Member.Home != "")
                        OneObj.Home = OneObj.Home + Member.Home;
                    if (Member.Mobile != null && Member.Mobile != "")
                    {
                        if (OneObj.Home != "") OneObj.Home = OneObj.Home + ", ";
                        OneObj.Home = OneObj.Home + Member.Mobile;
                    }
                    OneObj.Fax = Member.Fax;
                    OneObj.Email = Member.Email;
                    OneObj.MemberIsOutlet = false;
                    PointOfSaleCollection posColl = new PointOfSaleCollection();
                    posColl.Where(PointOfSale.UserColumns.LinkedMembershipNo, Member.MembershipNo);
                    posColl.Load();

                    if (posColl.Count > 0)
                    {
                        OneObj.MemberIsOutlet = true;
                    }

                }
                else
                {
                    OneObj.MembershipInfo = "-";
                    OneObj.MembershipAddress = "";
                    OneObj.MembershipNo = "-";
                    OneObj.Home = "";
                    OneObj.Fax = "";
                    OneObj.Email = "";
                    OneObj.MFirstName = "";
                    OneObj.MLastName = "";
                    OneObj.MGender = "";
                    OneObj.MMobile = "";
                    OneObj.MNRIC = "";
                    OneObj.MStreetName = "";
                    OneObj.MStreetName2 = "";
                    OneObj.MUnitNo = "";
                    OneObj.MZipCode = "";
                    OneObj.MemberIsOutlet = false;
                }

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
                OneObj.LineGST = Orders[Counter].GSTAmount.GetValueOrDefault(0);
                OneObj.TotalChange = 0;

                OneObj.ItemCode = Orders[Counter].ItemNo;
                OneObj.Description = Orders[Counter].Item.ItemName;
                if (Orders[Counter].Item.Userfld10 == "T" || Orders[Counter].Item.Userfld10 == "D")
                {
                    isRedeem = true;
                }
                //if (Orders[Counter].Remark != null && Orders[Counter].Remark != "")
                //    OneObj.Description += "\n" + Orders[Counter].Remark;

                OneObj.CategoryName = Orders[Counter].Item.CategoryName;
                OneObj.ItemDescription = Orders[Counter].Item.ItemDesc;
                OneObj.Quantity = Orders[Counter].Quantity;
                OneObj.DiscPercent = Orders[Counter].Discount;
                OneObj.UnitPrice = Orders[Counter].UnitPrice;
                OneObj.LineTotal = Orders[Counter].Amount;
                OneObj.Barcode = Orders[Counter].Item.Barcode;
                OneObj.Remarks = pos.GetRemarks();
                OneObj.DiscountDetail = Orders[Counter].DiscountDetail;
                OneObj.GSTRule = Orders[Counter].Item.GSTRule.GetValueOrDefault(0);
                OneObj.DeliverTo = "";
                Decimal attr1;
                if (Decimal.TryParse(Orders[Counter].Item.Attributes1, out attr1))
                {
                    OneObj.Attributes1 = ((decimal)Orders[Counter].Quantity * attr1).ToString();
                }
                if (File.Exists(LogoURL))
                {
                    FileStream fs = new FileStream(LogoURL,
                        System.IO.FileMode.Open, System.IO.FileAccess.Read);
                    System.Drawing.Image LoadedImg = System.Drawing.Image.FromFile(LogoURL);

                    byte[] Img = new byte[fs.Length];
                    fs.Read(Img, 0, Convert.ToInt32(fs.Length));
                    fs.Close();
                    //objDataRow[strImageField] = Image;

                    OneObj.CompanyName = "";

                    OneObj.LogoStream = Img;
                    OneObj.LogoWidth = LoadedImg.Width;
                    OneObj.LogoHeight = LoadedImg.Height;

                    LogoWidth = LoadedImg.Width;
                    LogoHeight = LoadedImg.Height;
                }
                //OneObj 

                //New UserField
                OneObj.HdrUserfld1 = pos.myOrderHdr.Userfld1;
                OneObj.HdrUserfld2 = pos.myOrderHdr.Userfld2;
                OneObj.HdrUserfld3 = pos.myOrderHdr.Userfld3;
                OneObj.HdrUserfld4 = pos.myOrderHdr.Userfld4;
                OneObj.HdrUserfld5 = pos.myOrderHdr.Userfld5;
                OneObj.HdrUserfld6 = pos.myOrderHdr.Userfld6;
                OneObj.HdrUserfld7 = pos.myOrderHdr.Userfld7;
                OneObj.HdrUserfld8 = pos.myOrderHdr.Userfld8;
                OneObj.HdrUserfld9 = pos.myOrderHdr.Userfld9;
                OneObj.HdrUserfld10 = pos.myOrderHdr.Userfld10;
                OneObj.LineRemark = Orders[Counter].Remark;
                OneObj.DetUserfld1 = Orders[Counter].Userfld1;
                OneObj.DetUserfld2 = Orders[Counter].Userfld2;
                OneObj.DetUserfld3 = Orders[Counter].Userfld3;
                OneObj.DetUserfld4 = Orders[Counter].Userfld4;
                OneObj.DetUserfld5 = Orders[Counter].Userfld5;
                OneObj.DetUserfld6 = Orders[Counter].Userfld6;
                OneObj.DetUserfld7 = Orders[Counter].Userfld7;
                OneObj.DetUserfld8 = Orders[Counter].Userfld8;
                OneObj.DetUserfld9 = Orders[Counter].Userfld9;
                OneObj.DetUserfld10 = Orders[Counter].Userfld10;

                DocumentInfo.AddDocumentInfoRow(OneObj);

            }



        }

        public void AssignData(string DocumentType, POSController pos, string LogoURL, out int LogoWidth, out int LogoHeight, bool reprint)
        {
            AssignData(DocumentType, pos, LogoURL, out LogoWidth, out LogoHeight, reprint, false);
        }

        public void AssignData(string DocumentType, POSController pos, string LogoURL, out int LogoWidth, out int LogoHeight, bool reprint, bool isPreview)
        {
            //Obj = new List<DocumentInfo>();
            //OrderDetCollection Orders= pos.FetchUnsavedOrderDet();
            //Orders.Sort(OrderDet.Columns.CreatedOn, true);
            
            OrderDetCollection Orders;
            if (isPreview)
                Orders = pos.myOrderDet;
            else
                Orders = pos.FetchOrderDetForReceipt(pos.myOrderHdr.OrderHdrID);

            #region *) Assign Total Outstanding

            decimal totalOutStanding = 0;

            string membershipno = pos.myOrderHdr.MembershipNo;

            if (!string.IsNullOrEmpty(membershipno) && membershipno != "WALK-IN")
            {
                DataTable source = Installment.GetMemberHistory(membershipno, new DateTime(1990, 1, 1), new DateTime(2100, 1, 1));

                object obj;
                decimal debitTotal = 0.00M;
                obj = source.Compute("SUM(debit)", "");
                if (obj is decimal) debitTotal = (decimal)obj;
                decimal creditTotal = 0.00M;
                obj = source.Compute("SUM(credit)", "");

                if (obj is decimal) creditTotal = (decimal)obj;
                decimal plusminus = debitTotal - creditTotal;
                totalOutStanding = -1 * plusminus;

            }
            #endregion

            #region *) Assign Delivery Detail

            var qrd = new Query("DeliveryOrder");
            qrd.AddWhere(DeliveryOrder.Columns.SalesOrderRefNo, pos.myOrderHdr.OrderHdrID);
            var doData = new DeliveryOrderController().FetchByQuery(qrd).Where(o => !o.Deleted.GetValueOrDefault(false)).ToList();

            for (int i = 0; i < doData.Count; i++)
            {
                DeliveryInfoRow doRow = DeliveryInfo.NewDeliveryInfoRow();
                doRow.Receipent = string.IsNullOrEmpty(doData[i].RecipientName) ? "KIV" : doData[i].RecipientName;
                doRow.MobileNo = string.IsNullOrEmpty(doData[i].MobileNo) ? "KIV" : doData[i].MobileNo;
                doRow.HomeNo = string.IsNullOrEmpty(doData[i].HomeNo) ? "KIV" : doData[i].HomeNo;
                doRow.PostalCode = string.IsNullOrEmpty(doData[i].PostalCode) ? "KIV" : doData[i].PostalCode;
                doRow.DeliveryAddress = string.IsNullOrEmpty(doData[i].DeliveryAddress) ? "KIV" : doData[i].DeliveryAddress;
                doRow.DeliveryDate = doData[i].DeliveryDate.HasValue ? doData[i].DeliveryDate.GetValueOrDefault(DateTime.Now).ToString("dd-MM-yyyy") : "KIV";
                doRow.DeliveryTime = (!doData[i].TimeSlotFrom.HasValue || !doData[i].TimeSlotTo.HasValue)
                    ? "KIV" : string.Format("{0} - {1}",
                    doData[i].TimeSlotFrom.GetValueOrDefault(DateTime.Now).ToString("hh:mm tt"),
                    doData[i].TimeSlotTo.GetValueOrDefault(DateTime.Now).ToString("hh:mm tt"));
                doRow.DeliveryNo = "Delivery #" + (i + 1);
                doRow.Remarks = doData[i].Remark;
                List<string> itemNoList = new List<string>();
                List<string> itemNameList = new List<string>();
                List<string> itemQuantityList = new List<string>();

                var doDetData = doData[i].DeliveryOrderDetails().ToList();
                for (int j = 0; j < doDetData.Count; j++)
                {
                    var theItem = new Item(Item.Columns.ItemNo, doDetData[j].ItemNo);
                    itemNoList.Add(doDetData[j].ItemNo);
                    itemNameList.Add(theItem.ItemName);
                    itemQuantityList.Add(doDetData[j].Quantity.GetValueOrDefault(0).ToString("N0"));
                }

                doRow.ItemNo = string.Join(Environment.NewLine, itemNoList.ToArray());
                doRow.ItemName = string.Join(Environment.NewLine, itemNameList.ToArray());
                doRow.Quantity = string.Join(Environment.NewLine, itemQuantityList.ToArray());
                DeliveryInfo.AddDeliveryInfoRow(doRow);
            }

            #endregion

            LogoWidth = 0;
            LogoHeight = 0;

            bool isRedeem = false;
            decimal TotalInstallment = 0;
            decimal TotalCash = 0;
            string PaymentRemark = "";
            for (int Counter = 0; Counter < pos.recDet.Count; Counter++)
            {
                if (pos.recDet[Counter].PaymentType.ToLower() == "installment")
                {
                    TotalInstallment += pos.recDet[Counter].Amount;
                    if (!string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.Payment.InstallmentText)))
                        pos.recDet[Counter].PaymentType = AppSetting.GetSetting(AppSetting.SettingsName.Payment.InstallmentText);                    
                }
                if (pos.recDet[Counter].PaymentType.ToLower().Contains("cash"))
                {
                    TotalCash += pos.recDet[Counter].Amount;

                }
                if (pos.recDet[Counter].PaymentType.ToLower() == "cheque")
                {
                    PaymentRemark = pos.recDet[Counter].Userfld1 + "-" + pos.recDet[Counter].Userfld2;

                }
            }

            for (int Counter = 0; Counter < Orders.Count; Counter++)
            {
                if (Orders[Counter].IsVoided) continue;

                DocumentInfoRow OneObj = DocumentInfo.NewDocumentInfoRow();

                OneObj.IsUsingDelivery = doData.Count > 0 ? 1 : 0;
                OneObj.TotalOutstanding = totalOutStanding;

                OneObj.DocumentType = DocumentType;
                OneObj.DocumentDate = pos.GetOrderDate();
                if (isPreview)
                    OneObj.DocumentNo = "";
                else
                    OneObj.DocumentNo = pos.GetCustomizedRefNo();
                UserMst cashier = new UserMst(pos.myOrderHdr.CashierID);
                OneObj.Cashier = cashier == null ? pos.myOrderHdr.CashierID : cashier.DisplayName;
                OneObj.POSName = PointOfSaleInfo.PointOfSaleName;

                //print document status - reprint and or voided
                OneObj.DocumentStatus = "";
                if (pos.IsVoided())
                    OneObj.DocumentStatus += "~" + "VOIDED";
                if(reprint)
                    OneObj.DocumentStatus += "~" + "REPRINT";
                OneObj.DocumentStatus = OneObj.DocumentStatus.Trim('~').Replace("~", Environment.NewLine);
                
                //sales person
                OneObj.DocumentSalesPerson = pos.GetSalesPerson();

                Membership Member = pos.GetMemberInfo();
                if (Member != null && Member.MembershipNo != null && Member.MembershipNo.ToLower() != "walk-in")
                {
                    OneObj.MembershipInfo = Member.NameToAppear;
                    //adding NRIC to membership info line
                    if (Member.Nric != null && Member.Nric != "")
                    {
                        OneObj.MembershipInfo = OneObj.MembershipInfo + "~" + Member.Nric;
                        OneObj.NRIC = Member.Nric;
                    }
                    else
                    {
                        OneObj.NRIC = "-";
                    }

                    OneObj.MembershipInfo = OneObj.MembershipInfo.Trim('~').Replace("~", Environment.NewLine);

                    //address information
                    OneObj.MembershipAddress = "";
                    if (Member.StreetName != null && Member.StreetName != "") OneObj.MembershipAddress = OneObj.MembershipAddress + "~" + Member.StreetName;
                    if (Member.StreetName2 != null && Member.StreetName2 != "") OneObj.MembershipAddress = OneObj.MembershipAddress + "~" + Member.StreetName2;
                    if (Member.City != null && Member.City != "") OneObj.MembershipAddress = OneObj.MembershipAddress + "~" + Member.City;
                    if (Member.City != null && Member.City != "" && Member.ZipCode != null && Member.ZipCode != "") 
                        OneObj.MembershipAddress = OneObj.MembershipAddress + ",";
                    else
                        OneObj.MembershipAddress = OneObj.MembershipAddress + "~";
                    if (Member.ZipCode != null && Member.ZipCode != "") OneObj.MembershipAddress = OneObj.MembershipAddress + Member.ZipCode;
                    if (Member.Mobile != null && Member.Mobile != "") OneObj.MembershipAddress = OneObj.MembershipAddress + "~" + Member.Mobile;
                    if (Member.Mobile != null && Member.Office != "") OneObj.MembershipAddress = OneObj.MembershipAddress + "~" + Member.Office;
                    if (Member.Mobile != null && Member.Fax != "") OneObj.MembershipAddress = OneObj.MembershipAddress + "~" + Member.Fax;
                    OneObj.MembershipAddress = OneObj.MembershipAddress.Trim('~').Replace("~", Environment.NewLine);
                    OneObj.MemberRemarks = Member.Remarks;
                    OneObj.MemberOccupation = Member.Occupation;
                    OneObj.Home = "";
                    if (Member.Home != null && Member.Home != "")
                        OneObj.Home = OneObj.Home + Member.Home;
                    if (Member.Mobile != null && Member.Mobile != "")
                    {
                        if (OneObj.Home != "") OneObj.Home = OneObj.Home + ", ";
                        OneObj.Home = OneObj.Home + Member.Mobile;
                    }
                    OneObj.Fax = Member.Fax;
                    OneObj.MembershipNo = Member.MembershipNo;
                    OneObj.Email = String.IsNullOrEmpty(Member.Email) ? "" : Member.Email;
                    OneObj.MemberIsOutlet = false;
                    PointOfSaleCollection posColl = new PointOfSaleCollection();
                    posColl.Where(PointOfSale.UserColumns.LinkedMembershipNo, Member.MembershipNo);
                    posColl.Load();

                    if (posColl.Count > 0)
                    {
                        OneObj.MemberIsOutlet = true;
                    }
                    OneObj.MFirstName = Member.FirstName;
                    OneObj.MLastName = Member.LastName;
                    OneObj.MGender = Member.Gender;
                    OneObj.MMobile = Member.Mobile;
                    OneObj.MNRIC = Member.Nric;
                    OneObj.MStreetName = Member.StreetName;
                    OneObj.MStreetName2 = Member.StreetName2;
                    OneObj.MUnitNo = Member.UnitNo;
                    OneObj.MZipCode = Member.ZipCode;


                }
                else
                {
                    OneObj.MembershipInfo = "-";
                    OneObj.MembershipAddress = "";
                    OneObj.MemberRemarks = "";
                    OneObj.MemberOccupation = "";
                    OneObj.Home = "";
                    OneObj.Fax = "";
                    OneObj.MembershipNo = "";
                    OneObj.Email = "";
                    OneObj.MemberIsOutlet = false;

                    OneObj.MFirstName = "";
                    OneObj.MLastName = "";
                    OneObj.MGender = "";
                    OneObj.MMobile = "";
                    OneObj.MNRIC = "";
                    OneObj.MStreetName = "";
                    OneObj.MStreetName2 = "";
                    OneObj.MUnitNo = "";
                    OneObj.MZipCode = "";
                }

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
                OneObj.LineGST = Orders[Counter].GSTAmount.GetValueOrDefault(0);
                OneObj.TotalChange = 0;

                //add Deposit
                DataTable dt = new DataTable();
                //dt = ReportController.FetchAccountStatementReport("2000-01-01", DateTime.Today.AddDays(1).ToString("yyyy-MM-dd"), Member.MembershipNo);

                //if (dt.Rows.Count > 0)
                //{
                //    OneObj.InstallmentBalance = Convert.ToDecimal(dt.Rows[0]["Balance"].ToString());
                //}
                //else
                //{
                //    OneObj.InstallmentBalance = 0;
                //}

                OneObj.InstallmentBalance = pos.OutstandingBalanceOverall;

                OneObj.TotalCash = TotalCash;
                
                string orderhdrID = pos.myOrderHdr.OrderHdrID;
                if (Orders[Counter].ItemNo == "INST_PAYMENT")
                {
                    //installment 
                    orderhdrID = Orders[Counter].Userfld3;
                }
                //dt = ReportController.FetchAccountStatementReportPerTransaction(DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd"), DateTime.Today.AddDays(1).ToString("yyyy-MM-dd"), orderhdrID);

                //if (dt.Rows.Count > 0)
                //{
                //    OneObj.OrderInstallmentBalance = Convert.ToDecimal(dt.Rows[0]["Balance"].ToString());
                //}
                //else
                //{
                //    OneObj.OrderInstallmentBalance = 0;
                //}

                OneObj.OrderInstallmentBalance = pos.OutstandingBalanceOrder;

                OneObj.ItemCode = Orders[Counter].ItemNo;
                OneObj.Description = Orders[Counter].Item.ItemName;
                if (Orders[Counter].Item.Userfld10 == "T" || Orders[Counter].Item.Userfld10 == "D")
                {
                    isRedeem = true;
                }


                if (Orders[Counter].Remark != null && Orders[Counter].Remark != "")
                {
                    Item tempItem = new Item(Orders[Counter].ItemNo);
                    OneObj.Description += " (" + Orders[Counter].Remark + ")\n";
                }
                else
                {
                    OneObj.Description += "\n";
                }


                //OneObj.GSTRule = Orders[Counter].Item.GSTRule.GetValueOrDefault(0);
                OneObj.GSTRule = pos.myOrderHdr.Userint1.HasValue ? pos.myOrderHdr.Userint1.Value : Orders[Counter].Item.GSTRule.GetValueOrDefault(0);
                OneObj.CategoryName = Orders[Counter].Item.CategoryName;
                OneObj.ItemDescription = Orders[Counter].Item.ItemDesc;
                OneObj.Quantity = Orders[Counter].Quantity.GetValueOrDefault(0);
                OneObj.DiscPercent = Orders[Counter].Discount;
                OneObj.UnitPrice = Orders[Counter].UnitPrice;
                OneObj.LineTotal = Orders[Counter].Amount;
                OneObj.Barcode = Orders[Counter].Item.Barcode;
                OneObj.Remarks = pos.GetRemarks();
                OneObj.DeliverTo = pos.myOrderHdr.Userfld7;
                OneObj.DeliveryAddress = pos.recHdr.Remark == null ? " ~ " : pos.recHdr.Remark;
                OneObj.DeliveryDate = pos.recHdr.DeliveryDate + " " + pos.recHdr.DeliveryTime;
                OneObj.StoreReference = pos.myOrderHdr.Userfld6;
                OneObj.PurchaseOrderNo = pos.myOrderHdr.Userfld8;
                OneObj.LineInfo = Orders[Counter].Userfld4;
                OneObj.UOM = Orders[Counter].UOM;
                OneObj.LineSalesPerson = Orders[Counter].Userfld1;
                OneObj.PaymentRemark = PaymentRemark;
                OneObj.DiscountDetail = Orders[Counter].DiscountDetail;
                
                Decimal attr1;
                if (Decimal.TryParse(Orders[Counter].Item.Attributes1, out attr1))
                {
                    OneObj.Attributes1 = ((decimal)Orders[Counter].Quantity * attr1).ToString();
                }

                string membershipNo = Member == null || Member.MembershipNo == null ? "WALK-IN" : Member.MembershipNo;
                decimal pointInitial = 0;
                decimal pointChanged = 0;
                decimal pointRemaining = 0;
                //OrderHdr myOrderHdr = new OrderHdr(OrderHdr.Columns.OrderRefNo, pos.GetUnsavedRefNo());
                //OneObj.PointInitial = myOrderHdr.InitialPoint.GetValueOrDefault(0);
                //OneObj.PointChanged = myOrderHdr.PointAmount.GetValueOrDefault(0);
                //OneObj.PointRemaining = OneObj.PointInitial + OneObj.PointChanged;

                #region *) Get Initial Point and Points Earned
                if (!string.IsNullOrEmpty(membershipNo) && membershipNo != "WALK-IN")
                {
                    PowerPOS.Feature.Package.GetInitialPointsAndPointsEarned(membershipNo, pos, out pointInitial, out pointChanged);
                    pointRemaining = pointInitial + pointChanged;
                }
                #endregion

                OneObj.PointInitial = pointInitial;
                OneObj.PointChanged = pointChanged;
                OneObj.PointRemaining = pointRemaining;
                OneObj.TotalInstallmentPayment = TotalInstallment;


                if (File.Exists(LogoURL))
                {
                    FileStream fs = new FileStream(LogoURL,
                        System.IO.FileMode.Open, System.IO.FileAccess.Read);
                    System.Drawing.Image LoadedImg = System.Drawing.Image.FromFile(LogoURL);

                    byte[] Img = new byte[fs.Length];
                    fs.Read(Img, 0, Convert.ToInt32(fs.Length));
                    fs.Close();
                    //objDataRow[strImageField] = Image;

                    OneObj.CompanyName = "";

                    OneObj.LogoStream = Img;
                    OneObj.LogoWidth = LoadedImg.Width;
                    OneObj.LogoHeight = LoadedImg.Height;
                   
                    LogoWidth = LoadedImg.Width;
                    LogoHeight = LoadedImg.Height;
                }
                //OneObj 

                string signatureFile = Application.StartupPath + "\\Signature\\sign" + pos.GetUnsavedRefNo() + ".jpg";

                        //take the image from the folder and print it
                
                if (File.Exists(signatureFile))
                {
                    FileStream fs = new FileStream(signatureFile,
                        System.IO.FileMode.Open, System.IO.FileAccess.Read);
                    System.Drawing.Image LoadedImg = System.Drawing.Image.FromFile(signatureFile);

                    byte[] Img = new byte[fs.Length];
                    fs.Read(Img, 0, Convert.ToInt32(fs.Length));
                    fs.Close();
                    //objDataRow[strImageField] = Image;

                    //OneObj.CompanyName = "";

                    OneObj.SignatureStream = Img;
                    //OneObj.LogoWidth = LoadedImg.Width;
                    //OneObj.LogoHeight = LoadedImg.Height;

                    LogoWidth = LoadedImg.Width;
                    LogoHeight = LoadedImg.Height;
                }

                if (Orders[Counter].IsPromo)
                {
                    OneObj.PromoAmount = Orders[Counter].PromoAmount;
                    OneObj.PromoUnitPrice = Orders[Counter].PromoUnitPrice;
                    OneObj.PromoName = "";
                    if (Orders[Counter].PromoHdrID.HasValue)
                    {
                        PromoCampaignHdr pch = new PromoCampaignHdr(PromoCampaignHdr.Columns.PromoCampaignHdrID, Orders[Counter].PromoHdrID.GetValueOrDefault(0));
                        if (!pch.IsNew)
                        {
                            OneObj.PromoName = pch.PromoCampaignName;
                        }
                    }
                    OneObj.PromoQty = 1;
                    if (Orders[Counter].PromoDetID.HasValue)
                    {
                        PromoCampaignDet pcd = new PromoCampaignDet(PromoCampaignDet.Columns.PromoCampaignDetID, Orders[Counter].PromoDetID.GetValueOrDefault(0));
                        if (!pcd.IsNew)
                        {
                            if (pcd.AnyQty > 0)
                                OneObj.PromoQty = Orders[Counter].Quantity.GetValueOrDefault(0)/pcd.AnyQty.GetValueOrDefault(1);
                            else
                                OneObj.PromoQty = Orders[Counter].Quantity.GetValueOrDefault(0)/pcd.UnitQty.GetValueOrDefault(1);
                            OneObj.PromoDetID = Orders[Counter].PromoDetID.ToString();
                        }

                    }
                    else
                    {
                        OneObj.PromoQty = 0;
                        OneObj.PromoDetID = "";
                    }
                }
                else
                {
                    OneObj.PromoAmount = 0;
                    OneObj.PromoUnitPrice = 0;

                    if (Orders[Counter].PromoHdrID.HasValue)
                    {
                        PromoCampaignHdr pch = new PromoCampaignHdr(PromoCampaignHdr.Columns.PromoCampaignHdrID, Orders[Counter].PromoHdrID.GetValueOrDefault(0));
                        if (!pch.IsNew)
                        {
                            OneObj.PromoName = "";
                        }
                    }
                }
                

                //--setting show/hide point on the receipt
                bool HidePrintPackageBalanceOnReceipt = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Print.HidePrintPackageBalanceOnReceipt), false);

                OneObj.IsPrintPointOnTheReceipt = (!HidePrintPackageBalanceOnReceipt && PrintSettingInfo.receiptSetting.ShowMembershipInfo && pos.MembershipApplied() && pos.GetMemberInfo().MembershipNo != "WALK-IN") ;

                OneObj.IsPrintBalancePaymentOnTheReceipt = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Print.PrintBalancePaymentOnTheReceipt), false);

                OneObj.BalancePayment = pos.GetBalancePayment();

                List<string> suppliers = new List<string>();
                foreach (var ism in Orders[Counter].Item.ItemSupplierMapRecords())
                {
                    if (ism.Supplier != null)
                    {
                        string telp = "";
                        if (!string.IsNullOrEmpty(ism.Supplier.ContactNo1)) telp = " Tel: " + ism.Supplier.ContactNo1;
                        suppliers.Add(ism.Supplier.SupplierName + telp);
                    }
                }
                OneObj.Supplier = string.Join(", ", suppliers.ToArray());

                OneObj.Attributes5 = Orders[Counter].Item.Attributes5;

                OneObj.ItemAttributes1 = Orders[Counter].Item.Attributes1;
                OneObj.ItemAttributes2 = Orders[Counter].Item.Attributes2;
                OneObj.ItemAttributes3 = Orders[Counter].Item.Attributes3;
                OneObj.ItemAttributes4 = Orders[Counter].Item.Attributes4;
                OneObj.ItemAttributes5 = Orders[Counter].Item.Attributes5;
                OneObj.ItemAttributes6 = Orders[Counter].Item.Attributes6;
                OneObj.ItemAttributes7 = Orders[Counter].Item.Attributes7;
                OneObj.ItemAttributes8 = Orders[Counter].Item.Attributes8;

                string[] extensions = new string[] { "jpg", "gif", "png", "bmp", "jpeg" };

                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.UseImageItemFromLocalFolder), false))
                {
                    string ImagePhotosFolder = AppSetting.GetSetting(AppSetting.SettingsName.Item.ItemImageFolderLocal);
                    if (!string.IsNullOrEmpty(ImagePhotosFolder) && Directory.Exists(ImagePhotosFolder))
                    {

                        foreach (string ext in extensions)
                        {
                            string ImagePath = ImagePhotosFolder + Orders[Counter].Item.ItemNo.Trim() + "." + ext;
                            int SizeResize = 40;
                            string SizeImageReport = AppSetting.GetSetting(AppSetting.SettingsName.Item.SizeImageReport);


                            if (String.IsNullOrEmpty(SizeImageReport) || (!String.IsNullOrEmpty(SizeImageReport) && !Int32.TryParse(SizeImageReport, out SizeResize)))
                            {
                                SizeResize = 40;
                            }

                            if (System.IO.File.Exists(ImagePath))
                            {
                                OneObj.ItemImage = ResizeImage(System.Drawing.Image.FromFile(ImagePath), new Size(SizeResize, SizeResize));
                            }
                        }
                    }
                }

                //OneObj.ItemImage = Orders[Counter].Item.Attributes8;

                OneObj.IsPreOrder = Orders[Counter].IsPreOrder.GetValueOrDefault(false);

                //New UserField
                OneObj.HdrUserfld1 = pos.myOrderHdr.Userfld1;
                OneObj.HdrUserfld2 = pos.myOrderHdr.Userfld2;
                OneObj.HdrUserfld3 = pos.myOrderHdr.Userfld3;
                OneObj.HdrUserfld4 = pos.myOrderHdr.Userfld4;
                OneObj.HdrUserfld5 = pos.myOrderHdr.Userfld5;
                OneObj.HdrUserfld6 = pos.myOrderHdr.Userfld6;
                OneObj.HdrUserfld7 = pos.myOrderHdr.Userfld7;
                OneObj.HdrUserfld8 = pos.myOrderHdr.Userfld8;
                OneObj.HdrUserfld9 = pos.myOrderHdr.Userfld9;
                OneObj.HdrUserfld10 = pos.myOrderHdr.Userfld10;
                OneObj.LineRemark = Orders[Counter].Remark;
                OneObj.DetUserfld1 = Orders[Counter].Userfld1;
                OneObj.DetUserfld2 = Orders[Counter].Userfld2;
                OneObj.DetUserfld3 = Orders[Counter].Userfld3;
                OneObj.DetUserfld4 = Orders[Counter].Userfld4;
                OneObj.DetUserfld5 = Orders[Counter].Userfld5;
                OneObj.DetUserfld6 = Orders[Counter].Userfld6;
                OneObj.DetUserfld7 = Orders[Counter].Userfld7;
                OneObj.DetUserfld8 = Orders[Counter].Userfld8;
                OneObj.DetUserfld9 = Orders[Counter].Userfld9;
                OneObj.DetUserfld10 = Orders[Counter].Userfld10;

                DocumentInfo.AddDocumentInfoRow(OneObj);
            }

            for (int Counter = 0; Counter < pos.recDet.Count; Counter++)
            {
                PaymentInfoRow OnePayment = PaymentInfo.NewPaymentInfoRow();
                bool isForeign = false;

                string paymentType = pos.recDet[Counter].PaymentType;
                if (pos.recDet[Counter].PaymentType.StartsWith("CASH-"))
                    isForeign = true;

                if (!string.IsNullOrEmpty(pos.recDet[Counter].Userfld1))
                {
                    // Get Points name
                    Item itm = new Item(pos.recDet[Counter].Userfld1);
                    if (itm != null && itm.ItemNo == pos.recDet[Counter].Userfld1)
                    {
                        paymentType = paymentType + " (" + itm.ItemName + ")";
                    }
                    else
                    {
                        //paymentType = paymentType + " (" + pos.recDet[Counter].Userfld1 + ")";
                        paymentType =  pos.recDet[Counter].Userfld1;
                    }

                    if (pos.recDet[Counter].PaymentType == "INSTALLMENT" && pos.recDet[Counter].Userfld1 != "C.O.D")
                    {
                        DataTable dt = ReportController.GetDueDateInstallmentPayment(pos.recDet[Counter].ReceiptDetID);
                        if (dt.Rows.Count > 0)
                        {
                            OnePayment.DueDate = (DateTime)dt.Rows[0]["DueDate"];
                        }
                    }
                }
                OnePayment.PaymentType = paymentType;
                OnePayment.ForeignCurrencyRate = pos.recDet[Counter].ForeignCurrencyRate.GetValueOrDefault(0);
                OnePayment.ForeignAmountReceived = pos.recDet[Counter].ForeignAmountReceived.GetValueOrDefault(0);
                OnePayment.ForeignAmountReturned = pos.recDet[Counter].ForeignAmountReturned.GetValueOrDefault(0);

                if (isForeign)
                    OnePayment.Amount = pos.recDet[Counter].ForeignAmountReceived.GetValueOrDefault(0);
                else
                    OnePayment.Amount = pos.recDet[Counter].Amount;

                if (pos.recDet[Counter].PaymentType.StartsWith("CASH"))
                    OnePayment.IsCash = true;
                else
                    OnePayment.IsCash = false;

                OnePayment.LocalCurrAmount = pos.recDet[Counter].Amount;

                if (pos.recDet[Counter].PaymentType == POSController.PAY_CHEQUE && !string.IsNullOrEmpty(pos.recDet[Counter].Userfld2))
                    OnePayment.BankName = pos.recDet[Counter].Userfld2;

                PaymentInfo.AddPaymentInfoRow(OnePayment);
           

                if (pos.recDet[Counter].Change.GetValueOrDefault(0) != 0 && DocumentInfo.Rows.Count > 0)
                {
                    decimal TotalChange = DocumentInfo[0].TotalChange + pos.recDet[Counter].Change.Value;
                    for (int ChangeCounter = 0; ChangeCounter < DocumentInfo.Rows.Count; ChangeCounter++)
                        DocumentInfo[ChangeCounter].TotalChange = TotalChange;
                }
            }

            System.Data.DataTable CurrentAmounts;
            string status = "";
            PowerPOS.Feature.Package.GetCurrentAmountLocal(pos.CurrentMember == null ? "" : pos.CurrentMember.MembershipNo, DateTime.Now, out CurrentAmounts, out status);

            #region *) Make sure the PointTempLog table is cleaned
            PointTempLogController.Clean();
            #endregion

            // Include point changes from PointTempLog into the CurrentAmounts.
            // (if PointTempLog got data that means the point has not been synced to posdb yet)
            DataTable dtPTL;
            PowerPOS.Feature.Package.AllocateNewlyEarnedPointsToCurrentAmounts(CurrentAmounts, pos.myOrderHdr.MembershipNo, out dtPTL, out status);
            CurrentAmounts = dtPTL.Copy();

            status = ""; /// Clear aja.. :)

            if (CurrentAmounts != null)
            {
                foreach (DataRow dr in CurrentAmounts.Rows)
                {
                    PointInfoRow pr = PointInfo.NewPointInfoRow();
                    pr.MembershipNo = pos.CurrentMember.MembershipNo;
                    pr.Userfld1 = dr[0].ToString();
                    Item i = new Item(pr.Userfld1);
                    pr.ItemName = i.ItemName;
                    decimal Points = 0;
                    decimal.TryParse(dr[1].ToString(), out Points);
                    pr.Points = Points;
                    pr.IsRedeem = isRedeem ? 1 : 0;

                    pr.PointAllocated = 0;

                    var qr = new Query("PointAllocationLog");
                    qr.AddWhere(PointAllocationLog.Columns.OrderHdrID, pos.myOrderHdr.OrderHdrID);
                    qr.AddWhere(PointAllocationLog.Columns.Userfld1, dr["userfld1"] + "");

                    //var pointLog = new PointAllocationLogController().FetchByQuery(qr).FirstOrDefault();
                    //if (pointLog != null && !pointLog.IsNew)
                    var pointLogs = new PointAllocationLogController().FetchByQuery(qr);
                    if (pointLogs.Count > 0)
                    {
                        //pr.PointAllocated = pointLog.PointAllocated;
                        pr.PointAllocated = pointLogs.Sum(p => p.PointAllocated);
                    }
                    else
                    {
                        if (pos.PointPackageBreakdown != null)
                        {
                            foreach (DataRow row in pos.PointPackageBreakdown.Rows)
                            {
                                if (dr["userfld1"].ToString() == row["RefNo"].ToString())
                                {
                                    pr.PointAllocated += row["Amount"].ToString().GetDecimalValue();
                                    //break;
                                }
                            }
                        }
                    }
                    pr.PointType = dr["PointType"] + "";

                    PointInfo.AddPointInfoRow(pr);
                }
            }

        }

        private Byte[] ResizeImage(System.Drawing.Image image, Size size)
        {
            System.Drawing.Image imgToResize = image;

            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)size.Width / (float)sourceWidth);
            nPercentH = ((float)size.Height / (float)sourceHeight);

            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((System.Drawing.Image)b);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();

            MemoryStream ms = new MemoryStream();
            b.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            return ms.ToArray();

            //return (System.Drawing.Image)b;
        }
    }

   
}
