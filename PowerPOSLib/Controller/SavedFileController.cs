using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic;

namespace PowerPOS
{
    public partial class SavedFileController
    {
        public static bool DeleteSavedFileAll(out string status)
        {
            status = "";
            try
            {
                Query qrUpdate = new Query("SavedFiles");
                qrUpdate.AddUpdateSetting(SavedFile.Columns.Deleted, true);
                qrUpdate.AddUpdateSetting(SavedFile.Columns.ModifiedOn, DateTime.Now);
                qrUpdate.AddWhere(SavedFile.Columns.Deleted, false);
                qrUpdate.Execute();
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog("Error Update Saved File :" + ex.Message);
                status = ex.Message;
                return false;
            }           
        }

        public static bool DeleteSavedFileByName(string saveName, out string status)
        {
            status = "";
            try
            {
                Query qrUpdate = new Query("SavedFiles");
                qrUpdate.AddUpdateSetting(SavedFile.Columns.Deleted, true);
                qrUpdate.AddUpdateSetting(SavedFile.Columns.ModifiedOn, DateTime.Now);
                qrUpdate.AddWhere(SavedFile.Columns.SaveName, saveName);
                qrUpdate.Execute();
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog("Error Update Saved File :" + ex.Message);
                status = ex.Message;
                return false;
            }
        }
    }
}
