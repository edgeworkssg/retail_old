using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using PowerPOS.Container;
using PowerPOS;
using SubSonic;
using System.ComponentModel;
using System.Collections;
using PowerPOSLib.Reports;
using System.Drawing;

namespace PowerPOS
{
    public partial class ReportController
    {
        public static DataTable FetchTenantHistory(DateTime startDate, DateTime endDate, string outletName, int posID, string accessSource, string search, string sortBy, string sortDir)
        {
            DataTable dt = new DataTable();

            try
            {
                string sortCriteria = "";
                if (string.IsNullOrEmpty(sortDir))
                    sortDir = "ASC";
                if (string.IsNullOrEmpty(sortBy))
                    sortCriteria = string.Format("AccessDate DESC, OutletName, RetailerLevel, ShopNo");
                else
                    sortCriteria = string.Format("{0} {1}", sortBy, sortDir);


                string sql =  @"DECLARE @StartDate AS DATETIME;
                                DECLARE @EndDate AS DATETIME;
                                DECLARE @OutletName AS NVARCHAR(200);
                                DECLARE @POSID AS INT;
                                DECLARE @AccessSource AS NVARCHAR(200);
                                DECLARE @Search AS NVARCHAR(200);

                                SET @StartDate = '{0}';
                                SET @EndDate = '{1}';
                                SET @OutletName = N'{2}';
                                SET @POSID = {3};
                                SET @AccessSource = N'{4}';
                                SET @Search = N'%{5}%';

                                SELECT   MAX(ISNULL(POS.OutletCode,'')) OutletCode
		                                ,MAX(ISNULL(POS.OutletName,'')) OutletName	 	
		                                ,MAX(ISNULL(POS.PointOfSaleID,0)) PointOfSaleID
		                                ,MAX(ISNULL(POS.TenantMachineID,'')) TenantMachineID		
		                                ,MAX(ISNULL(POS.PointOfSaleName,'')) PointOfSaleName		
		                                ,MAX(ISNULL(POS.RetailerLevel,'')) RetailerLevel
		                                ,MAX(ISNULL(POS.ShopNo,'')) ShopNo
		                                ,AL.AccessDate
		                                ,AL.LoginName
		                                ,AL.AccessSource		
		                                ,AL.AccessType
		                                ,AL.Remarks
                                FROM	AccessLog AL
		                                LEFT JOIN (
			                                SELECT  POS.PointOfSaleID
					                                ,POS.TenantMachineID
					                                ,OU.OutletName OutletName
					                                ,OU.userfld1 OutletCode
					                                ,POS.PointOfSaleName
					                                ,POS.RetailerLevel
					                                ,POS.ShopNo
			                                FROM	PointOfSale POS
					                                LEFT JOIN Outlet OU ON OU.OutletName = POS.OutletName
		                                ) POS ON (POS.PointOfSaleID = AL.PointOfSaleID OR POS.TenantMachineID = AL.userfld1)
                                WHERE	ISNULL(AL.Remarks,'') = 'Tenant History'
		                                AND CAST(AL.AccessDate AS DATE) BETWEEN CAST(@StartDate AS DATE) AND CAST(@EndDate AS DATE)
		                                AND (POS.OutletName = @OutletName OR @OutletName = 'ALL')
		                                AND (POS.PointOfSaleID = @POSID OR @POSID = 0)
		                                AND (AL.AccessSource = @AccessSource OR @AccessSource = 'ALL')
		                                AND (AL.AccessType LIKE @Search)
                                GROUP BY AL.AccessDate
		                                ,AL.LoginName
		                                ,AL.AccessSource		
		                                ,AL.AccessType
		                                ,AL.Remarks
                                ORDER BY {6}";
                sql = string.Format(sql, startDate.ToString("yyyy-MM-dd")
                                       , endDate.ToString("yyyy-MM-dd")
                                       , outletName
                                       , posID
                                       , accessSource
                                       , search
                                       , sortCriteria);
                dt.Load(DataService.GetReader(new QueryCommand(sql)));
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }

            return dt;
        }


