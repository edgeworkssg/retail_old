using System;
using System.Threading;
using System.Collections.Generic;
using System.Text;
using PowerPOS.Container;
using System.Data;
using System.IO;
using System.Security.Cryptography;
using SubSonic;
using System.Linq;
namespace PowerPOS
{

    public enum LoginType
    {
        Login = 0, Authorizing = 1
    }
    public class UserController
    {
        public static DataTable FetchUsers
            (string UserName, string DisplayName, string remark,
            int GroupID)
        {
            ViewUserCollection mySales = new ViewUserCollection();
            if (UserName != "")
            {
                mySales.Where(ViewUser.Columns.UserName, UserName);
            }
            else
            {               
                if (DisplayName != "")
                {
                    mySales.Where(ViewUser.Columns.DisplayName, SubSonic.Comparison.Like, "%" + DisplayName + "%");
                }
                if (remark != "")
                {
                    mySales.Where(ViewUser.Columns.Remark, SubSonic.Comparison.Like, "%" + remark + "%");
                }
                if (GroupID != 0)
                {
                    mySales.Where(ViewUser.Columns.GroupID, SubSonic.Comparison.Equals, GroupID);
                }
            }            
            return mySales.Load().ToDataTable();
        }

        public static ViewUserCollection FetchUsers (string UserName, bool ShowDeletedUser)
        {
            ViewUserCollection mySales = new ViewUserCollection();
            if (UserName != "")
            {
                mySales.Where(ViewUser.Columns.UserName, SubSonic.Comparison.Like, UserName);
            }

            if (!ShowDeletedUser)
            {
                mySales.Where(ViewUser.Columns.Deleted, false); 
            }
            return mySales.Load();
        }

