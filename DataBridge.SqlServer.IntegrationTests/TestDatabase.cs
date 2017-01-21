using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace DataBridge.SqlServer.IntegrationTests
{
    internal static class TestDatabase
    {
        private const int SqlErrorCodeDatabaseNotExists = -2146232060;

        public static void EnsureCleanDatabaseExists(string dbConnString, string msdbConnString)
        {
            var alreadyExists = false;
            string dbName = null;
            using (var conn = new SqlConnection(dbConnString))
            {
                dbName = conn.Database;
                try
                {
                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }
                    alreadyExists = true;
                    conn.Close();
                }
                catch (SqlException ex)
                {
                    if (ex.ErrorCode == SqlErrorCodeDatabaseNotExists)
                    {
                        alreadyExists = false;
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            if (alreadyExists)
            {
                KillDatabaseConnections(msdbConnString, dbName);
                DropDatabase(msdbConnString, dbName);
            }

            CreateDatabase(msdbConnString, dbName);
            CreateTableOne(dbConnString);
        }

        private static void CreateDatabase(string msdbConnString, string dbName)
        {
            using (var msdbConn = new SqlConnection(msdbConnString))
            {
                msdbConn.Open();
                var msdbCmd = new SqlCommand($"CREATE DATABASE [{dbName}]", msdbConn);
                msdbCmd.ExecuteNonQuery();
                msdbConn.Close();
            }
        }

        private static void KillDatabaseConnections(string msdbConnString, string dbName)
        {
            using (var msdbConn = new SqlConnection(msdbConnString))
            {
                msdbConn.Open();
                var msdbCmd = new SqlCommand($"ALTER DATABASE [{dbName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE", msdbConn);
                msdbCmd.ExecuteNonQuery();
                msdbConn.Close();
            }
        }

        private static void DropDatabase(string msdbConnString, string dbName)
        {
            using (var msdbConn = new SqlConnection(msdbConnString))
            {
                msdbConn.Open();
                var msdbCmd = new SqlCommand($"DROP DATABASE [{dbName}]", msdbConn);
                msdbCmd.ExecuteNonQuery();
                msdbConn.Close();
            }
        }

        private static void CreateTableOne(string dbConnString)
        {
            using (var dbConn = new SqlConnection(dbConnString))
            {
                dbConn.Open();
                var cmd = new SqlCommand($@"
CREATE TABLE TableOne
(
Id int IDENTITY(1,1) PRIMARY KEY,
LastUpdatedOnUtc DATETIME NOT NULL,
Name varchar(255),
DateOfBirthUtc DATETIME
)"
                    , dbConn);
                cmd.ExecuteNonQuery();
                dbConn.Close();
            }
        }

        public static void InsertOneRowIntoTableOne(string dbConnString)
        {
            using (var dbConn = new SqlConnection(dbConnString))
            {
                dbConn.Open();
                var cmd = new SqlCommand($@"
INSERT INTO TableOne (LastUpdatedOnUtc, Name, DateOfBirthUtc)
VALUES ('{DateTime.UtcNow.ToString("yyyy-MM-dd HH:m:ss", DateTimeFormatInfo.CurrentInfo)}', 'One', '2000-01-01 13:00:00')"
                    , dbConn);
                cmd.ExecuteNonQuery();
                dbConn.Close();
            }
        }
    }
}