        public static DataTable FetchDailySalesReport
        (
             DateTime startdate,
             DateTime enddate,
             int minuteSpan,
             string OutletName,
             int POSID,
             bool showAmountBeforeGST,
             string SortColumn,
             string SortDir
        )
        {
            if (OutletName == "") { OutletName = "ALL"; }

            DataTable dt = new DataTable();

            try
            {
                string sqlString = @"DECLARE @StartDate DATETIME; 
                                DECLARE @EndDate DATETIME; 
                                DECLARE @POSID INT; 
                                DECLARE @OutletName nvarchar(MAX); 
                                DECLARE @UseMallIntegration AS VARCHAR(20);
                                DECLARE @ShowAmountBeforeGSt BIT;

                                SET @StartDate = '{0}'; 
                                SET @EndDate = '{1}'; 
                                SET @POSID = {2}; 
                                SET @OutletName = N'{3}'; 
                                SET @UseMallIntegration = (SELECT TOP 1 AppSettingValue FROM AppSetting WHERE AppSettingKey = 'MallIntegration_UseMallManagement')
                                SET @ShowAmountBeforeGSt = {4};                                

                                SELECT	 CAST(TheDate.SalesDate AS DATE) SalesDate 
                                        ,CASE WHEN @POSID = 0 THEN 'ALL' ELSE TheDate.ShopLevel END ShopLevel
                                        ,CASE WHEN @POSID = 0 THEN 'ALL' ELSE TheDate.ShopNo END ShopNo
                                        ,CASE WHEN @POSID = 0 THEN 'ALL' ELSE TheDate.PointOfSaleName END PointOfSaleName    
                                        ,SUM(OH.TotalSales) TotalSales
                                        ,SUM(OH.TotalTrans) TotalTrans      
                                        ,COUNT(DISTINCT CASE WHEN MembershipNo = 'WALK-IN' THEN OH.OrderHdrID ELSE OH.MembershipNo END) AS TotalCustomer  
                                        ,CASE WHEN @POSID = 0 THEN 'ALL' ELSE TheDate.ShopArea END ShopArea
                                        ,CASE WHEN @OutletName = 'ALL' THEN 'ALL' else TheDate.OutletName END OutletName        
                                        ,CASE WHEN @POSID = 0 THEN 'ALL' ELSE TheDate.Attribute1 END Attribute1
                                        ,CASE WHEN @POSID = 0 THEN 'ALL' ELSE TheDate.Attribute2 END Attribute2
                                FROM	( 
                                    SELECT   DT.Date SalesDate
                                            ,POS.PointOfSaleID
                                            ,POS.PointOfSaleName
                                            ,POS.OutletName
                                            ,POS.BusinessStartDate
                                            ,POS.BusinessEndDate
                                            ,POS.ShopLevel
                                            ,POS.ShopNo
                                            ,POS.ShopArea
                                            ,POS.Attribute1
                                            ,POS.Attribute2
                                    FROM	FnGetEveryDateBetween(@StartDate, @EndDate) DT
                                            CROSS JOIN (
                                                SELECT  POS.PointOfSaleID
                                                        ,POS.PointOfSaleName
                                                        ,OU.OutletName
                                                        ,POS.TenantMachineID
                                                        ,POS.BusinessStartDate
                                                        ,POS.BusinessEndDate
                                                        ,POS.RetailerLevel ShopLevel
                                                        ,POS.ShopNo
                                                        ,ISNULL(RL.userfld2,'') ShopArea
                                                        ,ISNULL(RL.userfld3,'') Attribute1
                                                        ,ISNULL(RL.userfld4,'') Attribute2                                              
                                                FROM	PointOfSale POS
                                                        LEFT JOIN Outlet OU ON OU.OutletName = POS.OutletName
                                                        LEFT JOIN RetailerLevel RL ON ISNULL(RL.userfld1,'') = POS.OutletName
													                                  AND ISNULL(RL.ShopLevel,'') = ISNULL(POS.RetailerLevel,'')
													                                  AND ISNULL(RL.ShopNo,'') = ISNULL(POS.ShopNo,'')
                                                WHERE	ISNULL(POS.Deleted,0) = 0
                                                        AND ISNULL(OU.Deleted,0) = 0
                                                        AND (POS.PointOfSaleID = @POSID OR @POSID = 0 OR @POSID = -1)
                                                        AND (OU.OutletName = @OutletName OR @OutletName IN ('ALL', 'ALL - BreakDown'))
                                                        AND (POS.BusinessStartDate IS NULL OR POS.BusinessEndDate IS NULL 
					                                        OR (CAST(@StartDate AS DATE) BETWEEN CAST(POS.BusinessStartDate AS DATE) AND CAST(POS.BusinessEndDate AS DATE)                                                               
					                                        OR CAST(@EndDate AS DATE) BETWEEN CAST(POS.BusinessStartDate AS DATE) AND CAST(POS.BusinessEndDate AS DATE)                                                               
					                                        OR CAST(POS.BusinessStartDate AS DATE) BETWEEN CAST(@StartDate AS DATE) AND CAST(@EndDate AS DATE)                                                               
					                                        OR CAST(POS.BusinessEndDate AS DATE) BETWEEN CAST(@StartDate AS DATE) AND CAST(@EndDate AS DATE))) 
                                            ) POS
                                    ) TheDate LEFT JOIN (
                                        SELECT   OH.OrderHdrID
                                                ,OH.PointOfSaleID
                                                ,OH.OrderDate
                                                ,OH.MembershipNo
                                                ,CASE WHEN ISNULL(@UseMallIntegration,'False') = 'True' THEN ISNULL(OH.userint1,0) ELSE 1 END TotalTrans        
                                                ,OD.TotalSales
                                        FROM	OrderHdr OH 
                                                LEFT JOIN (
                                                    SELECT   OH.OrderHdrID
                                                            ,SUM(CASE WHEN @ShowAmountBeforeGSt = 1 THEN (OD.Amount-OD.GSTAmount) ELSE OD.Amount END) TotalSales  
                                                    FROM	OrderHdr OH
                                                            LEFT JOIN OrderDet OD ON OD.OrderHdrID = OH.OrderHdrID
                                                            LEFT JOIN PointOfSale POS ON POS.PointOfSaleID = OH.PointOfSaleID
                                                            LEFT JOIN Outlet OU ON OU.OutletName = POS.OutletName
                                                    WHERE	OH.IsVoided = 0 AND OD.IsVoided = 0
                                                            AND CAST(OH.OrderDate AS DATE) >= CAST(@StartDate AS DATE)
                                                            AND CAST(OH.OrderDate AS DATE) <= CAST(@EndDate AS DATE)
                                                            AND (OH.PointOfSaleID = @POSID OR @POSID = 0 OR @POSID = -1)
                                                            AND (OU.OutletName = @OutletName OR @OutletName = 'ALL' OR @OutletName = 'ALL - BreakDown')
                                                    GROUP BY OH.OrderHdrID        						
                                                ) OD ON OD.OrderHdrID = OH.OrderHdrID
                                        WHERE	OH.IsVoided = 0
                                                AND CAST(OH.OrderDate AS DATE) >= CAST(@StartDate AS DATE)
                                                AND CAST(OH.OrderDate AS DATE) <= CAST(@EndDate AS DATE)		
                                    ) OH ON OH.PointOfSaleID = TheDate.PointOfSaleID AND CAST(TheDate.SalesDate AS DATE) = CAST(OH.OrderDate AS DATE)
                                WHERE	(TheDate.PointOfSaleID = @POSID OR @POSID = 0 OR @POSID = -1)
                                        AND (TheDate.OutletName = @OutletName OR @OutletName = 'ALL' OR @OutletName = 'ALL - BreakDown')
                                        AND (OH.TotalTrans IS NOT NULL OR @UseMallIntegration = 'True')
                                GROUP BY  CAST(TheDate.SalesDate AS DATE) 
                                        ,CASE WHEN @POSID = 0 THEN 'ALL' ELSE TheDate.ShopLevel END
                                        ,CASE WHEN @POSID = 0 THEN 'ALL' ELSE TheDate.ShopNo END
                                        ,CASE WHEN @POSID = 0 THEN 'ALL' else TheDate.PointOfSaleName END    
		                                ,CASE WHEN @POSID = 0 THEN 'ALL' ELSE TheDate.ShopArea END
                                        ,CASE WHEN @OutletName = 'ALL' THEN 'ALL' else TheDate.OutletName END        
                                        ,CASE WHEN @POSID = 0 THEN 'ALL' ELSE TheDate.Attribute1 END
                                        ,CASE WHEN @POSID = 0 THEN 'ALL' ELSE TheDate.Attribute2 END
                                ORDER BY     SalesDate
			                                ,ShopLevel
			                                ,ShopNo
			                                ,OutletName
			                                ,PointOfSaleName
			                                ,TotalSales ";
                sqlString = string.Format(sqlString, startdate.ToString("yyyy-MM-dd HH:mm:ss")
                                                   , enddate.ToString("yyyy-MM-dd HH:mm:ss")
                                                   , POSID
                                                   , OutletName
                                                   , showAmountBeforeGST ? 1 : 0);
                Logger.writeLog(sqlString);
                dt = DataService.GetDataSet(new QueryCommand(sqlString)).Tables[0];
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }

            return dt;
        }

