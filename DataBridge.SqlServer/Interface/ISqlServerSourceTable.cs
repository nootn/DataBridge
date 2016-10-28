namespace DataBridge.SqlServer.Interface
{
    public interface ISqlServerSourceTable
    {
        string Id { get; }

        string Name { get; }

        string Schema { get; }

        int ChangeDetectionMode { get; }

        int PollIntervalInMilliseconds { get; }

        int QualityCheckIntervalInMilliseconds { get; }

        int QualityCheckRecordBatchSize { get; }
    }
}
