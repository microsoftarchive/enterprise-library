using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    public interface IDesignConfigurationSource : IProtectedConfigurationSource
    {
        ConfigurationSection GetLocalSection(string sectionName);

        void AddLocalSection(string sectionName, ConfigurationSection section);

        void RemoveLocalSection(string sectionName);
    }
}