        public static DataTable FetchDailySalesReportVSManualSubmission
        (
             DateTime startdate,
             DateTime enddate,
             int minuteSpan,
             string OutletName,
             int POSID,
             decimal discrepancy,
             string SortColumn,
             string SortDir
        )
        {
            if (OutletName == "") { OutletName = "ALL"; }

            DataTable dt = new DataTable();

            try
            {
                string sqlString = @"DECLARE @StartDate DATETIME; 
                                DECLARE @EndDate DATETIME; 
                                DECLARE @POSID INT; 
                                DECLARE @OutletName nvarchar(MAX); 
                                DECLARE @Discrepancy MONEY;

                                SET @StartDate = '{0}'; 
                                SET @EndDate = '{1}'; 
                                SET @POSID = {2}; 
                                SET @OutletName = N'{3}'; 
                                SET @Discrepancy = {4};

                                ;WITH CTE AS (
                                SELECT   POS.SalesDate
                                        ,POS.PointOfSaleID
                                        ,POS.PointOfSaleName
                                        ,POS.OutletName
                                        ,ISNULL(SALES.TotalSales,0) TotalSales
                                        ,ISNULL(MANUALSUMBIT.TotalSales,0) ManualSubmission
                                        ,(ISNULL(MANUALSUMBIT.TotalSales,0) - ISNULL(SALES.TotalSales,0)) Diff
                                FROM	(
                                            SELECT  DT.Date SalesDate
                                                    ,POS.PointOfSaleID
                                                    ,POS.PointOfSaleName
                                                    ,POS.OutletName
								                    ,POS.BusinessStartDate
								                    ,POS.BusinessEndDate
                                            FROM	FnGetEveryDateBetween(@StartDate, @EndDate) DT
                                                    CROSS JOIN (
                                                        SELECT  POS.PointOfSaleID
                                                                ,POS.PointOfSaleName
                                                                ,OU.OutletName
                                                                ,POS.RetailerLevel
                                                                ,POS.ShopNo
                                                                ,POS.TenantMachineID
								                                ,POS.BusinessStartDate
								                                ,POS.BusinessEndDate
                                                        FROM	PointOfSale POS
                                                                LEFT JOIN Outlet OU ON OU.OutletName = POS.OutletName
                                                        WHERE	ISNULL(POS.Deleted,0) = 0
                                                                AND ISNULL(OU.Deleted,0) = 0
                                                                AND (POS.PointOfSaleID = @POSID OR @POSID = 0)
                                                                AND (OU.OutletName = @OutletName OR @OutletName = 'ALL')
                                                                AND (CAST(@StartDate AS DATE) BETWEEN CAST(POS.BusinessStartDate AS DATE) AND CAST(POS.BusinessEndDate AS DATE)
                                                                     OR CAST(@EndDate AS DATE) BETWEEN CAST(POS.BusinessStartDate AS DATE) AND CAST(POS.BusinessEndDate AS DATE)
                                                                     OR CAST(POS.BusinessStartDate AS DATE) BETWEEN CAST(@StartDate AS DATE) AND CAST(@EndDate AS DATE)
                                                                     OR CAST(POS.BusinessEndDate AS DATE) BETWEEN CAST(@StartDate AS DATE) AND CAST(@EndDate AS DATE))
                                                    ) POS
                                            WHERE CAST(DT.Date AS DATE) BETWEEN CAST(POS.BusinessStartDate AS DATE) AND CAST(POS.BusinessEndDate AS DATE)
                                        ) POS
                                        LEFT JOIN (
                                            SELECT	 CAST(OH.OrderDate AS DATE) SalesDate 
                                                    ,POS.PointOfSaleID
                                                    ,POS.PointOfSaleName
                                                    ,OU.OutletName
                                                    ,SUM(OD.Amount) TotalSales   
                                            FROM	OrderHdr OH
                                                    LEFT JOIN OrderDet OD ON OD.OrderHdrID = OH.OrderHdrID
                                                    LEFT JOIN PointOfSale POS ON POS.PointOfSaleID = OH.PointOfSaleID
                                                    LEFT JOIN Outlet OU ON OU.OutletName = POS.OutletName
                                            WHERE	OH.IsVoided = 0 AND OD.IsVoided = 0
                                                    AND CAST(OH.OrderDate AS DATE) >= CAST(@StartDate AS DATE)
                                                    AND CAST(OH.OrderDate AS DATE) <= CAST(@EndDate AS DATE)
                                                    AND (OH.PointOfSaleID = @POSID OR @POSID = 0)
                                                    AND (OU.OutletName = @OutletName OR @OutletName = 'ALL')
                                            GROUP BY  CAST(OH.OrderDate AS DATE)
                                                     ,POS.PointOfSaleID
                                                     ,POS.PointOfSaleName     
                                                     ,OU.OutletName 		
                                        ) SALES ON CAST(SALES.SalesDate AS DATE) = CAST(POS.SalesDate AS DATE)
                                                    AND SALES.PointOfSaleID = POS.PointOfSaleID
                                        LEFT JOIN (
                                            SELECT  CAST(MSU.Date AS DATE) SalesDate 
                                                    ,POS.PointOfSaleID
                                                    ,POS.PointOfSaleName      
                                                    ,OU.OutletName  
                                                    ,SUM(MSU.TotalSalesAfterTax) TotalSales
                                            FROM	ManualSalesUpdate MSU
                                                    LEFT JOIN PointOfSale POS ON POS.PointOfSaleID = MSU.PointOfSaleID
                                                    LEFT JOIN Outlet OU ON OU.OutletName = POS.OutletName
                                            WHERE	(POS.PointOfSaleID = @POSID OR @POSID = 0 )
                                                    AND (OU.OutletName = @OutletName OR @OutletName = 'ALL')                    
                                            GROUP BY  CAST(MSU.Date AS DATE)
                                                     ,POS.PointOfSaleID
                                                     ,POS.PointOfSaleName     
                                                     ,OU.OutletName 			
                                        ) MANUALSUMBIT ON CAST(MANUALSUMBIT.SalesDate AS DATE) = CAST(POS.SalesDate AS DATE)
                                                          AND MANUALSUMBIT.PointOfSaleID = POS.PointOfSaleID
                                )

                                SELECT   CTE.SalesDate
		                                ,CTE.PointOfSaleID
		                                ,CTE.PointOfSaleName
		                                ,CTE.OutletName
		                                ,CTE.TotalSales
		                                ,CTE.ManualSubmission
		                                ,CTE.Diff
		                                ,CASE WHEN CTE.ManualSubmission = 0 AND CTE.TotalSales <> 0 THEN 100
                                              WHEN CTE.TotalSales = 0 THEN 0
			                                  ELSE (ABS(CTE.ManualSubmission - CTE.TotalSales) / CTE.ManualSubmission) * 100
			                                  END Discrepancy
                                FROM	 CTE         
                                WHERE	 (CASE WHEN CTE.ManualSubmission = 0 AND CTE.TotalSales <> 0 THEN 100
                                              WHEN CTE.TotalSales = 0 THEN 0
			                                  ELSE (ABS(CTE.ManualSubmission - CTE.TotalSales) / CTE.ManualSubmission) * 100
			                                  END) >= @Discrepancy
                                ORDER BY SalesDate, OutletName, PointOfSaleName ";
                sqlString = string.Format(sqlString, startdate.ToString("yyyy-MM-dd")
                                                   , enddate.ToString("yyyy-MM-dd")
                                                   , POSID
                                                   , OutletName
                                                   , discrepancy);
                dt = DataService.GetDataSet(new QueryCommand(sqlString)).Tables[0];
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }

            return dt;
        }

