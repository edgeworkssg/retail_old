using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SubSonic;
using PowerPOSLib.Container;
using PowerPOS;

namespace PowerPOSLib.Controller.Commission
{
    [System.ComponentModel.DataObject]
    public partial class CommissionBasedOnQtyController
    {
        //public int CheckIfDataExists(int SalesGroupID, string ItemNo)
        //{
        //    string strSql = "select count(*) as ItemAmount ";
        //    strSql += "from CommissionBasedOnQty a ";
        //    strSql += "where a.ItemNo = '" + ItemNo + "' ";
        //    strSql += "and a.SalesGroupID = " + SalesGroupID + " ";

        //    DataSet ds = DataService.GetDataSet(new QueryCommand(strSql, "PowerPOS"));
        //    //ds.Tables[0].Merge(ds.Tables[1]);
        //    return  Int32.Parse(ds.Tables[0].Rows[0]["ItemAmount"].ToString());
        //}

        public int CheckIfDataExists(int SalesGroupID, string ItemNo, string CommissionType)
        {
            string strSql = "select count(*) as ItemAmount ";
            strSql += "from CommissionBasedOnQty a ";
            strSql += "where a.ItemNo = '" + ItemNo + "' ";
            strSql += "and a.SalesGroupID = " + SalesGroupID + " ";
            strSql += "and a.CommissionType = '" + CommissionType + "' ";

            DataSet ds = DataService.GetDataSet(new QueryCommand(strSql, "PowerPOS"));
            //ds.Tables[0].Merge(ds.Tables[1]);
            return Int32.Parse(ds.Tables[0].Rows[0]["ItemAmount"].ToString());
        }

        public DataTable FetchCommissionBasedOnQtyList()
        {
            string strSql = "select a.UniqueID ";
            strSql += ", a.SalesGroupID ";
            strSql += ", c.GroupName ";
            strSql += ", a.CommissionType ";
            strSql += ", ItemCategory = b.CategoryName ";
            strSql += ", a.ItemNo ";
            strSql += ", b.ItemName ";
            strSql += ", a.Quantity ";
            strSql += ", a.AmountCommission ";
            strSql += "from CommissionBasedOnQty a ";
            strSql += "inner join Item b ";
            strSql += "on b.ItemNo = a.ItemNo ";
            strSql += "inner join UserGroup c ";
            strSql += "on c.GroupId = a.SalesGroupID";

            DataSet ds = DataService.GetDataSet(new QueryCommand(strSql, "PowerPOS"));
            //ds.Tables[0].Merge(ds.Tables[1]);
            return ds.Tables[0];
        }

        public JsonResult Save(int SalesGroupID, string ItemNo, int Quantity, decimal? AmountCommission , string ActiveUser, string CommissionType)
        {
            var result = new JsonResult();
            result.Status = false;
            result.Message = "";
            result.Details = "";
            result.Data = null;

            try
            {
                var commission = new CommissionBasedOnQty();
                commission.IsNew = true;
                commission.SalesGroupID = SalesGroupID;
                commission.CommissionType = CommissionType;
                commission.ItemNo = ItemNo;
                commission.Quantity = Quantity;
                commission.AmountCommission = AmountCommission.Value;
                commission.CreatedBy = ActiveUser;
                commission.ModifiedBy = ActiveUser;
                commission.CreatedOn = DateTime.Now;
                commission.ModifiedOn = DateTime.Now;

                commission.Save(ActiveUser);
                result.Status = true;
                result.Message = "Comission saved.";
            }
            catch (Exception)
            {
                result.Status = false;
                result.Message = "Cannot save commission.";
            }

            return result;
        }

        public JsonResult Update(int UniqueID, int SalesGroupID, string ItemNo, int Quantity, decimal? AmountCommission, string ActiveUser, string CommissionType)
        {
            var result = new JsonResult();
            result.Status = false;
            result.Message = "";
            result.Details = "";
            result.Data = null;

            try
            {
                var commission = new CommissionBasedOnQty(UniqueID);
                commission.IsNew = false;
                commission.SalesGroupID = SalesGroupID;
                commission.ItemNo = ItemNo;
                commission.Quantity = Quantity;
                commission.AmountCommission = AmountCommission.Value;
                commission.CommissionType = CommissionType;

                commission.Save(ActiveUser);
                result.Status = true;
                result.Message = "Comission saved.";
            }
            catch (Exception)
            {
                result.Status = false;
                result.Message = "Cannot save commission.";
            }

            return result;
        }

        public JsonResult Delete(int UniqueID)
        {
            var result = new JsonResult();
            result.Status = false;
            result.Message = "";
            result.Details = "";
            result.Data = null;

            try
            {
                var commission = new PowerPOS.CommissionBasedOnQtyController();
                commission.Delete(UniqueID);
                result.Status = true;
                result.Message = "Comission deleted.";
            }
            catch (Exception)
            {
                result.Status = false;
                result.Message = "Cannot delete commission.";
            }

            return result;
        }
    }
}
