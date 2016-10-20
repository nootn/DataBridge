namespace DataBridge.Core
{
    public class TableConfiguration
    {
        public TableConfiguration(string schemaName, string tableName, TableSyncSettings syncSettings)
        {
            SchemaName = schemaName;
            TableName = tableName;
            SyncSettings = syncSettings;
        }

        public string SchemaName { get; }

        public string TableName { get; }

        public TableSyncSettings SyncSettings { get; }
    }
}