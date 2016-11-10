using System;
using System.Data.SqlClient;
using DataBridge.Core;
using DataBridge.SqlServer.Extensions;
using EnsureThat;

namespace DataBridge.SqlServer
{
    internal static class SqlRunner
    {
        public enum ActionTaken
        {
            Unknown = 0,
            Successful = 1,
            None = 2
        }

        private const int ActionTakenErrorTableNotFound = -1;

        public static ActionTaken SetChangeTrackingOnSourceDatabase(SqlConnection conn, string sourceDatabaseName,
            int numDaysRetentionInitialValue)
        {
            Ensure.That(() => conn).IsNotNull();
            Ensure.That(() => sourceDatabaseName).IsNotNullOrWhiteSpace();
            Ensure.That(() => numDaysRetentionInitialValue).IsGt(0);

            conn.EnsureOpen();

            var cmdText = $@"
DECLARE @actionTaken INT
SET @actionTaken = {(int) ActionTaken.Unknown}
IF NOT EXISTS 
    (SELECT * FROM sys.change_tracking_databases 
     WHERE database_id = DB_ID('@sourceDatabaseName')) 
    BEGIN 
        ALTER DATABASE [{sourceDatabaseName}] 
        SET CHANGE_TRACKING = ON 
            (CHANGE_RETENTION = {numDaysRetentionInitialValue} DAYS, AUTO_CLEANUP = ON) 
        SET @actionTaken = {(int) ActionTaken.Successful}
    END
ELSE
    BEGIN
        SET @actionTaken = {(int) ActionTaken.None}
    END
SELECT @actionTaken
";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.Parameters.AddWithValue("sourceDatabaseName", sourceDatabaseName);
            var res = Convert.ToInt32(cmd.ExecuteScalar());
            return (ActionTaken) res;
        }

        public static ActionTaken SetChangeTrackingOnTable(SourceTableConfiguration currTable, SqlConnection conn)
        {
            Ensure.That(() => currTable).IsNotNull();
            Ensure.That(() => conn).IsNotNull();

            conn.EnsureOpen();

            var cmdText = $@"
DECLARE @actionTaken INT
SET @actionTaken = {(int) ActionTaken.Unknown}
IF NOT EXISTS
    (SELECT sys.tables.name
     FROM sys.tables JOIN 
     sys.schemas ON sys.schemas.schema_id = sys.tables.schema_id 
     WHERE sys.tables.name = '@sourceTableName' AND sys.schemas.name = '@sourceSchemaName') 
    BEGIN
        SET @actionTaken = {ActionTakenErrorTableNotFound}
    END
ELSE
    BEGIN
        IF NOT EXISTS 
            (SELECT sys.tables.name 
             FROM sys.change_tracking_tables JOIN 
             sys.tables ON sys.tables.object_id = sys.change_tracking_tables.object_id JOIN 
             sys.schemas ON sys.schemas.schema_id = sys.tables.schema_id 
             WHERE sys.tables.name = '@sourceTableName' AND sys.schemas.name = '@sourceSchemaName') 
            BEGIN 
                ALTER TABLE [{currTable.SchemaName}].[{currTable.TableName}] 
                ENABLE CHANGE_TRACKING WITH 
                    (TRACK_COLUMNS_UPDATED = OFF)
                SET @actionTaken = {(int) ActionTaken.Successful}
            END
        ELSE
            BEGIN
                SET @actionTaken = {(int) ActionTaken.None}
            END
    END
SELECT @actionTaken
";

            var cmd = new SqlCommand(cmdText, conn);
            cmd.Parameters.AddWithValue("sourceTableName", currTable.TableName);
            cmd.Parameters.AddWithValue("sourceSchemaName", currTable.SchemaName);

            var res = Convert.ToInt32(cmd.ExecuteScalar());

            if (res == ActionTakenErrorTableNotFound)
            {
                throw new ApplicationException(
                    $"{SqlServerSourceDatabase.ExceptionMessagePrefixTableNotExists} - [{currTable.SchemaName}].[{currTable.TableName}]");
            }

            return (ActionTaken) res;
        }
    }
}