        public static DataTable FetchMissingDailySales
        (
             DateTime startdate,
             DateTime enddate,
             string OutletName,
             int POSID,
             string SortColumn,
             string SortDir
        )
        {
            if (OutletName == "") { OutletName = "ALL"; }

            DataTable dt = new DataTable();

            try
            {
                string sqlString = @"DECLARE @StartDate DATETIME; 
                                DECLARE @EndDate DATETIME; 
                                DECLARE @POSID INT; 
                                DECLARE @OutletName nvarchar(MAX); 

                                SET @StartDate = '{0}'; 
                                SET @EndDate = '{1}'; 
                                SET @POSID = {2}; 
                                SET @OutletName = N'{3}'; 

                                SELECT   POS.SalesDate
                                        ,POS.OutletName
                                        ,POS.PointOfSaleID
                                        ,POS.PointOfSaleName
                                        ,POS.RetailerLevel
                                        ,POS.ShopNo
                                        ,POS.TenantMachineID
                                        ,POS.BusinessStartDate
                                        ,POS.BusinessEndDate
                                        ,SALES.TotalSales
                                FROM	(
			                                SELECT  DT.Date SalesDate
					                                ,POS.*
			                                FROM	FnGetEveryDateBetween(@StartDate, @EndDate) DT
					                                CROSS JOIN (
						                                SELECT   POS.PointOfSaleID
								                                ,POS.PointOfSaleName
								                                ,OU.OutletName
								                                ,POS.RetailerLevel
								                                ,POS.ShopNo
								                                ,POS.TenantMachineID
								                                ,POS.BusinessStartDate
								                                ,POS.BusinessEndDate
						                                FROM	PointOfSale POS
								                                LEFT JOIN Outlet OU ON OU.OutletName = POS.OutletName
						                                WHERE	ISNULL(POS.Deleted,0) = 0
								                                AND ISNULL(OU.Deleted,0) = 0
								                                AND (POS.PointOfSaleID = @POSID OR @POSID = 0)
								                                AND (OU.OutletName = @OutletName OR @OutletName = 'ALL')
                                                                AND (CAST(@StartDate AS DATE) BETWEEN CAST(POS.BusinessStartDate AS DATE) AND CAST(POS.BusinessEndDate AS DATE)
	                                                                 OR CAST(@EndDate AS DATE) BETWEEN CAST(POS.BusinessStartDate AS DATE) AND CAST(POS.BusinessEndDate AS DATE)
	                                                                 OR CAST(POS.BusinessStartDate AS DATE) BETWEEN CAST(@StartDate AS DATE) AND CAST(@EndDate AS DATE)
	                                                                 OR CAST(POS.BusinessEndDate AS DATE) BETWEEN CAST(@StartDate AS DATE) AND CAST(@EndDate AS DATE))
                                                    ) POS
                                                    WHERE CAST(DT.Date AS DATE) BETWEEN CAST(POS.BusinessStartDate AS DATE) AND CAST(POS.BusinessEndDate AS DATE)
                                                            AND CAST(DT.Date AS DATE) < CAST(GETDATE() AS DATE)
                                        ) POS
                                        LEFT JOIN (
                                            SELECT	 CAST(OH.OrderDate AS DATE) SalesDate 
                                                    ,POS.PointOfSaleID
                                                    ,POS.PointOfSaleName 
                                                    ,OU.OutletName
                                                    ,SUM(OD.Amount) TotalSales   
                                            FROM	OrderHdr OH
                                                    LEFT JOIN OrderDet OD ON OD.OrderHdrID = OH.OrderHdrID
                                                    LEFT JOIN PointOfSale POS ON POS.PointOfSaleID = OH.PointOfSaleID
                                                    LEFT JOIN Outlet OU ON OU.OutletName = POS.OutletName
                                            WHERE	OH.IsVoided = 0 AND OD.IsVoided = 0
                                                    AND CAST(OH.OrderDate AS DATE) >= CAST(@StartDate AS DATE)
                                                    AND CAST(OH.OrderDate AS DATE) <= CAST(@EndDate AS DATE)
                                                    AND (OH.PointOfSaleID = @POSID OR @POSID = 0)
                                                    AND (OU.OutletName = @OutletName OR @OutletName = 'ALL')
                                            GROUP BY  CAST(OH.OrderDate AS DATE)
                                                    ,POS.PointOfSaleID
                                                    ,POS.PointOfSaleName 
                                                    ,OU.OutletName		
                                        ) SALES ON CAST(SALES.SalesDate AS DATE) = CAST(POS.SalesDate AS DATE)
                                                    AND SALES.PointOfSaleID = POS.PointOfSaleID
                                WHERE	SALES.TotalSales IS NULL
                                ORDER BY SalesDate, OutletName, PointOfSaleName ";
                sqlString = string.Format(sqlString, startdate.ToString("yyyy-MM-dd")
                                                   , enddate.ToString("yyyy-MM-dd")
                                                   , POSID
                                                   , OutletName);
                Logger.writeLog(">>>> Missing Sales : " + sqlString);
                dt = DataService.GetDataSet(new QueryCommand(sqlString)).Tables[0];
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }

            return dt;
        }

