using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    public class DesignConfigurationSource : FileConfigurationSource, IDesignConfigurationSource
    {
        public DesignConfigurationSource(string configurationFilePath)
            :base(configurationFilePath)
        {
        }

        public System.Configuration.ConfigurationSection GetLocalSection(string sectionName)
        {
            return DoGetSection(sectionName);
        }

        public void AddLocalSection(string sectionName, System.Configuration.ConfigurationSection section)
        {
            DoAdd(sectionName, section);
        }

        public void RemoveLocalSection(string sectionName)
        {
            DoRemove(sectionName);
        }
    }
}
