using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Renci.SshNet;
using Renci.SshNet.Sftp;
using System.Net.FtpClient;
using System.IO;
using PowerPOS;
using System.Net;

namespace PowerWeb.BLL.Helper
{
    class FTP
    {
        struct MessageType
        {
            public static string INFO = "FTP_INFO";
            public static string ERROR = "FTP_ERROR";
        }

        public static bool StartTransfer(string direction, string localDir, string serverDir, out string result)
        {
            bool proceed = true;
            string message = "";
            result = "";

            try
            {
                DirectoryInfo fileDir = new DirectoryInfo(localDir);
                DirectoryInfo backupDir = new DirectoryInfo(Path.Combine(localDir, "backup"));
                string serverDirectory = "/" + serverDir.Trim('/');
                string serverBackupDir = serverDirectory + "/backup";

                #region *) Get value from FTPSettings
                string protocol = AppSetting.GetSetting(AppSetting.SettingsName.FTP.Protocol);
                string ftpHost = AppSetting.GetSetting(AppSetting.SettingsName.FTP.Host);
                int ftpPort = AppSetting.CastInt(AppSetting.GetSetting(AppSetting.SettingsName.FTP.Port), 21);
                string username = AppSetting.GetSetting(AppSetting.SettingsName.FTP.Username);
                string password = AppSetting.GetSetting(AppSetting.SettingsName.FTP.Password);
                bool passiveMode = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.FTP.PassiveMode), false);
                #endregion

                #region *) Validation
                if (string.IsNullOrEmpty(protocol))
                {
                    protocol = "FTP";
                }

                if (string.IsNullOrEmpty(direction) || (direction.ToLower() != "download" && direction.ToLower() != "upload"))
                {
                    result = MessageType.ERROR + ": Please specify the direction (download or upload) first.";
                    Logger.writeLog(result);
                    return false;
                }

                if (!fileDir.Exists)
                {
                    result = MessageType.ERROR + ": Directory does not exist.";
                    Logger.writeLog(result);
                    return false;
                }

                if (direction.ToLower() == "upload" && !backupDir.Exists)
                {
                    try
                    {
                        backupDir.Create();
                    }
                    catch
                    {
                        result = MessageType.ERROR + ": Failed to create backup directory.";
                        Logger.writeLog(result);
                        return false;
                    }
                }

                if (string.IsNullOrEmpty(ftpHost))
                {
                    result = MessageType.ERROR + ": Please specify the FTP Host in Setting first.";
                    Logger.writeLog(result);
                    return false;
                }
                else
                {
                    ftpHost = ftpHost.TrimEnd('/');
                }

                if (string.IsNullOrEmpty(serverDirectory))
                {
                    result = MessageType.ERROR + ": Please specify the Server Directory first.";
                    Logger.writeLog(result);
                    return false;
                }
                else
                {
                    serverDirectory = "/" + serverDirectory.Trim('/');
                }

                if (direction.ToLower() == "download" && string.IsNullOrEmpty(serverBackupDir))
                {
                    result = MessageType.ERROR + ": Please specify the Server Backup Directory first.";
                    Logger.writeLog(result);
                    return false;
                }
                else
                {
                    serverBackupDir = "/" + serverBackupDir.Trim('/');
                }

                if (string.IsNullOrEmpty(username))
                {
                    result = MessageType.ERROR + ": Please specify the username in Setting first.";
                    Logger.writeLog(result);
                    return false;
                }

                if (string.IsNullOrEmpty(password))
                {
                    result = MessageType.ERROR + ": Please specify the password in Setting first.";
                    Logger.writeLog(result);
                    return false;
                }
                #endregion

