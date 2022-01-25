using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using PowerPOS;
namespace PowerPOSLib.Controller
{
    public class ExchangeRateController
    {
        Hashtable exchangeRate;
        public void Save(string filename)
        {
            FileStream fs = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write);
            try
            {                                
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, exchangeRate);
            }
            finally
            {
                fs.Close();
            }
        }

        public void Load(string filename)
        {
            if (File.Exists(filename))
            {
                FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
                try
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    exchangeRate = (Hashtable)bf.Deserialize(fs);
                }
                catch (Exception ex)
                {
                    Logger.writeLog(ex);
                }
                finally
                {
                    fs.Close();
                }
            }
            else
            {
               //Create hash table
                exchangeRate = new Hashtable();
            }
        }

        public Hashtable GetHashTable()
        {
            return exchangeRate;
        }

        
        public void SetHashTable(Hashtable ht)
        {
            exchangeRate = ht;
        }
    }
}
