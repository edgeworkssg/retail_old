IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FetchCashOutReport]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[FetchCashOutReport]

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FetchCashOutReport]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[FetchCashOutReport]
    @startdate datetime,
    @enddate datetime,
    @PointOfSaleName varchar(50),
    @OutletName varchar(50),
    @DeptID varchar(5)
AS
BEGIN
	SET @OutletName = LTRIM(RTRIM(@OutletName));

    IF (@OutletName = ''ALL'' AND @PointOfSaleName = ''ALL'') --See total amt for all outlets 
    BEGIN
        SELECT CashRecordingTypeName, 0 - SUM(ISNULL(amount,0)) as TotalAmount
        FROM ViewCashRecording
        WHERE  (CashRecordingTime BETWEEN @startdate AND @enddate)			   
            AND CashRecordingTypeName = ''CASH OUT''
            AND DepartmentID like ''%'' + @DeptID
        GROUP BY CashRecordingTypeName
    END
	ELSE IF (@PointOfSaleName = ''ALL'' and @OutletName != ''ALL'')  --See total amt for PointOfSales at one outlet
    BEGIN
        SELECT CashRecordingTypeName, 0 - SUM(ISNULL(amount,0)) as TotalAmount
        FROM ViewCashRecording
        WHERE  (CashRecordingTime BETWEEN @startdate AND @enddate)			   
            AND CashRecordingTypeName = ''CASH OUT''
            AND DepartmentID like ''%'' + @DeptID
            AND OutletName Like @OutletName
        GROUP BY CashRecordingTypeName
    END
	ELSE IF (@PointOfSaleName != ''ALL'' and @OutletName = ''ALL'')  	
    BEGIN
        SELECT CashRecordingTypeName, 0 - SUM(ISNULL(amount,0)) as TotalAmount
        FROM ViewCashRecording
        WHERE  (CashRecordingTime BETWEEN @startdate AND @enddate)			   
            AND CashRecordingTypeName = ''CASH OUT''
            AND DepartmentID like ''%'' + @DeptID
            AND PointOfSaleName Like @PointOfSaleName
        GROUP BY CashRecordingTypeName
    END
    ELSE
    BEGIN
        SELECT CashRecordingTypeName, 0 - SUM(ISNULL(amount,0)) as TotalAmount
        FROM ViewCashRecording
        WHERE  (CashRecordingTime BETWEEN @startdate AND @enddate)			   
            AND CashRecordingTypeName = ''CASH OUT''
            AND DepartmentID like ''%'' + @DeptID
            AND PointOfSaleName Like @PointOfSaleName
            AND OutletName Like @OutletName
        GROUP BY CashRecordingTypeName
    END
END
' 
END
