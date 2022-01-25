IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FetchAuditLogReport]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[FetchAuditLogReport]

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FetchAuditLogReport]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE FetchAuditLogReport
	 @StartDate DATETIME,
	 @EndDate DATETIME,
	 @Operation NVARCHAR(MAX),   
	 @TableName NVARCHAR(MAX),
	 @Search NVARCHAR(MAX)
AS
BEGIN
	 SET NOCOUNT ON; 
	 
	 DECLARE @Sql NVARCHAR(MAX);
	 DECLARE @TableList AS TABLE ( TableName NVARCHAR(MAX) );
	 DECLARE @TablePointer AS NVARCHAR(MAX);
	 DECLARE @TempResult AS TABLE 
	 (
		AuditLogID NVARCHAR(MAX),
		LogDate DATETIME,
		Operation NVARCHAR(MAX),
		TableName NVARCHAR(MAX),
		PrimaryKeyCol NVARCHAR(MAX),
		PrimaryKeyVal NVARCHAR(MAX),
		OldValues NVARCHAR(MAX),
		NewValues NVARCHAR(MAX),
		Remarks NVARCHAR(MAX)
	 );  
	 DECLARE @LanguageSetting NVARCHAR(MAX);
	 DECLARE @LanguagePointer NVARCHAR(MAX);
	 DECLARE @LanguagePointerText NVARCHAR(MAX); 
	 CREATE TABLE #LanguageList 
	 (
		ID NVARCHAR(MAX),
		DisplayText NVARCHAR(MAX)
	 )  
	 
	 SET @LanguageSetting = ''ENG'';
	 SELECT TOP 1 @LanguageSetting = AppSettingValue FROM AppSetting WHERE AppSettingKey = ''LanguageSetting'' 

	 INSERT INTO @TempResult
	 SELECT  AL.AuditLogID  
			 ,AL.LogDate   
			 ,AL.Operation   
			 ,AL.TableName   
			 ,AL.PrimaryKeyCol  
			 ,AL.PrimaryKeyVal   
			 ,AL.OldValues  
			 ,AL.NewValues   
			 ,AL.Remarks   
	 FROM    AuditLog AL   
	 WHERE   (CAST(AL.LogDate AS DATE) >= CAST(@StartDate AS DATE)   
			 AND CAST(AL.LogDate AS DATE) <= CAST(@EndDate AS DATE))   
			 AND (AL.Operation = @Operation OR @Operation = ''ALL'')
			 AND AL.Operation <> ''INSERT''   
	 
	 INSERT INTO @TableList
	 SELECT DISTINCT TableName FROM @TempResult
	 
	 SET @TablePointer = (SELECT TOP 1 TableName FROM @TableList ORDER BY TableName ASC);
	 DELETE @TableList WHERE TableName = @TablePointer;
	 
	 WHILE @TablePointer IS NOT NULL BEGIN
		DELETE #LanguageList
		SET @Sql = N''INSERT INTO #LanguageList SELECT ID, ''+@LanguageSetting+'' FROM TEXT_LANGUAGE WHERE ID LIKE ''''''+@TablePointer+''.%''''''; 
		EXEC sp_executesql @Sql 
	 
		SET @LanguagePointer = (SELECT TOP 1 ID FROM #LanguageList ORDER BY ID);
		SET @LanguagePointerText = (SELECT TOP 1 DisplayText FROM #LanguageList WHERE ID = @LanguagePointer ORDER BY ID);
		DELETE #LanguageList WHERE ID = @LanguagePointer;
		
		WHILE @LanguagePointer IS NOT NULL BEGIN
			
			UPDATE	@TempResult
			SET		TableName = @LanguagePointerText
			WHERE	TableName = REPLACE(@LanguagePointer,@TablePointer+N''.'','''')
			
			UPDATE	@TempResult
			SET		PrimaryKeyCol = @LanguagePointerText
			WHERE	PrimaryKeyCol = REPLACE(@LanguagePointer,@TablePointer+N''.'','''')		
			
			UPDATE	@TempResult
			SET		 NewValues = REPLACE(NewValues,REPLACE(@LanguagePointer,@TablePointer+N''.'','''')+''='',@LanguagePointerText+''='')
					,OldValues = REPLACE(OldValues,REPLACE(@LanguagePointer,@TablePointer+N''.'','''')+''='',@LanguagePointerText+''='')				
			
			SET @LanguagePointer = (SELECT TOP 1 ID FROM #LanguageList ORDER BY ID);
			SET @LanguagePointerText = (SELECT TOP 1 DisplayText FROM #LanguageList WHERE ID = @LanguagePointer ORDER BY ID);
			DELETE #LanguageList WHERE ID = @LanguagePointer;
		END
		
		SET @TablePointer = (SELECT TOP 1 TableName FROM @TableList ORDER BY TableName ASC);
		DELETE @TableList WHERE TableName = @TablePointer; 
	 END
	 
	 DROP TABLE #LanguageList
	 
	 SELECT AL.* 
	 FROM	@TempResult AL
	 WHERE	(AL.TableName = @TableName OR @TableName = ''ALL'')	   	
			 AND (ISNULL(AL.PrimaryKeyCol,'''') LIKE ''%''+@Search+''%''   
				  OR ISNULL(AL.PrimaryKeyVal,'''') LIKE ''%''+@Search+''%''   
				  OR ISNULL(AL.OldValues,'''') LIKE ''%''+@Search+''%''   
				  OR ISNULL(AL.NewValues,'''') LIKE ''%''+@Search+''%''   
				  OR ISNULL(AL.Remarks,'''') LIKE ''%''+@Search+''%'')	    
	 ORDER BY Al.LogDate DESC   
END' 
END
