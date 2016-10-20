using System;
using System.Collections.Generic;
using System.Configuration;
using DataBridge.Core;
using DataBridge.SqlServer.Configuration;
using Serilog;

namespace DataBridge.SqlServer
{
    public class SqlServerSourceDatabase : SourceDatabaseBase
    {

        public SqlServerSourceDatabase(ILogger log)
        {
            Log = log;

            var config = ConfigurationManager.GetSection("SqlServerSource")
                 as SqlServerSourceConfigSection;


        }

        public override ILogger Log { get; }

        public override IList<SourceTableConfiguration> GetTableConfig()
        {
            throw new NotImplementedException();
        }

        public override void EnsureChangeTrackingIsConfigured()
        {
            throw new NotImplementedException();
        }

        public override void CommenceChangeTracking(IEnumerable<SourceTableConfiguration> tables)
        {
            throw new NotImplementedException();
        }

        public override void RunQualityCheck(SourceTableConfiguration table)
        {
            throw new NotImplementedException();
        }
    }
}