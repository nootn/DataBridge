using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using DataBridge.Core;
using DataBridge.Core.Interface;
using EnsureThat;
using Serilog;

namespace DataBridge.SqlServer
{
    internal class SqlChangeDetector
    {
        private readonly string _connectionString;
        private readonly IDestinationOfData _destination;
        private readonly ILogger _log;
        private readonly SourceTableConfiguration _table;
        private bool _isBeingCancelled;

        private long syncVersion = long.MinValue;

        public SqlChangeDetector(string connectionString, SourceTableConfiguration table, IDestinationOfData destination,
            ILogger log)
        {
            Ensure.That(() => connectionString).IsNotNullOrWhiteSpace();
            Ensure.That(() => table).IsNotNull();
            Ensure.That(() => destination).IsNotNull();

            _connectionString = connectionString;
            _table = table;
            _destination = destination;
            _log = log;
        }

        public void CommenceTracking()
        {
            SetSyncVersion();
            DetectChangesOnTable();
        }

        public void CancelWork()
        {
            _isBeingCancelled = true;
        }

        private void SetSyncVersion()
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(
                @"
SELECT CHANGE_TRACKING_CURRENT_VERSION()",
                conn))
            {
                conn.Open();
                var res = command.ExecuteScalar();
                conn.Close();
                if (res != null)
                {
                    syncVersion = (long) res;
                }
            }
        }

        private void DetectChangesOnTable()
        {
            using (var conn = new SqlConnection(_connectionString))
            using (
                var command =
                    new SqlCommand(
                        $@" 
SELECT {string.Join(",", _table.ColumnsToInclude.Select(colName => $"[{colName}]"))} 
FROM [{_table.SchemaName}].[{_table.TableName}]",
                        conn))
            {
                var dependency = new SqlDependency(command);
                dependency.OnChange += OnDependencyChange;
                conn.Open();
                command.ExecuteScalar();
                conn.Close();
            }
        }

        private void OnDependencyChange(object sender, SqlNotificationEventArgs e)
        {
            var syncVersionNow = syncVersion;
            if (_isBeingCancelled)
            {
                _log.Warning("OnDependencyChange Cancelled - {@SqlNotificationEventArgs} Sync Version: {SyncVersion}", e,
                    syncVersionNow);
            }
            else
            {
                try
                {
                    _log.Information("OnDependencyChange {@SqlNotificationEventArgs} Sync Version: {SyncVersion}", e,
                        syncVersionNow);

                    var dependency = sender as SqlDependency;
                    dependency.OnChange -= OnDependencyChange;

                    var insertsAndUpdates = new List<string>();
                    var deletes = new List<string>();

                    using (var conn = new SqlConnection(_connectionString))
                    using (var command = new SqlCommand(
                        $@"
SELECT CAST(CT.Id AS VARCHAR(64)) AS Id, CAST(CT.SYS_CHANGE_OPERATION AS VARCHAR(1)) AS Operation 
FROM CHANGETABLE(CHANGES [{_table.TableName}], @last_synchronization_version) AS CT",
                        conn))
                    {
                        command.Parameters.Add(new SqlParameter("last_synchronization_version", syncVersionNow));
                        conn.Open();
                        var res = command.ExecuteReader();

                        while (res.Read())
                        {
                            if (res.GetString(1) == "D")
                            {
                                deletes.Add(res.GetString(0));
                            }
                            else
                            {
                                insertsAndUpdates.Add(res.GetString(0));
                            }
                        }
                        conn.Close();
                    }

                    SetSyncVersion();

                    DetectChangesOnTable();

                    if (!insertsAndUpdates.Any() && !deletes.Any())
                    {
                        _log.Warning("No changes were found in version {SyncVersion}", syncVersionNow);
                    }
                    else
                    {
                        var rows = new List<TableRowData>();

                        string inClause;
                        if (_table.PrimaryKeyColumnIsNumber)
                        {
                            inClause = string.Join(",", insertsAndUpdates);
                        }
                        else
                        {
                            inClause = "'" + string.Join("','", insertsAndUpdates) + "'";
                        }

                        var colsToIncludeWithoutIdAndLastUpdate =
                            _table.ColumnsToInclude.Where(
                                    _ =>
                                        !string.Equals(_, _table.PrimaryKeyColumn,
                                            StringComparison.CurrentCultureIgnoreCase)
                                        &&
                                        !string.Equals(_, _table.LastUpdatedAtColumn,
                                            StringComparison.CurrentCultureIgnoreCase))
                                .ToList();

                        using (var conn = new SqlConnection(_connectionString))
                        using (
                            var command =
                                new SqlCommand(
                                    $@"
SELECT [{_table.PrimaryKeyColumn}], [{_table.LastUpdatedAtColumn}], {string.Join(",",
                                        colsToIncludeWithoutIdAndLastUpdate.Select(colName => $"[{colName}]"))}
FROM [{_table.SchemaName}].[{_table.TableName}]
WHERE {_table.PrimaryKeyColumn} IN ({inClause})",
                                    conn))
                        {
                            conn.Open();
                            var rdr = command.ExecuteReader();
                            while (rdr.Read())
                            {
                                var otherItems = new List<KeyValuePair<string, object>>();
                                var colIndex = 2;
                                foreach (var otherCol in colsToIncludeWithoutIdAndLastUpdate)
                                {
                                    otherItems.Add(new KeyValuePair<string, object>(otherCol, rdr.GetValue(colIndex)));
                                    colIndex++;
                                }
                                rows.Add(new TableRowData(false, rdr.GetValue(0).ToString(), rdr.GetValue(1).ToString(),
                                    otherItems));
                            }
                            conn.Close();
                        }

                        foreach (var currDelId in deletes)
                        {
                            rows.Add(new TableRowData(true, currDelId, null, null));
                        }

                        _destination.PossibleChangesFound(_table, rows);
                    }
                }
                catch (Exception ex)
                {
                    _log.Error(ex, "OnDependencyChange - Sync Version: {SyncVersion}", syncVersionNow);
                    if (!_isBeingCancelled)
                    {
                        throw;
                    }
                }
            }
        }
    }

}