//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Linq.Expressions;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Moq;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests.Configuration.CachingSettingsScenarios.given_settings_with_multiple_cache_data
{
    public abstract class Context : ArrangeActAssert
    {
        protected CachingSettings settings;
        protected TypeRegistration expectedRegistration1, expectedRegistration2, expectedRegistration3;

        protected override void Arrange()
        {
            base.Arrange();

            this.expectedRegistration1 =
                new TypeRegistration<ObjectCache>((Expression<Func<ObjectCache>>)(() => new InMemoryCache(null, 0, 0, null, null))) { Name = "cache1" };
            this.expectedRegistration2 =
                new TypeRegistration<ObjectCache>((Expression<Func<ObjectCache>>)(() => new InMemoryCache(null, 0, 0, null, null))) { Name = "cache2" };
            this.expectedRegistration3 =
                new TypeRegistration<ObjectCache>((Expression<Func<ObjectCache>>)(() => new InMemoryCache(null, 0, 0, null, null))) { Name = "cache3" };

            var data1Mock = new Mock<CacheData>();
            data1Mock.Setup(d => d.GetRegistrations(It.IsAny<IConfigurationSource>())).Returns(new[] { this.expectedRegistration1 });
            data1Mock.Setup(d => d.Name).Returns("cache1");
            var data2Mock = new Mock<CacheData>();
            data2Mock.Setup(d => d.GetRegistrations(It.IsAny<IConfigurationSource>())).Returns(new[] { this.expectedRegistration2 });
            data2Mock.Setup(d => d.Name).Returns("cache2");
            var data3Mock = new Mock<CacheData>();
            data3Mock.Setup(d => d.GetRegistrations(It.IsAny<IConfigurationSource>())).Returns(new[] { this.expectedRegistration3 });
            data3Mock.Setup(d => d.Name).Returns("cache3");

            this.settings =
                new CachingSettings
                {
                    Caches = { data1Mock.Object, data2Mock.Object, data3Mock.Object },
                    DefaultCache = "cache2"
                };
        }
    }
}
