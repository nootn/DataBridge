using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Serilog;
using Serilog.Events;

namespace DataBridge.Core
{
    public abstract class SourceDatabaseBase
    {
        public static readonly string ExceptionMessageNoTablesConfiguredPrefix =
            "There were no tables to configure - nothing to do!  Please configure some tables in GetTableConfig()";

        private readonly Dictionary<string, Timer> _tablePollTimers = new Dictionary<string, Timer>();
        private readonly Dictionary<string, Timer> _tableQualityCheckTimers = new Dictionary<string, Timer>();

        public abstract ILogger Log { get; }

        public void Process()
        {
            Log.Debug("Starting Process()");

            var tables = GetTableConfig();
            Log.Debug("Processing table(s) {@Tables}", tables);

            if ((tables == null) || !tables.Any())
            {
                throw new ApplicationException(ExceptionMessageNoTablesConfiguredPrefix);
            }

            using (Log.BeginTimedOperation("Ensure Change Tracking Is Configured", level: LogEventLevel.Debug))
            {
                EnsureChangeTrackingIsConfigured(tables);
            }

            //Start processing changes now!
            using (Log.BeginTimedOperation("Commence Tracking Changes Setup", level: LogEventLevel.Debug))
            {
                foreach (var currTable in tables)
                {
                    if (currTable.SyncSettings.ChangeDetectionMode == TableSyncSettings.ChangeDetectionModes.Polling)
                    {
                        var timer = new Timer(PollTimerCallback,
                            currTable,
                            0,
                            currTable.SyncSettings.PollIntervalInMilliseconds);
                        _tablePollTimers.Add(currTable.TableId, timer);
                    }
                    else if (currTable.SyncSettings.ChangeDetectionMode ==
                             TableSyncSettings.ChangeDetectionModes.AsSoonAsPossible)
                    {
                        using (Log.BeginTimedOperation("Commmencing ASAP Tracking", currTable.TableId, LogEventLevel.Debug))
                        {
                            CommmenceTrackingChanges(currTable);
                        }
                    }
                    else
                    {
                        throw new ApplicationException($"Unknown Change Detection Mode {currTable.SyncSettings.ChangeDetectionMode} on table {currTable.TableId}");
                    }
                }
            }

            //Now that we are tracking, kick off a background process to periodically do a quality check on the specified interval for each table
            foreach (var currTable in tables)
            {
                var timer = new Timer(QualityCheckTimerCallback,
                    currTable,
                    currTable.SyncSettings.QualityCheckIntervalInMilliseconds,
                    currTable.SyncSettings.QualityCheckIntervalInMilliseconds);
                _tableQualityCheckTimers.Add(currTable.TableId, timer);
            }
        }

        private void PollTimerCallback(object tableConfig)
        {
            var table = tableConfig as SourceTableConfiguration;
            if (table != null)
            {
                using (Log.BeginTimedOperation("Performing poll", table.TableId, LogEventLevel.Debug))
                {
                    PollForChanges(table);
                }
            }
            else
            {
                Log.Warning("Poll timer elapsed but object passed could not be processed: {@ArgumentPassed}", tableConfig);
            }
        }

        private void QualityCheckTimerCallback(object tableConfig)
        {
            var table = tableConfig as SourceTableConfiguration;
            if (table != null)
            {
                using (Log.BeginTimedOperation("Performing quality check", table.TableId, LogEventLevel.Debug))
                {
                    RunQualityCheck(table);
                }
            }
            else
            {
                Log.Warning("Quality check timer elapsed but object passed could not be processed: {@ArgumentPassed}", tableConfig);
            }
        }

        public abstract IList<SourceTableConfiguration> GetTableConfig();

        public abstract void EnsureChangeTrackingIsConfigured(IEnumerable<SourceTableConfiguration> tables);

        public abstract void CommmenceTrackingChanges(SourceTableConfiguration table);

        public abstract void PollForChanges(SourceTableConfiguration table);

        public abstract void RunQualityCheck(SourceTableConfiguration table);
    }
}