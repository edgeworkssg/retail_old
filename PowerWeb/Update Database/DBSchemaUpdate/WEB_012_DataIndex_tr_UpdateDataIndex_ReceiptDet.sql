IF NOT EXISTS (select * from sys.objects where type = 'TR' and name = 'tr_UpdateDataIndex_ReceiptDet') BEGIN
EXEC dbo.sp_executesql @statement = N'
	CREATE TRIGGER [dbo].[tr_UpdateDataIndex_ReceiptDet] 
		ON  [dbo].[ReceiptDet] 
		AFTER INSERT, UPDATE
	AS 
	BEGIN
		SET NOCOUNT ON;
		SET XACT_ABORT OFF;
		BEGIN TRY
			PRINT ''UPDATING DATA INDEX'';
			DECLARE @ReceiptDetIDTemp AS TABLE (ReceiptDetID VARCHAR(50));
			DECLARE @ReceiptDetID VARCHAR(50);

			INSERT INTO @ReceiptDetIDTemp
			SELECT ReceiptDetID FROM inserted ORDER BY ReceiptDetID

			SET @ReceiptDetID = (SELECT TOP 1 ReceiptDetID FROM @ReceiptDetIDTemp ORDER BY ReceiptDetID);
			WHILE @ReceiptDetID IS NOT NULL BEGIN
				DELETE @ReceiptDetIDTemp WHERE ReceiptDetID = @ReceiptDetID;

				DECLARE @DataIndexTab AS TABLE (DataIndex BIGINT);
				DECLARE @DataIndex BIGINT;

				INSERT INTO @DataIndexTab
				EXEC(''GetLatestDataIndex ''''ReceiptDet'''''')

				SELECT	TOP 1 @DataIndex = DataIndex 
				FROM	@DataIndexTab
				UPDATE ReceiptDet SET DataIndex = @DataIndex WHERE ReceiptDetID = @ReceiptDetID

				SET @ReceiptDetID = (SELECT TOP 1 ReceiptDetID FROM @ReceiptDetIDTemp ORDER BY ReceiptDetID);
			END
		
		END TRY
		BEGIN CATCH
			PRINT	 ''- ErrorProcedure : ''+CAST(ERROR_PROCEDURE() AS NVARCHAR(MAX))	 
					+CHAR(13)+''- ErrorLine : ''+CAST(ERROR_LINE() AS NVARCHAR(MAX))	 	 	 	 
					+CHAR(13)+''- ErrorNumber : ''+CAST(ERROR_MESSAGE() AS NVARCHAR(MAX))	 
					+CHAR(13)+''- ErrorMessage : ''+CAST(ERROR_NUMBER() AS NVARCHAR(MAX))	 		
		END CATCH; 
	END
 '
 
ALTER TABLE [dbo].[ReceiptDet] ENABLE TRIGGER [tr_UpdateDataIndex_ReceiptDet]
 
 END