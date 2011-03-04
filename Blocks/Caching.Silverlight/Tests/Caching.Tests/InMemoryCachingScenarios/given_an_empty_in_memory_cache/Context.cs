using System;
using Microsoft.Practices.EnterpriseLibrary.Caching.Scheduling;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Moq;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests.InMemoryCachingScenarios.given_an_empty_in_memory_cache
{
    public abstract class Context : ArrangeActAssert
    {
        protected InMemoryCache Cache;
        protected const string CacheName = "sample cache name";
        protected int NumberOfScavengesScheduled = 0;

        protected const int ItemsBeforeScavenging = 5;
        protected const int ItemsAfterScavenging = 1;

        protected Mock<IRecurringScheduledWork> ExpirationMock;

        private Action scavengingAction;
        
        protected override void Arrange()
        {
            base.Arrange();

            var schedulerMock = new Mock<IManuallyScheduledWork>();
            schedulerMock.Setup(s => s.SetAction(It.IsAny<Action>()))
                .Callback((Action sa) => scavengingAction = sa);
            schedulerMock.Setup(s => s.ScheduleWork())
                .Callback(() =>
                {
                    ++NumberOfScavengesScheduled;
                    scavengingAction();
                });

            ExpirationMock = new Mock<IRecurringScheduledWork>();
            ExpirationMock.Setup(e => e.SetAction(It.IsAny<Action>()))
                .Verifiable();
            ExpirationMock.Setup(e => e.Start())
                .Verifiable();

            Cache = new InMemoryCache(CacheName, ItemsBeforeScavenging, ItemsAfterScavenging,
                schedulerMock.Object, ExpirationMock.Object);
        }

        protected override void Teardown()
        {
            CachingTimeProvider.ResetTimeProvider();
            NumberOfScavengesScheduled = 0;
            base.Teardown();
        }
    }
}