        public static DataTable FetchOutletTransactionHourlyReport(DateTime startDate, DateTime endDate, int pointOfSaleID, string outletName, bool showBeforeGST)
        {
            DataTable dt = new DataTable();
            dt.TableName = "OutletTransactionHourlyReport";
            try
            {
                string sql = @"
DECLARE @StartDate AS DATETIME;
DECLARE @EndDate AS DATETIME;
DECLARE @PointOfSaleID AS INT;
DECLARE @Outlet AS NVARCHAR(MAX);
DECLARE @ShowBeforeGST AS BIT;

SET @StartDate = '{0}';
SET @EndDate = '{1}';
SET @PointOfSaleID= {2};
SET @Outlet = N'{3}';
SET @ShowBeforeGST = {4};

DECLARE @CursorDate AS DATETIME;
DECLARE @TempDate AS TABLE ( OrderDate DATE, OrderHour INT, Temp VARCHAR(20), OrderHour2 VARCHAR(50) );
DECLARE @TempResult AS TABLE (Temp VARCHAR(20), TotalAmount MONEY, PointOfSale NVARCHAR(200));

CREATE TABLE #FinalResult 
( 
     OrderDate DATETIME
    ,OrderHour2 NVARCHAR(100)
    ,OrderHour INT 
    ,TotalAmount MONEY
    ,PointOfSale NVARCHAR(500)
)

SET @CursorDate = @StartDate

WHILE  CAST(@CursorDate AS DATE) < CAST(@EndDate AS DATE) 
    BEGIN
    INSERT INTO @TempDate 
    SELECT   CAST(@CursorDate AS DATE), DATEPART(HOUR,@CursorDate)
            ,CAST(CAST(@CursorDate AS DATE) AS VARCHAR(20))+' '+CAST(DATEPART(HOUR,@CursorDate) AS VARCHAR(20))
            ,RIGHT('00' + CONVERT(varchar,datepart(hour,@CursorDate) % 12 ),2) + ' ' + CASE WHEN DATEPART(HOUR,@CursorDate) >= 12 THEN 'PM' ELSE 'AM' END + ' - ' +
             RIGHT('00' + CONVERT(varchar,(datepart(hour,@CursorDate)+1) % 12 ),2) + ' ' + CASE WHEN (DATEPART(HOUR,@CursorDate)+1) >= 12 AND (DATEPART(HOUR,@CursorDate)+1) < 24 THEN 'PM' ELSE 'AM' END
    SET @CursorDate = DATEADD(HOUR, 1, @CursorDate)
    PRINT @CursorDate
END

INSERT INTO @TempResult
SELECT   CAST(CAST(OH.OrderDate AS DATE) AS VARCHAR(20))+' '+CAST(DATEPART(HOUR,OH.OrderDate) AS VARCHAR(20))
        ,ISNULL(SUM(CASE WHEN @ShowBeforeGST = 1 THEN OH.NettAmount-OH.GSTAmount ELSE OH.NettAmount END),0) AS TotalAmount
        ,POS.PointOfSaleName PointOfSale 
FROM	OrderHdr OH 
        INNER JOIN PointOfSale POS ON POS.PointOfSaleID = OH.PointOfSaleID
WHERE	OH.IsVoided <> 1
        AND OH.OrderDate BETWEEN @StartDate AND @EndDate
        AND (POS.PointOfSaleID = @PointOfSaleID OR @PointOfSaleID = 0)
        AND (POS.OutletName = @Outlet OR @Outlet = 'ALL')
GROUP BY CAST(CAST(OH.OrderDate AS DATE) AS VARCHAR(20))+' '+CAST(DATEPART(HOUR,OH.OrderDate) AS VARCHAR(20))
        ,POS.PointOfSaleName         
HAVING SUM(OH.NettAmount) > 0

INSERT INTO #FinalResult
SELECT   OrderDate OrderDate
        ,OrderHour2
        ,TD.OrderHour
        ,TR.TotalAMount TotalAmount
        ,TR.PointOfSale PointOfSale
FROM	@TempDate TD
        INNER JOIN @TempResult TR ON TD.Temp = TR.Temp
ORDER BY TD.OrderDate DESC, TD.OrderHour DESC, TR.PointOfSale

INSERT INTO #FinalResult
SELECT   FR.OrderDate
        ,FR.OrderHour2
        ,FR.OrderHour
        ,SUM(FR.TotalAMount) TotalAmount 
        ,'Total' PointOfSale
FROM	#FinalResult FR
GROUP BY FR.OrderDate
        ,FR.OrderHour2
        ,FR.OrderHour   

DECLARE @cols AS NVARCHAR(MAX);
DECLARE @query AS NVARCHAR(MAX);

SET @cols = STUFF((SELECT DISTINCT ',' + QUOTENAME(FR.PointOfSaleName) 
            FROM PointOfSale FR             
            WHERE ISNULL(FR.Deleted,0) = 0
					AND (FR.PointOfSaleID = @PointOfSaleID OR @PointOfSaleID = 0)
					AND (FR.OutletName = @Outlet OR @Outlet = 'ALL')            
            FOR XML PATH(''), TYPE
            ).value('.', 'NVARCHAR(MAX)') 
        ,1,1,'')

SET @query ='SELECT OrderDate,OrderHour2 OrderHour,OrderHour OrderHour2, ' + @cols + ',[Total] FROM 
             (
                SELECT	 OrderDate
                        ,OrderHour2
                        ,OrderHour
                        ,TotalAmount
                        ,PointOfSale
                FROM	#FinalResult
             ) X
             PIVOT 
             (
                 SUM(TotalAmount)
                 FOR PointOfSale IN (' + @cols + ',[Total])
             ) P ORDER BY OrderDate, OrderHour2'

EXECUTE(@query)

DROP TABLE #FinalResult";
                sql = string.Format(sql, startDate.ToString("yyyy-MM-dd HH:mm:ss")
                                       , endDate.ToString("yyyy-MM-dd HH:mm:ss")
                                       , pointOfSaleID
                                       , outletName
                                       , showBeforeGST ? 1 : 0);
                dt.Load(DataService.GetReader(new QueryCommand(sql)));

                dt.Columns.Remove("OrderHour2");
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }

            return dt;
        }

