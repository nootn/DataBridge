using EnsureThat;

namespace DataBridge.Core
{
    public class SourceTableConfiguration
    {
        public SourceTableConfiguration(string schemaName, string tableName, TableSyncSettings syncSettings, 
            string primaryKeyColumn, string lastUpdatedAtColumn, string[] columnsToInclude, string[] columnsToIgnore)
        {
            Ensure.That(() => schemaName).IsNotNullOrWhiteSpace();
            Ensure.That(() => tableName).IsNotNullOrWhiteSpace();
            Ensure.That(() => syncSettings).IsNotNull();
            Ensure.That(() => primaryKeyColumn).IsNotNullOrWhiteSpace();
            Ensure.That(() => lastUpdatedAtColumn).IsNotNullOrWhiteSpace();
            Ensure.That(() => columnsToInclude).IsNotNull();
            Ensure.That(() => columnsToInclude).HasItems();


            SchemaName = schemaName;
            TableName = tableName;
            SyncSettings = syncSettings;
            PrimaryKeyColumn = primaryKeyColumn;
            LastUpdatedAtColumn = lastUpdatedAtColumn;
            ColumnsToInclude = columnsToInclude;
            ColumnsToIgnore = columnsToIgnore;
        }

        public string SchemaName { get; }

        public string TableName { get; }

        public TableSyncSettings SyncSettings { get; }

        public string TableId => string.Concat(SchemaName, ".", TableName);

        public string PrimaryKeyColumn { get; }

        public string LastUpdatedAtColumn { get; }

        public string[] ColumnsToInclude { get; }

        public string[] ColumnsToIgnore { get; }
    }
}