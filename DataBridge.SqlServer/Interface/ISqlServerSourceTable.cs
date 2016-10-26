namespace DataBridge.SqlServer.Interface
{
    public interface ISqlServerSourceTable
    {
        string Id { get; }

        string Name { get; }

        string Schema { get; }

        int ChangeDetectionMode { get; }

        uint PollIntervalInMilliseconds { get; }

        uint QualityCheckIntervalInMilliseconds { get; }

        uint QualityCheckRecordBatchSize { get; }
    }
}
