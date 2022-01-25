IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetFundingPrice]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[GetFundingPrice]

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetFundingPrice]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'
CREATE FUNCTION [dbo].[GetFundingPrice] 
(
	@ItemNo varchar(50),
    @FundingSettingKey varchar(50)
)
RETURNS money
AS
BEGIN

DECLARE @price money

SELECT @price = (1 - ISNULL((SELECT CONVERT(money, AppSettingValue) FROM AppSetting WHERE AppSettingKey = @FundingSettingKey), 0) / 100) * RetailPrice
FROM Item WHERE ItemNo = @ItemNo

-- Return the result of the function
RETURN ROUND(ISNULL(@price, 0), 2)

END


' 
END

