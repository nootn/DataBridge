using DataBridge.SqlServer.Configuration;

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

        string PrimaryKeyColumn { get; }

        bool PrimaryKeyColumnIsNumber { get; }

        string LastUpdatedAtColumn { get; }

        IBasicConfigElementCollection ColumnsToInclude { get; }

        IBasicConfigElementCollection ColumnsToIgnore { get; }
        
    }
}
