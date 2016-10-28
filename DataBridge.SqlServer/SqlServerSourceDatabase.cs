using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using DataBridge.Core;
using DataBridge.SqlServer.Interface;
using Serilog;

namespace DataBridge.SqlServer
{
    public class SqlServerSourceDatabase : SourceDatabaseBase
    {
        public static readonly string ExceptionMessageInvalidSourceDatabase =
            "Invalid Source Database";

        private readonly ISqlServerSource _config;

        public SqlServerSourceDatabase(ILogger log, ISqlServerSource config)
        {
            _config = config;
            Log = log;
        }

        public override ILogger Log { get; }

        public override IList<SourceTableConfiguration> GetTableConfig()
        {
            var config = new List<SourceTableConfiguration>();

            if (_config.SourceTables != null)
            {
                foreach (var configSourceTable in _config.SourceTables)
                {
                    config.Add(new SourceTableConfiguration(configSourceTable.Schema,
                        configSourceTable.Name,
                        new TableSyncSettings(
                            (TableSyncSettings.ChangeDetectionModes) configSourceTable.ChangeDetectionMode,
                            configSourceTable.PollIntervalInMilliseconds,
                            configSourceTable.QualityCheckIntervalInMilliseconds,
                            configSourceTable.QualityCheckRecordBatchSize)));
                }
            }

            return config;
        }

        public override void EnsureChangeTrackingIsConfigured(IEnumerable<SourceTableConfiguration> tables)
        {
            using (var conn = new SqlConnection(_config.SourceDatabaseConnectionString.Value))
            {
                try
                {
                    conn.Open();
                }
                catch (Exception ex)
                {
                    throw new ApplicationException(ExceptionMessageInvalidSourceDatabase, ex);
                }

                var sourceDatabaseName = conn.Database;

                SetChangeTrackingOnSourceDatabase(conn, sourceDatabaseName, _config.ChangeTrackingRetentionInitialValueInDays);

                conn.Close();
            }
        }

        public override void CommenceChangeTracking(IEnumerable<SourceTableConfiguration> tables)
        {
        }

        public override void RunQualityCheck(SourceTableConfiguration table)
        {
        }


        private static void SetChangeTrackingOnSourceDatabase(SqlConnection conn, string sourceDatabaseName,
            int numDaysRetentionInitialValue)
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }

            var cmdText = $@"
IF NOT EXISTS 
    (SELECT * FROM sys.change_tracking_databases 
     WHERE database_id = DB_ID('@sourceDatabaseName')) 
    BEGIN 
        ALTER DATABASE [@sourceDatabaseName] 
        SET CHANGE_TRACKING = ON 
            (CHANGE_RETENTION = {numDaysRetentionInitialValue} DAYS, AUTO_CLEANUP = ON) 
    END
";

            var cmd = new SqlCommand(cmdText, conn);
            cmd.Parameters.AddWithValue("sourceDatabaseName", sourceDatabaseName);
            cmd.ExecuteNonQuery();
        }
    }
}