using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PowerWeb.BLL.Helper
{
    public class Utility
    {
        public static void DecryptDefaultConnString()
        {
            if (!SubSonic.DataService.Provider.DefaultConnectionString.Contains("Integrated Security"))
            {
                string[] splitDataSource = SubSonic.DataService.Provider.DefaultConnectionString.Split(';');
                if (string.IsNullOrEmpty(splitDataSource.Last()))
                {
                    splitDataSource = splitDataSource.Take(splitDataSource.Length - 1).ToArray();
                }
                int startIndex = splitDataSource[splitDataSource.Length - 1].IndexOf("=", 0) + 1;
                string splitPassword = splitDataSource[splitDataSource.Length - 1].Substring(startIndex, splitDataSource[splitDataSource.Length - 1].Length - startIndex);
                if (EncryptionLib.ChiperHelper.TripleDESEncryption.TryDecrypt(splitPassword))
                {
                    string decryptedPassword = EncryptionLib.ChiperHelper.TripleDESEncryption.Decrypt(splitPassword, Properties.Settings.Default.SensitiveSalt);
                    splitDataSource[splitDataSource.Length - 1] = string.Format("{0}{1}", splitDataSource[splitDataSource.Length - 1].Substring(0, startIndex), decryptedPassword);
                    SubSonic.DataService.Provider.DefaultConnectionString = string.Join(";", splitDataSource);
                }
            }
        }
    }
}
