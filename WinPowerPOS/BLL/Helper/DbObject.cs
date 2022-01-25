using System;
using System.Data;
using System.Configuration;
using System.Linq;
//using System.Web.UI.HtmlControls;
//using System.Web.UI.WebControls;
//using System.Web.UI.WebControls.WebParts;
//using System.Xml.Linq;
using SubSonic;

namespace PowerPOS.BLL.Helper
{
    public class DbObject
    {
        public void CreateCommissionByPercentageStructure()
        {
            string strSql = "if object_id('CommissionBasedOnPercentage') is null " +
                            "begin " +

                            "CREATE TABLE [dbo].[CommissionBasedOnPercentage](																					  " +
                            "	[UniqueID] [int] IDENTITY(1,1) NOT NULL,                                                                                          " +
                            "	[SalesGroupID] [int] NOT NULL,                                                                                                    " +
                            "	[CommissionType] [varchar](20) NOT NULL,                                                                                          " +
                            "	[LowerLimit] [decimal](18, 3) NULL,                                                                                               " +
                            "	[UpperLimit] [decimal](18, 3) NULL,                                                                                               " +
                            "	[PercentCommission] [decimal](6, 3) NOT NULL,                                                                                     " +
                            "	[CreatedBy] [varchar](50) NOT NULL,                                                                                               " +
                            "	[CreatedOn] [datetime] NOT NULL,                                                                                                  " +
                            "	[ModifiedBy] [varchar](50) NULL,                                                                                                  " +
                            "	[ModifiedOn] [datetime] NULL,                                                                                                     " +
                            " CONSTRAINT [PK_CommissionBasedOnPercentage_1] PRIMARY KEY CLUSTERED                                                                 " +
                            "(                                                                                                                                    " +
                            "	[UniqueID] ASC                                                                                                                    " +
                            ")WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]" +
                            ") ON [PRIMARY]																														  " +



                            "end";

            QueryCommand cmd = new QueryCommand(strSql);
            object resultMV = DataService.ExecuteScalar(cmd);
        }

        public void CreateCommissionByQtyStructure()
        {
            string strSql = "if object_id('CommissionBasedOnQty') is null " +
                            "begin " +

                            "CREATE TABLE [dbo].[CommissionBasedOnQty](																							  " +
                            "	[UniqueID] [int] IDENTITY(1,1) NOT NULL,                                                                                          " +
                            "	[SalesGroupID] [int] NOT NULL,                                                                                                    " +
                            "	[ItemNo] [varchar](50) NOT NULL,                                                                                                  " +
                            "	[Quantity] [decimal](5, 0) NOT NULL,                                                                                              " +
                            "	[AmountCommission] [decimal](18, 6) NOT NULL,                                                                                     " +
                            "	[CreatedBy] [varchar](50) NOT NULL,                                                                                               " +
                            "	[CreatedOn] [datetime] NOT NULL,                                                                                                  " +
                            "	[ModifiedBy] [varchar](50) NULL,                                                                                                  " +
                            "	[ModifiedOn] [datetime] NULL,                                                                                                     " +
                            " CONSTRAINT [PK_CommissionBasedOnQty_1] PRIMARY KEY CLUSTERED                                                                        " +
                            "(                                                                                                                                    " +
                            "	[UniqueID] ASC                                                                                                                    " +
                            ")WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]" +
                            ") ON [PRIMARY]																														  " +

                            "end";

            QueryCommand cmd = new QueryCommand(strSql);
            object resultMV = DataService.ExecuteScalar(cmd);
        }

