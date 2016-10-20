using System.Configuration;

namespace DataBridge.SqlServer.Configuration
{
    public class SqlServerSourceTableCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new SqlServerSourceTableConfigElement();
        }   

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((SqlServerSourceTableConfigElement)element).Id;
        }
    }
}