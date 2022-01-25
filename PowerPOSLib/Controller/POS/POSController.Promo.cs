using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using SubSonic;
using PowerPOS.Container;
using System.Data;

namespace PowerPOS
{
    public partial class POSController
    {
        ApplyPromotionController promoCtrl;

        public bool GetPromotionInfo(out string campaignName, out string campaignType)
        {

            return promoCtrl.GetCurrentPromoInfo(out campaignName, out campaignType);

        }

        public bool SetOrderLineToUsePromoPrice(string OrderDetID, bool UsePromoPrice)        
        {
            try
            {
                OrderDet myOrderDetItem;

                myOrderDetItem = (PowerPOS.OrderDet)myOrderDet.Find(OrderDetID);
                myOrderDetItem.UsePromoPrice = UsePromoPrice;

                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return false;
            }
        }

        public bool SetOrderLineAsPromo
            (string OrderDetID, bool IsPromo, int promoHdrID, int promoDetID, out string status)
        {
            status = "";
            try
            {
                OrderDet myOrderDetItem;

                myOrderDetItem = (PowerPOS.OrderDet)myOrderDet.Find(OrderDetID);
                myOrderDetItem.IsPromo = IsPromo;
                if (IsPromo)
                {
                    //Copy by default - change if it is customized....
                    myOrderDetItem.PromoAmount = 0;  //myOrderDetItem.Amount;
                    myOrderDetItem.PromoDiscount = 0; //(double)myOrderDetItem.Discount;
                    myOrderDetItem.PromoHdrID = promoHdrID;
                    myOrderDetItem.PromoDetID = promoDetID;                    
                }
                else
                {
                    //From Promo to Non Promo....
                    myOrderDetItem.PromoAmount = 0;
                    myOrderDetItem.PromoDiscount = 0;
                    myOrderDetItem.PromoUnitPrice = 0;
                    myOrderDetItem.UsePromoPrice = false;
                    myOrderDetItem.PromoHdrID = null;
                    myOrderDetItem.PromoDetID = null;
                    myOrderDetItem.Discount = preferredDiscount;
                    myOrderDetItem.DiscountDetail = preferredDiscount.ToString("N0") + "%";
                    CalculateLineAmount(ref myOrderDetItem);
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = "";
                return false;
            }
        }

        //Set Promo Discount
        internal void SetOrderLinePromoDiscount
            (string OrderLineID, double newDiscount, out string status)
        {
            status = "";
            try
            {
                OrderDet myOrderDetItem;


                myOrderDetItem = (PowerPOS.OrderDet)myOrderDet.Find(OrderLineID);

                if (myOrderDetItem.IsPromo)
                    myOrderDetItem.PromoDiscount = newDiscount;

                CalculateLineAmount(ref myOrderDetItem);

                return;

            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = "Unable to to set PromoDiscount for line Item with OrderDetID = " + OrderLineID + ". Detail error: " + ex.Message;
                return;
            }
        }

        internal void SetOrderLinePromoDiscountNew
            (string OrderLineID, double newDiscount, out string status)
        {
            status = "";
            try
            {
                OrderDet myOrderDetItem;


                myOrderDetItem = (PowerPOS.OrderDet)myOrderDet.Find(OrderLineID);

                if (myOrderDetItem.IsPromo)
                {
                    myOrderDetItem.PromoDiscount = newDiscount;
                    var substract = (double)newDiscount / 100 * (double)myOrderDetItem.UnitPrice;
                    myOrderDetItem.PromoUnitPrice = myOrderDetItem.UnitPrice - (decimal)substract;
                }

                CalculateLineAmount(ref myOrderDetItem);

                return;

            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = "Unable to to set PromoDiscount for line Item with OrderDetID = " + OrderLineID + ". Detail error: " + ex.Message;
                return;
            }
        }

        internal void SetOrderLinePromoUnitPrice
            (string OrderLineID, decimal promoUnitPrice, out string status)
        {
            status = "";
            try
            {
                OrderDet myOrderDetItem;


                myOrderDetItem = (PowerPOS.OrderDet)myOrderDet.Find(OrderLineID);

                if (myOrderDetItem.IsPromo)
                    myOrderDetItem.PromoUnitPrice = promoUnitPrice;

                CalculateLineAmount(ref myOrderDetItem);

                return;

            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = "Unable to to set PromoPrice for line Item with OrderDetID = " + OrderLineID + ". Detail error: " + ex.Message;
                return;
            }
        }

        internal void SetOrderLinePromoUnitPriceNew
            (string OrderLineID, decimal promoUnitPrice, out string status)
        {
            status = "";
            try
            {
                OrderDet myOrderDetItem;


                myOrderDetItem = (PowerPOS.OrderDet)myOrderDet.Find(OrderLineID);

                if (myOrderDetItem.IsPromo)
                {
                    myOrderDetItem.PromoUnitPrice = promoUnitPrice;
                    var substract = ((myOrderDetItem.UnitPrice - myOrderDetItem.PromoUnitPrice) / myOrderDetItem.UnitPrice) * 100;
                    myOrderDetItem.PromoDiscount = (double)substract;
                }

                CalculateLineAmount(ref myOrderDetItem);

                return;

            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = "Unable to to set PromoPrice for line Item with OrderDetID = " + OrderLineID + ". Detail error: " + ex.Message;
                return;
            }
        }

        //Set Promo Amount
        internal void SetOrderLinePromoAmount(string OrderLineID, decimal amount, out string status)
        {
            status = "";
            try
            {
                OrderDet myOrderDetItem;


                myOrderDetItem = (PowerPOS.OrderDet)myOrderDet.Find(OrderLineID);

                if (myOrderDetItem.IsPromo)
                    myOrderDetItem.PromoAmount = amount;

                CalculateLineAmount(ref myOrderDetItem);

                return;

            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = "Unable to to set PromoAmount for line Item with OrderDetID = " + OrderLineID + ". Detail error: " + ex.Message;
                return;
            }
        }

        //Set Promo Amount
        internal void SetOrderLinePromoAmountToUseDefaultUnitPrice(string OrderLineID, out string status)
        {
            status = "";
            try
            {
                OrderDet myOrderDetItem;


                myOrderDetItem = (PowerPOS.OrderDet)myOrderDet.Find(OrderLineID);

                if (myOrderDetItem.IsPromo)
                    myOrderDetItem.PromoUnitPrice = myOrderDetItem.UnitPrice;

                CalculateLineAmount(ref myOrderDetItem);

                return;

            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = "Unable to to set PromoAmount for line Item with OrderDetID = " + OrderLineID + ". Detail error: " + ex.Message;
                return;
            }
        }
        public void UnSetAllIsPromoLine()
        {
            int i = 0; string status;
            while (i < myOrderDet.Count)
            {
                if (myOrderDet[i].IsPromo)
                {
                    SetOrderLineAsPromo(myOrderDet[i].OrderDetID, false,0,0, out status);
                }
                else
                {
                    i++;
                }
            }
        }

        public void DeleteAllIsPromoFOCLine()
        {
            int i = 0;
            while (i < myOrderDet.Count)
            {
                if ((bool)myOrderDet[i].IsPromoFreeOfCharge)
                {
                    myOrderDet.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }
        }

        public void UndoPromo()
        {
            promoCtrl.UndoPromoToOrder();
        }

        public void ApplyPromo()
        {
            promoCtrl.ApplyPromoToOrder();
        }

    }
}
