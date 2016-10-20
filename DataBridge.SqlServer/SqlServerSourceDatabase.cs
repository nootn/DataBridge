using System;
using System.Collections.Generic;
using DataBridge.Core;
using DataBridge.SqlServer.Configuration;
using Serilog;

namespace DataBridge.SqlServer
{
    public class SqlServerSourceDatabase : SourceDatabaseBase
    {
        private readonly SqlServerSourceConfigSection _config;

        public SqlServerSourceDatabase(ILogger log, SqlServerSourceConfigSection config)
        {
            _config = config;
            Log = log;
        }

        public override ILogger Log { get; }

        public override IList<SourceTableConfiguration> GetTableConfig()
        {
            var config = new List<SourceTableConfiguration>();

            foreach (SqlServerSourceTableConfigElement configSourceTable in _config.SourceTables)
            {
                config.Add(new SourceTableConfiguration(configSourceTable.Schema, 
                    configSourceTable.Name,
                    new TableSyncSettings((TableSyncSettings.ChangeDetectionModes)configSourceTable.ChangeDetectionMode,
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