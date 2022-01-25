using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;
namespace PowerPOS.Controller
{
    public class GSTOverride
    {
        //0- no override
        //1- all GST Exclusive
        //2 - all GST inclusive
        //3 - all no GST
        public static int GSTRule;

        public static void LoadGSTRule()
        {
            DataSet ds = new DataSet();

            try
            {
                //if file exist, Load Text file
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\GSTOverride.xml"))
                {

                    ds.ReadXml(AppDomain.CurrentDomain.BaseDirectory + "\\GSTOverride.xml");
                    int.TryParse(ds.Tables[0].Rows[0].ItemArray[0].ToString(), out GSTRule);
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);                
            }
        }
    }
}
