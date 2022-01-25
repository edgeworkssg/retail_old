
IF NOT EXISTS (SELECT * FROM Installment)
BEGIN

    BEGIN TRAN

    INSERT INTO Installment (InstallmentRefNo, OrderHdrId, MembershipNo, InstallmentCreatedDate, TotalAmount, CreatedBy, ModifiedBy, ModifiedOn, Deleted, CreatedOn, CurrentBalance, userfld1)
    SELECT oh.OrderHdrID InstallmentRefNo, oh.OrderHdrID, oh.MembershipNo, oh.OrderDate InstallmentCreatedDate, rd.Amount TotalAmount, 'SCRIPT' CreatedBy, 'SCRIPT' ModifiedBy, oh.OrderDate ModifiedOn, 0 Deleted, oh.OrderDate CreatedOn, 0 CurrentBalance, oh.userfld5
    FROM ReceiptDet rd
        INNER JOIN ReceiptHdr rh ON rh.ReceiptHdrID = rd.ReceiptHdrID
        INNER JOIN OrderHdr oh ON oh.OrderHdrID = rh.OrderHdrID
    WHERE PaymentType = 'INSTALLMENT'
        AND oh.IsVoided = 0 AND rh.IsVoided = 0

    INSERT INTO InstallmentDetail (InstallmentDetRefNo, InstallmentRefNo, InstallmentAmount, OutstandingAmount, Deleted, CreatedOn, CreatedBy, ModifiedOn, ModifiedBy, OrderHdrID, userfld1)
    SELECT oh.OrderHdrId + '.0' InstallmentDetRefNo, oh.OrderHdrID InstallmentRefNo, rd.Amount InstallmentAmount, rd.Amount OutstandingAmount, 0 Deleted, oh.OrderDate CreatedOn, 'SCRIPT' CreatedBy, oh.OrderDate ModifiedOn, 'SCRIPT' ModifiedBy, oh.OrderHdrID, oh.userfld5 
    FROM ReceiptDet rd
        INNER JOIN ReceiptHdr rh ON rh.ReceiptHdrID = rd.ReceiptHdrID
        INNER JOIN OrderHdr oh ON oh.OrderHdrID = rh.OrderHdrID
    WHERE rd.PaymentType = 'INSTALLMENT'
        AND oh.IsVoided = 0 AND rh.IsVoided = 0

    INSERT INTO InstallmentDetail (InstallmentDetRefNo, InstallmentRefNo, InstallmentAmount, OutstandingAmount, Deleted, CreatedOn, CreatedBy, ModifiedOn, ModifiedBy, OrderHdrID, userfld1)
    SELECT InstallmentRefNo + '.' + CAST(RowNum AS varchar(50)), InstallmentRefNo, InstallmentAmount, OutstandingAmount, Deleted, CreatedOn, CreatedBy, ModifiedOn, ModifiedBy, OrderHdrID, CustomInvNo
    FROM (
            SELECT ROW_NUMBER() OVER (PARTITION BY od.userfld3 ORDER BY od.OrderDetID) RowNum, 
                 i.InstallmentRefNo, -od.Amount InstallmentAmount, 0 OutstandingAmount, 0 Deleted, oh.OrderDate CreatedOn, 'SCRIPT' CreatedBy, oh.OrderDate ModifiedOn, 'SCRIPT' ModifiedBy, od.OrderHdrID, oh.userfld5 CustomInvNo
            FROM OrderDet od
                INNER JOIN OrderHdr oh ON oh.OrderHdrID = od.OrderHdrID
                INNER JOIN Installment i ON i.OrderHdrID = od.userfld3
            WHERE oh.IsVoided = 0 AND od.IsVoided = 0
                AND od.ItemNo = 'INST_PAYMENT'
         ) a

    UPDATE i
    SET CurrentBalance = a.InstallmentAmount
    FROM Installment i
        INNER JOIN (
            SELECT i.InstallmentRefNo, SUM(id.InstallmentAmount) InstallmentAmount
            FROM Installment i
                INNER JOIN  InstallmentDetail id ON i.InstallmentRefNo = id.InstallmentRefNo
            GROUP BY i.InstallmentRefNo
         ) a ON i.InstallmentRefNo = a.InstallmentRefNo

    UPDATE InstallmentDetail
    SET OutstandingAmount = (SELECT ISNULL(SUM(InstallmentAmount), 0) FROM InstallmentDetail tmp WHERE tmp.InstallmentRefNo = id.InstallmentRefNo AND tmp.OrderHdrID <= id.OrderHdrID)
    FROM InstallmentDetail id
    WHERE id.InstallmentDetRefNo NOT LIKE '%.0'

    COMMIT

END


IF EXISTS (SELECT * FROM Installment WHERE userfld1 IS NULL)
BEGIN
    BEGIN TRAN

    UPDATE inh
    SET inh.userfld1 = oh.userfld5, ModifiedOn = GETDATE() 
    FROM Installment inh
        INNER JOIN OrderHdr oh ON oh.OrderHdrID = inh.OrderHdrId
    WHERE inh.userfld1 IS NULL

    UPDATE ind
    SET ind.userfld1 = oh.userfld5, ModifiedOn = GETDATE()  
    FROM InstallmentDetail ind
        INNER JOIN OrderHdr oh ON oh.OrderHdrID = ind.OrderHdrID
    WHERE ind.userfld1 IS NULL

    COMMIT
END
