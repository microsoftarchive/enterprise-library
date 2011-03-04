using System;
using System.Linq.Expressions;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Moq;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests.Configuration.CachingSettingsScenarios.given_settings_with_single_cache_data
{
    public abstract class Context : ArrangeActAssert
    {
        protected CachingSettings settings;
        protected TypeRegistration expectedRegistration;
        protected IConfigurationSource actualConfigurationSource;

        protected override void Arrange()
        {
            base.Arrange();

            this.expectedRegistration = new TypeRegistration<ObjectCache>((Expression<Func<ObjectCache>>)(() => new InMemoryCache(null, 0, 0, null, null))) { Name = "cache" };

            var dataMock = new Mock<CacheData>();
            dataMock
                .Setup(d => d.GetRegistrations(It.IsAny<IConfigurationSource>()))
                .Callback<IConfigurationSource>(cs =>
                {
                    this.actualConfigurationSource = cs;
                })
                .Returns(new[] { this.expectedRegistration });

            this.settings =
                new CachingSettings
                {
                    Caches = { dataMock.Object }
                };
        }
    }
}
