using System;
using System.Collections.Generic;
using System.Configuration;
using DataBridge.SqlServer.Interface;

namespace DataBridge.SqlServer.Configuration
{
    public class SqlServerSourceTableCollection : ConfigurationElementCollection, ISqlServerSourceTableCollection
    {

        protected override ConfigurationElement CreateNewElement()
        {
            return new SqlServerSourceTableConfigElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((SqlServerSourceTableConfigElement) element).Id;
        }

        IEnumerator<ISqlServerSourceTable> IEnumerable<ISqlServerSourceTable>.GetEnumerator()
        {
            var enumerator = base.GetEnumerator();
            while (enumerator.MoveNext())
            {
                yield return (SqlServerSourceTableConfigElement) enumerator.Current;
            }
        }
    }
}