        public static DataTable FetchTransactionHourlyReport(bool useStartDate, bool useEndDate, DateTime StartDate, DateTime EndDate, 
            int PointOfSaleID, string outletName, bool showBeforeGST, string SortColumn, string SortDir)
        {
            QueryCommand Cmd;

            string query = "";

            query = @"DECLARE @StartDate AS DATETIME
                        DECLARE @EndDate AS DATETIME
                        DECLARE @CursorDate AS DATETIME
                        DECLARE @TempDate AS TABLE ( OrderDate DATE, OrderHour INT, Temp VARCHAR(20), OrderHour2 VARCHAR(50) )
                        DECLARE @TempResult AS TABLE (Temp VARCHAR(20), TotalAmount MONEY, TotalTrans INT, Outlet NVARCHAR(200), PointOfSale NVARCHAR(200))
                        DECLARE @Outlet AS NVARCHAR(MAX);
                        DECLARE @PointOfSaleID AS INT;
                        DECLARE @UseMallIntegration AS VARCHAR(20);
                        DECLARE @ShowBeforeGST AS BIT;

                        SET @StartDate = '{0}'
                        SET @EndDate = '{1}'
                        SET @Outlet = N'{2}';
                        SET @PointOfSaleID = {3};
                        SET @UseMallIntegration = (SELECT TOP 1 AppSettingValue FROM AppSetting WHERE AppSettingKey = 'MallIntegration_UseMallManagement')
                        SET @CursorDate = @StartDate
                        SET @ShowBeforeGST = {4};

                        WHILE  CAST(@CursorDate AS DATE) <= CAST(@EndDate AS DATE)
                        BEGIN
                            INSERT INTO @TempDate 
                            SELECT CAST(@CursorDate AS DATE), DATEPART(HOUR,@CursorDate),
                                    CAST(CAST(@CursorDate AS DATE) AS VARCHAR(20))+' '+CAST(DATEPART(HOUR,@CursorDate) AS VARCHAR(20)),
                                    RIGHT('00' + CONVERT(varchar,datepart(hour,@CursorDate) % 12 ),2) + ' ' + CASE WHEN datepart(hour,@CursorDate) > =12 THEN 'PM' ELSE 'AM' END + ' - ' +
                                    RIGHT('00' + CONVERT(varchar,(datepart(hour,@CursorDate)+1) % 12 ),2) + ' ' + CASE WHEN (datepart(hour,@CursorDate)+1) > =12 THEN 'PM' ELSE 'AM' END
                            SET @CursorDate = DATEADD(HOUR, 1, @CursorDate)
                            PRINT @CursorDate
                        END

                        INSERT INTO @TempResult
                        SELECT   CAST(CAST(OH.OrderDate AS DATE) AS VARCHAR(20))+' '+CAST(DATEPART(HOUR,OH.OrderDate) AS VARCHAR(20))
                                ,ISNULL(SUM(CASE WHEN @ShowBeforeGST = 1 THEN OH.NettAmount-OH.GSTAmount ELSE OH.NettAmount END),0) AS TotalAmount
                                ,CASE WHEN ISNULL(@UseMallIntegration,'False') = 'True' THEN SUM(ISNULL(OH.userint1,0)) ELSE COUNT(*) END TotalTrans
                                ,CASE WHEN @Outlet = 'ALL' THEN 'ALL' ELSE POS.OutletName END Outlet
                                ,CASE WHEN @PointOfSaleID = 0 THEN 'ALL' ELSE POS.PointOfSaleName END PointOfSale 
                        FROM	OrderHdr OH 
		                        LEFT JOIN PointOfSale POS ON POS.PointOfSaleID = OH.PointOfSaleID
                        WHERE	OH.IsVoided <> 1
                                AND OH.OrderDate BETWEEN @StartDate AND @EndDate
                                AND (POS.PointOfSaleID = @PointOfSaleID OR @PointOfSaleID = 0 OR @PointOfSaleID = -1)
                                AND (POS.OutletName = @Outlet OR @Outlet = 'ALL' OR @Outlet = 'ALL - BreakDown')
                        GROUP BY CAST(CAST(OH.OrderDate AS DATE) AS VARCHAR(20))+' '+CAST(DATEPART(HOUR,OH.OrderDate) AS VARCHAR(20))
                                ,CASE WHEN @Outlet = 'ALL' THEN 'ALL' ELSE POS.OutletName END
                                ,CASE WHEN @PointOfSaleID = 0 THEN 'ALL' ELSE POS.PointOfSaleName END 
                                
                        SELECT   CAST(OrderDate AS VARCHAR(20)) OrderDate
                                ,OrderHour2 OrderHour
                                ,ISNULL(TR.TotalAMount,0) TotalAmount
                                ,ISNULL(TR.TotalTrans,0) TotalTrans
                                ,ISNULL(TR.Outlet,'') Outlet
                                ,ISNULL(TR.PointOfSale,'') PointOfSale
                        FROM	@TempDate TD
                                LEFT OUTER JOIN @TempResult TR ON TD.Temp = TR.Temp
                        WHERE   ISNULL(TR.TotalTrans,0) <> 0
                        ORDER BY TD.OrderDate, TD.OrderHour";
            query = string.Format(query, StartDate.ToString("yyyy-MM-dd HH:mm:ss")
                                       , EndDate.ToString("yyyy-MM-dd HH:mm:ss")
                                       , outletName
                                       , PointOfSaleID
                                       , showBeforeGST ? 1 : 0);

            Cmd = new QueryCommand(query);
            DataTable Result = new DataTable();
            Result.Load(SubSonic.DataService.GetReader(Cmd));

            return Result;
        }

