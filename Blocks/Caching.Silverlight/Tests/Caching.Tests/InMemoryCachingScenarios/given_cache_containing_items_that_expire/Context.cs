using System;
using Microsoft.Practices.EnterpriseLibrary.Caching.Scheduling;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Moq;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests.InMemoryCachingScenarios.
    given_cache_containing_items_that_expire
{
    public abstract class Context : ArrangeActAssert
    {
        protected InMemoryCache Cache;
        protected DateTimeOffset NowForTest = new DateTimeOffset(2011, 1, 1, 11, 15, 00, TimeSpan.Zero);
        protected const string UnexpiredKey = "item that stays";
        protected Action DoExpirations;
        protected Mock<IRecurringScheduledWork> ExpirationMock;

        protected override void Arrange()
        {
            base.Arrange();

            var schedulerMock = new Mock<IManuallyScheduledWork>();

            ExpirationMock = new Mock<IRecurringScheduledWork>();
            ExpirationMock.Setup(e => e.SetAction(It.IsAny<Action>()))
                .Callback((Action a) => DoExpirations = a)
                .Verifiable("Expiration action was not set");
            ExpirationMock.Setup(e => e.Start())
                .Verifiable("Expiration timer was not started");

            Cache = new InMemoryCache("test cache", 100, 50, schedulerMock.Object,
                ExpirationMock.Object)
            {
                {UnexpiredKey, "value that stays", NowForTest + TimeSpan.FromHours(2)},
                {"Item that expires", "value that expires", NowForTest - TimeSpan.FromHours(2)}
            };

            CachingTimeProvider.SetTimeProviderForTests(() => NowForTest);
        }

        protected override void Teardown()
        {
            CachingTimeProvider.ResetTimeProvider();
            base.Teardown();
        }
    }
}