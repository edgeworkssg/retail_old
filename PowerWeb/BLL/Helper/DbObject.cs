using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using SubSonic;

namespace PowerWeb.BLL.Helper
{
    public class DbObject
    {
        public void CreateDashboardStructure()
        {
            string strSql = @"
                CREATE TABLE [dbo].[Dashboard](
	                [ID] [int] IDENTITY(1,1) NOT NULL,
	                [Title] [nvarchar](500) NULL,
	                [SubTitle] [nvarchar](500) NULL,
	                [Description] [nvarchar](max) NULL,
	                [PlotType] [varchar](200) NULL,
	                [PlotOption] [nvarchar](max) NULL,
	                [Width] [varchar](500) NULL,
	                [Height] [varchar](500) NULL,
	                [SQLString] [nvarchar](max) NULL,
	                [IsInline] [bit] NULL,
	                [BreakAfter] [bit] NULL,
	                [BreakBefore] [bit] NULL,
	                [ColumnStyle] [nvarchar](max) NULL,
	                [IsEnable] [bit] NULL,
	                [DisplayOrder] [int] NULL,
	                [CreatedOn] [datetime] NULL,
	                [CreatedBy] [varchar](50) NULL,
	                [ModifiedOn] [datetime] NULL,
	                [ModifiedBy] [varchar](50) NULL,
	                [UniqueID] [uniqueidentifier] NULL,
	                [Deleted] [bit] NULL,
                 CONSTRAINT [PK_Dashboard] PRIMARY KEY CLUSTERED 
                (
	                [ID] ASC
                )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
                ) ON [PRIMARY]
            ";

            try
            {
                QueryCommand cmd = new QueryCommand(strSql);
                object resultMV = DataService.ExecuteScalar(cmd);
            }
            catch (Exception) { }
        }

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

                          	"CREATE TABLE [dbo].[CommissionBasedOnQty](																							  "   +
                            "	[UniqueID] [int] IDENTITY(1,1) NOT NULL,                                                                                          "   +
                            "	[SalesGroupID] [int] NOT NULL,                                                                                                    "   +
                            "	[ItemNo] [varchar](50) NOT NULL,                                                                                                  "   +
                            "	[Quantity] [decimal](5, 0) NOT NULL,                                                                                              "   +
                            "	[AmountCommission] [decimal](18, 6) NOT NULL,                                                                                     "   +
                            "	[CreatedBy] [varchar](50) NOT NULL,                                                                                               "   +
                            "	[CreatedOn] [datetime] NOT NULL,                                                                                                  "   +
                            "	[ModifiedBy] [varchar](50) NULL,                                                                                                  "   +
                            "	[ModifiedOn] [datetime] NULL,                                                                                                     "   +
                            " CONSTRAINT [PK_CommissionBasedOnQty_1] PRIMARY KEY CLUSTERED                                                                        "   +
                            "(                                                                                                                                    "   +
                            "	[UniqueID] ASC                                                                                                                    "   +
                            ")WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]"   +
                            ") ON [PRIMARY]																														  "   +																												 

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
        }
    }
}
