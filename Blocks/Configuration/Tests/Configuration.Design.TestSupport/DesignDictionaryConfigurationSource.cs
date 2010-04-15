//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.TestSupport
{
    public class DesignDictionaryConfigurationSource : DictionaryConfigurationSource, IDesignConfigurationSource
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
}
