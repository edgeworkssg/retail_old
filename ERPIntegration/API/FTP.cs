using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERPIntegration.Properties;
using System.Reflection;
using System.IO;
using System.Net;
using System.Diagnostics;
using Renci.SshNet;
using Renci.SshNet.Sftp;
using System.Net.FtpClient;
using PowerPOS;

namespace ERPIntegration.API
{
    class FTP
    {
        struct MessageType
        {
            public static string INFO = "FTP_INFO";
            public static string ERROR = "FTP_ERROR";
        }
        
        public static void StartTransfer(string direction, string localDir, string serverDir)
        {
            bool proceed = true;
            string message = "";

            try
            {
                DirectoryInfo fileDir = new DirectoryInfo(localDir);
                DirectoryInfo backupDir = new DirectoryInfo(Path.Combine(localDir, "backup"));
                string serverDirectory = "/" + serverDir.Trim('/');
                string serverBackupDir = serverDirectory + "/backup";

                #region *) Get value from FTPSettings
                string protocol = FTPSettings.Default.Protocol;
                string ftpHost = FTPSettings.Default.FtpHost;
                int ftpPort = FTPSettings.Default.FtpPort;
                string username = FTPSettings.Default.username;
                string password = FTPSettings.Default.password;
                bool passiveMode = FTPSettings.Default.PassiveMode;
                #endregion

                #region *) Validation
                if (string.IsNullOrEmpty(protocol))
                {
                    protocol = "FTP";
                }

                if (string.IsNullOrEmpty(direction) || (direction.ToLower() != "download" && direction.ToLower() != "upload"))
                {
                    Helper.WriteLog("Please specify the direction (download or upload) first.", MessageType.ERROR);
                    proceed = false;
                }

                if (!fileDir.Exists)
                {
                    Helper.WriteLog("Directory does not exist.", MessageType.ERROR);
                    proceed = false;
                }

                if (direction.ToLower() == "upload" && !backupDir.Exists)
                {
                    try
                    {
                        backupDir.Create();
                    }
                    catch
                    {
                        Helper.WriteLog("Failed to create backup directory.", MessageType.ERROR);
                        proceed = false;
                    }
                }

                if (string.IsNullOrEmpty(ftpHost))
                {
                    Helper.WriteLog("Please specify the FTP Host in config file first.", MessageType.ERROR);
                    proceed = false;
                }
                else
                {
                    ftpHost = ftpHost.TrimEnd('/');
                }

                if (string.IsNullOrEmpty(serverDirectory))
                {
                    Helper.WriteLog("Please specify the Server Directory in config file first.", MessageType.ERROR);
                    proceed = false;
                }
                else
                {
                    serverDirectory = "/" + serverDirectory.Trim('/');
                }

                if (direction.ToLower() == "download" && string.IsNullOrEmpty(serverBackupDir))
                {
                    Helper.WriteLog("Please specify the Server Backup Directory first.", MessageType.ERROR);
                    proceed = false;
                }
                else
                {
                    serverBackupDir = "/" + serverBackupDir.Trim('/');
                }

                if (string.IsNullOrEmpty(username))
                {
                    Helper.WriteLog("Please specify the username in config file first.", MessageType.ERROR);
                    proceed = false;
                }

                if (string.IsNullOrEmpty(password))
                {
                    Helper.WriteLog("Please specify the password in config file first.", MessageType.ERROR);
                    proceed = false;
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
                                Helper.WriteLog(message, MessageType.INFO);
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

                                                message = "Upload file {0} ({1} bytes) complete";
                                                Helper.WriteLog(string.Format(message, file.Name, fileStream.Length), MessageType.INFO);

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
                                            Helper.WriteLog(string.Format("File {0} already exists in backup directory and will be replaced.", backupFile.Name), MessageType.INFO);
                                        }
                                        file.MoveTo(backupDir.FullName + "\\" + file.Name);

                                        message = "File {0} has been moved to backup directory";
                                        Helper.WriteLog(string.Format(message, file.Name), MessageType.INFO);
                                        Console.WriteLine();
                                        #endregion
                                    }

                                    conn.Disconnect();
                                }

                                Helper.WriteLog("Finish uploading files...", MessageType.INFO);
                            }
                            else
                            {
                                Helper.WriteLog("Started but found no file to upload.", MessageType.INFO);
                            }
                            #endregion
                        }
                        else if (protocol.ToUpper() == "SFTP")
                        {
                            #region ===== UPLOAD USING SFTP =====

                            if (fileDir.GetFiles().Length > 0)
                            {
                                message = "Start uploading files...";
                                Helper.WriteLog(message, MessageType.INFO);
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

                                                message = "Upload file {0} ({1} bytes) complete";
                                                Helper.WriteLog(string.Format(message, file.Name, fileStream.Length), MessageType.INFO);

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
                                            Helper.WriteLog(string.Format("File {0} already exists in backup directory and will be replaced.", backupFile.Name), MessageType.INFO);
                                        }
                                        file.MoveTo(backupDir.FullName + "\\" + file.Name);

                                        message = "File {0} has been moved to backup directory";
                                        Helper.WriteLog(string.Format(message, file.Name), MessageType.INFO);
                                        Console.WriteLine();
                                        #endregion
                                    }

                                    conn.Disconnect();
                                }

                                Helper.WriteLog("Finish uploading files...", MessageType.INFO);
                            }
                            else
                            {
                                Helper.WriteLog("Started but found no file to upload.", MessageType.INFO);
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
                                    message = "Start downloading files...";
                                    Helper.WriteLog(message, MessageType.INFO);
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

                                                message = "Download file {0} ({1} bytes) complete";
                                                Helper.WriteLog(string.Format(message, item.Name, fileStream.Length), MessageType.INFO);

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
                                        Helper.WriteLog(string.Format(message, item.Name, serverBackupDir), MessageType.INFO);
                                        Console.WriteLine();
                                        #endregion
                                    }

                                    message = "Finish downloading files...";
                                    Helper.WriteLog(message, MessageType.INFO);
                                }
                                else
                                {
                                    message = "Started but found no file to download.";
                                    Helper.WriteLog(message, MessageType.INFO);
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
                                    message = "Start downloading files...";
                                    Helper.WriteLog(message, MessageType.INFO);
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

                                                message = "Download file {0} ({1} bytes) complete";
                                                Helper.WriteLog(string.Format(message, item.Name, fileStream.Length), MessageType.INFO);

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

                                        message = "Move file {0} to {1} complete";
                                        Helper.WriteLog(string.Format(message, item.Name, serverBackupDir), MessageType.INFO);
                                        Console.WriteLine();
                                        #endregion
                                    }

                                    message = "Finish downloading files...";
                                    Helper.WriteLog(message, MessageType.INFO);
                                }
                                else
                                {
                                    message = "Started but found no file to download.";
                                    Helper.WriteLog(message, MessageType.INFO);
                                }

                                conn.Disconnect();
                            }

                            #endregion
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occured: " + ex.Message);
                Helper.WriteLog(ex.ToString(), MessageType.ERROR);
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
