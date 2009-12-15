using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Console.Wpf.Tests.VSTS.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Console.Wpf.Tests.VSTS.Contexts
{
    public abstract class CachingConfigurationContext : ContainerContext
    {
        private DictionaryConfigurationSource configSource;

        protected const string CacheManagerName = TestConfigurationBuilder.CacheManagerName;

        protected CacheManagerSettings CachingSettings
        {
            get
            {
                return (CacheManagerSettings)configSource.GetSection(BlockSectionNames.Caching);
            }
        }

        protected override void Arrange()
        {
            base.Arrange();

            var builder = new TestConfigurationBuilder();
            configSource = new DictionaryConfigurationSource();
            builder.AddCachingSettings()
                .Build(configSource);
        }
    }

}
