IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetLatestDataIndex]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetLatestDataIndex]

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetLatestDataIndex]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE GetLatestDataIndex
	@TableName VARCHAR(50)
AS
BEGIN

	SET NOCOUNT ON;

	DECLARE @DataIndex BIGINT;

	SELECT  @DataIndex = CAST(AppSettingValue AS BIGINT)
	FROM	AppSetting
	WHERE	AppSettingKey = ''DataIndex_''+@TableName

	IF @DataIndex IS NULL BEGIN
		DECLARE @sql NVARCHAR(MAX) = ''SELECT @DataIndex = MAX(DataIndex) FROM ''+@TableName+'' WITH(NOLOCK)'';
		EXECUTE sp_executesql @sql, N''@DataIndex INT OUTPUT'', @DataIndex OUTPUT;
	END

	SET @DataIndex = ISNULL(@DataIndex,0) + 1;

	IF (SELECT COUNT(*) FROM AppSetting WHERE AppSettingKey = ''DataIndex_''+@TableName) = 0 BEGIN
		INSERT INTO AppSetting
		(
			 AppSettingID
			,AppSettingKey
			,AppSettingValue
			,CreatedOn
			,CreatedBy
			,ModifiedOn
			,ModifiedBy
		)
		VALUES
		(NEWID(), ''DataIndex_''+@TableName, CAST(@DataIndex AS VARCHAR(50)), GETDATE(), ''SCRIPT'', GETDATE(), ''SCRIPT'')
	END
	ELSE BEGIN
		UPDATE AppSetting
		SET    AppSettingValue = CAST(@DataIndex AS VARCHAR(50))
		WHERE  AppSettingKey = ''DataIndex_''+@TableName
	END
    SELECT @DataIndex DataIndex;
END
' 
 
END