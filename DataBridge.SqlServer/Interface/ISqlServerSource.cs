namespace DataBridge.SqlServer.Interface
{
    public interface ISqlServerSource
    {
        ISqlDatabaseConnectionString SourceDatabaseConnectionString { get; }
        
        int ChangeTrackingRetentionInitialValueInDays { get; }

        ISqlServerSourceTableCollection SourceTables { get; }
    }
}