                if (proceed)
                {
                    if (direction.ToLower() == "upload")
                    {
                        if (protocol.ToUpper() == "FTP")
                        {
                            #region ===== UPLOAD USING FTP =====
                            if (fileDir.GetFiles().Length > 0)
                            {
                                message = "Start uploading files...";
                                Logger.writeLog(MessageType.INFO + ": " + message);
                                Console.WriteLine();

                                using (FtpClient conn = new FtpClient())
                                {
                                    conn.Host = ftpHost;
                                    conn.Port = ftpPort;
                                    conn.Credentials = new NetworkCredential(username, password);
                                    if (passiveMode)
                                        conn.DataConnectionType = FtpDataConnectionType.AutoPassive;
                                    else
                                        conn.DataConnectionType = FtpDataConnectionType.AutoActive;

                                    conn.Connect();

                                    foreach (FileInfo file in fileDir.GetFiles())
                                    {
                                        #region *) Upload the file
                                        using (Stream ostream = conn.OpenWrite(serverDirectory + "/" + file.Name))
                                        {
                                            try
                                            {
                                                FileStream fileStream = new FileStream(file.FullName, FileMode.Open);
                                                int bytesRead = 0;
                                                byte[] buffer = new byte[2048];

                                                while (true)
                                                {
                                                    bytesRead = fileStream.Read(buffer, 0, buffer.Length);
                                                    if (bytesRead == 0)
                                                        break;
                                                    ostream.Write(buffer, 0, bytesRead);
                                                }

                                                message = MessageType.INFO + ": Upload file {0} ({1} bytes) complete";
                                                Logger.writeLog(string.Format(message, file.Name, fileStream.Length));

                                                fileStream.Close();
                                                fileStream.Dispose();
                                            }
                                            catch (Exception ex)
                                            {
                                                throw ex;
                                            }
                                            finally
                                            {
                                                ostream.Close();
                                            }
                                        }
                                        #endregion

                                        #region *) Move the file to backup directory
                                        FileInfo backupFile = new FileInfo(backupDir.FullName + "\\" + file.Name);
                                        if (backupFile.Exists)
                                        {
                                            backupFile.Delete();
                                            Logger.writeLog(string.Format(MessageType.INFO + ": File {0} already exists in backup directory and will be replaced.", backupFile.Name));
                                        }
                                        file.MoveTo(backupDir.FullName + "\\" + file.Name);

                                        message = MessageType.INFO + ": File {0} has been moved to backup directory";
                                        Logger.writeLog(string.Format(message, file.Name));
                                        Console.WriteLine();
                                        #endregion
                                    }

                                    conn.Disconnect();
                                }

                                Logger.writeLog(MessageType.INFO + ": Finish uploading files...");
                            }
                            else
                            {
                                Logger.writeLog(MessageType.INFO + ": Started but found no file to upload.");
                            }
                            #endregion
                        }
                        else if (protocol.ToUpper() == "SFTP")
                        {
                            #region ===== UPLOAD USING SFTP =====

                            if (fileDir.GetFiles().Length > 0)
                            {
                                message = MessageType.INFO + ": Start uploading files...";
                                Logger.writeLog(message);
                                Console.WriteLine();

                                using (SftpClient conn = new SftpClient(ftpHost, ftpPort, username, password))
                                {
                                    conn.Connect();

                                    foreach (FileInfo file in fileDir.GetFiles())
                                    {
                                        #region *) Upload the file
                                        using (SftpFileStream ostream = conn.OpenWrite(serverDirectory + "/" + file.Name))
                                        {
                                            try
                                            {
                                                FileStream fileStream = new FileStream(file.FullName, FileMode.Open);
                                                int bytesRead = 0;
                                                byte[] buffer = new byte[2048];

                                                while (true)
                                                {
                                                    bytesRead = fileStream.Read(buffer, 0, buffer.Length);
                                                    if (bytesRead == 0)
                                                        break;
                                                    ostream.Write(buffer, 0, bytesRead);
                                                }

                                                message = MessageType.INFO + ": Upload file {0} ({1} bytes) complete";
                                                Logger.writeLog(string.Format(message, file.Name, fileStream.Length));

                                                fileStream.Close();
                                                fileStream.Dispose();
                                            }
                                            catch (Exception ex)
                                            {
                                                throw ex;
                                            }
                                            finally
                                            {
                                                ostream.Close();
                                            }
                                        }
                                        #endregion

                                        #region *) Move the file to backup directory
                                        FileInfo backupFile = new FileInfo(backupDir.FullName + "\\" + file.Name);
                                        if (backupFile.Exists)
                                        {
                                            backupFile.Delete();
                                            Logger.writeLog(string.Format(MessageType.INFO + ": File {0} already exists in backup directory and will be replaced.", backupFile.Name));
                                        }
                                        file.MoveTo(backupDir.FullName + "\\" + file.Name);

                                        message = MessageType.INFO + ": File {0} has been moved to backup directory";
                                        Logger.writeLog(string.Format(message, file.Name));
                                        Console.WriteLine();
                                        #endregion
                                    }

                                    conn.Disconnect();
                                }

                                Logger.writeLog(MessageType.INFO + ": Finish uploading files...");
                            }
                            else
                            {
                                Logger.writeLog(MessageType.INFO + ": Started but found no file to upload.");
                            }

                            #endregion
                        }
                    }
                    else if (direction.ToLower() == "download")
                    {
                        if (protocol.ToUpper() == "FTP")
                        {
                            #region ===== DOWNLOAD USING FTP =====

                            using (FtpClient conn = new FtpClient())
                            {
                                conn.Host = ftpHost;
                                conn.Port = ftpPort;
                                conn.Credentials = new NetworkCredential(username, password);
                                if (passiveMode)
                                    conn.DataConnectionType = FtpDataConnectionType.AutoPassive;
                                else
                                    conn.DataConnectionType = FtpDataConnectionType.AutoActive;

                                conn.Connect();

                                // Prepare the backup directory first
                                if (!conn.DirectoryExists(serverBackupDir))
                                {
                                    conn.CreateDirectory(serverBackupDir);
                                }

                                // Get list of files/dirs from server
                                List<FtpListItem> listItem = new List<FtpListItem>(conn.GetListing(serverDirectory));

                                // Only keep item with type = File
                                listItem.RemoveAll(i => i.Type != FtpFileSystemObjectType.File);

                                if (listItem.Count > 0)
                                {
                                    message = MessageType.INFO + ": Start downloading files...";
                                    Logger.writeLog(message);
                                    Console.WriteLine();

                                    foreach (FtpListItem item in listItem)
                                    {
                                        #region *) Download the file
                                        using (Stream istream = conn.OpenRead(item.FullName))
                                        {
                                            try
                                            {
                                                FileStream fileStream = new FileStream(fileDir.FullName + "\\" + item.Name, FileMode.Create);
                                                int bytesRead = 0;
                                                byte[] buffer = new byte[2048];

                                                while (true)
                                                {
                                                    bytesRead = istream.Read(buffer, 0, buffer.Length);
                                                    if (bytesRead == 0)
                                                        break;
                                                    fileStream.Write(buffer, 0, bytesRead);
                                                }

                                                message = MessageType.INFO + ": Download file {0} ({1} bytes) complete";
                                                Logger.writeLog(string.Format(message, item.Name, fileStream.Length));

                                                fileStream.Close();
                                                fileStream.Dispose();
                                            }
                                            catch (Exception ex)
                                            {
                                                throw ex;
                                            }
                                            finally
                                            {
                                                istream.Close();
                                            }
                                        }
                                        #endregion

                                        #region *) Move the file to backup directory
                                        if (conn.FileExists(serverBackupDir + "/" + item.Name))
                                        {
                                            // File exists, delete it
                                            conn.DeleteFile(serverBackupDir + "/" + item.Name);
                                        }
                                        conn.Rename(item.FullName, serverBackupDir + "/" + item.Name);

                                        message = "Move file {0} to {1} complete";
                                        Logger.writeLog(string.Format(message, item.Name, serverBackupDir));
                                        Console.WriteLine();
                                        #endregion
                                    }

                                    message = MessageType.INFO + ": Finish downloading files...";
                                    Logger.writeLog(message);
                                }
                                else
                                {
                                    message = MessageType.INFO + ": Started but found no file to download.";
                                    Logger.writeLog(message);
                                }

                                conn.Disconnect();
                            }

                            #endregion
                        }
                        else if (protocol.ToUpper() == "SFTP")
                        {
                            #region ===== DOWNLOAD USING SFTP =====

                            using (SftpClient conn = new SftpClient(ftpHost, ftpPort, username, password))
                            {
                                conn.Connect();

                                // Prepare the backup directory first
                                try
                                {
                                    // Try to create the backup directory
                                    conn.CreateDirectory(serverBackupDir);
                                }
                                catch (Renci.SshNet.Common.SshException ex)
                                {
                                    // If directory already exists, ignore it. But if other error occurs, throw exception
                                    if (ex.Message != "The file already exists.")
                                        throw ex;
                                }

                                // Get list of files/dirs from server
                                List<SftpFile> listItem = new List<SftpFile>(conn.ListDirectory(serverDirectory, null));

                                // Only keep item with type = File
                                listItem.RemoveAll(i => i.IsRegularFile == false);

                                if (listItem.Count > 0)
                                {
                                    message = MessageType.INFO + ": Start downloading files...";
                                    Logger.writeLog(message);
                                    Console.WriteLine();

                                    foreach (SftpFile item in listItem)
                                    {
                                        #region *) Download the file
                                        using (SftpFileStream istream = conn.OpenRead(item.FullName))
                                        {
                                            try
                                            {
                                                FileStream fileStream = new FileStream(fileDir.FullName + "\\" + item.Name, FileMode.Create);
                                                int bytesRead = 0;
                                                byte[] buffer = new byte[2048];

                                                while (true)
                                                {
                                                    bytesRead = istream.Read(buffer, 0, buffer.Length);
                                                    if (bytesRead == 0)
                                                        break;
                                                    fileStream.Write(buffer, 0, bytesRead);
                                                }

                                                message = MessageType.INFO + ": Download file {0} ({1} bytes) complete";
                                                Logger.writeLog(string.Format(message, item.Name, fileStream.Length));

                                                fileStream.Close();
                                                fileStream.Dispose();
                                            }
                                            catch (Exception ex)
                                            {
                                                throw ex;
                                            }
                                            finally
                                            {
                                                istream.Close();
                                            }
                                        }
                                        #endregion

                                        #region *) Move the file to backup directory
                                        try
                                        {
                                            // Try to delete the file
                                            conn.DeleteFile(serverBackupDir + "/" + item.Name);
                                        }
                                        catch (Renci.SshNet.Common.SftpPathNotFoundException)
                                        {
                                            // File does not exist. Do nothing
                                        }

                                        conn.RenameFile(item.FullName, serverBackupDir + "/" + item.Name);

                                        message = MessageType.INFO + ": Move file {0} to {1} complete";
                                        Logger.writeLog(string.Format(message, item.Name, serverBackupDir));
                                        Console.WriteLine();
                                        #endregion
                                    }

                                    message = MessageType.INFO + ": Finish downloading files...";
                                    Logger.writeLog(message);
                                }
                                else
                                {
                                    message = MessageType.INFO + ": Started but found no file to download.";
                                    Logger.writeLog(message);
                                }

                                conn.Disconnect();
                            }

                            #endregion
                        }
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occured: " + ex.Message);
                Logger.writeLog(ex.ToString());
                result = ex.ToString();
                return false;
            }
        }

        //private static void WriteToLog(string message, string msgtype)
        //{
        //    Console.WriteLine(message);
        //    Logger.writeLog(msgtype + ": " + message);

        //    //string source = FTPSettings.Default.EventLogName;
        //    //if (string.IsNullOrEmpty(source))
        //    //{
        //    //    source = Assembly.GetExecutingAssembly().GetName().Name;
        //    //}

        //    //if (!EventLog.SourceExists(source))
        //    //    EventLog.CreateEventSource(source, "Application");

        //    //EventLog.WriteEntry(source, message, logType);
        //    //(new Helper()).WriteLog(message);
        //}
    }
}
