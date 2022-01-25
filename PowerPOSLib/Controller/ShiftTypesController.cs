using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic;
using System.Data;
using PowerPOS.Container;
using System.Collections;

namespace PowerPOS
{
    public class ShiftTypesController
    {
        public static bool LoadShiftTypes(out string status)
        {
            try
            {
                status = "";
                if (ShiftInfo.ShiftTypes == null)
                {
                    DataSet ds = new DataSet();
                    ds.ReadXml(AppDomain.CurrentDomain.BaseDirectory + "\\ShiftType.xml");
                    ShiftInfo.ShiftTypes = ds.Tables[0];
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return false;
            }
        }
        public static DataTable FetchShiftByID(string ID)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Name", typeof(string));
            if (ID == "")
            {
                return dt;
            }
            DataRow[] dr = ShiftInfo.ShiftTypes.Select("Type = '" + ID.ToString() + "'");
            if (dr.Length > 0)
            {
                for (int i = 0; i < dr.Length; i++)
                {
                    dt.Rows.Add(dr[i]["Name"].ToString());
                }
                return dt;
            }
            else
            {
                return dt;
            }
        }

    }
}
