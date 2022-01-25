using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;

using PowerPOS.Container;

namespace PowerPOS
{
    public class HotKeysController
    {
        DataSet ds;
        public string GetHotKeyItem(string HotKey)
        {
            try
            {                
                ds = new DataSet();
                ds.ReadXml(AppDomain.CurrentDomain.BaseDirectory + "\\POSDevices\\HotKeys.xml");
                DataRow[] dr;

                dr = ds.Tables[0].Select("keyname='" + HotKey + "'");
                if (dr != null)
                {
                    for (int i = 0; i < dr.Length; i++)
                    {
                        return dr[i]["itemno"].ToString();
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                throw ex;
            }              
            finally {
                ds.Dispose();
            }            
        }
        
        public DataTable GetDataTables()
        {
            ds = new DataSet();
            ds.ReadXml(AppDomain.CurrentDomain.BaseDirectory + "\\POSDevices\\HotKeys.xml");
            if (ds != null)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }
        public void AssignHotkey(string HotKey, string ItemNo, string itemname)
        {
            try
            {                
                ds = new DataSet();
                ds.ReadXml(AppDomain.CurrentDomain.BaseDirectory + "\\POSDevices\\HotKeys.xml");
                DataRow[] dr;

                dr = ds.Tables[0].Select("keyname='" + HotKey + "'");
                if (dr != null)
                {
                    for (int i = 0; i < dr.Length; i++)
                    {
                        dr[i]["itemno"] = ItemNo;
                        dr[i]["itemname"] = itemname;
                    }
                }
                ds.WriteXml(AppDomain.CurrentDomain.BaseDirectory + "\\POSDevices\\HotKeys.xml");                
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                throw ex;
            }
            finally
            {
                ds.Dispose();
            } 
        }

        public ArrayList GetHotKeyList()
        {
            try
            {                
                ds = new DataSet();
                ds.ReadXml(AppDomain.CurrentDomain.BaseDirectory + "\\POSDevices\\HotKeys.xml");
                ArrayList ar = new ArrayList();
                ar.Add("");
                if (ds != null && ds.Tables.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        ar.Add(ds.Tables[0].Rows[i]["keyname"].ToString());
                    }                    
                    return ar;
                }                
                else 
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                throw ex;
            }
            finally
            {
                ds.Dispose();
            } 
        }

        public HotKeysController()
        {                        
        }
    }
}
