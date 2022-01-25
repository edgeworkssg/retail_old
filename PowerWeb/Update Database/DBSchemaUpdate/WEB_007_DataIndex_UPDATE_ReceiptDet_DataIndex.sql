IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UPDATE_ReceiptDet_DataIndex]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UPDATE_ReceiptDet_DataIndex]

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UPDATE_ReceiptDet_DataIndex]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[UPDATE_ReceiptDet_DataIndex]
	@OrderYear INT,
	@OrderMonth INT,
	@OrderDay INT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @ExistingKey AS BIGINT;

	SELECT TOP 1 @ExistingKey = CAST(AppSettingValue AS INT)  
	FROM AppSetting 
	WHERE AppSettingKey = ''DataIndex_ReceiptDet''

	UPDATE	RD
	SET		RD.DataIndex = ISNULL(@ExistingKey,0) + TAB.RowNo
	FROM	OrderHdr OH WITH(NOLOCK)
			INNER JOIN ReceiptHdr RH WITH(NOLOCK) ON RH.OrderHdrID = OH.OrderHdrID
			INNER JOIN ReceiptDet RD WITH(NOLOCK) ON RD.ReceiptHdrID = RH.ReceiptHdrID
			INNER JOIN (
			SELECT   RD.ReceiptDetID
					,ROW_NUMBER() OVER(ORDER BY RD.ModifiedOn, RD.ReceiptDetID) RowNo
			FROM    OrderHdr OH WITH(NOLOCK)
					INNER JOIN ReceiptHdr RH WITH(NOLOCK) ON RH.OrderHdrID = OH.OrderHdrID
					INNER JOIN ReceiptDet RD WITH(NOLOCK) ON RD.ReceiptHdrID = RH.ReceiptHdrID
			WHERE	RD.DataIndex IS NULL
					AND YEAR(OH.OrderDate) = @OrderYear
					AND MONTH(OH.OrderDate) = @OrderMonth
					AND DAY(OH.OrderDate) = @OrderDay
			) TAB ON TAB.ReceiptDetID = RD.ReceiptDetID
	WHERE	RD.DataIndex IS NULL
			AND	YEAR(OH.OrderDate) = @OrderYear
			AND MONTH(OH.OrderDate) = @OrderMonth
			AND DAY(OH.OrderDate) = @OrderDay

	UPDATE  AppSetting
	SET		AppSettingValue = CAST((SELECT MAX(DataIndex) FROM ReceiptDet WITH(NOLOCK)) AS VARCHAR(50))
	WHERE	AppSettingKey = ''DataIndex_ReceiptDet''

END

' 
 
END