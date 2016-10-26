using System.Collections.Generic;
using DataBridge.Core;
using DataBridge.SqlServer.Interface;
using Serilog;

namespace DataBridge.SqlServer
{
    public class SqlServerSourceDatabase : SourceDatabaseBase
    {
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

            return config;
        }

        public override void EnsureChangeTrackingIsConfigured()
        {
        }

        public override void CommenceChangeTracking(IEnumerable<SourceTableConfiguration> tables)
        {
        }

        public override void RunQualityCheck(SourceTableConfiguration table)
        {
        }
    }
}