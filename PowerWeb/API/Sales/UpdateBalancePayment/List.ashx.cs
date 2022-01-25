using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using Newtonsoft.Json;
using SubSonic;

namespace PowerWeb.API.Sales.UpdateBalancePayment
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class List : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string filterStartDate = context.Request.Params["StartDate"] ?? "";
            string filterEndDate = context.Request.Params["EndDate"] ?? "";
            string filterMembershipNo = context.Request.Params["MembershipNo"] ?? "";
            string filterMembershipName = context.Request.Params["MembershipName"] ?? "";
            string filterReceiptNo = context.Request.Params["ReceiptNo"] ?? "";
            string filterOutlet = context.Request.Params["Outlet"] ?? "";
            string filterDeliveryNo= context.Request.Params["DeliveryNo"] ?? "";
            string filterInvoiceNo = context.Request.Params["InvoiceNo"] ?? "";
            string filterOutstanding = context.Request.Params["Outstanding"] ?? "";

            string strSql = @"
                    DECLARE @credit AS VARCHAR(50); 
                    DECLARE @creditpayment AS VARCHAR(50); 
                    DECLARE @startdate AS DATETIME; 
                    DECLARE @enddate AS DATETIME; 
                    DECLARE @membershipno AS VARCHAR(50);  

                    SET @credit = 'INSTALLMENT'; 
                    SET @creditpayment = 'INST_PAYMENT';

                    begin try drop table #temp_1 end try begin catch end catch;
                    begin try drop table #temp_2 end try begin catch end catch;
                    begin try drop table #temp_3 end try begin catch end catch;

                    select * 
                      into #temp_1 
                      from ( 
	                    SELECT OrderHdrID AS ReceiptNo
		                     , ISNULL(OH.userfld5,OH.OrderHdrID) as OrderRefNo
		                     , ISNULL(OH.userfld5,OH.OrderHdrID) AS PaymentRefNo
		                     , OrderHdrID as PaymentFor
		                     , OrderDate AS ReceiptDate
		                     , ISNULL(SUM(Amount),0.00) AS Credit
		                     , 0.00 AS Debit FROM OrderHdr OH 
	                     inner join ReceiptDet RD 
		                    on OH.OrderHdrID = RD.ReceiptHdrID 
	                     inner join Membership MM 
		                    on MM.MembershipNo = OH.MembershipNo 
	                     WHERE OH.IsVoided = 0 
	                       AND PaymentType = @credit 
	                     GROUP BY OrderDate
		                     , OrderHdrID, OH.UserFld5  
                    	 
	                     UNION ALL  

	                    SELECT OH.OrderHdrId as ReceiptNo
		                     , ISNULL(OH.userfld5,OH.OrderHdrID) as OrderRefNo
		                     , ( SELECT ISNULL(OrderHdr.UserFld5,OrderHdrId) 
			                       FROM OrderHdr 
			                      WHERE OrderHdr.OrderHdrId= OD.Userfld3
		                       ) AS PaymentRefno
		                     , OD.Userfld3 as PaymentFor
		                     , OrderDate as ReceiptDate 
		                     , 0.00 as Credit
		                     , ISNULL(SUM(AMOUNT),0.00) as debit  
	                      FROM OrderHdr OH 
	                     INNER JOIN OrderDet OD 
		                    ON OH.OrderHdrID = OD.OrderHdrID 
	                     INNER JOIN Membership MM 
		                    on MM.MembershipNo = OH.MembershipNo 
	                     WHERE OH.IsVoided = 0 
	                       and OD.IsVoided = 0 
	                       AND ItemNo = @creditpayment 
	                     GROUP BY OH.OrderHdrID 
		                     , OD.Userfld3  
		                     , OrderDate 
		                     , OH.UserFld5 
	                    ) as temp_1;

                    select *
				  into #temp_2
				  from (
						select a.PaymentFor as InvoiceNo
							 , b.PointOfSaleID
							 , d.OutletName
							 , b.OrderDate as InvoiceDate
							 , b.MembershipNo
							 , c.NameToAppear as RecipientName
							 , c.Email
							 , c.StreetName
							 , c.StreetName2
							 , c.Block
							 , c.City
							 , f.PostalCode
							 , c.Country
							 , OfficeNo = isnull(c.Office, '')
							 , HomeNo = isnull(f.HomeNo, c.Home)
							 , MobileNo = isnull(f.MobileNo, c.Mobile)
							 , Remarks = isnull(f.Remark, '')
							 , sum(a.Credit) as Credit
							 , sum(a.Debit) as Debit
							 , BalancePayment = (sum(a.Credit) - sum(a.Debit))
							 , f.OrderNumber as DeliveryNo
							 , f.UnitNo
							 , g.Area1 as DeliveryAddress
							 , f.DeliveryDate 
							 , DeliveryTime = substring(convert(varchar, f.TimeSlotFrom, 108), 0, 6) + ' - ' + substring(convert(varchar, f.TimeSlotTo, 108), 0, 6)
							 , DeliveryDateDetails = convert(varchar, f.DeliveryDate, 106) + '  ' + substring(convert(varchar, f.TimeSlotFrom, 108), 0, 6) + ' - ' + substring(convert(varchar, f.TimeSlotTo, 108), 0, 6)
							 , b.userfld5 as CustomInvoiceNo
							 , f.PurchaseOrderRefNo as CustomDeliveryNo
						  from #temp_1 a 
						 inner join OrderHdr b
							on b.OrderHdrID = a.PaymentFor
						 inner join Membership c
							on c.MembershipNo = b.MembershipNo 
						 inner join PointOfSale d
							on d.PointOfSaleID = b.PointOfSaleID
						 inner join Outlet e
							on e.OutletName = d.OutletName
						 inner join DeliveryOrder f
							on f.SalesOrderRefNo = b.OrderHdrID 
						 inner join ReceiptHdr h 
							on h.OrderHdrID = a.PaymentFor
						  left join PostalCodeDB g
							on g.ZIP = f.PostalCode
						 where 1=1
						   AND f.DeliveryDate between '" + filterStartDate + @"' and '" + filterEndDate + @" 23:59:59'
                           and (a.PaymentFor like '%" + filterInvoiceNo + @"%' or b.userfld5 like '%" + filterInvoiceNo + @"%')
                           and (f.OrderNumber like '%" + filterDeliveryNo + @"%' or f.PurchaseOrderRefNo like '%" + filterDeliveryNo + @"%')
                        -- and b.MembershipNo like '%" + filterMembershipNo + @"%'
                        -- and c.NameToAppear like '%" + filterMembershipName + @"%'
                        -- and d.OutletName like '%" + filterOutlet + @"%'
                         group by a.PaymentFor
							 , b.MembershipNo
							 , c.NameToAppear
							 , c.Email
							 , f.Remark 
							 , c.Home 
							 , c.Mobile 
							 , c.Office 
							 , b.OrderDate
							 , c.StreetName
							 , c.StreetName2
							 , c.Block
							 , c.BuildingName
							 , c.UnitNo
							 , c.City
							 , f.PostalCode
							 , c.Country
							 , d.OutletName
							 , b.PointOfSaleID
							 , f.DeliveryDate
							 , f.HomeNo
							 , f.MobileNo
							 , f.OrderNumber
							 , f.UnitNo
							 , g.Area1
							 , h.ReceiptRefNo
							 , f.TimeSlotFrom
							 , f.TimeSlotTo
							 , b.userfld5
							 , f.PurchaseOrderRefNo
						) as temp_2;

					select a.*
					     , Status = (
								case
									when a.BalancePayment > 0 then '1'
									when a.BalancePayment = 0 then '0'
								end
						   )
                         , PaymentRemarks = STUFF((
                                  SELECT ', ' + rh.Remark
                                  FROM OrderDet od
                                    inner join ReceiptHdr rh on rh.OrderHdrID = od.OrderHdrID
                                  WHERE od.userfld3 = a.InvoiceNo
                                  FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, '')
					  from #temp_2 a
					 where (
								case
									when a.BalancePayment > 0 then '1'
									when a.BalancePayment = 0 then '0'
								end
						   ) like '%" + filterOutstanding + @"%';
            ";

            QueryCommand cmd = new QueryCommand(strSql, "PowerPOS");
            var data = DataService.GetDataSet(cmd).Tables[0];

            context.Response.ContentType = "application/json";
            context.Response.Write(JsonConvert.SerializeObject(data));
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
