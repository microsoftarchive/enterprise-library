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

using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Commands;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
{

#pragma warning disable 1591
    /// <summary>
    /// This class supports block-specific configuration design-time and is not
    /// intended to be used directly from your code.
    /// </summary>
    public class AddCachingBlockCommand : AddApplicationBlockCommand
    {
        public AddCachingBlockCommand(ConfigurationSourceModel configurationSourceModel, AddApplicationBlockCommandAttribute attribute, IUIServiceWpf uiService)
            : base(configurationSourceModel, attribute, uiService)
        {
        }

        protected override System.Configuration.ConfigurationSection CreateConfigurationSection()
        {
            return new CacheManagerSettings
            {
                DefaultCacheManager = Resources.AddCachingBlockCommandDefaultCacheManagerName,
                CacheManagers = 
                {{
                     new CacheManagerData
                     {
                         Name = Resources.AddCachingBlockCommandDefaultCacheManagerName,
                         CacheStorage = CacheManagerSectionViewModel.DefaultNullBackingStore
                     }
                }},

            };
        }
    }

#pragma warning restore 1591
}