        public static DataTable FetchGroupPrivileges(string GroupName)
        {
            try
            {
                ViewGroupPrivilegeCollection vgp = new ViewGroupPrivilegeCollection();
                vgp.Where(ViewGroupPrivilege.Columns.GroupName, GroupName);
                return vgp.Load().ToDataTable();
            }
            catch (UserControllerException ex)
            {
                Logger.writeLog(ex);
                return null;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
        }

        public static DataTable FetchGroupPrivilegesWithUsername(string GroupName, string UserName)
        {
            try
            {
                string sql = "SELECT DISTINCT * FROM ( " +
                                "{2} select ug.GroupName, up.PrivilegeName, up.FormName, ug.GroupDescription " +
                                "from GroupUserPrivilege gu " +
                                "inner join UserPrivilege up on gu.UserPrivilegeID = up.UserPrivilegeID " +
                                "inner join UserGroup ug on ug.GroupID = gu.GroupID " +
                                "where ISNULL(up.userflag1,0) = 0 AND ug.GroupName = '{0}' {1}) a ";

                string POSType = AppSetting.GetSetting(AppSetting.SettingsName.POSType);
                string sqlPOS = "";
                string sqlAdmin = "";
                if (string.IsNullOrEmpty(POSType))
                {
                    POSType = "Retail";
                }

                if (POSType.ToLower() == "wholesale")
                    sqlPOS += " AND ISNULL(up.userflag3,0) = 1 ";
                else if (POSType.ToLower() == "beauty")
                    sqlPOS += " AND ISNULL(up.userflag4,0) = 1 ";
                else
                    sqlPOS += " AND ISNULL(up.userflag2,0) = 1 ";

                if (UserName.ToLower() == "edgeworks")
                    sqlAdmin = "select 'edgeworks' as GroupName, up.PrivilegeName, up.FormName, 'edgeworks' as GroupDescription " +
                                "from UserPrivilege up " +
                                "where ISNULL(up.userflag1,0) = 1 UNION ";
                sql = string.Format(sql, GroupName, sqlPOS, sqlAdmin);

                DataTable dt = DataService.GetDataSet(new QueryCommand(sql)).Tables[0];

                return dt;
            }
            catch (UserControllerException ex)
            {
                Logger.writeLog(ex);
                return null;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
        }
        /*
        public static bool logEditBill(string userid, string witness, string refno, decimal prevAmt, decimal newAmt, out string status)
        {
            try
            {
                EditBillLog myEdit = new EditBillLog();
                myEdit.UserName = userid;
                myEdit.WitnessName = witness;
                myEdit.PreviousAmount = prevAmt;
                myEdit.NewAmount = newAmt;
                myEdit.OrderHdrRefNo = refno;
                myEdit.EditDateTime = DateTime.Now;
                myEdit.Save();
                status = "";
                return true;
            }
            catch (UserControllerException ex)
            {
                status = "Unknown error was encountered when logging edit bill";
                Logger.writeLog(ex);
                return false;
            }
            catch (Exception ex)
            {
                status = "Unknown error was encountered when logging edit bill";
                Logger.writeLog(ex);
                return false;
            }
        }*/
        public static bool ValidUserID(string userid)
        {
            Query Qr = UserMst.CreateQuery();
            Where whr = new Where();
            whr.ColumnName = "UserName";
            whr.Comparison = Comparison.Equals;
            whr.ParameterName = "@UserName";
            whr.ParameterValue = userid;
            whr.TableName = "UserMST";
            return ((int)Qr.GetCount(UserMst.Columns.UserName, whr) > 0);
        }
        public static void logout()
        {
            try
            {
                //log at user activity table..
                LoginActivity myLog = new LoginActivity();
                myLog.LoginDateTime = DateTime.Now;
                myLog.LoginType = "Logout";
                myLog.UserName = UserInfo.username;
                myLog.PointOfSaleID = PointOfSaleInfo.PointOfSaleID;
                //myLog.UniqueID = Guid.NewGuid();
                myLog.Save();

                //Close
                UserInfo.username = "";
                UserInfo.role = "";
            }
            catch (UserControllerException ex)
            {
                Logger.writeLog(ex);                                                
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);                
            }
        }

        public static bool loginWithMagneticStripeCard
            (string userid, LoginType type, out UserMst user,
            out string role, out string DeptID, out string status)
        {

            try
            {
                UserMst myUser = new UserMst(UserMst.Columns.UserName, userid);
                role = "";
                status = "";
                DeptID = "ERR";
                if (myUser == null || myUser.UserName == null || myUser.UserName == "" ||
                    (myUser.Deleted.HasValue && myUser.Deleted.Value))
                {
                    status = "Please enter a valid user ID";
                    user = null;
                    return false;
                }
                else
                {                    
                    role = myUser.UserGroup.GroupName;
                    DeptID = myUser.DepartmentID.ToString();
                    status = "";

                    if (PointOfSaleInfo.PointOfSaleID != 0)
                    {
                        try
                        {
                            //log at user activity table..
                            LoginActivity myLog = new LoginActivity();
                            myLog.LoginDateTime = DateTime.Now;
                            if (type == LoginType.Login)
                            {
                                myLog.LoginType = "Login";
                            }
                            else if (type == LoginType.Authorizing)
                            {
                                myLog.LoginType = "Authorizing";
                            }
                            myLog.UserName = userid;
                            myLog.UniqueID = Guid.NewGuid();
                            myLog.PointOfSaleID = PointOfSaleInfo.PointOfSaleID;

                            myLog.Save();
                        }
                        catch (Exception ex)
                        {
                            //capture the error
                            Logger.writeLog("Error capturing login log for user>" + userid + " and POSID > " + PointOfSaleInfo.PointOfSaleID);
                            Logger.writeLog(ex);
                        }
                    }
                    user = myUser;
                    return true;                    
                }
            }
            catch (UserControllerException ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                role = "";
                DeptID = "ERR";
                user = null;
                return false;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                role = "";
                user = null;
                DeptID = "ERR";
                return false;
            }
        }
        public static bool login
            (string userid, string password, LoginType type, out UserMst user, 
            out string role, out string DeptID, out string status)
        {

            try
            {
                UserMst myUser = new UserMst(UserMst.Columns.UserName, userid);
                role = "";
                status = "";
                DeptID = "ERR";
                if (myUser == null || myUser.UserName == null || myUser.UserName == "" || 
                    (myUser.Deleted.HasValue && myUser.Deleted.Value))
                {
                    status = "Please enter a valid user ID";
                    user = null;
                    return false;
                }
                else
                {
                    string[] selOutlet = myUser.AssignedOutletList;
                    bool linkToOutlet = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.User.LinkTheUserWithOutlet), false);


                    if (myUser.Password !=UserController.EncryptData(password))
                    {
                        status = "Please enter a valid password";
                        user = null;
                        return false;
                    }
                    else
                    {
                        role = myUser.UserGroup.GroupName;
                        DeptID = myUser.DepartmentID.ToString();
                        status = "";

                        if (PointOfSaleInfo.PointOfSaleID != 0)                        
                        {
                            try
                            {
                                if (linkToOutlet)
                                {
                                    if (!selOutlet.Contains(PointOfSaleInfo.OutletName) && userid != "edgeworks")
                                    {
                                        status = "This user doesn't belong to selected outlet";
                                        user = null;
                                        role = "";
                                        DeptID = "ERR";
                                        return false;
                                    }
                                }

                                //log at user activity table..
                                LoginActivity myLog = new LoginActivity();
                                myLog.LoginDateTime = DateTime.Now;
                                if (type == LoginType.Login)
                                {
                                    myLog.LoginType = "Login";
                                }
                                else if (type == LoginType.Authorizing)
                                {
                                    myLog.LoginType = "Authorizing";
                                }
                                myLog.UserName = userid;
                                myLog.UniqueID = Guid.NewGuid();
                                myLog.PointOfSaleID = PointOfSaleInfo.PointOfSaleID;

                                myLog.Save();
                            }
                            catch (Exception ex)
                            {
                                //capture the error
                                Logger.writeLog("Error capturing login log for user>" + userid + " and POSID > " + PointOfSaleInfo.PointOfSaleID );
                                Logger.writeLog(ex);
                            }
                        }
                        user = myUser;
                        return true;
                    }
                }
            }
            catch (UserControllerException ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                role = "";
                DeptID = "ERR";
                user = null;
                return false;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                role = "";
                user = null;
                DeptID = "ERR";
                return false;
            }
        }
        public static bool loginWithToken
           (string usertoken, LoginType type, out UserMst user,
           out string role, out string DeptID, out string status)
        {

            try
            {
                UserMst myUser = new UserMst(UserMst.UserColumns.UserToken, usertoken);
                role = "";
                status = "";
                DeptID = "ERR";
                if (myUser == null || myUser.UserName == null || myUser.UserName == "" ||
                    (myUser.Deleted.HasValue && myUser.Deleted.Value))
                {
                    status = "Please enter a valid user ID";
                    user = null;
                    return false;
                }
                else
                {
                    string[] selOutlet = myUser.AssignedOutletList;
                    bool linkToOutlet = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.User.LinkTheUserWithOutlet), false);

                    string userid = myUser.UserName;
                    role = myUser.UserGroup.GroupName;
                    DeptID = myUser.DepartmentID.ToString();
                    status = "";

                    if (PointOfSaleInfo.PointOfSaleID != 0)
                    {
                        try
                        {
                            if (linkToOutlet)
                            {
                                if (!selOutlet.Contains(PointOfSaleInfo.OutletName) && userid != "edgeworks")
                                {
                                    status = "This user doesn't belong to selected outlet";
                                    user = null;
                                    role = "";
                                    DeptID = "ERR";
                                    return false;
                                }
                            }

                            //log at user activity table..
                            LoginActivity myLog = new LoginActivity();
                            myLog.LoginDateTime = DateTime.Now;
                            if (type == LoginType.Login)
                            {
                                myLog.LoginType = "Login";
                            }
                            else if (type == LoginType.Authorizing)
                            {
                                myLog.LoginType = "Authorizing";
                            }
                            myLog.UserName = myUser.UserName;
                            myLog.UniqueID = Guid.NewGuid();
                            myLog.PointOfSaleID = PointOfSaleInfo.PointOfSaleID;

                            myLog.Save();
                        }
                        catch (Exception ex)
                        {
                            //capture the error
                            Logger.writeLog("Error capturing login log for user>" + myUser.UserName + " and POSID > " + PointOfSaleInfo.PointOfSaleID);
                            Logger.writeLog(ex);
                        }
                    }
                    user = myUser;
                    return true;
                    
                }
            }
            catch (UserControllerException ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                role = "";
                DeptID = "ERR";
                user = null;
                return false;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                role = "";
                user = null;
                DeptID = "ERR";
                return false;
            }
        }

        #region "Encryption"
        /// <summary>
        /// Use AES to encrypt data string. The output string is the encrypted bytes as a base64 string.
        /// The same password must be used to decrypt the string.
        /// </summary>
        /// <param name="data">Clear string to encrypt.</param>
        /// <param name="password">Password used to encrypt the string.</param>
        /// <returns>Encrypted result as Base64 string.</returns>
        public static string EncryptData(string data)
        {            
            string passPhrase = "Pas5pr@se";        // can be any string
            string saltValue = "s@1tValue";        // can be any string
            string hashAlgorithm = "SHA1";             // can be "MD5"
            int passwordIterations = 2;                  // can be any number
            string initVector = "@1B2c3D4e5F6g7H8"; // must be 16 bytes
            int keySize = 256;                // can be 192 or 128

            if (data == null)
                throw new ArgumentNullException("data");
            
            string cipherText = Encrypt(data,
                                                     passPhrase,
                                                     saltValue,
                                                     hashAlgorithm,
                                                     passwordIterations,
                                                     initVector,
                                                     keySize);
            return cipherText;
        }
        /// <summary>
        /// Decrypt the data string to the original string.  The data must be the base64 string
        /// returned from the EncryptData method.
        /// </summary>
        /// <param name="data">Encrypted data generated from EncryptData method.</param>
        /// <param name="password">Password used to decrypt the string.</param>
        /// <returns>Decrypted string.</returns>
        public static string DecryptData(string data)
        {            
            string passPhrase = "Pas5pr@se";        // can be any string
            string saltValue = "s@1tValue";        // can be any string
            string hashAlgorithm = "SHA1";             // can be "MD5"
            int passwordIterations = 2;                  // can be any number
            string initVector = "@1B2c3D4e5F6g7H8"; // must be 16 bytes
            int keySize = 256;                // can be 192 or 128
            if (data == null)
                throw new ArgumentNullException("data");
            
            string plainText = Decrypt(data,
                                                     passPhrase,
                                                     saltValue,
                                                     hashAlgorithm,
                                                     passwordIterations,
                                                     initVector,
                                                     keySize);

            return plainText;
        }
        /// <summary>
        /// Encrypts specified plaintext using Rijndael symmetric key algorithm
        /// and returns a base64-encoded result.
        /// </summary>
        /// <param name="plainText">
        /// Plaintext value to be encrypted.
        /// </param>
        /// <param name="passPhrase">
        /// Passphrase from which a pseudo-random password will be derived. The
        /// derived password will be used to generate the encryption key.
        /// Passphrase can be any string. In this example we assume that this
        /// passphrase is an ASCII string.
        /// </param>
        /// <param name="saltValue">
        /// Salt value used along with passphrase to generate password. Salt can
        /// be any string. In this example we assume that salt is an ASCII string.
        /// </param>
        /// <param name="hashAlgorithm">
        /// Hash algorithm used to generate password. Allowed values are: "MD5" and
        /// "SHA1". SHA1 hashes are a bit slower, but more secure than MD5 hashes.
        /// </param>
        /// <param name="passwordIterations">
        /// Number of iterations used to generate password. One or two iterations
        /// should be enough.
        /// </param>
        /// <param name="initVector">
        /// Initialization vector (or IV). This value is required to encrypt the
        /// first block of plaintext data. For RijndaelManaged class IV must be 
        /// exactly 16 ASCII characters long.
        /// </param>
        /// <param name="keySize">
        /// Size of encryption key in bits. Allowed values are: 128, 192, and 256. 
        /// Longer keys are more secure than shorter keys.
        /// </param>
        /// <returns>
        /// Encrypted value formatted as a base64-encoded string.
        /// </returns>
        public static string Encrypt(string plainText,
                                     string passPhrase,
                                     string saltValue,
                                     string hashAlgorithm,
                                     int passwordIterations,
                                     string initVector,
                                     int keySize)
        {
            // Convert strings into byte arrays.
            // Let us assume that strings only contain ASCII codes.
            // If strings include Unicode characters, use Unicode, UTF7, or UTF8 
            // encoding.
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);

            // Convert our plaintext into a byte array.
            // Let us assume that plaintext contains UTF8-encoded characters.
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            // First, we must create a password, from which the key will be derived.
            // This password will be generated from the specified passphrase and 
            // salt value. The password will be created using the specified hash 
            // algorithm. Password creation can be done in several iterations.
            PasswordDeriveBytes password = new PasswordDeriveBytes(
                                                            passPhrase,
                                                            saltValueBytes,
                                                            hashAlgorithm,
                                                            passwordIterations);

            // Use the password to generate pseudo-random bytes for the encryption
            // key. Specify the size of the key in bytes (instead of bits).
            byte[] keyBytes = password.GetBytes(keySize / 8);

            // Create uninitialized Rijndael encryption object.
            RijndaelManaged symmetricKey = new RijndaelManaged();

            // It is reasonable to set encryption mode to Cipher Block Chaining
            // (CBC). Use default options for other symmetric key parameters.
            symmetricKey.Mode = CipherMode.CBC;

            // Generate encryptor from the existing key bytes and initialization 
            // vector. Key size will be defined based on the number of the key 
            // bytes.
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(
                                                             keyBytes,
                                                             initVectorBytes);

            // Define memory stream which will be used to hold encrypted data.
            MemoryStream memoryStream = new MemoryStream();

            // Define cryptographic stream (always use Write mode for encryption).
            CryptoStream cryptoStream = new CryptoStream(memoryStream,
                                                         encryptor,
                                                         CryptoStreamMode.Write);
            // Start encrypting.
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);

            // Finish encrypting.
            cryptoStream.FlushFinalBlock();

            // Convert our encrypted data from a memory stream into a byte array.
            byte[] cipherTextBytes = memoryStream.ToArray();

            // Close both streams.
            memoryStream.Close();
            cryptoStream.Close();

            // Convert encrypted data into a base64-encoded string.
            string cipherText = Convert.ToBase64String(cipherTextBytes);

            // Return encrypted string.
            return cipherText;
        }

        /// <summary>
        /// Decrypts specified ciphertext using Rijndael symmetric key algorithm.
        /// </summary>
        /// <param name="cipherText">
        /// Base64-formatted ciphertext value.
        /// </param>
        /// <param name="passPhrase">
        /// Passphrase from which a pseudo-random password will be derived. The
        /// derived password will be used to generate the encryption key.
        /// Passphrase can be any string. In this example we assume that this
        /// passphrase is an ASCII string.
        /// </param>
        /// <param name="saltValue">
        /// Salt value used along with passphrase to generate password. Salt can
        /// be any string. In this example we assume that salt is an ASCII string.
        /// </param>
        /// <param name="hashAlgorithm">
        /// Hash algorithm used to generate password. Allowed values are: "MD5" and
        /// "SHA1". SHA1 hashes are a bit slower, but more secure than MD5 hashes.
        /// </param>
        /// <param name="passwordIterations">
        /// Number of iterations used to generate password. One or two iterations
        /// should be enough.
        /// </param>
        /// <param name="initVector">
        /// Initialization vector (or IV). This value is required to encrypt the
        /// first block of plaintext data. For RijndaelManaged class IV must be
        /// exactly 16 ASCII characters long.
        /// </param>
        /// <param name="keySize">
        /// Size of encryption key in bits. Allowed values are: 128, 192, and 256.
        /// Longer keys are more secure than shorter keys.
        /// </param>
        /// <returns>
        /// Decrypted string value.
        /// </returns>
        /// <remarks>
        /// Most of the logic in this function is similar to the Encrypt
        /// logic. In order for decryption to work, all parameters of this function
        /// - except cipherText value - must match the corresponding parameters of
        /// the Encrypt function which was called to generate the
        /// ciphertext.
        /// </remarks>
        public static string Decrypt(string cipherText,
                                     string passPhrase,
                                     string saltValue,
                                     string hashAlgorithm,
                                     int passwordIterations,
                                     string initVector,
                                     int keySize)
        {
            // Convert strings defining encryption key characteristics into byte
            // arrays. Let us assume that strings only contain ASCII codes.
            // If strings include Unicode characters, use Unicode, UTF7, or UTF8
            // encoding.
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);

            // Convert our ciphertext into a byte array.
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);

            // First, we must create a password, from which the key will be 
            // derived. This password will be generated from the specified 
            // passphrase and salt value. The password will be created using
            // the specified hash algorithm. Password creation can be done in
            // several iterations.
            PasswordDeriveBytes password = new PasswordDeriveBytes(
                                                            passPhrase,
                                                            saltValueBytes,
                                                            hashAlgorithm,
                                                            passwordIterations);

            // Use the password to generate pseudo-random bytes for the encryption
            // key. Specify the size of the key in bytes (instead of bits).
            byte[] keyBytes = password.GetBytes(keySize / 8);

            // Create uninitialized Rijndael encryption object.
            RijndaelManaged symmetricKey = new RijndaelManaged();

            // It is reasonable to set encryption mode to Cipher Block Chaining
            // (CBC). Use default options for other symmetric key parameters.
            symmetricKey.Mode = CipherMode.CBC;

            // Generate decryptor from the existing key bytes and initialization 
            // vector. Key size will be defined based on the number of the key 
            // bytes.
            ICryptoTransform decryptor = symmetricKey.CreateDecryptor(
                                                             keyBytes,
                                                             initVectorBytes);

            // Define memory stream which will be used to hold encrypted data.
            MemoryStream memoryStream = new MemoryStream(cipherTextBytes);

            // Define cryptographic stream (always use Read mode for encryption).
            CryptoStream cryptoStream = new CryptoStream(memoryStream,
                                                          decryptor,
                                                          CryptoStreamMode.Read);

            // Since at this point we don't know what the size of decrypted data
            // will be, allocate the buffer long enough to hold ciphertext;
            // plaintext is never longer than ciphertext.
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];

            // Start decrypting.
            int decryptedByteCount = cryptoStream.Read(plainTextBytes,
                                                       0,
                                                       plainTextBytes.Length);

            // Close both streams.
            memoryStream.Close();
            cryptoStream.Close();

            // Convert decrypted data into a string. 
            // Let us assume that the original plaintext string was UTF8-encoded.
            string plainText = Encoding.UTF8.GetString(plainTextBytes,
                                                       0,
                                                       decryptedByteCount);

            // Return decrypted string.   
            return plainText;
        }
        #endregion

        public static void ClearUserInfo()
        {
            UserInfo.displayName = "";
            UserInfo.username = "";
            UserInfo.role = "";
            UserInfo.privileges = null;
            UserInfo.SalesPersonGroupID = 0;
        }

        public static bool ChangePassword
            (string username, string oldPassword, string newPassword, out string status)
        {
            try
            {
                UserMst myUser = new UserMst(username);
                if (myUser.IsLoaded &&
                    !myUser.IsNew &&
                    DecryptData(myUser.Password) == oldPassword)
                {
                    myUser.Password = EncryptData(newPassword);
                    myUser.Save();
                    status = "";
                    return true;
                }
                else
                {
                    status = "Wrong password or user name does not exist";
                    return false;
                }

            }
            catch (Exception ex)
            {
                status = "Unknown error: " + ex.Message;
                Logger.writeLog(ex);
                return false;
            }
        }
    }
    public class UserControllerException : ApplicationException
    {
        // Default constructor
        public UserControllerException()
        {
        }
        // Constructor accepting a single string message
        public UserControllerException(string message)
            : base(message)
        {
        }
        // Constructor accepting a string message and an
        // inner exception which will be wrapped by this
        // custom exception class
        public UserControllerException(string message,
        Exception inner)
            : base(message, inner)
        {
        }
    }
}
