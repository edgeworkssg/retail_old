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
    public partial class CommissionBasedOnPercentageController
    {
        public int CheckIfDataExists(int SalesGroupID, string CommissionType)
        {
            string strSql = "select count(*) as DataCount ";
            strSql += "from CommissionBasedOnPercentage a ";
            strSql += "where a.SalesGroupID = " + SalesGroupID + " ";
            strSql += "and a.CommissionType = '" + CommissionType + "' ";

            DataSet ds = DataService.GetDataSet(new QueryCommand(strSql, "PowerPOS"));
            //ds.Tables[0].Merge(ds.Tables[1]);
            return Int32.Parse(ds.Tables[0].Rows[0]["DataCount"].ToString());
        }

        public int CheckIfDataExists(int SalesGroupID, string CommissionType, decimal LowerLimit, decimal UpperLimit)
        {
            string strSql = "select count(*) as DataCount ";
            strSql += "from CommissionBasedOnPercentage a ";
            strSql += "where a.SalesGroupID = " + SalesGroupID + " ";
            strSql += "and a.CommissionType = '" + CommissionType + "' ";
            strSql += "and ((a.LowerLimit between " + LowerLimit.ToString() + " and " + UpperLimit.ToString() + ") or (a.UpperLimit between " + LowerLimit.ToString() + " and " + UpperLimit.ToString() + "))";

            DataSet ds = DataService.GetDataSet(new QueryCommand(strSql, "PowerPOS"));
            //ds.Tables[0].Merge(ds.Tables[1]);
            return Int32.Parse(ds.Tables[0].Rows[0]["DataCount"].ToString());
        }

        public int CheckIfDataExists(int SalesGroupID, string CommissionType, decimal LowerLimit, decimal UpperLimit, int UniqueID)
        {
            string strSql = "select count(*) as DataCount ";
            strSql += "from CommissionBasedOnPercentage a ";
            strSql += "where a.SalesGroupID = " + SalesGroupID + " ";
            strSql += "and a.CommissionType = '" + CommissionType + "' ";
            strSql += "and ((a.LowerLimit between " + LowerLimit.ToString() + " and " + UpperLimit.ToString() + ") or (a.UpperLimit between " + LowerLimit.ToString() + " and " + UpperLimit.ToString() + ")) ";
            strSql += "and UniqueID != " + UniqueID +"";;

            DataSet ds = DataService.GetDataSet(new QueryCommand(strSql, "PowerPOS"));
            //ds.Tables[0].Merge(ds.Tables[1]);
            return Int32.Parse(ds.Tables[0].Rows[0]["DataCount"].ToString());
        }

        public DataTable FetchCommissionBaseOnPercentageList()
        {
            string strSql = "select a.UniqueID ";
            strSql += ", a.SalesGroupID ";
            strSql += ", b.GroupName ";
            strSql += ", a.CommissionType ";
            strSql += ", LowerLimit = isnull(a.LowerLimit, 0) ";
            strSql += ", UpperLimit = isnull(a.UpperLimit, 0) ";
            strSql += ", a.PercentCommission ";
            strSql += "from CommissionBasedOnPercentage a ";
            strSql += "inner join UserGroup b ";
            strSql += "on b.GroupId = a.SalesGroupID ";

            DataSet ds = DataService.GetDataSet(new QueryCommand(strSql, "PowerPOS"));
            //ds.Tables[0].Merge(ds.Tables[1]);
            return ds.Tables[0];
        }

        public JsonResult Save(int SalesGroupID, string CommissionType, decimal? LowerLimit, decimal? UpperLimit, decimal? PercentCommission, string ActiveUser)
        {
            var result = new JsonResult();
            result.Status = false;
            result.Message = "";
            result.Details = "";
            result.Data = "";

            try
            {
                var commission = new CommissionBasedOnPercentage();
                commission.IsNew = true;
                commission.SalesGroupID = SalesGroupID;
                commission.CommissionType = CommissionType;
                commission.UpperLimit = UpperLimit;
                commission.LowerLimit = LowerLimit;
                commission.PercentCommission = PercentCommission.Value;
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

        public JsonResult Update(int UniqueID, int SalesGroupID, string CommissionType, decimal? LowerLimit, decimal? UpperLimit, decimal? PercentCommission, string ActiveUser)
        {
            var result = new JsonResult();
            result.Status = false;
            result.Message = "";
            result.Details = "";
            result.Data = "";

            try
            {
                var commission = new CommissionBasedOnPercentage(UniqueID);
                commission.IsNew = false;
                commission.SalesGroupID = SalesGroupID;
                commission.CommissionType = CommissionType;
                commission.UpperLimit = UpperLimit;
                commission.LowerLimit = LowerLimit;
                commission.PercentCommission = PercentCommission.Value;
                //commission.CreatedBy = ActiveUser;
                //commission.ModifiedBy = ActiveUser;
                //commission.CreatedOn = DateTime.Now;
                //commission.ModifiedOn = DateTime.Now;

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
                var commission = new PowerPOS.CommissionBasedOnPercentageController();
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
