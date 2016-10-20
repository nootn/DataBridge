namespace DataBridge.Core
{
    public class TableSyncSettings
    {
        public enum ChangeDetectionModes
        {
            AsSoonAsPossible = 1,
            Polling = 2
        }

        public TableSyncSettings(ChangeDetectionModes changeDetectionMode, uint pollIntervalInMilliseconds, uint qualityCheckIntervalInMilliseconds)
        {
            ChangeDetectionMode = changeDetectionMode;
            PollIntervalInMilliseconds = pollIntervalInMilliseconds;
            QualityCheckIntervalInMilliseconds = qualityCheckIntervalInMilliseconds;
        }

        public ChangeDetectionModes ChangeDetectionMode { get; }

        public uint PollIntervalInMilliseconds { get; }

        public uint QualityCheckIntervalInMilliseconds { get; }
    }
}