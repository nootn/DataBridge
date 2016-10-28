using EnsureThat;

namespace DataBridge.Core
{
    public class SourceTableConfiguration
    {
        public SourceTableConfiguration(string schemaName, string tableName, TableSyncSettings syncSettings)
        {
            Ensure.That(() => schemaName).IsNotNullOrWhiteSpace();
            Ensure.That(() => tableName).IsNotNullOrWhiteSpace();

            SchemaName = schemaName;
            TableName = tableName;
            SyncSettings = syncSettings;
        }

        public string SchemaName { get; }

        public string TableName { get; }

        public TableSyncSettings SyncSettings { get; }

        public string TableId => string.Concat(SchemaName, ".", TableName);
    }
}