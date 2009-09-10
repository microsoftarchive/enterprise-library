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
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration
{
    public class ConfigurationSourceUpdatable : IConfigurationSource
    {
        Dictionary<string, ConfigurationSection> configurationsections = new Dictionary<string, ConfigurationSection>();
        #region IConfigurationSource Members

        public ConfigurationSection GetSection(string sectionName)
        {
            ConfigurationSection section;
            configurationsections.TryGetValue(sectionName, out section);
            return section;
        }

        public void Add(string sectionName, ConfigurationSection configurationSection)
        {
            configurationsections[sectionName] = configurationSection;
        }

        public void Remove(string sectionName)
        {
            configurationsections[sectionName] = null;
        }

        public void DoSourceChanged(IEnumerable<string> sectionNames)
        {
            if (SourceChanged != null)
            {
                SourceChanged(this, new ConfigurationSourceChangedEventArgs(this, sectionNames));
            }
        }

        public event EventHandler<ConfigurationSourceChangedEventArgs> SourceChanged;

        public void AddSectionChangeHandler(string sectionName, ConfigurationChangedEventHandler handler)
        {

        }

        public void RemoveSectionChangeHandler(string sectionName, ConfigurationChangedEventHandler handler)
        {

        }

        #endregion

        void IDisposable.Dispose()
        { }
    }
}