        public static DataTable FetchItemCookReport(DateTime startDate, DateTime endDate,
            int inventoryLocationID, string outletName, int posID, string search, string documentNo, string status, string orderBy, string orderDir)
        {
            DataTable dt = new DataTable();

            try
            {
                if (string.IsNullOrEmpty(orderBy))
                    orderBy = "CookDate";

                if (string.IsNullOrEmpty(orderDir))
                    orderDir = "ASC";

                string sql = @"DECLARE @StartDate DATETIME;
                                DECLARE @EndDate DATETIME;
                                DECLARE @POSID INT;
                                DECLARE @OutletName NVARCHAR(200);
                                DECLARE @InventoryLocationID INT;
                                DECLARE @Search NVARCHAR(500);
                                DECLARE @Status VARCHAR(50);
                                DECLARE @DocumentNo VARCHAR(50);

                                SET @StartDate = '{0}';
                                SET @EndDate = '{1}';
                                SET @POSID = {2};
                                SET @OutletName = N'{3}'
                                SET @InventoryLocationID = {4};
                                SET @Search = N'{5}';
                                SET @Status = '{8}';
                                SET @DocumentNo = '{9}';

                                SELECT   IL.InventoryLocationID
		                                ,IL.InventoryLocationName
		                                ,ISNULL(OU.OutletName,'') OutletName
		                                ,ISNULL(POS.PointOfSaleID,'') PointOfSaleID
		                                ,ISNULL(POS.PointOfSaleName,'') PointOfSaleName
		                                ,ID.ItemDepartmentID
		                                ,ID.DepartmentName
		                                ,CTG.CategoryName
		                                ,I.ItemNo
		                                ,I.ItemName
		                                ,ICH.CookDate
		                                ,ICH.Quantity
                                        ,ICH.ItemCookHistoryID
                                        ,ISNULL(ICH.Userfld1,'') as DocumentNo
                                        ,ISNULL(ICH.Userfld2,'') as Status
                                        ,ISNULL(ICH.Userfloat1,0) as COG
                                FROM	ItemCookHistory ICH
		                                INNER JOIN Item I ON I.ItemNo = ICH.ItemNo
		                                LEFT JOIN Category CTG ON CTG.CategoryName = I.CategoryName
		                                LEFT JOIN ItemDepartment ID ON ID.ItemDepartmentID = CTG.itemdepartmentid
		                                LEFT JOIN PointOfSale POS ON POS.PointOfSaleID = ICH.PointOfSaleID
		                                LEFT JOIN Outlet OU ON OU.OutletName = POS.OutletName
		                                LEFT JOIN InventoryLocation IL ON IL.InventoryLocationID = ICH.InventoryLocationID
                                WHERE	CAST(ICH.CookDate AS DATE) BETWEEN CAST(@StartDate AS DATE) AND CAST(@EndDate AS DATE)
		                                AND (ISNULL(POS.PointOfSaleID,0) = @POSID OR @POSID = 0)
		                                AND (ISNULL(OU.OutletName,'') = @OutletName OR @OutletName = 'ALL')
		                                AND (IL.InventoryLocationID = @InventoryLocationID OR @InventoryLocationID = 0)
                                        AND (@Status = '' OR @Status = 'ALL' OR ISNULL(ICH.Userfld2,'') = @Status)
                                        AND (@DocumentNo = '' OR ISNULL(ICH.Userfld1,'') = @DocumentNo)
		                                AND (ISNULL(ID.DepartmentName,'') LIKE '%'+@Search+'%'
			                                 OR ISNULL(CTG.CategoryName,'') LIKE '%'+@Search+'%'
			                                 OR ISNULL(I.ItemNo,'') LIKE '%'+@Search+'%'
			                                 OR ISNULL(I.ItemName,'') LIKE '%'+@Search+'%')
                                ORDER BY {6} {7}";
                sql = string.Format(sql, startDate.ToString("yyyy-MM-dd")
                                       , endDate.ToString("yyyy-MM-dd")
                                       , posID
                                       , outletName
                                       , inventoryLocationID
                                       , search
                                       , orderBy
                                       , orderDir
                                       , status
                                       , documentNo);
                dt.Load(DataService.GetReader(new QueryCommand(sql)));
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }

            return dt;
        }


    }
}
