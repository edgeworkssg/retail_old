using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using SubSonic;
using System.ComponentModel;
using System.Data;

namespace PowerPOS
{
    public partial class AttributesGroupController
    {
        public static ArrayList FetchAttGroupName_Chinese()
        {
            ArrayList ar = new ArrayList();
            ar.Add("");
            Query qr = AttributesGroup.CreateQuery();
            qr.QueryType = QueryType.Select;
            qr.SelectList = AttributesGroup.Columns.AttributesGroupName;
            IDataReader rdr = qr.ORDER_BY(AttributesGroup.Columns.AttributesGroupName).ExecuteReader();
            while (rdr.Read())
            {
                ar.Add(rdr.GetValue(0).ToString());
            }

            return ar;
        }

        public static ArrayList FetchAttGroupName_English()
        {
            ArrayList ar = new ArrayList();
            ar.Add("");
            Query qr = AttributesGroup.CreateQuery();
            qr.QueryType = QueryType.Select;
            qr.SelectList = AttributesGroup.Columns.UserField2;
            IDataReader rdr = qr.ORDER_BY(AttributesGroup.Columns.UserField2).ExecuteReader();
            while (rdr.Read())
            {
                ar.Add(rdr.GetValue(0).ToString());
            }

            return ar;
        }

        public AttributesGroupCollection FetchByName_Chinese(object name)
        {
            AttributesGroupCollection coll = new AttributesGroupCollection().
                Where(AttributesGroup.Columns.AttributesGroupName, Comparison.Like, name + "%").
                Load();
            coll.Sort(AttributesGroup.Columns.AttributesGroupName, true);
            return coll;
        }
        public AttributesGroupCollection FetchByName_English(object name)
        {
            AttributesGroupCollection coll = new AttributesGroupCollection().
                Where(AttributesGroup.Columns.UserField2, Comparison.Like, name + "%").
                Load();
            coll.Sort(AttributesGroup.Columns.UserField2, true);
            return coll;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public DataTable FetchAttributesGroup()
        {
            string QryTxt =
                "SELECT * FROM AttributesGroup";

            QueryCommand Cmd = new QueryCommand(QryTxt);
            IDataReader Rdr = SubSonic.DataService.GetReader(Cmd);

            DataTable Dt = new DataTable();
            Dt.Load(Rdr);

            return Dt;
        }

        public static string getNewAttGroupRefNo()
        {
            /// Assumption:
            /// Attributes Group Code are {a,b,c,d,e,...}
            /// 1 Char Value, only LowerCase;

            int runningNo = 0;
            string Qry =
                "SELECT MAX(AttributesGroupCode) FROM AttributesGroup";
            QueryCommand Cmd = new QueryCommand(Qry);
            object obj = DataService.ExecuteScalar(Cmd);
            if (obj == null || obj.ToString() == "")
                return "a";
            else
                return char.ConvertFromUtf32(char.ConvertToUtf32(obj.ToString(), 0) + 1);
        }
    }
}