        public void CreatePurchaseOrderHdrStructure()
        {
            string strSql = "if object_id('PurchaseOrderHdr') is null " +
                            "begin " +

                            "CREATE TABLE [dbo].[PurchaseOrderHdr]( " +
	                                "[PurchaseOrderHdrRefNo] [varchar](50) NOT NULL, " +
	                                "[PurchaseOrderDate] [datetime] NOT NULL, " +
	                                "[UserName] [varchar](50) NULL, " +
	                                "[ExchangeRate] [float] NOT NULL, " +
	                                "[Supplier] [varchar](50) NULL, " +
	                                "[FreightCharge] [money] NULL, " + 
	                                "[DeliveryCharge] [money] NULL, " + 
	                                "[Tax] [decimal](18, 2) NULL, " +
	                                "[Discount] [decimal](18, 2) NULL, " + 
	                                "[Remark] [nvarchar](max) NOT NULL, " +
	                                "[InventoryLocationID] [int] NULL, " +
	                                "[DepartmentID] [int] NULL, " +
	                                "[CreatedOn] [datetime] NULL, "+
	                                "[ModifiedOn] [datetime] NULL, " +
	                                "[CreatedBy] [varchar](50) NULL, " +
	                                "[ModifiedBy] [varchar](50) NULL, " +
	                                "[UniqueID] [uniqueidentifier] NOT NULL, " +
	                                "[userfld1] [varchar](50) NULL, " +
	                                "[userfld2] [varchar](50) NULL, " +
	                                "[userfld3] [varchar](50) NULL, " +
	                                "[userfld4] [varchar](50) NULL, " + 
	                                "[userfld5] [varchar](50) NULL, " +
	                                "[userfld6] [varchar](50) NULL, " +
	                                "[userfld7] [varchar](50) NULL, " +
	                                "[userfld8] [varchar](50) NULL, " +
	                                "[userfld9] [varchar](50) NULL, " +
	                                "[userfld10] [varchar](50) NULL, " +
	                                "[userflag1] [bit] NULL, " + 
	                                "[userflag2] [bit] NULL, " +
	                                "[userflag3] [bit] NULL, " +
	                                "[userflag4] [bit] NULL, " + 
	                                "[userflag5] [bit] NULL, " +
	                                "[userfloat1] [money] NULL, " +
	                                "[userfloat2] [money] NULL, " +
	                                "[userfloat3] [money] NULL, " +
	                                "[userfloat4] [money] NULL, " +
	                                "[userfloat5] [money] NULL, " +
	                                "[userint1] [int] NULL, " +
	                                "[userint2] [int] NULL, " +
	                                "[userint3] [int] NULL, " +
	                                "[userint4] [int] NULL, " +
	                                "[userint5] [int] NULL, " +
	                                "[CurrencyId] [int] NULL, " +
	                                "[TmpSavedData] [int] NULL, " +
	                                "[Deleted] [bit] NULL, " +
                                 "CONSTRAINT [PK_PurchaseOrderHdr] PRIMARY KEY CLUSTERED " +
                                "( " +
	                                "[PurchaseOrderHdrRefNo] ASC " +
                                ")WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY] " +
                                ") ON [PRIMARY] " +

                                
                                "ALTER TABLE [dbo].[PurchaseOrderHdr]  WITH CHECK ADD  CONSTRAINT [FK_PurchaseOrderHdr_Currencies] FOREIGN KEY([CurrencyId]) " +
                                "REFERENCES [dbo].[Currencies] ([CurrencyId]) " +
                                

                                "ALTER TABLE [dbo].[PurchaseOrderHdr] CHECK CONSTRAINT [FK_PurchaseOrderHdr_Currencies] " +
                                

                                "ALTER TABLE [dbo].[PurchaseOrderHdr]  WITH CHECK ADD  CONSTRAINT [FK_PurchaseOrderHdr_Department] FOREIGN KEY([DepartmentID]) " +
                                "REFERENCES [dbo].[Department] ([DepartmentID]) " +
                                

                                "ALTER TABLE [dbo].[PurchaseOrderHdr] CHECK CONSTRAINT [FK_PurchaseOrderHdr_Department] " +
                                

                                "ALTER TABLE [dbo].[PurchaseOrderHdr]  WITH NOCHECK ADD  CONSTRAINT [FK_PurchaseOrderHdr_InventoryLocation] FOREIGN KEY([InventoryLocationID]) " +
                                "REFERENCES [dbo].[InventoryLocation] ([InventoryLocationID]) " +
                                "ON UPDATE CASCADE " +
                                

                                "ALTER TABLE [dbo].[PurchaseOrderHdr] CHECK CONSTRAINT [FK_PurchaseOrderHdr_InventoryLocation] " +
                                

                                "ALTER TABLE [dbo].[PurchaseOrderHdr] ADD  CONSTRAINT [DF_PurchaseOrderHdr_DeliveryCharge]  DEFAULT ((0)) FOR [DeliveryCharge] " +
                                

                                "ALTER TABLE [dbo].[PurchaseOrderHdr] ADD  CONSTRAINT [DF_PurchaseOrderHdr_Tax]  DEFAULT ((0)) FOR [Tax] " +
                                

                                "ALTER TABLE [dbo].[PurchaseOrderHdr] ADD  CONSTRAINT [DF_PurchaseOrderHdr_Discount]  DEFAULT ((0)) FOR [Discount] " +
                                

                                "ALTER TABLE [dbo].[PurchaseOrderHdr] ADD  CONSTRAINT [DF_PurchaseOrderHdr_UniqueID]  DEFAULT (newid()) FOR [UniqueID] " +
                                
                            "end";

            QueryCommand cmd = new QueryCommand(strSql);
            object resultMV = DataService.ExecuteScalar(cmd);
        }

