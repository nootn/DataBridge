using System.Configuration;
using DataBridge.SqlServer.Interface;

namespace DataBridge.SqlServer.Configuration
{
    public class SqlServerSourceConfigSection : ConfigurationSection, ISqlServerSource
    {
        [ConfigurationProperty("SourceDatabaseConnectionString", IsRequired = true)]
        public SqlDatabaseConnectionStringConfigElement SourceDatabaseConnectionString
        {
            get { return (SqlDatabaseConnectionStringConfigElement) this["SourceDatabaseConnectionString"]; }
            set { this["SourceDatabaseConnectionString"] = value; }
        }

        [ConfigurationProperty("ChangeTrackingRetentionInitialValueInDays", IsRequired = true)]
        public int ChangeTrackingRetentionInitialValueInDays
        {
            get { return (int)this["ChangeTrackingRetentionInitialValueInDays"]; }
            set { this["ChangeTrackingRetentionInitialValueInDays"] = value; }
        }

        [ConfigurationProperty("SourceTables", IsRequired = true, IsDefaultCollection = true)]
        public SqlServerSourceTableCollection SourceTables
        {
            get { return (SqlServerSourceTableCollection) this["SourceTables"]; }
            set { this["SourceTables"] = value; }
        }

        ISqlDatabaseConnectionString ISqlServerSource.SourceDatabaseConnectionString => SourceDatabaseConnectionString;

        ISqlServerSourceTableCollection ISqlServerSource.SourceTables => SourceTables;
    }
}