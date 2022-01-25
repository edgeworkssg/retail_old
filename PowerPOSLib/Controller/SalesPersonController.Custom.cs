using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data;
using SubSonic;

namespace PowerPOS
{
    public partial class SalesPersonController
    {
        public static bool UpdateSalesPersonForTransaction(string OrderHdrRefNo, string SalesPersonID)
        {
            //throw new Exception("This method is not yet implemented");
            try
            {                
                
                SalesCommissionRecord sr = new SalesCommissionRecord(SalesCommissionRecord.Columns.OrderHdrID, new OrderHdr("OrderRefNo",OrderHdrRefNo).OrderHdrID);
                if (!sr.IsLoaded)
                    return false;
                if (sr.OrderHdr.OrderDate.Month < DateTime.Now.Month | sr.OrderHdr.OrderDate.Year < DateTime.Now.Year)
                    return false;

                Where whr = new Where();
                whr.TableName = "SalesPerson";
                whr.ColumnName = SalesPerson.Columns.SalesPersonID;
                whr.Comparison = Comparison.Equals;
                whr.ParameterName = "@SalesPersonID";
                whr.ParameterValue = SalesPersonID;
                int count = new Query("SalesPerson").GetCount(SalesPerson.Columns.SalesPersonID, whr);
                if (count != 1)
                    return false;

                sr.SalesPersonID = SalesPersonID;
                sr.Save(Container.UserInfo.username);
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return false;
            }
        }
        public static ArrayList FetchSalesPersonNames()
        {
            Query qry = new Query("SalesPerson");
            qry.SelectList = "SalesPersonName";
            qry.QueryType = QueryType.Select;
            IDataReader rdr = qry.ExecuteReader();
            ArrayList ar = new ArrayList();
            while (rdr.Read())
            {
                ar.Add(rdr.GetValue(0).ToString());
            }
            rdr.Close();
            return ar;
        }

        public static ArrayList FetchSalesGroupNames()
        {
            Query qry = new Query("SalesGroup");
            qry.SelectList = "GroupName";
            qry.QueryType = QueryType.Select;
            IDataReader rdr = qry.ExecuteReader();
            ArrayList ar = new ArrayList();
            ar.Add("");
            while (rdr.Read())
            {
                ar.Add(rdr.GetValue(0).ToString());
            }
            rdr.Close();
            return ar;
        }

        public static bool InsertLastMonthCommissionToHistory()
        {
            //For every salesman 
            ArrayList salespersonnames = FetchSalesPersonNames();
            Query qr;
 
            
            DataSet ds;
            DateTime firstDayLastMonth;
            if (DateTime.Today.Month > 1)
            {
                firstDayLastMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month - 1, 1); 
            }
            else
            {
                firstDayLastMonth = new DateTime(DateTime.Today.Year - 1, 12, 1); 
            }

            for (int i = 0; i < salespersonnames.Count; i++)
            {
                //Check if entry current salesman existed for 01-MM-YYYY                                
                ds = null;
                qr = new Query("SalesCommissionHistory");
                ds = qr.WHERE("MONTHYEAR",firstDayLastMonth).AND(SalesCommissionHistory.Columns.SalesPersonName, salespersonnames[i]).ExecuteDataSet();
                
                //if no do insert 
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {                    
                    ViewSalesPersonLastMonthCommissionCollection comm 
                        = new ViewSalesPersonLastMonthCommissionCollection();
                    comm.Where(ViewSalesPersonLastMonthCommission.Columns.SalesPersonName, salespersonnames[i].ToString());
                    comm.Load();

                    
                        for (int j = 0; j < comm.Count; j++)
                        {
                        
                           SalesCommissionHistory.Insert
                                (firstDayLastMonth, salespersonnames[i].ToString(),
                                (int)comm[j].ExpectedWorkingHours, comm[j].GroupName,
                                (decimal)comm[j].SalesAmount, comm[j].Rate,
                                (decimal)comm[j].CommissionAmount, 
                                DateTime.Now, "SYSTEM", DateTime.Now, "SYSTEM"
                                , "", "", "", "", "", "", "", "", "", "", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
                        }
                    
                }

                //else dont do anything... historical data is untouchables
            }
            return true;
        }
    }
}
