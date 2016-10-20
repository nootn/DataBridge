using System.Collections.Generic;

namespace DataBridge.Core.Interface
{
    public interface ISourceDatabase
    {
        IEnumerable<TableConfiguration> Tables { get; }

        void EnsureChangeTrackingIsConfigured();

        void CommenceChangeTracking();

        void ConfigureQualityCheck();
    }
}