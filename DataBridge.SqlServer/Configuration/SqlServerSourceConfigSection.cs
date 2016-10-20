using System.Configuration;

namespace DataBridge.SqlServer.Configuration
{
    public class SqlServerSourceConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("SourceDatabaseConnectionString", IsRequired = true)]
        public SqlDatabaseConnectionStringConfigElement SourceDatabaseConnectionString
        {
            get { return (SqlDatabaseConnectionStringConfigElement) this["SourceDatabaseConnectionString"]; }
            set { this["SourceDatabaseConnectionString"] = value; }
        }

        [ConfigurationProperty("SourceTables", IsRequired = true, IsDefaultCollection = true)]
        public SqlServerSourceTableCollection SourceTables
        {
            get { return (SqlServerSourceTableCollection) this["SourceTables"]; }
            set { this["SourceTables"] = value; }
        }
    }
}