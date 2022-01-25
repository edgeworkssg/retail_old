IF NOT EXISTS (select * from sys.objects where type = 'TR' and name = 'tr_UpdateDataIndex_OrderDet') BEGIN
EXEC dbo.sp_executesql @statement = N'
	CREATE TRIGGER [dbo].[tr_UpdateDataIndex_OrderDet] 
		ON  [dbo].[OrderDet] 
		AFTER INSERT, UPDATE
	AS 
	BEGIN
		SET NOCOUNT ON;
		SET XACT_ABORT OFF;
		BEGIN TRY
			PRINT ''UPDATING DATA INDEX'';
			DECLARE @OrderDetIDTemp AS TABLE (OrderDetID VARCHAR(50));
			DECLARE @OrderDetID VARCHAR(50);

			INSERT INTO @OrderDetIDTemp
			SELECT OrderDetID FROM inserted ORDER BY OrderDetID

			SET @OrderDetID = (SELECT TOP 1 OrderDetID FROM @OrderDetIDTemp ORDER BY OrderDetID);
			WHILE @OrderDetID IS NOT NULL BEGIN
				DELETE @OrderDetIDTemp WHERE OrderDetID = @OrderDetID;

				DECLARE @DataIndexTab AS TABLE (DataIndex BIGINT);
				DECLARE @DataIndex BIGINT;

				INSERT INTO @DataIndexTab
				EXEC(''GetLatestDataIndex ''''OrderDet'''''')

				SELECT	TOP 1 @DataIndex = DataIndex 
				FROM	@DataIndexTab
				UPDATE OrderDet SET DataIndex = @DataIndex WHERE OrderDetID = @OrderDetID

				SET @OrderDetID = (SELECT TOP 1 OrderDetID FROM @OrderDetIDTemp ORDER BY OrderDetID);
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
 
ALTER TABLE [dbo].[OrderDet] ENABLE TRIGGER [tr_UpdateDataIndex_OrderDet]
 
 END