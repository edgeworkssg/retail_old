using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.ComponentModel;
using SubSonic;
using System;
using System.Collections;
using System.Data;
namespace PowerPOS
{
    public partial class CashRecordingController
    {
        public static decimal GetTotalFloatAmount(int PointOfSaleID)
        {
            var UseOpeningBalance = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.CashRecording.UseOpeningBalance), false);
            string DefaultOpeningBalance = (AppSetting.GetSetting(AppSetting.SettingsName.CashRecording.DefaultOpeningBalance)+"").Trim();
            decimal DecOpeningBalance = 0;
            if(!String.IsNullOrEmpty(DefaultOpeningBalance))
                DecOpeningBalance = Convert.ToDecimal(DefaultOpeningBalance);
            decimal amount = 0;

            if (UseOpeningBalance)
            {
                amount += DecOpeningBalance;
            }
            else 
            {
                //Query qr = new Query("CashRecording");
                //qr.OrderBy = OrderBy.Desc("CashRecordingTime");
                //qr.QueryType = QueryType.Select;
                //qr.SelectList = "amount";
                //qr.AddWhere("CashRecordingTime", Comparison.LessOrEquals, DateTime.Now);
                //qr.AddWhere("CashRecordingTime", Comparison.GreaterOrEquals, ClosingController.FetchLastClosingTime(PointOfSaleID));
                //qr.AddWhere("PointOfSaleID", PointOfSaleID);
                //qr.AddWhere("CashRecordingTypeID", 3);
                //DataSet ds = qr.ExecuteDataSet();
                //if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                //{
                //    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                //    {
                //        amount += decimal.Parse(ds.Tables[0].Rows[i]["amount"].ToString());
                //    }
                //}

                DataSet ds = PowerPOS.SPs.GetTotalFloatAmount(PointOfSaleID).GetDataSet();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {

                    amount += decimal.Parse(ds.Tables[0].Rows[0][0].ToString());

                }
            }
            
            return amount;
        }
        public static ArrayList FetchCashRecordingTypes()
        {
            ArrayList ar = new ArrayList();
            ar.Add("");
            CashRecordingTypeCollection crCol = new CashRecordingTypeCollection();
            ar.AddRange(crCol.Load());
            return ar;
        }
        public void fetchCashRecordingSummary(DateTime startDate, DateTime endDate, int PointOfSaleId, out decimal cashIn, out decimal cashOut, out decimal openingBalance)
        {            
            //Get the sum of cash recording 
            CashRecordingCollection CashRecordings = new CashRecordingCollection();            
            CashRecordings.BetweenAnd("CashRecordingTime", startDate, endDate);
            CashRecordings.Where(CashRecording.Columns.PointOfSaleID,PointOfSaleId);
            CashRecordings.OrderByAsc(CashRecording.Columns.CashRecordingTypeId);
            CashRecordings.Load();
            cashIn = 0; cashOut = 0; openingBalance = 0;
            for (int i = 0; i < CashRecordings.Count; i++)
            {
                switch (CashRecordings[i].CashRecordingTypeId)
                {
                    case 1: //Cash In
                        cashIn += CashRecordings[i].Amount ;
                        break;
                    case 2: //Cash Out
                        cashOut += CashRecordings[i].Amount;
                        break;
                    case 3: //Opening Balance
                        openingBalance += CashRecordings[i].Amount;
                        break;
                    default:
                        break;
                }                                                
            }
                
        }
    }
}
