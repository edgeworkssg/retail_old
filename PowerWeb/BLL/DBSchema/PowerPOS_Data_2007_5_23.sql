ALTER TABLE [CashRecordingType] NOCHECK CONSTRAINT ALL
GO

SET IDENTITY_INSERT [CashRecordingType] ON 
PRINT 'Begin inserting data in CashRecordingType'
INSERT INTO [CashRecordingType] ([CashRecordingTypeId], [CashRecordingTypeName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy])
VALUES(1, 'Cash In', '-', '8/4/2007 12:00:00 AM', 'alberto', '8/4/2007 12:00:00 AM', 'alberto')
INSERT INTO [CashRecordingType] ([CashRecordingTypeId], [CashRecordingTypeName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy])
VALUES(2, 'Cash Out', '-', '8/4/2007 12:00:00 AM', 'alberto', '8/4/2007 12:00:00 AM', 'alberto')
INSERT INTO [CashRecordingType] ([CashRecordingTypeId], [CashRecordingTypeName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy])
VALUES(3, 'Opening Balance', '-', '8/4/2007 12:00:00 AM', 'alberto', '8/4/2007 12:00:00 AM', 'alberto')
INSERT INTO [CashRecordingType] ([CashRecordingTypeId], [CashRecordingTypeName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy])
VALUES(4, 'Closing Balance', NULL, '10/4/2007 7:48:47 PM', 'IMPACT-C491D6CC\Albert', '10/4/2007 7:48:47 PM', 'IMPACT-C491D6CC\Albert')
SET IDENTITY_INSERT [CashRecordingType] OFF 
ALTER TABLE [CashRecordingType] CHECK CONSTRAINT ALL
GO



ALTER TABLE [Category] NOCHECK CONSTRAINT ALL
GO

PRINT 'Begin inserting data in Category'
INSERT INTO [Category] ([CategoryName], [Remarks], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('Alcoholic Beverages', NULL, 'YOUR-F7C39A7C38\jenniferleet', '3/4/2007 8:43:57 PM', 'YOUR-F7C39A7C38\jenniferleet', '3/4/2007 8:43:57 PM')
INSERT INTO [Category] ([CategoryName], [Remarks], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('Books', '', '', '22/5/2007 3:40:13 AM', '', '22/5/2007 3:40:13 AM')
INSERT INTO [Category] ([CategoryName], [Remarks], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('Can Drinks', 'Coke, Pepsi, 7 up etc', '', '14/4/2007 3:10:59 AM', '', '14/4/2007 3:10:59 AM')
INSERT INTO [Category] ([CategoryName], [Remarks], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('Cigarettes', 'Cigarettes', '', '15/4/2007 8:47:24 PM', '', '15/4/2007 8:47:24 PM')
INSERT INTO [Category] ([CategoryName], [Remarks], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('Drinks', 'Drinks', 'YOUR-F7C39A7C38\jenniferleet', '3/4/2007 8:43:39 PM', 'YOUR-F7C39A7C38\jenniferleet', '3/4/2007 8:43:39 PM')
INSERT INTO [Category] ([CategoryName], [Remarks], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('Food', 'Food', 'YOUR-F7C39A7C38\jenniferleet', '3/4/2007 8:43:46 PM', 'IMPACT-C491D6CC\Albert', '12/4/2007 5:20:49 AM')
INSERT INTO [Category] ([CategoryName], [Remarks], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('Prata', 'kosong, egg, banana prata, etc', '', '14/4/2007 1:21:45 AM', '', '14/4/2007 1:21:45 AM')
INSERT INTO [Category] ([CategoryName], [Remarks], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('Seafood', 'Seafood paste', '', '23/4/2007 5:11:04 PM', '', '23/4/2007 5:11:04 PM')
ALTER TABLE [Category] CHECK CONSTRAINT ALL
GO



ALTER TABLE [Department] NOCHECK CONSTRAINT ALL
GO

SET IDENTITY_INSERT [Department] ON 
PRINT 'Begin inserting data in Department'
INSERT INTO [Department] ([DepartmentID], [DepartmentName], [Remark], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(1, 'Publishing', 'publish books. CD, and etc', 'IMPACT-C491D6CC\Albert', '19/5/2007 10:54:29 AM', 'IMPACT-C491D6CC\Albert', '19/5/2007 10:54:29 AM')
INSERT INTO [Department] ([DepartmentID], [DepartmentName], [Remark], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(2, 'Customer Service', 'Handles customer requests', 'IMPACT-C491D6CC\Albert', '19/5/2007 10:54:45 AM', 'IMPACT-C491D6CC\Albert', '19/5/2007 10:54:45 AM')
INSERT INTO [Department] ([DepartmentID], [DepartmentName], [Remark], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(3, 'Accounts', 'Handle accounts', 'IMPACT-C491D6CC\Albert', '19/5/2007 10:54:56 AM', 'IMPACT-C491D6CC\Albert', '19/5/2007 10:54:56 AM')
SET IDENTITY_INSERT [Department] OFF 
ALTER TABLE [Department] CHECK CONSTRAINT ALL
GO



ALTER TABLE [EditBillLog] NOCHECK CONSTRAINT ALL
GO

PRINT 'Begin inserting data in EditBillLog'
ALTER TABLE [EditBillLog] CHECK CONSTRAINT ALL
GO



ALTER TABLE [EditInvLog] NOCHECK CONSTRAINT ALL
GO

PRINT 'Begin inserting data in EditInvLog'
ALTER TABLE [EditInvLog] CHECK CONSTRAINT ALL
GO



ALTER TABLE [InventoryHdr] NOCHECK CONSTRAINT ALL
GO

PRINT 'Begin inserting data in InventoryHdr'
INSERT INTO [InventoryHdr] ([InventoryHdrRefNo], [InventoryDate], [UserName], [PurchaseOrderNo], [InvoiceNo], [DeliveryOrderNo], [Supplier], [Tax], [Discount], [Remark], [PointOfSaleId], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy], [UniqueID])
VALUES('IN07052100020001', '21/5/2007 11:33:51 PM', 'admin', NULL, '', NULL, '', NULL, NULL, 'Stock In', 2, '21/5/2007 11:34:44 PM', '21/5/2007 11:34:44 PM', 'admin', 'admin', 'e15dbb2c-dcb6-482f-8944-92863a60b751')
INSERT INTO [InventoryHdr] ([InventoryHdrRefNo], [InventoryDate], [UserName], [PurchaseOrderNo], [InvoiceNo], [DeliveryOrderNo], [Supplier], [Tax], [Discount], [Remark], [PointOfSaleId], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy], [UniqueID])
VALUES('IN07052100020002', '21/5/2007 11:36:18 PM', 'admin', NULL, '', NULL, '', NULL, NULL, 'Stock In', 2, '21/5/2007 11:36:39 PM', '21/5/2007 11:36:39 PM', 'admin', 'admin', '4298767a-cc8d-4152-8470-9f043a30f9ba')
INSERT INTO [InventoryHdr] ([InventoryHdrRefNo], [InventoryDate], [UserName], [PurchaseOrderNo], [InvoiceNo], [DeliveryOrderNo], [Supplier], [Tax], [Discount], [Remark], [PointOfSaleId], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy], [UniqueID])
VALUES('IN07052100020003', '21/5/2007 11:47:10 PM', 'admin', NULL, NULL, NULL, NULL, NULL, NULL, 'sales', 2, '21/5/2007 11:47:10 PM', '21/5/2007 11:47:10 PM', 'admin', 'admin', '11a16a0c-61a3-40d8-8790-7fef8b77d91e')
INSERT INTO [InventoryHdr] ([InventoryHdrRefNo], [InventoryDate], [UserName], [PurchaseOrderNo], [InvoiceNo], [DeliveryOrderNo], [Supplier], [Tax], [Discount], [Remark], [PointOfSaleId], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy], [UniqueID])
VALUES('IN07052100020004', '21/5/2007 11:49:37 PM', 'admin', NULL, NULL, NULL, NULL, NULL, NULL, 'sales', 2, '21/5/2007 11:49:37 PM', '21/5/2007 11:49:37 PM', 'admin', 'admin', 'ea92617f-7074-4f47-8d4d-1870340e7a58')
INSERT INTO [InventoryHdr] ([InventoryHdrRefNo], [InventoryDate], [UserName], [PurchaseOrderNo], [InvoiceNo], [DeliveryOrderNo], [Supplier], [Tax], [Discount], [Remark], [PointOfSaleId], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy], [UniqueID])
VALUES('IN07052100020005', '21/5/2007 11:49:59 PM', 'admin', NULL, NULL, NULL, NULL, NULL, NULL, 'sales', 2, '21/5/2007 11:49:59 PM', '21/5/2007 11:49:59 PM', 'admin', 'admin', '4172910b-e9bd-4bbc-a198-bb908ca08aee')
INSERT INTO [InventoryHdr] ([InventoryHdrRefNo], [InventoryDate], [UserName], [PurchaseOrderNo], [InvoiceNo], [DeliveryOrderNo], [Supplier], [Tax], [Discount], [Remark], [PointOfSaleId], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy], [UniqueID])
VALUES('IN07052100020006', '21/5/2007 11:50:19 PM', 'admin', NULL, '', NULL, '', NULL, NULL, 'Stock In', 2, '21/5/2007 11:50:28 PM', '21/5/2007 11:50:28 PM', 'admin', 'admin', '4b2a53ec-2d5f-4daf-8165-e4937e910ef3')
INSERT INTO [InventoryHdr] ([InventoryHdrRefNo], [InventoryDate], [UserName], [PurchaseOrderNo], [InvoiceNo], [DeliveryOrderNo], [Supplier], [Tax], [Discount], [Remark], [PointOfSaleId], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy], [UniqueID])
VALUES('IN07052100020007', '21/5/2007 11:50:59 PM', 'admin', NULL, NULL, NULL, NULL, NULL, NULL, 'sales', 2, '21/5/2007 11:50:59 PM', '21/5/2007 11:50:59 PM', 'admin', 'admin', 'db3c2560-949b-4f23-9207-8a92d237eefe')
INSERT INTO [InventoryHdr] ([InventoryHdrRefNo], [InventoryDate], [UserName], [PurchaseOrderNo], [InvoiceNo], [DeliveryOrderNo], [Supplier], [Tax], [Discount], [Remark], [PointOfSaleId], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy], [UniqueID])
VALUES('IN07052100020008', '21/5/2007 11:57:01 PM', 'admin', NULL, NULL, NULL, NULL, NULL, NULL, 'Expired', 2, '21/5/2007 11:57:11 PM', '21/5/2007 11:57:11 PM', 'admin', 'admin', 'fd9cb573-ec10-48e8-984d-b0a29ad9824f')
INSERT INTO [InventoryHdr] ([InventoryHdrRefNo], [InventoryDate], [UserName], [PurchaseOrderNo], [InvoiceNo], [DeliveryOrderNo], [Supplier], [Tax], [Discount], [Remark], [PointOfSaleId], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy], [UniqueID])
VALUES('IN07052200020001', '22/5/2007 1:51:02 AM', 'admin', NULL, NULL, NULL, NULL, NULL, NULL, 'sales', 2, '22/5/2007 1:51:02 AM', '22/5/2007 1:51:02 AM', 'admin', 'admin', '8c3e3cd7-56b1-4b44-8a70-9050876d1228')
INSERT INTO [InventoryHdr] ([InventoryHdrRefNo], [InventoryDate], [UserName], [PurchaseOrderNo], [InvoiceNo], [DeliveryOrderNo], [Supplier], [Tax], [Discount], [Remark], [PointOfSaleId], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy], [UniqueID])
VALUES('IN07052200020002', '22/5/2007 3:21:00 AM', 'admin', NULL, NULL, NULL, NULL, NULL, NULL, 'sales', 2, '22/5/2007 3:21:00 AM', '22/5/2007 3:21:00 AM', 'admin', 'admin', '72e094c9-9fe9-45b8-b461-b42acb4b4a43')
INSERT INTO [InventoryHdr] ([InventoryHdrRefNo], [InventoryDate], [UserName], [PurchaseOrderNo], [InvoiceNo], [DeliveryOrderNo], [Supplier], [Tax], [Discount], [Remark], [PointOfSaleId], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy], [UniqueID])
VALUES('IN07052200020003', '22/5/2007 3:25:53 AM', 'admin', NULL, NULL, NULL, NULL, NULL, NULL, 'sales', 2, '22/5/2007 3:25:53 AM', '22/5/2007 3:25:53 AM', 'admin', 'admin', '63ad5c24-84a2-42a0-82ae-7eb2c3518f57')
INSERT INTO [InventoryHdr] ([InventoryHdrRefNo], [InventoryDate], [UserName], [PurchaseOrderNo], [InvoiceNo], [DeliveryOrderNo], [Supplier], [Tax], [Discount], [Remark], [PointOfSaleId], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy], [UniqueID])
VALUES('IN07052200030001', '22/5/2007 3:27:43 AM', 'admin', NULL, NULL, NULL, NULL, NULL, NULL, 'sales', 3, '22/5/2007 3:27:43 AM', '22/5/2007 3:27:43 AM', 'admin', 'admin', '657440f1-eb25-4ac5-bfaf-2f49eee7af01')
INSERT INTO [InventoryHdr] ([InventoryHdrRefNo], [InventoryDate], [UserName], [PurchaseOrderNo], [InvoiceNo], [DeliveryOrderNo], [Supplier], [Tax], [Discount], [Remark], [PointOfSaleId], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy], [UniqueID])
VALUES('IN07052200030002', '22/5/2007 3:27:55 AM', 'admin', NULL, NULL, NULL, NULL, NULL, NULL, 'sales', 3, '22/5/2007 3:27:55 AM', '22/5/2007 3:27:55 AM', 'admin', 'admin', '33f683c1-bd71-4b74-8701-6d48808517e3')
INSERT INTO [InventoryHdr] ([InventoryHdrRefNo], [InventoryDate], [UserName], [PurchaseOrderNo], [InvoiceNo], [DeliveryOrderNo], [Supplier], [Tax], [Discount], [Remark], [PointOfSaleId], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy], [UniqueID])
VALUES('IN07052200030003', '22/5/2007 3:28:05 AM', 'admin', NULL, NULL, NULL, NULL, NULL, NULL, 'sales', 3, '22/5/2007 3:28:05 AM', '22/5/2007 3:28:05 AM', 'admin', 'admin', '71c89755-f2d2-43f5-ac89-c7f7da97fec1')
INSERT INTO [InventoryHdr] ([InventoryHdrRefNo], [InventoryDate], [UserName], [PurchaseOrderNo], [InvoiceNo], [DeliveryOrderNo], [Supplier], [Tax], [Discount], [Remark], [PointOfSaleId], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy], [UniqueID])
VALUES('IN07052200030004', '22/5/2007 7:31:40 PM', 'admin', NULL, '', NULL, '', NULL, NULL, 'Stock In', 3, '22/5/2007 7:31:50 PM', '22/5/2007 7:31:50 PM', 'admin', 'admin', '82451ac5-bdb0-4f33-9fd1-5994e378d28a')
ALTER TABLE [InventoryHdr] CHECK CONSTRAINT ALL
GO



ALTER TABLE [Item] NOCHECK CONSTRAINT ALL
GO

