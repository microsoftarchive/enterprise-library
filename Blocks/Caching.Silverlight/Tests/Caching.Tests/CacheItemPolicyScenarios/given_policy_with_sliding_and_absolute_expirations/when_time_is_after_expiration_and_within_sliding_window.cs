using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests.CacheItemPolicyScenarios.given_policy_with_sliding_and_absolute_expirations
{
    [TestClass]
    public class when_time_is_after_expiration_and_within_sliding_window : Context
    {
        private DateTimeOffset lastAccessTime;

        protected override void Act()
        {
            base.Act();

            var currentTime = AbsoluteItemExpiration + TimeSpan.FromMinutes(1);
            CachingTimeProvider.SetTimeProviderForTests(() => currentTime);
            lastAccessTime = currentTime - (SlidingItemExpiration - TimeSpan.FromMinutes(2));
        }

        [TestMethod]
        public void then_item_is_expired()
        {
            Assert.IsTrue(CacheItemPolicy.IsExpired(lastAccessTime));
        }
    }
}
