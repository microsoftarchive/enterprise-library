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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.TestSupport;
using Console.Wpf.Tests.VSTS.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Microsoft.Practices.Unity;

namespace Console.Wpf.Tests.VSTS.BlockSpecific.Caching.given_caching_configuraton
{
    public abstract class given_caching_configuration : ContainerContext
    {
        protected override void Arrange()
        {
            base.Arrange();

            var source = new DesignDictionaryConfigurationSource();
            new TestConfigurationBuilder().AddCachingSettings().Build(source);

            var sourceModel = this.Container.Resolve<ConfigurationSourceModel>();
            sourceModel.Load(source);

            CacheSectionViewModel =
                sourceModel.Sections.Where(x => x.ConfigurationType == typeof(CacheManagerSettings)).Single();

            CacheManager = CacheSectionViewModel.GetDescendentsOfType<CacheManagerData>().FirstOrDefault();
        }

        protected ElementViewModel CacheManager { get; private set; }
        protected SectionViewModel CacheSectionViewModel { get; private set; }
    }
}
