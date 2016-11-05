using System.Collections.Generic;

namespace DataBridge.Core.Interface
{
    public interface IDestinationOfData
    {
        void PossibleChangesFound(SourceTableConfiguration sourceTable, IList<TableRowData> changes);
    }
}