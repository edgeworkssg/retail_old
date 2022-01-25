IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UPDATE_OrderHdr_DataIndex]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UPDATE_OrderHdr_DataIndex]

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UPDATE_OrderHdr_DataIndex]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[UPDATE_OrderHdr_DataIndex]
	@OrderYear INT,
	@OrderMonth INT,
	@OrderDay INT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @ExistingKey AS BIGINT;

	SELECT TOP 1 @ExistingKey = CAST(AppSettingValue AS INT)  
	FROM AppSetting 
	WHERE AppSettingKey = ''DataIndex_OrderHdr''

	UPDATE	TAB
	SET		TAB.DataIndex = ISNULL(@ExistingKey,0) + TAB2.RowNo
	FROM	OrderHdr TAB WITH(NOLOCK)
			INNER JOIN (
			SELECT   OrderHdrID
					,ROW_NUMBER() OVER(ORDER BY ModifiedOn, OrderHdrID) RowNo
			FROM    OrderHdr WITH(NOLOCK)
			WHERE	DataIndex IS NULL
					AND YEAR(OrderDate) = @OrderYear
					AND MONTH(OrderDate) = @OrderMonth
					AND DAY(OrderDate) = @OrderDay
			) TAB2 ON TAB2.OrderHdrID = TAB.OrderHdrID
	WHERE	TAB.DataIndex IS NULL
			AND	YEAR(TAB.OrderDate) = @OrderYear
			AND MONTH(TAB.OrderDate) = @OrderMonth
			AND DAY(TAB.OrderDate) = @OrderDay

	UPDATE  AppSetting
	SET		AppSettingValue = CAST((SELECT MAX(DataIndex) FROM OrderHdr WITH(NOLOCK)) AS VARCHAR(50))
	WHERE	AppSettingKey = ''DataIndex_OrderHdr''

END
' 
 
END