using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PowerPOS
{
    public partial class POSController
    {
        public bool CreateAutoDelivery(out DeliveryOrderCollection delOrderHdrColl, out DeliveryOrderDetailCollection delOrderDetColl, out string status) {
            delOrderHdrColl = new DeliveryOrderCollection();
            delOrderDetColl = new DeliveryOrderDetailCollection();
            status = "";
            try
            {
                if(!MembershipApplied())
                    throw new Exception("Must assign member to create delivery");

                Logger.writeLog("delivery for " + GetMemberInfo().MembershipNo);

                DeliveryOrder doHdr = new DeliveryOrder();

                doHdr.OrderNumber = "0";
                doHdr.PersonAssigned = -1;
                doHdr.MembershipNo = GetMemberInfo().MembershipNo;
                doHdr.RecipientName = GetMemberInfo().NameToAppear;
                doHdr.MobileNo = GetMemberInfo().Mobile;
                doHdr.HomeNo = GetMemberInfo().Home;
                doHdr.PostalCode = GetMemberInfo().ZipCode;
                doHdr.DeliveryAddress = GetMemberInfo().StreetName + Environment.NewLine +
                                  GetMemberInfo().StreetName2 + Environment.NewLine +
                                  GetMemberInfo().Country + " " + GetMemberInfo().ZipCode;
                doHdr.UnitNo = "";
                doHdr.Remark = "Delivery for " + GetMemberInfo().NameToAppear;
                doHdr.IsVendorDelivery = false;
                DateTime delivery = DateTime.Now.AddDays(1); 
                doHdr.DeliveryDate = new DateTime(delivery.Year,delivery.Month, delivery.Day, 0,0,0) ;
                doHdr.TimeSlotFrom = new DateTime(delivery.Year, delivery.Month, delivery.Day, 10, 0, 0);
                doHdr.TimeSlotTo = new DateTime(delivery.Year, delivery.Month, delivery.Day, 13, 0, 0);

                delOrderHdrColl.Add(doHdr);

                
                for (int i = 0; i < myOrderDet.Count; i++)
                {
                    DeliveryOrderDetail doDet = new DeliveryOrderDetail();
                    OrderDet od = myOrderDet[i];
                    doDet.Dohdrid = "0";
                    doDet.ItemNo = od.ItemNo;
                    doDet.Quantity = od.Quantity;
                    doDet.DetailsID = doDet.Dohdrid + "." + i.ToString();
                    doDet.OrderDetID = od.OrderDetID;

                    delOrderDetColl.Add(doDet);
                }

                
                

                return true;
            }
            catch (Exception ex) 
            {
                Logger.writeLog("Error auto create delivery" + ex.Message);
                status = "Error auto create delivery" + ex.Message;
                return false;
            }
        }
    }
}
