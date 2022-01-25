using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;
using System.Data;
using SubSonic;

namespace PowerPOS
{
    [System.ComponentModel.DataObject]
    public partial class AttributesMapController
    {
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public DataTable FetchByItemNo(string ItemNo)
        {
            string QryTxt =
                "SELECT ItemAttributesMap.ItemAttributesMapID, AttributesGroup.AttributesGroupCode, AttributesGroup.AttributesGroupName " +
                "FROM ItemAttributesMap INNER JOIN " +
                    "AttributesGroup ON ItemAttributesMap.AttributesGroupCode = AttributesGroup.AttributesGroupCode " +
                "WHERE (ItemAttributesMap.ItemNo = @ItemNo)";

            QueryCommand Cmd = new QueryCommand(QryTxt);
            Cmd.AddParameter("@ItemNo", ItemNo, DbType.String);
            IDataReader Rdr = SubSonic.DataService.GetReader(Cmd);

            DataTable Dt = new DataTable();
            Dt.Load(Rdr);

            return Dt;
        }

        [DataObjectMethod(DataObjectMethodType.Insert, true)]
        public void Sub_Insert(string ItemNo, string AttributesGroupCode, string UserID)
        {
            string QueryStr;
            QueryCommand Cmd;

            QueryStr = "SELECT COUNT(*) FROM Item WHERE ItemNo = @ItemNo;";
            Cmd = new QueryCommand(QueryStr);

            Cmd.AddParameter("@ItemNo", ItemNo);

            if (((int)SubSonic.DataService.ExecuteScalar(Cmd)) < 1)
                throw new Exception("(warning)Item No is not found");

            QueryStr = "SELECT COUNT(*) FROM AttributesGroup WHERE AttributesGroupCode = @AttributesGroupCode;";
            Cmd = new QueryCommand(QueryStr);

            Cmd.AddParameter("@AttributesGroupCode", AttributesGroupCode, DbType.String);

            if (((int)SubSonic.DataService.ExecuteScalar(Cmd)) < 1)
                throw new Exception("(warning)Attributes Group is not found");

            QueryStr = "SELECT COUNT(*) FROM ItemAttributesMap WHERE ItemAttributesMapID = @ItemNo + @AttributesGroupCode;";
            Cmd = new QueryCommand(QueryStr);

            Cmd.AddParameter("@ItemNo", ItemNo, System.Data.DbType.String);
            Cmd.AddParameter("@AttributesGroupCode", AttributesGroupCode, DbType.String);

            if (((int)SubSonic.DataService.ExecuteScalar(Cmd)) > 0)
                throw new Exception("(warning)Attributes Group has been listed on the database");

            QueryStr =
                    "INSERT INTO [ItemAttributesMap] " +
                    "([ItemAttributesMapID],[ItemNo],[AttributesGroupCode],[CreatedOn],[CreatedBy],[ModifiedOn],[ModifiedBy],[Deleted]) " +
                    "VALUES " +
                    "(@ItemNo + @AttributesGroupCode,@ItemNo,@AttributesGroupCode,GETDATE(),@UserID,GETDATE(),@UserID,0)";
            Cmd = new QueryCommand(QueryStr);

            Cmd.AddParameter("@ItemNo", ItemNo, System.Data.DbType.String);
            Cmd.AddParameter("@AttributesGroupCode", AttributesGroupCode, System.Data.DbType.String);
            Cmd.AddParameter("@UserID", UserID, System.Data.DbType.String);

            SubSonic.DataService.ExecuteQuery(Cmd);
        }

        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public void Sub_Delete(string ItemNo, string AttributesGroupCode)
        {
            try
            {
                string QueryStr =
                    "DELETE FROM [ItemAttributesMap] " +
                    "WHERE ItemAttributesMapID = @ItemNo + @AttributesGroupCode";

                QueryCommand Cmd = new QueryCommand(QueryStr);

                Cmd.AddParameter("@ItemNo", ItemNo, System.Data.DbType.String);
                Cmd.AddParameter("@AttributesGroupCode", AttributesGroupCode, System.Data.DbType.String);

                SubSonic.DataService.ExecuteQuery(Cmd);
            }
            catch (Exception X)
            {
                Logger.writeLog(X);
            }
        }
    }
}
