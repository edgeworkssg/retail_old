using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using PowerPOS.Container;

namespace PowerPOS
{
    public partial class SavedItemController
    {
        public bool SaveObject(object obj, string saveName, string movementType, 
            string path, string remark, bool autosave, out string status)
        {
            try
            {
                string tmp = path + "\\S" + saveName + ".bin";
                if (File.Exists(tmp))
                {
                    File.Delete(tmp);
                }
                Stream a = File.OpenWrite(tmp);

                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(a, obj);
                a.Close();

                SavedFile saved = new SavedFile(SavedFile.Columns.SaveName, saveName);
                saved.SaveName = saveName;
                saved.MovementType = movementType;
                saved.SavedDate = DateTime.Now;
                saved.SavedBy = UserInfo.username;
                saved.Deleted = false;
                if (autosave) remark = "AUTOSAVE_A031179:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + remark;
                saved.Remark = remark;
                saved.Save(UserInfo.username);
                status = "";
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return false;
            }
        }

        public object LoadObject(string saveName, string path)
        {
            try
            {
                //check if file name exist
                string tmp = path + "\\S" + saveName + ".bin";

                //check if file name exist
                if (File.Exists(tmp))
                {
                    FileStream file = new FileStream
                        (tmp, FileMode.Open);

                    BinaryFormatter bf = new BinaryFormatter();
                    object obj = bf.Deserialize(file);
                    file.Close();
                    return obj;
                }
                return null;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
        }

        public bool removeFile(string saveName)
        {
            try
            {
                // Validation for save name
                if (saveName == "")
                    return false;

                SavedFile saved = new SavedFile(SavedFile.Columns.SaveName, saveName);
                if (saved == null || saved.SaveName == null || saved.SaveName == "")
                    return false;

                saved.Deleted = true;
                saved.Save(UserInfo.username);
                return true;

            }
            catch (Exception ex)
            {
                Logger.writeLog("Error When Trying to remove saved file." + ex.Message);
                return false;

            }

        }
    }
}
