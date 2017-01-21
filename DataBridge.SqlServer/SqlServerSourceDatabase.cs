using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using DataBridge.Core;
using DataBridge.Core.Interface;
using DataBridge.SqlServer.Interface;
using Serilog;

namespace DataBridge.SqlServer
{
    public class SqlServerSourceDatabase : SourceDatabaseBase, IDisposable
    {
        public static readonly string ExceptionMessageInvalidSourceDatabase =
            "Invalid Source Database";

        public static readonly string ExceptionMessagePrefixTableNotExists = "Cannot find table";

        private readonly ISqlServerSource _config;
        private readonly IDestinationOfData _destination;

        private List<SqlChangeDetector> _detectors = new List<SqlChangeDetector>();

        public SqlServerSourceDatabase(ILogger log, ISqlServerSource config, IDestinationOfData destination)
        {
            _config = config;
            _destination = destination;
            Log = log;
        }

        public override ILogger Log { get; protected set; }

#if DEBUG
        public void OverrideLogger(ILogger log)
        {
            Log = log;
        }
#endif

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
                            (TableSyncSettings.ChangeDetectionModes)configSourceTable.ChangeDetectionMode,
                            configSourceTable.PollIntervalInMilliseconds,
                            configSourceTable.QualityCheckIntervalInMilliseconds,
                            configSourceTable.QualityCheckRecordBatchSize),
                        configSourceTable.PrimaryKeyColumn,
                        configSourceTable.PrimaryKeyColumnIsNumber,
                        configSourceTable.LastUpdatedAtColumn,
                        configSourceTable.ColumnsToInclude?.Select(_ => _.Name).ToArray(),
                        configSourceTable.ColumnsToIgnore?.Select(_ => _.Name).ToArray()));
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
                Log.Information("{DatabaseName}: Action taken to setup tracking on database: {ActionTakenOnDb}",
                    sourceDatabaseName, setupTrackingOnDb);

                foreach (var currTable in tables)
                {
                    var setupTrackingOnTable = SqlRunner.SetChangeTrackingOnTable(currTable, conn);
                    Log.Information("{DatabaseName} - {TableId}: Action taken to setup tracking on table: {ActionTakenOnDb}",
                        sourceDatabaseName, currTable.TableId, setupTrackingOnTable);
                }

                conn.Close();
            }

            SqlDependency.Start(_config.SourceDatabaseConnectionString.Value);
        }

        public override void CommmenceTrackingChanges(SourceTableConfiguration table)
        {
            var detector = new SqlChangeDetector(_config.SourceDatabaseConnectionString.Value,
                table, _destination, Log.ForContext<SqlChangeDetector>());
            _detectors.Add(detector);
            detector.CommenceTracking();
        }

        public override void PollForChanges(SourceTableConfiguration table)
        {
        }

        public override void RunQualityCheck(SourceTableConfiguration table)
        {
        }

        public void Dispose()
        {
            foreach (var currDetector in _detectors)
            {
                currDetector.CancelWork();
            }
        }
    }
}