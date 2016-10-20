namespace DataBridge.Core
{
    public class TableSyncSettings
    {
        public enum ChangeDetectionModes
        {
            AsSoonAsPossible = 1,
            Polling = 2
        }

        public TableSyncSettings(ChangeDetectionModes changeDetectionMode, 
            uint pollIntervalInMilliseconds, 
            uint qualityCheckIntervalInMilliseconds,
            uint qualityCheckRecordBatchSize)
        {
            ChangeDetectionMode = changeDetectionMode;
            PollIntervalInMilliseconds = pollIntervalInMilliseconds;
            QualityCheckIntervalInMilliseconds = qualityCheckIntervalInMilliseconds;
            QualityCheckRecordBatchSize = qualityCheckRecordBatchSize;
        }

        public ChangeDetectionModes ChangeDetectionMode { get; }

        public uint PollIntervalInMilliseconds { get; }

        public uint QualityCheckIntervalInMilliseconds { get; }

        public uint QualityCheckRecordBatchSize { get; }
    }
}