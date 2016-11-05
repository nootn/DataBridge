using System;
using System.Collections.Generic;
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

        public static readonly string ExceptionMessagePrefixTableNotExists = "Cannot find table";

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
                            configSourceTable.QualityCheckRecordBatchSize),
                        configSourceTable.PrimaryKeyColumn,
                        configSourceTable.LastUpdatedAtColumn));
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

                var setupTrackingOnDb = SqlRunner.SetChangeTrackingOnSourceDatabase(conn, sourceDatabaseName,
                    _config.ChangeTrackingRetentionInitialValueInDays);
                Log.Debug("{DatabaseName}: Action taken to setup tracking on database: {ActionTakenOnDb}",
                    sourceDatabaseName, setupTrackingOnDb);

                foreach (var currTable in tables)
                {
                    var setupTrackingOnTable = SqlRunner.SetChangeTrackingOnTable(currTable, conn);
                    Log.Debug("{DatabaseName} - {TableId}: Action taken to setup tracking on table: {ActionTakenOnDb}",
                        sourceDatabaseName, currTable.TableId, setupTrackingOnTable);
                }

                conn.Close();
            }
        }

        public override void CommmenceTrackingChanges(SourceTableConfiguration table)
        {
        }

        public override void PollForChanges(SourceTableConfiguration table)
        {
        }

        public override void RunQualityCheck(SourceTableConfiguration table)
        {
        }
    }
}