IF NOT EXISTS (select * from sys.objects where type = 'TR' and name = 'tr_UpdateDataIndex_OrderHdr') BEGIN
EXEC dbo.sp_executesql @statement = N'
	CREATE TRIGGER [dbo].[tr_UpdateDataIndex_OrderHdr] 
		ON  [dbo].[OrderHdr] 
		AFTER INSERT, UPDATE
	AS 
	BEGIN
		SET NOCOUNT ON;
		SET XACT_ABORT OFF;
		BEGIN TRY
			PRINT ''UPDATING DATA INDEX'';
			DECLARE @OrderHdrIDTemp AS TABLE (OrderHdrID VARCHAR(50));
			DECLARE @OrderHdrID VARCHAR(50);

			INSERT INTO @OrderHdrIDTemp
			SELECT OrderHdrID FROM inserted ORDER BY OrderHdrID

			SET @OrderHdrID = (SELECT TOP 1 OrderHdrID FROM @OrderHdrIDTemp ORDER BY OrderHdrID);
			WHILE @OrderHdrID IS NOT NULL BEGIN
				DELETE @OrderHdrIDTemp WHERE OrderHdrID = @OrderHdrID;

				DECLARE @DataIndexTab AS TABLE (DataIndex BIGINT);
				DECLARE @DataIndex BIGINT;

				INSERT INTO @DataIndexTab
				EXEC(''GetLatestDataIndex ''''OrderHdr'''''')

				SELECT	TOP 1 @DataIndex = DataIndex 
				FROM	@DataIndexTab
				UPDATE OrderHdr SET DataIndex = @DataIndex WHERE OrderHdrID = @OrderHdrID

				SET @OrderHdrID = (SELECT TOP 1 OrderHdrID FROM @OrderHdrIDTemp ORDER BY OrderHdrID);
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
 
 ALTER TABLE [dbo].[OrderHdr] ENABLE TRIGGER [tr_UpdateDataIndex_OrderHdr]
 
 END