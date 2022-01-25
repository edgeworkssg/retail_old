using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data.SqlClient;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System.Collections.Specialized;

namespace System.Data
{
    public class SqlCustomTools
    {
        public static string[] get_ServerNames()
        {
            List<string> Result = new List<string>();
            try
            {
                DataTable Servers = SmoApplication.EnumAvailableSqlServers(true);

                foreach (DataRow oneServer in Servers.Rows)
                {
                    Result.Add(oneServer["Name"].ToString());
                }
            }
            catch (Exception X)
            {
                throw X;
            }

            return Result.ToArray();
        }

        public static bool IsDatabaseExist(string ConStr)
        {
            try
            {
                string DBName;
                SqlConnectionStringBuilder aa = new SqlConnectionStringBuilder(ConStr);
                DBName = aa.InitialCatalog;
                aa.InitialCatalog = "master";
                return (new Server(new ServerConnection(new SqlConnection(aa.ConnectionString)))).Databases.Contains(DBName);
            }
            catch (Exception X)
            {
                throw X;
            }
        }

        public static bool IsDatabaseExist(string ServerName, string DatabaseName)
        {
            try
            {
                return (new Server(ServerName)).Databases.Contains(DatabaseName);
            }
            catch (Exception X)
            {
                throw X;
            }
        }

        public static void AttachDatabase(string ServerName, string DatabaseName, string DataFileURI, string LogFileURI, string LoginID, string Password)
        {
            try
            {
                Server SqlServer = new Server(ServerName);

                ServerConnection SqlServerConnection = SqlServer.ConnectionContext;

                SqlServerConnection.LoginSecure = false;
                SqlServerConnection.Login = LoginID;
                SqlServerConnection.Password = Password;
                SqlServerConnection.DatabaseName = "master";

                Database NewDatabase = new Database(SqlServer, DatabaseName);

                FileGroup DatabaseFileGroup = new FileGroup(NewDatabase, "PRIMARY");
                NewDatabase.FileGroups.Add(DatabaseFileGroup);

                DataFile DatabaseDataFile = new DataFile(DatabaseFileGroup, DatabaseName);
                DatabaseFileGroup.Files.Add(DatabaseDataFile);

                DatabaseDataFile.FileName = DataFileURI;

                LogFile DatabaseLogFile = new LogFile(NewDatabase, DatabaseName + "_log");
                NewDatabase.LogFiles.Add(DatabaseLogFile);

                DatabaseLogFile.FileName = LogFileURI;

                StringCollection DatabaseFilesCollection = new StringCollection();

                DatabaseFilesCollection.Add(DatabaseDataFile.FileName);
                DatabaseFilesCollection.Add(DatabaseLogFile.FileName);

                SqlServer.AttachDatabase(DatabaseName, DatabaseFilesCollection);
            }
            catch (Exception X)
            {
                throw X;
            }
        }

        public static void AttachDatabase(string ServerName, string DatabaseName, string DataFileURI, string LogFileURI)
        {
            try
            {
                Server SqlServer = new Server(ServerName);

                ServerConnection SqlServerConnection = SqlServer.ConnectionContext;

                SqlServerConnection.LoginSecure = true;
                //SqlServerConnection.Login = LoginID;
                //SqlServerConnection.Password = Password;
                SqlServerConnection.DatabaseName = "master";

                Database NewDatabase = new Database(SqlServer, DatabaseName);

                FileGroup DatabaseFileGroup = new FileGroup(NewDatabase, "PRIMARY");
                NewDatabase.FileGroups.Add(DatabaseFileGroup);

                DataFile DatabaseDataFile = new DataFile(DatabaseFileGroup, DatabaseName);
                DatabaseFileGroup.Files.Add(DatabaseDataFile);

                DatabaseDataFile.FileName = DataFileURI;

                LogFile DatabaseLogFile = new LogFile(NewDatabase, DatabaseName + "_log");
                NewDatabase.LogFiles.Add(DatabaseLogFile);

                DatabaseLogFile.FileName = LogFileURI;

                StringCollection DatabaseFilesCollection = new StringCollection();

                DatabaseFilesCollection.Add(DatabaseDataFile.FileName);
                DatabaseFilesCollection.Add(DatabaseLogFile.FileName);

                SqlServer.AttachDatabase(DatabaseName, DatabaseFilesCollection);
            }
            catch (Exception X)
            {
                throw X;
            }
        }

