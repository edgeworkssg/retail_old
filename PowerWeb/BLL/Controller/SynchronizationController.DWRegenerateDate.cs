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

public partial class SynchronizationController
{
    public static QueryCommandCollection FetchRegenerateDate(string[][] dsOrder)
    {
        QueryCommandCollection qmc = new QueryCommandCollection();
        try
        {
            bool useDataWarehouse = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Report.UseDataWarehouse), false);
            if (useDataWarehouse)
            {
                var hdrColl = new OrderHdrCollection();
                hdrColl.Load(GetDataTableFromSyncData(dsOrder, new OrderHdrCollection().ToDataTable()));

                foreach (var oh in hdrColl)
                {
                    DateTime date = oh.OrderDate;
                    if (date.Date < DateTime.Now.Date)
                    {
                        var pos = new PointOfSale(PointOfSale.Columns.PointOfSaleID, oh.PointOfSaleID);
                        string outlet = pos.OutletName;

                        string sql = @"DECLARE @Date AS DATETIME;
                                DECLARE @OutletName AS NVARCHAR(500);

                                SET @Date = '{0}';
                                SET @OutletName = N'{1}';

                                IF((SELECT COUNT(*) FROM DW_RegenerateDate WHERE CAST(OrderDate AS DATE) = CAST(@Date AS DATE) AND OutletName =  @OutletName)<=0)
                                BEGIN 
	                                INSERT INTO DW_RegenerateDate (ID, OrderDate, OutletName, CreatedBy, CreatedOn, ModifiedBy, ModifiedOn, Deleted) 
	                                VALUES (NEWID(),CAST(@Date AS DATE),@OutletName,'SYSTEM',GETDATE(),'SYSTEM',GETDATE(),0) 
                                END";
                        sql = string.Format(sql, date.ToString("yyyy-MM-dd"), outlet);
                        QueryCommand cmd = new QueryCommand(sql);
                        qmc.Add(cmd);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
        }

        return qmc;
    }

    public static DataTable GetDataTableFromSyncData(string[][] data, DataTable dataStructure)
    {
        DataTable dt = new DataTable();
        try
        {

            dt = dataStructure.Clone();
            for (int i = 0; i < data.Length; i++)
            {
                var newRow = dt.NewRow();
                for (int j = 0; j < data[i].Length; j++)
                {
                    if (dt.Columns[j].DataType == typeof(decimal))
                        newRow[j] = (data[i][j]).GetDecimalValue();
                    else if (dt.Columns[j].DataType == typeof(DateTime))
                        newRow[j] = (data[i][j]).GetDateValueOfLongString();
                    else if (dt.Columns[j].DataType == typeof(int))
                        newRow[j] = (data[i][j]).GetIntValue();
                    else if (dt.Columns[j].DataType == typeof(bool))
                        newRow[j] = (data[i][j]).GetBoolValue(false);
                    else
                        newRow[j] = (data[i][j]);
                }
                dt.Rows.Add(newRow);
            }
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
        }

        return dt;
    }
}
