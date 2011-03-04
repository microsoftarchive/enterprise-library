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

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests.CacheItemPolicyScenarios.given_policy_with_nonremovable_priority_and_expiration
{
    [TestClass]
    public class when_expiring : Context
    {
        protected override void Act()
        {
            base.Act();
            CachingTimeProvider.SetTimeProviderForTests(() => ExpirationTime + TimeSpan.FromHours(2));
        }

        [TestMethod]
        public void then_item_expires()
        {
            Assert.IsTrue(Policy.IsExpired(ExpirationTime + TimeSpan.FromMinutes(2)));
        }
    }
}
