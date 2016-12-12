using System;
using System.Collections.Generic;
using System.Configuration;
using DataBridge.SqlServer.Interface;

namespace DataBridge.SqlServer.Configuration
{
    public class BasicConfigElementCollection : ConfigurationElementCollection, IBasicConfigElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new BasicConfigElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((BasicConfigElement) element).Name;
        }

        IEnumerator<IBasicConfigElement> IEnumerable<IBasicConfigElement>.GetEnumerator()
        {
            var enumerator = base.GetEnumerator();
            while (enumerator.MoveNext())
            {
                yield return (BasicConfigElement) enumerator.Current;
            }
        }
    }
}