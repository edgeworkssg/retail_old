using System;
using SubSonic;

namespace PowerPOS
{
    public partial class LineInfoController
    {
        public static void CreateLineInfoTable()
        {

            string SQL = "IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LineInfo]') AND type in (N'U')) " +
            "BEGIN " +
                "CREATE TABLE [dbo].[LineInfo]( " +
                "[LineInfoID] [int] IDENTITY(1,1) NOT NULL, " +
                "[LineInfoName] [nvarchar](100) NULL, " +
                "[CreatedOn] [datetime] NULL, " +
                "[CreatedBy] [varchar](50) NULL, " +
                "[ModifiedOn] [datetime] NULL, " +
                "[ModifiedBy] [varchar](50) NULL, " +
                "[UniqueID] [uniqueidentifier] NULL, " +
                "[userfld1] [varchar](50) NULL, " +
                "[userfld2] [varchar](50) NULL," +
                "[userfld3] [varchar](50) NULL, " +
                "[Deleted] [bit] NULL, " +
                "CONSTRAINT [LineInfoID] PRIMARY KEY CLUSTERED ([LineInfoID] ASC) " +
                "WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]) ON [PRIMARY] " +
            "END";
            DataService.ExecuteQuery(new QueryCommand(SQL));
        }
    }
}
