using System.Configuration;

namespace DataBridge.SqlServer.Configuration
{
    public class SqlServerSourceTableConfigElement : ConfigurationElement
    {
        [ConfigurationProperty("id", IsKey = true, IsRequired = true)]
        public string Id
        {
            get { return (string)base["id"]; }
            set { base["id"] = value; }
        }

        [ConfigurationProperty("name", IsKey = false, IsRequired = true)]
        public string Name
        {
            get { return (string) base["name"]; }
            set { base["name"] = value; }
        }

        [ConfigurationProperty("schema", IsKey = false, IsRequired = true)]
        public string Schema
        {
            get { return (string)base["schema"]; }
            set { base["schema"] = value; }
        }

        [ConfigurationProperty("changeDetectionMode", IsKey = false, IsRequired = true)]
        public int ChangeDetectionMode
        {
            get { return (int)base["changeDetectionMode"]; }
            set { base["changeDetectionMode"] = value; }
        }

        [ConfigurationProperty("pollIntervalInMilliseconds", IsKey = false, IsRequired = true)]
        public uint PollIntervalInMilliseconds
        {
            get { return (uint)base["pollIntervalInMilliseconds"]; }
            set { base["pollIntervalInMilliseconds"] = value; }
        }

        [ConfigurationProperty("qualityCheckIntervalInMilliseconds", IsKey = false, IsRequired = true)]
        public uint QualityCheckIntervalInMilliseconds
        {
            get { return (uint)base["qualityCheckIntervalInMilliseconds"]; }
            set { base["qualityCheckIntervalInMilliseconds"] = value; }
        }

        [ConfigurationProperty("qualityCheckRecordBatchSize", IsKey = false, IsRequired = true)]
        public uint QualityCheckRecordBatchSize
        {
            get { return (uint)base["qualityCheckRecordBatchSize"]; }
            set { base["qualityCheckRecordBatchSize"] = value; }
        }
    }
}