        public void CreatePurchaseOrderDetStructure()
        {
            string strSql = "if object_id('PurchaseOrderDet') is null " +
                            "begin " +

                            @"

CREATE TABLE [dbo].[PurchaseOrderDet]( 
	[PurchaseOrderDetRefNo] [varchar](50) NOT NULL, 
	[ItemNo] [varchar](50) NOT NULL, 
	[PurchaseOrderHdrRefNo] [varchar](50) NOT NULL, 
	[ExpiryDate] [datetime] NULL, 
	[Quantity] [int] NOT NULL, 
	[RemainingQty] [int] NOT NULL, 
	[FactoryPrice] [money] NOT NULL, 
	[GST] [float] NOT NULL, 
	[CostOfGoods] [money] NOT NULL, 
	[Remark] [nvarchar](max) NULL, 
	[Discount] [decimal](18, 2) NULL, 
	[CreatedBy] [varchar](50) NULL, 
	[CreatedOn] [datetime] NULL, 
	[ModifiedBy] [varchar](50) NULL, 
	[ModifiedOn] [datetime] NULL, 
	[UniqueID] [uniqueidentifier] NOT NULL, 
	[userfld1] [varchar](50) NULL, 
	[userfld2] [varchar](50) NULL, 
	[userfld3] [varchar](50) NULL, 
	[userfld4] [varchar](50) NULL, 
	[userfld5] [varchar](50) NULL, 
	[userfld6] [varchar](50) NULL, 
	[userfld7] [varchar](50) NULL, 
	[userfld8] [varchar](50) NULL, 
	[userfld9] [varchar](50) NULL, 
	[userfld10] [varchar](50) NULL, 
	[userflag1] [bit] NULL, 
	[userflag2] [bit] NULL, 
	[userflag3] [bit] NULL, 
	[userflag4] [bit] NULL, 
	[userflag5] [bit] NULL, 
	[userfloat1] [money] NULL, 
	[userfloat2] [money] NULL, 
	[userfloat3] [money] NULL, 
	[userfloat4] [money] NULL, 
	[userfloat5] [money] NULL, 
	[userint1] [int] NULL, 
	[userint2] [int] NULL, 
	[userint3] [int] NULL, 
	[userint4] [int] NULL, 
	[userint5] [int] NULL, 
	[BalanceBefore] [int] NOT NULL, 
	[BalanceAfter] [int] NOT NULL, 
	[Deleted] [bit] NULL, 
	[stockinrefno] [varchar](20) NULL, 
	[isdiscrepancy] [bit] NULL, 
 CONSTRAINT [PK_PurchaseOrderDet] PRIMARY KEY CLUSTERED  
( 
	[PurchaseOrderDetRefNo] ASC 
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY] 
) ON [PRIMARY] 
 


ALTER TABLE [dbo].[PurchaseOrderDet]  WITH NOCHECK ADD  CONSTRAINT [FK_PurchaseOrder_Item] FOREIGN KEY([ItemNo]) 
REFERENCES [dbo].[Item] ([ItemNo]) 
ON UPDATE CASCADE 


ALTER TABLE [dbo].[PurchaseOrderDet] CHECK CONSTRAINT [FK_PurchaseOrder_Item] 


ALTER TABLE [dbo].[PurchaseOrderDet]  WITH NOCHECK ADD  CONSTRAINT [FK_PurchaseOrderDet_PurchaseOrderHdr] FOREIGN KEY([PurchaseOrderHdrRefNo]) 
REFERENCES [dbo].[PurchaseOrderHdr] ([PurchaseOrderHdrRefNo]) 
ON UPDATE CASCADE 
ON DELETE CASCADE 


ALTER TABLE [dbo].[PurchaseOrderDet] CHECK CONSTRAINT [FK_PurchaseOrderDet_PurchaseOrderHdr] 

 
ALTER TABLE [dbo].[PurchaseOrderDet] ADD  CONSTRAINT [DF_PurchaseOrderDet_UniqueID]  DEFAULT (newid()) FOR [UniqueID] 
	" + 
                             "end";

            QueryCommand cmd = new QueryCommand(strSql);
            object resultMV = DataService.ExecuteScalar(cmd);
        }

        public void AlterCommissionByQtyStructure_1()
        {
            try
            {
                string strSql = "" +
                                "alter table CommissionBasedOnQty " +
                                "add CommissionType varchar(25)";

                QueryCommand cmd = new QueryCommand(strSql);
                object resultMV = DataService.ExecuteScalar(cmd);
            }
            catch (Exception) { }

            try
            {
                string strSql = "" +
                                "alter table CommissionBasedOnQty " +
                                "add UniqueID int";

                QueryCommand cmd = new QueryCommand(strSql);
                object resultMV = DataService.ExecuteScalar(cmd);
            }
            catch (Exception) { }
        }
    }
}
