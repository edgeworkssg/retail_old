using System;
using System.Data;
using System.Transactions;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using PowerPOS;
using SubSonic;
using System.Collections;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using PowerWeb.Synchronization.Classes;
using System.Text;
using Newtonsoft.Json;
using System.Threading;
using PowerPOS.Container;
using System.Reflection;
using System.Web.Script.Serialization;
using System.Web.Hosting;
using System.Drawing;
using POSDevices;
/// <summary>
/// Summary description for SynchronizationController
/// </summary>
public partial class SynchronizationController
{
    static string providerName = "PowerPOS";

    public SynchronizationController()
    {

    }

    #region "INCOMING - Get Data from Client to master"

    #region "Orders"
    public static string[][] GetOrderHdrList(DateTime startDate, DateTime endDate, int PointOfSaleID)
    {
        Query qr = OrderHdr.CreateQuery();
        qr.QueryType = QueryType.Select;
        qr.SelectList = "OrderHdrId, UniqueId";
        qr.AddWhere(OrderHdr.Columns.OrderDate, Comparison.GreaterOrEquals, startDate);
        qr.AddWhere(OrderHdr.Columns.OrderDate, Comparison.LessOrEquals, endDate);
        qr.AddWhere(OrderHdr.Columns.PointOfSaleID, PointOfSaleID);

        DataSet ds = qr.ExecuteDataSet();

        //convert ds to array of string from the GUID
        string[][] objHdr = new string[ds.Tables[0].Rows.Count][];

        for (int op = 0; op < ds.Tables[0].Rows.Count; op++)
        {

            string[] objArr = new string[2];
            objArr[0] = ds.Tables[0].Rows[op][0].ToString();
            objArr[1] = ds.Tables[0].Rows[op][1].ToString();
            objHdr[op] = objArr;
        }

        return objHdr;
    }
    public static string[][] GetOrderHdrListWithoutPOSID(DateTime startDate, DateTime endDate)
    {
        Query qr = OrderHdr.CreateQuery();
        qr.QueryType = QueryType.Select;
        qr.SelectList = "OrderHdrId, UniqueId";
        qr.AddWhere(OrderHdr.Columns.OrderDate, Comparison.GreaterOrEquals, startDate);
        qr.AddWhere(OrderHdr.Columns.OrderDate, Comparison.LessOrEquals, endDate);

        DataSet ds = qr.ExecuteDataSet();

        //convert ds to array of string from the GUID
        string[][] objHdr = new string[ds.Tables[0].Rows.Count][];

        for (int op = 0; op < ds.Tables[0].Rows.Count; op++)
        {

            string[] objArr = new string[2];
            objArr[0] = ds.Tables[0].Rows[op][0].ToString();
            objArr[1] = ds.Tables[0].Rows[op][1].ToString();
            objHdr[op] = objArr;
        }

        return objHdr;
    }

    public static string[][] GetInventoryHdrList(DateTime startDate, DateTime endDate)
    {
        Query qr = InventoryHdr.CreateQuery();
        qr.QueryType = QueryType.Select;
        qr.SelectList = "InventoryHdrRefNo, UniqueId";
        qr.AddWhere(InventoryHdr.Columns.InventoryDate, Comparison.GreaterOrEquals, startDate);
        qr.AddWhere(InventoryHdr.Columns.InventoryDate, Comparison.LessOrEquals, endDate);

        DataSet ds = qr.ExecuteDataSet();

        //convert ds to array of string from the GUID
        string[][] objHdr = new string[ds.Tables[0].Rows.Count][];

        for (int op = 0; op < ds.Tables[0].Rows.Count; op++)
        {

            string[] objArr = new string[2];
            objArr[0] = ds.Tables[0].Rows[op][0].ToString();
            objArr[1] = ds.Tables[0].Rows[op][1].ToString();
            objHdr[op] = objArr;
        }

        return objHdr;
    }

    public static DataSet FetchSales(DateTime startDate, DateTime endDate)
    {

        String SQLString = "Select * from OrderHdr where OrderDate Between '" +
            startDate.ToString("yyyy-MM-dd HH:mm:ss") + "' AND '" +
            endDate.ToString("yyyy-MM-dd HH:mm:ss") + "'";
        DataSet ds = new DataSet();
        DataTable dtOrderHdr = DataService.GetDataSet(new QueryCommand(SQLString)).Tables[0];
        ds.Tables.Add(dtOrderHdr);

        SQLString = "Select * from OrderDet where OrderHdrID In ( Select OrderHdrID from OrderHdr where OrderDate Between '" +
            startDate.ToString("yyyy-MM-dd HH:mm:ss") + "' AND '" +
            endDate.ToString("yyyy-MM-dd HH:mm:ss") + "')";
        DataTable dtOrderDet = DataService.GetDataSet(new QueryCommand(SQLString)).Tables[0];
        ds.Tables.Add(dtOrderDet);

        //convert ds to array of string from the GUID
        /*string[][] objHdr = new string[ds.Tables[0].Rows.Count][];

        for (int op = 0; op < ds.Tables[0].Rows.Count; op++)
        {

            string[] objArr = new string[2];
            objArr[0] = ds.Tables[0].Rows[op][0].ToString();
            objArr[1] = ds.Tables[0].Rows[op][1].ToString();
            objHdr[op] = objArr;
        }*/

        return ds;
    }

    public static DataSet FetchSales(DateTime startDate, DateTime endDate, String Membershipno)
    {
        String SQLString = "Select * from OrderHdr where OrderDate Between '" +
            startDate.ToString("yyyy-MM-dd HH:mm:ss") + "' AND '" +
            endDate.ToString("yyyy-MM-dd HH:mm:ss") + "' AND Membershipno Like '" + Membershipno + "'";
        DataSet ds = new DataSet();
        DataTable dtOrderHdr = DataService.GetDataSet(new QueryCommand(SQLString)).Tables[0];
        ds.Tables.Add(dtOrderHdr);

        SQLString = "Select * from OrderDet where OrderHdrID In ( Select OrderHdrID from OrderHdr where OrderDate Between '" +
           startDate.ToString("yyyy-MM-dd HH:mm:ss") + "' AND '" +
           endDate.ToString("yyyy-MM-dd HH:mm:ss") + "' AND Membershipno Like '" + Membershipno + "')";
        DataTable dtOrderDet = DataService.GetDataSet(new QueryCommand(SQLString)).Tables[0];
        ds.Tables.Add(dtOrderDet);

        //convert ds to array of string from the GUID
        /*string[][] objHdr = new string[ds.Tables[0].Rows.Count][];

        for (int op = 0; op < ds.Tables[0].Rows.Count; op++)
        {

            string[] objArr = new string[2];
            objArr[0] = ds.Tables[0].Rows[op][0].ToString();
            objArr[1] = ds.Tables[0].Rows[op][1].ToString();
            objHdr[op] = objArr;
        }*/

        return ds;
    }

    private static QueryCommandCollection FetchInventoryHdr(string[][] objHdr)
    {
        try
        {
            Query qry;
            Object tmpGuid;


            ArrayList duplicate = new ArrayList();

            //ORDER HEADER
            //Logger.writeLog("#records>" + objHdr.Length);

            InventoryHdr ord = new InventoryHdr();

            QueryCommand qr = ord.GetInsertCommand("SYNC");
            int UniqueIDIndex;
            UniqueIDIndex = -1;
            for (int f = 0; f < qr.Parameters.Count; f++)
            {
                if (qr.Parameters[f].ParameterName.ToLower() == "@uniqueid")
                {
                    UniqueIDIndex = f;
                    break;
                }
            }
            if (UniqueIDIndex == -1)
            {
                Logger.writeLog("Sync table does not contain unique ID or unique ID name does not compy.");
                throw new Exception("Sync table does not contain unique ID or unique ID name does not compy.");
            }

            QueryCommandCollection col = new QueryCommandCollection();
            ArrayList allList = new ArrayList();
            for (int i = 0; i < objHdr.Length; i++)
            {
                allList.Add(objHdr[i][0]);
            }

            //Check GUID uniqueness in Target DB
            qry = new Query("InventoryHdr");
            qry.SelectList = "InventoryHdrRefNo,UniqueID";
            DataTable dtUniqueID = qry.IN(InventoryHdr.Columns.InventoryHdrRefNo, allList).ExecuteDataSet().Tables[0];

            for (int i = 0; i < objHdr.Length; i++)
            {
                if (dtUniqueID.Rows.Count > 0)
                {
                    DataRow[] dr = dtUniqueID.Select("InventoryHdrRefNo = '" + objHdr[i][0] + "'");

                    if (dr.Length > 0)
                        tmpGuid = new Guid(dr[0]["UniqueID"].ToString());
                    else
                        tmpGuid = new Object();
                }
                else tmpGuid = new Object();

                if (dtUniqueID.Rows.Count > 0 && tmpGuid is Guid) //GUID Found?
                {
                    if ((Guid)tmpGuid != new Guid(objHdr[i][UniqueIDIndex]))
                    {
                        //same order but different unique ID - Houston we have a problem                        
                        duplicate.Add(qr.Parameters[0].ParameterValue.ToString());
                    }
                }
                else
                {
                    //New Item. Perform Create
                    qr = ord.GetInsertCommand("SYNC");
                    for (int oa = 0; oa < qr.Parameters.Count; oa++)
                    {
                        if (qr.Parameters[oa].ParameterName.ToLower() != "@createdon" &
                                   qr.Parameters[oa].ParameterName.ToLower() != "@createdby" &
                                    qr.Parameters[oa].ParameterName.ToLower() != "@modifiedon" &
                                    qr.Parameters[oa].ParameterName.ToLower() != "@modifiedby")
                        {
                            //Logger.writeLog(qr.Parameters[oa].ParameterName + " val: " + qr.Parameters[oa].ParameterValue.ToString());
                            switch (qr.Parameters[oa].DataType)
                            {
                                case DbType.Boolean:
                                    if (objHdr[i][oa] == "0")
                                        qr.Parameters[oa].ParameterValue = false;
                                    else if (objHdr[i][oa] == "1")
                                        qr.Parameters[oa].ParameterValue = true;
                                    break;
                                case DbType.Date:
                                case DbType.DateTime:
                                    if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = DateTime.Parse(objHdr[i][oa]);
                                    break;
                                case DbType.Decimal:
                                case DbType.Currency:
                                    if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Decimal.Parse(objHdr[i][oa]);
                                    break;
                                case DbType.Double:
                                    if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Double.Parse(objHdr[i][oa]);
                                    break;
                                case DbType.Single:
                                    if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Single.Parse(objHdr[i][oa]);
                                    break;
                                case DbType.Guid:
                                    if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = new Guid(objHdr[i][oa]);
                                    break;
                                case DbType.Int16:
                                    if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int16.Parse(objHdr[i][oa]);
                                    break;
                                case DbType.Int32:
                                    if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int32.Parse(objHdr[i][oa]);
                                    break;
                                case DbType.Int64:
                                    if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int64.Parse(objHdr[i][oa]);
                                    break;
                                case DbType.UInt16:
                                    if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt16.Parse(objHdr[i][oa]);
                                    break;
                                case DbType.UInt32:
                                    if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt32.Parse(objHdr[i][oa]);
                                    break;
                                case DbType.UInt64:
                                    if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt64.Parse(objHdr[i][oa]);
                                    break;
                                case DbType.StringFixedLength:
                                case DbType.String:
                                    qr.Parameters[oa].ParameterValue = objHdr[i][oa];
                                    break;
                                default:
                                    qr.Parameters[oa].ParameterValue = objHdr[i][oa];
                                    break;
                            }
                        }
                    }
                    col.Add(qr);
                }
            }
            if (duplicate.Count != 0)
            {
                string str;
                str = "Error: Attempting to enter duplicate InventoryHdrRefNoRef Number.";
                str = str + "\r\n" + "Duplicates:" + "\r\n";
                for (int i = 0; i < duplicate.Count; i++)
                    str += duplicate[i] + "\r\n";

                //Logger.writeLog(str);
                throw new Exception("Duplicate found");
            }

            return col;

        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
            throw ex;
        }
    }
    private static QueryCommandCollection FetchInventoryDet(string[][] objDet)
    {
        try
        {
            Query qry;
            Object tmpGuid;


            ArrayList duplicate = new ArrayList();

            //ORDER HEADER
            //Logger.writeLog("#records>" + objDet.Length);

            InventoryDet ord = new InventoryDet();

            QueryCommand qr = ord.GetInsertCommand("SYNC");
            int UniqueIDIndex, RQTY_COL = -1;
            UniqueIDIndex = -1;
            for (int f = 0; f < qr.Parameters.Count; f++)
            {
                if (qr.Parameters[f].ParameterName.ToLower() == "@uniqueid")
                {
                    UniqueIDIndex = f;
                }
                /*

            else if (qr.Parameters[f].ParameterName.ToLower() == "@remainingQty")               
            {
                RQTY_COL = f;
            }
                 */
            }
            /*
            if (RQTY_COL == -1)
            {
                Logger.writeLog("Sync table does not contain remainingQty or remainingQty name does not compy.");
                throw new Exception("Sync table does not contain remainingQty or remainingQty name does not compy.");
            }
            */
            if (UniqueIDIndex == -1)
            {
                Logger.writeLog("Sync table does not contain unique ID or unique ID name does not compy.");
                throw new Exception("Sync table does not contain unique ID or unique ID name does not compy.");
            }

            QueryCommandCollection col = new QueryCommandCollection();
            ArrayList allList = new ArrayList();
            for (int i = 0; i < objDet.Length; i++)
            {
                allList.Add(objDet[i][0]);
            }

            //Check GUID uniqueness in Target DB
            qry = new Query("InventoryDet");
            qry.SelectList = "InventoryDetRefNo,UniqueID";
            DataTable dtUniqueID = qry.IN(InventoryDet.Columns.InventoryDetRefNo, allList).ExecuteDataSet().Tables[0];

            for (int i = 0; i < objDet.Length; i++)
            {
                if (dtUniqueID.Rows.Count > 0)
                {
                    DataRow[] dr = dtUniqueID.Select("InventoryDetRefNo = '" + objDet[i][0] + "'");

                    if (dr.Length > 0)
                        tmpGuid = new Guid(dr[0]["UniqueID"].ToString());
                    else
                        tmpGuid = new Object();
                }
                else tmpGuid = new Object();

                if (dtUniqueID.Rows.Count > 0 && tmpGuid is Guid) //GUID Found?
                {
                    if ((Guid)tmpGuid != new Guid(objDet[i][UniqueIDIndex]))
                    {
                        //same order but different unique ID - Houston we have a problem                        
                        duplicate.Add(qr.Parameters[0].ParameterValue.ToString());
                    }
                    else
                    {
                        /*
                        int trQty = 0;
                        //UPDATE THE REMAINING QTY ONLY
                        if (int.TryParse(objDet[i][RQTY_COL].Trim(), out trQty))
                        {
                            string mySQL = "Update InventoryDet Set remainingQty = " + objDet[i][RQTY_COL].Trim() + " where inventorydetrefno = '" + objDet[i][0] + "'";
                            col.Add(new QueryCommand(mySQL, "PowerPOS"));
                        }*/
                    }
                }
                else
                {
                    //New Item. Perform Create
                    qr = ord.GetInsertCommand("SYNC");
                    for (int oa = 0; oa < qr.Parameters.Count; oa++)
                    {
                        if (qr.Parameters[oa].ParameterName.ToLower() != "@createdon" &
                                   qr.Parameters[oa].ParameterName.ToLower() != "@createdby" &
                                    qr.Parameters[oa].ParameterName.ToLower() != "@modifiedon" &
                                    qr.Parameters[oa].ParameterName.ToLower() != "@modifiedby")
                        {
                            //Logger.writeLog(qr.Parameters[oa].ParameterName + " val: " + qr.Parameters[oa].ParameterValue.ToString());
                            switch (qr.Parameters[oa].DataType)
                            {
                                case DbType.Boolean:
                                    if (objDet[i][oa] == "0")
                                        qr.Parameters[oa].ParameterValue = false;
                                    else if (objDet[i][oa] == "1")
                                        qr.Parameters[oa].ParameterValue = true;
                                    break;
                                case DbType.Date:
                                case DbType.DateTime:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = DateTime.Parse(objDet[i][oa]);
                                    break;
                                case DbType.Decimal:
                                case DbType.Currency:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Decimal.Parse(objDet[i][oa]);
                                    break;
                                case DbType.Double:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Double.Parse(objDet[i][oa]);
                                    break;
                                case DbType.Single:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Single.Parse(objDet[i][oa]);
                                    break;
                                case DbType.Guid:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = new Guid(objDet[i][oa]);
                                    break;
                                case DbType.Int16:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int16.Parse(objDet[i][oa]);
                                    break;
                                case DbType.Int32:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int32.Parse(objDet[i][oa]);
                                    break;
                                case DbType.Int64:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int64.Parse(objDet[i][oa]);
                                    break;
                                case DbType.UInt16:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt16.Parse(objDet[i][oa]);
                                    break;
                                case DbType.UInt32:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt32.Parse(objDet[i][oa]);
                                    break;
                                case DbType.UInt64:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt64.Parse(objDet[i][oa]);
                                    break;
                                case DbType.StringFixedLength:
                                case DbType.String:
                                    qr.Parameters[oa].ParameterValue = objDet[i][oa];
                                    break;
                                default:
                                    qr.Parameters[oa].ParameterValue = objDet[i][oa];
                                    break;
                            }
                        }
                    }
                    col.Add(qr);
                }
            }
            if (duplicate.Count != 0)
            {
                string str;
                str = "Error: Attempting to enter duplicate InventoryDetRefNoRef Number.";
                str = str + "\r\n" + "Duplicates:" + "\r\n";
                for (int i = 0; i < duplicate.Count; i++)
                    str += duplicate[i] + "\r\n";

                Logger.writeLog(str);
                throw new Exception("Duplicate found");
            }

            return col;

        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
            throw ex;
        }
    }
    private static QueryCommandCollection FetchOrderHdr(string[][] objHdr)
    {
        try
        {
            Query qry;
            Object tmpGuid;

            ArrayList duplicate = new ArrayList();

            OrderHdr ord = new OrderHdr();

            QueryCommand qr = ord.GetInsertCommand("SYNC");
            int UniqueIDIndex;
            UniqueIDIndex = -1;
            for (int f = 0; f < qr.Parameters.Count; f++)
            {
                if (qr.Parameters[f].ParameterName.ToLower() == "@uniqueid")
                {
                    UniqueIDIndex = f;
                    break;
                }
            }
            if (UniqueIDIndex == -1)
            {
                Logger.writeLog("Sync table does not contain unique ID or unique ID name does not compy.");
                throw new Exception("Sync table does not contain unique ID or unique ID name does not compy.");
            }

            QueryCommandCollection col = new QueryCommandCollection();
            ArrayList allList = new ArrayList();
            for (int i = 0; i < objHdr.Length; i++)
            {
                allList.Add(objHdr[i][0]);
            }

            //Check GUID uniqueness in Target DB
            qry = new Query("OrderHdr");
            qry.SelectList = "OrderHdrID,UniqueID";
            DataTable dtUniqueID = qry.IN(OrderHdr.Columns.OrderHdrID, allList).ExecuteDataSet().Tables[0];

            for (int i = 0; i < objHdr.Length; i++)
            {
                if (dtUniqueID.Rows.Count > 0)
                {
                    DataRow[] dr = dtUniqueID.Select("OrderHdrID = '" + objHdr[i][0] + "'");

                    if (dr.Length > 0)
                        tmpGuid = new Guid(dr[0]["UniqueID"].ToString());
                    else
                        tmpGuid = new Object();
                }
                else tmpGuid = new Object();

                if (dtUniqueID.Rows.Count > 0 && tmpGuid is Guid) //GUID Found?
                {
                    qr = ord.GetInsertCommand("SYNC");
                    Logger.writeLog(string.Format("Sync Update OrderHdr : {0}", objHdr[i].AsSingleLineString(",")));
                    for (int oa = 0; oa < qr.Parameters.Count; oa++)
                    {
                        if (oa >= objHdr[i].Length) continue;
                        switch (qr.Parameters[oa].DataType)
                        {
                            case DbType.Boolean:
                                if (objHdr[i][oa] == "0")
                                    qr.Parameters[oa].ParameterValue = false;
                                else if (objHdr[i][oa] == "1")
                                    qr.Parameters[oa].ParameterValue = true;
                                break;
                            case DbType.Date:
                            case DbType.DateTime:
                                if (!string.IsNullOrEmpty(objHdr[i][oa]))
                                {
                                    qr.Parameters[oa].ParameterValue = DateTime.Parse(objHdr[i][oa]);
                                    Logger.writeLog(">>>" + objHdr[i][oa]);
                                }
                                break;
                            case DbType.Decimal:
                            case DbType.Currency:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Decimal.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Double:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Double.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Single:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Single.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Guid:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = new Guid(objHdr[i][oa]);
                                break;
                            case DbType.Int16:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int16.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Int32:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int32.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Int64:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int64.Parse(objHdr[i][oa]);
                                break;
                            case DbType.UInt16:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt16.Parse(objHdr[i][oa]);
                                break;
                            case DbType.UInt32:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt32.Parse(objHdr[i][oa]);
                                break;
                            case DbType.UInt64:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt64.Parse(objHdr[i][oa]);
                                break;
                            case DbType.StringFixedLength:
                            case DbType.String:
                                qr.Parameters[oa].ParameterValue = objHdr[i][oa];
                                break;
                            default:
                                qr.Parameters[oa].ParameterValue = objHdr[i][oa];
                                break;
                        }
                    }
                    ord = new OrderHdr(objHdr[i][0]);
                    ord.IsVoided = (bool)qr.Parameters.GetParameter("@IsVoided").ParameterValue;
                    ord.ModifiedBy = (string)qr.Parameters.GetParameter("@ModifiedBy").ParameterValue;
                    ord.ModifiedOn = (DateTime)qr.Parameters.GetParameter("@ModifiedOn").ParameterValue;


                    QueryCommand qrUpdate = ord.GetUpdateCommand("SYNC");
                    if (qrUpdate.CommandSql.IndexOf("[ModifiedBy] = @ModifiedBy, [ModifiedOn] = @ModifiedOn,") > 0)
                    {
                        qrUpdate.CommandSql = qrUpdate.CommandSql.Remove(qrUpdate.CommandSql.IndexOf("[ModifiedBy] = @ModifiedBy, [ModifiedOn] = @ModifiedOn,"), 55);
                        qrUpdate.Parameters.GetParameter("@ModifiedOn").ParameterValue = ord.ModifiedOn;
                    }
                    else
                    {
                        qrUpdate.CommandSql = qrUpdate.CommandSql.Remove(qrUpdate.CommandSql.IndexOf(", [ModifiedOn] = @ModifiedOn"), 28);
                        qrUpdate.Parameters.GetParameter("@ModifiedOn").ParameterValue = ord.ModifiedOn;
                    }

                    col.Add(qrUpdate);
                }
                else
                {
                    //New Item. Perform Create
                    qr = ord.GetInsertCommand("SYNC");
                    Logger.writeLog(string.Format("Sync Insert OrderHdr : {0}", objHdr[i].AsSingleLineString(",")));
                    for (int oa = 0; oa < qr.Parameters.Count; oa++)
                    {
                        if (oa >= objHdr[i].Length) continue;
                        switch (qr.Parameters[oa].DataType)
                        {
                            case DbType.Boolean:
                                if (objHdr[i][oa] == "0")
                                    qr.Parameters[oa].ParameterValue = false;
                                else if (objHdr[i][oa] == "1")
                                    qr.Parameters[oa].ParameterValue = true;
                                break;
                            case DbType.Date:
                            case DbType.DateTime:
                                if (!string.IsNullOrEmpty(objHdr[i][oa]))
                                {
                                    qr.Parameters[oa].ParameterValue = DateTime.Parse(objHdr[i][oa]);
                                    Logger.writeLog(">>>" + objHdr[i][oa]);
                                }
                                break;
                            case DbType.Decimal:
                            case DbType.Currency:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Decimal.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Double:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Double.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Single:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Single.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Guid:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = new Guid(objHdr[i][oa]);
                                break;
                            case DbType.Int16:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int16.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Int32:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int32.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Int64:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int64.Parse(objHdr[i][oa]);
                                break;
                            case DbType.UInt16:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt16.Parse(objHdr[i][oa]);
                                break;
                            case DbType.UInt32:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt32.Parse(objHdr[i][oa]);
                                break;
                            case DbType.UInt64:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt64.Parse(objHdr[i][oa]);
                                break;
                            case DbType.StringFixedLength:
                            case DbType.String:
                                qr.Parameters[oa].ParameterValue = objHdr[i][oa];
                                break;
                            default:
                                qr.Parameters[oa].ParameterValue = objHdr[i][oa];
                                break;
                        }
                    }
                    col.Add(qr);
                }
            }
            if (duplicate.Count != 0)
            {
                string str;
                str = "Error: Attempting to enter duplicate Order Header Ref Number.";
                str = str + "\r\n" + "Duplicates:" + "\r\n";
                for (int i = 0; i < duplicate.Count; i++)
                    str += duplicate[i] + "\r\n";

                Logger.writeLog(str);
                throw new Exception("Duplicate found");
            }

            return col;

        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
            throw ex;
        }
    }

    private static QueryCommandCollection FetchOrderHdr(string[][] objHdr, out string[] OrderHdrList)
    {
        try
        {
            Query qry;
            Object tmpGuid;
            OrderHdrList = new string[objHdr.Length];

            ArrayList duplicate = new ArrayList();

            //ORDER HEADER
            //Logger.writeLog("#records>" + objHdr.Length);

            OrderHdr ord = new OrderHdr();

            QueryCommand qr = ord.GetInsertCommand("SYNC");
            int UniqueIDIndex;
            UniqueIDIndex = -1;
            for (int f = 0; f < qr.Parameters.Count; f++)
            {
                if (qr.Parameters[f].ParameterName.ToLower() == "@uniqueid")
                {
                    UniqueIDIndex = f;
                    break;
                }
            }
            if (UniqueIDIndex == -1)
            {
                Logger.writeLog("Sync table does not contain unique ID or unique ID name does not compy.");
                throw new Exception("Sync table does not contain unique ID or unique ID name does not compy.");
            }

            QueryCommandCollection col = new QueryCommandCollection();
            ArrayList allList = new ArrayList();
            for (int i = 0; i < objHdr.Length; i++)
            {
                allList.Add(objHdr[i][0]);
                OrderHdrList[i] = objHdr[i][0];
            }

            //Check GUID uniqueness in Target DB
            qry = new Query("OrderHdr");
            qry.SelectList = "OrderHdrID,UniqueID";
            DataTable dtUniqueID = qry.IN(OrderHdr.Columns.OrderHdrID, allList).ExecuteDataSet().Tables[0];

            for (int i = 0; i < objHdr.Length; i++)
            {
                if (dtUniqueID.Rows.Count > 0)
                {
                    DataRow[] dr = dtUniqueID.Select("OrderHdrID = '" + objHdr[i][0] + "'");

                    if (dr.Length > 0)
                        tmpGuid = new Guid(dr[0]["UniqueID"].ToString());
                    else
                        tmpGuid = new Object();
                }
                else tmpGuid = new Object();

                if (dtUniqueID.Rows.Count > 0 && tmpGuid is Guid) //GUID Found?
                {
                    qr = ord.GetInsertCommand("SYNC");
                    Logger.writeLog("Update OrderHdrID = " + objHdr[i][0]);
                    for (int oa = 0; oa < qr.Parameters.Count; oa++)
                    {
                        /*if (qr.Parameters[oa].ParameterName.ToLower() != "@createdon" &
                                   qr.Parameters[oa].ParameterName.ToLower() != "@createdby" &
                                    qr.Parameters[oa].ParameterName.ToLower() != "@modifiedon" &
                                    qr.Parameters[oa].ParameterName.ToLower() != "@modifiedby")
                        {*/
                        //Logger.writeLog(qr.Parameters[oa].ParameterName + " val: " + qr.Parameters[oa].ParameterValue.ToString());
                        if (oa >= objHdr[i].Length) continue;
                        switch (qr.Parameters[oa].DataType)
                        {
                            case DbType.Boolean:
                                if (objHdr[i][oa] == "0")
                                    qr.Parameters[oa].ParameterValue = false;
                                else if (objHdr[i][oa] == "1")
                                    qr.Parameters[oa].ParameterValue = true;
                                break;
                            case DbType.Date:
                            case DbType.DateTime:
                                if (!string.IsNullOrEmpty(objHdr[i][oa]))
                                {
                                    qr.Parameters[oa].ParameterValue = DateTime.Parse(objHdr[i][oa]);
                                    Logger.writeLog(">>>" + objHdr[i][oa]);
                                }
                                break;
                            case DbType.Decimal:
                            case DbType.Currency:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Decimal.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Double:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Double.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Single:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Single.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Guid:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = new Guid(objHdr[i][oa]);
                                break;
                            case DbType.Int16:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int16.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Int32:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int32.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Int64:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int64.Parse(objHdr[i][oa]);
                                break;
                            case DbType.UInt16:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt16.Parse(objHdr[i][oa]);
                                break;
                            case DbType.UInt32:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt32.Parse(objHdr[i][oa]);
                                break;
                            case DbType.UInt64:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt64.Parse(objHdr[i][oa]);
                                break;
                            case DbType.StringFixedLength:
                            case DbType.String:
                                qr.Parameters[oa].ParameterValue = objHdr[i][oa];
                                break;
                            default:
                                qr.Parameters[oa].ParameterValue = objHdr[i][oa];
                                break;
                        }
                        //}

                    }
                    ord = new OrderHdr(objHdr[i][0]);
                    ord.IsVoided = (bool)qr.Parameters.GetParameter("@IsVoided").ParameterValue;
                    ord.ModifiedBy = (string)qr.Parameters.GetParameter("@ModifiedBy").ParameterValue;
                    ord.ModifiedOn = (DateTime)qr.Parameters.GetParameter("@ModifiedOn").ParameterValue;
                    ord.Remark = (string)qr.Parameters.GetParameter("@Remark").ParameterValue;
                    ord.MembershipNo = (string)qr.Parameters.GetParameter("@MembershipNo").ParameterValue;

                    QueryCommand qrUpdate = ord.GetUpdateCommand("SYNC");
                    if (qrUpdate.CommandSql.IndexOf("[ModifiedBy] = @ModifiedBy, [ModifiedOn] = @ModifiedOn,") > 0)
                    {
                        qrUpdate.CommandSql = qrUpdate.CommandSql.Remove(qrUpdate.CommandSql.IndexOf("[ModifiedBy] = @ModifiedBy, [ModifiedOn] = @ModifiedOn,"), 55);
                        qrUpdate.Parameters.GetParameter("@ModifiedOn").ParameterValue = ord.ModifiedOn;
                    }
                    else
                    {
                        if (qrUpdate.CommandSql.IndexOf(", [ModifiedOn] = @ModifiedOn") > 0)
                        {
                            qrUpdate.CommandSql = qrUpdate.CommandSql.Remove(qrUpdate.CommandSql.IndexOf(", [ModifiedOn] = @ModifiedOn"), 28);
                            qrUpdate.Parameters.GetParameter("@ModifiedOn").ParameterValue = ord.ModifiedOn;
                        }
                        else
                        {
                            Logger.writeLog("Error in Query : " + qrUpdate.CommandSql);
                        }
                    }

                    col.Add(qrUpdate);

                    #region *) Upsert to ServerQuickRef table
                    if (AppSetting.CastBool(AppSetting.GetSetting("UseServerQuickRef"), false))
                    {
                        int pointofsaleID = (int)qr.Parameters.GetParameter("@PointOfSaleID").ParameterValue;
                        DateTime modifiedOn = (DateTime)qr.Parameters.GetParameter("@ModifiedOn").ParameterValue;
                        QueryCommand cmdUpsert = ServerQuickRefController.Generate_Upsert_OrderHdr(pointofsaleID, modifiedOn);
                        col.Add(cmdUpsert);
                    }
                    #endregion
                }
                else
                {
                    //New Item. Perform Create
                    ord = new OrderHdr();
                    qr = ord.GetInsertCommand("SYNC");
                    for (int oa = 0; oa < qr.Parameters.Count; oa++)
                    {
                        /*if (qr.Parameters[oa].ParameterName.ToLower() != "@createdon" &
                                   qr.Parameters[oa].ParameterName.ToLower() != "@createdby" &
                                    qr.Parameters[oa].ParameterName.ToLower() != "@modifiedon" &
                                    qr.Parameters[oa].ParameterName.ToLower() != "@modifiedby")
                        {*/
                        //Logger.writeLog(qr.Parameters[oa].ParameterName + " val: " + qr.Parameters[oa].ParameterValue.ToString());
                        if (oa >= objHdr[i].Length) continue;
                        switch (qr.Parameters[oa].DataType)
                        {
                            case DbType.Boolean:
                                if (objHdr[i][oa] == "0")
                                    qr.Parameters[oa].ParameterValue = false;
                                else if (objHdr[i][oa] == "1")
                                    qr.Parameters[oa].ParameterValue = true;
                                break;
                            case DbType.Date:
                            case DbType.DateTime:
                                if (!string.IsNullOrEmpty(objHdr[i][oa]))
                                {
                                    qr.Parameters[oa].ParameterValue = DateTime.Parse(objHdr[i][oa]);
                                    Logger.writeLog(">>>" + objHdr[i][oa]);
                                }
                                break;
                            case DbType.Decimal:
                            case DbType.Currency:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Decimal.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Double:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Double.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Single:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Single.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Guid:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = new Guid(objHdr[i][oa]);
                                break;
                            case DbType.Int16:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int16.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Int32:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int32.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Int64:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int64.Parse(objHdr[i][oa]);
                                break;
                            case DbType.UInt16:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt16.Parse(objHdr[i][oa]);
                                break;
                            case DbType.UInt32:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt32.Parse(objHdr[i][oa]);
                                break;
                            case DbType.UInt64:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt64.Parse(objHdr[i][oa]);
                                break;
                            case DbType.StringFixedLength:
                            case DbType.String:
                                qr.Parameters[oa].ParameterValue = objHdr[i][oa];
                                break;
                            default:
                                qr.Parameters[oa].ParameterValue = objHdr[i][oa];
                                break;
                        }
                        //}

                    }
                    col.Add(qr);

                    #region *) Upsert to ServerQuickRef table
                    if (AppSetting.CastBool(AppSetting.GetSetting("UseServerQuickRef"), false))
                    {
                        int pointofsaleID = (int)qr.Parameters.GetParameter("@PointOfSaleID").ParameterValue;
                        DateTime modifiedOn = (DateTime)qr.Parameters.GetParameter("@ModifiedOn").ParameterValue;
                        QueryCommand cmdUpsert = ServerQuickRefController.Generate_Upsert_OrderHdr(pointofsaleID, modifiedOn);
                        col.Add(cmdUpsert);
                    }
                    #endregion
                }
            }
            if (duplicate.Count != 0)
            {
                string str;
                str = "Error: Attempting to enter duplicate Order Header Ref Number.";
                str = str + "\r\n" + "Duplicates:" + "\r\n";
                for (int i = 0; i < duplicate.Count; i++)
                    str += duplicate[i] + "\r\n";

                Logger.writeLog(str);
                throw new Exception("Duplicate found");
            }

            return col;

        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
            throw ex;
        }
    }

    private static QueryCommandCollection FetchOrderDetail
        (string[][] objDet)
    {
        try
        {
            ArrayList approvedList = new ArrayList();
            QueryCommandCollection col = new QueryCommandCollection();
            //Logger.writeLog("#records>" + objDet.Length);
            int UniqueIDIndex;

            OrderDet ord = new OrderDet();

            QueryCommand qr = ord.GetInsertCommand("SYNC");
            UniqueIDIndex = -1;

            for (int f = 0; f < qr.Parameters.Count; f++)
            {
                if (qr.Parameters[f].ParameterName.ToLower() == "@uniqueid")
                {
                    UniqueIDIndex = f;
                    break;
                }
            }

            if (UniqueIDIndex == -1)
            {
                Logger.writeLog("Sync table does not contain OrderHdrID.");
                throw new Exception("Sync table does not contain OrderHdrID.");
            }
            ArrayList allList = new ArrayList();
            for (int i = 0; i < objDet.Length; i++)
            {
                allList.Add(objDet[i][0]);
            }

            Object tmpGuid;
            ArrayList duplicate = new ArrayList();
            //Check GUID uniqueness in Target DB
            Query qry = new Query("OrderDet");
            qry.SelectList = "OrderDetID,UniqueID";
            DataTable dtUniqueID = qry.IN(OrderDet.Columns.OrderDetID, allList).ExecuteDataSet().Tables[0];

            for (int i = 0; i < objDet.Length; i++)
            {
                if (dtUniqueID.Rows.Count > 0)
                {
                    DataRow[] dr = dtUniqueID.Select("OrderDetID = '" + objDet[i][0] + "'");

                    if (dr.Length > 0)
                        tmpGuid = new Guid(dr[0]["UniqueID"].ToString());
                    else
                        tmpGuid = new Object();
                }
                else tmpGuid = new Object();

                if (dtUniqueID.Rows.Count > 0 && tmpGuid is Guid) //GUID Found?
                {
                    //if ((Guid)tmpGuid != new Guid(objDet[i][UniqueIDIndex]))
                    //{
                    //    //same order but different unique ID - Houston we have a problem                        
                    //    duplicate.Add(qr.Parameters[0].ParameterValue.ToString());
                    //}

                    qr = ord.GetInsertCommand("SYNC");
                    Logger.writeLog("Update OrderDetID = " + objDet[i][0]);
                    for (int oa = 0; oa < qr.Parameters.Count; oa++)
                    {
                        /*if (qr.Parameters[oa].ParameterName.ToLower() != "@createdon" &
                                   qr.Parameters[oa].ParameterName.ToLower() != "@createdby" &
                                    qr.Parameters[oa].ParameterName.ToLower() != "@modifiedon" &
                                    qr.Parameters[oa].ParameterName.ToLower() != "@modifiedby")
                        {*/
                        //Logger.writeLog(qr.Parameters[oa].ParameterName + " val: " + qr.Parameters[oa].ParameterValue.ToString());
                        if (oa >= objDet[i].Length) continue;
                        switch (qr.Parameters[oa].DataType)
                        {
                            case DbType.Boolean:
                                if (objDet[i][oa] == "0")
                                    qr.Parameters[oa].ParameterValue = false;
                                else if (objDet[i][oa] == "1")
                                    qr.Parameters[oa].ParameterValue = true;
                                break;
                            case DbType.Date:
                            case DbType.DateTime:
                                if (!string.IsNullOrEmpty(objDet[i][oa]))
                                {
                                    qr.Parameters[oa].ParameterValue = DateTime.Parse(objDet[i][oa]);
                                    Logger.writeLog(">>>" + objDet[i][oa]);
                                }
                                break;
                            case DbType.Decimal:
                            case DbType.Currency:
                                if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Decimal.Parse(objDet[i][oa]);
                                break;
                            case DbType.Double:
                                if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Double.Parse(objDet[i][oa]);
                                break;
                            case DbType.Single:
                                if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Single.Parse(objDet[i][oa]);
                                break;
                            case DbType.Guid:
                                if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = new Guid(objDet[i][oa]);
                                break;
                            case DbType.Int16:
                                if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int16.Parse(objDet[i][oa]);
                                break;
                            case DbType.Int32:
                                if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int32.Parse(objDet[i][oa]);
                                break;
                            case DbType.Int64:
                                if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int64.Parse(objDet[i][oa]);
                                break;
                            case DbType.UInt16:
                                if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt16.Parse(objDet[i][oa]);
                                break;
                            case DbType.UInt32:
                                if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt32.Parse(objDet[i][oa]);
                                break;
                            case DbType.UInt64:
                                if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt64.Parse(objDet[i][oa]);
                                break;
                            case DbType.StringFixedLength:
                            case DbType.String:
                                qr.Parameters[oa].ParameterValue = objDet[i][oa];
                                break;
                            default:
                                qr.Parameters[oa].ParameterValue = objDet[i][oa];
                                break;
                        }
                        //}

                    }
                    ord = new OrderDet(objDet[i][0]);
                    ord.IsPreOrder = (bool)qr.Parameters.GetParameter("@IsPreOrder").ParameterValue;
                    ord.Userfld1 = (string)qr.Parameters.GetParameter("@Userfld1").ParameterValue;
                    ord.DepositAmount = qr.Parameters.GetParameter("@Userfloat1").ParameterValue.ToString().GetDecimalValue();
                    ord.ModifiedBy = (string)qr.Parameters.GetParameter("@ModifiedBy").ParameterValue;
                    ord.ModifiedOn = (DateTime)qr.Parameters.GetParameter("@ModifiedOn").ParameterValue;

                    QueryCommand qrUpdate = ord.GetUpdateCommand("SYNC");
                    if (qrUpdate.CommandSql.IndexOf("[ModifiedBy] = @ModifiedBy, [ModifiedOn] = @ModifiedOn,") > 0)
                    {
                        qrUpdate.CommandSql = qrUpdate.CommandSql.Remove(qrUpdate.CommandSql.IndexOf("[ModifiedBy] = @ModifiedBy, [ModifiedOn] = @ModifiedOn,"), 55);
                        qrUpdate.Parameters.GetParameter("@ModifiedOn").ParameterValue = ord.ModifiedOn;
                    }
                    else
                    {
                        qrUpdate.CommandSql = qrUpdate.CommandSql.Remove(qrUpdate.CommandSql.IndexOf(", [ModifiedOn] = @ModifiedOn"), 28);
                        qrUpdate.Parameters.GetParameter("@ModifiedOn").ParameterValue = ord.ModifiedOn;
                    }

                    col.Add(qrUpdate);
                }
                else
                {
                    //It's Approved! - else ignore....
                    qr = ord.GetInsertCommand("SYNC");
                    Logger.writeLog("Insert Orderdet: " + objDet[i][0]);
                    Logger.writeLog(string.Format("Sync Insert OrderDet : {0} ", objDet[i].AsSingleLineString(",")));
                    for (int oa = 0; oa < qr.Parameters.Count; oa++)
                    {
                        if (qr.Parameters[oa].ParameterName.ToLower() != "@createdon" &
                                   qr.Parameters[oa].ParameterName.ToLower() != "@createdby" &
                                    qr.Parameters[oa].ParameterName.ToLower() != "@modifiedon" &
                                    qr.Parameters[oa].ParameterName.ToLower() != "@modifiedby")
                        {
                            if (oa >= objDet[i].Length) continue;
                            switch (qr.Parameters[oa].DataType)
                            {
                                case DbType.Boolean:
                                    if (objDet[i][oa] == "0")
                                        qr.Parameters[oa].ParameterValue = false;
                                    else if (objDet[i][oa] == "1")
                                        qr.Parameters[oa].ParameterValue = true;
                                    break;
                                case DbType.Date:
                                case DbType.DateTime:
                                    if (objDet[i][oa] != string.Empty)
                                        qr.Parameters[oa].ParameterValue = DateTime.Parse(objDet[i][oa]);
                                    break;
                                case DbType.Decimal:
                                case DbType.Currency:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Decimal.Parse(objDet[i][oa]);
                                    break;
                                case DbType.Double:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Double.Parse(objDet[i][oa]);
                                    break;
                                case DbType.Single:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Single.Parse(objDet[i][oa]);
                                    break;
                                case DbType.Guid:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = new Guid(objDet[i][oa]);
                                    break;
                                case DbType.Int16:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int16.Parse(objDet[i][oa]);
                                    break;
                                case DbType.Int32:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int32.Parse(objDet[i][oa]);
                                    break;
                                case DbType.Int64:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int64.Parse(objDet[i][oa]);
                                    break;
                                case DbType.UInt16:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt16.Parse(objDet[i][oa]);
                                    break;
                                case DbType.UInt32:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt32.Parse(objDet[i][oa]);
                                    break;
                                case DbType.UInt64:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt64.Parse(objDet[i][oa]);
                                    break;
                                case DbType.StringFixedLength:
                                case DbType.String:
                                    qr.Parameters[oa].ParameterValue = objDet[i][oa];
                                    break;
                                default:
                                    qr.Parameters[oa].ParameterValue = objDet[i][oa];
                                    break;
                            }
                        }
                    }
                    col.Add(qr);
                }
            }
            return col;
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
            throw ex;
        }
    }

    private static QueryCommandCollection FetchReceiptHdr(string[][] objReceiptHdr)
    {
        try
        {
            QueryCommandCollection col = new QueryCommandCollection();
            Object tmpGuid;

            //Logger.writeLog("#records>" + objReceiptHdr.Length);

            ReceiptHdr ord = new ReceiptHdr();

            //Get all list for executing IN query
            ArrayList allList = new ArrayList();
            for (int i = 0; i < objReceiptHdr.Length; i++)
            {
                allList.Add(objReceiptHdr[i][0]);
            }

            //Check GUID uniqueness in Target DB
            Query qry = new Query("ReceiptHdr");
            qry.SelectList = "ReceiptHdrID,UniqueID";
            DataTable dtUniqueID = qry.IN(ReceiptHdr.Columns.ReceiptHdrID, allList).ExecuteDataSet().Tables[0];

            //Find the unique Index
            int UniqueIDIndex;
            UniqueIDIndex = -1;
            QueryCommand qr = ord.GetInsertCommand("SYNC");
            for (int f = 0; f < qr.Parameters.Count; f++)
            {
                if (qr.Parameters[f].ParameterName.ToLower() == "@uniqueid")
                {
                    UniqueIDIndex = f;
                    break;
                }
            }
            if (UniqueIDIndex == -1)
            {
                Logger.writeLog("Sync table does not contain unique ID or unique ID name does not compy.");
                throw new Exception("Sync table does not contain unique ID or unique ID name does not compy.");
            }
            ArrayList duplicate = new ArrayList();

            for (int i = 0; i < objReceiptHdr.Length; i++)
            {
                if (dtUniqueID.Rows.Count > 0)
                {
                    DataRow[] dr = dtUniqueID.Select("ReceiptHdrID = '" + objReceiptHdr[i][0] + "'");

                    if (dr.Length > 0)
                        tmpGuid = new Guid(dr[0]["UniqueID"].ToString());
                    else
                        tmpGuid = new Object();
                }
                else tmpGuid = new Object();

                if (dtUniqueID.Rows.Count > 0 && tmpGuid is Guid) //GUID Found?
                {
                    if ((Guid)tmpGuid != new Guid(objReceiptHdr[i][UniqueIDIndex]))
                    {
                        //same order but different unique ID - Houston we have a problem                        
                        duplicate.Add(qr.Parameters[0].ParameterValue.ToString());
                    }
                }
                else
                {
                    //It's Approved! - else ignore....
                    qr = ord.GetInsertCommand("SYNC");
                    Logger.writeLog(string.Format("Sync Insert ReceiptHdr : {0}", objReceiptHdr[i].AsSingleLineString(",")));
                    for (int oa = 0; oa < qr.Parameters.Count; oa++)
                    {
                        if (qr.Parameters[oa].ParameterName.ToLower() != "@createdon" &
                                   qr.Parameters[oa].ParameterName.ToLower() != "@createdby" &
                                    qr.Parameters[oa].ParameterName.ToLower() != "@modifiedon" &
                                    qr.Parameters[oa].ParameterName.ToLower() != "@modifiedby")
                        {
                            if (oa >= objReceiptHdr[i].Length) continue;
                            switch (qr.Parameters[oa].DataType)
                            {
                                case DbType.Boolean:
                                    if (objReceiptHdr[i][oa] == "0")
                                        qr.Parameters[oa].ParameterValue = false;
                                    else if (objReceiptHdr[i][oa] == "1")
                                        qr.Parameters[oa].ParameterValue = true;
                                    break;
                                case DbType.Date:
                                case DbType.DateTime:
                                    if (objReceiptHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = DateTime.Parse(objReceiptHdr[i][oa]);
                                    break;
                                case DbType.Decimal:
                                case DbType.Currency:
                                    if (objReceiptHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Decimal.Parse(objReceiptHdr[i][oa]);
                                    break;
                                case DbType.Double:
                                    if (objReceiptHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Double.Parse(objReceiptHdr[i][oa]);
                                    break;
                                case DbType.Single:
                                    if (objReceiptHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Single.Parse(objReceiptHdr[i][oa]);
                                    break;
                                case DbType.Guid:
                                    if (objReceiptHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = new Guid(objReceiptHdr[i][oa]);
                                    break;
                                case DbType.Int16:
                                    if (objReceiptHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int16.Parse(objReceiptHdr[i][oa]);
                                    break;
                                case DbType.Int32:
                                    if (objReceiptHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int32.Parse(objReceiptHdr[i][oa]);
                                    break;
                                case DbType.Int64:
                                    if (objReceiptHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int64.Parse(objReceiptHdr[i][oa]);
                                    break;
                                case DbType.UInt16:
                                    if (objReceiptHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt16.Parse(objReceiptHdr[i][oa]);
                                    break;
                                case DbType.UInt32:
                                    if (objReceiptHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt32.Parse(objReceiptHdr[i][oa]);
                                    break;
                                case DbType.UInt64:
                                    if (objReceiptHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt64.Parse(objReceiptHdr[i][oa]);
                                    break;
                                case DbType.StringFixedLength:
                                case DbType.String:
                                    qr.Parameters[oa].ParameterValue = objReceiptHdr[i][oa];
                                    break;
                                default:
                                    qr.Parameters[oa].ParameterValue = objReceiptHdr[i][oa];
                                    break;
                            }
                        }
                    }
                    col.Add(qr);
                }
            }
            if (duplicate.Count != 0)
            {
                /*string str;
                str = "Error: Attempting to enter duplicate Order Header Ref Number.";
                str = str + "\r\n" + "Duplicates:" + "\r\n";
                for (int i = 0; i < duplicate.Count; i++)
                    str += duplicate[i] + "\r\n";

                Logger.writeLog(str);
                throw new Exception("Duplicate found");*/
            }
            return col;
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
            throw ex;
        }
    }

    private static QueryCommandCollection FetchReceiptDet(string[][] objReceiptDet)
    {
        try
        {
            //Initialization
            QueryCommandCollection col = new QueryCommandCollection();
            //Logger.writeLog("#records>" + objReceiptDet.Length);
            ReceiptDet ord = new ReceiptDet();

            //Check GUID uniqueness in Target DB
            ArrayList allList = new ArrayList();
            for (int i = 0; i < objReceiptDet.Length; i++)
            {
                allList.Add(objReceiptDet[i][0]);
            }


            Query qry = new Query("ReceiptDet");
            qry.SelectList = "ReceiptDetID,UniqueID";
            DataTable dtUniqueID = qry.IN(ReceiptDet.Columns.ReceiptDetID, allList).ExecuteDataSet().Tables[0];

            //Get UniqueID Index for comparison purposes
            int UniqueIDIndex;
            UniqueIDIndex = -1;

            QueryCommand qr = ord.GetInsertCommand("SYNC");
            for (int f = 0; f < qr.Parameters.Count; f++)
            {
                if (qr.Parameters[f].ParameterName.ToLower() == "@uniqueid")
                {
                    UniqueIDIndex = f;
                    break;
                }
            }
            if (UniqueIDIndex == -1)
            {
                Logger.writeLog("Sync table does not contain unique ID or unique ID name does not compy.");
                throw new Exception("Sync table does not contain unique ID or unique ID name does not compy.");
            }
            Object tmpGuid;
            ArrayList duplicate = new ArrayList();
            for (int i = 0; i < objReceiptDet.Length; i++)
            {

                if (dtUniqueID.Rows.Count > 0)
                {
                    DataRow[] dr = dtUniqueID.Select("ReceiptDetID = '" + objReceiptDet[i][0] + "'");

                    if (dr.Length > 0)
                        tmpGuid = new Guid(dr[0]["UniqueID"].ToString());
                    else
                        tmpGuid = new Object();
                }
                else tmpGuid = new Object();

                if (dtUniqueID.Rows.Count > 0 && tmpGuid is Guid) //GUID Found?
                {
                    if ((Guid)tmpGuid != new Guid(objReceiptDet[i][UniqueIDIndex]))
                    {
                        //same order but different unique ID - Houston we have a problem                        
                        Query qrDelete = ReceiptDet.CreateQuery();
                        qrDelete.QueryType = QueryType.Delete;
                        col.Add(qrDelete.WHERE(ReceiptDet.Columns.ReceiptDetID, objReceiptDet[i][0]).BuildDeleteCommand());

                        qr = ord.GetInsertCommand("SYNC");
                        Logger.writeLog(string.Format("Sync Insert ReceiptDet : {0}", objReceiptDet[i].AsSingleLineString(",")));

                        for (int oa = 0; oa < qr.Parameters.Count; oa++)
                        {
                            if (qr.Parameters[oa].ParameterName.ToLower() != "@createdon" &
                                       qr.Parameters[oa].ParameterName.ToLower() != "@createdby" &
                                        qr.Parameters[oa].ParameterName.ToLower() != "@modifiedon" &
                                        qr.Parameters[oa].ParameterName.ToLower() != "@modifiedby")
                            {
                                if (oa >= objReceiptDet[i].Length) continue;
                                switch (qr.Parameters[oa].DataType)
                                {
                                    case DbType.Boolean:
                                        if (objReceiptDet[i][oa] == "0")
                                            qr.Parameters[oa].ParameterValue = false;
                                        else if (objReceiptDet[i][oa] == "1")
                                            qr.Parameters[oa].ParameterValue = true;
                                        break;
                                    case DbType.Date:
                                    case DbType.DateTime:
                                        if (objReceiptDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = DateTime.Parse(objReceiptDet[i][oa]);
                                        break;
                                    case DbType.Decimal:
                                    case DbType.Currency:
                                        if (objReceiptDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Decimal.Parse(objReceiptDet[i][oa]);
                                        break;
                                    case DbType.Double:
                                        if (objReceiptDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Double.Parse(objReceiptDet[i][oa]);
                                        break;
                                    case DbType.Single:
                                        if (objReceiptDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Single.Parse(objReceiptDet[i][oa]);
                                        break;
                                    case DbType.Guid:
                                        if (objReceiptDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = new Guid(objReceiptDet[i][oa]);
                                        break;
                                    case DbType.Int16:
                                        if (objReceiptDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int16.Parse(objReceiptDet[i][oa]);
                                        break;
                                    case DbType.Int32:
                                        if (objReceiptDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int32.Parse(objReceiptDet[i][oa]);
                                        break;
                                    case DbType.Int64:
                                        if (objReceiptDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int64.Parse(objReceiptDet[i][oa]);
                                        break;
                                    case DbType.UInt16:
                                        if (objReceiptDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt16.Parse(objReceiptDet[i][oa]);
                                        break;
                                    case DbType.UInt32:
                                        if (objReceiptDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt32.Parse(objReceiptDet[i][oa]);
                                        break;
                                    case DbType.UInt64:
                                        if (objReceiptDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt64.Parse(objReceiptDet[i][oa]);
                                        break;
                                    case DbType.StringFixedLength:
                                    case DbType.String:
                                        if (objReceiptDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = objReceiptDet[i][oa];
                                        break;
                                    default:
                                        if (objReceiptDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = objReceiptDet[i][oa];
                                        break;
                                }
                            }
                        }
                        col.Add(qr);
                    }


                }
                else
                {
                    //It's Approved! - else ignore....
                    qr = ord.GetInsertCommand("SYNC");
                    Logger.writeLog(string.Format("Sync Insert ReceiptDet : {0}", objReceiptDet[i].AsSingleLineString(",")));

                    for (int oa = 0; oa < qr.Parameters.Count; oa++)
                    {
                        if (qr.Parameters[oa].ParameterName.ToLower() != "@createdon" &
                                   qr.Parameters[oa].ParameterName.ToLower() != "@createdby" &
                                    qr.Parameters[oa].ParameterName.ToLower() != "@modifiedon" &
                                    qr.Parameters[oa].ParameterName.ToLower() != "@modifiedby")
                        {
                            if (oa >= objReceiptDet[i].Length) continue;
                            switch (qr.Parameters[oa].DataType)
                            {
                                case DbType.Boolean:
                                    if (objReceiptDet[i][oa] == "0")
                                        qr.Parameters[oa].ParameterValue = false;
                                    else if (objReceiptDet[i][oa] == "1")
                                        qr.Parameters[oa].ParameterValue = true;
                                    break;
                                case DbType.Date:
                                case DbType.DateTime:
                                    if (objReceiptDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = DateTime.Parse(objReceiptDet[i][oa]);
                                    break;
                                case DbType.Decimal:
                                case DbType.Currency:
                                    if (objReceiptDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Decimal.Parse(objReceiptDet[i][oa]);
                                    break;
                                case DbType.Double:
                                    if (objReceiptDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Double.Parse(objReceiptDet[i][oa]);
                                    break;
                                case DbType.Single:
                                    if (objReceiptDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Single.Parse(objReceiptDet[i][oa]);
                                    break;
                                case DbType.Guid:
                                    if (objReceiptDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = new Guid(objReceiptDet[i][oa]);
                                    break;
                                case DbType.Int16:
                                    if (objReceiptDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int16.Parse(objReceiptDet[i][oa]);
                                    break;
                                case DbType.Int32:
                                    if (objReceiptDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int32.Parse(objReceiptDet[i][oa]);
                                    break;
                                case DbType.Int64:
                                    if (objReceiptDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int64.Parse(objReceiptDet[i][oa]);
                                    break;
                                case DbType.UInt16:
                                    if (objReceiptDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt16.Parse(objReceiptDet[i][oa]);
                                    break;
                                case DbType.UInt32:
                                    if (objReceiptDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt32.Parse(objReceiptDet[i][oa]);
                                    break;
                                case DbType.UInt64:
                                    if (objReceiptDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt64.Parse(objReceiptDet[i][oa]);
                                    break;
                                case DbType.StringFixedLength:
                                case DbType.String:
                                    if (objReceiptDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = objReceiptDet[i][oa];
                                    break;
                                default:
                                    if (objReceiptDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = objReceiptDet[i][oa];
                                    break;
                            }
                        }
                    }
                    col.Add(qr);
                }
            }
            if (duplicate.Count != 0)
            {
                /*string str;
                str = "Error: Attempting to enter duplicate Order Header Ref Number.";
                str = str + "\r\n" + "Duplicates:" + "\r\n";
                for (int i = 0; i < duplicate.Count; i++)
                    str += duplicate[i] + "\r\n";

                Logger.writeLog(str);
                throw new Exception("Duplicate found");*/
            }

            return col;
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
            throw ex;
        }
    }

    private static QueryCommandCollection FetchAccessLog(string[][] objSalesComm)
    {
        try
        {
            QueryCommandCollection col = new QueryCommandCollection();
            AccessLog ord = new AccessLog();

            ArrayList allList = new ArrayList();
            for (int i = 0; i < objSalesComm.Length; i++)
                allList.Add(objSalesComm[i][0]);


            Query qry = new Query("AccessLog");
            qry.SelectList = "AccessLogID";
            DataTable dtUniqueID = qry.IN(AccessLog.Columns.AccessLogID, allList).ExecuteDataSet().Tables[0];

            //Get UniqueID Index for comparison purposes
            int UniqueIDIndex;
            UniqueIDIndex = -1;

            QueryCommand qr = ord.GetInsertCommand("SYNC");
            for (int f = 0; f < qr.Parameters.Count; f++)
            {
                if (qr.Parameters[f].ParameterName.ToLower() == "@accesslogid")
                {
                    UniqueIDIndex = f;
                    break;
                }
            }
            if (UniqueIDIndex == -1)
            {
                Logger.writeLog("Sync table does not contain unique ID or unique ID name does not compy.");
                throw new Exception("Sync table does not contain unique ID or unique ID name does not compy.");
            }
            Object tmpGuid;
            ArrayList duplicate = new ArrayList();
            for (int i = 0; i < objSalesComm.Length; i++)
            {

                if (dtUniqueID.Rows.Count > 0)
                {
                    DataRow[] dr = dtUniqueID.Select("AccessLogID = '" + objSalesComm[i][0] + "'");

                    if (dr.Length > 0)
                        tmpGuid = new Guid(dr[0]["AccessLogID"].ToString());
                    else
                        tmpGuid = new Object();
                }
                else tmpGuid = new Object();

                if (dtUniqueID.Rows.Count > 0 && tmpGuid is Guid) //GUID Found?
                {
                    if ((Guid)tmpGuid != new Guid(objSalesComm[i][UniqueIDIndex]))
                    {
                        //same order but different unique ID - Houston we have a problem                        
                        //duplicate.Add(qr.Parameters[0].ParameterValue.ToString());
                    }
                }
                else
                {
                    //It's Approved! - else ignore....
                    qr = ord.GetInsertCommand("SYNC");
                    Logger.writeLog(string.Format("Sync Insert AccessLog : {0}", objSalesComm[i].AsSingleLineString(",")));

                    for (int oa = 0; oa < qr.Parameters.Count; oa++)
                    {
                        if (qr.Parameters[oa].ParameterName.ToLower() != "@createdon" &
                                   qr.Parameters[oa].ParameterName.ToLower() != "@createdby" &
                                    qr.Parameters[oa].ParameterName.ToLower() != "@modifiedon" &
                                    qr.Parameters[oa].ParameterName.ToLower() != "@modifiedby")
                        {
                            if (oa >= objSalesComm[i].Length) continue;
                            switch (qr.Parameters[oa].DataType)
                            {
                                case DbType.Boolean:
                                    if (objSalesComm[i][oa] == "0")
                                        qr.Parameters[oa].ParameterValue = false;
                                    else if (objSalesComm[i][oa] == "1")
                                        qr.Parameters[oa].ParameterValue = true;
                                    break;
                                case DbType.Date:
                                case DbType.DateTime:
                                    if (objSalesComm[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = DateTime.Parse(objSalesComm[i][oa]);
                                    break;
                                case DbType.Decimal:
                                case DbType.Currency:
                                    if (objSalesComm[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Decimal.Parse(objSalesComm[i][oa]);
                                    break;
                                case DbType.Double:
                                    if (objSalesComm[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Double.Parse(objSalesComm[i][oa]);
                                    break;
                                case DbType.Single:
                                    if (objSalesComm[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Single.Parse(objSalesComm[i][oa]);
                                    break;
                                case DbType.Guid:
                                    if (objSalesComm[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = new Guid(objSalesComm[i][oa]);
                                    break;
                                case DbType.Int16:
                                    if (objSalesComm[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int16.Parse(objSalesComm[i][oa]);
                                    break;
                                case DbType.Int32:
                                    if (objSalesComm[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int32.Parse(objSalesComm[i][oa]);
                                    break;
                                case DbType.Int64:
                                    if (objSalesComm[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int64.Parse(objSalesComm[i][oa]);
                                    break;
                                case DbType.UInt16:
                                    if (objSalesComm[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt16.Parse(objSalesComm[i][oa]);
                                    break;
                                case DbType.UInt32:
                                    if (objSalesComm[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt32.Parse(objSalesComm[i][oa]);
                                    break;
                                case DbType.UInt64:
                                    if (objSalesComm[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt64.Parse(objSalesComm[i][oa]);
                                    break;
                                case DbType.StringFixedLength:
                                case DbType.String:
                                    if (objSalesComm[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = objSalesComm[i][oa];
                                    break;
                                default:
                                    if (objSalesComm[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = objSalesComm[i][oa];
                                    break;
                            }
                        }
                    }
                    col.Add(qr);
                }
            }
            if (duplicate.Count != 0)
            {
                /*string str;
                str = "Error: Attempting to enter duplicate Order Header Ref Number.";
                str = str + "\r\n" + "Duplicates:" + "\r\n";
                for (int i = 0; i < duplicate.Count; i++)
                    str += duplicate[i] + "\r\n";

                Logger.writeLog(str);
                throw new Exception("Duplicate found");*/
            }

            return col;
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
            throw ex;
        }
    }

    private static QueryCommandCollection FetchAttendance(string[][] objSalesComm)
    {
        try
        {
            QueryCommandCollection col = new QueryCommandCollection();
            MembershipAttendance ord = new MembershipAttendance();

            ArrayList allList = new ArrayList();
            for (int i = 0; i < objSalesComm.Length; i++)
                allList.Add(objSalesComm[i][5]);


            Query qry = new Query("MembershipAttendance");
            qry.SelectList = "UniqueID";
            DataTable dtUniqueID = qry.IN(MembershipAttendance.Columns.UniqueID, allList).ExecuteDataSet().Tables[0];

            //Get UniqueID Index for comparison purposes
            int UniqueIDIndex;
            UniqueIDIndex = -1;

            QueryCommand qr = ord.GetInsertCommand("SYNC");
            for (int f = 0; f < qr.Parameters.Count; f++)
            {
                if (qr.Parameters[f].ParameterName.ToLower() == "@uniqueid")
                {
                    UniqueIDIndex = f;
                    break;
                }
            }
            if (UniqueIDIndex == -1)
            {
                Logger.writeLog("Attendance table does not contain unique ID or unique ID name does not compy.");
                throw new Exception("Attendance table does not contain unique ID or unique ID name does not compy.");
            }
            Object tmpGuid;
            ArrayList duplicate = new ArrayList();
            for (int i = 0; i < objSalesComm.Length; i++)
            {

                if (dtUniqueID.Rows.Count > 0)
                {
                    DataRow[] dr = dtUniqueID.Select("UniqueID = '" + objSalesComm[i][6] + "'");

                    if (dr.Length > 0)
                        tmpGuid = new Guid(dr[0]["UniqueID"].ToString());
                    else
                        tmpGuid = new Object();
                }
                else tmpGuid = new Object();

                if (dtUniqueID.Rows.Count > 0 && tmpGuid is Guid) //GUID Found?
                {
                    if ((Guid)tmpGuid != new Guid(objSalesComm[i][UniqueIDIndex]))
                    {
                        //same order but different unique ID - Houston we have a problem                        
                        //duplicate.Add(qr.Parameters[0].ParameterValue.ToString());
                    }
                }
                else
                {
                    //It's Approved! - else ignore....
                    qr = ord.GetInsertCommand("SYNC");
                    Logger.writeLog(string.Format("Sync Insert Attendance : {0}", objSalesComm[i].AsSingleLineString(",")));

                    for (int oa = 0; oa < qr.Parameters.Count; oa++)
                    {
                        //if (qr.Parameters[oa].ParameterName.ToLower() != "@createdon" &
                        //           qr.Parameters[oa].ParameterName.ToLower() != "@createdby" &
                        //            qr.Parameters[oa].ParameterName.ToLower() != "@modifiedon" &
                        //            qr.Parameters[oa].ParameterName.ToLower() != "@modifiedby")
                        //{
                        if (oa >= objSalesComm[i].Length) continue;
                        switch (qr.Parameters[oa].DataType)
                        {
                            case DbType.Boolean:
                                if (objSalesComm[i][oa] == "0")
                                    qr.Parameters[oa].ParameterValue = false;
                                else if (objSalesComm[i][oa] == "1")
                                    qr.Parameters[oa].ParameterValue = true;
                                break;
                            case DbType.Date:
                            case DbType.DateTime:
                                if (objSalesComm[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = DateTime.Parse(objSalesComm[i][oa]);
                                break;
                            case DbType.Decimal:
                            case DbType.Currency:
                                if (objSalesComm[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Decimal.Parse(objSalesComm[i][oa]);
                                break;
                            case DbType.Double:
                                if (objSalesComm[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Double.Parse(objSalesComm[i][oa]);
                                break;
                            case DbType.Single:
                                if (objSalesComm[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Single.Parse(objSalesComm[i][oa]);
                                break;
                            case DbType.Guid:
                                if (objSalesComm[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = new Guid(objSalesComm[i][oa]);
                                break;
                            case DbType.Int16:
                                if (objSalesComm[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int16.Parse(objSalesComm[i][oa]);
                                break;
                            case DbType.Int32:
                                if (objSalesComm[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int32.Parse(objSalesComm[i][oa]);
                                break;
                            case DbType.Int64:
                                if (objSalesComm[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int64.Parse(objSalesComm[i][oa]);
                                break;
                            case DbType.UInt16:
                                if (objSalesComm[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt16.Parse(objSalesComm[i][oa]);
                                break;
                            case DbType.UInt32:
                                if (objSalesComm[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt32.Parse(objSalesComm[i][oa]);
                                break;
                            case DbType.UInt64:
                                if (objSalesComm[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt64.Parse(objSalesComm[i][oa]);
                                break;
                            case DbType.StringFixedLength:
                            case DbType.String:
                                if (objSalesComm[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = objSalesComm[i][oa];
                                break;
                            default:
                                if (objSalesComm[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = objSalesComm[i][oa];
                                break;
                        }
                        //}
                    }
                    col.Add(qr);
                }
            }
            if (duplicate.Count != 0)
            {
                /*string str;
                str = "Error: Attempting to enter duplicate Order Header Ref Number.";
                str = str + "\r\n" + "Duplicates:" + "\r\n";
                for (int i = 0; i < duplicate.Count; i++)
                    str += duplicate[i] + "\r\n";

                Logger.writeLog(str);
                throw new Exception("Duplicate found");*/
            }

            return col;
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
            throw ex;
        }
    }

    private static QueryCommandCollection FetchSalesCommission(string[][] objSalesComm)
    {
        try
        {
            //Initialization
            QueryCommandCollection col = new QueryCommandCollection();
            //Logger.writeLog("#records>" + objSalesComm.Length);
            SalesCommissionRecord ord = new SalesCommissionRecord();

            //Check GUID uniqueness in Target DB
            ArrayList allList = new ArrayList();
            for (int i = 0; i < objSalesComm.Length; i++)
            {
                allList.Add(objSalesComm[i][2]);
            }


            Query qry = new Query("SalesCommissionRecord");
            qry.SelectList = "OrderHdrID,UniqueID";
            DataTable dtUniqueID = qry.IN(SalesCommissionRecord.Columns.OrderHdrID, allList).ExecuteDataSet().Tables[0];

            //Get UniqueID Index for comparison purposes
            int UniqueIDIndex;
            UniqueIDIndex = -1;

            QueryCommand qr = ord.GetInsertCommand("SYNC");
            for (int f = 0; f < qr.Parameters.Count; f++)
            {
                if (qr.Parameters[f].ParameterName.ToLower() == "@uniqueid")
                {
                    UniqueIDIndex = f;
                    break;
                }
            }
            if (UniqueIDIndex == -1)
            {
                Logger.writeLog("Sync table does not contain unique ID or unique ID name does not compy.");
                throw new Exception("Sync table does not contain unique ID or unique ID name does not compy.");
            }
            Object tmpGuid;
            ArrayList duplicate = new ArrayList();
            for (int i = 0; i < objSalesComm.Length; i++)
            {

                if (dtUniqueID.Rows.Count > 0)
                {
                    Logger.writeLog("Checking duplicate for OrderHdrID : " + objSalesComm[i][2]);
                    DataRow[] dr = dtUniqueID.Select("OrderHdrID = '" + objSalesComm[i][2] + "'");

                    if (dr.Length > 0)
                        tmpGuid = new Guid(dr[0]["UniqueID"].ToString());
                    else
                        tmpGuid = new Object();
                }
                else tmpGuid = new Object();

                if (dtUniqueID.Rows.Count > 0 && tmpGuid is Guid) //GUID Found and duplicate?
                {
                    if ((Guid)tmpGuid != new Guid(objSalesComm[i][UniqueIDIndex]))
                    {
                        //same order but different unique ID - Houston we have a problem                        
                        //duplicate.Add(qr.Parameters[0].ParameterValue.ToString());
                    }
                }
                else
                {
                    //It's Approved! - else ignore....
                    qr = ord.GetInsertCommand("SYNC");
                    Logger.writeLog(string.Format("Sync Insert SalesCommissionRecord : {0}", objSalesComm[i].AsSingleLineString(",")));

                    for (int oa = 0; oa < qr.Parameters.Count; oa++)
                    {
                        if (qr.Parameters[oa].ParameterName.ToLower() != "@createdon" &
                                   qr.Parameters[oa].ParameterName.ToLower() != "@createdby" &
                                    qr.Parameters[oa].ParameterName.ToLower() != "@modifiedon" &
                                    qr.Parameters[oa].ParameterName.ToLower() != "@modifiedby")
                        {
                            if (oa >= objSalesComm[i].Length) continue;
                            switch (qr.Parameters[oa].DataType)
                            {
                                case DbType.Boolean:
                                    if (objSalesComm[i][oa] == "0")
                                        qr.Parameters[oa].ParameterValue = false;
                                    else if (objSalesComm[i][oa] == "1")
                                        qr.Parameters[oa].ParameterValue = true;
                                    break;
                                case DbType.Date:
                                case DbType.DateTime:
                                    if (objSalesComm[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = DateTime.Parse(objSalesComm[i][oa]);
                                    break;
                                case DbType.Decimal:
                                case DbType.Currency:
                                    if (objSalesComm[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Decimal.Parse(objSalesComm[i][oa]);
                                    break;
                                case DbType.Double:
                                    if (objSalesComm[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Double.Parse(objSalesComm[i][oa]);
                                    break;
                                case DbType.Single:
                                    if (objSalesComm[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Single.Parse(objSalesComm[i][oa]);
                                    break;
                                case DbType.Guid:
                                    if (objSalesComm[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = new Guid(objSalesComm[i][oa]);
                                    break;
                                case DbType.Int16:
                                    if (objSalesComm[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int16.Parse(objSalesComm[i][oa]);
                                    break;
                                case DbType.Int32:
                                    if (objSalesComm[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int32.Parse(objSalesComm[i][oa]);
                                    break;
                                case DbType.Int64:
                                    if (objSalesComm[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int64.Parse(objSalesComm[i][oa]);
                                    break;
                                case DbType.UInt16:
                                    if (objSalesComm[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt16.Parse(objSalesComm[i][oa]);
                                    break;
                                case DbType.UInt32:
                                    if (objSalesComm[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt32.Parse(objSalesComm[i][oa]);
                                    break;
                                case DbType.UInt64:
                                    if (objSalesComm[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt64.Parse(objSalesComm[i][oa]);
                                    break;
                                case DbType.StringFixedLength:
                                case DbType.String:
                                    if (objSalesComm[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = objSalesComm[i][oa];
                                    break;
                                default:
                                    if (objSalesComm[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = objSalesComm[i][oa];
                                    break;
                            }
                        }
                    }
                    col.Add(qr);
                }
            }
            if (duplicate.Count != 0)
            {
                /*string str;
                str = "Error: Attempting to enter duplicate Order Header Ref Number.";
                str = str + "\r\n" + "Duplicates:" + "\r\n";
                for (int i = 0; i < duplicate.Count; i++)
                    str += duplicate[i] + "\r\n";

                Logger.writeLog(str);
                throw new Exception("Duplicate found");*/
            }

            return col;
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
            throw ex;
        }
    }

    private static QueryCommandCollection FetchMembers(string[][] objMembers)
    {
        try
        {
            //Initialization
            QueryCommandCollection col = new QueryCommandCollection();
            //Logger.writeLog("#records>" + objMembers.Length);
            PowerPOS.Membership ord = new PowerPOS.Membership();

            //Check GUID uniqueness in Target DB
            ArrayList allList = new ArrayList();
            for (int i = 0; i < objMembers.Length; i++)
            {
                allList.Add(objMembers[i][0]);
            }


            Query qry = new Query("Membership");
            qry.SelectList = "MembershipNo,UniqueID";
            DataTable dtUniqueID = qry.IN(PowerPOS.Membership.Columns.MembershipNo, allList).ExecuteDataSet().Tables[0];

            //Get UniqueID Index for comparison purposes
            int UniqueIDIndex;
            UniqueIDIndex = -1;

            QueryCommand qr = ord.GetInsertCommand("SYNC");
            for (int f = 0; f < qr.Parameters.Count; f++)
            {
                if (qr.Parameters[f].ParameterName.ToLower() == "@uniqueid")
                {
                    UniqueIDIndex = f;
                    break;
                }
            }
            if (UniqueIDIndex == -1)
            {
                Logger.writeLog("Sync table does not contain unique ID or unique ID name does not compy.");
                throw new Exception("Sync table does not contain unique ID or unique ID name does not compy.");
            }
            Object tmpGuid;
            ArrayList duplicate = new ArrayList();
            for (int i = 0; i < objMembers.Length; i++)
            {

                if (dtUniqueID.Rows.Count > 0)
                {
                    DataRow[] dr = dtUniqueID.Select("MembershipNo = '" + objMembers[i][0] + "'");

                    if (dr.Length > 0)
                        tmpGuid = new Guid(dr[0]["UniqueID"].ToString());
                    else
                        tmpGuid = new Object();
                }
                else tmpGuid = new Object();

                if (dtUniqueID.Rows.Count > 0 && tmpGuid is Guid) //GUID Found?
                {
                    qr = ord.GetInsertCommand("SYNC");
                    Logger.writeLog(string.Format("Sync Insert Membership : {0}", objMembers[i].AsSingleLineString(",")));
                    for (int oa = 0; oa < qr.Parameters.Count; oa++)
                    {
                        if (oa >= objMembers[i].Length) continue;
                        if (qr.Parameters[oa].ParameterName.ToLower() != "@createdon" &
                                   qr.Parameters[oa].ParameterName.ToLower() != "@createdby" &
                                    qr.Parameters[oa].ParameterName.ToLower() != "@modifiedon" &
                                    qr.Parameters[oa].ParameterName.ToLower() != "@modifiedby")
                        {
                            switch (qr.Parameters[oa].DataType)
                            {
                                case DbType.Boolean:
                                    if (objMembers[i][oa] == "0")
                                        qr.Parameters[oa].ParameterValue = false;
                                    else if (objMembers[i][oa] == "1")
                                        qr.Parameters[oa].ParameterValue = true;
                                    break;
                                case DbType.Date:
                                case DbType.DateTime:
                                    if (objMembers[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = DateTime.Parse(objMembers[i][oa]);
                                    break;
                                case DbType.Decimal:
                                case DbType.Currency:
                                    if (objMembers[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Decimal.Parse(objMembers[i][oa]);
                                    break;
                                case DbType.Double:
                                    if (objMembers[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Double.Parse(objMembers[i][oa]);
                                    break;
                                case DbType.Single:
                                    if (objMembers[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Single.Parse(objMembers[i][oa]);
                                    break;
                                case DbType.Guid:
                                    if (objMembers[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = new Guid(objMembers[i][oa]);
                                    break;
                                case DbType.Int16:
                                    if (objMembers[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int16.Parse(objMembers[i][oa]);
                                    break;
                                case DbType.Int32:
                                    if (objMembers[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int32.Parse(objMembers[i][oa]);
                                    break;
                                case DbType.Int64:
                                    if (objMembers[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int64.Parse(objMembers[i][oa]);
                                    break;
                                case DbType.UInt16:
                                    if (objMembers[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt16.Parse(objMembers[i][oa]);
                                    break;
                                case DbType.UInt32:
                                    if (objMembers[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt32.Parse(objMembers[i][oa]);
                                    break;
                                case DbType.UInt64:
                                    if (objMembers[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt64.Parse(objMembers[i][oa]);
                                    break;
                                case DbType.StringFixedLength:
                                case DbType.String:
                                    if (objMembers[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = objMembers[i][oa];
                                    break;
                                default:
                                    if (objMembers[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = objMembers[i][oa];
                                    break;
                            }
                        }
                    }

                    PowerPOS.Membership member = new PowerPOS.Membership();
                    member = new PowerPOS.Membership(objMembers[i][0]);
                    member.MembershipGroupId = (int)qr.Parameters.GetParameter("@MembershipGroupId").ParameterValue;
                    if (qr.Parameters.GetParameter("@ExpiryDate").ParameterValue is DateTime)
                        member.ExpiryDate = (DateTime)qr.Parameters.GetParameter("@ExpiryDate").ParameterValue;
                    else
                        member.ExpiryDate = null;
                    //member.ModifiedBy = (string)qr.Parameters.GetParameter("@ModifiedBy").ParameterValue;
                    //member.ModifiedOn = (DateTime)qr.Parameters.GetParameter("@ModifiedOn").ParameterValue;


                    QueryCommand qrUpdate = member.GetUpdateCommand("SYNC");
                    /*if (qrUpdate.CommandSql.IndexOf("[ModifiedBy] = @ModifiedBy, [ModifiedOn] = @ModifiedOn,") > 0)
                    {
                        qrUpdate.CommandSql = qrUpdate.CommandSql.Remove(qrUpdate.CommandSql.IndexOf("[ModifiedBy] = @ModifiedBy, [ModifiedOn] = @ModifiedOn,"), 55);
                        qrUpdate.Parameters.GetParameter("@ModifiedOn").ParameterValue = ord.ModifiedOn;
                    }
                    else
                    {
                        qrUpdate.CommandSql = qrUpdate.CommandSql.Remove(qrUpdate.CommandSql.IndexOf(", [ModifiedOn] = @ModifiedOn"), 28);
                        qrUpdate.Parameters.GetParameter("@ModifiedOn").ParameterValue = ord.ModifiedOn;
                    }*/

                    col.Add(qrUpdate);

                }
                else
                {
                    //It's Approved! - else ignore....
                    qr = ord.GetInsertCommand("SYNC");
                    Logger.writeLog(string.Format("Sync Insert Membership : + 0", objMembers[i].AsSingleLineString(",")));
                    for (int oa = 0; oa < qr.Parameters.Count; oa++)
                    {
                        if (qr.Parameters[oa].ParameterName.ToLower() != "@createdon" &
                                   qr.Parameters[oa].ParameterName.ToLower() != "@createdby" &
                                    qr.Parameters[oa].ParameterName.ToLower() != "@modifiedon" &
                                    qr.Parameters[oa].ParameterName.ToLower() != "@modifiedby")
                        {
                            if (oa >= objMembers[i].Length) continue;
                            switch (qr.Parameters[oa].DataType)
                            {
                                case DbType.Boolean:
                                    if (objMembers[i][oa] == "0")
                                        qr.Parameters[oa].ParameterValue = false;
                                    else if (objMembers[i][oa] == "1")
                                        qr.Parameters[oa].ParameterValue = true;
                                    break;
                                case DbType.Date:
                                case DbType.DateTime:
                                    if (objMembers[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = DateTime.Parse(objMembers[i][oa]);
                                    break;
                                case DbType.Decimal:
                                case DbType.Currency:
                                    if (objMembers[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Decimal.Parse(objMembers[i][oa]);
                                    break;
                                case DbType.Double:
                                    if (objMembers[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Double.Parse(objMembers[i][oa]);
                                    break;
                                case DbType.Single:
                                    if (objMembers[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Single.Parse(objMembers[i][oa]);
                                    break;
                                case DbType.Guid:
                                    if (objMembers[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = new Guid(objMembers[i][oa]);
                                    break;
                                case DbType.Int16:
                                    if (objMembers[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int16.Parse(objMembers[i][oa]);
                                    break;
                                case DbType.Int32:
                                    if (objMembers[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int32.Parse(objMembers[i][oa]);
                                    break;
                                case DbType.Int64:
                                    if (objMembers[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int64.Parse(objMembers[i][oa]);
                                    break;
                                case DbType.UInt16:
                                    if (objMembers[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt16.Parse(objMembers[i][oa]);
                                    break;
                                case DbType.UInt32:
                                    if (objMembers[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt32.Parse(objMembers[i][oa]);
                                    break;
                                case DbType.UInt64:
                                    if (objMembers[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt64.Parse(objMembers[i][oa]);
                                    break;
                                case DbType.StringFixedLength:
                                case DbType.String:
                                    if (objMembers[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = objMembers[i][oa];
                                    break;
                                default:
                                    if (objMembers[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = objMembers[i][oa];
                                    break;
                            }
                        }
                    }
                    col.Add(qr);
                }
            }
            if (duplicate.Count != 0)
            {
                /*string str;
                str = "Error: Attempting to enter duplicate Order Header Ref Number.";
                str = str + "\r\n" + "Duplicates:" + "\r\n";
                for (int i = 0; i < duplicate.Count; i++)
                    str += duplicate[i] + "\r\n";

                Logger.writeLog(str);
                throw new Exception("Duplicate found");*/
            }

            return col;
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
            throw ex;
        }
    }

    private static QueryCommandCollection FetchMemberFromAppointmentRealTime(string[][] objHdr)
    {
        try
        {
            Query qry;
            Object tmpGuid;


            ArrayList duplicate = new ArrayList();

            //ORDER HEADER
            //Logger.writeLog("#records>" + objHdr.Length);

            PowerPOS.Membership app = new PowerPOS.Membership();

            QueryCommand qr = app.GetInsertCommand("SYNC");
            int UniqueIDIndex;
            UniqueIDIndex = -1;
            for (int f = 0; f < qr.Parameters.Count; f++)
            {
                if (qr.Parameters[f].ParameterName.ToLower() == "@uniqueid")
                {
                    UniqueIDIndex = f;
                    break;
                }
            }
            if (UniqueIDIndex == -1)
            {
                Logger.writeLog("Sync table does not contain unique ID or unique ID name does not compy.");
                throw new Exception("Sync table does not contain unique ID or unique ID name does not compy.");
            }

            QueryCommandCollection col = new QueryCommandCollection();
            ArrayList allList = new ArrayList();
            for (int i = 0; i < objHdr.Length; i++)
            {
                allList.Add(objHdr[i][0]);
            }

            //Check GUID uniqueness in Target DB
            qry = new Query("Membership");
            qry.SelectList = "MembershipNo,UniqueId";
            DataTable dtUniqueID = qry.IN(PowerPOS.Membership.Columns.MembershipNo, allList).ExecuteDataSet().Tables[0];

            for (int i = 0; i < objHdr.Length; i++)
            {
                if (dtUniqueID.Rows.Count > 0)
                {
                    DataRow[] dr = dtUniqueID.Select("MembershipNo = '" + objHdr[i][0] + "'");

                    if (dr.Length > 0)
                        tmpGuid = new Guid(dr[0]["UniqueID"].ToString());
                    else
                        tmpGuid = new Object();
                }
                else tmpGuid = new Object();

                if ((dtUniqueID.Rows.Count > 0 && tmpGuid is Guid) && ((Guid)tmpGuid != new Guid(objHdr[i][UniqueIDIndex]))) //GUID Found?
                {
                    //same order but different unique ID - Houston we have a problem                        
                    duplicate.Add(qr.Parameters[0].ParameterValue.ToString());
                }
                else
                {

                    if ((dtUniqueID.Rows.Count > 0 && tmpGuid is Guid) && (((Guid)tmpGuid).ToString() == (new Guid(objHdr[i][UniqueIDIndex])).ToString()))
                    {

                    }
                    else
                    {
                        //New Item. Perform Create
                        qr = app.GetInsertCommand("SYNC");

                        for (int oa = 0; oa < qr.Parameters.Count; oa++)
                        {
                            if (oa >= objHdr[i].Length) continue;
                            if (qr.Parameters[oa].ParameterName.ToLower() != "@createdon" &
                                       qr.Parameters[oa].ParameterName.ToLower() != "@createdby" &
                                        qr.Parameters[oa].ParameterName.ToLower() != "@modifiedon" &
                                        qr.Parameters[oa].ParameterName.ToLower() != "@modifiedby")
                            {
                                Logger.writeLog(qr.Parameters[oa].ParameterName + " val: " + objHdr[i][oa]);
                                switch (qr.Parameters[oa].DataType)
                                {
                                    case DbType.Boolean:
                                        if (objHdr[i][oa] == "0")
                                            qr.Parameters[oa].ParameterValue = false;
                                        else if (objHdr[i][oa] == "1")
                                            qr.Parameters[oa].ParameterValue = true;
                                        break;
                                    case DbType.Date:
                                    case DbType.DateTime:
                                        Logger.writeLog(qr.Parameters[oa].ParameterName.ToLower() + " " + objHdr[i][oa]);
                                        if (objHdr[i][oa] != null && objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = DateTime.Parse(objHdr[i][oa]);
                                        break;
                                    case DbType.Decimal:
                                    case DbType.Currency:
                                        if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Decimal.Parse(objHdr[i][oa]);
                                        break;
                                    case DbType.Double:
                                        if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Double.Parse(objHdr[i][oa]);
                                        break;
                                    case DbType.Single:
                                        if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Single.Parse(objHdr[i][oa]);
                                        break;
                                    case DbType.Guid:
                                        if (objHdr[i][oa] != string.Empty)
                                            qr.Parameters[oa].ParameterValue = new Guid(objHdr[i][oa]);
                                        break;
                                    case DbType.Int16:
                                        if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int16.Parse(objHdr[i][oa]);
                                        break;
                                    case DbType.Int32:
                                        if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int32.Parse(objHdr[i][oa]);
                                        break;
                                    case DbType.Int64:
                                        if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int64.Parse(objHdr[i][oa]);
                                        break;
                                    case DbType.UInt16:
                                        if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt16.Parse(objHdr[i][oa]);
                                        break;
                                    case DbType.UInt32:
                                        if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt32.Parse(objHdr[i][oa]);
                                        break;
                                    case DbType.UInt64:
                                        if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt64.Parse(objHdr[i][oa]);
                                        break;
                                    case DbType.StringFixedLength:
                                    case DbType.String:
                                        qr.Parameters[oa].ParameterValue = objHdr[i][oa];
                                        break;
                                    default:
                                        qr.Parameters[oa].ParameterValue = objHdr[i][oa];
                                        break;
                                }

                            }
                        }

                        col.Add(qr);
                    }

                }
            }

            return col;

        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
            throw ex;
        }
    }

    private static QueryCommandCollection FetchAppointmentRealTime(string[][] objHdr)
    {
        try
        {
            Query qry;
            Object tmpGuid;


            ArrayList duplicate = new ArrayList();

            //ORDER HEADER
            //Logger.writeLog("#records>" + objHdr.Length);

            Appointment app = new Appointment();

            QueryCommand qr = app.GetInsertCommand("SYNC");
            int UniqueIDIndex;
            UniqueIDIndex = -1;
            for (int f = 0; f < qr.Parameters.Count; f++)
            {
                if (qr.Parameters[f].ParameterName.ToLower() == "@id")
                {
                    UniqueIDIndex = f;
                    break;
                }
            }
            if (UniqueIDIndex == -1)
            {
                Logger.writeLog("Sync table does not contain unique ID or unique ID name does not compy.");
                throw new Exception("Sync table does not contain unique ID or unique ID name does not compy.");
            }

            QueryCommandCollection col = new QueryCommandCollection();
            ArrayList allList = new ArrayList();
            for (int i = 0; i < objHdr.Length; i++)
            {
                allList.Add(objHdr[i][0]);
            }

            //Check GUID uniqueness in Target DB
            qry = new Query("Appointment");
            qry.SelectList = "Id";
            DataTable dtUniqueID = qry.IN(Appointment.Columns.Id, allList).ExecuteDataSet().Tables[0];

            for (int i = 0; i < objHdr.Length; i++)
            {
                if (dtUniqueID.Rows.Count > 0)
                {
                    DataRow[] dr = dtUniqueID.Select("Id = '" + objHdr[i][0] + "'");

                    if (dr.Length > 0)
                        tmpGuid = new Guid(dr[0]["Id"].ToString());
                    else
                        tmpGuid = new Object();
                }
                else tmpGuid = new Object();

                if ((dtUniqueID.Rows.Count > 0 && tmpGuid is Guid) && ((Guid)tmpGuid != new Guid(objHdr[i][UniqueIDIndex]))) //GUID Found?
                {
                    //same order but different unique ID - Houston we have a problem                        
                    duplicate.Add(qr.Parameters[0].ParameterValue.ToString());
                }
                else
                {

                    if ((dtUniqueID.Rows.Count > 0 && tmpGuid is Guid) && (((Guid)tmpGuid).ToString() == (new Guid(objHdr[i][UniqueIDIndex])).ToString()))
                    {
                        qr = app.GetInsertCommand("SYNC");

                        Query qr2 = new Query("Appointment");
                        qr2.AddWhere(Appointment.Columns.Id, new Guid(objHdr[i][UniqueIDIndex]));
                        qr2.QueryType = QueryType.Update;

                        for (int oa = 0; oa < qr.Parameters.Count; oa++)
                        {
                            if (oa >= objHdr[i].Length) continue;
                            if (qr.Parameters[oa].ParameterName.ToLower() != "@createdon" &
                                       qr.Parameters[oa].ParameterName.ToLower() != "@createdby" &
                                        qr.Parameters[oa].ParameterName.ToLower() != "@modifiedon" &
                                        qr.Parameters[oa].ParameterName.ToLower() != "@modifiedby")
                            {
                                Logger.writeLog(qr.Parameters[oa].ParameterName + " val: " + objHdr[i][oa]);
                                switch (qr.Parameters[oa].ParameterName.ToLower())
                                {
                                    case "@starttime":
                                        if (objHdr[i][oa] != null && objHdr[i][oa] != string.Empty)
                                            qr2.AddUpdateSetting(Appointment.Columns.StartTime, DateTime.Parse(objHdr[i][oa]));
                                        break;
                                    case "@duration":
                                        if (objHdr[i][oa] != null && objHdr[i][oa] != string.Empty)
                                            qr2.AddUpdateSetting(Appointment.Columns.Duration, Int32.Parse(objHdr[i][oa]));
                                        break;
                                    case "@salespersonid":
                                        if (objHdr[i][oa] != null && objHdr[i][oa] != string.Empty)
                                            qr2.AddUpdateSetting(Appointment.Columns.SalesPersonID, objHdr[i][oa]);
                                        break;
                                    case "@checkinbywho":
                                        if (objHdr[i][oa] != null && objHdr[i][oa] != string.Empty)
                                            qr2.AddUpdateSetting(Appointment.Columns.CheckInByWho, objHdr[i][oa]);
                                        break;
                                    case "@checkintime":
                                        if (objHdr[i][oa] != null && objHdr[i][oa] != string.Empty)
                                            qr2.AddUpdateSetting(Appointment.Columns.CheckInTime, DateTime.Parse(objHdr[i][oa]));
                                        break;
                                    case "@resourceid":
                                        if (objHdr[i][oa] != null && objHdr[i][oa] != string.Empty)
                                            qr2.AddUpdateSetting(Appointment.Columns.ResourceID, objHdr[i][oa]);
                                        break;
                                    case "@isserverupdate":
                                        if (objHdr[i][oa] == "0")
                                            qr2.AddUpdateSetting(Appointment.Columns.IsServerUpdate, false);
                                        else if (objHdr[i][oa] == "1")
                                            qr2.AddUpdateSetting(Appointment.Columns.IsServerUpdate, false);
                                        break;
                                    case "@deleted":
                                        if (objHdr[i][oa] == "0")
                                            qr2.AddUpdateSetting(Appointment.Columns.Deleted, false);
                                        else if (objHdr[i][oa] == "1")
                                            qr2.AddUpdateSetting(Appointment.Columns.Deleted, true);
                                        break;
                                    case "@orderhdrid":
                                        if (objHdr[i][oa] != null && objHdr[i][oa] != string.Empty)
                                            qr2.AddUpdateSetting(Appointment.Columns.OrderHdrID, objHdr[i][oa]);
                                        break;
                                }
                            }
                        }
                        QueryCommand cmd = qr2.BuildUpdateCommand();
                        col.Add(cmd);
                        QueryCommand cmdDelete = new QueryCommand("Delete from AppointmentItem where AppointmentId = '" + tmpGuid.ToString() + "'");
                        col.Add(cmdDelete);
                    }
                    else
                    {
                        //New Item. Perform Create
                        qr = app.GetInsertCommand("SYNC");

                        for (int oa = 0; oa < qr.Parameters.Count; oa++)
                        {
                            if (oa >= objHdr[i].Length) continue;
                            if (qr.Parameters[oa].ParameterName.ToLower() != "@createdon" &
                                       qr.Parameters[oa].ParameterName.ToLower() != "@createdby" &
                                        qr.Parameters[oa].ParameterName.ToLower() != "@modifiedon" &
                                        qr.Parameters[oa].ParameterName.ToLower() != "@modifiedby")
                            {
                                Logger.writeLog(qr.Parameters[oa].ParameterName + " val: " + objHdr[i][oa]);
                                switch (qr.Parameters[oa].DataType)
                                {
                                    case DbType.Boolean:
                                        if (objHdr[i][oa] == "0")
                                            qr.Parameters[oa].ParameterValue = false;
                                        else if (objHdr[i][oa] == "1")
                                            qr.Parameters[oa].ParameterValue = true;
                                        break;
                                    case DbType.Date:
                                    case DbType.DateTime:
                                        Logger.writeLog(qr.Parameters[oa].ParameterName.ToLower() + " " + objHdr[i][oa]);
                                        if (objHdr[i][oa] != null && objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = DateTime.Parse(objHdr[i][oa]);
                                        break;
                                    case DbType.Decimal:
                                    case DbType.Currency:
                                        if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Decimal.Parse(objHdr[i][oa]);
                                        break;
                                    case DbType.Double:
                                        if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Double.Parse(objHdr[i][oa]);
                                        break;
                                    case DbType.Single:
                                        if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Single.Parse(objHdr[i][oa]);
                                        break;
                                    case DbType.Guid:
                                        if (objHdr[i][oa] != string.Empty)
                                            qr.Parameters[oa].ParameterValue = new Guid(objHdr[i][oa]);
                                        break;
                                    case DbType.Int16:
                                        if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int16.Parse(objHdr[i][oa]);
                                        break;
                                    case DbType.Int32:
                                        if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int32.Parse(objHdr[i][oa]);
                                        break;
                                    case DbType.Int64:
                                        if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int64.Parse(objHdr[i][oa]);
                                        break;
                                    case DbType.UInt16:
                                        if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt16.Parse(objHdr[i][oa]);
                                        break;
                                    case DbType.UInt32:
                                        if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt32.Parse(objHdr[i][oa]);
                                        break;
                                    case DbType.UInt64:
                                        if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt64.Parse(objHdr[i][oa]);
                                        break;
                                    case DbType.StringFixedLength:
                                    case DbType.String:
                                        qr.Parameters[oa].ParameterValue = objHdr[i][oa];
                                        break;
                                    default:
                                        qr.Parameters[oa].ParameterValue = objHdr[i][oa];
                                        break;
                                }
                                if (qr.Parameters[oa].ParameterName.ToLower().Equals("@membershipno")
                                        && qr.Parameters[oa].ParameterValue.ToString().Equals(""))
                                {
                                    qr.Parameters[oa].ParameterValue = null;
                                }
                                if (qr.Parameters[oa].ParameterName.ToLower().Equals("@isserverupdate")
                                        )
                                {
                                    qr.Parameters[oa].ParameterValue = false;
                                }
                            }
                        }

                        col.Add(qr);
                    }

                }
            }
            if (duplicate.Count != 0)
            {
                string str;
                str = "Error: Attempting to enter duplicate Appointment .";
                str = str + "\r\n" + "Duplicates:" + "\r\n";
                for (int i = 0; i < duplicate.Count; i++)
                    str += duplicate[i] + "\r\n";

                Logger.writeLog(str);
                throw new Exception("Duplicate found");
            }

            return col;

        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
            throw ex;
        }
    }

    private static QueryCommandCollection FetchAppointmentItem
        (string[][] objDet)
    {
        try
        {
            ArrayList approvedList = new ArrayList();
            QueryCommandCollection col = new QueryCommandCollection();
            Logger.writeLog("#records>" + objDet.Length);
            int UniqueIDIndex;

            AppointmentItem ord = new AppointmentItem();

            QueryCommand qr = ord.GetInsertCommand("SYNC");
            UniqueIDIndex = -1;

            for (int f = 0; f < qr.Parameters.Count; f++)
            {
                if (qr.Parameters[f].ParameterName.ToLower() == "@id")
                {
                    UniqueIDIndex = f;
                    break;
                }
            }

            if (UniqueIDIndex == -1)
            {
                Logger.writeLog("Sync table does not contain OrderHdrID.");
                throw new Exception("Sync table does not contain OrderHdrID.");
            }
            ArrayList allList = new ArrayList();
            for (int i = 0; i < objDet.Length; i++)
            {
                allList.Add(objDet[i][0]);
            }

            Object tmpGuid;
            ArrayList duplicate = new ArrayList();
            //Check GUID uniqueness in Target DB
            Query qry = new Query("AppointmentItem");
            qry.SelectList = "Id";
            DataTable dtUniqueID = qry.IN(AppointmentItem.Columns.Id, allList).ExecuteDataSet().Tables[0];

            for (int i = 0; i < objDet.Length; i++)
            {

                if (dtUniqueID.Rows.Count > 0)
                {
                    DataRow[] dr = dtUniqueID.Select("Id = '" + objDet[i][0] + "'");

                    if (dr.Length > 0)
                        tmpGuid = new Guid(dr[0]["Id"].ToString());
                    else
                        tmpGuid = new Object();
                }
                else tmpGuid = new Object();

                if (dtUniqueID.Rows.Count > 0 && tmpGuid is Guid) //GUID Found?
                {
                    //if ((Guid)tmpGuid != new Guid(objDet[i][UniqueIDIndex]))
                    //{
                    //    //same order but different unique ID - Houston we have a problem                        
                    //    duplicate.Add(qr.Parameters[0].ParameterValue.ToString());
                    //}

                    qr = ord.GetInsertCommand("SYNC");
                    for (int oa = 0; oa < qr.Parameters.Count; oa++)
                    {
                        if (oa >= objDet[i].Length) continue;
                        if (qr.Parameters[oa].ParameterName.ToLower() != "@createdon" &
                                   qr.Parameters[oa].ParameterName.ToLower() != "@createdby" &
                                    qr.Parameters[oa].ParameterName.ToLower() != "@modifiedon" &
                                    qr.Parameters[oa].ParameterName.ToLower() != "@modifiedby")
                        {
                            switch (qr.Parameters[oa].DataType)
                            {
                                case DbType.Boolean:
                                    if (objDet[i][oa] == "0")
                                        qr.Parameters[oa].ParameterValue = false;
                                    else if (objDet[i][oa] == "1")
                                        qr.Parameters[oa].ParameterValue = true;
                                    break;
                                case DbType.Date:
                                case DbType.DateTime:
                                    if (objDet[i][oa] != null && objDet[i][oa] != string.Empty)
                                        qr.Parameters[oa].ParameterValue = DateTime.Parse(objDet[i][oa]);
                                    break;
                                case DbType.Decimal:
                                case DbType.Currency:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Decimal.Parse(objDet[i][oa]);
                                    break;
                                case DbType.Double:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Double.Parse(objDet[i][oa]);
                                    break;
                                case DbType.Single:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Single.Parse(objDet[i][oa]);
                                    break;
                                case DbType.Guid:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = new Guid(objDet[i][oa]);
                                    break;
                                case DbType.Int16:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int16.Parse(objDet[i][oa]);
                                    break;
                                case DbType.Int32:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int32.Parse(objDet[i][oa]);
                                    break;
                                case DbType.Int64:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int64.Parse(objDet[i][oa]);
                                    break;
                                case DbType.UInt16:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt16.Parse(objDet[i][oa]);
                                    break;
                                case DbType.UInt32:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt32.Parse(objDet[i][oa]);
                                    break;
                                case DbType.UInt64:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt64.Parse(objDet[i][oa]);
                                    break;
                                case DbType.StringFixedLength:
                                case DbType.String:
                                    qr.Parameters[oa].ParameterValue = objDet[i][oa];
                                    break;
                                default:
                                    qr.Parameters[oa].ParameterValue = objDet[i][oa];
                                    break;
                            }
                        }
                    }
                    col.Add(qr);

                }
                else
                {
                    //It's Approved! - else ignore....
                    qr = ord.GetInsertCommand("SYNC");
                    for (int oa = 0; oa < qr.Parameters.Count; oa++)
                    {
                        if (oa >= objDet[i].Length) continue;
                        if (qr.Parameters[oa].ParameterName.ToLower() != "@createdon" &
                                   qr.Parameters[oa].ParameterName.ToLower() != "@createdby" &
                                    qr.Parameters[oa].ParameterName.ToLower() != "@modifiedon" &
                                    qr.Parameters[oa].ParameterName.ToLower() != "@modifiedby")
                        {
                            switch (qr.Parameters[oa].DataType)
                            {
                                case DbType.Boolean:
                                    if (objDet[i][oa] == "0")
                                        qr.Parameters[oa].ParameterValue = false;
                                    else if (objDet[i][oa] == "1")
                                        qr.Parameters[oa].ParameterValue = true;
                                    break;
                                case DbType.Date:
                                case DbType.DateTime:
                                    if (objDet[i][oa] != null && objDet[i][oa] != string.Empty)
                                        qr.Parameters[oa].ParameterValue = DateTime.Parse(objDet[i][oa]);
                                    break;
                                case DbType.Decimal:
                                case DbType.Currency:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Decimal.Parse(objDet[i][oa]);
                                    break;
                                case DbType.Double:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Double.Parse(objDet[i][oa]);
                                    break;
                                case DbType.Single:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Single.Parse(objDet[i][oa]);
                                    break;
                                case DbType.Guid:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = new Guid(objDet[i][oa]);
                                    break;
                                case DbType.Int16:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int16.Parse(objDet[i][oa]);
                                    break;
                                case DbType.Int32:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int32.Parse(objDet[i][oa]);
                                    break;
                                case DbType.Int64:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int64.Parse(objDet[i][oa]);
                                    break;
                                case DbType.UInt16:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt16.Parse(objDet[i][oa]);
                                    break;
                                case DbType.UInt32:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt32.Parse(objDet[i][oa]);
                                    break;
                                case DbType.UInt64:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt64.Parse(objDet[i][oa]);
                                    break;
                                case DbType.StringFixedLength:
                                case DbType.String:
                                    qr.Parameters[oa].ParameterValue = objDet[i][oa];
                                    break;
                                default:
                                    qr.Parameters[oa].ParameterValue = objDet[i][oa];
                                    break;
                            }
                        }
                    }
                    col.Add(qr);
                }
            }
            return col;
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
            throw ex;
        }
    }

    public static bool AllocatePendingPackage(out string status)
    {
        status = "";
        try
        {
            List<string> orderHdrList = new List<string>();
            OrderHdrCollection myOrderHdrs;
            #region *) Quarry: Load Outstanding OrderHdr
            string sqlOrderHdrCollector =
                "SELECT * " +
                "FROM OrderHdr " +
                "WHERE (IsPointAllocated = 0 OR IsPointAllocated IS NULL) " +
                    "AND MembershipNo IS NOT NULL AND MembershipNo <> '' AND MembershipNo <> 'WALK-IN'";
            myOrderHdrs = new OrderHdrCollection();
            myOrderHdrs.Load(DataService.GetReader(new QueryCommand(sqlOrderHdrCollector, "PowerPOS")));
            for (int i = 0; i < myOrderHdrs.Count; i++)
            {
                orderHdrList.Add(myOrderHdrs[i].OrderHdrID);
            }
            #endregion

            #region *) Delete PointAllocationLog of those outstanding OrderHdr
            string sqlDelAllocationLog = @"
                                            DELETE PointAllocationLog WHERE OrderHdrID IN 
                                            (SELECT OrderHdrID 
                                            FROM OrderHdr 
                                            WHERE (IsPointAllocated = 0 OR IsPointAllocated IS NULL) 
                                                AND MembershipNo IS NOT NULL AND MembershipNo <> '' AND MembershipNo <> 'WALK-IN')
                                         ";
            DataService.ExecuteQuery(new QueryCommand(sqlDelAllocationLog, "PowerPOS"));
            #endregion

            if (!AllocatePackage(orderHdrList.ToArray()))
                return false;
            else
                return true;
        }
        catch (Exception ex) { status = ex.Message; return false; }
    }


    public static bool FetchOrdersDataCCMW(
    string[][] dsHeaders, string[][] dsDetails,
    string[][] dsReceiptHdr, string[][] dsReceiptDet)
    {
        Logger.writeLog("Orders Synchronization started.");

        try
        {
            QueryCommandCollection qr = new QueryCommandCollection();

            qr.AddRange(FetchOrderHdr(dsHeaders));
            qr.AddRange(FetchOrderDetail(dsDetails));
            qr.AddRange(FetchReceiptHdr(dsReceiptHdr));
            qr.AddRange(FetchReceiptDet(dsReceiptDet));

            DataService.ExecuteTransaction(qr);

            Logger.writeLog("Orders Synchronization finishes.");

            //Points Allocation

            return true;
        }
        catch (Exception ex)
        {
            Logger.writeLog("Orders Synchronization failed. Please check your log file");
            Logger.writeLog(ex);
            return false;
        }
    }

    public static bool FetchCounterCloseRealTime(
    string[][] dsCounterClose)
    {
        //Logger.writeLog("Orders Synchronization started.");

        try
        {
            QueryCommandCollection qr = new QueryCommandCollection();

            qr.AddRange(FetchCounterCloseLog(dsCounterClose));

            DataService.ExecuteTransaction(qr);

            //Logger.writeLog("Orders Synchronization finishes.");
            return true;
        }
        catch (Exception ex)
        {
            //Logger.writeLog("Orders Synchronization failed. Please check your log file");
            Logger.writeLog("Closing Sync Failed." + ex.Message);
            return false;
        }
    }

    public static bool FetchCounterCloseWithDetailRealTime(
   string[][] dsCounterClose, string[][] dsCounterCloseDet)
    {
        //Logger.writeLog("Orders Synchronization started.");

        try
        {
            QueryCommandCollection qr = new QueryCommandCollection();

            qr.AddRange(FetchCounterCloseLog(dsCounterClose));
            qr.AddRange(FetchCounterCloseDet(dsCounterCloseDet));

            DataService.ExecuteTransaction(qr);

            //Logger.writeLog("Orders Synchronization finishes.");
            return true;
        }
        catch (Exception ex)
        {
            //Logger.writeLog("Orders Synchronization failed. Please check your log file");
            Logger.writeLog("Closing Sync Failed." + ex.Message);
            return false;
        }
    }


    public static bool FetchLoginActivityRealTime(
    string[][] dsLoginActivity)
    {
        //Logger.writeLog("Orders Synchronization started.");

        try
        {
            QueryCommandCollection qr = new QueryCommandCollection();

            qr.AddRange(FetchLoginActivity(dsLoginActivity));

            DataService.ExecuteTransaction(qr);

            //Logger.writeLog("Orders Synchronization finishes.");
            return true;
        }
        catch (Exception ex)
        {
            //Logger.writeLog("Orders Synchronization failed. Please check your log file");
            Logger.writeLog("Closing Sync Failed." + ex.Message);
            return false;
        }
    }

    public static bool FetchQuotationRealTime(string[][] dsQuoteHdr, string[][] dsQuoteDet)
    {
        try
        {
            QueryCommandCollection qr = new QueryCommandCollection();
            qr.AddRange(FetchQuotationHdr(dsQuoteHdr));
            qr.AddRange(FetchQuotationDet(dsQuoteDet));
            DataService.ExecuteTransaction(qr);
            return true;
        }
        catch (Exception ex)
        {
            Logger.writeLog("Quotation Sync Failed." + ex.Message);
            return false;
        }
    }

    public static bool FetchCashRecordingRealTime(string[][] dsCashRecording)
    {
        try
        {
            QueryCommandCollection qr = new QueryCommandCollection();
            qr.AddRange(FetchCashRecording(dsCashRecording));
            DataService.ExecuteTransaction(qr);
            return true;
        }
        catch (Exception ex)
        {
            Logger.writeLog("CashRecording Sync Failed." + ex.Message);
            return false;
        }
    }

    public static bool FetchPerformanceLogRealTime(string[][] dsPerformanceLog)
    {
        try
        {
            QueryCommandCollection qr = new QueryCommandCollection();
            qr.AddRange(FetchPerformanceLog(dsPerformanceLog));
            DataService.ExecuteTransaction(qr);
            return true;
        }
        catch (Exception ex)
        {
            Logger.writeLog("PerformanceLog Sync Failed." + ex.Message);
            return false;
        }
    }

    public static bool FetchPerformanceLogSummaryRealTime(string[][] dsPerformanceLogSummary)
    {
        try
        {
            QueryCommandCollection qr = new QueryCommandCollection();
            qr.AddRange(FetchPerformanceLogSummary(dsPerformanceLogSummary));
            DataService.ExecuteTransaction(qr);
            return true;
        }
        catch (Exception ex)
        {
            Logger.writeLog("PerformanceLogSummary Sync Failed." + ex.Message);
            return false;
        }
    }

    public static bool FetchOrdersDataCCMWRealTime(
    string[][] dsHeaders, string[][] dsDetails,
    string[][] dsReceiptHdr, string[][] dsReceiptDet, string[][] dsSalesComm, string[][] dsMember, string[][] dsVoidLog, string[][] dsUOMConv)
    {
        //Logger.writeLog("Orders Synchronization started.");

        try
        {
            QueryCommandCollection qr = new QueryCommandCollection();

            string[] orderHdrList;
            qr.AddRange(FetchOrderHdr(dsHeaders, out orderHdrList));

            qr.AddRange(FetchOrderDetail(dsDetails));

            qr.AddRange(FetchReceiptHdr(dsReceiptHdr));

            qr.AddRange(FetchReceiptDet(dsReceiptDet));

            qr.AddRange(FetchSalesCommission(dsSalesComm));

            qr.AddRange(FetchMembers(dsMember));

            if (dsVoidLog != null)
                qr.AddRange(FetchVoidLog(dsVoidLog));

            if (dsUOMConv != null)
                qr.AddRange(FetchOrderDetUOMConversion(dsUOMConv));

            qr.AddRange(FetchRegenerateDate(dsHeaders));

            DataService.ExecuteTransaction(qr);

            //Setting points
            AllocatePackage(orderHdrList);

            //Setting Installment
            UpdateInstallment(orderHdrList);

            //Updating Voucher Status
            UpdateVoucherStatus(orderHdrList);
            //Logger.writeLog("Orders Synchronization finishes.");

            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.UseCustomerPricing), false))
            {
                UpdateCustomerPricing(orderHdrList);
            }

            AssignAutoDeposit(orderHdrList);

            RefundPreOrder(orderHdrList);
            return true;
        }
        catch (Exception ex)
        {
            //Logger.writeLog("Orders Synchronization failed. Please check your log file");
            Logger.writeLog("Orders Synchronization failed." + ex.Message);
            return false;
        }
    }

    public static bool FetchAppointmentDataRealTime(
        string[][] dsHeaders, string[][] dsDetails, string[][] dsMembers)
    {
        Logger.writeLog("Appointment Synchronization started.");

        try
        {
            QueryCommandCollection qr = new QueryCommandCollection();
            qr.AddRange(FetchMemberFromAppointmentRealTime(dsMembers));
            qr.AddRange(FetchAppointmentRealTime(dsHeaders));
            qr.AddRange(FetchAppointmentItem(dsDetails));

            DataService.ExecuteTransaction(qr);

            Logger.writeLog("Appointment Synchronization finishes.");
            return true;
        }
        catch (Exception ex)
        {
            Logger.writeLog("Orders Synchronization failed. Please check your log file");
            Logger.writeLog(ex);
            return false;
        }
    }

    public static bool AllocatePackage(string[] orderHdrList)
    {
        try
        {
            decimal diffPoint = 0;
            decimal availablePoint = 0;
            string status;

            if (orderHdrList != null && orderHdrList.GetLength(0) > 0)
            {
                for (int i = 0; i < orderHdrList.GetLength(0); i++)
                {
                    POSController pos = new POSController(orderHdrList[i]);
                    if (pos != null && pos.GetUnsavedCustomRefNo() != "" && !pos.IsVoided() && !pos.myOrderHdr.IsPointAllocated)
                    {
                        DataTable PackageList = new DataTable();
                        if (pos.GetMemberInfo() != null && pos.GetMemberInfo().MembershipNo != "WALK-IN")
                        {


                            #region -= Points & Packages - Validation =-
                            string PercentagePointsName = AppSetting.GetSetting(AppSetting.SettingsName.Points.PercentagePointName);
                            decimal pointPercentage = 0.0M;
                            ReceiptDetCollection recDet = pos.FetchReceiptDet();
                            if (pos.GetMemberInfo() != null && pos.GetMemberInfo().MembershipGroup != null && pos.GetMemberInfo().MembershipNo != "WALK-IN")
                            {
                                pointPercentage = pos.GetMemberInfo().MembershipGroup.PointsPercentage;
                            }
                            if (!PowerPOS.Feature.Package.BreakDownPackageChangesList
                                (pos.myOrderDet, recDet, pointPercentage, out PackageList, out diffPoint, out status))
                                throw new Exception(status);

                            if (PackageList != null && PackageList.Rows.Count > 0 && !pos.MembershipApplied())
                                throw new Exception("Cannot find member to allocate point/package");
                            #endregion

                            #region Check Package Usage
                            bool isHavePackageUsage = false;
                            for (int j = 0; j < pos.myOrderDet.Count; j++)
                            {
                                if (pos.myOrderDet[j].Userflag5.GetValueOrDefault(false) && pos.myOrderDet[j].IsVoided == false)
                                {
                                    string refNo = string.IsNullOrEmpty(pos.myOrderDet[j].PointItemNo) ? pos.myOrderDet[j].ItemNo : pos.myOrderDet[j].PointItemNo;
                                    PackageList = PackageList.DeleteRow(PackageList.Columns[0].ColumnName, refNo);
                                    isHavePackageUsage = true;
                                }
                            }
                            #endregion

                            #region *) PostTransaction: Apply Points from Sales
                            bool isPointAllocated = true;
                            if (isHavePackageUsage)
                            {
                                decimal InitialPoint = 0;
                                decimal DiffPoint = 0;

                                try
                                {
                                    #region #) Core: Send data to server


                                    DataTable dt = new DataTable("PointPackage");
                                    dt.Columns.Add("RefNo", Type.GetType("System.String"));
                                    dt.Columns.Add("Amount", Type.GetType("System.Decimal"));
                                    dt.Columns.Add("PointType", Type.GetType("System.String"));

                                    foreach (OrderDet oneKey in pos.myOrderDet)
                                    {
                                        if (oneKey.Userflag5.GetValueOrDefault(false) && oneKey.IsVoided == false)
                                        {
                                            dt.Rows.Add(new object[] { string.IsNullOrEmpty(oneKey.PointItemNo) ? oneKey.ItemNo : oneKey.PointItemNo, 0 - oneKey.Quantity, Item.PointMode.Times });
                                        }
                                    }

                                    if (!PowerPOS.Feature.Package.UpdatePackageServer(dt, pos.myOrderHdr.OrderHdrID, pos.myOrderHdr.OrderDate, 0, pos.CurrentMember.MembershipNo, pos.GetSalesPerson(), UserInfo.username, out InitialPoint, out DiffPoint, out status))
                                    { Logger.writeLog(status); isPointAllocated = false; }

                                    #endregion
                                }
                                catch (System.Net.WebException X)
                                {
                                    Logger.writeLog(X);
                                    status = "Point allocation failed.";
                                    isPointAllocated = false;
                                }
                            }

                            if (PackageList != null && PackageList.Rows.Count > 0)
                            {
                                isPointAllocated = true;

                                /// If PackageList is greater 0, assumed that membership 
                                /// is applied (Look at code above)
                                try
                                {
                                    DataTable dt = new DataTable("PointPackage");
                                    dt.Columns.Add("RefNo", Type.GetType("System.String"));
                                    dt.Columns.Add("Amount", Type.GetType("System.Decimal"));
                                    dt.Columns.Add("PointType", Type.GetType("System.String"));

                                    foreach (DataRow Rw in PackageList.Rows)
                                    {
                                        string oneKey = Rw[0].ToString();
                                        string myItemNo = oneKey;
                                        //break open price package...
                                        if (oneKey.Contains("|OPP|")) //if it is open price package
                                        {
                                            myItemNo = oneKey.Substring(0, oneKey.IndexOf("|OPP|"));
                                        }
                                        Item myItem = new Item(myItemNo);
                                        string Mode = Item.PointMode.Times;
                                        if (!myItem.IsNew && myItem.IsLoaded) Mode =
                                            myItem.PointGetMode;
                                        if (oneKey == PercentagePointsName) Mode = Item.PointMode.Dollar;
                                        dt.Rows.Add(new object[] { oneKey, decimal.Parse(Rw[1].ToString()), Mode });
                                    }
                                    string validPeriodInMonth = AppSetting.GetSetting(AppSetting.SettingsName.MembershipPoint.ValidityPeriodInMonth);
                                    int validPeriod = 0;
                                    if (validPeriodInMonth != null)
                                    {
                                        int.TryParse(validPeriodInMonth, out validPeriod);
                                    }
                                    if (!PowerPOS.Feature.Package.UpdatePackageServer(dt, pos.myOrderHdr.OrderHdrID,
                                        pos.myOrderHdr.OrderDate, validPeriod, pos.CurrentMember.MembershipNo, pos.GetSalesPerson(), UserInfo.username, out availablePoint, out diffPoint, out status))
                                    { Logger.writeLog(status); isPointAllocated = false; }
                                }
                                catch (System.Net.WebException X)
                                {
                                    Logger.writeLog(X);
                                    status = "Point allocation failed.";
                                    isPointAllocated = false;
                                }

                            }
                            else
                            {
                                isPointAllocated = true;
                            }
                            #endregion

                            #region *) PostTransaction: Set IsPointAllocated as TRUE on OrderHdr
                            if (isPointAllocated)
                            {
                                string sql = "UPDATE OrderHdr SET {0} = {1}, {2} = {3}, IsPointAllocated = 1 WHERE OrderHdrID = '{4}'";
                                sql = string.Format(sql, OrderHdr.UserColumns.InitialPoint, availablePoint,
                                                    OrderHdr.UserColumns.PointAmount, diffPoint,
                                                    pos.myOrderHdr.OrderHdrID);
                                DataService.ExecuteQuery(new QueryCommand(sql, "PowerPOS"));
                            }
                            #endregion

                            string sqlQuery1 = "Select max(modifiedon) from membershipPoints where membershipno = '" + pos.GetMemberInfo().MembershipNo + "'";
                            if (pos.MembershipApplied())
                            {
                                object maxModifiedOn = DataService.ExecuteScalar(new QueryCommand(sqlQuery1));
                                DateTime testMo = DateTime.Now;
                                if (DateTime.TryParse(maxModifiedOn.ToString(), out testMo))
                                {
                                    Logger.writeLog("After Update Points. " + testMo.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                                }
                            }
                        }

                    }
                    else
                    {
                        //revert package
                        if (pos.IsVoided())
                        {
                            string sts = "";
                            if (!PowerPOS.Feature.Package.RevertPackageServer(pos.myOrderHdr.OrderHdrID, DateTime.Now, pos.myOrderHdr.MembershipNo, "VOID [" + UserInfo.username + "]", UserInfo.username, out sts))
                            {
                                //if failed prompt message box???? 
                                //TODO: Void should not pop message box. Handle this more gracefully
                                Logger.writeLog("Error Revert Points From Order " + pos.myOrderHdr.OrderHdrID + ". " + sts);
                                return false;
                            }
                        }
                    }

                }
            }

            return true;
        }
        catch (Exception ex) { Logger.writeLog("Allocate Point Failed. " + ex.Message); return false; }
    }

    public static bool UpdateInstallment(string[] orderHdrList)
    {
        try
        {
            if (orderHdrList != null && orderHdrList.GetLength(0) > 0)
            {
                string status;
                for (int o = 0; o < orderHdrList.GetLength(0); o++)
                {
                    InstallmentController.UpdateInstallmentByOrderHdr(orderHdrList[o], out status);

                    #region *) OBSOLETE (MOVED TO InstallmentController.Custom.cs)
                    #endregion
                }
            }

            return true;
        }
        catch (Exception ex) { Logger.writeLog("Update Installment Balance failed. " + ex.Message); return false; }
    }

    public static bool UpdateVoucherStatus(string[] orderHdrList)
    {
        try
        {
            string status;

            if (orderHdrList != null && orderHdrList.GetLength(0) > 0)
            {
                for (int o = 0; o < orderHdrList.GetLength(0); o++)
                {
                    POSController pos = new POSController(orderHdrList[o]);
                    if (pos != null && pos.GetUnsavedCustomRefNo() != "" && !pos.IsVoided())
                    {
                        QueryCommandCollection cmdColl = new QueryCommandCollection();
                        OrderDetCollection myOrderDet = pos.myOrderDet;
                        for (int i = 0; i < myOrderDet.Count; i++)
                        {
                            if (myOrderDet[i].IsVoided) continue;
                            if (myOrderDet[i].ItemNo == POSController.VOUCHER_BARCODE)
                            {
                                //update voucher id....
                                Query qr = Voucher.CreateQuery();
                                qr.QueryType = QueryType.Update;
                                qr.AddWhere(Voucher.Columns.VoucherNo, myOrderDet[i].VoucherNo);
                                if (myOrderDet[i].Quantity > 0)
                                {
                                    qr.AddWhere(Voucher.Columns.VoucherStatusId, VoucherController.VOUCHER_PRINTED);
                                    qr.AddUpdateSetting(Voucher.Columns.VoucherStatusId, VoucherController.VOUCHER_SOLD);
                                    qr.AddUpdateSetting(Voucher.Columns.DateSold, pos.myOrderHdr.OrderDate);
                                    qr.AddUpdateSetting(Voucher.Columns.ModifiedOn, DateTime.Now);
                                    qr.AddUpdateSetting(Voucher.Columns.ModifiedBy, "SYNC");
                                    cmdColl.Add(qr.BuildUpdateCommand());

                                    // Update SoldQuantity in VoucherHeader
                                    string sql = @"
                                                    UPDATE VoucherHeader 
                                                    SET SoldQuantity = (SELECT COUNT(*) FROM Vouchers WHERE VoucherHeaderID = vh.VoucherHeaderID AND DateSold IS NOT NULL), 
                                                        ModifiedOn = GETDATE()
                                                    FROM VoucherHeader vh 
                                                        INNER JOIN Vouchers vs ON vs.VoucherHeaderID = vh.VoucherHeaderID
                                                    WHERE vs.VoucherNo = '{0}'
                                                  ";
                                    sql = string.Format(sql, myOrderDet[i].VoucherNo);
                                    cmdColl.Add(new QueryCommand(sql, "PowerPOS"));
                                }
                                else
                                {
                                    qr.AddWhere(Voucher.Columns.VoucherStatusId, VoucherController.VOUCHER_SOLD);
                                    qr.AddUpdateSetting(Voucher.Columns.VoucherStatusId, VoucherController.VOUCHER_REDEEMED);
                                    qr.AddUpdateSetting(Voucher.Columns.DateRedeemed, pos.myOrderHdr.OrderDate);
                                    qr.AddUpdateSetting(Voucher.Columns.ModifiedOn, DateTime.Now);
                                    qr.AddUpdateSetting(Voucher.Columns.ModifiedBy, "SYNC");
                                    cmdColl.Add(qr.BuildUpdateCommand());

                                    // Update RedeemedQuantity in VoucherHeader
                                    string sql = @"
                                                    UPDATE VoucherHeader 
                                                    SET RedeemedQuantity = (SELECT COUNT(*) FROM Vouchers WHERE VoucherHeaderID = vh.VoucherHeaderID AND DateRedeemed IS NOT NULL), 
                                                        ModifiedOn = GETDATE()
                                                    FROM VoucherHeader vh 
                                                        INNER JOIN Vouchers vs ON vs.VoucherHeaderID = vh.VoucherHeaderID
                                                    WHERE vs.VoucherNo = '{0}'
                                                  ";
                                    sql = string.Format(sql, myOrderDet[i].VoucherNo);
                                    cmdColl.Add(new QueryCommand(sql, "PowerPOS"));
                                }
                            }
                        }

                        if (cmdColl.Count > 0) DataService.ExecuteTransaction(cmdColl);
                    }
                    else
                    {
                        //revert Voucher status
                        if (pos.IsVoided())
                        {
                            QueryCommandCollection cmdColl = new QueryCommandCollection();
                            OrderDetCollection myOrderDet = pos.myOrderDet;
                            for (int i = 0; i < myOrderDet.Count; i++)
                            {
                                if (myOrderDet[i].IsVoided) continue;
                                if (myOrderDet[i].ItemNo == POSController.VOUCHER_BARCODE)
                                {
                                    //update voucher id....
                                    Query qr = Voucher.CreateQuery();
                                    qr.QueryType = QueryType.Update;
                                    qr.AddWhere(Voucher.Columns.VoucherNo, myOrderDet[i].VoucherNo);
                                    if (myOrderDet[i].Quantity > 0)
                                    {
                                        qr.AddWhere(Voucher.Columns.VoucherStatusId, VoucherController.VOUCHER_SOLD);
                                        qr.AddUpdateSetting(Voucher.Columns.VoucherStatusId, VoucherController.VOUCHER_PRINTED);
                                        qr.AddUpdateSetting(Voucher.Columns.DateSold, DBNull.Value);
                                        qr.AddUpdateSetting(Voucher.Columns.ModifiedOn, DateTime.Now);
                                        qr.AddUpdateSetting(Voucher.Columns.ModifiedBy, "SYNC");
                                        cmdColl.Add(qr.BuildUpdateCommand());

                                        // Update SoldQuantity in VoucherHeader
                                        string sql = @"
                                                    UPDATE VoucherHeader 
                                                    SET SoldQuantity = (SELECT COUNT(*) FROM Vouchers WHERE VoucherHeaderID = vh.VoucherHeaderID AND DateSold IS NOT NULL), 
                                                        ModifiedOn = GETDATE()
                                                    FROM VoucherHeader vh 
                                                        INNER JOIN Vouchers vs ON vs.VoucherHeaderID = vh.VoucherHeaderID
                                                    WHERE vs.VoucherNo = '{0}'
                                                  ";
                                        sql = string.Format(sql, myOrderDet[i].VoucherNo);
                                        cmdColl.Add(new QueryCommand(sql, "PowerPOS"));
                                    }
                                    else
                                    {
                                        qr.AddWhere(Voucher.Columns.VoucherStatusId, VoucherController.VOUCHER_REDEEMED);
                                        qr.AddUpdateSetting(Voucher.Columns.VoucherStatusId, VoucherController.VOUCHER_SOLD);
                                        qr.AddUpdateSetting(Voucher.Columns.DateRedeemed, DBNull.Value);
                                        qr.AddUpdateSetting(Voucher.Columns.ModifiedOn, DateTime.Now);
                                        qr.AddUpdateSetting(Voucher.Columns.ModifiedBy, "SYNC");
                                        cmdColl.Add(qr.BuildUpdateCommand());

                                        // Update RedeemedQuantity in VoucherHeader
                                        string sql = @"
                                                    UPDATE VoucherHeader 
                                                    SET RedeemedQuantity = (SELECT COUNT(*) FROM Vouchers WHERE VoucherHeaderID = vh.VoucherHeaderID AND DateRedeemed IS NOT NULL), 
                                                        ModifiedOn = GETDATE()
                                                    FROM VoucherHeader vh 
                                                        INNER JOIN Vouchers vs ON vs.VoucherHeaderID = vh.VoucherHeaderID
                                                    WHERE vs.VoucherNo = '{0}'
                                                  ";
                                        sql = string.Format(sql, myOrderDet[i].VoucherNo);
                                        cmdColl.Add(new QueryCommand(sql, "PowerPOS"));
                                    }
                                }
                            }

                            if (cmdColl.Count > 0) DataService.ExecuteTransaction(cmdColl);
                        }
                    }
                }
            }

            return true;
        }
        catch (Exception ex) { Logger.writeLog("Update Voucher Status failed. " + ex.Message); return false; }
    }

    public static bool UpdateCustomerPricing(string[] orderHdrList)
    {
        try
        {
            string status;

            if (orderHdrList != null && orderHdrList.GetLength(0) > 0)
            {
                for (int o = 0; o < orderHdrList.GetLength(0); o++)
                {
                    POSController pos = new POSController(orderHdrList[o]);
                    if (pos != null && pos.GetUnsavedCustomRefNo() != "" && pos.myOrderHdr.MembershipNo != "WALK-IN" && !pos.IsVoided())
                    {
                        string MembershipNo = pos.myOrderHdr.MembershipNo;
                        int PointOfSaleID = pos.myOrderHdr.PointOfSaleID;
                        QueryCommandCollection cmdColl = new QueryCommandCollection();
                        OrderDetCollection myOrderDet = pos.myOrderDet;
                        for (int i = 0; i < myOrderDet.Count; i++)
                        {
                            if (myOrderDet[i].IsVoided) continue;
                            if (myOrderDet[i].Quantity <= 0) continue;
                            if (myOrderDet[i].Item.CategoryName == "SYSTEM") continue;
                            //check if using outlet price then update outlet itempricing
                            string query = @"                                                         
                                SELECT ItemNo, OutletName, PointOfSaleID
                                FROM
                                (
	                                select ogm.ItemNo, o.OutletName ,p.PointofSaleID, p.PointOfSaleName 
	                                from outletgroupitemmap ogm 
	                                inner join Item i on ogm.ItemNo = i.ItemNo and ISNULL(i.Deleted,0) = 0 
	                                left join outlet o on o.OutletName = ogm.OutletName and ISNULL(o.Deleted,0) = 0
	                                left join pointofsale p on p.OutletName = o.OutletName and ISNULL(p.Deleted,0) = 0
	                                where ISNULL(ogm.IsItemDeleted, 0) = 0
	                                union 
	                                select ogm.ItemNo, o.OutletName, p.PointofSaleID, p.PointOfSaleName 
	                                from outletgroupitemmap ogm 
	                                inner join Item i on ogm.ItemNo = i.ItemNo and ISNULL(i.Deleted,0) = 0 
	                                left join outlet o on o.outletgroupID = ogm.OutletGroupID and ISNULL(o.Deleted,0) = 0
	                                left join pointofsale p on p.OutletName = o.OutletName and ISNULL(p.Deleted,0) = 0
	                                where ISNULL(ogm.IsItemDeleted, 0) = 0
                                )ox
                                where ItemNo = '{0}' and PointOfSaleID = {1} ";
                            query = String.Format(query, myOrderDet[i].ItemNo, PointOfSaleID);

                            DataTable dt = DataService.GetDataSet(new QueryCommand(query)).Tables[0];

                            string ItemNo = myOrderDet[i].ItemNo;
                            //decimal theAmount = myOrderDet[i].Item.GSTRule == 1 ? myOrderDet[i].Amount - myOrderDet[i].GSTAmount.GetValueOrDefault(0) : myOrderDet[i].Amount;
                            //decimal NewPrice = theAmount / (myOrderDet[i].Quantity ?? 1);
                            decimal NewPrice = myOrderDet[i].UnitPrice;

                            if (dt.Rows.Count > 0)
                            {
                                //update outpetcustomerpricring;
                                OutletCustomerPricing oul = new OutletCustomerPricing();
                                string OutletName = dt.Rows[0]["OutletName"].ToString();
                                OutletCustomerPricingCollection col = new OutletCustomerPricingCollection();
                                col.Where(OutletCustomerPricing.Columns.MembershipNo, MembershipNo);
                                col.Where(OutletCustomerPricing.Columns.OutletName, OutletName);
                                col.Where(OutletCustomerPricing.Columns.ItemNo, ItemNo);
                                col.Load();

                                if (col.Count > 0)
                                {
                                    oul = col[0];
                                }

                                oul.MembershipNo = MembershipNo;
                                oul.OutletName = OutletName;
                                oul.RetailPrice = NewPrice;
                                oul.ItemNo = ItemNo;
                                oul.Deleted = false;

                                if (oul.IsNew)
                                    cmdColl.Add(oul.GetInsertCommand("SYNC"));
                                else
                                    cmdColl.Add(oul.GetUpdateCommand("SYNC"));
                            }
                            else
                            {
                                CustomerPricing oul = new CustomerPricing();

                                CustomerPricingCollection col = new CustomerPricingCollection();
                                col.Where(CustomerPricing.Columns.MembershipNo, MembershipNo);
                                col.Where(CustomerPricing.Columns.ItemNo, ItemNo);
                                col.Load();

                                if (col.Count > 0)
                                {
                                    oul = col[0];
                                }

                                oul.MembershipNo = MembershipNo;
                                oul.RetailPrice = NewPrice;
                                oul.ItemNo = ItemNo;
                                oul.Deleted = false;

                                if (oul.IsNew)
                                    cmdColl.Add(oul.GetInsertCommand("SYNC"));
                                else
                                    cmdColl.Add(oul.GetUpdateCommand("SYNC"));
                            }

                        }

                        if (cmdColl.Count > 0) DataService.ExecuteTransaction(cmdColl);
                    }
                }
            }

            return true;
        }
        catch (Exception ex) { Logger.writeLog("Update Customer Price failed. " + ex.Message); return false; }
    }

    public static bool AssignAutoDeposit(string[] orderHdrList)
    {
        try
        {
            /*if setting show pop up preorder and C&C disable but have an item pre order*/

            string status = "";

            if (orderHdrList != null && orderHdrList.GetLength(0) > 0)
            {
                for (int o = 0; o < orderHdrList.GetLength(0); o++)
                {
                    PreOrderController.AssignAutoDeposit(orderHdrList[o], out status);
                }
            }

            return true;
        }
        catch (Exception ex) { Logger.writeLog("Update Assign Auto Deposit failed. " + ex.Message); return false; }
    }

    public static bool RefundPreOrder(string[] orderHdrList)
    {
        try
        {
            /*if setting show pop up preorder and C&C disable but have an item pre order*/

            string status = "";

            if (orderHdrList != null && orderHdrList.GetLength(0) > 0)
            {
                for (int o = 0; o < orderHdrList.GetLength(0); o++)
                {
                    PreOrderController.RefundPreOrder(orderHdrList[o], out status);
                }
            }

            return true;
        }
        catch (Exception ex) { Logger.writeLog("Update refund failed. " + ex.Message); return false; }
    }

    public static bool FetchAccessLogRealTime(string[][] accessLog)
    {
        bool isSuccess = false;
        try
        {
            QueryCommandCollection qr = new QueryCommandCollection();
            qr.AddRange(FetchAccessLog(accessLog));
            DataService.ExecuteTransaction(qr);
            isSuccess = true;
        }
        catch (Exception ex)
        {
            isSuccess = false;
            Logger.writeLog(ex);
        }
        return isSuccess;
    }

    public static bool FetchAttendanceRealTime(string[][] attendance)
    {
        bool isSuccess = false;
        try
        {
            QueryCommandCollection qr = new QueryCommandCollection();
            qr.AddRange(FetchAttendance(attendance));
            DataService.ExecuteTransaction(qr);
            isSuccess = true;
        }
        catch (Exception ex)
        {
            isSuccess = false;
            Logger.writeLog(ex);
        }
        return isSuccess;
    }

    private static void FetchOrderHdr(DataSet dsHeaders)
    {
        try
        {
            Query qry;
            Object tmpGuid;

            ArrayList duplicate = new ArrayList();

            OrderHdrCollection myHdr = new OrderHdrCollection();
            myHdr.Load(dsHeaders.Tables[0]);

            //Translate from DataSet to actual database

            //ORDER HEADER
            for (int i = 0; i < myHdr.Count; i++)
            {


                //Get GUID - This is needed due to bug with SubSonic
                myHdr[i].UniqueID = new Guid(
                    dsHeaders.Tables[0].
                    Select("OrderHdrID = '" + myHdr[i].OrderHdrID + "'")[0]["UniqueID"].ToString());

                //Check GUID uniqueness in Target DB
                qry = new Query("OrderHdr");
                qry.SelectList = "UniqueID";
                tmpGuid = qry.WHERE("OrderHdrID", myHdr[i].OrderHdrID).ExecuteScalar();

                if (tmpGuid is Guid) //GUID Found?
                {
                    if ((Guid)tmpGuid == myHdr[i].UniqueID)
                    {
                        //same Order ID and unique ID - no problem, do update....                        
                        //myHdr[i].Save("SYNC");             
                    }
                    else
                    {
                        //same order but different unique ID - Houston we have a problem                        
                        duplicate.Add(myHdr[i].OrderRefNo);
                    }
                }
                else
                {
                    myHdr[i].IsNew = true;
                    Logger.writeLog("OH>" + myHdr[i].OrderHdrID + " " + myHdr[i].OrderDate.ToString());
                    myHdr[i].Save("SYNC");
                }
            }
            if (duplicate.Count != 0)
            {
                string str;
                str = "Error: Attempting to enter duplicate Order Header Ref Number.";
                str = str + "\r\n" + "Duplicates:" + "\r\n";
                for (int i = 0; i < duplicate.Count; i++)
                    str += duplicate[i] + "\r\n";

                Logger.writeLog(str);
                throw new Exception("Duplicate found");
            }
            return;
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
            throw ex;
        }
    }
    private static void FetchOrderDetail(DataSet dsDetails)
    {
        try
        {
            Query qry;
            Object tmpGuid;

            ArrayList duplicate = new ArrayList();
            OrderDetCollection myDet = new OrderDetCollection();
            myDet.Load(dsDetails.Tables[0]);

            qry = new Query("OrderDet");
            //Translate from DataSet to actual database
            for (int i = 0; i < myDet.Count; i++)
            {
                //Get count....                    
                myDet[i].UniqueID = new Guid(
                dsDetails.Tables[0].
                Select("OrderDetID = '" + myDet[i].OrderDetID + "'")[0]["UniqueID"].ToString());

                //Check GUID uniqueness in Target DB
                qry = new Query("OrderDet");
                qry.SelectList = "UniqueID";
                tmpGuid = qry.WHERE("OrderDetID", myDet[i].OrderDetID).ExecuteScalar();

                if (tmpGuid is Guid) //GUID Found?
                {
                    if ((Guid)tmpGuid == myDet[i].UniqueID)
                    {
                        //same Order ID and unique ID - no problem, do update....                        
                        //myDet[i].Save("SYNC");
                    }
                    else
                    {
                        //same order but different unique ID - Houston we have a problem                        
                        duplicate.Add(myDet[i].OrderDetID);
                    }
                }
                else
                {
                    myDet[i].IsNew = true;
                    Logger.writeLog("OD>" + myDet[i].OrderDetID + " " + myDet[i].ItemNo.ToString());
                    myDet[i].Save("SYNC");
                }
            }

            if (duplicate.Count != 0)
            {
                string str;
                str = "Error: Attempting to enter duplicate Order Number. Check if two PointOfSale are assigned the same PointOfSale ID.";
                str = str + "\r\n" + "Duplicates ID:" + "\r\n";
                for (int i = 0; i < duplicate.Count; i++)
                    str += duplicate[i] + "\r\n";


                Logger.writeLog(str);
                throw new Exception("Duplicate found");
            }
            return;
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
            throw ex;
        }
    }
    private static void FetchReceiptHdr(DataSet dsHeaders)
    {
        try
        {
            Query qry;
            Object tmpGuid;

            ArrayList duplicate = new ArrayList();

            ReceiptHdrCollection myHdr = new ReceiptHdrCollection();
            myHdr.Load(dsHeaders.Tables[0]);

            //Translate from DataSet to actual database

            //ORDER HEADER
            for (int i = 0; i < myHdr.Count; i++)
            {
                //Get GUID - This is needed due to bug with SubSonic
                myHdr[i].UniqueID = new Guid(
                    dsHeaders.Tables[0].
                    Select("ReceiptHdrID = '" + myHdr[i].ReceiptHdrID + "'")[0]["UniqueID"].ToString());

                //Check GUID uniqueness in Target DB
                qry = new Query("ReceiptHdr");
                qry.SelectList = "UniqueID";
                tmpGuid = qry.WHERE("ReceiptHdrID", myHdr[i].ReceiptHdrID).ExecuteScalar();

                if (tmpGuid is Guid) //GUID Found?
                {
                    if ((Guid)tmpGuid == myHdr[i].UniqueID)
                    {
                        //myHdr[i].Save("SYNC");
                    }
                    else
                    {
                        //same order but different unique ID - Houston we have a problem                        
                        duplicate.Add(myHdr[i].ReceiptRefNo);
                    }
                }
                else
                {
                    myHdr[i].IsNew = true;
                    Logger.writeLog("RH>" + myHdr[i].ReceiptHdrID);
                    myHdr[i].Save("SYNC");
                }
            }
            if (duplicate.Count != 0)
            {
                string str;
                str = "Error: Attempting to enter duplicate Receipt Header Ref Number.";
                str = str + "\r\n" + "Duplicates:" + "\r\n";
                for (int i = 0; i < duplicate.Count; i++)
                    str += duplicate[i] + "\r\n";


                Logger.writeLog(str);
                throw new Exception("Duplicate found");
            }
            return;
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
            throw ex;
        }
    }
    private static void FetchReceiptDet(DataSet dsDetails)
    {
        try
        {
            Query qry;
            Object tmpGuid;

            ArrayList duplicate = new ArrayList();

            ReceiptDetCollection myDet = new ReceiptDetCollection();
            myDet.Load(dsDetails.Tables[0]);

            //Translate from DataSet to actual database

            //Receipt HEADER
            for (int i = 0; i < myDet.Count; i++)
            {
                //Get GUID - This is needed due to bug with SubSonic
                myDet[i].UniqueID = new Guid(
                    dsDetails.Tables[0].
                    Select("ReceiptDetID = '" + myDet[i].ReceiptDetID + "'")[0]["UniqueID"].ToString());

                //Check GUID uniqueness in Target DB
                qry = new Query("ReceiptDet");
                qry.SelectList = "UniqueID";
                tmpGuid = qry.WHERE("ReceiptDetID", myDet[i].ReceiptDetID).ExecuteScalar();

                if (tmpGuid is Guid) //GUID Found?
                {
                    if ((Guid)tmpGuid == myDet[i].UniqueID)
                    {
                        //myDet[i].Save("SYNC");
                    }
                    else
                    {
                        //same order but different unique ID - Houston we have a problem                        
                        duplicate.Add(myDet[i].ReceiptDetID);
                    }
                }
                else
                {
                    Logger.writeLog("RD>" + myDet[i].ReceiptDetID + " " + myDet[i].PaymentType + " " + myDet[i].Amount);
                    myDet[i].IsNew = true;
                    myDet[i].Save("SYNC");
                }
            }
            if (duplicate.Count != 0)
            {
                string str;
                str = "Error: Attempting to enter duplicate Receipt Det ID.";
                str = str + "\r\n" + "Duplicates:" + "\r\n";
                for (int i = 0; i < duplicate.Count; i++)
                    str += duplicate[i] + "\r\n";
                Logger.writeLog(str);
                throw new Exception("Duplicate found.");
            }
            return;
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
            throw ex;
        }
    }
    private static void FetchSalesCommissionRecord(DataSet dsComm)
    {
        try
        {
            Query qry;
            Object tmpGuid;

            ArrayList duplicate = new ArrayList();
            SalesCommissionRecordCollection comm = new SalesCommissionRecordCollection();
            comm.Load(dsComm.Tables[0]);

            qry = new Query("SalesCommissionRecord");
            //Translate from DataSet to actual database
            for (int i = 0; i < comm.Count; i++)
            {
                //Get count....                    
                comm[i].UniqueID = new Guid(
                dsComm.Tables[0].
                Select("OrderHdrID = '" + comm[i].OrderHdrID + "'")[0]["UniqueID"].ToString());

                //Check GUID uniqueness in Target DB
                qry = new Query("SalesCommissionRecord");
                qry.SelectList = "UniqueID";
                tmpGuid = qry.WHERE("OrderHdrID", comm[i].OrderHdrID).ExecuteScalar();

                if (tmpGuid is Guid) //GUID Found?
                {
                    if ((Guid)tmpGuid != comm[i].UniqueID)
                        //same order but different unique ID - Houston we have a problem                        
                        duplicate.Add(comm[i].OrderHdrID);
                }
                else
                {
                    Logger.writeLog("SC>" + comm[i].OrderHdrID + " " + comm[i].SalesPersonID);
                    comm[i].IsNew = true;
                    comm[i].Save("SYNC");
                }
            }

            if (duplicate.Count != 0)
            {
                string str;
                str = "Error: Attempting to enter duplicate commission sales ID. Check if two PointOfSale are assigned the same PointOfSale ID.";
                str = str + "\r\n" + "Duplicates ID:" + "\r\n";
                for (int i = 0; i < duplicate.Count; i++)
                    str += duplicate[i] + "\r\n";


                Logger.writeLog(str);
                throw new Exception("Duplicate found");
            }
            return;
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
            throw ex;
        }
    }
    public static bool FetchOrdersData
        (DataSet dsHeaders, DataSet dsDetails,
        DataSet dsReceiptHdr, DataSet dsReceiptDet, DataSet dsComm)
    {
        Logger.writeLog("Orders Synchronization started.");
        //using (SharedDbConnectionScope sharedConnectionScope = new SharedDbConnectionScope())
        using (TransactionScope ts = new TransactionScope())
        {
            try
            {
                FetchOrderHdr(dsHeaders);
                FetchOrderDetail(dsDetails);
                FetchSalesCommissionRecord(dsComm);
                FetchReceiptHdr(dsReceiptHdr);
                FetchReceiptDet(dsReceiptDet);
                ts.Complete();

                Logger.writeLog("Orders Synchronization finishes.");
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog("Orders Synchronization failed. Please check your log file");
                Logger.writeLog(ex);
                ts.Dispose();
                return false;
            }
        }
    }
    public static bool FetchNewMembershipSignUpsData(DataSet dsMember)
    {
        Logger.writeLog("New Membership Signup Synchronization started.");
        //using (SharedDbConnectionScope sharedConnectionScope = new SharedDbConnectionScope())
        //using (TransactionScope ts = new TransactionScope())
        {
            QueryCommandCollection cmd = new QueryCommandCollection();
            try
            {
                Query qry;
                Object tmpGuid;

                ArrayList duplicate = new ArrayList();

                MembershipCollection myMember
                    = new MembershipCollection();
                myMember.Load(dsMember.Tables[0]);

                //Translate from DataSet to actual database                
                //MEMBERSHIP
                for (int i = 0; i < myMember.Count; i++)
                {
                    //Get GUID - This is needed due to bug with SubSonic
                    myMember[i].UniqueID = new Guid(
                        dsMember.Tables[0].
                        Select("MembershipNo = '" +
                        myMember[i].MembershipNo + "'")
                        [0]["UniqueID"].ToString());

                    //Check GUID uniqueness in Target DB
                    qry = new Query("Membership");
                    qry.SelectList = "UniqueID";
                    tmpGuid = qry.WHERE
                        ("MembershipNo",
                        myMember[i].MembershipNo).ExecuteScalar();

                    if (tmpGuid is Guid) //GUID Found?
                    {
                        if ((Guid)tmpGuid == myMember[i].UniqueID)
                        {
                        }
                        else
                        {
                            //same order but different unique ID - Houston we have a problem  
                            duplicate.Add(myMember[i].MembershipNo);
                        }
                    }
                    else
                    {
                        Logger.writeLog("MM>" + myMember[i].MembershipNo);
                        myMember[i].IsNew = true;
                        cmd.Add(myMember[i].GetSaveCommand("SYNC"));
                    }
                }
                if (duplicate.Count != 0)
                {
                    string str;
                    str = "Error: Attempting to enter duplicate Order Header Ref Number.";
                    str = str + "\r\n" + "Duplicates:" + "\r\n";
                    for (int i = 0; i < duplicate.Count; i++)
                        str += duplicate[i] + "\r\n";

                    Logger.writeLog(str);
                    throw new Exception("Duplicate found");
                }

                #region AttachedParticular
                // Only if there is AttachedParticular table in the dataset
                if (dsMember.Tables.Contains("AttachedParticular"))
                {
                    duplicate = new ArrayList();

                    AttachedParticularCollection myAP = new AttachedParticularCollection();
                    myAP.Load(dsMember.Tables["AttachedParticular"]);

                    //Translate from DataSet to actual database                
                    //AttachedParticular
                    for (int i = 0; i < myAP.Count; i++)
                    {
                        //Get GUID - This is needed due to bug with SubSonic
                        myAP[i].UniqueID = new Guid(
                            dsMember.Tables["AttachedParticular"].
                            Select("AttachedParticularID = '" +
                            myAP[i].AttachedParticularID + "'")
                            [0]["UniqueID"].ToString());

                        //Check GUID uniqueness in Target DB
                        qry = new Query("AttachedParticular");
                        qry.SelectList = "UniqueID";
                        tmpGuid = qry.WHERE
                            ("AttachedParticularID",
                            myAP[i].AttachedParticularID).ExecuteScalar();

                        if (tmpGuid is Guid) //GUID Found?
                        {
                            if ((Guid)tmpGuid == myAP[i].UniqueID)
                            {
                            }
                            else
                            {
                                //same order but different unique ID - Houston we have a problem  
                                duplicate.Add(myAP[i].AttachedParticularID);
                            }
                        }
                        else
                        {
                            Logger.writeLog("MM_AP>" + myAP[i].AttachedParticularID);
                            myAP[i].IsNew = true;
                            cmd.Add(myAP[i].GetSaveCommand("SYNC"));
                        }
                    }
                    if (duplicate.Count != 0)
                    {
                        string str;
                        str = "Error: Attempting to enter duplicate Attached Particular ID.";
                        str = str + "\r\n" + "Duplicates:" + "\r\n";
                        for (int i = 0; i < duplicate.Count; i++)
                            str += duplicate[i] + "\r\n";

                        Logger.writeLog(str);
                        throw new Exception("Duplicate found");
                    }

                }
                #endregion

                DataService.ExecuteTransaction(cmd);
                //ts.Complete();
                Logger.writeLog("New Membership Signup Synchronization finishes.");
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog("New Membership Signup Synchronization failed. Please check your log file");
                Logger.writeLog(ex);
                //ts.Dispose();
                return false;
            }
        }
    }
    /// <summary>
    /// Save modified membership remarks. (created by John Harries)
    /// </summary>
    /// <param name="dsMember">Modified membership dataset.</param>
    /// <returns>True for success process and false for unsuccess process.</returns>
    public static bool FetchModifiedMembershipRemarks(DataSet dsMember)
    {
        try
        {
            MembershipCollection memberColl = new MembershipCollection();
            memberColl.Load(dsMember.Tables["Membership"]);

            QueryCommandCollection cmdColl = new QueryCommandCollection();
            foreach (var item in memberColl)
            {
                var query = string.Format("update Membership set Remarks = '{0}' where MembershipNo = '{1}'", item.Remarks, item.MembershipNo);
                cmdColl.Add(new QueryCommand(query, "PowerPOS"));
                cmdColl.Add(item.GetSaveCommand("SYNC"));
            }

            DataService.ExecuteTransaction(cmdColl);

            Logger.writeLog("Modify membership remarks synchronization finished.");
            return true;
        }
        catch (Exception ex)
        {
            Logger.writeLog("Modify membership remarks synchronization failed. Please check your log file");
            Logger.writeLog(ex);
            return false;
        }
    }
    public static bool FetchLogTable(DataSet dsLogTable, string logTableName)
    {
        CultureInfo ct = new CultureInfo("");
        DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
        dtFormat.ShortDatePattern = "yyyy-MM-dd";
        ct.DateTimeFormat = dtFormat;
        System.Threading.Thread.CurrentThread.CurrentCulture = ct;
        System.Threading.Thread.CurrentThread.CurrentUICulture = ct;

        try
        {
            Query qry;
            Guid tmpGuid;

            qry = new Query(logTableName);
            ArrayList Columns = new ArrayList();
            ArrayList Parameters = new ArrayList();
            Logger.writeLog("Fetch:" + logTableName);

            int startCol = 0;
            if (DataService.GetSchema(logTableName, "PowerPOS").PrimaryKey.AutoIncrement)
            {
                startCol = 1;
            }

            if (DataService.GetSchema(logTableName, "PowerPOS").PrimaryKey.DataType == DbType.Guid)
            {
                int f = 0;
                f = f + 1;
            }
            string ColumnList = "", ParameterList = "";
            for (int i = startCol; i < dsLogTable.Tables[0].Columns.Count; i++)
            {
                if (i == dsLogTable.Tables[0].Columns.Count - 1)
                {
                    ColumnList += dsLogTable.Tables[0].Columns[i].ColumnName;
                    ParameterList += "@" + dsLogTable.Tables[0].Columns[i].ColumnName;
                }
                else
                {
                    ColumnList += dsLogTable.Tables[0].Columns[i].ColumnName + ",";
                    ParameterList += "@" + dsLogTable.Tables[0].Columns[i].ColumnName + ",";
                }
                Columns.Add(dsLogTable.Tables[0].Columns[i].ColumnName);
                Parameters.Add("@" + dsLogTable.Tables[0].Columns[i].ColumnName);
            }
            for (int i = 0; i < dsLogTable.Tables[0].Rows.Count; i++)
            {
                tmpGuid = new Guid(dsLogTable.Tables[0].Rows[i]["UniqueID"].ToString());

                Where whr = new Where();
                whr.ParameterName = "@UniqueID";
                whr.ParameterValue = tmpGuid.ToString();
                whr.ColumnName = "UniqueID";
                whr.TableName = logTableName;

                if (qry.GetCount("UniqueID", whr) == 0)
                {
                    //

                    //if GUID exist, ignore. There should be no updates for logs
                    //if non existance, do insert.....

                    string sql = "insert into " + logTableName + " (" + ColumnList + ")";
                    sql += " values (" + ParameterList + ") ";
                    QueryCommand cmd = new QueryCommand(sql);
                    for (int k = 0; k < Columns.Count; k++)
                    {
                        //if (Columns[k].ToString().ToLower() == "uniqueid")
                        if (DataService.GetSchema(logTableName, "PowerPOS").GetColumn(Columns[k].ToString()).DataType == DbType.Guid)
                        {
                            cmd.AddParameter(Parameters[k].ToString(), (new Guid(dsLogTable.Tables[0].Rows[i][Columns[k].ToString()].ToString())).ToString());
                        }
                        else
                        {
                            cmd.AddParameter(Parameters[k].ToString(), dsLogTable.Tables[0].Rows[i][Columns[k].ToString()]);
                        }
                    }
                    //Logger.writeLog(dsLogTable.Tables[0].Rows[i][Columns[0].ToString()].ToString());
                    SubSonic.DataService.ExecuteQuery(cmd);
                }
                //i++;
            }
            return true;
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
            throw ex;
        }
    }

    public static bool FetchLogTableWithUpdateOption(DataSet dsLogTable, string logTableName, bool doUpdate)
    {
        CultureInfo ct = new CultureInfo("");
        DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
        dtFormat.ShortDatePattern = "yyyy-MM-dd";
        ct.DateTimeFormat = dtFormat;
        System.Threading.Thread.CurrentThread.CurrentCulture = ct;
        System.Threading.Thread.CurrentThread.CurrentUICulture = ct;

        try
        {
            Query qry;
            Guid tmpGuid;

            qry = new Query(logTableName);
            ArrayList Columns = new ArrayList();
            ArrayList Parameters = new ArrayList();
            Logger.writeLog("Fetch:" + logTableName);

            int startCol = 0;
            if (DataService.GetSchema(logTableName, "PowerPOS").PrimaryKey.AutoIncrement)
            {
                startCol = 1;
            }

            if (DataService.GetSchema(logTableName, "PowerPOS").PrimaryKey.DataType == DbType.Guid)
            {
                int f = 0;
                f = f + 1;
            }
            string ColumnList = "", ParameterList = "";
            for (int i = startCol; i < dsLogTable.Tables[0].Columns.Count; i++)
            {
                if (i == dsLogTable.Tables[0].Columns.Count - 1)
                {
                    ColumnList += dsLogTable.Tables[0].Columns[i].ColumnName;
                    ParameterList += "@" + dsLogTable.Tables[0].Columns[i].ColumnName;
                }
                else
                {
                    ColumnList += dsLogTable.Tables[0].Columns[i].ColumnName + ",";
                    ParameterList += "@" + dsLogTable.Tables[0].Columns[i].ColumnName + ",";
                }
                Columns.Add(dsLogTable.Tables[0].Columns[i].ColumnName);
                Parameters.Add("@" + dsLogTable.Tables[0].Columns[i].ColumnName);
            }
            for (int i = 0; i < dsLogTable.Tables[0].Rows.Count; i++)
            {
                tmpGuid = new Guid(dsLogTable.Tables[0].Rows[i]["UniqueID"].ToString());

                Where whr = new Where();
                whr.ParameterName = "@UniqueID";
                whr.ParameterValue = tmpGuid.ToString();
                whr.ColumnName = "UniqueID";
                whr.TableName = logTableName;

                if (qry.GetCount("UniqueID", whr) == 0)
                {
                    //if non existance, do insert.....

                    string sql = "insert into " + logTableName + " (" + ColumnList + ")";
                    sql += " values (" + ParameterList + ") ";
                    QueryCommand cmd = new QueryCommand(sql);
                    for (int k = 0; k < Columns.Count; k++)
                    {
                        //if (Columns[k].ToString().ToLower() == "uniqueid")
                        if (DataService.GetSchema(logTableName, "PowerPOS").GetColumn(Columns[k].ToString()).DataType == DbType.Guid)
                        {
                            cmd.AddParameter(Parameters[k].ToString(), (new Guid(dsLogTable.Tables[0].Rows[i][Columns[k].ToString()].ToString())).ToString());
                        }
                        else
                        {
                            cmd.AddParameter(Parameters[k].ToString(), dsLogTable.Tables[0].Rows[i][Columns[k].ToString()]);
                        }
                    }
                    //Logger.writeLog(dsLogTable.Tables[0].Rows[i][Columns[0].ToString()].ToString());
                    SubSonic.DataService.ExecuteQuery(cmd);
                }
                else
                {
                    if (doUpdate)
                    {
                        // do update......

                        QueryCommand cmd = new QueryCommand("");
                        string sql = "update " + logTableName + " set ";
                        for (int k = 0; k < Columns.Count; k++)
                        {
                            if (Columns[k].ToString().ToUpper() == "UNIQUEID") continue;  // Skip if column = UniqueID

                            sql += Columns[k].ToString() + "=" + Parameters[k].ToString() + ",";

                            if (DataService.GetSchema(logTableName, "PowerPOS").GetColumn(Columns[k].ToString()).DataType == DbType.Guid)
                            {
                                cmd.AddParameter(Parameters[k].ToString(), (new Guid(dsLogTable.Tables[0].Rows[i][Columns[k].ToString()].ToString())).ToString());
                            }
                            else
                            {
                                cmd.AddParameter(Parameters[k].ToString(), dsLogTable.Tables[0].Rows[i][Columns[k].ToString()]);
                            }
                        }
                        sql = sql.Trim(',');
                        sql += " where UniqueID = @UniqueID and ModifiedOn < @ModifiedOn ";  // Only update if received data is newer
                        cmd.AddParameter("@UniqueID", tmpGuid, DbType.Guid);
                        cmd.CommandSql = sql;

                        SubSonic.DataService.ExecuteQuery(cmd);
                    }
                }
                //i++;
            }
            return true;
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
            throw ex;
        }
    }

    public static bool FetchDeliveryOrderAndDetails(DataSet dsData)
    {
        try
        {
            if (dsData.Tables.Contains("DeliveryOrder"))
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(dsData.Tables["DeliveryOrder"].Copy());
                FetchLogTableWithUpdateOption(ds, "DeliveryOrder", true);
            }

            if (dsData.Tables.Contains("DeliveryOrderDetails"))
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(dsData.Tables["DeliveryOrderDetails"].Copy());
                FetchLogTableWithUpdateOption(ds, "DeliveryOrderDetails", true);
            }

            if (dsData.Tables.Contains("DepositAmount"))
            {
                foreach (DataRow dr in dsData.Tables["DepositAmount"].Rows)
                {
                    OrderDet od = new OrderDet(dr["OrderDetID"].ToString());
                    if (!string.IsNullOrEmpty(od.OrderDetID))
                    {
                        od.DepositAmount = Convert.ToDecimal(dr["DepositAmount"]);
                        od.Save("SYNC");
                    }
                }
            }

            InventoryController.AssignStockOutToPreOrderSalesUsingTransaction();

            return true;
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
            throw ex;
        }
    }

    public static bool UpdateOrderHdrRemark(string orderHdrID, string remark)
    {
        try
        {
            OrderHdr oh = new OrderHdr(orderHdrID);
            if (oh != null && oh.OrderHdrID == orderHdrID)
            {
                oh.Remark = remark;
                oh.Save("SYNC");
            }
            return true;
        }
        catch (Exception ex)
        {
            Logger.writeLog("Error in UpdateOrderHdrRemarks. Please check the error log below.");
            Logger.writeLog(ex);
            return false;
        }
    }
    #endregion

    #region FreezePOS

    public static bool FreezePOSByPointOfSaleID(int PointOfSaleID)
    {
        try
        {
            PointOfSale po = new PointOfSale(PointOfSaleID);
            if (po != null && po.PointOfSaleID == PointOfSaleID)
            {
                po.Userflag5 = true;
                po.Save("SYNC");
            }
            return true;
        }
        catch (Exception ex)
        {
            Logger.writeLog("Error in Freeze POS. Please check the error log below.");
            Logger.writeLog(ex);
            return false;
        }
    }


    #endregion

    #region "Inventory"
    /*
    private static QueryCommandCollection FetchInventoryHdr(DataSet dsHeaders)
    {
        try
        {
            QueryCommandCollection cmd = new QueryCommandCollection();
            QueryCommand mycmd;
            Query qry;
            Object tmpGuid;

            ArrayList duplicate = new ArrayList();

            InventoryHdrCollection myHdr = new InventoryHdrCollection();
            myHdr.Load(dsHeaders.Tables[0]);

            //Translate from DataSet to actual database

            //Inventory HEADER
            for (int i = 0; i < myHdr.Count; i++)
            {
                //Get GUID - This is needed due to bug with SubSonic
                myHdr[i].UniqueID = new Guid(dsHeaders.Tables[0].Rows[i]["UniqueID"].ToString());
                    //Select("InventoryHdrRefNo = '" + myHdr[i].InventoryHdrRefNo + "'")[0]["UniqueID"].ToString());

                //Check GUID uniqueness in Target DB
                qry = new Query("InventoryHdr");
                qry.SelectList = "UniqueID";
                tmpGuid = qry.WHERE("InventoryHdrRefNo", myHdr[i].InventoryHdrRefNo).ExecuteScalar();

                if (tmpGuid is Guid) //GUID Found?
                {
                    if ((Guid)tmpGuid == myHdr[i].UniqueID)
                    {
                        //same Inventory ID and unique ID - no problem, do update....                        
                        mycmd = myHdr[i].GetUpdateCommand("SYNC");
                        cmd.Add(mycmd);
                    }
                    else
                    {
                        //same Inventory but different unique ID - Houston we have a problem                        
                        duplicate.Add(myHdr[i].InventoryHdrRefNo);
                    }
                }
                else
                {
                    mycmd = myHdr[i].GetInsertCommand("SYNC");
                    cmd.Add(mycmd);
                }
            }
            if (duplicate.Count != 0)
            {
                string str;
                str = "Error: Attempting to enter duplicate Inventory Header Ref Number.";
                str = str + "\r\n" + "Duplicates:" + "\r\n";
                for (int i = 0; i < duplicate.Count; i++)
                {
                    str += duplicate[i] + "\r\n";
                }

                Logger.writeLog(str);
                return null;
            }
            return cmd;
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
            return null;
        }
    }
    private static QueryCommandCollection FetchInventoryDetail(DataSet dsDetails)
    {
        try
        {
            QueryCommandCollection cmd = new QueryCommandCollection();
            QueryCommand mycmd;
            Query qry;
            Object tmpGuid;

            ArrayList duplicate = new ArrayList();
            InventoryDetCollection myDet = new InventoryDetCollection();
            myDet.Load(dsDetails.Tables[0]);

            qry = new Query("InventoryDet");
            //Translate from DataSet to actual database
            for (int i = 0; i < myDet.Count; i++)
            {
                //Get count....                                    
                myDet[i].UniqueID = new Guid(
                dsDetails.Tables[0].Rows[i]["UniqueID"].ToString());
                //Select("InventoryDetRefNo = '" + myDet[i].InventoryDetRefNo + "'")[0]["UniqueID"].ToString());

                //Check GUID uniqueness in Target DB
                qry = new Query("InventoryDet");
                qry.SelectList = "UniqueID";
                tmpGuid = qry.WHERE("InventoryDetRefNo", myDet[i].InventoryDetRefNo).ExecuteScalar();

                if (tmpGuid is Guid) //GUID Found?
                {
                    if ((Guid)tmpGuid == myDet[i].UniqueID)
                    {
                        //same Inventory ID and unique ID - no problem, do update....                        
                        mycmd = myDet[i].GetUpdateCommand("SYNC");
                        cmd.Add(mycmd);
                    }
                    else
                    {
                        //same Inventory but different unique ID - Houston we have a problem                        
                        duplicate.Add(myDet[i].InventoryDetRefNo);
                    }
                }
                else
                {
                    mycmd = myDet[i].GetInsertCommand("SYNC");
                    cmd.Add(mycmd);
                }

            }
            if (duplicate.Count != 0)            
            {
                string str;
                str = "Error: Attempting to enter duplicate Inventory Number. Check if two PointOfSale are assigned the same PointOfSale ID.";
                str = str + "\r\n" + "Duplicates ID:" + "\r\n";
                for (int i = 0; i < duplicate.Count; i++)
                {
                    str += duplicate[i] + "\r\n";
                }

                Logger.writeLog(str);
                return null;
            }
            return cmd;
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
            return null;
        }
    }
    public static bool FetchInventoryData(DataSet dsHeaders, DataSet dsDetails)
    {
        try
        {
            Logger.writeLog("Inventory Synchronization started.");

            QueryCommandCollection cmd, tmp;
            cmd = new QueryCommandCollection();

            tmp = FetchInventoryHdr(dsHeaders);
            if (tmp != null)
            {
                cmd.AddRange(tmp);
            }
            else
            {
                return false;
            }

            tmp = FetchInventoryDetail(dsDetails);
            if (tmp != null)
            {
                cmd.AddRange(tmp);
            }
            else
            {
                return false;
            }

            SubSonic.DataService.ExecuteTransaction(cmd);
            Logger.writeLog("Inventories Synchronization finishes.");
            return true;
        }
        catch (Exception ex)
        {
            Logger.writeLog("Inventories Synchronization failed. Please check your log file");
            Logger.writeLog(ex);
            return true;
        }
    }
    */
    /*
    public static bool FetchInventoryData(DataSet dsHeaders, DataSet dsDetails)
    {
        try
        {
            QueryCommandCollection cmd;
            QueryCommand mycmd;
            Query qry;
            Where whr;
            ArrayList duplicate = new ArrayList();
            InventoryHdrCollection myHdr = new InventoryHdrCollection();
            myHdr.Load(dsHeaders.Tables[0]);

            //Set where condition to check the count....
            cmd = new QueryCommandCollection();
            qry = new Query("InventoryHdr");
            whr = new Where();
            whr.ColumnName = "InventoryHdrRefNo";
            whr.Comparison = Comparison.Equals;
            whr.ParameterName = "@InventoryHdrRefNo";
            //Translate from DataSet to actual database
            for (int i = 0; i < myHdr.Count; i++)
            {
                //Get count....
                whr.ParameterValue = myHdr[i].InventoryHdrRefNo;
                if (qry.GetCount(InventoryHdr.Columns.InventoryHdrRefNo, whr) > 0)
                {
                    duplicate.Add(myHdr[i].InventoryHdrRefNo);
                }
                else
                {
                    mycmd = myHdr[i].GetInsertCommand("SYNC");
                    cmd.Add(mycmd);
                }
            }
            if (duplicate.Count == 0)
            {
                InventoryDetCollection myDet = new InventoryDetCollection();
                myDet.Load(dsDetails.Tables[0]);

                qry = new Query("InventoryDet");
                whr = new Where();
                whr.ColumnName = "InventoryDetRefNo";
                whr.Comparison = Comparison.Equals;
                whr.ParameterName = "@InventoryDetRefNo";
                //Translate from DataSet to actual database
                for (int i = 0; i < myDet.Count; i++)
                {
                    //Get count....
                    whr.ParameterValue = myDet[i].InventoryDetRefNo;
                    if (qry.GetCount(InventoryDet.Columns.InventoryDetRefNo, whr) > 0)
                    {
                        throw new Exception("Duplicate InventoryDetail ID encountered:" + myDet[i].InventoryDetRefNo);
                    }
                    else
                    {
                        mycmd = myDet[i].GetInsertCommand("SYNC");
                    }
                    cmd.Add(mycmd);
                }

                SubSonic.DataService.ExecuteTransaction(cmd);
            }
            else
            {
                string str;
                str = "Error: Attempting to enter duplicate Inventory Header Ref Number.";
                str = str + "\r\n" + "Duplicates:" + "\r\n";
                for (int i = 0; i < duplicate.Count; i++)
                {
                    str += duplicate[i] + "\r\n";
                }

                Logger.writeLog(str);
                return false;
            }

            Logger.writeLog("Synchronization completed.");
            return true;
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
            return false;
        }
    }*/
    #endregion

    #region "Item"
    public static bool FetchItemsData(DataSet dsItems)
    {
        try
        {
            Logger.writeLog("Synchronization started for Item.");
            QueryCommandCollection cmd;
            QueryCommand mycmd;
            Query qry;
            Where whr;

            //Get the PointOfSales            
            ItemCollection Items = new ItemCollection();
            Items.Load(dsItems.Tables[0]);

            //Set where condition to check the count....
            cmd = new QueryCommandCollection();
            qry = new Query("Item");
            whr = new Where();
            whr.ColumnName = "ItemNo";
            whr.Comparison = Comparison.Equals;
            whr.ParameterName = "@ItemNo";

            for (int i = 0; i < Items.Count; i++)
            {
                //Get count....
                whr.ParameterValue = Items[i].ItemNo;
                if (qry.GetCount(Item.Columns.ItemNo, whr) > 0)
                {
                    mycmd = Items[i].GetUpdateCommand("SYNC");
                }
                else
                {
                    mycmd = Items[i].GetInsertCommand("SYNC");
                }
                cmd.Add(mycmd);
            }
            DataService.ExecuteTransaction(cmd);
            Logger.writeLog("Synchronization completed.");
            return true;
        }
        catch (Exception ex)
        {
            Logger.writeLog("Synchronization failed.");
            Logger.writeLog(ex);
            return false;
        }
    }
    public static bool FetchCategoriesData(DataSet dsCategories)
    {
        try
        {
            Logger.writeLog("Synchronization started for Category.");
            QueryCommandCollection cmd;
            QueryCommand mycmd;
            Query qry;
            Where whr;


            //Get the PointOfSales            
            CategoryCollection Categories = new CategoryCollection();
            Categories.Load(dsCategories.Tables[0]);

            //Set where condition to check the count....
            cmd = new QueryCommandCollection();
            qry = new Query("Category");
            whr = new Where();
            whr.ColumnName = "CategoryName";
            whr.Comparison = Comparison.Equals;
            whr.ParameterName = "@CategoryName";

            for (int i = 0; i < Categories.Count; i++)
            {
                //Get count....
                whr.ParameterValue = Categories[i].CategoryName;
                if (qry.GetCount(Category.Columns.CategoryName, whr) > 0)
                {
                    mycmd = Categories[i].GetUpdateCommand("SYNC");
                }
                else
                {
                    mycmd = Categories[i].GetInsertCommand("SYNC");
                }
                cmd.Add(mycmd);
            }
            DataService.ExecuteTransaction(cmd);
            Logger.writeLog("Synchronization completed.");
            return true;
        }
        catch (Exception ex)
        {
            Logger.writeLog("Synchronization failed.");
            Logger.writeLog(ex);
            return false;
        }
    }
    #endregion

    #region "Appointment"
    public static bool SaveAppointment(DataSet Objs)
    {
        try
        {
            QueryCommandCollection Cmds = new QueryCommandCollection();

            Guid AppointmentID = Guid.NewGuid();
            DateTime ModifiedOn = DateTime.Now;

            for (int CntTbl = 0; CntTbl < Objs.Tables.Count; CntTbl++)
            {
                DataTable ActiveTable = Objs.Tables[CntTbl];

                for (int CntRow = 0; CntRow < ActiveTable.Rows.Count; CntRow++)
                {
                    #region -= Object Factory =-
                    IActiveRecord Obj;
                    if (ActiveTable.TableName.ToLower() == "appointment")
                    { Obj = new Appointment(); }
                    else if (ActiveTable.TableName.ToLower() == "appointmentitem")
                    {
                        Obj = new AppointmentItem();
                        string sqlremoveAppItems = "UPDATE AppointmentItem Set Deleted = 1 where AppointmentId = '" + AppointmentID + "'";
                        DataService.ExecuteQuery(new QueryCommand(sqlremoveAppItems));
                    }
                    else if (ActiveTable.TableName.ToLower() == "membership")
                    { Obj = new PowerPOS.Membership(); }
                    else
                        continue;

                    Obj.Load(ActiveTable.Rows[CntRow]);
                    #endregion

                    bool isExist = false;
                    #region *) Validation: Check if the Object exists on server
                    string sqlChecker =
                        "SELECT ISNULL(COUNT(*),0) " +
                        "FROM " + Obj.GetSchema().TableName + " " +
                        "WHERE " + Obj.GetSchema().PrimaryKey.ColumnName + " = '" + Obj.GetPrimaryKeyValue().ToString() + "'";

                    isExist = ((int)DataService.ExecuteScalar(new QueryCommand(sqlChecker))) > 0;
                    #endregion

                    #region *) Validation: Specify the command type (INSERT/UPDATE)
                    Obj.IsNew = !isExist;
                    #endregion

                    #region *) Saving: Put the command into transaction
                    if (isExist)
                    {
                        if (ActiveTable.TableName.ToLower() == "membership")
                        {
                            PowerPOS.Membership mbr = (PowerPOS.Membership)Obj;
                            //Logger.writeLog(">>> Deleted" + itm.Deleted.ToString());
                            //Logger.writeLog(">>> UPDATING APPOINTMENTITEM ID : " + itm.Id);

                            PowerPOS.Membership member = new PowerPOS.Membership(mbr.MembershipNo);
                            if (member == null || member.MembershipNo == null || member.MembershipNo == "")
                            {
                                Cmds.Add(mbr.GetInsertCommand("SYNC"));
                            }


                        }
                        else if (ActiveTable.TableName.ToLower() == "appointment")
                        {
                            Appointment i = (Appointment)Obj;
                            Logger.writeLog(">>> Deleted" + i.Deleted.ToString());
                            Logger.writeLog(">>> UPDATING APPOINTMENT ID : " + i.Id);
                            AppointmentID = i.Id;

                            Appointment it = new Appointment(i.Id);
                            it.StartTime = i.StartTime;
                            it.Duration = i.Duration;
                            it.Description = i.Description;
                            it.BackColor = i.BackColor;
                            it.FontColor = i.FontColor;
                            it.SalesPersonID = i.SalesPersonID;
                            it.MembershipNo = i.MembershipNo;
                            it.OrderHdrID = i.OrderHdrID;
                            it.Deleted = i.Deleted;
                            it.Organization = i.Organization;
                            it.PickUpLocation = i.PickUpLocation;
                            it.NoOfChildren = i.NoOfChildren;
                            it.PointOfSaleID = i.PointOfSaleID;
                            it.ResourceID = i.ResourceID;
                            it.CheckInByWho = i.CheckInByWho;
                            it.CheckOutByWho = i.CheckOutByWho;
                            it.CheckInTime = i.CheckInTime;
                            it.CheckOutTime = i.CheckOutTime;
                            it.Remark = i.Remark;
                            it.TimeExtension = i.TimeExtension;
                            Cmds.Add(it.GetUpdateCommand("SYNC"));
                        }
                        else if (ActiveTable.TableName.ToLower() == "appointmentitem")
                        {
                            AppointmentItem itm = (AppointmentItem)Obj;
                            Logger.writeLog(">>> Deleted" + itm.Deleted.ToString());
                            Logger.writeLog(">>> UPDATING APPOINTMENTITEM ID : " + itm.Id);

                            AppointmentItem it = new AppointmentItem(itm.Id);
                            it.AppointmentId = itm.AppointmentId;
                            it.ItemNo = itm.ItemNo;
                            it.UnitPrice = itm.UnitPrice;
                            it.Quantity = itm.Quantity;
                            it.Deleted = itm.Deleted;
                            Cmds.Add(it.GetUpdateCommand("SYNC"));
                        }

                    }
                    else
                    {
                        if (ActiveTable.TableName.ToLower() == "appointmentitem")
                        {
                            AppointmentItem i = (AppointmentItem)Obj;
                            i.AppointmentId = AppointmentID;
                            Logger.writeLog(">>> INSERTING NEW APPOINTMENTITEM NO : " + i.AppointmentId);
                            Cmds.Add(Obj.GetInsertCommand("SYNC"));
                        }
                        else if (ActiveTable.TableName.ToLower() == "membership")
                        {
                            PowerPOS.Membership mbr = (PowerPOS.Membership)Obj;
                            //Logger.writeLog(">>> Deleted" + itm.Deleted.ToString());
                            //Logger.writeLog(">>> UPDATING APPOINTMENTITEM ID : " + itm.Id);

                            PowerPOS.Membership member = new PowerPOS.Membership(mbr.MembershipNo);
                            if (member == null || member.MembershipNo == null || member.MembershipNo == "")
                            {
                                Cmds.Add(mbr.GetInsertCommand("SYNC"));
                            }


                        }
                        else if (ActiveTable.TableName.ToLower() == "appointment")
                        {
                            Appointment i = (Appointment)Obj;
                            i.Id = AppointmentID;
                            Logger.writeLog(">>> INSERTING NEW APPOINTMENT NO : " + i.Id);
                            Cmds.Add(i.GetInsertCommand("SYNC"));
                        }

                    }

                    #endregion
                }
            }

            DataService.ExecuteTransaction(Cmds);

            return true;
        }
        catch (Exception X)
        {
            Logger.writeLog(X);
            return false;
        }
    }
    #endregion

    #endregion

    #region "OUTGOING - Send Data from Master to client"
    /*
    public static DataSet GetDataTable(string TableName, bool syncAll)
    {
        Query qry = new Query(TableName);
        qry.QueryType = QueryType.Select;
        
        
        //Get data that was modified within 7 days from now
        if (!syncAll)
            qry.AddWhere("ModifiedOn",Comparison.GreaterOrEquals,DateTime.Now.AddDays(-7));

        return qry.ExecuteDataSet();
    }
    */
    public static DataSet GetDataTable(string TableName, bool syncAll)
    {
        return SPs.GetDataTable(TableName, syncAll, DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd")).GetDataSet();
    }

    public static string GetDataTableSerial(string TableName, bool syncAll)
    {
        return GetJSONString(SPs.GetDataTable(TableName, syncAll, DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd")).GetDataSet().Tables[0]);
    }

    public static string GetItemSerial(bool syncAll)
    {
        ItemController it = new ItemController();
        DataTable dt = it.SearchItem_PlusPointInfoForIntegration(syncAll);
        dt = CommonUILib.DataTableConvertBoolColumnToYesOrNo(dt);
        return JsonConvert.SerializeObject(dt);
    }

    public static string GetJSONString(DataTable Dt)
    {

        string[] StrDc = new string[Dt.Columns.Count];
        string HeadStr = string.Empty;

        for (int i = 0; i < Dt.Columns.Count; i++)
        {
            StrDc[i] = Dt.Columns[i].Caption;
            HeadStr += "\"" + StrDc[i] + "\" : \"" + StrDc[i] + i.ToString() + "¾" + "\",";
        }

        HeadStr = HeadStr.Substring(0, HeadStr.Length - 1);
        StringBuilder Sb = new StringBuilder();
        Sb.Append("{\"" + Dt.TableName + "\" : [");

        for (int i = 0; i < Dt.Rows.Count; i++)
        {
            string TempStr = HeadStr;
            Sb.Append("{");

            for (int j = 0; j < Dt.Columns.Count; j++)
            {
                TempStr = TempStr.Replace(Dt.Columns[j] + j.ToString() + "¾", Dt.Rows[i][j].ToString());
            }
            Sb.Append(TempStr + "},");
        }
        Sb = new StringBuilder(Sb.ToString().Substring(0, Sb.ToString().Length - 1));
        Sb.Append("]}");

        return Sb.ToString();
    }

    public static string[] ParseOrder(string orderData)
    {
        string[] result;
        result = orderData.Split('}');
        for (int i = 0; i < result.Length; i++)
        {
            if (result[i].Length > 0)
            {
                result[i] = result[i].Substring(result[i].IndexOf('{') + 1);
            }
        }
        return result;
    }

    public static byte[] GetDataTableCompressed(string TableName, bool syncAll)
    {
        DataSet myDataSet = SPs.GetDataTable(TableName, syncAll, DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd")).GetDataSet();

        byte[] data = SyncClientController.CompressDataSetToByteArray(myDataSet);
        return data;
    }

    public static byte[] GetDataTableCustomCompressed(string sqlQuery)
    {
        QueryCommand cmd = new QueryCommand(sqlQuery);
        DataSet ds = DataService.GetDataSet(cmd);
        return SyncClientController.CompressDataSetToByteArray(ds);
    }

    public static DataSet GetDeliveryOrder(DateTime startDate, DateTime endDate, int PointOfSaleID)
    {
        CultureInfo ct = new CultureInfo("");
        DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
        dtFormat.ShortDatePattern = "yyyy-MM-dd";
        ct.DateTimeFormat = dtFormat;
        System.Threading.Thread.CurrentThread.CurrentCulture = ct;
        System.Threading.Thread.CurrentThread.CurrentUICulture = ct;

        DataSet ds;
        DataSet resultDS = new DataSet();
        string sql;
        QueryCommand cmd;

        try
        {
            sql = @"SELECT do.* 
                    FROM DeliveryOrder do 
                        INNER JOIN OrderHdr oh ON oh.OrderHdrID = do.SalesOrderRefNo 
                    WHERE ((do.CreatedOn BETWEEN @StartDate AND @EndDate) OR (do.ModifiedOn BETWEEN @StartDate AND @EndDate)) 
                        AND oh.PointOfSaleID = @PointOfSaleID 
                  ";
            cmd = new QueryCommand(sql, "PowerPOS");
            cmd.Parameters.Add("@StartDate", startDate, DbType.DateTime);
            cmd.Parameters.Add("@EndDate", endDate, DbType.DateTime);
            cmd.Parameters.Add("@PointOfSaleID", PointOfSaleID, DbType.Int32);
            ds = DataService.GetDataSet(cmd);
            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0].Copy();
                dt.TableName = "DeliveryOrder";
                resultDS.Tables.Add(dt);
            }

            sql = @"SELECT dod.* 
                    FROM DeliveryOrderDetails dod
                        INNER JOIN DeliveryOrder do ON do.OrderNumber = dod.DOHDRID
                        INNER JOIN OrderHdr oh ON oh.OrderHdrID = do.SalesOrderRefNo
                    WHERE ((dod.CreatedOn BETWEEN @StartDate AND @EndDate) OR (dod.ModifiedOn BETWEEN @StartDate AND @EndDate)) 
                        AND oh.PointOfSaleID = @PointOfSaleID 
                  ";
            cmd = new QueryCommand(sql, "PowerPOS");
            cmd.Parameters.Add("@StartDate", startDate, DbType.DateTime);
            cmd.Parameters.Add("@EndDate", endDate, DbType.DateTime);
            cmd.Parameters.Add("@PointOfSaleID", PointOfSaleID, DbType.Int32);
            ds = DataService.GetDataSet(cmd);
            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0].Copy();
                dt.TableName = "DeliveryOrderDetails";
                resultDS.Tables.Add(dt);
            }

            // Get Deposit Amount from OrderDet
            sql = @"SELECT od.* 
                    FROM OrderDet od
                        INNER JOIN OrderHdr oh ON oh.OrderHdrID = od.OrderHdrID
                    WHERE ((od.CreatedOn BETWEEN @StartDate AND @EndDate) OR (od.ModifiedOn BETWEEN @StartDate AND @EndDate)) 
                        AND oh.PointOfSaleID = @PointOfSaleID 
                  ";
            cmd = new QueryCommand(sql, "PowerPOS");
            cmd.Parameters.Add("@StartDate", startDate, DbType.DateTime);
            cmd.Parameters.Add("@EndDate", endDate, DbType.DateTime);
            cmd.Parameters.Add("@PointOfSaleID", PointOfSaleID, DbType.Int32);
            ds = DataService.GetDataSet(cmd);
            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dt = new DataTable("DepositAmount");
                dt.Columns.Add("OrderDetID", Type.GetType("System.String"));
                dt.Columns.Add("DepositAmount", Type.GetType("System.Decimal"));
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    OrderDet od = new OrderDet(dr["OrderDetID"].ToString());
                    if (!string.IsNullOrEmpty(od.OrderDetID))
                    {
                        dt.Rows.Add(od.OrderDetID, od.DepositAmount);
                    }
                }
                resultDS.Tables.Add(dt);
            }

            //// Get Deposit Amount from OrderDet if there's modified DeliveryOrderDetails
            //if (resultDS.Tables.Contains("DeliveryOrderDetails") && resultDS.Tables["DeliveryOrderDetails"].Rows.Count > 0)
            //{
            //    DataTable dt = new DataTable("DepositAmount");
            //    dt.Columns.Add("OrderDetID", Type.GetType("System.String"));
            //    dt.Columns.Add("DepositAmount", Type.GetType("System.Decimal"));
            //    foreach (DataRow dr in resultDS.Tables["DeliveryOrderDetails"].Rows)
            //    {
            //        OrderDet od = new OrderDet(dr["OrderDetID"].ToString());
            //        if (!string.IsNullOrEmpty(od.OrderDetID))
            //        {
            //            dt.Rows.Add(od.OrderDetID, od.DepositAmount);
            //        }
            //    }
            //    resultDS.Tables.Add(dt);
            //}

            return resultDS;
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
            return null;
        }
    }

    #region "Junk Code"
    /*
    public static DataSet GetCashRecordingTypeData()
    {
        CashRecordingTypeCollection myCashRecordingTypes = new CashRecordingTypeCollection();

        //parse to generic array of string....
        myCashRecordingTypes.Load();

        DataSet ds = new DataSet();
        ds.Tables.Add(myCashRecordingTypes.ToDataTable());
        ds.Tables[0].TableName = "CashRecordingType";

        //From DataSet to datatable
        return ds;
    }
    public static DataSet GetOutletData()
    {
        OutletCollection myOutlets = new OutletCollection();

        //parse to generic array of string....
        myOutlets.Load();      

        DataSet ds = new DataSet();
        ds.Tables.Add(myOutlets.ToDataTable());
        ds.Tables[0].TableName = "Outlet";

        //From DataSet to datatable
        return ds;
    }
    public static DataSet GetPointOfSaleData()
    {
        PointOfSaleCollection st = new PointOfSaleCollection();

        //parse to generic array of string....
        st.Load();        

        DataSet ds = new DataSet();
            ds.Tables.Add(st.ToDataTable());
        ds.Tables[0].TableName = "PointOfSale";

        //From DataSet to datatable
        return ds;
    }
    public static DataSet GetGroupPrivilegesData()
    {
        GroupUserPrivilegeCollection gup = new GroupUserPrivilegeCollection();

        //parse to generic array of string....
        gup.Load();

        DataSet ds = new DataSet();
        ds.Tables.Add(gup.ToDataTable());
        ds.Tables[0].TableName = "GroupUserPrivilege";

        //From DataSet to datatable
        return ds;
    }
    public static DataSet GetUserMstData()
    {
        UserMstCollection gup = new UserMstCollection();

        //parse to generic array of string....
        gup.Load();

        DataSet ds = new DataSet();
        ds.Tables.Add(gup.ToDataTable());
        ds.Tables[0].TableName = "UserMst";

        //From DataSet to datatable
        return ds;
    }
    public static DataSet GetUserPrivilegeData()
    {
        UserPrivilegeCollection gup = new UserPrivilegeCollection();
        
        //parse to generic array of string....
        gup.Load();

        DataSet ds = new DataSet();
        ds.Tables.Add(gup.ToDataTable());
        ds.Tables[0].TableName = "UserPrivilege";

        //From DataSet to datatable
        return ds;
    }

    
    public static DataSet GetUserGroupData()
    {
        UserGroupCollection gup = new UserGroupCollection();
        
        //parse to generic array of string....
        gup.Load();

        DataSet ds = new DataSet();
        ds.Tables.Add(gup.ToDataTable());
        ds.Tables[0].TableName = "UserGroup";

        //From DataSet to datatable
        return ds;
    }*/
    #endregion
    #endregion

    #region "MISC"

    public static bool FetchInventorysData
            (string[][] dsHeaders, string[][] dsDetails, string[][] remainingQty
            )
    {
        Logger.writeLog("Inventorys Synchronization started.");

        try
        {
            QueryCommandCollection qr = new QueryCommandCollection();
            qr.AddRange(FetchInventoryHdr(dsHeaders));
            qr.AddRange(FetchInventoryDet(dsDetails));
            qr.AddRange(UpdateRemainingQty(remainingQty));
            DataService.ExecuteTransaction(qr);
            Logger.writeLog("Inventorys Synchronization finishes.");
            return true;
        }
        catch (Exception ex)
        {
            Logger.writeLog("Inventorys Synchronization failed. Please check your log file");
            Logger.writeLog(ex);

            return false;
        }

    }

    public static List<InventoryItem> GetStockQuantity(Int32 InventoryLocationID)
    {
        QueryCommand cmd = new QueryCommand("SELECT ItemNo, (SUM(CASE WHEN movementtype LIKE 'Stock In' THEN quantity ELSE 0 END) - SUM(CASE WHEN movementtype LIKE 'Stock Out' THEN quantity ELSE 0 END) + SUM(CASE WHEN movementtype LIKE 'Transfer In' THEN quantity ELSE 0 END) - SUM(CASE WHEN movementtype LIKE 'Transfer Out' THEN quantity ELSE 0 END) + SUM(CASE WHEN movementtype LIKE 'Adjustment In' THEN quantity ELSE 0 END) - SUM(CASE WHEN movementtype LIKE 'Adjustment Out' THEN quantity ELSE 0 END)) as RunningQty FROM InventoryHdr IH INNER JOIN InventoryDet ID ON IH.InventoryHdrRefNo = ID.InventoryHdrRefNo INNER JOIN InventoryLocation LI ON IH.InventoryLocationID = LI.InventoryLocationID WHERE CAST(IH.InventoryLocationID AS VARCHAR(4)) LIKE '" + InventoryLocationID.ToString().Replace("'", "") + "'GROUP BY ItemNo", SynchronizationController.providerName);
        DataSet ds = DataService.GetDataSet(cmd);

        if (ds.Tables.Count <= 0)
        {
            throw new Exception("No item found in inventory location id " + InventoryLocationID);
        }

        List<InventoryItem> items = new List<InventoryItem>();

        foreach (DataRow row in ds.Tables[0].Rows)
        {
            items.Add(new InventoryItem() { ItemNo = row["ItemNo"].ToString(), Qty = Convert.ToDecimal(row["RunningQty"]) });
        }

        return items;
    }

    public static string GetStockQuantityByItem(Int32 InventoryLocationID, string ItemNo)
    {
        QueryCommand cmd = new QueryCommand("SELECT ItemNo, (SUM(CASE WHEN movementtype LIKE 'Stock In' THEN quantity ELSE 0 END) - SUM(CASE WHEN movementtype LIKE 'Stock Out' THEN quantity ELSE 0 END) + SUM(CASE WHEN movementtype LIKE 'Transfer In' THEN quantity ELSE 0 END) - SUM(CASE WHEN movementtype LIKE 'Transfer Out' THEN quantity ELSE 0 END) + SUM(CASE WHEN movementtype LIKE 'Adjustment In' THEN quantity ELSE 0 END) - SUM(CASE WHEN movementtype LIKE 'Adjustment Out' THEN quantity ELSE 0 END)) as RunningQty FROM InventoryHdr IH INNER JOIN InventoryDet ID ON IH.InventoryHdrRefNo = ID.InventoryHdrRefNo INNER JOIN InventoryLocation LI ON IH.InventoryLocationID = LI.InventoryLocationID WHERE CAST(IH.InventoryLocationID AS VARCHAR(4)) LIKE '" + InventoryLocationID.ToString().Replace("'", "") + "' AND ItemNo = '" + ItemNo + "' GROUP BY ItemNo", SynchronizationController.providerName);
        DataSet ds = DataService.GetDataSet(cmd);

        if (ds.Tables.Count <= 0)
        {
            return "0";
        }
        else
        {
            if (ds.Tables[0].Rows.Count <= 0)
                return "0";
        }

        return ds.Tables[0].Rows[0]["RunningQty"].ToString();
    }

    private static QueryCommandCollection UpdateRemainingQty(string[][] remainingQty)
    {
        try
        {
            QueryCommandCollection col = new QueryCommandCollection();
            for (int i = 0; i < remainingQty.Length; i++)
            {
                for (int j = 0; j < remainingQty[i].Length; j++)
                {
                    string SQL = "update inventorydet set remainingqty = " + remainingQty[i][1] + " where inventoryDetrefno = '" + remainingQty[i][0] + "';";
                    col.Add(new QueryCommand(SQL, "PowerPOS"));
                }
            }
            return col;
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
            return null;
        }
    }

    // serverVersion : -1 = error, else = working
    public static long GetChangeTrackingCurrentVersion()
    {
        long serverVersion = -1;
        QueryCommand getCT = new QueryCommand("SELECT CHANGE_TRACKING_CURRENT_VERSION()");
        object resultCT = DataService.ExecuteScalar(getCT);
        if (resultCT.ToString() != string.Empty)
        {
            long.TryParse(resultCT.ToString(), out serverVersion);
        }
        else if (resultCT.ToString() == string.Empty)
        {
            if (EnableChangeTrackingInCurrentDatabase())
            {
                return GetChangeTrackingCurrentVersion();
            }
        }
        return serverVersion;
    }

    // syncStatus : -1 = error, 1 = syncall, 2 = partial sync, 3 = no need sync (already synced)
    public static int GetChangeTrackingVersionSyncStatus(long appSettingChangeTrackingVersion)
    {
        int syncStatus = -1;

        try
        {
            Logger.writeLog("GetVersionSyncStatus - Start");

            #region Get Server Current Version (serverVersion)
            long serverVersion = GetChangeTrackingCurrentVersion();
            if (serverVersion == -1)
            {
                throw new Exception("Change Tracking Current Version is NULL");
            }
            #endregion

            #region Get Server Min Valid Version (minValidVersion)
            long minValidVersion = -1;
            QueryCommand getMV = new QueryCommand("SELECT MAX(min_valid_version) FROM sys.change_tracking_tables");
            object resultMV = DataService.ExecuteScalar(getMV);
            if (resultMV != null)
            {
                long.TryParse(resultMV.ToString(), out minValidVersion);
            }
            if (minValidVersion == -1)
            {
                throw new Exception("Min Valid Version is NULL (Check Change Tracking Status for Tables)");
            }
            #endregion

            if (appSettingChangeTrackingVersion < minValidVersion)
            {
                // need to resync all because the latest sync is not min valid
                syncStatus = 1;
            }
            else if (appSettingChangeTrackingVersion >= minValidVersion)
            {
                if (appSettingChangeTrackingVersion < serverVersion)
                {
                    // need to do partial sync
                    syncStatus = 2;
                }
                else if (appSettingChangeTrackingVersion == serverVersion)
                {
                    // already synced
                    syncStatus = 3;
                }
                else if (appSettingChangeTrackingVersion > serverVersion)
                {
                    throw new Exception("App Setting Version > Server Change Tracking Version");
                }
            }

            Logger.writeLog("GetVersionSyncStatus - Finished");
        }
        catch (Exception ex)
        {
            Logger.writeLog("GetVersionSyncStatus - Exception");
            Logger.writeLog(ex);
        }

        return syncStatus;
    }

    public static byte[] GetChangeTrackingChangesCompressed(string tableName, long lastSyncVersion, string primaryKeyName)
    {
        string sqlGetCTChanges = "SELECT SYS_CHANGE_VERSION, SYS_CHANGE_CREATION_VERSION, SYS_CHANGE_OPERATION, " +
            "SYS_CHANGE_COLUMNS, SYS_CHANGE_CONTEXT, '" + primaryKeyName + "' AS PK, TBL.* " +
            "FROM CHANGETABLE(CHANGES " + tableName + ", " + lastSyncVersion.ToString() + ") CHG " +
            "LEFT JOIN " + tableName + " TBL ON CHG." + primaryKeyName + " = TBL." + primaryKeyName; // this code can be sql injected
        QueryCommand getCTChanges = new QueryCommand(sqlGetCTChanges);
        DataSet resultDataSet = DataService.GetDataSet(getCTChanges);
        if (resultDataSet.Tables.Count > 0)
        {
            resultDataSet.Tables[0].TableName = tableName;
        }
        return SyncClientController.CompressDataSetToByteArray(resultDataSet);
    }

    private static bool EnableChangeTrackingInCurrentDatabase()
    {
        QueryCommand getDBName = new QueryCommand("SELECT DB_NAME()");
        object resultDBName = DataService.ExecuteScalar(getDBName);

        if (resultDBName.ToString() != string.Empty)
        {
            QueryCommand enableCT = new QueryCommand("ALTER DATABASE " + resultDBName.ToString() + " " +
                "SET CHANGE_TRACKING = ON (AUTO_CLEANUP = ON, CHANGE_RETENTION = 14 DAYS); " +
                "ALTER TABLE InventoryHdr ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = ON); " +
                "ALTER TABLE InventoryDet ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = ON); " +
                "ALTER TABLE StockTake ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = ON); " +
                "ALTER TABLE LocationTransfer ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = ON); " +
                "");

            DataService.ExecuteQuery(enableCT);

            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool DeleteInventoryDetFromVoidedOrder()
    {
        try
        {
            string getInventoryHdrRefNo = "SELECT ID.InventoryHdrRefNo FROM InventoryDet ID " +
                "INNER JOIN InventoryHdr IH ON IH.InventoryHdrRefNo = ID.InventoryHdrRefNo " +
                "INNER JOIN OrderDet OD ON OD.InventoryHdrRefNo = IH.InventoryHdrRefNo " +
                "INNER JOIN OrderHdr OH ON OH.OrderHdrID = OD.OrderHdrID " +
                "WHERE OH.IsVoided = 1";
            QueryCommand getCTChanges = new QueryCommand(getInventoryHdrRefNo, providerName);
            DataSet resultDataSet = DataService.GetDataSet(getCTChanges);
            if (resultDataSet.Tables.Count > 0)
            {
                if (resultDataSet.Tables[0].Rows.Count > 0)
                {
                    System.Transactions.TransactionOptions to = new System.Transactions.TransactionOptions();
                    to.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
                    using (System.Transactions.TransactionScope transScope =
                    new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Required, to))
                    {
                        for (int i = 0; i < resultDataSet.Tables[0].Rows.Count; i++)
                        {
                            string currentInventoryHdrRefNo = resultDataSet.Tables[0].Rows[i]["InventoryHdrRefNo"].ToString();
                            InventoryController.UndoStockOut(currentInventoryHdrRefNo);
                        }

                        transScope.Complete();
                    }
                }
            }

            return true;
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
            return false;
        }
    }

    #endregion

    #region "Real Time Sync"

    public static int GetInventoryTableRealTimeCount(DateTime lastModifiedOn)
    {
        //Logger.writeLog("Get Inventory Table Real Time Count :" + lastModifiedOn.ToString("yyyy-MM-dd"));
        //DataSet myDataSet = SPs.GetDataTable(TableName, syncAll, DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd")).GetDataSet();
        DataSet dt = SynchronizationController.GetInventoryRealTimeDataCount(lastModifiedOn);

        return dt.Tables[0].Rows.Count;
    }

    public static DataSet GetInventoryRealTimeDataCount(DateTime lastModifiedOn)
    {
        DataSet ds = new DataSet();

        try
        {
            string sqlString = "Select * from InventoryHdr where ModifiedOn > '" + lastModifiedOn.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";
            ds = DataService.GetDataSet(new QueryCommand(sqlString));

            return ds;

        }
        catch (Exception ex)
        { Logger.writeLog(ex.Message); return ds; }

    }

    public static DataSet GetInventoryRealTimeData(DateTime lastModifiedOn, int recordsPerTime)
    {
        DataSet ds = new DataSet();

        try
        {
            string sqlString = "Select top " + recordsPerTime.ToString() + " * from InventoryHdr where ModifiedOn > '" + lastModifiedOn.ToString("yyyy-MM-dd HH:mm:ss.fff") + "' order by modifiedon";
            ds = DataService.GetDataSet(new QueryCommand(sqlString));

            InventoryDetCollection idCol = new InventoryDetCollection();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                InventoryHdr ih = new InventoryHdr(dr["InventoryHdrRefNo"].ToString());
                InventoryDetCollection idColTemp = new InventoryDetCollection();
                sqlString = "Select * from InventoryDet where InventoryHdrRefNo ='" + dr["InventoryHdrRefNo"].ToString() + "'";
                DataTable dt = DataService.GetDataSet(new QueryCommand(sqlString)).Tables[0];
                idColTemp.Load(dt);
                idCol.AddRange(idColTemp);

            }
            ds.Tables.Add(idCol.ToDataTable());

            return ds;

        }
        catch (Exception ex)
        { Logger.writeLog(ex.Message); return ds; }

    }

    public static DataSet GetPromoRealTimeData(DateTime lastModifiedOn, int recordsPerTime, int PointOfSaleId)
    {
        DataSet ds = new DataSet();
        PointOfSale ps = new PointOfSale(PointOfSaleId);
        try
        {
            string sqlString = "Select top " + recordsPerTime.ToString() + " * from PromoCampaignHdr " +
                "WHERE ModifiedOn > '" + lastModifiedOn.ToString("yyyy-MM-dd HH:mm:ss.fff") + "' " +
                "AND PromoCampaignHdrID IN (Select PromoCampaignHdrID from PromoOutlet " +
                " WHERE OutletName = '" + ps.OutletName + "') " +
                "order by modifiedon";
            ds = DataService.GetDataSet(new QueryCommand(sqlString));

            PromoCampaignDetCollection idCol = new PromoCampaignDetCollection();
            PromoOutletCollection poCol = new PromoOutletCollection();
            PromoDaysMapCollection pdCol = new PromoDaysMapCollection();
            PromoLocationMapCollection plCol = new PromoLocationMapCollection();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                PromoCampaignHdr ih = new PromoCampaignHdr(dr["PromoCampaignHdrID"].ToString());
                PromoCampaignDetCollection idColTemp = new PromoCampaignDetCollection();
                sqlString = "Select * from PromoCampaignDet where PromoCampaignHdrID ='" + dr["PromoCampaignHdrID"].ToString() + "'";
                DataTable dt = DataService.GetDataSet(new QueryCommand(sqlString)).Tables[0];
                idColTemp.Load(dt);
                idCol.AddRange(idColTemp);

                PromoOutletCollection poColTemp = new PromoOutletCollection();
                sqlString = "Select * from PromoOutlet where PromoCampaignHdrID ='" + dr["PromoCampaignHdrID"].ToString() + "' and OutletName = '" + ps.OutletName + "'";
                DataTable dt1 = DataService.GetDataSet(new QueryCommand(sqlString)).Tables[0];
                poColTemp.Load(dt1);
                poCol.AddRange(poColTemp);

                PromoDaysMapCollection pdColTemp = new PromoDaysMapCollection();
                sqlString = "Select * from PromoDaysMap where PromoCampaignHdrID ='" + dr["PromoCampaignHdrID"].ToString() + "'";
                DataTable dt2 = DataService.GetDataSet(new QueryCommand(sqlString)).Tables[0];
                pdColTemp.Load(dt2);
                pdCol.AddRange(pdColTemp);

                PromoLocationMapCollection plColTemp = new PromoLocationMapCollection();
                sqlString = "Select * from PromoLocationMap where PromoCampaignHdrID ='" + dr["PromoCampaignHdrID"].ToString() + "'";
                DataTable dt3 = DataService.GetDataSet(new QueryCommand(sqlString)).Tables[0];
                plColTemp.Load(dt3);
                plCol.AddRange(plColTemp);
            }
            ds.Tables.Add(idCol.ToDataTable());
            ds.Tables.Add(poCol.ToDataTable());
            ds.Tables.Add(pdCol.ToDataTable());
            ds.Tables.Add(plCol.ToDataTable());

            return ds;

        }
        catch (Exception ex)
        { Logger.writeLog(ex.Message); return ds; }

    }

    public static int GetCountPromoRealTimeDataAllNEW(DateTime lastModifiedOn, int count)
    {
        DataTable ds = new DataTable();
        //PointOfSale ps = new PointOfSale(PointOfSaleId);
        try
        {
            string sqlString = @"DECLARE @EndDate DATETIME;
                                DECLARE @Row INT;

                                SET @EndDate = '{0}';
                                SET @Row = {1};
                                Select PromoCampaignHdr.* from PromoCampaignHdr 
                                INNER JOIN (
                                SELECT PromoCampaignHdrID TheKey
                                        ,ModifiedOn TheModifiedOn
                                        ,DENSE_RANK() OVER(ORDER BY ModifiedOn) TheNo
                                FROM PromoCampaignHdr 
                                WHERE ModifiedOn > @EndDate    
                                ) TAB ON TAB.TheKey = PromoCampaignHdr.PromoCampaignHdrID
                                WHERE TAB.TheNo <= @Row
                                ORDER BY TAB.TheNo, TAB.TheModifiedOn, TAB.TheKey";

            sqlString = string.Format(sqlString, lastModifiedOn.ToString("yyyy-MM-dd HH:mm:ss:fff"), count);
            ds = DataService.GetDataSet(new QueryCommand(sqlString)).Tables[0];

            return ds.Rows.Count;

        }
        catch (Exception ex)
        { Logger.writeLog(ex.Message); return 0; }

    }

    public static DataSet GetPromoRealTimeDataAllNEW(DateTime lastModifiedOn, int recordsPerTime)
    {
        DataSet ds = new DataSet();
        //PointOfSale ps = new PointOfSale(PointOfSaleId);
        try
        {
            string sqlString = @"DECLARE @EndDate DATETIME;
                                DECLARE @Row INT;

                                SET @EndDate = '{0}';
                                SET @Row = {1};
                                Select PromoCampaignHdr.* from PromoCampaignHdr 
                                INNER JOIN (
                                SELECT PromoCampaignHdrID TheKey
                                        ,ModifiedOn TheModifiedOn
                                        ,DENSE_RANK() OVER(ORDER BY ModifiedOn) TheNo
                                FROM PromoCampaignHdr 
                                WHERE ModifiedOn > @EndDate    
                                ) TAB ON TAB.TheKey = PromoCampaignHdr.PromoCampaignHdrID
                                WHERE TAB.TheNo <= @Row
                                ORDER BY TAB.TheNo, TAB.TheModifiedOn, TAB.TheKey";

            sqlString = string.Format(sqlString, lastModifiedOn.ToString("yyyy-MM-dd HH:mm:ss:fff"), recordsPerTime);
            ds = DataService.GetDataSet(new QueryCommand(sqlString));

            PromoCampaignDetCollection idCol = new PromoCampaignDetCollection();
            PromoOutletCollection poCol = new PromoOutletCollection();
            PromoDaysMapCollection pdCol = new PromoDaysMapCollection();
            PromoLocationMapCollection plCol = new PromoLocationMapCollection();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                PromoCampaignHdr ih = new PromoCampaignHdr(dr["PromoCampaignHdrID"].ToString());
                PromoCampaignDetCollection idColTemp = new PromoCampaignDetCollection();
                sqlString = "Select * from PromoCampaignDet where PromoCampaignHdrID ='" + dr["PromoCampaignHdrID"].ToString() + "'";
                DataTable dt = DataService.GetDataSet(new QueryCommand(sqlString)).Tables[0];
                idColTemp.Load(dt);
                idCol.AddRange(idColTemp);

                PromoOutletCollection poColTemp = new PromoOutletCollection();
                sqlString = "Select * from PromoOutlet where PromoCampaignHdrID ='" + dr["PromoCampaignHdrID"].ToString() + "' "; // and OutletName = '" + ps.OutletName + "'";
                DataTable dt1 = DataService.GetDataSet(new QueryCommand(sqlString)).Tables[0];
                poColTemp.Load(dt1);
                poCol.AddRange(poColTemp);

                PromoDaysMapCollection pdColTemp = new PromoDaysMapCollection();
                sqlString = "Select * from PromoDaysMap where PromoCampaignHdrID ='" + dr["PromoCampaignHdrID"].ToString() + "'";
                DataTable dt2 = DataService.GetDataSet(new QueryCommand(sqlString)).Tables[0];
                pdColTemp.Load(dt2);
                pdCol.AddRange(pdColTemp);

                PromoLocationMapCollection plColTemp = new PromoLocationMapCollection();
                sqlString = "Select * from PromoLocationMap where PromoCampaignHdrID ='" + dr["PromoCampaignHdrID"].ToString() + "'";
                DataTable dt3 = DataService.GetDataSet(new QueryCommand(sqlString)).Tables[0];
                plColTemp.Load(dt3);
                plCol.AddRange(plColTemp);
            }
            ds.Tables.Add(idCol.ToDataTable());
            ds.Tables.Add(poCol.ToDataTable());
            ds.Tables.Add(pdCol.ToDataTable());
            ds.Tables.Add(plCol.ToDataTable());

            return ds;

        }
        catch (Exception ex)
        { Logger.writeLog(ex.Message); return ds; }

    }

    public static DataSet GetPromoRealTimeDataAll(DateTime lastModifiedOn, int recordsPerTime)
    {
        DataSet ds = new DataSet();
        //PointOfSale ps = new PointOfSale(PointOfSaleId);
        try
        {
            string sqlString = "Select top " + recordsPerTime.ToString() + " * from PromoCampaignHdr " +
                "WHERE ModifiedOn > '" + lastModifiedOn.ToString("yyyy-MM-dd HH:mm:ss.fff") + "' " +
                //"AND PromoCampaignHdrID IN (Select PromoCampaignHdrID from PromoOutlet " +
                //" WHERE OutletName = '" + ps.OutletName + "') " +
                "order by modifiedon";
            ds = DataService.GetDataSet(new QueryCommand(sqlString));

            PromoCampaignDetCollection idCol = new PromoCampaignDetCollection();
            PromoOutletCollection poCol = new PromoOutletCollection();
            PromoDaysMapCollection pdCol = new PromoDaysMapCollection();
            PromoLocationMapCollection plCol = new PromoLocationMapCollection();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                PromoCampaignHdr ih = new PromoCampaignHdr(dr["PromoCampaignHdrID"].ToString());
                PromoCampaignDetCollection idColTemp = new PromoCampaignDetCollection();
                sqlString = "Select * from PromoCampaignDet where PromoCampaignHdrID ='" + dr["PromoCampaignHdrID"].ToString() + "'";
                DataTable dt = DataService.GetDataSet(new QueryCommand(sqlString)).Tables[0];
                idColTemp.Load(dt);
                idCol.AddRange(idColTemp);

                PromoOutletCollection poColTemp = new PromoOutletCollection();
                sqlString = "Select * from PromoOutlet where PromoCampaignHdrID ='" + dr["PromoCampaignHdrID"].ToString() + "' "; // and OutletName = '" + ps.OutletName + "'";
                DataTable dt1 = DataService.GetDataSet(new QueryCommand(sqlString)).Tables[0];
                poColTemp.Load(dt1);
                poCol.AddRange(poColTemp);

                PromoDaysMapCollection pdColTemp = new PromoDaysMapCollection();
                sqlString = "Select * from PromoDaysMap where PromoCampaignHdrID ='" + dr["PromoCampaignHdrID"].ToString() + "'";
                DataTable dt2 = DataService.GetDataSet(new QueryCommand(sqlString)).Tables[0];
                pdColTemp.Load(dt2);
                pdCol.AddRange(pdColTemp);

                PromoLocationMapCollection plColTemp = new PromoLocationMapCollection();
                sqlString = "Select * from PromoLocationMap where PromoCampaignHdrID ='" + dr["PromoCampaignHdrID"].ToString() + "'";
                DataTable dt3 = DataService.GetDataSet(new QueryCommand(sqlString)).Tables[0];
                plColTemp.Load(dt3);
                plCol.AddRange(plColTemp);
            }
            ds.Tables.Add(idCol.ToDataTable());
            ds.Tables.Add(poCol.ToDataTable());
            ds.Tables.Add(pdCol.ToDataTable());
            ds.Tables.Add(plCol.ToDataTable());

            return ds;

        }
        catch (Exception ex)
        { Logger.writeLog(ex.Message); return ds; }

    }

    public static DataSet GetPurchaseOrderRealTimeData(DateTime lastModifiedOn, int recordsPerTime, int InventoryLocationID)
    {
        DataSet ds = new DataSet();
        InventoryLocation ps = new InventoryLocation(InventoryLocationID);
        try
        {
            string sqlString = "Select top " + recordsPerTime.ToString() + " * from PurchaseOrderHdr " +
                "WHERE ModifiedOn > '" + lastModifiedOn.ToString("yyyy-MM-dd HH:mm:ss.fff") + "' ";
            if (InventoryLocationID != 0)
                sqlString = sqlString + " AND InventorylocationID = " + InventoryLocationID + " ";
            sqlString = sqlString + "order by modifiedon";
            ds = DataService.GetDataSet(new QueryCommand(sqlString));

            PurchaseOrderDetCollection poCol = new PurchaseOrderDetCollection();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                PurchaseOrderHdr ih = new PurchaseOrderHdr(dr["PurchaseOrderHdrRefNo"].ToString());
                PurchaseOrderDetCollection idColTemp = new PurchaseOrderDetCollection();
                sqlString = "Select * from PurchaseOrderDet where PurchaseOrderHdrRefNo ='" + dr["PurchaseOrderHdrRefNo"].ToString() + "'";
                DataTable dt = DataService.GetDataSet(new QueryCommand(sqlString)).Tables[0];
                idColTemp.Load(dt);
                poCol.AddRange(idColTemp);


            }
            ds.Tables.Add(poCol.ToDataTable());

            return ds;

        }
        catch (Exception ex)
        { Logger.writeLog(ex.Message); return ds; }

    }

    public static DataSet GetItemGroupRealTimeData(DateTime lastModifiedOn, int recordsPerTime)
    {
        DataSet ds = new DataSet();

        try
        {
            string sqlString = "Select top " + recordsPerTime.ToString() + " * from ItemGroup where ModifiedOn > '" + lastModifiedOn.ToString("yyyy-MM-dd HH:mm:ss.fff") + "' order by modifiedon";
            ds = DataService.GetDataSet(new QueryCommand(sqlString));

            ItemGroupMapCollection idCol = new ItemGroupMapCollection();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ItemGroup ih = new ItemGroup(dr["ItemGroupID"].ToString());
                ItemGroupMapCollection idColTemp = new ItemGroupMapCollection();
                sqlString = "Select * from ItemGroupMap where ItemGroupID ='" + dr["ItemGroupID"].ToString() + "'";
                DataTable dt = DataService.GetDataSet(new QueryCommand(sqlString)).Tables[0];
                idColTemp.Load(dt);
                idCol.AddRange(idColTemp);


            }
            ds.Tables.Add(idCol.ToDataTable());


            return ds;

        }
        catch (Exception ex)
        { Logger.writeLog(ex.Message); return ds; }

    }

    public static DataSet GetStockTakeRealTimeData(DateTime lastModifiedOn, int recordsPerTime)
    {
        DataSet ds = new DataSet();

        try
        {
            //stock take
            string sqlString = "Select * from StockTake where ModifiedOn > '" + lastModifiedOn.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";
            DataTable dt1 = DataService.GetDataSet(new QueryCommand(sqlString)).Tables[0];
            StockTakeCollection stCol = new StockTakeCollection();
            stCol.Load(dt1);
            ds.Tables.Add(stCol.ToDataTable());
            return ds;

        }
        catch (Exception ex)
        { Logger.writeLog(ex.Message); return ds; }

    }

    public static DataSet GetPointsRealTimeDataAll(DateTime lastModifiedOn, int recordsPerTime, string membershipNo)
    {
        DataSet ds = new DataSet();
        try
        {
            string sqlString = "SELECT TOP {0} * FROM MembershipPoints WHERE ModifiedOn > '{1}' AND MembershipNo = '{2}' ORDER BY ModifiedOn";
            sqlString = string.Format(sqlString,
                                        recordsPerTime.ToString(),
                                        lastModifiedOn.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                                        membershipNo);
            ds = DataService.GetDataSet(new QueryCommand(sqlString, "PowerPOS"));

            DataTable dt = new DataTable();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                PointAllocationLogCollection idColTemp = new PointAllocationLogCollection();
                sqlString = "SELECT * FROM PointAllocationLog WHERE MembershipNo ='{0}' AND userfld1 = '{1}' AND ModifiedOn > '{2}'";
                sqlString = string.Format(sqlString,
                                            membershipNo,
                                            dr["userfld1"].ToString(),
                                            lastModifiedOn.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                DataTable dtTemp = DataService.GetDataSet(new QueryCommand(sqlString, "PowerPOS")).Tables[0];
                dt.Merge(dtTemp, false, MissingSchemaAction.Add);
            }
            DataTable distinctTable = dt.DefaultView.ToTable(true);
            ds.Tables.Add(distinctTable);

            return ds;
        }
        catch (Exception ex)
        { Logger.writeLog(ex.Message); return ds; }

    }

    private static QueryCommandCollection FetchQuotationHdr(string[][] objHdr)
    {
        try
        {
            Query qry;
            Object tmpGuid;
            ArrayList duplicate = new ArrayList();
            QuotationHdr ord = new QuotationHdr();
            QueryCommand qr = ord.GetInsertCommand("SYNC");
            int UniqueIDIndex;
            UniqueIDIndex = -1;
            for (int f = 0; f < qr.Parameters.Count; f++)
            {
                if (qr.Parameters[f].ParameterName.ToLower() == "@uniqueid")
                {
                    UniqueIDIndex = f;
                    break;
                }
            }
            if (UniqueIDIndex == -1)
            {
                Logger.writeLog("Sync table does not contain unique ID or unique ID name does not compy.");
                throw new Exception("Sync table does not contain unique ID or unique ID name does not compy.");
            }

            QueryCommandCollection col = new QueryCommandCollection();
            ArrayList allList = new ArrayList();
            for (int i = 0; i < objHdr.Length; i++)
            {
                allList.Add(objHdr[i][0]);
            }

            //Check GUID uniqueness in Target DB
            qry = new Query("QuotationHdr");
            qry.SelectList = "OrderHdrID,UniqueID";
            DataTable dtUniqueID = qry.IN(QuotationHdr.Columns.OrderHdrID, allList).ExecuteDataSet().Tables[0];

            for (int i = 0; i < objHdr.Length; i++)
            {
                if (dtUniqueID.Rows.Count > 0)
                {
                    DataRow[] dr = dtUniqueID.Select("OrderHdrID = '" + objHdr[i][0] + "'");

                    if (dr.Length > 0)
                        tmpGuid = new Guid(dr[0]["UniqueID"].ToString());
                    else
                        tmpGuid = new Object();
                }
                else tmpGuid = new Object();

                if (dtUniqueID.Rows.Count > 0 && tmpGuid is Guid) //GUID Found?
                {
                    qr = ord.GetInsertCommand("SYNC");
                    for (int oa = 0; oa < qr.Parameters.Count; oa++)
                    {
                        if (oa >= objHdr[i].Length) continue;
                        switch (qr.Parameters[oa].DataType)
                        {
                            case DbType.Boolean:
                                if (objHdr[i][oa] == "0")
                                    qr.Parameters[oa].ParameterValue = false;
                                else if (objHdr[i][oa] == "1")
                                    qr.Parameters[oa].ParameterValue = true;
                                break;
                            case DbType.Date:
                            case DbType.DateTime:
                                if (objHdr[i][oa] != string.Empty)
                                {
                                    qr.Parameters[oa].ParameterValue = DateTime.Parse(objHdr[i][oa]);
                                    Logger.writeLog(">>>" + objHdr[i][oa]);
                                }
                                break;
                            case DbType.Decimal:
                            case DbType.Currency:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Decimal.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Double:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Double.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Single:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Single.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Guid:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = new Guid(objHdr[i][oa]);
                                break;
                            case DbType.Int16:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int16.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Int32:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int32.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Int64:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int64.Parse(objHdr[i][oa]);
                                break;
                            case DbType.UInt16:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt16.Parse(objHdr[i][oa]);
                                break;
                            case DbType.UInt32:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt32.Parse(objHdr[i][oa]);
                                break;
                            case DbType.UInt64:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt64.Parse(objHdr[i][oa]);
                                break;
                            case DbType.StringFixedLength:
                            case DbType.String:
                                qr.Parameters[oa].ParameterValue = objHdr[i][oa];
                                break;
                            default:
                                qr.Parameters[oa].ParameterValue = objHdr[i][oa];
                                break;
                        }

                    }
                    ord = new QuotationHdr("OrderHdrID", objHdr[i][0]);
                    ord.IsVoided = (bool)qr.Parameters.GetParameter("@IsVoided").ParameterValue;
                    ord.ModifiedBy = (string)qr.Parameters.GetParameter("@ModifiedBy").ParameterValue;
                    ord.ModifiedOn = (DateTime)qr.Parameters.GetParameter("@ModifiedOn").ParameterValue;


                    QueryCommand qrUpdate = ord.GetUpdateCommand("SYNC");
                    if (qrUpdate.CommandSql.IndexOf("[ModifiedBy] = @ModifiedBy, [ModifiedOn] = @ModifiedOn,") > 0)
                    {
                        qrUpdate.CommandSql = qrUpdate.CommandSql.Remove(qrUpdate.CommandSql.IndexOf("[ModifiedBy] = @ModifiedBy, [ModifiedOn] = @ModifiedOn,"), 55);
                        qrUpdate.Parameters.GetParameter("@ModifiedOn").ParameterValue = ord.ModifiedOn;
                    }
                    else
                    {
                        qrUpdate.CommandSql = qrUpdate.CommandSql.Remove(qrUpdate.CommandSql.IndexOf(", [ModifiedOn] = @ModifiedOn"), 28);
                        qrUpdate.Parameters.GetParameter("@ModifiedOn").ParameterValue = ord.ModifiedOn;
                    }

                    col.Add(qrUpdate);
                }
                else
                {
                    //New Item. Perform Create
                    qr = ord.GetInsertCommand("SYNC");
                    for (int oa = 0; oa < qr.Parameters.Count; oa++)
                    {
                        if (oa >= objHdr[i].Length) continue;
                        switch (qr.Parameters[oa].DataType)
                        {
                            case DbType.Boolean:
                                if (objHdr[i][oa] == "0")
                                    qr.Parameters[oa].ParameterValue = false;
                                else if (objHdr[i][oa] == "1")
                                    qr.Parameters[oa].ParameterValue = true;
                                break;
                            case DbType.Date:
                            case DbType.DateTime:
                                if (objHdr[i][oa] != string.Empty)
                                {
                                    qr.Parameters[oa].ParameterValue = DateTime.Parse(objHdr[i][oa]);
                                    Logger.writeLog(">>>" + objHdr[i][oa]);
                                }
                                break;
                            case DbType.Decimal:
                            case DbType.Currency:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Decimal.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Double:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Double.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Single:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Single.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Guid:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = new Guid(objHdr[i][oa]);
                                break;
                            case DbType.Int16:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int16.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Int32:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int32.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Int64:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int64.Parse(objHdr[i][oa]);
                                break;
                            case DbType.UInt16:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt16.Parse(objHdr[i][oa]);
                                break;
                            case DbType.UInt32:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt32.Parse(objHdr[i][oa]);
                                break;
                            case DbType.UInt64:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt64.Parse(objHdr[i][oa]);
                                break;
                            case DbType.StringFixedLength:
                            case DbType.String:
                                qr.Parameters[oa].ParameterValue = objHdr[i][oa];
                                break;
                            default:
                                qr.Parameters[oa].ParameterValue = objHdr[i][oa];
                                break;
                        }
                    }
                    col.Add(qr);
                }
            }
            if (duplicate.Count != 0)
            {
                string str;
                str = "Error: Attempting to enter duplicate Quotation Header Ref Number.";
                str = str + "\r\n" + "Duplicates:" + "\r\n";
                for (int i = 0; i < duplicate.Count; i++)
                    str += duplicate[i] + "\r\n";

                Logger.writeLog(str);
                throw new Exception("Duplicate found");
            }

            return col;

        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
            throw ex;
        }
    }

    private static QueryCommandCollection FetchCashRecording(string[][] objHdr)
    {
        try
        {
            Query qry;
            Object tmpGuid;
            ArrayList duplicate = new ArrayList();
            CashRecording ord = new CashRecording();
            QueryCommand qr = ord.GetInsertCommand("SYNC");
            int UniqueIDIndex;
            UniqueIDIndex = -1;
            for (int f = 0; f < qr.Parameters.Count; f++)
            {
                if (qr.Parameters[f].ParameterName.ToLower() == "@uniqueid")
                {
                    UniqueIDIndex = f;
                    break;
                }
            }
            if (UniqueIDIndex == -1)
            {
                Logger.writeLog("Sync table does not contain unique ID or unique ID name does not compy.");
                throw new Exception("Sync table does not contain unique ID or unique ID name does not compy.");
            }

            QueryCommandCollection col = new QueryCommandCollection();
            ArrayList allList = new ArrayList();
            for (int i = 0; i < objHdr.Length; i++)
            {
                allList.Add(objHdr[i][0]);
            }

            //Check GUID uniqueness in Target DB
            qry = new Query("CashRecording");
            qry.SelectList = "CashRecRefNo,UniqueID";
            DataTable dtUniqueID = qry.IN(CashRecording.Columns.CashRecRefNo, allList).ExecuteDataSet().Tables[0];

            for (int i = 0; i < objHdr.Length; i++)
            {
                if (dtUniqueID.Rows.Count > 0)
                {
                    DataRow[] dr = dtUniqueID.Select("CashRecRefNo = '" + objHdr[i][0] + "'");

                    if (dr.Length > 0)
                        tmpGuid = new Guid(dr[0]["UniqueID"].ToString());
                    else
                        tmpGuid = new Object();
                }
                else tmpGuid = new Object();

                if (dtUniqueID.Rows.Count > 0 && tmpGuid is Guid) //GUID Found?
                {
                    //Existing Item. Perform Update
                    //Skip this process
                }
                else
                {
                    //New Item. Perform Create
                    qr = ord.GetInsertCommand("SYNC");
                    for (int oa = 0; oa < qr.Parameters.Count; oa++)
                    {
                        if (oa >= objHdr[i].Length) continue;
                        switch (qr.Parameters[oa].DataType)
                        {
                            case DbType.Boolean:
                                if (objHdr[i][oa] == "0")
                                    qr.Parameters[oa].ParameterValue = false;
                                else if (objHdr[i][oa] == "1")
                                    qr.Parameters[oa].ParameterValue = true;
                                break;
                            case DbType.Date:
                            case DbType.DateTime:
                                if (objHdr[i][oa] != string.Empty)
                                {
                                    qr.Parameters[oa].ParameterValue = DateTime.Parse(objHdr[i][oa]);
                                    Logger.writeLog(">>>" + objHdr[i][oa]);
                                }
                                break;
                            case DbType.Decimal:
                            case DbType.Currency:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Decimal.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Double:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Double.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Single:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Single.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Guid:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = new Guid(objHdr[i][oa]);
                                break;
                            case DbType.Int16:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int16.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Int32:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int32.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Int64:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int64.Parse(objHdr[i][oa]);
                                break;
                            case DbType.UInt16:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt16.Parse(objHdr[i][oa]);
                                break;
                            case DbType.UInt32:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt32.Parse(objHdr[i][oa]);
                                break;
                            case DbType.UInt64:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt64.Parse(objHdr[i][oa]);
                                break;
                            case DbType.StringFixedLength:
                            case DbType.String:
                                qr.Parameters[oa].ParameterValue = objHdr[i][oa];
                                break;
                            default:
                                qr.Parameters[oa].ParameterValue = objHdr[i][oa];
                                break;
                        }
                    }
                    col.Add(qr);
                }
            }
            if (duplicate.Count != 0)
            {
                string str;
                str = "Error: Attempting to enter duplicate CashRecording ID.";
                str = str + "\r\n" + "Duplicates:" + "\r\n";
                for (int i = 0; i < duplicate.Count; i++)
                    str += duplicate[i] + "\r\n";

                Logger.writeLog(str);
                throw new Exception("Duplicate found");
            }

            return col;

        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
            throw ex;
        }
    }

    private static QueryCommandCollection FetchPerformanceLog(string[][] objHdr)
    {
        try
        {
            Query qry;
            Object tmpGuid;
            ArrayList duplicate = new ArrayList();
            PerformanceLog ord = new PerformanceLog();
            QueryCommand qr = ord.GetInsertCommand("SYNC");
            int UniqueIDIndex;
            UniqueIDIndex = -1;
            for (int f = 0; f < qr.Parameters.Count; f++)
            {
                if (qr.Parameters[f].ParameterName.ToLower() == "@performancelogid")
                {
                    UniqueIDIndex = f;
                    break;
                }
            }
            if (UniqueIDIndex == -1)
            {
                Logger.writeLog("Sync table does not contain unique ID or unique ID name does not compy.");
                throw new Exception("Sync table does not contain unique ID or unique ID name does not compy.");
            }

            QueryCommandCollection col = new QueryCommandCollection();
            ArrayList allList = new ArrayList();
            for (int i = 0; i < objHdr.Length; i++)
            {
                allList.Add(objHdr[i][0]);
            }

            //Check GUID uniqueness in Target DB
            qry = new Query("PerformanceLog");
            qry.SelectList = "PerformanceLogID";
            DataTable dtUniqueID = qry.IN(PerformanceLog.Columns.PerformanceLogID, allList).ExecuteDataSet().Tables[0];

            List<string> deleteList = new List<string>();

            for (int i = 0; i < objHdr.Length; i++)
            {
                if (dtUniqueID.Rows.Count > 0)
                {
                    DataRow[] dr = dtUniqueID.Select("PerformanceLogID = '" + objHdr[i][0] + "'");

                    if (dr.Length > 0)
                        tmpGuid = new Guid(dr[0]["PerformanceLogID"].ToString());
                    else
                        tmpGuid = new Object();
                }
                else tmpGuid = new Object();

                if (dtUniqueID.Rows.Count > 0 && tmpGuid is Guid) //GUID Found?
                {
                    //Existing Item. Perform Update
                    //Skip this process
                }
                else
                {
                    //New Item. Perform Create
                    qr = ord.GetInsertCommand("SYNC");
                    for (int oa = 0; oa < qr.Parameters.Count; oa++)
                    {
                        if (oa >= objHdr[i].Length) continue;
                        switch (qr.Parameters[oa].DataType)
                        {
                            case DbType.Boolean:
                                if (objHdr[i][oa] == "0")
                                    qr.Parameters[oa].ParameterValue = false;
                                else if (objHdr[i][oa] == "1")
                                    qr.Parameters[oa].ParameterValue = true;
                                break;
                            case DbType.Date:
                            case DbType.DateTime:
                                if (objHdr[i][oa] != string.Empty)
                                {
                                    qr.Parameters[oa].ParameterValue = DateTime.Parse(objHdr[i][oa]);
                                    Logger.writeLog(">>>" + objHdr[i][oa]);
                                }
                                break;
                            case DbType.Decimal:
                            case DbType.Currency:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Decimal.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Double:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Double.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Single:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Single.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Guid:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = new Guid(objHdr[i][oa]);
                                break;
                            case DbType.Int16:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int16.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Int32:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int32.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Int64:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int64.Parse(objHdr[i][oa]);
                                break;
                            case DbType.UInt16:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt16.Parse(objHdr[i][oa]);
                                break;
                            case DbType.UInt32:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt32.Parse(objHdr[i][oa]);
                                break;
                            case DbType.UInt64:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt64.Parse(objHdr[i][oa]);
                                break;
                            case DbType.StringFixedLength:
                            case DbType.String:
                                qr.Parameters[oa].ParameterValue = objHdr[i][oa];
                                break;
                            default:
                                qr.Parameters[oa].ParameterValue = objHdr[i][oa];
                                break;
                        }
                    }
                    col.Add(qr);

                    string tmpstr = qr.Parameters.GetParameter("@ModuleName").ParameterValue + ";" +
                                    qr.Parameters.GetParameter("@FunctionName").ParameterValue + ";" +
                                    qr.Parameters.GetParameter("@PointOfSaleID").ParameterValue;
                    if (!deleteList.Contains(tmpstr))
                        deleteList.Add(tmpstr);
                }
            }
            if (duplicate.Count != 0)
            {
                string str;
                str = "Error: Attempting to enter duplicate PerformanceLogID.";
                str = str + "\r\n" + "Duplicates:" + "\r\n";
                for (int i = 0; i < duplicate.Count; i++)
                    str += duplicate[i] + "\r\n";

                Logger.writeLog(str);
                throw new Exception("Duplicate found");
            }

            foreach (string str in deleteList)
            {
                string moduleName = str.Split(';')[0];
                string functionName = str.Split(';')[1];
                int pointOfSaleID;
                int.TryParse(str.Split(';')[2], out pointOfSaleID);

                QueryCommand cmd = PerformanceLogController.DeleteOldLogCommand(moduleName, functionName, pointOfSaleID);
                if (cmd != null)
                    col.Add(cmd);
            }

            return col;

        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
            throw ex;
        }
    }

    private static QueryCommandCollection FetchPerformanceLogSummary(string[][] objHdr)
    {
        try
        {
            Query qry;
            Object tmpGuid;
            ArrayList duplicate = new ArrayList();
            PerformanceLogSummary ord = new PerformanceLogSummary();
            QueryCommand qr = ord.GetInsertCommand("SYNC");
            int UniqueIDIndex;
            UniqueIDIndex = -1;
            for (int f = 0; f < qr.Parameters.Count; f++)
            {
                if (qr.Parameters[f].ParameterName.ToLower() == "@performancelogsummaryid")
                {
                    UniqueIDIndex = f;
                    break;
                }
            }
            if (UniqueIDIndex == -1)
            {
                Logger.writeLog("Sync table does not contain unique ID or unique ID name does not compy.");
                throw new Exception("Sync table does not contain unique ID or unique ID name does not compy.");
            }

            QueryCommandCollection col = new QueryCommandCollection();
            ArrayList allList = new ArrayList();
            for (int i = 0; i < objHdr.Length; i++)
            {
                allList.Add(objHdr[i][0]);
            }

            //Check GUID uniqueness in Target DB
            qry = new Query("PerformanceLogSummary");
            qry.SelectList = "PerformanceLogSummaryID";
            DataTable dtUniqueID = qry.IN(PerformanceLogSummary.Columns.PerformanceLogSummaryID, allList).ExecuteDataSet().Tables[0];

            for (int i = 0; i < objHdr.Length; i++)
            {
                if (dtUniqueID.Rows.Count > 0)
                {
                    DataRow[] dr = dtUniqueID.Select("PerformanceLogSummaryID = '" + objHdr[i][0] + "'");

                    if (dr.Length > 0)
                        tmpGuid = new Guid(dr[0]["PerformanceLogSummaryID"].ToString());
                    else
                        tmpGuid = new Object();
                }
                else tmpGuid = new Object();

                if (dtUniqueID.Rows.Count > 0 && tmpGuid is Guid) //GUID Found?
                {
                    //Existing Item. Perform Update
                    //Skip this process
                }
                else
                {
                    //New Item. Perform Create
                    qr = ord.GetInsertCommand("SYNC");
                    for (int oa = 0; oa < qr.Parameters.Count; oa++)
                    {
                        if (oa >= objHdr[i].Length) continue;
                        switch (qr.Parameters[oa].DataType)
                        {
                            case DbType.Boolean:
                                if (objHdr[i][oa] == "0")
                                    qr.Parameters[oa].ParameterValue = false;
                                else if (objHdr[i][oa] == "1")
                                    qr.Parameters[oa].ParameterValue = true;
                                break;
                            case DbType.Date:
                            case DbType.DateTime:
                                if (objHdr[i][oa] != string.Empty)
                                {
                                    qr.Parameters[oa].ParameterValue = DateTime.Parse(objHdr[i][oa]);
                                    Logger.writeLog(">>>" + objHdr[i][oa]);
                                }
                                break;
                            case DbType.Decimal:
                            case DbType.Currency:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Decimal.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Double:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Double.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Single:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Single.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Guid:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = new Guid(objHdr[i][oa]);
                                break;
                            case DbType.Int16:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int16.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Int32:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int32.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Int64:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int64.Parse(objHdr[i][oa]);
                                break;
                            case DbType.UInt16:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt16.Parse(objHdr[i][oa]);
                                break;
                            case DbType.UInt32:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt32.Parse(objHdr[i][oa]);
                                break;
                            case DbType.UInt64:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt64.Parse(objHdr[i][oa]);
                                break;
                            case DbType.StringFixedLength:
                            case DbType.String:
                                qr.Parameters[oa].ParameterValue = objHdr[i][oa];
                                break;
                            default:
                                qr.Parameters[oa].ParameterValue = objHdr[i][oa];
                                break;
                        }
                    }
                    col.Add(qr);
                }
            }
            if (duplicate.Count != 0)
            {
                string str;
                str = "Error: Attempting to enter duplicate PerformanceLogSummaryID.";
                str = str + "\r\n" + "Duplicates:" + "\r\n";
                for (int i = 0; i < duplicate.Count; i++)
                    str += duplicate[i] + "\r\n";

                Logger.writeLog(str);
                throw new Exception("Duplicate found");
            }

            return col;

        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
            throw ex;
        }
    }

    private static QueryCommandCollection FetchQuotationDet(string[][] objHdr)
    {
        try
        {
            Query qry;
            Object tmpGuid;
            ArrayList duplicate = new ArrayList();
            QuotationDet ord = new QuotationDet();
            QueryCommand qr = ord.GetInsertCommand("SYNC");
            int UniqueIDIndex;
            UniqueIDIndex = -1;
            for (int f = 0; f < qr.Parameters.Count; f++)
            {
                if (qr.Parameters[f].ParameterName.ToLower() == "@uniqueid")
                {
                    UniqueIDIndex = f;
                    break;
                }
            }
            if (UniqueIDIndex == -1)
            {
                Logger.writeLog("Sync table does not contain unique ID or unique ID name does not compy.");
                throw new Exception("Sync table does not contain unique ID or unique ID name does not compy.");
            }

            QueryCommandCollection col = new QueryCommandCollection();
            ArrayList allList = new ArrayList();
            for (int i = 0; i < objHdr.Length; i++)
            {
                allList.Add(objHdr[i][0]);
            }

            //Check GUID uniqueness in Target DB
            qry = new Query("QuotationDet");
            qry.SelectList = "OrderDetID,UniqueID";
            DataTable dtUniqueID = qry.IN(QuotationHdr.Columns.OrderHdrID, allList).ExecuteDataSet().Tables[0];

            for (int i = 0; i < objHdr.Length; i++)
            {
                if (dtUniqueID.Rows.Count > 0)
                {
                    DataRow[] dr = dtUniqueID.Select("OrderDetID = '" + objHdr[i][0] + "'");

                    if (dr.Length > 0)
                        tmpGuid = new Guid(dr[0]["UniqueID"].ToString());
                    else
                        tmpGuid = new Object();
                }
                else tmpGuid = new Object();

                if (dtUniqueID.Rows.Count > 0 && tmpGuid is Guid) //GUID Found?
                {
                    qr = ord.GetInsertCommand("SYNC");
                    for (int oa = 0; oa < qr.Parameters.Count; oa++)
                    {
                        if (oa >= objHdr[i].Length) continue;
                        switch (qr.Parameters[oa].DataType)
                        {
                            case DbType.Boolean:
                                if (objHdr[i][oa] == "0")
                                    qr.Parameters[oa].ParameterValue = false;
                                else if (objHdr[i][oa] == "1")
                                    qr.Parameters[oa].ParameterValue = true;
                                break;
                            case DbType.Date:
                            case DbType.DateTime:
                                if (objHdr[i][oa] != string.Empty)
                                {
                                    qr.Parameters[oa].ParameterValue = DateTime.Parse(objHdr[i][oa]);
                                    Logger.writeLog(">>>" + objHdr[i][oa]);
                                }
                                break;
                            case DbType.Decimal:
                            case DbType.Currency:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Decimal.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Double:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Double.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Single:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Single.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Guid:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = new Guid(objHdr[i][oa]);
                                break;
                            case DbType.Int16:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int16.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Int32:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int32.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Int64:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int64.Parse(objHdr[i][oa]);
                                break;
                            case DbType.UInt16:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt16.Parse(objHdr[i][oa]);
                                break;
                            case DbType.UInt32:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt32.Parse(objHdr[i][oa]);
                                break;
                            case DbType.UInt64:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt64.Parse(objHdr[i][oa]);
                                break;
                            case DbType.StringFixedLength:
                            case DbType.String:
                                qr.Parameters[oa].ParameterValue = objHdr[i][oa];
                                break;
                            default:
                                qr.Parameters[oa].ParameterValue = objHdr[i][oa];
                                break;
                        }

                    }
                    ord = new QuotationDet("OrderDetID", objHdr[i][0]);
                    ord.IsVoided = (bool)qr.Parameters.GetParameter("@IsVoided").ParameterValue;
                    ord.ModifiedBy = (string)qr.Parameters.GetParameter("@ModifiedBy").ParameterValue;
                    ord.ModifiedOn = (DateTime)qr.Parameters.GetParameter("@ModifiedOn").ParameterValue;


                    QueryCommand qrUpdate = ord.GetUpdateCommand("SYNC");
                    if (qrUpdate.CommandSql.IndexOf("[ModifiedBy] = @ModifiedBy, [ModifiedOn] = @ModifiedOn,") > 0)
                    {
                        qrUpdate.CommandSql = qrUpdate.CommandSql.Remove(qrUpdate.CommandSql.IndexOf("[ModifiedBy] = @ModifiedBy, [ModifiedOn] = @ModifiedOn,"), 55);
                        qrUpdate.Parameters.GetParameter("@ModifiedOn").ParameterValue = ord.ModifiedOn;
                    }
                    else
                    {
                        qrUpdate.CommandSql = qrUpdate.CommandSql.Remove(qrUpdate.CommandSql.IndexOf(", [ModifiedOn] = @ModifiedOn"), 28);
                        qrUpdate.Parameters.GetParameter("@ModifiedOn").ParameterValue = ord.ModifiedOn;
                    }

                    col.Add(qrUpdate);
                }
                else
                {
                    //New Item. Perform Create
                    qr = ord.GetInsertCommand("SYNC");
                    for (int oa = 0; oa < qr.Parameters.Count; oa++)
                    {
                        if (oa >= objHdr[i].Length) continue;
                        switch (qr.Parameters[oa].DataType)
                        {
                            case DbType.Boolean:
                                if (objHdr[i][oa] == "0")
                                    qr.Parameters[oa].ParameterValue = false;
                                else if (objHdr[i][oa] == "1")
                                    qr.Parameters[oa].ParameterValue = true;
                                break;
                            case DbType.Date:
                            case DbType.DateTime:
                                if (objHdr[i][oa] != string.Empty && objHdr[i][oa] != null)
                                {
                                    qr.Parameters[oa].ParameterValue = DateTime.Parse(objHdr[i][oa]);
                                    Logger.writeLog(">>>" + objHdr[i][oa]);
                                }
                                break;
                            case DbType.Decimal:
                            case DbType.Currency:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Decimal.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Double:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Double.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Single:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Single.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Guid:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = new Guid(objHdr[i][oa]);
                                break;
                            case DbType.Int16:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int16.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Int32:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int32.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Int64:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int64.Parse(objHdr[i][oa]);
                                break;
                            case DbType.UInt16:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt16.Parse(objHdr[i][oa]);
                                break;
                            case DbType.UInt32:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt32.Parse(objHdr[i][oa]);
                                break;
                            case DbType.UInt64:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt64.Parse(objHdr[i][oa]);
                                break;
                            case DbType.StringFixedLength:
                            case DbType.String:
                                qr.Parameters[oa].ParameterValue = objHdr[i][oa];
                                break;
                            default:
                                qr.Parameters[oa].ParameterValue = objHdr[i][oa];
                                break;
                        }
                    }
                    col.Add(qr);
                }
            }
            if (duplicate.Count != 0)
            {
                string str;
                str = "Error: Attempting to enter duplicate Quotation Det Ref Number.";
                str = str + "\r\n" + "Duplicates:" + "\r\n";
                for (int i = 0; i < duplicate.Count; i++)
                    str += duplicate[i] + "\r\n";

                Logger.writeLog(str);
                throw new Exception("Duplicate found");
            }

            return col;

        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
            throw ex;
        }
    }

    private static QueryCommandCollection FetchCounterCloseLog(string[][] objHdr)
    {
        try
        {
            Query qry;
            Object tmpGuid;


            ArrayList duplicate = new ArrayList();

            //ORDER HEADER
            //Logger.writeLog("#records>" + objHdr.Length);

            CounterCloseLog ord = new CounterCloseLog();

            QueryCommand qr = ord.GetInsertCommand("SYNC");
            int UniqueIDIndex;
            UniqueIDIndex = -1;
            for (int f = 0; f < qr.Parameters.Count; f++)
            {
                if (qr.Parameters[f].ParameterName.ToLower() == "@uniqueid")
                {
                    UniqueIDIndex = f;
                    break;
                }
            }
            if (UniqueIDIndex == -1)
            {
                Logger.writeLog("Sync table does not contain unique ID or unique ID name does not compy.");
                throw new Exception("Sync table does not contain unique ID or unique ID name does not compy.");
            }

            QueryCommandCollection col = new QueryCommandCollection();
            ArrayList allList = new ArrayList();
            for (int i = 0; i < objHdr.Length; i++)
            {
                allList.Add(objHdr[i][0]);
            }

            //Check GUID uniqueness in Target DB
            qry = new Query("CounterCloseLog");
            qry.SelectList = "CounterCloseID,UniqueID";
            DataTable dtUniqueID = qry.IN(CounterCloseLog.Columns.CounterCloseID, allList).ExecuteDataSet().Tables[0];

            for (int i = 0; i < objHdr.Length; i++)
            {
                if (dtUniqueID.Rows.Count > 0)
                {
                    DataRow[] dr = dtUniqueID.Select("CounterCloseID = '" + objHdr[i][0] + "'");

                    if (dr.Length > 0)
                        tmpGuid = new Guid(dr[0]["UniqueID"].ToString());
                    else
                        tmpGuid = new Object();
                }
                else tmpGuid = new Object();

                if (dtUniqueID.Rows.Count > 0 && tmpGuid is Guid) //GUID Found?
                {
                    qr = ord.GetInsertCommand("SYNC");
                    for (int oa = 0; oa < qr.Parameters.Count; oa++)
                    {
                        if (oa >= objHdr[i].Length) continue;
                        switch (qr.Parameters[oa].DataType)
                        {
                            case DbType.Boolean:
                                if (objHdr[i][oa] == "0")
                                    qr.Parameters[oa].ParameterValue = false;
                                else if (objHdr[i][oa] == "1")
                                    qr.Parameters[oa].ParameterValue = true;
                                break;
                            case DbType.Date:
                            case DbType.DateTime:
                                if (objHdr[i][oa] != string.Empty)
                                {
                                    qr.Parameters[oa].ParameterValue = DateTime.Parse(objHdr[i][oa]);
                                    Logger.writeLog(">>>" + objHdr[i][oa]);
                                }
                                break;
                            case DbType.Decimal:
                            case DbType.Currency:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Decimal.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Double:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Double.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Single:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Single.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Guid:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = new Guid(objHdr[i][oa]);
                                break;
                            case DbType.Int16:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int16.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Int32:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int32.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Int64:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int64.Parse(objHdr[i][oa]);
                                break;
                            case DbType.UInt16:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt16.Parse(objHdr[i][oa]);
                                break;
                            case DbType.UInt32:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt32.Parse(objHdr[i][oa]);
                                break;
                            case DbType.UInt64:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt64.Parse(objHdr[i][oa]);
                                break;
                            case DbType.StringFixedLength:
                            case DbType.String:
                                qr.Parameters[oa].ParameterValue = objHdr[i][oa];
                                break;
                            default:
                                qr.Parameters[oa].ParameterValue = objHdr[i][oa];
                                break;
                        }

                    }
                    ord = new CounterCloseLog("CounterCloseID", objHdr[i][0]);
                    ord.TotalSystemRecorded = (decimal)qr.Parameters.GetParameter("@TotalSystemRecorded").ParameterValue;
                    ord.TotalActualCollected = (decimal)qr.Parameters.GetParameter("@TotalActualCollected").ParameterValue;
                    ord.ModifiedBy = (string)qr.Parameters.GetParameter("@ModifiedBy").ParameterValue;
                    ord.ModifiedOn = (DateTime)qr.Parameters.GetParameter("@ModifiedOn").ParameterValue;


                    QueryCommand qrUpdate = ord.GetUpdateCommand("SYNC");
                    if (qrUpdate.CommandSql.IndexOf("[ModifiedBy] = @ModifiedBy, [ModifiedOn] = @ModifiedOn,") > 0)
                    {
                        qrUpdate.CommandSql = qrUpdate.CommandSql.Remove(qrUpdate.CommandSql.IndexOf("[ModifiedBy] = @ModifiedBy, [ModifiedOn] = @ModifiedOn,"), 55);
                        qrUpdate.Parameters.GetParameter("@ModifiedOn").ParameterValue = ord.ModifiedOn;
                    }
                    else
                    {
                        qrUpdate.CommandSql = qrUpdate.CommandSql.Remove(qrUpdate.CommandSql.IndexOf(", [ModifiedOn] = @ModifiedOn"), 28);
                        qrUpdate.Parameters.GetParameter("@ModifiedOn").ParameterValue = ord.ModifiedOn;
                    }

                    col.Add(qrUpdate);
                }
                else
                {
                    //New Item. Perform Create
                    qr = ord.GetInsertCommand("SYNC");
                    for (int oa = 0; oa < qr.Parameters.Count; oa++)
                    {
                        if (oa >= objHdr[i].Length) continue;
                        switch (qr.Parameters[oa].DataType)
                        {
                            case DbType.Boolean:
                                if (objHdr[i][oa] == "0")
                                    qr.Parameters[oa].ParameterValue = false;
                                else if (objHdr[i][oa] == "1")
                                    qr.Parameters[oa].ParameterValue = true;
                                break;
                            case DbType.Date:
                            case DbType.DateTime:
                                if (objHdr[i][oa] != string.Empty)
                                {
                                    qr.Parameters[oa].ParameterValue = DateTime.Parse(objHdr[i][oa]);
                                    Logger.writeLog(">>>" + objHdr[i][oa]);
                                }
                                break;
                            case DbType.Decimal:
                            case DbType.Currency:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Decimal.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Double:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Double.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Single:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Single.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Guid:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = new Guid(objHdr[i][oa]);
                                break;
                            case DbType.Int16:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int16.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Int32:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int32.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Int64:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int64.Parse(objHdr[i][oa]);
                                break;
                            case DbType.UInt16:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt16.Parse(objHdr[i][oa]);
                                break;
                            case DbType.UInt32:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt32.Parse(objHdr[i][oa]);
                                break;
                            case DbType.UInt64:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt64.Parse(objHdr[i][oa]);
                                break;
                            case DbType.StringFixedLength:
                            case DbType.String:
                                qr.Parameters[oa].ParameterValue = objHdr[i][oa];
                                break;
                            default:
                                qr.Parameters[oa].ParameterValue = objHdr[i][oa];
                                break;
                        }
                    }
                    col.Add(qr);
                }
            }
            if (duplicate.Count != 0)
            {
                string str;
                str = "Error: Attempting to enter duplicate Order Header Ref Number.";
                str = str + "\r\n" + "Duplicates:" + "\r\n";
                for (int i = 0; i < duplicate.Count; i++)
                    str += duplicate[i] + "\r\n";

                Logger.writeLog(str);
                throw new Exception("Duplicate found");
            }

            return col;

        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
            throw ex;
        }
    }

    private static QueryCommandCollection FetchCounterCloseDet(string[][] objHdr)
    {
        try
        {
            Query qry;
            Object tmpGuid;


            ArrayList duplicate = new ArrayList();

            //ORDER HEADER
            //Logger.writeLog("#records>" + objHdr.Length);

            CounterCloseDet ord = new CounterCloseDet();

            QueryCommand qr = ord.GetInsertCommand("SYNC");
            int UniqueIDIndex;
            UniqueIDIndex = -1;
            for (int f = 0; f < qr.Parameters.Count; f++)
            {
                if (qr.Parameters[f].ParameterName.ToLower() == "@uniqueid")
                {
                    UniqueIDIndex = f;
                    break;
                }
            }
            if (UniqueIDIndex == -1)
            {
                Logger.writeLog("Sync table does not contain unique ID or unique ID name does not compy.");
                throw new Exception("Sync table does not contain unique ID or unique ID name does not compy.");
            }

            QueryCommandCollection col = new QueryCommandCollection();
            ArrayList allList = new ArrayList();
            for (int i = 0; i < objHdr.Length; i++)
            {
                allList.Add(objHdr[i][0]);
            }

            //Check GUID uniqueness in Target DB
            qry = new Query("CounterCloseDet");
            qry.SelectList = "CounterCloseDetID,UniqueID";
            DataTable dtUniqueID = qry.IN(CounterCloseDet.Columns.CounterCloseID, allList).ExecuteDataSet().Tables[0];

            for (int i = 0; i < objHdr.Length; i++)
            {
                if (dtUniqueID.Rows.Count > 0)
                {
                    DataRow[] dr = dtUniqueID.Select("CounterCloseDetID = '" + objHdr[i][0] + "'");

                    if (dr.Length > 0)
                        tmpGuid = new Guid(dr[0]["UniqueID"].ToString());
                    else
                        tmpGuid = new Object();
                }
                else tmpGuid = new Object();

                if (dtUniqueID.Rows.Count > 0 && tmpGuid is Guid) //GUID Found?
                {
                    qr = ord.GetInsertCommand("SYNC");
                    for (int oa = 0; oa < qr.Parameters.Count; oa++)
                    {

                        if (oa >= objHdr[i].Length) continue;
                        switch (qr.Parameters[oa].DataType)
                        {
                            case DbType.Boolean:
                                if (objHdr[i][oa] == "0")
                                    qr.Parameters[oa].ParameterValue = false;
                                else if (objHdr[i][oa] == "1")
                                    qr.Parameters[oa].ParameterValue = true;
                                break;
                            case DbType.Date:
                            case DbType.DateTime:
                                if (objHdr[i][oa] != string.Empty)
                                {
                                    qr.Parameters[oa].ParameterValue = DateTime.Parse(objHdr[i][oa]);
                                    Logger.writeLog(">>>" + objHdr[i][oa]);
                                }
                                break;
                            case DbType.Decimal:
                            case DbType.Currency:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Decimal.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Double:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Double.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Single:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Single.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Guid:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = new Guid(objHdr[i][oa]);
                                break;
                            case DbType.Int16:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int16.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Int32:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int32.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Int64:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int64.Parse(objHdr[i][oa]);
                                break;
                            case DbType.UInt16:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt16.Parse(objHdr[i][oa]);
                                break;
                            case DbType.UInt32:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt32.Parse(objHdr[i][oa]);
                                break;
                            case DbType.UInt64:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt64.Parse(objHdr[i][oa]);
                                break;
                            case DbType.StringFixedLength:
                            case DbType.String:
                                qr.Parameters[oa].ParameterValue = objHdr[i][oa];
                                break;
                            default:
                                qr.Parameters[oa].ParameterValue = objHdr[i][oa];
                                break;
                        }

                    }
                    ord = new CounterCloseDet("CounterCloseDetID", objHdr[i][0]);
                    ord.TotalCount = (int)qr.Parameters.GetParameter("@TotalCount").ParameterValue;
                    ord.TotalAmount = (decimal)qr.Parameters.GetParameter("@TotalAmount").ParameterValue;
                    ord.ModifiedBy = (string)qr.Parameters.GetParameter("@ModifiedBy").ParameterValue;
                    ord.ModifiedOn = (DateTime)qr.Parameters.GetParameter("@ModifiedOn").ParameterValue;


                    QueryCommand qrUpdate = ord.GetUpdateCommand("SYNC");
                    if (qrUpdate.CommandSql.IndexOf("[ModifiedBy] = @ModifiedBy, [ModifiedOn] = @ModifiedOn,") > 0)
                    {
                        qrUpdate.CommandSql = qrUpdate.CommandSql.Remove(qrUpdate.CommandSql.IndexOf("[ModifiedBy] = @ModifiedBy, [ModifiedOn] = @ModifiedOn,"), 55);
                        qrUpdate.Parameters.GetParameter("@ModifiedOn").ParameterValue = ord.ModifiedOn;
                    }
                    else
                    {
                        qrUpdate.CommandSql = qrUpdate.CommandSql.Remove(qrUpdate.CommandSql.IndexOf(", [ModifiedOn] = @ModifiedOn"), 28);
                        qrUpdate.Parameters.GetParameter("@ModifiedOn").ParameterValue = ord.ModifiedOn;
                    }

                    col.Add(qrUpdate);
                }
                else
                {
                    //New Item. Perform Create
                    qr = ord.GetInsertCommand("SYNC");
                    for (int oa = 0; oa < qr.Parameters.Count; oa++)
                    {
                        
                            if (oa >= objHdr[i].Length) continue;
                            switch (qr.Parameters[oa].DataType)
                            {
                                case DbType.Boolean:
                                    if (objHdr[i][oa] == "0")
                                        qr.Parameters[oa].ParameterValue = false;
                                    else if (objHdr[i][oa] == "1")
                                        qr.Parameters[oa].ParameterValue = true;
                                    break;
                                case DbType.Date:
                                case DbType.DateTime:
                                    if (objHdr[i][oa] != string.Empty)
                                    {
                                        qr.Parameters[oa].ParameterValue = DateTime.Parse(objHdr[i][oa]);
                                        Logger.writeLog(">>>" + objHdr[i][oa]);
                                    }
                                    break;
                                case DbType.Decimal:
                                case DbType.Currency:
                                    if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Decimal.Parse(objHdr[i][oa]);
                                    break;
                                case DbType.Double:
                                    if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Double.Parse(objHdr[i][oa]);
                                    break;
                                case DbType.Single:
                                    if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Single.Parse(objHdr[i][oa]);
                                    break;
                                case DbType.Guid:
                                    if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = new Guid(objHdr[i][oa]);
                                    break;
                                case DbType.Int16:
                                    if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int16.Parse(objHdr[i][oa]);
                                    break;
                                case DbType.Int32:
                                    if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int32.Parse(objHdr[i][oa]);
                                    break;
                                case DbType.Int64:
                                    if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int64.Parse(objHdr[i][oa]);
                                    break;
                                case DbType.UInt16:
                                    if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt16.Parse(objHdr[i][oa]);
                                    break;
                                case DbType.UInt32:
                                    if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt32.Parse(objHdr[i][oa]);
                                    break;
                                case DbType.UInt64:
                                    if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt64.Parse(objHdr[i][oa]);
                                    break;
                                case DbType.StringFixedLength:
                                case DbType.String:
                                    qr.Parameters[oa].ParameterValue = objHdr[i][oa];
                                    break;
                                default:
                                    qr.Parameters[oa].ParameterValue = objHdr[i][oa];
                                    break;
                            }
                        
                    }
                    col.Add(qr);
                }
            }
            if (duplicate.Count != 0)
            {
                string str;
                str = "Error: Attempting to enter duplicate CounterCloseDet Number.";
                str = str + "\r\n" + "Duplicates:" + "\r\n";
                for (int i = 0; i < duplicate.Count; i++)
                    str += duplicate[i] + "\r\n";

                Logger.writeLog(str);
                throw new Exception("Duplicate found");
            }

            return col;

        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
            throw ex;
        }
    }

    private static QueryCommandCollection FetchLoginActivity(string[][] objHdr)
    {
        try
        {
            Query qry;
            Object tmpGuid;


            ArrayList duplicate = new ArrayList();

            //ORDER HEADER
            //Logger.writeLog("#records>" + objHdr.Length);

            LoginActivity ord = new LoginActivity();

            QueryCommand qr = ord.GetInsertCommand("SYNC");
            int UniqueIDIndex;
            UniqueIDIndex = -1;
            for (int f = 0; f < qr.Parameters.Count; f++)
            {
                if (qr.Parameters[f].ParameterName.ToLower() == "@uniqueid")
                {
                    UniqueIDIndex = f;
                    break;
                }
            }
            if (UniqueIDIndex == -1)
            {
                Logger.writeLog("Sync table does not contain unique ID or unique ID name does not compy.");
                throw new Exception("Sync table does not contain unique ID or unique ID name does not compy.");
            }

            QueryCommandCollection col = new QueryCommandCollection();
            ArrayList allList = new ArrayList();
            for (int i = 0; i < objHdr.Length; i++)
            {
                allList.Add(objHdr[i][0]);
            }

            //Check GUID uniqueness in Target DB
            string sqlString = "select * from loginactivity where loginactivityid in (";
            if (allList.Count > 0)
            {
                for (int i = 0; i < allList.Count; i++)
                {
                    sqlString = sqlString + "'" + allList[i] + "',";
                }
                sqlString = sqlString.Substring(0, sqlString.Length - 1) + ")";

            }
            else
            {
                sqlString = sqlString + "''";
            }
            DataTable dtUniqueID = DataService.GetDataSet(new QueryCommand(sqlString)).Tables[0];

            for (int i = 0; i < objHdr.Length; i++)
            {
                if (dtUniqueID.Rows.Count > 0)
                {
                    DataRow[] dr = dtUniqueID.Select("LoginActivityID = '" + objHdr[i][0] + "'");

                    if (dr.Length > 0)
                        tmpGuid = new Guid(dr[0]["UniqueID"].ToString());
                    else
                        tmpGuid = new Object();
                }
                else tmpGuid = new Object();

                if (dtUniqueID.Rows.Count > 0 && tmpGuid is Guid) //GUID Found?
                {
                    qr = ord.GetInsertCommand("SYNC");
                    for (int oa = 0; oa < qr.Parameters.Count; oa++)
                    {
                        if (oa >= objHdr[i].Length) continue;
                        switch (qr.Parameters[oa].DataType)
                        {
                            case DbType.Boolean:
                                if (objHdr[i][oa] == "0")
                                    qr.Parameters[oa].ParameterValue = false;
                                else if (objHdr[i][oa] == "1")
                                    qr.Parameters[oa].ParameterValue = true;
                                break;
                            case DbType.Date:
                            case DbType.DateTime:
                                if (objHdr[i][oa] != string.Empty)
                                {
                                    qr.Parameters[oa].ParameterValue = DateTime.Parse(objHdr[i][oa]);
                                    Logger.writeLog(">>>" + objHdr[i][oa]);
                                }
                                break;
                            case DbType.Decimal:
                            case DbType.Currency:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Decimal.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Double:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Double.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Single:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Single.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Guid:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = new Guid(objHdr[i][oa]);
                                break;
                            case DbType.Int16:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int16.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Int32:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int32.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Int64:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int64.Parse(objHdr[i][oa]);
                                break;
                            case DbType.UInt16:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt16.Parse(objHdr[i][oa]);
                                break;
                            case DbType.UInt32:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt32.Parse(objHdr[i][oa]);
                                break;
                            case DbType.UInt64:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt64.Parse(objHdr[i][oa]);
                                break;
                            case DbType.StringFixedLength:
                            case DbType.String:
                                qr.Parameters[oa].ParameterValue = objHdr[i][oa];
                                break;
                            default:
                                qr.Parameters[oa].ParameterValue = objHdr[i][oa];
                                break;
                        }

                    }
                    ord = new LoginActivity("LoginActivityID", objHdr[i][0]);
                    //ord.TotalSystemRecorded = (decimal)qr.Parameters.GetParameter("@TotalSystemRecorded").ParameterValue;
                    //ord.TotalActualCollected = (decimal)qr.Parameters.GetParameter("@TotalActualCollected").ParameterValue;
                    ord.ModifiedBy = (string)qr.Parameters.GetParameter("@ModifiedBy").ParameterValue;
                    ord.ModifiedOn = (DateTime)qr.Parameters.GetParameter("@ModifiedOn").ParameterValue;


                    QueryCommand qrUpdate = ord.GetUpdateCommand("SYNC");
                    if (qrUpdate.CommandSql.IndexOf("[ModifiedBy] = @ModifiedBy, [ModifiedOn] = @ModifiedOn,") > 0)
                    {
                        qrUpdate.CommandSql = qrUpdate.CommandSql.Remove(qrUpdate.CommandSql.IndexOf("[ModifiedBy] = @ModifiedBy, [ModifiedOn] = @ModifiedOn,"), 55);
                        qrUpdate.Parameters.GetParameter("@ModifiedOn").ParameterValue = ord.ModifiedOn;
                    }
                    else
                    {
                        //qrUpdate.CommandSql = qrUpdate.CommandSql.Remove(qrUpdate.CommandSql.IndexOf(", [ModifiedOn] = @ModifiedOn"), 28);
                        //qrUpdate.Parameters.GetParameter("@ModifiedOn").ParameterValue = ord.ModifiedOn;
                    }

                    col.Add(qrUpdate);
                }
                else
                {
                    //New Item. Perform Create
                    qr = ord.GetInsertCommand("SYNC");
                    for (int oa = 0; oa < qr.Parameters.Count; oa++)
                    {
                        if (oa >= objHdr[i].Length) continue;
                        switch (qr.Parameters[oa].DataType)
                        {
                            case DbType.Boolean:
                                if (objHdr[i][oa] == "0")
                                    qr.Parameters[oa].ParameterValue = false;
                                else if (objHdr[i][oa] == "1")
                                    qr.Parameters[oa].ParameterValue = true;
                                break;
                            case DbType.Date:
                            case DbType.DateTime:
                                if (objHdr[i][oa] != string.Empty)
                                {
                                    qr.Parameters[oa].ParameterValue = DateTime.Parse(objHdr[i][oa]);
                                    Logger.writeLog(">>>" + objHdr[i][oa]);
                                }
                                break;
                            case DbType.Decimal:
                            case DbType.Currency:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Decimal.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Double:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Double.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Single:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Single.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Guid:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = new Guid(objHdr[i][oa]);
                                break;
                            case DbType.Int16:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int16.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Int32:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int32.Parse(objHdr[i][oa]);
                                break;
                            case DbType.Int64:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int64.Parse(objHdr[i][oa]);
                                break;
                            case DbType.UInt16:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt16.Parse(objHdr[i][oa]);
                                break;
                            case DbType.UInt32:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt32.Parse(objHdr[i][oa]);
                                break;
                            case DbType.UInt64:
                                if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt64.Parse(objHdr[i][oa]);
                                break;
                            case DbType.StringFixedLength:
                            case DbType.String:
                                qr.Parameters[oa].ParameterValue = objHdr[i][oa];
                                break;
                            default:
                                qr.Parameters[oa].ParameterValue = objHdr[i][oa];
                                break;
                        }
                    }
                    col.Add(qr);
                }
            }
            if (duplicate.Count != 0)
            {
                string str;
                str = "Error: Attempting to enter duplicate Login Activity ID.";
                str = str + "\r\n" + "Duplicates:" + "\r\n";
                for (int i = 0; i < duplicate.Count; i++)
                    str += duplicate[i] + "\r\n";

                Logger.writeLog(str);
                throw new Exception("Duplicate found");
            }

            return col;

        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
            throw ex;
        }
    }

    private static QueryCommandCollection FetchVoidLog(string[][] objHdr)
    {
        try
        {
            Query qry;
            Object tmpGuid;


            ArrayList duplicate = new ArrayList();

            VoidLog ord = new VoidLog();

            QueryCommand qr = ord.GetInsertCommand("SYNC");
            QueryCommandCollection col = new QueryCommandCollection();
            ArrayList allList = new ArrayList();
            for (int i = 0; i < objHdr.Length; i++)
            {
                allList.Add(objHdr[i][0]);
            }

            //Check GUID uniqueness in Target DB
            qry = new Query("VoidLog");
            qry.SelectList = "OrderHdrID";
            DataTable dtUniqueID = qry.IN(VoidLog.Columns.OrderHdrID, allList).ExecuteDataSet().Tables[0];

            for (int i = 0; i < objHdr.Length; i++)
            {
                /*if (dtUniqueID.Rows.Count > 0)
                {
                    DataRow[] dr = dtUniqueID.Select("OrderHdrID = '" + objHdr[i][1] + "'");
                    Logger.writeLog("Sync Void Log = " + objHdr[i][1]);
                }*/
                //else tmpGuid = new Object();
                ArrayList theList = new ArrayList();
                theList.Add(objHdr[i][0]);

                qry = new Query("VoidLog");
                qry.SelectList = "OrderHdrID";
                DataTable dtUnique = qry.IN(VoidLog.Columns.OrderHdrID, theList).ExecuteDataSet().Tables[0];

                if (dtUnique.Rows.Count > 0)
                {
                    //Update Now
                    qr = ord.GetInsertCommand("SYNC");
                    for (int oa = 0; oa < qr.Parameters.Count; oa++)
                    {
                        if (oa >= objHdr[i].Length) continue;
                        if (qr.Parameters[oa].ParameterName.ToLower() != "@createdon" &
                                   qr.Parameters[oa].ParameterName.ToLower() != "@createdby" &
                                    qr.Parameters[oa].ParameterName.ToLower() != "@modifiedon" &
                                    qr.Parameters[oa].ParameterName.ToLower() != "@modifiedby")
                        {
                            switch (qr.Parameters[oa].DataType)
                            {
                                case DbType.Boolean:
                                    if (objHdr[i][oa] == "0")
                                        qr.Parameters[oa].ParameterValue = false;
                                    else if (objHdr[i][oa] == "1")
                                        qr.Parameters[oa].ParameterValue = true;
                                    break;
                                case DbType.Date:
                                case DbType.DateTime:
                                    if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = DateTime.Parse(objHdr[i][oa]);
                                    break;
                                case DbType.Decimal:
                                case DbType.Currency:
                                    if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Decimal.Parse(objHdr[i][oa]);
                                    break;
                                case DbType.Double:
                                    if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Double.Parse(objHdr[i][oa]);
                                    break;
                                case DbType.Single:
                                    if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Single.Parse(objHdr[i][oa]);
                                    break;
                                case DbType.Guid:
                                    if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = new Guid(objHdr[i][oa]);
                                    break;
                                case DbType.Int16:
                                    if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int16.Parse(objHdr[i][oa]);
                                    break;
                                case DbType.Int32:
                                    if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int32.Parse(objHdr[i][oa]);
                                    break;
                                case DbType.Int64:
                                    if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int64.Parse(objHdr[i][oa]);
                                    break;
                                case DbType.UInt16:
                                    if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt16.Parse(objHdr[i][oa]);
                                    break;
                                case DbType.UInt32:
                                    if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt32.Parse(objHdr[i][oa]);
                                    break;
                                case DbType.UInt64:
                                    if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt64.Parse(objHdr[i][oa]);
                                    break;
                                case DbType.StringFixedLength:
                                case DbType.String:
                                    qr.Parameters[oa].ParameterValue = objHdr[i][oa];
                                    break;
                                default:
                                    qr.Parameters[oa].ParameterValue = objHdr[i][oa];
                                    break;
                            }
                        }
                    }

                    ord = new VoidLog(VoidLog.Columns.OrderHdrID, objHdr[i][0]);
                    //ord.ModifiedBy = (string)qr.Parameters.GetParameter("@ModifiedBy").ParameterValue;
                    //ord.ModifiedOn = (DateTime)qr.Parameters.GetParameter("@ModifiedOn").ParameterValue;


                    QueryCommand qrUpdate = ord.GetUpdateCommand("SYNC");
                    //if (qrUpdate.CommandSql.IndexOf("[ModifiedBy] = @ModifiedBy, [ModifiedOn] = @ModifiedOn,") > 0)
                    //{
                    //    qrUpdate.CommandSql = qrUpdate.CommandSql.Remove(qrUpdate.CommandSql.IndexOf("[ModifiedBy] = @ModifiedBy, [ModifiedOn] = @ModifiedOn,"), 55);
                    //    qrUpdate.Parameters.GetParameter("@ModifiedOn").ParameterValue = ord.ModifiedOn;
                    //}
                    //else
                    //{
                    //    qrUpdate.CommandSql = qrUpdate.CommandSql.Remove(qrUpdate.CommandSql.IndexOf(", [ModifiedOn] = @ModifiedOn"), 28);
                    //    qrUpdate.Parameters.GetParameter("@ModifiedOn").ParameterValue = ord.ModifiedOn;
                    //}

                    col.Add(qrUpdate);
                }
                else
                {
                    //New Item. Perform Insert
                    qr = ord.GetInsertCommand("SYNC");
                    Logger.writeLog("Insert VoidLog = " + objHdr[i][0]);
                    for (int oa = 0; oa < qr.Parameters.Count; oa++)
                    {
                        if (oa >= objHdr[i].Length) continue;
                        if (qr.Parameters[oa].ParameterName.ToLower() != "@createdon" &
                                   qr.Parameters[oa].ParameterName.ToLower() != "@createdby" &
                                    qr.Parameters[oa].ParameterName.ToLower() != "@modifiedon" &
                                    qr.Parameters[oa].ParameterName.ToLower() != "@modifiedby")
                        {
                            switch (qr.Parameters[oa].DataType)
                            {
                                case DbType.Boolean:
                                    if (objHdr[i][oa] == "0")
                                        qr.Parameters[oa].ParameterValue = false;
                                    else if (objHdr[i][oa] == "1")
                                        qr.Parameters[oa].ParameterValue = true;
                                    break;
                                case DbType.Date:
                                case DbType.DateTime:
                                    if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = DateTime.Parse(objHdr[i][oa]);
                                    break;
                                case DbType.Decimal:
                                case DbType.Currency:
                                    if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Decimal.Parse(objHdr[i][oa]);
                                    break;
                                case DbType.Double:
                                    if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Double.Parse(objHdr[i][oa]);
                                    break;
                                case DbType.Single:
                                    if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Single.Parse(objHdr[i][oa]);
                                    break;
                                case DbType.Guid:
                                    if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = new Guid(objHdr[i][oa]);
                                    break;
                                case DbType.Int16:
                                    if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int16.Parse(objHdr[i][oa]);
                                    break;
                                case DbType.Int32:
                                    if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int32.Parse(objHdr[i][oa]);
                                    break;
                                case DbType.Int64:
                                    if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int64.Parse(objHdr[i][oa]);
                                    break;
                                case DbType.UInt16:
                                    if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt16.Parse(objHdr[i][oa]);
                                    break;
                                case DbType.UInt32:
                                    if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt32.Parse(objHdr[i][oa]);
                                    break;
                                case DbType.UInt64:
                                    if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt64.Parse(objHdr[i][oa]);
                                    break;
                                case DbType.StringFixedLength:
                                case DbType.String:
                                    qr.Parameters[oa].ParameterValue = objHdr[i][oa];
                                    break;
                                default:
                                    qr.Parameters[oa].ParameterValue = objHdr[i][oa];
                                    break;
                            }
                        }
                    }
                    col.Add(qr);
                }
            }
            return col;

        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
            throw ex;
        }
    }

    private static QueryCommandCollection FetchOrderDetUOMConversion(string[][] objDet)
    {
        try
        {
            ArrayList approvedList = new ArrayList();
            QueryCommandCollection col = new QueryCommandCollection();
            //Logger.writeLog("#records>" + objDet.Length);
            int UniqueIDIndex;

            OrderDetUOMConversion ord = new OrderDetUOMConversion();

            QueryCommand qr = ord.GetInsertCommand("SYNC");
            UniqueIDIndex = -1;

            for (int f = 0; f < qr.Parameters.Count; f++)
            {
                if (qr.Parameters[f].ParameterName.ToLower() == "@uniqueid")
                {
                    UniqueIDIndex = f;
                    break;
                }
            }

            if (UniqueIDIndex == -1)
            {
                Logger.writeLog("Sync table does not contain Unique ID.");
                throw new Exception("Sync table does not contain UniqueID.");
            }
            ArrayList allList = new ArrayList();
            for (int i = 0; i < objDet.Length; i++)
            {
                allList.Add(objDet[i][0]);
            }

            Object tmpGuid;
            ArrayList duplicate = new ArrayList();
            //Check GUID uniqueness in Target DB
            Query qry = new Query("OrderDetUOMConversion");
            qry.SelectList = "OrderDetUOMConvID,UniqueID";
            DataTable dtUniqueID = qry.IN(OrderDetUOMConversion.Columns.OrderDetUOMConvID, allList).ExecuteDataSet().Tables[0];

            for (int i = 0; i < objDet.Length; i++)
            {
                if (dtUniqueID.Rows.Count > 0)
                {
                    DataRow[] dr = dtUniqueID.Select("OrderDetUOMConvID = '" + objDet[i][0] + "'");

                    if (dr.Length > 0)
                        tmpGuid = new Guid(dr[0]["UniqueID"].ToString());
                    else
                        tmpGuid = new Object();
                }
                else tmpGuid = new Object();

                if (dtUniqueID.Rows.Count > 0 && tmpGuid is Guid) //GUID Found?
                {
                    //if ((Guid)tmpGuid != new Guid(objDet[i][UniqueIDIndex]))
                    //{
                    //    //same order but different unique ID - Houston we have a problem                        
                    //    duplicate.Add(qr.Parameters[0].ParameterValue.ToString());
                    //}

                    qr = ord.GetInsertCommand("SYNC");
                    Logger.writeLog("Update OrderDetUOMConvID = " + objDet[i][0]);
                    for (int oa = 0; oa < qr.Parameters.Count; oa++)
                    {
                        if (qr.Parameters[oa].ParameterName.ToLower() != "@createdon" &
                                   qr.Parameters[oa].ParameterName.ToLower() != "@createdby" &
                                    qr.Parameters[oa].ParameterName.ToLower() != "@modifiedon" &
                                    qr.Parameters[oa].ParameterName.ToLower() != "@modifiedby")
                        {
                            //Logger.writeLog(qr.Parameters[oa].ParameterName + " val: " + qr.Parameters[oa].ParameterValue.ToString());
                            if (oa >= objDet[i].Length) continue;
                            switch (qr.Parameters[oa].DataType)
                            {
                                case DbType.Boolean:
                                    if (objDet[i][oa] == "0")
                                        qr.Parameters[oa].ParameterValue = false;
                                    else if (objDet[i][oa] == "1")
                                        qr.Parameters[oa].ParameterValue = true;
                                    break;
                                case DbType.Date:
                                case DbType.DateTime:
                                    if (!string.IsNullOrEmpty(objDet[i][oa]))
                                    {
                                        qr.Parameters[oa].ParameterValue = DateTime.Parse(objDet[i][oa]);
                                        Logger.writeLog(">>>" + objDet[i][oa]);
                                    }
                                    break;
                                case DbType.Decimal:
                                case DbType.Currency:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Decimal.Parse(objDet[i][oa]);
                                    break;
                                case DbType.Double:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Double.Parse(objDet[i][oa]);
                                    break;
                                case DbType.Single:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Single.Parse(objDet[i][oa]);
                                    break;
                                case DbType.Guid:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = new Guid(objDet[i][oa]);
                                    break;
                                case DbType.Int16:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int16.Parse(objDet[i][oa]);
                                    break;
                                case DbType.Int32:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int32.Parse(objDet[i][oa]);
                                    break;
                                case DbType.Int64:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int64.Parse(objDet[i][oa]);
                                    break;
                                case DbType.UInt16:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt16.Parse(objDet[i][oa]);
                                    break;
                                case DbType.UInt32:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt32.Parse(objDet[i][oa]);
                                    break;
                                case DbType.UInt64:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt64.Parse(objDet[i][oa]);
                                    break;
                                case DbType.StringFixedLength:
                                case DbType.String:
                                    qr.Parameters[oa].ParameterValue = objDet[i][oa];
                                    break;
                                default:
                                    qr.Parameters[oa].ParameterValue = objDet[i][oa];
                                    break;
                            }
                        }
                    }

                    ord = new OrderDetUOMConversion(objDet[i][0]);
                    ord.IsVoided = (bool)qr.Parameters.GetParameter("@IsVoided").ParameterValue;
                    ord.ModifiedBy = (string)qr.Parameters.GetParameter("@ModifiedBy").ParameterValue;
                    ord.ModifiedOn = (DateTime)qr.Parameters.GetParameter("@ModifiedOn").ParameterValue;

                    QueryCommand qrUpdate = ord.GetUpdateCommand("SYNC");
                    if (qrUpdate.CommandSql.IndexOf("[ModifiedBy] = @ModifiedBy, [ModifiedOn] = @ModifiedOn,") > 0)
                    {
                        qrUpdate.CommandSql = qrUpdate.CommandSql.Remove(qrUpdate.CommandSql.IndexOf("[ModifiedBy] = @ModifiedBy, [ModifiedOn] = @ModifiedOn,"), 55);
                        qrUpdate.Parameters.GetParameter("@ModifiedOn").ParameterValue = ord.ModifiedOn;
                    }
                    else
                    {
                        qrUpdate.CommandSql = qrUpdate.CommandSql.Remove(qrUpdate.CommandSql.IndexOf(", [ModifiedOn] = @ModifiedOn"), 28);
                        qrUpdate.Parameters.GetParameter("@ModifiedOn").ParameterValue = ord.ModifiedOn;
                    }

                    col.Add(qrUpdate);
                }
                else
                {
                    //It's Approved! - else ignore....
                    qr = ord.GetInsertCommand("SYNC");
                    Logger.writeLog("Insert OrderdetUOMConversion: " + objDet[i][0]);
                    Logger.writeLog(string.Format("Sync Insert OrderDetUOMConversion : {0} ", objDet[i].AsSingleLineString(",")));
                    for (int oa = 0; oa < qr.Parameters.Count; oa++)
                    {
                        if (qr.Parameters[oa].ParameterName.ToLower() != "@createdon" &
                                   qr.Parameters[oa].ParameterName.ToLower() != "@createdby" &
                                    qr.Parameters[oa].ParameterName.ToLower() != "@modifiedon" &
                                    qr.Parameters[oa].ParameterName.ToLower() != "@modifiedby")
                        {
                            if (oa >= objDet[i].Length) continue;
                            switch (qr.Parameters[oa].DataType)
                            {
                                case DbType.Boolean:
                                    if (objDet[i][oa] == "0")
                                        qr.Parameters[oa].ParameterValue = false;
                                    else if (objDet[i][oa] == "1")
                                        qr.Parameters[oa].ParameterValue = true;
                                    break;
                                case DbType.Date:
                                case DbType.DateTime:
                                    if (objDet[i][oa] != string.Empty)
                                        qr.Parameters[oa].ParameterValue = DateTime.Parse(objDet[i][oa]);
                                    break;
                                case DbType.Decimal:
                                case DbType.Currency:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Decimal.Parse(objDet[i][oa]);
                                    break;
                                case DbType.Double:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Double.Parse(objDet[i][oa]);
                                    break;
                                case DbType.Single:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Single.Parse(objDet[i][oa]);
                                    break;
                                case DbType.Guid:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = new Guid(objDet[i][oa]);
                                    break;
                                case DbType.Int16:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int16.Parse(objDet[i][oa]);
                                    break;
                                case DbType.Int32:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int32.Parse(objDet[i][oa]);
                                    break;
                                case DbType.Int64:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int64.Parse(objDet[i][oa]);
                                    break;
                                case DbType.UInt16:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt16.Parse(objDet[i][oa]);
                                    break;
                                case DbType.UInt32:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt32.Parse(objDet[i][oa]);
                                    break;
                                case DbType.UInt64:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt64.Parse(objDet[i][oa]);
                                    break;
                                case DbType.StringFixedLength:
                                case DbType.String:
                                    qr.Parameters[oa].ParameterValue = objDet[i][oa];
                                    break;
                                default:
                                    qr.Parameters[oa].ParameterValue = objDet[i][oa];
                                    break;
                            }
                        }
                    }
                    col.Add(qr);
                }
            }
            return col;
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
            throw ex;
        }
    }

    public static int FetchRecordNoByTimestamp(string tableName, DateTime modifiedOn)
    {
        int count = 0;

        try
        {
            string sql = "SELECT COUNT(*) NoOfRecords FROM {0} WHERE ModifiedOn > '{1}'";
            sql = string.Format(sql, tableName, modifiedOn.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            DataTable dt = new DataTable();
            dt.Load(DataService.GetReader(new QueryCommand(sql)));

            if (dt.Rows.Count > 0)
                count = (int)dt.Rows[0]["NoOfRecords"];
        }
        catch (Exception ex)
        {
            count = 0;
            Logger.writeLog(">> FetchRecordNoByTimestamp ERROR on table : " + tableName);
            Logger.writeLog(ex);
        }

        return count;
    }

    public static int FetchRecordNoByTimestampByInvLocID(string tableName, DateTime modifiedOn, int invLocID)
    {
        int count = 0;

        try
        {
            string sql = "SELECT COUNT(*) NoOfRecords FROM {0} WHERE ModifiedOn > '{1}' AND InventoryLocationID = '{2}'";
            sql = string.Format(sql, tableName, modifiedOn.ToString("yyyy-MM-dd HH:mm:ss.fff"), invLocID);
            DataTable dt = new DataTable();
            dt.Load(DataService.GetReader(new QueryCommand(sql)));
            if (dt.Rows.Count > 0)
                count = (int)dt.Rows[0]["NoOfRecords"];
        }
        catch (Exception ex)
        {
            count = 0;
            Logger.writeLog(">> FetchRecordNoByTimestampByInvLocID ERROR on table : " + tableName);
            Logger.writeLog(ex);
        }

        return count;
    }

    public static int FetchRecordNoByTimestampByPOSID(string tableName, DateTime modifiedOn, int POSID)
    {
        int count = 0;

        try
        {
            string sql = "SELECT COUNT(*) NoOfRecords FROM {0} WHERE ModifiedOn > '{1}' AND PointOfSaleID = '{2}'";
            sql = string.Format(sql, tableName, modifiedOn.ToString("yyyy-MM-dd HH:mm:ss.fff"), POSID);
            DataTable dt = new DataTable();
            dt.Load(DataService.GetReader(new QueryCommand(sql)));
            if (dt.Rows.Count > 0)
                count = (int)dt.Rows[0]["NoOfRecords"];
        }
        catch (Exception ex)
        {
            count = 0;
            Logger.writeLog(">> FetchRecordNoByTimestampByPOSID ERROR on table : " + tableName);
            Logger.writeLog(ex);
        }

        return count;
    }

    public static int FetchRecordNoByTimestampByOutletName(string tableName, DateTime modifiedOn, string OutletName)
    {
        int count = 0;

        try
        {
            string sql = "SELECT COUNT(*) NoOfRecords FROM {0} WHERE ModifiedOn > '{1}' AND OutletName = '{2}'";
            sql = string.Format(sql, tableName, modifiedOn.ToString("yyyy-MM-dd HH:mm:ss.fff"), OutletName);
            DataTable dt = new DataTable();
            dt.Load(DataService.GetReader(new QueryCommand(sql)));
            if (dt.Rows.Count > 0)
                count = (int)dt.Rows[0]["NoOfRecords"];
        }
        catch (Exception ex)
        {
            count = 0;
            Logger.writeLog(">> FetchRecordNoByTimestampByOutletName ERROR on table : " + tableName);
            Logger.writeLog(ex);
        }

        return count;
    }

    public static int FetchRecordNoByTimestampByMember(string tableName, DateTime modifiedOn, string membershipNo)
    {
        int count = 0;

        try
        {
            DateTime timeSyncPointStarted;
            TimeSpan ts;
            timeSyncPointStarted = DateTime.Now;

            string sql = "SELECT COUNT(*) NoOfRecords FROM {0} WHERE ModifiedOn > '{1}' AND MembershipNo = '{2}'";
            sql = string.Format(sql, tableName, modifiedOn.ToString("yyyy-MM-dd HH:mm:ss.fff"), membershipNo);
            DataTable dt = new DataTable();
            dt.Load(DataService.GetReader(new QueryCommand(sql)));

            if (dt.Rows.Count > 0)
                count = (int)dt.Rows[0]["NoOfRecords"];

            ts = DateTime.Now - timeSyncPointStarted;
            if (tableName.ToLower() == "membershippoints" || tableName.ToLower() == "pointallocationlog")
                Logger.writeLog(string.Format("{1} ws.FetchRecordNoByTimestampByMember completed in {0} seconds.", ts.Seconds.ToString(), tableName));
        }
        catch (Exception ex)
        {
            count = 0;
            Logger.writeLog(">> FetchRecordNoByTimestamp Per Member ERROR on table : " + tableName + " and membershipno = " + membershipNo);
            Logger.writeLog(ex);
        }

        return count;
    }

    public static int FetchRecordNoByTimestampByModuleName(string tableName, DateTime modifiedOn, string moduleName)
    {
        int count = 0;

        try
        {
            string sql = "SELECT COUNT(*) NoOfRecords FROM {0} WHERE ModifiedOn > '{1}' AND ModuleName = '{2}'";
            sql = string.Format(sql, tableName, modifiedOn.ToString("yyyy-MM-dd HH:mm:ss.fff"), moduleName);
            DataTable dt = new DataTable();
            dt.Load(DataService.GetReader(new QueryCommand(sql)));

            if (dt.Rows.Count > 0)
                count = (int)dt.Rows[0]["NoOfRecords"];
        }
        catch (Exception ex)
        {
            count = 0;
            Logger.writeLog(">> FetchRecordNoByTimestamp Per ModuleName ERROR on table : " + tableName + " and moduleName = " + moduleName);
            Logger.writeLog(ex);
        }

        return count;
    }

    public static int FetchPromoRecordNoByTimestampByOutletName(DateTime modifiedOn, string OutletName)
    {
        int count = 0;

        try
        {
            string sql = "SELECT COUNT(*) NoOfRecords FROM PromoCampaignHdr WHERE ModifiedOn > '{0}' AND PromoCampaignHdrID in (Select PromoCampaignHdrID from PromoOutlet where OutletName = '{1}')";
            sql = string.Format(sql, modifiedOn.ToString("yyyy-MM-dd HH:mm:ss.fff"), OutletName);
            DataTable dt = new DataTable();
            dt.Load(DataService.GetReader(new QueryCommand(sql)));
            if (dt.Rows.Count > 0)
                count = (int)dt.Rows[0]["NoOfRecords"];
        }
        catch (Exception ex)
        {
            count = 0;
            Logger.writeLog(">> FetchRecordNoByTimestampByOutletName ERROR on table PromoCampaignHdr ");
            Logger.writeLog(ex);
        }

        return count;
    }

    public static int FetchRecordNoAppointment(DateTime modifiedOn, int PointOfSaleID)
    {
        int count = 0;

        try
        {
            string sql = "SELECT COUNT(*) NoOfRecords FROM {0} WHERE (ModifiedOn > '{1}' OR IsServerUpdate = 1) and PointOfSaleID = {2}";
            sql = string.Format(sql, "Appointment", modifiedOn.ToString("yyyy-MM-dd HH:mm:ss.fff"), PointOfSaleID);
            DataTable dt = new DataTable();
            dt.Load(DataService.GetReader(new QueryCommand(sql)));

            if (dt.Rows.Count > 0)
                count = (int)dt.Rows[0]["NoOfRecords"];
        }
        catch (Exception ex)
        {
            count = 0;
            Logger.writeLog(">> FetchRecordNoByTimestamp ERROR on table Appointment ");
            Logger.writeLog(ex);
        }

        return count;
    }

    public static DataSet FetchDataSetRealTime(string tableName, string pkColumn, DateTime modifiedDate,
        int noOfRecords, bool useInventoryLoc, int invLocID, bool usePOSID, int POSID)
    {
        var dt = new DataTable();

        string sql = @"DECLARE @EndDate DATETIME;
                        DECLARE @Row INT;

                        SET @EndDate = '{0}';
                        SET @Row = {1};

                        SELECT  I.*
                        FROM	{2} I
		                        INNER JOIN (
		                        SELECT	 {3} TheKey
				                        ,ModifiedOn TheModifiedOn
				                        ,DENSE_RANK() OVER(ORDER BY ModifiedOn) TheNo
		                        FROM	{2} 
		                        WHERE	ModifiedOn > @EndDate {4} {5} 
		                        ) TAB ON TAB.TheKey = {3}
                        WHERE	TAB.TheNo <= @Row  
                                {4} {5}  
                        ORDER BY TAB.TheNo, TAB.TheModifiedOn, TAB.TheKey";
        string invLoc = "";
        string pos = "";
        if (useInventoryLoc)
            invLoc = string.Format(" AND InventoryLocationID = '{0}' ", invLocID);
        if (usePOSID)
            pos = string.Format(" AND PointOfSaleID = '{0}' ", POSID);


        sql = string.Format(sql, modifiedDate.ToString("yyyy-MM-dd HH:mm:ss.fff")
                               , noOfRecords
                               , tableName
                               , pkColumn
                               , invLoc
                               , pos);
        dt.Load(DataService.GetReader(new QueryCommand(sql)));
        dt.TableName = tableName;

        var ds = new DataSet();
        ds.Tables.Add(dt);

        return ds;
    }

    public static DataSet FetchDataSetRealTimePerMember(string tableName, string pkColumn, DateTime modifiedDate,
        int noOfRecords, string membershipNo)
    {
        var dt = new DataTable();

        string sql = @"DECLARE @EndDate DATETIME;
                        DECLARE @Row INT;

                        SET @EndDate = '{0}';
                        SET @Row = {1};

                        SELECT  I.*
                        FROM	{2} I
		                        INNER JOIN (
		                        SELECT	 {3} TheKey
				                        ,ModifiedOn TheModifiedOn
				                        ,DENSE_RANK() OVER(ORDER BY ModifiedOn) TheNo
		                        FROM	{2} 
		                        WHERE	ModifiedOn > @EndDate {4} 
		                        ) TAB ON TAB.TheKey = {3}
                        WHERE	TAB.TheNo <= @Row  
                                {4} 
                        ORDER BY TAB.TheNo, TAB.TheModifiedOn, TAB.TheKey";
        string invLoc = "";
        //string pos = "";

        invLoc = string.Format(" AND MembershipNo = '{0}' ", membershipNo);



        sql = string.Format(sql, modifiedDate.ToString("yyyy-MM-dd HH:mm:ss.fff")
                               , noOfRecords
                               , tableName
                               , pkColumn
                               , invLoc
                               );
        dt.Load(DataService.GetReader(new QueryCommand(sql)));
        dt.TableName = tableName;

        var ds = new DataSet();
        ds.Tables.Add(dt);

        return ds;
    }

    public static DataSet FetchDataSetRealTimePerModuleName(string tableName, string pkColumn, DateTime modifiedDate,
        int noOfRecords, string moduleName)
    {
        var dt = new DataTable();

        string sql = @"DECLARE @EndDate DATETIME;
                        DECLARE @Row INT;

                        SET @EndDate = '{0}';
                        SET @Row = {1};

                        SELECT  I.*
                        FROM	{2} I
		                        INNER JOIN (
		                        SELECT	 {3} TheKey
				                        ,ModifiedOn TheModifiedOn
				                        ,DENSE_RANK() OVER(ORDER BY ModifiedOn) TheNo
		                        FROM	{2} 
		                        WHERE	ModifiedOn > @EndDate {4} 
		                        ) TAB ON TAB.TheKey = {3}
                        WHERE	TAB.TheNo <= @Row  
                                {4} 
                        ORDER BY TAB.TheNo, TAB.TheModifiedOn, TAB.TheKey";
        string invLoc = "";

        invLoc = string.Format(" AND ModuleName = '{0}' ", moduleName);

        sql = string.Format(sql, modifiedDate.ToString("yyyy-MM-dd HH:mm:ss.fff")
                               , noOfRecords
                               , tableName
                               , pkColumn
                               , invLoc
                               );
        dt.Load(DataService.GetReader(new QueryCommand(sql)));
        dt.TableName = tableName;

        var ds = new DataSet();
        ds.Tables.Add(dt);

        return ds;
    }

    public static byte[] FetchDataSetRealTimeCompressed(string tableName, string pkColumn, DateTime modifiedDate,
        int noOfRecords, bool useInventoryLoc, int invLocID, bool usePOSID, int POSID)
    {
        var dt = FetchDataSetRealTime(tableName, pkColumn, modifiedDate, noOfRecords, useInventoryLoc, invLocID, usePOSID, POSID);

        MemoryStream memStream = new MemoryStream();
        GZipStream zipStream = new GZipStream(memStream, CompressionMode.Compress);
        dt.WriteXml(zipStream, XmlWriteMode.WriteSchema);
        zipStream.Close();
        byte[] data = memStream.ToArray();
        memStream.Close();

        return data;
    }

    public static byte[] FetchDataSetRealTimeCompressedPerMember(string tableName, string pkColumn, DateTime modifiedDate,
        int noOfRecords, string membershipNo)
    {
        DateTime timeSyncPointStarted;
        TimeSpan ts;
        timeSyncPointStarted = DateTime.Now;

        var dt = FetchDataSetRealTimePerMember(tableName, pkColumn, modifiedDate, noOfRecords, membershipNo);

        ts = DateTime.Now - timeSyncPointStarted;
        if (tableName.ToLower() == "membershippoints" || tableName.ToLower() == "pointallocationlog")
            Logger.writeLog(string.Format("{1} ws.FetchDataSetByTimeStampPerMember completed in {0} seconds.", ts.Seconds.ToString(), tableName));


        timeSyncPointStarted = DateTime.Now;

        MemoryStream memStream = new MemoryStream();
        GZipStream zipStream = new GZipStream(memStream, CompressionMode.Compress);
        dt.WriteXml(zipStream, XmlWriteMode.WriteSchema);
        zipStream.Close();
        byte[] data = memStream.ToArray();
        memStream.Close();

        ts = DateTime.Now - timeSyncPointStarted;
        if (tableName.ToLower() == "membershippoints" || tableName.ToLower() == "pointallocationlog")
            Logger.writeLog(string.Format("{1} Compress Data completed in {0} seconds.", ts.Seconds.ToString(), tableName));


        return data;
    }

    public static byte[] FetchDataSetRealTimeCompressedPerModuleName(string tableName, string pkColumn, DateTime modifiedDate,
        int noOfRecords, string moduleName)
    {
        var dt = FetchDataSetRealTimePerModuleName(tableName, pkColumn, modifiedDate, noOfRecords, moduleName);

        MemoryStream memStream = new MemoryStream();
        GZipStream zipStream = new GZipStream(memStream, CompressionMode.Compress);
        dt.WriteXml(zipStream, XmlWriteMode.WriteSchema);
        zipStream.Close();
        byte[] data = memStream.ToArray();
        memStream.Close();

        return data;
    }

    public static DataSet FetchProductRealTime(DateTime lastModifiedOn, string outletName, int count)
    {
        DataSet ds = new DataSet();
        try
        {
            string outletgroupid = "0";
            Outlet o = new Outlet(outletName);
            if (o.IsLoaded && o.OutletName != "")
                outletgroupid = o.OutletGroupID == null ? "0" : o.OutletGroupID.ToString();
            string sqlString = @"DECLARE @EndDate DATETIME;
                                DECLARE @Row INT;

                                SET @EndDate = '{0}';
                                SET @Row = {1};

                                SELECT I.ItemNo
                                      ,CASE WHEN ISNULL(ou.Userfld1, '') <> '' THEN ou.Userfld1 ELSE case when ISNULL(og.Userfld1, '') <> '' THEN og.Userfld1 ELSE i.ItemName END END ItemName
                                      ,I.Barcode
                                      ,I.CategoryName
                                      ,CASE WHEN ISNULL(ou.RetailPrice, -1) <> -1 THEN ou.RetailPrice ELSE case when ISNULL(og.RetailPrice, -1) <> -1 THEN og.RetailPrice ELSE i.RetailPrice END END RetailPrice
                                      ,I.FactoryPrice
                                      ,I.MinimumPrice
                                      ,I.ItemDesc
                                      ,I.IsServiceItem
                                      ,I.IsInInventory
                                      ,I.IsNonDiscountable
                                      ,I.IsCourse
                                      ,I.CourseTypeID
                                      ,I.Brand
                                      ,I.ProductLine
                                      ,I.Attributes1
                                      ,I.Attributes2
                                      ,I.Attributes3
                                      ,I.Attributes4
                                      ,I.Attributes5
                                      ,I.Attributes6
                                      ,I.Attributes7
                                      ,I.Attributes8
                                      ,I.Remark
                                      ,I.ProductionDate
                                      ,I.IsGST
                                      ,I.hasWarranty
                                      ,I.CreatedOn
                                      ,I.CreatedBy
                                      ,I.ModifiedOn
                                      ,I.ModifiedBy
                                      ,I.UniqueID
                                      ,CASE WHEN ou.OutletGroupItemMapID IS NOT NULL THEN ISNULL(ou.Deleted,0) ELSE case when og.OutletGroupItemMapID IS NOT NULL THEN ISNULL(og.Deleted,0) ELSE i.Deleted END END Deleted
                                      ,I.userfld1
                                      ,I.userfld2
                                      ,I.userfld3
                                      ,I.userfld4
                                      ,I.userfld5
                                      ,I.userfld6
                                      ,I.userfld7
                                      ,I.userfld8
                                      ,I.userfld9
                                      ,I.userfld10
                                      ,I.userflag1
                                      ,I.userflag2
                                      ,I.userflag3
                                      ,I.userflag4
                                      ,I.userflag5
                                      ,I.userfloat1
                                      ,I.userfloat2
                                      ,I.userfloat3
                                      ,I.userfloat4
                                      ,I.userfloat5
                                      ,I.userint1
                                      ,I.userint2
                                      ,I.userint3
                                      ,I.userint4
                                      ,I.userint5
                                      ,I.IsDelivery
                                      ,I.GSTRule
                                      ,I.IsVitaMix
                                      ,I.IsWaterFilter
                                      ,I.IsYoung
                                      ,I.IsJuicePlus
                                      ,I.IsCommission
                                      ,I.ItemImage
                                      ,CASE WHEN ISNULL(ou.P1, -1) <> -1 THEN ou.P1 ELSE case when ISNULL(og.P1, -1) <> -1 THEN og.P1 ELSE i.Userfloat6 END END Userfloat6
                                      ,CASE WHEN ISNULL(ou.P2, -1) <> -1 THEN ou.P2 ELSE case when ISNULL(og.P2, -1) <> -1 THEN og.P2 ELSE i.Userfloat7 END END Userfloat7
                                      ,CASE WHEN ISNULL(ou.P3, -1) <> -1 THEN ou.P3 ELSE case when ISNULL(og.P3, -1) <> -1 THEN og.P3 ELSE i.Userfloat8 END END Userfloat8
                                      ,CASE WHEN ISNULL(ou.P4, -1) <> -1 THEN ou.P4 ELSE case when ISNULL(og.P4, -1) <> -1 THEN og.P4 ELSE i.Userfloat9 END END Userfloat9
                                      ,CASE WHEN ISNULL(ou.P5, -1) <> -1 THEN ou.P5 ELSE case when ISNULL(og.P5, -1) <> -1 THEN og.P5 ELSE i.Userfloat10 END END Userfloat10
                                      ,I.AvgCostPrice
                                      ,I.BalanceQuantity
                                      ,I.userflag6
                                      ,I.userflag7
                                      ,I.userflag8
                                      ,I.userflag9
                                      ,I.userflag10
                                FROM   Item I
	                                   INNER JOIN (
                                       SELECT	 ItemNo TheKey
                                                ,ModifiedOn TheModifiedOn
                                                ,DENSE_RANK() OVER(ORDER BY ModifiedOn) TheNo
                                       FROM		Item 
                                       WHERE	ModifiedOn > @EndDate	   
	                                   ) TAB ON TAB.TheKey = I.ItemNo
                                       LEFT OUTER JOIN OutletGroupItemMap OG ON I.itemno = OG.ItemNo AND OG.OutletGroupID = '{2}' 
                                       LEFT OUTER JOIN OutletGroupItemMap OU ON i.itemno = OU.ItemNo AND OU.OutletName = '{3}' 
                                WHERE	TAB.TheNo <= @Row
                                ORDER BY TAB.TheNo, TAB.TheModifiedOn, TAB.TheKey";
            sqlString = string.Format(sqlString, lastModifiedOn.ToString("yyyy-MM-dd HH:mm:ss.fff")
                                               , count
                                               , outletgroupid
                                               , outletName);
            DataTable dt = new DataTable();
            dt.Load(DataService.GetReader(new QueryCommand(sqlString)));
            dt.TableName = "Item";
            ds.Tables.Add(dt);

        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
        }

        return ds;
    }

    public static DataSet FetchItemQuantityTriggerRealTime(DateTime lastModifiedOn, int inventoryLocationID, int count)
    {
        DataSet ds = new DataSet();
        try
        {
            string sqlString = @"DECLARE @EndDate DATETIME;
                                DECLARE @Row INT;

                                SET @EndDate = '{0}';
                                SET @Row = {1};

                                SELECT IQT.*
                                FROM   ItemQuantityTrigger IQT
	                                   INNER JOIN (
                                       SELECT	 TriggerID TheKey
			                                    ,ModifiedOn TheModifiedOn
			                                    ,DENSE_RANK() OVER(ORDER BY ModifiedOn, TriggerID) TheNo
	                                   FROM	ItemQuantityTrigger 
	                                   WHERE	ModifiedOn > @EndDate AND InventoryLocationID = {2} 
	                                   ) TAB ON TAB.TheKey = IQT.TriggerID
                                WHERE TAB.TheNo <= @Row
                                ORDER BY TAB.TheNo, TAB.TheModifiedOn, TAB.TheKey";
            sqlString = string.Format(sqlString, lastModifiedOn.ToString("yyyy-MM-dd HH:mm:ss.fff")
                                               , count
                                               , inventoryLocationID);
            DataTable dt = new DataTable();
            dt.Load(DataService.GetReader(new QueryCommand(sqlString)));
            dt.TableName = "ItemQuantityTrigger";
            ds.Tables.Add(dt);
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
        }

        return ds;
    }

    public static DataSet FetchProductRealTime(DateTime lastModifiedOn, string outletName, int count, int skip)
    {
        DataSet ds = new DataSet();
        try
        {
            string outletgroupid = "0";
            Outlet o = new Outlet(outletName);
            if (o.IsLoaded && o.OutletName != "")
                outletgroupid = o.OutletGroupID == null ? "0" : o.OutletGroupID.ToString();
            string sqlString = @"DECLARE @EndDate DATETIME;
                                DECLARE @Row INT;
                                DECLARE @Skip INT;

                                SET @EndDate = '{0}';
                                SET @Row = {1};
                                SET @Skip = {4}

                                SELECT I.ItemNo
                                      ,I.ItemName
                                      ,I.Barcode
                                      ,I.CategoryName
                                      ,CASE WHEN ISNULL(ou.RetailPrice, -1) <> -1 THEN ou.RetailPrice ELSE case when ISNULL(og.RetailPrice, -1) <> -1 THEN og.RetailPrice ELSE i.RetailPrice END END RetailPrice
                                      ,CASE WHEN ISNULL(ou.CostPrice, -1) <> -1 THEN ou.CostPrice ELSE case when ISNULL(og.CostPrice, -1) <> -1 THEN og.CostPrice ELSE i.FactoryPrice END END FactoryPrice
                                      ,I.MinimumPrice
                                      ,I.ItemDesc
                                      ,I.IsServiceItem
                                      ,I.IsInInventory
                                      ,I.IsNonDiscountable
                                      ,I.IsCourse
                                      ,I.CourseTypeID
                                      ,I.Brand
                                      ,I.ProductLine
                                      ,I.Attributes1
                                      ,I.Attributes2
                                      ,I.Attributes3
                                      ,I.Attributes4
                                      ,I.Attributes5
                                      ,I.Attributes6
                                      ,I.Attributes7
                                      ,I.Attributes8
                                      ,I.Remark
                                      ,I.ProductionDate
                                      ,I.IsGST
                                      ,I.hasWarranty
                                      ,I.CreatedOn
                                      ,I.CreatedBy
                                      ,I.ModifiedOn
                                      ,I.ModifiedBy
                                      ,I.UniqueID
                                      ,CASE WHEN ou.OutletGroupItemMapID IS NOT NULL THEN ISNULL(ou.IsItemDeleted,0) ELSE case when og.OutletGroupItemMapID IS NOT NULL THEN ISNULL(og.IsItemDeleted,0) ELSE i.Deleted END END Deleted
                                      ,I.userfld1
                                      ,I.userfld2
                                      ,I.userfld3
                                      ,I.userfld4
                                      ,I.userfld5
                                      ,I.userfld6
                                      ,I.userfld7
                                      ,I.userfld8
                                      ,I.userfld9
                                      ,I.userfld10
                                      ,I.userflag1
                                      ,I.userflag2
                                      ,I.userflag3
                                      ,I.userflag4
                                      ,I.userflag5
                                      ,I.userfloat1
                                      ,I.userfloat2
                                      ,I.userfloat3
                                      ,I.userfloat4
                                      ,I.userfloat5
                                      ,I.userint1
                                      ,I.userint2
                                      ,I.userint3
                                      ,I.userint4
                                      ,I.userint5
                                      ,I.IsDelivery
                                      ,I.GSTRule
                                      ,I.IsVitaMix
                                      ,I.IsWaterFilter
                                      ,I.IsYoung
                                      ,I.IsJuicePlus
                                      ,I.IsCommission
                                      ,I.ItemImage
                                      ,CASE WHEN ISNULL(ou.P1, -1) <> -1 THEN ou.P1 ELSE case when ISNULL(og.P1, -1) <> -1 THEN og.P1 ELSE i.UserFloat6 END END UserFloat6
                                      ,CASE WHEN ISNULL(ou.P2, -1) <> -1 THEN ou.P2 ELSE case when ISNULL(og.P2, -1) <> -1 THEN og.P2 ELSE i.UserFloat7 END END UserFloat7
                                      ,CASE WHEN ISNULL(ou.P3, -1) <> -1 THEN ou.P3 ELSE case when ISNULL(og.P3, -1) <> -1 THEN og.P3 ELSE i.UserFloat8 END END UserFloat8
                                      ,CASE WHEN ISNULL(ou.P4, -1) <> -1 THEN ou.P4 ELSE case when ISNULL(og.P4, -1) <> -1 THEN og.P4 ELSE i.UserFloat9 END END UserFloat9
                                      ,CASE WHEN ISNULL(ou.P5, -1) <> -1 THEN ou.P5 ELSE case when ISNULL(og.P5, -1) <> -1 THEN og.P5 ELSE i.UserFloat10 END END UserFloat10
                                      ,I.AvgCostPrice
                                      ,I.BalanceQuantity
                                FROM   Item I
	                                   INNER JOIN (
                                       SELECT	 ItemNo TheKey
                                                ,ModifiedOn TheModifiedOn
                                                ,DENSE_RANK() OVER(ORDER BY ModifiedOn, ItemNo) TheNo
                                       FROM		Item 
                                       WHERE	ModifiedOn > @EndDate	   
	                                   ) TAB ON TAB.TheKey = I.ItemNo
                                       LEFT OUTER JOIN OutletGroupItemMap OG ON I.itemno = OG.ItemNo AND ISNULL(OG.Deleted,0) = 0 AND OG.OutletGroupID = '{2}' 
                                       LEFT OUTER JOIN OutletGroupItemMap OU ON i.itemno = OU.ItemNo AND ISNULL(OU.Deleted,0) = 0 AND OU.OutletName = '{3}' 
                                WHERE TAB.TheNo > @Skip AND  TAB.TheNo <= @Row
                                ORDER BY TAB.TheNo, TAB.TheModifiedOn, TAB.TheKey";
            sqlString = string.Format(sqlString, lastModifiedOn.ToString("yyyy-MM-dd HH:mm:ss.fff")
                                               , count + skip
                                               , outletgroupid
                                               , outletName
                                               , skip);
            DataTable dt = new DataTable();
            dt.Load(DataService.GetReader(new QueryCommand(sqlString)));
            dt.TableName = "Item";
            ds.Tables.Add(dt);

        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
        }

        return ds;
    }

    public static DataSet FetchItemQuantityTriggerRealTime(DateTime lastModifiedOn, int inventoryLocationID, int count, int skip)
    {
        DataSet ds = new DataSet();
        try
        {
            string sqlString = @"DECLARE @EndDate DATETIME;
                                DECLARE @Row INT;
                                DECLARE @Skip INT;

                                SET @EndDate = '{0}';
                                SET @Row = {1};
                                SET @Skip = {3};

                                SELECT IQT.*
                                FROM   ItemQuantityTrigger IQT
	                                   INNER JOIN (
                                       SELECT	 TriggerID TheKey
			                                    ,ModifiedOn TheModifiedOn
			                                    ,DENSE_RANK() OVER(ORDER BY ModifiedOn, TriggerID) TheNo
	                                   FROM	ItemQuantityTrigger 
	                                   WHERE	ModifiedOn > @EndDate AND InventoryLocationID = {2} 
	                                   ) TAB ON TAB.TheKey = IQT.TriggerID
                                WHERE TAB.TheNo > @Skip AND  TAB.TheNo <= @Row
                                ORDER BY TAB.TheNo, TAB.TheModifiedOn, TAB.TheKey";
            sqlString = string.Format(sqlString, lastModifiedOn.ToString("yyyy-MM-dd HH:mm:ss.fff")
                                               , count + skip
                                               , inventoryLocationID
                                               , skip);
            DataTable dt = new DataTable();
            dt.Load(DataService.GetReader(new QueryCommand(sqlString)));
            dt.TableName = "ItemQuantityTrigger";
            ds.Tables.Add(dt);
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
        }

        return ds;
    }

    public static byte[] GetItemTableRealTime(DateTime lastModifiedOn, string outletName, int count)
    {
        DataSet dt = FetchProductRealTime(lastModifiedOn, outletName, count);



        MemoryStream memStream = new MemoryStream();

        GZipStream zipStream = new GZipStream(memStream, CompressionMode.Compress);

        dt.WriteXml(zipStream, XmlWriteMode.WriteSchema);

        zipStream.Close();

        byte[] data = memStream.ToArray();

        memStream.Close();

        return data;
    }

    public static int GetItemTableRealTimeCount(DateTime lastModifiedOn, string outletName)
    {
        DataSet dt = FetchProductRealTime(lastModifiedOn, outletName, int.MaxValue);

        return dt.Tables[0].Rows.Count;
    }

    public static int GetPromoTableRealTimeCount(DateTime lastModifiedOn)
    {
        return GetCountPromoRealTimeDataAllNEW(lastModifiedOn, int.MaxValue);
    }

    public static int GetItemQuantityTriggerTableRealTimeCount(DateTime lastModifiedOn, int inventoryLocationID)
    {
        DataSet dt = FetchItemQuantityTriggerRealTime(lastModifiedOn, inventoryLocationID, int.MaxValue);

        return dt.Tables[0].Rows.Count;
    }

    public static byte[] FetchGroupPrivilegesRealTimeCompressed(int groupID)
    {
        var dt = FetchGroupPrivilegesRealTime(groupID);

        MemoryStream memStream = new MemoryStream();
        GZipStream zipStream = new GZipStream(memStream, CompressionMode.Compress);
        dt.WriteXml(zipStream, XmlWriteMode.WriteSchema);
        zipStream.Close();
        byte[] data = memStream.ToArray();
        memStream.Close();

        return data;
    }

    public static DataSet FetchGroupPrivilegesRealTime(int groupID)
    {
        var dt = new DataTable();

        string sql = "SELECT * FROM GroupUserPrivilege WHERE GroupID = '{0}'";
        sql = string.Format(sql, groupID);
        dt.Load(DataService.GetReader(new QueryCommand(sql)));
        dt.TableName = "GroupUserPrivilege";

        var ds = new DataSet();
        ds.Tables.Add(dt);

        return ds;
    }

    public static DataSet FetchItemSupplierMapRealTime(DateTime lastModifiedOn, int count)
    {
        DataSet ds = new DataSet();
        try
        {
            string sqlString = @"DECLARE @EndDate DATETIME;
                                DECLARE @Row INT;
                                DECLARE @Skip INT;

                                SET @EndDate = '{0}';
                                SET @Row = {1};

                                SELECT ISM.*
                                FROM   ItemSupplierMap ISM
	                                   INNER JOIN (
                                       SELECT	 ItemSupplierMapID TheKey
			                                    ,ModifiedOn TheModifiedOn
			                                    ,DENSE_RANK() OVER(ORDER BY ModifiedOn, ItemSupplierMapID) TheNo
	                                   FROM	ItemSupplierMap 
	                                   WHERE	ModifiedOn > @EndDate  
	                                   ) TAB ON TAB.TheKey = ISM.ItemSupplierMapID
                                ORDER BY TAB.TheNo, TAB.TheModifiedOn, TAB.TheKey";
            sqlString = string.Format(sqlString, lastModifiedOn.ToString("yyyy-MM-dd HH:mm:ss.fff")
                                               , count);
            DataTable dt = new DataTable();
            dt.Load(DataService.GetReader(new QueryCommand(sqlString)));
            dt.TableName = "ItemSupplierMap";
            ds.Tables.Add(dt);
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
        }

        return ds;
    }

    public static DataSet FetchItemSupplierMapRealTime(DateTime lastModifiedOn, int count, int skip)
    {
        DataSet ds = new DataSet();
        try
        {
            string sqlString = @"DECLARE @EndDate DATETIME;
                                DECLARE @Row INT;
                                DECLARE @Skip INT;

                                SET @EndDate = '{0}';
                                SET @Row = {1};
                                SET @Skip = {2};

                                SELECT ISM.*
                                FROM   ItemSupplierMap ISM
	                                   INNER JOIN (
                                       SELECT	 ItemSupplierMapID TheKey
			                                    ,ModifiedOn TheModifiedOn
			                                    ,DENSE_RANK() OVER(ORDER BY ModifiedOn, ItemSupplierMapID) TheNo
	                                   FROM	ItemSupplierMap 
	                                   WHERE	ModifiedOn > @EndDate  
	                                   ) TAB ON TAB.TheKey = ISM.ItemSupplierMapID
                                WHERE TAB.TheNo > @Skip AND  TAB.TheNo <= @Row
                                ORDER BY TAB.TheNo, TAB.TheModifiedOn, TAB.TheKey";
            sqlString = string.Format(sqlString, lastModifiedOn.ToString("yyyy-MM-dd HH:mm:ss.fff")
                                               , count + skip
                                               , skip);
            DataTable dt = new DataTable();
            dt.Load(DataService.GetReader(new QueryCommand(sqlString)));
            dt.TableName = "ItemSupplierMap";
            ds.Tables.Add(dt);
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
        }

        return ds;
    }

    public static int GetItemSupplierMapTableRealTimeCount(DateTime lastModifiedOn)
    {
        DataSet dt = FetchItemSupplierMapRealTime(lastModifiedOn, int.MaxValue);

        return dt.Tables[0].Rows.Count;
    }

    public static DataSet FetchPromoRealTime(DateTime lastModifiedOn, int count, int skip)
    {
        DataSet ds = new DataSet();

        try
        {
            string sqlString = @"DECLARE @EndDate DATETIME;
                                DECLARE @Row INT;
                                DECLARE @Skip INT;

                                SET @EndDate = '{0}';
                                SET @Row = {1};
                                SET @Skip = {2};

                                SELECT PCH.*
                                FROM   PromoCampaignHdr PCH
                                       INNER JOIN (
                                       SELECT	 PromoCampaignHdrID TheKey
                                                ,ModifiedOn TheModifiedOn
                                                ,DENSE_RANK() OVER(ORDER BY ModifiedOn, PromoCampaignHdrID) TheNo
                                       FROM	PromoCampaignHdr PCH
                                       WHERE	ModifiedOn > @EndDate  
                                       ) TAB ON TAB.TheKey = PCH.PromoCampaignHdrID
                                WHERE TAB.TheNo > @Skip AND  TAB.TheNo <= @Row
                                ORDER BY TAB.TheNo, TAB.TheModifiedOn, TAB.TheKey";
            sqlString = string.Format(sqlString, lastModifiedOn.ToString("yyyy-MM-dd HH:mm:ss.fff")
                                               , count + skip
                                               , skip);

            ds = DataService.GetDataSet(new QueryCommand(sqlString));

            PromoCampaignDetCollection idCol = new PromoCampaignDetCollection();
            PromoOutletCollection poCol = new PromoOutletCollection();
            PromoDaysMapCollection pdCol = new PromoDaysMapCollection();
            PromoLocationMapCollection plCol = new PromoLocationMapCollection();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                PromoCampaignHdr ih = new PromoCampaignHdr(dr["PromoCampaignHdrID"].ToString());
                PromoCampaignDetCollection idColTemp = new PromoCampaignDetCollection();
                sqlString = "Select * from PromoCampaignDet where PromoCampaignHdrID ='" + dr["PromoCampaignHdrID"].ToString() + "'";
                DataTable dt = DataService.GetDataSet(new QueryCommand(sqlString)).Tables[0];
                idColTemp.Load(dt);
                idCol.AddRange(idColTemp);

                PromoOutletCollection poColTemp = new PromoOutletCollection();
                sqlString = "Select * from PromoOutlet where PromoCampaignHdrID ='" + dr["PromoCampaignHdrID"].ToString() + "' "; // and OutletName = '" + ps.OutletName + "'";
                DataTable dt1 = DataService.GetDataSet(new QueryCommand(sqlString)).Tables[0];
                poColTemp.Load(dt1);
                poCol.AddRange(poColTemp);

                PromoDaysMapCollection pdColTemp = new PromoDaysMapCollection();
                sqlString = "Select * from PromoDaysMap where PromoCampaignHdrID ='" + dr["PromoCampaignHdrID"].ToString() + "'";
                DataTable dt2 = DataService.GetDataSet(new QueryCommand(sqlString)).Tables[0];
                pdColTemp.Load(dt2);
                pdCol.AddRange(pdColTemp);

                PromoLocationMapCollection plColTemp = new PromoLocationMapCollection();
                sqlString = "Select * from PromoLocationMap where PromoCampaignHdrID ='" + dr["PromoCampaignHdrID"].ToString() + "'";
                DataTable dt3 = DataService.GetDataSet(new QueryCommand(sqlString)).Tables[0];
                plColTemp.Load(dt3);
                plCol.AddRange(plColTemp);
            }
            ds.Tables.Add(idCol.ToDataTable());
            ds.Tables.Add(poCol.ToDataTable());
            ds.Tables.Add(pdCol.ToDataTable());
            ds.Tables.Add(plCol.ToDataTable());

            return ds;
        }

        catch (Exception ex)
        { Logger.writeLog(ex.Message); return ds; }
    }

    public static DataSet FetchItemGroupRealTime(DateTime lastModifiedOn, int count, int skip)
    {
        DataSet ds = new DataSet();

        try
        {
            string sqlString = @"DECLARE @EndDate DATETIME;
                                DECLARE @Row INT;
                                DECLARE @Skip INT;

                                SET @EndDate = '{0}';
                                SET @Row = {1};
                                SET @Skip = {2};

                                SELECT IG.*
                                FROM   ItemGroup IG
                                       INNER JOIN (
                                       SELECT	 ItemGroupId TheKey
                                                ,ModifiedOn TheModifiedOn
                                                ,DENSE_RANK() OVER(ORDER BY ModifiedOn, ItemGroupId) TheNo
                                       FROM	ItemGroup IG
                                       WHERE	ModifiedOn > @EndDate  
                                       ) TAB ON TAB.TheKey = IG.ItemGroupId
                                WHERE TAB.TheNo > @Skip AND  TAB.TheNo <= @Row
                                ORDER BY TAB.TheNo, TAB.TheModifiedOn, TAB.TheKey";
            sqlString = string.Format(sqlString, lastModifiedOn.ToString("yyyy-MM-dd HH:mm:ss.fff")
                                               , count + skip
                                               , skip);

            ds = DataService.GetDataSet(new QueryCommand(sqlString));

            ItemGroupMapCollection idCol = new ItemGroupMapCollection();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ItemGroup ih = new ItemGroup(dr["ItemGroupID"].ToString());
                ItemGroupMapCollection idColTemp = new ItemGroupMapCollection();
                sqlString = "Select * from ItemGroupMap where ItemGroupID ='" + dr["ItemGroupID"].ToString() + "'";
                DataTable dt = DataService.GetDataSet(new QueryCommand(sqlString)).Tables[0];
                idColTemp.Load(dt);
                idCol.AddRange(idColTemp);


            }
            ds.Tables.Add(idCol.ToDataTable());

            return ds;
        }

        catch (Exception ex)
        { Logger.writeLog(ex.Message); return ds; }
    }

    #region *) Appointment

    public static DataSet FetchDataSetRealTimeAppointment(DateTime modifiedDate,
        int noOfRecords, bool usePOSID, int POSID)
    {
        var dt = new DataTable();
        //get Appointment
        var ds = new DataSet();
        string sql = @"DECLARE @EndDate DATETIME;
                        DECLARE @Row INT;

                        SET @EndDate = '{0}';
                        SET @Row = {1};

                        SELECT  I.*
                        FROM	Appointment I
		                        INNER JOIN (
		                        SELECT	 id TheKey
				                        ,ModifiedOn TheModifiedOn
				                        ,DENSE_RANK() OVER(ORDER BY ModifiedOn) TheNo
		                        FROM	Appointment 
		                        WHERE	(ModifiedOn > @EndDate OR IsServerUpdate = 1) {2} 
		                        ) TAB ON TAB.TheKey = Id
                        WHERE	TAB.TheNo <= @Row  
                                {2} 
                        ORDER BY TAB.TheNo, TAB.TheModifiedOn, TAB.TheKey";
        string pos = "";
        if (usePOSID)
            pos = string.Format(" AND PointOfSaleID = '{0}' ", POSID);


        sql = string.Format(sql, modifiedDate.ToString("yyyy-MM-dd HH:mm:ss.fff")
                               , noOfRecords
                               , pos);
        dt.Load(DataService.GetReader(new QueryCommand(sql)));
        dt.TableName = "Appointment";


        ds.Tables.Add(dt);

        // get appointment item
        var dt2 = new DataTable();
        sql = @"DECLARE @EndDate DATETIME;
                        DECLARE @Row INT;

                        SET @EndDate = '{0}';
                        SET @Row = {1};
                        
                        SELECT  IT.*
                        FROM	AppointmentItem IT where AppointmentId in 
                        (
                        SELECT  I.Id
                        FROM	Appointment I
		                        INNER JOIN (
		                        SELECT	 id TheKey
				                        ,ModifiedOn TheModifiedOn
				                        ,DENSE_RANK() OVER(ORDER BY ModifiedOn) TheNo
		                        FROM	Appointment 
		                        WHERE	(ModifiedOn > @EndDate OR IsServerUpdate = 1) {2} 
		                        ) TAB ON TAB.TheKey = Id
                        WHERE	TAB.TheNo <= @Row  
                                {2} 
                        )";
        pos = "";
        if (usePOSID)
            pos = string.Format(" AND PointOfSaleID = '{0}' ", POSID);


        sql = string.Format(sql, modifiedDate.ToString("yyyy-MM-dd HH:mm:ss.fff")
                               , noOfRecords
                               , pos);
        dt2.Load(DataService.GetReader(new QueryCommand(sql)));
        dt2.TableName = "AppointmentItem";

        ds.Tables.Add(dt2);

        return ds;
    }

    public static byte[] FetchDataSetRealTimeCompressedAppointment(DateTime modifiedDate, int noOfRecords, bool usePOSID, int POSID)
    {
        var dt = FetchDataSetRealTimeAppointment(modifiedDate, noOfRecords, usePOSID, POSID);

        MemoryStream memStream = new MemoryStream();
        GZipStream zipStream = new GZipStream(memStream, CompressionMode.Compress);
        dt.WriteXml(zipStream, XmlWriteMode.WriteSchema);
        zipStream.Close();
        byte[] data = memStream.ToArray();
        memStream.Close();

        return data;
    }

    public static bool UpdateAppointmentIsUpdated(string listID)
    {
        return AppointmentController.UpdateAppointmentIsUpdated(listID);
    }

    #endregion

    #region *) Upload Delivery Order
    public static bool FetchDeliveryOrderDataRealTime(string[][] dsHeaders, string[][] dsDetails, string[][] dsMembers, string[][] dsDeposit)
    {
        Logger.writeLog("Delivery Order Synchronization started.");

        try
        {
            QueryCommandCollection qr = new QueryCommandCollection();
            qr.AddRange(FetchMemberFromAppointmentRealTime(dsMembers));
            qr.AddRange(FetchDeliveryOrderRealTime(dsHeaders));
            qr.AddRange(FetchDeliveryOrderDetailsRealTime(dsDetails));

            #region *) Update Deposit Amount in OrderDet
            if (dsDeposit != null)
            {
                for (int i = 0; i < dsDeposit.Length; i++)
                {
                    OrderDet od = new OrderDet(dsDeposit[i][0]);
                    if (!string.IsNullOrEmpty(od.OrderDetID))
                    {
                        if (!string.IsNullOrEmpty(dsDeposit[i][1]))
                        {
                            od.DepositAmount = Convert.ToDecimal(dsDeposit[i][1]);
                            qr.Add(od.GetUpdateCommand("SYNC"));
                        }
                    }
                }
            }
            #endregion

            DataService.ExecuteTransaction(qr);

            InventoryController.AssignStockOutToPreOrderSalesUsingTransaction();

            Logger.writeLog("Delivery Order Synchronization finishes.");
            return true;
        }
        catch (Exception ex)
        {
            Logger.writeLog("Delivery Order Synchronization failed. Please check your log file");
            Logger.writeLog(ex);
            return false;
        }
    }

    private static QueryCommandCollection FetchDeliveryOrderRealTime(string[][] objHdr)
    {
        try
        {
            Query qry;
            Object tmpGuid;


            ArrayList duplicate = new ArrayList();

            //ORDER HEADER
            //Logger.writeLog("#records>" + objHdr.Length);

            DeliveryOrder app = new DeliveryOrder();

            QueryCommand qr = app.GetInsertCommand("SYNC");
            int UniqueIDIndex;
            UniqueIDIndex = -1;
            for (int f = 0; f < qr.Parameters.Count; f++)
            {
                if (qr.Parameters[f].ParameterName.ToLower() == "@uniqueid")
                {
                    UniqueIDIndex = f;
                    break;
                }
            }
            if (UniqueIDIndex == -1)
            {
                Logger.writeLog("Sync table does not contain unique ID or unique ID name does not compy.");
                throw new Exception("Sync table does not contain unique ID or unique ID name does not compy.");
            }

            QueryCommandCollection col = new QueryCommandCollection();
            ArrayList allList = new ArrayList();
            for (int i = 0; i < objHdr.Length; i++)
            {
                allList.Add(objHdr[i][0]);
            }

            //Check GUID uniqueness in Target DB
            qry = new Query("DeliveryOrder");
            qry.SelectList = "OrderNumber,UniqueID";
            DataTable dtUniqueID = qry.IN(DeliveryOrder.Columns.OrderNumber, allList).ExecuteDataSet().Tables[0];

            for (int i = 0; i < objHdr.Length; i++)
            {
                if (dtUniqueID.Rows.Count > 0)
                {
                    DataRow[] dr = dtUniqueID.Select("OrderNumber = '" + objHdr[i][0] + "'");

                    if (dr.Length > 0)
                        tmpGuid = new Guid(dr[0]["UniqueID"].ToString());
                    else
                        tmpGuid = new Object();
                }
                else tmpGuid = new Object();

                if ((dtUniqueID.Rows.Count > 0 && tmpGuid is Guid) && ((Guid)tmpGuid != new Guid(objHdr[i][UniqueIDIndex]))) //GUID Found?
                {
                    //same order but different unique ID - Houston we have a problem                        
                    duplicate.Add(qr.Parameters[0].ParameterValue.ToString());
                }
                else
                {

                    if ((dtUniqueID.Rows.Count > 0 && tmpGuid is Guid) && (((Guid)tmpGuid).ToString() == (new Guid(objHdr[i][UniqueIDIndex])).ToString()))
                    {
                        qr = app.GetInsertCommand("SYNC");

                        Query qr2 = new Query("DeliveryOrder");
                        qr2.AddWhere(DeliveryOrder.Columns.UniqueID, new Guid(objHdr[i][UniqueIDIndex]));
                        qr2.QueryType = QueryType.Update;

                        for (int oa = 0; oa < qr.Parameters.Count; oa++)
                        {
                            if (oa >= objHdr[i].Length) continue;
                            if (qr.Parameters[oa].ParameterName.ToLower() != "@createdon" &
                                       qr.Parameters[oa].ParameterName.ToLower() != "@createdby" &
                                        qr.Parameters[oa].ParameterName.ToLower() != "@modifiedon" &
                                        qr.Parameters[oa].ParameterName.ToLower() != "@modifiedby")
                            {
                                Logger.writeLog(qr.Parameters[oa].ParameterName + " val: " + objHdr[i][oa]);
                                switch (qr.Parameters[oa].ParameterName.ToLower())
                                {
                                    case "@deliverydate":
                                        if (objHdr[i][oa] != null && objHdr[i][oa] != string.Empty)
                                            qr2.AddUpdateSetting(DeliveryOrder.Columns.DeliveryDate, DateTime.Parse(objHdr[i][oa]));
                                        break;
                                    case "@deliveryaddress":
                                        if (objHdr[i][oa] != null)
                                            qr2.AddUpdateSetting(DeliveryOrder.Columns.DeliveryAddress, objHdr[i][oa]);
                                        break;
                                    case "@timeslotfrom":
                                        if (objHdr[i][oa] != null && objHdr[i][oa] != string.Empty)
                                            qr2.AddUpdateSetting(DeliveryOrder.Columns.TimeSlotFrom, DateTime.Parse(objHdr[i][oa]));
                                        break;
                                    case "@timeslotto":
                                        if (objHdr[i][oa] != null && objHdr[i][oa] != string.Empty)
                                            qr2.AddUpdateSetting(DeliveryOrder.Columns.TimeSlotTo, DateTime.Parse(objHdr[i][oa]));
                                        break;
                                    case "@deleted":
                                        if (objHdr[i][oa] == "0")
                                            qr2.AddUpdateSetting(DeliveryOrder.Columns.Deleted, false);
                                        else if (objHdr[i][oa] == "1")
                                            qr2.AddUpdateSetting(DeliveryOrder.Columns.Deleted, true);
                                        break;
                                    case "@membershipno":
                                        if (objHdr[i][oa] != null && objHdr[i][oa] != string.Empty)
                                            qr2.AddUpdateSetting(DeliveryOrder.Columns.MembershipNo, objHdr[i][oa]);
                                        break;
                                    case "@recipientname":
                                        if (objHdr[i][oa] != null)
                                            qr2.AddUpdateSetting(DeliveryOrder.Columns.RecipientName, objHdr[i][oa]);
                                        break;
                                    case "@mobileno":
                                        if (objHdr[i][oa] != null)
                                            qr2.AddUpdateSetting(DeliveryOrder.Columns.MobileNo, objHdr[i][oa]);
                                        break;
                                    case "@homeno":
                                        if (objHdr[i][oa] != null)
                                            qr2.AddUpdateSetting(DeliveryOrder.Columns.HomeNo, objHdr[i][oa]);
                                        break;
                                    case "@postalcode":
                                        if (objHdr[i][oa] != null)
                                            qr2.AddUpdateSetting(DeliveryOrder.Columns.PostalCode, objHdr[i][oa]);
                                        break;
                                    case "@unitno":
                                        if (objHdr[i][oa] != null)
                                            qr2.AddUpdateSetting(DeliveryOrder.Columns.UnitNo, objHdr[i][oa]);
                                        break;
                                    case "@remark":
                                        if (objHdr[i][oa] != null)
                                            qr2.AddUpdateSetting(DeliveryOrder.Columns.Remark, objHdr[i][oa]);
                                        break;
                                    case "@isvendordelivery":
                                        if (objHdr[i][oa] == "0")
                                            qr2.AddUpdateSetting(DeliveryOrder.Columns.IsVendorDelivery, false);
                                        else if (objHdr[i][oa] == "1")
                                            qr2.AddUpdateSetting(DeliveryOrder.Columns.IsVendorDelivery, true);
                                        break;
                                    case "@deliveryoutlet":
                                        if (objHdr[i][oa] != null)
                                            qr2.AddUpdateSetting(DeliveryOrder.Columns.DeliveryOutlet, objHdr[i][oa]);
                                        break;
                                    case "@isdelivered":
                                        if (objHdr[i][oa] == "0")
                                            qr2.AddUpdateSetting(DeliveryOrder.Columns.IsDelivered, false);
                                        else if (objHdr[i][oa] == "1")
                                            qr2.AddUpdateSetting(DeliveryOrder.Columns.IsDelivered, true);
                                        break;
                                    case "@isserverupdate":
                                        qr2.AddUpdateSetting(DeliveryOrder.Columns.IsServerUpdate, false);
                                        break;
                                }
                            }
                        }
                        QueryCommand cmd = qr2.BuildUpdateCommand();
                        col.Add(cmd);
                        //QueryCommand cmdDelete = new QueryCommand("Delete from AppointmentItem where AppointmentId = '" + tmpGuid.ToString() + "'");
                        //col.Add(cmdDelete);
                    }
                    else
                    {
                        //New Item. Perform Create
                        qr = app.GetInsertCommand("SYNC");

                        for (int oa = 0; oa < qr.Parameters.Count; oa++)
                        {
                            if (oa >= objHdr[i].Length) continue;
                            if (qr.Parameters[oa].ParameterName.ToLower() != "@createdon" &
                                       qr.Parameters[oa].ParameterName.ToLower() != "@createdby" &
                                        qr.Parameters[oa].ParameterName.ToLower() != "@modifiedon" &
                                        qr.Parameters[oa].ParameterName.ToLower() != "@modifiedby")
                            {
                                Logger.writeLog(qr.Parameters[oa].ParameterName + " val: " + objHdr[i][oa]);
                                switch (qr.Parameters[oa].DataType)
                                {
                                    case DbType.Boolean:
                                        if (objHdr[i][oa] == "0")
                                            qr.Parameters[oa].ParameterValue = false;
                                        else if (objHdr[i][oa] == "1")
                                            qr.Parameters[oa].ParameterValue = true;
                                        break;
                                    case DbType.Date:
                                    case DbType.DateTime:
                                        Logger.writeLog(qr.Parameters[oa].ParameterName.ToLower() + " " + objHdr[i][oa]);
                                        if (objHdr[i][oa] != null && objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = DateTime.Parse(objHdr[i][oa]);
                                        break;
                                    case DbType.Decimal:
                                    case DbType.Currency:
                                        if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Decimal.Parse(objHdr[i][oa]);
                                        break;
                                    case DbType.Double:
                                        if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Double.Parse(objHdr[i][oa]);
                                        break;
                                    case DbType.Single:
                                        if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Single.Parse(objHdr[i][oa]);
                                        break;
                                    case DbType.Guid:
                                        if (objHdr[i][oa] != string.Empty)
                                            qr.Parameters[oa].ParameterValue = new Guid(objHdr[i][oa]);
                                        break;
                                    case DbType.Int16:
                                        if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int16.Parse(objHdr[i][oa]);
                                        break;
                                    case DbType.Int32:
                                        if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int32.Parse(objHdr[i][oa]);
                                        break;
                                    case DbType.Int64:
                                        if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int64.Parse(objHdr[i][oa]);
                                        break;
                                    case DbType.UInt16:
                                        if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt16.Parse(objHdr[i][oa]);
                                        break;
                                    case DbType.UInt32:
                                        if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt32.Parse(objHdr[i][oa]);
                                        break;
                                    case DbType.UInt64:
                                        if (objHdr[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt64.Parse(objHdr[i][oa]);
                                        break;
                                    case DbType.StringFixedLength:
                                    case DbType.String:
                                        qr.Parameters[oa].ParameterValue = objHdr[i][oa];
                                        break;
                                    default:
                                        qr.Parameters[oa].ParameterValue = objHdr[i][oa];
                                        break;
                                }
                                if (qr.Parameters[oa].ParameterName.ToLower().Equals("@membershipno")
                                        && qr.Parameters[oa].ParameterValue.ToString().Equals(""))
                                {
                                    qr.Parameters[oa].ParameterValue = null;
                                }
                                if (qr.Parameters[oa].ParameterName.ToLower().Equals("@isserverupdate"))
                                {
                                    qr.Parameters[oa].ParameterValue = false;
                                }
                            }
                        }

                        col.Add(qr);
                    }

                }
            }
            if (duplicate.Count != 0)
            {
                string str;
                str = "Error: Attempting to enter duplicate Delivery Order .";
                str = str + "\r\n" + "Duplicates:" + "\r\n";
                for (int i = 0; i < duplicate.Count; i++)
                    str += duplicate[i] + "\r\n";

                Logger.writeLog(str);
                throw new Exception("Duplicate found");
            }

            return col;

        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
            throw ex;
        }
    }

    private static QueryCommandCollection FetchDeliveryOrderDetailsRealTime(string[][] objDet)
    {
        try
        {
            //ArrayList approvedList = new ArrayList();
            QueryCommandCollection col = new QueryCommandCollection();
            Logger.writeLog("#records>" + objDet.Length);
            int UniqueIDIndex;

            DeliveryOrderDetail ord = new DeliveryOrderDetail();

            QueryCommand qr = ord.GetInsertCommand("SYNC");
            UniqueIDIndex = -1;

            for (int f = 0; f < qr.Parameters.Count; f++)
            {
                if (qr.Parameters[f].ParameterName.ToLower() == "@uniqueid")
                {
                    UniqueIDIndex = f;
                    break;
                }
            }

            if (UniqueIDIndex == -1)
            {
                Logger.writeLog("Sync table does not contain uniqueid.");
                throw new Exception("Sync table does not contain uniqueid.");
            }
            ArrayList allList = new ArrayList();
            for (int i = 0; i < objDet.Length; i++)
            {
                allList.Add(objDet[i][0]);
            }

            Object tmpGuid;
            ArrayList duplicate = new ArrayList();
            //Check GUID uniqueness in Target DB
            Query qry = new Query("DeliveryOrderDetails");
            qry.SelectList = "DetailsID,UniqueID";
            DataTable dtUniqueID = qry.IN(DeliveryOrderDetail.Columns.DetailsID, allList).ExecuteDataSet().Tables[0];

            for (int i = 0; i < objDet.Length; i++)
            {
                if (dtUniqueID.Rows.Count > 0)
                {
                    DataRow[] dr = dtUniqueID.Select("DetailsID = '" + objDet[i][0] + "'");

                    if (dr.Length > 0)
                        tmpGuid = new Guid(dr[0]["UniqueID"].ToString());
                    else
                        tmpGuid = new Object();
                }
                else tmpGuid = new Object();

                if (dtUniqueID.Rows.Count > 0 && tmpGuid is Guid) //GUID Found?
                {
                    //if ((Guid)tmpGuid != new Guid(objDet[i][UniqueIDIndex]))
                    //{
                    //    //same order but different unique ID - Houston we have a problem                        
                    //    duplicate.Add(qr.Parameters[0].ParameterValue.ToString());
                    //}

                    qr = ord.GetInsertCommand("SYNC");
                    for (int oa = 0; oa < qr.Parameters.Count; oa++)
                    {
                        if (oa >= objDet[i].Length) continue;
                        if (qr.Parameters[oa].ParameterName.ToLower() != "@createdon" &
                                   qr.Parameters[oa].ParameterName.ToLower() != "@createdby" &
                                    qr.Parameters[oa].ParameterName.ToLower() != "@modifiedon" &
                                    qr.Parameters[oa].ParameterName.ToLower() != "@modifiedby")
                        {
                            switch (qr.Parameters[oa].DataType)
                            {
                                case DbType.Boolean:
                                    if (objDet[i][oa] == "0")
                                        qr.Parameters[oa].ParameterValue = false;
                                    else if (objDet[i][oa] == "1")
                                        qr.Parameters[oa].ParameterValue = true;
                                    break;
                                case DbType.Date:
                                case DbType.DateTime:
                                    if (objDet[i][oa] != null && objDet[i][oa] != string.Empty)
                                        qr.Parameters[oa].ParameterValue = DateTime.Parse(objDet[i][oa]);
                                    break;
                                case DbType.Decimal:
                                case DbType.Currency:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Decimal.Parse(objDet[i][oa]);
                                    break;
                                case DbType.Double:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Double.Parse(objDet[i][oa]);
                                    break;
                                case DbType.Single:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Single.Parse(objDet[i][oa]);
                                    break;
                                case DbType.Guid:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = new Guid(objDet[i][oa]);
                                    break;
                                case DbType.Int16:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int16.Parse(objDet[i][oa]);
                                    break;
                                case DbType.Int32:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int32.Parse(objDet[i][oa]);
                                    break;
                                case DbType.Int64:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int64.Parse(objDet[i][oa]);
                                    break;
                                case DbType.UInt16:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt16.Parse(objDet[i][oa]);
                                    break;
                                case DbType.UInt32:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt32.Parse(objDet[i][oa]);
                                    break;
                                case DbType.UInt64:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt64.Parse(objDet[i][oa]);
                                    break;
                                case DbType.StringFixedLength:
                                case DbType.String:
                                    qr.Parameters[oa].ParameterValue = objDet[i][oa];
                                    break;
                                default:
                                    qr.Parameters[oa].ParameterValue = objDet[i][oa];
                                    break;
                            }
                        }
                    }

                    ord = new DeliveryOrderDetail(objDet[i][0]);
                    if (qr.Parameters.GetParameter("@Quantity").ParameterValue is decimal)
                        ord.Quantity = (decimal)qr.Parameters.GetParameter("@Quantity").ParameterValue;
                    if (qr.Parameters.GetParameter("@Remarks").ParameterValue is string)
                        ord.Remarks = (string)qr.Parameters.GetParameter("@Remarks").ParameterValue;
                    if (qr.Parameters.GetParameter("@Deleted").ParameterValue is bool)
                        ord.Deleted = (bool)qr.Parameters.GetParameter("@Deleted").ParameterValue;

                    QueryCommand qrUpdate = ord.GetUpdateCommand("SYNC");
                    if (qrUpdate.Parameters.Count > 3) // If only 3 (@DetailsID, @ModifiedBy, @ModifiedOn) then no need to update because no data has changed.
                    {
                        if (qrUpdate.CommandSql.IndexOf("[ModifiedBy] = @ModifiedBy, [ModifiedOn] = @ModifiedOn,") > 0)
                        {
                            qrUpdate.CommandSql = qrUpdate.CommandSql.Remove(qrUpdate.CommandSql.IndexOf("[ModifiedBy] = @ModifiedBy, [ModifiedOn] = @ModifiedOn,"), 55);
                            qrUpdate.Parameters.GetParameter("@ModifiedOn").ParameterValue = ord.ModifiedOn;
                        }
                        else
                        {
                            qrUpdate.CommandSql = qrUpdate.CommandSql.Remove(qrUpdate.CommandSql.IndexOf(", [ModifiedOn] = @ModifiedOn"), 28);
                            qrUpdate.Parameters.GetParameter("@ModifiedOn").ParameterValue = ord.ModifiedOn;
                        }

                        col.Add(qrUpdate);
                    }
                }
                else
                {
                    //It's Approved! - else ignore....
                    qr = ord.GetInsertCommand("SYNC");
                    for (int oa = 0; oa < qr.Parameters.Count; oa++)
                    {
                        if (oa >= objDet[i].Length) continue;
                        if (qr.Parameters[oa].ParameterName.ToLower() != "@createdon" &
                                   qr.Parameters[oa].ParameterName.ToLower() != "@createdby" &
                                    qr.Parameters[oa].ParameterName.ToLower() != "@modifiedon" &
                                    qr.Parameters[oa].ParameterName.ToLower() != "@modifiedby")
                        {
                            switch (qr.Parameters[oa].DataType)
                            {
                                case DbType.Boolean:
                                    if (objDet[i][oa] == "0")
                                        qr.Parameters[oa].ParameterValue = false;
                                    else if (objDet[i][oa] == "1")
                                        qr.Parameters[oa].ParameterValue = true;
                                    break;
                                case DbType.Date:
                                case DbType.DateTime:
                                    if (objDet[i][oa] != null && objDet[i][oa] != string.Empty)
                                        qr.Parameters[oa].ParameterValue = DateTime.Parse(objDet[i][oa]);
                                    break;
                                case DbType.Decimal:
                                case DbType.Currency:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Decimal.Parse(objDet[i][oa]);
                                    break;
                                case DbType.Double:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Double.Parse(objDet[i][oa]);
                                    break;
                                case DbType.Single:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Single.Parse(objDet[i][oa]);
                                    break;
                                case DbType.Guid:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = new Guid(objDet[i][oa]);
                                    break;
                                case DbType.Int16:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int16.Parse(objDet[i][oa]);
                                    break;
                                case DbType.Int32:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int32.Parse(objDet[i][oa]);
                                    break;
                                case DbType.Int64:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = Int64.Parse(objDet[i][oa]);
                                    break;
                                case DbType.UInt16:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt16.Parse(objDet[i][oa]);
                                    break;
                                case DbType.UInt32:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt32.Parse(objDet[i][oa]);
                                    break;
                                case DbType.UInt64:
                                    if (objDet[i][oa] != string.Empty) qr.Parameters[oa].ParameterValue = UInt64.Parse(objDet[i][oa]);
                                    break;
                                case DbType.StringFixedLength:
                                case DbType.String:
                                    qr.Parameters[oa].ParameterValue = objDet[i][oa];
                                    break;
                                default:
                                    qr.Parameters[oa].ParameterValue = objDet[i][oa];
                                    break;
                            }
                        }
                    }
                    col.Add(qr);
                }
            }
            return col;
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
            throw ex;
        }
    }

    public static bool CreateDeliveryPreOrderSingleOrderDetForWeb(string orderdetid, int qty, int personnelID, int pointOfSaleID, byte[] signature, out string status)
    {
        status = "";
        try
        {
            POSController pos = new POSController();

            Bitmap bmp = null;
            if (signature != null)
            {
                using (var ms = new MemoryStream(signature))
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    bmp = new Bitmap(ms);
                }
            }
            if (!pos.CreateDeliveryPreOrderSingleOrderDet(orderdetid, qty, personnelID, pointOfSaleID, bmp, true))
            {
                throw new Exception("Error create single delivery, please contact Administrator");
            }

            InventoryController.AssignStockOutToPreOrderSalesUsingTransaction();

            return true;
        }
        catch (Exception ex)
        {
            status = ex.Message;
            Logger.writeLog(ex.Message);
            return false;
        }
    }

    public static bool SendNotifiyMailDelivery(string orderHdrID, string orderDetID, out string status)
    {
        status = "";
        try
        {
            POSController pos = new POSController();
            OrderHdr oh = new OrderHdr(orderHdrID);
            OrderDetCollection odcol = new OrderDetCollection();
            OrderDet od = new OrderDet(orderDetID);
            odcol.Add(od);

            var member = new PowerPOS.Membership(oh.MembershipNo);
            pos.myOrderHdr = oh;
            pos.myOrderDet = odcol;
            PointOfSaleController.GetPointOfSaleInfo();

            string EmailTo = "";
            string EmailSubject = "";
            string EmailBody = "";
            string EmailBcc = "";
            if (member != null)
            {
                if (!string.IsNullOrEmpty(member.Email))
                    EmailTo = member.Email;
                else
                    EmailTo = AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_DefaultMailTo);

                string useForReceiptNo = AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_ReceiptNoInEmailReceipt);
                string receiptNo = "";
                if (string.IsNullOrEmpty(useForReceiptNo) || useForReceiptNo.ToLower() == "orderhdrid")
                    receiptNo = oh.OrderHdrID;
                else if (useForReceiptNo.ToLower() == "custom invoice no")
                    receiptNo = oh.Userfld5;
                else if (useForReceiptNo.ToLower() == "line info")
                    receiptNo = od.LineInfo;
                else
                    receiptNo = oh.OrderHdrID;

                EmailSubject = string.Format("Receipt {0} for purchase at {1}", receiptNo, CompanyInfo.CompanyName);
                EmailBody = "Please find the receipt attachment";
            }

            #region *) Send BCC if necessary
            bool sendBcc = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_BccToOwner), false);
            string ownerEmail = AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_OwnerEmailAddress);
            if (sendBcc && !string.IsNullOrEmpty(ownerEmail))
            {
                EmailBcc = ownerEmail;
            }
            #endregion

            if (!POSDeviceController.SendMailNotifyDelivery(pos, EmailTo, EmailSubject, EmailBody, EmailBcc))
                throw new Exception("Error when Send Email. Please tell the Administrator");

            return true;
        }
        catch (Exception ex)
        {
            status = "Error Send Email Notify Delivery: " + ex.Message;
            Logger.writeLog("Error Send Email Notify Delivery: " + ex.Message);

            return false;
        }
    }

    public static bool AutoAssignDepositWhenpayInstallment(string orderHdrID, decimal amount, out string status)
    {
        status = "";

        try
        {
            OrderHdr refOH = new OrderHdr(orderHdrID);
            QueryCommandCollection cmdColl = new QueryCommandCollection();
            OrderDetCollection refOD = refOH.OrderDetRecords();
            decimal remainingAmt = amount;
            foreach (OrderDet od in refOD)
            {
                if (od.Amount == null) od.Amount = 0;
                if (od.DepositAmount == null) od.DepositAmount = 0;

                if (od.Amount > 0 && od.Amount > od.DepositAmount)
                {
                    decimal discrepancy = od.Amount - od.DepositAmount;
                    if (discrepancy <= remainingAmt)
                    {
                        od.DepositAmount += discrepancy;
                        remainingAmt -= discrepancy;
                    }
                    else
                    {
                        od.DepositAmount += remainingAmt;
                        remainingAmt = 0;
                    }

                    if (remainingAmt <= 0) break;
                }
            }

            cmdColl.AddRange(refOD.GetSaveCommands(UserInfo.username));
            DataService.ExecuteTransaction(cmdColl);

            return true;
        }
        catch (Exception ex)
        {
            status = "Error When Auto Assign Deposit: " + ex.Message;
            Logger.writeLog("Error When Auto Assign Deposit: " + ex.Message);
            return false;
        }
    }

    #endregion

    #region *) Download Delivery Order
    public static int FetchRecordNoDeliveryOrder(DateTime modifiedOn, int PointOfSaleID)
    {
        int count = 0;

        try
        {
            string sql = "SELECT COUNT(*) NoOfRecords FROM DeliveryOrder do INNER JOIN OrderHdr oh ON oh.OrderHdrID = do.SalesOrderRefNo where (do.ModifiedOn > '" + modifiedOn.ToString("yyyy-MM-dd HH:mm:ss.fff") + "' OR ISNULL(do.IsServerUpdate, 0) = 1) AND oh.PointOfSaleID = " + PointOfSaleID.ToString();
            DataTable dt = new DataTable();
            dt.Load(DataService.GetReader(new QueryCommand(sql, "PowerPOS")));

            if (dt.Rows.Count > 0)
                count = (int)dt.Rows[0]["NoOfRecords"];
        }
        catch (Exception ex)
        {
            count = 0;
            Logger.writeLog(">> FetchRecordNoByTimestamp ERROR on table DeliveryOrder ");
            Logger.writeLog(ex);
        }

        return count;
    }

    public static byte[] FetchDataSetRealTimeCompressedDeliveryOrder(DateTime modifiedDate, int noOfRecords, int POSID)
    {
        var dt = FetchDataSetRealTimeDeliveryOrder(modifiedDate, noOfRecords, POSID);

        MemoryStream memStream = new MemoryStream();
        GZipStream zipStream = new GZipStream(memStream, CompressionMode.Compress);
        dt.WriteXml(zipStream, XmlWriteMode.WriteSchema);
        zipStream.Close();
        byte[] data = memStream.ToArray();
        memStream.Close();

        return data;
    }

    public static DataSet FetchDataSetRealTimeDeliveryOrder(DateTime modifiedDate, int noOfRecords, int POSID)
    {
        var dt = new DataTable();

        //get DeliveryOrder
        var ds = new DataSet();
        string sql = @"DECLARE @EndDate DATETIME;
                        DECLARE @Row INT;

                        SET @EndDate = '{0}';
                        SET @Row = {1};

                        SELECT  I.*
                        FROM	DeliveryOrder I
		                        INNER JOIN (
		                        SELECT	 do.OrderNumber TheKey
				                        ,do.ModifiedOn TheModifiedOn
				                        ,DENSE_RANK() OVER(ORDER BY do.ModifiedOn) TheNo
		                        FROM	DeliveryOrder do 
                                INNER JOIN OrderHdr oh ON oh.OrderHdrID = do.SalesOrderRefNo 
		                        WHERE	(do.ModifiedOn > @EndDate OR ISNULL(do.IsServerUpdate, 0) = 1) AND oh.PointOfSaleID = {2}  
		                        ) TAB ON TAB.TheKey = I.OrderNumber
                        WHERE	TAB.TheNo <= @Row  
                        ORDER BY TAB.TheNo, TAB.TheModifiedOn, TAB.TheKey";

        sql = string.Format(sql, modifiedDate.ToString("yyyy-MM-dd HH:mm:ss.fff")
                               , noOfRecords
                               , POSID);
        dt.Load(DataService.GetReader(new QueryCommand(sql, "PowerPOS")));
        dt.TableName = "DeliveryOrder";

        ds.Tables.Add(dt);

        // get Delivery Order Details
        var dt2 = new DataTable();
        sql = @"DECLARE @EndDate DATETIME;
                        DECLARE @Row INT;

                        SET @EndDate = '{0}';
                        SET @Row = {1};
                        
                        SELECT  IT.*
                        FROM	DeliveryOrderDetails IT where DOHDRID in 
                        (
                        SELECT  I.OrderNumber
                        FROM	DeliveryOrder I
		                        INNER JOIN (
		                        SELECT	 do.OrderNumber TheKey
				                        ,do.ModifiedOn TheModifiedOn
				                        ,DENSE_RANK() OVER(ORDER BY do.ModifiedOn) TheNo
		                        FROM	DeliveryOrder do 
                                INNER JOIN OrderHdr oh ON oh.OrderHdrID = do.SalesOrderRefNo 
		                        WHERE	(do.ModifiedOn > @EndDate OR ISNULL(do.IsServerUpdate, 0) = 1) AND oh.PointOfSaleID = {2}  
		                        ) TAB ON TAB.TheKey = I.OrderNumber
                        WHERE	TAB.TheNo <= @Row  
                        )";

        sql = string.Format(sql, modifiedDate.ToString("yyyy-MM-dd HH:mm:ss.fff")
                               , noOfRecords
                               , POSID);
        dt2.Load(DataService.GetReader(new QueryCommand(sql, "PowerPOS")));
        dt2.TableName = "DeliveryOrderDetails";

        ds.Tables.Add(dt2);

        // get Deposit Amount from OrderDet
        var dt3 = new DataTable();
        sql = @"DECLARE @EndDate DATETIME;
                        DECLARE @Row INT;

                        SET @EndDate = '{0}';
                        SET @Row = {1};
                        
                        SELECT OrderDetID, Userfloat1 AS DepositAmount 
                        FROM OrderDet WHERE OrderDetID in 
                        (
                        SELECT  IT.OrderDetID
                        FROM	DeliveryOrderDetails IT where DOHDRID in 
                        (
                        SELECT  I.OrderNumber
                        FROM	DeliveryOrder I
		                        INNER JOIN (
		                        SELECT	 do.OrderNumber TheKey
				                        ,do.ModifiedOn TheModifiedOn
				                        ,DENSE_RANK() OVER(ORDER BY do.ModifiedOn) TheNo
		                        FROM	DeliveryOrder do 
                                INNER JOIN OrderHdr oh ON oh.OrderHdrID = do.SalesOrderRefNo 
		                        WHERE	(do.ModifiedOn > @EndDate OR ISNULL(do.IsServerUpdate, 0) = 1) AND oh.PointOfSaleID = {2}  
		                        ) TAB ON TAB.TheKey = I.OrderNumber
                        WHERE	TAB.TheNo <= @Row  
                        )
                        )";

        sql = string.Format(sql, modifiedDate.ToString("yyyy-MM-dd HH:mm:ss.fff")
                               , noOfRecords
                               , POSID);
        dt3.Load(DataService.GetReader(new QueryCommand(sql, "PowerPOS")));
        dt3.TableName = "DepositAmount";

        ds.Tables.Add(dt3);

        return ds;
    }

    public static bool UpdateDeliveryOrderIsUpdated(string listID)
    {
        return DeliveryOrderController.UpdateDeliveryOrderIsUpdated(listID);
    }
    #endregion

    #region *) Special Discount
    public static byte[] FetchDataSetRealTimeCompressedSpecialDiscounts(DateTime modifiedDate, int noOfRecords, string outletName)
    {
        var dt = FetchDataSetRealTimeSpecialDiscounts(modifiedDate, noOfRecords, outletName);

        MemoryStream memStream = new MemoryStream();
        GZipStream zipStream = new GZipStream(memStream, CompressionMode.Compress);
        dt.WriteXml(zipStream, XmlWriteMode.WriteSchema);
        zipStream.Close();
        byte[] data = memStream.ToArray();
        memStream.Close();

        return data;
    }

    public static DataSet FetchDataSetRealTimeSpecialDiscounts(DateTime modifiedDate, int noOfRecords, string outletName)
    {
        var dt = new DataTable();
        //get Special Discounts
        var ds = new DataSet();
        string sql = @"
                        DECLARE @EndDate DATETIME;
                        DECLARE @Row INT;
                        DECLARE @OutletName VARCHAR(50);

                        SET @EndDate = '{0}';
                        SET @Row = {1};
                        SET @OutletName = '{2}';

                        SELECT  SpecialDiscountID, 
                                DiscountName, 
                                DiscountPercentage, 
                                ShowLabel, 
                                PriorityLevel, 
                                Deleted = CASE 
                                            WHEN Deleted = 1 THEN 1
                                            WHEN (AssignedOutlet = 'ALL' OR ISNULL(CHARINDEX(@OutletName, NULLIF(AssignedOutlet, ''), 0), 1) > 0) THEN 0 
                                            ELSE 1 
                                          END, 
                                CreatedOn, 
                                CreatedBy, 
                                ModifiedOn, 
                                ModifiedBy, 
                                UseSPP, 
                                Enabled, 
                                ApplicableToAllItem, 
                                StartDate, 
                                EndDate, 
                                MinimumSpending, 
                                isBankPromo, 
                                DiscountLabel, 
                                AssignedOutlet
                        FROM	SpecialDiscounts S
		                        INNER JOIN (
		                                SELECT	 DiscountName TheKey
				                                ,ModifiedOn TheModifiedOn
				                                ,DENSE_RANK() OVER(ORDER BY ModifiedOn) TheNo
		                                FROM	SpecialDiscounts 
		                                WHERE	(ModifiedOn > @EndDate)
		                            ) TAB ON TAB.TheKey = DiscountName
                        WHERE	TAB.TheNo <= @Row  
                        ORDER BY TAB.TheNo, TAB.TheModifiedOn, TAB.TheKey
                      ";

        sql = string.Format(sql, modifiedDate.ToString("yyyy-MM-dd HH:mm:ss.fff")
                               , noOfRecords
                               , outletName);
        dt.Load(DataService.GetReader(new QueryCommand(sql)));
        dt.TableName = "SpecialDiscounts";
        ds.Tables.Add(dt);
        return ds;
    }

    #endregion

    #region *) Voucher
    public static byte[] FetchDataSetRealTimeCompressedVoucherHeader(DateTime modifiedDate, int noOfRecords, string outletName)
    {
        var dt = FetchDataSetRealTimeVoucherHeader(modifiedDate, noOfRecords, outletName);

        MemoryStream memStream = new MemoryStream();
        GZipStream zipStream = new GZipStream(memStream, CompressionMode.Compress);
        dt.WriteXml(zipStream, XmlWriteMode.WriteSchema);
        zipStream.Close();
        byte[] data = memStream.ToArray();
        memStream.Close();

        return data;
    }

    public static DataSet FetchDataSetRealTimeVoucherHeader(DateTime modifiedDate, int noOfRecords, string outletName)
    {
        var dt = new DataTable();
        //get VoucherHeader
        var ds = new DataSet();
        string sql = @"
                        DECLARE @EndDate DATETIME;
                        DECLARE @Row INT;
                        DECLARE @OutletName VARCHAR(50);

                        SET @EndDate = '{0}';
                        SET @Row = {1};
                        SET @OutletName = '{2}';

                        SELECT  VoucherHeaderID, 
                                VoucherHeaderName, 
                                Amount, 
                                ValidFrom, 
                                ValidTo, 
                                IssuedQuantity,
                                SoldQuantity,
                                RedeemedQuantity,
                                CanceledQuantity,
                                RedeemedQuantityWithoutVoucherNo,
                                Outlet,
                                VoucherPrefix,
                                StartNumber,
                                EndNumber,
                                VoucherSuffix,
                                NumOfDigit,
                                CreatedOn, 
                                CreatedBy, 
                                ModifiedOn, 
                                ModifiedBy, 
                                Deleted = CASE 
                                            WHEN ISNULL(Deleted, 0) = 1 THEN 1
                                            WHEN (Outlet = 'ALL' OR ISNULL(CHARINDEX(@OutletName, NULLIF(Outlet, ''), 0), 1) > 0) THEN 0 
                                            ELSE 1 
                                          END, 
                                userfld1, userfld2, userfld3, userfld4, userfld5, userfld6, userfld7, userfld8, userfld9, userfld10, 
                                userflag1, userflag2, userflag3, userflag4, userflag5, 
                                userfloat1, userfloat2, userfloat3, userfloat4, userfloat5, 
                                userint1, userint2, userint3, userint4, userint5
                        FROM	VoucherHeader S
		                        INNER JOIN (
		                                SELECT	 VoucherHeaderID TheKey
				                                ,ModifiedOn TheModifiedOn
				                                ,DENSE_RANK() OVER(ORDER BY ModifiedOn) TheNo
		                                FROM	VoucherHeader 
		                                WHERE	(ModifiedOn > @EndDate)
		                            ) TAB ON TAB.TheKey = VoucherHeaderID
                        WHERE	TAB.TheNo <= @Row  
                        ORDER BY TAB.TheNo, TAB.TheModifiedOn, TAB.TheKey
                      ";

        sql = string.Format(sql, modifiedDate.ToString("yyyy-MM-dd HH:mm:ss.fff")
                               , noOfRecords
                               , outletName);
        dt.Load(DataService.GetReader(new QueryCommand(sql)));
        dt.TableName = "VoucherHeader";
        ds.Tables.Add(dt);
        return ds;
    }

    public static byte[] FetchDataSetRealTimeCompressedVouchers(DateTime modifiedDate, int noOfRecords, string outletName)
    {
        var dt = FetchDataSetRealTimeVouchers(modifiedDate, noOfRecords, outletName);

        MemoryStream memStream = new MemoryStream();
        GZipStream zipStream = new GZipStream(memStream, CompressionMode.Compress);
        dt.WriteXml(zipStream, XmlWriteMode.WriteSchema);
        zipStream.Close();
        byte[] data = memStream.ToArray();
        memStream.Close();

        return data;
    }

    public static DataSet FetchDataSetRealTimeVouchers(DateTime modifiedDate, int noOfRecords, string outletName)
    {
        var dt = new DataTable();
        //get Vouchers
        var ds = new DataSet();
        string sql = @"
                        DECLARE @EndDate DATETIME;
                        DECLARE @Row INT;
                        DECLARE @OutletName VARCHAR(50);

                        SET @EndDate = '{0}';
                        SET @Row = {1};
                        SET @OutletName = '{2}';

                        SELECT  VoucherID, 
                                VoucherNo, 
                                Amount, 
                                Remark, 
                                VoucherStatusId, 
                                DateIssued, 
                                DateSold, 
                                DateRedeemed, 
                                ExpiryDate, 
                                CreatedOn, 
                                CreatedBy, 
                                ModifiedOn, 
                                ModifiedBy, 
                                Deleted = CASE 
                                            WHEN ISNULL(Deleted, 0) = 1 THEN 1
                                            WHEN (Outlet = 'ALL' OR ISNULL(CHARINDEX(@OutletName, NULLIF(Outlet, ''), 0), 1) > 0) THEN 0 
                                            ELSE 1 
                                          END, 
                                RedeemAmount, 
                                DateCanceled, 
                                VoucherHeaderID, 
                                Outlet
                        FROM	Vouchers S
		                        INNER JOIN (
		                                SELECT	 VoucherID TheKey
				                                ,ModifiedOn TheModifiedOn
				                                ,DENSE_RANK() OVER(ORDER BY ModifiedOn) TheNo
		                                FROM	Vouchers 
		                                WHERE	(ModifiedOn > @EndDate)
		                            ) TAB ON TAB.TheKey = VoucherID
                        WHERE	TAB.TheNo <= @Row  
                        ORDER BY TAB.TheNo, TAB.TheModifiedOn, TAB.TheKey
                      ";

        sql = string.Format(sql, modifiedDate.ToString("yyyy-MM-dd HH:mm:ss.fff")
                               , noOfRecords
                               , outletName);
        dt.Load(DataService.GetReader(new QueryCommand(sql)));
        dt.TableName = "Vouchers";
        ds.Tables.Add(dt);
        return ds;
    }

    #endregion

    #endregion

    #region Saved Inventory File
    public static string SaveInventoryFile(
        DataSet invHdr, string movementType, string remark, bool autosave)
    {

        var status = "";
        try
        {
            SavedItemController s = new SavedItemController();
            string path = HostingEnvironment.ApplicationPhysicalPath + "SavedFile\\";
            path = path.Replace("file:\\", "");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            InventoryController invCtrl = new InventoryController();
            //invCtrl.InvHdr = inventoryHdr;
            if (invHdr.Tables.Count > 0)
            {
                invCtrl.LoadFromDataTable(invHdr.Tables[0], invHdr.Tables[1]);
            }

            if (invCtrl.getUniqueID() == Guid.Empty)
            {
                invCtrl.createNewGUID();
            }
            Logger.writeLog("Save Object");
            if (s.SaveObject
                (invCtrl, invCtrl.getUniqueID().ToString(), movementType,
                 path, remark, autosave, out status))
            {
                status = "Success";
                //MessageBox.Show();
            }
            else
            {
                status = "Error. " + status;
                Logger.writeLog(status);
            }

            return status;
        }
        catch (Exception ex)
        {
            status = "Error. " + ex.Message;
            return status;
        }
    }


    public static byte[] GetInventorySavedFile(string ar)
    {
        DataSet ds = new DataSet();
        try
        {
            string[] theList = ar.Split(',');
            SavedFileCollection col = new SavedFileCollection();
            col.Where(SavedFile.Columns.Deleted, false);
            col.Where(SavedFile.Columns.MovementType, SubSonic.Comparison.In, theList);
            col.OrderByDesc(SavedFile.Columns.SavedDate);
            col.Load();

            DataTable dt = col.ToDataTable();

            ds.Tables.Add(dt);
            return SyncClientController.CompressDataSetToByteArray(ds);
        }
        catch (Exception ex)
        {
            return SyncClientController.CompressDataSetToByteArray(ds);
        }
    }

    public static byte[] LoadInventoryFromFile(string saveName)
    {
        DataSet ds = new DataSet();
        try
        {
            SavedItemController s = new SavedItemController();
            string path = HostingEnvironment.ApplicationPhysicalPath + "SavedFile\\";
            path = path.Replace("file:\\", "");
            Object tmp = s.LoadObject(saveName, path);

            ds.Tables.Add(((InventoryController)tmp).InvHdrToDataTable());
            ds.Tables.Add(((InventoryController)tmp).InvDetToDataTable());

            //s.removeFile(saveName);

            return SyncClientController.CompressDataSetToByteArray(ds);
        }
        catch (Exception ex) { Logger.writeLog(ex.Message); return SyncClientController.CompressDataSetToByteArray(ds); }
    }

    public static bool removeSavedFile(string saveName)
    {
        SavedItemController s = new SavedItemController();
        return s.removeFile(saveName);
    }
    #endregion
}
