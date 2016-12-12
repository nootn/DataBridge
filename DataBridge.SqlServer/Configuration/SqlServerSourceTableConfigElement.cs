using System.Configuration;
using DataBridge.SqlServer.Interface;

namespace DataBridge.SqlServer.Configuration
{
    public class SqlServerSourceTableConfigElement : ConfigurationElement, ISqlServerSourceTable
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
        public int PollIntervalInMilliseconds
        {
            get { return (int)base["pollIntervalInMilliseconds"]; }
            set { base["pollIntervalInMilliseconds"] = value; }
        }

        [ConfigurationProperty("qualityCheckIntervalInMilliseconds", IsKey = false, IsRequired = true)]
        public int QualityCheckIntervalInMilliseconds
        {
            get { return (int)base["qualityCheckIntervalInMilliseconds"]; }
            set { base["qualityCheckIntervalInMilliseconds"] = value; }
        }

        [ConfigurationProperty("qualityCheckRecordBatchSize", IsKey = false, IsRequired = true)]
        public int QualityCheckRecordBatchSize
        {
            get { return (int)base["qualityCheckRecordBatchSize"]; }
            set { base["qualityCheckRecordBatchSize"] = value; }
        }

        [ConfigurationProperty("primaryKeyColumn", IsKey = false, IsRequired = true)]
        public string PrimaryKeyColumn
        {
            get { return (string)base["primaryKeyColumn"]; }
            set { base["primaryKeyColumn"] = value; }
        }

        [ConfigurationProperty("lastUpdatedAtColumn", IsKey = false, IsRequired = true)]
        public string LastUpdatedAtColumn
        {
            get { return (string)base["lastUpdatedAtColumn"]; }
            set { base["lastUpdatedAtColumn"] = value; }
        }

        [ConfigurationProperty("ColumnsToInclude", IsRequired = true, IsDefaultCollection = true)]
        public BasicConfigElementCollection ColumnsToInclude
        {
            get { return (BasicConfigElementCollection)this["ColumnsToInclude"]; }
            set { this["ColumnsToInclude"] = value; }
        }

        [ConfigurationProperty("ColumnsToIgnore", IsRequired = true, IsDefaultCollection = false)]
        public BasicConfigElementCollection ColumnsToIgnore
        {
            get { return (BasicConfigElementCollection)this["ColumnsToIgnore"]; }
            set { this["ColumnsToIgnore"] = value; }
        }
        
        IBasicConfigElementCollection ISqlServerSourceTable.ColumnsToInclude => ColumnsToInclude;

        IBasicConfigElementCollection ISqlServerSourceTable.ColumnsToIgnore => ColumnsToIgnore;
    }
}