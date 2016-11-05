using System.Collections.Generic;
using EnsureThat;

namespace DataBridge.Core
{
    public class TableRowData
    {
        public TableRowData(string primaryKeyValue, string lastUpdatedAtValue,
            IList<KeyValuePair<string, object>> values)
        {
            Ensure.That(() => primaryKeyValue).IsNotNullOrWhiteSpace();
            Ensure.That(() => lastUpdatedAtValue).IsNotNullOrWhiteSpace();

            PrimaryKeyValue = primaryKeyValue;
            LastUpdatedAtValue = lastUpdatedAtValue;
            Values = values;
        }

        public string PrimaryKeyValue { get; }

        public string LastUpdatedAtValue { get; }

        public IList<KeyValuePair<string, object>> Values { get; }
    }
}