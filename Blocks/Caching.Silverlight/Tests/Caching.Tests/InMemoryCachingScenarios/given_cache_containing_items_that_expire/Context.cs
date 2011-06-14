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
using Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.Scheduling;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using Moq;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests.InMemoryCachingScenarios.
    given_cache_containing_items_that_expire
{
    public abstract class Context : ArrangeActAssert
    {
        protected InMemoryCache Cache;
        protected DateTimeOffset NowForTest = new DateTimeOffset(2011, 1, 1, 11, 15, 00, TimeSpan.Zero);
        protected const string UnexpiredKey = "item that stays";
        protected const string ExpiredKey = "item that expires";
        protected const string ExpiredKeyWithCallback = "item that expires with callback";
        protected Action DoExpirations;
        protected Mock<IRecurringWorkScheduler> ExpirationMock;
        protected CacheEntryRemovedCallback RemovedCallback;

        protected override void Arrange()
        {
            base.Arrange();

            var schedulerMock = new Mock<IManuallyScheduledWork>();

            ExpirationMock = new Mock<IRecurringWorkScheduler>();
            ExpirationMock.Setup(e => e.SetAction(It.IsAny<Action>()))
                .Callback((Action a) => DoExpirations = a)
                .Verifiable("Expiration action was not set");
            ExpirationMock.Setup(e => e.Start())
                .Verifiable("Expiration timer was not started");

            Cache = new InMemoryCache("test cacheData", 100, 50, schedulerMock.Object, ExpirationMock.Object)
            {
                { UnexpiredKey, "value that stays", NowForTest + TimeSpan.FromHours(2) },
                { ExpiredKey, "value that expires", NowForTest - TimeSpan.FromHours(2) },
                { ExpiredKeyWithCallback, "value", new CacheItemPolicy { RemovedCallback = OnRemovedCallback, AbsoluteExpiration = NowForTest - TimeSpan.FromHours(2) } }
            };

            CachingTimeProvider.SetTimeProviderForTests(() => NowForTest);
        }

        private void OnRemovedCallback(CacheEntryRemovedArguments arguments)
        {
            if (this.RemovedCallback != null) this.RemovedCallback(arguments);
        }

        protected override void Teardown()
        {
            CachingTimeProvider.ResetTimeProvider();
            base.Teardown();
        }
    }
}
