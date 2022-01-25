IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UPDATE_OrderDet_DataIndex]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UPDATE_OrderDet_DataIndex]

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UPDATE_OrderDet_DataIndex]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[UPDATE_OrderDet_DataIndex]
	@OrderYear INT,
	@OrderMonth INT,
	@OrderDay INT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @ExistingKey AS BIGINT;

	SELECT TOP 1 @ExistingKey = CAST(AppSettingValue AS INT)  
	FROM AppSetting 
	WHERE AppSettingKey = ''DataIndex_OrderDet''

	UPDATE	TAB
	SET		TAB.DataIndex = ISNULL(@ExistingKey,0) + TAB2.RowNo
	FROM	OrderDet TAB WITH(NOLOCK)
			INNER JOIN (
			SELECT   OrderDetID
					,ROW_NUMBER() OVER(ORDER BY ModifiedOn, OrderDetID) RowNo
			FROM    OrderDet WITH(NOLOCK)
			WHERE	DataIndex IS NULL
					AND YEAR(OrderDetDate) = @OrderYear
					AND MONTH(OrderDetDate) = @OrderMonth
					AND DAY(OrderDetDate) = @OrderDay
			) TAB2 ON TAB2.OrderDetID = TAB.OrderDetID
	WHERE	TAB.DataIndex IS NULL
			AND	YEAR(TAB.OrderDetDate) = @OrderYear
			AND MONTH(TAB.OrderDetDate) = @OrderMonth
			AND DAY(TAB.OrderDetDate) = @OrderDay

	UPDATE  AppSetting
	SET		AppSettingValue = CAST((SELECT MAX(DataIndex) FROM OrderDet WITH(NOLOCK)) AS VARCHAR(50))
	WHERE	AppSettingKey = ''DataIndex_OrderDet''

END
' 
 
END