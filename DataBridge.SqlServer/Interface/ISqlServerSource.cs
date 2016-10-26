namespace DataBridge.SqlServer.Interface
{
    public interface ISqlServerSource
    {
        ISqlDatabaseConnectionString SourceDatabaseConnectionString { get; }

        ISqlServerSourceTableCollection SourceTables { get; }
    }
}