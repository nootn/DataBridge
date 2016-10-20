using System.Configuration;

namespace DataBridge.SqlServer.Configuration
{
    public class SqlDatabaseConnectionStringConfigElement : ConfigurationElement
    {
        [ConfigurationProperty("value", IsKey = true, IsRequired = true)]
        public string Value
        {
            get { return (string) base["value"]; }
            set { base["value"] = value; }
        }
    }
}