        public static void Create_NewDatabase(string ConnectionString)
        {
            try
            {
                string DatabaseName;
                SqlConnectionStringBuilder aa = new SqlConnectionStringBuilder(ConnectionString);
                DatabaseName = aa.InitialCatalog;
                aa.InitialCatalog = "master";
                ServerConnection Conn = new ServerConnection(new SqlConnection(aa.ConnectionString));

                Create_NewDatabase(Conn, DatabaseName);
            }
            catch (Exception X)
            {
                throw X;
            }
        }
        public static void Create_NewDatabase(ServerConnection ServerName, string DatabaseName)
        {
            try
            {
                string ExecutedQuery =
                    "CREATE DATABASE [@DatabaseName] ON  PRIMARY " +
                    "( NAME = N'@DatabaseName', FILENAME = N'@FilePath\\@DatabaseName.mdf' , SIZE = 3072KB , FILEGROWTH = 1024KB ) " +
                     "LOG ON " +
                    "( NAME = N'@DatabaseName_log', FILENAME = N'@FilePath\\@DatabaseName_log.ldf' , SIZE = 1024KB , FILEGROWTH = 10%); " +
                    "ALTER DATABASE [@DatabaseName] SET COMPATIBILITY_LEVEL = 100; " +
                    "ALTER DATABASE [@DatabaseName] SET ANSI_NULL_DEFAULT OFF; " +
                    "ALTER DATABASE [@DatabaseName] SET ANSI_NULLS OFF; " +
                    "ALTER DATABASE [@DatabaseName] SET ANSI_PADDING OFF; " +
                    "ALTER DATABASE [@DatabaseName] SET ANSI_WARNINGS OFF; " +
                    "ALTER DATABASE [@DatabaseName] SET ARITHABORT OFF; " +
                    "ALTER DATABASE [@DatabaseName] SET AUTO_CLOSE OFF; " +
                    "ALTER DATABASE [@DatabaseName] SET AUTO_CREATE_STATISTICS ON; " +
                    "ALTER DATABASE [@DatabaseName] SET AUTO_SHRINK OFF; " +
                    "ALTER DATABASE [@DatabaseName] SET AUTO_UPDATE_STATISTICS ON; " +
                    "ALTER DATABASE [@DatabaseName] SET CURSOR_CLOSE_ON_COMMIT OFF; " +
                    "ALTER DATABASE [@DatabaseName] SET CURSOR_DEFAULT  GLOBAL; " +
                    "ALTER DATABASE [@DatabaseName] SET CONCAT_NULL_YIELDS_NULL OFF; " +
                    "ALTER DATABASE [@DatabaseName] SET NUMERIC_ROUNDABORT OFF; " +
                    "ALTER DATABASE [@DatabaseName] SET QUOTED_IDENTIFIER OFF; " +
                    "ALTER DATABASE [@DatabaseName] SET RECURSIVE_TRIGGERS OFF; " +
                    "ALTER DATABASE [@DatabaseName] SET  DISABLE_BROKER; " +
                    "ALTER DATABASE [@DatabaseName] SET AUTO_UPDATE_STATISTICS_ASYNC OFF; " +
                    "ALTER DATABASE [@DatabaseName] SET DATE_CORRELATION_OPTIMIZATION OFF; " +
                    "ALTER DATABASE [@DatabaseName] SET PARAMETERIZATION SIMPLE; " +
                    "ALTER DATABASE [@DatabaseName] SET READ_WRITE; " +
                    "ALTER DATABASE [@DatabaseName] SET RECOVERY SIMPLE; " +
                    "ALTER DATABASE [@DatabaseName] SET MULTI_USER; " +
                    "ALTER DATABASE [@DatabaseName] SET PAGE_VERIFY CHECKSUM;";

                Microsoft.SqlServer.Management.Smo.Server Sr = new Server(ServerName);

                if (Sr.Databases.Contains(DatabaseName))
                    throw new Exception("(warning)Database already exists");

                //ExecutedQuery = ExecutedQuery.Replace("@FilePath", Sr.BackupDirectory).Replace("@DatabaseName", DatabaseName);

                string[] ExecutedQueries = ExecutedQuery.Split(';');

                SqlConnection Conn = ServerName.SqlConnectionObject;
                if (Conn.State != ConnectionState.Open) Conn.Open();

                SqlCommand Cmd = Conn.CreateCommand();
                Cmd.Connection = Conn;
                Cmd.CommandText = ExecutedQuery;
                Cmd.ExecuteNonQuery();
            }
            catch (Exception X)
            {
                throw X;
            }
        }

