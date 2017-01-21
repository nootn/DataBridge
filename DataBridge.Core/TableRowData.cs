using System.Collections.Generic;
using EnsureThat;

namespace DataBridge.Core
{
    public class TableRowData
    {
        public TableRowData(bool isBeingDeleted, string primaryKeyValue, string lastUpdatedAtValue,
            IList<KeyValuePair<string, object>> values)
        {
            Ensure.That(() => primaryKeyValue).IsNotNullOrWhiteSpace();

            if (!isBeingDeleted)
            {
                Ensure.That(() => lastUpdatedAtValue).IsNotNullOrWhiteSpace();
                Ensure.That(() => values).IsNotNull();
            }
            
            IsBeingDeleted = isBeingDeleted;
            PrimaryKeyValue = primaryKeyValue;
            LastUpdatedAtValue = lastUpdatedAtValue;
            Values = values;
        }

        public bool IsBeingDeleted { get; }

        public string PrimaryKeyValue { get; }

        public string LastUpdatedAtValue { get; }

        public IList<KeyValuePair<string, object>> Values { get; }
    }
}