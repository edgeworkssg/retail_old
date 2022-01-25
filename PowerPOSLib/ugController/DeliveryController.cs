
using SubSonic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data ;
using PowerPOS.Container;
using System.Data.SqlClient;

namespace PowerPOS
{
    public partial class DeliveryController
    {
        public DeliveryOrder myDeliveryOrderHdr;
        public DeliveryOrderDetailCollection myDeliveryOrderDet;

        public DeliveryController()
        {
            Init_DeliveryController(PointOfSaleInfo.PointOfSaleID);
        }

        public DeliveryController(int pointOfSaleID)
        {
            Init_DeliveryController(pointOfSaleID);
        }

        private void Init_DeliveryController(int pointOfSaleID)
        {
            myDeliveryOrderHdr = new DeliveryOrder();
            myDeliveryOrderDet = new DeliveryOrderDetailCollection();
            myDeliveryOrderHdr.OrderNumber = CreateNewDeliveryNo(pointOfSaleID);
            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Receipt.DO_UseCustomNo), false))
            {
                myDeliveryOrderHdr.PurchaseOrderRefNo = CreateNewCustomReceiptNo();
            }
            else
            {
                myDeliveryOrderHdr.PurchaseOrderRefNo = "OR" + myDeliveryOrderHdr.OrderNumber;
            }
        }

        public DeliveryController(string OrderNumber)
        {
            myDeliveryOrderHdr = new DeliveryOrder(OrderNumber);
            myDeliveryOrderDet = new DeliveryOrderDetailCollection();
            myDeliveryOrderDet.Where(DeliveryOrderDetail.Columns.Dohdrid, OrderNumber);
            myDeliveryOrderDet.Load();
        }

        public string CreateNewCustomReceiptNo()
        {
            string prefixselect = "select AppSettingValue from AppSetting where AppSettingKey='DO_CustomPrefix'";
            string prefix = DataService.ExecuteScalar(new QueryCommand(prefixselect)).ToString();

            //get current receiptno
            string sql = "select case AppSettingValue when '' then '0' else AppSettingValue end from AppSetting where AppSettingKey='DO_CurrentReceiptNo'";
            QueryCommand Qcmd = new QueryCommand(sql);
            string currentReceiptNo = DataService.ExecuteScalar(Qcmd).ToString();

            //default max receiptno is 4
            string sql2 = "select case AppSettingValue when '' then '4' else AppSettingValue end from AppSetting where AppSettingKey='DO_ReceiptLength'";
            QueryCommand Qcmd2 = new QueryCommand(sql2);
            int maxReceiptNo = 4;
            int.TryParse(DataService.ExecuteScalar(Qcmd2).ToString(), out maxReceiptNo);

            int runningNo = 0;
            if (currentReceiptNo != null)
            {
                if (!int.TryParse(currentReceiptNo, out runningNo))
                {
                    runningNo = 0;
                }
                runningNo += 1;
            }

            return prefix + runningNo.ToString().PadLeft(maxReceiptNo, '0');
        }

        public static DataTable FetchDeliveryList(string RefNo, int POSID, DateTime StartDate, DateTime EndDate, string Search)
        {
            string SQLString;
            #region -= SQL STRING =-
            SQLString =
                "SET @RefNo = ISNULL(NULLIF(@RefNo, ''),'%'); " +
                "SET @Search = ISNULL(NULLIF(@Search, ''),'%'); " +
                "SET @EndDate = DATEADD(DAY,1,@EndDate); " +
                "SELECT OH.OrderHdrID, OD.OrderDetID, OrderDate, PointOfSaleID, IT.ItemNo, ItemName, Quantity, Barcode, CategoryName, DepartmentName " +
                    ", Attributes1, Attributes2, Attributes3, Attributes4, Attributes5, Attributes6, Attributes7, Attributes8 " +
                    ", MM.MembershipNo, MemberName, oh.ModeOfDelivery, oh.StoreReference, " +
                    "OH.DeliveryAddress, OH.DeliveryRemark " +
                    ", CASE WHEN ID.OrderDetID IS NOT NULL AND ID.OrderDetID <> '' THEN 'Delivered' ELSE 'Pending' END AS DeliveryStatus " +
                "FROM ( " +
                        "SELECT OrderHdrID, OrderDetID, ItemNo, Quantity " +
                        "FROM OrderDet " +
                        "WHERE OrderDetDate BETWEEN @StartDate AND @EndDate " +
                            "AND InventoryHdrRefNo IN ('DELIVERY') " +
                            "AND OrderHdrID LIKE @RefNo " +
                            "AND IsVoided = 0 " +
                    ") OD " +
                    "INNER JOIN (" +
                        "SELECT OrderHdr.OrderHdrID, OrderDate, OrderHdr.PointOfSaleID, OrderHdr.MembershipNo, OrderHdr.Userfld6 as StoreReference, OrderHdr.Userfld7 as ModeOfDelivery,   " +
                        "substring(ISNULL(ReceiptHdr.Remark,' ~ '), 1, CharIndex('~',ISNULL(ReceiptHdr.Remark,' ~ '))-1) AS DeliveryRemark, " +
                        "substring(ISNULL(ReceiptHdr.Remark,' ~ '), CharIndex('~',ISNULL(ReceiptHdr.Remark,' ~ '))+ 1, " +
                        "LEN(ISNULL(ReceiptHdr.Remark,' ~ ')) - CharIndex('~',ISNULL(ReceiptHdr.Remark,' ~ '))) AS DeliveryAddress " +
                        "FROM OrderHdr, ReceiptHdr " +
                        "WHERE OrderDate BETWEEN @StartDate AND @EndDate " +
                            "AND OrderHdr.OrderHdrID LIKE @RefNo AND OrderHdr.OrderHdrID = ReceiptHdr.OrderHdrID " +
                            "AND OrderHdr.PointOfSaleID = @POSID " +
                            "AND OrderHdr.IsVoided = 0 " +
                    ") OH ON OH.OrderHdrID = OD.OrderHdrID " +
                    "INNER JOIN ( " +
                        "SELECT ItemNo, ItemName, Barcode, Category.CategoryName, DepartmentName " +
                            ", ISNULL(Attributes1,'') AS Attributes1, ISNULL(Attributes2,'') AS Attributes2 " +
                            ", ISNULL(Attributes3,'') AS Attributes3, ISNULL(Attributes4,'') AS Attributes4 " +
                            ", ISNULL(Attributes5,'') AS Attributes5, ISNULL(Attributes6,'') AS Attributes6 " +
                            ", ISNULL(Attributes7,'') AS Attributes7, ISNULL(Attributes8,'') AS Attributes8 " +
                        "FROM Item " +
                            "INNER JOIN Category ON Item.CategoryName = Category.CategoryName " +
                            "INNER JOIN ItemDepartment ON Category.ItemDepartmentId = ItemDepartment.ItemDepartmentID " +
                    ") IT ON IT.ItemNo = OD.ItemNo " +
                    "INNER JOIN (SELECT MembershipNo, NameToAppear AS MemberName FROM Membership) MM ON MM.MembershipNo = OH.MembershipNo " +
                    "LEFT OUTER JOIN (SELECT DISTINCT UserFld1 AS OrderDetID FROM InventoryDet WHERE UserFld1 IS NOT NULL AND UserFld1 <> '') ID ON ID.OrderDetID = OD.OrderDetID "+
                "WHERE IT.ItemNo + '|' + ItemName + '|' + Barcode + '|' + CategoryName + '|' + DepartmentName " +
                    "+ '|' + ISNULL(Attributes1,'') + '|' + ISNULL(Attributes2,'') + '|' + ISNULL(Attributes3,'') " +
                    "+ '|' + ISNULL(Attributes4,'') + '|' + ISNULL(Attributes5,'') + '|' + ISNULL(Attributes6,'') " +
                    "+ '|' + ISNULL(Attributes7,'') + '|' + ISNULL(Attributes8,'') + '|' + MM.MembershipNo + '|' + MM.MemberName LIKE '%' + @Search + '%'";
            #endregion

            //if ()

            DataTable DT = new DataTable();
            #region *) Fetch: Load the Data
            QueryCommand Cmd = new QueryCommand(SQLString);
            Cmd.AddParameter("@RefNo", RefNo);
            Cmd.AddParameter("@POSID", POSID, DbType.Int32);
            Cmd.AddParameter("@StartDate", StartDate, DbType.DateTime);
            Cmd.AddParameter("@EndDate", EndDate, DbType.DateTime);
            Cmd.AddParameter("@Search", Search);

            DT.Load(DataService.GetReader(Cmd));
            #endregion

            return DT;
        }

        public static DataTable FetchDeliveryReport(string RefNo, int POSID, DateTime StartDate, DateTime EndDate, string Search)
        {
            string SQLString;
            #region -= SQL STRING =-
            if (string.IsNullOrEmpty(Search))
                Search = "%";

            if(string.IsNullOrEmpty(RefNo))
                RefNo = "%";
            SQLString =

                "select do.OrderNumber, do.DeliveryDate, do.DeliveryAddress, do.SalesOrderRefNo, do.MembershipNo, ISNULL(NULLIF(do.RecipientName, ''), m.NameToAppear) as RecipientName, " +
                "m.NameToAppear, dod.ItemNo, i.ItemName, dod.Quantity, ISNULL(do.PurchaseOrderRefNo,'') as CustomDONo, oh.userfld5 as CustomOrderRefNo, CASE ISNULL(do.IsDelivered,0) WHEN 1 THEN 'Delivered' ELSE 'Not Delivered' END as IsDelivered " +
                "from deliveryorder do, " +
                "deliveryorderdetails dod, " +
                "membership m, Item i, OrderHdr oh " +
                "where do.ordernumber = dod.DohdrID and do.membershipno = m.membershipNo and i.itemno = dod.itemno and do.SalesOrderRefNo = oh.OrderHdrID " +
                "and (do.deliverydate is null or do.deliverydate between '" + StartDate.ToString("yyyy-MM-dd") + "' and '" +
                EndDate.ToString("yyyy-MM-dd") + "') and ISNULL(do.membershipNo,'') + ISNULL(m.NameToAppear,'') + ISNULL(do.RecipientName,'') + ISNULL(dod.ItemNo,'') + ISNULL(i.itemname,'') like '%"
                + Search + "%' and (ISNULL(do.OrderNumber,'') like '%" + RefNo + "%' or ISNULL(do.PurchaseOrderRefNo,'') like '%" + RefNo + "%' or ISNULL(oh.userfld5,'') like '%" + RefNo + "%') " +
                "and ISNULL(oh.IsVoided,0) = 0 AND ISNULL(do.Deleted,0) = 0 ";
            #endregion

            //if ()

            DataTable DT = new DataTable();
            #region *) Fetch: Load the Data
            QueryCommand Cmd = new QueryCommand(SQLString);
            /*Cmd.AddParameter("@RefNo", RefNo);
            Cmd.AddParameter("@POSID", POSID, DbType.Int32);
            Cmd.AddParameter("@StartDate", StartDate, DbType.DateTime);
            Cmd.AddParameter("@EndDate", EndDate, DbType.DateTime);
            Cmd.AddParameter("@Search", Search);*/

            DT.Load(DataService.GetReader(Cmd));
            #endregion

            return DT;
        }

        public Membership getMemberInfo()
        {
            return myDeliveryOrderHdr.Membership;
        }

        public bool AssignMembership(string tmpMembershipNo)
        {
            DateTime ExpiryDate;
            Membership CurrentMember;
            if (MembershipController.CheckMembershipValid
                (tmpMembershipNo, out CurrentMember, out ExpiryDate))
            {
                myDeliveryOrderHdr.MembershipNo = tmpMembershipNo;
                myDeliveryOrderHdr.Membership = CurrentMember;
                return true;

            }
            else
            {
                return false;
            }
        }

        public DataTable FetchDeliveryItems()
        {
            DataTable dt = myDeliveryOrderDet.ToDataTable();
            dt.Columns.Add("ItemName");
            foreach (DataRow dr in dt.Rows)
            {
                Item i = new Item(dr["ItemNo"]);
                if (i != null && i.IsLoaded && i.ItemNo != "")
                {
                    dr["ItemName"] = i.ItemName;
                }
            }
            return dt;
        }

        public static string CreateNewDeliveryNo(int PointOfSaleID)
        {
            int runningNo = 0;

            //use stored procedure to pull out the biggest number for today's order
            //format of order: YYMMDDSSSSNNNN
            //This stored procedure pull out the last order number
            IDataReader ds = PowerPOS.SPs.GetNewDeliveryNoByPointOfSaleID(PointOfSaleID).GetReader();
            while (ds.Read())
            {
                string result = ds.GetValue(0).ToString();
                if (result.EndsWith("W")) result = result.TrimEnd('W');
                if (!int.TryParse(result, out runningNo))
                {
                    runningNo = 0;
                }
            }
            ds.Close();
            runningNo += 1;

            //YYMMDDSSSSNNNN                
            //YY - year
            //MM - month
            //DD - day
            //SSSS - PointOfSale ID
            //NNNN - Running No
            return DateTime.Now.ToString("yyMMdd") + PointOfSaleID.ToString().PadLeft(4, '0') + runningNo.ToString().PadLeft(4, '0');
        }

        public void LoadFromPOS(string orderhdrid, out string status)
        {
            status = "";
            POSController pos = new POSController(orderhdrid);
            if (pos.myOrderHdr.IsLoaded)
            {
                LoadFromPOS(pos, out status);
            }
        }

        public void LoadFromPOS(POSController pos, out string status)
        {
            status = "";
            if (!pos.MembershipApplied())  // Allow Delivery Order to WALK-IN membership   // || pos.myOrderHdr.MembershipNo == "WALK-IN"
            {
                status = "No Membership Applied";
                return;
            }

            if (pos.IsVoided())
            {
                status = "Error. Order already voided.";
                return;
            }

            myDeliveryOrderHdr.SalesOrderRefNo = pos.myOrderHdr.OrderHdrID;
            myDeliveryOrderHdr.Membership = pos.GetMemberInfo();
            myDeliveryOrderHdr.MembershipNo = pos.GetMemberInfo().MembershipNo;

            OrderDetCollection coll = pos.FetchUnsavedOrderDet();
            int runningno = 1;
            foreach (OrderDet c in coll)
            {
                if (!c.IsVoided)
                {
                    DeliveryOrderDetail dod = new DeliveryOrderDetail();
                    dod.DetailsID = myDeliveryOrderHdr.OrderNumber + "." + runningno;
                    dod.Dohdrid = myDeliveryOrderHdr.OrderNumber;
                    dod.ItemNo = c.ItemNo;
                    dod.Quantity = c.Quantity;
                    dod.OrderDetID = c.OrderDetID;
                    myDeliveryOrderDet.Add(dod);
                    runningno++;
                }
            }
        }

        public static DataTable FetchDeliveryOrderToPrint(string OrderNumber)
        {
            string querystr = @"select do.PurchaseOrderRefNo as OrderNumber, do.DeliveryDate, 
                        ISNULL(m.StreetName,'') + ' ' + ISNULL(m.streetName2,'') + ' ' + ISNULL(m.Country,'') + ' ' + ISNULL(m.ZipCode,'') as MembershipAddress, 
                        ISNULL(do.DeliveryAddress,'') as DeliveryAddress,  
                        do.SalesOrderRefno, do.MembershipNo, dod.ItemNo, i.ItemName + ' ' + ISNULL(od.Remark, '') as [ItemName], 
                        dod.Quantity, m.NameToAppear, CASE WHEN ISNULL(do.HomeNo,'') = '' THEN do.MobileNo ELSE do.HomeNo END as Home, m.Fax, do.remark, sr.salespersonID,  
                        ISNULL(ap.AppSettingValue,'') as Terms, do.RecipientName as ContactPerson, dod.Remarks as LineRemarks,    
                        m.ZipCode as MembershipPostalCode, m.Mobile + ' - ' + m.Home as MembershipPhoneNo, 
                        do.PostalCode as DeliveryPostalCode, do.MobileNo + ' - ' + do.HomeNo as DeliveryPhoneNo, 
                        oh.userfld5 as InvoiceNo, oh.OrderDate as InvoiceDate, pos.OutletName,   
                        od.UnitPrice, od.Amount as LineAmount, oh.GrossAmount, oh.NettAmount, oh.GSTAmount, 
                        od.GrossSales as LineGrossAmount, od.GSTAmount as LineGSTAmount, 
                        ISNULL(od.userfloat1,0) as LineDepositAmount, od.Amount - ISNULL(od.userfloat1,0) as LineOutstanding 
		                ,do.TimeSlotFrom
		                ,do.TimeSlotTo  
                        from deliveryorder do 
                        LEFT JOIN deliveryorderdetails dod on do.ordernumber = dod.dohdrid 
                        LEFT JOIN Membership m on do.membershipno = m.membershipno 
                        LEFT JOIN Item i on dod.itemno = i.itemno 
                        LEFT JOIN Orderhdr oh on do.SalesOrderRefNo = oh.orderhdrid 
                        LEFT JOIN SalesCommissionRecord sr on sr.orderhdrid = do.SalesOrderRefNo 
                        LEFT JOIN appsetting ap on ap.AppSettingKey = 'DeliveryTerms' 
                        LEFT JOIN PointOfSale pos on pos.PointOfSaleID = oh.PointOfSaleID 
                        LEFT JOIN OrderDet od on od.OrderHdrID = oh.OrderHdrID and od.OrderDetID = dod.OrderDetID 
                        WHERE do.ordernumber = '" + OrderNumber + "' AND ISNULL(dod.Deleted, 0) = 0";

            DataSet ds1 = new DataSet();

            ds1 = DataService.GetDataSet(new QueryCommand(querystr));

            // Get the Payment Made and Outstanding Balance
            if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds1.Tables[0].Rows[0];
                decimal totalInvoice = decimal.Parse(dr["NettAmount"].ToString());
                decimal outstanding = Installment.GetOutstandingBalance(dr["SalesOrderRefno"].ToString(), DateTime.Now);
                decimal paymentMade = totalInvoice - outstanding;

                ds1.Tables[0].Columns.Add("PaymentMade", Type.GetType("System.Decimal"));
                ds1.Tables[0].Columns.Add("Outstanding", Type.GetType("System.Decimal"));
                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {
                    dr = ds1.Tables[0].Rows[i];
                    dr["PaymentMade"] = paymentMade;
                    dr["Outstanding"] = outstanding;
                }
            }

            return ds1.Tables[0];
        }

        public static string DeliveryGetDeliveryTrack(string orderdetid)
        {
            try
            {
                string sql = "select ROW_NUMBER() over(order by d.Orderdetid asc) as DeliveryNo, " +
                                "convert(varchar(50), h.deliverydate, 106) as DeliveryDate, d.Quantity as DeliveryQty, " +
                                "Case isnull(IsDelivered, 0) when 1 then 'Delivered' else 'Pending' end as DeliveryStatus, d.OrderDetID,  d.DOHdrID " +
                                "from deliveryorderdetails d inner join deliveryorder h on d.DOHDRID = h.OrderNumber " +
                                "where d.OrderDetID = '" + orderdetid + "'";

                DataTable ds = DataService.GetDataSet(new QueryCommand(sql)).Tables[0];

                return Newtonsoft.Json.JsonConvert.SerializeObject(ds);
            }
            catch (Exception ex)
            {
                Logger.writeLog("Error Get Delivery Track Data:" + ex.Message);
                return string.Empty;
            }
        }

        public static bool DeliverySetDelivered(string doHdrID, string username, out string status)
        {
            status = "";
            try
            {
                Query qr = new Query("DeliveryOrder");
                qr.QueryType = QueryType.Update;
                qr.AddWhere(DeliveryOrder.Columns.OrderNumber, doHdrID);
                qr.AddUpdateSetting(DeliveryOrder.Columns.IsDelivered, true);
                qr.AddUpdateSetting(DeliveryOrder.Columns.ModifiedOn, DateTime.Now);
                qr.AddUpdateSetting(DeliveryOrder.Columns.ModifiedBy, username);

                DataService.ExecuteQuery(qr.BuildUpdateCommand());

                return true;
            }
            catch (Exception ex)
            {
                status = "Error when Set Delivered :" + ex.Message;
                Logger.writeLog("Error when Set Delivered :" + ex.Message);
                return false;
            }
        }

        public static string FetchDeliveryOrderToPrintByOrderDetID(string OrderDetID)
        {
            try
            {
                DataTable dt = new DataTable();

                string querystr = @"select do.PurchaseOrderRefNo as OrderNumber, do.DeliveryDate, 
                        ISNULL(m.StreetName,'') + ' ' + ISNULL(m.streetName2,'') + ' ' + ISNULL(m.Country,'') + ' ' + ISNULL(m.ZipCode,'') as MembershipAddress, 
                        ISNULL(do.DeliveryAddress,'') as DeliveryAddress,  
                        do.SalesOrderRefno, do.MembershipNo, dod.ItemNo, i.ItemName + ' ' + ISNULL(od.Remark, '') as [ItemName], 
                        dod.Quantity, m.NameToAppear, CASE WHEN ISNULL(do.HomeNo,'') = '' THEN do.MobileNo ELSE do.HomeNo END as Home, m.Fax, do.remark, sr.salespersonID,  
                        ISNULL(ap.AppSettingValue,'') as Terms, do.RecipientName as ContactPerson, dod.Remarks as LineRemarks,    
                        m.ZipCode as MembershipPostalCode, m.Mobile + ' - ' + m.Home as MembershipPhoneNo, 
                        do.PostalCode as DeliveryPostalCode, do.MobileNo + ' - ' + do.HomeNo as DeliveryPhoneNo, 
                        oh.userfld5 as InvoiceNo, oh.OrderDate as InvoiceDate, pos.OutletName,   
                        od.UnitPrice, od.Amount as LineAmount, oh.GrossAmount, oh.NettAmount, oh.GSTAmount, 
                        od.GrossSales as LineGrossAmount, od.GSTAmount as LineGSTAmount, 
                        ISNULL(od.userfloat1,0) as LineDepositAmount, od.Amount - ISNULL(od.userfloat1,0) as LineOutstanding 
		                ,do.TimeSlotFrom
		                ,do.TimeSlotTo  
                        ,oh.userfld1 as OHUserfld1 ,oh.userfld2 as OHUserfld2 ,oh.userfld3 as OHUserfld3 ,oh.userfld4 as OHUserfld4 ,oh.userfld5 as OHUserfld5
                        ,oh.userfld6 as OHUserfld6 ,oh.userfld7 as OHUserfld7 ,oh.userfld1 as OHUserfld8 ,oh.userfld9 as OHUserfld9 ,oh.userfld10 as OHUserfld10 
                        ,i.Attributes1, i.Attributes2, i.Attributes3, i.Attributes4, i.Attributes5, i.Attributes6, i.Attributes7, i.Attributes8    
                        from deliveryorder do 
                        LEFT JOIN deliveryorderdetails dod on do.ordernumber = dod.dohdrid 
                        LEFT JOIN Membership m on do.membershipno = m.membershipno 
                        LEFT JOIN Item i on dod.itemno = i.itemno 
                        LEFT JOIN Orderhdr oh on do.SalesOrderRefNo = oh.orderhdrid 
                        LEFT JOIN SalesCommissionRecord sr on sr.orderhdrid = do.SalesOrderRefNo 
                        LEFT JOIN appsetting ap on ap.AppSettingKey = 'DeliveryTerms' 
                        LEFT JOIN PointOfSale pos on pos.PointOfSaleID = oh.PointOfSaleID 
                        LEFT JOIN OrderDet od on od.OrderHdrID = oh.OrderHdrID and od.OrderDetID = dod.OrderDetID 
                        WHERE dod.orderdetid = '" + OrderDetID + "' AND ISNULL(dod.Deleted, 0) = 0";

                DataSet ds1 = new DataSet();

                ds1 = DataService.GetDataSet(new QueryCommand(querystr));

                // Get the Payment Made and Outstanding Balance
                if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds1.Tables[0].Rows[0];
                    decimal totalInvoice = decimal.Parse(dr["NettAmount"].ToString());
                    decimal outstanding = Installment.GetOutstandingBalance(dr["SalesOrderRefno"].ToString(), DateTime.Now);
                    decimal paymentMade = totalInvoice - outstanding;

                    ds1.Tables[0].Columns.Add("PaymentMade", Type.GetType("System.Decimal"));
                    ds1.Tables[0].Columns.Add("Outstanding", Type.GetType("System.Decimal"));
                    for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                    {
                        dr = ds1.Tables[0].Rows[i];
                        dr["PaymentMade"] = paymentMade;
                        dr["Outstanding"] = outstanding;
                    }
                }

                dt = ds1.Tables[0];

                return Newtonsoft.Json.JsonConvert.SerializeObject(dt);
            }
            catch (Exception ex)
            {
                Logger.writeLog("Error Get Delivery Data" + ex.Message);
                return string.Empty;
            }
        }
    }
}