        public static void Restore_FromBakFile(string ConnectionString, string BackupFile)
        {
            try
            {
                string DatabaseName;
                SqlConnectionStringBuilder aa = new SqlConnectionStringBuilder(ConnectionString);
                DatabaseName = aa.InitialCatalog;
                aa.InitialCatalog = "master";
                ServerConnection Conn = new ServerConnection(new SqlConnection(aa.ConnectionString));

                Restore_FromBakFile(Conn, DatabaseName, BackupFile);
            }
            catch (Exception X)
            {
                throw X;
            }
        }
        public static void Restore_FromBakFile(ServerConnection ServerName, string DatabaseName, string BackupFile)
        {
            try
            {
                Microsoft.SqlServer.Management.Smo.Server Sr = new Server(ServerName);

                string newPath =  "\\tempDB.bak";

                File.Copy(BackupFile, newPath, true);

                string ExecutedQuery =
                    "RESTORE DATABASE [@DatabaseName] " +
                    "FROM  DISK = N'@BackupPath' WITH  FILE = 1, " +
                    "MOVE N'AHAVADB' TO N'@FilePath\\@DatabaseName.mdf', " +
                    "MOVE N'AHAVADB_log' TO N'@FilePath\\@DatabaseName_log.ldf',  NOUNLOAD, REPLACE, STATS = 10";

                ExecutedQuery =
                    ExecutedQuery
                    .Replace("@BackupPath", newPath)
                    .Replace("@FilePath", "")
                    .Replace("@DatabaseName", DatabaseName);

                SqlConnection Conn = ServerName.SqlConnectionObject;
                if (Conn.State != ConnectionState.Open) Conn.Open();

                SqlCommand Cmd = Conn.CreateCommand();
                Cmd.Connection = Conn;
                Cmd.CommandText = ExecutedQuery;
                Cmd.ExecuteNonQuery();

                File.Delete(newPath);
            }
            catch (Exception X)
            {
                throw X;
            }
        }

        public static void Execute_SqlScriptFile(string SqlScriptURI, string SqlConnStr, out string Status)
        {
            string SqlScript = "";
            Status = "";

            try
            {
                if (!File.Exists(SqlScriptURI))
                    throw new Exception("(warning)File '" + SqlScriptURI + "' is not found");

                SqlScript = File.ReadAllText(SqlScriptURI);
                //SqlScript = SqlScript.Replace("\r", "");
                //SqlScript = SqlScript.Replace("\n", "");

                SqlConnection SqlConn = new SqlConnection(SqlConnStr);
                SqlConn.Open();
                Server server = new Server( new ServerConnection(SqlConn));
                server.ConnectionContext.ExecuteNonQuery(SqlScript);

                Status = SqlScript;
            }
            catch (Exception X)
            {
                Status = X.Message;
                for (Exception lX = X.InnerException; lX != null; lX = lX.InnerException)
                {
                    Status += Environment.NewLine + lX.Message;
                }
            }
        }

        public static void WriteToLog()
        {

        }
    }
}