PRINT 'Begin inserting data in Item'
INSERT INTO [Item] ([ItemNo], [ItemName], [Barcode], [CategoryName], [Price], [MinPrice], [ItemDesc], [IsInInventory], [Brand], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('I00000000001', 'Coca Cola', '8888002076009', 'Can Drinks', 1.2000, 0.0000, 'always coca cola', 1, 'coke', 'nice', '14/4/2007 3:11:34 AM', '', '9/5/2007 7:58:19 PM', 'SYNC', 'f01c6db6-7b65-41af-bcdf-eaf221435d2a')
INSERT INTO [Item] ([ItemNo], [ItemName], [Barcode], [CategoryName], [Price], [MinPrice], [ItemDesc], [IsInInventory], [Brand], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('I00000000002', 'Kopi', '12091829101', 'Drinks', 1.2000, 0.9000, 'Coffee with sugar and tea', NULL, 'Apek', '', '4/4/2007 9:39:00 AM', 'YOUR-F7C39A7C38\jenniferleet', '9/5/2007 7:58:19 PM', 'SYNC', 'a7a396b9-f81d-4b29-af95-53da8dd99b76')
INSERT INTO [Item] ([ItemNo], [ItemName], [Barcode], [CategoryName], [Price], [MinPrice], [ItemDesc], [IsInInventory], [Brand], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('I00000000003', 'TE-0', '12345', 'Alcoholic Beverages', 0.9000, 0.9000, 'Tea without milk', NULL, 'Apek', '', '4/4/2007 9:40:02 AM', 'YOUR-F7C39A7C38\jenniferleet', '9/5/2007 7:58:19 PM', 'SYNC', 'd3172e2b-6d0e-4042-bfb5-ddec95b2772f')
INSERT INTO [Item] ([ItemNo], [ItemName], [Barcode], [CategoryName], [Price], [MinPrice], [ItemDesc], [IsInInventory], [Brand], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('I00000000004', 'Chicken Teriyaki', '987654', 'Food', 3.0000, 0.0000, '', NULL, '', '', '12/4/2007 5:19:28 AM', 'IMPACT-C491D6CC\Albert', '9/5/2007 7:58:19 PM', 'SYNC', '6bac64e4-7927-4cbb-800e-7d7ac2c005fd')
INSERT INTO [Item] ([ItemNo], [ItemName], [Barcode], [CategoryName], [Price], [MinPrice], [ItemDesc], [IsInInventory], [Brand], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('I00000000005', 'Chicken Katsu Don', '456456456', 'Food', 3.5000, 2.0000, '', NULL, 'Moshi Moshi', '', '12/4/2007 5:21:40 AM', 'IMPACT-C491D6CC\Albert', '9/5/2007 7:58:19 PM', 'SYNC', '83c33ba1-7b37-4d15-9d0e-0e9c70db2a20')
INSERT INTO [Item] ([ItemNo], [ItemName], [Barcode], [CategoryName], [Price], [MinPrice], [ItemDesc], [IsInInventory], [Brand], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('I00000000006', 'Hokkien Mee', '', 'Food', 3.5000, 0.0000, 'Seafood', NULL, '', '', '14/4/2007 3:21:52 AM', '', '9/5/2007 7:58:19 PM', 'SYNC', '2cebfe17-c913-41f0-8198-af49d7c29345')
INSERT INTO [Item] ([ItemNo], [ItemName], [Barcode], [CategoryName], [Price], [MinPrice], [ItemDesc], [IsInInventory], [Brand], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('I00000000007', 'Chicken Cutlet', '13245688797', 'Food', 5.0000, 3.0000, 'Chicken chopped in small pieces', NULL, '', '', '15/4/2007 7:21:27 PM', '', '9/5/2007 7:58:19 PM', 'SYNC', 'b705b01a-d873-4713-8de1-b3e9d57bd51d')
INSERT INTO [Item] ([ItemNo], [ItemName], [Barcode], [CategoryName], [Price], [MinPrice], [ItemDesc], [IsInInventory], [Brand], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('I00000000008', 'Pork Chop', '', 'Food', 6.0000, 5.0000, 'Pork meat', NULL, '', '', '15/4/2007 7:22:04 PM', '', '9/5/2007 7:58:19 PM', 'SYNC', '8534a992-38cd-45fc-960e-d2d2a2b8dc21')
INSERT INTO [Item] ([ItemNo], [ItemName], [Barcode], [CategoryName], [Price], [MinPrice], [ItemDesc], [IsInInventory], [Brand], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('I00000000009', 'Banana Prata', '', 'Prata', 1.4000, 0.0000, '', NULL, '', '', '15/4/2007 7:22:20 PM', '', '9/5/2007 7:58:19 PM', 'SYNC', 'f7595927-2dc7-4509-b4bb-f18bb6c1ce0e')
INSERT INTO [Item] ([ItemNo], [ItemName], [Barcode], [CategoryName], [Price], [MinPrice], [ItemDesc], [IsInInventory], [Brand], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('I00000000010', 'Egg Prata', '', 'Prata', 1.2000, 0.0000, 'Prata mix with egg', NULL, '', '', '15/4/2007 7:22:45 PM', '', '9/5/2007 7:58:19 PM', 'SYNC', '6aa243bb-1e15-40b3-9d10-5e7d21ca8f9f')
INSERT INTO [Item] ([ItemNo], [ItemName], [Barcode], [CategoryName], [Price], [MinPrice], [ItemDesc], [IsInInventory], [Brand], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('I00000000011', '中文', '123456789', 'Alcoholic Beverages', 1.2000, 0.0000, '', NULL, '', '', '16/4/2007 3:22:26 PM', '', '9/5/2007 7:58:19 PM', 'SYNC', '209e8f10-029b-414a-96c5-bfd1411e4c35')
INSERT INTO [Item] ([ItemNo], [ItemName], [Barcode], [CategoryName], [Price], [MinPrice], [ItemDesc], [IsInInventory], [Brand], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('I00000000012', 'Fried Rice', '12345678901255', 'Food', 2.5000, 0.0000, 'Malaysian Fried Rice', NULL, '', '', '17/4/2007 2:56:52 PM', '', '9/5/2007 7:58:19 PM', 'SYNC', '88b916e4-3a3d-45d3-8e4c-af71bba62f18')
INSERT INTO [Item] ([ItemNo], [ItemName], [Barcode], [CategoryName], [Price], [MinPrice], [ItemDesc], [IsInInventory], [Brand], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('I00000000013', 'Char Kwey Teo', '1254781249100', 'Food', 3.5000, 0.0000, '', NULL, '', '', '17/4/2007 2:57:21 PM', '', '9/5/2007 7:58:19 PM', 'SYNC', '5e378967-3a6a-4c3b-8982-2ead08c10bbe')
INSERT INTO [Item] ([ItemNo], [ItemName], [Barcode], [CategoryName], [Price], [MinPrice], [ItemDesc], [IsInInventory], [Brand], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('I00000000014', 'Roti Prata', '251347981223', 'Prata', 4.5000, 0.0000, 'Standard ', NULL, '', '', '17/4/2007 2:59:56 PM', '', '9/5/2007 7:58:19 PM', 'SYNC', '2f075595-78a9-4641-b535-58aa15c5e99e')
INSERT INTO [Item] ([ItemNo], [ItemName], [Barcode], [CategoryName], [Price], [MinPrice], [ItemDesc], [IsInInventory], [Brand], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('I00000000015', 'Pineapple Fried Rice', '784513546810', 'Food', 2.3000, 0.0000, 'Fried Rice mixed with pineapple', NULL, '', '', '17/4/2007 3:00:35 PM', '', '9/5/2007 7:58:19 PM', 'SYNC', 'dc1bdb71-70e0-4aac-85d1-eb5bf5b2b2b3')
INSERT INTO [Item] ([ItemNo], [ItemName], [Barcode], [CategoryName], [Price], [MinPrice], [ItemDesc], [IsInInventory], [Brand], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('I00000000016', 'Singapore Sling', '5462138791546', 'Drinks', 6.0500, 4.0000, 'Vodka mixed with lime juice', NULL, '', '', '17/4/2007 3:02:00 PM', '', '9/5/2007 7:58:19 PM', 'SYNC', '2749346e-5f23-40da-8c48-9e98ee4ccd98')
INSERT INTO [Item] ([ItemNo], [ItemName], [Barcode], [CategoryName], [Price], [MinPrice], [ItemDesc], [IsInInventory], [Brand], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('I00000000017', 'Peach Juice', '787454121214', 'Drinks', 3.5000, 2.5000, 'Juice mixed with peach', NULL, '', '', '17/4/2007 3:02:58 PM', '', '9/5/2007 7:58:19 PM', 'SYNC', '6b90891e-a81d-4a31-b0cd-6fb438b5ae97')
INSERT INTO [Item] ([ItemNo], [ItemName], [Barcode], [CategoryName], [Price], [MinPrice], [ItemDesc], [IsInInventory], [Brand], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('I00000000018', 'Sushi', '45121348714213', 'Food', 4.5000, 3.0000, 'Many Mix of Sushi ', NULL, '', '', '17/4/2007 3:03:56 PM', '', '9/5/2007 7:58:19 PM', 'SYNC', 'f91a3e2c-4d4e-4ea7-95fd-6a20b7b211af')
INSERT INTO [Item] ([ItemNo], [ItemName], [Barcode], [CategoryName], [Price], [MinPrice], [ItemDesc], [IsInInventory], [Brand], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('I00000000019', 'Taro Prawn', '7814254546213', 'Food', 4.5000, 1.2000, 'Fried Prawn Cooked Thai Style', NULL, '', '', '17/4/2007 3:08:57 PM', '', '9/5/2007 7:58:19 PM', 'SYNC', 'f83fae3c-7072-4d94-b538-6cf9de881b78')
INSERT INTO [Item] ([ItemNo], [ItemName], [Barcode], [CategoryName], [Price], [MinPrice], [ItemDesc], [IsInInventory], [Brand], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('I00000000020', 'Crab Cakes', '12345787945131', 'Food', 4.0000, 2.0000, 'Appetizire', NULL, '', '', '17/4/2007 3:09:39 PM', '', '9/5/2007 7:58:19 PM', 'SYNC', '048228c5-90b9-4fea-8cb8-c298e5f9f743')
INSERT INTO [Item] ([ItemNo], [ItemName], [Barcode], [CategoryName], [Price], [MinPrice], [ItemDesc], [IsInInventory], [Brand], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('I00000000021', 'Curry Tofu', '123456/913123213', 'Food', 3.0000, 0.0000, 'Curry with Tofu', NULL, '', '', '17/4/2007 3:15:06 PM', '', '9/5/2007 7:58:19 PM', 'SYNC', '479813db-e791-483b-950b-5a331d23b289')
INSERT INTO [Item] ([ItemNo], [ItemName], [Barcode], [CategoryName], [Price], [MinPrice], [ItemDesc], [IsInInventory], [Brand], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('I00000000022', 'Guiness Stout', '7546456465123', 'Alcoholic Beverages', 10.0000, 8.0000, '', 1, '', '', '17/4/2007 3:15:53 PM', '', '9/5/2007 7:58:19 PM', 'SYNC', '0ccda21b-056d-46de-99c9-df6cfcfab38a')
INSERT INTO [Item] ([ItemNo], [ItemName], [Barcode], [CategoryName], [Price], [MinPrice], [ItemDesc], [IsInInventory], [Brand], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('I00000000023', 'Thai Noodle', '7894562135464', 'Food', 3.5000, 0.0000, 'Thai Style Noodle', NULL, '', '', '17/4/2007 3:16:44 PM', '', '9/5/2007 7:58:19 PM', 'SYNC', '65c46f63-8806-4164-86f8-9f6f9a4178ca')
INSERT INTO [Item] ([ItemNo], [ItemName], [Barcode], [CategoryName], [Price], [MinPrice], [ItemDesc], [IsInInventory], [Brand], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('I00000000024', 'Green Apple Juice', '132154567879', 'Drinks', 3.0000, 0.0000, 'Juice of apple', NULL, '', '', '17/4/2007 3:17:27 PM', '', '9/5/2007 7:58:19 PM', 'SYNC', '81bbfbae-e156-4839-8cba-2133b58c5d32')
INSERT INTO [Item] ([ItemNo], [ItemName], [Barcode], [CategoryName], [Price], [MinPrice], [ItemDesc], [IsInInventory], [Brand], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('I00000000025', 'Mango Noodle ', '12345689789412', 'Food', 2.8000, 1.2000, 'Noodle with mango flavor', NULL, '', '', '17/4/2007 3:20:07 PM', '', '9/5/2007 7:58:19 PM', 'SYNC', 'c232ceb1-4c6a-4fdc-b984-468816206192')
INSERT INTO [Item] ([ItemNo], [ItemName], [Barcode], [CategoryName], [Price], [MinPrice], [ItemDesc], [IsInInventory], [Brand], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('I00000000026', 'Tempura', '4645621313', 'Food', 3.5000, 0.0000, '', NULL, '', '', '17/4/2007 3:20:36 PM', '', '9/5/2007 7:58:19 PM', 'SYNC', '00ddeb19-ea80-442e-a0bf-a2df7f98b3e6')
INSERT INTO [Item] ([ItemNo], [ItemName], [Barcode], [CategoryName], [Price], [MinPrice], [ItemDesc], [IsInInventory], [Brand], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('I00000000027', 'Walnut Cakes', '213567894561', 'Food', 4.5000, 0.0000, '', NULL, '', '', '17/4/2007 3:21:05 PM', '', '9/5/2007 7:58:19 PM', 'SYNC', '629f71e1-3dd0-4845-acf5-58eebd54bbe1')
INSERT INTO [Item] ([ItemNo], [ItemName], [Barcode], [CategoryName], [Price], [MinPrice], [ItemDesc], [IsInInventory], [Brand], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('I00000000028', 'Mixed Green', '12345678915516', 'Food', 4.5000, 0.0000, 'Vegetables and salad mix', NULL, '', '', '17/4/2007 3:23:16 PM', '', '9/5/2007 7:58:19 PM', 'SYNC', 'e3c89337-ea3f-464b-a7c8-bb729311f5b0')
INSERT INTO [Item] ([ItemNo], [ItemName], [Barcode], [CategoryName], [Price], [MinPrice], [ItemDesc], [IsInInventory], [Brand], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('I00000000029', 'Aquarin', '8888815800662', 'Drinks', 0.9000, 0.0000, '', NULL, '', '', '17/4/2007 4:51:31 PM', '', '9/5/2007 7:58:19 PM', 'SYNC', '6065ead7-8389-4f5c-b8fd-ab18f5b8e6c5')
INSERT INTO [Item] ([ItemNo], [ItemName], [Barcode], [CategoryName], [Price], [MinPrice], [ItemDesc], [IsInInventory], [Brand], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('I00000000030', 'Hot Plate Chicken', '1234568812012', 'Food', 4.5000, 0.0000, '', NULL, '', '', '23/4/2007 5:10:48 PM', '', '9/5/2007 7:58:19 PM', 'SYNC', 'be5119e0-0328-4a70-953f-f62056de0376')
INSERT INTO [Item] ([ItemNo], [ItemName], [Barcode], [CategoryName], [Price], [MinPrice], [ItemDesc], [IsInInventory], [Brand], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('I00200000031', 'Macaroni', '546789454000', 'Alcoholic Beverages', 1.5000, 0.0000, '', NULL, '', '', '30/4/2007 5:12:32 PM', '', '9/5/2007 7:58:19 PM', 'SYNC', 'd85dd703-90a9-4be3-91e0-d705e20d2d7a')
INSERT INTO [Item] ([ItemNo], [ItemName], [Barcode], [CategoryName], [Price], [MinPrice], [ItemDesc], [IsInInventory], [Brand], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('I00200000032', 'Fanta', '5845412100', 'Drinks', 1.2000, 1.0000, '', 1, '', '', '8/5/2007 10:13:28 PM', 'SYNC', '9/5/2007 7:58:19 PM', 'SYNC', 'c4203104-541b-4fd3-930d-54badf13a425')
ALTER TABLE [Item] CHECK CONSTRAINT ALL
GO



ALTER TABLE [ItemGroup] NOCHECK CONSTRAINT ALL
GO

SET IDENTITY_INSERT [ItemGroup] ON 
PRINT 'Begin inserting data in ItemGroup'
SET IDENTITY_INSERT [ItemGroup] OFF 
ALTER TABLE [ItemGroup] CHECK CONSTRAINT ALL
GO



ALTER TABLE [ItemGroupMap] NOCHECK CONSTRAINT ALL
GO

SET IDENTITY_INSERT [ItemGroupMap] ON 
PRINT 'Begin inserting data in ItemGroupMap'
SET IDENTITY_INSERT [ItemGroupMap] OFF 
ALTER TABLE [ItemGroupMap] CHECK CONSTRAINT ALL
GO



ALTER TABLE [Outlet] NOCHECK CONSTRAINT ALL
GO

PRINT 'Begin inserting data in Outlet'
INSERT INTO [Outlet] ([OutletName], [OutletAddress], [PhoneNo], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy])
VALUES('Hougang', 'Hougang Avenue 3', '67121213', 'test PointOfSale', '16/4/2007 6:19:48 AM', 'IMPACT-C491D6CC\Albert', '23/4/2007 12:58:11 AM', 'IMPACT-C491D6CC\Albert')
INSERT INTO [Outlet] ([OutletName], [OutletAddress], [PhoneNo], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy])
VALUES('Serangoon', 'Serangoon Ave 4', '1234567', '11', '4/4/2007 9:40:24 AM', 'YOUR-F7C39A7C38\jenniferleet', '4/4/2007 9:40:24 AM', 'YOUR-F7C39A7C38\jenniferleet')
INSERT INTO [Outlet] ([OutletName], [OutletAddress], [PhoneNo], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy])
VALUES('Web', 'Web Module', NULL, NULL, NULL, NULL, NULL, NULL)
ALTER TABLE [Outlet] CHECK CONSTRAINT ALL
GO



ALTER TABLE [PaymentTerm] NOCHECK CONSTRAINT ALL
GO

SET IDENTITY_INSERT [PaymentTerm] ON 
PRINT 'Begin inserting data in PaymentTerm'
INSERT INTO [PaymentTerm] ([PaymentTermID], [PaymentTermName], [Remark], [CreatedBy], [ModifiedBy], [CreatedOn], [ModifiedOn])
VALUES(1, '15 Days', NULL, 'IMPACT-C491D6CC\Albert', 'IMPACT-C491D6CC\Albert', '19/5/2007 1:01:56 PM', '19/5/2007 1:01:56 PM')
INSERT INTO [PaymentTerm] ([PaymentTermID], [PaymentTermName], [Remark], [CreatedBy], [ModifiedBy], [CreatedOn], [ModifiedOn])
VALUES(2, '30 Days', NULL, 'IMPACT-C491D6CC\Albert', 'IMPACT-C491D6CC\Albert', '19/5/2007 1:02:01 PM', '19/5/2007 1:02:01 PM')
INSERT INTO [PaymentTerm] ([PaymentTermID], [PaymentTermName], [Remark], [CreatedBy], [ModifiedBy], [CreatedOn], [ModifiedOn])
VALUES(3, 'Immediate', NULL, 'IMPACT-C491D6CC\Albert', 'IMPACT-C491D6CC\Albert', '19/5/2007 1:02:07 PM', '19/5/2007 1:02:07 PM')
INSERT INTO [PaymentTerm] ([PaymentTermID], [PaymentTermName], [Remark], [CreatedBy], [ModifiedBy], [CreatedOn], [ModifiedOn])
VALUES(4, 'Forever', NULL, 'IMPACT-C491D6CC\Albert', 'IMPACT-C491D6CC\Albert', '19/5/2007 1:02:12 PM', '19/5/2007 1:02:12 PM')
SET IDENTITY_INSERT [PaymentTerm] OFF 
ALTER TABLE [PaymentTerm] CHECK CONSTRAINT ALL
GO



ALTER TABLE [PaymentType] NOCHECK CONSTRAINT ALL
GO

PRINT 'Begin inserting data in PaymentType'
ALTER TABLE [PaymentType] CHECK CONSTRAINT ALL
GO



ALTER TABLE [PromoCampaignHdr] NOCHECK CONSTRAINT ALL
GO

SET IDENTITY_INSERT [PromoCampaignHdr] ON 
PRINT 'Begin inserting data in PromoCampaignHdr'
INSERT INTO [PromoCampaignHdr] ([PromoCampaignHdrID], [PromoCampaignName], [CampaignType], [DateFrom], [DateTo], [PromoPrice], [PromoDiscount], [FreeQty], [FreeItemNo], [Remark], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(15, 'Cigar Promo', 'DiscountByCategory', '22/5/2007 12:00:00 AM', '29/5/2007 12:00:00 AM', NULL, 3.2, NULL, NULL, NULL, NULL, '22/5/2007 1:26:10 PM', NULL, '22/5/2007 1:26:10 PM')
INSERT INTO [PromoCampaignHdr] ([PromoCampaignHdrID], [PromoCampaignName], [CampaignType], [DateFrom], [DateTo], [PromoPrice], [PromoDiscount], [FreeQty], [FreeItemNo], [Remark], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(16, 'Prata Day', 'DiscountByCategory', '22/5/2007 12:00:00 AM', '30/5/2007 12:00:00 AM', NULL, 4, NULL, NULL, NULL, NULL, '22/5/2007 1:27:56 PM', NULL, '22/5/2007 1:27:56 PM')
INSERT INTO [PromoCampaignHdr] ([PromoCampaignHdrID], [PromoCampaignName], [CampaignType], [DateFrom], [DateTo], [PromoPrice], [PromoDiscount], [FreeQty], [FreeItemNo], [Remark], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(17, 'Free Flow Night', 'DiscountByCategory', '22/5/2007 12:00:00 AM', '22/5/2007 12:00:00 AM', NULL, 15, NULL, NULL, NULL, NULL, '22/5/2007 1:28:43 PM', NULL, '22/5/2007 1:28:43 PM')
SET IDENTITY_INSERT [PromoCampaignHdr] OFF 
ALTER TABLE [PromoCampaignHdr] CHECK CONSTRAINT ALL
GO



ALTER TABLE [ScheduledDiscount] NOCHECK CONSTRAINT ALL
GO

SET IDENTITY_INSERT [ScheduledDiscount] ON 
PRINT 'Begin inserting data in ScheduledDiscount'
SET IDENTITY_INSERT [ScheduledDiscount] OFF 
ALTER TABLE [ScheduledDiscount] CHECK CONSTRAINT ALL
GO



ALTER TABLE [PointOfSale] NOCHECK CONSTRAINT ALL
GO

SET IDENTITY_INSERT [PointOfSale] ON 
PRINT 'Begin inserting data in PointOfSale'
INSERT INTO [PointOfSale] ([PointOfSaleID], [PointOfSaleName], [PointOfSaleDescription], [OutletName], [PhoneNo], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy])
VALUES(1, 'Web', 'Web Module', 'Web', NULL, NULL, NULL, NULL, NULL)
INSERT INTO [PointOfSale] ([PointOfSaleID], [PointOfSaleName], [PointOfSaleDescription], [OutletName], [PhoneNo], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy])
VALUES(2, 'Muthu PointOfSale', 'Sell Drink', 'Serangoon', '321221', '4/4/2007 9:52:16 AM', 'YOUR-F7C39A7C38\jenniferleet', '4/4/2007 9:52:16 AM', 'YOUR-F7C39A7C38\jenniferleet')
INSERT INTO [PointOfSale] ([PointOfSaleID], [PointOfSaleName], [PointOfSaleDescription], [OutletName], [PhoneNo], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy])
VALUES(3, 'Moshi Moshi', 'Sell Japanese food', 'Serangoon', NULL, '12/4/2007 5:03:43 AM', 'IMPACT-C491D6CC\Albert', '12/4/2007 5:04:06 AM', 'IMPACT-C491D6CC\Albert')
INSERT INTO [PointOfSale] ([PointOfSaleID], [PointOfSaleName], [PointOfSaleDescription], [OutletName], [PhoneNo], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy])
VALUES(4, 'FoodCourt', 'Sell Rice', 'Hougang', NULL, '16/4/2007 10:14:51 AM', 'IMPACT-C491D6CC\Albert', '16/4/2007 10:14:51 AM', 'IMPACT-C491D6CC\Albert')
SET IDENTITY_INSERT [PointOfSale] OFF 
ALTER TABLE [PointOfSale] CHECK CONSTRAINT ALL
GO



ALTER TABLE [Supplier] NOCHECK CONSTRAINT ALL
GO

SET IDENTITY_INSERT [Supplier] ON 
PRINT 'Begin inserting data in Supplier'
INSERT INTO [Supplier] ([SupplierID], [SupplierName], [CustomerAddress], [ShipToAddress], [BillToAddress], [ContactNo1], [ContactNo2], [ContactNo3], [ContactPerson1], [ContactPerson2], [ContactPerson3], [AccountNo])
VALUES(1, 'EdgeWorks', '10 Anson Road #01-02', '10 Anson Road #01-02', '10 Anson Road #01-02', '61234567', NULL, NULL, 'Albert Tirtohadi', NULL, NULL, '100-100-200-0')
INSERT INTO [Supplier] ([SupplierID], [SupplierName], [CustomerAddress], [ShipToAddress], [BillToAddress], [ContactNo1], [ContactNo2], [ContactNo3], [ContactPerson1], [ContactPerson2], [ContactPerson3], [AccountNo])
VALUES(2, 'Hong Wah Pte Ltd', '10 Kallang Pudding ', '112 Ubi Crescent #01-52', '10 Kallang Pudding', '64512552', '92348712', NULL, 'Ronnie Tan', 'David Tao', NULL, '15100-4561-22')
SET IDENTITY_INSERT [Supplier] OFF 
ALTER TABLE [Supplier] CHECK CONSTRAINT ALL
GO



ALTER TABLE [UserGroup] NOCHECK CONSTRAINT ALL
GO

SET IDENTITY_INSERT [UserGroup] ON 
PRINT 'Begin inserting data in UserGroup'
INSERT INTO [UserGroup] ([GroupID], [GroupName], [GroupDescription], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy])
VALUES(1, 'Cashier', 'handle cash registers', '8/4/2007 10:02:49 PM', 'IMPACT-C491D6CC\Albert', '8/4/2007 10:02:49 PM', 'IMPACT-C491D6CC\Albert')
INSERT INTO [UserGroup] ([GroupID], [GroupName], [GroupDescription], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy])
VALUES(2, 'Supervisor', 'PointOfSale supervisors', '8/4/2007 10:03:00 PM', 'IMPACT-C491D6CC\Albert', '8/4/2007 10:03:00 PM', 'IMPACT-C491D6CC\Albert')
INSERT INTO [UserGroup] ([GroupID], [GroupName], [GroupDescription], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy])
VALUES(3, 'Admin', 'Administrators', '8/4/2007 10:03:09 PM', 'IMPACT-C491D6CC\Albert', '8/4/2007 10:03:09 PM', 'IMPACT-C491D6CC\Albert')
INSERT INTO [UserGroup] ([GroupID], [GroupName], [GroupDescription], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy])
VALUES(4, 'test', 'test grp', '3/5/2007 9:58:44 PM', 'IMPACT-C491D6CC\Albert', '3/5/2007 9:58:44 PM', 'IMPACT-C491D6CC\Albert')
SET IDENTITY_INSERT [UserGroup] OFF 
ALTER TABLE [UserGroup] CHECK CONSTRAINT ALL
GO



ALTER TABLE [UserMst] NOCHECK CONSTRAINT ALL
GO

PRINT 'Begin inserting data in UserMst'
INSERT INTO [UserMst] ([UserName], [Password], [Remark], [GroupName], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('admin', '5gPNqhmBNpf/o5q8UUfhug==', 'primary administrator 1', 3, 'IMPACT-C491D6CC\Albert', '8/4/2007 10:03:46 PM', 'IMPACT-C491D6CC\Albert', '7/5/2007 2:18:29 AM')
INSERT INTO [UserMst] ([UserName], [Password], [Remark], [GroupName], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('admin2', '5gPNqhmBNpf/o5q8UUfhug==', 'administrator2', 3, 'IMPACT-C491D6CC\Albert', '7/5/2007 2:14:08 AM', 'IMPACT-C491D6CC\Albert', '7/5/2007 2:14:08 AM')
INSERT INTO [UserMst] ([UserName], [Password], [Remark], [GroupName], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('alberto', 'Qxju/Rtb9/pq9z8rcKlyBA==', '123', 3, 'IMPACT-C491D6CC\Albert', '23/4/2007 6:18:34 PM', 'IMPACT-C491D6CC\Albert', '7/5/2007 2:19:06 AM')
INSERT INTO [UserMst] ([UserName], [Password], [Remark], [GroupName], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('cashier1', '5gPNqhmBNpf/o5q8UUfhug==', 'first cashier', 4, 'IMPACT-C491D6CC\Albert', '8/4/2007 10:04:09 PM', 'IMPACT-C491D6CC\Albert', '7/5/2007 2:18:48 AM')
INSERT INTO [UserMst] ([UserName], [Password], [Remark], [GroupName], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('cashier2', 'xfh4rSLBqa0FkFU1TwMr+G0PCVzeDGHZX3yIM7s42iQ=', NULL, 1, 'IMPACT-C491D6CC\Albert', '15/4/2007 9:36:11 PM', 'IMPACT-C491D6CC\Albert', '7/5/2007 2:23:37 AM')
INSERT INTO [UserMst] ([UserName], [Password], [Remark], [GroupName], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('cashier3', 'DzhYiwU72pQMy/mTEmm+Yg==', 'this is another cashier 3', 1, 'IMPACT-C491D6CC\Albert', '16/4/2007 6:24:45 AM', 'IMPACT-C491D6CC\Albert', '7/5/2007 2:20:21 AM')
INSERT INTO [UserMst] ([UserName], [Password], [Remark], [GroupName], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('supervisor1', '5gPNqhmBNpf/o5q8UUfhug==', 'PointOfSale supervisor1', 2, 'IMPACT-C491D6CC\Albert', '8/4/2007 10:04:25 PM', 'IMPACT-C491D6CC\Albert', '7/5/2007 2:25:06 AM')
INSERT INTO [UserMst] ([UserName], [Password], [Remark], [GroupName], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('supervisor2', '5gPNqhmBNpf/o5q8UUfhug==', NULL, 2, 'IMPACT-C491D6CC\Albert', '15/4/2007 9:36:29 PM', 'IMPACT-C491D6CC\Albert', '7/5/2007 2:30:21 AM')
ALTER TABLE [UserMst] CHECK CONSTRAINT ALL
GO



ALTER TABLE [UserPrivilege] NOCHECK CONSTRAINT ALL
GO

SET IDENTITY_INSERT [UserPrivilege] ON 
PRINT 'Begin inserting data in UserPrivilege'
INSERT INTO [UserPrivilege] ([UserPrivilegeID], [PrivilegeName], [FormName], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(1, 'CashBill', NULL, 'IMPACT-C491D6CC\Albert', '2/5/2007 4:44:41 PM', 'IMPACT-C491D6CC\Albert', '3/5/2007 9:27:39 PM')
INSERT INTO [UserPrivilege] ([UserPrivilegeID], [PrivilegeName], [FormName], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(2, 'EditBill', NULL, 'IMPACT-C491D6CC\Albert', '2/5/2007 4:45:24 PM', 'IMPACT-C491D6CC\Albert', '3/5/2007 9:27:44 PM')
INSERT INTO [UserPrivilege] ([UserPrivilegeID], [PrivilegeName], [FormName], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(3, 'InventoryTransaction', NULL, 'IMPACT-C491D6CC\Albert', '2/5/2007 4:45:46 PM', 'IMPACT-C491D6CC\Albert', '3/5/2007 9:27:56 PM')
INSERT INTO [UserPrivilege] ([UserPrivilegeID], [PrivilegeName], [FormName], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(4, 'EditInventory', NULL, 'IMPACT-C491D6CC\Albert', '2/5/2007 4:46:00 PM', 'IMPACT-C491D6CC\Albert', '3/5/2007 9:28:01 PM')
INSERT INTO [UserPrivilege] ([UserPrivilegeID], [PrivilegeName], [FormName], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(5, 'ProductSalesReport', 'ProductSalesReport.aspx', 'IMPACT-C491D6CC\Albert', '2/5/2007 4:46:19 PM', 'IMPACT-C491D6CC\Albert', '3/5/2007 9:29:02 PM')
INSERT INTO [UserPrivilege] ([UserPrivilegeID], [PrivilegeName], [FormName], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(6, 'InventoryTransactionReport', 'InventoryTransactionReport.aspx', 'IMPACT-C491D6CC\Albert', '2/5/2007 4:46:28 PM', 'IMPACT-C491D6CC\Albert', '3/5/2007 9:28:56 PM')
INSERT INTO [UserPrivilege] ([UserPrivilegeID], [PrivilegeName], [FormName], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(7, 'TransactionReport', 'TransactionReport.aspx', 'IMPACT-C491D6CC\Albert', '2/5/2007 4:46:44 PM', 'IMPACT-C491D6CC\Albert', '3/5/2007 9:29:18 PM')
INSERT INTO [UserPrivilege] ([UserPrivilegeID], [PrivilegeName], [FormName], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(8, 'CashRecordingReport', 'CashRecordingReport.aspx', 'IMPACT-C491D6CC\Albert', '2/5/2007 4:47:18 PM', 'IMPACT-C491D6CC\Albert', '3/5/2007 9:29:50 PM')
INSERT INTO [UserPrivilege] ([UserPrivilegeID], [PrivilegeName], [FormName], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(9, 'CashRecording', NULL, 'IMPACT-C491D6CC\Albert', '2/5/2007 4:50:04 PM', 'IMPACT-C491D6CC\Albert', '3/5/2007 9:29:59 PM')
INSERT INTO [UserPrivilege] ([UserPrivilegeID], [PrivilegeName], [FormName], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(10, 'CloseCounter', NULL, 'IMPACT-C491D6CC\Albert', '2/5/2007 4:50:12 PM', 'IMPACT-C491D6CC\Albert', '3/5/2007 9:30:39 PM')
INSERT INTO [UserPrivilege] ([UserPrivilegeID], [PrivilegeName], [FormName], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(11, 'ChangeUnitPrice', NULL, 'IMPACT-C491D6CC\Albert', '2/5/2007 4:54:14 PM', 'IMPACT-C491D6CC\Albert', '3/5/2007 9:30:47 PM')
INSERT INTO [UserPrivilege] ([UserPrivilegeID], [PrivilegeName], [FormName], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(12, 'VoidBill', NULL, 'IMPACT-C491D6CC\Albert', '2/5/2007 4:54:31 PM', 'IMPACT-C491D6CC\Albert', '3/5/2007 9:30:53 PM')
INSERT INTO [UserPrivilege] ([UserPrivilegeID], [PrivilegeName], [FormName], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(13, 'CategorySalesReport', 'CategorySalesReport.aspx', 'IMPACT-C491D6CC\Albert', '3/5/2007 9:32:27 PM', 'IMPACT-C491D6CC\Albert', '3/5/2007 9:32:27 PM')
INSERT INTO [UserPrivilege] ([UserPrivilegeID], [PrivilegeName], [FormName], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(14, 'CounterCloseLogReport', 'CounterCloseLogReport.aspx', 'IMPACT-C491D6CC\Albert', '3/5/2007 9:32:50 PM', 'IMPACT-C491D6CC\Albert', '3/5/2007 9:32:50 PM')
INSERT INTO [UserPrivilege] ([UserPrivilegeID], [PrivilegeName], [FormName], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(15, 'LoginReport', 'LoginReport.aspx', 'IMPACT-C491D6CC\Albert', '3/5/2007 9:33:10 PM', 'IMPACT-C491D6CC\Albert', '3/5/2007 9:33:10 PM')
INSERT INTO [UserPrivilege] ([UserPrivilegeID], [PrivilegeName], [FormName], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(16, 'StockOnHandReport', 'StockOnHandReport.aspx', 'IMPACT-C491D6CC\Albert', '3/5/2007 9:33:31 PM', 'IMPACT-C491D6CC\Albert', '3/5/2007 9:33:31 PM')
INSERT INTO [UserPrivilege] ([UserPrivilegeID], [PrivilegeName], [FormName], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(17, 'Camera Set Up', 'CameraScaffold.aspx', 'IMPACT-C491D6CC\Albert', '3/5/2007 9:34:34 PM', 'IMPACT-C491D6CC\Albert', '4/5/2007 1:49:04 PM')
INSERT INTO [UserPrivilege] ([UserPrivilegeID], [PrivilegeName], [FormName], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(18, 'Group Setup', 'GroupScaffold.aspx', 'IMPACT-C491D6CC\Albert', '3/5/2007 9:35:17 PM', 'IMPACT-C491D6CC\Albert', '4/5/2007 1:50:51 PM')
INSERT INTO [UserPrivilege] ([UserPrivilegeID], [PrivilegeName], [FormName], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(19, 'Outlet Setup', 'OutletScaffold.aspx', 'IMPACT-C491D6CC\Albert', '3/5/2007 9:36:00 PM', 'IMPACT-C491D6CC\Albert', '4/5/2007 1:50:58 PM')
INSERT INTO [UserPrivilege] ([UserPrivilegeID], [PrivilegeName], [FormName], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(20, 'Privilege Setup', 'PrivilegeScaffold.aspx', 'IMPACT-C491D6CC\Albert', '3/5/2007 9:36:22 PM', 'IMPACT-C491D6CC\Albert', '4/5/2007 1:51:05 PM')
INSERT INTO [UserPrivilege] ([UserPrivilegeID], [PrivilegeName], [FormName], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(21, 'Promotion Setup', 'PromotionScaffold.aspx', 'IMPACT-C491D6CC\Albert', '3/5/2007 9:36:55 PM', 'IMPACT-C491D6CC\Albert', '4/5/2007 1:51:15 PM')
INSERT INTO [UserPrivilege] ([UserPrivilegeID], [PrivilegeName], [FormName], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(22, 'PointOfSale Setup', 'PointOfSaleScaffold.aspx', 'IMPACT-C491D6CC\Albert', '3/5/2007 9:37:30 PM', 'IMPACT-C491D6CC\Albert', '4/5/2007 1:51:24 PM')
INSERT INTO [UserPrivilege] ([UserPrivilegeID], [PrivilegeName], [FormName], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(23, 'Assign Privilege', 'UserGroupPrivilege.aspx', 'IMPACT-C491D6CC\Albert', '3/5/2007 9:39:17 PM', 'IMPACT-C491D6CC\Albert', '4/5/2007 1:51:37 PM')
INSERT INTO [UserPrivilege] ([UserPrivilegeID], [PrivilegeName], [FormName], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(24, 'User Setup', 'UserMstscaffold.aspx', 'IMPACT-C491D6CC\Albert', '3/5/2007 9:39:39 PM', 'IMPACT-C491D6CC\Albert', '4/5/2007 1:51:46 PM')
INSERT INTO [UserPrivilege] ([UserPrivilegeID], [PrivilegeName], [FormName], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(25, 'ViewPastBill', NULL, 'IMPACT-C491D6CC\Albert', '4/5/2007 12:24:16 PM', 'IMPACT-C491D6CC\Albert', '4/5/2007 12:24:16 PM')
INSERT INTO [UserPrivilege] ([UserPrivilegeID], [PrivilegeName], [FormName], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(26, 'RePrintBill', NULL, 'IMPACT-C491D6CC\Albert', '4/5/2007 12:24:36 PM', 'IMPACT-C491D6CC\Albert', '4/5/2007 12:24:36 PM')
INSERT INTO [UserPrivilege] ([UserPrivilegeID], [PrivilegeName], [FormName], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(27, 'GiveDiscount', NULL, 'IMPACT-C491D6CC\Albert', '4/5/2007 12:26:25 PM', 'IMPACT-C491D6CC\Albert', '4/5/2007 12:26:25 PM')
INSERT INTO [UserPrivilege] ([UserPrivilegeID], [PrivilegeName], [FormName], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(28, 'ApplyTax', NULL, 'IMPACT-C491D6CC\Albert', '4/5/2007 12:26:40 PM', 'IMPACT-C491D6CC\Albert', '4/5/2007 12:26:40 PM')
INSERT INTO [UserPrivilege] ([UserPrivilegeID], [PrivilegeName], [FormName], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(29, 'ProductSetup', NULL, 'IMPACT-C491D6CC\Albert', '4/5/2007 1:35:52 PM', 'IMPACT-C491D6CC\Albert', '4/5/2007 1:36:13 PM')
INSERT INTO [UserPrivilege] ([UserPrivilegeID], [PrivilegeName], [FormName], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(30, 'CollectionReport', 'CollectionReport.aspx', 'IMPACT-C491D6CC\Albert', '6/5/2007 10:17:40 PM', 'IMPACT-C491D6CC\Albert', '6/5/2007 10:17:40 PM')
SET IDENTITY_INSERT [UserPrivilege] OFF 
ALTER TABLE [UserPrivilege] CHECK CONSTRAINT ALL
GO



ALTER TABLE [Camera] NOCHECK CONSTRAINT ALL
GO

SET IDENTITY_INSERT [Camera] ON 
PRINT 'Begin inserting data in Camera'
INSERT INTO [Camera] ([CameraId], [CameraIp], [CameraNo], [PointOfSaleID], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy])
VALUES(4, 'localhost', 0, 3, '23/4/2007 10:51:13 PM', 'IMPACT-C491D6CC\Albert', '23/4/2007 10:51:13 PM', 'IMPACT-C491D6CC\Albert')
INSERT INTO [Camera] ([CameraId], [CameraIp], [CameraNo], [PointOfSaleID], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy])
VALUES(5, 'locahost', 0, 2, '23/4/2007 10:51:21 PM', 'IMPACT-C491D6CC\Albert', '23/4/2007 10:51:21 PM', 'IMPACT-C491D6CC\Albert')
INSERT INTO [Camera] ([CameraId], [CameraIp], [CameraNo], [PointOfSaleID], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy])
VALUES(8, 'localhost', 1, 3, '26/4/2007 1:14:17 AM', 'IMPACT-C491D6CC\Albert', '26/4/2007 1:14:17 AM', 'IMPACT-C491D6CC\Albert')
SET IDENTITY_INSERT [Camera] OFF 
ALTER TABLE [Camera] CHECK CONSTRAINT ALL
GO



ALTER TABLE [CashRecording] NOCHECK CONSTRAINT ALL
GO

PRINT 'Begin inserting data in CashRecording'
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07041300030001', '13/4/2007 2:17:18 AM', 12000.0000, 1, 3, 'cashier1', 'supervisor1', '', '13/4/2007 2:17:18 AM', 'cashier1', '13/4/2007 2:17:18 AM', 'cashier1', 'fb81cd7f-53a9-4b1b-a565-f3ae04f58b51')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07041300030002', '13/4/2007 8:56:06 AM', 1000.0000, 3, 3, 'cashier1', 'supervisor1', '', '13/4/2007 8:56:06 AM', 'cashier1', '13/4/2007 8:56:06 AM', 'cashier1', 'a2fd6f62-6643-482d-85c7-f83c2147dba6')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07041500030001', '15/4/2007 2:49:01 PM', 2500.0000, 3, 3, 'cashier1', 'supervisor1', '', '15/4/2007 2:49:01 PM', 'cashier1', '15/4/2007 2:49:01 PM', 'cashier1', '306f1b6c-e819-48a3-8ac9-7269e9dc6482')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07041500030002', '15/4/2007 2:51:34 PM', 300.0000, 1, 3, 'cashier1', 'supervisor1', '', '15/4/2007 2:51:34 PM', 'cashier1', '15/4/2007 2:51:34 PM', 'cashier1', '7a1bddfd-499a-444c-af07-f340de87bf81')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07041500030003', '15/4/2007 3:00:39 PM', 1500.0000, 2, 3, 'cashier1', 'supervisor1', '', '15/4/2007 3:00:39 PM', 'cashier1', '15/4/2007 3:00:39 PM', 'cashier1', '9a42da21-c8bf-4380-80a4-efe91fcb1053')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07041500030004', '15/4/2007 3:02:28 PM', 1331.2000, 4, 3, 'cashier1', 'supervisor1', NULL, '15/4/2007 3:03:24 PM', 'cashier1', '15/4/2007 3:03:24 PM', 'cashier1', '269e8b6c-c1cf-4b3d-9ad1-a0a2a2f48908')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07041500030005', '15/4/2007 8:32:11 PM', 1394.9500, 4, 3, 'supervisor1', 'supervisor1', NULL, '15/4/2007 8:32:11 PM', 'supervisor1', '15/4/2007 8:32:11 PM', 'supervisor1', '95e1a39a-7af6-4f3f-9fae-3d612bce97c9')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07041500030006', '15/4/2007 8:53:28 PM', 66.4500, 4, 3, 'cashier1', 'supervisor1', NULL, '15/4/2007 8:53:28 PM', 'cashier1', '15/4/2007 8:53:28 PM', 'cashier1', '9fb2800b-4669-434a-8318-9c0cc1310a08')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07041500030007', '15/4/2007 9:42:45 PM', 1500.0000, 3, 3, 'cashier2', 'supervisor1', 'testing', '15/4/2007 9:42:45 PM', 'cashier2', '15/4/2007 9:42:45 PM', 'cashier2', 'db6a8980-66a0-40f7-94e2-18bb45d4c5d3')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07041500030008', '15/4/2007 9:44:08 PM', 500.0000, 1, 3, 'cashier2', 'supervisor2', '', '15/4/2007 9:44:08 PM', 'cashier2', '15/4/2007 9:44:08 PM', 'cashier2', 'd28ac8b7-60ee-4a0b-a8f7-74a6245096c7')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07041500030009', '15/4/2007 9:45:33 PM', 600.0000, 2, 3, 'cashier2', 'supervisor2', '', '15/4/2007 9:45:33 PM', 'cashier2', '15/4/2007 9:45:33 PM', 'cashier2', 'fc34f756-9e1f-47af-9ab3-e38e3bb00fed')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07041600020001', '16/4/2007 10:04:21 AM', 1500.0000, 3, 2, 'cashier1', 'supervisor2', '', '16/4/2007 10:04:21 AM', 'cashier1', '16/4/2007 10:04:21 AM', 'cashier1', '9e6ce803-81ce-466a-a20c-a6b45db4c7c4')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07041600020002', '16/4/2007 10:04:35 AM', 500.0000, 2, 2, 'cashier1', 'supervisor1', '', '16/4/2007 10:04:35 AM', 'cashier1', '16/4/2007 10:04:35 AM', 'cashier1', 'cb3093b6-d52a-4de4-9de4-fb44bdca85fd')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07041600020003', '16/4/2007 10:10:47 AM', 1076.6000, 4, 2, 'cashier1', 'supervisor1', NULL, '16/4/2007 10:10:48 AM', 'cashier1', '16/4/2007 10:10:48 AM', 'cashier1', '61f3870b-2b77-4b5f-823f-36fa33a424c0')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07041700020001', '17/4/2007 6:14:20 AM', 20.3000, 4, 2, 'cashier1', 'supervisor1', NULL, '17/4/2007 6:14:20 AM', 'cashier1', '17/4/2007 6:14:20 AM', 'cashier1', '869cd3dd-d395-4751-af37-c41ad3628f3e')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07041700020002', '17/4/2007 6:22:18 AM', 20.3000, 4, 2, 'cashier1', 'supervisor1', NULL, '17/4/2007 6:22:18 AM', 'cashier1', '17/4/2007 6:22:18 AM', 'cashier1', '19122dc3-12df-41c2-883c-abdb041b8744')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07041700020003', '17/4/2007 6:24:53 AM', 20.3000, 4, 2, 'cashier1', 'supervisor1', NULL, '17/4/2007 6:24:53 AM', 'cashier1', '17/4/2007 6:24:53 AM', 'cashier1', 'abf08b2c-e0cd-4c03-8513-9f3b4a21545e')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07041700020004', '17/4/2007 6:27:48 AM', 20.3000, 4, 2, 'cashier1', 'supervisor1', NULL, '17/4/2007 6:27:48 AM', 'cashier1', '17/4/2007 6:27:48 AM', 'cashier1', '732cd22d-cb09-4e3d-ae47-8e08404d810f')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07041700020005', '17/4/2007 6:52:39 AM', 1139.5500, 4, 2, 'cashier1', 'supervisor1', NULL, '17/4/2007 6:52:39 AM', 'cashier1', '17/4/2007 6:52:39 AM', 'cashier1', 'd4bd5e39-5881-4aab-9fcb-0d943031cb89')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07041700020006', '17/4/2007 6:53:36 AM', 1139.5500, 4, 2, 'cashier1', 'supervisor1', NULL, '17/4/2007 6:53:36 AM', 'cashier1', '17/4/2007 6:53:36 AM', 'cashier1', '4bf10b67-dad0-40d1-8988-03fbf863e9bd')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07041700020007', '17/4/2007 5:02:07 PM', 55.6000, 4, 2, 'cashier1', 'supervisor1', NULL, '17/4/2007 5:02:07 PM', 'cashier1', '17/4/2007 5:02:07 PM', 'cashier1', 'b51cd1d6-b6f5-4d8c-a815-5a84cdd52d11')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07042100020001', '21/4/2007 3:54:23 PM', 1361.2000, 4, 2, 'cashier1', 'supervisor1', NULL, '21/4/2007 3:54:23 PM', 'cashier1', '21/4/2007 3:54:23 PM', 'cashier1', '55cf228a-e7f0-48b2-92fe-b994d236ab89')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07042300020001', '23/4/2007 5:09:08 PM', 1200.0000, 1, 2, 'cashier1', 'supervisor1', '', '23/4/2007 5:09:08 PM', 'cashier1', '23/4/2007 5:09:08 PM', 'cashier1', 'efd5ff48-725b-4ab9-be1c-d30415a735c6')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07042300020002', '23/4/2007 5:09:43 PM', 1216.0500, 4, 2, 'cashier1', 'supervisor1', NULL, '23/4/2007 5:09:43 PM', 'cashier1', '23/4/2007 5:09:43 PM', 'cashier1', 'd87d01f0-4624-4c4f-b985-cd663d58fc72')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07042500030001', '25/4/2007 9:01:05 PM', 5000.0000, 1, 3, 'cashier1', 'supervisor1', '', '25/4/2007 9:01:05 PM', 'cashier1', '25/4/2007 9:01:05 PM', 'cashier1', 'bf473b92-2599-4014-800f-9a1fe7a820f6')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07043000020001', '30/4/2007 1:47:18 PM', 7.1000, 4, 2, 'cashier1', 'supervisor1', NULL, '30/4/2007 1:47:18 PM', 'cashier1', '30/4/2007 1:47:18 PM', 'cashier1', 'd3444043-5c6f-4053-849e-1a9249b4c359')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07043000020002', '30/4/2007 2:08:23 PM', 0.0000, 4, 2, 'cashier1', 'supervisor1', NULL, '30/4/2007 2:08:23 PM', 'cashier1', '30/4/2007 2:08:23 PM', 'cashier1', '7e65266a-8919-4447-a0e6-8d7ab6f9eef3')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07043000020003', '30/4/2007 2:26:31 PM', 100.0000, 3, 2, 'cashier1', 'supervisor1', '', '30/4/2007 2:26:31 PM', 'cashier1', '30/4/2007 2:26:31 PM', 'cashier1', '924ef732-28cb-452f-bf7c-063fa77723f8')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07043000020004', '30/4/2007 2:26:43 PM', 20.0000, 1, 2, 'cashier1', 'supervisor1', '', '30/4/2007 2:26:43 PM', 'cashier1', '30/4/2007 2:26:43 PM', 'cashier1', '6b5231e1-f139-4dbe-8ea6-6ecceaa00b7a')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07043000020005', '30/4/2007 2:26:59 PM', 10.0000, 2, 2, 'cashier1', 'supervisor1', '', '30/4/2007 2:26:59 PM', 'cashier1', '30/4/2007 2:26:59 PM', 'cashier1', '6d8c8b68-7388-4097-9136-6102ed316b0d')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07043000020006', '30/4/2007 2:27:30 PM', 117.8000, 4, 2, 'cashier1', 'supervisor1', NULL, '30/4/2007 2:27:30 PM', 'cashier1', '30/4/2007 2:27:30 PM', 'cashier1', '7ee26c44-5ab0-4e3d-a4da-a1bf39e6738b')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07051400020001', '14/5/2007 5:12:04 PM', 3000.0000, 1, 2, 'admin', 'admin2', '', '14/5/2007 5:12:04 PM', 'admin', '14/5/2007 5:12:04 PM', 'admin', '0d2b7b87-ead4-4785-877c-a1fc6fa13946')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07052000020001', '20/5/2007 11:07:54 PM', 1888.0000, 3, 2, 'cashier1', 'cashier1', '', '20/5/2007 11:07:55 PM', 'cashier1', '20/5/2007 11:07:55 PM', 'cashier1', 'e832f4c4-d562-4996-bb92-2cd29cb4f6e1')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07052100020001', '21/5/2007 10:55:18 AM', 510.0000, 3, 2, 'admin', 'admin', '', '21/5/2007 10:55:18 AM', 'admin', '21/5/2007 10:55:18 AM', 'admin', 'ff0595f3-4f75-49dd-8ea3-f67e409fc466')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07052100020002', '21/5/2007 10:59:36 AM', 0.0000, 3, 2, 'cashier1', 'cashier1', '', '21/5/2007 10:59:36 AM', 'cashier1', '21/5/2007 10:59:36 AM', 'cashier1', 'add36f04-4c97-4655-8fe1-ea7481d77c39')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07052100020003', '21/5/2007 11:01:52 AM', 0.0000, 3, 2, 'admin', 'admin', '', '21/5/2007 11:01:52 AM', 'admin', '21/5/2007 11:01:52 AM', 'admin', 'c445b99c-2832-4d37-95aa-24fb66bbe019')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07052100020004', '21/5/2007 11:04:24 AM', 200.0000, 3, 2, 'admin', 'admin', '', '21/5/2007 11:04:24 AM', 'admin', '21/5/2007 11:04:24 AM', 'admin', '6fcb9b57-c9c7-416a-a184-c856eb403bff')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07052100020005', '21/5/2007 12:24:54 PM', 500.0000, 3, 2, 'admin', 'admin', '', '21/5/2007 12:24:55 PM', 'admin', '21/5/2007 12:24:55 PM', 'admin', 'a7aa2637-d6dd-43da-a9ab-e25df93708be')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07052100020006', '21/5/2007 12:31:30 PM', 2000.0000, 3, 2, 'cashier1', 'cashier1', '', '21/5/2007 12:31:30 PM', 'cashier1', '21/5/2007 12:31:30 PM', 'cashier1', '2a16c57d-79f6-452b-836b-d2c2e0e542a0')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07052100020007', '21/5/2007 12:33:02 PM', 200.0000, 3, 2, 'admin', 'admin', '', '21/5/2007 12:33:02 PM', 'admin', '21/5/2007 12:33:02 PM', 'admin', 'c8311c00-41c6-469f-a221-d4f799ff6fb5')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07052100020008', '21/5/2007 12:38:37 PM', 0.0000, 3, 2, 'cashier1', 'cashier1', '', '21/5/2007 12:38:37 PM', 'cashier1', '21/5/2007 12:38:37 PM', 'cashier1', '158a8d89-55fa-4e27-afd9-2fb031cc116b')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07052100020009', '21/5/2007 12:38:50 PM', 0.0000, 3, 2, 'admin', 'admin', '', '21/5/2007 12:38:51 PM', 'admin', '21/5/2007 12:38:51 PM', 'admin', 'a048f0c5-6afb-4b9a-b0ea-83fabcbab7d4')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07052100020010', '21/5/2007 12:41:01 PM', 0.0000, 3, 2, 'admin', 'admin', '', '21/5/2007 12:41:01 PM', 'admin', '21/5/2007 12:41:01 PM', 'admin', '272957ab-5113-49fe-8c15-563efffdc3c7')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07052100020011', '21/5/2007 12:42:18 PM', 0.0000, 3, 2, 'admin', 'admin', '', '21/5/2007 12:42:18 PM', 'admin', '21/5/2007 12:42:18 PM', 'admin', '7af4e89e-b263-498a-bab1-f662d431a605')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07052100020012', '21/5/2007 12:43:49 PM', 300.0000, 3, 2, 'admin', 'admin', '', '21/5/2007 12:43:49 PM', 'admin', '21/5/2007 12:43:49 PM', 'admin', '1691c540-ada8-461e-9119-e90b9ca32587')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07052100020013', '21/5/2007 12:46:50 PM', 300.0000, 3, 2, 'admin', 'admin', '', '21/5/2007 12:46:50 PM', 'admin', '21/5/2007 12:46:50 PM', 'admin', '2c75d3c7-8e6c-429c-b811-30cee0c4be40')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07052100020014', '21/5/2007 12:52:42 PM', 300.0000, 3, 2, 'admin', 'admin', '', '21/5/2007 12:52:42 PM', 'admin', '21/5/2007 12:52:42 PM', 'admin', 'f2381aa6-f04a-4085-a6ca-39a1dc3cc821')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07052100020015', '21/5/2007 1:00:46 PM', 300.0000, 3, 2, 'admin', 'admin', '', '21/5/2007 1:00:46 PM', 'admin', '21/5/2007 1:00:46 PM', 'admin', 'd0573ffe-1fe2-45ad-97b5-c69ae9eb5190')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07052100020016', '21/5/2007 1:07:58 PM', 3000.0000, 3, 2, 'admin', 'admin', '', '21/5/2007 1:07:59 PM', 'admin', '21/5/2007 1:07:59 PM', 'admin', '3e32570d-2b74-48b2-9f3e-f9f0b80e24a9')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07052100020017', '21/5/2007 1:10:21 PM', 350.0000, 3, 2, 'admin', 'admin', '', '21/5/2007 1:10:21 PM', 'admin', '21/5/2007 1:10:21 PM', 'admin', 'fd654bf0-58bc-46f4-9aa1-ab7a2dbe3c7a')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07052100020018', '21/5/2007 1:11:02 PM', 0.0000, 3, 2, 'admin', 'admin', '', '21/5/2007 1:11:02 PM', 'admin', '21/5/2007 1:11:02 PM', 'admin', 'a0c82a8c-2950-4317-b94a-8b047cde7fec')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07052100020019', '21/5/2007 1:18:18 PM', 0.0000, 3, 2, 'admin', 'admin', '', '21/5/2007 1:18:18 PM', 'admin', '21/5/2007 1:18:18 PM', 'admin', 'd01bdf8c-5b5a-48c5-a34c-5a21f0385618')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07052100020020', '21/5/2007 1:21:24 PM', 3310.9000, 4, 2, 'admin', 'admin2', NULL, '21/5/2007 1:21:24 PM', 'admin', '21/5/2007 1:21:24 PM', 'admin', '50044ebb-83ea-44a4-9032-5ac7ac04fefa')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07052100020021', '21/5/2007 1:22:20 PM', 300.0000, 3, 2, 'admin', 'admin', '', '21/5/2007 1:22:20 PM', 'admin', '21/5/2007 1:22:20 PM', 'admin', '1a80a4fa-c24f-472b-9e76-42cf37d2d825')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07052100020022', '21/5/2007 1:22:55 PM', 10.0000, 1, 2, 'admin', 'supervisor1', '', '21/5/2007 1:22:55 PM', 'admin', '21/5/2007 1:22:55 PM', 'admin', 'dcb0029d-0c8d-4930-a0c9-76a22743a6a0')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07052100020023', '21/5/2007 1:23:10 PM', 2.0000, 2, 2, 'admin', 'supervisor1', '', '21/5/2007 1:23:10 PM', 'admin', '21/5/2007 1:23:10 PM', 'admin', '73fa5915-e15c-4840-b574-6b85ef482dcb')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07052100020024', '21/5/2007 1:24:18 PM', 12.0000, 4, 2, 'admin', 'supervisor1', NULL, '21/5/2007 1:24:18 PM', 'admin', '21/5/2007 1:24:18 PM', 'admin', '0a63cc4f-6528-4395-b646-321dbe725c1a')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07052100020025', '21/5/2007 1:34:57 PM', 350.0000, 3, 2, 'admin', 'admin', '', '21/5/2007 1:34:57 PM', 'admin', '21/5/2007 1:34:57 PM', 'admin', 'a2afccfc-cee3-4995-8524-a7bd7a061d4c')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07052100020026', '21/5/2007 3:03:15 PM', 0.0000, 3, 2, 'admin', 'admin', '', '21/5/2007 3:03:15 PM', 'admin', '21/5/2007 3:03:15 PM', 'admin', '411df989-d550-47a4-9b57-db4cf7f11852')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07052100020027', '21/5/2007 3:08:06 PM', 0.0000, 3, 2, 'admin', 'admin', '', '21/5/2007 3:08:06 PM', 'admin', '21/5/2007 3:08:06 PM', 'admin', 'b716f69a-47eb-4cb1-8c2e-3cfaa8da24bc')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07052100020028', '21/5/2007 3:11:30 PM', 0.0000, 3, 2, 'admin', 'admin', '', '21/5/2007 3:11:30 PM', 'admin', '21/5/2007 3:11:30 PM', 'admin', 'ddd34a12-5e3a-40c6-af67-531adc6d33ac')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07052100020029', '21/5/2007 3:15:39 PM', 0.0000, 3, 2, 'admin', 'admin', '', '21/5/2007 3:15:39 PM', 'admin', '21/5/2007 3:15:39 PM', 'admin', '95543f18-aefd-4b5c-81a2-2a35a21dc67a')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07052100020030', '21/5/2007 3:19:10 PM', 0.0000, 3, 2, 'admin', 'admin', '', '21/5/2007 3:19:10 PM', 'admin', '21/5/2007 3:19:10 PM', 'admin', 'daf50568-4e22-44a1-b370-8a38e7370587')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07052100020031', '21/5/2007 3:21:00 PM', 0.0000, 3, 2, 'admin', 'admin', '', '21/5/2007 3:21:01 PM', 'admin', '21/5/2007 3:21:01 PM', 'admin', '9c43fbe1-ae3e-4062-9d23-dbbff0dfd0fc')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07052100020032', '21/5/2007 3:23:52 PM', 0.0000, 3, 2, 'admin', 'admin', '', '21/5/2007 3:23:52 PM', 'admin', '21/5/2007 3:23:52 PM', 'admin', 'a6705699-c272-494b-adc1-8824d1b299f5')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07052100020033', '21/5/2007 3:30:35 PM', 0.0000, 3, 2, 'admin', 'admin', '', '21/5/2007 3:30:35 PM', 'admin', '21/5/2007 3:30:35 PM', 'admin', '7207c324-ea69-44f3-aedc-e82e4d8fcb49')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07052100020034', '21/5/2007 3:31:05 PM', 0.0000, 3, 2, 'admin', 'admin', '', '21/5/2007 3:31:05 PM', 'admin', '21/5/2007 3:31:05 PM', 'admin', 'b4c3a632-80d0-4fda-8745-e735c4af6b2c')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07052100020035', '21/5/2007 3:33:52 PM', 0.0000, 3, 2, 'admin', 'admin', '', '21/5/2007 3:33:52 PM', 'admin', '21/5/2007 3:33:52 PM', 'admin', '4f952a6d-a266-4938-8c98-4426716bb394')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07052100020036', '21/5/2007 3:39:06 PM', 200.0000, 3, 2, 'admin', 'admin', '', '21/5/2007 3:39:07 PM', 'admin', '21/5/2007 3:39:07 PM', 'admin', '3f3c0c8a-80fd-4225-8edd-8a712d7e4716')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07052100020037', '21/5/2007 3:41:23 PM', 91.2500, 4, 2, 'admin', 'supervisor1', NULL, '21/5/2007 3:41:23 PM', 'admin', '21/5/2007 3:41:23 PM', 'admin', '6e1fb782-3580-4c47-909f-3ff10237e5e5')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07052100020038', '21/5/2007 5:35:12 PM', 0.0000, 3, 2, 'admin', 'admin', '', '21/5/2007 5:35:13 PM', 'admin', '21/5/2007 5:35:13 PM', 'admin', '918f14f5-a1ae-4ad2-9840-d1e6dc56a6b4')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07052100020039', '21/5/2007 5:36:18 PM', 3757.7500, 4, 2, 'admin', 'supervisor1', NULL, '21/5/2007 5:36:18 PM', 'admin', '21/5/2007 5:36:18 PM', 'admin', '6ae686f2-e77e-48bf-af49-f97ea30d24d4')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07052100020040', '21/5/2007 5:47:51 PM', 0.0000, 3, 2, 'admin', 'admin', '', '21/5/2007 5:47:51 PM', 'admin', '21/5/2007 5:47:51 PM', 'admin', '7ba9484c-1b19-44b1-9e65-30ae32577a67')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07052100020041', '21/5/2007 5:48:58 PM', 6.0000, 4, 2, 'admin', 'supervisor1', NULL, '21/5/2007 5:48:58 PM', 'admin', '21/5/2007 5:48:58 PM', 'admin', '9a198450-7649-4314-bcfb-27058d2c3e98')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07052100020042', '21/5/2007 11:19:16 PM', 0.0000, 3, 2, 'admin', 'admin', '', '21/5/2007 11:19:16 PM', 'admin', '21/5/2007 11:19:16 PM', 'admin', '2899ff26-49c2-417a-b060-82f6b3c21ad8')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07052100020043', '21/5/2007 11:19:54 PM', 0.0000, 3, 2, 'admin', 'admin', '', '21/5/2007 11:19:54 PM', 'admin', '21/5/2007 11:19:54 PM', 'admin', '9d996bdb-dc01-4aca-b02b-5ef097bad5c8')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07052100020044', '21/5/2007 11:46:59 PM', 0.0000, 3, 2, 'admin', 'admin', '', '21/5/2007 11:46:59 PM', 'admin', '21/5/2007 11:46:59 PM', 'admin', 'fb1cdaf4-04bc-4667-8f0d-681964f6d7c9')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07052100020045', '21/5/2007 11:49:06 PM', 0.0000, 3, 2, 'admin', 'admin', '', '21/5/2007 11:49:06 PM', 'admin', '21/5/2007 11:49:06 PM', 'admin', '73e26857-c9ff-4be9-8cf1-039ce5570472')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07052100020046', '21/5/2007 11:50:43 PM', 0.0000, 3, 2, 'admin', 'admin', '', '21/5/2007 11:50:43 PM', 'admin', '21/5/2007 11:50:43 PM', 'admin', '0adc28fd-6d19-4e61-8d26-1842d934f4d2')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07052100020047', '21/5/2007 11:58:27 PM', 0.0000, 3, 2, 'admin', 'admin', '', '21/5/2007 11:58:27 PM', 'admin', '21/5/2007 11:58:27 PM', 'admin', '9650d9e3-8d58-4c45-b2ff-51cf7423d8fa')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07052200020001', '22/5/2007 1:47:30 AM', 0.0000, 3, 2, 'admin', 'admin', '', '22/5/2007 1:47:30 AM', 'admin', '22/5/2007 1:47:30 AM', 'admin', '7d4f03ae-39c8-458b-b957-3ce4e1cb9a73')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07052200020002', '22/5/2007 1:50:36 AM', 3000.0000, 3, 2, 'admin', 'admin', '', '22/5/2007 1:50:36 AM', 'admin', '22/5/2007 1:50:36 AM', 'admin', 'ee3f1302-eceb-4d1f-a452-b9577523e1cc')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07052200020003', '22/5/2007 3:20:27 AM', 0.0000, 3, 2, 'admin', 'admin', '', '22/5/2007 3:20:27 AM', 'admin', '22/5/2007 3:20:27 AM', 'admin', 'e457719e-cc71-4775-836e-cc775b2fb639')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07052200020004', '22/5/2007 3:20:45 AM', 0.0000, 3, 2, 'admin', 'admin', '', '22/5/2007 3:20:45 AM', 'admin', '22/5/2007 3:20:45 AM', 'admin', '9981ab8e-f4e0-4e0a-9992-f4b4f4a726ef')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07052200020005', '22/5/2007 3:25:30 AM', 0.0000, 3, 2, 'admin', 'admin', '', '22/5/2007 3:25:30 AM', 'admin', '22/5/2007 3:25:30 AM', 'admin', '701d0135-8998-4926-a632-8961382dff43')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07052200020006', '22/5/2007 3:26:17 AM', 3122.4500, 4, 2, 'admin', 'supervisor1', NULL, '22/5/2007 3:26:17 AM', 'admin', '22/5/2007 3:26:17 AM', 'admin', '8e5bc3bd-3cba-43ae-8747-ce292ea33dee')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07052200030001', '22/5/2007 3:27:34 AM', 0.0000, 3, 3, 'admin', 'admin', '', '22/5/2007 3:27:34 AM', 'admin', '22/5/2007 3:27:34 AM', 'admin', 'e1ce3e28-b984-4ac8-8c88-e1089b66a99f')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07052200030002', '22/5/2007 3:28:37 AM', 68.2000, 4, 3, 'admin', 'supervisor1', NULL, '22/5/2007 3:28:37 AM', 'admin', '22/5/2007 3:28:37 AM', 'admin', '84a3f6f9-b787-4390-ab2f-3be8fd1291c6')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07052200030003', '22/5/2007 3:28:48 AM', 0.0000, 3, 3, 'admin', 'admin', '', '22/5/2007 3:28:48 AM', 'admin', '22/5/2007 3:28:48 AM', 'admin', '68c48e3f-ac0c-4fdd-b212-e1ecb7e4a4c2')
INSERT INTO [CashRecording] ([CashRecRefNo], [CashRecordingTime], [amount], [CashRecordingTypeId], [PointOfSaleID], [CashierName], [SupervisorName], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CR07052200030004', '22/5/2007 7:30:55 PM', 0.0000, 3, 3, 'admin', 'admin', '', '22/5/2007 7:30:56 PM', 'admin', '22/5/2007 7:30:56 PM', 'admin', '535895e9-5e37-42b1-a556-0dabc4cc944b')
ALTER TABLE [CashRecording] CHECK CONSTRAINT ALL
GO



ALTER TABLE [CounterCloseLog] NOCHECK CONSTRAINT ALL
GO

PRINT 'Begin inserting data in CounterCloseLog'
INSERT INTO [CounterCloseLog] ([CounterCloseID], [FloatBalance], [StartTime], [EndTime], [Cashier], [Supervisor], [CashIn], [CashOut], [OpeningBalance], [SystemCollected], [CashCollected], [NetsCollected], [NetsTerminalID], [VisaCollected], [VisaBatchNo], [AmexCollected], [AmexBatchNo], [DepositBagNo], [TotalActualCollected], [ClosingCashOut], [Variance], [PointOfSaleID], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CL07052100020001', 100.0000, '9/5/2007 8:05:27 PM', '21/5/2007 5:35:23 PM', 'admin', 'supervisor1', 3010.0000, 2.0000, 10698.0000, 13757.7500, 11000.0000, 3057.7000, '25', 0.0000, '', 0.0000, '', '50', 14057.7000, 200.0000, 0.0500, 2, '21/5/2007 5:36:18 PM', 'admin', '21/5/2007 5:36:18 PM', 'admin', '694f2573-7c12-439d-854b-f05ab8268592')
INSERT INTO [CounterCloseLog] ([CounterCloseID], [FloatBalance], [StartTime], [EndTime], [Cashier], [Supervisor], [CashIn], [CashOut], [OpeningBalance], [SystemCollected], [CashCollected], [NetsCollected], [NetsTerminalID], [VisaCollected], [VisaBatchNo], [AmexCollected], [AmexBatchNo], [DepositBagNo], [TotalActualCollected], [ClosingCashOut], [Variance], [PointOfSaleID], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CL07052100020002', 0.0000, '21/5/2007 5:35:23 PM', '21/5/2007 5:48:40 PM', 'admin', 'supervisor1', 0.0000, 0.0000, 0.0000, 86.0000, 0.0000, 0.0000, '', 86.0000, '', 0.0000, '', '', 86.0000, 0.0000, 0.0000, 2, '21/5/2007 5:48:58 PM', 'admin', '21/5/2007 5:48:58 PM', 'admin', 'a68d624e-2b7c-432b-8633-911208a756fa')
INSERT INTO [CounterCloseLog] ([CounterCloseID], [FloatBalance], [StartTime], [EndTime], [Cashier], [Supervisor], [CashIn], [CashOut], [OpeningBalance], [SystemCollected], [CashCollected], [NetsCollected], [NetsTerminalID], [VisaCollected], [VisaBatchNo], [AmexCollected], [AmexBatchNo], [DepositBagNo], [TotalActualCollected], [ClosingCashOut], [Variance], [PointOfSaleID], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CL07052200020001', 0.0000, '21/5/2007 5:48:40 PM', '22/5/2007 3:25:57 AM', 'admin', 'supervisor1', 0.0000, 0.0000, 3000.0000, 3122.4500, 0.0000, 3122.4500, '52', 0.0000, '', 0.0000, '', '', 3122.4500, 0.0000, 0.0000, 2, '22/5/2007 3:26:17 AM', 'admin', '22/5/2007 3:26:17 AM', 'admin', 'd88bdcc0-60f8-4dc8-8bc1-3dd1ea50ad47')
INSERT INTO [CounterCloseLog] ([CounterCloseID], [FloatBalance], [StartTime], [EndTime], [Cashier], [Supervisor], [CashIn], [CashOut], [OpeningBalance], [SystemCollected], [CashCollected], [NetsCollected], [NetsTerminalID], [VisaCollected], [VisaBatchNo], [AmexCollected], [AmexBatchNo], [DepositBagNo], [TotalActualCollected], [ClosingCashOut], [Variance], [PointOfSaleID], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('CL07052200030001', 0.0000, '22/5/2007 3:27:26 AM', '22/5/2007 3:28:08 AM', 'admin', 'supervisor1', 0.0000, 0.0000, 0.0000, 68.2000, 50.0000, 0.0000, '', 18.2000, '1', 0.0000, '', '', 68.2000, 0.0000, 0.0000, 3, '22/5/2007 3:28:37 AM', 'admin', '22/5/2007 3:28:37 AM', 'admin', '665729dd-1ac2-4115-a5ae-f306055a6c63')
ALTER TABLE [CounterCloseLog] CHECK CONSTRAINT ALL
GO



ALTER TABLE [GroupUserPrivilege] NOCHECK CONSTRAINT ALL
GO

SET IDENTITY_INSERT [GroupUserPrivilege] ON 
PRINT 'Begin inserting data in GroupUserPrivilege'
INSERT INTO [GroupUserPrivilege] ([GroupUserPrivilegeID], [GroupID], [UserPrivilegeID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(91, 1, 1, NULL, NULL, NULL, NULL)
INSERT INTO [GroupUserPrivilege] ([GroupUserPrivilegeID], [GroupID], [UserPrivilegeID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(92, 1, 9, NULL, NULL, NULL, NULL)
INSERT INTO [GroupUserPrivilege] ([GroupUserPrivilegeID], [GroupID], [UserPrivilegeID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(93, 1, 10, NULL, NULL, NULL, NULL)
INSERT INTO [GroupUserPrivilege] ([GroupUserPrivilegeID], [GroupID], [UserPrivilegeID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(94, 1, 27, NULL, NULL, NULL, NULL)
INSERT INTO [GroupUserPrivilege] ([GroupUserPrivilegeID], [GroupID], [UserPrivilegeID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(95, 1, 28, NULL, NULL, NULL, NULL)
INSERT INTO [GroupUserPrivilege] ([GroupUserPrivilegeID], [GroupID], [UserPrivilegeID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(96, 2, 1, NULL, NULL, NULL, NULL)
INSERT INTO [GroupUserPrivilege] ([GroupUserPrivilegeID], [GroupID], [UserPrivilegeID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(97, 2, 2, NULL, NULL, NULL, NULL)
INSERT INTO [GroupUserPrivilege] ([GroupUserPrivilegeID], [GroupID], [UserPrivilegeID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(98, 2, 3, NULL, NULL, NULL, NULL)
INSERT INTO [GroupUserPrivilege] ([GroupUserPrivilegeID], [GroupID], [UserPrivilegeID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(99, 2, 4, NULL, NULL, NULL, NULL)
INSERT INTO [GroupUserPrivilege] ([GroupUserPrivilegeID], [GroupID], [UserPrivilegeID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(100, 2, 9, NULL, NULL, NULL, NULL)
INSERT INTO [GroupUserPrivilege] ([GroupUserPrivilegeID], [GroupID], [UserPrivilegeID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(101, 2, 10, NULL, NULL, NULL, NULL)
INSERT INTO [GroupUserPrivilege] ([GroupUserPrivilegeID], [GroupID], [UserPrivilegeID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(102, 2, 11, NULL, NULL, NULL, NULL)
INSERT INTO [GroupUserPrivilege] ([GroupUserPrivilegeID], [GroupID], [UserPrivilegeID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(103, 2, 12, NULL, NULL, NULL, NULL)
INSERT INTO [GroupUserPrivilege] ([GroupUserPrivilegeID], [GroupID], [UserPrivilegeID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(104, 2, 25, NULL, NULL, NULL, NULL)
INSERT INTO [GroupUserPrivilege] ([GroupUserPrivilegeID], [GroupID], [UserPrivilegeID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(105, 2, 26, NULL, NULL, NULL, NULL)
INSERT INTO [GroupUserPrivilege] ([GroupUserPrivilegeID], [GroupID], [UserPrivilegeID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(106, 2, 27, NULL, NULL, NULL, NULL)
INSERT INTO [GroupUserPrivilege] ([GroupUserPrivilegeID], [GroupID], [UserPrivilegeID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(107, 2, 28, NULL, NULL, NULL, NULL)
INSERT INTO [GroupUserPrivilege] ([GroupUserPrivilegeID], [GroupID], [UserPrivilegeID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(152, 4, 1, NULL, NULL, NULL, NULL)
INSERT INTO [GroupUserPrivilege] ([GroupUserPrivilegeID], [GroupID], [UserPrivilegeID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(153, 4, 3, NULL, NULL, NULL, NULL)
INSERT INTO [GroupUserPrivilege] ([GroupUserPrivilegeID], [GroupID], [UserPrivilegeID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(154, 4, 13, NULL, NULL, NULL, NULL)
INSERT INTO [GroupUserPrivilege] ([GroupUserPrivilegeID], [GroupID], [UserPrivilegeID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(155, 4, 15, NULL, NULL, NULL, NULL)
INSERT INTO [GroupUserPrivilege] ([GroupUserPrivilegeID], [GroupID], [UserPrivilegeID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(156, 4, 17, NULL, NULL, NULL, NULL)
INSERT INTO [GroupUserPrivilege] ([GroupUserPrivilegeID], [GroupID], [UserPrivilegeID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(157, 4, 22, NULL, NULL, NULL, NULL)
INSERT INTO [GroupUserPrivilege] ([GroupUserPrivilegeID], [GroupID], [UserPrivilegeID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(158, 4, 24, NULL, NULL, NULL, NULL)
INSERT INTO [GroupUserPrivilege] ([GroupUserPrivilegeID], [GroupID], [UserPrivilegeID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(159, 4, 25, NULL, NULL, NULL, NULL)
INSERT INTO [GroupUserPrivilege] ([GroupUserPrivilegeID], [GroupID], [UserPrivilegeID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(160, 3, 1, NULL, NULL, NULL, NULL)
INSERT INTO [GroupUserPrivilege] ([GroupUserPrivilegeID], [GroupID], [UserPrivilegeID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(161, 3, 2, NULL, NULL, NULL, NULL)
INSERT INTO [GroupUserPrivilege] ([GroupUserPrivilegeID], [GroupID], [UserPrivilegeID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(162, 3, 3, NULL, NULL, NULL, NULL)
INSERT INTO [GroupUserPrivilege] ([GroupUserPrivilegeID], [GroupID], [UserPrivilegeID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(163, 3, 4, NULL, NULL, NULL, NULL)
INSERT INTO [GroupUserPrivilege] ([GroupUserPrivilegeID], [GroupID], [UserPrivilegeID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(164, 3, 5, NULL, NULL, NULL, NULL)
INSERT INTO [GroupUserPrivilege] ([GroupUserPrivilegeID], [GroupID], [UserPrivilegeID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(165, 3, 6, NULL, NULL, NULL, NULL)
INSERT INTO [GroupUserPrivilege] ([GroupUserPrivilegeID], [GroupID], [UserPrivilegeID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(166, 3, 7, NULL, NULL, NULL, NULL)
INSERT INTO [GroupUserPrivilege] ([GroupUserPrivilegeID], [GroupID], [UserPrivilegeID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(167, 3, 8, NULL, NULL, NULL, NULL)
INSERT INTO [GroupUserPrivilege] ([GroupUserPrivilegeID], [GroupID], [UserPrivilegeID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(168, 3, 9, NULL, NULL, NULL, NULL)
INSERT INTO [GroupUserPrivilege] ([GroupUserPrivilegeID], [GroupID], [UserPrivilegeID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(169, 3, 10, NULL, NULL, NULL, NULL)
INSERT INTO [GroupUserPrivilege] ([GroupUserPrivilegeID], [GroupID], [UserPrivilegeID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(170, 3, 11, NULL, NULL, NULL, NULL)
INSERT INTO [GroupUserPrivilege] ([GroupUserPrivilegeID], [GroupID], [UserPrivilegeID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(171, 3, 12, NULL, NULL, NULL, NULL)
INSERT INTO [GroupUserPrivilege] ([GroupUserPrivilegeID], [GroupID], [UserPrivilegeID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(172, 3, 13, NULL, NULL, NULL, NULL)
INSERT INTO [GroupUserPrivilege] ([GroupUserPrivilegeID], [GroupID], [UserPrivilegeID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(173, 3, 14, NULL, NULL, NULL, NULL)
INSERT INTO [GroupUserPrivilege] ([GroupUserPrivilegeID], [GroupID], [UserPrivilegeID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(174, 3, 15, NULL, NULL, NULL, NULL)
INSERT INTO [GroupUserPrivilege] ([GroupUserPrivilegeID], [GroupID], [UserPrivilegeID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(175, 3, 16, NULL, NULL, NULL, NULL)
INSERT INTO [GroupUserPrivilege] ([GroupUserPrivilegeID], [GroupID], [UserPrivilegeID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(176, 3, 17, NULL, NULL, NULL, NULL)
INSERT INTO [GroupUserPrivilege] ([GroupUserPrivilegeID], [GroupID], [UserPrivilegeID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(177, 3, 18, NULL, NULL, NULL, NULL)
INSERT INTO [GroupUserPrivilege] ([GroupUserPrivilegeID], [GroupID], [UserPrivilegeID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(178, 3, 19, NULL, NULL, NULL, NULL)
INSERT INTO [GroupUserPrivilege] ([GroupUserPrivilegeID], [GroupID], [UserPrivilegeID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(179, 3, 20, NULL, NULL, NULL, NULL)
INSERT INTO [GroupUserPrivilege] ([GroupUserPrivilegeID], [GroupID], [UserPrivilegeID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(180, 3, 21, NULL, NULL, NULL, NULL)
INSERT INTO [GroupUserPrivilege] ([GroupUserPrivilegeID], [GroupID], [UserPrivilegeID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(181, 3, 22, NULL, NULL, NULL, NULL)
INSERT INTO [GroupUserPrivilege] ([GroupUserPrivilegeID], [GroupID], [UserPrivilegeID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(182, 3, 23, NULL, NULL, NULL, NULL)
INSERT INTO [GroupUserPrivilege] ([GroupUserPrivilegeID], [GroupID], [UserPrivilegeID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(183, 3, 24, NULL, NULL, NULL, NULL)
INSERT INTO [GroupUserPrivilege] ([GroupUserPrivilegeID], [GroupID], [UserPrivilegeID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(184, 3, 25, NULL, NULL, NULL, NULL)
INSERT INTO [GroupUserPrivilege] ([GroupUserPrivilegeID], [GroupID], [UserPrivilegeID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(185, 3, 26, NULL, NULL, NULL, NULL)
INSERT INTO [GroupUserPrivilege] ([GroupUserPrivilegeID], [GroupID], [UserPrivilegeID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(186, 3, 27, NULL, NULL, NULL, NULL)
INSERT INTO [GroupUserPrivilege] ([GroupUserPrivilegeID], [GroupID], [UserPrivilegeID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(187, 3, 28, NULL, NULL, NULL, NULL)
INSERT INTO [GroupUserPrivilege] ([GroupUserPrivilegeID], [GroupID], [UserPrivilegeID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(188, 3, 29, NULL, NULL, NULL, NULL)
INSERT INTO [GroupUserPrivilege] ([GroupUserPrivilegeID], [GroupID], [UserPrivilegeID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(189, 3, 30, NULL, NULL, NULL, NULL)
SET IDENTITY_INSERT [GroupUserPrivilege] OFF 
ALTER TABLE [GroupUserPrivilege] CHECK CONSTRAINT ALL
GO



ALTER TABLE [InventoryDet] NOCHECK CONSTRAINT ALL
GO

PRINT 'Begin inserting data in InventoryDet'
INSERT INTO [InventoryDet] ([InventoryDetRefNo], [ItemNo], [InventoryHdrRefNo], [ExpiryDate], [Quantity], [CostOfGoods], [Remark], [Discount], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [UniqueID])
VALUES('IN07052100020002.0', 'I00000000001', 'IN07052100020002', NULL, 3500, 0.6300, NULL, 0.00, 'admin', '21/5/2007 11:36:39 PM', 'admin', '21/5/2007 11:36:39 PM', '6637f1ed-79e3-415f-9f2f-ec64e268884a')
INSERT INTO [InventoryDet] ([InventoryDetRefNo], [ItemNo], [InventoryHdrRefNo], [ExpiryDate], [Quantity], [CostOfGoods], [Remark], [Discount], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [UniqueID])
VALUES('IN07052100020002.1', 'I00000000022', 'IN07052100020002', NULL, 4000, 6.8250, NULL, 0.00, 'admin', '21/5/2007 11:36:39 PM', 'admin', '21/5/2007 11:36:39 PM', '71353c5b-5c1b-41c5-a9dc-a34f0f48f71e')
INSERT INTO [InventoryDet] ([InventoryDetRefNo], [ItemNo], [InventoryHdrRefNo], [ExpiryDate], [Quantity], [CostOfGoods], [Remark], [Discount], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [UniqueID])
VALUES('IN07052100020002.2', 'I00200000032', 'IN07052100020002', NULL, 6000, 0.5250, NULL, 0.00, 'admin', '21/5/2007 11:36:39 PM', 'admin', '21/5/2007 11:36:39 PM', '1cbe2353-dc87-432c-ac8e-22bdee3030bc')
INSERT INTO [InventoryDet] ([InventoryDetRefNo], [ItemNo], [InventoryHdrRefNo], [ExpiryDate], [Quantity], [CostOfGoods], [Remark], [Discount], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [UniqueID])
VALUES('IN07052100020004.1', 'I00000000022', 'IN07052100020004', NULL, -1, 6.8250, '', NULL, 'admin', '21/5/2007 11:49:37 PM', 'admin', '21/5/2007 11:49:37 PM', 'aeb1410d-c472-4b22-9c9d-b2ee4dcd99c9')
INSERT INTO [InventoryDet] ([InventoryDetRefNo], [ItemNo], [InventoryHdrRefNo], [ExpiryDate], [Quantity], [CostOfGoods], [Remark], [Discount], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [UniqueID])
VALUES('IN07052100020005.1', 'I00000000022', 'IN07052100020005', NULL, -1, 6.8250, '', NULL, 'admin', '21/5/2007 11:49:59 PM', 'admin', '21/5/2007 11:49:59 PM', '490389b1-b0b8-4bd4-a531-2e7ec42a618c')
INSERT INTO [InventoryDet] ([InventoryDetRefNo], [ItemNo], [InventoryHdrRefNo], [ExpiryDate], [Quantity], [CostOfGoods], [Remark], [Discount], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [UniqueID])
VALUES('IN07052100020006.0', 'I00000000022', 'IN07052100020006', NULL, 300, 5.2500, NULL, 0.00, 'admin', '21/5/2007 11:50:28 PM', 'admin', '21/5/2007 11:50:28 PM', '842528a0-33cb-4973-9c3e-803514426be6')
INSERT INTO [InventoryDet] ([InventoryDetRefNo], [ItemNo], [InventoryHdrRefNo], [ExpiryDate], [Quantity], [CostOfGoods], [Remark], [Discount], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [UniqueID])
VALUES('IN07052100020007.1', 'I00000000022', 'IN07052100020007', NULL, -1, 6.7151, '', NULL, 'admin', '21/5/2007 11:50:59 PM', 'admin', '21/5/2007 11:50:59 PM', '3f95c419-2186-47f1-accd-3b2aebe9eb48')
INSERT INTO [InventoryDet] ([InventoryDetRefNo], [ItemNo], [InventoryHdrRefNo], [ExpiryDate], [Quantity], [CostOfGoods], [Remark], [Discount], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [UniqueID])
VALUES('IN07052100020008.1', 'I00200000032', 'IN07052100020008', NULL, -1000, 0.5250, NULL, 0.00, 'admin', '21/5/2007 11:57:11 PM', 'admin', '21/5/2007 11:57:11 PM', 'd4328f86-a861-47dd-b2ce-16e0288b976f')
INSERT INTO [InventoryDet] ([InventoryDetRefNo], [ItemNo], [InventoryHdrRefNo], [ExpiryDate], [Quantity], [CostOfGoods], [Remark], [Discount], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [UniqueID])
VALUES('IN07052200020001.1', 'I00000000001', 'IN07052200020001', NULL, -30, 0.6300, '', NULL, 'admin', '22/5/2007 1:51:02 AM', 'admin', '22/5/2007 1:51:02 AM', 'b93f2e83-e7c8-48ce-84b1-abfb286ce5e7')
INSERT INTO [InventoryDet] ([InventoryDetRefNo], [ItemNo], [InventoryHdrRefNo], [ExpiryDate], [Quantity], [CostOfGoods], [Remark], [Discount], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [UniqueID])
VALUES('IN07052200020003.1', 'I00000000022', 'IN07052200020003', NULL, -1, 6.7151, '', NULL, 'admin', '22/5/2007 3:25:53 AM', 'admin', '22/5/2007 3:25:53 AM', '7b36a078-35bf-4367-a85a-4644f908d086')
INSERT INTO [InventoryDet] ([InventoryDetRefNo], [ItemNo], [InventoryHdrRefNo], [ExpiryDate], [Quantity], [CostOfGoods], [Remark], [Discount], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [UniqueID])
VALUES('IN07052200030001.2', 'I00000000022', 'IN07052200030001', NULL, -1, 0.0000, '', NULL, 'admin', '22/5/2007 3:27:43 AM', 'admin', '22/5/2007 3:27:43 AM', 'c55fc3e0-4e9c-4a85-9e43-f5a245de54b4')
INSERT INTO [InventoryDet] ([InventoryDetRefNo], [ItemNo], [InventoryHdrRefNo], [ExpiryDate], [Quantity], [CostOfGoods], [Remark], [Discount], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [UniqueID])
VALUES('IN07052200030004.0', 'I00000000022', 'IN07052200030004', NULL, 6000, 3.1500, NULL, 0.00, 'admin', '22/5/2007 7:31:50 PM', 'admin', '22/5/2007 7:31:50 PM', '1f3f1974-3c4e-4eaa-ab02-75f10128d39b')
ALTER TABLE [InventoryDet] CHECK CONSTRAINT ALL
GO



ALTER TABLE [LoginActivity] NOCHECK CONSTRAINT ALL
GO

PRINT 'Begin inserting data in LoginActivity'
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('9d03b9ab-361f-40a7-bcc4-0097b06d282b', 'admin', 'Login', '22/5/2007 7:30:55 PM', 3, '', '22/5/2007 7:30:55 PM', '', '22/5/2007 7:30:55 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('3c3793c0-4702-4d12-a553-01b3da68dbbb', 'admin2', 'Authorizing', '14/5/2007 5:12:03 PM', 2, '', '14/5/2007 5:12:04 PM', '', '14/5/2007 5:12:04 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('3ef5ce2c-1d64-4b90-a192-036e3c1edbe8', 'admin', 'Logout', '21/5/2007 12:41:23 PM', 2, '', '21/5/2007 12:41:23 PM', '', '21/5/2007 12:41:23 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('6a1526c8-14f7-4faf-8a47-03bdcee39bed', 'admin', 'Login', '21/5/2007 12:38:50 PM', 2, '', '21/5/2007 12:38:50 PM', '', '21/5/2007 12:38:50 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('833b433b-96d7-49a1-9055-047e1e46950f', 'admin', 'Login', '14/5/2007 5:08:32 PM', 2, '', '14/5/2007 5:08:33 PM', '', '14/5/2007 5:08:33 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('748f8933-cc1d-4e29-b7f4-07a8f1bc9ae7', 'admin', 'Login', '21/5/2007 1:34:49 PM', 2, '', '21/5/2007 1:34:49 PM', '', '21/5/2007 1:34:49 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('0e5d599d-965b-42c4-aaee-094677182d60', 'admin', 'Logout', '21/5/2007 12:54:45 PM', 2, '', '21/5/2007 12:54:45 PM', '', '21/5/2007 12:54:45 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('61f837d2-a4c1-4ad6-a9c2-0a61725d5e7b', 'admin', 'Login', '19/5/2007 11:57:51 PM', 2, '', '19/5/2007 11:57:51 PM', '', '19/5/2007 11:57:51 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('3db044cd-bb49-4417-912c-0d6e8b032141', 'admin', 'Login', '9/5/2007 8:58:05 PM', 2, '', '9/5/2007 8:58:05 PM', '', '9/5/2007 8:58:05 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('b05d3b39-6ef8-46e9-aeed-0d95dcd05aba', 'admin', 'Login', '21/5/2007 12:52:42 PM', 2, '', '21/5/2007 12:52:42 PM', '', '21/5/2007 12:52:42 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('1485eec5-6007-4d4b-a088-0e0605598478', 'admin', 'Login', '21/5/2007 1:11:02 PM', 2, '', '21/5/2007 1:11:02 PM', '', '21/5/2007 1:11:02 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('7d6631fd-b0e8-484e-b8b6-102a805a229f', 'admin', 'Logout', '21/5/2007 3:34:05 PM', 2, '', '21/5/2007 3:34:05 PM', '', '21/5/2007 3:34:05 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('f6d91a40-2c3e-4af2-aded-1030e0867d99', 'admin', 'Login', '22/5/2007 3:28:48 AM', 3, '', '22/5/2007 3:28:48 AM', '', '22/5/2007 3:28:48 AM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('c5683217-860d-4271-8a93-116ed56c10e0', 'admin', 'Login', '21/5/2007 3:03:15 PM', 2, '', '21/5/2007 3:03:15 PM', '', '21/5/2007 3:03:15 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('df2c69ae-1444-4c66-8fb7-12b4c292c31d', 'admin', 'Login', '21/5/2007 3:30:35 PM', 2, '', '21/5/2007 3:30:35 PM', '', '21/5/2007 3:30:35 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('05e2d48b-4984-4324-a710-134fb04ffa9e', 'admin', 'Logout', '21/5/2007 10:59:08 AM', 2, '', '21/5/2007 10:59:08 AM', '', '21/5/2007 10:59:08 AM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('d246758a-1195-4c4f-86f6-14e9dd0f6556', 'admin', 'Login', '21/5/2007 12:42:11 PM', 2, '', '21/5/2007 12:42:11 PM', '', '21/5/2007 12:42:11 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('0f5c3388-f3bb-45ec-bfc2-14fe8f735428', 'admin', 'Login', '21/5/2007 11:01:42 AM', 2, '', '21/5/2007 11:01:42 AM', '', '21/5/2007 11:01:42 AM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('45a0370b-11bb-4bce-bbd9-154754730fd1', 'admin', 'Login', '21/5/2007 11:50:43 PM', 2, '', '21/5/2007 11:50:43 PM', '', '21/5/2007 11:50:43 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('7cf883b6-dda1-45ab-ae69-1791d4736e99', 'cashier1', 'Logout', '20/5/2007 11:07:58 PM', 2, '', '20/5/2007 11:07:58 PM', '', '20/5/2007 11:07:58 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('62211531-776b-4565-975f-1b0fc05439b6', 'admin', 'Logout', '9/5/2007 9:16:16 PM', 2, '', '9/5/2007 9:16:16 PM', '', '9/5/2007 9:16:16 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('a10415f9-094e-4f7e-a1d1-1b138fa8ab99', 'admin', 'Login', '22/5/2007 1:47:30 AM', 2, '', '22/5/2007 1:47:30 AM', '', '22/5/2007 1:47:30 AM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('3207928d-f895-4643-b903-1b423734dd1b', 'cashier1', 'Login', '21/5/2007 10:59:36 AM', 2, '', '21/5/2007 10:59:36 AM', '', '21/5/2007 10:59:36 AM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('3686cea6-d017-4ad9-894f-1d85d09dfc9e', 'admin', 'Login', '21/5/2007 11:19:54 PM', 2, '', '21/5/2007 11:19:54 PM', '', '21/5/2007 11:19:54 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('91faebfa-78c3-4d38-877f-1e195474764c', 'admin', 'Logout', '10/5/2007 9:10:35 AM', 2, '', '10/5/2007 9:10:35 AM', '', '10/5/2007 9:10:35 AM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('351138e2-d46a-43bc-a305-1e3799f62ba7', 'supervisor1', 'Authorizing', '21/5/2007 1:23:10 PM', 2, '', '21/5/2007 1:23:10 PM', '', '21/5/2007 1:23:10 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('85a81770-4c0e-4532-a75a-21734c47e1fb', 'admin', 'Login', '21/5/2007 3:11:24 PM', 2, '', '21/5/2007 3:11:24 PM', '', '21/5/2007 3:11:24 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('0aaedef9-1ee1-4d4e-8b6c-21f4bf4a01f1', 'admin', 'Login', '21/5/2007 3:34:10 PM', 2, '', '21/5/2007 3:34:10 PM', '', '21/5/2007 3:34:10 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('90e9b624-6ad2-4f02-8730-221e4168f037', 'admin', 'Login', '14/5/2007 3:36:22 PM', 2, '', '14/5/2007 3:36:22 PM', '', '14/5/2007 3:36:22 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('2ba5be76-dbaa-432f-aecf-23b675babfaf', 'admin', 'Login', '14/5/2007 7:11:29 PM', 1, '', '14/5/2007 7:11:29 PM', '', '14/5/2007 7:11:29 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('427db731-0201-440a-8da3-23e4dfb436cd', 'admin', 'Login', '21/5/2007 10:55:18 AM', 2, '', '21/5/2007 10:55:18 AM', '', '21/5/2007 10:55:18 AM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('517f6e3d-b53c-4a8c-a54f-2451d86fc363', 'admin', 'Logout', '19/5/2007 11:59:05 PM', 2, '', '19/5/2007 11:59:05 PM', '', '19/5/2007 11:59:05 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('1ca88e69-43d5-4d2d-ac4d-26d2e3197ef9', 'admin', 'Logout', '14/5/2007 6:39:23 PM', 2, '', '14/5/2007 6:39:23 PM', '', '14/5/2007 6:39:23 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('70597355-819c-4591-91d3-270009c13fa8', 'admin', 'Logout', '21/5/2007 12:33:26 PM', 2, '', '21/5/2007 12:33:26 PM', '', '21/5/2007 12:33:26 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('cecccd0c-c71c-4d5e-9065-285e73acedc3', 'supervisor1', 'Authorizing', '22/5/2007 3:22:23 AM', 2, '', '22/5/2007 3:22:23 AM', '', '22/5/2007 3:22:23 AM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('8bee9699-1f11-4790-8cc8-2904be36b905', 'admin', 'Login', '21/5/2007 11:46:59 PM', 2, '', '21/5/2007 11:46:59 PM', '', '21/5/2007 11:46:59 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('8f486aa8-9796-43db-a383-292bf1525255', 'admin2', 'Authorizing', '21/5/2007 1:21:24 PM', 2, '', '21/5/2007 1:21:24 PM', '', '21/5/2007 1:21:24 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('76aae95c-0bc4-4cac-9582-2a5dd215bf8d', 'admin', 'Login', '21/5/2007 12:41:01 PM', 2, '', '21/5/2007 12:41:01 PM', '', '21/5/2007 12:41:01 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('85f7361b-a571-472d-9451-2c5c60bc3060', 'admin', 'Logout', '21/5/2007 3:42:40 PM', 2, '', '21/5/2007 3:42:40 PM', '', '21/5/2007 3:42:40 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('96184650-2be5-48c6-bd40-2c744bf9d1fd', 'admin', 'Login', '21/5/2007 3:19:10 PM', 2, '', '21/5/2007 3:19:10 PM', '', '21/5/2007 3:19:10 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('d378b87f-8b05-4a15-8cb6-2ced4a0ef2ec', 'admin', 'Login', '22/5/2007 1:50:36 AM', 2, '', '22/5/2007 1:50:36 AM', '', '22/5/2007 1:50:36 AM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('95c750fa-0913-4f93-82ac-2ddd59fa4804', 'admin', 'Login', '10/5/2007 8:59:21 AM', 1, '', '10/5/2007 8:59:21 AM', '', '10/5/2007 8:59:21 AM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('5d5296c4-02be-455c-8f0c-2e9917ac532b', 'admin', 'Login', '21/5/2007 1:00:40 PM', 2, '', '21/5/2007 1:00:40 PM', '', '21/5/2007 1:00:40 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('8d346f7d-2705-4ae2-b53d-2f031ae4de4f', 'admin', 'Logout', '21/5/2007 1:21:30 PM', 2, '', '21/5/2007 1:21:30 PM', '', '21/5/2007 1:21:30 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('a3aedc40-0815-4db9-87d5-33fec949a4b0', 'admin', 'Login', '10/5/2007 9:00:18 AM', 1, '', '10/5/2007 9:00:18 AM', '', '10/5/2007 9:00:18 AM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('6155c00c-5768-46ae-9183-340ebb28d355', 'admin', 'Login', '21/5/2007 12:46:43 PM', 2, '', '21/5/2007 12:46:44 PM', '', '21/5/2007 12:46:44 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('7a6243eb-9ca4-494f-b815-35801ce4f219', 'admin', 'Login', '14/5/2007 5:52:28 PM', 2, '', '14/5/2007 5:52:28 PM', '', '14/5/2007 5:52:28 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('c4a5b48e-dd04-45ad-b12d-35d1dcb82d5e', 'admin', 'Login', '10/5/2007 12:48:40 AM', 1, '', '10/5/2007 12:48:40 AM', '', '10/5/2007 12:48:40 AM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('0d7068d2-7b7e-47fd-9b21-36786c0231c1', 'admin', 'Login', '17/5/2007 11:51:28 PM', 2, '', '17/5/2007 11:51:28 PM', '', '17/5/2007 11:51:28 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('2281aeab-1907-4876-a0c4-36c8f9c439ab', 'admin', 'Login', '21/5/2007 8:12:36 PM', 2, '', '21/5/2007 8:12:36 PM', '', '21/5/2007 8:12:36 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('de9319f0-7bfe-4b10-838c-36cd6af44119', 'admin', 'Logout', '21/5/2007 3:08:15 PM', 2, '', '21/5/2007 3:08:15 PM', '', '21/5/2007 3:08:15 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('3d9698e8-d01a-496e-a67b-372eb99613b6', 'admin', 'Login', '22/5/2007 3:27:26 AM', 3, '', '22/5/2007 3:27:26 AM', '', '22/5/2007 3:27:26 AM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('52410df9-c0f3-48fc-a0e7-38f829229a00', 'admin', 'Login', '21/5/2007 3:39:06 PM', 2, '', '21/5/2007 3:39:06 PM', '', '21/5/2007 3:39:06 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('65173dae-d478-49f0-a297-3a66459fe361', 'admin', 'Logout', '17/5/2007 12:19:03 PM', 2, '', '17/5/2007 12:19:03 PM', '', '17/5/2007 12:19:03 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('bbfda7e3-7441-46ef-b9ed-3ab711aaaf94', 'admin', 'Login', '21/5/2007 11:22:41 PM', 2, '', '21/5/2007 11:22:41 PM', '', '21/5/2007 11:22:41 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('ebfb9734-9dd7-42a0-bca3-3b31a84e0a09', 'admin', 'Logout', '21/5/2007 11:04:48 AM', 2, '', '21/5/2007 11:04:48 AM', '', '21/5/2007 11:04:48 AM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('fc8cc94e-a1bc-49a9-9d9d-3bc35144c8a5', 'admin', 'Login', '19/5/2007 11:54:43 PM', 2, '', '19/5/2007 11:54:43 PM', '', '19/5/2007 11:54:43 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('d05e9c8f-384d-4536-ae83-3cbe575e160f', 'admin', 'Logout', '14/5/2007 5:12:07 PM', 2, '', '14/5/2007 5:12:07 PM', '', '14/5/2007 5:12:07 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('5e8d3ed2-364b-4eee-93a6-3ce3ecad911c', 'admin', 'Login', '22/5/2007 3:20:27 AM', 2, '', '22/5/2007 3:20:27 AM', '', '22/5/2007 3:20:27 AM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('2ee3aaf0-e049-49a1-94c6-3dc1997860f2', 'admin', 'Login', '21/5/2007 12:43:43 PM', 2, '', '21/5/2007 12:43:43 PM', '', '21/5/2007 12:43:43 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('f98950f0-98ce-49c6-9141-3edb24ef7e1f', 'admin', 'Login', '21/5/2007 3:21:00 PM', 2, '', '21/5/2007 3:21:00 PM', '', '21/5/2007 3:21:00 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('fb01914b-77a9-4f0a-b06e-3f562fbc2ba0', 'admin', 'Login', '21/5/2007 12:33:02 PM', 2, '', '21/5/2007 12:33:02 PM', '', '21/5/2007 12:33:02 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('e4e45efa-eb5a-4abf-8443-41b5ce2ef30c', 'cashier1', 'Login', '20/5/2007 10:34:22 PM', 2, '', '20/5/2007 10:34:22 PM', '', '20/5/2007 10:34:22 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('1633c995-075d-4c36-b2af-44cf0b436bcc', 'admin', 'Login', '22/5/2007 1:50:29 AM', 2, '', '22/5/2007 1:50:29 AM', '', '22/5/2007 1:50:29 AM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('f93a2fc1-6494-4a0d-897b-45931bde8f7f', 'supervisor1', 'Authorizing', '21/5/2007 5:36:18 PM', 2, '', '21/5/2007 5:36:18 PM', '', '21/5/2007 5:36:18 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('4ebc2a2f-9b41-484d-952b-45f4905facfc', 'admin', 'Login', '22/5/2007 3:40:02 AM', 3, '', '22/5/2007 3:40:02 AM', '', '22/5/2007 3:40:02 AM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('071ecbff-f918-4ea6-919f-46705f292fbe', 'admin', 'Login', '14/5/2007 12:50:00 PM', 2, '', '14/5/2007 12:50:00 PM', '', '14/5/2007 12:50:00 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('1bc05831-a0f4-4ab3-a307-467a900e4592', 'admin', 'Login', '21/5/2007 5:35:12 PM', 2, '', '21/5/2007 5:35:12 PM', '', '21/5/2007 5:35:12 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('037ba35d-fc1d-414b-9f17-49283a92d9d0', 'admin', 'Login', '21/5/2007 11:58:27 PM', 2, '', '21/5/2007 11:58:27 PM', '', '21/5/2007 11:58:27 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('9e415c96-fbb9-4052-8c0e-499a45ff729a', 'admin', 'Login', '20/5/2007 10:31:13 PM', 2, '', '20/5/2007 10:31:13 PM', '', '20/5/2007 10:31:13 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('0c06876f-700b-453f-81ab-4d1b4f3f84de', 'admin', 'Login', '21/5/2007 3:15:39 PM', 2, '', '21/5/2007 3:15:39 PM', '', '21/5/2007 3:15:39 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('0c10eebf-2587-4534-a4b4-4d337c6c16d9', 'admin', 'Login', '10/5/2007 8:58:42 AM', 2, '', '10/5/2007 8:58:42 AM', '', '10/5/2007 8:58:42 AM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('3074ad20-fb1b-41d8-a84e-4e1aef908ca8', 'admin', 'Login', '9/5/2007 11:32:19 PM', 1, '', '9/5/2007 11:32:19 PM', '', '9/5/2007 11:32:19 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('e508d4d0-2d85-4dac-9d6d-4f9348270718', 'supervisor1', 'Authorizing', '22/5/2007 3:26:17 AM', 2, '', '22/5/2007 3:26:17 AM', '', '22/5/2007 3:26:17 AM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('3d3d9824-1036-47aa-9463-528009df99fd', 'admin', 'Login', '17/5/2007 11:49:41 PM', 2, '', '17/5/2007 11:49:41 PM', '', '17/5/2007 11:49:41 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('f5f7b778-cdfe-4494-b713-55229cf1e86b', 'admin', 'Logout', '21/5/2007 12:26:23 PM', 2, '', '21/5/2007 12:26:23 PM', '', '21/5/2007 12:26:23 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('51af9a35-8388-4d7a-b766-55d7fbaf974e', 'admin', 'Logout', '14/5/2007 4:20:40 PM', 2, '', '14/5/2007 4:20:40 PM', '', '14/5/2007 4:20:40 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('e7d0fd5c-2280-41ed-998a-56ca340b487e', 'admin', 'Login', '21/5/2007 1:10:21 PM', 2, '', '21/5/2007 1:10:21 PM', '', '21/5/2007 1:10:21 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('b4f894f8-e76f-4b36-a58b-57d8d70b8d9d', 'admin', 'Login', '21/5/2007 3:38:53 PM', 2, '', '21/5/2007 3:38:53 PM', '', '21/5/2007 3:38:53 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('c8aa2a51-34b0-4aff-bfce-5a1feb604775', 'admin', 'Logout', '14/5/2007 3:38:05 PM', 2, '', '14/5/2007 3:38:05 PM', '', '14/5/2007 3:38:05 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('ad5eaa62-6feb-4d72-97fe-5a4efbf2c238', 'admin', 'Logout', '21/5/2007 3:31:22 PM', 2, '', '21/5/2007 3:31:22 PM', '', '21/5/2007 3:31:22 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('f3f87ee8-ad77-42fe-837d-5c57cffbc85d', 'cashier1', 'Logout', '9/5/2007 8:05:42 PM', 2, '', '9/5/2007 8:05:42 PM', '', '9/5/2007 8:05:42 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('14d9ff06-3b80-461b-887e-5c5f970c44b5', 'cashier1', 'Logout', '20/5/2007 10:13:39 PM', 2, '', '20/5/2007 10:13:39 PM', '', '20/5/2007 10:13:39 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('60f2575e-d692-4212-bc88-5caeadcc55cc', 'admin', 'Login', '21/5/2007 11:04:18 AM', 2, '', '21/5/2007 11:04:18 AM', '', '21/5/2007 11:04:18 AM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('69dc365f-f900-43d1-9e06-5d1f18df573d', 'admin', 'Login', '22/5/2007 3:25:30 AM', 2, '', '22/5/2007 3:25:30 AM', '', '22/5/2007 3:25:30 AM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('060244ba-1bb9-4c22-9c5f-5f40c0e230d2', 'admin', 'Logout', '9/5/2007 8:59:21 PM', 2, '', '9/5/2007 8:59:21 PM', '', '9/5/2007 8:59:21 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('954402f3-bc65-4bd6-93e2-60de93fcf280', 'admin', 'Login', '21/5/2007 12:52:36 PM', 2, '', '21/5/2007 12:52:36 PM', '', '21/5/2007 12:52:36 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('c6c541be-842e-461f-b314-6443f0416d6f', 'admin', 'Login', '21/5/2007 3:23:52 PM', 2, '', '21/5/2007 3:23:52 PM', '', '21/5/2007 3:23:52 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('a3e49368-bfbd-4e17-b1c6-654b802b7ef7', 'admin', 'Logout', '21/5/2007 11:02:35 AM', 2, '', '21/5/2007 11:02:35 AM', '', '21/5/2007 11:02:35 AM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('5355191c-696c-44f3-a744-65b0399feaae', 'admin', 'Logout', '9/5/2007 8:05:35 PM', 2, '', '9/5/2007 8:05:35 PM', '', '9/5/2007 8:05:35 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('31bfce7a-153b-4b94-93b3-6718c42410d5', 'admin', 'Logout', '14/5/2007 3:40:12 PM', 2, '', '14/5/2007 3:40:12 PM', '', '14/5/2007 3:40:12 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('84f0a0d4-d5c1-446b-80e2-67272ef8bf50', 'admin', 'Logout', '21/5/2007 1:26:48 PM', 2, '', '21/5/2007 1:26:48 PM', '', '21/5/2007 1:26:48 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('26646398-6696-4e73-bbf2-6850150c05f3', 'admin', 'Login', '20/5/2007 12:02:02 AM', 2, '', '20/5/2007 12:02:02 AM', '', '20/5/2007 12:02:02 AM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('037cbbf7-7c2a-4b19-bda1-693d8a6dd6ff', 'admin', 'Login', '21/5/2007 11:19:05 PM', 2, '', '21/5/2007 11:19:05 PM', '', '21/5/2007 11:19:05 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('e63b388b-24ab-40fd-a158-6aa67612ec0b', 'admin', 'Login', '9/5/2007 8:05:27 PM', 2, '', '9/5/2007 8:05:27 PM', '', '9/5/2007 8:05:27 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('fb50e5cc-37b8-4550-b627-6ae5d584cc1f', 'admin', 'Logout', '21/5/2007 1:11:46 PM', 2, '', '21/5/2007 1:11:46 PM', '', '21/5/2007 1:11:46 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('3aca3da5-19a2-4605-bfc0-6bbd3e979deb', 'cashier1', 'Login', '20/5/2007 10:53:57 PM', 2, '', '20/5/2007 10:53:57 PM', '', '20/5/2007 10:53:57 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('bdb2b16d-9085-4e9b-a3f7-6cf5c34b5bf4', 'admin', 'Login', '21/5/2007 3:31:05 PM', 2, '', '21/5/2007 3:31:05 PM', '', '21/5/2007 3:31:05 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('f6ac26be-a23a-4182-9adb-6e277fa448e5', 'admin', 'Login', '21/5/2007 11:46:47 PM', 2, '', '21/5/2007 11:46:47 PM', '', '21/5/2007 11:46:47 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('881d927a-d001-4947-99b3-70ca634c2400', 'admin', 'Logout', '21/5/2007 3:21:08 PM', 2, '', '21/5/2007 3:21:08 PM', '', '21/5/2007 3:21:08 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('e868ff84-99bb-42d8-85df-730246812eba', 'admin', 'Login', '10/5/2007 9:09:05 AM', 2, '', '10/5/2007 9:09:05 AM', '', '10/5/2007 9:09:05 AM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('1a1a7c1d-4253-4356-88cf-73752b29480b', 'admin', 'Login', '21/5/2007 12:32:55 PM', 2, '', '21/5/2007 12:32:55 PM', '', '21/5/2007 12:32:55 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('402e030a-cf56-4990-a47b-73937bc56709', 'admin', 'Logout', '21/5/2007 1:03:57 PM', 2, '', '21/5/2007 1:03:57 PM', '', '21/5/2007 1:03:57 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('985b28a1-05e0-4df1-b0ac-7472407faaa7', 'admin', 'Logout', '21/5/2007 5:49:02 PM', 2, '', '21/5/2007 5:49:02 PM', '', '21/5/2007 5:49:02 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('048eb2d6-f19e-4233-a0e3-76e30eea0bcb', 'admin', 'Login', '14/5/2007 3:39:21 PM', 2, '', '14/5/2007 3:39:21 PM', '', '14/5/2007 3:39:21 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('567d04b2-229c-40f9-982f-777237391434', 'admin', 'Logout', '18/5/2007 1:35:19 AM', 2, '', '18/5/2007 1:35:19 AM', '', '18/5/2007 1:35:19 AM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('cbde818f-df0a-4913-aac2-79ce5fc4fce9', 'admin', 'Login', '21/5/2007 1:22:13 PM', 2, '', '21/5/2007 1:22:13 PM', '', '21/5/2007 1:22:13 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('a8654b19-a743-4105-8692-7a9505ed28c8', 'cashier1', 'Login', '21/5/2007 12:38:37 PM', 2, '', '21/5/2007 12:38:37 PM', '', '21/5/2007 12:38:37 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('f8c602b0-8984-4f17-addc-7c0b56dfa07d', 'cashier1', 'Login', '20/5/2007 10:11:14 PM', 2, '', '20/5/2007 10:11:14 PM', '', '20/5/2007 10:11:14 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('d0feff76-7ebf-4543-a673-7e3b33e693cc', 'supervisor1', 'Authorizing', '22/5/2007 3:28:37 AM', 3, '', '22/5/2007 3:28:37 AM', '', '22/5/2007 3:28:37 AM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('d51e6b01-81fe-416a-9e91-7e92cf969a2d', 'admin', 'Login', '21/5/2007 11:33:41 PM', 2, '', '21/5/2007 11:33:41 PM', '', '21/5/2007 11:33:41 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('be72f855-bf23-460a-8917-7eb5bfbc3d01', 'admin', 'Logout', '21/5/2007 3:11:51 PM', 2, '', '21/5/2007 3:11:51 PM', '', '21/5/2007 3:11:51 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('da440a21-0557-4a3a-9a8d-7f93adcd450e', 'admin', 'Login', '21/5/2007 1:07:51 PM', 2, '', '21/5/2007 1:07:51 PM', '', '21/5/2007 1:07:51 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('3717a887-e4f0-4303-b1cf-8071b3a11516', 'admin', 'Login', '14/5/2007 6:38:56 PM', 2, '', '14/5/2007 6:38:56 PM', '', '14/5/2007 6:38:56 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('ab1b3ade-ac1c-49d6-ac92-8072313c361e', 'admin', 'Login', '21/5/2007 12:38:45 PM', 2, '', '21/5/2007 12:38:46 PM', '', '21/5/2007 12:38:46 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('198d9721-2d7f-4591-bc0f-839edc67a64b', 'admin', 'Login', '21/5/2007 3:11:30 PM', 2, '', '21/5/2007 3:11:30 PM', '', '21/5/2007 3:11:30 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('50e35a00-e846-4882-a2de-852830f1b3c9', 'admin', 'Login', '22/5/2007 3:20:45 AM', 2, '', '22/5/2007 3:20:45 AM', '', '22/5/2007 3:20:45 AM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('35df139a-43ec-43ec-a879-86bfd6f762dd', 'admin', 'Logout', '21/5/2007 5:36:25 PM', 2, '', '21/5/2007 5:36:25 PM', '', '21/5/2007 5:36:25 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('dbe0a101-b48d-4ea8-adaa-889cc0c0d9dd', 'admin', 'Logout', '21/5/2007 1:35:14 PM', 2, '', '21/5/2007 1:35:14 PM', '', '21/5/2007 1:35:14 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('2a14b6da-310e-4f7e-b493-89c30207365f', 'admin', 'Login', '21/5/2007 3:23:46 PM', 2, '', '21/5/2007 3:23:46 PM', '', '21/5/2007 3:23:46 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('5acc9d45-3d5c-42a9-9887-8deb38d53c5a', 'admin', 'Login', '10/5/2007 3:43:05 AM', 1, '', '10/5/2007 3:43:05 AM', '', '10/5/2007 3:43:05 AM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('1ec530e0-b127-4b5d-8fa8-911782ae6772', 'admin', 'Login', '21/5/2007 3:20:55 PM', 2, '', '21/5/2007 3:20:55 PM', '', '21/5/2007 3:20:55 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('2bd28c0a-641b-4cd1-b2c6-913e4fec0669', 'admin', 'Logout', '15/5/2007 6:22:40 PM', 2, '', '15/5/2007 6:22:40 PM', '', '15/5/2007 6:22:40 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('f158e9bb-3c41-489f-ad5e-91767c76fdc1', 'admin', 'Login', '21/5/2007 1:18:18 PM', 2, '', '21/5/2007 1:18:18 PM', '', '21/5/2007 1:18:18 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('d4f39cb2-1d26-4d68-a243-91a694eebd9b', 'admin', 'Logout', '21/5/2007 8:19:24 PM', 2, '', '21/5/2007 8:19:24 PM', '', '21/5/2007 8:19:24 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('227013c5-6f3b-4c75-a281-959f72df384b', 'cashier1', 'Logout', '20/5/2007 10:32:54 PM', 2, '', '20/5/2007 10:32:54 PM', '', '20/5/2007 10:32:54 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('15bfc096-5f4f-4c1d-923f-9600384ce53f', 'admin', 'Login', '21/5/2007 3:08:00 PM', 2, '', '21/5/2007 3:08:00 PM', '', '21/5/2007 3:08:00 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('7b53603e-eb66-4fe4-93b7-967ecdb6bef1', 'supervisor1', 'Authorizing', '22/5/2007 3:22:10 AM', 2, '', '22/5/2007 3:22:10 AM', '', '22/5/2007 3:22:10 AM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('d460de81-99e1-4fb7-a662-970d59e6711a', 'admin', 'Login', '22/5/2007 1:47:23 AM', 2, '', '22/5/2007 1:47:23 AM', '', '22/5/2007 1:47:23 AM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('e8e43ab2-cc02-43db-9ab5-9905b8119d3e', 'admin', 'Logout', '14/5/2007 5:55:04 PM', 2, '', '14/5/2007 5:55:04 PM', '', '14/5/2007 5:55:04 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('19b743a0-0998-4ca0-90c5-9c8f2a5e235b', 'cashier1', 'Login', '9/5/2007 8:05:41 PM', 2, '', '9/5/2007 8:05:41 PM', '', '9/5/2007 8:05:41 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('f2a0be28-8f7e-474b-b1d4-9cb9a09c6632', 'admin', 'Logout', '21/5/2007 11:30:32 PM', 2, '', '21/5/2007 11:30:32 PM', '', '21/5/2007 11:30:32 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('e450d8de-2d91-4fcf-92fa-9cc5b0676df1', 'admin', 'Login', '21/5/2007 11:36:10 PM', 2, '', '21/5/2007 11:36:10 PM', '', '21/5/2007 11:36:10 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('17ddfee3-29e0-4d0d-9bfa-9dcacd1df397', 'admin', 'Login', '22/5/2007 7:30:43 PM', 3, '', '22/5/2007 7:30:43 PM', '', '22/5/2007 7:30:43 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('4a6d35d4-4949-4628-9dce-9f068782979b', 'admin', 'Login', '9/5/2007 8:06:08 PM', 1, '', '9/5/2007 8:06:08 PM', '', '9/5/2007 8:06:08 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('93041575-d270-445d-b442-9f49b8532dc8', 'admin', 'Logout', '21/5/2007 12:43:08 PM', 2, '', '21/5/2007 12:43:08 PM', '', '21/5/2007 12:43:08 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('57149c9f-9948-42f2-8247-9f7f588a568f', 'admin', 'Logout', '22/5/2007 2:03:54 AM', 2, '', '22/5/2007 2:03:54 AM', '', '22/5/2007 2:03:54 AM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('a0c35a7d-3729-4cdf-8826-9fcc73a80815', 'admin', 'Login', '21/5/2007 1:10:13 PM', 2, '', '21/5/2007 1:10:13 PM', '', '21/5/2007 1:10:13 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('433c7800-9a6d-47f4-893c-a36089122509', 'admin', 'Logout', '18/5/2007 1:19:37 AM', 2, '', '18/5/2007 1:19:37 AM', '', '18/5/2007 1:19:37 AM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('e60ce494-0deb-450a-a692-a4a70744efed', 'admin', 'Login', '21/5/2007 1:00:46 PM', 2, '', '21/5/2007 1:00:46 PM', '', '21/5/2007 1:00:46 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('2cfc4a1e-6680-4488-8b76-a57443689c8c', 'admin', 'Login', '14/5/2007 4:19:45 PM', 2, '', '14/5/2007 4:19:45 PM', '', '14/5/2007 4:19:45 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('fb496036-23ab-48fa-9221-a961cafb2457', 'admin', 'Logout', '14/5/2007 1:14:51 PM', 2, '', '14/5/2007 1:14:51 PM', '', '14/5/2007 1:14:51 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('2588943a-bf14-41ea-90b1-aa25bf113838', 'cashier1', 'Login', '21/5/2007 12:31:30 PM', 2, '', '21/5/2007 12:31:30 PM', '', '21/5/2007 12:31:30 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('09f2de7f-a34f-4a7c-8d69-aa3108453240', 'admin', 'Logout', '22/5/2007 3:26:33 AM', 2, '', '22/5/2007 3:26:33 AM', '', '22/5/2007 3:26:33 AM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('2b2322c1-feb2-49a7-b7f1-ab52195f6b54', 'admin', 'Login', '19/5/2007 11:50:46 PM', 2, '', '19/5/2007 11:50:46 PM', '', '19/5/2007 11:50:46 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('3a57f0a4-c5bf-430a-9f18-ac1f215d9ef8', 'admin', 'Login', '21/5/2007 11:49:06 PM', 2, '', '21/5/2007 11:49:06 PM', '', '21/5/2007 11:49:06 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('a94aa580-f710-4eb3-a76b-ad3d4cd31948', 'admin', 'Login', '21/5/2007 1:18:08 PM', 2, '', '21/5/2007 1:18:08 PM', '', '21/5/2007 1:18:08 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('217e4fb9-8ccd-4eb4-8979-b020e99bed77', 'admin', 'Login', '22/5/2007 3:25:24 AM', 2, '', '22/5/2007 3:25:24 AM', '', '22/5/2007 3:25:24 AM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('29af0c39-8a35-465b-82e9-b24fcce3096c', 'admin', 'Logout', '21/5/2007 12:39:23 PM', 2, '', '21/5/2007 12:39:23 PM', '', '21/5/2007 12:39:23 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('d15b27a2-154e-45ac-9378-b2be851a9c82', 'admin', 'Logout', '17/5/2007 11:49:55 PM', 2, '', '17/5/2007 11:49:55 PM', '', '17/5/2007 11:49:55 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('6c6e67cc-cce6-4f6f-919c-b33007675a23', 'cashier1', 'Logout', '21/5/2007 12:32:45 PM', 2, '', '21/5/2007 12:32:45 PM', '', '21/5/2007 12:32:45 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('ef0e8734-2ca2-4ea3-8ed6-b3b12bd04e7f', 'admin', 'Login', '14/5/2007 7:04:29 PM', 1, '', '14/5/2007 7:04:29 PM', '', '14/5/2007 7:04:29 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('3335206c-ecf7-46ed-98c3-b5a1046c87cc', 'admin', 'Login', '21/5/2007 12:42:18 PM', 2, '', '21/5/2007 12:42:18 PM', '', '21/5/2007 12:42:18 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('c05cf0d4-21ec-4a43-9462-b7314f2d2055', 'admin', 'Login', '17/5/2007 11:56:27 PM', 2, '', '17/5/2007 11:56:27 PM', '', '17/5/2007 11:56:27 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('632fe80f-cbde-41af-8302-b88c899f9501', 'cashier1', 'Logout', '20/5/2007 10:34:47 PM', 2, '', '20/5/2007 10:34:47 PM', '', '20/5/2007 10:34:47 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('93cb3d4d-ccfe-4630-8f8b-b9061616808a', 'admin', 'Logout', '22/5/2007 3:40:42 AM', 3, '', '22/5/2007 3:40:42 AM', '', '22/5/2007 3:40:42 AM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('287d353b-0ecc-4942-a702-b961a253471b', 'admin', 'Login', '21/5/2007 1:10:57 PM', 2, '', '21/5/2007 1:10:57 PM', '', '21/5/2007 1:10:57 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('e3638b18-3f2f-46c0-a427-b9a3fe5f64d5', 'admin', 'Logout', '21/5/2007 3:34:21 PM', 2, '', '21/5/2007 3:34:21 PM', '', '21/5/2007 3:34:21 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('9636cbf3-d65d-40b1-a07b-ba7ced48b3dc', 'admin', 'Login', '21/5/2007 12:43:48 PM', 2, '', '21/5/2007 12:43:48 PM', '', '21/5/2007 12:43:48 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('7dc4e2dd-1dfc-4013-b4a2-bab885d06f90', 'admin', 'Login', '14/5/2007 6:41:19 PM', 1, '', '14/5/2007 6:41:19 PM', '', '14/5/2007 6:41:19 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('db8fe545-e385-4ede-80f1-bae80321e6bf', 'admin', 'Login', '21/5/2007 11:27:00 PM', 2, '', '21/5/2007 11:27:00 PM', '', '21/5/2007 11:27:00 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('3b28c22b-a5c8-45db-b4f9-bb86c932fdde', 'admin', 'Login', '21/5/2007 5:47:51 PM', 2, '', '21/5/2007 5:47:51 PM', '', '21/5/2007 5:47:51 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('19e38afd-7286-4ff2-b3d7-be4d79aac174', 'admin', 'Login', '9/5/2007 8:58:57 PM', 2, '', '9/5/2007 8:58:57 PM', '', '9/5/2007 8:58:57 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('70d6efc7-6616-4b39-b57f-bfc6765d7527', 'admin', 'Logout', '21/5/2007 11:59:48 PM', 2, '', '21/5/2007 11:59:48 PM', '', '21/5/2007 11:59:48 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('0cfe8b1f-33cd-4226-be53-bfe4e8d3db7b', 'admin', 'Login', '21/5/2007 10:55:06 AM', 2, '', '21/5/2007 10:55:06 AM', '', '21/5/2007 10:55:06 AM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('cb84ba4d-839b-47ed-8ed9-c208562d3880', 'admin', 'Login', '18/5/2007 1:33:50 AM', 2, '', '18/5/2007 1:33:50 AM', '', '18/5/2007 1:33:50 AM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('2f45d05e-dd9d-49d0-860b-c38ada1bd206', 'admin', 'Login', '21/5/2007 3:33:47 PM', 2, '', '21/5/2007 3:33:47 PM', '', '21/5/2007 3:33:47 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('21a70330-c512-4ea0-8db5-c41a95f189e3', 'admin', 'Login', '21/5/2007 12:38:31 PM', 2, '', '21/5/2007 12:38:31 PM', '', '21/5/2007 12:38:31 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('11a97768-f444-4cbb-9d0d-c532aff7aa7c', 'admin', 'Login', '21/5/2007 12:46:50 PM', 2, '', '21/5/2007 12:46:50 PM', '', '21/5/2007 12:46:50 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('82d8c1d3-7d13-4a4a-8dac-c595f7520e03', 'admin', 'Login', '21/5/2007 11:01:52 AM', 2, '', '21/5/2007 11:01:52 AM', '', '21/5/2007 11:01:52 AM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('18459351-0efe-4d4b-93b9-c7d9c450b821', 'admin', 'Login', '21/5/2007 3:31:00 PM', 2, '', '21/5/2007 3:31:00 PM', '', '21/5/2007 3:31:00 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('9168775c-c42d-44ed-9319-c89e67d960fa', 'admin', 'Login', '21/5/2007 3:03:08 PM', 2, '', '21/5/2007 3:03:08 PM', '', '21/5/2007 3:03:08 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('5f6f6eca-c2df-42c5-b625-cb39d0015e34', 'admin', 'Logout', '21/5/2007 1:44:05 PM', 2, '', '21/5/2007 1:44:05 PM', '', '21/5/2007 1:44:05 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('bc14605e-0ff0-441f-b64f-cc3b2f2f3ca4', 'admin', 'Logout', '17/5/2007 11:56:57 PM', 2, '', '17/5/2007 11:56:57 PM', '', '17/5/2007 11:56:57 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('001ce053-4546-43e2-b145-cd1730d8ae02', 'admin', 'Login', '18/5/2007 1:16:17 AM', 2, '', '18/5/2007 1:16:17 AM', '', '18/5/2007 1:16:17 AM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('d318f965-3f09-4af9-b25b-cd539acc8a4a', 'admin', 'Login', '9/5/2007 9:15:53 PM', 2, '', '9/5/2007 9:15:54 PM', '', '9/5/2007 9:15:54 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('b5aa9237-cc55-46d3-a1ac-cd81a0d077db', 'admin', 'Login', '21/5/2007 3:30:29 PM', 2, '', '21/5/2007 3:30:29 PM', '', '21/5/2007 3:30:29 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('22b67e4b-c439-4648-b63c-ce037dc91c71', 'admin', 'Logout', '21/5/2007 12:48:21 PM', 2, '', '21/5/2007 12:48:21 PM', '', '21/5/2007 12:48:21 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('bfd0dc6f-b03b-4adc-b2ae-ce2df845fbce', 'admin', 'Logout', '22/5/2007 3:29:00 AM', 3, '', '22/5/2007 3:29:00 AM', '', '22/5/2007 3:29:00 AM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('ffba1103-a004-43f3-ae38-cf1d280e297e', 'admin', 'Login', '21/5/2007 11:56:51 PM', 2, '', '21/5/2007 11:56:51 PM', '', '21/5/2007 11:56:51 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('340f3d41-1513-4028-90c9-cfc6bb65a55c', 'cashier1', 'Logout', '21/5/2007 10:59:41 AM', 2, '', '21/5/2007 10:59:41 AM', '', '21/5/2007 10:59:41 AM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('25ae0c1e-951d-487c-9a46-d07cfebf6114', 'admin', 'Login', '21/5/2007 10:59:28 AM', 2, '', '21/5/2007 10:59:29 AM', '', '21/5/2007 10:59:29 AM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('59892878-a42f-4efe-83b8-d1416bf7aa5c', 'admin', 'Login', '21/5/2007 12:31:17 PM', 2, '', '21/5/2007 12:31:17 PM', '', '21/5/2007 12:31:17 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('dc1d7480-ff41-4c3d-b16c-d1e0ff3ab4dd', 'admin', 'Login', '20/5/2007 10:34:10 PM', 2, '', '20/5/2007 10:34:10 PM', '', '20/5/2007 10:34:10 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('1f4346ef-3c86-45d5-8aca-d2ac94768200', 'admin', 'Login', '21/5/2007 3:08:06 PM', 2, '', '21/5/2007 3:08:06 PM', '', '21/5/2007 3:08:06 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('3376fd2a-2fd1-40f0-be60-d35946a98b6d', 'admin', 'Login', '19/5/2007 11:52:54 PM', 2, '', '19/5/2007 11:52:55 PM', '', '19/5/2007 11:52:55 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('88fe26c0-eae4-4e96-9cc3-d4b546fe88a1', 'cashier1', 'Login', '20/5/2007 10:31:22 PM', 2, '', '20/5/2007 10:31:22 PM', '', '20/5/2007 10:31:22 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('53fd56d0-4882-4659-a4ec-d72a8547f1d8', 'admin', 'Login', '14/5/2007 5:56:09 PM', 1, '', '14/5/2007 5:56:09 PM', '', '14/5/2007 5:56:09 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('ffef37ea-7126-4cec-8574-d743577ea3de', 'admin', 'Login', '21/5/2007 5:47:46 PM', 2, '', '21/5/2007 5:47:46 PM', '', '21/5/2007 5:47:46 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('2b1e282f-9b4d-4b75-ab1d-d9944ab5047c', 'admin', 'Login', '15/5/2007 6:21:58 PM', 2, '', '15/5/2007 6:21:59 PM', '', '15/5/2007 6:21:59 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('667e4f66-e408-4577-b036-daf303c5d6c0', 'admin', 'Logout', '14/5/2007 3:06:27 PM', 2, '', '14/5/2007 3:06:27 PM', '', '14/5/2007 3:06:27 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('ff8dcd4b-a97f-4d83-826f-db1df64c2629', 'admin', 'Login', '21/5/2007 1:07:58 PM', 2, '', '21/5/2007 1:07:58 PM', '', '21/5/2007 1:07:58 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('51e17e38-226d-49f6-9c16-dc18eb462ac0', 'admin', 'Login', '22/5/2007 3:27:34 AM', 3, '', '22/5/2007 3:27:34 AM', '', '22/5/2007 3:27:34 AM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('58a8f2b1-318b-480e-a074-dc647785d060', 'supervisor1', 'Authorizing', '21/5/2007 1:22:55 PM', 2, '', '21/5/2007 1:22:55 PM', '', '21/5/2007 1:22:55 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('1405d0a5-1edb-419e-ad74-dcf34d622f36', 'supervisor1', 'Authorizing', '21/5/2007 3:41:23 PM', 2, '', '21/5/2007 3:41:23 PM', '', '21/5/2007 3:41:23 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('2cf7986f-c955-4652-be36-de446a7c4efb', 'admin', 'Logout', '21/5/2007 3:36:24 PM', 2, '', '21/5/2007 3:36:24 PM', '', '21/5/2007 3:36:24 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('d54c8cf5-8f76-4501-a031-e0dac2eff247', 'cashier1', 'Login', '20/5/2007 11:07:54 PM', 2, '', '20/5/2007 11:07:54 PM', '', '20/5/2007 11:07:54 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('1277fe46-2ff0-4249-b450-e16ca7932a71', 'admin', 'Login', '20/5/2007 10:53:47 PM', 2, '', '20/5/2007 10:53:47 PM', '', '20/5/2007 10:53:47 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('a52b2835-c530-49d0-9fa8-e1f28fda2b6a', 'admin', 'Login', '21/5/2007 3:33:52 PM', 2, '', '21/5/2007 3:33:52 PM', '', '21/5/2007 3:33:52 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('5f5c266b-077b-4008-91f3-e368e98554d5', 'admin', 'Login', '14/5/2007 6:39:46 PM', 1, '', '14/5/2007 6:39:46 PM', '', '14/5/2007 6:39:46 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('676e0e49-a360-4204-94ed-e4a3d973c5bf', 'admin', 'Logout', '21/5/2007 1:09:36 PM', 2, '', '21/5/2007 1:09:36 PM', '', '21/5/2007 1:09:36 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('53a2c8f3-ac3c-4fb9-86ba-e688de308a47', 'supervisor1', 'Authorizing', '21/5/2007 1:24:18 PM', 2, '', '21/5/2007 1:24:18 PM', '', '21/5/2007 1:24:18 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('a207c294-cfac-4d38-a4f4-e6a8c29a7e66', 'admin', 'Login', '21/5/2007 11:19:16 PM', 2, '', '21/5/2007 11:19:16 PM', '', '21/5/2007 11:19:16 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('f34a9dbd-1385-4f8f-8c84-e9561fd907b4', 'admin', 'Login', '20/5/2007 10:11:04 PM', 2, '', '20/5/2007 10:11:04 PM', '', '20/5/2007 10:11:04 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('793f7f2b-9d93-45f9-b784-e992b6da17b6', 'admin', 'Login', '21/5/2007 11:48:52 PM', 2, '', '21/5/2007 11:48:52 PM', '', '21/5/2007 11:48:52 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('c02c55ab-293a-4fdb-b364-e9cd72124cbb', 'admin', 'Login', '9/5/2007 11:42:00 PM', 1, '', '9/5/2007 11:42:00 PM', '', '9/5/2007 11:42:00 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('08eff5d1-37e6-4038-8a8a-eaddb8d5fa08', 'admin', 'Login', '21/5/2007 1:34:57 PM', 2, '', '21/5/2007 1:34:57 PM', '', '21/5/2007 1:34:57 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('0f6682b8-7187-45aa-8b49-eb9023a44538', 'admin', 'Logout', '21/5/2007 12:44:38 PM', 2, '', '21/5/2007 12:44:38 PM', '', '21/5/2007 12:44:38 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('8d868660-3882-423a-abca-ebbc02240c37', 'admin', 'Login', '14/5/2007 3:05:27 PM', 2, '', '14/5/2007 3:05:27 PM', '', '14/5/2007 3:05:27 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('8a479aef-aa40-4761-b9be-ec97c68346f7', 'admin', 'Login', '20/5/2007 11:07:45 PM', 2, '', '20/5/2007 11:07:45 PM', '', '20/5/2007 11:07:45 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('52825970-ac03-4c64-8f37-ec99b44e9193', 'admin', 'Logout', '20/5/2007 12:02:24 AM', 2, '', '20/5/2007 12:02:24 AM', '', '20/5/2007 12:02:24 AM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('2e55d664-05d7-46fe-bf42-ec9a67adc15f', 'admin', 'Login', '21/5/2007 11:04:24 AM', 2, '', '21/5/2007 11:04:24 AM', '', '21/5/2007 11:04:24 AM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('6ee5c18d-3397-4db2-b13f-ef35ae4a7445', 'admin', 'Logout', '21/5/2007 3:05:50 PM', 2, '', '21/5/2007 3:05:50 PM', '', '21/5/2007 3:05:50 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('51214abd-43be-4e36-a029-f1355bba2b24', 'supervisor1', 'Authorizing', '21/5/2007 5:48:58 PM', 2, '', '21/5/2007 5:48:58 PM', '', '21/5/2007 5:48:58 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('3ff6caa9-9eea-4967-bd51-f20c81eac405', 'admin', 'Login', '21/5/2007 3:19:05 PM', 2, '', '21/5/2007 3:19:05 PM', '', '21/5/2007 3:19:05 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('9f14412e-7ba4-48f5-9e38-f41bf2575b45', 'admin', 'Login', '21/5/2007 1:44:03 PM', 2, '', '21/5/2007 1:44:03 PM', '', '21/5/2007 1:44:03 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('a27b80c9-7f45-485c-ab12-f5a5bd76222f', 'admin', 'Login', '21/5/2007 12:24:47 PM', 2, '', '21/5/2007 12:24:47 PM', '', '21/5/2007 12:24:47 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('d27688b0-5147-4fa9-b19c-f60f8e0e13bd', 'admin', 'Login', '21/5/2007 5:35:06 PM', 2, '', '21/5/2007 5:35:06 PM', '', '21/5/2007 5:35:06 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('13ba9439-7e65-4d09-ab06-f6542ccb3e47', 'cashier1', 'Logout', '20/5/2007 10:55:09 PM', 2, '', '20/5/2007 10:55:09 PM', '', '20/5/2007 10:55:09 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('b9399022-0af8-47f3-b59e-f6fc76da1f70', 'admin', 'Logout', '22/5/2007 7:32:12 PM', 3, '', '22/5/2007 7:32:12 PM', '', '22/5/2007 7:32:12 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('7b2c391a-fa14-413b-915e-f7b9940afed3', 'admin', 'Login', '17/5/2007 12:18:41 PM', 2, '', '17/5/2007 12:18:42 PM', '', '17/5/2007 12:18:42 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('a010ff8c-4269-45bc-b53f-f8e7aa110309', 'cashier1', 'Logout', '21/5/2007 12:38:41 PM', 2, '', '21/5/2007 12:38:41 PM', '', '21/5/2007 12:38:41 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('20350405-5d4a-4fad-a517-fa5abd7d0ff7', 'admin', 'Login', '21/5/2007 12:24:54 PM', 2, '', '21/5/2007 12:24:54 PM', '', '21/5/2007 12:24:54 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('48ad00c8-ffae-4a4b-ab09-faedf6ff4d53', 'admin', 'Login', '21/5/2007 3:35:56 PM', 2, '', '21/5/2007 3:35:56 PM', '', '21/5/2007 3:35:56 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('d9d75688-1d9c-449e-9e22-fb27ead2c4a5', 'admin', 'Logout', '21/5/2007 11:35:20 PM', 2, '', '21/5/2007 11:35:20 PM', '', '21/5/2007 11:35:20 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('b03ff542-7d97-42d2-9130-fbf4538d8049', 'admin', 'Login', '21/5/2007 3:15:35 PM', 2, '', '21/5/2007 3:15:35 PM', '', '21/5/2007 3:15:35 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('0a4ef31f-ca24-453a-b64c-fc019bc917d6', 'admin', 'Login', '22/5/2007 3:20:21 AM', 2, '', '22/5/2007 3:20:21 AM', '', '22/5/2007 3:20:21 AM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('f1858dbf-7500-4c38-837d-fc6d194a0c3e', 'admin', 'Login', '21/5/2007 1:22:20 PM', 2, '', '21/5/2007 1:22:20 PM', '', '21/5/2007 1:22:20 PM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('40485586-18c2-469b-a1b1-feac9406a97d', 'admin', 'Logout', '22/5/2007 3:22:28 AM', 2, '', '22/5/2007 3:22:28 AM', '', '22/5/2007 3:22:28 AM')
INSERT INTO [LoginActivity] ([LoginActivityID], [UserName], [LoginType], [LoginDateTime], [PointOfSaleID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('c619afd8-ac24-49b0-910e-ff6d1a46fb2a', 'admin', 'Login', '21/5/2007 12:40:57 PM', 2, '', '21/5/2007 12:40:57 PM', '', '21/5/2007 12:40:57 PM')
ALTER TABLE [LoginActivity] CHECK CONSTRAINT ALL
GO



ALTER TABLE [OrderDet] NOCHECK CONSTRAINT ALL
GO

PRINT 'Begin inserting data in OrderDet'
INSERT INTO [OrderDet] ([OrderDetID], [ItemNo], [Quantity], [UnitPrice], [Discount], [Amount], [CostOfGoodSold], [IsVoided], [OrderHdrID], [ModifiedOn], [ModifiedBy], [CreatedBy], [CreatedOn], [UniqueID])
VALUES('07052100020002.1', 'I00000000022', 1, 10.0000, 0.0000, 10.0000, 6.8250,  0, '07052100020002', '21/5/2007 11:49:37 PM', 'admin', 'admin', '21/5/2007 11:49:37 PM', 'b967a047-6e36-4a70-89b7-6e91a8f5be5d')
INSERT INTO [OrderDet] ([OrderDetID], [ItemNo], [Quantity], [UnitPrice], [Discount], [Amount], [CostOfGoodSold], [IsVoided], [OrderHdrID], [ModifiedOn], [ModifiedBy], [CreatedBy], [CreatedOn], [UniqueID])
VALUES('07052100020003.1', 'I00000000022', 1, 10.0000, 0.0000, 10.0000, 6.8250,  0, '07052100020003', '21/5/2007 11:49:59 PM', 'admin', 'admin', '21/5/2007 11:49:59 PM', '1cb3cb70-acbf-46d3-9e28-aabadf83642b')
INSERT INTO [OrderDet] ([OrderDetID], [ItemNo], [Quantity], [UnitPrice], [Discount], [Amount], [CostOfGoodSold], [IsVoided], [OrderHdrID], [ModifiedOn], [ModifiedBy], [CreatedBy], [CreatedOn], [UniqueID])
VALUES('07052100020004.1', 'I00000000022', 1, 10.0000, 0.0000, 10.0000, 6.7151,  0, '07052100020004', '21/5/2007 11:50:59 PM', 'admin', 'admin', '21/5/2007 11:50:59 PM', 'f8503a5a-f6a8-4ff1-a179-6a76e480a5c1')
INSERT INTO [OrderDet] ([OrderDetID], [ItemNo], [Quantity], [UnitPrice], [Discount], [Amount], [CostOfGoodSold], [IsVoided], [OrderHdrID], [ModifiedOn], [ModifiedBy], [CreatedBy], [CreatedOn], [UniqueID])
VALUES('07052200020001.1', 'I00000000001', 30, 1.2000, 0.0000, 36.0000, 0.6300,  0, '07052200020001', '22/5/2007 1:51:02 AM', 'admin', 'admin', '22/5/2007 1:51:02 AM', 'e27e764b-83a3-47c1-b761-0a6761287b9a')
INSERT INTO [OrderDet] ([OrderDetID], [ItemNo], [Quantity], [UnitPrice], [Discount], [Amount], [CostOfGoodSold], [IsVoided], [OrderHdrID], [ModifiedOn], [ModifiedBy], [CreatedBy], [CreatedOn], [UniqueID])
VALUES('07052200020002.1', 'I00000000014', 2, 4.5000, 0.0000, 9.0000, 0.0000,  0, '07052200020002', '22/5/2007 3:21:00 AM', 'admin', 'admin', '22/5/2007 3:21:00 AM', 'e9514c89-6969-494d-9f17-f2eb0b4ec7bd')
INSERT INTO [OrderDet] ([OrderDetID], [ItemNo], [Quantity], [UnitPrice], [Discount], [Amount], [CostOfGoodSold], [IsVoided], [OrderHdrID], [ModifiedOn], [ModifiedBy], [CreatedBy], [CreatedOn], [UniqueID])
VALUES('07052200020002.2', 'I00000000015', 2, 2.3000, 0.0000, 4.6000, 0.0000,  0, '07052200020002', '22/5/2007 3:21:00 AM', 'admin', 'admin', '22/5/2007 3:21:00 AM', 'a407b327-8077-431f-8dd1-c2cc40c41bf9')
INSERT INTO [OrderDet] ([OrderDetID], [ItemNo], [Quantity], [UnitPrice], [Discount], [Amount], [CostOfGoodSold], [IsVoided], [OrderHdrID], [ModifiedOn], [ModifiedBy], [CreatedBy], [CreatedOn], [UniqueID])
VALUES('07052200020002.3', 'I00000000017', 1, 3.5000, 0.0000, 3.5000, 0.0000,  0, '07052200020002', '22/5/2007 3:21:00 AM', 'admin', 'admin', '22/5/2007 3:21:00 AM', '0892c5e6-14e6-4c27-af23-0020cd58f80b')
INSERT INTO [OrderDet] ([OrderDetID], [ItemNo], [Quantity], [UnitPrice], [Discount], [Amount], [CostOfGoodSold], [IsVoided], [OrderHdrID], [ModifiedOn], [ModifiedBy], [CreatedBy], [CreatedOn], [UniqueID])
VALUES('07052200020002.4', 'I00000000016', 1, 6.0500, 0.0000, 6.0500, 0.0000,  0, '07052200020002', '22/5/2007 3:21:00 AM', 'admin', 'admin', '22/5/2007 3:21:00 AM', 'e98eadf1-e4d3-4a83-a68b-d70c37fbea96')
INSERT INTO [OrderDet] ([OrderDetID], [ItemNo], [Quantity], [UnitPrice], [Discount], [Amount], [CostOfGoodSold], [IsVoided], [OrderHdrID], [ModifiedOn], [ModifiedBy], [CreatedBy], [CreatedOn], [UniqueID])
VALUES('07052200020002.5', 'I00000000012', 2, 2.5000, 0.0000, 5.0000, 0.0000,  0, '07052200020002', '22/5/2007 3:21:00 AM', 'admin', 'admin', '22/5/2007 3:21:00 AM', 'b9a80756-fe86-4105-875d-23078edd37e4')
INSERT INTO [OrderDet] ([OrderDetID], [ItemNo], [Quantity], [UnitPrice], [Discount], [Amount], [CostOfGoodSold], [IsVoided], [OrderHdrID], [ModifiedOn], [ModifiedBy], [CreatedBy], [CreatedOn], [UniqueID])
VALUES('07052200020002.6', 'I00000000013', 1, 3.5000, 0.0000, 3.5000, 0.0000,  0, '07052200020002', '22/5/2007 3:21:00 AM', 'admin', 'admin', '22/5/2007 3:21:00 AM', '227e6da9-09fd-4c40-96dd-6541883a7444')
INSERT INTO [OrderDet] ([OrderDetID], [ItemNo], [Quantity], [UnitPrice], [Discount], [Amount], [CostOfGoodSold], [IsVoided], [OrderHdrID], [ModifiedOn], [ModifiedBy], [CreatedBy], [CreatedOn], [UniqueID])
VALUES('07052200020003.1', 'I00000000022', 1, 10.0000, 0.0000, 10.0000, 6.7151,  0, '07052200020003', '22/5/2007 3:25:53 AM', 'admin', 'admin', '22/5/2007 3:25:53 AM', '94e09925-facf-4e4b-baa5-5b3ebf95b7d3')
INSERT INTO [OrderDet] ([OrderDetID], [ItemNo], [Quantity], [UnitPrice], [Discount], [Amount], [CostOfGoodSold], [IsVoided], [OrderHdrID], [ModifiedOn], [ModifiedBy], [CreatedBy], [CreatedOn], [UniqueID])
VALUES('07052200030001.1', 'I00000000015', 1, 2.3000, 0.0000, 2.3000, 0.0000,  0, '07052200030001', '22/5/2007 3:27:43 AM', 'admin', 'admin', '22/5/2007 3:27:43 AM', '34f84ccb-fd1d-455a-8eb9-16e223f5a04e')
INSERT INTO [OrderDet] ([OrderDetID], [ItemNo], [Quantity], [UnitPrice], [Discount], [Amount], [CostOfGoodSold], [IsVoided], [OrderHdrID], [ModifiedOn], [ModifiedBy], [CreatedBy], [CreatedOn], [UniqueID])
VALUES('07052200030001.2', 'I00000000022', 1, 10.0000, 0.0000, 10.0000, 6.7151,  0, '07052200030001', '22/5/2007 3:27:43 AM', 'admin', 'admin', '22/5/2007 3:27:43 AM', '467219b9-1df3-4368-ad0a-c776b6bf2443')
INSERT INTO [OrderDet] ([OrderDetID], [ItemNo], [Quantity], [UnitPrice], [Discount], [Amount], [CostOfGoodSold], [IsVoided], [OrderHdrID], [ModifiedOn], [ModifiedBy], [CreatedBy], [CreatedOn], [UniqueID])
VALUES('07052200030001.3', 'I00000000018', 1, 4.5000, 0.0000, 4.5000, 0.0000,  0, '07052200030001', '22/5/2007 3:27:43 AM', 'admin', 'admin', '22/5/2007 3:27:43 AM', '2882c21c-71d9-4c26-a386-a794784a8058')
INSERT INTO [OrderDet] ([OrderDetID], [ItemNo], [Quantity], [UnitPrice], [Discount], [Amount], [CostOfGoodSold], [IsVoided], [OrderHdrID], [ModifiedOn], [ModifiedBy], [CreatedBy], [CreatedOn], [UniqueID])
VALUES('07052200030002.1', 'I00000000016', 2, 6.0500, 0.0000, 12.1000, 0.0000,  0, '07052200030002', '22/5/2007 3:27:55 AM', 'admin', 'admin', '22/5/2007 3:27:55 AM', '21d420d7-3a9b-4464-8324-cd044b2eeba8')
INSERT INTO [OrderDet] ([OrderDetID], [ItemNo], [Quantity], [UnitPrice], [Discount], [Amount], [CostOfGoodSold], [IsVoided], [OrderHdrID], [ModifiedOn], [ModifiedBy], [CreatedBy], [CreatedOn], [UniqueID])
VALUES('07052200030002.2', 'I00000000014', 1, 4.5000, 0.0000, 4.5000, 0.0000,  0, '07052200030002', '22/5/2007 3:27:55 AM', 'admin', 'admin', '22/5/2007 3:27:55 AM', '2ef96449-cf83-4318-bf59-ca3b723e93e3')
INSERT INTO [OrderDet] ([OrderDetID], [ItemNo], [Quantity], [UnitPrice], [Discount], [Amount], [CostOfGoodSold], [IsVoided], [OrderHdrID], [ModifiedOn], [ModifiedBy], [CreatedBy], [CreatedOn], [UniqueID])
VALUES('07052200030002.3', 'I00000000015', 5, 2.3000, 0.0000, 11.5000, 0.0000,  0, '07052200030002', '22/5/2007 3:27:55 AM', 'admin', 'admin', '22/5/2007 3:27:55 AM', 'dcbd45e8-2535-4ace-b0ef-3e9de218c25a')
INSERT INTO [OrderDet] ([OrderDetID], [ItemNo], [Quantity], [UnitPrice], [Discount], [Amount], [CostOfGoodSold], [IsVoided], [OrderHdrID], [ModifiedOn], [ModifiedBy], [CreatedBy], [CreatedOn], [UniqueID])
VALUES('07052200030002.4', 'I00000000021', 3, 3.0000, 0.0000, 9.0000, 0.0000,  0, '07052200030002', '22/5/2007 3:27:55 AM', 'admin', 'admin', '22/5/2007 3:27:55 AM', 'fd149c6c-d7b5-4ab5-9937-bb42eb45d878')
INSERT INTO [OrderDet] ([OrderDetID], [ItemNo], [Quantity], [UnitPrice], [Discount], [Amount], [CostOfGoodSold], [IsVoided], [OrderHdrID], [ModifiedOn], [ModifiedBy], [CreatedBy], [CreatedOn], [UniqueID])
VALUES('07052200030003.1', 'I00000000026', 1, 3.5000, 0.0000, 3.5000, 0.0000,  0, '07052200030003', '22/5/2007 3:28:05 AM', 'admin', 'admin', '22/5/2007 3:28:05 AM', '58ce27d8-96cb-4782-9111-e54a2d675d3f')
INSERT INTO [OrderDet] ([OrderDetID], [ItemNo], [Quantity], [UnitPrice], [Discount], [Amount], [CostOfGoodSold], [IsVoided], [OrderHdrID], [ModifiedOn], [ModifiedBy], [CreatedBy], [CreatedOn], [UniqueID])
VALUES('07052200030003.2', 'I00000000027', 1, 4.5000, 0.0000, 4.5000, 0.0000,  0, '07052200030003', '22/5/2007 3:28:05 AM', 'admin', 'admin', '22/5/2007 3:28:05 AM', 'bdec9c84-a650-405f-af56-dd9e26e810ac')
INSERT INTO [OrderDet] ([OrderDetID], [ItemNo], [Quantity], [UnitPrice], [Discount], [Amount], [CostOfGoodSold], [IsVoided], [OrderHdrID], [ModifiedOn], [ModifiedBy], [CreatedBy], [CreatedOn], [UniqueID])
VALUES('07052200030003.3', 'I00000000024', 1, 3.0000, 0.0000, 3.0000, 0.0000,  0, '07052200030003', '22/5/2007 3:28:05 AM', 'admin', 'admin', '22/5/2007 3:28:05 AM', 'c8cc995d-ef8c-46e7-b90a-5c901017bed4')
ALTER TABLE [OrderDet] CHECK CONSTRAINT ALL
GO



ALTER TABLE [OrderHdr] NOCHECK CONSTRAINT ALL
GO

PRINT 'Begin inserting data in OrderHdr'
INSERT INTO [OrderHdr] ([OrderHdrID], [OrderRefNo], [Discount], [ServiceCharge], [CashierID], [PointOfSaleID], [OrderDate], [IsSuspended], [IsVoided], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('07052100020001', 'OR07052100020001', 0.00, 0.00, 'admin', 2, '21/5/2007 11:47:02 PM',  0,  0, NULL, '21/5/2007 11:47:10 PM', 'admin', '21/5/2007 11:47:10 PM', 'admin', 'a77231b0-434d-4f02-852f-65f33cd00945')
INSERT INTO [OrderHdr] ([OrderHdrID], [OrderRefNo], [Discount], [ServiceCharge], [CashierID], [PointOfSaleID], [OrderDate], [IsSuspended], [IsVoided], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('07052100020002', 'OR07052100020002', 0.00, 0.00, 'admin', 2, '21/5/2007 11:49:08 PM',  0,  0, NULL, '21/5/2007 11:49:14 PM', 'admin', '21/5/2007 11:49:14 PM', 'admin', 'b774a5b7-07b5-447e-a5b8-89f27cfc065f')
INSERT INTO [OrderHdr] ([OrderHdrID], [OrderRefNo], [Discount], [ServiceCharge], [CashierID], [PointOfSaleID], [OrderDate], [IsSuspended], [IsVoided], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('07052100020003', 'OR07052100020003', 0.00, 0.00, 'admin', 2, '21/5/2007 11:49:46 PM',  0,  0, NULL, '21/5/2007 11:49:57 PM', 'admin', '21/5/2007 11:49:57 PM', 'admin', '0411d91e-c439-49ba-a74e-db85488627ff')
INSERT INTO [OrderHdr] ([OrderHdrID], [OrderRefNo], [Discount], [ServiceCharge], [CashierID], [PointOfSaleID], [OrderDate], [IsSuspended], [IsVoided], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('07052100020004', 'OR07052100020004', 0.00, 0.00, 'admin', 2, '21/5/2007 11:50:46 PM',  0,  0, NULL, '21/5/2007 11:50:51 PM', 'admin', '21/5/2007 11:50:51 PM', 'admin', 'a34b0a22-f5ab-461d-83ab-0557a4acf41d')
INSERT INTO [OrderHdr] ([OrderHdrID], [OrderRefNo], [Discount], [ServiceCharge], [CashierID], [PointOfSaleID], [OrderDate], [IsSuspended], [IsVoided], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('07052200020001', 'OR07052200020001', 3.00, 0.00, 'admin', 2, '22/5/2007 1:50:43 AM',  0,  0, NULL, '22/5/2007 1:51:02 AM', 'admin', '22/5/2007 1:51:02 AM', 'admin', '497d1632-0cc9-4d0a-aeeb-215c0ef4a1a4')
INSERT INTO [OrderHdr] ([OrderHdrID], [OrderRefNo], [Discount], [ServiceCharge], [CashierID], [PointOfSaleID], [OrderDate], [IsSuspended], [IsVoided], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('07052200020002', 'OR07052200020002', 0.00, 0.00, 'admin', 2, '22/5/2007 3:20:46 AM',  0,  0, NULL, '22/5/2007 3:21:00 AM', 'admin', '22/5/2007 3:21:00 AM', 'admin', '6f2188a3-b7b3-4368-96d2-1835fcf2e54d')
INSERT INTO [OrderHdr] ([OrderHdrID], [OrderRefNo], [Discount], [ServiceCharge], [CashierID], [PointOfSaleID], [OrderDate], [IsSuspended], [IsVoided], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('07052200020003', 'OR07052200020003', 0.00, 0.00, 'admin', 2, '22/5/2007 3:25:46 AM',  0,  0, NULL, '22/5/2007 3:25:53 AM', 'admin', '22/5/2007 3:25:53 AM', 'admin', '512ccddd-722c-47b0-ab01-af1aabebc69d')
INSERT INTO [OrderHdr] ([OrderHdrID], [OrderRefNo], [Discount], [ServiceCharge], [CashierID], [PointOfSaleID], [OrderDate], [IsSuspended], [IsVoided], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('07052200030001', 'OR07052200030001', 0.00, 0.00, 'admin', 3, '22/5/2007 3:27:36 AM',  0,  0, NULL, '22/5/2007 3:27:43 AM', 'admin', '22/5/2007 3:27:43 AM', 'admin', '363672ae-5d7a-49bd-863e-2ed5f57212b0')
INSERT INTO [OrderHdr] ([OrderHdrID], [OrderRefNo], [Discount], [ServiceCharge], [CashierID], [PointOfSaleID], [OrderDate], [IsSuspended], [IsVoided], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('07052200030002', 'OR07052200030002', 0.00, 0.00, 'admin', 3, '22/5/2007 3:27:44 AM',  0,  0, NULL, '22/5/2007 3:27:55 AM', 'admin', '22/5/2007 3:27:55 AM', 'admin', '29d3c7b2-5b15-49ec-86ad-31cce754f932')
INSERT INTO [OrderHdr] ([OrderHdrID], [OrderRefNo], [Discount], [ServiceCharge], [CashierID], [PointOfSaleID], [OrderDate], [IsSuspended], [IsVoided], [Remark], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('07052200030003', 'OR07052200030003', 0.00, 0.00, 'admin', 3, '22/5/2007 3:27:57 AM',  0,  0, NULL, '22/5/2007 3:28:05 AM', 'admin', '22/5/2007 3:28:05 AM', 'admin', '21d83a4a-b56e-469f-a3f9-ed530f3cd57e')
ALTER TABLE [OrderHdr] CHECK CONSTRAINT ALL
GO



ALTER TABLE [PromoCampaignDet] NOCHECK CONSTRAINT ALL
GO

SET IDENTITY_INSERT [PromoCampaignDet] ON 
PRINT 'Begin inserting data in PromoCampaignDet'
INSERT INTO [PromoCampaignDet] ([PromoCampaignDetID], [PromoCampaignHdrID], [ItemGroupID], [ItemNo], [CategoryName], [FromQuantity], [ToQuantity], [MinQuantity], [UnitQty], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(15, 15, NULL, NULL, 'Cigarettes', NULL, NULL, NULL, NULL, NULL, '22/5/2007 1:26:10 PM', NULL, '22/5/2007 1:26:10 PM')
INSERT INTO [PromoCampaignDet] ([PromoCampaignDetID], [PromoCampaignHdrID], [ItemGroupID], [ItemNo], [CategoryName], [FromQuantity], [ToQuantity], [MinQuantity], [UnitQty], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(16, 16, NULL, NULL, 'Prata', NULL, NULL, NULL, NULL, NULL, '22/5/2007 1:27:56 PM', NULL, '22/5/2007 1:27:56 PM')
INSERT INTO [PromoCampaignDet] ([PromoCampaignDetID], [PromoCampaignHdrID], [ItemGroupID], [ItemNo], [CategoryName], [FromQuantity], [ToQuantity], [MinQuantity], [UnitQty], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES(17, 17, NULL, NULL, 'Alcoholic Beverages', NULL, NULL, NULL, NULL, NULL, '22/5/2007 1:28:43 PM', NULL, '22/5/2007 1:28:43 PM')
SET IDENTITY_INSERT [PromoCampaignDet] OFF 
ALTER TABLE [PromoCampaignDet] CHECK CONSTRAINT ALL
GO



ALTER TABLE [PurchaseOrderDetail] NOCHECK CONSTRAINT ALL
GO

PRINT 'Begin inserting data in PurchaseOrderDetail'
INSERT INTO [PurchaseOrderDetail] ([PurchaseOrderDetailID], [ItemNo], [PurchaseOrderNo], [Quantity], [Price], [Discount], [IsNonGst], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('', 'I00000000018', 'PR0125121300', 12, 1.5000, 3.0000, 1, 'IMPACT-C491D6CC\Albert', '19/5/2007 11:18:06 PM', 'IMPACT-C491D6CC\Albert', '19/5/2007 11:18:06 PM')
ALTER TABLE [PurchaseOrderDetail] CHECK CONSTRAINT ALL
GO



ALTER TABLE [PurchaseOrderHeader] NOCHECK CONSTRAINT ALL
GO

PRINT 'Begin inserting data in PurchaseOrderHeader'
INSERT INTO [PurchaseOrderHeader] ([PurchaseOrderNo], [RequestedBy], [PurchaseOrderDate], [SupplierID], [DepartmentID], [PaymentTermID], [ShipVia], [ShipTo], [DateNeededBy], [TotalDiscount], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('', '', '22/5/2007 12:00:00 AM', 1, 3, NULL, NULL, NULL, NULL, NULL, 'IMPACT-C491D6CC\Albert', '22/5/2007 7:27:33 PM', 'IMPACT-C491D6CC\Albert', '22/5/2007 7:27:33 PM')
INSERT INTO [PurchaseOrderHeader] ([PurchaseOrderNo], [RequestedBy], [PurchaseOrderDate], [SupplierID], [DepartmentID], [PaymentTermID], [ShipVia], [ShipTo], [DateNeededBy], [TotalDiscount], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
VALUES('PR0125121300', 'Albert', '1/5/2007 12:00:00 AM', 1, 3, 1, 'Land', 'SKFIP', '29/5/2007 12:00:00 AM', 10.0000, 'IMPACT-C491D6CC\Albert', '19/5/2007 1:03:08 PM', 'IMPACT-C491D6CC\Albert', '19/5/2007 11:18:30 PM')
ALTER TABLE [PurchaseOrderHeader] CHECK CONSTRAINT ALL
GO



ALTER TABLE [ReceiptDet] NOCHECK CONSTRAINT ALL
GO

PRINT 'Begin inserting data in ReceiptDet'
ALTER TABLE [ReceiptDet] CHECK CONSTRAINT ALL
GO



ALTER TABLE [ReceiptHdr] NOCHECK CONSTRAINT ALL
GO

PRINT 'Begin inserting data in ReceiptHdr'
INSERT INTO [ReceiptHdr] ([ReceiptHdrID], [ReceiptRefNo], [ReceiptDate], [PaymentType], [CashierID], [PointOfSaleID], [OrderHdrID], [Amount], [AmountBeforeRounding], [Tax], [IsVoided], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('07052100020001', 'RCP07052100020001', '21/5/2007 11:47:10 PM', 'CASH', 'admin', 2, '07052100020001', 10.5000, 0.0000, 5,  0, '21/5/2007 11:47:10 PM', 'admin', '21/5/2007 11:47:10 PM', 'admin', '7d5a61a8-d4dc-4989-8f4f-aa54cd672232')
INSERT INTO [ReceiptHdr] ([ReceiptHdrID], [ReceiptRefNo], [ReceiptDate], [PaymentType], [CashierID], [PointOfSaleID], [OrderHdrID], [Amount], [AmountBeforeRounding], [Tax], [IsVoided], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('07052100020002', 'RCP07052100020002', '21/5/2007 11:49:14 PM', 'CASH', 'admin', 2, '07052100020002', 10.5000, 0.0000, 5,  0, '21/5/2007 11:49:37 PM', 'admin', '21/5/2007 11:49:37 PM', 'admin', '5e0d5499-50d4-4045-97be-be43e6d963f7')
INSERT INTO [ReceiptHdr] ([ReceiptHdrID], [ReceiptRefNo], [ReceiptDate], [PaymentType], [CashierID], [PointOfSaleID], [OrderHdrID], [Amount], [AmountBeforeRounding], [Tax], [IsVoided], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('07052100020003', 'RCP07052100020003', '21/5/2007 11:49:57 PM', 'CASH', 'admin', 2, '07052100020003', 10.5000, 0.0000, 5,  0, '21/5/2007 11:49:59 PM', 'admin', '21/5/2007 11:49:59 PM', 'admin', '973b2d58-d9aa-4627-9de1-d0a3302127a1')
INSERT INTO [ReceiptHdr] ([ReceiptHdrID], [ReceiptRefNo], [ReceiptDate], [PaymentType], [CashierID], [PointOfSaleID], [OrderHdrID], [Amount], [AmountBeforeRounding], [Tax], [IsVoided], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('07052100020004', 'RCP07052100020004', '21/5/2007 11:50:51 PM', 'CASH', 'admin', 2, '07052100020004', 10.5000, 0.0000, 5,  0, '21/5/2007 11:50:59 PM', 'admin', '21/5/2007 11:50:59 PM', 'admin', 'b5d1f76c-d01f-4f6c-8633-635c380b9dd3')
INSERT INTO [ReceiptHdr] ([ReceiptHdrID], [ReceiptRefNo], [ReceiptDate], [PaymentType], [CashierID], [PointOfSaleID], [OrderHdrID], [Amount], [AmountBeforeRounding], [Tax], [IsVoided], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('07052200020001', 'RCP07052200020001', '22/5/2007 1:51:02 AM', 'CASH', 'admin', 2, '07052200020001', 36.7000, 0.0000, 5,  0, '22/5/2007 1:51:02 AM', 'admin', '22/5/2007 1:51:02 AM', 'admin', 'ae699c68-ec0b-4e90-ae62-6504ac4b5bac')
INSERT INTO [ReceiptHdr] ([ReceiptHdrID], [ReceiptRefNo], [ReceiptDate], [PaymentType], [CashierID], [PointOfSaleID], [OrderHdrID], [Amount], [AmountBeforeRounding], [Tax], [IsVoided], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('07052200020002', 'RCP07052200020002', '22/5/2007 3:21:00 AM', 'NETS', 'admin', 2, '07052200020002', 33.2500, 0.0000, 5,  0, '22/5/2007 3:21:00 AM', 'admin', '22/5/2007 3:21:00 AM', 'admin', 'ebb1f318-3094-403d-bd26-71d4ca6de86d')
INSERT INTO [ReceiptHdr] ([ReceiptHdrID], [ReceiptRefNo], [ReceiptDate], [PaymentType], [CashierID], [PointOfSaleID], [OrderHdrID], [Amount], [AmountBeforeRounding], [Tax], [IsVoided], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('07052200020003', 'RCP07052200020003', '22/5/2007 3:25:53 AM', 'AMEX', 'admin', 2, '07052200020003', 10.5000, 0.0000, 5,  0, '22/5/2007 3:25:53 AM', 'admin', '22/5/2007 3:25:53 AM', 'admin', 'c8a73435-db3e-4482-b37d-6aa2a340bdca')
INSERT INTO [ReceiptHdr] ([ReceiptHdrID], [ReceiptRefNo], [ReceiptDate], [PaymentType], [CashierID], [PointOfSaleID], [OrderHdrID], [Amount], [AmountBeforeRounding], [Tax], [IsVoided], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('07052200030001', 'RCP07052200030001', '22/5/2007 3:27:43 AM', 'NETS', 'admin', 3, '07052200030001', 17.6500, 0.0000, 5,  0, '22/5/2007 3:27:43 AM', 'admin', '22/5/2007 3:27:43 AM', 'admin', 'e84354f9-e975-412b-8030-7c0ecb36e045')
INSERT INTO [ReceiptHdr] ([ReceiptHdrID], [ReceiptRefNo], [ReceiptDate], [PaymentType], [CashierID], [PointOfSaleID], [OrderHdrID], [Amount], [AmountBeforeRounding], [Tax], [IsVoided], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('07052200030002', 'RCP07052200030002', '22/5/2007 3:27:55 AM', 'VISA/MASTERCARD', 'admin', 3, '07052200030002', 39.0000, 0.0000, 5,  0, '22/5/2007 3:27:55 AM', 'admin', '22/5/2007 3:27:55 AM', 'admin', '64f52924-9ea5-435f-ae38-40134f5010f9')
INSERT INTO [ReceiptHdr] ([ReceiptHdrID], [ReceiptRefNo], [ReceiptDate], [PaymentType], [CashierID], [PointOfSaleID], [OrderHdrID], [Amount], [AmountBeforeRounding], [Tax], [IsVoided], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [UniqueID])
VALUES('07052200030003', 'RCP07052200030003', '22/5/2007 3:28:05 AM', 'CASH', 'admin', 3, '07052200030003', 11.5500, 0.0000, 5,  0, '22/5/2007 3:28:05 AM', 'admin', '22/5/2007 3:28:05 AM', 'admin', 'd81e236b-62a3-4467-b446-471601e8dc4f')
ALTER TABLE [ReceiptHdr] CHECK CONSTRAINT ALL
GO



