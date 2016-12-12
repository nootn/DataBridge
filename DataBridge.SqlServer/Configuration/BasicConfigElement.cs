using System.Configuration;
using DataBridge.SqlServer.Interface;

namespace DataBridge.SqlServer.Configuration
{
    public class BasicConfigElement : ConfigurationElement, IBasicConfigElement
    {
        [ConfigurationProperty("name", IsKey = false, IsRequired = true)]
        public string Name
        {
            get { return (string) base["name"]; }
            set { base["name"] = value; }
        }
    }
}