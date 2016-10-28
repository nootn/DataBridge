using EnsureThat;

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
            int pollIntervalInMilliseconds, 
            int qualityCheckIntervalInMilliseconds,
            int qualityCheckRecordBatchSize)
        {
            Ensure.That(() => pollIntervalInMilliseconds).IsGt(0);
            Ensure.That(() => qualityCheckIntervalInMilliseconds).IsGt(0);
            Ensure.That(() => qualityCheckRecordBatchSize).IsGt(0);

            ChangeDetectionMode = changeDetectionMode;
            PollIntervalInMilliseconds = pollIntervalInMilliseconds;
            QualityCheckIntervalInMilliseconds = qualityCheckIntervalInMilliseconds;
            QualityCheckRecordBatchSize = qualityCheckRecordBatchSize;
        }

        public ChangeDetectionModes ChangeDetectionMode { get; }

        public int PollIntervalInMilliseconds { get; }

        public int QualityCheckIntervalInMilliseconds { get; }

        public int QualityCheckRecordBatchSize { get; }
    }
}