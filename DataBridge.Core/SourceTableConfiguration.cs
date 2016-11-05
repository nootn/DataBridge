using EnsureThat;

namespace DataBridge.Core
{
    public class SourceTableConfiguration
    {
        public SourceTableConfiguration(string schemaName, string tableName, TableSyncSettings syncSettings, string primaryKeyColumn, string lastUpdatedAtColumn)
        {
            Ensure.That(() => schemaName).IsNotNullOrWhiteSpace();
            Ensure.That(() => tableName).IsNotNullOrWhiteSpace();
            Ensure.That(() => syncSettings).IsNotNull();
            Ensure.That(() => primaryKeyColumn).IsNotNullOrWhiteSpace();
            Ensure.That(() => lastUpdatedAtColumn).IsNotNullOrWhiteSpace();

            SchemaName = schemaName;
            TableName = tableName;
            SyncSettings = syncSettings;
            PrimaryKeyColumn = primaryKeyColumn;
            LastUpdatedAtColumn = lastUpdatedAtColumn;
        }

        public string SchemaName { get; }

        public string TableName { get; }

        public TableSyncSettings SyncSettings { get; }

        public string TableId => string.Concat(SchemaName, ".", TableName);

        public string PrimaryKeyColumn { get; }

        public string LastUpdatedAtColumn { get; }
    }
}