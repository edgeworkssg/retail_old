IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetInstallmentOutstandingBalance]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[GetInstallmentOutstandingBalance]

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetInstallmentOutstandingBalance]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'

-- =============================================
-- Description:	Function to get the outstanding balance of Installment
-- =============================================
CREATE FUNCTION [dbo].[GetInstallmentOutstandingBalance] 
(
	@OrderHdrID varchar(50),
    @ReportDate datetime
)
RETURNS money
AS
BEGIN
    DECLARE @credit varchar(50)
    DECLARE @creditpayment varchar(50)
	DECLARE @Result money

	SET @credit = ''INSTALLMENT''
    SET @creditpayment = ''INST_PAYMENT''

    SELECT @Result = ISNULL(SUM(Credit),0.00) - ISNULL(SUM(Debit),0.00)
    FROM ( 
            SELECT OrderHdrID AS ReceiptNo , ISNULL(SUM(Amount),0.00) AS Credit, 0.00 AS Debit 
            FROM OrderHdr OH 
                INNER JOIN ReceiptDet RD ON OH.OrderHdrID = RD.ReceiptHdrID 
            WHERE OH.IsVoided = 0 
                AND PaymentType = @credit 
                AND OrderDate <= @ReportDate 
                AND OH.OrderHdrID LIKE @OrderHdrID 
            GROUP BY OrderHdrID  
            UNION ALL  
            SELECT OD.Userfld3 as ReceiptNo ,0.00 as Credit, ISNULL(SUM(AMOUNT),0.00) as Debit  
            FROM OrderHdr OH 
                INNER JOIN OrderDet OD ON OH.OrderHdrID = OD.OrderHdrID 
            WHERE OH.IsVoided = 0 
                AND OD.IsVoided = 0 
                AND ItemNo = @creditpayment 
                AND OrderDate <= @ReportDate 
                AND OD.Userfld3 LIKE @OrderHdrID 
            GROUP BY OD.Userfld3 
        ) DT 
    GROUP BY ReceiptNo  

	-- Return the result of the function
	RETURN ISNULL(@Result, 0)

END

                        
' 
END

