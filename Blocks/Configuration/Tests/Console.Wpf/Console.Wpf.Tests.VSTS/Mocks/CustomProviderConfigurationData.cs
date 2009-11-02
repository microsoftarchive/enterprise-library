using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Collections.Specialized;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Console.Wpf.Tests.VSTS.Mocks
{
    public class ConfigurationSectionWithCustomProvider : ConfigurationSection
    {
        public ConfigurationSectionWithCustomProvider()
        {
            Custom = new CustomProviderConfigurationElement();
        }

        [ConfigurationProperty("Custom")]
        public CustomProviderConfigurationElement Custom
        {
            get { return (CustomProviderConfigurationElement)base["Custom"]; }
            set { base["Custom"] = value; }
        }
    }

    public class CustomProviderConfigurationElement : ConfigurationElement, ICustomProviderData
    {
        NameValueCollection attributes = new NameValueCollection();

        [ConfigurationProperty("Name")]
        public string Name
        {
            get { return (string)base["Name"]; }
            set { base["Name"] = value; }
        }

        public System.Collections.Specialized.NameValueCollection Attributes
        {
            get { return attributes; }
        }
    }
}
