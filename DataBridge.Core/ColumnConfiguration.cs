namespace DataBridge.Core
{
    public class ColumnConfiguration
    {
        public ColumnConfiguration(string columnName, bool isChangeTrackable)
        {
            ColumnName = columnName;
            IsChangeTrackable = isChangeTrackable;
        }

        public string ColumnName { get; }

        public bool IsChangeTrackable { get; }
    }
}