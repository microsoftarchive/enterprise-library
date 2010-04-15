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
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.HostAdapter;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Hosting;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Console.Wpf.Tests.VSTS.TestSupport;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;

namespace Console.Wpf.Tests.VSTS.DevTests.given_host_adapter
{
    public abstract class given_host_adapter : ContainerContext
    {
        protected ISingleHierarchyConfigurationUIHostAdapter HostAdapter;
        protected SectionViewModel CachingViewModel;
        protected ElementViewModel CacheManager;

        protected override void Arrange()
        {
            base.Arrange();

            HostAdapter = new SingleHierarchyConfigurationUIHostAdapter(new HostAdapterConfiguration(AppDomain.CurrentDomain.BaseDirectory), null);
            ConfigurationSourceModel sourceModel = (ConfigurationSourceModel)HostAdapter.GetService(typeof(ConfigurationSourceModel));
            
            sourceModel.AddSection(CacheManagerSettings.SectionName, new CacheManagerSettings {DefaultCacheManager = "Cache Manager",  CacheManagers = { { new CacheManagerData { Name = "Cache Manager" } } }, BackingStores = {{ new CustomCacheStorageData("name", "custom store type") }} });

            CachingViewModel = sourceModel.Sections.Single();
            CacheManager = CachingViewModel.GetDescendentsOfType<CacheManagerData>().First();
        }
    }
}
