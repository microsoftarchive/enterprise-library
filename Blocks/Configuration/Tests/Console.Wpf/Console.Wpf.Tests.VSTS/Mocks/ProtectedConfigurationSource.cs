using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Console.Wpf.Tests.VSTS.Mocks
{

    public class DesignDictionaryConfigurationSource: DictionaryConfigurationSource, IDesignConfigurationSource
    {
        public System.Configuration.ConfigurationSection GetLocalSection(string sectionName)
        {
            return GetSection(sectionName);
        }

        public void AddLocalSection(string sectionName, System.Configuration.ConfigurationSection section)
        {
            Add(sectionName, section);
        }

        public void RemoveLocalSection(string sectionName)
        {
            Remove(sectionName);
        }

        public virtual void Add(string sectionName, System.Configuration.ConfigurationSection configurationSection, string protectionProviderName)
        {
            Add(sectionName, configurationSection);
        }
    }

    class ProtectedConfigurationSource : DesignDictionaryConfigurationSource
    {
        public string protectionProviderNameOnLastCall;
        public int ProtectedAddCallCount;

        public override void Add(string sectionName, System.Configuration.ConfigurationSection configurationSection, string protectionProviderName)
        {
            ProtectedAddCallCount++;
            protectionProviderNameOnLastCall = protectionProviderName;   
        }

    }
}
