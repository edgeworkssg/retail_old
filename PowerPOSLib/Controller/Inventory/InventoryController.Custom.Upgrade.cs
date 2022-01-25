using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Transactions;
using System.Collections;
using PowerPOS.Container;
using SubSonic;
using System.ComponentModel;
namespace PowerPOS
{
    public partial class InventoryController
    {
        public void GetInventoryStockTakeTakenBy(out string TakenBy, out string verifiedBy)
        {
            TakenBy = InvHdr.Userfld1;
            verifiedBy = InvHdr.Userfld2;
        }

        public void setInventoryStockTakeTakenBy(string takenBy, string verifiedBy)
        {
            InvHdr.Userfld1 = takenBy;
            InvHdr.Userfld2 = verifiedBy;
        }

        public string GetMovementType()
        {
            return InvHdr.MovementType;
        }

        public string getPurchaseOrderNo()
        {
            return InvHdr.PurchaseOrderNo;
        }

        public string getVendorInvoiceNo()
        {
            return InvHdr.VendorInvoiceNo;
        }

        public string getSupplier()
        {
            return InvHdr.Supplier;
        }

        public string getSupplierName()
        {
            string supplierName = "";
            int supplierID;
            if ((InvHdr.Supplier != "") && (Int32.TryParse(InvHdr.Supplier, out supplierID)))
            {
                Supplier sup = new Supplier(supplierID);

                if (sup != null)
                    supplierName = sup.SupplierName;
            }

            return supplierName;
        }

        public decimal GetFreightCharges()
        {
            if (InvHdr.FreightCharge.HasValue)
                return InvHdr.FreightCharge.Value;
            return 0;
        }

        public decimal getDiscount()
        {
            if (InvHdr.Discount.HasValue)
                return InvHdr.Discount.Value;
            return 0;
        }

        public double getExchangeRate()
        {
            return InvHdr.ExchangeRate;
        }
        
        public string GetRemark()
        {
            return InvHdr.Remark;
        }

        public DateTime GetInventoryDate()
        {
            return InvHdr.InventoryDate;
        }

        public int GetInventoryLocationID()
        {
            if (InvHdr.InventoryLocationID.HasValue)
                return InvHdr.InventoryLocationID.Value;

            return 0;
        }

        public string GetInventoryLocation()
        {
            if (InvHdr.InventoryLocation != null)
                return InvHdr.InventoryLocation.InventoryLocationName;

            return "";
        }

        public void setInventoryStockOutReasonID(int stockOutReasonID)
        {
            InvHdr.StockOutReasonID = stockOutReasonID;
        }

        public int getStockOutReasonID()
        {
            if (InvHdr.StockOutReasonID.HasValue)
                return InvHdr.StockOutReasonID.Value;
            return 0;
        }

        public int getTransferDestination()
        {
            if (InvHdr.TmpSavedData.HasValue)
                return InvHdr.TmpSavedData.Value;
            return 0;
        }

        public void setTmpSavedData(int i)
        {
            InvHdr.TmpSavedData = i;
        }

        public int getStockAdjustmentDirection()
        {
            if (InvHdr.TmpSavedData.HasValue)
                return InvHdr.TmpSavedData.Value;
            return 0;
        }
        
        public bool AddInvDet(InventoryDetCollection invDetCol)
        {
            try
            {
                InvDet.AddRange(invDetCol);

                //make sure no duplicate refno
                for (int i = 0; i < InvDet.Count; i++)
                {
                    InvDet[i].InventoryDetRefNo = "." + (i + 1).ToString();
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return false;
            }
        }
        
        public InventoryDet GetInvDetClone(string ID)
        {

            try
            {
                return ((PowerPOS.InventoryDet)InvDet.Find(ID)).Clone();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }

        }
        
        public Guid getUniqueID()
        {
            return InvHdr.UniqueID;
        }
        
        public bool ChangeRemark(string ID, string remark, out string status)
        {
            status = "";
            try
            {
                InventoryDet myTmpDet;

                myTmpDet = (PowerPOS.InventoryDet)InvDet.Find(ID);

                if (myTmpDet == null)
                    return false;

                myTmpDet.Remark = remark;

                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return false;
            }
        }
        
        public bool SetFreightCharges(decimal FreightCharges)
        {
            InvHdr.FreightCharge = FreightCharges;

            return true;
        }
        
        public bool SetDiscount(decimal Discount)
        {
            InvHdr.Discount = Discount;

            return true;
        }
        
        public bool SetExchangeRate(double ExchangeRate)
        {
            InvHdr.ExchangeRate = ExchangeRate;

            return true;
        }

        public bool SetSupplier(string Supplier)
        {
            InvHdr.Supplier = Supplier;

            return true;
        }

        public bool SetRemark(string remark)
        {
            InvHdr.Remark = remark;
            return true;
        }
        
        public bool MarkAsDeletedFromInventoryDetail(string ID, bool value, out string status)
        {
            status = "";
            try
            {
                ((InventoryDet)InvDet.Find(ID)).Deleted = value;
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return false;
            }
        }

		public void MarkAllAsDeletedFromInventoryDetail()
		{
			foreach (var invDet in InvDet)
				invDet.Deleted = true;
		}

		public void UnmarkkAllAsDeletedFromInventoryDetail()
		{
			foreach (var invDet in InvDet)
				invDet.Deleted = false;
		}

		public void InvertAllAsDeletedFromInventoryDetail()
		{
			foreach (var invDet in InvDet)
				invDet.Deleted = !invDet.Deleted;
		}

		public decimal GetTotalCostOfGoods()
		{
			var result = 0M;

			foreach (var invDet in InvDet)
                result += invDet.FactoryPrice * invDet.Quantity.GetValueOrDefault(0);

			return result;
		}

        public decimal GetTotalQuantity()
        {
            var result = 0M;

            foreach (var invDet in InvDet)
                result += invDet.Quantity.GetValueOrDefault(0);

            return result;
        }

        public bool LoadFromDataTable(DataTable hdr, DataTable det)
        {
            try
            {
                InvHdr.Load(hdr);
                InvDet.Load(det);
                InvHdr.IsNew = true;
                for (int i = 0; i < InvDet.Count; i++)
                {
                    InvDet[i].IsNew = true;
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return false;
            }
        }

        public bool LoadFromDataTableWithoutSetIsNew(DataTable hdr, DataTable det)
        {
            try
            {
                InvHdr.Load(hdr);
                InvDet.Load(det);
                
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return false;
            }
        }
    }
}
