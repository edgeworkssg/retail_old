using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SubSonic;

namespace PowerPOS
{
    public partial class ServerQuickRefController
    {
        public static QueryCommand Generate_Upsert_OrderHdr(int pointofsaleID, DateTime modifiedOn)
        {
            string upsert = @"
                                IF EXISTS (SELECT * FROM ServerQuickRef WHERE TableName = 'OrderHdr' AND PointOfSaleID = @PointOfSaleID)
                                    UPDATE ServerQuickRef SET LastModifiedon = @ModifiedOn
                                    WHERE TableName = 'OrderHdr' AND PointOfSaleID = @PointOfSaleID AND LastModifiedon < @ModifiedOn
                                ELSE
                                    INSERT INTO ServerQuickRef (TableName, LastModifiedon, PointOfSaleID)
                                    VALUES ('OrderHdr', @ModifiedOn, @PointOfSaleID)
                             ";
            QueryCommand cmdUpsert = new QueryCommand(upsert, "PowerPOS");
            cmdUpsert.Parameters.Add("@PointOfSaleID", pointofsaleID, DbType.Int32);
            cmdUpsert.Parameters.Add("@ModifiedOn", modifiedOn, DbType.DateTime);
            return cmdUpsert;
        }
    }
}
