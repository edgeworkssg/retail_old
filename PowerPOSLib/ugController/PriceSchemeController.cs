using System;
using SubSonic;

namespace PowerPOS
{
    public partial class PriceSchemeController
    {
        public static void CreatePriceSchemeTable()
        {
            string sqlCheckPriceSchemeTable =
            "IF (NOT EXISTS (SELECT * " +
                 "FROM INFORMATION_SCHEMA.TABLES " +
                 "WHERE TABLE_NAME = 'PriceScheme')) " +
            "BEGIN " +
                "CREATE TABLE [dbo].[PriceScheme]( " +
                    "[SchemeID] [varchar](50) NOT NULL, " +
                    "[ItemNo] [varchar](50) NULL, " +
                    "[Price] [money] NULL " +
                ") " +
            "END";
            QueryCommand checkPriceSchemeTable = new QueryCommand(sqlCheckPriceSchemeTable, "PowerPOS");
            DataService.ExecuteQuery(checkPriceSchemeTable);

        }

    }
}
